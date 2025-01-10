using MachineDeptApp.MsgClass;
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
using System.IO;

namespace MachineDeptApp.NG_Input
{
    public partial class NGAdjustSearchForm : Form
    {
        ErrorMsgClass EMsg = new ErrorMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();   
        QuestionMsgClass QMsg = new QuestionMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        DataTable dtOBSUnitPrice;
        DataTable dtForPrintExcel;

        string ErrorText;

        public NGAdjustSearchForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Shown += NGAdjustSearchForm_Shown;
            this.dtpRegFrom.ValueChanged += DtpRegFrom_ValueChanged;
            this.dtpRegTo.ValueChanged += DtpRegTo_ValueChanged;

            this.dgvSearchResult.CellClick += DgvSearchResult_CellClick;

            this.btnUnSelectAll.Click += BtnUnSelectAll_Click;
            this.btnSelectAll.Click += BtnSelectAll_Click;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnExport.Click += BtnExport_Click;
            this.btnPrint.Click += BtnPrint_Click;

        }

        
        private void DgvSearchResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSearchResult.Columns[e.ColumnIndex].Name == "ChkForPrint")
            {
                if (e.RowIndex > -1)
                {
                    if (dgvSearchResult.Rows[e.RowIndex].Cells["PrintStatus"].Value.ToString() != "OK")
                    {
                        if (dgvSearchResult.Rows[e.RowIndex].Cells["ChkForPrint"].Value.ToString().ToUpper() == "FALSE")
                        {
                            dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                        }
                        else
                        {
                            dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                        }
                        dgvSearchResult.ClearSelection();
                        dgvSearchResult.CurrentCell = null;
                        dgvSearchResult.Refresh();
                    }
                    else
                    {
                        WMsg.WarningText = "ទិន្នន័យនេះព្រីនចេញរួចរាល់ហើយ!";
                        WMsg.ShowingMsg();
                    }
                }
            }
        }
        private void NGAdjustSearchForm_Shown(object sender, EventArgs e)
        {
            CboStatus.SelectedIndex = 0;
        }
        private void DtpRegTo_ValueChanged(object sender, EventArgs e)
        {
            ChkRegDate.Checked = true;
        }
        private void DtpRegFrom_ValueChanged(object sender, EventArgs e)
        {
            ChkRegDate.Checked = true;
        }

        private void BtnUnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                if (dgvRow.Cells["PrintStatus"].Value.ToString() != "OK")
                {
                    dgvRow.Cells["ChkForPrint"].Value = false;
                }
            }
            dgvSearchResult.Refresh();
            dgvSearchResult.ClearSelection();
        }
        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                if (dgvRow.Cells["PrintStatus"].Value.ToString() != "OK")
                {
                    dgvRow.Cells["ChkForPrint"].Value = true;
                }
            }
            dgvSearchResult.Refresh();
            dgvSearchResult.ClearSelection();
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            dgvSearchResult.Rows.Clear();
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();

            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Val");

            if (txtRMCode.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("Code = ", "'" + txtRMCode.Text + "'");
            }
            if (txtRMName.Text.Trim() != "")
            {
                string SearchValue = txtRMName.Text;
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*", "");
                }
                dtSQLCond.Rows.Add("ItemName LIKE ", "'%" + SearchValue + "%'");
            }
            if (CboStatus.Text != "ALL")
            {
                dtSQLCond.Rows.Add("ReqStatus = '"+CboStatus.Text+"'");
            }
            if (ChkRegDate.Checked == true)
            {
                dtSQLCond.Rows.Add("RegDate BETWEEN ", "'" + dtpRegFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dtpRegTo.Value.ToString("yyyy-MM-dd") + " 23:59:59'");
            }
            if (txtPOSC.Text.Trim() != "")
            {
                string SearchValue = txtPOSC.Text;
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*", "%");
                    dtSQLCond.Rows.Add("POSNo LIKE ", "'" + SearchValue + "'");
                }
                else
                {
                    dtSQLCond.Rows.Add("POSNo = ", "'" + SearchValue + "'");
                }
            }

            string SQLConds = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                if (SQLConds.Trim() == "")
                {
                    SQLConds = "WHERE " + row["Col"] + row["Val"];
                }
                else
                {
                    SQLConds += " AND " + row["Col"] + row["Val"];
                }
            }

            //Taking Data
            DataTable dtSearchResult = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT * FROM " +
                    "\n(SELECT T1.*, ItemName FROM " +
                    "\n(SELECT SysNo, POSNo, Code, Qty, " +
                    "\nCASE " +
                    "\n\tWHEN ReqStatus=0 THEN 'NOT YET' " +
                    "\n\tELSE 'OK' " +
                    "\nEND AS ReqStatus, RegDate FROM tbNGReqAdj) T1 " +
                    "\nLEFT JOIN (SELECT * FROM tbMasterItem) T2 " +
                    "\nON T1.Code=T2.ItemCode) TbAllData \n" + SQLConds +
                    "\nORDER BY RegDate ASC";
                //Console.WriteLine(SQLQuery);
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtSearchResult);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

            //Taking UP & Amount
            if(ErrorText.Trim() == "" && dtSearchResult.Rows.Count > 0)
            {
                string CodeIN = "";
                foreach(DataRow row in dtSearchResult.Rows)
                {
                    if (CodeIN.Contains(row["Code"].ToString())==false)
                    {
                        if (CodeIN.Trim() == "")
                        {
                            CodeIN = "'" + row["Code"].ToString() + "'";
                        }
                        else
                        {
                            CodeIN += ", '" + row["Code"].ToString() + "'";
                        }
                    }
                }

                try
                {
                    cnnOBS.conOBS.Open();
                    string SQLQuery = "SELECT T1.ItemCode, ItemName, T3.EffDate , COALESCE(T2.UnitPrice,0) AS UnitPrice FROM " +
                        "\n(SELECT ItemCode, ItemName FROM mstitem WHERE DelFlag=0 AND ItemType=2) T1 " +
                        "\nLEFT JOIN (SELECT ItemCode, UnitPrice, EffDate FROM mstpurchaseprice WHERE DelFlag=0) T2 " +
                        "\nON T1.ItemCode=T2.ItemCode " +
                        "\nINNER JOIN (SELECT ItemCode, MAX(EffDate) AS EffDate FROM mstpurchaseprice GROUP BY ItemCode) T3 " +
                        "\nON T2.ItemCode=T3.ItemCode AND T2.EffDate=T3.EffDate " +
                        "\nWHERE T1.ItemCode IN (" + CodeIN + ") " +
                        "\nORDER BY ItemCode ASC ";
                    //Console.WriteLine(SQLQuery);
                    dtOBSUnitPrice = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                    sda.Fill(dtOBSUnitPrice);
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                }
                cnnOBS.conOBS.Close();
            }

            //Add to Dgv
            foreach (DataRow row in dtSearchResult.Rows)
            {
                dgvSearchResult.Rows.Add();
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["ChkForPrint"].Value = false;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count-1].Cells["SysNo"].Value = row["SysNo"];
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["POSNo"].Value = row["POSNo"];
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Code"].Value = row["Code"];
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["ItemName"].Value = row["ItemName"];
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Qty"].Value = Convert.ToDouble(row["Qty"]);
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["PrintStatus"].Value = row["ReqStatus"];
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["RegDate"].Value = Convert.ToDateTime(row["RegDate"]);

            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                LbStatus.Text = "រកឃើញទិន្នន័យ " + dgvSearchResult.Rows.Count.ToString("N0");
                LbStatus.Refresh();
                dgvSearchResult.ClearSelection();
                dgvSearchResult.CurrentCell = null;
            }
            else
            {
                EMsg.AlertText = ErrorText;
                EMsg.ShowingMsg();
            }
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvSearchResult.Rows.Count > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV file (*.csv)|*.csv";
                saveDialog.FileName = "NG Adjust " + DateTime.Now.ToString("yyyyMMdd HHmmss") + ".csv";
                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;
                    try
                    {
                        //Write Column name
                        int columnCount = 0;
                        foreach (DataGridViewColumn DgvCol in dgvSearchResult.Columns)
                        {
                            if (DgvCol.Visible == true)
                            {
                                columnCount = columnCount + 1;
                            }
                        }
                        string columnNames = "";

                        //String array for Csv
                        string[] outputCsv;
                        outputCsv = new string[dgvSearchResult.Rows.Count + 1];

                        //Set Column Name
                        for (int i = 0; i < columnCount; i++)
                        {
                            if (dgvSearchResult.Columns[i].Visible == true)
                            {
                                columnNames += dgvSearchResult.Columns[i].HeaderText.ToString() + ",";
                            }
                        }
                        outputCsv[0] += columnNames;

                        //Row of data 
                        for (int i = 1; (i - 1) < dgvSearchResult.Rows.Count; i++)
                        {
                            for (int j = 0; j < columnCount; j++)
                            {
                                if (dgvSearchResult.Columns[j].Visible == true)
                                {
                                    string Value = "";
                                    if (dgvSearchResult.Rows[i - 1].Cells[j].Value != null)
                                    {
                                        Value = dgvSearchResult.Rows[i - 1].Cells[j].Value.ToString();
                                    }
                                    //Fix don't separate if it contain '\n' or ','
                                    Value = "\"" + Value.Replace("\"", "\"\"") + "\"";
                                    outputCsv[i] += Value + ",";
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
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            int FoundTobePrint = 0;
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                if (dgvRow.Cells["ChkForPrint"].Value.ToString().ToLower() == "true" && dgvRow.Cells["ChkForPrint"].Value.ToString().ToLower() != "ok")
                {
                    FoundTobePrint++;
                    break;
                }
            }
            if (FoundTobePrint > 0)
            {
                QMsg.QAText = "តើអ្នកចង់ព្រីនទិន្នន័យទាំងនេះចេញមែនឬទេ?";
                QMsg.UserClickedYes = false;
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    PrintExcelOut();
                }
            }
            else
            {
                MessageBox.Show("សូមជ្រើសរើសទិន្នន័យដើម្បីព្រីនចេញ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }



        //Function
        private void PrintExcelOut()
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            string fName = "";
            string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\NGRequest";
            ClearDtForPrintExcel();

            //Double Check Document is already print or not
            string SysNoIN = "";
            try
            {
                cnn.con.Open();
                foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
                {
                    if (dgvRow.Cells["ChkForPrint"].Value.ToString().ToLower() == "true" && dgvRow.Cells["ChkForPrint"].Value.ToString().ToLower() != "ok")
                    {
                        string SysNoCheck = dgvRow.Cells["SysNo"].Value.ToString();
                        string SQLQuery = "SELECT * FROM tbNGReqAdj WHERE ReqStatus = 0 AND SysNo='"+SysNoCheck+"'";
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (SysNoIN.Trim() == "")
                            {
                                SysNoIN = "'"+SysNoCheck+"'";
                            }
                            else
                            {
                                SysNoIN += ", '" + SysNoCheck + "'";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

            //Calculate Total
            Console.WriteLine(SysNoIN);
            if (ErrorText.Trim() == "" && SysNoIN.Trim()!="")
            {
                try
                {
                    cnn.con.Open();

                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                }
                cnn.con.Close();
            }


            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {

            }
            else
            {
                EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                EMsg.ShowingMsg();
            }
        }

        private void ClearDtForPrintExcel()
        {
            dtForPrintExcel = new DataTable();
            dtForPrintExcel.Columns.Add("RMCode");
            dtForPrintExcel.Columns.Add("ItemName");
            dtForPrintExcel.Columns.Add("MatCalcFlag");
            dtForPrintExcel.Columns.Add("Maker");
            dtForPrintExcel.Columns.Add("MatTypeName");
            dtForPrintExcel.Columns.Add("Qty");
            dtForPrintExcel.Columns.Add("UnitPrice");
        }
    }
}
