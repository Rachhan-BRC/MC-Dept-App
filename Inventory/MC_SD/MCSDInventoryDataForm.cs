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
using System.Diagnostics;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.Inventory.MC_SD
{
    public partial class MCSDInventoryDataForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        string ErrorText;

        public MCSDInventoryDataForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += MCSDInventoryDataForm_Load;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnPrint.Click += BtnPrint_Click;
            this.dgvSearchResult.CellFormatting += DgvSearchResult_CellFormatting;
            this.dgvSearchResult.SelectionChanged += DgvSearchResult_SelectionChanged;
            this.btnExport.Click += BtnExport_Click;

        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvSearchResult.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("តើអ្នកចង់ទាញទិន្នន័យចេញមែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "CSV file (*.csv)|*.csv";
                    saveDialog.FileName = "MCSD_Inventory" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
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
        }
        private void MCSDInventoryDataForm_Load(object sender, EventArgs e)
        {
            ErrorText = "";
            string[] Status = new string[] { "", "មិនទាន់លុប", "លុប" };
            for (int i = 0; i < Status.Length; i++)
            {
                CboStatus.Items.Add(Status[i].ToString());
            }
            CboStatus.SelectedIndex = 0;

            DataTable dtSubLoc = new DataTable();
            DataTable dtCountAs = new DataTable();
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT SubLoc FROM tbInventory WHERE LocCode='WIR1' GROUP BY SubLoc ORDER BY SubLoc ASC", cnn.con);
                sda.Fill(dtSubLoc);

                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT CountingMethod2 FROM tbInventory WHERE LocCode='WIR1' GROUP BY CountingMethod2 ORDER BY CountingMethod2 ASC", cnn.con);
                sda1.Fill(dtCountAs);

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
            }
            cnn.con.Close();

            if (ErrorText.Trim() == "")
            {
                CboSubLocation.Items.Add("");
                foreach (DataRow row in dtSubLoc.Rows)
                {
                    CboSubLocation.Items.Add(row[0].ToString());
                }
                CboSubLocation.SelectedIndex = 0;

                CboCountAs.Items.Add("");
                foreach (DataRow row in dtCountAs.Rows)
                {
                    CboCountAs.Items.Add(row[0].ToString());
                }
                CboCountAs.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

        }
        private void DgvSearchResult_SelectionChanged(object sender, EventArgs e)
        {
            CheckBtnPrint();
        }
        private void DgvSearchResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvSearchResult.Rows[e.RowIndex].Cells[11].Value.ToString() == "Deleted")
            {
                dgvSearchResult.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Pink;
            }
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (dgvSearchResult.SelectedCells.Count > 0)
            {
                if (dgvSearchResult.CurrentCell.RowIndex > -1)
                {
                    DialogResult DSL = MessageBox.Show("តើអ្នកចង់ព្រីនឡាប៊ែលនេះមែនឬទេ?","Rachhan System",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                    if (DSL == DialogResult.Yes)
                    {
                        ErrorText = "";
                        DataTable dtInventory = new DataTable();
                        DataTable dtOBSRM = new DataTable();
                        string LabelNo = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[1].Value.ToString();
                        //Take inventory info
                        try
                        {
                            cnn.con.Open();
                            SqlDataAdapter sda = new SqlDataAdapter("SELECT LabelNo, CountingMethod2, ItemCode, Qty, QtyDetails, QtyDetails2, BobbinsOrReil FROM " +
                                "(SELECT * FROM tbInventory WHERE LocCode='WIR1') T1 " +
                                "LEFT JOIN (SELECT * FROM tbSDMstUncountMat) T2 " +
                                "ON T1.ItemCode=T2.Code " +
                                "WHERE LabelNo = " + LabelNo, cnn.con);
                            sda.Fill(dtInventory);
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
                        }
                        cnn.con.Close();

                        if (ErrorText.Trim() == "")
                        {
                            if (dtInventory.Rows.Count > 0)
                            {
                                //Take OBS 
                                try
                                {
                                    cnnOBS.conOBS.Open();
                                    SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, MatTypeName, Resv1 FROM " +
                                        "(SELECT * FROM mstitem WHERE ItemType = 2 AND DelFlag=0) T1 " +
                                        "INNER JOIN (SELECT * FROM MstMatType WHERE DelFlag=0) T2 " +
                                        "ON T1.MatTypeCode=T2.MatTypeCode " +
                                        "WHERE ItemCode = '" + dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[3].Value.ToString() + "' ", cnnOBS.conOBS);
                                    sda.Fill(dtOBSRM);
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
                                }
                                cnnOBS.conOBS.Close();

                                //Print 
                                if (ErrorText.Trim() == "")
                                {
                                    try
                                    {
                                        //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                                        string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\InventoryLable\SDMC";
                                        if (!Directory.Exists(SavePath))
                                        {
                                            Directory.CreateDirectory(SavePath);
                                        }

                                        //open excel application and create new workbook
                                        var CDirectory = Environment.CurrentDirectory;
                                        Excel.Application excelApp = new Excel.Application();
                                        Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\InventoryLabel_SD_Template.xlsx", Editable: true);

                                        //Weight
                                        if (dtInventory.Rows[0]["CountingMethod2"].ToString() == "Weight")
                                        {
                                            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Weight"];

                                            worksheet.Cells[2, 6] = "SD MC";
                                            worksheet.Cells[2, 4] = LabelNo;
                                            worksheet.Cells[3, 1] = "*" + LabelNo + "*";
                                            worksheet.Cells[7, 4] = dtInventory.Rows[0]["ItemCode"].ToString();
                                            worksheet.Cells[9, 4] = dtOBSRM.Rows[0]["ItemName"].ToString();
                                            worksheet.Cells[11, 4] = dtOBSRM.Rows[0]["MatTypeName"].ToString();
                                            worksheet.Cells[13, 4] = dtOBSRM.Rows[0]["Resv1"].ToString();
                                            worksheet.Cells[15, 4] = dtInventory.Rows[0]["Qty"].ToString();
                                            //Summary
                                            string BobbinsQty = "";
                                            string[] CalcBobbinsQty = dtInventory.Rows[0]["QtyDetails2"].ToString().Split('|');
                                            if (Convert.ToDouble(CalcBobbinsQty[1].ToString()) == 1)
                                            {
                                                if (dtInventory.Rows[0]["BobbinsOrReil"].ToString() == "Bobbins")
                                                {
                                                    BobbinsQty = CalcBobbinsQty[1].ToString() + " Bobbin";
                                                }
                                                else
                                                {
                                                    BobbinsQty = CalcBobbinsQty[1].ToString() + " Reel";
                                                }
                                            }
                                            else
                                            {
                                                if (dtInventory.Rows[0]["BobbinsOrReil"].ToString() == "Bobbins")
                                                {
                                                    BobbinsQty = CalcBobbinsQty[1].ToString() + " Bobbins";
                                                }
                                                else
                                                {
                                                    BobbinsQty = CalcBobbinsQty[1].ToString() + " Reels";
                                                }
                                            }

                                            worksheet.Cells[15, 6] = "( " + dtInventory.Rows[0]["QtyDetails"].ToString() + " )\n" + BobbinsQty;

                                            // Saving the modified Excel file
                                            var CDirectory2 = Environment.CurrentDirectory;
                                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                            string file = LabelNo.ToString();
                                            string fName = file + "-Reprint( " + date + " )";

                                            //លុប Manual worksheet
                                            Excel.Worksheet wsBarcode = (Excel.Worksheet)xlWorkBook.Sheets["Box"];
                                            excelApp.DisplayAlerts = false;
                                            wsBarcode.Delete();
                                            excelApp.DisplayAlerts = true;

                                            worksheet.Name = "RachhanSystem";
                                            worksheet.SaveAs(SavePath + @"\" + fName + ".xlsx");
                                            worksheet.PrintOut();
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
                                        //Box
                                        else
                                        {
                                            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Box"];
                                            worksheet.Cells[2, 6] = "SD MC";
                                            worksheet.Cells[2, 4] = LabelNo;
                                            worksheet.Cells[3, 1] = "*" + LabelNo + "*";
                                            worksheet.Cells[7, 4] = dtInventory.Rows[0]["ItemCode"].ToString();
                                            worksheet.Cells[9, 4] = dtOBSRM.Rows[0]["ItemName"].ToString();
                                            worksheet.Cells[11, 4] = dtOBSRM.Rows[0]["MatTypeName"].ToString();
                                            worksheet.Cells[13, 4] = dtOBSRM.Rows[0]["Resv1"].ToString();
                                            worksheet.Cells[15, 4] = dtInventory.Rows[0]["Qty"].ToString();
                                            //Summary
                                            string BobbinsQty = "";
                                            if (Convert.ToDouble(dtInventory.Rows[0]["QtyDetails2"].ToString()) == 1)
                                            {
                                                if (dtInventory.Rows[0]["BobbinsOrReil"].ToString() == "Bobbins")
                                                {
                                                    BobbinsQty = dtInventory.Rows[0]["QtyDetails2"].ToString() + " Bobbin";
                                                }
                                                else
                                                {
                                                    BobbinsQty = dtInventory.Rows[0]["QtyDetails2"].ToString() + " Reel";
                                                }
                                            }
                                            else
                                            {
                                                if (dtInventory.Rows[0]["BobbinsOrReil"].ToString() == "Bobbins")
                                                {
                                                    BobbinsQty = dtInventory.Rows[0]["QtyDetails2"].ToString() + " Bobbins";
                                                }
                                                else
                                                {
                                                    BobbinsQty = dtInventory.Rows[0]["QtyDetails2"].ToString() + " Reels";
                                                }
                                            }

                                            worksheet.Cells[15, 6] = "( " + dtInventory.Rows[0]["QtyDetails"].ToString() + " )\n" + BobbinsQty;

                                            // Saving the modified Excel file
                                            var CDirectory2 = Environment.CurrentDirectory;
                                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                            string file = LabelNo.ToString();
                                            string fName = file + "-Reprint( " + date + " )";

                                            //លុប Manual worksheet
                                            Excel.Worksheet wsBarcode = (Excel.Worksheet)xlWorkBook.Sheets["Weight"];
                                            excelApp.DisplayAlerts = false;
                                            wsBarcode.Delete();
                                            excelApp.DisplayAlerts = true;

                                            worksheet.Name = "RachhanSystem";
                                            worksheet.SaveAs(SavePath + @"\" + fName + ".xlsx");
                                            worksheet.PrintOut();
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

                                    }
                                    catch (Exception ex)
                                    {
                                        if (ErrorText.Trim() == "")
                                        {
                                            ErrorText = "Print Label : " + ex.Message;
                                        }
                                        else
                                        {
                                            ErrorText = ErrorText + "\nPrint Label : " + ex.Message;
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
                                    if (ErrorText.Trim() != "")
                                    {
                                        MessageBox.Show(ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show(ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            dgvSearchResult.Rows.Clear();
            DataTable dtSearchResult = new DataTable();
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();

            DataTable dtSQLConds = new DataTable();
            dtSQLConds.Columns.Add("Columns");
            dtSQLConds.Columns.Add("Values");
            if (txtLabelNo.Text.Trim() != "")
            {
                dtSQLConds.Rows.Add("LabelNo = ", +Convert.ToDouble(txtLabelNo.Text) + " ");
            }
            if (CboSubLocation.Text.Trim() != "")
            {
                dtSQLConds.Rows.Add("SubLoc = ", "'" + CboSubLocation.Text + "' ");
            }
            if (CboCountAs.Text.Trim() != "")
            {
                dtSQLConds.Rows.Add("CountingMethod2 = ", "'" + CboCountAs.Text + "' ");
            }
            if (CboStatus.Text.Trim() != "")
            {
                if (CboStatus.Text.ToString() == "លុប")
                {
                    dtSQLConds.Rows.Add("CancelStatus = ", "1 ");
                }
                else
                {
                    dtSQLConds.Rows.Add("CancelStatus = ", "0 ");
                }
            }
            if (txtCode.Text.Trim() != "")
            {
                dtSQLConds.Rows.Add("ItemCode =", "'" + txtCode.Text + "' ");
            }

            string SQLConds = "";
            foreach (DataRow row in dtSQLConds.Rows)
            {
                if (SQLConds.Trim() == "")
                {
                    SQLConds = "AND " + row[0].ToString() + row[1].ToString();
                }
                else
                {
                    SQLConds = SQLConds + "AND " + row[0].ToString() + row[1].ToString();
                }
            }

            //Take Inventory Data
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT SubLoc, LabelNo, CountingMethod2, ItemCode, '' AS ItemName, Qty, QtyDetails, RegDate, RegBy, UpdateDate, UpdateBy, " +
                                            "CASE " +
                                            "  WHEN CancelStatus=1 THEN 'Deleted' " +
                                            "  ELSE 'Active' " +
                                            "END AS Status FROM tbInventory WHERE LocCode='WIR1' " + SQLConds + " ORDER BY RegDate ASC, SeqNo ASC", cnn.con);
                sda.Fill(dtSearchResult);

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
            }
            cnn.con.Close();

            //Take OBS RM Master
            DataTable dtRMDetailOBS = new DataTable();
            if (dtSearchResult.Rows.Count > 0)
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, Resv1 FROM mstitem WHERE ItemType = 2 AND DelFlag=0", cnnOBS.conOBS);
                    sda.Fill(dtRMDetailOBS);
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
                }
                cnnOBS.conOBS.Close();
            }

            //Set Itemname to dtSearchResult
            foreach (DataRow row in dtSearchResult.Rows)
            {
                foreach (DataRow rowOBS in dtRMDetailOBS.Rows)
                {
                    if (row[3].ToString() == rowOBS[0].ToString())
                    {
                        row[4] = rowOBS[1].ToString();
                        dtSearchResult.AcceptChanges();
                        break;
                    }
                }
            }

            Cursor = Cursors.Default;
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtSearchResult.Rows)
                {
                    int RowHeader = dgvSearchResult.Rows.Count + 1;
                    string SubLoc = row[0].ToString();
                    int LabelNo = Convert.ToInt32(row[1].ToString());
                    string CountAs = row[2].ToString();
                    string ItemCode = row[3].ToString();
                    string ItemName = row[4].ToString();
                    double Qty = Convert.ToDouble(row[5].ToString());
                    string Remarks = row[6].ToString();
                    DateTime RegDate = Convert.ToDateTime(row[7].ToString());
                    string RegBy = row[8].ToString();
                    DateTime UpdateDate = Convert.ToDateTime(row[9].ToString());
                    string UpdateBy = row[10].ToString();
                    string Status = row[11].ToString();

                    if (txtName.Text.Trim() != "")
                    {
                        if (ItemName.ToLower().Contains(txtName.Text.ToLower()) == true)
                        {
                            dgvSearchResult.Rows.Add(SubLoc, LabelNo,CountAs, ItemCode, ItemName, Qty, Remarks, RegDate, RegBy, UpdateDate, UpdateBy, Status);
                        }
                    }
                    else
                    {
                        dgvSearchResult.Rows.Add(SubLoc, LabelNo, CountAs, ItemCode, ItemName, Qty, Remarks, RegDate, RegBy, UpdateDate, UpdateBy, Status);
                    }
                }
                dgvSearchResult.ClearSelection();
                LbStatus.Text = "រកឃើញទិន្នន័យ " + dgvSearchResult.Rows.Count.ToString("N0");
                LbStatus.Refresh();
                CheckBtnPrint();
            }
            else
            {
                LbStatus.Text = "មានបញ្ហា!";
                LbStatus.Refresh();
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CheckBtnPrint()
        {
            if (dgvSearchResult.SelectedCells.Count > 0)
            {
                if (dgvSearchResult.CurrentCell.RowIndex > -1)
                {
                    if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[11].Value.ToString() != "Deleted")
                    {
                        btnPrint.Enabled = true;
                        btnPrint.BackColor = Color.White;
                    }
                    else
                    {
                        btnPrint.Enabled = false;
                        btnPrint.BackColor = Color.DarkGray;
                    }
                }
                else
                {
                    btnPrint.Enabled = false;
                    btnPrint.BackColor = Color.DarkGray;
                }
            }
            else
            {
                btnPrint.Enabled = false;
                btnPrint.BackColor = Color.DarkGray;
            }
        }
    }
}
