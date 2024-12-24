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
using System.IO;

namespace MachineDeptApp.Admin
{
    public partial class MachineTypeMasterForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SqlCommand cmd;

        //1 add, 2 update
        string AddOrUpdate;

        

        public MachineTypeMasterForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Load += MachineTypeMasterForm_Load;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnNew.Click += BtnNew_Click;
            this.btnUpdate.Click += BtnUpdate_Click;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnClose.Click += BtnClose_Click;
            this.btnSave.Click += BtnSave_Click;
            this.dgvSearchResult.CellClick += DgvSearchResult_CellClick;
            this.btnExport.Click += BtnExport_Click;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvSearchResult.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("តើអ្នកចង់ទាញទិន្នន័យចេញមែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "CSV file (*.csv)|*.csv";
                    saveDialog.FileName = "MC_Name.csv";
                    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;
                        try
                        {
                            //Write Column name
                            int columnCount = 0;
                            foreach (DataGridViewColumn DgvCol in dgvSearchResult.Columns)
                            {
                                if (DgvCol.Visible == true)
                                {
                                    columnCount = columnCount + 1;
                                }
                            }
                            string columnNames = "";

                            //String array for Csv
                            string[] outputCsv;
                            outputCsv = new string[dgvSearchResult.Rows.Count + 1];

                            //Set Column Name
                            for (int i = 0; i < columnCount; i++)
                            {
                                if (dgvSearchResult.Columns[i].Visible == true)
                                {
                                    columnNames += dgvSearchResult.Columns[i].HeaderText.ToString() + ",";
                                }
                            }
                            outputCsv[0] += columnNames;

                            //Row of data 
                            for (int i = 1; (i - 1) < dgvSearchResult.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    if (dgvSearchResult.Columns[j].Visible == true)
                                    {
                                        string Value = "";
                                        if (dgvSearchResult.Rows[i - 1].Cells[j].Value != null)
                                        {
                                            Value = dgvSearchResult.Rows[i - 1].Cells[j].Value.ToString();
                                        }
                                        //Fix don't separate if it contain '\n' or ','
                                        Value = "\"" + Value.Replace("\"", "\"\"") + "\"";
                                        outputCsv[i] += Value + ",";
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

        private void DgvSearchResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CboMCTypeEdit.Text = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[0].Value.ToString();
            txtMCNameEdit.Text = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[1].Value.ToString();
            CheckBtnUpdateDel();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (AddOrUpdate == "1")
            {
                SaveToDB();
            }
            else
            {
                UpdateToDB();
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            HideEditTabContrl();
            btnSearch.Enabled = true;
            btnNew.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            CheckBtnUpdateDel();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult DLR = MessageBox.Show("តើអ្នកចង់លុបម៉ាស៊ីននេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DLR == DialogResult.Yes)
            {
                try
                {
                    cnn.con.Open();
                    //Delete in SaleHis                     
                    SqlCommand cmd = new SqlCommand("DELETE FROM tbMasterMCType WHERE MCType ='" + dgvSearchResult.CurrentRow.Cells[0].Value.ToString() + "' AND MCName='" + dgvSearchResult.CurrentRow.Cells[1].Value.ToString() + "';", cnn.con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("ម៉ាស៊ីននេះត្រូវបានលុបរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    int selectedIndex = dgvSearchResult.CurrentCell.RowIndex;
                    if (selectedIndex > -1)
                    {
                        dgvSearchResult.Rows.RemoveAt(selectedIndex);
                        dgvSearchResult.Refresh();
                    }
                    dgvSearchResult.ClearSelection();
                    CheckBtnUpdateDel();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ការលុបមានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                cnn.con.Close();
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            tabContrlEdit.TabPages[0].Text = "អាប់ដេតគណនី";
            AddOrUpdate = "2";
            toolTip1.SetToolTip(btnSave, "អាប់ដេត");
            ShowEditTabContrl();
            btnSearch.Enabled = false;
            btnNew.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            txtMCNameEdit.Focus();
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearAdd();
            tabContrlEdit.TabPages[0].Text = "បន្ថែមម៉ាស៊ីន";
            AddOrUpdate = "1";
            toolTip1.SetToolTip(btnSave, "រក្សាទុក");
            ShowEditTabContrl();
            btnSearch.Enabled = false;
            btnNew.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            CboMCTypeEdit.Focus();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Visible = true;
            LbStatus.Refresh();
            dgvSearchResult.Rows.Clear();
            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Value");

            if (CboMCType.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("MCType = ", "'" + CboMCType.Text + "' ");
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
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbMasterMCType " + SQLConds + " ORDER BY MCType ASC, MCName ASC", cnn.con);
                Console.WriteLine("SELECT * FROM tbMasterMCType " + SQLConds + " ORDER BY MCType ASC, MCName ASC");
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    dgvSearchResult.Rows.Add(row[0], row[1], Convert.ToDateTime(row[2].ToString()), row[3], Convert.ToDateTime(row[4].ToString()), row[5]);
                }
                dgvSearchResult.ClearSelection();
                LbStatus.Text = "រកឃើញទិន្នន័យចំនួន " + dgvSearchResult.Rows.Count.ToString("N0") + " !";
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            cnn.con.Close();
            CheckBtnUpdateDel();
        }

        private void MachineTypeMasterForm_Load(object sender, EventArgs e)
        {
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbMasterItemPlan2", cnn.con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                CboMCType.Items.Add("");
                CboMCTypeEdit.Items.Add("");
                foreach (DataRow row in dt.Rows)
                {
                    CboMCType.Items.Add(row[0]);
                    CboMCTypeEdit.Items.Add(row[0]);
                }
                HideEditTabContrl();
            }
            catch(Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n"+ex.Message,"Rachhan System",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
        }

        private void CheckBtnUpdateDel()
        {
            if (dgvSearchResult.SelectedRows.Count > 0)
            {
                btnUpdate.Enabled = true;
                btnUpdate.BackColor = Color.White;
                btnDelete.Enabled = true;
                btnDelete.BackColor = Color.White;

            }
            else
            {
                btnUpdate.Enabled = false;
                btnUpdate.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
                btnDelete.Enabled = false;
                btnDelete.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
            }
        }

        private void ClearAdd()
        {
            CboMCTypeEdit.Text = "";
            txtMCNameEdit.Text = "";
            CboMCTypeEdit.Focus();

        }

        private void ShowEditTabContrl()
        {
            GroupBoxResult.Height = GroupBoxResult.Height - tabContrlEdit.Height;
            tabContrlEdit.Visible = true;
        }

        private void HideEditTabContrl()
        {
            tabContrlEdit.Visible = false;
            GroupBoxResult.Height = GroupBoxResult.Height + tabContrlEdit.Height;
        }

        private void SaveToDB()
        {
            if (CboMCTypeEdit.Text.Trim() != "" && txtMCNameEdit.Text.Trim() != "")
            {

                DialogResult DLR = MessageBox.Show("តើអ្នកចង់បន្ថែមម៉ាស៊ីននេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    try
                    {
                        cnn.con.Open();
                        cmd = new SqlCommand("INSERT INTO tbMasterMCType (MCType, MCName, RegDate, RegBy, UpdateDate, UpdateBy) " +
                                                                        "VALUES (@McT, @McN, @RegD, @RegB, @UpD, @UpB)", cnn.con);

                        cmd.Parameters.AddWithValue("@McT", CboMCTypeEdit.Text);
                        cmd.Parameters.AddWithValue("@McN", txtMCNameEdit.Text);
                        cmd.Parameters.AddWithValue("@RegD", DateTime.Now);
                        cmd.Parameters.AddWithValue("@RegB", MenuFormV2.UserForNextForm);
                        cmd.Parameters.AddWithValue("@UpD", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UpB", MenuFormV2.UserForNextForm);
                        cmd.ExecuteNonQuery();
                        dgvSearchResult.Rows.Add(CboMCTypeEdit.Text, txtMCNameEdit.Text, DateTime.Now, MenuFormV2.UserForNextForm, DateTime.Now, MenuFormV2.UserForNextForm);
                        MessageBox.Show("បន្ថែមម៉ាស៊ីនថ្មីរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvSearchResult.ClearSelection();
                        ClearAdd();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ម៉ាស៊ីននេះត្រូវបានមានរួចរាល់ហើយ !\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cnn.con.Close();
                }
            }
            else
            {
                MessageBox.Show("សូមបំពេញព័ត៌មានទាំងអស់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void UpdateToDB()
        {
            if (CboMCTypeEdit.Text.Trim() != "" && txtMCNameEdit.Text.Trim()!="")
            {
                DialogResult DLR = MessageBox.Show("តើអ្នកចង់អាប់ដេតគណនីនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    try
                    {
                        cnn.con.Open();
                        string query = "UPDATE tbMasterMCType SET " +
                                            "MCType='" + CboMCTypeEdit.Text + "'," +
                                            "MCName='" + txtMCNameEdit.Text + "'," +
                                            "UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            "UpdateBy=N'" + MenuFormV2.UserForNextForm + "' " +
                                            "WHERE MCType = '" + dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[0].Value.ToString() + "' AND MCName='" + dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[1].Value.ToString() +"' ;";
                        SqlCommand cmd = new SqlCommand(query, cnn.con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("អាប់ដេតរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvSearchResult.CurrentRow.Cells[0].Value = CboMCTypeEdit.Text;
                        dgvSearchResult.CurrentRow.Cells[1].Value = txtMCNameEdit.Text;
                        dgvSearchResult.CurrentRow.Cells[4].Value = DateTime.Now;
                        dgvSearchResult.CurrentRow.Cells[5].Value = MenuFormV2.UserForNextForm;
                        ClearAdd();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("អាប់ដេតមានបញ្ហា !\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cnn.con.Close();
                }
            }
            else
            {
                MessageBox.Show("សូមបំពេញព័ត៌មានទាំងអស់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


    }
}
