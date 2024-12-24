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
        WarningMsgClass WMsg = new WarningMsgClass();
        ErrorMsgClass EMsg = new ErrorMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        ErrorReportAsTxt EOutput = new ErrorReportAsTxt();
        SQLConnect cnn = new SQLConnect();
        SqlCommand cmd;
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

            //Btn
            this.btnDelete.Click += BtnDelete_Click;
            this.btnSave.Click += BtnSave_Click;
        }

        private void DgvScanned_CurrentCellChanged(object sender, EventArgs e)
        {
            dgvConsumption.Rows.Clear();
            if (dgvScanned.SelectedCells.Count>0 && dgvScanned.CurrentCell != null && dgvScanned.CurrentCell.RowIndex > -1)
            {
                foreach (DataRow row in dtConsumtionSave.Rows)
                {
                    if (dgvScanned.Rows[dgvScanned.CurrentCell.RowIndex].Cells["POSNo"].Value.ToString() == row["POSNo"].ToString())
                    {
                        dgvConsumption.Rows.Add();
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["RMCode"].Value = row["RMCode"];
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["RMName"].Value = row["RMName"];
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["TotalQty"].Value = Convert.ToDouble(row["ConsumpQty"]);
                        dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["StockPOS"].Value = Convert.ToDouble(row["TransferQty"]);
                    }
                }
            }
            CheckBtn();
        }
        private void DgvScanned_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //Assign SeqNo
            foreach (DataGridViewRow row in dgvScanned.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString();
            }
            CheckBtn();
        }
        private void DgvScanned_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //Assign SeqNo
            foreach (DataGridViewRow row in dgvScanned.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString();
            }
            CheckBtn();
            dgvScanned.CurrentCell = null;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            int FoundAbnormal = 0;
            foreach (DataGridViewRow row in dgvScanned.Rows)
            {
                int Found = 0;
                foreach (DataRow dtRow in dtConsumtionSave.Rows)
                {
                    if (row.Cells["POSNo"].Value.ToString() == dtRow["POSNo"].ToString())
                    {
                        Found++;
                        break;
                    }
                }

                if (Found == 0)
                {
                    FoundAbnormal++;
                    ErrorText = row.Cells["POSNo"].Value.ToString() + " : No Consumption Data!"; 
                    break;
                }
            }

            if (FoundAbnormal == 0)
            {
                QMsg.QAText = "តើអ្នករក្សាទុកទិន្នន័យនេះមែនទេ?";
                QMsg.UserClickedYes = false;
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    LbStatus.Text = "កំពុងរក្សាទុក . . . .";
                    LbStatus.Visible = true;
                    LbStatus.Refresh();
                    Cursor = Cursors.WaitCursor;

                    //Save
                    foreach (DataGridViewRow dgvRow in dgvScanned.Rows)
                    {
                        try
                        {
                            DateTime RegNow = DateTime.Now;
                            string User = MenuFormV2.UserForNextForm;
                            string TransNo = "TRF0000000001";
                            //Find Last TransNo
                            cnn.con.Open();
                            SqlDataAdapter da = new SqlDataAdapter("SELECT SysNo FROM tbSDMCAllTransaction " +
                                                                                            "WHERE SysNo=(SELECT MAX(SysNo) FROM tbSDMCAllTransaction WHERE Funct =2) Group By SysNo", cnn.con);
                            DataTable dtTransNo = new DataTable();
                            da.Fill(dtTransNo);
                            if (dtTransNo.Rows.Count > 0)
                            {
                                string LastTransNo = dtTransNo.Rows[0][0].ToString();
                                double NextTransNo = Convert.ToDouble(LastTransNo.Substring(LastTransNo.Length - 10)) + 1;
                                TransNo = "TRF" + NextTransNo.ToString("0000000000");

                            }

                            string POSNoSave = "";
                            POSNoSave = dgvRow.Cells["POSNo"].Value.ToString();
                            SqlCommand cmd;

                            //Add to Transaction
                            foreach (DataRow row in dtConsumtionSave.Rows)
                            {
                                if (row["POSNo"].ToString() == POSNoSave)
                                {
                                    string Code = row["RMCode"].ToString();
                                    string RMName = row["RMName"].ToString();
                                    double TransQty = Convert.ToDouble(row["ConsumpQty"].ToString());

                                    //Add to AllTransaction Stock-Out
                                    cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                                                   "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs, @Rm)", cnn.con);
                                    cmd.Parameters.AddWithValue("@Sn", TransNo);
                                    cmd.Parameters.AddWithValue("@Ft", 2);
                                    cmd.Parameters.AddWithValue("@Lc", "KIT3");
                                    cmd.Parameters.AddWithValue("@Pn", POSNoSave);
                                    cmd.Parameters.AddWithValue("@Cd", Code);
                                    cmd.Parameters.AddWithValue("@Rmd", RMName);
                                    cmd.Parameters.AddWithValue("@Rq", 0);
                                    cmd.Parameters.AddWithValue("@Tq", TransQty);
                                    cmd.Parameters.AddWithValue("@Sv", (TransQty * (-1)));
                                    cmd.Parameters.AddWithValue("@Rd", RegNow);
                                    cmd.Parameters.AddWithValue("@Rb", User);
                                    cmd.Parameters.AddWithValue("@Cs", 0);
                                    cmd.Parameters.AddWithValue("@Rm", POSNoSave);
                                    cmd.ExecuteNonQuery();

                                    //Add to AllTransaction Stock-In
                                    cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                                                   "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs, @Rm)", cnn.con);
                                    cmd.Parameters.AddWithValue("@Sn", TransNo);
                                    cmd.Parameters.AddWithValue("@Ft", 2);
                                    cmd.Parameters.AddWithValue("@Lc", "MC1");
                                    //cmd.Parameters.AddWithValue("@Pn", "");
                                    cmd.Parameters.AddWithValue("@Pn", POSNoSave);
                                    cmd.Parameters.AddWithValue("@Cd", Code);
                                    cmd.Parameters.AddWithValue("@Rmd", RMName);
                                    cmd.Parameters.AddWithValue("@Rq", TransQty);
                                    cmd.Parameters.AddWithValue("@Tq", 0);
                                    cmd.Parameters.AddWithValue("@Sv", TransQty);
                                    cmd.Parameters.AddWithValue("@Rd", RegNow.AddSeconds(1));
                                    cmd.Parameters.AddWithValue("@Rb", User);
                                    cmd.Parameters.AddWithValue("@Cs", 0);
                                    cmd.Parameters.AddWithValue("@Rm", POSNoSave);
                                    cmd.ExecuteNonQuery();

                                }
                            }

                            //Set Status to Complete for tbSDConn1Rec
                            string query = "UPDATE tbSDConn1Rec SET " +
                                                "Status=3, " +
                                                "UpdateDate='" + RegNow.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                "UpdateBy=N'" + User + "' " +
                                                "WHERE Status=2 AND POSNo = '" + POSNoSave + "';";
                            cmd = new SqlCommand(query, cnn.con);
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            if (ErrorText.Trim() == "")
                            {
                                ErrorText = POSNo + " : " + ex.Message;
                            }
                            else
                            {
                                ErrorText += "\n" + POSNo + " : " + ex.Message;
                            }
                        }
                        cnn.con.Close();
                    }

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        CleardtConsumtionSave();
                        LbStatus.Text = "រក្សាទុករួចរាល់";
                        MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LbStatus.Visible = false;
                        dgvScanned.Rows.Clear();
                        dgvScanned.CurrentCell = null;
                        dgvConsumption.Rows.Clear();

                    }
                    else
                    {
                        LbStatus.Text = "រក្សាទុកមានបញ្ហា";
                        ErrorReportAsTxt.PrintError = ErrorText;
                        this.EOutput.Output();
                        EMsg.AlertText = "រក្សាទុកមានបញ្ហា! File txt លម្អិតអំពី Error : \n" + ErrorReportAsTxt.ErrorPath;
                        EMsg.ShowingMsg();
                    }
                }
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvScanned.SelectedCells.Count > 0 && dgvScanned.CurrentCell.RowIndex > -1)
            {
                QMsg.UserClickedYes = false;
                QMsg.QAText = "តើអ្នកចង់លុបទិន្នន័យនេះមែនដែរឬទេ?";
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    for (int j = dtConsumtionSave.Rows.Count - 1; j > -1; j--)
                    {
                        if (dtConsumtionSave.Rows[j]["POSNo"].ToString() == dgvScanned.Rows[dgvScanned.CurrentCell.RowIndex].Cells["POSNo"].Value.ToString())
                        {
                            dtConsumtionSave.Rows[j].Delete();
                        }
                    }
                    dtConsumtionSave.AcceptChanges();
                    dgvScanned.Rows.RemoveAt(dgvScanned.CurrentCell.RowIndex);
                    dgvScanned.Refresh();
                    dgvScanned.ClearSelection();
                    dgvScanned.CurrentCell= null;
                    dgvConsumption.Rows.Clear();
                }
            }
        }

        private void SDTransferForm_Load(object sender, EventArgs e)
        {
            CleardtConsumtionSave();
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
                        ScanBarcode();
                        if (dgvScanned.Rows.Count > 0)
                        {
                            this.dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["POSNo"].Selected = true;

                        }
                        this.dgvScanned.ClearSelection();
                        this.dgvScanned.CurrentCell = null;
                    }
                }
            }
        }

        //Method
        private void CleardtConsumtionSave()
        {
            dtConsumtionSave = new DataTable();
            dtConsumtionSave.Columns.Add("POSNo");
            dtConsumtionSave.Columns.Add("RMCode");
            dtConsumtionSave.Columns.Add("RMName");
            dtConsumtionSave.Columns.Add("ConsumpQty");
            dtConsumtionSave.Columns.Add("TransferQty");

        }
        private void ScanBarcode()
        {
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
        }
        private void CheckBtn()
        {
            //btnDelete
            if (dgvScanned.SelectedCells.Count > 0 && dgvScanned.CurrentCell != null && dgvScanned.CurrentCell.RowIndex>-1)
            {
                btnDelete.Enabled = true;
                btnDeleteGRAY.SendToBack();
            }
            else
            {
                btnDelete.Enabled = false;
                btnDeleteGRAY.BringToFront();
            }

            //btnSave
            if (dgvScanned.Rows.Count > 0)
            {
                btnSave.Enabled = true;
                btnSaveGRAY.SendToBack();
            }
            else
            {
                btnSave.Enabled = false;
                btnSaveGRAY.BringToFront();
            }
        }

    }
}
