using MachineDeptApp.MsgClass;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    public partial class WireRemainingCalcAndPrintTag : Form
    {
        QuestionMsgClass QMsg = new QuestionMsgClass();
        ErrorMsgClass EMsg = new ErrorMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();

        SQLConnect cnn = new SQLConnect();
        DataTable dtTotal;
        double BeforeEditng;

        string ErrorText;
        
        public WireRemainingCalcAndPrintTag()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Load += WireRemainingCalcAndPrintTag_Load;

            //Button
            this.btnNew.Click += BtnNew_Click;
            this.btnAdd.Click += BtnAdd_Click;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnPrint.Click += BtnPrint_Click;

            //Dgv
            this.dgvInput.RowsAdded += DgvInput_RowsAdded;
            this.dgvInput.RowsRemoved += DgvInput_RowsRemoved;
            this.dgvInput.SelectionChanged += DgvInput_SelectionChanged;
            this.dgvInput.CellFormatting += DgvInput_CellFormatting;
            this.dgvInput.CellClick += DgvInput_CellClick;
            this.dgvBobbinsW.LostFocus += DgvBobbinsW_LostFocus;
            this.dgvBobbinsW.GotFocus += DgvBobbinsW_GotFocus;
            this.dgvBobbinsW.CellClick += DgvBobbinsW_CellClick;
            this.dgvInput.CellBeginEdit += DgvInput_CellBeginEdit;
            this.dgvInput.CellValueChanged += DgvInput_CellValueChanged;

        }

        //dgvInput
        private void DgvInput_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvInput.Columns[e.ColumnIndex].Name == "BobbinW" || dgvInput.Columns[e.ColumnIndex].Name == "TototalW" || dgvInput.Columns[e.ColumnIndex].Name == "LotNo")
            {
                if (dgvInput.Columns[e.ColumnIndex].Name == "BobbinW")
                {
                    if (dgvInput.Rows[e.RowIndex].Cells["BobbinOrReel"].Value.ToString() == "Bobbin")
                    {
                        e.CellStyle.ForeColor = Color.Black;
                        e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambang", 9, FontStyle.Bold);
                    }
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambang", 9, FontStyle.Bold);
                }
            }
        }
        private void DgvInput_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            CheckBtnPrintOrDelete();
        }
        private void DgvInput_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            CheckBtnPrintOrDelete();
        }
        private void DgvInput_SelectionChanged(object sender, EventArgs e)
        {
            CheckBtnPrintOrDelete();
        }
        private void DgvInput_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (dgvInput.Columns[e.ColumnIndex].Name == "BobbinW")
                {
                    if (dgvInput.Rows[e.RowIndex].Cells["BobbinOrReel"].Value.ToString() == "Bobbin")
                    {
                        try
                        {
                            cnn.con.Open();
                            DataTable dtBobbinsW = new DataTable();
                            SqlDataAdapter sda = new SqlDataAdapter("SELECT RMType, BobbinsW FROM tbSDMstBobbinsWeight " +
                                            "WHERE RMType ='" + dgvInput.Rows[e.RowIndex].Cells["RMType"].Value.ToString() + "' ORDER BY BobbinsW ASC", cnn.con);
                            sda.Fill(dtBobbinsW);

                            dgvBobbinsW.Rows.Clear();
                            foreach (DataRow row in dtBobbinsW.Rows)
                            {
                                dgvBobbinsW.Rows.Add(row["BobbinsW"]);
                            }
                            dgvBobbinsW.CurrentCell = dgvBobbinsW.Rows[0].Cells[0];
                            dgvBobbinsW.ClearSelection();
                            if (Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) != 0)
                            {
                                foreach (DataGridViewRow dgvRow in dgvBobbinsW.Rows)
                                {
                                    if (Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()).ToString() == dgvRow.Cells[0].Value.ToString())
                                    {
                                        dgvBobbinsW.CurrentCell = dgvRow.Cells[0];
                                        dgvRow.Cells[0].Selected = true;
                                    }
                                }
                            }
                            System.Drawing.Rectangle oRectangle = dgvInput.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                            int X = oRectangle.X + 7;
                            int Y = oRectangle.Y + 105;
                            dgvBobbinsW.Location = new System.Drawing.Point(X, Y);
                            if (dgvBobbinsW.Rows.Count > 6)
                            {
                                dgvBobbinsW.Columns[0].Width = 102;
                                dgvBobbinsW.Size = new Size(122, 135);
                            }
                            else
                            {
                                dgvBobbinsW.Columns[0].Width = 119;
                                dgvBobbinsW.Size = new Size(122, (22 + dgvBobbinsW.Rows.Count) * dgvBobbinsW.Rows.Count);
                            }
                            dgvBobbinsW.Focus();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        cnn.con.Close();
                    }
                }
            }
        }
        private void DgvInput_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvInput.Columns[e.ColumnIndex].Name == "TototalW")
            {
                BeforeEditng = Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells["TototalW"].Value.ToString());
            }
        }
        private void DgvInput_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvInput.Rows.Count > 0 && e.RowIndex>-1 && e.ColumnIndex>-1)
            {
                //BobbinsW Changed
                if (dgvInput.Columns[e.ColumnIndex].Name == "BobbinW")
                {
                    double BobbinsW = Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    double NetW = Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells["NetW"].Value.ToString());
                    double MOQ = Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells["MOQ"].Value.ToString());
                    double InputTotalW = Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells["TototalW"].Value.ToString());
                    double TotalRemainingQty = Math.Round((InputTotalW - BobbinsW) / NetW * MOQ, 2);
                    dgvInput.Rows[e.RowIndex].Cells["RemainQty"].Value = TotalRemainingQty.ToString("N0");
                }
                //Weigth Changed
                if (dgvInput.Columns[e.ColumnIndex].Name == "TototalW")
                {
                    if (dgvInput.Rows[e.RowIndex].Cells["TototalW"].Value != null)
                    {
                        try
                        {
                            double R3OrWeight = Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells["TototalW"].Value.ToString());
                            if (R3OrWeight > 0)
                            {
                                //Bobbins
                                if (dgvInput.Rows[e.RowIndex].Cells["BobbinOrReel"].Value.ToString() == "Bobbin")
                                {
                                    double BobbinsW = Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells["BobbinW"].Value.ToString());
                                    double MOQ = Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells["MOQ"].Value.ToString());
                                    double NetW = Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells["NetW"].Value.ToString());
                                    double TotalRemainingQty = Math.Round((R3OrWeight - BobbinsW) / NetW * MOQ, 2);
                                    dgvInput.Rows[e.RowIndex].Cells["RemainQty"].Value = TotalRemainingQty.ToString("N0");
                                }
                                //Reil
                                else
                                {
                                    double R2 = Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells["BobbinW"].Value.ToString());
                                    double MOQ = Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells["MOQ"].Value.ToString());
                                    double R1 = Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells["NetW"].Value.ToString());
                                    double TotalRemainingQty = Math.Round(MOQ * (R3OrWeight * R3OrWeight - R2 * R2) / (R1 * R1 - R2 * R2), 2);
                                    dgvInput.Rows[e.RowIndex].Cells["RemainQty"].Value = TotalRemainingQty.ToString("N0");
                                }
                            }
                            else
                            {
                                WMsg.WarningText = "ចំនួននេះត្រូវតែជាចំនួនដែលធំជាង ០ ដាច់ខាត!";
                                WMsg.ShowingMsg();
                                dgvInput.Rows[e.RowIndex].Cells["TototalW"].Value = BeforeEditng;
                            }
                        }
                        catch
                        {
                            EMsg.AlertText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!";
                            EMsg.ShowingMsg();
                            dgvInput.Rows[e.RowIndex].Cells["TototalW"].Value = BeforeEditng;
                        }
                    }
                    else
                    {
                        WMsg.WarningText = "សូមបញ្ចូលចំនួន! មិនអាចទទេបានទេ!";
                        WMsg.ShowingMsg();
                        dgvInput.Rows[e.RowIndex].Cells["TototalW"].Value = BeforeEditng;
                    }
                }
            }
        }
        //dgvBobbinsW
        private void DgvBobbinsW_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                dgvInput.Rows[dgvInput.CurrentCell.RowIndex].Cells["BobbinW"].Value = Convert.ToDouble(dgvBobbinsW.Rows[e.RowIndex].Cells[0].Value.ToString());
                dgvInput.Focus();
            }
        }
        private void DgvBobbinsW_GotFocus(object sender, EventArgs e)
        {
            dgvBobbinsW.BringToFront();
        }
        private void DgvBobbinsW_LostFocus(object sender, EventArgs e)
        {
            dgvBobbinsW.SendToBack();
        }

        //Button
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            WireRemainingCalcAndPrintTagAdd Wrcapta = new WireRemainingCalcAndPrintTagAdd(this);
            Wrcapta.ShowDialog();
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            //Check RemainQty under or Equal 0
            int UnderOrEqual0 = 0;
            foreach (DataGridViewRow DgvRow in dgvInput.Rows)
            {
                if (Convert.ToDouble(DgvRow.Cells["RemainQty"].Value.ToString()) <= 0)
                {
                    UnderOrEqual0 = UnderOrEqual0 + 1;
                }
            }
            if (UnderOrEqual0 == 0)
            {
                PrintOut();
            }
            else
            {
                QMsg.QAText = "ទិន្នន័យខ្លះមាន <ប្រវែង/ចំនួននៅសល់> តូចជាងឬស្មើ ០!\nតើអ្នកចង់បន្ដព្រីនទិន្នន័យទាំងនេះទៀតមែនឬទេ?";
                QMsg.UserClickedYes = false;
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    PrintOut();
                }
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvInput.SelectedCells.Count > 0)
            {
                if (dgvInput.CurrentCell.RowIndex > -1)
                {
                    QMsg.QAText = "តើអ្នកចង់លុបទិន្នន័យនេះមែនឬទេ?";
                    QMsg.UserClickedYes = false;
                    QMsg.ShowingMsg();
                    if (QMsg.UserClickedYes == true)
                    {
                        dgvInput.Rows.RemoveAt(dgvInput.CurrentCell.RowIndex);
                        //Assign new No.
                        foreach (DataGridViewRow DgvRow in dgvInput.Rows)
                        {
                            DgvRow.HeaderCell.Value = (Convert.ToInt32(DgvRow.Index) + 1).ToString();
                        }
                        dgvInput.Refresh();
                        dgvInput.ClearSelection();
                        CheckBtnPrintOrDelete();
                    }
                }
            }
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void WireRemainingCalcAndPrintTag_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn DgvCol in dgvInput.Columns)
            {
                string Text = DgvCol.HeaderText.ToString();
                Text = Text.Replace("/", "\n");
                DgvCol.HeaderText = Text;
                if (DgvCol.Name != "TototalW" && DgvCol.Name != "LotNo")
                {
                    DgvCol.ReadOnly = true;
                }
                //Console.WriteLine(DgvCol.Name);
            }
        }

        //Function
        private void ClearAll()
        {
            dgvInput.Rows.Clear();
            CheckBtnPrintOrDelete();
        }
        private void CheckBtnPrintOrDelete()
        {
            if (dgvInput.Rows.Count > 0)
            {
                btnPrint.Enabled = true;
                btnPrintGRAY.SendToBack();
            }
            else
            {
                btnPrint.Enabled = false;
                btnPrintGRAY.BringToFront();
            }

            if (dgvInput.SelectedCells.Count > 0)
            {
                btnDelete.Enabled = true;
                btnDeleteGRAY.SendToBack();
            }
            else
            {
                btnDelete.Enabled = false;
                btnDeleteGRAY.BringToFront();
            }
        }
        private void CalcTotal()
        {
            dtTotal = new DataTable();
            dtTotal.Columns.Add("Code");
            dtTotal.Columns.Add("Name");
            dtTotal.Columns.Add("BobbinQty");
            dtTotal.Columns.Add("TotalQty");

            foreach (DataGridViewRow DgvRow in dgvInput.Rows)
            {
                int Found = 0;
                string Code = DgvRow.Cells["RMCode"].Value.ToString();
                string ItemName = DgvRow.Cells["RMName"].Value.ToString();
                foreach (DataRow row in dtTotal.Rows)
                {
                    if (row[0].ToString() == DgvRow.Cells["RMCode"].Value.ToString())
                    {
                        Found = Found + 1;
                        break;
                    }
                }

                if (Found == 0)
                {
                    int BobbinsQty = 0;
                    int TotalQty = 0;
                    foreach (DataGridViewRow DgvRow1 in dgvInput.Rows)
                    {
                        if (Code == DgvRow1.Cells["RMCode"].Value.ToString())
                        {
                            BobbinsQty = BobbinsQty + 1;
                            TotalQty = TotalQty + Convert.ToInt32(DgvRow1.Cells["RemainQty"].Value.ToString());
                        }
                    }
                    dtTotal.Rows.Add(Code, ItemName, BobbinsQty, TotalQty);
                }

            }

            //Sort By Code ASC
            DataView dv = dtTotal.DefaultView;
            dv.Sort = "Code ASC";
            dtTotal = dv.ToTable();
        }
        private void PrintOut()
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងព្រីន . . .";
            LbStatus.Refresh();
            string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\Remain Tag";
            string fName = "";
            //ឆែករកមើល Folder បើគ្មាន => បង្កើត
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: Environment.CurrentDirectory.ToString() + @"\Template\RemainingTagTemplate.xlsx", Editable: true);
            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["RachhanSystem"];

            try
            {
                //Count and Insert new Rows
                int paper = dgvInput.Rows.Count;
                worksheet.Range["A1:A7"].EntireRow.Copy();
                for (int i = 1; i < paper; i++)
                {
                    int indexExcel = 7 * i + 1;
                    worksheet.Range["A" + indexExcel].EntireRow.PasteSpecial(XlPasteType.xlPasteAll, XlPasteSpecialOperation.xlPasteSpecialOperationNone, Type.Missing, Type.Missing);

                }

                //Write data to the excel
                foreach (DataGridViewRow DgvRow in dgvInput.Rows)
                {
                    //Bobbins
                    if (DgvRow.Cells["BobbinOrReel"].Value.ToString() == "Bobbin")
                    {
                        string Code = DgvRow.Cells["RMCode"].Value.ToString();
                        string Name = DgvRow.Cells["RMName"].Value.ToString();
                        string RemainUnit = " Pcs";
                        if (DgvRow.Cells["RMType"].Value.ToString() != "Terminal")
                        {
                            RemainUnit = " m";
                        }
                        string Maker = DgvRow.Cells["Maker"].Value.ToString();
                        string MOQ = Convert.ToDouble(DgvRow.Cells["MOQ"].Value.ToString()).ToString("N0");
                        string RemainQty = Convert.ToDouble(DgvRow.Cells["RemainQty"].Value.ToString()).ToString("N0");
                        double R3OrWeigth = Convert.ToDouble(DgvRow.Cells["TototalW"].Value.ToString());
                        double BobbinWeigth = Convert.ToDouble(DgvRow.Cells["BobbinW"].Value.ToString());
                        string LotNo = DgvRow.Cells["LotNo"].Value.ToString();

                        worksheet.Cells[7 * DgvRow.Index + 1, 3] = Code;
                        worksheet.Cells[7 * DgvRow.Index + 2, 3] = Name;
                        worksheet.Cells[7 * DgvRow.Index + 3, 3] = Maker;
                        worksheet.Cells[7 * DgvRow.Index + 4, 3] = MOQ + RemainUnit;
                        worksheet.Cells[7 * DgvRow.Index + 5, 3] = RemainQty + RemainUnit + " ( " + R3OrWeigth + " KG = "+ (R3OrWeigth-BobbinWeigth).ToString() + "+"+BobbinWeigth.ToString() +" )";
                        worksheet.Cells[7 * DgvRow.Index + 6, 3] = LotNo;
                        worksheet.Cells[7 * DgvRow.Index + 7, 3] = DateTime.Now;

                    }
                    //Reil
                    else
                    {
                        string Code = DgvRow.Cells["RMCode"].Value.ToString();
                        string Name = DgvRow.Cells["RMName"].Value.ToString();
                        string RemainUnit = " Pcs";
                        if (DgvRow.Cells["RMType"].Value.ToString() != "Terminal")
                        {
                            RemainUnit = " m";
                        }
                        string Maker = DgvRow.Cells["Maker"].Value.ToString();
                        string MOQ = Convert.ToDouble(DgvRow.Cells["MOQ"].Value.ToString()).ToString("N0");
                        string RemainQty = Convert.ToDouble(DgvRow.Cells["RemainQty"].Value.ToString()).ToString("N0");
                        double R3OrWeigth = Convert.ToDouble(DgvRow.Cells["TototalW"].Value.ToString());
                        double BobbinWeigth = Convert.ToDouble(DgvRow.Cells["BobbinW"].Value.ToString());


                        string LotNo = DgvRow.Cells[10].Value.ToString();

                        worksheet.Cells[7 * DgvRow.Index + 1, 3] = Code;
                        worksheet.Cells[7 * DgvRow.Index + 2, 3] = Name;
                        worksheet.Cells[7 * DgvRow.Index + 3, 3] = Maker;
                        worksheet.Cells[7 * DgvRow.Index + 4, 3] = MOQ + RemainUnit;
                        worksheet.Cells[7 * DgvRow.Index + 5, 3] = RemainQty + RemainUnit + " ( " + R3OrWeigth + " mm = " + (R3OrWeigth - BobbinWeigth).ToString() + "+" + BobbinWeigth.ToString() + " )";
                        worksheet.Cells[7 * DgvRow.Index + 6, 3] = LotNo;
                        worksheet.Cells[7 * DgvRow.Index + 7, 3] = DateTime.Now;
                    }
                }

                //Calc Total By each Items
                CalcTotal();
                //Insert if more than 1
                Excel.Worksheet worksheetTotal = (Excel.Worksheet)xlWorkBook.Sheets["Total"];
                if (dtTotal.Rows.Count > 1)
                {
                    worksheetTotal.Range["3:" + (dtTotal.Rows.Count + 1)].Insert();
                    worksheetTotal.Range["A3:D" + (dtTotal.Rows.Count + 1)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                }
                //Write data to the excel
                for (int i = 0; i < dtTotal.Rows.Count; i++)
                {
                    for (int j = 0; j < dtTotal.Columns.Count; j++)
                    {
                        worksheetTotal.Cells[i + 2, j + 1] = dtTotal.Rows[i][j].ToString();
                    }
                }

                // Saving the modified Excel file                        
                string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                string file = "Remain Tag ";
                fName = file + "( " + date + " )";
                worksheet.SaveAs(SavePath + @"\" + fName + ".xlsx");

            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }

            //Close Worksbook
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
                LbStatus.Text = "ឯកសារ Excel រួចរាល់!";
                LbStatus.Refresh();
                InfoMsg.InfoText = "ឯកសារ Excel រួចរាល់!";
                InfoMsg.ShowingMsg();
                btnNew.PerformClick();
                System.Diagnostics.Process.Start(SavePath + @"\" + fName + ".xlsx");
                LbStatus.Text = "";
                LbStatus.Refresh();
            }
            else
            {
                LbStatus.Text = "មានបញ្ហា!";
                LbStatus.Refresh();
                EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                EMsg.ShowingMsg();
            }

        }
    }
}
