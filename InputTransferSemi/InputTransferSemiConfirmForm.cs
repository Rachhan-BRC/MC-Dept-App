using MachineDeptApp.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace MachineDeptApp.InputTransferSemi
{
    public partial class InputTransferSemiConfirmForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        DataTable PosDetails;
        InputTransferSemiForm fgrid;

        public InputTransferSemiConfirmForm(InputTransferSemiForm fg)
        {
            InitializeComponent();
            cnn.Connection();
            this.fgrid = fg;
            this.txtQty.KeyPress += TxtQty_KeyPress;
            this.txtQty.Leave += TxtQty_Leave;

        }

        private void TxtQty_Leave(object sender, EventArgs e)
        {
            if (txtQty.Text.Trim() != "")
            {
                try
                {
                    double Qty = Convert.ToDouble(txtQty.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQty.Text = "";
                }
            }
        }
        private void TxtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtPosNo.Text.Trim() != "" && txtWipCode.Text.Trim() != "" && txtWipName.Text.Trim() != "" && txtBoxNo.Text.Trim() != "" && txtQty.Text.Trim() != "")
            {
                if (txtPosNo.Text.Length < 10 || (Regex.Matches(txtPosNo.Text.ToString(), "[~!@#$%^&*()_+{}:\"<>?-]").Count) > 1 || (Regex.Matches(txtPosNo.Text.ToString(), "[-]").Count) < 1 || txtPosNo.Text.ToString().Trim()[2].ToString() != "-")
                {
                    MessageBox.Show("លេខ​ POS ខុសទម្រង់ហើយ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPosNo.Focus();
                }
                else
                {
                    if (txtBoxNo.Text.Trim().ToString() != "0")
                    {
                        if (txtQty.Text.Trim().ToString() != "0")
                        {
                            try
                            {
                                double Remain = Convert.ToDouble(PosDetails.Rows[0]["PosCRemainQty"].ToString());
                                double AlreadyTrans = 0;
                                double NewTrans = Convert.ToDouble(txtQty.Text.ToString());
                                foreach (DataGridViewRow dgvRow in fgrid.dgvScannedTag.Rows)
                                {
                                    if (dgvRow.Cells["POSNo"].Value.ToString() == txtPosNo.Text.Trim())
                                    {
                                        NewTrans = NewTrans + Convert.ToDouble(dgvRow.Cells["Qty"].Value.ToString());
                                    }
                                }

                                if (Remain > AlreadyTrans + NewTrans || Remain == AlreadyTrans + NewTrans)
                                {
                                    fgrid.dgvScannedTag.Rows.Add();
                                    fgrid.dgvScannedTag.Rows[fgrid.dgvScannedTag.Rows.Count-1].Cells["POSNo"].Value = txtPosNo.Text;
                                    fgrid.dgvScannedTag.Rows[fgrid.dgvScannedTag.Rows.Count - 1].Cells["WIPCode"].Value = txtWipCode.Text;
                                    fgrid.dgvScannedTag.Rows[fgrid.dgvScannedTag.Rows.Count - 1].Cells["WIPName"].Value = txtWipName.Text;
                                    fgrid.dgvScannedTag.Rows[fgrid.dgvScannedTag.Rows.Count - 1].Cells["BoxNo"].Value = Convert.ToDouble(txtBoxNo.Text);
                                    fgrid.dgvScannedTag.Rows[fgrid.dgvScannedTag.Rows.Count - 1].Cells["Qty"].Value = Convert.ToDouble(txtQty.Text);
                                    fgrid.dgvScannedTag.Rows[fgrid.dgvScannedTag.Rows.Count - 1].Cells["Remarks"].Value = txtRemark.Text;
                                    fgrid.dgvScannedTag.ClearSelection();
                                    fgrid.dgvScannedTag.CurrentCell = null;
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("អ្នកបញ្ចូលទិន្នន័យលើសចំនួន POS ហើយ ​! \nចំនួន POS នៅសល់ = " + Remain.ToString("N0") + "\nចំនួនវេរ     = " + (AlreadyTrans + NewTrans).ToString("N0"), "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("លេខកាតុង និង ចំនួនត្រូវតែជាលេខ !" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                        else
                        {
                            MessageBox.Show("ចំនួនមិនអាចស្មើ 0 បានទេ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }
                    else
                    {
                        MessageBox.Show("លេខកាតុងមិនអាចជាលេខ ០ បានទេ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                MessageBox.Show("សូមបំពេញប្រអប់ដែលមានផ្ទៃក្រោយព៌ណទឹកក្រូចទាំងអស់ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }

        private void InputTransferSemiConfirmForm_Load(object sender, EventArgs e)
        {
            string [] Separate = InputTransferSemiForm.Barcode.Split('/');
            txtPosNo.Text = Separate[0].ToString();
            txtWipCode.Text = Separate[1].ToString();
            txtQty.Text = Separate[2].ToString();
            txtBoxNo.Text = Convert.ToDouble(Separate[3].ToString()).ToString();

            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT WIPCode, ItemName, PosCRemainQty, PosCStatus FROM "+
                                                                                    "(SELECT WIPCode, PosCStatus, PosCRemainQty FROM tbPOSDetailofMC WHERE PosCNo = '" + txtPosNo.Text.Trim() + "' AND WIPCode='" + txtWipCode.Text.Trim() + "') T1 "+
                                                                                    "LEFT JOIN (SELECT ItemCode, ItemName  FROM tbMasterItem WHERE ItemType='Work In Process') T2 ON T1.WIPCode = T2.ItemCode;", cnn.con);
                PosDetails = new DataTable();
                sda.Fill(PosDetails);
                txtWipName.Text = PosDetails.Rows[0]["ItemName"].ToString();
                if (PosDetails.Rows[0]["PosCStatus"].ToString() == "2")
                {
                    MessageBox.Show("POS នេះត្រូវបានផលិតចប់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Close();
                }
                else
                {
                    Timer timer = new Timer();
                    timer.Interval = 500; // 0.5 seconds            
                    timer.Tick += new EventHandler(timer_Tick);
                    timer.Start();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            cnn.con.Close();            
        }

        //Method
        void timer_Tick(object sender, EventArgs e)
        {
            ((Timer)sender).Stop();
            //Check complete or not
            btnOK.Enabled = false;
            double Remain = Convert.ToDouble(PosDetails.Rows[0][2].ToString());
            double AlreadyTrans = 0;
            double NewTrans = Convert.ToDouble(txtQty.Text.ToString());
            for (int i = 0; i < fgrid.dgvScannedTag.Rows.Count; i++)
            {
                string PosNo = txtPosNo.Text.Trim();
                if (fgrid.dgvScannedTag.Rows[i].Cells[0].Value.ToString().Trim() == PosNo)
                {
                    NewTrans = NewTrans + Convert.ToDouble(fgrid.dgvScannedTag.Rows[i].Cells[4].Value.ToString());
                }

            }
            if (Remain > AlreadyTrans + NewTrans)
            {
                btnOK.Enabled = true;
                txtQty.Focus();

            }
            else if (Remain == AlreadyTrans + NewTrans)
            {
                int FoundNGNotYet = 0;
                DataTable dtChecking = new DataTable();
                try
                {
                    cnn.con.Open();
                    string SQLQuery = "SELECT TbPlans.*, NGMC1, NGMC2, NGMC3 FROM " +
                        "\n(SELECT PosCNo, WIPCode, MC1Name, MC2Name, MC3Name FROM tbPOSDetailofMC) TbPlans " +
                        "\nLEFT JOIN (SELECT PosCNo, COUNT(RMCode) AS NGMC1 FROM tbNGInprocess WHERE MCSeqNo ='MC1' GROUP BY PosCNo) tbMC1 " +
                        "\nON TbPlans.PosCNo = tbMC1.PosCNo " +
                        "\nLEFT JOIN (SELECT PosCNo, COUNT(RMCode) AS NGMC2 FROM tbNGInprocess WHERE MCSeqNo ='MC2'GROUP BY PosCNo) tbMC2 " +
                        "\nON TbPlans.PosCNo = tbMC2.PosCNo " +
                        "\nLEFT JOIN (SELECT PosCNo, COUNT(RMCode) AS NGMC3 FROM tbNGInprocess WHERE MCSeqNo ='MC3'GROUP BY PosCNo) tbMC3 " +
                        "\nON TbPlans.PosCNo = tbMC3.PosCNo " +
                        "\nWHERE TbPlans.PosCNo='" + txtPosNo.Text.Trim() + "' AND WIPCode='" + txtWipCode.Text.Trim() + "' ";

                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtChecking);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហា​!\n"+ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnn.con.Close();
                
                if (dtChecking.Rows.Count > 0)
                {
                    if (dtChecking.Rows[0]["MC1Name"].ToString().Trim()!="" && dtChecking.Rows[0]["NGMC1"].ToString().Trim() == "")
                    {
                        FoundNGNotYet++;
                    }
                    if (dtChecking.Rows[0]["MC2Name"].ToString().Trim() != "" && dtChecking.Rows[0]["NGMC2"].ToString().Trim() == "")
                    {
                        FoundNGNotYet++;
                    }
                    if (dtChecking.Rows[0]["MC3Name"].ToString().Trim() != "" && dtChecking.Rows[0]["NGMC3"].ToString().Trim() == "")
                    {
                        FoundNGNotYet++;
                    }

                    if (FoundNGNotYet == 0)
                    {
                        MessageBox.Show("POS នេះគ្រប់ចំនួនហើយ ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnOK.Enabled = true;
                        txtQty.Focus();
                    }
                    else
                    {
                        MessageBox.Show("POS នេះមិនទាន់បញ្ចូល NG រួចរាល់ទេ!\nត្រូវបញ្ចប់ទិន្នន័យ NG ជាមុនសិនមុននឹងបញ្ចប់ POS!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                MessageBox.Show("លើសចំនួន POS ហើយ ​!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
    }
}
