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

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            
            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Val");
            if (txtCode.Text.Trim() == "")
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
            if (txtItem.Text.Trim() == "")
            {
                string SearchValue = txtItem.Text;
                SearchValue = SearchValue.Replace("*", "%");
                dtSQLCond.Rows.Add("ItemName LIKE ", "'" + SearchValue + "'");
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
                    SQLConds = " AND " + row["Col"] + row["Val"];
                }
            }

            //Taking Data
            dtStockQty = new DataTable();
            dtStockDetails = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQueryDetails = "SELECT T1.*, T4.ItemName, MCName1, MCName2 FROM " +
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
                    "\nWHERE NOT TotalQty=0 ";

                string SQLQueryTotal = "SELECT Code, ItemName, SUM(TotalQty) AS TotalQty FROM (\n"+SQLQueryDetails+"\n) TbDetails " +
                    "\nGROUP BY Code, ItemName " +
                    "\nORDER BY Code ASC";

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
