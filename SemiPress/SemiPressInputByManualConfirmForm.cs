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
    public partial class SemiPressInputByManualConfirmForm : Form
    {
        SQLConnect cnn= new SQLConnect();
        SemiPressInputByManualForm fgrid1;
        SqlCommand cmd;
        DataTable TotalPOS;
        DataTable PosDetails;

        public SemiPressInputByManualConfirmForm(SemiPressInputByManualForm fg1)
        {
            InitializeComponent();
            this.fgrid1 = fg1;
            cnn.Connection();
            this.txtBoxNo.KeyPress += TxtBoxNo_KeyPress;
            this.txtQty.KeyPress += TxtQty_KeyPress;
            this.txtQty.Leave += TxtQty_Leave;
            this.txtPosNo.KeyPress += TxtPosNo_KeyPress;
            this.txtPosNo.Leave += TxtPosNo_Leave;

        }

        private void TxtPosNo_Leave(object sender, EventArgs e)
        {
            if (txtPosNo.Text.Trim() == "")
            {
                MessageBox.Show("សូមបំពេញលេខ POS ជាមុនសិន !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPosNo.Focus();
            }
            else
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbPOSData WHERE POSNo ='" + txtPosNo.Text.Trim() + "' AND WipCode='" + txtWipCode.Text.Trim() + "';", cnn.con);
                PosDetails = new DataTable();
                sda.Fill(PosDetails);
                if (PosDetails.Rows.Count > 0)
                {
                    SqlDataAdapter sda1 = new SqlDataAdapter("SELECT PosNo, WipCode,SUM(convert(decimal(9,0),Qty))as TotalQty FROM tbSemiPress WHERE POSNo ='" + txtPosNo.Text.Trim() + "' AND WipCode='" + txtWipCode.Text.Trim() + "' AND DeleteState='0' GROUP BY PosNo, WipCode;", cnn.con);
                    TotalPOS = new DataTable();
                    sda1.Fill(TotalPOS);
                    if (TotalPOS.Rows.Count == 0)
                    {

                    }
                    else
                    {
                        if (Convert.ToDouble(TotalPOS.Rows[0][2].ToString()) == Convert.ToDouble(PosDetails.Rows[0][2].ToString()))
                        {
                            MessageBox.Show("POS នេះត្រូវបានផលិតចប់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txtPosNo.Text = "";
                            txtPosNo.Focus();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("មិនមាន POS នេះនៅក្នុងប្រព័ន្ធទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPosNo.Text = "";
                    txtPosNo.Focus();
                }
                cnn.con.Close();
            }

        }

        private void TxtPosNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != '-') && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '-') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('-') > -1))
            {
                e.Handled = true;
            }
        }

        private void TxtQty_Leave(object sender, EventArgs e)
        {
            if (txtQty.Text.ToString().Trim() == "")
            {

            }
            else
            {
                Timer timer = new Timer();
                timer.Interval = 500; // 0.5 seconds            
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
            }
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
                for (int i = 0; i < fgrid1.dgvInputWip.Rows.Count; i++)
                {
                    string PosNo = txtPosNo.Text.Trim();
                    if (fgrid1.dgvInputWip.Rows[i].Cells[0].Value.ToString().Trim() == PosNo)
                    {
                        NewTrans = NewTrans + Convert.ToDouble(fgrid1.dgvInputWip.Rows[i].Cells[4].Value.ToString());
                    }
                    else
                    {

                    }
                }
                if (Total > AlreadyTrans + NewTrans)
                {
                    txtBoxNo.Focus();

                }
                else if (Total == AlreadyTrans + NewTrans)
                {
                    MessageBox.Show("POS នេះគ្រប់ចំនួនហើយ ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBoxNo.Focus();

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
                for (int i = 0; i < fgrid1.dgvInputWip.Rows.Count; i++)
                {
                    string PosNo = txtPosNo.Text.Trim();
                    if (fgrid1.dgvInputWip.Rows[i].Cells[0].Value.ToString().Trim() == PosNo)
                    {
                        NewTrans = NewTrans + Convert.ToDouble(fgrid1.dgvInputWip.Rows[i].Cells[4].Value.ToString());
                    }
                    else
                    {

                    }
                }
                if (Total > AlreadyTrans + NewTrans)
                {
                    txtBoxNo.Focus();

                }
                else if (Total == AlreadyTrans + NewTrans)
                {
                    MessageBox.Show("POS នេះគ្រប់ចំនួនហើយ ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBoxNo.Focus();

                }
                else
                {
                    MessageBox.Show("លើសចំនួន POS ហើយ ​!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

        private void TxtBoxNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SemiPressInputByManualConfirmForm_Load(object sender, EventArgs e)
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

            txtWipCode.Text = SemiPressInputByManualForm.WipCode;
            txtWipName.Text = SemiPressInputByManualForm.WipName;
            txtPin.Text = SemiPressInputByManualForm.Pin;
            txtWire.Text = SemiPressInputByManualForm.Wire;
            txtLength.Text = SemiPressInputByManualForm.Length;
            txtQty.Focus();

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtQty.Text.Trim() != "" && txtPosNo.Text.Trim() != "" && txtBoxNo.Text.Trim() != "")
            {
                if (txtPosNo.Text.Length < 10 || (Regex.Matches(txtPosNo.Text.ToString(), "[~!@#$%^&*()_+{}:\"<>?-]").Count) > 1 || (Regex.Matches(txtPosNo.Text.ToString(), "[-]").Count) < 1 || txtPosNo.Text.ToString().Trim()[2].ToString() != "-")
                {
                    MessageBox.Show("លេខ​ POS ខុសទម្រង់ហើយ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPosNo.Focus();
                }
                else
                {
                    try
                    {
                        if (txtBoxNo.Text.Trim().ToString() == "0")
                        {
                            MessageBox.Show("លេខកាតុងមិនអាចជាលេខ ០ បានទេ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            int box = Convert.ToInt32(txtBoxNo.Text.ToString());
                            int qty = Convert.ToInt32(txtQty.Text.ToString());

                            int DupRow = 0;

                            for (int i = 0; i < fgrid1.dgvInputWip.Rows.Count; i++)
                            {
                                string PosNo = txtPosNo.Text.Trim();
                                int Box = Convert.ToInt32(txtBoxNo.Text.ToString().Trim());
                                if (fgrid1.dgvInputWip.Rows[i].Cells[0].Value.ToString().Trim() == PosNo && Convert.ToInt32(fgrid1.dgvInputWip.Rows[i].Cells[3].Value.ToString().Trim()) == Box)
                                {
                                    DupRow = DupRow + 1;
                                }
                                else
                                {

                                }
                            }
                            //Check In Dgv
                            if (DupRow > 0)
                            {
                                MessageBox.Show("ទិន្នន័យនេះអ្នកទើបតែបញ្ចូលរួចមុននេះ​ ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            }
                            else
                            {
                                cnn.con.Open();
                                SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM tbSemiPress WHERE PosNo ='" + txtPosNo.Text.Trim() + "' AND BoxNo='" + Convert.ToInt32(txtBoxNo.Text.Trim()) + "' AND DeleteState='0';", cnn.con);
                                System.Data.DataTable dt = new System.Data.DataTable();
                                sda.Fill(dt);
                                if (Convert.ToInt32(dt.Rows[0][0].ToString()) > 0)
                                {
                                    MessageBox.Show("ទិន្នន័យនេះធ្លាប់បញ្ចូលម្ដងរួចមកហើយ ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                }
                                else
                                {
                                    if (txtQty.Text.Trim().ToString() != "0")
                                    {
                                        if (TotalPOS.Rows.Count > 0)
                                        {
                                            double Total = Convert.ToDouble(PosDetails.Rows[0][2].ToString());

                                            double AlreadyTrans = Convert.ToDouble(TotalPOS.Rows[0][2].ToString());

                                            double NewTrans = Convert.ToDouble(txtQty.Text.ToString());
                                            for (int i = 0; i < fgrid1.dgvInputWip.Rows.Count; i++)
                                            {
                                                string PosNo = txtPosNo.Text.Trim();
                                                if (fgrid1.dgvInputWip.Rows[i].Cells[0].Value.ToString().Trim() == PosNo)
                                                {
                                                    NewTrans = NewTrans + Convert.ToDouble(fgrid1.dgvInputWip.Rows[i].Cells[4].Value.ToString());
                                                }
                                                else
                                                {

                                                }
                                            }

                                            if (Total > AlreadyTrans + NewTrans || Total == AlreadyTrans + NewTrans)
                                            {
                                                fgrid1.dgvInputWip.Rows.Add(txtPosNo.Text, txtWipCode.Text, txtWipName.Text, txtBoxNo.Text, Convert.ToInt64(txtQty.Text), txtRemark.Text);
                                                fgrid1.dgvInputWip.ClearSelection();
                                                fgrid1.dgvWipCode.Rows.Clear();
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
                                            for (int i = 0; i < fgrid1.dgvInputWip.Rows.Count; i++)
                                            {
                                                string PosNo = txtPosNo.Text.Trim();
                                                if (fgrid1.dgvInputWip.Rows[i].Cells[0].Value.ToString().Trim() == PosNo)
                                                {
                                                    NewTrans = NewTrans + Convert.ToDouble(fgrid1.dgvInputWip.Rows[i].Cells[4].Value.ToString());
                                                }
                                                else
                                                {

                                                }
                                            }

                                            if (Total > AlreadyTrans + NewTrans || Total == AlreadyTrans + NewTrans)
                                            {
                                                fgrid1.dgvInputWip.Rows.Add(txtPosNo.Text, txtWipCode.Text, txtWipName.Text, txtBoxNo.Text, Convert.ToInt64(txtQty.Text), txtRemark.Text);
                                                fgrid1.dgvInputWip.ClearSelection();
                                                fgrid1.dgvWipCode.Rows.Clear();
                                                this.Close();
                                            }
                                            else
                                            {
                                                MessageBox.Show("អ្នកបញ្ចូលទិន្នន័យលើសចំនួន POS ហើយ ​! \nចំនួន POS = " + Total.ToString("N0") + "\nចំនួនវេរ     = " + (AlreadyTrans + NewTrans).ToString("N0") + " (" + AlreadyTrans + "+" + NewTrans + ")", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("ចំនួនមិនអាចស្មើ 0 បានទេ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    }
                                }
                                cnn.con.Close();
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("លេខកាតុង និង ចំនួនត្រូវតែជាលេខ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
