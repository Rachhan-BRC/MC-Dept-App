using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;

namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    public partial class NGRatePrintLabelSDWireForm : Form
    {
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        string fName;
        DataTable dtPOSDetails;
        DataTable dtPOSConsumption;
        string ReadNotFound;

        string ErrorText;

        public NGRatePrintLabelSDWireForm()
        {
            InitializeComponent();
            this.cnnOBS.Connection();
            this.btnImport.Click += BtnImport_Click;
            this.btnPrint.Click += BtnPrint_Click;

        }


        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (dtPOSConsumption.Rows.Count > 0)
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់ព្រីនឡាប៊ែលមែនដែរឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    LbStatus.Text = "កំពុងព្រីន . . .";
                    LbStatus.Refresh();
                    string ErrorText = "";
                    var CDirectory = Environment.CurrentDirectory;

                    //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                    string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\NG Rate Label\SD";
                    if (!Directory.Exists(SavePath))
                    {
                        Directory.CreateDirectory(SavePath);
                    }

                    try
                    {
                        Excel.Application excelApp = new Excel.Application();
                        Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\NGRateLabelTemp.xlsx", Editable: true);
                        Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["RachhanSystem"];
                        //add to All Data
                        try
                        {
                            if (dtPOSDetails.Rows.Count == 1)
                            {
                                //Header Info
                                //Shipment Date
                                worksheet.Cells[4, 7] = Convert.ToDateTime(dtPOSDetails.Rows[0]["POSDeliveryDate"].ToString());
                                //Item
                                worksheet.Cells[3, 4] = dtPOSDetails.Rows[0]["WIPName"].ToString();
                                //Mo
                                worksheet.Cells[4, 4] = dtPOSDetails.Rows[0]["DONo"].ToString();
                                //POS Qty
                                worksheet.Cells[5, 4] = dtPOSDetails.Rows[0]["PlanQty"].ToString();

                                //Mat Info
                                int printeRow = 9;
                                for (int i = 0; i < dtPOSConsumption.Rows.Count; i++)
                                {
                                    int SeqNo = 1;
                                    if (dtPOSDetails.Rows[0]["DONo"].ToString() == dtPOSConsumption.Rows[i]["DONo"].ToString())
                                    {
                                        worksheet.Cells[printeRow, 2] = SeqNo;
                                        worksheet.Cells[printeRow, 3] = dtPOSConsumption.Rows[i]["ItemCode"].ToString();
                                        worksheet.Cells[printeRow, 4] = dtPOSConsumption.Rows[i]["ItemName"].ToString();
                                        printeRow = printeRow + 1;
                                        SeqNo++;
                                    }
                                }



                            }
                            else
                            {
                                //Insert new rows
                                for (int j = 0; j < dtPOSDetails.Rows.Count - 1; j++)
                                {
                                    worksheet.Range["A1:A19"].EntireRow.Copy();
                                    int indexExcel = 19 * (j + 1) + 1;
                                    worksheet.Range["A" + indexExcel].EntireRow.PasteSpecial(XlPasteType.xlPasteAll, XlPasteSpecialOperation.xlPasteSpecialOperationNone, Type.Missing, Type.Missing);
                                }
                                // Copy shapes
                                foreach (Excel.Shape shape in worksheet.Shapes)
                                {
                                    if (shape.TopLeftCell.Row >= 1 && shape.TopLeftCell.Row <= 19)
                                    {
                                        shape.Copy();
                                        for (int j = 0; j < dtPOSDetails.Rows.Count - 1; j++)
                                        {
                                            int indexExcel = 19 * (j + 1) + 1;
                                            worksheet.Paste(worksheet.Cells[indexExcel + 19 - 2, 5], shape);
                                        }
                                    }
                                }

                                int ItemRow = 3;
                                int POSNoRow = 4;
                                int DateRow = 4;
                                int POSQtyRow = 5;
                                int MaterialRow = 9;

                                //Starting Write
                                for (int i = 0; i < dtPOSDetails.Rows.Count; i++)
                                {
                                    //Header
                                    worksheet.Cells[ItemRow, 4] = dtPOSDetails.Rows[i]["WIPName"].ToString();
                                    worksheet.Cells[POSNoRow, 4] = dtPOSDetails.Rows[i]["DONo"].ToString();
                                    worksheet.Cells[DateRow, 7] = Convert.ToDateTime(dtPOSDetails.Rows[i]["POSDeliveryDate"].ToString());
                                    worksheet.Cells[POSQtyRow, 4] = dtPOSDetails.Rows[i]["PlanQty"].ToString();

                                    //Material List
                                    int printeRow = MaterialRow;
                                    int SeqNo = 1;
                                    for (int j = 0; j < dtPOSConsumption.Rows.Count; j++)
                                    {
                                        if (dtPOSDetails.Rows[i][0].ToString() == dtPOSConsumption.Rows[j][0].ToString())
                                        {
                                            worksheet.Cells[printeRow, 2] = SeqNo;
                                            worksheet.Cells[printeRow, 3] = dtPOSConsumption.Rows[j]["ItemCode"].ToString();
                                            worksheet.Cells[printeRow, 4] = dtPOSConsumption.Rows[j]["ItemName"].ToString();
                                            printeRow = printeRow + 1;
                                            SeqNo++;
                                        }
                                    }

                                    ItemRow = ItemRow + 19;
                                    POSNoRow = POSNoRow + 19;
                                    DateRow = DateRow + 19;
                                    POSQtyRow = POSQtyRow + 19;
                                    MaterialRow = MaterialRow + 19;
                                }
                            }

                            // Saving the modified Excel file                        
                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                            string file = "NG Rate Print ";
                            fName = file + "( " + date + " )";
                            worksheet.SaveAs(SavePath.ToString() + @"\" + fName + ".xlsx");
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
                        catch (System.Exception ex)
                        {
                            excelApp.DisplayAlerts = false;
                            xlWorkBook.Close();
                            excelApp.DisplayAlerts = true;
                            excelApp.Quit();
                            if (ErrorText.Trim() == "")
                            {
                                ErrorText = ex.Message;
                            }
                            else
                            {
                                ErrorText = ErrorText + "\n" + ex.Message;
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
                        LbStatus.Text = "ព្រីនរួចរាល់!";
                        LbStatus.Refresh();
                        MessageBox.Show("ព្រីនរួចរាល់!" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Diagnostics.Process.Start(SavePath.ToString() + @"\" + fName + ".xlsx");
                        fName = "";
                        dgvExcelData.Rows.Clear();
                        btnPrint.Enabled = false;
                        btnPrint.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
                    }
                    else
                    {
                        LbStatus.Text = "មានបញ្ហា!";
                        LbStatus.Refresh();
                        MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("គ្មានភីអូអេសដែលប្រើ ធើមីណល និង ខ្សែភ្លើង ទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void BtnImport_Click(object sender, EventArgs e)
        {
            ReadExcelData();
        }


        //Method
        private void ReadExcelData()
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងអានទិន្នន័យ . . .";
            LbStatus.Refresh();
            dgvExcelData.Rows.Clear();
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

                    TakePOSConsumption();                    
                    dgvExcelData.ClearSelection();
                    CheckBtnPrint();

                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
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

                Cursor = Cursors.Default;

                if (ErrorText.Trim() == "")
                {
                    if (dtPOSConsumption.Rows.Count > 0)
                    {
                        LbStatus.Text = "ការអានរួចរាល់ !";
                        MessageBox.Show("ការអានរួចរាល់ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        LbStatus.Text = "ការអានរួចរាល់!";
                        MessageBox.Show("ការអានរួចរាល់, ប៉ុន្តែគ្មានទិន្នន័យសម្រាប់ព្រីនទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {

                    LbStatus.Text = "ការអានបរាជ័យ !";
                    MessageBox.Show("មានបញ្ហា!\n"+ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void TakePOSConsumption()
        {
            if (ErrorText.Trim()=="" && dgvExcelData.Rows.Count > 0)
            {
                string POSPIn = "";
                foreach (DataGridViewRow DgvRow in dgvExcelData.Rows)
                {
                    if (POSPIn.Trim() == "")
                    {
                        POSPIn = " '" + DgvRow.Cells[2].Value.ToString() + "' ";
                    }
                    else
                    {
                        POSPIn = POSPIn + ", '" + DgvRow.Cells[2].Value.ToString() + "' ";
                    }
                }         
                
                //Take data from OBS
                try
                {
                    cnnOBS.conOBS.Open();
                    string QueryPOSConsumtion = "SELECT DONo, T1.ItemCode AS WIPCode, T6.ItemName AS WIPName, PlanQty, POSDeliveryDate, ConsumpSeqNo, T4.ItemCode, T4.ItemName, MatTypeName, ConsumpQty, (ConsumpQty*0.01) AS NGRate FROM \n" +
                        "\t(SELECT * FROM prgproductionorder WHERE LineCode='MC1') T1 \n" +
                        "\tINNER JOIN (SELECT * FROM mstitem WHERE ItemType = 1 AND DelFlag = 0) T2 \n" +
                        "\tON T1.ItemCode=T2.ItemCode \n" +
                        "\tINNER JOIN (SELECT * FROM prgconsumtionorder) T3 \n" +
                        "\tON T1.ProductionCode=T3.ProductionCode \n" +
                        "\tINNER JOIN (SELECT * FROM mstitem WHERE ItemType = 2 AND DelFlag=0) T4 \n" +
                        "\tON T3.ItemCode = T4.ItemCode \n" +
                        "\tINNER JOIN (SELECT * FROM MstMatType WHERE DelFlag=0) T5 \n" +
                        "\tON T4.MatTypeCode=T5.MatTypeCode \n" +
                        "\tINNER JOIN (SELECT * FROM mstitem WHERE ItemType=1) T6 \n" +
                        "\tON T1.ItemCode=T6.ItemCode \n" +
                        "\tWHERE NOT MatTypeName = 'Connector' AND PPOSNO IN ( "+POSPIn+" ) \n";

                    //POS Consumption
                    SqlDataAdapter sda = new SqlDataAdapter(QueryPOSConsumtion + 
                        "ORDER BY DONo ASC, ConsumpSeqNo ASC", cnnOBS.conOBS);
                    dtPOSConsumption = new DataTable();
                    sda.Fill(dtPOSConsumption);

                    //POS Detail
                    sda = new SqlDataAdapter("SELECT DONo, WIPCode, WIPName, PlanQty, POSDeliveryDate FROM ("+QueryPOSConsumtion+ ") TbHeader " +
                        "GROUP BY DONo, WIPCode, WIPName, PlanQty, POSDeliveryDate " +
                        "ORDER BY DONo ASC", cnnOBS.conOBS);
                    dtPOSDetails = new DataTable();
                    sda.Fill(dtPOSDetails);
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                }
                cnnOBS.conOBS.Close();

                //Remove only one Wire
                for (int i = dtPOSDetails.Rows.Count - 1; i > -1; i--)
                {
                    string Type = "";
                    foreach (DataRow rowConsumption in dtPOSConsumption.Rows)
                    {
                        if (dtPOSDetails.Rows[i]["DONo"].ToString() == rowConsumption["DONo"].ToString())
                        {
                            Type += rowConsumption["MatTypeName"].ToString();
                        }
                    }
                    if (Type.Replace("Wire","").Trim() == "")
                    {
                        dtPOSDetails.Rows.RemoveAt(i);
                        dtPOSDetails.AcceptChanges();
                    }
                }

                string ConsoleText = "";
                foreach (DataColumn col in dtPOSConsumption.Columns)
                {
                    if (ConsoleText.Trim() == "")
                    {
                        ConsoleText = col.ColumnName.ToString();
                    }
                    else
                    {
                        ConsoleText = ConsoleText + "\t" + col.ColumnName.ToString();
                    }
                }
                //Console.WriteLine(ConsoleText);
                foreach (DataRow row in dtPOSConsumption.Rows)
                {
                    ConsoleText = "";
                    foreach (DataColumn col in dtPOSConsumption.Columns)
                    {
                        if (col.ColumnName.ToString() == "NGRate")
                        {
                            row[col] = Math.Floor(Convert.ToDouble(row[col].ToString()));
                            dtPOSConsumption.AcceptChanges();
                        }
                        if (ConsoleText.Trim() == "")
                        {
                            ConsoleText = row[col].ToString();
                        }
                        else
                        {
                            ConsoleText = ConsoleText + "\t" + row[col].ToString();
                        }
                    }
                    //Console.WriteLine(ConsoleText);
                }

            }
        }
        private void CheckBtnPrint()
        {
            if (dtPOSConsumption.Rows.Count > 0)
            {
                btnPrint.Enabled = true;
                btnPrint.BackColor = Color.White;
                btnPrint.Focus();
            }
            else
            {
                btnPrint.Enabled = false;
                btnPrint.BackColor = Color.DarkGray;
            }
        }
    
    }
}
