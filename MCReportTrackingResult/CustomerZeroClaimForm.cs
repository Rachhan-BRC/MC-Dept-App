using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MachineDeptApp.MCReportTrackingResult
{

    public partial class CustomerZeroClaimForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        public CustomerZeroClaimForm()
        {
            InitializeComponent();
            cnn.Connection();
            this.Load += CustomerZeroClaimForm_Load;
            btnAdd.Click += (s, e) => {
                var dlg = new AddCustomerZeroClaim(dgvCustomerZeroClaim);
                dlg.ShowDialog(this);
                Btnserch_Click(null, null);
            };
            btnEdit.Click += (s, e) =>
            {
                if (dgvCustomerZeroClaim.CurrentCell == null) return;
                new EditCustomerZeroClaim(dgvCustomerZeroClaim).ShowDialog(this);
            };
            btnserch.Click += Btnserch_Click;
            btnDelete.Click += (s, e) =>
            {
                if (dgvCustomerZeroClaim.CurrentCell == null) return;
                int rowIndex = dgvCustomerZeroClaim.CurrentCell.RowIndex;
                string SysNo = dgvCustomerZeroClaim.Rows[rowIndex].Cells["SysNo"].Value.ToString();
                if (MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        cnn.con.Open();
                        SqlCommand cmd = new SqlCommand("Delete from tbCustomerZeroClaim where SysNo = @SysNo", cnn.con);
                        cmd.Parameters.AddWithValue("@SysNo", SysNo);
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
            };
            dtpStart.ValueChanged += DtpStart_ValueChanged;
            dtpEnd.ValueChanged += DtpStart_ValueChanged;
        }

        private void DtpStart_ValueChanged(object sender, EventArgs e)
        {
            cbStartDate.Checked = true;
        }

        private void Btnserch_Click(object sender, EventArgs e)
        {
            string qeury = "'";
            if (cbStartDate.Checked)
            {
                qeury = "Where StartDate between '" + dtpStart.Value.ToString("yyyy-MM-dd") + "' and '" + dtpEnd.Value.ToString("yyyy-MM-dd") + "' ";
            }
            else
            {
                qeury = " Where StartDate >= '" + DateTime.Now.Year + "' ";
            }
            SqlDataAdapter sda = new SqlDataAdapter("Select * from tbCustomerZeroClaim " + qeury, cnn.con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dgvCustomerZeroClaim.Rows.Clear();
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dgvCustomerZeroClaim.Rows.Add();
                    dgvCustomerZeroClaim.Rows[i].Cells[0].Value = dt.Rows[i]["SysNo"].ToString();
                    dgvCustomerZeroClaim.Rows[i].Cells[1].Value = dt.Rows[i]["Cycle"].ToString();
                    dgvCustomerZeroClaim.Rows[i].Cells[2].Value = dt.Rows[i]["Category"].ToString();
                    dgvCustomerZeroClaim.Rows[i].Cells[3].Value = dt.Rows[i]["StartDate"];
                    dgvCustomerZeroClaim.Rows[i].Cells[4].Value = dt.Rows[i]["EndDate"];
                    if (dt.Rows[i]["EndDate"] != null && dt.Rows[i]["EndDate"] != DBNull.Value)
                    {
                        DateTime startDate = Convert.ToDateTime(dt.Rows[i]["StartDate"]);
                        DateTime endDate = Convert.ToDateTime(dt.Rows[i]["EndDate"]);
                        int days = (endDate - startDate).Days;
                        dgvCustomerZeroClaim.Rows[i].Cells[5].Value = days;
                    }
                    else
                    {
                        DateTime startDate = Convert.ToDateTime(dt.Rows[i]["StartDate"]);
                        int days = (DateTime.Now - startDate).Days;
                        dgvCustomerZeroClaim.Rows[i].Cells[5].Value = days;
                    }
                    dgvCustomerZeroClaim.Rows[i].Cells[6].Value = dt.Rows[i]["Status"];

                }
                
                dgvCustomerZeroClaim.ClearSelection();
                DesignChart();
            }
        }

        private void CustomerZeroClaimForm_Load(object sender, EventArgs e)
        {
            Btnserch_Click(null, null);
            DesignChart();
        }

        private void DesignChart()
        {

            int max = 0;
            foreach (DataGridViewRow row in dgvCustomerZeroClaim.Rows)
            {
                if (row.Cells["colAchieveDay"].Value != null && row.Cells["colAchieveDay"].Value != DBNull.Value)
                {
                    int value = Convert.ToInt32(row.Cells["colAchieveDay"].Value);
                    if (value > max)
                        max = value;
                }
            }
            var chart = chartCustomerZeroClaim;
            var area = chart.ChartAreas["ChartArea"];

            // Chart & plot area background
            chart.BackColor = Color.FromArgb(230, 255, 200);
            area.BackColor = Color.Yellow;

            // Title
            chart.Titles["Title1"].Text = " CUSTOMER ZERO CLAIM ";
            chart.Titles["Title1"].Font = new Font("Arial", 13F, FontStyle.Bold);
            chart.Titles["Title1"].ForeColor = Color.Green;

            // Y-axis
            area.AxisY.Minimum = 0;
            if( dgvCustomerZeroClaim.Rows.Count > 0)
            {
                area.AxisY.Maximum = max + 5;
            } else
            {
                area.AxisY.Maximum = 50;
            }
            double interval = 10;
            if (max > 100)
            {
                interval = 20;
            }
            //area.AxisY.Maximum = max + 5;
            area.AxisY.Interval = interval;
            area.AxisY.LabelStyle.ForeColor = Color.Red;
            area.AxisY.LabelStyle.Font = new Font("Arial", 9F);
            area.AxisY.LabelStyle.Format = "00 Day";
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(200, 200, 200);
            area.AxisY.MajorTickMark.Enabled = false;
            area.AxisY.LineColor = Color.FromArgb(200, 200, 200);

            // X-axis
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.MajorTickMark.Enabled = false;
            area.AxisX.LabelStyle.Font = new Font("Arial", 9F);
            area.AxisX.LineColor = Color.FromArgb(200, 200, 200);
            area.AxisX.Interval = 1;

            // Hide legend
            chart.Legends.Clear();

            // Series style
            var series = chart.Series["Zero Claim"];
            series.ChartType = SeriesChartType.Column;
            series.IsValueShownAsLabel = true;
            series.Font = new Font("Arial", 9F, FontStyle.Bold);
            series["PointWidth"] = "0.5";
            series.Points.Clear();

            var points = new List<(int Value, string Label, string Cycle, Color Color)>
            { };
                for (int i = 0; i < dgvCustomerZeroClaim.Rows.Count; i++)
                {
                    var cellValue = dgvCustomerZeroClaim.Rows[i].Cells["colAchieveDay"].Value;
                    int value = 0;
                    if (cellValue != null && cellValue != DBNull.Value)
                    {
                        int.TryParse(cellValue.ToString(), out value);
                    }

                    var cycleCell = dgvCustomerZeroClaim.Rows[i].Cells["colCycle"].Value;
                    string cycle = cycleCell?.ToString() ?? $"Cycle{i + 1}";

                    string label = value > 0 ? $"{value} Day" : "";
                    Color color = value > 0 ? Color.FromArgb(0, 180, 100) : Color.Transparent;
                    points.Add((Value: value, Label: label, Cycle: cycle, Color: color));
                }
            
          
            foreach (var p in points)
            {
                var pt = series.Points.Add(p.Value);
                pt.AxisLabel = $"Zero Claim\n{p.Cycle}";
                pt.Color = p.Color;
                pt.Label = p.Label;
            }
        }
    }
}
