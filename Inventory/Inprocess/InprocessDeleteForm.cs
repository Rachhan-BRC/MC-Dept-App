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

        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.Trim() != "")
                {
                    Cursor = Cursors.WaitCursor;
                    ErrorText = "";
                    string Barcode = txtBarcode.Text;
                    dtInventory = new DataTable();
                    try
                    {
                        cnn.con.Open();
                        if (Barcode.Contains("/") == true)
                        {
                            string[] splitText = Barcode.Split('/');
                            Barcode = splitText[0];
                        }
                        string SQLQuery = "SELECT SubLoc, CountingMethod, CountingMethod2, ItemType, ItemCode, Qty, QtyDetails, QtyDetails2 FROM tbInventory " +
                                                    "\nWHERE LocCode='MC1' AND CancelStatus=0 AND LabelNo=" + Convert.ToInt32(Barcode) + " ORDER BY SeqNo ASC";                        
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
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
                            CboSubLoc.Items.Clear();
                            CboSubLoc.Items.Add(SubLoc);
                            CboSubLoc.SelectedIndex = 0;
                            LbLabelNo.Text = Barcode;
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

                        //For SD
                        query = "DELETE FROM tbInventoryWandTDetails " +
                                "WHERE LabelNo=" + Convert.ToInt32(LbLabelNo.Text);
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
            LbDocumentSD.Text = "";

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
            //Semi
            else if (CountMethod == "Semi")
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
            //WireTerminal
            else
            {
                LbDocumentSD.Text = QtyDetails;
                ErrorText = "";
                DataTable dtRMDetails = new DataTable();
                try
                {
                    cnn.con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT tbInventory.ItemCode, ItemName, Qty FROM tbInventory " +
                        "\nINNER JOIN (SELECT * FROM tbMasterItem WHERE ItemType = 'Material') T1 ON tbInventory.ItemCode = T1.ItemCode " +
                        "\nWHERE LocCode='MC1' AND CancelStatus=0 AND LabelNo= "+LbLabelNo.Text+" " +
                        "\nORDER BY SeqNo ASC ", cnn.con);
                    sda.Fill(dtRMDetails);
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
                    foreach (DataRow row in dtRMDetails.Rows)
                    {
                        dgvWireTerminal_W.Rows.Add();
                        dgvWireTerminal_W.Rows[dgvWireTerminal_W.Rows.Count-1].HeaderCell.Value = dgvWireTerminal_W.Rows.Count.ToString();
                        dgvWireTerminal_W.Rows[dgvWireTerminal_W.Rows.Count - 1].Cells["RMCode"].Value = row["ItemCode"].ToString();
                        dgvWireTerminal_W.Rows[dgvWireTerminal_W.Rows.Count - 1].Cells["RMName"].Value = row["ItemName"].ToString();
                        dgvWireTerminal_W.Rows[dgvWireTerminal_W.Rows.Count - 1].Cells["Qty"].Value = Convert.ToDouble(row["Qty"]);
                    }
                    panelStockCardWireTerminal.Visible = true;
                    panelStockCardWireTerminal.BringToFront();
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
