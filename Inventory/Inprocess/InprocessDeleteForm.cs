using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.Inventory.Inprocess
{
    public partial class InprocessDeleteForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SqlCommand cmd;
        DataTable dtInventory;
        DataTable dtColor;
        string SubLoc;
        string CountMethod;
        string CountMethod2;
        string ItemCode;
        string QtyDetails;
        string QtyDetails2;


        //WireTerminal
        double NetWSelected;
        double BobbinQtyInputted;
        double MOQSelected;

        string ErrorText;

        public InprocessDeleteForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += InprocessDeleteForm_Load;
            this.btnNew.Click += BtnNew_Click;
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;
            this.btnDelete.Click += BtnDelete_Click;


            //Semi
            this.LbWireTubeSemi.TextChanged += LbWireTubeSemi_TextChanged;

            //WireTerminal
            this.txtQtyWireTerminal.TextChanged += TxtQtyWireTerminal_TextChanged;

        }

        
        //Semi
        private void LbWireTubeSemi_TextChanged(object sender, EventArgs e)
        {
            if (LbWireTubeSemi.Text.Trim() != "")
            {
                foreach (DataRow row in dtColor.Rows)
                {
                    if (LbWireTubeSemi.Text.ToString().Contains(row[0].ToString()))
                    {
                        LbWireTubeSemi.ForeColor = Color.FromName(row[1].ToString());
                        LbWireTubeSemi.BackColor = Color.FromName(row[2].ToString());
                    }
                }
            }
            else
            {
                LbWireTubeSemi.ForeColor = Color.Black;
                LbWireTubeSemi.BackColor = Color.White;
            }
        }

        //WireTerminal
        private void TxtQtyWireTerminal_TextChanged(object sender, EventArgs e)
        {
            if (txtQtyWireTerminal.Text.Trim() != "")
            {
                if (RdbWeight.Checked == true)
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

        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.Trim() != "")
                {
                    Cursor = Cursors.WaitCursor;
                    ErrorText = "";
                    dtInventory = new DataTable();
                    try
                    {
                        cnn.con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT SubLoc, CountingMethod, CountingMethod2, ItemType, ItemCode, Qty, QtyDetails, QtyDetails2 FROM tbInventory " +
                            " WHERE LocCode='MC1' AND CancelStatus=0 AND LabelNo=" + Convert.ToInt32(txtBarcode.Text) + " ORDER BY SeqNo ASC", cnn.con);
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
                            ErrorText = ErrorText + "\n" + ex.Message;
                        }
                    }
                    cnn.con.Close();

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        SubLoc = "";
                        CountMethod = "";
                        CountMethod2 = "";
                        ItemCode = "";
                        QtyDetails = "";
                        QtyDetails2 = "";
                        ClearAllText();

                        if (dtInventory.Rows.Count > 0)
                        {
                            SubLoc = dtInventory.Rows[0]["SubLoc"].ToString();
                            CountMethod = dtInventory.Rows[0]["CountingMethod"].ToString();
                            CountMethod2 = dtInventory.Rows[0]["CountingMethod2"].ToString();
                            ItemCode = dtInventory.Rows[0]["ItemCode"].ToString();
                            QtyDetails = dtInventory.Rows[0]["QtyDetails"].ToString();
                            QtyDetails2 = dtInventory.Rows[0]["QtyDetails2"].ToString();

                            string CountTypeInKhmer = "";
                            if (CountMethod == "POS")
                            {
                                CountTypeInKhmer = "ខនិកទ័រ";
                            }
                            else if (CountMethod == "Semi")
                            {
                                CountTypeInKhmer = "សឺមី";
                            }
                            else
                            {
                                CountTypeInKhmer = "ខ្សែភ្លើង/ធើមីណល";
                            }
                            LbCountMethod.Text = " ( " + CountTypeInKhmer + " )";
                            CboSubLoc.Text = SubLoc;
                            LbLabelNo.Text = txtBarcode.Text;
                            txtBarcode.Text = "";
                            ShowingAfterScan();
                        }
                        else
                        {
                            MessageBox.Show("គ្មានទិន្នន័យឡាប៊ែលនេះទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txtBarcode.Focus();
                            txtBarcode.SelectAll();
                        }
                    }
                    else
                    {
                        MessageBox.Show(ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearAllText();
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }
        private void InprocessDeleteForm_Load(object sender, EventArgs e)
        {
            string[] MCName = new string[] { "MS01", "DCAM", "DE SK", "JAM", "TWIST", "SEMI", "SEMI MQC" };
            for (int i = 0; i < MCName.Length; i++)
            {
                CboSubLoc.Items.Add(MCName[i].ToString());
            }

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

            txtBarcode.Focus();
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (LbLabelNo.Visible == true && LbLabelNo.Text.Trim() != "")
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;
                    try
                    {
                        cnn.con.Open();
                        DateTime UpdateDate = DateTime.Now;
                        string Username = MenuFormV2.UserForNextForm;
                        string query = "UPDATE tbInventory SET CancelStatus=1, " +
                                                        "UpdateDate='" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                        "UpdateBy=N'" + Username + "' " +
                                                        "WHERE LocCode='MC1' AND LabelNo=" + Convert.ToInt32(LbLabelNo.Text);
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

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        MessageBox.Show("លុបរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearAllText();
                        txtBarcode.Focus();
                    }
                    else
                    {
                        MessageBox.Show("មានបញ្ហា!\n"+ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                MessageBox.Show("សូមស្កេនបាកូដជាមុនសិន!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBarcode.Focus();
            }
        }

        //Function
        private void ClearAllText()
        {
            LbLabelNo.Text = "";
            LbLabelNoTitle.Visible = false;
            CboSubLoc.Text = "";
            CboSubLoc.Visible = false;
            CboSubLocTitle.Visible = false;
            LbCountMethod.Text = "";
            panelSemi.Visible = false;
            panelPOS.Visible = false;
            panelStockCardWireTerminal.Visible = false;

            //Semi
            LbWIPCodeSemi.Text = "";
            LbWIPNameSemi.Text = "";
            LbLengthSemi.Text = "";
            LbPINSemi.Text = "";
            LbWireTubeSemi.Text = "";
            txtBatchQtySemi.Text = "";
            txtQtyPerBatchSemi.Text = "";
            txtQtyReaySemi.Text = "";
            LbTotalQtySemi.Text = "";

            //POS-Connector
            LbPOSNoPOS.Text = "";
            LbQtyPOS.Text = "";
            LbItemNamePOS.Text = "";
            LbShipmentDatePOS.Text = "";
            dgvRMListPOS.Rows.Clear();

            //WireTerminal
            LbCodeWireTerminal.Text = "";
            LbItemNameWireTerminal.Text = "";
            LbMakerWireTerminal.Text = "";
            LbTypeWireTerminal.Text = "";
            CboBobbinWWireTerminal.Text = "";
            LbNetWWireTerminal.Text = "";
            txtQtyWireTerminal.Text = "";

        }
        private void ShowingAfterScan()
        {
            LbLabelNo.Visible = true;
            LbLabelNoTitle.Visible = true;
            CboSubLocTitle.Visible = true;
            CboSubLoc.Visible = true;
            LbCountMethod.Visible = true;

            //POS-Connector
            if (CountMethod == "POS")
            {
                ErrorText = "";
                DataTable dt = new DataTable();
                DataTable dtRMOBS = new DataTable();
                //Find POS Detail from OBS
                try
                {
                    //POS Details
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT T1.ItemCode, ItemName, PlanQty, POSDeliveryDate FROM " +
                        "(SELECT * FROM prgproductionorder) T1 " +
                        "INNER JOIN " +
                        "(SELECT * FROM mstitem WHERE ItemType = 1 AND DelFlag=0) T2 " +
                        "ON T1.ItemCode=T2.ItemCode " +
                        "WHERE DONo='" + QtyDetails + "'", cnnOBS.conOBS);
                    sda.Fill(dt);

                    //Take RM 
                    string CodeIN = "";
                    foreach (DataRow row in dtInventory.Rows)
                    {
                        if (CodeIN.Trim() == "")
                        {
                            CodeIN = " '" + row["ItemCode"].ToString() + "' ";
                        }
                        else
                        {
                            CodeIN = CodeIN + ", '" + row["ItemCode"].ToString() + "' ";
                        }
                    }
                    SqlDataAdapter sda1 = new SqlDataAdapter("SELECT ItemCode, ItemName FROM mstitem WHERE ItemType = 2 AND DelFlag = 0 AND ItemCode IN (" + CodeIN + ")", cnnOBS.conOBS);
                    sda1.Fill(dtRMOBS);
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
                    //Show POS Details
                    if (dt.Rows.Count > 0)
                    {
                        LbPOSNoPOS.Text = QtyDetails;
                        LbQtyPOS.Text = Convert.ToDouble(dt.Rows[0]["PlanQty"].ToString()).ToString("N0");
                        LbItemNamePOS.Text = dt.Rows[0]["ItemName"].ToString();
                        LbShipmentDatePOS.Text = Convert.ToDateTime(dt.Rows[0]["POSDeliveryDate"].ToString()).ToString("dd-MM-yyyy");
                    }
                    //Showing RM List
                    foreach (DataRow row in dtInventory.Rows)
                    {
                        string Code = row["ItemCode"].ToString();
                        string Name = "";
                        double Qty = Convert.ToDouble(row["Qty"].ToString());
                        foreach (DataRow rowOBS in dtRMOBS.Rows)
                        {
                            if (Code == rowOBS["ItemCode"].ToString())
                            {
                                Name = rowOBS["ItemName"].ToString();
                                break;
                            }
                        }
                        dgvRMListPOS.Rows.Add(Code, Name, Qty);
                    }
                    dgvRMListPOS.ClearSelection();
                    panelPOS.Visible = true;
                    panelPOS.BringToFront();
                }
                else
                {
                    MessageBox.Show(ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //WireTerminal
            else if (CountMethod == "StockCard")
            {
                ErrorText = "";
                DataTable dtRMOBS = new DataTable();
                //Find RM from OBS
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, Resv1 FROM mstitem WHERE ItemType = 2 AND DelFlag = 0 AND ItemCode = '" + ItemCode + "'", cnnOBS.conOBS);
                    sda.Fill(dtRMOBS);
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
                    DataTable dt = new DataTable();
                    try
                    {
                        cnn.con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbSDMstUncountMat WHERE Code='" + ItemCode + "'", cnn.con);
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

                    if (ErrorText.Trim() == "")
                    {
                        //Show info
                        string ItemName = "";
                        string Maker = "";
                        if (dtRMOBS.Rows.Count > 0)
                        {
                            ItemName = dtRMOBS.Rows[0]["ItemName"].ToString();
                            Maker = dtRMOBS.Rows[0]["Resv1"].ToString();
                        }
                        LbCodeWireTerminal.Text = ItemCode;
                        LbItemNameWireTerminal.Text = ItemName;
                        LbMakerWireTerminal.Text = Maker;
                        LbTypeWireTerminal.Text = dt.Rows[0]["RMType"].ToString();
                        CboBobbinWWireTerminal.Items.Clear();
                        //Box Or Weigth
                        if (CountMethod2 == "Weight")
                        {
                            RdbWeight.Checked = true;
                            string[] QtyD2 = QtyDetails2.Split('|');
                            CboBobbinWWireTerminal.Items.Add(QtyD2[0].ToString());
                            CboBobbinWWireTerminal.SelectedIndex = 0;

                            if (dt.Rows[0]["BobbinsOrReil"].ToString() == "Bobbins")
                            {
                                LbBobbinsWTitleWireTerminal.Text = "ទម្ងន់ប៊ូប៊ីន (KG)";
                                LbNetWTitleWireTerminal.Text = "ទម្ងន់សាច់សុទ្ធ (KG)";
                                LbNetWTitleWireTerminal.Font = new Font("Khmer OS Battambang", 14);
                                LbQtyTitleWireTerminal.Text = "ទម្ងន់សរុប";

                            }
                            else
                            {
                                LbBobbinsWTitleWireTerminal.Text = "ប្រវែងគ្មានធើមីណល R2 (cm)";
                                LbBobbinsWTitleWireTerminal.Font = new Font("Khmer OS Battambang", 11, FontStyle.Regular);
                                LbNetWTitleWireTerminal.Text = "ប្រវែងពេញ R1 (cm)";
                                LbQtyTitleWireTerminal.Text = "ប្រវែងសរុប";
                            }
                            NetWSelected = Convert.ToDouble(dt.Rows[0]["R1OrNetW"].ToString());
                            MOQSelected = Convert.ToDouble(dt.Rows[0]["MOQ"].ToString());
                            LbNetWWireTerminal.Text = NetWSelected.ToString("N2") + " = " + MOQSelected.ToString("N0");
                        }
                        else
                        {
                            RdbBox.Checked = true;
                            CboBobbinWWireTerminal.Items.Clear();
                            CboBobbinWWireTerminal.Items.Add(dt.Rows[0]["R2OrBobbinsW"].ToString());
                            CboBobbinWWireTerminal.Text = dt.Rows[0]["R2OrBobbinsW"].ToString();
                            if (dt.Rows[0]["BobbinsOrReil"].ToString() == "Bobbins")
                            {
                                LbBobbinsWTitleWireTerminal.Text = "ទម្ងន់ប៊ូប៊ីន (KG)";
                                LbNetWTitleWireTerminal.Text = "ទម្ងន់សាច់សុទ្ធ (KG)";
                                LbNetWTitleWireTerminal.Font = new Font("Khmer OS Battambang", 14);
                                LbQtyTitleWireTerminal.Text = "សម្រាយចំនួន";

                            }
                            else
                            {
                                LbBobbinsWTitleWireTerminal.Text = "ប្រវែងគ្មានធើមីណល R2 (cm)";
                                LbBobbinsWTitleWireTerminal.Font = new Font("Khmer OS Battambang", 11, FontStyle.Regular);
                                LbNetWTitleWireTerminal.Text = "ប្រវែងពេញ R1 (cm)";
                                LbQtyTitleWireTerminal.Text = "សម្រាយចំនួន";
                            }
                            NetWSelected = Convert.ToDouble(dt.Rows[0]["R1OrNetW"].ToString());
                            MOQSelected = Convert.ToDouble(dt.Rows[0]["MOQ"].ToString());
                            LbNetWWireTerminal.Text = NetWSelected.ToString("N2") + " = " + MOQSelected.ToString("N0");
                        }
                        txtQtyWireTerminal.Text = QtyDetails.Replace("x", "*");
                        panelStockCardWireTerminal.Visible = true;
                        panelStockCardWireTerminal.BringToFront();
                    }
                    else
                    {
                        MessageBox.Show(ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //Semi
            else
            {
                ErrorText = "";
                LbWIPCodeSemi.Text = ItemCode;
                string[] QtyD = QtyDetails.Split('x');
                txtBatchQtySemi.Text = QtyD[0].ToString();
                if (QtyD[1].ToString().Contains("+") == true)
                {
                    string[] QtyPlus = QtyD[1].Split('+');
                    txtQtyPerBatchSemi.Text = QtyPlus[0].ToString();
                    txtQtyReaySemi.Text = QtyPlus[1].ToString();
                }
                else
                {
                    txtQtyPerBatchSemi.Text = QtyD[1].ToString();
                }
                LbTotalQtySemi.Text = Convert.ToDouble(dtInventory.Rows[0]["Qty"].ToString()).ToString("N0");

                //Take Semi Detail from OBS
                DataTable dtSemiOBS = new DataTable();
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, Remark2,Remark3,Remark4 FROM mstitem WHERE ItemType = 1 AND DelFlag = 0 AND ItemCode='" + ItemCode + "' ", cnnOBS.conOBS);
                    sda.Fill(dtSemiOBS);
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
                    //Show POS Details
                    if (dtSemiOBS.Rows.Count > 0)
                    {
                        LbWIPNameSemi.Text = dtSemiOBS.Rows[0]["ItemName"].ToString();
                        LbLengthSemi.Text = dtSemiOBS.Rows[0]["Remark4"].ToString();
                        LbPINSemi.Text = dtSemiOBS.Rows[0]["Remark2"].ToString();
                        LbWireTubeSemi.Text = dtSemiOBS.Rows[0]["Remark3"].ToString();
                    }
                    panelSemi.Visible = true;
                    panelSemi.BringToFront();
                }
                else
                {
                    MessageBox.Show(ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            btnDelete.Focus();
        }

    }
}
