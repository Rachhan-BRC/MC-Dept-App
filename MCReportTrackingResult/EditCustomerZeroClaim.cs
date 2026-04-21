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

namespace MachineDeptApp.MCReportTrackingResult
{
    public partial class EditCustomerZeroClaim : Form
    {
        SQLConnect cnn = new SQLConnect();
        DataGridView dgvMain;
        string tbId;
        public EditCustomerZeroClaim(DataGridView dgvMain)
        {
            InitializeComponent();
            cnn.Connection();
            btnSave.Click += BtnSave_Click;
            cbEnd.Checked = true;
            cbStart.Checked = true;
            this.dgvMain = dgvMain;
            this.Load += EditCustomerZeroClaim_Load;
        }

        private void EditCustomerZeroClaim_Load(object sender, EventArgs e)
        {
            int rowIndex = dgvMain.CurrentCell.RowIndex;
            string SysNo = dgvMain.Rows[rowIndex].Cells["SysNo"].Value.ToString();
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("Select * from tbCustomerZeroClaim where SysNo = @SysNo", cnn.con);
                sda.SelectCommand.Parameters.AddWithValue("@SysNo", SysNo);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    tbId = row["SysNo"].ToString();
                    tbCycle.Text = row["Cycle"].ToString();
                    tbCategory.Text = row["Category"].ToString();
                    if (row["StartDate"] != DBNull.Value)
                    {
                        dtpStart.Value = Convert.ToDateTime(row["StartDate"]);
                        cbStart.Checked = true;
                    }
                    if (row["EndDate"] != DBNull.Value)
                    {
                        dtpEnd.Value = Convert.ToDateTime(row["EndDate"]);
                        cbEnd.Checked = true;
                    }
                    comboxStatus.Text = row["Status"].ToString();
                }
            } catch { }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (tbCycle.Text == "" || tbCategory.Text == "")
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }
            try
            {
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand("Update tbCustomerZeroClaim set Cycle = @Cycle, Category = @Category, StartDate = @StartDate, EndDate = @EndDate, Status = @Status, UpdateBy = @UpdateBy, UpdateDate = @UpdateDate, AchieveDay = @AchieveDay where SysNo = @SysNo", cnn.con);
                cmd.Parameters.AddWithValue("@SysNo", tbId);
                cmd.Parameters.AddWithValue("@Cycle", tbCycle.Text);
                cmd.Parameters.AddWithValue("@Category", tbCategory.Text);
                if (cbStart.Checked)
                    cmd.Parameters.AddWithValue("@StartDate", dtpStart.Value);
                else
                    cmd.Parameters.AddWithValue("@StartDate", DBNull.Value);
                if (cbEnd.Checked)
                    cmd.Parameters.AddWithValue("@EndDate", dtpEnd.Value);
                else
                    cmd.Parameters.AddWithValue("@EndDate", DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", comboxStatus.Text);
                cmd.Parameters.AddWithValue("@UpdateBy", MenuFormV2.UserForNextForm);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                int achieveDay = 0;
                if (cbStart.Checked && cbEnd.Checked)
                {
                    achieveDay = (dtpEnd.Value - dtpStart.Value).Days;
                    cmd.Parameters.AddWithValue("@AchieveDay", achieveDay);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@AchieveDay", DBNull.Value);
                }

                    cmd.ExecuteNonQuery();
                MessageBox.Show("Record updated successfully.");
                dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["colCycle"].Value = tbCycle.Text;
                dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["colCategory"].Value = tbCategory.Text;
                dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["colStart"].Value = cbStart.Checked ? dtpStart.Value.ToString("dd-MM-yyyy") : "";
                dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["colEnd"].Value = cbEnd.Checked ? dtpEnd.Value.ToString("dd-MM-yyyy") : "";
                dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["colAchieveDay"].Value = achieveDay > 0 ? achieveDay + "Days" : "";
                dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["colStatus"].Value = comboxStatus.Text;
               
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