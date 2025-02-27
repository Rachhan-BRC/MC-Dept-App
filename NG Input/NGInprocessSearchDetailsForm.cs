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

namespace MachineDeptApp.NG_Input
{
    public partial class NGInprocessSearchDetailsForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        NGInprocessSearchForm fgrid;

        string ErrorText;

        public NGInprocessSearchDetailsForm(NGInprocessSearchForm fg)
        {
            InitializeComponent();
            this.cnn.Connection();
            this.fgrid = fg;
            this.Shown += NGInprocessSearchDetailsForm_Shown;
            this.Load += NGInprocessSearchDetailsForm_Load;
            this.CboMCNo.SelectedIndexChanged += CboMCNo_SelectedIndexChanged;
            this.btnDelete.Click += BtnDelete_Click;

        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvNGList.Rows.Count > 0 && LbMaintenant.Text.Trim() != "" && LbSubLeader.Text.Trim() != "" && LbIPQC.Text.Trim() != "")
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    ErrorText = "";

                    //Check Have Data or not? && Status is Not Yet request
                    DataTable dt = new DataTable();
                    try
                    {
                        cnn.con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT ReqStatus FROM tbNGInprocess WHERE PosCNo='" + fgrid.dgvSearchResult.Rows[fgrid.dgvSearchResult.CurrentCell.RowIndex].Cells["PosCNo"].Value.ToString() + "' AND MCSeqNo ='" + CboMCNo.Text + "' GROUP BY ReqStatus", cnn.con);
                        sda.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        ErrorText = ex.Message;
                    }
                    cnn.con.Close();

