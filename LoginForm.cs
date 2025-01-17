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

namespace MachineDeptApp
{
    public partial class LoginForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        public static string IDValueForNextForm = "";

        public LoginForm()
        {
            InitializeComponent();
            cnn.Connection();
            this.FormClosing += LoginForm_FormClosing;
            this.PicShow.Click += PicShow_Click;
            this.PicHide.Click += PicHide_Click;

        }

        private void PicHide_Click(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '*';
            PicShow.Visible = true;
            PicHide.Visible = false;
        }
        private void PicShow_Click(object sender, EventArgs e)
        {
            string currenttext = txtPassword.Text;
            txtPassword.PasswordChar = '\0';
            PicShow.Visible = false;
            PicHide.Visible = true;
        }
        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();
        }
        private void LoginForm_Load(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 250;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;
            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.PicHide, "លាក់ពាក្យសម្ងាត់");
            toolTip1.SetToolTip(this.PicShow, "បង្ហាញពាក្យសម្ងាត់");
            toolTip1.SetToolTip(this.btnLogin, "ចូលប្រើប្រាស់");
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtID.Text.Trim() == "" || txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("ការចូលប្រើបរាជ័យ!", "Login Fail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                //try
                //{
                    cnn.con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM tbUser WHERE ID ='" + txtID.Text + "' AND Password='" + txtPassword.Text + "'", cnn.con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        IDValueForNextForm = txtID.Text;
                        this.Hide();
                        MenuFormV2 uaf = new MenuFormV2();
                        uaf.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("មិនមានគណនីនេះនៅក្នុងប្រព័ន្ធទេ!", "Login Fail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show("ការភ្ជាប់បណ្ដាញមានបញ្ហា!\nសូមឆែកមើលការកំណត់របស់កម្មវិធី!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
                cnn.con.Close();
            }
        }
    }
}
