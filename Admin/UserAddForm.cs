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
    public partial class UserAddForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SqlCommand cmd;
        UserForm fgrid;
        string user = UserForm.UsernameForNextForm;

        public UserAddForm(UserForm fg)
        {
            InitializeComponent();
            cnn.Connection();
            this.fgrid = fg;
            this.CboRole.KeyPress += CboRole_KeyPress;
        }

        private void CboRole_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void UserAddForm_Load(object sender, EventArgs e)
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
            else
            {
                DialogResult DLR = MessageBox.Show("Do you want to add this user into the system?", "Add new user", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    try
                    {

                        cnn.con.Open();
                        cmd = new SqlCommand("INSERT INTO tbUser(ID, Username, Password, Role, RegDate, RegBy, UpdateDate, UpdateBy) VALUES (@id, @un, @pw, @rl, @Rd, @Rb, @UD, @Ub)", cnn.con);
                        cmd.Parameters.AddWithValue("@id", txtID.Text);
                        cmd.Parameters.AddWithValue("@un", txtUsername.Text);
                        cmd.Parameters.AddWithValue("@pw", txtPassword.Text);
                        cmd.Parameters.AddWithValue("@rl", CboRole.Text);
                        cmd.Parameters.AddWithValue("@Rd", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Rb", user);
                        cmd.Parameters.AddWithValue("@UD", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Ub", user);

                        try
                        {

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully added new users !", "Add new user", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            fgrid.dgvUser.Rows.Add(txtID.Text, txtUsername.Text, txtPassword.Text, CboRole.Text, DateTime.Now, user, DateTime.Now, user);
                            txtID.Text = "";
                            CboRole.Text = "";
                            txtUsername.Text = "";
                            txtPassword.Text = "";
                            this.Close();
                        }
                        catch
                        {
                            MessageBox.Show("This ID already exists, please recheck it !", "Add new user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
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
