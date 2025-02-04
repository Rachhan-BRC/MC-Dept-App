using MachineDeptApp.MsgClass;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MachineDeptApp.NG_Input
{
    public partial class NGInputByCountForm : Form
    {
        ErrorMsgClass EMsg = new ErrorMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SQLConnect cnn = new SQLConnect();
        NGInputForm fgrid;

        string ErrorText;

        public NGInputByCountForm(NGInputForm fg)
        {
            InitializeComponent();
            cnn.Connection();
            cnnOBS.Connection();
            this.fgrid = fg;
            this.Shown += NGInputByCountForm_Shown;

            this.txtKG.KeyPress += TxtKG_KeyPress;
            this.txtKG.Leave += TxtKG_Leave;

            this.CboItems.SelectedIndexChanged += CboItems_SelectedIndexChanged;
            this.CboItems.TextChanged += CboItems_TextChanged;

        }

        
        private void CboItems_TextChanged(object sender, EventArgs e)
        {
            txtCode.Text = "";
        }
        private void CboItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CboItems.Text.Trim() != "")
            {
                try
                {
                    cnn.con.Open();
                    string SQLQuery = "SELECT ItemCode, ItemName FROM tbMasterItem WHERE ItemType='Material' AND ItemName='" + CboItems.Text + "'";
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    DataTable RMCode = new DataTable();
                    sda.Fill(RMCode);
                    if (RMCode.Rows.Count > 0)
                    {
                        txtCode.Text = RMCode.Rows[0]["ItemCode"].ToString();
                        txtKG.Focus();
                    }
                }
                catch (Exception ex)
                {
                    EMsg.AlertText = ex.Message;
                    EMsg.ShowingMsg();
                }
                cnn.con.Close();
            }
        }        

        private void btnOK_Click(object sender, EventArgs e)
        {
            CheckBeforeSave();
            if (PicAlertItems.Visible == false && PicAlertQty.Visible == false && PicAlertRemarks.Visible == false)
            {
                QMsg.QAText = "តើអ្នកចង់រក្សាទុកទិន្នន័យនេះមែនឬទេ?";
                QMsg.UserClickedYes = false;
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    Cursor = Cursors.WaitCursor;
                    ErrorText = "";

                    DateTime RegNow = DateTime.Now;
                    string RegBy = MenuFormV2.UserForNextForm;
                    string TransNo = "";
                    try
                    {
                        //Check Stock Remain first
                        cnn.con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT Code,StockValue FROM " +
                            "(SELECT Code, SUM(StockValue) AS StockValue  FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='MC1' AND POSNo='' " +
                            "GROUP BY Code) T1 " +
                            "WHERE StockValue>0 AND Code = '" + txtCode.Text + "' ", cnn.con);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            if (Convert.ToDouble(dt.Rows[0]["StockValue"].ToString()) >= Convert.ToDouble(txtKG.Text))
                            {
                                //Find Last TransNo
                                SqlDataAdapter da = new SqlDataAdapter("SELECT SysNo FROM tbSDMCAllTransaction " +
                                                                                                "WHERE SysNo=(SELECT MAX(SysNo) FROM tbSDMCAllTransaction WHERE Funct =6) Group By SysNo", cnn.con);
                                DataTable dtTransNo = new DataTable();
                                da.Fill(dtTransNo);
                                if (dtTransNo.Rows.Count == 0)
                                {
                                    TransNo = "NG0000000001";
                                }
                                else
                                {
                                    string LastTransNo = dtTransNo.Rows[0][0].ToString();
                                    double NextTransNo = Convert.ToDouble(LastTransNo.Substring(LastTransNo.Length - 10)) + 1;
                                    TransNo = "NG" + NextTransNo.ToString("0000000000");

                                }

                                //Add to tbNGHistory
                                SqlCommand cmd = new SqlCommand("INSERT INTO tbNGHistory (SysNo, ItemCode, NGQty, ReqStat, RegDate, RegBy, Remarks) " +
                                    "VALUES (@Sn, @Cd, @NG, @Rs, @Rd, @Rb, @Rms)", cnn.con);
                                cmd.Parameters.AddWithValue("@Sn", TransNo);
                                cmd.Parameters.AddWithValue("@Cd", txtCode.Text.ToString());
                                cmd.Parameters.AddWithValue("@NG", Convert.ToDouble(txtKG.Text.ToString()));
                                cmd.Parameters.AddWithValue("@Rs", 0);
                                cmd.Parameters.AddWithValue("@Rd", RegNow);
                                cmd.Parameters.AddWithValue("@Rb", RegBy);
                                cmd.Parameters.AddWithValue("@Rms", txtRemarks.Text.ToString());
                                cmd.ExecuteNonQuery();

                                //Add to tbSDMCAllTransaction
                                cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction (SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                    "VALUES (@Sn, @Fc, @Lc, @POSN, @Cd, @Rmd, @Rqty, @Tqty, @SV, @Rd, @Rb, @Cs, @Rms)", cnn.con);
                                cmd.Parameters.AddWithValue("@Sn", TransNo);
                                cmd.Parameters.AddWithValue("@Fc", 6);
                                cmd.Parameters.AddWithValue("@Lc", "MC1");
                                cmd.Parameters.AddWithValue("@POSN", "");
                                cmd.Parameters.AddWithValue("@Cd", txtCode.Text.ToString());
                                cmd.Parameters.AddWithValue("@Rmd", CboItems.Text);
                                cmd.Parameters.AddWithValue("@Rqty", 0);
                                cmd.Parameters.AddWithValue("@Tqty", Convert.ToDouble(txtKG.Text.ToString()));
                                cmd.Parameters.AddWithValue("@SV", Convert.ToDouble(txtKG.Text.ToString()) * (-1));
                                cmd.Parameters.AddWithValue("@Rd", RegNow);
                                cmd.Parameters.AddWithValue("@Rb", RegBy);
                                cmd.Parameters.AddWithValue("@Cs", 0);
                                cmd.Parameters.AddWithValue("@Rms", txtRemarks.Text.ToString());
                                cmd.ExecuteNonQuery();


                                //Add to Parent form
                                fgrid.dgvNGalready.Rows.Add();
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["ChkPrint"].Value = false;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["SysNo"].Value = TransNo;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["RMCode"].Value = txtCode.Text;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["RMName"].Value = CboItems.Text;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["Qty"].Value = Convert.ToDouble(txtKG.Text.ToString());
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["Remarks"].Value = txtRemarks.Text;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["RegDate"].Value = RegNow;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["RegBy"].Value = RegBy;
                                fgrid.dgvNGalready.ClearSelection();
                                fgrid.dgvNGalready.CurrentCell = null;
                                CboItems.Focus();
                            }
                            else
                            {
                                ErrorText = "ស្តុកវត្ថុធាតុដើមនេះមិនគ្រប់គ្រាន់ទេ!\nស្ដុកនៅសល់ ៖ " + Convert.ToDouble(dt.Rows[0][1].ToString()).ToString("N0") + "\nស្តុកខ្វះ ៖ " + (Convert.ToDouble(txtKG.Text) - Convert.ToDouble(dt.Rows[0][1].ToString())).ToString("N0");
                            }
                        }
                        else
                        {
                            ErrorText = "មិនមានស្តុកវត្ថុធាតុដើមនេះទេ!\nស្តុកខ្វះ ៖ " + Convert.ToDouble(txtKG.Text).ToString("N0");
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorText = ex.Message;
                        txtKG.Focus();
                        txtKG.SelectAll();
                    }
                    cnn.con.Close();

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
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

        private void TxtKG_Leave(object sender, EventArgs e)
        {
            if (txtKG.Text.Trim() != "")
            {
                try
                {
                    double TotalNG = Convert.ToDouble(txtKG.Text);
                    txtKG.Text = TotalNG.ToString("N0");
                }
                catch (Exception ex)
                {
                    WMsg.WarningText = ex.Message;
                    WMsg.ShowingMsg();
                    txtKG.Text = "";
                }
            }
        }
        private void TxtKG_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }

        private void NGInputByCountForm_Shown(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;

            try
            {
                cnnOBS.conOBS.Open();
                string SQLQuery = "SELECT ItemCode, ItemName FROM mstitem " +
                                            "\nWHERE ItemType =2 AND DelFlag=0 AND NOT MatTypeCode=2 ORDER BY ItemCode ASC";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                DataRow row = dt.NewRow();
                row[0] = "";
                dt.Rows.InsertAt(row, 0);

                CboItems.DataSource = dt;
                CboItems.DisplayMember = "ItemName";

                //Set AutoCompleteMode.
                CboItems.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                CboItems.AutoCompleteSource = AutoCompleteSource.ListItems;

            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnnOBS.conOBS.Close();

            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
            {
                EMsg.AlertText = ErrorText;
                EMsg.ShowingMsg();
            }
        }

        //Function
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
        private async void CheckBeforeSave()
        {
            PicAlertItems.Visible = false;
            PicAlertQty.Visible = false;
            PicAlertRemarks.Visible = false;
            var tasksBlink = new List<Task>();

            if (txtCode.Text.Trim() == "" || CboItems.Text.Trim() == "")
            {
                tasksBlink.Add(BlinkPictureBox(PicAlertItems));
            }
            if (txtKG.Text.Trim() == "")
            {
                tasksBlink.Add(BlinkPictureBox(PicAlertQty));
            }
            if (txtRemarks.Text.Trim() == "")
            {
                tasksBlink.Add(BlinkPictureBox(PicAlertRemarks));
            }

            await Task.WhenAll(tasksBlink);
        }
        private void ClearAllText()
        {
            CboItems.Text = "";
            txtCode.Text = "";
            txtKG.Text = "";
            txtKG.Text = "";
            //txtRemarks.Text = "";
            CboItems.Focus();
        }

    }
}
