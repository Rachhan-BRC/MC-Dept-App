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

namespace MachineDeptApp.AllSection
{
    public partial class AllSectionMCOverviewFormStock : Form
    {
        SQLConnectAllSection cnnAll = new SQLConnectAllSection();

        public AllSectionMCOverviewFormStock()
        {
            InitializeComponent();
            this.cnnAll.Connection();
            this.Load += AllSectionMCOverviewFormStock_Load;
        }

        private void AllSectionMCOverviewFormStock_Load(object sender, EventArgs e)
        {
            this.Text = AllSectionMCOverviewForm.PosNo + " | " + AllSectionMCOverviewForm.Code + " | " + AllSectionMCOverviewForm.FGName;
            try
            {
                cnnAll.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM " +
                                                                                    "(SELECT POS_No, ItemCode, LocCode, SUM(RealValue) AS TOTAL FROM tbAllTransaction WHERE CancelStatus=0 GROUP BY POS_No, ItemCode, LocCode) T1 " +
                                                                                    "WHERE T1.TOTAL>0 AND POS_No='"+ AllSectionMCOverviewForm.PosNo + "' AND ItemCode='"+ AllSectionMCOverviewForm.Code + "' ", cnnAll.con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    dgvScannedTag.Rows.Add(row[2], Convert.ToDouble(row[3].ToString()));
                }
                dgvScannedTag.ClearSelection();
            }
            catch(Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n"+ex.Message,"Rachhan System", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            cnnAll.con.Close();
        }
    }
}
