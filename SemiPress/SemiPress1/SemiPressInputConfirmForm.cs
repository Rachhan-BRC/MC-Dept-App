using MachineDeptApp.InputTransferSemi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.SemiPress
{
    public partial class SemiPressInputConfirmForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SemiPressInputForm fgrid;
        DataTable TotalPOS;
        DataTable PosDetails;
        string OldWipCode = "";
        string OldWipName = "";

        public SemiPressInputConfirmForm(SemiPressInputForm fg)
        {
            InitializeComponent();
            this.fgrid = fg;
            cnn.Connection();
            this.txtQty.KeyPress += TxtQty_KeyPress;
            this.txtWipCode.Leave += TxtWipCode_Leave;
            this.txtWipCode.Enter += TxtWipCode_Enter;
            this.txtWipCode.KeyDown += TxtWipCode_KeyDown;
            this.txtWipCode.KeyPress += TxtWipCode_KeyPress;
            this.txtBoxNo.KeyPress += TxtBoxNo_KeyPress;
            this.txtPosNo.KeyPress += TxtPosNo_KeyPress;
        }

        private void TxtPosNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != '-') && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '-') && ((sender as TextBox).Text.IndexOf('-') > -1))
            {
                e.Handled = true;
            }
        }

        private void TxtBoxNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtWipCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtWipCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK.Focus();
            }
            else
            {

            }
        }

        private void TxtWipCode_Enter(object sender, EventArgs e)
        {
            OldWipCode = txtWipCode.Text;
            OldWipName = txtWipName.Text;
        }

        private void TxtWipCode_Leave(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM tbMasterItem WHERE ItemCode ='" + txtWipCode.Text + "';", cnn.con);
            try
            {
                cnn.con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtWipName.Text = dr.GetValue(1).ToString();

                }
                else
                {
                    txtWipName.Text = "";
                }
            }
            catch
            {
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
            if (txtWipName.Text.Trim() == "")
            {
                MessageBox.Show("គ្មានកូដនេះកម្មវិធីនេះទេ ! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtWipCode.Text = OldWipCode;
                txtWipName.Text = OldWipName;
            }
            else
            {

            }
        }

        private void TxtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SemiPressInputConfirmForm_Load(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 250;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;
            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.btnOK, "OK");

            int boxNo = Convert.ToInt32(SemiPressInputForm.BoxNo);
            txtPosNo.Text = SemiPressInputForm.PosNo.Trim();
            txtWipCode.Text = SemiPressInputForm.WipCode.ToString();
            txtQty.Text = SemiPressInputForm.Qty.ToString();
            txtBoxNo.Text = boxNo.ToString().Trim();

            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT PosCNo, WIPCode, PosCQty, PosPDelDate FROM tbPOSDetailofMC WHERE PosCNo ='" + txtPosNo.Text.Trim() + "' AND WipCode='" + txtWipCode.Text.Trim() + "';", cnn.con);
                PosDetails = new DataTable();
                sda.Fill(PosDetails);
                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT PosNo, WipCode,SUM(convert(decimal(9,0),Qty))as TotalQty FROM tbSemiPress WHERE POSNo ='" + txtPosNo.Text.Trim() + "' AND WipCode='" + txtWipCode.Text.Trim() + "' AND DeleteState='0'  GROUP BY PosNo, WipCode;", cnn.con);
                TotalPOS = new DataTable();
                sda1.Fill(TotalPOS);
                if (TotalPOS.Rows.Count == 0)
                {
                    Timer timer = new Timer();
                    timer.Interval = 500; // 0.5 seconds            
                    timer.Tick += new EventHandler(timer_Tick);
                    timer.Start();
                }
                else
                {
                    if (Convert.ToDouble(TotalPOS.Rows[0][2].ToString()) == Convert.ToDouble(PosDetails.Rows[0][2].ToString()))
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
            }
            catch
            {
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            cnn.con.Close();

            SqlCommand cmd = new SqlCommand("SELECT * FROM tbMasterItem WHERE ItemCode ='" + txtWipCode.Text + "';", cnn.con);
            try
            {
                cnn.con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtWipName.Text = dr.GetValue(1).ToString();

                }
            }
            catch
            {
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            cnn.con.Close();

        }

        void timer_Tick(object sender, EventArgs e)
        {
            ((Timer)sender).Stop();
            //Check complete or not
            if (TotalPOS.Rows.Count > 0)
            {
                double Total = Convert.ToDouble(PosDetails.Rows[0][2].ToString());
                double AlreadyTrans = Convert.ToDouble(TotalPOS.Rows[0][2].ToString());
                double NewTrans = Convert.ToDouble(txtQty.Text.ToString());

                for (int i = 0; i < fgrid.dgvScannedTag.Rows.Count; i++)
                {
                    string PosNo = txtPosNo.Text.Trim();
                    if (fgrid.dgvScannedTag.Rows[i].Cells[0].Value.ToString().Trim() == PosNo)
                    {
                        NewTrans = NewTrans + Convert.ToDouble(fgrid.dgvScannedTag.Rows[i].Cells[4].Value.ToString());
                    }
                    else
                    {

                    }
                }

                if (Total > AlreadyTrans + NewTrans)
                {
                    btnOK.Focus();

                }
                else if (Total == AlreadyTrans + NewTrans)
                {
                    MessageBox.Show("POS នេះគ្រប់ចំនួនហើយ ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnOK.Focus();

                }
                else
                {
                    MessageBox.Show("លើសចំនួន POS ហើយ ​!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                double Total = Convert.ToDouble(PosDetails.Rows[0][2].ToString());

                double AlreadyTrans = 0;

                double NewTrans = Convert.ToDouble(txtQty.Text.ToString());
                for (int i = 0; i < fgrid.dgvScannedTag.Rows.Count; i++)
                {
                    string PosNo = txtPosNo.Text.Trim();
                    if (fgrid.dgvScannedTag.Rows[i].Cells[0].Value.ToString().Trim() == PosNo)
                    {
                        NewTrans = NewTrans + Convert.ToDouble(fgrid.dgvScannedTag.Rows[i].Cells[4].Value.ToString());
                    }
                    else
                    {

                    }
                }
                if (Total > AlreadyTrans + NewTrans)
                {
                    btnOK.Focus();

                }
                else if (Total == AlreadyTrans + NewTrans)
                {
                    MessageBox.Show("POS នេះគ្រប់ចំនួនហើយ ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnOK.Focus();

                }
                else
                {
                    MessageBox.Show("លើសចំនួន POS ហើយ ​!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
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
                                if (TotalPOS.Rows.Count > 0)
                                {
                                    double Total = Convert.ToDouble(PosDetails.Rows[0][2].ToString());

                                    double AlreadyTrans = Convert.ToDouble(TotalPOS.Rows[0][2].ToString());

                                    double NewTrans = Convert.ToDouble(txtQty.Text.ToString());
                                    for (int i = 0; i < fgrid.dgvScannedTag.Rows.Count; i++)
                                    {
                                        string PosNo = txtPosNo.Text.Trim();
                                        if (fgrid.dgvScannedTag.Rows[i].Cells[0].Value.ToString().Trim() == PosNo)
                                        {
                                            NewTrans = NewTrans + Convert.ToDouble(fgrid.dgvScannedTag.Rows[i].Cells[4].Value.ToString());
                                        }
                                        else
                                        {

                                        }
                                    }

                                    if (Total > AlreadyTrans + NewTrans || Total == AlreadyTrans + NewTrans)
                                    {
                                        fgrid.dgvScannedTag.Rows.Add(txtPosNo.Text, txtWipCode.Text, txtWipName.Text, txtBoxNo.Text, Convert.ToInt64(txtQty.Text), txtRemark.Text);
                                        fgrid.dgvScannedTag.ClearSelection();
                                        this.Close();
                                    }
                                    else
                                    {
                                        MessageBox.Show("អ្នកបញ្ចូលទិន្នន័យលើសចំនួន POS ហើយ ​! \nចំនួន POS = " + Total.ToString("N0") + "\nចំនួនវេរ     = " + (AlreadyTrans + NewTrans).ToString("N0") + " (" + AlreadyTrans + "+" + NewTrans + ")", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    }
                                }
                                else if (TotalPOS.Rows.Count == 0)
                                {
                                    double Total = Convert.ToDouble(PosDetails.Rows[0][2].ToString());

                                    double AlreadyTrans = 0;

                                    double NewTrans = Convert.ToDouble(txtQty.Text.ToString());
                                    for (int i = 0; i < fgrid.dgvScannedTag.Rows.Count; i++)
                                    {
                                        string PosNo = txtPosNo.Text.Trim();
                                        if (fgrid.dgvScannedTag.Rows[i].Cells[0].Value.ToString().Trim() == PosNo)
                                        {
                                            NewTrans = NewTrans + Convert.ToDouble(fgrid.dgvScannedTag.Rows[i].Cells[4].Value.ToString());
                                        }
                                        else
                                        {

                                        }
                                    }

                                    if (Total > AlreadyTrans + NewTrans || Total == AlreadyTrans + NewTrans)
                                    {
                                        int box = Convert.ToInt32(txtBoxNo.Text.ToString());
                                        int qty = Convert.ToInt32(txtQty.Text.ToString());
                                        fgrid.dgvScannedTag.Rows.Add(txtPosNo.Text, txtWipCode.Text, txtWipName.Text, txtBoxNo.Text, Convert.ToInt64(txtQty.Text), txtRemark.Text);
                                        fgrid.dgvScannedTag.ClearSelection();
                                        this.Close();
                                    }
                                    else
                                    {
                                        MessageBox.Show("អ្នកបញ្ចូលទិន្នន័យលើសចំនួន POS ហើយ ​! \nចំនួន POS = " + Total.ToString("N0") + "\nចំនួនវេរ     = " + (AlreadyTrans + NewTrans).ToString("N0") + " (" + AlreadyTrans + "+" + NewTrans + ")", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    }
                                }
                            }
                            catch
                            {
                                MessageBox.Show("លេខកាតុង និង ចំនួនត្រូវតែជាលេខ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

    }
}
