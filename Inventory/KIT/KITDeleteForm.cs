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

namespace MachineDeptApp.Inventory.KIT
{
    public partial class KITDeleteForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SqlCommand cmd;
        string TypeOfCounting;

        public KITDeleteForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += KITDeleteForm_Load;
            this.btnNew.Click += BtnNew_Click;
            this.btnDelete.Click += BtnDelete_Click;
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;
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
                            "WHERE CancelStatus = 0 AND LocCode ='KIT3' AND LabelNo=" + txtBarcode.Text + " " +
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
                            ErrorText = ErrorText + "\n" + ex.Message;
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
                                    ItemCodeIn = "'" + row[3].ToString() + "'";
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
                                    "WHERE ItemCode IN (" + ItemCodeIn + ");", cnnOBS.conOBS);
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
                                            "WHERE DONo='" + LbPOSNo.Text + "' ", cnnOBS.conOBS);
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
                                        btnDelete.Focus();
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
                                    btnDelete.Focus();
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
                        MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.Focus();
                    }
                }
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (TypeOfCounting == "POS")
            {
                DeletePOS();
            }
            else
            {
                DeleteFraction();
            }
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearTextAll();
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }
        private void KITDeleteForm_Load(object sender, EventArgs e)
        {
            panelFraction.BringToFront();
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
        private void DeletePOS()
        {
            if (dgvPOSList.Rows.Count > 0 && LbPOSLabelNo.Text.Trim() != "")
            {
                if (dgvPOSList.Rows.Count > 0)
                {
                    DialogResult DSL = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DSL == DialogResult.Yes)
                    {
                        string ErrorText = "";
                        LbStatus.Text = "កំពុងលុប​ . . .";
                        LbStatus.Refresh();
                        Cursor = Cursors.WaitCursor;

                        try
                        {
                            cnn.con.Open();
                            DateTime UpdateDate = DateTime.Now;
                            string UserName = MenuFormV2.UserForNextForm;

                            string query = "UPDATE tbInventory SET CancelStatus=1, " +
                                                    "UpdateDate='" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                    "UpdateBy=N'" + UserName + "' WHERE LocCode='KIT3' AND LabelNo=" + Convert.ToInt32(LbPOSLabelNo.Text) + "";
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
                                ErrorText = ErrorText + "\n" + ex.Message;
                            }
                        }
                        cnn.con.Close();

                        Cursor = Cursors.Default;
                        if (ErrorText.Trim() == "")
                        {
                            LbStatus.Text = "លុបរួចរាល់!";
                            LbStatus.Refresh();
                            MessageBox.Show("លុបរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("គ្មានទិន្នន័យសម្រាប់លុបទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtBarcode.Focus();
                }
            }
            else
            {
                MessageBox.Show("សូមស្កេនបាកូដជាមុនសិន!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBarcode.Focus();
            }
        }
        private void DeleteFraction()
        {
            if (LbFractQty.Text.Trim() != "" && LbFractLabelNo.Text != "")
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    string ErrorText = "";
                    LbStatus.Text = "កំពុងលុប​ . . .";
                    LbStatus.Refresh();
                    Cursor = Cursors.WaitCursor;

                    try
                    {
                        cnn.con.Open();
                        DateTime UpdateDate = DateTime.Now;
                        string UserName = MenuFormV2.UserForNextForm;

                        string query = "UPDATE tbInventory SET CancelStatus=1, " +
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
                            ErrorText = ErrorText + "\n" + ex.Message;
                        }
                    }
                    cnn.con.Close();

                    Cursor = Cursors.Default;
                    if (ErrorText.Trim() == "")
                    {
                        LbStatus.Text = "លុបរួចរាល់!";
                        LbStatus.Refresh();
                        MessageBox.Show("លុបរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("សូមស្កេនបាកូដជាមុនសិន!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBarcode.Focus();
            }
        }
    }
}
