using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    public partial class WireStockINScanForm : Form
    {
        SqlCommand cmd;

        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        double BeforeEdit;

        public WireStockINScanForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += WireStockINScanForm_Load;

            //Txt
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;

            //Dgv
            this.dgvScanned.CellFormatting += DgvScanned_CellFormatting;
            this.dgvScanned.CellBeginEdit += DgvScanned_CellBeginEdit;
            this.dgvScanned.CellEndEdit += DgvScanned_CellEndEdit;
            this.dgvScanned.CellClick += DgvScanned_CellClick;

            //Btn
            this.btnDelete.Click += BtnDelete_Click;
            this.btnSave.Click += BtnSave_Click;
        }

        //Btn
        private void BtnSave_Click(object sender, EventArgs e)
        {
            DialogResult DSL = MessageBox.Show("តើអ្នកចង់រក្សាទុកទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DSL == DialogResult.Yes)
            {
                SaveToDB();
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult DSL = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនទេ?","Rachhan System",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (DSL == DialogResult.Yes)
            {
                dgvScanned.Rows.RemoveAt(dgvScanned.CurrentCell.RowIndex);
                dgvScanned.Refresh();
                AssignNumber();
                dgvScanned.ClearSelection();
                CheckButtonSaveDelete();
            }
        }

        //Dgv
        private void DgvScanned_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                e.CellStyle.ForeColor = Color.Black;
            }
        }
        private void DgvScanned_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                double NewValue = Convert.ToDouble(dgvScanned.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                NewValue = Math.Round(NewValue,0);
                if (NewValue > 0)
                {
                    if (NewValue <= Convert.ToDouble(dgvScanned.Rows[e.RowIndex].Cells[5].Value.ToString()))
                    {
                        dgvScanned.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = NewValue;
                    }
                    else
                    {
                        MessageBox.Show("អ្នកបញ្ចូលលើសចំនួនដែលនៅសល់ហើយ!\nចំនួននៅសល់ ៖ " + Convert.ToInt32(dgvScanned.Rows[e.RowIndex].Cells[5].Value.ToString()).ToString("N0"), "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dgvScanned.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = BeforeEdit;
                    }
                }
                else
                {
                    MessageBox.Show("ចំនួនត្រូវតែលើសពី ០!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    dgvScanned.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = BeforeEdit;
                }
            }
            catch
            {
                MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dgvScanned.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = BeforeEdit;
            }
        }
        private void DgvScanned_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            BeforeEdit = Convert.ToDouble(dgvScanned.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
        }
        private void DgvScanned_CellClick(object sender, DataGridViewCellEventArgs e)
        {            
            CheckButtonSaveDelete();
        }

        //Txt
        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtBarcode.Text.Trim() != "")
                {
                    if (txtBarcode.Text.ToString().Contains("MAT")==true)
                    {
                        dgvScanned.Focus();
                        ScanAsMATRequest();
                    }
                    else
                    {
                        dgvScanned.Focus();
                        ScanAsRemarkRequest();
                    }
                    CheckButtonSaveDelete();
                }
            }
        }

        private void WireStockINScanForm_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn Col in dgvScanned.Columns)
            {
                if (Col.Index != 2)
                {
                    Col.ReadOnly = true;
                }
            }
        }

        private void ScanAsMATRequest()
        {
            string Remarks = "";
            DataTable dtOBS = new DataTable();
            try
            {
                cnnOBS.conOBS.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, SUM(ReceiveQty) AS ReceiveQty, Remark, MatReqNo FROM " +
                                                                                    "(SELECT T1.ItemCode, ItemName, ReceiveQty, T1.Remark, T2.MatReqNo FROM " +
                                                                                    "(SELECT * FROM prgalltransaction WHERE LocCode = 'MC1' AND GRICode ='50' AND TypeCode=1) T1 " +
                                                                                    "INNER JOIN " +
                                                                                    "(SELECT * FROM PrgMatRequest WHERE NOT TRIM(Remark)='') T2 " +
                                                                                    "ON T1.Remark=T2.Remark AND T1.ItemCode=T2.ItemCode " +
                                                                                    "INNER JOIN " +
                                                                                    "(SELECT * FROM mstitem WHERE DelFlag=0 AND MatCalcFlag=1) T3 " +
                                                                                    "ON T2.ItemCode=T3.ItemCode) TB1 " +
                                                                                    "WHERE MatReqNo ='" + txtBarcode.Text.ToString() + "' " +
                                                                                    "GROUP BY ItemCode, ItemName, Remark, MatReqNo " +
                                                                                    "ORDER BY MatReqNo ASC", cnnOBS.conOBS);
                sda.Fill(dtOBS);
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnnOBS.conOBS.Close();

            if (dtOBS.Rows.Count > 0)
            {
                Remarks = dtOBS.Rows[0][3].ToString();
                DataTable dtMCRec = new DataTable();
                try
                {
                    cnn.con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT Code, SUM(ReceiveQty) AS ReceiveQty FROM tbSDMCAllTransaction WHERE Funct=1 AND CancelStatus =0 AND LocCode='WIR1' " +
                                                                                        "AND POSNo='" + Remarks + "' " +
                                                                                        "GROUP BY Code", cnn.con);
                    sda.Fill(dtMCRec);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnn.con.Close();

                foreach (DataRow row in dtOBS.Rows)
                {
                    string Code = row[0].ToString();
                    string Name = row[1].ToString();
                    double WHQty = Convert.ToDouble(row[2].ToString());
                    string Remark = row[3].ToString();
                    double MCRec = 0;
                    foreach (DataRow MCRow in dtMCRec.Rows)
                    {
                        if (Code == MCRow[0].ToString())
                        {
                            MCRec = Convert.ToDouble(MCRow[1].ToString());
                            break;
                        }
                    }
                    double RemainQty = WHQty - MCRec;
                    if (RemainQty > 0)
                    {
                        int Dupl = 0;
                        foreach (DataGridViewRow DgvRow in dgvScanned.Rows)
                        {
                            if (Code == DgvRow.Cells[0].Value.ToString() && Remark == DgvRow.Cells[6].Value.ToString())
                            {
                                Dupl = Dupl + 1;
                            }
                        }

                        if (Dupl == 0)
                        {
                            dgvScanned.Rows.Add(Code, Name, RemainQty, MCRec, WHQty, RemainQty, Remark);
                            AssignNumber();
                        }
                        else
                        {
                            MessageBox.Show("ទិន្នន័យនេះមានរួចហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        MessageBox.Show("អ្នកទទួលគ្រប់ចំនួនហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                dgvScanned.ClearSelection();
                txtBarcode.Text = "";
                txtBarcode.Focus();
            }
            else
            {
                MessageBox.Show("គ្មានទិន្នន័យវេរទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBarcode.Focus();
                txtBarcode.SelectAll();
            }
        }
        private void ScanAsRemarkRequest()
        {
            dgvScanned.Rows.Clear();
            DataTable dtMCRec  = new DataTable();
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT Code, SUM(ReceiveQty) AS ReceiveQty FROM tbSDMCAllTransaction WHERE Funct=1 AND CancelStatus =0 AND LocCode='WIR1' " +
                                                                                    "AND POSNo='"+txtBarcode.Text.ToString()+"' " +
                                                                                    "GROUP BY Code", cnn.con);
                sda.Fill(dtMCRec);
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

            try
            {
                cnnOBS.conOBS.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, SUM(ReceiveQty) AS ReceiveQty, Remark, MatReqNo FROM " +
                                                                                    "(SELECT T1.ItemCode, ItemName, ReceiveQty, T1.Remark, T2.MatReqNo FROM " +
                                                                                    "(SELECT * FROM prgalltransaction WHERE LocCode = 'MC1' AND GRICode ='50' AND TypeCode=1) T1 " +
                                                                                    "INNER JOIN " +
                                                                                    "(SELECT * FROM PrgMatRequest WHERE NOT TRIM(Remark)='') T2 " +
                                                                                    "ON T1.Remark=T2.Remark AND T1.ItemCode=T2.ItemCode " +
                                                                                    "INNER JOIN " +
                                                                                    "(SELECT * FROM mstitem WHERE DelFlag=0 AND MatCalcFlag=1) T3 " +
                                                                                    "ON T2.ItemCode=T3.ItemCode) TB1 " +
                                                                                    "WHERE Remark ='"+txtBarcode.Text.ToString()+"' " +
                                                                                    "GROUP BY ItemCode, ItemName, Remark, MatReqNo " +
                                                                                    "ORDER BY MatReqNo ASC", cnnOBS.conOBS);
                DataTable dtOBS = new DataTable();
                sda.Fill(dtOBS);
                if (dtOBS.Rows.Count > 0)
                {
                    foreach (DataRow row in dtOBS.Rows)
                    {
                        string Code = row[0].ToString();
                        string Name = row[1].ToString();
                        double WHQty = Convert.ToDouble(row[2].ToString());
                        string Remark = row[3].ToString();
                        double MCRec = 0;
                        foreach (DataRow MCRow in dtMCRec.Rows)
                        {
                            if (Code == MCRow[0].ToString())
                            {
                                MCRec = Convert.ToDouble(MCRow[1].ToString()); 
                                break;
                            }
                        }
                        double RemainQty = WHQty-MCRec;
                        if (RemainQty > 0)
                        {
                            dgvScanned.Rows.Add(Code, Name, RemainQty, MCRec, WHQty, RemainQty, Remark);
                            AssignNumber();
                        }
                    }
                    dgvScanned.ClearSelection();
                    txtBarcode.Text = "";
                    if (dgvScanned.Rows.Count == 0)
                    {
                        MessageBox.Show("អ្នកទទួលគ្រប់ចំនួនហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtBarcode.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("គ្មានទិន្នន័យវេរទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtBarcode.Focus();
                    txtBarcode.SelectAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n"+ex.Message,"Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnnOBS.conOBS.Close();
        }
        private void AssignNumber()
        {
            int rowNumber = 1;
            foreach (DataGridViewRow row in dgvScanned.Rows)
            {
                row.HeaderCell.Value = rowNumber.ToString();
                rowNumber = rowNumber + 1;
            }
        }
        private void CheckButtonSaveDelete()
        {
            if (dgvScanned.SelectedCells.Count > 0)
            {
                this.btnDelete.Enabled = true;
                this.btnDelete.BackColor = Color.FromKnownColor(KnownColor.White);
            }
            else
            {
                this.btnDelete.Enabled = false;
                this.btnDelete.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
            }

            if (dgvScanned.Rows.Count > 0)
            {
                this.btnSave.Enabled = true;
                this.btnSave.BackColor = Color.FromKnownColor(KnownColor.White);
            }
            else
            {
                this.btnSave.Enabled = false;
                this.btnSave.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
            }

        }
        private void SaveToDB()
        {
            LbStatus.Text = "កំពុងរក្សាទុក . . . .";
            LbStatus.Refresh();
            Cursor = Cursors.WaitCursor;
            string Errors = "";
            DateTime RegNow = DateTime.Now;
            string User = MenuFormV2.UserForNextForm;
            string TransNo = "";

            //Find Last TransNo
            cnn.con.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT SysNo FROM tbSDMCAllTransaction " +
                                                                            "WHERE " +
                                                                            "RegDate=(SELECT MAX(RegDate) FROM tbSDMCAllTransaction WHERE Funct =1) AND " +
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

            //Save
            try
            {
                //Add to tbSDMCAllTransaction
                for (int i = 0; i < dgvScanned.Rows.Count; i++)
                {
                    cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus ) " +
                                                   "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs)", cnn.con);
                    cmd.Parameters.AddWithValue("@Sn", TransNo);
                    cmd.Parameters.AddWithValue("@Ft", 1);
                    cmd.Parameters.AddWithValue("@Lc", "WIR1");
                    cmd.Parameters.AddWithValue("@Pn", dgvScanned.Rows[i].Cells[6].Value.ToString());
                    cmd.Parameters.AddWithValue("@Cd", dgvScanned.Rows[i].Cells[0].Value.ToString());
                    cmd.Parameters.AddWithValue("@Rmd", dgvScanned.Rows[i].Cells[1].Value.ToString());
                    cmd.Parameters.AddWithValue("@Rq", Convert.ToDouble(dgvScanned.Rows[i].Cells[2].Value.ToString()));
                    cmd.Parameters.AddWithValue("@Tq", 0);
                    cmd.Parameters.AddWithValue("@Sv", Convert.ToDouble(dgvScanned.Rows[i].Cells[2].Value.ToString()));
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
                LbStatus.Text = "";
                LbStatus.Refresh();
                dgvScanned.Rows.Clear();
                txtBarcode.Focus();
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
    }
}
