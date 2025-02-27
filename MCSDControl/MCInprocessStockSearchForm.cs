using MachineDeptApp.MsgClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.IO;

namespace MachineDeptApp.MCSDControl
{
    public partial class MCInprocessStockSearchForm : Form
    {
        ErrorMsgClass EMsg = new ErrorMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        SQLConnect cnn = new SQLConnect();
        DataTable dtStockQty;
        DataTable dtStockDetails;

        string ErrorText;

        public MCInprocessStockSearchForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.btnSearch.Click += BtnSearch_Click;
            this.btnExport.Click += BtnExport_Click;
            this.dgvStock.CurrentCellChanged += DgvStock_CurrentCellChanged;

        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dtStockQty.Rows.Count > 0 && dtStockDetails.Rows.Count > 0)
            {
                DataTable dtClone = dtStockDetails.Copy();
                dtClone.Columns.Add("Location", typeof(string));
                foreach (DataRow row in dtClone.Rows)
                {
                    if (row["POSNo"].ToString().Trim() == "")
                    {
                        row["Location"] = "ទិន្នន័យ​ស្តុកនៅសល់";
                    }
                    else
                    {
                        if (row["MCName1"].ToString().Trim() != "")
                        {
                            row["Location"] = row["MCName1"].ToString();
                        }
                        else
                        {
                            row["Location"] = row["MCName2"].ToString();
                        }
                    }
                }
                dtClone.Columns.Remove("MCName1");
                dtClone.Columns.Remove("MCName2");
                dtClone.AcceptChanges();

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV file (*.csv)|*.csv";
                saveDialog.FileName = "MC Inprocess Details.csv";
                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;
                    try
                    {
                        //String array for Csv
                        string[] outputCsv;
                        outputCsv = new string[dtClone.Rows.Count + 1];

                        //Write Column name
                        string columnNames = "";
                        //Set Column Name
                        foreach (DataColumn col in dtClone.Columns)
                        {
                            columnNames += col.ColumnName.ToString() + ",";
                        }
                        outputCsv[0] += columnNames;

                        //Row of data 
                        foreach (DataRow row in dtClone.Rows)
                        {
                            foreach (DataColumn col in dtClone.Columns)
                            {
                                string Value = "";
                                if (row[col.ColumnName].ToString().Trim() != "")
                                {
                                    Value = row[col.ColumnName].ToString();
                                }
                                //Fix don't separate if it contain '\n' or ','
                                Value = "\"" + Value.Replace("\"", "\"\"") + "\"";
                                
                                outputCsv[dtClone.Rows.IndexOf(row) + 1] += Value + ",";
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
                /*
                string ConsoleText = "";
                foreach (DataColumn col in dtClone.Columns)
                {
                    ConsoleText +=col.ColumnName +"\t";
                }
                Console.WriteLine(ConsoleText);
                foreach (DataRow row in dtClone.Rows)
                {
                    ConsoleText = "";
                    foreach (DataColumn col in dtClone.Columns)
                    {
                        ConsoleText += row[col.ColumnName].ToString() + "\t";
                    }
                    Console.WriteLine(ConsoleText);
                }
                */
            }
        }

        private void DgvStock_CurrentCellChanged(object sender, EventArgs e)
        {
            dgvDetails.Rows.Clear();
            if (dgvStock.SelectedCells.Count > 0 && dgvStock.CurrentCell != null && dgvStock.CurrentCell.RowIndex > -1)
            {
                string Code = dgvStock.Rows[dgvStock.CurrentCell.RowIndex].Cells["RMCode"].Value.ToString();
                foreach (DataRow row in dtStockDetails.Rows)
                {
                    if (row["Code"].ToString() == Code)
                    {
                        string DocNo = row["POSNo"].ToString();
                        string Location = "ទិន្នន័យនៅសល់";
                        if (DocNo.Trim() != "")
                        {
                            if (row["MCName1"].ToString().Trim() != "")
                            {
                                Location = row["MCName1"].ToString();
                            }
                            else
                            {
                                Location = row["MCName2"].ToString();
                            }
                        }
                        dgvDetails.Rows.Add();
                        dgvDetails.Rows[dgvDetails.Rows.Count - 1].HeaderCell.Value = dgvDetails.Rows.Count.ToString();
                        dgvDetails.Rows[dgvDetails.Rows.Count - 1].Cells["Location"].Value = Location;
                        dgvDetails.Rows[dgvDetails.Rows.Count - 1].Cells["DocNo"].Value = DocNo;
                        dgvDetails.Rows[dgvDetails.Rows.Count-1].Cells["Qty"].Value = Convert.ToDouble(row["TotalQty"].ToString());
                    }
                }
                dgvDetails.ClearSelection();
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            dgvStock.Rows.Clear();
            
            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Val");
            if (txtCode.Text.Trim() != "")
            {
                string SearchValue = txtCode.Text;
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*","%");
                    dtSQLCond.Rows.Add("Code LIKE ", "'"+SearchValue+"'");
                }
                else
                {
                    dtSQLCond.Rows.Add("Code = ", "'" + SearchValue + "'");
                }
            }
            if (txtItem.Text.Trim() != "")
            {
                string SearchValue = txtItem.Text;
                dtSQLCond.Rows.Add("ItemName LIKE ", "'%" + SearchValue + "%'");
            }
            string SQLConds = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                if (SQLConds.Trim() == "")
                {
                    SQLConds = "AND " + row["Col"] + row["Val"];
                }
                else
                {
                    SQLConds = " AND " + row["Col"] + row["Val"];
                }
            }

            //Taking Data
            dtStockQty = new DataTable();
            dtStockDetails = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQueryDetails = "SELECT Code, ItemName, POSNo, TotalQty, MCName1, MCName2 FROM " +
                    "\n(SELECT Code, POSNo, SUM(StockValue) AS TotalQty FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='MC1' GROUP BY Code, POSNo) T1 " +
                    "\nLEFT JOIN (SELECT SysNo, MCName AS MCName1 FROM tbSDAllocateStock GROUP BY SysNo, MCName) T2 ON T1.POSNo=T2.SysNo " +
                    "\nLEFT JOIN (SELECT PosCNo," +
                    "\nNULLIF(CONCAT(MC1Name, " +
                    "\nCASE " +
                    "\n\tWHEN LEN(MC2Name)>1 THEN ' & ' " +
                    "\n\tELSE '' " +
                    "\nEND, MC2Name, " +
                    "\nCASE " +
                    "\n\tWHEN LEN(MC3Name)>1 THEN ' & ' " +
                    "\n\tELSE '' " +
                    "\nEND, MC3Name),'') AS MCName2 FROM tbPOSDetailofMC) T3 ON T1.POSNo = T3.PosCNo " +
                    "\nLEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') T4 ON T1.Code=T4.ItemCode" +
                    "\nWHERE NOT TotalQty=0 "+ SQLConds;
                Console.WriteLine(SQLQueryDetails + "\nORDER BY Code ASC, POSNo ASC");

                string SQLQueryTotal = "SELECT Code, ItemName, SUM(TotalQty) AS TotalQty FROM (\n"+SQLQueryDetails+"\n) TbDetails " +
                    "\nGROUP BY Code, ItemName " +
                    "\nORDER BY Code ASC";
                //Console.WriteLine(SQLQueryTotal);

                SqlDataAdapter sda = new SqlDataAdapter(SQLQueryDetails + "\nORDER BY Code ASC, POSNo ASC", cnn.con);
                sda.Fill(dtStockDetails);

                sda = new SqlDataAdapter(SQLQueryTotal, cnn.con);
                sda.Fill(dtStockQty);

            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

            //Add to dgv
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtStockQty.Rows)
                {
                    dgvStock.Rows.Add();
                    dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["RMCode"].Value = row["Code"].ToString();
                    dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["RMName"].Value = row["ItemName"].ToString();
                    dgvStock.Rows[dgvStock.Rows.Count-1].Cells["TotalQty"].Value = Convert.ToDouble(row["TotalQty"].ToString());
                }
                dgvStock.ClearSelection();
                dgvStock.CurrentCell = null;
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                LbStatus.Text = "រកឃើញទិន្នន័យ "+ dgvStock.Rows.Count.ToString("N0");
                LbStatus.Refresh();
            }
            else
            {
                EMsg.AlertText = "មានបញ្ហា!\n"+ErrorText;
                EMsg.ShowingMsg();
            }
        }

    }
}
