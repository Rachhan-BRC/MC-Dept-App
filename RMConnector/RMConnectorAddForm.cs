using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MachineDeptApp.RMConnector
{
    public partial class RMConnectorAddForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        DataGridView dgvMain;

        public RMConnectorAddForm(DataGridView dgv)
        {
            InitializeComponent();
            cnn.Connection();
            dgvMain = dgv;

            this.btnSave.Click += BtnSave_Click;
            this.btnCancel.Click += (s, e) => this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (tbRMCode.Text.Trim() == "" || tbRMDescription.Text.Trim() == "" ||
                tbType.Text.Trim() == "")
            {
                MessageBox.Show("Please fill in all fields!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult dlr = MessageBox.Show("Do you want to add this record?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr != DialogResult.Yes) return;

            try
            {
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO tbMstRMConnector (RMCode, RMDescription, Type, Location, RegBy, RegDate, UpdateBy, UpdateDate) " +
                    "VALUES (@RMCode, @Desc, @Type, @Location, @RegBy, @RegDate, @UpdateBy, @UpdateDate)", cnn.con);

                cmd.Parameters.AddWithValue("@RMCode",     tbRMCode.Text.Trim());
                cmd.Parameters.AddWithValue("@Desc",       tbRMDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@Type",       tbType.Text.Trim());
                cmd.Parameters.AddWithValue("@Location",   tbLocation.Text.Trim());
                cmd.Parameters.AddWithValue("@RegBy",      MenuFormV2.UserForNextForm);
                cmd.Parameters.AddWithValue("@RegDate",    DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdateBy",   MenuFormV2.UserForNextForm);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.ExecuteNonQuery();
                // Add new row to dgv
                int no = dgvMain.Rows.Count + 1;
                dgvMain.Rows.Add(
                    no,
                    tbRMCode.Text.Trim(),
                    tbRMDescription.Text.Trim(),
                    tbType.Text.Trim(),
                    tbLocation.Text.Trim(),
                    MenuFormV2.UserForNextForm,
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    MenuFormV2.UserForNextForm,
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                 );

                MessageBox.Show("Added successfully!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Insert failed!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
        }
    }
}
