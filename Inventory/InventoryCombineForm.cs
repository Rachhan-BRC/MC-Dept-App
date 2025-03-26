using MachineDeptApp.MsgClass;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;

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
                    string SQLQuery = "SELECT SubLoc, LabelNo, ItemCode, QtyDetails AS DocumentNo, Qty FROM tbInventory " +
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
                        "\nLEFT JOIN (SELECT SysNo, POSNo FROM tbSDAllocateStock INNER JOIN (SELECT SD_DocNo FROM tbBobbinRecords WHERE In_Date IS NULL GROUP BY SD_DocNo)T3_1 ON tbSDAllocateStock.SysNo=T3_1.SD_DocNo) T3 ON T1.POSNo=T3.POSNo " +
                        "\nINNER JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') T4 ON T2.LowItemCode = T4.ItemCode " +
                        "\nLEFT JOIN (SELECT Code FROM tbSDMCAllTransaction WHERE CancelStatus = 0 AND LocCode = 'MC1' AND ReceiveQty>0 AND POSNo LIKE 'SD%' GROUP BY Code) T5 ON T2.LowItemCode = T5.Code " +
                        "\nLEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Work In Process') T6 ON T1.ItemCode=T6.ItemCode";
                    sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtSemiDetails);
                    SQLQuery = "SELECT LowItemCode, DocumentNo, ROUND(SUM(TotalQty),2) AS TotalQty  FROM " +
                        "\n("+SQLQuery+ "\n) TbSemi " +
                        "\nGROUP BY LowItemCode, DocumentNo " +
                        "\nORDER BY DocumentNo ASC, LowItemCode ASC";
                    sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtSemi);

                    //SD
                    SQLQuery = "SELECT SubLoc, LabelNo, QtyDetails AS DocumentNo, ItemCode, Qty FROM tbInventory " +
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
                        "\nEND AS DocumentNo, POSNo, RMCode, Qty FROM tbNGInprocess " +
                        "\nLEFT JOIN (SELECT SysNo AS SDNo, T1.POSNo  FROM " +
                        "\n\t(SELECT *  FROM tbSDAllocateStock) T1 INNER JOIN (SELECT POSNo, MIN(RegDate) AS RegDate FROM tbSDAllocateStock GROUP BY POSNo) T2 ON T1.POSNo=T2.POSNo AND T1.RegDate=T2.RegDate) TbSDAlloc ON tbNGInprocess.PosCNo=TbSDAlloc.POSNo " +
                        "\nLEFT JOIN (SELECT Code FROM tbSDMCAllTransaction WHERE CancelStatus = 0 AND LocCode = 'MC1' AND ReceiveQty>0 AND POSNo LIKE 'SD%' GROUP BY Code) T3 ON RMCode = T3.Code " +
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
                    double SysQty = Convert.ToDouble(Convert.ToDouble(row["TotalQty"]).ToString("N0"));
                    double POSQty = Convert.ToDouble(row["POS"]);
                    double SemiQty = Convert.ToDouble(Convert.ToDouble(row["Semi"]).ToString("N0"));
                    double SDQty = Convert.ToDouble(row["SD"]);
                    double NGQty = Convert.ToDouble(Convert.ToDouble(row["NG"]).ToString("N0"));
                    double GAP = (POSQty+ SemiQty + SDQty + NGQty) - SysQty;

                    dgvInprocess.Rows.Add();
                    dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["MC1Name"].Value = MC1Name;
                    dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["MCName"].Value = MCName;
                    dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["DocumentNo"].Value = DocumentNo;
                    dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["RMCode"].Value = RMCode;
                    dgvInprocess.Rows[dgvInprocess.Rows.Count - 1].Cells["RMName"].Value = RMName;
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
