using MachineDeptApp.MsgClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.NG_Input
{
    public partial class MstBOMForm : Form
    {
        ErrorMsgClass EMsg = new ErrorMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        SQLConnect cnn = new SQLConnect();
        string ErrorText;

        public MstBOMForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Shown += MstBOMForm_Shown;
            this.btnSearch.Click += BtnSearch_Click;
            this.dgvSemi.CurrentCellChanged += DgvSemi_CurrentCellChanged;

        }

        private void DgvSemi_CurrentCellChanged(object sender, EventArgs e)
        {
            dgvConsump.Rows.Clear();
            if (dgvSemi.SelectedCells.Count > 0 && dgvSemi.CurrentCell != null && dgvSemi.CurrentCell.RowIndex > -1)
            {
                TakingConsumption();
            }
        }
        private void MstBOMForm_Shown(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn col in dgvSemi.Columns)
            {
                //Console.WriteLine(col.Name);
            }
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            ErrorText = "";
            LbStatus.Text = "កំពុងស្វែងរក​ . . . .";
            LbStatus.Refresh();
            dgvSemi.Rows.Clear();
            dgvConsump.Rows.Clear();

            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Val");
            if (txtCode.Text.Trim() != "")
            {
                string SearchValue = txtCode.Text;
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*","%");
                    dtSQLCond.Rows.Add("ItemCode LIKE ", "'"+SearchValue+"'");
                }
                else
                {
                    dtSQLCond.Rows.Add("ItemCode = ", "'" + SearchValue + "'");
                }
            }
            if (txtItemname.Text.Trim() != "")
            {
                string SearchValue = txtItemname.Text;
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*", "%");
                }
                else
                {
                    SearchValue = "%" + SearchValue + "%";
                }
                dtSQLCond.Rows.Add("ItemName LIKE ", "'" + SearchValue + "'");
            }
            string SQLConds = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                SQLConds += " AND " + row["Col"] + row["Val"];
            }

            //Taking Data
            DataTable dtSearchResult = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT UpItemCode, ItemName, Remarks1, Remarks2, Remarks3, LastUpdate FROM " +
                    "\n(SELECT UpItemCode, Max(UpdateDate) AS LastUpdate FROM MstBOM GROUP BY UpItemCode) T1 " +
                    "\nINNER JOIN(SELECT ItemCode, ItemName, Remarks1, Remarks2, Remarks3, ItemType FROM tbMasterItem) T2 ON T1.UpItemCode = T2.ItemCode " +
                    "\nWHERE ItemType ='Work In Process' "+SQLConds+" " +
                    "\nORDER BY UpItemCode ASC";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtSearchResult);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

            //Add to Dgv
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtSearchResult.Rows)
                {
                    dgvSemi.Rows.Add();
                    dgvSemi.Rows[dgvSemi.Rows.Count - 1].Cells["WIPCode"].Value = row["UpItemCode"].ToString();
                    dgvSemi.Rows[dgvSemi.Rows.Count - 1].Cells["WIPName"].Value = row["ItemName"].ToString();
                    dgvSemi.Rows[dgvSemi.Rows.Count - 1].Cells["PIN"].Value = row["Remarks1"].ToString();
                    dgvSemi.Rows[dgvSemi.Rows.Count - 1].Cells["Wire"].Value = row["Remarks2"].ToString();
                    dgvSemi.Rows[dgvSemi.Rows.Count - 1].Cells["Length"].Value = row["Remarks3"].ToString();
                    dgvSemi.Rows[dgvSemi.Rows.Count - 1].Cells["LastUpdate"].Value = Convert.ToDateTime(row["LastUpdate"].ToString());
                }
            }
            
            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                dgvSemi.ClearSelection();
                dgvSemi.CurrentCell = null;
                LbStatus.Text = "រកឃើញទិន្នន័យ " + dgvSemi.Rows.Count.ToString("N0");
                LbStatus.Refresh();
            }
            else
            {
                EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                EMsg.ShowingMsg();
            }
        }

        //Function
        private void TakingConsumption()
        {
            Cursor = Cursors.WaitCursor;
            ErrorText = "";

            //Taking Data
            DataTable dtConsumption = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT UpItemCode, LowItemCode, ItemName, LowQty, Yield FROM " +
                    "\n(SELECT * FROM MstBOM) T1 " +
                    "\nLEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') T2 ON T1.LowItemCode = T2.ItemCode " +
                    "\nWHERE UpItemCode = '" + dgvSemi.Rows[dgvSemi.CurrentCell.RowIndex].Cells["WIPCode"].Value.ToString() +"' " +
                    "\nORDER BY UpItemCode ASC, LowItemCode ASC";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtConsumption);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

            //Add to Dgv
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtConsumption.Rows)
                {
                    dgvConsump.Rows.Add();
                    dgvConsump.Rows[dgvConsump.Rows.Count - 1].HeaderCell.Value = dgvConsump.Rows.Count.ToString();
                    dgvConsump.Rows[dgvConsump.Rows.Count - 1].Cells["RMCode"].Value = row["LowItemCode"].ToString();
                    dgvConsump.Rows[dgvConsump.Rows.Count - 1].Cells["RMName"].Value = row["ItemName"].ToString();
                    dgvConsump.Rows[dgvConsump.Rows.Count - 1].Cells["Qty"].Value = Convert.ToDouble(row["LowQty"].ToString());
                    dgvConsump.Rows[dgvConsump.Rows.Count - 1].Cells["Rate"].Value = Convert.ToDouble(row["Yield"].ToString()) / 100;
                }
                dgvConsump.ClearSelection();
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
            {
                EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                EMsg.ShowingMsg();
            }
        }

    }
}
