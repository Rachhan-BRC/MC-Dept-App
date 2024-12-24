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
    public partial class WireStockINFormAdd : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        WireStockINForm fgrid;
       
        string ErrorText;

        public WireStockINFormAdd(WireStockINForm fg)
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.fgrid = fg;
            this.Shown += WireStockINFormAdd_Shown;

            //Btn
            this.btnSearch.Click += BtnSearch_Click;
            this.btnSelectAll.Click += BtnSelectAll_Click;
            this.btnUnSelectAll.Click += BtnUnSelectAll_Click;
            this.btnOK.Click += BtnOK_Click;

            //Dgv
            this.dgvSearchResult.CellClick += DgvSearchResult_CellClick;


        }

        //Dgv  
        private void DgvSearchResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSearchResult.Columns[e.ColumnIndex].Name == "ChkForPrint" && e.RowIndex>-1)
            {
                if (dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper() == "TRUE")
                {
                    dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                }
                else
                {
                    dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                }
                dgvSearchResult.ClearSelection();
            }
            CheckBtnOK();
        }


        //Btn
        private void BtnOK_Click(object sender, EventArgs e)
        {
            TakingAlreadyRecQty();            
        }
        private void BtnUnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                dgvRow.Cells["ChkForPrint"].Value = false;
            }
            CheckBtnOK();
        }
        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                dgvRow.Cells["ChkForPrint"].Value = true;
            }
            CheckBtnOK();
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            dgvSearchResult.Rows.Clear();
            LbStatus.Text = "កំពុងស្វែងរក . . . ";
            LbStatus.Refresh();
            Cursor = Cursors.WaitCursor;

            //SQL WHERE Condition
            DataTable dtSQLConds = new DataTable();
            dtSQLConds.Columns.Add("Col");
            dtSQLConds.Columns.Add("Val");
            dtSQLConds.Rows.Add("T1.CreateDate BETWEEN ","'"+dtpRegFrom.Value.ToString("yyyy-MM-dd")+" 00:00:00' AND '"+dtpRegTo.Value.ToString("yyyy-MM-dd") + " 23:59:59'");
            if (txtRMCode.Text.Trim() != "")
            {
                dtSQLConds.Rows.Add("T1.ItemCode LIKE ", "'%"+txtRMCode.Text+"%'");
            }
            if (txtRMName.Text.Trim() != "")
            {
                dtSQLConds.Rows.Add("ItemName LIKE ", "'%" + txtRMName.Text + "%'");
            }
            if (CboType.Text.ToString() != "ទាំងអស់")
            {
                if (CboType.Text.ToString() == "រាប់មិនបាន")
                {
                    dtSQLConds.Rows.Add("T2.MatCalcFlag = ", "1");
                }
                else
                {
                    dtSQLConds.Rows.Add("T2.MatCalcFlag = ", "0");
                }
            }
            if (txtDocNo.Text.Trim() != "")
            {
                dtSQLConds.Rows.Add("T1.Remark LIKE ", "'%" + txtDocNo.Text + "%'");
            }
            string SQLConds = "";
            foreach (DataRow row in dtSQLConds.Rows)
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

            //Taking OBS Data
            DataTable dtSearchResult = new DataTable();
            try
            {
                cnnOBS.conOBS.Open();
                string SQLQuery = "SELECT T1.ItemCode, ItemName, T1.Remark, SUM(ReceiveQty) AS ReceiveQty FROM " +
                    "\n(SELECT ItemCode, ReceiveQty, Remark, CreateDate FROM prgalltransaction WHERE TypeCode=1 AND LocCode='MC1' AND GRICode=50) T1 " +
                    "\nINNER JOIN (SELECT * FROM mstitem WHERE DelFlag=0 AND ItemType=2) T2 " +
                    "\nON T1.ItemCode=T2.ItemCode \n" + SQLConds +
                    "\nGROUP BY T1.ItemCode, ItemName, T1.Remark " +
                    "\nORDER BY T1.Remark ASC, T1.ItemCode ASC";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery,cnnOBS.conOBS);
                sda.Fill(dtSearchResult);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnnOBS.conOBS.Close();

            //Remove Data Already have in DGV
            for (int i = dtSearchResult.Rows.Count - 1; i > -1; i--)
            {
                foreach (DataGridViewRow dgvRow in fgrid.dgvScanned.Rows)
                {
                    if (dtSearchResult.Rows[i]["ItemCode"].ToString() == dgvRow.Cells["RMCode"].Value.ToString() &&
                        dtSearchResult.Rows[i]["Remark"].ToString() == dgvRow.Cells["DocNo"].Value.ToString())
                    {
                        dtSearchResult.Rows.RemoveAt(i);
                        dtSearchResult.AcceptChanges();
                        break;
                    }
                }
            }

            //Add to DGV
            foreach (DataRow row in dtSearchResult.Rows)
            {
                dgvSearchResult.Rows.Add();
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["ChkForPrint"].Value = false;
                string RMCode = row["ItemCode"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count-1].Cells["RMCode"].Value = RMCode;
                string RMName = row["ItemName"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["RMName"].Value = RMName;
                string Remark = row["Remark"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Remark"].Value = Remark;
                double Qty = Convert.ToDouble(row["ReceiveQty"].ToString());
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Qty"].Value = Qty;
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                dgvSearchResult.ClearSelection();
                dgvSearchResult.CurrentCell = null;
                LbStatus.Text = "រកឃើញទិន្នន័យ "+ dgvSearchResult.Rows.Count.ToString("N0");
                LbStatus.Refresh();
            }
            else
            {
                MessageBox.Show("មានបញ្ហា!\n"+ErrorText,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void WireStockINFormAdd_Shown(object sender, EventArgs e)
        {
            CboType.SelectedIndex = 0;
        }

        //Method
        private void CheckBtnOK()
        {
            int FoundChecked = 0;
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                if (dgvRow.Cells["ChkForPrint"].Value.ToString().ToUpper() == "TRUE")
                {
                    FoundChecked++;
                    break;
                }
            }
            if(FoundChecked == 0)
            {
                btnOK.Enabled = false;
                btnOKGRAY.BringToFront();
            }
            else
            {
                btnOK.Enabled = true;
                btnOKGRAY.SendToBack();
            }
        }
        private void TakingAlreadyRecQty()
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;

            //Set Doc No for SQL
            string DocIN = "";
            string RMCodeIN = "";
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                if (dgvRow.Cells["ChkForPrint"].Value.ToString().ToUpper() == "TRUE")
                {
                    if (DocIN.Trim() == "")
                    {
                        DocIN = "'"+ dgvRow.Cells["Remark"].Value.ToString() + "'";
                    }
                    else
                    {
                        DocIN += ", '" + dgvRow.Cells["Remark"].Value.ToString() + "'";
                    }

                    if (RMCodeIN.Trim() == "")
                    {
                        RMCodeIN = "'" + dgvRow.Cells["RMCode"].Value.ToString() + "'";
                    }
                    else
                    {
                        RMCodeIN += ", '" + dgvRow.Cells["RMCode"].Value.ToString() + "'";
                    }

                }
            }
            //Console.WriteLine(DocIN);
            //Console.WriteLine(RMCodeIN);

            //Taking RecQty
            DataTable dtRecQty = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT Code, Remarks, SUM(ReceiveQty) AS ReceiveQty from tbSDMCAllTransaction " +
                    "\nWHERE CancelStatus = 0 AND Funct =1 AND LocCode='WIR1' " +
                    "\n AND Remarks IN ("+DocIN+") " +
                    "\n AND Code IN ("+RMCodeIN+") " +
                    "\n GROUP BY Code, Remarks ";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery,cnn.con);
                sda.Fill(dtRecQty);
            }
            catch(Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
                {
                    if (dgvRow.Cells["ChkForPrint"].Value.ToString().ToUpper() == "TRUE")
                    {
                        string RMCode = dgvRow.Cells["RMCode"].Value.ToString();
                        string RMName = dgvRow.Cells["RMName"].Value.ToString();
                        string DocNo = dgvRow.Cells["Remark"].Value.ToString();
                        double Qty = Convert.ToDouble(dgvRow.Cells["Qty"].Value.ToString());
                        double RecQty = 0;
                        foreach (DataRow row in dtRecQty.Rows)
                        {
                            if (row["Code"].ToString() == RMCode && row["Remarks"].ToString() == DocNo)
                            {
                                RecQty = Convert.ToDouble(row["ReceiveQty"].ToString());
                                break;
                            }
                        }
                        fgrid.dgvScanned.Rows.Add(RMCode, RMName, Qty, RecQty, DocNo);
                        fgrid.AssignNumber();
                    }
                }
                fgrid.dgvScanned.ClearSelection();
                this.Close();
            }
            else
            {
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

    }
}
