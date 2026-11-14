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
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Controls;

namespace MachineDeptApp.Inventory.MC_SD
{
    public partial class MCSDInventoryDataForm : Form
    {
        SQLConnect con = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        public MCSDInventoryDataForm()
        {
            InitializeComponent();
            this.con.Connection();
            this.cnnOBS.Connection();
            this.btnSearch.Click += BtnSearch_Click;
            this.dgvSearchResult.CellFormatting += DgvSearchResult_CellFormatting;
            this.txtCode.TextChanged += TxtCode_TextChanged;
            this.txtLabelNo.TextChanged += TxtLabelNo_TextChanged;
            this.txtName.TextChanged += TxtName_TextChanged;
            this.btnExport.Click += BtnExport_Click;
            this.btnExportVerify.Click += BtnExportVerify_Click;
            //Same export icon as btnExport, small size beside the text 'ផ្ទៀងផ្ទាត់'
            Bitmap verifyIcon = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(verifyIcon))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(this.btnExport.BackgroundImage, 0, 0, 32, 32);
            }
            this.btnExportVerify.Image = verifyIcon;
            this.tabCtrl.SelectedIndexChanged += TabCtrl_SelectedIndexChanged;
            this.cboResult.SelectedIndex = 0;
            this.cboResult.SelectedIndexChanged += CboResult_SelectedIndexChanged;
            this.cboType.SelectedIndex = 0;
            this.cboType.SelectedIndexChanged += CboType_SelectedIndexChanged;
            this.btnPrint.Click += BtnPrint_Click;
            this.dgvSearchResult.SelectionChanged += DgvSearchResult_SelectionChanged;
        }

        private void DgvSearchResult_SelectionChanged(object sender, EventArgs e)
        {
            CheckBtnPrint();
        }

        //Print button : only on tab 'លម្អិត' with a selected row that is not Deleted
        private void CheckBtnPrint()
        {
            bool canPrint = false;
            if (tabCtrl.SelectedTab == tabDetail && dgvSearchResult.SelectedCells.Count > 0)
            {
                DataGridViewRow drow = dgvSearchResult.Rows[dgvSearchResult.SelectedCells[0].RowIndex];
                canPrint = drow.Tag is DataRow && Convert.ToString(drow.Cells["status"].Value) != "Deleted";
            }
            btnPrint.Enabled = canPrint;
            btnPrint.BackColor = canPrint ? Color.White : Color.DarkGray;
        }

        //Re-print the label of the selected row, same as the label printed in 'MCSDCountingForm' when this row was counted :
        //Total Qty = all PIC of this Code, Bobbin count = this PIC of this Code (both up to this row, Active only)
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (tabCtrl.SelectedTab != tabDetail || dgvSearchResult.SelectedCells.Count == 0)
            {
                return;
            }
            DataRow data = dgvSearchResult.Rows[dgvSearchResult.SelectedCells[0].RowIndex].Tag as DataRow;
            if (data == null || data["Status"].ToString() == "Deleted")
            {
                return;
            }
            string itemCode = data["Code"].ToString();
            string sysNo = data["SysNo"].ToString().Trim();
            string picName = data["PIC"].ToString().Trim();
            string labelNo;
            DateTime regDate;
            try
            {
                labelNo = Convert.ToInt32(data["LabelNo"]).ToString("D4");
                regDate = Convert.ToDateTime(data["RegDate"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ទិន្នន័យឡាប៊ែលនេះមិនត្រឹមត្រូវ!\n" + ex.Message, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult DSL = MessageBox.Show("តើអ្នកចង់ព្រីនឡាប៊ែលនេះឡើងវិញមែនឬទេ?\n\n" +
                "ល.រ.ឡាប៊ែល : " + labelNo + "\n" +
                (sysNo != "" ? "SysNo : " + sysNo + "\n" : "") +
                "លេខកូដ : " + itemCode, MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DSL != DialogResult.Yes)
            {
                return;
            }

            string OldStatus = LbStatus.Text;
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងព្រីន . . .";
            LbStatus.Refresh();

            //Totals as they were when this row was counted
            double totalQty = 0;
            double bobbinCount = 0;
            try
            {
                con.con.Open();
                SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(Qty), 0) FROM tbSDMCStockInventory WHERE Code = @Code AND Status = 'Active' AND RegDate <= @RegDate", con.con);
                cmd.Parameters.AddWithValue("@Code", itemCode);
                cmd.Parameters.Add("@RegDate", SqlDbType.DateTime2).Value = regDate;
                totalQty = Convert.ToDouble(cmd.ExecuteScalar());
                cmd = new SqlCommand("SELECT ISNULL(SUM(BobbinQty), 0) FROM tbSDMCStockInventory WHERE Code = @Code AND PIC = @PIC AND Status = 'Active' AND RegDate <= @RegDate", con.con);
                cmd.Parameters.AddWithValue("@Code", itemCode);
                cmd.Parameters.AddWithValue("@PIC", picName);
                cmd.Parameters.Add("@RegDate", SqlDbType.DateTime2).Value = regDate;
                bobbinCount = Convert.ToDouble(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                con.con.Close();
                Cursor = Cursors.Default;
                LbStatus.Text = OldStatus;
                MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 3 !" + ex.Message, "Error RePrint", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            con.con.Close();

            string ErrorText = "";
            Excel.Application excelApp = null;
            Excel.Workbook xlWorkBook = null;
            Excel.Worksheet worksheet = null;
            try
            {
                string SavePath = Path.Combine(Environment.CurrentDirectory, @"Report\LabelSDInventory");
                Directory.CreateDirectory(SavePath);
                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                xlWorkBook = excelApp.Workbooks.Open(Path.Combine(Environment.CurrentDirectory, @"Template\LabelSDInventory.xlsx"), Editable: false);
                worksheet = (Excel.Worksheet)xlWorkBook.Sheets[1];
                //Same cells as 'MCSDCountingForm' >> BtnPrint_Click
                worksheet.Cells[1, 2] = "Date : " + regDate.ToString("dd-MM-yyyy");
                worksheet.Cells[2, 3] = labelNo;
                //Scanned label >> barcode = SysNo, Code typed in (no SysNo) >> barcode = Label No
                worksheet.Cells[4, 1] = "*" + (sysNo != "" ? sysNo : labelNo) + "*";
                worksheet.Cells[6, 2] = itemCode;
                worksheet.Cells[7, 2] = data["RMName"].ToString();
                worksheet.Cells[8, 2] = totalQty.ToString("N2");
                worksheet.Cells[9, 2] = bobbinCount.ToString("0.##");
                //cboPic items in the counting form start with 2 spaces
                worksheet.Cells[14, 6] = "  " + picName;
                xlWorkBook.SaveAs(Path.Combine(SavePath, "LabelSDInventory" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".xlsx"));
                worksheet.PrintOutEx();
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            finally
            {
                if (xlWorkBook != null)
                    xlWorkBook.Close(false);
                if (excelApp != null)
                    excelApp.Quit();
                if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                if (xlWorkBook != null) Marshal.ReleaseComObject(xlWorkBook);
                if (excelApp != null) Marshal.ReleaseComObject(excelApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Cursor = Cursors.Default;
            LbStatus.Text = OldStatus;
            if (ErrorText != "")
            {
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

        //Filter by Type (OBS Material Type) same as 'ទិន្នន័យស្តុក MC' >> "" when ទាំងអស់
        private string TypeCondition(string codeColumn)
        {
            if (cboType.SelectedIndex <= 0)
            {
                return "";
            }
            return codeColumn + " IN (SELECT mstitem.ItemCode FROM [" + cnnOBS.server + "].[" + cnnOBS.db + "].dbo.mstitem " +
                "LEFT JOIN [" + cnnOBS.server + "].[" + cnnOBS.db + "].dbo.MstMatType ON mstitem.MatTypeCode = MstMatType.MatTypeCode " +
                "WHERE ItemType = 2 AND mstitem.DelFlag = 0 AND MatTypeName = '" + cboType.Text + "')";
        }

        private void CboResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

        private void TabCtrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Label No only applies to the detail tab, Result only to the verify tab
            txtLabelNo.Enabled = tabCtrl.SelectedTab == tabDetail;
            cboResult.Enabled = tabCtrl.SelectedTab == tabVerify;
            btnSearch.PerformClick();
            CheckBtnPrint();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            DialogResult DLS = MessageBox.Show("Are you sure you want to export?", "Confirm Export", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DLS == DialogResult.Yes)
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV file (*.csv)|*.csv";
                saveDialog.FileName = "DataImportMulti" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv";
                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    Cursor = Cursors.WaitCursor;
                    try
                    {
                        //Export the grid of the active tab
                        DataGridView dgv = tabCtrl.SelectedTab == tabVerify ? dgvVerify : dgvSearchResult;

                        //Write Column name
                        int columnCount = 0;
                        foreach (DataGridViewColumn DgvCol in dgv.Columns)
                        {
                            if (DgvCol.Visible == true)
                            {
                                columnCount = columnCount + 1;
                            }
                        }
                        string columnNames = "";

                        //String array for Csv
                        string[] outputCsv;
                        outputCsv = new string[dgv.Rows.Count + 1];

                        //Set Column Name
                        for (int i = 0; i < columnCount; i++)
                        {
                            if (dgv.Columns[i].Visible == true)
                            {
                                columnNames += dgv.Columns[i].HeaderText.ToString().Replace("\n", " ") + ",";
                            }
                        }
                        outputCsv[0] += columnNames;

                        //Row of data
                        for (int i = 1; (i - 1) < dgv.Rows.Count; i++)
                        {
                            for (int j = 0; j < columnCount; j++)
                            {
                                if (dgv.Columns[j].Visible == true)
                                {
                                    string Value = "";
                                    if (dgv.Rows[i - 1].Cells[j].Value != null)
                                    {
                                        Value = dgv.Rows[i - 1].Cells[j].Value.ToString();
                                    }
                                    //Fix don't separate if it contain '\n' or ','
                                    Value = "\"" + Value.Replace("\"", "\"\"") + "\"";
                                    outputCsv[i] += Value + ",";
                                }
                            }
                        }

                        File.WriteAllLines(saveDialog.FileName, outputCsv, Encoding.UTF8);
                        Cursor = Cursors.Default;
                        MessageBox.Show("Export success!", "Success.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Process.Start(saveDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("Error while export !\n" + ex.Message, "Error exporting plan !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //Button 'ផ្ទៀងផ្ទាត់' : export tab 'ផ្ទៀងផ្ទាត់' (stock in system vs counted Qty) to Excel
        private void BtnExportVerify_Click(object sender, EventArgs e)
        {
            //Switching tab loads the verify data (TabCtrl_SelectedIndexChanged)
            if (tabCtrl.SelectedTab != tabVerify)
            {
                tabCtrl.SelectedTab = tabVerify;
            }
            if (dgvVerify.Rows.Count == 0)
            {
                MessageBox.Show("គ្មានទិន្នន័យសម្រាប់ទាញចេញទេ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Excel file (*.xlsx)|*.xlsx";
            saveDialog.FileName = "MC SD Inventory Verify ( " + DateTime.Now.ToString("dd-MM-yyyy HH_mm") + " ).xlsx";
            if (saveDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string FullPath = saveDialog.FileName;

            //Grid -> array (one write to Excel is much faster than writing cell by cell)
            const int XlFirstDataRow = 6;
            int RowCount = dgvVerify.Rows.Count;
            int LastRow = XlFirstDataRow + RowCount - 1;
            int okCount = 0;
            object[,] Header = new object[1, 8] { { "ល.រ", vcode.HeaderText, vname.HeaderText, vstock.HeaderText, vcount.HeaderText, vbobbin.HeaderText, vdiff.HeaderText, vresult.HeaderText } };
            object[,] Data = new object[RowCount, 8];
            for (int i = 0; i < RowCount; i++)
            {
                DataGridViewRow drow = dgvVerify.Rows[i];
                Data[i, 0] = i + 1;
                Data[i, 1] = CellText(drow, vcode);
                Data[i, 2] = CellText(drow, vname);
                Data[i, 3] = drow.Cells[vstock.Index].Value;
                Data[i, 4] = drow.Cells[vcount.Index].Value;
                Data[i, 5] = drow.Cells[vbobbin.Index].Value;
                Data[i, 6] = drow.Cells[vdiff.Index].Value;
                Data[i, 7] = CellText(drow, vresult);
                if (Convert.ToDouble(drow.Cells[vdiff.Index].Value) == 0)
                {
                    okCount++;
                }
            }
            string Info = "ថ្ងៃទាញ : " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm") + "   •   ទាញដោយ : " + MenuFormV2.UserForNextForm +
                "   •   ប្រភេទ : " + cboType.Text + "   •   លទ្ធផល : " + cboResult.Text;
            if (txtCode.Text.Trim() != "")
            {
                Info += "   •   កូដ : " + txtCode.Text.Trim();
            }
            if (txtName.Text.Trim() != "")
            {
                Info += "   •   ឈ្មោះ : " + txtName.Text.Trim();
            }

            //Sheet 'លម្អិត' : every Active label of the Codes in sheet 'ផ្ទៀងផ្ទាត់' (sum of Qty per Code = 'ចំនួនរាប់បាន')
            HashSet<string> verifyCodes = new HashSet<string>();
            foreach (DataGridViewRow drow in dgvVerify.Rows)
            {
                verifyCodes.Add(CellText(drow, vcode));
            }
            DataTable dtDetail = new DataTable();
            try
            {
                string query = "SELECT * FROM tbSDMCStockInventory WHERE Status = 'Active' AND Code LIKE @Code " +
                    (TypeCondition("Code") == "" ? "" : "AND " + TypeCondition("Code") + " ") +
                    "ORDER BY Code, LabelNo, RegDate";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.SelectCommand.Parameters.AddWithValue("@Code", "%" + txtCode.Text.Trim() + "%");
                sda.Fill(dtDetail);
            }
            catch (Exception ex)
            {
                con.con.Close();
                MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 4 !" + ex.Message, "Error ExportDetail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            con.con.Close();
            List<DataRow> detailRows = dtDetail.Rows.Cast<DataRow>().Where(r => verifyCodes.Contains(r["Code"].ToString())).ToList();
            int DetailCount = detailRows.Count;
            int DetailLastRow = XlFirstDataRow + DetailCount - 1;
            double DetailBobbin = 0;
            double DetailQty = 0;
            object[,] DetailHeader = new object[1, 13] { { "ល.រ", lbno.HeaderText, sysno.HeaderText, this.code.HeaderText, this.name.HeaderText, ttlweight.HeaderText, bobinw.HeaderText,
                bobinQty.HeaderText, Qty.HeaderText, regdate.HeaderText, this.pic.HeaderText, updatedate.HeaderText, regby.HeaderText } };
            object[,] DetailData = new object[Math.Max(DetailCount, 1), 13];
            for (int i = 0; i < DetailCount; i++)
            {
                DataRow r = detailRows[i];
                DetailData[i, 0] = i + 1;
                DetailData[i, 1] = XlNumber(r["LabelNo"]);
                DetailData[i, 2] = r["SysNo"].ToString();
                DetailData[i, 3] = r["Code"].ToString();
                DetailData[i, 4] = r["RMName"].ToString();
                DetailData[i, 5] = XlNumber(r["TotalWeight"]);
                DetailData[i, 6] = XlNumber(r["BobinWeight"]);
                DetailData[i, 7] = XlNumber(r["BobbinQty"]);
                DetailData[i, 8] = XlNumber(r["Qty"]);
                DetailData[i, 9] = XlDate(r["RegDate"]);
                DetailData[i, 10] = r["PIC"].ToString();
                DetailData[i, 11] = XlDate(r["UpdateDate"]);
                DetailData[i, 12] = r["UpdateBy"].ToString();
                DetailBobbin += r["BobbinQty"] == DBNull.Value ? 0 : Convert.ToDouble(r["BobbinQty"]);
                DetailQty += r["Qty"] == DBNull.Value ? 0 : Convert.ToDouble(r["Qty"]);
            }

            string ErrorText = "";
            string OldStatus = LbStatus.Text;
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងទាញទិន្នន័យ . . .";
            LbStatus.Refresh();
            Excel.Application excelApp = null;
            Excel.Workbook xlWorkBook = null;
            Excel.Worksheet worksheet = null;
            Excel.Worksheet wsDetail = null;
            try
            {
                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                xlWorkBook = excelApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                worksheet = (Excel.Worksheet)xlWorkBook.Sheets[1];
                worksheet.Name = "ផ្ទៀងផ្ទាត់";
                worksheet.Cells.Font.Name = "Khmer OS Battambang";
                worksheet.Cells.Font.Size = 10;

                //Title, export info, summary
                Excel.Range rngTitle = worksheet.Range["A1", "H1"];
                rngTitle.Merge();
                rngTitle.Value2 = "ផ្ទៀងផ្ទាត់ស្តុក MC SD : ស្តុកក្នុងប្រព័ន្ធ ទល់នឹង ចំនួនរាប់បាន";
                rngTitle.Font.Size = 14;
                rngTitle.Font.Bold = true;
                rngTitle.RowHeight = 32;
                Excel.Range rngInfo = worksheet.Range["A2", "H2"];
                rngInfo.Merge();
                rngInfo.Value2 = Info;
                rngInfo.Font.Size = 9;
                rngInfo.Font.Italic = true;
                rngInfo.Font.Color = ColorTranslator.ToOle(Color.FromArgb(107, 114, 128));
                rngInfo.RowHeight = 22;
                Excel.Range rngSummary = worksheet.Range["A3", "H3"];
                rngSummary.Merge();
                rngSummary.Value2 = "សរុប : " + RowCount.ToString("N0") + "   •   ត្រឹមត្រូវ : " + okCount.ToString("N0") + "   •   ខុស : " + (RowCount - okCount).ToString("N0");
                rngSummary.Font.Bold = true;
                rngSummary.RowHeight = 24;

                //Header
                Excel.Range rngHeader = worksheet.Range["A" + (XlFirstDataRow - 1), "H" + (XlFirstDataRow - 1)];
                rngHeader.Value2 = Header;
                rngHeader.Font.Bold = true;
                rngHeader.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(185, 209, 234));
                rngHeader.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                //Data (Code stays as text, no number conversion of the codes)
                worksheet.Range["B" + XlFirstDataRow, "B" + LastRow].NumberFormat = "@";
                Excel.Range rngData = worksheet.Range["A" + XlFirstDataRow, "H" + LastRow];
                rngData.Value2 = Data;
                worksheet.Range["A" + XlFirstDataRow, "B" + LastRow].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                worksheet.Range["H" + XlFirstDataRow, "H" + LastRow].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                Excel.Range rngNum = worksheet.Range["D" + XlFirstDataRow, "G" + LastRow];
                rngNum.NumberFormat = "#,##0.##";
                rngNum.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //Whole numbers without the trailing '.' (formula is relative to the active cell -> select the first cell)
                worksheet.Activate();
                worksheet.Range["D" + XlFirstDataRow].Select();
                Excel.FormatCondition fcWhole = (Excel.FormatCondition)rngNum.FormatConditions.Add(Excel.XlFormatConditionType.xlExpression, Type.Missing, "=MOD(D" + XlFirstDataRow + ",1)=0");
                fcWhole.NumberFormat = "#,##0";
                //Red like the grid when Count <> Stock
                Excel.FormatCondition fcDiff = (Excel.FormatCondition)worksheet.Range["G" + XlFirstDataRow, "G" + LastRow].FormatConditions.Add(Excel.XlFormatConditionType.xlCellValue, Excel.XlFormatConditionOperator.xlNotEqual, "=0");
                fcDiff.Font.Color = ColorTranslator.ToOle(Color.Red);
                fcDiff.Font.Bold = true;
                Excel.FormatCondition fcNG = (Excel.FormatCondition)worksheet.Range["H" + XlFirstDataRow, "H" + LastRow].FormatConditions.Add(Excel.XlFormatConditionType.xlCellValue, Excel.XlFormatConditionOperator.xlEqual, "=\"ខុស\"");
                fcNG.Font.Color = ColorTranslator.ToOle(Color.Red);
                fcNG.Font.Bold = true;

                //Borders, column width, row height, filter buttons on the header
                Excel.Range rngTable = worksheet.Range["A" + (XlFirstDataRow - 1), "H" + LastRow];
                rngTable.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                rngTable.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(191, 191, 191));
                rngTable.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                rngTable.Rows.AutoFit();
                double[] ColWidth = { 6, 14, 42, 16, 16, 12, 14, 12 };
                for (int c = 0; c < ColWidth.Length; c++)
                {
                    ((Excel.Range)worksheet.Columns[c + 1]).ColumnWidth = ColWidth[c];
                }
                rngTable.AutoFilter(1);

                FreezeHeader(xlWorkBook, XlFirstDataRow - 1);
                worksheet.Range["A1"].Select();

                //Sheet 'លម្អិត' (added after 'ផ្ទៀងផ្ទាត់', it becomes the active sheet)
                wsDetail = (Excel.Worksheet)xlWorkBook.Worksheets.Add(After: worksheet);
                wsDetail.Name = "លម្អិត";
                wsDetail.Cells.Font.Name = "Khmer OS Battambang";
                wsDetail.Cells.Font.Size = 10;
                Excel.Range rngDTitle = wsDetail.Range["A1", "M1"];
                rngDTitle.Merge();
                rngDTitle.Value2 = "លម្អិតស្តុក MC SD : ឡាប៊ែលដែលបានរាប់";
                rngDTitle.Font.Size = 14;
                rngDTitle.Font.Bold = true;
                rngDTitle.RowHeight = 32;
                Excel.Range rngDInfo = wsDetail.Range["A2", "M2"];
                rngDInfo.Merge();
                rngDInfo.Value2 = Info;
                rngDInfo.Font.Size = 9;
                rngDInfo.Font.Italic = true;
                rngDInfo.Font.Color = ColorTranslator.ToOle(Color.FromArgb(107, 114, 128));
                rngDInfo.RowHeight = 22;
                Excel.Range rngDSummary = wsDetail.Range["A3", "M3"];
                rngDSummary.Merge();
                rngDSummary.Value2 = "សរុប : " + DetailCount.ToString("N0") + "   •   " + bobinQty.HeaderText + " : " + DetailBobbin.ToString("#,##0.##") +
                    "   •   " + vcount.HeaderText + " : " + DetailQty.ToString("#,##0.##");
                rngDSummary.Font.Bold = true;
                rngDSummary.RowHeight = 24;

                Excel.Range rngDHeader = wsDetail.Range["A" + (XlFirstDataRow - 1), "M" + (XlFirstDataRow - 1)];
                rngDHeader.Value2 = DetailHeader;
                rngDHeader.Font.Bold = true;
                rngDHeader.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(185, 209, 234));
                rngDHeader.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                if (DetailCount > 0)
                {
                    //SysNo + Code stay as text
                    wsDetail.Range["C" + XlFirstDataRow, "D" + DetailLastRow].NumberFormat = "@";
                    wsDetail.Range["A" + XlFirstDataRow, "M" + DetailLastRow].Value2 = DetailData;
                    //Label No printed with 4 digits (0001) like on the label
                    wsDetail.Range["B" + XlFirstDataRow, "B" + DetailLastRow].NumberFormat = "0000";
                    wsDetail.Range["A" + XlFirstDataRow, "D" + DetailLastRow].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    wsDetail.Range["J" + XlFirstDataRow, "M" + DetailLastRow].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    Excel.Range rngDNum = wsDetail.Range["F" + XlFirstDataRow, "I" + DetailLastRow];
                    rngDNum.NumberFormat = "#,##0.##";
                    rngDNum.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    wsDetail.Range["F" + XlFirstDataRow].Select();
                    Excel.FormatCondition fcDWhole = (Excel.FormatCondition)rngDNum.FormatConditions.Add(Excel.XlFormatConditionType.xlExpression, Type.Missing, "=MOD(F" + XlFirstDataRow + ",1)=0");
                    fcDWhole.NumberFormat = "#,##0";
                    wsDetail.Range["J" + XlFirstDataRow, "J" + DetailLastRow].NumberFormat = "dd-mm-yyyy hh:mm:ss";
                    wsDetail.Range["L" + XlFirstDataRow, "L" + DetailLastRow].NumberFormat = "dd-mm-yyyy hh:mm:ss";
                }

                Excel.Range rngDTable = wsDetail.Range["A" + (XlFirstDataRow - 1), "M" + Math.Max(DetailLastRow, XlFirstDataRow - 1)];
                rngDTable.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                rngDTable.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(191, 191, 191));
                rngDTable.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                rngDTable.Rows.AutoFit();
                double[] DColWidth = { 6, 12, 16, 14, 40, 12, 12, 12, 14, 20, 14, 20, 14 };
                for (int c = 0; c < DColWidth.Length; c++)
                {
                    ((Excel.Range)wsDetail.Columns[c + 1]).ColumnWidth = DColWidth[c];
                }
                rngDTable.AutoFilter(1);
                FreezeHeader(xlWorkBook, XlFirstDataRow - 1);
                wsDetail.Range["A1"].Select();

                //Open the file on sheet 'ផ្ទៀងផ្ទាត់'
                worksheet.Activate();

                xlWorkBook.SaveAs(FullPath, Excel.XlFileFormat.xlOpenXMLWorkbook);
            }
            catch (Exception ex)
            {
                ErrorText = "Write to Excel : \n" + ex.Message;
            }
            finally
            {
                if (xlWorkBook != null)
                    xlWorkBook.Close(false);
                if (excelApp != null)
                    excelApp.Quit();
                if (wsDetail != null) Marshal.ReleaseComObject(wsDetail);
                if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                if (xlWorkBook != null) Marshal.ReleaseComObject(xlWorkBook);
                if (excelApp != null) Marshal.ReleaseComObject(excelApp);
                //Release the Range objects too, so no hidden EXCEL.EXE stays open
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            if (ErrorText == "")
            {
                //Sheets are saved in tab order : sheet1 = 'ផ្ទៀងផ្ទាត់', sheet2 = 'លម្អិត'
                Dictionary<string, string> TextRanges = new Dictionary<string, string>();
                TextRanges.Add("xl/worksheets/sheet1.xml", "B" + XlFirstDataRow + ":B" + LastRow);
                if (DetailCount > 0)
                {
                    TextRanges.Add("xl/worksheets/sheet2.xml", "C" + XlFirstDataRow + ":D" + DetailLastRow);
                }
                IgnoreNumberAsTextError(FullPath, TextRanges);
            }
            Cursor = Cursors.Default;

            if (ErrorText == "")
            {
                LbStatus.Text = OldStatus;
                Process.Start(FullPath);
            }
            else
            {
                LbStatus.Text = "មានបញ្ហា!";
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //No green "number stored as text" triangle on the Code column (e.g. 01200300).
        //Setting it cell by cell through COM is very slow, so <ignoredErrors> is added to the saved file directly.
        //Only a cosmetic fix: any problem here is ignored.
        //SheetSqref : zip entry of the sheet (xl/worksheets/sheet1.xml) -> range (B6:B100)
        private static void IgnoreNumberAsTextError(string XlsxPath, Dictionary<string, string> SheetSqref)
        {
            try
            {
                using (FileStream fs = new FileStream(XlsxPath, FileMode.Open, FileAccess.ReadWrite))
                using (ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Update))
                {
                    foreach (KeyValuePair<string, string> sheet in SheetSqref)
                    {
                        ZipArchiveEntry entry = zip.GetEntry(sheet.Key);
                        if (entry == null)
                            continue;
                        string xml;
                        using (StreamReader sr = new StreamReader(entry.Open(), Encoding.UTF8))
                            xml = sr.ReadToEnd();
                        if (xml.Contains("<ignoredErrors"))
                            continue;

                        //OOXML element order : <ignoredErrors> comes after <pageMargins>/<pageSetup>/<headerFooter>
                        //and before the first of these elements (or </worksheet>)
                        int start = xml.IndexOf("<pageMargins");
                        if (start < 0)
                            continue;
                        Match m = new Regex("<(smartTags|drawing|legacyDrawing|legacyDrawingHF|drawingHF|picture|oleObjects|controls|webPublishItems|tableParts|extLst)[\\s/>]|</worksheet>").Match(xml, start);
                        if (!m.Success)
                            continue;
                        xml = xml.Insert(m.Index, "<ignoredErrors><ignoredError sqref=\"" + sheet.Value + "\" numberStoredAsText=\"1\"/></ignoredErrors>");

                        using (Stream s = entry.Open())
                        {
                            s.SetLength(0);
                            byte[] bytes = new UTF8Encoding(false).GetBytes(xml);
                            s.Write(bytes, 0, bytes.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("IgnoreNumberAsTextError : " + ex.Message);
            }
        }

        //Keep the header visible while scrolling, on the active sheet (cosmetic only: export continues if it fails)
        private static void FreezeHeader(Excel.Workbook xlWorkBook, int HeaderRow)
        {
            try
            {
                Excel.Window xlWindow = xlWorkBook.Windows[1];
                xlWindow.FreezePanes = false;
                xlWindow.SplitRow = HeaderRow;
                xlWindow.SplitColumn = 0;
                xlWindow.FreezePanes = true;
            }
            catch (Exception exFreeze)
            {
                Console.WriteLine("FreezePanes : " + exFreeze.Message);
            }
        }

        //DB value -> Excel cell value (empty cell when NULL)
        private static object XlNumber(object value)
        {
            return value == DBNull.Value ? null : (object)Convert.ToDouble(value);
        }

        private static object XlDate(object value)
        {
            return value == DBNull.Value ? null : (object)Convert.ToDateTime(value).ToOADate();
        }

        private static string CellText(DataGridViewRow drow, DataGridViewColumn col)
        {
            object value = drow.Cells[col.Index].Value;
            return value == null ? "" : value.ToString();
        }

        private void TxtName_TextChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

        private void TxtLabelNo_TextChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

        private void TxtCode_TextChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

        private void DgvSearchResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = dgvSearchResult.Rows[e.RowIndex]; 
            if (row.Cells["status"].Value != null && row.Cells["status"].Value.ToString() == "Deleted") 
            { 
                row.DefaultCellStyle.BackColor = Color.Pink; 
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (tabCtrl.SelectedTab == tabVerify)
            {
                LoadVerify();
            }
            else
            {
                LoadDetail();
            }
        }

        //Tab 'ផ្ទៀងផ្ទាត់' : per Code, stock in system (WIR1) vs counted Qty
        private void LoadVerify()
        {
            dgvVerify.Rows.Clear();
            Cursor = Cursors.WaitCursor;
            DataTable dtVerify = new DataTable();
            try
            {
                //Master Code = Code that SD MC > 0 (same as 'ទិន្នន័យស្តុក MC' >> SD MC) + Code that counted in tbSDMCStockInventory
                string query = "SELECT COALESCE(c.Code, s.Code) AS Code, COALESCE(c.RMName, s.RMDes) AS RMName, " +
                    "ISNULL(s.StockQty, 0) AS StockQty, ISNULL(c.CountQty, 0) AS CountQty, ISNULL(c.BobbinQty, 0) AS BobbinQty " +
                    "FROM (SELECT Code, MAX(RMDes) AS RMDes, SUM(StockValue) AS StockQty FROM tbSDMCAllTransaction " +
                    "      WHERE LocCode = 'WIR1' AND CancelStatus = 0 " +
                    "      AND CAST(RegDate AS date) BETWEEN '2023-11-01' AND CAST(GETDATE() AS date) GROUP BY Code) s " +
                    "FULL OUTER JOIN (SELECT Code, MAX(RMName) AS RMName, SUM(Qty) AS CountQty, SUM(BobbinQty) AS BobbinQty FROM tbSDMCStockInventory " +
                    "      WHERE Status = 'Active' GROUP BY Code) c ON s.Code = c.Code " +
                    "WHERE (s.StockQty > 0 OR c.Code IS NOT NULL) " +
                    "AND COALESCE(c.Code, s.Code) LIKE @Code " +
                    "AND ISNULL(COALESCE(c.RMName, s.RMDes), '') LIKE @Name " +
                    (TypeCondition("COALESCE(c.Code, s.Code)") == "" ? "" : "AND " + TypeCondition("COALESCE(c.Code, s.Code)") + " ") +
                    "ORDER BY COALESCE(c.Code, s.Code)";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.SelectCommand.Parameters.AddWithValue("@Code", "%" + txtCode.Text.Trim() + "%");
                sda.SelectCommand.Parameters.AddWithValue("@Name", "%" + txtName.Text.Trim() + "%");
                sda.Fill(dtVerify);
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 2 !" + ex.Message, "Error SearchVerify", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            int okCount = 0;
            int ngCount = 0;
            foreach (DataRow row in dtVerify.Rows)
            {
                double stock = Convert.ToDouble(row["StockQty"]);
                double count = Convert.ToDouble(row["CountQty"]);
                double diff = Math.Round(count - stock, 2);
                //Filter by Result : 0 = ទាំងអស់, 1 = ត្រឹមត្រូវ, 2 = ខុស
                if ((cboResult.SelectedIndex == 1 && diff != 0) || (cboResult.SelectedIndex == 2 && diff == 0))
                {
                    continue;
                }
                DataGridViewRow dgvRow = dgvVerify.Rows[dgvVerify.Rows.Add()];
                dgvRow.Cells["vcode"].Value = row["Code"].ToString();
                dgvRow.Cells["vname"].Value = row["RMName"].ToString();
                dgvRow.Cells["vstock"].Value = stock;
                dgvRow.Cells["vcount"].Value = count;
                dgvRow.Cells["vbobbin"].Value = Convert.ToDouble(row["BobbinQty"]);
                dgvRow.Cells["vdiff"].Value = diff;
                dgvRow.Cells["vresult"].Value = diff == 0 ? "ត្រឹមត្រូវ" : "ខុស";
                if (diff == 0)
                {
                    okCount++;
                }
                else
                {
                    ngCount++;
                }
                if (diff != 0)
                {
                    dgvRow.Cells["vdiff"].Style.ForeColor = Color.Red;
                    dgvRow.Cells["vresult"].Style.ForeColor = Color.Red;
                }
            }
            dgvVerify.ClearSelection();
            LbStatus.Text = "រកឃើញទិន្នន័យចំនួន " + dgvVerify.Rows.Count.ToString("N0") +
                "  |  ត្រឹមត្រូវ : " + okCount.ToString("N0") +
                "  |  ខុស : " + ngCount.ToString("N0");
            con.con.Close();
            Cursor = Cursors.Default;
        }

        //Tab 'លម្អិត' : every counted label
        private void LoadDetail()
        {
            dgvSearchResult.Rows.Clear();
            DataTable Cond = new DataTable();
            Cond.Columns.Add("cond");
            Cursor = Cursors.WaitCursor;
            if (!string.IsNullOrEmpty(txtLabelNo.Text.Trim()))
            {
                Cond.Rows.Add("LabelNo LIKE '%" + txtLabelNo.Text.Trim() + "%'");
            }
            if (!string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                Cond.Rows.Add("Code LIKE '%" + txtCode.Text.Trim() + "%'");
            }
            if (!string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                Cond.Rows.Add("RMName LIKE '%" + txtName.Text.Trim() + "%'");
            }
            if (TypeCondition("Code") != "")
            {
                Cond.Rows.Add(TypeCondition("Code"));
            }
            string WHERE = "";
            foreach (DataRow row in Cond.Rows)
            {
                if (WHERE == "")
                {
                    WHERE = " WHERE " + row["cond"].ToString();
                }
                else
                {
                    WHERE += " AND " + row["cond"].ToString();
                }
            }
            DataTable dtselect = new DataTable();
            try
            {
                string selectquery = "SELECT * FROM tbSDMCStockInventory " + WHERE + " ORDER BY RegDate, LabelNo";
                SqlDataAdapter sdaselect = new SqlDataAdapter(selectquery, con.con);
                sdaselect.Fill(dtselect);
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 1 !" + ex.Message, "Error SearchAll", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            foreach (DataRow row in dtselect.Rows)
            {
                dgvSearchResult.Rows.Add();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["lbno"].Value = row["LabelNo"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["sysno"].Value = row["SysNo"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["code"].Value = row["Code"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["name"].Value = row["RMName"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["ttlweight"].Value = row["TotalWeight"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["bobinw"].Value = row["BobinWeight"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["bobinQty"].Value = row["BobbinQty"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["Qty"].Value = row["Qty"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["status"].Value = row["Status"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["regdate"].Value = row["RegDate"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["pic"].Value = row["PIC"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["updatedate"].Value = row["UpdateDate"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Cells["regby"].Value = row["UpdateBy"].ToString();
                //Original values (exact RegDate, LabelNo) for re-printing the label
                dgvSearchResult.Rows[dgvSearchResult.RowCount - 1].Tag = row;
            }
            dgvSearchResult.ClearSelection();
            LbStatus.Text = "រកឃើញទិន្នន័យចំនួន " + dgvSearchResult.Rows.Count.ToString("N0");
            con.con.Close();
            Cursor = Cursors.Default;
        }
    }
}
