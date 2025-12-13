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

namespace MachineDeptApp.SparePartControll
{
    public partial class AddPrint : Form
    {
        SQLConnect con = new SQLConnect();
        DataTable dtSearch = new DataTable();
        DataGridView dgvPrint;
        string dept = "MC";
        public AddPrint(DataGridView dgvprint)
        {
            dgvPrint = dgvprint;
            this.con.Connection();
            InitializeComponent();
            this.Load += AddPrint_Load;
            this.txtSearch.TextChanged += TxtSearch_TextChanged;
            this.btnMstRM.Click += BtnMstRM_Click;
            this.btnSearch.Click += BtnSearch_Click;
            this.dgvmst.CellClick += Dgvmst_CellClick;
            this.dgvSearch.CellClick += DgvSearch_CellClick;
            this.txtqty.TextChanged += Txtqty_TextChanged;
            this.btnAddtoDgv.Click += BtnAddtoDgv_Click;
            //
            this.txtqty.KeyPress += Txtqty_KeyPress;
            this.txtleadtime.KeyPress += Txtleadtime_KeyPress;
            this.txtmoq.KeyPress += Txtmoq_KeyPress;
            this.txtunitprice.KeyPress += Txtunitprice_KeyPress;
        }

        private void Txtunitprice_KeyPress(object sender, KeyPressEventArgs e)
        {  // Allow control keys (like Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // block non-numeric input
            }
        }

