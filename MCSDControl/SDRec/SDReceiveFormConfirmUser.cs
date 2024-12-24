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

namespace MachineDeptApp.MCSDControl.SDRec
{
    public partial class SDReceiveFormConfirmUser : Form
    {
        SQLConnect cnn = new SQLConnect();
        public SDReceiveFormConfirmUser()
        {
            InitializeComponent();
            cnn.Connection();
            this.btnCancel.Click += BtnCancel_Click;
            this.btnOK.Click += BtnOK_Click;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (txtID.Text.ToString().Trim() != "" && txtPassword.Text.ToString().Trim() != "")
            {
                try
                {
                    cnn.con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM tbUser WHERE Role='Admin' AND ID ='" + txtID.Text + "' AND Password='" + txtPassword.Text + "';", cnn.con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        SDReceiveFormConfirm.ID = txtID.Text;
                        SDReceiveFormConfirm.Password = txtPassword.Text;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("មិនមានគណនី Admin នេះនៅក្នុងប្រព័ន្ធទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Login Fail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                cnn.con.Close();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
