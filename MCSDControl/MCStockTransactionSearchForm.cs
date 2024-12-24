using MachineDeptApp.MsgClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace MachineDeptApp.MCSDControl
{
    public partial class MCStockTransactionSearchForm : Form
    {
        ErrorMsgClass EMsg = new ErrorMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SqlCommand cmd;

        string ErrorText;

        public MCStockTransactionSearchForm()
        {
            InitializeComponent(); 
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Shown += MCStockTransactionSearchForm_Shown;
            this.btnExport.Click += BtnExport_Click;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnDelete.Click += BtnDelete_Click;
        }

        
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            /*

            string FuncNo = "";
            foreach (DataRow row in dtM_Func.Rows)
            {
                if (row[1].ToString() == dgvTransaction.Rows[dgvTransaction.CurrentCell.RowIndex].Cells[1].Value.ToString())
                {
                    FuncNo = row[0].ToString();
                }
            }

            if (FuncNo.Trim() != "" && FuncNo == "1")
            {
                DialogResult DLS = MessageBox.Show("តើអ្នកចង់ លុប/Cancel ទិន្នន័យនេះមែនដែរឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (DLS == DialogResult.Yes)
                {
                    //Receive
                    if (FuncNo == "1")
                    {
                        CheckAndCancelRecieveFunct();

                    }
                    SetColorForCancel();
                    dgvTransaction.ClearSelection();
                    FuncNo = "";

                }
            }
            else
            {
                MessageBox.Show("មុខងារនេះមិនទាន់អាច Cancel បានទេ!\nវានឹងមានឆាប់ៗនេះ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            */
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LbStatus.Visible = true;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            Cursor = Cursors.WaitCursor;
            dgvTransaction.Rows.Clear();
            Search();
            LbStatus.Text = "រកឃើញ  " + dgvTransaction.Rows.Count.ToString("N0") + "  ទិន្នន័យ";
            SumDgvFound();
            Cursor = Cursors.Default;

        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvTransaction.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("តើអ្នកចង់ទាញទិន្នន័យចេញមែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "CSV file (*.csv)|*.csv";
                    saveDialog.FileName = "MC_Transaction.csv";
                    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;
                        try
                        {
                            //String array for Csv
                            string[] outputCsv;
                            outputCsv = new string[dgvTransaction.Rows.Count + 1];

                            //Write Column name
                            string columnNames = "";
                            //Set Column Name
                            foreach (DataGridViewColumn col in dgvTransaction.Columns)
                            {
                                if (col.Visible == true)
                                {
                                    columnNames += col.HeaderText.ToString() + ",";
                                }
                            }
                            outputCsv[0] += columnNames;

                            //Row of data 
                            foreach (DataGridViewRow row in dgvTransaction.Rows)
                            {
                                foreach (DataGridViewColumn col in dgvTransaction.Columns)
                                {
                                    if (col.Visible == true)
                                    {
                                        string Value = "";
                                        if (row.Cells[col.Index].Value != null)
                                        {
                                            Value = row.Cells[col.Index].Value.ToString();
                                        }
                                        //Fix don't separate if it contain '\n' or ','
                                        Value = "\"" + Value.Replace("\"", "\"\"") + "\"";
                                        outputCsv[row.Index+1] += Value + ",";
                                    }
                                }
                            }                            

                            File.WriteAllLines(saveDialog.FileName, outputCsv, Encoding.UTF8);
                            Cursor = Cursors.Default;
                            MessageBox.Show("ទាញទិន្នន័យចេញរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            Cursor = Cursors.Default;
                            MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
        }
        private void MCStockTransactionSearchForm_Shown(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;

            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbSDMCLocation " +
                                                                                "ORDER BY LocName DESC", cnn.con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    CboLoc.Items.Add(row["LocName"].ToString());
                }
                CboLoc.SelectedIndex = 0;

                sda = new SqlDataAdapter("SELECT FuncCode, FuncName FROM tbSDMCFunction " +
                                                                                "ORDER BY FuncCode ASC", cnn.con);
                dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    CboFunc.Items.Add(row["FuncName"].ToString());
                }
                CboFunc.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
            {
                EMsg.AlertText = ErrorText;
                EMsg.ShowingMsg();
                panelHeader.Enabled = false;
            }
        }

        //Method
        private void CheckAndCancelBorrowFunct()
        {
            /*

            //Check Borrow to Stock Remain
            try
            {
                cnn.con.Open();
                SqlDataAdapter da = new SqlDataAdapter("", cnn.con);

                DataTable dtChecking = new DataTable();
                da.Fill(dtChecking);
                int FoundNotSame = 0;
                foreach (DataRow row in dtChecking.Rows)
                {
                    if (Convert.ToDouble(row[2].ToString()) != Convert.ToDouble(row[3].ToString()))
                    {
                        FoundNotSame = FoundNotSame + 1;
                    }
                }

                if (FoundNotSame == 0)
                {
                    DateTime UpdateDate = DateTime.Now;
                    string User = MenuFormV2.UserForNextForm;

                    //Set Status to tbSDConn1Rec
                    string query = "UPDATE tbSDConn1Rec SET " +
                                        "Status=0, " +
                                        "UpdateDate='" + UpdateDate + "'," +
                                        "UpdateBy=N'" + User + "' " +
                                        "WHERE SysNo = '" + SysNoSelected + "';";
                    cmd = new SqlCommand(query, cnn.con);
                    cmd.ExecuteNonQuery();

                    //Delete from tbSDConn2Consump
                    cmd = new SqlCommand("DELETE FROM tbSDConn2Consump WHERE SysNo ='" + SysNoSelected + "';", cnn.con);
                    cmd.ExecuteNonQuery();


                    //Set Status to tbSDMCAllTransaction
                    string query3 = "UPDATE tbSDMCAllTransaction SET " +
                                        "CancelStatus=1 " +
                                        "WHERE SysNo = '" + SysNoSelected + "';";
                    cmd = new SqlCommand(query3, cnn.con);
                    cmd.ExecuteNonQuery();

                    for (int i = 0; i < dgvTransaction.Rows.Count - 1; i++)
                    {
                        if (dgvTransaction.Rows[i].Cells[0].Value.ToString() == SysNoSelected)
                        {
                            dgvTransaction.Rows[i].Cells[10].Value = true;
                        }
                    }

                }
                else
                {
                    MessageBox.Show("ទិន្នន័យស្តុកនេះត្រូវបានប្រើប្រាស់ឬក៏ប្រទាក់ជាមួយទិន្នន័យផ្សេង!\nសូមលុបទិន្នន័យដែលប្រើប្រាស់វាជាមុនសិន!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

            //Cancel Transaction

            */
        }
        private void CheckAndCancelRecieveFunct()
        {
            /*

            try
            {
                cnn.con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT T1.Code, T1.LocCode, T1.POSNo, ReceiveQty, StockValue FROM " +
                    "(SELECT Code, LocCode, POSNo, SUM(ReceiveQty) AS ReceiveQty FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND SysNo ='"+SysNoSelected+"' " +
                    "GROUP BY Code, LocCode, POSNo) T1 LEFT JOIN (SELECT Code, LocCode, POSNo, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction WHERE CancelStatus = 0 " +
                    "GROUP BY Code, LocCode, POSNo) T2 " +
                    "ON T1.Code=T2.Code AND T1.LocCode = T2.LocCode AND T1.POSNo=T2.POSNo", cnn.con);

                DataTable dtChecking = new DataTable();
                da.Fill(dtChecking);
                int FoundNotSame = 0;
                foreach (DataRow row in dtChecking.Rows)
                {
                    if (Convert.ToDouble(row[3].ToString()) > Convert.ToDouble(row[4].ToString()))
                    {
                        FoundNotSame = FoundNotSame + 1;
                    }
                }

                if (FoundNotSame == 0)
                {
                    DateTime UpdateDate = DateTime.Now;
                    string User = MenuFormV2.UserForNextForm;

                    //Set Status to tbSDConn1Rec
                    string query = "UPDATE tbSDConn1Rec SET " +
                                        "Status=0, " +
                                        "UpdateDate='" + UpdateDate + "'," +
                                        "UpdateBy=N'" + User + "' " +
                                        "WHERE SysNo = '" + SysNoSelected + "';";
                    cmd = new SqlCommand(query, cnn.con);
                    cmd.ExecuteNonQuery();

                    //Delete from tbSDConn2Consump
                    cmd = new SqlCommand("DELETE FROM tbSDConn2Consump WHERE SysNo ='" + SysNoSelected + "';", cnn.con);
                    cmd.ExecuteNonQuery();


                    //Set Status to tbSDMCAllTransaction
                    string query3 = "UPDATE tbSDMCAllTransaction SET " +
                                        "CancelStatus=1 " +
                                        "WHERE SysNo = '" + SysNoSelected + "';";
                    cmd = new SqlCommand(query3, cnn.con);
                    cmd.ExecuteNonQuery();

                    for (int i = 0; i < dgvTransaction.Rows.Count - 1; i++)
                    {
                        if (dgvTransaction.Rows[i].Cells[0].Value.ToString() == SysNoSelected)
                        {
                            dgvTransaction.Rows[i].Cells[10].Value = true;
                        }
                    }

                }
                else
                {
                    MessageBox.Show("ទិន្នន័យស្តុកនេះត្រូវបានប្រើប្រាស់ឬក៏ប្រទាក់ជាមួយទិន្នន័យផ្សេង!\nសូមលុបទិន្នន័យដែលប្រើប្រាស់វាជាមុនសិន!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

            */
        }
        private void CheckBtnCancel()
        {
            if (dgvTransaction.SelectedCells.Count > 0 && dgvTransaction.CurrentCell.RowIndex < dgvTransaction.Rows.Count - 1)
            {
                if (dgvTransaction.Rows[dgvTransaction.CurrentCell.RowIndex].Cells[10].Value.ToString() == "False")
                {
                    btnDelete.Enabled = true;
                    btnDelete.BackColor = Color.White;
                }
                else
                {
                    btnDelete.Enabled = false;
                    btnDelete.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
                }
            }
            else
            {
                btnDelete.Enabled = false;
                btnDelete.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
            }
        }
        private void SetColorForCancel()
        {
            for (int i = 0; i < dgvTransaction.Rows.Count; i++)
            {
                if (dgvTransaction.Rows[i].Cells[10].Value != null && dgvTransaction.Rows[i].Cells[10].Value.ToString() == "True")
                {
                    dgvTransaction.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
                }
            }
        }
        private void SumDgvFound()
        {
            if (dgvTransaction.Rows.Count > 0)
            {
                double TotalReceived = 0;
                double TotalTransfered = 0;
                foreach (DataGridViewRow Row in dgvTransaction.Rows)
                {
                    if (Row.Cells[10].Value != null && Row.Cells[10].Value.ToString() == "False")
                    {
                        TotalReceived = TotalReceived + Convert.ToDouble(Row.Cells[6].Value.ToString());
                        TotalTransfered = TotalTransfered + Convert.ToDouble(Row.Cells[7].Value.ToString());
                    }
                }

                int lastrowofDgv = dgvTransaction.RowCount;

                dgvTransaction.Rows.Insert(lastrowofDgv, "", "", "", "", "", "សរុប​ ", TotalReceived, TotalTransfered);
                dgvTransaction.Rows[lastrowofDgv].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvTransaction.Rows[lastrowofDgv].DefaultCellStyle.Font = new System.Drawing.Font("Khmer OS Battambang", 10, FontStyle.Bold);

                SetColorForCancel();
            }
        }
        private void Search()
        {
            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Column");
            dtSQLCond.Columns.Add("Value");
            if (txtCode.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("Code = ", "'" + txtCode.Text + "' ");
            }
            if (txtItem.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("RMDes LIKE ", "'%" + txtItem.Text + "%' ");
            }
            if (ChkDate.Checked == true)
            {
                dtSQLCond.Rows.Add("RegDate BETWEEN ", "'" + DtDate.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + DtEndDate.Value.ToString("yyyy-MM-dd") + " 23:59:59' ");
            }
            if (CboFunc.Text.Trim() != "ទាំងអស់")
            {
                dtSQLCond.Rows.Add("FuncName = ", "N'" + CboFunc.Text + "' ");
            }
            if (CboStatus.Text.Trim() != "ទាំងអស់")
            {
                dtSQLCond.Rows.Add("CancelStatus = ", "0");
            }
            if (txtDocNo.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("POSNo LIKE ", "'%" + txtDocNo.Text + "%' ");
            }
            if (txtSysNo.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("SysNo LIKE ", "'%" + txtSysNo.Text + "%' ");
            }
            if (CboLoc.Text.Trim() != "ទាំងអស់")
            {
                dtSQLCond.Rows.Add("LocName = ", "N'" + CboLoc.Text + "' ");
            }
            if (txtRemarks.Text.Trim() != "")
            {
                string SearchValue = txtRemarks.Text;
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*","%");
                    dtSQLCond.Rows.Add("Remarks LIKE ", "'" + SearchValue + "' ");
                }
                else
                {
                    dtSQLCond.Rows.Add("Remarks = ", "'" + SearchValue + "' ");
                }
            }
            string SQLCond = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                if (SQLCond.Trim() == "")
                {
                    SQLCond = "WHERE " + row[0].ToString() + row[1].ToString();
                }
                else
                {
                    SQLCond = SQLCond + "AND " + row[0].ToString() + row[1].ToString();
                }
            }

            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT SysNo, FuncName, LocName, POSNo, Code, RMDes, ReceiveQty, TransferQty, RegDate, RegBy, CancelStatus, TableTrans.Remarks FROM " +
                    "\n\t(Select * From tbSDMCAllTransaction) TableTrans " +
                    "\n\tINNER JOIN (SELECT * FROM tbSDMCFunction) TableFunct ON TableTrans.Funct = TableFunct.FuncCode " +
                    "\n\tLEFT JOIN (SELECT * FROM tbSDMCLocation) TableLoc ON TableTrans.LocCode = TableLoc.LocCode " + SQLCond +
                    "\nORDER BY RegDate ASC, SysNo ASC, Code ASC";
                Console.WriteLine(SQLQuery);
                SqlDataAdapter da = new SqlDataAdapter(SQLQuery, cnn.con);
                DataTable dtFound = new DataTable();
                da.Fill(dtFound);
                foreach (DataRow row in dtFound.Rows)
                {
                    dgvTransaction.Rows.Add();
                    Boolean CancelStat = false;
                    if (row["CancelStatus"].ToString() != "0")
                    {
                        CancelStat = true;
                    }
                    dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["SysNo"].Value = row["SysNo"];
                    dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["FuncName"].Value = row["FuncName"];
                    dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["LocName"].Value = row["LocName"];
                    dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["POSNo"].Value = row["POSNo"];
                    dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["Code"].Value = row["Code"];
                    dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["ItemName"].Value = row["RMDes"];
                    dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["ReceiveQty"].Value = Convert.ToDouble(row["ReceiveQty"]);
                    dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["TransferQty"].Value = Convert.ToDouble(row["TransferQty"]);
                    dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["RegDate"].Value = Convert.ToDateTime(row["RegDate"]);
                    dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["RegBy"].Value = row["RegBy"];
                    dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["CancelStatus"].Value = CancelStat;
                    dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["Remarks"].Value = row["Remarks"];
                }
                dgvTransaction.ClearSelection();
                CheckBtnCancel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            cnn.con.Close();
        }

    }
}
