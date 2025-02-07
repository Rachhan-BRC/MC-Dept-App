using MachineDeptApp.MsgClass;
using MachineDeptApp.InputTransferSemi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MachineDeptApp.NG_Input
{
    public partial class NGInputBySetForm : Form
    {
        ErrorMsgClass EMsg = new ErrorMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        SQLConnect cnn = new SQLConnect();
        NGInputForm fgrid;

        string ErrorText;

        public NGInputBySetForm(NGInputForm fg)
        {
            InitializeComponent();
            cnn.Connection();
            this.fgrid = fg;
            this.Shown += NGInputBySetForm_Shown;
            this.txtWIPNameSearch.KeyDown += TxtItemname_KeyDown;
            this.txtWIPCodeSearch.KeyDown += TxtWIPCodeSearch_KeyDown;

            this.dgvSemi.CurrentCellChanged += DgvSemi_CurrentCellChanged;
            this.txtNGQty.KeyPress += TxtNGQty_KeyPress;

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CheckBeforeSave();
            if (PicAlertItems.Visible == false && PicAlertQty.Visible == false && PicAlertRemarks.Visible == false)
            {
                QMsg.QAText = "តើអ្នករក្សាទុកទិន្នន័យនេះមែនទេ ?";
                QMsg.UserClickedYes = false;
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    Cursor = Cursors.WaitCursor;
                    DateTime RegNow = DateTime.Now;
                    string RegBy = MenuFormV2.UserForNextForm;
                    string ErrorText = "";
                    string TransNo = "";
                    string ShortageRM = "";

                    try
                    {
                        cnn.con.Open();
                        int NGQty = Convert.ToInt16(txtNGQty.Text.ToString());
                        string RMCodeIN = "";
                        foreach (DataGridViewRow row in dgvConsump.Rows)
                        {
                            if (RMCodeIN.Trim() == "")
                            {
                                RMCodeIN = "'" + row.Cells["RMCode"].Value.ToString() + "'";
                            }
                            else
                            {
                                RMCodeIN += ", '"+ row.Cells["RMCode"].Value.ToString() + "'";
                            }
                        }
                        //Checking Stock
                        DataTable dtStock = new DataTable();
                        string SQLQuery = "SELECT Code,StockValue FROM " +
                            "(SELECT Code, SUM(StockValue) AS StockValue  FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='MC1' AND POSNo='' " +
                            "GROUP BY Code) T1 " +
                            "WHERE StockValue>0 AND Code IN ("+RMCodeIN+") ";
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        sda.Fill(dtStock);
                        foreach (DataGridViewRow row in dgvConsump.Rows)
                        {
                            string RMCode = row.Cells["RMCode"].Value.ToString();
                            string RMName = row.Cells["RMName"].Value.ToString();
                            double UsageQty = Convert.ToDouble(row.Cells["ConsumpQty"].Value.ToString()) * NGQty;
                            double RemainStock = 0;
                            int NotOK = 1;
                            foreach (DataRow rowStock in dtStock.Rows)
                            {
                                if (RMCode == rowStock["Code"].ToString())
                                {
                                    RemainStock = Convert.ToDouble(rowStock["StockValue"]);
                                    if (UsageQty <= RemainStock)
                                    {
                                        NotOK=0;
                                    }
                                    break;
                                }
                            }
                            if (NotOK > 0)
                            {
                                if (ShortageRM.Trim() == "")
                                {
                                    ShortageRM = RMName+"  |  "+UsageQty.ToString()+"  |  "+RemainStock.ToString()+"  |  "+ (UsageQty-RemainStock).ToString();
                                }
                                else
                                {
                                    ShortageRM += "\n"+ RMName + "  |  " + UsageQty.ToString() + "  |  " + RemainStock.ToString() + "  |  " + (UsageQty - RemainStock).ToString();
                                }
                            }
                        }

                        if (ShortageRM.Trim() == "")
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

                            //Add to tbNGHistory & Add to tbSDMCAllTransaction
                            foreach (DataGridViewRow row in dgvConsump.Rows)
                            {
                                string RMCode = row.Cells["RMCode"].Value.ToString();
                                string RMName = row.Cells["RMName"].Value.ToString();
                                double UsageQty = Convert.ToDouble(txtNGQty.Text.ToString()) * Convert.ToDouble(row.Cells["ConsumpQty"].Value.ToString());

                                //Add to tbNGHistory
                                SqlCommand cmd = new SqlCommand("INSERT INTO tbNGHistory (SysNo, ItemCode, NGQty, ReqStat, RegDate, RegBy, Remarks) " +
                                    "VALUES (@Sn, @Cd, @NG, @Rs, @Rd, @Rb, @Rms)", cnn.con);
                                cmd.Parameters.AddWithValue("@Sn", TransNo);
                                cmd.Parameters.AddWithValue("@Cd", RMCode);
                                cmd.Parameters.AddWithValue("@NG", UsageQty);
                                cmd.Parameters.AddWithValue("@Rs", 0);
                                cmd.Parameters.AddWithValue("@Rd", RegNow);
                                cmd.Parameters.AddWithValue("@Rb", RegBy);
                                cmd.Parameters.AddWithValue("@Rms", txtRemarks.Text);
                                cmd.ExecuteNonQuery();

                                //Add to tbSDMCAllTransaction
                                cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction (SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                    "VALUES (@Sn, @Fc, @Lc, @POSN, @Cd, @Rmd, @Rqty, @Tqty, @SV, @Rd, @Rb, @Cs, @Rms)", cnn.con);
                                cmd.Parameters.AddWithValue("@Sn", TransNo);
                                cmd.Parameters.AddWithValue("@Fc", 6);
                                cmd.Parameters.AddWithValue("@Lc", "MC1");
                                cmd.Parameters.AddWithValue("@POSN", "");
                                cmd.Parameters.AddWithValue("@Cd", RMCode);
                                cmd.Parameters.AddWithValue("@Rmd", RMName);
                                cmd.Parameters.AddWithValue("@Rqty", 0);
                                cmd.Parameters.AddWithValue("@Tqty", UsageQty);
                                cmd.Parameters.AddWithValue("@SV", UsageQty*(-1));
                                cmd.Parameters.AddWithValue("@Rd", RegNow);
                                cmd.Parameters.AddWithValue("@Rb", RegBy);
                                cmd.Parameters.AddWithValue("@Cs", 0);
                                cmd.Parameters.AddWithValue("@Rms", txtRemarks.Text);
                                cmd.ExecuteNonQuery();

                                //Add to Parent form
                                fgrid.dgvNGalready.Rows.Add();
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["ChkPrint"].Value = false;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["SysNo"].Value = TransNo;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["RMCode"].Value = RMCode;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["RMName"].Value = RMName;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["Qty"].Value = UsageQty;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["Remarks"].Value = txtRemarks.Text;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["RegDate"].Value = RegNow;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["RegBy"].Value = RegBy;
                                fgrid.dgvNGalready.ClearSelection();
                                fgrid.dgvNGalready.CurrentCell = null;

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WMsg.WarningText = "ចំនួន <NG Qty> ត្រូវតែជាលេខចំនួនគត់!\n"+ex.Message;
                        WMsg.ShowingMsg();
                        txtNGQty.Focus();
                        txtNGQty.SelectAll();
                    }
                    cnn.con.Close();

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        if (ShortageRM.Trim() == "")
                        {
                            ClearAllText();
                        }
                        else
                        {
                            WMsg.WarningText = "ស្តុកវត្ថុធាតុដើមមិនគ្រប់គ្រាន់ ៖ " +
                                "\nឈ្មោះ  |  ចំនួនប្រើ  |  ស្តុកនៅសល់  |  ស្តុកខ្វះ" +
                                "\n" + ShortageRM;
                            WMsg.ShowingMsg();
                        }
                    }
                    else
                    {
                        EMsg.AlertText = "មានបញ្ហា!\n"+ErrorText;
                        EMsg.ShowingMsg();
                    }
                }
            }
        }

        private void DgvSemi_CurrentCellChanged(object sender, EventArgs e)
        {
            dgvConsump.Rows.Clear();
            if (dgvSemi.SelectedCells.Count > 0 && dgvSemi.CurrentCell != null && dgvSemi.CurrentCell.RowIndex > -1)
            {
                Cursor = Cursors.WaitCursor;
                ErrorText = "";

                //Taking Data
                string WIPCodeSelected = dgvSemi.Rows[dgvSemi.CurrentCell.RowIndex].Cells["WIPCode"].Value.ToString();
                DataTable dt = new DataTable();
                try
                {
                    cnn.con.Open();
                    string SQLQuery = "SELECT UpItemCode, LowItemCode, ItemName, LowQty FROM " +
                        "\n(SELECT * FROM MstBOM) T1 " +
                        "\nINNER JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') T2 ON T1.LowItemCode=T2.ItemCode " +
                        "\nWHERE UpItemCode = '"+WIPCodeSelected+"' " +
                        "\nORDER BY LowItemCode ASC";
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dt);
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                }
                cnn.con.Close();

                //Add to DGV
                if (ErrorText.Trim() == "")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        dgvConsump.Rows.Add();
                        dgvConsump.Rows[dgvConsump.Rows.Count-1].HeaderCell.Value = dgvConsump.Rows.Count.ToString();
                        dgvConsump.Rows[dgvConsump.Rows.Count - 1].Cells["RMCode"].Value = row["LowItemCode"].ToString();
                        dgvConsump.Rows[dgvConsump.Rows.Count - 1].Cells["RMName"].Value = row["ItemName"].ToString();
                        dgvConsump.Rows[dgvConsump.Rows.Count - 1].Cells["ConsumpQty"].Value = Convert.ToDouble(row["LowQty"].ToString());
                    }
                    txtNGQty.Focus();
                }

                Cursor = Cursors.Default;

                if (ErrorText.Trim() != "")
                {
                    EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                    EMsg.ShowingMsg();
                }
            }
        }
        private void TxtNGQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }        
        private void TxtItemname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchWIP();
            }
        }
        private void TxtWIPCodeSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchWIP();
            }
        }
        private void NGInputBySetForm_Shown(object sender, EventArgs e)
        {
            float FontSize = Convert.ToSingle("9.75");
            dgvConsump.RowHeadersDefaultCellStyle.Font = new Font("Khmer OS Battambang", FontSize, FontStyle.Regular);
        }

        //Function
        private void SearchWIP()
        {
            Cursor = Cursors.WaitCursor;
            dgvSemi.Rows.Clear();
            ErrorText = "";

            //SQL Condition
            DataTable dtSQLCon = new DataTable();
            dtSQLCon.Columns.Add("Col");
            dtSQLCon.Columns.Add("Val");
            if (txtWIPCodeSearch.Text.Trim() != "")
            {
                dtSQLCon.Rows.Add("ItemCode LIKE ", "'%"+txtWIPCodeSearch.Text+"%' ");
            }
            if (txtWIPNameSearch.Text.Trim() != "")
            {
                dtSQLCon.Rows.Add("ItemName LIKE ", "'%" + txtWIPNameSearch.Text + "%' ");
            }
            string SQLConds = "";
            foreach (DataRow row in dtSQLCon.Rows)
            {
                if (SQLConds.Trim() == "")
                {
                    SQLConds = "WHERE " + row["Col"] + row["Val"];
                }
                else
                {
                    SQLConds += " AND " + row["Col"] + row["Val"];
                }
            }

            //Taking Data
            DataTable dt = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT T1.* FROM " +
                                            "\n(SELECT * FROM tbMasterItem WHERE ItemType ='Work In Process') T1 " +
                                            "\nINNER JOIN (SELECT * FROM MstBOM) T2 ON T1.ItemCode=T2.UpItemCode \n"+
                                            SQLConds+" ORDER BY ItemCode ASC ";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

            //Add to DGV
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dt.Rows)
                {
                    dgvSemi.Rows.Add();
                    dgvSemi.Rows[dgvSemi.Rows.Count-1].Cells["WIPCode"].Value = row["ItemCode"].ToString();
                    dgvSemi.Rows[dgvSemi.Rows.Count - 1].Cells["WIPName"].Value = row["ItemName"].ToString();
                    dgvSemi.Rows[dgvSemi.Rows.Count - 1].Cells["PIN"].Value = row["Remarks1"].ToString();
                    dgvSemi.Rows[dgvSemi.Rows.Count - 1].Cells["Wire"].Value = row["Remarks2"].ToString();
                    dgvSemi.Rows[dgvSemi.Rows.Count - 1].Cells["Length"].Value = row["Remarks3"].ToString();
                }
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                dgvSemi.ClearSelection();
                dgvSemi.CurrentCell = null;
            }
            else
            {
                EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                EMsg.ShowingMsg();
            }

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
        private async void CheckBeforeSave()
        {
            PicAlertItems.Visible = false;
            PicAlertQty.Visible = false;
            PicAlertRemarks.Visible = false;
            var tasksBlink = new List<Task>();

            if (dgvSemi.SelectedCells.Count==0 || dgvConsump.Rows.Count==0)
            {
                tasksBlink.Add(BlinkPictureBox(PicAlertItems));
            }
            if (txtNGQty.Text.Trim() == "")
            {
                tasksBlink.Add(BlinkPictureBox(PicAlertQty));
            }
            else
            {
                if (Convert.ToDouble(txtNGQty.Text) <= 0)
                {
                    tasksBlink.Add(BlinkPictureBox(PicAlertQty));
                }
            }
            if (txtRemarks.Text.Trim() == "")
            {
                tasksBlink.Add(BlinkPictureBox(PicAlertRemarks));
            }

            await Task.WhenAll(tasksBlink);
        }
        private void ClearAllText()
        {
            txtNGQty.Text = "";
            dgvSemi.Rows.Clear();
        }

    }
}
