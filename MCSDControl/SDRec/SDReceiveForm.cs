using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MachineDeptApp.MCSDControl.SDRec
{
    public partial class SDReceiveForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        public DataTable dtSQLSaving;

        string ErrorText = "", cnnIP = "";

        public SDReceiveForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnIP = cnn.server.ToString();
            this.cnnOBS.Connection();
            this.Load += SDReceiveForm_Load;
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;
            this.dgvScanned.CurrentCellChanged += DgvScanned_CurrentCellChanged;
            this.dgvScanned.RowsAdded += DgvScanned_RowsAdded;
            this.dgvScanned.RowsRemoved += DgvScanned_RowsRemoved;
            this.dgvConsumption.CellFormatting += DgvConsumption_CellFormatting;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnDelete.EnabledChanged += BtnDelete_EnabledChanged;
            this.btnSave.Click += BtnSave_Click;
            this.btnSave.EnabledChanged += BtnSave_EnabledChanged;
        }

        private void DgvScanned_CurrentCellChanged(object sender, EventArgs e)
        {
            dgvConsumption.Rows.Clear();
            if (dgvScanned.SelectedCells.Count > 0 && dgvScanned.CurrentCell != null && dgvScanned.CurrentCell.RowIndex >=0 && dgvScanned.CurrentCell.ColumnIndex >= 0)
            {
                foreach (DataRow row in dtSQLSaving.Rows)
                {
                    if (dgvScanned.Rows[dgvScanned.CurrentCell.RowIndex].Cells["POSNo"].Value.ToString() == row["POSNo"].ToString())
                    {
                        dgvConsumption.Rows.Add();
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].HeaderCell.Value = dgvConsumption.Rows.Count.ToString();
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["RMCode"].Value = row["ItemCode"];
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["RMName"].Value = row["ItemName"];
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["BOMQty"].Value = Convert.ToDouble(row["BOMQty"]);
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["ConsumptionQty"].Value = Convert.ToDouble(row["ConsumpQty"]);
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["KITTransferQty"].Value = Convert.ToDouble(row["KITTranQty"]);
                        if (row["RecQty"].ToString().Trim() != "")
                            dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["MCRecQty"].Value = Convert.ToDouble(row["RecQty"]);
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["MCRemQty"].Value = Convert.ToDouble(row["RemainMC_KITQty"]);
                    }
                }
            }
            CheckForEnableBtn();
            dgvConsumption.ClearSelection();
        }
        private void DgvScanned_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //Remove Consumption
            for (int j = dtSQLSaving.Rows.Count - 1; j >= 0; j--)
            {
                int Found = 0;
                foreach (DataGridViewRow row in dgvScanned.Rows)
                {
                    if (dtSQLSaving.Rows[j]["POSNo"].ToString() == row.Cells["POSNo"].Value.ToString())
                    {
                        Found++;
                        break;
                    }
                }

                if (Found == 0)
                {
                    dtSQLSaving.Rows[j].Delete();
                    dtSQLSaving.AcceptChanges();
                }
            }
            AssignNumber();
            CheckForEnableBtn();

            /*
            string ConsoleText = "";
            foreach (DataColumn col in dtSQLSaving.Columns)
                ConsoleText += col.ColumnName + "\t";
            Console.WriteLine(ConsoleText);
            foreach (DataRow row in dtSQLSaving.Rows)
            {
                ConsoleText = "";
                foreach (DataColumn col in dtSQLSaving.Columns)
                {
                    ConsoleText += row[col.ColumnName] + "\t";
                }
                Console.WriteLine(ConsoleText);
            }
            */
        }
        private void DgvScanned_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            AssignNumber();
            CheckForEnableBtn();
        }
        private void BtnSave_EnabledChanged(object sender, EventArgs e)
        {
            if (btnSave.Enabled == true)
                btnSaveGray.SendToBack();
            else
                btnSaveGray.BringToFront();
        }
        private void BtnDelete_EnabledChanged(object sender, EventArgs e)
        {
            if (btnDelete.Enabled == true)
                btnDeleteGray.SendToBack();
            else
                btnDeleteGray.BringToFront();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            DialogResult DLR = MessageBox.Show("តើអ្នករក្សាទុកទិន្នន័យនេះមែនទេ?", MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DLR == DialogResult.Yes)
            {
                Cursor = Cursors.WaitCursor;
                LbStatus.Text = "កំពុងរក្សាទុក . . . .";
                LbStatus.Visible = true;
                LbStatus.Refresh();

                ErrorText = "";
                string User = MenuFormV2.UserForNextForm;

                //Save to DB
                try
                {
                    cnn.con.Open();
                    foreach (DataGridViewRow rowDgv in dgvScanned.Rows)
                    {
                        string POSNo = rowDgv.Cells["POSNo"].Value.ToString();
                        string WIPCode = rowDgv.Cells["WIPCode"].Value.ToString();
                        string WIPName = rowDgv.Cells["WIPName"].Value.ToString();
                        double POSQty = Convert.ToDouble(rowDgv.Cells["POSQty"].Value.ToString());
                        DateTime POSDelDate = Convert.ToDateTime(rowDgv.Cells["ShipDate"].Value.ToString());
                        double Status = 1; 
                        if (Convert.ToBoolean(rowDgv.Cells["CompleteSet"].Value) == true)
                            Status = 2;
                        DateTime RegNow = DateTime.Now;
                        string TransNo = "REC0000000001";

                        //Find Last TransNo
                        string SQLQuery = "SELECT SysNo FROM tbSDMCAllTransaction " +
                                                    "\nWHERE SysNo=(SELECT MAX(SysNo) FROM tbSDMCAllTransaction WHERE Funct =1) Group By SysNo";
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        DataTable dtTransNo = new DataTable();
                        sda.Fill(dtTransNo);
                        if (dtTransNo.Rows.Count > 0)
                        {
                            string LastTransNo = dtTransNo.Rows[0][0].ToString();
                            double NextTransNo = Convert.ToDouble(LastTransNo.Substring(LastTransNo.Length - 10)) + 1;
                            TransNo = "REC" + NextTransNo.ToString("0000000000");
                        }

                        //Check already Received or Not
                        SQLQuery = "SELECT * FROM tbSDConn1Rec WHERE POSNo = '"+POSNo+"' ";
                        sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        DataTable dtChecking = new DataTable();
                        sda.Fill(dtChecking);
                        if ((dtChecking.Rows.Count == 0) || (dtChecking.Rows.Count > 0 && Convert.ToDouble(dtChecking.Rows[0]["Status"]) < 2)) 
                        {
                            //tbSDMCAllTransaction
                            foreach (DataRow rowDt in dtSQLSaving.Rows)
                            {
                                if (rowDt["POSNo"].ToString() == POSNo)
                                {
                                    if (Convert.ToDouble(rowDt["RemainMC_KITQty"]) > 0)
                                    {
                                        SqlCommand cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks ) " +
                                                                       "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs, @Rm)", cnn.con);
                                        cmd.Parameters.AddWithValue("@Sn", TransNo);
                                        cmd.Parameters.AddWithValue("@Ft", 1);
                                        cmd.Parameters.AddWithValue("@Lc", "KIT3");
                                        cmd.Parameters.AddWithValue("@Pn", POSNo);
                                        cmd.Parameters.AddWithValue("@Cd", rowDt["ItemCode"].ToString());
                                        cmd.Parameters.AddWithValue("@Rmd", rowDt["ItemName"].ToString());
                                        cmd.Parameters.AddWithValue("@Rq", Convert.ToDouble(rowDt["RemainMC_KITQty"]));
                                        cmd.Parameters.AddWithValue("@Tq", 0);
                                        cmd.Parameters.AddWithValue("@Sv", Convert.ToDouble(rowDt["RemainMC_KITQty"]));
                                        cmd.Parameters.AddWithValue("@Rd", RegNow);
                                        cmd.Parameters.AddWithValue("@Rb", User);
                                        cmd.Parameters.AddWithValue("@Cs", 0);
                                        cmd.Parameters.AddWithValue("@Rm", POSNo);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }

                            //tbSDConn1Rec & tbSDConn2Consump - Add/Insert
                            if (dtChecking.Rows.Count == 0)
                            {
                                //Add tbSDConn1Rec
                                SqlCommand cmd = new SqlCommand("INSERT INTO tbSDConn1Rec ( SysNo, POSNo, WIPCode, WIPName, PosQty, PosShipD, Status, RegDate, RegBy, UpdateDate, UpdateBy ) " +
                                                                       "\nVALUES (@Sn, @Pn, @Wc, @Wn, @Pq, @Ps, @St, @Rd, @Rb, @Ud, @Ub)", cnn.con);
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

                                //tbSDConn2Consump
                                int SeqNo = 1;
                                foreach (DataRow rowDt in dtSQLSaving.Rows)
                                {
                                    if (rowDt["POSNo"].ToString() == POSNo)
                                    {
                                        cmd = new SqlCommand("INSERT INTO tbSDConn2Consump ( SysNo, SeqNo, ItemCode, ItemName, BOMQty, ConsumpQty, RecQty, TransferQty ) " +
                                                                       "VALUES ( @Sn, @Sqn, @Ic, @In, @Bqty, @Cqty, @RQty, @TQty )", cnn.con);
                                        cmd.Parameters.AddWithValue("@Sn", TransNo);
                                        cmd.Parameters.AddWithValue("@Sqn", SeqNo);
                                        cmd.Parameters.AddWithValue("@Ic", rowDt["ItemCode"].ToString());
                                        cmd.Parameters.AddWithValue("@In", rowDt["ItemName"].ToString());
                                        cmd.Parameters.AddWithValue("@Bqty", Convert.ToDouble(rowDt["BOMQty"]));
                                        cmd.Parameters.AddWithValue("@Cqty", Convert.ToDouble(rowDt["ConsumpQty"]));
                                        cmd.Parameters.AddWithValue("@RQty", Convert.ToInt32(rowDt["RemainMC_KITQty"]));
                                        cmd.Parameters.AddWithValue("@TQty", 0);
                                        cmd.ExecuteNonQuery();
                                        SeqNo = SeqNo + 1;

                                    }
                                }
                            }
                            //tbSDConn1Rec & tbSDConn2Consump - Update
                            else
                            {
                                //tbSDConn2Consump
                                foreach (DataRow rowDt in dtSQLSaving.Rows)
                                {
                                    if (rowDt["POSNo"].ToString() == POSNo && Convert.ToDouble(rowDt["RemainMC_KITQty"]) > 0)
                                    {
                                        SqlCommand cmd = new SqlCommand("DECLARE @SysNo nvarchar(50) = (SELECT SysNo FROM tbSDConn1Rec WHERE POSNo = @POSNo) " +
                                            "\nDECLARE @CurrentQty int = (SELECT RecQty FROM tbSDConn2Consump WHERE SysNo = @SysNo AND ItemCode = @ItemCode) " +
                                            "\nUPDATE tbSDConn2Consump SET RecQty = @AddQty + @CurrentQty WHERE SysNo = @SysNo AND ItemCode = @ItemCode", cnn.con);
                                        cmd.Parameters.AddWithValue("@POSNo", POSNo);
                                        cmd.Parameters.AddWithValue("@ItemCode", rowDt["ItemCode"].ToString());
                                        cmd.Parameters.AddWithValue("@AddQty", Convert.ToInt32(rowDt["RemainMC_KITQty"]));
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                //tbSDConn1Rec
                                SQLQuery = "SELECT SysNo, POSNo, COUNT(ItemCode) AS ConsumptionCount, SUM(RecComplete) AS RecCompleteCount, SUM(TransferComplete) AS TransferCompleteCount FROM " +
                                    "\n(SELECT T1.SysNo, POSNo, T2.ItemCode, T2.ItemName, T2.ConsumpQty, T2.RecQty, T2.TransferQty, " +
                                    "\nCASE " +
                                    "\n\tWHEN T2.ConsumpQty = T2.RecQty THEN 1 " +
                                    "\n\tELSE 0 " +
                                    "\nEND AS RecComplete, " +
                                    "\nCASE " +
                                    "\n\tWHEN T2.ConsumpQty = T2.TransferQty THEN 1 " +
                                    "\n\tELSE 0 " +
                                    "\nEND AS TransferComplete FROM " +
                                    "\n(SELECT * FROM tbSDConn1Rec) T1 " +
                                    "\nINNER JOIN (SELECT * FROM tbSDConn2Consump) T2 ON T1.SysNo = T2.SysNo ) T1 " +
                                    "\nWHERE POSNo = '"+POSNo+"' " +
                                    "\nGROUP BY SysNo, POSNo " +
                                    "\nORDER BY POSNo ASC";
                                sda = new SqlDataAdapter(SQLQuery, cnn.con);
                                DataTable dt = new DataTable();
                                sda.Fill(dt);

                                if (dt.Rows.Count > 0)
                                {
                                    Status = 1;
                                    if (Convert.ToDouble(dt.Rows[0]["ConsumptionCount"]) == Convert.ToDouble(dt.Rows[0]["RecCompleteCount"]))
                                        Status = 2;
                                    //Add tbSDConn1Rec
                                    SqlCommand cmd = new SqlCommand("UPDATE tbSDConn1Rec SET Status = @St, UpdateDate = @Ud, UpdateBy = @Ub " +
                                                                                            "\nWHERE POSNo = @Pn ", cnn.con);
                                    cmd.Parameters.AddWithValue("@Pn", POSNo);
                                    cmd.Parameters.AddWithValue("@St", Status);
                                    cmd.Parameters.AddWithValue("@Ud", RegNow);
                                    cmd.Parameters.AddWithValue("@Ub", User);
                                    cmd.ExecuteNonQuery();

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorText = "Save to DB : " + ex.Message;
                }
                cnn.con.Close();

                Cursor = Cursors.Default;

                if (ErrorText.Trim() == "")
                {
                    LbStatus.Text = "រក្សាទុករួចរាល់";
                    MessageBox.Show("រក្សាទុករួចរាល់!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LbStatus.Text = "";
                    dgvScanned.Rows.Clear();
                    dgvConsumption.Rows.Clear();
                    ClearDtSQLSaving();
                    CheckForEnableBtn();
                }
                else
                {
                    LbStatus.Text = "រក្សាទុកមានបញ្ហា!";
                    LbStatus.Refresh();
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
                        fs.Write(Encoding.UTF8.GetBytes(ErrorText), 0, ErrorText.Length);
                    }
                    MessageBox.Show("រក្សាទុកមានបញ្ហា! File txt លម្អិតអំពី Error : \n" + ErrorPath, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult DLS = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនដែរឬទេ?", MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (DLS == DialogResult.Yes)
            {
                //Remove dgvScanned
                dgvScanned.Rows.RemoveAt(dgvScanned.CurrentCell.RowIndex);
                dgvScanned.Refresh();
                dgvScanned.ClearSelection();
                dgvScanned.CurrentCell = null;
                CheckForEnableBtn();
            }
        }
        private void SDReceiveForm_Load(object sender, EventArgs e)
        {
            this.dgvScanned.RowHeadersDefaultCellStyle.Font = new Font("Calibri", 10, FontStyle.Regular);
            this.dgvConsumption.RowHeadersDefaultCellStyle.Font = new Font("Calibri", 10, FontStyle.Regular);
            foreach (DataGridViewColumn col in dgvScanned.Columns)
                //Console.WriteLine(col.Name);
            ClearDtSQLSaving();
        }
        private void DgvConsumption_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvConsumption.Columns[e.ColumnIndex].Name == "KITTransferQty")
                {
                    double ConsumptionQty = Convert.ToDouble(dgvConsumption.Rows[e.RowIndex].Cells["ConsumptionQty"].Value);
                    double KITTransferQty = Convert.ToDouble(dgvConsumption.Rows[e.RowIndex].Cells["KITTransferQty"].Value);
                    if (KITTransferQty >= ConsumptionQty)
                    {
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.Font = new Font(dgvConsumption.AlternatingRowsDefaultCellStyle.Font, FontStyle.Bold);
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.Red;
                        e.CellStyle.Font = new Font(dgvConsumption.AlternatingRowsDefaultCellStyle.Font, FontStyle.Bold);
                    }
                }

                if (dgvConsumption.Columns[e.ColumnIndex].Name == "MCRemQty")
                {
                    double ConsumptionQty = Convert.ToDouble(dgvConsumption.Rows[e.RowIndex].Cells["ConsumptionQty"].Value);
                    double MCRemainQty = Convert.ToDouble(dgvConsumption.Rows[e.RowIndex].Cells["MCRemQty"].Value);
                    if (MCRemainQty > 0)
                    {
                        if (MCRemainQty == ConsumptionQty)
                        {
                            e.CellStyle.ForeColor = Color.Green;
                            e.CellStyle.Font = new Font(dgvConsumption.AlternatingRowsDefaultCellStyle.Font, FontStyle.Bold);
                        }
                        else
                        {
                            e.CellStyle.ForeColor = Color.Orange;
                            e.CellStyle.Font = new Font(dgvConsumption.AlternatingRowsDefaultCellStyle.Font, FontStyle.Bold);
                        }
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.Font = new Font(dgvConsumption.AlternatingRowsDefaultCellStyle.Font, FontStyle.Bold);
                    }
                }

            }
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
                        ScanBarcode(txtBarcode.Text);
                        //Focus/Scroll to the Last-Row
                        if (dgvScanned.Rows.Count > 0)
                            this.dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells[0].Selected = true;
                        dgvScanned.ClearSelection();
                        dgvScanned.CurrentCell = null;
                        CheckForEnableBtn();

                        /*

                        string ConsoleText = "";
                        foreach (DataColumn col in dtSQLSaving.Columns)
                            ConsoleText += col.ColumnName + "\t";
                        Console.WriteLine(ConsoleText);
                        foreach (DataRow row in dtSQLSaving.Rows)
                        {
                            ConsoleText = "";
                            foreach (DataColumn col in dtSQLSaving.Columns)
                            {
                                ConsoleText += row[col.ColumnName] + "\t";
                            }
                            Console.WriteLine(ConsoleText);
                        }

                        */
                             
                    }
                }
            }
        }

        //Method
        private void CheckForEnableBtn()
        {
            //btnSave
            if (dgvScanned.Rows.Count > 0)
                btnSave.Enabled = true;
            else
                btnSave.Enabled = false;

            //btnDelete
            if (dgvScanned.SelectedCells.Count > 0 && dgvScanned.CurrentCell != null)
                btnDelete.Enabled = true;
            else
                btnDelete.Enabled = false;
        }
        private void AssignNumber()
        {
            foreach (DataGridViewRow row in dgvScanned.Rows)
                row.HeaderCell.Value = (row.Index + 1).ToString();
        }
        private void ClearDtSQLSaving()
        {
            dtSQLSaving = new DataTable();
            dtSQLSaving.Columns.Add("POSNo");
            dtSQLSaving.Columns.Add("ItemCode");
            dtSQLSaving.Columns.Add("ItemName");
            dtSQLSaving.Columns.Add("BOMQty");
            dtSQLSaving.Columns.Add("ConsumpQty");
            dtSQLSaving.Columns.Add("KITTranQty");
            dtSQLSaving.Columns.Add("RecQty");
            dtSQLSaving.Columns.Add("RemainMC_KITQty");
            dtSQLSaving.PrimaryKey = new DataColumn[] {
                                dtSQLSaving.Columns["POSNo"],
                                dtSQLSaving.Columns["ItemCode"]
                            };

        }
        private void ScanBarcode(string POSNo)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;

            //Taking Data
            DataTable dtPOSDetail = new DataTable();
            DataTable dtDetails = new DataTable();
            try
            {
                cnnOBS.conOBS.Open();
                string SQLQuery = "SELECT tbOBS.*, MC_KITStatus, tbKITDetails.RecQty, (COALESCE(KITTranQty,0)-COALESCE(RecQty,0)) AS RemainMC_KITQty FROM " +
                    "\n--OBS Data" +
                    "\n(" +
                    "\n\tSELECT DONo, ConsumpSeqNo, prgconsumtionorder.ItemCode, ItemName, BOMQty, ConsumpQty, KITTranQty FROM prgconsumtionorder " +
                    "\n\tINNER JOIN prgproductionorder ON prgconsumtionorder.ProductionCode = prgproductionorder.ProductionCode " +
                    "\n\tINNER JOIN (SELECT * FROM mstitem WHERE DelFlag = 0 AND ItemType=2 AND MatCalcFlag = 0) tbItems ON prgconsumtionorder.ItemCode = tbItems.ItemCode " +
                    "\n\tLEFT JOIN (SELECT Remark, ItemCode, SUM(IssueQty) AS KITTranQty FROM prgalltransaction WHERE TypeCode = 1 AND LocCode = 'KIT2' AND GRICode = 60 " +
                    "\n\tGROUP BY Remark, ItemCode) tbTransaction ON DONo = tbTransaction.Remark AND prgconsumtionorder.ItemCode = tbTransaction.ItemCode " +
                    "\n) tbOBS " +
                    "\nLEFT JOIN " +
                    "\n--MC KIT" +
                    "\n(" +
                    "\n\tSELECT tbSDConn1Rec.POSNo, Status AS MC_KITStatus, tbSDConn2Consump.* FROM ["+cnnIP+"].MachineDB.dbo.tbSDConn1Rec " +
                    "\n\tINNER JOIN ["+cnnIP+"].MachineDB.dbo.tbSDConn2Consump " +
                    "\n\tON tbSDConn1Rec.SysNo = tbSDConn2Consump.SysNo " +
                    "\n) tbKITDetails " +
                    "\nON tbOBS.DONo = tbKITDetails.POSNo AND tbOBS.ItemCode = tbKITDetails.ItemCode " +
                    "\n\nWHERE DONo = '"+ POSNo + "' " +
                    "\n\nORDER BY DONo ASC, ConsumpSeqNo ASC";
                //Console.WriteLine("\n\n" + SQLQuery);
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                sda.Fill(dtDetails);

                SQLQuery = "SELECT DONO, prgproductionorder.ItemCode, ItemName, POSDeliveryDate, PlanQty FROM prgproductionorder " +
                    "\nINNER JOIN (SELECT * FROM mstitem WHERE DelFlag = 0 AND ItemType IN (0,1)) T2 " +
                    "\nON prgproductionorder.ItemCode = T2.ItemCode WHERE DONo = '"+POSNo+"' ";

                //Console.WriteLine("\n\n" + SQLQuery);
                sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                sda.Fill(dtPOSDetail);

            }
            catch (Exception ex)
            {
                ErrorText = "Taking Data : " + ex.Message;
            }
            cnnOBS.conOBS.Close();

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                if (dtPOSDetail.Rows.Count > 0)
                {
                    if (dtPOSDetail.Rows.Count > 0 && dtDetails.Rows.Count > 0)
                    {
                        if (dtDetails.Rows[0]["MC_KITStatus"].ToString().Trim() == "" || Convert.ToDouble(dtDetails.Rows[0]["MC_KITStatus"]) < 2)
                        {
                            bool FoundScanned = false;
                            foreach (DataGridViewRow row in dgvScanned.Rows)
                            {
                                if (row.Cells["POSNo"].Value.ToString() == POSNo)
                                {
                                    FoundScanned = true;
                                    break;
                                }
                            }
                            if (FoundScanned == false)
                            {
                                int FoundKITNotYet = 0;
                                int FoundMCRemain = 0;
                                foreach (DataRow row in dtDetails.Rows)
                                {
                                    if (row["KITTranQty"].ToString().Trim() != "")
                                    {
                                        if (Convert.ToDouble(row["RemainMC_KITQty"].ToString()) > 0)
                                            FoundMCRemain++;
                                    }
                                    else
                                        FoundKITNotYet++;
                                }

                                if (FoundKITNotYet < dtDetails.Rows.Count)
                                {
                                    if (FoundMCRemain > 0)
                                    {
                                        txtBarcode.Text = "";
                                        SDReceiveFormConfirm Sdrfc = new SDReceiveFormConfirm(this.dtSQLSaving, this.dgvScanned, dtPOSDetail, dtDetails);
                                        Sdrfc.ShowDialog();
                                    }
                                    else
                                    {
                                        MessageBox.Show("អ្នកបានទទួលរួចហើយ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        txtBarcode.Focus();
                                        txtBarcode.SelectAll();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("PP-KIT មិនទាន់វេរមកនុះទេ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    txtBarcode.Focus();
                                    txtBarcode.SelectAll();
                                }
                            }
                            else
                            {
                                MessageBox.Show("ឡាប៊ែលនេះស្កេនមុននេះរួចហើយ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                txtBarcode.Focus();
                                txtBarcode.SelectAll();
                            }
                        }
                        else
                        {
                            MessageBox.Show("ភីអូអេសនេះធ្លាប់បានស្កេនម្ដងរួចហើយ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txtBarcode.Focus();
                            txtBarcode.SelectAll();
                        }
                    }
                    else
                    {
                        MessageBox.Show("គ្មានខនិកទ័រដែលប្រើជាមួយភីអូអេសនេះទេ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtBarcode.Focus();
                        txtBarcode.SelectAll();
                    }
                }
                else
                {
                    MessageBox.Show("គ្មានភីអូអេសនេះនៅក្នុង System ទេ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtBarcode.Focus();
                    txtBarcode.SelectAll();
                }
            }
            else
            {
                MessageBox.Show("ការភ្ជាប់បណ្ដាញមានបញ្ហា!\nសូមពិនិត្យមើល Wifi/Internet!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBarcode.Focus();
                txtBarcode.SelectAll();
            }
        }
    }
}
