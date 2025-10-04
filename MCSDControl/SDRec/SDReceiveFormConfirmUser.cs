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
            this.Load += SDReceiveFormConfirmUser_Load;
            this.btnCancel.Click += BtnCancel_Click;
            this.btnOK.Click += BtnOK_Click;
        }

        private void SDReceiveFormConfirmUser_Load(object sender, EventArgs e)
        {
            this.ID = "";
            this.Password = "";
        }
        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (txtID.Text.ToString().Trim() != "" && txtPassword.Text.ToString().Trim() != "")
            {
                try
                {
                    cnn.con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbUser WHERE ID ='" + txtID.Text + "' AND Password='" + txtPassword.Text + "';", cnn.con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count>0)
                    {
                        if (dt.Rows[0]["Role"].ToString() == "Admin")
                        {
                            this.ID = txtID.Text;
                            this.Password = txtPassword.Text;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("គណនីនេះមិនមានសិទ្ធិបន្តទេ!" +
                                                            "\nសូមបញ្ចូលគណនី Admin តែប៉ុណ្នោះ!" +
                                                            "\n • ឈ្មោះគណនី ៖  " + dt.Rows[0]["Username"].ToString() +
                                                            "\n • តួនាទី          ៖  " + dt.Rows[0]["Role"].ToString(), MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                        MessageBox.Show("គ្មានគណនីនេះនៅក្នុងប្រព័ន្ធទេ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហា! \n" + ex.Message, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                cnn.con.Close();
            }
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Method
        public string ID { get; private set; }
        public string Password { get; private set; }
    }
}
