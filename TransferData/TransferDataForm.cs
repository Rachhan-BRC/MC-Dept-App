using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Windows.Forms.VisualStyles;
using DataTable = System.Data.DataTable;
using System.Diagnostics;

namespace MachineDeptApp.TransferData
{
    public partial class TransferDataForm : Form
    {
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SQLConnect cnn = new SQLConnect();
        string fName = "";
        DataTable tubeCut;

        public TransferDataForm()
        {
            InitializeComponent();
            cnn.Connection();
            cnnOBS.Connection();
            CboStatus.KeyPress += CboStatus_KeyPress;
            
        }

        private void CboStatus_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dgvInventoryData.Rows.Clear();
            dgvAllData.Rows.Clear();
            LbTotalResult.Visible = false;
            //3 Conditions

            if (txtWipDes.Text.Trim() != "" && CboStatus.Text == "មិនទាន់លុប" && chkDate.Checked == true)
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE WipDes LIKE '%" + txtWipDes.Text.ToString() + "%' AND DeleteState = '0' AND RegDate BETWEEN '" + DtInput.Value.ToString("yyyy-MM-dd")+(" 00:00:00") +"' AND '" + DtEndInput.Value.ToString("yyyy-MM-dd") + (" 23:59:59") + "' ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (txtWipDes.Text.Trim() != "" && CboStatus.Text == "លុប" && chkDate.Checked == true)
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE WipDes LIKE '%" + txtWipDes.Text.ToString() + "%' AND DeleteState = '1' AND RegDate BETWEEN '" + DtInput.Value.ToString("yyyy-MM-dd") + (" 00:00:00") + "' AND '" + DtEndInput.Value.ToString("yyyy-MM-dd") + (" 23:59:59") + "' ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }                
            }
            else if (txtWipDes.Text.Trim() != "" && CboStatus.Text == "ទាំងអស់" && chkDate.Checked == true)
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE WipDes LIKE '%" + txtWipDes.Text.ToString() + "%' AND RegDate BETWEEN '" + DtInput.Value.ToString("yyyy-MM-dd") + (" 00:00:00") + "' AND '" + DtEndInput.Value.ToString("yyyy-MM-dd") + (" 23:59:59") + "' ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (txtWipDes.Text.Trim() != "" && CboStatus.Text.Trim() == "" && chkDate.Checked == true)
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE WipDes LIKE '%" + txtWipDes.Text.ToString() + "%' AND RegDate BETWEEN '" + DtInput.Value.ToString("yyyy-MM-dd") + (" 00:00:00") + "' AND '" + DtEndInput.Value.ToString("yyyy-MM-dd") + (" 23:59:59") + "' ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //2 Conditions
            else if (txtWipDes.Text.Trim() != "" && CboStatus.Text == "មិនទាន់លុប")
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE WipDes LIKE '%" + txtWipDes.Text.ToString() + "%' AND DeleteState = '0' ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (txtWipDes.Text.Trim() != "" && CboStatus.Text == "លុប")
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE WipDes LIKE '%" + txtWipDes.Text.ToString() + "%' AND DeleteState = '1' ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (txtWipDes.Text.Trim() != "" && CboStatus.Text == "ទាំងអស់")
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE WipDes LIKE '%" + txtWipDes.Text.ToString() + "%'​​ ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (txtWipDes.Text.Trim() != "" && CboStatus.Text.Trim() == "")
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE WipDes LIKE '%" + txtWipDes.Text.ToString() + "%'​​ ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else if (txtWipDes.Text.Trim() != "" && chkDate.Checked == true)
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE WipDes LIKE '%" + txtWipDes.Text.ToString() + "%' AND RegDate BETWEEN '" + DtInput.Value.ToString("yyyy-MM-dd") + (" 00:00:00") + "' AND '" + DtEndInput.Value.ToString("yyyy-MM-dd") + (" 23:59:59") + "' ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else if (CboStatus.Text == "មិនទាន់លុប" && chkDate.Checked == true)
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE DeleteState = '0' AND RegDate BETWEEN '" + DtInput.Value.ToString("yyyy-MM-dd") + (" 00:00:00") + "' AND '" + DtEndInput.Value.ToString("yyyy-MM-dd") + (" 23:59:59") + "' ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (CboStatus.Text == "លុប" && chkDate.Checked == true)
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE DeleteState = '1' AND RegDate BETWEEN '" + DtInput.Value.ToString("yyyy-MM-dd") + (" 00:00:00") + "' AND '" + DtEndInput.Value.ToString("yyyy-MM-dd") + (" 23:59:59") + "' ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (CboStatus.Text == "ទាំងអស់" && chkDate.Checked == true)
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE RegDate BETWEEN '" + DtInput.Value.ToString("yyyy-MM-dd") + (" 00:00:00") + "' AND '" + DtEndInput.Value.ToString("yyyy-MM-dd") + (" 23:59:59") + "' ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (CboStatus.Text.Trim() == "" && chkDate.Checked == true)
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE RegDate BETWEEN '" + DtInput.Value.ToString("yyyy-MM-dd") + (" 00:00:00") + "' AND '" + DtEndInput.Value.ToString("yyyy-MM-dd") + (" 23:59:59") + "' ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
            //1 Condition
            else if (txtWipDes.Text.Trim() != "")
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE WipDes LIKE '%" + txtWipDes.Text.ToString() + "%' ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else if (CboStatus.Text == "មិនទាន់លុប")
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE DeleteState = '0' ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (CboStatus.Text == "លុប")
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE DeleteState = '1' ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (CboStatus.Text == "ទាំងអស់")
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (CboStatus.Text.Trim() == "")
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else if (chkDate.Checked == true)
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE RegDate BETWEEN '" + DtInput.Value.ToString("yyyy-MM-dd") + (" 00:00:00") + "' AND '" + DtEndInput.Value.ToString("yyyy-MM-dd") + (" 23:59:59") + "' ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //No Condition
            else
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbWipTransactions ORDER BY RegDate ASC​;", cnn.con);
                    System.Data.DataTable table = new System.Data.DataTable();
                    da.Fill(table);
                    //to get rows
                    foreach (DataRow row in table.Rows)
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], null, row[11]);
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //add check for delete and convert Qty as number value
            if (dgvAllData.Rows.Count > 0)
            {
                for (int i = 0; i < dgvAllData.Rows.Count; i++)
                {
                    if (dgvAllData.Rows[i].Cells[11].Value.ToString() == "0")
                    {
                        dgvAllData.Rows[i].Cells[10].Value = false;
                    }
                    else
                    {
                        dgvAllData.Rows[i].Cells[10].Value = true;
                        dgvAllData.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
                        
                    }
                }
                for (int i = 0; i < dgvAllData.Rows.Count; i++)
                {
                    long Qty = 0;
                    Qty = Convert.ToInt64(dgvAllData.Rows[i].Cells[4].Value.ToString());
                    dgvAllData.Rows[i].Cells[4].Value = Qty;
                }


                try
                {
                    //add to all total dgv
                    //add to datatable
                    //Creating DataTable.
                    System.Data.DataTable dtTotal = new System.Data.DataTable();
                    
                    //Adding the Columns.
                    for (int i = 0; i < 5; i++)
                    {
                        dtTotal.Columns.Add(dgvAllData.Columns[i].HeaderText);
                    }
                    //Adding the Rows.
                    for (int i = 0; i < dgvAllData.Rows.Count; i++)
                    {
                        if (dgvAllData.Rows[i].Cells[11].Value.ToString() != "1")
                        {
                            dtTotal.Rows.Add(dgvAllData.Rows[i].Cells[0].Value, dgvAllData.Rows[i].Cells[1].Value, dgvAllData.Rows[i].Cells[2].Value, dgvAllData.Rows[i].Cells[3].Value, dgvAllData.Rows[i].Cells[4].Value);

                        }
                        else
                        {

                        }
                    }
                    dtTotal = dtTotal.AsEnumerable()
                       .GroupBy(r => new { Col1 = r["កូដ"], Col2 = r["ឈ្មោះ"] })
                       .Select(g => g.OrderBy(r => r["កូដ"]).First())
                       .CopyToDataTable();

                    foreach (DataRow row in dtTotal.Rows)
                    {
                        dgvInventoryData.Rows.Add(row[1].ToString(), row[2].ToString());
                    }

                }
                catch
                {
                    
                }
                //Sum Total Qty for Total Dgv
                for (int i = 0; i < dgvInventoryData.Rows.Count; i++)
                {
                    long SumQty = 0;
                    for (int j = 0; j < dgvAllData.Rows.Count; j++)
                    {
                        if (dgvInventoryData.Rows[i].Cells[0].Value.ToString() == dgvAllData.Rows[j].Cells[1].Value.ToString() && dgvAllData.Rows[j].Cells[11].Value.ToString()!="1")
                        {
                            SumQty = SumQty + Convert.ToInt64(dgvAllData.Rows[j].Cells[4].Value.ToString());
                        }
                    }
                    dgvInventoryData.Rows[i].Cells[2].Value = SumQty;
                    SumQty = 0;
                }


                //Add tube Remark
                if (dgvAllData.Rows.Count > 0)
                {
                    for (int j = 0; j < dgvAllData.Rows.Count; j++)
                    {
                        for (int i = 0; i < tubeCut.Rows.Count; i++)
                        {
                            if (dgvAllData.Rows[j].Cells[1].Value.ToString() == tubeCut.Rows[i][0].ToString())
                            {
                                dgvAllData.Rows[j].Cells[5].Value = tubeCut.Rows[i][44].ToString();
                            }
                        }
                    }
                    
                }

            }
            dgvAllData.Columns[11].Visible = false;

            if (dgvInventoryData.Rows.Count > 0)
            {
                long Sum = 0;
                for (int i = 0; i < dgvInventoryData.Rows.Count; i++)
                {
                    Sum += Convert.ToInt64(dgvInventoryData.Rows[i].Cells[2].Value);

                }
                int lastrowofDgv = dgvInventoryData.RowCount;

                dgvInventoryData.Rows.Insert(lastrowofDgv, "", "សរុប​ ", Sum);
                dgvInventoryData.Rows[lastrowofDgv].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvInventoryData.Rows[lastrowofDgv].DefaultCellStyle.Font= new System.Drawing.Font("Khmer OS Battambang",11, FontStyle.Bold);

            }

            if (dgvInventoryData.Rows.Count > 0)
            {
                LbTotalResult.Text = "   " + ((dgvInventoryData.Rows.Count) - 1).ToString() + "   ផលិតផលរកឃើញ";
                LbTotalResult.Visible = true;
            }
            else
            {
                LbTotalResult.Text = "   0   ផលិតផលរកឃើញ";
                LbTotalResult.Visible = true;
            }


            
        }

        private void TransferDataForm_Load(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 250;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;
            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.btnSearch, "រក្សាទុក");
            toolTip1.SetToolTip(this.btnReprint, "ព្រីនចេញជា Excel");

            CboStatus.Items.Add("មិនទាន់លុប");
            CboStatus.Items.Add("លុប");
            CboStatus.Items.Add("ទាំងអស់");
            chkDate.Checked = true;
            DtInput.Value = DateTime.Now;

            CboStatus.SelectedIndex = 0;

            tubeCut = new DataTable();            
            try
            {
                cnnOBS.conOBS.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM mstitem Where Remark2='TUBE=>CUT';", cnnOBS.conOBS);
                da.Fill(tubeCut);
                
            }
            catch
            {
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnnOBS.conOBS.Close();

        }

        private void btnReprint_Click(object sender, EventArgs e)
        {
            if (dgvInventoryData.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("តើអ្នកចង់​ព្រីនមែន​ ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    LbExportStatus.Visible = true;
                    LbExportStatus.Text = "កំពុងបង្កើត File​ . . . .";
                    int row = dgvInventoryData.Rows.Count;
                    int RowFormodify = row + 5;

                    var CDirectory = Environment.CurrentDirectory;
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\WIP_Transaction.xlsx", Editable: true);
                    Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Total"];
                    //add to All Data
                    try
                    {
                        if (dgvInventoryData.Rows.Count == 1)
                        {
                            worksheet.Cells[3, 2] = DateTime.Now;

                            for (int i = 0; i < dgvInventoryData.Rows.Count; i++)
                            {
                                for (int j = 0; j < dgvInventoryData.Columns.Count; j++)
                                {
                                    if (dgvInventoryData.Rows[i].Cells[j].Value != null)
                                    {
                                        worksheet.Cells[i + 6, j + 1] = dgvInventoryData.Rows[i].Cells[j].Value.ToString();

                                    }
                                    else
                                    {
                                        //worksheet1.Cells[i + 5, j + 1] = "";
                                    }
                                }
                            }

                            Excel.Worksheet worksheetAllData = (Excel.Worksheet)xlWorkBook.Sheets["AllData"];
                            int row1 = dgvAllData.Rows.Count;
                            int RowFormodify1 = row1 + 1;

                            if (dgvAllData.Rows.Count == 1)
                            {
                                for (int i = 0; i < dgvAllData.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dgvAllData.Columns.Count; j++)
                                    {
                                        if (dgvAllData.Rows[i].Cells[j].Value != null)
                                        {
                                            worksheetAllData.Cells[i + 1, j + 1] = dgvAllData.Rows[i].Cells[j].Value.ToString();

                                        }
                                        else
                                        {
                                            //worksheet1.Cells[i + 5, j + 1] = "";
                                        }
                                    }
                                }
                            }
                            else if (dgvAllData.Rows.Count > 1)
                            {
                                int RowOfDgv = dgvAllData.Rows.Count;
                                worksheetAllData.Range["3:" + (RowOfDgv + 1)].Insert();

                                for (int i = 0; i < dgvAllData.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dgvAllData.Columns.Count; j++)
                                    {
                                        if (dgvAllData.Rows[i].Cells[j].Value != null)
                                        {
                                            worksheetAllData.Cells[i + 1, j + 1] = dgvAllData.Rows[i].Cells[j].Value.ToString();

                                        }
                                        else
                                        {
                                            //worksheet1.Cells[i + 5, j + 1] = "";
                                        }
                                    }
                                }
                            }
                            else
                            {

                            }
                            worksheetAllData.Range["A2:K" + RowFormodify1].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                            worksheetAllData.Range["A2:K" + RowFormodify1].Font.Name = "Khmer OS Battambang";
                            worksheetAllData.Range["A2:K" + RowFormodify1].Font.Size = 10;
                            worksheetAllData.Range["A2:K" + RowFormodify1].Font.Bold = false;



                            // Saving the modified Excel file                        
                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                            string file = "WIP_Transact ";
                            fName = file + "( " + date + " )";
                            worksheet.SaveAs(CDirectory.ToString() + @"\Report\WIP_Transaction\" + fName + ".xlsx");
                            xlWorkBook.Save();
                            xlWorkBook.Close();
                            excelApp.Quit();
                        }
                        else if (dgvInventoryData.Rows.Count > 1)
                        {
                            int RowOfDgv = dgvInventoryData.Rows.Count;
                            worksheet.Range["7:" + (RowOfDgv + 5)].Insert();
                            worksheet.Cells[3, 2] = DateTime.Now;

                            for (int i = 0; i < dgvInventoryData.Rows.Count; i++)
                            {
                                for (int j = 0; j < dgvInventoryData.Columns.Count; j++)
                                {
                                    if (dgvInventoryData.Rows[i].Cells[j].Value != null)
                                    {
                                        worksheet.Cells[i + 6, j + 1] = dgvInventoryData.Rows[i].Cells[j].Value.ToString();

                                    }
                                    else
                                    {
                                        //worksheet1.Cells[i + 5, j + 1] = "";
                                    }
                                }
                            }
                            worksheet.Range["A6:C" + (RowFormodify-1)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                            worksheet.Range["A6:C" + RowFormodify].Font.Name = "Khmer OS Battambang";
                            worksheet.Range["A6:C" + RowFormodify].Font.Size = 11;
                            worksheet.Range["A6:C" + (RowFormodify - 1)].Font.Bold = false;

                            worksheet.Range["B"+ RowFormodify + ":C" + RowFormodify].Style.HorizontalAlignment = HorizontalAlignment.Right;
                            worksheet.Range["B" + RowFormodify + ":C" + RowFormodify].Style.VerticalAlignment = VerticalAlignment.Center;
                            worksheet.Range["B" + RowFormodify + ":C" + RowFormodify].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                            worksheet.Range["B" + RowFormodify + ":C" + RowFormodify].Font.Bold = true;

                            Excel.Worksheet worksheetAllData = (Excel.Worksheet)xlWorkBook.Sheets["AllData"];
                            int row1 = dgvAllData.Rows.Count;
                            int RowFormodify1 = row1 + 1;

                            if (dgvAllData.Rows.Count == 1)
                            {
                                for (int i = 0; i < dgvAllData.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dgvAllData.Columns.Count-1; j++)
                                    {
                                        if (dgvAllData.Rows[i].Cells[j].Value != null)
                                        {
                                            worksheetAllData.Cells[i + 2, j + 1] = dgvAllData.Rows[i].Cells[j].Value.ToString();

                                        }
                                        else
                                        {
                                            //worksheet1.Cells[i + 5, j + 1] = "";
                                        }
                                    }
                                }
                            }
                            else if (dgvAllData.Rows.Count > 1)
                            {
                                int RowOfDgv2 = dgvAllData.Rows.Count;
                                worksheetAllData.Range["3:" + (RowOfDgv2 + 1)].Insert();

                                for (int i = 0; i < dgvAllData.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dgvAllData.Columns.Count-1; j++)
                                    {
                                        if (dgvAllData.Rows[i].Cells[j].Value != null)
                                        {
                                            worksheetAllData.Cells[i + 2, j + 1] = dgvAllData.Rows[i].Cells[j].Value.ToString();

                                        }
                                        else
                                        {
                                            //worksheet1.Cells[i + 5, j + 1] = "";
                                        }
                                    }
                                }
                            }
                            else
                            {

                            }
                            worksheetAllData.Range["A2:K" + RowFormodify1].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                            worksheetAllData.Range["A2:K" + RowFormodify1].Font.Name = "Khmer OS Battambang";
                            worksheetAllData.Range["A2:K" + RowFormodify1].Font.Size = 10;
                            worksheetAllData.Range["A2:K" + RowFormodify1].Font.Bold = false;


                            // Saving the modified Excel file
                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                            string file = "WIP_Transact ";
                            fName = file + "( " + date + " )";
                            worksheet.SaveAs(CDirectory.ToString() + @"\Report\WIP_Transaction\" + fName + ".xlsx");
                            xlWorkBook.Save();
                            xlWorkBook.Close();
                            excelApp.Quit();
                        }
                        else
                        {

                        }
                    }
                    catch (System.Exception ex)
                    {
                        excelApp.DisplayAlerts = false;
                        xlWorkBook.Close();
                        excelApp.DisplayAlerts = true;
                        excelApp.Quit();
                        LbExportStatus.Text = "ការរក្សាទុកមានបញ្ហា ៖​ " + ex.Message + ", អាចមកពី File នេះកំពុងតែបើក !";
                    }

                    //Kill all Excel background process
                    var processes = from p in Process.GetProcessesByName("EXCEL")
                                    select p;
                    foreach (var process in processes)
                    {
                        if (process.MainWindowTitle.ToString().Trim() == "")
                            process.Kill();
                    }

                    LbExportStatus.Text = "ព្រីនបានជោគជ័យ​ !";
                    MessageBox.Show("ការព្រីនបានជោគជ័យ​ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(CDirectory.ToString() + @"\\Report\WIP_Transaction\" + fName + ".xlsx");
                    fName = "";
                    LbExportStatus.Visible = false;

                }
                else
                {
                    
                }
            }
        }
    }
}
