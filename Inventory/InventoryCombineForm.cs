using MachineDeptApp.MsgClass;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MachineDeptApp.Inventory
{
    public partial class InventoryCombineForm : Form
    {
        InformationMsgClass InfoMsg = new InformationMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        ErrorMsgClass EMsg = new ErrorMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();

        DataTable dtPOSDetails;
        DataTable dtSemiDetails;
        DataTable dtSDDetails;
        DataTable dtNGDetails;

        string ErrorText = "";

        public InventoryCombineForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Shown += InventoryCombineForm_Shown;
            this.btnPrint.EnabledChanged += BtnPrint_EnabledChanged;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnPrint.Click += BtnPrint_Click;
            this.btnExport.Click += BtnExport_Click;

            //Inprocess
            this.dgvInprocess.CellPainting += DgvInprocess_CellPainting;
            this.dgvInprocess.CellDoubleClick += DgvInprocess_CellDoubleClick;

        }

        //Inprocess
        private void DgvInprocess_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                DataTable dtForNext = new DataTable();
                string Type = dgvInprocess.Columns[e.ColumnIndex].Name.ToString();
                if (Type.Contains("POS") == true)
                {
                    Type = "POS";
                    dtForNext = dtPOSDetails.Copy();
                }
                else if (Type.Contains("Semi") == true)
                {
                    Type = "Semi";
                    dtForNext = dtSemiDetails.Copy();
                }
                else if (Type.Contains("SD") == true)
                {
                    Type = "SD";
                    dtForNext = dtSDDetails.Copy();
                }
                else if (Type.Contains("NG") == true)
                {
                    Type = "NG";
                    dtForNext = dtNGDetails.Copy();
                }
                else
                {
                    Type = "";
                }

                if (Type.Trim() != "")
                {
                    InventoryCombineDetailsForm Icdf = new InventoryCombineDetailsForm(Type, dgvInprocess, dtForNext);
                    Icdf.ShowDialog();
                }
            }
        }
        private void DgvInprocess_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (dgvInprocess.Columns[e.ColumnIndex].Name == "GAP")
                {
                    if (Convert.ToDouble(dgvInprocess.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == 0)
                    {
                        e.CellStyle.Font = new Font(e.CellStyle.Font,FontStyle.Bold);
                        e.CellStyle.ForeColor = Color.Green;
                    }
                    else
                    {
                        if (Convert.ToDouble(dgvInprocess.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) < 0)
                        {
                            e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                            e.CellStyle.ForeColor = Color.Red;
                        }
                        else
                        {
                            e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                            e.CellStyle.ForeColor = Color.Orange;
                        }
                    }
                }
            }
        }

        private void BtnPrint_EnabledChanged(object sender, EventArgs e)
        {
            if (btnPrint.Enabled == true)
            {
                btnPrintGRAY.SendToBack();
            }
            else
            {
                btnPrintGRAY.BringToFront();
            }
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            DataGridView dgvExport = new DataGridView();
            if (tabLocation.SelectedTab.Name == "PageInprocess")
            {
                dgvExport = dgvInprocess;
            }
            if (dgvExport.Rows.Count > 0)
            {
                QMsg.QAText = "Do want to export this data?";
                QMsg.UserClickedYes = false;
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "CSV file (*.csv)|*.csv";
                    saveDialog.FileName = "MC Inventory-Export.csv";
                    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        ErrorText = "";
                        Cursor = Cursors.WaitCursor;

                        try
                        {
                            //String array for Csv
                            string[] outputCsv;
                            outputCsv = new string[dgvExport.Rows.Count + 1];

                            //Write Column name
                            string columnNames = "";
                            //Set Column Name
                            foreach (DataGridViewColumn col in dgvExport.Columns)
                            {
                                if (col.Visible == true)
                                {
                                    columnNames += col.HeaderText.ToString() + ",";
                                }
                            }
                            outputCsv[0] += columnNames;

                            //Row of data 
                            foreach (DataGridViewRow row in dgvExport.Rows)
                            {
                                foreach (DataGridViewColumn col in dgvExport.Columns)
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
                                        outputCsv[row.Index + 1] += Value + ",";
                                    }
                                }
                            }
                            File.WriteAllLines(saveDialog.FileName, outputCsv, Encoding.UTF8);

                        }
                        catch (Exception ex)
                        {
                            ErrorText = ex.Message;
                        }

                        Cursor = Cursors.Default;

                        if (ErrorText.Trim() == "")
                        {
                            InfoMsg.InfoText = "ទាញទិន្នន័យចេញរួចរាល់!";
                            InfoMsg.ShowingMsg();
                        }
                        else
                        {
                            EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                            EMsg.ShowingMsg();
                        }
                    }
                }
            }
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (tabLocation.SelectedTab.Name == "PageInprocess")
            {
                PrintInprocessData();
            }
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (tabLocation.SelectedTab.Name == "PageInprocess")
            {
                TakingInprocessData();
            }
            CheckBtnPrint();
        }
        private void InventoryCombineForm_Shown(object sender, EventArgs e)
        {
            tabLocation.TabPages.RemoveByKey("PageKIT");
            tabLocation.TabPages.RemoveByKey("PageSD");
            btnPrint.Enabled = false;
            //Taking MC Name
            DataTable dtMCName = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT MCName FROM tbMasterMCType " +
                                            "\nORDER BY MCType ASC, MCName ASC";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtMCName);
            }
            catch (Exception ex)
            {
                ErrorText = "Taking MC Name : " + ex.Message;
            }
            cnn.con.Close();

            if (ErrorText.Trim() == "")
            {
                //Remove for MS01 Duplicate
                for (int i = dtMCName.Rows.Count-1; i >= 0; i--)
                {
                    int Count = 0;
                    foreach (DataRow row in dtMCName.Rows)
                    {
                        if (dtMCName.Rows[i]["MCName"].ToString() == row["MCName"].ToString())
                        {
                            Count++;
                            if (Count > 1)
                            {
                                dtMCName.Rows.RemoveAt(i);
                                dtMCName.AcceptChanges();
                                break;
                            }
                        }
                    }
                }

                //Add to Cbo
                foreach (DataRow row in dtMCName.Rows)
                    CboMCName.Items.Add(row["MCName"].ToString());

                //MC1Name
                //MCName
                //DocumentNo
                //RMCode
                //RMName
                //SystemQty
                //POSQty
                //SemiQty
                //SDQty
                //NGQty
                //GAP
                foreach (DataGridViewColumn col in dgvInprocess.Columns)
                {
                    if (col.Name == "SystemQty")
                    {
                        col.HeaderCell.Style.BackColor = Color.Orange;
                    }
                    if (col.Name == "POSQty" || col.Name == "SemiQty" || col.Name == "SDQty" || col.Name == "NGQty")
                    {
                        col.HeaderCell.Style.BackColor = Color.Yellow;
                    }
                    if (col.Name == "GAP")
                    {
                        col.HeaderCell.Style.BackColor = Color.Red;
                        col.HeaderCell.Style.ForeColor = Color.White;
                    }
                    //Console.WriteLine(col.Name);                
                }
            }
            else
            {
                EMsg.AlertText = "Something's wrong!\n"+ErrorText;
                EMsg.ShowingMsg();
            }
        }

        //Method
        //Inprocess 
        private void TakingInprocessData()
        {
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "Searching . . .";
            LbStatus.Refresh();
            dgvInprocess.Rows.Clear();
            dtPOSDetails = new DataTable();
            dtSemiDetails = new DataTable();
            dtSDDetails = new DataTable();
            dtNGDetails = new DataTable();

            //SQLCondition
            string SQLConds = "";
            if (CboMCName.Text.Trim() != "")
            {
                string SearchValue = "(MCName1 LIKE '%" + CboMCName.Text + "%' OR MCName2 LIKE '%" + CboMCName.Text + "%') ";
                SQLConds += "AND "+SearchValue;
            }
            if (txtDocNo.Text.Trim() != "")
            {
                string SearchValue = txtDocNo.Text;
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*","%");
                    SearchValue = "DocumentNo LIKE '"+SearchValue+"' ";
                }
                else
                {
                    SearchValue = "DocumentNo = '" + SearchValue + "' ";
                }

                //For Other Stock
                if (SearchValue.ToUpper().Contains("STOCK") == true)
                {
                    SearchValue = "(TRIM(DocumentNo)='' OR DocumentNo IS NULL) ";
                }
                SQLConds += "AND " + SearchValue;
            }
            if (txtRMCode.Text.Trim() != "")
            {
                string SearchValue = txtRMCode.Text;
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*", "%");
                    SearchValue = "Code LIKE '" + SearchValue + "' ";
                }
                else
                {
                    SearchValue = "Code = '" + SearchValue + "' ";
                }
                SQLConds += "AND " + SearchValue;
            }
            if (txtRMName.Text.Trim() != "")
            {
                string SearchValue = "ItemName LIKE '%" + txtRMName.Text + "%' ";
                SQLConds += "AND " + SearchValue;
            }

            //Take CurrentStock add MC1
            DataTable dtCurrentStock = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT Code, ItemName, RMTypeName, DocumentNo, " +
                    "\nCASE " +
                    "\n\tWHEN MCName1 IS NOT NULL AND MCName2 IS NULL THEN MCName1 " +
                    "\n\tWHEN MCName1 IS NULL AND MCName2 IS NOT NULL THEN MCName2 " +
                    "\n\tELSE NULL " +
                    "\nEND AS MCName, TotalQty, 0 AS POS, 0.00 AS Semi, 0 AS SD, 0.00 AS NG FROM " +
                    "\n(SELECT Code, POSNo AS DocumentNo, ROUND(SUM(StockValue),0) AS TotalQty FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='MC1' GROUP BY Code, POSNo) T1 " +
                    "\nLEFT JOIN (SELECT SysNo, MCName AS MCName1 FROM tbSDAllocateStock GROUP BY SysNo, MCName) T2 ON T1.DocumentNo=T2.SysNo " +
                    "\nLEFT JOIN (SELECT PosCNo, " +
                    "\nNULLIF(CONCAT(MC1Name,  " +
                    "\nCASE " +
                    "\n\tWHEN LEN(MC2Name)>1 THEN ' & ' " +
                    "\n\tELSE '' " +
                    "\nEND, MC2Name, " +
                    "\nCASE " +
                    "\n\tWHEN LEN(MC3Name)>1 THEN ' & ' " +
                    "\n\tELSE '' " +
                    "\nEND, MC3Name),'') AS MCName2 FROM tbPOSDetailofMC) T3 ON T1.DocumentNo = T3.PosCNo " +
                    "\nLEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') T4 ON T1.Code=T4.ItemCode " +
                    "\nWHERE TotalQty <> 0 " + SQLConds +
                    "\nORDER BY Code ASC, DocumentNo ASC";
                //Console.WriteLine(SQLQuery);
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtCurrentStock);
            }
            catch (Exception ex)
            {
                ErrorText = "Take CurrentStock : " + ex.Message;
            }
            cnn.con.Close();

            //Taking POS, Semi, SD
            DataTable dtPOS = new DataTable();
            DataTable dtSemi = new DataTable();
            DataTable dtSD = new DataTable();
            DataTable dtNG = new DataTable();
            if (ErrorText.Trim() == "")
            {
                try
                {
                    cnn.con.Open();
                    //POS
                    string SQLQuery = "SELECT SubLoc, LabelNo, tbInventory.ItemCode, ItemName, QtyDetails AS DocumentNo, Qty FROM tbInventory " +
                        "\nLEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') T1_1 ON tbInventory.ItemCode=T1_1.ItemCode " +
                        "\nWHERE LocCode='MC1' AND CancelStatus = 0 AND CountingMethod='POS' ";
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtPOSDetails);
                    SQLQuery = "SELECT ItemCode, DocumentNo, SUM(Qty) AS TotalQty FROM " +
                                        "\n(" + SQLQuery + ") T1"+
                                        "\nGROUP BY ItemCode, DocumentNo";
                    sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtPOS);

                    //Semi
                    SQLQuery = "SELECT SubLoc, LabelNo, T6.ItemName AS WIPName, Qty AS WIPQty, " +
                        "\nCASE " +
                        "\n\tWHEN T5.Code IS NULL THEN T1.POSNo " +
                        "\n\tELSE T3.SysNo " +
                        "\nEND AS DocumentNo, T1.POSNo, T2.LowItemCode, T4.ItemName, (T2.LowQty*Qty) As TotalQty FROM " +
                        "\n(SELECT SubLoc, LabelNo, LEFT(QtyDetails2, CHARINDEX('|', QtyDetails2) - 1) AS POSNo, ItemCode, Qty FROM tbInventory WHERE LocCode='MC1' AND CancelStatus = 0 AND CountingMethod = 'Semi') T1 " +
                        "\nINNER JOIN (SELECT * FROM MstBOM) T2 ON T1.ItemCode=T2.UpItemCode " +
                        "\nLEFT JOIN (SELECT SysNo, POSNo FROM tbSDAllocateStock) T3 ON T1.POSNo=T3.POSNo " +
                        "\nINNER JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') T4 ON T2.LowItemCode = T4.ItemCode " +
                        "\nLEFT JOIN (SELECT Code FROM tbSDMCAllTransaction WHERE CancelStatus = 0 AND LocCode = 'MC1' AND ReceiveQty>0 AND POSNo LIKE 'SD%' GROUP BY Code) T5 ON T2.LowItemCode = T5.Code " +
                        "\nLEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Work In Process') T6 ON T1.ItemCode=T6.ItemCode";
                    //Console.WriteLine(SQLQuery);
                    sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtSemiDetails);
                    SQLQuery = "SELECT LowItemCode, DocumentNo, ROUND(SUM(TotalQty),2) AS TotalQty  FROM " +
                        "\n("+SQLQuery+ "\n) TbSemi " +
                        "\nGROUP BY LowItemCode, DocumentNo " +
                        "\nORDER BY DocumentNo ASC, LowItemCode ASC";
                    sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtSemi);

                    //SD
                    SQLQuery = "SELECT SubLoc, LabelNo, QtyDetails AS DocumentNo, tbInventory.ItemCode, ItemName, Qty FROM tbInventory " +
                        "\nLEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') T1_1 ON tbInventory.ItemCode=T1_1.ItemCode " +
                        "\nWHERE LocCode = 'MC1' AND CancelStatus = 0 AND CountingMethod = 'SD Document' ";
                    sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtSDDetails);
                    SQLQuery = "SELECT ItemCode, DocumentNo, SUM(Qty) AS TotalQty FROM " +
                        "\n("+SQLQuery+ ") T1 " +
                        "\nGROUP BY ItemCode, DocumentNo " +
                        "\nORDER BY ItemCode ASC, DocumentNo ASC";
                    sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtSD);

                    //NG
                    SQLQuery = "SELECT MCSeqNo, " +
                        "\nCASE " +
                        "\n\tWHEN Code IS NULL THEN PosCNo " +
                        "\n\tELSE SDNo " +
                        "\nEND AS DocumentNo, POSNo, RMCode, ItemName, Qty FROM tbNGInprocess " +
                        "\nLEFT JOIN (SELECT SysNo AS SDNo, T1.POSNo  FROM " +
                        "\n\t(SELECT *  FROM tbSDAllocateStock) T1 INNER JOIN (SELECT POSNo, MIN(RegDate) AS RegDate FROM tbSDAllocateStock GROUP BY POSNo) T2 ON T1.POSNo=T2.POSNo AND T1.RegDate=T2.RegDate) TbSDAlloc ON tbNGInprocess.PosCNo=TbSDAlloc.POSNo " +
                        "\nLEFT JOIN (SELECT Code FROM tbSDMCAllTransaction WHERE CancelStatus = 0 AND LocCode = 'MC1' AND ReceiveQty>0 AND POSNo LIKE 'SD%' GROUP BY Code) T3 ON RMCode = T3.Code " +
                        "\nLEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') T4 ON RMCode=T4.ItemCode " +
                        "\nWHERE ReqStatus = 0 AND Qty<>0";
                    sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtNGDetails);
                    SQLQuery = "SELECT RMCode, DocumentNo, SUM(Qty) AS TotalQty FROM " +
                        "\n(\n"+SQLQuery+ "\n) tbNG " +
                        "\nGROUP BY RMCode, DocumentNo " +
                        "\nORDER BY RMCode ASC, DocumentNo ASC";
                    sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtNG);
                }
                catch(Exception ex)
                {
                    ErrorText = "Taking POS, Semi, SD : " + ex.Message;
                }
                cnn.con.Close();
            }

            //Calculate/Combine CurrentStock, POS, Semi, SD
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtCurrentStock.Rows)
                {
                    string RMCode = row["Code"].ToString();
                    string DocNo = row["DocumentNo"].ToString();
                    double POSQty = 0;
                    double SemiQty = 0;
                    double SDQty = 0;
                    double NGQty = 0;

                    //POS
                    foreach (DataRow rowPOS in dtPOS.Rows)
                    {
                        if (RMCode == rowPOS["ItemCode"].ToString() && DocNo == rowPOS["DocumentNo"].ToString())
                        {
                            POSQty += Convert.ToDouble(rowPOS["TotalQty"]);
                        }
                    }

                    //Semi
                    foreach (DataRow rowSemi in dtSemi.Rows)
                    {
                        if (RMCode == rowSemi["LowItemCode"].ToString() && DocNo == rowSemi["DocumentNo"].ToString())
                        {
                            SemiQty += Convert.ToDouble(rowSemi["TotalQty"]);
                        }
                    }

                    //SD
                    foreach (DataRow rowSD in dtSD.Rows)
                    {
                        if (RMCode == rowSD["ItemCode"].ToString() && DocNo == rowSD["DocumentNo"].ToString())
                        {
                            SDQty += Convert.ToDouble(rowSD["TotalQty"]);
                        }
                    }

                    //NG
                    foreach (DataRow rowNG in dtNG.Rows)
                    {
                        if (RMCode == rowNG["RMCode"].ToString() && DocNo == rowNG["DocumentNo"].ToString())
                        {
                            NGQty += Convert.ToDouble(rowNG["TotalQty"]);
                        }
                    }

                    row["POS"] = POSQty;
                    row["Semi"] = SemiQty;
                    row["SD"] = SDQty;
                    row["NG"] = NGQty;
                    dtCurrentStock.AcceptChanges();
                }
            }

            //Console
            /*
            //Print column headers
            foreach (DataColumn col in dtCurrentStock.Columns)
            {
                Console.Write(col.ColumnName + "\t");
            }
            Console.WriteLine();
            //Print rows
            foreach (DataRow row in dtCurrentStock.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    Console.Write(item + "\t");
                }
                Console.WriteLine();
            }
            */

            //Add to DGV
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtCurrentStock.Rows)
                {
                    string MCName = row["MCName"].ToString();
                    string MC1Name = MCName;
                    if (MC1Name.Contains(" & ") == true)
                    {
                        string[] splitText = MC1Name.Split('&');
                        MC1Name = splitText[0].ToString();
                    }
                    string DocumentNo = row["DocumentNo"].ToString();
                    if (DocumentNo.Trim() == "")
                        DocumentNo = "Other Stock";
                    string RMCode = row["Code"].ToString();
                    string RMName = row["ItemName"].ToString();
                    string RMType = row["RMTypeName"].ToString();
                    double SysQty = Convert.ToDouble(Convert.ToDouble(row["TotalQty"]).ToString("N0"));
                    double POSQty = Convert.ToDouble(row["POS"]);
                    double SemiQty = Convert.ToDouble(Convert.ToDouble(row["Semi"]).ToString("N0"));
                    double SDQty = Convert.ToDouble(row["SD"]);
                    double NGQty = Convert.ToDouble(Convert.ToDouble(row["NG"]).ToString("N0"));
                    double GAP = (POSQty+ SemiQty + SDQty + NGQty) - SysQty;

                    /*
                    if (DocumentNo != "Other Stock")
                    {
                        dgvInprocess.Rows.Add();
                        dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["MC1Name"].Value = MC1Name;
                        dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["MCName"].Value = MCName;
                        dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["DocumentNo"].Value = DocumentNo;
                        dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["RMCode"].Value = RMCode;
                        dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["RMName"].Value = RMName;
                        dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["RMType"].Value = RMType;
                        dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["SystemQty"].Value = SysQty;
                        dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["POSQty"].Value = POSQty;
                        dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["SemiQty"].Value = SemiQty;
                        dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["SDQty"].Value = SDQty;
                        dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["NGQty"].Value = NGQty;
                        dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["GAP"].Value = GAP;
                    }
                    */

                    dgvInprocess.Rows.Add();
                    dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["MC1Name"].Value = MC1Name;
                    dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["MCName"].Value = MCName;
                    dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["DocumentNo"].Value = DocumentNo;
                    dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["RMCode"].Value = RMCode;
                    dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["RMName"].Value = RMName;
                    dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["RMType"].Value = RMType;
                    dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["SystemQty"].Value = SysQty;
                    dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["POSQty"].Value = POSQty;
                    dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["SemiQty"].Value = SemiQty;
                    dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["SDQty"].Value = SDQty;
                    dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["NGQty"].Value = NGQty;
                    dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["GAP"].Value = GAP;

                }
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                LbStatus.Text = "Found  " + dgvInprocess.Rows.Count.ToString("N0") + "  data";
                LbStatus.Refresh();
                dgvInprocess.ClearSelection();
            }
            else
            {
                LbStatus.Text = "Something's wrong!";
                LbStatus.Refresh();
                EMsg.AlertText = "Something's wrong!\n" + ErrorText;
                EMsg.ShowingMsg();
            }
        }
        private void PrintInprocessData()
        {
            if (dgvInprocess.Rows.Count > 0)
            {
                QMsg.QAText = "Do you want to print?";
                QMsg.UserClickedYes = false;
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;
                    LbStatus.Text = "Print excel, please wait patiently!";
                    LbStatus.Refresh();

                    //Create New table
                    DataTable dtTotalCombine = new DataTable();
                    dtTotalCombine.Columns.Add("RMCode");
                    dtTotalCombine.Columns.Add("RMName");
                    dtTotalCombine.Columns.Add("RMType");
                    dtTotalCombine.Columns.Add("InventoryQty");
                    dtTotalCombine.Columns.Add("SystemQty");
                    dtTotalCombine.Columns.Add("GAPQty");
                    foreach (DataGridViewRow row in dgvInprocess.Rows)
                    {
                        string RMCode = row.Cells["RMCode"].Value.ToString();
                        string RMName = row.Cells["RMName"].Value.ToString();
                        string RMType = row.Cells["RMType"].Value.ToString();
                        double POSQty = 0;
                        double SemiQty = 0;
                        double SDQty = 0;
                        double NGQty = 0;
                        double GAPQty = 0;
                        double SystemQty = 0;
                        double CountDupl = 0; 

                        foreach (DataRow rowDt in dtTotalCombine.Rows)
                        {
                            if (RMCode == rowDt["RMCode"].ToString())
                            {
                                CountDupl++;
                                break;
                            }
                        }

                        if (CountDupl == 0)
                        {
                            foreach (DataGridViewRow rowSum in dgvInprocess.Rows)
                            {
                                if (RMCode == rowSum.Cells["RMCode"].Value.ToString())
                                {
                                    POSQty += Convert.ToDouble(rowSum.Cells["POSQty"].Value);
                                    SemiQty += Convert.ToDouble(rowSum.Cells["SemiQty"].Value);
                                    SDQty += Convert.ToDouble(rowSum.Cells["SDQty"].Value);
                                    NGQty += Convert.ToDouble(rowSum.Cells["NGQty"].Value);
                                    SystemQty += Convert.ToDouble(rowSum.Cells["SystemQty"].Value);
                                    GAPQty += Convert.ToDouble(rowSum.Cells["GAP"].Value);
                                }
                            }
                            dtTotalCombine.Rows.Add();
                            dtTotalCombine.Rows[dtTotalCombine.Rows.Count-1]["RMCode"] = RMCode;
                            dtTotalCombine.Rows[dtTotalCombine.Rows.Count - 1]["RMName"] = RMName;
                            dtTotalCombine.Rows[dtTotalCombine.Rows.Count - 1]["RMType"] = RMType;
                            dtTotalCombine.Rows[dtTotalCombine.Rows.Count - 1]["InventoryQty"] = "="+POSQty.ToString()+"+"+SemiQty.ToString()+"+"+SDQty.ToString()+"+"+NGQty.ToString();
                            dtTotalCombine.Rows[dtTotalCombine.Rows.Count - 1]["SystemQty"] = SystemQty;
                            dtTotalCombine.Rows[dtTotalCombine.Rows.Count - 1]["GAPQty"] = GAPQty;
                        }

                    }

                    //Console
                    /*
                    //Print column headers
                    foreach (DataColumn col in dtTotalCombine.Columns)
                    {
                        Console.Write(col.ColumnName + "\t");
                    }
                    Console.WriteLine();
                    //Print rows
                    foreach (DataRow row in dtTotalCombine.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            Console.Write(item + "\t");
                        }
                        Console.WriteLine();
                    }
                    */

                    //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                    string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\Inventory\Report";
                    string fName = "";
                    if (!Directory.Exists(SavePath))
                    {
                        Directory.CreateDirectory(SavePath);
                    }
                    try
                    {

                        //open excel application and create new workbook
                        Excel.Application excelApp = new Excel.Application();
                        Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: Environment.CurrentDirectory + @"\Template\Inventory-ReportTemplate.xlsx", Editable: true);
                        Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Inprocess"];
                        Excel.Worksheet WsPOS = (Excel.Worksheet)xlWorkBook.Sheets["Inprocess-POS"];
                        Excel.Worksheet WsSemi = (Excel.Worksheet)xlWorkBook.Sheets["Inprocess-Semi"];
                        Excel.Worksheet WsSD = (Excel.Worksheet)xlWorkBook.Sheets["Inprocess-SD"];
                        Excel.Worksheet WsNG = (Excel.Worksheet)xlWorkBook.Sheets["Inprocess-NG"];

                        Excel.Worksheet DeleteWs1 = (Excel.Worksheet)xlWorkBook.Sheets["KIT"];
                        Excel.Worksheet DeleteWs2 = (Excel.Worksheet)xlWorkBook.Sheets["SD"];

                        double MatchingCount = 0;
                        double OverCount = 0;
                        double MinusCount = 0;

                        //Inprocess-Combine
                        //Insert Row
                        if (dtTotalCombine.Rows.Count > 1)
                        {
                            worksheet.Range["9:" + (dtTotalCombine.Rows.Count + 7)].Insert();
                            worksheet.Range["A9:F" + (dtTotalCombine.Rows.Count + 7)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        }
                        //Add Data
                        foreach (DataRow row in dtTotalCombine.Rows)
                        {
                            string RMCode = row["RMCode"].ToString();
                            string RMDescription = row["RMName"].ToString();
                            string RMType = row["RMType"].ToString();
                            string Inventory = row["InventoryQty"].ToString();
                            double Sys = Convert.ToDouble(row["SystemQty"].ToString());
                            double GAP = Convert.ToDouble(row["GAPQty"].ToString());

                            worksheet.Cells[dtTotalCombine.Rows.IndexOf(row)+8, 1] = RMCode;
                            worksheet.Cells[dtTotalCombine.Rows.IndexOf(row) + 8, 2] = RMDescription;
                            worksheet.Cells[dtTotalCombine.Rows.IndexOf(row) + 8, 3] = RMType;
                            worksheet.Cells[dtTotalCombine.Rows.IndexOf(row) + 8, 4] = Inventory;
                            worksheet.Cells[dtTotalCombine.Rows.IndexOf(row) + 8, 5] = Sys;
                            worksheet.Cells[dtTotalCombine.Rows.IndexOf(row) + 8, 6] = GAP;

                            if (GAP == 0)
                            {
                                MatchingCount++;
                            }
                            else
                            {
                                if (GAP < 0)
                                {
                                    MinusCount++;
                                }
                                else
                                {
                                    OverCount++;
                                }
                            }
                        }
                        //Header
                        // Loop through all shapes in the worksheet for set Text
                        double TotalItems = dtTotalCombine.Rows.Count;
                        foreach (Excel.Shape shape in worksheet.Shapes)
                        {
                            //Date
                            if (shape.Name == "DateShape")
                            {
                                // Set the text for the shape
                                shape.TextFrame.Characters().Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
                            }
                            //Location
                            if (shape.Name == "LocationShape")
                            {
                                shape.TextFrame.Characters().Text = "MC Inprocess";
                            }

                            //TotalItemsShape, MatchingShape, OverShape, MinusShape
                            if (shape.Type == Microsoft.Office.Core.MsoShapeType.msoGroup) // Check if the shape is a group
                            {
                                foreach (Excel.Shape childShape in shape.GroupItems) // Iterate through the grouped items
                                {
                                    //TotalItemsShape
                                    if (childShape.Name == "TotalItemsShape") // Check for your shape by name
                                    {
                                        childShape.TextFrame.Characters().Text = TotalItems.ToString("N0");
                                    }

                                    //Matching
                                    if (childShape.Name == "MatchingQtyShape")
                                    {
                                        // Set the text for the childShape
                                        childShape.TextFrame.Characters().Text = MatchingCount.ToString("N0");
                                    }
                                    if (childShape.Name == "Matching%Shape")
                                    {
                                        // Set the text for the childShape
                                        childShape.TextFrame.Characters().Text = ((MatchingCount/TotalItems) * 100).ToString("N2") + " %";
                                    }

                                    //Over
                                    if (childShape.Name == "OverQtyShape")
                                    {
                                        // Set the text for the childShape
                                        childShape.TextFrame.Characters().Text = OverCount.ToString("N0");
                                    }
                                    if (childShape.Name == "Over%Shape")
                                    {
                                        // Set the text for the childShape
                                        childShape.TextFrame.Characters().Text = ((OverCount / TotalItems) * 100).ToString("N2") + " %";
                                    }

                                    //Minus
                                    if (childShape.Name == "MinusQtyShape")
                                    {
                                        // Set the text for the childShape
                                        childShape.TextFrame.Characters().Text = MinusCount.ToString("N0");
                                    }
                                    if (childShape.Name == "Minus%Shape")
                                    {
                                        // Set the text for the childShape
                                        childShape.TextFrame.Characters().Text = ((MinusCount / TotalItems) * 100).ToString("N2") + " %";
                                    }
                                }
                            }

                        }


                        //Inprocess-POS
                        //Insert Row
                        if (dtPOSDetails.Rows.Count > 1)
                        {
                            WsPOS.Range["3:" + (dtPOSDetails.Rows.Count + 1)].Insert();
                            WsPOS.Range["A3:F" + (dtPOSDetails.Rows.Count + 1)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        }
                        //Add Data
                        foreach (DataRow row in dtPOSDetails.Rows)
                        {
                            string Loc = row["SubLoc"].ToString();
                            string LabelNo = row["LabelNo"].ToString(); 
                            string RMCode = row["ItemCode"].ToString();
                            string RMDescription = row["ItemName"].ToString();
                            string DocumentNo = row["DocumentNo"].ToString();
                            double Qty = Convert.ToDouble(row["Qty"].ToString());

                            WsPOS.Cells[dtPOSDetails.Rows.IndexOf(row) + 2, 1] = Loc;
                            WsPOS.Cells[dtPOSDetails.Rows.IndexOf(row) + 2, 2] = LabelNo;
                            WsPOS.Cells[dtPOSDetails.Rows.IndexOf(row) + 2, 3] = RMCode;
                            WsPOS.Cells[dtPOSDetails.Rows.IndexOf(row) + 2, 4] = RMDescription;
                            WsPOS.Cells[dtPOSDetails.Rows.IndexOf(row) + 2, 5] = DocumentNo;
                            WsPOS.Cells[dtPOSDetails.Rows.IndexOf(row) + 2, 6] = Qty;

                        }


                        //Inprocess-Semi
                        //Insert Row
                        if (dtSemiDetails.Rows.Count > 1)
                        {
                            WsSemi.Range["3:" + (dtSemiDetails.Rows.Count + 1)].Insert();
                            WsSemi.Range["A3:I" + (dtSemiDetails.Rows.Count + 1)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        }
                        //Add Data
                        foreach (DataRow row in dtSemiDetails.Rows)
                        {
                            string Loc = row["SubLoc"].ToString();
                            string LabelNo = row["LabelNo"].ToString();
                            string WIPName = row["WIPName"].ToString(); 
                            double WIPQty = Convert.ToDouble(row["WIPQty"].ToString());
                            string DocumentNo = row["DocumentNo"].ToString();
                            string POSNo = row["POSNo"].ToString();				
                            string RMCode = row["LowItemCode"].ToString();
                            string RMDescription = row["ItemName"].ToString();
                            double UsageQty = Convert.ToDouble(row["TotalQty"].ToString());

                            WsSemi.Cells[dtSemiDetails.Rows.IndexOf(row) + 2, 1] = Loc;
                            WsSemi.Cells[dtSemiDetails.Rows.IndexOf(row) + 2, 2] = LabelNo;
                            WsSemi.Cells[dtSemiDetails.Rows.IndexOf(row) + 2, 3] = WIPName;
                            WsSemi.Cells[dtSemiDetails.Rows.IndexOf(row) + 2, 4] = WIPQty;
                            WsSemi.Cells[dtSemiDetails.Rows.IndexOf(row) + 2, 5] = DocumentNo;
                            WsSemi.Cells[dtSemiDetails.Rows.IndexOf(row) + 2, 6] = POSNo;
                            WsSemi.Cells[dtSemiDetails.Rows.IndexOf(row) + 2, 7] = RMCode;
                            WsSemi.Cells[dtSemiDetails.Rows.IndexOf(row) + 2, 8] = RMDescription;
                            WsSemi.Cells[dtSemiDetails.Rows.IndexOf(row) + 2, 9] = UsageQty;

                        }


                        //Inprocess-SD
                        //Insert Row
                        if (dtSDDetails.Rows.Count > 1)
                        {
                            WsSD.Range["3:" + (dtSDDetails.Rows.Count + 1)].Insert();
                            WsSD.Range["A3:F" + (dtSDDetails.Rows.Count + 1)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        }
                        //Add Data
                        foreach (DataRow row in dtSDDetails.Rows)
                        {
                            string Loc = row["SubLoc"].ToString();
                            string LabelNo = row["LabelNo"].ToString();
                            string DocumentNo = row["DocumentNo"].ToString();
                            string RMCode = row["ItemCode"].ToString();
                            string RMDescription = row["ItemName"].ToString();
                            double Qty = Convert.ToDouble(row["Qty"].ToString());

                            WsSD.Cells[dtSDDetails.Rows.IndexOf(row) + 2, 1] = Loc;
                            WsSD.Cells[dtSDDetails.Rows.IndexOf(row) + 2, 2] = LabelNo;
                            WsSD.Cells[dtSDDetails.Rows.IndexOf(row) + 2, 3] = DocumentNo;
                            WsSD.Cells[dtSDDetails.Rows.IndexOf(row) + 2, 4] = RMCode;
                            WsSD.Cells[dtSDDetails.Rows.IndexOf(row) + 2, 5] = RMDescription;
                            WsSD.Cells[dtSDDetails.Rows.IndexOf(row) + 2, 6] = Qty;

                        }


                        //Inprocess-NG
                        //Insert Row
                        if (dtNGDetails.Rows.Count > 1)
                        {
                            WsNG.Range["3:" + (dtNGDetails.Rows.Count + 1)].Insert();
                            WsNG.Range["A3:F" + (dtNGDetails.Rows.Count + 1)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        }
                        //Add Data
                        foreach (DataRow row in dtNGDetails.Rows)
                        {
                            string MCNo = row["MCSeqNo"].ToString();
                            string DocumentNo = row["DocumentNo"].ToString();
                            string POSNo = row["POSNo"].ToString();
                            string RMCode = row["RMCode"].ToString();
                            string RMDescription = row["ItemName"].ToString();
                            double Qty = Convert.ToDouble(row["Qty"].ToString());

                            WsNG.Cells[dtNGDetails.Rows.IndexOf(row) + 2, 1] = MCNo;
                            WsNG.Cells[dtNGDetails.Rows.IndexOf(row) + 2, 3] = DocumentNo;
                            WsNG.Cells[dtNGDetails.Rows.IndexOf(row) + 2, 2] = POSNo;
                            WsNG.Cells[dtNGDetails.Rows.IndexOf(row) + 2, 4] = RMCode;
                            WsNG.Cells[dtNGDetails.Rows.IndexOf(row) + 2, 5] = RMDescription;
                            WsNG.Cells[dtNGDetails.Rows.IndexOf(row) + 2, 6] = Qty;

                        }

                        // Saving the modified Excel file
                        string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                        string file = "MC Inventory-Report ";
                        fName = file + "( " + date + " )";


                        //លុប Manual worksheet
                        excelApp.DisplayAlerts = false;
                        DeleteWs1.Delete();
                        DeleteWs2.Delete();
                        excelApp.DisplayAlerts = true;


                        //Save
                        worksheet.Name = "Inprocess-Combine";
                        worksheet.SaveAs(SavePath + @"\" + fName + ".xlsx");

                        //Close workbook for keep original format
                        excelApp.DisplayAlerts = false;
                        xlWorkBook.Close();
                        excelApp.DisplayAlerts = true;
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
                    catch (Exception ex)
                    {
                        ErrorText = ex.Message;
                    }                    
                    
                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        LbStatus.Text = "Print excel successfully!";
                        LbStatus.Refresh();
                        InfoMsg.InfoText = "Print excel successfully!";
                        InfoMsg.ShowingMsg();
                        System.Diagnostics.Process.Start(SavePath + @"\" + fName + ".xlsx");
                    }
                    else
                    {
                        EMsg.AlertText = "Something's wrong!\n" + ErrorText;
                        EMsg.ShowingMsg();
                    }
                }
            }
        }


        private void CheckBtnPrint()
        {
            DataGridView dgvChecking = new DataGridView();
            if (tabLocation.SelectedTab.Name == "PageInprocess")
            {
                dgvChecking = dgvInprocess;
            }
            if (dgvChecking.Rows.Count > 0)
                btnPrint.Enabled = true;
            else
                btnPrint.Enabled = false;
        }
    }
}
