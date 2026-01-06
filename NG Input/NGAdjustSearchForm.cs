using MachineDeptApp.MsgClass;
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
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;

namespace MachineDeptApp.NG_Input
{
    public partial class NGAdjustSearchForm : Form
    {
        ErrorMsgClass EMsg = new ErrorMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();   
        QuestionMsgClass QMsg = new QuestionMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        DataTable dtOBSUnitPrice;
        DataTable dtForPrintExcel;

        string ErrorText;

        public NGAdjustSearchForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Shown += NGAdjustSearchForm_Shown;
            this.dtpRegFrom.ValueChanged += DtpRegFrom_ValueChanged;
            this.dtpRegTo.ValueChanged += DtpRegTo_ValueChanged;

            this.dgvSearchResult.CellClick += DgvSearchResult_CellClick;

            this.btnUnSelectAll.Click += BtnUnSelectAll_Click;
            this.btnSelectAll.Click += BtnSelectAll_Click;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnExport.Click += BtnExport_Click;
            this.btnPrint.Click += BtnPrint_Click;

        }

        private void DgvSearchResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSearchResult.Columns[e.ColumnIndex].Name == "ChkForPrint")
            {
                if (e.RowIndex > -1)
                {
                    if (dgvSearchResult.Rows[e.RowIndex].Cells["PrintStatus"].Value.ToString() != "OK")
                    {
                        if (dgvSearchResult.Rows[e.RowIndex].Cells["ChkForPrint"].Value.ToString().ToUpper() == "FALSE")
                        {
                            dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                        }
                        else
                        {
                            dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                        }
                        dgvSearchResult.ClearSelection();
                        dgvSearchResult.CurrentCell = null;
                        dgvSearchResult.Refresh();
                    }
                    else
                    {
                        dgvSearchResult.ClearSelection();
                        dgvSearchResult.CurrentCell = null;
                        dgvSearchResult.Refresh();
                        WMsg.WarningText = "ទិន្នន័យនេះព្រីនចេញរួចរាល់ហើយ!";
                        WMsg.ShowingMsg();
                    }
                }
            }
        }
        private void NGAdjustSearchForm_Shown(object sender, EventArgs e)
        {
            CboStatus.SelectedIndex = 0;
        }
        private void DtpRegTo_ValueChanged(object sender, EventArgs e)
        {
            ChkRegDate.Checked = true;
        }
        private void DtpRegFrom_ValueChanged(object sender, EventArgs e)
        {
            ChkRegDate.Checked = true;
        }

        private void BtnUnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                if (dgvRow.Cells["PrintStatus"].Value.ToString() != "OK")
                {
                    dgvRow.Cells["ChkForPrint"].Value = false;
                }
            }
            dgvSearchResult.Refresh();
            dgvSearchResult.ClearSelection();
        }
        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                if (dgvRow.Cells["PrintStatus"].Value.ToString() != "OK")
                {
                    dgvRow.Cells["ChkForPrint"].Value = true;
                }
            }
            dgvSearchResult.Refresh();
            dgvSearchResult.ClearSelection();
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            dgvSearchResult.Rows.Clear();
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();

            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Val");

            if (txtRMCode.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("Code = ", "'" + txtRMCode.Text + "'");
            }
            if (txtRMName.Text.Trim() != "")
            {
                string SearchValue = txtRMName.Text;
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*", "");
                }
                dtSQLCond.Rows.Add("ItemName LIKE ", "'%" + SearchValue + "%'");
            }
            if (CboStatus.Text != "ALL")
            {
                dtSQLCond.Rows.Add("ReqStatus = '"+CboStatus.Text+"'");
            }
            if (ChkRegDate.Checked == true)
            {
                dtSQLCond.Rows.Add("RegDate BETWEEN ", "'" + dtpRegFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dtpRegTo.Value.ToString("yyyy-MM-dd") + " 23:59:59'");
            }
            if (txtPOSC.Text.Trim() != "")
            {
                string SearchValue = txtPOSC.Text;
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*", "%");
                    dtSQLCond.Rows.Add("POSNo LIKE ", "'" + SearchValue + "'");
                }
                else
                {
                    dtSQLCond.Rows.Add("POSNo = ", "'" + SearchValue + "'");
                }
            }

            string SQLConds = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                if (SQLConds.Trim() == "")
                {
                    SQLConds = "WHERE " + row["Col"] + row["Val"];
                }
                else
                {
                    SQLConds += " AND " + row["Col"] + row["Val"];
                }
            }

            //Taking Data
            DataTable dtSearchResult = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT * FROM " +
                    "\n(SELECT T1.*, ItemName FROM " +
                    "\n(SELECT SysNo, POSNo, Code, Qty, " +
                    "\nCASE " +
                    "\n\tWHEN ReqStatus=0 THEN 'NOT YET' " +
                    "\n\tELSE 'OK' " +
                    "\nEND AS ReqStatus, RegDate FROM tbNGReqAdj) T1 " +
                    "\nLEFT JOIN (SELECT * FROM tbMasterItem) T2 " +
                    "\nON T1.Code=T2.ItemCode) TbAllData \n" + SQLConds +
                    "\nORDER BY RegDate ASC";
                //Console.WriteLine(SQLQuery);
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtSearchResult);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

            //Taking UP & Amount
            if(ErrorText.Trim() == "" && dtSearchResult.Rows.Count > 0)
            {
                string CodeIN = "";
                foreach(DataRow row in dtSearchResult.Rows)
                {
                    if (CodeIN.Contains(row["Code"].ToString())==false)
                    {
                        if (CodeIN.Trim() == "")
                        {
                            CodeIN = "'" + row["Code"].ToString() + "'";
                        }
                        else
                        {
                            CodeIN += ", '" + row["Code"].ToString() + "'";
                        }
                    }
                }

                try
                {
                    cnnOBS.conOBS.Open();
                    string SQLQuery = "SELECT T1.ItemCode, ItemName, MatCalcFlag, Resv1 AS Maker, MatTypeName, COALESCE(T2.UnitPrice,0) AS UnitPrice FROM " +
                        "\n(SELECT * FROM mstitem WHERE DelFlag=0 AND ItemType=2) T1 " +
                        "\nLEFT JOIN (SELECT ItemCode, UnitPrice, EffDate FROM mstpurchaseprice WHERE DelFlag=0) T2 ON T1.ItemCode=T2.ItemCode " +
                        "\nINNER JOIN (SELECT ItemCode, MAX(EffDate) AS EffDate FROM mstpurchaseprice WHERE DelFlag=0 GROUP BY ItemCode) T3 ON T2.ItemCode=T3.ItemCode AND T2.EffDate=T3.EffDate " +
                        "\nINNER JOIN (SELECT * FROM MstMatType WHERE DelFlag=0) T4 ON T1.MatTypeCode=T4.MatTypeCode " +
                        "\nWHERE T1.ItemCode IN ("+CodeIN+") " +
                        "\nORDER BY ItemCode ASC";
                    //Console.WriteLine(SQLQuery);
                    dtOBSUnitPrice = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                    sda.Fill(dtOBSUnitPrice);
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                }
                cnnOBS.conOBS.Close();
            }

            //Add to Dgv
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtSearchResult.Rows)
                {
                    dgvSearchResult.Rows.Add();
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["ChkForPrint"].Value = false;
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["SysNo"].Value = row["SysNo"];
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["POSNo"].Value = row["POSNo"];
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Code"].Value = row["Code"];
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["ItemName"].Value = row["ItemName"];
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Qty"].Value = Convert.ToDouble(row["Qty"]);
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["PrintStatus"].Value = row["ReqStatus"];
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["RegDate"].Value = Convert.ToDateTime(row["RegDate"]);
                }
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                LbStatus.Text = "រកឃើញទិន្នន័យ " + dgvSearchResult.Rows.Count.ToString("N0");
                LbStatus.Refresh();
                dgvSearchResult.ClearSelection();
                dgvSearchResult.CurrentCell = null;
            }
            else
            {
                EMsg.AlertText = ErrorText;
                EMsg.ShowingMsg();
            }
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvSearchResult.Rows.Count > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV file (*.csv)|*.csv";
                saveDialog.FileName = "NG Adjust " + DateTime.Now.ToString("yyyyMMdd HHmmss") + ".csv";
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
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            int FoundTobePrint = 0;
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                if (dgvRow.Cells["ChkForPrint"].Value.ToString().ToLower() == "true" && dgvRow.Cells["PrintStatus"].Value.ToString().ToLower() != "ok")
                {
                    FoundTobePrint++;
                    break;
                }
            }
            if (FoundTobePrint > 0)
            {
                QMsg.QAText = "តើអ្នកចង់ព្រីនទិន្នន័យទាំងនេះចេញមែនឬទេ?";
                QMsg.UserClickedYes = false;
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    PrintExcelOut();
                }
            }
            else
            {
                MessageBox.Show("សូមជ្រើសរើសទិន្នន័យដើម្បីព្រីនចេញ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //Function
        private void PrintExcelOut()
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            string fName = "";
            DateTime PrintingDate = DateTime.Now;
            string PrintingBy = MenuFormV2.UserForNextForm;
            string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\NGAdjust";
            //ឆែករកមើល Folder បើគ្មាន => បង្កើត
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
            ClearDtForPrintExcel();

            //Double Check Document is already print or not
            string SysNoIN = "";
            DataTable dtTotalQty = new DataTable();
            try
            {
                cnn.con.Open();

                //Taking SysNo
                foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
                {
                    if (dgvRow.Cells["ChkForPrint"].Value.ToString().ToLower() == "true" && dgvRow.Cells["PrintStatus"].Value.ToString().ToLower() != "ok")
                    {
                        string SysNoCheck = dgvRow.Cells["SysNo"].Value.ToString();
                        string SQLQuery = "SELECT * FROM tbNGReqAdj WHERE ReqStatus = 0 AND SysNo='"+SysNoCheck+"'";
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (SysNoIN.Trim() == "")
                            {
                                SysNoIN = "'"+SysNoCheck+"'";
                            }
                            else
                            {
                                SysNoIN += ", '" + SysNoCheck + "'";
                            }
                        }
                    }
                }

                //Calculate Total
                //Console.WriteLine(SysNoIN);
                if (SysNoIN.Trim() != "")
                {
                    string SQLQuery = "SELECT Code, SUM(Qty) AS TotalQty FROM tbNGReqAdj WHERE ReqStatus = 0 AND SysNo IN (" + SysNoIN + ") " +
                        "\nGROUP BY Code " +
                        "\nORDER BY Code ASC";
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtTotalQty);
                }
            }
            catch (Exception ex)
            {
                ErrorText = "Calculate Total : " + ex.Message;
            }
            cnn.con.Close();

            //Add to dtForPrintExcel
            if (ErrorText.Trim() == "" && dtTotalQty.Rows.Count > 0)
            {
                try
                {
                    foreach (DataRow row in dtTotalQty.Rows)
                    {
                        string Code = row["Code"].ToString();
                        double Qty = Convert.ToDouble(row["TotalQty"].ToString());
                        string ItemName = "";
                        int MatCalcFlag = 0;
                        string Maker = "";
                        string MatTypeName = "";
                        double UP = 0;
                        foreach (DataRow rowOBS in dtOBSUnitPrice.Rows)
                        {
                            if (Code == rowOBS["ItemCode"].ToString())
                            {
                                ItemName = rowOBS["ItemName"].ToString();
                                MatCalcFlag = Convert.ToInt32(rowOBS["MatCalcFlag"].ToString());
                                Maker = rowOBS["Maker"].ToString();
                                MatTypeName = rowOBS["MatTypeName"].ToString();
                                UP = Convert.ToDouble(rowOBS["UnitPrice"].ToString());
                                break;
                            }
                        }

                        dtForPrintExcel.Rows.Add();
                        dtForPrintExcel.Rows[dtForPrintExcel.Rows.Count - 1]["RMCode"] = Code;
                        dtForPrintExcel.Rows[dtForPrintExcel.Rows.Count - 1]["ItemName"] = ItemName;
                        dtForPrintExcel.Rows[dtForPrintExcel.Rows.Count - 1]["MatCalcFlag"] = MatCalcFlag;
                        dtForPrintExcel.Rows[dtForPrintExcel.Rows.Count - 1]["Maker"] = Maker;
                        dtForPrintExcel.Rows[dtForPrintExcel.Rows.Count - 1]["MatTypeName"] = MatTypeName;
                        dtForPrintExcel.Rows[dtForPrintExcel.Rows.Count - 1]["Qty"] = Qty;
                        dtForPrintExcel.Rows[dtForPrintExcel.Rows.Count - 1]["UnitPrice"] = UP;
                    }
                }
                catch (Exception ex)
                {
                    ErrorText = "Add to dtForPrintExcel : " + ex.Message;
                }
            }

            //Update Status
            if (ErrorText.Trim() == "")
            {
                try
                {
                    cnn.con.Open();
                    string query = "UPDATE tbNGReqAdj SET " +
                                            "ReqStatus=1," +
                                            "UpdateDate='" + PrintingDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                            "WHERE SysNo IN ("+SysNoIN+") ";
                    SqlCommand cmd = new SqlCommand(query, cnn.con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ErrorText = "Update Status : " + ex.Message;
                }
                cnn.con.Close();
            }

            //Print Out 
            if (ErrorText.Trim() == "" && dtForPrintExcel.Rows.Count > 0)
            {
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: Environment.CurrentDirectory.ToString() + @"\Template\NGReq_Template.xlsx", Editable: true);
                try
                {
                    //Write to Countable
                    Excel.Worksheet worksheetCountable = (Excel.Worksheet)xlWorkBook.Sheets["Countable"];
                    //Rename
                    worksheetCountable.Name = "Rachhan System";
                    //Header
                    worksheetCountable.Cells[4, 10] = PrintingDate;
                    worksheetCountable.Cells[1, 9] = "";
                    worksheetCountable.Cells[5, 10] = PrintingBy;

                    worksheetCountable.Cells[8, 5] = "Adjust OBS Stock";
                    worksheetCountable.Cells[14, 12] = "Add to OBS";

                    foreach (Excel.Shape shape in worksheetCountable.Shapes)
                    {
                        //TotalItemsShape, MatchingShape, OverShape, MinusShape
                        if (shape.Type == Microsoft.Office.Core.MsoShapeType.msoGroup) // Check if the shape is a group
                        {
                            foreach (Excel.Shape childShape in shape.GroupItems) // Iterate through the grouped items
                            {
                                if (childShape.Name == "FromLocName") // Check for your shape by name
                                {
                                    childShape.TextFrame.Characters().Text = "IT";
                                }

                                if (childShape.Name == "FromLocCode") // Check for your shape by name
                                {
                                    childShape.TextFrame.Characters().Text = "";
                                }

                                if (childShape.Name == "ToLocName")
                                {
                                    // Set the text for the childShape
                                    childShape.TextFrame.Characters().Text = "Machine";
                                }

                                if (childShape.Name == "ToLocCode")
                                {
                                    // Set the text for the childShape
                                    childShape.TextFrame.Characters().Text = "MC1";
                                }

                            }
                        }
                    }

                    if (dtForPrintExcel.Rows.Count > 1)
                    {
                        //Insert More Rows
                        // Define the range that you want to copy.
                        Excel.Range sourceRange = worksheetCountable.Range["A14:M14"];
                        // Perform the copy operation.
                        sourceRange.Copy(Type.Missing);
                        // Specify the range where you want to insert the copied rows.
                        Excel.Range destinationRange = worksheetCountable.Range["A15:A" + (14 + dtForPrintExcel.Rows.Count - 1)];
                        // Insert the copied cells and shift the existing cells down.
                        destinationRange.Insert(Excel.XlInsertShiftDirection.xlShiftDown, sourceRange.Copy());
                        worksheetCountable.Range["15:" + (14 + dtForPrintExcel.Rows.Count - 1)].RowHeight = 18;
                    }

                    //Write Data to cells
                    int WriteCellsIndex = 14;
                    foreach (DataRow row in dtForPrintExcel.Rows)
                    {
                        worksheetCountable.Cells[WriteCellsIndex, 1] = (WriteCellsIndex - 14) + 1;
                        worksheetCountable.Cells[WriteCellsIndex, 4] = row["RMCode"].ToString();
                        worksheetCountable.Cells[WriteCellsIndex, 5] = row["ItemName"].ToString();
                        worksheetCountable.Cells[WriteCellsIndex, 7] = row["Maker"].ToString();
                        worksheetCountable.Cells[WriteCellsIndex, 8] = row["MatTypeName"].ToString();
                        worksheetCountable.Cells[WriteCellsIndex, 9] = row["Qty"].ToString();
                        worksheetCountable.Cells[WriteCellsIndex, 10] = row["UnitPrice"].ToString();
                        worksheetCountable.Cells[WriteCellsIndex, 11] = "=I" + WriteCellsIndex + "*J" + WriteCellsIndex;

                        WriteCellsIndex = WriteCellsIndex + 1;
                    }
                    //Subtotal
                    worksheetCountable.Cells[WriteCellsIndex, 11] = "=SUM(K14:K" + (WriteCellsIndex - 1) + ")";
                    worksheetCountable.Range[WriteCellsIndex + ":" + WriteCellsIndex].RowHeight = 30;

                    //Delete Uncountable sheet
                    Excel.Worksheet worksheetDeleted = (Excel.Worksheet)xlWorkBook.Sheets["Uncountable"];
                    excelApp.DisplayAlerts = false;
                    worksheetDeleted.Delete();
                    excelApp.DisplayAlerts = true;

                    // Saving the modified Excel file                 
                    string file = "NG Adjust ";
                    fName = file + " ( " + PrintingDate.ToString("dd-MM-yyyy HH_mm_ss") + " )";
                    worksheetCountable.SaveAs(SavePath + @"\" + fName + ".xlsx");
                }
                catch (Exception ex)
                {
                    
                    ErrorText += "\n" + ex.Message;
                }

                excelApp.DisplayAlerts = false;
                xlWorkBook.Close();
                excelApp.DisplayAlerts = true;
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

            //Update Dgv 
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                if (dgvRow.Cells["ChkForPrint"].Value.ToString().ToLower() == "true" && dgvRow.Cells["PrintStatus"].Value.ToString().ToLower() != "ok")
                {
                    dgvRow.Cells["ChkForPrint"].Value = false;
                    dgvRow.Cells["PrintStatus"].Value = "OK";
                }
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                InfoMsg.InfoText = "ព្រីនរួចរាល់!";
                InfoMsg.ShowingMsg();
                System.Diagnostics.Process.Start(SavePath + @"\" + fName + ".xlsx");
                fName = "";
            }
            else
            {
                EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                EMsg.ShowingMsg();
            }
        }
        private void ClearDtForPrintExcel()
        {
            dtForPrintExcel = new DataTable();
            dtForPrintExcel.Columns.Add("RMCode");
            dtForPrintExcel.Columns.Add("ItemName");
            dtForPrintExcel.Columns.Add("MatCalcFlag");
            dtForPrintExcel.Columns.Add("Maker");
            dtForPrintExcel.Columns.Add("MatTypeName");
            dtForPrintExcel.Columns.Add("Qty");
            dtForPrintExcel.Columns.Add("UnitPrice");
        }
    }
}
