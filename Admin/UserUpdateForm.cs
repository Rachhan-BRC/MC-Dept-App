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
    public partial class UserUpdateForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        string User = UserForm.UsernameForNextForm;

        public UserUpdateForm()
        {
            InitializeComponent();
            cnn.Connection();
            this.CboRole.KeyPress += CboRole_KeyPress;
        }

        private void CboRole_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void UserUpdateForm_Load(object sender, EventArgs e)
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
            CboRole.Items.Add("Admin");
            CboRole.Items.Add("User");

            txtID.Text = UserForm.IdForNextForm;
            //add all data to control box
            SqlCommand cmd = new SqlCommand("SELECT * FROM tbUser WHERE ID ='" + txtID.Text + "';", cnn.con);
            try
            {
                cnn.con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtUsername.Text = dr.GetValue(1).ToString();
                    txtPassword.Text = dr.GetValue(3).ToString();
                    CboRole.Text = dr.GetValue(2).ToString();

                }
            }
            catch
            {
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            PicID.Visible = false;
            PicPass.Visible = false;
            PicRole.Visible = false;
            PicUsername.Visible = false;

            if (txtID.Text.Trim() == "" || CboRole.Text.Trim() == "" || txtUsername.Text.Trim() == "" || txtPassword.Text.Trim() == "")
            {
                //4
                if (txtID.Text.Trim() == "" && CboRole.Text.Trim() == "" && txtUsername.Text.Trim() == "" && txtPassword.Text.Trim() == "")
                {
                    PicID.Visible = true;
                    PicRole.Visible = true;
                    PicUsername.Visible = true;
                    PicPass.Visible = true;
                    txtID.Focus();

                }
                //3
                //1,2,3
                else if (txtID.Text.Trim() == "" && CboRole.Text.Trim() == "" && txtUsername.Text.Trim() == "")
                {
                    PicID.Visible = true;
                    PicRole.Visible = true;
                    PicUsername.Visible = true;
                    txtID.Focus();

                }
                //1,2,4
                else if (txtID.Text.Trim() == "" && CboRole.Text.Trim() == "" && txtPassword.Text.Trim() == "")
                {
                    PicID.Visible = true;
                    PicRole.Visible = true;
                    PicPass.Visible = true;
                    txtID.Focus();

                }
                //2,3,4
                else if (CboRole.Text.Trim() == "" && txtUsername.Text.Trim() == "" && txtPassword.Text.Trim() == "")
                {
                    PicUsername.Visible = true;
                    PicRole.Visible = true;
                    PicPass.Visible = true;
                    CboRole.Focus();

                }
                //3,4,1
                else if (txtID.Text.Trim() == "" && txtUsername.Text.Trim() == "" && txtPassword.Text.Trim() == "")
                {
                    PicID.Visible = true;
                    PicRole.Visible = true;
                    PicPass.Visible = true;
                    txtID.Focus();

                }

                //2
                //1,2
                else if (txtID.Text.Trim() == "" && CboRole.Text.Trim() == "")
                {
                    PicID.Visible = true;
                    PicRole.Visible = true;
                    txtID.Focus();

                }
                //1,3
                else if (txtID.Text.Trim() == "" && txtUsername.Text.Trim() == "")
                {
                    PicID.Visible = true;
                    PicUsername.Visible = true;
                    txtID.Focus();

                }
                // 1,4
                else if (txtID.Text.Trim() == "" && txtPassword.Text.Trim() == "")
                {
                    PicID.Visible = true;
                    PicPass.Visible = true;
                    txtID.Focus();

                }
                // 2,3
                else if (CboRole.Text.Trim() == "" && txtUsername.Text.Trim() == "")
                {
                    PicRole.Visible = true;
                    PicUsername.Visible = true;
                    CboRole.Focus();

                }
                // 2,4
                else if (CboRole.Text.Trim() == "" && txtPassword.Text.Trim() == "")
                {
                    PicRole.Visible = true;
                    PicPass.Visible = true;
                    CboRole.Focus();

                }
                // 3,4
                else if (txtUsername.Text.Trim() == "" && txtPassword.Text.Trim() == "")
                {
                    PicUsername.Visible = true;
                    PicPass.Visible = true;
                    txtUsername.Focus();

                }

                //1
                else if (txtID.Text.Trim() == "")
                {
                    PicID.Visible = true;
                    txtID.Focus();

                }
                else if (CboRole.Text.Trim() == "")
                {
                    PicRole.Visible = true;
                    CboRole.Focus();

                }
                else if (txtUsername.Text.Trim() == "")
                {
                    PicUsername.Visible = true;
                    txtUsername.Focus();

                }
                else if (txtPassword.Text.Trim() == "")
                {
                    PicPass.Visible = true;
                    txtPassword.Focus();

                }
                else
                {

                }
            }

            else if (txtID.Text == "Admin")
            {
                MessageBox.Show("Not allow to update Admin account!", "Update", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                DialogResult DLR = MessageBox.Show("Do you want to update for this user account?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {

                    try
                    {
                        cnn.con.Open();
                        string query = "UPDATE tbUser SET " +
                                            "Username=N'" + txtUsername.Text + "'," +
                                            "Password='" + txtPassword.Text + "'," +
                                            "Role='" + CboRole.Text + "'," +
                                            "UpdateDate='" + DateTime.Now + "'," +
                                            "UpdateBy=N'" + User + "'" +
                                            "WHERE ID = '" + txtID.Text +"';";
                        SqlCommand cmd = new SqlCommand(query, cnn.con);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Successfully updated !", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();

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
}
