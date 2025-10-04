using MachineDeptApp.MsgClass;
using MachineDeptApp.OtherClass;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MachineDeptApp.MCSDControl
{
    public partial class StockInOutComparisonForm : Form
    {
        InformationMsgClass InfoMsg = new InformationMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        ErrorMsgClass EMsg = new ErrorMsgClass();
        ExportAsCSVClass CSV = new ExportAsCSVClass();
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();

        DataTable dtSubSysTransaction;
        DataTable dtOBSTransaction;
        DataTable dtAllRM;

        string ErrorText;

        public StockInOutComparisonForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Shown += StockInOutComparisonForm_Shown;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnExport.Click += BtnExport_Click;
            this.txtItemCode.TextChanged += TxtItemCode_TextChanged;
            this.txtItemName.TextChanged += TxtItemName_TextChanged;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (this.dgvTotalCombine.Rows.Count > 0)
            {
                CSV.CSVErrorT = "";
                CSV.ExportAsCSV(this.dgvTotalCombine, "1. Total Combine-" + DateTime.Now.ToString("yyyyMMdd HHmm"), 0);
                if (CSV.CSVErrorT.Trim() != "")
                {
                    if (CSV.CSVErrorT.Trim() == "OK")
                    {
                        InfoMsg.InfoText = "ទាញទិន្នន័យរួចរាល់!";
                        InfoMsg.ShowingMsg();
                    }
                    else
                    {
                        EMsg.AlertText = "Total Combine : " + CSV.CSVErrorT;
                        EMsg.ShowingMsg();
                    }
                }
            }
            if (this.dgvTotalByRM.Rows.Count > 0)
            {
                CSV.CSVErrorT = "";
                CSV.ExportAsCSV(this.dgvTotalByRM, "2. Total By RM-" + DateTime.Now.ToString("yyyyMMdd HHmm"), 0);
                if (CSV.CSVErrorT.Trim() != "")
                {
                    if (CSV.CSVErrorT.Trim() == "OK")
                    {
                        InfoMsg.InfoText = "ទាញទិន្នន័យរួចរាល់!";
                        InfoMsg.ShowingMsg();
                    }
                    else
                    {
                        EMsg.AlertText = "Total By RM : " + CSV.CSVErrorT;
                        EMsg.ShowingMsg();
                    }
                }
            }
            if (this.dgvMCTransaction.Rows.Count > 0)
            {
                CSV.CSVErrorT = "";
                CSV.ExportAsCSV(this.dgvMCTransaction, "3. MC Transaction -" + DateTime.Now.ToString("yyyyMMdd HHmm"), 0);
                if (CSV.CSVErrorT.Trim() != "")
                {
                    if (CSV.CSVErrorT.Trim() == "OK")
                    {
                        InfoMsg.InfoText = "ទាញទិន្នន័យរួចរាល់!";
                        InfoMsg.ShowingMsg();
                    }
                    else
                    {
                        EMsg.AlertText = "MC Transaction : " + CSV.CSVErrorT;
                        EMsg.ShowingMsg();
                    }
                }
            }
            if (this.dgvOBSTransaction.Rows.Count > 0)
            {
                CSV.CSVErrorT = "";
                CSV.ExportAsCSV(this.dgvOBSTransaction, "4. OBS Transaction -" + DateTime.Now.ToString("yyyyMMdd HHmm"), 0);
                if (CSV.CSVErrorT.Trim() != "")
                {
                    if (CSV.CSVErrorT.Trim() == "OK")
                    {
                        InfoMsg.InfoText = "ទាញទិន្នន័យរួចរាល់!";
                        InfoMsg.ShowingMsg();
                    }
                    else
                    {
                        EMsg.AlertText = "OBS Transaction : " + CSV.CSVErrorT;
                        EMsg.ShowingMsg();
                    }
                }
            }
        }
        private void TxtItemName_TextChanged(object sender, EventArgs e)
        {
            if (txtItemName.Text.Trim() != "")
            {
                chkItemName.Checked = true;
            }
            else
            {
                chkItemName.Checked = false;
            }
        }
        private void TxtItemCode_TextChanged(object sender, EventArgs e)
        {
            if (txtItemCode.Text.Trim() != "")
            {
                chkItemCode.Checked = true;
            }
            else
            {
                chkItemCode.Checked = false;
            }
        }
        private void TxtSearchMC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Console.WriteLine("SSSS");
            }
            else
            {
                Console.WriteLine($"Key Pressed: {e.KeyCode}");
            }
        }
        private void PicSearchMC_Click(object sender, EventArgs e)
        {
            TxtSearchMC_KeyDown(sender, new KeyEventArgs(Keys.Enter));
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            dgvTotalByRM.Rows.Clear();
            dgvMCTransaction.Rows.Clear();
            dgvOBSTransaction.Rows.Clear();
            AssignTotalView("Search");
            dtSubSysTransaction = new DataTable();
            dtOBSTransaction = new DataTable();
            dtAllRM = new DataTable();
            dtAllRM.Columns.Add("Code");

            string DateBetween = " BETWEEN '"+DtDate.Value.ToString("yyyy-MM-dd")+" "+DtDateTime.Value.ToString("HH:mm:00")+"' AND '"+DtEndDate.Value.ToString("yyyy-MM-dd")+" 23:59:59' ";
            string SQLCondsSub = "";
            string SQLCondsOBS = "";
            if (chkItemCode.Checked == true && txtItemCode.Text.Trim() != "")
            {
                string SearchValue = txtItemCode.Text;
                string SubSearchValue = "";
                string OBSSearchValue = "";
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*", "%");
                    SubSearchValue = "Code LIKE '" + SearchValue + "'";
                    OBSSearchValue = "T1.ItemCode LIKE '" + SearchValue + "'";
                }
                else
                {
                    SubSearchValue = "Code = '" + SearchValue + "'";
                    OBSSearchValue = "T1.ItemCode = '" + SearchValue + "'";
                }

                SQLCondsSub += " AND " + SubSearchValue;
                SQLCondsOBS += " AND " + OBSSearchValue;
            }
            if (chkItemName.Checked == true && txtItemName.Text.Trim() != "")
            {
                string SearchValue = txtItemName.Text;
                string SubSearchValue = "";
                string OBSSearchValue = "";
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*", "");
                }
                SubSearchValue = "RMDes LIKE '%" + SearchValue + "%'";
                OBSSearchValue = "ItemName LIKE '%" + SearchValue + "%'";

                SQLCondsSub += " AND " + SubSearchValue;
                SQLCondsOBS += " AND " + OBSSearchValue;
            }

            //Taking Sub-Sys Transaction
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT SysNo, FuncName, LocName, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, TableTrans.Remarks FROM " +
                    "\n\t(SELECT * From tbSDMCAllTransaction WHERE CancelStatus=0) TableTrans " +
                    "\n\tINNER JOIN (SELECT * FROM tbSDMCFunction) TableFunct ON TableTrans.Funct = TableFunct.FuncCode " +
                    "\n\tLEFT JOIN (SELECT * FROM tbSDMCLocation) TableLoc ON TableTrans.LocCode = TableLoc.LocCode WHERE RegDate " + DateBetween + SQLCondsSub+
                    "\nORDER BY RegDate ASC, SysNo ASC, Code ASC";
                //Console.WriteLine(SQLQuery);
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtSubSysTransaction);
            }
            catch (Exception ex)
            {
                ErrorText = "Taking Sub-Sys Transaction : " + ex.Message;
            }
            cnn.con.Close();

            //Taking OBS Transaction
            if (ErrorText.Trim() == "")
            {
                try
                {
                    cnnOBS.conOBS.Open();
                    string SQLQuery = "SELECT DocumentNo, GRIName, T1.ItemCode, ItemName AS OBSItemName, TransactionDate, ReceiveQty AS OBSReceiveQty, IssueQty AS OBSIssueQty, RealValueQty, T1.Remark AS OBSRemark, SurName, T1.CreateDate FROM " +
                        "\n(SELECT * FROM prgalltransaction WHERE TypeCode=1 AND LocCode = 'MC1' AND NOT GRICode IN (70,80)) T1 " +
                        "\nINNER JOIN (SELECT * FROM mstgri WHERE DelFlag=0) T2 ON T1.GRICode=T2.GRICode " +
                        "\nINNER JOIN (SELECT * FROM mstitem WHERE ItemType=2 AND DelFlag=0) T3 ON T1.ItemCode=T3.ItemCode " +
                        "\nLEFT JOIN (SELECT * FROM mststaff WHERE DelFlag=0) T4 ON T1.CreateUser=T4.StaffId " +
                        "\nWHERE T1.CreateDate " + DateBetween + SQLCondsOBS +
                        "\nORDER BY TransactionDate ASC, T1.CreateDate ASC";
                    //Console.WriteLine(SQLQuery);
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                    sda.Fill(dtOBSTransaction);
                }
                catch (Exception ex)
                {
                    ErrorText = "Taking OBS Transaction : " + ex.Message;
                }
                cnnOBS.conOBS.Close();
            }

            //Calc
            if (ErrorText.Trim() == "")
            {
                try
                {
                    foreach (DataGridViewRow rowCombine in dgvTotalCombine.Rows)
                    {
                        string SubFunctName = rowCombine.Cells["FuntionName"].Value.ToString();
                        double SubQty = 0;
                        string OBSFunctName = rowCombine.Cells["OBSFunctName"].Value.ToString();
                        double OBSQty = 0;

                        foreach (DataRow row in dtSubSysTransaction.Rows)
                        {
                            if (row["FuncName"].ToString() == SubFunctName)
                                SubQty += Convert.ToDouble(row["StockValue"]);
                        }

                        foreach (DataRow row in dtOBSTransaction.Rows)
                        {
                            if (row["GRIName"].ToString() == OBSFunctName)
                                OBSQty += Convert.ToDouble(row["RealValueQty"]);
                        }

                        rowCombine.Cells["SubSystem"].Value = SubQty;
                        rowCombine.Cells["OBS"].Value = OBSQty;
                        if (OBSQty < 0 || SubQty < 0)
                            rowCombine.Cells["GAP"].Value = -SubQty + OBSQty;
                        else
                            rowCombine.Cells["GAP"].Value = SubQty - OBSQty;
                    }
                    dgvTotalCombine.ClearSelection();
                }
                catch (Exception ex)
                {
                    ErrorText = "Calc : " + ex.Message;
                }
            }

            //Add to Dgv
            if (ErrorText.Trim() == "")
            {
                try
                {

                    foreach (DataRow row in dtSubSysTransaction.Rows)
                    {
                        dgvMCTransaction.Rows.Add();
                        dgvMCTransaction.Rows[dgvMCTransaction.Rows.Count - 1].Cells["SysNo"].Value = row["SysNo"];
                        dgvMCTransaction.Rows[dgvMCTransaction.Rows.Count - 1].Cells["FuncName"].Value = row["FuncName"];
                        dgvMCTransaction.Rows[dgvMCTransaction.Rows.Count - 1].Cells["LocName"].Value = row["LocName"];
                        dgvMCTransaction.Rows[dgvMCTransaction.Rows.Count - 1].Cells["POSNo"].Value = row["POSNo"];
                        dgvMCTransaction.Rows[dgvMCTransaction.Rows.Count - 1].Cells["Code"].Value = row["Code"];
                        dgvMCTransaction.Rows[dgvMCTransaction.Rows.Count - 1].Cells["ItemName"].Value = row["RMDes"];
                        dgvMCTransaction.Rows[dgvMCTransaction.Rows.Count - 1].Cells["ReceiveQty"].Value = Convert.ToDouble(row["ReceiveQty"]);
                        dgvMCTransaction.Rows[dgvMCTransaction.Rows.Count - 1].Cells["TransferQty"].Value = Convert.ToDouble(row["TransferQty"]);
                        dgvMCTransaction.Rows[dgvMCTransaction.Rows.Count - 1].Cells["Remarks"].Value = Convert.ToDateTime(row["RegDate"]);
                        dgvMCTransaction.Rows[dgvMCTransaction.Rows.Count - 1].Cells["RegDate"].Value = row["RegBy"];
                        dgvMCTransaction.Rows[dgvMCTransaction.Rows.Count - 1].Cells["RegBy"].Value = row["Remarks"];
                        dtAllRM.Rows.Add(row["Code"]);
                    }
                    foreach (DataRow row in dtOBSTransaction.Rows)
                    {
                        dgvOBSTransaction.Rows.Add();
                        dgvOBSTransaction.Rows[dgvOBSTransaction.Rows.Count - 1].Cells["DocumentNo"].Value = row["DocumentNo"];
                        dgvOBSTransaction.Rows[dgvOBSTransaction.Rows.Count - 1].Cells["GRIName"].Value = row["GRIName"];
                        dgvOBSTransaction.Rows[dgvOBSTransaction.Rows.Count - 1].Cells["ItemCode"].Value = row["ItemCode"];
                        dgvOBSTransaction.Rows[dgvOBSTransaction.Rows.Count - 1].Cells["OBSItemName"].Value = row["OBSItemName"];
                        dgvOBSTransaction.Rows[dgvOBSTransaction.Rows.Count - 1].Cells["OBSReceiveQty"].Value = Convert.ToDouble(row["OBSReceiveQty"]);
                        dgvOBSTransaction.Rows[dgvOBSTransaction.Rows.Count - 1].Cells["OBSIssueQty"].Value = Convert.ToDouble(row["OBSIssueQty"]);
                        dgvOBSTransaction.Rows[dgvOBSTransaction.Rows.Count - 1].Cells["OBSRemark"].Value = row["OBSRemark"];
                        dgvOBSTransaction.Rows[dgvOBSTransaction.Rows.Count - 1].Cells["SurName"].Value = row["SurName"];
                        dgvOBSTransaction.Rows[dgvOBSTransaction.Rows.Count - 1].Cells["CreateDate"].Value = Convert.ToDateTime(row["CreateDate"]);
                        dtAllRM.Rows.Add(row["ItemCode"]);
                    }
                }
                catch (Exception ex)
                {
                    ErrorText = "Add to Dgv : " + ex.Message;
                }
            }

            //By Items
            if (ErrorText.Trim() == "" && dtAllRM.Rows.Count>0)
            {
                try
                {
                    // Remove duplicates and order by "Code"
                    dtAllRM = dtAllRM.AsEnumerable()
                                     .GroupBy(row => row["Code"].ToString()) // Group by "Code"
                                     .Select(group => group.First()) // Keep only the first occurrence
                                     .OrderBy(row => row["Code"].ToString()) // Sort by "Code"
                                     .CopyToDataTable();
                    //Add to dgv
                    string RMCodeIN = "";
                    foreach (DataRow row in dtAllRM.Rows)
                    {
                        string Code = row["Code"].ToString();
                        if (RMCodeIN.Trim() == "")
                            RMCodeIN = "'"+Code+"'";
                        else
                            RMCodeIN += ", '" + Code + "'";
                        string Name = "";
                        foreach (DataGridViewRow dgvRow in dgvMCTransaction.Rows)
                        {
                            if (Code == dgvRow.Cells["Code"].Value.ToString())
                            {
                                Name = dgvRow.Cells["ItemName"].Value.ToString();
                                break;
                            }
                        }
                        if (Name.Trim() == "")
                        {
                            foreach (DataGridViewRow dgvRow in dgvOBSTransaction.Rows)
                            {
                                if (Code == dgvRow.Cells["ItemCode"].Value.ToString())
                                {
                                    Name = dgvRow.Cells["OBSItemName"].Value.ToString();
                                    break;
                                }
                            }
                        }

                        foreach (DataGridViewRow dgvRow in dgvTotalCombine.Rows)
                        {
                            dgvTotalByRM.Rows.Add("", "", "", "",0,0,0);
                            dgvTotalByRM.Rows[dgvTotalByRM.Rows.Count - 1].Cells["RMCode"].Value = Code;
                            dgvTotalByRM.Rows[dgvTotalByRM.Rows.Count - 1].Cells["RMName"].Value = Name;
                            dgvTotalByRM.Rows[dgvTotalByRM.Rows.Count - 1].Cells["RMFunctName"].Value = dgvRow.Cells["FuntionName"].Value.ToString();
                            dgvTotalByRM.Rows[dgvTotalByRM.Rows.Count - 1].Cells["RMOBSFunctName"].Value = dgvRow.Cells["OBSFunctName"].Value.ToString();
                        }
                    }

                    //Taking OBS UP
                    string SQLQuery = "SELECT mstpurchaseprice.ItemCode, UnitPrice FROM mstpurchaseprice " +
                        "\nINNER JOIN (SELECT ItemCode, MAX(EffDate) AS MaxEffDate FROM mstpurchaseprice WHERE DelFlag = 0 GROUP BY ItemCode) T1 " +
                        "\nON mstpurchaseprice.ItemCode = T1.ItemCode AND mstpurchaseprice.EffDate = T1.MaxEffDate " +
                        "\nWHERE mstpurchaseprice.ItemCode IN ("+RMCodeIN+") " +
                        "\nORDER BY mstpurchaseprice.ItemCode ASC";
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                    DataTable dtOBS_UP = new DataTable();
                    sda.Fill(dtOBS_UP);

                    //Calc
                    double TTLAmount = 0;
                    foreach (DataGridViewRow row in dgvTotalByRM.Rows)
                    {
                        string RMCode = row.Cells["RMCode"].Value.ToString();
                        string OBSFunct = row.Cells["RMOBSFunctName"].Value.ToString();
                        string SubFunct = row.Cells["RMFunctName"].Value.ToString();
                        double SubQty = 0;
                        foreach (DataRow SubRow in dtSubSysTransaction.Rows)
                        {
                            if (SubRow["FuncName"].ToString() == SubFunct && SubRow["Code"].ToString() == RMCode)
                            {
                                SubQty += Convert.ToDouble(SubRow["StockValue"].ToString());
                            }
                        }

                        double OBSQty = 0;
                        foreach (DataRow OBSRow in dtOBSTransaction.Rows)
                        {
                            if (OBSRow["GRIName"].ToString() == OBSFunct && OBSRow["ItemCode"].ToString() == RMCode)
                            {
                                OBSQty += Convert.ToDouble(OBSRow["RealValueQty"].ToString());
                            }
                        }

                        double UP = 0;
                        foreach (DataRow rowUP in dtOBS_UP.Rows)
                        {
                            if (rowUP["ItemCode"].ToString() == RMCode)
                            {
                                UP = Convert.ToDouble(rowUP["UnitPrice"]);
                                break;
                            }
                        }

                        row.Cells["RMSubSys"].Value = SubQty;
                        row.Cells["RMOBS"].Value = OBSQty;
                        row.Cells["UnitPriceGAP"].Value = UP;

                        if (OBSQty < 0 || SubQty < 0)
                        {
                            row.Cells["RMGAP"].Value = -SubQty + OBSQty;
                            double Amount = (-SubQty + OBSQty) * UP;
                            Amount = Convert.ToDouble(Amount.ToString("N2"));
                            row.Cells["AmountGAP"].Value = Amount;
                            TTLAmount += Amount;
                        }
                        else
                        {
                            row.Cells["RMGAP"].Value = SubQty - OBSQty;
                            double Amount = (SubQty - OBSQty) * UP;
                            Amount = Convert.ToDouble(Amount.ToString("N2"));
                            row.Cells["AmountGAP"].Value = Amount;
                            TTLAmount += Amount;
                        }                        
                    }

                    //Sub-TTL
                    if (dgvTotalByRM.Rows.Count > 0)
                    {
                        dgvTotalByRM.Rows.Add();
                        dgvTotalByRM.Rows[dgvTotalByRM.Rows.Count-1].Cells["AmountGAP"].Value = TTLAmount;
                    }
                }
                catch (Exception ex)
                {
                    ErrorText = "By Items : " + ex.Message;
                }
            }
            
            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                dgvMCTransaction.ClearSelection();
                dgvOBSTransaction.ClearSelection();
            }
            else
            {
                EMsg.AlertText = "មានបញ្ហា!\n"+ErrorText;
                EMsg.ShowingMsg();
            }
        }
        private void StockInOutComparisonForm_Shown(object sender, EventArgs e)
        {
            DtDateTime.Value = Convert.ToDateTime(DateTime.Now.ToString("dd-MM-yyyy")+ "  10:00:00");
            foreach (DataGridViewColumn col in dgvTotalCombine.Columns)
            {
                //Console.WriteLine(col.Name);
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn col in dgvMCTransaction.Columns)
            {
                //Console.WriteLine(col.Name);
            }
            foreach (DataGridViewColumn col in dgvOBSTransaction.Columns)
            {
                //Console.WriteLine(col.Name);
            }

            AssignTotalView("New");
            foreach (DataGridViewRow row in dgvTotalCombine.Rows)
            {
                row.Cells["FuntionName"].Style.BackColor = Color.FromKnownColor(KnownColor.GradientActiveCaption);
                row.Cells["FuntionName"].Style.Font = new Font(dgvTotalCombine.RowsDefaultCellStyle.Font, FontStyle.Bold);

                row.Cells["OBSFunctName"].Style.BackColor = Color.FromKnownColor(KnownColor.GradientActiveCaption);
                row.Cells["OBSFunctName"].Style.Font = new Font(dgvTotalCombine.RowsDefaultCellStyle.Font, FontStyle.Bold);
            }

        }

        private void AssignTotalView(string Type)
        {
            if (Type != "Search")
            {
                dgvTotalCombine.Rows.Clear();
                DataTable dt = new DataTable();
                dt.Columns.Add("SubFunctName");
                dt.Columns.Add("OBSFunctName");
                dt.Rows.Add("NG", "NG");
                dt.Rows.Add("កែស្តុក", "Other Recieve");
                dt.Rows.Add("ចូលស្តុក", "Transfer In");
                dt.Rows.Add("ដកស្តុក", "Other Issue");
                dt.Rows.Add("ផលិតសឺមី", "Consumption Result");

                foreach (DataRow row in dt.Rows)
                {
                    dgvTotalCombine.Rows.Add();
                    dgvTotalCombine.Rows[dgvTotalCombine.Rows.Count - 1].Cells["FuntionName"].Value = row["SubFunctName"];
                    dgvTotalCombine.Rows[dgvTotalCombine.Rows.Count - 1].Cells["OBSFunctName"].Value = row["OBSFunctName"];
                }                
            }
            foreach (DataGridViewRow row in dgvTotalCombine.Rows)
            {
                row.Cells["SubSystem"].Value = 0;
                row.Cells["OBS"].Value = 0;
                row.Cells["GAP"].Value = 0;
            }
            dgvTotalCombine.ClearSelection();
        }

    }
}
