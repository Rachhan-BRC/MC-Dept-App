using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.MCPlans
{
    public partial class POSDataCheckAmountForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();

        string fName;

        public POSDataCheckAmountForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.btnSearch.Click += BtnSearch_Click;
            this.Load += POSDataCheckAmountForm_Load;
            this.btnPrint.Click += BtnPrint_Click;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (dgvSearchResult.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("Do you want to print?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    LbStatus.Text = "Printing as Excel . . .";
                    LbStatus.Refresh();
                    LbStatus.Visible = true;
                    var CDirectory = Environment.CurrentDirectory;
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\POS_Data_CheckTemplate.xlsx", Editable: true);

                    //POS​_Status
                    Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["POS​_Status"];
                    if (dgvSearchResult.Rows.Count > 1)
                    {
                        worksheet.Range["3:" + (3+dgvSearchResult.Rows.Count-2)].Insert();
                    }
                    for (int i = 0; i < dgvSearchResult.Rows.Count; i++)
                    {
                        for (int j = 0; j < dgvSearchResult.Columns.Count; j++)
                        {
                            worksheet.Cells[i+2, j + 1] = dgvSearchResult.Rows[i].Cells[j].Value;
                        }
                    }

                    //AmountChecking
                    Excel.Worksheet worksheet1 = (Excel.Worksheet)xlWorkBook.Sheets["AmountChecking"];
                    if (dgvSearchResult2.Rows.Count > 1)
                    {
                        worksheet1.Range["3:" + (3 + dgvSearchResult2.Rows.Count - 3)].Insert();
                    }
                    for (int i = 0; i < dgvSearchResult2.Rows.Count-1; i++)
                    {
                        for (int j = 0; j < dgvSearchResult2.Columns.Count; j++)
                        {
                            worksheet1.Cells[i + 2, j + 1] = dgvSearchResult2.Rows[i].Cells[j].Value;
                        }
                    }



                    //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                    string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\POS_Checking - Data";
                    if (!Directory.Exists(SavePath))
                    {
                        Directory.CreateDirectory(SavePath);
                    }

                    // Saving the modified Excel file                        
                    string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                    string file = "POSChecking - Data ";
                    fName = file + "( " + date + " )";
                    worksheet.SaveAs(CDirectory.ToString() + @"\Report\POS_Checking - Data\" + fName + ".xlsx");
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
                    LbStatus.Text = "Excel finished!";
                    LbStatus.Refresh();
                    MessageBox.Show("Excel finished!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(CDirectory.ToString() + @"\\Report\POS_Checking - Data\" + fName + ".xlsx");
                    fName = "";
                }
            }
        }

        private void POSDataCheckAmountForm_Load(object sender, EventArgs e)
        {
            CboStatus.Items.Add("ALL");
            CboStatus.Items.Add("IN PROCESS");
            CboStatus.Items.Add("FINISH");
            CboStatus.SelectedIndex = 0;

            dgvSearchResult2.Columns[5].HeaderCell.Style.BackColor = Color.YellowGreen;
            dgvSearchResult2.Columns[6].HeaderCell.Style.BackColor = Color.YellowGreen;
            dgvSearchResult2.Columns[7].HeaderCell.Style.BackColor = Color.Blue;
            dgvSearchResult2.Columns[7].HeaderCell.Style.ForeColor = Color.White;
            dgvSearchResult2.Columns[8].HeaderCell.Style.BackColor = Color.Blue;
            dgvSearchResult2.Columns[8].HeaderCell.Style.ForeColor = Color.White;


        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LbStatus.Text = "Searching . . .";
            LbStatus.Refresh();
            LbStatus.Visible = true;
            Cursor = Cursors.WaitCursor;
            dgvSearchResult.Rows.Clear();
            dgvSearchResult2.Rows.Clear();
            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Value");

            if (txtWIPName.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("ItemName LIKE ", "'%"+txtWIPName.Text+"%' ");
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
                    dtSQLCond.Rows.Add("DocNo IN ", "(" + DocNoIN + ") ");

                }
                else if (txtDocNo.Text.ToString().Contains("*") == true)
                {
                    DocNo = DocNo.Replace("*", "%");
                    dtSQLCond.Rows.Add("DocNo LIKE ", "'" + DocNo + "' ");
                }
                else
                {
                    dtSQLCond.Rows.Add("DocNo = ", "'" + DocNo + "' ");
                }
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
                    SQLConds = "AND " + row[0] + row[1];
                }
            }

            DataTable dtSearchResult = new DataTable();
            DataTable dtUPFromOBS = new DataTable();

            //select from DB
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT FGCode, ItemName, PosPNo, PosPQty, PosPDelDate, KITPromise, DocNo, RegDate, CAST(PosCStatus AS varchar) AS PosCStatus, '' AS TwoWeeks, '' AS UP, '' AS Amount, '' AS Line, Remarks2, Remarks FROM " +
                                                                                    "(SELECT FGCode, PosPNo, PosPQty, PosPDelDate, KITPromise, DocNo, CONVERT(datetime,CONVERT (varchar, RegDate, 23)) AS RegDate, PosCStatus FROM tbPOSDetailofMC " +
                                                                                    "WHERE PosPDelDate BETWEEN '"+dtpFrom.Value.ToString("yyyy-MM-dd")+" 00:00:00' AND '"+dtpToo.Value.ToString("yyyy-MM-dd")+" 23:59:59' " +
                                                                                    "GROUP BY FGCode, PosPNo, PosPQty, PosPDelDate, KITPromise, DocNo, CONVERT(datetime,CONVERT (varchar, RegDate, 23)), PosCStatus) T1 " +
                                                                                    "LEFT JOIN " +
                                                                                    "(SELECT ItemCode, ItemName, Remarks2, Remarks FROM tbMasterItem WHERE NOT Remarks1 IS NULL AND NOT TRIM(Remarks1)='' AND LEN(ItemCode)=4) T2 " +
                                                                                    "ON T1.FGCode=T2.ItemCode "+SQLConds+ " ORDER BY PosPNo ASC", cnn.con);
                
                sda.Fill(dtSearchResult);
            }
            catch(Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n"+ex.Message,"Rachhan System",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

            //Cal Status
            foreach (DataRow row in dtSearchResult.Rows)
            {
                string Status = "";
                foreach (DataRow row1 in dtSearchResult.Rows)
                {
                    if (row[2].ToString() == row1[2].ToString())
                    {
                        if (Status.Trim() == "")
                        {
                            Status = row1[8].ToString();
                        }
                        else
                        {
                            Status = Status + ", " + row1[8].ToString();
                        }
                    }
                }
                if (Status.Contains("1") == true || Status.Contains("0") == true)
                {
                    row[8] = "IN PROCESS";
                }
                else
                {
                    row[8] = "FINISH";
                }
            }
            dtSearchResult.AcceptChanges();

            //Remove Duplicate
            if (dtSearchResult.Rows.Count > 0)
            {
                for (int i = dtSearchResult.Rows.Count - 1; i >= 0; i--)
                {
                    int Count = 0;
                    string POSNo = dtSearchResult.Rows[i][2].ToString();

                    DataRow dr = dtSearchResult.Rows[i];

                    for (int j = 0; j < dtSearchResult.Rows.Count; j++)
                    {
                        if (dtSearchResult.Rows[j][2].ToString() == dr[2].ToString())
                        {
                            Count = Count + 1;

                        }
                    }

                    if (Count > 1)
                    {
                        dr.Delete();
                        dtSearchResult.AcceptChanges();
                    }
                }
            }

            //Calculate Day of 2Week
            foreach (DataRow row in dtSearchResult.Rows)
            {
                DateTime d1 = Convert.ToDateTime(row[4].ToString());
                DateTime d2 = Convert.ToDateTime(row[5].ToString());
                TimeSpan Diff_dates = d1.Subtract(d2);
                row[9]= Diff_dates.Days.ToString();
                dtSearchResult.AcceptChanges();
            }


            //Select UP from OBS
            try
            {
                cnnOBS.conOBS.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT TBSaleP.*, CONCAT(TBMasterItem.DefaultSupplier,'') AS DefaultSupplier FROM " +
                    "(SELECT ItemCode, DefaultSupplier FROM mstitem WHERE ItemType=0 AND DelFlag=0) TBMasterItem " +
                    "INNER JOIN " +
                    "(SELECT T1.ItemCode, T2.UnitPrice FROM " +
                    "  (SELECT ItemCode, MAX(EffDate) AS EffDate FROM mstsalesprice WHERE DelFlag=0 GROUP BY ItemCode) T1 " +
                    "  INNER JOIN " +
                    "  (SELECT * FROM mstsalesprice) T2 " +
                    "  ON T1.ItemCode=T2.ItemCode AND T1.EffDate=T2.EffDate) TBSaleP " +
                    "ON TBSaleP.ItemCode=TBMasterItem.ItemCode", cnnOBS.conOBS);
                sda.Fill(dtUPFromOBS);
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnnOBS.conOBS.Close();

            //Assign UP and Cal Amount and Line
            foreach (DataRow row in dtSearchResult.Rows)
            {
                int Found = 0;
                foreach (DataRow rowUP in dtUPFromOBS.Rows)
                {
                    if (row[0].ToString() == rowUP[0].ToString())
                    {
                        row[10] = Math.Round(Convert.ToDouble(rowUP[1].ToString()),4).ToString();
                        row[11] = Math.Round(Convert.ToDouble(row[3]) * Math.Round(Convert.ToDouble(rowUP[1]),4),2).ToString();
                        if (rowUP[2].ToString().Trim() != "")
                        {
                            row[12] = "S3N";
                        }
                        else
                        {
                            row[12] = "Assy";
                        }
                        Found++;
                        break;
                    }
                }
                if (Found == 0)
                {
                    row[10] = 0;
                    row[11] = 0;
                }
                dtSearchResult.AcceptChanges();

            }
                        
            if (CboStatus.Text.Trim() != "ALL")
            {
                //Add to dgvSearchResult
                foreach (DataRow row in dtSearchResult.Rows)
                {
                    string Code = row[0].ToString();
                    string Items = row[1].ToString();
                    string POS_NO = row[2].ToString();
                    string QTY = row[3].ToString();
                    string SHIP_ORIGINE = row[4].ToString();
                    string KIT_DATE = row[5].ToString();
                    string DOC_NO = row[6].ToString();
                    string RECEIVED_DATE = row[7].ToString();
                    string STATUS = row[8].ToString();
                    string TwoWEEK = row[9].ToString();
                    string UP = "0";
                    string AMOUNT = "0";
                    if (row[10].ToString().Trim() != "")
                    {
                        UP = row[10].ToString();
                        AMOUNT = row[11].ToString();
                    }
                    string LINE = row[12].ToString();
                    string CUSTOMER = row[13].ToString();
                    string MODEL = row[14].ToString();

                    if (STATUS == CboStatus.Text.ToString())
                    {
                        dgvSearchResult.Rows.Add(Code, Items, POS_NO, Convert.ToDouble(QTY), Convert.ToDateTime(SHIP_ORIGINE), Convert.ToDateTime(KIT_DATE), DOC_NO, Convert.ToDateTime(RECEIVED_DATE), STATUS, Convert.ToDouble(TwoWEEK), Convert.ToDouble(UP), Convert.ToDouble(AMOUNT), LINE, CUSTOMER, MODEL);
                    }
                }
            }
            else
            {
                //Add to dgvSearchResult
                foreach (DataRow row in dtSearchResult.Rows)
                {
                    string Code = row[0].ToString();
                    string Items = row[1].ToString();
                    string POS_NO = row[2].ToString();
                    string QTY = row[3].ToString();
                    string SHIP_ORIGINE = row[4].ToString();
                    string KIT_DATE = row[5].ToString();
                    string DOC_NO = row[6].ToString();
                    string RECEIVED_DATE = row[7].ToString();
                    string STATUS = row[8].ToString();
                    string TwoWEEK = row[9].ToString();
                    string UP = "0";
                    string AMOUNT = "0";
                    if (row[10].ToString().Trim() != "")
                    {
                        UP = row[10].ToString();
                        AMOUNT = row[11].ToString();
                    }
                    string LINE = row[12].ToString();
                    string CUSTOMER = row[13].ToString();
                    string MODEL = row[14].ToString();

                    dgvSearchResult.Rows.Add(Code, Items, POS_NO, Convert.ToDouble(QTY), Convert.ToDateTime(SHIP_ORIGINE), Convert.ToDateTime(KIT_DATE), DOC_NO, Convert.ToDateTime(RECEIVED_DATE), STATUS, Convert.ToDouble(TwoWEEK), Convert.ToDouble(UP), Convert.ToDouble(AMOUNT), LINE, CUSTOMER, MODEL);
                }
            }
            dgvSearchResult.ClearSelection();

            CalcAmount();



            Cursor = Cursors.Default;
            LbStatus.Text = "Found "+dgvSearchResult.Rows.Count.ToString("N0");
            LbStatus.Refresh();

        }

        private void CalcAmount()
        {
            DataTable dtSearchResult2 = new DataTable();
            dtSearchResult2.Columns.Add("KITDate");
            dtSearchResult2.Columns.Add("DocNo");
            dtSearchResult2.Columns.Add("ShipDate");
            dtSearchResult2.Columns.Add("TwoWeek");
            dtSearchResult2.Columns.Add("Amount");
            dtSearchResult2.Columns.Add("INProcessAmount");
            dtSearchResult2.Columns.Add("INProcessPercent");
            dtSearchResult2.Columns.Add("FinishAmount");
            dtSearchResult2.Columns.Add("FinishPercent");

            foreach (DataGridViewRow DgvRow in dgvSearchResult.Rows)
            {
                string KITDate = DgvRow.Cells[5].Value.ToString();
                string DocNo = DgvRow.Cells[6].Value.ToString();
                string ShipDate = DgvRow.Cells[4].Value.ToString();
                string TwoWeek = DgvRow.Cells[9].Value.ToString();
                double Amount = 0; 
                double INProcessAmount = 0;
                double FINISHAmount = 0;
                int Found = 0;

                //Find alread add or not
                foreach (DataRow row in dtSearchResult2.Rows)
                {
                    if (Convert.ToDateTime(row[0].ToString()) == Convert.ToDateTime(KITDate) && row[1].ToString() == DocNo && Convert.ToDateTime(row[2].ToString()) == Convert.ToDateTime(ShipDate))
                    {
                        Found = Found + 1;
                        break;
                    }
                }
                
                //If not yet >> add
                if (Found == 0)
                {
                    foreach (DataGridViewRow DgvRowSum in dgvSearchResult.Rows)
                    {
                        if (Convert.ToDateTime(KITDate) == Convert.ToDateTime(DgvRowSum.Cells[5].Value.ToString()) && DocNo == DgvRowSum.Cells[6].Value.ToString() && Convert.ToDateTime(ShipDate) == Convert.ToDateTime(DgvRowSum.Cells[4].Value.ToString()))
                        {
                            Amount = Amount + Convert.ToDouble(DgvRowSum.Cells[11].Value.ToString());
                            if (DgvRowSum.Cells[8].Value.ToString() == "FINISH")
                            {
                                FINISHAmount = FINISHAmount + Convert.ToDouble(DgvRowSum.Cells[11].Value.ToString());
                            }
                            else
                            {
                                INProcessAmount = INProcessAmount + Convert.ToDouble(DgvRowSum.Cells[11].Value.ToString());
                            }
                        }
                    }
                    dtSearchResult2.Rows.Add(KITDate, DocNo, ShipDate, TwoWeek, Amount, INProcessAmount, "", FINISHAmount, "");
                }
            }

            double TotalAmount = 0;
            double InprocessAmount = 0;
            double FinishAmount = 0;

            foreach (DataRow row in dtSearchResult2.Rows)
            {
                string KITDate = row[0].ToString();
                string DocNo = row[1].ToString();
                string ShipDate = row[2].ToString();
                string TwoWeek = row[3].ToString();
                double Amount = Convert.ToDouble(row[4].ToString());
                double INProcessAmount = Convert.ToDouble(row[5].ToString());
                double INProcessPercent = 0;
                if (Amount > 0)
                {
                    INProcessPercent = INProcessAmount / Amount;
                }
                double FINISHAmount = Convert.ToDouble(row[7].ToString());
                double FINISHPercent = 0;
                if (Amount > 0)
                {
                    FINISHPercent = FINISHAmount / Amount;
                }

                Console.WriteLine(DocNo +" : "+ FinishAmount.ToString()+"/"+Amount.ToString());

                dgvSearchResult2.Rows.Add(Convert.ToDateTime(KITDate), DocNo, Convert.ToDateTime(ShipDate), Convert.ToDouble(TwoWeek), Amount, INProcessAmount, INProcessPercent, FINISHAmount, FINISHPercent);

                TotalAmount = TotalAmount + Amount;
                InprocessAmount = InprocessAmount + INProcessAmount;
                FinishAmount = FinishAmount + FINISHAmount;
            }
            
            if (dgvSearchResult2.Rows.Count > 0)
            {
                dgvSearchResult2.Rows.Add(null, "", null, null, TotalAmount, InprocessAmount, null, FinishAmount, null);
                dgvSearchResult2.Rows[dgvSearchResult2.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Red;
                dgvSearchResult2.Rows[dgvSearchResult2.Rows.Count - 1].DefaultCellStyle.Font = new Font("Khmer OS Battambang", 9, FontStyle.Bold);

            }
            dgvSearchResult2.ClearSelection();
        }
    }
}
