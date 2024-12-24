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

namespace MachineDeptApp.Inventory.Inprocess
{
    public partial class InprocessUpdateForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SqlCommand cmd;
        DataTable dtInventory;
        DataTable dtColor;
        string SubLoc;
        string CountMethod;
        string CountMethod2;
        string ItemCode;
        string QtyDetails;
        string QtyDetails2;

        //POS-Connector
        double ValueBeforeUpdate;

        //WireTerminal
        double NetWSelected;
        double MOQSelected;
        double BobbinQtyInputted;

        string ErrorText;


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
            this.CboBobbinWWireTerminal.SelectedIndexChanged += CboBobbinWWireTerminal_SelectedIndexChanged;
            this.txtQtyWireTerminal.KeyPress += TxtQtyWireTerminal_KeyPress;
            this.txtQtyWireTerminal.KeyDown += TxtQtyWireTerminal_KeyDown;
            this.txtQtyWireTerminal.TextChanged += TxtQtyWireTerminal_TextChanged;

            //Semi
            this.LbWireTubeSemi.TextChanged += LbWireTubeSemi_TextChanged;
            this.txtBatchQtySemi.KeyPress += TxtBatchQtySemi_KeyPress;
            this.txtBatchQtySemi.KeyDown += TxtBatchQtySemi_KeyDown;
            this.txtQtyPerBatchSemi.KeyPress += TxtQtyPerBatchSemi_KeyPress;
            this.txtQtyPerBatchSemi.KeyDown += TxtQtyPerBatchSemi_KeyDown;
            this.txtQtyReaySemi.KeyPress += TxtQtyReaySemi_KeyPress;
            this.txtQtyReaySemi.KeyDown += TxtQtyReaySemi_KeyDown;


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


