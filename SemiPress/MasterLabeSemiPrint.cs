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
using System.Windows.Media.TextFormatting;
using System.Xml;

namespace MachineDeptApp
{
    public partial class MasterLabelSemiPrint : Form
    {
        SQLConnect con = new SQLConnect();
        public MasterLabelSemiPrint()
        {
            this.con.Connection();
            InitializeComponent();
            this.btnAdd.Click += BtnAdd_Click;
            this.btnUpdate.Click += BtnUpdate_Click;
            this.btnCancel.Click += BtnCancel_Click;
            this.btnSave.MouseEnter += BtnSave_MouseEnter;
            this.btnSave.MouseLeave += BtnSave_MouseLeave;
            this.btnCancel.MouseEnter += BtnCancel_MouseEnter;
            this.btnCancel.MouseLeave += BtnCancel_MouseLeave;
            this.picsave.MouseEnter += BtnSave_MouseEnter;
            this.picsave.MouseLeave += BtnSave_MouseLeave;
            this.piccancel.MouseEnter +=BtnCancel_MouseEnter;
            this.piccancel.MouseLeave += BtnCancel_MouseLeave;
            this.piccancel.Click += BtnCancel_Click;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnSave.Click += BtnSave_Click;
            this.picsave.Click += BtnSave_Click;
            this.dgvMst.CellClick += DgvMst_CellClick;
            this.btnDelete.Click += BtnDelete_Click;
            this.searchcode.KeyDown += Searchcode_KeyDown;
            this.searchname.KeyDown += Searchcode_KeyDown;
        }

