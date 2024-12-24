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
    public partial class MCSDUpdateForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SqlCommand cmd;
        string CodeSelected;
        string CountType;
        double NetWSelected;
        double MOQSelected;
        string BobbinOrReilSelected;
        double BobbinQtyInputted;
        int LabelNoSelected;
        string ErrorText;

        public MCSDUpdateForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += MCSDUpdateForm_Load;
            this.FormClosed += MCSDUpdateForm_FormClosed;

            //txt
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;
            this.txtQty.KeyPress += TxtQty_KeyPress;
            this.txtQty.KeyDown += TxtQty_KeyDown;
            this.txtQty.TextChanged += TxtQty_TextChanged;

            //Cbo
            this.CboBobbinW.SelectedIndexChanged += CboBobbinW_SelectedIndexChanged;

            //btn
            this.btnNew.Click += BtnNew_Click;
            this.btnSave.Click += BtnSave_Click;

        }


        //btn
        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearAllText();
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (LbCode.Text.Trim() != "" && LbItems.Text.Trim() != "" && LbTotalQty.Text.Trim() != "" && LbBobbinQty.Text.Trim() != "")
            {
                //Check QtyDetail first
                if (CountType.Contains("Weight") == true)
                {
                    //Only 0-9 & '.' & '+'
                    int FoundError = 0;
                    string[] AllowText = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ".", "+" };
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
                            FoundError++;
                            MessageBox.Show("ក្នុង <ទម្ងន់សរុប> ខ្លះមានទម្ងន់តិចជាង <ទម្ងន់ប៊ូប៊ីន> !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                        FoundError++;
                    }

                    if (FoundError == 0)
                    {
                        DialogResult DSL = MessageBox.Show("តើអ្នកចង់អាប់ដេតទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (DSL == DialogResult.Yes)
                        {
                            LbStatus.Text = "កំពុងអាប់ដេត . . .";
                            LbStatus.Refresh();
                            Cursor = Cursors.WaitCursor;

                            ErrorText = "";
                            int Qty = Convert.ToInt32(LbTotalQty.Text.Replace(",", ""));
                            string QtyDetails = txtQty.Text.Replace("*", "x");
                            string QtyDetails2 = CboBobbinW.Text + "|" + BobbinQtyInputted;

                            //Update data
                            try
                            {
                                cnn.con.Open();
                                string Username = MenuFormV2.UserForNextForm;
                                DateTime UpdateDate = DateTime.Now;


                                string query = "UPDATE tbInventory SET Qty=" + Qty + ", " +
                                                    "QtyDetails='" + QtyDetails + "', " +
                                                    "QtyDetails2='" + QtyDetails2 + "', " +
                                                    "UpdateDate='" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                    "UpdateBy=N'" + Username + "' WHERE LocCode='WIR1' AND LabelNo=" + LabelNoSelected + " ";
                                cmd = new SqlCommand(query, cnn.con);
                                cmd.ExecuteNonQuery();

                            }
                            catch (Exception ex)
                            {
                                if (ErrorText.Trim() == "")
                                {
                                    ErrorText = "Update DB : " + ex.Message;
                                }
                                else
                                {
                                    ErrorText = ErrorText + "\nUpdate DB : " + ex.Message;
                                }
                            }
                            cnn.con.Close();

                            //Print
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
                                        worksheet.Cells[2, 4] = LabelNoSelected;
                                        worksheet.Cells[3, 1] = "*" + LabelNoSelected + "*";
                                        worksheet.Cells[7, 4] = CodeSelected;
                                        worksheet.Cells[9, 4] = LbItems.Text;
                                        worksheet.Cells[11, 4] = LbType.Text;
                                        worksheet.Cells[13, 4] = LbMaker.Text;
                                        worksheet.Cells[15, 4] = Qty;
                                        //Summary
                                        worksheet.Cells[15, 6] = "( " + QtyDetails + " )\n" + LbBobbinQty.Text.Replace("( ", "").Replace(" )", "");

                                        // Saving the modified Excel file
                                        var CDirectory2 = Environment.CurrentDirectory;
                                        string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                        string file = LabelNoSelected.ToString();
                                        string fName = file + "-Update( " + date + " )";

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

                            Cursor = Cursors.Default;
                            if (ErrorText.Trim() == "")
                            {
                                LbStatus.Text = "អាប់ដេតរួចរាល់!";
                                LbStatus.Refresh();
                                MessageBox.Show("អាប់ដេតរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ClearAllText();
                                txtBarcode.Focus();
                            }
                            else
                            {
                                LbStatus.Text = "អាប់ដេតមានបញ្ហា!";
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
                        DialogResult DSL = MessageBox.Show("តើអ្នកចង់អាប់ដេតទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (DSL == DialogResult.Yes)
                        {
                            LbStatus.Text = "កំពុងអាប់ដេត . . .";
                            LbStatus.Refresh();
                            Cursor = Cursors.WaitCursor;

                            ErrorText = "";
                            int Qty = Convert.ToInt32(LbTotalQty.Text.Replace(",", ""));
                            string QtyDetails = txtQty.Text.Replace("*", "x");
                            string QtyDetails2 = BobbinQtyInputted.ToString();

                            //Update DB
                            try
                            {
                                cnn.con.Open();                                
                                string Username = MenuFormV2.UserForNextForm;
                                DateTime UpdateDate = DateTime.Now;

                                string query = "UPDATE tbInventory SET Qty=" + Qty + ", " +
                                                    "QtyDetails='" + QtyDetails + "', " +
                                                    "QtyDetails2='" + QtyDetails2 + "', " +
                                                    "UpdateDate='" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                    "UpdateBy=N'" + Username + "' WHERE LocCode='WIR1' AND LabelNo=" + LabelNoSelected + " ";
                                cmd = new SqlCommand(query, cnn.con);
                                cmd.ExecuteNonQuery();

                            }
                            catch (Exception ex)
                            {
                                if (ErrorText.Trim() == "")
                                {
                                    ErrorText = "Update DB : " + ex.Message;
                                }
                                else
                                {
                                    ErrorText = ErrorText + "\nUpdate DB : " + ex.Message;
                                }
                            }
                            cnn.con.Close();

                            //Print
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
                                        worksheet.Cells[2, 4] = LabelNoSelected;
                                        worksheet.Cells[3, 1] = "*" + LabelNoSelected + "*";
                                        worksheet.Cells[7, 4] = CodeSelected;
                                        worksheet.Cells[9, 4] = LbItems.Text;
                                        worksheet.Cells[11, 4] = LbType.Text;
                                        worksheet.Cells[13, 4] = LbMaker.Text;
                                        worksheet.Cells[15, 4] = Qty;
                                        //Summary
                                        worksheet.Cells[15, 6] = "( " + QtyDetails + " )\n" + LbBobbinQty.Text.Replace("( ", "").Replace(" )", "");

                                        // Saving the modified Excel file
                                        var CDirectory2 = Environment.CurrentDirectory;
                                        string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                        string file = LabelNoSelected.ToString();
                                        string fName = file + "-Update( " + date + " )";

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


                            Cursor = Cursors.Default;
                            if (ErrorText.Trim() == "")
                            {
                                LbStatus.Text = "អាប់ដេតរួចរាល់!";
                                LbStatus.Refresh();
                                MessageBox.Show("អាប់ដេតរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ClearAllText();
                                txtBarcode.Focus();
                            }
                            else
                            {
                                LbStatus.Text = "អាប់ដេតមានបញ្ហា!";
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


        //Cbo
        private void CboBobbinW_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtQty.Text.Trim() != "")
            {
                string QtyDetails = txtQty.Text;
                //split betwen + Symbol.
                string[] QtyArray = QtyDetails.Split('+');

                //If Count By Reil
                if (LbBobbinsWTitle.Text.ToString().Contains("R1") == true && LbNetWTitle.Text.ToString().Contains("R2") == true)
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

        //txt
        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.Trim() != "")
                {
                    ClearAllText();
                    ErrorText = "";
                    DataTable dtMCDB = new DataTable();
                    DataTable dtRMOBS = new DataTable();
                    try
                    {
                        cnn.con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT LocCode, LabelNo, CountingMethod2, ItemCode, Qty, QtyDetails, QtyDetails2, RMType, BobbinsOrReil, R1OrNetW, MOQ,R2OrBobbinsW  FROM " +
                            "(SELECT * FROM tbInventory) T1 " +
                            "LEFT JOIN " +
                            "(SELECT * FROM tbSDMstUncountMat) T2 " +
                            "ON T1.ItemCode=T2.Code WHERE T1.CancelStatus=0 AND T1.LocCode='WIR1' AND T1.LabelNo='" + txtBarcode.Text+"'",cnn.con);
                        sda.Fill(dtMCDB);

                    }
                    catch (Exception ex)
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
                        if (dtMCDB.Rows.Count > 0)
                        {
                            try
                            {
                                cnnOBS.conOBS.Open();
                                SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemName, Resv1 FROM mstitem WHERE ItemType=2 AND ItemCode='" + dtMCDB.Rows[0][3].ToString() +"'", cnnOBS.conOBS);
                                sda.Fill(dtRMOBS);

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
                            cnnOBS.conOBS.Close();

                            if (ErrorText.Trim() == "")
                            {
                                txtQty.Text = "";
                                CountType = dtMCDB.Rows[0][2].ToString();
                                CodeSelected = dtMCDB.Rows[0][3].ToString();
                                LbCode.Text = CodeSelected;
                                LbItems.Text = dtRMOBS.Rows[0][0].ToString();
                                LbMaker.Text = dtRMOBS.Rows[0][1].ToString();
                                LbType.Text= dtMCDB.Rows[0][7].ToString();
                                TakeTerminalOrWireBobbinsW();
                                if (CountType == "Weight")
                                {
                                    ShowWeight();
                                    //Separate Detail
                                    string[] DetailArray = dtMCDB.Rows[0][6].ToString().Split('|');
                                    CboBobbinW.Text = DetailArray[0].ToString();
                                    if (dtMCDB.Rows[0][8].ToString() == "Reel")
                                    {
                                        LbBobbinsWTitle.Text = "ប្រវែងពេញ R1 (mm)";
                                        LbNetWTitle.Text = "ប្រវែងគ្មានធើមីណល R2 (mm)";
                                        LbNetWTitle.Font = new Font("Khmer OS Battambang", 11, FontStyle.Regular);
                                        LbQtyTitle.Text = "ប្រវែងសរុប";
                                        CboBobbinW.Items.Clear();
                                        CboBobbinW.Items.Add(DetailArray[0].ToString());
                                        CboBobbinW.Text = DetailArray[0].ToString();
                                        CboBobbinW.Enabled=false;
                                    }
                                    else
                                    {
                                        LbBobbinsWTitle.Text = "ទម្ងន់ប៊ូប៊ីន (KG)";
                                        LbNetWTitle.Text = "ទម្ងន់សាច់សុទ្ធ (KG)";
                                        LbNetWTitle.Font = new Font("Khmer OS Battambang", 14);
                                        LbQtyTitle.Text = "ទម្ងន់សរុប";
                                    }
                                }
                                else
                                {
                                    LbBobbinsWTitle.Text = "ទម្ងន់ប៊ូប៊ីន (KG)";
                                    LbNetWTitle.Text = "ទម្ងន់សាច់សុទ្ធ (KG)";
                                    LbNetWTitle.Font = new Font("Khmer OS Battambang", 14);
                                    LbQtyTitle.Text = "សម្រាយចំនួន";
                                    ShowBox();
                                    CboBobbinW.Items.Clear();
                                    CboBobbinW.Items.Add(dtMCDB.Rows[0][11].ToString());
                                    CboBobbinW.Text = dtMCDB.Rows[0][11].ToString();
                                    CboBobbinW.Enabled = false;
                                }

                                NetWSelected = Convert.ToDouble(dtMCDB.Rows[0][9].ToString());
                                MOQSelected = Convert.ToDouble(dtMCDB.Rows[0][10].ToString());
                                LbNetW.Text= NetWSelected.ToString()+" = "+MOQSelected.ToString("N0"); 
                                txtQty.Text= dtMCDB.Rows[0][5].ToString().Replace("x","*");
                                LabelNoSelected = Convert.ToInt32(txtBarcode.Text);
                                LbCode.Text = CodeSelected + "                                   លេខរៀងឡាប៊ែល   ៖   " + LabelNoSelected;
                                txtBarcode.Text = "";
                                txtQty.Focus();
                            }
                            else
                            {
                                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("គ្មានទិន្នន័យឡាប៊ែលនេះទេ!","Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                            txtBarcode.Focus();
                            txtBarcode.SelectAll();
                        }
                    }
                    else
                    {
                        MessageBox.Show("មានបញ្ហា!\n"+ErrorText,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
            }
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
                                if (LbBobbinsWTitle.Text.ToString().Contains("R1") == true && LbNetWTitle.Text.ToString().Contains("R2") == true)
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
                            if (QtyArray[i].ToString().Trim() != "")
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
                            if (LbBobbinsWTitle.Text.ToString().Contains("R1") == true && LbNetWTitle.Text.ToString().Contains("R2") == true)
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

        private void MCSDUpdateForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.MCSDPrintStatus = chkPrintStatus.Checked;
            Properties.Settings.Default.Save();
        }
        private void MCSDUpdateForm_Load(object sender, EventArgs e)
        {
            chkPrintStatus.Checked = Properties.Settings.Default.MCSDPrintStatus;
            ErrorText = "";
        }

        //Function
        private void ShowWeight()
        {
            groupBox3.Text = "ព៌ត័មានរបស់វត្ថុធាតុដើម ( Count by " + CountType + " )";
            //Enable CboBobbinW
            CboBobbinW.Enabled = true;
        }
        private void ShowBox()
        {
            groupBox3.Text = "ព៌ត័មានរបស់វត្ថុធាតុដើម ( Count by " + CountType + " )";
            //Enable CboBobbinW
            CboBobbinW.Enabled = false;
        }
        private void ClearAllText()
        {
            LbCode.Text = "";
            LbItems.Text = "";
            LbMaker.Text = "";
            LbType.Text = "";
            CboBobbinW.Items.Clear();
            CboBobbinW.Text = "";
            LbNetW.Text = "";
            txtQty.Text = "";
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
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
