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

namespace MachineDeptApp
{
    public partial class ReportNGList : Form
    {
        SQLConnect con = new SQLConnect();
        public ReportNGList()
        {
            InitializeComponent();
            this.con.Connection();
            this.btnSearchExport.Click += BtnSearchExport_Click;
        }

        private void BtnSearchExport_Click(object sender, EventArgs e)
        {
            con.con.Open();
            try
            {
                dgvList.Rows.Clear();
                DataTable dtsearch = new DataTable();
                string query = "SELECT * FROM tbNGTypeDetails";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dtsearch);

                foreach (DataRow row in dtsearch.Rows)
                {
                    dgvList.Rows.Add();
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["sysno"].Value = row["SysNo"].ToString();
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["posc"].Value = row["POSC"].ToString();
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["code"].Value = row["ItemCode"].ToString();
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["stopinfo"].Value = row["StopInfo"].ToString();
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["type"].Value = row["Type"].ToString();
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["qty"].Value = Convert.ToDouble(row["Qty"]);
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["pic"].Value = row["PIC"].ToString();
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["regdate"].Value = Convert.ToDateTime(row["RegDate"]);
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["regby"].Value = row["RegBy"].ToString();
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["update"].Value = Convert.ToDateTime(row["UpdateDate"]);
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["upby"].Value = row["UpdateBy"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong! Please contact Phanun\n" + ex.Message, "Something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.con.Close();
            dgvList.ClearSelection();
        }
    }
}
