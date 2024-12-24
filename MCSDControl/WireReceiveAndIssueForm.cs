using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    public partial class WireReceiveAndIssueForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SqlCommand cmd;
        DataTable dtFunction;
        DataTable dtLocation;
        DataTable dtRM;

        public WireReceiveAndIssueForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Load += WireReceiveAndIssueForm_Load;

            //Button
            this.btnShowDgvItems.Click += BtnShowDgvItems_Click;
            this.btnSave.Click += BtnSave_Click;
            this.btnOK.Click += BtnOK_Click;
            this.btnNew.Click += BtnNew_Click;
            this.btnDelete.Click += BtnDelete_Click;

            //txtSearchItem
            this.txtSearchItem.LostFocus += TxtSearchItem_LostFocus;

            //DgvSearchItem
            this.DgvSearchItem.CellClick += DgvSearchItem_CellClick;
            this.DgvSearchItem.LostFocus += DgvSearchItem_LostFocus;

            //DgvInput
            this.dgvInput.CellClick += DgvInput_CellClick;


            //txtCode
            this.txtCode.Leave += TxtCode_Leave;
            this.txtCode.KeyDown += TxtCode_KeyDown;
            this.txtSearchItem.TextChanged += TxtSearchItem_TextChanged;

            //Cbo
            this.CboRemark.Leave += CboRemark_Leave;
            this.CboRemark.SelectedIndexChanged += CboRemark_SelectedIndexChanged;
            this.CboLocation.SelectedIndexChanged += CboLocation_SelectedIndexChanged;

        }

        
        //Cbo
        private void CboRemark_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCurrentQty.Text = "0.00";
            txtQty.Text = "";
            if (CboLocation.Text.Trim() != "" && txtCode.Text.Trim() != "")
            {
                TakeCurrentStockQty();
            }
            txtQty.Focus();
        }
        private void CboRemark_Leave(object sender, EventArgs e)
        {
            txtCurrentQty.Text = "0.00";
            txtQty.Text = "";
            if (CboLocation.Text.Trim() != "" && txtCode.Text.Trim() != "")
            {
                TakeCurrentStockQty();
            }
        }
        private void CboLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            CboRemark.Text = "";
            txtCurrentQty.Text = "0.00";
            TakeItemName();
        }

        //txtCode
        private void TxtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtCode.Text.Trim() != "")
                {
                    TakeItemName();
                }
            }
        }
        private void TxtCode_Leave(object sender, EventArgs e)
        {
            if (txtCode.Text.Trim() != "")
            {
                TakeItemName();
            }
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
                    if (Found >0)
                    {
                        DgvSearchItem.Rows.Add(row[0].ToString(), row[1].ToString());
                    }
                }
            }
            if (DgvSearchItem.Rows.Count > 8)
            {
                DgvSearchItem.Columns[1].Width = 200 - 16;
            }
            else
            {
                DgvSearchItem.Columns[1].Width = 202;
            }
        }

        //DgvSearchItem
        private void DgvSearchItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCode.Text = DgvSearchItem.Rows[e.RowIndex].Cells[0].Value.ToString();
            TakeItemName();
        }
        private void DgvSearchItem_LostFocus(object sender, EventArgs e)
        {
            HidePanelItems();
        }

        //txtSearchItem
        private void TxtSearchItem_LostFocus(object sender, EventArgs e)
        {
            HidePanelItems();
        }

        //DgvInput
        private void DgvInput_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvInput.SelectedCells.Count > 0)
            {
                if (e.RowIndex > -1)
                {
                    if (Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells[3].Value.ToString()) > 0)
                    {
                        CboFunction.SelectedIndex = 1;
                    }
                    else
                    {
                        CboFunction.SelectedIndex=2;
                    }

                    CboLocation.Text = dgvInput.Rows[e.RowIndex].Cells[2].Value.ToString();
                    txtCode.Text = dgvInput.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtItems.Text = dgvInput.Rows[e.RowIndex].Cells[1].Value.ToString();
                    CboRemark.Text = dgvInput.Rows[e.RowIndex].Cells[5].Value.ToString();
                    CboRemark.Focus();
                    txtQty.Focus();
                    if (Convert.ToDouble(dgvInput.Rows[e.RowIndex].Cells[3].Value.ToString()) > 0)
                    {
                        txtQty.Text = dgvInput.Rows[e.RowIndex].Cells[3].Value.ToString();
                    }
                    else
                    {
                        txtQty.Text = dgvInput.Rows[e.RowIndex].Cells[4].Value.ToString();
                    }
                    txtRemark.Text = dgvInput.Rows[e.RowIndex].Cells[6].Value.ToString();
                    CheckBtnSaveAndDelete();
                }
            }
        }

        //Button
        private void BtnShowDgvItems_Click(object sender, EventArgs e)
        {
            DgvSearchItem.Rows.Clear();
            int PanelItemY = panelChooseItemAndStock.Location.Y + txtItems.Location.Y-2;
            if (dtRM.Rows.Count > 0)
            {
                if (dtRM.Rows.Count > 8)
                {
                    txtSearchItem.Visible = true;
                    panelSearchItem.Size = new Size(253, 253);
                    panelSearchItem.Location = new Point(80, PanelItemY - panelSearchItem.Height);
                }
                else
                {
                    int HeightofDgv = dtRM.Rows.Count * 26;
                    txtSearchItem.Visible = false;
                    panelSearchItem.Size = new Size(253, HeightofDgv);
                    DgvSearchItem.Size = new Size(253, HeightofDgv);
                    panelSearchItem.Location = new Point(80, PanelItemY - panelSearchItem.Height);
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
            if (CboFunction.Text.Trim() != "" && CboLocation.Text.Trim() != "" && txtCode.Text.Trim() != "" && txtItems.Text.Trim() != "" && txtQty.Text.Trim() != "")
            {
                if (Convert.ToDouble(txtQty.Text) > 0)
                {
                    //In
                    if (CboFunction.SelectedIndex == 1)
                    {
                        //Check already have or not
                        int Found = 0;
                        foreach (DataGridViewRow DgvRow in dgvInput.Rows)
                        {
                            string Code = DgvRow.Cells[0].Value.ToString();
                            string LocCode = DgvRow.Cells[2].Value.ToString();
                            string Remark = DgvRow.Cells[5].Value.ToString();
                            if (txtCode.Text == Code && CboLocation.Text == LocCode && CboRemark.Text == Remark)
                            {
                                Found = Found + 1;
                                DgvRow.Cells[3].Value = Convert.ToDouble(txtQty.Text);
                                DgvRow.Cells[4].Value = 0;
                                break;
                            }
                        }
                        if (Found == 0)
                        {
                            dgvInput.Rows.Add(txtCode.Text, txtItems.Text, CboLocation.Text, Convert.ToDouble(txtQty.Text), 0, CboRemark.Text, txtRemark.Text);
                        }
                        ClearInputText();
                    }

                    //Out
                    if (CboFunction.SelectedIndex == 2)
                    {
                        int Found = 0;
                        foreach (DataGridViewRow DgvRow in dgvInput.Rows)
                        {
                            string Code = DgvRow.Cells[0].Value.ToString();
                            string LocCode = DgvRow.Cells[2].Value.ToString();
                            string Remark = DgvRow.Cells[5].Value.ToString();
                            if (txtCode.Text == Code && CboLocation.Text == LocCode && CboRemark.Text == Remark)
                            {
                                Found = Found + 1;
                                if (Convert.ToDouble(txtCurrentQty.Text) >= Convert.ToDouble(txtQty.Text))
                                {
                                    DgvRow.Cells[3].Value = 0;
                                    DgvRow.Cells[4].Value = Convert.ToDouble(txtQty.Text);
                                    ClearInputText();
                                    break;
                                }
                                else
                                {
                                    MessageBox.Show("ស្តុកមិនគ្រប់គ្រាន់ទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                            }
                        }
                        if (Found == 0)
                        {
                            if (Convert.ToDouble(txtCurrentQty.Text) >= Convert.ToDouble(txtQty.Text))
                            {
                                dgvInput.Rows.Add(txtCode.Text, txtItems.Text, CboLocation.Text, 0, Convert.ToDouble(txtQty.Text), CboRemark.Text, txtRemark.Text);
                                ClearInputText();
                            }
                            else
                            {
                                MessageBox.Show("ស្តុកមិនគ្រប់គ្រាន់ទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                    }

                    CheckBtnSaveAndDelete();
                    AssignNoToDgv();
                    dgvInput.ClearSelection();
                    txtCode.Focus();
                }
                else
                {
                    MessageBox.Show("ចំនួនត្រូវតែច្រើនជាង 0 !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtQty.Focus();
                    txtQty.SelectAll();
                }
            }
            else
            {
                MessageBox.Show("សូមបំពេញព៌ត័មានទាំងអស់ដែលមានផ្ទាំងក្រោយពណ៌ស!\nលើកលែងតែ <លេខឯកសារ និង ចំណាំ> ប៉ុណ្នោះដែលអាចទទេបាន!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            this.dgvInput.Rows.Clear();
            CboFunction.Text = "";
            CboLocation.Text = "";
            ClearInputText();

        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult DSL = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DSL == DialogResult.Yes)
            {
                dgvInput.Rows.RemoveAt(dgvInput.CurrentCell.RowIndex);
                dgvInput.Update();
                dgvInput.ClearSelection();
                CheckBtnSaveAndDelete();
            }
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (dgvInput.Rows.Count > 0)
            {
                DataTable dtError = new DataTable();
                dtError.Columns.Add("Code");
                dtError.Columns.Add("Items");
                dtError.Columns.Add("LocName");
                dtError.Columns.Add("Doc");
                dtError.Columns.Add("Remark");

                DialogResult DSL = MessageBox.Show("តើអ្នកចង់រក្សាទុកទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    DateTime RegNow = DateTime.Now;
                    string User = MenuFormV2.UserForNextForm;

                    foreach (DataGridViewRow DgvRow in dgvInput.Rows)
                    {
                        string Code = DgvRow.Cells[0].Value.ToString();
                        string Items = DgvRow.Cells[1].Value.ToString();
                        string LocCode = "";
                        string LocName = DgvRow.Cells[2].Value.ToString();
                        foreach (DataRow row in dtLocation.Rows)
                        {
                            if (row[1].ToString() == LocName)
                            {
                                LocCode = row[0].ToString();
                                break;
                            }
                        }
                        double RecQty = Convert.ToDouble(DgvRow.Cells[3].Value.ToString());
                        double TransQty = Convert.ToDouble(DgvRow.Cells[4].Value.ToString());
                        double StockValue = (RecQty) + (TransQty * (-1));
                        string DocNo = DgvRow.Cells[5].Value.ToString();
                        string Remark = DgvRow.Cells[6].Value.ToString();
                        double Function = 1;

                        string TransNo = "";

                        //try
                        //{
                        cnn.con.Open();
                        if (StockValue > 0)
                        {
                            //Find Last TransNo
                            SqlDataAdapter da = new SqlDataAdapter("SELECT SysNo FROM tbSDMCAllTransaction " +
                                                                                            "WHERE " +
                                                                                            "SysNo=(SELECT MAX(SysNo) FROM tbSDMCAllTransaction WHERE Funct =1) Group By SysNo", cnn.con);
                            DataTable dtTransNo = new DataTable();
                            da.Fill(dtTransNo);
                            if (dtTransNo.Rows.Count == 0)
                            {
                                TransNo = "REC0000000001";
                            }
                            else
                            {
                                string LastTransNo = dtTransNo.Rows[0][0].ToString();
                                double NextTransNo = Convert.ToDouble(LastTransNo.Substring(LastTransNo.Length - 10)) + 1;
                                TransNo = "REC" + NextTransNo.ToString("0000000000");

                            }

                            //Add to AllTransaction
                            cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                                           "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs, @Rm)", cnn.con);

                            cmd.Parameters.AddWithValue("@Sn", TransNo);
                            cmd.Parameters.AddWithValue("@Ft", Function);
                            cmd.Parameters.AddWithValue("@Lc", LocCode);
                            cmd.Parameters.AddWithValue("@Pn", DocNo);
                            cmd.Parameters.AddWithValue("@Cd", Code);
                            cmd.Parameters.AddWithValue("@Rmd", Items);
                            cmd.Parameters.AddWithValue("@Rq", RecQty);
                            cmd.Parameters.AddWithValue("@Tq", TransQty);
                            cmd.Parameters.AddWithValue("@Sv", StockValue);
                            cmd.Parameters.AddWithValue("@Rd", RegNow);
                            cmd.Parameters.AddWithValue("@Rb", User);
                            cmd.Parameters.AddWithValue("@Cs", 0);
                            cmd.Parameters.AddWithValue("@Rm", Remark);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            Function = 7;
                            //Find Last TransNo
                            SqlDataAdapter da = new SqlDataAdapter("SELECT SysNo FROM tbSDMCAllTransaction " +
                                                                                            "WHERE SysNo=(SELECT MAX(SysNo) FROM tbSDMCAllTransaction WHERE Funct =7) Group By SysNo", cnn.con);
                            DataTable dtTransNo = new DataTable();
                            da.Fill(dtTransNo);
                            if (dtTransNo.Rows.Count == 0)
                            {
                                TransNo = "OUT0000000001";
                            }
                            else
                            {
                                string LastTransNo = dtTransNo.Rows[0][0].ToString();
                                double NextTransNo = Convert.ToDouble(LastTransNo.Substring(LastTransNo.Length - 10)) + 1;
                                TransNo = "OUT" + NextTransNo.ToString("0000000000");

                            }

                            //Check Remaining Stock First
                            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM " +
                                "(SELECT Code, LocCode, POSNo, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction " +
                                "WHERE CancelStatus = 0 " +
                                "GROUP BY Code, LocCode, POSNo) T1 " +
                                "WHERE StockValue>0 AND Code='" + Code + "' AND LocCode='" + LocCode + "' AND POSNo ='" + DocNo + "' ", cnn.con);
                            DataTable dtChecking = new DataTable();
                            sda.Fill(dtChecking);

                            if (Convert.ToDouble(dtChecking.Rows[0][0].ToString()) > 0)
                            {
                                //Add to AllTransaction
                                cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                                               "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs, @Rm)", cnn.con);

                                cmd.Parameters.AddWithValue("@Sn", TransNo);
                                cmd.Parameters.AddWithValue("@Ft", Function);
                                cmd.Parameters.AddWithValue("@Lc", LocCode);
                                cmd.Parameters.AddWithValue("@Pn", DocNo);
                                cmd.Parameters.AddWithValue("@Cd", Code);
                                cmd.Parameters.AddWithValue("@Rmd", Items);
                                cmd.Parameters.AddWithValue("@Rq", RecQty);
                                cmd.Parameters.AddWithValue("@Tq", TransQty);
                                cmd.Parameters.AddWithValue("@Sv", StockValue);
                                cmd.Parameters.AddWithValue("@Rd", RegNow);
                                cmd.Parameters.AddWithValue("@Rb", User);
                                cmd.Parameters.AddWithValue("@Cs", 0);
                                cmd.Parameters.AddWithValue("@Rm", Remark);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                dtError.Rows.Add(Code, Items, LocName, DocNo, Remark);
                            }
                        }
                        //}
                        //catch
                        //{
                        //    dtError.Rows.Add(Code, Items, LocName, DocNo, Remark);
                        //}
                        cnn.con.Close();
                    }

                    if (dtError.Rows.Count == 0)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("រក្សាទុករួចរាល់", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvInput.Rows.Clear();
                    }
                    else
                    {
                        string Msg = "រក្សាទុកមានបញ្ហា ៖ ";
                        foreach (DataRow row in dtError.Rows)
                        {
                            Msg = Msg + "\n • " + row[0] + "\t" + row[1] + "\t\t" + row[2] + "\t" + row[3] + "\t\t" + row[4];
                        }
                        Cursor = Cursors.Default;
                        MessageBox.Show(Msg, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        for (int i = dgvInput.Rows.Count - 1; i > -1; i--)
                        {
                            int Found = 0;
                            string Code = dgvInput.Rows[i].Cells[0].Value.ToString();
                            string LocName = dgvInput.Rows[i].Cells[2].Value.ToString();
                            string DocNo = dgvInput.Rows[i].Cells[5].Value.ToString();
                            string Remark = dgvInput.Rows[i].Cells[6].Value.ToString();

                            foreach (DataRow row in dtError.Rows)
                            {
                                if (row[0].ToString() == Code && row[2].ToString() == LocName && row[3].ToString() == DocNo && row[4].ToString() == Remark)
                                {
                                    Found = Found + 1;
                                    break;
                                }
                            }
                            if (Found > 0)
                            {
                                dgvInput.Rows.RemoveAt(i);
                            }
                        }
                        dgvInput.Update();
                    }

                }
            }
        }


        private void WireReceiveAndIssueForm_Load(object sender, EventArgs e)
        {
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbSDMCLocation", cnn.con);
                dtLocation = new DataTable();
                sda.Fill(dtLocation);
                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT ItemCode, ItemName FROM tbMasterItem WHERE Remarks1 IS NULL ", cnn.con);
                dtRM = new DataTable();
                sda1.Fill(dtRM); 
                SqlDataAdapter sda2 = new SqlDataAdapter("SELECT * FROM tbSDMCFunction WHERE FuncCode IN (1,7)", cnn.con);
                dtFunction = new DataTable();
                sda2.Fill(dtFunction);
                foreach (DataRow row in dtLocation.Rows)
                {
                    CboLocation.Items.Add(row[1].ToString());
                }
                foreach (DataRow row in dtFunction.Rows)
                {
                    CboFunction.Items.Add(row[1].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n"+ex.Message,"Rachhan System",MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
            cnn.con.Close();
        }

        private void TakeCurrentStockQty()
        {
            string LocCode = "";
            foreach (DataRow row in dtLocation.Rows)
            {
                if (row[1].ToString() == CboLocation.Text) 
                {
                    LocCode = row[0].ToString();
                    break;
                }
            }

            try
            {
                //cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM " +
                    "(SELECT LocCode, Code, POSNo, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction WHERE CancelStatus=0 " +
                    "GROUP BY LocCode, Code, POSNo) T1 " +
                    "WHERE StockValue>0 AND LocCode = '"+LocCode+"' AND Code = '"+txtCode.Text+"' AND POSNo = '"+CboRemark.Text+"' ", cnn.con);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    txtCurrentQty.Text = Convert.ToDouble(dt.Rows[0][3].ToString()).ToString("N3");
                }
                txtQty.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //cnn.con.Close();
        }
        private void TakeItemName()
        {
            txtItems.Text = "";
            txtQty.Text = "";
            string LocCode = "";
            foreach (DataRow row in dtLocation.Rows)
            {
                if (row[1].ToString() == CboLocation.Text)
                {
                    LocCode = row[0].ToString(); 
                    break;
                }
            }
            //Take ItemName
            foreach (DataRow row in dtRM.Rows)
            {
                if (txtCode.Text == row[0].ToString())
                {
                    txtItems.Text = row[1].ToString();
                    break;
                }
            }
            //TakeStockRemain
            try
            {
                //cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM " +
                    "(SELECT LocCode, Code, POSNo, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction WHERE CancelStatus=0 " +
                    "GROUP BY LocCode, Code, POSNo) T1 " +
                    "WHERE StockValue>0 AND LocCode = '"+LocCode+"' AND Code = '"+txtCode.Text+"' ", cnn.con);
                DataTable dtRemark = new DataTable();
                sda.Fill(dtRemark);
                CboRemark.Items.Clear();
                foreach (DataRow row in dtRemark.Rows)
                {
                    CboRemark.Items.Add(row[2].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //cnn.con.Close();
            CboRemark.Focus();
        }
        private void HidePanelItems()
        {
            if (DgvSearchItem.Focused == false && txtSearchItem.Focused == false)
            {
                panelSearchItem.SendToBack();
            }
        }
        private void ClearInputText()
        {
            txtCode.Text = "";
            txtItems.Text = "";
            CboRemark.Text = "";
            txtCurrentQty.Text = "0.00";
            txtQty.Text = "";
            txtRemark.Text = "";
        }
        private void CheckBtnSaveAndDelete()
        {
            if(dgvInput.SelectedCells.Count>0)
            {
                btnDelete.Enabled = true;
                btnDelete.BackColor = Color.White;
            }
            else
            {
                btnDelete.Enabled = false;
                btnDelete.BackColor = Color.DarkGray;
            }

            if (dgvInput.Rows.Count > 0)
            {
                btnSave.Enabled = true;
                btnSave.BackColor = Color.White;
            }
            else
            {
                btnSave.Enabled = false;
                btnSave.BackColor = Color.DarkGray;
            }
        }
        private void AssignNoToDgv()
        {
            int No = 1;
            foreach (DataGridViewRow DgvRow in dgvInput.Rows)
            {
                DgvRow.HeaderCell.Value = No.ToString();
                No = No + 1;
            }
        }

    }
}
