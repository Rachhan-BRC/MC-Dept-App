using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Animation;
using  Excel = Microsoft.Office.Interop.Excel;
namespace MachineDeptApp
{
    public partial class NGCalculateForm : Form
    {
        SQLConnectOBS OBS = new SQLConnectOBS();
        public NGCalculateForm()
        {
            InitializeComponent();
            this.OBS.Connection();
            this.Shown += NGCalculateForm_Shown;
            this.btnBrowseAddjust.Click += BtnBrowseAddjust_Click;
            this.txtadd.Click += BtnBrowseAddjust_Click;
            this.btnBrowseSearch.Click += BtnBrowseSearch_Click;
            this.btnNew.Click += BtnNew_Click;
            this.btnPrint.Click += BtnPrint_Click;
            this.btnExport.Click += BtnExport_Click;
           
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvNGCal.Rows.Count > 0 && dgvNGCal.Rows.Count > 0)
            {
                var choice = CustomMessageBox.ShowDialog("សូមជ្រើសរើសប្រភេទ :", "Data Export");

                if (choice == CustomMessageBox.CustomDialogResult.DataResult)
                {
                     if (dgvResult.Rows.Count > 0)
                    {
                        SaveFileDialog saveDialog = new SaveFileDialog();
                        saveDialog.Filter = "CSV file (*.csv)|*.csv";
                        saveDialog.FileName = "DataResult" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                        if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            Cursor = Cursors.WaitCursor;
                            try
                            {
                                //Write Column name
                                int columnCount = 1;
                                foreach (DataGridViewColumn DgvCol in dgvResult.Columns)
                                {
                                    if (DgvCol.Visible == true)
                                    {
                                        columnCount = columnCount + 1;
                                    }
                                }
                                string columnNames = "";

                                //String array for Csv
                                string[] outputCsv;
                                outputCsv = new string[dgvResult.Rows.Count + 1];

                                //Set Column Name
                                for (int i = 0; i < columnCount; i++)
                                {
                                    if (dgvResult.Columns[i].Visible == true)
                                    {
                                        columnNames += dgvResult.Columns[i].HeaderText.ToString() + ",";
                                    }
                                }
                                outputCsv[0] += columnNames;

                                //Row of data 
                                for (int i = 1; (i - 1) < dgvResult.Rows.Count; i++)
                                {
                                    for (int j = 0; j < columnCount; j++)
                                    {
                                        if (dgvResult.Columns[j].Visible == true)
                                        {
                                            string Value = "";
                                            if (dgvResult.Rows[i - 1].Cells[j].Value != null)
                                            {
                                                Value = dgvResult.Rows[i - 1].Cells[j].Value.ToString();
                                            }
                                            //Fix don't separate if it contain '\n' or ','
                                            Value = "\"" + Value.Replace("\"", "\"\"") + "\"";
                                            outputCsv[i] += Value + ",";
                                        }
                                    }
                                }

                                File.WriteAllLines(saveDialog.FileName, outputCsv, Encoding.UTF8);
                                Cursor = Cursors.Default;
                                MessageBox.Show("Export successfully !", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                Cursor = Cursors.Default;
                                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Error Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else if (choice == CustomMessageBox.CustomDialogResult.DataImport)
                {
                    if (dgvNGCal.Rows.Count > 0)
                    {
                        SaveFileDialog saveDialog = new SaveFileDialog();
                        saveDialog.Filter = "CSV file (*.csv)|*.csv";
                        saveDialog.FileName = "DataImport" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                        if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            Cursor = Cursors.WaitCursor;
                            try
                            {
                                //Write Column name
                                int columnCount = 1;
                                foreach (DataGridViewColumn DgvCol in dgvNGCal.Columns)
                                {
                                    if (DgvCol.Visible == true)
                                    {
                                        columnCount = columnCount + 1;
                                    }
                                }
                                string columnNames = "";

                                //String array for Csv
                                string[] outputCsv;
                                outputCsv = new string[dgvNGCal.Rows.Count + 1];

                                //Set Column Name
                                for (int i = 0; i < columnCount; i++)
                                {
                                    if (dgvNGCal.Columns[i].Visible == true)
                                    {
                                        columnNames += dgvNGCal.Columns[i].HeaderText.ToString() + ",";
                                    }
                                }
                                outputCsv[0] += columnNames;

                                //Row of data 
                                for (int i = 1; (i - 1) < dgvNGCal.Rows.Count; i++)
                                {
                                    for (int j = 0; j < columnCount; j++)
                                    {
                                        if (dgvNGCal.Columns[j].Visible == true)
                                        {
                                            string Value = "";
                                            if (dgvNGCal.Rows[i - 1].Cells[j].Value != null)
                                            {
                                                Value = dgvNGCal.Rows[i - 1].Cells[j].Value.ToString();
                                            }
                                            //Fix don't separate if it contain '\n' or ','
                                            Value = "\"" + Value.Replace("\"", "\"\"") + "\"";
                                            outputCsv[i] += Value + ",";
                                        }
                                    }
                                }

                                File.WriteAllLines(saveDialog.FileName, outputCsv, Encoding.UTF8);
                                Cursor = Cursors.Default;
                                MessageBox.Show("Export successfully !", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                Cursor = Cursors.Default;
                                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Error Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
          }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            DialogResult ask = MessageBox.Show("តើអ្នកប្រាកដថាចង់ព្រីនដែរឬទេ ?", "បញ្ជាក់", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask == DialogResult.Yes)
            {

                string error = ""; 
                string SavePath1 = "";
                string SavePath2 = "";
                string SavePath3 = "";
                string SavePath4 = "";
                string fName1 = "";
                string fName2 = "";
                string fName3 = "";
                string fName4 = "";
                Cursor = Cursors.WaitCursor;
                //print obs countable
                try
                {

                    //taking OBS
                    DataTable dtOBS = new DataTable();
                    DataTable dtPtrint = new DataTable();
                    dtPtrint.Columns.Add("RMCode");
                    dtPtrint.Columns.Add("RMDes");
                    dtPtrint.Columns.Add("Maker");
                    dtPtrint.Columns.Add("Type");
                    dtPtrint.Columns.Add("Qty");
                    dtPtrint.Columns.Add("Up");
                    dtPtrint.Columns.Add("Amount");
                    dtPtrint.Columns.Add("MatCalcFlag");

                    try
                    {
                        string RMCodeIN = "";
                        foreach (DataGridViewRow row in dgvResult.Rows)
                        {
                            if (RMCodeIN.Contains(row.Cells["rmcode3"].Value.ToString()) == false)
                            {
                                if (RMCodeIN.Trim() == "")
                                {
                                    RMCodeIN = "'" + row.Cells["rmcode3"].Value.ToString() + "'";
                                }
                                else
                                {
                                    RMCodeIN += ", '" + row.Cells["rmcode3"].Value.ToString() + "'";
                                }
                            }
                        }
                        OBS.conOBS.Open();
                        string query = "SELECT T1.ItemCode, MatCalcFlag, MatTypeName FROM " +
                            "(SELECT ItemCode, MatCalcFlag, MatTypeCode FROM mstitem WHERE DelFlag=0 AND ItemType=2) T1 " +
                            "LEFT JOIN (SELECT ItemCode, EffDate FROM mstpurchaseprice WHERE DelFlag=0) T2 " +
                            "ON T1.ItemCode=T2.ItemCode \r\nINNER JOIN (SELECT ItemCode, MAX(EffDate) AS EffDate FROM mstpurchaseprice WHERE DelFlag=0 GROUP BY ItemCode) T3 " +
                            "ON T2.ItemCode=T3.ItemCode AND T2.EffDate=T3.EffDate " +
                            "LEFT JOIN (SELECT * FROM MstMatType WHERE DelFlag = 0) T4 ON T1.MatTypeCode=T4.MatTypeCode " +
                            "WHERE T1.ItemCode IN (" + RMCodeIN + ") ORDER BY ItemCode ASC ";
                        SqlDataAdapter sda = new SqlDataAdapter(query, OBS.conOBS);
                        sda.Fill(dtOBS);
                        OBS.conOBS.Close();
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                        MessageBox.Show("Error taking OBS " + ex.Message, "Error Adjust", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    //combine
                    foreach (DataGridViewRow row1 in dgvResult.Rows)
                    {
                        string code1 = row1.Cells["rmcode3"].Value.ToString();
                        double qty3 = Convert.ToDouble(row1.Cells["qty3"].Value);
                        string rmdes = row1.Cells["rmdes3"].Value.ToString();
                        string maker = row1.Cells["maker3"].Value.ToString();
                        string type = row1.Cells["type3"].Value.ToString();
                        double up3 = Convert.ToDouble(row1.Cells["up3"].Value);
                        double amount3 = Convert.ToDouble(row1.Cells["amount3"].Value);
                        if (qty3 > 0)
                        {
                            foreach (DataRow row2 in dtOBS.Rows)
                            {
                                string code2 = row2["ItemCode"].ToString();
                                string matflag = row2["MatCalcFlag"].ToString();
                                if (code1 == code2)
                                {

                                    dtPtrint.Rows.Add(code1, rmdes, maker, type, qty3, up3, amount3, matflag);
                                }
                            }
                        }

                    }

                    //print to excel count
                    try
                    {

                        Excel.Application excelApp = new Excel.Application();
                        Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(
                            Path.Combine(Environment.CurrentDirectory, @"Template\ReciveIssueImport.xlsx"), Editable: true);

                         SavePath1= Path.Combine(Environment.CurrentDirectory, @"Report\NGCalculate");
                        if (!Directory.Exists(SavePath1))
                        {
                            Directory.CreateDirectory(SavePath1);
                        }
                        //add to count sheet
                        try
                        {
                            Excel.Worksheet worksheetCountable = (Excel.Worksheet)xlWorkBook.Sheets["Import"];
                            int startrow = 2;
                            foreach (DataRow row in dtPtrint.Rows)
                            {
                                double matflag = Convert.ToDouble(row["MatCalcFlag"]);

                                if (matflag == 0)
                                {
                                    worksheetCountable.Cells[startrow, 1] = DateTime.Now.ToString("yyyyMMdd");
                                    worksheetCountable.Cells[startrow, 2] = 2;
                                    worksheetCountable.Cells[startrow, 3] = "MC1";
                                    worksheetCountable.Cells[startrow, 4] = row["RMCode"].ToString();
                                    worksheetCountable.Cells[startrow, 6] = row["Qty"];
                                    startrow = startrow + 1;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            error = ex.Message;
                            MessageBox.Show("Error while fill print count" + ex.Message, "Errorwhile fill print Count", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        fName1 = "NG_Import_OBS(UnCountable)" + (DateTime.Now.ToString(" dd-MM-yyyy hh_mm_ss ")) + "";
                        xlWorkBook.SaveAs(SavePath1 + @"\" +fName1 + ".xlsx");
                        
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
                       
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                        MessageBox.Show("Error print Search Result Count" + ex.Message, "Error Search Count", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    MessageBox.Show("Error print Search Result" + ex.Message, "Error Adjust", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //print obs Uncountable
                try
                {
                    //taking OBS
                    DataTable dtOBS = new DataTable();
                    DataTable dtPtrint = new DataTable();
                    dtPtrint.Columns.Add("RMCode");
                    dtPtrint.Columns.Add("RMDes");
                    dtPtrint.Columns.Add("Maker");
                    dtPtrint.Columns.Add("Type");
                    dtPtrint.Columns.Add("Qty");
                    dtPtrint.Columns.Add("Up");
                    dtPtrint.Columns.Add("Amount");
                    dtPtrint.Columns.Add("MatCalcFlag");

                    try
                    {
                        string RMCodeIN = "";
                        foreach (DataGridViewRow row in dgvResult.Rows)
                        {
                            if (RMCodeIN.Contains(row.Cells["rmcode3"].Value.ToString()) == false)
                            {
                                if (RMCodeIN.Trim() == "")
                                {
                                    RMCodeIN = "'" + row.Cells["rmcode3"].Value.ToString() + "'";
                                }
                                else
                                {
                                    RMCodeIN += ", '" + row.Cells["rmcode3"].Value.ToString() + "'";
                                }
                            }
                        }
                        OBS.conOBS.Open();
                        string query = "SELECT T1.ItemCode, MatCalcFlag, MatTypeName FROM " +
                            "(SELECT ItemCode, MatCalcFlag, MatTypeCode FROM mstitem WHERE DelFlag=0 AND ItemType=2) T1 " +
                            "LEFT JOIN (SELECT ItemCode, EffDate FROM mstpurchaseprice WHERE DelFlag=0) T2 " +
                            "ON T1.ItemCode=T2.ItemCode \r\nINNER JOIN (SELECT ItemCode, MAX(EffDate) AS EffDate FROM mstpurchaseprice WHERE DelFlag=0 GROUP BY ItemCode) T3 " +
                            "ON T2.ItemCode=T3.ItemCode AND T2.EffDate=T3.EffDate " +
                            "LEFT JOIN (SELECT * FROM MstMatType WHERE DelFlag = 0) T4 ON T1.MatTypeCode=T4.MatTypeCode " +
                            "WHERE T1.ItemCode IN (" + RMCodeIN + ") ORDER BY ItemCode ASC ";
                        SqlDataAdapter sda = new SqlDataAdapter(query, OBS.conOBS);
                        sda.Fill(dtOBS);
                        OBS.conOBS.Close();
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                        MessageBox.Show("Error taking OBS " + ex.Message, "Error Adjust", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    //combine
                    foreach (DataGridViewRow row1 in dgvResult.Rows)
                    {
                        string code1 = row1.Cells["rmcode3"].Value.ToString();
                        double qty3 = Convert.ToDouble(row1.Cells["qty3"].Value);
                        string rmdes = row1.Cells["rmdes3"].Value.ToString();
                        string maker = row1.Cells["maker3"].Value.ToString();
                        string type = row1.Cells["type3"].Value.ToString();
                        double up3 = Convert.ToDouble(row1.Cells["up3"].Value);
                        double amount3 = Convert.ToDouble(row1.Cells["amount3"].Value);
                        if (qty3 > 0)
                        {
                            foreach (DataRow row2 in dtOBS.Rows)
                            {
                                string code2 = row2["ItemCode"].ToString();
                                string matflag = row2["MatCalcFlag"].ToString();
                                if (code1 == code2)
                                {

                                    dtPtrint.Rows.Add(code1, rmdes, maker, type, qty3, up3, amount3, matflag);
                                }
                            }
                        }

                    }

                    //print to excel Uncount
                    try
                    {

                        Excel.Application excelApp = new Excel.Application();
                        Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(
                            Path.Combine(Environment.CurrentDirectory, @"Template\ReciveIssueImport.xlsx"), Editable: true);

                         SavePath2 = Path.Combine(Environment.CurrentDirectory, @"Report\NGCalculate");
                        if (!Directory.Exists(SavePath2))
                        {
                            Directory.CreateDirectory(SavePath2);
                        }
                        //add to uncount sheet
                        try
                        {
                            Excel.Worksheet worksheetCountable = (Excel.Worksheet)xlWorkBook.Sheets["Import"];
                            int startrow = 2;
                            foreach (DataRow row in dtPtrint.Rows)
                            {
                                double matflag = Convert.ToDouble(row["MatCalcFlag"]);

                                if (matflag != 0)
                                {
                                    worksheetCountable.Cells[startrow, 1] = DateTime.Now.ToString("yyyyMMdd");
                                    worksheetCountable.Cells[startrow, 2] = 2;
                                    worksheetCountable.Cells[startrow, 3] = "MC1";
                                    worksheetCountable.Cells[startrow, 4] = row["RMCode"].ToString();
                                    worksheetCountable.Cells[startrow, 6] = row["Qty"];
                                    startrow = startrow + 1;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            error = ex.Message;
                            MessageBox.Show("Error while fill print uncount" + ex.Message, "Error while fill print UnCount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        fName2 = "NG_Import_OBS(Countable)" + (DateTime.Now.ToString(" dd-MM-yyyy hh_mm_ss ")) + "";
                        xlWorkBook.SaveAs(SavePath2 + @"\" +fName2+ ".xlsx");
                      
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
                        
                       
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                        MessageBox.Show("Error print Search Result UnCount" + ex.Message, "Error Search UnCount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    MessageBox.Show("Error print Search Result" + ex.Message, "Error Adjust", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //print seearch 
                try
                {

                    //taking OBS
                    DataTable dtOBS = new DataTable();
                    DataTable dtPtrint = new DataTable();
                    dtPtrint.Columns.Add("RMCode");
                    dtPtrint.Columns.Add("RMDes");
                    dtPtrint.Columns.Add("Maker");
                    dtPtrint.Columns.Add("Type");
                    dtPtrint.Columns.Add("Qty");
                    dtPtrint.Columns.Add("Up");
                    dtPtrint.Columns.Add("Amount");
                    dtPtrint.Columns.Add("MatCalcFlag");

                    try
                    {
                        string RMCodeIN = "";
                        foreach (DataGridViewRow row in dgvResult.Rows)
                        {
                            if (RMCodeIN.Contains(row.Cells["rmcode3"].Value.ToString()) == false)
                            {
                                if (RMCodeIN.Trim() == "")
                                {
                                    RMCodeIN = "'" + row.Cells["rmcode3"].Value.ToString() + "'";
                                }
                                else
                                {
                                    RMCodeIN += ", '" + row.Cells["rmcode3"].Value.ToString() + "'";
                                }
                            }
                        }
                        OBS.conOBS.Open();
                        string query = "SELECT T1.ItemCode, MatCalcFlag, MatTypeName FROM " +
                            "(SELECT ItemCode, MatCalcFlag, MatTypeCode FROM mstitem WHERE DelFlag=0 AND ItemType=2) T1 " +
                            "LEFT JOIN (SELECT ItemCode, EffDate FROM mstpurchaseprice WHERE DelFlag=0) T2 " +
                            "ON T1.ItemCode=T2.ItemCode \r\nINNER JOIN (SELECT ItemCode, MAX(EffDate) AS EffDate FROM mstpurchaseprice WHERE DelFlag=0 GROUP BY ItemCode) T3 " +
                            "ON T2.ItemCode=T3.ItemCode AND T2.EffDate=T3.EffDate " +
                            "LEFT JOIN (SELECT * FROM MstMatType WHERE DelFlag = 0) T4 ON T1.MatTypeCode=T4.MatTypeCode " +
                            "WHERE T1.ItemCode IN (" + RMCodeIN + ") ORDER BY ItemCode ASC ";
                        SqlDataAdapter sda = new SqlDataAdapter(query, OBS.conOBS);
                        sda.Fill(dtOBS);
                        OBS.conOBS.Close();
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                        MessageBox.Show("Error taking OBS " + ex.Message, "Error Adjust", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    //combine
                    foreach (DataGridViewRow row1 in dgvResult.Rows)
                    {
                        string code1 = row1.Cells["rmcode3"].Value.ToString();
                        double qty3 = Convert.ToDouble(row1.Cells["qty3"].Value);
                        string rmdes = row1.Cells["rmdes3"].Value.ToString();
                        string maker = row1.Cells["maker3"].Value.ToString();
                        string type = row1.Cells["type3"].Value.ToString();
                        double up3 = Convert.ToDouble(row1.Cells["up3"].Value);
                        double amount3 = Convert.ToDouble(row1.Cells["amount3"].Value);
                        if (qty3 > 0)
                        {
                            foreach (DataRow row2 in dtOBS.Rows)
                            {
                                string code2 = row2["ItemCode"].ToString();
                                string matflag = row2["MatCalcFlag"].ToString();
                                if (code1 == code2)
                                {

                                    dtPtrint.Rows.Add(code1, rmdes, maker, type, qty3, up3, amount3, matflag);
                                }
                            }
                        }

                    }

                    //print to excel count
                    try
                    {

                        Excel.Application excelApp = new Excel.Application();
                        Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(
                            Path.Combine(Environment.CurrentDirectory, @"Template\NGReq_Template.xlsx"), Editable: true);

                         SavePath3 = Path.Combine(Environment.CurrentDirectory, @"Report\NGCalculate");
                        if (!Directory.Exists(SavePath3))
                        {
                            Directory.CreateDirectory(SavePath3);
                        }
                        //add to count sheet
                        try
                        {
                            Excel.Worksheet worksheetCountable = (Excel.Worksheet)xlWorkBook.Sheets["Countable"];
                            Excel.Range sourceRange = worksheetCountable.Range["A14:M14"];
                            worksheetCountable.Cells[1, 9] = Tranno.Text;
                            worksheetCountable.Cells[4, 10] = DateTime.Now.ToString("dd-MMM-yy");
                            worksheetCountable.Cells[5, 10] = MenuFormV2.UserForNextForm;
                            int copy = 0;
                            int startrow = 14;
                            foreach (DataRow row in dtPtrint.Rows)
                            {
                                double matflag = Convert.ToDouble(row["MatCalcFlag"]);

                                if (matflag == 0)
                                {
                                    if (copy > 0)
                                    {
                                        Excel.Range row1 = (Excel.Range)worksheetCountable.Rows[startrow];
                                        row1.Insert(Excel.XlInsertShiftDirection.xlShiftDown, sourceRange.Copy());
                                    }
                                    worksheetCountable.Cells[startrow, 4] = row["RMCode"].ToString();
                                    worksheetCountable.Cells[startrow, 5] = row["RMDes"];
                                    worksheetCountable.Cells[startrow, 7] = row["Maker"];
                                    worksheetCountable.Cells[startrow, 8] = row["Type"];
                                    worksheetCountable.Cells[startrow, 9] = row["Qty"];
                                    worksheetCountable.Cells[startrow, 10] = row["Up"];
                                    worksheetCountable.Cells[startrow, 11] = row["Amount"];
                                    startrow = startrow + 1;
                                    copy++;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            error = ex.Message;
                            MessageBox.Show("Error while fill print count" + ex.Message, "Errorwhile fill print Count", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        //add to uncount sheet
                        try
                        {
                            Excel.Worksheet worksheetUnCountable = (Excel.Worksheet)xlWorkBook.Sheets["Uncountable"];
                            Excel.Range sourceRange = worksheetUnCountable.Range["A14:M14"];
                            worksheetUnCountable.Cells[1, 9] = Tranno.Text;
                            worksheetUnCountable.Cells[4, 10] = DateTime.Now.ToString("dd-MMM-yy");
                            worksheetUnCountable.Cells[5, 10] = MenuFormV2.UserForNextForm;
                            int copy = 0;
                            int startrow = 14;
                            foreach (DataRow row in dtPtrint.Rows)
                            {
                                double matflag = Convert.ToDouble(row["MatCalcFlag"]);

                                if (matflag != 0)
                                {
                                    if (copy > 0)
                                    {
                                        Excel.Range row1 = (Excel.Range)worksheetUnCountable.Rows[startrow];
                                        row1.Insert(Excel.XlInsertShiftDirection.xlShiftDown, sourceRange.Copy());
                                    }
                                    worksheetUnCountable.Cells[startrow, 4] = row["RMCode"].ToString();
                                    worksheetUnCountable.Cells[startrow, 5] = row["RMDes"];
                                    worksheetUnCountable.Cells[startrow, 7] = row["Maker"];
                                    worksheetUnCountable.Cells[startrow, 8] = row["Type"];
                                    worksheetUnCountable.Cells[startrow, 9] = row["Qty"];
                                    worksheetUnCountable.Cells[startrow, 10] = row["Up"];
                                    worksheetUnCountable.Cells[startrow, 11] = row["Amount"];
                                    startrow = startrow + 1;
                                    copy++;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            error = ex.Message;
                            MessageBox.Show("Error while fill print count" + ex.Message, "Errorwhile fill print Count", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        fName3 = "NG_Req_Result" + (DateTime.Now.ToString(" dd-MM-yyyy hh_mm_ss ")) + "";
                        xlWorkBook.SaveAs(SavePath3 + @"\" +fName3+ ".xlsx");
                       
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
                        
                      
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                        MessageBox.Show("Error print Search Result Count" + ex.Message, "Error Search Count", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    MessageBox.Show("Error print Search Result" + ex.Message, "Error Adjust", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //print AdjustResult
                try
                {
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(
                        Path.Combine(Environment.CurrentDirectory, @"Template\NGAdjustResult.xlsx"), Editable: true);
                    Excel.Worksheet worksheetCountable = (Excel.Worksheet)xlWorkBook.Sheets["Rachhan System"];
                    Excel.Range sourceRange = worksheetCountable.Range["A14:M14"];
                    try
                    {
                         SavePath4 = Path.Combine(Environment.CurrentDirectory, @"Report\NGCalculate");
                        // Create folder if not exists
                        if (!Directory.Exists(SavePath4))
                        {
                            Directory.CreateDirectory(SavePath4);
                        }
                        int startrow = 14;
                        worksheetCountable.Cells[4, 10] = DateTime.Now.ToString("dd-MMM-yy");
                        worksheetCountable.Cells[5, 10] = MenuFormV2.UserForNextForm;
                        int copy = 0;
                        foreach (DataGridViewRow row in dgvResult.Rows)
                        {
                            double qty4 = Convert.ToDouble(row.Cells["qty4"].Value);
                            if (qty4 > 0)
                            {
                                if (copy > 0)
                                {
                                    Excel.Range row1 = (Excel.Range)worksheetCountable.Rows[startrow];
                                    row1.Insert(Excel.XlInsertShiftDirection.xlShiftDown, sourceRange.Copy());
                                }
                                worksheetCountable.Cells[startrow, 4] = row.Cells["rmcode3"].Value?.ToString();
                                worksheetCountable.Cells[startrow, 5] = row.Cells["rmdes3"].Value?.ToString();
                                worksheetCountable.Cells[startrow, 7] = row.Cells["maker3"].Value?.ToString();
                                worksheetCountable.Cells[startrow, 8] = row.Cells["type3"].Value?.ToString();
                                worksheetCountable.Cells[startrow, 9] = row.Cells["qty4"].Value?.ToString();
                                worksheetCountable.Cells[startrow, 10] = row.Cells["up4"].Value?.ToString();
                                worksheetCountable.Cells[startrow, 11] = row.Cells["amount4"].Value?.ToString();
                                startrow = startrow + 1;
                                copy++;
                            }
                        }
                         fName4 = "NGAdjustResult" + (DateTime.Now.ToString(" dd-MM-yyyy hh_mm_ss ")) + "";
                        worksheetCountable.SaveAs(SavePath4 + @"\" +fName4 + ".xlsx");
                       
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
                       
                     
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
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
                        error = ex.Message;
                    }

                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    MessageBox.Show("Error print Adjust Result" + ex.Message, "Error Adjust", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                if (error == "")
                {
                    btnPrint.Enabled = false;
                    btnPrintGrey.BringToFront();
                    btnExport.Enabled = false;
                    btnExportGrey.BringToFront();
                    dgvNGCal.Rows.Clear();
                    dgvResult.Rows.Clear();
                    txtadd.Text = "'";
                    txtsearch.Text = "'";
                    MessageBox.Show("Print Sucessfully !", "Sucess.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(SavePath1 + @"\" + fName1 + ".xlsx");
                    System.Diagnostics.Process.Start(SavePath2 + @"\" + fName2 + ".xlsx");
                    System.Diagnostics.Process.Start(SavePath3 + @"\" + fName3 + ".xlsx");
                    System.Diagnostics.Process.Start(SavePath4 + @"\" + fName4 + ".xlsx");
                }
                Cursor = Cursors.Default;
            }
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            if (dgvNGCal.Rows.Count > 0 || dgvResult.Rows.Count > 0)
            {
                DialogResult ask = MessageBox.Show("តើអ្នកមិនចង់ព្រីនសិនទេ? សម្អាតទិន្ន័យទាំងអស់ !", "Clear Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask == DialogResult.Yes)
                {

                    dgvNGCal.Rows.Clear();
                    dgvResult.Rows.Clear();
                    txtsearch.Text = "'";
                    txtadd.Text = "'";
                    lbfound.Text = "Found : " + dgvNGCal.Rows.Count.ToString();
                    btnPrint.Enabled = false;
                    btnPrint.SendToBack();
                }
            }
        }
        private void BtnBrowseSearch_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFD = new OpenFileDialog
            {
                Title = "Choose an Excel file for import",
                Filter = "Excel Files|*.xlsx"
            };
            if (openFD.ShowDialog()!= DialogResult.OK) return ;
            Cursor = Cursors.WaitCursor;
            string strFileName = openFD.FileName;
            string fileNameOnly = Path.GetFileName(strFileName);
            txtsearch.Text = fileNameOnly;
            Microsoft.Office.Interop.Excel.Application xlApp = null;
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = null;
            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = null;
            Microsoft.Office.Interop.Excel.Range xlRange = null;

            try
            {
                int count = dgvNGCal.Rows.Count;
                Cursor = Cursors.WaitCursor;
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkbook = xlApp.Workbooks.Open(strFileName);

                //UnCount
                xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets["Uncountable"];
                xlRange = xlWorksheet.UsedRange;

                int rowCount2 = xlRange.Rows.Count;
                dgvNGCal.Rows.Clear();
                string tranno1 = Convert.ToString((xlRange.Cells[1, 9] as Microsoft.Office.Interop.Excel.Range)?.Text);
                Tranno.Text = tranno1;
                for (int row = 14; row <= rowCount2; row++)
                {
                    string rmcode = Convert.ToString((xlRange.Cells[row, 4] as Microsoft.Office.Interop.Excel.Range)?.Text);
                    string rmdes = Convert.ToString((xlRange.Cells[row, 5] as Microsoft.Office.Interop.Excel.Range)?.Text);
                    string maker = Convert.ToString((xlRange.Cells[row, 7] as Microsoft.Office.Interop.Excel.Range)?.Text);
                    string type = Convert.ToString((xlRange.Cells[row, 8] as Microsoft.Office.Interop.Excel.Range)?.Text);
                    string qty = Convert.ToString((xlRange.Cells[row, 9] as Microsoft.Office.Interop.Excel.Range)?.Text);
                    string up = Convert.ToString((xlRange.Cells[row, 10] as Microsoft.Office.Interop.Excel.Range)?.Text);
                    string amount = Convert.ToString((xlRange.Cells[row, 11] as Microsoft.Office.Interop.Excel.Range)?.Text);


                    if (!string.IsNullOrEmpty(rmcode) &&
                        !string.IsNullOrEmpty(rmdes) &&
                        !string.IsNullOrEmpty(qty) &&
                        !string.IsNullOrEmpty(maker) &&
                        !string.IsNullOrEmpty(type) &&
                           !string.IsNullOrEmpty(up) &&
                              !string.IsNullOrEmpty(amount)

                        )
                    {
                            dgvNGCal.Rows.Add();
                            dgvNGCal.Rows[dgvNGCal.Rows.Count - 1].HeaderCell.Value = dgvNGCal.Rows.Count.ToString();
                            dgvNGCal.Rows[dgvNGCal.Rows.Count - 1].Cells["rmcode"].Value = rmcode;
                            dgvNGCal.Rows[dgvNGCal.Rows.Count - 1].Cells["rmdes"].Value = rmdes;
                            dgvNGCal.Rows[dgvNGCal.Rows.Count - 1].Cells["maker"].Value = maker;
                            dgvNGCal.Rows[dgvNGCal.Rows.Count - 1].Cells["type"].Value = rmdes = type;
                            dgvNGCal.Rows[dgvNGCal.Rows.Count - 1].Cells["qty"].Value = Convert.ToDouble(qty);
                            dgvNGCal.Rows[dgvNGCal.Rows.Count - 1].Cells["up"].Value = Convert.ToDouble(up);
                            dgvNGCal.Rows[dgvNGCal.Rows.Count - 1].Cells["amount"].Value = amount;
                        lbfound.Text = "Found : " + dgvNGCal.Rows.Count.ToString();
                    }

                }

                //Count
                xlWorksheet = null;
                xlRange = null;
                xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets["Countable"];
                xlRange = xlWorksheet.UsedRange;
                int rowCount = xlRange.Rows.Count;
                for (int row = 14; row <= rowCount; row++)
                {
                    string rmcode = Convert.ToString((xlRange.Cells[row, 4] as Microsoft.Office.Interop.Excel.Range)?.Text);
                    string rmdes = Convert.ToString((xlRange.Cells[row, 5] as Microsoft.Office.Interop.Excel.Range)?.Text);
                    string maker = Convert.ToString((xlRange.Cells[row, 7] as Microsoft.Office.Interop.Excel.Range)?.Text);
                    string type = Convert.ToString((xlRange.Cells[row, 8] as Microsoft.Office.Interop.Excel.Range)?.Text);
                    string qty = Convert.ToString((xlRange.Cells[row, 9] as Microsoft.Office.Interop.Excel.Range)?.Text);
                    string up = Convert.ToString((xlRange.Cells[row, 10] as Microsoft.Office.Interop.Excel.Range)?.Text);
                    string amount = Convert.ToString((xlRange.Cells[row, 11] as Microsoft.Office.Interop.Excel.Range)?.Text);

                    if (!string.IsNullOrEmpty(rmcode) &&
                        !string.IsNullOrEmpty(rmdes) &&
                        !string.IsNullOrEmpty(qty) &&
                        !string.IsNullOrEmpty(maker) &&
                        !string.IsNullOrEmpty(type) &&
                           !string.IsNullOrEmpty(up) &&
                              !string.IsNullOrEmpty(amount)

                        )
                    {
                        dgvNGCal.Rows.Add();
                        dgvNGCal.Rows[dgvNGCal.Rows.Count - 1].HeaderCell.Value = dgvNGCal.Rows.Count.ToString();
                        dgvNGCal.Rows[dgvNGCal.Rows.Count - 1].Cells["rmcode"].Value = rmcode;
                        dgvNGCal.Rows[dgvNGCal.Rows.Count - 1].Cells["rmdes"].Value = rmdes;
                        dgvNGCal.Rows[dgvNGCal.Rows.Count - 1].Cells["maker"].Value = maker;
                        dgvNGCal.Rows[dgvNGCal.Rows.Count - 1].Cells["type"].Value = rmdes = type;
                        dgvNGCal.Rows[dgvNGCal.Rows.Count - 1].Cells["qty"].Value = Convert.ToDouble(qty);
                        dgvNGCal.Rows[dgvNGCal.Rows.Count - 1].Cells["up"].Value = Convert.ToDouble(up);
                        dgvNGCal.Rows[dgvNGCal.Rows.Count - 1].Cells["amount"].Value = amount;
                        lbfound.Text = "Found : " + dgvNGCal.Rows.Count.ToString();
                    }

                }

              
                // Clean up
                xlApp.DisplayAlerts = false;
                xlWorkbook.Close(false);
                xlApp.Quit();
                xlApp.DisplayAlerts = true;
                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);
                Marshal.ReleaseComObject(xlWorkbook);
                Marshal.ReleaseComObject(xlApp);
                foreach (var process in Process.GetProcessesByName("EXCEL"))
                {
                    if (string.IsNullOrEmpty(process.MainWindowTitle))
                        process.Kill();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("អ្នកបញ្ចូល Excel ខុសទម្រង់ហើយ​ សូមពិនិត្យ \n" + ex.Message, "Error import", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            Cursor = Cursors.Default;
            dgvNGCal.ClearSelection();
        }
        private void NGCalculateForm_Shown(object sender, EventArgs e)
        {
          
            dgvNGCal.Columns["qty"].HeaderCell.Style.BackColor = Color.FromArgb(255, 192, 255);
            dgvNGCal.Columns["up"].HeaderCell.Style.BackColor = Color.FromArgb(255, 192, 255);
            dgvNGCal.Columns["amount"].HeaderCell.Style.BackColor = Color.FromArgb(255, 192, 255);

            dgvNGCal.Columns["qty2"].HeaderCell.Style.BackColor = Color.FromArgb(192, 255, 255);
            dgvNGCal.Columns["up2"].HeaderCell.Style.BackColor = Color.FromArgb(192, 255, 255);
            dgvNGCal.Columns["amount2"].HeaderCell.Style.BackColor = Color.FromArgb(192, 255, 255);

            dgvResult.Columns["qty3"].HeaderCell.Style.BackColor = Color.FromArgb(255, 192, 255);
            dgvResult.Columns["up3"].HeaderCell.Style.BackColor = Color.FromArgb(255, 192, 255);
            dgvResult.Columns["amount3"].HeaderCell.Style.BackColor = Color.FromArgb(255, 192, 255);

            dgvResult.Columns["qty4"].HeaderCell.Style.BackColor = Color.FromArgb(192, 255, 255);
            dgvResult.Columns["up4"].HeaderCell.Style.BackColor = Color.FromArgb(192, 255, 255);
            dgvResult.Columns["amount4"].HeaderCell.Style.BackColor = Color.FromArgb(192, 255, 255);
        }
        private void BtnBrowseAddjust_Click(object sender, EventArgs e)
        {
            if (dgvNGCal.Rows.Count > 0)
            {

                OpenFileDialog openFD = new OpenFileDialog
                {
                    Title = "Choose an Excel file for Import",
                    Filter = "Excel Files|*.xlsx;"
                };

                if (openFD.ShowDialog() != DialogResult.OK) return;
                Cursor = Cursors.WaitCursor;
                string strFileName = openFD.FileName;
                string fileNameOnly = Path.GetFileName(strFileName);
                txtadd.Text = fileNameOnly;
                Microsoft.Office.Interop.Excel.Application xlApp = null;
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = null;
                Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = null;
                Microsoft.Office.Interop.Excel.Range xlRange = null;

                try
                {
                    Cursor = Cursors.WaitCursor;
                    xlApp = new Microsoft.Office.Interop.Excel.Application();
                    xlWorkbook = xlApp.Workbooks.Open(strFileName);
                    xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets["Rachhan System"];
                    xlRange = xlWorksheet.UsedRange;

                    int rowCount = xlRange.Rows.Count;
                    for (int row = 14; row <= rowCount; row++)
                    {
                        string rmcode = Convert.ToString((xlRange.Cells[row, 4] as Microsoft.Office.Interop.Excel.Range)?.Text);
                        string qty = Convert.ToString((xlRange.Cells[row, 9] as Microsoft.Office.Interop.Excel.Range)?.Text);
                        string up = Convert.ToString((xlRange.Cells[row, 10] as Microsoft.Office.Interop.Excel.Range)?.Text);
                        string amount = Convert.ToString((xlRange.Cells[row, 11] as Microsoft.Office.Interop.Excel.Range)?.Text);

                        if (!string.IsNullOrEmpty(rmcode) &&
                            !string.IsNullOrEmpty(qty) &&
                               !string.IsNullOrEmpty(up) &&
                                  !string.IsNullOrEmpty(amount)

                            )
                        {
                            foreach (DataGridViewRow row1 in dgvNGCal.Rows)
                            {
                                string code1 = row1.Cells["rmcode"].Value.ToString();
                                if (code1 == rmcode)
                                {
                                    row1.Cells["qty2"].Value = Convert.ToDouble(qty);
                                    row1.Cells["up2"].Value = Convert.ToDouble(up);
                                    row1.Cells["amount2"].Value = amount;
                                }
                            }
                           
                        }
                    }
                    Callculate();
                    // Clean up
                    xlApp.DisplayAlerts = false;
                    xlWorkbook.Close(false);
                    xlApp.Quit();
                    xlApp.DisplayAlerts = true;
                    Marshal.ReleaseComObject(xlRange);
                    Marshal.ReleaseComObject(xlWorksheet);
                    Marshal.ReleaseComObject(xlWorkbook);
                    Marshal.ReleaseComObject(xlApp);
                    foreach (var process in Process.GetProcessesByName("EXCEL"))
                    {
                        if (string.IsNullOrEmpty(process.MainWindowTitle))
                            process.Kill();
                    }
                }
                catch (Exception ex)
                {
                    xlApp.DisplayAlerts = false;
                    xlWorkbook.Close(false);
                    xlApp.Quit();
                    xlApp.DisplayAlerts = true;
                    foreach (var process in Process.GetProcessesByName("EXCEL"))
                    {
                        if (string.IsNullOrEmpty(process.MainWindowTitle))
                            process.Kill();
                    }
                    MessageBox.Show("អ្នកបញ្ចូល Excel ខុសទម្រង់ហើយ​ សូមពិនិត្យ \n" + ex.Message, "Error Import", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (dgvResult.Rows.Count > 0)
                {
                    btnPrint.Enabled = true;
                    btnPrint.BringToFront();
                    btnExport.Enabled = true;
                    btnExport.BringToFront();
                }
                Cursor = Cursors.Default;
                dgvNGCal.ClearSelection();
            }
            else
            {
                MessageBox.Show("សូមបញ្ចូល Excel NG Search ជាមុនសិន !", "Alert.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void Callculate()
        {
            if (dgvNGCal.Rows.Count > 0)
            {
                dgvResult.Rows.Clear();
                foreach (DataGridViewRow row in dgvNGCal.Rows)
                {
                    string qty = row.Cells["qty"].Value?.ToString();
                    string qty2 = row.Cells["qty2"].Value?.ToString();
                    string rmdes = row.Cells["rmdes"].Value?.ToString();
                    string maker = row.Cells["maker"].Value?.ToString();
                    string type = row.Cells["type"].Value?.ToString();
                    string rmcode = row.Cells["rmcode"].Value?.ToString();
                    string up = row.Cells["up"].Value?.ToString();
                    string up2 = row.Cells["up2"].Value?.ToString();
                    string amount = row.Cells["amount"].Value?.ToString();
                    string amount2 = row.Cells["amount2"].Value?.ToString();
                    double qty3= 0, up3 = 0, amount3 = 0, qty4 = 0, up4 = 0, amount4 = 0;

                    qty3 = Convert.ToDouble(qty) - Convert.ToDouble(qty2);
                    up3 = Convert.ToDouble(up);
                    amount3 = (Convert.ToDouble(qty3) * Convert.ToDouble(up3));

                    qty4 = Convert.ToDouble(qty) + (Convert.ToDouble(qty2) * -1);
                    up4 = Convert.ToDouble(up2);
                    
                    if (qty3 <= 0)
                    {
                        qty3 = 0;
                        amount3 = 0;
                    }
                    if (qty4 >= 0)
                    {
                        qty4 = 0;
                        amount4 = 0;
                    }
                    else 
                    {
                        qty4 = qty4 * (-1);
                        amount4 = (Convert.ToDouble(qty4) * Convert.ToDouble(up3));
                    }
                    dgvResult.Rows.Add();
                    dgvResult.Rows[dgvResult.Rows.Count - 1].HeaderCell.Value = dgvResult.Rows.Count.ToString();
                    dgvResult.Rows[dgvResult.Rows.Count - 1].Cells["rmcode3"].Value = rmcode;
                    dgvResult.Rows[dgvResult.Rows.Count - 1].Cells["rmdes3"].Value = rmdes;
                    dgvResult.Rows[dgvResult.Rows.Count - 1].Cells["maker3"].Value = maker;
                    dgvResult.Rows[dgvResult.Rows.Count - 1].Cells["type3"].Value = type;
                    dgvResult.Rows[dgvResult.Rows.Count - 1].Cells["qty3"].Value = Convert.ToDouble(qty3);
                    dgvResult.Rows[dgvResult.Rows.Count - 1].Cells["up3"].Value = Convert.ToDouble(up3);
                    dgvResult.Rows[dgvResult.Rows.Count - 1].Cells["amount3"].Value = amount3;
                    dgvResult.Rows[dgvResult.Rows.Count - 1].Cells["qty4"].Value = Convert.ToDouble(qty4);
                    dgvResult.Rows[dgvResult.Rows.Count - 1].Cells["up4"].Value = Convert.ToDouble(up4);
                    dgvResult.Rows[dgvResult.Rows.Count - 1].Cells["amount4"].Value = amount4;
                }
                dgvResult.ClearSelection();
            }
        }
    }
}
