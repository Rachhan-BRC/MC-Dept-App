using MachineDeptApp.MCSDControl.SDRec;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static string SelectedCode;
        public static string SelectedItems;
        public static string SelectedLocCode;
        public static string SelectedLocName;
        string fName;

        public MCStockSearchForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += MCStockSearchForm_Load;
            this.dgvStock.CellDoubleClick += DgvStock_CellDoubleClick;
            this.dgvStock.CellFormatting += DgvStock_CellFormatting;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnPrint.Click += BtnPrint_Click;
            this.btnExport.Click += BtnExport_Click;
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

        private void DgvStock_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 6)
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

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (dgvStock.Rows.Count > 0)
            {
                LbStatus.Text = "កំពុងព្រីន . . . .";
                var CDirectory = Environment.CurrentDirectory;
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\MCStock.xlsx", Editable: true);
                Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["RachhanSystem"];

                if (dgvStock.Rows.Count > 1)
                {
                    worksheet.Range["7:" + (dgvStock.Rows.Count + 5)].Insert();

                }

                worksheet.Cells[2, 2] = DateTime.Now;
                for (int i = 0; i < dgvStock.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvStock.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 6, j + 1] = dgvStock.Rows[i].Cells[j].Value.ToString();

                    }
                }
                worksheet.Range["A6:H" + (dgvStock.Rows.Count + 5)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

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

                LbStatus.Text = "ព្រីនបានជោគជ័យ​ !";
                MessageBox.Show("ការព្រីនបានជោគជ័យ​ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(CDirectory.ToString() + @"\\Report\MCKITStock\" + fName + ".xlsx");
                fName = "";
                LbStatus.Visible = false;

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
            CheckBtnPrint();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            LbStatus.Visible = true;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            dgvStock.Rows.Clear();

            //DB OBS Stock
            DataTable dtOBSStock = new DataTable();
            DataTable dtMatType = new DataTable();
            DataTable dtSQLCondOBS = new DataTable();
            dtSQLCondOBS.Columns.Add("Col");
            dtSQLCondOBS.Columns.Add("Val");
            if (txtCode.Text.Trim() != "")
            {
                if (txtCode.Text.ToString().Contains("*") == true)
                {
                    string SearchValue = txtCode.Text.ToString();
                    SearchValue = SearchValue.Replace("*", "%");
                    dtSQLCondOBS.Rows.Add("ItemCode LIKE ", "'" + SearchValue + "' ");
                }
                else
                {
                    dtSQLCondOBS.Rows.Add("ItemCode = ", "'" + txtCode.Text + "' ");
                }
            }
            if (txtItem.Text.Trim() != "")
            {
                dtSQLCondOBS.Rows.Add("ItemName LIKE ", "'%" + txtItem.Text + "%' ");
            }            
            string SQLCondsOBS = "";
            foreach (DataRow row in dtSQLCondOBS.Rows)
            {
                if (SQLCondsOBS.Trim() == "")
                {
                    SQLCondsOBS = "AND " + row[0] + row[1];
                }
                else
                {
                    SQLCondsOBS = SQLCondsOBS + "AND " + row[0] + row[1];
                }
            }
            try
            {
                cnnOBS.conOBS.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT LocCode, LocName, ItemCode, ItemName, ItemTypeName, SUM(StockQty) AS StockQty FROM" +
                                                                                    "(SELECT T1.LocCode, LocName, T1.ItemCode, T3.ItemName, T4.ItemTypeName, T1.LotNo, T1.BoxNO, Quantity-CONCAT(Qty,0) AS StockQty FROM " +
                                                                                    "(SELECT * FROM prgstock WHERE TypeCode=1) T1 " +
                                                                                    "LEFT JOIN " +
                                                                                    "(SELECT LocCode, LocName FROM mstlocation WHERE DelFlag=0) T2 " +
                                                                                    "ON T1.LocCode=T2.LocCode " +
                                                                                    "INNER JOIN " +
                                                                                    "(SELECT ItemCode, ItemName, ItemType FROM mstitem WHERE DelFlag=0) T3 " +
                                                                                    "ON T1.ItemCode=T3.ItemCode " +
                                                                                    "LEFT JOIN " +
                                                                                    "(SELECT ItemType, ItemTypeName FROM mstitemtype WHERE DelFlag=0) T4 " +
                                                                                    "ON T3.ItemType=T4.ItemType " +
                                                                                    "LEFT JOIN  " +
                                                                                    "(SELECT LocCode, ItemCode, LotNo, BoxNO, SUM(RealValueQty) AS Qty FROM prgalltransaction WHERE TypeCode =1  AND TransactionDate>'" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59' GROUP BY LocCode, ItemCode, LotNo, BoxNO) T5 " +
                                                                                    "ON T1.LocCode=T5.LocCode AND T1.ItemCode=T5.ItemCode AND T1.LotNo=T5.LotNo AND T1.BoxNO=T5.BoxNO " +
                                                                                    "WHERE Quantity-CONCAT(Qty,0)>0) TbStock " +
                                                                                    "WHERE LocCode='MC1' " + SQLCondsOBS +
                                                                                    "GROUP BY LocCode, LocName, ItemCode, ItemName, ItemTypeName " +
                                                                                    "ORDER BY LocCode ASC, ItemCode ASC", cnnOBS.conOBS);
                sda.Fill(dtOBSStock);

                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT ItemCode, ItemName, " +
                    "CASE  " +
                    "  WHEN MatTypeName IS NULL THEN T3.ItemTypeName " +
                    "  ELSE MatTypeName " +
                    "END AS MatTypeName FROM " +
                    "(SELECT * FROM mstitem WHERE DelFlag=0) T1 " +
                    "LEFT JOIN " +
                    "(SELECT * FROM MstMatType WHERE DelFlag=0) T2 " +
                    "ON T1.MatTypeCode =T2.MatTypeCode " +
                    "INNER JOIN " +
                    "(SELECT * FROM mstitemtype) T3 ON T1.ItemType=T3.ItemType", cnnOBS.conOBS);
                sda1.Fill(dtMatType);

            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                LbStatus.Text = "មានបញ្ហា!";
                LbStatus.Refresh();
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnnOBS.conOBS.Close();

            //DB MC Stock
            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Columns");
            dtSQLCond.Columns.Add("Value");
            if (txtCode.Text.ToString().Trim() != "")
            {
                dtSQLCond.Rows.Add("T1.Code = ", "'" + txtCode.Text + "' ");
            }
            if (txtItem.Text.ToString().Trim() != "")
            {
                dtSQLCond.Rows.Add("RMDes LIKE ", "'%" + txtItem.Text + "%' ");
            }

            string SQLCond = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                if (SQLCond.Trim() == "")
                {
                    SQLCond = "WHERE " + row[0].ToString() + row[1].ToString();
                }
                else
                {
                    SQLCond = SQLCond + "AND " + row[0].ToString() + row[1].ToString();
                }
            }
            try
            {
                cnn.con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT T1.Code, RMDes, " +
                    "COALESCE(KIT3Stock,0) AS KIT3Stock, " +
                    "COALESCE(WIR1Stock, 0) AS WIR1Stock, " +
                    "COALESCE(MC1Stock,0) AS MC1Stock FROM " +
                    "(SELECT Code FROM tbSDMCAllTransaction GROUP BY Code) T1 " +
                    "FULL JOIN " +
                    "(SELECT Code, SUM(StockValue) AS KIT3Stock FROM tbSDMCAllTransaction WHERE LocCode = 'KIT3' AND CancelStatus=0 AND RegDate BETWEEN '2023-11-01 00:00:00' AND '"+ DtDate.Value.ToString("yyyy-MM-dd") + " 23:59:59' GROUP BY Code) T2 " +
                    "ON T1.Code=T2.Code " +
                    "FULL JOIN " +
                    "(SELECT Code, SUM(StockValue) AS WIR1Stock FROM tbSDMCAllTransaction WHERE LocCode = 'WIR1' AND CancelStatus=0 AND RegDate BETWEEN '2023-11-01 00:00:00' AND '"+ DtDate.Value.ToString("yyyy-MM-dd") + " 23:59:59' GROUP BY Code) T3 " +
                    "ON T1.Code=T3.Code " +
                    "FULL JOIN " +
                    "(SELECT Code, SUM(StockValue) AS MC1Stock FROM tbSDMCAllTransaction WHERE LocCode = 'MC1' AND CancelStatus=0 AND RegDate BETWEEN '2023-11-01 00:00:00' AND '"+ DtDate.Value.ToString("yyyy-MM-dd") + " 23:59:59' GROUP BY Code) T4 " +
                    "ON T1.Code=T4.Code " +
                    "LEFT JOIN (SELECT ItemCode, ItemName AS RMDes FROM tbMasterItem WHERE Remarks1 IS NULL OR TRIM(Remarks1)='') T5 " +
                    "ON T1.Code=T5.ItemCode " + SQLCond +
                    "ORDER BY Code ASC", cnn.con);

                SqlDataAdapter da1 = new SqlDataAdapter("SELECT Code, POSNo, " +
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
                    "WHERE NOT POSQty IS NULL AND NOT POSQty=0", cnn.con);
                DataTable dtStock = new DataTable();
                da.Fill(dtStock);
                dtStock.Columns.Add("OBS", typeof(double));
                dtStock.Columns.Add("Diff", typeof(double));

                foreach (DataRow OBSRow in dtOBSStock.Rows)
                {
                    int Found = 0;
                    foreach (DataRow MCSDRow in dtStock.Rows)
                    {
                        if (MCSDRow[0].ToString() == OBSRow[2].ToString())
                        {
                            Found = Found + 1;
                            MCSDRow[5] = Math.Round(Convert.ToDouble(OBSRow[5].ToString()),4);
                            //MCSDRow[6] = Math.Round(Convert.ToDouble(OBSRow[5].ToString())-(Convert.ToDouble(MCSDRow[2].ToString())+ Convert.ToDouble(MCSDRow[3].ToString())+Convert.ToDouble(MCSDRow[4].ToString())),4);
                            MCSDRow[6] = Math.Round((Convert.ToDouble(MCSDRow[2].ToString()) + Convert.ToDouble(MCSDRow[3].ToString()) + Convert.ToDouble(MCSDRow[4].ToString()))- Convert.ToDouble(OBSRow[5].ToString()), 4);

                            break;
                        }
                    }
                    if (Found == 0)
                    {
                        dtStock.Rows.Add(OBSRow["ItemCode"].ToString(), OBSRow["ItemName"].ToString(), 0.0000, 0.0000, 0.0000, Convert.ToDouble(OBSRow["StockQty"].ToString()), Convert.ToDouble(OBSRow["StockQty"].ToString())*(-1));
                    }
                    dtStock.AcceptChanges();
                }

                dtStockDetails = new DataTable();
                da1.Fill(dtStockDetails);
                dtStockDetails.Columns.Add("ItemName");
                foreach (DataRow row in dtStockDetails.Rows)
                {
                    foreach (DataRow rowStock in dtStock.Rows)
                    {
                        if (row["Code"].ToString() == rowStock["Code"].ToString())
                        {
                            row["ItemName"] = rowStock["RMDes"].ToString();
                            break;
                        }
                    }
                }
                dtStockDetails.AcceptChanges();

                foreach (DataRow row in dtStock.Rows)
                {
                    if (row[6].ToString().Trim() != "")
                    {
                        if (Convert.ToDouble(row[2].ToString()) != 0 || Convert.ToDouble(row[3].ToString()) != 0 || Convert.ToDouble(row[4].ToString()) != 0 || Convert.ToDouble(row[5].ToString()) != 0)
                        {
                            //ItemType
                            if (CboType.SelectedIndex == 0)
                            {
                                dgvStock.Rows.Add(row[0], row[1], Convert.ToDouble(row[2].ToString()), Convert.ToDouble(row[3].ToString()), Convert.ToDouble(row[4].ToString()), Convert.ToDouble(row[5].ToString()), Convert.ToDouble(row[6].ToString()));
                            }
                            else
                            {
                                int Found = 0;
                                foreach (DataRow rowType in dtMatType.Rows)
                                {
                                    if (rowType[0].ToString() == row[0].ToString() && rowType[2].ToString() == CboType.Text.ToString())
                                    {
                                        Found = Found + 1;
                                        break;
                                    }

                                }
                                if (Found > 0)
                                {
                                    dgvStock.Rows.Add(row[0], row[1], Convert.ToDouble(row[2].ToString()), Convert.ToDouble(row[3].ToString()), Convert.ToDouble(row[4].ToString()), Convert.ToDouble(row[5].ToString()), Convert.ToDouble(row[6].ToString()));
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToDouble(row[2].ToString()) != 0 || Convert.ToDouble(row[3].ToString()) != 0 || Convert.ToDouble(row[4].ToString()) != 0)
                        {
                            double Total = Math.Round(Convert.ToDouble(row[2].ToString()) + Convert.ToDouble(row[3].ToString()) + Convert.ToDouble(row[4].ToString()), 4);
                            dgvStock.Rows.Add(row[0], row[1], Convert.ToDouble(row[2].ToString()), Convert.ToDouble(row[3].ToString()), Convert.ToDouble(row[4].ToString()), 0.0000, Total * (-1));
                        }
                    }
                }
                dgvStock.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

            LbStatus.Text = "រកឃើញ  " + dgvStock.Rows.Count.ToString("N0") + "  វត្ថុធាតុ";
            LbStatus.Refresh();
            CheckBtnPrint();
            Cursor = Cursors.Default;

        }

        private void DgvStock_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                DataGridViewRow row = this.dgvStock.Rows[e.RowIndex];
                SelectedCode = row.Cells[0].Value.ToString();
                SelectedItems = row.Cells[1].Value.ToString();
                SelectedLocCode = "KIT3";
                SelectedLocName = dgvStock.Columns[e.ColumnIndex].HeaderText.ToString();
                if (Convert.ToDouble(row.Cells[2].Value.ToString()) != 0)
                {
                    MCStockSearchDetailForm Mssdf = new MCStockSearchDetailForm(this);
                    Mssdf.ShowDialog();
                }
            }
            if (e.ColumnIndex == 3)
            {
                DataGridViewRow row = this.dgvStock.Rows[e.RowIndex];
                SelectedCode = row.Cells[0].Value.ToString();
                SelectedItems = row.Cells[1].Value.ToString();
                SelectedLocCode = "WIR1";
                SelectedLocName = dgvStock.Columns[e.ColumnIndex].HeaderText.ToString();
                if (Convert.ToDouble(row.Cells[3].Value.ToString()) != 0)
                {
                    MCStockSearchDetailForm Mssdf = new MCStockSearchDetailForm(this);
                    Mssdf.ShowDialog();
                }
            }
            if (e.ColumnIndex == 4)
            {
                DataGridViewRow row = this.dgvStock.Rows[e.RowIndex];
                SelectedCode = row.Cells[0].Value.ToString();
                SelectedItems = row.Cells[1].Value.ToString();
                SelectedLocCode = "MC1";
                SelectedLocName = dgvStock.Columns[e.ColumnIndex].HeaderText.ToString();
                if (Convert.ToDouble(row.Cells[4].Value.ToString()) != 0)
                {
                    MCStockSearchDetailForm Mssdf = new MCStockSearchDetailForm(this);
                    Mssdf.ShowDialog();
                }
            }
        }

        private void CheckBtnPrint()
        {
            if (dgvStock.Rows.Count > 0)
            {
                btnPrint.Enabled = true;
                btnPrint.BackColor = Color.White;
            }
            else
            {
                btnPrint.Enabled = false;
                btnPrint.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
            }
        }

    }
}
