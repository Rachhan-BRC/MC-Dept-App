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

namespace MachineDeptApp.Inventory
{
    public partial class InventoryCombineForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        //When Search
        DataTable dtInventory;
        public DataTable dtInventorySumByLocCode;

        //When Calc
        DataTable dtInventoryFinalGenerated;
        public DataTable dtOBSBOM;

        DataTable dtLocCode;
        public DataTable dtOBSMstItem;
        string ErrorText;

        public static string CodeSelected;
        public static string NameSelected;
        public static string QtySelected;


        public InventoryCombineForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.dgvInventoryData.CellFormatting += DgvInventoryData_CellFormatting;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnExport.Click += BtnExport_Click;
            this.btnCalculate.Click += BtnCalculate_Click;
            this.dgvCombineData.CellDoubleClick += DgvCombineData_CellDoubleClick;
            this.btnPrint.Click += BtnPrint_Click;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (dgvCombineData.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("តើអ្នកចង់ព្រីនទិន្នន័យចេញមែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    ErrorText = "";
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "CSV file (*.csv)|*.csv";
                    saveDialog.FileName = "Inventory" + DateTime.Now.ToString("yyyyMMddHHmm");
                    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;
                        //All Data
                        try
                        {
                            //Write Column name
                            int columnCount = 0;
                            foreach (DataGridViewColumn DgvCol in dgvInventoryData.Columns)
                            {
                                if (DgvCol.Visible == true)
                                {
                                    columnCount = columnCount + 1;
                                }
                            }
                            string columnNames = "";

                            //String array for Csv
                            string[] outputCsv;
                            outputCsv = new string[dgvInventoryData.Rows.Count + 1];

                            //Set Column Name
                            for (int i = 0; i < columnCount; i++)
                            {
                                if (dgvInventoryData.Columns[i].Visible == true)
                                {
                                    columnNames += dgvInventoryData.Columns[i].HeaderText.ToString() + ",";
                                }
                            }
                            outputCsv[0] += columnNames;

                            //Row of data 
                            for (int i = 1; (i - 1) < dgvInventoryData.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    if (dgvInventoryData.Columns[j].Visible == true)
                                    {
                                        string Value = "";
                                        if (dgvInventoryData.Rows[i - 1].Cells[j].Value != null)
                                        {
                                            Value = dgvInventoryData.Rows[i - 1].Cells[j].Value.ToString();
                                        }
                                        //Fix don't separate if it contain '\n' or ','
                                        Value = "\"" + Value.Replace("\"", "\"\"") + "\"";
                                        outputCsv[i] += Value + ",";
                                    }
                                }
                            }

                            File.WriteAllLines(saveDialog.FileName + "-All.csv", outputCsv, Encoding.UTF8);                            
                        }
                        catch (Exception ex)
                        {
                            if (ErrorText.Trim() == "")
                            {
                                ErrorText = ex.Message;
                            }
                            else
                            {
                                ErrorText += "\n"+ ex.Message;
                            }
                        }
                                                
                        //Combine
                        if (ErrorText.Trim() == "")
                        {
                            try
                            {
                                //Write Column name
                                int columnCount = 0;
                                foreach (DataGridViewColumn DgvCol in dgvCombineData.Columns)
                                {
                                    if (DgvCol.Visible == true)
                                    {
                                        columnCount = columnCount + 1;
                                    }
                                }
                                string columnNames = "";

                                //String array for Csv
                                string[] outputCsv;
                                outputCsv = new string[dgvCombineData.Rows.Count + 1];

                                //Set Column Name
                                for (int i = 0; i < columnCount; i++)
                                {
                                    if (dgvCombineData.Columns[i].Visible == true)
                                    {
                                        columnNames += dgvCombineData.Columns[i].HeaderText.ToString() + ",";
                                    }
                                }
                                outputCsv[0] += columnNames;

                                //Row of data 
                                for (int i = 1; (i - 1) < dgvCombineData.Rows.Count; i++)
                                {
                                    for (int j = 0; j < columnCount; j++)
                                    {
                                        if (dgvCombineData.Columns[j].Visible == true)
                                        {
                                            string Value = "";
                                            if (dgvCombineData.Rows[i - 1].Cells[j].Value != null)
                                            {
                                                Value = dgvCombineData.Rows[i - 1].Cells[j].Value.ToString();
                                            }
                                            //Fix don't separate if it contain '\n' or ','
                                            Value = "\"" + Value.Replace("\"", "\"\"") + "\"";
                                            outputCsv[i] += Value + ",";
                                        }
                                    }
                                }

                                File.WriteAllLines(saveDialog.FileName + "-FinalCombine.csv", outputCsv, Encoding.UTF8);
                            }
                            catch (Exception ex)
                            {
                                if (ErrorText.Trim() == "")
                                {
                                    ErrorText = ex.Message;
                                }
                                else
                                {
                                    ErrorText += "\n" + ex.Message;
                                }
                            }
                        }

                        Cursor = Cursors.Default;

                        if (ErrorText.Trim() == "")
                        {
                            MessageBox.Show("ទាញទិន្នន័យចេញរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void DgvCombineData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (dgvCombineData.Rows[e.RowIndex].Cells[0].Value.ToString() == "MC Inprocess")
                {
                    CodeSelected = dgvCombineData.Rows[e.RowIndex].Cells[1].Value.ToString(); 
                    NameSelected = dgvCombineData.Rows[e.RowIndex].Cells[2].Value.ToString(); 
                    QtySelected = Convert.ToDouble(dgvCombineData.Rows[e.RowIndex].Cells[3].Value.ToString()).ToString("N0");
                    InventoryCombineDetailsForm Icdf = new InventoryCombineDetailsForm(this);
                    Icdf.ShowDialog();
                }
            }
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            DialogResult DSL = MessageBox.Show("តើអ្នកចង់ចាប់ផ្ដើមការគណនាមែនដែរឬទេ?","Rachhan System",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (DSL == DialogResult.Yes)
            {
                Cursor = Cursors.WaitCursor;
                ErrorText = "";
                dtOBSBOM = new DataTable();

                //Take OBS Semi BOM
                try
                {
                    string WIPCodeIN = "";
                    foreach (DataRow row in dtInventorySumByLocCode.Rows)
                    {
                        if (row["ItemType"].ToString() == "Semi")
                        {
                            if (WIPCodeIN.Trim() == "")
                            {
                                WIPCodeIN = " '" + row["ItemCode"].ToString() + "' ";
                            }
                            else
                            {
                                WIPCodeIN += ", '" + row["ItemCode"].ToString() + "' ";
                            }
                        }
                    }
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT UpItemCode, LowItemCode, LowQty FROM mstbom WHERE DelFlag=0 AND UpItemCode IN (" + WIPCodeIN+") ", cnnOBS.conOBS);
                    sda.Fill(dtOBSBOM);

                }
                catch(Exception ex)
                {
                    if (ErrorText.Trim() == "")
                    {
                        ErrorText = ex.Message;
                    }
                    else
                    {
                        ErrorText += "\n" + ex.Message;
                    }
                }
                cnnOBS.conOBS.Close();


                Cursor = Cursors.Default;

                if (ErrorText.Trim() == "")
                {
                    //Calc Total BOM Usage
                    dtOBSBOM.Columns.Add("UsageQty");
                    dtOBSBOM.AcceptChanges();
                    foreach (DataRow row in dtOBSBOM.Rows)
                    {
                        double Total = 0;
                        foreach (DataRow rowInv in dtInventorySumByLocCode.Rows)
                        {
                            if (rowInv["LocName"].ToString() == "MC Inprocess" && rowInv["ItemType"].ToString() == "Semi")
                            {
                                if (row["UpItemCode"].ToString() == rowInv["ItemCode"].ToString())
                                {
                                    Total = Convert.ToDouble(row["LowQty"].ToString()) * Convert.ToDouble(rowInv["TotalQty"].ToString());
                                    Total = Convert.ToDouble(Total.ToString("N0"));
                                    break;
                                }
                            }
                        }
                        row["UsageQty"] = Total;
                    }

                    /*
                    foreach (DataRow row in dtOBSBOM.Rows)
                    {
                        string Cons = "";
                        foreach (DataColumn col in dtOBSBOM.Columns)
                        {
                            if (Cons.Trim() == "")
                            {
                                Cons = row[col].ToString();
                            }
                            else
                            {
                                Cons += "\t"+ row[col].ToString();
                            }
                        }
                        Console.WriteLine(Cons);
                    }
                    */

                    //Add to dtInventoryFinalGenerated
                    dtInventoryFinalGenerated = new DataTable();
                    dtInventoryFinalGenerated.Columns.Add("LocName");
                    dtInventoryFinalGenerated.Columns.Add("ItemCode");
                    dtInventoryFinalGenerated.Columns.Add("TotalQty");
                    dtInventoryFinalGenerated.AcceptChanges();
                    //Add only material
                    foreach (DataRow row in dtInventorySumByLocCode.Rows)
                    {
                        if (row["LocName"].ToString() != "MC Inprocess")
                        {
                            dtInventoryFinalGenerated.Rows.Add(row["LocName"].ToString(), row["ItemCode"].ToString(), row["TotalQty"].ToString());
                        }
                        else
                        {
                            if (row["ItemType"].ToString() != "Semi")
                            {
                                dtInventoryFinalGenerated.Rows.Add(row["LocName"].ToString(), row["ItemCode"].ToString(), row["TotalQty"].ToString());
                            }
                        }
                        dtInventoryFinalGenerated.AcceptChanges();
                    }
                    //Add RM Generate from Semi OBS BOM
                    foreach (DataRow row in dtOBSBOM.Rows)
                    {
                        int Found = 0;
                        foreach (DataRow rowInv in dtInventoryFinalGenerated.Rows)
                        {                            
                            if (rowInv["LocName"].ToString() == "MC Inprocess")
                            {
                                if (row["LowItemCode"].ToString() == rowInv["ItemCode"].ToString())
                                {
                                    rowInv["TotalQty"] = (Convert.ToDouble(rowInv["TotalQty"].ToString()) + Convert.ToDouble(row["UsageQty"].ToString())).ToString();
                                    Found++; 
                                    break;
                                }
                            }
                        }
                        if (Found == 0)
                        {
                            dtInventoryFinalGenerated.Rows.Add("MC Inprocess", row["LowItemCode"].ToString(), row["UsageQty"].ToString());
                        }
                        dtInventoryFinalGenerated.AcceptChanges();
                    }

                    //Add ItemName to dtInventoryFinalGenerated
                    dtInventoryFinalGenerated.Columns.Add("ItemName");
                    foreach (DataRow row in dtInventoryFinalGenerated.Rows)
                    {
                        foreach (DataRow rowOBSItem in dtOBSMstItem.Rows)
                        {
                            if (row["ItemCode"].ToString() == rowOBSItem["ItemCode"].ToString())
                            {
                                row["ItemName"] = rowOBSItem["ItemName"].ToString();
                                break;
                            }
                        }
                        dtInventoryFinalGenerated.AcceptChanges();
                    }

                    //Add ItemName to dtInventorySumByLocCode
                    dtInventorySumByLocCode.Columns.Add("ItemName");
                    foreach (DataRow row in dtInventorySumByLocCode.Rows)
                    {
                        foreach (DataRow rowOBSItem in dtOBSMstItem.Rows)
                        {
                            if (row["ItemCode"].ToString() == rowOBSItem["ItemCode"].ToString())
                            {
                                row["ItemName"] = rowOBSItem["ItemName"].ToString();
                                break;
                            }
                        }
                        dtInventorySumByLocCode.AcceptChanges();
                    }

                    //Sort dtInventoryFinalGenerated
                    // Create a DataView from your DataTable
                    DataView dv = dtInventoryFinalGenerated.DefaultView;
                    // Sort the DataView by LocName and ItemCode in ascending order
                    dv.Sort = "LocName DESC, ItemCode ASC";
                    // Convert the DataView back to a DataTable if needed
                    dtInventoryFinalGenerated = dv.ToTable();

                    //Show to dgvCombineData
                    dgvCombineData.Rows.Clear();
                    foreach (DataRow row in dtInventoryFinalGenerated.Rows)
                    {
                        string LocName = row["LocName"].ToString();
                        string RMCode = row["ItemCode"].ToString();
                        string RMName = row["ItemName"].ToString();
                        double Qty = Convert.ToDouble(row["TotalQty"].ToString());

                        dgvCombineData.Rows.Add(LocName, RMCode, RMName, Qty);
                    }
                    dgvCombineData.ClearSelection();
                    tabControl1.SelectedIndex = 1;
                    btnCalculate.Enabled = false;
                    btnCalculate.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
                    CheckForBtnPrint();
                }
                else
                {
                    MessageBox.Show(ErrorText ,"Rachhan System", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                }

            }
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvInventoryData.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("តើអ្នកចង់ទាញទិន្នន័យចេញមែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "CSV file (*.csv)|*.csv";
                    saveDialog.FileName = "All_Inventory" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;
                        try
                        {
                            //Write Column name
                            int columnCount = 0;
                            foreach (DataGridViewColumn DgvCol in dgvInventoryData.Columns)
                            {
                                if (DgvCol.Visible == true)
                                {
                                    columnCount = columnCount + 1;
                                }
                            }
                            string columnNames = "";

                            //String array for Csv
                            string[] outputCsv;
                            outputCsv = new string[dgvInventoryData.Rows.Count + 1];

                            //Set Column Name
                            for (int i = 0; i < columnCount; i++)
                            {
                                if (dgvInventoryData.Columns[i].Visible == true)
                                {
                                    columnNames += dgvInventoryData.Columns[i].HeaderText.ToString() + ",";
                                }
                            }
                            outputCsv[0] += columnNames;

                            //Row of data 
                            for (int i = 1; (i - 1) < dgvInventoryData.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    if (dgvInventoryData.Columns[j].Visible == true)
                                    {
                                        string Value = "";
                                        if (dgvInventoryData.Rows[i - 1].Cells[j].Value != null)
                                        {
                                            Value = dgvInventoryData.Rows[i - 1].Cells[j].Value.ToString();
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
        private void DgvInventoryData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvInventoryData.Rows[e.RowIndex].Cells[dgvInventoryData.Columns.Count - 1].Value.ToString() == "Deleted")
            {
                dgvInventoryData.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Pink;
            }
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            dgvInventoryData.Rows.Clear();
            Cursor = Cursors.WaitCursor; 
            btnPrint.Enabled = false;
            btnPrint.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            ErrorText = "";

            //Take Inventory Data
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbInventory ORDER BY LabelNo ASC, SeqNo ASC", cnn.con);
                dtInventory = new DataTable();
                sda.Fill(dtInventory);
                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM tbSDMCLocation", cnn.con);
                dtLocCode = new DataTable();
                sda1.Fill(dtLocCode);
                SqlDataAdapter sda2 = new SqlDataAdapter("SELECT LocName, ItemCode, ItemType, SUM(Qty) AS TotalQty FROM " +
                    "(SELECT * FROM tbInventory WHERE CancelStatus=0) T1 " +
                    "LEFT JOIN (SELECT * FROM tbSDMCLocation) T2 " +
                    "ON T1.LocCode = T2.LocCode " +
                    "GROUP BY LocName, ItemCode, ItemType " +
                    "ORDER BY LocName ASC, ItemCode ASC", cnn.con);
                dtInventorySumByLocCode = new DataTable();
                sda2.Fill(dtInventorySumByLocCode);

            }
            catch (Exception ex)
            {
                if (ErrorText.Trim() == "")
                {
                    ErrorText = ex.Message;
                }
                else
                {
                    ErrorText = ErrorText + "\n" + ex.Message;
                }
            }
            cnn.con.Close();

            //Take OBS Mst Items
            try
            {
                cnnOBS.conOBS.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM mstitem WHERE DelFlag=0 AND ItemType IN (1,2) ORDER BY ItemCode ASC", cnnOBS.conOBS);
                dtOBSMstItem = new DataTable();
                sda.Fill(dtOBSMstItem);
            }
            catch ( Exception ex )
            {
                if (ErrorText.Trim() == "")
                {
                    ErrorText = ex.Message;
                }
                else
                {
                    ErrorText = ErrorText + "\n" + ex.Message;
                }
            }
            cnnOBS.conOBS.Close();

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                //Add ItemName to dtInventory
                if (dtInventory.Rows.Count > 0)
                {
                    dtInventory.Columns.Add("ItemName");
                    dtInventory.Columns.Add("LocAsKhmer");
                    dtInventory.AcceptChanges();
                }
                foreach (DataRow row in dtInventory.Rows)
                {
                    foreach (DataRow rowOBS in dtOBSMstItem.Rows)
                    {
                        if (row["ItemCode"].ToString() == rowOBS["ItemCode"].ToString())
                        {
                            row["ItemName"] = rowOBS["ItemName"].ToString();
                            dtInventory.AcceptChanges();
                            break;
                        }
                    }
                }

                //Add LocAsKhmer
                foreach (DataRow row in dtInventory.Rows)
                {
                    foreach (DataRow rowLoc in dtLocCode.Rows)
                    {
                        if (row["LocCode"].ToString() == rowLoc["LocCode"].ToString())
                        {
                            row["LocAsKhmer"] = rowLoc["LocName"].ToString();
                            dtInventory.AcceptChanges();
                            break;
                        }
                    }
                    
                }

                //Add data to dgvInventoryData
                foreach (DataRow row in dtInventory.Rows)
                {
                    string LocName = row["LocAsKhmer"].ToString();
                    string SubLoc = row["SubLoc"].ToString();
                    double LabelNo = Convert.ToDouble(row["LabelNo"].ToString());
                    string CountAs = row["CountingMethod"].ToString();
                    string ItemCode = row["ItemCode"].ToString();
                    string ItemName = row["ItemName"].ToString();
                    double Qty = Convert.ToDouble(row["Qty"].ToString());
                    string Remarks = row["QtyDetails"].ToString();
                    string Remarks2 = "";
                    if (row["QtyDetails2"].ToString().Trim() != "")
                    {
                        if (row["QtyDetails2"].ToString().Contains("|") == true)
                        {
                            string[] Remarks2Array = row["QtyDetails2"].ToString().Split('|');
                            Remarks2 = Remarks2Array[1].ToString() + " Bobbins/Reels";
                        }
                        else
                        {
                            Remarks2 = row["QtyDetails2"].ToString() + " Bobbins/Reels";
                        }
                    }
                    string RegBy = row["RegBy"].ToString();
                    DateTime RegDate = Convert.ToDateTime(row["RegDate"].ToString());
                    string UpdateBy = row["UpdateBy"].ToString(); ;
                    DateTime UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                    string Status = "Active";
                    if (row["CancelStatus"].ToString() == "1")
                    {
                        Status = "Deleted";
                    }

                    dgvInventoryData.Rows.Add(LocName, SubLoc, LabelNo, CountAs, ItemCode, ItemName, Qty, Remarks, Remarks2, RegDate, RegBy, UpdateDate, UpdateBy, Status);

                }
                dgvInventoryData.ClearSelection();
                CheckForBtnCalc();

                LbStatus.Text = "រកឃើញទិន្នន័យ "+dgvInventoryData.Rows.Count.ToString("N0");
                LbStatus.Refresh();
            }
            else
            {
                LbStatus.Text = "មានបញ្ហា!";
                LbStatus.Refresh();
                MessageBox.Show(ErrorText,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        private void CheckForBtnCalc()
        {
            int ActiveQty = 0;
            foreach (DataGridViewRow DgvRow in dgvInventoryData.Rows)
            {
                if (DgvRow.Cells[13].Value.ToString() != "Deleted")
                {
                    ActiveQty++;
                }
            }

            if(ActiveQty > 0)
            {
                btnCalculate.Enabled = true;
                btnCalculate.BackColor = Color.White;
            }
            else
            {
                btnCalculate.Enabled = false;
                btnCalculate.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
            }
        }
        private void CheckForBtnPrint()
        {
            if(dgvCombineData.Rows.Count > 0)
            {
                btnPrint.Enabled = true;
                btnPrint.BackColor = Color.White;
            }
            else
            {
                btnPrint.Enabled = false;
                btnPrint.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
            }
        }

    }
}
