using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.Inventory.KIT
{
    public partial class KITCountingForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        DataTable dtRMMaster;
        SqlCommand cmd;
        string LocStart;
        double ValueBeforeUpdate;
        string ValueBeforeUpdateFranct; 
        string ErrorText = "";


        public KITCountingForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += CountingForm_Load;
            this.btnStart.Click += BtnStart_Click;
            this.FormClosed += KITCountingForm_FormClosed;
            this.btnSave.Click += BtnSave_Click;
            this.btnNew.Click += BtnNew_Click;

            //Count for POS
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;
            this.dgvPOSList.CellFormatting += DgvPOSList_CellFormatting;
            this.dgvPOSList.KeyDown += DgvPOSList_KeyDown;
            this.dgvPOSList.CellBeginEdit += DgvPOSList_CellBeginEdit;
            this.dgvPOSList.CellEndEdit += DgvPOSList_CellEndEdit;

            //Count for Fraction
            this.txtItems.KeyDown += TxtItems_KeyDown;
            this.btnShowDgvItems.Click += BtnShowDgvItems_Click;
            this.txtItems.TextChanged += TxtItems_TextChanged;
            this.dgvItems.CellClick += DgvItems_CellClick;
            this.txtQty.TextChanged += TxtQty_TextChanged;
            this.txtQty.KeyPress += TxtQty_KeyPress;
            this.txtQty.KeyDown += TxtQty_KeyDown;
            this.dgvFractList.CellFormatting += DgvFractList_CellFormatting;
            this.dgvFractList.CellBeginEdit += DgvFractList_CellBeginEdit;
            this.dgvFractList.CellEndEdit += DgvFractList_CellEndEdit;

        }


        //Count for POS
        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.ToString().Trim() != "")
                {
                    if (txtBarcode.Text.Length < 10 || (Regex.Matches(txtBarcode.Text.ToString(), "[~!@#$%^&*()_+{}:\"<>?-]").Count) > 1 || (Regex.Matches(txtBarcode.Text.ToString(), "[-]").Count) < 1 || txtBarcode.Text.ToString().Trim()[2].ToString() != "-")
                    {
                        MessageBox.Show("លេខ​ POS ផ្ដើមខុសទម្រង់ហើយ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.Focus();
                        txtBarcode.SelectAll();
                    }
                    else
                    {
                        dgvPOSList.Rows.Clear();
                        DataTable dtPOSDetails = new DataTable();
                        DataTable dtPOSConsumpt = new DataTable();
                        ClearTextPos();
                        string POS = txtBarcode.Text;
                        DataTable dtAlreadyCount = new DataTable();
                        string ErrorText = "";

                        //ឆែកមើលថាធ្លាប់បញ្ចូលឬអត់?
                        try
                        {
                            cnn.con.Open();
                            SqlDataAdapter da = new SqlDataAdapter("SELECT QtyDetails AS POSNo FROM tbInventory WHERE LocCode='KIT3' AND SubLoc='KIT-POS' AND CancelStatus = 0 AND QtyDetails ='" + POS + "'", cnn.con);
                            da.Fill(dtAlreadyCount);
                        }
                        catch (Exception ex)
                        {
                            ErrorText = "MC DB : " + ex.Message;
                        }
                        cnn.con.Close();

                        //Take POS Detail & Consumption from OBS
                        if (dtAlreadyCount.Rows.Count == 0 && ErrorText.Trim() == "")
                        {
                            //Take POS Detail & Consumption from OBS
                            try
                            {
                                cnnOBS.conOBS.Open();
                                //Take POS Detail 
                                SqlDataAdapter sda = new SqlDataAdapter("SELECT DONo, ItemName, PlanQty, POSDeliveryDate FROM " +
                                    "(SELECT * FROM prgproductionorder) T1 " +
                                    "INNER JOIN " +
                                    "(SELECT * FROM mstitem WHERE DelFlag=0 AND ItemType=1) T2 " +
                                    "ON T1.ItemCode = T2.ItemCode " +
                                    "WHERE DONo='" + POS + "' ", cnnOBS.conOBS);
                                sda.Fill(dtPOSDetails);

                                //Take POS Consumption
                                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT DONo, ConsumpSeqNo, T2.ItemCode, ItemName, ConsumpQty FROM " +
                                    "(SELECT * FROM prgproductionorder) T1 " +
                                    "INNER JOIN " +
                                    "(SELECT * FROM prgconsumtionorder) T2 " +
                                    "ON T1.ProductionCode=T2.ProductionCode " +
                                    "INNER JOIN " +
                                    "(SELECT * FROM mstitem WHERE DelFlag =0 AND ItemType=2 AND MatCalcFlag=0) T3 " +
                                    "ON T2.ItemCode=T3.ItemCode " +
                                    "WHERE DONo = '" + POS + "' " +
                                    "ORDER BY ConsumpSeqNo ASC", cnnOBS.conOBS);
                                sda1.Fill(dtPOSConsumpt);

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

                            //Assign POS Details
                            if (dtPOSDetails.Rows.Count > 0)
                            {
                                if (dtPOSConsumpt.Rows.Count > 0)
                                {
                                    LbPOSNo.Text = dtPOSDetails.Rows[0][0].ToString();
                                    LbItemName.Text = dtPOSDetails.Rows[0][1].ToString();
                                    LbQty.Text = Convert.ToDouble(dtPOSDetails.Rows[0][2]).ToString("N0");
                                    LbShipmentDate.Text = Convert.ToDateTime(dtPOSDetails.Rows[0][3]).ToString("dd-MM-yyyy");

                                    //Assign POS Consumption
                                    foreach (DataRow row in dtPOSConsumpt.Rows)
                                    {
                                        string Code = row[2].ToString();
                                        string RMName = row[3].ToString();
                                        double Qty = Convert.ToDouble(row[4].ToString());

                                        dgvPOSList.Rows.Add(Code, RMName, Qty, Qty);
                                        AssignHeaderCellValuePOS();
                                    }
                                    dgvPOSList.ClearSelection();
                                    txtBarcode.Text = "";
                                    btnSave.Focus();
                                }
                                else
                                {
                                    MessageBox.Show("POS នេះមិនប្រើប្រាស់ខនិកទ័រទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    txtBarcode.Focus();
                                    txtBarcode.SelectAll();
                                }
                            }
                            else
                            {
                                MessageBox.Show("គ្មាន POS នេះនៅក្នុងប្រព័ន្ធទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                txtBarcode.Focus();
                                txtBarcode.SelectAll();
                            }
                        }
                        else if (dtAlreadyCount.Rows.Count > 0 && ErrorText.Trim() == "")
                        {

                            MessageBox.Show("លេខ​ POS នេះស្កេនម្ដងរួចហើយ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtBarcode.Focus();
                            txtBarcode.SelectAll();
                        }
                        else
                        {
                            MessageBox.Show("មានបញ្ហាអ្វីមួយ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ!\nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private void DgvPOSList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                e.CellStyle.ForeColor = Color.Black;
            }
        }
        private void DgvPOSList_KeyDown(object sender, KeyEventArgs e)
        {
            //Delete
            if (e.KeyCode == Keys.Delete)
            {
                if (dgvPOSList.SelectedCells.Count > 0)
                {
                    DialogResult DSL = MessageBox.Show("តើអ្នកចង់លុបវត្ថុធាតុដើមនេះមែនដែរឬទេ", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DSL == DialogResult.Yes)
                    {
                        dgvPOSList.Rows.RemoveAt(dgvPOSList.CurrentCell.RowIndex);
                        AssignHeaderCellValuePOS();
                        dgvPOSList.Update();
                        dgvPOSList.ClearSelection();
                    }
                }
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


        //Count for Fraction
        private void KITCountingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.KITPrintStatus = Convert.ToBoolean(chkPrintStatus.Checked);
            Properties.Settings.Default.Save();
        }
        private void TxtItems_TextChanged(object sender, EventArgs e)
        {
            dgvItems.Rows.Clear();
            if (txtItems.Text.Trim() != "")
            {
                foreach (DataRow row in dtRMMaster.Rows)
                {
                    if (row[0].ToString().ToLower().Contains(txtItems.Text.ToLower()) == true || row[1].ToString().ToLower().Contains(txtItems.Text.ToLower()) == true)
                    {
                        dgvItems.Rows.Add(row[0], row[1]);
                    }
                }
            }
            else
            {
                foreach (DataRow row in dtRMMaster.Rows)
                {
                    dgvItems.Rows.Add(row[0], row[1]);
                }
            }
            dgvItems.ClearSelection();
        }
        private void BtnShowDgvItems_Click(object sender, EventArgs e)
        {
            dgvItems.Rows.Clear();
            foreach (DataRow row in dtRMMaster.Rows)
            {
                dgvItems.Rows.Add(row[0], row[1]);
            }
            dgvItems.ClearSelection();
            if (dgvItems.Visible == true)
            {
                dgvItems.Visible = false;
            }
            else
            {
                dgvItems.Visible = true;
            }
            txtItems.Focus();
        }
        private void TxtItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtItems.Text.Trim() != "")
                {
                    ClearTextFraction();
                    foreach (DataRow row in dtRMMaster.Rows)
                    {
                        if (row[0].ToString() == txtItems.Text)
                        {
                            LbCode.Text = row[0].ToString();
                            LbFractItems.Text = row[1].ToString();
                            LbFractType.Text = row[2].ToString();
                            LbFractMaker.Text = row[3].ToString();
                            txtItems.Text = "";
                            txtQty.Focus();
                        }
                    }
                    if (LbCode.Text.Trim() == "")
                    {
                        MessageBox.Show("គ្មានលេខកូដនេះនៅក្នុងប្រព័ន្ធទេ!","Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                        txtItems.Focus();
                        txtItems.SelectAll();
                    }
                }
            }
        }
        private void DgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                LbCode.Text = dgvItems.Rows[e.RowIndex].Cells[0].Value.ToString();
                foreach (DataRow row in dtRMMaster.Rows)
                {
                    if (LbCode.Text == row[0].ToString())
                    {
                        LbFractItems.Text = row[1].ToString();
                        LbFractType.Text = row[2].ToString();
                        LbFractMaker.Text = row[3].ToString();
                    }
                }
                dgvItems.Visible = false;
                txtItems.Text = "";
                txtQty.Focus();
            }
        }
        private void TxtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '+') && (e.KeyChar != '*'))
            {
                e.Handled = true;
            }
        }
        private void TxtQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (LbFractQty.Text.ToString().Trim() != "" || LbCode.Text.ToString().Trim() != "")
                {
                    string Code = LbCode.Text;
                    string Items = LbFractItems.Text;
                    string Type = LbFractType.Text;
                    string Maker = LbFractMaker.Text;
                    double Qty = Convert.ToDouble(LbFractQty.Text.ToString());
                    string QtySumm = txtQty.Text.ToString();
                    QtySumm = QtySumm.Replace("*", "x");
                    //SQL Label No
                    dgvFractList.Rows.Add(Code, Items, Type, Maker, Qty, QtySumm);
                    AssignHeaderCellValueFraction();
                    dgvFractList.ClearSelection();
                    ClearTextFraction();
                    txtQty.Text = "";
                    LbFractQty.Text = "";
                    txtItems.Focus();
                }
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
        private void DgvFractList_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                if (e.RowIndex > -1)
                {
                    ValueBeforeUpdateFranct = dgvFractList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(); 
                    string Edit = ValueBeforeUpdateFranct.Replace("x", "*");
                    dgvFractList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Edit;
                }
            }
        }
        private void DgvFractList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                e.CellStyle.ForeColor = Color.Black;
            }
        }
        private void DgvFractList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                if (e.RowIndex > -1)
                {

                    string summary = dgvFractList[e.ColumnIndex, e.RowIndex].Value.ToString();
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
                        dgvFractList[4, e.RowIndex].Value = Total;
                        summary = summary.Replace("*", "x");
                        dgvFractList[e.ColumnIndex, e.RowIndex].Value = summary;

                    }
                    else
                    {
                        MessageBox.Show("អ្នកបញ្ចូលសម្រាយខុសទម្រង់ហើយសូមពិនិត្យម្ដងទៀត !\nត្រូវច្បាស់ថាអ្នកបញ្ចូលត្រឹមត្រូវហើយ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dgvFractList[e.ColumnIndex, e.RowIndex].Value = ValueBeforeUpdateFranct;
                    }
                }
            }
        }


        private void CountingForm_Load(object sender, EventArgs e)
        {
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

            //Set ReadOnly to Columns of dgvFractList
            foreach (DataGridViewColumn DgvCol in dgvFractList.Columns)
            {
                if (DgvCol.Index != 5)
                {
                    DgvCol.ReadOnly = true;
                }
            }

            //Take MasterRM from OBS
            try
            {
                cnnOBS.conOBS.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, MatTypeName, Resv1 FROM " +
                    "(SELECT * FROM mstitem WHERE ItemType = 2 AND MatCalcFlag = 0 AND DelFlag=0) T1 " +
                    "INNER JOIN " +
                    "(SELECT * FROM MstMatType WHERE DelFlag = 0) T2 " +
                    "ON T1.MatTypeCode=T2.MatTypeCode", cnnOBS.conOBS);
                dtRMMaster = new DataTable();   
                sda.Fill(dtRMMaster);
            }
            catch (Exception ex) 
            {
                MessageBox.Show("មានបញ្ហា!\n"+ex.Message,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            cnnOBS.conOBS.Close();


            LocStart = "";
            string [] LocArray = { "KIT-POS", "KIT-Fraction"};
            for (int i = 0; i < LocArray.Length; i++)
            {
                CboLoc.Items.Add(LocArray[i].ToString());
            }
            CboLoc.SelectedIndex = 0;
            chkPrintStatus.Checked = Properties.Settings.Default.KITPrintStatus;
            dgvItems.BringToFront();

        }
        private void BtnStart_Click(object sender, EventArgs e)
        {
            LocStart = "";
            if (CboLoc.Text.Trim() != "")
            {
                LocStart = CboLoc.Text;
                btnSave.Visible = true;
                btnNew.Visible = true;
                chkPrintStatus.Visible = true;
                numPrintQty.Visible = true;
                ShowPOSOrFraction();
            }
            else
            {
                MessageBox.Show("សូមជ្រើសរើសទីតាំងជាមុនសិន!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                CboLoc.Focus();
            }
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (LocStart == "KIT-POS" || LocStart == "KIT-Fraction")
            {
                if (LocStart == "KIT-POS")
                {
                    SavePOS();
                }
                else
                {
                    SaveFract();
                }
            }
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            if (LocStart == "KIT-POS")
            {
                ClearTextPos();
                dgvPOSList.Rows.Clear();
                txtBarcode.Focus();                
            }
            else
            {
                ClearTextFraction();
                dgvFractList.Rows.Clear();
                txtItems.Focus();
            }
        }

        //Function
        private void ShowPOSOrFraction()
        {
            if (LocStart.Contains("POS") == true)
            {
                panelFraction.Visible = false;
                if (panelPOS.Visible == false)
                {
                    panelPOS.Visible = true;
                    panelPOS.BringToFront();
                    txtBarcode.Focus();
                }
            }
            else
            {
                panelPOS.Visible = false;
                if (panelPOS.Visible == false)
                {
                    panelFraction.Visible = true;
                    panelFraction.BringToFront();
                    txtItems.Focus();
                }
            }
        }
        private void AssignHeaderCellValuePOS()
        {
            foreach (DataGridViewRow DgvRow in dgvPOSList.Rows)
            {
                DgvRow.HeaderCell.Value = (DgvRow.Index+1).ToString();
                dgvPOSList.Update();
            }
        }
        private void AssignHeaderCellValueFraction()
        {
            foreach (DataGridViewRow DgvRow in dgvFractList.Rows)
            {
                DgvRow.HeaderCell.Value = (DgvRow.Index + 1).ToString();
                dgvFractList.Update();
            }
        }
        private void ClearTextFraction()
        {
            LbCode.Text = "";
            LbFractItems.Text = "";
            LbFractType.Text = "";
            LbFractMaker.Text = "";
        }
        private void ClearTextPos()
        {
            LbItemName.Text = "";
            LbPOSNo.Text = "";
            LbQty.Text = "";
            LbShipmentDate.Text = "";

        }
        private void SavePOS()
        {
            if (dgvPOSList.Rows.Count>0 && LbQty.Text.Trim()!="")
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់រក្សាទុកទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;
                    DataTable dtLabelNo = new DataTable();
                    DataTable dtLabelNoRange = new DataTable();

                    //Check LabelNo
                    try
                    {
                        cnn.con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT LabelNo AS LastLabelNo FROM tbInventory WHERE LabelNo = (SELECT MAX(LabelNo) FROM tbInventory WHERE LocCode='KIT3') GROUP BY LabelNo", cnn.con);
                        sda.Fill(dtLabelNo);

                        SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM tbSDMCLocation WHERE LocCode='KIT3'", cnn.con);
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
                        string SubLoc = LocStart;
                        int LabelNo = Convert.ToInt32(dtLabelNoRange.Rows[0][2].ToString());
                        if (dtLabelNo.Rows.Count > 0)
                        {
                            LabelNo = Convert.ToInt32(dtLabelNo.Rows[0][0].ToString()) + 1;
                        }
                        string Username = MenuFormV2.UserForNextForm;
                        DateTime RegDate = DateTime.Now;

                        foreach (DataGridViewRow DgvRow in dgvPOSList.Rows)
                        {
                            try
                            {
                                cnn.con.Open();
                                string LocCode = "KIT3";
                                string CountingMethod = "POS";
                                string ItemCode = DgvRow.Cells[0].Value.ToString();
                                string ItemType = "Material";
                                int Qty = Convert.ToInt32(DgvRow.Cells[3].Value.ToString());
                                string QtyDetails = LbPOSNo.Text;

                                if (LabelNo <= Convert.ToInt32(dtLabelNoRange.Rows[0][3].ToString()))
                                {
                                    cmd = new SqlCommand("INSERT INTO tbInventory(LocCode, SubLoc, LabelNo, CountingMethod, SeqNo, ItemCode, ItemType, Qty, QtyDetails, RegDate, RegBy, UpdateDate, UpdateBy, CancelStatus) " +
                                    "VALUES (@Lc, @Slc, @lbn, @Cm, @Sn, @Ic, @It, @Qty, @QtyD, @RegD, @RegB, @UpD, @UpB, @Cs)", cnn.con);

                                    cmd.Parameters.AddWithValue("@Lc", LocCode);
                                    cmd.Parameters.AddWithValue("@Slc", SubLoc);
                                    cmd.Parameters.AddWithValue("@lbn", LabelNo);
                                    cmd.Parameters.AddWithValue("@Cm", CountingMethod);
                                    cmd.Parameters.AddWithValue("@Sn", DgvRow.Index + 1);
                                    cmd.Parameters.AddWithValue("@Ic", ItemCode);
                                    cmd.Parameters.AddWithValue("@It", ItemType);
                                    cmd.Parameters.AddWithValue("@Qty", Qty);
                                    cmd.Parameters.AddWithValue("@QtyD", QtyDetails);
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
                                    break;
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
                                break;
                            }
                            cnn.con.Close();
                        }
                        
                        //Print
                        if (chkPrintStatus.Checked == true)
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
                            wsBarcode.Cells[2, 3] = LabelNo.ToString();
                            wsBarcode.Cells[3, 1] = "*" + LabelNo.ToString() + "*";
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
                            string file = LabelNo.ToString();
                            wsBarcode.Name = "RachhanSystem";
                            fName = file + "( " + date + " )";
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
                        MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearTextPos();
                        dgvPOSList.Rows.Clear();
                        txtBarcode.Focus();
                    }
                    else
                    {
                        MessageBox.Show("រក្សាទុកមានបញ្ហា!\n"+ ErrorText, "Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("គ្មានទិន្នន័យ <បញ្ជីវត្ថុធាតុដើម> ដែលត្រូវរក្សាទុកទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBarcode.Focus();
            }
        }
        private void SaveFract()
        {
            if (dgvFractList.Rows.Count > 0)
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់រក្សាទុកទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(DSL == DialogResult.Yes)
                {
                    if (dgvFractList.Rows.Count > 0)
                    {
                        ErrorText = "";
                        Cursor = Cursors.WaitCursor;
                        DataTable dtLabelNo = new DataTable();
                        DataTable dtLabelNoRange = new DataTable();

                        //For Remove Successful rows
                        DataTable dtSaveSucceed = new DataTable();
                        dtSaveSucceed.Columns.Add("RowsIndex");

                        foreach (DataGridViewRow DgvRow in dgvFractList.Rows)
                        {
                            //Check LabelNo
                            try
                            {
                                cnn.con.Open();
                                SqlDataAdapter sda = new SqlDataAdapter("SELECT LabelNo AS LastLabelNo FROM tbInventory WHERE LabelNo = (SELECT MAX(LabelNo) FROM tbInventory WHERE LocCode='KIT3') GROUP BY LabelNo", cnn.con);
                                dtLabelNo=new DataTable();
                                sda.Fill(dtLabelNo);

                                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM tbSDMCLocation WHERE LocCode='KIT3'", cnn.con);
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
                                break;
                            }
                            cnn.con.Close();

                            //Insert data to DB
                            if (ErrorText.Trim() == "")
                            {
                                string LocCode = "KIT3";
                                string SubLoc = LocStart;
                                int LabelNo = Convert.ToInt32(dtLabelNoRange.Rows[0][2].ToString());
                                if (dtLabelNo.Rows.Count > 0)
                                {
                                    LabelNo = Convert.ToInt32(dtLabelNo.Rows[0][0].ToString()) + 1;
                                }
                                string CountingMethod = "StockCard";
                                string ItemCode = DgvRow.Cells[0].Value.ToString();
                                string ItemType = "Material";
                                int Qty = Convert.ToInt32(DgvRow.Cells[4].Value.ToString());
                                string QtyDetails = DgvRow.Cells[5].Value.ToString();
                                string Username = MenuFormV2.UserForNextForm;
                                DateTime RegDate = DateTime.Now;

                                try
                                {
                                    cnn.con.Open(); 
                                    if (LabelNo <= Convert.ToInt32(dtLabelNoRange.Rows[0][3].ToString()))
                                    {
                                        cmd = new SqlCommand("INSERT INTO tbInventory(LocCode, SubLoc, LabelNo, CountingMethod, SeqNo, ItemCode, ItemType, Qty, QtyDetails, RegDate, RegBy, UpdateDate, UpdateBy, CancelStatus) " +
                                        "VALUES (@Lc, @Slc, @lbn, @Cm, @Sn, @Ic, @It, @Qty, @QtyD, @RegD, @RegB, @UpD, @UpB, @Cs)", cnn.con);

                                        cmd.Parameters.AddWithValue("@Lc", LocCode);
                                        cmd.Parameters.AddWithValue("@Slc", SubLoc);
                                        cmd.Parameters.AddWithValue("@lbn", LabelNo);
                                        cmd.Parameters.AddWithValue("@Cm", CountingMethod);
                                        cmd.Parameters.AddWithValue("@Sn", 1);
                                        cmd.Parameters.AddWithValue("@Ic", ItemCode);
                                        cmd.Parameters.AddWithValue("@It", ItemType);
                                        cmd.Parameters.AddWithValue("@Qty", Qty);
                                        cmd.Parameters.AddWithValue("@QtyD", QtyDetails);
                                        cmd.Parameters.AddWithValue("@RegD", RegDate);
                                        cmd.Parameters.AddWithValue("@RegB", Username);
                                        cmd.Parameters.AddWithValue("@UpD", RegDate);
                                        cmd.Parameters.AddWithValue("@UpB", Username);
                                        cmd.Parameters.AddWithValue("@Cs", 0);

                                        cmd.ExecuteNonQuery();

                                        dtSaveSucceed.Rows.Add(DgvRow.Index.ToString());

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
                                        break;
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
                                    break;
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
                                            worksheet.Cells[2, 4] = LabelNo;
                                            worksheet.Cells[3, 1] = "*" + LabelNo + "*";
                                            worksheet.Cells[7, 4] = ItemCode;
                                            worksheet.Cells[9, 4] = DgvRow.Cells[1].Value.ToString();
                                            worksheet.Cells[11, 4] = ItemType;
                                            worksheet.Cells[13, 4] = DgvRow.Cells[3].Value.ToString();
                                            worksheet.Cells[16, 4] = Qty;
                                            //Summary
                                            worksheet.Cells[16, 6] = "( " + QtyDetails + " )";

                                            // Saving the modified Excel file
                                            var CDirectory2 = Environment.CurrentDirectory;
                                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                            string file = LabelNo.ToString();
                                            string fName = file + "( " + date + " )";

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
                                            break;
                                        }
                                    }
                                }                                
                            }
                        }

                        Cursor = Cursors.Default;
                        if (ErrorText.Trim() == "")
                        {
                            MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearTextFraction();
                            dgvFractList.Rows.Clear();
                            txtItems.Focus();
                        }
                        else
                        {
                            MessageBox.Show("រក្សាទុកមានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            for (int i = dtSaveSucceed.Rows.Count - 1; i > -1; i--)
                            {
                                dgvFractList.Rows.RemoveAt(Convert.ToInt32(dtSaveSucceed.Rows[i][0].ToString()));
                            }
                            dgvFractList.Update();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("គ្មានទិន្នន័យ <បញ្ជីវត្ថុធាតុដើម> ដែលត្រូវរក្សាទុកទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtItems.Focus();
            }
        }
        
    }
}
