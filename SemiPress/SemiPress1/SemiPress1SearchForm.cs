using MachineDeptApp.InputTransferSemi;
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

namespace MachineDeptApp.SemiPress.SemiPress1
{
    public partial class SemiPress1SearchForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        public static string IdForNextForm = "";
        public static string UsernameForNextForm = "";
        SqlCommand cmd = new SqlCommand();
        string IdSelected;

        public SemiPress1SearchForm()
        {
            InitializeComponent();
            cnn.Connection();
            this.dgvAllData.CellClick += DgvAllData_CellClick;
        }

        private void DgvAllData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            IdSelected = "";
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvAllData.Rows[e.RowIndex];
                IdSelected = row.Cells[10].Value.ToString();
                IdSelected = IdSelected.Trim();

            }
            else
            {

            }
        }

        private void SemiPress1SearchForm_Load(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 250;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;
            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.btnSearch, "ស្វែងរក");
            toolTip1.SetToolTip(this.btnUpdate, "អាប់ដេត");
            toolTip1.SetToolTip(this.btnDelete, "លុប");

            chkDate.Checked = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (IdSelected == "" || IdSelected == null)
            {

            }
            else
            {
                DialogResult DLR = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនដែរ ឬទេ ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    try
                    {
                        cnn.con.Open();
                        string query = "UPDATE tbSemiPress SET " +
                                            "DeleteState='1'," +
                                            "UpdateDate='" + DateTime.Now + "'," +
                                            "UpdateBy=N'" + MenuFormV2.UserForNextForm + "'" +
                                            "WHERE SysNo = '" + IdSelected + "';";
                        SqlCommand cmd = new SqlCommand(query, cnn.con);
                        cmd.ExecuteNonQuery();
                        int selectedIndex = dgvAllData.CurrentCell.RowIndex;
                        if (selectedIndex > -1)
                        {
                            dgvAllData.Rows.RemoveAt(selectedIndex);
                            dgvAllData.Refresh();
                        }
                        dgvAllData.ClearSelection();
                        IdSelected = "";
                        MessageBox.Show("លុបទិន្នន័យរួចរាល់ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch
                    {
                        MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញជាមុន ! \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cnn.con.Close();
                }
                else
                {

                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            LbStatus.Visible = true;
            IdSelected = "";
            IdForNextForm = "";
            UsernameForNextForm = "";
            dgvAllData.Rows.Clear();

            DataTable dtSQLCond= new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Value");

            dtSQLCond.Rows.Add("DeleteState = ", "'0' ");
            if (txtPOSNo.Text.Trim() != "")
            {
                if (txtPOSNo.Text.Contains("*") == true)
                {
                    string SearchValue = txtPOSNo.Text.ToString();
                    SearchValue = SearchValue.Replace("*","%");
                    dtSQLCond.Rows.Add("PosNo LIKE ", "'"+SearchValue+"' ");
                }
                else
                {
                    dtSQLCond.Rows.Add("PosNo = ", "'" + txtPOSNo.Text + "' ");
                }
            }
            if (txtWipDes.Text.Trim() != "")
            {
                if (txtWipDes.Text.Contains("*") == true)
                {
                    string SearchValue = txtWipDes.Text.ToString();
                    SearchValue = SearchValue.Replace("*", "%");
                    dtSQLCond.Rows.Add("WipDes LIKE ", "'" + SearchValue + "' ");
                }
                else
                {
                    dtSQLCond.Rows.Add("WipDes = ", "'" + txtWipDes.Text + "' ");
                }
            }
            if (txtWIPCode.Text.Trim() != "")
            {
                if (txtWIPCode.Text.Contains("*") == true)
                {
                    string SearchValue = txtWIPCode.Text.ToString();
                    SearchValue = SearchValue.Replace("*", "%");
                    dtSQLCond.Rows.Add("WipCode LIKE ", "'" + SearchValue + "' ");
                }
                else
                {
                    dtSQLCond.Rows.Add("WipCode = ", "'" + txtWIPCode.Text + "' ");
                }
            }
            if (txtRemark.Text.Trim() != "")
            {
                if (txtRemark.Text.Contains("*") == true)
                {
                    string SearchValue = txtRemark.Text.ToString();
                    SearchValue = SearchValue.Replace("*", "%");
                    dtSQLCond.Rows.Add("Remarks LIKE ", "'" + SearchValue + "' ");
                }
                else
                {
                    dtSQLCond.Rows.Add("Remarks = ", "'" + txtRemark.Text + "' ");
                }
            }
            if (chkDate.Checked == true)
            {
                dtSQLCond.Rows.Add("RegDate BETWEEN ", "'"+ DtInput.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '"+DtEndInput.Value.ToString("yyyy-MM-dd") +" 23:59:59' ");
            }

            string SQLConds = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                if (SQLConds.Trim() == "")
                {
                    SQLConds = "WHERE " + row[0] + row[1];
                }
                else
                {
                    SQLConds = SQLConds + "AND " + row[0] + row[1];
                }
            }

            try
            {
                cnn.con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbSemiPress "+SQLConds+"ORDER BY RegDate ASC​;", cnn.con);
                DataTable table = new DataTable();
                da.Fill(table);
                //to get rows
                foreach (DataRow row in table.Rows)
                {
                    dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], Convert.ToDouble(row[4].ToString()), row[5], Convert.ToDateTime(row[6].ToString()), row[7], Convert.ToDateTime(row[8].ToString()), row[9], row[10]);
                }

                LbStatus.Text = "ស្វែងរកឃើញទិន្នន័យ "+dgvAllData.Rows.Count.ToString("N0");
                LbStatus.Refresh();
                Cursor = Cursors.Default;
                dgvAllData.ClearSelection();

            }
            catch
            {
                Cursor = Cursors.Default;
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
        }
    }
}
