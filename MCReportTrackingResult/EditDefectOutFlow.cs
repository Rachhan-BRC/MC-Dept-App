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
    public partial class EditDefectOutFlow : Form
    {
        SQLConnect cnn = new SQLConnect();
        private readonly string _category;
        private readonly string _type;
        private readonly DateTime _monthYear;
        public EditDefectOutFlow(string category, string type, DateTime monthYear)
        {
            InitializeComponent();

            cnn.Connection();
            _category = category;
            _type = type;
            _monthYear = monthYear;

            lblCategory.Text = "Category : " + category;
            lblType.Text = "Type       : " + type;
            lblMonth.Text = "Month     : " + monthYear.ToString("MMM yyyy");
            btnSave.Click += BtnSave_Click;

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(tbQty.Text.Trim(), out decimal qty))
            {
                MessageBox.Show("Please enter a valid number for Qty.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbQty.Focus();
                return;
            }

            try
            {
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand(
                    "Update tbDefectOutFlow Set Qty = @Qty, UpdateBy = @UpdateBy, UpdateDate = @UpdateDate " +
                    "Where Category = @Category And Type = @Type And MonthYear = @MonthYear", cnn.con);

                cmd.Parameters.AddWithValue("@Category", _category);
                cmd.Parameters.AddWithValue("@Type", _type);
                cmd.Parameters.AddWithValue("@MonthYear", new DateTime(_monthYear.Year, _monthYear.Month, 1));
                cmd.Parameters.AddWithValue("@Qty", qty);
                //cmd.Parameters.AddWithValue("@RegisterBy", "Test");
                //cmd.Parameters.AddWithValue("@RegisterDate", DateTime.Now);
                // cmd.Parameters.AddWithValue("@UpdateBy", MenuFormV2.UserForNextForm);
                cmd.Parameters.AddWithValue("@UpdateBy", MenuFormV2.UserForNextForm);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.ExecuteNonQuery();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (cnn.con.State == System.Data.ConnectionState.Open)
                    cnn.con.Close();
            }
        }
    }
}
