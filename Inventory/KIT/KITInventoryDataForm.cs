using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.Inventory.KIT
{
    public partial class KITInventoryDataForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        string ErrorText;

        public KITInventoryDataForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection(); 
            this.Load += KITInventoryDataForm_Load;
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
                    saveDialog.FileName = "KIT_Inventory" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
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
        private void DgvSearchResult_SelectionChanged(object sender, EventArgs e)
        {
            CheckBtnPrint();
        }
        private void DgvSearchResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvSearchResult.Rows[e.RowIndex].Cells[10].Value.ToString() == "Deleted")
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
                    DialogResult DSL = MessageBox.Show("តើអ្នកចង់ព្រីនឡាប៊ែលនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DSL == DialogResult.Yes)
                    {
                        string LabelNo = Convert.ToInt32(dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[1].Value.ToString()).ToString();
                        Cursor = Cursors.WaitCursor;
                        DataTable  dtInventory = new DataTable();
                        DataTable dtOBSItems = new DataTable();
                        DataTable dtOBSHeader = new DataTable();
                        ErrorText = "";

                        //Take inventory label
                        try
                        {
                            cnn.con.Open();
                            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbInventory WHERE LocCode='KIT3' AND LabelNo = " + LabelNo, cnn.con);
                            sda.Fill(dtInventory);
                        }
                        catch(Exception ex)
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
                            //Take OBS data
                            if (dtInventory.Rows.Count > 0)
                            {
                                if (dtInventory.Rows[0]["CountingMethod"].ToString() == "POS")
                                {
                                    try
                                    {
                                        cnnOBS.conOBS.Open();
                                        string CodeIN = "";
                                        foreach (DataRow row in dtInventory.Rows)
                                        {
                                            if (CodeIN.Trim() == "")
                                            {
                                                CodeIN = " '" + row["ItemCode"].ToString() +"' ";
                                            }
                                            else
                                            {
                                                CodeIN = CodeIN + ", '" + row["ItemCode"].ToString() + "' ";
                                            }
                                        }
                                        SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM mstitem WHERE ItemType = 2 AND DelFlag=0 AND ItemCode IN ("+CodeIN+")", cnnOBS.conOBS);
                                        sda.Fill(dtOBSItems);
                                        SqlDataAdapter sda1 = new SqlDataAdapter("SELECT DONo, PlanDate, T1.ItemCode, ItemName, PlanQty, POSDeliveryDate  FROM " +
                                            "(SELECT * FROM prgproductionorder) T1 " +
                                            "LEFT JOIN " +
                                            "(SELECT * FROM mstitem WHERE DelFlag=0 AND ItemType=1) T2 " +
                                            "ON T1.ItemCode=T2.ItemCode " +
                                            "WHERE DONo='"+ dtInventory.Rows[0]["QtyDetails"].ToString() + "' ", cnnOBS.conOBS);
                                        sda1.Fill(dtOBSHeader);
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
                                else
                                {
                                    //Take OBS 
                                    try
                                    {
                                        cnnOBS.conOBS.Open();                                        
                                        SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, MatTypeName, Resv1 FROM " +
                                            "(SELECT * FROM mstitem WHERE ItemType = 2 AND DelFlag=0) T1 " +
                                            "INNER JOIN " +
                                            "(SELECT * FROM MstMatType WHERE DelFlag=0) T2 " +
                                            "ON T1.MatTypeCode=T2.MatTypeCode " +
                                            "WHERE ItemCode = '" + dtInventory.Rows[0]["ItemCode"].ToString() +"' ", cnnOBS.conOBS);
                                        sda.Fill(dtOBSItems);
                                        
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
                            }
                            else
                            {
                                if (ErrorText.Trim() == "")
                                {
                                    ErrorText = "គ្មានទិន្នន័យនេះទេ!";
                                }
                                else
                                {
                                    ErrorText = ErrorText + "\n" + "គ្មានទិន្នន័យនេះទេ!";
                                }
                            }

                            //Print
                            if (ErrorText.Trim() == "")
                            {
                                //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                                string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\InventoryLable\KIT3";
                                if (!Directory.Exists(SavePath))
                                {
                                    Directory.CreateDirectory(SavePath);
                                }

                                //សរសេរចូល Excel
                                var CDirectory = Environment.CurrentDirectory;
                                Excel.Application excelApp = new Excel.Application();
                                Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\InventoryLabel_KIT_Template.xlsx", Editable: true);

                                if (dtInventory.Rows[0]["CountingMethod"].ToString() == "POS")
                                {
                                    Excel.Worksheet wsBarcode = (Excel.Worksheet)xlWorkBook.Sheets["POS"];
                                    //ក្បាលលើ
                                    wsBarcode.Cells[2, 3] = LabelNo.ToString();
                                    wsBarcode.Cells[3, 1] = "*" + LabelNo.ToString() + "*";
                                    wsBarcode.Cells[2, 4] = "ម៉ាស៊ីនឃីត (ជាន់លើ)";
                                    wsBarcode.Cells[4, 3] = dtOBSHeader.Rows[0]["DONo"].ToString();
                                    wsBarcode.Cells[5, 3] = dtOBSHeader.Rows[0]["ItemName"].ToString();
                                    wsBarcode.Cells[6, 3] = dtOBSHeader.Rows[0]["PlanQty"].ToString();

                                    // Material List
                                    for (int i = 0; i < dtInventory.Rows.Count; i++)
                                    {
                                        string RMName = "";
                                        foreach (DataRow row in dtOBSItems.Rows)
                                        {
                                            if (dtInventory.Rows[i]["ItemCode"].ToString() == row["ItemCode"].ToString())
                                            {
                                                RMName = row["ItemName"].ToString();
                                            }
                                        }
                                        wsBarcode.Cells[i + 10, 1] = dtInventory.Rows[i]["ItemCode"].ToString();
                                        wsBarcode.Cells[i + 10, 2] = RMName;
                                        wsBarcode.Cells[i + 10, 5] = dtInventory.Rows[i]["Qty"].ToString();

                                    }

                                    //លុប Manual worksheet
                                    Excel.Worksheet wsManual = (Excel.Worksheet)xlWorkBook.Sheets["Fraction"];
                                    excelApp.DisplayAlerts = false;
                                    wsManual.Delete();
                                    excelApp.DisplayAlerts = true;
                                    //ព្រីនចេញ
                                    wsBarcode.PrintOut();


                                    //Save Excel ទុក
                                    string fName = "";
                                    string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                    string file = LabelNo.ToString();
                                    wsBarcode.Name = "RachhanSystem";
                                    fName = file + "-Reprint( " + date + " )";
                                    wsBarcode.SaveAs(SavePath + @"\" + fName + ".xlsx");
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
                                else
                                {
                                    Excel.Worksheet wsBarcode = (Excel.Worksheet)xlWorkBook.Sheets["Fraction"];
                                    //ក្បាលលើ
                                    wsBarcode.Cells[2, 4] = LabelNo.ToString();
                                    wsBarcode.Cells[3, 1] = "*" + LabelNo.ToString() + "*";
                                    wsBarcode.Cells[2, 6] = "ម៉ាស៊ីនឃីត (ជាន់លើ)";

                                    // Material
                                    wsBarcode.Cells[7, 4] = dtOBSItems.Rows[0]["ItemCode"].ToString();
                                    wsBarcode.Cells[9, 4] = dtOBSItems.Rows[0]["ItemName"].ToString();
                                    wsBarcode.Cells[11, 4] = dtOBSItems.Rows[0]["MatTypeName"].ToString();
                                    wsBarcode.Cells[13, 4] = dtOBSItems.Rows[0]["Resv1"].ToString();
                                    wsBarcode.Cells[16, 4] = dtInventory.Rows[0]["Qty"].ToString();
                                    wsBarcode.Cells[16, 6] = "( "+ dtInventory.Rows[0]["QtyDetails"].ToString() + " )";

                                    //លុប Manual worksheet
                                    Excel.Worksheet wsManual = (Excel.Worksheet)xlWorkBook.Sheets["POS"];
                                    excelApp.DisplayAlerts = false;
                                    wsManual.Delete();
                                    excelApp.DisplayAlerts = true;
                                    //ព្រីនចេញ
                                    wsBarcode.PrintOut();


                                    //Save Excel ទុក
                                    string fName = "";
                                    string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                    string file = LabelNo.ToString();
                                    wsBarcode.Name = "RachhanSystem";
                                    fName = file + "-Reprint( " + date + " )";
                                    wsBarcode.SaveAs(SavePath + @"\" + fName + ".xlsx");
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
                        }
                        else
                        {
                            MessageBox.Show("មានបញ្ហា!\n"+ErrorText,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        }

                        Cursor = Cursors.Default;

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
                dtSQLConds.Rows.Add("LabelNo = ", +Convert.ToDouble(txtLabelNo.Text)+" ");
            }
            if (CboSubLocation.Text.Trim() != "")
            {
                dtSQLConds.Rows.Add("SubLoc = ", "'"+CboSubLocation.Text+"' ");
            }
            if (txtRemarks.Text.Trim() != "")
            {
                string SearchValue = txtRemarks.Text;
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*","%");
                    dtSQLConds.Rows.Add("QtyDetails LIKE ", "'" + SearchValue + "' ");
                }
                else
                {
                    dtSQLConds.Rows.Add("QtyDetails = ", "'" + SearchValue + "' ");
                }
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
                    SQLConds = SQLConds + "AND "+ row[0].ToString() + row[1].ToString();
                }
            }

            //Take Inventory Data
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT SubLoc, LabelNo, ItemCode, '' AS ItemName, Qty, QtyDetails, RegDate, RegBy, UpdateDate, UpdateBy, " +
                                            "CASE " +
                                            "  WHEN CancelStatus=1 THEN 'Deleted' " +
                                            "  ELSE 'Active' " +
                                            "END AS Status FROM tbInventory WHERE LocCode='KIT3' "+SQLConds+ " ORDER BY RegDate ASC, SeqNo ASC", cnn.con);
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
                    if (row[2].ToString() == rowOBS[0].ToString())
                    {
                        row[3] = rowOBS[1].ToString();
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
                    int RowHeader = dgvSearchResult.Rows.Count+1;
                    string SubLoc = row[0].ToString();
                    int LabelNo = Convert.ToInt32(row[1].ToString());
                    string ItemCode = row[2].ToString();
                    string ItemName = row[3].ToString();
                    double Qty = Convert.ToDouble(row[4].ToString());
                    string Remarks = row[5].ToString();
                    DateTime RegDate = Convert.ToDateTime(row[6].ToString());
                    string RegBy = row[7].ToString();
                    DateTime UpdateDate = Convert.ToDateTime(row[8].ToString());
                    string UpdateBy = row[9].ToString();
                    string Status = row[10].ToString();

                    if (txtName.Text.Trim() != "")
                    {
                        if (ItemName.ToLower().Contains(txtName.Text.ToLower()) == true)
                        {
                            dgvSearchResult.Rows.Add(SubLoc, LabelNo, ItemCode, ItemName, Qty, Remarks, RegDate, RegBy, UpdateDate, UpdateBy, Status);
                        }
                    }
                    else
                    {
                        dgvSearchResult.Rows.Add(SubLoc, LabelNo, ItemCode, ItemName, Qty, Remarks, RegDate, RegBy, UpdateDate, UpdateBy, Status);
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
                MessageBox.Show("មានបញ្ហា!\n"+ErrorText,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        private void KITInventoryDataForm_Load(object sender, EventArgs e)
        {
            ErrorText = "";
            string[] Status = new string[] { "", "មិនទាន់លុប", "លុប" }; 
            for (int i = 0; i < Status.Length; i++)
            {
                CboStatus.Items.Add(Status[i].ToString());
            }
            CboStatus.SelectedIndex = 0;

            DataTable dtSubLoc = new DataTable();
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT SubLoc FROM tbInventory WHERE LocCode='KIT3' GROUP BY SubLoc ORDER BY SubLoc DESC", cnn.con);
                sda.Fill(dtSubLoc);
            }
            catch(Exception ex)
            {
                if (ErrorText.Trim() == "")
                {
                    ErrorText = ex.Message;
                }
                else
                {
                    ErrorText = ErrorText +"\n"+ ex.Message;
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
            }
            else
            {
                MessageBox.Show("មានបញ្ហា!\n"+ErrorText,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
            }

        }

        private void CheckBtnPrint()
        {
            if (dgvSearchResult.SelectedCells.Count > 0)
            {
                if (dgvSearchResult.CurrentCell.RowIndex > -1)
                {
                    if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[10].Value.ToString() != "Deleted")
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
