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
        DataTable dtRMMst;

        string ErrorText;

        public NGInputByCountForm(NGInputForm fg)
        {
            InitializeComponent();
            cnn.Connection();
            cnnOBS.Connection();
            this.fgrid = fg;
            this.Shown += NGInputByCountForm_Shown;
            this.btnShowDgvItems.Click += BtnShowDgvItems_Click;
            this.DgvSearchItem.LostFocus += DgvSearchItem_LostFocus;
            this.DgvSearchItem.CellClick += DgvSearchItem_CellClick;
            this.txtSearchItem.LostFocus += TxtSearchItem_LostFocus;
            this.txtSearchItem.TextChanged += TxtSearchItem_TextChanged;
            this.txtCode.KeyDown += TxtCode_KeyDown;
            this.txtCode.Leave += TxtCode_Leave;

            this.txtNGQty.KeyPress += TxtKG_KeyPress;
            this.txtNGQty.Leave += TxtKG_Leave;

        }

        private void DgvSearchItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                txtCode.Text = DgvSearchItem.Rows[e.RowIndex].Cells[0].Value.ToString();
                TakingItemName();
                txtNGQty.Focus();
            }
        }
        private void TxtCode_Leave(object sender, EventArgs e)
        {
            TakingItemName();
        }
        private void TxtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                TakingItemName();
        }
        private void TxtSearchItem_TextChanged(object sender, EventArgs e)
        {
            SearchItems();
        }
        private void TxtSearchItem_LostFocus(object sender, EventArgs e)
        {
            if (DgvSearchItem.Focused == false && txtSearchItem.Focused == false)
            {
                panelSearchItem.Visible = false;
            }
        }
        private void DgvSearchItem_LostFocus(object sender, EventArgs e)
        {
            if (DgvSearchItem.Focused == false && txtSearchItem.Focused == false)
            {
                panelSearchItem.Visible = false;
            }
        }
        private void BtnShowDgvItems_Click(object sender, EventArgs e)
        {
            if (panelSearchItem.Visible == false)
            {
                panelSearchItem.Visible = true;
                txtSearchItem.Focus();
                SearchItems();
            }
            else
            {
                panelSearchItem.Visible = false;
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
                            if (Convert.ToDouble(dt.Rows[0]["StockValue"].ToString()) >= Convert.ToDouble(txtNGQty.Text))
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
                                cmd.Parameters.AddWithValue("@NG", Convert.ToDouble(txtNGQty.Text.ToString()));
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
                                cmd.Parameters.AddWithValue("@Rmd", txtItems.Text);
                                cmd.Parameters.AddWithValue("@Rqty", 0);
                                cmd.Parameters.AddWithValue("@Tqty", Convert.ToDouble(txtNGQty.Text.ToString()));
                                cmd.Parameters.AddWithValue("@SV", Convert.ToDouble(txtNGQty.Text.ToString()) * (-1));
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
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["RMName"].Value = txtItems.Text;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["Qty"].Value = Convert.ToDouble(txtNGQty.Text.ToString());
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["Remarks"].Value = txtRemarks.Text;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["RegDate"].Value = RegNow;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["RegBy"].Value = RegBy;
                                fgrid.dgvNGalready.ClearSelection();
                                fgrid.dgvNGalready.CurrentCell = null;
                                txtItems.Focus();
                            }
                            else
                            {
                                ErrorText = "ស្តុកវត្ថុធាតុដើមនេះមិនគ្រប់គ្រាន់ទេ!\nស្ដុកនៅសល់ ៖ " + Convert.ToDouble(dt.Rows[0][1].ToString()).ToString("N0") + "\nស្តុកខ្វះ ៖ " + (Convert.ToDouble(txtNGQty.Text) - Convert.ToDouble(dt.Rows[0][1].ToString())).ToString("N0");
                            }
                        }
                        else
                        {
                            ErrorText = "មិនមានស្តុកវត្ថុធាតុដើមនេះទេ!\nស្តុកខ្វះ ៖ " + Convert.ToDouble(txtNGQty.Text).ToString("N0");
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorText = ex.Message;
                        txtNGQty.Focus();
                        txtNGQty.SelectAll();
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
            if (txtNGQty.Text.Trim() != "")
            {
                try
                {
                    double TotalNG = Convert.ToDouble(txtNGQty.Text);
                    txtNGQty.Text = TotalNG.ToString("N0");
                }
                catch (Exception ex)
                {
                    WMsg.WarningText = ex.Message;
                    WMsg.ShowingMsg();
                    txtNGQty.Text = "";
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
            panelSearchItem.BringToFront();

            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT tbMasterItem.* FROM tbMasterItem " +
                                            "\nINNER JOIN (SELECT LowItemCode FROM MstBOM GROUP BY LowItemCode) T1 ON ItemCode=LowItemCode " +
                                            "\nORDER BY ItemCode ASC";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                dtRMMst = new DataTable();
                sda.Fill(dtRMMst);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

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

            if (txtCode.Text.Trim() == "" || txtItems.Text.Trim() == "")
            {
                tasksBlink.Add(BlinkPictureBox(PicAlertItems));
            }
            if (txtNGQty.Text.Trim() == "")
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
            txtItems.Text = "";
            txtCode.Text = "";
            txtNGQty.Text = "";
            txtNGQty.Text = "";
            //txtRemarks.Text = "";
            txtCode.Focus();
        }
        private void SearchItems()
        {
            DgvSearchItem.Rows.Clear();
            if (txtSearchItem.Text.Trim() == "")
            {
                foreach (DataRow row in dtRMMst.Rows)
                    DgvSearchItem.Rows.Add(row["ItemCode"], row["ItemName"]);
                DgvSearchItem.ClearSelection();
            }
            else
            {
                string SearchText = txtSearchItem.Text.ToLower();
                foreach (DataRow row in dtRMMst.Rows)
                {
                    if (row["ItemCode"].ToString().ToLower().Contains(SearchText) ==true || row["ItemName"].ToString().ToLower().Contains(SearchText) == true)
                        DgvSearchItem.Rows.Add(row["ItemCode"], row["ItemName"]);
                }
                DgvSearchItem.ClearSelection();
            }
        }
        private void TakingItemName()
        {
            string ItemCode = txtCode.Text;
            txtItems.Text = "";
            foreach (DataRow row in dtRMMst.Rows)
            {
                if (row["ItemCode"].ToString() == ItemCode)
                {
                    txtItems.Text = row["ItemName"].ToString();
                    txtNGQty.Focus();
                    break;
                }
            }
        }

    }
}
