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
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Diagnostics;


namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    public partial class WireStockReturnForm : Form
    {
        ErrorMsgClass EMsg = new ErrorMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        SQLConnect cnn = new SQLConnect();
        DataTable dtTotalStock;
        DataTable dtExcelPrint;
        string SDNoSelected;

        string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\Received From MC";
        string fName;
        string ErrorText;

        public WireStockReturnForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Shown += WireStockReturnForm_Shown;
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;
            this.tabContrl.SelectedIndexChanged += TabContrl_SelectedIndexChanged;
            //btn
            this.btnDelete.Click += BtnDelete_Click;
            this.btnSave.Click += BtnSave_Click;
            this.btnNew.Click += BtnNew_Click;
            this.btnPrint.Click += BtnPrint_Click;

            //New 
            this.txtBarcodeNew.KeyDown += TxtBarcodeNew_KeyDown;
            this.dgvInputNew.CellPainting += DgvInputNew_CellPainting;

            //Old
            this.dgvInput.RowsAdded += DgvInput_RowsAdded;
            this.dgvInput.RowsRemoved += DgvInput_RowsRemoved;
            this.dgvInput.CurrentCellChanged += DgvInput_CurrentCellChanged;

        }

        private void DgvInputNew_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvInputNew.Columns[e.ColumnIndex].Name == "Status")
                {
                    if (dgvInputNew.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "✖️")
                    {
                        e.CellStyle.ForeColor = Color.Red;
                        e.CellStyle.SelectionForeColor = Color.Red;
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.SelectionForeColor = Color.Green;
                    }
                }
            }
        }
        private void TxtBarcodeNew_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcodeNew.Text.Trim() != "" && btnPrint.Enabled==false)
                {
                    if (LbBarcodeTitle.Text.ToString().Contains("SD") == true)
                    {
                        ErrorText = "";
                        SDNoSelected = "";
                        string SelectedRMType = "";
                        if (rdbTerminal.Checked == true)
                        {
                            SelectedRMType = rdbTerminal.Text;
                            dgvInputNew.Columns["TotalWNew"].Visible = false;
                        }
                        else
                        {
                            SelectedRMType = rdbWire.Text;
                            dgvInputNew.Columns["TotalWNew"].Visible = true;
                        }

                        Cursor = Cursors.WaitCursor;

                        DataTable dt = new DataTable();
                        try
                        {
                            cnn.con.Open();
                            string SQLQuery = "SELECT SD_DocNo, BobbinSysNo, RMCode, ItemName, RMTypeName, AStock_Kg, AStock_QTY FROM " +
                                "\n(SELECT * FROM tbBobbinRecords WHERE In_Date IS NULL) T1 " +
                                "\nLEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType = 'Material') T2 ON T1.RMCode = T2.ItemCode " +
                                "\nWHERE SD_DocNo = '"+txtBarcodeNew.Text+ "' AND RMTypeName='"+SelectedRMType+"' " +
                                "\nORDER BY BobbinSysNo ASC, RMTypeName ASC";
                            //Console.WriteLine(SQLQuery);
                            SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                            sda.Fill(dt);
                        }
                        catch (Exception ex)
                        {
                            ErrorText = ex.Message;
                        }
                        cnn.con.Close();

                        Cursor = Cursors.Default;

                        if (ErrorText.Trim() == "")
                        {
                            if (dt.Rows.Count > 0)
                            {
                                SDNoSelected = txtBarcodeNew.Text;
                                txtBarcodeNew.Text = "";
                                foreach (DataRow row in dt.Rows)
                                {
                                    //DocNoNew
                                    //LabelNoNew
                                    //RMCodeNew
                                    //RMNameNew
                                    //RMTypeNew
                                    //TotalWNew
                                    //QtyNew
                                    //Status

                                    dgvInputNew.Rows.Add();
                                    dgvInputNew.Rows[dgvInputNew.Rows.Count - 1].HeaderCell.Value = dgvInputNew.Rows.Count.ToString();
                                    dgvInputNew.Rows[dgvInputNew.Rows.Count - 1].Cells["DocNoNew"].Value = row["SD_DocNo"].ToString();
                                    dgvInputNew.Rows[dgvInputNew.Rows.Count - 1].Cells["LabelNoNew"].Value = row["BobbinSysNo"].ToString();
                                    dgvInputNew.Rows[dgvInputNew.Rows.Count - 1].Cells["RMCodeNew"].Value = row["RMCode"].ToString();
                                    dgvInputNew.Rows[dgvInputNew.Rows.Count - 1].Cells["RMNameNew"].Value = row["ItemName"].ToString();
                                    dgvInputNew.Rows[dgvInputNew.Rows.Count - 1].Cells["RMTypeNew"].Value = row["RMTypeName"].ToString();
                                    dgvInputNew.Rows[dgvInputNew.Rows.Count - 1].Cells["Status"].Value = "✖️";

                                }
                                dgvInputNew.ClearSelection();
                                LbBarcodeTitle.Text = "ស្កេនបាកូដឡាប៊ែលត្រង់នេះ";
                                LbBarcodeTitle.Refresh();
                                txtBarcode.Focus();
                            }
                            else
                            {
                                WMsg.WarningText = "គ្មានទិន្នន័យ SD នេះនៅទីតាំង Inprocess នុះទេ!";
                                WMsg.ShowingMsg();
                                txtBarcodeNew.Focus();
                                txtBarcodeNew.SelectAll();
                            }
                        }
                        else
                        {
                            EMsg.AlertText = "មានបញ្ហា!\n"+ ErrorText;
                            EMsg.ShowingMsg();
                        }
                    }
                    else
                    {
                        string BobbinCode = txtBarcodeNew.Text;
                        string SDNo = "";
                        string RMCode = "";
                        foreach (DataGridViewRow row in dgvInputNew.Rows)
                        {
                            if (row.Cells["LabelNoNew"].Value.ToString() == BobbinCode)
                            {
                                SDNo = row.Cells["DocNoNew"].Value.ToString();
                                RMCode = row.Cells["RMCodeNew"].Value.ToString();
                                break;
                            }
                        }

                        if (SDNo.Trim() != "" && RMCode.Trim() != "")
                        {
                            ErrorText = "";
                            Cursor = Cursors.WaitCursor;

                            DataTable dt = new DataTable();
                            try
                            {
                                cnn.con.Open();
                                string SQLQuery = "SELECT * FROM tbBobbinRecords WHERE In_Date IS NULL AND AStock_QTY IS NOT NULL " +
                                    "\nAND SD_DocNo = '"+SDNo+"' AND BobbinSysNo = '"+BobbinCode+"' AND RMCode='"+RMCode+"' ";
                                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                                sda.Fill(dt);
                            }
                            catch (Exception ex)
                            {
                                ErrorText = ex.Message;
                            }
                            cnn.con.Close();

                            Cursor = Cursors.Default;

                            if (ErrorText.Trim() == "")
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    txtBarcodeNew.Text = "";
                                    double RemainW = 0;
                                    if (dt.Rows[0]["AStock_Kg"].ToString().Trim() != "")
                                        RemainW = Convert.ToDouble(dt.Rows[0]["AStock_Kg"]);
                                    double RemainQty = 0;
                                        RemainQty = Convert.ToDouble(dt.Rows[0]["AStock_QTY"]);

                                    foreach (DataGridViewRow row in dgvInputNew.Rows)
                                    {
                                        if (BobbinCode == row.Cells["LabelNoNew"].Value.ToString())
                                        {
                                            row.Cells["TotalWNew"].Value = RemainW;
                                            row.Cells["QtyNew"].Value = RemainQty;
                                            row.Cells["Status"].Value = "✔️";
                                            break;
                                        }
                                    }
                                    dgvInputNew.ClearSelection();
                                    CheckingBtn();
                                }
                                else
                                {
                                    WMsg.WarningText = "គ្មានទិន្នន័យប៊ូប៊ីននេះទេ!";
                                    WMsg.ShowingMsg();
                                    txtBarcodeNew.Focus();
                                    txtBarcodeNew.SelectAll();
                                }
                            }
                            else
                            {
                                EMsg.AlertText = "មានបញ្ហា!\n"+ErrorText;
                                EMsg.ShowingMsg();
                            }
                        }
                        else
                        {
                            WMsg.WarningText = "គ្មានទិន្នន័យប៊ូប៊ីននេះទេ!";
                            WMsg.ShowingMsg();
                            txtBarcodeNew.Focus();
                            txtBarcodeNew.SelectAll();
                        }
                    }
                }
            }
        }
        private void TabContrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckingBtn();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            PrintOut();
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            if (tabContrl.SelectedIndex == 0)
            {
                this.dgvInputNew.Rows.Clear();
                LbBarcodeTitle.Text = "ស្កេនបាកូដ SD ត្រង់នេះ";
                txtBarcodeNew.Text = "";
                txtBarcodeNew.Focus();
            }
            else
            {
                this.dgvInput.Rows.Clear();
                txtBarcode.Text = "";
                txtBarcode.Focus();
            }            
            CheckingBtn();
            btnPrint.Enabled = false;
            btnPrintGRAY.BringToFront();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (tabContrl.SelectedIndex == 0)
            {
                if (dgvInputNew.Rows.Count > 0)
                {
                    //Recheck
                    int FoundOK = 0;
                    foreach (DataGridViewRow row in dgvInputNew.Rows)
                    {
                        if (row.Cells["Status"].Value.ToString() == "✔️" && row.Cells["QtyNew"].Value != null && row.Cells["QtyNew"].Value.ToString().Trim()!="")
                        {
                            FoundOK++;
                        }
                    }

                    if (dgvInputNew.Rows.Count>0 && FoundOK == dgvInputNew.Rows.Count)
                    {
                        QMsg.QAText = "តើអ្នកចង់រក្សាទុកមែនឬទេ?";
                        QMsg.UserClickedYes = false;
                        QMsg.ShowingMsg();
                        if (QMsg.UserClickedYes == true)
                        {
                            ErrorText = "";
                            ClearDtTotalStock();
                            ClearDtExcelPrint();
                            Cursor = Cursors.WaitCursor;

                            //Take TotalStock
                            string CodeIN = "";
                            string DocNoIN = "";
                            string LabelNoIN = "";
                            foreach (DataGridViewRow dgv in dgvInputNew.Rows)
                            {
                                //dtTotalStock
                                int Found = 0;
                                foreach (DataRow row in dtTotalStock.Rows)
                                {
                                    if (dgv.Cells["DocNoNew"].Value.ToString() == row["DocNo"].ToString() && dgv.Cells["RMCodeNew"].Value.ToString() == row["Code"].ToString())
                                    {
                                        row["Qty"] = Convert.ToDouble(row["Qty"]) + Convert.ToDouble(dgv.Cells["QtyNew"].Value.ToString());
                                        Found++;
                                        break;
                                    }
                                }
                                if (Found == 0)
                                {
                                    dtTotalStock.Rows.Add();
                                    dtTotalStock.Rows[dtTotalStock.Rows.Count - 1]["DocNo"] = dgv.Cells["DocNoNew"].Value.ToString();
                                    dtTotalStock.Rows[dtTotalStock.Rows.Count - 1]["Code"] = dgv.Cells["RMCodeNew"].Value.ToString();
                                    dtTotalStock.Rows[dtTotalStock.Rows.Count - 1]["RMName"] = dgv.Cells["RMNameNew"].Value.ToString();
                                    dtTotalStock.Rows[dtTotalStock.Rows.Count - 1]["Qty"] = Convert.ToDouble(dgv.Cells["QtyNew"].Value.ToString());
                                    dtTotalStock.Rows[dtTotalStock.Rows.Count - 1]["MC1StockRemain"] = 0;
                                }
                                dtTotalStock.AcceptChanges();

                                //dtExcelPrint
                                Found = 0;
                                foreach (DataRow row in dtExcelPrint.Rows)
                                {
                                    if (dgv.Cells["RMCodeNew"].Value.ToString() == row["RMCode"].ToString())
                                    {
                                        row["BobbinQty"] = Convert.ToDouble(row["BobbinQty"]) + 1;
                                        row["Qty"] = Convert.ToDouble(row["Qty"]) + Convert.ToDouble(dgv.Cells["QtyNew"].Value.ToString());
                                        Found++;
                                        break;
                                    }
                                }
                                if (Found == 0)
                                {
                                    dtExcelPrint.Rows.Add();
                                    dtExcelPrint.Rows[dtExcelPrint.Rows.Count - 1]["RMCode"] = dgv.Cells["RMCodeNew"].Value.ToString();
                                    dtExcelPrint.Rows[dtExcelPrint.Rows.Count - 1]["RMName"] = dgv.Cells["RMNameNew"].Value.ToString();
                                    dtExcelPrint.Rows[dtExcelPrint.Rows.Count - 1]["BobbinQty"] = 1;
                                    dtExcelPrint.Rows[dtExcelPrint.Rows.Count - 1]["Qty"] = Convert.ToDouble(dgv.Cells["QtyNew"].Value.ToString());
                                }
                                dtExcelPrint.AcceptChanges();

                                //For Seleting Stock & Update Label
                                if (CodeIN.Contains(dgv.Cells["RMCodeNew"].Value.ToString()) == false)
                                {
                                    if (CodeIN.Trim() == "")
                                    {
                                        CodeIN = "'" + dgv.Cells["RMCodeNew"].Value.ToString() + "'";
                                    }
                                    else
                                    {
                                        CodeIN += ", '" + dgv.Cells["RMCodeNew"].Value.ToString() + "'";
                                    }
                                }
                                if (DocNoIN.Contains(dgv.Cells["DocNoNew"].Value.ToString()) == false)
                                {
                                    if (DocNoIN.Trim() == "")
                                    {
                                        DocNoIN = "'" + dgv.Cells["DocNoNew"].Value.ToString() + "'";
                                    }
                                    else
                                    {
                                        DocNoIN += ", '" + dgv.Cells["DocNoNew"].Value.ToString() + "'";
                                    }
                                }
                                if (LabelNoIN.Trim() == "")
                                {
                                    LabelNoIN = "'" + dgv.Cells["LabelNoNew"].Value.ToString() + "'";
                                }
                                else
                                {
                                    LabelNoIN += ", '" + dgv.Cells["LabelNoNew"].Value.ToString() + "'";
                                }
                            }

                            //Taking Stock Remain
                            try
                            {
                                cnn.con.Open();
                                string SQLQuery = "SELECT POSNo, Code, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction " +
                                    "\nWHERE CancelStatus=0 AND LocCode='MC1' AND Code IN ( " + CodeIN + " ) AND POSNo IN ( " + DocNoIN + " ) " +
                                    "\nGROUP BY POSNo, Code";
                                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                                DataTable dt = new DataTable();
                                sda.Fill(dt);
                                //Add to dtTotalStock
                                foreach (DataRow row in dt.Rows)
                                {
                                    foreach (DataRow TTS in dtTotalStock.Rows)
                                    {
                                        if (row["POSNo"].ToString() == TTS["DocNo"].ToString() && row["Code"].ToString() == TTS["Code"].ToString())
                                        {
                                            TTS["MC1StockRemain"] = Convert.ToDouble(row["StockValue"]);
                                            break;
                                        }
                                    }
                                }
                                dtTotalStock.AcceptChanges();

                            }
                            catch (Exception ex)
                            {
                                ErrorText = "មានបញ្ហា!\nTaking Stock : " + ex.Message;
                            }
                            cnn.con.Close();

                            //Find Stock not enough
                            int NotOKFound = 0;
                            if (ErrorText.Trim() == "")
                            {
                                foreach (DataRow row in dtTotalStock.Rows)
                                {
                                    if (Convert.ToDouble(row["Qty"]) > Convert.ToDouble(row["MC1StockRemain"]))
                                    {
                                        NotOKFound++;
                                        if (ErrorText.Trim() == "")
                                        {
                                            ErrorText = "វត្ថុធាតុដើមខ្វះស្តុក ៖\n • " + row["Code"] + "\t" + row["RMName"] + "\t" + row["DocNo"] + " : Not Enough Stock";
                                        }
                                        else
                                        {
                                            ErrorText += "\n • " + row["Code"] + "\t" + row["RMName"] + "\t" + row["DocNo"] + " : Not Enough Stock";
                                        }
                                    }
                                }
                            }

                            //Add to Transaction & Update
                            if (ErrorText.Trim() == "" && NotOKFound == 0)
                            {
                                //Add to Transaction & Update
                                try
                                {
                                    cnn.con.Open();
                                    //Taking SysNo
                                    DateTime RegDate = DateTime.Now;
                                    string User = MenuFormV2.UserForNextForm;
                                    string TransNo = "TRF0000000001";
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

                                    //Add to Transaction
                                    SqlCommand cmd;
                                    foreach (DataRow row in dtTotalStock.Rows)
                                    {
                                        string POSNo = row["DocNo"].ToString();
                                        string Code = row["Code"].ToString();
                                        string Items = row["RMName"].ToString();
                                        double TransferQty = Convert.ToDouble(row["Qty"]);
                                        string Remark = "MC Inprocess return";

                                        //Save To DB Stock-Out
                                        cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                                                       "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs, @Rem)", cnn.con);
                                        cmd.Parameters.AddWithValue("@Sn", TransNo);
                                        cmd.Parameters.AddWithValue("@Ft", 2);
                                        cmd.Parameters.AddWithValue("@Lc", "MC1");
                                        cmd.Parameters.AddWithValue("@Pn", POSNo);
                                        cmd.Parameters.AddWithValue("@Cd", Code);
                                        cmd.Parameters.AddWithValue("@Rmd", Items);
                                        cmd.Parameters.AddWithValue("@Rq", 0);
                                        cmd.Parameters.AddWithValue("@Tq", TransferQty);
                                        cmd.Parameters.AddWithValue("@Sv", (TransferQty * (-1)));
                                        cmd.Parameters.AddWithValue("@Rd", RegDate.ToString("yyyy-MM-dd HH:mm:ss"));
                                        cmd.Parameters.AddWithValue("@Rb", User);
                                        cmd.Parameters.AddWithValue("@Cs", 0);
                                        cmd.Parameters.AddWithValue("@Rem", Remark);
                                        cmd.ExecuteNonQuery();

                                        //Save To DB Stock-IN
                                        cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                                                       "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs, @Rem)", cnn.con);
                                        cmd.Parameters.AddWithValue("@Sn", TransNo);
                                        cmd.Parameters.AddWithValue("@Ft", 2);
                                        cmd.Parameters.AddWithValue("@Lc", "WIR1");
                                        cmd.Parameters.AddWithValue("@Pn", "");
                                        cmd.Parameters.AddWithValue("@Cd", Code);
                                        cmd.Parameters.AddWithValue("@Rmd", Items);
                                        cmd.Parameters.AddWithValue("@Rq", TransferQty);
                                        cmd.Parameters.AddWithValue("@Tq", 0);
                                        cmd.Parameters.AddWithValue("@Sv", TransferQty);
                                        cmd.Parameters.AddWithValue("@Rd", RegDate);
                                        cmd.Parameters.AddWithValue("@Rb", User);
                                        cmd.Parameters.AddWithValue("@Cs", 0);
                                        cmd.Parameters.AddWithValue("@Rem", Remark);
                                        cmd.ExecuteNonQuery();
                                    }

                                    //Update tbBobbinRecords
                                    string SQLQuery = "UPDATE tbBobbinRecords SET In_Date = '" + RegDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                        "WHERE In_Date IS NULL AND SD_DocNo = '"+SDNoSelected+ "' AND BobbinSysNo IN (" + LabelNoIN + ")";
                                    cmd = new SqlCommand(SQLQuery, cnn.con);
                                    cmd.ExecuteNonQuery();

                                    //Update tbMstRMRegister
                                    SQLQuery = "UPDATE tbMstRMRegister SET C_Location = 'WIR1', UpdateBy = N'"+ User + "', UpdateDate = '" + RegDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                        "WHERE BobbinSysNo IN (" + LabelNoIN + ")";
                                    cmd = new SqlCommand(SQLQuery, cnn.con);
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    ErrorText = "មានបញ្ហា!\n" + ex.Message;
                                }
                                cnn.con.Close();

                                //Console dtTotalStock
                                /*
                                string CS = "";
                                foreach (DataColumn col in dtTotalStock.Columns)
                                {
                                    CS += col.ColumnName.ToString() + "\t";
                                }
                                Console.WriteLine(CS);
                                foreach (DataRow row in dtTotalStock.Rows)
                                {
                                    CS = "";
                                    foreach (DataColumn col in dtTotalStock.Columns)
                                    {
                                        CS += row[col.ColumnName].ToString() + "\t";
                                    }
                                    Console.WriteLine(CS);
                                }
                                */
                            }

                            Cursor = Cursors.Default;

                            if (ErrorText.Trim() == "")
                            {
                                btnDelete.Enabled = false;
                                btnDeleteGRAY.BringToFront();
                                btnSave.Enabled = false;
                                btnSaveGRAY.BringToFront();
                                btnPrint.Enabled = true;
                                btnPrintGRAY.SendToBack();
                                InfoMsg.InfoText = "រក្សាទុករួចរាល់!";
                                InfoMsg.ShowingMsg();

                                //Console dtExcelPrint
                                string CS = "";
                                foreach (DataColumn col in dtExcelPrint.Columns)
                                {
                                    CS += col.ColumnName.ToString() + "\t";
                                }
                                //Console.WriteLine(CS);
                                foreach (DataRow row in dtExcelPrint.Rows)
                                {
                                    CS = "";
                                    foreach (DataColumn col in dtExcelPrint.Columns)
                                    {
                                        CS += row[col.ColumnName].ToString() + "\t";
                                    }
                                    //Console.WriteLine(CS);
                                }
                            }
                            else
                            {
                                EMsg.AlertText = ErrorText;
                                EMsg.ShowingMsg();
                            }
                        }
                    }
                    else
                    {
                        WMsg.WarningText = "សូមស្កេនទិន្នន័យប៊ូប៊ីនដែលមាននៅ\n" +
                                                           "ក្នុង List ទាំងអស់ជាមុនសិន!";
                        WMsg.ShowingMsg();
                    }
                }
            }
            else
            {
                if (dgvInput.Rows.Count > 0)
                {
                    QMsg.QAText = "តើអ្នកចង់រក្សាទុកមែនឬទេ?";
                    QMsg.UserClickedYes = false;
                    QMsg.ShowingMsg();
                    if (QMsg.UserClickedYes == true)
                    {
                        ErrorText = "";
                        ClearDtTotalStock();
                        ClearDtExcelPrint();
                        Cursor = Cursors.WaitCursor;

                        //Take TotalStock
                        string CodeIN = "";
                        string DocNoIN = "";
                        string LabelNoIN = "";
                        foreach (DataGridViewRow dgv in dgvInput.Rows)
                        {
                            //dtTotalStock
                            int Found = 0;
                            foreach (DataRow row in dtTotalStock.Rows)
                            {
                                if (dgv.Cells["DocNo"].Value.ToString() == row["DocNo"].ToString() && dgv.Cells["RMCode"].Value.ToString() == row["Code"].ToString())
                                {
                                    row["Qty"] = Convert.ToDouble(row["Qty"]) + Convert.ToDouble(dgv.Cells["Qty"].Value.ToString());
                                    Found++;
                                    break;
                                }
                            }
                            if (Found == 0)
                            {
                                dtTotalStock.Rows.Add();
                                dtTotalStock.Rows[dtTotalStock.Rows.Count - 1]["DocNo"] = dgv.Cells["DocNo"].Value.ToString();
                                dtTotalStock.Rows[dtTotalStock.Rows.Count - 1]["Code"] = dgv.Cells["RMCode"].Value.ToString();
                                dtTotalStock.Rows[dtTotalStock.Rows.Count - 1]["RMName"] = dgv.Cells["RMName"].Value.ToString();
                                dtTotalStock.Rows[dtTotalStock.Rows.Count - 1]["Qty"] = Convert.ToDouble(dgv.Cells["Qty"].Value.ToString());
                                dtTotalStock.Rows[dtTotalStock.Rows.Count - 1]["MC1StockRemain"] = 0;
                            }
                            dtTotalStock.AcceptChanges();

                            //dtExcelPrint
                            Found = 0;
                            foreach (DataRow row in dtExcelPrint.Rows)
                            {
                                if (dgv.Cells["RMCode"].Value.ToString() == row["RMCode"].ToString())
                                {
                                    row["BobbinQty"] = Convert.ToDouble(row["BobbinQty"]) + 1;
                                    row["Qty"] = Convert.ToDouble(row["Qty"]) + Convert.ToDouble(dgv.Cells["Qty"].Value.ToString());
                                    Found++;
                                    break;
                                }
                            }
                            if (Found == 0)
                            {
                                dtExcelPrint.Rows.Add();
                                dtExcelPrint.Rows[dtExcelPrint.Rows.Count - 1]["RMCode"] = dgv.Cells["RMCode"].Value.ToString();
                                dtExcelPrint.Rows[dtExcelPrint.Rows.Count - 1]["RMName"] = dgv.Cells["RMName"].Value.ToString();
                                dtExcelPrint.Rows[dtExcelPrint.Rows.Count - 1]["BobbinQty"] = 1;
                                dtExcelPrint.Rows[dtExcelPrint.Rows.Count - 1]["Qty"] = Convert.ToDouble(dgv.Cells["Qty"].Value.ToString());
                            }
                            dtExcelPrint.AcceptChanges();

                            //For Seleting Stock & Update Label
                            if (CodeIN.Contains(dgv.Cells["RMCode"].Value.ToString()) == false)
                            {
                                if (CodeIN.Trim() == "")
                                {
                                    CodeIN = "'" + dgv.Cells["RMCode"].Value.ToString() + "'";
                                }
                                else
                                {
                                    CodeIN += ", '" + dgv.Cells["RMCode"].Value.ToString() + "'";
                                }
                            }
                            if (DocNoIN.Contains(dgv.Cells["DocNo"].Value.ToString()) == false)
                            {
                                if (DocNoIN.Trim() == "")
                                {
                                    DocNoIN = "'" + dgv.Cells["DocNo"].Value.ToString() + "'";
                                }
                                else
                                {
                                    DocNoIN += ", '" + dgv.Cells["DocNo"].Value.ToString() + "'";
                                }
                            }
                            if (LabelNoIN.Trim() == "")
                            {
                                LabelNoIN = "'" + dgv.Cells["LabelNo"].Value.ToString() + "'";
                            }
                            else
                            {
                                LabelNoIN += ", '" + dgv.Cells["LabelNo"].Value.ToString() + "'";
                            }
                        }

                        //Taking Stock Remain
                        try
                        {
                            cnn.con.Open();
                            string SQLQuery = "SELECT POSNo, Code, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction " +
                                "\nWHERE CancelStatus=0 AND LocCode='MC1' AND Code IN ( " + CodeIN + " ) AND POSNo IN ( " + DocNoIN + " ) " +
                                "\nGROUP BY POSNo, Code";
                            SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            //Add to dtTotalStock
                            foreach (DataRow row in dt.Rows)
                            {
                                foreach (DataRow TTS in dtTotalStock.Rows)
                                {
                                    if (row["POSNo"].ToString() == TTS["DocNo"].ToString() && row["Code"].ToString() == TTS["Code"].ToString())
                                    {
                                        TTS["MC1StockRemain"] = Convert.ToDouble(row["StockValue"]);
                                        break;
                                    }
                                }
                            }
                            dtTotalStock.AcceptChanges();

                        }
                        catch (Exception ex)
                        {
                            ErrorText = "មានបញ្ហា!\nTaking Stock : " + ex.Message;
                        }
                        cnn.con.Close();

                        //Find Stock not enough
                        int NotOKFound = 0;
                        if (ErrorText.Trim() == "")
                        {
                            foreach (DataRow row in dtTotalStock.Rows)
                            {
                                if (Convert.ToDouble(row["Qty"]) > Convert.ToDouble(row["MC1StockRemain"]))
                                {
                                    NotOKFound++;
                                    if (ErrorText.Trim() == "")
                                    {
                                        ErrorText = "វត្ថុធាតុដើមខ្វះស្តុក ៖\n • " + row["Code"] + "\t" + row["RMName"] + "\t" + row["DocNo"] + " : Not Enough Stock";
                                    }
                                    else
                                    {
                                        ErrorText += "\n • " + row["Code"] + "\t" + row["RMName"] + "\t" + row["DocNo"] + " : Not Enough Stock";
                                    }
                                }
                            }
                        }

                        //Add to Transaction & Update
                        if (ErrorText.Trim() == "" && NotOKFound == 0)
                        {
                            //Add to Transaction & Update
                            try
                            {
                                cnn.con.Open();
                                //Taking SysNo
                                DateTime RegDate = DateTime.Now;
                                string User = MenuFormV2.UserForNextForm;
                                string TransNo = "TRF0000000001";
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

                                //Add to Transaction
                                SqlCommand cmd;
                                foreach (DataRow row in dtTotalStock.Rows)
                                {
                                    string POSNo = row["DocNo"].ToString();
                                    string Code = row["Code"].ToString();
                                    string Items = row["RMName"].ToString();
                                    double TransferQty = Convert.ToDouble(row["Qty"]);
                                    string Remark = "MC Inprocess return";

                                    //Save To DB Stock-Out
                                    cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                                                   "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs, @Rem)", cnn.con);
                                    cmd.Parameters.AddWithValue("@Sn", TransNo);
                                    cmd.Parameters.AddWithValue("@Ft", 2);
                                    cmd.Parameters.AddWithValue("@Lc", "MC1");
                                    cmd.Parameters.AddWithValue("@Pn", POSNo);
                                    cmd.Parameters.AddWithValue("@Cd", Code);
                                    cmd.Parameters.AddWithValue("@Rmd", Items);
                                    cmd.Parameters.AddWithValue("@Rq", 0);
                                    cmd.Parameters.AddWithValue("@Tq", TransferQty);
                                    cmd.Parameters.AddWithValue("@Sv", (TransferQty * (-1)));
                                    cmd.Parameters.AddWithValue("@Rd", RegDate.ToString("yyyy-MM-dd HH:mm:ss"));
                                    cmd.Parameters.AddWithValue("@Rb", User);
                                    cmd.Parameters.AddWithValue("@Cs", 0);
                                    cmd.Parameters.AddWithValue("@Rem", Remark);
                                    cmd.ExecuteNonQuery();

                                    //Save To DB Stock-IN
                                    cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                                                   "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs, @Rem)", cnn.con);
                                    cmd.Parameters.AddWithValue("@Sn", TransNo);
                                    cmd.Parameters.AddWithValue("@Ft", 2);
                                    cmd.Parameters.AddWithValue("@Lc", "WIR1");
                                    cmd.Parameters.AddWithValue("@Pn", "");
                                    cmd.Parameters.AddWithValue("@Cd", Code);
                                    cmd.Parameters.AddWithValue("@Rmd", Items);
                                    cmd.Parameters.AddWithValue("@Rq", TransferQty);
                                    cmd.Parameters.AddWithValue("@Tq", 0);
                                    cmd.Parameters.AddWithValue("@Sv", TransferQty);
                                    cmd.Parameters.AddWithValue("@Rd", RegDate);
                                    cmd.Parameters.AddWithValue("@Rb", User);
                                    cmd.Parameters.AddWithValue("@Cs", 0);
                                    cmd.Parameters.AddWithValue("@Rem", Remark);
                                    cmd.ExecuteNonQuery();
                                }

                                //Update
                                string SQLQuery = "UPDATE tbSDAllocateReturnDetails SET Status = 'Closed', UpdateDate = '" + RegDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                    "WHERE Status = 'Open' AND SysNo IN (" + LabelNoIN + ")";
                                cmd = new SqlCommand(SQLQuery, cnn.con);
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                ErrorText = "មានបញ្ហា!\n" + ex.Message;
                            }
                            cnn.con.Close();

                            //Console dtTotalStock
                            /*
                            string CS = "";
                            foreach (DataColumn col in dtTotalStock.Columns)
                            {
                                CS += col.ColumnName.ToString() + "\t";
                            }
                            Console.WriteLine(CS);
                            foreach (DataRow row in dtTotalStock.Rows)
                            {
                                CS = "";
                                foreach (DataColumn col in dtTotalStock.Columns)
                                {
                                    CS += row[col.ColumnName].ToString() + "\t";
                                }
                                Console.WriteLine(CS);
                            }
                            */
                        }

                        Cursor = Cursors.Default;

                        if (ErrorText.Trim() == "")
                        {
                            btnDelete.Enabled = false;
                            btnDeleteGRAY.BringToFront();
                            btnSave.Enabled = false;
                            btnSaveGRAY.BringToFront();
                            btnPrint.Enabled = true;
                            btnPrintGRAY.SendToBack();
                            InfoMsg.InfoText = "រក្សាទុករួចរាល់!";
                            InfoMsg.ShowingMsg();

                            //Console dtExcelPrint
                            string CS = "";
                            foreach (DataColumn col in dtExcelPrint.Columns)
                            {
                                CS += col.ColumnName.ToString() + "\t";
                            }
                            //Console.WriteLine(CS);
                            foreach (DataRow row in dtExcelPrint.Rows)
                            {
                                CS = "";
                                foreach (DataColumn col in dtExcelPrint.Columns)
                                {
                                    CS += row[col.ColumnName].ToString() + "\t";
                                }
                                //Console.WriteLine(CS);
                            }
                        }
                        else
                        {
                            EMsg.AlertText = ErrorText;
                            EMsg.ShowingMsg();
                        }
                    }
                }
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvInput.SelectedCells.Count > 0 && dgvInput.CurrentCell != null && dgvInput.CurrentCell.RowIndex > -1)
            {
                QMsg.QAText = "តើអ្នកចង់លុបទិន្នន័យនេះមែនឬទេ?";
                QMsg.UserClickedYes = false;
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    dgvInput.Rows.RemoveAt(dgvInput.CurrentCell.RowIndex);
                    dgvInput.Refresh();
                    dgvInput.ClearSelection();
                    dgvInput.CurrentCell = null;
                }
            }
        }

        private void DgvInput_CurrentCellChanged(object sender, EventArgs e)
        {
            CheckingBtn();
        }
        private void DgvInput_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            foreach (DataGridViewRow Row in dgvInput.Rows)
            {
                Row.HeaderCell.Value = (Row.Index + 1).ToString();
            }
            dgvInput.ClearSelection();
            dgvInput.CurrentCell = null;
            CheckingBtn();
        }
        private void DgvInput_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            foreach (DataGridViewRow Row in dgvInput.Rows)
            {
                Row.HeaderCell.Value = (Row.Index+1).ToString();
            }
            dgvInput.ClearSelection();
            dgvInput.CurrentCell = null;
            CheckingBtn();
        }

        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.Trim() != "" && btnPrint.Enabled == false)
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;

                    //Check Already Scan
                    int Found = 0;
                    foreach (DataGridViewRow row in dgvInput.Rows)
                    {
                        if (txtBarcode.Text == row.Cells["LabelNo"].Value.ToString())
                        {
                            Found++;
                            break;
                        }
                    }
                    if (Found == 0)
                    {
                        //Checking Label Status
                        try
                        {
                            cnn.con.Open();
                            string SQLQuery = "SELECT SysNo, DocumentNo, POSNo, T1.Code, ItemName, RMType, LotNo, TotalWeight, Qty, Status FROM " +
                                "\n(SELECT * FROM tbSDAllocateReturnDetails WHERE NOT Status = 'Deleted') T1 " +
                                "\nINNER JOIN (SELECT * FROM tbMasterItem) T2 ON T1.Code=T2.ItemCode " +
                                "\nLEFT JOIN (SELECT * FROM tbSDMstUncountMat) T3 ON T1.Code=T3.Code " +
                                "\nWHERE T1.SysNo='" + txtBarcode.Text + "' ";
                            SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                            DataTable dtChecking = new DataTable();
                            sda.Fill(dtChecking);
                            if (dtChecking.Rows.Count > 0)
                            {
                                if (dtChecking.Rows[0]["Status"].ToString() == "Open")
                                {
                                    dgvInput.Rows.Add();
                                    dgvInput.Rows[dgvInput.Rows.Count - 1].Cells["DocNo"].Value = dtChecking.Rows[0]["DocumentNo"];
                                    dgvInput.Rows[dgvInput.Rows.Count - 1].Cells["LabelNo"].Value = dtChecking.Rows[0]["SysNo"];
                                    dgvInput.Rows[dgvInput.Rows.Count - 1].Cells["RMCode"].Value = dtChecking.Rows[0]["Code"];
                                    dgvInput.Rows[dgvInput.Rows.Count - 1].Cells["RMName"].Value = dtChecking.Rows[0]["ItemName"];
                                    dgvInput.Rows[dgvInput.Rows.Count - 1].Cells["RMType"].Value = dtChecking.Rows[0]["RMType"];
                                    dgvInput.Rows[dgvInput.Rows.Count - 1].Cells["TotalW"].Value = Convert.ToDouble(dtChecking.Rows[0]["TotalWeight"]);
                                    dgvInput.Rows[dgvInput.Rows.Count - 1].Cells["Qty"].Value = Convert.ToDouble(dtChecking.Rows[0]["Qty"]);
                                    dgvInput.Rows[dgvInput.Rows.Count - 1].Cells["LotNo"].Value = dtChecking.Rows[0]["LotNo"];
                                    dgvInput.ClearSelection();
                                    dgvInput.CurrentCell = null;
                                }
                                else
                                {
                                    ErrorText = "ឡាប៊ែលនេះស្កេនរួចហើយ!";
                                }
                            }
                            else
                            {
                                ErrorText = "គ្មានទិន្នន័យឡាប៊ែលនេះទេ!";
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorText = "មានបញ្ហា!\n" + ex.Message;
                        }
                        cnn.con.Close();
                    }
                    else
                    {
                        ErrorText = "ស្កេនមុននេះរួចហើយ!";
                    }

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        txtBarcode.Text = "";
                        txtBarcode.Focus();
                    }
                    else
                    {
                        EMsg.AlertText = ErrorText;
                        EMsg.ShowingMsg();
                        txtBarcode.Focus();
                        txtBarcode.SelectAll();
                    }

                }
            }
        }
        private void WireStockReturnForm_Shown(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn DgvCol in dgvInput.Columns)
            {
                string Text = DgvCol.HeaderText.ToString();
                Text = Text.Replace("/", "\n");
                DgvCol.HeaderText = Text;
                DgvCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                //if (DgvCol.Index != 8 && DgvCol.Index != 10)
                //{
                //    DgvCol.ReadOnly = true;
                //}
                Console.WriteLine(DgvCol.Name);
            }
        }

        //Method
        private void CheckingBtn()
        {
            //btnSave
            if (btnPrint.Enabled == false)
            {
                if (tabContrl.SelectedIndex == 0)
                {
                    if (dgvInputNew.Rows.Count > 0)
                    {
                        int CountAlready = 0;
                        foreach (DataGridViewRow row in dgvInputNew.Rows)
                        {
                            if (row.Cells["Status"].Value.ToString() == "✔️")
                            {
                                CountAlready++;
                            }
                        }
                        if (CountAlready == dgvInputNew.Rows.Count)
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
                    else
                    {
                        btnSave.Enabled = false;
                        btnSaveGRAY.BringToFront();
                    }
                }
                else
                {
                    if (dgvInput.Rows.Count > 0)
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

            //btnDelete
            if (tabContrl.SelectedIndex == 1)
            {
                if (btnPrint.Enabled == false)
                {
                    if (dgvInput.SelectedCells.Count > 0 && dgvInput.CurrentCell != null && dgvInput.CurrentCell.RowIndex > -1)
                    {
                        btnDelete.Enabled = true;
                        btnDeleteGRAY.SendToBack();
                    }
                    else
                    {
                        btnDelete.Enabled = false;
                        btnDeleteGRAY.BringToFront();
                    }
                }
            }
            else
            {
                btnDelete.Enabled = false;
                btnDeleteGRAY.BringToFront();
            }

        }
        private void ClearDtTotalStock()
        {
            dtTotalStock = new DataTable();
            dtTotalStock.Columns.Add("DocNo");
            dtTotalStock.Columns.Add("Code");
            dtTotalStock.Columns.Add("RMName");
            dtTotalStock.Columns.Add("Qty");
            dtTotalStock.Columns.Add("MC1StockRemain");
        }
        private void ClearDtExcelPrint()
        {
            dtExcelPrint = new DataTable();
            dtExcelPrint.Columns.Add("RMCode");
            dtExcelPrint.Columns.Add("RMName");
            dtExcelPrint.Columns.Add("BobbinQty");
            dtExcelPrint.Columns.Add("Qty");
        }
        private void PrintOut()
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងព្រីន . . .";
            LbStatus.Refresh();
            var CDirectory = Environment.CurrentDirectory;
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\RemainingTagTemplate.xlsx", Editable: true);

            try
            {
                Excel.Worksheet worksheetDelete = (Excel.Worksheet)xlWorkBook.Sheets["RachhanSystem"];
                Excel.Worksheet worksheetTotal = (Excel.Worksheet)xlWorkBook.Sheets["Total"];
                if (dtExcelPrint.Rows.Count > 1)
                {
                    worksheetTotal.Range["3:" + (dtExcelPrint.Rows.Count + 1)].Insert();
                    worksheetTotal.Range["A3:D" + (dtExcelPrint.Rows.Count + 1)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                }
                //Write data to the excel
                for (int i = 0; i < dtExcelPrint.Rows.Count; i++)
                {
                    for (int j = 0; j < dtExcelPrint.Columns.Count; j++)
                    {
                        worksheetTotal.Cells[i + 2, j + 1] = dtExcelPrint.Rows[i][j].ToString();
                    }
                }

                //Delete Sheet
                excelApp.DisplayAlerts = false;
                worksheetDelete.Delete();
                excelApp.DisplayAlerts = true;

                //Rename Sheet
                worksheetTotal.Name = "RachhanSystem";

                //ឆែករកមើល Folder បើគ្មាន => បង្កើត            
                if (!Directory.Exists(SavePath))
                {
                    Directory.CreateDirectory(SavePath);
                }

                // Saving the modified Excel file                        
                string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                string file = "Received MC Inprocess ";
                fName = file + "( " + date + " )";
                worksheetTotal.SaveAs(SavePath + @"\" + fName + ".xlsx");
                xlWorkBook.Save();
                xlWorkBook.Close();
                excelApp.Quit();
            }
            catch (Exception ex)
            {
                ErrorText = "មានបញ្ហា!\n"+ex.Message;
                excelApp.DisplayAlerts = false;
                xlWorkBook.Save();
                xlWorkBook.Close();
                excelApp.Quit();
                excelApp.DisplayAlerts = true;
            }
            
            //Kill all Excel background process
            var processes = from p in Process.GetProcessesByName("EXCEL")
                            select p;
            foreach (var process in processes)
            {
                if (process.MainWindowTitle.ToString().Trim() == "")
                    process.Kill();
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                System.Diagnostics.Process.Start(SavePath + @"\" + fName + ".xlsx");
                fName = "";
            }
            else
            {
                EMsg.AlertText = ErrorText;
                EMsg.ShowingMsg();
            }
        }

    }
}
