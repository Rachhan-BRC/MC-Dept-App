using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MachineDeptApp.MCReportTrackingResult
{
    public partial class QualityControlForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        public QualityControlForm()
        {
            InitializeComponent();
            this.Load += QualityControlForm_Load;
            btnserch.Click += Btnserch_Click;
            dgvQualityControl.CellDoubleClick += DgvQualityControl_DoubleClick;
            //  dgvQualityControl.DoubleClick += DgvQualityControl_CellClick;
            cnn.Connection();
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var cell1 = dgvQualityControl.CurrentCell;
            if (cell1 == null) return;
            int rowIndex = cell1.RowIndex;
            int colIndex = cell1.ColumnIndex;
            if (rowIndex < 0 || colIndex < 0) return;
            string colName = dgvQualityControl.Columns[colIndex].Name;
            if (!colName.StartsWith("col_") || colName == "colTotal")
            {
                MessageBox.Show("Please select a month column cell (not No, Category, QC or Total).",
                    "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var cell = dgvQualityControl.Rows[rowIndex].Cells[colName];
            if (cell.Value == null || cell.Value.ToString() == "")
            {
                MessageBox.Show("This cell is empty. Please select a cell with data to edit.",
                    "Cell Empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string type = dgvQualityControl.Rows[rowIndex].Cells["colQC"].Value?.ToString() ?? "";
            if (type == "" || type == "Gap")
            {
                MessageBox.Show("Cannot edit a Gap row. Please select a Plan or Actual cell.",
                    "Invalid Row", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string category = "";
            if (rowIndex <= 2)
                category = dgvQualityControl.Rows[0].Cells["colCategory"].Value?.ToString() ?? "";
            else if (rowIndex >= 3)
                category = dgvQualityControl.Rows[3].Cells["colCategory"].Value?.ToString() ?? "";
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
                    SqlCommand cmd = new SqlCommand("Delete from tbQCReport Where Category = @Category And Type = @Type And MonthYear = @MonthYear", cnn.con);
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
            var cell = dgvQualityControl.CurrentCell;
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
            var cell = dgvQualityControl.CurrentCell;
            if (cell == null)
            {
                MessageBox.Show("Please select a cell first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            OpenInsertForm(cell.RowIndex, cell.ColumnIndex);
        }

        //private void DgvQualityControl_DoubleClick(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void QualityControlForm_Load(object sender, EventArgs e)
        {
            dtpStart.Format = DateTimePickerFormat.Custom;
            dtpStart.CustomFormat = "MMM yyyy";
            dtpEnd.Format = DateTimePickerFormat.Custom;
            dtpEnd.CustomFormat = "MMM yyyy";

            if (cbStartDate.Checked)
                BuildGrid(dtpStart.Value, dtpEnd.Value);
            else
                BuildGrid(new DateTime(DateTime.Now.Year, 1, 1), new DateTime(DateTime.Now.Year, 12, 1));
            Btnserch_Click(null, null);
            
        }
        private void DesignChart()
        {
            // ChartCrimpingDesign();
            PlotCrimpingChartData();
            PlotPressingChartData();
            //ChartPressingDesign();
        }
        private void PlotCrimpingChartData()
        {
            var chart = chartCrimping;
            chart.Series.Clear();

            // PLAN as Bar (Column)
            var planSeries = new Series("Crimping PPM PLAN")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.RoyalBlue,
                BorderWidth = 2
            };

            // ACTUAL as Line with markers
            var actualSeries = new Series("Crimping PPM ACTUAL")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Orange,
                BorderWidth = 5,
                MarkerStyle = MarkerStyle.Diamond,
                MarkerSize = 15,
                MarkerColor = Color.OrangeRed
            };

            // Data
            var monthCols = dgvQualityControl.Columns
     .Cast<DataGridViewColumn>()
     .Where(c => c.Name.StartsWith("col_") && c.Name != "colTotal")
     .ToList();

            double?[] plan = new double?[monthCols.Count];
            double?[] actual = new double?[monthCols.Count];

            for (int i = 0; i < monthCols.Count; i++)
            {
                // PLAN (row 0 for Pressing PPM)
                var planVal = dgvQualityControl.Rows[0].Cells[monthCols[i].Name].Value;
                if (planVal != null && double.TryParse(planVal.ToString(), out double p))
                    plan[i] = p;
                else
                    plan[i] = null;

                // ACTUAL (row 1 for Pressing PPM)
                var actualVal = dgvQualityControl.Rows[1].Cells[monthCols[i].Name].Value;
                if (actualVal != null && double.TryParse(actualVal.ToString(), out double a))
                    actual[i] = a;
                else
                    actual[i] = null;
            }

            for (int i = 0; i < 12; i++)
            {
                planSeries.Points.AddXY(i + 1, plan[i] ?? 0);
                actualSeries.Points.AddXY(i + 1, actual[i] ?? double.NaN);
            }

            chart.Series.Add(planSeries);
            chart.Series.Add(actualSeries);

            // Title
            chart.Titles.Clear();
            chart.Titles.Add("CRIMPING NG PPM");
            chart.Titles[0].Font = new Font("Arial", 16F, FontStyle.Bold);
            chart.Titles[0].ForeColor = Color.Green;

            // Chart area background
            var area = chart.ChartAreas[0];
            chart.BackColor = Color.FromArgb(180, 255, 255);
            area.BackColor = Color.FromArgb(230, 255, 255);

            // Y axis
            double max = 45;
            double maxPlan = plan.Max() ?? 0;
            double maxActual = actual.Max() ?? 0;
            if (maxActual > maxPlan)
            {
                max = maxActual;
            } else
            {
                max = maxPlan;
            }
            double interval = 5;
            if (max > 50)
            {
                interval = 10;
            } else if (max > 100)
            {
                interval = 20;
            }
            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = max + 2;
            area.AxisY.Interval = interval;
            area.AxisY.LabelStyle.Font = new Font("Arial", 10F);
            area.AxisY.LabelStyle.ForeColor = Color.Black;

            // X axis
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Font = new Font("Arial", 10F);
            area.AxisX.LabelStyle.ForeColor = Color.Black;
            
            // Legend
            chart.Legends.Clear();
            chart.Legends.Add(new Legend());
            chart.Legends[0].Docking = Docking.Bottom;
        }

        private void PlotPressingChartData()
        {
            var chart = chartPressing;
            chart.Series.Clear();

            // PLAN as Bar (Column)
            var planSeries = new Series("PLAN")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.SkyBlue,
                BorderWidth = 2
            };

            // ACTUAL as Line with markers
            var actualSeries = new Series("ACTUAL")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.LimeGreen,
                BorderWidth = 5,
                MarkerStyle = MarkerStyle.Star5,
                MarkerSize = 15,
                MarkerColor = Color.LimeGreen
            };
            var monthCols = dgvQualityControl.Columns
             .Cast<DataGridViewColumn>()
            .Where(c => c.Name.StartsWith("col_") && c.Name != "colTotal")
            .ToList();

            double?[] plan = new double?[monthCols.Count];
            double?[] actual = new double?[monthCols.Count];

            for (int i = 0; i < monthCols.Count; i++)
            {
                // PLAN (row 3 for Pressing PPM)
                var planVal = dgvQualityControl.Rows[3].Cells[monthCols[i].Name].Value;
                if (planVal != null && double.TryParse(planVal.ToString(), out double p))
                    plan[i] = p;
                else
                    plan[i] = null;

                // ACTUAL (row 4 for Pressing PPM)
                var actualVal = dgvQualityControl.Rows[4].Cells[monthCols[i].Name].Value;
                if (actualVal != null && double.TryParse(actualVal.ToString(), out double a))
                    actual[i] = a;
                else
                    actual[i] = null;
            }

            for (int i = 0; i < 12; i++)
            {
                planSeries.Points.AddXY(i + 1, plan[i] ?? 0);
                actualSeries.Points.AddXY(i + 1, actual[i] ?? double.NaN);
            }

            chart.Series.Add(planSeries);
            chart.Series.Add(actualSeries);

            // Title
            chart.Titles.Clear();
            chart.Titles.Add("PRESSING NG PPM");
            chart.Titles[0].Font = new Font("Arial", 16F, FontStyle.Bold);
            chart.Titles[0].ForeColor = Color.RoyalBlue;

            // Chart area background
            var area = chart.ChartAreas[0];
            chart.BackColor = Color.FromArgb(255, 245, 204);
            area.BackColor = Color.FromArgb(255, 230, 204);

            // Y axis
            Double max = 250;
            double maxPlan = plan.Max() ?? 0;
            double maxActual = actual.Max() ?? 0;
            if (maxActual > maxPlan)
            {
                max = maxActual;
            }
            else
            {
                max = maxPlan;
            }
            double interval = 50;
            if (max > 500)
            {
                interval = 100;
            }
            else if (max > 1000)
            {
                interval = 200;
            }
            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = max + 10;
            area.AxisY.Interval = interval;
            area.AxisY.LabelStyle.Font = new Font("Arial", 10F);
            area.AxisY.LabelStyle.ForeColor = Color.Black;

            // X axis
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Font = new Font("Arial", 10F);
            area.AxisX.LabelStyle.ForeColor = Color.Black;

            // Legend
            chart.Legends.Clear();
            chart.Legends.Add(new Legend());
            chart.Legends[0].Docking = Docking.Bottom;
        }
        private void CalculateTotals()
        {
            // Find all month columns (skip colNo, colCategory, colQC, colTotal)
            var monthCols = dgvQualityControl.Columns
                .Cast<DataGridViewColumn>()
                .Where(c => c.Name.StartsWith("col_") && c.Name != "colTotal")
                .ToList();

            foreach (DataGridViewRow row in dgvQualityControl.Rows)
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
                SqlDataAdapter adapter = new SqlDataAdapter(" Select * From tbQCReport Where Status = 1 " + queryCondition, cnn.con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string monthYear = Convert.ToDateTime(dt.Rows[i]["MonthYear"]).ToString("MMyyyy");
                        string colName = "col_" + monthYear;
                        if (dgvQualityControl.Columns.Contains(colName))
                        {
                            for (int j = 0; dgvQualityControl.Rows.Count > j; j++)
                            {

                                if (j < 3 && dt.Rows[i]["Category"].ToString() == dgvQualityControl.Rows[0].Cells["colCategory"].Value.ToString() && dt.Rows[i]["Type"].ToString() == dgvQualityControl.Rows[j].Cells["colQC"].Value.ToString())
                                {
                                    dgvQualityControl.Rows[j].Cells[colName].Value = dt.Rows[i]["Qty"];
                                }
                                if (j >= 3 && dt.Rows[i]["Category"].ToString() == dgvQualityControl.Rows[3].Cells["colCategory"].Value.ToString() && dt.Rows[i]["Type"].ToString() == dgvQualityControl.Rows[j].Cells["colQC"].Value.ToString())
                                {
                                    dgvQualityControl.Rows[j].Cells[colName].Value = dt.Rows[i]["Qty"];
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
            DesignChart();
            CalculateTotals();
        }

        private void CalculateGap()
        {
            foreach (DataGridViewColumn col in dgvQualityControl.Columns)
            {
                if (!col.Name.StartsWith("col_") || col.Name == "colTotal") continue;

                // Crimping PPM: rows 0=Plan, 1=Actual, 2=Gap
                object planVal = dgvQualityControl.Rows[0].Cells[col.Name].Value;
                object actualVal = dgvQualityControl.Rows[1].Cells[col.Name].Value;
                if (planVal != null && planVal.ToString() != "" &&
                    actualVal != null && actualVal.ToString() != "")
                {
                    if (decimal.TryParse(planVal.ToString(), out decimal p1) &&
                        decimal.TryParse(actualVal.ToString(), out decimal a1))
                        dgvQualityControl.Rows[2].Cells[col.Name].Value = p1 - a1;
                }

                // Pressing PPM: rows 3=Plan, 4=Actual, 5=Gap
                object planVal2 = dgvQualityControl.Rows[3].Cells[col.Name].Value;
                object actualVal2 = dgvQualityControl.Rows[4].Cells[col.Name].Value;
                if (planVal2 != null && planVal2.ToString() != "" &&
                    actualVal2 != null && actualVal2.ToString() != "")
                {
                    if (decimal.TryParse(planVal2.ToString(), out decimal p2) &&
                        decimal.TryParse(actualVal2.ToString(), out decimal a2))
                        dgvQualityControl.Rows[5].Cells[col.Name].Value = p2 - a2;
                }

                dgvQualityControl.Rows[0].Cells["colQC"].Style.BackColor = Color.LightGreen;
                dgvQualityControl.Rows[2].Cells["colQC"].Style.BackColor = Color.Red;
                dgvQualityControl.Rows[3].Cells["colQC"].Style.BackColor = Color.LightGreen;
                dgvQualityControl.Rows[5].Cells["colQC"].Style.BackColor = Color.Red;
                for (int i = 3; i < dgvQualityControl.ColumnCount - 1; i++)
                {
                    var typeCell = dgvQualityControl.Rows[2].Cells[i];
                    var typeCell2 = dgvQualityControl.Rows[4].Cells[i];
                    if (typeCell.Value != null && double.TryParse(typeCell.Value.ToString(), out double d))
                    {
                        if (d < 0)
                        {
                            dgvQualityControl.Rows[2].Cells[i].Style.BackColor = Color.Red;
                        }
                    }
                    if (typeCell2.Value != null && double.TryParse(typeCell2.Value.ToString(), out double d2))
                    {
                        if (d2 < 0)
                        {
                            dgvQualityControl.Rows[4].Cells[i].Style.BackColor = Color.Red;
                        }
                    }
                }
            }
        }

        private void BuildGrid(DateTime start, DateTime end)
        {
            // Remove all dynamic month/total columns (keep colNo, colCategory, colQC)
            for (int i = dgvQualityControl.Columns.Count - 1; i >= 0; i--)
            {
                string name = dgvQualityControl.Columns[i].Name;
                if (name != "colNo" && name != "colCategory" && name != "colQC")
                    dgvQualityControl.Columns.RemoveAt(i);
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
                dgvQualityControl.Columns.Add(col);
                current = current.AddMonths(1);
            }

            // Add TOTAL column
            dgvQualityControl.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTotal",
                HeaderText = "TOTAL",
                Width = 80,
                ReadOnly = true
            });

            // Rebuild rows
            dgvQualityControl.Rows.Clear();
            for (int i = 0; i < 6; i++)
                dgvQualityControl.Rows.Add();

            dgvQualityControl.Rows[0].Cells["colNo"].Value = "1";
            dgvQualityControl.Rows[0].Cells["colCategory"].Value = "Crimping PPM";
            dgvQualityControl.Rows[0].Cells["colQC"].Value = "Plan";
            dgvQualityControl.Rows[1].Cells["colQC"].Value = "Actual";
            dgvQualityControl.Rows[2].Cells["colQC"].Value = "Gap";

            dgvQualityControl.Rows[3].Cells["colNo"].Value = "2";
            dgvQualityControl.Rows[3].Cells["colCategory"].Value = "Pressing PPM";
            dgvQualityControl.Rows[3].Cells["colQC"].Value = "Plan";
            dgvQualityControl.Rows[4].Cells["colQC"].Value = "Actual";
            dgvQualityControl.Rows[5].Cells["colQC"].Value = "Gap";
        }

        private void DgvQualityControl_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OpenInsertForm(e.RowIndex, e.ColumnIndex);
        }
        private void OpendUpdateFrom(int rowIndex, int colIndex)
        {
            if (rowIndex < 0 || colIndex < 0) return;
            string colName = dgvQualityControl.Columns[colIndex].Name;
            if (!colName.StartsWith("col_") || colName == "colTotal")
            {
                MessageBox.Show("Please select a month column cell (not No, Category, QC or Total).",
                    "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var cell = dgvQualityControl.Rows[rowIndex].Cells[colName];
            if (cell.Value == null || cell.Value.ToString() == "")
            {
                MessageBox.Show("This cell is empty. Please select a cell with data to edit.",
                    "Cell Empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string type = dgvQualityControl.Rows[rowIndex].Cells["colQC"].Value?.ToString() ?? "";
            if (type == "" || type == "Gap")
            {
                MessageBox.Show("Cannot edit a Gap row. Please select a Plan or Actual cell.",
                    "Invalid Row", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string category = "";
            if (rowIndex <= 2)
                category = dgvQualityControl.Rows[0].Cells["colCategory"].Value?.ToString() ?? "";
            else if (rowIndex >= 3)
                category = dgvQualityControl.Rows[3].Cells["colCategory"].Value?.ToString() ?? "";
            if (category == "") return;
            string mmyyyy = colName.Replace("col_", "");
            if (!DateTime.TryParseExact(mmyyyy, "MMyyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime monthYear)) return;
            using (var form = new EditQCForm(category, type, monthYear))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    Btnserch_Click(null, null);
            }
        }

        private void OpenInsertForm(int rowIndex, int colIndex)
        {
            if (rowIndex < 0 || colIndex < 0) return;

            string colName = dgvQualityControl.Columns[colIndex].Name;

            if (!colName.StartsWith("col_") || colName == "colTotal")
            {
                MessageBox.Show("Please select a month column cell (not No, Category, QC or Total).",
                    "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var cell = dgvQualityControl.Rows[rowIndex].Cells[colName];
            if (cell.Value != null && cell.Value.ToString() != "")
            {
                MessageBox.Show("This cell already has data. Please select an empty cell.",
                    "Cell Not Empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string type = dgvQualityControl.Rows[rowIndex].Cells["colQC"].Value?.ToString() ?? "";
            if (type == "" || type == "Gap")
            {
                MessageBox.Show("Cannot insert into a Gap row. Please select a Plan or Actual cell.",
                    "Invalid Row", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string category = "";
            if (rowIndex <= 2)
                category = dgvQualityControl.Rows[0].Cells["colCategory"].Value?.ToString() ?? "";
            else if (rowIndex >= 3)
                category = dgvQualityControl.Rows[3].Cells["colCategory"].Value?.ToString() ?? "";

            if (category == "") return;

            string mmyyyy = colName.Replace("col_", "");
            if (!DateTime.TryParseExact(mmyyyy, "MMyyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime monthYear)) return;

            using (var form = new AddQCReport(category, type, monthYear))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    Btnserch_Click(null, null);
            }
        }
    }
}
