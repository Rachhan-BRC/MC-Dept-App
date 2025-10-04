using MachineDeptApp.MCSDControl.SDRec;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace MachineDeptApp.MCSDControl
{
    public partial class MCStockSearchForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        public DataTable dtStockDetails;
        string ErrorText = "";

        public MCStockSearchForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += MCStockSearchForm_Load;
            this.dgvStock.CellDoubleClick += DgvStock_CellDoubleClick;
            this.dgvStock.CellPainting += DgvStock_CellPainting;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnPrint.Click += BtnPrint_Click;
            this.btnPrint.EnabledChanged += BtnPrint_EnabledChanged;
            this.btnExport.Click += BtnExport_Click;

        }

        private void BtnPrint_EnabledChanged(object sender, EventArgs e)
        {
            if (btnPrint.Enabled == true)
                btnPrintGray.SendToBack();
            else
                btnPrintGray.BringToFront();
        }
        private void DgvStock_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvStock.Rows[e.RowIndex].Cells["RMCode"].Value != null && dgvStock.Rows[e.RowIndex].Cells["RMCode"].Value.ToString().Trim() != "")
                {
                    if (dgvStock.Columns[e.ColumnIndex].Name == "GAP")
                    {
                        if (Convert.ToDouble(dgvStock.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) > 0)
                        {
                            e.CellStyle.Font = new Font("Khmer OS Battambang", 10, FontStyle.Bold);
                            e.CellStyle.ForeColor = Color.Orange;
                        }
                        else if (Convert.ToDouble(dgvStock.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) < 0)
                        {
                            e.CellStyle.Font = new Font("Khmer OS Battambang", 10, FontStyle.Bold);
                            e.CellStyle.ForeColor = Color.Red;
                        }
                        else
                        {
                            e.CellStyle.Font = new Font("Khmer OS Battambang", 10, FontStyle.Bold);
                            e.CellStyle.ForeColor = Color.Green;
                        }
                    }
                }
            }
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dtStockDetails.Rows.Count > 0)
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់ទាញទិន្នន័យលម្អិតរបស់ស្តុកមែនឬទេ?","Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    //Make CSV
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "CSV file (*.csv)|*.csv";
                    saveDialog.FileName = "MC_StockDetail.csv";
                    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;
                        try
                        {
                            //Write Column name
                            int columnCount = 0;
                            foreach (DataColumn DtCol in dtStockDetails.Columns)
                            {
                                columnCount = columnCount + 1;
                            }
                            string columnNames = "";

                            //String array for Csv
                            string[] outputCsv;
                            outputCsv = new string[dtStockDetails.Rows.Count + 1];

                            //Set Column Name
                            columnNames = "Code,ItemName,LocCode,POSNo,Remarks,POSQty";
                            outputCsv[0] += columnNames;

                            //Row of data 
                            for (int i = 1; (i - 1) < dtStockDetails.Rows.Count; i++)
                            {
                                string Code = dtStockDetails.Rows[i-1]["Code"].ToString();
                                Code = "\"" + Code.Replace("\"", "\"\"") + "\"";
                                string ItemName = dtStockDetails.Rows[i-1]["ItemName"].ToString();
                                ItemName = "\"" + ItemName.Replace("\"", "\"\"") + "\"";
                                string POSNo = dtStockDetails.Rows[i-1]["POSNo"].ToString();
                                POSNo = "\"" + POSNo.Replace("\"", "\"\"") + "\"";
                                string Remarks = dtStockDetails.Rows[i-1]["Remarks"].ToString();
                                Remarks = "\"" + Remarks.Replace("\"", "\"\"") + "\"";
                                string LocCode = dtStockDetails.Rows[i-1]["LocCode"].ToString();
                                if (LocCode == "KIT3")
                                {
                                    LocCode = "MC KIT";
                                }
                                else if (LocCode == "WIR1")
                                {
                                    LocCode = "SD MC";
                                }
                                else 
                                {
                                    LocCode = "MC Inprocess";
                                }
                                LocCode = "\"" + LocCode.Replace("\"", "\"\"") + "\"";
                                string POSQty = dtStockDetails.Rows[i-1]["POSQty"].ToString();
                                POSQty = "\"" + POSQty.Replace("\"", "\"\"") + "\"";

                                outputCsv[i] = Code + "," + ItemName + "," + LocCode + "," + POSNo + "," + Remarks + "," + POSQty;

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
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (dgvStock.Rows.Count > 0)
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់ព្រីនមែនដែរឬទេ?",MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;
                    LbStatus.Text = "កំពុងព្រីន . . . .";
                    LbStatus.Refresh();
                    string fName = "";

                    var CDirectory = Environment.CurrentDirectory;
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\MCStock.xlsx", Editable: true);
                    Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["RachhanSystem"];

                    //Write to Excel
                    try
                    {
                        worksheet.Cells[2, 2] = DateTime.Now;

                        if (dgvStock.Rows.Count - 1 > 1)
                        {
                            worksheet.Range["7:" + (dgvStock.Rows.Count - 1 + 5)].Insert();
                            worksheet.Range["A6:K" + (dgvStock.Rows.Count - 1 + 5)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        }

                        int SubTTLInd = -1;
                        int WriteRowInd = 6;
                        foreach (DataGridViewRow row in dgvStock.Rows)
                        {
                            if (row.Cells["RMCode"].Value != null && row.Cells["RMCode"].Value.ToString().Trim() != "")
                            {
                                worksheet.Cells[WriteRowInd, 1] = row.Cells["RMCode"].Value;
                                worksheet.Cells[WriteRowInd, 2] = row.Cells["RMDes"].Value;
                                worksheet.Cells[WriteRowInd, 3] = row.Cells["RMType"].Value;
                                worksheet.Cells[WriteRowInd, 4] = row.Cells["UnitPrice"].Value;
                                worksheet.Cells[WriteRowInd, 5] = row.Cells["KIT3"].Value;
                                worksheet.Cells[WriteRowInd, 6] = row.Cells["Wir1"].Value;
                                worksheet.Cells[WriteRowInd, 7] = row.Cells["MC1"].Value;
                                worksheet.Cells[WriteRowInd, 8] = row.Cells["OBS"].Value;
                                worksheet.Cells[WriteRowInd, 9] = row.Cells["GAP"].Value;
                                worksheet.Cells[WriteRowInd, 10] = row.Cells["Amount"].Value;
                                WriteRowInd++;
                            }
                            else
                            {
                                SubTTLInd = row.Index;
                            }
                        }

                        //Sub-TTL
                        if (SubTTLInd >= 0)
                        {
                            worksheet.Cells[WriteRowInd, 5] = dgvStock.Rows[SubTTLInd].Cells["KIT3"].Value;
                            worksheet.Cells[WriteRowInd, 6] = dgvStock.Rows[SubTTLInd].Cells["Wir1"].Value;
                            worksheet.Cells[WriteRowInd, 7] = dgvStock.Rows[SubTTLInd].Cells["MC1"].Value;
                            worksheet.Cells[WriteRowInd, 10] = dgvStock.Rows[SubTTLInd].Cells["Amount"].Value;
                        }

                        //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                        string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\MCKITStock";
                        if (!Directory.Exists(SavePath))
                        {
                            Directory.CreateDirectory(SavePath);
                        }

                        // Saving the modified Excel file                        
                        string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                        string file = "MC_Stock ";
                        fName = file + "( " + date + " )";
                        worksheet.SaveAs(CDirectory.ToString() + @"\Report\MCKITStock\" + fName + ".xlsx");
                    }
                    catch (Exception ex) 
                    {
                        ErrorText = "Write to Excel : " + ex.Message;
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

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        LbStatus.Text = "ព្រីនបានជោគជ័យ​ !";
                        LbStatus.Refresh();
                        MessageBox.Show("ការព្រីនបានជោគជ័យ​ !", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Diagnostics.Process.Start(CDirectory.ToString() + @"\\Report\MCKITStock\" + fName + ".xlsx");
                    }
                    else
                    {
                        LbStatus.Text = "មានបញ្ហា!";
                        LbStatus.Refresh();
                        MessageBox.Show("មានបញ្ហា!\n"+ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void MCStockSearchForm_Load(object sender, EventArgs e)
        {
            CboType.Items.Add("ទាំងអស់");
            CboType.Items.Add("Connector");
            CboType.Items.Add("Terminal");
            CboType.Items.Add("Wire");
            CboType.Items.Add("Other");
            CboType.SelectedIndex = 0;

        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            dgvStock.Rows.Clear();

            //Assign WHERE CLAUSE
            string SQLCond = "";
            string SQLCondOBS = "";
            if (txtCode.Text.ToString().Trim() != "")
            {
                string SValue = txtCode.Text;
                if (SValue.Contains("*") == true)
                {
                    SQLCond += " AND T1.Code LIKE '" + SValue.Replace("*", "%") + "'";
                    SQLCondOBS += " AND T1.ItemCode LIKE '" + SValue.Replace("*", "%") + "'";
                }
                else
                {
                    SQLCond += " AND T1.Code = '" + SValue + "'";
                    SQLCondOBS += " AND T1.ItemCode = '" + SValue + "'";
                }
            }
            if (txtItem.Text.ToString().Trim() != "")
            {
                SQLCond += " AND tbOBSMst.ItemName LIKE '%" + txtItem.Text + "%'";
                SQLCondOBS += " AND ItemName LIKE '%" + txtItem.Text + "%'";
            }
            if (CboType.Text.ToString() != "ទាំងអស់")
            {
                SQLCond += " AND tbOBSMst.MatTypeName = '" + CboType.Text + "' ";
                SQLCondOBS += " AND MatTypeName = '" + CboType.Text + "'";
            }

            //Taking MC DB 
            DataTable dtStock = new DataTable();
            dtStockDetails = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "DECLARE @SDate AS date = '2023-11-01' " +
                    "\nDECLARE @EDate AS date = '" + DtDate.Value.ToString("yyyy-MM-dd") + "' " +
                    "\nSELECT T1.Code, tbOBSMst.ItemName AS OBS_RMDes, tbOBSMst.MatTypeName AS OBS_RMType, COALESCE(tbOBSMst.UnitPrice, 0) AS OBS_UP, " +
                    "\nCOALESCE(KIT3Stock,0) AS KIT3Stock, " +
                    "\nCOALESCE(WIR1Stock, 0) AS WIR1Stock, " +
                    "\nCOALESCE(MC1Stock,0) AS MC1Stock, 0.000 AS OBS, 0.00 AS OBSAmount, (COALESCE(KIT3Stock,0)+COALESCE(WIR1Stock, 0)+COALESCE(MC1Stock,0)) AS Diff FROM " +
                    "\n(SELECT Code FROM tbSDMCAllTransaction GROUP BY Code) T1 " +
                    "\nFULL JOIN " +
                    "\n(SELECT Code, SUM(StockValue) AS KIT3Stock FROM tbSDMCAllTransaction WHERE LocCode = 'KIT3' AND CancelStatus=0 AND CAST(RegDate AS date) BETWEEN @SDate AND @EDate GROUP BY Code) T2 " +
                    "\nON T1.Code=T2.Code " +
                    "\nFULL JOIN " +
                    "\n(SELECT Code, SUM(StockValue) AS WIR1Stock FROM tbSDMCAllTransaction WHERE LocCode = 'WIR1' AND CancelStatus=0 AND CAST(RegDate AS date) BETWEEN @SDate AND @EDate GROUP BY Code) T3 " +
                    "\nON T1.Code=T3.Code " +
                    "\nFULL JOIN " +
                    "\n(SELECT Code, SUM(StockValue) AS MC1Stock FROM tbSDMCAllTransaction WHERE LocCode = 'MC1' AND CancelStatus=0 AND CAST(RegDate AS date) BETWEEN @SDate AND @EDate GROUP BY Code) T4 " +
                    "\nON T1.Code=T4.Code " +
                    "\nLEFT JOIN " +
                    "\n(\tSELECT mstitem.ItemCode, ItemName, MatTypeName, UnitPrice FROM ["+cnnOBS.server+"].[Marunix].dbo.mstitem " +
                    "\n\tLEFT JOIN ["+cnnOBS.server+"].[Marunix].dbo.MstMatType ON mstitem.MatTypeCode=MstMatType.MatTypeCode " +
                    "\n\tLEFT JOIN (SELECT mstpurchaseprice.ItemCode, UnitPrice FROM ["+cnnOBS.server+"].[Marunix].dbo.mstpurchaseprice " +
                    "\n\tINNER JOIN (SELECT ItemCode, MAX(EffDate) AS MaxEffDate FROM ["+cnnOBS.server+"].[Marunix].dbo.mstpurchaseprice WHERE DelFlag = 0 GROUP BY ItemCode) T1 " +
                    "\n\tON mstpurchaseprice.ItemCode = T1.ItemCode AND mstpurchaseprice.EffDate = T1.MaxEffDate) tbUP " +
                    "\n\tON mstitem.ItemCode = tbUP.ItemCode " +
                    "\n\tWHERE ItemType = 2 AND mstitem.DelFlag = 0 " +
                    "\n) tbOBSMst " +
                    "\nON tbOBSMst.ItemCode = T1.Code " +
                    "\n\nWHERE (COALESCE(KIT3Stock,0)+COALESCE(WIR1Stock, 0)+COALESCE(MC1Stock,0)) > 0 " + SQLCond +
                    "\n\nORDER BY Code ASC";
                //Console.WriteLine(SQLQuery);
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);                
                sda.Fill(dtStock);

                SQLQuery = "SELECT Code, POSNo, " +
                    "CASE " +
                    " WHEN ItemName IS NULL THEN " +
                    "   CASE " +
                    "     WHEN LocCode = 'KIT3' THEN N'ទិន្នន័យសង/សល់'" +
                    "     ELSE N'ទិន្នន័យទទួល/សល់' " +
                    "   END" +
                    " ELSE ItemName " +
                    "END AS Remarks, LocCode, POSQty FROM " +
                    "(SELECT Code, POSNo, LocCode, SUM(StockValue) AS POSQty FROM tbSDMCAllTransaction " +
                    "WHERE CancelStatus = '0' AND tbSDMCAllTransaction.RegDate BETWEEN '2023-11-01 00:00:00' AND '" + DtDate.Value.ToString("yyyy-MM-dd") + " 23:59:59' " +
                    "GROUP BY Code, POSNo, LocCode) T1 " +
                    "FULL JOIN " +
                    "(SELECT PosCNo, WIPCode FROM tbPOSDetailofMC) T2 " +
                    "ON T1.POSNo=T2.PosCNo " +
                    "FULL JOIN " +
                    "(SELECT * FROM tbMasterItem) T3 " +
                    "ON T2.WIPCode=T3.ItemCode " +
                    "WHERE NOT POSQty IS NULL AND NOT POSQty=0";

                SqlDataAdapter da1 = new SqlDataAdapter(SQLQuery, cnn.con);
                da1.Fill(dtStockDetails);
            }
            catch (Exception ex)
            {
                ErrorText = "Taking MC DB : " + ex.Message;
            }
            cnn.con.Close();

            //Taking OBS DB 
            DataTable dtOBSStock = new DataTable();
            if (ErrorText.Trim() == "")
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    string SQLQuery = "DECLARE @Loc as nvarchar(50) = 'MC1' " +
                        "\nSELECT T1.ItemCode, ItemName, MatTypeName, COALESCE(UnitPrice, 0) AS UnitPrice, (SUM(Quantity) - SUM(COALESCE(Qty,0))) AS StockQty FROM " +
                        "\n(SELECT LocCode, ItemCode, LotNo, BoxNO, Quantity FROM prgstock WHERE TypeCode = 1) T1 " +
                        "\nINNER JOIN (SELECT * FROM mstitem WHERE DelFlag = 0 AND ItemType = 2) T2 ON T1.ItemCode = T2.ItemCode " +
                        "\nLEFT JOIN (SELECT LocCode, ItemCode, LotNo, BoxNO, SUM(RealValueQty) AS Qty FROM prgalltransaction " +
                        "\nWHERE TypeCode =1  AND CAST(TransactionDate AS date) > CAST(GETDATE() AS date) " +
                        "\nGROUP BY LocCode, ItemCode, LotNo, BoxNO) T3 ON T1.LocCode = T3.LocCode AND T1.ItemCode = T2.ItemCode AND T1.LotNo = T3.LotNo AND T1.BoxNO = T3.BoxNO " +
                        "\nLEFT JOIN (SELECT * FROM MstMatType) T4 ON T2.MatTypeCode = T4.MatTypeCode " +
                        "\nLEFT JOIN " +
                        "\n(\tSELECT mstpurchaseprice.ItemCode, UnitPrice FROM mstpurchaseprice " +
                        "\n\tINNER JOIN (SELECT ItemCode, MAX(EffDate) AS MaxEffDate FROM mstpurchaseprice WHERE DelFlag = 0 GROUP BY ItemCode) T1 " +
                        "\n\tON mstpurchaseprice.ItemCode = T1.ItemCode AND mstpurchaseprice.EffDate = T1.MaxEffDate " +
                        "\n) TbUP ON T1.ItemCode = TbUP.ItemCode " +
                        "\n\nWHERE Quantity - COALESCE(Qty,0) > 0 AND T1.LocCode = @Loc " + SQLCondOBS +
                        "\n\nGROUP BY  T1.ItemCode, ItemName, MatTypeName, UnitPrice " +
                        "\nORDER BY T1.ItemCode ASC ";
                    //Console.WriteLine(SQLQuery);
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                    sda.Fill(dtOBSStock);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហា!\n"+ "Taking OBS DB : " + ex.Message, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }

            //Add to Dgv
            if (ErrorText.Trim() == "")
            {
                //Combine dtStock & dtOBSStock                
                if (dtOBSStock.Rows.Count > 0)
                {
                    foreach (DataRow row in dtOBSStock.Rows)
                    {
                        string Code = row["ItemCode"].ToString();
                        string Name = row["ItemName"].ToString();
                        string Type = row["MatTypeName"].ToString();
                        double UP = Convert.ToDouble(row["UnitPrice"]);
                        double OBSQty = Convert.ToDouble(row["StockQty"]);
                        double Amount = OBSQty * UP;
                        Amount = Convert.ToDouble(Amount.ToString("N2"));

                        bool Found = false;
                        foreach (DataRow rowMC in dtStock.Rows)
                        {
                            if (Code == rowMC["Code"].ToString())
                            {
                                rowMC["OBS"] = OBSQty;
                                UP = Convert.ToDouble(rowMC["OBS_UP"]);
                                Amount = OBSQty * UP;
                                Amount = Convert.ToDouble(Amount.ToString("N2"));
                                rowMC["OBSAmount"] = Amount;
                                rowMC["Diff"] = Math.Round((Convert.ToDouble(rowMC["KIT3Stock"])+ Convert.ToDouble(rowMC["WIR1Stock"]) + Convert.ToDouble(rowMC["MC1Stock"])) - OBSQty, 4);
                                Found = true;
                                break;
                            }
                        }

                        //Add to dtStock if not found
                        if (Found == false)
                        {
                            dtStock.Rows.Add(Code, Name, Type, UP, 0.0, 0.0, 0.0, OBSQty, Amount, Math.Round((-1)*OBSQty,4));
                        }
                        dtStock.AcceptChanges();
                    }
                }

                //Sort dtStock
                DataView dataView = new DataView(dtStock);
                dataView.Sort = "Code ASC";
                dtStock = dataView.ToTable();

                //Add RMDes dtStockDetails
                dtStockDetails.Columns.Add("ItemName");
                foreach (DataRow row in dtStockDetails.Rows)
                {
                    foreach (DataRow rowStock in dtStock.Rows)
                    {
                        if (row["Code"].ToString() == rowStock["Code"].ToString())
                        {
                            row["ItemName"] = rowStock["OBS_RMDes"].ToString();
                            break;
                        }
                    }
                }
                dtStockDetails.AcceptChanges();

                //DGV
                double TTLAmount = 0;
                double TTLAmountKIT = 0;
                double TTLAmountWir = 0;
                double TTLAmountMC = 0;
                foreach (DataRow row in dtStock.Rows)
                {
                    dgvStock.Rows.Add();
                    dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["RMCode"].Value = row["Code"];
                    dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["RMDes"].Value = row["OBS_RMDes"];
                    dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["RMType"].Value = row["OBS_RMType"];
                    dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["UnitPrice"].Value = Convert.ToDouble(row["OBS_UP"]);
                    dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["KIT3"].Value = Convert.ToDouble(row["KIT3Stock"]);
                    dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["Wir1"].Value = Convert.ToDouble(row["WIR1Stock"]);
                    dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["MC1"].Value = Convert.ToDouble(row["MC1Stock"]);
                    dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["OBS"].Value = Convert.ToDouble(row["OBS"]);
                    dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["Amount"].Value = Convert.ToDouble(row["OBSAmount"]);
                    dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["GAP"].Value = Convert.ToDouble(row["Diff"]);
                    TTLAmount += Convert.ToDouble(row["OBSAmount"]);
                    TTLAmountKIT += Convert.ToDouble((Convert.ToDouble(row["OBS_UP"]) * Convert.ToDouble(row["KIT3Stock"])).ToString("N2"));
                    TTLAmountWir += Convert.ToDouble((Convert.ToDouble(row["OBS_UP"]) * Convert.ToDouble(row["WIR1Stock"])).ToString("N2"));
                    TTLAmountMC += Convert.ToDouble((Convert.ToDouble(row["OBS_UP"]) * Convert.ToDouble(row["MC1Stock"])).ToString("N2"));
                }
                //Sub-TTL
                if (dgvStock.Rows.Count > 0)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.DefaultCellStyle.Font = new Font(dgvStock.AlternatingRowsDefaultCellStyle.Font, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
                    row.DefaultCellStyle.ForeColor = Color.Orange;
                    row.DefaultCellStyle.Format = "#,##0.00 $";
                    dgvStock.Rows.Add(row);
                    dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["RMType"].Value = "TTL Amount";
                    dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["Amount"].Value = TTLAmount;
                    dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["KIT3"].Value = TTLAmountKIT;
                    dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["Wir1"].Value = TTLAmountWir;
                    dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["MC1"].Value = TTLAmountMC;
                }
            }

            Cursor = Cursors.Default;
            
            if (ErrorText.Trim() == "")
            {
                dgvStock.ClearSelection();
                LbStatus.Text = "រកឃើញ  " + dgvStock.Rows.Count.ToString("N0") + "  វត្ថុធាតុដើម";
                LbStatus.Refresh();
                if (dgvStock.Rows.Count > 0)
                    btnPrint.Enabled = true;
                else
                    btnPrint.Enabled = false;
            }
            else
            {
                LbStatus.Text = "មានបញ្ហា!";
                LbStatus.Refresh();
                MessageBox.Show("មានបញ្ហា!\n"+ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DgvStock_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvStock.Rows[e.RowIndex].Cells["RMCode"].Value != null && dgvStock.Rows[e.RowIndex].Cells["RMCode"].Value.ToString().Trim() != "")
                {
                    if (dgvStock.Columns[e.ColumnIndex].Name == "KIT3" || dgvStock.Columns[e.ColumnIndex].Name == "Wir1" || dgvStock.Columns[e.ColumnIndex].Name == "MC1")
                    {
                        if (Convert.ToDouble(dgvStock.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) != 0)
                        {
                            MCStockSearchDetailForm Mssdf = new MCStockSearchDetailForm(this.dgvStock, this.dtStockDetails);
                            Mssdf.ShowDialog();
                        }
                    }
                }
            }
        }

    }
}
