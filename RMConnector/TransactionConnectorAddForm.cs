using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MachineDeptApp.RMConnector
{
    public partial class TransactionConnectorAddForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        DataGridView dgvMain;
        string _type = "";
        string _location = "";

        public TransactionConnectorAddForm(DataGridView dgv)
        {
            InitializeComponent();
            cnn.Connection();
            dgvMain = dgv;

            this.btnRMSearch.Click += BtnRMSearch_Click;
            this.dgvRMSearch.CellClick += DgvRMSearch_CellClick;
            this.btnSave.Click += BtnSave_Click;
            this.btnCancel.Click += (s, e) => this.Close();
        }

        private void BtnRMSearch_Click(object sender, EventArgs e)
        {
            dgvRMSearch.Rows.Clear();

            string where = "WHERE 1=1";
            if (tbSearchCode.Text.Trim() != "")
                where += " AND RMCode LIKE '%" + tbSearchCode.Text.Trim() + "%'";
            if (tbSearchDesc.Text.Trim() != "")
                where += " AND RMDescription LIKE '%" + tbSearchDesc.Text.Trim() + "%'";

            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(
                    "SELECT RMCode, RMDescription, Type, Location FROM tbMstRMConnector " + where, cnn.con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                    dgvRMSearch.Rows.Add(row[0], row[1], row[2], row[3]);
                dgvRMSearch.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
        }

        private void DgvRMSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            tbItemCode.Text = dgvRMSearch.Rows[e.RowIndex].Cells["colRMCode"].Value.ToString();
            tbItemName.Text = dgvRMSearch.Rows[e.RowIndex].Cells["colRMDescription"].Value.ToString();
            _type           = dgvRMSearch.Rows[e.RowIndex].Cells["colType"].Value.ToString();
            _location       = dgvRMSearch.Rows[e.RowIndex].Cells["colLocation"].Value.ToString();

            double bal = GetCurrentBalance(tbItemCode.Text);
            lbBalance.Text      = bal.ToString("N2");
            lbBalance.ForeColor = bal > 0
                ? System.Drawing.Color.DarkGreen
                : System.Drawing.Color.Red;
        }

        private double GetCurrentBalance(string itemCode)
        {
            double balance = 0;
            try
            {
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT ISNULL(SUM(ReceiveQty), 0) - ISNULL(SUM(IssuedQty), 0) " +
                    "FROM tbTransactionConnector WHERE ItemCode = @ItemCode AND Status = '1'", cnn.con);
                cmd.Parameters.AddWithValue("@ItemCode", itemCode);
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    balance = Convert.ToDouble(result);
            }
            catch { }
            cnn.con.Close();
            return balance;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

            if (tbItemCode.Text.Trim() == "" || tbReceiveQty.Text.Trim() == "" ||
                tbIssuedQty.Text.Trim() == "")
            {
                MessageBox.Show("Please select an RM item and fill in all required fields!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!double.TryParse(tbIssuedQty.Text.Trim(), out double issuedQty) || issuedQty < 0)
            {
                MessageBox.Show("Issued Qty must be a valid positive number!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (issuedQty > 0)
            {
                double currentBalance = GetCurrentBalance(tbItemCode.Text.Trim());
                if (issuedQty > currentBalance)
                {
                    MessageBox.Show(
                        "Cannot issue " + issuedQty.ToString("N2") + " unit(s).\n" +
                        "Current balance for \"" + tbItemCode.Text.Trim() + "\" is " + currentBalance.ToString("N2") + " unit(s).",
                        "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            DialogResult dlr = MessageBox.Show("Do you want to add this record?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr != DialogResult.Yes) return;

            try
            {
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO tbTransactionConnector (ItemCode, ReceiveQty, IssuedQty, Remark, RegisterBy, RegisterDate, UpdateBy, UpdateDate, Status) " +
                    "VALUES (@ItemCode, @ReceiveQty, @IssuedQty, @Remark, @RegisterBy, @RegisterDate, @UpdateBy, @UpdateDate, '1')", cnn.con);

                cmd.Parameters.AddWithValue("@ItemCode",     tbItemCode.Text.Trim());
                cmd.Parameters.AddWithValue("@ReceiveQty",   tbReceiveQty.Text.Trim());
                cmd.Parameters.AddWithValue("@IssuedQty",    tbIssuedQty.Text.Trim());
                cmd.Parameters.AddWithValue("@Type",         _type);
                cmd.Parameters.AddWithValue("@Remark",       tbRemark.Text.Trim());
                cmd.Parameters.AddWithValue("@Location",     _location);
                cmd.Parameters.AddWithValue("@RegisterBy",   MenuFormV2.UserForNextForm);
                cmd.Parameters.AddWithValue("@RegisterDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdateBy",     MenuFormV2.UserForNextForm);
                cmd.Parameters.AddWithValue("@UpdateDate",   DateTime.Now);
                cmd.ExecuteNonQuery();

                SqlCommand cmdId = new SqlCommand("SELECT SCOPE_IDENTITY()", cnn.con);
                string newId = cmdId.ExecuteScalar().ToString();

                dgvMain.Rows.Add(
                    newId,
                    tbItemCode.Text.Trim(),
                    tbItemName.Text.Trim(),
                    tbReceiveQty.Text.Trim(),
                    tbIssuedQty.Text.Trim(),
                    _type,
                    tbRemark.Text.Trim(),
                    _location,
                    MenuFormV2.UserForNextForm,
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

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
