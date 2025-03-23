using System;
using MachineDeptApp.MsgClass;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.Inventory.Inprocess
{
    public partial class InprocessCountingForm : Form
    {
        WarningMsgClass WMsg = new WarningMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        ErrorMsgClass EMsg = new ErrorMsgClass();
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SqlCommand cmd;
        DataTable dtColor;
        public static string CountType;
        public static int CountTypeChanged;
        string LocSelected;

        //POS-Connector
        double ValueBeforeUpdate;

        //WireTerminal
        string CountType2="Weight";
        string CodeSelected;
        string BobbinOrReilSelected;
        string LastTypeSelected;
        double NetWSelected;
        double MOQSelected;
        double BobbinQtyInputted;
        DataTable dtRM;



        string ErrorText;

        public InprocessCountingForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += InprocessCountingForm_Load;
            this.FormClosed += InprocessCountingForm_FormClosed;
            this.btnStart.Click += BtnStart_Click;
            this.btnNew.Click += BtnNew_Click;
            this.btnSave.Click += BtnSave_Click;
            this.txtCountingType.TextChanged += TxtCountingType_TextChanged;
            this.txtCountingType.Click += TxtCountingType_Click;
            this.PicCurrentCountingType.Click += PicCurrentCountingType_Click;

            //WireTerminal
            this.RdbBox.CheckedChanged += RdbBox_CheckedChanged;
            this.txtItems.KeyDown += TxtItems_KeyDown;
            this.btnShowDgvItems.Click += BtnShowDgvItems_Click;
            this.txtItems.TextChanged += TxtItems_TextChanged;
            this.dgvItems.CellClick += DgvItems_CellClick;
            this.CboBobbinWWireTerminal.SelectedIndexChanged += CboBobbinWWireTerminal_SelectedIndexChanged;
            this.txtQtyWireTerminal.KeyPress += TxtQtyWireTerminal_KeyPress;
            this.txtQtyWireTerminal.KeyDown += TxtQtyWireTerminal_KeyDown;
            this.txtQtyWireTerminal.TextChanged += TxtQtyWireTerminal_TextChanged;

            //POS-Connector
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;
            this.dgvRMListPOS.CellFormatting += DgvRMListPOS_CellFormatting;
            this.dgvRMListPOS.CellBeginEdit += DgvRMListPOS_CellBeginEdit;
            this.dgvRMListPOS.CellEndEdit += DgvRMListPOS_CellEndEdit;

            //Semi BC
            this.LbWireTubeSemiBC.TextChanged += LbWireTubeSemiBC_TextChanged;
            this.txtBarcodeSemi.KeyDown += TxtBarcodeSemi_KeyDown;
            this.txtBatchQtyBC.KeyPress += TxtBatchQtyBC_KeyPress;
            this.txtBatchQtyBC.KeyDown += TxtBatchQtyBC_KeyDown;
            this.txtQtyPerBatchBC.KeyPress += TxtQtyPerBatchBC_KeyPress;
            this.txtQtyPerBatchBC.KeyDown += TxtQtyPerBatchBC_KeyDown;
            this.txtQtyReayBC.KeyPress += TxtQtyReayBC_KeyPress;
            this.txtQtyReayBC.KeyDown += TxtQtyReayBC_KeyDown;

            this.tabCtrlSemi.SelectedIndexChanged += TabCtrlSemi_SelectedIndexChanged;

            //Semi
            this.txtWipNameSemi.KeyDown += TxtWipNameSemi_KeyDown;
            this.dgvSemi.CellFormatting += DgvSemi_CellFormatting;
            this.txtBatchQtySemi.KeyPress += TxtBatchQtySemi_KeyPress;
            this.txtBatchQtySemi.KeyDown += TxtBatchQtySemi_KeyDown;
            this.txtQtyPerBatchSemi.KeyPress += TxtQtyPerBatchSemi_KeyPress;
            this.txtQtyPerBatchSemi.KeyDown += TxtQtyPerBatchSemi_KeyDown;
            this.txtQtyReaySemi.KeyPress += TxtQtyReaySemi_KeyPress;
            this.txtQtyReaySemi.KeyDown += TxtQtyReaySemi_KeyDown;
            this.dgvSemi.CellClick += DgvSemi_CellClick;

        }
                
        //WireTerminal
        private void RdbBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RdbBox.Checked == false)
            {
                CountType2 = "Weight";
            }
            else
            {
                CountType2 = "Box";
            }
            ShowWeightOrBox();
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
        private void TxtItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtItems.Text.Trim() != "")
                {
                    ClearAllText();
                    CodeSelected = txtItems.Text;
                    ScanOrSelectItem();
                    if (LbCodeWireTerminal.Text.Trim() == "" && LbItemNameWireTerminal.Text.Trim() == "")
                    {
                        LbTypeWireTerminal.Text = "";
                        CboBobbinWWireTerminal.Items.Clear();
                        MessageBox.Show("គ្មានវត្ថុធាតុដើមនេះទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtItems.Focus();
                        txtItems.SelectAll();
                    }
                    else
                    {
                        txtItems.Text = "";
                        txtQtyWireTerminal.Focus();
                    }
                }
            }
        }
        private void CboBobbinWWireTerminal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtQtyWireTerminal.Text.Trim() != "")
            {
                string QtyDetails = txtQtyWireTerminal.Text;
                //split betwen + Symbol.
                string[] QtyArray = QtyDetails.Split('+');

                //If Count By Reil
                if (LbBobbinsWTitleWireTerminal.Text.ToString().Contains("R2") == true && LbNetWTitleWireTerminal.Text.ToString().Contains("R1") == true)
                {
                    //Calc Total Qty
                    double TotalQty = 0;
                    for (int i = 0; i < QtyArray.Length; i++)
                    {
                        if (QtyArray[i].ToString().Trim() != "")
                        {
                            double R2 = Convert.ToDouble(CboBobbinWWireTerminal.Text);
                            double R1 = NetWSelected;
                            double MOQ = MOQSelected;
                            double InputedR3 = Convert.ToDouble(QtyArray[i].ToString());
                            TotalQty = TotalQty + Math.Round(MOQ * (InputedR3 * InputedR3 - R2 * R2) / (R1 * R1 - R2 * R2), 2);
                            TotalQty = Convert.ToDouble(TotalQty.ToString("N0"));
                        }
                    }

                    LbTotalQtyWireTerminal.Text = TotalQty.ToString("N0");

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
                            double BobbinW = Convert.ToDouble(CboBobbinWWireTerminal.Text);
                            TotalQty = TotalQty + Math.Round((W - BobbinW) / NetWSelected * MOQSelected, 2);
                            TotalQty = Convert.ToDouble(TotalQty.ToString("N0"));
                        }
                    }

                    LbTotalQtyWireTerminal.Text = TotalQty.ToString("N0");

                }
            }
        }
        private void DgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                ClearAllText();
                CodeSelected = dgvItems.Rows[e.RowIndex].Cells[0].Value.ToString();
                dgvItems.Visible = false;
                ScanOrSelectItem();
                if (LbCodeWireTerminal.Text.Trim() == "" && LbItemNameWireTerminal.Text.Trim() == "")
                {
                    LbTypeWireTerminal.Text = "";
                    CboBobbinWWireTerminal.Items.Clear();
                    MessageBox.Show("គ្មានវត្ថុធាតុដើមនេះទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtItems.Focus();
                    txtItems.SelectAll();
                }
                else
                {
                    txtItems.Text = "";
                    txtQtyWireTerminal.Focus();
                }
            }
        }
        private void TxtQtyWireTerminal_TextChanged(object sender, EventArgs e)
        {
            if (txtQtyWireTerminal.Text.Trim() != "")
            {
                if (CountType2.Contains("Weight") == true)
                {
                    if (CboBobbinWWireTerminal.Text.Trim() != "")
                    {
                        if (txtQtyWireTerminal.ToString().Contains("*") == false)
                        {
                            string QtyDetails = txtQtyWireTerminal.Text;
                            //split betwen + Symbol.
                            string[] QtyArray = QtyDetails.Split('+');
                            BobbinQtyInputted = QtyArray.Length;

                            try
                            {
                                //If Count By Reil
                                if (LbBobbinsWTitleWireTerminal.Text.ToString().Contains("R2") == true && LbNetWTitleWireTerminal.Text.ToString().Contains("R1") == true)
                                {
                                    //Calc Total Qty
                                    double TotalQty = 0;
                                    for (int i = 0; i < QtyArray.Length; i++)
                                    {
                                        if (QtyArray[i].ToString().Trim() != "")
                                        {
                                            double R2 = Convert.ToDouble(CboBobbinWWireTerminal.Text);
                                            double R1 = NetWSelected;
                                            double MOQ = MOQSelected;
                                            double InputedR3 = Convert.ToDouble(QtyArray[i].ToString());
                                            TotalQty = TotalQty + Math.Round(MOQ * (InputedR3 * InputedR3 - R2 * R2) / (R1 * R1 - R2 * R2), 2);
                                            TotalQty = Convert.ToDouble(TotalQty.ToString("N0"));
                                        }
                                    }

                                    LbTotalQtyWireTerminal.Text = TotalQty.ToString("N0");

                                    //Assign Bobbin/Reel
                                    if (BobbinQtyInputted > 1)
                                    {
                                        LbBobbinQtyWireTerminal.Text = "( " + BobbinQtyInputted + " Reels )";
                                    }
                                    else
                                    {
                                        LbBobbinQtyWireTerminal.Text = "( " + BobbinQtyInputted + " Reel )";
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
                                            double BobbinW = Convert.ToDouble(CboBobbinWWireTerminal.Text);
                                            TotalQty = TotalQty + Math.Round((W - BobbinW) / NetWSelected * MOQSelected, 2);
                                            TotalQty = Convert.ToDouble(TotalQty.ToString("N0"));
                                        }
                                    }

                                    LbTotalQtyWireTerminal.Text = TotalQty.ToString("N0");

                                    //Assign Bobbin/Reel
                                    if (BobbinQtyInputted > 1)
                                    {
                                        LbBobbinQtyWireTerminal.Text = "( " + BobbinQtyInputted + " Bobbins )";
                                    }
                                    else
                                    {
                                        LbBobbinQtyWireTerminal.Text = "( " + BobbinQtyInputted + " Bobbin )";
                                    }
                                }
                            }
                            catch
                            {
                                LbTotalQtyWireTerminal.Text = "";
                                LbBobbinQtyWireTerminal.Text = "";
                                MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                        else
                        {
                            LbTotalQtyWireTerminal.Text = "";
                            LbBobbinQtyWireTerminal.Text = "";
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
                    string QtyDetails = txtQtyWireTerminal.Text;
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
                                        LbTotalQtyWireTerminal.Text = "";
                                        LbBobbinQtyWireTerminal.Text = "";
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
                            LbTotalQtyWireTerminal.Text = TotalQty.ToString("N0");

                            //Assign Bobbin/Reel
                            if (LbBobbinsWTitleWireTerminal.Text.ToString().Contains("R1") == true && LbNetWTitleWireTerminal.Text.ToString().Contains("R2") == true)
                            {
                                if (BobbinQtyInputted > 1)
                                {
                                    LbBobbinQtyWireTerminal.Text = "( " + BobbinQtyInputted + " Reels )";
                                }
                                else
                                {
                                    LbBobbinQtyWireTerminal.Text = "( " + BobbinQtyInputted + " Reel )";
                                }
                            }
                            else
                            {
                                if (BobbinQtyInputted > 1)
                                {
                                    LbBobbinQtyWireTerminal.Text = "( " + BobbinQtyInputted + " Bobbins )";
                                }
                                else
                                {
                                    LbBobbinQtyWireTerminal.Text = "( " + BobbinQtyInputted + " Bobbin )";
                                }
                            }
                        }
                    }
                    catch
                    {
                        LbTotalQtyWireTerminal.Text = "";
                        LbBobbinQtyWireTerminal.Text = "";
                        MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                LbTotalQtyWireTerminal.Text = "";
                LbBobbinQtyWireTerminal.Text = "";
            }
        }
        private void TxtQtyWireTerminal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave.PerformClick();
            }
        }
        private void TxtQtyWireTerminal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CountType2.Contains("Weight") == true)
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

        //POS-Connector
        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.Trim() != "")
                {
                    if (txtBarcode.Text.Length < 10 || (Regex.Matches(txtBarcode.Text.ToString(), "[~!@#$%^&*()_+{}:\"<>?-]").Count) > 1 || (Regex.Matches(txtBarcode.Text.ToString(), "[-]").Count) < 1 || txtBarcode.Text.ToString().Trim()[2].ToString() != "-")
                    {
                        MessageBox.Show("លេខ​ POS ខុសទម្រង់ហើយ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.Focus();
                        txtBarcode.SelectAll();
                    }
                    else
                    {
                        dgvRMListPOS.Rows.Clear();
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
                            SqlDataAdapter da = new SqlDataAdapter("SELECT QtyDetails AS POSNo FROM tbInventory WHERE LocCode='MC1' AND CancelStatus = 0 AND QtyDetails ='" + POS + "'", cnn.con);
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
                            //Take POS Details
                            try
                            {
                                cnn.con.Open(); 
                                string SQLQuery = "SELECT PosCNo, ItemName, PosCQty, PosPDelDate, " +
                                    "\nNULLIF(CONCAT(MC1Name, " +
                                    "\nCASE " +
                                    "\n\tWHEN LEN(MC2Name)>1 THEN ' & ' " +
                                    "\n\tELSE '' " +
                                    "\nEND, MC2Name, " +
                                    "\nCASE " +
                                    "\n\tWHEN LEN(MC3Name)>1 THEN ' & ' " +
                                    "\n\tELSE '' " +
                                    "\nEND, MC3Name),'') AS MCName FROM " +
                                    "\n(SELECT * FROM tbPOSDetailofMC) T1 " +
                                    "\nINNER JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Work In Process') T2 ON T1.WIPCode=T2.ItemCode " +
                                    "\nWHERE PosCNo='"+POS+"' ";
                                //Console.WriteLine(SQLQuery);
                                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                                sda.Fill(dtPOSDetails);
                            }
                            catch (Exception ex)
                            {
                                ErrorText = "POS Details : " + ex.Message;
                            }
                            cnn.con.Close();

                            //Consumption from OBS
                            if (ErrorText.Trim() == "")
                            {
                                //Assign POS Details
                                if (dtPOSDetails.Rows.Count > 0)
                                {
                                    try
                                    {
                                        cnnOBS.conOBS.Open();
                                        //Take POS Consumption
                                        string SQLQuery = "SELECT DONo, ConsumpSeqNo, T2.ItemCode, ItemName, ConsumpQty FROM " +
                                            "(SELECT * FROM prgproductionorder) T1 " +
                                            "INNER JOIN " +
                                            "(SELECT * FROM prgconsumtionorder) T2 " +
                                            "ON T1.ProductionCode=T2.ProductionCode " +
                                            "INNER JOIN " +
                                            "(SELECT * FROM mstitem WHERE DelFlag =0 AND ItemType=2 AND MatCalcFlag=0) T3 " +
                                            "ON T2.ItemCode=T3.ItemCode " +
                                            "WHERE DONo = '" + POS + "' " +
                                            "ORDER BY ConsumpSeqNo ASC";
                                        //Console.WriteLine(SQLQuery);
                                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                                        sda.Fill(dtPOSConsumpt);
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

                                    if (dtPOSConsumpt.Rows.Count > 0)
                                    {
                                        LbPOSNoPOS.Text = dtPOSDetails.Rows[0]["PosCNo"].ToString();
                                        LbItemNamePOS.Text = dtPOSDetails.Rows[0]["ItemName"].ToString();
                                        LbQtyPOS.Text = Convert.ToDouble(dtPOSDetails.Rows[0]["PosCQty"]).ToString("N0");
                                        LbShipmentDatePOS.Text = Convert.ToDateTime(dtPOSDetails.Rows[0]["PosPDelDate"]).ToString("dd-MM-yyyy");
                                        LbMCNamePOS.Text = dtPOSDetails.Rows[0]["MCName"].ToString();

                                        //Assign POS Consumption
                                        foreach (DataRow row in dtPOSConsumpt.Rows)
                                        {
                                            string Code = row[2].ToString();
                                            string RMName = row[3].ToString();
                                            double Qty = Convert.ToDouble(row[4].ToString());

                                            dgvRMListPOS.Rows.Add(Code, RMName, Qty, Qty);
                                            dgvRMListPOS.Rows[dgvRMListPOS.Rows.Count - 1].HeaderCell.Value = dgvRMListPOS.Rows.Count.ToString();
                                        }
                                        dgvRMListPOS.ClearSelection();
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
        private void DgvRMListPOS_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                e.CellStyle.ForeColor = Color.Black;
            }
        }
        private void DgvRMListPOS_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 3)
                {
                    if (dgvRMListPOS.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        try
                        {
                            double NewValue = Convert.ToDouble(dgvRMListPOS.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                            NewValue = Convert.ToDouble(NewValue.ToString("N0"));
                            if (NewValue > 0)
                            {
                                dgvRMListPOS.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = NewValue;
                            }
                            else
                            {
                                MessageBox.Show("ចំនួនត្រូវតែធំជាង ០!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                dgvRMListPOS.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ValueBeforeUpdate;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dgvRMListPOS.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ValueBeforeUpdate;
                        }
                    }
                    else
                    {
                        MessageBox.Show("មិនអាចទទេបានទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dgvRMListPOS.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ValueBeforeUpdate;
                    }
                }
            }
        }
        private void DgvRMListPOS_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 3)
                {
                    try
                    {
                        ValueBeforeUpdate = Convert.ToDouble(dgvRMListPOS.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    }
                    catch
                    {

                    }
                }
            }
        }

        //Semi BC
        private void LbWireTubeSemiBC_TextChanged(object sender, EventArgs e)
        {
            if (LbWireTubeSemiBC.Text.Trim() != "")
            {
                foreach (DataRow row in dtColor.Rows)
                {
                    if (LbWireTubeSemiBC.Text.ToString().Contains(row[0].ToString()))
                    {
                        LbWireTubeSemiBC.ForeColor = Color.FromName(row[1].ToString());
                        LbWireTubeSemiBC.BackColor = Color.FromName(row[2].ToString());
                    }
                }
            }
            else
            {
                LbWireTubeSemiBC.ForeColor = Color.Black;
                LbWireTubeSemiBC.BackColor = Color.White;
            }
        }
        private void TxtQtyReayBC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBatchQtyBC.Text.Trim() != "" && txtQtyPerBatchBC.Text.Trim() != "")
                {
                    ErrorText = "";
                    //Calc
                    try
                    {
                        double Batch = Convert.ToDouble(Convert.ToDouble(txtBatchQtyBC.Text).ToString("N0"));
                        txtBatchQtyBC.Text = Batch.ToString();
                        double QtyPerBatch = Convert.ToDouble(Convert.ToDouble(txtQtyPerBatchBC.Text).ToString("N0"));
                        txtQtyPerBatchBC.Text = QtyPerBatch.ToString();
                        double Reay = 0;
                        if (txtQtyReayBC.Text.Trim() != "")
                        {
                            Reay = Convert.ToDouble(Convert.ToDouble(txtQtyReayBC.Text).ToString("N0")); 
                            txtQtyReayBC.Text = Reay.ToString();
                        }
                        LbTotalQtySemiBC.Text = ((Batch * QtyPerBatch) + Reay).ToString("N0");
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

                    if (ErrorText.Trim() == "")
                    {
                        btnSave.PerformClick();
                    }
                    else
                    {
                        LbTotalQtySemiBC.Text = "";
                        MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtBatchQtyBC.Focus();
                    }
                }
                else
                {
                    LbTotalQtySemiBC.Text = "";
                    MessageBox.Show("សូមបញ្ចូលត្រង់ប្រអប់ដែលមានពណ៌!","Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    txtBatchQtyBC.Focus();
                }
            }
        }
        private void TxtQtyReayBC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void TxtQtyPerBatchBC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtQtyReayBC.Focus();
            }
        }
        private void TxtQtyPerBatchBC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void TxtBatchQtyBC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtQtyPerBatchBC.Focus();
            }
        }
        private void TxtBatchQtyBC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void TxtBarcodeSemi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcodeSemi.Text.Contains("/") == true)
                {
                    Cursor = Cursors.WaitCursor;
                    string[] BarcodeArray = txtBarcodeSemi.Text.ToString().Split('/');
                    string POSNo = BarcodeArray[0].ToString();
                    string WipCode = BarcodeArray[1].ToString();
                    ErrorText = "";
                    ClearAllText();

                    //Take POS Details
                    DataTable dt = new DataTable();
                    try
                    {
                        cnn.con.Open();
                        string SQLQuery = "SELECT PosCNo, WIPCode, ItemName, Remarks1, Remarks2, Remarks3, " +
                            "\nNULLIF(CONCAT(MC1Name, " +
                            "\nCASE " +
                            "\n\tWHEN LEN(MC2Name)>1 THEN ' & ' " +
                            "\n\tELSE '' " +
                            "\nEND, MC2Name, " +
                            "\nCASE " +
                            "\n\tWHEN LEN(MC3Name)>1 THEN ' & ' " +
                            "\n\tELSE '' " +
                            "\nEND, MC3Name),'') AS MCName FROM " +
                            "\n(SELECT * FROM tbPOSDetailofMC) T1 " +
                            "\nINNER JOIN (SELECT * FROM tbMasterItem WHERE ItemType = 'Work In Process') T2 ON T1.WIPCode=T2.ItemCode " +
                            "\nWHERE PosCNo = '" + POSNo + "' AND WIPCode = '" + WipCode + "' " +
                            "\nORDER BY ItemName ASC, ItemCode ASC";
                        //Console.WriteLine(SQLQuery);
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        sda.Fill(dt);
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

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        if (dt.Rows.Count > 0)
                        {
                            LbWIPCodeSemiBC.Text = WipCode;
                            LbPOSNoSemiBC.Text = dt.Rows[0]["PosCNo"].ToString();
                            string MCName = dt.Rows[0]["MCName"].ToString();
                            if (MCName.Contains("&") == true)
                            {
                                MCName = MCName.Replace("&","&&");
                            }
                            LbMCNameSemiBC.Text = MCName;
                            LbWIPNameSemiBC.Text = dt.Rows[0]["ItemName"].ToString();
                            LbPINSemiBC.Text = dt.Rows[0]["Remarks1"].ToString();
                            LbLengthSemiBC.Text = dt.Rows[0]["Remarks3"].ToString();
                            LbWireTubeSemiBC.Text = dt.Rows[0]["Remarks2"].ToString();
                            txtBarcodeSemi.Text = "";
                            txtBatchQtyBC.Focus();
                        }
                        else
                        {
                            WMsg.WarningText = "គ្មានទិន្នន័យសឺមីនេះទេ!" +
                                "\nWIP Code : " + WipCode + " " +
                                "\nPOS No    : " + POSNo;
                            WMsg.ShowingMsg();
                            txtBarcodeSemi.Focus();
                            txtBarcodeSemi.SelectAll();
                        }
                    }
                    else
                    {
                        EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                        EMsg.ShowingMsg();
                        txtBarcodeSemi.Focus();
                        txtBarcodeSemi.SelectAll();
                    }
                }
                else
                {
                    WMsg.WarningText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!";
                    WMsg.ShowingMsg();
                    txtBarcodeSemi.Focus();
                    txtBarcodeSemi.SelectAll();
                }
            }
        }
        private void TabCtrlSemi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCtrlSemi.SelectedIndex == 0)
            {
                txtBarcodeSemi.Focus();
            }
            else
            {
                txtWipNameSemi.Focus();
            }
        }

        //Semi
        private void TxtWipNameSemi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtWipNameSemi.Text.Trim() != "")
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;
                    dgvSemi.Rows.Clear();
                    DataTable dt = new DataTable();
                    try
                    {
                        cnnOBS.conOBS.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT UpItemCode, ItemName, Remark2, Remark3, Remark4 FROM " +
                            "(SELECT UpItemCode FROM mstbom WHERE DelFlag=0 GROUP BY UpItemCode)T1 " +
                            "INNER JOIN " +
                            "(SELECT * FROM mstitem WHERE ItemType=1 AND DelFlag=0)T2 " +
                            "ON T1.UpItemCode=T2.ItemCode " +
                            "WHERE ItemName LIKE '%" + txtWipNameSemi.Text + "%' " +
                            "ORDER BY ItemName ASC,UpItemCode ASC", cnnOBS.conOBS);
                        sda.Fill(dt);
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
                        foreach (DataRow row in dt.Rows)
                        {
                            dgvSemi.Rows.Add(row[0], row[1], row[2], row[3], row[4]);
                        }
                        dgvSemi.ClearSelection();
                    }
                    else
                    {
                        MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtWipNameSemi.Focus();
                    }
                }
            }
        }
        private void DgvSemi_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                if (e.Value.ToString() != "")
                {
                    foreach (DataRow row in dtColor.Rows)
                    {
                        if (e.Value.ToString().ToLower().Contains(row[0].ToString().ToLower()))
                        {
                            e.CellStyle.ForeColor = Color.FromName(row[1].ToString());
                            e.CellStyle.BackColor = Color.FromName(row[2].ToString());
                        }
                    }
                }
            }
        }
        private void TxtQtyReaySemi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBatchQtySemi.Text.Trim() != "" && txtQtyPerBatchSemi.Text.Trim() != "")
                {
                    ErrorText = "";
                    //Calc
                    try
                    {
                        double Batch = Convert.ToDouble(Convert.ToDouble(txtBatchQtySemi.Text).ToString("N0"));
                        txtBatchQtySemi.Text = Batch.ToString();
                        double QtyPerBatch = Convert.ToDouble(Convert.ToDouble(txtQtyPerBatchSemi.Text).ToString("N0"));
                        txtQtyPerBatchSemi.Text = QtyPerBatch.ToString();
                        double Reay = 0;
                        if (txtQtyReaySemi.Text.Trim() != "")
                        {
                            Reay = Convert.ToDouble(Convert.ToDouble(txtQtyReaySemi.Text).ToString("N0"));
                            txtQtyReaySemi.Text = Reay.ToString();
                        }
                        LbTotalQtySemi.Text = ((Batch * QtyPerBatch) + Reay).ToString("N0");
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

                    if (ErrorText.Trim() == "")
                    {
                        btnSave.PerformClick();
                    }
                    else
                    {
                        LbTotalQtySemi.Text = "";
                        MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtBatchQtySemi.Focus();
                    }
                }
                else
                {
                    LbTotalQtySemi.Text = "";
                    MessageBox.Show("សូមបញ្ចូលត្រង់ប្រអប់ដែលមានពណ៌!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtBatchQtySemi.Focus();
                }
            }
        }
        private void TxtQtyReaySemi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void TxtQtyPerBatchSemi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtQtyReaySemi.Focus();
            }
        }
        private void TxtQtyPerBatchSemi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void TxtBatchQtySemi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtQtyPerBatchSemi.Focus();
            }
        }
        private void TxtBatchQtySemi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void DgvSemi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSemi.SelectedCells.Count > 0)
            {
                if (e.RowIndex > -1)
                {
                    txtBatchQtySemi.Focus();
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            //POS-Connector
            if (txtCountingType.Text.Contains("ខនិកទ័រ") == true)
            {
                SavePOSConnector();
            }
            //Semi
            else if (txtCountingType.Text.Contains("សឺមី") == true) 
            {
                SaveSemi();
            }
            //Wire/Teminal
            else
            {
                SaveWireTerminal();
            }
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearAllText();
            if (CountType == "POS")
            {
                txtBarcode.Focus();
            }
            else if (CountType == "Semi")
            {
                if (tabCtrlSemi.SelectedIndex == 0)
                {
                    txtBarcodeSemi.Focus();
                }
                else
                {
                    txtWipNameSemi.Focus();
                }
            }
            else
            {
                txtItems.Focus();
            }
        }
        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (CboLoc.Text.Trim() != "")
            {
                CountTypeChanged = 0;
                //InprocessCountingTypeForm Ictf = new InprocessCountingTypeForm();
                //Ictf.ShowDialog();
                if (CountTypeChanged > 0)
                {
                    LocSelected = CboLoc.Text;
                    string CountTypeInKhmer = "";
                    if (CountType == "POS")
                    {
                        CountTypeInKhmer = "ខនិកទ័រ";
                    }
                    else if (CountType == "Semi")
                    {
                        CountTypeInKhmer = "សឺមី";
                    }
                    else
                    {
                        CountTypeInKhmer = "ខ្សែភ្លើង/ធើមីណល";
                    }
                    GrbLocAndType.Text = "ទីតាំង ៖ " + LocSelected + " ( " + CountTypeInKhmer + " )";
                    //ShowGrbLocAndType();
                }
            }
            else
            {
                MessageBox.Show("សូមជ្រើសរើស < ទីតាំង > ជាមុនសិន!","Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }
        private void InprocessCountingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.MCInproPrintStatus = chkPrintStatus.Checked;
            Properties.Settings.Default.Save();
        }
        private void InprocessCountingForm_Load(object sender, EventArgs e)
        {
            chkPrintStatus.Checked = Properties.Settings.Default.MCInproPrintStatus;
            txtCountingType.Text = "រាប់ខនិកទ័រ (ស្កេន)";
            tabCtrlSemi.TabPages.RemoveAt(1);
            //string[] MCName = new string[] { "", "MS01", "DCAM", "DE SK", "JAM", "TWIST", "SEMI", "SEMI MQC" };
            //for (int i = 0; i < MCName.Length; i++)
            //{
            //    //CboLoc.Items.Add(MCName[i].ToString());
            //}

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

            //Set POS-Connector columns to Read-Only 
            foreach (DataGridViewColumn DgvCol in dgvRMListPOS.Columns)
            {
                if (DgvCol.Index != 3)
                {
                    DgvCol.ReadOnly = true;
                }
            }

            //WireTerminal
            ErrorText = "";
            dgvItems.BringToFront();
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
        private void TxtCountingType_TextChanged(object sender, EventArgs e)
        {
            string CountTypeInKhmer = "";
            if (txtCountingType.Text.Contains("ខនិកទ័រ") == true)
            {
                CountTypeInKhmer = "ខនិកទ័រ";
                PicCurrentCountingType.BackgroundImage = imgListCountingType.Images[0];
                panelPOS.Visible = true;
                panelPOS.BringToFront();
                panelSemi.Visible = false;
                panelStockCardWireTerminal.Visible = false;
                txtBarcode.Focus();
            }
            else if (txtCountingType.Text.Contains("សឺមី") == true)
            {
                CountTypeInKhmer = "សឺមី";
                PicCurrentCountingType.BackgroundImage = imgListCountingType.Images[1]; 
                panelSemi.Visible = true;
                panelSemi.BringToFront();
                panelPOS.Visible = false;
                panelStockCardWireTerminal.Visible = false;
                txtBarcodeSemi.Focus();
            }
            else
            {
                CountTypeInKhmer = "ខ្សែភ្លើង/ធើមីណល";
                PicCurrentCountingType.BackgroundImage = imgListCountingType.Images[2];
                panelStockCardWireTerminal.Visible = true;
                panelStockCardWireTerminal.BringToFront();
                panelSemi.Visible = false;
                panelPOS.Visible = false;
                txtItems.Focus();
            }
            GrbLocAndType.Text = "ប្រភេទ ៖ " + CountTypeInKhmer ;

            /*
            if (CountType == "POS")
            {
                panelPOS.Visible = true;
                panelPOS.BringToFront();
                panelSemi.Visible = false;
                panelStockCardWireTerminal.Visible = false;
                txtBarcode.Focus();
            }
            else if (CountType == "Semi")
            {
                panelSemi.Visible = true;
                panelSemi.BringToFront();
                panelPOS.Visible = false;
                panelStockCardWireTerminal.Visible = false;
                txtBarcodeSemi.Focus();
            }
            else
            {
                panelStockCardWireTerminal.Visible = true;
                panelStockCardWireTerminal.BringToFront();
                panelSemi.Visible = false;
                panelPOS.Visible = false;
                txtItems.Focus();
            }
            */
        }
        private void TxtCountingType_Click(object sender, EventArgs e)
        {
            InprocessCountingTypeForm Ictf = new InprocessCountingTypeForm(this.txtCountingType);
            Ictf.ShowDialog();
        }
        private void PicCurrentCountingType_Click(object sender, EventArgs e)
        {
            InprocessCountingTypeForm Ictf = new InprocessCountingTypeForm(this.txtCountingType);
            Ictf.ShowDialog();
        }

        //Function
        private void ClearAllText()
        {
            //POS Connector
            LbMCNamePOS.Text = "";
            LbPOSNoPOS.Text = "";
            LbQtyPOS.Text = "";
            LbItemNamePOS.Text = "";
            LbShipmentDatePOS.Text = "";
            dgvRMListPOS.Rows.Clear();

            //SemiBC
            LbMCNameSemiBC.Text = "";
            LbPOSNoSemiBC.Text = "";
            LbWIPCodeSemiBC.Text = "";
            LbWIPNameSemiBC.Text = "";
            LbLengthSemiBC.Text = "";
            LbPINSemiBC.Text = "";
            LbWireTubeSemiBC.Text = "";
            txtBatchQtyBC.Text = "";
            txtQtyPerBatchBC.Text = "";
            txtQtyReayBC.Text = "";
            LbTotalQtySemiBC.Text = "";

            //SemiManual
            txtWipNameSemi.Text = "";
            dgvSemi.Rows.Clear();
            txtBatchQtySemi.Text = "";
            txtQtyPerBatchSemi.Text = "";
            txtQtyReaySemi.Text = "";
            LbTotalQtySemi.Text = "";


            //Stock Card WireTerminal
            LbCodeWireTerminal.Text = "";
            LbItemNameWireTerminal.Text = "";
            LbMakerWireTerminal.Text = "";
            LbTypeWireTerminal.Text = "";
            LbNetWWireTerminal.Text = "";
            txtQtyWireTerminal.Text = "";

        }
        private void ClearTextPos()
        {
            LbItemNamePOS.Text = "";
            LbPOSNoPOS.Text = "";
            LbQtyPOS.Text = "";
            LbShipmentDatePOS.Text = "";

        }
        private void SavePOSConnector()
        {
            if (LbItemNamePOS.Text.Trim() != "" && dgvRMListPOS.Rows.Count > 0)
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់រក្សាទុកមែនឬទេ?","Rachhan System",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
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
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT LabelNo AS LastLabelNo FROM tbInventory WHERE LabelNo = (SELECT MAX(LabelNo) FROM tbInventory WHERE LocCode='MC1') GROUP BY LabelNo", cnn.con);
                        sda.Fill(dtLabelNo);

                        SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM tbSDMCLocation WHERE LocCode='MC1'", cnn.con);
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
                        int LabelNo = Convert.ToInt32(dtLabelNoRange.Rows[0][2].ToString());
                        if (dtLabelNo.Rows.Count > 0)
                        {
                            LabelNo = Convert.ToInt32(dtLabelNo.Rows[0][0].ToString()) + 1;
                        }
                        string Username = MenuFormV2.UserForNextForm;
                        DateTime RegDate = DateTime.Now;

                        foreach (DataGridViewRow DgvRow in dgvRMListPOS.Rows)
                        {
                            try
                            {
                                cnn.con.Open();
                                string LocCode = "MC1";
                                string SubLoc = LbMCNamePOS.Text;
                                string CountingMethod = "POS";
                                string QtyDetails = LbPOSNoPOS.Text;
                                string ItemCode = DgvRow.Cells[0].Value.ToString();
                                string ItemType = "Material";
                                int Qty = Convert.ToInt32(DgvRow.Cells[3].Value.ToString());

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
                            string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\InventoryLable\Inprocess";
                            if (!Directory.Exists(SavePath))
                            {
                                Directory.CreateDirectory(SavePath);
                            }

                            //សរសេរចូល Excel
                            var CDirectory = Environment.CurrentDirectory;
                            Excel.Application excelApp = new Excel.Application();
                            Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\InventoryLabel_Inprocess_Template.xlsx", Editable: true);
                            Excel.Worksheet wsBarcode = (Excel.Worksheet)xlWorkBook.Sheets["POS"];
                            //ក្បាលលើ
                            wsBarcode.Cells[2, 3] = LabelNo.ToString();
                            wsBarcode.Cells[3, 1] = "*" + LabelNo.ToString() + "*";
                            wsBarcode.Cells[2, 4] = "Inprocess(" + LocSelected + ")";
                            wsBarcode.Cells[4, 3] = LbPOSNoPOS.Text;
                            wsBarcode.Cells[5, 3] = LbItemNamePOS.Text.ToString();
                            wsBarcode.Cells[6, 3] = LbQtyPOS.Text.ToString();

                            // Material List
                            for (int i = 0; i < dgvRMListPOS.Rows.Count; i++)
                            {
                                wsBarcode.Cells[i + 10, 1] = dgvRMListPOS.Rows[i].Cells[0].Value.ToString();
                                wsBarcode.Cells[i + 10, 2] = dgvRMListPOS.Rows[i].Cells[1].Value.ToString();
                                wsBarcode.Cells[i + 10, 5] = dgvRMListPOS.Rows[i].Cells[3].Value.ToString();

                            }

                            //លុប Manual worksheet
                            excelApp.DisplayAlerts = false;
                            Excel.Worksheet wsDelete = (Excel.Worksheet)xlWorkBook.Sheets["Box"];
                            wsDelete.Delete();
                            Excel.Worksheet wsDelete1 = (Excel.Worksheet)xlWorkBook.Sheets["Weight"];
                            wsDelete1.Delete();
                            Excel.Worksheet wsDelete2 = (Excel.Worksheet)xlWorkBook.Sheets["Semi"];
                            wsDelete2.Delete();
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
                        dgvRMListPOS.Rows.Clear();
                        txtBarcode.Focus();
                    }
                    else
                    {
                        MessageBox.Show("រក្សាទុកមានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("គ្មានទិន្នន័យ <បញ្ជីវត្ថុធាតុដើម> ដែលត្រូវរក្សាទុកទេ?", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBarcode.Focus();
            }
        }
        private void SaveSemi()
        {
            string Loc = "MC1";
            string SubLoc = LocSelected;
            string WipCode = "";
            string WIPName = "";
            string Pin = "";
            string WireColor = "";
            string Length = "";
            string CountingM = "Semi";
            string ItemType = CountingM;
            int Qty = 0;
            string QtyDetails = "";
            ErrorText = "";

            //Semi BC
            if (tabCtrlSemi.SelectedIndex == 0)
            {
                if (LbWIPNameSemiBC.Text.Trim() != "")
                {
                    if (txtBatchQtyBC.Text.Trim() != "" && txtQtyPerBatchBC.Text.Trim() != "")
                    {
                        //Calc
                        try
                        {
                            double Batch = Convert.ToDouble(Convert.ToDouble(txtBatchQtyBC.Text).ToString("N0"));
                            txtBatchQtyBC.Text = Batch.ToString();
                            double QtyPerBatch = Convert.ToDouble(Convert.ToDouble(txtQtyPerBatchBC.Text).ToString("N0"));
                            txtQtyPerBatchBC.Text = QtyPerBatch.ToString();
                            double Reay = 0;
                            if (txtQtyReayBC.Text.Trim() != "")
                            {
                                Reay = Convert.ToDouble(Convert.ToDouble(txtQtyReayBC.Text).ToString("N0"));
                                txtQtyReayBC.Text = Reay.ToString();
                            }
                            LbTotalQtySemiBC.Text = ((Batch * QtyPerBatch) + Reay).ToString("N0");
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

                        if (ErrorText.Trim() == "")
                        {
                            WipCode = LbWIPCodeSemiBC.Text;
                            WIPName = LbWIPNameSemiBC.Text;
                            Pin = LbPINSemiBC.Text;
                            WireColor =LbWireTubeSemiBC.Text;
                            Length = LbLengthSemiBC.Text;
                            Qty = Convert.ToInt32(LbTotalQtySemiBC.Text.Replace(",",""));
                            QtyDetails = txtBatchQtyBC.Text.ToString()+"x"+txtQtyPerBatchBC.Text.ToString();
                            if (txtQtyReayBC.Text.Trim() != "")
                            {
                                QtyDetails = QtyDetails+"+"+txtQtyReayBC.Text.ToString();
                            }
                        }
                        else
                        {
                            LbTotalQtySemiBC.Text = "";
                            if (ErrorText.Trim() == "")
                            {
                                ErrorText = "អ្នកបញ្ចូលខុសទម្រង់ហើយ!\n" + ErrorText;
                            }
                            else
                            {
                                ErrorText = ErrorText + "\nអ្នកបញ្ចូលខុសទម្រង់ហើយ!\n" + ErrorText;
                            }
                            txtBatchQtyBC.Focus();
                        }
                    }
                    else
                    {
                        LbTotalQtySemiBC.Text = "";
                        if (ErrorText.Trim() == "")
                        {
                            ErrorText = "សូមបញ្ចូលត្រង់ សម្រាយចំនួន និងចំនួនសរុប! (ប្រអប់ដែលមានពណ៌)";
                        }
                        else
                        {
                            ErrorText = ErrorText + "\nសូមបញ្ចូលត្រង់ សម្រាយចំនួន និងចំនួនសរុប! (ប្រអប់ដែលមានពណ៌)";
                        }
                        txtBatchQtyBC.Focus();
                    }
                }
                else
                {
                    if (ErrorText.Trim() == "")
                    {
                        ErrorText = "សូមស្កេនបាកូដជាមុនសិន!";
                    }
                    else
                    {
                        ErrorText = ErrorText + "\nសូមស្កេនបាកូដជាមុនសិន!";
                    }
                    txtBarcodeSemi.Focus();
                }
            }
            //Semi Manual
            else
            {
                if (dgvSemi.SelectedCells.Count>0)
                {
                    WipCode = dgvSemi.Rows[dgvSemi.CurrentCell.RowIndex].Cells[0].Value.ToString();
                    WIPName = dgvSemi.Rows[dgvSemi.CurrentCell.RowIndex].Cells[1].Value.ToString();
                    Pin = dgvSemi.Rows[dgvSemi.CurrentCell.RowIndex].Cells[2].Value.ToString();
                    WireColor = dgvSemi.Rows[dgvSemi.CurrentCell.RowIndex].Cells[3].Value.ToString();
                    Length = dgvSemi.Rows[dgvSemi.CurrentCell.RowIndex].Cells[4].Value.ToString();
                    if (txtBatchQtySemi.Text.Trim() != "" && txtQtyPerBatchSemi.Text.Trim() != "")
                    {
                        ErrorText = "";
                        //Calc
                        try
                        {
                            double Batch = Convert.ToDouble(Convert.ToDouble(txtBatchQtySemi.Text).ToString("N0"));
                            txtBatchQtySemi.Text = Batch.ToString();
                            double QtyPerBatch = Convert.ToDouble(Convert.ToDouble(txtQtyPerBatchSemi.Text).ToString("N0"));
                            txtQtyPerBatchSemi.Text = QtyPerBatch.ToString();
                            double Reay = 0;
                            if (txtQtyReaySemi.Text.Trim() != "")
                            {
                                Reay = Convert.ToDouble(Convert.ToDouble(txtQtyReaySemi.Text).ToString("N0"));
                                txtQtyReaySemi.Text = Reay.ToString();
                            }
                            LbTotalQtySemi.Text = ((Batch * QtyPerBatch) + Reay).ToString("N0");
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

                        if (ErrorText.Trim() == "")
                        {
                            Qty = Convert.ToInt32(LbTotalQtySemi.Text.Replace(",", ""));
                            QtyDetails = txtBatchQtySemi.Text.ToString() + "x" + txtQtyPerBatchSemi.Text.ToString();
                            if (txtQtyReaySemi.Text.Trim() != "")
                            {
                                QtyDetails = QtyDetails + "+" + txtQtyReaySemi.Text.ToString();
                            }
                        }
                        else
                        {
                            LbTotalQtySemi.Text = "";
                            MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txtBatchQtySemi.Focus();
                        }
                    }
                    else
                    {
                        LbTotalQtySemi.Text = "";
                        if (ErrorText.Trim() == "")
                        {
                            ErrorText = "សូមបញ្ចូលត្រង់ សម្រាយចំនួន និងចំនួនសរុប! (ប្រអប់ដែលមានពណ៌)";
                        }
                        else
                        {
                            ErrorText = ErrorText + "\nសូមបញ្ចូលត្រង់ សម្រាយចំនួន និងចំនួនសរុប! (ប្រអប់ដែលមានពណ៌)";
                        }
                        txtBatchQtySemi.Focus();
                    }
                }
                else
                {
                    if (ErrorText.Trim() == "")
                    {
                        ErrorText = "សូមស្វែងរកនិងជ្រើសរើសសឺមីជាមុនសិន!";
                    }
                    else
                    {
                        ErrorText = ErrorText + "\nសូមស្វែងរកនិងជ្រើសរើសសឺមីជាមុនសិន!";
                    }
                    txtWipNameSemi.Focus();
                }
            }

            if (ErrorText.Trim() == "")
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់រក្សាទុកមែនឬទេ?" + "( " +WIPName + " )", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT LabelNo AS LastLabelNo FROM tbInventory WHERE LabelNo = (SELECT MAX(LabelNo) FROM tbInventory WHERE LocCode='MC1') GROUP BY LabelNo", cnn.con);
                        sda.Fill(dtLabelNo);

                        SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM tbSDMCLocation WHERE LocCode='MC1'", cnn.con);
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
                        int LabelNo = Convert.ToInt32(dtLabelNoRange.Rows[0][2].ToString());
                        if (dtLabelNo.Rows.Count > 0)
                        {
                            LabelNo = Convert.ToInt32(dtLabelNo.Rows[0][0].ToString()) + 1;
                        }
                        string Username = MenuFormV2.UserForNextForm;
                        DateTime RegDate = DateTime.Now;

                        try
                        {
                            cnn.con.Open();
                            
                            if (LabelNo <= Convert.ToInt32(dtLabelNoRange.Rows[0][3].ToString()))
                            {
                                cmd = new SqlCommand("INSERT INTO tbInventory(LocCode, SubLoc, LabelNo, CountingMethod, SeqNo, ItemCode, ItemType, Qty, QtyDetails, RegDate, RegBy, UpdateDate, UpdateBy, CancelStatus) " +
                                "VALUES (@Lc, @Slc, @lbn, @Cm, @Sn, @Ic, @It, @Qty, @QtyD, @RegD, @RegB, @UpD, @UpB, @Cs)", cnn.con);

                                cmd.Parameters.AddWithValue("@Lc", Loc);
                                cmd.Parameters.AddWithValue("@Slc", SubLoc);
                                cmd.Parameters.AddWithValue("@lbn", LabelNo);
                                cmd.Parameters.AddWithValue("@Cm", CountingM);
                                cmd.Parameters.AddWithValue("@Sn", 1);
                                cmd.Parameters.AddWithValue("@Ic", WipCode);
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

                        //Print
                        if (chkPrintStatus.Checked == true)
                        {
                            if (ErrorText.Trim() == "")
                            {
                                try
                                {
                                    //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                                    string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\InventoryLable\Inprocess";
                                    if (!Directory.Exists(SavePath))
                                    {
                                        Directory.CreateDirectory(SavePath);
                                    }

                                    //open excel application and create new workbook
                                    var CDirectory = Environment.CurrentDirectory;
                                    Excel.Application excelApp = new Excel.Application();
                                    Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\InventoryLabel_Inprocess_Template.xlsx", Editable: true);
                                    Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Semi"];

                                    worksheet.Cells[2, 6] = "Inprocess(" + SubLoc + ")";
                                    worksheet.Cells[2, 4] = LabelNo;
                                    worksheet.Cells[3, 1] = "*" + LabelNo + "*";
                                    worksheet.Cells[7, 4] = WipCode;
                                    worksheet.Cells[9, 4] = WIPName;
                                    worksheet.Cells[11, 4] = Pin;
                                    worksheet.Cells[13, 4] = WireColor;
                                    worksheet.Cells[15, 4] = Length;

                                    worksheet.Cells[17, 4] = Qty;
                                    //Summary
                                    worksheet.Cells[17, 6] = "( " + QtyDetails + " )";

                                    // Saving the modified Excel file
                                    var CDirectory2 = Environment.CurrentDirectory;
                                    string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                    string file = LabelNo.ToString();
                                    string fName = file + "( " + date + " )";

                                    //លុប Manual worksheet
                                    excelApp.DisplayAlerts = false;
                                    Excel.Worksheet wsDelete1 = (Excel.Worksheet)xlWorkBook.Sheets["Weight"];
                                    wsDelete1.Delete();
                                    Excel.Worksheet wsDelete2 = (Excel.Worksheet)xlWorkBook.Sheets["POS"];
                                    wsDelete2.Delete();
                                    Excel.Worksheet wsDelete3 = (Excel.Worksheet)xlWorkBook.Sheets["Box"];
                                    wsDelete3.Delete();
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
                        MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnNew.PerformClick();
                    }
                    else
                    {
                        MessageBox.Show("រក្សាទុកមានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show(ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void SaveWireTerminal()
        {
            if (LbCodeWireTerminal.Text.Trim() != "" && LbItemNameWireTerminal.Text.Trim() != "" && LbTotalQtyWireTerminal.Text.Trim() != "" && LbBobbinQtyWireTerminal.Text.Trim() != "")
            {
                //Check QtyDetail first
                if (CountType2.Contains("Weight") == true)
                {
                    //Only 0-9 & '.' & '+'
                    int FoundError = 0;
                    string[] AllowText = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ".", "+" };
                    for (int i = 0; i < txtQtyWireTerminal.Text.ToString().Length; i++)
                    {
                        int OK = 0;
                        for (int j = 0; j < AllowText.Length; j++)
                        {
                            if (txtQtyWireTerminal.Text[i].ToString() == AllowText[j].ToString())
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
                    string[] QtyArray = txtQtyWireTerminal.Text.ToString().Split('+');
                    for (int i = 0; i < QtyArray.Length; i++)
                    {
                        if (Convert.ToDouble(QtyArray[i].ToString()) < Convert.ToDouble(CboBobbinWWireTerminal.Text))
                        {
                            FoundError++;
                            MessageBox.Show("ក្នុង <ទម្ងន់សរុប> ខ្លះមានទម្ងន់តិចជាង <ទម្ងន់ប៊ូប៊ីន> !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                        }
                    }

                    //Sum/CalcByComputer
                    try
                    {
                        string value = new DataTable().Compute(txtQtyWireTerminal.Text.ToString(), null).ToString();
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
                                SqlDataAdapter sda = new SqlDataAdapter("SELECT LabelNo AS LastLabelNo FROM tbInventory WHERE LabelNo = (SELECT MAX(LabelNo) FROM tbInventory WHERE LocCode='MC1') GROUP BY LabelNo", cnn.con);
                                dtLabelNo = new DataTable();
                                sda.Fill(dtLabelNo);

                                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM tbSDMCLocation WHERE LocCode='MC1'", cnn.con);
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
                                string LocCode = "MC1";
                                string SubLoc = LocSelected;
                                int LabelNo = Convert.ToInt32(dtLabelNoRange.Rows[0][2].ToString());
                                if (dtLabelNo.Rows.Count > 0)
                                {
                                    LabelNo = Convert.ToInt32(dtLabelNo.Rows[0][0].ToString()) + 1;
                                }
                                string CountingMethod = "StockCard";
                                string ItemCode = LbCodeWireTerminal.Text;
                                string CountingMethod2 = "Box";
                                if (CountType2.Contains("Weight") == true)
                                {
                                    CountingMethod2 = "Weight";
                                }
                                string ItemType = "Material";
                                int Qty = Convert.ToInt32(LbTotalQtyWireTerminal.Text.Replace(",", ""));
                                string QtyDetails = txtQtyWireTerminal.Text.Replace("*", "x");
                                string QtyDetails2 = CboBobbinWWireTerminal.Text + "|" + BobbinQtyInputted;

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
                                            string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\InventoryLable\Inprocess";
                                            if (!Directory.Exists(SavePath))
                                            {
                                                Directory.CreateDirectory(SavePath);
                                            }

                                            //open excel application and create new workbook
                                            var CDirectory = Environment.CurrentDirectory;
                                            Excel.Application excelApp = new Excel.Application();
                                            Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\InventoryLabel_Inprocess_Template.xlsx", Editable: true);
                                            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Weight"];

                                            worksheet.Cells[2, 6] = "Inprocess("+ SubLoc + ")";
                                            worksheet.Cells[2, 4] = LabelNo;
                                            worksheet.Cells[3, 1] = "*" + LabelNo + "*";
                                            worksheet.Cells[7, 4] = ItemCode;
                                            worksheet.Cells[9, 4] = LbItemNameWireTerminal.Text;
                                            worksheet.Cells[11, 4] = LbTypeWireTerminal.Text;
                                            worksheet.Cells[13, 4] = LbMakerWireTerminal.Text;
                                            worksheet.Cells[15, 4] = Qty;
                                            //Summary
                                            worksheet.Cells[15, 6] = "( " + QtyDetails + " )\n" + LbBobbinQtyWireTerminal.Text.Replace("( ", "").Replace(" )", "");

                                            // Saving the modified Excel file
                                            var CDirectory2 = Environment.CurrentDirectory;
                                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                            string file = LabelNo.ToString();
                                            string fName = file + "( " + date + " )";

                                            //លុប Manual worksheet
                                            excelApp.DisplayAlerts = false; 
                                            Excel.Worksheet wsDelete1 = (Excel.Worksheet)xlWorkBook.Sheets["Box"];
                                            wsDelete1.Delete();
                                            Excel.Worksheet wsDelete2 = (Excel.Worksheet)xlWorkBook.Sheets["POS"];
                                            wsDelete2.Delete();
                                            Excel.Worksheet wsDelete3 = (Excel.Worksheet)xlWorkBook.Sheets["Semi"];
                                            wsDelete3.Delete();
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
                        txtQtyWireTerminal.Focus();
                    }
                }
                else
                {
                    //Only 0-9 & '*' & '+'
                    int FoundError = 0;
                    string[] AllowText = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "*", "+" };
                    for (int i = 0; i < txtQtyWireTerminal.Text.ToString().Length; i++)
                    {
                        int OK = 0;
                        for (int j = 0; j < AllowText.Length; j++)
                        {
                            if (txtQtyWireTerminal.Text[i].ToString() == AllowText[j].ToString())
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
                        string value = new DataTable().Compute(txtQtyWireTerminal.Text.ToString(), null).ToString();
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
                                SqlDataAdapter sda = new SqlDataAdapter("SELECT LabelNo AS LastLabelNo FROM tbInventory WHERE LabelNo = (SELECT MAX(LabelNo) FROM tbInventory WHERE LocCode='MC1') GROUP BY LabelNo", cnn.con);
                                dtLabelNo = new DataTable();
                                sda.Fill(dtLabelNo);

                                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM tbSDMCLocation WHERE LocCode='MC1'", cnn.con);
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
                                string LocCode = "MC1";
                                string SubLoc = LocSelected;
                                int LabelNo = Convert.ToInt32(dtLabelNoRange.Rows[0][2].ToString());
                                if (dtLabelNo.Rows.Count > 0)
                                {
                                    LabelNo = Convert.ToInt32(dtLabelNo.Rows[0][0].ToString()) + 1;
                                }
                                string CountingMethod = "StockCard";
                                string ItemCode = LbCodeWireTerminal.Text;
                                string CountingMethod2 = "Box";
                                if (CountType2.Contains("Weight") == true)
                                {
                                    CountingMethod2 = "Weight";
                                }
                                string ItemType = "Material";
                                int Qty = Convert.ToInt32(LbTotalQtyWireTerminal.Text.Replace(",", ""));
                                string QtyDetails = txtQtyWireTerminal.Text.Replace("*", "x");
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
                                            string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\InventoryLable\Inprocess";
                                            if (!Directory.Exists(SavePath))
                                            {
                                                Directory.CreateDirectory(SavePath);
                                            }

                                            //open excel application and create new workbook
                                            var CDirectory = Environment.CurrentDirectory;
                                            Excel.Application excelApp = new Excel.Application();
                                            Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\InventoryLabel_Inprocess_Template.xlsx", Editable: true);
                                            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Box"];

                                            worksheet.Cells[2, 6] = "Inprocess(" + SubLoc + ")";
                                            worksheet.Cells[2, 4] = LabelNo;
                                            worksheet.Cells[3, 1] = "*" + LabelNo + "*";
                                            worksheet.Cells[7, 4] = ItemCode;
                                            worksheet.Cells[9, 4] = LbItemNameWireTerminal.Text;
                                            worksheet.Cells[11, 4] = LbTypeWireTerminal.Text;
                                            worksheet.Cells[13, 4] = LbMakerWireTerminal.Text;
                                            worksheet.Cells[15, 4] = Qty;
                                            //Summary
                                            worksheet.Cells[15, 6] = "( " + QtyDetails + " )\n" + LbBobbinQtyWireTerminal.Text.Replace("( ", "").Replace(" )", "");

                                            // Saving the modified Excel file
                                            var CDirectory2 = Environment.CurrentDirectory;
                                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                                            string file = LabelNo.ToString();
                                            string fName = file + "( " + date + " )";

                                            //លុប Manual worksheet
                                            excelApp.DisplayAlerts = false;
                                            Excel.Worksheet wsDelete1 = (Excel.Worksheet)xlWorkBook.Sheets["Weight"];
                                            wsDelete1.Delete();
                                            Excel.Worksheet wsDelete2 = (Excel.Worksheet)xlWorkBook.Sheets["POS"];
                                            wsDelete2.Delete();
                                            Excel.Worksheet wsDelete3 = (Excel.Worksheet)xlWorkBook.Sheets["Semi"];
                                            wsDelete3.Delete();
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
                        txtQtyWireTerminal.Focus();
                    }
                }
            }
        }

        //WireTerminal Function
        private void ShowWeightOrBox()
        {            
            if (CountType2 == "Weight")
            {
                groupBox3.Text = "ព៌ត័មានរបស់វត្ថុធាតុដើម ( Count by " + CountType2 + " )";
                btnNew.Visible = true;
                btnSave.Visible = true;
                chkPrintStatus.Visible = true;
                numPrintQty.Visible = true;
                panelBody.Visible = true;
                //Enable CboBobbinW
                if (BobbinOrReilSelected == "Bobbins")
                {
                    CboBobbinWWireTerminal.Enabled = true;
                }
            }
            else
            {
                groupBox3.Text = "ព៌ត័មានរបស់វត្ថុធាតុដើម ( Count by " + CountType2 + " )";
                btnNew.Visible = true;
                btnSave.Visible = true;
                chkPrintStatus.Visible = true;
                numPrintQty.Visible = true;
                panelBody.Visible = true;
                //Enable CboBobbinW
                CboBobbinWWireTerminal.Enabled = false;
            }            
        }
        private void TakeTerminalOrWireBobbinsW()
        {
            CboBobbinWWireTerminal.Items.Clear();
            if (LbTypeWireTerminal.Text == "Wire")
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
                            CboBobbinWWireTerminal.Items.Add(row[1].ToString());
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
                            CboBobbinWWireTerminal.Items.Add(row[1].ToString());
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
        private void ScanOrSelectItem()
        {
            if (CountType2.Contains("Weight") == true)
            {
                foreach (DataRow row in dtRM.Rows)
                {
                    if (CodeSelected == row[0].ToString())
                    {
                        LbCodeWireTerminal.Text = row[0].ToString();
                        LbItemNameWireTerminal.Text = row[1].ToString();
                        LbMakerWireTerminal.Text = row[2].ToString();
                        if (BobbinOrReilSelected != row[4].ToString())
                        {
                            BobbinOrReilSelected = row[4].ToString();
                        }
                        if (BobbinOrReilSelected == "Bobbin")
                        {
                            LbBobbinsWTitleWireTerminal.Text = "ទម្ងន់ប៊ូប៊ីន (KG)";
                            LbNetWTitleWireTerminal.Text = "ទម្ងន់សាច់សុទ្ធ (KG)";
                            LbNetWTitleWireTerminal.Font = new Font("Khmer OS Battambang", 14);
                            LbQtyTitleWireTerminal.Text = "ទម្ងន់សរុប";
                            if (LastTypeSelected != row[3].ToString())
                            {
                                LastTypeSelected = row[3].ToString();
                                LbTypeWireTerminal.Text = LastTypeSelected;
                                TakeTerminalOrWireBobbinsW();
                                CboBobbinWWireTerminal.Text = row[5].ToString();
                            }
                            else
                            {
                                LbTypeWireTerminal.Text = LastTypeSelected;
                                if (CboBobbinWWireTerminal.Text.Trim() == "")
                                {
                                    TakeTerminalOrWireBobbinsW();
                                    CboBobbinWWireTerminal.Text = row[5].ToString();
                                }
                            }
                            CboBobbinWWireTerminal.Enabled = true;
                        }
                        else
                        {
                            LbTypeWireTerminal.Text = row[3].ToString();
                            CboBobbinWWireTerminal.Items.Clear();
                            CboBobbinWWireTerminal.Items.Add(row[5].ToString());
                            CboBobbinWWireTerminal.Text = row[5].ToString();
                            LbBobbinsWTitleWireTerminal.Text = "ប្រវែងគ្មានធើមីណល R2 (mm)";
                            LbBobbinsWTitleWireTerminal.Font = new Font("Khmer OS Battambang", 11, FontStyle.Regular);
                            LbNetWTitleWireTerminal.Text =  "ប្រវែងពេញ R1 (mm)";
                            LbQtyTitleWireTerminal.Text = "ប្រវែងសរុប";
                            CboBobbinWWireTerminal.Enabled = false;
                        }
                        NetWSelected = Convert.ToDouble(row[6].ToString());
                        MOQSelected = Convert.ToDouble(row[7].ToString());
                        LbNetWWireTerminal.Text = NetWSelected.ToString("N2") + " = " + MOQSelected.ToString("N0");
                    }
                }
            }
            else
            {
                foreach (DataRow row in dtRM.Rows)
                {
                    if (CodeSelected == row[0].ToString())
                    {
                        LbCodeWireTerminal.Text = row[0].ToString();
                        LbItemNameWireTerminal.Text = row[1].ToString();
                        LbMakerWireTerminal.Text = row[2].ToString();
                        if (BobbinOrReilSelected != row[4].ToString())
                        {
                            BobbinOrReilSelected = row[4].ToString();
                        }
                        if (BobbinOrReilSelected == "Bobbin")
                        {
                            LbBobbinsWTitleWireTerminal.Text = "ទម្ងន់ប៊ូប៊ីន (KG)";
                            LbNetWTitleWireTerminal.Text = "ទម្ងន់សាច់សុទ្ធ (KG)";
                            LbNetWTitleWireTerminal.Font = new Font("Khmer OS Battambang", 14);
                            LbQtyTitleWireTerminal.Text = "សម្រាយចំនួន";
                            if (LastTypeSelected != row[3].ToString())
                            {
                                LastTypeSelected = row[3].ToString();
                                LbTypeWireTerminal.Text = LastTypeSelected;
                                TakeTerminalOrWireBobbinsW();
                                CboBobbinWWireTerminal.Text = row[5].ToString();
                            }
                            else
                            {
                                LbTypeWireTerminal.Text = LastTypeSelected;
                            }
                        }
                        else
                        {
                            LbTypeWireTerminal.Text = row[3].ToString();
                            CboBobbinWWireTerminal.Items.Clear();
                            CboBobbinWWireTerminal.Items.Add(row[5].ToString());
                            CboBobbinWWireTerminal.Text = row[5].ToString();
                            LbBobbinsWTitleWireTerminal.Text = "ប្រវែងគ្មានធើមីណល R2 (mm)";
                            LbBobbinsWTitleWireTerminal.Font = new Font("Khmer OS Battambang", 11, FontStyle.Regular);
                            LbNetWTitleWireTerminal.Text = "ប្រវែងពេញ R1 (mm)";
                            LbQtyTitleWireTerminal.Text = "សម្រាយចំនួន";
                        }
                        NetWSelected = Convert.ToDouble(row[6].ToString());
                        MOQSelected = Convert.ToDouble(row[7].ToString());
                        LbNetWWireTerminal.Text = NetWSelected.ToString("N2") + " = " + MOQSelected.ToString("N0");
                    }
                }
            }
        }

    }
}
