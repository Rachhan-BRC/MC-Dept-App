﻿using System;
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
using Excel = Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

namespace MachineDeptApp.NG_Input
{
    public partial class NGInprocessSearchForm : Form
    {
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SQLConnect cnn = new SQLConnect();
        ErrorReportAsTxt OutputError = new ErrorReportAsTxt();
        DataTable dtForPrintExcel;
        DataTable dtCuttingStock;
        public DataTable dtNotEnough;

        string fName;
        string ErrorText;

        public NGInprocessSearchForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Shown += NGInprocessSearchForm_Shown;
            this.dgvSearchResult.CellClick += DgvSearchResult_CellClick;
            this.dgvSearchResult.CellDoubleClick += DgvSearchResult_CellDoubleClick;
            this.dtpFrom.ValueChanged += DtpFrom_ValueChanged;
            this.dtpToo.ValueChanged += DtpToo_ValueChanged;
            this.dtpRegFrom.ValueChanged += DtpRegFrom_ValueChanged;
            this.dtpRegTo.ValueChanged += DtpRegTo_ValueChanged;

            //Btn
            this.btnSearch.Click += BtnSearch_Click;
            this.btnUnSelectAll.Click += BtnUnSelectAll_Click;
            this.btnSelectAll.Click += BtnSelectAll_Click;
            this.btnExport.Click += BtnExport_Click;
            this.btnPrint.Click += BtnPrint_Click;

        }

        private void DtpRegTo_ValueChanged(object sender, EventArgs e)
        {
            ChkRegDate.Checked= true;
        }
        private void DtpRegFrom_ValueChanged(object sender, EventArgs e)
        {
            ChkRegDate.Checked = true;
        }
        private void DtpToo_ValueChanged(object sender, EventArgs e)
        {
            ChkDate.Checked = true;
        }
        private void DtpFrom_ValueChanged(object sender, EventArgs e)
        {
            ChkDate.Checked = true;
        }

