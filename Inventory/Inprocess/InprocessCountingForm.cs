using System;
using MachineDeptApp.MsgClass;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using System.ComponentModel;

namespace MachineDeptApp.Inventory.Inprocess
{
    public partial class InprocessCountingForm : Form
    {
        WarningMsgClass WMsg = new WarningMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        ErrorMsgClass EMsg = new ErrorMsgClass();
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SqlCommand cmd;
        DataTable dtColor;
        public static string CountType;
        //public static int CountTypeChanged;
        string LocSelected;

        //POS-Connector
        double ValueBeforeUpdate;

        //Wire
        double PerUnit;

        string ErrorText;

        public InprocessCountingForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += InprocessCountingForm_Load;
            this.FormClosed += InprocessCountingForm_FormClosed;
            this.btnNew.Click += BtnNew_Click;
            this.btnSave.Click += BtnSave_Click;
            this.txtCountingType.TextChanged += TxtCountingType_TextChanged;
            this.txtCountingType.Click += TxtCountingType_Click;
            this.PicCurrentCountingType.Click += PicCurrentCountingType_Click;

            //WireTerminal
            this.txtBarcodeWireTerminal.KeyDown += TxtBarcodeWireTerminal_KeyDown;
            this.dgvWireTerminal_W.CellPainting += DgvWireTerminal_W_CellPainting;
            this.dgvWireTerminal_T.CellPainting += DgvWireTerminal_T_CellPainting;
            this.dgvWireTerminal_W.SortCompare += DgvWireTerminal_W_SortCompare;
            this.dgvWireTerminal_T.SortCompare += DgvWireTerminal_T_SortCompare;
            //Wire 
            this.btnOK_W.EnabledChanged += BtnOK_W_EnabledChanged;
            this.txtWAWireTerminal_W.KeyPress += TxtWAWireTerminal_W_KeyPress;
            this.txtWAWireTerminal_W.Leave += TxtWAWireTerminal_W_Leave;
            this.txtWAWireTerminal_W.TextChanged += TxtWAWireTerminal_W_TextChanged;
            this.btnOK_W.Click += BtnOK_W_Click;

            //Terminal
            this.btnOK_T.EnabledChanged += BtnOK_T_EnabledChanged;
            this.txtUseQtyWireTerminal_T.KeyPress += TxtUseQtyWireTerminal_T_KeyPress;
            this.txtUseQtyWireTerminal_T.Leave += TxtUseQtyWireTerminal_T_Leave;
            this.btnOK_T.Click += BtnOK_T_Click;

            //POS-Connector
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;
            this.dgvRMListPOS.CellFormatting += DgvRMListPOS_CellFormatting;
            this.dgvRMListPOS.CellBeginEdit += DgvRMListPOS_CellBeginEdit;
            this.dgvRMListPOS.CellEndEdit += DgvRMListPOS_CellEndEdit;

            //Semi BC
            this.LbWireTubeSemiBC.TextChanged += LbWireTubeSemiBC_TextChanged;
            this.txtBarcodeSemi.KeyDown += TxtBarcodeSemi_KeyDown;
            this.txtBatchQtyBC.KeyPress += TxtBatchQtyBC_KeyPress;
            this.txtBatchQtyBC.KeyDown += TxtBatchQtyBC_KeyDown;
            this.txtQtyPerBatchBC.KeyPress += TxtQtyPerBatchBC_KeyPress;
            this.txtQtyPerBatchBC.KeyDown += TxtQtyPerBatchBC_KeyDown;
            this.txtQtyReayBC.KeyPress += TxtQtyReayBC_KeyPress;
            this.txtQtyReayBC.KeyDown += TxtQtyReayBC_KeyDown;

            this.tabCtrlSemi.SelectedIndexChanged += TabCtrlSemi_SelectedIndexChanged;

            //Semi
            this.txtWipNameSemi.KeyDown += TxtWipNameSemi_KeyDown;
            this.dgvSemi.CellFormatting += DgvSemi_CellFormatting;
            this.txtBatchQtySemi.KeyPress += TxtBatchQtySemi_KeyPress;
            this.txtBatchQtySemi.KeyDown += TxtBatchQtySemi_KeyDown;
            this.txtQtyPerBatchSemi.KeyPress += TxtQtyPerBatchSemi_KeyPress;
            this.txtQtyPerBatchSemi.KeyDown += TxtQtyPerBatchSemi_KeyDown;
            this.txtQtyReaySemi.KeyPress += TxtQtyReaySemi_KeyPress;
            this.txtQtyReaySemi.KeyDown += TxtQtyReaySemi_KeyDown;
            this.dgvSemi.CellClick += DgvSemi_CellClick;

        }

        //WireTerminal
        private void TxtUseQtyWireTerminal_T_Leave(object sender, EventArgs e)
        {
            if (txtUseQtyWireTerminal_T.Text.Trim() != "")
            {
                try
                {
                    double InputtedValue = Convert.ToDouble(txtUseQtyWireTerminal_T.Text);
                    if (InputtedValue >= 0)
                    {
                        if (InputtedValue <= Convert.ToDouble(txtQtyBWireTerminal_T.Text))
                            txtUseQtyWireTerminal_T.Text = InputtedValue.ToString("N0");
                        else
                        {
                            txtUseQtyWireTerminal_T.Text = "";
                        }
                    }
                    else
                    {
                        WMsg.WarningText = "ចំនួនប្រើប្រាស់មិនអាចតូចជាង 0 បានទេ!";
                        WMsg.ShowingMsg();
                        txtUseQtyWireTerminal_T.Text = "";
                    }
                }
                catch
                {
                    WMsg.WarningText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!";
                    WMsg.ShowingMsg();
                    txtUseQtyWireTerminal_T.Text = "";
                }
            }
        }
        private void TxtUseQtyWireTerminal_T_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void BtnOK_T_Click(object sender, EventArgs e)
        {
            if (txtUseQtyWireTerminal_T.Text.Trim() != "" && Convert.ToDouble(txtUseQtyWireTerminal_T.Text) >= 0 && Convert.ToDouble(txtQtyBWireTerminal_T.Text) >= Convert.ToDouble(txtUseQtyWireTerminal_T.Text))
            {
                foreach (DataGridViewRow row in dgvWireTerminal_T.Rows)
                {
                    if (row.Cells["BobbinCodeT"].Value.ToString() == LbBobbinNoWireTerminal_T.Text)
                    {
                        int CurrentSeqNo = 1;
                        foreach (DataGridViewRow SeqRow in dgvWireTerminal_T.Rows)
                        {
                            if (SeqRow.Cells["ScanSeqT"].Value != null && SeqRow.Cells["ScanSeqT"].Value.ToString().Trim() != "")
                                CurrentSeqNo++;
                        }
                        row.Cells["RemainQtyT"].Value = Convert.ToDouble(txtQtyBWireTerminal_T.Text) - Convert.ToDouble(txtUseQtyWireTerminal_T.Text);
                        row.Cells["StatusT"].Value = "✔️";
                        ClearAllText();
                        btnOK_T.Enabled = false;
                        txtBarcodeWireTerminal.Focus();
                        break;
                    }
                }                
            }
            else
            {
                WMsg.WarningText = "សូមបញ្ចូលប្រអប់ដែលមានពណ៌នៅពីក្រោយជាមុនសិន!";
                WMsg.ShowingMsg();
                txtUseQtyWireTerminal_T.Focus();
            }
        }
        private void BtnOK_W_Click(object sender, EventArgs e)
        {
            if (txtWAWireTerminal_W.Text.Trim() != "" && txtQtyAWireTerminal_W.Text.Trim() != "")
            {
                if (Convert.ToDouble(txtQtyAWireTerminal_W.Text) >= 0)
                {
                    foreach (DataGridViewRow row in dgvWireTerminal_W.Rows)
                    {
                        if (row.Cells["BobbinCodeW"].Value.ToString() == LbBobbinNoWireTerminal_W.Text)
                        {
                            int CurrentSeqNo = 1;
                            foreach (DataGridViewRow SeqRow in dgvWireTerminal_W.Rows)
                            {
                                if (SeqRow.Cells["ScanSeqW"].Value != null && SeqRow.Cells["ScanSeqW"].Value.ToString().Trim() != "")
                                    CurrentSeqNo++;
                            }
                            row.Cells["ScanSeqW"].Value = CurrentSeqNo++;
                            row.Cells["RemainWW"].Value = Convert.ToDouble(txtWAWireTerminal_W.Text);
                            row.Cells["RemainQtyW"].Value = Convert.ToDouble(txtQtyAWireTerminal_W.Text);
                            row.Cells["StatusW"].Value = "✔️";
                            ClearAllText();
                            btnOK_W.Enabled = false;
                            txtBarcodeWireTerminal.Focus();
                            break;
                        }
                    }
                    //dgvWireTerminal_W.Columns["ScanSeqW"].SortMode = DataGridViewColumnSortMode.Automatic;
                    //dgvWireTerminal_W.Sort(dgvWireTerminal_W.Columns["ScanSeqW"], ListSortDirection.Ascending);
                    //dgvWireTerminal_W.Columns["ScanSeqW"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    //dgvWireTerminal_W.Columns["RMCodeW"].SortMode = DataGridViewColumnSortMode.Automatic;
                    //dgvWireTerminal_W.Sort(dgvWireTerminal_W.Columns["RMCodeW"], ListSortDirection.Ascending);
                    //dgvWireTerminal_W.Columns["RMCodeW"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    //AssignSeqNo();
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
                txtWAWireTerminal_W.Focus();
            }
        }
        private void BtnOK_T_EnabledChanged(object sender, EventArgs e)
        {
            if (btnOK_T.Enabled == true)
            {
                btnOK_T.ForeColor = Color.Black;
                btnOK_T.BackColor = Color.FromArgb(0, 192, 0);
            }
            else
            {
                btnOK_T.ForeColor = Color.FromArgb(64, 64, 64);
                btnOK_T.BackColor = Color.Silver;
            }
        }
        private void BtnOK_W_EnabledChanged(object sender, EventArgs e)
        {
            if (btnOK_W.Enabled == true)
            {
                btnOK_W.ForeColor = Color.Black;
                btnOK_W.BackColor = Color.FromArgb(0, 192, 0);
            }
            else
            {
                btnOK_W.ForeColor = Color.FromArgb(64, 64, 64);
                btnOK_W.BackColor = Color.Silver;
            }
        }
        private void TxtWAWireTerminal_W_TextChanged(object sender, EventArgs e)
        {
            if (btnOK_W.Enabled == true)
            {
                if (txtWAWireTerminal_W.Text.Trim() != "")
                {
                    try
                    {
                        double AfterW = Convert.ToDouble(txtWAWireTerminal_W.Text);
                        double BeforeW = Convert.ToDouble(txtWBWireTerminal_W.Text);
                        double BeforeQty = Convert.ToDouble(txtQtyBWireTerminal_W.Text);

                        if (AfterW >= 0)
                        {
                            if (AfterW > 0)
                            {
                                if (AfterW <= BeforeW)
                                {
                                    double AfterQty = BeforeQty - (BeforeW - AfterW) * PerUnit * 1000;
                                    txtQtyAWireTerminal_W.Text = AfterQty.ToString("N0");
                                }
                                else
                                {
                                    txtQtyAWireTerminal_W.Text = "";
                                }
                            }
                            else
                            {
                                txtQtyAWireTerminal_W.Text = "0";
                            }
                        }
                        else
                        {
                            WMsg.WarningText = "ទម្ងន់សល់មិនអាចតូចជាង 0 បានទេ!";
                            WMsg.ShowingMsg();
                            txtWAWireTerminal_W.Text = "";
                        }
                    }
                    catch
                    {
                        EMsg.AlertText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!";
                        EMsg.ShowingMsg();
                        txtWAWireTerminal_W.Text = "";
                    }
                }
                else
                {
                    txtQtyAWireTerminal_W.Text = "";
                }
            }
        }        
        private void TxtWAWireTerminal_W_Leave(object sender, EventArgs e)
        {
            if (txtWAWireTerminal_W.Text.Trim() != "")
            {
                try
                {
                    double InputtedValue = Convert.ToDouble(txtWAWireTerminal_W.Text);
                    txtWAWireTerminal_W.Text = InputtedValue.ToString("N2");
                }
                catch
                {
                    WMsg.WarningText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!";
                    WMsg.ShowingMsg();
                    txtWAWireTerminal_W.Text = "";
                }
            }
        }
        private void TxtWAWireTerminal_W_KeyPress(object sender, KeyPressEventArgs e)
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
        private void DgvWireTerminal_T_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            int num1, num2;
            bool isNum1 = int.TryParse(e.CellValue1?.ToString(), out num1);
            bool isNum2 = int.TryParse(e.CellValue2?.ToString(), out num2);

            if (isNum1 && isNum2)
            {
                e.SortResult = num1.CompareTo(num2);
            }
            else if (isNum1)
            {
                e.SortResult = -1; // Numbers come before strings
            }
            else if (isNum2)
            {
                e.SortResult = 1; // Numbers come before strings
            }
            else
            {
                e.SortResult = string.Compare(e.CellValue1?.ToString(), e.CellValue2?.ToString());
            }

            e.Handled = true;
        }
        private void DgvWireTerminal_W_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            int num1, num2;
            bool isNum1 = int.TryParse(e.CellValue1?.ToString(), out num1);
            bool isNum2 = int.TryParse(e.CellValue2?.ToString(), out num2);

