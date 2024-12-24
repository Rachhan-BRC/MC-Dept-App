using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.MCSDControl.SDRec
{
    public partial class MCPOSDetailsSearchForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        DataTable dtStatus;

        public MCPOSDetailsSearchForm()
        {
            InitializeComponent(); 
            this.cnn.Connection();
            this.Load += MCPOSDetailsSearchForm_Load;
            this.DtDate.ValueChanged += DtDate_ValueChanged;
            this.btnSearch.Click += BtnSearch_Click;
            this.DgvPOS.CurrentCellChanged += DgvPOS_CurrentCellChanged;
            this.DgvConsumption.CellFormatting += DgvConsumption_CellFormatting;
            this.btnExport.Click += BtnExport_Click;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (DgvPOS.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("តើអ្នកទាញចេញជាឯកសារ Excel មែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    LbStatus.Text = "កំពុងបង្កើតឯកសារ Excel . . .";
                    LbStatus.Refresh();
                    LbStatus.Visible = true;
                    //Export to excel
                    //open excel application and create new workbook
                    Excel.Application app = new Excel.Application();
                    Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
                    Excel.Worksheet worksheet = null;

                    //hide new excel application and give a sheet name
                    app.Visible = false;
                    worksheet = workbook.Sheets["Sheet1"];
                    worksheet = workbook.ActiveSheet;
                    worksheet.Name = "Rachhan System";

                    try
                    {
                        //Add Header 
                        for (int i = 1; i < DgvPOS.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i] = DgvPOS.Columns[i].HeaderText;
                            worksheet.Cells[1, i].Font.Name = "Khmer OS Battambang";
                            worksheet.Cells[1, i].Font.Size = 10;
                            worksheet.Cells[1, i].Font.Bold = true;

                        }

                        //Add Data
                        for (int i = 0; i < DgvPOS.Rows.Count; i++)
                        {
                            for (int j = 1; j < DgvPOS.Columns.Count; j++)
                            {
                                worksheet.Cells[i + 2, j] = DgvPOS.Rows[i].Cells[j].Value.ToString();
                            }
                        }

                        //Set Format
                        worksheet.Range["A2:F" + (DgvPOS.Rows.Count + 1)].Font.Name = "Khmer OS Battambang";
                        worksheet.Range["A2:F" + (DgvPOS.Rows.Count + 1)].Font.Size = 9;

                        worksheet.Range["D2:D" + (DgvPOS.Rows.Count + 1)].NumberFormat = "#,##0";
                        worksheet.Range["F2:F" + (DgvPOS.Rows.Count + 1)].NumberFormat = "dd-mm-yy";

                        //Auto fit
                        worksheet.Columns["A:F"].Autofit();

                        //Getting the location and file name of the excel to save from user. 
                        SaveFileDialog saveDialog = new SaveFileDialog();
                        saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                        saveDialog.FilterIndex = 1;

                        if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            string saveDir = saveDialog.FileName;
                            app.DisplayAlerts = false;
                            workbook.SaveAs(saveDialog.FileName, Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, Type.Missing, false, Excel.XlSaveAsAccessMode.xlExclusive, Excel.XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
                            app.DisplayAlerts = true;
                            workbook.Save();
                            workbook.Close();
                            app.Quit();

                            //Kill all Excel background process
                            var processes = from p in Process.GetProcessesByName("EXCEL")
                                            select p;
                            foreach (var process in processes)
                            {
                                if (process.MainWindowTitle.ToString().Trim() == "")
                                    process.Kill();
                            }

                            LbStatus.Text = "បង្កើតឯកសារ Excel រួចរាល់!";
                            LbStatus.Refresh();
                            Cursor = Cursors.Default;
                            MessageBox.Show("ទាញទិន្នន័យចេញបានជោគជ័យ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            System.Diagnostics.Process.Start(saveDir.ToString());

                        }
                        else
                        {
                            //don't save and close workbook that just created
                            app.DisplayAlerts = false;
                            workbook.Close();
                            app.DisplayAlerts = true;
                            app.Quit();
                            Cursor = Cursors.Default;
                            LbStatus.Visible = false;

                        }
                    }
                    catch
                    {
                        app.DisplayAlerts = false;
                        workbook.Close();
                        app.DisplayAlerts = true;
                        app.Quit();
                        Cursor = Cursors.Default;
                        LbStatus.Text = "បង្កើតឯកសារ Excel មានបញ្ហា!";
                        LbStatus.Refresh();

                    }
                }
            }
        }

        private void DgvConsumption_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.Value.ToString() != "")
            {
                if (Convert.ToDouble(DgvConsumption[2, e.RowIndex].Value.ToString()) == Convert.ToDouble(DgvConsumption[3, e.RowIndex].Value.ToString()))
                {
                    e.CellStyle.ForeColor = Color.Blue;
                    e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambong", 9, FontStyle.Regular);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambong", 9, FontStyle.Bold);
                }

            }
        }

        private void DgvPOS_CurrentCellChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (DgvPOS.SelectedCells.Count > 0)
            {
                DgvConsumption.Rows.Clear();
                try
                {
                    cnn.con.Open();
                    string SysNo = DgvPOS.Rows[DgvPOS.CurrentCell.RowIndex].Cells[0].Value.ToString();
                    string POSNo = DgvPOS.Rows[DgvPOS.CurrentCell.RowIndex].Cells[1].Value.ToString();

                    SqlDataAdapter sda = new SqlDataAdapter("SELECT t1.ItemCode, t1.ItemName, t1.ConsumpQty, t2.RecQty, t2.TranQty FROM " +
                                                                                        "(SELECT SeqNo, tbSDConn1Rec.POSNo, ItemCode, ItemName, ConsumpQty FROM tbSDConn2Consump " +
                                                                                        "INNER JOIN tbSDConn1Rec ON tbSDConn1Rec.SysNo = tbSDConn2Consump.SysNo " +
                                                                                        "WHERE tbSDConn2Consump.SysNo = '" + SysNo + "') t1 " +
                                                                                        "INNER JOIN " +
                                                                                        "(SELECT POSNo, Code, SUM(ReceiveQty) AS RecQty, SUM(TransferQty) AS TranQty FROM tbSDMCAllTransaction " +
                                                                                        "WHERE POSNo = '" + POSNo + "' AND CancelStatus=0 " +
                                                                                        "GROUP BY POSNo, Code, RMDes) t2 " +
                                                                                        "ON t1.POSNo = t2.POSNo AND t1.ItemCode = t2.Code " +
                                                                                        "ORDER BY t1.SeqNo ASC", cnn.con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        DgvConsumption.Rows.Add(row[0], row[1], Convert.ToDouble(row[2]), Convert.ToDouble(row[3]), Convert.ToDouble(row[4]));
                    }

                    DgvConsumption.ClearSelection();
                }
                catch
                {

                }
                cnn.con.Close();
            }
            Cursor = Cursors.Default;

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Visible = true;
            LbStatus.Refresh();
            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Value");
            DgvPOS.Rows.Clear();

            if (txtCode.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("WIPCode", " = '" + txtCode.Text + "' ");
            }
            if (txtPOS.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("POSNo", " = '" + txtPOS.Text + "' ");
            }
            if (txtItem.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("WIPName", " LIKE '%" + txtItem.Text + "%' ");
            }
            if (CboStatus.Text != "ទាំងអស់")
            {
                foreach (DataRow row in dtStatus.Rows)
                {
                    if (row[0].ToString() == CboStatus.Text)
                    {
                        dtSQLCond.Rows.Add("Status", row[1].ToString());
                        break;
                    }
                }

            }
            if (ChkDate.Checked == true)
            {
                dtSQLCond.Rows.Add("PosShipD ", "BETWEEN '" + DtDate.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + DtDate.Value.ToString("yyyy-MM-dd") + " 23:59:59' ");
            }

            string SQLConds = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                if (SQLConds.Trim() == "")
                {
                    SQLConds = "WHERE " + row[0] + row[1];
                }
                else
                {
                    SQLConds = SQLConds + "AND " + row[0] + row[1];
                }
            }

            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbSDConn1Rec " + SQLConds + " ORDER BY POSNo ASC", cnn.con);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    string Status = "";
                    foreach (DataRow rowStat in dtStatus.Rows)
                    {
                        string StatCode = rowStat[1].ToString().Replace("=", "");
                        StatCode = StatCode.Replace(" ", "");
                        if (StatCode == row[6].ToString())
                        {
                            Status = rowStat[0].ToString();
                            break;
                        }
                    }
                    DgvPOS.Rows.Add(row[0], row[1], row[2], row[3], Convert.ToDouble(row[4]), Status, Convert.ToDateTime(row[5]));
                }

                SetColorForCancel();
                DgvPOS.ClearSelection();
                Cursor = Cursors.Default;
                LbStatus.Text = "រកឃើញទិន្នន័យចំនួន ៖ " + DgvPOS.Rows.Count.ToString("N0") + " ទិន្នន័យ";
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                LbStatus.Text = "ការស្វែងរកមានបញ្ហា!";
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

        }

        private void DtDate_ValueChanged(object sender, EventArgs e)
        {
            ChkDate.Checked = true;
        }

        private void MCPOSDetailsSearchForm_Load(object sender, EventArgs e)
        {
            dtStatus = new DataTable();
            dtStatus.Columns.Add("Status");
            dtStatus.Columns.Add("Value");

            //All
            dtStatus.Rows.Add("ទាំងអស់", "");

            //OK & Shortage (1 & 2)
            dtStatus.Rows.Add("ខ្វះ & គ្រប់", " IN (1,2) ");

            //OK ( 2 )
            dtStatus.Rows.Add("គ្រប់", " = 2 ");

            //Shortage ( 1 )
            dtStatus.Rows.Add("ខ្វះ", " = 1 ");

            //Transfered ( 3 )
            dtStatus.Rows.Add("វេររួច", " = 3 ");

            //Cancel ( 0 )
            dtStatus.Rows.Add("Cancel", " = 0 ");

            foreach (DataRow row in dtStatus.Rows)
            {
                CboStatus.Items.Add(row[0]);
            }
            CboStatus.SelectedIndex = 0;
        }

        private void SetColorForCancel()
        {
            foreach (DataGridViewRow row in DgvPOS.Rows)
            {
                if (row.Cells[5].Value.ToString() == "Cancel")
                {

                    row.DefaultCellStyle.BackColor = Color.LightPink;
                }
            }
        }

    }
}
