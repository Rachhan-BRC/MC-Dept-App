using MachineDeptApp.Admin;
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
using Microsoft.VisualBasic;
using System.IO;

namespace MachineDeptApp.TransferData
{
    public partial class TransferDataEditForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        public static string IdForNextForm = "";
        public static string UsernameForNextForm = "";
        SqlCommand cmd = new SqlCommand();
        string IdSelected;
        string PosSelected;
        double QtySelected;

        public TransferDataEditForm()
        {
            InitializeComponent();
            cnn.Connection();
            this.dgvAllData.CellClick += DgvAllData_CellClick;
            this.btnExport.Click += BtnExport_Click;

        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvAllData.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("តើអ្នកចង់ទាញទិន្នន័យចេញមែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "CSV file (*.csv)|*.csv";
                    saveDialog.FileName = "WIP_Transfer.csv";
                    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;
                        try
                        {
                            //String array for Csv
                            string[] outputCsv;
                            outputCsv = new string[dgvAllData.Rows.Count + 1];

                            //Write Column name
                            string columnNames = "";
                            //Set Column Name
                            foreach (DataGridViewColumn col in dgvAllData.Columns)
                            {
                                if (col.Visible == true)
                                {
                                    columnNames += col.HeaderText.ToString() + ",";
                                }
                            }
                            outputCsv[0] += columnNames;

                            //Row of data 
                            foreach (DataGridViewRow row in dgvAllData.Rows)
                            {
                                foreach (DataGridViewColumn col in dgvAllData.Columns)
                                {
                                    if (col.Visible == true)
                                    {
                                        string Value = "";
                                        if (row.Cells[col.Index].Value != null)
                                        {
                                            Value = row.Cells[col.Index].Value.ToString();
                                        }
                                        //Fix don't separate if it contain '\n' or ','
                                        Value = "\"" + Value.Replace("\"", "\"\"") + "\"";
                                        outputCsv[row.Index + 1] += Value + ",";
                                    }
                                }
                            }

                            File.WriteAllLines(saveDialog.FileName, outputCsv, Encoding.UTF8);
                            Cursor = Cursors.Default;
                            MessageBox.Show("ទាញទិន្នន័យចេញរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            Cursor = Cursors.Default;
                            MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void DgvAllData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            IdSelected = "";
            PosSelected = "";
            QtySelected = 0;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvAllData.Rows[e.RowIndex];
                IdSelected = row.Cells[10].Value.ToString();
                IdSelected = IdSelected.Trim();
                PosSelected = row.Cells[0].Value.ToString();
                QtySelected = Convert.ToDouble(row.Cells[4].Value.ToString());
            }
        }

        private void TransferDataEditForm_Load(object sender, EventArgs e)
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
            toolTip1.SetToolTip(this.btnDelete, "លុប");

            chkDate.Checked = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            LbStatus.Visible = true;
            IdSelected = "";
            PosSelected = "";
            QtySelected = 0;
            IdForNextForm = "";
            UsernameForNextForm = "";
            dgvAllData.Rows.Clear();
           
            DataTable dtSQLConds = new DataTable();
            dtSQLConds.Columns.Add("Col");
            dtSQLConds.Columns.Add("Value");

            dtSQLConds.Rows.Add("DeleteState = ", "'0' ");
            if (txtWipCode.Text.Trim() != "")
            {
                if (txtWipCode.Text.ToString().Contains("*") == true)
                {
                    string SearchValue = txtWipCode.Text.ToString();
                    SearchValue = SearchValue.Replace("*", "%");
                    dtSQLConds.Rows.Add("WipCode LIKE ","'"+SearchValue+"' ");
                }
                else
                {
                    dtSQLConds.Rows.Add("WipCode = ", "'" + txtWipCode.Text + "' ");
                }
            }
            if (txtWipDes.Text.Trim() != "")
            {
                dtSQLConds.Rows.Add("WipDes LIKE ", "'%" + txtWipDes.Text + "%' ");
            }
            if (txtPOSNo.Text.Trim() != "")
            {
                if (txtPOSNo.Text.ToString().Contains("*") == true)
                {
                    string SearchValue = txtPOSNo.Text.ToString();
                    SearchValue = SearchValue.Replace("*", "%");
                    dtSQLConds.Rows.Add("PosNo LIKE ", "'" + SearchValue + "' ");
                }
                else
                {
                    dtSQLConds.Rows.Add("PosNo = ", "'" + txtPOSNo.Text + "' ");
                }
            }
            if (chkDate.Checked == true)
            {
                dtSQLConds.Rows.Add("RegDate BETWEEN ", "'"+DtInput.Value.ToString("yyyy-MM-dd")+" 00:00:00' AND '"+DtEndInput.Value.ToString("yyyy-MM-dd")+" 23:59:59' ");
            }
            if (txtSysNo.Text.Trim() != "")
            {
                if (txtSysNo.Text.ToString().Contains("*") == true)
                {
                    string SearchValue = txtSysNo.Text.ToString();
                    SearchValue = SearchValue.Replace("*", "%");
                    dtSQLConds.Rows.Add("SysNo LIKE ", "'" + SearchValue + "' ");
                }
                else
                {
                    dtSQLConds.Rows.Add("SysNo = ", "'" + txtSysNo.Text + "' ");
                }
            }

            string SQLCond = "";
            foreach (DataRow row in dtSQLConds.Rows)
            {
                if (SQLCond.Trim() == "")
                {
                    SQLCond = "WHERE " + row[0] + row[1];
                }
                else
                {
                    SQLCond = SQLCond + "AND " + row[0] + row[1];
                }
            }

            try
            {
                cnn.con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions "+SQLCond+" ORDER BY RegDate ASC​;", cnn.con);
                System.Data.DataTable table = new System.Data.DataTable();
                da.Fill(table);
                //to get rows
                foreach (DataRow row in table.Rows)
                {
                    dgvAllData.Rows.Add(row[0], row[1], row[2], Convert.ToDouble(row[3]), Convert.ToDouble(row[4]), row[5], Convert.ToDateTime(row[6]), row[7], Convert.ToDateTime(row[8]), row[9], row[10]);
                }
                Cursor = Cursors.Default;
                LbStatus.Text = "រកឃើញទិន្នន័យ "+ dgvAllData.Rows.Count.ToString("N0");
                LbStatus.Refresh();
                dgvAllData.ClearSelection();
            }
            catch
            {
                Cursor = Cursors.Default;
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (IdSelected != "")
            {
                //check ID first 
                if (LoginForm.IDValueForNextForm == "3913" || LoginForm.IDValueForNextForm == "3132")
                {
                    //Enter the Password
                    string PassDel = Interaction.InputBox("សូមបញ្ចូលពាក្យសម្ងាត់ដើម្បីលុបទិន្នន័យនេះ !", "Rachhan System", "");
                    if (PassDel.Trim() == "3913" || PassDel.Trim() == "123" || PassDel.Trim().ToUpper() == "ADMIN")
                    {
                        DialogResult DLR = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនដែរ ឬទេ ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (DLR == DialogResult.Yes)
                        {
                            try
                            {
                                cnn.con.Open();
                                string query = "UPDATE tbWipTransactions SET " +
                                                    "DeleteState='1'," +
                                                    "UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    "UpdateBy=N'" + MenuFormV2.UserForNextForm + "'" +
                                                    "WHERE SysNo = '" + IdSelected + "';";
                                SqlCommand cmd = new SqlCommand(query, cnn.con);
                                cmd.ExecuteNonQuery();

                                string query2 = "UPDATE tbSDMCAllTransaction SET " +
                                                    "CancelStatus=1 WHERE CancelStatus=0 AND Funct=5 AND LocCode='MC1' AND Remarks = '" + IdSelected + "';";
                                SqlCommand cmd2 = new SqlCommand(query2, cnn.con);
                                cmd2.ExecuteNonQuery();

                                //Update Status of tbPOSDetailofMC
                                using (cmd = new SqlCommand("UpdatePOSDetailofMC_TransStatus", cnn.con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@NewResultPosC", (QtySelected * (-1)));
                                    cmd.Parameters.AddWithValue("@PosC", PosSelected);
                                    cmd.ExecuteNonQuery();
                                }

                                int selectedIndex = dgvAllData.CurrentCell.RowIndex;
                                if (selectedIndex > -1)
                                {
                                    dgvAllData.Rows.RemoveAt(selectedIndex);
                                    dgvAllData.Refresh();
                                }
                                dgvAllData.ClearSelection();
                                IdSelected = "";
                                PosSelected = "";
                                QtySelected = 0;
                                MessageBox.Show("លុបទិន្នន័យរួចរាល់ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                            catch
                            {
                                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញជាមុន ! \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            cnn.con.Close();
                        }
                    }
                    else
                    {
                        if (PassDel.Length == 0)
                        {

                        }
                        else
                        {
                            MessageBox.Show("ពាក្យសម្ងាត់មិនត្រឹមត្រូវទេ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        }
                    }
                }
                else
                {
                    MessageBox.Show("គណនីនេះគ្មានសិទ្ធិក្នុងការលុបទេ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
            }
        }
    }
}
