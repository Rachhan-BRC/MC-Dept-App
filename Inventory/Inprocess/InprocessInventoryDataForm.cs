using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.Inventory.Inprocess
{
    public partial class InprocessInventoryDataForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        DataTable dtSubLoc;
        DataTable dtFunction;
        DataTable dtItemType;

        string ErrorText;

        public InprocessInventoryDataForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += InprocessInventoryDataForm_Load;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnExport.Click += BtnExport_Click;
            this.btnPrint.Click += BtnPrint_Click;
            this.dgvSearchResult.CellFormatting += DgvSearchResult_CellFormatting;
            this.dgvSearchResult.SelectionChanged += DgvSearchResult_SelectionChanged;

        }

        private void DgvSearchResult_SelectionChanged(object sender, EventArgs e)
        {
            CheckBtnPrint();
        }

        private void DgvSearchResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvSearchResult.Rows[e.RowIndex].Cells[12].Value.ToString() == "Deleted")
            {
                dgvSearchResult.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Pink;
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (dgvSearchResult.SelectedCells.Count > 0)
            {
                if (dgvSearchResult.CurrentCell.RowIndex > -1)
                {
                    DialogResult DSL = MessageBox.Show("តើអ្នកចង់ព្រីនឡាប៊ែលនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DSL == DialogResult.Yes)
                    {
                        ErrorText = "";
                        DataTable dtInventory = new DataTable();
                        DataTable dtOBSItems = new DataTable();
                        DataTable dtOBSHeader = new DataTable();

                        //Take the Inventory Data
                        try
                        {
                            cnn.con.Open();
                            SqlDataAdapter sda = new SqlDataAdapter("SELECT LabelNo, SubLoc, CountingMethod, CountingMethod2, ItemCode, Qty, QtyDetails, QtyDetails2, BobbinsOrReil FROM " +
                                "(SELECT * FROM tbInventory WHERE LocCode='MC1') T1 " +
                                "LEFT JOIN (SELECT * FROM tbSDMstUncountMat) T2 " +
                                "ON T1.ItemCode=T2.Code " +
                                "WHERE LabelNo = " + dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[1].Value.ToString(), cnn.con);
                            sda.Fill(dtInventory);
                        }
                        catch (Exception ex)
                        {
                            if (ErrorText.Trim() == "")
                            {
                                ErrorText = ex.Message;
                            }
                            else
                            {
                                ErrorText = ErrorText + "\n" + ex.Message;
                            }
                        }
                        cnn.con.Close();

                        if (ErrorText.Trim() == "")
                        {
                            if (dtInventory.Rows.Count > 0)
                            {
                                //If Semi
                                if (dtInventory.Rows[0]["CountingMethod"].ToString() == "Semi")
                                {
                                    string ItemCode = dtInventory.Rows[0]["ItemCode"].ToString();
                                    //Take OBS 
                                    try
                                    {
                                        cnnOBS.conOBS.Open();
                                        SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM mstitem WHERE ItemType=1 AND DelFlag=0 AND ItemCode='" + ItemCode + "' ", cnnOBS.conOBS);
                                        sda.Fill(dtOBSItems);

                                    }
                                    catch (Exception ex)
                                    {
                                        if (ErrorText.Trim() == "")
                                        {
                                            ErrorText = ex.Message;
                                        }
                                        else
                                        {
                                            ErrorText = ErrorText + "\n" + ex.Message;
                                        }
                                    }
                                    cnnOBS.conOBS.Close();

                                    if (ErrorText.Trim() == "")
                                    {
                                        try
                                        {
                                            //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                                            string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\InventoryLable\Inprocess";
                                            if (!Directory.Exists(SavePath))
                                            {
                                                Directory.CreateDirectory(SavePath);
                                            }

                                            //open excel application and create new workbook
                                            var CDirectory = Environment.CurrentDirectory;
                                            Excel.Application excelApp = new Excel.Application();
                                            Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\InventoryLabel_Inprocess_Template.xlsx", Editable: true);
                                            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Semi"];

                                            worksheet.Cells[2, 6] = "Inprocess(" + dtInventory.Rows[0]["SubLoc"].ToString() + ")";
                                            worksheet.Cells[2, 4] = dtInventory.Rows[0]["LabelNo"].ToString();
                                            worksheet.Cells[3, 1] = "*" + dtInventory.Rows[0]["LabelNo"].ToString() + "*";
                                            worksheet.Cells[7, 4] = dtInventory.Rows[0]["ItemCode"].ToString();
                                            worksheet.Cells[9, 4] = dtOBSItems.Rows[0]["ItemName"].ToString();
                                            worksheet.Cells[11, 4] = dtOBSItems.Rows[0]["Remark2"].ToString();
                                            worksheet.Cells[13, 4] = dtOBSItems.Rows[0]["Remark3"].ToString();
                                            worksheet.Cells[15, 4] = dtOBSItems.Rows[0]["Remark4"].ToString();

                                            worksheet.Cells[17, 4] = dtInventory.Rows[0]["Qty"].ToString();
                                            //Summary
                                            worksheet.Cells[17, 6] = "( " + dtInventory.Rows[0]["QtyDetails"].ToString() + " )";

                                            // Saving the modified Excel file
                                            var CDirectory2 = Environment.CurrentDirectory;
                                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                            string file = dtInventory.Rows[0]["LabelNo"].ToString();
                                            string fName = file + "-Reprint( " + date + " )";

                                            //លុប Manual worksheet
                                            excelApp.DisplayAlerts = false;
                                            Excel.Worksheet wsDelete1 = (Excel.Worksheet)xlWorkBook.Sheets["Weight"];
                                            wsDelete1.Delete();
                                            Excel.Worksheet wsDelete2 = (Excel.Worksheet)xlWorkBook.Sheets["POS"];
                                            wsDelete2.Delete();
                                            Excel.Worksheet wsDelete3 = (Excel.Worksheet)xlWorkBook.Sheets["Box"];
                                            wsDelete3.Delete();
                                            excelApp.DisplayAlerts = true;

                                            worksheet.Name = "RachhanSystem";
                                            worksheet.SaveAs(SavePath + @"\" + fName + ".xlsx");
                                            worksheet.PrintOut();
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
                                            if (ErrorText.Trim() == "")
                                            {
                                                ErrorText = "Print Label : " + ex.Message;
                                            }
                                            else
                                            {
                                                ErrorText = ErrorText + "\nPrint Label : " + ex.Message;
                                            }
                                            //Kill all Excel background process
                                            var processes = from p in Process.GetProcessesByName("EXCEL")
                                                            select p;
                                            foreach (var process in processes)
                                            {
                                                if (process.MainWindowTitle.ToString().Trim() == "")
                                                    process.Kill();
                                            }
                                        }

                                        if (ErrorText.Trim() != "")
                                        {
                                            MessageBox.Show(ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        }
                                    }
                                }
                                //POS-Connector & WireTerminal
                                else
                                {
                                    //POS-Connector
                                    if (dtInventory.Rows[0]["CountingMethod"].ToString() == "POS")
                                    {
                                        //Take OBS
                                        try
                                        {
                                            string CodeIN = "";
                                            foreach (DataRow row in dtInventory.Rows)
                                            {
                                                if (CodeIN.Trim() == "")
                                                {
                                                    CodeIN = " '" + row["ItemCode"].ToString() + "' ";
                                                }
                                                else
                                                {
                                                    CodeIN = CodeIN + ", '" + row["ItemCode"].ToString() + "' ";
                                                }
                                            }
                                            cnnOBS.conOBS.Open();
                                            SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, MatTypeName, Resv1 FROM " +
                                                "(SELECT * FROM mstitem WHERE ItemType = 2 AND DelFlag=0) T1 " +
                                                "INNER JOIN " +
                                                "(SELECT * FROM MstMatType WHERE DelFlag=0) T2 " +
                                                "ON T1.MatTypeCode=T2.MatTypeCode " +
                                                "WHERE ItemCode IN (" + CodeIN + ")", cnnOBS.conOBS);
                                            sda.Fill(dtOBSItems);

                                            SqlDataAdapter sda1 = new SqlDataAdapter("SELECT DONo, PlanDate, T1.ItemCode, ItemName, PlanQty, POSDeliveryDate  FROM " +
                                                "(SELECT * FROM prgproductionorder) T1 " +
                                                "LEFT JOIN " +
                                                "(SELECT * FROM mstitem WHERE DelFlag=0 AND ItemType=1) T2 " +
                                                "ON T1.ItemCode=T2.ItemCode " +
                                                "WHERE DONo='" + dtInventory.Rows[0]["QtyDetails"].ToString() + "'", cnnOBS.conOBS);
                                            sda1.Fill(dtOBSHeader);

                                        }
                                        catch (Exception ex)
                                        {
                                            if (ErrorText.Trim() == "")
                                            {
                                                ErrorText = ex.Message;
                                            }
                                            else
                                            {
                                                ErrorText = ErrorText + "\n" + ex.Message;
                                            }
                                        }
                                        cnnOBS.conOBS.Close();

                                        if (ErrorText.Trim() == "")
                                        {
                                            //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                                            string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\InventoryLable\Inprocess";
                                            if (!Directory.Exists(SavePath))
                                            {
                                                Directory.CreateDirectory(SavePath);
                                            }

                                            //សរសេរចូល Excel
                                            var CDirectory = Environment.CurrentDirectory;
                                            Excel.Application excelApp = new Excel.Application();
                                            Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\InventoryLabel_Inprocess_Template.xlsx", Editable: true);
                                            Excel.Worksheet wsBarcode = (Excel.Worksheet)xlWorkBook.Sheets["POS"];
                                            //ក្បាលលើ
                                            wsBarcode.Cells[2, 3] = dtInventory.Rows[0]["LabelNo"].ToString();
                                            wsBarcode.Cells[3, 1] = "*" + dtInventory.Rows[0]["LabelNo"].ToString() + "*";
                                            wsBarcode.Cells[2, 4] = "Inprocess(" + dtInventory.Rows[0]["SubLoc"].ToString() + ")";
                                            wsBarcode.Cells[4, 3] = dtInventory.Rows[0]["QtyDetails"].ToString();
                                            wsBarcode.Cells[5, 3] = dtOBSHeader.Rows[0]["ItemName"].ToString();
                                            wsBarcode.Cells[6, 3] = dtOBSHeader.Rows[0]["PlanQty"].ToString();

                                            // Material List
                                            for (int i = 0; i < dtInventory.Rows.Count; i++)
                                            {
                                                string RMName = "";
                                                foreach (DataRow row in dtOBSItems.Rows)
                                                {
                                                    if (dtInventory.Rows[i]["ItemCode"].ToString() == row["ItemCode"].ToString())
                                                    {
                                                        RMName = row["ItemName"].ToString();
                                                        break;
                                                    }
                                                }
                                                wsBarcode.Cells[i + 10, 1] = dtInventory.Rows[i]["ItemCode"].ToString();
                                                wsBarcode.Cells[i + 10, 2] = RMName;
                                                wsBarcode.Cells[i + 10, 5] = dtInventory.Rows[i]["Qty"].ToString();

                                            }

                                            //លុប Manual worksheet
                                            excelApp.DisplayAlerts = false;
                                            Excel.Worksheet wsDelete = (Excel.Worksheet)xlWorkBook.Sheets["Box"];
                                            wsDelete.Delete();
                                            Excel.Worksheet wsDelete1 = (Excel.Worksheet)xlWorkBook.Sheets["Weight"];
                                            wsDelete1.Delete();
                                            Excel.Worksheet wsDelete2 = (Excel.Worksheet)xlWorkBook.Sheets["Semi"];
                                            wsDelete2.Delete();
                                            excelApp.DisplayAlerts = true;

                                            //ព្រីនចេញ
                                            wsBarcode.PrintOut();

                                            //Save Excel ទុក
                                            string fName = "";
                                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                            string file = dtInventory.Rows[0]["LabelNo"].ToString();
                                            wsBarcode.Name = "RachhanSystem";
                                            fName = file + "-Reprint( " + date + " )";
                                            wsBarcode.SaveAs(SavePath + @"\" + fName + ".xlsx");
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
                                    }
                                    //WireTerminal
                                    else
                                    {
                                        //Take OBS
                                        try
                                        {
                                            cnnOBS.conOBS.Open();
                                            SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, MatTypeName, Resv1 FROM " +
                                                "(SELECT * FROM mstitem WHERE ItemType = 2 AND DelFlag=0) T1 " +
                                                "INNER JOIN " +
                                                "(SELECT * FROM MstMatType WHERE DelFlag=0) T2 " +
                                                "ON T1.MatTypeCode=T2.MatTypeCode " +
                                                "WHERE ItemCode = '" + dtInventory.Rows[0]["ItemCode"].ToString() + "'", cnnOBS.conOBS);
                                            sda.Fill(dtOBSItems);

                                        }
                                        catch (Exception ex)
                                        {
                                            if (ErrorText.Trim() == "")
                                            {
                                                ErrorText = ex.Message;
                                            }
                                            else
                                            {
                                                ErrorText = ErrorText + "\n" + ex.Message;
                                            }
                                        }
                                        cnnOBS.conOBS.Close();

                                        if (ErrorText.Trim() == "")
                                        {
                                            try
                                            {
                                                //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                                                string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\InventoryLable\Inprocess";
                                                if (!Directory.Exists(SavePath))
                                                {
                                                    Directory.CreateDirectory(SavePath);
                                                }

                                                //open excel application and create new workbook
                                                var CDirectory = Environment.CurrentDirectory;
                                                Excel.Application excelApp = new Excel.Application();
                                                Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\InventoryLabel_Inprocess_Template.xlsx", Editable: true);

                                                //Weight
                                                if (dtInventory.Rows[0]["CountingMethod2"].ToString() == "Weight")
                                                {
                                                    Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Weight"];

                                                    worksheet.Cells[2, 6] = "Inprocess(" + dtInventory.Rows[0]["SubLoc"].ToString() + ")";
                                                    worksheet.Cells[2, 4] = dtInventory.Rows[0]["LabelNo"].ToString();
                                                    worksheet.Cells[3, 1] = "*" + dtInventory.Rows[0]["LabelNo"].ToString() + "*";
                                                    worksheet.Cells[7, 4] = dtInventory.Rows[0]["ItemCode"].ToString();
                                                    worksheet.Cells[9, 4] = dtOBSItems.Rows[0]["ItemName"].ToString();
                                                    worksheet.Cells[11, 4] = dtOBSItems.Rows[0]["MatTypeName"].ToString();
                                                    worksheet.Cells[13, 4] = dtOBSItems.Rows[0]["Resv1"].ToString();
                                                    worksheet.Cells[15, 4] = dtInventory.Rows[0]["Qty"].ToString();
                                                    //Summary
                                                    string BobbinsQty = "";
                                                    string[] CalcBobbinsQty = dtInventory.Rows[0]["QtyDetails2"].ToString().Split('|');
                                                    if (Convert.ToDouble(CalcBobbinsQty[1].ToString()) == 1)
                                                    {
                                                        if (dtInventory.Rows[0]["BobbinsOrReil"].ToString() == "Bobbins")
                                                        {
                                                            BobbinsQty = CalcBobbinsQty[1].ToString() + " Bobbin";
                                                        }
                                                        else
                                                        {
                                                            BobbinsQty = CalcBobbinsQty[1].ToString() + " Reel";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dtInventory.Rows[0]["BobbinsOrReil"].ToString() == "Bobbins")
                                                        {
                                                            BobbinsQty = CalcBobbinsQty[1].ToString() + " Bobbins";
                                                        }
                                                        else
                                                        {
                                                            BobbinsQty = CalcBobbinsQty[1].ToString() + " Reels";
                                                        }
                                                    }

                                                    worksheet.Cells[15, 6] = "( " + dtInventory.Rows[0]["QtyDetails"].ToString() + " )\n" + BobbinsQty;

                                                    // Saving the modified Excel file
                                                    var CDirectory2 = Environment.CurrentDirectory;
                                                    string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                                    string file = dtInventory.Rows[0]["SubLoc"].ToString();
                                                    string fName = file + "-Reprint( " + date + " )";

                                                    //លុប Manual worksheet
                                                    excelApp.DisplayAlerts = false;
                                                    Excel.Worksheet wsDelete = (Excel.Worksheet)xlWorkBook.Sheets["POS"];
                                                    wsDelete.Delete();
                                                    Excel.Worksheet wsDelete1 = (Excel.Worksheet)xlWorkBook.Sheets["Box"];
                                                    wsDelete1.Delete();
                                                    Excel.Worksheet wsDelete2 = (Excel.Worksheet)xlWorkBook.Sheets["Semi"];
                                                    wsDelete2.Delete();
                                                    excelApp.DisplayAlerts = true;

                                                    worksheet.Name = "RachhanSystem";
                                                    worksheet.SaveAs(SavePath + @"\" + fName + ".xlsx");
                                                    worksheet.PrintOut();
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
                                                //Box
                                                else
                                                {
                                                    Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Box"];
                                                    worksheet.Cells[2, 6] = "Inprocess(" + dtInventory.Rows[0]["SubLoc"].ToString() + ")";
                                                    worksheet.Cells[2, 4] = dtInventory.Rows[0]["LabelNo"].ToString();
                                                    worksheet.Cells[3, 1] = "*" + dtInventory.Rows[0]["LabelNo"].ToString() + "*";
                                                    worksheet.Cells[7, 4] = dtInventory.Rows[0]["ItemCode"].ToString();
                                                    worksheet.Cells[9, 4] = dtOBSItems.Rows[0]["ItemName"].ToString();
                                                    worksheet.Cells[11, 4] = dtOBSItems.Rows[0]["MatTypeName"].ToString();
                                                    worksheet.Cells[13, 4] = dtOBSItems.Rows[0]["Resv1"].ToString();
                                                    worksheet.Cells[15, 4] = dtInventory.Rows[0]["Qty"].ToString();
                                                    //Summary
                                                    string BobbinsQty = "";
                                                    if (Convert.ToDouble(dtInventory.Rows[0]["QtyDetails2"].ToString()) == 1)
                                                    {
                                                        if (dtInventory.Rows[0]["BobbinsOrReil"].ToString() == "Bobbins")
                                                        {
                                                            BobbinsQty = dtInventory.Rows[0]["QtyDetails2"].ToString() + " Bobbin";
                                                        }
                                                        else
                                                        {
                                                            BobbinsQty = dtInventory.Rows[0]["QtyDetails2"].ToString() + " Reel";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dtInventory.Rows[0]["BobbinsOrReil"].ToString() == "Bobbins")
                                                        {
                                                            BobbinsQty = dtInventory.Rows[0]["QtyDetails2"].ToString() + " Bobbins";
                                                        }
                                                        else
                                                        {
                                                            BobbinsQty = dtInventory.Rows[0]["QtyDetails2"].ToString() + " Reels";
                                                        }
                                                    }

                                                    worksheet.Cells[15, 6] = "( " + dtInventory.Rows[0]["QtyDetails"].ToString() + " )\n" + BobbinsQty;

                                                    // Saving the modified Excel file
                                                    var CDirectory2 = Environment.CurrentDirectory;
                                                    string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                                    string file = dtInventory.Rows[0]["LabelNo"].ToString();
                                                    string fName = file + "-Reprint( " + date + " )";

                                                    //លុប Manual worksheet
                                                    excelApp.DisplayAlerts = false;
                                                    Excel.Worksheet wsDelete = (Excel.Worksheet)xlWorkBook.Sheets["POS"];
                                                    wsDelete.Delete();
                                                    Excel.Worksheet wsDelete1 = (Excel.Worksheet)xlWorkBook.Sheets["Weight"];
                                                    wsDelete1.Delete();
                                                    Excel.Worksheet wsDelete2 = (Excel.Worksheet)xlWorkBook.Sheets["Semi"];
                                                    wsDelete2.Delete();
                                                    excelApp.DisplayAlerts = true;

                                                    worksheet.Name = "RachhanSystem";
                                                    worksheet.SaveAs(SavePath + @"\" + fName + ".xlsx");
                                                    worksheet.PrintOut();
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

                                            }
                                            catch (Exception ex)
                                            {
                                                if (ErrorText.Trim() == "")
                                                {
                                                    ErrorText = "Print Label : " + ex.Message;
                                                }
                                                else
                                                {
                                                    ErrorText = ErrorText + "\nPrint Label : " + ex.Message;
                                                }
                                                //Kill all Excel background process
                                                var processes = from p in Process.GetProcessesByName("EXCEL")
                                                                select p;
                                                foreach (var process in processes)
                                                {
                                                    if (process.MainWindowTitle.ToString().Trim() == "")
                                                        process.Kill();
                                                }
                                            }
                                            if (ErrorText.Trim() != "")
                                            {
                                                MessageBox.Show(ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show(ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvSearchResult.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("តើអ្នកចង់ទាញទិន្នន័យចេញមែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "CSV file (*.csv)|*.csv";
                    saveDialog.FileName = "MCInprocess_Inventory" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;
                        try
                        {
                            //Write Column name
                            int columnCount = 0;
                            foreach (DataGridViewColumn DgvCol in dgvSearchResult.Columns)
                            {
                                if (DgvCol.Visible == true)
                                {
                                    columnCount = columnCount + 1;
                                }
                            }
                            string columnNames = "";

                            //String array for Csv
                            string[] outputCsv;
                            outputCsv = new string[dgvSearchResult.Rows.Count + 1];

                            //Set Column Name
                            for (int i = 0; i < columnCount; i++)
                            {
                                if (dgvSearchResult.Columns[i].Visible == true)
                                {
                                    columnNames += dgvSearchResult.Columns[i].HeaderText.ToString() + ",";
                                }
                            }
                            outputCsv[0] += columnNames;

                            //Row of data 
                            for (int i = 1; (i - 1) < dgvSearchResult.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    if (dgvSearchResult.Columns[j].Visible == true)
                                    {
                                        string Value = "";
                                        if (dgvSearchResult.Rows[i - 1].Cells[j].Value != null)
                                        {
                                            Value = dgvSearchResult.Rows[i - 1].Cells[j].Value.ToString();
                                        }
                                        //Fix don't separate if it contain '\n' or ','
                                        Value = "\"" + Value.Replace("\"", "\"\"") + "\"";
                                        outputCsv[i] += Value + ",";
                                    }
                                }
                            }

                            File.WriteAllLines(saveDialog.FileName, outputCsv, Encoding.UTF8);
                            Cursor = Cursors.Default;
                            MessageBox.Show("ទាញទិន្នន័យចេញរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            Cursor = Cursors.Default;
                            MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            ErrorText = "";
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            dgvSearchResult.Rows.Clear();

            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Value");

            if (txtLabelNo.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("LabelNo = ", Convert.ToInt32(txtLabelNo.Text).ToString());
            }
            if (CboSubLocation.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("SubLoc = ", "'"+CboSubLocation.Text+"'");                
            }
            if (CboFunction.Text.Trim() != "")
            {
                string CountM = "";
                foreach (DataRow row in dtFunction.Rows)
                {
                    if (row["CountMetAsKhmer"].ToString() == CboFunction.Text)
                    {
                        CountM = row["CountingMethod"].ToString();
                        break;
                    }
                }
                dtSQLCond.Rows.Add("CountingMethod = ", "'"+CountM+"'");
            }
            if (CboItemType.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("ItemType = ", "'" + CboItemType.Text + "'");
            }
            if (txtCode.Text.Trim() != "")
            {
                string Code = txtCode.Text;
                if (Code.Contains("*") == true)
                {
                    Code = Code.Replace("*", "%"); 
                    dtSQLCond.Rows.Add("ItemCode LIKE ", "'" + Code + "'");
                }
                else
                {
                    dtSQLCond.Rows.Add("ItemCode = ", "'" + Code + "'");
                }
            }
            if (txtRemarks.Text.Trim() != "")
            {
                string Remarks = txtRemarks.Text;
                if (Remarks.Contains("*") == true)
                {
                    Remarks = Remarks.Replace("*", "%");
                    dtSQLCond.Rows.Add("QtyDetails LIKE ", "'" + Remarks + "'");
                }
                else
                {
                    dtSQLCond.Rows.Add("QtyDetails = ", "'" + Remarks + "'");
                }
            }
            if (CboStatus.Text.Trim() != "")
            {
                if (CboStatus.Text == "លុប")
                {
                    dtSQLCond.Rows.Add("CancelStatus = ", "1");
                }
                else
                {
                    dtSQLCond.Rows.Add("CancelStatus = ", "0");
                }
            }

            string SQLConds = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                SQLConds = SQLConds + " AND " + row[0].ToString() + row[1].ToString();
            }

            //Take Inventory data
            DataTable dtSearchResult = new DataTable();
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbInventory WHERE LocCode='MC1'"+SQLConds+" ORDER BY LabelNo ASC", cnn.con);
                sda.Fill(dtSearchResult);
            }
            catch(Exception ex)
            {
                if (ErrorText.Trim() == "")
                {
                    ErrorText = ex.Message;
                }
                else
                {
                    ErrorText = ErrorText + "\n" + ex.Message;
                }
            }
            cnn.con.Close();

            
            if (ErrorText.Trim() == "")
            {
                if (dtSearchResult.Rows.Count > 0)
                {
                    //Take OBS MstItem
                    DataTable dtOBSItem = new DataTable();
                    try
                    {
                        cnnOBS.conOBS.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM mstitem WHERE DelFlag=0 AND ItemType IN (1,2)", cnnOBS.conOBS);
                        sda.Fill(dtOBSItem);
                    }
                    catch (Exception ex)
                    {
                        if (ErrorText.Trim() == "")
                        {
                            ErrorText = ex.Message;
                        }
                        else
                        {
                            ErrorText = ErrorText +"\n"+ ex.Message;
                        }
                    }
                    cnnOBS.conOBS.Close();

                    //Set Function to Khmer
                    foreach (DataRow row in dtSearchResult.Rows)
                    {
                        foreach (DataRow rowFunct in dtFunction.Rows)
                        {
                            if (row["CountingMethod"].ToString() == rowFunct["CountingMethod"].ToString())
                            {
                                row["CountingMethod"] = rowFunct["CountMetAsKhmer"].ToString();
                                break;
                            }
                        }
                        dtSearchResult.AcceptChanges();
                    }

                    //Add ItemName
                    dtSearchResult.Columns.Add("Name");
                    foreach (DataRow row in dtSearchResult.Rows)
                    {
                        foreach (DataRow rowOBS in dtOBSItem.Rows)
                        {
                            if (row["ItemCode"].ToString() == rowOBS["ItemCode"].ToString())
                            {
                                row["Name"] = rowOBS["ItemName"].ToString();
                                break;
                            }
                        }
                        dtSearchResult.AcceptChanges();
                    }

                    //Showing in Dgv
                    foreach (DataRow row in dtSearchResult.Rows)
                    {
                        string SubLocations = row["SubLoc"].ToString();
                        string LabelNo = row["LabelNo"].ToString();
                        string Func = row["CountingMethod"].ToString();
                        string Code = row["ItemCode"].ToString();
                        string Name = row["Name"].ToString();
                        string Type = row["ItemType"].ToString();
                        double Qty = Convert.ToDouble(row["Qty"].ToString());
                        string Remark = row["QtyDetails"].ToString();
                        DateTime RegD = Convert.ToDateTime(row["RegDate"].ToString());
                        string RegB = row["RegBy"].ToString();
                        DateTime UpD = Convert.ToDateTime(row["UpdateDate"].ToString());
                        string UpB = row["UpdateBy"].ToString();
                        string Status = "Active";
                        if (row["CancelStatus"].ToString() == "1")
                        {
                            Status = "Deleted";
                        }

                        if (txtName.Text.Trim() != "")
                        {
                            if (Name.ToLower().Contains(txtName.Text.ToString().ToLower()) == true)
                            {
                                dgvSearchResult.Rows.Add(SubLocations, LabelNo, Func, Code, Name, Type, Qty, Remark, RegD, RegB, UpD, UpB, Status);
                            }
                        }
                        else
                        {
                            dgvSearchResult.Rows.Add(SubLocations, LabelNo, Func, Code, Name, Type, Qty, Remark, RegD, RegB, UpD, UpB, Status);
                        }
                    }
                    dgvSearchResult.ClearSelection();
                    CheckBtnPrint();
                    Cursor = Cursors.Default;

                }
                LbStatus.Text = "រកឃើញទិន្នន័យ " + dgvSearchResult.Rows.Count;
                LbStatus.Refresh();
            }
            else
            {
                Cursor = Cursors.Default;
                MessageBox.Show("មានបញ្ហា!\n"+ErrorText,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void InprocessInventoryDataForm_Load(object sender, EventArgs e)
        {
            //SubLoc
            dtSubLoc = new DataTable();
            //Function
            dtFunction = new DataTable();
            //ItemType
            dtItemType = new DataTable();

            ErrorText = "";

            try
            {
                cnn.con.Open();
                SqlDataAdapter sdaSubL = new SqlDataAdapter("SELECT SubLoc FROM tbInventory WHERE LocCode = 'MC1' GROUP BY SubLoc ORDER BY SubLoc ASC", cnn.con);  
                sdaSubL.Fill(dtSubLoc);

                SqlDataAdapter sdaFunct = new SqlDataAdapter("SELECT CountingMethod, " +
                                            "CASE " +
                                            "   WHEN CountingMethod ='POS' THEN N'ខនិកទ័រ' " +
                                            "   WHEN CountingMethod ='Semi' THEN N'សឺមី' " +
                                            "   ELSE N'ខ្សែភ្លើង/ធើមីណល' " +
                                            "END AS CountMetAsKhmer FROM " +
                                            "(SELECT CountingMethod FROM tbInventory WHERE LocCode = 'MC1' GROUP BY CountingMethod) T1 " +
                                            "ORDER BY CountingMethod ASC", cnn.con);
                sdaFunct.Fill(dtFunction);

                SqlDataAdapter sdaType = new SqlDataAdapter("SELECT ItemType FROM tbInventory WHERE LocCode = 'MC1' GROUP BY ItemType  ORDER BY ItemType ASC", cnn.con);
                sdaType.Fill(dtItemType);

            }
            catch (Exception ex)
            {
                if (ErrorText.Trim() == "")
                {
                    ErrorText = ex.Message;
                }
                else
                {
                    ErrorText = ErrorText +"\n"+ ex.Message; 
                }
            }
            cnn.con.Close();

            if (ErrorText.Trim() == "")
            {
                string[] Status = new string[] { "", "មិនទាន់លុប", "លុប" };
                for (int i = 0; i < Status.Length; i++)
                {
                    CboStatus.Items.Add(Status[i].ToString());
                }
                CboStatus.SelectedIndex = 0;

                CboSubLocation.Items.Add("");
                foreach (DataRow row in dtSubLoc.Rows)
                {
                    CboSubLocation.Items.Add(row[0].ToString());
                }
                CboFunction.Items.Add("");
                foreach (DataRow row in dtFunction.Rows)
                {
                    CboFunction.Items.Add(row["CountMetAsKhmer"].ToString());
                }
                CboItemType.Items.Add("");
                foreach (DataRow row in dtItemType.Rows)
                {
                    CboItemType.Items.Add(row[0].ToString());
                }
                
            }
            else
            {
                MessageBox.Show("មានបញ្ហា!\n"+ErrorText, "Rachhan System", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void CheckBtnPrint()
        {
            if (dgvSearchResult.SelectedCells.Count > 0)
            {
                if (dgvSearchResult.CurrentCell.RowIndex > -1)
                {
                    if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[12].Value.ToString() != "Deleted")
                    {
                        btnPrint.Enabled = true;
                        btnPrint.BackColor = Color.White;
                    }
                    else
                    {
                        btnPrint.Enabled = false;
                        btnPrint.BackColor = Color.DarkGray;
                    }
                }
                else
                {
                    btnPrint.Enabled = false;
                    btnPrint.BackColor = Color.DarkGray;
                }
            }
            else
            {
                btnPrint.Enabled = false;
                btnPrint.BackColor = Color.DarkGray;
            }
        }

    }
}
