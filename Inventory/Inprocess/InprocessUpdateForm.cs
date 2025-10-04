using MachineDeptApp.AllSection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using MachineDeptApp.MsgClass;
using System.Windows.Controls.Primitives;
using System.Runtime.CompilerServices;

namespace MachineDeptApp.Inventory.Inprocess
{
    public partial class InprocessUpdateForm : Form
    {
        WarningMsgClass WMsg = new WarningMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        ErrorMsgClass EMsg = new ErrorMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        DataTable dtInventory;
        DataTable dtInventoryDetails;
        DataTable dtColor;
        string CountMethod;
        string CountTypeInKhmer;

        //POS-Connector
        double ValueBeforeUpdate;

        //WireTerminal
        double NetWSelected;
        double MOQSelected;
        double BobbinQtyInputted;

        string ErrorText;

        //Rachhan Branch testing2222



        public InprocessUpdateForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += InprocessUpdateForm_Load;
            this.FormClosed += InprocessUpdateForm_FormClosed;
            this.btnNew.Click += BtnNew_Click;
            this.btnSave.Click += BtnSave_Click;
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;


            //POS-Connector
            this.dgvRMListPOS.CellFormatting += DgvRMListPOS_CellFormatting;
            this.dgvRMListPOS.CellBeginEdit += DgvRMListPOS_CellBeginEdit;
            this.dgvRMListPOS.CellEndEdit += DgvRMListPOS_CellEndEdit;


            //WireTerminal
            //Wire
            this.dgvListSDWire.CellClick += DgvListSDWire_CellClick;
            this.btnSDWireOK.EnabledChanged += BtnSDWireOK_EnabledChanged;
            this.txtSDWireWA.KeyPress += TxtSDWireWA_KeyPress;
            this.txtSDWireWA.TextChanged += TxtSDWireWA_TextChanged;
            this.txtSDWireWA.Leave += TxtSDWireWA_Leave;
            this.btnSDWireOK.Click += BtnSDWireOK_Click;
            //Terminal
            this.dgvListSDTerminal.CellClick += DgvListSDTerminal_CellClick;
            this.btnSDTermOK.EnabledChanged += BtnSDTerminalOK_EnabledChanged;
            this.txtSDTermUseQty.KeyPress += TxtSDTermUseQty_KeyPress;
            this.txtSDTermUseQty.Leave += TxtSDTermUseQty_Leave;
            this.btnSDTermOK.Click += BtnSDTermOK_Click;


            //Semi
            this.LbWireTubeSemi.TextChanged += LbWireTubeSemi_TextChanged;
            this.txtBatchQtySemi.KeyPress += TxtBatchQtySemi_KeyPress;
            this.txtBatchQtySemi.KeyDown += TxtBatchQtySemi_KeyDown;
            this.txtQtyPerBatchSemi.KeyPress += TxtQtyPerBatchSemi_KeyPress;
            this.txtQtyPerBatchSemi.KeyDown += TxtQtyPerBatchSemi_KeyDown;
            this.txtQtyReaySemi.KeyPress += TxtQtyReaySemi_KeyPress;
            this.txtQtyReaySemi.KeyDown += TxtQtyReaySemi_KeyDown;


        }

