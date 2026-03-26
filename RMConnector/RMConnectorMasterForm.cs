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
    public partial class RMConnectorMasterForm : Form
    {
        SQLConnect cnn = new SQLConnect();

        public RMConnectorMasterForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.btnSearch.Click += BtnSearch_Click;
            this.btnAdd.Click += BtnAdd_Click;
            this.btnEdit.Click += BtnEdit_Click;
            this.btnDelete.Click += BtnDelete_Click;
          //  this.dgvRM.CellClick += DgvRM_CellClick;
          //  btnEdit.Enabled = false;
        }

        //private void DgvRM_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    CheckBtnEdit();
        //}

        //private void CheckBtnEdit()
        //{
        //    bool hasSelection = dgvRM.SelectedRows.Count > 0;
        //    btnEdit.Enabled = hasSelection;
        //    btnEdit.BackColor = hasSelection ? System.Drawing.Color.White : System.Drawing.SystemColors.ButtonHighlight;
        //}

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvRM.CurrentCell == null) return;

            int rowIndex = dgvRM.CurrentCell.RowIndex;
            string rmCode = dgvRM.Rows[rowIndex].Cells["Code"].Value.ToString();

            DialogResult dlr = MessageBox.Show("Do you want to delete \"" + rmCode + "\"?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr != DialogResult.Yes) return;

            try
            {
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM tbMstRMConnector WHERE RMCode = @RMCode", cnn.con);
                cmd.Parameters.AddWithValue("@RMCode", rmCode);
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

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            RMConnectorAddForm addForm = new RMConnectorAddForm(dgvRM);
            addForm.ShowDialog(this);
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvRM.CurrentCell == null) return;
            RMConnectorEditForm editForm = new RMConnectorEditForm(dgvRM);
            editForm.ShowDialog(this);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            dgvRM.Rows.Clear();

            DataTable dtCond = new DataTable();
            dtCond.Columns.Add("Col");
            dtCond.Columns.Add("Value");

            if (tbCode.Text.Trim() != "")
                dtCond.Rows.Add("RMCode LIKE ", "'%" + tbCode.Text.Trim() + "%' ");
            if (tbDescription.Text.Trim() != "")
                dtCond.Rows.Add("RMDescription LIKE ", "'%" + tbDescription.Text.Trim() + "%' ");

            string sqlWhere = "";
            foreach (DataRow row in dtCond.Rows)
            {
                if (sqlWhere.Trim() == "")
                    sqlWhere = "WHERE " + row[0] + row[1];
                else
                    sqlWhere += "AND " + row[0] + row[1];
            }

            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(
                    "SELECT RMCode, RMDescription, Type, Location, RegBy, RegDate, UpdateBy, UpdateDate FROM tbMstRMConnector " + sqlWhere, cnn.con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                int no = 1;
                foreach (DataRow row in dt.Rows)
                {
                    dgvRM.Rows.Add(no++, row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7]);
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
