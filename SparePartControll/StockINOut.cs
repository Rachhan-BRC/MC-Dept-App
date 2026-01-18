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

namespace MachineDeptApp
{

    public partial class StockINOut : Form
    {
        SQLConnect con = new SQLConnect();
        DataTable dtMst = new DataTable();

        string Dept = "MC", ErrorText = "";

        public StockINOut()
        {
            InitializeComponent();
            this.con.Connection();
            this.Shown += StockINOut_Shown;
            this.btnAdd.MouseEnter += BtnAdd_MouseEnter;
            this.btnAdd.MouseLeave += BtnAdd_MouseLeave;
            this.btnAddPic.MouseEnter += BtnAdd_MouseEnter;
            this.btnAddPic.MouseLeave += BtnAdd_MouseLeave;
            this.btnAdd.Click += BtnAdd_Click;
            this.btnAddPic.Click += BtnAdd_Click;
            this.btnShowSearch.Click += BtnShowSearch_Click;


            this.dgvSearch.LostFocus += DgvSearch_LostFocus;
            this.dgvSearch.CellClick += DgvSearch_CellClick;

            this.rdbStockOut.CheckedChanged += RdbStockOut_CheckedChanged;

            this.txtSearch.Enter += TxtSearch_Enter;
            this.txtSearch.Leave += TxtSearch_Leave;
            this.txtSearch.TextChanged += TxtSearch_TextChanged;
            this.txtSearch.LostFocus += DgvSearch_LostFocus;
            this.txtCode.KeyDown += TxtCode_KeyDown;
            this.txtQty.KeyPress += TxtQty_KeyPress;
            this.txtQty.KeyDown += TxtQty_KeyDown;
            this.txtRemark.KeyDown += TxtRemark_KeyDown;

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
                if (col.Name != "ColStockIn" && col.Name == "ColStockOut" && col.Name == "ColRemark")
                    col.ReadOnly = true;
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

            if (txtPartNo.Text.Trim() == "")
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
                            if (Qty > StockQty)
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
