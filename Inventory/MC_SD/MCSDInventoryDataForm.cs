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
using System.Diagnostics;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Controls;

namespace MachineDeptApp.Inventory.MC_SD
{
    public partial class MCSDInventoryDataForm : Form
    {
        SQLConnect con = new SQLConnect();

        public MCSDInventoryDataForm()
        {
            InitializeComponent();
            this.con.Connection();
            this.btnSearch.Click += BtnSearch_Click;
            this.dgvSearchResult.CellFormatting += DgvSearchResult_CellFormatting;
            this.txtCode.TextChanged += TxtCode_TextChanged;
            this.txtLabelNo.TextChanged += TxtLabelNo_TextChanged;
            this.txtName.TextChanged += TxtName_TextChanged;
        }

        private void TxtName_TextChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

        private void TxtLabelNo_TextChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

        private void TxtCode_TextChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

        private void DgvSearchResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = dgvSearchResult.Rows[e.RowIndex]; 
            if (row.Cells["status"].Value != null && row.Cells["status"].Value.ToString() == "Deleted") 
            { 
                row.DefaultCellStyle.BackColor = Color.Pink; 
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            dgvSearchResult.Rows.Clear();
            DataTable Cond = new DataTable();
            Cond.Columns.Add("cond");
            Cursor = Cursors.WaitCursor;
            if (!string.IsNullOrEmpty(txtLabelNo.Text.Trim()))
            {
                Cond.Rows.Add("LabelNo LIKE '%" + txtLabelNo.Text.Trim() + "%'");
            }
            if (!string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                Cond.Rows.Add("Code LIKE '%" + txtCode.Text.Trim() + "%'");
            }
            if (!string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                Cond.Rows.Add("RMName LIKE '%" + txtName.Text.Trim() + "%'");
            }
            string WHERE = "";
            foreach (DataRow row in Cond.Rows)
            {
                if (WHERE == "")
                {
                    WHERE = " WHERE " + row["cond"].ToString();
                }
                else
                {
                    WHERE += " AND " + row["cond"].ToString();
                }
            }
            DataTable dtselect = new DataTable();
            try
            {
                string selectquery = "SELECT * FROM tbSDMCStockInventory " + WHERE;
                SqlDataAdapter sdaselect = new SqlDataAdapter(selectquery, con.con);
                sdaselect.Fill(dtselect);
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 1 !" + ex.Message, "Error SearchAll'", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            foreach (DataRow row in dtselect.Rows)
            {
                dgvSearchResult.Rows.Add();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["lbno"].Value = row["LabelNo"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["code"].Value = row["Code"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["name"].Value = row["RMName"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["ttlweight"].Value = row["TotalWeight"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["bobinw"].Value = row["BobinWeight"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["bobinQty"].Value = row["BobbinQty"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["Qty"].Value = row["Qty"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["status"].Value = row["Status"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["regdate"].Value = row["RegDate"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["pic"].Value = row["PIC"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["updatedate"].Value = row["UpdateDate"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["regby"].Value = row["UpdateBy"].ToString();
            }
            dgvSearchResult.ClearSelection();
            con.con.Close();
            Cursor = Cursors.Default;
        }
    }
}