        private void Searchcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvMst.SelectedCells.Count > 0)
            {
                DialogResult ask = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះឬទេ ?", "បញ្ជាក់", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    try
                    {
                        con.con.Open();
                        string querydelete = "DELETE FROM tbMasterSemiPrint WHERE SysNo=@SysNo";
                        SqlCommand cmd = new SqlCommand(querydelete, con.con);
                        cmd.Parameters.AddWithValue("@SysNo", txtsysno.Text.Trim());
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("ទិន្នន័យត្រូវបានលុបដោយជោគជ័យ!", "ជោគជ័យ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("មានបព្ហា! សូមទាក់ទងទៅអាយធី (ផានន្ទ)!\n" + ex.Message, "Something Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Cursor = Cursors.Default;
                    con.con.Close();
                    btnSearch.PerformClick();
                }
            }
            else
            {
                MessageBox.Show("សូមជ្រើសរើសទិន្នន័យមួយដើម្បីលុប!", "សូមពិនិត្យ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DgvMst_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dgvMst.Rows[e.RowIndex];
                txtcode.Text = row.Cells["semicode"].Value.ToString();
                txtname.Text = row.Cells["seminame"].Value.ToString();
                txtpcs.Text = row.Cells["semipcs"].Value.ToString();
                txtbox.Text = row.Cells["labelbox"].Value.ToString();
                txtsysno.Text = row.Cells["SysNo"].Value.ToString();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtcode.Text.Trim()) && !string.IsNullOrEmpty(txtname.Text.Trim()) && !string.IsNullOrEmpty(txtpcs.Text.Trim()) && !string.IsNullOrEmpty(txtbox.Text.Trim()))
            {
                DialogResult ask = MessageBox.Show("តើអ្នកចង់រក្សាទុកទិន្នន័យនេះឬទេ ?", "បញ្ជាក់", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask == DialogResult.Yes)
                {
                    if (btnSave.Text == "Add")
                    {
                        Cursor = Cursors.WaitCursor;
                        try
                        {

                            con.con.Open();
                            DataTable dt = new DataTable();
                            string querycompare = "SELECT Code FROM tbMasterSemiPrint WHERE Code='"+txtcode.Text.Trim()+"'";
                            SqlDataAdapter sda = new SqlDataAdapter(querycompare, con.con);
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show("កូដនេះមានរួចហើយ!", "សូមពិនិត្យ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                string queryinsert = "INSERT INTO tbMasterSemiPrint (Code, Name, Pcs ,QtyBox, RegBy, RegDate, UpdateBy, UpdateDate) VALUES (@Code, @Name, @Pcs, @QtyBox, @RegBy, @RegDate, @UpdateBy, @UpdateDate)";
                                SqlCommand cmd = new SqlCommand(queryinsert, con.con);
                                cmd.Parameters.AddWithValue("@Code", txtcode.Text.Trim());
                                cmd.Parameters.AddWithValue("@Name", txtname.Text.Trim());
                                cmd.Parameters.AddWithValue("@Pcs", txtpcs.Text.Trim());
                                cmd.Parameters.AddWithValue("@QtyBox", txtbox.Text.Trim());
                                cmd.Parameters.AddWithValue("@RegBy", MenuFormV2.UserForNextForm);
                                cmd.Parameters.AddWithValue("@RegDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@UpdateBy", MenuFormV2.UserForNextForm);
                                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("ទិន្នន័យត្រូវបានរក្សាទុកដោយជោគជ័យ!", "ជោគជ័យ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("មានបព្ហា! សូមទាក់ទងទៅអាយធី (ផានន្ទ)! \n" + ex.Message, "Something Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        con.con.Close();
                        Cursor = Cursors.Default;
                    }
                    else
                    {
                        Cursor = Cursors.WaitCursor;
                        try
                        {
                            con.con.Open();
                            string queryupdate = "UPDATE tbMasterSemiPrint SET Name=@Name, Pcs=@Pcs, QtyBox=@QtyBox, UpdateBy=@UpdateBy, UpdateDate=@UpdateDate WHERE Code=@Code AND SysNo = @sysno";
                            SqlCommand cmd = new SqlCommand(queryupdate, con.con);
                            cmd.Parameters.AddWithValue("@Code", txtcode.Text.Trim());
                            cmd.Parameters.AddWithValue("@SysNo", txtsysno.Text.Trim());
                            cmd.Parameters.AddWithValue("@Name", txtname.Text.Trim());
                            cmd.Parameters.AddWithValue("@Pcs", txtpcs.Text.Trim());
                            cmd.Parameters.AddWithValue("@QtyBox", txtbox.Text.Trim());
                            cmd.Parameters.AddWithValue("@UpdateBy", MenuFormV2.UserForNextForm);
                            cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("ទិន្នន័យត្រូវបានរក្សាទុកដោយជោគជ័យ!", "ជោគជ័យ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("មានបព្ហា! សូមទាក់ទងទៅអាយធី (ផានន្ទ)! \n" + ex.Message, "Something Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        con.con.Close();
                        Cursor = Cursors.Default;
                       
                    }
                    btnSearch.PerformClick();
                }
            }
            else
            {
                MessageBox.Show("សូមបំពេញពត៌មានទាំងអស់ !", "សូមពិនិត្យ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                dgvMst.Rows.Clear();
                con.con.Open();
                string where = "";
                DataTable dtcond = new DataTable();
                DataTable dtsearch = new DataTable();
                dtcond.Columns.Add("Val");
                if (searchcode.Text.Trim() != "")
                {
                  dtcond.Rows.Add("Code LIKE '%" + searchcode.Text.Trim() + "%'");
                }
                if (searchname.Text.Trim() != "")
                {
                    dtcond.Rows.Add("Name LIKE '%" + searchname.Text.Trim() + "%'");
                }
                foreach (DataRow dr in dtcond.Rows)
                {
                    if (where == "")
                    {
                        where = "WHERE " + dr["Val"].ToString();
                    }
                    else
                    {
                        where += " AND " + dr["Val"].ToString();
                    }
                }
                string queryselect = "SELECT * FROM tbMasterSemiPrint " + where;
                SqlDataAdapter sda = new SqlDataAdapter(queryselect, con.con);
                sda.Fill(dtsearch);

                if (dtsearch.Rows.Count > 0)
                {
                    foreach (DataRow row in dtsearch.Rows)
                    {
                        dgvMst.Rows.Add();
                        dgvMst.Rows[dgvMst.Rows.Count -1].Cells["sysno"].Value = row["SysNo"].ToString();
                        dgvMst.Rows[dgvMst.Rows.Count -1].Cells["semicode"].Value = row["Code"].ToString();
                        dgvMst.Rows[dgvMst.Rows.Count -1].Cells["seminame"].Value = row["Name"].ToString();
                        dgvMst.Rows[dgvMst.Rows.Count -1].Cells["semipcs"].Value = row["Pcs"].ToString();
                        dgvMst.Rows[dgvMst.Rows.Count -1].Cells["labelbox"].Value = row["QtyBox"].ToString();
                        dgvMst.Rows[dgvMst.Rows.Count -1].Cells["regby"].Value = row["RegBy"].ToString();
                        dgvMst.Rows[dgvMst.Rows.Count -1].Cells["regdate"].Value = row["RegDate"].ToString();
                        dgvMst.Rows[dgvMst.Rows.Count -1].Cells["upby"].Value = row["UpdateBy"].ToString();
                        dgvMst.Rows[dgvMst.Rows.Count -1].Cells["update"].Value = row["UpdateDate"].ToString();

                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Please contact to Phanun for support! \n" + ex.Message, "Something Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dgvMst.ClearSelection();
            con.con.Close();
            Cursor = Cursors.Default;
        }

        private void BtnCancel_MouseLeave(object sender, EventArgs e)
        {
            btnCancel.BackColor = Color.White;
            piccancel.BackColor = Color.White;
        }

        private void BtnCancel_MouseEnter(object sender, EventArgs e)
        {
            btnCancel.BackColor = Color.SkyBlue;
            piccancel.BackColor = Color.SkyBlue;
        }

        private void BtnSave_MouseLeave(object sender, EventArgs e)
        {
            btnSave.BackColor = Color.White;
            picsave.BackColor = Color.White;
        }

        private void BtnSave_MouseEnter(object sender, EventArgs e)
        {
            btnSave.BackColor = Color.SkyBlue;
            picsave.BackColor = Color.SkyBlue;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            gpaddup.Visible= false;
          
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            btnSave.Text = "Update";
            gpaddup.Visible = true;
            gpaddup.Text = "Update";
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            btnSave.Text = "Add";
            gpaddup.Visible = true;
            gpaddup.Text = "Add";
        }

    }
}
