using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MachineDeptApp.NG_Input
{
    public partial class NGInputByCountForm : Form
    {
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SQLConnect cnn = new SQLConnect();
        NGInputForm fgrid;
        SqlCommand cmd;
        string Username;
        string lastSysNo;
        int nextLabelNo;

        public NGInputByCountForm(NGInputForm fg)
        {
            InitializeComponent();
            cnn.Connection();
            cnnOBS.Connection();
            this.fgrid = fg;
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
                SqlDataAdapter sda = new SqlDataAdapter("Select ItemCode From tbMasterItem Where Len(ItemCode)<5 AND ItemName='" + CboItems.Text + "'", cnn.con);
                try
                {
                    cnn.con.Open();
                    //Fill the DataTable with records from Table.
                    DataTable RMCode = new DataTable();
                    sda.Fill(RMCode);
                    if (RMCode.Rows.Count > 0)
                    {
                        txtCode.Text = RMCode.Rows[0][0].ToString();
                        txtKG.Focus();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnn.con.Close();
            }
        }

        private void TxtKG_Leave(object sender, EventArgs e)
        {
            if (txtKG.Text.Trim() != "")
            {
                try
                {
                    double TotalNG = Convert.ToDouble(txtKG.Text);
                    txtKG.Text = String.Format("{0:#,###}", TotalNG);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtKG.Text = "";

                }
            }
            else
            {

            }
        }

        private void TxtKG_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            
        }

        private void NGInputByCountForm_Load(object sender, EventArgs e)
        {
            //add all data to control box
            SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName FROM mstitem WHERE ItemType =2 AND DelFlag=0 AND NOT MatTypeCode=2 " +
                "ORDER BY ItemCode ASC", cnnOBS.conOBS);
            try
            {
                cnnOBS.conOBS.Open();
                //Fill the DataTable with records from Table.
                DataTable dt = new DataTable();
                sda.Fill(dt);

                //Insert the Default Item to DataTable.
                DataRow row = dt.NewRow();
                row[0] = "";
                dt.Rows.InsertAt(row, 0);

                //Assign DataTable as DataSource.
                CboItems.DataSource = dt;
                CboItems.DisplayMember = "ItemName";

                //Set AutoCompleteMode.
                CboItems.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                CboItems.AutoCompleteSource = AutoCompleteSource.ListItems;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnnOBS.conOBS.Close();
            Username = MenuFormV2.UserForNextForm;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            PicKG.Visible = false;
            PicItems.Visible = false;

            if (txtCode.Text.Trim() == "" || txtKG.Text.Trim() == "")
            {
                //2
                //1,2
                if (txtCode.Text.Trim() == "" && txtKG.Text.Trim() == "")
                {
                    PicKG.Visible = true;
                    PicItems.Visible = true;
                    CboItems.Focus();

                }
                //1
                else if (txtCode.Text.Trim() == "")
                {
                    PicItems.Visible = true;
                    CboItems.Focus();

                }
                else if (txtKG.Text.Trim() == "")
                {
                    PicKG.Visible = true;
                    txtKG.Focus();

                }

                else
                {

                }
            }
            else
            {
                DialogResult DLR = MessageBox.Show("តើអ្នកចង់រក្សាទុកទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    DateTime RegNow = DateTime.Now;
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
                            if (Convert.ToDouble(dt.Rows[0][1].ToString()) >= Convert.ToDouble(txtKG.Text))
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
                                cmd = new SqlCommand("INSERT INTO tbNGHistory (SysNo, ItemCode, NGQty, ReqStat, RegDate, RegBy) VALUES (@Sn, @Cd, @NG, @Rs, @Rd, @Rb)", cnn.con);
                                cmd.Parameters.AddWithValue("@Sn", TransNo);
                                cmd.Parameters.AddWithValue("@Cd", txtCode.Text.ToString());
                                cmd.Parameters.AddWithValue("@NG", Convert.ToDouble(txtKG.Text.ToString()));
                                cmd.Parameters.AddWithValue("@Rs", 0);
                                cmd.Parameters.AddWithValue("@Rd", RegNow.ToString("yyyy-MM-dd HH:mm:ss"));
                                cmd.Parameters.AddWithValue("@Rb", Username);
                                cmd.ExecuteNonQuery();

                                //Add to tbSDMCAllTransaction
                                cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction (SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus) VALUES (@Sn, @Fc, @Lc, @POSN, @Cd, @Rmd, @Rqty, @Tqty, @SV, @Rd, @Rb, @Cs)", cnn.con);
                                cmd.Parameters.AddWithValue("@Sn", TransNo);
                                cmd.Parameters.AddWithValue("@Fc", 6);
                                cmd.Parameters.AddWithValue("@Lc", "MC1");
                                cmd.Parameters.AddWithValue("@POSN", "");
                                cmd.Parameters.AddWithValue("@Cd", txtCode.Text.ToString());
                                cmd.Parameters.AddWithValue("@Rmd", CboItems.Text);
                                cmd.Parameters.AddWithValue("@Rqty", 0);
                                cmd.Parameters.AddWithValue("@Tqty", Convert.ToDouble(txtKG.Text.ToString()));
                                cmd.Parameters.AddWithValue("@SV", Convert.ToDouble(txtKG.Text.ToString()) * (-1));
                                cmd.Parameters.AddWithValue("@Rd", RegNow.ToString("yyyy-MM-dd HH:mm:ss"));
                                cmd.Parameters.AddWithValue("@Rb", Username);
                                cmd.Parameters.AddWithValue("@Cs", 0);
                                cmd.ExecuteNonQuery();


                                //Add to Parent form
                                fgrid.dgvNGalready.Rows.Add(txtCode.Text.ToString(), CboItems.Text, Convert.ToDouble(txtKG.Text.ToString()), false, RegNow, Username, TransNo);
                                fgrid.dgvNGalready.ClearSelection();
                                CboItems.Text = "";
                                txtCode.Text = "";
                                txtKG.Text = "";
                                CboItems.Focus();

                            }
                            else
                            {
                                MessageBox.Show("ស្តុកវត្ថុធាតុដើមនេះមិនគ្រប់គ្រាន់ទេ!\nស្ដុកនៅសល់ ៖ " + Convert.ToDouble(dt.Rows[0][1].ToString()).ToString("N0")+ "\nស្តុកខ្វះ ៖ "+ (Convert.ToDouble(txtKG.Text) - Convert.ToDouble(dt.Rows[0][1].ToString())).ToString("N0"), "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                        else
                        {
                            MessageBox.Show("មិនមានស្តុកវត្ថុធាតុដើមនេះទេ!\nស្តុកខ្វះ ៖ "+ Convert.ToDouble(txtKG.Text).ToString("N0"), "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtKG.Focus();
                        txtKG.SelectAll();
                    }
                    cnn.con.Close();
                }
                else
                {

                }
            }
        }

    }
}
