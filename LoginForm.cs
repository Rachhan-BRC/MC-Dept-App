using MachineDeptApp.MsgClass;
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
        ErrorMsgClass EMsg = new ErrorMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        SQLConnect cnn = new SQLConnect();
        public static string IDValueForNextForm = "";
        public static string NameForNextForm = "";
        public static string RoleForNextForm = "";

        string ErrorText;

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
                WMsg.WarningText = "ការចូលប្រើបរាជ័យ!";
                WMsg.ShowingMsg();
            }
            else
            {
                Cursor = Cursors.WaitCursor;
                ErrorText = "";

                //Taking Data
                DataTable dt = new DataTable();                
                try
                {
                    cnn.con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbUser WHERE ID ='" + txtID.Text + "' AND Password='" + txtPassword.Text + "'", cnn.con);
                    sda.Fill(dt);
                }
                catch (Exception ex)
                {
                    ErrorText = "ការភ្ជាប់បណ្ដាញមានបញ្ហា!\nសូមឆែកមើលការកំណត់របស់កម្មវិធី!\n" + ex.Message;
                }
                cnn.con.Close();

                Cursor = Cursors.Default;

                if (ErrorText.Trim() == "")
                {
                    if (dt.Rows.Count>0)
                    {
                        IDValueForNextForm = dt.Rows[0]["ID"].ToString();
                        NameForNextForm = dt.Rows[0]["Username"].ToString();
                        RoleForNextForm = dt.Rows[0]["Role"].ToString();
                        this.Hide();
                        MenuFormV2 uaf = new MenuFormV2();
                        uaf.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        WMsg.WarningText = "មិនមានគណនីនេះនៅក្នុងប្រព័ន្ធទេ!";
                        WMsg.ShowingMsg();
                    }
                }
                else
                {
                    EMsg.AlertText = ErrorText;
                    EMsg.ShowingMsg();
                }
            }
        }
    }
}
