using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.MCPlans
{
    public partial class MCPlanStatusSearchForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        DataTable dtColor;
        DataTable dtMCName;
        string fName;
        public static string TypeOfExport;

        public MCPlanStatusSearchForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += MCPlanStatusSearchForm_Load;
            this.btnSearch.Click += BtnSearch_Click;
            this.dgvSearchResult.CellFormatting += DgvSearchResult_CellFormatting;
            this.btnPrint.Click += BtnPrint_Click;
            this.btnExport.Click += BtnExport_Click;
            this.dtpFrom.ValueChanged += DtpFrom_ValueChanged;
            this.dtpToo.ValueChanged += DtpToo_ValueChanged;
            this.dtpPrintDate.ValueChanged += DtpPrintDate_ValueChanged;
            this.CboMC1.SelectedValueChanged += CboMC1_SelectedValueChanged;

        }

        private void CboMC1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (CboMC1.Text.Trim() != "")
            {
                try
                {
                    cnn.con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT MCName FROM tbMasterMCType WHERE MCType='" + CboMC1.Text + "' ", cnn.con);
                    DataTable dtMCName = new DataTable();
                    sda.Fill(dtMCName);
                    CboMCName.Items.Clear();
                    CboMCName.Items.Add("");
                    foreach (DataRow row in dtMCName.Rows)
                    {
                        CboMCName.Items.Add(row[0].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnn.con.Close();
            }
        }

        private void DtpPrintDate_ValueChanged(object sender, EventArgs e)
        {
            ChkPrintDate.Checked = true;
        }

        private void DtpToo_ValueChanged(object sender, EventArgs e)
        {
            ChkDate.Checked = true;
        }

        private void DtpFrom_ValueChanged(object sender, EventArgs e)
        {
            ChkDate.Checked = true;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvSearchResult.Rows.Count > 0)
            {
                TypeOfExport = "";
                MCPlanStatusSearchFormExport Mpssfe = new MCPlanStatusSearchFormExport();
                Mpssfe.ShowDialog();
                if (TypeOfExport == "0")
                {
                    ExportMasterPlanAll();
                }
                else if (TypeOfExport == "1")
                {
                    ExportMasterPlanKIT();
                }
                else
                {

                }
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (dgvSearchResult.Rows.Count > 0)
            {
                int InsertRow = 0;
                foreach (DataGridViewRow DgvRow in dgvSearchResult.Rows)
                {
                    if (DgvRow.Cells["MC1Name"].Value != null && DgvRow.Cells["MC1Name"].Value.ToString().Trim() != "")
                    {
                        InsertRow = InsertRow + 1;
                    }
                }

                if (InsertRow > 0)
                {
                    DialogResult DLS = MessageBox.Show("តើអ្នកចង់ព្រីនគម្រោងនេះមែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DLS == DialogResult.Yes)
                    {
                        Cursor = Cursors.WaitCursor;
                        LbStatus.Text = "កំពុងទាញឯកសារ Excel . . .";
                        LbStatus.Refresh();
                        LbStatus.Visible = true;
                        var CDirectory = Environment.CurrentDirectory;
                        Excel.Application excelApp = new Excel.Application();
                        Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\POSDetailMCTemplate - Plan.xlsx", Editable: true);
                        Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["RachhanSystem"];

                        if (InsertRow > 1)
                        {
                            worksheet.Range["8:" + (InsertRow + 6)].Insert();
                        }

                        string POSCNoPrintStatus = "";
                        worksheet.Cells[2, 9] = DateTime.Now;
                        int rowIndex = 7;

                        foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
                        {
                            if (dgvRow.Cells["MC1Name"].Value != null && dgvRow.Cells["MC1Name"].Value.ToString().Trim() != "")
                            {
                                string Customer = dgvRow.Cells["Customer"].Value.ToString();
                                string Model = dgvRow.Cells["Model"].Value.ToString();
                                string Line = dgvRow.Cells["LineCode"].Value.ToString();
                                string FGCode = dgvRow.Cells["FGCode"].Value.ToString();
                                string POSPNo = dgvRow.Cells["PosPNo"].Value.ToString();
                                int PQty = Convert.ToInt32(dgvRow.Cells["PosPQty"].Value.ToString());
                                DateTime DelDate = Convert.ToDateTime(dgvRow.Cells["PosPDelDate"].Value.ToString());
                                string WIPCode = dgvRow.Cells["WIPCode"].Value.ToString();
                                string WIPName = dgvRow.Cells["WIPName"].Value.ToString();
                                string POSCNo = dgvRow.Cells["PosCNo"].Value.ToString();
                                string PIN = dgvRow.Cells["PINAssign"].Value.ToString();
                                string Wire = dgvRow.Cells["WireTube"].Value.ToString();
                                string Length = dgvRow.Cells["Length"].Value.ToString();
                                int SemiQty = Convert.ToInt32(dgvRow.Cells["SemiQty"].Value.ToString());
                                int CQty = Convert.ToInt32(dgvRow.Cells["PosCQty"].Value.ToString());
                                int PassedQty = Convert.ToInt32(dgvRow.Cells["PosCResultQty"].Value.ToString());
                                int RemainQty = Convert.ToInt32(dgvRow.Cells["PosCRemainQty"].Value.ToString());
                                string Remarks = dgvRow.Cells["Remarks"].Value.ToString();
                                string DocNo = dgvRow.Cells["DocNo"].Value.ToString();
                                DateTime KITPromiss = Convert.ToDateTime(dgvRow.Cells["KITPromise"].Value.ToString());
                                Nullable<DateTime> ActualKIT = null;
                                if (dgvRow.Cells["ActualKIT"].Value != null && dgvRow.Cells["ActualKIT"].Value.ToString().Trim() != "")
                                {
                                    ActualKIT = Convert.ToDateTime(dgvRow.Cells["ActualKIT"].Value.ToString());
                                }
                                Nullable<DateTime> KITSD = null;
                                if (dgvRow.Cells["KITSD"].Value != null && dgvRow.Cells["KITSD"].Value.ToString().Trim() != "")
                                {
                                    KITSD = Convert.ToDateTime(dgvRow.Cells["KITSD"].Value.ToString());
                                }
                                Nullable<DateTime> KITSDTrans = null;
                                if (dgvRow.Cells["KITLastTransfer"].Value != null && dgvRow.Cells["KITLastTransfer"].Value.ToString().Trim() != "")
                                {
                                    KITSDTrans = Convert.ToDateTime(dgvRow.Cells["KITLastTransfer"].Value.ToString());
                                }
                                string MC1Label = dgvRow.Cells["LabelStatus"].Value.ToString();
                                string MC1Slot = dgvRow.Cells["Slot"].Value.ToString();
                                string PlanSeqNo = dgvRow.Cells["PlanSeqNo"].Value.ToString();
                                string MC1 = dgvRow.Cells["MC1"].Value.ToString();
                                string MC1Name = dgvRow.Cells["MC1Name"].Value.ToString();
                                string MC1Status = dgvRow.Cells["MC1Status"].Value.ToString();
                                Nullable<DateTime> MC1Date = null;
                                if (dgvRow.Cells["MC1ETA"].Value != null && dgvRow.Cells["MC1ETA"].Value.ToString().Trim() != "")
                                {
                                    MC1Date = Convert.ToDateTime(dgvRow.Cells["MC1ETA"].Value.ToString());
                                }
                                string MC2 = dgvRow.Cells["MC2"].Value.ToString();
                                string MC2Name = dgvRow.Cells["MC2Name"].Value.ToString();
                                string MC2Status = dgvRow.Cells["MC2Status"].Value.ToString();
                                Nullable<DateTime> MC2Date = null;
                                if (dgvRow.Cells["MC2ETA"].Value != null && dgvRow.Cells["MC2ETA"].Value.ToString().Trim() != "")
                                {
                                    MC2Date = Convert.ToDateTime(dgvRow.Cells["MC2ETA"].Value.ToString());
                                }
                                string MC3 = dgvRow.Cells["MC3"].Value.ToString();
                                string MC3Name = dgvRow.Cells["MC3Name"].Value.ToString();
                                string MC3Status = dgvRow.Cells["MC3Status"].Value.ToString();
                                Nullable<DateTime> MC3Date = null;
                                if (dgvRow.Cells["MC3ETA"].Value != null && dgvRow.Cells["MC3ETA"].Value.ToString().Trim() != "")
                                {
                                    MC3Date = Convert.ToDateTime(dgvRow.Cells["MC3ETA"].Value.ToString());
                                }
                                string MQCName = dgvRow.Cells["LastMQCName"].Value.ToString();
                                string MQCStatus = dgvRow.Cells["MQCStatus"].Value.ToString();
                                Nullable<DateTime> MQCDate = null;
                                if (dgvRow.Cells["LastMQCDate"].Value != null && dgvRow.Cells["LastMQCDate"].Value.ToString().Trim() != "")
                                {
                                    MQCDate = Convert.ToDateTime(dgvRow.Cells["LastMQCDate"].Value.ToString());
                                }
                                string Status = dgvRow.Cells["POSStatus"].Value.ToString();
                                Nullable<DateTime> LastWIPTrans = null;
                                if (dgvRow.Cells["LastRegDate"].Value != null && dgvRow.Cells["LastRegDate"].Value.ToString().Trim() != "")
                                {
                                    LastWIPTrans = Convert.ToDateTime(dgvRow.Cells["LastRegDate"].Value.ToString());
                                }

                                worksheet.Cells[rowIndex, 1] = "= SUBTOTAL(3,$J$7:J" + rowIndex + ")";
                                worksheet.Cells[rowIndex, 2] = Customer;
                                worksheet.Cells[rowIndex, 3] = Model;
                                worksheet.Cells[rowIndex, 4] = Line;
                                worksheet.Cells[rowIndex, 5] = FGCode;
                                worksheet.Cells[rowIndex, 6] = POSPNo;
                                worksheet.Cells[rowIndex, 7] = PQty;
                                worksheet.Cells[rowIndex, 8] = DelDate;
                                worksheet.Cells[rowIndex, 9] = WIPCode;
                                worksheet.Cells[rowIndex, 10] = WIPName;
                                worksheet.Cells[rowIndex, 11] = POSCNo;
                                worksheet.Cells[rowIndex, 12] = PIN;
                                worksheet.Cells[rowIndex, 13] = Wire;
                                worksheet.Cells[rowIndex, 14] = Length;
                                worksheet.Cells[rowIndex, 15] = SemiQty;
                                worksheet.Cells[rowIndex, 16] = CQty;
                                worksheet.Cells[rowIndex, 17] = PassedQty;
                                worksheet.Cells[rowIndex, 18] = RemainQty;
                                worksheet.Cells[rowIndex, 19] = Remarks;
                                worksheet.Cells[rowIndex, 20] = DocNo;
                                worksheet.Cells[rowIndex, 21] = KITPromiss;
                                worksheet.Cells[rowIndex, 22] = ActualKIT;
                                worksheet.Cells[rowIndex, 23] = KITSD;
                                worksheet.Cells[rowIndex, 24] = KITSDTrans;
                                worksheet.Cells[rowIndex, 25] = MC1Label;
                                worksheet.Cells[rowIndex, 26] = MC1Slot;
                                worksheet.Cells[rowIndex, 27] = PlanSeqNo;
                                worksheet.Cells[rowIndex, 28] = MC1;
                                worksheet.Cells[rowIndex, 29] = MC1Name;
                                worksheet.Cells[rowIndex, 30] = MC1Status;
                                worksheet.Cells[rowIndex, 31] = MC1Date;
                                worksheet.Cells[rowIndex, 32] = MC2;
                                worksheet.Cells[rowIndex, 33] = MC2Name;
                                worksheet.Cells[rowIndex, 34] = MC2Status;
                                worksheet.Cells[rowIndex, 35] = MC2Date;
                                worksheet.Cells[rowIndex, 36] = MC3;
                                worksheet.Cells[rowIndex, 37] = MC3Name;
                                worksheet.Cells[rowIndex, 38] = MC3Status;
                                worksheet.Cells[rowIndex, 39] = MC3Date;
                                worksheet.Cells[rowIndex, 40] = MQCName;
                                worksheet.Cells[rowIndex, 41] = MQCStatus;
                                worksheet.Cells[rowIndex, 42] = MQCDate;
                                worksheet.Cells[rowIndex, 43] = Status;
                                worksheet.Cells[rowIndex, 44] = LastWIPTrans;

                                if (POSCNoPrintStatus.Trim() == "")
                                {
                                    POSCNoPrintStatus = "'" + dgvRow.Cells["POSCNo"].Value.ToString() + "'";
                                }
                                else
                                {
                                    POSCNoPrintStatus = POSCNoPrintStatus + ", '" + dgvRow.Cells["POSCNo"].Value.ToString() + "'";
                                }

                                rowIndex = rowIndex + 1;
                            }                            
                        }

                        worksheet.Cells[InsertRow + 7, 1] = "End";
                        worksheet.Cells[InsertRow + 7, 1].Font.Color = Color.White;
                        worksheet.Range["A7:AR" + (InsertRow + 6)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        worksheet.Range["A" + (InsertRow + 6) + ":AR" + (InsertRow + 6)].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;


                        //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                        string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\POSDetailMC - Plan";
                        if (!Directory.Exists(SavePath))
                        {
                            Directory.CreateDirectory(SavePath);
                        }

                        // Saving the modified Excel file                        
                        string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                        string file = "POSDetailMC - Plan ";
                        fName = file + "( " + date + " )";
                        worksheet.SaveAs(CDirectory.ToString() + @"\Report\POSDetailMC - Plan\" + fName + ".xlsx");
                        xlWorkBook.Save();
                        xlWorkBook.Close();
                        excelApp.Quit();

                        //Kill all Excel background process
                        var processes = from p in Process.GetProcessesByName("EXCEL")
                                        select p;
                        foreach (var process in processes)
                        {
                            if (process.MainWindowTitle.ToString().Trim() == "")
                                process.Kill();
                        }

                        //Update PrintStatus
                        if (POSCNoPrintStatus.Trim() != "")
                        {
                            try
                            {
                                cnn.con.Open();
                                string query = "UPDATE tbPOSDetailofMC SET " +
                                                            "PrintStatus = 1, " +
                                                            "PrintDate = '"+DateTime.Now.ToString("yyyy-MM-dd")+" 00:00:00' " +
                                                            "WHERE PosCNo IN (" + POSCNoPrintStatus + ") ;";
                                SqlCommand cmd = new SqlCommand(query, cnn.con);
                                cmd.ExecuteNonQuery();
                            }
                            catch
                            {

                            }
                            cnn.con.Close();

                        }

                        Cursor = Cursors.Default;
                        LbStatus.Text = "ឯកសារ Excel រួចរាល់!";
                        LbStatus.Refresh();
                        MessageBox.Show("ឯកសារ Excel រួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Diagnostics.Process.Start(CDirectory.ToString() + @"\\Report\POSDetailMC - Plan\" + fName + ".xlsx");
                        fName = "";
                        btnSearch.PerformClick();
                    }
                }
                else
                {
                    MessageBox.Show("ភីអូអេសទាំងអស់នេះមិនទាន់ជ្រើសរើស ម៉ាស៊ីន រួចនុះទេ!","Rachhan System", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                }
            }
        }

        private void DgvSearchResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 11 && e.Value.ToString() != "")
            {
                foreach (DataRow row in dtColor.Rows)
                {
                    if (e.Value.ToString().Contains(row[0].ToString()))
                    {
                        e.CellStyle.ForeColor = Color.FromName(row[1].ToString());
                        e.CellStyle.BackColor = Color.FromName(row[2].ToString());
                    }
                }
            }

            //Status
            if (dgvSearchResult.Columns[e.ColumnIndex].Name == "MC1Status" || dgvSearchResult.Columns[e.ColumnIndex].Name == "MC2Status" || dgvSearchResult.Columns[e.ColumnIndex].Name == "MC3Status" || dgvSearchResult.Columns[e.ColumnIndex].Name == "MQCStatus")
            {
                if (e.Value.ToString() == "STOP")
                {
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.BackColor = Color.Green;
                }
                if (e.Value.ToString() == "RUN")
                {
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.BackColor = Color.Orange;
                }
                if (e.Value.ToString() == "FINISH")
                {
                    e.CellStyle.ForeColor = Color.White;
                    e.CellStyle.BackColor = Color.Red;
                }
            }

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            dgvSearchResult.Rows.Clear();
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            LbStatus.Visible = true;
            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Value");

            if (txtPOSP.Text.Trim() != "")
            {
                string POSP = txtPOSP.Text;
                POSP = POSP.Replace("*", "%");
                dtSQLCond.Rows.Add("T1.PosPNo LIKE ", "'"+POSP+"' ");
            }
            if (txtPOSC.Text.Trim() != "")
            {
                string POSC = txtPOSC.Text;
                POSC = POSC.Replace("*", "%");
                dtSQLCond.Rows.Add("T1.PosCNo LIKE ", "'" + POSC + "' ");
            }
            if (txtWIPName.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("T3.ItemName LIKE ", "'%" + txtWIPName.Text + "%' ");
            }
            if (ChkDate.Checked == true)
            {
                dtSQLCond.Rows.Add("T1.PosPDelDate BETWEEN ", "'"+dtpFrom.Value.ToString("yyyy-MM-dd")+" 00:00:00' AND '"+dtpToo.Value.ToString("yyyy-MM-dd")+" 23:59:59' ");
            }
            if (CboStatus.Text.Trim() != "All")
            {
                if (CboStatus.SelectedIndex == 0)
                {
                    dtSQLCond.Rows.Add("NOT T1.PosCStatus = ", "2 ");
                }
                else
                {
                    dtSQLCond.Rows.Add("T1.PosCStatus = ", "2 ");
                }
            }
            if (CboMC1.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("T3.MC1Type = ", "'"+CboMC1.Text+"' ");
            }
            if (txtPin.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("T3.Remarks1 LIKE ", "'%"+ txtPin.Text +"%' ");
            }
            if (txtWire.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("T3.Remarks2 LIKE ", "'%" + txtWire.Text + "%' ");
            }
            if (CboPrintStatus.Text.Trim() != "All")
            {
                if (CboPrintStatus.Text == "NOT YET")
                {
                    dtSQLCond.Rows.Add("NOT T1.PrintStatus = ", "1 ");
                }
                else
                {
                    dtSQLCond.Rows.Add("T1.PrintStatus = ", "1 ");
                }
            }
            if (ChkPrintDate.Checked == true)
            {
                dtSQLCond.Rows.Add("T1.PrintDate = ","'"+dtpPrintDate.Value.ToString("yyyy-MM-dd")+" 00:00:00' ");
            }
            if (txtDocNo.Text.Trim() != "")
            {
                string DocNo = txtDocNo.Text;
                if (txtDocNo.Text.ToString().Contains("&") == true)
                {
                    string[] array = DocNo.Split('&');
                    string DocNoIN = "";
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (DocNoIN.Trim() == "")
                        {
                            DocNoIN = "'" + array[i].ToString() + "'";
                        }
                        else
                        {
                            DocNoIN = DocNoIN + " ,'" + array[i].ToString() + "'";
                        }
                    }
                    dtSQLCond.Rows.Add("T1.DocNo IN ", "(" + DocNoIN + ") ");

                }
                else if (txtDocNo.Text.ToString().Contains("*") == true) 
                {
                    DocNo = DocNo.Replace("*", "%");
                    dtSQLCond.Rows.Add("T1.DocNo LIKE ", "'" + DocNo + "' ");
                }
                else
                {
                    dtSQLCond.Rows.Add("T1.DocNo = ", "'" + DocNo + "' ");
                }
            }
            if (CboMCName.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("T1.MC1Name = ", "'" + CboMCName.Text + "' ");
            }

            string SQLCond = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                if (SQLCond.Trim() == "")
                {
                    SQLCond = "WHERE " + row[0] + row[1];
                }
                else
                {
                    SQLCond = SQLCond + "AND " + row[0] + row[1];
                }
            }

            cnn.con.Open();

            string SQLQueryText = "SELECT T2.Remarks AS Customer,T2.Remarks2 AS Model, '' AS LineCode , FGCode, T1.PosPNo, T1.PosPQty, T1.PosPDelDate, T1.WIPCode, T3.ItemName AS WIPName, T1.PosCNo, T3.Remarks1 AS PINAssign, T3.Remarks2 AS WireTube, T3.Remarks3 AS Length, T1.SemiQty, T1.PosCQty, T1.PosCResultQty, T1.PosCRemainQty, T1.Remarks, T1.DocNo, T1.KITPromise, '' AS ActualKIT, T5.LastRegDate AS KITSD, T12.KITLastTransfer, LabelStatus, T3.Slot, T1.PlanSeqNo, " +
                "\nCONCAT(T3.MC1Type, '') AS MC1, T1.MC1Name, T1.MC1Status, T1.MC1ETA, " +
                "\nCONCAT(T3.MC2Type, '') AS MC2, T1.MC2Name, T1.MC2Status, T1.MC2ETA, " +
                "\nCONCAT(T3.MC3Type, '') AS MC3, T1.MC3Name, T1.MC3Status, T1.MC3ETA, " +
                "\nCONCAT(T10.Name, '') AS LastMQCName, T7.MQCStatus, T8.LastMQCDate, COALESCE(TotalQty,0) AS MQCTotalQty, " +
                "\n\tCASE " +
                "\n\t\tWHEN T1.PosCStatus = 2 THEN 'OK' " +
                "\n\t\tELSE 'NOT YET' " +
                "\n\tEND AS POSStatus, T6.LastRegDate FROM " +
                "\n(SELECT * FROM tbPOSDetailofMC) T1 " +
                "\nLEFT JOIN (SELECT Remarks2, Remarks, ItemCode FROM tbMasterItem WHERE LEN(ItemCode) < 5 AND ItemCode > 5000)T2 " +
                "\nON T1.FGCode = T2.ItemCode " +
                "\nLEFT JOIN (SELECT T1.ItemCode, ItemName, Remarks1, Remarks2, Remarks3, CONCAT(MC1Type, '') AS MC1Type, CONCAT(MC2Type, '') AS MC2Type, CONCAT(MC3Type, '') AS MC3Type, T2.Slot FROM (SELECT ItemCode, ItemName, Remarks1, Remarks2, Remarks3 FROM tbMasterItem WHERE LEN(ItemCode) > 4) T1 LEFT JOIN(SELECT ItemCode, MC1Type, MC2Type, MC3Type, Slot FROM tbMasterItemPlan) T2 ON T1.ItemCode = T2.ItemCode) T3 " +
                "\nON T1.WIPCode = T3.ItemCode " +
                "\nLEFT JOIN (SELECT * FROM tbMasterMCType) T4 " +
                "\nON T1.MC1Name = T4.MCName " +
                "\nLEFT JOIN (SELECT POSNo, MAX(RegDate) AS LastRegDate FROM tbSDConn1Rec Group By POSNo) T5 " +
                "\nON T1.PosCNo = T5.POSNo " +
                "\nLEFT JOIN (SELECT PosNo, MAX(RegDate) AS LastRegDate FROM tbWipTransactions GROUP BY PosNo) T6 " +
                "\nON T1.PosCNo = T6.PosNo " +
                "\nLEFT JOIN (SELECT PosCNo, STRING_AGG(MQCStatus, ',') AS MQCStatus FROM (SELECT DISTINCT PosCNo, MQCStatus FROM tbMQCData WHERE MQCStatus IS NOT NULL) AS DistinctStatuses GROUP BY PosCNo) T7 " +
                "\nON T1.PosCNo=T7.PosCNo " +
                "\nLEFT JOIN (SELECT PosCNo, MAX(MQCDate) AS LastMQCDate FROM tbMQCData WHERE MQCStatus IS NOT NULL GROUP BY PosCNo) T8 " +
                "\nON T7.PosCNo=T8.PosCNo " +
                "\nLEFT JOIN (SELECT * FROM tbMQCData) T9 " +
                "\nON T8.PosCNo=T9.PosCNo AND T8.LastMQCDate=T9.MQCDate " +
                "\nLEFT JOIN (SELECT * FROM TD_DatabaseTemp.dbo.Employee_list) T10 " +
                "\nON T9.MQCID=T10.Staff_No " +
                "\nLEFT JOIN (SELECT PosCNo, SUM(Qty) AS TotalQty  FROM tbMQCData GROUP BY PosCNo) T11 " +
                "\nON T7.PosCNo=T11.PosCNo " +
                "\nLEFT JOIN (SELECT POSNo, MAX(RegDate) AS KITLastTransfer FROM tbSDMCAllTransaction WHERE LocCode='KIT3' AND Funct=2 AND CancelStatus=0 GROUP BY POSNo) T12 " +
                "\nON T1.PosCNo=T12.POSNo \n";
            SQLQueryText += SQLCond + "\nORDER BY T1.PosCNo ASC";

            //Console.WriteLine(SQLQueryText);

            SqlDataAdapter sda = new SqlDataAdapter(SQLQueryText, cnn.con);

            DataTable dt = new DataTable();
            DataTable dtActualKIT = new DataTable();
            DataTable dtFGLineCode = new DataTable();
            DataTable dtLabelStatus = new DataTable();
            sda.Fill(dt);
            cnn.con.Close();

            if (dt.Rows.Count > 0)
            {
                //Remove Duplicate
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    int Count = 0;
                    string POSNo = dt.Rows[i][9].ToString();

                    DataRow dr = dt.Rows[i];

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j][9].ToString() == dr[9].ToString())
                        {
                            Count = Count + 1;

                        }
                    }
                    
                    if (Count > 1)
                    {
                        dr.Delete();
                        dt.AcceptChanges();
                    }
                }
                
                string minimumPOS = dt.Rows[0][9].ToString();
                string maximumPOS = dt.Rows[dt.Rows.Count - 1][9].ToString();

                cnnOBS.conOBS.Open();
                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT Remark, MAX(CreateDate) AS CreateDate FROM prgalltransaction WHERE LocCode='KIT2' AND GRICode='60' AND Remark BETWEEN '" + minimumPOS + "' AND '" + maximumPOS + "' GROUP BY Remark", cnnOBS.conOBS);
                sda1.Fill(dtActualKIT);
                SqlDataAdapter sda2 = new SqlDataAdapter("SELECT ItemCode FROM mstitem WHERE ItemType=0 AND DefaultSupplier='SUP005'", cnnOBS.conOBS);
                sda2.Fill(dtFGLineCode);
                SqlDataAdapter sda3 = new SqlDataAdapter("SELECT DONo, " +
                                                                                    "CASE " +
                                                                                    "   WHEN PlanQty-Resv8=0 THEN 'OK' " +
                                                                                    "   ELSE 'NOT YET' " +
                                                                                    "END AS LabelStatus FROM prgproductionorder WHERE DONo BETWEEN '" + minimumPOS + "' AND '" + maximumPOS + "' ", cnnOBS.conOBS);
                sda3.Fill(dtLabelStatus);
                cnnOBS.conOBS.Close();
            }

            foreach (DataRow row in dt.Rows)
            {
                int Found = 0;
                foreach (DataRow rowFGLine in dtFGLineCode.Rows)
                {
                    if (row["FGCode"].ToString() == rowFGLine["ItemCode"].ToString())
                    {
                        row["LineCode"] = "S3N";
                        Found = Found + 1;
                        break;
                    }
                }
                if (Found == 0)
                {
                    row["LineCode"] = "ASSY";
                }
            }
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataRow rowActualKIT in dtActualKIT.Rows)
                {
                    if (row["PosCNo"].ToString() == rowActualKIT["Remark"].ToString())
                    {
                        row["ActualKIT"] = rowActualKIT["CreateDate"].ToString();
                        break;
                    }
                }
            }
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataRow rowLabelStat in dtLabelStatus.Rows)
                {
                    if (row["PosCNo"].ToString() == rowLabelStat["DONo"].ToString())
                    {
                        row["LabelStatus"] = rowLabelStat["LabelStatus"].ToString();
                        break;
                    }
                }
            }

            //Assign MC Name,.....etc.
            foreach (DataRow row in dt.Rows)
            {
                //MC Type from MCName
                if (row["MC1Name"].ToString().Trim() != "")
                {
                    foreach (DataRow rowMCName in dtMCName.Rows)
                    {
                        if (row["MC1Name"].ToString() == rowMCName["MCName"].ToString())
                        {
                            row["MC1"] = rowMCName["MCType"].ToString();
                            break;
                        }
                    }
                }
                if (row["MC2Name"].ToString().Trim() != "")
                {
                    foreach (DataRow rowMCName in dtMCName.Rows)
                    {
                        if (row["MC2Name"].ToString() == rowMCName["MCName"].ToString())
                        {
                            row["MC2"] = rowMCName["MCType"].ToString();
                            break;
                        }
                    }
                }
                if (row["MC3Name"].ToString().Trim() != "")
                {
                    foreach (DataRow rowMCName in dtMCName.Rows)
                    {
                        if (row["MC3Name"].ToString() == rowMCName["MCName"].ToString())
                        {
                            row["MC3"] = rowMCName["MCType"].ToString();
                            break;
                        }
                    }
                }

            }

            //Add to DGV
            double TotalQty = 0;
            foreach (DataRow row in dt.Rows)
            {
                string Customer = row["Customer"].ToString();
                string Model = row["Model"].ToString();
                string LineCode = row["LineCode"].ToString();
                string FGCode = row["FGCode"].ToString();
                string PosPNo = row["PosPNo"].ToString();
                string PosPQty = row["PosPQty"].ToString();
                DateTime PosPDelDate = Convert.ToDateTime(row["PosPDelDate"].ToString());
                string WIPCode = row["WIPCode"].ToString();
                string WIPName = row["WIPName"].ToString();
                string PosCNo = row["PosCNo"].ToString();
                string PINAssign = row["PINAssign"].ToString();
                string WireTube = row["WireTube"].ToString();
                string Length = row["Length"].ToString();
                int SemiQty = Convert.ToInt32(row["SemiQty"].ToString());
                int PosCQty = Convert.ToInt32(row["PosCQty"].ToString());
                int PosCResultQty = Convert.ToInt32(row["PosCResultQty"].ToString());
                int PosCRemainQty = Convert.ToInt32(row["PosCRemainQty"].ToString());
                string Remarks = row["Remarks"].ToString();
                string DocNo = row["DocNo"].ToString();
                DateTime KITPromise = Convert.ToDateTime(row["KITPromise"].ToString());
                Nullable<DateTime> ActualKIT = null;
                if (row["ActualKIT"].ToString().Trim() != "")
                {
                    ActualKIT = Convert.ToDateTime(row["ActualKIT"].ToString());
                }
                Nullable<DateTime> KITSD = null;
                if (row["KITSD"].ToString().Trim() != "")
                {
                    KITSD = Convert.ToDateTime(row["KITSD"].ToString());
                }
                Nullable<DateTime> KITLastTransfer = null;
                if (row["KITLastTransfer"].ToString().Trim() != "")
                {
                    KITLastTransfer = Convert.ToDateTime(row["KITLastTransfer"].ToString());
                }                
                string LabelStatus = row["LabelStatus"].ToString();
                string Slot = row["Slot"].ToString();
                string PlanSeqNo = row["PlanSeqNo"].ToString();
                string MC1 = row["MC1"].ToString();
                string MC1Name = row["MC1Name"].ToString();
                string MC1Status = row["MC1Status"].ToString();
                if (MC1Status.Trim() != "")
                {
                    if (MC1Status == "0")
                    {
                        MC1Status = "STOP";
                    }
                    else if (MC1Status == "1")
                    {
                        MC1Status = "RUN";
                    }
                    else
                    {
                        MC1Status = "FINISH";
                    }
                }
                Nullable<DateTime> MC1ETA = null;
                if (row["MC1ETA"].ToString().Trim() != "")
                {
                    MC1ETA = Convert.ToDateTime(row["MC1ETA"].ToString());
                }
                string MC2 = row["MC2"].ToString();
                string MC2Name = row["MC2Name"].ToString();
                string MC2Status = row["MC2Status"].ToString();
                if (MC2Status.Trim() != "")
                {
                    if (MC2Status == "0")
                    {
                        MC2Status = "STOP";
                    }
                    else if (MC2Status == "1")
                    {
                        MC2Status = "RUN";
                    }
                    else
                    {
                        MC2Status = "FINISH";
                    }
                }
                Nullable<DateTime> MC2ETA = null;
                if (row["MC2ETA"].ToString().Trim() != "")
                {
                    MC2ETA = Convert.ToDateTime(row["MC2ETA"].ToString());
                }
                string MC3 = row["MC3"].ToString();
                string MC3Name = row["MC3Name"].ToString();
                string MC3Status = row["MC3Status"].ToString();
                if (MC3Status.Trim() != "")
                {
                    if (MC3Status == "0")
                    {
                        MC3Status = "STOP";
                    }
                    else if (MC3Status == "1")
                    {
                        MC3Status = "RUN";
                    }
                    else
                    {
                        MC3Status = "FINISH";
                    }
                }
                Nullable<DateTime> MC3ETA = null;
                if (row["MC3ETA"].ToString().Trim() != "")
                {
                    MC3ETA = Convert.ToDateTime(row["MC3ETA"].ToString());
                }
                string LastMQCName = row["LastMQCName"].ToString();
                if (LastMQCName.Trim() != "")
                {
                    string[] Name = LastMQCName.Split(' ');
                    LastMQCName = "";
                    for (int i = 1; i < Name.Length; i++)
                    {
                        LastMQCName += Name[i].ToString();
                    }
                }
                int MQCTotalQty = Convert.ToInt32(row["MQCTotalQty"].ToString());
                string MQCStatus = row["MQCStatus"].ToString();
                if (MQCStatus.Trim() != "")
                {
                    if (MQCStatus.Trim() != "FINISH")
                    {
                        if (MQCStatus.Trim() != "STOP")
                        {
                            MQCStatus = "RUN";
                        }
                    }
                    else
                    {
                        if (PosCQty > MQCTotalQty)
                        {
                            MQCStatus = "RUN";
                        }
                    }
                    
                    Console.WriteLine(PosCNo + "\t" + MQCStatus);

                }
                Nullable<DateTime> LastMQCDate = null;
                if (row["LastMQCDate"].ToString().Trim() != "")
                {
                    LastMQCDate = Convert.ToDateTime(row["LastMQCDate"].ToString());
                }
                string POSStatus = row["POSStatus"].ToString();
                Nullable<DateTime> LastRegDate = null;
                if (row["LastRegDate"].ToString().Trim() != "")
                {
                    LastRegDate = Convert.ToDateTime(row["LastRegDate"].ToString());
                }

                dgvSearchResult.Rows.Add();
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Customer"].Value = Customer;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Model"].Value = Model;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["LineCode"].Value = LineCode;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["FGCode"].Value = FGCode;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["PosPNo"].Value = PosPNo;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["PosPQty"].Value = PosPQty;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["PosPDelDate"].Value = PosPDelDate;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["WIPCode"].Value = WIPCode;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["WIPName"].Value = WIPName;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["PosCNo"].Value = PosCNo;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["PINAssign"].Value = PINAssign;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["WireTube"].Value = WireTube;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Length"].Value = Length;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["SemiQty"].Value = SemiQty;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["PosCQty"].Value = PosCQty;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["PosCResultQty"].Value = PosCResultQty;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["PosCRemainQty"].Value = PosCRemainQty;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Remarks"].Value = Remarks;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["DocNo"].Value = DocNo;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["KITPromise"].Value = KITPromise;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["ActualKIT"].Value = ActualKIT;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["KITSD"].Value = KITSD;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["KITLastTransfer"].Value = KITLastTransfer;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["LabelStatus"].Value = LabelStatus;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Slot"].Value = Slot;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["PlanSeqNo"].Value = PlanSeqNo;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["MC1"].Value = MC1;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["MC1Name"].Value = MC1Name;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["MC1Status"].Value = MC1Status;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["MC1ETA"].Value = MC1ETA;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["MC2"].Value = MC2;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["MC2Name"].Value = MC2Name;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["MC2Status"].Value = MC2Status;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["MC2ETA"].Value = MC2ETA;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["MC3"].Value = MC3;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["MC3Name"].Value = MC3Name;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["MC3Status"].Value = MC3Status;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["MC3ETA"].Value = MC3ETA;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["LastMQCName"].Value = LastMQCName;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["MQCStatus"].Value = MQCStatus;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["LastMQCDate"].Value = LastMQCDate;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["POSStatus"].Value = POSStatus;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["LastRegDate"].Value = LastRegDate;

                TotalQty = TotalQty + PosCQty;

                /*

                string Customer = row[0].ToString();
                string Model = row[1].ToString();
                string Line = row[2].ToString();
                string FGCode = row[3].ToString();
                string PosP = row[4].ToString();
                double PosPQty = Convert.ToDouble(row[5].ToString());
                DateTime PosPShipDate = Convert.ToDateTime(row[6].ToString());
                string WIPCode = row[7].ToString();
                string WIPName = row[8].ToString();
                string PosC = row[9].ToString();
                string PIN = row[10].ToString();
                string Wire = row[11].ToString();
                string Lenght = row[12].ToString();
                double SemiQty = Convert.ToDouble(row[13].ToString());
                double PosCQty = Convert.ToDouble(row[14].ToString());
                double TransQty = Convert.ToDouble(row[15].ToString());
                double RemainQty = Convert.ToDouble(row[16].ToString());
                string Remark = row[17].ToString();
                string DocNo = row[18].ToString();
                DateTime KITPromise = Convert.ToDateTime(row[19].ToString());
                string ActualKIT = "";
                if (row[20].ToString().Trim() != "")
                {
                    ActualKIT = Convert.ToDateTime(row[20].ToString()).ToString("dd-MM-yy hh:mm tt");
                }

                string KITSD = "";
                if (row[21].ToString().Trim() != "")
                {
                    KITSD = Convert.ToDateTime(row[21].ToString()).ToString("dd-MM-yy hh:mm tt");
                }

                string LabelStat = row[22].ToString();

                string Slot = "";
                if (row[23].ToString().Trim() != "")
                {
                    Slot = row[23].ToString();
                }

                string SeqNo = row[24].ToString();
                string MC1= row[25].ToString();
                string MC1Name = row[26].ToString(); 
                string MC1Stat = row[27].ToString();
                string MC1ETA = "";
                if (row[28].ToString().Trim() != "")
                {
                    MC1ETA = Convert.ToDateTime(row[28].ToString()).ToString("dd-MM-yy hh:mm tt");
                }

                string MC2 = row[29].ToString();
                string MC2Name = row[30].ToString();
                string MC2Stat = row[31].ToString();
                string MC2ETA = "";
                if (row[32].ToString().Trim() != "")
                {
                    MC2ETA = Convert.ToDateTime(row[32].ToString()).ToString("dd-MM-yy hh:mm tt");
                }

                string MC3 = row[33].ToString();
                string MC3Name = row[34].ToString();
                string MC3Stat = row[35].ToString();
                string MC3ETA = "";
                if (row[36].ToString().Trim() != "")
                {
                    MC3ETA = Convert.ToDateTime(row[36].ToString()).ToString("dd-MM-yy hh:mm tt");
                }

                string MQC = row["LastMQCName"].ToString();
                if (MQC.Trim() != "")
                {
                    string[] Name = MQC.Split(' ');
                    MQC = "";
                    for (int i = 1; i < Name.Length; i++)
                    {
                        MQC+= Name[i].ToString();
                    }
                }
                string MQCStat = row["MQCStatus"].ToString();                
                if (MQCStat.Trim() != "")
                {
                    if (MQCStat.Trim() != "FINISH")
                    {
                        Console.WriteLine(PosC + "\t" + MQCStat);
                        if (MQCStat.Trim() != "STOP")
                        {
                            MQCStat = "RUN";
                        }
                    }
                    Console.WriteLine(PosC + "\t" + MQCStat);

                }
                string MQCETA = "";
                if (row["LastMQCDate"].ToString().Trim() != "")
                {
                    MQCETA = Convert.ToDateTime(row["LastMQCDate"].ToString()).ToString("dd-MM-yy hh:mm tt");
                }


                string PosStatus = row[40].ToString();
                string LastReg = "";
                if (row[41].ToString().Trim() != "")
                {
                    LastReg = Convert.ToDateTime(row[41].ToString()).ToString("dd-MM-yy hh:mm tt");
                }

                dgvSearchResult.Rows.Add(Customer, Model, Line, FGCode, PosP, PosPQty, PosPShipDate, WIPCode, WIPName, PosC, PIN, Wire, Lenght, SemiQty, PosCQty, TransQty, RemainQty, Remark, DocNo, KITPromise, ActualKIT, KITSD, LabelStat, Slot, SeqNo, MC1, MC1Name, MC1Stat, MC1ETA, MC2, MC2Name, MC2Stat, MC2ETA, MC3, MC3Name, MC3Stat, MC3ETA, MQC, MQCStat, MQCETA, PosStatus, LastReg) ;
                
                */

            }

            Cursor = Cursors.Default;
            dgvSearchResult.ClearSelection();
            LbStatus.Text = "រកឃើញទិន្នន័យចំនួន " + dgvSearchResult.Rows.Count.ToString("N0") + " ស្មើនឹងចំនួន = " + TotalQty.ToString("N0");
            LbStatus.Refresh();

        }

        private void MCPlanStatusSearchForm_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn dgvCol in dgvSearchResult.Columns)
            {
                if (dgvCol.HeaderText.ToString().Contains("MC1") == true)
                {
                    dgvCol.HeaderCell.Style.BackColor = Color.SlateBlue;
                    dgvCol.HeaderCell.Style.SelectionBackColor = Color.SlateBlue;
                    dgvCol.HeaderCell.Style.SelectionForeColor = Color.White;
                    dgvCol.HeaderCell.Style.ForeColor = Color.White;
                }
                if (dgvCol.HeaderText.ToString().Contains("MC2") == true)
                {
                    dgvCol.HeaderCell.Style.BackColor = Color.Black;
                    dgvCol.HeaderCell.Style.SelectionBackColor = Color.Black;
                    dgvCol.HeaderCell.Style.SelectionForeColor = Color.White;
                    dgvCol.HeaderCell.Style.ForeColor = Color.White;
                }
                if (dgvCol.HeaderText.ToString().Contains("MC3") == true)
                {
                    dgvCol.HeaderCell.Style.BackColor = Color.DodgerBlue;
                    dgvCol.HeaderCell.Style.SelectionBackColor = Color.DodgerBlue;
                    dgvCol.HeaderCell.Style.SelectionForeColor = Color.White;
                    dgvCol.HeaderCell.Style.ForeColor = Color.White;
                }
            }

            dtColor = new DataTable();
            dtColor.Columns.Add("ShortText");
            dtColor.Columns.Add("ColorText");
            dtColor.Columns.Add("BackColor");
            dtColor.Rows.Add("RED", "White", "Red");
            dtColor.Rows.Add("BLK", "White", "Black");
            dtColor.Rows.Add("PINK", "Black", "Pink");
            dtColor.Rows.Add("YEL", "Black", "Yellow");
            dtColor.Rows.Add("BLU", "White", "Blue");
            dtColor.Rows.Add("BRN", "White", "Brown");
            dtColor.Rows.Add("G/Y", "Black", "GreenYellow");
            dtColor.Rows.Add("GRN", "White", "Green");
            dtColor.Rows.Add("GRY", "White", "Gray");
            dtColor.Rows.Add("ORG", "Black", "Orange");
            dtColor.Rows.Add("PNK", "Black", "Pink");
            dtColor.Rows.Add("SKY", "Black", "SkyBlue");
            dtColor.Rows.Add("VLT", "White", "Purple");
            dtColor.Rows.Add("WHT", "Black", "White");

            for (int i = 0; i < 15; i++)
            {
                dgvSearchResult.Columns[i].Frozen = true;
            }

            CboStatus.Items.Add("NOT YET");
            CboStatus.Items.Add("OK");
            CboStatus.Items.Add("All");
            CboStatus.SelectedIndex = 0;

            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT MCType, MCName FROM tbMasterMCType", cnn.con);
                dtMCName = new DataTable();
                sda.Fill(dtMCName);

                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT MCType FROM tbMasterItemPlan2", cnn.con);
                DataTable dtMCType = new DataTable();
                sda1.Fill(dtMCType);


                CboMC1.Items.Add("");                
                foreach (DataRow row in dtMCType.Rows)
                {
                    CboMC1.Items.Add(row[0]);
                    
                }
                CboMC1.SelectedIndex = 0;                 
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n"+ex.Message,"Rachhan System",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

            CboPrintStatus.Items.Add("NOT YET");
            CboPrintStatus.Items.Add("OK");
            CboPrintStatus.Items.Add("All");
            CboPrintStatus.SelectedIndex = 0;

        }

        private void ExportMasterPlanKIT()
        {
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងទាញឯកសារ Excel . . .";
            LbStatus.Refresh();
            LbStatus.Visible = true;
            var CDirectory = Environment.CurrentDirectory;
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\POSDetailMCKITTemplate.xlsx", Editable: true);
            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["RachhanSystem"];

            if (dgvSearchResult.Rows.Count > 1)
            {
                
                worksheet.Range["3:" + (dgvSearchResult.Rows.Count + 1)].Insert();
                worksheet.Range["A3:J" + (dgvSearchResult.Rows.Count + 1)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            }
            foreach (DataGridViewRow DgvRow in dgvSearchResult.Rows)
            {
                string PosP = DgvRow.Cells[4].Value.ToString();
                string PosPQty = DgvRow.Cells[5].Value.ToString();
                DateTime ShipDate = Convert.ToDateTime(DgvRow.Cells[6].Value.ToString());
                string WIPCode = DgvRow.Cells[7].Value.ToString();
                string WIPName = DgvRow.Cells[8].Value.ToString();
                string PosCQty = DgvRow.Cells[9].Value.ToString();
                string ITNo = DgvRow.Cells[18].Value.ToString();
                string KITP = "";
                if (DgvRow.Cells[19].Value != null)
                {
                    if (DgvRow.Cells[19].Value.ToString().Trim() != "")
                    {
                        KITP = Convert.ToDateTime(DgvRow.Cells[19].Value.ToString()).ToString("dd-MM-yyyy");
                    }
                }
                string KITA = "";
                if (DgvRow.Cells[20].Value != null)
                {
                    if (DgvRow.Cells[20].Value.ToString().Trim() != "")
                    {
                        KITA = Convert.ToDateTime(DgvRow.Cells[20].Value.ToString()).ToString("dd-MM-yyyy");
                    }
                }
                string SLOT = DgvRow.Cells[23].Value.ToString();

                worksheet.Cells[DgvRow.Index + 2, 1] = PosP;
                worksheet.Cells[DgvRow.Index + 2, 2] = PosPQty;
                worksheet.Cells[DgvRow.Index + 2, 3] = ShipDate;
                worksheet.Cells[DgvRow.Index + 2, 4] = WIPCode;
                worksheet.Cells[DgvRow.Index + 2, 5] = WIPName;
                worksheet.Cells[DgvRow.Index + 2, 6] = PosCQty;
                worksheet.Cells[DgvRow.Index + 2, 7] = ITNo;
                if (KITP.Trim() != "")
                {
                    worksheet.Cells[DgvRow.Index + 2, 8] = Convert.ToDateTime(KITP);

                }
                if (KITA.Trim() != "")
                {
                    worksheet.Cells[DgvRow.Index + 2, 9] = Convert.ToDateTime(KITA);

                }
                worksheet.Cells[DgvRow.Index + 2, 10] = SLOT;

            }


            //ឆែករកមើល Folder បើគ្មាន => បង្កើត
            string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\POSDetailMCKIT";
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }

            // Saving the modified Excel file                        
            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
            string file = "POSDetailMCKIT ";
            fName = file + "( " + date + " )";
            worksheet.SaveAs(CDirectory.ToString() + @"\Report\POSDetailMCKIT\" + fName + ".xlsx");
            xlWorkBook.Save();
            xlWorkBook.Close();
            excelApp.Quit();

            //Kill all Excel background process
            var processes = from p in Process.GetProcessesByName("EXCEL")
                            select p;
            foreach (var process in processes)
            {
                if (process.MainWindowTitle.ToString().Trim() == "")
                    process.Kill();
            }

            Cursor = Cursors.Default;
            LbStatus.Text = "ឯកសារ Excel រួចរាល់!";
            LbStatus.Refresh();
            MessageBox.Show("ឯកសារ Excel រួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
            System.Diagnostics.Process.Start(CDirectory.ToString() + @"\\Report\POSDetailMCKIT\" + fName + ".xlsx");
            fName = "";

        }

        private void ExportMasterPlanAll()
        {
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងទាញឯកសារ Excel . . .";
            LbStatus.Refresh();
            LbStatus.Visible = true;
            var CDirectory = Environment.CurrentDirectory;
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\POSDetailMCTemplate.xlsx", Editable: true);
            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["RachhanSystem"];

            if (dgvSearchResult.Rows.Count > 1)
            {
                worksheet.Range["8:" + (dgvSearchResult.Rows.Count + 6)].Insert();

            }

            worksheet.Cells[2, 9] = DateTime.Now;
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                string Customer = dgvRow.Cells["Customer"].Value.ToString();
                string Model = dgvRow.Cells["Model"].Value.ToString();
                string Line = dgvRow.Cells["LineCode"].Value.ToString();
                string FGCode = dgvRow.Cells["FGCode"].Value.ToString();
                string POSPNo = dgvRow.Cells["PosPNo"].Value.ToString();
                int PQty = Convert.ToInt32(dgvRow.Cells["PosPQty"].Value.ToString());
                DateTime DelDate = Convert.ToDateTime(dgvRow.Cells["PosPDelDate"].Value.ToString());
                string WIPCode = dgvRow.Cells["WIPCode"].Value.ToString();
                string WIPName = dgvRow.Cells["WIPName"].Value.ToString();
                string POSCNo = dgvRow.Cells["PosCNo"].Value.ToString();
                string PIN = dgvRow.Cells["PINAssign"].Value.ToString();
                string Wire = dgvRow.Cells["WireTube"].Value.ToString();
                string Length = dgvRow.Cells["Length"].Value.ToString();
                int SemiQty = Convert.ToInt32(dgvRow.Cells["SemiQty"].Value.ToString());
                int CQty = Convert.ToInt32(dgvRow.Cells["PosCQty"].Value.ToString());
                int PassedQty = Convert.ToInt32(dgvRow.Cells["PosCResultQty"].Value.ToString());
                int RemainQty = Convert.ToInt32(dgvRow.Cells["PosCRemainQty"].Value.ToString());
                string Remarks = dgvRow.Cells["Remarks"].Value.ToString();
                string DocNo = dgvRow.Cells["DocNo"].Value.ToString();
                DateTime KITPromiss = Convert.ToDateTime(dgvRow.Cells["KITPromise"].Value.ToString());
                Nullable<DateTime> ActualKIT = null;
                if (dgvRow.Cells["ActualKIT"].Value != null && dgvRow.Cells["ActualKIT"].Value.ToString().Trim() != "")
                {
                    ActualKIT = Convert.ToDateTime(dgvRow.Cells["ActualKIT"].Value.ToString());
                }
                Nullable<DateTime> KITSD = null;
                if (dgvRow.Cells["KITSD"].Value != null && dgvRow.Cells["KITSD"].Value.ToString().Trim() != "")
                {
                    KITSD = Convert.ToDateTime(dgvRow.Cells["KITSD"].Value.ToString());
                }
                Nullable<DateTime> KITSDTrans = null;
                if (dgvRow.Cells["KITLastTransfer"].Value != null && dgvRow.Cells["KITLastTransfer"].Value.ToString().Trim() != "")
                {
                    KITSDTrans = Convert.ToDateTime(dgvRow.Cells["KITLastTransfer"].Value.ToString());
                }
                string MC1Label = dgvRow.Cells["LabelStatus"].Value.ToString();
                string MC1Slot = dgvRow.Cells["Slot"].Value.ToString();
                string PlanSeqNo = dgvRow.Cells["PlanSeqNo"].Value.ToString();
                string MC1 = dgvRow.Cells["MC1"].Value.ToString();
                string MC1Name = dgvRow.Cells["MC1Name"].Value.ToString();
                string MC1Status = dgvRow.Cells["MC1Status"].Value.ToString();
                Nullable<DateTime> MC1Date = null;
                if (dgvRow.Cells["MC1ETA"].Value != null && dgvRow.Cells["MC1ETA"].Value.ToString().Trim() != "")
                {
                    MC1Date = Convert.ToDateTime(dgvRow.Cells["MC1ETA"].Value.ToString());
                }
                string MC2 = dgvRow.Cells["MC2"].Value.ToString();
                string MC2Name = dgvRow.Cells["MC2Name"].Value.ToString();
                string MC2Status = dgvRow.Cells["MC2Status"].Value.ToString();
                Nullable<DateTime> MC2Date = null;
                if (dgvRow.Cells["MC2ETA"].Value != null && dgvRow.Cells["MC2ETA"].Value.ToString().Trim() != "")
                {
                    MC2Date = Convert.ToDateTime(dgvRow.Cells["MC2ETA"].Value.ToString());
                }
                string MC3 = dgvRow.Cells["MC3"].Value.ToString();
                string MC3Name = dgvRow.Cells["MC3Name"].Value.ToString();
                string MC3Status = dgvRow.Cells["MC3Status"].Value.ToString();
                Nullable<DateTime> MC3Date = null;
                if (dgvRow.Cells["MC3ETA"].Value != null && dgvRow.Cells["MC3ETA"].Value.ToString().Trim() != "")
                {
                    MC3Date = Convert.ToDateTime(dgvRow.Cells["MC3ETA"].Value.ToString());
                }
                string MQCName = dgvRow.Cells["LastMQCName"].Value.ToString();
                string MQCStatus = dgvRow.Cells["MQCStatus"].Value.ToString();
                Nullable<DateTime> MQCDate = null;
                if (dgvRow.Cells["LastMQCDate"].Value != null && dgvRow.Cells["LastMQCDate"].Value.ToString().Trim() != "")
                {
                    MQCDate = Convert.ToDateTime(dgvRow.Cells["LastMQCDate"].Value.ToString());
                }
                string Status = dgvRow.Cells["POSStatus"].Value.ToString();
                Nullable<DateTime> LastWIPTrans = null;
                if (dgvRow.Cells["LastRegDate"].Value != null && dgvRow.Cells["LastRegDate"].Value.ToString().Trim() != "")
                {
                    LastWIPTrans = Convert.ToDateTime(dgvRow.Cells["LastRegDate"].Value.ToString());
                }

                worksheet.Cells[dgvRow.Index + 7, 1] = Customer;
                worksheet.Cells[dgvRow.Index + 7, 2] = Model;
                worksheet.Cells[dgvRow.Index + 7, 3] = Line;
                worksheet.Cells[dgvRow.Index + 7, 4] = FGCode;
                worksheet.Cells[dgvRow.Index + 7, 5] = POSPNo;
                worksheet.Cells[dgvRow.Index + 7, 6] = PQty;
                worksheet.Cells[dgvRow.Index + 7, 7] = DelDate;
                worksheet.Cells[dgvRow.Index + 7, 8] = WIPCode;
                worksheet.Cells[dgvRow.Index + 7, 9] = WIPName;
                worksheet.Cells[dgvRow.Index + 7, 10] = POSCNo;
                worksheet.Cells[dgvRow.Index + 7, 11] = PIN;
                worksheet.Cells[dgvRow.Index + 7, 12] = Wire;
                worksheet.Cells[dgvRow.Index + 7, 13] = Length;
                worksheet.Cells[dgvRow.Index + 7, 14] = SemiQty;
                worksheet.Cells[dgvRow.Index + 7, 15] = CQty;
                worksheet.Cells[dgvRow.Index + 7, 16] = PassedQty;
                worksheet.Cells[dgvRow.Index + 7, 17] = RemainQty;
                worksheet.Cells[dgvRow.Index + 7, 18] = Remarks;
                worksheet.Cells[dgvRow.Index + 7, 19] = DocNo;
                worksheet.Cells[dgvRow.Index + 7, 20] = KITPromiss;
                worksheet.Cells[dgvRow.Index + 7, 21] = ActualKIT;
                worksheet.Cells[dgvRow.Index + 7, 22] = KITSD;
                worksheet.Cells[dgvRow.Index + 7, 23] = KITSDTrans;
                worksheet.Cells[dgvRow.Index + 7, 24] = MC1Label;
                worksheet.Cells[dgvRow.Index + 7, 25] = MC1Slot;
                worksheet.Cells[dgvRow.Index + 7, 26] = PlanSeqNo;
                worksheet.Cells[dgvRow.Index + 7, 27] = MC1;
                worksheet.Cells[dgvRow.Index + 7, 28] = MC1Name;
                worksheet.Cells[dgvRow.Index + 7, 29] = MC1Status;
                worksheet.Cells[dgvRow.Index + 7, 30] = MC1Date;
                worksheet.Cells[dgvRow.Index + 7, 31] = MC2;
                worksheet.Cells[dgvRow.Index + 7, 32] = MC2Name;
                worksheet.Cells[dgvRow.Index + 7, 33] = MC2Status;
                worksheet.Cells[dgvRow.Index + 7, 34] = MC2Date;
                worksheet.Cells[dgvRow.Index + 7, 35] = MC3;
                worksheet.Cells[dgvRow.Index + 7, 36] = MC3Name;
                worksheet.Cells[dgvRow.Index + 7, 37] = MC3Status;
                worksheet.Cells[dgvRow.Index + 7, 38] = MC3Date;
                worksheet.Cells[dgvRow.Index + 7, 39] = MQCName;
                worksheet.Cells[dgvRow.Index + 7, 40] = MQCStatus;
                worksheet.Cells[dgvRow.Index + 7, 41] = MQCDate;
                worksheet.Cells[dgvRow.Index + 7, 42] = Status;
                worksheet.Cells[dgvRow.Index + 7, 43] = LastWIPTrans;

            }

            worksheet.Range["A7:AQ" + (dgvSearchResult.Rows.Count + 6)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            worksheet.Range["A" + (dgvSearchResult.Rows.Count + 6) + ":AQ" + (dgvSearchResult.Rows.Count + 6)].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;


            //ឆែករកមើល Folder បើគ្មាន => បង្កើត
            string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\POSDetailMC";
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }

            // Saving the modified Excel file                        
            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
            string file = "POSDetailMC ";
            fName = file + "( " + date + " )";
            worksheet.SaveAs(CDirectory.ToString() + @"\Report\POSDetailMC\" + fName + ".xlsx");
            xlWorkBook.Save();
            xlWorkBook.Close();
            excelApp.Quit();

            //Kill all Excel background process
            var processes = from p in Process.GetProcessesByName("EXCEL")
                            select p;
            foreach (var process in processes)
            {
                if (process.MainWindowTitle.ToString().Trim() == "")
                    process.Kill();
            }
                        
            Cursor = Cursors.Default;
            LbStatus.Text = "ឯកសារ Excel រួចរាល់!";
            LbStatus.Refresh();
            MessageBox.Show("ឯកសារ Excel រួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
            System.Diagnostics.Process.Start(CDirectory.ToString() + @"\\Report\POSDetailMC\" + fName + ".xlsx");
            fName = "";
        }

    }
}
