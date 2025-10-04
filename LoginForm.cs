using MachineDeptApp.MsgClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp
{
    public partial class LoginForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectUpdate cnnUpdate = new SQLConnectUpdate();
        DataTable dtUpdate;
        ErrorMsgClass EMsg = new ErrorMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        public static string IDValueForNextForm = "";
        public static string NameForNextForm = "";
        public static string RoleForNextForm = "";

        string ErrorText;

        public LoginForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnUpdate.Connection();
            this.Shown += LoginForm_Shown;
            this.FormClosing += LoginForm_FormClosing;
            this.PicShow.Click += PicShow_Click;
            this.PicHide.Click += PicHide_Click;


            this.btnUpdate.MouseEnter += BtnUpdate_MouseEnter;
            this.PicUpdate.MouseEnter += BtnUpdate_MouseEnter;
            this.btnUpdate.MouseLeave += BtnUpdate_MouseLeave;
            this.PicUpdate.MouseLeave += BtnUpdate_MouseLeave;
            this.btnUpdate.Click += BtnUpdate_Click;
            this.PicUpdate.Click += BtnUpdate_Click;

        }

        private async void LoginForm_Shown(object sender, EventArgs e)
        {
            await Task.Delay(100);
            Cursor = Cursors.WaitCursor;
            CheckingForNewUpdate();
            Cursor = Cursors.Default;
            if (dtUpdate.Rows.Count > 0)
            {
                btnUpdate.Visible = true;
                PicUpdate.Visible = true;
                PicUpdate.BringToFront();
                this.Refresh();
            }
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //Open UpdateChecker 
                System.Diagnostics.Process.Start(Environment.CurrentDirectory.ToString() + @"\\UpdateChecker.exe");

                //Close this App
                Application.ExitThread();

            }
            catch (Exception ex)
            {
                WMsg.WarningText = "Something when wrong!\n" + ex.Message;
                WMsg.ShowingMsg();
            }
        }
        private void BtnUpdate_MouseLeave(object sender, EventArgs e)
        {
            int PadNum = PicUpdate.Padding.Top - 2;
            PicUpdate.Padding = new Padding(PadNum, PadNum, PadNum, PadNum);
            this.btnUpdate.Font = new Font(this.btnUpdate.Font.FontFamily.Name, this.btnUpdate.Font.Size + 1, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
            Color NewColor = Color.White;
            btnUpdate.BackColor = NewColor;
            btnUpdate.ForeColor = Color.FromArgb(0, 192, 0);
            PicUpdate.BackColor = NewColor;
        }
        private void BtnUpdate_MouseEnter(object sender, EventArgs e)
        {
            int PadNum = PicUpdate.Padding.Top + 2;
            PicUpdate.Padding = new Padding(PadNum, PadNum, PadNum, PadNum);
            this.btnUpdate.Font = new Font(this.btnUpdate.Font.FontFamily.Name, this.btnUpdate.Font.Size - 1, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
            Color NewColor = Color.LightBlue;
            btnUpdate.BackColor = NewColor;
            btnUpdate.ForeColor = Color.Black;
            PicUpdate.BackColor = NewColor;
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
                        MenuFormV2 uaf = new MenuFormV2(this.dtUpdate);
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


        //Method
        private void CheckingForNewUpdate()
        {
            dtUpdate = new DataTable();
            try
            {
                string ProjectName = Assembly.GetExecutingAssembly().GetName().Name;
                string filePath = Assembly.GetExecutingAssembly().Location;
                DateTime ReleaseDate = File.GetLastWriteTime(filePath);
                cnnUpdate.con.Open();
                string SQLQuery = "SELECT ProjName, ProjVersion, ReleaseDate FROM tbUpdateHistory WHERE ProjName = '" + ProjectName + "' AND ReleaseDate>'" + ReleaseDate.ToString("yyyy-MM-dd HH:mm") + ":00'  ORDER BY ReleaseDate ASC";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnUpdate.con);
                sda.Fill(dtUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            cnnUpdate.con.Close();

        }
    }
}
