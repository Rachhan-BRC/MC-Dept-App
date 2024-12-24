using MachineDeptApp.InputTransferSemi;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.SemiNGReq
{
    public partial class SemiNGReqInputForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SqlCommand cmd;
        public DataTable dtRecLocation;
        public static string WipCode;
        public static string WipName;
        public static string Pin;
        public static string Wire;
        public static string Length;
        string LocBeginEdit;
        double QtyBeginEdit;


        public SemiNGReqInputForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.txtWipNameS.KeyDown += TxtWipNameS_KeyDown;
            this.dgvWipCode.CellDoubleClick += DgvWipCode_CellDoubleClick;
            this.dgvInput.SelectionChanged += DgvInput_SelectionChanged;
            this.dgvInput.CellFormatting += DgvInput_CellFormatting;
            this.dgvInput.CellBeginEdit += DgvInput_CellBeginEdit;
            this.dgvInput.CellEndEdit += DgvInput_CellEndEdit;

        }

        private void DgvInput_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                double Digit1 = Math.Round(Convert.ToDouble(dgvInput[2, e.RowIndex].Value.ToString()), 1, MidpointRounding.AwayFromZero);
                double Digit0 = Math.Round(Digit1, 0, MidpointRounding.AwayFromZero);
                double Qty = Digit0;
                if (Qty <= 0)
                {
                    MessageBox.Show("ចំនួនមិនអាចតូចជាង 0 បានទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dgvInput[2, e.RowIndex].Value = QtyBeginEdit;
                }
                else
                {
                    string Code= dgvInput[0, e.RowIndex].Value.ToString();
                    string CurrentLoc = dgvInput[6, e.RowIndex].Value.ToString();
                    int FoundDupli = 0;
                    for (int i = 0; i < dgvInput.Rows.Count; i++)
                    {
                        if (CurrentLoc == dgvInput.Rows[i].Cells[6].Value.ToString() && Qty == Convert.ToDouble(dgvInput.Rows[i].Cells[2].Value.ToString()) && Code== dgvInput.Rows[i].Cells[0].Value.ToString())
                        {
                            FoundDupli=FoundDupli + 1;
                        }
                    }
                    if (FoundDupli == 1)
                    {
                        dgvInput[2, e.RowIndex].Value = Qty;
                        dgvInput[6, e.RowIndex].Value = CurrentLoc;
                    }
                    else
                    {
                        MessageBox.Show("ទិន្នន័យនេះមានរួចហើយសូមលុបចោលមួយ!","Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                        dgvInput[2, e.RowIndex].Value = QtyBeginEdit;
                        dgvInput[6, e.RowIndex].Value = LocBeginEdit;
                    }
                }
            }
            catch
            {
                MessageBox.Show("បញ្ចូលខុសទម្រង់ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvInput[2, e.RowIndex].Value = QtyBeginEdit;
            }
        }

        private void DgvInput_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            QtyBeginEdit = Convert.ToDouble(dgvInput[2, e.RowIndex].Value.ToString());
            LocBeginEdit = dgvInput[6, e.RowIndex].Value.ToString();
        }

        private void DgvInput_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.Value.ToString() != "")
            {
                e.CellStyle.ForeColor = Color.Black;
            }
        }

        private void CheckForbtnSave()
        {
            if (dgvInput.Rows.Count > 0)
            {
                btnSave.Enabled = true;
                btnSave.BackColor = Color.White;
            }
            else
            {
                btnSave.Enabled = false;
                btnSave.BackColor = Color.FromKnownColor(KnownColor.LightGray);
            }
        }
        
        private void ReAssignRowNo()
        {
            int rowNumber = 1;
            foreach (DataGridViewRow row in dgvInput.Rows)
            {
                row.HeaderCell.Style.BackColor = Color.FromKnownColor(KnownColor.GradientActiveCaption);
                row.HeaderCell.Style.Font = new Font("Khmer OS Battambong", 11, FontStyle.Regular);
                row.HeaderCell.Value = rowNumber.ToString();
                rowNumber = rowNumber + 1;
            }
        }

        private void DgvInput_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvInput.SelectedCells.Count > 0)
            {
                btnDelete.Enabled = true;
                btnDelete.BackColor = Color.White;
            }
            else
            {
                btnDelete.Enabled = false;
                btnDelete.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
            }
        }

        private void DgvWipCode_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            WipCode = "";
            WipName = "";
            Pin = "";
            Wire = "";
            Length = "";
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvWipCode.Rows[e.RowIndex];
                WipCode = row.Cells[0].Value.ToString();
                WipName = row.Cells[1].Value.ToString();
                Pin = row.Cells[2].Value.ToString();
                Wire = row.Cells[3].Value.ToString();
                Length = row.Cells[4].Value.ToString();

                if (WipCode.Trim() != "" && WipName.Trim() != "" && Pin.Trim() != "" && Wire.Trim() != "" && Length.Trim() != "")
                {
                    txtWipNameS.Focus();
                    txtWipNameS.Text = "";
                    txtWipNameS.Focus();
                    SemiNGReqInputFormConfirm Snrfs = new SemiNGReqInputFormConfirm(this);
                    Snrfs.ShowDialog();
                    CheckForbtnSave();
                    ReAssignRowNo();
                }
            }
        }

        private void TxtWipNameS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dgvWipCode.Rows.Clear();
                SqlDataAdapter da = new SqlDataAdapter("SELECT ItemCode, ItemName, Remarks1, Remarks2, Remarks3 FROM tbMasterItem WHERE LEN(ItemCode)>4 AND ItemName LIKE '%" + txtWipNameS.Text.ToString()+"%' "+
                                                                                "GROUP BY ItemCode, ItemName, Remarks1, Remarks2, Remarks3 ORDER BY ItemCode ASC", cnn.con);
                DataTable dtWIPMaster=new DataTable();
                da.Fill(dtWIPMaster);
                foreach (DataRow row in dtWIPMaster.Rows)
                {
                    dgvWipCode.Rows.Add(row[0], row[1], row[2], row[3], row[4]);
                }
                dgvWipCode.ClearSelection();
            }
        }

        private void SemiNGReqInputForm_Load(object sender, EventArgs e)
        {            
            string[] Location = new string[] { "WIP", "Assy" };
            dtRecLocation = new DataTable();
            dtRecLocation.Columns.Add("Name");
            for (int i = 0; i < Location.Length; i++)
            {
                dtRecLocation.Rows.Add(Location[i]);
            }

            int[] ColumnNotEditable = new int[] { 0, 1, 3, 4, 5 };
            for (int i = 0; i < ColumnNotEditable.Length; i++)
            {
                dgvInput.Columns[ColumnNotEditable[i]].ReadOnly=true;
            }

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            dgvWipCode.Rows.Clear();
            dgvInput.Rows.Clear();
            txtWipNameS.Text = "";
            CheckForbtnSave();
            txtWipNameS.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvInput.SelectedCells.Count > 0)
            {
                DialogResult DLR = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនដែរ ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    int selectedIndex = dgvInput.CurrentCell.RowIndex;
                    if (selectedIndex > -1)
                    {
                        dgvInput.Rows.RemoveAt(selectedIndex);
                        dgvInput.Refresh();
                        ReAssignRowNo();
                        CheckForbtnSave();
                        dgvInput.ClearSelection();
                    }                    
                    selectedIndex = -1;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvInput.Rows.Count > 0)
            {
                DialogResult DLR = MessageBox.Show("តើអ្នកចង់រក្សាទុកទិន្នន័យនេះមែនដែរ ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    string Errors = "";
                    //Save
                    for (int i = 0; i < dgvInput.Rows.Count; i++)
                    {
                        try
                        {
                            DateTime RegNow = DateTime.Now;
                            RegNow = RegNow.AddSeconds(i);
                            string User = MenuFormV2.UserForNextForm;
                            double NextSysNo;
                            //Find Last TransNo
                            cnn.con.Open();
                            SqlDataAdapter da = new SqlDataAdapter("SELECT SysNo FROM tbSemiNGReq WHERE RegDate=(SELECT MAX(RegDate) FROM tbSemiNGReq) Group By SysNo", cnn.con);
                            DataTable dtSysNo = new DataTable();
                            da.Fill(dtSysNo);
                            if (dtSysNo.Rows.Count == 0)
                            {
                                NextSysNo = 1;
                            }
                            else
                            {
                                NextSysNo = Convert.ToDouble(dtSysNo.Rows[0][0].ToString())+1;

                            }
                            string WipCode = dgvInput.Rows[i].Cells[0].Value.ToString();
                            string WipDes = dgvInput.Rows[i].Cells[1].Value.ToString();
                            double Qty = Convert.ToDouble(dgvInput.Rows[i].Cells[2].Value.ToString());
                            string RecSection = dgvInput.Rows[i].Cells[6].Value.ToString();

                            //Add tbWIP1ScanIN
                            cmd = new SqlCommand("INSERT INTO tbSemiNGReq (SysNo, WipCode, WipDes, Qty, RecSection, RegDate, RegBy, UpdateDate, UpdateBy, DeleteStat) " +
                                                                   "VALUES (@Sn, @Wc, @Wd, @Qq, @Rs, @Rd, @Rb, @Ud, @Ub, @Ds)", cnn.con);
                            cmd.Parameters.AddWithValue("@Sn", NextSysNo);
                            cmd.Parameters.AddWithValue("@Wc", WipCode);
                            cmd.Parameters.AddWithValue("@Wd", WipDes);
                            cmd.Parameters.AddWithValue("@Qq", Qty);
                            cmd.Parameters.AddWithValue("@Rs", RecSection);
                            cmd.Parameters.AddWithValue("@Rd", RegNow);
                            cmd.Parameters.AddWithValue("@Rb", User);
                            cmd.Parameters.AddWithValue("@Ud", RegNow);
                            cmd.Parameters.AddWithValue("@Ub", User);
                            cmd.Parameters.AddWithValue("@Ds", 0);
                            cmd.ExecuteNonQuery();

                        }
                        catch (Exception ex)
                        {
                            if (Errors.Trim() != "")
                            {

                            }
                            Errors = Errors + "\n" + ex.Message;
                        }
                        cnn.con.Close();
                    }

                    if (Errors.Trim() == "")
                    {
                        MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvWipCode.Rows.Clear();
                        dgvInput.Rows.Clear();
                        CheckForbtnSave();
                        txtWipNameS.Focus();
                    }
                    else
                    {
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

                        MessageBox.Show("រក្សាទុកមានបញ្ហា! File txt លម្អិតអំពី Error : \n" + ErrorPath, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
