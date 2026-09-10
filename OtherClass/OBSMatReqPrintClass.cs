using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.OtherClass
{
    internal class OBSMatReqPrintClass
    {
        public static string SavePath { private set; get; }
        public static string ErrorText { private set; get; }
        public void PrintExcelOut(DateTime PrintedDate, string PrintedBy, DataTable dtPrintList)
        {
            SavePath = ""; ErrorText = "";

            if (dtPrintList == null || dtPrintList.Rows.Count == 0)
            {
                ErrorText = "គ្មានទិន្នន័យសម្រាប់ព្រីនទេ!";
                return;
            }

            //Sort By RMType, ItemCode ASC
            DataView dv = dtPrintList.DefaultView;
            dv.Sort = "RMType ASC, ItemCode ASC";
            dtPrintList = dv.ToTable();

            string reportFolder = Path.Combine(Environment.CurrentDirectory, @"Report\OBS Material Request");
            if (!Directory.Exists(reportFolder))
                Directory.CreateDirectory(reportFolder);

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(
                Path.Combine(Environment.CurrentDirectory, @"Template\OBSMatReq Template.xlsx"), Editable: true);
            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets[1];

            string savedFilePath = "";
            try
            {
                //Header
                worksheet.Cells[2, 2] = dtPrintList.Rows[0]["Remarks"].ToString();
                worksheet.Cells[2, 8] = PrintedDate;

                //Footer
                worksheet.Cells[7, 3] = PrintedBy;

                //Insert rows for data if there are more than 1 row in the DataTable
                if (dtPrintList.Rows.Count > 1)
                {
                    worksheet.Range["5:" + (dtPrintList.Rows.Count+3)].Insert();
                    worksheet.Range["A5:H" + (dtPrintList.Rows.Count + 3)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    worksheet.Range["A5:H" + (dtPrintList.Rows.Count + 3)].Borders.Weight = Excel.XlBorderWeight.xlHairline;
                    worksheet.Range["A5:A" + (dtPrintList.Rows.Count + 3)].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                    worksheet.Range["A5:A" + (dtPrintList.Rows.Count + 3)].Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlMedium;
                }

                //Write data to the worksheet
                foreach (DataRow row in dtPrintList.Rows)
                {
                    worksheet.Cells[dtPrintList.Rows.IndexOf(row) + 4, 1] = row["ItemCode"].ToString();
                    worksheet.Cells[dtPrintList.Rows.IndexOf(row) + 4, 2] = row["ItemName"].ToString();
                    worksheet.Cells[dtPrintList.Rows.IndexOf(row) + 4, 3] = row["Maker"].ToString();
                    worksheet.Cells[dtPrintList.Rows.IndexOf(row) + 4, 4] = row["RMType"].ToString();
                    worksheet.Cells[dtPrintList.Rows.IndexOf(row) + 4, 5] = Convert.ToInt32(row["PackSize"]);
                    worksheet.Cells[dtPrintList.Rows.IndexOf(row) + 4, 6] = Convert.ToInt32(row["PackQty"]);
                    worksheet.Cells[dtPrintList.Rows.IndexOf(row) + 4, 7] = Convert.ToInt32(row["TTLReqQty"]);
                    worksheet.Cells[dtPrintList.Rows.IndexOf(row) + 4, 8] = "*" + row["MATReqNo"].ToString() + "*";

                }

                string fileName = "OBS Mat Request " + DateTime.Now.ToString("yyyyMMdd HH_mm_ss") + ".xlsx";
                savedFilePath = Path.Combine(reportFolder, fileName);
                xlWorkBook.SaveAs(savedFilePath);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }

            excelApp.DisplayAlerts = false;
            xlWorkBook.Close();
            excelApp.Quit();
            excelApp.DisplayAlerts = true;

            //Kill all Excel background process
            var processes = from p in Process.GetProcessesByName("EXCEL")
                            select p;
            foreach (var process in processes)
            {
                if (process.MainWindowTitle.ToString().Trim() == "")
                    process.Kill();
            }

            if (ErrorText.Trim() == "")
                SavePath = savedFilePath;
        }
    }
}
