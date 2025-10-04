using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.MCSDControl.SDRec
{
    public partial class MCPOSDetailsSearchForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        DataTable dtStatus;

        string ErrorText = "";

        public MCPOSDetailsSearchForm()
        {
            InitializeComponent(); 
            this.cnn.Connection();
            this.Load += MCPOSDetailsSearchForm_Load;
            this.DtDate.ValueChanged += DtDate_ValueChanged;
            this.btnSearch.Click += BtnSearch_Click;
            this.DgvPOS.CurrentCellChanged += DgvPOS_CurrentCellChanged;
            this.DgvConsumption.CellFormatting += DgvConsumption_CellFormatting;
            this.DgvPOS.CellPainting += DgvPOS_CellPainting;
            this.btnExport.Click += BtnExport_Click;

        }

        private void DgvPOS_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (DgvPOS.Rows[e.RowIndex].Cells["Status"].Value.ToString() == "Cancel")
                {
                    DgvPOS.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Pink;
                }
            }
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
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (DgvConsumption.Columns[e.ColumnIndex].Name == "RecQty")
                {
                    if (Convert.ToDouble(e.Value.ToString()) < Convert.ToDouble(DgvConsumption.Rows[e.RowIndex].Cells["ConsumptionQty"].Value))
                    {
                        e.CellStyle.ForeColor = Color.Red;
                        e.CellStyle.Font = new Font(DgvConsumption.AlternatingRowsDefaultCellStyle.Font, FontStyle.Bold);
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.Font = new Font(DgvConsumption.AlternatingRowsDefaultCellStyle.Font, FontStyle.Bold);
                    }
                }

                if (DgvConsumption.Columns[e.ColumnIndex].Name == "TransferQty")
                {
                    if (Convert.ToDouble(e.Value.ToString()) < Convert.ToDouble(DgvConsumption.Rows[e.RowIndex].Cells["ConsumptionQty"].Value))
                    {
                        e.CellStyle.ForeColor = Color.Orange;
                        e.CellStyle.Font = new Font(DgvConsumption.AlternatingRowsDefaultCellStyle.Font, FontStyle.Bold);
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.Font = new Font(DgvConsumption.AlternatingRowsDefaultCellStyle.Font, FontStyle.Bold);
                    }

                }
            }
        }
        private void DgvPOS_CurrentCellChanged(object sender, EventArgs e)
        {
            ErrorText = "";
            DgvConsumption.Rows.Clear();

            if (DgvPOS.SelectedCells.Count > 0 && DgvPOS.CurrentCell != null && DgvPOS.CurrentCell.RowIndex >= 0 && DgvPOS.CurrentCell.ColumnIndex >= 0)
            {
                Cursor = Cursors.WaitCursor;
                string SysNo = DgvPOS.Rows[DgvPOS.CurrentCell.RowIndex].Cells["SysNo"].Value.ToString();
                string POSNo = DgvPOS.Rows[DgvPOS.CurrentCell.RowIndex].Cells["POSNo"].Value.ToString(); 
                DataTable dt = new DataTable();
                try
                {
                    cnn.con.Open();
                    string SQLQuery = "SELECT T1.POSNo, tbSDConn2Consump.* FROM tbSDConn2Consump " +
                                                "\nINNER JOIN (SELECT * FROM tbSDConn1Rec) T1 " +
                                                "\nON tbSDConn2Consump.SysNo = T1.SysNo " +
                                                "\nWHERE T1.POSNo = '" + POSNo + "' AND tbSDConn2Consump.SysNo = '" + SysNo + "' " +
                                                "\nORDER BY POSNo ASC, SeqNo ASC";
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dt);
                }
                catch(Exception ex)
                {
                    ErrorText = ex.Message;
                }
                cnn.con.Close();
                //Add to Dgv
                if (ErrorText.Trim() == "")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        DgvConsumption.Rows.Add();
                        DgvConsumption.Rows[DgvConsumption.Rows.Count - 1].HeaderCell.Value = DgvConsumption.Rows.Count.ToString();
                        DgvConsumption.Rows[DgvConsumption.Rows.Count-1].Cells["RMCode"].Value = row["ItemCode"].ToString();
                        DgvConsumption.Rows[DgvConsumption.Rows.Count - 1].Cells["RMName"].Value = row["ItemName"].ToString();
                        DgvConsumption.Rows[DgvConsumption.Rows.Count - 1].Cells["ConsumptionQty"].Value = Convert.ToDouble(row["ConsumpQty"]);
                        DgvConsumption.Rows[DgvConsumption.Rows.Count - 1].Cells["RecQty"].Value = Convert.ToDouble(row["RecQty"]);
                        DgvConsumption.Rows[DgvConsumption.Rows.Count - 1].Cells["TransferQty"].Value = Convert.ToDouble(row["TransferQty"]);                        
                    }
                    DgvConsumption.ClearSelection();
                }
                Cursor = Cursors.Default;
            }

            if (ErrorText.Trim() != "")
                MessageBox.Show("មានបញ្ហា!\n"+ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            DgvPOS.Rows.Clear();
            ErrorText = "";

            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Value");
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


            //Taking Data
            DataTable dtSearch = new DataTable();
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT *, " +
                    "\nCASE " +
                    "\n\tWHEN Status = 1 THEN N'ខ្វះ' " +
                    "\n\tWHEN Status = 2 THEN N'គ្រប់' " +
                    "\n\tWHEN Status = 3 THEN N'វេររួច' " +
                    "\n\tELSE N'Cancel' " +
                    "\nEND AS StatusText FROM tbSDConn1Rec " + SQLConds + " ORDER BY POSNo ASC", cnn.con);
                sda.Fill(dtSearch);

            }
            catch (Exception ex)
            {
                ErrorText = "Taking Data : " + ex.Message; 
            }
            cnn.con.Close();

            //Add to DGV
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtSearch.Rows)
                {
                    DgvPOS.Rows.Add();
                    DgvPOS.Rows[DgvPOS.Rows.Count - 1].Cells["SysNo"].Value = row["SysNo"].ToString();
                    DgvPOS.Rows[DgvPOS.Rows.Count - 1].Cells["POSNo"].Value = row["POSNo"].ToString();
                    DgvPOS.Rows[DgvPOS.Rows.Count - 1].Cells["WIPCode"].Value = row["WIPCode"].ToString();
                    DgvPOS.Rows[DgvPOS.Rows.Count - 1].Cells["WIPName"].Value = row["WIPName"].ToString();
                    DgvPOS.Rows[DgvPOS.Rows.Count - 1].Cells["POSQty"].Value = Convert.ToDouble(row["PosQty"]);
                    DgvPOS.Rows[DgvPOS.Rows.Count - 1].Cells["Status"].Value = row["StatusText"].ToString();
                    DgvPOS.Rows[DgvPOS.Rows.Count - 1].Cells["ShipDate"].Value = Convert.ToDateTime(row["PosShipD"]);
                    DgvPOS.Rows[DgvPOS.Rows.Count - 1].Cells["RegDate"].Value = Convert.ToDateTime(row["RegDate"]);
                    DgvPOS.Rows[DgvPOS.Rows.Count - 1].Cells["RegBy"].Value = row["RegBy"].ToString();
                    DgvPOS.Rows[DgvPOS.Rows.Count - 1].Cells["UpdateDate"].Value = Convert.ToDateTime(row["UpdateDate"]);
                    DgvPOS.Rows[DgvPOS.Rows.Count - 1].Cells["UpdateBy"].Value = row["UpdateBy"].ToString();
                }
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                LbStatus.Text = "រកឃើញទិន្នន័យចំនួន ៖ " + DgvPOS.Rows.Count.ToString("N0") + " ទិន្នន័យ";
                LbStatus.Refresh();
                DgvPOS.ClearSelection(); DgvPOS.CurrentCell = null;
            }
            else
            {
                LbStatus.Text = "ការស្វែងរកមានបញ្ហា!";
                LbStatus.Refresh();
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void DtDate_ValueChanged(object sender, EventArgs e)
        {
            ChkDate.Checked = true;
        }
        private void MCPOSDetailsSearchForm_Load(object sender, EventArgs e)
        {
            DgvConsumption.RowHeadersDefaultCellStyle.Font = new Font("Calibri",10, FontStyle.Regular);
            //foreach (DataGridViewColumn col in DgvConsumption.Columns)
            //    Console.WriteLine(col.Name);

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
            CboStatus.SelectedIndex = 1;
        }

    }
}
