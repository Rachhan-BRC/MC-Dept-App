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

namespace MachineDeptApp
{
    public partial class OBSMatRequestForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        string ErrorText = "";
        public OBSMatRequestForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Shown += OBSMatRequestForm_Shown;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnSearchAPI.Click += BtnSearchAPI_Click;
            this.btnPrint.Click += BtnPrint_Click;
            this.btnNew.Click += BtnNew_Click;

            this.txtCode.TextChanged += TxtCode_TextChanged;
            this.txtDescription.TextChanged += TxtDescription_TextChanged;
            this.cboRMType.TextChanged += CboRMType_TextChanged;
            this.dtpShipDate.ValueChanged += DtpShipDate_ValueChanged;
            this.cboMCReqStatus.TextChanged += CboMCReqStatus_TextChanged;
            this.txtRemark.TextChanged += TxtRemark_TextChanged;
            this.dgvSearch.CellClick += DgvSearch_CellClick;
            this.chkSelectAll.Click += ChkSelectAll_Click;

        }

        private void BtnSearchAPI_Click(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }
        private void ChkSelectAll_Click(object sender, EventArgs e)
        {
            bool bSelectAll = chkSelectAll.Checked;
            foreach (DataGridViewRow row in dgvSearch.Rows)
            {
                row.Cells["chkSelect"].Value = bSelectAll;
            }
            btnPrint.Enabled = CheckForEnableBtnPrint();
        }
        private void DgvSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgvSearch.SelectedCells.Count>0 && e.RowIndex >=0 && e.ColumnIndex == dgvSearch.Columns["chkSelect"].Index)
            {
                bool currentValue = Convert.ToBoolean(dgvSearch.SelectedCells[dgvSearch.Columns["chkSelect"].Index].Value);
                dgvSearch.SelectedCells[dgvSearch.Columns["chkSelect"].Index].Value = !currentValue;
                dgvSearch.ClearSelection(); dgvSearch.CurrentCell = null;
                btnPrint.Enabled = CheckForEnableBtnPrint();
                int SelectedCount = 0;
                foreach (DataGridViewRow row in dgvSearch.Rows)
                {
                    if (row.Cells["chkSelect"].Value != null && row.Cells["chkSelect"].Value.ToString() == "True")
                        SelectedCount++;
                }
                if(SelectedCount == dgvSearch.Rows.Count)
                    chkSelectAll.Checked = true;
                else
                    chkSelectAll.Checked = false;
            }
        }
        private void TxtRemark_TextChanged(object sender, EventArgs e)
        {
            if (txtRemark.Text.Trim() != "")
                chkRemark.Checked = true;
            else
                chkRemark.Checked = false;
        }
        private void CboMCReqStatus_TextChanged(object sender, EventArgs e)
        {
            if (cboMCReqStatus.Text.Trim() != "")
                chkMCReqStatus.Checked = true;
            else
                chkMCReqStatus.Checked = false;
        }
        private void CboRMType_TextChanged(object sender, EventArgs e)
        {
            if (cboRMType.Text.Trim() != "")
                chkRMType.Checked = true;
            else
                chkRMType.Checked = false;
        }
        private void TxtCode_TextChanged(object sender, EventArgs e)
        {
            if(txtCode.Text.Trim()!="")
                chkCode.Checked = true;
            else
                chkCode.Checked = false;
        }
        private void TxtDescription_TextChanged(object sender, EventArgs e)
        {
            if(txtDescription.Text.Trim()!="")
                chkDescription.Checked = true;
            else
                chkDescription.Checked = false;
        }
        private void DtpShipDate_ValueChanged(object sender, EventArgs e)
        {
            chkShipDate.Checked = true;
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            OBSMatRequestImportForm form = new OBSMatRequestImportForm();
            form.ShowDialog();
        }        
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            dgvSearch.Rows.Clear();
            foreach (DataGridViewColumn col in dgvSearch.Columns)
                col.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;

            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Value");
            if (chkCode.Checked && txtCode.Text.Trim() != "")
            {
                string SValue = txtCode.Text;
                if(SValue.Contains("*"))
                    dtSQLCond.Rows.Add("ItemCode", " LIKE '" + txtCode.Text.Replace("*", "%") + "' ");
                else
                    dtSQLCond.Rows.Add("ItemCode", " = '" + txtCode.Text + "' ");
            }
            if (chkDescription.Checked && txtDescription.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("ItemName", " LIKE '%" + txtDescription.Text + "%' ");
            }
            if (chkRemark.Checked && txtRemark.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("Remarks", " = '" + txtRemark.Text + "' ");
            }
            if (chkRMType.Checked && cboRMType.Text.Trim() != "" && cboRMType.Text != "ទាំងអស់")
            {
                dtSQLCond.Rows.Add("RMType", " = '" + cboRMType.Text.Trim() + "' ");
            }
            if (chkShipDate.Checked)
            {
                dtSQLCond.Rows.Add("ShipDate", " = '" + dtpShipDate.Value.ToString("yyyy-MM-dd") + "' ");
            }
            if (chkMCReqStatus.Checked && cboMCReqStatus.Text.Trim() != "" && cboMCReqStatus.Text != "ទាំងអស់")
            {
                dtSQLCond.Rows.Add("MCReqDate", cboMCReqStatus.Text == "ព្រីនរួច" ? " IS NOT NULL " : " IS NULL ");
            }
            string SQLConds = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                if (SQLConds.Trim() == "")
                    SQLConds = "WHERE " + row[0] + row[1];
                else
                    SQLConds = SQLConds + "AND " + row[0] + row[1];
            }

            DataTable dtSearch = new DataTable();
            try
            {
                if (cnn.con.State == ConnectionState.Closed)
                    cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(@"SELECT *, 
                    CASE 
	                    WHEN MCReqDate IS NOT NULL THEN 'TRUE' 
	                    ELSE 'FALSE' 
                    END AS MCReqStatus FROM tbOBSMatRequest 
                    " + SQLConds + @" 
                    ORDER BY Remarks, RegDate, RMType, ItemCode", cnn.con);
                sda.Fill(dtSearch);
            }
            catch (Exception ex)
            {
                ErrorText = "Taking Data : " + ex.Message;
            }
            finally
            {
                if (cnn.con.State == ConnectionState.Open)
                    cnn.con.Close();
            }

            //Add to Dgv
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtSearch.Rows)
                {
                    DataGridViewRow drow = dgvSearch.Rows[dgvSearch.Rows.Add()];
                    drow.Cells["CodeNo"].Value = row["ItemCode"].ToString();
                    drow.Cells["Description"].Value = row["ItemName"].ToString();
                    drow.Cells["Maker"].Value = row["Maker"].ToString();
                    drow.Cells["Type"].Value = row["RMType"].ToString();
                    drow.Cells["Pack1Qty"].Value = Convert.ToInt32(row["PackSize"]);
                    drow.Cells["Pack"].Value = Convert.ToInt32(row["PackQty"]);
                    drow.Cells["TotalQty"].Value = Convert.ToInt32(row["TTLReqQty"]);
                    drow.Cells["Barcode"].Value = row["MatReqNo"].ToString();
                    drow.Cells["Remarks"].Value = row["Remarks"].ToString();
                    drow.Cells["ShipDate"].Value = row["ShipDate"] == DBNull.Value ? null : (object)Convert.ToDateTime(row["ShipDate"]);
                    drow.Cells["MCReqStatus"].Value = row["MCReqStatus"].ToString() == "TRUE";
                    drow.Cells["MCReqDate"].Value = row["MCReqDate"] == DBNull.Value ? null : (object)Convert.ToDateTime(row["MCReqDate"]);
                    drow.Cells["RegDate"].Value = Convert.ToDateTime(row["RegDate"]);
                    drow.Cells["RegBy"].Value = row["RegBy"].ToString();
                    drow.Cells["UpdateDate"].Value = row["UpdateDate"] == DBNull.Value ? null : (object)Convert.ToDateTime(row["UpdateDate"]);
                    drow.Cells["UpdateBy"].Value = row["UpdateBy"] == DBNull.Value ? "" : row["UpdateBy"].ToString();
                }
            }

            btnPrint.Enabled = CheckForEnableBtnPrint();

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                LbStatus.Text = "រកឃើញទិន្នន័យចំនួន ៖ " + dgvSearch.Rows.Count.ToString("N0") + " ទិន្នន័យ";
                LbStatus.Refresh();
                dgvSearch.ClearSelection();
                dgvSearch.CurrentCell = null;
            }
            else
            {
                LbStatus.Text = "ការស្វែងរកមានបញ្ហា!";
                LbStatus.Refresh();
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OBSMatRequestForm_Shown(object sender, EventArgs e)
        {
            foreach(DataGridViewColumn col in dgvSearch.Columns)
                col.HeaderText = col.HeaderText.Replace("|", "\n");
            ErrorText = "";
            Cursor = Cursors.WaitCursor;

            DataTable dtRMType = dtGetRMType();
            foreach(DataRow row in dtRMType.Rows)
            {
                cboRMType.Items.Add(row["RMTypeName"].ToString());
            }
            cboRMType.Items.Add("ទាំងអស់");

            Cursor = Cursors.Default;

            if(ErrorText.Trim() != "")
            {
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //Method
        private DataTable dtGetRMType()
        {
            DataTable dt = new DataTable();

            //Taking Mst RMType
            try
            {
                if (cnn.con.State == ConnectionState.Closed)
                    cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(@"SELECT DISTINCT RMTypeName FROM [tbMasterItem] 
                    WHERE ItemType = 'Material' AND RMTypeName IS NOT NULL AND RMTypeName <> 'Connector' 
                    ORDER BY RMTypeName ", cnn.con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                ErrorText = "Taking Mst RMType : \n" + ex.Message;
            }
            finally
            {
                if (cnn.con.State == ConnectionState.Open)
                    cnn.con.Close();
            }

            return dt;
        }
        private bool CheckForEnableBtnPrint()
        {
            bool bEnable = false;
            foreach (DataGridViewRow row in dgvSearch.Rows)
            {
                if (row.Cells["chkSelect"].Value != null && row.Cells["chkSelect"].Value.ToString() == "True")
                {
                    bEnable = true;
                    break;
                }
            }
            return bEnable;
        }

    }
}
