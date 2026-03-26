using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MachineDeptApp.RMConnector
{
    public partial class BalanceStockConnectorForm : Form
    {
        SQLConnect cnn = new SQLConnect();

        public BalanceStockConnectorForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.btnserch.Click += BtnSerch_Click;
            this.btnExport.Click += BtnExport_Click;
            this.chkDate.CheckedChanged += ChkDate_CheckedChanged;
            this.Shown += BalanceStockConnectorForm_Shown;
        }

        private void ChkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpFrom.Enabled = chkDate.Checked;
            dtpTo.Enabled   = chkDate.Checked;
        }

        private void BalanceStockConnectorForm_Shown(object sender, EventArgs e)
        {
            btnserch.PerformClick();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvRM.Rows.Count == 0)
            {
                MessageBox.Show("No data to export!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "CSV file (*.csv)|*.csv";
            saveDialog.FileName = "Balance Stock Connector " + DateTime.Now.ToString("yyyyMMdd HHmmss");

            if (saveDialog.ShowDialog() != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;
            try
            {
                string[] columns = { "RM Code", "RM Description", "Type", "Location", "Total Receive", "Total Issued", "Balance" };
                string[] colNames = { "colRMCode", "colRMDescription", "colType", "colLocation", "colTotalReceive", "colTotalIssued", "colBalance" };

                string[] outputCsv = new string[dgvRM.Rows.Count + 1];

                // Header row
                outputCsv[0] = string.Join(",", columns);

                // Data rows
                for (int i = 0; i < dgvRM.Rows.Count; i++)
                {
                    string[] rowValues = new string[colNames.Length];
                    for (int j = 0; j < colNames.Length; j++)
                    {
                        string val = dgvRM.Rows[i].Cells[colNames[j]].Value?.ToString() ?? "";
                        rowValues[j] = "\"" + val.Replace("\"", "\"\"") + "\"";
                    }
                    outputCsv[i + 1] = string.Join(",", rowValues);
                }

                File.WriteAllLines(saveDialog.FileName, outputCsv, Encoding.UTF8);
                Cursor = Cursors.Default;
                MessageBox.Show("Export successfully!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Export failed!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSerch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            dgvRM.Rows.Clear();

            string extraWhere = "";
            if (tbCode.Text.Trim() != "")
                extraWhere += " AND r.RMCode LIKE '%" + tbCode.Text.Trim() + "%'";
            if (tbDescription.Text.Trim() != "")
                extraWhere += " AND r.RMDescription LIKE '%" + tbDescription.Text.Trim() + "%'";

            string dateJoinCondition = "t.Status = '1'";
            if (chkDate.Checked)
                dateJoinCondition += " AND CAST(t.RegisterDate AS DATE) BETWEEN @DateFrom AND @DateTo";

            string sql =
                "SELECT r.RMCode, r.RMDescription, r.Type, r.Location, " +
                "ISNULL(SUM(t.ReceiveQty), 0) AS TotalReceive, " +
                "ISNULL(SUM(t.IssuedQty), 0) AS TotalIssued, " +
                "(ISNULL(SUM(t.ReceiveQty), 0) - ISNULL(SUM(t.IssuedQty), 0)) AS Balance " +
                "FROM tbMstRMConnector r " +
                "LEFT JOIN tbTransactionConnector t ON t.ItemCode = r.RMCode AND " + dateJoinCondition + " " +
                "WHERE 1=1" + extraWhere + " " +
                "GROUP BY r.RMCode, r.RMDescription, r.Type, r.Location " +
                "ORDER BY r.RMCode";

            try
            {
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand(sql, cnn.con);
                if (chkDate.Checked)
                {
                    cmd.Parameters.AddWithValue("@DateFrom", dtpFrom.Value.Date);
                    cmd.Parameters.AddWithValue("@DateTo",   dtpTo.Value.Date);
                }
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    double totalReceive = double.TryParse(row["TotalReceive"]?.ToString(), out var r1) ? r1 : 0;
                    double totalIssued  = double.TryParse(row["TotalIssued"]?.ToString(),  out var r2) ? r2 : 0;
                    double balance      = double.TryParse(row["Balance"]?.ToString(),      out var r3) ? r3 : 0;

                    int rowIdx = dgvRM.Rows.Add(
                        row["RMCode"]?.ToString(),
                        row["RMDescription"]?.ToString(),
                        row["Type"]?.ToString(),
                        row["Location"]?.ToString(),
                        totalReceive,
                        totalIssued,
                        balance);

                    if (balance < 0)
                        dgvRM.Rows[rowIdx].Cells["colBalance"].Style.BackColor = Color.LightPink;
                    else if (balance == 0)
                        dgvRM.Rows[rowIdx].Cells["colBalance"].Style.BackColor = Color.LightYellow;
                    else
                        dgvRM.Rows[rowIdx].Cells["colBalance"].Style.BackColor = Color.LightGreen;
                }

                dgvRM.ClearSelection();
                lbFound.Text = "Found : " + dgvRM.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
            Cursor = Cursors.Default;
        }
    }
}
