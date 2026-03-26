using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MachineDeptApp.RMConnector
{
    public partial class TransactionConnectorEditForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        DataGridView dgvMain;

        public TransactionConnectorEditForm(DataGridView dgv)
        {
            InitializeComponent();
            cnn.Connection();
            dgvMain = dgv;

            this.Load += TransactionConnectorEditForm_Load;
            this.btnSave.Click += BtnSave_Click;
            this.btnCancel.Click += (s, e) => this.Close();
        }

        private void TransactionConnectorEditForm_Load(object sender, EventArgs e)
        {
            int rowIndex = dgvMain.CurrentCell.RowIndex;
            string id = dgvMain.Rows[rowIndex].Cells["Id"].Value.ToString();

            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(
                    "SELECT Id, ItemCode, ReceiveQty, IssuedQty, Remark " +
                    "FROM tbTransactionConnector WHERE Id = @Id", cnn.con);
                sda.SelectCommand.Parameters.AddWithValue("@Id", id);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    tbId.Text         = row["Id"].ToString();
                    tbItemCode.Text   = row["ItemCode"].ToString();
                    tbReceiveQty.Text = row["ReceiveQty"].ToString();
                    tbIssuedQty.Text  = row["IssuedQty"].ToString();
                    tbRemark.Text     = row["Remark"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load failed!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (tbItemCode.Text.Trim() == "" || tbReceiveQty.Text.Trim() == "" ||
                tbIssuedQty.Text.Trim() == "")
            {
                MessageBox.Show("Please fill in all required fields!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult dlr = MessageBox.Show("Do you want to save changes?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr != DialogResult.Yes) return;

            try
            {
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbTransactionConnector SET " +
                    "ItemCode = @ItemCode, " +
                    "ReceiveQty = @ReceiveQty, " +
                    "IssuedQty = @IssuedQty, " +
                    "Remark = @Remark, " +
                    "UpdateBy = @UpdateBy, " +
                    "UpdateDate = @UpdateDate " +
                    "WHERE Id = @Id", cnn.con);

                cmd.Parameters.AddWithValue("@ItemCode",   tbItemCode.Text.Trim());
                cmd.Parameters.AddWithValue("@ReceiveQty", tbReceiveQty.Text.Trim());
                cmd.Parameters.AddWithValue("@IssuedQty",  tbIssuedQty.Text.Trim());
                cmd.Parameters.AddWithValue("@Remark",     tbRemark.Text.Trim());
                cmd.Parameters.AddWithValue("@UpdateBy",   MenuFormV2.UserForNextForm);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@Id",         tbId.Text.Trim());
                cmd.ExecuteNonQuery();

                int rowIndex = dgvMain.CurrentCell.RowIndex;
                dgvMain.Rows[rowIndex].Cells["ItemCode"].Value   = tbItemCode.Text.Trim();
                dgvMain.Rows[rowIndex].Cells["ReceiveQty"].Value = tbReceiveQty.Text.Trim();
                dgvMain.Rows[rowIndex].Cells["IssuedQty"].Value  = tbIssuedQty.Text.Trim();
                dgvMain.Rows[rowIndex].Cells["Remark"].Value     = tbRemark.Text.Trim();

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
