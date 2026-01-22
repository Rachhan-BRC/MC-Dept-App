using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Windows.Markup.Localizer;

namespace MachineDeptApp
{

    public partial class StockINOut : Form
    {
        SQLConnect con = new SQLConnect();
        DataTable dtMst = new DataTable();
        double BeforeEdit = 0;


        string Dept = "MC", RemarkBeforeEdit = "", ErrorText = "";

        public StockINOut()
        {
            InitializeComponent();
            this.con.Connection();
            this.Shown += StockINOut_Shown;
            this.rdbStockOut.CheckedChanged += RdbStockOut_CheckedChanged;

            //btn
            this.btnAdd.MouseEnter += BtnAdd_MouseEnter;
            this.btnAdd.MouseLeave += BtnAdd_MouseLeave;
            this.btnAddPic.MouseEnter += BtnAdd_MouseEnter;
            this.btnAddPic.MouseLeave += BtnAdd_MouseLeave;
            this.btnAdd.Click += BtnAdd_Click;
            this.btnAddPic.Click += BtnAdd_Click;
            this.btnShowSearch.Click += BtnShowSearch_Click;
            this.btnSave.Click += BtnSave_Click;
            this.btnNew.Click += BtnNew_Click;
            //Dgv
            this.dgvSearch.LostFocus += DgvSearch_LostFocus;
            this.dgvSearch.CellClick += DgvSearch_CellClick;
            this.dgvInput.CellFormatting += DgvInput_CellFormatting;
            this.dgvInput.CellClick += DgvInput_CellClick;
            this.dgvInput.RowsAdded += DgvInput_RowsAdded;
            this.dgvInput.RowsRemoved += DgvInput_RowsRemoved;
            this.dgvInput.CellBeginEdit += DgvInput_CellBeginEdit;
            this.dgvInput.CellEndEdit += DgvInput_CellEndEdit;

            //txt
            this.txtSearch.Enter += TxtSearch_Enter;
            this.txtSearch.Leave += TxtSearch_Leave;
            this.txtSearch.TextChanged += TxtSearch_TextChanged;
            this.txtSearch.LostFocus += DgvSearch_LostFocus;
            this.txtCode.KeyDown += TxtCode_KeyDown;
            this.txtQty.KeyPress += TxtQty_KeyPress;
            this.txtQty.KeyDown += TxtQty_KeyDown;
            this.txtRemark.KeyDown += TxtRemark_KeyDown;

        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            if (dgvInput.Rows.Count > 0)
            {
                DialogResult ask = MessageBox.Show("Are you want to clear all data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask == DialogResult.No)
                    return;
                else
                    dgvInput.Rows.Clear();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DialogResult DR = MessageBox.Show("Are you want to save this data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DR == DialogResult.No)
            {
                return;
            }
            Cursor = Cursors.WaitCursor;
            string transno = "TR-A0000001";
            DataTable dtTran = new DataTable();
            try
            {
                con.con.Open();
                string querySelect = "SELECT MAX(TransNo) AS TransNo FROM SparePartTrans";
                SqlDataAdapter sda = new SqlDataAdapter(querySelect, con.con);
                sda.Fill(dtTran);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while selecting transno data " + ex.Message, "Error TransNo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.con.Close();
            if (dtTran.Rows[0]["TransNo"] != DBNull.Value)
            {
                string lastTransNo = dtTran.Rows[0]["TransNo"].ToString();
                int num = int.Parse(lastTransNo.Substring(4));
                transno = "TR-A" + (num + 1).ToString("D7");
            }
            else
            {
                transno = "TR-A0000001";
            }
            try
            {
                    con.con.Open();
                    foreach (DataGridViewRow row in dgvInput.Rows)
                    {
                        if (row.Cells["ColCode"].Value != null &&
                            row.Cells["ColPartNo"].Value != null &&
                            row.Cells["ColPartName"].Value != null)
                        {
                            string code = row.Cells["ColCode"].Value.ToString();
                            string partno = row.Cells["ColPartNo"].Value.ToString();
                            string partname = row.Cells["ColPartName"].Value.ToString();
                            decimal stockin = Convert.ToDecimal(row.Cells["ColStockIn"].Value ?? 0);
                            decimal stockout = Convert.ToDecimal(row.Cells["ColStockOut"].Value ?? 0);
                            decimal stockvalue = stockin - stockout;
                            DateTime regdate = DateTime.Now;
                            DateTime updatedate = DateTime.Now;
                            string pic = MenuFormV2.UserForNextForm;
                            string remark = row.Cells["ColRemark"].Value?.ToString() ?? "";

                            string query = "INSERT INTO SparePartTrans (TransNo, Code, Part_No, Part_Name, Dept, Stock_In, Stock_Out, Stock_Value, Stock_Amount, PO_No, Invoice, Status, RegDate, UpdateDate, PIC, Remark) " +
                                           "VALUES (@transno, @code, @partno, @partname, @dept, @stockin, @stockout, @stockvalue, @stockamount, @pono, @invoice, @status, @regdate, @updatedate, @pic, @remark)";
                            using (SqlCommand cmd = new SqlCommand(query, con.con))
                            {
                                cmd.Parameters.AddWithValue("@transno", transno);
                                cmd.Parameters.AddWithValue("@code", code);
                                cmd.Parameters.AddWithValue("@partno", partno);
                                cmd.Parameters.AddWithValue("@partname", partname);
                                cmd.Parameters.AddWithValue("@dept", Dept);
                                cmd.Parameters.AddWithValue("@stockin", stockin);
                                cmd.Parameters.AddWithValue("@stockout", stockout);
                                cmd.Parameters.AddWithValue("@stockvalue", stockvalue);
                                cmd.Parameters.AddWithValue("@stockamount", DBNull.Value);
                                cmd.Parameters.AddWithValue("@pono", DBNull.Value);
                                cmd.Parameters.AddWithValue("@invoice", DBNull.Value);
                                cmd.Parameters.AddWithValue("@status", "1");
                                cmd.Parameters.AddWithValue("@regdate", regdate);
                                cmd.Parameters.AddWithValue("@updatedate", updatedate);
                                cmd.Parameters.AddWithValue("@pic", pic);
                                cmd.Parameters.AddWithValue("@remark", remark);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while saving data " + ex.Message, "Error SaveDB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            con.con.Close();
                dgvInput.Rows.Clear();
            MessageBox.Show("Save Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Cursor = Cursors.Default;
            }

        private void DgvInput_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow CurrentRow = dgvInput.Rows[e.RowIndex];
                if (dgvInput.Columns[e.ColumnIndex].Name == "ColStockIn")
                {
                    if (CurrentRow.Cells[e.ColumnIndex].Value != null && CurrentRow.Cells[e.ColumnIndex].Value.ToString().Trim() != "")
                    {
                        try
                        {
                            double Qty = Convert.ToDouble(CurrentRow.Cells[e.ColumnIndex].Value);
                            if (Qty >= 0)
                            {
                                if (Qty > 0)
                                {
                                    CurrentRow.Cells[e.ColumnIndex].Value = Qty;
                                    CurrentRow.Cells["ColStockOut"].Value = 0;
                                }
                                else 
                                {
                                    if (Qty == 0 && Convert.ToDouble(CurrentRow.Cells["ColStockOut"].Value) > 0)
                                    {
                                        CurrentRow.Cells[e.ColumnIndex].Value = Qty;
                                    }
                                    else if (Qty == 0 && Convert.ToDouble(CurrentRow.Cells["ColStockOut"].Value) == 0)
                                    {
                                        CurrentRow.Cells[e.ColumnIndex].Value = BeforeEdit;
                                    }
                                    else
                                    {
                                        MessageBox.Show("ចំនួនត្រូវតែធំជាង ០!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        CurrentRow.Cells[e.ColumnIndex].Value = BeforeEdit;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("ចំនួនត្រូវតែធំជាង ០!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                CurrentRow.Cells[e.ColumnIndex].Value = BeforeEdit;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!\nសូមបញ្ចូលជាចំនួនលេខ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            CurrentRow.Cells[e.ColumnIndex].Value = BeforeEdit;
                        }
                    }
                    else
                        CurrentRow.Cells[e.ColumnIndex].Value = BeforeEdit;
                }
                if (dgvInput.Columns[e.ColumnIndex].Name == "ColStockOut")
                {
                    if (CurrentRow.Cells[e.ColumnIndex].Value != null && CurrentRow.Cells[e.ColumnIndex].Value.ToString().Trim() != "")
                    {
                        try
                        {
                            double Qty = Convert.ToDouble(CurrentRow.Cells[e.ColumnIndex].Value);
                            if (Qty >= 0)
                            {
                                if (Qty > 0)
                                {
                                    //select compare 
                                    DataTable dtStock = new DataTable();
                                    string code = CurrentRow.Cells["ColCode"].Value.ToString();
                                    string queryStock = "SELECT COALESCE(SUM(Stock_Value),0) AS StockRemain FROM SparePartTrans WHERE Code = '" + code + "' AND Dept = '" + Dept + "'";
                                    SqlDataAdapter sdaStock = new SqlDataAdapter(queryStock, con.con);
                                    sdaStock.Fill(dtStock);
                                    double StockRemain = Convert.ToDouble(dtStock.Rows[0]["StockRemain"]);
                                    foreach (DataGridViewRow row in dgvInput.Rows)
                                    {
                                        if (row.Index != e.RowIndex && row.Cells["ColCode"].Value.ToString() == code)
                                        {
                                            StockRemain -= Convert.ToDouble(row.Cells["ColStockOut"].Value);
                                        }
                                    }
                                    if (Qty > StockRemain)
                                    {
                                        MessageBox.Show("ចំនួនស្តុកនៅសល់មិនគ្រប់គ្រាន់សម្រាប់ដកចេញទេ!\nស្តុកនៅសល់មានតែ " + StockRemain.ToString("N0") + " តែប៉ុណ្ណោះ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        CurrentRow.Cells[e.ColumnIndex].Value = BeforeEdit;
                                        return;
                                    }
                                    CurrentRow.Cells[e.ColumnIndex].Value = Qty;
                                    CurrentRow.Cells["ColStockIn"].Value = 0;
                                }
                                else
                                {
                                    if (Qty == 0 && Convert.ToDouble(CurrentRow.Cells["ColStockIn"].Value) > 0)
                                    {
                                        CurrentRow.Cells[e.ColumnIndex].Value = Qty;
                                    }
                                    else if (Qty == 0 && Convert.ToDouble(CurrentRow.Cells["ColStockIn"].Value) == 0)
                                    {
                                        CurrentRow.Cells[e.ColumnIndex].Value = BeforeEdit;
                                    }
                                    else
                                    {
                                        MessageBox.Show("ចំនួនត្រូវតែធំជាង ០!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        CurrentRow.Cells[e.ColumnIndex].Value = BeforeEdit;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("ចំនួនត្រូវតែធំជាង ០!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                CurrentRow.Cells[e.ColumnIndex].Value = BeforeEdit;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!\nសូមបញ្ចូលជាចំនួនលេខ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            CurrentRow.Cells[e.ColumnIndex].Value = BeforeEdit;
                        }
                    }
                    else
                        CurrentRow.Cells[e.ColumnIndex].Value = BeforeEdit;
                }
                if (dgvInput.Columns[e.ColumnIndex].Name == "ColRemark")
                {
                    if (CurrentRow.Cells[e.ColumnIndex].Value == null || CurrentRow.Cells[e.ColumnIndex].Value.ToString().Trim() == "")
                        CurrentRow.Cells[e.ColumnIndex].Value = RemarkBeforeEdit;
                }
            }
        }
        private void DgvInput_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvInput.Columns[e.ColumnIndex].Name == "ColStockIn" || dgvInput.Columns[e.ColumnIndex].Name == "ColStockOut")
                {
                    BeforeEdit = Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                }
                if (dgvInput.Columns[e.ColumnIndex].Name == "ColRemark")
                    RemarkBeforeEdit = dgvInput.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
        }
        private void DgvInput_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (dgvInput.Rows.Count == 0)
            {
                btnSave.Enabled = false;
                btnSaveGRAY.BringToFront();
            }
        }
        private void DgvInput_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            btnSave.Enabled = true;
            btnSaveGRAY.SendToBack();
        }
        private void DgvInput_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvInput.SelectedCells.Count>0 && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvInput.Columns[e.ColumnIndex].Name == "Remove")
                {
                    DialogResult DSL = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនដែរឬទេ?", MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DSL == DialogResult.Yes)
                    {
                        dgvInput.Rows.RemoveAt(e.RowIndex);
                        foreach (DataGridViewRow row in dgvInput.Rows)
                            row.HeaderCell.Value = (row.Index + 1).ToString();
                        dgvInput.ClearSelection();
                        dgvInput.CurrentCell = null;
                    }
                }
            }
        }
        private void DgvInput_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string ColName = dgvInput.Columns[e.ColumnIndex].Name;
                if (ColName == "ColRemark")
                    e.CellStyle.ForeColor = Color.Black;
                if (ColName == "ColStockIn")
                {
                    try
                    {
                        if (Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells["ColStockIn"].Value) > 0)
                        {
                            e.CellStyle.ForeColor = Color.Green;
                            dgvInput.Rows[e.RowIndex].Cells["ColStockOut"].Style.ForeColor = Color.Black;
                        }
                    }
                    catch { }
                }
                if (ColName == "ColStockOut")
                {
                    try
                    {
                        if (Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells["ColStockOut"].Value) > 0)
                        {
                            e.CellStyle.ForeColor = Color.Red;
                            dgvInput.Rows[e.RowIndex].Cells["ColStockIn"].Style.ForeColor = Color.Black;
                        }
                    }
                    catch { }
                }
                if (ColName == "Remove")
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.SelectionBackColor = Color.White;
                }
            }
        }
        private void TxtRemark_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAdd.Focus();
                btnAdd.PerformClick();
            }
        }
        private void TxtQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtRemark.Focus();
                txtRemark.Select(txtRemark.Text.Length,0);
            }
        }
        private void TxtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control keys (Backspace, Delete, etc.)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true; // Block non-numeric input
        }
        private void TxtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtCode.Text.Trim()!="")
            {
                ErrorText = "";
                Cursor = Cursors.WaitCursor;

                //Taking Info
                ClearInfoText();
                DataTable dtInfo = new DataTable();
                try
                {
                    con.con.Open();
                    string SQLQuery = "SELECT I.Code, I.Part_No, I.Part_Name, Use_For, Maker, Box, COALESCE(SUM(T.Stock_Value),0) AS StockRemain " +
                        "\r\nFROM [MstMCSparePart] I " +
                        "\r\nLEFT JOIN SparePartTrans T ON I.Code = T.Code AND T.Dept = '"+Dept+"' " +
                        "\r\nWHERE I.Code = '"+txtCode.Text+"' " +
                        "\r\nGROUP BY I.Code, I.Part_No, I.Part_Name, Use_For, Maker, Box";

                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, con.con);
                    sda.Fill(dtInfo);

                    if (dtInfo.Rows.Count > 0)
                    {
                        DataRow row = dtInfo.Rows[0];
                        txtCode.Text = row["Code"].ToString();
                        txtPartName.Text = row["Part_Name"].ToString();
                        txtPartNo.Text = row["Part_No"].ToString();
                        txtMCName.Text = row["Use_For"].ToString();
                        txtMaker.Text = row["Maker"].ToString();
                        txtLocation.Text = row["Box"].ToString();
                        LbStockRemain.Text = "/ "+ Convert.ToDouble(row["StockRemain"]).ToString("N0");
                        LbStockRemain.Refresh();
                    }

                }
                catch (Exception ex)
                {
                    ErrorText = "Taking Info : "+ex.Message;
                }
                con.con.Close();

                Cursor = Cursors.Default;

                if (ErrorText.Trim() == "")
                {
                    txtQty.Focus();
                    txtQty.Select(txtQty.Text.Length, 0);
                }
                else
                    MessageBox.Show("មានបញ្ហា!\n"+ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DgvSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                txtCode.Text = dgvSearch.CurrentRow.Cells["ColCodeS"].Value.ToString();
                txtPartName.Text = dgvSearch.CurrentRow.Cells["ColPartNameS"].Value.ToString();
                txtPartNo.Text = dgvSearch.CurrentRow.Cells["ColPartNumberS"].Value.ToString();
                TxtCode_KeyDown(sender, new KeyEventArgs(Keys.Enter));
            }
        }
        private void DgvSearch_LostFocus(object sender, EventArgs e)
        {
            if (dgvSearch.Focused == false && txtSearch.Focused == false && btnShowSearch.Focused == false)
                panelSearch.Visible = false;
        }
        private void TxtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text.ToString().Contains("ដើម្បីស្វែងរក"))
            {
                txtSearch.Text = "";
            }
        }
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            dgvSearch.Rows.Clear();
            if (txtSearch.Text.Trim() == "" || txtSearch.Text.ToString().Contains("ដើម្បីស្វែងរក"))
            {
                if (txtSearch.Text.ToString().Contains("ដើម្បីស្វែងរក"))
                {
                    txtSearch.Font = new Font(txtSearch.Font, FontStyle.Regular | FontStyle.Italic);
                    txtSearch.ForeColor = Color.Silver;
                }                
                foreach (DataRow row in dtMst.Rows)
                {
                    dgvSearch.Rows.Add(row["Code"].ToString(), row["Part_No"].ToString(), row["Part_Name"].ToString());
                }
            }
            else
            {
                txtSearch.Font = new Font(txtSearch.Font, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;

                string SValue = txtSearch.Text.ToUpper();
                foreach (DataRow row in dtMst.Rows)
                {
                    int OKToAdd = 0;
                    if (txtSearch.Text.Trim() != "")
                    {
                        if (row["Code"].ToString().ToUpper().Contains(SValue) ||
                            row["Part_No"].ToString().ToUpper().Contains(SValue) ||
                            row["Part_Name"].ToString().ToUpper().Contains(SValue))
                            OKToAdd++;
                    }
                    else
                        OKToAdd++;

                    if (OKToAdd > 0)
                        dgvSearch.Rows.Add(row["Code"].ToString(), row["Part_No"].ToString(), row["Part_Name"].ToString());
                }

            }
            dgvSearch.ClearSelection();
            Cursor = Cursors.Default;
        }
        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (txtSearch.Text.ToString().Contains("ដើម្បីស្វែងរក") || txtSearch.Text.Trim() == "")
                txtSearch.Text = "បំពេញព័ត៌មានដើម្បីស្វែងរក";
        }
        private void BtnShowSearch_Click(object sender, EventArgs e)
        {
            if (panelSearch.Visible)
                panelSearch.Visible = false;
            else
            {
                //TakingMst();
                if (dtMst.Rows.Count >= 10)
                {
                    panelSearch.Size = new Size(549, 282);
                    txtSearch.Visible = true;
                }
                else
                {
                    panelSearch.Size = new Size(549, (dtMst.Rows.Count * 25) + dtMst.Rows.Count + splitter1.Size.Height + 2);
                    txtSearch.Visible = false;
                }
                //Console.WriteLine("Location : " + panelSearch.Location.X + ", "+ panelSearch.Location.Y);
                //Console.WriteLine("PanelComAndMini H : " + panelSearch.Size.Height);
                //Console.WriteLine("PanelBody H : " + panelBody.Size.Height);
                //Console.WriteLine("tabEdit H : " + tabContrlEdit.Size.Height);
                //panelSearch.Location = new Point(131, panelBody.Size.Height - tabContrlEdit.Size.Height - panelSearch.Height + 73);
                panelSearch.Visible = true;
                dgvSearch.ClearSelection();
                dgvSearch.Focus();
            }
        }
        private void RdbStockOut_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbStockOut.Checked)
            {
                LbQty.Text = rdbStockOut.Text +" Qty";
                LbQty.ForeColor = Color.Red;
                LbStockRemain.Visible = true;
            }
            else
            {
                LbQty.Text = rdbStockIN.Text + " Qty";
                LbQty.ForeColor = Color.Green;
                LbStockRemain.Visible = false;
            }
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            CheckBeforeAdd();
            int FoundError = 0;
            foreach (Control ctrl in GrbAdd.Controls)
            {
                if (ctrl is PictureBox pic && ctrl.Name.ToUpper().Contains("PICALERT") && pic.Visible == true)
                {
                    FoundError++;
                }
            }
            if (FoundError == 0)
            {
                string Code = txtCode.Text;
                string PartNo = txtPartNo.Text;
                string PartName = txtPartName.Text;
                string Location = txtLocation.Text;
                string Remark = txtRemark.Text;
                DateTime TransDate = dtpTransDate.Value.Date;
                double InQty = 0, OutQty = 0;
                if (rdbStockIN.Checked)
                    InQty = Convert.ToDouble(txtQty.Text);
                else
                    OutQty = Convert.ToDouble(txtQty.Text);

                dgvInput.Rows.Add();
                DataGridViewRow drow = dgvInput.Rows[dgvInput.Rows.Count-1];
                drow.HeaderCell.Value = (drow.Index+1).ToString();
                drow.Cells["ColCode"].Value = Code;
                drow.Cells["ColPartNo"].Value = PartNo;
                drow.Cells["ColPartName"].Value = PartName;
                drow.Cells["ColLocation"].Value = Location;
                drow.Cells["ColTransDate"].Value = TransDate;
                drow.Cells["ColStockIn"].Value = InQty;
                drow.Cells["ColStockOut"].Value = OutQty;
                drow.Cells["ColRemark"].Value = Remark;
                dgvInput.ClearSelection();
                dgvInput.CurrentCell = null;
                ClearInfoText();
                txtCode.Text = "";
                txtQty.Text = "";
                txtRemark.Text = "";
                txtCode.Focus();
            }
        }
        private void BtnAdd_MouseLeave(object sender, EventArgs e)
        {
            Color NewColor = Color.White;
            btnAdd.BackColor = NewColor;
            btnAddPic.BackColor = NewColor;
        }
        private void BtnAdd_MouseEnter(object sender, EventArgs e)
        {
            Color NewColor = Color.LightBlue;
            btnAdd.BackColor = NewColor;
            btnAddPic.BackColor = NewColor;
        }
        private void StockINOut_Shown(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            this.panelSearch.BringToFront();
            foreach (DataGridViewColumn col in dgvInput.Columns)
            {
                if (col.Index < 3)
                    col.Frozen = true;
                if (col.Name == "Remove")
                    col.Resizable = DataGridViewTriState.False;
                if (col.Name != "ColStockIn" && col.Name != "ColStockOut" && col.Name != "ColRemark")
                    col.ReadOnly = true;
                //Console.WriteLine(col.Name);
            }

            TakingMst();
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtMst.Rows)
                {
                    dgvSearch.Rows.Add(row["Code"].ToString(), row["Part_No"].ToString(), row["Part_Name"].ToString());
                }
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
                MessageBox.Show("មានបញ្ហា!\n"+ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        //Method
        private async void CheckBeforeAdd()
        {
            PicAlertCode.Visible = false;
            PicAlertQty.Visible = false;
            PicAlertRemark.Visible = false;
            var tasksBlink = new List<Task>();

            if (txtPartNo.Text.Trim() == "" || txtCode.Text.Trim()=="")
                tasksBlink.Add(BlinkPictureBox(PicAlertCode));

            if (txtQty.Text.Trim() == "")
            {
                toolTip1.SetToolTip(PicAlertQty, "សូមបញ្ចូល " + LbQty.Text);
                tasksBlink.Add(BlinkPictureBox(PicAlertQty));
            }
            else
            {
                try
                {
                    double Qty = Convert.ToDouble(txtQty.Text);
                    double StockQty = Convert.ToDouble(LbStockRemain.Text.Replace("/ ", ""));
                    if (Qty > 0)
                    {
                        //Comparing Stock if > Transaction is Stock-Out
                        if (rdbStockOut.Checked)
                        {
                            double AlreadyInputQty = 0;
                            foreach (DataGridViewRow row in dgvInput.Rows)
                                AlreadyInputQty += Convert.ToDouble(row.Cells["ColStockOut"].Value);
                            if (Qty+AlreadyInputQty > StockQty)
                            {
                                toolTip1.SetToolTip(PicAlertQty, "ចំនួនស្តុកមិនគ្រប់គ្រាន់ទេ!");
                                tasksBlink.Add(BlinkPictureBox(PicAlertQty));
                            }
                        }
                    }
                    else
                    {
                        toolTip1.SetToolTip(PicAlertQty, "ចំនួនត្រូវតែធំជាង 0!");
                        tasksBlink.Add(BlinkPictureBox(PicAlertQty));
                    }
                }
                catch
                {
                    toolTip1.SetToolTip(PicAlertQty, "អ្នកបញ្ចូលខុសទម្រង់ហើយ!\nសូមបញ្ចូលជាចំនួនលេខ!");
                    tasksBlink.Add(BlinkPictureBox(PicAlertQty));
                }
            }

            if (txtRemark.Text.Trim() == "")
                tasksBlink.Add(BlinkPictureBox(PicAlertRemark));

            await Task.WhenAll(tasksBlink);
        }
        private async Task BlinkPictureBox(PictureBox pictureBox)
        {
            pictureBox.Visible = false;
            for (int i = 0; i < 7; i++)
            {
                pictureBox.Visible = !pictureBox.Visible;
                await Task.Delay(300);
            }
            pictureBox.Visible = true;
        }
        private void TakingMst()
        {
            //Taking Mst
            dtMst = new DataTable();
            try
            {
                con.con.Open();
                string SQLQuery = "SELECT * FROM MstMCSparePart WHERE Dept = '" + Dept + "' ORDER BY Code";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, con.con);
                sda.Fill(dtMst);
            }
            catch (Exception ex)
            {
                ErrorText = "Taking Mst : " + ex.Message;
            }
            con.con.Close();
        }
        private void ClearInfoText()
        {
            txtPartNo.Text = "";
            txtPartName.Text = "";
            txtLocation.Text = "";
            txtMCName.Text = "";
            txtMaker.Text = "";
            LbStockRemain.Text = "";
        }
    }
}