        //Btn
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
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់ព្រីនទិន្នន័យទាំងនេះចេញមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    CalcForPrint();
                }
            }
            else
            {
                MessageBox.Show("សូមជ្រើសរើសទិន្នន័យដើម្បីព្រីនចេញ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvSearchResult.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("តើអ្នកចង់ទាញទិន្នន័យចេញមែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "CSV file (*.csv)|*.csv";
                    saveDialog.FileName = "NGInprocess "+DateTime.Now.ToString("yyyyMMdd HHmmss")+".csv";
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
            dgvSearchResult.Rows.Clear();
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();

            //Set SQL WHERE Condition
            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Val");
            if (txtRMCode.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("TbNGPsc.RMCode = ", "'" + txtRMCode.Text + "'");
            }
            if (txtRMName.Text.Trim() != "")
            {
                string SearchValue = txtRMName.Text;
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*", "");
                }
                dtSQLCond.Rows.Add("TbRMMaster.ItemName LIKE ", "'%" + SearchValue + "%'");
            }
            if (txtPOSC.Text.Trim() != "")
            {
                string SearchValue = txtPOSC.Text;
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*", "%");
                    dtSQLCond.Rows.Add("TbNGPsc.PosCNo LIKE ", "'" + SearchValue + "'");
                }
                else
                {
                    dtSQLCond.Rows.Add("TbNGPsc.PosCNo = ", "'" + SearchValue + "'");
                }
            }
            if (CboStatus.Text.Trim() != "" && CboStatus.Text.ToString() != "ALL")
            {
                string Status = "= 0";
                if (CboStatus.Text.ToString() != "NOT YET")
                {
                    Status = "IS NULL";
                }
                dtSQLCond.Rows.Add("TbNGStatus.ReqStatus ", Status);
            }
            if (ChkDate.Checked == true)
            {
                dtSQLCond.Rows.Add("PosPDelDate BETWEEN '" + dtpFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dtpToo.Value.ToString("yyyy-MM-dd") + " 23:59:59'");
            }
            if (ChkRegDate.Checked == true)
            {
                dtSQLCond.Rows.Add("TbNGStatus.RegDate BETWEEN '" + dtpRegFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dtpRegTo.Value.ToString("yyyy-MM-dd") + " 23:59:59'");
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

            //Take Data from DB
            DataTable dtSearchResult = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT TbPOSDetails.PosPDelDate, TbNGPsc.PosCNo, " +
                    "\nNULLIF(CONCAT(MC1Name, " +
                    "\n\tCASE " +
                    "\n\t\tWHEN LEN(MC2Name)>1 THEN ' & '  " +
                    "\n\t\tELSE ''  " +
                    "\n\t\tEND, MC2Name,  " +
                    "\n\tCASE  " +
                    "\n\t\tWHEN LEN(MC3Name)>1 THEN ' & '  " +
                    "\n\t\tELSE ''  " +
                    "\n\tEND, MC3Name),'') AS MCName, " +
                    "\nTbNGPsc.RMCode, TbRMMaster.ItemName, (COALESCE(TbNGPsc.TotalQty,0)+COALESCE(TbNGSet.TotalQty,0)) AS TotalQty, COALESCE(TbNGStatus.ReqStatus, 1) AS ReqStatus, TbNGStatus.RegDate FROM " +
                    "\n\n\n(SELECT PosCNo, RMCode, " +
                    "\n\tCASE " +
                    "\n\t\tWHEN Unit='Gram (g)' THEN ((TotalQtyPcs/1000) / (COALESCE(T2.R1OrNetW, 1)) * (COALESCE (MOQ,1))) " +
                    "\n\t\tELSE TotalQtyPcs " +
                    "\n\tEND AS TotalQty, TotalQtyPcs, COALESCE(T2.R1OrNetW, 1) AS R1OrNetW, COALESCE (MOQ,1) AS MOQ FROM " +
                    "\n(SELECT PosCNo, RMCode, SUM(QTY) AS TotalQtyPcs, Unit FROM tbNGInprocess WHERE NGType='NGPcs' GROUP BY PosCNo, RMCode, Unit) T1 " +
                    "\nLEFT JOIN (SELECT * FROM tbSDMstUncountMat) T2 " +
                    "\nON T1.RMCode=T2.Code " +
                    "\n) TbNGPsc " +
                    "\n\n\nLEFT JOIN " +
                    "\n(SELECT T1.PosCNo, RMCode, (TotalQtySet * SemiQtyOfFG * T3.LowQty) AS TotalQty, TotalQtySet, SemiQtyOfFG, T3.LowQty FROM " +
                    "\n(SELECT PosCNo, RMCode, SUM(QTY) AS TotalQtySet FROM tbNGInprocess WHERE NGType='NGSet' GROUP BY PosCNo, RMCode) T1 " +
                    "\nINNER JOIN (SELECT * FROM tbPOSDetailofMC) T2 " +
                    "\nON T1.PosCNo=T2.PosCNo " +
                    "\nLEFT JOIN (SELECT * FROM MstBOM) T3 " +
                    "\nON T2.WIPCode=T3.UpItemCode AND T1.RMCode=T3.LowItemCode " +
                    "\n) TbNGSet " +
                    "\nON TbNGPsc.PosCNo=TbNGSet.PosCNo AND TbNGPsc.RMCode=TbNGSet.RMCode " +
                    "\n\n\nLEFT JOIN " +
                    "\n(SELECT T1.PosCNo, RegDate, ReqStatus FROM " +
                    "\n(SELECT PosCNo, MIN(RegDate) AS RegDate FROM tbNGInprocess GROUP BY PosCNo) T1 " +
                    "\nLEFT JOIN (SELECT PosCNo, ReqStatus FROM tbNGInprocess WHERE ReqStatus=0 GROUP BY PosCNo, ReqStatus) T2 " +
                    "\nON T1.PosCNo=T2.PosCNo " +
                    "\n) TbNGStatus " +
                    "\nON TbNGPsc.PosCNo=TBNGStatus.PosCNo " +
                    "\n\n\nLEFT JOIN (SELECT * FROM tbPOSDetailofMC) TbPOSDetails " +
                    "\nON TbNGPsc.PosCNo=TbPOSDetails.PosCNo " +
                    "\nLEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') TbRMMaster " +
                    "\nON TbNGPsc.RMCode=TbRMMaster.ItemCode ";
                SQLQuery += SQLConds + " ORDER BY TbNGPsc.PosCNo ASC ";
                //Console.WriteLine(SQLQuery);
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtSearchResult);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

            //TAKE Unit Price of RM
            if (ErrorText.Trim() == "" && dtSearchResult.Rows.Count>0)
            {
                string RMCodeIN = "";
                foreach (DataRow row in dtSearchResult.Rows)
                {
                    if (RMCodeIN.Contains(row["RMCode"].ToString()) == false)
                    {
                        if (RMCodeIN.Trim() == "")
                        {
                            RMCodeIN = "'" + row["RMCode"].ToString() + "'";
                        }
                        else
                        {
                            RMCodeIN += ", '" + row["RMCode"].ToString() + "'";
                        }
                    }
                }
                DataTable dtOBS = new DataTable();
                try
                {
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT T1.ItemCode, ItemName, T3.EffDate , COALESCE(T2.UnitPrice,0) AS UnitPrice FROM " +
                        "\n(SELECT ItemCode, ItemName FROM mstitem WHERE DelFlag=0 AND ItemType=2) T1 " +
                        "\nLEFT JOIN (SELECT ItemCode, UnitPrice, EffDate FROM mstpurchaseprice WHERE DelFlag=0) T2 " +
                        "\nON T1.ItemCode=T2.ItemCode " +
                        "\nINNER JOIN (SELECT ItemCode, MAX(EffDate) AS EffDate FROM mstpurchaseprice GROUP BY ItemCode) T3 " +
                        "\nON T2.ItemCode=T3.ItemCode AND T2.EffDate=T3.EffDate " +
                        "\nWHERE T1.ItemCode IN (" + RMCodeIN + ") " +
                        "\nORDER BY ItemCode ASC ", cnnOBS.conOBS);
                    sda.Fill(dtOBS);
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                }

                //Add to dtSearchResult
                dtSearchResult.Columns.Add("UP");
                dtSearchResult.Columns.Add("Amount");
                foreach (DataRow row in dtSearchResult.Rows)
                {
                    double UP = 0;
                    double Amount = 0;
                    foreach (DataRow rowOBS in dtOBS.Rows)
                    {
                        if (row["RMCode"].ToString() == rowOBS["ItemCode"].ToString())
                        {
                            UP = Convert.ToDouble(rowOBS["UnitPrice"]);
                            UP = Convert.ToDouble(UP.ToString("N4"));
                            Amount = (Convert.ToDouble(row["TotalQty"].ToString())) * UP;
                            Amount = Convert.ToDouble(Amount.ToString("N3"));
                            break;
                        }
                    }
                    row["UP"] = UP;
                    row["Amount"] = Amount;
                }
                dtSearchResult.AcceptChanges();

            }

            //Add to DGV 
            if (ErrorText.Trim() == "" && dtSearchResult.Rows.Count > 0)
            {
                foreach (DataRow row in dtSearchResult.Rows)
                {
                    int CheckMCName = 0;
                    if (CboMCName.Text.Trim() != "")
                    {
                        if (row["MCName"].ToString().Trim() != "" && row["MCName"].ToString().ToUpper().Contains(CboMCName.Text.ToString().ToUpper()) == true)
                        {
                            CheckMCName++;
                        }
                    }
                    else
                    {
                        CheckMCName++;
                    }

                    if (CheckMCName > 0)
                    {
                        dgvSearchResult.Rows.Add();
                        dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["ChkForPrint"].Value = false;
                        DateTime DelDate = Convert.ToDateTime(row["PosPDelDate"]);
                        dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["PosPDelDate"].Value = DelDate;
                        string POSCNo = row["PosCNo"].ToString();
                        dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["PosCNo"].Value = POSCNo;
                        string MCName = row["MCName"].ToString();
                        dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["MCName"].Value = MCName;
                        string Code = row["RMCode"].ToString();
                        dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["RMCode"].Value = Code;
                        string Name = row["ItemName"].ToString();
                        dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["RMName"].Value = Name;

                        double Qty = Convert.ToDouble(row["TotalQty"]);
                        Qty = Convert.ToDouble(Qty.ToString("N4"));
                        dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Qty"].Value = Qty;

                        string Status = "NOT YET";
                        if (row["ReqStatus"].ToString() == "1")
                        {
                            Status = "OK";
                        }
                        dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["PrintStatus"].Value = Status;

                        DateTime RegDate = Convert.ToDateTime(row["RegDate"]);
                        dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["RegDate"].Value = RegDate;

                        double UP = Convert.ToDouble(row["UP"]);
                        dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["UP"].Value = UP;

                        double Amount = Convert.ToDouble(row["Amount"]); 
                        dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Amount"].Value = Amount;

                    }
                }
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                dgvSearchResult.ClearSelection();
                dgvSearchResult.CurrentCell = null;
                LbStatus.Text = "រកឃើញទិន្នន័យ " + dgvSearchResult.Rows.Count.ToString("N0");
                LbStatus.Refresh();
            }
            else
            {

                LbStatus.Text = "មានបញ្ហា!";
                LbStatus.Refresh();
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvSearchResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSearchResult.Columns[e.ColumnIndex].Name == "ChkForPrint")
            {
                if (e.RowIndex > -1)
                {
                    if (dgvSearchResult.Rows[e.RowIndex].Cells["PrintStatus"].Value.ToString() != "OK")
                    {
                        bool ChkPrint = false;
                        if (dgvSearchResult.Rows[e.RowIndex].Cells["ChkForPrint"].Value.ToString().ToUpper() == "FALSE")
                        {
                            ChkPrint = true;
                            //dgvSearchResult.Rows[e.RowIndex].Cells["ChkForPrint"].Value = true;
                        }
                        foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
                        {
                            if (dgvRow.Cells["PosCNo"].Value.ToString() == dgvSearchResult.Rows[e.RowIndex].Cells["PosCNo"].Value.ToString() &&
                                dgvRow.Cells["PrintStatus"].Value.ToString() != "OK")
                            {
                                dgvRow.Cells["ChkForPrint"].Value = ChkPrint;
                            }
                        }
                        dgvSearchResult.Refresh();
                        dgvSearchResult.ClearSelection();
                    }
                    else
                    {
                        MessageBox.Show("ទិន្នន័យនេះព្រីនចេញរួចរាល់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }        
        private void DgvSearchResult_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (dgvSearchResult.Columns[e.ColumnIndex].Name != "ChkForPrint")
                {
                    NGInprocessSearchDetailsForm Nisdf = new NGInprocessSearchDetailsForm(this);
                    Nisdf.ShowDialog();
                }
            }
        }
        private void NGInprocessSearchForm_Shown(object sender, EventArgs e)
        {
            CboStatus.SelectedIndex = 0;
            ErrorText = "";
            Cursor = Cursors.WaitCursor;

            //Take MC Name
            DataTable dtMCName = new DataTable();
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbMasterMCType ORDER BY MCType, MCName ASC", cnn.con);
                sda.Fill(dtMCName);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

            foreach (DataRow row in dtMCName.Rows)
            {
                CboMCName.Items.Add(row["MCName"].ToString());
            }

            //CboMCName.Text = "MS-01 (E)";

            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
            {
                MessageBox.Show("មានបញ្ហា!\n"+ErrorText, "Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }

        //Method
        private void CalcForPrint()
        {
            ErrorText = "";
            fName = "";
            Cursor = Cursors.WaitCursor;
            string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\NGRequest";

            //Take POSNo
            DataTable dtPOSCannotSave = new DataTable();
            dtPOSCannotSave.Columns.Add("POSNo");

            string POSNoIN = "";
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                if (dgvRow.Cells["ChkForPrint"].Value.ToString().ToLower() == "true")
                {
                    if (POSNoIN.Contains(dgvRow.Cells["PosCNo"].Value.ToString()) == false)
                    {
                        if (POSNoIN.Trim() == "")
                        {
                            POSNoIN = dgvRow.Cells["PosCNo"].Value.ToString();
                        }
                        else
                        {
                            POSNoIN += "," + dgvRow.Cells["PosCNo"].Value.ToString();
                        }
                    }
                }
            }
            //Console.WriteLine(POSNoIN);
            string[] ArrPOSNo = POSNoIN.Split(',');
            
            //Take NG SysNo
            DateTime RegDate = DateTime.Now;
            string RegBy = MenuFormV2.UserForNextForm;
            string TransNo = "NG0000000001";
            try
            {
                cnn.con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT SysNo FROM tbSDMCAllTransaction " +
                                                                                    "WHERE SysNo=(SELECT MAX(SysNo) FROM tbSDMCAllTransaction WHERE Funct =6) Group By SysNo", cnn.con);
                DataTable dtTransNo = new DataTable();
                da.Fill(dtTransNo);
                if (dtTransNo.Rows.Count > 0)
                {
                    string LastTransNo = dtTransNo.Rows[0][0].ToString();
                    double NextTransNo = Convert.ToDouble(LastTransNo.Substring(LastTransNo.Length - 10)) + 1;
                    TransNo = "NG" + NextTransNo.ToString("0000000000");
                }
            }
            catch (Exception ex)
            {
                ErrorText = "Taking TransNo : " + ex.Message;
            }
            cnn.con.Close();

            //Calc and Cutting Stock
            if (ErrorText.Trim() == "")
            {
                ClearDtForPrintExcel();
                ClearDtNotEnough();
                for (int i = 0; i < ArrPOSNo.Length; i++)
                {
                    string ErrorText2 = "";
                    ClearDtCuttingStock();
                    string POSNo = ArrPOSNo[i].ToString().Trim();
                    int FoundStockNotEnough = 0;

                    //Taking NG Qty & Stock Remain For Cutting
                    DataTable dtNGQtyTemp = new DataTable();
                    DataTable dtPOSAndMCIN = new DataTable();
                    DataTable dtPOSStock = new DataTable();
                    DataTable dtOtherStock = new DataTable();
                    string RMCodeIN = "";
                    try
                    {
                        cnn.con.Open();
                        string SQLQuery = "SELECT PosCNo, RMCode, ROUND(SUM(TotalQty),0) AS Qty FROM " +
                                "\n( " +
                                "\n \n\tSELECT tbNGHeader.*, tbNGDetailsPcs.RMCode, (tbNGDetailsPcs.TotalQty+ tbNGDetailsSET.TotalQty) AS TotalQty  FROM " +
                                "\n\t(SELECT MCSeqNo, PosCNo FROM tbNGInprocess WHERE ReqStatus=0 GROUP BY MCSeqNo, PosCNo) tbNGHeader " +
                                "\n \n \n\tINNER JOIN  " +
                                "\n\t(SELECT MCSeqNo, PosCNo, RMCode,  " +
                                "\n\tCASE  " +
                                "\n\t\tWHEN Unit='Gram (g)' THEN ROUND(Qty/1000/R1OrNetW*MOQ, 2) " +
                                "\n\t\tELSE Qty " +
                                "\n\tEND AS TotalQty FROM " +
                                "\n\t(SELECT MCSeqNo, PosCNo, RMCode, Qty, Unit FROM tbNGInprocess WHERE NGType='NGPcs' AND ReqStatus=0) T1 " +
                                "\n\tLEFT JOIN " +
                                "\n\t(SELECT Code, R1OrNetW, MOQ FROM tbSDMstUncountMat) T2 " +
                                "\n\tON T1.RMCode=T2.Code " +
                                "\n\t) tbNGDetailsPcs " +
                                "\n\tON tbNGHeader.MCSeqNo=tbNGDetailsPcs.MCSeqNo AND tbNGHeader.PosCNo=tbNGDetailsPcs.PosCNo " +
                                "\n\t \n \n\tINNER JOIN  " +
                                "\n\t(SELECT MCSeqNo, T1.PosCNo, RMCode,  " +
                                "\n\tROUND(Qty * LowQty*SemiQtyOfFG,2) AS TotalQty FROM " +
                                "\n\t(SELECT MCSeqNo, PosCNo, RMCode, Qty FROM tbNGInprocess WHERE NGType='NGSet' AND ReqStatus=0) T1  " +
                                "\n\tLEFT JOIN  " +
                                "\n\t(SELECT PosCNo, WIPCode FROM tbPOSDetailofMC) T2  " +
                                "\n\tON T1.PosCNo=T2.PosCNo  " +
                                "\n\tINNER JOIN  " +
                                "\n\t(SELECT * FROM MstBOM) T3  " +
                                "\n\tON T2.WIPCode=T3.UpItemCode AND T1.RMCode=T3.LowItemCode) tbNGDetailsSET " +
                                "\n\tON tbNGDetailsPcs.MCSeqNo=tbNGDetailsSET.MCSeqNo AND tbNGDetailsPcs.PosCNo=tbNGDetailsSET.PosCNo AND tbNGDetailsPcs.RMCode=tbNGDetailsSET.RMCode " +
                                "\n WHERE ROUND((tbNGDetailsPcs.TotalQty+ tbNGDetailsSET.TotalQty),0)>0 AND tbNGHeader.PosCNo = '"+POSNo+"' " +
                                "\n \n) TbFinal " +
                                "\nGROUP BY PosCNo, RMCode " +
                                "\nORDER BY PosCNo ASC, RMCode ASC ";

                        //Console.WriteLine("--dtNGQtyTemp\n" + SQLQuery);
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        sda.Fill(dtNGQtyTemp);

                        //Take for Update ReqStatus 
                        SQLQuery = "SELECT MCSeqNo, PosCNo FROM tbNGInprocess WHERE ReqStatus=0 AND PosCNo = '" + POSNo + "' GROUP BY MCSeqNo, PosCNo";
                        //Console.WriteLine("\n\n--dtPOSAndMCIN\n" + SQLQuery);
                        sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        sda.Fill(dtPOSAndMCIN);

                        //Taking Stock
                        if (dtNGQtyTemp.Rows.Count > 0)
                        {
                            //POS Stock
                            SQLQuery = "SELECT * FROM " +
                                        "\n(SELECT TbTst.Code, TbTst.POSNo AS LotNo, StockValue, " +
                                        "\nCASE " +
                                        "\n\tWHEN NOT TRIM(TbTst.POSNo) = '' THEN " +
                                        "\n\t\tCASE " +
                                        "\n\t\t\tWHEN LEFT(TbTst.POSNo,2) = 'SD' THEN TbAllo.POSNo " +
                                        "\n\t\t\tWHEN RIGHT(TbTst.POSNo,6) = 'NGRate' THEN LEFT(TbTst.POSNo,10) " +
                                        "\n\t\t\tELSE TbTst.POSNo " +
                                        "\n\t\tEND " +
                                        "\n\tELSE '' " +
                                        "\nEND AS POSNo, TbRegDate.RegDate FROM " +
                                        "\n(SELECT Code, POSNo, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='MC1' GROUP BY Code, POSNo) TbTst " +
                                        "\nLEFT JOIN (SELECT * FROM tbSDAllocateStock) TbAllo ON TbTst.POSNo=TbAllo.SysNo " +
                                        "\nINNER JOIN (SELECT Code, POSNo, MIN(RegDate) AS RegDate FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='MC1' AND Funct=2 AND NOT TRIM(POSNo) = '' GROUP BY Code, POSNo) TbRegDate ON TbTst.Code=TbRegDate.Code AND TbTst.POSNo=TbRegDate.POSNo) X " +
                                        "\nWHERE StockValue>0 AND POSNo = '" + POSNo + "' " +
                                        "\nORDER BY Code ASC, RegDate ASC ";
                            //Console.WriteLine("\n\n--dtPOSStock\n" + SQLQuery);
                            sda = new SqlDataAdapter(SQLQuery, cnn.con);
                            sda.Fill(dtPOSStock);

                            //Other Stock
                            foreach (DataRow row in dtNGQtyTemp.Rows)
                            {
                                if (RMCodeIN.Contains(row["RMCode"].ToString()) == false)
                                {
                                    if (RMCodeIN.Trim() == "")
                                    {
                                        RMCodeIN = "'" + row["RMCode"].ToString() + "'";
                                    }
                                    else
                                    {
                                        RMCodeIN += ", '" + row["RMCode"].ToString() + "'";
                                    }
                                }
                            }
                            SQLQuery = "SELECT Code, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction " +
                                                            "\nWHERE CancelStatus=0 AND LocCode='MC1' AND POSNo = '' AND Code IN (" + RMCodeIN + ") " +
                                                            "\nGROUP BY Code";
                            //Console.WriteLine("\n--dtOtherStock\n" + SQLQuery);
                            sda = new SqlDataAdapter(SQLQuery, cnn.con);
                            sda.Fill(dtOtherStock);

                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorText2 = ex.Message;
                    }
                    cnn.con.Close();

                    //Calc NG Qty vs Stock Remain >> Enough or Not
                    if (ErrorText2.Trim() == "")
                    {
                        //Add Detail to dtNGQtyTemp From OBS
                        if (dtNGQtyTemp.Rows.Count > 0)
                        {
                            DataTable dtOBSRM = new DataTable();
                            try
                            {
                                cnnOBS.conOBS.Open();
                                string SQLQuery = "SELECT T1.ItemCode AS RMCode, ItemName, MatTypeName, MatCalcFlag, RESV1 AS Maker, COALESCE(T3.UnitPrice, 0) AS UnitPrice FROM " +
                                     "\n(SELECT * FROM mstitem WHERE DelFlag=0 AND ItemType=2) T1 " +
                                     "\nLEFT JOIN (SELECT * FROM MstMatType) T2 " +
                                     "\nON T1.MatTypeCode=T2.MatTypeCode " +
                                     "\nLEFT JOIN (SELECT ItemCode, EffDate, UnitPrice FROM mstpurchaseprice) T3 " +
                                     "\nON T1.ItemCode=T3.ItemCode " +
                                     "\nINNER JOIN (SELECT ItemCode, MAX(EffDate) AS EffDate FROM mstpurchaseprice GROUP BY ItemCode) T4 " +
                                     "\nON T3.ItemCode=T4.ItemCode AND T3.EffDate=T4.EffDate " +
                                     "\nWHERE T1.ItemCode IN (" + RMCodeIN + ") " +
                                     "\nORDER BY RMCode ASC ";
                                //Console.WriteLine(SQLQuery);
                                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                                sda.Fill(dtOBSRM);
                            }
                            catch (Exception ex)
                            {
                                ErrorText2 = "Taking OBS Mst : " + ex.Message;
                            }
                            cnnOBS.conOBS.Close();
                            dtOBSRM.Columns.Add("Qty");
                            foreach (DataRow row in dtNGQtyTemp.Rows)
                            {
                                string RMCode = row["RMCode"].ToString();
                                double Qty = Convert.ToDouble(row["Qty"]);
                                int Found = 0;
                                foreach (DataRow rowOBS in dtOBSRM.Rows)
                                {
                                    if (RMCode == rowOBS["RMCode"].ToString())
                                    {
                                        Found++;
                                        rowOBS["Qty"] = Qty;
                                        break;
                                    }
                                }
                                if (Found == 0)
                                {
                                    dtOBSRM.Rows.Add();
                                    dtOBSRM.Rows[dtOBSRM.Rows.Count - 1]["RMCode"] = RMCode;
                                    dtOBSRM.Rows[dtOBSRM.Rows.Count - 1]["MatCalcFlag"] = 0;
                                    dtOBSRM.Rows[dtOBSRM.Rows.Count - 1]["UnitPrice"] = 0;
                                    dtOBSRM.Rows[dtOBSRM.Rows.Count - 1]["Qty"] = Qty;
                                }
                            }
                            //Copy dtOBSRM > dtNGQtyTemp
                            dtNGQtyTemp = new DataTable();
                            dtNGQtyTemp = dtOBSRM.Copy();
                            dtNGQtyTemp.AcceptChanges();
                        }

                        //For Console dtNGQtyTemp
                        /*
                        string ColumnTex = "";
                        foreach (DataColumn col in dtNGQtyTemp.Columns)
                        {
                            ColumnTex += col.ColumnName + "\t";
                        }
                        Console.WriteLine(ColumnTex);
                        foreach (DataRow row in dtNGQtyTemp.Rows)
                        {
                            string Msg = "";
                            foreach (DataColumn col in dtNGQtyTemp.Columns)
                            {
                                Msg += row[col.ColumnName] + "\t";
                            }
                            Console.WriteLine(Msg);
                        }

                        */

                        //Checking Stock Enough or Not
                        if (ErrorText2.Trim() == "")
                        {
                            string NotEnoughStock = "";
                            foreach (DataRow row in dtNGQtyTemp.Rows)
                            {
                                string RMCode = row["RMCode"].ToString();
                                string RMName = row["ItemName"].ToString();
                                double NGQty = Convert.ToDouble(row["Qty"]);
                                double StockOutRemain = NGQty;

                                //POS
                                foreach (DataRow rowPOS in dtPOSStock.Rows)
                                {
                                    if (StockOutRemain > 0)
                                    {
                                        if (RMCode == rowPOS["Code"].ToString() && Convert.ToDouble(rowPOS["StockValue"].ToString()) > 0)
                                        {
                                            string LotOrPOSNo = rowPOS["LotNo"].ToString();
                                            double StockRemain = Convert.ToDouble(rowPOS["StockValue"].ToString());
                                            if (StockRemain >= StockOutRemain)
                                            {
                                                dtCuttingStock.Rows.Add();
                                                dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["Code"] = RMCode;
                                                dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["RMName"] = RMName;
                                                dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["LotOrPOSNo"] = LotOrPOSNo;
                                                dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["Qty"] = StockOutRemain;
                                                StockOutRemain = 0;
                                                break;
                                            }
                                            else
                                            {
                                                dtCuttingStock.Rows.Add();
                                                dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["Code"] = RMCode;
                                                dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["RMName"] = RMName;
                                                dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["LotOrPOSNo"] = LotOrPOSNo;
                                                dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["Qty"] = StockRemain;
                                                StockOutRemain -= StockRemain;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                //Other
                                foreach (DataRow rowOther in dtOtherStock.Rows)
                                {
                                    if (StockOutRemain > 0)
                                    {
                                        if (RMCode == rowOther["Code"].ToString() && Convert.ToDouble(rowOther["StockValue"].ToString()) > 0)
                                        {
                                            string LotOrPOSNo = "";
                                            double StockRemain = Convert.ToDouble(rowOther["StockValue"].ToString());
                                            if (StockRemain >= StockOutRemain)
                                            {
                                                dtCuttingStock.Rows.Add();
                                                dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["Code"] = RMCode;
                                                dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["RMName"] = RMName;
                                                dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["LotOrPOSNo"] = LotOrPOSNo;
                                                dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["Qty"] = StockOutRemain;
                                                StockOutRemain = 0;
                                                break;
                                            }
                                            else
                                            {
                                                dtCuttingStock.Rows.Add();
                                                dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["Code"] = RMCode;
                                                dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["RMName"] = RMName;
                                                dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["LotOrPOSNo"] = LotOrPOSNo;
                                                dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["Qty"] = StockRemain;
                                                StockOutRemain -= StockRemain;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                //If Not Enough
                                if (StockOutRemain > 0)
                                {
                                    dtNotEnough.Rows.Add();
                                    dtNotEnough.Rows[dtNotEnough.Rows.Count - 1]["POSNo"] = POSNo;
                                    dtNotEnough.Rows[dtNotEnough.Rows.Count - 1]["ItemCode"] = RMCode;
                                    dtNotEnough.Rows[dtNotEnough.Rows.Count - 1]["ItemName"] = RMName;
                                    dtNotEnough.Rows[dtNotEnough.Rows.Count - 1]["Qty"] = NGQty;
                                    dtNotEnough.AcceptChanges();

                                    if (NotEnoughStock.Trim() == "")
                                    {
                                        NotEnoughStock = "Stock not enough : " + RMCode + "|" + RMName;
                                    }
                                    else
                                    {
                                        NotEnoughStock += ", " + RMCode + "|" + RMName;
                                    }

                                    FoundStockNotEnough++;
                                }
                            }

                            //If Not Enough
                            if (NotEnoughStock.Trim() != "")
                            {
                                ErrorText2 = NotEnoughStock;
                            }
                        }

                        //If Enough >> Cutting Stock & Update NG Inprocess Print Status
                        if (ErrorText2.Trim() == "" && FoundStockNotEnough == 0)
                        {
                            //Console dtCutting Stock
                            /*

                            string ColumnTex = "";
                            foreach (DataColumn col in dtCuttingStock.Columns)
                            {
                                ColumnTex += col.ColumnName + "\t";
                            }
                            Console.WriteLine(ColumnTex+"POS");
                            foreach (DataRow row in dtCuttingStock.Rows)
                            {
                                string Msg = "";
                                foreach (DataColumn col in dtCuttingStock.Columns)
                                {
                                    Msg += row[col.ColumnName] + "\t";
                                }
                                Console.WriteLine(Msg+POSNo);
                            }

                            */

                            //Update & Cutting
                            try
                            {
                                cnn.con.Open();
                                //Update NG Inprocess Print Status
                                foreach (DataRow row in dtPOSAndMCIN.Rows)
                                {
                                    string MCNo = row["MCSeqNo"].ToString();
                                    string PosCNo = row["PosCNo"].ToString();

                                    string query = "UPDATE tbNGInprocess SET " +
                                                        "TransactionNo='" + TransNo + "'," +
                                                        "ReqStatus=1 " +
                                                        "WHERE ReqStatus=0 AND MCSeqNo = '" + MCNo + "' AND PosCNo='" + PosCNo + "';";
                                    SqlCommand cmd = new SqlCommand(query, cnn.con);
                                    cmd.ExecuteNonQuery();
                                }

                                //Add Stock Cutting >> tbSDMCAllTransaction
                                foreach (DataRow row in dtCuttingStock.Rows)
                                {
                                    SqlCommand cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction (SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                        "VALUES (@Sn, @Fc, @Lc, @POSN, @Cd, @Rmd, @Rqty, @Tqty, @SV, @Rd, @Rb, @Cs, @Rm)", cnn.con);
                                    cmd.Parameters.AddWithValue("@Sn", TransNo);
                                    cmd.Parameters.AddWithValue("@Fc", 6);
                                    cmd.Parameters.AddWithValue("@Lc", "MC1");
                                    cmd.Parameters.AddWithValue("@POSN", row["LotOrPOSNo"].ToString());
                                    cmd.Parameters.AddWithValue("@Cd", row["Code"].ToString());
                                    cmd.Parameters.AddWithValue("@Rmd", row["RMName"].ToString());
                                    cmd.Parameters.AddWithValue("@Rqty", 0);
                                    cmd.Parameters.AddWithValue("@Tqty", Convert.ToDouble(row["Qty"].ToString()));
                                    cmd.Parameters.AddWithValue("@SV", Convert.ToDouble(row["Qty"].ToString()) * (-1));
                                    cmd.Parameters.AddWithValue("@Rd", RegDate);
                                    cmd.Parameters.AddWithValue("@Rb", RegBy);
                                    cmd.Parameters.AddWithValue("@Cs", 0);
                                    cmd.Parameters.AddWithValue("@Rm", POSNo);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                ErrorText2 = ex.Message;
                            }
                            cnn.con.Close();

                            //Add to dtForPrintExcel
                            if (ErrorText2.Trim() == "")
                            {
                                foreach (DataRow row in dtNGQtyTemp.Rows)
                                {
                                    int Found = 0;
                                    foreach (DataRow rowFPE in dtForPrintExcel.Rows)
                                    {
                                        if (row["RMCode"].ToString() == rowFPE["RMCode"].ToString())
                                        {
                                            Found++;
                                            rowFPE["Qty"] = Convert.ToDouble(rowFPE["Qty"]) + Convert.ToDouble(row["Qty"]);
                                            break;
                                        }
                                    }
                                    if (Found == 0)
                                    {
                                        dtForPrintExcel.Rows.Add();
                                        dtForPrintExcel.Rows[dtForPrintExcel.Rows.Count - 1]["RMCode"] = row["RMCode"].ToString();
                                        dtForPrintExcel.Rows[dtForPrintExcel.Rows.Count - 1]["ItemName"] = row["ItemName"].ToString();
                                        dtForPrintExcel.Rows[dtForPrintExcel.Rows.Count - 1]["MatTypeName"] = row["MatTypeName"].ToString();
                                        dtForPrintExcel.Rows[dtForPrintExcel.Rows.Count - 1]["MatCalcFlag"] = row["MatCalcFlag"].ToString();
                                        dtForPrintExcel.Rows[dtForPrintExcel.Rows.Count - 1]["Maker"] = row["Maker"].ToString();
                                        dtForPrintExcel.Rows[dtForPrintExcel.Rows.Count - 1]["UnitPrice"] = Convert.ToDouble(row["UnitPrice"].ToString());
                                        dtForPrintExcel.Rows[dtForPrintExcel.Rows.Count - 1]["Qty"] = Convert.ToDouble(row["Qty"].ToString());
                                    }
                                    dtForPrintExcel.AcceptChanges();
                                }
                                dtForPrintExcel.AcceptChanges();
                            }
                        }
                    }

                    //if Error
                    if (ErrorText2.Trim() != "")
                    {
                        dtPOSCannotSave.Rows.Add(POSNo);
                        if (ErrorText.Trim() == "")
                        {
                            ErrorText = " - " + POSNo + " : " + ErrorText2;
                        }
                        else
                        {
                            ErrorText += "\n - " + POSNo + " : " + ErrorText2;
                        }
                    }

                }

                //Print Out Excel Req document
                if (dtForPrintExcel.Rows.Count > 0)
                {
                    //Sort dtForPrintExcel
                    DataView dataView = dtForPrintExcel.DefaultView;//datatable to dataview
                    dataView.Sort = "MatCalcFlag ASC, RMCode ASC";//string that contains the column name  followed by "ASC" (ascending) or "DESC" (descending)
                    dtForPrintExcel = dataView.ToTable();
                    dtForPrintExcel.AcceptChanges();
                                        
                    /*
                     
                    string ColumnTex = "";
                    foreach (DataColumn col in dtForPrintExcel.Columns)
                    {
                        ColumnTex += col.ColumnName + "\t";
                    }
                    Console.WriteLine(ColumnTex);
                    foreach (DataRow row in dtForPrintExcel.Rows)
                    {
                        string Msg = "";
                        foreach (DataColumn col in dtForPrintExcel.Columns)
                        {
                            Msg += row[col.ColumnName] + "\t";
                        }
                        Console.WriteLine(Msg);
                    }

                    */

                    //Print to Excel
                    var CDirectory = Environment.CurrentDirectory;
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\NGReq_Template.xlsx", Editable: true);
                    try
                    {             
                        //Write to Countable
                        Excel.Worksheet worksheetCountable = (Excel.Worksheet)xlWorkBook.Sheets["Countable"];
                        //Header
                        worksheetCountable.Cells[4, 10] = RegDate;
                        worksheetCountable.Cells[1, 9] = TransNo;
                        worksheetCountable.Cells[5, 10] = RegBy;
                        int WriteItems = 0;
                        foreach (DataRow row in dtForPrintExcel.Rows)
                        {
                            if (row["MatCalcFlag"].ToString() == "0")
                            {
                                WriteItems = WriteItems + 1;
                            }
                        }
                        if (WriteItems > 1)
                        {
                            //Insert More Rows
                            // Define the range that you want to copy.
                            Excel.Range sourceRange = worksheetCountable.Range["A14:M14"];
                            // Perform the copy operation.
                            sourceRange.Copy(Type.Missing);
                            // Specify the range where you want to insert the copied rows.
                            Excel.Range destinationRange = worksheetCountable.Range["A15:A" + (14 + WriteItems - 1)];
                            // Insert the copied cells and shift the existing cells down.
                            destinationRange.Insert(Excel.XlInsertShiftDirection.xlShiftDown, sourceRange.Copy());
                            worksheetCountable.Range["15:" + (14 + WriteItems - 1)].RowHeight = 18;

                            //Write Data to cells
                            int WriteCellsIndex = 14;
                            foreach (DataRow row in dtForPrintExcel.Rows)
                            {
                                if (row["MatCalcFlag"].ToString() == "0")
                                {
                                    worksheetCountable.Cells[WriteCellsIndex, 1] = (WriteCellsIndex - 14) + 1;
                                    worksheetCountable.Cells[WriteCellsIndex, 4] = row["RMCode"].ToString();
                                    worksheetCountable.Cells[WriteCellsIndex, 5] = row["ItemName"].ToString();
                                    worksheetCountable.Cells[WriteCellsIndex, 7] = row["Maker"].ToString();
                                    worksheetCountable.Cells[WriteCellsIndex, 8] = row["MatTypeName"].ToString();
                                    worksheetCountable.Cells[WriteCellsIndex, 9] = row["Qty"].ToString();
                                    worksheetCountable.Cells[WriteCellsIndex, 10] = row["UnitPrice"].ToString();
                                    worksheetCountable.Cells[WriteCellsIndex, 11] = "=I" + WriteCellsIndex + "*J" + WriteCellsIndex;

                                    WriteCellsIndex = WriteCellsIndex + 1;
                                }
                            }
                            //Subtotal
                            worksheetCountable.Cells[WriteCellsIndex, 11] = "=SUM(K14:K" + (WriteCellsIndex - 1) + ")";
                            worksheetCountable.Range[WriteCellsIndex + ":" + WriteCellsIndex].RowHeight = 30;
                        }
                        else
                        {
                            foreach (DataRow row in dtForPrintExcel.Rows)
                            {
                                if (row["MatCalcFlag"].ToString() == "0")
                                {
                                    worksheetCountable.Cells[14, 1] = 1;
                                    worksheetCountable.Cells[14, 4] = row["RMCode"].ToString();
                                    worksheetCountable.Cells[14, 5] = row["ItemName"].ToString();
                                    worksheetCountable.Cells[14, 7] = row["Maker"].ToString();
                                    worksheetCountable.Cells[14, 8] = row["MatTypeName"].ToString();
                                    worksheetCountable.Cells[14, 9] = row["Qty"].ToString();
                                    worksheetCountable.Cells[14, 10] = row["UnitPrice"].ToString();
                                    worksheetCountable.Cells[14, 11] = "=I14*J14";

                                }
                            }
                        }

                        //Write to Uncountable
                        Excel.Worksheet worksheetUncountable = (Excel.Worksheet)xlWorkBook.Sheets["Uncountable"];
                        //Header
                        worksheetUncountable.Cells[4, 10] = RegDate;
                        worksheetUncountable.Cells[1, 9] = TransNo;
                        worksheetUncountable.Cells[5, 10] = RegBy;
                        WriteItems = 0;
                        foreach (DataRow row in dtForPrintExcel.Rows)
                        {
                            if (row["MatCalcFlag"].ToString() != "0")
                            {
                                WriteItems = WriteItems + 1;
                            }
                        }
                        if (WriteItems > 1)
                        {
                            //Insert More Rows
                            // Define the range that you want to copy.
                            Excel.Range sourceRange = worksheetUncountable.Range["A14:M14"];
                            // Perform the copy operation.
                            sourceRange.Copy(Type.Missing);
                            // Specify the range where you want to insert the copied rows.
                            Excel.Range destinationRange = worksheetUncountable.Range["A15:A" + (14 + WriteItems - 1)];
                            // Insert the copied cells and shift the existing cells down.
                            destinationRange.Insert(Excel.XlInsertShiftDirection.xlShiftDown, sourceRange.Copy());
                            worksheetUncountable.Range["15:" + (14 + WriteItems - 1)].RowHeight = 18;

                            //Write Data to cells
                            int WriteCellsIndex = 14;
                            foreach (DataRow row in dtForPrintExcel.Rows)
                            {
                                if (row["MatCalcFlag"].ToString() != "0")
                                {
                                    worksheetUncountable.Cells[WriteCellsIndex, 1] = (WriteCellsIndex - 14) + 1;
                                    worksheetUncountable.Cells[WriteCellsIndex, 4] = row["RMCode"].ToString();
                                    worksheetUncountable.Cells[WriteCellsIndex, 5] = row["ItemName"].ToString();
                                    worksheetUncountable.Cells[WriteCellsIndex, 7] = row["Maker"].ToString();
                                    worksheetUncountable.Cells[WriteCellsIndex, 8] = row["MatTypeName"].ToString();
                                    worksheetUncountable.Cells[WriteCellsIndex, 9] = row["Qty"].ToString();
                                    worksheetUncountable.Cells[WriteCellsIndex, 10] = row["UnitPrice"].ToString();
                                    worksheetUncountable.Cells[WriteCellsIndex, 11] = "=I" + WriteCellsIndex + "*J" + WriteCellsIndex;

                                    WriteCellsIndex = WriteCellsIndex + 1;
                                }
                            }
                            //Subtotal
                            worksheetUncountable.Cells[WriteCellsIndex, 11] = "=SUM(K14:K" + (WriteCellsIndex - 1) + ")";
                            worksheetUncountable.Range[WriteCellsIndex + ":" + WriteCellsIndex].RowHeight = 30;

                        }
                        else
                        {
                            foreach (DataRow row in dtForPrintExcel.Rows)
                            {
                                if (row["MatCalcFlag"].ToString() != "0")
                                {
                                    worksheetUncountable.Cells[14, 1] = 1;
                                    worksheetUncountable.Cells[14, 4] = row["RMCode"].ToString();
                                    worksheetUncountable.Cells[14, 5] = row["ItemName"].ToString();
                                    worksheetUncountable.Cells[14, 7] = row["Maker"].ToString();
                                    worksheetUncountable.Cells[14, 8] = row["MatTypeName"].ToString();
                                    worksheetUncountable.Cells[14, 9] = row["Qty"].ToString();
                                    worksheetUncountable.Cells[14, 10] = row["UnitPrice"].ToString();
                                    worksheetUncountable.Cells[14, 11] = "=I14*J14";
                                }
                            }
                        }

                        //ឆែករកមើល Folder បើគ្មាន => បង្កើត                
                        if (!Directory.Exists(SavePath))
                        {
                            Directory.CreateDirectory(SavePath);
                        }

                        // Saving the modified Excel file                        
                        string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                        string file = "NGReq " + TransNo;
                        fName = file + " ( " + date + " )";
                        worksheetCountable.SaveAs(SavePath + @"\" + fName + ".xlsx");
                        xlWorkBook.Save();
                        xlWorkBook.Close();
                        excelApp.Quit();

                    }
                    catch (Exception ex)
                    {
                        excelApp.DisplayAlerts = false;
                        xlWorkBook.Close();
                        excelApp.DisplayAlerts = true;
                        excelApp.Quit();
                        ErrorText += "\n" + ex.Message;
                    }
                    
                    //Kill all Excel background process
                    var processes = from p in Process.GetProcessesByName("EXCEL")
                                    select p;
                    foreach (var process in processes)
                    {
                        if (process.MainWindowTitle.ToString().Trim() == "")
                            process.Kill();
                    }
                }
            }

            //Console.WriteLine(ErrorText);

            /*

            //Start Calc Total NG Cutting
            dtNGTotalAndStock = new DataTable();
            //DataTable dtPOSAndMCIN = new DataTable();
            DataTable dtExcelPrinting = new DataTable();
            try
            {
                cnn.con.Open();

                string SQLQuery = "SELECT RMCode, SUM(TotalQty) AS Qty FROM " +
                    "\n( " +
                    "\n \n\tSELECT tbNGHeader.*, tbNGDetailsPcs.RMCode, (tbNGDetailsPcs.TotalQty+ tbNGDetailsSET.TotalQty) AS TotalQty  FROM " +
                    "\n\t(SELECT MCSeqNo, PosCNo FROM tbNGInprocess WHERE ReqStatus=0 GROUP BY MCSeqNo, PosCNo) tbNGHeader " +
                    "\n \n \n\tINNER JOIN  " +
                    "\n\t(SELECT MCSeqNo, PosCNo, RMCode,  " +
                    "\n\tCASE  " +
                    "\n\t\tWHEN Unit='Gram (g)' THEN ROUND(Qty/1000/R1OrNetW*MOQ, 5) " +
                    "\n\t\tELSE Qty " +
                    "\n\tEND AS TotalQty FROM " +
                    "\n\t(SELECT MCSeqNo, PosCNo, RMCode, Qty, Unit FROM tbNGInprocess WHERE NGType='NGPcs' AND ReqStatus=0) T1 " +
                    "\n\tLEFT JOIN " +
                    "\n\t(SELECT Code, R1OrNetW, MOQ FROM tbSDMstUncountMat) T2 " +
                    "\n\tON T1.RMCode=T2.Code " +
                    "\n\t) tbNGDetailsPcs " +
                    "\n\tON tbNGHeader.MCSeqNo=tbNGDetailsPcs.MCSeqNo AND tbNGHeader.PosCNo=tbNGDetailsPcs.PosCNo " +
                    "\n\t \n \n\tINNER JOIN  " +
                    "\n\t(SELECT MCSeqNo, T1.PosCNo, RMCode,  " +
                    "\n\tROUND(Qty * LowQty*SemiQtyOfFG,5) AS TotalQty FROM " +
                    "\n\t(SELECT MCSeqNo, PosCNo, RMCode, Qty FROM tbNGInprocess WHERE NGType='NGSet' AND ReqStatus=0) T1  " +
                    "\n\tLEFT JOIN  " +
                    "\n\t(SELECT PosCNo, WIPCode FROM tbPOSDetailofMC) T2  " +
                    "\n\tON T1.PosCNo=T2.PosCNo  " +
                    "\n\tINNER JOIN  " +
                    "\n\t(SELECT * FROM MstBOM) T3  " +
                    "\n\tON T2.WIPCode=T3.UpItemCode AND T1.RMCode=T3.LowItemCode) tbNGDetailsSET " +
                    "\n\tON tbNGDetailsPcs.MCSeqNo=tbNGDetailsSET.MCSeqNo AND tbNGDetailsPcs.PosCNo=tbNGDetailsSET.PosCNo AND tbNGDetailsPcs.RMCode=tbNGDetailsSET.RMCode " +
                    "\n WHERE tbNGHeader.PosCNo IN ( "+POSNoIN+" )" +
                    "\n \n) TbFinal " +
                    "\nGROUP BY RMCode " +
                    "\nORDER BY RMCode ASC " ;

                string SQLQuery = "SELECT PosCNo, RMCode, ROUND(SUM(TotalQty),0) AS Qty FROM " +
                    "\n( " +
                    "\n \n\tSELECT tbNGHeader.*, tbNGDetailsPcs.RMCode, (tbNGDetailsPcs.TotalQty+ tbNGDetailsSET.TotalQty) AS TotalQty  FROM " +
                    "\n\t(SELECT MCSeqNo, PosCNo FROM tbNGInprocess WHERE ReqStatus=0 GROUP BY MCSeqNo, PosCNo) tbNGHeader " +
                    "\n \n \n\tINNER JOIN  " +
                    "\n\t(SELECT MCSeqNo, PosCNo, RMCode,  " +
                    "\n\tCASE  " +
                    "\n\t\tWHEN Unit='Gram (g)' THEN ROUND(Qty/1000/R1OrNetW*MOQ, 2) " +
                    "\n\t\tELSE Qty " +
                    "\n\tEND AS TotalQty FROM " +
                    "\n\t(SELECT MCSeqNo, PosCNo, RMCode, Qty, Unit FROM tbNGInprocess WHERE NGType='NGPcs' AND ReqStatus=0) T1 " +
                    "\n\tLEFT JOIN " +
                    "\n\t(SELECT Code, R1OrNetW, MOQ FROM tbSDMstUncountMat) T2 " +
                    "\n\tON T1.RMCode=T2.Code " +
                    "\n\t) tbNGDetailsPcs " +
                    "\n\tON tbNGHeader.MCSeqNo=tbNGDetailsPcs.MCSeqNo AND tbNGHeader.PosCNo=tbNGDetailsPcs.PosCNo " +
                    "\n\t \n \n\tINNER JOIN  " +
                    "\n\t(SELECT MCSeqNo, T1.PosCNo, RMCode,  " +
                    "\n\tROUND(Qty * LowQty*SemiQtyOfFG,2) AS TotalQty FROM " +
                    "\n\t(SELECT MCSeqNo, PosCNo, RMCode, Qty FROM tbNGInprocess WHERE NGType='NGSet' AND ReqStatus=0) T1  " +
                    "\n\tLEFT JOIN  " +
                    "\n\t(SELECT PosCNo, WIPCode FROM tbPOSDetailofMC) T2  " +
                    "\n\tON T1.PosCNo=T2.PosCNo  " +
                    "\n\tINNER JOIN  " +
                    "\n\t(SELECT * FROM MstBOM) T3  " +
                    "\n\tON T2.WIPCode=T3.UpItemCode AND T1.RMCode=T3.LowItemCode) tbNGDetailsSET " +
                    "\n\tON tbNGDetailsPcs.MCSeqNo=tbNGDetailsSET.MCSeqNo AND tbNGDetailsPcs.PosCNo=tbNGDetailsSET.PosCNo AND tbNGDetailsPcs.RMCode=tbNGDetailsSET.RMCode " +
                    "\n WHERE ROUND((tbNGDetailsPcs.TotalQty+ tbNGDetailsSET.TotalQty),0)>0 AND tbNGHeader.PosCNo IN ( " + POSNoIN + " )" +
                    "\n \n) TbFinal " +
                    "\nGROUP BY PosCNo, RMCode " +
                    "\nORDER BY PosCNo ASC, RMCode ASC ";

                //Console.WriteLine(SQLQuery);
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery,cnn.con);
                sda.Fill(dtNGTotalAndStock);

                //Take for Update ReqStatus 
                SQLQuery = "SELECT MCSeqNo, PosCNo FROM tbNGInprocess WHERE ReqStatus=0 AND PosCNo IN ( " + POSNoIN + " ) GROUP BY MCSeqNo, PosCNo";
                sda = new SqlDataAdapter(SQLQuery, cnn.con);
                //sda.Fill(dtPOSAndMCIN);

            }
            catch (Exception ex)
            {
                ErrorText = "Calc Total NG : " + ex.Message;
            }
            cnn.con.Close();
            //Remove less than 1 

            if (ErrorText.Trim() == "" && dtNGTotalAndStock.Rows.Count > 0)
            {
                for (int i = dtNGTotalAndStock.Rows.Count - 1; i > -1; i--)
                {
                    double Qty = Convert.ToDouble(dtNGTotalAndStock.Rows[i]["Qty"]);
                    Qty = Convert.ToDouble(Qty.ToString("N0"));
                    if (Qty < 1)
                    {
                        dtNGTotalAndStock.Rows.RemoveAt(i);
                    }
                    else
                    {
                        dtNGTotalAndStock.Rows[i]["Qty"] = Qty;
                    }
                    dtNGTotalAndStock.AcceptChanges();
                }

            }

            //Taking RM Stock & OBS Mst
            //string TransNo = "NG0000000001";
            int FoundStockNotEnough = 0;
            if (ErrorText.Trim() == "" && dtNGTotalAndStock.Rows.Count > 0)
            {
                string RMCodeIN = "";
                foreach (DataRow row in dtNGTotalAndStock.Rows)
                {
                    if (RMCodeIN.Contains(row["RMCode"].ToString()) == false)
                    {
                        if (RMCodeIN.Trim() == "")
                        {
                            RMCodeIN = "'" + row["RMCode"].ToString() + "'";
                        }
                        else
                        {
                            RMCodeIN += ", '" + row["RMCode"].ToString() + "'";
                        }
                    }
                }

                //OBS Mst
                try
                {
                    cnnOBS.conOBS.Open();
                    string SQLQuery = "SELECT T1.ItemCode, ItemName, MatTypeName, MatCalcFlag, RESV1 AS Maker, COALESCE(T3.UnitPrice, 0) AS UnitPrice FROM " +
                        "\n(SELECT * FROM mstitem WHERE DelFlag=0 AND ItemType=2) T1 " +
                        "\nLEFT JOIN (SELECT * FROM MstMatType) T2 " +
                        "\nON T1.MatTypeCode=T2.MatTypeCode " +
                        "\nLEFT JOIN (SELECT ItemCode, EffDate, UnitPrice FROM mstpurchaseprice) T3 " +
                        "\nON T1.ItemCode=T3.ItemCode " +
                        "\nINNER JOIN (SELECT ItemCode, MAX(EffDate) AS EffDate FROM mstpurchaseprice GROUP BY ItemCode) T4 " +
                        "\nON T3.ItemCode=T4.ItemCode AND T3.EffDate=T4.EffDate " +
                        "\nWHERE T1.ItemCode IN ("+RMCodeIN+") " +
                        "\nORDER BY ItemCode ASC ";
                    //Console.WriteLine(SQLQuery);
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery,cnnOBS.conOBS);
                    sda.Fill(dtExcelPrinting);
                }
                catch(Exception ex)
                {
                    ErrorText = "Taking OBS Mst : " + ex.Message;
                }
                cnnOBS.conOBS.Close();

                //Add to dtNGTotalAndStock
                dtNGTotalAndStock.Columns.Add("ItemName");
                dtNGTotalAndStock.Columns.Add("ItemType");
                //dtNGTotalAndStock.Columns.Add("Maker");
                //dtNGTotalAndStock.Columns.Add("CountOrUncount");
                //dtNGTotalAndStock.Columns.Add("UP");    
                dtNGTotalAndStock.AcceptChanges();
                foreach (DataRow row in dtNGTotalAndStock.Rows)
                {
                    string ItemName = "";
                    string ItemType = "";
                    string Maker = "";
                    string CountOrUncount = "Count";
                    double UP = 0;

                    foreach (DataRow rowOBS in dtExcelPrinting.Rows)
                    {
                        if (row["RMCode"].ToString() == rowOBS["ItemCode"].ToString())
                        {
                            ItemName = rowOBS["ItemName"].ToString();
                            ItemType = rowOBS["MatTypeName"].ToString();
                            //Maker = rowOBS["Maker"].ToString();
                            //if (rowOBS["MatCalcFlag"].ToString() != "0")
                            //{
                            //    CountOrUncount = "Uncount";
                            //}
                            //UP = Convert.ToDouble(rowOBS["UnitPrice"].ToString());
                            //UP = Convert.ToDouble(UP.ToString("N4"));
                            break;
                        }
                    }

                    row["ItemName"] = ItemName;
                    row["ItemType"] = ItemType;
                    //row["Maker"] = Maker;
                    //row["CountOrUncount"] = CountOrUncount;
                    //row["UP"] = UP;
                    dtNGTotalAndStock.AcceptChanges();

                }

                //Taking MS-Inprocess Stock
                DataTable dtPOSStock = new DataTable();
                DataTable dtOtherStock = new DataTable();
                try
                {
                    cnn.con.Open();
                    string SQLQuery = "SELECT * FROM " +
                                        "\n(SELECT TbTst.Code, TbTst.POSNo AS LotNo, StockValue, StockValue AS RemainStockValue, " +
                                        "\nCASE " +
                                        "\n\tWHEN NOT TRIM(TbTst.POSNo) = '' THEN " +
                                        "\n\t\tCASE " +
                                        "\n\t\t\tWHEN LEFT(TbTst.POSNo,2) = 'SD' THEN TbAllo.POSNo " +
                                        "\n\t\t\tWHEN RIGHT(TbTst.POSNo,6) = 'NGRate' THEN LEFT(TbTst.POSNo,10) " +
                                        "\n\t\t\tELSE TbTst.POSNo " +
                                        "\n\t\tEND " +
                                        "\n\tELSE '' " +
                                        "\nEND AS POSNo, TbRegDate.RegDate FROM " +
                                        "\n(SELECT Code, POSNo, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='MC1' GROUP BY Code, POSNo) TbTst " +
                                        "\nLEFT JOIN (SELECT * FROM tbSDAllocateStock) TbAllo ON TbTst.POSNo=TbAllo.SysNo " +
                                        "\nINNER JOIN (SELECT Code, POSNo, MIN(RegDate) AS RegDate FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='MC1' AND Funct=2 AND NOT TRIM(POSNo) = '' GROUP BY Code, POSNo) TbRegDate ON TbTst.Code=TbRegDate.Code AND TbTst.POSNo=TbRegDate.POSNo) X " +
                                        "\nWHERE StockValue>0 AND POSNo IN ( " + POSNoIN + " ) " +
                                        "\nORDER BY Code ASC, RegDate ASC ";
                    //Console.WriteLine("\ndtPOSStock\n" + SQLQuery);
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtPOSStock);

                    SQLQuery = "SELECT Code, SUM(StockValue) AS StockValue, SUM(StockValue) AS RemainStockValue FROM tbSDMCAllTransaction " +
                                                    "\nWHERE CancelStatus=0 AND LocCode='MC1' AND POSNo = '' AND Code IN (" + RMCodeIN + ") " +
                                                    "\nGROUP BY Code";
                    //Console.WriteLine("\ndtOtherStock\n" + SQLQuery);
                    sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtOtherStock);

                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                }
                cnn.con.Close();

                //Taking MS-Inprocess Stock
                if (ErrorText.Trim() == "")
                {
                    DataTable dtInproStock = new DataTable();
                    try
                    {
                        cnn.con.Open();
                        string SQLQuery = "SELECT Code, SUM(StockValue) AS StockQTY FROM tbSDMCAllTransaction " +
                            "\nWHERE LocCode='MC1' AND CancelStatus=0 AND Code IN ("+RMCodeIN+") " +
                            "\nGROUP BY Code " +
                            "\nORDER BY Code ASC";
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery,cnn.con);
                        sda.Fill(dtInproStock);
                    }
                    catch (Exception ex)
                    {
                        ErrorText = "Taking MC-Inprocess Stock : " + ex.Message;
                    }
                    cnn.con.Close();

                    //Add to dtNGTotalAndStock
                    dtNGTotalAndStock.Columns.Add("StockQty");
                    dtNGTotalAndStock.AcceptChanges();
                    foreach (DataRow row in dtNGTotalAndStock.Rows)
                    {
                        double StockQty = 0;
                        foreach (DataRow rowStock in dtInproStock.Rows)
                        {
                            if (row["RMCode"].ToString() == rowStock["Code"].ToString())
                            {
                                StockQty = Convert.ToDouble(rowStock["StockQTY"].ToString());
                                break;
                            }
                        }

                        //Check Stock Enough or not
                        if (StockQty < Convert.ToDouble(row["Qty"].ToString()))
                        {
                            FoundStockNotEnough++;
                        }

                        row["StockQty"] = StockQty;
                        dtNGTotalAndStock.AcceptChanges();
                    }
                }

            }

            //Add to Transaction & Update ReqStatus
            if (ErrorText.Trim() == "" && dtNGTotalAndStock.Rows.Count > 0 && FoundStockNotEnough == 0)
            {
                try
                {
                    cnn.con.Open();
                    //Find Last TransNo
                    SqlDataAdapter da = new SqlDataAdapter("SELECT SysNo FROM tbSDMCAllTransaction " +
                                                                                    "WHERE SysNo=(SELECT MAX(SysNo) FROM tbSDMCAllTransaction WHERE Funct =6) Group By SysNo", cnn.con);
                    DataTable dtTransNo = new DataTable();
                    da.Fill(dtTransNo);
                    if (dtTransNo.Rows.Count > 0)
                    {
                        string LastTransNo = dtTransNo.Rows[0][0].ToString();
                        double NextTransNo = Convert.ToDouble(LastTransNo.Substring(LastTransNo.Length - 10)) + 1;
                        TransNo = "NG" + NextTransNo.ToString("0000000000");
                    }
                    //DateTime RegDate = DateTime.Now;

                    //MessageBox.Show(TransNo);



                    //Key Codes is here mtf 

                    

                    //Add to tbSDMCAllTransaction
                    foreach (DataRow row in dtNGTotalAndStock.Rows)
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction (SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus) VALUES (@Sn, @Fc, @Lc, @POSN, @Cd, @Rmd, @Rqty, @Tqty, @SV, @Rd, @Rb, @Cs)", cnn.con);
                        cmd.Parameters.AddWithValue("@Sn", TransNo);
                        cmd.Parameters.AddWithValue("@Fc", 6);
                        cmd.Parameters.AddWithValue("@Lc", "MC1");
                        cmd.Parameters.AddWithValue("@POSN", "");
                        cmd.Parameters.AddWithValue("@Cd", row["RMCode"].ToString());
                        cmd.Parameters.AddWithValue("@Rmd", row["ItemName"].ToString());
                        cmd.Parameters.AddWithValue("@Rqty", 0);
                        cmd.Parameters.AddWithValue("@Tqty", Convert.ToDouble(row["Qty"].ToString()));
                        cmd.Parameters.AddWithValue("@SV", Convert.ToDouble(row["Qty"].ToString()) * (-1));
                        cmd.Parameters.AddWithValue("@Rd", RegDate);
                        cmd.Parameters.AddWithValue("@Rb", MenuFormV2.UserForNextForm);
                        cmd.Parameters.AddWithValue("@Cs", 0);
                        //cmd.ExecuteNonQuery();

                    }

                    //Update tbNGInprocess
                    foreach (DataRow row in dtPOSAndMCIN.Rows)
                    {
                        string MCNo = row["MCSeqNo"].ToString();
                        string PosCNo = row["PosCNo"].ToString();

                        string query = "UPDATE tbNGInprocess SET " +
                                            "TransactionNo='" + TransNo + "'," +
                                            "ReqStatus=1 " +
                                            "WHERE ReqStatus=0 AND MCSeqNo = '" + MCNo + "' AND PosCNo='"+PosCNo+"';";
                        SqlCommand cmd = new SqlCommand(query, cnn.con);
                        //cmd.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    ErrorText = "Transaction & Update : " + ex.Message;
                }
                cnn.con.Close();
            }
            
            //Print to Excel
            if (ErrorText.Trim() == "" && dtNGTotalAndStock.Rows.Count > 0 && FoundStockNotEnough == 0)
            {
                var CDirectory = Environment.CurrentDirectory;
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\NGReq_Template.xlsx", Editable: true);

                //Write to Countable
                Excel.Worksheet worksheetCountable = (Excel.Worksheet)xlWorkBook.Sheets["Countable"];
                //Header
                worksheetCountable.Cells[4, 10] = DateTime.Now;
                worksheetCountable.Cells[1, 9] = TransNo;
                worksheetCountable.Cells[5, 10] = MenuFormV2.UserForNextForm;
                int WriteItems = 0;
                foreach (DataRow row in dtNGTotalAndStock.Rows)
                {
                    if (row["CountOrUncount"].ToString() == "Count")
                    {
                        WriteItems = WriteItems + 1;
                    }
                }
                if (WriteItems > 1)
                {
                    //Insert More Rows
                    // Define the range that you want to copy.
                    Excel.Range sourceRange = worksheetCountable.Range["A14:M14"];
                    // Perform the copy operation.
                    sourceRange.Copy(Type.Missing);
                    // Specify the range where you want to insert the copied rows.
                    Excel.Range destinationRange = worksheetCountable.Range["A15:A" + (14 + WriteItems - 1)];
                    // Insert the copied cells and shift the existing cells down.
                    destinationRange.Insert(Excel.XlInsertShiftDirection.xlShiftDown, sourceRange.Copy());
                    worksheetCountable.Range["15:" + (14 + WriteItems - 1)].RowHeight = 18;

                    //Write Data to cells
                    int WriteCellsIndex = 14;
                    foreach (DataRow row in dtNGTotalAndStock.Rows)
                    {
                        if (row["CountOrUncount"].ToString() == "Count")
                        {
                            worksheetCountable.Cells[WriteCellsIndex, 1] = (WriteCellsIndex - 14)+1;
                            worksheetCountable.Cells[WriteCellsIndex, 4] = row["RMCode"].ToString();
                            worksheetCountable.Cells[WriteCellsIndex, 5] = row["ItemName"].ToString();
                            worksheetCountable.Cells[WriteCellsIndex, 7] = row["Maker"].ToString();
                            worksheetCountable.Cells[WriteCellsIndex, 8] = row["ItemType"].ToString();
                            worksheetCountable.Cells[WriteCellsIndex, 9] = row["Qty"].ToString();
                            worksheetCountable.Cells[WriteCellsIndex, 10] = row["UP"].ToString();
                            worksheetCountable.Cells[WriteCellsIndex, 11] = "=I" + WriteCellsIndex + "*J" + WriteCellsIndex;

                            WriteCellsIndex = WriteCellsIndex + 1;
                        }
                    }
                    //Subtotal
                    worksheetCountable.Cells[WriteCellsIndex, 11] = "=SUM(K14:K" + (WriteCellsIndex - 1) + ")";
                    worksheetCountable.Range[WriteCellsIndex + ":" + WriteCellsIndex].RowHeight = 30;
                }
                else
                {
                    foreach (DataRow row in dtNGTotalAndStock.Rows)
                    {
                        if (row["CountOrUncount"].ToString() == "Count")
                        {
                            worksheetCountable.Cells[14, 1] = 1;
                            worksheetCountable.Cells[14, 4] = row["RMCode"].ToString();
                            worksheetCountable.Cells[14, 5] = row["ItemName"].ToString();
                            worksheetCountable.Cells[14, 7] = row["Maker"].ToString();
                            worksheetCountable.Cells[14, 8] = row["ItemType"].ToString();
                            worksheetCountable.Cells[14, 9] = row["Qty"].ToString();
                            worksheetCountable.Cells[14, 10] = row["UP"].ToString();
                            worksheetCountable.Cells[14, 11] = "=I14*J14";

                        }
                    }
                }

                //Write to Uncountable
                Excel.Worksheet worksheetUncountable = (Excel.Worksheet)xlWorkBook.Sheets["Uncountable"];
                //Header
                worksheetUncountable.Cells[4, 10] = DateTime.Now;
                worksheetUncountable.Cells[1, 9] = TransNo;
                worksheetUncountable.Cells[5, 10] = MenuFormV2.UserForNextForm;
                WriteItems = 0;
                foreach (DataRow row in dtNGTotalAndStock.Rows)
                {
                    if (row["CountOrUncount"].ToString() != "Count")
                    {
                        WriteItems = WriteItems + 1;
                    }
                }
                if (WriteItems > 1)
                {
                    //Insert More Rows
                    // Define the range that you want to copy.
                    Excel.Range sourceRange = worksheetUncountable.Range["A14:M14"];
                    // Perform the copy operation.
                    sourceRange.Copy(Type.Missing);
                    // Specify the range where you want to insert the copied rows.
                    Excel.Range destinationRange = worksheetUncountable.Range["A15:A" + (14 + WriteItems - 1)];
                    // Insert the copied cells and shift the existing cells down.
                    destinationRange.Insert(Excel.XlInsertShiftDirection.xlShiftDown, sourceRange.Copy());
                    worksheetUncountable.Range["15:" + (14 + WriteItems - 1)].RowHeight = 18;

                    //Write Data to cells
                    int WriteCellsIndex = 14;
                    foreach (DataRow row in dtNGTotalAndStock.Rows)
                    {
                        if (row["CountOrUncount"].ToString() != "Count")
                        {
                            worksheetUncountable.Cells[WriteCellsIndex, 1] = (WriteCellsIndex - 14) + 1;
                            worksheetUncountable.Cells[WriteCellsIndex, 4] = row["RMCode"].ToString();
                            worksheetUncountable.Cells[WriteCellsIndex, 5] = row["ItemName"].ToString();
                            worksheetUncountable.Cells[WriteCellsIndex, 7] = row["Maker"].ToString();
                            worksheetUncountable.Cells[WriteCellsIndex, 8] = row["ItemType"].ToString();
                            worksheetUncountable.Cells[WriteCellsIndex, 9] = row["Qty"].ToString();
                            worksheetUncountable.Cells[WriteCellsIndex, 10] = row["UP"].ToString();
                            worksheetUncountable.Cells[WriteCellsIndex, 11] = "=I" + WriteCellsIndex + "*J" + WriteCellsIndex;

                            WriteCellsIndex = WriteCellsIndex + 1;
                        }
                    }
                    //Subtotal
                    worksheetUncountable.Cells[WriteCellsIndex, 11] = "=SUM(K14:K" + (WriteCellsIndex - 1) + ")";
                    worksheetUncountable.Range[WriteCellsIndex + ":" + WriteCellsIndex].RowHeight = 30;

                }
                else
                {
                    foreach (DataRow row in dtNGTotalAndStock.Rows)
                    {
                        if (row["CountOrUncount"].ToString() != "Count")
                        {
                            worksheetUncountable.Cells[14, 1] = 1;
                            worksheetUncountable.Cells[14, 4] = row["RMCode"].ToString();
                            worksheetUncountable.Cells[14, 5] = row["ItemName"].ToString();
                            worksheetUncountable.Cells[14, 7] = row["Maker"].ToString();
                            worksheetUncountable.Cells[14, 8] = row["ItemType"].ToString();
                            worksheetUncountable.Cells[14, 9] = row["Qty"].ToString();
                            worksheetUncountable.Cells[14, 10] = row["UP"].ToString();
                            worksheetUncountable.Cells[14, 11] = "=I14*J14";
                        }
                    }
                }


                //ឆែករកមើល Folder បើគ្មាន => បង្កើត                
                if (!Directory.Exists(SavePath))
                {
                    Directory.CreateDirectory(SavePath);
                }

                // Saving the modified Excel file                        
                string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                string file = "NGReq " + TransNo;
                fName = file + " ( " + date + " )";
                worksheetCountable.SaveAs(SavePath + @"\" + fName + ".xlsx");
                xlWorkBook.Save();
                xlWorkBook.Close();
                excelApp.Quit();

                //Kill all Excel background process
                var processes = from p in Process.GetProcessesByName("EXCEL")
                                select p;
                foreach (var process in processes)
                {
                    if (process.MainWindowTitle.ToString().Trim() == "")
                        process.Kill();
                }
            }

            string ColumnTex = "";
            foreach (DataColumn col in dtNGTotalAndStock.Columns)
            {
                ColumnTex += col.ColumnName + "\t";
            }
            Console.WriteLine(ColumnTex);
            foreach (DataRow row in dtNGTotalAndStock.Rows)
            {
                string Msg = "";
                foreach (DataColumn col in dtNGTotalAndStock.Columns)
                {
                    Msg += row[col.ColumnName] + "\t";
                }
                Console.WriteLine(Msg);
            }

            */


            //Update Dgv
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                if (dgvRow.Cells["ChkForPrint"].Value.ToString().ToUpper() == "TRUE")
                {
                    int Found = 0;
                    foreach (DataRow row in dtPOSCannotSave.Rows)
                    {
                        if (dgvRow.Cells["PosCNo"].Value.ToString() == row["POSNo"].ToString())
                        {
                            Found++;
                            break;
                        }
                    }
                    dgvRow.Cells["ChkForPrint"].Value = false;
                    if (Found == 0)
                    {
                        dgvRow.Cells["PrintStatus"].Value = "OK";
                    }
                }
            }
            dgvSearchResult.ClearSelection();
            dgvSearchResult.Refresh();

            //Open Excel
            if (dtForPrintExcel.Rows.Count > 0 && fName.Trim()!="")
            {
                System.Diagnostics.Process.Start(SavePath + @"\" + fName + ".xlsx");
                fName = "";
            }

            Cursor = Cursors.Default;   

            //Alert to User
            if (ErrorText.Trim() == "")
            {
                MessageBox.Show("ព្រីនរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

                ErrorReportAsTxt.PrintError = ErrorText;
                this.OutputError.Output();

                if (dtNotEnough.Rows.Count > 0)
                {
                    MessageBox.Show("មានទិន្នន័យស្តុកខ្វះ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    NGInprocessShortageStockDetailsForm Nissdf = new NGInprocessShortageStockDetailsForm(this);
                    Nissdf.ShowDialog();
                }
                else
                {
                    MessageBox.Show("មានបញ្ហា!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                    
                }
            }

        }        
        private void ClearDtNotEnough()
        {
            dtNotEnough = new DataTable();
            dtNotEnough.Columns.Add("POSNo");
            dtNotEnough.Columns.Add("ItemCode");
            dtNotEnough.Columns.Add("ItemName");
            dtNotEnough.Columns.Add("Qty");
        }
        private void ClearDtCuttingStock()
        {
            dtCuttingStock = new DataTable();
            dtCuttingStock.Columns.Add("Code");
            dtCuttingStock.Columns.Add("RMName");
            dtCuttingStock.Columns.Add("LotOrPOSNo");
            dtCuttingStock.Columns.Add("Qty");
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