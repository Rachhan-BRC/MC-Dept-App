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

namespace MachineDeptApp
{
    public partial class SDMstPic : Form
    {
        SQLConnect con = new SQLConnect();
        public SDMstPic()
        {
            con.Connection();
            InitializeComponent();
            this.Load += SDMstPic_Load;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnCancel.Click += BtnCancel_Click;
            this.btnAdd.Click += BtnAdd_Click;
            this.btnSave.Click += BtnSave_Click;
            this.txtlbendno.TextChanged += Txtlbendno_TextChanged;
            this.txtlbstartno.TextChanged += Txtlbstartno_TextChanged;
            this.txtpic.TextChanged += Txtpic_TextChanged;
        }

        private void Txtpic_TextChanged(object sender, EventArgs e)
        {
            if (txtpic.Text.Trim() != "" && txtlbstartno.Text.Trim() != "" && txtlbendno.Text.Trim() != "")
            {
                btnSave.Enabled = true;
                btnSave.BringToFront();
            }
        }

        private void Txtlbstartno_TextChanged(object sender, EventArgs e)
        {
            if (txtpic.Text.Trim() != "" && txtlbstartno.Text.Trim() != "" && txtlbendno.Text.Trim() != "")
            {
                btnSave.Enabled = true;
                btnSave.BringToFront();
            }
        }

        private void Txtlbendno_TextChanged(object sender, EventArgs e)
        {
            if (txtpic.Text.Trim() != "" && txtlbstartno.Text.Trim() != "" && txtlbendno.Text.Trim() != "")
            {
                btnSave.Enabled = true;
                btnSave.BringToFront();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            Cursor = Cursors.WaitCursor;
            string pic = txtpic.Text.Trim();
            string lbstartno = txtlbstartno.Text.Trim();
            string lbendno = txtlbendno.Text.Trim();
            if (pic != "" && lbendno != "" && lbstartno != "")
            {
                try
                {
                    con.con.Open();
                    string query = "DECLARE @Start int =  '"+lbstartno+"' DECLARE @End int =  '"+lbendno+"' " +
                        "SELECT *, CONCAT(LabelNoStart, '~',LabelNoEnd) AS Combine FROM tbMstInventoryPic " +
                        "WHERE @Start BETWEEN LabelNoStart AND LabelNoEnd " +
                        "OR @End BETWEEN LabelNoStart AND LabelNoEnd " +
                        "OR LabelNoStart BETWEEN @Start AND @End " +
                        "OR LabelNoEnd BETWEEN @Start AND @End " +
                        "ORDER BY LabelNoStart ASC";
                    SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("លេខឡាប៊ែលនេះមានក្នុងជួររួចហើយ!",  "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        con.con.Close();
                        Cursor = Cursors.Default;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាបច្ចេកទេស សូមទាក់ទងទៅ IT (Phanun) 2" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.con.Close();
                    Cursor = Cursors.Default;
                    return;
                }
                try
                {
                    string insertQuery = "INSERT INTO tbMstInventoryPic " +
                        "(Location, PIC, LabelNoStart, LabelNoEnd, RegBy, RegDate, UpdateBy, UpdateDate) " +
                        "VALUES (@location, @pic, @labelNoStart, @labelNoEnd, @regBy, @regDate, @updateBy, @updateDate)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, con.con);
                    insertCmd.Parameters.AddWithValue("@location", "SD");
                    insertCmd.Parameters.AddWithValue("@pic", pic);
                    insertCmd.Parameters.AddWithValue("@labelNoStart", Convert.ToInt32(lbstartno));
                    insertCmd.Parameters.AddWithValue("@labelNoEnd", Convert.ToInt32(lbendno));
                    insertCmd.Parameters.AddWithValue("@regBy", MenuFormV2.UserForNextForm);
                    insertCmd.Parameters.AddWithValue("@regDate", DateTime.Now);
                    insertCmd.Parameters.AddWithValue("@updateBy", MenuFormV2.UserForNextForm);
                    insertCmd.Parameters.AddWithValue("@updateDate", DateTime.Now);
                    dgvPic.Rows.Add();
                    dgvPic.Rows[dgvPic.RowCount - 1].Cells["location"].Value = "SD";
                    dgvPic.Rows[dgvPic.RowCount - 1].Cells["pic"].Value = pic;
                    dgvPic.Rows[dgvPic.RowCount - 1].Cells["lbstartno"].Value = lbstartno;
                    dgvPic.Rows[dgvPic.RowCount - 1].Cells["lbendno"].Value = lbendno;
                    dgvPic.Rows[dgvPic.RowCount - 1].Cells["regby"].Value = MenuFormV2.UserForNextForm;
                    dgvPic.Rows[dgvPic.RowCount - 1].Cells["regdate"].Value = DateTime.Now;
                    dgvPic.Rows[dgvPic.RowCount - 1].Cells["updateby"].Value = MenuFormV2.UserForNextForm;
                    dgvPic.Rows[dgvPic.RowCount - 1].Cells["UpdateDate"].Value = DateTime.Now;
                    int result = insertCmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("បានរក្សាទុកដោយជោគជ័យ!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtpic.Clear();
                        txtlbstartno.Clear();
                        txtlbendno.Clear();
                        panelRegUp.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("រក្សាទុកមិនជោគជ័យ សូមព្យាយាមម្តងទៀត!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាបច្ចេកទេស សូមទាក់ទងទៅ IT (Phanun) 3" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.con.Close();
                    Cursor = Cursors.Default;
                    return;
                }

                con.con.Close();
                Cursor = Cursors.Default;
            }

        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            panelRegUp.Visible = true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            panelRegUp.Visible = false;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            dgvPic.Rows.Clear();
            DataTable dt  = new DataTable();
            Cursor = Cursors.WaitCursor;
            try
            {
                string query = "SELECT * FROM tbMstInventoryPic";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string loc = row["Location"].ToString();
                        string pic = row["PIC"].ToString();
                        int lbnostart = Convert.ToInt32(row["LabelNoStart"].ToString());
                        int lbnoend = Convert.ToInt32(row["LabelNoEnd"]);
                        string regby = row["RegBy"].ToString();
                        DateTime regdate = Convert.ToDateTime(row["RegDate"]);
                        string updateby = row["UpdateBy"].ToString();
                        DateTime updatedate = Convert.ToDateTime(row["UpdateDate"]);
                        dgvPic.Rows.Add();
                        dgvPic.Rows[dgvPic.RowCount - 1].Cells["location"].Value = loc;
                        dgvPic.Rows[dgvPic.RowCount - 1].Cells["pic"].Value = pic;
                        dgvPic.Rows[dgvPic.RowCount - 1].Cells["lbstartno"].Value = lbnostart;
                        dgvPic.Rows[dgvPic.RowCount - 1].Cells["lbendno"].Value = lbnoend;
                        dgvPic.Rows[dgvPic.RowCount - 1].Cells["regby"].Value = regby;
                        dgvPic.Rows[dgvPic.RowCount - 1].Cells["regdate"].Value = regdate;
                        dgvPic.Rows[dgvPic.RowCount - 1].Cells["updateby"].Value = updateby;
                        dgvPic.Rows[dgvPic.RowCount - 1].Cells["UpdateDate"].Value = updatedate;
                    }
                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហាបច្ចេកទេស សូមទាក់ទងទៅ IT (Phanun) 1" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor = Cursors.Default;
        }

        private void SDMstPic_Load(object sender, EventArgs e)
        {
            panelRegUp.Visible = false;
        }
    }
}
