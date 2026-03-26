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

namespace MachineDeptApp.RMConnector
{
    public partial class TransactionConnector : Form
    {
        SQLConnect cnn = new SQLConnect();

        public TransactionConnector()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.btnSearch.Click += BtnSearch_Click;
            this.btnAdd.Click += BtnAdd_Click;
            this.btnEdit.Click += BtnEdit_Click;
            this.btnDelete.Click += BtnDelete_Click;
            this.chkDate.CheckedChanged += ChkDate_CheckedChanged;
        }

        private void ChkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpFrom.Enabled = chkDate.Checked;
            dtpTo.Enabled   = chkDate.Checked;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            TransactionConnectorAddForm addForm = new TransactionConnectorAddForm(dgvRM);
            addForm.ShowDialog(this);
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvRM.CurrentCell == null) return;
            TransactionConnectorEditForm editForm = new TransactionConnectorEditForm(dgvRM);
            editForm.ShowDialog(this);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvRM.CurrentCell == null) return;

            int rowIndex = dgvRM.CurrentCell.RowIndex;
            string id = dgvRM.Rows[rowIndex].Cells["Id"].Value.ToString();

            DialogResult dlr = MessageBox.Show("Do you want to delete record Id \"" + id + "\"?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr != DialogResult.Yes) return;

            try
            {
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbTransactionConnector SET Status = '0' WHERE Id = @Id", cnn.con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();

                dgvRM.Rows.RemoveAt(rowIndex);
                dgvRM.ClearSelection();
                MessageBox.Show("Deleted successfully!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete failed!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            dgvRM.Rows.Clear();

            string extraWhere = "";
            if (tbCode.Text.Trim() != "")
                extraWhere += " AND t.ItemCode LIKE '%" + tbCode.Text.Trim() + "%'";
            if (tbDescription.Text.Trim() != "")
                extraWhere += " AND r.RMDescription LIKE '%" + tbDescription.Text.Trim() + "%'";
            if (chkDate.Checked)
                extraWhere += " AND CAST(t.RegisterDate AS DATE) BETWEEN @DateFrom AND @DateTo";

            string sql =
                "SELECT t.Id, t.ItemCode, r.RMDescription, t.ReceiveQty, t.IssuedQty, " +
                "r.Type, t.Remark, r.Location, t.RegisterBy, t.RegisterDate, " +
                "t.UpdateBy, t.UpdateDate, t.Status " +
                "FROM tbTransactionConnector t " +
                "INNER JOIN tbMstRMConnector r ON t.ItemCode = r.RMCode " +
                "WHERE t.Status = '1'" + extraWhere;

            try
            {
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand(sql, cnn.con);
                if (chkDate.Checked)
                {
                    cmd.Parameters.AddWithValue("@DateFrom", dtpFrom.Value.Date);
                    cmd.Parameters.AddWithValue("@DateTo",   dtpTo.Value.Date);
                }
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    dgvRM.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9]);
                }
                dgvRM.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
            Cursor = Cursors.Default;
        }
    }
}
