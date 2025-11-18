
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
    public partial class RegUpdateForm : Form
    {
        SQLConnect con = new SQLConnect();
        string AddOrUpdate;
        DataGridView dgvMain;
        public RegUpdateForm(string type1, DataGridView dgv1)
        {
            con.Connection();
            InitializeComponent();
            dgvMain = dgv1;
            AddOrUpdate = type1;
            this.Load += RegUpdateForm_Load;
            this.btnClear.Click += BtnClear_Click;
            this.btnSave.Click += BtnSave_Click;
            this.cbcodeprefix.SelectedIndexChanged += Cbcodeprefix_SelectedIndexChanged;
            //txt
            this.txtmoq.KeyPress += Txtmoq_KeyPress;
            this.txtsafetystock.KeyPress += Txtsafetystock_KeyPress;
            this.txtunitprice.KeyPress += Txtunitprice_KeyPress;
            this.txtleadtime.KeyPress += Txtleadtime_KeyPress;
            
        }

        private void Txtleadtime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Block the key
            }
        }
        private void Txtunitprice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Block the key
            }
        }
        private void Txtsafetystock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Block the key
            }
        }
        private void Txtmoq_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Block the key
            }
        }
        private void Cbcodeprefix_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            DataTable dtselect = new DataTable();
            string Code = "";
            try
            {
                con.con.Open();
                string SQLQuery = "DECLARE @LastID VARCHAR(6); " +
                    " DECLARE @Number int; " +
                    " DECLARE @NewID VARCHAR(6) ; " +
                    " DECLARE @PreFix VARCHAR(6) ='"+ cbcodeprefix.Text.Trim().ToString()+"'; " +
                    " Select @LastID = Max(Code) from MstMCSparePart where Code like @PreFix + '%' AND ISNUMERIC(SUBSTRING(Code, LEN(@PreFix) + 1, LEN(Code))) = 1 " +
                    " IF @LastID IS NULL    BEGIN        SET @NewID = @PreFix+'0001' " +
                    " END " +
                    " ELSE " +
                    "    BEGIN  SET @Number = CAST(right(@LastID,4) AS INT)+1" +
                    "  SET @NewID = CONCAT(@PreFix, format(@Number, '0000')) " +
                    " END " +
                    " Select @NewID as NextCode ";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, con.con);
                sda.Fill(dtselect);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error while select code !\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.con.Close();
            foreach (DataRow row in dtselect.Rows)
            {
                 Code = row["NextCode"].ToString();
            }
            Cursor = Cursors.Default;
            txtcode.Text = Code;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DateTime date1 = DateTime.Now;
            string date = date1.ToString("dd-MM-yyyy");
            string code =txtcode.Text;
            string pnum = txtpnumber.Text;
            string pname = txtpname.Text;
            string use = cbusefor.Text;
            string cate = cbcategory.Text;
            string sup = cbsupplier.Text;
            string maker = cbmaker.Text;    
            string LT = txtleadtime.Text;
            string safe = txtsafetystock.Text;
            string moq = txtmoq.Text;   
            double up = 0;
            string Code = txtcode.Text;
            if (chkup.Checked == true)
            {
                up = Convert.ToDouble(txtunitprice.Text);
            }
            double upcn = 0;
            if (chkupcn.Checked == true)
            {
                upcn = Convert.ToDouble(txtunitprice.Text);
            }
            double upjp = 0;
            if (chkupjp.Checked == true)
            {
                upjp = Convert.ToDouble(txtunitprice.Text);
            }
            if (code.Trim() != "" &&
                 pnum.Trim() != "" &&
                    pname.Trim() != "" &&
                    use.Trim() != "" &&
                    cate.Trim() != "" &&
                    sup.Trim() != "" &&
                    maker.Trim() != "" &&
                    LT.Trim() != "" &&
                    safe.Trim() != "" &&
                    moq.Trim() != "")
            {
                if (AddOrUpdate == "Add")
                {
                    
                    
                    Cursor = Cursors.WaitCursor;
                    try
                    {
                        con.con.Open();
                        string query = "INSERT INTO MstMCSparePart (Code, Part_No, Part_Name, Dept, Category, Maker, Use_For, Supplier, Safety_Stock, Lead_Time_Week, MOQ, Unit_Price, Unit_Price_CN, Unit_Price_JP, Remark) " +
                                               " VALUES (@code, @pno, @pname,  @dept, @cate, @maker, @usefor, @sup, @safe, @LT, @moq, @up, @upcn, @upjp, @rm)";
                        SqlCommand cmd = new SqlCommand(query, con.con);
                        cmd.Parameters.AddWithValue("@code", Code);
                        cmd.Parameters.AddWithValue("@pno", pnum);
                        cmd.Parameters.AddWithValue("@pname", pname);
                        cmd.Parameters.AddWithValue("@dept", "MC");
                        cmd.Parameters.AddWithValue("@cate", cate);
                        cmd.Parameters.AddWithValue("@maker", maker);
                        cmd.Parameters.AddWithValue("@usefor", use);
                        cmd.Parameters.AddWithValue("@sup", sup);
                        cmd.Parameters.AddWithValue("@safe", safe);
                        cmd.Parameters.AddWithValue("@LT", LT);
                        cmd.Parameters.AddWithValue("@moq", moq); 
                        cmd.Parameters.AddWithValue("@up", up);
                        cmd.Parameters.AddWithValue("@upcn", upcn);
                        cmd.Parameters.AddWithValue("@upjp", upjp);
                        cmd.Parameters.AddWithValue("@rm", "New Item" + date);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Register Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvMain.Rows.Add(code, pnum, pname, cate, maker, use, sup, safe, LT, moq, up, upcn, upjp);
                        dgvMain.Rows[dgvMain.Rows.Count - 1].HeaderCell.Value = dgvMain.Rows.Count.ToString();
                        dgvMain.ClearSelection();
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("Something went wrong!\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    con.con.Close();
                    Cursor = Cursors.Default;

                    //Update the DataGridView
                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["code"].Value = Code;
                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["Pno"].Value = pnum;
                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["Pname"].Value = pname;
                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["category"].Value = cate;
                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["maker"].Value = maker;
                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["usefor"].Value = use;
                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["supplier"].Value = sup;
                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["safetystock"].Value = safe;
                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["LTWeek"].Value = LT;
                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["moq"].Value = moq;
                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["unitprice"].Value = up;
                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["unitpricecn"].Value = upcn;
                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["unitpricejp"].Value = upjp;
                    dgvMain.ClearSelection();
                    dgvMain.CurrentCell = null;
                }
                //Update
                else
                {
                    DateTime date2 = DateTime.Now;
                    string rm = "Update: " + date2.ToString("dd-MM-yyyy");
                    Cursor = Cursors.WaitCursor;
                    int SysNo = Convert.ToInt32(dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["SysNo"].Value);

                    con.con.Open();
                    string query = "UPDATE MstMCSparePart SET " +
               "Part_No = @pno, " +
               "Part_Name = @pname, " +
               "Dept = @dept, " +
               "Category = @cate, " +
               "Maker = @maker, " +
               "Use_For = @usefor, " +
               "Supplier = @sup, " +
               "Safety_Stock = @safe, " +
               "Lead_Time_Week = @LT, " +
               "MOQ = @moq, " +
               "Unit_Price = @up, " +
               "Unit_Price_CN = @upcn, " +
               "Unit_Price_JP = @upjp, " +
               "Remark = @rm " +
               "WHERE Code = @code";
                    SqlCommand cmd = new SqlCommand(query, con.con);
                    cmd.Parameters.AddWithValue("@code", Code);
                    cmd.Parameters.AddWithValue("@pno", pnum);
                    cmd.Parameters.AddWithValue("@pname", pname);
                    cmd.Parameters.AddWithValue("@dept", "MC");
                    cmd.Parameters.AddWithValue("@cate", cate);
                    cmd.Parameters.AddWithValue("@maker", maker);
                    cmd.Parameters.AddWithValue("@usefor", use);
                    cmd.Parameters.AddWithValue("@sup", sup);
                    cmd.Parameters.AddWithValue("@safe", safe);
                    cmd.Parameters.AddWithValue("@LT", LT);
                    cmd.Parameters.AddWithValue("@moq", moq);
                    cmd.Parameters.AddWithValue("@up", up);
                    cmd.Parameters.AddWithValue("@upcn", upcn);
                    cmd.Parameters.AddWithValue("@upjp", upjp);
                    cmd.Parameters.AddWithValue("@rm", rm);

                    cmd.ExecuteNonQuery();

                    //Update the DataGridView 
                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["code"].Value = Code;
                    dgvMain.ClearSelection();
                    dgvMain.CurrentCell = null;

                    Cursor = Cursors.Default;
                    MessageBox.Show(" Updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            else
                MessageBox.Show("Please enter all data.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtcode.Text = "";
            txtpname.Text = "";
            txtpnumber.Text = "";
            txtleadtime.Text =  "";
            txtmoq.Text = "";
            txtsafetystock.Text = "";
            txtunitprice.Text = "";
            cbcodeprefix.Text = "";
            cbcategory.Text = "";
            cbmaker.Text = "";
            cbsupplier.Text = "";
            cbusefor.Text = "";
        }

        private void RegUpdateForm_Load(object sender, EventArgs e)
        {
            if (AddOrUpdate != "Add")
            {
               
                this.Text = "Update Master";
                btnSave.Text = "Update";
                txtcode.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["code"].Value.ToString();
                txtpnumber.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["Pno"].Value.ToString();
                txtpname.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["Pname"].Value.ToString();
                cbcategory.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["category"].Value.ToString();
                cbmaker.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["maker"].Value.ToString();
                cbusefor.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["usefor"].Value.ToString();
                cbsupplier.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["supplier"].Value.ToString();
                txtsafetystock.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["safetystock"].Value.ToString();
                txtleadtime.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["LTWeek"].Value.ToString();
                txtmoq.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["moq"].Value.ToString();
                double up = Convert.ToDouble(dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["unitprice"].Value);
                double upcn = Convert.ToDouble(dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["unitpricecn"].Value);
                double upjp = Convert.ToDouble(dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["unitpricejp"].Value);
                if (up > 0)
                {
                    txtunitprice.Text = up.ToString();
                }
                if (upcn > 0)
                {
                    txtunitprice.Text = up.ToString();
                }
                if (upcn > 0)
                {
                    txtunitprice.Text = up.ToString();
                }

            }
        }
    }
}
