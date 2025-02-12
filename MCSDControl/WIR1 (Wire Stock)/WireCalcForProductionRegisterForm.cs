using MachineDeptApp.MsgClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Linq;

namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    public partial class WireCalcForProductionRegisterForm : Form
    {
        ErrorMsgClass EMsg = new ErrorMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        WireCalcForProduction fgrid;
        DataTable dtMstRM;
        string SeletedRMType;
        string SeletedMaker;

        string ErrorText;

        public WireCalcForProductionRegisterForm(WireCalcForProduction fg)
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.fgrid = fg;
            this.Shown += WireCalcForProductionRegisterForm_Shown;
            this.tabContrlType.SelectedIndexChanged += TabContrlType_SelectedIndexChanged;
            this.tabPageManual.Click += TabPageManual_Click;
            this.dgvInputted.RowsAdded += DgvInputted_RowsAdded;
            this.dgvInputted.RowsRemoved += DgvInputted_RowsRemoved;

            //New Register
            this.btnRegister.Click += BtnRegister_Click;
            this.rdbNewReg.CheckedChanged += RdbNewReg_CheckedChanged;
            this.CboBobbinW.KeyPress += CboBobbinW_KeyPress;
            this.CboBobbinW.TextChanged += CboBobbinW_TextChanged;
            this.CboBobbinW.Leave += CboBobbinW_Leave;

            this.txtTotalWNew.KeyPress += TxtTotalW_KeyPress;
            this.txtTotalWNew.Leave += TxtTotalW_Leave;
            this.txtTotalWNew.TextChanged += TxtTotalWNew_TextChanged;

            this.txtNetWNew.KeyPress += TxtNetWNew_KeyPress;
            this.txtNetWNew.Leave += TxtNetWNew_Leave;
            this.txtNetWNew.TextChanged += TxtNetWNew_TextChanged;

            this.txtQtyNew.KeyPress += TxtQty_KeyPress;
            this.txtQtyNew.Leave += TxtQty_Leave;
            this.txtQtyNew.TextChanged += TxtQtyNew_TextChanged;

            //Old Register
            this.txtTotalWOld.KeyPress += TxtTotalWOld_KeyPress;
            this.txtTotalWOld.Leave += TxtTotalWOld_Leave;
            this.txtTotalWOld.TextChanged += TxtTotalWOld_TextChanged;

            this.txtQtyOld.KeyPress += TxtQtyOld_KeyPress;
            this.txtQtyOld.Leave += TxtQtyOld_Leave;
            this.txtQtyOld.TextChanged += TxtQtyOld_TextChanged;

        }

        private void DgvInputted_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            CalcSummaryDetails();
        }
        private void DgvInputted_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            CalcSummaryDetails();
        }
        private void TxtQtyNew_TextChanged(object sender, EventArgs e)
        {
            CalcUnitPerGram();
        }
        private void CboBobbinW_Leave(object sender, EventArgs e)
        {
            if (CboBobbinW.Text.Trim() != "")
            {
                try
                {
                    double InputQty = Convert.ToDouble(CboBobbinW.Text.Trim());
                    InputQty = Convert.ToDouble(InputQty.ToString("N2"));
                    CboBobbinW.Text = InputQty.ToString();
                }
                catch
                {
                    WMsg.WarningText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!\nសូមបញ្ចូលតែចំនួនលេខប៉ុណ្ណោះ!";
                    WMsg.ShowingMsg();
                    CboBobbinW.Text = "";
                }
            }
        }
        private void CboBobbinW_TextChanged(object sender, EventArgs e)
        {
            if (rdbOldReg.Checked == true)
            {
                CalcUnitPerGram();
            }
        }
        private void TxtQtyOld_TextChanged(object sender, EventArgs e)
        {
            CalcUnitPerGram();
        }
        private void TxtTotalWOld_TextChanged(object sender, EventArgs e)
        {
            CalcUnitPerGram();
        }
        private void CboBobbinW_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the entered key is not a control key, digit or period
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // Check if the period is already present in the TextBox
            else if (e.KeyChar == '.' && (sender as ComboBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
        private void RdbNewReg_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbNewReg.Checked == true)
            {
                panelNewReg.Enabled = true;
                panelOldReg.Enabled = false;
                TakingBobbinW();
            }
            else
            {
                panelNewReg.Enabled = false;
                panelOldReg.Enabled = true;
                TakingBobbinW();

            }
            CalcUnitPerGram();
        }
        private void TxtTotalWNew_TextChanged(object sender, EventArgs e)
        {
            if (txtNetWNew.Text.Trim() != "" && txtTotalWNew.Text.Trim() != "")
            {
                try
                {
                    double NetW = Convert.ToDouble(txtNetWNew.Text.Trim());
                    double TotalW = Convert.ToDouble(txtTotalWNew.Text.Trim());
                    double BobbinW = TotalW - NetW;
                    BobbinW = Convert.ToDouble(BobbinW.ToString("N2"));
                    CboBobbinW.Items.Add(BobbinW.ToString());
                    CboBobbinW.Text = BobbinW.ToString();
                }
                catch
                {
                    CboBobbinW.Text = "";
                }
            }
            else
            {
                CboBobbinW.Text = "";
            }
            
        }
        private void TxtNetWNew_TextChanged(object sender, EventArgs e)
        {
            if (txtNetWNew.Text.Trim() != "" && txtTotalWNew.Text.Trim() != "")
            {
                try
                {
                    double NetW = Convert.ToDouble(txtNetWNew.Text.Trim());
                    double TotalW = Convert.ToDouble(txtTotalWNew.Text.Trim());
                    double BobbinW = TotalW - NetW;
                    BobbinW = Convert.ToDouble(BobbinW.ToString("N2"));
                    CboBobbinW.Items.Add(BobbinW.ToString());
                    CboBobbinW.Text = BobbinW.ToString();
                }
                catch
                {
                    CboBobbinW.Text = "";
                }
            }
            else
            {
                CboBobbinW.Text = "";
            }
            CalcUnitPerGram();
        }
        private void TxtTotalWOld_Leave(object sender, EventArgs e)
        {
            if (txtTotalWOld.Text.Trim() != "")
            {
                try
                {
                    double InputQty = Convert.ToDouble(txtTotalWOld.Text.Trim());
                    InputQty = Convert.ToDouble(InputQty.ToString("N2"));
                    if(InputQty>0)
                        txtTotalWOld.Text = InputQty.ToString();
                    else
                    {
                        WMsg.WarningText = "សូមបញ្ចូលតែចំនួនដែលធំជាង 0!";
                        WMsg.ShowingMsg();
                        txtTotalWOld.Text = "";
                    }
                }
                catch
                {
                    WMsg.WarningText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!\nសូមបញ្ចូលតែចំនួនលេខប៉ុណ្ណោះ!";
                    WMsg.ShowingMsg();
                    txtTotalWOld.Text = "";
                }
            }
        }
        private void TxtTotalWOld_KeyPress(object sender, KeyPressEventArgs e)
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
        private void TxtQtyOld_Leave(object sender, EventArgs e)
        {
            if (txtQtyOld.Text.Trim() != "")
            {
                try
                {
                    double InputQty = Convert.ToDouble(txtQtyOld.Text.Trim());
                    if (InputQty > 0)
                    {
                        txtQtyOld.Text = InputQty.ToString("N0");
                    }
                    else
                    {
                        WMsg.WarningText = "សូមបញ្ចូលតែចំនួនដែលធំជាង 0!";
                        WMsg.ShowingMsg();
                        txtQtyOld.Text = "";
                    }
                }
                catch
                {
                    WMsg.WarningText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!\nសូមបញ្ចូលតែចំនួនលេខប៉ុណ្ណោះ!";
                    WMsg.ShowingMsg();
                    txtQtyOld.Text = "";
                }
            }
        }
        private void TxtQtyOld_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void TxtNetWNew_Leave(object sender, EventArgs e)
        {
            if (txtNetWNew.Text.Trim() != "")
            {
                try
                {
                    double InputQty = Convert.ToDouble(txtNetWNew.Text.Trim());
                    InputQty = Convert.ToDouble(InputQty.ToString("N2"));
                    if (InputQty > 0)
                    {
                        txtNetWNew.Text = InputQty.ToString();
                    }
                    else
                    {
                        WMsg.WarningText = "សូមបញ្ចូលតែចំនួនដែលធំជាង 0!";
                        WMsg.ShowingMsg();
                        txtNetWNew.Text = "";
                    }
                }
                catch
                {
                    WMsg.WarningText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!\nសូមបញ្ចូលតែចំនួនលេខប៉ុណ្ណោះ!";
                    WMsg.ShowingMsg();
                    txtNetWNew.Text = "";
                }
            }
        }
        private void TxtNetWNew_KeyPress(object sender, KeyPressEventArgs e)
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
        private void TabPageManual_Click(object sender, EventArgs e)
        {
            this.tabPageManual.Focus();
        }
        private void TxtQty_Leave(object sender, EventArgs e)
        {
            if (txtQtyNew.Text.Trim() != "")
            {
                try
                {
                    double InputQty = Convert.ToDouble(txtQtyNew.Text.Trim());
                    if(InputQty>0)
                    {
                        txtQtyNew.Text = InputQty.ToString("N0");
                    }
                    else
                    {
                        WMsg.WarningText = "សូមបញ្ចូលតែចំនួនដែលធំជាង 0!";
                        WMsg.ShowingMsg();
                        txtQtyNew.Text = "";
                    }
                }
                catch
                {
                    WMsg.WarningText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!\nសូមបញ្ចូលតែចំនួនលេខប៉ុណ្ណោះ!";
                    WMsg.ShowingMsg();
                    txtQtyNew.Text = "";
                }
            }
        }
        private void TxtTotalW_Leave(object sender, EventArgs e)
        {
            if (txtTotalWNew.Text.Trim() != "")
            {
                try
                {
                    double InputQty = Convert.ToDouble(txtTotalWNew.Text.Trim());
                    InputQty = Convert.ToDouble(InputQty.ToString("N2"));
                    txtTotalWNew.Text = InputQty.ToString();
                }
                catch
                {
                    WMsg.WarningText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!\nសូមបញ្ចូលតែចំនួនលេខប៉ុណ្ណោះ!";
                    WMsg.ShowingMsg();
                    txtTotalWNew.Text = "";
                }
            }
        }
        private void TxtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void TxtTotalW_KeyPress(object sender, KeyPressEventArgs e)
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
        
        private void TabContrlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabContrlType.SelectedIndex == 0)
            {
                panelIput.BackColor = Color.FromArgb(255, 192, 128);
            }
            else
            {
                panelIput.BackColor = Color.FromArgb(128, 128, 255);
            }
        }
        private void WireCalcForProductionRegisterForm_Shown(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            ErrorText = "";
            SeletedMaker = "";
            dgvInputted.RowHeadersDefaultCellStyle.Font = new Font(dgvInputted.RowHeadersDefaultCellStyle.Font, FontStyle.Regular);

            //Taking Data 
            dtMstRM = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT ItemCode, ItemName, RMType, R2OrBobbinsW, R1OrNetW, MOQ, BobbinsOrReil FROM " +
                    "\n(SELECT * FROM tbMasterItem WHERE ItemType = 'Material') T1 " +
                    "\nLEFT JOIN (SELECT * FROM tbSDMstUncountMat) T2 ON T1.ItemCode = T2.Code " +
                    "\nWHERE ItemCode = '" + fgrid.dgvRMUsage.Rows[fgrid.dgvRMUsage.CurrentCell.RowIndex].Cells["RMCode"].Value.ToString() +"' ";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtMstRM);
            }
            catch (Exception ex)
            {
                ErrorText = "Taking Data : " + ex.Message;
            }
            cnn.con.Close();

            //Taking Maker from OBS
            if (ErrorText.Trim() == "")
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    string SQLQuery = "SELECT Resv1 FROM mstitem WHERE ItemType = 2 AND ItemCode = '"+ fgrid.dgvRMUsage.Rows[fgrid.dgvRMUsage.CurrentCell.RowIndex].Cells["RMCode"].Value.ToString() + "' ";
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    if (dt.Rows.Count > 0)
                        SeletedMaker = dt.Rows[0]["Resv1"].ToString();
                }
                catch (Exception ex)
                {
                    ErrorText = "OBS Maker : " + ex.Message;
                }
                cnnOBS.conOBS.Close();
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                string RMCode = dtMstRM.Rows[0]["ItemCode"].ToString();
                string RMName = dtMstRM.Rows[0]["ItemName"].ToString();
                string NetW = dtMstRM.Rows[0]["R1OrNetW"].ToString();
                string MOQ = dtMstRM.Rows[0]["MOQ"].ToString();
                SeletedRMType = dtMstRM.Rows[0]["RMType"].ToString();

                LbRMCodeBC.Text = RMCode;
                LbRMNameBC.Text = RMName;

                txtRMCode.Text = RMCode;
                txtRMName.Text = RMName;
                if(MOQ.Trim()!="")
                    txtQtyNew.Text = Convert.ToDouble(MOQ).ToString("N0");
                if(NetW.Trim()!="" && dtMstRM.Rows[0]["BobbinsOrReil"].ToString() == "Bobbin")
                    txtNetWNew.Text = NetW;

                dgvSummary.Rows.Add(0, 0, 0, WireCalcForProduction.SelectedQtyRequierement);
            }
            else
            {
                EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                EMsg.ShowingMsg();
            }
        }
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            CheckBeforeAdd();
            if (PicAlertBC.Visible == false && PicAlertBobbinW.Visible == false && PicAlertLNew.Visible == false && PicAlertNWNew.Visible == false
                && PicAlertWNew.Visible == false && PicAlertLOld.Visible == false && PicAlertWOld.Visible == false)
            {
                QMsg.QAText = "តើអ្នកចង់ Register មែនដែរឬទេ?";
                QMsg.UserClickedYes = false;
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    Cursor = Cursors.WaitCursor;
                    ErrorText = "";

                    DateTime RegDate = DateTime.Now;
                    //string RegBy = MenuFormV2.UserForNextForm;
                    string RegBy = "Rachhan";
                    double BobbinW = Convert.ToDouble(CboBobbinW.Text.Trim());
                    double PerUnit = Convert.ToDouble(txtUnitPerGram.Text.Trim()); double MOQ_W = 0;
                    double MOQ_L = 0;
                    double Remain_W = 0;
                    double Remain_L = 0;
                    string LocCode = "WIR1";
                    string BobbinSysNo = "WA-0000001";
                    if (SeletedRMType != "Wire")
                    {
                        BobbinSysNo = "TA-0000001";
                    }

                    //Taking Last SysNo
                    try
                    {
                        cnn.con.Open();
                        string SQLQuery = "SELECT MAX(BobbinSysNo) AS BobbinSysNo FROM tbMstRMRegister WHERE RMType = '"+SeletedRMType+"' ";
                        if (SeletedRMType != "Wire")
                        {
                            SQLQuery = "SELECT MAX(BobbinSysNo) AS BobbinSysNo FROM tbMstRMRegister WHERE NOT RMType = 'Wire' ";
                        }
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        if (dt.Rows.Count > 0 && dt.Rows[0]["BobbinSysNo"].ToString().Trim() != "")
                        {
                            string[] SplitText = dt.Rows[0]["BobbinSysNo"].ToString().Split('-');
                            string Prefix = SplitText[0].ToString();
                            int LastSysNo = Convert.ToInt32(SplitText[1].ToString());
                            BobbinSysNo = Prefix + "-" + (LastSysNo + 1).ToString("D7");
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorText = "Taking L.SysNo : " + ex.Message;
                    }
                    cnn.con.Close();

                    //Save to DB
                    if (ErrorText.Trim() == "")
                    {
                        if (rdbNewReg.Checked == true)
                        {
                            MOQ_W = Convert.ToDouble(txtTotalWNew.Text.Trim());
                            MOQ_L = Convert.ToDouble(txtQtyNew.Text.Trim());
                            Remain_W = MOQ_W;
                            Remain_L = MOQ_L;
                        }
                        else
                        {
                            Remain_W = Convert.ToDouble(txtTotalWOld.Text.Trim());
                            Remain_L = Convert.ToDouble(txtQtyOld.Text.Trim());
                        }

                        try
                        {
                            cnn.con.Open();
                            SqlCommand cmd = new SqlCommand("INSERT INTO tbMstRMRegister (BobbinSysNo, RMCode, RMType, BobbinW, MOQ_W, MOQ_L, Remain_W, Remain_L, Per_Unit, C_Location, Status, RegDate, RegBy, UpdateDate, UpdateBy) " +
                                                           "VALUES (@SysNo, @Rc, @Rt, @Bw, @MoqW, @MoqL, @RemW, @RemL, @PU, @Loc, @St, @RegD, @RegB, @UpD, @UpB)", cnn.con);

                            cmd.Parameters.AddWithValue("@SysNo", BobbinSysNo);
                            cmd.Parameters.AddWithValue("@Rc", txtRMCode.Text);
                            cmd.Parameters.AddWithValue("@Rt", SeletedRMType);
                            cmd.Parameters.AddWithValue("@Bw", BobbinW);
                            cmd.Parameters.AddWithValue("@MoqW", MOQ_W);
                            cmd.Parameters.AddWithValue("@MoqL", MOQ_L);
                            cmd.Parameters.AddWithValue("@RemW", Remain_W);
                            cmd.Parameters.AddWithValue("@RemL", Remain_L);
                            cmd.Parameters.AddWithValue("@PU", PerUnit);
                            cmd.Parameters.AddWithValue("@Loc", LocCode);
                            cmd.Parameters.AddWithValue("@St", "Active");
                            cmd.Parameters.AddWithValue("@RegD", RegDate);
                            cmd.Parameters.AddWithValue("@RegB", RegBy);
                            cmd.Parameters.AddWithValue("@UpD", RegDate);
                            cmd.Parameters.AddWithValue("@UpB", RegBy);
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            ErrorText = "Save to DB : " + ex.Message;
                        }
                        cnn.con.Close();
                    }

                    //Print Excel Out 
                    if (ErrorText.Trim() == "")
                    {
                        string fName = "";
                        string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\RM Register";
                        //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                        if (!Directory.Exists(SavePath))
                        {
                            Directory.CreateDirectory(SavePath);
                        }

                        Excel.Application excelApp = new Excel.Application();
                        Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: Environment.CurrentDirectory.ToString() + @"\Template\RMRegisterTagTemplate.xlsx", Editable: true);
                        try
                        {
                            //Write to 
                            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["RachhanSystem"];
                            //Header
                            worksheet.Cells[1,1] = "*"+BobbinSysNo+"*";
                            worksheet.Cells[2, 3] = txtRMCode.Text;
                            worksheet.Cells[3, 3] = txtRMName.Text;
                            worksheet.Cells[4, 3] = SeletedMaker;
                            worksheet.Cells[5, 3] = BobbinW;
                            worksheet.Cells[6, 3] = Remain_W;
                            worksheet.Cells[7, 3] = Remain_L;
                            worksheet.Cells[8, 3] = RegDate;

                            //Print
                            worksheet.PrintOut();

                            // Saving the modified Excel file                 
                            string file = txtRMCode.Text + " - " + BobbinSysNo;
                            fName = file + " ( " + RegDate.ToString("dd-MM-yyyy HH_mm_ss") + " )";
                            worksheet.SaveAs(SavePath + @"\" + fName + ".xlsx");
                        }
                        catch (Exception ex)
                        {
                            ErrorText = "Print Excel : " + ex.Message;
                        }

                        excelApp.DisplayAlerts = false;
                        xlWorkBook.Close();
                        excelApp.DisplayAlerts = true;
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

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() =="")
                    {
                        dgvInputted.Rows.Add();
                        dgvInputted.Rows[dgvInputted.Rows.Count - 1].HeaderCell.Value = dgvInputted.Rows.Count.ToString();
                        dgvInputted.Rows[dgvInputted.Rows.Count-1].Cells["BobbinCodeInputted"].Value = BobbinSysNo;
                        dgvInputted.Rows[dgvInputted.Rows.Count - 1].Cells["WeightInputted"].Value = Remain_W;
                        dgvInputted.Rows[dgvInputted.Rows.Count - 1].Cells["LengthInputted"].Value = Remain_L;
                        ClearAllText();
                    }
                    else
                    {
                        EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                        EMsg.ShowingMsg();
                    }
                }
            }
        }

        //Method
        private async void CheckBeforeAdd()
        {
            PicAlertBC.Visible = false;

            PicAlertBobbinW.Visible = false;
            PicAlertLNew.Visible = false;
            PicAlertNWNew.Visible = false;
            PicAlertWNew.Visible = false;

            PicAlertLOld.Visible = false;
            PicAlertWOld.Visible = false;
            var tasksBlink = new List<Task>();


            if (txtUnitPerGram.Text.Trim() == "")
            {
                if (rdbNewReg.Checked == true)
                {
                    if (txtQtyNew.Text.Trim() == "")
                        tasksBlink.Add(BlinkPictureBox(PicAlertLNew));

                    if (txtNetWNew.Text.Trim() == "")
                        tasksBlink.Add(BlinkPictureBox(PicAlertNWNew));
                }
                else
                {
                    if (txtTotalWOld.Text.Trim() != "" && txtQtyOld.Text.Trim() != "" && CboBobbinW.Text.Trim() != "")
                    {
                        if (Convert.ToDouble(CboBobbinW.Text.Trim()) >= Convert.ToDouble(txtTotalWOld.Text.Trim()))
                        {
                            tasksBlink.Add(BlinkPictureBox(PicAlertWOld));
                            tasksBlink.Add(BlinkPictureBox(PicAlertBobbinW));
                        }
                    }
                    else 
                    {
                        if (txtTotalWOld.Text.Trim() == "")
                            tasksBlink.Add(BlinkPictureBox(PicAlertWOld));

                        if (txtQtyOld.Text.Trim() == "")
                            tasksBlink.Add(BlinkPictureBox(PicAlertLOld));

                        if (CboBobbinW.Text.Trim() == "")
                            tasksBlink.Add(BlinkPictureBox(PicAlertBobbinW));
                    }
                }
            }
            else
            {
                if (Convert.ToDouble(txtUnitPerGram.Text.Trim()) > 0)
                {
                    if (rdbNewReg.Checked == true)
                    {
                        if (CboBobbinW.Text.Trim() == "")
                        {
                            tasksBlink.Add(BlinkPictureBox(PicAlertWNew));
                        }
                        else
                        {
                            if (Convert.ToDouble(CboBobbinW.Text.Trim()) < 0)
                            {
                                tasksBlink.Add(BlinkPictureBox(PicAlertNWNew));

                                tasksBlink.Add(BlinkPictureBox(PicAlertWNew));
                            }
                        }
                    }
                }
                else
                {
                    if (rdbOldReg.Checked == true)
                    {
                        if (Convert.ToDouble(CboBobbinW.Text.Trim()) >= Convert.ToDouble(txtTotalWOld.Text.Trim()))
                        {
                            tasksBlink.Add(BlinkPictureBox(PicAlertWOld));
                            tasksBlink.Add(BlinkPictureBox(PicAlertBobbinW));
                        }
                    }
                }
            }
            await Task.WhenAll(tasksBlink);
        }
        private async Task BlinkPictureBox(PictureBox pictureBox)
        {
            pictureBox.Visible = false;
            for (int i = 0; i < 7; i++)
            {
                pictureBox.Visible = !pictureBox.Visible;
                await Task.Delay(350);
            }
            pictureBox.Visible = true;
        }        
        private void ClearAllText()
        {
            if (tabContrlType.SelectedIndex == 1)
            {
                if (rdbNewReg.Checked == true)
                {
                    txtTotalWNew.Text = "";
                    txtTotalWNew.Focus();
                }
                else
                {
                    txtQtyOld.Text = "";
                    txtTotalWOld.Text = "";
                    txtQtyOld.Focus();
                }
                CalcUnitPerGram();
            }
        }
        private void TakingBobbinW()
        {
            CboBobbinW.Items.Clear();
            if (rdbNewReg.Checked == true)
            {
                if (txtNetWNew.Text.Trim() != "" && txtTotalWNew.Text.Trim() != "")
                {
                    try
                    {
                        double NetW = Convert.ToDouble(txtNetWNew.Text.Trim());
                        double TotalW = Convert.ToDouble(txtTotalWNew.Text.Trim());
                        double BobbinW = TotalW - NetW;
                        CboBobbinW.Items.Add(BobbinW.ToString());
                        CboBobbinW.Text = BobbinW.ToString();
                    }
                    catch
                    {
                        CboBobbinW.Text = "";
                    }
                }
                else
                {
                    CboBobbinW.Text = "";
                }
                CboBobbinW.Enabled = false;
            }
            else
            {
                if (SeletedRMType.Trim() != "")
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        cnn.con.Open();
                        string SQLQuery = "SELECT * FROM tbSDMstBobbinsWeight WHERE RMType='" + SeletedRMType + "' " +
                                                    "\nORDER BY BobbinsW ASC";
                        //Console.WriteLine(SQLQuery);
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        sda.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        ErrorText = ex.Message;
                    }
                    cnn.con.Close();

                    CboBobbinW.Items.Clear();
                    if (ErrorText.Trim() == "")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            CboBobbinW.Items.Add(row["BobbinsW"]);
                        }
                    }
                }
                CboBobbinW.Enabled = true;
            }
        }
        private void CalcUnitPerGram()
        {
            if (rdbNewReg.Checked == true)
            {
                if (txtQtyNew.Text.Trim() != "" && txtNetWNew.Text.Trim() != "")
                {
                    try
                    {
                        double MOQ = Convert.ToDouble(txtQtyNew.Text.Trim());
                        double NetW = Convert.ToDouble(txtNetWNew.Text.Trim());
                        double TotalQty = MOQ / NetW / 1000;
                        TotalQty = Convert.ToDouble(TotalQty.ToString("N4"));
                        if(MOQ!=0 && NetW!=0)
                        {
                            txtUnitPerGram.Text = TotalQty.ToString();
                        }
                        else
                        {
                            txtUnitPerGram.Text = "";
                        }
                    }
                    catch
                    {
                        txtUnitPerGram.Text = "";
                    }
                }
                else
                {
                    txtUnitPerGram.Text = "";
                }
            }
            else
            {
                if (CboBobbinW.Text.Trim() != "" && txtQtyOld.Text.Trim() != "" && txtTotalWOld.Text.Trim()!="")
                {
                    try
                    {
                        double BobbinW = Convert.ToDouble(CboBobbinW.Text.Trim());
                        double RemainW = Convert.ToDouble(txtTotalWOld.Text.Trim());
                        double RemainL = Convert.ToDouble(txtQtyOld.Text.Trim());
                        double TotalQty = RemainL / (RemainW - BobbinW)/1000;
                        TotalQty = Convert.ToDouble(TotalQty.ToString("N4"));
                        if(RemainW!=BobbinW)
                        {
                            txtUnitPerGram.Text = TotalQty.ToString();
                        }
                        else
                        {
                            txtUnitPerGram.Text = "";
                        }
                    }
                    catch
                    {
                        txtUnitPerGram.Text = "";
                    }                    
                }
                else
                {
                    txtUnitPerGram.Text = "";
                }
            }

        }
        private void CalcSummaryDetails()
        {
            int BobbinQty = dgvInputted.Rows.Count;
            double TotalW = 0;
            double TotalL = 0;
            foreach (DataGridViewRow row in dgvInputted.Rows)
            {
                TotalW += Convert.ToDouble(row.Cells["WeightInputted"].Value);
                TotalL += Convert.ToDouble(row.Cells["LengthInputted"].Value);
            }
            dgvSummary.Rows[0].Cells["TotalBobbinQty"].Value = BobbinQty;
            dgvSummary.Rows[0].Cells["TotalWeight"].Value = TotalW;
            dgvSummary.Rows[0].Cells["TotalLength"].Value = TotalL;
        }

    }
}
