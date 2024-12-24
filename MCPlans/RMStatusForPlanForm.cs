using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.MCPlans
{
    public partial class RMStatusForPlanForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SqlCommand cmd;
        DataTable dtChild;
        DataTable dtColor;
        string ReadNotFound;
        DateTime BeforeEdit;


        DataTable dtCombineAndCountParentExcelMtr;
        DataTable dtChildDataExcelMtr;
        DataTable dtChildNotFoundExcelMtr;
        DataTable dtCodeNotFoundExcelMtr;
        DataTable dtExcelCodeIndexExcelMtr;


        string CurrentExcelDirExcelMtr = "";
        string CurrentPassExcelMtr = "";
        string NewExcelDirExcelMtr = "";
        string NewPassExcelMtr = "";

        

        public RMStatusForPlanForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.FormClosing += RMStatusForPlanForm_FormClosing;
            this.btnImport.Click += BtnImport_Click;
            this.dgvExcelData.CellClick += DgvExcelData_CellClick;
            this.Load += RMStatusForPlanForm_Load;
            this.dgvPosChild.CellFormatting += DgvPosChild_CellFormatting;
            this.dgvPosChild.CellBeginEdit += DgvPosChild_CellBeginEdit;
            this.dgvPosChild.CellEndEdit += DgvPosChild_CellEndEdit;
            this.btnSave.Click += BtnSave_Click;
            this.btnChoosingExcel.Click += BtnChoosingExcel_Click;

        }

        private void BtnChoosingExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFD = new OpenFileDialog();
            openFD.Filter = "Excel|*.xls;*.xlsx;";
            openFD.ShowDialog();
            txtExcelDirectory.Text = openFD.FileName;
        }

        private void RMStatusForPlanForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            NewExcelDirExcelMtr = txtExcelDirectory.Text;
            NewPassExcelMtr = txtPassword.Text;
            if (NewExcelDirExcelMtr == CurrentExcelDirExcelMtr && NewPassExcelMtr == CurrentPassExcelMtr)
            {

            }
            else
            {
                DialogResult DLS = MessageBox.Show("តើអ្នករក្សាទុក <ទីតាំងឯកសារ> និង <ពាក្យសម្ងាត់> ថ្មីនេះដែរឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    Properties.Settings.Default.FileExcelDirectory = NewExcelDirExcelMtr;
                    Properties.Settings.Default.PasswordExcel = NewPassExcelMtr;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void DgvPosChild_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == dgvPosChild.Columns.Count - 1)
            {
                BeforeEdit = Convert.ToDateTime(dgvPosChild[e.ColumnIndex, e.RowIndex].Value);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (dgvExcelData.Rows.Count > 0)
            {
                SaveToDB();
            }
        }

        private void DgvPosChild_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvPosChild.Columns.Count - 1)
            {
                try
                {
                    DateTime newEdit = Convert.ToDateTime(dgvPosChild[e.ColumnIndex, e.RowIndex].Value);
                    dgvPosChild[e.ColumnIndex, e.RowIndex].Value = newEdit;
                    foreach (DataRow row in dtChild.Rows)
                    {
                        if (row[3].ToString() == dgvPosChild.Rows[e.RowIndex].Cells[2].Value.ToString())
                        {
                            row[e.ColumnIndex + 1] = newEdit;
                        }
                    }
                }
                catch 
                {
                    MessageBox.Show("ទិន្នន័យដែលអ្នកបញ្ចូលមិនមែនជាទិន្នន័យដែលជា ថ្ងៃខែឆ្នាំ ទេ!","Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    dgvPosChild[e.ColumnIndex, e.RowIndex].Value = BeforeEdit;
                }
            }
            else
            {
                foreach (DataRow row in dtChild.Rows)
                {
                    if (row[3].ToString() == dgvPosChild.Rows[e.RowIndex].Cells[2].Value.ToString())
                    {
                        row[e.ColumnIndex + 1] = dgvPosChild[e.ColumnIndex, e.RowIndex].Value;
                    }
                }
            }
            
        }

        private void DgvPosChild_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvPosChild.Columns.Count-1)
            {
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambong", 9, FontStyle.Regular);
            }
            if (e.ColumnIndex == dgvPosChild.Columns.Count - 2)
            {
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambong", 9, FontStyle.Regular);
            }

            if (e.ColumnIndex == 4 && e.Value.ToString() != "")
            {
                foreach (DataRow row in dtColor.Rows)
                {
                    if (e.Value.ToString().Contains(row[0].ToString()))
                    {
                        e.CellStyle.ForeColor = Color.FromName(row[1].ToString());
                        e.CellStyle.BackColor = Color.FromName(row[2].ToString());
                    }
                }
            }
        }

        private void RMStatusForPlanForm_Load(object sender, EventArgs e)
        {
            dtColor = new DataTable();
            dtColor.Columns.Add("ShortText");
            dtColor.Columns.Add("ColorText");
            dtColor.Columns.Add("BackColor");
            dtColor.Rows.Add("RED", "White", "Red");
            dtColor.Rows.Add("BLK", "White", "Black");
            dtColor.Rows.Add("PINK", "Black", "Pink");
            dtColor.Rows.Add("YEL", "Black", "Yellow");
            dtColor.Rows.Add("BLU", "White", "Blue");
            dtColor.Rows.Add("BRN", "White", "Brown");
            dtColor.Rows.Add("G/Y", "Black", "GreenYellow");
            dtColor.Rows.Add("GRN", "White", "Green");
            dtColor.Rows.Add("GRY", "White", "Gray");
            dtColor.Rows.Add("ORG", "Black", "Orange");
            dtColor.Rows.Add("PNK", "Black", "Pink");
            dtColor.Rows.Add("SKY", "Black", "SkyBlue");
            dtColor.Rows.Add("VLT", "White", "Purple");
            dtColor.Rows.Add("WHT", "Black", "White");

            //Set Frezze to column
            for (int i = 0; i < 3; i++)
            {
                dgvPosChild.Columns[i].Frozen=true;
            }

            //Read only all except only Remark & KIT
            for (int i = 0; i < dgvPosChild.Columns.Count - 2; i++)
            {
                dgvPosChild.Columns[i].ReadOnly = true;
            }

            CurrentExcelDirExcelMtr = Properties.Settings.Default.FileExcelDirectory.ToString();
            CurrentPassExcelMtr = Properties.Settings.Default.PasswordExcel.ToString();
            txtExcelDirectory.Text = CurrentExcelDirExcelMtr;
            txtPassword.Text = CurrentPassExcelMtr;

        }

        private void DgvExcelData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvExcelData.SelectedCells.Count > 0)
            {
                dgvPosChild.Rows.Clear();
                foreach (DataRow row in dtChild.Rows)
                {
                    try
                    {
                        if (row[0].ToString() == dgvExcelData.Rows[dgvExcelData.CurrentCell.RowIndex].Cells[2].Value.ToString())
                        {
                            if (row[6] != DBNull.Value)
                            {
                                dgvPosChild.Rows.Add(row[1], row[2], row[3], row[4], row[5], row[6], Convert.ToDouble(row[7]), Convert.ToDouble(row[8]), row[9], Convert.ToDateTime(row[10]));

                            }
                            else
                            {
                                dgvPosChild.Rows.Add(row[1], row[2], row[3], row[4], row[5], row[6], Convert.ToDouble(row[7]), Convert.ToDouble(row[8]), row[9], DBNull.Value);

                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                dgvPosChild.ClearSelection();
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            
            ReadExcelData();
        }

        private void CheckBtnSave()
        {
            if (dgvExcelData.Rows.Count > 0)
            {
                ReadNotFound = "";
                int NotFound = 0;

                foreach (DataGridViewRow dgvRow in dgvExcelData.Rows)
                {
                    int Check = 0;
                    foreach (DataRow row in dtChild.Rows)
                    {
                        if (dgvRow.Cells[2].Value.ToString() == row[0].ToString())
                        {
                            Check = Check + 1;
                        }
                    }
                    if (Check == 0)
                    {
                        if (ReadNotFound.Trim() == "")
                        {
                            ReadNotFound = "• " + dgvRow.Cells[2].Value.ToString();
                        }
                        else
                        {
                            ReadNotFound = ReadNotFound + "\n• " + dgvRow.Cells[2].Value.ToString();
                        }
                        NotFound = NotFound + 1;
                    }
                }

                if (NotFound == 0)
                {
                    btnSave.Enabled = true;
                    btnSave.BackColor = Color.White;
                }
                else
                {
                    btnSave.Enabled = false;
                    btnSave.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
                }
            }
            else
            {
                btnSave.Enabled = false;
                btnSave.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
            }            
        }

        private void ReadExcelData()
        {
            LbStatus.Visible = false;
            LbStatus.Text = "កំពុងអានទិន្នន័យ . . .";
            dgvExcelData.Rows.Clear();
            dgvPosChild.Rows.Clear();
            btnSave.Enabled = false;
            btnSave.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
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
                    xlWorksheet = xlWorkbook.Worksheets[1];
                    xlRange = xlWorksheet.UsedRange;

                    int xlDataRange = 5;
                    string DocNo = "";
                    for (xlRow = 5; xlRow <= xlRange.Rows.Count; xlRow++)
                    {
                        string Code = "";
                        string Items = "";
                        string POS = "";

                        DocNo= xlRange.Cells[2, 11].Text;
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
                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(dgvExcelData);
                        row.Cells[0].Value = xlRange.Cells[xlRow, 2].Text;
                        row.Cells[1].Value = xlRange.Cells[xlRow, 3].Text;
                        row.Cells[2].Value = xlRange.Cells[xlRow, 4].Text;
                        row.Cells[3].Value = Convert.ToDouble(xlRange.Cells[xlRow, 5].Text);
                        row.Cells[4].Value = Convert.ToDateTime(xlRange.Cells[xlRow, 6].Text);
                        row.Cells[5].Value = Convert.ToDateTime(xlRange.Cells[xlRow, 9].Text);
                        row.Cells[7].Value = DocNo;
                        row.HeaderCell.Style.BackColor = Color.FromKnownColor(KnownColor.GradientActiveCaption);
                        row.HeaderCell.Style.Font = new Font("Khmer OS Battambang", 9, FontStyle.Regular);
                        row.HeaderCell.Value = (xlRow - 4).ToString();
                        dgvExcelData.Rows.Add(row);

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

                    CheckOBSandKIT();
                    Cursor = Cursors.Default;
                    CheckBtnSave();
                    if (ReadNotFound.Trim() == "")
                    {
                        MessageBox.Show("ការអានរួចរាល់ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LbStatus.Text = "ការអានរួចរាល់ !";
                    }
                    else
                    {
                        MessageBox.Show("ការអានរួចរាល់ ! ប៉ុន្តែមិនមានទិន្នន័យ POS ទាំងនេះទេ ៖ \n"+ReadNotFound, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        LbStatus.Text = "ការអានរួចរាល់ !";
                    }                    
                    dgvExcelData.Focus();
                    dgvExcelData.ClearSelection();
                }
                catch (Exception ex)
                {
                    LbStatus.Text = "ការអានបរាជ័យ !";
                    Cursor = Cursors.Default;
                    MessageBox.Show("Something wrong!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                }
            }
        }

        private void CheckOBSandKIT()
        {
            if (dgvExcelData.Rows.Count > 0)
            {
                string AllPOS = "";
                foreach (DataGridViewRow row in dgvExcelData.Rows)
                {
                    if (AllPOS.Trim() == "")
                    {
                        AllPOS = "'" + row.Cells[2].Value.ToString() +"'";
                    }
                    else
                    {
                        AllPOS = AllPOS+ ", '" + row.Cells[2].Value.ToString() + "'";
                    }
                }

                try
                {
                    cnnOBS.conOBS.Open();
                    //Find Remark for POS Parent
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT DONo, Remark FROM prgproductionorder WHERE DONo  IN ( "+ AllPOS + " )", cnnOBS.conOBS);
                    DataTable dtTemp = new DataTable();
                    sda.Fill(dtTemp);
                    //Add Remark to Dgv
                    foreach (DataGridViewRow DgvRow in dgvExcelData.Rows)
                    {
                        foreach (DataRow row in dtTemp.Rows)
                        {
                            if (DgvRow.Cells[2].Value.ToString() == row[0].ToString())
                            {
                                DgvRow.Cells[6].Value = row[1].ToString();
                            }
                        }

                    }

                    SqlDataAdapter sda1 = new SqlDataAdapter("SELECT T1.DONo, T2.ItemCode, T3.ItemName, T2.DONo, T3.Remark2, T3.Remark3, T3.Remark4, T2.SemiQty, PlanQty, Remark FROM " +
                                                                                        "(SELECT ProductNo, DONo FROM prgproductionorder WHERE DONo IN("+ AllPOS + ")) T1 "+
                                                                                        "INNER JOIN "+
                                                                                        "(SELECT ProductNo, ItemCode, DONo, SemiQty, PlanQty, Remark FROM prgproductionorder WHERE LineCode = 'MC1') T2 " +
                                                                                        "ON T1.ProductNo = T2.ProductNo "+
                                                                                        "INNER JOIN "+
                                                                                        "(SELECT ItemCode, ItemName, Remark2, Remark3, Remark4 FROM mstitem WHERE  ItemType='1') T3 " +
                                                                                        "ON T3.ItemCode = T2.ItemCode "+
                                                                                        "ORDER BY T1.DONo ASC, T2.DONo ASC", cnnOBS.conOBS);
                    dtChild = new DataTable();
                    sda1.Fill(dtChild);
                    dtChild.Columns.Add("KIT Promise");
                    foreach (DataRow row in dtChild.Rows)
                    {
                        foreach (DataGridViewRow DgvRow in dgvExcelData.Rows)
                        {
                            if (row[0].ToString() == DgvRow.Cells[2].Value.ToString())
                            {
                                row[10] = Convert.ToDateTime(DgvRow.Cells[5].Value.ToString()).ToString("dd-MM-yyyy");
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something wrong!"+ex.Message, "Rachhan System",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }
        }

        private void SaveToDB()
        {
            DialogResult DLS = MessageBox.Show("តើអ្នករក្សាទុកទិន្នន័យមែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DLS == DialogResult.Yes)
            {
                Cursor = Cursors.WaitCursor;
                LbStatus.Text = "កំពុងគណនា និងបញ្ចូល . . .";
                LbStatus.Refresh();
                LbStatus.Visible = true;
                FindChildPOS();
                if (dtChildNotFoundExcelMtr.Rows.Count == 0)
                {
                    WriteToExcelFileExcelMtr();
                    LbStatus.Text = "កំពុងរក្សាទុក . . .";
                    LbStatus.Refresh();
                    DateTime DateReg = DateTime.Now;
                    string POSError = "";
                    try
                    {
                        cnn.con.Open();
                        foreach (DataGridViewRow DgvRow in dgvExcelData.Rows)
                        {
                            string ErrorFound = "";
                            foreach (DataRow row in dtChild.Rows)
                            {
                                if (row[0].ToString() == DgvRow.Cells[2].Value.ToString())
                                {
                                    try
                                    {
                                        cmd = new SqlCommand("INSERT INTO tbPOSDetailofMC (FGCode, PosPNo, PosPQty, PosPDelDate, WIPCode, PosCNo, SemiQty, PosCQty, PosCRemainQty, PosCResultQty, PosCStatus, Remarks, DocNo, KITPromise, LabelStatus, RegDate, RegBy, UpdateDate, UpdateBy, PrintStatus) " +
                                                                            "VALUES (@Fc, @Ppn, @Ppqty, @Ppd, @Wc, @Pcn, @Sqty, @Pcqty, @Pcr, @Pcrs, @Pcs, @Rm, @Dn, @Kpm, @Ls, @Rd, @Rb, @Ud, @Ub, @Ps)", cnn.con);
                                        cmd.Parameters.AddWithValue("@Fc", DgvRow.Cells[0].Value.ToString());
                                        cmd.Parameters.AddWithValue("@Ppn", DgvRow.Cells[2].Value.ToString());
                                        cmd.Parameters.AddWithValue("@Ppqty", Convert.ToDouble(DgvRow.Cells[3].Value.ToString()));
                                        cmd.Parameters.AddWithValue("@Ppd", Convert.ToDateTime(DgvRow.Cells[4].Value.ToString()));
                                        cmd.Parameters.AddWithValue("@Wc", row[1].ToString());
                                        cmd.Parameters.AddWithValue("@Pcn", row[3].ToString());
                                        cmd.Parameters.AddWithValue("@Sqty", Convert.ToDouble(row[7].ToString()));
                                        cmd.Parameters.AddWithValue("@Pcqty", Convert.ToDouble(row[8].ToString()));
                                        cmd.Parameters.AddWithValue("@Pcr", Convert.ToDouble(row[8].ToString()));
                                        cmd.Parameters.AddWithValue("@Pcrs", 0);
                                        cmd.Parameters.AddWithValue("@Pcs", 0);
                                        cmd.Parameters.AddWithValue("@Rm", row[9].ToString());
                                        cmd.Parameters.AddWithValue("@Dn", DgvRow.Cells[7].Value.ToString());
                                        cmd.Parameters.AddWithValue("@Kpm", Convert.ToDateTime(row[10].ToString()).ToString("yyyy-MM-dd 00:00:00"));
                                        cmd.Parameters.AddWithValue("@Ls", "NOT YET");
                                        DateTime Now = DateTime.Now;
                                        cmd.Parameters.AddWithValue("@Rd", Now);
                                        cmd.Parameters.AddWithValue("@Rb", MenuFormV2.UserForNextForm);
                                        cmd.Parameters.AddWithValue("@Ud", Now);
                                        cmd.Parameters.AddWithValue("@Ub", MenuFormV2.UserForNextForm);
                                        cmd.Parameters.AddWithValue("@Ps", "0");
                                        cmd.ExecuteNonQuery();

                                    }
                                    catch
                                    {
                                        if (ErrorFound.Trim() == "")
                                        {
                                            ErrorFound = DgvRow.Cells[2].Value.ToString() + " : " + row[3].ToString();
                                        }
                                        else
                                        {
                                            ErrorFound = ErrorFound + ", " + row[3].ToString();
                                        }
                                    }
                                }
                            }
                            if (ErrorFound.Trim() != "")
                            {
                                if (POSError.Trim() == "")
                                {
                                    POSError = "- " + ErrorFound + ".";
                                }
                                else
                                {
                                    POSError = POSError + "\n- " + ErrorFound + ".";
                                }
                            }
                        }

                        if (POSError.Trim() == "")
                        {
                            Cursor = Cursors.Default;
                            LbStatus.Text = "រក្សាទុករួចរាល់!";
                            LbStatus.Refresh();
                            MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvExcelData.Rows.Clear();
                            dgvPosChild.Rows.Clear();
                            CleardtExcelMtr();
                            CheckBtnSave();
                        }
                        else
                        {
                            Cursor = Cursors.Default;
                            LbStatus.Text = "រក្សាទុករួចរាល់! ប៉ុន្តែទិន្នន័យខ្លះត្រូវបានរំលង!";
                            LbStatus.Refresh();
                            MessageBox.Show("រក្សាទុករួចរាល់!\nប៉ុន្តែទិន្នន័យទាំងនេះមិនត្រូវបានរក្សាទុកទេ៖\n" + POSError, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            dgvExcelData.Rows.Clear();
                            dgvPosChild.Rows.Clear();
                            CheckBtnSave();
                        }
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        LbStatus.Text = "រក្សាមានបញ្ហា!";
                        LbStatus.Refresh();
                        MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cnn.con.Close();
                }
                else
                {
                    string MsgBox = "គ្មានទិន្នន័យ POS ទាំងនេះទេ៖";
                    foreach (DataRow row in dtChildNotFoundExcelMtr.Rows)
                    {
                        MsgBox = MsgBox + "\n • " + row[0].ToString();
                    }
                    Cursor = Cursors.Default;
                    MessageBox.Show(MsgBox, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void CleardtExcelMtr()
        {
            dtCombineAndCountParentExcelMtr = new DataTable();
            dtCombineAndCountParentExcelMtr.Columns.Add("POSParentCode");
            dtCombineAndCountParentExcelMtr.Columns.Add("Count");

            dtChildDataExcelMtr = new DataTable();
            dtChildDataExcelMtr.Columns.Add("NoOfPOSParent");
            dtChildDataExcelMtr.Columns.Add("POSParent");
            dtChildDataExcelMtr.Columns.Add("POSParentCode");
            dtChildDataExcelMtr.Columns.Add("POSParentQty");
            dtChildDataExcelMtr.Columns.Add("POSParentShipDate");
            dtChildDataExcelMtr.Columns.Add("ItemCode");
            dtChildDataExcelMtr.Columns.Add("TotalQty");

            dtChildNotFoundExcelMtr = new DataTable();
            dtChildNotFoundExcelMtr.Columns.Add("POSParent");

            dtCodeNotFoundExcelMtr = new DataTable();
            dtCodeNotFoundExcelMtr.Columns.Add("Code");

        }

        private void WriteToExcelFileExcelMtr()
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: txtExcelDirectory.Text.ToString() + @"", Editable: true);
            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Mtr.data"];
            Excel.Range xlRange;

            try
            {
                xlRange = worksheet.UsedRange;
                worksheet.Unprotect(txtPassword.Text.ToString());

                //Check header and WipCode index
                int HeaderRowIndex = 0;
                int WipCodeColIndex = 0;
                for (int i = 1; i <= xlRange.Rows.Count; i++)
                {
                    for (int j = 1; j <= xlRange.Columns.Count; j++)
                    {
                        if (xlRange.Cells[i, j].Value != null && xlRange.Cells[i, j].Value == "WIP-Code")
                        {
                            HeaderRowIndex = i;
                            WipCodeColIndex = j;
                            break;
                        }
                    }
                    if (HeaderRowIndex != 0)
                    {
                        break;
                    }
                }

                //Check Column that match with SaveDate
                int indexOfWritingDate = 0;
                for (int i = 1; i <= xlRange.Columns.Count; i++)
                {
                    if (xlRange.Cells[HeaderRowIndex, i].Value != null)
                    {
                        try
                        {
                            string ExcelDate = (xlRange.Cells[HeaderRowIndex, i].Value).ToString();
                            if (DtStart.Value.ToString("dd-MM-yyyy") == Convert.ToDateTime(ExcelDate).ToString("dd-MM-yyyy"))
                            {
                                indexOfWritingDate = i;
                                break;
                            }
                        }
                        catch
                        {

                        }
                    }
                }

                //If SaveDate was found
                if (indexOfWritingDate != 0)
                {
                    //Read Index by each Code, Easy to write into excel faster
                    dtExcelCodeIndexExcelMtr = new DataTable();
                    dtExcelCodeIndexExcelMtr.Columns.Add("Code");
                    dtExcelCodeIndexExcelMtr.Columns.Add("Index");

                    if (WipCodeColIndex != 0)
                    {
                        for (int i = HeaderRowIndex; i <= xlRange.Rows.Count; i++)
                        {
                            if (xlRange.Cells[i, WipCodeColIndex].Value != null)
                            {
                                string Code = xlRange.Cells[i, WipCodeColIndex].Text;
                                if (Code.Trim() != "")
                                {
                                    dtExcelCodeIndexExcelMtr.Rows.Add(Code, i+1);//if excel row1=0+1=1, row2=1+1=2
                                }
                            }
                        }

                        foreach (DataRow row in dtChildDataExcelMtr.Rows)
                        {

                            //Find Code index in excel through dtExcelCodeIndex
                            int indexOfWritingCode = 0;
                            foreach (DataRow rowIndex in dtExcelCodeIndexExcelMtr.Rows)
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
                                dtCodeNotFoundExcelMtr.Rows.Add(row[5]);
                            }
                        }

                        if (dtCodeNotFoundExcelMtr.Rows.Count == 0)
                        {

                            worksheet.Protect(txtPassword.Text.ToString(), AllowFiltering: true);
                            xlWorkBook.Save();
                            xlWorkBook.Close();
                            excelApp.Quit();
                            CleardtExcelMtr();
                        }
                        else
                        {
                            excelApp.DisplayAlerts = false;
                            xlWorkBook.Close(false);
                            excelApp.DisplayAlerts = true;
                            excelApp.Quit();
                            string MsgBox = "រក្សាទិន្នន័យទុកក្នុងឯកសារ Excel មានបញ្ហា!\nរកមិនឃើញកូដទាំងនេះទេ៖";
                            foreach (DataRow row in dtCodeNotFoundExcelMtr.Rows)
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
                    MessageBox.Show("Can't Find this date( " + DtStart.Value.ToString("dd-MM-yyyy") + " ) in this sheet 'Mtr.data' !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            catch (Exception ex)
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
                MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FindChildPOS()
        {
            CleardtExcelMtr();
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
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT t1.ItemCode, t1.PlanQty  FROM( " +
                                                                                            "(SELECT ProductNo, ItemCode, PlanQty FROM prgproductionorder WHERE LineCode = 'MC1') t1 " +
                                                                                            "INNER JOIN " +
                                                                                            "(SELECT ProductNo, DONo, ItemCode, PlanQty, POSDeliveryDate FROM prgproductionorder " +
                                                                                            "WHERE DONo = '" + POSParent + "') t2 " +
                                                                                            "ON t2.ProductNo = t1.ProductNo)", cnnOBS.conOBS);
                        DataTable dtTemp = new DataTable();
                        sda.Fill(dtTemp);
                        if (dtTemp.Rows.Count > 0)
                        {
                            double Count = 0;
                            if (dtCombineAndCountParentExcelMtr.Rows.Count == 0)
                            {
                                dtCombineAndCountParentExcelMtr.Rows.Add(POSParentCode, 1);
                            }
                            else
                            {
                                int Found = 0;
                                foreach (DataRow row in dtCombineAndCountParentExcelMtr.Rows)
                                {
                                    if (row[0].ToString() == POSParentCode)
                                    {
                                        Count = Convert.ToDouble(row[1].ToString());
                                        row[1] = Convert.ToDouble(row[1].ToString()) + 1;
                                        Found = Found + 1;
                                        break;
                                    }
                                }
                                if (Found == 0)
                                {
                                    dtCombineAndCountParentExcelMtr.Rows.Add(POSParentCode, 1);
                                }
                            }

                            foreach (DataRow row in dtTemp.Rows)
                            {
                                dtChildDataExcelMtr.Rows.Add(Count, POSParent, POSParentCode, POSParentQty, POSDelDate, row[0], Convert.ToDouble(row[1]));
                            }
                        }
                        else
                        {
                            dtChildNotFoundExcelMtr.Rows.Add(dgvRow.Cells[2].Value.ToString());

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

    }
}
