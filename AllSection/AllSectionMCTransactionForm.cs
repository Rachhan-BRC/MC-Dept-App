using Microsoft.VisualBasic.ApplicationServices;
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

namespace MachineDeptApp.AllSection
{
    public partial class AllSectionMCTransactionForm : Form
    {
        SQLConnectAllSection cnnAll = new SQLConnectAllSection();

        public AllSectionMCTransactionForm()
        {
            InitializeComponent();
            this.cnnAll.Connection();
            this.Load += AllSectionMCTransactionForm_Load;
            this.btnSearch.Click += BtnSearch_Click;
            this.dtpFrom.ValueChanged += DtpFrom_ValueChanged;
            this.dtpToo.ValueChanged += DtpToo_ValueChanged;
            this.btnDelete.Click += BtnDelete_Click;
            this.dgvSearchResult.CellClick += DgvSearchResult_CellClick;

        }

        private void DgvSearchResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CheckBtnDelete();
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[1].Value.ToString() == "MC1")
            {
                string SysNo = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[0].Value.ToString();
                string Funct = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[2].Value.ToString();
                string PosNo = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[3].Value.ToString();
                string ItemCode = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[4].Value.ToString();
                string BoxNo = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[6].Value.ToString();

                //Rec in
                if (Funct == "ទទួលចូល")
                {
                    DialogResult DLS = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DLS == DialogResult.Yes)
                    {
                        try
                        {
                            cnnAll.con.Open();
                            string query = "UPDATE tbAllTransaction SET " +
                                            "CancelStatus=1, " +
                                            "UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            "UpdateBy=N'" + MenuFormV2.UserForNextForm + "'" +
                                            "WHERE SysNo='" + SysNo + "' AND POS_No='" + PosNo + "' AND ItemCode='" + ItemCode + "' AND BoxNo=" + BoxNo;
                            SqlCommand cmd = new SqlCommand(query, cnnAll.con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("លុបទិន្នន័យរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvSearchResult.Rows.RemoveAt(dgvSearchResult.CurrentCell.RowIndex);
                            dgvSearchResult.Refresh();
                            dgvSearchResult.ClearSelection();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        cnnAll.con.Close();
                    }
                }
                //Tran Out
                if (Funct == "វេរចេញ")
                {
                    //Check WIP Already Rec or not?
                    DataTable dt = new DataTable();
                    try
                    {
                        cnnAll.con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT POS_No, ItemCode, BoxNo FROM tbAllTransaction WHERE Funct=N'ទទួលចូល' AND LocCode='WIP1' AND CancelStatus = 0 AND " +
                                                                                            "POS_No='" + PosNo + "' AND ItemCode='" + ItemCode + "' AND BoxNo=" + BoxNo, cnnAll.con);
                        sda.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cnnAll.con.Close();
                    if (dt.Rows.Count == 0)
                    {
                        DialogResult DLS = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (DLS == DialogResult.Yes)
                        {
                            try
                            {
                                cnnAll.con.Open();
                                SqlDataAdapter sda = new SqlDataAdapter("SELECT POS_No, ItemCode, BoxNo FROM tbAllTransaction WHERE Funct=N'ទទួលចូល' AND LocCode='WIP1' AND CancelStatus = 0 AND " +
                                                                                                    "POS_No='" + PosNo + "' AND ItemCode='" + ItemCode + "' AND BoxNo=" + BoxNo, cnnAll.con);
                                DataTable reCheck = new DataTable();
                                sda.Fill(reCheck);
                                if (reCheck.Rows.Count == 0)
                                {
                                    string query = "UPDATE tbAllTransaction SET " +
                                                "CancelStatus=1, " +
                                                "UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                "UpdateBy=N'" + MenuFormV2.UserForNextForm + "'" +
                                                "WHERE SysNo='" + SysNo + "' AND POS_No='" + PosNo + "' AND ItemCode='" + ItemCode + "' AND BoxNo=" + BoxNo;
                                    SqlCommand cmd = new SqlCommand(query, cnnAll.con);
                                    cmd.ExecuteNonQuery();

                                    MessageBox.Show("លុបទិន្នន័យរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    dgvSearchResult.Rows.RemoveAt(dgvSearchResult.CurrentCell.RowIndex);
                                    dgvSearchResult.Refresh();
                                    dgvSearchResult.ClearSelection();
                                }
                                else
                                {
                                    MessageBox.Show("មិនអាចលុបទិន្នន័យនេះបានទេ!\nWIP1 ទទួលចូល រួចហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            cnnAll.con.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("មិនអាចលុបទិន្នន័យនេះបានទេ!\nWIP1 ទទួលចូល រួចហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }
        private void DtpToo_ValueChanged(object sender, EventArgs e)
        {
            ChkDate.Checked = true;
        }
        private void DtpFrom_ValueChanged(object sender, EventArgs e)
        {
            ChkDate.Checked = true;
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            dgvSearchResult.Rows.Clear();
            Cursor = Cursors.WaitCursor;
            LbStatus.Visible = true;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();

            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Val");

            if (CboLocCode.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("LocCode = ", "'"+CboLocCode.Text+"' ");
            }
            if (txtItemCode.Text.Trim() != "")
            {
                string ItemCode = txtItemCode.Text;
                if (ItemCode.Contains("*") == true)
                {
                    ItemCode = ItemCode.Replace("*", "%");
                    dtSQLCond.Rows.Add("ItemCode LIKE ", "'" + ItemCode + "' ");
                }
                else
                {
                    dtSQLCond.Rows.Add("ItemCode = ", "'" + ItemCode + "' ");
                }                
            }
            if (txtItemName.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("ItemName LIKE ", "'" + txtItemName.Text + "' ");
            }
            if (ChkDate.Checked==true)
            {
                dtSQLCond.Rows.Add("ShipDate BETWEEN ", "'"+dtpFrom.Value.ToString("yyyy-MM-dd")+" 00:00:00' AND '"+dtpToo.Value.ToString("yyyy-MM-dd") +" 23:59:59' ");
            }
            if (txtPosNo.Text.Trim() != "")
            {
                string PosNo = txtPosNo.Text;
                if (PosNo.Contains("*") == true)
                {
                    PosNo = PosNo.Replace("*", "%");
                    dtSQLCond.Rows.Add("POS_No LIKE ", "'" + PosNo + "' ");
                }
                else
                {
                    dtSQLCond.Rows.Add("POS_No = ", "'" + PosNo + "' ");
                }
            }
            if (txtBoxNo.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("BoxNo = ", "'" + txtBoxNo.Text + "' ");
            }
            if (txtSysNo.Text.Trim() != "")
            {
                string SysNo = txtSysNo.Text;
                if (SysNo.Contains("*") == true)
                {
                    SysNo = SysNo.Replace("*", "%");
                    dtSQLCond.Rows.Add("SysNo LIKE ", "'" + SysNo + "' ");
                }
                else
                {
                    dtSQLCond.Rows.Add("SysNo = ", "'" + SysNo + "' ");
                }
            }

            string SQLConds = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                if (SQLConds.Trim() == "")
                {
                    SQLConds = "AND " + row[0].ToString() + row[1].ToString();
                }
                else
                {
                    SQLConds = SQLConds + "AND " + row[0].ToString() + row[1].ToString();
                }
            }

            DataTable dt = new DataTable();

            try
            {
                cnnAll.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT SysNo, LocCode, Funct, POS_No, ItemCode, ItemName, BoxNo, RecQty, TransQty, ShipDate, RegDate, RegBy, UpdateDate, UpdateBy FROM tbAllTransaction " +
                                                                                "WHERE CancelStatus =0 " + SQLConds +
                                                                                "ORDER BY RegDate ASC", cnnAll.con);                
                sda.Fill(dt);
            }
            catch(Exception ex)
            {
                MessageBox.Show("មានបញ្ហា !\n"+ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnnAll.con.Close();

            foreach (DataRow row in dt.Rows)
            {
                string SysNo = row[0].ToString();
                string LocCode = row[1].ToString();
                string Funct = row[2].ToString();
                string PosNo = row[3].ToString();
                string ItemCode = row[4].ToString();
                string ItemName = row[5].ToString();
                double BoxNo  = Convert.ToDouble(row[6].ToString());
                double RecQty = Convert.ToDouble(row[7].ToString());
                double TranQty = Convert.ToDouble(row[8].ToString());
                DateTime ShipDate = Convert.ToDateTime(row[9].ToString());
                DateTime RegDate = Convert.ToDateTime(row[10].ToString());
                string RegBy  = row[11].ToString();
                DateTime UpdateDate = Convert.ToDateTime(row[12].ToString());
                string UpdateBy = row[13].ToString();

                dgvSearchResult.Rows.Add(SysNo, LocCode, Funct, PosNo, ItemCode, ItemName, BoxNo, RecQty, TranQty, ShipDate, RegDate, RegBy, UpdateDate, UpdateBy);
            }

            double TotalRec = 0;
            double TotalTrans = 0;

            if (dgvSearchResult.Rows.Count > 0)
            {
                foreach (DataGridViewRow DgvRow in dgvSearchResult.Rows)
                {
                    TotalRec = TotalRec + Convert.ToDouble(DgvRow.Cells[7].Value.ToString());
                    TotalTrans = TotalTrans + Convert.ToDouble(DgvRow.Cells[8].Value.ToString());
                }
            }

            Cursor = Cursors.Default;
            LbStatus.Text = "រកឃើញទិន្នន័យ "+dgvSearchResult.Rows.Count.ToString("N0");
            LbStatus.Refresh();

            if (dgvSearchResult.Rows.Count > 0)
            {
                dgvSearchResult.Rows.Add("","","","","","",null, TotalRec, TotalTrans, null, null,"",null,"");
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Red;
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].DefaultCellStyle.Font = new Font("Khmer OS Battambang", 9, FontStyle.Bold);
            }

            dgvSearchResult.ClearSelection();
        }
        private void AllSectionMCTransactionForm_Load(object sender, EventArgs e)
        {
            DataTable dtLoc = new DataTable();
            dtLoc.Columns.Add("LocCode");
            dtLoc.Rows.Add("");
            dtLoc.Rows.Add("MC1");
            dtLoc.Rows.Add("WIP1");
            dtLoc.Rows.Add("Assy1");
            foreach (DataRow row in dtLoc.Rows)
            {
                CboLocCode.Items.Add(row[0]);
            }

        }
        private void CheckBtnDelete()
        {
            if (dgvSearchResult.SelectedCells.Count > 0)
            {
                if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[1].Value.ToString() == "MC1")
                {
                    btnDelete.Enabled = true;
                    btnDelete.BackColor = Color.White;
                }
                else
                {
                    btnDelete.Enabled = false;
                    btnDelete.BackColor = Color.DarkGray;
                }
            }
        }
    }
}
