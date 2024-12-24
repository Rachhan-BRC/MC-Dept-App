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

namespace MachineDeptApp.Admin
{
    public partial class SLOTMasterForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SqlCommand cmd;

        public SLOTMasterForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Load += SLOTMasterForm_Load;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnNew.Click += BtnNew_Click;
            this.btnClose.Click += BtnClose_Click;
            this.btnSave.Click += BtnSave_Click;
            this.btnDelete.Click += BtnDelete_Click;
            this.dgvSearchResult.CellClick += DgvSearchResult_CellClick;
        }

        private void DgvSearchResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CheckBtnDelete();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult DLR = MessageBox.Show("តើអ្នកចង់លុប SLOT នេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DLR == DialogResult.Yes)
            {
                try
                {
                    cnn.con.Open();
                    //Delete in SaleHis                     
                    SqlCommand cmd = new SqlCommand("DELETE FROM tbMasterSLOT WHERE SLOTName ='" + dgvSearchResult.CurrentRow.Cells[0].Value.ToString() + "';", cnn.con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("SLOT នេះត្រូវបានលុបរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    int selectedIndex = dgvSearchResult.CurrentCell.RowIndex;
                    if (selectedIndex > -1)
                    {
                        dgvSearchResult.Rows.RemoveAt(selectedIndex);
                        dgvSearchResult.Refresh();
                    }
                    dgvSearchResult.ClearSelection();
                    CheckBtnDelete();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ការលុបមានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                cnn.con.Close();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (txtSLOTEdit.Text.Trim() != "")
            {
                DialogResult DLR = MessageBox.Show("តើអ្នកចង់ SLOT នេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    try
                    {
                        cnn.con.Open();
                        cmd = new SqlCommand("INSERT INTO tbMasterSLOT (SLOTName, RegDate, RegBy) " +
                                                                        "VALUES (@Sn, @RegD, @RegB)", cnn.con);

                        cmd.Parameters.AddWithValue("@Sn", txtSLOTEdit.Text);
                        cmd.Parameters.AddWithValue("@RegD", DateTime.Now);
                        cmd.Parameters.AddWithValue("@RegB", MenuFormV2.UserForNextForm);
                        cmd.ExecuteNonQuery();
                        dgvSearchResult.Rows.Add(txtSLOTEdit.Text, DateTime.Now, MenuFormV2.UserForNextForm);
                        MessageBox.Show("បន្ថែម SLOT ថ្មីរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvSearchResult.ClearSelection();
                        txtSLOTEdit.Text = "";
                        txtSLOTEdit.Focus();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("SLOT នេះមានរួចរាល់ហើយ !\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cnn.con.Close();
                }
            }
            else
            {
                MessageBox.Show("សូមបំពេញព័ត៌មានទាំងអស់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            HideEditTabContrl();
            btnSearch.Enabled = true;
            btnNew.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(btnSave, "រក្សាទុក");
            ShowEditTabContrl();
            btnSearch.Enabled = false;
            btnNew.Enabled = false;            
            btnDelete.Enabled = false;
            txtSLOTEdit.Focus();
        }

        private void SLOTMasterForm_Load(object sender, EventArgs e)
        {
            HideEditTabContrl();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                LbStatus.Text = "កំពុងស្វែងរក . . .";
                LbStatus.Refresh();
                LbStatus.Visible = true;
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT SLOTName, RegDate, RegBy FROM tbMasterSLOT ORDER BY SLOTName ASC", cnn.con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgvSearchResult.Rows.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    dgvSearchResult.Rows.Add(row[0], Convert.ToDateTime(row[1].ToString()), row[2]);
                }
                dgvSearchResult.ClearSelection();
                CheckBtnDelete();
                Cursor = Cursors.Default;
                LbStatus.Text = "ស្វែងរកឃើញទិន្នន័យ " + dgvSearchResult.Rows.Count.ToString("N0");
                LbStatus.Refresh();
            }
            catch(Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("មានបញ្ហា!\n"+ex.Message,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            cnn.con.Close();
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

        private void CheckBtnDelete()
        {
            if (dgvSearchResult.SelectedCells.Count > 0)
            {
                btnDelete.Enabled = true;
                btnDelete.BackColor = Color.White;

            }
            else
            {
                btnDelete.Enabled = false;
                btnDelete.BackColor = Color.DarkGray;
            }
        }
    }
}
