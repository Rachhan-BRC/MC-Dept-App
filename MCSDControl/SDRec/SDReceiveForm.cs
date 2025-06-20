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

namespace MachineDeptApp.MCSDControl.SDRec
{
    public partial class SDReceiveForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SqlCommand cmd;
        string[] CountRMExcept;
        public DataTable dtSQLSaving;
        public DataTable dtPOSConsump;
        public static string POSNo;
        public static string WIPCode;
        public static string WIPName;
        public static string Qty;
        public static string ShipDate;

        public SDReceiveForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;
            this.dgvScanned.CellClick += DgvScanned_CellClick;
            this.dgvConsumption.CellFormatting += DgvConsumption_CellFormatting;
            this.Load += SDReceiveForm_Load;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DialogResult DLR = MessageBox.Show("តើអ្នករក្សាទុកទិន្នន័យនេះមែនទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DLR == DialogResult.Yes)
            {
                LbStatus.Text = "កំពុងរក្សាទុក . . . .";
                LbStatus.Visible = true;
                LbStatus.Refresh();
                Cursor = Cursors.WaitCursor;
                string Errors = "";
                //Save
                for (int i = 0; i < dgvScanned.Rows.Count; i++)
                {
                    try
                    {
                        DateTime RegNow = DateTime.Now;
                        string User = MenuFormV2.UserForNextForm;
                        string TransNo = "";
                        //Find Last TransNo
                        cnn.con.Open();
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

                        string POSNo = dgvScanned.Rows[i].Cells[0].Value.ToString();
                        string WIPCode = dgvScanned.Rows[i].Cells[1].Value.ToString();
                        string WIPName = dgvScanned.Rows[i].Cells[2].Value.ToString();
                        double POSQty = Convert.ToDouble(dgvScanned.Rows[i].Cells[3].Value.ToString());
                        DateTime POSDelDate = Convert.ToDateTime(dgvScanned.Rows[i].Cells[4].Value.ToString());
                        double Status = 1;
                        if (dgvScanned.Rows[i].Cells[5].Value.ToString() == "True")
                        {
                            Status = 2;
                        }

                        //Add tbSDConn1Rec
                        cmd = new SqlCommand("INSERT INTO tbSDConn1Rec ( SysNo, POSNo, WIPCode, WIPName, PosQty, PosShipD, Status, RegDate, RegBy, UpdateDate, UpdateBy ) " +
                                                               "VALUES (@Sn, @Pn, @Wc, @Wn, @Pq, @Ps, @St, @Rd, @Rb, @Ud, @Ub)", cnn.con);
                        cmd.Parameters.AddWithValue("@Sn", TransNo);
                        cmd.Parameters.AddWithValue("@Pn", POSNo);
                        cmd.Parameters.AddWithValue("@Wc", WIPCode);
                        cmd.Parameters.AddWithValue("@Wn", WIPName);
                        cmd.Parameters.AddWithValue("@Pq", POSQty);
                        cmd.Parameters.AddWithValue("@Ps", POSDelDate);
                        cmd.Parameters.AddWithValue("@St", Status);
                        cmd.Parameters.AddWithValue("@Rd", RegNow);
                        cmd.Parameters.AddWithValue("@Rb", User);
                        cmd.Parameters.AddWithValue("@Ud", RegNow);
                        cmd.Parameters.AddWithValue("@Ub", User);
                        cmd.ExecuteNonQuery();


                        int SeqNo = 1;
                        //Add tbSDMCAllTransaction 
                        for (int j = 0; j < dtSQLSaving.Rows.Count; j++)
                        {
                            if (dtSQLSaving.Rows[j][0].ToString() == POSNo)
                            {
                                string Code = dtSQLSaving.Rows[j][1].ToString();
                                string RMName = dtSQLSaving.Rows[j][2].ToString();
                                double BOMQty = Convert.ToDouble(dtSQLSaving.Rows[j][3].ToString());
                                double ConsumpQty = Convert.ToDouble(dtSQLSaving.Rows[j][4].ToString());
                                double TransQty = Convert.ToDouble(dtSQLSaving.Rows[j][5].ToString());

                                //Add to tbSDConn2Consump
                                cmd = new SqlCommand("INSERT INTO tbSDConn2Consump ( SysNo, SeqNo, ItemCode, ItemName, BOMQty, ConsumpQty ) " +
                                                               "VALUES ( @Sn, @Sqn, @Ic, @In, @Bqty, @Cqty )", cnn.con);
                                cmd.Parameters.AddWithValue("@Sn", TransNo);
                                cmd.Parameters.AddWithValue("@Sqn", SeqNo);
                                cmd.Parameters.AddWithValue("@Ic", Code);
                                cmd.Parameters.AddWithValue("@In", RMName);
                                cmd.Parameters.AddWithValue("@Bqty", BOMQty);
                                cmd.Parameters.AddWithValue("@Cqty", ConsumpQty);
                                cmd.ExecuteNonQuery();
                                SeqNo = SeqNo + 1;

                                //Add to AllTransaction
                                cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks ) " +
                                                               "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs, @Rm)", cnn.con);
                                cmd.Parameters.AddWithValue("@Sn", TransNo);
                                cmd.Parameters.AddWithValue("@Ft", 1);
                                cmd.Parameters.AddWithValue("@Lc", "KIT3");
                                cmd.Parameters.AddWithValue("@Pn", POSNo);
                                cmd.Parameters.AddWithValue("@Cd", Code);
                                cmd.Parameters.AddWithValue("@Rmd", RMName);
                                cmd.Parameters.AddWithValue("@Rq", TransQty);
                                cmd.Parameters.AddWithValue("@Tq", 0);
                                cmd.Parameters.AddWithValue("@Sv", TransQty);
                                cmd.Parameters.AddWithValue("@Rd", RegNow);
                                cmd.Parameters.AddWithValue("@Rb", User);
                                cmd.Parameters.AddWithValue("@Cs", 0);
                                cmd.Parameters.AddWithValue("@Rm", POSNo);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Errors = Errors + "\t" + ex.Message;
                    }
                    cnn.con.Close();
                }

                if (Errors.Trim() == "")
                {
                    Cursor = Cursors.Default;
                    LbStatus.Text = "រក្សាទុករួចរាល់";
                    MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LbStatus.Visible = false;
                    dgvScanned.Rows.Clear();
                    dgvConsumption.Rows.Clear();
                    CleardtSQLSaving();

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
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult DLS = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនដែរឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (DLS == DialogResult.Yes)
            {
                for (int j = dtSQLSaving.Rows.Count - 1; j > -1; j--)
                {
                    if (dtSQLSaving.Rows[j][0].ToString() == dgvScanned.CurrentRow.Cells[0].Value.ToString())
                    {
                        dtSQLSaving.Rows[j].Delete();

                    }
                }

                dtSQLSaving.AcceptChanges();
                int selectedIndex = dgvScanned.CurrentCell.RowIndex;
                if (selectedIndex > -1)
                {
                    dgvScanned.Rows.RemoveAt(selectedIndex);
                    dgvScanned.Refresh();
                }
                dgvScanned.ClearSelection();
                AssignNumber();
                dgvConsumption.Rows.Clear();
                CheckButtonDelete();
                CheckButtonSave();
            }
        }
        private void SDReceiveForm_Load(object sender, EventArgs e)
        {
            CountRMExcept = new string[] { "2114", "1000", "1053", "1132", "1138", "0386", "1015", "1019" };
            CleardtSQLSaving();
        }
        private void DgvConsumption_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 4 && e.Value.ToString() != "")
            {
                if (Convert.ToDouble(dgvConsumption[3, e.RowIndex].Value.ToString()) == Convert.ToDouble(dgvConsumption[4, e.RowIndex].Value.ToString()))
                {
                    e.CellStyle.ForeColor = Color.Blue;
                    e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambong", 9, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambong", 9, FontStyle.Bold);
                }
            }
        }
        private void DgvScanned_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvConsumption.Rows.Clear();
            foreach (DataRow row in dtSQLSaving.Rows)
            {
                if (dgvScanned.CurrentRow.Cells[0].Value.ToString() == row[0].ToString())
                {
                    dgvConsumption.Rows.Add(row[1], row[2], Convert.ToDouble(row[3].ToString()), Convert.ToDouble(row[4].ToString()), Convert.ToDouble(row[5].ToString()));
                }
            }
            CheckButtonDelete();
            dgvConsumption.ClearSelection();
        }
        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtBarcode.Text.Trim() != "")
                {
                    if (txtBarcode.Text.Length < 10 || (Regex.Matches(txtBarcode.Text.ToString(), "[~!@#$%^&*()_+{}:\"<>?-]").Count) > 1 || (Regex.Matches(txtBarcode.Text.ToString(), "[-]").Count) < 1 || txtBarcode.Text.ToString().Trim()[2].ToString() != "-")
                    {
                        MessageBox.Show("បាកូដខុសទម្រង់ហើយ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtBarcode.Focus();
                        txtBarcode.SelectAll();
                    }
                    else
                    {
                        POSNo = txtBarcode.Text;
                        ScanBarcode();
                        AssignNumber();
                        if (dgvScanned.Rows.Count > 0)
                        {
                            this.dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells[0].Selected = true;

                        }
                        this.dgvScanned.ClearSelection();
                        CheckButtonDelete();
                        CheckButtonSave();
                    }
                }
            }
        }
        private void CheckButtonSave()
        {
            if (dgvScanned.Rows.Count > 0)
            {
                btnSave.Enabled = true;
                btnSave.BackColor = Color.FromKnownColor(KnownColor.White);
            }
            else
            {
                btnSave.Enabled = false;
                btnSave.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
            }
        }
        private void CheckButtonDelete()
        {
            if (dgvScanned.SelectedRows.Count > 0)
            {
                btnDelete.Enabled = true;
                btnDelete.BackColor = Color.FromKnownColor(KnownColor.White);
            }
            else
            {
                btnDelete.Enabled = false;
                btnDelete.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
                dgvConsumption.Rows.Clear();


            }
        }
        private void AssignNumber()
        {
            int rowNumber = 1;
            foreach (DataGridViewRow row in dgvScanned.Rows)
            {
                row.HeaderCell.Style.BackColor = Color.FromKnownColor(KnownColor.GradientActiveCaption);
                row.HeaderCell.Style.Font = new Font("Khmer OS Battambong", 11, FontStyle.Regular);
                row.HeaderCell.Value = rowNumber.ToString();
                rowNumber = rowNumber + 1;
            }
        }
        private void CleardtSQLSaving()
        {
            dtSQLSaving = new DataTable();
            dtSQLSaving.Columns.Add("POSNo");
            dtSQLSaving.Columns.Add("RMCode");
            dtSQLSaving.Columns.Add("RMName");
            dtSQLSaving.Columns.Add("BOMQty");
            dtSQLSaving.Columns.Add("Qty");
            dtSQLSaving.Columns.Add("TransferQty");

        }
        private void ScanBarcode()
        {
            //Check in DB already have or not
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda3 = new SqlDataAdapter("SELECT COUNT(tbSDConn1Rec.SysNo) AS CountQty FROM tbSDConn1Rec " +
                                                                                    "INNER JOIN tbSDConn2Consump ON tbSDConn2Consump.SysNo = tbSDConn1Rec.SysNo " +
                                                                                    "WHERE NOT tbSDConn1Rec.Status = 0 AND tbSDConn1Rec.POSNo = '" + POSNo + "'", cnn.con);
                DataTable dt3 = new DataTable();
                sda3.Fill(dt3);
                if (dt3.Rows.Count > 0 && Convert.ToDouble(dt3.Rows[0][0].ToString()) > 0)
                {
                    MessageBox.Show("គ្មានភីអូអេសនេះធ្លាប់បានស្កេនម្ដងរួចហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtBarcode.Focus();
                    txtBarcode.SelectAll();
                }
                else
                {
                    //Check in dgvScanned Already have or not
                    int Found = 0;
                    foreach (DataGridViewRow row in this.dgvScanned.Rows)
                    {
                        if (POSNo == row.Cells[0].Value.ToString())
                        {
                            Found++;
                        }
                    }
                    if (Found == 0)
                    {
                        try
                        {
                            cnnOBS.conOBS.Open();
                            SqlDataAdapter sda = new SqlDataAdapter("SELECT DONo, prgproductionorder.ItemCode, mstitem.ItemName, PlanQty, POSDeliveryDate FROM prgproductionorder " +
                                                                                                "INNER JOIN mstitem ON mstitem.ItemCode = prgproductionorder.ItemCode " +
                                                                                                "WHERE DONo = '" + POSNo + "'", cnnOBS.conOBS);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                WIPCode = dt.Rows[0][1].ToString();
                                WIPName = dt.Rows[0][2].ToString();
                                Qty = dt.Rows[0][3].ToString();
                                ShipDate = Convert.ToDateTime(dt.Rows[0][4].ToString()).ToString("dd-MM-yyyy");
                                string ExceptRM = "";
                                for (int i = 0; i < CountRMExcept.Length; i++)
                                {
                                    if (ExceptRM.Trim() == "")
                                    {
                                        ExceptRM = "'" + CountRMExcept[i] + "'";
                                    }
                                    else
                                    {
                                        ExceptRM = ExceptRM + ", " + "'" + CountRMExcept[i] + "'";
                                    }
                                }
                                //Check BOM
                                //SqlDataAdapter sda1 = new SqlDataAdapter("SELECT prgconsumtionorder.ItemCode, mstitem.ItemName, BOMQty, ConsumpQty FROM prgconsumtionorder " +
                                //                                                                    "INNER JOIN prgproductionorder ON prgproductionorder.ProductionCode = prgconsumtionorder.ProductionCode " +
                                //                                                                    "INNER JOIN mstitem ON  mstitem.ItemCode = prgconsumtionorder.ItemCode " +
                                //                                                                    "WHERE LEN(prgproductionorder.ItemCode) > 4 AND mstitem.MatCalcFlag = 0 AND mstitem.ItemType = 2 AND NOT mstitem.ItemCode IN (" + ExceptRM + ")AND DONo = '" + POSNo + "' " +
                                //                                                                    "ORDER BY DONo ASC, ConsumpSeqNo ASC", cnnOBS.conOBS);
                                
                                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT prgconsumtionorder.ItemCode, mstitem.ItemName, BOMQty, ConsumpQty FROM prgconsumtionorder " +
                                                                                                    "INNER JOIN prgproductionorder ON prgproductionorder.ProductionCode = prgconsumtionorder.ProductionCode " +
                                                                                                    "INNER JOIN mstitem ON  mstitem.ItemCode = prgconsumtionorder.ItemCode " +
                                                                                                    "WHERE LEN(prgproductionorder.ItemCode) > 4 AND mstitem.MatCalcFlag = 0 AND mstitem.ItemType = 2 AND DONo = '" + POSNo + "' " +
                                                                                                    "ORDER BY DONo ASC, ConsumpSeqNo ASC", cnnOBS.conOBS);

                                dtPOSConsump = new DataTable();
                                sda1.Fill(dtPOSConsump);
                                if (dtPOSConsump.Rows.Count > 0)
                                {
                                    txtBarcode.Text = "";
                                    SDReceiveFormConfirm Sdrfc = new SDReceiveFormConfirm(this);
                                    Sdrfc.ShowDialog();
                                }
                                else
                                {
                                    MessageBox.Show("គ្មានខនិកទ័រដែលប្រើជាមួយភីអូអេសនេះទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    txtBarcode.Focus();
                                    txtBarcode.SelectAll();
                                }
                            }
                            else
                            {
                                MessageBox.Show("គ្មានភីអូអេសនេះនៅក្នុង System ទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                txtBarcode.Focus();
                                txtBarcode.SelectAll();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ការភ្ជាប់បណ្ដាញមានបញ្ហា!\nសូមពិនិត្យមើល Wifi/Internet!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtBarcode.Focus();
                            txtBarcode.SelectAll();
                        }
                        cnnOBS.conOBS.Close();
                    }
                    else
                    {
                        MessageBox.Show("ឡាប៊ែលនេះស្កេនមុននេះរួចហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.Focus();
                        txtBarcode.SelectAll();
                    }
                }
            }
            catch
            {


            }
            cnn.con.Close();

        }

    }
}