        //WireTerminal
        private void BtnSDTermOK_Click(object sender, EventArgs e)
        {
            if (txtSDTermUseQty.Text.Trim() != "" && Convert.ToDouble(txtSDTermUseQty.Text) >= 0 && Convert.ToDouble(txtSDTermQtyB.Text) >= Convert.ToDouble(txtSDTermUseQty.Text))
            {
                foreach (DataGridViewRow row in dgvListSDTerminal.Rows)
                {
                    if (row.Cells["BobbinCodeT"].Value.ToString() == LbSDTermBobbinNo.Text)
                    {
                        row.Cells["BeforeQtyT"].Value = Convert.ToDouble(txtSDTermQtyB.Text);
                        row.Cells["UsingQtyT"].Value = Convert.ToDouble(txtSDTermUseQty.Text);
                        row.Cells["RemainQtyT"].Value = Convert.ToDouble(txtSDTermQtyB.Text) - Convert.ToDouble(txtSDTermUseQty.Text);
                        LbSDTermBobbinNo.Text = "";
                        txtSDTermQtyB.Text = "";
                        txtSDTermUseQty.Text = "";
                        btnSDTermOK.Enabled = false;
                        dgvListSDTerminal.Focus();
                        break;
                    }
                }
            }
            else
            {
                WMsg.WarningText = "សូមបញ្ចូលប្រអប់ដែលមានពណ៌នៅពីក្រោយជាមុនសិន!";
                WMsg.ShowingMsg();
                txtSDTermUseQty.Focus();
            }

        }
        private void TxtSDTermUseQty_Leave(object sender, EventArgs e)
        {
            if (txtSDTermUseQty.Text.Trim() != "")
            {
                try
                {
                    double InputtedValue = Convert.ToDouble(txtSDTermUseQty.Text);
                    if (InputtedValue >= 0)
                    {
                        if (InputtedValue <= Convert.ToDouble(txtSDTermQtyB.Text))
                            txtSDTermUseQty.Text = InputtedValue.ToString("N0");
                        else
                        {
                            txtSDTermUseQty.Text = "";
                        }
                    }
                    else
                    {
                        WMsg.WarningText = "ចំនួនប្រើប្រាស់មិនអាចតូចជាង 0 បានទេ!";
                        WMsg.ShowingMsg();
                        txtSDTermUseQty.Text = "";
                    }
                }
                catch
                {
                    WMsg.WarningText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!";
                    WMsg.ShowingMsg();
                    txtSDTermUseQty.Text = "";
                }
            }
        }
        private void TxtSDTermUseQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void BtnSDTerminalOK_EnabledChanged(object sender, EventArgs e)
        {
            if (btnSDTermOK.Enabled == true)
            {
                btnSDTermOK.ForeColor = Color.Black;
                btnSDTermOK.BackColor = Color.FromArgb(0, 192, 0);
            }
            else
            {
                btnSDTermOK.ForeColor = Color.FromArgb(64, 64, 64);
                btnSDTermOK.BackColor = Color.Silver;
            }
        }
        private void DgvListSDTerminal_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (dgvListSDTerminal.Columns[e.ColumnIndex].Name == "EdtingSDT")
                {
                    string BobbinCode = dgvListSDTerminal.Rows[e.RowIndex].Cells["BobbinCodeT"].Value.ToString();
                    double BQty = Convert.ToDouble(dgvListSDTerminal.Rows[e.RowIndex].Cells["BeforeQtyT"].Value);
                    double UsingQty = Convert.ToDouble(dgvListSDTerminal.Rows[e.RowIndex].Cells["UsingQtyT"].Value);

                    LbSDTermBobbinNo.Text = BobbinCode;
                    txtSDTermQtyB.Text = BQty.ToString("N0");
                    txtSDTermUseQty.Text = UsingQty.ToString("N0");
                    btnSDTermOK.Enabled = true;
                    txtSDTermUseQty.Focus();
                }
            }
        }

        private void TxtSDWireWA_Leave(object sender, EventArgs e)
        {
            if (txtSDWireWA.Text.Trim() != "")
            {
                try
                {
                    double InputtedValue = Convert.ToDouble(txtSDWireWA.Text);
                    txtSDWireWA.Text = InputtedValue.ToString("N2");
                }
                catch
                {
                    WMsg.WarningText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!";
                    WMsg.ShowingMsg();
                    txtSDWireWA.Text = "";
                }
            }
        }
        private void TxtSDWireWA_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the entered key is not a control key, digit or period
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // Check if the period is already present in the TextBox
            else if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
        private void TxtSDWireWA_TextChanged(object sender, EventArgs e)
        {
            if (btnSDWireOK.Enabled == true)
            {
                if (txtSDWireWA.Text.Trim() != "")
                {
                    try
                    {
                        double AfterW = Convert.ToDouble(txtSDWireWA.Text);
                        double BeforeW = Convert.ToDouble(txtSDWireWB.Text);
                        double BeforeQty = Convert.ToDouble(txtSDWireQtyB.Text);
                        double PerUnit = Convert.ToDouble(LbSDWirePerUnit.Text);

                        if (AfterW >= 0)
                        {
                            if (AfterW > 0)
                            {
                                if (AfterW <= BeforeW)
                                {
                                    double AfterQty = BeforeQty - (BeforeW - AfterW) * PerUnit * 1000;
                                    txtSDWireQtyA.Text = AfterQty.ToString("N0");
                                }
                                else
                                {
                                    txtSDWireQtyA.Text = "";
                                }
                            }
                            else
                            {
                                txtSDWireQtyA.Text = "0";
                            }
                        }
                        else
                        {
                            WMsg.WarningText = "ទម្ងន់សល់មិនអាចតូចជាង 0 បានទេ!";
                            WMsg.ShowingMsg();
                            txtSDWireWA.Text = "";
                        }
                    }
                    catch
                    {
                        EMsg.AlertText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!";
                        EMsg.ShowingMsg();
                        txtSDWireWA.Text = "";
                    }
                }
                else
                {
                    txtSDWireQtyA.Text = "";
                }
            }
        }        
        private void BtnSDWireOK_EnabledChanged(object sender, EventArgs e)
        {
            if (btnSDWireOK.Enabled == true)
            {
                btnSDWireOK.ForeColor = Color.Black;
                btnSDWireOK.BackColor = Color.FromArgb(0, 192, 0);
            }
            else
            {
                btnSDWireOK.ForeColor = Color.FromArgb(64, 64, 64);
                btnSDWireOK.BackColor = Color.Silver;
            }
        }
        private void BtnSDWireOK_Click(object sender, EventArgs e)
        {
            if (txtSDWireWA.Text.Trim() != "" && txtSDWireQtyA.Text.Trim() != "")
            {
                if (Convert.ToDouble(txtSDWireQtyA.Text) >= 0)
                {
                    foreach (DataGridViewRow row in dgvListSDWire.Rows)
                    {
                        if (row.Cells["BobbinCodeW"].Value.ToString() == LbSDWireBobbinNo.Text)
                        {
                            row.Cells["RemainWW"].Value = Convert.ToDouble(txtSDWireWA.Text);
                            row.Cells["RemainQtyW"].Value = Convert.ToDouble(txtSDWireQtyA.Text);
                            txtSDWireQtyB.Text = "";
                            txtSDWireWB.Text = "";
                            txtSDWireQtyA.Text = "";
                            txtSDWireWA.Text = "";
                            LbSDWireBobbinNo.Text = "";
                            LbSDWirePerUnit.Text = "";
                            btnSDWireOK.Enabled = false;
                            dgvListSDWire.Focus();
                            break;
                        }
                    }
                }
                else
                {
                    WMsg.WarningText = "ប្រវែងនៅសល់មិនអាចតូចជាង 0 បានទេ!";
                    WMsg.ShowingMsg();
                }
            }
            else
            {
                WMsg.WarningText = "សូមបញ្ចូលប្រអប់ដែលមានពណ៌នៅពីក្រោយជាមុនសិន!";
                WMsg.ShowingMsg();
                txtSDWireWA.Focus();
            }
        }
        private void DgvListSDWire_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (dgvListSDWire.Columns[e.ColumnIndex].Name == "EditingSDW")
                {
                    string BobbinCode = dgvListSDWire.Rows[e.RowIndex].Cells["BobbinCodeW"].Value.ToString();
                    double BQty = Convert.ToDouble(dgvListSDWire.Rows[e.RowIndex].Cells["BeforeQtySDWire"].Value);
                    double BWeight = Convert.ToDouble(dgvListSDWire.Rows[e.RowIndex].Cells["BeforeWSDWire"].Value);
                    double AQty = Convert.ToDouble(dgvListSDWire.Rows[e.RowIndex].Cells["RemainQtyW"].Value);
                    double AWeight = Convert.ToDouble(dgvListSDWire.Rows[e.RowIndex].Cells["RemainWW"].Value);
                    double PerUnit = Convert.ToDouble(dgvListSDWire.Rows[e.RowIndex].Cells["PerUnit"].Value);

                    LbSDWireBobbinNo.Text = BobbinCode;
                    txtSDWireQtyB.Text = BQty.ToString("N0");
                    txtSDWireWB.Text = BWeight.ToString("N2");
                    txtSDWireQtyA.Text = AQty.ToString("N0");
                    txtSDWireWA.Text = AWeight.ToString("N2");
                    LbSDWirePerUnit.Text = PerUnit.ToString();
                    btnSDWireOK.Enabled = true;
                    txtSDWireWA.Focus();
                }
            }
        }


        //Semi
        private void TxtQtyReaySemi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBatchQtySemi.Text.Trim() != "" && txtQtyPerBatchSemi.Text.Trim() != "")
                {
                    ErrorText = "";
                    //Calc
                    try
                    {
                        double Batch = Convert.ToDouble(Convert.ToDouble(txtBatchQtySemi.Text).ToString("N0"));
                        txtBatchQtySemi.Text = Batch.ToString();
                        double QtyPerBatch = Convert.ToDouble(Convert.ToDouble(txtQtyPerBatchSemi.Text).ToString("N0"));
                        txtQtyPerBatchSemi.Text = QtyPerBatch.ToString();
                        double Reay = 0;
                        if (txtQtyReaySemi.Text.Trim() != "")
                        {
                            Reay = Convert.ToDouble(Convert.ToDouble(txtQtyReaySemi.Text).ToString("N0"));
                            txtQtyReaySemi.Text = Reay.ToString();
                        }
                        LbTotalQtySemi.Text = ((Batch * QtyPerBatch) + Reay).ToString("N0");
                    }
                    catch (Exception ex)
                    {
                        if (ErrorText.Trim() == "")
                        {
                            ErrorText = ex.Message;
                        }
                        else
                        {
                            ErrorText = ErrorText + "\n" + ex.Message;
                        }
                    }

                    if (ErrorText.Trim() == "")
                    {
                        btnSave.PerformClick();
                    }
                    else
                    {
                        LbTotalQtySemi.Text = "";
                        MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtBatchQtySemi.Focus();
                    }
                }
                else
                {
                    LbTotalQtySemi.Text = "";
                    MessageBox.Show("សូមបញ្ចូលត្រង់ប្រអប់ដែលមានពណ៌!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtBatchQtySemi.Focus();
                }
            }
        }
        private void TxtQtyReaySemi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void TxtQtyPerBatchSemi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtQtyReaySemi.Focus();
            }
        }
        private void TxtQtyPerBatchSemi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void TxtBatchQtySemi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtQtyPerBatchSemi.Focus();
            }
        }
        private void TxtBatchQtySemi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void LbWireTubeSemi_TextChanged(object sender, EventArgs e)
        {
            if (LbWireTubeSemi.Text.Trim() != "")
            {
                foreach (DataRow row in dtColor.Rows)
                {
                    if (LbWireTubeSemi.Text.ToString().Contains(row[0].ToString()))
                    {
                        LbWireTubeSemi.ForeColor = Color.FromName(row[1].ToString());
                        LbWireTubeSemi.BackColor = Color.FromName(row[2].ToString());
                    }
                }
            }
            else
            {
                LbWireTubeSemi.ForeColor = Color.Black;
                LbWireTubeSemi.BackColor = Color.White;
            }
        }

        //POS-Connector
        private void DgvRMListPOS_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 2)
                {
                    if (dgvRMListPOS.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        try
                        {
                            double NewValue = Convert.ToDouble(dgvRMListPOS.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                            NewValue = Convert.ToDouble(NewValue.ToString("N0"));
                            if (NewValue > 0)
                            {
                                dgvRMListPOS.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = NewValue;
                            }
                            else
                            {
                                MessageBox.Show("ចំនួនត្រូវតែធំជាង ០!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                dgvRMListPOS.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ValueBeforeUpdate;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dgvRMListPOS.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ValueBeforeUpdate;
                        }
                    }
                    else
                    {
                        MessageBox.Show("មិនអាចទទេបានទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dgvRMListPOS.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ValueBeforeUpdate;
                    }
                }
            }
        }
        private void DgvRMListPOS_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 2)
                {
                    try
                    {
                        ValueBeforeUpdate = Convert.ToDouble(dgvRMListPOS.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    }
                    catch
                    {

                    }
                }
            }
        }
        private void DgvRMListPOS_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                e.CellStyle.ForeColor = Color.Black;
            }
        }


        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.Trim() != "")
                {
                    Cursor = Cursors.WaitCursor;
                    ErrorText = "";
                    dtInventory = new DataTable();
                    dtInventoryDetails = new DataTable();
                    string Barcode = txtBarcode.Text;

                    try
                    {
                        cnn.con.Open();
                        if (Barcode.Contains("/") == true)
                        {
                            //Inventory Data
                            string[] splitText = Barcode.Split('/');
                            string SQLQuery = "SELECT SubLoc, CountingMethod, CountingMethod2, tbInventory.ItemType, tbInventory.ItemCode, ItemName, RMTypeName, Qty, QtyDetails, QtyDetails2 FROM tbInventory " +
                                            "\nLEFT JOIN tbMasterItem ON tbInventory.ItemCode = tbMasterItem.ItemCode " +
                                            "\nWHERE LocCode='MC1' AND CancelStatus=0 AND LabelNo=  " + Convert.ToInt32(splitText[0]) + " AND tbInventory.ItemCode = '" + splitText[1] + "' " +
                                            "\nORDER BY SeqNo ASC";
                            //Console.WriteLine(SQLQuery);
                            SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                            sda.Fill(dtInventory);

                            //Take Bobbin Information
                            SQLQuery = "SELECT tbInventoryWandTDetails.BobbinSysNo, tbInventoryWandTDetails.RMCode, T1.BStock_Kg, BStock_QTY, tbInventoryWandTDetails.RemainQty,tbInventoryWandTDetails.RemainW, Per_Unit FROM tbInventoryWandTDetails " +
                                "\nLEFT JOIN (SELECT * FROM tbBobbinRecords WHERE In_Date IS NULL) T1 ON tbInventoryWandTDetails.BobbinSysNo = T1.BobbinSysNo AND tbInventoryWandTDetails.RMCode=T1.RMCode " +
                                "\nINNER JOIN (SELECT * FROM tbMstRMRegister WHERE Status='Active') T2 ON tbInventoryWandTDetails.BobbinSysNo=T2.BobbinSysNo " +
                                "\nWHERE LabelNo = '"+ Convert.ToInt32(splitText[0]) + "' AND tbInventoryWandTDetails.RMCode='"+ splitText[1] + "' " +
                                "\nORDER BY tbInventoryWandTDetails.BobbinSysNo ASC";
                            //Console.WriteLine(SQLQuery);
                            sda = new SqlDataAdapter(SQLQuery, cnn.con);
                            sda.Fill(dtInventoryDetails);
                        }
                        else
                        {
                            string SQLQuery = "SELECT SubLoc, CountingMethod, CountingMethod2, tbInventory.ItemType, tbInventory.ItemCode, ItemName, Qty, QtyDetails, QtyDetails2 FROM tbInventory " +
                                            "\nLEFT JOIN tbMasterItem ON tbInventory.ItemCode = tbMasterItem.ItemCode " +
                                            "\nWHERE LocCode='MC1' AND CancelStatus=0 AND CountingMethod <> 'SD Document' AND LabelNo= " + Convert.ToInt32(Barcode) + " " +
                                            "\nORDER BY SeqNo ASC";
                            SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                            sda.Fill(dtInventory);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ErrorText.Trim() == "")
                        {
                            ErrorText = ex.Message;
                        }
                        else
                        {
                            ErrorText = ErrorText +"\n"+ ex.Message; 
                        }
                    }
                    cnn.con.Close();

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        CountMethod = "";
                        ClearAllText();
                        if (dtInventory.Rows.Count > 0)
                        {
                            CountMethod = dtInventory.Rows[0]["CountingMethod"].ToString();
                            CountTypeInKhmer = "";
                            if (CountMethod == "POS")
                            {
                                CountTypeInKhmer = "ខនិកទ័រ";
                            }
                            else if (CountMethod == "Semi")
                            {
                                CountTypeInKhmer = "សឺមី";
                            }
                            else
                            {
                                CountTypeInKhmer = "ខ្សែភ្លើង/ធើមីណល";
                            }
                            LbLocation.Text = dtInventory.Rows[0]["SubLoc"].ToString() + " ( " + CountTypeInKhmer + " )";
                            if (Barcode.Contains("/") == false)
                                LbLabelNo.Text = Barcode;
                            else
                            {
                                string[] SplitText = Barcode.Split('/');
                                LbLabelNo.Text = SplitText[0].ToString();
                            }
                            txtBarcode.Text = "";
                            ShowingAfterScan();
                        }
                        else
                        {
                            WMsg.WarningText = "គ្មានទិន្នន័យឡាប៊ែលនេះទេ!";
                            WMsg.ShowingMsg();
                            txtBarcode.Focus();
                            txtBarcode.SelectAll();
                        }
                    }
                    else
                    {
                        EMsg.AlertText = ErrorText;
                        EMsg.ShowingMsg();
                    }
                }
            }
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            //POS-Connector
            if (CountMethod == "POS")
            {
                UpdatePOSConnector();
            }
            //Semi
            else if (CountMethod == "Semi")
            {
                UpdateSemi();
            }
            //Wire/Teminal
            else
            {
                UpdateWireTerminal();
            }

        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearAllText();
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }
        private void InprocessUpdateForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.MCInproPrintStatus = chkPrintStatus.Checked;
            Properties.Settings.Default.Save();
        }
        private void InprocessUpdateForm_Load(object sender, EventArgs e)
        {
            chkPrintStatus.Checked = Properties.Settings.Default.MCInproPrintStatus;

            dtColor = new DataTable();
            dtColor.Columns.Add("ShortText");
            dtColor.Columns.Add("ColorText");
            dtColor.Columns.Add("BackColor");
            dtColor.Rows.Add("RED", "White", "Red");
            dtColor.Rows.Add("BLK", "White", "Black");
            dtColor.Rows.Add("PINK", "Black", "Pink");
            dtColor.Rows.Add("YEL", "Black", "Yellow");
            dtColor.Rows.Add("BLU", "White", "Blue");
            dtColor.Rows.Add("BRN", "White", "Brown");
            dtColor.Rows.Add("G/Y", "Black", "GreenYellow");
            dtColor.Rows.Add("GRN", "White", "Green");
            dtColor.Rows.Add("GRY", "White", "Gray");
            dtColor.Rows.Add("ORG", "Black", "Orange");
            dtColor.Rows.Add("PNK", "Black", "Pink");
            dtColor.Rows.Add("SKY", "Black", "SkyBlue");
            dtColor.Rows.Add("VLT", "White", "Purple");
            dtColor.Rows.Add("WHT", "Black", "White");

            //Set POS-Connector columns to Read-Only 
            foreach (DataGridViewColumn DgvCol in dgvRMListPOS.Columns)
            {
                if (DgvCol.Index != 2)
                {
                    DgvCol.ReadOnly = true;
                }
            }

            //Not Resize column
            foreach (DataGridViewColumn DgvCol in dgvListSDWire.Columns)
            {
                if(DgvCol.Name == "EditingSDW")
                    DgvCol.Resizable = DataGridViewTriState.False;
            }
            foreach (DataGridViewColumn DgvCol in dgvListSDTerminal.Columns)
            {
                if (DgvCol.Name == "EditingSDT")
                    DgvCol.Resizable = DataGridViewTriState.False;
            }
        }


        //Function
        private void ClearAllText()
        {
            LbLabelNo.Text = "";
            LbLabelNo.Refresh();
            LbLocation.Text = "";
            LbLocation.Refresh();

            //Semi
            LbWIPCodeSemi.Text = "";
            LbWIPNameSemi.Text = "";
            LbLengthSemi.Text = "";
            LbPINSemi.Text = "";
            LbWireTubeSemi.Text = "";
            txtBatchQtySemi.Text = "";
            txtQtyPerBatchSemi.Text = "";
            txtQtyReaySemi.Text = "";
            LbTotalQtySemi.Text = "";

            //POS-Connector
            LbPOSNoPOS.Text = "";
            LbQtyPOS.Text = "";
            LbItemNamePOS.Text = "";
            LbShipmentDatePOS.Text = "";
            dgvRMListPOS.Rows.Clear();

            //WireTerminal
            //Wire
            LbSDWireRMCode.Text = "";
            LbSDWireRMName.Text = "";
            LbSDWireDocumentNo.Text = "";
            txtSDWireQtyB.Text = "";
            txtSDWireWB.Text = "";
            txtSDWireQtyA.Text = "";
            txtSDWireWA.Text = "";
            LbSDWireBobbinNo.Text = "";
            LbSDWirePerUnit.Text = "";
            btnSDWireOK.Enabled = false;
            dgvListSDWire.Rows.Clear();
            //Terminal
            LbSDTermRMCode.Text = "";
            LbSDTermRMName.Text = "";
            LbSDTermBobbinNo.Text = "";
            LbSDTermDocumentNo.Text = "";
            txtSDTermQtyB.Text="";
            txtSDTermUseQty.Text = "";
            dgvListSDTerminal.Rows.Clear();
        }
        private void ShowingAfterScan()
        {
            LbLabelNo.Visible = true;
            LbLabelNoTitle.Visible = true;
            LbSubLocTitle.Visible = true;
            LbLocation.Visible = true;

            //POS-Connector
            if (CountMethod == "POS")
            {
                ErrorText = "";
                string POSNo = dtInventory.Rows[0]["QtyDetails"].ToString();
                DataTable dt = new DataTable();
                DataTable dtRMOBS = new DataTable();
                //Find POS Detail from OBS
                try
                {
                    //POS Details
                    cnnOBS.conOBS.Open();
                    string SQLQuery = "SELECT T1.ItemCode, ItemName, PlanQty, POSDeliveryDate FROM " +
                        "\n(SELECT * FROM prgproductionorder) T1 " +
                        "\nINNER JOIN (SELECT * FROM mstitem WHERE ItemType = 1 AND DelFlag=0) T2 ON T1.ItemCode=T2.ItemCode " +
                        "\nWHERE DONo='" + POSNo + "'";
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                    sda.Fill(dt);

                    //Take RM 
                    string CodeIN = "";
                    foreach (DataRow row in dtInventory.Rows)
                    {
                        if (CodeIN.Trim() == "")
                        {
                            CodeIN = " '" + row["ItemCode"].ToString() +"' ";
                        }
                        else
                        {
                            CodeIN = CodeIN + ", '" + row["ItemCode"].ToString() + "' ";
                        }
                    }
                    SqlDataAdapter sda1 = new SqlDataAdapter("SELECT ItemCode, ItemName FROM mstitem WHERE ItemType = 2 AND DelFlag = 0 AND ItemCode IN ("+CodeIN+")", cnnOBS.conOBS);
                    sda1.Fill(dtRMOBS);
                }
                catch(Exception ex)
                {
                    if (ErrorText.Trim() == "")
                    {
                        ErrorText = ex.Message;
                    }
                    else
                    {
                        ErrorText = ErrorText + "\n" + ex.Message;
                    }
                }
                cnnOBS.conOBS.Close();

                if (ErrorText.Trim() == "")
                {
                    //Show POS Details
                    if (dt.Rows.Count > 0)
                    {
                        LbPOSNoPOS.Text = POSNo;
                        LbQtyPOS.Text = Convert.ToDouble(dt.Rows[0]["PlanQty"].ToString()).ToString("N0");
                        LbItemNamePOS.Text = dt.Rows[0]["ItemName"].ToString();
                        LbShipmentDatePOS.Text = Convert.ToDateTime(dt.Rows[0]["POSDeliveryDate"].ToString()).ToString("dd-MM-yyyy");
                    }
                    //Showing RM List
                    foreach (DataRow row in dtInventory.Rows)
                    {
                        string Code = row["ItemCode"].ToString();
                        string Name = "";
                        double Qty = Convert.ToDouble(row["Qty"].ToString());
                        foreach (DataRow rowOBS in dtRMOBS.Rows)
                        {
                            if (Code == rowOBS["ItemCode"].ToString())
                            {
                                Name = rowOBS["ItemName"].ToString();
                                break;
                            }
                        }
                        dgvRMListPOS.Rows.Add(Code, Name, Qty);
                    }
                    dgvRMListPOS.ClearSelection();
                    panelPOS.Visible=true; 
                    panelPOS.BringToFront();
                    dgvRMListPOS.Focus();
                }
                else
                {
                    EMsg.AlertText = ErrorText;
                    EMsg.ShowingMsg();
                }
            }
            //Semi
            else if (CountMethod == "Semi")
            {
                ErrorText = "";
                string [] SplitText = dtInventory.Rows[0]["QtyDetails2"].ToString().Split('|');
                string POSNo = SplitText[0].ToString();
                string WIPCode = SplitText[1].ToString();
                string BoxNo = Convert.ToDouble(SplitText[2]).ToString("N0");
                LbWIPCodeSemi.Text = WIPCode;
                string[] QtyD = dtInventory.Rows[0]["QtyDetails"].ToString().Split('x');
                txtBatchQtySemi.Text = QtyD[0].ToString();
                if (QtyD[1].ToString().Contains("+") == true)
                {
                    string[] QtyPlus = QtyD[1].Split('+');
                    txtQtyPerBatchSemi.Text = QtyPlus[0].ToString();
                    txtQtyReaySemi.Text = QtyPlus[1].ToString();
                }
                else
                {
                    txtQtyPerBatchSemi.Text = QtyD[1].ToString();
                }
                LbTotalQtySemi.Text = Convert.ToDouble(dtInventory.Rows[0]["Qty"].ToString()).ToString("N0");

                //Take Semi Detail from OBS
                DataTable dtSemiInfo = new DataTable();
                try
                {
                    cnn.con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, Remarks1, Remarks2, Remarks3 FROM tbMasterItem " +
                        "\nWHERE ItemType = 'Work In Process' AND ItemCode = '" + WIPCode + "' ", cnn.con);
                    sda.Fill(dtSemiInfo);
                }
                catch (Exception ex)
                {
                    if (ErrorText.Trim() == "")
                    {
                        ErrorText = ex.Message;
                    }
                    else
                    {
                        ErrorText = ErrorText + "\n" + ex.Message;
                    }
                }
                cnn.con.Close();

                if (ErrorText.Trim() == "")
                {
                    //Show POS Details
                    if (dtSemiInfo.Rows.Count > 0)
                    {
                        LbPOSNoSemi.Text = POSNo;
                        LbBoxNoSemi.Text = BoxNo;
                        LbWIPNameSemi.Text = dtSemiInfo.Rows[0]["ItemName"].ToString();
                        LbLengthSemi.Text = dtSemiInfo.Rows[0]["Remarks3"].ToString();
                        LbPINSemi.Text = dtSemiInfo.Rows[0]["Remarks1"].ToString();
                        LbWireTubeSemi.Text = dtSemiInfo.Rows[0]["Remarks2"].ToString();
                    }

                    panelSemi.Visible = true;
                    panelSemi.BringToFront();
                    txtBatchQtySemi.Focus();
                }
                else
                {
                    EMsg.AlertText = ErrorText;
                    EMsg.ShowingMsg();
                }
            }

            //WireTerminal
            else
            {
                string RMCode = dtInventory.Rows[0]["ItemCode"].ToString();
                string RMName = dtInventory.Rows[0]["ItemName"].ToString();
                string RMType = dtInventory.Rows[0]["RMTypeName"].ToString();
                string DocumentNo = dtInventory.Rows[0]["QtyDetails"].ToString();
                if ( RMType == "Terminal")
                {
                    tabContrlSDWire.Visible = false; 
                    LbSDTermRMCode.Text = RMCode;
                    LbSDTermRMName.Text = RMName;
                    LbSDTermDocumentNo.Text = DocumentNo;
                    btnSDTermOK.Enabled = false;

                    foreach (DataRow row in dtInventoryDetails.Rows)
                    {
                        dgvListSDTerminal.Rows.Add();
                        dgvListSDTerminal.Rows[dgvListSDTerminal.Rows.Count - 1].Cells["BobbinCodeT"].Value = row["BobbinSysNo"].ToString();
                        dgvListSDTerminal.Rows[dgvListSDTerminal.Rows.Count - 1].Cells["BeforeQtyT"].Value = Convert.ToDouble(row["BStock_QTY"]);
                        dgvListSDTerminal.Rows[dgvListSDTerminal.Rows.Count - 1].Cells["UsingQtyT"].Value = Convert.ToDouble(row["BStock_QTY"])-Convert.ToDouble(row["RemainQty"]);
                        dgvListSDTerminal.Rows[dgvListSDTerminal.Rows.Count - 1].Cells["RemainQtyT"].Value = Convert.ToDouble(row["RemainQty"]);
                    }
                }
                else
                {
                    tabContrlSDWire.Visible = true;
                    LbSDWireRMCode.Text = RMCode;
                    LbSDWireRMName.Text = RMName;
                    LbSDWireDocumentNo.Text = DocumentNo;
                    btnSDWireOK.Enabled = false;

                    foreach (DataRow row in dtInventoryDetails.Rows)
                    {
                        dgvListSDWire.Rows.Add();
                        dgvListSDWire.Rows[dgvListSDWire.Rows.Count-1].Cells["BobbinCodeW"].Value = row["BobbinSysNo"].ToString();
                        dgvListSDWire.Rows[dgvListSDWire.Rows.Count - 1].Cells["PerUnit"].Value = Convert.ToDouble(row["Per_Unit"]);
                        dgvListSDWire.Rows[dgvListSDWire.Rows.Count - 1].Cells["BeforeQtySDWire"].Value = Convert.ToDouble(row["BStock_QTY"]);
                        dgvListSDWire.Rows[dgvListSDWire.Rows.Count - 1].Cells["BeforeWSDWire"].Value = Convert.ToDouble(row["BStock_Kg"]);
                        dgvListSDWire.Rows[dgvListSDWire.Rows.Count - 1].Cells["RemainWW"].Value = Convert.ToDouble(row["RemainW"]);
                        dgvListSDWire.Rows[dgvListSDWire.Rows.Count - 1].Cells["RemainQtyW"].Value = Convert.ToDouble(row["RemainQty"]);
                    }
                }
                panelStockCardWireTerminal.Visible = true;
                panelStockCardWireTerminal.BringToFront();
            }
            
        }
        private void UpdatePOSConnector()
        {
            if (LbLabelNo.Text.Trim()!="" && LbItemNamePOS.Text.Trim() != "" && dgvRMListPOS.Rows.Count > 0)
            {
                QMsg.QAText = "តើអ្នកចង់អាប់ដេតមែនឬទេ?";
                QMsg.UserClickedYes = false;
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;

                    //Update
                    string Username = MenuFormV2.UserForNextForm;
                    DateTime UpdateDate = DateTime.Now;

                    foreach (DataGridViewRow DgvRow in dgvRMListPOS.Rows)
                    {
                        string RMCode = DgvRow.Cells[0].Value.ToString();
                        int Qty = Convert.ToInt32(DgvRow.Cells[2].Value.ToString());
                        try
                        {
                            cnn.con.Open();
                            string query = "UPDATE tbInventory SET Qty=" + Qty + ", " +
                                                    "UpdateDate='" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                    "UpdateBy=N'" + Username + "' " +
                                                    "WHERE LocCode='MC1' AND LabelNo=" + Convert.ToInt32(LbLabelNo.Text) + " AND ItemCode='"+RMCode+"' ";
                            SqlCommand cmd = new SqlCommand(query, cnn.con);
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            if (ErrorText.Trim() == "")
                            {
                                ErrorText = "Update to DB : " + ex.Message;
                            }
                            else
                            {
                                ErrorText = ErrorText + "\nUpdate to DB : " + ex.Message;
                            }
                            break;
                        }
                        cnn.con.Close();
                    }

                    //Print
                    if (chkPrintStatus.Checked == true)
                    {
                        if (ErrorText.Trim() == "")
                        {
                            //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                            string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\InventoryLable\Inprocess";
                            if (!Directory.Exists(SavePath))
                            {
                                Directory.CreateDirectory(SavePath);
                            }

                            //សរសេរចូល Excel
                            var CDirectory = Environment.CurrentDirectory;
                            Excel.Application excelApp = new Excel.Application();
                            Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\InventoryLabel_Inprocess_Template.xlsx", Editable: true);
                            Excel.Worksheet wsBarcode = (Excel.Worksheet)xlWorkBook.Sheets["POS"];
                            //ក្បាលលើ
                            wsBarcode.Cells[2, 3] = LbLabelNo.Text;
                            wsBarcode.Cells[3, 1] = "*" + LbLabelNo.Text + "*";
                            wsBarcode.Cells[2, 4] = "Inprocess(" + LbLocation.Text.ToString().Replace(" ( " + CountTypeInKhmer + " )","") + ")";
                            wsBarcode.Cells[4, 3] = LbPOSNoPOS.Text;
                            wsBarcode.Cells[5, 3] = LbItemNamePOS.Text;
                            wsBarcode.Cells[6, 3] = LbQtyPOS.Text;

                            // Material List
                            for (int i = 0; i < dgvRMListPOS.Rows.Count; i++)
                            {
                                wsBarcode.Cells[i + 10, 1] = dgvRMListPOS.Rows[i].Cells[0].Value.ToString();
                                wsBarcode.Cells[i + 10, 2] = dgvRMListPOS.Rows[i].Cells[1].Value.ToString();
                                wsBarcode.Cells[i + 10, 5] = dgvRMListPOS.Rows[i].Cells[2].Value.ToString();

                            }

                            //លុប Manual worksheet
                            excelApp.DisplayAlerts = false;
                            Excel.Worksheet wsDelete = (Excel.Worksheet)xlWorkBook.Sheets["Box"];
                            wsDelete.Delete();
                            Excel.Worksheet wsDelete1 = (Excel.Worksheet)xlWorkBook.Sheets["Weight"];
                            wsDelete1.Delete();
                            Excel.Worksheet wsDelete2 = (Excel.Worksheet)xlWorkBook.Sheets["Semi"];
                            wsDelete2.Delete();
                            excelApp.DisplayAlerts = true;

                            //ព្រីនចេញ
                            for (int i = 0; i < numPrintQty.Value; i++)
                            {
                                wsBarcode.PrintOut();
                            }

                            //Save Excel ទុក
                            string fName = "";
                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                            string file = LbLabelNo.Text;
                            wsBarcode.Name = "RachhanSystem";
                            fName = file + "-Update( " + date + " )";
                            wsBarcode.SaveAs(SavePath + @"\" + fName + ".xlsx");
                            xlWorkBook.Save();
                            xlWorkBook.Close();
                            excelApp.Quit();
                            //Kill all Excel background process
                            var processes = from p in Process.GetProcessesByName("EXCEL")
                                            select p;
                            foreach (var process in processes)
                            {
                                if (process.MainWindowTitle.ToString().Trim() == "")
                                    process.Kill();
                            }
                        }
                    }

                    Cursor = Cursors.Default;
                    if (ErrorText.Trim() == "")
                    {
                        InfoMsg.InfoText = "រក្សាទុករួចរាល់!";
                        InfoMsg.ShowingMsg();
                        ClearAllText();
                        txtBarcode.Focus();
                    }
                    else
                    {
                        EMsg.AlertText = "រក្សាទុកមានបញ្ហា!\n" + ErrorText;
                        EMsg.ShowingMsg();
                    }
                }
            }
            else
            {
                WMsg.WarningText = "សូមស្កេនបាកូដជាមុនសិន!";
                WMsg.ShowingMsg();
                txtBarcode.Focus();
            }
        }
        private void UpdateSemi()
        {
            if (LbWIPNameSemi.Text.Trim() != "")
            {
                if (txtBatchQtySemi.Text.Trim() != "" && txtQtyPerBatchSemi.Text.Trim() != "")
                {
                    ErrorText = "";

                    //Calc
                    try
                    {
                        double Batch = Convert.ToDouble(Convert.ToDouble(txtBatchQtySemi.Text).ToString("N0"));
                        txtBatchQtySemi.Text = Batch.ToString();
                        double QtyPerBatch = Convert.ToDouble(Convert.ToDouble(txtQtyPerBatchSemi.Text).ToString("N0"));
                        txtQtyPerBatchSemi.Text = QtyPerBatch.ToString();
                        double Reay = 0;
                        if (txtQtyReaySemi.Text.Trim() != "")
                        {
                            Reay = Convert.ToDouble(Convert.ToDouble(txtQtyReaySemi.Text).ToString("N0"));
                            txtQtyReaySemi.Text = Reay.ToString();
                        }
                        LbTotalQtySemi.Text = ((Batch * QtyPerBatch) + Reay).ToString("N0");
                    }
                    catch (Exception ex)
                    {
                        if (ErrorText.Trim() == "")
                        {
                            ErrorText = ex.Message;
                        }
                        else
                        {
                            ErrorText = ErrorText + "\n" + ex.Message;
                        }
                    }

                    if (ErrorText.Trim() == "")
                    {
                        QMsg.QAText = "តើអ្នកចង់អាប់ដេតមែនឬទេ?";
                        QMsg.UserClickedYes = false;
                        QMsg.ShowingMsg();
                        if (QMsg.UserClickedYes == true)
                        {
                            LbStatus.Text = "កំពុងអាប់ដេត . . .";
                            LbStatus.Refresh();
                            Cursor = Cursors.WaitCursor;

                            ErrorText = "";
                            string NewSubLoc = LbLocation.Text;
                            string WipCode = LbWIPCodeSemi.Text;
                            int Qty = Convert.ToInt32(LbTotalQtySemi.Text.Replace(",", ""));
                            string QtyDetails = txtBatchQtySemi.Text.ToString() + "x" + txtQtyPerBatchSemi.Text.ToString();
                            if (txtQtyReaySemi.Text.Trim() != "")
                            {
                                QtyDetails = QtyDetails + "+" + txtQtyReaySemi.Text.ToString();
                            }

                            try
                            {
                                cnn.con.Open();                                
                                string Username = MenuFormV2.UserForNextForm;
                                DateTime UpdateDate = DateTime.Now;

                                string query = "UPDATE tbInventory SET SubLoc='" + NewSubLoc + "', " +
                                                        "Qty=" + Qty + ", " +
                                                        "QtyDetails='" + QtyDetails + "', " +
                                                        "UpdateDate='" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                        "UpdateBy=N'" + Username + "' " +
                                                        "WHERE LocCode='MC1' AND LabelNo=" + Convert.ToInt32(LbLabelNo.Text) + " AND ItemCode='" + WipCode + "' ";
                                SqlCommand cmd = new SqlCommand(query, cnn.con);
                                cmd.ExecuteNonQuery();

                            }
                            catch (Exception ex)
                            {
                                if (ErrorText.Trim() == "")
                                {
                                    ErrorText = "Update to DB : " + ex.Message;
                                }
                                else
                                {
                                    ErrorText = ErrorText + "\nUpdate to DB : " + ex.Message;
                                }
                            }
                            cnn.con.Close();

                            //Print
                            if(chkPrintStatus.Checked ==  true)
                            {
                                if (ErrorText.Trim() == "")
                                {
                                    try
                                    {
                                        //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                                        string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\InventoryLable\Inprocess";
                                        if (!Directory.Exists(SavePath))
                                        {
                                            Directory.CreateDirectory(SavePath);
                                        }

                                        //open excel application and create new workbook
                                        var CDirectory = Environment.CurrentDirectory;
                                        Excel.Application excelApp = new Excel.Application();
                                        Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\InventoryLabel_Inprocess_Template.xlsx", Editable: true);
                                        Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Semi"];

                                        worksheet.Cells[2, 6] = "Inprocess(" + LbLocation.Text.ToString().Replace(" ( " + CountTypeInKhmer + " )", "") + ")";
                                        worksheet.Cells[2, 4] = LbLabelNo.Text;
                                        worksheet.Cells[3, 1] = "*" + LbLabelNo.Text + "*";
                                        worksheet.Cells[7, 4] = LbWIPCodeSemi.Text;
                                        worksheet.Cells[9, 4] = LbWIPNameSemi.Text;
                                        worksheet.Cells[11, 4] = LbPINSemi.Text;
                                        worksheet.Cells[13, 4] = LbWireTubeSemi.Text;
                                        worksheet.Cells[15, 4] = LbLengthSemi.Text;

                                        worksheet.Cells[17, 4] = Qty.ToString();
                                        //Summary
                                        worksheet.Cells[17, 6] = "( " + QtyDetails + " )";

                                        // Saving the modified Excel file
                                        var CDirectory2 = Environment.CurrentDirectory;
                                        string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                        string file = LbLabelNo.Text;
                                        string fName = file + "-Update( " + date + " )";

                                        //លុប Manual worksheet
                                        excelApp.DisplayAlerts = false;
                                        Excel.Worksheet wsDelete1 = (Excel.Worksheet)xlWorkBook.Sheets["Weight"];
                                        wsDelete1.Delete();
                                        Excel.Worksheet wsDelete2 = (Excel.Worksheet)xlWorkBook.Sheets["POS"];
                                        wsDelete2.Delete();
                                        Excel.Worksheet wsDelete3 = (Excel.Worksheet)xlWorkBook.Sheets["Box"];
                                        wsDelete3.Delete();
                                        excelApp.DisplayAlerts = true;

                                        worksheet.Name = "RachhanSystem";
                                        worksheet.SaveAs(SavePath + @"\" + fName + ".xlsx");
                                        for (int i = 0; i < numPrintQty.Value; i++)
                                        {
                                            worksheet.PrintOut();
                                        }
                                        xlWorkBook.Save();
                                        xlWorkBook.Close();
                                        excelApp.Quit();

                                        //Kill all Excel background process
                                        var processes = from p in Process.GetProcessesByName("EXCEL")
                                                        select p;
                                        foreach (var process in processes)
                                        {
                                            if (process.MainWindowTitle.ToString().Trim() == "")
                                                process.Kill();
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        if (ErrorText.Trim() == "")
                                        {
                                            ErrorText = "Print Label : " + ex.Message;
                                        }
                                        else
                                        {
                                            ErrorText = ErrorText + "\nPrint Label : " + ex.Message;
                                        }
                                        //Kill all Excel background process
                                        var processes = from p in Process.GetProcessesByName("EXCEL")
                                                        select p;
                                        foreach (var process in processes)
                                        {
                                            if (process.MainWindowTitle.ToString().Trim() == "")
                                                process.Kill();
                                        }
                                    }
                                }
                            }

                            Cursor = Cursors.Default;
                            if (ErrorText.Trim() == "")
                            {
                                LbStatus.Text = "អាប់ដេតរួចរាល់!";
                                LbStatus.Refresh();
                                InfoMsg.InfoText = "អាប់ដេតរួចរាល់!";
                                InfoMsg.ShowingMsg();
                                ClearAllText();
                                txtBarcode.Focus();
                            }
                            else
                            {
                                LbStatus.Text = "អាប់ដេតមានបញ្ហា!";
                                LbStatus.Refresh();
                                EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                                EMsg.ShowingMsg();
                            }
                        }
                    }
                    else
                    {
                        LbTotalQtySemi.Text = "";
                        if (ErrorText.Trim() == "")
                        {
                            ErrorText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!\n" + ErrorText;
                        }
                        else
                        {
                            ErrorText = ErrorText + "\nអ្នកបញ្ចូលខុសទម្រង់ហើយ!\n" + ErrorText;
                        }
                        txtBatchQtySemi.Focus();
                    }
                }
                else
                {
                    LbTotalQtySemi.Text = "";
                    WMsg.WarningText = "សូមបញ្ចូលត្រង់ សម្រាយចំនួន និងចំនួនសរុប! (ប្រអប់ដែលមានពណ៌)";
                    WMsg.ShowingMsg();
                    txtBatchQtySemi.Focus();
                }

            }
            else
            {
                WMsg.WarningText = "សូមស្កេនបាកូដជាមុនសិន!";
                WMsg.ShowingMsg();
                txtBarcode.Focus();
            }
        }
        private void UpdateWireTerminal()
        {
            if (dgvListSDWire.Rows.Count > 0 || dgvListSDTerminal.Rows.Count > 0)
            {
                string Username = MenuFormV2.UserForNextForm;
                DateTime UpdateDate = DateTime.Now;
                string LabelNo = LbLabelNo.Text;
                string RMCode = "";
                string RMName = "";
                string SDNo = "";

                QMsg.QAText = "តើអ្នកចង់អាប់ដេតមែនឬទេ?";
                QMsg.UserClickedYes = false;
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;
                    LbStatus.Text = "កំពុងអាប់ដេត . . .";
                    LbStatus.Refresh();

                    //Check Still not yet delete or not
                    DataTable dtChecking = new DataTable();
                    try
                    {
                        cnn.con.Open();
                        string SQLQuery = "SELECT * FROM tbInventory WHERE LocCode = 'MC1' AND CancelStatus = 0 AND CountingMethod = 'SD Document' AND LabelNo = '" + LabelNo + "' ";
                        Console.WriteLine(SQLQuery);
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        sda.Fill(dtChecking);
                    }
                    catch (Exception ex)
                    {
                        ErrorText = "Check : " + ex.Message;
                    }
                    cnn.con.Close();

                    //Update
                    int TotalQty = 0;
                    string RMType = "Wire";
                    if (ErrorText.Trim() == "")
                    {
                        if (dtChecking.Rows.Count > 0)
                        {
                            //Wire
                            if (tabContrlSDWire.Visible == true)
                            {
                                RMCode = LbSDWireRMCode.Text;
                                RMName = LbSDWireRMName.Text;
                                SDNo = LbSDWireDocumentNo.Text;
                                try
                                {
                                    cnn.con.Open();
                                    //Update tbInventoryWandTDetail
                                    string query = "";
                                    SqlCommand cmd = new SqlCommand();
                                    foreach (DataGridViewRow row in dgvListSDWire.Rows)
                                    {
                                        string BobbinCode = row.Cells["BobbinCodeW"].Value.ToString();
                                        double RemainW = Convert.ToDouble(row.Cells["RemainWW"].Value);
                                        int RemainQty = Convert.ToInt32(row.Cells["RemainQtyW"].Value);
                                        TotalQty += RemainQty;

                                        query = "UPDATE tbInventoryWandTDetails " +
                                                                "SET RemainW=" + RemainW + ", " +
                                                                "RemainQty='" + RemainQty + "' " +
                                                                "WHERE LabelNo=" + LabelNo + " AND SDNo='" + SDNo + "' AND RMCode='" + RMCode + "' AND BobbinSysNo = '" + BobbinCode + "' ";
                                        cmd = new SqlCommand(query, cnn.con);
                                        cmd.ExecuteNonQuery();
                                    }

                                    //Update tbInventory
                                    query = "UPDATE tbInventory " +
                                                                "SET Qty=" + TotalQty + ", " +
                                                                "UpdateDate = '" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                                "UpdateBy=N'" + Username + "' " +
                                                                "WHERE LocCode = 'MC1' AND CountingMethod='SD Document' AND LabelNo=" + LabelNo + " AND ItemCode='" + RMCode + "' ";
                                    cmd = new SqlCommand(query, cnn.con);
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    ErrorText = "Update : " + ex.Message;
                                }
                                cnn.con.Close();
                            }
                            //Terminal
                            else
                            {
                                RMType = "Terminal";
                                RMCode = LbSDTermRMCode.Text;
                                RMName = LbSDTermRMName.Text;
                                SDNo = LbSDTermDocumentNo.Text;
                                try
                                {
                                    cnn.con.Open();
                                    //Update tbInventoryWandTDetail
                                    string query = "";
                                    SqlCommand cmd = new SqlCommand();
                                    foreach (DataGridViewRow row in dgvListSDTerminal.Rows)
                                    {
                                        string BobbinCode = row.Cells["BobbinCodeT"].Value.ToString();
                                        int RemainQty = Convert.ToInt32(row.Cells["RemainQtyT"].Value);
                                        TotalQty += RemainQty;

                                        query = "UPDATE tbInventoryWandTDetails " +
                                                                "SET RemainQty='" + RemainQty + "' " +
                                                                "WHERE LabelNo=" + LabelNo + " AND SDNo='" + SDNo + "' AND RMCode='" + RMCode + "' AND BobbinSysNo = '" + BobbinCode + "' ";
                                        cmd = new SqlCommand(query, cnn.con);
                                        cmd.ExecuteNonQuery();
                                    }

                                    //Update tbInventory
                                    query = "UPDATE tbInventory " +
                                                                "SET Qty=" + TotalQty + ", " +
                                                                "UpdateDate = '" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                                "UpdateBy=N'" + Username + "' " +
                                                                "WHERE LocCode = 'MC1' AND CountingMethod='SD Document' AND LabelNo=" + LabelNo + " AND ItemCode='" + RMCode + "' ";
                                    cmd = new SqlCommand(query, cnn.con);
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    ErrorText = "Update : " + ex.Message;
                                }
                                cnn.con.Close();
                            }
                        }
                        else
                        {
                            ErrorText = "ទិន្នន័យត្រូវបានលុបបាត់ហើយ!";
                        }
                    }

                    LbStatus.Text = "កំពុងព្រីនឡាប៊ែល . . .";
                    LbStatus.Refresh();
                    //Print
                    if (ErrorText.Trim() == "")
                    {
                        if (chkPrintStatus.Checked == true)
                        {
                            //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                            string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\InventoryLable\Inprocess";
                            if (!Directory.Exists(SavePath))
                            {
                                Directory.CreateDirectory(SavePath);
                            }

                            //open excel application and create new workbook
                            var CDirectory = Environment.CurrentDirectory;
                            Excel.Application excelApp = new Excel.Application();
                            Excel.Workbook xlWorkBook = null;
                            Excel.Worksheet worksheet = null;
                            Excel.Worksheet DeleteWorksheet = null;

                            try
                            {
                                double TototalBobbinQty = dgvListSDWire.Rows.Count;
                                string QtyAndBobbin = TotalQty.ToString("N0") + " m (" + TototalBobbinQty.ToString("N0") + " Bobbins)";

                                xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\InventoryLabel_SD_Template.xlsx", Editable: true);
                                worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Wire"];
                                DeleteWorksheet = (Excel.Worksheet)xlWorkBook.Sheets["Terminal"];
                                if (RMType != "Wire")
                                {
                                    TototalBobbinQty = dgvListSDTerminal.Rows.Count;
                                    worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Terminal"];
                                    DeleteWorksheet = (Excel.Worksheet)xlWorkBook.Sheets["Wire"];
                                    QtyAndBobbin = TotalQty.ToString("N0") + " Pcs (" + TototalBobbinQty.ToString("N0") + " Bobbins/Reels)";
                                }

                                //Header
                                worksheet.Cells[1, 1] = UpdateDate;
                                worksheet.Cells[2, 3] = LabelNo + "/" + RMCode;
                                worksheet.Cells[4, 1] = "*" + LabelNo + "/" + RMCode + "*";
                                worksheet.Cells[5, 2] = RMCode;
                                worksheet.Cells[6, 2] = RMName;


                                worksheet.Cells[7, 2] = QtyAndBobbin;
                                //Insert if more than 1
                                if (TototalBobbinQty > 1)
                                {
                                    worksheet.Range["10:" + (TototalBobbinQty + 8)].Insert();
                                    //For Merge
                                    worksheet.Range["A9:D9"].Copy();
                                    worksheet.Range["A10:D" + (TototalBobbinQty + 8)].PasteSpecial(Excel.XlPasteType.xlPasteFormats);
                                    //Border
                                    worksheet.Range["A10:D" + (TototalBobbinQty + 8)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                                }

                                //Bobbin List 
                                if (RMType == "Wire")
                                {
                                    int WriteSeqNo = 1;
                                    int WriteIndex = 9;
                                    foreach (DataGridViewRow RowDgv in dgvListSDWire.Rows)
                                    {
                                        worksheet.Cells[WriteIndex, 1] = WriteSeqNo;
                                        worksheet.Cells[WriteIndex, 2] = RowDgv.Cells["BobbinCodeW"].Value.ToString();
                                        worksheet.Cells[WriteIndex, 3] = Convert.ToDouble(RowDgv.Cells["RemainWW"].Value);
                                        worksheet.Cells[WriteIndex, 4] = Convert.ToDouble(RowDgv.Cells["RemainQtyW"].Value);

                                        WriteSeqNo++;
                                        WriteIndex++;
                                    }
                                }
                                else
                                {
                                    int WriteSeqNo = 1;
                                    int WriteIndex = 9;
                                    foreach (DataGridViewRow RowDgv in dgvListSDTerminal.Rows)
                                    {
                                        worksheet.Cells[WriteIndex, 1] = WriteSeqNo;
                                        worksheet.Cells[WriteIndex, 2] = RowDgv.Cells["BobbinCodeT"].Value.ToString();
                                        worksheet.Cells[WriteIndex, 4] = Convert.ToDouble(RowDgv.Cells["RemainQtyT"].Value);

                                        WriteSeqNo++;
                                        WriteIndex++;
                                    }
                                }

                                // Saving the modified Excel file
                                string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                string file = LabelNo.ToString() + "_" + RMCode;
                                string fName = file + "( " + date + " )";

                                //Delete worksheet
                                excelApp.DisplayAlerts = false;
                                DeleteWorksheet.Delete();
                                excelApp.DisplayAlerts = true;

                                worksheet.Name = "RachhanSystem";
                                worksheet.SaveAs(SavePath + @"\" + fName + ".xlsx");
                                for (int j = 0; j < numPrintQty.Value; j++)
                                {
                                    worksheet.PrintOut();
                                }
                                excelApp.DisplayAlerts = false;
                                xlWorkBook.Close();
                                excelApp.DisplayAlerts = true;

                            }
                            catch (Exception ex)
                            {
                                excelApp.DisplayAlerts = false;
                                xlWorkBook.Close();
                                excelApp.DisplayAlerts = true;
                                excelApp.Quit();
                                if (ErrorText.Trim() == "")
                                {
                                    ErrorText = "Print Label : " + ex.Message;
                                }
                                else
                                {
                                    ErrorText = ErrorText + "\nPrint Label : " + ex.Message;
                                }
                            }

                            //Kill all Excel background process
                            var processes = from p in Process.GetProcessesByName("EXCEL")
                                            select p;
                            foreach (var process in processes)
                            {
                                if (process.MainWindowTitle.ToString().Trim() == "")
                                    process.Kill();
                            }
                        }
                    }

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        LbStatus.Text = "អាប់ដេតរួចរាល់!";
                        LbStatus.Refresh();
                        InfoMsg.InfoText = "អាប់ដេតរួចរាល់!";
                        InfoMsg.ShowingMsg();
                        btnNew.PerformClick();
                        LbStatus.Text = "";
                        LbStatus.Refresh();
                    }
                    else
                    {
                        LbStatus.Text = "មានបញ្ហា!";
                        LbStatus.Refresh();
                        EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                        EMsg.ShowingMsg();
                    }
                }
            }
            else
            {
                WMsg.WarningText = "សូមស្កេនបាកូដជាមុនសិន!";
                WMsg.ShowingMsg();
                txtBarcode.Focus();
            }
        }

    }
}
