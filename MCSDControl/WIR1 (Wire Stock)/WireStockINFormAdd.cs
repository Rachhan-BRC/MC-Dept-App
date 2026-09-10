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
            List<string> SQLCondList = new List<string>();
            List<SqlParameter> SQLParams = new List<SqlParameter>();

            SQLCondList.Add("CAST(tbT.CreateDate AS date) = @RegDate");
            SQLParams.Add(new SqlParameter("@RegDate", dtpRegDate.Value.Date));

            if (dtpRegTime.Checked)
            {
                SQLCondList.Add("CAST(tbT.CreateDate AS time) >= @RegTime");
                SQLParams.Add(new SqlParameter("@RegTime", dtpRegTime.Value.ToString("HH:mm")));
            }
            if (txtRMCode.Text.Trim() != "")
            {
                SQLCondList.Add("tbT.ItemCode LIKE @RMCode");
                SQLParams.Add(new SqlParameter("@RMCode", "%" + txtRMCode.Text.Trim() + "%"));
            }
            if (txtRMName.Text.Trim() != "")
            {
                SQLCondList.Add("tbI.ItemName LIKE @RMName");
                SQLParams.Add(new SqlParameter("@RMName", "%" + txtRMName.Text.Trim() + "%"));
            }
            if (CboType.Text.ToString() != "ទាំងអស់")
            {
                SQLCondList.Add("tbI.MatCalcFlag = @MatCalcFlag");
                SQLParams.Add(new SqlParameter("@MatCalcFlag", CboType.Text.ToString() == "រាប់មិនបាន" ? 1 : 0));
            }
            if (txtDocNo.Text.Trim() != "")
            {
                SQLCondList.Add("tbT.Remark LIKE @DocNo");
                SQLParams.Add(new SqlParameter("@DocNo", "%" + txtDocNo.Text.Trim() + "%"));
            }

            string SQLConds = "WHERE " + string.Join(" AND ", SQLCondList);

            //Taking OBS Data
            DataTable dtSearchResult = new DataTable();
            try
            {
                cnnOBS.conOBS.Open();
                string SQLQuery = @"SELECT tbT.*, tbI.ItemName FROM 
                    ( 
	                    SELECT ItemCode, Remark, SUM(ReceiveQty) AS ReceiveQty, 
		                    DATEADD(MINUTE, DATEDIFF(MINUTE, 0, CreateDate), 0) AS CreateDate 
	                    FROM prgalltransaction  WHERE TypeCode = 1 AND LocCode = 'MC1' AND GRICode = 50 
	                    GROUP BY ItemCode, Remark, DATEADD(MINUTE, DATEDIFF(MINUTE, 0, CreateDate), 0) 
                    ) tbT 
                    INNER JOIN mstitem tbI ON tbT.ItemCode = tbI.ItemCode AND tbI.DelFlag = 0 AND tbI.ItemType = 2 
                    "+SQLConds+@" 
                    ORDER BY tbT.Remark, tbT.CreateDate ASC, tbT.ItemCode ASC ";
                //Console.WriteLine(SQLQuery);
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery,cnnOBS.conOBS);
                sda.SelectCommand.Parameters.AddRange(SQLParams.ToArray());
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
                DataRow searchRow = dtSearchResult.Rows[i];
                string ItemCode = searchRow["ItemCode"].ToString();
                string Remark = searchRow["Remark"].ToString();
                DateTime RegDate = Convert.ToDateTime(searchRow["CreateDate"].ToString());
                foreach (DataGridViewRow dgvRow in fgrid.dgvScanned.Rows)
                {
                    if (ItemCode == dgvRow.Cells["RMCode"].Value.ToString() &&
                        Remark == dgvRow.Cells["DocNo"].Value.ToString() &&
                        RegDate == Convert.ToDateTime(dgvRow.Cells["RegDate"].Value))
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
                DataGridViewRow NewRow = dgvSearchResult.Rows[dgvSearchResult.Rows.Add()];
                string RMCode = row["ItemCode"].ToString();
                string RMName = row["ItemName"].ToString();
                string Remark = row["Remark"].ToString();
                double Qty = Convert.ToDouble(row["ReceiveQty"].ToString());

                NewRow.Cells["ChkForPrint"].Value = false;
                NewRow.Cells["RMCode"].Value = RMCode;
                NewRow.Cells["RMName"].Value = RMName;
                NewRow.Cells["Remark"].Value = Remark;
                NewRow.Cells["Qty"].Value = Qty;
                NewRow.Cells["RegDate"].Value = Convert.ToDateTime(row["CreateDate"]);
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
                        DateTime RegDate = Convert.ToDateTime(dgvRow.Cells["RegDate"].Value);
                        double RecQty = 0;
                        foreach (DataRow row in dtRecQty.Rows)
                        {
                            if (row["Code"].ToString() == RMCode && row["Remarks"].ToString() == DocNo)
                            {
                                RecQty = Convert.ToDouble(row["ReceiveQty"].ToString());
                                break;
                            }
                        }
                        fgrid.dgvScanned.Rows.Add(RMCode, RMName, Qty, RecQty, DocNo, RegDate);
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
