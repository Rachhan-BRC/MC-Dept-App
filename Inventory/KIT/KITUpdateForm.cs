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

namespace MachineDeptApp.Inventory.KIT
{
    public partial class KITUpdateForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SqlCommand cmd;
        double ValueBeforeUpdate;
        string TypeOfCounting;

        public KITUpdateForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += KITUpdateForm_Load;
            this.FormClosed += KITUpdateForm_FormClosed;
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;
            this.btnNew.Click += BtnNew_Click;
            this.btnSave.Click += BtnSave_Click;

            //POS
            this.dgvPOSList.CellFormatting += DgvPOSList_CellFormatting;
            this.dgvPOSList.CellBeginEdit += DgvPOSList_CellBeginEdit;
            this.dgvPOSList.CellEndEdit += DgvPOSList_CellEndEdit;

            //Fraction
            this.txtQty.TextChanged += TxtQty_TextChanged;
            this.txtQty.KeyPress += TxtQty_KeyPress;

        }

        //POS
        private void DgvPOSList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                e.CellStyle.ForeColor = Color.Black;
            }
        }
        private void DgvPOSList_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 3)
                {
                    try
                    {
                        ValueBeforeUpdate = Convert.ToDouble(dgvPOSList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    }
                    catch
                    {

                    }
                }
            }
        }
        private void DgvPOSList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 3)
                {
                    try
                    {
                        double NewValue = Convert.ToDouble(dgvPOSList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                        NewValue = Convert.ToDouble(NewValue.ToString("N0"));
                        //if (NewValue <= Convert.ToDouble(dgvPOSList.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString()))
                        //{
                            if (NewValue > 0)
                            {
                                dgvPOSList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = NewValue;
                            }
                            else
                            {
                                MessageBox.Show("ចំនួនត្រូវតែធំជាង ០!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                dgvPOSList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ValueBeforeUpdate;
                            }
                        //}
                        //else
                        //{
                        //    MessageBox.Show("អ្នកបញ្ចូលលើសចំនួន POS ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //    dgvPOSList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ValueBeforeUpdate;
                        //}
                    }
                    catch
                    {
                        MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dgvPOSList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ValueBeforeUpdate;
                    }
                }
            }
        }


        //Fraction
        private void TxtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '+') && (e.KeyChar != '*'))
            {
                e.Handled = true;
            }
        }
        private void TxtQty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtQty.Text.Trim() != "")
                {
                    string summary = txtQty.Text.ToString();
                    string[] exception = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "*", "+" };
                    int countChar = 0;
                    for (int i = 0; i < summary.Length; i++)
                    {
                        int Found = 0;
                        for (int j = 0; j < exception.Length; j++)
                        {
                            if (summary[i].ToString() == exception[j].ToString())
                            {
                                Found = Found + 1;
                                break;
                            }
                        }
                        if (Found == 0)
                        {
                            countChar = countChar + 1;
                        }
                    }


                    if (countChar == 0)
                    {
                        string value = new DataTable().Compute(summary, null).ToString();
                        double Total = Convert.ToDouble(value);
                        LbFractQty.Text = Total.ToString("N0");

                    }
                    else
                    {
                        MessageBox.Show("អ្នកបញ្ចូលសម្រាយខុសទម្រង់ហើយសូមពិនិត្យម្ដងទៀត !\nត្រូវច្បាស់ថាអ្នកបញ្ចូលត្រឹមត្រូវហើយ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        LbFractQty.Text = "";
                        txtQty.Focus();
                    }
                }
                else
                {
                    LbFractQty.Text = "";
                }
            }
            catch
            {

            }
        }


        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearTextAll(); 
            txtBarcode.Focus();
        }
        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.Trim() != "")
                {
                    ClearTextAll();
                    //Select Inventory Data from MC DB
                    DataTable dtInventory = new DataTable();
                    DataTable dtPOSDetail = new DataTable();
                    string ErrorText = "";
                    try
                    {
                        cnn.con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT LabelNo, CountingMethod, SeqNo, ItemCode, Qty, QtyDetails FROM tbInventory " +
                            "WHERE CancelStatus = 0 AND LocCode ='KIT3' AND LabelNo="+txtBarcode.Text+" " +
                            "ORDER BY LabelNo ASC", cnn.con);
                        sda.Fill(dtInventory);
                    }
                    catch (Exception ex)
                    {
                        if (ErrorText.Trim() == "")
                        {
                            ErrorText = ex.Message;
                        }
                        else
                        {
                            ErrorText = ErrorText + "\n"+ ex.Message;
                        }
                    }
                    cnn.con.Close();


                    if (ErrorText.Trim() == "")
                    {
                        if (dtInventory.Rows.Count > 0)
                        {
                            string ItemCodeIn = "";
                            foreach (DataRow row in dtInventory.Rows)
                            {
                                if (ItemCodeIn.Trim() == "")
                                {
                                    ItemCodeIn = "'" + row[3].ToString() +"'";
                                }
                                else
                                {
                                    ItemCodeIn = ItemCodeIn + ", '" + row[3].ToString() + "'";
                                }
                            }
                            //Console.WriteLine(ItemCodeIn);

                            //Select ItemDetail from OBS DB
                            DataTable dtRMDetail = new DataTable();
                            try
                            {
                                cnnOBS.conOBS.Open();
                                SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName,MatTypeName, Resv1 FROM " +
                                    "(SELECT * FROM mstitem WHERE ItemType = 2 AND DelFlag=0 AND MatCalcFlag=0) T1 " +
                                    "INNER JOIN " +
                                    "(SELECT * FROM MstMatType WHERE DelFlag=0) T2 " +
                                    "ON T1.MatTypeCode=T2.MatTypeCode " +
                                    "WHERE ItemCode IN ("+ItemCodeIn+");", cnnOBS.conOBS);
                                sda.Fill(dtRMDetail);
                            }
                            catch (Exception ex)
                            {
                                if (ErrorText.Trim() == "")
                                {
                                    ErrorText = "OBS DB : " + ex.Message;
                                }
                                else
                                {
                                    ErrorText = ErrorText + "\nOBS DB : " + ex.Message;
                                }
                            }
                            cnnOBS.conOBS.Close();
                            
                            if (ErrorText.Trim() == "")
                            {
                                //In case count by POS
                                if (dtInventory.Rows[0][1].ToString() == "POS")
                                {
                                    TypeOfCounting = dtInventory.Rows[0][1].ToString();
                                    LbPOSLabelNo.Text = txtBarcode.Text;
                                    LbPOSNo.Text = dtInventory.Rows[0][5].ToString();

                                    //Select POS Detail from OBS DB
                                    try
                                    {
                                        cnnOBS.conOBS.Open();
                                        SqlDataAdapter sda = new SqlDataAdapter("SELECT DONo, ItemName, PlanQty, POSDeliveryDate FROM " +
                                            "(SELECT * FROM prgproductionorder) T1 " +
                                            "INNER JOIN " +
                                            "(SELECT * FROM mstitem WHERE ItemType=1 AND DelFlag=0) T2 " +
                                            "ON T1.ItemCode= T2.ItemCode " +
                                            "WHERE DONo='"+LbPOSNo.Text+"' ", cnnOBS.conOBS);
                                        DataTable dt = new DataTable();
                                        sda.Fill(dt);

                                        if (dt.Rows.Count > 0)
                                        {
                                            LbQty.Text = Convert.ToDouble(dt.Rows[0][2].ToString()).ToString("N0");
                                            LbItemName.Text = dt.Rows[0][1].ToString();
                                            LbShipmentDate.Text = Convert.ToDateTime(dt.Rows[0][3].ToString()).ToString("dd-MM-yyyy");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (ErrorText.Trim() == "")
                                        {
                                            ErrorText = "OBS DB : " + ex.Message;
                                        }
                                        else
                                        {
                                            ErrorText = ErrorText + "\nOBS DB : " + ex.Message;
                                        }
                                    }
                                    cnnOBS.conOBS.Close();

                                    if (ErrorText.Trim() == "")
                                    {
                                        foreach (DataRow InvRow in dtInventory.Rows)
                                        {
                                            string Code = InvRow[3].ToString();
                                            string RMName = "";
                                            foreach (DataRow RmRow in dtRMDetail.Rows)
                                            {
                                                if (Code == RmRow[0].ToString())
                                                {
                                                    RMName = RmRow[1].ToString();
                                                    break;
                                                }
                                            }
                                            double POSUsageQty = 0;
                                            double Qty = Convert.ToDouble(InvRow[4].ToString());
                                            dgvPOSList.Rows.Add(Code, RMName, POSUsageQty, Qty);
                                        }
                                        txtBarcode.Text = ""; 
                                        panelPOS.BringToFront();
                                        dgvPOSList.ClearSelection();
                                        dgvPOSList.Focus();
                                    }
                                    else
                                    {
                                        MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        txtBarcode.Focus();
                                    }
                                }
                                //In cse count by Fraction
                                else
                                {
                                    TypeOfCounting = dtInventory.Rows[0][1].ToString();
                                    LbFractLabelNo.Text = dtInventory.Rows[0][0].ToString();
                                    LbCode.Text = dtInventory.Rows[0][3].ToString();
                                    foreach (DataRow row in dtRMDetail.Rows)
                                    {
                                        if (LbCode.Text.ToString() == row[0].ToString())
                                        {
                                            LbFractItems.Text = row[1].ToString();
                                            LbFractType.Text = row[2].ToString();
                                            LbFractMaker.Text = row[3].ToString();
                                        }
                                    }
                                    txtQty.Text = dtInventory.Rows[0][5].ToString().Replace("x", "*");
                                    LbFractQty.Text = Convert.ToDouble(dtInventory.Rows[0][4].ToString()).ToString("N0");
                                    txtBarcode.Text = "";
                                    panelFraction.BringToFront();
                                    txtQty.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtBarcode.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("គ្មានទិន្នន័យរាប់ស្តុករបស់ឡាប៊ែលនេះទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtBarcode.Focus();
                            txtBarcode.SelectAll();
                        }
                    }
                    else
                    {
                        MessageBox.Show("មានបញ្ហា!\n"+ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.Focus();
                    }
                }
            }
        }
        private void KITUpdateForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.KITPrintStatus = chkPrintStatus.Checked;
            Properties.Settings.Default.Save();
        }
        private void KITUpdateForm_Load(object sender, EventArgs e)
        {
            panelFraction.BringToFront();
            chkPrintStatus.Checked = Properties.Settings.Default.KITPrintStatus;

            //Set ReadOnly to Columns of dgvPOSList
            foreach (DataGridViewColumn DgvCol in dgvPOSList.Columns)
            {
                if (DgvCol.Index != 3)
                {
                    DgvCol.ReadOnly = true;
                }
                if (DgvCol.Index == 2)
                {
                    DgvCol.Visible = false;
                }
            }

            txtBarcode.Focus();

        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (TypeOfCounting == "POS")
            {
                UpdatePOS();
            }
            else
            {
                UpdateFraction();
            }
        }


        private void ClearTextAll()
        {
            LbPOSLabelNo.Text = "";
            LbPOSNo.Text = "";
            LbQty.Text = "";
            LbItemName.Text = "";
            LbShipmentDate.Text = "";
            dgvPOSList.Rows.Clear();
            LbFractLabelNo.Text = "";
            LbCode.Text = "";
            LbFractItems.Text = "";
            LbFractMaker.Text = "";
            LbFractType.Text = "";
            LbFractQty.Text = "";
            txtQty.Text = "";
        }
        private void UpdatePOS()
        {
            if (dgvPOSList.Rows.Count > 0 && LbPOSLabelNo.Text.Trim()!="")
            {
                if (dgvPOSList.Rows.Count > 0)
                {
                    DialogResult DSL = MessageBox.Show("តើអ្នកចង់អាប់ដេតទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DSL == DialogResult.Yes)
                    {
                        string ErrorText = "";
                        LbStatus.Text = "កំពុងអាប់ដេត​ . . .";
                        LbStatus.Refresh();
                        Cursor = Cursors.WaitCursor;

                        try
                        {
                            cnn.con.Open();
                            DateTime UpdateDate = DateTime.Now;

                            foreach (DataGridViewRow DgvRow in dgvPOSList.Rows)
                            {
                                string Code = DgvRow.Cells[0].Value.ToString();
                                int Qty = Convert.ToInt32(DgvRow.Cells[3].Value.ToString());
                                string UserName = MenuFormV2.UserForNextForm;

                                string query = "UPDATE tbInventory SET Qty=" + Qty + ", " +
                                                        "UpdateDate='" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                        "UpdateBy=N'" + UserName + "' WHERE LocCode='KIT3' AND LabelNo=" + Convert.ToInt32(LbPOSLabelNo.Text) + " AND ItemCode='"+Code+"' ";
                                cmd = new SqlCommand(query, cnn.con);
                                cmd.ExecuteNonQuery();
                            }
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

                        //Print
                        if (chkPrintStatus.Checked == true)
                        {
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
                                Excel.Worksheet wsBarcode = (Excel.Worksheet)xlWorkBook.Sheets["POS"];

                                //ក្បាលលើ
                                wsBarcode.Cells[2, 3] = LbPOSLabelNo.Text.ToString();
                                wsBarcode.Cells[3, 1] = "*" + LbPOSLabelNo.Text.ToString() + "*";
                                wsBarcode.Cells[2, 4] = "ម៉ាស៊ីនឃីត (ជាន់លើ)";
                                wsBarcode.Cells[4, 3] = LbPOSNo.Text;
                                wsBarcode.Cells[5, 3] = LbItemName.Text.ToString();
                                wsBarcode.Cells[6, 3] = LbQty.Text.ToString();

                                // Material List
                                for (int i = 0; i < dgvPOSList.Rows.Count; i++)
                                {
                                    wsBarcode.Cells[i + 10, 1] = dgvPOSList.Rows[i].Cells[0].Value.ToString();
                                    wsBarcode.Cells[i + 10, 2] = dgvPOSList.Rows[i].Cells[1].Value.ToString();
                                    wsBarcode.Cells[i + 10, 5] = dgvPOSList.Rows[i].Cells[3].Value.ToString();

                                }

                                //លុប Manual worksheet
                                Excel.Worksheet wsManual = (Excel.Worksheet)xlWorkBook.Sheets["Fraction"];
                                excelApp.DisplayAlerts = false;
                                wsManual.Delete();
                                excelApp.DisplayAlerts = true;

                                //ព្រីនចេញ
                                for (int j = 0; j < numPrintQty.Value; j++)
                                {
                                    wsBarcode.PrintOut();
                                }


                                //Save Excel ទុក
                                string fName = "";
                                string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                string file = LbPOSLabelNo.Text.ToString();
                                wsBarcode.Name = "RachhanSystem";
                                fName = file + "-Update( " + date + " )";
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

                        Cursor = Cursors.Default;
                        if (ErrorText.Trim() == "")
                        {
                            LbStatus.Text = "អាប់ដេតរួចរាល់!";
                            LbStatus.Refresh();
                            MessageBox.Show("អាប់ដេតរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LbStatus.Text = "";
                            LbStatus.Refresh();
                            ClearTextAll();
                            txtBarcode.Focus();
                        }
                        else
                        {
                            LbStatus.Text = "មានបញ្ហា!";
                            LbStatus.Refresh();
                            MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("គ្មានទិន្នន័យសម្រាប់អាប់ដេតទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtBarcode.Focus();
                }                
            }
            else
            {
                MessageBox.Show("សូមស្កេនបាកូដជាមុនសិន!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBarcode.Focus();
            }
        }
        private void UpdateFraction()
        {
            if (LbFractQty.Text.Trim() != "" && LbFractLabelNo.Text!="")
            {
                if (LbFractQty.Text.Trim() != "")
                {
                    DialogResult DSL = MessageBox.Show("តើអ្នកចង់អាប់ដេតទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DSL == DialogResult.Yes)
                    {
                        string ErrorText = "";
                        LbStatus.Text = "កំពុងអាប់ដេត​ . . .";
                        LbStatus.Refresh();
                        Cursor = Cursors.WaitCursor;

                        try
                        {
                            cnn.con.Open();
                            int Qty = Convert.ToInt32(LbFractQty.Text.ToString().Replace(",",""));
                            string QtyDetails = txtQty.Text.ToString().Replace("*", "x");
                            DateTime UpdateDate = DateTime.Now;
                            string UserName = MenuFormV2.UserForNextForm;

                            string query = "UPDATE tbInventory SET Qty=" + Qty + ", " +
                                                    "QtyDetails='" + QtyDetails + "', " +
                                                    "UpdateDate='" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                    "UpdateBy=N'" + UserName + "' WHERE LocCode='KIT3' AND LabelNo=" + Convert.ToInt32(LbFractLabelNo.Text) + " ";
                            cmd = new SqlCommand(query, cnn.con);
                            cmd.ExecuteNonQuery();
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

                        //Print
                        if (chkPrintStatus.Checked == true)
                        {
                            if (ErrorText.Trim() == "")
                            {
                                try
                                {
                                    //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                                    string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\InventoryLable\KIT3";
                                    if (!Directory.Exists(SavePath))
                                    {
                                        Directory.CreateDirectory(SavePath);
                                    }

                                    //open excel application and create new workbook
                                    var CDirectory = Environment.CurrentDirectory;
                                    Excel.Application excelApp = new Excel.Application();
                                    Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\InventoryLabel_KIT_Template.xlsx", Editable: true);
                                    Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Fraction"];

                                    worksheet.Cells[2, 6] = "ម៉ាស៊ីនឃីត (ជាន់លើ)";
                                    worksheet.Cells[2, 4] = LbFractLabelNo.Text;
                                    worksheet.Cells[3, 1] = "*" + LbFractLabelNo.Text + "*";
                                    worksheet.Cells[7, 4] = LbCode.Text;
                                    worksheet.Cells[9, 4] = LbFractItems.Text;
                                    worksheet.Cells[11, 4] = LbFractType.Text;
                                    worksheet.Cells[13, 4] = LbFractMaker.Text;
                                    worksheet.Cells[16, 4] = LbFractQty.Text;
                                    //Summary
                                    worksheet.Cells[16, 6] = "( " + txtQty.Text.ToString().Replace("*", "x") + " )";

                                    // Saving the modified Excel file
                                    var CDirectory2 = Environment.CurrentDirectory;
                                    string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                    string file = LbFractLabelNo.Text.ToString();
                                    string fName = file + "-Update( " + date + " )";

                                    //លុប Manual worksheet
                                    Excel.Worksheet wsBarcode = (Excel.Worksheet)xlWorkBook.Sheets["POS"];
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
                            LbStatus.Text = "";
                            LbStatus.Refresh();
                            ClearTextAll();
                            txtBarcode.Focus();
                        }
                        else
                        {
                            LbStatus.Text = "មានបញ្ហា!";
                            LbStatus.Refresh();
                            MessageBox.Show("មានបញ្ហា!\n"+ErrorText,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);                            
                        }
                    }
                }
                else
                {
                    MessageBox.Show("សូមបញ្ចូល  < ចំនួន >  ដើម្បីអាប់ដេត!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtQty.Focus();
                }
            }
            else
            {
                MessageBox.Show("សូមស្កេនបាកូដជាមុនសិន!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBarcode.Focus();
            }
        }

    }
}
