using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.NG_Input
{
    public partial class MstBOMForm : Form
    {
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SQLConnect cnn = new SQLConnect();
        DataTable SemiInforSearch;
        DataTable SemiConsumpSearch;
        DataTable CurrentBOM;
        DataTable NewBOM;
        DataTable MaxUpdateSemi;
        SqlCommand cmd;
        string ErrorStat;

        public MstBOMForm()
        {
            InitializeComponent();
            progressBar1.Visible = false;
            cnn.Connection();
            cnnOBS.Connection();
            this.dgvSemi.CellClick += DgvSemi_CellClick;
            this.btnSearch.Click += BtnSearch_Click;
            this.Load += MstBOMForm_Load;

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LbExportStatus.Visible = true;
            LbExportStatus.Text = "កំពុងស្វែងរក​ . . . .";
            LbExportStatus.Refresh();
            dgvSemi.Rows.Clear();
            dgvConsump.Rows.Clear();
            if (txtItemname.Text.Trim() != "")
            {
                //Take Semi Inform
                try
                {
                    cnn.con.Open();
                    //SqlDataAdapter da = new SqlDataAdapter("Select * From (Select UpItemCode, Max(UpdateDate) as LastUpdate From MstBOM Group By UpItemCode)t1 INNER JOIN (Select * From tbMasterItem Where LEN(ItemCode)>4  AND ItemName Like '%" + txtItemname.Text.ToString()+"%') t2 ON t1.UpItemCode=t2.ItemCode", cnn.con);
                    SqlDataAdapter da = new SqlDataAdapter("Select UpItemCode, ItemName, Remarks1, Remarks2, Remarks3, LastUpdate From " +
                                                                                    "(Select UpItemCode, Max(UpdateDate) as LastUpdate From MstBOM Group By UpItemCode) t1 " +
                                                                                    "INNER JOIN(Select ItemCode, ItemName, Remarks1, Remarks2, Remarks3 From tbMasterItem Where LEN(ItemCode) > 4  AND ItemName Like '%" + txtItemname.Text.ToString() + "%') t2 " +
                                                                                    "ON t1.UpItemCode = t2.ItemCode", cnn.con);
                    SemiInforSearch = new DataTable();
                    da.Fill(SemiInforSearch);
                    SqlDataAdapter da1 = new SqlDataAdapter("Select * From (Select UpItemCode, LowItemCode, (LowQty*SemiQtyOfFG) as TotalUse, Yield From MstBOM) t1 INNER JOIN (Select ItemCode, ItemName From tbMasterItem Where LEN(ItemCode)<5) t2 ON t1.LowItemCode=t2.ItemCode", cnn.con);
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
                        dgvSemi.Rows.Add(SemiInforSearch.Rows[i][0].ToString(), SemiInforSearch.Rows[i][1].ToString(), SemiInforSearch.Rows[i][2].ToString(), SemiInforSearch.Rows[i][3].ToString(), SemiInforSearch.Rows[i][4].ToString(), Convert.ToDateTime(SemiInforSearch.Rows[i][5].ToString()));

                    }
                    dgvSemi.ClearSelection();
                }

            }
            else
            {
                //Take Semi Inform
                try
                {
                    cnn.con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select UpItemCode, ItemName, Remarks1, Remarks2, Remarks3, LastUpdate From (Select UpItemCode, Max(UpdateDate) as LastUpdate From MstBOM Group By UpItemCode) t1 " +
                                                                                "INNER JOIN (Select ItemCode, ItemName,Remarks1, Remarks2, Remarks3 From tbMasterItem Where LEN(ItemCode)>4) t2 " +
                                                                                "ON t1.UpItemCode=t2.ItemCode", cnn.con);
                    SemiInforSearch = new DataTable();
                    da.Fill(SemiInforSearch);
                    SqlDataAdapter da1 = new SqlDataAdapter("Select * From (Select UpItemCode, LowItemCode, (LowQty*SemiQtyOfFG) as TotalUse, Yield From MstBOM) t1 INNER JOIN (Select ItemCode, ItemName From tbMasterItem Where LEN(ItemCode)<5) t2 ON t1.LowItemCode=t2.ItemCode", cnn.con);
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
                        dgvSemi.Rows.Add(SemiInforSearch.Rows[i][0].ToString(), SemiInforSearch.Rows[i][1].ToString(), SemiInforSearch.Rows[i][2].ToString(), SemiInforSearch.Rows[i][3].ToString(), SemiInforSearch.Rows[i][4].ToString(), Convert.ToDateTime(SemiInforSearch.Rows[i][5].ToString()));

                    }
                    dgvSemi.ClearSelection();
                }
            }

            LbExportStatus.Text = "រកឃើញ " + String.Format("{0:N0}", dgvSemi.Rows.Count);
        }

        private void DgvSemi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string SemiSelected = "";
            dgvConsump.Rows.Clear();
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvSemi.Rows[e.RowIndex];
                SemiSelected = row.Cells[0].Value.ToString();
                for (int i = 0; i < SemiConsumpSearch.Rows.Count; i++)
                {
                    if (SemiConsumpSearch.Rows[i][0].ToString() == SemiSelected)
                    {
                        dgvConsump.Rows.Add(SemiConsumpSearch.Rows[i][1].ToString(), SemiConsumpSearch.Rows[i][5].ToString(), Convert.ToDouble(SemiConsumpSearch.Rows[i][2].ToString()), Convert.ToDouble("100") - Convert.ToDouble(SemiConsumpSearch.Rows[i][3]));
                    }

                }
                dgvConsump.ClearSelection();

            }
            else
            {

            }
        }

        private void MstBOMForm_Load(object sender, EventArgs e)
        {
            //Retrive Current BOM
            try
            {
                cnn.con.Open();
                SqlDataAdapter da = new SqlDataAdapter("Select UpItemCode From MstBOM Group By UpItemCode;", cnn.con);
                CurrentBOM = new DataTable();
                da.Fill(CurrentBOM);
                SqlDataAdapter da1 = new SqlDataAdapter("Select MAX(UpdateDate) As Udate From MstBOM;", cnn.con);
                MaxUpdateSemi = new DataTable();
                da1.Fill(MaxUpdateSemi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 100; i++)
            {
                Thread.Sleep(10);
                backgroundWorker1.WorkerReportsProgress = true;
                backgroundWorker1.ReportProgress(i);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Show();
            progressBar1.Value = e.ProgressPercentage;
            LbPercent.Text = e.ProgressPercentage.ToString() + " % ";
        }


    }
}
