using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.MCPlans
{
    public partial class GenerateMCPlansForm : Form
    {
        SQLConnectOBS cnnOBS= new SQLConnectOBS();
        DataTable dtCombineAndCountParent;        
        DataTable dtChildData;
        DataTable dtChildNotFound;
        DataTable dtCodeNotFound;
        DataTable dtExcelCodeIndex;

        string CurrentExcelDir = "";
        string CurrentPass = "";
        string NewExcelDir = "";
        string NewPass = "";

        public GenerateMCPlansForm()
        {
            InitializeComponent();
            this.cnnOBS.Connection();
            this.FormClosing += GenerateMCPlansForm_FormClosing;


        }

        private void Cleardt()
        {
            dtCombineAndCountParent=new DataTable();
            dtCombineAndCountParent.Columns.Add("POSParentCode");
            dtCombineAndCountParent.Columns.Add("Count");

            dtChildData = new DataTable();
            dtChildData.Columns.Add("NoOfPOSParent");
            dtChildData.Columns.Add("POSParent"); 
            dtChildData.Columns.Add("POSParentCode");
            dtChildData.Columns.Add("POSParentQty");
            dtChildData.Columns.Add("POSParentShipDate");
            dtChildData.Columns.Add("ItemCode");
            dtChildData.Columns.Add("TotalQty");

            dtChildNotFound = new DataTable();
            dtChildNotFound.Columns.Add("POSParent");

            dtCodeNotFound = new DataTable();
            dtCodeNotFound.Columns.Add("Code");

        }

        private void WriteToExcelFile()
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: txtExcelDirectory.Text.ToString() + @"", Editable: true);
            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Mtr.data"];
            Excel.Range xlRange;

            try
            {                
                xlRange = worksheet.UsedRange;
                worksheet.Unprotect(txtPassword.Text.ToString());

                int indexOfWritingDate = 0;
                
                for (int i = 34; i <= xlRange.Columns.Count; i++)
                {
                    string ExcelDate = (xlRange.Cells[4, i].Value).ToString();
                    if (DtStart.Value.ToString("dd-MM-yyyy") == Convert.ToDateTime(ExcelDate).ToString("dd-MM-yyyy"))
                    {
                        indexOfWritingDate = i;
                                                
                    }
                }

                if (indexOfWritingDate != 0)
                {
                    //Read Index by each Code, Easy to write into excel faster
                    dtExcelCodeIndex = new DataTable();
                    dtExcelCodeIndex.Columns.Add("Code");
                    dtExcelCodeIndex.Columns.Add("Index");

                    int CodeIndexInExcel = 0;
                    for (int i = 1; i < 34; i++)
                    {
                        string ColHeader = xlRange.Cells[4, i].Text;
                        if (ColHeader == "WIP-Code")
                        {
                            CodeIndexInExcel = i;
                        }
                    }

                    if (CodeIndexInExcel != 0)
                    {
                        for (int i = 5; i <= xlRange.Rows.Count; i++)
                        {
                            string Code = xlRange.Cells[i, CodeIndexInExcel].Text;
                            if (Code.Trim() != "")
                            {
                                dtExcelCodeIndex.Rows.Add(Code, i);
                            }
                        }

                        foreach (DataRow row in dtChildData.Rows)
                        {

                            //Find Code index in excel through dtExcelCodeIndex
                            int indexOfWritingCode = 0;
                            foreach (DataRow rowIndex in dtExcelCodeIndex.Rows)
                            {
                                if (rowIndex[0].ToString().Trim() == row[5].ToString().Trim())
                                {
                                    indexOfWritingCode = Convert.ToInt32(rowIndex[1].ToString());
                                    break;
                                }
                            }

                            //If found Write into Excel
                            if (indexOfWritingCode != 0)
                            {
                                //Check if already have cmt >> delete
                                if (worksheet.Cells[indexOfWritingCode, (indexOfWritingDate + Convert.ToInt32(row[0]))].Comment != null)
                                {
                                    worksheet.Cells[indexOfWritingCode, (indexOfWritingDate + Convert.ToInt32(row[0]))].Comment.Delete();
                                }
                                //Insert Qty
                                worksheet.Cells[indexOfWritingCode, (indexOfWritingDate + Convert.ToInt32(row[0]))] = row[6];
                                //Insert cmt
                                worksheet.Cells[indexOfWritingCode, (indexOfWritingDate + Convert.ToInt32(row[0]))].AddComment(row[1] + "\n" + Convert.ToDateTime(row[4]).ToString("dd-MM-yy") + "\n" + Convert.ToDouble(row[3]).ToString("N0"));
                            }
                            else
                            {
                                dtCodeNotFound.Rows.Add(row[5]);
                            }
                        }

                        if (dtCodeNotFound.Rows.Count == 0)
                        {

                            worksheet.Protect(txtPassword.Text.ToString(), AllowFiltering: true);
                            xlWorkBook.Save();
                            xlWorkBook.Close();
                            excelApp.Quit();



                            LbStatus.Text = "រក្សាទិន្នន័យទុកក្នុងឯកសារ Excel រួចរាល់!";
                            MessageBox.Show("រក្សាទិន្នន័យទុកក្នុងឯកសារ Excel រួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvExcelData.Rows.Clear();
                            Cleardt();
                            CheckBtnSave();
                        }
                        else
                        {
                            excelApp.DisplayAlerts = false;
                            xlWorkBook.Close(false);
                            excelApp.DisplayAlerts = true;
                            excelApp.Quit();
                            string MsgBox = "រក្សាទិន្នន័យទុកក្នុងឯកសារ Excel មានបញ្ហា!\nរកមិនឃើញកូដទាំងនេះទេ៖";
                            foreach (DataRow row in dtCodeNotFound.Rows)
                            {
                                MsgBox = MsgBox + "\n" + row[0].ToString();
                            }
                            LbStatus.Text = "រក្សាទិន្នន័យទុកក្នុងឯកសារ Excel មានបញ្ហា!";
                            MessageBox.Show(MsgBox, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        excelApp.DisplayAlerts = false;
                        xlWorkBook.Close(false);
                        excelApp.DisplayAlerts = true;
                        excelApp.Quit();
                        LbStatus.Text = "រក្សាទិន្នន័យទុកក្នុងឯកសារ Excel មានបញ្ហា!";
                        MessageBox.Show("រក្សាទិន្នន័យទុកក្នុងឯកសារ Excel មានបញ្ហា!\nមិនអាចស្វែងរកឃើញជួរ 'WIP-Code' ក្នុង Excel ទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    excelApp.DisplayAlerts = false;
                    xlWorkBook.Close(false);
                    excelApp.DisplayAlerts = true;
                    excelApp.Quit();
                    LbStatus.Text = "រក្សាទិន្នន័យទុកក្នុងឯកសារ Excel មានបញ្ហា!";
                    MessageBox.Show("Can't Find this date( "+DtStart.Value.ToString("dd-MM-yyyy")+" ) in this sheet 'Mtr.data' !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            catch(Exception ex)
            {
                excelApp.DisplayAlerts = false;
                xlWorkBook.Close(false);
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
                LbStatus.Text = "រក្សាទិន្នន័យទុកក្នុងឯកសារ Excel មានបញ្ហា!";
                MessageBox.Show(ex.Message,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
                
            }
        }

        private void FindChildPOS()
        {
            Cleardt();
            if (dgvExcelData.Rows.Count > 0)
            {
                foreach (DataGridViewRow dgvRow in dgvExcelData.Rows)
                {
                    string POSParent = dgvRow.Cells[2].Value.ToString();
                    string POSParentCode = dgvRow.Cells[0].Value.ToString();
                    double POSParentQty = Convert.ToDouble(dgvRow.Cells[3].Value.ToString());
                    DateTime POSDelDate = Convert.ToDateTime(dgvRow.Cells[4].Value.ToString());

                    try
                    {
                        cnnOBS.conOBS.Open();
                        //Search for SemiPress1
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT t1.ItemCode, t1.PlanQty  FROM( "+
                                                                                            "(SELECT ProductNo, ItemCode, PlanQty FROM prgproductionorder WHERE LineCode = 'MC1') t1 "+
                                                                                            "INNER JOIN "+
                                                                                            "(SELECT ProductNo, DONo, ItemCode, PlanQty, POSDeliveryDate FROM prgproductionorder "+
                                                                                            "WHERE DONo = '" + POSParent +"') t2 "+
                                                                                            "ON t2.ProductNo = t1.ProductNo)", cnnOBS.conOBS);
                        DataTable dtTemp = new DataTable();
                        sda.Fill(dtTemp);
                        if (dtTemp.Rows.Count > 0)
                        {
                            double Count = 0;
                            if (dtCombineAndCountParent.Rows.Count == 0)
                            {
                                dtCombineAndCountParent.Rows.Add(POSParentCode, 1);
                            }
                            else
                            {
                                int Found = 0;
                                foreach (DataRow row in dtCombineAndCountParent.Rows)
                                {
                                    if (row[0].ToString() == POSParentCode)
                                    {
                                        Count = Convert.ToDouble(row[1].ToString());
                                        row[1] = Convert.ToDouble(row[1].ToString())+1;
                                        Found = Found+1;
                                        break;
                                    }
                                }                                
                                if (Found == 0)
                                {
                                    dtCombineAndCountParent.Rows.Add(POSParentCode, 1);
                                }
                            }
                            
                            foreach (DataRow row in dtTemp.Rows)
                            {
                                dtChildData.Rows.Add(Count, POSParent, POSParentCode, POSParentQty, POSDelDate, row[0], Convert.ToDouble(row[1]));
                            }
                        }
                        else
                        {
                            dtChildNotFound.Rows.Add(dgvRow.Cells[2].Value.ToString());

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cnnOBS.conOBS.Close();
                }

            }

        }

        private void GenerateMCPlansForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            NewExcelDir=txtExcelDirectory.Text;
            NewPass = txtPassword.Text;
            if (NewExcelDir == CurrentExcelDir && NewPass == CurrentPass)
            {

            }
            else
            {
                DialogResult DLS = MessageBox.Show("តើអ្នករក្សាទុក <ទីតាំងឯកសារ> និង <ពាក្យសម្ងាត់> ថ្មីនេះដែរឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    Properties.Settings.Default.FileExcelDirectory = NewExcelDir;
                    Properties.Settings.Default.PasswordExcel = NewPass;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void CheckBtnSave()
        {
            if (dgvExcelData.Rows.Count > 0)
            {
                btnSave.Enabled = true;
                btnSave.BackColor= Color.White;
            }
            else
            {
                btnSave.Enabled = false;
                btnSave.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
            }
        }

        private void AssignNumberToDgv()
        {
            int rowNumber = 1;
            foreach (DataGridViewRow row in dgvExcelData.Rows)
            {
                row.HeaderCell.Style.BackColor = Color.FromKnownColor(KnownColor.GradientActiveCaption);
                row.HeaderCell.Style.Font = new Font("Khmer OS Battambang",9, FontStyle.Regular);
                row.HeaderCell.Value = rowNumber.ToString();
                rowNumber = rowNumber + 1;
            }
            dgvExcelData.ClearSelection();
        }

        private void ReadExcelData()
        {
            LbStatus.Visible = false;
            LbStatus.Text = "កំពុងអានទិន្នន័យ . . .";
            dgvExcelData.Rows.Clear();
            CheckBtnSave();
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet;
            Microsoft.Office.Interop.Excel.Range xlRange;

            int xlRow;
            string strFileName;
            OpenFileDialog openFD = new OpenFileDialog();
            openFD.Filter = "Excel|*.xls;*.xlsx;";
            openFD.ShowDialog();
            strFileName = openFD.FileName;

            if (strFileName != "")
            {
                Cursor = Cursors.WaitCursor;
                LbStatus.Visible = true;
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkbook = xlApp.Workbooks.Open(strFileName);

                try
                {
                    xlWorksheet = xlWorkbook.Worksheets["MC"];
                    xlRange = xlWorksheet.UsedRange;

                    int xlDataRange = 5;
                    for (xlRow = 5; xlRow <= xlRange.Rows.Count; xlRow++)
                    {
                        string Code = "";
                        string Items = "";
                        string POS = "";

                        Code = xlRange.Cells[xlRow, 2].Text;
                        Items = xlRange.Cells[xlRow, 3].Text;
                        POS = xlRange.Cells[xlRow, 4].Text;

                        if (Code.Trim() != "" && Items.Trim() != "" && POS.Trim() != "")
                        {
                            xlDataRange = xlDataRange + 1;
                        }
                        else
                        {
                            break;
                        }
                    }

                    for (xlRow = 5; xlRow < xlDataRange; xlRow++)
                    {

                        dgvExcelData.Rows.Add(xlRange.Cells[xlRow, 2].Text,
                                                            xlRange.Cells[xlRow, 3].Text,
                                                            xlRange.Cells[xlRow, 4].Text,
                                                            Convert.ToDouble(xlRange.Cells[xlRow, 5].Text),
                                                            Convert.ToDateTime(xlRange.Cells[xlRow, 6].Text));

                    }
                    
                    //close excel after import to dgv success
                    xlApp.DisplayAlerts = false;
                    xlWorkbook.Close();
                    xlApp.Quit();
                    xlApp.DisplayAlerts = true;

                    //Kill all Excel background process
                    var processes = from p in Process.GetProcessesByName("EXCEL")
                                    select p;
                    foreach (var process in processes)
                    {
                        if (process.MainWindowTitle.ToString().Trim() == "")
                            process.Kill();
                    }
                    AssignNumberToDgv();
                    Cursor = Cursors.Default;
                    CheckBtnSave();
                    MessageBox.Show("ការអានរួចរាល់ !", "Rachhan Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LbStatus.Text = "ការអានរួចរាល់ !";
                    btnSave.Focus();
                }
                catch(Exception ex)
                {
                    LbStatus.Text = "ការអានបរាជ័យ !";
                    Cursor = Cursors.Default;
                    MessageBox.Show("Can't find sheet name : MC !\n"+ex.Message, "Rachhan Application", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    xlApp.DisplayAlerts = false;
                    xlWorkbook.Close();
                    xlApp.Quit();
                    xlApp.DisplayAlerts = true;
                }
            }
        }

        private void btnChoosingExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFD = new OpenFileDialog();
            openFD.Filter = "Excel|*.xls;*.xlsx;";
            openFD.ShowDialog();
            txtExcelDirectory.Text = openFD.FileName;
        }

        private void GenerateMCPlansForm_Load(object sender, EventArgs e)
        {
            CurrentExcelDir= Properties.Settings.Default.FileExcelDirectory.ToString();
            CurrentPass = Properties.Settings.Default.PasswordExcel.ToString();
            txtExcelDirectory.Text = CurrentExcelDir;
            txtPassword.Text=CurrentPass;
            Cleardt();

        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            ReadExcelData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult DLS = MessageBox.Show("តើអ្នកចង់ចាប់ផ្ដើមគណនា, បញ្ចូលនិងរក្សាទុកក្នុង\nឯកសារ Excel មែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DLS == DialogResult.Yes)
            {
                LbStatus.Visible = false;
                LbStatus.Text = "កំពុងគណនា, បញ្ចូលនិងរក្សាទុក . . .";
                LbStatus.Visible = true;
                Cursor = Cursors.WaitCursor;

                FindChildPOS();
                if (dtChildNotFound.Rows.Count == 0)
                {
                    WriteToExcelFile();
                    Cursor = Cursors.Default;
                    btnImport.Focus();
                }
                else
                {
                    string MsgBox = "គ្មានទិន្នន័យ POS ទាំងនេះទេ៖";
                    foreach (DataRow row in dtChildNotFound.Rows)
                    {
                        MsgBox = MsgBox + "\n • " + row[0].ToString();
                    }
                    Cursor = Cursors.Default;
                    MessageBox.Show(MsgBox, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

    }
}
