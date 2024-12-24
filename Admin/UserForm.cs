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

namespace MachineDeptApp.Admin
{
    public partial class UserForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        public static string IdForNextForm = "";
        public static string UsernameForNextForm = "";
        SqlCommand cmd = new SqlCommand();
        string IdSelected;


        public UserForm()
        {
            InitializeComponent();
            cnn.Connection();
            this.dgvUser.CellClick += DgvUser_CellClick;
        }

        private void DgvUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            IdSelected = "";
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvUser.Rows[e.RowIndex];
                IdSelected = row.Cells[0].Value.ToString();
                IdSelected = IdSelected.Trim();

            }
            else
            {

            }
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 250;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;
            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.btnSearch, "ស្វែងរក");
            toolTip1.SetToolTip(this.btnNew, "បន្ថែមគណនីអ្នកប្រើប្រាស់");
            toolTip1.SetToolTip(this.btnUpdate, "អាប់ដេត");
            toolTip1.SetToolTip(this.btnDelete, "លុប");
                        
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            IdSelected = "";
            IdForNextForm = "";
            UsernameForNextForm = "";
            dgvUser.Rows.Clear();
            if (txtID.Text.Trim() != "")
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbUser WHERE ID = '" + txtID.Text + "' ORDER BY RegDate ASC​;", cnn.con);
                    DataTable table = new DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvUser.Rows.Add(row[0], row[1], row[3], row[2], row[4], row[5], row[6], row[7]);

                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                dgvUser.ClearSelection();
            }
            else
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbUser ORDER BY RegDate ASC​;", cnn.con);
                    DataTable table = new DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvUser.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7]);

                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            dgvUser.ClearSelection();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (IdSelected == "")
            {

            }
            else
            {
                if (IdSelected.Trim() == "Admin")
                {
                    MessageBox.Show("Not allow to delete Admin account !", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    DialogResult DLR = MessageBox.Show("Do you want to delete this user account ?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DLR == DialogResult.Yes)
                    {
                        try
                        {
                            cnn.con.Open();
                            //Delete in SaleHis                     
                            SqlCommand cmd = new SqlCommand("DELETE FROM tbUser WHERE ID ='" + IdSelected + "';", cnn.con);

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Successfully deleted !", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            int selectedIndex = dgvUser.CurrentCell.RowIndex;
                            if (selectedIndex > -1)
                            {
                                dgvUser.Rows.RemoveAt(selectedIndex);
                                dgvUser.Refresh();
                            }
                            dgvUser.ClearSelection();
                            IdSelected = "";

                        }
                        catch
                        {
                            MessageBox.Show("Something wrong ! Please check the connection first !\nOr contact to developer !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        cnn.con.Close();
                    }
                    else
                    {

                    }
                }
            }

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            UsernameForNextForm = MenuFormV2.UserForNextForm;
            UserAddForm uaf = new UserAddForm(this);
            uaf.ShowDialog();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (IdSelected == "")
            {

            }
            else
            {
                IdForNextForm = IdSelected;
                IdSelected = "";
                UsernameForNextForm =  MenuFormV2.UserForNextForm;
                dgvUser.ClearSelection();
                UserUpdateForm UUF = new UserUpdateForm();
                UUF.ShowDialog();
            }
        }
    }
}