        private void Txtmoq_KeyPress(object sender, KeyPressEventArgs e)
        {  // Allow control keys (like Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // block non-numeric input
            }
        }

        private void Txtleadtime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // block non-numeric input
            }
        }

        private void Txtqty_KeyPress(object sender, KeyPressEventArgs e)
        {  // Allow control keys (like Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // block non-numeric input
            }
        }

        private void BtnAddtoDgv_Click(object sender, EventArgs e)
        {
            string code = txtcode.Text;
            string partNumber = txtpnumber.Text;
            string partName = txtpname.Text;
            string machineName = txtmachinename.Text;
            string supplier = cbsupplier.Text;
            string maker = cbmaker.Text;
            string leadTime = txtleadtime.Text;
            string unitPrice = txtunitprice.Text;
            string moq = txtmoq.Text;
            string qty = txtqty.Text;
            string eta = dtpeta.Value.ToString("yyyy-MM-dd");
            string amount = txteta.Text;
            if (!string.IsNullOrWhiteSpace(code) &&
    !string.IsNullOrWhiteSpace(partNumber) &&
    !string.IsNullOrWhiteSpace(partName) &&
    !string.IsNullOrWhiteSpace(machineName) &&
    !string.IsNullOrWhiteSpace(supplier) &&
    !string.IsNullOrWhiteSpace(maker) &&
    !string.IsNullOrWhiteSpace(unitPrice) &&
    !string.IsNullOrWhiteSpace(qty) &&
    !string.IsNullOrWhiteSpace(eta) &&
    !string.IsNullOrWhiteSpace(amount))
            {
                dgvPrint.Rows.Add();
                dgvPrint.Rows[dgvPrint.Rows.Count - 1].Cells["Code"].Value = code;
                dgvPrint.Rows[dgvPrint.Rows.Count - 1].Cells["partno"].Value = partNumber;
                dgvPrint.Rows[dgvPrint.Rows.Count - 1].Cells["partname"].Value = partName;
                dgvPrint.Rows[dgvPrint.Rows.Count - 1].Cells["machinename"].Value = machineName;
                dgvPrint.Rows[dgvPrint.Rows.Count - 1].Cells["supplier"].Value = supplier;
                dgvPrint.Rows[dgvPrint.Rows.Count - 1].Cells["maker"].Value = maker;
                dgvPrint.Rows[dgvPrint.Rows.Count - 1].Cells["unitprice"].Value = unitPrice;
                dgvPrint.Rows[dgvPrint.Rows.Count - 1].Cells["amount"].Value = amount;
                dgvPrint.Rows[dgvPrint.Rows.Count - 1].Cells["qty"].Value = Convert.ToDouble(qty).ToString("N0");
                dgvPrint.Rows[dgvPrint.Rows.Count - 1].Cells["eta"].Value = eta;
                dgvPrint.Rows[dgvPrint.Rows.Count - 1].Cells["leadtime"].Value = leadTime;
                dgvPrint.Rows[dgvPrint.Rows.Count - 1].Cells["find"].Value = "Not";
                txtqty.Text = "";
                txteta.Text = "";
            }
        }

        private void Txtqty_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtunitprice.Text) &&
    double.TryParse(txtunitprice.Text, out double unitPrice)&&
    !string.IsNullOrWhiteSpace(txtqty.Text) &&
    double.TryParse(txtqty.Text, out double qty))
            {
                double total = unitPrice * qty;
                string totalFormatted = total.ToString("N2");
                txteta.Text = totalFormatted;
            }
            else
            {
                txteta.Text = "";
            }

        }

        private void DgvSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cellValue = dgvSearch.Rows[e.RowIndex].Cells["code"].Value;
                if (cellValue != null)
                {
                    try
                    {
                        con.con.Open();
                        string query = "SELECT * FROM MstMCSparePart WHERE Code = @Code AND Dept = '" + dept +"'";
                        SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                        sda.SelectCommand.Parameters.AddWithValue("@Code", cellValue);

                        DataTable dtsearch = new DataTable();
                        sda.Fill(dtsearch);
                        con.con.Close();
                        foreach (DataRow row in dtsearch.Rows)
                        {
                            txtcode.Text = row["Code"].ToString();
                            txtpnumber.Text = row["Part_No"].ToString();
                            txtpname.Text = row["Part_Name"].ToString();
                            txtmachinename.Text = row["Use_For"].ToString();
                            cbsupplier.Text = row["Supplier"].ToString();
                            cbmaker.Text = row["Maker"].ToString();
                            txtleadtime.Text = row["Lead_Time_Week"] != DBNull.Value ? row["Lead_Time_Week"].ToString() : "";
                            txtunitprice.Text = row["Unit_price"] != DBNull.Value ? Convert.ToDouble(row["Unit_Price"]).ToString("N2") : "";
                            txtmoq.Text = row["MOQ"] != DBNull.Value ? row["MOQ"].ToString() : "";
                            double un = 0;
                            if (row["Unit_Price"] != DBNull.Value)
                            {
                                un = Convert.ToDouble(row["Unit_Price"]);
                            }
                            // Safely get lead time in weeks
                            int leadWeeks = 0;
                            if (row["Lead_Time_Week"] != DBNull.Value)
                            {
                                int.TryParse(row["Lead_Time_Week"].ToString(), out leadWeeks);
                            }
                            int months = leadWeeks / 4;
                            DateTime now = DateTime.Now.AddMonths(months);
                            dtpeta.Value = now;



                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void Dgvmst_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Example: get the value of the clicked cell
                var cellValue = dgvmst.Rows[e.RowIndex].Cells["partnumber1"].Value;
                if (cellValue != null)
                {
                    txtSearch.Text = cellValue.ToString();
                    dgvmst.Visible = false;
                    dgvmst.ClearSelection();
                }
            }
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            dgvSearch.Rows.Clear();
            try
            {
                DataTable dtDgv = new DataTable();
                con.con.Open();
                string query = "SELECT * FROM MstMCSparePart WHERE Dept = '"+dept+"' AND  Part_Name LIKE '%" + txtSearch.Text.Trim() + "%'";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                Console.WriteLine(query);
                sda.Fill(dtDgv);
                foreach (DataRow row in dtDgv.Rows)
                {
                    string code = row["Code"].ToString();
                    string partName = row["Part_Name"].ToString();
                    dgvSearch.Rows.Add();
                    dgvSearch.Rows[dgvSearch.Rows.Count - 1].Cells["code"].Value = code;
                    dgvSearch.Rows[dgvSearch.Rows.Count - 1].Cells["partname"].Value = partName;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error while search data !"+ ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            con.con.Close();
            dgvSearch.ClearSelection();
            Cursor = Cursors.Default;
        }
        private void BtnMstRM_Click(object sender, EventArgs e)
        {
            if (dgvmst.Visible == false)
            {
                dgvmst.Visible = true;
            }
            else
            {
                dgvmst.Visible = false;
                return;
            }
                try
                {
                    foreach (DataRow row1 in dtSearch.Rows)
                    {
                        string code = row1["Code"].ToString();
                        string partName = row1["Part_Name"].ToString();
                        dgvmst.Rows.Add(code, partName);
                    }
                }
                catch
                {
                    MessageBox.Show("Error while fill data !", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            dgvmst.ClearSelection();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim() != "")
            {
                btnSearch.PerformClick();
            }
            else
            {
                dgvSearch.Rows.Clear();
            }
            
        }
        private void AddPrint_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                con.con.Open();
                string query = "SELECT Code, Part_Name FROM MstMCSparePart WHERE Dept = '"+dept+"'";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dtSearch);
            }
            catch
            {
                MessageBox.Show("Error while selecting data !", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            con.con.Close();
            Cursor = Cursors.Default;
        }

    }
}