        //WireTerminal
        private void CboBobbinWWireTerminal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtQtyWireTerminal.Text.Trim() != "")
            {
                string QtyDetails = txtQtyWireTerminal.Text;
                //split betwen + Symbol.
                string[] QtyArray = QtyDetails.Split('+');

                //If Count By Reil
                if (LbBobbinsWTitleWireTerminal.Text.ToString().Contains("R2") == true && LbNetWTitleWireTerminal.Text.ToString().Contains("R1") == true)
                {
                    //Calc Total Qty
                    double TotalQty = 0;
                    for (int i = 0; i < QtyArray.Length; i++)
                    {
                        if (QtyArray[i].ToString().Trim() != "")
                        {
                            double R2 = Convert.ToDouble(CboBobbinWWireTerminal.Text);
                            double R1 = NetWSelected;
                            double MOQ = MOQSelected;
                            double InputedR3 = Convert.ToDouble(QtyArray[i].ToString());
                            TotalQty = TotalQty + Math.Round(MOQ * (InputedR3 * InputedR3 - R2 * R2) / (R1 * R1 - R2 * R2), 2);
                            TotalQty = Convert.ToDouble(TotalQty.ToString("N0"));
                        }
                    }

                    LbTotalQtyWireTerminal.Text = TotalQty.ToString("N0");

                }
                //If Count By Bobbins
                else
                {
                    //Calc Total Qty
                    double TotalQty = 0;
                    for (int i = 0; i < QtyArray.Length; i++)
                    {
                        if (QtyArray[i].ToString().Trim() != "")
                        {
                            double W = Convert.ToDouble(QtyArray[i].ToString());
                            double BobbinW = Convert.ToDouble(CboBobbinWWireTerminal.Text);
                            TotalQty = TotalQty + Math.Round((W - BobbinW) / NetWSelected * MOQSelected, 2);
                            TotalQty = Convert.ToDouble(TotalQty.ToString("N0"));
                        }
                    }

                    LbTotalQtyWireTerminal.Text = TotalQty.ToString("N0");

                }
            }
        }
        private void TxtQtyWireTerminal_TextChanged(object sender, EventArgs e)
        {
            if (txtQtyWireTerminal.Text.Trim() != "")
            {
                if (RdbWeight.Checked == true)
                {
                    if (CboBobbinWWireTerminal.Text.Trim() != "")
                    {
                        if (txtQtyWireTerminal.ToString().Contains("*") == false)
                        {
                            string QtyDetails = txtQtyWireTerminal.Text;
                            //split betwen + Symbol.
                            string[] QtyArray = QtyDetails.Split('+');
                            BobbinQtyInputted = QtyArray.Length;

                            try
                            {
                                //If Count By Reil
                                if (LbBobbinsWTitleWireTerminal.Text.ToString().Contains("R2") == true && LbNetWTitleWireTerminal.Text.ToString().Contains("R1") == true)
                                {
                                    //Calc Total Qty
                                    double TotalQty = 0;
                                    for (int i = 0; i < QtyArray.Length; i++)
                                    {
                                        if (QtyArray[i].ToString().Trim() != "")
                                        {
                                            double R2 = Convert.ToDouble(CboBobbinWWireTerminal.Text);
                                            double R1 = NetWSelected;
                                            double MOQ = MOQSelected;
                                            double InputedR3 = Convert.ToDouble(QtyArray[i].ToString());
                                            TotalQty = TotalQty + Math.Round(MOQ * (InputedR3 * InputedR3 - R2 * R2) / (R1 * R1 - R2 * R2), 2);
                                            TotalQty = Convert.ToDouble(TotalQty.ToString("N0"));
                                        }
                                    }

                                    LbTotalQtyWireTerminal.Text = TotalQty.ToString("N0");

                                    //Assign Bobbin/Reel
                                    if (BobbinQtyInputted > 1)
                                    {
                                        LbBobbinQtyWireTerminal.Text = "( " + BobbinQtyInputted + " Reels )";
                                    }
                                    else
                                    {
                                        LbBobbinQtyWireTerminal.Text = "( " + BobbinQtyInputted + " Reel )";
                                    }
                                }
                                //If Count By Bobbins
                                else
                                {
                                    //Calc Total Qty
                                    double TotalQty = 0;
                                    for (int i = 0; i < QtyArray.Length; i++)
                                    {
                                        if (QtyArray[i].ToString().Trim() != "")
                                        {
                                            double W = Convert.ToDouble(QtyArray[i].ToString());
                                            double BobbinW = Convert.ToDouble(CboBobbinWWireTerminal.Text);
                                            TotalQty = TotalQty + Math.Round((W - BobbinW) / NetWSelected * MOQSelected, 2);
                                            TotalQty = Convert.ToDouble(TotalQty.ToString("N0"));
                                        }
                                    }

                                    LbTotalQtyWireTerminal.Text = TotalQty.ToString("N0");

                                    //Assign Bobbin/Reel
                                    if (BobbinQtyInputted > 1)
                                    {
                                        LbBobbinQtyWireTerminal.Text = "( " + BobbinQtyInputted + " Bobbins )";
                                    }
                                    else
                                    {
                                        LbBobbinQtyWireTerminal.Text = "( " + BobbinQtyInputted + " Bobbin )";
                                    }
                                }
                            }
                            catch
                            {
                                LbTotalQtyWireTerminal.Text = "";
                                LbBobbinQtyWireTerminal.Text = "";
                                MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                        else
                        {
                            LbTotalQtyWireTerminal.Text = "";
                            LbBobbinQtyWireTerminal.Text = "";
                            MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        MessageBox.Show("សូមជ្រើសរើស <ទម្ងន់ប៊ូប៊ីន (KG)> ជាមុនសិន!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    string QtyDetails = txtQtyWireTerminal.Text;
                    //split betwen + Symbol.
                    string[] QtyArray = QtyDetails.Split('+');
                    BobbinQtyInputted = 0;
                    for (int i = 0; i < QtyArray.Length; i++)
                    {
                        if (QtyArray[i].Trim() != "")
                        {
                            double CheckBobbinQty = 1;
                            if (QtyArray[i].ToString().Contains("*") == true)
                            {
                                string[] CheckBobbinQtyArray = QtyArray[i].ToString().Split('*');
                                if (CheckBobbinQtyArray[1].ToString().Trim() != "")
                                {
                                    CheckBobbinQty = Convert.ToDouble(CheckBobbinQtyArray[1].ToString());
                                }
                            }
                            BobbinQtyInputted = BobbinQtyInputted + CheckBobbinQty;
                        }
                    }

                    try
                    {
                        //Calc Total Qty
                        double TotalQty = 0;
                        int ErrorCalc = 0;
                        for (int i = 0; i < QtyArray.Length; i++)
                        {
                            double CheckQty = 0;
                            if (QtyArray[i].ToString().Trim() != "")
                            {
                                if (QtyArray[i].ToString().Contains("*") == true)
                                {
                                    string[] CheckQtyArray = QtyArray[i].Split('*');
                                    if (CheckQtyArray.Length < 3)
                                    {
                                        if (CheckQtyArray[1].ToString().Trim() != "")
                                        {
                                            CheckQty = CheckQty + (Convert.ToDouble(CheckQtyArray[0].ToString()) * Convert.ToDouble(CheckQtyArray[1].ToString()));
                                        }
                                    }
                                    else
                                    {
                                        LbTotalQtyWireTerminal.Text = "";
                                        LbBobbinQtyWireTerminal.Text = "";
                                        MessageBox.Show("អ្នកមិនអាចគុណតគ្នាបែបនេះបានទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        ErrorCalc++;
                                        break;
                                    }
                                }
                                else
                                {
                                    CheckQty = CheckQty + Convert.ToDouble(QtyArray[i].ToString());
                                }
                            }
                            TotalQty = TotalQty + CheckQty;
                        }

                        if (ErrorCalc == 0)
                        {
                            LbTotalQtyWireTerminal.Text = TotalQty.ToString("N0");

                            //Assign Bobbin/Reel
                            if (LbBobbinsWTitleWireTerminal.Text.ToString().Contains("R1") == true && LbNetWTitleWireTerminal.Text.ToString().Contains("R2") == true)
                            {
                                if (BobbinQtyInputted > 1)
                                {
                                    LbBobbinQtyWireTerminal.Text = "( " + BobbinQtyInputted + " Reels )";
                                }
                                else
                                {
                                    LbBobbinQtyWireTerminal.Text = "( " + BobbinQtyInputted + " Reel )";
                                }
                            }
                            else
                            {
                                if (BobbinQtyInputted > 1)
                                {
                                    LbBobbinQtyWireTerminal.Text = "( " + BobbinQtyInputted + " Bobbins )";
                                }
                                else
                                {
                                    LbBobbinQtyWireTerminal.Text = "( " + BobbinQtyInputted + " Bobbin )";
                                }
                            }
                        }
                    }
                    catch
                    {
                        LbTotalQtyWireTerminal.Text = "";
                        LbBobbinQtyWireTerminal.Text = "";
                        MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                LbTotalQtyWireTerminal.Text = "";
                LbBobbinQtyWireTerminal.Text = "";
            }
        }
        private void TxtQtyWireTerminal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave.PerformClick();
            }
        }
        private void TxtQtyWireTerminal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RdbWeight.Checked == true)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '+') && (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '+') && (e.KeyChar != '*'))
                {
                    e.Handled = true;
                }
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
                    try
                    {
                        cnn.con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT SubLoc, CountingMethod, CountingMethod2, ItemType, ItemCode, Qty, QtyDetails, QtyDetails2 FROM tbInventory " +
                            " WHERE LocCode='MC1' AND CancelStatus=0 AND LabelNo="+Convert.ToInt32(txtBarcode.Text) + " ORDER BY SeqNo ASC", cnn.con);
                        sda.Fill(dtInventory);

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
                        SubLoc = "";
                        CountMethod = "";
                        CountMethod2 = "";
                        ItemCode = "";
                        QtyDetails = "";
                        QtyDetails2 = "";
                        ClearAllText();

                        if (dtInventory.Rows.Count > 0)
                        {
                            SubLoc = dtInventory.Rows[0]["SubLoc"].ToString();
                            CountMethod = dtInventory.Rows[0]["CountingMethod"].ToString();
                            CountMethod2 = dtInventory.Rows[0]["CountingMethod2"].ToString();
                            ItemCode = dtInventory.Rows[0]["ItemCode"].ToString();
                            QtyDetails = dtInventory.Rows[0]["QtyDetails"].ToString();
                            QtyDetails2 = dtInventory.Rows[0]["QtyDetails2"].ToString();

                            string CountTypeInKhmer = "";
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
                            LbCountMethod.Text = " ( " + CountTypeInKhmer + " )";
                            CboSubLoc.Text = SubLoc;
                            LbLabelNo.Text = txtBarcode.Text;
                            txtBarcode.Text = "";
                            ShowingAfterScan();
                        }
                        else
                        {
                            MessageBox.Show("គ្មានទិន្នន័យឡាប៊ែលនេះទេ!","Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                            txtBarcode.Focus();
                            txtBarcode.SelectAll();
                        }
                    }
                    else
                    {
                        MessageBox.Show(ErrorText, "Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
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

            string[] MCName = new string[] { "MS01", "DCAM", "DE SK", "JAM", "TWIST", "SEMI", "SEMI MQC" };
            for (int i = 0; i < MCName.Length; i++)
            {
                CboSubLoc.Items.Add(MCName[i].ToString());
            }

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

        }


        //Function
        private void ClearAllText()
        {
            LbLabelNo.Text = "";
            LbLabelNoTitle.Visible = false;
            CboSubLoc.Text = "";
            CboSubLoc.Visible = false;
            CboSubLocTitle.Visible = false;
            LbCountMethod.Text = "";
            panelSemi.Visible = false;
            panelPOS.Visible = false;
            panelStockCardWireTerminal.Visible = false;

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
            LbCodeWireTerminal.Text = "";
            LbItemNameWireTerminal.Text = "";
            LbMakerWireTerminal.Text = "";
            LbTypeWireTerminal.Text = "";
            CboBobbinWWireTerminal.Text = "";
            LbNetWWireTerminal.Text = "";
            txtQtyWireTerminal.Text = "";

        }
        private void ShowingAfterScan()
        {
            LbLabelNo.Visible = true;
            LbLabelNoTitle.Visible = true;
            CboSubLocTitle.Visible = true;
            CboSubLoc.Visible = true;
            LbCountMethod.Visible=true;

            //POS-Connector
            if (CountMethod == "POS")
            {
                ErrorText = "";
                DataTable dt = new DataTable();
                DataTable dtRMOBS = new DataTable();
                //Find POS Detail from OBS
                try
                {
                    //POS Details
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT T1.ItemCode, ItemName, PlanQty, POSDeliveryDate FROM " +
                        "(SELECT * FROM prgproductionorder) T1 " +
                        "INNER JOIN " +
                        "(SELECT * FROM mstitem WHERE ItemType = 1 AND DelFlag=0) T2 " +
                        "ON T1.ItemCode=T2.ItemCode " +
                        "WHERE DONo='"+QtyDetails+"'", cnnOBS.conOBS);
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
                        LbPOSNoPOS.Text = QtyDetails;
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
                    MessageBox.Show(ErrorText,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            //WireTerminal
            else if (CountMethod == "StockCard")
            {
                ErrorText = "";
                DataTable dtRMOBS = new DataTable();
                //Find RM from OBS
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, Resv1 FROM mstitem WHERE ItemType = 2 AND DelFlag = 0 AND ItemCode = '" + ItemCode + "'", cnnOBS.conOBS);
                    sda.Fill(dtRMOBS);
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
                cnnOBS.conOBS.Close();

                if (ErrorText.Trim() == "")
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        cnn.con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbSDMstUncountMat WHERE Code='"+ItemCode+"'", cnn.con);
                        sda.Fill(dt);
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

                    if (ErrorText.Trim() == "")
                    {
                        //Show info
                        string ItemName = "";
                        string Maker = "";
                        if (dtRMOBS.Rows.Count > 0)
                        {
                            ItemName = dtRMOBS.Rows[0]["ItemName"].ToString();
                            Maker = dtRMOBS.Rows[0]["Resv1"].ToString();
                        }
                        LbCodeWireTerminal.Text = ItemCode;
                        LbItemNameWireTerminal.Text = ItemName;
                        LbMakerWireTerminal.Text= Maker;
                        LbTypeWireTerminal.Text = dt.Rows[0]["RMType"].ToString();
                        CboBobbinWWireTerminal.Items.Clear();
                        //Box Or Weigth
                        if (CountMethod2 == "Weight")
                        {
                            RdbWeight.Checked = true;
                            string [] QtyD2 = QtyDetails2.Split('|');
                            CboBobbinWWireTerminal.Items.Add(QtyD2[0].ToString());
                            CboBobbinWWireTerminal.SelectedIndex = 0;

                            if (dt.Rows[0]["BobbinsOrReil"].ToString() == "Bobbins")
                            {
                                LbBobbinsWTitleWireTerminal.Text = "ទម្ងន់ប៊ូប៊ីន (KG)";
                                LbNetWTitleWireTerminal.Text = "ទម្ងន់សាច់សុទ្ធ (KG)";
                                LbNetWTitleWireTerminal.Font = new Font("Khmer OS Battambang", 14);
                                LbQtyTitleWireTerminal.Text = "ទម្ងន់សរុប";
                                
                            }
                            else
                            {
                                LbBobbinsWTitleWireTerminal.Text = "ប្រវែងគ្មានធើមីណល R2 (cm)";
                                LbBobbinsWTitleWireTerminal.Font = new Font("Khmer OS Battambang", 11, FontStyle.Regular);
                                LbNetWTitleWireTerminal.Text = "ប្រវែងពេញ R1 (cm)";
                                LbQtyTitleWireTerminal.Text = "ប្រវែងសរុប";
                                //CboBobbinWWireTerminal.Items.Add(dt.Rows[0]["R2OrBobbinsW"].ToString());
                                //CboBobbinWWireTerminal.Text = dt.Rows[0]["R2OrBobbinsW"].ToString();

                            }
                            NetWSelected = Convert.ToDouble(dt.Rows[0]["R1OrNetW"].ToString());
                            MOQSelected = Convert.ToDouble(dt.Rows[0]["MOQ"].ToString());
                            LbNetWWireTerminal.Text = NetWSelected.ToString("N2") + " = " + MOQSelected.ToString("N0");
                        }
                        else
                        {
                            RdbBox.Checked = true; 
                            CboBobbinWWireTerminal.Items.Clear();
                            CboBobbinWWireTerminal.Items.Add(dt.Rows[0]["R2OrBobbinsW"].ToString());
                            CboBobbinWWireTerminal.Text = dt.Rows[0]["R2OrBobbinsW"].ToString();
                            if (dt.Rows[0]["BobbinsOrReil"].ToString() == "Bobbins")
                            {
                                LbBobbinsWTitleWireTerminal.Text = "ទម្ងន់ប៊ូប៊ីន (KG)";
                                LbNetWTitleWireTerminal.Text = "ទម្ងន់សាច់សុទ្ធ (KG)";
                                LbNetWTitleWireTerminal.Font = new Font("Khmer OS Battambang", 14);
                                LbQtyTitleWireTerminal.Text = "សម្រាយចំនួន";

                            }
                            else
                            {
                                LbBobbinsWTitleWireTerminal.Text = "ប្រវែងគ្មានធើមីណល R2 (cm)";
                                LbBobbinsWTitleWireTerminal.Font = new Font("Khmer OS Battambang", 11, FontStyle.Regular);
                                LbNetWTitleWireTerminal.Text = "ប្រវែងពេញ R1 (cm)";
                                LbQtyTitleWireTerminal.Text = "សម្រាយចំនួន";
                            }
                            NetWSelected = Convert.ToDouble(dt.Rows[0]["R1OrNetW"].ToString());
                            MOQSelected = Convert.ToDouble(dt.Rows[0]["MOQ"].ToString());
                            LbNetWWireTerminal.Text = NetWSelected.ToString("N2") + " = " + MOQSelected.ToString("N0");
                        }
                        txtQtyWireTerminal.Text = QtyDetails.Replace("x","*");
                        panelStockCardWireTerminal.Visible = true;
                        panelStockCardWireTerminal.BringToFront();
                        txtQtyWireTerminal.Focus();
                    }
                    else
                    {
                        MessageBox.Show(ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //Semi
            else
            {
                ErrorText = "";
                LbWIPCodeSemi.Text = ItemCode;
                string[] QtyD = QtyDetails.Split('x');
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
                DataTable dtSemiOBS = new DataTable();
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, Remark2,Remark3,Remark4 FROM mstitem WHERE ItemType = 1 AND DelFlag = 0 AND ItemCode='"+ItemCode+"' ", cnnOBS.conOBS);
                    sda.Fill(dtSemiOBS);
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
                cnnOBS.conOBS.Close();

                if (ErrorText.Trim() == "")
                {
                    //Show POS Details
                    if (dtSemiOBS.Rows.Count > 0)
                    {
                        LbWIPNameSemi.Text = dtSemiOBS.Rows[0]["ItemName"].ToString();
                        LbLengthSemi.Text = dtSemiOBS.Rows[0]["Remark4"].ToString();
                        LbPINSemi.Text = dtSemiOBS.Rows[0]["Remark2"].ToString();
                        LbWireTubeSemi.Text = dtSemiOBS.Rows[0]["Remark3"].ToString();
                    }
                    panelSemi.Visible = true;
                    panelSemi.BringToFront();
                    txtBatchQtySemi.Focus();
                }
                else
                {
                    MessageBox.Show(ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        private void UpdatePOSConnector()
        {
            if (LbLabelNo.Text.Trim()!="" && LbItemNamePOS.Text.Trim() != "" && dgvRMListPOS.Rows.Count > 0)
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់អាប់ដេតមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;

                    //Update
                    string Username = MenuFormV2.UserForNextForm;
                    DateTime UpdateDate = DateTime.Now;
                    string NewSubLoc = CboSubLoc.Text;

                    foreach (DataGridViewRow DgvRow in dgvRMListPOS.Rows)
                    {
                        string RMCode = DgvRow.Cells[0].Value.ToString();
                        int Qty = Convert.ToInt32(DgvRow.Cells[2].Value.ToString());
                        try
                        {
                            cnn.con.Open();
                            string query = "UPDATE tbInventory SET SubLoc='" + NewSubLoc + "', " +
                                                    "Qty=" + Qty + ", " +
                                                    "UpdateDate='" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                    "UpdateBy=N'" + Username + "' " +
                                                    "WHERE LocCode='MC1' AND LabelNo=" + Convert.ToInt32(LbLabelNo.Text) + " AND ItemCode='"+RMCode+"' ";
                            cmd = new SqlCommand(query, cnn.con);
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
                            wsBarcode.Cells[2, 4] = "Inprocess(" + NewSubLoc + ")";
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
                        MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearAllText();
                        txtBarcode.Focus();
                    }
                    else
                    {
                        MessageBox.Show("រក្សាទុកមានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("សូមស្កេនបាកូដជាមុនសិន!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                        DialogResult DSL = MessageBox.Show("តើអ្នកចង់អាប់ដេតមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (DSL == DialogResult.Yes)
                        {
                            LbStatus.Text = "កំពុងអាប់ដេត . . .";
                            LbStatus.Refresh();
                            Cursor = Cursors.WaitCursor;

                            ErrorText = "";
                            string NewSubLoc = CboSubLoc.Text;
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
                                cmd = new SqlCommand(query, cnn.con);
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

                                        worksheet.Cells[2, 6] = "Inprocess(" + NewSubLoc + ")";
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
                                MessageBox.Show("អាប់ដេតរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ClearAllText();
                                txtBarcode.Focus();
                            }
                            else
                            {
                                LbStatus.Text = "អាប់ដេតមានបញ្ហា!";
                                LbStatus.Refresh();
                                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("សូមបញ្ចូលត្រង់ សម្រាយចំនួន និងចំនួនសរុប! (ប្រអប់ដែលមានពណ៌)","Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    txtBatchQtySemi.Focus();
                }

            }
            else
            {
                MessageBox.Show("សូមស្កេនបាកូដជាមុនសិន!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBarcode.Focus();
            }
        }
        private void UpdateWireTerminal()
        {
            if (LbCodeWireTerminal.Text.Trim() != "" && LbItemNameWireTerminal.Text.Trim() != "")
            {
                if (LbTotalQtyWireTerminal.Text.Trim() != "" && LbBobbinQtyWireTerminal.Text.Trim() != "")
                {
                    //Check QtyDetail first
                    if (RdbWeight.Checked == true)
                    {
                        //Only 0-9 & '.' & '+'
                        int FoundError = 0;
                        string[] AllowText = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ".", "+" };
                        for (int i = 0; i < txtQtyWireTerminal.Text.ToString().Length; i++)
                        {
                            int OK = 0;
                            for (int j = 0; j < AllowText.Length; j++)
                            {
                                if (txtQtyWireTerminal.Text[i].ToString() == AllowText[j].ToString())
                                {
                                    OK = 1;
                                    break;
                                }
                            }
                            if (OK == 0)
                            {
                                FoundError++;
                            }
                        }

                        //Check each Qty less than BobbinW/R1 or not
                        string[] QtyArray = txtQtyWireTerminal.Text.ToString().Split('+');
                        for (int i = 0; i < QtyArray.Length; i++)
                        {
                            if (Convert.ToDouble(QtyArray[i].ToString()) < Convert.ToDouble(CboBobbinWWireTerminal.Text))
                            {
                                FoundError++;
                                MessageBox.Show("ក្នុង <ទម្ងន់សរុប> ខ្លះមានទម្ងន់តិចជាង <ទម្ងន់ប៊ូប៊ីន> !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                break;
                            }
                        }

                        //Sum/CalcByComputer
                        try
                        {
                            string value = new DataTable().Compute(txtQtyWireTerminal.Text.ToString(), null).ToString();
                            double Total = Convert.ToDouble(value);
                        }
                        catch
                        {
                            FoundError++;
                        }

                        if (FoundError == 0)
                        {
                            DialogResult DSL = MessageBox.Show("តើអ្នកចង់អាប់ដេតមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (DSL == DialogResult.Yes)
                            {
                                LbStatus.Text = "កំពុងអាប់ដេត . . .";
                                LbStatus.Refresh();
                                Cursor = Cursors.WaitCursor;

                                ErrorText = "";
                                string NewSubLoc = CboSubLoc.Text;
                                string RMCode = LbCodeWireTerminal.Text;
                                int Qty = Convert.ToInt32(LbTotalQtyWireTerminal.Text.Replace(",", ""));
                                string QtyDetail = txtQtyWireTerminal.Text;
                                string QtyDetail2 = CboBobbinWWireTerminal.Text + "|" + BobbinQtyInputted.ToString();

                                try
                                {
                                    cnn.con.Open();
                                    string Username = MenuFormV2.UserForNextForm;
                                    DateTime UpdateDate = DateTime.Now;

                                    string query = "UPDATE tbInventory SET SubLoc='" + NewSubLoc + "', " +
                                                            "Qty=" + Qty + ", " +
                                                            "QtyDetails='" + QtyDetail + "', " +
                                                            "QtyDetails2='" + QtyDetail2 + "', " +
                                                            "UpdateDate='" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                            "UpdateBy=N'" + Username + "' " +
                                                            "WHERE LocCode='MC1' AND LabelNo=" + Convert.ToInt32(LbLabelNo.Text) + " AND ItemCode='" + RMCode + "' ";
                                    cmd = new SqlCommand(query, cnn.con);
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
                                if (chkPrintStatus.Checked == true)
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
                                            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Weight"];
                                            worksheet.Cells[2, 6] = "Inprocess(" + NewSubLoc + ")";
                                            worksheet.Cells[2, 4] = LbLabelNo.Text;
                                            worksheet.Cells[3, 1] = "*" + LbLabelNo.Text + "*";
                                            worksheet.Cells[7, 4] = LbCodeWireTerminal.Text;
                                            worksheet.Cells[9, 4] = LbItemNameWireTerminal.Text;
                                            worksheet.Cells[11, 4] = LbTypeWireTerminal.Text;
                                            worksheet.Cells[13, 4] = LbMakerWireTerminal.Text;
                                            worksheet.Cells[15, 4] = LbTotalQtyWireTerminal.Text;
                                            //Summary                                            
                                            worksheet.Cells[15, 6] = "( " + txtQtyWireTerminal.Text.Replace("*", "x") + " )\n" + LbBobbinQtyWireTerminal.Text.Replace("( ","").Replace(" )","") ;

                                            // Saving the modified Excel file
                                            var CDirectory2 = Environment.CurrentDirectory;
                                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                            string file = LbLabelNo.Text;
                                            string fName = file + "-Update( " + date + " )";

                                            //លុប Manual worksheet
                                            excelApp.DisplayAlerts = false;
                                            Excel.Worksheet wsDelete = (Excel.Worksheet)xlWorkBook.Sheets["POS"];
                                            wsDelete.Delete();
                                            Excel.Worksheet wsDelete1 = (Excel.Worksheet)xlWorkBook.Sheets["Box"];
                                            wsDelete1.Delete();
                                            Excel.Worksheet wsDelete2 = (Excel.Worksheet)xlWorkBook.Sheets["Semi"];
                                            wsDelete2.Delete();
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
                                    MessageBox.Show("អាប់ដេតរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    ClearAllText();
                                    txtBarcode.Focus();
                                }
                                else
                                {
                                    LbStatus.Text = "អាប់ដេតមានបញ្ហា!";
                                    LbStatus.Refresh();
                                    MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txtQtyWireTerminal.Focus();
                        }
                    }
                    else
                    {
                        //Only 0-9 & '*' & '+'
                        int FoundError = 0;
                        string[] AllowText = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "*", "+" };
                        for (int i = 0; i < txtQtyWireTerminal.Text.ToString().Length; i++)
                        {
                            int OK = 0;
                            for (int j = 0; j < AllowText.Length; j++)
                            {
                                if (txtQtyWireTerminal.Text[i].ToString() == AllowText[j].ToString())
                                {
                                    OK = 1;
                                    break;
                                }
                            }
                            if (OK == 0)
                            {
                                FoundError++;
                            }
                        }

                        //Sum/CalcByComputer
                        try
                        {
                            string value = new DataTable().Compute(txtQtyWireTerminal.Text.ToString(), null).ToString();
                            double Total = Convert.ToDouble(value);
                        }
                        catch
                        {
                            FoundError++;
                        }

                        if (FoundError == 0)
                        {
                            DialogResult DSL = MessageBox.Show("តើអ្នកចង់អាប់ដេតមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (DSL == DialogResult.Yes)
                            {
                                LbStatus.Text = "កំពុងអាប់ដេត . . .";
                                LbStatus.Refresh();
                                Cursor = Cursors.WaitCursor;

                                ErrorText = "";
                                string NewSubLoc = CboSubLoc.Text;
                                string RMCode = LbCodeWireTerminal.Text;
                                int Qty = Convert.ToInt32(LbTotalQtyWireTerminal.Text.Replace(",", ""));
                                string QtyDetail = txtQtyWireTerminal.Text;
                                string QtyDetail2 = BobbinQtyInputted.ToString();

                                try
                                {
                                    cnn.con.Open();
                                    string Username = MenuFormV2.UserForNextForm;
                                    DateTime UpdateDate = DateTime.Now;

                                    string query = "UPDATE tbInventory SET SubLoc='" + NewSubLoc + "', " +
                                                            "Qty=" + Qty + ", " +
                                                            "QtyDetails='" + QtyDetail + "', " +
                                                            "QtyDetails2='" + QtyDetail2 + "', " +
                                                            "UpdateDate='" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                            "UpdateBy=N'" + Username + "' " +
                                                            "WHERE LocCode='MC1' AND LabelNo=" + Convert.ToInt32(LbLabelNo.Text) + " AND ItemCode='" + RMCode + "' ";
                                    cmd = new SqlCommand(query, cnn.con);
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
                                if (chkPrintStatus.Checked == true)
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
                                            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Box"];
                                            worksheet.Cells[2, 6] = "Inprocess(" + NewSubLoc + ")";
                                            worksheet.Cells[2, 4] = LbLabelNo.Text;
                                            worksheet.Cells[3, 1] = "*" + LbLabelNo.Text + "*";
                                            worksheet.Cells[7, 4] = LbCodeWireTerminal.Text;
                                            worksheet.Cells[9, 4] = LbItemNameWireTerminal.Text;
                                            worksheet.Cells[11, 4] = LbTypeWireTerminal.Text;
                                            worksheet.Cells[13, 4] = LbMakerWireTerminal.Text;
                                            worksheet.Cells[15, 4] = LbTotalQtyWireTerminal.Text;
                                            //Summary                                            
                                            worksheet.Cells[15, 6] = "( " + txtQtyWireTerminal.Text.Replace("*", "x") + " )\n" + LbBobbinQtyWireTerminal.Text.Replace("( ", "").Replace(" )", "");

                                            // Saving the modified Excel file
                                            var CDirectory2 = Environment.CurrentDirectory;
                                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                            string file = LbLabelNo.Text;
                                            string fName = file + "-Update( " + date + " )";


                                            //លុប Manual worksheet
                                            excelApp.DisplayAlerts = false;
                                            Excel.Worksheet wsDelete = (Excel.Worksheet)xlWorkBook.Sheets["POS"];
                                            wsDelete.Delete();
                                            Excel.Worksheet wsDelete1 = (Excel.Worksheet)xlWorkBook.Sheets["Weight"];
                                            wsDelete1.Delete();
                                            Excel.Worksheet wsDelete2 = (Excel.Worksheet)xlWorkBook.Sheets["Semi"];
                                            wsDelete2.Delete();
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
                                    MessageBox.Show("អាប់ដេតរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    ClearAllText();
                                    txtBarcode.Focus();
                                }
                                else
                                {
                                    LbStatus.Text = "អាប់ដេតមានបញ្ហា!";
                                    LbStatus.Refresh();
                                    MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txtQtyWireTerminal.Focus();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("សូមបញ្ចូលចំនួន!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtQtyWireTerminal.Focus();
                }
            }
            else
            {
                MessageBox.Show("សូមស្កេនបាកូដជាមុនសិន!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBarcode.Focus();
            }
        }

    }
}
