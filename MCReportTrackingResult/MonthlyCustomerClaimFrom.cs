using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MachineDeptApp.MCReportTrackingResult
{
    public partial class MonthlyCustomerClaimFrom : Form
    {
        SQLConnect cnn = new SQLConnect();
        public MonthlyCustomerClaimFrom()
        {
            InitializeComponent();
            cnn.Connection();
            this.Load += MonthlyCustomerClaimFrom_Load;
            btnserch.Click += Btnserch_Click;
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            dgvMonthlyCustomerClaim.CellDoubleClick += DgvMonthlyCustomerClaim_CellDoubleClick;
        }



        // Call this after updating your DataGridView data
        private void UpdateCustomerClaimChart()
        {
            chartMonthlyCustomerClaim.Series.Clear();
            chartMonthlyCustomerClaim.ChartAreas[0].BackColor = Color.FromArgb(204, 255, 255); // Light blue
            chartMonthlyCustomerClaim.BackColor = Color.FromArgb(255, 255, 204); // Light yellow
            chartMonthlyCustomerClaim.Titles.Clear();
            chartMonthlyCustomerClaim.Titles.Add("Monthly Customer Claim");
            chartMonthlyCustomerClaim.Titles[0].Font = new Font("Segoe UI", 16, FontStyle.Regular);
            chartMonthlyCustomerClaim.Titles[0].ForeColor = Color.DeepSkyBlue;
            chartMonthlyCustomerClaim.Legends[0].Docking = Docking.Bottom;

            // Prepare X axis labels (months)
            var monthCols = dgvMonthlyCustomerClaim.Columns
                .Cast<DataGridViewColumn>()
                .Where(c => c.Name.StartsWith("col_") && c.Name != "colTotal")
                .ToList();

            // Get PLAN and ACTUAL values
            double[] plan = new double[monthCols.Count];
            double[] actual = new double[monthCols.Count];
            for (int i = 0; i < monthCols.Count; i++)
            {
                plan[i] = 0;
                actual[i] = 0;
                if (dgvMonthlyCustomerClaim.Rows.Count > 0 && double.TryParse(dgvMonthlyCustomerClaim.Rows[0].Cells[monthCols[i].Name].Value?.ToString(), out double p))
                    plan[i] = p;
                if (dgvMonthlyCustomerClaim.Rows.Count > 1 && double.TryParse(dgvMonthlyCustomerClaim.Rows[1].Cells[monthCols[i].Name].Value?.ToString(), out double a))
                    actual[i] = a;
            }

            // ACTUAL: Red column/bar
            var seriesActual = new Series("Customer Claim ACTUAL")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.Red,
                BorderWidth = 2,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Brown,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            // PLAN: Green line with diamond markers
            var seriesPlan = new Series("Customer Claim PLAN")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Green,
                BorderWidth = 2,
                MarkerStyle = MarkerStyle.Diamond,
                MarkerSize = 10,
                MarkerColor = Color.Orange,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Brown,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            for (int i = 0; i < monthCols.Count; i++)
            {
                string xLabel = (i + 1).ToString();
                seriesActual.Points.AddXY(xLabel, actual[i]);
                seriesPlan.Points.AddXY(xLabel, plan[i]);
                // Optionally, set custom label text
                seriesActual.Points[i].Label = actual[i].ToString("0");
                seriesPlan.Points[i].Label = plan[i].ToString("0");
            }

            chartMonthlyCustomerClaim.Series.Add(seriesActual);
            chartMonthlyCustomerClaim.Series.Add(seriesPlan);

            // Axis and grid styling
            var area = chartMonthlyCustomerClaim.ChartAreas[0];
            area.AxisX.MajorGrid.LineColor = Color.White;
            area.AxisY.MajorGrid.LineColor = Color.White;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            area.AxisX.Interval = 1;
            area.AxisY.Minimum = -0.2;
            area.AxisY.Maximum = Math.Max(plan.Concat(actual).Max(), 2);

            // Remove border
            area.BorderWidth = 0;
            area.BorderColor = Color.Transparent;
        }

        private void MonthlyCustomerClaimFrom_Load(object sender, EventArgs e)
        {
            Btnserch_Click(null, null);
            
        }

        private void CalculateTotals()
        {
            // Find all month columns (skip colNo, colCategory, colType, colTotal)
            var monthCols = dgvMonthlyCustomerClaim.Columns
                .Cast<DataGridViewColumn>()
                .Where(c => c.Name.StartsWith("col_") && c.Name != "colTotal")
                .ToList();

            foreach (DataGridViewRow row in dgvMonthlyCustomerClaim.Rows)
            {
                double sum = 0;
                foreach (var col in monthCols)
                {
                    var val = row.Cells[col.Name].Value;
                    if (val != null && double.TryParse(val.ToString(), out double d))
                        sum += d;
                }
                row.Cells["colTotal"].Value = sum == 0 ? "" : sum.ToString("0.##");
            }
            dgvMonthlyCustomerClaim.Rows[0].Cells["colType"].Style.BackColor = Color.LightGreen;
            dgvMonthlyCustomerClaim.Rows[2].Cells["colType"].Style.BackColor = Color.Red;
            for (int i = 3; i < dgvMonthlyCustomerClaim.ColumnCount - 1; i++)
            {
                var typeCell = dgvMonthlyCustomerClaim.Rows[2].Cells[i];
                if ( typeCell.Value != null && double.TryParse(typeCell.Value.ToString(), out double d))
                {
                    if (d < 0)
                    {
                        dgvMonthlyCustomerClaim.Rows[2].Cells[i].Style.BackColor = Color.Red;
                    }
                }
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var cell1 = dgvMonthlyCustomerClaim.CurrentCell;
            if (cell1 == null) return;
            int rowIndex = cell1.RowIndex;
            int colIndex = cell1.ColumnIndex;
            if (rowIndex < 0 || colIndex < 0) return;
            string colName = dgvMonthlyCustomerClaim.Columns[colIndex].Name;
            if (!colName.StartsWith("col_") || colName == "colTotal")
            {
                MessageBox.Show("Please select a month column cell (not No, Category, QC or Total).",
                    "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var cell = dgvMonthlyCustomerClaim.Rows[rowIndex].Cells[colName];
            if (cell.Value == null || cell.Value.ToString() == "")
            {
                MessageBox.Show("This cell is empty. Please select a cell with data to edit.",
                    "Cell Empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string type = dgvMonthlyCustomerClaim.Rows[rowIndex].Cells["colType"].Value?.ToString() ?? "";
            if (type == "" || type == "Gap")
            {
                MessageBox.Show("Cannot edit a Gap row. Please select a Plan or Actual cell.",
                    "Invalid Row", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string category = "";
            if (rowIndex <= 2)
                category = dgvMonthlyCustomerClaim.Rows[0].Cells["colCategory"].Value?.ToString() ?? "";
            else if (rowIndex >= 3)
                category = dgvMonthlyCustomerClaim.Rows[3].Cells["colCategory"].Value?.ToString() ?? "";
            if (category == "") return;
            string mmyyyy = colName.Replace("col_", "");
            if (!DateTime.TryParseExact(mmyyyy, "MMyyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime monthYear)) return;
            if (MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    cnn.con.Open();
                    SqlCommand cmd = new SqlCommand("Delete from tbCustomerClaim Where Category = @Category And Type = @Type And MonthYear = @MonthYear", cnn.con);
                    cmd.Parameters.AddWithValue("@Category", category);
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@MonthYear", new DateTime(monthYear.Year, monthYear.Month, 1));

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record deleted successfully.");
                    Btnserch_Click(null, null); // Refresh the data grid view
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting record: " + ex.Message);
                }
                finally
                {
                    cnn.con.Close();
                }
            }
        }


        private void BtnEdit_Click(object sender, EventArgs e)
        {
            var cell = dgvMonthlyCustomerClaim.CurrentCell;
            if (cell == null)
            {
                MessageBox.Show("Please select a cell first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            OpendUpdateFrom(cell.RowIndex, cell.ColumnIndex);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var cell = dgvMonthlyCustomerClaim.CurrentCell;
            if (cell == null)
            {
                MessageBox.Show("Please select a cell first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            OpenInsertForm(cell.RowIndex, cell.ColumnIndex);
        }

        private void DgvMonthlyCustomerClaim_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OpenInsertForm(e.RowIndex, e.ColumnIndex);
        }


        private void OpendUpdateFrom(int rowIndex, int colIndex)
        {
            if (rowIndex < 0 || colIndex < 0) return;
            string colName = dgvMonthlyCustomerClaim.Columns[colIndex].Name;
            if (!colName.StartsWith("col_") || colName == "colTotal")
            {
                MessageBox.Show("Please select a month column cell (not No, Category, QC or Total).",
                    "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var cell = dgvMonthlyCustomerClaim.Rows[rowIndex].Cells[colName];
            if (cell.Value == null || cell.Value.ToString() == "")
            {
                MessageBox.Show("This cell is empty. Please select a cell with data to edit.",
                    "Cell Empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string type = dgvMonthlyCustomerClaim.Rows[rowIndex].Cells["colType"].Value?.ToString() ?? "";
            if (type == "" || type == "Gap")
            {
                MessageBox.Show("Cannot edit a Gap row. Please select a Plan or Actual cell.",
                    "Invalid Row", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string category = "";
            if (rowIndex <= 2)
                category = dgvMonthlyCustomerClaim.Rows[0].Cells["colCategory"].Value?.ToString() ?? "";
            else if (rowIndex >= 3)
                category = dgvMonthlyCustomerClaim.Rows[3].Cells["colCategory"].Value?.ToString() ?? "";
            if (category == "") return;
            string mmyyyy = colName.Replace("col_", "");
            if (!DateTime.TryParseExact(mmyyyy, "MMyyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime monthYear)) return;
            using (var form = new EditCustomerClaim(category, type, monthYear))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    Btnserch_Click(null, null);
            }
        }

        private void OpenInsertForm(int rowIndex, int colIndex)
        {
            if (rowIndex < 0 || colIndex < 0) return;

            string colName = dgvMonthlyCustomerClaim.Columns[colIndex].Name;

            if (!colName.StartsWith("col_") || colName == "colTotal")
            {
                MessageBox.Show("Please select a month column cell (not No, Category, QC or Total).",
                    "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var cell = dgvMonthlyCustomerClaim.Rows[rowIndex].Cells[colName];
            if (cell.Value != null && cell.Value.ToString() != "")
            {
                MessageBox.Show("This cell already has data. Please select an empty cell.",
                    "Cell Not Empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string type = dgvMonthlyCustomerClaim.Rows[rowIndex].Cells["colType"].Value?.ToString() ?? "";
            if (type == "" || type == "Gap")
            {
                MessageBox.Show("Cannot insert into a Gap row. Please select a Plan or Actual cell.",
                    "Invalid Row", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string category = "";
            if (rowIndex <= 2)
                category = dgvMonthlyCustomerClaim.Rows[0].Cells["colCategory"].Value?.ToString() ?? "";

            if (category == "") return;

            string mmyyyy = colName.Replace("col_", "");
            if (!DateTime.TryParseExact(mmyyyy, "MMyyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime monthYear)) return;

            using (var form = new AddCustomerClaimForm(category, type, monthYear))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    Btnserch_Click(null, null);
            }
        }




        private void Btnserch_Click(object sender, EventArgs e)
        {
            string queryCondition = "";
            if (cbStartDate.Checked)
            {
                if (dtpStart.Value > dtpEnd.Value)
                {
                    MessageBox.Show("Start date must be before or equal to end date.", "Invalid Range",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                BuildGrid(dtpStart.Value, dtpEnd.Value);
                queryCondition = $" And MonthYear >= '{dtpStart.Value:yyyy-MM-01}' AND MonthYear <= '{dtpEnd.Value:yyyy-MM-dd}'";
            }
            else
            {
                BuildGrid(new DateTime(DateTime.Now.Year, 1, 1), new DateTime(DateTime.Now.Year, 12, 1));
            }

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(" Select * From tbCustomerClaim Where Status = 1 " + queryCondition, cnn.con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string monthYear = Convert.ToDateTime(dt.Rows[i]["MonthYear"]).ToString("MMyyyy");
                        string colName = "col_" + monthYear;
                        if (dgvMonthlyCustomerClaim.Columns.Contains(colName))
                        {
                            for (int j = 0; dgvMonthlyCustomerClaim.Rows.Count > j; j++)
                            {
                                if (j < 3 && dt.Rows[i]["Category"].ToString() == dgvMonthlyCustomerClaim.Rows[0].Cells["colCategory"].Value.ToString() && dt.Rows[i]["Type"].ToString() == dgvMonthlyCustomerClaim.Rows[j].Cells["colType"].Value.ToString())
                                {
                                    dgvMonthlyCustomerClaim.Rows[j].Cells[colName].Value = dt.Rows[i]["Qty"];
                                }

                            }
                        }
                    }
                }
            }
            catch
            {

            }
            CalculateGap();
            CalculateTotals();
            UpdateCustomerClaimChart();
        }
        private void CalculateGap()
        {
            foreach (DataGridViewColumn col in dgvMonthlyCustomerClaim.Columns)
            {
                if (!col.Name.StartsWith("col_") || col.Name == "colTotal") continue;

                // Crimping PPM: rows 0=Plan, 1=Actual, 2=Gap
                object planVal = dgvMonthlyCustomerClaim.Rows[0].Cells[col.Name].Value;
                object actualVal = dgvMonthlyCustomerClaim.Rows[1].Cells[col.Name].Value;
                if (planVal != null && planVal.ToString() != "" &&
                    actualVal != null && actualVal.ToString() != "")
                {
                    if (decimal.TryParse(planVal.ToString(), out decimal p1) &&
                        decimal.TryParse(actualVal.ToString(), out decimal a1))
                        dgvMonthlyCustomerClaim.Rows[2].Cells[col.Name].Value = p1 - a1;
                }
            }
        }
        private void BuildGrid(DateTime start, DateTime end)
        {
            // Remove all dynamic month/total columns (keep colNo, colCategory, colQC)
            for (int i = dgvMonthlyCustomerClaim.Columns.Count - 1; i >= 0; i--)
            {
                string name = dgvMonthlyCustomerClaim.Columns[i].Name;
                if (name != "colNo" && name != "colCategory" && name != "colType")
                    dgvMonthlyCustomerClaim.Columns.RemoveAt(i);
            }

            // Add month columns
            DateTime current = new DateTime(start.Year, start.Month, 1);
            DateTime endMonth = new DateTime(end.Year, end.Month, 1);

            while (current <= endMonth)
            {
                string header = current.ToString("MMM yy").ToUpper();
                string colName = "col_" + current.ToString("MMyyyy");
                var col = new DataGridViewTextBoxColumn
                {
                    Name = colName,
                    HeaderText = header,
                    Width = 80,
                    ReadOnly = true
                };
                dgvMonthlyCustomerClaim.Columns.Add(col);
                current = current.AddMonths(1);
            }

            // Add TOTAL column
            dgvMonthlyCustomerClaim.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTotal",
                HeaderText = "TOTAL",
                Width = 80,
                ReadOnly = true
            });

            // Rebuild rows
            dgvMonthlyCustomerClaim.Rows.Clear();
            for (int i = 0; i < 3; i++)
                dgvMonthlyCustomerClaim.Rows.Add();

            dgvMonthlyCustomerClaim.Rows[0].Cells["colNo"].Value = "1";
            dgvMonthlyCustomerClaim.Rows[0].Cells["colCategory"].Value = "Customer Claim";
            dgvMonthlyCustomerClaim.Rows[0].Cells[2].Value = "Plan";
            dgvMonthlyCustomerClaim.Rows[1].Cells[2].Value = "Actual";
            dgvMonthlyCustomerClaim.Rows[2].Cells[2].Value = "Gap";

        }


    }
}
