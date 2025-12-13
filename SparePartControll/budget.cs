using MachineDeptApp.Inventory.Inprocess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace MachineDeptApp
{
    public partial class budget : Form
    {
        SQLConnect con = new SQLConnect();
        string dept = "MC";
       
        public budget()
        {
            this.con.Connection();
            InitializeComponent();
            this.btnSearch.Click += BtnSearch_Click;
            this.dgvBudget.CellClick += DgvBudget_CellClick;
            this.btnUpdate.Click += BtnUpdate_Click;
            this.btnSave.Click += BtnSave_Click;
            dgvBudget.EditingControlShowing += DgvBudget_EditingControlShowing;
            this.dgvBudget.CellEndEdit += DgvBudget_CellEndEdit;
        }

        private void DgvBudget_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string end = dgvBudget.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString()?? "" ;
            if (end.Trim() == "")
            {
                dgvBudget.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DialogResult ask = MessageBox.Show("Are you sure you want to update? ", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask == DialogResult.No)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;
            int err = 0;
            try
            {
                foreach (DataGridViewRow row1 in dgvBudget.Rows)
            {
                string year = row1.Cells["budgetyear"].Value.ToString();
                string type = row1.Cells["budgettype"].Value.ToString();
                double jan = Convert.ToDouble(row1.Cells["Jan"].Value);
                double feb = Convert.ToDouble(row1.Cells["Feb"].Value);
                double mar = Convert.ToDouble(row1.Cells["Mar"].Value);
                double apr = Convert.ToDouble(row1.Cells["Apr"].Value);
                double may = Convert.ToDouble(row1.Cells["May"].Value);
                double jun = Convert.ToDouble(row1.Cells["Jun"].Value);
                double jul = Convert.ToDouble(row1.Cells["Jul"].Value);
                double aug = Convert.ToDouble(row1.Cells["Aug"].Value);
                double sep = Convert.ToDouble(row1.Cells["Sep"].Value);
                double oct = Convert.ToDouble(row1.Cells["Oct"].Value);
                double nov = Convert.ToDouble(row1.Cells["Nov"].Value);
                double dec = Convert.ToDouble(row1.Cells["Dec"].Value);
                    try
                    {
                        con.con.Open();
                        string query = "UPDATE SparePartBudget\r " +
                       " SET " +
                       "Budget_Year = @Year, " +
                       "Jan = @Jan," +
                       " Feb = @Feb, " +
                       "Mar = @Mar,    " +
                       "Apr = @Apr,   " +
                       "May = @May, " +
                       "Jun = @Jun,  " +
                       "Jul = @Jul, " +
                       "Aug = @Aug, " +
                       "Sep = @Sep, " +
                       "Oct = @Oct, " +
                       "Nov = @Nov, " +
                       "Dec = @Dec " +
                       "WHERE " +
                       "Budget_Dept = @Dept " +
                       "AND Budget_Type = @Type; ";
                        SqlCommand cmd = new SqlCommand(query, con.con);
                        cmd.Parameters.AddWithValue("@Jan", jan);
                        cmd.Parameters.AddWithValue("@Feb", feb);
                        cmd.Parameters.AddWithValue("@Mar", mar);
                        cmd.Parameters.AddWithValue("@Apr", apr);
                        cmd.Parameters.AddWithValue("@May", may);
                        cmd.Parameters.AddWithValue("@Jun", jun);
                        cmd.Parameters.AddWithValue("@Jul", jul);
                        cmd.Parameters.AddWithValue("@Aug", aug);
                        cmd.Parameters.AddWithValue("@Sep", sep);
                        cmd.Parameters.AddWithValue("@Oct", oct);
                        cmd.Parameters.AddWithValue("@Nov", nov);
                        cmd.Parameters.AddWithValue("@Dec", dec);
                        cmd.Parameters.AddWithValue("@Dept", dept);
                        cmd.Parameters.AddWithValue("@Year", year);
                        cmd.Parameters.AddWithValue("@Type", type);
                        cmd.ExecuteNonQuery();
                        con.con.Close();
                    }
                    catch (Exception ex)
                    {
                        err++;
                        MessageBox.Show("Error while updating budget1 !" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Cursor = Cursors.Default;
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error while updating budget2 !" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                err++;
            }
            if (err== 0)
            {
                MessageBox.Show("Update successfully !", "Done.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
                btnSaveGrey.BringToFront();
                btnUpdate.Enabled = false;
                btnUpdateGrey.BringToFront();
                btnSearch.PerformClick();
            }
            Cursor = Cursors.Default;
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            dgvBudget.Rows[dgvBudget.CurrentCell.RowIndex].Cells["budgetyear"].ReadOnly = false;
            dgvBudget.Rows[dgvBudget.CurrentCell.RowIndex].Cells["budgetyear"].ReadOnly = false;
            dgvBudget.Rows[dgvBudget.CurrentCell.RowIndex].Cells["Jan"].ReadOnly = false;
            dgvBudget.Rows[dgvBudget.CurrentCell.RowIndex].Cells["Feb"].ReadOnly = false;
            dgvBudget.Rows[dgvBudget.CurrentCell.RowIndex].Cells["Mar"].ReadOnly = false;
            dgvBudget.Rows[dgvBudget.CurrentCell.RowIndex].Cells["Apr"].ReadOnly = false;
            dgvBudget.Rows[dgvBudget.CurrentCell.RowIndex].Cells["May"].ReadOnly = false;
            dgvBudget.Rows[dgvBudget.CurrentCell.RowIndex].Cells["Jun"].ReadOnly = false;
            dgvBudget.Rows[dgvBudget.CurrentCell.RowIndex].Cells["Jul"].ReadOnly = false;
            dgvBudget.Rows[dgvBudget.CurrentCell.RowIndex].Cells["Aug"].ReadOnly = false;
            dgvBudget.Rows[dgvBudget.CurrentCell.RowIndex].Cells["Sep"].ReadOnly = false;
            dgvBudget.Rows[dgvBudget.CurrentCell.RowIndex].Cells["Oct"].ReadOnly = false;
            dgvBudget.Rows[dgvBudget.CurrentCell.RowIndex].Cells["Nov"].ReadOnly = false;
            dgvBudget.Rows[dgvBudget.CurrentCell.RowIndex].Cells["Dec"].ReadOnly = false;

            dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgetyear"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgettype"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Jan"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Feb"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Mar"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Apr"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["May"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Jun"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Jul"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Aug"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Sep"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Oct"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Nov"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Dec"].ReadOnly = true;
            dgvBudget.BeginEdit(true);
        }

        private void DgvBudget_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                btnUpdate.Enabled = true;
                btnUpdate.BringToFront();
            }

        }
        private void OnlyDigits(object sender, KeyPressEventArgs e)
        {
            btnSave.Enabled = true;
            btnSave.BringToFront();
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void DgvBudget_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox tb &&
               dgvBudget.CurrentCell != null &&
               dgvBudget.CurrentCell.ColumnIndex >= 0 )
            {
                tb.KeyPress -= OnlyDigits;
                tb.KeyPress += OnlyDigits;
            }
        }

        //taking data from database
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            dgvBudget.Rows.Clear();
            DataTable dtbudget = new DataTable();
            DataTable dtVal = new DataTable();
            Cursor = Cursors.WaitCursor;
            Cursor = Cursors.WaitCursor;
            try
            {
                con.con.Open();
                string query = "SELECT FORMAT(YearMonth, 'yyyy-MM') AS YearMonth,SUM(TotalRemain) AS TotalValue" +
                    " FROM (SELECT Receive_Date AS YearMonth, SUM(RemainAmount) AS TotalRemain " +
                    "FROM MCSparePartRequest WHERE Dept = 'MC' GROUP BY Receive_Date UNION ALL " +
                    "SELECT RegDate AS YearMonth, SUM(Stock_Amount) AS TotalRemain FROM SparePartTrans " +
                    "WHERE Dept = 'MC' GROUP BY RegDate) t " +
                    "GROUP BY FORMAT(YearMonth, 'yyyy-MM') " +
                    "ORDER BY YearMonth;";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dtVal);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while taking data" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.con.Close();
            try
            {
                DateTime now = DateTime.Now;
                string yearnow = now.ToString("yyyy");
                con.con.Open();
                string query = "SELECT * FROM SparePartBudget WHERE Budget_Dept = '" + dept + "' AND Budget_Year = " + yearnow;
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dtbudget);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while taking data" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.con.Close();
            Cursor = Cursors.Default;
            //select budget
            try
            {
                foreach (DataRow row1 in dtbudget.Rows)
                {
                    string year = row1["Budget_Year"].ToString();
                    string type = row1["Budget_Type"].ToString();
                    double jan = Convert.ToDouble(row1["Jan"]);
                    double feb = Convert.ToDouble(row1["Feb"]);
                    double mar = Convert.ToDouble(row1["Mar"]);
                    double apr = Convert.ToDouble(row1["Apr"]);
                    double may = Convert.ToDouble(row1["May"]);
                    double jun = Convert.ToDouble(row1["Jun"]);
                    double jul = Convert.ToDouble(row1["Jul"]);
                    double aug = Convert.ToDouble(row1["Aug"]);
                    double sep = Convert.ToDouble(row1["Sep"]);
                    double oct = Convert.ToDouble(row1["Oct"]);
                    double nov = Convert.ToDouble(row1["Nov"]);
                    double dec = Convert.ToDouble(row1["Dec"]);

                    dgvBudget.Rows.Add();
                    dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgetyear"].Value = year;
                    dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgettype"].Value = type;
                    dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Jan"].Value = jan;
                    dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Feb"].Value = feb;
                    dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Mar"].Value = mar;
                    dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Apr"].Value = apr;
                    dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["May"].Value = may;
                    dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Jun"].Value = jun;
                    dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Jul"].Value = jul;
                    dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Aug"].Value = aug;
                    dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Sep"].Value = sep;
                    dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Oct"].Value = oct;
                    dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Nov"].Value = nov;
                    dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Dec"].Value = dec;

                }
                dgvBudget.Rows.Add();
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgetyear"].Value = dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["budgetyear"].Value;
                dgvBudget.Rows[dgvBudget.Rows.Count -1].Cells["budgettype"].Value = "Actual";
                for (int i = 2; i < dgvBudget.Columns.Count; i++)
                {
                    string monthName = dgvBudget.Columns[i].Name;
                    DateTime dt = DateTime.ParseExact(monthName, "MMM",
                        System.Globalization.CultureInfo.InvariantCulture);
                    string Colname = $"{DateTime.Now.Year}-{dt:MM}";
                    foreach (DataRow row in dtVal.Rows)
                    {
                        string Date = row["YearMonth"].ToString();
                        double total = Convert.ToDouble(row["TotalValue"]);
                        if (Date == Colname)
                        {
                            dgvBudget.Rows[dgvBudget.Rows.Count -1].Cells[i].Value = total;
                        }
                    }
                }
                string[] months = { "Jan","Feb","Mar","Apr","May","Jun",
                    "Jul","Aug","Sep","Oct","Nov","Dec" };

                foreach (DataGridViewRow row1 in dgvBudget.Rows)
                {
                    foreach (string m in months)
                    {
                        string val = row1.Cells[m].Value?.ToString().Trim();

                        if (string.IsNullOrEmpty(val))
                        {
                            row1.Cells[m].Value = "0";  
                        }
                    }
                    row1.Cells["budgetyear"].ReadOnly = true;
                    row1.Cells["budgettype"].ReadOnly = true;
                    row1.Cells["Jan"].ReadOnly = true;
                    row1.Cells["Feb"].ReadOnly = true;
                    row1.Cells["Mar"].ReadOnly = true;
                    row1.Cells["Apr"].ReadOnly = true;
                    row1.Cells["May"].ReadOnly = true;
                    row1.Cells["Jun"].ReadOnly = true;
                    row1.Cells["Jul"].ReadOnly = true;
                    row1.Cells["Aug"].ReadOnly = true;
                    row1.Cells["Sep"].ReadOnly = true;
                    row1.Cells["Oct"].ReadOnly = true;
                    row1.Cells["Nov"].ReadOnly = true;
                    row1.Cells["Dec"].ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while fill data !" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            dgvBudget.ClearSelection();
            con.con.Close();
            Cursor = Cursors.Default;
            lbFound.Text = "Found: " + dgvBudget.Rows.Count.ToString();
        }
    }
}
