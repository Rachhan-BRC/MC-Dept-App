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
    public partial class MstSparePart : Form
    {
        SQLConnect con = new SQLConnect();
        string dept = "MC";
        
        public MstSparePart()
        {
            con.Connection();
            InitializeComponent();
            this.Load += MstSparePart_Load;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnAdd.Click += BtnAdd_Click;
            this.btnUpdate.Click += BtnUpdate_Click;
            this.dgvMst.CurrentCellChanged += DgvMst_CurrentCellChanged;
            this.btnDelete.Click += BtnDelete_Click;
            //txt
            this.txtcode.TextChanged += Txtcode_TextChanged;
            this.txtPname.TextChanged += TxtPname_TextChanged;
            this.txtPno.TextChanged += TxtPno_TextChanged;
        }

        private void MstSparePart_Load(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

        private void TxtPno_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPno.Text))
            {
                chkPno.Checked = true;
            }
            else
            {
                chkPno.Checked = false;
            }
            btnSearch.PerformClick();
        }

        private void TxtPname_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPname.Text))
            {
                chkPname.Checked = true;
            }
            else
            {
                chkPname.Checked = false;
            }
            btnSearch.PerformClick();
        }

        private void Txtcode_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtcode.Text))
            {
                chkcode.Checked = true;
               
            }
            else
            {
                chkcode.Checked = false;
            }
            btnSearch.PerformClick();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult ask = MessageBox.Show("Are you sure to delete this record ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask == DialogResult.No)
            {
                return;
            }
            if (dgvMst.SelectedCells.Count > 0)
            {
                Cursor = Cursors.WaitCursor;
                try
                {
                    string code = dgvMst.Rows[dgvMst.CurrentCell.RowIndex].Cells["code"].Value.ToString();
                    con.con.Open();
                    string query = "DELETE FROM MstMCSparePart WHERE Code = @code AND Dept = '" +dept+ "'";
                    SqlCommand cmd = new SqlCommand(query, con.con);
                    cmd.Parameters.AddWithValue("@code" , code);
                    cmd.ExecuteNonQuery();
                    dgvMst.Rows.RemoveAt(dgvMst.CurrentCell.RowIndex);
                    MessageBox.Show("Delete successfully !", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error while searching" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                con.con.Close();
                Cursor = Cursors.Default;
            }
            btnSearch.PerformClick();
        }

        private void DgvMst_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dgvMst.SelectedCells.Count > 0 && dgvMst.CurrentCell != null && dgvMst.CurrentCell.RowIndex >= 0 && dgvMst.CurrentCell.ColumnIndex >= 0)
            {
                btnUpdate.BringToFront();
                btnDelete.BringToFront();
                this.btnUpdate.Enabled = true;
                this.btnDelete.Enabled = true;
            }
            else
            {
                btnUpdateGrey.BringToFront();
                btnDeleteGray.BringToFront();
                this.btnUpdate.Enabled = false;
                this.btnDelete.Enabled = false;
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvMst.SelectedCells.Count > 0 && dgvMst.CurrentCell != null)
            {
                RegUpdateForm Up = new RegUpdateForm("Update", dgvMst);
                Up.ShowDialog();
                btnSearch.PerformClick();
            }
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            RegUpdateForm Reg = new RegUpdateForm("Add", dgvMst);
            Reg.ShowDialog();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            dgvMst.Rows.Clear();
            DataTable conds = new DataTable();
            conds.Columns.Add("Val");
            if (chkcode.Checked == true)
            {
                string code = txtcode.Text.Trim();
                conds.Rows.Add("Code LIKE '%" + code + "%'");
            }
            if (chkPname.Checked == true)
            {
                string name = txtPname.Text.Trim();
                conds.Rows.Add("Part_Name LIKE '%" + name + "%'");
            }
            if (chkPno.Checked == true)
            {
                string no = txtPno.Text.Trim();
                conds.Rows.Add("Part_No LIKE '%" + no + "%'");
            }
            string where = "";

            foreach (DataRow row in conds.Rows)
            {
                if (where == "")
                {
                    where = " AND " + row["Val"].ToString();
                }
                else
                {
                    where += " AND " + row["Val"].ToString();
                }
            }

            DataTable dtsearch = new DataTable();
            try
            {
                con.con.Open();
                string query = "SELECT * FROM MstMCSparePart WHERE Dept = '"+dept+"' " + where;
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dtsearch);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error while searching" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            con.con.Close();
            foreach (DataRow row in dtsearch.Rows)
            {
                string code = row["Code"]?.ToString() ?? "";
                string Pno = row["Part_No"]?.ToString() ?? "";
                string Pname = row["Part_Name"]?.ToString() ?? "";
                string Category = row["Category"]?.ToString() ?? "";
                string Maker = row["Maker"]?.ToString() ?? "";
                string use = row["Use_For"]?.ToString() ?? "";
                string Supplier = row["Supplier"]?.ToString() ?? "";

                double safetystock = row["Safety_Stock"] == DBNull.Value ? 0 : Convert.ToDouble(row["Safety_Stock"]);
                double LT = row["Lead_Time_Week"] == DBNull.Value ? 0 : Convert.ToDouble(row["Lead_Time_Week"]);
                double Moq = row["MOQ"] == DBNull.Value ? 0 : Convert.ToDouble(row["MOQ"]);
                double Up = row["Unit_Price"] == DBNull.Value ? 0 : Convert.ToDouble(row["Unit_Price"]);
                double UpCn = row["Unit_Price_CN"] == DBNull.Value ? 0 : Convert.ToDouble(row["Unit_Price_CN"]);
                double UpJP = row["Unit_Price_JP"] == DBNull.Value ? 0 : Convert.ToDouble(row["Unit_Price_JP"]);
                string box = row["Box"]?.ToString() ?? "";
                string status = row["Status"]?.ToString() ?? "";
                dgvMst.Rows.Add();
                dgvMst.Rows[dgvMst.Rows.Count -1].Cells["code"].Value = code;
                dgvMst.Rows[dgvMst.Rows.Count - 1].Cells["Pno"].Value = Pno;
                dgvMst.Rows[dgvMst.Rows.Count - 1].Cells["Pname"].Value = Pname;
                dgvMst.Rows[dgvMst.Rows.Count - 1].Cells["category"].Value = Category;
                dgvMst.Rows[dgvMst.Rows.Count - 1].Cells["maker"].Value = Maker;
                dgvMst.Rows[dgvMst.Rows.Count - 1].Cells["usefor"].Value = use;
                dgvMst.Rows[dgvMst.Rows.Count - 1].Cells["supplier"].Value = Supplier;
                dgvMst.Rows[dgvMst.Rows.Count - 1].Cells["safetystock"].Value = safetystock;
                dgvMst.Rows[dgvMst.Rows.Count - 1].Cells["LTWeek"].Value = LT;
                dgvMst.Rows[dgvMst.Rows.Count - 1].Cells["moq"].Value = Moq;
                dgvMst.Rows[dgvMst.Rows.Count - 1].Cells["unitprice"].Value = Up;
                dgvMst.Rows[dgvMst.Rows.Count - 1].Cells["unitpricecn"].Value = UpCn;
                dgvMst.Rows[dgvMst.Rows.Count - 1].Cells["unitpricejp"].Value = UpJP;
                dgvMst.Rows[dgvMst.Rows.Count - 1].Cells["box"].Value = box;
                dgvMst.Rows[dgvMst.Rows.Count - 1].Cells["status"].Value = status;
            }
            dgvMst.ClearSelection();
            Cursor = Cursors.Default;
            lbFound.Text = "Found: " + dgvMst.Rows.Count;
        }

    }
}
