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
    public partial class SemiPressInputByManualForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SqlCommand cmd;
        public static string WipCode = "";
        public static string WipName = "";
        public static string Pin = "";
        public static string Wire = "";
        public static string Length = "";
        string lastSysNo;
        int nextLabelNo;
        SqlDataReader myReader;

        public SemiPressInputByManualForm()
        {
            InitializeComponent();
            cnn.Connection();
            this.FormClosed += SemiPressInputByManualForm_FormClosed;
            this.txtWipNameS.KeyDown += TxtWipNameS_KeyDown;
            this.txtWipNameS.Leave += TxtWipNameS_Leave;
            this.dgvWipCode.CellDoubleClick += DgvWipCode_CellDoubleClick;

        }

        private void DgvWipCode_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            WipCode = "";
            WipName = "";
            Pin = "";
            Wire = "";
            Length = "";
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvWipCode.Rows[e.RowIndex];
                WipCode = row.Cells[0].Value.ToString();
                WipName = row.Cells[1].Value.ToString();
                Pin = row.Cells[2].Value.ToString();
                Wire = row.Cells[3].Value.ToString();
                Length = row.Cells[4].Value.ToString();

                if (WipCode.Trim() != "" && WipName.Trim() != "" && Pin.Trim() != "" && Wire.Trim() != "" && Length.Trim() != "")
                {
                    txtWipNameS.Focus();
                    txtWipNameS.Text = "";
                    txtWipNameS.Focus();
                    SemiPressInputByManualConfirmForm Spibmcf = new SemiPressInputByManualConfirmForm(this);
                    Spibmcf.ShowDialog();

                }
                else
                {

                }
            }
            else
            {

            }
        }

        private void TxtWipNameS_Leave(object sender, EventArgs e)
        {
            if (txtWipNameS.Text.Trim() != "")
            {
                dgvWipCode.Rows.Clear();
                try
                {
                    //Get all Semi
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbMasterItem WHERE ItemName LIKE '%" + txtWipNameS.Text + "%';", cnn.con);
                    DataTable table = new DataTable();
                    da.Fill(table);
                    foreach (DataRow row in table.Rows)
                    {
                        dgvWipCode.Rows.Add(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString());
                    }
                    dgvWipCode.Sort(this.dgvWipCode.Columns[0], ListSortDirection.Ascending);
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                dgvWipCode.ClearSelection();
            }
            else
            {

            }
        }

        private void TxtWipNameS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                groupBox2.Focus();
            }
            else
            {

            }
        }

        private void SemiPressInputByManualForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            MenuForm mf = new MenuForm();
            mf.ShowDialog();
            this.Close();

        }

        private void SemiPressInputByManualForm_Load(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 250;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;
            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.btnSave, "រក្សាទុក");
            toolTip1.SetToolTip(this.btnNew, "ចាប់ផ្ដើមសារថ្មី");
            toolTip1.SetToolTip(this.btnDelete, "លុប");

            LbLoginID.Text = MenuForm.NameForNextForm;

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            dgvWipCode.Rows.Clear();
            dgvInputWip.Rows.Clear();
            txtWipNameS.Text = "";
            txtWipNameS.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvInputWip.SelectedCells.Count > 0)
            {
                DialogResult DLR = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនដែរ ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    int selectedIndex = dgvInputWip.CurrentCell.RowIndex;
                    if (selectedIndex > -1)
                    {
                        dgvInputWip.Rows.RemoveAt(selectedIndex);
                        dgvInputWip.Refresh();

                    }
                    else
                    {

                    }
                    dgvInputWip.ClearSelection();
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
            if (dgvInputWip.Rows.Count > 0)
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

                    for (int i = 0; i < dgvInputWip.Rows.Count; i++)
                    {
                        try
                        {
                            cnn.con.Open();
                            cmd = new SqlCommand("INSERT INTO tbSemiPress (PosNo, WipCode, WipDes, BoxNo, Qty, Remarks, RegDate, RegBy, UpdateDate, UpdateBy, SysNo, DeleteState) VALUES (@Pn, @wc, @wd, @bn,@qty, @rm, @Rd, @Rb, @Ud, @Ub, @Sn, @DS)", cnn.con);
                            cmd.Parameters.AddWithValue("@Pn", dgvInputWip.Rows[i].Cells[0].Value.ToString());
                            cmd.Parameters.AddWithValue("@wc", dgvInputWip.Rows[i].Cells[1].Value.ToString());
                            cmd.Parameters.AddWithValue("@wd", dgvInputWip.Rows[i].Cells[2].Value.ToString());
                            cmd.Parameters.AddWithValue("@bn", dgvInputWip.Rows[i].Cells[3].Value.ToString());
                            cmd.Parameters.AddWithValue("@qty", dgvInputWip.Rows[i].Cells[4].Value.ToString());
                            cmd.Parameters.AddWithValue("@rm", dgvInputWip.Rows[i].Cells[5].Value.ToString());
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
                            MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        cnn.con.Close();
                        nextLabelNo = nextLabelNo + 1;
                    }
                    MessageBox.Show("រក្សាទុកបានជោគជ័យ​ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvInputWip.Rows.Clear();
                    dgvWipCode.Rows.Clear();
                    txtWipNameS.Text = "";
                    txtWipNameS.Focus();
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
