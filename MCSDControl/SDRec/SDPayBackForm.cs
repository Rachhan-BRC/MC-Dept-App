using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.MCSDControl.SDRec
{
    public partial class SDPayBackForm : Form
    {
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SQLConnect cnn = new SQLConnect();
        SqlCommand cmd;
        DataTable dtRMMaster;
        double QtyBeginEdit;

        public SDPayBackForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += SDPayBackForm_Load;
            this.dgvItems.LostFocus += DgvItems_LostFocus;
            this.dgvItems.KeyPress += DgvItems_KeyPress;
            this.dgvItems.CellClick += DgvItems_CellClick;
            this.txtQty.KeyPress += TxtQty_KeyPress;
            this.txtQty.KeyDown += TxtQty_KeyDown;
            this.txtCode.KeyDown += TxtCode_KeyDown;
            this.txtCode.Leave += TxtCode_Leave;
            this.dgvInput.CellFormatting += DgvInput_CellFormatting;
            this.dgvInput.CellBeginEdit += DgvInput_CellBeginEdit;
            this.dgvInput.CellEndEdit += DgvInput_CellEndEdit;
            this.dgvInput.KeyDown += DgvInput_KeyDown;
            this.btnShowDgvItems.Click += BtnShowDgvItems_Click;
            this.btnOK.Click += BtnOK_Click;
            this.btnNew.Click += BtnNew_Click;
            this.btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DialogResult DLR = MessageBox.Show("តើអ្នករក្សាទុកទិន្នន័យនេះមែនទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DLR == DialogResult.Yes)
            {
                SaveToDB();
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            picItem.Visible = false;
            PicQty.Visible = false;
            dgvInput.Rows.Clear();
            ClearAll();
            CheckBtnSave();
        }

        private async void BtnOK_Click(object sender, EventArgs e)
        {
            int CountDuplicate = 0;
            foreach (DataGridViewRow row in dgvInput.Rows)
            {
                if (row.Cells[0].Value.ToString() == txtCode.Text)
                {
                    CountDuplicate = CountDuplicate + 1;
                }
            }

            if (CountDuplicate > 0)
            {
                MessageBox.Show("វត្ថុធាតុដើមនេះបានបញ្ចូលរួចហើយ!\nសូមបូកទិន្នន័យបញ្ចូលគ្នា រួចបញ្ចូលវាឡើងវិញនៅជួរទី៣!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //Blink Blink
                for (int i = 0; i < 5; i++)
                {
                    picItem.Visible = !picItem.Visible;
                    await Task.Delay(500);
                }
                picItem.Visible = true;
            }
            else
            {
                AddOK();
                CheckBtnSave();
            }

        }

        private void BtnShowDgvItems_Click(object sender, EventArgs e)
        {
            ShowDgvItem();
        }

        private void SDPayBackForm_Load(object sender, EventArgs e)
        {
            string[] CountRMExcept = new string[] { "2114", "1000", "1053", "1132", "1138", "0386", "1015", "1019" };
            string Code = "";
            foreach (string item in CountRMExcept)
            {
                if (Code.Trim() == "")
                {
                    Code = "'" + item + "' ";
                }
                else
                {
                    Code = Code + ", '" + item + "' ";
                }
            }
            try
            {
                cnnOBS.conOBS.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName FROM mstitem WHERE ItemType=2 AND MatCalcFlag=0 AND NOT ItemCode IN(" + Code + ")", cnnOBS.conOBS);
                dtRMMaster = new DataTable();
                sda.Fill(dtRMMaster);
                for (int i = 0; i < dtRMMaster.Rows.Count; i++)
                {
                    dgvItems.Rows.Add(dtRMMaster.Rows[i][0].ToString(), dtRMMaster.Rows[i][1].ToString());
                }
                for (int i = 0; i < dgvInput.Columns.Count - 1; i++)
                {
                    dgvInput.Columns[i].ReadOnly = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnnOBS.conOBS.Close();

        }

        private void DgvInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (dgvInput.SelectedCells.Count > 0)
            {
                int selectedIndex = dgvInput.CurrentCell.RowIndex;
                if (e.KeyCode == Keys.Delete)
                {
                    DialogResult DLR = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DLR == DialogResult.Yes)
                    {
                        dgvInput.Rows.RemoveAt(selectedIndex);
                        dgvInput.Refresh();
                        AssignNo();
                        dgvInput.ClearSelection();
                        CheckBtnSave();
                    }
                }
            }
        }

        private void DgvInput_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvInput.CurrentCell == dgvInput[2, e.RowIndex])
            {
                try
                {
                    double InputQty = Math.Round(Convert.ToDouble(dgvInput[e.ColumnIndex, e.RowIndex].Value.ToString()), 1, MidpointRounding.AwayFromZero);
                    double Qty = Math.Round(InputQty, MidpointRounding.AwayFromZero);
                    if (Qty == 0)
                    {
                        MessageBox.Show("ចំនួនមិនអាច 0 បានទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dgvInput[e.ColumnIndex, e.RowIndex].Value = QtyBeginEdit;
                    }
                    else if (Qty < 0)
                    {
                        MessageBox.Show("ចំនួនមិនអាចតិចជាង 0 បានទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dgvInput[e.ColumnIndex, e.RowIndex].Value = QtyBeginEdit;
                    }
                    else
                    {
                        dgvInput[e.ColumnIndex, e.RowIndex].Value = Qty;

                    }
                }
                catch
                {
                    MessageBox.Show("បញ្ចូលខុសទម្រង់ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dgvInput[e.ColumnIndex, e.RowIndex].Value = QtyBeginEdit;
                }
            }
        }

        private void DgvInput_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvInput.CurrentCell == dgvInput[2, e.RowIndex])
            {
                QtyBeginEdit = Convert.ToDouble(dgvInput[e.ColumnIndex, e.RowIndex].Value.ToString());

            }
        }

        private void DgvInput_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.Value.ToString() != "")
            {
                e.CellStyle.ForeColor = Color.Black;
            }
        }

        private void TxtCode_Leave(object sender, EventArgs e)
        {
            txtItems.Text = "";
            foreach (DataRow row in dtRMMaster.Rows)
            {
                if (row[0].ToString() == txtCode.Text)
                {
                    txtItems.Text = row[1].ToString();
                }
            }
        }

        private void TxtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtQty.Focus();
            }
        }

        private void TxtQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK.PerformClick();
            }
        }

        private void TxtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void DgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dgvItems.Rows[e.RowIndex];
            txtCode.Text = row.Cells[0].Value.ToString();
            txtItems.Text = row.Cells[1].Value.ToString();
            txtQty.Focus();
        }

        private void DgvItems_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                if (txtItems.Text.Length > 0)
                {
                    string Items = txtItems.Text;
                    Items = Items.Substring(0, Items.Length - 1);
                    txtItems.Text = Items;
                    ShowDgvItem();
                }
            }
            else
            {
                string Items = txtItems.Text;
                Items = Items + e.KeyChar;
                txtItems.Text = Items;
                ShowDgvItem();
            }
        }

        private void DgvItems_LostFocus(object sender, EventArgs e)
        {
            this.dgvItems.Visible = false;
            this.dgvItems.SendToBack();
        }

        private void ClearAll()
        {
            txtCode.Text = "";
            txtItems.Text = "";
            txtQty.Text = "";
            txtCode.Focus();
        }

        private void SaveToDB()
        {
            LbStatus.Text = "កំពុងរក្សាទុក . . . .";
            LbStatus.Visible = true;
            LbStatus.Refresh();
            Cursor = Cursors.WaitCursor;
            string Errors = "";

            DateTime RegNow = DateTime.Now;
            string User = MenuFormV2.UserForNextForm;
            string TransNo = "";
            try
            {
                //Find Last TransNo
                cnn.con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT SysNo FROM tbSDMCAllTransaction " +
                                                                                "WHERE " +
                                                                                "RegDate=(SELECT MAX(RegDate) FROM tbSDMCAllTransaction WHERE Funct =4) AND " +
                                                                                "SysNo=(SELECT MAX(SysNo) FROM tbSDMCAllTransaction WHERE Funct =4) Group By SysNo", cnn.con);
                DataTable dtTransNo = new DataTable();
                da.Fill(dtTransNo);
                if (dtTransNo.Rows.Count == 0)
                {
                    TransNo = "PAY0000000001";
                }
                else
                {
                    string LastTransNo = dtTransNo.Rows[0][0].ToString();
                    double NextTransNo = Convert.ToDouble(LastTransNo.Substring(LastTransNo.Length - 10)) + 1;
                    TransNo = "PAY" + NextTransNo.ToString("0000000000");

                }

                //Save
                for (int i = 0; i < dgvInput.Rows.Count; i++)
                {
                    string LotNo = RegNow.ToString("yyyy") + RegNow.ToString("MM") + RegNow.ToString("dd");
                    string Code = dgvInput.Rows[i].Cells[0].Value.ToString();
                    string RMName = dgvInput.Rows[i].Cells[1].Value.ToString();
                    double TransQty = Convert.ToDouble(dgvInput.Rows[i].Cells[2].Value.ToString());



                    //Add to AllTransaction
                    cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus ) " +
                                                   "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs)", cnn.con);
                    cmd.Parameters.AddWithValue("@Sn", TransNo);
                    cmd.Parameters.AddWithValue("@Ft", 4);
                    cmd.Parameters.AddWithValue("@Lc", "KIT3");
                    cmd.Parameters.AddWithValue("@Pn", LotNo);
                    cmd.Parameters.AddWithValue("@Cd", Code);
                    cmd.Parameters.AddWithValue("@Rmd", RMName);
                    cmd.Parameters.AddWithValue("@Rq", TransQty);
                    cmd.Parameters.AddWithValue("@Tq", 0);
                    cmd.Parameters.AddWithValue("@Sv", TransQty);
                    cmd.Parameters.AddWithValue("@Rd", RegNow);
                    cmd.Parameters.AddWithValue("@Rb", User);
                    cmd.Parameters.AddWithValue("@Cs", 0);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Errors = Errors + "\t" + ex.Message;
            }
            cnn.con.Close();

            if (Errors.Trim() == "")
            {
                Cursor = Cursors.Default;
                LbStatus.Text = "រក្សាទុករួចរាល់";
                MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LbStatus.Visible = false;
                btnNew.PerformClick();

            }
            else
            {
                LbStatus.Text = "រក្សាទុកមានបញ្ហា";
                //Create txt file for report Errors
                string y = DateTime.Now.ToString("yyyy");
                string m = DateTime.Now.ToString("MM");
                string d = DateTime.Now.ToString("dd");
                string hh = DateTime.Now.ToString("hh");
                string mm = DateTime.Now.ToString("mm");
                string ss = DateTime.Now.ToString("ss");
                string FileName = "Error" + y + m + d + hh + mm + ss;
                string ErrorPath = (Environment.CurrentDirectory).ToString() + @"\ErrorReport\" + FileName + ".txt";
                using (FileStream fs = File.Create(ErrorPath))
                {
                    fs.Write(Encoding.UTF8.GetBytes(Errors), 0, Errors.Length);
                }
                Cursor = Cursors.Default;
                MessageBox.Show("រក្សាទុកមានបញ្ហា! File txt លម្អិតអំពី Error : \n" + ErrorPath, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckBtnSave()
        {
            if (dgvInput.Rows.Count > 0)
            {
                btnSave.Enabled = true;
                btnSave.BackColor = Color.White;
            }
            else
            {
                btnSave.Enabled = false;
                btnSave.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
            }
        }

        private void AssignNo()
        {
            int rowNumber = 1;
            foreach (DataGridViewRow row in dgvInput.Rows)
            {
                row.HeaderCell.Style.BackColor = Color.FromKnownColor(KnownColor.GradientActiveCaption);
                row.HeaderCell.Style.Font = new Font("Khmer OS Battambong", 10, FontStyle.Regular);
                row.HeaderCell.Value = rowNumber.ToString();
                rowNumber = rowNumber + 1;
            }
        }

        private async void AddOK()
        {
            picItem.Visible = false;
            PicQty.Visible = false;
            if ((txtItems.Text.Trim() == "" || txtCode.Text.Trim() == "") && txtQty.Text.Trim() == "")
            {
                //Blink Blink
                for (int i = 0; i < 5; i++)
                {
                    picItem.Visible = !picItem.Visible;
                    PicQty.Visible = !PicQty.Visible;
                    await Task.Delay(500);
                }
                picItem.Visible = true;
                PicQty.Visible = true;
            }
            else if (txtItems.Text.Trim() == "" || txtCode.Text.Trim() == "")
            {
                //Blink Blink
                for (int i = 0; i < 5; i++)
                {
                    picItem.Visible = !picItem.Visible;
                    await Task.Delay(500);
                }
                picItem.Visible = true;
            }
            else if (txtQty.Text.Trim() == "")
            {
                //Blink Blink
                for (int i = 0; i < 5; i++)
                {
                    PicQty.Visible = !PicQty.Visible;
                    await Task.Delay(500);
                }
                PicQty.Visible = true;
            }
            else
            {
                string Code = txtCode.Text;
                string Items = txtItems.Text;
                double Qty = 0;
                int ItemError = 0;

                //Check Code & Item
                foreach (DataRow row in dtRMMaster.Rows)
                {
                    if (row[0].ToString() == Code && row[1].ToString() != Items)
                    {
                        ItemError = ItemError + 1;
                    }
                }

                if (ItemError > 0)
                {
                    //Blink Blink
                    for (int i = 0; i < 5; i++)
                    {
                        picItem.Visible = !picItem.Visible;
                        await Task.Delay(500);
                    }
                    picItem.Visible = true;
                }
                else
                {
                    //Check Qty
                    try
                    {
                        Qty = Convert.ToDouble(txtQty.Text);
                        dgvInput.Rows.Add(Code, Items, Qty);
                        AssignNo();
                        ClearAll();
                        dgvInput.ClearSelection();
                    }
                    catch
                    {
                        //Blink Blink
                        for (int i = 0; i < 5; i++)
                        {
                            PicQty.Visible = !PicQty.Visible;
                            await Task.Delay(500);
                        }
                        PicQty.Visible = true;
                    }
                }

            }
        }

        private void AutoSizeDgvItem()
        {
            int Hei = 0;
            int Wid = 384;
            Hei = dgvItems.RowTemplate.Height * dgvItems.Rows.Count + 3;
            if (Hei > 221)
            {
                dgvItems.Size = new Size(Wid, 221);
            }
            else
            {
                dgvItems.Size = new Size(Wid, Hei);
            }
        }

        private void ShowDgvItem()
        {
            dgvItems.Rows.Clear();
            for (int i = 0; i < dtRMMaster.Rows.Count; i++)
            {
                if (dtRMMaster.Rows[i][1].ToString().ToLower().Contains(this.txtItems.Text.ToString().ToLower()))
                {
                    dgvItems.Rows.Add(dtRMMaster.Rows[i][0].ToString(), dtRMMaster.Rows[i][1].ToString());
                }
            }
            if (dgvItems.Rows.Count > 1)
            {
                AutoSizeDgvItem();

            }
            this.dgvItems.BringToFront();
            this.dgvItems.Visible = true;
            dgvItems.Focus();
        }

    }
}
