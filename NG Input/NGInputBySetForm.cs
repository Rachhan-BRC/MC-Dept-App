using MachineDeptApp.InputTransferSemi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace MachineDeptApp.NG_Input
{
    public partial class NGInputBySetForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        NGInputForm fgrid;
        DataTable SemiInforSearch;
        DataTable SemiConsumpSearch;
        DataTable SelectedConsump;
        SqlCommand cmd;
        string Username;
        string lastSysNo;
        int nextLabelNo;


        public NGInputBySetForm(NGInputForm fg)
        {
            InitializeComponent();
            cnn.Connection();
            this.fgrid = fg;
            this.txtItemname.KeyDown += TxtItemname_KeyDown;
            this.dgvSemi.CellClick += DgvSemi_CellClick;
            this.txtNGQty.KeyPress += TxtNGQty_KeyPress;

        }

        private void TxtNGQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void DgvSemi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string SemiSelected = "";
            dgvConsump.Rows.Clear();
            SelectedConsump.Rows.Clear();
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvSemi.Rows[e.RowIndex];
                SemiSelected = row.Cells[0].Value.ToString();
                for (int i = 0; i < SemiConsumpSearch.Rows.Count; i++)
                {
                    if (SemiConsumpSearch.Rows[i][0].ToString() == SemiSelected)
                    {
                        dgvConsump.Rows.Add(SemiConsumpSearch.Rows[i][1].ToString(), SemiConsumpSearch.Rows[i][5].ToString(), Convert.ToDouble(SemiConsumpSearch.Rows[i][2].ToString()));
                        SelectedConsump.Rows.Add(SemiConsumpSearch.Rows[i][1].ToString(), SemiConsumpSearch.Rows[i][5].ToString(), Convert.ToDouble(SemiConsumpSearch.Rows[i][2].ToString()));
                    }

                }
                dgvConsump.ClearSelection();
                txtNGQty.Focus();
            }
            else
            {

            }
            
        }

        private void TxtItemname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dgvSemi.Rows.Clear();
                dgvConsump.Rows.Clear();
                SelectedConsump.Rows.Clear();
                if (txtItemname.Text.Trim() != "")
                {
                    //Take Semi Inform
                    try
                    {
                        cnn.con.Open();
                        SqlDataAdapter da = new SqlDataAdapter("Select UpItemCode, tbMasterItem.ItemName, tbMasterItem.Remarks1, tbMasterItem.Remarks2, tbMasterItem.Remarks3, Max(UpdateDate) as LastUpdate From MstBOM "+
                                                                                        "INNER JOIN tbMasterItem ON MstBOM.UpItemCode = tbMasterItem.ItemCode "+
                                                                                        "Where LEN(ItemCode) > 4  AND ItemName Like '%"+txtItemname.Text.ToString()+"%' "+
                                                                                        "Group By UpItemCode, tbMasterItem.ItemName, tbMasterItem.Remarks1, tbMasterItem.Remarks2, tbMasterItem.Remarks3", cnn.con);
                        SemiInforSearch = new DataTable();
                        da.Fill(SemiInforSearch);
                        SqlDataAdapter da1 = new SqlDataAdapter("Select * From (Select UpItemCode, LowItemCode, LowQty as OnePcsUse, Yield From MstBOM) t1 INNER JOIN (Select ItemCode, ItemName From tbMasterItem Where LEN(ItemCode)<5 Group By ItemCode, ItemName) t2 ON t1.LowItemCode=t2.ItemCode", cnn.con);
                        SemiConsumpSearch = new DataTable();
                        da1.Fill(SemiConsumpSearch);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cnn.con.Close();

                    //Add to dgv
                    if (SemiInforSearch.Rows.Count > 0)
                    {
                        for (int i = 0; i < SemiInforSearch.Rows.Count; i++)
                        {
                            dgvSemi.Rows.Add(SemiInforSearch.Rows[i][0].ToString(), SemiInforSearch.Rows[i][1].ToString(), SemiInforSearch.Rows[i][2].ToString(), SemiInforSearch.Rows[i][3].ToString(), SemiInforSearch.Rows[i][4].ToString());

                        }
                        dgvSemi.ClearSelection();
                    }
                    dgvSemi.Focus();
                    
                }
                else
                {
                    MessageBox.Show("សូមបញ្ចូលឈ្មោះផលិតផលដើម្បីស្វែងរក!","Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                }
            }
            else
            {

            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtNGQty.Text.Trim() == "")
            {
                MessageBox.Show("សូមបញ្ចូល NG Qty !","Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
                txtNGQty.Focus();
            }
            else
            {
                DialogResult DLR = MessageBox.Show("តើអ្នករក្សាទុកទិន្នន័យនេះមែនទេ ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    DateTime RegNow = DateTime.Now;
                    string TransNo = "";
                    try
                    {
                        int NG = Convert.ToInt16(txtNGQty.Text.ToString());
                        //Calculate Total
                        for (int i = 0; i < SelectedConsump.Rows.Count; i++)
                        {
                            SelectedConsump.Rows[i][3] = Math.Round(Convert.ToDouble(NG) * Convert.ToDouble(SelectedConsump.Rows[i][2].ToString()), 3, MidpointRounding.AwayFromZero);
                            
                        }


                        //Compare remain Stock
                        cnn.con.Open();
                        string CompareString = "ឈ្មោះ  |  ចំនួនប្រើ  |  ស្តុកនៅសល់  |  ស្តុកខ្វះ";
                        int FoundNotEnoung = 0;
                        foreach (DataRow row in SelectedConsump.Rows)
                        {
                            SqlDataAdapter sda = new SqlDataAdapter("SELECT Code,StockValue FROM " +
                                "(SELECT Code, SUM(StockValue) AS StockValue  FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='MC1' AND POSNo='' " +
                                "GROUP BY Code) T1 " +
                                "WHERE StockValue>0 AND Code = '" + row[0].ToString() + "' ", cnn.con);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            if (dt.Rows.Count == 0)
                            {
                                FoundNotEnoung = FoundNotEnoung + 1;
                                CompareString = CompareString + "\n" + row[1] + "  |  " + row[3] + "  |  0  |  "+ row[3];
                            }
                            else
                            {
                                if (Convert.ToDouble(dt.Rows[0][1].ToString()) < Convert.ToDouble(row[3].ToString()))
                                {
                                    FoundNotEnoung = FoundNotEnoung + 1;
                                    CompareString = CompareString + "\n" + row[1] + "  |  " + row[3] + "  |  " + dt.Rows[0][1] + "  |  " + (Convert.ToDouble(row[3].ToString()) - Convert.ToDouble(dt.Rows[0][1].ToString())).ToString("N3");
                                }
                            }                            
                        }

                        if (FoundNotEnoung == 0)
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
                            for (int i = 0; i < SelectedConsump.Rows.Count; i++)
                            {
                                cmd = new SqlCommand("INSERT INTO tbNGHistory (SysNo, ItemCode, NGQty, ReqStat, RegDate, RegBy) VALUES (@Sn, @Cd, @NG, @Rs, @Rd, @Rb)", cnn.con);
                                cmd.Parameters.AddWithValue("@Sn", TransNo);
                                cmd.Parameters.AddWithValue("@Cd", SelectedConsump.Rows[i][0].ToString());
                                cmd.Parameters.AddWithValue("@NG", Convert.ToDouble(SelectedConsump.Rows[i][3].ToString()));
                                cmd.Parameters.AddWithValue("@Rs", 0);
                                cmd.Parameters.AddWithValue("@Rd", RegNow.ToString("yyyy-MM-dd HH:mm:ss"));
                                cmd.Parameters.AddWithValue("@Rb", Username);
                                cmd.ExecuteNonQuery();
                                SelectedConsump.Rows[i][4] = TransNo;

                            }

                            foreach (DataRow row in SelectedConsump.Rows)
                            {
                                //Add to tbSDMCAllTransaction
                                cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction (SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus) VALUES (@Sn, @Fc, @Lc, @POSN, @Cd, @Rmd, @Rqty, @Tqty, @SV, @Rd, @Rb, @Cs)", cnn.con);
                                cmd.Parameters.AddWithValue("@Sn", TransNo);
                                cmd.Parameters.AddWithValue("@Fc", 6);
                                cmd.Parameters.AddWithValue("@Lc", "MC1");
                                cmd.Parameters.AddWithValue("@POSN", "");
                                cmd.Parameters.AddWithValue("@Cd", row[0].ToString());
                                cmd.Parameters.AddWithValue("@Rmd", row[1].ToString());
                                cmd.Parameters.AddWithValue("@Rqty", 0);
                                cmd.Parameters.AddWithValue("@Tqty", Convert.ToDouble(row[3].ToString()));
                                cmd.Parameters.AddWithValue("@SV", Convert.ToDouble(row[3].ToString()) * (-1));
                                cmd.Parameters.AddWithValue("@Rd", RegNow.ToString("yyyy-MM-dd HH:mm:ss"));
                                cmd.Parameters.AddWithValue("@Rb", Username);
                                cmd.Parameters.AddWithValue("@Cs", 0);
                                cmd.ExecuteNonQuery();
                            }

                            //Add to Parent form
                            for (int i = 0; i < SelectedConsump.Rows.Count; i++)
                            {
                                fgrid.dgvNGalready.Rows.Add(SelectedConsump.Rows[i][0].ToString(), SelectedConsump.Rows[i][1].ToString(), Convert.ToDouble(SelectedConsump.Rows[i][3].ToString()), false, DateTime.Now, Username, SelectedConsump.Rows[i][4].ToString());
                            }
                            fgrid.dgvNGalready.ClearSelection();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("ស្តុកវត្ថុធាតុដើមមិនគ្រប់គ្រាន់ ៖ \n"+CompareString,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ចំនួន <NG Qty> ត្រូវតែជាលេខចំនួនគត់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtNGQty.Focus();
                        txtNGQty.SelectAll();
                    }
                    cnn.con.Close();
                }
            }
        }

        private void NGInputBySetForm_Load(object sender, EventArgs e)
        {
            SelectedConsump = new DataTable();
            SelectedConsump.Columns.Add("Code", typeof(string));
            SelectedConsump.Columns.Add("Name", typeof(string));
            SelectedConsump.Columns.Add("Qty", typeof(double));
            SelectedConsump.Columns.Add("TotalQty", typeof(double));
            SelectedConsump.Columns.Add("SysN", typeof(string));
            Username = MenuFormV2.UserForNextForm;

        }

    }
}
