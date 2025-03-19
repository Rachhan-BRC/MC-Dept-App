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
        public DataTable dtAvailableBobbin;
        string SeletedRMType;
        string SeletedMaker;

        string ErrorText;

        public WireCalcForProductionRegisterForm(WireCalcForProduction fg)
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.FormClosed += WireCalcForProductionRegisterForm_FormClosed;
            this.fgrid = fg;
            this.btnNew.Click += BtnNew_Click;
            this.btnOK.Click += BtnOK_Click;
            this.Shown += WireCalcForProductionRegisterForm_Shown;
            this.tabContrlType.SelectedIndexChanged += TabContrlType_SelectedIndexChanged;
            this.tabPageManual.Click += TabPageManual_Click;
            this.dgvInputted.RowsRemoved += DgvInputted_RowsRemoved;
            this.dgvInputted.CellClick += DgvInputted_CellClick;
            //this.dgvAvailable.CellClick += DgvAvailable_CellClick;
            this.dgvAvailable.RowsAdded += DgvAvailable_RowsAdded;
            this.dgvAvailable.RowsRemoved += DgvAvailable_RowsRemoved;

            //BC
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;
         
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

            //Terminal Register
            this.txtQtyTM.KeyPress += TxtQtyTM_KeyPress;
            this.txtQtyTM.Leave += TxtQtyTM_Leave;

        }

        private void WireCalcForProductionRegisterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            double TotalQty=0;
            foreach (DataRow row in fgrid.dtSeletedBobbin.Rows)
            {
                if (LbRMCodeBC.Text == row["RMCode"].ToString())
                {
                    TotalQty += Convert.ToDouble(row["Qty"]);
                }
            }
            fgrid.dgvRMUsage.Rows[fgrid.dgvRMUsage.CurrentCell.RowIndex].Cells["TransferQty"].Value = TotalQty;
        }
        private void TxtQtyTM_Leave(object sender, EventArgs e)
        {
            if (txtQtyTM.Text.Trim() != "")
            {
                try
                {
                    double InputQty = Convert.ToDouble(txtQtyTM.Text.Trim());
                    if (InputQty > 0)
                    {
                        txtQtyTM.Text = InputQty.ToString("N0");
                    }
                    else
                    {
                        WMsg.WarningText = "សូមបញ្ចូលតែចំនួនដែលធំជាង 0!";
                        WMsg.ShowingMsg();
                        txtQtyTM.Text = "";
                    }
                }
                catch
                {
                    WMsg.WarningText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!\nសូមបញ្ចូលតែចំនួនលេខប៉ុណ្ណោះ!";
                    WMsg.ShowingMsg();
                    txtQtyTM.Text = "";
                }
            }
        }
        private void TxtQtyTM_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (Convert.ToDouble(dgvSummary.Rows[0].Cells["TotalLength"].Value) >= Convert.ToDouble(dgvSummary.Rows[0].Cells["NeedQty"].Value))
            {
                Cursor = Cursors.WaitCursor;
                ErrorText = "";

                string RMCode = LbRMCodeBC.Text;

                //Delete current SelectedBobbins
                for (int i = fgrid.dtSeletedBobbin.Rows.Count - 1; i > -1; i--)
                {
                    if (RMCode == fgrid.dtSeletedBobbin.Rows[i]["RMCode"].ToString())
                    {
                        fgrid.dtSeletedBobbin.Rows.RemoveAt(i);
                        fgrid.dtSeletedBobbin.AcceptChanges();
                    }
                }

                //Add to SelectedBobbins
                try
                {
                    foreach (DataGridViewRow row in dgvInputted.Rows)
                    {
                        double Weigth = Convert.ToDouble(row.Cells["WeightInputted"].Value);
                        double Qty = Convert.ToDouble(row.Cells["LengthInputted"].Value);
                        string BobbinCode = row.Cells["BobbinCodeInputted"].Value.ToString();
                        fgrid.dtSeletedBobbin.Rows.Add(RMCode, BobbinCode, Weigth, Qty);
                    }
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                }

                Cursor = Cursors.Default;

                if (ErrorText.Trim() == "")
                {                    
                    this.Close();
                }
                else
                {
                    EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                    EMsg.ShowingMsg();
                }
            }
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            //Delete From dtSelectedBobbin
            for (int i = fgrid.dtSeletedBobbin.Rows.Count - 1; i > -1; i--)
            {
                if (fgrid.dtSeletedBobbin.Rows[i]["RMCode"].ToString() == LbRMCodeBC.Text)
                {
                    fgrid.dtSeletedBobbin.Rows.RemoveAt(i);
                }
            }
            fgrid.dtSeletedBobbin.AcceptChanges();
            
            dtAvailableBobbin = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT ItemCode, ItemName, RMType, R2OrBobbinsW, R1OrNetW, MOQ, BobbinsOrReil FROM " +
                    "\n(SELECT * FROM tbMasterItem WHERE ItemType = 'Material') T1 " +
                    "\nLEFT JOIN (SELECT * FROM tbSDMstUncountMat) T2 ON T1.ItemCode = T2.Code " +
                    "\nWHERE ItemCode = '" + fgrid.dgvRMUsage.Rows[fgrid.dgvRMUsage.CurrentCell.RowIndex].Cells["RMCode"].Value.ToString() + "' ";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtMstRM);

                SQLQuery = "SELECT RMCode, BobbinSysNo, Remain_W, Remain_L FROM tbMstRMRegister " +
                    "\nWHERE C_Location='WIR1' AND Remain_L>0 AND RMCode='" + fgrid.dgvRMUsage.Rows[fgrid.dgvRMUsage.CurrentCell.RowIndex].Cells["RMCode"].Value.ToString() + "' " +
                    "\nORDER BY Remain_L ASC";
                sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtAvailableBobbin);
            }
            catch (Exception ex)
            {
                ErrorText = "Taking Data : " + ex.Message;
            }
            cnn.con.Close();
            
            SortingAvailableBobbins();
            dgvInputted.Rows.Clear();

            Cursor = Cursors.Default;

            if (ErrorText.Trim()!= "")
            {
                EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                EMsg.ShowingMsg();
            }
        }
        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.Trim() != "")
                {
                    CheckBeforeAdd();
                    if (PicAlertBC.Visible == false)
                    {
                        string BobbinCode = txtBarcode.Text;
                        double W = 0;
                        double Qty = 0;
                        foreach (DataRow row in dtAvailableBobbin.Rows)
                        {
                            if (row["BobbinSysNo"].ToString() == BobbinCode)
                            {
                                W = Convert.ToDouble(row["Remain_W"]);
                                Qty = Convert.ToDouble(row["Remain_L"]);
                                dtAvailableBobbin.Rows.Remove(row);
                                dtAvailableBobbin.AcceptChanges();
                                break;
                            }
                        }

                        dgvInputted.Rows.Add();
                        dgvInputted.Rows[dgvInputted.Rows.Count - 1].HeaderCell.Value = dgvInputted.Rows.Count.ToString();
                        dgvInputted.Rows[dgvInputted.Rows.Count - 1].Cells["BobbinCodeInputted"].Value = BobbinCode;
                        dgvInputted.Rows[dgvInputted.Rows.Count - 1].Cells["WeightInputted"].Value = W;
                        dgvInputted.Rows[dgvInputted.Rows.Count - 1].Cells["LengthInputted"].Value = Qty;
                        CalcSummaryDetails();
                        dgvInputted.ClearSelection();
                        dgvAvailable.ClearSelection();
                        txtBarcode.Text = "";
                        txtBarcode.Focus();
                    }
                    else
                    {
                        txtBarcode.Focus();
                        txtBarcode.SelectAll();
                    }
                }
            }
        }
        private void DgvAvailable_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            foreach (DataGridViewRow row in dgvAvailable.Rows)
            {
                row.HeaderCell.Value = (row.Index+1).ToString();
            }
        }
        private void DgvAvailable_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            foreach (DataGridViewRow row in dgvAvailable.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString();
            }
        }
        private void DgvAvailable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex>-1)
            {
                string BobbinCode = dgvAvailable.Rows[e.RowIndex].Cells["BobbinCodeAvailable"].Value.ToString();
                double W = Convert.ToDouble(dgvAvailable.Rows[e.RowIndex].Cells["WeightAvailable"].Value);
                double Qty = Convert.ToDouble(dgvAvailable.Rows[e.RowIndex].Cells["QtyAvailable"].Value);

                foreach (DataRow row in dtAvailableBobbin.Rows)
                {
                    if (row["BobbinSysNo"].ToString() == BobbinCode)
                    {
                        dtAvailableBobbin.Rows.Remove(row);
                        dtAvailableBobbin.AcceptChanges();
                        break;
                    }
                }

                dgvInputted.Rows.Add();
                dgvInputted.Rows[dgvInputted.Rows.Count - 1].HeaderCell.Value = dgvInputted.Rows.Count.ToString();
                dgvInputted.Rows[dgvInputted.Rows.Count - 1].Cells["BobbinCodeInputted"].Value = BobbinCode;
                dgvInputted.Rows[dgvInputted.Rows.Count - 1].Cells["WeightInputted"].Value = W;
                dgvInputted.Rows[dgvInputted.Rows.Count - 1].Cells["LengthInputted"].Value = Qty;
                CalcSummaryDetails();
                dgvInputted.ClearSelection();
                dgvAvailable.ClearSelection();
            }
        }
        private void DgvInputted_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvInputted.Columns[e.ColumnIndex].Name == "Remove")
            {
                if (e.RowIndex > -1)
                {
                    WireCalcForProductionRegisterDeleteForm Wcfprdf = new WireCalcForProductionRegisterDeleteForm(this);
                    Wcfprdf.ShowDialog();
                }
            }
        }
        private void DgvInputted_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            SortingAvailableBobbins();
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
                    if (InputQty < 2)
                    {
                        InputQty = Convert.ToDouble(InputQty.ToString("N2"));
                        CboBobbinW.Text = InputQty.ToString();
                    }
                    else
                    {
                        WMsg.WarningText = "ទម្ងន់ខុសប្រក្រតី!";
                        WMsg.ShowingMsg();
                        CboBobbinW.Text = "";
                    }
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
            CboBobbinW.Text = "";
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
                    if (InputQty > 0)
                    {
                        int UserAccepted = 0;
                        if (InputQty >= 20)
                        {
                            QMsg.QAText = "ទម្ងន់ខុសប្រក្រតី! តើអ្នកចង់បន្ដទៀតឬទេ?";
                            QMsg.MsgIcon = MessageBoxIcon.Exclamation;
                            QMsg.UserClickedYes = false;
                            QMsg.ShowingMsg();
                            if (QMsg.UserClickedYes == true)
                            {
                                UserAccepted++;
                            }
                            QMsg.MsgIcon = MessageBoxIcon.Question;
                        }
                        else
                        {
                            UserAccepted++;
                        }

                        if (UserAccepted > 0)
                        {
                            txtTotalWOld.Text = InputQty.ToString();
                        }
                        else
                        {
                            txtTotalWOld.Text = "";
                        }
                    }
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
                        int UserAccepted = 0;
                        if (InputQty >= 20)
                        {
                            QMsg.QAText = "ទម្ងន់ខុសប្រក្រតី! តើអ្នកចង់បន្ដទៀតឬទេ?";
                            QMsg.MsgIcon = MessageBoxIcon.Exclamation;
                            QMsg.UserClickedYes = false;
                            QMsg.ShowingMsg();
                            if (QMsg.UserClickedYes == true)
                            {
                                UserAccepted++;
                            }
                            QMsg.MsgIcon = MessageBoxIcon.Question;
                        }
                        else
                        {
                            UserAccepted++;
                        }

                        if (UserAccepted > 0)
                        {
                            txtNetWNew.Text = InputQty.ToString();
                        }
                        else
                        {
                            txtNetWNew.Text = "";
                        }
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
                    int UserAccepted = 0;
                    if (InputQty >= 20)
                    {
                        QMsg.QAText = "ទម្ងន់ខុសប្រក្រតី! តើអ្នកចង់បន្ដទៀតឬទេ?";
                        QMsg.MsgIcon = MessageBoxIcon.Exclamation;
                        QMsg.UserClickedYes = false;
                        QMsg.ShowingMsg();
                        if (QMsg.UserClickedYes == true)
                        {
                            UserAccepted++;
                        }
                        QMsg.MsgIcon = MessageBoxIcon.Question;
                    }
                    else
                    {
                        UserAccepted++;
                    }

                    if (UserAccepted > 0)
                    {
                        txtTotalWNew.Text = InputQty.ToString();
                    }
                    else
                    {
                        txtTotalWNew.Text = "";
                    }
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
            dgvAvailable.RowHeadersDefaultCellStyle.Font = new Font(dgvAvailable.RowHeadersDefaultCellStyle.Font, FontStyle.Regular);
            dgvInputted.RowHeadersDefaultCellStyle.Font = new Font(dgvInputted.RowHeadersDefaultCellStyle.Font, FontStyle.Regular);
            foreach (DataGridViewColumn col in dgvAvailable.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn col in dgvInputted.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //Taking Data 
            dtMstRM = new DataTable();
            dtAvailableBobbin = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT ItemCode, ItemName, RMType, R2OrBobbinsW, R1OrNetW, MOQ, BobbinsOrReil FROM " +
                    "\n(SELECT * FROM tbMasterItem WHERE ItemType = 'Material') T1 " +
                    "\nLEFT JOIN (SELECT * FROM tbSDMstUncountMat) T2 ON T1.ItemCode = T2.Code " +
                    "\nWHERE ItemCode = '" + fgrid.dgvRMUsage.Rows[fgrid.dgvRMUsage.CurrentCell.RowIndex].Cells["RMCode"].Value.ToString() +"' ";
                //Console.WriteLine(SQLQuery);
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtMstRM);

                SQLQuery = "SELECT RMCode, BobbinSysNo, Remain_W, Remain_L FROM tbMstRMRegister " +
                    "\nWHERE C_Location='WIR1' AND Remain_L>0 AND RMCode='"+ fgrid.dgvRMUsage.Rows[fgrid.dgvRMUsage.CurrentCell.RowIndex].Cells["RMCode"].Value.ToString() + "' " +
                    "\nORDER BY Remain_L ASC";
                //Console.WriteLine(SQLQuery );
                sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtAvailableBobbin);
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

                SortingAvailableBobbins();
                
                LbRMCodeBC.Text = RMCode;
                LbRMNameBC.Text = RMName;

                txtRMCode.Text = RMCode;
                txtRMName.Text = RMName;
                if (SeletedRMType == "Wire")
                {
                    if (MOQ.Trim() != "")
                        txtQtyNew.Text = Convert.ToDouble(MOQ).ToString("N0");
                    panelRegWire.BringToFront();
                    panelRegOther.SendToBack();
                }
                else
                {
                    if (MOQ.Trim() != "")
                        txtQtyTM.Text = Convert.ToDouble(MOQ).ToString("N0");
                    panelRegWire.SendToBack();
                    panelRegOther.BringToFront();
                }
                
                dgvSummary.Rows.Add(0, 0, 0, WireCalcForProduction.SelectedQtyRequierement);
                CalcSummaryDetails();
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
                && PicAlertWNew.Visible == false && PicAlertLOld.Visible == false && PicAlertWOld.Visible == false && PicAlertQtyTM.Visible == false)
            {
                QMsg.QAText = "តើអ្នកចង់ Register មែនដែរឬទេ?";
                QMsg.UserClickedYes = false;
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    Cursor = Cursors.WaitCursor;
                    ErrorText = "";

                    DateTime RegDate = DateTime.Now;
                    string RegBy = MenuFormV2.UserForNextForm;
                    //string RegBy = "Rachhan";
                    double BobbinW = 0;
                    double PerUnit = 0; 
                    double MOQ_W = 0;
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
                            if (LastSysNo == 9999999)
                            {
                                Console.WriteLine(Prefix[1].ToString() + "\t" +LastSysNo);
                                int CurrentASCiiCode = (int)Prefix[1];
                                if (Prefix[1].ToString() != "Z")
                                {
                                    CurrentASCiiCode++;
                                    Console.WriteLine(CurrentASCiiCode + "\t" +(Convert.ToChar(CurrentASCiiCode)).ToString());
                                    Prefix = Prefix[0].ToString() + (Convert.ToChar(CurrentASCiiCode)).ToString();
                                    LastSysNo = 0;
                                }
                                else 
                                {
                                    ErrorText = "You're reach the Maximum Number : WZ-9999999 !";
                                }
                            }
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
                        if (SeletedRMType == "Wire")
                        {
                            BobbinW = Convert.ToDouble(CboBobbinW.Text.Trim());
                            PerUnit = Convert.ToDouble(txtUnitPerGram.Text.Trim());
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

                        }
                        else
                        {
                            Remain_L = Convert.ToDouble(txtQtyTM.Text.Trim());
                            if (rdbNewRegTM.Checked == true)
                            {
                                MOQ_L = Remain_L;
                            }
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
                            //Excel.Worksheet WS = (Excel.Worksheet)xlWorkBook.Sheets["RachhanSystem"];
                            Excel.Worksheet WS = new Excel.Worksheet();
                            Excel.Worksheet DeletedWS = new Excel.Worksheet();

                            //Assign WS 
                            if (SeletedRMType == "Wire")
                            {
                                WS = (Excel.Worksheet)xlWorkBook.Sheets["Wire"];
                                DeletedWS = (Excel.Worksheet)xlWorkBook.Sheets["Terminal"];
                            }
                            else
                            {
                                WS = (Excel.Worksheet)xlWorkBook.Sheets["Terminal"];
                                DeletedWS = (Excel.Worksheet)xlWorkBook.Sheets["Wire"];
                            }

                            //Rename Sheet
                            WS.Name = "RachhanSystem";

                            //Write Data
                            //Header
                            WS.Cells[1,1] = "*"+BobbinSysNo+"*";
                            WS.Cells[3, 4] = txtRMName.Text;
                            WS.Cells[3, 2] = txtRMCode.Text;

                            if (SeletedRMType == "Wire")
                            {
                                if (MOQ_W != 0)
                                {
                                    WS.Cells[4, 2] = MOQ_W;
                                }
                                else
                                {
                                    WS.Cells[4, 2] = Remain_W;
                                }
                                //WS.Cells[4, 2] = MOQ_W;
                                WS.Cells[4, 6] = BobbinW;
                                if (MOQ_L != 0)
                                {
                                    WS.Cells[5, 2] = MOQ_L;
                                }
                                else
                                {
                                    WS.Cells[5, 2] = Remain_L;
                                }
                                //WS.Cells[5, 2] = MOQ_L;
                                WS.Cells[5, 6] = PerUnit;
                                WS.Cells[8, 3] = Remain_W;
                                WS.Cells[8, 1] = RegDate;
                                WS.Cells[8, 2] = Remain_L;
                            }
                            else
                            {
                                if (rdbNewRegTM.Checked == true)
                                    WS.Cells[4, 2] = MOQ_L;
                                WS.Cells[7, 1] = RegDate;
                                WS.Cells[7, 2] = Remain_L;
                            }

                            //Delete Sheet 
                            excelApp.DisplayAlerts = false;
                            DeletedWS.Delete();
                            excelApp.DisplayAlerts = true;

                            //Print
                            WS.PrintOut();

                            // Saving the modified Excel file                 
                            string file = txtRMCode.Text + " - " + BobbinSysNo;
                            fName = file + " ( " + RegDate.ToString("dd-MM-yyyy HH_mm_ss") + " )";
                            WS.SaveAs(SavePath + @"\" + fName + ".xlsx");
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
                        dgvInputted.Rows[dgvInputted.Rows.Count - 1].Cells["BobbinCodeInputted"].Value = BobbinSysNo;
                        dgvInputted.Rows[dgvInputted.Rows.Count - 1].Cells["WeightInputted"].Value = Remain_W;
                        dgvInputted.Rows[dgvInputted.Rows.Count - 1].Cells["LengthInputted"].Value = Remain_L;
                        ClearAllText();
                        CalcSummaryDetails();
                        dgvInputted.ClearSelection();
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

            PicAlertQtyTM.Visible = false;

            var tasksBlink = new List<Task>();

            if (tabContrlType.SelectedIndex==0)
            {
                int Found = 0;
                string BC = txtBarcode.Text;
                foreach (DataGridViewRow row in dgvAvailable.Rows)
                {
                    if (BC == row.Cells["BobbinCodeAvailable"].Value.ToString())
                    {
                        Found++;
                        break;
                    }
                }

                if (Found == 0)
                {
                    tasksBlink.Add(BlinkPictureBox(PicAlertBC));
                }
            }
            else
            {
                if (SeletedRMType == "Wire")
                {
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

                }
                else
                    if(txtQtyTM.Text.Trim()=="")
                        tasksBlink.Add(BlinkPictureBox(PicAlertQtyTM));
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
                if (SeletedRMType == "Wire")
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
                }
                else
                {
                    if (rdbOldRegTM.Checked == true)
                        txtQtyTM.Text = "";
                    txtQtyTM.Focus();
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
                //Console.WriteLine(row.Cells["WeightInputted"].Value.ToString());
                TotalW += Convert.ToDouble(row.Cells["WeightInputted"].Value.ToString());
                TotalL += Convert.ToDouble(row.Cells["LengthInputted"].Value.ToString());
            }
            dgvSummary.Rows[0].Cells["TotalBobbinQty"].Value = BobbinQty;
            dgvSummary.Rows[0].Cells["TotalWeight"].Value = TotalW;
            dgvSummary.Rows[0].Cells["TotalLength"].Value = TotalL;

            //Check for enable BtnOK
            if (TotalL >= Convert.ToDouble(dgvSummary.Rows[0].Cells["NeedQty"].Value))
            {
                btnOK.Enabled = true;
                btnOKGRAY.SendToBack();
            }
            else
            {
                btnOK.Enabled = false;
                btnOKGRAY.BringToFront();
            }
        }
        private void SortingAvailableBobbins()
        {
            DataView view = new DataView(dtAvailableBobbin);
            view.Sort = "Remain_L ASC";
            DataTable dtSorted = view.ToTable();
            dtAvailableBobbin = new DataTable();
            dtAvailableBobbin = dtSorted.Copy();
            dtAvailableBobbin.AcceptChanges();

            //Add from dtSeletedBobbin
            foreach (DataRow row in fgrid.dtSeletedBobbin.Rows)
            {
                foreach (DataRow rowAB in dtAvailableBobbin.Rows)
                {
                    string BobbinCode = rowAB["BobbinSysNo"].ToString();
                    double W = Convert.ToDouble(rowAB["Remain_W"].ToString());
                    double Qty = Convert.ToDouble(rowAB["Remain_L"].ToString());

                    if (row["RMCode"].ToString() == rowAB["RMCode"].ToString() && row["BobbinCode"].ToString() == BobbinCode)
                    {
                        dgvInputted.Rows.Add();
                        dgvInputted.Rows[dgvInputted.Rows.Count - 1].HeaderCell.Value = dgvInputted.Rows.Count.ToString();
                        dgvInputted.Rows[dgvInputted.Rows.Count - 1].Cells["BobbinCodeInputted"].Value = BobbinCode;
                        dgvInputted.Rows[dgvInputted.Rows.Count - 1].Cells["WeightInputted"].Value = W;
                        dgvInputted.Rows[dgvInputted.Rows.Count - 1].Cells["LengthInputted"].Value = Qty;
                        dgvInputted.ClearSelection();
                        dtAvailableBobbin.Rows.Remove(rowAB);
                        break;
                    }
                }
            }
            dtAvailableBobbin.AcceptChanges();

            dgvAvailable.AutoGenerateColumns = false; // Keep your predefined columns
            dgvAvailable.Columns["BobbinCodeAvailable"].DataPropertyName = "BobbinSysNo";
            dgvAvailable.Columns["WeightAvailable"].DataPropertyName = "Remain_W";
            dgvAvailable.Columns["QtyAvailable"].DataPropertyName = "Remain_L";
            dgvAvailable.DataSource = dtAvailableBobbin;
            dgvAvailable.ClearSelection();
        }
        
    }
}
