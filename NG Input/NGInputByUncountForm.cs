using MachineDeptApp.MsgClass;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MachineDeptApp.NG_Input
{
    public partial class NGInputByUncountForm : Form
    {
        ErrorMsgClass EMsg = new ErrorMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        SQLConnect cnn = new SQLConnect();
        NGInputForm fgrid;
        DataTable dtMstRM;

        string ErrorText;

        public NGInputByUncountForm(NGInputForm fg)
        {
            InitializeComponent();
            this.cnn.Connection();
            this.fgrid = fg;
            this.Shown += NGInputByUncountForm_Shown;

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
            cnn.con.Close();
            if (CboItems.Text.Trim() != "")
            {
                //add all data to control box
                SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode FROM tbMasterItem WHERE Len(ItemCode)<5 AND ItemName='" + CboItems.Text + "'", cnn.con);
                try
                {
                    cnn.con.Open();
                    //Fill the DataTable with records from Table.
                    DataTable RMCode = new DataTable();
                    sda.Fill(RMCode);
                    if (RMCode.Rows.Count > 0)
                    {
                        txtCode.Text = RMCode.Rows[0][0].ToString();
                        txtLength.Text = "";
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
        private void TxtKG_Leave(object sender, EventArgs e)
        {
            if (CboItems.Text.Trim() != "")
            {
                if (txtKG.Text.Trim() != "")
                {
                    try
                    {
                        double MOQ;
                        double KG;
                        double TotalMeter;
                        double roundInputKG = Convert.ToDouble(txtKG.Text);
                        txtKG.Text = Math.Round(roundInputKG, 3, MidpointRounding.AwayFromZero).ToString("N3");

                        //add all data to control box
                        SqlCommand cmd = new SqlCommand("Select * From MstUncountMat WHERE LowItemCode='" + txtCode.Text + "'", cnn.con);
                        try
                        {
                            cnn.con.Open();
                            SqlDataReader dr = cmd.ExecuteReader();
                            if (dr.Read())
                            {
                                MOQ = Convert.ToDouble(dr.GetValue(2).ToString());
                                KG = Convert.ToDouble(dr.GetValue(1).ToString());
                                TotalMeter = roundInputKG / KG * MOQ;
                                txtLength.Text = TotalMeter.ToString("N3");

                            }
                        }
                        catch
                        {
                            MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        cnn.con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtLength.Text = "";
                    }
                }
            }
            else
            {
                txtLength.Text = "";
            }
        }
        private void TxtKG_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            CheckBeforeSave();
            if (PicAlertItems.Visible == false && PicAlertWeight.Visible == false && PicAlertRemarks.Visible == false)
            {
                QMsg.QAText = "តើអ្នកចង់រក្សាទុកទិន្នន័យនេះមែនឬទេ?";
                QMsg.UserClickedYes = false;
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;

                    string TransNo = "";
                    DateTime RegNow = DateTime.Now;
                    string RegBy = MenuFormV2.UserForNextForm;
                    try
                    {
                        cnn.con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT Code, StockValue FROM " +
                            "(SELECT Code, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='MC1' AND POSNo='' " +
                            "GROUP BY Code) T1 " +
                            "WHERE StockValue > 0 AND Code = '" + txtCode.Text + "' ", cnn.con);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            if (Convert.ToDouble(dt.Rows[0]["StockValue"].ToString()) >= Convert.ToDouble(txtLength.Text))
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
                                cmd.Parameters.AddWithValue("@NG", Convert.ToDouble(txtLength.Text.ToString()));
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
                                cmd.Parameters.AddWithValue("@Tqty", Convert.ToDouble(txtLength.Text.ToString()));
                                cmd.Parameters.AddWithValue("@SV", Convert.ToDouble(txtLength.Text.ToString()) * (-1));
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
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["Qty"].Value = Convert.ToDouble(txtLength.Text);
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["Remarks"].Value = txtRemarks.Text;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["RegDate"].Value = RegNow;
                                fgrid.dgvNGalready.Rows[fgrid.dgvNGalready.Rows.Count - 1].Cells["RegBy"].Value = RegBy;
                                fgrid.dgvNGalready.ClearSelection();
                                fgrid.dgvNGalready.CurrentCell = null;
                            }
                            else
                            {
                                ErrorText = "ស្តុកវត្ថុធាតុដើមនេះមិនគ្រប់គ្រាន់ទេ!\nស្ដុកនៅសល់ ៖ " + Convert.ToDouble(dt.Rows[0][1].ToString()).ToString("N3") + "\nស្តុកខ្វះ ៖ " + (Convert.ToDouble(txtLength.Text) - Convert.ToDouble(dt.Rows[0][1].ToString())).ToString("N3");                                
                            }
                        }
                        else
                        {
                            ErrorText = "មិនមានស្តុកវត្ថុធាតុដើមនេះទេ!\nស្តុកខ្វះ ៖ " + Convert.ToDouble(txtLength.Text).ToString("N3");
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

        private void NGInputByUncountForm_Shown(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            ErrorText = "";

            //Taking Data
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT * FROM " +
                    "\n(SELECT * FROM MstUncountMat) T1 " +
                    "\nINNER JOIN (SELECT ItemCode, ItemName FROM tbMasterItem WHERE ItemType='Material') T2 ON T1.LowItemCode=T2.ItemCode";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                dtMstRM = new DataTable();
                sda.Fill(dtMstRM);

                //Insert the Default Item to DataTable.
                DataRow row = dtMstRM.NewRow();
                row[0] = "";
                dtMstRM.Rows.InsertAt(row, 0);

                //Assign DataTable as DataSource.
                CboItems.DataSource = dtMstRM;
                CboItems.DisplayMember = "ItemName";

                //Set AutoCompleteMode.
                CboItems.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                CboItems.AutoCompleteSource = AutoCompleteSource.ListItems;

            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
            {
                EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
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
            PicAlertWeight.Visible = false;
            PicAlertRemarks.Visible = false;
            var tasksBlink = new List<Task>();

            if (txtCode.Text.Trim() == "" || CboItems.Text.Trim()=="")
            {
                tasksBlink.Add(BlinkPictureBox(PicAlertItems));
            }
            if (txtKG.Text.Trim() == "")
            {
                tasksBlink.Add(BlinkPictureBox(PicAlertWeight));
            }
            else
            {
                if (txtLength.Text.Trim()=="")
                {
                    tasksBlink.Add(BlinkPictureBox(PicAlertWeight));
                }
                else
                {
                     if (Convert.ToDouble(txtLength.Text) <= 0)
                    {
                        tasksBlink.Add(BlinkPictureBox(PicAlertWeight));
                    }
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
            CboItems.Text = "";
            txtCode.Text = "";
            txtKG.Text = "";
            txtLength.Text = "";
            //txtRemarks.Text = "";
            CboItems.Focus();
        }

    }
}
