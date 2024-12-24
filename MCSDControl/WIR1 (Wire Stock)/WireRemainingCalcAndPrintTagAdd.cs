using MachineDeptApp.Admin;
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

namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    public partial class WireRemainingCalcAndPrintTagAdd : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        DataTable dtRM;
        WireRemainingCalcAndPrintTag fgrid;
        string BobbinsOrReil;

        public WireRemainingCalcAndPrintTagAdd(WireRemainingCalcAndPrintTag fg)
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.fgrid = fg;
            this.Load += WireRemainingCalcAndPrintTagAdd_Load;

            //Cbo
            this.CboR2OrBobbinsWBobbins.TextChanged += CboR2OrBobbinsWBobbins_TextChanged;

            //txt
            this.txtCodeEdit.KeyDown += TxtCodeEdit_KeyDown;
            this.txtCodeEdit.Leave += TxtCodeEdit_Leave;
            this.txtSearchItem.TextChanged += TxtSearchItem_TextChanged;
            this.txtSearchItem.LostFocus += TxtSearchItem_LostFocus;
            this.txtTotalWeight.TextChanged += TxtTotalWeight_TextChanged;
            this.txtTotalWeight.KeyPress += TxtTotalWeight_KeyPress;
            this.txtR3.TextChanged += TxtR3_TextChanged;
            this.txtR3.KeyPress += TxtR3_KeyPress;

            //Button
            this.btnShowDgvItems.Click += BtnShowDgvItems_Click;
            this.btnOK.Click += BtnOK_Click;

            //Dgv
            this.DgvSearchItem.LostFocus += DgvSearchItem_LostFocus;
            this.DgvSearchItem.CellClick += DgvSearchItem_CellClick;

        }

        

        //Cbo
        private void CboR2OrBobbinsWBobbins_TextChanged(object sender, EventArgs e)
        {
            if (txtTotalWeight.Text.Trim() != "" && LbR1OrNetWBobbins.Text.Trim() != "" && LbMOQBobbins.Text.Trim() != "" && CboR2OrBobbinsWBobbins.Text.Trim() != "")
            {
                try
                {
                    double BobbinsW = Convert.ToDouble(CboR2OrBobbinsWBobbins.Text);
                    double NetW = Convert.ToDouble(LbR1OrNetWBobbins.Text);
                    double MOQ = Convert.ToDouble(LbMOQBobbins.Text);
                    double InputTotalW = Convert.ToDouble(txtTotalWeight.Text);
                    double TotalRemainingQty = Math.Round((InputTotalW - BobbinsW) / NetW * MOQ, 2);
                    txtTotalQtyBobbins.Text = TotalRemainingQty.ToString("N0");

                }
                catch
                {
                    txtTotalQtyBobbins.Text = "";
                }
            }
        }

        //Dgv
        private void DgvSearchItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCodeEdit.Text = DgvSearchItem.Rows[e.RowIndex].Cells[0].Value.ToString();
            TakeItemName();
        }
        private void DgvSearchItem_LostFocus(object sender, EventArgs e)
        {
            HidePanelItems();
        }

        //Button
        private void BtnShowDgvItems_Click(object sender, EventArgs e)
        {
            DgvSearchItem.Rows.Clear();
            int PanelItemY = panelSearchItems.Location.Y + 285 + txtNameEdit.Location.Y - 5;            
            if (dtRM.Rows.Count > 0)
            {
                if (dtRM.Rows.Count > 8)
                {
                    txtSearchItem.Visible = true;
                    panelSearchItem.Size = new Size(390, 247);
                    DgvSearchItem.Size = new Size(390, 226);
                    DgvSearchItem.Columns[1].Width = 336 - 16;
                    panelSearchItem.Location = new Point(228, PanelItemY - panelSearchItem.Height);
                }
                else
                {
                    int HeightofDgv = dtRM.Rows.Count * 26;
                    txtSearchItem.Visible = false;
                    panelSearchItem.Size = new Size(390, HeightofDgv);
                    DgvSearchItem.Columns[1].Width = 338;
                    DgvSearchItem.Size = new Size(390, HeightofDgv);
                    panelSearchItem.Location = new Point(228, PanelItemY - panelSearchItem.Height);
                }
                foreach (DataRow row in dtRM.Rows)
                {
                    DgvSearchItem.Rows.Add(row[0].ToString(), row[1].ToString());
                }
            }
            DgvSearchItem.ClearSelection();
            panelSearchItem.BringToFront();
            DgvSearchItem.Focus();
        }
        private void BtnOK_Click(object sender, EventArgs e)
        {
            //Bobbins
            if (tabContrlBobbins.Visible == true)
            {
                if (txtCodeEdit.Text.Trim() != "" && txtNameEdit.Text.Trim() != "" && CboR2OrBobbinsWBobbins.Text.Trim() != "" && LbR1OrNetWBobbins.Text.Trim() != "" && LbMOQBobbins.Text.Trim() != "" && txtTotalQtyBobbins.Text.Trim() != "")
                {
                    string Code = txtCodeEdit.Text;
                    string Name = txtNameEdit.Text;
                    string Type = LbRMTypeBobbins.Text;
                    string Maker = LbMakerBobbins.Text;
                    double MOQ = Convert.ToDouble(LbMOQBobbins.Text);
                    double BobbinsW = Convert.ToDouble(CboR2OrBobbinsWBobbins.Text);
                    double NetW = Convert.ToDouble(LbR1OrNetWBobbins.Text);
                    double TotalW = Convert.ToDouble(txtTotalWeight.Text);
                    double TotalRemainQty = Convert.ToDouble(txtTotalQtyBobbins.Text);
                    string LotNo = txtLotNoBobbins.Text;
                    if (TotalRemainQty > 0)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(fgrid.dgvInput);
                        row.Cells[0].Value = Code;
                        row.Cells[1].Value = Name;
                        row.Cells[2].Value = Type;
                        row.Cells[3].Value = Maker;
                        row.Cells[4].Value = BobbinsOrReil;
                        row.Cells[5].Value = MOQ;
                        row.Cells[6].Value = BobbinsW;
                        row.Cells[7].Value = NetW;
                        row.Cells[8].Value = TotalW;
                        row.Cells[9].Value = TotalRemainQty;
                        row.Cells[10].Value = LotNo;
                        row.HeaderCell.Style.BackColor = Color.FromKnownColor(KnownColor.GradientActiveCaption);
                        row.HeaderCell.Style.Font = new Font("Khmer OS Battambang", 9, FontStyle.Regular);
                        row.HeaderCell.Value = (fgrid.dgvInput.Rows.Count + 1).ToString();
                        fgrid.dgvInput.Rows.Add(row);
                        fgrid.dgvInput.ClearSelection();
                        txtTotalWeight.Text = "";
                        txtTotalWeight.Focus();
                    }
                    else
                    {
                        MessageBox.Show("ចំនួននៅសល់ត្រូវតែធំជាង 0 !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("សូមបញ្ចូលនូវព័ត៌មានដែលមានផ្ទាំងក្រោយ\nពណ៌ <ទឹកក្រូច> ទាំងអស់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            //Reil
            else
            {
                if (txtCodeEdit.Text.Trim() != "" && txtNameEdit.Text.Trim() != "" && LbR2OrBobbinsWReil.Text.Trim() != "" && LbR1OrNetWReil.Text.Trim() != "" && LbMOQReil.Text.Trim() != "" && txtTotalQtyReil.Text.Trim() != "")
                {
                    string Code = txtCodeEdit.Text;
                    string Name = txtNameEdit.Text;
                    string Type = LbRMTypeReil.Text;
                    string Maker = LbMakerReil.Text;
                    double MOQ = Convert.ToDouble(LbMOQReil.Text);
                    double R2 = Convert.ToDouble(LbR2OrBobbinsWReil.Text);
                    double R1 = Convert.ToDouble(LbR1OrNetWReil.Text);
                    double R3 = Convert.ToDouble(txtR3.Text);
                    double TotalRemainQty = Convert.ToDouble(txtTotalQtyReil.Text);
                    string LotNo = txtLotNoReil.Text;

                    if (TotalRemainQty > 0)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(fgrid.dgvInput);
                        row.Cells[0].Value = Code;
                        row.Cells[1].Value = Name;
                        row.Cells[2].Value = Type;
                        row.Cells[3].Value = Maker;
                        row.Cells[4].Value = BobbinsOrReil;
                        row.Cells[5].Value = MOQ;
                        row.Cells[6].Value = R2;
                        row.Cells[7].Value = R1;
                        row.Cells[8].Value = R3;
                        row.Cells[9].Value = TotalRemainQty;
                        row.Cells[10].Value = LotNo;
                        row.HeaderCell.Style.BackColor = Color.FromKnownColor(KnownColor.GradientActiveCaption);
                        row.HeaderCell.Style.Font = new Font("Khmer OS Battambang", 9, FontStyle.Regular);
                        row.HeaderCell.Value = (fgrid.dgvInput.Rows.Count + 1).ToString();
                        fgrid.dgvInput.Rows.Add(row);
                        fgrid.dgvInput.ClearSelection();
                        txtR3.Text = "";
                        txtR3.Focus();
                    }
                    else
                    {
                        MessageBox.Show("ចំនួននៅសល់ត្រូវតែធំជាង 0 !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("សូមបញ្ចូលនូវព័ត៌មានដែលមានផ្ទាំងក្រោយ\nពណ៌ <ទឹកក្រូច> ទាំងអស់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        //txt
        private void TxtCodeEdit_Leave(object sender, EventArgs e)
        {
            if (txtCodeEdit.Text.Trim() != "")
            {
                txtNameEdit.Text = "";
                TakeItemName();
            }
        }
        private void TxtSearchItem_LostFocus(object sender, EventArgs e)
        {
            HidePanelItems();
        }
        private void TxtSearchItem_TextChanged(object sender, EventArgs e)
        {
            DgvSearchItem.Rows.Clear();
            if (txtSearchItem.Text.Trim() == "")
            {
                foreach (DataRow row in dtRM.Rows)
                {
                    DgvSearchItem.Rows.Add(row[0].ToString(), row[1].ToString());
                }
            }
            else
            {

                foreach (DataRow row in dtRM.Rows)
                {
                    int Found = 0;
                    if (row[0].ToString().ToLower().Contains(txtSearchItem.Text.ToString().ToLower()) == true || row[1].ToString().ToLower().Contains(txtSearchItem.Text.ToString().ToLower()) == true)
                    {
                        Found = Found + 1;
                    }
                    if (Found > 0)
                    {
                        DgvSearchItem.Rows.Add(row[0].ToString(), row[1].ToString());
                    }
                }
            }
            if (DgvSearchItem.Rows.Count > 8)
            {
                DgvSearchItem.Columns[1].Width = 336 - 16;
            }
            else
            {
                DgvSearchItem.Columns[1].Width = 338;
            }
        }
        private void TxtCodeEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtNameEdit.Focus();
            }
        }
        private void TxtTotalWeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        private void TxtTotalWeight_TextChanged(object sender, EventArgs e)
        {
            if (txtTotalWeight.Text.Trim() != "" && LbR1OrNetWBobbins.Text.Trim() != "" && LbMOQBobbins.Text.Trim() != "" && CboR2OrBobbinsWBobbins.Text.Trim() != "")
            {
                try
                {
                    double BobbinsW = Convert.ToDouble(CboR2OrBobbinsWBobbins.Text);
                    double NetW = Convert.ToDouble(LbR1OrNetWBobbins.Text);
                    double MOQ = Convert.ToDouble(LbMOQBobbins.Text);
                    double InputTotalW = Convert.ToDouble(txtTotalWeight.Text);
                    double TotalRemainingQty = Math.Round((InputTotalW - BobbinsW) / NetW * MOQ, 2);
                    txtTotalQtyBobbins.Text = TotalRemainingQty.ToString("N0");

                }
                catch
                {
                    txtTotalQtyBobbins.Text = "";
                }
            }
            else
            {
                txtTotalQtyBobbins.Text = "";
            }
        }
        private void TxtR3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        private void TxtR3_TextChanged(object sender, EventArgs e)
        {
            if (txtR3.Text.Trim() != "" && LbR1OrNetWReil.Text.Trim() != "" && LbMOQReil.Text.Trim() != "" && LbR2OrBobbinsWReil.Text.Trim() != "")
            {
                try
                {
                    double R2 = Convert.ToDouble(LbR2OrBobbinsWReil.Text);
                    double R1 = Convert.ToDouble(LbR1OrNetWReil.Text);
                    double MOQ = Convert.ToDouble(LbMOQReil.Text);
                    double InputedR3 = Convert.ToDouble(txtR3.Text);
                    double TotalRemainingQty = Math.Round(MOQ*(InputedR3*InputedR3-R2*R2)/(R1*R1-R2*R2),2);
                    txtTotalQtyReil.Text = TotalRemainingQty.ToString("N0");

                }
                catch
                {
                    txtTotalQtyReil.Text = "";
                }
            }
            else
            {
                txtTotalQtyReil.Text = "";
            }
        }

        private void WireRemainingCalcAndPrintTagAdd_Load(object sender, EventArgs e)
        {
            dtRM = new DataTable();
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT Code, ItemName FROM " +
                "(SELECT Code FROM tbSDMstUncountMat) T1 " +
                "INNER JOIN " +
                "(SELECT * FROM tbMasterItem)T2 " +
                "ON T1.Code=T2.ItemCode " +
                "ORDER BY Code ASC", cnn.con);
                sda.Fill(dtRM);

            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            cnn.con.Close();

        }
        private void HidePanelItems()
        {
            if (DgvSearchItem.Focused == false && txtSearchItem.Focused == false)
            {
                panelSearchItem.SendToBack();
            }
        }
        private void ShowBobbinsOrReil()
        {
            if (BobbinsOrReil == "Bobbin")
            {
                tabContrlBobbins.Visible = true;
                tabContrlReil.Visible = false;
                panelBody.BackColor = Color.Coral;
                panelBody.Refresh();
            }
            else
            {
                tabContrlBobbins.Visible = false;
                tabContrlReil.Visible = true;
                panelBody.BackColor = Color.FromKnownColor(KnownColor.HotTrack);
                panelBody.Refresh();
            }
        }
        private void ClearAllText()
        {
            LbMakerBobbins.Text = "";
            LbMakerReil.Text = "";
            LbRMTypeBobbins.Text = "";
            LbRMTypeReil.Text = "";
            LbR1OrNetWBobbins.Text = "";
            LbR1OrNetWReil.Text = "";
            LbR2OrBobbinsWReil.Text = "";
            LbMOQBobbins.Text = "";
            LbMOQReil.Text = "";
            txtTotalWeight.Text = "";
            txtR3.Text = "";
            txtTotalQtyBobbins.Text = "";
            txtTotalQtyReil.Text = "";
        }
        private void TakeItemName()
        {
            ClearAllText();
            DataTable dt = new DataTable();
            DataTable dtBobbinsW = new DataTable();
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT Code, ItemName, RMType, BobbinsOrReil, R2OrBobbinsW, R1OrNetW, MOQ FROM " +
                    "(SELECT * FROM tbSDMstUncountMat) T1 " +
                    "INNER JOIN " +
                    "(SELECT * FROM tbMasterItem) T2 " +
                    "ON T1.Code=T2.ItemCode " +
                    "WHERE Code='" + txtCodeEdit.Text + "'", cnn.con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][3].ToString() == "Bobbin")
                    {
                        SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM tbSDMstBobbinsWeight " +
                            "WHERE RMType ='" + dt.Rows[0][2].ToString() + "' ORDER BY BobbinsW ASC", cnn.con);
                        sda1.Fill(dtBobbinsW);
                    }
                    CboR2OrBobbinsWBobbins.Items.Clear();
                    foreach (DataRow row in dtBobbinsW.Rows)
                    {
                        CboR2OrBobbinsWBobbins.Items.Add(row[1].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
            if (dt.Rows.Count > 0)
            {
                //Take the Maker from OBS
                DataTable dtOBS = new DataTable();
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, Resv1 FROM mstitem WHERE ItemType=2 AND ItemCode='" + txtCodeEdit.Text + "'", cnnOBS.conOBS);
                    sda.Fill(dtOBS);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
                txtNameEdit.Text = dt.Rows[0][1].ToString();
                BobbinsOrReil = dt.Rows[0][3].ToString();
                if (BobbinsOrReil == "Bobbin")
                {
                    LbRMTypeBobbins.Text = dt.Rows[0][2].ToString();
                    if (dt.Rows[0][4].ToString().Trim() != "")
                    {
                        CboR2OrBobbinsWBobbins.Text = dt.Rows[0][4].ToString();
                    }
                    else
                    {
                        CboR2OrBobbinsWBobbins.Text = "0";
                    }
                    LbR1OrNetWBobbins.Text = dt.Rows[0][5].ToString();
                    LbMOQBobbins.Text = Convert.ToDouble(dt.Rows[0][6].ToString()).ToString("N0");
                    if (dtOBS.Rows.Count > 0)
                    {
                        LbMakerBobbins.Text = dtOBS.Rows[0][1].ToString();
                    }
                    ShowBobbinsOrReil();
                    txtTotalWeight.Focus();
                }
                else
                {
                    LbRMTypeReil.Text = dt.Rows[0][2].ToString();
                    LbR2OrBobbinsWReil.Text = dt.Rows[0][4].ToString();
                    LbR1OrNetWReil.Text = dt.Rows[0][5].ToString();
                    LbMOQReil.Text = Convert.ToDouble(dt.Rows[0][6].ToString()).ToString("N0");
                    if (dtOBS.Rows.Count > 0)
                    {
                        LbMakerReil.Text = dtOBS.Rows[0][1].ToString();
                    }
                    ShowBobbinsOrReil();
                    txtR3.Focus();
                }
            }
        }

    }
}
