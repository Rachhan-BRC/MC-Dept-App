using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.Inventory.MC_SD
{
    public partial class MCSDCountingForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SqlCommand cmd;
        DataTable dtRM;
        string CountType;
        string CodeSelected;
        double NetWSelected;
        double MOQSelected;
        string BobbinOrReilSelected;
        double BobbinQtyInputted;
        string LastTypeSelected;
        string ErrorText;


        public MCSDCountingForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Shown += MCSDCountingForm_Shown;
            this.FormClosed += MCSDCountingForm_FormClosed;
            
            //btn
            this.btnShowDgvItems.Click += BtnShowDgvItems_Click;
            this.btnStart.Click += BtnStart_Click;
            this.btnNew.Click += BtnNew_Click;
            this.btnSave.Click += BtnSave_Click;

            //dgv
            this.dgvItems.CellClick += DgvItems_CellClick;
            
            //txt
            this.txtItems.TextChanged += TxtItems_TextChanged;
            this.txtItems.KeyDown += TxtItems_KeyDown;
            this.txtQty.KeyPress += TxtQty_KeyPress;
            this.txtQty.KeyDown += TxtQty_KeyDown;
            this.txtQty.TextChanged += TxtQty_TextChanged;

            //Cbo
            this.CboBobbinW.SelectedIndexChanged += CboBobbinW_SelectedIndexChanged;

        }

        
        //Cbo
        private void CboBobbinW_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtQty.Text.Trim() != "")
            {
                string QtyDetails = txtQty.Text;
                //split betwen + Symbol.
                string[] QtyArray = QtyDetails.Split('+');

                //If Count By Reil
                if (LbBobbinsWTitle.Text.ToString().Contains("R2") == true && LbNetWTitle.Text.ToString().Contains("R1") == true)
                {
                    //Calc Total Qty
                    double TotalQty = 0;
                    for (int i = 0; i < QtyArray.Length; i++)
                    {
                        if (QtyArray[i].ToString().Trim() != "")
                        {
                            double R2 = Convert.ToDouble(CboBobbinW.Text);
                            double R1 = NetWSelected;
                            double MOQ = MOQSelected;
                            double InputedR3 = Convert.ToDouble(QtyArray[i].ToString());
                            TotalQty = TotalQty + Math.Round(MOQ * (InputedR3 * InputedR3 - R2 * R2) / (R1 * R1 - R2 * R2), 2);
                            TotalQty = Convert.ToDouble(TotalQty.ToString("N0"));
                        }
                    }

                    LbTotalQty.Text = TotalQty.ToString("N0");

                }
                //If Count By Bobbins
                else
                {
                    //Calc Total Qty
                    double TotalQty = 0;
                    for (int i = 0; i < QtyArray.Length; i++)
                    {
                        if (QtyArray[i].ToString().Trim() != "")
                        {
                            double W = Convert.ToDouble(QtyArray[i].ToString());
                            double BobbinW = Convert.ToDouble(CboBobbinW.Text);
                            TotalQty = TotalQty + Math.Round((W - BobbinW) / NetWSelected * MOQSelected, 2);
                            TotalQty = Convert.ToDouble(TotalQty.ToString("N0"));
                        }
                    }

                    LbTotalQty.Text = TotalQty.ToString("N0");

                }
            }
        }

        //dgv
        private void DgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                ClearAllText();
                CodeSelected = dgvItems.Rows[e.RowIndex].Cells[0].Value.ToString();
                dgvItems.Visible= false;
                ScanOrSelectItem();
                if (LbCode.Text.Trim() == "" && LbItems.Text.Trim() == "")
                {
                    LbType.Text = "";
                    CboBobbinW.Items.Clear();
                    MessageBox.Show("គ្មានវត្ថុធាតុដើមនេះទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtItems.Focus();
                    txtItems.SelectAll();
                }
                else
                {
                    txtItems.Text = "";
                    txtQty.Focus();
                }
            }
        }

        //txt
        private void TxtItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtItems.Text.Trim() != "")
                {
                    ClearAllText();
                    CodeSelected = txtItems.Text;
                    ScanOrSelectItem();
                    if (LbCode.Text.Trim() == "" && LbItems.Text.Trim() == "")
                    {
                        LbType.Text = "";
                        CboBobbinW.Items.Clear();
                        MessageBox.Show("គ្មានវត្ថុធាតុដើមនេះទេ!","Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                        txtItems.Focus();
                        txtItems.SelectAll();
                    }
                    else
                    {
                        txtItems.Text = "";
                        txtQty.Focus();
                    }
                }
            }
        }
        private void TxtItems_TextChanged(object sender, EventArgs e)
        {
            dgvItems.Rows.Clear();
            if (txtItems.Text.Trim() == "")
            {
                foreach (DataRow row in dtRM.Rows)
                {
                    dgvItems.Rows.Add(row[0], row[1]);
                }
            }
            else
            {
                foreach (DataRow row in dtRM.Rows)
                {
                    if (row[0].ToString().ToLower().Contains(txtItems.Text.ToLower()) == true || row[1].ToString().ToLower().Contains(txtItems.Text.ToLower()) == true)
                    {
                        dgvItems.Rows.Add(row[0], row[1]);
                    }
                }
            }
            dgvItems.ClearSelection();
        }
        private void TxtQty_TextChanged(object sender, EventArgs e)
        {
            if (txtQty.Text.Trim() != "")
            {
                if (CountType.Contains("Weight") == true)
                {
                    if (CboBobbinW.Text.Trim() != "")
                    {
                        if (txtQty.ToString().Contains("*") == false)
                        {
                            string QtyDetails = txtQty.Text;
                            //split betwen + Symbol.
                            string[] QtyArray = QtyDetails.Split('+');
                            BobbinQtyInputted = QtyArray.Length;

                            try
                            {
                                //If Count By Reil
                                if (LbBobbinsWTitle.Text.ToString().Contains("R2") == true && LbNetWTitle.Text.ToString().Contains("R1") == true)
                                {
                                    //Calc Total Qty
                                    double TotalQty = 0;
                                    for (int i = 0; i < QtyArray.Length; i++)
                                    {
                                        if (QtyArray[i].ToString().Trim() != "")
                                        {
                                            double R2 = Convert.ToDouble(CboBobbinW.Text);
                                            double R1 = NetWSelected;
                                            double MOQ = MOQSelected;
                                            double InputedR3 = Convert.ToDouble(QtyArray[i].ToString());
                                            TotalQty = TotalQty + Math.Round(MOQ * (InputedR3 * InputedR3 - R2 * R2) / (R1 * R1 - R2 * R2), 2);
                                            TotalQty = Convert.ToDouble(TotalQty.ToString("N0"));
                                        }
                                    }

                                    LbTotalQty.Text = TotalQty.ToString("N0");

                                    //Assign Bobbin/Reel
                                    if (BobbinQtyInputted > 1)
                                    {
                                        LbBobbinQty.Text = "( " + BobbinQtyInputted + " Reels )";
                                    }
                                    else
                                    {
                                        LbBobbinQty.Text = "( " + BobbinQtyInputted + " Reel )";
                                    }
                                }
                                //If Count By Bobbins
                                else
                                {
                                    //Calc Total Qty
                                    double TotalQty = 0;
                                    for (int i = 0; i < QtyArray.Length; i++)
                                    {
                                        if (QtyArray[i].ToString().Trim() != "")
                                        {
                                            double W = Convert.ToDouble(QtyArray[i].ToString());
                                            double BobbinW = Convert.ToDouble(CboBobbinW.Text);
                                            TotalQty = TotalQty + Math.Round((W - BobbinW) / NetWSelected * MOQSelected, 2);
                                            TotalQty = Convert.ToDouble(TotalQty.ToString("N0"));
                                        }
                                    }

                                    LbTotalQty.Text = TotalQty.ToString("N0");

                                    //Assign Bobbin/Reel
                                    if (BobbinQtyInputted > 1)
                                    {
                                        LbBobbinQty.Text = "( " + BobbinQtyInputted + " Bobbins )";
                                    }
                                    else
                                    {
                                        LbBobbinQty.Text = "( " + BobbinQtyInputted + " Bobbin )";
                                    }
                                }
                            }
                            catch
                            {
                                LbTotalQty.Text = "";
                                LbBobbinQty.Text = "";
                                MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                        else
                        {
                            LbTotalQty.Text = "";
                            LbBobbinQty.Text = "";
                            MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        MessageBox.Show("សូមជ្រើសរើស <ទម្ងន់ប៊ូប៊ីន (KG)> ជាមុនសិន!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    string QtyDetails = txtQty.Text;
                    //split betwen + Symbol.
                    string[] QtyArray = QtyDetails.Split('+');
                    BobbinQtyInputted = 0;
                    for (int i = 0; i < QtyArray.Length; i++)
                    {
                        if (QtyArray[i].Trim() != "")
                        {
                            double CheckBobbinQty = 1;
                            if (QtyArray[i].ToString().Contains("*") == true)
                            {
                                string[] CheckBobbinQtyArray = QtyArray[i].ToString().Split('*');
                                if (CheckBobbinQtyArray[1].ToString().Trim() != "")
                                {
                                    CheckBobbinQty = Convert.ToDouble(CheckBobbinQtyArray[1].ToString());
                                }
                            }
                            BobbinQtyInputted = BobbinQtyInputted + CheckBobbinQty;
                        }
                    }

                    try
                    {
                        //Calc Total Qty
                        double TotalQty = 0;
                        int ErrorCalc = 0;
                        for (int i = 0; i < QtyArray.Length; i++)
                        {
                            double CheckQty = 0;
                            if (QtyArray[i].ToString().Trim()!="")
                            {
                                if (QtyArray[i].ToString().Contains("*") == true)
                                {
                                    string[] CheckQtyArray = QtyArray[i].Split('*');
                                    if (CheckQtyArray.Length < 3)
                                    {
                                        if (CheckQtyArray[1].ToString().Trim() != "")
                                        {
                                            CheckQty = CheckQty + (Convert.ToDouble(CheckQtyArray[0].ToString()) * Convert.ToDouble(CheckQtyArray[1].ToString()));
                                        }
                                    }
                                    else
                                    {
                                        LbTotalQty.Text = "";
                                        LbBobbinQty.Text = "";
                                        MessageBox.Show("អ្នកមិនអាចគុណតគ្នាបែបនេះបានទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        ErrorCalc++;
                                        break;
                                    }
                                }
                                else
                                {
                                    CheckQty = CheckQty + Convert.ToDouble(QtyArray[i].ToString());
                                }
                            }
                            TotalQty = TotalQty + CheckQty;
                        }

                        if (ErrorCalc == 0)
                        {
                            LbTotalQty.Text = TotalQty.ToString("N0");

                            //Assign Bobbin/Reel
                            if (LbBobbinsWTitle.Text.ToString().Contains("R2") == true && LbNetWTitle.Text.ToString().Contains("R1") == true)
                            {
                                if (BobbinQtyInputted > 1)
                                {
                                    LbBobbinQty.Text = "( " + BobbinQtyInputted + " Reels )";
                                }
                                else
                                {
                                    LbBobbinQty.Text = "( " + BobbinQtyInputted + " Reel )";
                                }
                            }
                            else
                            {
                                if (BobbinQtyInputted > 1)
                                {
                                    LbBobbinQty.Text = "( " + BobbinQtyInputted + " Bobbins )";
                                }
                                else
                                {
                                    LbBobbinQty.Text = "( " + BobbinQtyInputted + " Bobbin )";
                                }
                            }
                        }
                    }
                    catch
                    {
                        LbTotalQty.Text = "";
                        LbBobbinQty.Text = "";
                        MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                LbTotalQty.Text = "";
                LbBobbinQty.Text = "";
            }
        }
        private void TxtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CountType.Contains("Weight") == true)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '+') && (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '+') && (e.KeyChar != '*'))
                {
                    e.Handled = true;
                }
            }
        }
        private void TxtQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave.PerformClick();
            }
        }

        //btn
        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (CboCountType.Text.ToString().Contains("Weight") ==true)
            {
                CountType = "Weight";
                txtQty.Text = "";
                ShowWeight();
            }
            else
            {
                CountType = "Box";
                txtQty.Text = "";
                ShowBox();
            }
        }
        private void BtnShowDgvItems_Click(object sender, EventArgs e)
        {
            if (dgvItems.Visible == true)
            {
                dgvItems.Visible = false;
            }
            else
            {
                dgvItems.Visible = true; 
            }
            if (txtItems.Text.Trim() == "")
            {
                dgvItems.Rows.Clear();
                foreach (DataRow row in dtRM.Rows)
                {
                    dgvItems.Rows.Add(row[0], row[1]);
                }
            }
            txtItems.Focus();
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearAllText();
            LbType.Text = "";
            CboBobbinW.Items.Clear();
            txtItems.Focus();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (LbCode.Text.Trim() != "" && LbItems.Text.Trim() != "" && LbTotalQty.Text.Trim() != "" && LbBobbinQty.Text.Trim()!="")
            {
                //Check QtyDetail first
                if (CountType.Contains("Weight") == true)
                {
                    //Only 0-9 & '.' & '+'
                    int FoundError = 0;
                    string[] AllowText = {"0","1","2","3","4","5","6","7","8","9",".","+"};
                    for (int i = 0; i < txtQty.Text.ToString().Length; i++)
                    {
                        int OK = 0;
                        for (int j = 0; j < AllowText.Length; j++)
                        {
                            if (txtQty.Text[i].ToString() == AllowText[j].ToString())
                            {
                                OK = 1;
                                break;
                            }
                        }
                        if (OK == 0)
                        {
                            FoundError++;
                        }
                    }

                    //Check each Qty less than BobbinW/R1 or not
                    string[] QtyArray = txtQty.Text.ToString().Split('+');
                    for (int i = 0; i < QtyArray.Length; i++)
                    {
                        if (Convert.ToDouble(QtyArray[i].ToString()) < Convert.ToDouble(CboBobbinW.Text))
                        {
                            FoundError ++;
                            MessageBox.Show("ក្នុង <ទម្ងន់សរុប> ខ្លះមានទម្ងន់តិចជាង <ទម្ងន់ប៊ូប៊ីន> !","Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                            break;
                        }
                    }

                    //Sum/CalcByComputer
                    try
                    {
                        string value = new DataTable().Compute(txtQty.Text.ToString(), null).ToString();
                        double Total = Convert.ToDouble(value);
                    }
                    catch
                    {
                        FoundError ++;
                    }

                    if (FoundError == 0)
                    {
                        DialogResult DSL = MessageBox.Show("តើអ្នកចង់រក្សាទុកមែនឬទេ?","Rachhan System",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                        if (DSL == DialogResult.Yes)
                        {
                            LbStatus.Text = "កំពុងរក្សាទុក . . .";
                            LbStatus.Refresh();
                            Cursor = Cursors.WaitCursor;

                            ErrorText = "";
                            DataTable dtLabelNo= new DataTable();
                            DataTable dtLabelNoRange = new DataTable();

                            //Check LabelNo
                            try
                            {
                                cnn.con.Open();
                                SqlDataAdapter sda = new SqlDataAdapter("SELECT LabelNo AS LastLabelNo FROM tbInventory WHERE LabelNo = (SELECT MAX(LabelNo) FROM tbInventory WHERE LocCode='WIR1') GROUP BY LabelNo", cnn.con);
                                dtLabelNo = new DataTable();
                                sda.Fill(dtLabelNo);

                                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM tbSDMCLocation WHERE LocCode='WIR1'", cnn.con);
                                dtLabelNoRange = new DataTable();
                                sda1.Fill(dtLabelNoRange);

                            }
                            catch (Exception ex)
                            {
                                if (ErrorText.Trim() == "")
                                {
                                    ErrorText = "Take Label No : " + ex.Message;
                                }
                                else
                                {
                                    ErrorText = ErrorText + "\nTake Label No : " + ex.Message;
                                }
                            }
                            cnn.con.Close();

                            //Insert data to DB & Print
                            if (ErrorText.Trim() == "")
                            {
                                string LocCode = "WIR1";
                                string SubLoc = "Wire&Tube";
                                if (LbType.Text == "Terminal")
                                {
                                    SubLoc = "Terminal";
                                }
                                int LabelNo = Convert.ToInt32(dtLabelNoRange.Rows[0][2].ToString());
                                if (dtLabelNo.Rows.Count > 0)
                                {
                                    LabelNo = Convert.ToInt32(dtLabelNo.Rows[0][0].ToString()) + 1;
                                }
                                string CountingMethod = "StockCard";
                                string ItemCode = LbCode.Text;
                                string CountingMethod2 = "Box";
                                if (CountType.Contains("Weight") == true)
                                {
                                    CountingMethod2 = "Weight";
                                }
                                string ItemType = "Material";
                                int Qty = Convert.ToInt32(LbTotalQty.Text.Replace(",", ""));
                                string QtyDetails = txtQty.Text.Replace("*", "x");
                                string QtyDetails2 = CboBobbinW.Text + "|" + BobbinQtyInputted;

                                try
                                {
                                    cnn.con.Open();
                                    string Username = MenuFormV2.UserForNextForm;
                                    DateTime RegDate = DateTime.Now;

                                    if (LabelNo <= Convert.ToInt32(dtLabelNoRange.Rows[0][3].ToString()))
                                    {
                                        cmd = new SqlCommand("INSERT INTO tbInventory(LocCode, SubLoc, LabelNo, CountingMethod, CountingMethod2, SeqNo, ItemCode, ItemType, Qty, QtyDetails, QtyDetails2, RegDate, RegBy, UpdateDate, UpdateBy, CancelStatus) " +
                                        "VALUES (@Lc, @Slc, @lbn, @Cm, @Cm2, @Sn, @Ic, @It, @Qty, @QtyD, @QtyD2, @RegD, @RegB, @UpD, @UpB, @Cs)", cnn.con);

                                        cmd.Parameters.AddWithValue("@Lc", LocCode);
                                        cmd.Parameters.AddWithValue("@Slc", SubLoc);
                                        cmd.Parameters.AddWithValue("@lbn", LabelNo);
                                        cmd.Parameters.AddWithValue("@Cm", CountingMethod);
                                        cmd.Parameters.AddWithValue("@Cm2", CountingMethod2);
                                        cmd.Parameters.AddWithValue("@Sn", 1);
                                        cmd.Parameters.AddWithValue("@Ic", ItemCode);
                                        cmd.Parameters.AddWithValue("@It", ItemType);
                                        cmd.Parameters.AddWithValue("@Qty", Qty);
                                        cmd.Parameters.AddWithValue("@QtyD", QtyDetails);
                                        cmd.Parameters.AddWithValue("@QtyD2", QtyDetails2);
                                        cmd.Parameters.AddWithValue("@RegD", RegDate);
                                        cmd.Parameters.AddWithValue("@RegB", Username);
                                        cmd.Parameters.AddWithValue("@UpD", RegDate);
                                        cmd.Parameters.AddWithValue("@UpB", Username);
                                        cmd.Parameters.AddWithValue("@Cs", 0);

                                        cmd.ExecuteNonQuery();

                                    }
                                    else
                                    {
                                        if (ErrorText.Trim() == "")
                                        {
                                            ErrorText = "Label No reach maximum : " + LabelNo.ToString("N0");
                                        }
                                        else
                                        {
                                            ErrorText = ErrorText + "\nLabel No reach maximum : " + LabelNo.ToString("N0");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (ErrorText.Trim() == "")
                                    {
                                        ErrorText = "Insert to DB : " + ex.Message;
                                    }
                                    else
                                    {
                                        ErrorText = ErrorText + "\nInsert to DB : " + ex.Message;
                                    }
                                }
                                cnn.con.Close();

                                //Print Label
                                if (chkPrintStatus.Checked == true)
                                {
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
                                            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Weight"];

                                            worksheet.Cells[2, 6] = "SD MC";
                                            worksheet.Cells[2, 4] = LabelNo;
                                            worksheet.Cells[3, 1] = "*" + LabelNo + "*";
                                            worksheet.Cells[7, 4] = ItemCode;
                                            worksheet.Cells[9, 4] = LbItems.Text;
                                            worksheet.Cells[11, 4] = LbType.Text;
                                            worksheet.Cells[13, 4] = LbMaker.Text;
                                            worksheet.Cells[15, 4] = Qty;
                                            //Summary
                                            worksheet.Cells[15, 6] = "( " + QtyDetails + " )\n" + LbBobbinQty.Text.Replace("( ","").Replace(" )","") ;

                                            // Saving the modified Excel file
                                            var CDirectory2 = Environment.CurrentDirectory;
                                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                            string file = LabelNo.ToString();
                                            string fName = file + "( " + date + " )";

                                            //លុប Manual worksheet
                                            Excel.Worksheet wsBarcode = (Excel.Worksheet)xlWorkBook.Sheets["Box"];
                                            excelApp.DisplayAlerts = false;
                                            wsBarcode.Delete();
                                            excelApp.DisplayAlerts = true;

                                            worksheet.Name = "RachhanSystem";
                                            worksheet.SaveAs(SavePath + @"\" + fName + ".xlsx");
                                            for (int j = 0; j < numPrintQty.Value; j++)
                                            {
                                                worksheet.PrintOut();
                                            }
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
                                    }
                                }
                            }

                            Cursor = Cursors.Default;
                            if (ErrorText.Trim() == "")
                            {
                                LbStatus.Text = "រក្សាទុករួចរាល់!";
                                LbStatus.Refresh();
                                MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ClearAllText();
                                txtItems.Focus();
                            }
                            else
                            {
                                LbStatus.Text = "រក្សាទុកមានបញ្ហា!";
                                LbStatus.Refresh();
                                MessageBox.Show("មានបញ្ហា!\n"+ErrorText,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtQty.Focus();
                    }
                }
                else
                {
                    //Only 0-9 & '*' & '+'
                    int FoundError = 0;
                    string[] AllowText = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "*", "+" };
                    for (int i = 0; i < txtQty.Text.ToString().Length; i++)
                    {
                        int OK = 0;
                        for (int j = 0; j < AllowText.Length; j++)
                        {
                            if (txtQty.Text[i].ToString() == AllowText[j].ToString())
                            {
                                OK = 1;
                                break;
                            }
                        }
                        if (OK == 0)
                        {
                            FoundError++;
                        }
                    }

                    //Sum/CalcByComputer
                    try
                    {
                        string value = new DataTable().Compute(txtQty.Text.ToString(), null).ToString();
                        double Total = Convert.ToDouble(value);
                    }
                    catch
                    {
                        FoundError++;
                    }

                    if (FoundError == 0)
                    {
                        DialogResult DSL = MessageBox.Show("តើអ្នកចង់រក្សាទុកមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (DSL == DialogResult.Yes)
                        {
                            LbStatus.Text = "កំពុងរក្សាទុក . . .";
                            LbStatus.Refresh();
                            Cursor = Cursors.WaitCursor;

                            ErrorText = "";
                            DataTable dtLabelNo = new DataTable();
                            DataTable dtLabelNoRange = new DataTable();

                            //Check LabelNo
                            try
                            {
                                cnn.con.Open();
                                SqlDataAdapter sda = new SqlDataAdapter("SELECT LabelNo AS LastLabelNo FROM tbInventory WHERE LabelNo = (SELECT MAX(LabelNo) FROM tbInventory WHERE LocCode='WIR1') GROUP BY LabelNo", cnn.con);
                                dtLabelNo = new DataTable();
                                sda.Fill(dtLabelNo);

                                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM tbSDMCLocation WHERE LocCode='WIR1'", cnn.con);
                                dtLabelNoRange = new DataTable();
                                sda1.Fill(dtLabelNoRange);

                            }
                            catch (Exception ex)
                            {
                                if (ErrorText.Trim() == "")
                                {
                                    ErrorText = "Take Label No : " + ex.Message;
                                }
                                else
                                {
                                    ErrorText = ErrorText + "\nTake Label No : " + ex.Message;
                                }
                            }
                            cnn.con.Close();

                            //Insert data to DB
                            if (ErrorText.Trim() == "")
                            {
                                string LocCode = "WIR1";
                                string SubLoc = "Wire&Tube";
                                if (LbType.Text == "Terminal")
                                {
                                    SubLoc = "Terminal";
                                }
                                int LabelNo = Convert.ToInt32(dtLabelNoRange.Rows[0][2].ToString());
                                if (dtLabelNo.Rows.Count > 0)
                                {
                                    LabelNo = Convert.ToInt32(dtLabelNo.Rows[0][0].ToString()) + 1;
                                }
                                string CountingMethod = "StockCard";
                                string ItemCode = LbCode.Text;
                                string CountingMethod2 = "Box";
                                if (CountType.Contains("Weight") == true)
                                {
                                    CountingMethod2 = "Weight";
                                }
                                string ItemType = "Material";
                                int Qty = Convert.ToInt32(LbTotalQty.Text.Replace(",", ""));
                                string QtyDetails = txtQty.Text.Replace("*", "x");
                                string QtyDetails2 = BobbinQtyInputted.ToString();
                                try
                                {
                                    cnn.con.Open();                                    
                                    string Username = MenuFormV2.UserForNextForm;
                                    DateTime RegDate = DateTime.Now;
                                    if (LabelNo <= Convert.ToInt32(dtLabelNoRange.Rows[0][3].ToString()))
                                    {
                                        cmd = new SqlCommand("INSERT INTO tbInventory(LocCode, SubLoc, LabelNo, CountingMethod, CountingMethod2, SeqNo, ItemCode, ItemType, Qty, QtyDetails, QtyDetails2, RegDate, RegBy, UpdateDate, UpdateBy, CancelStatus) " +
                                        "VALUES (@Lc, @Slc, @lbn, @Cm, @Cm2, @Sn, @Ic, @It, @Qty, @QtyD, @QtyD2, @RegD, @RegB, @UpD, @UpB, @Cs)", cnn.con);

                                        cmd.Parameters.AddWithValue("@Lc", LocCode);
                                        cmd.Parameters.AddWithValue("@Slc", SubLoc);
                                        cmd.Parameters.AddWithValue("@lbn", LabelNo);
                                        cmd.Parameters.AddWithValue("@Cm", CountingMethod);
                                        cmd.Parameters.AddWithValue("@Cm2", CountingMethod2);
                                        cmd.Parameters.AddWithValue("@Sn", 1);
                                        cmd.Parameters.AddWithValue("@Ic", ItemCode);
                                        cmd.Parameters.AddWithValue("@It", ItemType);
                                        cmd.Parameters.AddWithValue("@Qty", Qty);
                                        cmd.Parameters.AddWithValue("@QtyD", QtyDetails);
                                        cmd.Parameters.AddWithValue("@QtyD2", QtyDetails2);
                                        cmd.Parameters.AddWithValue("@RegD", RegDate);
                                        cmd.Parameters.AddWithValue("@RegB", Username);
                                        cmd.Parameters.AddWithValue("@UpD", RegDate);
                                        cmd.Parameters.AddWithValue("@UpB", Username);
                                        cmd.Parameters.AddWithValue("@Cs", 0);

                                        cmd.ExecuteNonQuery();

                                    }
                                    else
                                    {
                                        if (ErrorText.Trim() == "")
                                        {
                                            ErrorText = "Label No reach maximum : " + LabelNo.ToString("N0");
                                        }
                                        else
                                        {
                                            ErrorText = ErrorText + "\nLabel No reach maximum : " + LabelNo.ToString("N0");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (ErrorText.Trim() == "")
                                    {
                                        ErrorText = "Insert to DB : " + ex.Message;
                                    }
                                    else
                                    {
                                        ErrorText = ErrorText + "\nInsert to DB : " + ex.Message;
                                    }
                                }
                                cnn.con.Close();

                                //Print Label
                                if (chkPrintStatus.Checked == true)
                                {
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
                                            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Box"];

                                            worksheet.Cells[2, 6] = "SD MC";
                                            worksheet.Cells[2, 4] = LabelNo;
                                            worksheet.Cells[3, 1] = "*" + LabelNo + "*";
                                            worksheet.Cells[7, 4] = ItemCode;
                                            worksheet.Cells[9, 4] = LbItems.Text;
                                            worksheet.Cells[11, 4] = LbType.Text;
                                            worksheet.Cells[13, 4] = LbMaker.Text;
                                            worksheet.Cells[15, 4] = Qty;
                                            //Summary
                                            worksheet.Cells[15, 6] = "( " + QtyDetails + " )\n" + LbBobbinQty.Text.Replace("( ", "").Replace(" )", "");

                                            // Saving the modified Excel file
                                            var CDirectory2 = Environment.CurrentDirectory;
                                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                            string file = LabelNo.ToString();
                                            string fName = file + "( " + date + " )";

                                            //លុប Manual worksheet
                                            Excel.Worksheet wsBarcode = (Excel.Worksheet)xlWorkBook.Sheets["Weight"];
                                            excelApp.DisplayAlerts = false;
                                            wsBarcode.Delete();
                                            excelApp.DisplayAlerts = true;

                                            worksheet.Name = "RachhanSystem";
                                            worksheet.SaveAs(SavePath + @"\" + fName + ".xlsx");
                                            for (int j = 0; j < numPrintQty.Value; j++)
                                            {
                                                worksheet.PrintOut();
                                            }
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
                                    }
                                }
                            }

                            Cursor = Cursors.Default;
                            if (ErrorText.Trim() == "")
                            {
                                LbStatus.Text = "រក្សាទុករួចរាល់!";
                                LbStatus.Refresh();
                                MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ClearAllText();
                                txtItems.Focus();
                            }
                            else
                            {
                                LbStatus.Text = "រក្សាទុកមានបញ្ហា!";
                                LbStatus.Refresh();
                                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtQty.Focus();
                    }
                }
            }
        }

        private void MCSDCountingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.MCSDPrintStatus = chkPrintStatus.Checked;
            Properties.Settings.Default.Save();
        }
        private void MCSDCountingForm_Shown(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            dgvItems.BringToFront();
            chkPrintStatus.Checked = Properties.Settings.Default.MCSDPrintStatus;
            string[] LocCode = { "Count by Weight", "Count by Box" };
            for (int i = 0; i < LocCode.Length; i++)
            {
                CboCountType.Items.Add(LocCode[i]);
            }
            CboCountType.SelectedIndex = 0;

            ErrorText = "";
            dtRM = new DataTable();
            DataTable dtRMTemp = new DataTable();
            //Take RM from MC DB
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT Code,RMType, BobbinsOrReil, R2OrBobbinsW, R1OrNetW, MOQ FROM tbSDMstUncountMat ORDER BY Code ASC", cnn.con);
                sda.Fill(dtRMTemp);
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

            //Take RM from OBS DB
            try
            {
                cnnOBS.conOBS.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, Resv1 FROM mstitem WHERE ItemType=2 AND DelFlag=0", cnnOBS.conOBS);
                sda.Fill(dtRM);
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

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                //Insert new columns to dtRM
                string[] RMColumns = { "RMType", " BobbinsOrReil", " R2OrBobbinsW", " R1OrNetW", " MOQ" };
                for (int i = 0; i < RMColumns.Length; i++)
                {
                    dtRM.Columns.Add(RMColumns[i]);
                    dtRM.AcceptChanges();
                }

                //Forloop copy information from dtRMTemp >> dtRM new columns
                for (int i = dtRM.Rows.Count - 1; i > -1; i--)
                {
                    int Found = 0;
                    foreach (DataRow TempRow in dtRMTemp.Rows)
                    {
                        if (dtRM.Rows[i][0].ToString() == TempRow[0].ToString())
                        {
                            //RM Type
                            dtRM.Rows[i][3] = TempRow[1].ToString();

                            //BobbinsOrReil
                            dtRM.Rows[i][4] = TempRow[2].ToString();

                            //R2
                            dtRM.Rows[i][5] = TempRow[3].ToString();

                            //R1
                            dtRM.Rows[i][6] = TempRow[4].ToString();

                            //MOQ
                            dtRM.Rows[i][7] = TempRow[5].ToString();

                            Found = Found + 1;
                            break;
                        }
                    }
                    if (Found == 0)
                    {
                        dtRM.Rows.RemoveAt(i);
                    }
                    dtRM.AcceptChanges();
                }

            }
            else
            {
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }


        //Function
        private void ShowWeight()
        {
            groupBox3.Text = "ព៌ត័មានរបស់វត្ថុធាតុដើម ( Count by " + CountType + " )";
            btnNew.Visible = true;
            btnSave.Visible = true;
            chkPrintStatus.Visible = true;
            numPrintQty.Visible = true;
            panelBody.Visible = true;
            //Enable CboBobbinW
            CboBobbinW.Enabled = true;
        }
        private void ShowBox()
        {
            groupBox3.Text = "ព៌ត័មានរបស់វត្ថុធាតុដើម ( Count by " + CountType + " )";
            btnNew.Visible = true;
            btnSave.Visible = true;
            chkPrintStatus.Visible = true;
            numPrintQty.Visible = true;
            panelBody.Visible = true;
            //Enable CboBobbinW
            CboBobbinW.Enabled = false;
        }
        private void ClearAllText()
        {
            LbCode.Text = "";
            LbItems.Text = "";
            LbMaker.Text = "";
            LbType.Text = "";
            LbNetW.Text = "";
            txtQty.Text = "";
        }
        private void ScanOrSelectItem()
        {
            if (CountType.Contains("Weight") == true)
            {
                foreach (DataRow row in dtRM.Rows)
                {
                    if (CodeSelected == row[0].ToString())
                    {
                        LbCode.Text = row[0].ToString();
                        LbItems.Text = row[1].ToString();
                        LbMaker.Text = row[2].ToString();
                        if (BobbinOrReilSelected != row[4].ToString())
                        {
                            BobbinOrReilSelected = row[4].ToString();
                        }
                        if (BobbinOrReilSelected == "Bobbin")
                        {
                            LbBobbinsWTitle.Text = "ទម្ងន់ប៊ូប៊ីន (KG)";
                            LbNetWTitle.Text = "ទម្ងន់សាច់សុទ្ធ (KG)";
                            LbNetWTitle.Font = new Font("Khmer OS Battambang", 14);
                            LbQtyTitle.Text = "ទម្ងន់សរុប";
                            if (LastTypeSelected != row[3].ToString())
                            {
                                LastTypeSelected = row[3].ToString(); 
                                LbType.Text = LastTypeSelected;
                                TakeTerminalOrWireBobbinsW();
                                CboBobbinW.Text = row[5].ToString();
                            }
                            else
                            {
                                LbType.Text = LastTypeSelected;
                                if (CboBobbinW.Text.Trim() == "")
                                {
                                    TakeTerminalOrWireBobbinsW();
                                    CboBobbinW.Text = row[5].ToString();
                                }
                            }
                            CboBobbinW.Enabled = true;
                        }
                        else
                        {
                            LbType.Text = row[3].ToString();
                            CboBobbinW.Items.Clear();
                            CboBobbinW.Items.Add(row[5].ToString());
                            CboBobbinW.Text = row[5].ToString();
                            LbBobbinsWTitle.Text = "ប្រវែងគ្មានធើមីណល R2 (mm)";
                            LbBobbinsWTitle.Font = new Font("Khmer OS Battambang", 11, FontStyle.Regular);
                            LbNetWTitle.Text = "ប្រវែងពេញ R1 (mm)";
                            LbQtyTitle.Text = "ប្រវែងសរុប";
                            CboBobbinW.Enabled = false;
                        }
                        NetWSelected = Convert.ToDouble(row[6].ToString());
                        MOQSelected = Convert.ToDouble(row[7].ToString());
                        LbNetW.Text = NetWSelected.ToString("N2") + " = " + MOQSelected.ToString("N0");
                    }
                }
            }
            else
            {
                foreach (DataRow row in dtRM.Rows)
                {
                    if (CodeSelected == row[0].ToString())
                    {
                        LbCode.Text = row[0].ToString();
                        LbItems.Text = row[1].ToString();
                        LbMaker.Text = row[2].ToString();
                        if (BobbinOrReilSelected != row[4].ToString())
                        {
                            BobbinOrReilSelected = row[4].ToString();
                        }
                        if (BobbinOrReilSelected == "Bobbin")
                        {
                            LbBobbinsWTitle.Text = "ទម្ងន់ប៊ូប៊ីន (KG)";
                            LbNetWTitle.Text = "ទម្ងន់សាច់សុទ្ធ (KG)";
                            LbNetWTitle.Font = new Font("Khmer OS Battambang", 14);
                            LbQtyTitle.Text = "សម្រាយចំនួន";
                            if (LastTypeSelected != row[3].ToString())
                            {
                                LastTypeSelected = row[3].ToString();
                                LbType.Text = LastTypeSelected;
                                TakeTerminalOrWireBobbinsW();
                                CboBobbinW.Text = row[5].ToString();
                            }
                            else
                            {
                                LbType.Text = LastTypeSelected;
                            }
                        }
                        else
                        {
                            LbType.Text = row[3].ToString();
                            CboBobbinW.Items.Clear();
                            CboBobbinW.Items.Add(row[5].ToString());
                            CboBobbinW.Text = row[5].ToString();
                            LbBobbinsWTitle.Text = "ប្រវែងគ្មានធើមីណល R2 (mm)";
                            LbBobbinsWTitle.Font = new Font("Khmer OS Battambang", 11, FontStyle.Regular);
                            LbNetWTitle.Text =  "ប្រវែងពេញ R1 (mm)";
                            LbQtyTitle.Text = "សម្រាយចំនួន";
                        }
                        NetWSelected = Convert.ToDouble(row[6].ToString());
                        MOQSelected = Convert.ToDouble(row[7].ToString());
                        LbNetW.Text = NetWSelected.ToString("N2") + " = " + MOQSelected.ToString("N0");
                    }
                }
            }
        }
        private void TakeTerminalOrWireBobbinsW()
        {
            CboBobbinW.Items.Clear();
            if (LbType.Text == "Wire")
            {
                try
                {
                    cnn.con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT RMType, BobbinsW FROM tbSDMstBobbinsWeight WHERE RMType='Wire'", cnn.con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            CboBobbinW.Items.Add(row[1].ToString());
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("មានបញ្ហា!\n"+ex.Message,"",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                cnn.con.Close();
            }
            else
            {
                try
                {
                    cnn.con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT RMType, BobbinsW FROM tbSDMstBobbinsWeight WHERE RMType='Terminal'", cnn.con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            CboBobbinW.Items.Add(row[1].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnn.con.Close();
            }
        }
    }
}
