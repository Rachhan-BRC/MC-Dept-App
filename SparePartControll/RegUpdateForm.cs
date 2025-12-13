
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
using System.Windows.Navigation;

namespace MachineDeptApp
{
    public partial class RegUpdateForm : Form
    {
        SQLConnect con = new SQLConnect();
        Dictionary<Control, object> originalValues = new Dictionary<Control, object>();
        string AddOrUpdate;
        DataGridView dgvMain;
        string dept = "MC";
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
            double upcn = 0;
            double upjp = 0;
            string Code = txtcode.Text;
            up = chkup.Checked && double.TryParse(txtunitprice.Text.Trim(), out var val) ? val : 0;
            upcn = chkupcn.Checked && double.TryParse(txtunitprice.Text.Trim(), out var val1) ? val1 : 0;
            upjp = chkupjp.Checked && double.TryParse(txtunitprice.Text.Trim(), out var val2) ? val2 : 0;

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
                    foreach (DataGridViewRow row in dgvMain.Rows)
                    {
                        string dgvcode = row.Cells["code"].Value?.ToString();
                        string textcode = txtcode.Text.Trim();
                        if (dgvcode == textcode)
                        {
                            MessageBox.Show("Code already exists. Please use a different code.", "Duplicate Code", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
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
                        cmd.Parameters.AddWithValue("@dept", dept);
                        cmd.Parameters.AddWithValue("@cate", cate);
                        cmd.Parameters.AddWithValue("@maker", maker);
                        cmd.Parameters.AddWithValue("@usefor", use);
                        cmd.Parameters.AddWithValue("@sup", sup);
                        cmd.Parameters.AddWithValue("@safe", safe);
                        cmd.Parameters.AddWithValue("@LT", LT);
                        cmd.Parameters.AddWithValue("@moq", moq); 
                        cmd.Parameters.AddWithValue("@up", up);
                        cmd.Parameters.AddWithValue("@upcn", upcn);
                        cmd.Parameters.AddWithValue("@upjp", upcn);
                        cmd.Parameters.AddWithValue("@rm", "New Item" + date);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Register Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvMain.Rows.Add();
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["code"].Value = Code;
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["Pno"].Value = pnum;
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["Pname"].Value = pname;
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["category"].Value = cate;
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["maker"].Value = maker;
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["usefor"].Value = use;
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["supplier"].Value = sup;
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["safetystock"].Value = safe;
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["LTWeek"].Value = LT;
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["moq"].Value = moq;
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["unitprice"].Value = up;
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["unitpricecn"].Value = upcn;
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["unitpricejp"].Value = upjp;
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
                    int success = 0;
                    foreach (DataGridViewRow row in dgvMain.Rows)
                    {
                        string dgvcode = row.Cells["code"].Value.ToString();
                        string textcode = txtcode.Text.Trim().ToString();
                        if (dgvcode == textcode)
                        {
                            string Up = "";
                            double upUPDATE = 0;
                            if (chkup.Checked)
                            {
                                Up = "Unit_Price ";
                                upUPDATE = Convert.ToDouble(txtunitprice.Text.Trim());
                            }
                            else if (chkupcn.Checked)
                            {
                                Up = "Unit_Price_CN ";
                                upUPDATE = Convert.ToDouble(txtunitprice.Text.Trim());
                            }
                            else if (chkupjp.Checked)
                            {
                                Up = "Unit_Price_JP ";
                                upUPDATE = Convert.ToDouble(txtunitprice.Text.Trim());
                            }
                            DateTime date2 = DateTime.Now;
                            string rm = "Update: " + date2.ToString("dd-MM-yyyy");
                            Cursor = Cursors.WaitCursor;

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
                       " " + Up + " = @up, " +
                       "Remark = @rm " +
                       "WHERE Code = @code";
                            SqlCommand cmd = new SqlCommand(query, con.con);
                            cmd.Parameters.AddWithValue("@code", Code);
                            cmd.Parameters.AddWithValue("@pno", pnum);
                            cmd.Parameters.AddWithValue("@pname", pname);
                            cmd.Parameters.AddWithValue("@dept", dept);
                            cmd.Parameters.AddWithValue("@cate", cate);
                            cmd.Parameters.AddWithValue("@maker", maker);
                            cmd.Parameters.AddWithValue("@usefor", use);
                            cmd.Parameters.AddWithValue("@sup", sup);
                            cmd.Parameters.AddWithValue("@safe", safe);
                            cmd.Parameters.AddWithValue("@LT", LT);
                            cmd.Parameters.AddWithValue("@moq", moq);
                            cmd.Parameters.AddWithValue("@rm", rm);
                            cmd.Parameters.AddWithValue("@up", upUPDATE);
                            cmd.ExecuteNonQuery();
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
                            con.con.Close();
                            Cursor = Cursors.Default;
                            MessageBox.Show(" Updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            success++;
                        }
                    }
                    if (success>0)
                    {
                        this.Close();
                    }
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
            chkup.Checked = true;
            if (AddOrUpdate != "Add")
            {
                this.Text = "Update Master";
                btnSave.Text = "Update";

                txtcode.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["code"].Value.ToString();
                txtcode.Enabled = false;
                txtpnumber.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["Pno"].Value.ToString();
                txtpname.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["Pname"].Value.ToString();
                cbcategory.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["category"].Value.ToString();
                cbmaker.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["maker"].Value.ToString();
                cbusefor.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["usefor"].Value.ToString();
                cbsupplier.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["supplier"].Value.ToString();
                txtsafetystock.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["safetystock"].Value.ToString(); 
                txtleadtime.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["LTWeek"].Value.ToString();
                txtmoq.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["moq"].Value.ToString();
                 txtunitprice.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["unitprice"].Value.ToString();
            }
        }
    }
}