            if (isNum1 && isNum2)
            {
                e.SortResult = num1.CompareTo(num2);
            }
            else if (isNum1)
            {
                e.SortResult = -1; // Numbers come before strings
            }
            else if (isNum2)
            {
                e.SortResult = 1; // Numbers come before strings
            }
            else
            {
                e.SortResult = string.Compare(e.CellValue1?.ToString(), e.CellValue2?.ToString());
            }

            e.Handled = true;
        }
        private void DgvWireTerminal_T_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvWireTerminal_T.Columns[e.ColumnIndex].Name == "StatusT")
                {
                    if (dgvWireTerminal_T.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "✖️")
                    {
                        e.CellStyle.ForeColor = Color.Red;
                        e.CellStyle.SelectionForeColor = Color.Red;
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.SelectionForeColor = Color.Green;
                    }
                }
            }
        }
        private void DgvWireTerminal_W_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvWireTerminal_W.Columns[e.ColumnIndex].Name == "StatusW")
                {
                    if (dgvWireTerminal_W.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "✖️")
                    {
                        e.CellStyle.ForeColor = Color.Red;
                        e.CellStyle.SelectionForeColor = Color.Red;
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.SelectionForeColor = Color.Green;
                    }
                }
            }
        }
        private void TxtBarcodeWireTerminal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtBarcodeWireTerminal.Text.ToString().Trim()!="")
            {
                //SD
                if (LbBarcodeTitleWireTerminal.Text.Contains("SD") == true)
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;
                    ClearAllText();
                    dgvWireTerminal_W.Rows.Clear();
                    dgvWireTerminal_T.Rows.Clear();
                    string SDNo = txtBarcodeWireTerminal.Text;
                    DataTable dtSDDetails = new DataTable();
                    DataTable dtBobbinsW = new DataTable();
                    DataTable dtBobbinsT = new DataTable();
                    try
                    {
                        cnn.con.Open();
                        string SQLQuery = "SELECT MCName FROM tbSDAllocateStock " +
                            "\nWHERE SysNo = '"+ SDNo + "' " +
                            "\nGROUP BY MCName";
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        sda.Fill(dtSDDetails);

                        SQLQuery = "SELECT T1.*, ItemName, T2.RMType FROM " +
                            "\n(SELECT * FROM tbBobbinRecords) T1 " +
                            "\nINNER JOIN (SELECT * FROM tbMstRMRegister) T2 ON T1.BobbinSysNo=T2.BobbinSysNo " +
                            "\nLEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') T3 ON T1.RMCode=T3.ItemCode " +
                            "\nWHERE In_Date IS NULL AND RMType = 'Wire' AND SD_DocNo = '"+ SDNo + "' " +
                            "\nORDER BY RMCode ASC, BobbinSysNo ASC";
                        sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        sda.Fill(dtBobbinsW);

                        SQLQuery = "SELECT T1.*, ItemName, T2.RMType FROM " +
                            "\n(SELECT * FROM tbBobbinRecords) T1 " +
                            "\nINNER JOIN (SELECT * FROM tbMstRMRegister) T2 ON T1.BobbinSysNo=T2.BobbinSysNo " +
                            "\nLEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') T3 ON T1.RMCode=T3.ItemCode " +
                            "\nWHERE In_Date IS NULL AND RMType <> 'Wire' AND SD_DocNo = '" + SDNo + "' " +
                            "\nORDER BY RMCode ASC, BobbinSysNo ASC";
                        sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        sda.Fill(dtBobbinsT);
                    }
                    catch (Exception ex)
                    {
                        ErrorText = ex.Message;
                    }
                    cnn.con.Close();

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        if (dtSDDetails.Rows.Count > 0 && dtBobbinsW.Rows.Count > 0)
                        {
                            //Header
                            LbSDNoWireTerminal.Text = SDNo;
                            LbMCNameWireTerminal.Text = dtSDDetails.Rows[0]["MCName"].ToString();

                            //Wire & Terminal List
                            foreach (DataRow row in dtBobbinsW.Rows)
                            {
                                dgvWireTerminal_W.Rows.Add();
                                dgvWireTerminal_W.Rows[dgvWireTerminal_W.Rows.Count - 1].HeaderCell.Value = dgvWireTerminal_W.Rows.Count.ToString();
                                dgvWireTerminal_W.Rows[dgvWireTerminal_W.Rows.Count - 1].Cells["RMCodeW"].Value = row["RMCode"].ToString();
                                dgvWireTerminal_W.Rows[dgvWireTerminal_W.Rows.Count - 1].Cells["RMNameW"].Value = row["ItemName"].ToString();
                                dgvWireTerminal_W.Rows[dgvWireTerminal_W.Rows.Count - 1].Cells["BobbinCodeW"].Value = row["BobbinSysNo"].ToString();
                                dgvWireTerminal_W.Rows[dgvWireTerminal_W.Rows.Count - 1].Cells["StatusW"].Value = "✖️";
                            }
                            foreach (DataRow row in dtBobbinsT.Rows)
                            {
                                dgvWireTerminal_T.Rows.Add();
                                dgvWireTerminal_T.Rows[dgvWireTerminal_T.Rows.Count - 1].HeaderCell.Value = dgvWireTerminal_T.Rows.Count.ToString();
                                dgvWireTerminal_T.Rows[dgvWireTerminal_T.Rows.Count - 1].Cells["BobbinCodeT"].Value = row["BobbinSysNo"].ToString();
                                dgvWireTerminal_T.Rows[dgvWireTerminal_T.Rows.Count - 1].Cells["RMCodeT"].Value = row["RMCode"].ToString();
                                dgvWireTerminal_T.Rows[dgvWireTerminal_T.Rows.Count - 1].Cells["RMNameT"].Value = row["ItemName"].ToString();
                                dgvWireTerminal_T.Rows[dgvWireTerminal_T.Rows.Count - 1].Cells["StatusT"].Value = "✖️";
                            }
                            dgvWireTerminal_W.ClearSelection();
                            dgvWireTerminal_T.ClearSelection();
                            LbBarcodeTitleWireTerminal.Text = "ស្កេនឡាប៊ែលនៅលើប៊ូប៊ីន";
                            txtBarcodeWireTerminal.Focus();
                            txtBarcodeWireTerminal.Text = "";
                        }
                        else
                        {
                            WMsg.WarningText = "គ្មានទិន្នន័យ SD នេះនៅ Inprocess ទៀតនុះទេ!";
                            WMsg.ShowingMsg();
                            txtBarcodeWireTerminal.Focus();
                            txtBarcodeWireTerminal.SelectAll();
                        }
                    }
                    else
                    {
                        EMsg.AlertText = "មានបញ្ហា!\n"+ErrorText;
                        EMsg.ShowingMsg();
                    }
                }
                //Bobbin
                else
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;
                    ClearAllText();
                    string BobbinNo = txtBarcodeWireTerminal.Text;

                    //Select Bobbin with PerUnit
                    DataTable dt = new DataTable();
                    try
                    {
                        cnn.con.Open();
                        string SQLQuery = "SELECT SD_DocNo, T1.BobbinSysNo, T1.RMCode, ItemName, BStock_Kg, BStock_QTY, AStock_Kg, AStock_QTY, PerUnit, RMType FROM " +
                            "\n(SELECT * FROM tbBobbinRecords) T1 " +
                            "\nINNER JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') T4 ON T1.RMCode=T4.ItemCode " +
                            "\nLEFT JOIN (SELECT BobbinSysNo, RMCode, ROUND(Per_Unit,4) AS PerUnit, RMType FROM tbMstRMRegister WHERE Status = 'Active') T2 ON T1.RMCode=T2.RMCode AND T1.BobbinSysNo=T2.BobbinSysNo " +
                            "\nWHERE In_Date IS NULL AND T1.BobbinSysNo = '" + BobbinNo+ "' AND SD_DocNo = '"+LbSDNoWireTerminal.Text+"' ";
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        sda.Fill(dt);
                    }
                    catch(Exception ex) 
                    {
                        ErrorText = ex.Message;
                    }
                    cnn.con.Close();

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        if (dt.Rows.Count > 0)
                        {
                            txtBarcodeWireTerminal.Text = "";
                            string RMType = dt.Rows[0]["RMType"].ToString();
                            string RMName = dt.Rows[0]["ItemName"].ToString();
                            double BStockW = Convert.ToDouble(dt.Rows[0]["BStock_Kg"].ToString());
                            double BStockQty = Convert.ToDouble(dt.Rows[0]["BStock_QTY"].ToString());
                            PerUnit = 0;
                            if ( RMType == "Wire")
                            {
                                PerUnit = Convert.ToDouble(dt.Rows[0]["PerUnit"].ToString());
                                tabContrlWireTerminal.SelectedIndex = 0;
                                LbBobbinNoWireTerminal_W.Text = BobbinNo;
                                LbRMNameWireTerminal_W.Text = RMName;
                                txtWBWireTerminal_W.Text = BStockW.ToString("N2");
                                txtQtyBWireTerminal_W.Text = BStockQty.ToString("N0");
                                btnOK_W.Enabled = true;
                                txtWAWireTerminal_W.Focus();
                            }
                            else
                            {
                                tabContrlWireTerminal.SelectedIndex = 1;
                                LbBobbinNoWireTerminal_T.Text = BobbinNo;
                                LbRMNameWireTerminal_T.Text = RMName;
                                txtQtyBWireTerminal_T.Text = BStockQty.ToString("N0");
                                btnOK_T.Enabled = true;
                                txtUseQtyWireTerminal_T.Focus();
                            }
                        }
                        else
                        {
                            WMsg.WarningText = "គ្មានទិន្នន័យប៊ូប៊ីននេះទេ!";
                            WMsg.ShowingMsg();
                            btnOK_W.Enabled = false;
                            btnOK_T.Enabled = false;
                            txtBarcodeWireTerminal.Focus();
                        }
                    }
                    else
                    {
                        EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                        EMsg.ShowingMsg();
                        btnOK_W.Enabled = false;
                        btnOK_T.Enabled = false;
                    }
                }
            }
        }
        
        //POS-Connector
        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.Trim() != "")
                {
                    if (txtBarcode.Text.Length < 10 || (Regex.Matches(txtBarcode.Text.ToString(), "[~!@#$%^&*()_+{}:\"<>?-]").Count) > 1 || (Regex.Matches(txtBarcode.Text.ToString(), "[-]").Count) < 1 || txtBarcode.Text.ToString().Trim()[2].ToString() != "-")
                    {
                        MessageBox.Show("លេខ​ POS ខុសទម្រង់ហើយ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.Focus();
                        txtBarcode.SelectAll();
                    }
                    else
                    {
                        dgvRMListPOS.Rows.Clear();
                        DataTable dtPOSDetails = new DataTable();
                        DataTable dtPOSConsumpt = new DataTable();
                        ClearTextPos();
                        string POS = txtBarcode.Text;
                        DataTable dtAlreadyCount = new DataTable();
                        string ErrorText = "";

                        //ឆែកមើលថាធ្លាប់បញ្ចូលឬអត់?
                        try
                        {
                            cnn.con.Open();
                            SqlDataAdapter da = new SqlDataAdapter("SELECT QtyDetails AS POSNo FROM tbInventory WHERE LocCode='MC1' AND CancelStatus = 0 AND QtyDetails ='" + POS + "'", cnn.con);
                            da.Fill(dtAlreadyCount);
                        }
                        catch (Exception ex)
                        {
                            ErrorText = "MC DB : " + ex.Message;
                        }
                        cnn.con.Close();

                        //Take POS Detail & Consumption from OBS
                        if (dtAlreadyCount.Rows.Count == 0 && ErrorText.Trim() == "")
                        {
                            //Take POS Details
                            try
                            {
                                cnn.con.Open(); 
                                string SQLQuery = "SELECT PosCNo, ItemName, PosCQty, PosPDelDate, " +
                                    "\nNULLIF(CONCAT(MC1Name, " +
                                    "\nCASE " +
                                    "\n\tWHEN LEN(MC2Name)>1 THEN ' & ' " +
                                    "\n\tELSE '' " +
                                    "\nEND, MC2Name, " +
                                    "\nCASE " +
                                    "\n\tWHEN LEN(MC3Name)>1 THEN ' & ' " +
                                    "\n\tELSE '' " +
                                    "\nEND, MC3Name),'') AS MCName FROM " +
                                    "\n(SELECT * FROM tbPOSDetailofMC) T1 " +
                                    "\nINNER JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Work In Process') T2 ON T1.WIPCode=T2.ItemCode " +
                                    "\nWHERE PosCNo='"+POS+"' ";
                                //Console.WriteLine(SQLQuery);
                                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                                sda.Fill(dtPOSDetails);
                            }
                            catch (Exception ex)
                            {
                                ErrorText = "POS Details : " + ex.Message;
                            }
                            cnn.con.Close();

                            //Consumption from OBS
                            if (ErrorText.Trim() == "")
                            {
                                //Assign POS Details
                                if (dtPOSDetails.Rows.Count > 0)
                                {
                                    try
                                    {
                                        cnnOBS.conOBS.Open();
                                        //Take POS Consumption
                                        string SQLQuery = "SELECT DONo, ConsumpSeqNo, T2.ItemCode, ItemName, ConsumpQty FROM " +
                                            "(SELECT * FROM prgproductionorder) T1 " +
                                            "INNER JOIN " +
                                            "(SELECT * FROM prgconsumtionorder) T2 " +
                                            "ON T1.ProductionCode=T2.ProductionCode " +
                                            "INNER JOIN " +
                                            "(SELECT * FROM mstitem WHERE DelFlag =0 AND ItemType=2 AND MatCalcFlag=0) T3 " +
                                            "ON T2.ItemCode=T3.ItemCode " +
                                            "WHERE DONo = '" + POS + "' " +
                                            "ORDER BY ConsumpSeqNo ASC";
                                        //Console.WriteLine(SQLQuery);
                                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                                        sda.Fill(dtPOSConsumpt);
                                    }
                                    catch (Exception ex)
                                    {
                                        if (ErrorText.Trim() == "")
                                        {
                                            ErrorText = "OBS DB : " + ex.Message;
                                        }
                                        else
                                        {
                                            ErrorText = ErrorText + "\nOBS DB : " + ex.Message;
                                        }
                                    }
                                    cnnOBS.conOBS.Close();

                                    if (dtPOSConsumpt.Rows.Count > 0)
                                    {
                                        LbPOSNoPOS.Text = dtPOSDetails.Rows[0]["PosCNo"].ToString();
                                        LbItemNamePOS.Text = dtPOSDetails.Rows[0]["ItemName"].ToString();
                                        LbQtyPOS.Text = Convert.ToDouble(dtPOSDetails.Rows[0]["PosCQty"]).ToString("N0");
                                        LbShipmentDatePOS.Text = Convert.ToDateTime(dtPOSDetails.Rows[0]["PosPDelDate"]).ToString("dd-MM-yyyy");
                                        LbMCNamePOS.Text = dtPOSDetails.Rows[0]["MCName"].ToString();

                                        //Assign POS Consumption
                                        foreach (DataRow row in dtPOSConsumpt.Rows)
                                        {
                                            string Code = row[2].ToString();
                                            string RMName = row[3].ToString();
                                            double Qty = Convert.ToDouble(row[4].ToString());

                                            dgvRMListPOS.Rows.Add(Code, RMName, Qty, Qty);
                                            dgvRMListPOS.Rows[dgvRMListPOS.Rows.Count - 1].HeaderCell.Value = dgvRMListPOS.Rows.Count.ToString();
                                        }
                                        dgvRMListPOS.ClearSelection();
                                        txtBarcode.Text = "";
                                        btnSave.Focus();
                                    }
                                    else
                                    {
                                        MessageBox.Show("POS នេះមិនប្រើប្រាស់ខនិកទ័រទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        txtBarcode.Focus();
                                        txtBarcode.SelectAll();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("គ្មាន POS នេះនៅក្នុងប្រព័ន្ធទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    txtBarcode.Focus();
                                    txtBarcode.SelectAll();
                                }
                            }
                        }
                        else if (dtAlreadyCount.Rows.Count > 0 && ErrorText.Trim() == "")
                        {

                            MessageBox.Show("លេខ​ POS នេះស្កេនម្ដងរួចហើយ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtBarcode.Focus();
                            txtBarcode.SelectAll();
                        }
                        else
                        {
                            MessageBox.Show("មានបញ្ហាអ្វីមួយ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ!\nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private void DgvRMListPOS_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                e.CellStyle.ForeColor = Color.Black;
            }
        }
        private void DgvRMListPOS_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 3)
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
                if (e.ColumnIndex == 3)
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

        //Semi BC
        private void LbWireTubeSemiBC_TextChanged(object sender, EventArgs e)
        {
            if (LbWireTubeSemiBC.Text.Trim() != "")
            {
                foreach (DataRow row in dtColor.Rows)
                {
                    if (LbWireTubeSemiBC.Text.ToString().Contains(row[0].ToString()))
                    {
                        LbWireTubeSemiBC.ForeColor = Color.FromName(row[1].ToString());
                        LbWireTubeSemiBC.BackColor = Color.FromName(row[2].ToString());
                    }
                }
            }
            else
            {
                LbWireTubeSemiBC.ForeColor = Color.Black;
                LbWireTubeSemiBC.BackColor = Color.White;
            }
        }
        private void TxtQtyReayBC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBatchQtyBC.Text.Trim() != "" && txtQtyPerBatchBC.Text.Trim() != "")
                {
                    ErrorText = "";
                    //Calc
                    try
                    {
                        double Batch = Convert.ToDouble(Convert.ToDouble(txtBatchQtyBC.Text).ToString("N0"));
                        txtBatchQtyBC.Text = Batch.ToString();
                        double QtyPerBatch = Convert.ToDouble(Convert.ToDouble(txtQtyPerBatchBC.Text).ToString("N0"));
                        txtQtyPerBatchBC.Text = QtyPerBatch.ToString();
                        double Reay = 0;
                        if (txtQtyReayBC.Text.Trim() != "")
                        {
                            Reay = Convert.ToDouble(Convert.ToDouble(txtQtyReayBC.Text).ToString("N0")); 
                            txtQtyReayBC.Text = Reay.ToString();
                        }
                        LbTotalQtySemiBC.Text = ((Batch * QtyPerBatch) + Reay).ToString("N0");
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
                        LbTotalQtySemiBC.Text = "";
                        MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtBatchQtyBC.Focus();
                    }
                }
                else
                {
                    LbTotalQtySemiBC.Text = "";
                    MessageBox.Show("សូមបញ្ចូលត្រង់ប្រអប់ដែលមានពណ៌!","Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    txtBatchQtyBC.Focus();
                }
            }
        }
        private void TxtQtyReayBC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void TxtQtyPerBatchBC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtQtyReayBC.Focus();
            }
        }
        private void TxtQtyPerBatchBC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void TxtBatchQtyBC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtQtyPerBatchBC.Focus();
            }
        }
        private void TxtBatchQtyBC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void TxtBarcodeSemi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcodeSemi.Text.Contains("/") == true)
                {
                    Cursor = Cursors.WaitCursor;
                    string[] BarcodeArray = txtBarcodeSemi.Text.ToString().Split('/');
                    string POSNo = BarcodeArray[0].ToString();
                    string WipCode = BarcodeArray[1].ToString();
                    string BoxNo = Convert.ToDouble(BarcodeArray[3]).ToString();
                    ErrorText = "";
                    ClearAllText();

                    //Checking Already input & Take POS Details
                    DataTable dtCheck = new DataTable();
                    DataTable dt = new DataTable();
                    try
                    {
                        cnn.con.Open();
                        string SQLQuery = "SELECT * FROM tbInventory WHERE LocCode = 'MC1' AND CountingMethod='Semi' " +
                            "\nAND CancelStatus = 0 AND QtyDetails2 = '"+POSNo+"|"+WipCode+"|"+BoxNo+"' ";
                        Console.WriteLine(SQLQuery);
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        sda.Fill(dtCheck);
                        
                        SQLQuery = "SELECT PosCNo, WIPCode, ItemName, Remarks1, Remarks2, Remarks3, " +
                            "\nNULLIF(CONCAT(MC1Name, " +
                            "\nCASE " +
                            "\n\tWHEN LEN(MC2Name)>1 THEN ' & ' " +
                            "\n\tELSE '' " +
                            "\nEND, MC2Name, " +
                            "\nCASE " +
                            "\n\tWHEN LEN(MC3Name)>1 THEN ' & ' " +
                            "\n\tELSE '' " +
                            "\nEND, MC3Name),'') AS MCName FROM " +
                            "\n(SELECT * FROM tbPOSDetailofMC) T1 " +
                            "\nINNER JOIN (SELECT * FROM tbMasterItem WHERE ItemType = 'Work In Process') T2 ON T1.WIPCode=T2.ItemCode " +
                            "\nWHERE PosCNo = '" + POSNo + "' AND WIPCode = '" + WipCode + "' " +
                            "\nORDER BY ItemName ASC, ItemCode ASC";
                        //Console.WriteLine(SQLQuery);
                        sda = new SqlDataAdapter(SQLQuery, cnn.con);
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
                            ErrorText = ErrorText + "\n" + ex.Message;
                        }
                    }
                    cnn.con.Close();

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        if (dtCheck.Rows.Count == 0)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                LbWIPCodeSemiBC.Text = WipCode;
                                LbPOSNoSemiBC.Text = dt.Rows[0]["PosCNo"].ToString();
                                LbBoxNoSemiBC.Text = BoxNo;
                                string MCName = dt.Rows[0]["MCName"].ToString();
                                if (MCName.Contains("&") == true)
                                {
                                    MCName = MCName.Replace("&", "&&");
                                }
                                LbMCNameSemiBC.Text = MCName;
                                LbWIPNameSemiBC.Text = dt.Rows[0]["ItemName"].ToString();
                                LbPINSemiBC.Text = dt.Rows[0]["Remarks1"].ToString();
                                LbLengthSemiBC.Text = dt.Rows[0]["Remarks3"].ToString();
                                LbWireTubeSemiBC.Text = dt.Rows[0]["Remarks2"].ToString();
                                txtBarcodeSemi.Text = "";
                                txtBatchQtyBC.Focus();
                            }
                            else
                            {
                                WMsg.WarningText = "គ្មានទិន្នន័យសឺមីនេះទេ!" +
                                    "\nWIP Code : " + WipCode + " " +
                                    "\nPOS No    : " + POSNo;
                                WMsg.ShowingMsg();
                                txtBarcodeSemi.Focus();
                                txtBarcodeSemi.SelectAll();
                            }
                        }
                        else
                        {
                            WMsg.WarningText = "ទិន្នន័យសឺមីនេះស្កេនរួចហើយ!" +
                                "\nWIP Code : " + WipCode + " " +
                                "\nPOS No    : " + POSNo + " " +
                                "\nBox No     : " + BoxNo;
                            WMsg.ShowingMsg();
                            txtBarcodeSemi.Focus();
                            txtBarcodeSemi.SelectAll();
                        }
                    }
                    else
                    {
                        EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                        EMsg.ShowingMsg();
                        txtBarcodeSemi.Focus();
                        txtBarcodeSemi.SelectAll();
                    }
                }
                else
                {
                    WMsg.WarningText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!";
                    WMsg.ShowingMsg();
                    txtBarcodeSemi.Focus();
                    txtBarcodeSemi.SelectAll();
                }
            }
        }
        private void TabCtrlSemi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCtrlSemi.SelectedIndex == 0)
            {
                txtBarcodeSemi.Focus();
            }
            else
            {
                txtWipNameSemi.Focus();
            }
        }

        //Semi
        private void TxtWipNameSemi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtWipNameSemi.Text.Trim() != "")
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;
                    dgvSemi.Rows.Clear();
                    DataTable dt = new DataTable();
                    try
                    {
                        cnnOBS.conOBS.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT UpItemCode, ItemName, Remark2, Remark3, Remark4 FROM " +
                            "(SELECT UpItemCode FROM mstbom WHERE DelFlag=0 GROUP BY UpItemCode)T1 " +
                            "INNER JOIN " +
                            "(SELECT * FROM mstitem WHERE ItemType=1 AND DelFlag=0)T2 " +
                            "ON T1.UpItemCode=T2.ItemCode " +
                            "WHERE ItemName LIKE '%" + txtWipNameSemi.Text + "%' " +
                            "ORDER BY ItemName ASC,UpItemCode ASC", cnnOBS.conOBS);
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
                            ErrorText = ErrorText + "\n" + ex.Message;
                        }
                    }
                    cnnOBS.conOBS.Close();

                    Cursor = Cursors.Default;
                    if (ErrorText.Trim() == "")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            dgvSemi.Rows.Add(row[0], row[1], row[2], row[3], row[4]);
                        }
                        dgvSemi.ClearSelection();
                    }
                    else
                    {
                        MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtWipNameSemi.Focus();
                    }
                }
            }
        }
        private void DgvSemi_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                if (e.Value.ToString() != "")
                {
                    foreach (DataRow row in dtColor.Rows)
                    {
                        if (e.Value.ToString().ToLower().Contains(row[0].ToString().ToLower()))
                        {
                            e.CellStyle.ForeColor = Color.FromName(row[1].ToString());
                            e.CellStyle.BackColor = Color.FromName(row[2].ToString());
                        }
                    }
                }
            }
        }
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
        private void DgvSemi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSemi.SelectedCells.Count > 0)
            {
                if (e.RowIndex > -1)
                {
                    txtBatchQtySemi.Focus();
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            //POS-Connector
            if (txtCountingType.Text.Contains("ខនិកទ័រ") == true)
            {
                SavePOSConnector();
            }
            //Semi
            else if (txtCountingType.Text.Contains("សឺមី") == true) 
            {
                SaveSemi();
            }
            //Wire/Teminal
            else
            {
                SaveWireTerminal();
            }
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearAllText();
            if (CountType == "POS")
            {
                txtBarcode.Focus();
            }
            else if (CountType == "Semi")
            {
                if (tabCtrlSemi.SelectedIndex == 0)
                {
                    txtBarcodeSemi.Focus();
                }
                else
                {
                    txtWipNameSemi.Focus();
                }
            }
            else
            {
                LbBarcodeTitleWireTerminal.Text = "ស្កេនឯកសារSD";
                ClearAllText();
                LbBobbinNoWireTerminal_W.Text = "";
                LbRMNameWireTerminal_W.Text = "";
                txtQtyBWireTerminal_W.Text = "";
                txtWBWireTerminal_W.Text = "";
                txtQtyAWireTerminal_W.Text = "";
                txtWBWireTerminal_W.Text = "";
                txtWAWireTerminal_W.Text = ""; 
                LbBobbinNoWireTerminal_T.Text = "";
                LbRMNameWireTerminal_T.Text = "";
                txtQtyBWireTerminal_T.Text = "";
                txtUseQtyWireTerminal_T.Text = "";
                dgvWireTerminal_W.Rows.Clear();
                dgvWireTerminal_T.Rows.Clear();
                btnOK_W.Enabled = false;
                btnOK_T.Enabled = false;
                txtBarcodeWireTerminal.Focus();
            }
        }
        private void InprocessCountingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.MCInproPrintStatus = chkPrintStatus.Checked;
            Properties.Settings.Default.Save();
        }
        private void InprocessCountingForm_Load(object sender, EventArgs e)
        {
            chkPrintStatus.Checked = Properties.Settings.Default.MCInproPrintStatus;
            txtCountingType.Text = "រាប់ខនិកទ័រ (ស្កេន)";
            dgvWireTerminal_W.RowHeadersDefaultCellStyle.Font = new Font(dgvWireTerminal_W.RowHeadersDefaultCellStyle.Font, FontStyle.Regular);
            foreach (DataGridViewColumn col in dgvWireTerminal_W.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.btnOK_W.Enabled = false;
            dgvWireTerminal_T.RowHeadersDefaultCellStyle.Font = new Font(dgvWireTerminal_T.RowHeadersDefaultCellStyle.Font, FontStyle.Regular);
            foreach (DataGridViewColumn col in dgvWireTerminal_T.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.btnOK_T.Enabled = false;

            //Remove tap of Semi Manual
            tabCtrlSemi.TabPages.RemoveAt(1);
                        
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
                if (DgvCol.Index != 3)
                {
                    DgvCol.ReadOnly = true;
                }
            }

        }
        private void TxtCountingType_TextChanged(object sender, EventArgs e)
        {
            string CountTypeInKhmer = "";
            if (txtCountingType.Text.Contains("ខនិកទ័រ") == true)
            {
                CountTypeInKhmer = "ខនិកទ័រ";
                PicCurrentCountingType.BackgroundImage = imgListCountingType.Images[0];
                panelPOS.Visible = true;
                panelPOS.BringToFront();
                panelSemi.Visible = false;
                panelStockCardWireTerminal.Visible = false;
                txtBarcode.Focus();
            }
            else if (txtCountingType.Text.Contains("សឺមី") == true)
            {
                CountTypeInKhmer = "សឺមី";
                PicCurrentCountingType.BackgroundImage = imgListCountingType.Images[1]; 
                panelSemi.Visible = true;
                panelSemi.BringToFront();
                panelPOS.Visible = false;
                panelStockCardWireTerminal.Visible = false;
                txtBarcodeSemi.Focus();
            }
            else
            {
                CountTypeInKhmer = "ខ្សែភ្លើង/ធើមីណល";
                PicCurrentCountingType.BackgroundImage = imgListCountingType.Images[2];
                panelStockCardWireTerminal.Visible = true;
                panelStockCardWireTerminal.BringToFront();
                panelSemi.Visible = false;
                panelPOS.Visible = false;
                txtBarcodeWireTerminal.Focus();
            }
            GrbLocAndType.Text = "ប្រភេទ ៖ " + CountTypeInKhmer ;

            /*
            if (CountType == "POS")
            {
                panelPOS.Visible = true;
                panelPOS.BringToFront();
                panelSemi.Visible = false;
                panelStockCardWireTerminal.Visible = false;
                txtBarcode.Focus();
            }
            else if (CountType == "Semi")
            {
                panelSemi.Visible = true;
                panelSemi.BringToFront();
                panelPOS.Visible = false;
                panelStockCardWireTerminal.Visible = false;
                txtBarcodeSemi.Focus();
            }
            else
            {
                panelStockCardWireTerminal.Visible = true;
                panelStockCardWireTerminal.BringToFront();
                panelSemi.Visible = false;
                panelPOS.Visible = false;
                txtItems.Focus();
            }
            */
        }
        private void TxtCountingType_Click(object sender, EventArgs e)
        {
            InprocessCountingTypeForm Ictf = new InprocessCountingTypeForm(this.txtCountingType);
            Ictf.ShowDialog();
        }
        private void PicCurrentCountingType_Click(object sender, EventArgs e)
        {
            InprocessCountingTypeForm Ictf = new InprocessCountingTypeForm(this.txtCountingType);
            Ictf.ShowDialog();
        }

        //Function
        private void ClearAllText()
        {
            //POS Connector
            if (CountType == "POS")
            {
                LbMCNamePOS.Text = "";
                LbPOSNoPOS.Text = "";
                LbQtyPOS.Text = "";
                LbItemNamePOS.Text = "";
                LbShipmentDatePOS.Text = "";
                dgvRMListPOS.Rows.Clear();
            }

            //SemiBC
            else if (CountType == "Semi") 
            {
                LbMCNameSemiBC.Text = "";
                LbBoxNoSemiBC.Text = "";
                LbPOSNoSemiBC.Text = "";
                LbWIPCodeSemiBC.Text = "";
                LbWIPNameSemiBC.Text = "";
                LbLengthSemiBC.Text = "";
                LbPINSemiBC.Text = "";
                LbWireTubeSemiBC.Text = "";
                txtBatchQtyBC.Text = "";
                txtQtyPerBatchBC.Text = "";
                txtQtyReayBC.Text = "";
                LbTotalQtySemiBC.Text = "";

                //SemiManual
                //txtWipNameSemi.Text = "";
                //dgvSemi.Rows.Clear();
                //txtBatchQtySemi.Text = "";
                //txtQtyPerBatchSemi.Text = "";
                //txtQtyReaySemi.Text = "";
                //LbTotalQtySemi.Text = "";
            }

            //WireTerminal
            else
            {                
                if (LbBarcodeTitleWireTerminal.Text.ToString().Contains("SD")==true)
                {
                    LbSDNoWireTerminal.Text = "";
                    LbMCNameWireTerminal.Text = "";
                }
                if (tabContrlWireTerminal.SelectedIndex == 0)
                {
                    LbBobbinNoWireTerminal_W.Text = "";
                    LbRMNameWireTerminal_W.Text = "";
                    txtQtyBWireTerminal_W.Text = "";
                    txtWBWireTerminal_W.Text = "";
                    txtQtyAWireTerminal_W.Text = "";
                    txtWBWireTerminal_W.Text = "";
                    txtWAWireTerminal_W.Text = "";
                }
                else
                {
                    LbBobbinNoWireTerminal_T.Text = "";
                    LbRMNameWireTerminal_T.Text = "";
                    txtQtyBWireTerminal_T.Text = "";
                    txtUseQtyWireTerminal_T.Text = "";
                }
            }
        }
        private void ClearTextPos()
        {
            LbItemNamePOS.Text = "";
            LbPOSNoPOS.Text = "";
            LbQtyPOS.Text = "";
            LbShipmentDatePOS.Text = "";

        }
        private void SavePOSConnector()
        {
            if (LbItemNamePOS.Text.Trim() != "" && dgvRMListPOS.Rows.Count > 0)
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់រក្សាទុកមែនឬទេ?","Rachhan System",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;
                    DataTable dtLabelNo = new DataTable();
                    DataTable dtLabelNoRange = new DataTable();

                    //Check LabelNo
                    try
                    {
                        cnn.con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT LabelNo AS LastLabelNo FROM tbInventory WHERE LabelNo = (SELECT MAX(LabelNo) FROM tbInventory WHERE LocCode='MC1') GROUP BY LabelNo", cnn.con);
                        sda.Fill(dtLabelNo);

                        SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM tbSDMCLocation WHERE LocCode='MC1'", cnn.con);
                        sda1.Fill(dtLabelNoRange);

                    }
                    catch (Exception ex)
                    {
                        if (ErrorText.Trim() == "")
                        {
                            ErrorText = "Take Label No : " + ex.Message;
                        }
                        else
                        {
                            ErrorText = ErrorText + "\nTake Label No : " + ex.Message;
                        }
                    }
                    cnn.con.Close();

                    //Insert data to DB
                    if (ErrorText.Trim() == "")
                    {
                        int LabelNo = Convert.ToInt32(dtLabelNoRange.Rows[0][2].ToString());
                        if (dtLabelNo.Rows.Count > 0)
                        {
                            LabelNo = Convert.ToInt32(dtLabelNo.Rows[0][0].ToString()) + 1;
                        }
                        string Username = MenuFormV2.UserForNextForm;
                        DateTime RegDate = DateTime.Now;

                        foreach (DataGridViewRow DgvRow in dgvRMListPOS.Rows)
                        {
                            try
                            {
                                cnn.con.Open();
                                string LocCode = "MC1";
                                string SubLoc = LbMCNamePOS.Text;
                                string CountingMethod = "POS";
                                string QtyDetails = LbPOSNoPOS.Text;
                                string ItemCode = DgvRow.Cells[0].Value.ToString();
                                string ItemType = "Material";
                                int Qty = Convert.ToInt32(DgvRow.Cells[3].Value.ToString());

                                if (LabelNo <= Convert.ToInt32(dtLabelNoRange.Rows[0][3].ToString()))
                                {
                                    cmd = new SqlCommand("INSERT INTO tbInventory(LocCode, SubLoc, LabelNo, CountingMethod, SeqNo, ItemCode, ItemType, Qty, QtyDetails, RegDate, RegBy, UpdateDate, UpdateBy, CancelStatus) " +
                                    "VALUES (@Lc, @Slc, @lbn, @Cm, @Sn, @Ic, @It, @Qty, @QtyD, @RegD, @RegB, @UpD, @UpB, @Cs)", cnn.con);

                                    cmd.Parameters.AddWithValue("@Lc", LocCode);
                                    cmd.Parameters.AddWithValue("@Slc", SubLoc);
                                    cmd.Parameters.AddWithValue("@lbn", LabelNo);
                                    cmd.Parameters.AddWithValue("@Cm", CountingMethod);
                                    cmd.Parameters.AddWithValue("@Sn", DgvRow.Index + 1);
                                    cmd.Parameters.AddWithValue("@Ic", ItemCode);
                                    cmd.Parameters.AddWithValue("@It", ItemType);
                                    cmd.Parameters.AddWithValue("@Qty", Qty);
                                    cmd.Parameters.AddWithValue("@QtyD", QtyDetails);
                                    cmd.Parameters.AddWithValue("@RegD", RegDate);
                                    cmd.Parameters.AddWithValue("@RegB", Username);
                                    cmd.Parameters.AddWithValue("@UpD", RegDate);
                                    cmd.Parameters.AddWithValue("@UpB", Username);
                                    cmd.Parameters.AddWithValue("@Cs", 0);

                                    cmd.ExecuteNonQuery();

                                }
                                else
                                {
                                    if (ErrorText.Trim() == "")
                                    {
                                        ErrorText = "Label No reach maximum : " + LabelNo.ToString("N0");
                                    }
                                    else
                                    {
                                        ErrorText = ErrorText + "\nLabel No reach maximum : " + LabelNo.ToString("N0");
                                    }
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ErrorText.Trim() == "")
                                {
                                    ErrorText = "Insert to DB : " + ex.Message;
                                }
                                else
                                {
                                    ErrorText = ErrorText + "\nInsert to DB : " + ex.Message;
                                }
                                break;
                            }
                            cnn.con.Close();
                        }

                        //Print
                        if (chkPrintStatus.Checked == true)
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
                            wsBarcode.Cells[2, 3] = LabelNo.ToString();
                            wsBarcode.Cells[3, 1] = "*" + LabelNo.ToString() + "*";
                            wsBarcode.Cells[2, 4] = "Inprocess(" + LocSelected + ")";
                            wsBarcode.Cells[4, 3] = LbPOSNoPOS.Text;
                            wsBarcode.Cells[5, 3] = LbItemNamePOS.Text.ToString();
                            wsBarcode.Cells[6, 3] = LbQtyPOS.Text.ToString();

                            // Material List
                            for (int i = 0; i < dgvRMListPOS.Rows.Count; i++)
                            {
                                wsBarcode.Cells[i + 10, 1] = dgvRMListPOS.Rows[i].Cells[0].Value.ToString();
                                wsBarcode.Cells[i + 10, 2] = dgvRMListPOS.Rows[i].Cells[1].Value.ToString();
                                wsBarcode.Cells[i + 10, 5] = dgvRMListPOS.Rows[i].Cells[3].Value.ToString();

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
                            for (int j = 0; j < numPrintQty.Value; j++)
                            {
                                wsBarcode.PrintOut();
                            }


                            //Save Excel ទុក
                            string fName = "";
                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                            string file = LabelNo.ToString();
                            wsBarcode.Name = "RachhanSystem";
                            fName = file + "( " + date + " )";
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
                        ClearTextPos();
                        dgvRMListPOS.Rows.Clear();
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
                MessageBox.Show("គ្មានទិន្នន័យ <បញ្ជីវត្ថុធាតុដើម> ដែលត្រូវរក្សាទុកទេ?", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBarcode.Focus();
            }
        }
        private void SaveSemi()
        {
            string Loc = "MC1";
            string SubLoc = LbMCNameSemiBC.Text;
            string WipCode = "";
            string WIPName = "";
            string Pin = "";
            string WireColor = "";
            string Length = "";
            string CountingM = "Semi";
            string ItemType = CountingM;
            int Qty = 0;
            string QtyDetails = "";
            string QtyDetails2= "";
            ErrorText = "";

            //Semi BC
            if (tabCtrlSemi.SelectedIndex == 0)
            {
                if (LbWIPNameSemiBC.Text.Trim() != "")
                {
                    if (txtBatchQtyBC.Text.Trim() != "" && txtQtyPerBatchBC.Text.Trim() != "")
                    {
                        //Calc
                        try
                        {
                            double Batch = Convert.ToDouble(Convert.ToDouble(txtBatchQtyBC.Text).ToString("N0"));
                            txtBatchQtyBC.Text = Batch.ToString();
                            double QtyPerBatch = Convert.ToDouble(Convert.ToDouble(txtQtyPerBatchBC.Text).ToString("N0"));
                            txtQtyPerBatchBC.Text = QtyPerBatch.ToString();
                            double Reay = 0;
                            if (txtQtyReayBC.Text.Trim() != "")
                            {
                                Reay = Convert.ToDouble(Convert.ToDouble(txtQtyReayBC.Text).ToString("N0"));
                                txtQtyReayBC.Text = Reay.ToString();
                            }
                            LbTotalQtySemiBC.Text = ((Batch * QtyPerBatch) + Reay).ToString("N0");
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
                            WipCode = LbWIPCodeSemiBC.Text;
                            WIPName = LbWIPNameSemiBC.Text;
                            Pin = LbPINSemiBC.Text;
                            WireColor =LbWireTubeSemiBC.Text;
                            Length = LbLengthSemiBC.Text;
                            Qty = Convert.ToInt32(LbTotalQtySemiBC.Text.Replace(",",""));
                            QtyDetails = txtBatchQtyBC.Text.ToString()+"x"+txtQtyPerBatchBC.Text.ToString();
                            QtyDetails2 = LbPOSNoSemiBC.Text + "|" + WipCode + "|" + LbBoxNoSemiBC.Text;
                            if (txtQtyReayBC.Text.Trim() != "")
                            {
                                QtyDetails = QtyDetails+"+"+txtQtyReayBC.Text.ToString();
                            }
                        }
                        else
                        {
                            LbTotalQtySemiBC.Text = "";
                            if (ErrorText.Trim() == "")
                            {
                                ErrorText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!\n" + ErrorText;
                            }
                            else
                            {
                                ErrorText = ErrorText + "\nអ្នកបញ្ចូលខុសទម្រង់ហើយ!\n" + ErrorText;
                            }
                            txtBatchQtyBC.Focus();
                        }
                    }
                    else
                    {
                        LbTotalQtySemiBC.Text = "";
                        if (ErrorText.Trim() == "")
                        {
                            ErrorText = "សូមបញ្ចូលត្រង់ សម្រាយចំនួន និងចំនួនសរុប! (ប្រអប់ដែលមានពណ៌)";
                        }
                        else
                        {
                            ErrorText = ErrorText + "\nសូមបញ្ចូលត្រង់ សម្រាយចំនួន និងចំនួនសរុប! (ប្រអប់ដែលមានពណ៌)";
                        }
                        txtBatchQtyBC.Focus();
                    }
                }
                else
                {
                    if (ErrorText.Trim() == "")
                    {
                        ErrorText = "សូមស្កេនបាកូដជាមុនសិន!";
                    }
                    else
                    {
                        ErrorText = ErrorText + "\nសូមស្កេនបាកូដជាមុនសិន!";
                    }
                    txtBarcodeSemi.Focus();
                }
            }
            //Semi Manual
            /*
            else
            {
                if (dgvSemi.SelectedCells.Count>0)
                {
                    WipCode = dgvSemi.Rows[dgvSemi.CurrentCell.RowIndex].Cells[0].Value.ToString();
                    WIPName = dgvSemi.Rows[dgvSemi.CurrentCell.RowIndex].Cells[1].Value.ToString();
                    Pin = dgvSemi.Rows[dgvSemi.CurrentCell.RowIndex].Cells[2].Value.ToString();
                    WireColor = dgvSemi.Rows[dgvSemi.CurrentCell.RowIndex].Cells[3].Value.ToString();
                    Length = dgvSemi.Rows[dgvSemi.CurrentCell.RowIndex].Cells[4].Value.ToString();
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
                            Qty = Convert.ToInt32(LbTotalQtySemi.Text.Replace(",", ""));
                            QtyDetails = txtBatchQtySemi.Text.ToString() + "x" + txtQtyPerBatchSemi.Text.ToString();
                            if (txtQtyReaySemi.Text.Trim() != "")
                            {
                                QtyDetails = QtyDetails + "+" + txtQtyReaySemi.Text.ToString();
                            }
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
                        if (ErrorText.Trim() == "")
                        {
                            ErrorText = "សូមបញ្ចូលត្រង់ សម្រាយចំនួន និងចំនួនសរុប! (ប្រអប់ដែលមានពណ៌)";
                        }
                        else
                        {
                            ErrorText = ErrorText + "\nសូមបញ្ចូលត្រង់ សម្រាយចំនួន និងចំនួនសរុប! (ប្រអប់ដែលមានពណ៌)";
                        }
                        txtBatchQtySemi.Focus();
                    }
                }
                else
                {
                    if (ErrorText.Trim() == "")
                    {
                        ErrorText = "សូមស្វែងរកនិងជ្រើសរើសសឺមីជាមុនសិន!";
                    }
                    else
                    {
                        ErrorText = ErrorText + "\nសូមស្វែងរកនិងជ្រើសរើសសឺមីជាមុនសិន!";
                    }
                    txtWipNameSemi.Focus();
                }
            }
            */

            if (ErrorText.Trim() == "")
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់រក្សាទុកមែនឬទេ?" + "( " +WIPName + " )", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;
                    DataTable dtLabelNo = new DataTable();
                    DataTable dtLabelNoRange = new DataTable();

                    //Check LabelNo
                    try
                    {
                        cnn.con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT LabelNo AS LastLabelNo FROM tbInventory WHERE LabelNo = (SELECT MAX(LabelNo) FROM tbInventory WHERE LocCode='MC1') GROUP BY LabelNo", cnn.con);
                        sda.Fill(dtLabelNo);

                        SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM tbSDMCLocation WHERE LocCode='MC1'", cnn.con);
                        sda1.Fill(dtLabelNoRange);

                    }
                    catch (Exception ex)
                    {
                        if (ErrorText.Trim() == "")
                        {
                            ErrorText = "Take Label No : " + ex.Message;
                        }
                        else
                        {
                            ErrorText = ErrorText + "\nTake Label No : " + ex.Message;
                        }
                    }
                    cnn.con.Close();

                    //Insert data to DB
                    if (ErrorText.Trim() == "")
                    {
                        int LabelNo = Convert.ToInt32(dtLabelNoRange.Rows[0][2].ToString());
                        if (dtLabelNo.Rows.Count > 0)
                        {
                            LabelNo = Convert.ToInt32(dtLabelNo.Rows[0][0].ToString()) + 1;
                        }
                        string Username = MenuFormV2.UserForNextForm;
                        DateTime RegDate = DateTime.Now;

                        try
                        {
                            cnn.con.Open();                            
                            if (LabelNo <= Convert.ToInt32(dtLabelNoRange.Rows[0][3].ToString()))
                            {
                                cmd = new SqlCommand("INSERT INTO tbInventory(LocCode, SubLoc, LabelNo, CountingMethod, SeqNo, ItemCode, ItemType, Qty, QtyDetails, QtyDetails2, RegDate, RegBy, UpdateDate, UpdateBy, CancelStatus) " +
                                "VALUES (@Lc, @Slc, @lbn, @Cm, @Sn, @Ic, @It, @Qty, @QtyD, @QtyD2, @RegD, @RegB, @UpD, @UpB, @Cs)", cnn.con);

                                cmd.Parameters.AddWithValue("@Lc", Loc);
                                cmd.Parameters.AddWithValue("@Slc", SubLoc);
                                cmd.Parameters.AddWithValue("@lbn", LabelNo);
                                cmd.Parameters.AddWithValue("@Cm", CountingM);
                                cmd.Parameters.AddWithValue("@Sn", 1);
                                cmd.Parameters.AddWithValue("@Ic", WipCode);
                                cmd.Parameters.AddWithValue("@It", ItemType);
                                cmd.Parameters.AddWithValue("@Qty", Qty);
                                cmd.Parameters.AddWithValue("@QtyD", QtyDetails);
                                cmd.Parameters.AddWithValue("@QtyD2", QtyDetails2);
                                cmd.Parameters.AddWithValue("@RegD", RegDate);
                                cmd.Parameters.AddWithValue("@RegB", Username);
                                cmd.Parameters.AddWithValue("@UpD", RegDate);
                                cmd.Parameters.AddWithValue("@UpB", Username);
                                cmd.Parameters.AddWithValue("@Cs", 0);

                                cmd.ExecuteNonQuery();

                            }
                            else
                            {
                                if (ErrorText.Trim() == "")
                                {
                                    ErrorText = "Label No reach maximum : " + LabelNo.ToString("N0");
                                }
                                else
                                {
                                    ErrorText = ErrorText + "\nLabel No reach maximum : " + LabelNo.ToString("N0");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ErrorText.Trim() == "")
                            {
                                ErrorText = "Insert to DB : " + ex.Message;
                            }
                            else
                            {
                                ErrorText = ErrorText + "\nInsert to DB : " + ex.Message;
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
                                    Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Semi"];

                                    worksheet.Cells[2, 6] = "Inprocess(" + SubLoc + ")";
                                    worksheet.Cells[2, 4] = LabelNo;
                                    worksheet.Cells[3, 1] = "*" + LabelNo + "*";
                                    worksheet.Cells[7, 4] = WipCode;
                                    worksheet.Cells[9, 4] = WIPName;
                                    worksheet.Cells[11, 4] = Pin;
                                    worksheet.Cells[13, 4] = WireColor;
                                    worksheet.Cells[15, 4] = Length;

                                    worksheet.Cells[17, 4] = Qty;
                                    //Summary
                                    worksheet.Cells[17, 6] = "( " + QtyDetails + " )";

                                    // Saving the modified Excel file
                                    var CDirectory2 = Environment.CurrentDirectory;
                                    string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                    string file = LabelNo.ToString();
                                    string fName = file + "( " + date + " )";

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
                                    for (int j = 0; j < numPrintQty.Value; j++)
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

                    }

                    Cursor = Cursors.Default;
                    if (ErrorText.Trim() == "")
                    {
                        MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnNew.PerformClick();
                    }
                    else
                    {
                        MessageBox.Show("រក្សាទុកមានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show(ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void SaveWireTerminal()
        {
            if (dgvWireTerminal_W.Rows.Count + dgvWireTerminal_T.Rows.Count > 0)
            {
                string RMTypeNotYet = "";
                foreach (DataGridViewRow row in dgvWireTerminal_W.Rows)
                {
                    if (row.Cells["StatusW"].Value == null || row.Cells["StatusW"].Value.ToString().Trim() != "✔️")
                    {
                        RMTypeNotYet = "Wire";
                        break;
                    }
                }
                foreach (DataGridViewRow row in dgvWireTerminal_T.Rows)
                {
                    if (row.Cells["StatusT"].Value == null || row.Cells["StatusT"].Value.ToString().Trim() != "✔️")
                    {
                        if (RMTypeNotYet.Trim() == "")
                            RMTypeNotYet = "Terminal";
                        else
                            RMTypeNotYet += " & Terminal";
                        break;
                    }
                }

                if (RMTypeNotYet.Trim() == "")
                {
                    QMsg.QAText = "តើអ្នកចង់រក្សាទិន្នន័យនេះទុកមែនឬទេ?";
                    QMsg.UserClickedYes = false;
                    QMsg.ShowingMsg();
                    if (QMsg.UserClickedYes == true)
                    {
                        ErrorText = "";
                        Cursor = Cursors.WaitCursor;

                        DataTable dtLabelNo = new DataTable();
                        DataTable dtLabelNoRange = new DataTable();

                        //Check LabelNo
                        try
                        {
                            cnn.con.Open();
                            SqlDataAdapter sda = new SqlDataAdapter("SELECT LabelNo AS LastLabelNo FROM tbInventory WHERE LabelNo = (SELECT MAX(LabelNo) FROM tbInventory WHERE LocCode='MC1') GROUP BY LabelNo", cnn.con);
                            sda.Fill(dtLabelNo);

                            sda = new SqlDataAdapter("SELECT * FROM tbSDMCLocation WHERE LocCode='MC1'", cnn.con);
                            sda.Fill(dtLabelNoRange);

                        }
                        catch (Exception ex)
                        {
                            if (ErrorText.Trim() == "")
                            {
                                ErrorText = "Take Label No : " + ex.Message;
                            }
                            else
                            {
                                ErrorText = ErrorText + "\nTake Label No : " + ex.Message;
                            }
                        }
                        cnn.con.Close();

                        //Insert data to DB
                        if (ErrorText.Trim() == "")
                        {
                            int LabelNo = Convert.ToInt32(dtLabelNoRange.Rows[0][2].ToString());
                            if (dtLabelNo.Rows.Count > 0)
                            {
                                LabelNo = Convert.ToInt32(dtLabelNo.Rows[0][0].ToString()) + 1;
                            }
                            string Username = MenuFormV2.UserForNextForm;
                            DateTime RegDate = DateTime.Now;

                            if (LabelNo <= Convert.ToInt32(dtLabelNoRange.Rows[0][3].ToString()))
                            {
                                DataTable dtItemSaving = new DataTable();
                                dtItemSaving.Columns.Add("SeqNo");
                                dtItemSaving.Columns.Add("RMCode");
                                dtItemSaving.Columns.Add("RMName");
                                dtItemSaving.Columns.Add("RMType");
                                dtItemSaving.Columns.Add("TotalQty");
                                dtItemSaving.Columns.Add("TotalBobbinsQty");

                                //Wire
                                foreach (DataGridViewRow row in dgvWireTerminal_W.Rows)
                                {
                                    string RMCode = row.Cells["RMCodeW"].Value.ToString();
                                    string RMName = row.Cells["RMNameW"].Value.ToString();
                                    string RMType = "Wire";
                                    int CountDup = 0;
                                    foreach (DataRow dtRow in dtItemSaving.Rows)
                                    {
                                        if (RMCode == dtRow["RMCode"].ToString())
                                        {
                                            CountDup++;
                                            break;
                                        }
                                    }

                                    if (CountDup == 0)
                                    {
                                        int TotalQty = 0;
                                        int TotalBobbinQty = 0;
                                        foreach (DataGridViewRow rowTotal in dgvWireTerminal_W.Rows)
                                        {
                                            if (RMCode == rowTotal.Cells["RMCodeW"].Value.ToString())
                                            {
                                                TotalQty += Convert.ToInt32(rowTotal.Cells["RemainQtyW"].Value.ToString());
                                                TotalBobbinQty++;
                                            }                                                
                                        }
                                        dtItemSaving.Rows.Add((dtItemSaving.Rows.Count+1), RMCode, RMName, RMType, TotalQty, TotalBobbinQty);
                                        dtItemSaving.AcceptChanges();
                                    }
                                }

                                //Terminal
                                foreach (DataGridViewRow row in dgvWireTerminal_T.Rows)
                                {
                                    string RMCode = row.Cells["RMCodeT"].Value.ToString();
                                    string RMName = row.Cells["RMNameT"].Value.ToString();
                                    string RMType = "Terminal";
                                    int CountDup = 0;
                                    foreach (DataRow dtRow in dtItemSaving.Rows)
                                    {
                                        if (RMCode == dtRow["RMCode"].ToString())
                                        {
                                            CountDup++;
                                            break;
                                        }
                                    }

                                    if (CountDup == 0)
                                    {
                                        int TotalQty = 0;
                                        int TotalBobbinQty = 0;
                                        foreach (DataGridViewRow rowTotal in dgvWireTerminal_T.Rows)
                                        {
                                            if (RMCode == rowTotal.Cells["RMCodeT"].Value.ToString())
                                            {
                                                TotalQty += Convert.ToInt32(rowTotal.Cells["RemainQtyT"].Value.ToString());
                                                TotalBobbinQty++;
                                            }
                                        }
                                        dtItemSaving.Rows.Add((dtItemSaving.Rows.Count + 1), RMCode, RMName, RMType, TotalQty, TotalBobbinQty);
                                        dtItemSaving.AcceptChanges();
                                    }
                                }

                                /*

                                // Display the DataTable in the console
                                Console.WriteLine(LabelNo.ToString());
                                foreach (DataColumn column in dtItemSaving.Columns)
                                {
                                    Console.Write($"{column.ColumnName}\t");
                                }
                                Console.WriteLine(); // New line for each row
                                foreach (DataRow row in dtItemSaving.Rows)
                                {
                                    foreach (DataColumn column in dtItemSaving.Columns)
                                    {
                                        Console.Write($"{row[column]} \t");
                                    }
                                    Console.WriteLine(); // New line for each row
                                }

                                */

                                //Insert to DB
                                try
                                {
                                    cnn.con.Open();
                                    foreach (DataRow row in dtItemSaving.Rows)
                                    {
                                        cmd = new SqlCommand("INSERT INTO tbInventory(LocCode, SubLoc, LabelNo, CountingMethod, SeqNo, ItemCode, ItemType, Qty, QtyDetails, RegDate, RegBy, UpdateDate, UpdateBy, CancelStatus) " +
                                        "VALUES (@Lc, @Slc, @lbn, @Cm, @Sn, @Ic, @It, @Qty, @QtyD, @RegD, @RegB, @UpD, @UpB, @Cs)", cnn.con);

                                        cmd.Parameters.AddWithValue("@Lc", "MC1");
                                        cmd.Parameters.AddWithValue("@Slc", LbMCNameWireTerminal.Text);
                                        cmd.Parameters.AddWithValue("@lbn", LabelNo);
                                        cmd.Parameters.AddWithValue("@Cm", "SD Document");
                                        cmd.Parameters.AddWithValue("@Sn", Convert.ToInt32(row["SeqNo"]));
                                        cmd.Parameters.AddWithValue("@Ic", row["RMCode"]);
                                        cmd.Parameters.AddWithValue("@It", "Material");
                                        cmd.Parameters.AddWithValue("@Qty", Convert.ToInt32(row["TotalQty"]));
                                        cmd.Parameters.AddWithValue("@QtyD", LbSDNoWireTerminal.Text);
                                        cmd.Parameters.AddWithValue("@RegD", RegDate);
                                        cmd.Parameters.AddWithValue("@RegB", Username);
                                        cmd.Parameters.AddWithValue("@UpD", RegDate);
                                        cmd.Parameters.AddWithValue("@UpB", Username);
                                        cmd.Parameters.AddWithValue("@Cs", 0);
                                        cmd.ExecuteNonQuery();
                                    }

                                    foreach (DataGridViewRow row in dgvWireTerminal_W.Rows)
                                    {
                                        cmd = new SqlCommand("INSERT INTO tbInventoryWandTDetails(LabelNo, SDNo, BobbinSysNo, RMCode, RemainW, RemainQty) " +
                                        "VALUES (@Ln, @Sn, @Bobbin, @Rc, @RmW, @RmQty)", cnn.con);

                                        cmd.Parameters.AddWithValue("@Ln", LabelNo);
                                        cmd.Parameters.AddWithValue("@Sn", LbSDNoWireTerminal.Text);
                                        cmd.Parameters.AddWithValue("@Bobbin", row.Cells["BobbinCodeW"].Value.ToString());
                                        cmd.Parameters.AddWithValue("@Rc", row.Cells["RMCodeW"].Value.ToString());
                                        cmd.Parameters.AddWithValue("@RmW", Convert.ToDouble(row.Cells["RemainWW"].Value.ToString()));
                                        cmd.Parameters.AddWithValue("@RmQty", Convert.ToInt32(row.Cells["RemainQtyW"].Value.ToString()));
                                        cmd.ExecuteNonQuery();
                                    }

                                }
                                catch(Exception ex)
                                {
                                    if (ErrorText.Trim() == "")
                                    {
                                        ErrorText = "Insert to DB : " + ex.Message;
                                    }
                                    else
                                    {
                                        ErrorText = ErrorText + "\nInsert to DB : " + ex.Message;
                                    }
                                }
                                cnn.con.Close();

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

                                        //open excel application and create new workbook
                                        var CDirectory = Environment.CurrentDirectory;
                                        Excel.Application excelApp = new Excel.Application();
                                        Excel.Workbook xlWorkBook = null;
                                        Excel.Worksheet worksheet = null;
                                        Excel.Worksheet DeleteWorksheet = null;

                                        try
                                        {                                            
                                            foreach (DataRow row in dtItemSaving.Rows)
                                            {
                                                string RMCode = row["RMCode"].ToString();
                                                string RMName = row["RMName"].ToString();
                                                string RMType = row["RMType"].ToString();
                                                double TotalQty = Convert.ToDouble(row["TotalQty"]);
                                                double TototalBobbinQty = Convert.ToDouble(row["TotalBobbinsQty"]);
                                                string QtyAndBobbin = TotalQty.ToString("N0") + " m (" + TototalBobbinQty.ToString("N0") +" Bobbins)";
                                                if (TotalQty > 0)
                                                {
                                                    xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\InventoryLabel_SD_Template.xlsx", Editable: true);
                                                    worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Wire"];
                                                    DeleteWorksheet = (Excel.Worksheet)xlWorkBook.Sheets["Terminal"];
                                                    if (RMType != "Wire")
                                                    {
                                                        worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Terminal"];
                                                        DeleteWorksheet = (Excel.Worksheet)xlWorkBook.Sheets["Wire"];
                                                        QtyAndBobbin = TotalQty.ToString("N0") + " Pcs (" + TototalBobbinQty.ToString("N0") + " Bobbins/Reels)";
                                                    }

                                                    //Header
                                                    worksheet.Cells[1, 1] = RegDate;
                                                    worksheet.Cells[2, 3] = LabelNo+"/"+ RMCode;
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
                                                        foreach (DataGridViewRow RowDgv in dgvWireTerminal_W.Rows)
                                                        {
                                                            if (RMCode == RowDgv.Cells["RMCodeW"].Value.ToString())
                                                            {
                                                                worksheet.Cells[WriteIndex, 1] = WriteSeqNo;
                                                                worksheet.Cells[WriteIndex, 2] = RowDgv.Cells["BobbinCodeW"].Value.ToString();
                                                                worksheet.Cells[WriteIndex, 3] = Convert.ToDouble(RowDgv.Cells["RemainWW"].Value);
                                                                worksheet.Cells[WriteIndex, 4] = Convert.ToDouble(RowDgv.Cells["RemainQtyW"].Value);

                                                                WriteSeqNo++;
                                                                WriteIndex++;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        int WriteSeqNo = 1;
                                                        int WriteIndex = 9;
                                                        foreach (DataGridViewRow RowDgv in dgvWireTerminal_T.Rows)
                                                        {
                                                            if (RMCode == RowDgv.Cells["RMCodeT"].Value.ToString())
                                                            {
                                                                worksheet.Cells[WriteIndex, 1] = WriteSeqNo;
                                                                worksheet.Cells[WriteIndex, 2] = RowDgv.Cells["BobbinCodeT"].Value.ToString();
                                                                worksheet.Cells[WriteIndex, 4] = Convert.ToDouble(RowDgv.Cells["RemainQtyT"].Value);

                                                                WriteSeqNo++;
                                                                WriteIndex++;
                                                            }
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
                                            }
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
                            }
                            else
                            {
                                if (ErrorText.Trim() == "")
                                {
                                    ErrorText = "Label No reach maximum : " + LabelNo.ToString("N0");
                                }
                                else
                                {
                                    ErrorText = ErrorText + "\nLabel No reach maximum : " + LabelNo.ToString("N0");
                                }
                            }
                        }

                        Cursor = Cursors.Default;

                        if (ErrorText.Trim() == "")
                        {
                            InfoMsg.InfoText = "រក្សាទុករួចរាល់!";
                            InfoMsg.ShowingMsg();
                            btnNew.PerformClick();
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
                    WMsg.WarningText = "រកឃើញទិន្នន័យប៊ូប៊ីនដែលមិនទាន់បញ្ចូល! (" + RMTypeNotYet + ")" +
                        "\nសូមបញ្ចូលជាមុនសិនមុនហ្នឹងបន្ត!";
                    WMsg.ShowingMsg();
                    txtBarcodeWireTerminal.Focus();
                }
            }
            else
            {
                WMsg.WarningText = "សូមស្កេនឯកសារ SD ជាមុនសិន!";
                WMsg.ShowingMsg();
                txtBarcodeWireTerminal.Focus();
            }
        }

    }
}
