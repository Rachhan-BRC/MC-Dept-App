using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
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
            this.txtRemark.TextChanged += TxtRemark_TextChanged;

            this.cboRMType.TextChanged += CboRMType_TextChanged;
            this.cboMCReqStatus.TextChanged += CboMCReqStatus_TextChanged;

            this.dtpShipDate.ValueChanged += DtpShipDate_ValueChanged;

            this.dgvSearch.CellClick += DgvSearch_CellClick;
            this.dgvSearch.CellPainting += DgvSearch_CellPainting;
            this.chkSelectAll.Click += ChkSelectAll_Click;

        }

        private void DgvSearch_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow drow = dgvSearch.Rows[e.RowIndex];
                bool.TryParse(drow.Cells["MCReqStatus"].Value?.ToString() ?? "False", out bool MC);
                drow.DefaultCellStyle.ForeColor = MC ? Color.Gray : dgvSearch.AlternatingRowsDefaultCellStyle.ForeColor;
            }
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
                if (!Convert.ToBoolean(row.Cells[dgvSearch.Columns["MCReqStatus"].Index].Value))
                    row.Cells["chkSelect"].Value = bSelectAll;
            }
            btnPrint.Enabled = CheckForEnableBtnPrint();
        }
        private void DgvSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgvSearch.SelectedCells.Count>0 && e.RowIndex >=0 && e.ColumnIndex == dgvSearch.Columns["chkSelect"].Index)
            {
                if (!Convert.ToBoolean(dgvSearch.CurrentRow.Cells[dgvSearch.Columns["MCReqStatus"].Index].Value))
                {
                    bool currentValue = Convert.ToBoolean(dgvSearch.SelectedCells[dgvSearch.Columns["chkSelect"].Index].Value);
                    dgvSearch.SelectedCells[dgvSearch.Columns["chkSelect"].Index].Value = !currentValue;
                    btnPrint.Enabled = CheckForEnableBtnPrint();
                    int SelectedCount = 0;
                    foreach (DataGridViewRow row in dgvSearch.Rows)
                    {
                        if (row.Cells["chkSelect"].Value != null && row.Cells["chkSelect"].Value.ToString() == "True")
                            SelectedCount++;
                    }
                    if (SelectedCount == dgvSearch.Rows.Count)
                        chkSelectAll.Checked = true;
                    else
                        chkSelectAll.Checked = false;
                }
                dgvSearch.ClearSelection(); dgvSearch.CurrentCell = null;
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
            DataTable dtSelectedList = new DataTable();
            dtSelectedList.Columns.Add("ItemCode", typeof(string));
            dtSelectedList.Columns.Add("ItemName", typeof(string));
            dtSelectedList.Columns.Add("Maker", typeof(string));
            dtSelectedList.Columns.Add("RMType", typeof(string));
            dtSelectedList.Columns.Add("PackSize", typeof(int));
            dtSelectedList.Columns.Add("PackQty", typeof(int));
            dtSelectedList.Columns.Add("TTLReqQty", typeof(int));
            dtSelectedList.Columns.Add("MATReqNo", typeof(string));
            dtSelectedList.Columns.Add("Remarks", typeof(string));
            foreach (DataGridViewRow row in dgvSearch.Rows)
            {
                if (row.Cells["chkSelect"].Value != null && row.Cells["chkSelect"].Value.ToString() == "True" &&
                    !Convert.ToBoolean(row.Cells["MCReqStatus"].Value?.ToString() ?? "FALSE"))
                {
                    DataRow drow = dtSelectedList.NewRow();
                    drow["ItemCode"] = row.Cells["CodeNo"].Value.ToString();
                    drow["ItemName"] = row.Cells["Description"].Value.ToString();
                    drow["Maker"] = row.Cells["Maker"].Value.ToString();
                    drow["RMType"] = row.Cells["Type"].Value.ToString();
                    drow["PackSize"] = Convert.ToInt32(row.Cells["Pack1Qty"].Value);
                    drow["PackQty"] = Convert.ToInt32(row.Cells["Pack"].Value);
                    drow["TTLReqQty"] = Convert.ToInt32(row.Cells["TotalQty"].Value);
                    drow["MATReqNo"] = row.Cells["Barcode"].Value.ToString();
                    drow["Remarks"] = row.Cells["Remarks"].Value.ToString();
                    dtSelectedList.Rows.Add(drow);
                }
            }
            if (dtSelectedList.Rows.Count > 0)
            {
                int DistinctPOSCount = dtSelectedList.AsEnumerable()
                    .Select(r => r.Field<string>("Remarks"))
                    .Distinct()
                    .Count();
                if (DistinctPOSCount == 1)
                {
                    DialogResult DSL = MessageBox.Show("តើអ្នកចង់ព្រីនមែនដែរឬទេ?", MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DSL == DialogResult.Yes)
                    {
                        ErrorText = "";
                        Cursor = Cursors.WaitCursor;
                        LbStatus.Text = "កំពុងឆែកទិន្នន័យ . . . ";
                        LbStatus.Refresh();

                        string Username = MenuFormV2.UserForNextForm;
                        DateTime PrintDate = DateTime.Now;
                        string POS = dtSelectedList.Rows[0]["Remarks"].ToString();
                        DataTable dtFinalList = dtSelectedList.Clone();

                        //Update to DB
                        try
                        {
                            if (cnn.con.State != ConnectionState.Open)
                                cnn.con.Open();
                            foreach (DataRow row in dtSelectedList.Rows)
                            {
                                string MATReqNo = row["MATReqNo"].ToString();
                                bool OkToBePrint = CheckAlreadyRequested(POS, MATReqNo);                                
                                if (OkToBePrint)
                                {
                                    SqlCommand cmd = new SqlCommand(@"UPDATE tbOBSMatRequest SET 
                                    MCReqDate = @PrintD, 
                                    UpdateDate = @PrintD, 
                                    UpdateBy = @UpB 
                                    WHERE Remarks = @Rem AND MatReqNo = @MatNo", cnn.con);
                                    cmd.Parameters.AddWithValue("@PrintD", PrintDate);
                                    cmd.Parameters.AddWithValue("@UpB", Username);
                                    cmd.Parameters.AddWithValue("@Rem", POS);
                                    cmd.Parameters.AddWithValue("@MatNo", MATReqNo);
                                    cmd.ExecuteNonQuery();
                                    dtFinalList.ImportRow(row);
                                }
                                else
                                {
                                    SqlCommand cmd = new SqlCommand(@"SELECT Remarks, MatReqNo, ItemCode, 
                                        ItemName, MCReqDate, UpdateDate, UpdateBy FROM tbOBSMatRequest 
                                        WHERE Remarks = @Rem AND MatReqNo = @MatNo", cnn.con);
                                    cmd.Parameters.AddWithValue("@Rem", POS);
                                    cmd.Parameters.AddWithValue("@MatNo", MATReqNo);
                                    using (SqlDataReader dr = cmd.ExecuteReader())
                                    {
                                        if (dr.Read())
                                        {
                                            foreach (DataGridViewRow drow in dgvSearch.Rows)
                                            {
                                                if (drow.Cells["Remarks"].Value.ToString() == POS && drow.Cells["Barcode"].Value.ToString() == MATReqNo)
                                                {
                                                    drow.Cells["chkSelect"].Value = false;
                                                    drow.Cells["MCReqStatus"].Value = true;
                                                    drow.Cells["MCReqDate"].Value = dr["MCReqDate"] == DBNull.Value ? null : (object)Convert.ToDateTime(dr["MCReqDate"]);
                                                    drow.Cells["UpdateDate"].Value = dr["UpdateDate"] == DBNull.Value ? null : (object)Convert.ToDateTime(dr["UpdateDate"]);
                                                    drow.Cells["UpdateBy"].Value = dr["UpdateBy"] == DBNull.Value ? "" : dr["UpdateBy"].ToString();
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            dtFinalList.AcceptChanges();
                        }
                        catch (Exception ex)
                        {
                            ErrorText = "Update to DB : \n" + ex.Message;
                        }
                        finally
                        {
                            if (cnn.con.State != ConnectionState.Closed)
                                cnn.con.Close();
                        }

                        //Update Dgv & Print Excel
                        string PrintedPath = "";
                        if (ErrorText.Trim() == "")
                        {

                            if (dtFinalList.Rows.Count > 0)
                            {
                                LbStatus.Text = "ព្រីនឯកសារ . . . ";
                                LbStatus.Refresh();

                                //Print Excel
                                OtherClass.OBSMatReqPrintClass OBSMatPrint = new OtherClass.OBSMatReqPrintClass();
                                OBSMatPrint.PrintExcelOut(PrintDate, Username, dtFinalList);
                                PrintedPath = OtherClass.OBSMatReqPrintClass.SavePath;
                                ErrorText = OtherClass.OBSMatReqPrintClass.ErrorText;

                                //Update Dgv
                                foreach (DataRow row in dtFinalList.Rows)
                                {
                                    foreach (DataGridViewRow drow in dgvSearch.Rows)
                                    {
                                        if (drow.Cells["Remarks"].Value.ToString() == row["Remarks"].ToString() && drow.Cells["Barcode"].Value.ToString() == row["MATReqNo"].ToString())
                                        {
                                            drow.Cells["chkSelect"].Value = false;
                                            drow.Cells["MCReqStatus"].Value = true;
                                            drow.Cells["MCReqDate"].Value = PrintDate;
                                            drow.Cells["UpdateDate"].Value = PrintDate;
                                            drow.Cells["UpdateBy"].Value = Username;
                                            break;
                                        }
                                    }
                                }

                            }
                            else
                                ErrorText = "គ្មានទិន្នន័យដែលត្រូវព្រីនទេ!";
                        }

                        Cursor = Cursors.Default;

                        if(ErrorText.Trim()== "")
                        {
                            LbStatus.Text = "ព្រីនឯកសារបានជោគជ័យ!";
                            LbStatus.Refresh();
                            MessageBox.Show("ព្រីនឯកសារបានជោគជ័យ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (PrintedPath.Trim() == "")
                                return;
                        }
                        else
                        {
                            LbStatus.Text = "ព្រីនឯកសារមានបញ្ហា!";
                            LbStatus.Refresh();
                            if (PrintedPath.Trim() == "")
                            {
                                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                MessageBox.Show("ឯកសារត្រូវបានព្រីន ប៉ុន្តែមានបញ្ហាខ្លះ!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                                
                            }
                        }

                       //Open Printed Excel
                        if (PrintedPath.Trim() != "")
                        {
                            try
                            {
                                System.Diagnostics.Process.Start(PrintedPath);
                            }
                            catch { }
                        }

                    }
                }
                else
                    MessageBox.Show("សូមជ្រើសរើសទិន្នន័យនៃ POS/Remark តែមួយ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            btnPrint.Enabled = CheckForEnableBtnPrint();
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
            chkSelectAll.Checked = false;

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
                DataTable dtMCStock = Get_dtMCStock(), dtWHStock = Get_dtWHStock();
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

                    //Add MCStock & WHStock
                    if (!Convert.ToBoolean(drow.Cells["MCReqStatus"].Value?.ToString()?? "False"))
                    {
                        string ItemCode = drow.Cells["CodeNo"].Value.ToString();
                        foreach (DataRow rowMC in dtMCStock.Rows)
                        {
                            if(rowMC["Code"].ToString() == ItemCode)
                            {
                                drow.Cells["MCStock"].Value = Convert.ToInt32(rowMC["MCStock"]);
                                break;
                            }
                        }

                        foreach (DataRow rowWH in dtWHStock.Rows)
                        {
                            if (rowWH["ItemCode"].ToString() == ItemCode)
                            {
                                drow.Cells["WHStock"].Value = Convert.ToInt32(rowWH["WHStock"]);
                                break;
                            }
                        }

                    }

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
            cboMCReqStatus.SelectedIndex = 0;
            foreach (DataGridViewColumn col in dgvSearch.Columns)
            {
                col.HeaderText = col.HeaderText.Replace("|", "\n");
                if (col.Name == "MCStock" || col.Name == "WHStock")
                {
                    col.HeaderCell.Style.BackColor = Color.Orange;
                    col.HeaderCell.Style.SelectionBackColor = Color.Orange;
                }
            }
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
        private bool CheckAlreadyRequested(string POS,  string MATReqNo)
        {
            bool bOkToPrint = false;
            try
            {
                SqlCommand cmd = new SqlCommand(@"SELECT Remarks, MatReqNo, ItemCode, 
                    ItemName, MCReqDate, UpdateDate, UpdateBy FROM tbOBSMatRequest 
                    WHERE Remarks = @Rem AND MatReqNo = @MatNo", cnn.con);
                cmd.Parameters.AddWithValue("@Rem", POS);
                cmd.Parameters.AddWithValue("@MatNo", MATReqNo);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        if (dr["MCReqDate"].ToString().Trim() == "")
                            bOkToPrint = true;
                    }
                }
            }
            catch { }
            return bOkToPrint;
        }
        private DataTable Get_dtMCStock()
        {
            DataTable dt = new DataTable();
            try
            {
                if (cnn.con.State == ConnectionState.Closed)
                    cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(@"SELECT Code, SUM(StockValue) AS MCStock FROM tbSDMCAllTransaction 
                WHERE CancelStatus = 0 AND LocCode <> 'MC1' 
                GROUP BY Code 
				HAVING SUM(StockValue) > 0 ", cnn.con);
                sda.Fill(dt);
            }
            catch { }
            finally
            {
                if (cnn.con.State == ConnectionState.Open)
                    cnn.con.Close();
            }
            return dt;
        }
        private DataTable Get_dtWHStock()
        {
            DataTable dt = new DataTable();
            try
            {
                if (cnn.con.State == ConnectionState.Closed)
                    cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(@"SELECT ItemCode, SUM(StockValue) AS WHStock FROM [RawMaterialWHDB].[dbo].[tbRMCtrl_LabelTransaction] 
	                WHERE Status = 'Active' 
	                GROUP BY ItemCode 
				    HAVING SUM(StockValue) > 0 ", cnn.con);
                sda.Fill(dt);
            }
            catch { }
            finally
            {
                if (cnn.con.State == ConnectionState.Open)
                    cnn.con.Close();
            }
            return dt;
        }

    }
}
