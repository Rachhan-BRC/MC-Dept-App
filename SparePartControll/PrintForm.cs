
using System;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.SparePartControll
{
    public partial class PrintForm : Form
    {
        SQLConnect con = new SQLConnect();
        string dept = "MC";
        string nextdocno = "";
        public PrintForm()
        {
            con.Connection();
            InitializeComponent();
            this.Load += PrintForm_Load;
            this.btnAddColor.Click += BtnAddColor_Click;
            this.btnPrint.Click += BtnPrint_Click;
            this.dgvTTL.CellClick += DgvTTL_CellClick;
            this.btnDeletColor.Click += BtnDeletColor_Click;
            this.btnPrintAgain.Click += BtnPrintAgain_Click;
        }

        private void BtnPrintAgain_Click(object sender, EventArgs e)
        {

            if (dgvTTL.Rows.Count == 0)
            {
                MessageBox.Show("No data to export!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Cursor = Cursors.WaitCursor;
            PrintAgain();
            Cursor = Cursors.Default;
        }

        private void BtnDeletColor_Click(object sender, EventArgs e)
        {
            int rowIndex = dgvTTL.CurrentCell.RowIndex;
            if (rowIndex >= 0 && rowIndex < dgvTTL.Rows.Count)
            {
                dgvTTL.Rows.RemoveAt(rowIndex);
                btnDeletColor.Enabled = false;
                btnDeletColor.SendToBack();
                dgvTTL.ClearSelection();
            }
        }

        private void DgvTTL_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >=0)
            {
                btnDeletColor.Enabled = true;
                btnDeletColor.BringToFront();
            }

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (dgvTTL.Rows.Count == 0)
            {
                return;
            }
            int cannot = 0;
            try
            {
                DataTable dtMst = new DataTable();
                Cursor = Cursors.WaitCursor;
                List<string> code = new List<string>();
                foreach (DataGridViewRow row in dgvTTL.Rows)
                {
                    code.Add(row.Cells["Code"].Value.ToString());
                }
                string codelist = "('" + string.Join("','", code) + "')";
                con.con.Close();
                //select from master
                try
                {
                    con.con.Open();
                    string query = "SELECT Code, Part_No, Unit_Price FROM MstMCSparePart WHERE Dept =  '" + dept + "' AND Code IN " + codelist + "ORDER BY Code";
                    SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                    sda.Fill(dtMst);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while selecting master: " + ex.Message);
                }
                con.con.Close();
                if (dtMst.Rows.Count == 0)
                {
                    MessageBox.Show("This spare part not have in Master", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                foreach (DataGridViewRow row1 in dgvTTL.Rows)
                {
                    string code1 = row1.Cells["code"].Value.ToString();
                    foreach (DataRow row in dtMst.Rows)
                    {
                        string code2 = row["Code"].ToString();
                        if (code1 == code2)
                        {
                            row1.Cells["find"].Value = "Have";
                            break;
                        }
                    }
                }
                foreach (DataGridViewRow row1 in dgvTTL.Rows)
                {
                    if (row1.Cells["find"].Value?.ToString() == "Not")
                    {
                        row1.Cells["code"].Style.BackColor = Color.LightPink;
                        cannot++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error while compare master!" +ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (cannot == 0)
            {
                try
                {
                
                    DateTime now1 = DateTime.Now;
                    string now = now1.ToString("yyyy-MM-dd");
                    string pono = GetNextDocNo();
                    nextdocno = pono;
                    foreach (DataGridViewRow row1 in dgvTTL.Rows)
                    {
                        string code = row1.Cells["Code"].Value.ToString();
                        double orderqty = Convert.ToDouble(row1.Cells["qty"].Value);
                        double unitprice = Convert.ToDouble(row1.Cells["unitprice"].Value);
                        double amount = Convert.ToDouble(row1.Cells["amount"].Value);
                        DateTime Eta = Convert.ToDateTime(row1.Cells["eta"].Value);
                       
                        try
                        {
                            con.con.Open();
                            string query = "INSERT INTO MCSparePartRequest (Code, PO_No, Dept, IssueDate, ETA, Order_Qty, UnitPrice, Amount, ReceiveQTY, Balance, RemainAmount, Receive_Date, Order_State, Remark) " +
                                                                                        " VALUES (@code, @pono, @dept, @issuedate, @eta, @orderqty, @unitprice, @amount, @receiveqty, @balance, @reAmount, @recdate, @orderstate, @remark)";
                            SqlCommand cmd = new SqlCommand(query, con.con);
                            cmd.Parameters.AddWithValue("@code", code);
                            cmd.Parameters.AddWithValue("@pono", pono);
                            cmd.Parameters.AddWithValue("@dept", dept);
                            cmd.Parameters.AddWithValue("@issuedate", now);
                            cmd.Parameters.AddWithValue("@orderqty", orderqty);
                            cmd.Parameters.AddWithValue("@unitprice", unitprice);
                            cmd.Parameters.AddWithValue("@amount", amount);
                            cmd.Parameters.AddWithValue("@receiveqty", 0);
                            cmd.Parameters.AddWithValue("@balance", orderqty);
                            cmd.Parameters.AddWithValue("@reAmount", amount);
                            cmd.Parameters.AddWithValue("@eta", Eta);
                            cmd.Parameters.AddWithValue("@recdate", Eta);
                            cmd.Parameters.AddWithValue("@orderstate", "Waiting for PO Update");
                            cmd.Parameters.AddWithValue("@remark", "Update: " + now);
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error while getting Docno! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        con.con.Close();
                    }
                    try
                    {
                        con.con.Open();
                        string queryDocNo = "INSERT INTO MCDocNo (DocNo) VALUES (@DocNo)";
                        SqlCommand cmdDocNo = new SqlCommand(queryDocNo, con.con);
                        cmdDocNo.Parameters.AddWithValue("@DocNo", pono);
                        cmdDocNo.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while inserting data! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    con.con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while inserting data! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
              
                if (rdnormal.Checked == true)
                {
                    ExportToExcelFromTemplate();
                }
                if (rdMTH.Checked == true)
                {
                    ExportToExcelFromTemplateMTH();
                }
            }
           if (cannot > 0)
            {
                MessageBox.Show("Some spare part not have in Master", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           Cursor = Cursors.Default;
        }


        private void PrintForm_Load(object sender, EventArgs e)
        {
            btnPrintAgain.Enabled = false;
            rdnormal.Checked = true;
        }

        private void BtnAddColor_Click(object sender, EventArgs e)
        {
            AddPrint ad = new AddPrint(dgvTTL);
            ad.ShowDialog();
        }
        private void ExportToExcelFromTemplate()
        {
            if (dgvTTL.Rows.Count == 0)
            {
                MessageBox.Show("No data to export!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Cursor = Cursors.WaitCursor;
            try
            {
                // Ensure folder exists
                string SavePath = Path.Combine(Environment.CurrentDirectory, @"Report\SparePartRequestSheetIPO");
                Directory.CreateDirectory(SavePath);

                // Open Excel template
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(
                    Path.Combine(Environment.CurrentDirectory, @"Template\SparePartRequestSheetIPO.xlsx"), Editable: true);
                Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets[1];

                int startRow = 4;

                // Fill Excel from DataGridView
                string date = DateTime.Now.ToString("dd");
                string month = DateTime.Now.ToString("MM");
                string year = DateTime.Now.ToString("yyyy");
                for (int i = 0; i < dgvTTL.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvTTL.Rows[i];

                    if (i > 0)
                    {
                        // Insert new row with same format
                        Excel.Range sourceRow = worksheet.Rows[startRow];
                        sourceRow.Copy();

                        Excel.Range insertRow = worksheet.Rows[startRow + i];
                        insertRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                    }

                    worksheet.Cells[startRow + i, 1] = row.Cells["Code"].Value?.ToString();
                    worksheet.Cells[startRow + i, 2] = row.Cells["partno"].Value?.ToString();
                    worksheet.Cells[startRow + i, 3] = row.Cells["partname"].Value?.ToString();
                    worksheet.Cells[startRow + i, 4] = row.Cells["machinename"].Value?.ToString();
                    worksheet.Cells[startRow + i, 5] = row.Cells["supplier"].Value?.ToString();
                    worksheet.Cells[startRow + i, 6] = row.Cells["maker"].Value?.ToString();
                    worksheet.Cells[startRow + i, 7] = row.Cells["qty"].Value?.ToString();
                    worksheet.Cells[startRow + i, 8] = row.Cells["unitprice"].Value?.ToString();
                    worksheet.Cells[startRow + i, 9] = row.Cells["amount"].Value?.ToString();

                    worksheet.Cells[startRow + i, 11] = row.Cells["leadtime"].Value?.ToString();
                    worksheet.Cells[startRow + 4 + i, 8] = "Date:   " + date + "  /  " + month + "  /  " + year + "";
                }
                // Save Excel

                string DateExcel = DateTime.Now.ToString("yyMMdd");
                string fileName = "Unit Price Request "+nextdocno +").xlsx";

                string fullPath = Path.Combine(SavePath, fileName);
             
                worksheet.Cells[2, 10] = nextdocno;
             
                xlWorkBook.SaveAs(fullPath);

                // Cleanup
                excelApp.DisplayAlerts = false;
                xlWorkBook.Close();
                excelApp.Quit();
                excelApp.DisplayAlerts = true;

                // Release COM objects to avoid leaving Excel.exe open
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                dgvTTL.Rows.Clear();
                MessageBox.Show("Print Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(fullPath);
            }
            catch (Exception ex)
            { 
                btnPrintAgain.Enabled = true;
                btnPrintAgain.BringToFront();
                Cursor = Cursors.Default;
                MessageBox.Show("File excel នេះកំពុងបើក, សូមបិទជាមុនសិន​ រួច Print ម្ដងទៀត!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Cursor = Cursors.Default;
        }
        private void ExportToExcelFromTemplateMTH()
        {
            if (dgvTTL.Rows.Count == 0)
            {
                MessageBox.Show("No data to export!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Cursor = Cursors.WaitCursor;
            try
            {
                // Ensure folder exists
                string SavePath = Path.Combine(Environment.CurrentDirectory, @"Report\SparePartRequestSheetMTH");
                Directory.CreateDirectory(SavePath);

                // Open Excel template
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(
                    Path.Combine(Environment.CurrentDirectory, @"Template\SparePartRequestSheetMTH.xlsx"), Editable: true);
                Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets[1];

                int startRow = 4;

                // Fill Excel from DataGridView
                string date = DateTime.Now.ToString("dd");
                string month = DateTime.Now.ToString("MM");
                string year = DateTime.Now.ToString("yyyy");
                for (int i = 0; i < dgvTTL.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvTTL.Rows[i];

                    if (i > 0)
                    {
                        // Insert new row with same format
                        Excel.Range sourceRow = worksheet.Rows[startRow];
                        sourceRow.Copy();

                        Excel.Range insertRow = worksheet.Rows[startRow + i];
                        insertRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                    }

                    worksheet.Cells[startRow + i, 1] = row.Cells["Code"].Value?.ToString();
                    worksheet.Cells[startRow + i, 2] = row.Cells["partno"].Value?.ToString();
                    worksheet.Cells[startRow + i, 3] = row.Cells["partname"].Value?.ToString();
                    worksheet.Cells[startRow + i, 4] = row.Cells["machinename"].Value?.ToString();
                    worksheet.Cells[startRow + i, 5] = row.Cells["supplier"].Value?.ToString();
                    worksheet.Cells[startRow + i, 6] = row.Cells["maker"].Value?.ToString();
                    worksheet.Cells[startRow + i, 7] = row.Cells["qty"].Value?.ToString();
                    worksheet.Cells[startRow + i, 8] = row.Cells["unitprice"].Value?.ToString();
                    worksheet.Cells[startRow + i, 9] = row.Cells["amount"].Value?.ToString();

                    worksheet.Cells[startRow + i, 11] = row.Cells["leadtime"].Value?.ToString();
                    worksheet.Cells[startRow + 4 + i, 8] = "Date:   " + date + "  /  " + month + "  /  " + year + "";
                }
                // Save Excel
                string nextDocNo = nextdocno;
                string DateExcel = DateTime.Now.ToString("yyMMdd");
                string fileName = "Unit Price Request "+nextDocNo+".xlsx";

                string fullPath = Path.Combine(SavePath, fileName);

               
                worksheet.Cells[2, 10] = nextDocNo;
                xlWorkBook.SaveAs(fullPath);

                // Cleanup
                excelApp.DisplayAlerts = false;
                xlWorkBook.Close();
                excelApp.Quit();
                excelApp.DisplayAlerts = true;

                // Release COM objects to avoid leaving Excel.exe open
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                dgvTTL.Rows.Clear();
                MessageBox.Show("Print Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(fullPath);
            }
            catch (Exception ex)
            {
                btnPrintAgain.Enabled = true;
                btnPrintAgain.BringToFront();
                Cursor = Cursors.Default;
                MessageBox.Show("File excel នេះកំពុងបើក, សូមបិទជាមុនសិន​ រួច Print ម្ដងទៀត!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Cursor = Cursors.Default;
        }
        private void PrintAgain()
        {
          if (rdnormal.Checked == true)
            {
                try
                {
                    // Ensure folder exists
                    string SavePath = Path.Combine(Environment.CurrentDirectory, @"Report\SparePartRequestSheetIPO");
                    Directory.CreateDirectory(SavePath);

                    // Open Excel template
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(
                        Path.Combine(Environment.CurrentDirectory, @"Template\SparePartRequestSheetIPO.xlsx"), Editable: true);
                    Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets[1];

                    int startRow = 4;

                    // Fill Excel from DataGridView
                    string date = DateTime.Now.ToString("dd");
                    string month = DateTime.Now.ToString("MM");
                    string year = DateTime.Now.ToString("yyyy");
                    for (int i = 0; i < dgvTTL.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvTTL.Rows[i];

                        if (i > 0)
                        {
                            // Insert new row with same format
                            Excel.Range sourceRow = worksheet.Rows[startRow];
                            sourceRow.Copy();

                            Excel.Range insertRow = worksheet.Rows[startRow + i];
                            insertRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                        }

                        worksheet.Cells[startRow + i, 1] = row.Cells["Code"].Value?.ToString();
                        worksheet.Cells[startRow + i, 2] = row.Cells["partno"].Value?.ToString();
                        worksheet.Cells[startRow + i, 3] = row.Cells["partname"].Value?.ToString();
                        worksheet.Cells[startRow + i, 4] = row.Cells["machinename"].Value?.ToString();
                        worksheet.Cells[startRow + i, 5] = row.Cells["supplier"].Value?.ToString();
                        worksheet.Cells[startRow + i, 6] = row.Cells["maker"].Value?.ToString();
                        worksheet.Cells[startRow + i, 7] = row.Cells["qty"].Value?.ToString();
                        worksheet.Cells[startRow + i, 8] = row.Cells["unitprice"].Value?.ToString();
                        worksheet.Cells[startRow + i, 9] = row.Cells["amount"].Value?.ToString();

                        worksheet.Cells[startRow + i, 11] = row.Cells["leadtime"].Value?.ToString();
                        worksheet.Cells[startRow + 4 + i, 8] = "Date:   " + date + "  /  " + month + "  /  " + year + "";
                    }
                    // Save Excel

                    string DateExcel = DateTime.Now.ToString("yyMMdd");
                    string fileName = "Unit Price Request MC(" + DateExcel + "01).xlsx";

                    string fullPath = Path.Combine(SavePath, fileName);

                    worksheet.Cells[2, 10] = "MC" + DateExcel + "01";

                    xlWorkBook.SaveAs(fullPath);

                    // Cleanup
                    excelApp.DisplayAlerts = false;
                    xlWorkBook.Close();
                    excelApp.Quit();
                    excelApp.DisplayAlerts = true;

                    // Release COM objects to avoid leaving Excel.exe open
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                    dgvTTL.Rows.Clear();
                    MessageBox.Show("Print Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start(fullPath);
                    btnPrintAgain.Enabled = false;
                    btnPrint.BringToFront();
                }
                catch (Exception ex)
                {
                    btnPrintAgain.Enabled = true;
                    btnPrintAgain.BringToFront();
                    Cursor = Cursors.Default;
                    MessageBox.Show("File excel នេះកំពុងបើក, សូមបិទជាមុនសិន​ រួច Print ម្ដងទៀត!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
          if (rdMTH.Checked == true)
            {
                try
                {
                    // Ensure folder exists
                    string SavePath = Path.Combine(Environment.CurrentDirectory, @"Report\SparePartRequestSheetMTH");
                    Directory.CreateDirectory(SavePath);

                    // Open Excel template
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(
                        Path.Combine(Environment.CurrentDirectory, @"Template\SparePartRequestSheetMTH.xlsx"), Editable: true);
                    Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets[1];

                    int startRow = 4;

                    // Fill Excel from DataGridView
                    string date = DateTime.Now.ToString("dd");
                    string month = DateTime.Now.ToString("MM");
                    string year = DateTime.Now.ToString("yyyy");
                    for (int i = 0; i < dgvTTL.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvTTL.Rows[i];

                        if (i > 0)
                        {
                            // Insert new row with same format
                            Excel.Range sourceRow = worksheet.Rows[startRow];
                            sourceRow.Copy();

                            Excel.Range insertRow = worksheet.Rows[startRow + i];
                            insertRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                        }

                        worksheet.Cells[startRow + i, 1] = row.Cells["Code"].Value?.ToString();
                        worksheet.Cells[startRow + i, 2] = row.Cells["partno"].Value?.ToString();
                        worksheet.Cells[startRow + i, 3] = row.Cells["partname"].Value?.ToString();
                        worksheet.Cells[startRow + i, 4] = row.Cells["machinename"].Value?.ToString();
                        worksheet.Cells[startRow + i, 5] = row.Cells["supplier"].Value?.ToString();
                        worksheet.Cells[startRow + i, 6] = row.Cells["maker"].Value?.ToString();
                        worksheet.Cells[startRow + i, 7] = row.Cells["qty"].Value?.ToString();
                        worksheet.Cells[startRow + i, 8] = row.Cells["unitprice"].Value?.ToString();
                        worksheet.Cells[startRow + i, 9] = row.Cells["amount"].Value?.ToString();

                        worksheet.Cells[startRow + i, 11] = row.Cells["leadtime"].Value?.ToString();
                        worksheet.Cells[startRow + 4 + i, 8] = "Date:   " + date + "  /  " + month + "  /  " + year + "";
                    }
                    // Save Excel

                    string DateExcel = DateTime.Now.ToString("yyMMdd");
                    string fileName = "Unit Price Request MC(" + DateExcel + "01).xlsx";

                    string fullPath = Path.Combine(SavePath, fileName);

                    worksheet.Cells[2, 10] = "MC" + DateExcel + "01";

                    xlWorkBook.SaveAs(fullPath);

                    // Cleanup
                    excelApp.DisplayAlerts = false;
                    xlWorkBook.Close();
                    excelApp.Quit();
                    excelApp.DisplayAlerts = true;

                    // Release COM objects to avoid leaving Excel.exe open
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                    dgvTTL.Rows.Clear();
                    MessageBox.Show("Print Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start(fullPath);
                    btnPrintAgain.Enabled = false;
                    btnPrint.BringToFront();
                }
                catch (Exception ex)
                {
                    btnPrintAgain.Enabled = true;
                    btnPrintAgain.BringToFront();
                    Cursor = Cursors.Default;
                    MessageBox.Show("File excel នេះកំពុងបើក, សូមបិទជាមុនសិន​ រួច Print ម្ដងទៀត!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
        private string GetNextDocNo()
        {
            string currentDocNo = "";
            // Build prefix: Dept + YYMMDD
            string currentDateString = dept + DateTime.Now.ToString("yyMMdd");
            Cursor = Cursors.WaitCursor;
            try
            {
                DataTable docno = new DataTable();
                con.con.Open();
                string query = "SELECT MAX(DocNo) AS DocNo FROM MCDocNo WHERE DocNo LIKE '%" + currentDateString + "%' ";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(docno);
                con.con.Close();
                string docnoLast = docno.Rows[0][0] == DBNull.Value? null : docno.Rows[0][0].ToString();
                if (docnoLast == null)
                {
                    currentDocNo = currentDateString + "01";
                }
                else
                {
                    string lastIndex = docnoLast.Substring(docnoLast.Length - 2);
                    int NewIndex = Convert.ToInt32(lastIndex) + 1;
                    string NewIndexStr = NewIndex.ToString("D2");
                    currentDocNo = currentDateString + NewIndexStr;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while give Docno !"+ ex.Message, "Error." , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor = Cursors.Default;
            return currentDocNo;
        }
    }
}
