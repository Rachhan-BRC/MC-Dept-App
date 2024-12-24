using MachineDeptApp.InputTransferSemi;
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

namespace MachineDeptApp.SemiPress
{
    public partial class SemiPressInputForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SqlCommand cmd;
        public static string PosNo = "";
        public static int WipCode;
        public static long Qty;
        public static int BoxNo;
        string lastSysNo;
        int nextLabelNo;
        DataTable PosDetails;
        SqlDataReader myReader;

        public SemiPressInputForm()
        {
            InitializeComponent();
            cnn.Connection();
            this.FormClosed += SemiPressInputForm_FormClosed;
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;
            this.txtBarcode.Leave += TxtBarcode_Leave;

        }

        private void TxtBarcode_Leave(object sender, EventArgs e)
        {
            if (txtBarcode.Text.Trim() == "")
            {

            }
            else
            {
                if (txtBarcode.Text.Length >= 23)
                {
                    string OriginBarcode = txtBarcode.Text;
                    try
                    {
                        PosNo = "";
                        string separate = txtBarcode.Text;
                        // Part 1: split on a single character.
                        string[] array = separate.Split('/');
                        PosNo = array[0].Replace(" ", "");
                        WipCode = Convert.ToInt32(array[1].Replace(" ", ""));
                        Qty = Convert.ToInt64(array[2].Replace(" ", ""));
                        BoxNo = Convert.ToInt32(array[3].Replace(" ", ""));
                        txtBarcode.Text = "";
                        int DupRow = 0;

                        for (int i = 0; i < dgvScannedTag.Rows.Count; i++)
                        {
                            int Box = Convert.ToInt32(BoxNo);
                            if (dgvScannedTag.Rows[i].Cells[0].Value.ToString().Trim() == PosNo && Convert.ToInt32(dgvScannedTag.Rows[i].Cells[3].Value.ToString().Trim()) == Box)
                            {
                                DupRow = DupRow + 1;
                            }
                            else
                            {

                            }
                        }

                        if (DupRow > 0)
                        {
                            txtBarcode.Text = OriginBarcode;
                            MessageBox.Show("ឡាប៊ែលនេះស្កេនរួចហើយ​ ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtBarcode.SelectAll();
                            txtBarcode.Focus();
                        }
                        else
                        {
                            if (PosNo.Trim() != "" && WipCode.ToString().Trim() != "" && Qty.ToString().Trim() != "" && BoxNo.ToString().Trim() != "")
                            {

                                cnn.con.Open();
                                SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM tbSemiPress WHERE PosNo ='" + PosNo + "' AND BoxNo='" + BoxNo + "' AND DeleteState='0';", cnn.con);
                                DataTable dt = new DataTable();
                                sda.Fill(dt);
                                if (Convert.ToInt32(dt.Rows[0][0].ToString()) > 0)
                                {
                                    txtBarcode.Text = OriginBarcode;
                                    MessageBox.Show("ឡាប៊ែលនេះស្កេនរួចហើយ ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    txtBarcode.SelectAll();
                                    txtBarcode.Focus();
                                }
                                else
                                {
                                    SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM tbPOSData WHERE POSNo ='" + PosNo + "' AND WipCode='" + WipCode + "';", cnn.con);
                                    PosDetails = new DataTable();
                                    sda1.Fill(PosDetails);
                                    if (PosDetails.Rows.Count > 0)
                                    {
                                        txtBarcode.Focus();
                                        SemiPressInputConfirmForm Spicf = new SemiPressInputConfirmForm(this);
                                        Spicf.ShowDialog();

                                    }
                                    else
                                    {
                                        txtBarcode.Text = OriginBarcode;
                                        MessageBox.Show("មិនមាន POS នេះនៅក្នុងប្រព័ន្ធទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        txtBarcode.SelectAll();
                                        txtBarcode.Focus();
                                    }
                                }
                                cnn.con.Close();
                            }
                            else
                            {

                            }
                        }
                    }
                    catch
                    {

                        MessageBox.Show("Wrong Barcode format ! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.SelectAll();
                        txtBarcode.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Wrong Barcode format ! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBarcode.SelectAll();
                    txtBarcode.Focus();
                }
            }
        }

        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                groupBox1.Focus();
            }
            else
            {

            }
        }

        private void SemiPressInputForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            MenuForm mf = new MenuForm();
            mf.ShowDialog();
            this.Close();
        }

        private void SemiPressInputForm_Load(object sender, EventArgs e)
        {
            LbLoginID.Text = MenuForm.NameForNextForm;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            dgvScannedTag.Rows.Clear();
            txtBarcode.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvScannedTag.SelectedCells.Count > 0)
            {
                DialogResult DLR = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនដែរ ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    int selectedIndex = dgvScannedTag.CurrentCell.RowIndex;
                    if (selectedIndex > -1)
                    {
                        dgvScannedTag.Rows.RemoveAt(selectedIndex);
                        dgvScannedTag.Refresh();

                    }
                    else
                    {

                    }
                    dgvScannedTag.ClearSelection();
                    selectedIndex = -1;
                }
                else
                {

                }
            }
            else
            {

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvScannedTag.Rows.Count > 0)
            {
                DialogResult DRS = MessageBox.Show("តើអ្នកចង់រក្សាទុកទិន្នន័យទាំងនេះមែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DRS == DialogResult.Yes)
                {
                    SqlCommand myCommand = new SqlCommand("SELECT *FROM tbSemiPress WHERE RegDate=(SELECT max(RegDate) FROM tbSemiPress);", cnn.con);
                    cnn.con.Open();
                    myReader = myCommand.ExecuteReader();
                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            lastSysNo = (myReader["SysNo"].ToString());
                        }
                        nextLabelNo = Convert.ToInt32(lastSysNo);
                        nextLabelNo = nextLabelNo + 1;
                    }
                    else
                    {
                        nextLabelNo = 1;
                    }
                    cnn.con.Close();

                    for (int i = 0; i < dgvScannedTag.Rows.Count; i++)
                    {
                        try
                        {
                            cnn.con.Open();
                            try
                            {
                                cmd = new SqlCommand("INSERT INTO tbSemiPress (PosNo, WipCode, WipDes, BoxNo, Qty, Remarks, RegDate, RegBy, UpdateDate, UpdateBy, SysNo, DeleteState) VALUES (@Pn, @wc, @wd, @bn,@qty, @rm, @Rd, @Rb, @Ud, @Ub, @Sn, @DS)", cnn.con);
                                cmd.Parameters.AddWithValue("@Pn", dgvScannedTag.Rows[i].Cells[0].Value.ToString());
                                cmd.Parameters.AddWithValue("@wc", dgvScannedTag.Rows[i].Cells[1].Value.ToString());
                                cmd.Parameters.AddWithValue("@wd", dgvScannedTag.Rows[i].Cells[2].Value.ToString());
                                cmd.Parameters.AddWithValue("@bn", dgvScannedTag.Rows[i].Cells[3].Value.ToString());
                                cmd.Parameters.AddWithValue("@qty", dgvScannedTag.Rows[i].Cells[4].Value.ToString());
                                cmd.Parameters.AddWithValue("@rm", dgvScannedTag.Rows[i].Cells[5].Value.ToString());
                                cmd.Parameters.AddWithValue("@Rd", DateTime.Now);
                                cmd.Parameters.AddWithValue("@Rb", LbLoginID.Text);
                                cmd.Parameters.AddWithValue("@Ud", DateTime.Now);
                                cmd.Parameters.AddWithValue("@Ub", LbLoginID.Text);
                                cmd.Parameters.AddWithValue("@Sn", nextLabelNo.ToString());
                                cmd.Parameters.AddWithValue("@DS", "0");
                                cmd.ExecuteNonQuery();

                            }
                            catch
                            {

                            }

                        }
                        catch
                        {
                            MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        cnn.con.Close();
                        nextLabelNo = nextLabelNo + 1;
                    }
                    MessageBox.Show("រក្សាទុកបានជោគជ័យ​ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvScannedTag.Rows.Clear();
                    txtBarcode.Focus();
                }
                else
                {

                }
            }
            else
            {

            }
        }
    }
}
