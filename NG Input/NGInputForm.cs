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
                    dtPrint.Columns.Add("Qty", typeof(double));

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

                    //Convert total
                    //for (int i = 0; i < dtPrint.Rows.Count; i++)
                    //{
                    //    dtPrint.Rows[i][2] = Math.Round(Convert.ToDouble(dtPrint.Rows[i][2].ToString()), 0, MidpointRounding.AwayFromZero);
                    //}

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
                            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM mstitem WHERE DelFlag=0 AND ItemCode IN (" + RMCodeIN + ") ORDER BY ItemCode ASC;", cnnOBS.conOBS);
                            sda.Fill(dtOBS);
                        }
                        catch (Exception ex)
                        {
                            ErrorText = ex.Message;
                        }
                        cnnOBS.conOBS.Close();

                        if (ErrorText.Trim() == "")
                        {
                            dtPrint.Columns.Add("MatCalcFlag");
                            foreach (DataRow row in dtPrint.Rows)
                            {
                                row["MatCalcFlag"] = "0";
                                foreach (DataRow rowOBS in dtOBS.Rows)
                                {
                                    if (row["RMCode"].ToString() == rowOBS["ItemCode"].ToString())
                                    {
                                        row["MatCalcFlag"] = rowOBS["MatCalcFlag"].ToString();
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
                        //Print excel
                        Excel.Application excelApp = new Excel.Application();
                        Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: Environment.CurrentDirectory + @"\Template\NG_Calculate.xlsx", Editable: true);

                        try
                        {
                            //Countable
                            Excel.Worksheet wsCount = (Excel.Worksheet)xlWorkBook.Sheets["Countable"];
                            int Countable = 0;
                            foreach (DataRow row in dtPrint.Rows)
                            {
                                if (row["MatCalcFlag"].ToString() == "0")
                                {
                                    Countable++;
                                }
                            }
                            if (Countable > 1)
                            {
                                wsCount.Range["5:" + (Countable - 1 + 4)].Insert();
                                wsCount.Range["A4:C" + (Countable - 1 + 4)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                            }
                            wsCount.Cells[2, 2] = DateTime.Now;
                            int RowIndex = 4;
                            for (int i = 0; i < dtPrint.Rows.Count; i++)
                            {
                                if (dtPrint.Rows[i]["MatCalcFlag"].ToString() == "0")
                                {
                                    for (int j = 0; j < dtPrint.Columns.Count - 1; j++)
                                    {
                                        if (dtPrint.Rows[i][j] != null)
                                        {
                                            wsCount.Cells[RowIndex, j + 1] = dtPrint.Rows[i][j];
                                        }
                                    }
                                    RowIndex++;
                                }
                            }

                            //Uncount
                            Excel.Worksheet wsUncount = (Excel.Worksheet)xlWorkBook.Sheets["Uncountable"];
                            int Uncountable = 0;
                            foreach (DataRow row in dtPrint.Rows)
                            {
                                if (row["MatCalcFlag"].ToString() == "1")
                                {
                                    Uncountable++;
                                }
                            }
                            if (Uncountable > 1)
                            {
                                wsUncount.Range["5:" + (Uncountable - 1 + 4)].Insert();
                                wsUncount.Range["A4:C" + (Uncountable - 1 + 4)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                            }
                            wsUncount.Cells[2, 2] = DateTime.Now;
                            RowIndex = 4;
                            for (int i = 0; i < dtPrint.Rows.Count; i++)
                            {
                                if (dtPrint.Rows[i]["MatCalcFlag"].ToString() == "1")
                                {
                                    for (int j = 0; j < dtPrint.Columns.Count - 1; j++)
                                    {
                                        if (dtPrint.Rows[i][j] != null)
                                        {
                                            wsUncount.Cells[RowIndex, j + 1] = dtPrint.Rows[i][j];
                                        }
                                    }
                                    RowIndex++;
                                }
                            }

                            // Saving the modified Excel file
                            string file = "NG_Calculated ";
                            fName = file + "( " + DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss") + " )";
                            wsCount.SaveAs(SavePath + @"\" + fName + ".xlsx");
                        }
                        catch (Exception ex)
                        {
                            ErrorText = ex.Message;
                        }

                        //Close Excel
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
                        EMsg.AlertText = "ព្រីនមានបញ្ហា​ !";
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
