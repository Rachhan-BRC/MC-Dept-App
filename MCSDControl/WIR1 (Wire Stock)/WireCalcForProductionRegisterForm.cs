using MachineDeptApp.MsgClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    public partial class WireCalcForProductionRegisterForm : Form
    {
        ErrorMsgClass EMsg = new ErrorMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        SQLConnect cnn = new SQLConnect(); 
        WireCalcForProduction fgrid;
        DataTable dtMstRM;
        string SeletedRMType;

        string ErrorText;

        public WireCalcForProductionRegisterForm(WireCalcForProduction fg)
        {
            InitializeComponent();
            this.cnn.Connection();
            this.fgrid = fg;
            this.Shown += WireCalcForProductionRegisterForm_Shown;
            this.tabContrlType.SelectedIndexChanged += TabContrlType_SelectedIndexChanged;
            this.tabPageManual.Click += TabPageManual_Click;

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

;
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
                    txtTotalWOld.Text = InputQty.ToString();
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
                    txtQtyOld.Text = InputQty.ToString("N0");
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
                    txtQtyNew.Text = InputQty.ToString("N0");
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
                ErrorText = ex.Message;
            }
            cnn.con.Close();

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
                    if (txtTotalWOld.Text.Trim() == "")
                        tasksBlink.Add(BlinkPictureBox(PicAlertWOld));

                    if (txtQtyOld.Text.Trim() == "")
                        tasksBlink.Add(BlinkPictureBox(PicAlertLOld));

                    if(CboBobbinW.Text.Trim()=="")
                        tasksBlink.Add(BlinkPictureBox(PicAlertBobbinW));
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
                            if(txtNetWNew.Text.Trim()=="")
                                tasksBlink.Add(BlinkPictureBox(PicAlertNWNew));

                            if(txtTotalWNew.Text.Trim()=="")
                                tasksBlink.Add(BlinkPictureBox(PicAlertWNew));
                        }
                    }

                }
            }



            if (CboBobbinW.Text.Trim() == "")
            {
                if (rdbNewReg.Checked == true)
                {
                    if (txtNetWNew.Text.Trim() == "")
                        tasksBlink.Add(BlinkPictureBox(PicAlertNWNew));

                    if (txtTotalWNew.Text.Trim() == "")
                        tasksBlink.Add(BlinkPictureBox(PicAlertWNew));

                }
                else
                {
                    tasksBlink.Add(BlinkPictureBox(PicAlertBobbinW));
                }
            }
            else
            {
                if (Convert.ToDouble(CboBobbinW.Text.Trim()) <= 0)
                {
                    if (rdbNewReg.Checked == true)
                    {
                        tasksBlink.Add(BlinkPictureBox(PicAlertWNew));
                        tasksBlink.Add(BlinkPictureBox(PicAlertNWNew));
                    }
                    else
                    {
                        tasksBlink.Add(BlinkPictureBox(PicAlertBobbinW));
                    }
                }
            }

            if (rdbNewReg.Checked == true)
            {
                if (txtQtyNew.Text.Trim() == "")
                    tasksBlink.Add(BlinkPictureBox(PicAlertLNew));

                if (txtNetWNew.Text.Trim() == "")
                    tasksBlink.Add(BlinkPictureBox(PicAlertNWNew));

                if (txtTotalWNew.Text.Trim() == "")
                    tasksBlink.Add(BlinkPictureBox(PicAlertWNew));

            }
            else
            {
                if (txtTotalWOld.Text.Trim() == "")
                    tasksBlink.Add(BlinkPictureBox(PicAlertWOld));

                if (txtQtyOld.Text.Trim() == "")
                    tasksBlink.Add(BlinkPictureBox(PicAlertLOld));
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
            if (tabContrlType.SelectedIndex == 0)
            {
            }
            else
            {

                CboBobbinW.Items.Clear();

                if (rdbNewReg.Checked == true)
                {
                    //txtTotalWNew.Text = "";
                    //txtQtyNew.Text = "";
                    txtNetWNew.Text = "";
                }
                else
                {
                    txtQtyOld.Text = "";
                    txtTotalWOld.Text = "";
                }

                SeletedRMType = "";
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
                        double TotalQty = RemainL / (RemainW - BobbinW);
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

    }
}
