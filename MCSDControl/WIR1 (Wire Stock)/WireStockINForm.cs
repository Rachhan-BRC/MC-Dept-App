using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    public partial class WireStockINForm : Form
    {
        SqlCommand cmd;
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        double BeforeEdit;
        string fName;

        string ErrorText;

        public WireStockINForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += WireStockINScanForm_Load;

            //Dgv
            this.dgvScanned.CellFormatting += DgvScanned_CellFormatting;
            this.dgvScanned.CellBeginEdit += DgvScanned_CellBeginEdit;
            this.dgvScanned.CellEndEdit += DgvScanned_CellEndEdit;
            this.dgvScanned.CellClick += DgvScanned_CellClick;
            this.dgvScanned.RowsAdded += DgvScanned_RowsAdded;
            this.dgvScanned.RowsRemoved += DgvScanned_RowsRemoved;

            //Btn
            this.btnDelete.Click += BtnDelete_Click;
            this.btnSave.Click += BtnSave_Click;
            this.btnAdd.Click += BtnAdd_Click;
            this.btnNew.Click += BtnNew_Click;
            this.btnPrint.Click += BtnPrint_Click;
        }

        



        //Btn
        private void BtnSave_Click(object sender, EventArgs e)
        {
            DialogResult DSL = MessageBox.Show("តើអ្នកចង់រក្សាទុកទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DSL == DialogResult.Yes)
            {
                SaveToDB();
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult DSL = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនទេ?","Rachhan System",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (DSL == DialogResult.Yes)
            {
                dgvScanned.Rows.RemoveAt(dgvScanned.CurrentCell.RowIndex);
                dgvScanned.Refresh();
                AssignNumber();
                dgvScanned.ClearSelection();
                CheckButtonSaveDelete();
            }
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            WireStockINFormAdd Wsifa = new WireStockINFormAdd(this);
            Wsifa.ShowDialog();
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            this.dgvScanned.Rows.Clear();
            CheckButtonSaveDelete();
            btnAdd.Enabled = true;
            btnAddGRAY.SendToBack();
            btnPrint.Enabled = false;
            btnPrintGRAY.BringToFront();
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;

            //Calc Total by each RM
            DataTable dtTotal = new DataTable();
            dtTotal.Columns.Add("Code");
            dtTotal.Columns.Add("Description");
            dtTotal.Columns.Add("TotalQty");
            dtTotal.AcceptChanges();
            foreach (DataGridViewRow dgvRow in dgvScanned.Rows)
            {
                int FoundDupl = 0;
                string Code = dgvRow.Cells["RMCode"].Value.ToString();
                string Description = dgvRow.Cells["RMName"].Value.ToString();
                double TotalQty = 0;
                //double TotalQty = Convert.ToDouble(dgvRow.Cells["Qty"].Value.ToString());
                foreach (DataRow row in dtTotal.Rows)
                {
                    if (Code == row["Code"].ToString())
                    {
                        FoundDupl++;
                        break;
                    }
                }

                if (FoundDupl == 0)
                {
                    foreach (DataGridViewRow dgvRow2 in dgvScanned.Rows)
                    {
                        if (Code == dgvRow2.Cells["RMCode"].Value.ToString())
                        {
                            TotalQty += Convert.ToDouble(dgvRow2.Cells["Qty"].Value.ToString());
                        }
                    }
                    dtTotal.Rows.Add(Code, Description, TotalQty);
                    dtTotal.AcceptChanges();
                }
            }

            //Print
            var CDirectory = Environment.CurrentDirectory;
            //open excel application and create new workbook
            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook ExcelWB = ExcelApp.Workbooks.Add(Type.Missing);
            Excel.Worksheet ExcelWS = null;
            ExcelApp.Visible = false;
            ExcelWS = ExcelWB.Sheets["Sheet1"];
            ExcelWS = ExcelWB.ActiveSheet;
            ExcelWS.Name = "Rachhan System";

            //Column Header
            foreach (DataColumn col in dtTotal.Columns)
            {
                ExcelWS.Cells[1, (dtTotal.Columns.IndexOf(col) + 1)] = col.ColumnName.ToString();
            }
            //ExcelWS.Cells[1, dtTotal.Columns.Count].Font.Name = "Khmer OS Battambang";
            ExcelWS.Range["A1:C1"].Font.Size = 15;
            ExcelWS.Range["A1:C1"].Font.Bold = true;

            //Set Border & Format
            ExcelWS.Range["A1:C" + (dtTotal.Rows.Count + 1)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            ExcelWS.Range["A2:C" + (dtTotal.Rows.Count + 1)].Font.Size = 13;
            ExcelWS.Range["A2:A" + (dtTotal.Rows.Count + 1)].NumberFormat = "0000";
            ExcelWS.Range["C2:C" + (dtTotal.Rows.Count + 1)].NumberFormat = "#,##0";

            //Auto fit
            ExcelWS.Columns["A:C"].Autofit();

            //Add data
            foreach (DataRow row in dtTotal.Rows)
            {
                ExcelWS.Cells[(dtTotal.Rows.IndexOf(row) + 2), 1] = row["Code"].ToString();
                ExcelWS.Cells[(dtTotal.Rows.IndexOf(row) + 2), 2] = row["Description"].ToString();
                ExcelWS.Cells[(dtTotal.Rows.IndexOf(row) + 2), 3] = row["TotalQty"].ToString();
            }

            //ឆែករកមើល Folder បើគ្មាន => បង្កើត
            string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\KITReceive";
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }

            // Saving the modified Excel file                        
            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
            string file = "KIT_Rec ";
            fName = file + "( " + date + " )";
            ExcelApp.DisplayAlerts = false;
            ExcelWB.SaveAs(SavePath + @"\" + fName, Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, Type.Missing, false, Excel.XlSaveAsAccessMode.xlExclusive, Excel.XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
            ExcelApp.DisplayAlerts = true;
            ExcelWB.Save();
            ExcelWB.Close();
            ExcelApp.Quit();

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
                MessageBox.Show("ព្រីនរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(SavePath + @"\" + fName + ".xlsx");
                fName = "";

            }
            else
            {
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //Dgv
        private void DgvScanned_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                e.CellStyle.ForeColor = Color.Black;
            }
        }
        private void DgvScanned_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                double NewValue = Convert.ToDouble(dgvScanned.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                NewValue = Math.Round(NewValue,0);
                if (NewValue > 0)
                {
                    dgvScanned.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = NewValue;
                }
                else
                {
                    MessageBox.Show("ចំនួនត្រូវតែលើសពី ០!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    dgvScanned.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = BeforeEdit;
                }
            }
            catch
            {
                MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dgvScanned.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = BeforeEdit;
            }
        }
        private void DgvScanned_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            BeforeEdit = Convert.ToDouble(dgvScanned.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
        }
        private void DgvScanned_CellClick(object sender, DataGridViewCellEventArgs e)
        {            
            CheckButtonSaveDelete();
        }
        private void DgvScanned_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            CheckButtonSaveDelete();
        }
        private void DgvScanned_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            CheckButtonSaveDelete();
        }


        private void WireStockINScanForm_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn Col in dgvScanned.Columns)
            {
                if (Col.Index != 2)
                {
                    Col.ReadOnly = true;
                }
            }
        }
        
        //Method
        public void AssignNumber()
        {
            int rowNumber = 1;
            foreach (DataGridViewRow row in dgvScanned.Rows)
            {
                row.HeaderCell.Value = rowNumber.ToString();
                rowNumber = rowNumber + 1;
            }
        }
        private void CheckButtonSaveDelete()
        {
            if (dgvScanned.SelectedCells.Count > 0)
            {
                if (btnPrint.Enabled == false)
                {
                    this.btnDelete.Enabled = true;
                    this.btnDeleteGRAY.SendToBack();
                }
                else
                {
                    this.btnDelete.Enabled = false;
                    this.btnDeleteGRAY.BringToFront();
                }
            }
            else
            {
                this.btnDelete.Enabled = false;
                this.btnDeleteGRAY.BringToFront();
            }

            if (dgvScanned.Rows.Count > 0)
            {
                this.btnSave.Enabled = true;
                this.btnSaveGRAY.SendToBack();
            }
            else
            {
                this.btnSave.Enabled = false;
                this.btnSaveGRAY.BringToFront();
            }

        }
        private void SaveToDB()
        {
            LbStatus.Text = "កំពុងរក្សាទុក . . . .";
            LbStatus.Refresh();
            Cursor = Cursors.WaitCursor;
            string Errors = "";
            DateTime RegNow = DateTime.Now;
            string User = MenuFormV2.UserForNextForm;
            string TransNo = "";

            //Find Last TransNo
            cnn.con.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT SysNo FROM tbSDMCAllTransaction " +
                                                                            "WHERE " +
                                                                            "RegDate=(SELECT MAX(RegDate) FROM tbSDMCAllTransaction WHERE Funct =1) AND " +
                                                                            "SysNo=(SELECT MAX(SysNo) FROM tbSDMCAllTransaction WHERE Funct =1) Group By SysNo", cnn.con);
            DataTable dtTransNo = new DataTable();
            da.Fill(dtTransNo);
            if (dtTransNo.Rows.Count == 0)
            {
                TransNo = "REC0000000001";
            }
            else
            {
                string LastTransNo = dtTransNo.Rows[0][0].ToString();
                double NextTransNo = Convert.ToDouble(LastTransNo.Substring(LastTransNo.Length - 10)) + 1;
                TransNo = "REC" + NextTransNo.ToString("0000000000");

            }

            //Save
            try
            {
                //Add to tbSDMCAllTransaction
                for (int i = 0; i < dgvScanned.Rows.Count; i++)
                {
                    cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks ) " +
                                                   "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs, @Rm)", cnn.con);
                    cmd.Parameters.AddWithValue("@Sn", TransNo);
                    cmd.Parameters.AddWithValue("@Ft", 1);
                    cmd.Parameters.AddWithValue("@Lc", "WIR1");
                    cmd.Parameters.AddWithValue("@Pn", "");
                    cmd.Parameters.AddWithValue("@Rm", dgvScanned.Rows[i].Cells["DocNo"].Value.ToString());
                    cmd.Parameters.AddWithValue("@Cd", dgvScanned.Rows[i].Cells["RMCode"].Value.ToString());
                    cmd.Parameters.AddWithValue("@Rmd", dgvScanned.Rows[i].Cells["RMName"].Value.ToString());
                    cmd.Parameters.AddWithValue("@Rq", Convert.ToDouble(dgvScanned.Rows[i].Cells["Qty"].Value.ToString()));
                    cmd.Parameters.AddWithValue("@Tq", 0);
                    cmd.Parameters.AddWithValue("@Sv", Convert.ToDouble(dgvScanned.Rows[i].Cells["Qty"].Value.ToString()));
                    cmd.Parameters.AddWithValue("@Rd", RegNow);
                    cmd.Parameters.AddWithValue("@Rb", User);
                    cmd.Parameters.AddWithValue("@Cs", 0);
                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                Errors = Errors + "\t" + ex.Message;
            }
            cnn.con.Close();

            if (Errors.Trim() == "")
            {
                Cursor = Cursors.Default;
                btnPrint.Enabled = true;
                btnPrintGRAY.SendToBack();
                btnAdd.Enabled = false;
                btnAddGRAY.BringToFront();
                btnSave.Enabled = false;
                btnSaveGRAY.BringToFront();
                btnDelete.Enabled = false;
                btnDeleteGRAY.BringToFront();
                LbStatus.Text = "រក្សាទុករួចរាល់";
                MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LbStatus.Text = "";
                LbStatus.Refresh();
            }
            else
            {
                LbStatus.Text = "រក្សាទុកមានបញ្ហា";
                //Create txt file for report Errors
                string y = DateTime.Now.ToString("yyyy");
                string m = DateTime.Now.ToString("MM");
                string d = DateTime.Now.ToString("dd");
                string hh = DateTime.Now.ToString("hh");
                string mm = DateTime.Now.ToString("mm");
                string ss = DateTime.Now.ToString("ss");
                string FileName = "Error" + y + m + d + hh + mm + ss;
                string ErrorPath = (Environment.CurrentDirectory).ToString() + @"\ErrorReport\" + FileName + ".txt";
                using (FileStream fs = File.Create(ErrorPath))
                {
                    fs.Write(Encoding.UTF8.GetBytes(Errors), 0, Errors.Length);
                }
                Cursor = Cursors.Default;
                MessageBox.Show("រក្សាទុកមានបញ្ហា! File txt លម្អិតអំពី Error : \n" + ErrorPath, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
