using MachineDeptApp.Inventory.Inprocess;
using System;
using System.Collections;
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
using System.Windows.Forms.DataVisualization.Charting;
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
            this.Shown += Budget_Shown;
            this.btnSearch.Click += BtnSearch_Click;
            this.dgvBudget.CellClick += DgvBudget_CellClick;
            this.btnUpdate.Click += BtnUpdate_Click;
            this.btnSave.Click += BtnSave_Click;
            this.dgvBudget.EditingControlShowing += DgvBudget_EditingControlShowing;
            this.dgvBudget.CellEndEdit += DgvBudget_CellEndEdit;
            this.dtpyear.ValueChanged += Dtpyear_ValueChanged;
            this.btnDelete.Click += BtnDelete_Click;
           
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult ask = MessageBox.Show("Are you sure you want to delete data budget "+dtpyear.Value.Year+" ? ", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask == DialogResult.No)
            {
                return;
            }
            int erro = 0;
            Cursor = Cursors.WaitCursor;
            try
            {
                con.con.Open();
                string query = "DELETE FROM SparePartBudget WHERE Budget_Year = @year AND Budget_Dept =@dept";
                SqlCommand cmd = new SqlCommand(query, con.con);
                cmd.Parameters.AddWithValue("@year", dtpyear.Value.Year);
                cmd.Parameters.AddWithValue("@dept", dept);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while delete data !" + ex.Message, "Error delete ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                erro++;
            }
            con.con.Close();
            if (erro == 0)
            {
                btnSearch.PerformClick();
                btnDelete.Enabled = false;
                btnDeleteGray.BringToFront();
                MessageBox.Show("Deleted successfully !", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Cursor = Cursors.Default;
        }

        private void Budget_Shown(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
            SetupChart();
        }

        private void Dtpyear_ValueChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
            SetupChart();
            if (dgvBudget.Rows.Count >= 3)
            {
                btnDelete.Enabled = true;
                btnDelete.BringToFront();
            }
            else
            {
                btnDelete.Enabled = false;
                btnDeleteGray.BringToFront();
            }
        }


        private void SetupChart()
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add("MainArea");

            string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            double[] safety = new double[12];
            double[] target = new double[12];
            double[] actual = new double[12];

            foreach (DataGridViewRow row in dgvBudget.Rows)
            {
                string category = row.Cells[1].Value?.ToString();
                if (string.IsNullOrEmpty(category)) continue;

                for (int i = 0; i < months.Length; i++)
                {
                    object cellValue = row.Cells[months[i]].Value;
                    double value = 0;
                    double.TryParse(cellValue?.ToString(), out value);

                    if (category == "Safety") safety[i] = value;
                    else if (category == "Target") target[i] = value;
                    else if (category == "Actual Order") actual[i] = value;
                }
            }

            var area = chart1.ChartAreas[0];
            area.AxisX.LineWidth = 0;
            area.AxisX.MajorTickMark.Enabled = false;
            area.AxisX.MajorGrid.LineWidth = 0;
            area.AxisX.Interval = 1;

            var safetySeries = new Series("Safety") { ChartType = SeriesChartType.Line, Color = Color.Gold, BorderWidth = 2 };
            var targetSeries = new Series("Target") { ChartType = SeriesChartType.Line, Color = Color.Red, BorderWidth = 2 };
            var actualSeries = new Series("Actual Order") { ChartType = SeriesChartType.Column, IsValueShownAsLabel = true, LabelForeColor = Color.Blue, Font = new Font("Arial", 10, FontStyle.Bold), Color = Color.LightGreen };

            for (int i = 0; i < months.Length; i++)
            {
                safetySeries.Points.AddXY(months[i], safety[i]);
                targetSeries.Points.AddXY(months[i], target[i]);
                actualSeries.Points.AddXY(months[i], actual[i]);
            }

            chart1.Series.Add(safetySeries);
            chart1.Series.Add(targetSeries);
            chart1.Series.Add(actualSeries);
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
            DataTable dtbudget = new DataTable();
            try
            {
                int yearnow = dtpyear.Value.Year;
                con.con.Open();
                string query = "SELECT * FROM SparePartBudget WHERE Budget_Dept = '" + dept + "' AND Budget_Year = " + yearnow;

                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dtbudget);

                Console.WriteLine(dtbudget.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while taking data" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.con.Close();
            //update and insert
            if (dtbudget.Rows.Count > 0)
            {
                Cursor = Cursors.WaitCursor;
                int err = 0;
                try
                {
                    con.con.Open();

                    decimal SafeDecimal(object val)
                    {
                        decimal result;
                        return decimal.TryParse(val?.ToString(), out result) ? result : 0;
                    }

                    foreach (DataGridViewRow row1 in dgvBudget.Rows)
                    {
                        string year = row1.Cells["budgetyear"].Value?.ToString();
                        string type = row1.Cells["budgettype"].Value?.ToString();
                        decimal jan = SafeDecimal(row1.Cells["Jan"].Value);
                        decimal feb = SafeDecimal(row1.Cells["Feb"].Value);
                        decimal mar = SafeDecimal(row1.Cells["Mar"].Value);
                        decimal apr = SafeDecimal(row1.Cells["Apr"].Value);
                        decimal may = SafeDecimal(row1.Cells["May"].Value);
                        decimal jun = SafeDecimal(row1.Cells["Jun"].Value);
                        decimal jul = SafeDecimal(row1.Cells["Jul"].Value);
                        decimal aug = SafeDecimal(row1.Cells["Aug"].Value);
                        decimal sep = SafeDecimal(row1.Cells["Sep"].Value);
                        decimal oct = SafeDecimal(row1.Cells["Oct"].Value);
                        decimal nov = SafeDecimal(row1.Cells["Nov"].Value);
                        decimal dec = SafeDecimal(row1.Cells["Dec"].Value);

                        string query = "UPDATE SparePartBudget SET " +
                                       "Budget_Year = @Year, Jan = @Jan, Feb = @Feb, Mar = @Mar, Apr = @Apr, " +
                                       "May = @May, Jun = @Jun, Jul = @Jul, Aug = @Aug, Sep = @Sep, Oct = @Oct, " +
                                       "Nov = @Nov, Dec = @Dec " +
                                       "WHERE Budget_Dept = @Dept AND Budget_Type = @Type AND Budget_Year = @oldyear";

                        SqlCommand cmd = new SqlCommand(query, con.con);
                        cmd.Parameters.AddWithValue("@Year", year);
                        cmd.Parameters.AddWithValue("@oldyear", dtpyear.Value.Year);
                        cmd.Parameters.AddWithValue("@Type", type);
                        cmd.Parameters.AddWithValue("@Dept", dept);
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
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while updating budget2! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    err++;
                }
                con.con.Close();

                if (err == 0)
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
            else
            {
                int err = 0;
                try
                { 
                    Cursor = Cursors.WaitCursor;
                    con.con.Open();
                    decimal SafeDecimal(object val)
                    {
                        decimal result;
                        return decimal.TryParse(val?.ToString(), out result) ? result : 0;
                    }
                    foreach (DataGridViewRow row1 in dgvBudget.Rows)
                    {
                        string year = row1.Cells["budgetyear"].Value?.ToString();
                        string type = row1.Cells["budgettype"].Value?.ToString();
                        decimal jan = SafeDecimal(row1.Cells["Jan"].Value);
                        decimal feb = SafeDecimal(row1.Cells["Feb"].Value);
                        decimal mar = SafeDecimal(row1.Cells["Mar"].Value);
                        decimal apr = SafeDecimal(row1.Cells["Apr"].Value);
                        decimal may = SafeDecimal(row1.Cells["May"].Value);
                        decimal jun = SafeDecimal(row1.Cells["Jun"].Value);
                        decimal jul = SafeDecimal(row1.Cells["Jul"].Value);
                        decimal aug = SafeDecimal(row1.Cells["Aug"].Value);
                        decimal sep = SafeDecimal(row1.Cells["Sep"].Value);
                        decimal oct = SafeDecimal(row1.Cells["Oct"].Value);
                        decimal nov = SafeDecimal(row1.Cells["Nov"].Value);
                        decimal dec = SafeDecimal(row1.Cells["Dec"].Value);

                        string query = "INSERT INTO SparePartBudget (Budget_Year, Budget_Dept, Budget_Type, Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec ) " +
                                                                                 "VALUES (@Year, @Dept, @Type, @Jan, @Feb, @Mar, @Apr, @May, @Jun, @Jul, @Aug, @Sep, @Oct, @Nov, @Dec)";
                        SqlCommand cmd = new SqlCommand(query, con.con);
                        cmd.Parameters.AddWithValue("@Year", year);
                        cmd.Parameters.AddWithValue("@Type", type);
                        cmd.Parameters.AddWithValue("@Dept", dept);
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
                        cmd.ExecuteNonQuery();
                    }

                }
                catch (Exception ex) 
                {
                    MessageBox.Show("Error while inserting data !" + ex.Message, "Error saving", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    err++;
                }
                con.con.Close();
                Cursor = Cursors.Default;
                if (err == 0)
                {
                    MessageBox.Show("Insert successfully !", "Done.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnSave.Enabled = false;
                    btnSaveGrey.BringToFront();
                    btnUpdate.Enabled = false;
                    btnUpdateGrey.BringToFront();
                    btnSearch.PerformClick();
                }
            }

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

            dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["budgetyear"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["budgettype"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["Jan"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["Feb"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["Mar"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["Apr"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["May"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["Jun"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["Jul"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["Aug"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["Sep"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["Oct"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["Nov"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["Dec"].ReadOnly = true;

            dgvBudget.Rows[dgvBudget.Rows.Count - 3].Cells["budgetyear"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 3].Cells["budgettype"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 3].Cells["Jan"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 3].Cells["Feb"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 3].Cells["Mar"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 3].Cells["Apr"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 3].Cells["May"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 3].Cells["Jun"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 3].Cells["Jul"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 3].Cells["Aug"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 3].Cells["Sep"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 3].Cells["Oct"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 3].Cells["Nov"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 3].Cells["Dec"].ReadOnly = true;

            dgvBudget.Rows[dgvBudget.Rows.Count - 4].Cells["budgetyear"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 4].Cells["budgettype"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 4].Cells["Jan"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 4].Cells["Feb"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 4].Cells["Mar"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 4].Cells["Apr"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 4].Cells["May"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 4].Cells["Jun"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 4].Cells["Jul"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 4].Cells["Aug"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 4].Cells["Sep"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 4].Cells["Oct"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 4].Cells["Nov"].ReadOnly = true;
            dgvBudget.Rows[dgvBudget.Rows.Count - 4].Cells["Dec"].ReadOnly = true;
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
            DataTable dtActual = new DataTable();
            DataTable dtRemain = new DataTable();
            Cursor = Cursors.WaitCursor;
            try
            {
                con.con.Open();
                string query = "SELECT FORMAT(Receive_Date, 'yyyy-MM') AS YearMonth, " +
                    "SUM(Amount) AS ActualOrder " +
                    "FROM MCSparePartRequest " +
                    "WHERE Dept = '"+dept+"' " +
                    "GROUP BY FORMAT(Receive_Date, 'yyyy-MM') " +
                    "ORDER BY YearMonth;";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dtActual);
                Console.WriteLine(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while taking data" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.con.Close();
            try
            {
                con.con.Open();
                string query = "SELECT FORMAT(Receive_Date, 'yyyy-MM') AS YearMonth, SUM(RemainAmount) AS TotalRemain " +
                    "FROM MCSparePartRequest " +
                    "WHERE Dept = '"+dept+"'" +
                    "GROUP BY FORMAT(Receive_Date, 'yyyy-MM') " +
                    "ORDER BY YearMonth;";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dtRemain);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while taking data" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.con.Close();
            try
            {
                int yearnow = dtpyear.Value.Year;
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
            if (dtbudget.Rows.Count == 0)
            {
                dgvBudget.Rows.Add();
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgetyear"].Value = dtpyear.Value.Year;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgettype"].Value = "Safety";
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Jan"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Feb"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Mar"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Apr"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["May"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Jun"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Jul"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Aug"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Sep"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Oct"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Nov"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Dec"].Value = 0;

                dgvBudget.Rows.Add();
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgetyear"].Value = dtpyear.Value.Year;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgettype"].Value = "Target";
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Jan"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Feb"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Mar"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Apr"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["May"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Jun"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Jul"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Aug"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Sep"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Oct"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Nov"].Value = 0;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["Dec"].Value = 0;

              
                Cursor = Cursors.Default;
                return;
            }
            else
            {
                DataTable sortedTable = dtbudget.AsEnumerable()
               .OrderBy(row => row["Budget_Type"].ToString() == "Safety" ? 0 : 1) // Safety first
               .ThenBy(row => row["Budget_Type"].ToString())                      // then alphabetical
               .CopyToDataTable();
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
                if (dgvBudget.Rows.Count < 2 && dgvBudget.Rows[0].Cells[1].Value?.ToString() == "Target")
                {
                    dgvBudget.Rows.Add();
                    dgvBudget.Rows[0].Cells["budgettype"].Value = "Safety";
                    dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgetyear"].Value = dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["budgetyear"].Value;
                   
                }
                else if (dgvBudget.Rows.Count < 2 && dgvBudget.Rows[0].Cells[1].Value?.ToString() == "Safety")
                {
                    dgvBudget.Rows.Add();
                    dgvBudget.Rows[1].Cells["budgettype"].Value = "Target";
                    dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgetyear"].Value = dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["budgetyear"].Value;
                   
            }
                dgvBudget.Rows.Add();
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgetyear"].Value = dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["budgetyear"].Value;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgettype"].Value = "Actual Order";

                dgvBudget.Rows.Add();
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgetyear"].Value = dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["budgetyear"].Value;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgettype"].Value = "GAP";

                dgvBudget.Rows.Add();
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgetyear"].Value = dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["budgetyear"].Value;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgettype"].Value = "Actual Receive";

                dgvBudget.Rows.Add();
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgetyear"].Value = dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells["budgetyear"].Value;
                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells["budgettype"].Value = "Over Due";
                //actual order
                for (int i = 2; i < 14; i++)
                {
                    string monthName = dgvBudget.Columns[i].Name;
                    DateTime dt = DateTime.ParseExact(monthName, "MMM",
                        System.Globalization.CultureInfo.InvariantCulture);
                    string Colname = $"{dtpyear.Value.Year}-{dt:MM}";
                    foreach (DataRow row in dtActual.Rows)
                    {
                        string Date = row["YearMonth"].ToString();
                        double total = Convert.ToDouble(row["ActualOrder"]);
                        if (Date == Colname)
                        {
                            dgvBudget.Rows[dgvBudget.Rows.Count - 4].Cells[i].Value = total;
                            break;

                        }
                    }
                }
                //over due
                for (int i = 2; i < 14; i++)
                {
                    string monthName = dgvBudget.Columns[i].Name;
                    DateTime dt = DateTime.ParseExact(monthName, "MMM",
                        System.Globalization.CultureInfo.InvariantCulture);
                    string Colname = $"{dtpyear.Value.Year}-{dt:MM}";
                    foreach (DataRow row in dtRemain.Rows)
                    {
                        string Date = row["YearMonth"].ToString();
                        double total = Convert.ToDouble(row["TotalRemain"]);
                        DateTime now = Convert.ToDateTime(DateTime.Now);
                        string now1 = now.ToString("yyyy-MM");
                        DateTime now2 = Convert.ToDateTime(now1);
                        DateTime col = Convert.ToDateTime(Colname);
                        string Yearmonth = $"{now.Year}-{now:MM}";

                        if (Date == Colname)
                        {
                            if (now2 > col)
                            {

                                dgvBudget.Rows[dgvBudget.Rows.Count - 1].Cells[i].Value = total;
                            }
                            break;

                        }
                    }
                }
                //gap
                for (int i = 2; i < 14; i++)
                {
                    double  target = Convert.ToDouble(dgvBudget.Rows[dgvBudget.Rows.Count - 6].Cells[i].Value);
                    double actualorder = Convert.ToDouble(dgvBudget.Rows[dgvBudget.Rows.Count - 4].Cells[i].Value);
                    double gap = Convert.ToDouble(actualorder - target);
                    dgvBudget.Rows[dgvBudget.Rows.Count -3].Cells[i].Value = gap;
                }
                //actual receive
                for (int i = 2; i < 14; i++)
                {
                    
                    string monthName = dgvBudget.Columns[i].Name;
                    DateTime dt = DateTime.ParseExact(monthName, "MMM",
                        System.Globalization.CultureInfo.InvariantCulture);
                    string Colname = $"{dtpyear.Value.Year}-{dt:MM}";
                    foreach (DataRow row in dtRemain.Rows)
                    {
                        string Date = row["YearMonth"].ToString();
                        DateTime now = Convert.ToDateTime(DateTime.Now);
                        string now1 = now.ToString("yyyy-MM");
                        DateTime now2 = Convert.ToDateTime(now1);
                        DateTime col = Convert.ToDateTime(Colname);
                        string Yearmonth = $"{now.Year}-{now:MM}";
                        if (Date == Colname)
                        {
                            double overdue = Convert.ToDouble(row["TotalRemain"]);
                            double actualorder = Convert.ToDouble(dgvBudget.Rows[dgvBudget.Rows.Count - 4].Cells[i].Value);
                            double actrec = Convert.ToDouble(actualorder - overdue);
                            dgvBudget.Rows[dgvBudget.Rows.Count - 2].Cells[i].Value = actrec;
                            break;

                        }
                    }
                }



                string[] months = { "Jan","Feb","Mar","Apr","May","Jun",
                    "Jul","Aug","Sep","Oct","Nov","Dec" };

                for (int i = 2; i <14; i++)
                {
                    string safe = dgvBudget.Rows[0].Cells[i].Value?.ToString() ?? "";
                    string tag = dgvBudget.Rows[1].Cells[i].Value?.ToString() ?? "";
                    string act = dgvBudget.Rows[2].Cells[i].Value?.ToString() ?? "";
                    string gap = dgvBudget.Rows[3].Cells[i].Value?.ToString() ?? "";
                    string actrec = dgvBudget.Rows[4].Cells[i].Value?.ToString() ?? "";
                    if (string.IsNullOrEmpty(safe))
                    {
                        dgvBudget.Rows[0].Cells[i].Value = 0;
                    }
                    if (string.IsNullOrEmpty(tag))
                    {
                        dgvBudget.Rows[1].Cells[i].Value = 0;
                    }
                    if (string.IsNullOrEmpty(act))
                    {
                        dgvBudget.Rows[2].Cells[i].Value = 0;
                    }
                    if (string.IsNullOrEmpty(gap))
                    {
                        dgvBudget.Rows[3].Cells[i].Value = 0;
                    }
                    if (string.IsNullOrEmpty(actrec))
                    {
                        dgvBudget.Rows[4].Cells[i].Value = 0;
                    }


                }
                foreach (DataGridViewRow row1 in dgvBudget.Rows)
                {
                  
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
            SetupChart();
            dgvBudget.ClearSelection();
            con.con.Close();
            Cursor = Cursors.Default;
            lbFound.Text = "Found: " + dgvBudget.Rows.Count.ToString();
            if (dgvBudget.Rows.Count >= 3 )
            {
                btnDelete.Enabled = true;
                btnDelete.BringToFront();
            }
            else
            {
                btnDelete.Enabled = false;
                btnDeleteGray.BringToFront();
            }
        }
    }
}
