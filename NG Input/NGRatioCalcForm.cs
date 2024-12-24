using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

namespace MachineDeptApp.NG_Input
{
    public partial class NGRatioCalcForm : Form
    {
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        DataTable dtChild;
        DataTable dtColor;
        string ReadNotFound;
        string fName;

        public NGRatioCalcForm()
        {
            InitializeComponent();
            this.cnnOBS.Connection();
            this.btnImport.Click += BtnImport_Click;
            this.Load += NGRatioCalcForm_Load;
            this.btnPrint.Click += BtnPrint_Click;
            this.btnCalculate.Click += BtnCalculate_Click;
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            dgvUncountable.Rows.Clear();
            dgvCountable.Rows.Clear();
            DataTable dtCombine = new DataTable();
            dtCombine.Columns.Add("WipCode");
            dtCombine.Columns.Add("WipName");
            dtCombine.Columns.Add("Qty");
            DialogResult DSL = MessageBox.Show("តើអ្នកចង់គណនាមែនឬទេ", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DSL == DialogResult.Yes)
            {
                //Combine dtChild
                foreach (DataRow row in dtChild.Rows)
                {
                    string POSDetails = "";
                    foreach (DataColumn col in dtChild.Columns)
                    {
                        if (POSDetails.Trim() == "")
                        {
                            POSDetails = row[col].ToString();
                        }
                        else
                        {
                            POSDetails = POSDetails + "\t" + row[col].ToString();
                        }
                    }
                    //Console.WriteLine(POSDetails);
                    int Found = 0;
                    foreach (DataRow row1 in dtCombine.Rows)
                    {
                        if (row[1].ToString() == row1[0].ToString())
                        {
                            Found = Found + 1;
                            row1[2] = Convert.ToDouble(row1[2].ToString()) + Convert.ToDouble(row[8].ToString());
                            break;
                        }
                    }
                    if (Found == 0)
                    {
                        dtCombine.Rows.Add(row[1].ToString(), row[2].ToString(), Convert.ToDouble(row[8].ToString()));
                    }
                }
                //Console.WriteLine("a jm \n\n");

                //Set ItemCode IN for SQL
                string SQLItemCodeIn = "";
                foreach (DataRow row in dtCombine.Rows)
                {
                    if (SQLItemCodeIn.Trim() == "")
                    {
                        SQLItemCodeIn = " '" + row[0].ToString() + "'";
                    }
                    else
                    {
                        SQLItemCodeIn = SQLItemCodeIn + ", '" + row[0].ToString() + "'";
                    }

                    string POSCombine = "";
                    foreach (DataColumn col in dtCombine.Columns)
                    {
                        if (POSCombine.Trim() == "")
                        {
                            POSCombine = row[col].ToString();
                        }
                        else
                        {
                            POSCombine = POSCombine + "\t" + row[col].ToString();
                        }
                    }
                    //Console.WriteLine(POSCombine);

                }
                //Console.WriteLine(SQLItemCodeIn);

                //Take MstBOM
                DataTable dtBOM = new DataTable();
                DataTable dtRMMaster = new DataTable();
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT UpItemCode, LowItemCode, LowQty FROM mstbom WHERE UpItemCode IN (" + SQLItemCodeIn + ")", cnnOBS.conOBS);
                    sda.Fill(dtBOM);
                    SqlDataAdapter sda1 = new SqlDataAdapter("SELECT ItemCode, ItemName, MatTypeName, UnitCode, MatCalcFlag, PackSize, Resv1 FROM " +
                                                                                        "(SELECT * FROM mstitem WHERE DelFlag = 0 AND ItemType=2) T1 " +
                                                                                        "INNER JOIN " +
                                                                                        "(SELECT * FROM MstMatType WHERE DelFlag=0) T2 " +
                                                                                        "ON T1.MatTypeCode=T2.MatTypeCode", cnnOBS.conOBS);
                    sda1.Fill(dtRMMaster);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();

                //Calc TotalUsed of RM
                dtBOM.Columns.Add("TotalQty");
                dtBOM.Columns.Add("TotalUsage");
                foreach (DataRow row in dtBOM.Rows)
                {
                    foreach (DataRow row1 in dtCombine.Rows)
                    {
                        if (row[0].ToString() == row1[0].ToString())
                        {
                            row[3] = Convert.ToDouble(row1[2].ToString());
                            row[4] = Convert.ToDouble(row[2].ToString()) * Convert.ToDouble(row[3].ToString());
                            break;
                        }
                    }
                    dtBOM.AcceptChanges();
                }

                foreach (DataRow row in dtBOM.Rows)
                {
                    string POSDetails = "";
                    foreach (DataColumn col in dtBOM.Columns)
                    {
                        if (POSDetails.Trim() == "")
                        {
                            POSDetails = row[col].ToString();
                        }
                        else
                        {
                            POSDetails = POSDetails + "\t" + row[col].ToString();
                        }
                    }
                    //Console.WriteLine(POSDetails);
                }

                //Combine BOM 
                DataTable dtBOMCombine = new DataTable();
                dtBOMCombine.Columns.Add("RMCode");
                dtBOMCombine.Columns.Add("TotalUsed");
                foreach (DataRow row in dtBOM.Rows)
                {
                    int Found = 0;
                    foreach (DataRow row1 in dtBOMCombine.Rows)
                    {
                        if (row[1].ToString() == row1[0].ToString())
                        {
                            Found = Found + 1;
                            row1[1] = Convert.ToDouble(row1[1].ToString()) + Convert.ToDouble(row[4].ToString());
                            break;
                        }
                    }
                    if (Found == 0)
                    {
                        dtBOMCombine.Rows.Add(row[1], Convert.ToDouble(row[4].ToString()));
                    }
                }

                foreach (DataRow row in dtBOMCombine.Rows)
                {
                    foreach (DataRow row1 in dtRMMaster.Rows)
                    {
                        if (row[0].ToString() == row1[0].ToString())
                        {
                            //Uncountable
                            if (row1[4].ToString() == "1")
                            {
                                double NGRate = 0;
                                double SPQ = Convert.ToDouble(row1[5].ToString());
                                //Wire = 0.5% = 0.005
                                if (row1[2].ToString() == "Wire")
                                {
                                    NGRate = 0.005;
                                }
                                //Terminal = 0.5% = 0.005
                                else if (row1[2].ToString() == "Terminal")
                                {
                                    NGRate = 0.005;
                                }
                                //Other = 0.1% = 0.001
                                else if (row1[2].ToString() == "Other")
                                {
                                    NGRate = 0.001;
                                }
                                dgvUncountable.Rows.Add(row[0], row1[1], row1[2], row1[6], Convert.ToDouble(row[1].ToString()), Math.Truncate(Convert.ToDouble(row[1].ToString()) * NGRate), SPQ);
                            }
                            //Countable
                            else
                            {
                                double NGRate = 0;
                                //Connector = 1% = 0.01
                                if (row1[2].ToString() == "Connector")
                                {
                                    NGRate = 0.01;
                                }
                                //Terminal = 0.5% = 0.005
                                else if (row1[2].ToString() == "Terminal")
                                {
                                    NGRate = 0.005;
                                }
                                //else if (row1[2].ToString() == "Other")
                                //{
                                //    NGRate = 0.1 / 100;
                                //}
                                dgvCountable.Rows.Add(row[0], row1[1], row1[2], row1[6], Convert.ToDouble(row[1].ToString()), Math.Truncate(Convert.ToDouble(row[1].ToString()) * NGRate));
                            }
                            dgvUncountable.Sort(dgvUncountable.Columns[0], ListSortDirection.Ascending);
                            dgvCountable.Sort(dgvCountable.Columns[0], ListSortDirection.Ascending);
                        }
                    }
                }

                btnPrint.Enabled = true;
                btnPrint.BackColor = Color.White;

            }
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            DialogResult DSL = MessageBox.Show("តើអ្នកចង់ព្រីនមែនឬទេ", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DSL == DialogResult.Yes)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    LbStatus.Text = "កំពុងព្រីនឯកសារ Excel . . .";
                    LbStatus.Refresh();
                    LbStatus.Visible = true;
                    var CDirectory = Environment.CurrentDirectory;
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\NGRate_Template.xlsx", Editable: true);

                    //Write to Countable
                    Excel.Worksheet worksheetCountable = (Excel.Worksheet)xlWorkBook.Sheets["Countable"];
                    //Header
                    worksheetCountable.Cells[4, 10] = DateTime.Now;
                    worksheetCountable.Cells[8, 3] = "NG Rate (" + dgvExcelData.Rows[0].Cells[7].Value.ToString()+")";
                    int WriteItems = 0;
                    foreach (DataGridViewRow DgvRow in dgvCountable.Rows)
                    {
                        if (Convert.ToDouble(DgvRow.Cells[5].Value.ToString()) > 0)
                        {
                            WriteItems = WriteItems + 1;
                        }
                    }                    
                    if (WriteItems > 1)
                    {
                        //Insert More Rows
                        // Define the range that you want to copy.
                        Excel.Range sourceRange = worksheetCountable.Range["A14:M14"];
                        // Perform the copy operation.
                        sourceRange.Copy(Type.Missing);
                        // Specify the range where you want to insert the copied rows.
                        Excel.Range destinationRange = worksheetCountable.Range["A15:A"+(14+WriteItems-1)];
                        // Insert the copied cells and shift the existing cells down.
                        destinationRange.Insert(Excel.XlInsertShiftDirection.xlShiftDown, sourceRange.Copy());
                        worksheetCountable.Range["15:"+(14 + WriteItems - 1)].RowHeight = 18;

                        //Write Data to cells
                        int WriteCellsIndex = 14;
                        foreach (DataGridViewRow DgvRow in dgvCountable.Rows)
                        {
                            if (Convert.ToDouble(DgvRow.Cells[5].Value.ToString()) > 0)
                            {
                                worksheetCountable.Cells[WriteCellsIndex,4] = DgvRow.Cells[0].Value.ToString();
                                worksheetCountable.Cells[WriteCellsIndex, 5] = DgvRow.Cells[1].Value.ToString();
                                worksheetCountable.Cells[WriteCellsIndex, 7] = DgvRow.Cells[3].Value.ToString();
                                worksheetCountable.Cells[WriteCellsIndex, 8] = DgvRow.Cells[2].Value.ToString();
                                worksheetCountable.Cells[WriteCellsIndex, 9] = DgvRow.Cells[5].Value.ToString();
                                worksheetCountable.Cells[WriteCellsIndex, 11] = "=I"+WriteCellsIndex+"*J" + WriteCellsIndex;

                                WriteCellsIndex = WriteCellsIndex + 1;
                            }
                        }
                        //Subtotal
                        worksheetCountable.Cells[WriteCellsIndex, 11] = "=SUM(K14:K"+(WriteCellsIndex-1)+")";

                        //
                    }
                    else
                    {
                        foreach (DataGridViewRow DgvRow in dgvCountable.Rows)
                        {
                            if (Convert.ToDouble(DgvRow.Cells[5].Value.ToString()) > 0)
                            {
                                worksheetCountable.Cells[14, 4] = DgvRow.Cells[0].Value.ToString();
                                worksheetCountable.Cells[14, 5] = DgvRow.Cells[1].Value.ToString();
                                worksheetCountable.Cells[14, 7] = DgvRow.Cells[3].Value.ToString();
                                worksheetCountable.Cells[14, 8] = DgvRow.Cells[2].Value.ToString();
                                worksheetCountable.Cells[14, 9] = DgvRow.Cells[5].Value.ToString();
                                worksheetCountable.Cells[14, 11] = "=I14*J14";

                            }
                        }
                    }

                    //Write to Uncountable
                    Excel.Worksheet worksheetUncountable = (Excel.Worksheet)xlWorkBook.Sheets["Uncountable"];
                    //Header
                    worksheetUncountable.Cells[4, 10] = DateTime.Now;
                    worksheetUncountable.Cells[8, 3] = "NG Rate (" + dgvExcelData.Rows[0].Cells[7].Value.ToString() + ")";
                    WriteItems = 0;
                    foreach (DataGridViewRow DgvRow in dgvUncountable.Rows)
                    {
                        if (Convert.ToDouble(DgvRow.Cells[5].Value.ToString()) > 0)
                        {
                            WriteItems = WriteItems + 1;
                        }
                    }
                    if (WriteItems > 1)
                    {
                        //Insert More Rows
                        // Define the range that you want to copy.
                        Excel.Range sourceRange = worksheetUncountable.Range["A14:M14"];
                        // Perform the copy operation.
                        sourceRange.Copy(Type.Missing);
                        // Specify the range where you want to insert the copied rows.
                        Excel.Range destinationRange = worksheetUncountable.Range["A15:A" + (14 + WriteItems - 1)];
                        // Insert the copied cells and shift the existing cells down.
                        destinationRange.Insert(Excel.XlInsertShiftDirection.xlShiftDown, sourceRange.Copy());
                        worksheetUncountable.Range["15:" + (14 + WriteItems - 1)].RowHeight = 18;

                        //Write Data to cells
                        int WriteCellsIndex = 14;
                        foreach (DataGridViewRow DgvRow in dgvUncountable.Rows)
                        {
                            if (Convert.ToDouble(DgvRow.Cells[5].Value.ToString()) > 0)
                            {
                                worksheetUncountable.Cells[WriteCellsIndex, 4] = DgvRow.Cells[0].Value.ToString();
                                worksheetUncountable.Cells[WriteCellsIndex, 5] = DgvRow.Cells[1].Value.ToString();
                                worksheetUncountable.Cells[WriteCellsIndex, 7] = DgvRow.Cells[3].Value.ToString();
                                worksheetUncountable.Cells[WriteCellsIndex, 8] = DgvRow.Cells[2].Value.ToString();
                                double RequestBySPQ = Convert.ToDouble(DgvRow.Cells[5].Value.ToString()); 
                                worksheetUncountable.Cells[WriteCellsIndex, 9] = RequestBySPQ;
                                worksheetUncountable.Cells[WriteCellsIndex, 11] = "=I" + WriteCellsIndex + "*J" + WriteCellsIndex;

                                WriteCellsIndex = WriteCellsIndex + 1;
                            }
                        }
                        //Subtotal
                        worksheetUncountable.Cells[WriteCellsIndex, 11] = "=SUM(K14:K" + (WriteCellsIndex - 1) + ")";

                    }
                    else
                    {
                        foreach (DataGridViewRow DgvRow in dgvUncountable.Rows)
                        {
                            if (Convert.ToDouble(DgvRow.Cells[5].Value.ToString()) > 0)
                            {
                                worksheetUncountable.Cells[14, 4] = DgvRow.Cells[0].Value.ToString();
                                worksheetUncountable.Cells[14, 5] = DgvRow.Cells[1].Value.ToString();
                                worksheetUncountable.Cells[14, 7] = DgvRow.Cells[3].Value.ToString();
                                worksheetUncountable.Cells[14, 8] = DgvRow.Cells[2].Value.ToString();
                                double RequestBySPQ = Convert.ToDouble(DgvRow.Cells[5].Value.ToString());
                                worksheetUncountable.Cells[14, 9] = RequestBySPQ;
                                worksheetUncountable.Cells[14, 11] = "=I14*J14";

                            }
                        }
                    }

                    //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                    string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\NGRate";
                    if (!Directory.Exists(SavePath))
                    {
                        Directory.CreateDirectory(SavePath);
                    }

                    // Saving the modified Excel file                        
                    string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                    string file = "NGRate " + dgvExcelData.Rows[0].Cells[7].Value.ToString();
                    fName = file + " ( " + date + " )";
                    worksheetCountable.SaveAs(CDirectory.ToString() + @"\Report\NGRate\" + fName + ".xlsx");
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
                    LbStatus.Text = "ព្រីនបានជោគជ័យ​ !";
                    MessageBox.Show("ការព្រីនបានជោគជ័យ​ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(CDirectory.ToString() + @"\\Report\NGRate\" + fName + ".xlsx");
                    fName = "";
                    dgvExcelData.Rows.Clear();
                    dgvCountable.Rows.Clear();
                    dgvUncountable.Rows.Clear();
                    btnCalculate.Enabled = false;
                    btnCalculate.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
                    btnPrint.Enabled = false;
                    btnPrint.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
                    LbStatus.Visible = false;
                }
                catch (Exception ex)
                {
                    Cursor = Cursors.Default;
                    LbStatus.Text = "ព្រីនមានបញ្ហា !";
                    MessageBox.Show(ex.Message,"Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } 
            }
        }
        private void NGRatioCalcForm_Load(object sender, EventArgs e)
        {
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

        }
        private void BtnImport_Click(object sender, EventArgs e)
        {
            ReadExcelData();
        }

        private void CheckBtnCalc()
        {
            if (dgvExcelData.Rows.Count > 0)
            {
                ReadNotFound = "";
                int NotFound = 0;

                foreach (DataGridViewRow dgvRow in dgvExcelData.Rows)
                {
                    int Check = 0;
                    foreach (DataRow row in dtChild.Rows)
                    {
                        if (dgvRow.Cells[2].Value.ToString() == row[0].ToString())
                        {
                            Check = Check + 1;
                        }
                    }
                    if (Check == 0)
                    {
                        if (ReadNotFound.Trim() == "")
                        {
                            ReadNotFound = "• " + dgvRow.Cells[2].Value.ToString();
                        }
                        else
                        {
                            ReadNotFound = ReadNotFound + "\n• " + dgvRow.Cells[2].Value.ToString();
                        }
                        NotFound = NotFound + 1;
                    }
                }

                if (NotFound == 0)
                {
                    btnCalculate.Enabled = true;
                    btnCalculate.BackColor = Color.White;
                }
                else
                {
                    btnCalculate.Enabled = false;
                    btnCalculate.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
                }
            }
            else
            {
                btnCalculate.Enabled = false;
                btnCalculate.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
            }
        }
        private void CheckOBSandKIT()
        {
            if (dgvExcelData.Rows.Count > 0)
            {
                string AllPOS = "";
                foreach (DataGridViewRow row in dgvExcelData.Rows)
                {
                    if (AllPOS.Trim() == "")
                    {
                        AllPOS = "'" + row.Cells[2].Value.ToString() + "'";
                    }
                    else
                    {
                        AllPOS = AllPOS + ", '" + row.Cells[2].Value.ToString() + "'";
                    }
                }

                try
                {
                    cnnOBS.conOBS.Open();
                    //Find Remark for POS Parent
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT DONo, Remark FROM prgproductionorder WHERE DONo  IN ( " + AllPOS + " )", cnnOBS.conOBS);
                    DataTable dtTemp = new DataTable();
                    sda.Fill(dtTemp);
                    //Add Remark to Dgv
                    foreach (DataGridViewRow DgvRow in dgvExcelData.Rows)
                    {
                        foreach (DataRow row in dtTemp.Rows)
                        {
                            if (DgvRow.Cells[2].Value.ToString() == row[0].ToString())
                            {
                                DgvRow.Cells[6].Value = row[1].ToString();
                            }
                        }

                    }

                    SqlDataAdapter sda1 = new SqlDataAdapter("SELECT T1.DONo, T2.ItemCode, T3.ItemName, T2.DONo, T3.Remark2, T3.Remark3, T3.Remark4, T2.SemiQty, PlanQty, Remark FROM " +
                                                                                        "(SELECT ProductNo, DONo FROM prgproductionorder WHERE DONo IN(" + AllPOS + ")) T1 " +
                                                                                        "INNER JOIN " +
                                                                                        "(SELECT ProductNo, ItemCode, DONo, SemiQty, PlanQty, Remark FROM prgproductionorder WHERE LineCode = 'MC1') T2 " +
                                                                                        "ON T1.ProductNo = T2.ProductNo " +
                                                                                        "INNER JOIN " +
                                                                                        "(SELECT ItemCode, ItemName, Remark2, Remark3, Remark4 FROM mstitem WHERE  ItemType='1') T3 " +
                                                                                        "ON T3.ItemCode = T2.ItemCode " +
                                                                                        "ORDER BY T1.DONo ASC, T2.DONo ASC", cnnOBS.conOBS);
                    dtChild = new DataTable();
                    sda1.Fill(dtChild);
                    dtChild.Columns.Add("KIT Promise");
                    foreach (DataRow row in dtChild.Rows)
                    {
                        foreach (DataGridViewRow DgvRow in dgvExcelData.Rows)
                        {
                            if (row[0].ToString() == DgvRow.Cells[2].Value.ToString())
                            {
                                row[10] = Convert.ToDateTime(DgvRow.Cells[5].Value.ToString()).ToString("dd-MM-yyyy");
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something wrong!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }
        }
        private void ReadExcelData()
        {
            LbStatus.Visible = false;
            LbStatus.Text = "កំពុងអានទិន្នន័យ . . .";
            dgvExcelData.Rows.Clear();
            dgvUncountable.Rows.Clear();
            btnCalculate.Enabled = false;
            btnCalculate.BackColor = Color.FromKnownColor(KnownColor.DarkGray); 
            btnPrint.Enabled = false;
            btnPrint.BackColor = Color.FromKnownColor(KnownColor.DarkGray);

            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet;
            Microsoft.Office.Interop.Excel.Range xlRange;

            int xlRow;
            string strFileName;
            OpenFileDialog openFD = new OpenFileDialog();
            openFD.Filter = "Excel|*.xls;*.xlsx;";
            openFD.ShowDialog();
            strFileName = openFD.FileName;

            if (strFileName != "")
            {
                Cursor = Cursors.WaitCursor;
                LbStatus.Visible = true;
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkbook = xlApp.Workbooks.Open(strFileName);

                try
                {
                    xlWorksheet = xlWorkbook.Worksheets[1];
                    xlRange = xlWorksheet.UsedRange;

                    int xlDataRange = 5;
                    string DocNo = "";
                    for (xlRow = 5; xlRow <= xlRange.Rows.Count; xlRow++)
                    {
                        string Code = "";
                        string Items = "";
                        string POS = "";

                        DocNo = xlRange.Cells[2, 11].Text;
                        Code = xlRange.Cells[xlRow, 2].Text;
                        Items = xlRange.Cells[xlRow, 3].Text;
                        POS = xlRange.Cells[xlRow, 4].Text;

                        if (Code.Trim() != "" && Items.Trim() != "" && POS.Trim() != "")
                        {
                            xlDataRange = xlDataRange + 1;

                        }
                        else
                        {
                            break;
                        }
                    }

                    for (xlRow = 5; xlRow < xlDataRange; xlRow++)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(dgvExcelData);
                        row.Cells[0].Value = xlRange.Cells[xlRow, 2].Text;
                        row.Cells[1].Value = xlRange.Cells[xlRow, 3].Text;
                        row.Cells[2].Value = xlRange.Cells[xlRow, 4].Text;
                        row.Cells[3].Value = Convert.ToDouble(xlRange.Cells[xlRow, 5].Text);
                        row.Cells[4].Value = Convert.ToDateTime(xlRange.Cells[xlRow, 6].Text);
                        row.Cells[5].Value = Convert.ToDateTime(xlRange.Cells[xlRow, 9].Text);
                        row.Cells[7].Value = DocNo;
                        row.HeaderCell.Style.BackColor = Color.FromKnownColor(KnownColor.GradientActiveCaption);
                        row.HeaderCell.Style.Font = new System.Drawing.Font("Khmer OS Battambang", 9, FontStyle.Regular);
                        row.HeaderCell.Value = (xlRow - 4).ToString();
                        dgvExcelData.Rows.Add(row);

                    }

                    //close excel after import to dgv success
                    xlApp.DisplayAlerts = false;
                    xlWorkbook.Close();
                    xlApp.Quit();
                    xlApp.DisplayAlerts = true;

                    //Kill all Excel background process
                    var processes = from p in Process.GetProcessesByName("EXCEL")
                                    select p;
                    foreach (var process in processes)
                    {
                        if (process.MainWindowTitle.ToString().Trim() == "")
                            process.Kill();
                    }

                    CheckOBSandKIT();
                    Cursor = Cursors.Default;
                    CheckBtnCalc();
                    if (ReadNotFound.Trim() == "")
                    {
                        MessageBox.Show("ការអានរួចរាល់ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LbStatus.Text = "ការអានរួចរាល់ !";
                    }
                    else
                    {
                        MessageBox.Show("ការអានរួចរាល់ ! ប៉ុន្តែមិនមានទិន្នន័យ POS ទាំងនេះទេ ៖ \n" + ReadNotFound, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        LbStatus.Text = "ការអានរួចរាល់ !";
                    }
                    dgvExcelData.Focus();
                    dgvExcelData.ClearSelection();
                }
                catch (Exception ex)
                {
                    LbStatus.Text = "ការអានបរាជ័យ !";
                    Cursor = Cursors.Default;
                    MessageBox.Show("Something wrong!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    xlApp.DisplayAlerts = false;
                    xlWorkbook.Close();
                    xlApp.Quit();
                    xlApp.DisplayAlerts = true;
                    //Kill all Excel background process
                    var processes = from p in Process.GetProcessesByName("EXCEL")
                                    select p;
                    foreach (var process in processes)
                    {
                        if (process.MainWindowTitle.ToString().Trim() == "")
                            process.Kill();
                    }
                }
            }
        }

    }
}
