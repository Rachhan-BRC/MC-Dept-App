using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MachineDeptApp.RMConnector
{
    public partial class RMConnectorEditForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        DataGridView dgvMain;

        public RMConnectorEditForm(DataGridView dgv)
        {
            InitializeComponent();
            cnn.Connection();
            dgvMain = dgv;

            this.Load += RMConnectorEditForm_Load;
            this.btnSave.Click += BtnSave_Click;
            this.btnCancel.Click += (s, e) => this.Close();
        }

        private void RMConnectorEditForm_Load(object sender, EventArgs e)
        {
            int rowIndex = dgvMain.CurrentCell.RowIndex;
            tbRMCode.Text        = dgvMain.Rows[rowIndex].Cells["Code"].Value.ToString();
            tbRMDescription.Text = dgvMain.Rows[rowIndex].Cells["Description"].Value.ToString();
            tbType.Text          = dgvMain.Rows[rowIndex].Cells["Type"].Value.ToString();
            tbLocation.Text      = dgvMain.Rows[rowIndex].Cells["Location"].Value.ToString();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (tbRMDescription.Text.Trim() == "" || tbType.Text.Trim() == "")
            {
                MessageBox.Show("Please fill in all fields!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult dlr = MessageBox.Show("Do you want to save changes?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr != DialogResult.Yes) return;

            try
            {
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbMstRMConnector SET " +
                    "RMDescription = @Desc, " +
                    "Type = @Type, " +
                    "Location = @Location, " +
                    "UpdateBy = @UpdateBy, " +
                    "UpdateDate = @UpdateDate " +
                    "WHERE RMCode = @RMCode", cnn.con);

                cmd.Parameters.AddWithValue("@Desc",       tbRMDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@Type",       tbType.Text.Trim());
                cmd.Parameters.AddWithValue("@Location",   tbLocation.Text.Trim());
                cmd.Parameters.AddWithValue("@UpdateBy",   MenuFormV2.UserForNextForm);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@RMCode",     tbRMCode.Text.Trim());
                cmd.ExecuteNonQuery();

                // Update dgv row
                int rowIndex = dgvMain.CurrentCell.RowIndex;
                dgvMain.Rows[rowIndex].Cells["Description"].Value = tbRMDescription.Text.Trim();
                dgvMain.Rows[rowIndex].Cells["Type"].Value        = tbType.Text.Trim();
                dgvMain.Rows[rowIndex].Cells["Location"].Value    = tbLocation.Text.Trim();
                dgvMain.Rows[rowIndex].Cells["UpdateBy"].Value    = MenuFormV2.UserForNextForm;
                dgvMain.Rows[rowIndex].Cells["UpdateDate"].Value  = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                MessageBox.Show("Updated successfully!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update failed!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
        }
    }
}
