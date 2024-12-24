using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.OBS
{
    public partial class OBSTransactionForm : Form
    {
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        DataTable dtFunct;
        DataTable dtTransaction;
        public DataTable dtWIPResult;
        public static string POSResultSelected;

        public OBSTransactionForm()
        {
            InitializeComponent();
            cnnOBS.Connection();
            this.groupBox1.Click += GroupBox1_Click;
            this.dgvFunct.CellClick += DgvFunct_CellClick;
            this.dgvTransaction.CellDoubleClick += DgvTransaction_CellDoubleClick;
        }

        private void DgvTransaction_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            POSResultSelected="";
            if (e.RowIndex > -1)
            {
                DataGridViewRow row = this.dgvTransaction.Rows[e.RowIndex];
                if (row.Cells[1].Value.ToString() == "កាត់កងជាមួយសឺមី")
                {
                    POSResultSelected = row.Cells[0].Value.ToString();
                    OBSTransactionWIPResultForm Otwrf = new OBSTransactionWIPResultForm(this);
                    Otwrf.ShowDialog();
                }
            }
        }

        private void hideDgvFunct()
        {
            dgvFunct.Visible = false;
            dgvFunct.SendToBack();

        }

        private void DgvFunct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dgvFunct.Rows[e.RowIndex];
            txtFunction.Text = row.Cells[1].Value.ToString();
            hideDgvFunct();
            
        }

        private void GroupBox1_Click(object sender, EventArgs e)
        {
            dgvFunct.Visible = false;
            dgvFunct.SendToBack();
        }

        private void OBSTransactionForm_Load(object sender, EventArgs e)
        {
            dtFunct = new DataTable();
            dtFunct.Columns.Add("FunctCode");
            dtFunct.Columns.Add("FunctName");
            dtFunct.Rows.Add(50, "ស្តុកចូល");
            dtFunct.Rows.Add(60, "ស្តុកចេញ");
            dtFunct.Rows.Add(40, "កាត់កងជាមួយសឺមី");
            dtFunct.Rows.Add(91, "NG");
            dtFunct.Rows.Add(90, "ដាក់ស្តុកចូល");
            dtFunct.Rows.Add(95, "ដកស្តុកចេញ");
            dtFunct.Rows.Add(70, "ទិន្នន័យរាប់ស្តុក");

            for (int i = 0; i < dtFunct.Rows.Count-1; i++)
            {
                dgvFunct.Rows.Add(dtFunct.Rows[i][0].ToString(), dtFunct.Rows[i][1].ToString());                
            }

        }

        private void btnShowDgvItems_Click(object sender, EventArgs e)
        {
            dgvFunct.ClearSelection();
            dgvFunct.Visible = true;
            dgvFunct.BringToFront();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LbTotalResult.Visible = true;
            LbTotalResult.Text = "កំពុងស្វែងរក . . . . ";
            LbTotalResult.Update();
            

            string ItemName = txtItem.Text.ToString();
            string GRICode = "";
            for (int i = 0; i < dtFunct.Rows.Count; i++)
            {
                if (txtFunction.Text.ToString() == dtFunct.Rows[i][1].ToString())
                {
                    GRICode = dtFunct.Rows[i][0].ToString();
                }
            }
            string DocN=txtDocNo.Text.ToString();
            string Remark = txtRemark.Text.ToString();
            string StartDate = DtInput.Value.ToString("yyyy-MM-dd")+" 00:00:00";
            string EndDate = DtEndInput.Value.ToString("yyyy-MM-dd") + " 23:59:59";

            dgvTransaction.Rows.Clear();
            dtTransaction= new DataTable();
            dtWIPResult = new DataTable();

            //4 Conditions
            if (ItemName.Trim() != "" && GRICode.Trim() != "" && DocN.Trim() != "" && Remark.Trim() != "")
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * FROM "+
                                                                                 "(Select DocumentNo, GRICode, ItemCode, SUM(ReceiveQty) As RecQty, SUM(IssueQty) As IssQty, Remark, TransactionDate, CreateUser  From prgalltransaction "+
                                                                                 "Where GRICode = '"+GRICode+ "' AND DocumentNo Like '%"+DocN+"%' AND Remark Like '%" + Remark+"%' AND LocCode = 'MC1' AND TransactionDate BETWEEN '"+StartDate+"' AND '"+EndDate+"' Group By DocumentNo, GRICode, ItemCode, Remark, TransactionDate, CreateUser) t1 "+
                                                                                 "INNER JOIN "+
                                                                                 "(Select ItemCode, ItemName, UnitCode From mstitem Where ItemName Like '%"+ItemName+"%') t2 "+
                                                                                 "ON t1.ItemCode = t2.ItemCode "+
                                                                                 "INNER JOIN "+
                                                                                 "(Select StaffId, SurName from mststaff) t3 "+
                                                                                 "ON t3.StaffId = t1.CreateUser "+
                                                                                 "Order By TransactionDate ASC, DocumentNo ASC, t2.ItemCode ASC", cnnOBS.conOBS);
                    da.Fill(dtTransaction);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!"+ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }

            //3 Conditions
            else if (ItemName.Trim() != "" && GRICode.Trim() != "" && DocN.Trim() != "")
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * FROM " +
                                                                                 "(Select DocumentNo, GRICode, ItemCode, SUM(ReceiveQty) As RecQty, SUM(IssueQty) As IssQty, Remark, TransactionDate, CreateUser  From prgalltransaction " +
                                                                                 "Where GRICode = '" + GRICode + "' AND DocumentNo Like '%" + DocN + "%' AND LocCode = 'MC1' AND TransactionDate BETWEEN '" + StartDate + "' AND '" + EndDate + "' Group By DocumentNo, GRICode, ItemCode, Remark, TransactionDate, CreateUser) t1 " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select ItemCode, ItemName, UnitCode From mstitem Where ItemName Like '%" + ItemName + "%') t2 " +
                                                                                 "ON t1.ItemCode = t2.ItemCode " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select StaffId, SurName from mststaff) t3 " +
                                                                                 "ON t3.StaffId = t1.CreateUser "+
                                                                                 "Order By TransactionDate ASC, DocumentNo ASC, t2.ItemCode ASC", cnnOBS.conOBS);
                    da.Fill(dtTransaction);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }
            else if (ItemName.Trim() != "" && GRICode.Trim() != "" && Remark.Trim() != "")
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * FROM " +
                                                                                 "(Select DocumentNo, GRICode, ItemCode, SUM(ReceiveQty) As RecQty, SUM(IssueQty) As IssQty, Remark, TransactionDate, CreateUser  From prgalltransaction " +
                                                                                 "Where GRICode = '" + GRICode + "' AND Remark Like '%" + Remark + "%' AND LocCode = 'MC1' AND TransactionDate BETWEEN '" + StartDate + "' AND '" + EndDate + "' Group By DocumentNo, GRICode, ItemCode, Remark, TransactionDate, CreateUser) t1 " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select ItemCode, ItemName, UnitCode From mstitem Where ItemName Like '%" + ItemName + "%') t2 " +
                                                                                 "ON t1.ItemCode = t2.ItemCode " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select StaffId, SurName from mststaff) t3 " +
                                                                                 "ON t3.StaffId = t1.CreateUser "+
                                                                                 "Order By TransactionDate ASC, DocumentNo ASC, t2.ItemCode ASC", cnnOBS.conOBS);
                    da.Fill(dtTransaction);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }
            else if (ItemName.Trim() != "" && DocN.Trim() != "" && Remark.Trim() != "")
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * FROM " +
                                                                                 "(Select DocumentNo, GRICode, ItemCode, SUM(ReceiveQty) As RecQty, SUM(IssueQty) As IssQty, Remark, TransactionDate, CreateUser  From prgalltransaction " +
                                                                                 "Where DocumentNo Like '%" + DocN + "%' AND Remark Like '%" + Remark + "%' AND LocCode = 'MC1' AND TransactionDate BETWEEN '" + StartDate + "' AND '" + EndDate + "' Group By DocumentNo, GRICode, ItemCode, Remark, TransactionDate, CreateUser) t1 " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select ItemCode, ItemName, UnitCode From mstitem Where ItemName Like '%" + ItemName + "%') t2 " +
                                                                                 "ON t1.ItemCode = t2.ItemCode " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select StaffId, SurName from mststaff) t3 " +
                                                                                 "ON t3.StaffId = t1.CreateUser "+
                                                                                 "Order By TransactionDate ASC, DocumentNo ASC, t2.ItemCode ASC", cnnOBS.conOBS);
                    da.Fill(dtTransaction);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }
            else if (GRICode.Trim() != "" && DocN.Trim() != "" && Remark.Trim() != "")
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * FROM " +
                                                                                 "(Select DocumentNo, GRICode, ItemCode, SUM(ReceiveQty) As RecQty, SUM(IssueQty) As IssQty, Remark, TransactionDate, CreateUser  From prgalltransaction " +
                                                                                 "Where GRICode = '" + GRICode + "' AND DocumentNo Like '%" + DocN + "%' AND Remark Like '%" + Remark + "%' AND LocCode = 'MC1' AND TransactionDate BETWEEN '" + StartDate + "' AND '" + EndDate + "' Group By DocumentNo, GRICode, ItemCode, Remark, TransactionDate, CreateUser) t1 " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select ItemCode, ItemName, UnitCode From mstitem) t2 " +
                                                                                 "ON t1.ItemCode = t2.ItemCode " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select StaffId, SurName from mststaff) t3 " +
                                                                                 "ON t3.StaffId = t1.CreateUser "+
                                                                                 "Order By TransactionDate ASC, DocumentNo ASC, t2.ItemCode ASC", cnnOBS.conOBS);
                    da.Fill(dtTransaction);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }

            //2 Conditions
            else if (ItemName.Trim() != "" && GRICode.Trim() != "" )
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * FROM " +
                                                                                 "(Select DocumentNo, GRICode, ItemCode, SUM(ReceiveQty) As RecQty, SUM(IssueQty) As IssQty, Remark, TransactionDate, CreateUser  From prgalltransaction " +
                                                                                 "Where GRICode = '" + GRICode + "' AND LocCode = 'MC1' AND TransactionDate BETWEEN '" + StartDate + "' AND '" + EndDate + "' Group By DocumentNo, GRICode, ItemCode, Remark, TransactionDate, CreateUser) t1 " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select ItemCode, ItemName, UnitCode From mstitem Where ItemName Like '%" + ItemName + "%') t2 " +
                                                                                 "ON t1.ItemCode = t2.ItemCode " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select StaffId, SurName from mststaff) t3 " +
                                                                                 "ON t3.StaffId = t1.CreateUser "+
                                                                                 "Order By TransactionDate ASC, DocumentNo ASC, t2.ItemCode ASC", cnnOBS.conOBS);
                    da.Fill(dtTransaction);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }
            else if (ItemName.Trim() != "" && DocN.Trim() != "")
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * FROM " +
                                                                                 "(Select DocumentNo, GRICode, ItemCode, SUM(ReceiveQty) As RecQty, SUM(IssueQty) As IssQty, Remark, TransactionDate, CreateUser  From prgalltransaction " +
                                                                                 "Where DocumentNo Like '%" + DocN + "%' AND LocCode = 'MC1' AND TransactionDate BETWEEN '" + StartDate + "' AND '" + EndDate + "' Group By DocumentNo, GRICode, ItemCode, Remark, TransactionDate, CreateUser) t1 " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select ItemCode, ItemName, UnitCode From mstitem Where ItemName Like '%" + ItemName + "%') t2 " +
                                                                                 "ON t1.ItemCode = t2.ItemCode " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select StaffId, SurName from mststaff) t3 " +
                                                                                 "ON t3.StaffId = t1.CreateUser "+
                                                                                 "Order By TransactionDate ASC, DocumentNo ASC, t2.ItemCode ASC", cnnOBS.conOBS);
                    da.Fill(dtTransaction);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }
            else if (ItemName.Trim() != "" && Remark.Trim() != "")
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * FROM " +
                                                                                 "(Select DocumentNo, GRICode, ItemCode, SUM(ReceiveQty) As RecQty, SUM(IssueQty) As IssQty, Remark, TransactionDate, CreateUser  From prgalltransaction " +
                                                                                 "Where Remark Like '%" + Remark + "%' AND LocCode = 'MC1' AND TransactionDate BETWEEN '" + StartDate + "' AND '" + EndDate + "' Group By DocumentNo, GRICode, ItemCode, Remark, TransactionDate, CreateUser) t1 " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select ItemCode, ItemName, UnitCode From mstitem Where ItemName Like '%" + ItemName + "%') t2 " +
                                                                                 "ON t1.ItemCode = t2.ItemCode " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select StaffId, SurName from mststaff) t3 " +
                                                                                 "ON t3.StaffId = t1.CreateUser "+
                                                                                 "Order By TransactionDate ASC, DocumentNo ASC, t2.ItemCode ASC", cnnOBS.conOBS);
                    da.Fill(dtTransaction);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }
            else if (GRICode.Trim() != "" && DocN.Trim() != "")
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * FROM " +
                                                                                 "(Select DocumentNo, GRICode, ItemCode, SUM(ReceiveQty) As RecQty, SUM(IssueQty) As IssQty, Remark, TransactionDate, CreateUser  From prgalltransaction " +
                                                                                 "Where GRICode = '" + GRICode + "' AND DocumentNo Like '%" + DocN + "%' AND LocCode = 'MC1' AND TransactionDate BETWEEN '" + StartDate + "' AND '" + EndDate + "' Group By DocumentNo, GRICode, ItemCode, Remark, TransactionDate, CreateUser) t1 " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select ItemCode, ItemName, UnitCode From mstitem) t2 " +
                                                                                 "ON t1.ItemCode = t2.ItemCode " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select StaffId, SurName from mststaff) t3 " +
                                                                                 "ON t3.StaffId = t1.CreateUser "+
                                                                                 "Order By TransactionDate ASC, DocumentNo ASC, t2.ItemCode ASC", cnnOBS.conOBS);
                    da.Fill(dtTransaction);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }
            else if (GRICode.Trim() != "" && Remark.Trim() != "")
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * FROM " +
                                                                                 "(Select DocumentNo, GRICode, ItemCode, SUM(ReceiveQty) As RecQty, SUM(IssueQty) As IssQty, Remark, TransactionDate, CreateUser  From prgalltransaction " +
                                                                                 "Where GRICode = '" + GRICode + "' AND Remark Like '%" + Remark + "%' AND LocCode = 'MC1' AND TransactionDate BETWEEN '" + StartDate + "' AND '" + EndDate + "' Group By DocumentNo, GRICode, ItemCode, Remark, TransactionDate, CreateUser) t1 " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select ItemCode, ItemName, UnitCode From mstitem) t2 " +
                                                                                 "ON t1.ItemCode = t2.ItemCode " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select StaffId, SurName from mststaff) t3 " +
                                                                                 "ON t3.StaffId = t1.CreateUser "+
                                                                                 "Order By TransactionDate ASC, DocumentNo ASC, t2.ItemCode ASC", cnnOBS.conOBS);
                    da.Fill(dtTransaction);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }
            else if (DocN.Trim() != "" && Remark.Trim() != "")
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * FROM " +
                                                                                 "(Select DocumentNo, GRICode, ItemCode, SUM(ReceiveQty) As RecQty, SUM(IssueQty) As IssQty, Remark, TransactionDate, CreateUser  From prgalltransaction " +
                                                                                 "Where DocumentNo Like '%" + DocN + "%' AND Remark Like '%" + Remark + "%' AND LocCode = 'MC1' AND TransactionDate BETWEEN '" + StartDate + "' AND '" + EndDate + "' Group By DocumentNo, GRICode, ItemCode, Remark, TransactionDate, CreateUser) t1 " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select ItemCode, ItemName, UnitCode From mstitem) t2 " +
                                                                                 "ON t1.ItemCode = t2.ItemCode " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select StaffId, SurName from mststaff) t3 " +
                                                                                 "ON t3.StaffId = t1.CreateUser "+
                                                                                 "Order By TransactionDate ASC, DocumentNo ASC, t2.ItemCode ASC", cnnOBS.conOBS);
                    da.Fill(dtTransaction);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }

            //1 Condition
            else if (ItemName.Trim() != "")
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * FROM " +
                                                                                 "(Select DocumentNo, GRICode, ItemCode, SUM(ReceiveQty) As RecQty, SUM(IssueQty) As IssQty, Remark, TransactionDate, CreateUser  From prgalltransaction " +
                                                                                 "Where LocCode = 'MC1' AND TransactionDate BETWEEN '" + StartDate + "' AND '" + EndDate + "' Group By DocumentNo, GRICode, ItemCode, Remark, TransactionDate, CreateUser) t1 " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select ItemCode, ItemName, UnitCode From mstitem Where ItemName Like '%" + ItemName + "%') t2 " +
                                                                                 "ON t1.ItemCode = t2.ItemCode " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select StaffId, SurName from mststaff) t3 " +
                                                                                 "ON t3.StaffId = t1.CreateUser "+
                                                                                 "Order By TransactionDate ASC, DocumentNo ASC, t2.ItemCode ASC", cnnOBS.conOBS);
                    da.Fill(dtTransaction);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }
            else if (GRICode.Trim() != "")
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * FROM " +
                                                                                 "(Select DocumentNo, GRICode, ItemCode, SUM(ReceiveQty) As RecQty, SUM(IssueQty) As IssQty, Remark, TransactionDate, CreateUser  From prgalltransaction " +
                                                                                 "Where GRICode = '" + GRICode + "' AND LocCode = 'MC1' AND TransactionDate BETWEEN '" + StartDate + "' AND '" + EndDate + "' Group By DocumentNo, GRICode, ItemCode, Remark, TransactionDate, CreateUser) t1 " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select ItemCode, ItemName, UnitCode From mstitem) t2 " +
                                                                                 "ON t1.ItemCode = t2.ItemCode " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select StaffId, SurName from mststaff) t3 " +
                                                                                 "ON t3.StaffId = t1.CreateUser "+
                                                                                 "Order By TransactionDate ASC, DocumentNo ASC, t2.ItemCode ASC", cnnOBS.conOBS);
                    da.Fill(dtTransaction);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }
            else if (DocN.Trim() != "")
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * FROM " +
                                                                                 "(Select DocumentNo, GRICode, ItemCode, SUM(ReceiveQty) As RecQty, SUM(IssueQty) As IssQty, Remark, TransactionDate, CreateUser  From prgalltransaction " +
                                                                                 "Where DocumentNo Like '%" + DocN + "%' AND LocCode = 'MC1' AND TransactionDate BETWEEN '" + StartDate + "' AND '" + EndDate + "' Group By DocumentNo, GRICode, ItemCode, Remark, TransactionDate, CreateUser) t1 " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select ItemCode, ItemName, UnitCode From mstitem) t2 " +
                                                                                 "ON t1.ItemCode = t2.ItemCode " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select StaffId, SurName from mststaff) t3 " +
                                                                                 "ON t3.StaffId = t1.CreateUser "+
                                                                                 "Order By TransactionDate ASC, DocumentNo ASC, t2.ItemCode ASC", cnnOBS.conOBS);
                    da.Fill(dtTransaction);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }
            else if (Remark.Trim() != "")
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * FROM " +
                                                                                 "(Select DocumentNo, GRICode, ItemCode, SUM(ReceiveQty) As RecQty, SUM(IssueQty) As IssQty, Remark, TransactionDate, CreateUser  From prgalltransaction " +
                                                                                 "Where Remark Like '%" + Remark + "%' AND LocCode = 'MC1' AND TransactionDate BETWEEN '" + StartDate + "' AND '" + EndDate + "' Group By DocumentNo, GRICode, ItemCode, Remark, TransactionDate, CreateUser) t1 " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select ItemCode, ItemName, UnitCode From mstitem) t2 " +
                                                                                 "ON t1.ItemCode = t2.ItemCode " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select StaffId, SurName from mststaff) t3 " +
                                                                                 "ON t3.StaffId = t1.CreateUser "+
                                                                                 "Order By TransactionDate ASC, DocumentNo ASC, t2.ItemCode ASC", cnnOBS.conOBS);
                    da.Fill(dtTransaction);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }

            //No Condition
            else 
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * FROM " +
                                                                                 "(Select DocumentNo, GRICode, ItemCode, SUM(ReceiveQty) As RecQty, SUM(IssueQty) As IssQty, Remark, TransactionDate, CreateUser  From prgalltransaction " +
                                                                                 "Where LocCode = 'MC1' AND TransactionDate BETWEEN '" + StartDate + "' AND '" + EndDate + "' Group By DocumentNo, GRICode, ItemCode, Remark, TransactionDate, CreateUser) t1 " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select ItemCode, ItemName, UnitCode From mstitem) t2 " +
                                                                                 "ON t1.ItemCode = t2.ItemCode " +
                                                                                 "INNER JOIN " +
                                                                                 "(Select StaffId, SurName from mststaff) t3 " +
                                                                                 "ON t3.StaffId = t1.CreateUser "+
                                                                                 "Order By TransactionDate ASC, DocumentNo ASC, t2.ItemCode ASC", cnnOBS.conOBS);
                    da.Fill(dtTransaction);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }

            if (dtTransaction.Rows.Count > 0)
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da = new SqlDataAdapter(" Select * FROM "+
                                                                                     "(Select DocumentNo, ItemCode, ReceiveQty, BoxNO, Remark, CreateDate, CreateUser  From prgalltransaction " +
                                                                                     "Where GRICode = '30' AND LocCode = 'WIP1' AND TransactionDate BETWEEN '" + StartDate + "' AND '" + EndDate + "') t1 " +
                                                                                     "INNER JOIN "+
                                                                                     "(Select ItemCode, ItemName, Remark2, Remark3, Remark4 From mstitem) t2 "+
                                                                                     "ON t1.ItemCode = t2.ItemCode "+
                                                                                     "INNER JOIN "+
                                                                                     "(Select StaffId, SurName from mststaff) t3 "+
                                                                                     "ON t3.StaffId = t1.CreateUser "+
                                                                                     "Order By t1.CreateDate ASC", cnnOBS.conOBS);
                    da.Fill(dtWIPResult);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();

                for (int i = 0; i < dtTransaction.Rows.Count; i++)
                {
                    string DocNo = dtTransaction.Rows[i][0].ToString();
                    string GRIName = "";
                    for (int j = 0; j < dtFunct.Rows.Count; j++)
                    {
                        if (dtTransaction.Rows[i][1].ToString() == dtFunct.Rows[j][0].ToString())
                        {
                            GRIName = dtFunct.Rows[j][1].ToString(); 
                        }
                    }
                    string ItemC= dtTransaction.Rows[i][8].ToString();
                    string ItemN = dtTransaction.Rows[i][9].ToString();
                    double Instock= Convert.ToDouble(dtTransaction.Rows[i][3].ToString());
                    double Outstock = Convert.ToDouble(dtTransaction.Rows[i][4].ToString());
                    string Unit= dtTransaction.Rows[i][10].ToString();
                    string Rem = dtTransaction.Rows[i][5].ToString();
                    DateTime TransDate = Convert.ToDateTime(dtTransaction.Rows[i][6].ToString());
                    string InputBy= dtTransaction.Rows[i][12].ToString();

                    if (GRIName.Trim() != "")
                    {
                        dgvTransaction.Rows.Add(DocNo, GRIName, ItemC, ItemN, Instock, Outstock, Unit, Rem, TransDate, InputBy);

                    }
                }

                //add Total
                double SumIn = 0;
                double SumOut = 0;
                for (int i = 0; i < dgvTransaction.Rows.Count; i++)
                {
                    SumIn += Convert.ToDouble(dgvTransaction.Rows[i].Cells[4].Value);
                    SumOut += Convert.ToDouble(dgvTransaction.Rows[i].Cells[5].Value);

                }
                int lastrowofDgv = dgvTransaction.RowCount;

                dgvTransaction.Rows.Insert(lastrowofDgv, "", "", "", "សរុប​ ", SumIn, SumOut);
                dgvTransaction.Rows[lastrowofDgv].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvTransaction.Rows[lastrowofDgv].DefaultCellStyle.Font = new System.Drawing.Font("Khmer OS Battambang", 11, FontStyle.Bold);

                LbTotalResult.Text = "រកឃើញទិន្នន័យ  " +dtTransaction.Rows.Count.ToString("N0");
                LbTotalResult.Update();
                
                dgvTransaction.ClearSelection();
            }
            else
            {
                LbTotalResult.Text = "រកឃើញទិន្នន័យ  " + dtTransaction.Rows.Count.ToString("N0");
                LbTotalResult.Update();

            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvTransaction.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("តើអ្នកចង់ទាញទិន្នន័យចេញជា Excel មែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {                    
                    //Export to excel
                    //open excel application and create new workbook
                    Excel.Application app = new Excel.Application();
                    Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
                    Excel.Worksheet worksheet = null;

                    //hide new excel application and give a sheet name
                    app.Visible = false;
                    worksheet = workbook.Sheets["Sheet1"];
                    worksheet = workbook.ActiveSheet;
                    worksheet.Name = "Rachhan System";

                    try
                    {
                        //Add Header
                        for (int i = 0; i < dgvTransaction.Columns.Count; i++)
                        {
                            string ColN = dgvTransaction.Columns[i].HeaderText.ToString();
                            worksheet.Cells[1, 1 + i] = ColN;
                        }
                        for (int i = 0; i < dgvTransaction.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i + 1].Font.Name = "Khmer OS Battambang";
                            worksheet.Cells[1, i + 1].Font.Size = 12;
                            worksheet.Cells[1, i + 1].Font.Bold = true;
                        }

                        //Add list
                        for (int i = 0; i < dgvTransaction.Rows.Count; i++)
                        {
                            for (int j = 0; j < dgvTransaction.Columns.Count; j++)
                            {
                                if (dgvTransaction.Rows[i].Cells[j].Value != null)
                                {
                                    worksheet.Cells[i + 2, 1 + j] = dgvTransaction.Rows[i].Cells[j].Value.ToString();
                                }
                                else
                                {
                                    worksheet.Cells[i + 2, 1 + j] = "";
                                }
                            }
                        }
                                                
                        int rows= dgvTransaction.Rows.Count+1;
                        worksheet.Range["A1:J" + (rows-1)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        worksheet.Range["A2:J" + rows].Font.Name = "Khmer OS Battambang";
                        worksheet.Range["A2:J" + rows].Font.Size = 11;
                        worksheet.Range["A2:J" + rows].Font.Bold = false;
                        worksheet.Range["A"+rows+":J" + rows].Font.Bold = true;

                        //Set Column Fit
                        for (int i = 0; i < dgvTransaction.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i + 1].EntireColumn.AutoFit();

                        }

                        //Getting the location and file name of the excel to save from user. 
                        SaveFileDialog saveDialog = new SaveFileDialog();
                        saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                        saveDialog.FilterIndex = 1;

                        if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            string saveDir = saveDialog.FileName;
                            app.DisplayAlerts = false;
                            workbook.SaveAs(saveDialog.FileName, Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, Type.Missing, false, Excel.XlSaveAsAccessMode.xlExclusive, Excel.XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
                            workbook.Close();
                            app.DisplayAlerts = true;
                            app.Quit();

                            //Kill all Excel background process
                            var processes = from p in Process.GetProcessesByName("EXCEL")
                            select p;
                            foreach (var process in processes)
                            {
                                if (process.MainWindowTitle.ToString().Trim() == "")
                                    process.Kill();
                            }

                            MessageBox.Show("ទាញទិន្នន័យចេញបានជោគជ័យ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            System.Diagnostics.Process.Start(saveDir.ToString());

                        }
                        else
                        {
                            //don't save and close workbook that just created
                            app.DisplayAlerts = false;
                            workbook.Close();
                            app.DisplayAlerts = true;
                            app.Quit();

                        }
                    }
                    catch
                    {
                        app.DisplayAlerts = false;
                        workbook.Close();
                        app.DisplayAlerts = true;
                        app.Quit();

                    }
                }
            }
        }
    }
}
