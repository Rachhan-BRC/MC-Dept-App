using MachineDeptApp.MsgClass;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace MachineDeptApp.NG_Input
{
    public partial class NGInputForm : Form
    {
        ErrorMsgClass EMsg = new ErrorMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();

        string ErrorText;

        public NGInputForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Shown += NGInputForm_Shown;
            this.chkSelectAll.Click += ChkSelectAll_Click;

            this.btnScale.Click += BtnScale_Click;
            this.btnCountable.Click += BtnCountable_Click;
            this.btnSemiSet.Click += BtnSemiSet_Click;
            this.btnPrint.Click += BtnPrint_Click;

            this.dgvNGalready.CellClick += DgvNGalready_CellClick;
            this.dgvNGalready.KeyDown += DgvNGalready_KeyDown;

        }

        
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            int FoundChecked = 0;
            foreach (DataGridViewRow row in dgvNGalready.Rows)
            {
                if (row.Cells["ChkPrint"].Value.ToString().ToUpper() == "TRUE")
                {
                    FoundChecked++;
                    break;
                }
            }
            if (FoundChecked > 0)
            {
                QMsg.QAText = "តើអ្នកចង់ព្រីនមែនដែរឬទេ?";
                QMsg.UserClickedYes = false;
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    Cursor = Cursors.WaitCursor;
                    LbExportStatus.Text = "កំពុងបង្កើត File​ . . . .";
                    LbExportStatus.Refresh();
                    ErrorText = "";
                    DateTime RegDate = DateTime.Now;
                    string RegBy = MenuFormV2.UserForNextForm;


                    string fName = "";
                    string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\NG";
                    //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                    if (!Directory.Exists(SavePath))
                    {
                        Directory.CreateDirectory(SavePath);
                    }

                    DataTable dtStatus = new DataTable();
                    dtStatus.Columns.Add("SysNo", typeof(string));
                    dtStatus.Columns.Add("RMCode", typeof(string));
                    DataTable dtPrint = new DataTable();
                    dtPrint.Columns.Add("RMCode", typeof(string));
                    dtPrint.Columns.Add("RMName", typeof(string));
                    dtPrint.Columns.Add("MatCalcFlag", typeof(string));
                    dtPrint.Columns.Add("Maker", typeof(string));
                    dtPrint.Columns.Add("Type", typeof(string));
                    dtPrint.Columns.Add("Qty", typeof(double));
                    dtPrint.Columns.Add("UnitPrice", typeof(double));

                    foreach (DataGridViewRow row in dgvNGalready.Rows)
                    {
                        if (row.Cells["ChkPrint"].Value.ToString().ToUpper() == "TRUE")
                        {
                            string SysNo = row.Cells["SysNo"].Value.ToString();
                            string RMCode = row.Cells["RMCode"].Value.ToString();
                            string RMName = row.Cells["RMName"].Value.ToString();
                            double Qty = Convert.ToDouble(row.Cells["Qty"].Value.ToString());

                            //For Update Status
                            dtStatus.Rows.Add();
                            dtStatus.Rows[dtStatus.Rows.Count-1]["SysNo"] = SysNo;
                            dtStatus.Rows[dtStatus.Rows.Count - 1]["RMCode"] = RMCode;

                            //For Print
                            int FoundDupl = 0;
                            foreach (DataRow rowPrint in dtPrint.Rows)
                            {
                                if (rowPrint["RMCode"].ToString() == RMCode)
                                {
                                    FoundDupl++;
                                    rowPrint["Qty"] = Convert.ToDouble(rowPrint["Qty"]) + Qty;
                                    break;
                                }
                            }
                            if (FoundDupl == 0)
                            {
                                dtPrint.Rows.Add();
                                dtPrint.Rows[dtPrint.Rows.Count - 1]["RMCode"] = RMCode;
                                dtPrint.Rows[dtPrint.Rows.Count - 1]["RMName"] = RMName;
                                dtPrint.Rows[dtPrint.Rows.Count - 1]["Qty"] = Qty;
                            }
                            dtPrint.AcceptChanges();

                        }
                    }


                    //Add type of Count/Uncount
                    if (dtPrint.Rows.Count > 0)
                    {
                        DataTable dtOBS = new DataTable();
                        string RMCodeIN = "";
                        foreach (DataRow row in dtPrint.Rows)
                        {
                            if (RMCodeIN.Trim() == "")
                            {
                                RMCodeIN = "'" + row["RMCode"].ToString() + "'";
                            }
                            else
                            {
                                RMCodeIN += ", '" + row["RMCode"].ToString() + "'";
                            }
                        }
                        try
                        {
                            cnnOBS.conOBS.Open();
                            SqlDataAdapter sda = new SqlDataAdapter(@"SELECT T1.ItemCode AS RMCode, ItemName, MatTypeName, MatCalcFlag, RESV1 AS Maker, COALESCE(T3.UnitPrice, 0) AS UnitPrice FROM 
                                     (SELECT * FROM mstitem WHERE DelFlag=0 AND ItemType=2) T1 
                                     LEFT JOIN (SELECT * FROM MstMatType) T2 
                                     ON T1.MatTypeCode=T2.MatTypeCode 
                                     LEFT JOIN (SELECT ItemCode, EffDate, UnitPrice FROM mstpurchaseprice) T3 
                                     ON T1.ItemCode=T3.ItemCode 
                                     INNER JOIN (SELECT ItemCode, MAX(EffDate) AS EffDate FROM mstpurchaseprice GROUP BY ItemCode) T4 
                                     ON T3.ItemCode=T4.ItemCode AND T3.EffDate=T4.EffDate 
                                     WHERE T1.ItemCode IN ( "+RMCodeIN+ @") 
                                     ORDER BY T1.ItemCode ASC", cnnOBS.conOBS);
                            sda.Fill(dtOBS);
                        }
                        catch (Exception ex)
                        {
                            ErrorText = ex.Message;
                        }
                        cnnOBS.conOBS.Close();

                        if (ErrorText.Trim() == "")
                        {
                            foreach (DataRow row in dtPrint.Rows)
                            {
                                foreach (DataRow rowOBS in dtOBS.Rows)
                                {
                                    if (row["RMCode"].ToString() == rowOBS["RMCode"].ToString())
                                    {
                                        row["MatCalcFlag"] = rowOBS["MatCalcFlag"].ToString();
                                        row["Maker"] = rowOBS["Maker"].ToString();
                                        row["Type"] = rowOBS["MatTypeName"].ToString();
                                        row["UnitPrice"] = Convert.ToDouble(rowOBS["UnitPrice"]);
                                        break;
                                    }
                                }
                            }
                            dtPrint.AcceptChanges();
                        }

                    }
                    //Update to SQL
                    if(ErrorText.Trim() == "")
                    {
                        try
                        {
                            cnn.con.Open();
                            foreach (DataRow row in dtStatus.Rows)
                            {
                                string query = "UPDATE tbNGHistory SET " +
                                                "ReqStat=1" +
                                                "WHERE SysNo = '" + row["SysNo"] + "' AND ItemCode = '" + row["RMCode"] +"' ";
                                SqlCommand cmd = new SqlCommand(query, cnn.con);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorText = ex.Message;
                        }
                        cnn.con.Close();
                    }

                    //Print to Excel
                    if (ErrorText.Trim() == "" && dtPrint.Rows.Count>0)
                    {
                        //Sort dtPrint
                        DataView dataView = dtPrint.DefaultView;//datatable to dataview
                        dataView.Sort = "MatCalcFlag ASC, RMCode ASC";//string that contains the column name  followed by "ASC" (ascending) or "DESC" (descending)
                        dtPrint = dataView.ToTable();
                        dtPrint.AcceptChanges();

                        //Print to Excel
                        var CDirectory = Environment.CurrentDirectory;
                        Excel.Application excelApp = new Excel.Application();
                        Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\NGReq_Template.xlsx", Editable: true);
                        try
                        {
                            //Write to Countable
                            Excel.Worksheet worksheetCountable = (Excel.Worksheet)xlWorkBook.Sheets["Countable"];
                            //Header
                            worksheetCountable.Cells[4, 10] = RegDate;
                            worksheetCountable.Cells[1, 9] = "";
                            worksheetCountable.Cells[5, 10] = RegBy;
                            int WriteItems = 0;
                            foreach (DataRow row in dtPrint.Rows)
                            {
                                if (row["MatCalcFlag"].ToString() == "0")
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
                                Excel.Range destinationRange = worksheetCountable.Range["A15:A" + (14 + WriteItems - 1)];
                                // Insert the copied cells and shift the existing cells down.
                                destinationRange.Insert(Excel.XlInsertShiftDirection.xlShiftDown, sourceRange.Copy());
                                worksheetCountable.Range["15:" + (14 + WriteItems - 1)].RowHeight = 18;

                                //Write Data to cells
                                int WriteCellsIndex = 14;
                                foreach (DataRow row in dtPrint.Rows)
                                {
                                    if (row["MatCalcFlag"].ToString() == "0")
                                    {
                                        worksheetCountable.Cells[WriteCellsIndex, 1] = (WriteCellsIndex - 14) + 1;
                                        worksheetCountable.Cells[WriteCellsIndex, 4] = row["RMCode"].ToString();
                                        worksheetCountable.Cells[WriteCellsIndex, 5] = row["RMName"].ToString();
                                        worksheetCountable.Cells[WriteCellsIndex, 7] = row["Maker"].ToString();
                                        worksheetCountable.Cells[WriteCellsIndex, 8] = row["Type"].ToString();
                                        worksheetCountable.Cells[WriteCellsIndex, 9] = row["Qty"].ToString();
                                        worksheetCountable.Cells[WriteCellsIndex, 10] = row["UnitPrice"].ToString();
                                        worksheetCountable.Cells[WriteCellsIndex, 11] = "=I" + WriteCellsIndex + "*J" + WriteCellsIndex;

                                        WriteCellsIndex = WriteCellsIndex + 1;
                                    }
                                }
                                //Subtotal
                                worksheetCountable.Cells[WriteCellsIndex, 11] = "=SUM(K14:K" + (WriteCellsIndex - 1) + ")";
                                worksheetCountable.Range[WriteCellsIndex + ":" + WriteCellsIndex].RowHeight = 30;
                            }
                            else
                            {
                                foreach (DataRow row in dtPrint.Rows)
                                {
                                    if (row["MatCalcFlag"].ToString() == "0")
                                    {
                                        worksheetCountable.Cells[14, 1] = 1;
                                        worksheetCountable.Cells[14, 4] = row["RMCode"].ToString();
                                        worksheetCountable.Cells[14, 5] = row["RMName"].ToString();
                                        worksheetCountable.Cells[14, 7] = row["Maker"].ToString();
                                        worksheetCountable.Cells[14, 8] = row["Type"].ToString();
                                        worksheetCountable.Cells[14, 9] = row["Qty"].ToString();
                                        worksheetCountable.Cells[14, 10] = row["UnitPrice"].ToString();
                                        worksheetCountable.Cells[14, 11] = "=I14*J14";

                                    }
                                }
                            }

                            //Write to Uncountable
                            Excel.Worksheet worksheetUncountable = (Excel.Worksheet)xlWorkBook.Sheets["Uncountable"];
                            //Header
                            worksheetUncountable.Cells[4, 10] = RegDate;
                            worksheetUncountable.Cells[1, 9] = "";
                            worksheetUncountable.Cells[5, 10] = RegBy;
                            WriteItems = 0;
                            foreach (DataRow row in dtPrint.Rows)
                            {
                                if (row["MatCalcFlag"].ToString() != "0")
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
                                foreach (DataRow row in dtPrint.Rows)
                                {
                                    if (row["MatCalcFlag"].ToString() != "0")
                                    {
                                        worksheetUncountable.Cells[WriteCellsIndex, 1] = (WriteCellsIndex - 14) + 1;
                                        worksheetUncountable.Cells[WriteCellsIndex, 4] = row["RMCode"].ToString();
                                        worksheetUncountable.Cells[WriteCellsIndex, 5] = row["RMName"].ToString();
                                        worksheetUncountable.Cells[WriteCellsIndex, 7] = row["Maker"].ToString();
                                        worksheetUncountable.Cells[WriteCellsIndex, 8] = row["Type"].ToString();
                                        worksheetUncountable.Cells[WriteCellsIndex, 9] = row["Qty"].ToString();
                                        worksheetUncountable.Cells[WriteCellsIndex, 10] = row["UnitPrice"].ToString();
                                        worksheetUncountable.Cells[WriteCellsIndex, 11] = "=I" + WriteCellsIndex + "*J" + WriteCellsIndex;

                                        WriteCellsIndex = WriteCellsIndex + 1;
                                    }
                                }
                                //Subtotal
                                worksheetUncountable.Cells[WriteCellsIndex, 11] = "=SUM(K14:K" + (WriteCellsIndex - 1) + ")";
                                worksheetUncountable.Range[WriteCellsIndex + ":" + WriteCellsIndex].RowHeight = 30;

                            }
                            else
                            {
                                foreach (DataRow row in dtPrint.Rows)
                                {
                                    if (row["MatCalcFlag"].ToString() != "0")
                                    {
                                        worksheetUncountable.Cells[14, 1] = 1;
                                        worksheetUncountable.Cells[14, 4] = row["RMCode"].ToString();
                                        worksheetUncountable.Cells[14, 5] = row["RMName"].ToString();
                                        worksheetUncountable.Cells[14, 7] = row["Maker"].ToString();
                                        worksheetUncountable.Cells[14, 8] = row["Type"].ToString();
                                        worksheetUncountable.Cells[14, 9] = row["Qty"].ToString();
                                        worksheetUncountable.Cells[14, 10] = row["UnitPrice"].ToString();
                                        worksheetUncountable.Cells[14, 11] = "=I14*J14";
                                    }
                                }
                            }

                            // Saving the modified Excel file                        
                            string file = "NG_Calculated ";
                            fName = file + "( " + DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss") + " )";
                            worksheetCountable.SaveAs(SavePath + @"\" + fName + ".xlsx");
                            xlWorkBook.Save();
                            xlWorkBook.Close();
                            excelApp.Quit();

                        }
                        catch (Exception ex)
                        {
                            excelApp.DisplayAlerts = false;
                            xlWorkBook.Close();
                            excelApp.DisplayAlerts = true;
                            excelApp.Quit();
                            ErrorText += "\n" + ex.Message;
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

                    //Refres dgv
                    if (ErrorText.Trim() == "")
                    {
                        for (int i = dgvNGalready.Rows.Count - 1; i > -1; i--)
                        {
                            if (dgvNGalready.Rows[i].Cells["ChkPrint"].Value.ToString().ToUpper() == "TRUE")
                            {
                                dgvNGalready.Rows.RemoveAt(i);
                            }
                        }
                        dgvNGalready.ClearSelection();
                        dgvNGalready.CurrentCell = null;
                        CheckForEnableBtn();
                    }

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        LbExportStatus.Text = "ព្រីនបានជោគជ័យ​ !";
                        LbExportStatus.Refresh();
                        chkSelectAll.Checked = false; 
                        InfoMsg.InfoText = "ការព្រីនបានជោគជ័យ​ !";
                        InfoMsg.ShowingMsg();
                        System.Diagnostics.Process.Start(SavePath + @"\" + fName + ".xlsx");
                    }
                    else
                    {
                        LbExportStatus.Text = "ព្រីនមានបញ្ហា​ !";
                        LbExportStatus.Refresh();
                        EMsg.AlertText = "ព្រីនមានបញ្ហា​ !"+ErrorText;
                        EMsg.ShowingMsg();
                    }
                }
            }
        }
        private void BtnSemiSet_Click(object sender, EventArgs e)
        {
            NGInputBySetForm Nibsf = new NGInputBySetForm(this);
            Nibsf.ShowDialog();
        }
        private void BtnCountable_Click(object sender, EventArgs e)
        {
            NGInputByCountForm Nibcf = new NGInputByCountForm(this);
            Nibcf.ShowDialog();
        }
        private void BtnScale_Click(object sender, EventArgs e)
        {
            NGInputByUncountForm Nibuf = new NGInputByUncountForm(this);
            Nibuf.ShowDialog();
        }

        private void DgvNGalready_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (dgvNGalready.SelectedCells.Count > 0 && dgvNGalready.CurrentCell != null && dgvNGalready.CurrentCell.RowIndex > -1)
                {
                    QMsg.QAText = "តើអ្នកចង់លុបទិន្នន័យនេះមែនឬទេ?";
                    QMsg.UserClickedYes = false;
                    QMsg.ShowingMsg();
                    if (QMsg.UserClickedYes == true)
                    {
                        ErrorText = "";
                        Cursor = Cursors.WaitCursor;
                        string SysNo = dgvNGalready.Rows[dgvNGalready.CurrentCell.RowIndex].Cells["SysNo"].Value.ToString();
                        string ItemCode = dgvNGalready.Rows[dgvNGalready.CurrentCell.RowIndex].Cells["RMCode"].Value.ToString();
                        try
                        {
                            cnn.con.Open();
                            //Delete in tbNGHistory                     
                            SqlCommand cmd = new SqlCommand("DELETE FROM tbNGHistory WHERE SysNo ='" + SysNo + "' AND ItemCode='" + ItemCode + "' ;", cnn.con);
                            cmd.ExecuteNonQuery();
                            //Update tbTrasaction
                            SqlCommand cmd1 = new SqlCommand("UPDATE tbSDMCAllTransaction SET CancelStatus=1 WHERE SysNo ='" + SysNo + "' AND Code='" + ItemCode + "' ;", cnn.con);
                            cmd1.ExecuteNonQuery();                            
                        }
                        catch (Exception ex)
                        {
                            ErrorText = ex.Message;
                        }
                        cnn.con.Close();

                        Cursor = Cursors.Default;

                        if (ErrorText.Trim() == "")
                        {
                            InfoMsg.InfoText = "លុបរួចរាល់!";
                            InfoMsg.ShowingMsg();
                            dgvNGalready.Rows.RemoveAt(dgvNGalready.CurrentCell.RowIndex);
                            dgvNGalready.Refresh();
                            dgvNGalready.ClearSelection();
                            dgvNGalready.CurrentCell = null;
                            CheckForEnableBtn();
                        }
                        else
                        {
                            EMsg.AlertText = "មានបញ្ហា!\n"+ErrorText;
                            EMsg.ShowingMsg();
                        }
                    }
                }
            }
        }
        private void DgvNGalready_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvNGalready.Columns[e.ColumnIndex].Name == "ChkPrint" && e.RowIndex > -1)
            {
                if (dgvNGalready.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper()=="TRUE")
                {
                    dgvNGalready.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                }
                else
                {
                    dgvNGalready.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                }
                dgvNGalready.ClearSelection();
                dgvNGalready.CurrentCell = null;
                CheckForEnableBtn();
                int ChkCount = 0;
                foreach (DataGridViewRow row in dgvNGalready.Rows)
                {
                    if (row.Cells[e.ColumnIndex].Value.ToString().ToUpper() == "TRUE")
                    {
                        ChkCount++;
                    }
                }
                if (ChkCount == dgvNGalready.Rows.Count)
                {
                    chkSelectAll.Checked = true;
                }
                else
                {
                    chkSelectAll.Checked = false;
                }
            }
        }

        private void NGInputForm_Shown(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            ErrorText = "";
            foreach (DataGridViewColumn col in dgvNGalready.Columns)
            {
                //Console.WriteLine(col.Name);
            }

            //Taking Data
            DataTable dt = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT SysNo, T1.ItemCode, ItemName, NGQty, T1.Remarks, RegDate, RegBy FROM " +
                    "\n(SELECT * FROM tbNGHistory WHERE ReqStat = 0) T1 " +
                    "\nLEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') T2 ON T1.ItemCode=T2.ItemCode " +
                    "\nORDER BY SysNo ASC";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

            //Add to Dgv
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dt.Rows)
                {
                    dgvNGalready.Rows.Add();
                    dgvNGalready.Rows[dgvNGalready.Rows.Count - 1].Cells["ChkPrint"].Value = false;
                    dgvNGalready.Rows[dgvNGalready.Rows.Count - 1].Cells["SysNo"].Value = row["SysNo"].ToString();
                    dgvNGalready.Rows[dgvNGalready.Rows.Count - 1].Cells["RMCode"].Value = row["ItemCode"].ToString();
                    dgvNGalready.Rows[dgvNGalready.Rows.Count - 1].Cells["RMName"].Value = row["ItemName"].ToString();
                    dgvNGalready.Rows[dgvNGalready.Rows.Count - 1].Cells["Qty"].Value = Convert.ToDouble(row["NGQty"]);
                    dgvNGalready.Rows[dgvNGalready.Rows.Count - 1].Cells["RegDate"].Value = Convert.ToDateTime(row["RegDate"]);
                    dgvNGalready.Rows[dgvNGalready.Rows.Count - 1].Cells["RegBy"].Value = row["RegBy"].ToString();
                    dgvNGalready.Rows[dgvNGalready.Rows.Count - 1].Cells["Remarks"].Value = row["Remarks"].ToString();
                }
                dgvNGalready.ClearSelection();
                dgvNGalready.CurrentCell = null;
                CheckForEnableBtn();
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
            {
                EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                EMsg.ShowingMsg();
            }
        }
        private void ChkSelectAll_Click(object sender, EventArgs e)
        {
            if (dgvNGalready.Rows.Count > 0)
            {
                if (chkSelectAll.Checked == true)
                {
                    foreach (DataGridViewRow row in dgvNGalready.Rows)
                    {
                        row.Cells["ChkPrint"].Value = true;
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in dgvNGalready.Rows)
                    {
                        row.Cells["ChkPrint"].Value = false;
                    }
                }
                CheckForEnableBtn();
            }
            else
            {
                chkSelectAll.Checked = false;
            }
        }

        //Functions
        private void CheckForEnableBtn()
        {
            //btnDelete
            int FoundChecked = 0;
            foreach (DataGridViewRow row in dgvNGalready.Rows)
            {
                if (row.Cells["ChkPrint"].Value.ToString().ToUpper() == "TRUE")
                {
                    FoundChecked++;
                    break;
                }
            }
            if (FoundChecked > 0)
            {
                btnPrint.Enabled = true;
                btnPrintGRAY.SendToBack();
            }
            else
            {
                btnPrint.Enabled = false;
                btnPrintGRAY.BringToFront();
            }
        }

    }
}
