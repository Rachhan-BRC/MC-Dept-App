using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    public partial class WireCalcForProduction : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        public DataTable dtSeletedBobbin;
        public static double SelectedQtyRequierement;
        string ExceptedRM;
        double BeginEdit;
        string SysNo;
        string MCNameSave;
        string fName;

        string ErrorText;

        public WireCalcForProduction()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += WireCalcForProduction_Load;

            //Dgv
            this.dgvPOS.RowsAdded += DgvPOS_RowsAdded;
            this.dgvPOS.RowsRemoved += DgvPOS_RowsRemoved;
            this.dgvPOS.KeyDown += DgvPOS_KeyDown;
            this.dgvRMUsage.CellFormatting += DgvRMUsage_CellFormatting;
            this.dgvRMUsage.CellBeginEdit += DgvRMUsage_CellBeginEdit;
            this.dgvRMUsage.CellEndEdit += DgvRMUsage_CellEndEdit;
            this.dgvRMUsage.CellClick += DgvRMUsage_CellClick;

            //btn
            this.btnNew.Click += BtnNew_Click;
            this.btnAdd.Click += BtnAdd_Click;
            this.btnCalculate.Click += BtnCalculate_Click;
            this.btnSave.Click += BtnSave_Click;
            this.btnPrint.Click += BtnPrint_Click;

        }

        //btn
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            int FoundToBePrint = 0;
            foreach (DataGridViewRow dgvRow in dgvRMUsage.Rows)
            {
                if (dgvRow.Cells["DocNo"].Value != null && dgvRow.Cells["DocNo"].Value.ToString().Trim() != "")
                {
                    FoundToBePrint++;
                }
            }
            if (FoundToBePrint > 0)
            {
                PrintAsExcel();
            }
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(MCNameSave);
            if (dgvRMUsage.Rows.Count > 0)
            {
                int UserAccepted = 0;
                int FoundZero = 0;
                foreach (DataGridViewRow dgvRow in dgvRMUsage.Rows)
                {
                    if (Convert.ToDouble(dgvRow.Cells["TransferQty"].Value.ToString()) == 0)
                    {
                        FoundZero++;
                    }
                }                
                if (FoundZero != dgvRMUsage.Rows.Count)
                {
                    if (FoundZero > 0)
                    {
                        /*
                        DialogResult DSL = MessageBox.Show("មានទិន្នន័យខ្លះ ចំនួនវេរ ស្មើ 0 ! \nតើអ្នកបន្តរក្សាទុកទៀតដែរឬទេ?", "Rachhan System", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                        if (DSL == DialogResult.Yes)
                        {
                            UserAccepted++;
                        }
                        */
                        MessageBox.Show("មានទិន្នន័យខ្លះ ចំនួនវេរ ស្មើ 0 ! \nសូមបញ្ចូលដើម្បីបន្ត!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        DialogResult DSL = MessageBox.Show("តើអ្នកចង់រក្សាទុកទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (DSL == DialogResult.Yes)
                        {
                            UserAccepted++;
                        }
                    }

                    if (UserAccepted > 0)
                    {
                        ErrorText = "";
                        Cursor = Cursors.WaitCursor;

                        //Rechecking Stock again
                        string RMCodeIN = "";
                        foreach (DataGridViewRow DgvRow in dgvRMUsage.Rows)
                        {
                            if (RMCodeIN.Trim() == "")
                            {
                                RMCodeIN = "'" + DgvRow.Cells["RMCode"].Value.ToString() + "'";
                            }
                            else
                            {
                                RMCodeIN += ", '" + DgvRow.Cells["RMCode"].Value.ToString() + "'";
                            }
                        }
                        DataTable dtSDStock = new DataTable();
                        try
                        {
                            cnn.con.Open();
                            string SQLQuery = "SELECT Code, ItemName, POSNo, StockValue FROM " +
                                "\n(SELECT Code, POSNo, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='WIR1' GROUP BY Code, POSNo) T1 " +
                                "\nFULL OUTER JOIN (SELECT ItemCode, ItemName FROM tbMasterItem WHERE Remarks1 IS NULL) T2 " +
                                "\nON T1.Code = T2.ItemCode " +
                                "\nWHERE NOT StockValue IS NULL AND StockValue>0 AND Code IN (" + RMCodeIN + ") " +
                                "\nORDER BY Code ASC, POSNo ASC";
                            SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                            sda.Fill(dtSDStock);
                        }
                        catch (Exception ex)
                        {
                            ErrorText = "មានបញ្ហា!\n" + ex.Message;
                        }
                        cnn.con.Close();
                        int FoundStockNotEnough = 0;
                        foreach (DataGridViewRow DgvRow in dgvRMUsage.Rows)
                        {
                            double TransferQty = Convert.ToDouble(DgvRow.Cells["TransferQty"].Value.ToString());
                            if (TransferQty > 0)
                            {
                                double StockQty = 0;
                                foreach (DataRow row in dtSDStock.Rows)
                                {
                                    if (DgvRow.Cells["RMCode"].Value.ToString() == row["Code"].ToString() && row["POSNo"].ToString().Trim() == "")
                                    {
                                        StockQty = Convert.ToDouble(row["StockValue"].ToString());
                                        break;
                                    }
                                }
                                if (TransferQty > StockQty)
                                {
                                    FoundStockNotEnough++;
                                }
                            }
                        }

                        //Insert if Stock enough
                        SysNo = "SD" + DateTime.Now.ToString("yyMMdd") + "001";
                        string TransNo = "TRF0000000001";
                        string RegBy = MenuFormV2.UserForNextForm;
                        DateTime RegDate = DateTime.Now;
                        if (FoundStockNotEnough == 0)
                        {
                            //Taking SD SysNo & TransNo
                            DataTable dtSys = new DataTable();
                            DataTable dtTransNo = new DataTable();
                            try
                            {
                                cnn.con.Open();
                                //SD SysNo
                                string SQLQuery = "SELECT MAX(SysNo) AS SysNo FROM " +
                                    "\n(SELECT SysNo, RegDate FROM tbSDAllocateStock GROUP BY SysNo, RegDate) T1 " +
                                    "\nWHERE T1.RegDate BETWEEN '" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59' ";
                                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                                sda.Fill(dtSys);

                                //TransNo
                                SQLQuery = "SELECT SysNo FROM tbSDMCAllTransaction " +
                                    "\nWHERE SysNo=(SELECT MAX(SysNo) FROM tbSDMCAllTransaction WHERE Funct =2) Group By SysNo";
                                sda = new SqlDataAdapter(SQLQuery, cnn.con);
                                sda.Fill(dtTransNo);

                            }
                            catch (Exception ex)
                            {
                                ErrorText = "មានបញ្ហា!\n" + ex.Message;
                            }
                            cnn.con.Close();
                            if (ErrorText.Trim() == "" && dtSys.Rows.Count > 0 && dtSys.Rows[0]["SysNo"].ToString().Trim() != "")
                            {
                                string CurrentSysNo = dtSys.Rows[0]["SysNo"].ToString();
                                int LastNo = Convert.ToInt32(CurrentSysNo.Substring(CurrentSysNo.Length - 3, 3));
                                LastNo++;
                                SysNo = "SD" + DateTime.Now.ToString("yyMMdd") + LastNo.ToString("D3");

                            }
                            if (ErrorText.Trim() == "" && dtTransNo.Rows.Count > 0)
                            {
                                string LastTransNo = dtTransNo.Rows[0][0].ToString();
                                double NextTransNo = Convert.ToDouble(LastTransNo.Substring(LastTransNo.Length - 10)) + 1;
                                TransNo = "TRF" + NextTransNo.ToString("0000000000");
                            }

                            //Add to tbSDAllocateStock
                            if (ErrorText.Trim() == "")
                            {
                                try
                                {
                                    cnn.con.Open();
                                    foreach (DataGridViewRow dgvRow in dgvPOS.Rows)
                                    {
                                        string POSNo = dgvRow.Cells["POSNo"].Value.ToString();
                                        SqlCommand cmd = new SqlCommand("INSERT INTO tbSDAllocateStock(SysNo, MCName, POSNo, RegDate, RegBy) VALUES (@Sn, @Mn, @Pn, @Rd, @Rb)", cnn.con);
                                        cmd.Parameters.AddWithValue("@Sn", SysNo); 
                                        cmd.Parameters.AddWithValue("@Mn", MCNameSave);
                                        cmd.Parameters.AddWithValue("@Pn", POSNo);
                                        cmd.Parameters.AddWithValue("@Rd", RegDate);
                                        cmd.Parameters.AddWithValue("@Rb", RegBy);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ErrorText = "tbSDAllocateStock : " + ex.Message;
                                }
                                cnn.con.Close();
                            }

                            //Add tb tbSDMCAllTransaction
                            if (ErrorText.Trim() == "")
                            {
                                try
                                {
                                    cnn.con.Open();
                                    foreach (DataGridViewRow DgvRow in dgvRMUsage.Rows)
                                    {
                                        if (Convert.ToDouble(DgvRow.Cells["TransferQty"].Value.ToString()) > 0)
                                        {
                                            string Code = DgvRow.Cells["RMCode"].Value.ToString();
                                            string Items = DgvRow.Cells["RMName"].Value.ToString();
                                            string OutLocCode = "WIR1";
                                            string InLocCode = "MC1";
                                            string LotOrPOS = "";
                                            double TransferQty = Convert.ToDouble(DgvRow.Cells["TransferQty"].Value.ToString());

                                            //Save To DB Stock-Out
                                            SqlCommand cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                                                           "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs, @Rem)", cnn.con);
                                            cmd.Parameters.AddWithValue("@Sn", TransNo);
                                            cmd.Parameters.AddWithValue("@Ft", 2);
                                            cmd.Parameters.AddWithValue("@Lc", OutLocCode);
                                            cmd.Parameters.AddWithValue("@Pn", LotOrPOS);
                                            cmd.Parameters.AddWithValue("@Cd", Code);
                                            cmd.Parameters.AddWithValue("@Rmd", Items);
                                            cmd.Parameters.AddWithValue("@Rq", 0);
                                            cmd.Parameters.AddWithValue("@Tq", TransferQty);
                                            cmd.Parameters.AddWithValue("@Sv", (TransferQty * (-1)));
                                            cmd.Parameters.AddWithValue("@Rd", RegDate);
                                            cmd.Parameters.AddWithValue("@Rb", RegBy);
                                            cmd.Parameters.AddWithValue("@Cs", 0);
                                            cmd.Parameters.AddWithValue("@Rem", SysNo);
                                            cmd.ExecuteNonQuery();

                                            //Save To DB Stock-IN
                                            cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                                                           "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs, @Rem)", cnn.con);
                                            cmd.Parameters.AddWithValue("@Sn", TransNo);
                                            cmd.Parameters.AddWithValue("@Ft", 2);
                                            cmd.Parameters.AddWithValue("@Lc", InLocCode);
                                            //cmd.Parameters.AddWithValue("@Pn", "");
                                            cmd.Parameters.AddWithValue("@Pn", SysNo);
                                            cmd.Parameters.AddWithValue("@Cd", Code);
                                            cmd.Parameters.AddWithValue("@Rmd", Items);
                                            cmd.Parameters.AddWithValue("@Rq", TransferQty);
                                            cmd.Parameters.AddWithValue("@Tq", 0);
                                            cmd.Parameters.AddWithValue("@Sv", TransferQty);
                                            cmd.Parameters.AddWithValue("@Rd", RegDate);
                                            cmd.Parameters.AddWithValue("@Rb", RegBy);
                                            cmd.Parameters.AddWithValue("@Cs", 0);
                                            cmd.Parameters.AddWithValue("@Rem", SysNo);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ErrorText = "tbSDMCAllTransaction : " + ex.Message;
                                }
                                cnn.con.Close();

                            }

                            //Add to tbBobbinRecords
                            if (ErrorText.Trim() == "")
                            {
                                try
                                {
                                    cnn.con.Open();
                                    foreach (DataRow row in dtSeletedBobbin.Rows)
                                    {
                                        string BobbinNo = row["BobbinCode"].ToString();
                                        string RMCode = row["RMCode"].ToString();
                                        double BeginW = Convert.ToDouble(row["Weigth"]);
                                        double BeginQty = Convert.ToDouble(row["Qty"]);

                                        //tbBobbinRecords
                                        SqlCommand cmd = new SqlCommand("INSERT INTO tbBobbinRecords(BobbinSysNo, RMCode, BStock_Kg, BStock_QTY, MCName, SD_DocNo, Out_Date) " +
                                                                                                    "VALUES (@BSysNo, @Rc, @BW, @Bqty, @MC, @SDSysNo, @OD)", cnn.con);
                                        cmd.Parameters.AddWithValue("@BSysNo", BobbinNo);
                                        cmd.Parameters.AddWithValue("@Rc", RMCode);
                                        cmd.Parameters.AddWithValue("@BW", BeginW);
                                        cmd.Parameters.AddWithValue("@Bqty", BeginQty);
                                        cmd.Parameters.AddWithValue("@MC", MCNameSave);
                                        cmd.Parameters.AddWithValue("@SDSysNo", SysNo);
                                        cmd.Parameters.AddWithValue("@OD", RegDate);
                                        cmd.ExecuteNonQuery();

                                        //Update tbMstRMRegister
                                        string query = "UPDATE tbMstRMRegister SET " +
                                            "C_Location='MC1', " +
                                            "UpdateDate='" + RegDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                            "UpdateBy=N'" + RegBy + "' " +
                                            "WHERE BobbinSysNo = '" + BobbinNo + "' AND RMCode = '"+ RMCode + "';";
                                        cmd = new SqlCommand(query, cnn.con);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ErrorText = "tbBobbinRecords : " + ex.Message;
                                }
                                cnn.con.Close();
                            }
                        }
                        else
                        {
                            ErrorText = "មានវត្ថុធាតុដើមមួយចំនួនគឺខ្វះស្តុក!";
                        }

                        //Add SysNo to DGV
                        if (ErrorText.Trim() == "")
                        {
                            foreach (DataGridViewRow dgvRow in dgvRMUsage.Rows)
                            {
                                if (Convert.ToDouble(dgvRow.Cells["TransferQty"].Value.ToString()) > 0)
                                {
                                    dgvRow.Cells["DocNo"].Value = SysNo;
                                }
                            }
                        }

                        Cursor = Cursors.Default;

                        if (ErrorText.Trim() == "")
                        {
                            MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearDtSeletedBobbin();
                            btnAdd.Enabled = false;
                            btnAddGRAY.BringToFront();
                            btnCalculate.Enabled = false;
                            btnCalculateGRAY.BringToFront();
                            btnSave.Enabled = false;
                            btnSaveGRAY.BringToFront();
                            btnPrint.Enabled = true;
                            btnPrintGRAY.SendToBack();
                        }
                        else
                        {
                            MessageBox.Show(ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("សូមបញ្ចូលចំនួនវេរជាមុនសិន!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            if (dgvPOS.Rows.Count > 0)
            {
                ErrorText = "";
                MCNameSave = "";
                dgvRMUsage.Rows.Clear();
                Cursor = Cursors.WaitCursor;

                //Taking Total Semi Qty
                string WIPCodeIN = "";
                DataTable dtSemiTotal = new DataTable();
                dtSemiTotal.Columns.Add("WIPCode");
                dtSemiTotal.Columns.Add("TotalQty");
                foreach (DataGridViewRow dgvRow in dgvPOS.Rows)
                {
                    if (MCNameSave.Trim() == "")
                    {
                        MCNameSave = dgvRow.Cells["MCName"].Value.ToString();
                    }
                    else
                    {
                        if (MCNameSave.Contains(dgvRow.Cells["MCName"].Value.ToString()) == false)
                        {
                            MCNameSave += "&" + dgvRow.Cells["MCName"].Value.ToString();
                        }
                    }

                    int FoundAlready = 0;
                    foreach (DataRow row in dtSemiTotal.Rows)
                    {
                        if (row["WIPCode"].ToString() == dgvRow.Cells["WIPCode"].Value.ToString())
                        {
                            FoundAlready++;
                            row["TotalQty"] = (Convert.ToDouble(row["TotalQty"]) + Convert.ToDouble(dgvRow.Cells["Qty"].Value.ToString())).ToString();
                            break;
                        }
                    }
                    if (FoundAlready == 0)
                    {
                        if (WIPCodeIN.Trim() == "")
                        {
                            WIPCodeIN = "'"+ dgvRow.Cells["WIPCode"].Value.ToString() + "'";
                        }
                        else
                        {
                            WIPCodeIN += ", '" + dgvRow.Cells["WIPCode"].Value.ToString() + "'";
                        }
                        dtSemiTotal.Rows.Add();
                        dtSemiTotal.Rows[dtSemiTotal.Rows.Count - 1]["WIPCode"] = dgvRow.Cells["WIPCode"].Value.ToString();
                        dtSemiTotal.Rows[dtSemiTotal.Rows.Count - 1]["TotalQty"] = Convert.ToDouble(dgvRow.Cells["Qty"].Value.ToString()).ToString();
                    }
                }

                //Calculate Consumtion
                DataTable dtConsumtion = new DataTable();
                DataTable dtRMList = new DataTable();
                try
                {
                    cnnOBS.conOBS.Open();
                    //string SQLQuery = "SELECT T1.UpItemCode, T2.ItemName, T1.LowItemCode, T3.ItemName AS RMName, T1.LowQty FROM " +
                    //    "\n(SELECT * FROM mstbom) T1 " +
                    //    "\nINNER JOIN (SELECT * FROM mstitem WHERE DelFlag=0 AND ItemType=1) T2 ON T1.UpItemCode=T2.ItemCode " +
                    //    "\nINNER JOIN (SELECT * FROM mstitem WHERE DelFlag=0 AND ItemType=2 AND (MatCalcFlag=1 OR ItemCode IN ( "+ExceptedRM+" ))) T3 ON T1.LowItemCode = T3.ItemCode " +
                    //    "\nWHERE T1.UpItemCode IN ("+WIPCodeIN+") ";

                    string SQLQuery = "SELECT T1.UpItemCode, T2.ItemName, T1.LowItemCode, T3.ItemName AS RMName, T1.LowQty FROM " +
                        "\n(SELECT * FROM mstbom) T1 " +
                        "\nINNER JOIN (SELECT * FROM mstitem WHERE DelFlag=0 AND ItemType=1) T2 ON T1.UpItemCode=T2.ItemCode " +
                        "\nINNER JOIN (SELECT * FROM mstitem WHERE DelFlag=0 AND ItemType=2 AND MatCalcFlag=1) T3 ON T1.LowItemCode = T3.ItemCode " +
                        "\nWHERE T1.UpItemCode IN (" + WIPCodeIN + ") ";


                    //Console.WriteLine(SQLQuery);

                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                    sda.Fill(dtConsumtion);
                    dtConsumtion.Columns.Add("TotalSemiQty");
                    dtConsumtion.Columns.Add("TotalUsage");
                    dtConsumtion.AcceptChanges();

                    //RM List
                    SQLQuery = "SELECT LowItemCode, RMName FROM " +
                        "\n(" + SQLQuery + ") X " +
                        "\nGROUP BY LowItemCode, RMName " +
                        "\nORDER BY LowItemCode ASC";
                    sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                    sda.Fill(dtRMList);
                }
                catch(Exception ex)
                {
                    ErrorText = ex.Message;
                }
                cnnOBS.conOBS.Close();

                //Calc TotalUsage
                foreach (DataRow row in dtSemiTotal.Rows)
                {
                    int Found = 0;
                    foreach (DataRow rowConsupt in dtConsumtion.Rows)
                    {
                        if (row["WIPCode"].ToString() == rowConsupt["UpItemCode"].ToString())
                        {
                            double SemiTotalQty = Convert.ToDouble(row["TotalQty"]);
                            double BOMQty = Convert.ToDouble(rowConsupt["LowQty"].ToString());
                            double TotalUsage = SemiTotalQty * BOMQty;
                            TotalUsage = Convert.ToDouble(TotalUsage.ToString("N3"));
                            rowConsupt["TotalSemiQty"] = SemiTotalQty;
                            rowConsupt["TotalUsage"] = TotalUsage;
                            Found++;
                        }
                    }

                    //Add BOM Status to Dgv
                    if (Found > 0)
                    {
                        foreach (DataGridViewRow dgvRow in dgvPOS.Rows)
                        {
                            if (dgvRow.Cells["WIPCode"].Value.ToString() == row["WIPCode"].ToString())
                            {
                                dgvRow.Cells["mstBOM"].Value = true;
                            }
                        }
                    }
                }

                //For Show dtConsumtion
                /*

                foreach (DataRow row in dtConsumtion.Rows)
                {
                    string Text = "";
                    foreach (DataColumn col in dtConsumtion.Columns)
                    {
                        if (Text.Trim() == "")
                        {
                            Text = row[col.ColumnName].ToString();
                        }
                        else
                        {
                            Text += "\t" + row[col.ColumnName].ToString();
                        }
                    }
                    Console.WriteLine(Text);
                }

                */

                //Taking SD Stock
                DataTable dtSDStock = new DataTable();
                if (ErrorText.Trim() == "" && dtRMList.Rows.Count > 0)
                {
                    string RMCodeIN = "";
                    foreach (DataRow row in dtRMList.Rows)
                    {
                        if (RMCodeIN.Trim() == "")
                        {
                            RMCodeIN = "'" + row["LowItemCode"] +"'";
                        }
                        else
                        {
                            RMCodeIN += ", '" + row["LowItemCode"] + "'";
                        }
                    }
                    try
                    {
                        cnn.con.Open();
                        string SQLQuery = "SELECT Code, ItemName, POSNo, StockValue FROM " +
                            "\n(SELECT Code, POSNo, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='WIR1' GROUP BY Code, POSNo) T1 " +
                            "\nFULL OUTER JOIN (SELECT ItemCode, ItemName FROM tbMasterItem WHERE Remarks1 IS NULL) T2 " +
                            "\nON T1.Code = T2.ItemCode " +
                            "\nWHERE NOT StockValue IS NULL AND StockValue>0 AND Code IN ("+RMCodeIN+") " +
                            "\nORDER BY Code ASC, POSNo ASC";

                        //Console.WriteLine(SQLQuery);

                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        sda.Fill(dtSDStock);
                    }
                    catch(Exception ex)
                    {
                        ErrorText = ex.Message;
                    }
                    cnn.con.Close();
                }

                //Add to Dgv
                if (ErrorText.Trim() == "" && dtRMList.Rows.Count > 0)
                {
                    foreach (DataRow row in dtRMList.Rows)
                    {
                        dgvRMUsage.Rows.Add();
                        dgvRMUsage.Rows[dgvRMUsage.Rows.Count - 1].HeaderCell.Value = dgvRMUsage.Rows.Count.ToString();
                        dgvRMUsage.Rows[dgvRMUsage.Rows.Count - 1].Cells["RMCode"].Value = row["LowItemCode"].ToString();
                        dgvRMUsage.Rows[dgvRMUsage.Rows.Count - 1].Cells["RMName"].Value = row["RMName"].ToString();

                        double TotalRMUsageQty = 0;
                        foreach (DataRow rowConsump in dtConsumtion.Rows)
                        {
                            if (row["LowItemCode"].ToString() == rowConsump["LowItemCode"].ToString())
                            {
                                TotalRMUsageQty += Convert.ToDouble(rowConsump["TotalUsage"]);
                            }
                        }
                        dgvRMUsage.Rows[dgvRMUsage.Rows.Count - 1].Cells["TotalUsageQty"].Value = TotalRMUsageQty;

                        dgvRMUsage.Rows[dgvRMUsage.Rows.Count - 1].Cells["TransferQty"].Value = 0;

                        double SDStockQty = 0;
                        foreach (DataRow rowSD in dtSDStock.Rows)
                        {
                            if (row["LowItemCode"].ToString() == rowSD["Code"].ToString() && rowSD["POSNo"].ToString().Trim()=="")
                            {
                                SDStockQty = Convert.ToDouble(rowSD["StockValue"].ToString());
                                break;
                            }
                        }
                        dgvRMUsage.Rows[dgvRMUsage.Rows.Count - 1].Cells["SDStockQty"].Value = SDStockQty;

                    }
                }

                Cursor = Cursors.Default;   

                if (ErrorText.Trim() == "")
                {
                    if (dgvRMUsage.Rows.Count > 0)
                    {
                        btnSave.Enabled = true;
                        btnSaveGRAY.SendToBack();
                    }
                    else
                    {
                        btnSave.Enabled = false;
                        btnSaveGRAY.SendToBack();
                    }
                    dgvRMUsage.ClearSelection();
                }
                else
                {
                    MessageBox.Show("មានបញ្ហា!\n"+ErrorText, "Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            WireCalcForProductionAddForm Wcfpaf = new WireCalcForProductionAddForm(this);
            Wcfpaf.ShowDialog();
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            dgvPOS.Rows.Clear();
            dgvRMUsage.Rows.Clear();
            ClearDtSeletedBobbin();
            btnAdd.Enabled = true;
            btnAddGRAY.SendToBack();
            CheckBtnForEnable();
            btnPrint.Enabled = false;
            btnPrintGRAY.BringToFront();
        }

        //Dgv
        private void DgvPOS_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            dgvRMUsage.Rows.Clear();
            CheckBtnForEnable();
            AssignSeqNo();
        }
        private void DgvPOS_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dgvRMUsage.Rows.Clear();
            CheckBtnForEnable();
            AssignSeqNo();
        }
        private void DgvPOS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (dgvPOS.SelectedCells.Count > 0 && dgvPOS.CurrentCell.RowIndex > -1 && btnPrint.Enabled == false)
                {
                    DialogResult DSL = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DSL == DialogResult.Yes)
                    {
                        dgvPOS.Rows.RemoveAt(dgvPOS.CurrentCell.RowIndex);
                        dgvPOS.Refresh();
                        dgvPOS.ClearSelection();
                        dgvPOS.CurrentCell = null;
                        AssignSeqNo();
                        CheckBtnForEnable();
                    }
                }
            }
        }
        private void DgvRMUsage_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvRMUsage.Columns[e.ColumnIndex].Name == "TransferQty")
            {
                try
                {
                    double UsageQty = Convert.ToDouble(dgvRMUsage.Rows[e.RowIndex].Cells["TotalUsageQty"].Value.ToString());
                    double TransferQty = Convert.ToDouble(dgvRMUsage.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    double StockQty = Convert.ToDouble(dgvRMUsage.Rows[e.RowIndex].Cells["SDStockQty"].Value.ToString());
                    if (TransferQty == 0)
                    {
                        e.CellStyle.ForeColor = Color.Black;
                        e.CellStyle.Font = new Font(e.CellStyle.Font, e.CellStyle.Font.Style | FontStyle.Regular);
                    }
                    else
                    {
                        if (TransferQty <= StockQty)
                        {
                            e.CellStyle.ForeColor = Color.Green;
                            e.CellStyle.Font = new Font(e.CellStyle.Font, e.CellStyle.Font.Style | FontStyle.Bold);
                        }
                        else
                        {
                            e.CellStyle.ForeColor = Color.Red;
                            e.CellStyle.Font = new Font(e.CellStyle.Font, e.CellStyle.Font.Style | FontStyle.Bold);
                        }
                    }
                }
                catch
                {

                }
            }
        }
        private void DgvRMUsage_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvRMUsage.Columns[e.ColumnIndex].Name == "TransferQty" && e.RowIndex > -1 && btnSave.Enabled==true)
            {
                try
                {
                    double NewValue = Convert.ToDouble(dgvRMUsage.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    NewValue = Convert.ToDouble(NewValue.ToString("N0"));
                    if (NewValue >= 0)
                    {
                        double StockQty = Convert.ToDouble(dgvRMUsage.Rows[e.RowIndex].Cells["SDStockQty"].Value.ToString());
                        StockQty = Convert.ToDouble(StockQty.ToString("N0"));
                        if(NewValue <= StockQty) 
                        {
                            dgvRMUsage.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = NewValue;
                        }
                        else
                        {
                            MessageBox.Show("ចំនួនស្តុកមិនគ្រប់ទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            dgvRMUsage.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = BeginEdit;
                        }
                    }
                    else
                    {
                        MessageBox.Show("ចំនួនមិនអាចតូចជាង 0 បានទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dgvRMUsage.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = BeginEdit;
                    }
                }
                catch
                {
                    MessageBox.Show("អ្នកបញ្ចូលខុសទម្រង់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    dgvRMUsage.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = BeginEdit;
                }
            }
        }
        private void DgvRMUsage_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvRMUsage.Columns[e.ColumnIndex].Name == "TransferQty" && e.RowIndex > -1 && btnSave.Enabled == true)
            {
                BeginEdit = 0;
                try
                {
                    BeginEdit = Convert.ToDouble(dgvRMUsage.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }
                catch
                {

                }
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void DgvRMUsage_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvRMUsage.Columns[e.ColumnIndex].Name == "TransferQty" && e.RowIndex > -1 && btnSave.Enabled == true)
            {
                SelectedQtyRequierement = Convert.ToDouble(dgvRMUsage.Rows[dgvRMUsage.CurrentCell.RowIndex].Cells["TotalUsageQty"].Value);
                WireCalcForProductionRegisterForm Wcfprf = new WireCalcForProductionRegisterForm(this);
                Wcfprf.ShowDialog();
            }
        }

        private void WireCalcForProduction_Load(object sender, EventArgs e)
        {
            ExceptedRM =  "'2114', '1000', '1053', '1132', '1138', '0386', '1015', '1019'";
            foreach (DataGridViewColumn dgvCol in dgvRMUsage.Columns)
            {
                if (dgvCol.Name != "TransferQty")
                {
                    dgvCol.ReadOnly = true;
                }
            }
            ClearDtSeletedBobbin();
        }

        //Method
        private void CheckBtnForEnable()
        {
            //btnCalc
            if (dgvPOS.Rows.Count > 0)
            {
                btnCalculate.Enabled = true;
                btnCalculateGRAY.SendToBack();
            }
            else
            {
                btnCalculate.Enabled = false;
                btnCalculateGRAY.BringToFront();
            }

            //btnSave
            if (dgvRMUsage.Rows.Count > 0)
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
        private void AssignSeqNo()
        {
            foreach(DataGridViewRow dgvRow in dgvPOS.Rows)
            {
                dgvRow.HeaderCell.Value = (dgvRow.Index+1).ToString();
            }
        }
        private void PrintAsExcel()
        {
            Cursor = Cursors.WaitCursor;
            var CDirectory = Environment.CurrentDirectory;
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\SDCalcForProduction_Template.xlsx", Editable: true);
            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["RachhanSystem"];

            int InsertRow = 0;
            foreach (DataGridViewRow dgvRow in dgvRMUsage.Rows)
            {
                if (dgvRow.Cells["DocNo"].Value != null && dgvRow.Cells["DocNo"].Value.ToString().Trim() != "")
                {
                    InsertRow++;
                }
            }

            //Header
            worksheet.Cells[3 , 1] = DateTime.Now;
            worksheet.Cells[4, 1] = SysNo;
            worksheet.Cells[4, 3] = MCNameSave;
            //Insert if more than 1
            if (InsertRow > 1)
            {
                worksheet.Range["8:" + (InsertRow+6)].Insert();
                worksheet.Range["A8:C" + (InsertRow+6)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            }
            //Add data
            int RowIndex = 7;
            foreach (DataGridViewRow dgvRow in dgvRMUsage.Rows)
            {
                if (dgvRow.Cells["DocNo"].Value != null && dgvRow.Cells["DocNo"].Value.ToString().Trim() != "")
                {
                    worksheet.Cells[RowIndex,1] = dgvRow.Cells["RMCode"].Value.ToString();
                    worksheet.Cells[RowIndex, 2] = dgvRow.Cells["RMName"].Value.ToString();
                    worksheet.Cells[RowIndex, 3] = dgvRow.Cells["TransferQty"].Value.ToString();
                    RowIndex++;
                }
            }

            //POS List
            Excel.Worksheet worksheetPOS = (Excel.Worksheet)xlWorkBook.Sheets["POSList"];
            if (dgvPOS.Rows.Count > 1)
            {
                worksheetPOS.Range["6:" + (5+dgvPOS.Rows.Count-1)].Insert();
                worksheetPOS.Range["A6:G" + (5 + dgvPOS.Rows.Count - 1)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            }
            foreach (DataGridViewRow row in dgvPOS.Rows)
            {
                worksheetPOS.Cells[row.Index+5, 1] = row.Cells["WIPCode"].Value.ToString();
                worksheetPOS.Cells[row.Index+5, 2] = row.Cells["WIPName"].Value.ToString();
                worksheetPOS.Cells[row.Index+5, 3] = row.Cells["POSNo"].Value.ToString();
                worksheetPOS.Cells[row.Index+5, 4] = row.Cells["PIN"].Value.ToString();
                worksheetPOS.Cells[row.Index+5, 5] = row.Cells["Wire"].Value.ToString();
                worksheetPOS.Cells[row.Index+5, 6] = row.Cells["Length"].Value.ToString();
                worksheetPOS.Cells[row.Index+5, 7] = row.Cells["Qty"].Value.ToString();
            }


            //ឆែករកមើល Folder បើគ្មាន => បង្កើត
            string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\SDCalcForProduction";
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }

            // Saving the modified Excel file
            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
            string file = "SD_Calc ";
            fName = file + "( " + date + " )";
            worksheet.SaveAs(SavePath + @"\" + fName + ".xlsx");
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

            Cursor = Cursors.Default;
            MessageBox.Show("ការព្រីនបានជោគជ័យ​ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
            System.Diagnostics.Process.Start(SavePath + @"\" + fName + ".xlsx");
            fName = "";

        }
        private void ClearDtSeletedBobbin()
        {
            dtSeletedBobbin = new DataTable();
            dtSeletedBobbin.Columns.Add("RMCode");
            dtSeletedBobbin.Columns.Add("BobbinCode");
            dtSeletedBobbin.Columns.Add("Weigth");
            dtSeletedBobbin.Columns.Add("Qty");

            // Set the primary key
            dtSeletedBobbin.PrimaryKey = new DataColumn[] { dtSeletedBobbin.Columns["RMCode"], dtSeletedBobbin.Columns["BobbinCode"] };
        }

    }
}
