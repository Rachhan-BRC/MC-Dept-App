using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MachineDeptApp
{
    public partial class MstStopInfo : Form
    {
        SQLConnect con = new SQLConnect();
        public MstStopInfo()
        {
            this.con.Connection();
            InitializeComponent();
            this.btnSwitch.Click += BtnSwitch_Click;
            this.btnSearchAdd.Click += BtnSearchAdd_Click;
            this.btnSearchAdd.MouseEnter += BtnSearchAdd_MouseEnter;
            this.btnSearchAdd.MouseLeave += BtnSearchAdd_MouseLeave;
            this.picadd.MouseEnter += BtnSearchAdd_MouseEnter;
            this.picadd.MouseLeave += BtnSearchAdd_MouseLeave;
            this.picsearch.MouseEnter += BtnSearchAdd_MouseEnter;
            this.picsearch.MouseLeave += BtnSearchAdd_MouseLeave;
            this.picadd.Click += BtnSearchAdd_Click;
            this.picsearch.Click += BtnSearchAdd_Click;
            this.dgvData.CellClick += DgvData_CellClick;
        }
        private void DgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvData.Columns[e.ColumnIndex].Name == "delete")
            {
                dgvData.ClearSelection();
                DialogResult ask = MessageBox.Show("Are you sure you want to delete this?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask == DialogResult.Yes)
                {
                    con.con.Open();
                    try
                    {
                        string querydelete = "DELETE FROM tbNGTypeMst WHERE SysNo = '" + dgvData.Rows[e.RowIndex].Cells["SysNo"].Value.ToString() + "'";
                        SqlCommand cmd = new SqlCommand(querydelete, con.con);
                        cmd.ExecuteNonQuery();
                        dgvData.Rows.RemoveAt(e.RowIndex);
                        MessageBox.Show("Row deleted successfully.", "Delete Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Cannot delete this row. \nPlease contact Phanun for support.", "Something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    con.con.Close();
                    search();
                    if (dgvData.Rows.Count > 0)
                    {
                        dgvData.Rows[dgvData.Rows.Count - 1].Cells["sysno"].Selected = true;
                    }
                    dgvData.ClearSelection();
                }
            }
        }

        private void BtnSearchAdd_MouseLeave(object sender, EventArgs e)
        {
            picsearch.BackColor = Color.White;
            btnSearchAdd.BackColor = Color.White;
            picadd.BackColor = Color.White;
        }

        private void BtnSearchAdd_MouseEnter(object sender, EventArgs e)
        {
            picsearch.BackColor = Color.SkyBlue;
            btnSearchAdd.BackColor = Color.SkyBlue;
            picadd.BackColor = Color.SkyBlue;

        }

        private void BtnSearchAdd_Click(object sender, EventArgs e)
        {
            if (btnSearchAdd.Text == "បន្ថែម / Add")
            {
                if (txttype.Text.Trim() != "" && cbfunct.Text.Trim() != "")
                {
                    DialogResult ask = MessageBox.Show("Are you sure you want to add this?", "Confirm Add", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ask == DialogResult.Yes)
                    {
                        try
                        {
                            int cb = 0;
                            if (cbfunct.SelectedIndex == 1)
                            {
                                cb = 1;
                            }
                            else if (cbfunct.SelectedIndex == 2)
                            {
                                cb = 2;
                            }
                            con.con.Open();
                            string queryadd = "INSERT INTO tbNGTypeMst (Name, Type, RegDate, RegBy, UpdateDate, UpdateBy, Funct) VALUES (@Name, @Type, @RegDate, @RegBy, @UpdateDate, @UpdateBy, @Funct)";
                            SqlCommand cmd = new SqlCommand(queryadd, con.con);
                            cmd.Parameters.AddWithValue("@Name", txttype.Text.Trim());
                            cmd.Parameters.AddWithValue("@Type", "Stop Info");
                            cmd.Parameters.AddWithValue("@RegDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@RegBy", MenuFormV2.UserForNextForm);
                            cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@UpdateBy", MenuFormV2.UserForNextForm);
                            cmd.Parameters.AddWithValue("@Funct", cb);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Row added successfully.", "Add Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txttype.Text = "";
                            cbfunct.SelectedIndex = 0;
                            txttype.Focus();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Cannot add this row. \nPlease contact Phanun for support. \n " + ex.Message, "Something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        con.con.Close();
                        search();
                        if (dgvData.Rows.Count > 0)
                        {
                            dgvData.Rows[dgvData.Rows.Count - 1].Cells["sysno"].Selected = true;
                        }
                        
                        dgvData.ClearSelection();
                    }
                }
                else
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                try
                {
                    search();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot search. \nPlease contact Phanun for support. \n " + ex.Message, "Something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void BtnSwitch_Click(object sender, EventArgs e)
        {
            if (btnSearchAdd.Text == "ស្វែងរក / Search")
            {
                picadd.BringToFront();
                btnSearchAdd.Text = "បន្ថែម / Add";
            }
            else
            {
                picsearch.BringToFront();
                btnSearchAdd.Text = "ស្វែងរក / Search";
            }

        }
        private void search()
        {
            dgvData.Rows.Clear();
            con.con.Open();
            string where = "";
            DataTable dtcond = new DataTable();
            DataTable dtsearch = new DataTable();
            dtcond.Columns.Add("Val");
            if (txttype.Text.Trim() != "")
            {
                dtcond.Rows.Add("Name Like '%" + txttype.Text.Trim() + "%'");
            }
            if (cbfunct.Text.Trim() != "")
            {
                dtcond.Rows.Add("Type Like '%" + cbfunct.Text.Trim() + "%'");
            }
            foreach (DataRow row in dtcond.Rows)
            {
                if (where == "")
                {
                    where = " AND " + row["Val"].ToString();
                }
                else
                {
                    where += " AND " + row["Val"].ToString();
                }
            }
            string querysearch = "'";
            querysearch = "SELECT * FROM tbNGTypeMst WHERE Funct = 1 AND Type = 'Stop Info'" + where;
            SqlDataAdapter sda = new SqlDataAdapter(querysearch, con.con);
            sda.Fill(dtsearch);
            foreach (DataRow row in dtsearch.Rows)
            {
                dgvData.Rows.Add();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["sysno"].Value = row["SysNo"].ToString();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["type"].Value = row["Name"].ToString();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["funct"].Value = row["Type"].ToString();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["regdate"].Value = row["RegDate"].ToString();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["regby"].Value = row["RegBy"].ToString();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["update"].Value = row["UpdateDate"].ToString();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["upby"].Value = row["UpdateBy"].ToString();
            }
            con.con.Close();
            dgvData.ClearSelection();
        }
    }

}
