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

namespace MachineDeptApp.Inventory.MC_SD
{
    public partial class MCSDDeleteForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SqlCommand cmd;
        int LabelNoSelected;
        string CountType;
        string ErrorText;

        public MCSDDeleteForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;

            //btn
            this.btnNew.Click += BtnNew_Click;
            this.btnDelete.Click += BtnDelete_Click;
        }


        //btn
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (LbCode.Text.Trim() != "" && LbTotalQty.Text.Trim()!="")
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនទេ?","Rachhan System",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
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
                                                    "UpdateBy=N'" + Username + "' WHERE LocCode='WIR1' AND LabelNo=" + LabelNoSelected + " ";
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
                        MessageBox.Show("លុបរួចរាល់!","Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        ClearAllText();
                        txtBarcode.Focus();
                    }
                    else
                    {
                        MessageBox.Show("មានបញ្ហា!\n"+ErrorText,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
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
                            "ON T1.ItemCode=T2.Code WHERE T1.CancelStatus=0 AND T1.LocCode='WIR1' AND T1.LabelNo='" + txtBarcode.Text + "'", cnn.con);
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
                            ErrorText = ErrorText + "\n" + ex.Message;
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
                                SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemName, Resv1 FROM mstitem WHERE ItemType=2 AND ItemCode='" + dtMCDB.Rows[0][3].ToString() + "'", cnnOBS.conOBS);
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
                                txtQty.Text = "";
                                CountType = dtMCDB.Rows[0][2].ToString();
                                LbCode.Text = dtMCDB.Rows[0][3].ToString();
                                LbItems.Text = dtRMOBS.Rows[0][0].ToString();
                                LbMaker.Text = dtRMOBS.Rows[0][1].ToString();
                                LbType.Text = dtMCDB.Rows[0][7].ToString();
                                if (CountType == "Weight")
                                {
                                    ShowWeight();
                                    //Separate Detail
                                    string[] DetailArray = dtMCDB.Rows[0][6].ToString().Split('|');
                                    double BobbinQty = Convert.ToDouble(DetailArray[1].ToString());
                                    if (dtMCDB.Rows[0][8].ToString() == "Reil")
                                    {
                                        LbBobbinsWTitle.Text = "ប្រវែងពេញ R1 (cm)";
                                        LbNetWTitle.Text = "ប្រវែងគ្មានធើមីណល R2 (cm)";
                                        LbNetWTitle.Font = new Font("Khmer OS Battambang", 11, FontStyle.Regular);
                                        LbQtyTitle.Text = "ប្រវែងសរុប";
                                        CboBobbinW.Items.Clear();
                                        CboBobbinW.Items.Add(DetailArray[0].ToString());
                                        CboBobbinW.Text = DetailArray[0].ToString();
                                        if (BobbinQty == 1)
                                        {
                                            LbBobbinQty.Text = "( "+BobbinQty.ToString()+" Reel )";
                                        }
                                        else
                                        {
                                            LbBobbinQty.Text = "( " + BobbinQty.ToString() + " Reels )";
                                        }
                                    }
                                    else
                                    {
                                        LbBobbinsWTitle.Text = "ទម្ងន់ប៊ូប៊ីន (KG)";
                                        LbNetWTitle.Text = "ទម្ងន់សាច់សុទ្ធ (KG)";
                                        LbNetWTitle.Font = new Font("Khmer OS Battambang", 14);
                                        LbQtyTitle.Text = "ទម្ងន់សរុប";
                                        CboBobbinW.Items.Clear();
                                        CboBobbinW.Items.Add(DetailArray[0].ToString());
                                        CboBobbinW.Text = DetailArray[0].ToString();
                                        if (BobbinQty == 1)
                                        {
                                            LbBobbinQty.Text = "( " + BobbinQty.ToString() + " Bobbin )";
                                        }
                                        else
                                        {
                                            LbBobbinQty.Text = "( " + BobbinQty.ToString() + " Bobbins )";
                                        }
                                    }
                                }
                                else
                                {
                                    double BobbinQty = Convert.ToDouble(dtMCDB.Rows[0][6].ToString());
                                    LbBobbinsWTitle.Text = "ទម្ងន់ប៊ូប៊ីន (KG)";
                                    LbNetWTitle.Text = "ទម្ងន់សាច់សុទ្ធ (KG)";
                                    LbNetWTitle.Font = new Font("Khmer OS Battambang", 14);
                                    LbQtyTitle.Text = "សម្រាយចំនួន";
                                    ShowBox();
                                    if (dtMCDB.Rows[0][8].ToString() == "Reil")
                                    {
                                        LbBobbinsWTitle.Text = "ប្រវែងពេញ R1 (cm)";
                                        LbNetWTitle.Text = "ប្រវែងគ្មានធើមីណល R2 (cm)";
                                        LbNetWTitle.Font = new Font("Khmer OS Battambang", 11, FontStyle.Regular);
                                        if (BobbinQty == 1)
                                        {
                                            LbBobbinQty.Text = "( " + BobbinQty.ToString() + " Reel )";
                                        }
                                        else
                                        {
                                            LbBobbinQty.Text = "( " + BobbinQty.ToString() + " Reels )";
                                        }
                                    }
                                    else
                                    {
                                        LbBobbinsWTitle.Text = "ទម្ងន់ប៊ូប៊ីន (KG)";
                                        LbNetWTitle.Text = "ទម្ងន់សាច់សុទ្ធ (KG)";
                                        LbNetWTitle.Font = new Font("Khmer OS Battambang", 14);
                                        if (BobbinQty == 1)
                                        {
                                            LbBobbinQty.Text = "( " + BobbinQty.ToString() + " Bobbin )";
                                        }
                                        else
                                        {
                                            LbBobbinQty.Text = "( " + BobbinQty.ToString() + " Bobbins )";
                                        }
                                    }
                                }

                                LbNetW.Text = Convert.ToDouble(dtMCDB.Rows[0][9].ToString()) + " = " + Convert.ToDouble(dtMCDB.Rows[0][10].ToString()).ToString("N0");
                                txtQty.Text = dtMCDB.Rows[0][5].ToString();
                                LbTotalQty.Text = Convert.ToDouble(dtMCDB.Rows[0][4].ToString()).ToString("N0");
                                LabelNoSelected = Convert.ToInt32(txtBarcode.Text);
                                LbCode.Text = LbCode.Text + "                                   លេខរៀងឡាប៊ែល   ៖   " + LabelNoSelected;
                                txtBarcode.Text = "";
                                btnDelete.Focus();
                            }
                            else
                            {
                                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
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
                        MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ShowWeight()
        {
            groupBox3.Text = "ព៌ត័មានរបស់វត្ថុធាតុដើម ( Count by " + CountType + " )";
        }
        private void ShowBox()
        {
            groupBox3.Text = "ព៌ត័មានរបស់វត្ថុធាតុដើម ( Count by " + CountType + " )";
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
            LbTotalQty.Text = "";
            LbBobbinQty.Text = "";
        }

    }
}