                    //Delete
                    if (ErrorText.Trim() == "")
                    {
                        if (dt.Rows.Count > 0 && dt.Rows[0]["ReqStatus"].ToString() == "0")
                        {
                            try
                            {
                                cnn.con.Open();
                                SqlCommand cmd = new SqlCommand("DELETE FROM tbNGInprocess WHERE PosCNo='" + fgrid.dgvSearchResult.Rows[fgrid.dgvSearchResult.CurrentCell.RowIndex].Cells["PosCNo"].Value.ToString() + "' AND MCSeqNo ='" + CboMCNo.Text + "'", cnn.con);
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                ErrorText = ex.Message;
                            }
                            cnn.con.Close();
                        }
                    }

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["ReqStatus"].ToString() == "0")
                            {
                                MessageBox.Show("លុបទិន្នន័យរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ClearAllText();
                            }
                            else
                            {
                                MessageBox.Show("ទិន្នន័យនេះព្រីនចេញរួចរាល់ហើយមិនអាចលុបបានទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                        else
                        {
                            MessageBox.Show("គ្មានទិន្នន័យនេះទេ! អាចមកពីអ្នកផ្សេង\nបានលុបរួចរាល់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void CboMCNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TakingData();
        }
        private void NGInprocessSearchDetailsForm_Shown(object sender, EventArgs e)
        {
            this.Text = "NG Details for : " + fgrid.dgvSearchResult.Rows[fgrid.dgvSearchResult.CurrentCell.RowIndex].Cells["PosCNo"].Value.ToString();
            this.CboMCNo.SelectedIndex = 0;

            dgvNGList.RowHeadersDefaultCellStyle.Font = new Font(dgvNGList.RowHeadersDefaultCellStyle.Font, FontStyle.Regular);

            //Rename HeaderText
            foreach (DataGridViewColumn dgvCol in dgvNGList.Columns)
            {
                string ColumnText = dgvCol.HeaderText.ToString();
                ColumnText = ColumnText.Replace("|", "\n");
                dgvCol.HeaderText = ColumnText;
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable;

                //Set HeaderColumnBackColor
                if (dgvCol.Name == "NGQty")
                {
                    dgvCol.HeaderCell.Style.BackColor = Color.Yellow;
                    dgvCol.HeaderCell.Style.SelectionBackColor = Color.Yellow;
                }

                //Froze
                if (dgvCol.Name == "RMCode" || dgvCol.Name == "RMName")
                {
                    dgvCol.Frozen = true;
                }
            }

        }
        private async void NGInprocessSearchDetailsForm_Load(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            TakingData();
        }

        //Method
        private void TakingData()
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            ClearAllText();

            //Taking Data from DB
            DataTable dt = new DataTable();
            DataTable dtConfirm = new DataTable();
            try
            {
                cnn.con.Open();
                /*
                string SQLQuery = "SELECT T1.UpItemCode, T2.ItemCode, ItemName, RMType, TbNGSet.Qty AS NGSet, LowQty*SemiQtyOfFG AS BOMQty, ROUND((TbNGSet.Qty * LowQty*SemiQtyOfFG),3) AS NGSetQty, ROUND(TbNGPcs.Qty,3) AS NGPcs FROM " +
                    "\n\t(SELECT * FROM MstBOM) T1 " +
                    "\n\tLEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType = 'Material') T2 " +
                    "\n\tON T1.LowItemCode = T2.ItemCode " +
                    "\n\tLEFT JOIN (SELECT * FROM tbSDMstUncountMat) T3 " +
                    "\n\tON T2.ItemCode=T3.Code " +
                    "\n\tLEFT JOIN (SELECT * FROM tbPOSDetailofMC) T4 " +
                    "\n\tON T1.UpItemCode=T4.WIPCode " +
                    "\n\tLEFT JOIN (SELECT * FROM tbNGInprocess WHERE NGType='NGSet' AND MCSeqNo='" + CboMCNo.Text + "') TbNGSet  " +
                    "\n\tON TbNGSet.PosCNo=T4.PosCNo AND T1.LowItemCode=TbNGSet.RMCode " +
                    "\n\tLEFT JOIN (SELECT * FROM tbNGInprocess WHERE NGType='NGPcs' AND MCSeqNo='" + CboMCNo.Text + "') TbNGPcs " +
                    "\n\tON TbNGPcs.PosCNo=T4.PosCNo AND T1.LowItemCode=TbNGPcs.RMCode " +
                    "\n\tWHERE T4.PosCNo='" + fgrid.dgvSearchResult.Rows[fgrid.dgvSearchResult.CurrentCell.RowIndex].Cells["PosCNo"].Value.ToString() + "' " +
                    "\n\tORDER BY UpItemCode ASC, ItemCode ASC ";
                */

                string SQLQuery = "SELECT T1.UpItemCode, T2.ItemCode, ItemName, COALESCE(NGKITQty,0) AS NGKITQty, ROUND(TbNGQty.Qty,3) AS NGPcs FROM " +
                    "\n(SELECT * FROM MstBOM) T1 " +
                    "\nLEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType = 'Material') T2 ON T1.LowItemCode = T2.ItemCode " +
                    "\nLEFT JOIN (SELECT * FROM tbPOSDetailofMC) T3 ON T1.UpItemCode=T3.WIPCode " +
                    "\nLEFT JOIN (SELECT * FROM tbNGInprocess WHERE NGType='NGPcs' AND MCSeqNo='" + CboMCNo.Text + "') TbNGQty " +
                    "\nON TbNGQty.PosCNo=T3.PosCNo AND T1.LowItemCode=TbNGQty.RMCode " +
                    "\nLEFT JOIN (SELECT POSNo, Code, SUM(ReceiveQty) AS NGKITQty FROM tbSDMCAllTransaction " +
                    "\nWHERE CancelStatus=0 AND LocCode = 'MC1' AND Remarks LIKE '%NG%' AND POSNo='"+ fgrid.dgvSearchResult.Rows[fgrid.dgvSearchResult.CurrentCell.RowIndex].Cells["PosCNo"].Value.ToString() + "' GROUP BY POSNo, Code) TbKITNG " +
                    "\nON T2.ItemCode=TbKITNG.Code " +
                    "\nWHERE T3.PosCNo='"+ fgrid.dgvSearchResult.Rows[fgrid.dgvSearchResult.CurrentCell.RowIndex].Cells["PosCNo"].Value.ToString() + "' " +
                    "\nORDER BY UpItemCode ASC, ItemCode ASC ";
                //Console.WriteLine(SQLQuery);
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery,cnn.con);
                sda.Fill(dt);

                SQLQuery = "SELECT T1.PosCNo, "+ CboMCNo.Text+ "Name, MCSeqNo, TbMain.Name AS MCMaint, TbSubLeader.Name AS MCSubLeader, TbIPQC.Name AS IPQC FROM " +
                    "\n\t(SELECT PosCNo, MCSeqNo, MCMaint, MCSubLeader,IPQC FROM tbNGInprocess GROUP BY PosCNo, MCSeqNo, MCMaint, MCSubLeader,IPQC) T1 " +
                    "\n\tLEFT JOIN (SELECT * FROM [TD_Database].dbo.[Employee_list]) TbMain " +
                    "\n\tON T1.MCMaint=TbMain.Staff_No " +
                    "\n\tLEFT JOIN (SELECT * FROM [TD_Database].dbo.[Employee_list]) TbSubLeader " +
                    "\n\tON T1.MCSubLeader=TbSubLeader.Staff_No " +
                    "\n\tLEFT JOIN (SELECT * FROM [TD_Database].dbo.[Employee_list]) TbIPQC " +
                    "\n\tON T1.IPQC=TbIPQC.Staff_No " +
                    "\n\tLEFT JOIN (SELECT * FROM tbPOSDetailofMC) TbMasterPlan " +
                    "\n\tON T1.PosCNo=TbMasterPlan.PosCNo" +
                    "\n\tWHERE T1.PosCNo = '"+ fgrid.dgvSearchResult.Rows[fgrid.dgvSearchResult.CurrentCell.RowIndex].Cells["PosCNo"].Value.ToString() + "' AND MCSeqNo = '"+ CboMCNo.Text + "' ";
                sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtConfirm);
            }
            catch (Exception ex)
            {
                ErrorText =ex.Message;
            }
            cnn.con.Close();

            //Add to DGV
            foreach (DataRow row in dt.Rows)
            {
                string ItemCode = row["ItemCode"].ToString();
                string ItemName = row["ItemName"].ToString();
                
                dgvNGList.Rows.Add();
                dgvNGList.Rows[dgvNGList.Rows.Count - 1].HeaderCell.Value = dgvNGList.Rows.Count.ToString();
                dgvNGList.Rows[dgvNGList.Rows.Count - 1].Cells["RMCode"].Value = ItemCode;
                dgvNGList.Rows[dgvNGList.Rows.Count - 1].Cells["RMName"].Value = ItemName;
                if (row["NGKITQty"].ToString().Trim() != "")
                {
                    dgvNGList.Rows[dgvNGList.Rows.Count - 1].Cells["KITSDQty"].Value = Convert.ToDouble(row["NGKITQty"].ToString());
                }
                if (row["NGPcs"].ToString().Trim() != "")
                {
                    dgvNGList.Rows[dgvNGList.Rows.Count - 1].Cells["NGQty"].Value = Convert.ToDouble(row["NGPcs"].ToString());
                }

            }
            dgvNGList.ClearSelection();


            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                //Confirm
                if (dtConfirm.Rows.Count > 0)
                {
                    LbMCName.Text = dtConfirm.Rows[0][CboMCNo.Text+"Name"].ToString();
                    LbMaintenant.Text = dtConfirm.Rows[0]["MCMaint"].ToString();
                    LbSubLeader.Text = dtConfirm.Rows[0]["MCSubLeader"].ToString();
                    LbIPQC.Text = dtConfirm.Rows[0]["IPQC"].ToString();
                }
            }
            else
            {
                MessageBox.Show("មានបញ្ហា!\n"+ErrorText,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
            }
        }
        private void ClearAllText()
        {
            LbMCName.Text = "";
            dgvNGList.Rows.Clear();
            LbMaintenant.Text = "";
            LbSubLeader.Text = "";
            LbIPQC.Text = "";
        }
    }
}
