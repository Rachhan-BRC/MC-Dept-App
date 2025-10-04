using MachineDeptApp.MsgClass;
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
    public partial class SDTransferForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        ErrorReportAsTxt EOutput = new ErrorReportAsTxt();
        DataTable dtConsumtionSave;

        string ErrorText;

        public SDTransferForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Load += SDTransferForm_Load;
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;

            //Dgv
            this.dgvScanned.RowsAdded += DgvScanned_RowsAdded;
            this.dgvScanned.RowsRemoved += DgvScanned_RowsRemoved;
            this.dgvScanned.CurrentCellChanged += DgvScanned_CurrentCellChanged;
            this.dgvScanned.CellPainting += DgvScanned_CellPainting;
            this.dgvConsumption.CellPainting += DgvConsumption_CellPainting;

            //Btn
            this.btnNew.Click += BtnNew_Click;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnDelete.EnabledChanged += BtnDelete_EnabledChanged;
            this.btnSave.Click += BtnSave_Click;
            this.btnSave.EnabledChanged += BtnSave_EnabledChanged;

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DialogResult DSL = MessageBox.Show("តើអ្នកចង់រក្សាទុកមែនឬទេ?", MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DSL == DialogResult.Yes)
            {
                Cursor = Cursors.WaitCursor;
                DataTable dtErrorList = new DataTable();
                dtErrorList.Columns.Add("POSNo");
                dtErrorList.Columns.Add("Details");
                dtErrorList.AcceptChanges();

                string User = MenuFormV2.UserForNextForm;
                //Save to DB
                foreach (DataGridViewRow row in dgvScanned.Rows)
                {
                    ErrorText = "";
                    string POSNo = row.Cells["POSNo"].Value.ToString();
                    double TransferSet = Convert.ToDouble(row.Cells["POSTransferQty"].Value);
                    DateTime RegNow = DateTime.Now;
                    string TransNo = "TRF0000000001";

                    //Tacking SysNo
                    DataTable dtStockCompare = new DataTable();
                    try
                    {
                        cnn.con.Open();
                        string SQLQuery = "SELECT SysNo FROM tbSDMCAllTransaction " +
                                                "\nWHERE SysNo=(SELECT MAX(SysNo) FROM tbSDMCAllTransaction WHERE Funct =2) Group By SysNo";
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        DataTable dtTransNo = new DataTable();
                        sda.Fill(dtTransNo);
                        if (dtTransNo.Rows.Count > 0)
                        {
                            string LastTransNo = dtTransNo.Rows[0][0].ToString();
                            double NextTransNo = Convert.ToDouble(LastTransNo.Substring(LastTransNo.Length - 10)) + 1;
                            TransNo = "TRF" + NextTransNo.ToString("0000000000");

                        }

                        SQLQuery = "DECLARE @POSNo nvarchar(25) = '" + POSNo + "' " +
                            "\nDECLARE @SETQty int = " + TransferSet.ToString() + " " +
                            "\nSELECT T1.SysNo, T2.POSNo, Status, T1.ItemCode, BOMQty, (BOMQty * @SETQty) AS TTLCutQty, RemainStockQty, (RemainStockQty-(BOMQty * @SETQty)) AS ShortageQty FROM " +
                            "\n(SELECT * FROM tbSDConn2Consump) T1 " +
                            "\nINNER JOIN (SELECT * FROM tbSDConn1Rec) T2 ON T1.SysNo = T2.SysNo " +
                            "\nLEFT JOIN (SELECT POSNo, Code, SUM(StockValue) AS RemainStockQty FROM tbSDMCAllTransaction " +
                            "\n\t\t\t\tWHERE CancelStatus = 0 AND LocCode = 'KIT3' GROUP BY POSNo, Code) T3 " +
                            "\n\t\t\t\tON T2.POSNo = T3.POSNo AND T1.ItemCode = T3.Code " +
                            "\nWHERE T2.POSNo = @POSNo " +
                            "\nORDER BY T1.SeqNo ASC ";
                        sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        sda.Fill(dtStockCompare);
                    }
                    catch (Exception ex)
                    {
                        ErrorText = "Tacking SysNo : " + ex.Message;
                    }
                    cnn.con.Close();


                    if (ErrorText.Trim() == "")
                    {
                        if (dtStockCompare.Rows[0]["Status"].ToString() == "1" || dtStockCompare.Rows[0]["Status"].ToString() == "2")
                        {
                            string SysNo = dtStockCompare.Rows[0]["SysNo"].ToString();
                            int FoundShortage = 0;
                            foreach (DataRow rowCheck in dtStockCompare.Rows)
                            {
                                if (Convert.ToDouble(rowCheck["ShortageQty"]) < 0)
                                {
                                    FoundShortage++;
                                    break;
                                }
                            }

                            if (FoundShortage == 0)
                            {
                                //Final Save
                                try
                                {
                                    cnn.con.Open();
                                    //Add to AllTransaction
                                    foreach (DataRow rowSave in dtConsumtionSave.Rows)
                                    {
                                        if (POSNo == rowSave["POSNo"].ToString())
                                        {
                                            string Code = rowSave["ItemCode"].ToString();
                                            string RMName = rowSave["ItemName"].ToString();
                                            double TransQty = Convert.ToDouble(rowSave["ActualTransQty"].ToString());

                                            if (TransQty > 0)
                                            {
                                                //Add to AllTransaction Stock-Out
                                                SqlCommand cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                                                               "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs, @Rm)", cnn.con);
                                                cmd.Parameters.AddWithValue("@Sn", TransNo);
                                                cmd.Parameters.AddWithValue("@Ft", 2);
                                                cmd.Parameters.AddWithValue("@Lc", "KIT3");
                                                cmd.Parameters.AddWithValue("@Pn", POSNo);
                                                cmd.Parameters.AddWithValue("@Cd", Code);
                                                cmd.Parameters.AddWithValue("@Rmd", RMName);
                                                cmd.Parameters.AddWithValue("@Rq", 0);
                                                cmd.Parameters.AddWithValue("@Tq", TransQty);
                                                cmd.Parameters.AddWithValue("@Sv", (TransQty * (-1)));
                                                cmd.Parameters.AddWithValue("@Rd", RegNow);
                                                cmd.Parameters.AddWithValue("@Rb", User);
                                                cmd.Parameters.AddWithValue("@Cs", 0);
                                                cmd.Parameters.AddWithValue("@Rm", POSNo);
                                                cmd.ExecuteNonQuery();

                                                //Add to AllTransaction Stock-In
                                                cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                                                               "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs, @Rm)", cnn.con);
                                                cmd.Parameters.AddWithValue("@Sn", TransNo);
                                                cmd.Parameters.AddWithValue("@Ft", 2);
                                                cmd.Parameters.AddWithValue("@Lc", "MC1");
                                                cmd.Parameters.AddWithValue("@Pn", POSNo);
                                                cmd.Parameters.AddWithValue("@Cd", Code);
                                                cmd.Parameters.AddWithValue("@Rmd", RMName);
                                                cmd.Parameters.AddWithValue("@Rq", TransQty);
                                                cmd.Parameters.AddWithValue("@Tq", 0);
                                                cmd.Parameters.AddWithValue("@Sv", TransQty);
                                                cmd.Parameters.AddWithValue("@Rd", RegNow.AddMilliseconds(25));
                                                cmd.Parameters.AddWithValue("@Rb", User);
                                                cmd.Parameters.AddWithValue("@Cs", 0);
                                                cmd.Parameters.AddWithValue("@Rm", POSNo);
                                                cmd.ExecuteNonQuery();

                                                //Update tbSDConn2Consump
                                                string SQLQuery = "DECLARE @CurrentTranQty int = (SELECT TransferQty FROM tbSDConn2Consump WHERE SysNo = @SysNo AND ItemCode = @ItemCode) " +
                                                    "\nUPDATE tbSDConn2Consump SET TransferQty = @CurrentTranQty + @NewTranQty WHERE SysNo = @SysNo AND ItemCode = @ItemCode ";
                                                cmd = new SqlCommand(SQLQuery, cnn.con);
                                                cmd.Parameters.AddWithValue("@SysNo", SysNo);
                                                cmd.Parameters.AddWithValue("@ItemCode", Code);
                                                cmd.Parameters.AddWithValue("@NewTranQty", Convert.ToInt32(TransQty.ToString("N0").Replace(",", "")));
                                                cmd.ExecuteNonQuery();
                                            }

                                        }
                                    }

                                    //tbSDConn1Rec
                                    string SQLQuery2 = "SELECT SysNo, POSNo, COUNT(ItemCode) AS ConsumptionCount, SUM(RecComplete) AS RecCompleteCount, SUM(TransferComplete) AS TransferCompleteCount FROM " +
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
                                        "\nWHERE POSNo = '" + POSNo + "' " +
                                        "\nGROUP BY SysNo, POSNo " +
                                        "\nORDER BY POSNo ASC";
                                    SqlDataAdapter sda2 = new SqlDataAdapter(SQLQuery2, cnn.con);
                                    DataTable dt = new DataTable();
                                    sda2.Fill(dt);

                                    if (dt.Rows.Count > 0)
                                    {
                                        int Status = Convert.ToInt16(dtStockCompare.Rows[0]["Status"]);
                                        if (Convert.ToDouble(dt.Rows[0]["ConsumptionCount"]) == Convert.ToDouble(dt.Rows[0]["TransferCompleteCount"]))
                                            Status = 3;
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
                                catch (Exception ex)
                                {
                                    dtErrorList.Rows.Add(POSNo, ex.Message);
                                    dtErrorList.AcceptChanges();
                                }
                                cnn.con.Close();
                            }
                            else
                            {
                                dtErrorList.Rows.Add(POSNo, "Shortage RM Stock!");
                                dtErrorList.AcceptChanges();
                            }
                        }
                        else
                        {
                            dtErrorList.Rows.Add(POSNo, "Already Transfered by other user!");
                            dtErrorList.AcceptChanges();
                        }
                    }
                    else
                    {
                        dtErrorList.Rows.Add(POSNo, ErrorText);
                        dtErrorList.AcceptChanges();
                    }
                }

                Cursor = Cursors.Default;

                if (dtErrorList.Rows.Count == 0)
                {
                    ClearDtConsumtionSave();
                    LbStatus.Text = "រក្សាទុករួចរាល់";
                    MessageBox.Show("រក្សាទុករួចរាល់!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LbStatus.Text = "";
                    dgvScanned.Rows.Clear();
                    dgvScanned.CurrentCell = null;
                    dgvConsumption.Rows.Clear();
                }
                else
                {
                    ErrorText = "POSNo\t\tErrorText";
                    foreach (DataRow row in dtErrorList.Rows)
                    {
                        ErrorText += "\n" + row["POSNo"].ToString() + "\t" + row["Details"].ToString();
                    }
                    ErrorReportAsTxt.PrintError = ErrorText;
                    this.EOutput.Output();
                    if (dgvScanned.Rows.Count > dtErrorList.Rows.Count)
                    {
                        LbStatus.Text = "រក្សាទុករួចរាល់ ប៉ុន្តែមានបញ្ហាខ្លះ!";
                        MessageBox.Show("រក្សាទុករួចរាល់ ប៉ុន្តែមានបញ្ហាខ្លះ!\nFile txt លម្អិតអំពី Error : \n" + ErrorReportAsTxt.ErrorPath, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        LbStatus.Text = "រក្សាទុកមានបញ្ហា!";
                        MessageBox.Show("រក្សាទុកមានបញ្ហា! File txt លម្អិតអំពី Error : \n" + ErrorReportAsTxt.ErrorPath, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Cursor = Cursors.WaitCursor;
                    for (int i = dgvScanned.Rows.Count - 1; i >= 0; i--)
                    {
                        bool Found = false;
                        foreach (DataRow row in dtErrorList.Rows)
                        {
                            if (dgvScanned.Rows[i].Cells["POSNo"].Value.ToString() == row["POSNo"].ToString())
                            {
                                Found = true;
                                break;
                            }
                        }
                        if (Found == false)
                        {
                            dgvScanned.Rows.RemoveAt(i);
                        }
                    }
                    dgvScanned.Refresh();
                    dgvScanned.ClearSelection();
                    dgvScanned.CurrentCell = null;
                    Cursor = Cursors.Default;
                }
            }
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            dgvScanned.Rows.Clear();
            dgvConsumption.Rows.Clear();
            ClearDtConsumtionSave();
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }
        private void DgvScanned_CurrentCellChanged(object sender, EventArgs e)
        {
            dgvConsumption.Rows.Clear();
            if (dgvScanned.SelectedCells.Count > 0 && dgvScanned.CurrentCell != null && dgvScanned.CurrentCell.RowIndex >= 0 && dgvScanned.CurrentCell.ColumnIndex >= 0)
            {
                string POSNo = dgvScanned.Rows[dgvScanned.CurrentCell.RowIndex].Cells["POSNo"].Value.ToString();
                foreach (DataRow row in dtConsumtionSave.Rows)
                {
                    if (POSNo == row["POSNo"].ToString())
                    {
                        dgvConsumption.Rows.Add();
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].HeaderCell.Value = dgvConsumption.Rows.Count.ToString();
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["RMCode"].Value = row["ItemCode"];
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["RMName"].Value = row["ItemName"];
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["BOMQty"].Value = Convert.ToDouble(row["BOMQty"]);
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["TotalQty"].Value = Convert.ToDouble(row["ConsumpQty"]);
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["RecQty"].Value = Convert.ToDouble(row["RecQty"]);
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["TransferedQty"].Value = Convert.ToDouble(row["TransferQty"]);
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["RemainQty"].Value = Convert.ToDouble(row["RemainQty"]);
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["StockRemain"].Value = Convert.ToDouble(row["RemainStockQty"]);
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["ActualTransferQty"].Value = Convert.ToDouble(row["ActualTransQty"]);
                    }
                }
            }
            CheckForEnableBtn();
            dgvConsumption.ClearSelection();
        }
        private void DgvScanned_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //Remove Consumption
            for (int j = dtConsumtionSave.Rows.Count - 1; j >= 0; j--)
            {
                int Found = 0;
                foreach (DataGridViewRow row in dgvScanned.Rows)
                {
                    if (dtConsumtionSave.Rows[j]["POSNo"].ToString() == row.Cells["POSNo"].Value.ToString())
                    {
                        Found++;
                        break;
                    }
                }

                if (Found == 0)
                {
                    dtConsumtionSave.Rows[j].Delete();
                    dtConsumtionSave.AcceptChanges();
                }
            }

            //Assign SeqNo
            foreach (DataGridViewRow row in dgvScanned.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString();
            }

            CheckForEnableBtn();

            /*

            string ConsoleText = "";
            foreach (DataColumn col in dtConsumtionSave.Columns)
                ConsoleText += col.ColumnName + "\t";
            Console.WriteLine(ConsoleText);
            foreach (DataRow row in dtConsumtionSave.Rows)
            {
                ConsoleText = "";
                foreach (DataColumn col in dtConsumtionSave.Columns)
                {
                    ConsoleText += row[col.ColumnName] + "\t";
                }
                Console.WriteLine(ConsoleText);
            }

            */

        }
        private void DgvScanned_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //Assign SeqNo
            foreach (DataGridViewRow row in dgvScanned.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString();
            }
            CheckForEnableBtn();
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvScanned.SelectedCells.Count > 0 && dgvScanned.CurrentCell != null && dgvScanned.CurrentCell.RowIndex >= 0 && dgvScanned.CurrentCell.ColumnIndex >= 0)
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនដែរឬទេ?", MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    dgvScanned.Rows.RemoveAt(dgvScanned.CurrentCell.RowIndex);
                    dgvScanned.Refresh();
                    dgvScanned.ClearSelection();
                    dgvScanned.CurrentCell = null;
                    CheckForEnableBtn();
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
                        MessageBox.Show("បាកូដខុសទម្រង់ហើយ !", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtBarcode.Focus();
                        txtBarcode.SelectAll();
                    }
                    else
                    {
                        ScanBarcode(txtBarcode.Text);
                    }
                }
            }
        }
        private void DgvConsumption_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvConsumption.Columns[e.ColumnIndex].Name == "ActualTransferQty")
                {
                    if (Convert.ToDouble(dgvConsumption.Rows[e.RowIndex].Cells["TotalQty"].Value) == Convert.ToDouble(e.Value))
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
            }
        }
        private void DgvScanned_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvScanned.Columns[e.ColumnIndex].Name == "POSTransferQty")
                {
                    if (Convert.ToDouble(dgvScanned.Rows[e.RowIndex].Cells["POSQty"].Value) == Convert.ToDouble(e.Value))
                    {
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.Font = new Font(dgvScanned.AlternatingRowsDefaultCellStyle.Font, FontStyle.Bold);
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.Orange;
                        e.CellStyle.Font = new Font(dgvScanned.AlternatingRowsDefaultCellStyle.Font, FontStyle.Bold);
                    }
                }
            }
        }
        private void SDTransferForm_Load(object sender, EventArgs e)
        {
            dgvScanned.RowHeadersDefaultCellStyle.Font = new Font("Calibri", 10, FontStyle.Regular);
            dgvConsumption.RowHeadersDefaultCellStyle.Font = new Font("Calibri", 10, FontStyle.Regular);
            Color colColor = Color.Orange;
            foreach (DataGridViewColumn col in dgvScanned.Columns)
            {
                if (col.Name == "POSTransferQty")
                {
                    dgvScanned.Columns[col.Name].HeaderCell.Style.BackColor = colColor;
                    dgvScanned.Columns[col.Name].HeaderCell.Style.SelectionBackColor = colColor;
                }
                //Console.WriteLine(col.Name);
            }
            foreach (DataGridViewColumn col in dgvConsumption.Columns)
            {
                if (col.Name == "ActualTransferQty")
                {
                    dgvConsumption.Columns[col.Name].HeaderCell.Style.BackColor = colColor;
                    dgvConsumption.Columns[col.Name].HeaderCell.Style.SelectionBackColor = colColor;
                }
                Console.WriteLine(col.Name);
            }
            ClearDtConsumtionSave();
        }
        private void BtnSave_EnabledChanged(object sender, EventArgs e)
        {
            if (btnSave.Enabled == true)
                btnSaveGRAY.SendToBack();
            else
                btnSaveGRAY.BringToFront();
        }
        private void BtnDelete_EnabledChanged(object sender, EventArgs e)
        {
            if (btnDelete.Enabled == true)
                btnDeleteGRAY.SendToBack();
            else
                btnDeleteGRAY.BringToFront();
        }


        //Method
        private void ClearDtConsumtionSave()
        {
            dtConsumtionSave = new DataTable();
            dtConsumtionSave.Columns.Add("POSNo");
            dtConsumtionSave.Columns.Add("ItemCode");
            dtConsumtionSave.Columns.Add("ItemName");
            dtConsumtionSave.Columns.Add("BOMQty");
            dtConsumtionSave.Columns.Add("ConsumpQty");
            dtConsumtionSave.Columns.Add("RecQty");
            dtConsumtionSave.Columns.Add("TransferQty");
            dtConsumtionSave.Columns.Add("RemainQty");
            dtConsumtionSave.Columns.Add("RemainStockQty");
            dtConsumtionSave.Columns.Add("ActualTransQty");
            dtConsumtionSave.PrimaryKey = new DataColumn[] {
                                dtConsumtionSave.Columns["POSNo"],
                                dtConsumtionSave.Columns["ItemCode"]
                            };
        }
        private void ScanBarcode(string POSNo)
        {
            Cursor = Cursors.WaitCursor;
            ErrorText = "";

            //Taking Data
            DataTable dtPOSDetails = new DataTable();
            DataTable dtRMDetails = new DataTable();
            try
            {
                cnn.con.Open();

                string SQLQuery = "SELECT tbSDConn1Rec.POSNo, WIPCode, WIPName, PosQty, PosShipD, " +
                    "\nCASE " +
                    "\n\tWHEN Status = 1 THEN N'ខ្វះ' " +
                    "\n\tWHEN Status = 2 THEN N'គ្រប់' " +
                    "\n\tWHEN Status = 3 THEN N'វេររួច' " +
                    "\n\tELSE N'Cancel' " +
                    "\nEND AS StatusText, MIN(((RecQty-TransferQty)/BOMQty)) AS MinRemainSet, MIN((RemainStockQty/BOMQty)) AS MinRemainStockSet FROM tbSDConn1Rec " +
                    "\nINNER JOIN (SELECT * FROM tbSDConn2Consump) T2 ON tbSDConn1Rec.SysNo = T2.SysNo " +
                    "\nLEFT JOIN (SELECT POSNo, Code, SUM(StockValue) AS RemainStockQty FROM tbSDMCAllTransaction " +
                    "\nWHERE CancelStatus = 0 AND LocCode = 'KIT3' GROUP BY POSNo, Code) T3 " +
                    "\nON tbSDConn1Rec.POSNo = T3.POSNo AND T2.ItemCode = T3.Code  WHERE tbSDConn1Rec.POSNo = '" + POSNo+ "' " +
                    "\nGROUP BY tbSDConn1Rec.POSNo, WIPCode, Status, WIPName, PosQty, PosShipD ";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtPOSDetails);

                SQLQuery = "SELECT T1.POSNo, ItemCode, ItemName, BOMQty, T2.ConsumpQty, RecQty, TransferQty, " +
                    "\nRecQty-TransferQty AS RemainQty, RemainStockQty FROM " +
                    "\n(SELECT * FROM tbSDConn1Rec) T1 " +
                    "\nINNER JOIN (SELECT * FROM tbSDConn2Consump) T2 ON T1.SysNo = T2.SysNo " +
                    "\nLEFT JOIN (SELECT POSNo, Code, SUM(StockValue) AS RemainStockQty FROM tbSDMCAllTransaction " +
                    "\nWHERE CancelStatus = 0 AND LocCode = 'KIT3' GROUP BY POSNo, Code) T3 " +
                    "\nON T1.POSNo = T3.POSNo AND T2.ItemCode = T3.Code " +
                    "\nWHERE T1.POSNo = '"+POSNo+"' " +
                    "\nGROUP BY T1.POSNo, T2.SeqNo, ItemCode, ItemName, BOMQty, T2.ConsumpQty, T2.RecQty, T2.TransferQty, T3.RemainStockQty " +
                    "\nORDER BY T1.POSNo ASC, T2.SeqNo ASC ";
                sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtRMDetails);

            }
            catch (Exception ex)
            {
                ErrorText = "Taking Data : " + ex.Message;
            }
            cnn.con.Close();

            //Add to Dgv
            if (ErrorText.Trim() == "")
            {
                if (dtPOSDetails.Rows.Count > 0)
                {
                    if (dtPOSDetails.Rows[0]["StatusText"].ToString() == "ខ្វះ" || dtPOSDetails.Rows[0]["StatusText"].ToString() == "គ្រប់")
                    {
                        if (Convert.ToDouble(dtPOSDetails.Rows[0]["MinRemainSet"]) > 0)
                        {
                            if (Convert.ToDouble(dtPOSDetails.Rows[0]["MinRemainStockSet"]) > 0)
                            {
                                bool Found = false;
                                foreach (DataGridViewRow row in dgvScanned.Rows)
                                {
                                    if (row.Cells["POSNo"].Value.ToString() == POSNo)
                                    {
                                        Found = true;
                                        break;
                                    }
                                }
                                if (Found == false)
                                {
                                    double QtyAsSet = Convert.ToDouble(dtPOSDetails.Rows[0]["MinRemainSet"]);
                                    if (QtyAsSet > Convert.ToDouble(dtPOSDetails.Rows[0]["MinRemainStockSet"]))
                                        QtyAsSet = Convert.ToDouble(dtPOSDetails.Rows[0]["MinRemainStockSet"]);

                                    //Add to dtConsumption
                                    foreach (DataRow row in dtRMDetails.Rows)
                                    {
                                        dtConsumtionSave.Rows.Add(row["POSNo"].ToString(), row["ItemCode"].ToString());
                                        dtConsumtionSave.Rows[dtConsumtionSave.Rows.Count-1]["ItemName"] = row["ItemName"];
                                        dtConsumtionSave.Rows[dtConsumtionSave.Rows.Count - 1]["BOMQty"] = Convert.ToDouble(row["BOMQty"]);
                                        dtConsumtionSave.Rows[dtConsumtionSave.Rows.Count - 1]["ConsumpQty"] = Convert.ToDouble(row["ConsumpQty"]);
                                        dtConsumtionSave.Rows[dtConsumtionSave.Rows.Count - 1]["RecQty"] = Convert.ToDouble(row["RecQty"]);
                                        dtConsumtionSave.Rows[dtConsumtionSave.Rows.Count - 1]["TransferQty"] = Convert.ToDouble(row["TransferQty"]);
                                        dtConsumtionSave.Rows[dtConsumtionSave.Rows.Count - 1]["RemainQty"] = Convert.ToDouble(row["RemainQty"]);
                                        dtConsumtionSave.Rows[dtConsumtionSave.Rows.Count - 1]["RemainStockQty"] = Convert.ToDouble(row["RemainStockQty"]);
                                        dtConsumtionSave.Rows[dtConsumtionSave.Rows.Count - 1]["ActualTransQty"] = Convert.ToDouble(row["BOMQty"])* QtyAsSet;
                                        dtConsumtionSave.AcceptChanges();
                                    }

                                    //Add to DGV
                                    dgvScanned.Rows.Add();
                                    dgvScanned.Rows[dgvScanned.Rows.Count - 1].HeaderCell.Value = dgvScanned.Rows.Count.ToString();
                                    dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["POSNo"].Value = dtPOSDetails.Rows[0]["POSNo"].ToString();
                                    dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["WIPCode"].Value = dtPOSDetails.Rows[0]["WIPCode"].ToString();
                                    dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["WIPName"].Value = dtPOSDetails.Rows[0]["WIPName"].ToString();
                                    dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["POSQty"].Value = Convert.ToDouble(dtPOSDetails.Rows[0]["PosQty"]);
                                    dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["POSTransferQty"].Value = QtyAsSet;
                                    dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["ShipDate"].Value = Convert.ToDateTime(dtPOSDetails.Rows[0]["PosShipD"]);
                                    if (dgvScanned.Rows.Count > 0)
                                        this.dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["POSNo"].Selected = true;
                                    this.dgvScanned.ClearSelection();
                                    this.dgvScanned.CurrentCell = null;
                                    txtBarcode.Text = "";
                                    txtBarcode.Focus();
                                }
                                else
                                {
                                    MessageBox.Show("ស្កេនរួចហើយ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    txtBarcode.Focus();
                                    txtBarcode.SelectAll();
                                }
                            }
                            else
                            {
                                MessageBox.Show("គ្មានស្តុកគ្រប់គ្រាន់ទេ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                txtBarcode.Focus();
                                txtBarcode.SelectAll();
                            }
                        }
                        else
                        {
                            MessageBox.Show("មិនទាន់គ្រប់ SET ទេ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txtBarcode.Focus();
                            txtBarcode.SelectAll();
                        }
                    }
                    else
                    {
                        MessageBox.Show("ភីអូអេសនេះត្រូវបាន < "+ dtPOSDetails.Rows[0]["StatusText"].ToString() + " > រួចហើយ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtBarcode.Focus();
                        txtBarcode.SelectAll();
                    }
                }
                else
                {
                    MessageBox.Show("គ្មានភីអូអេសនេះនៅក្នុងប្រព័ន្ធទេ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtBarcode.Focus();
                    txtBarcode.SelectAll();
                }
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
            {
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBarcode.Focus();
                txtBarcode.SelectAll();
            }

            /*

                try
                {
                    cnn.con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT WIPCode, WIPName, PosQty, PosShipD, Status FROM tbSDConn1Rec " +
                                                                                    "WHERE NOT Status = 0 AND POSNo = '" + txtBarcode.Text.ToString() + "'", cnn.con);
                    DataTable dtPOSDetails = new DataTable();
                    da.Fill(dtPOSDetails);

                    if (dtPOSDetails.Rows.Count > 0)
                    {
                        if (dtPOSDetails.Rows[0]["Status"].ToString() == "2")
                        {
                            //Check Already have or not
                            int FoundDup = 0;
                            foreach (DataGridViewRow dgvRow in dgvScanned.Rows)
                            {
                                if (dgvRow.Cells["POSNo"].Value.ToString() == txtBarcode.Text.ToString())
                                {
                                    FoundDup = FoundDup + 1;
                                }
                            }

                            if (FoundDup == 0)
                            {
                                //Take Consumption
                                da = new SqlDataAdapter("SELECT T1.POSNo, ItemCode, ItemName, ConsumpQty, StockQTY FROM ( " +
                                                                                                "(SELECT POSNo, ItemCode, ItemName, ConsumpQty FROM  tbSDConn2Consump " +
                                                                                                "INNER JOIN tbSDConn1Rec ON tbSDConn1Rec.SysNo = tbSDConn2Consump.SysNo " +
                                                                                                "WHERE NOT Status = 0 AND POSNo = '" + txtBarcode.Text.ToString() + "') T1 " +
                                                                                                "FULL JOIN " +
                                                                                                "(SELECT POSNo, Code, SUM(StockValue) AS StockQTY FROM tbSDMCAllTransaction " +
                                                                                                "WHERE CancelStatus = 0 AND LocCode = 'KIT3' AND POSNo = '" + txtBarcode.Text.ToString() + "' " +
                                                                                                "GROUP BY POSNo, Code) T2 " +
                                                                                                "ON T1.POSNo = T2.POSNo AND T1.ItemCode = T2.Code)", cnn.con);
                                DataTable dtPOSConsum = new DataTable();
                                da.Fill(dtPOSConsum);
                                foreach (DataRow row in dtPOSConsum.Rows)
                                {
                                    dtConsumtionSave.Rows.Add();
                                    dtConsumtionSave.Rows[dtConsumtionSave.Rows.Count - 1]["POSNo"] = row["POSNo"].ToString();
                                    dtConsumtionSave.Rows[dtConsumtionSave.Rows.Count - 1]["RMCode"] = row["ItemCode"].ToString();
                                    dtConsumtionSave.Rows[dtConsumtionSave.Rows.Count - 1]["RMName"] = row["ItemName"].ToString();
                                    dtConsumtionSave.Rows[dtConsumtionSave.Rows.Count - 1]["ConsumpQty"] = Convert.ToDouble(row["ConsumpQty"]);
                                    dtConsumtionSave.Rows[dtConsumtionSave.Rows.Count - 1]["TransferQty"] = Convert.ToDouble(row["StockQTY"]);
                                    dtConsumtionSave.AcceptChanges();
                                }

                                //Add to DGV
                                if (dtPOSConsum.Rows.Count > 0)
                                {
                                    dgvScanned.Rows.Add();
                                    dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["POSNo"].Value = txtBarcode.Text;
                                    dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["WIPCode"].Value = dtPOSDetails.Rows[0]["WIPCode"];
                                    dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["WIPName"].Value = dtPOSDetails.Rows[0]["WIPName"];
                                    dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["POSQty"].Value = Convert.ToDouble(dtPOSDetails.Rows[0]["PosQty"]);
                                    dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["ShipDate"].Value = Convert.ToDateTime(dtPOSDetails.Rows[0]["PosShipD"]);
                                    txtBarcode.Text = "";
                                    txtBarcode.Focus();
                                }

                            }
                            else
                            {
                                WMsg.WarningText = "ភីអូអេសនេះស្កេនរួចហើយ!";
                                WMsg.ShowingMsg();
                                txtBarcode.Focus();
                                txtBarcode.SelectAll();
                            }

                        }
                        else if (dtPOSDetails.Rows[0]["Status"].ToString() == "3")
                        {
                            WMsg.WarningText = "ភីអូអេសនេះបញ្ចេញរួចរាល់ហើយ!";
                            WMsg.ShowingMsg();
                            txtBarcode.Focus();
                            txtBarcode.SelectAll();
                        }
                        else
                        {
                            WMsg.WarningText = "ភីអូអេសនេះគឺមិនទាន់គ្រប់ SET ទេ!";
                            WMsg.ShowingMsg();
                            txtBarcode.Focus();
                            txtBarcode.SelectAll();
                        }
                    }
                    else
                    {
                        WMsg.WarningText = "គ្មានភីអូអេសនេះនៅក្នុងប្រព័ន្ធទេ!";
                        WMsg.ShowingMsg();
                        txtBarcode.Focus();
                        txtBarcode.SelectAll();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtBarcode.Focus();
                    txtBarcode.SelectAll();
                }
            cnn.con.Close();

            */
        }
        private void CheckForEnableBtn()
        {
            //btnSave
            if (dgvScanned.Rows.Count > 0)
                btnSave.Enabled = true;
            else
                btnSave.Enabled = false;

            //btnDelete
            if (dgvScanned.SelectedCells.Count > 0)
                btnDelete.Enabled = true;
            else
                btnDelete.Enabled = false;
        }

    }
}
