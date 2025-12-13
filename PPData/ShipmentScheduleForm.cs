using MachineDeptApp;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PP_Dept_App._4FinalPlan
{
    public partial class ShipmentScheduleForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        string AddOrUpdate;
        int AssyDay = 7;
        int MCDay = 7;
        int KITDay = 5;
        int WHDay = 5;
        int KITReqDay = 1;
        int POSIssueDay = 2;
        string FormatDate = "dd-MMM";

        //DateTime ShipmentDateForCalc;
        string ErrorText;


        public ShipmentScheduleForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Load += ShipmentScheduleForm_Load;
            //Btn
            this.btnSearch.Click += BtnSearch_Click;
            this.btnExport.Click += BtnExport_Click;
        }

       
        private void ShipmentScheduleForm_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn dgvCol in dgvShipmentPlans.Columns)
            {
                //Set Header BackColor
                if (dgvCol.HeaderText.ToString().Contains("W/H") == true)
                {
                    dgvCol.HeaderCell.Style.BackColor = Color.FromKnownColor(KnownColor.DarkSeaGreen);
                    dgvCol.HeaderCell.Style.ForeColor = Color.Black;
                    dgvCol.HeaderCell.Style.SelectionBackColor = Color.FromKnownColor(KnownColor.DarkSeaGreen);
                    dgvCol.HeaderCell.Style.SelectionForeColor = Color.Black;
                }
                if (dgvCol.HeaderText.ToString().Contains("KIT") == true)
                {
                    if (dgvCol.HeaderText.ToString().Contains("RQ") == false)
                    {
                        dgvCol.HeaderCell.Style.BackColor = Color.FromKnownColor(KnownColor.Pink);
                        dgvCol.HeaderCell.Style.ForeColor = Color.Black;
                        dgvCol.HeaderCell.Style.SelectionBackColor = Color.FromKnownColor(KnownColor.Pink);
                        dgvCol.HeaderCell.Style.SelectionForeColor = Color.Black;
                    }
                }
                if (dgvCol.HeaderText.ToString().Contains("MC") == true)
                {
                    dgvCol.HeaderCell.Style.BackColor = Color.FromKnownColor(KnownColor.Yellow);
                    dgvCol.HeaderCell.Style.ForeColor = Color.Black;
                    dgvCol.HeaderCell.Style.SelectionBackColor = Color.FromKnownColor(KnownColor.Yellow);
                    dgvCol.HeaderCell.Style.SelectionForeColor = Color.Black;
                }
                if (dgvCol.HeaderText.ToString().Contains("Assy") == true)
                {
                    dgvCol.HeaderCell.Style.BackColor = Color.FromKnownColor(KnownColor.Bisque);
                    dgvCol.HeaderCell.Style.ForeColor = Color.Black;
                    dgvCol.HeaderCell.Style.SelectionBackColor = Color.FromKnownColor(KnownColor.Bisque);
                    dgvCol.HeaderCell.Style.SelectionForeColor = Color.Black;
                }

                //Set Format
                if (dgvCol.Index < dgvShipmentPlans.Columns.Count - 5)
                {
                    if (dgvCol.Index > 0)
                    {
                        if (dgvCol.HeaderText.ToString() != "Day")
                        {
                            dgvCol.DefaultCellStyle.Format = FormatDate;
                        }
                    }
                }
            }
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            AddOrUpdate = "Add";
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "Searching . . .";
            LbStatus.Refresh();
            dgvShipmentPlans.Rows.Clear();
            ErrorText = "";

            //Set Condition of WHERE
            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Val");
            dtSQLCond.Rows.Add("Status = ", "'Active'");
            if (ChkShipmentDate.Checked == true)
            {
                dtSQLCond.Rows.Add("ShipmentDate BETWEEN ", "'" + dtpShipmentDateStart.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dtpShipmentDateEnd.Value.ToString("yyyy-MM-dd") + " 23:59:59'");
            }
            string SQLConds = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                if (SQLConds.Trim() == "")
                {
                    SQLConds = "WHERE " + row["Col"] + row["Val"];
                }
                else
                {
                    SQLConds += " AND " + row["Col"] + row["Val"];
                }
            }

            //Take Data from DB
            DataTable dtSearchResult = new DataTable();
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM [PPDeptDB].[dbo].[tbPOSSchedule] " + SQLConds + " ORDER BY ShipmentDate ASC", cnn.con);
                sda.Fill(dtSearchResult);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

            //Add to DGV
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtSearchResult.Rows)
                {
                    dgvShipmentPlans.Rows.Add();
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].HeaderCell.Value = dgvShipmentPlans.Rows.Count.ToString();
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["SysNo"].Value = Convert.ToInt32(row["SysNo"]);
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["POSIssue"].Value = Convert.ToDateTime(row["POSIssue"]);
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["KITReq"].Value = Convert.ToDateTime(row["KITReq"]);
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["WHStart"].Value = Convert.ToDateTime(row["WHStart"]);
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["WHFinish"].Value = Convert.ToDateTime(row["WHFinish"]);
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["WHDays"].Value = WHDay;
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["KITStart"].Value = Convert.ToDateTime(row["KITStart"]);
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["KITFinish"].Value = Convert.ToDateTime(row["KITFinish"]);
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["KITDays"].Value = KITDay;
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["MCStart"].Value = Convert.ToDateTime(row["MCStart"]);
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["MCFinish"].Value = Convert.ToDateTime(row["MCFinish"]);
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["MCDays"].Value = MCDay;
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["AssyStart"].Value = Convert.ToDateTime(row["AssyStart"]);
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["AssyFinish"].Value = Convert.ToDateTime(row["AssyFinish"]);
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["AssyDays"].Value = AssyDay;
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["Remarks"].Value = row["Remarks"].ToString();
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["ShipDate"].Value = Convert.ToDateTime(row["ShipmentDate"].ToString());
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["RegDate"].Value = Convert.ToDateTime(row["RegDate"]);
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["RegBy"].Value = row["RegBy"].ToString();
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["UpdateDate"].Value = Convert.ToDateTime(row["UpdateDate"]);
                    dgvShipmentPlans.Rows[dgvShipmentPlans.Rows.Count - 1].Cells["UpdateBy"].Value = row["UpdateBy"].ToString();

                    /*

                    if (AddOrUpdate == "Add")
                    {
                        dgvShipmentPlans.Rows.Add(SysNo, POSIssueDate, KITReqDate, WHStartDate, WHFinishDate, WHDay, KITStartDate, KITFinishDate, KITDay, MCStartDate, MCFinishDate, MCDay, AssyStartDate, AssyFinishDate, AssyDay, ShipmentDate, Remarks, RegDate, RegBy, UpdateDate, UpdateBy);
                        AssignSeqToDGV();
                    }
                    else
                    {
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[1].Value = POSIssueDate;
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[2].Value = KITReqDate;
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[3].Value = WHStartDate;
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[4].Value = WHFinishDate;
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[5].Value = WHDay;
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[6].Value = KITStartDate;
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[7].Value = KITFinishDate;
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[8].Value = KITDay;
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[9].Value = MCStartDate;
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[10].Value = MCFinishDate;
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[11].Value = MCDay;
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[12].Value = AssyStartDate;
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[13].Value = AssyFinishDate;
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[14].Value = AssyDay;
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[15].Value = ShipmentDate;
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[16].Value = Remarks;
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[19].Value = UpdateDate;
                        dgvShipmentPlans.Rows[dgvShipmentPlans.CurrentCell.RowIndex].Cells[20].Value = UpdateBy;
                    }

                    */

                }
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                LbStatus.Text = "Found " + dgvShipmentPlans.Rows.Count.ToString("N0") + " data";
                LbStatus.Refresh();
                dgvShipmentPlans.ClearSelection();
                dgvShipmentPlans.CurrentCell = null;
            }
            else
            {
                LbStatus.Text = "Something wrong!";
                LbStatus.Refresh();
                MessageBox.Show("Something wrong!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvShipmentPlans.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("Do you want to export?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;
                    string fName = "";

                    //Write to excel for printing
                    var CDirectory = Environment.CurrentDirectory;
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\POS Schedule_Template.xlsx", Editable: true);
                    Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["RachhanSystem"];

                    try
                    {
                        //Header Inv
                        worksheet.Cells[3, 1] = DateTime.Now;

                        worksheet.Range["A6"].EntireRow.Copy();
                        //Insert if over than 1
                        if (dgvShipmentPlans.Rows.Count > 1)
                        {
                            worksheet.Range["7:" + (7 + (dgvShipmentPlans.Rows.Count - 2)).ToString()].Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                            worksheet.Range["A7:T" + (7 + (dgvShipmentPlans.Rows.Count - 2)).ToString()].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        }


                        foreach (DataGridViewRow dgvRow in dgvShipmentPlans.Rows)
                        {
                            int SeqNo = Convert.ToInt32(dgvRow.HeaderCell.Value.ToString());


                            worksheet.Cells[dgvRow.Index + 6, 1] = SeqNo;
                            worksheet.Cells[dgvRow.Index + 6, 2] = dgvRow.Cells[1].Value;//POSIssue
                            worksheet.Cells[dgvRow.Index + 6, 3] = dgvRow.Cells[2].Value;//KITRQ
                            worksheet.Cells[dgvRow.Index + 6, 4] = dgvRow.Cells[3].Value;//WH S
                            worksheet.Cells[dgvRow.Index + 6, 5] = dgvRow.Cells[4].Value;//WH F
                            worksheet.Cells[dgvRow.Index + 6, 6] = dgvRow.Cells[5].Value;//Day
                            worksheet.Cells[dgvRow.Index + 6, 7] = dgvRow.Cells[6].Value;//KIT S
                            worksheet.Cells[dgvRow.Index + 6, 8] = dgvRow.Cells[7].Value;//KIT F
                            worksheet.Cells[dgvRow.Index + 6, 9] = dgvRow.Cells[8].Value;//Day
                            worksheet.Cells[dgvRow.Index + 6, 10] = dgvRow.Cells[9].Value;//MC S
                            worksheet.Cells[dgvRow.Index + 6, 11] = dgvRow.Cells[10].Value;//MC F
                            worksheet.Cells[dgvRow.Index + 6, 12] = dgvRow.Cells[11].Value;//Day
                            worksheet.Cells[dgvRow.Index + 6, 13] = dgvRow.Cells[12].Value;//Assy S
                            worksheet.Cells[dgvRow.Index + 6, 14] = dgvRow.Cells[13].Value;//Assy F
                            worksheet.Cells[dgvRow.Index + 6, 15] = dgvRow.Cells[14].Value;//Day
                            worksheet.Cells[dgvRow.Index + 6, 16] = dgvRow.Cells[15].Value;//Ship.Date
                            worksheet.Cells[dgvRow.Index + 6, 17] = "=N" + ((dgvRow.Index) + 6).ToString() + "-J" + ((dgvRow.Index) + 6).ToString();//Prod LT
                            worksheet.Cells[dgvRow.Index + 6, 18] = "=P" + ((dgvRow.Index) + 6).ToString() + "-N" + ((dgvRow.Index) + 6).ToString();//Spare Time
                            worksheet.Cells[dgvRow.Index + 6, 19] = "=N" + ((dgvRow.Index) + 6).ToString() + "-B" + ((dgvRow.Index) + 6).ToString();//TTL LT
                            worksheet.Cells[dgvRow.Index + 6, 20] = dgvRow.Cells[16].Value;//Remark
                        }

                        //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                        string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\POSSchedule";
                        if (!Directory.Exists(SavePath))
                        {
                            Directory.CreateDirectory(SavePath);
                        }

                        // Saving the modified Excel file                        
                        string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                        fName = "POS Schedule ( " + date + " )";
                        worksheet.SaveAs(CDirectory.ToString() + @"\Report\POSSchedule\" + fName + ".xlsx");
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

                    }
                    catch (Exception ex)
                    {
                        ErrorText = ex.Message;

                        excelApp.DisplayAlerts = false;
                        xlWorkBook.Close();
                        excelApp.DisplayAlerts = true;
                        excelApp.Quit();

                        //Kill all Excel background process
                        var processes = from p in Process.GetProcessesByName("EXCEL")
                                        select p;
                        foreach (var process in processes)
                        {
                            if (process.MainWindowTitle.ToString().Trim() == "")
                                process.Kill();
                        }
                    }




                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        MessageBox.Show("Export successfully.", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Diagnostics.Process.Start(Environment.CurrentDirectory.ToString() + @"\\Report\POSSchedule\" + fName + ".xlsx");
                        fName = "";
                    }
                    else
                    {
                        MessageBox.Show("Something wrong!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
