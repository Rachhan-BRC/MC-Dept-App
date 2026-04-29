using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MachineDeptApp.MCReportTrackingResult
{
    public partial class AddCustomerZeroClaim : Form
    {
      
        SQLConnect cnn = new SQLConnect();
        DataGridView dgvMain;
        public AddCustomerZeroClaim(DataGridView view)
        {
            InitializeComponent();
            cnn.Connection();
            btnSave.Click += BtnSave_Click;
            dgvMain = view;
            dtpStart.ValueChanged += DtpStart_ValueChanged;
            dtpEnd.ValueChanged += DtpEnd_ValueChanged;
        }

        

        private void DtpEnd_ValueChanged(object sender, EventArgs e)
        {
            cbEnd.Checked = true;
        }
        private void DtpStart_ValueChanged(object sender, EventArgs e)
        {
            cbStart.Checked = true;
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
                string txtStart = "";
                string txtEnd = "";
                int achieveDay = 0;
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand("Insert into tbCustomerZeroClaim( Cycle, Category, StartDate, EndDate, Status, RegisterBy, RegisterDate, UpdateBy, UpdateDate, AchieveDay ) values" +
                    "( @Cycle, @Category, @StartDate, @EndDate, @Status, @RegisterBy, @RegisterDate, @UpdateBy, @UpdateDate, @AchieveDay) ", cnn.con);
                cmd.Parameters.AddWithValue("@Cycle", tbCycle.Text);
                cmd.Parameters.AddWithValue("@Category", tbCategory.Text);
                if (cbStart.Checked)
                {
                    cmd.Parameters.AddWithValue("@StartDate", dtpStart.Value);
                    txtStart = dtpStart.Value.ToString("dd-MM-yyyy");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@StartDate", DBNull.Value);
                }
                if (cbEnd.Checked)
                {
                    cmd.Parameters.AddWithValue("@EndDate", dtpEnd.Value);
                    txtEnd = dtpEnd.Value.ToString("dd-MM-yyyy");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@EndDate", DBNull.Value);
                }
                if (cbStart.Checked && cbEnd.Checked)
                {
                    achieveDay = (dtpEnd.Value - dtpStart.Value).Days;
                    cmd.Parameters.AddWithValue("@AchieveDay", achieveDay);
                } else
                {
                    cmd.Parameters.AddWithValue("@AchieveDay", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("@Status", comboxStatus.Text);
                cmd.Parameters.AddWithValue("@RegisterBy", MenuFormV2.UserForNextForm);
                cmd.Parameters.AddWithValue("@RegisterDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdateBy", MenuFormV2.UserForNextForm);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.ExecuteNonQuery();
                dgvMain.Rows.Add( "", tbCycle.Text, tbCategory.Text, txtStart, txtEnd, achieveDay > 0 ? achieveDay + "Days": "", comboxStatus.Text );
                tbCategory.Text = "";
                tbCycle.Text = "";
                cbStart.Checked = false;
                cbEnd.Checked = false;
                comboxStatus.Text = "";
            }
            catch (Exception ex) { 
                MessageBox.Show("Error: " + ex.Message);
            }
            cnn.con.Close();
        }
    }
}
