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

namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    public partial class WireCalcForProductionSearch : Form
    {
        SQLConnect cnn = new SQLConnect();

        string ErrorText;

        public WireCalcForProductionSearch()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.btnSearch.Click += BtnSearch_Click;
            this.dgvSearchResult.CurrentCellChanged += DgvSearchResult_CurrentCellChanged;
        }

        private void DgvSearchResult_CurrentCellChanged(object sender, EventArgs e)
        {
            dgvPOSList.Rows.Clear();
            dgvRMList.Rows.Clear();
            if (dgvSearchResult.SelectedCells.Count > 0 && dgvSearchResult.CurrentCell != null && dgvSearchResult.CurrentCell.RowIndex > -1)
            {
                ErrorText = "";
                Cursor = Cursors.WaitCursor;

                //Taking POS Data & RM Data
                DataTable dtPOSList = new DataTable();
                DataTable dtRMList = new DataTable();
                try
                {
                    cnn.con.Open();
                    //POS Data
                    string SQLQuery = "SELECT " +
                        "\nNULLIF(CONCAT(T2.MC1Name, " +
                        "\n\tCASE " +
                        "\n\t\tWHEN LEN(T2.MC2Name)>1 THEN ' & '  " +
                        "\n\t\tELSE ''  " +
                        "\n\t\tEND, T2.MC2Name,  " +
                        "\n\tCASE  " +
                        "\n\t\tWHEN LEN(T2.MC3Name)>1 THEN ' & '  " +
                        "\n\t\tELSE ''  " +
                        "\n\tEND, T2.MC3Name),'') AS MCName, " +
                        "\nWIPCode, ItemName, PosCNo, Remarks1, Remarks2, Remarks3, PosCQty FROM " +
                        "\n(SELECT * FROM tbSDAllocateStock) T1 " +
                        "\nINNER JOIN (SELECT * FROM tbPOSDetailofMC) T2 ON T1.POSNo=T2.PosCNo " +
                        "\nLEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType = 'Work In Process') T3 ON T2.WIPCode=T3.ItemCode " +
                        "\nWHERE T1.SysNo = '" + dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells["DocNo"].Value.ToString() +"' ";
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery,cnn.con);
                    sda.Fill(dtPOSList);

                    //RM Data
                    SQLQuery = "SELECT * FROM tbSDMCAllTransaction WHERE LocCode='MC1' AND Funct=2 AND CancelStatus=0 AND Remarks='"+ dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells["DocNo"].Value.ToString() + "' " +
                        "\nORDER BY Code ASC  ";
                    sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtRMList);

                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                }
                cnn.con.Close();

                //Add to dgv
                if (ErrorText.Trim()=="" && dtPOSList.Rows.Count > 0)
                {
                    foreach (DataRow row in dtPOSList.Rows)
                    {
                        dgvPOSList.Rows.Add();
                        dgvPOSList.Rows[dgvPOSList.Rows.Count - 1].HeaderCell.Value = dgvPOSList.Rows.Count.ToString();
                        dgvPOSList.Rows[dgvPOSList.Rows.Count - 1].Cells["MCName"].Value = row["MCName"].ToString();
                        dgvPOSList.Rows[dgvPOSList.Rows.Count - 1].Cells["WIPCode"].Value = row["WIPCode"].ToString();
                        dgvPOSList.Rows[dgvPOSList.Rows.Count - 1].Cells["WIPName"].Value = row["ItemName"].ToString();
                        dgvPOSList.Rows[dgvPOSList.Rows.Count - 1].Cells["POSNo"].Value = row["PosCNo"].ToString();
                        dgvPOSList.Rows[dgvPOSList.Rows.Count - 1].Cells["PIN"].Value = row["Remarks1"].ToString();
                        dgvPOSList.Rows[dgvPOSList.Rows.Count - 1].Cells["Wire"].Value = row["Remarks2"].ToString();
                        dgvPOSList.Rows[dgvPOSList.Rows.Count - 1].Cells["Length"].Value = row["Remarks3"].ToString();
                        dgvPOSList.Rows[dgvPOSList.Rows.Count - 1].Cells["Qty"].Value = Convert.ToDouble(row["PosCQty"].ToString());
                    }
                    dgvPOSList.ClearSelection();
                }
                if (ErrorText.Trim() == "" && dtRMList.Rows.Count > 0)
                {
                    foreach (DataRow row in dtRMList.Rows)
                    {
                        dgvRMList.Rows.Add();
                        dgvRMList.Rows[dgvRMList.Rows.Count - 1].HeaderCell.Value = dgvRMList.Rows.Count.ToString();
                        dgvRMList.Rows[dgvRMList.Rows.Count - 1].Cells["RMCode"].Value = row["Code"].ToString();
                        dgvRMList.Rows[dgvRMList.Rows.Count - 1].Cells["RMName"].Value = row["RMDes"].ToString();
                        dgvRMList.Rows[dgvRMList.Rows.Count - 1].Cells["TransferQty"].Value = Convert.ToDouble(row["ReceiveQty"].ToString());
                    }
                    dgvRMList.ClearSelection();
                }

                Cursor = Cursors.Default;

                if (ErrorText.Trim() != "")
                {
                    MessageBox.Show("មានបញ្ហា!\n"+ErrorText,"Rachhan System",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            dgvSearchResult.Rows.Clear();            
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . . ";
            LbStatus.Refresh();

            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Val");
            if (txtCode.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("Code = ", "'"+txtCode.Text+"'");
            }
            if (txtName.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("RMDes LIKE ", "'%" + txtName.Text + "%'");
            }
            if (txtPOSNo.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("T1.POSNo LIKE ", "'%" + txtPOSNo.Text + "%'");
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

            //Taking data
            DataTable dtSearchResult = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT T1.SysNo, T1.RegDate, T1.RegBy FROM " +
                    "\n(SELECT * FROM tbSDAllocateStock) T1 " +
                    "\nINNER JOIN (SELECT * FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='MC1' AND Funct=2 AND Remarks LIKE 'SD%') T2 ON T1.SysNo=T2.Remarks " + SQLConds+
                    "\nGROUP BY T1.SysNo, T1.RegDate, T1.RegBy " +
                    "\nORDER BY T1.RegDate ASC";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtSearchResult);
            }
            catch (Exception ex)
            {
                ErrorText= ex.Message;
            }
            cnn.con.Close();

            //Add DGV
            foreach (DataRow row in dtSearchResult.Rows)
            {
                dgvSearchResult.Rows.Add();
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["DocNo"].Value = row["SysNo"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["RegDate"].Value = Convert.ToDateTime(row["RegDate"].ToString());
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["RegBy"].Value = row["RegBy"].ToString();
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
                MessageBox.Show("មានបញ្ហា!\n"+ ErrorText,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

    }
}
