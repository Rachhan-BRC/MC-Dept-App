using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.MCSDControl
{
    //RM (Material) that still has MC stock, with its last movement date in tbSDMCAllTransaction
    //Status = 'No Movement' / 'Slow Movement' / blank (moved recently)
    public partial class RMMovementStatusForm : Form
    {
        SQLConnect cnn = new SQLConnect();

        string ErrorText = "";

        //Search conditions of the data in the grid (printed on the Excel report)
        string LastSearchInfo = "";

        //Excel template : banner / KPI tiles / header row 11 / conditional formats are in the template.
        //Data goes to columns B:H from row 12 (No, RM Code, RM Name, RM Type, Stock Qty, Last Movement Date, Status)
        const string ExcelTemplate = @"Template\RMMovementStatus_Template.xlsx";
        const int XlFirstDataRow = 12;

        const string AllText = "ទាំងអស់";
        const string NoMovement = "No Movement";
        const string SlowMovement = "Slow Movement";

        //Main query (no ORDER BY so it can be used as a sub-query for the search conditions)
        const string MovementQuery = @"
SELECT [RM Code], [RM Name], [RM Type], [Stock Qty], [Last Movement Date],
CASE
 WHEN [Last Movement Date]<=LastOneYearDate AND [Last Movement Date]<LastSixMonthsDate  THEN 'No Movement'
 WHEN [Last Movement Date]<=LastSixMonthsDate AND [Last Movement Date]>LastOneYearDate  THEN 'Slow Movement'
 ELSE NULL
END AS Status FROM
( SELECT M.ItemCode AS [RM Code], M.ItemName AS [RM Name], M.ItemType, M.RMTypeName AS [RM Type], StockQty AS [Stock Qty], [Last Movement Date], CAST(DATEADD(MONTH, -3, GETDATE()) AS date) AS LastSixMonthsDate,
CAST(DATEADD(MONTH, -6, GETDATE()) AS date) AS LastOneYearDate FROM tbMasterItem M
LEFT JOIN (SELECT Code, SUM(StockValue) AS StockQty, CAST(MAX(RegDate) AS date) AS [Last Movement Date] FROM tbSDMCAllTransaction WHERE CancelStatus = 0 GROUP BY Code) T
ON M.ItemCode = T.Code
WHERE ItemType = 'Material' ) tb
WHERE [Stock Qty]>0";

        public RMMovementStatusForm()
        {
            InitializeComponent();
            this.cnn.Connection();

            this.Load += RMMovementStatusForm_Load;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnExport.Click += BtnExport_Click;
        }

        private void RMMovementStatusForm_Load(object sender, EventArgs e)
        {
            DgvSearch.RowHeadersDefaultCellStyle.Font = new Font(DgvSearch.AlternatingRowsDefaultCellStyle.Font, FontStyle.Regular);

            cboStatus.Items.Add(AllText);
            cboStatus.Items.Add(NoMovement);
            cboStatus.Items.Add(SlowMovement);
            cboStatus.SelectedIndex = 0;

            //RM Type list
            cboRMType.Items.Add(AllText);
            try
            {
                cnn.con.Open();
                DataTable dtType = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT DISTINCT RMTypeName FROM tbMasterItem WHERE ItemType = 'Material' AND RMTypeName IS NOT NULL AND RMTypeName <> '' ORDER BY RMTypeName", cnn.con);
                sda.Fill(dtType);
                foreach (DataRow row in dtType.Rows)
                    cboRMType.Items.Add(row["RMTypeName"].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\nTaking RM Type : \n" + ex.Message, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
            cboRMType.SelectedIndex = 0;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            DgvSearch.Rows.Clear();
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();

            //Taking Data
            DataTable dtData = new DataTable();
            try
            {
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.con;

                string SQLConds = "";
                string SearchInfo = "";
                if (txtRMCode.Text.Trim() != "")
                {
                    SQLConds += "AND [RM Code] LIKE @RMCode ";
                    cmd.Parameters.Add("@RMCode", SqlDbType.NVarChar, 200).Value = ToLikePattern(txtRMCode.Text);
                    SearchInfo += "   RM Code: " + txtRMCode.Text.Trim();
                }
                if (txtRMName.Text.Trim() != "")
                {
                    SQLConds += "AND [RM Name] LIKE @RMName ";
                    cmd.Parameters.Add("@RMName", SqlDbType.NVarChar, 500).Value = ToLikePattern(txtRMName.Text);
                    SearchInfo += "   RM Name: " + txtRMName.Text.Trim();
                }
                if (cboRMType.Text != AllText)
                {
                    SQLConds += "AND [RM Type] = @RMType ";
                    cmd.Parameters.Add("@RMType", SqlDbType.NVarChar, 200).Value = cboRMType.Text;
                    SearchInfo += "   RM Type: " + cboRMType.Text;
                }
                if (cboStatus.Text != AllText)
                {
                    SQLConds += "AND Status = @Status ";
                    cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = cboStatus.Text;
                    SearchInfo += "   Status: " + cboStatus.Text;
                }
                LastSearchInfo = SearchInfo.Trim() == "" ? "All RM" : SearchInfo.Trim();
                cmd.CommandText = "SELECT * FROM (" + MovementQuery + "\n) tbStatus \nWHERE 1 = 1 " + SQLConds + "\nORDER BY [RM Code] ASC";
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dtData);
            }
            catch (Exception ex)
            {
                ErrorText = "Taking Data : \n" + ex.Message;
            }
            cnn.con.Close();

            //Add to DGV
            int CountNoMove = 0;
            int CountSlowMove = 0;
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtData.Rows)
                {
                    int i = DgvSearch.Rows.Add();
                    DataGridViewRow drow = DgvSearch.Rows[i];
                    drow.HeaderCell.Value = (i + 1).ToString();
                    drow.Cells[colRMCode.Index].Value = row["RM Code"].ToString();
                    drow.Cells[colRMName.Index].Value = row["RM Name"].ToString();
                    drow.Cells[colRMType.Index].Value = row["RM Type"].ToString();
                    drow.Cells[colStockQty.Index].Value = Convert.ToDouble(row["Stock Qty"]);
                    drow.Cells[colLastMoveDate.Index].Value = row["Last Movement Date"] == DBNull.Value ? null : (object)Convert.ToDateTime(row["Last Movement Date"]);

                    string Status = row["Status"].ToString();
                    drow.Cells[colStatus.Index].Value = Status;
                    if (Status == NoMovement)
                    {
                        drow.Cells[colStatus.Index].Style.ForeColor = Color.Red;
                        CountNoMove++;
                    }
                    else if (Status == SlowMovement)
                    {
                        drow.Cells[colStatus.Index].Style.ForeColor = Color.Orange;
                        CountSlowMove++;
                    }
                }
                DgvSearch.ClearSelection();
                DgvSearch.CurrentCell = null;
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                LbStatus.Text = NoMovement + " : " + CountNoMove.ToString("N0") + "   |   " + SlowMovement + " : " + CountSlowMove.ToString("N0") + "   |   រកឃើញទិន្នន័យ  " + DgvSearch.Rows.Count.ToString("N0");
                LbStatus.Refresh();
            }
            else
            {
                LbStatus.Text = "មានបញ្ហា!";
                LbStatus.Refresh();
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (DgvSearch.Rows.Count == 0)
            {
                MessageBox.Show("គ្មានទិន្នន័យសម្រាប់ទាញចេញទេ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string TemplatePath = Path.Combine(Environment.CurrentDirectory, ExcelTemplate);
            if (!File.Exists(TemplatePath))
            {
                MessageBox.Show("រកមិនឃើញ Template!\n" + TemplatePath, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ErrorText = "";
            string FullPath = "";
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងទាញទិន្នន័យ . . .";
            LbStatus.Refresh();

            //Grid -> array (one write to Excel is much faster than writing cell by cell)
            int RowCount = DgvSearch.Rows.Count;
            object[,] Data = new object[RowCount, 7];
            for (int i = 0; i < RowCount; i++)
            {
                DataGridViewRow drow = DgvSearch.Rows[i];
                object LastMove = drow.Cells[colLastMoveDate.Index].Value;
                Data[i, 0] = i + 1;
                Data[i, 1] = CellText(drow, colRMCode);
                Data[i, 2] = CellText(drow, colRMName);
                Data[i, 3] = CellText(drow, colRMType);
                Data[i, 4] = drow.Cells[colStockQty.Index].Value;
                Data[i, 5] = LastMove is DateTime ? (object)((DateTime)LastMove).ToOADate() : null;
                Data[i, 6] = CellText(drow, colStatus);
            }

            int LastRow = XlFirstDataRow + RowCount - 1;
            Excel.Application excelApp = null;
            Excel.Workbook xlWorkBook = null;
            Excel.Worksheet worksheet = null;
            try
            {
                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                xlWorkBook = excelApp.Workbooks.Open(TemplatePath);
                worksheet = (Excel.Worksheet)xlWorkBook.Sheets[1];

                worksheet.Range["B4"].Value2 = "📅 " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm") + "   •   👤 " + MenuFormV2.UserForNextForm + "   •   🔎 " + LastSearchInfo;

                //Text columns stay as typed (no number conversion of the codes)
                worksheet.Range["C" + XlFirstDataRow, "E" + LastRow].NumberFormat = "@";
                Excel.Range rngData = worksheet.Range["B" + XlFirstDataRow, "H" + LastRow];
                rngData.Value2 = Data;
                rngData.Font.Name = "Segoe UI";
                rngData.Font.Size = 10;
                rngData.Font.Color = ColorTranslator.ToOle(Color.FromArgb(31, 41, 55));
                rngData.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                rngData.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rngData.RowHeight = 21;

                worksheet.Range["B" + XlFirstDataRow, "B" + LastRow].Font.Color = ColorTranslator.ToOle(Color.FromArgb(156, 163, 175));
                Excel.Range rngName = worksheet.Range["D" + XlFirstDataRow, "D" + LastRow];
                rngName.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                rngName.IndentLevel = 1;
                Excel.Range rngQty = worksheet.Range["F" + XlFirstDataRow, "F" + LastRow];
                rngQty.NumberFormat = "#,##0.###"; //whole numbers -> "#,##0" by conditional format in the template
                rngQty.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                rngQty.IndentLevel = 1;
                rngQty.Font.Bold = true;
                worksheet.Range["G" + XlFirstDataRow, "G" + LastRow].NumberFormat = "[$-409]dd-mmm-yyyy";

                //Filter buttons on the header
                worksheet.Range["B" + (XlFirstDataRow - 1), "H" + LastRow].AutoFilter(1);

                //End note
                Excel.Range rngEnd = worksheet.Range["B" + (LastRow + 2), "H" + (LastRow + 2)];
                rngEnd.Merge();
                rngEnd.Value2 = "✨ End of list  •  " + RowCount.ToString("N0") + " items";
                rngEnd.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rngEnd.Font.Name = "Segoe UI";
                rngEnd.Font.Size = 9;
                rngEnd.Font.Italic = true;
                rngEnd.Font.Color = ColorTranslator.ToOle(Color.FromArgb(156, 163, 175));

                //Save to Report folder
                string SavePath = Path.Combine(Environment.CurrentDirectory, @"Report\RMMovementStatus");
                Directory.CreateDirectory(SavePath);
                FullPath = Path.Combine(SavePath, "RM Movement Status ( " + DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss") + " ).xlsx");
                xlWorkBook.SaveAs(FullPath);
            }
            catch (Exception ex)
            {
                ErrorText = "Write to Excel : \n" + ex.Message;
            }
            finally
            {
                if (xlWorkBook != null)
                    xlWorkBook.Close(false);
                if (excelApp != null)
                    excelApp.Quit();
                if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                if (xlWorkBook != null) Marshal.ReleaseComObject(xlWorkBook);
                if (excelApp != null) Marshal.ReleaseComObject(excelApp);
                //Release the Range objects too, so no hidden EXCEL.EXE stays open
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            if (ErrorText.Trim() == "")
                IgnoreNumberAsTextError(FullPath, "C" + XlFirstDataRow + ":E" + LastRow);
            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                LbStatus.Text = "ទាញទិន្នន័យរួចរាល់!";
                LbStatus.Refresh();
                Process.Start(FullPath);
            }
            else
            {
                LbStatus.Text = "មានបញ្ហា!";
                LbStatus.Refresh();
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //No green "number stored as text" triangle on the text columns (e.g. RM Code 01200300).
        //Excel clears the template's ignore-setting on every cell we write, and setting it again cell by cell
        //through COM is very slow, so the saved file's <ignoredErrors> range is replaced directly.
        //Only a cosmetic fix: any problem here is ignored.
        private static void IgnoreNumberAsTextError(string XlsxPath, string Sqref)
        {
            try
            {
                using (FileStream fs = new FileStream(XlsxPath, FileMode.Open, FileAccess.ReadWrite))
                using (ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry entry = zip.GetEntry("xl/worksheets/sheet1.xml");
                    if (entry == null)
                        return;
                    string xml;
                    using (StreamReader sr = new StreamReader(entry.Open(), Encoding.UTF8))
                        xml = sr.ReadToEnd();

                    //The template has <ignoredErrors>, so Excel keeps the element (at its correct place) in the saved file
                    Regex rx = new Regex("<ignoredErrors>.*?</ignoredErrors>", RegexOptions.Singleline);
                    if (!rx.IsMatch(xml))
                        return;
                    xml = rx.Replace(xml, "<ignoredErrors><ignoredError sqref=\"" + Sqref + "\" numberStoredAsText=\"1\"/></ignoredErrors>", 1);

                    using (Stream s = entry.Open())
                    {
                        s.SetLength(0);
                        byte[] bytes = new UTF8Encoding(false).GetBytes(xml);
                        s.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("IgnoreNumberAsTextError : " + ex.Message);
            }
        }

        private static string CellText(DataGridViewRow drow, DataGridViewColumn col)
        {
            object value = drow.Cells[col.Index].Value;
            return value == null ? "" : value.ToString();
        }

        //Text box value -> LIKE pattern. '*' works as wildcard, other LIKE special characters are escaped.
        private static string ToLikePattern(string text)
        {
            string s = text.Trim().Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%");
            return "%" + s + "%";
        }
    }
}
