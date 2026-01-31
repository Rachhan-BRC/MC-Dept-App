using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management.Instrumentation;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace MachineDeptApp
{
    public partial class balance : Form
    {
        string dept = "MC";
        SQLConnect con = new SQLConnect();
        public balance()
        {
            this.con.Connection();
            InitializeComponent();
            //btn
            this.btnSearch.Click += BtnSearch_Click;
            this.btnExport.Click += BtnExport_Click;
            this.btnPrint.Click += BtnPrint_Click;

            ////txt
            this.txtRcode.TextChanged += TxtRcode_TextChanged;
            this.txtRname.TextChanged += TxtRname_TextChanged;

            //form
            this.Load += Balance_Load;

            //dgv
            this.dtpDate.ValueChanged += DtpDate_ValueChanged;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (dgvTTL.Rows.Count == 0)
            {
                MessageBox.Show("No data to export!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DialogResult ask = MessageBox.Show("Are you want to print this data ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (ask == DialogResult.No)
            {
                return;
            }
            Cursor = Cursors.WaitCursor;
            try
            {
                // Ensure folder exists
                string SavePath = Path.Combine(Environment.CurrentDirectory, @"Report\SparePartBalance");
                Directory.CreateDirectory(SavePath);

                // Open Excel template
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(
                    Path.Combine(Environment.CurrentDirectory, @"Template\SparePartBalance.xlsx"), Editable: true);
                Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets[1];

                int startRow = 3;

                // Fill Excel from DataGridView
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
                    worksheet.Cells[startRow + i, 4] = row.Cells["supplier"].Value?.ToString();
                    worksheet.Cells[startRow + i, 5] = row.Cells["prestock"].Value?.ToString();
                    worksheet.Cells[startRow + i, 6] = row.Cells["stockout"].Value?.ToString();
                    worksheet.Cells[startRow + i, 7] = row.Cells["stockin"].Value?.ToString();
                    worksheet.Cells[startRow + i, 8] = row.Cells["stockremain"].Value?.ToString();
                    worksheet.Cells[startRow + i, 9] = row.Cells["safetystock"].Value?.ToString();
                    worksheet.Cells[startRow + i, 10] = row.Cells["box"].Value?.ToString();
                    worksheet.Cells[startRow + i, 11] = row.Cells["orderqty"].Value?.ToString();
                    worksheet.Cells[startRow + i, 12] = row.Cells["leadtime"].Value?.ToString();
                    worksheet.Cells[startRow + i, 13] = row.Cells["eta"].Value?.ToString();
                    worksheet.Cells[startRow + i, 14] = row.Cells["status"].Value?.ToString();

                }
                // Save Excel

                string DateExcel = DateTime.Now.ToString("yyMMdd");
                string fileName = "Spare - Part Balance"+DateTime.Now.ToString("yyyy-MM-dd HHmmss")+".xlsx";

                string fullPath = Path.Combine(SavePath, fileName);
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
                MessageBox.Show("Print Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(fullPath);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("File excel នេះកំពុងបើក, សូមបិទជាមុនសិន​ រួច Print ម្ដងទៀត!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Cursor = Cursors.Default;
        }

        //btn
        private void BtnExport_Click(object sender, EventArgs e) 
        {
            DataTable dtselect = new DataTable();
            DataTable dtmaster = new DataTable();
            try
            {
                con.con.Open();
                //new stock 
                int year = dtpDate.Value.Year;
                int month = dtpDate.Value.Month;
                int day = DateTime.DaysInMonth(year, month);
                DateTime firstDay = new DateTime(year, month, 1);
                DateTime lastDay = new DateTime(year, month, day);
                //for pre stockint 
                DateTime prevMonthDate = dtpDate.Value.AddMonths(-1);
                int prevYear = prevMonthDate.Year;
                int prevMonth = prevMonthDate.Month;
                int prevDay = DateTime.DaysInMonth(prevYear, prevMonth);
                DateTime preStockLastDay = new DateTime(prevYear, prevMonth, prevDay);
                string query = "SELECT tbTr.Code, tbMSt.Supplier, tbMst.Part_No, tbMst.Part_Name, tbPre.StockIn, tbPre.StockOut, tbPreS.PreStock " +
                    "" + "FROM SparePartTrans tbTr " + "LEFT JOIN (SELECT Code,Supplier,Part_Name,Part_No FROM MstMCSparePart) tbMst ON tbMst.Code = tbTr.Code "
                    + "LEFT JOIN (SELECT Code, SUM(Stock_In) AS StockIn, SUM(Stock_Out) AS StockOut FROM SparePartTrans " +
                    "WHERE RegDate BETWEEN '" + firstDay + "' AND '" + lastDay + "' AND Dept ='" + dept + "' GROUP BY Code) tbPre ON tbPre.Code = tbTr.Code " +
                    "LEFT JOIN (SELECT Code, SUM(Stock_Value) AS PreStock FROM SparePartTrans " +
                    "WHERE CAST(RegDate AS date) <= '" + preStockLastDay + "' AND Dept ='MC' GROUP BY Code) tbPreS ON tbPreS.Code = tbPre.Code " +
                    "WHERE CAST(tbTr.RegDate AS DATE) > '" + preStockLastDay + "' AND tbTr.Dept ='" + dept + "' " +
                    "GROUP BY tbTr.Code, tbMst.Supplier, tbMst.Part_No, tbMst.Part_Name, tbPre.StockIn, tbPre.StockOut, tbPreS.PreStock " +
                    "ORDER BY tbTr.Code";
              
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dtselect);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while selecting data !" + ex.Message, "Error tbTrans.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.con.Close();
            }
            try 
            {
                con.con.Open(); 
                string querymaster = "SELECT Code, Part_NO AS [Part No], Part_Name AS [Part Name], Supplier, Safety_Stock AS [Safety Stock], Box, Status,Lead_Time_Week AS [Lead time] " +
                    "FROM MstMCSparePart WHERE Dept = '" + dept + "'"; 
                SqlDataAdapter sda = new SqlDataAdapter(querymaster, con.con); 
                sda.Fill(dtmaster); 
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Error while selecting master ! " + ex.Message, "Error master", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
            con.con.Close();

            if (dtmaster.Rows.Count > 0)
            {
                //Combine DataTable
                dtmaster.Columns.Add("Stock IN");
                dtmaster.Columns.Add("Stock Out");
                dtmaster.Columns.Add("Previous Stock");
                dtmaster.AcceptChanges();
                foreach (DataRow row in dtselect.Rows)
                {
                    foreach (DataRow row2 in dtmaster.Rows)
                    {
                        if (row["Code"].ToString() == row2["Code"].ToString())
                        {
                            row2["Stock IN"] = row["StockIn"];
                            row2["Stock Out"] = row["StockOut"];
                            row2["Previous Stock"] = row["PreStock"];
                            dtmaster.AcceptChanges();
                            break;
                        }
                    }
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV file (*.csv)|*.csv";
                saveDialog.FileName = "Spare-Part Balance "+DateTime.Now.ToString("yyyyMMdd HHmmss");
                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        //Write Column name
                        int columnCount = dtmaster.Columns.Count;
                        string columnNames = "";

                        //String array for Csv
                        string[] outputCsv;
                        outputCsv = new string[dtmaster.Rows.Count + 1];

                        //Set Column Name
                        for (int i = 0; i < columnCount; i++)
                        {
                            columnNames += dtmaster.Columns[i].ColumnName.ToString() + ",";
                        }
                        outputCsv[0] += columnNames;

                        //Row of data 
                        for (int i = 1; (i - 1) < dtmaster.Rows.Count; i++)
                        {
                            for (int j = 0; j < columnCount; j++)
                            {
                                string Value = "";
                                Value = dtmaster.Rows[i - 1][j].ToString();
                                //Fix don't separate if it contain '\n' or ','
                                Value = "\"" + Value.Replace("\"", "\"\"") + "\"";
                                outputCsv[i] += Value + ",";
                            }
                        }

                        File.WriteAllLines(saveDialog.FileName, outputCsv, Encoding.UTF8);
                        Cursor.Current = Cursors.Default; 
                        MessageBox.Show("Export successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Something went wrong!\n" + ex.Message, "Error master", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
            }

        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            con.con.Close();
            dgvTTL.Rows.Clear();
            Cursor = Cursors.WaitCursor;
            DataTable cond = new DataTable();
            DataTable dtselect = new DataTable();
            DataTable dtmaster = new DataTable();
            DataTable dtbalance = new DataTable();
            cond.Columns.Add("Value");
            if (chkRcode.Checked == true)
            {
                string val = txtRcode.Text;
                if (val.Trim() != "")
                {
                    cond.Rows.Add("tbTr.Code LIKE '%" + val + "%'");
                }
            }
            if (chkRname.Checked == true)
            {
                string val = txtRname.Text;
                if (val.Trim() != "")
                {
                    cond.Rows.Add("tbMst.Part_Name LIKE '%" + val + "%'");
                }
            }
            string Conds = "";
            foreach (DataRow row in cond.Rows)
            {
                if (Conds.Trim() == "")
                {
                    Conds = " AND " + row["Value"];
                }
                else
                {
                    Conds += " AND " + row["Value"];
                }
            }
            try
            {
                con.con.Open();
                //new stock 
                int year = dtpDate.Value.Year;
                int month = dtpDate.Value.Month;
                int day = DateTime.DaysInMonth(year, month);
                DateTime firstDay = new DateTime(year, month, 1);
                DateTime lastDay = new DateTime(year, month, day);

                //for pre stockint 
                DateTime prevMonthDate = dtpDate.Value.AddMonths(-1);
                int prevYear = prevMonthDate.Year;
                int prevMonth = prevMonthDate.Month;
                int prevDay = DateTime.DaysInMonth(prevYear, prevMonth);
                DateTime preStockLastDay = new DateTime(prevYear, prevMonth, prevDay);
                dtpDate.Value = lastDay;

                string query = "SELECT tbTr.Code, tbMSt.Supplier, tbMst.Part_No, tbMst.Part_Name, tbPre.QtyIn, tbPre.QtyOut , tbPreS.PreQty " +
                    "FROM SparePartTrans tbTr " +
                    "LEFT JOIN  " +
                    "(SELECT Code,Supplier,Part_Name,Part_No FROM MstMCSparePart) tbMst ON tbMst.Code = tbTr.Code " +
                    "LEFT JOIN " +
                    "(SELECT Code, SUM(Stock_In) AS QtyIn,SUM(Stock_Out) AS QtyOut FROM SparePartTrans " +
                    "WHERE RegDate BETWEEN '" + firstDay + "' AND '" + lastDay + "' AND Dept ='" + dept + "' GROUP BY Code ) tbPre ON tbPre.Code = tbTr.Code " +
                    "LEFT JOIN (SELECT Code, SUM(Stock_Value) AS PreQty FROM SparePartTrans " +
                    "WHERE CAST(RegDate AS date) <= '" + preStockLastDay + "' AND Dept ='MC' GROUP BY Code ) tbPreS ON tbPreS.Code = tbPre.Code " +
                    "WHERE CAST(tbTr.RegDate AS DATE) > '" + preStockLastDay + "' AND tbtr.Dept ='" + dept + "' " + Conds + " " +
                    "GROUP BY tbTr.Code, tbMst.Supplier,tbMst.Part_No, tbMst.Part_Name,tbPre.QtyIn,tbPre.QtyOut, tbPreS.PreQty " +
                    "Order by tbTr.Code";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dtselect);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while selecting data !" + ex.Message, "Error tbTrans.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.con.Close(); 
            try
            {
                con.con.Open();
                string querymaster = "SELECT * FROM MstMCSparePart WHERE Dept = '" + dept + "'";
                SqlDataAdapter sda = new SqlDataAdapter(querymaster, con.con);
                sda.Fill(dtmaster);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while selecting master ! " + ex.Message, "Error master", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.con.Close();
            List<string> codellist = new List<string>();
            foreach (DataGridViewRow row in dgvTTL.Rows)
            {
                codellist.Add(row.Cells["code"].Value.ToString());
            }
            string codelist = "('" + string.Join("','", codellist) + "')";
            try
            {
                con.con.Open();
                string querybalance = "SELECT Code, Balance, Receive_Date FROM MCSparePartRequest WHERE Code IN "+codelist+"";
                SqlDataAdapter sda = new SqlDataAdapter (querybalance, con.con);
                sda.Fill(dtbalance);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while select balance !" + ex.Message, "Error balance", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
            con.con.Close();
            if (dtmaster.Rows.Count > 0 && dtselect.Rows.Count > 0)
            {
                if (dtselect.Rows.Count > 0)
                {
                    foreach (DataRow row in dtselect.Rows)
                    {
                        string code = row["Code"]?.ToString() ?? "";
                        string partno = row["Part_No"]?.ToString() ?? "";
                        string partname = row["Part_Name"]?.ToString() ?? "";
                        string supplier = row["Supplier"]?.ToString() ?? "";
                        double prestock = double.TryParse(row["PreQty"]?.ToString(), out var ps) ? ps : 0;
                        double stockout = double.TryParse(row["QtyOut"]?.ToString(), out var so) ? so : 0;
                        double stockin = double.TryParse(row["QtyIn"]?.ToString(), out var si) ? si : 0;
                        double stockremain = prestock + stockin + stockout;
                        dgvTTL.Rows.Add();
                        dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["code"].Value = code;
                        dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["partno"].Value = partno;
                        dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["partname"].Value = partname;
                        dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["supplier"].Value = supplier;
                        dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["prestock"].Value = prestock;
                        dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["stockin"].Value = stockin;
                        dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["stockout"].Value = stockout;
                        dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["stockremain"].Value = stockremain;
                    }
                }
                if (dtmaster.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row1 in dgvTTL.Rows)
                    {
                        string code1 = row1.Cells["code"].Value.ToString();
                        foreach (DataRow row in dtmaster.Rows)
                        {
                            string code = row["Code"]?.ToString() ?? "";
                            if (code1 == code)
                            {
                                string box = row["Box"]?.ToString() ?? "";
                                int weeks = row["Lead_Time_Week"] is DBNull ? 0 : Convert.ToInt32(row["Lead_Time_Week"]);
                                int days = weeks * 7; // exact conversion
                                DateTime ? eta = null;
                                int safety = row["Safety_Stock"] is DBNull ? 0 : Convert.ToInt32(row["Safety_Stock"]);

                                double stockremain = Convert.ToDouble(row1.Cells["stockremain"].Value);
                                double qty = stockremain - safety;
                                string order = "";
                                if (qty < 0)
                                {
                                    order = "" + qty * (-1) + "";
                                    eta = DateTime.Now.AddDays(days);
                                   
                                }
                                row1.Cells["box"].Value = box;
                                row1.Cells["leadtime"].Value = weeks;
                                row1.Cells["eta"].Value = eta;
                                row1.Cells["safetystock"].Value = safety;
                                row1.Cells["orderqty"].Value = order;
                                double status = double.TryParse(order, out var v) ? v : 0;
                                if (status > 0)
                                {
                                    row1.Cells["status"].Value = "Order";
                                    row1.Cells["status"].Style.BackColor = Color.LightPink;

                                }
                                else
                                {
                                    row1.Cells["status"].Value = "No Order";
                                    row1.Cells["status"].Style.BackColor = Color.LightGreen;
                                }
                            }

                        }
                    }
                }
                if (dtbalance.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row3 in dgvTTL.Rows)
                    {
                        string code3 = row3.Cells["code"].Value.ToString();
                        double orderqty = 0;
                        var rowqty = row3.Cells["orderqty"].Value.ToString();
                        if (!string.IsNullOrEmpty(rowqty))
                        {
                            orderqty = Convert.ToDouble(row3.Cells["orderqty"].Value);
                        }
                            foreach (DataRow row4 in dtbalance.Rows)
                            {
                                string code4 = row4["Code"].ToString();
                                if (code3 == code4 && orderqty > 0)
                                {
                                    row3.Cells["balanceorder"].Value = row4["Balance"].ToString();
                                    row3.Cells["receivedate"].Value = Convert.ToDateTime(row4["Receive_Date"].ToString());
                                }
                            }
                    }

                }

            }
            lbFound.Text = "Found : " + dgvTTL.Rows.Count.ToString();
            con.con.Close();
            Cursor = Cursors.Default;
        }
        //date
        private void DtpDate_ValueChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }
        //form
        private void Balance_Load(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }
        //txt
        private void TxtRname_TextChanged(object sender, EventArgs e)
        {
            

            if (txtRname.Text.Trim() == "")
            {
                btnSearch.PerformClick();
                chkRname.Checked = false;
            }
            else
            {
                chkRname.Checked = true;
                btnSearch.PerformClick();
            }
           
        }
        private void TxtRcode_TextChanged(object sender, EventArgs e)
        {
          
            if (txtRcode.Text.Trim() == "")
            {
                chkRcode.Checked = false;
                btnSearch.PerformClick();
            }
            else
            {
                chkRcode.Checked = true;
                btnSearch.PerformClick();
            }
        }

    }
}
