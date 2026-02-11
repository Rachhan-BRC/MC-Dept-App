using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management.Instrumentation;
using System.Runtime.CompilerServices;
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
            this.cbstatus.TextChanged += Cbstatus_TextChanged;

            //form
            this.Shown += Balance_Shown;

        }

        private void Cbstatus_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbstatus.Text.Trim()))
            {
                chkstatus.Checked = true;
            }
            else
            {
                chkstatus.Checked = false;
            }
        }

        private void Balance_Shown(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }


        //btn
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
                    worksheet.Cells[startRow + i, 15] = row.Cells["balanceorder"].Value?.ToString();
                    worksheet.Cells[startRow + i, 16] = row.Cells["planeta"].Value?.ToString();

                }
                // Save Excel

                string DateExcel = DateTime.Now.ToString("yyMMdd");
                string fileName = "Spare - Part Balance" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".xlsx";

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
            dgvTTL.Rows.Clear();
            Cursor = Cursors.WaitCursor;
            DataTable cond = new DataTable();
            DataTable dtselect = new DataTable();
            cond.Columns.Add("Value");
            if (chkRcode.Checked == true)
            {
                string val = txtRcode.Text;
                if (val.Trim() != "")
                {
                    cond.Rows.Add("tbMst.Code LIKE '%" + val + "%'");
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
            if (chkstatus.Checked == true)
            {
                string val = cbstatus.Text;
                if (val.Trim() == "Order")
                {
                    cond.Rows.Add("(tbTran.QtyIn - tbTran.QtyOut - tbMst.Safety_Stock) < 0");
                }
                else if (val.Trim() == "No Order")
                {
                    cond.Rows.Add("(tbTran.QtyIn - tbTran.QtyOut - tbMst.Safety_Stock) > 0");
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
                DateTime firstDay = new DateTime(year, month,1);
                DateTime lastDay = new DateTime(year, month, day);

                //for pre stockint 
                DateTime prevMonthDate = dtpDate.Value.AddMonths(-1);
                int prevYear = prevMonthDate.Year;
                int prevMonth = prevMonthDate.Month;
                int prevDay = DateTime.DaysInMonth(prevYear, prevMonth);
                DateTime preStockLastDay = new DateTime(prevYear, prevMonth, prevDay);
               

                string query = @"SELECT tbMst.Code, tbMst.Supplier, tbMst.Part_No AS PartNo, tbMst.Part_Name AS PartName, tbPre.PreQty, tbTran.QtyIn, tbTran.QtyOut, 
                                        (tbTran.QtyIn - tbTran.QtyOut) AS StockRemain, tbMst.Safety_Stock AS Safety,tbMst.Lead_Time_Week AS LT,tbMst.Box, tbReq.Balance, 
                                        (tbTran.QtyIn - tbTran.QtyOut - tbMst.Safety_Stock) AS Status,tbReq.ETA AS PlanEta FROM MstMCSparePart tbMst

                                        LEFT JOIN 
                                        (SELECT Code, SUM(Stock_In) AS QtyIn,SUM(Stock_Out) AS QtyOut FROM SparePartTrans WHERE CAST (RegDate AS date) <= @firstDay AND Dept ='MC' GROUP BY Code )
                                        tbTran ON tbMst.Code = tbTran.Code
                                        LEFT JOIN
                                        (SELECT Code, SUM(Stock_Value) AS PreQty FROM SparePartTrans WHERE CAST(RegDate AS date) <= @preStockLastDay AND Dept ='MC' GROUP BY Code ) 
                                        tbPre ON tbMst.Code  = tbPre.Code
                                        LEFT JOIN 
                                        (SELECT Code, Balance, ETA FROM MCSparePartRequest WHERE Balance > 0 AND Dept = 'MC' ) tbReq ON tbMst.Code = tbReq.Code  WHERE Dept = 'MC' AND Status = 'Active'
                                       " + Conds+
                                        "GROUP BY tbMst.Code, tbMst.Supplier, tbMst.Part_No, tbMst.Part_Name,tbTran.QtyIn,tbTran.QtyOut, tbPre.PreQty, tbMst.Box, tbMst.Safety_Stock, tbMst.Lead_Time_Week, tbReq.Balance, tbReq.ETA Order by tbMst.Code";
                using (SqlCommand cmd = new SqlCommand(query, con.con)) 
                {
                    cmd.Parameters.AddWithValue("@preStockLastDay", preStockLastDay); 
                    cmd.Parameters.AddWithValue("@firstDay", lastDay); 
                    SqlDataAdapter sda = new SqlDataAdapter(cmd); 
                    sda.Fill(dtselect); 
                }
                Console.WriteLine(query);
                Console.WriteLine(firstDay);
                Console.WriteLine(preStockLastDay);
                dtpDate.Value = lastDay;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while selecting data !" + ex.Message, "Error tbTrans.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.con.Close();
            foreach (DataRow row in dtselect.Rows)
            {
                string code = row["Code"]?.ToString() ?? "";
                string partno = row["PartNo"]?.ToString() ?? "";
                string supplier = row["Supplier"]?.ToString() ?? "";
                string partName = row["PartName"]?.ToString() ?? "";
                double qtyIn = double.TryParse(row["QtyIn"]?.ToString(), out var In) ? In : 0;
                double qtyOut = double.TryParse(row["QtyOut"]?.ToString(), out var Out) ? Out : 0;
                int preQty = int.TryParse(row["PreQty"]?.ToString(), out var pre) ? pre : 0;
                double remain = double.TryParse(row["StockRemain"]?.ToString(), out var re) ? re : 0;
                int safety = int.TryParse(row["Safety"]?.ToString(), out var safe) ? safe : 0;
                int balance = int.TryParse(row["Balance"]?.ToString(), out var bal) ? bal : 0;
                int LT = int.TryParse(row["LT"]?.ToString(), out var val) ? val : 0;
                string box = row["Box"]?.ToString() ?? "";
                double statval = Convert.ToDouble(qtyIn - qtyOut - safety);
                DateTime? planeta = row["PlanEta"] == DBNull.Value
    ? null
    : (DateTime?)row["PlanEta"];


                dgvTTL.Rows.Add();
                dgvTTL.Rows[dgvTTL.Rows.Count -1].Cells["code"].Value = code;
                dgvTTL.Rows[dgvTTL.Rows.Count -1].Cells["partno"].Value = partno;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["partname"].Value = partName;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["supplier"].Value = supplier;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["stockin"].Value = qtyIn;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["stockout"].Value =qtyOut;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["prestock"].Value = preQty;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["stockremain"].Value =remain;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["safetystock"].Value = safety;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["Leadtime"].Value = LT;
                int lead = (LT / 4);
                DateTime eta = DateTime.Now.AddMonths(lead);
                
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["box"].Value = box;
               
                if (statval >= 0)
                {
                  
                    dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["status"].Value = "No Order";
                    dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["status"].Style.BackColor = Color.LightGreen;
                }
                else
                {
                    dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["eta"].Value = eta;
                    dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["orderqty"].Value = statval * (-1);
                    dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["status"].Value = "Order";
                    dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["status"].Style.BackColor = Color.LightPink;
                    if (balance > 0)
                    {
                        dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["balanceorder"].Value = balance;
                    }
                    dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["planeta"].Value = planeta;
                }
               
               


                // do something with these values
            }

            dgvTTL.Columns["Code"].Frozen = true;
            dgvTTL.Columns["partno"].Frozen = true;
            dgvTTL.Columns["partname"].Frozen = true;
            dgvTTL.Columns["supplier"].Frozen = true;
            lbFound.Text = "Found : " + dgvTTL.Rows.Count.ToString();
            con.con.Close();
            Cursor = Cursors.Default;
        }
       
        //txt
        private void TxtRname_TextChanged(object sender, EventArgs e)
        {
            if (txtRname.Text.Trim() == "")
            {
                chkRname.Checked = false;
            }
            else
            {
                chkRname.Checked = true;
            }
           
        }
        private void TxtRcode_TextChanged(object sender, EventArgs e)
        {
          
            if (txtRcode.Text.Trim() == "")
            {
                chkRcode.Checked = false;
            }
            else
            {
                chkRcode.Checked = true;
            }
        }

    }
}
