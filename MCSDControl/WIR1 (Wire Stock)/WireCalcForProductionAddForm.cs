using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    public partial class WireCalcForProductionAddForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        WireCalcForProduction fgrid;
        DataTable dtColor;

        string ErrorText;

        public WireCalcForProductionAddForm(WireCalcForProduction fg)
        {
            InitializeComponent();
            this.fgrid = fg;
            this.cnn.Connection();
            this.Shown += WireCalcForProductionAddForm_Shown;
            this.dtpDelDate.ValueChanged += DtpDelDate_ValueChanged;
            this.dgvSearchResult.CellClick += DgvSearchResult_CellClick;


            //Cbo
            this.CboMCNo.SelectedIndexChanged += CboMCNo_SelectedIndexChanged;
            this.CboMCType.SelectedIndexChanged += CboMCType_SelectedIndexChanged;

            //btn
            this.btnSearch.Click += BtnSearch_Click;
            this.btnSelectAll.Click += BtnSelectAll_Click;
            this.btnUnSelectAll.Click += BtnUnSelectAll_Click;
            this.btnOK.Click += BtnOK_Click;

        }

        
        //btn
        private void BtnOK_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                if (dgvRow.Cells["ChkSelect"].Value.ToString().ToUpper() == "TRUE")
                {
                    fgrid.dgvPOS.Rows.Add();
                    fgrid.dgvPOS.Rows[fgrid.dgvPOS.Rows.Count - 1].Cells["MCName"].Value = dgvRow.Cells["MachineName"].Value.ToString();
                    fgrid.dgvPOS.Rows[fgrid.dgvPOS.Rows.Count - 1].Cells["WIPCode"].Value = dgvRow.Cells["WIPCode"].Value.ToString();
                    fgrid.dgvPOS.Rows[fgrid.dgvPOS.Rows.Count - 1].Cells["WIPName"].Value = dgvRow.Cells["WIPName"].Value.ToString();
                    fgrid.dgvPOS.Rows[fgrid.dgvPOS.Rows.Count - 1].Cells["POSNo"].Value = dgvRow.Cells["POSNo"].Value.ToString();
                    fgrid.dgvPOS.Rows[fgrid.dgvPOS.Rows.Count - 1].Cells["PIN"].Value = dgvRow.Cells["PIN"].Value.ToString();
                    fgrid.dgvPOS.Rows[fgrid.dgvPOS.Rows.Count - 1].Cells["Wire"].Value = dgvRow.Cells["Wire"].Value.ToString();
                    fgrid.dgvPOS.Rows[fgrid.dgvPOS.Rows.Count - 1].Cells["Length"].Value = dgvRow.Cells["Length"].Value.ToString();
                    fgrid.dgvPOS.Rows[fgrid.dgvPOS.Rows.Count - 1].Cells["Qty"].Value = Convert.ToDouble(dgvRow.Cells["Qty"].Value.ToString());
                }
            }
            fgrid.dgvPOS.ClearSelection();
            fgrid.dgvPOS.CurrentCell = null;
            this.Close();
        }
        private void BtnUnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                dgvRow.Cells["ChkSelect"].Value = false;
            }
            ChkBtnOK();
        }
        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                dgvRow.Cells["ChkSelect"].Value = true;
            }
            ChkBtnOK();
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (CboMCNo.Text.Trim() != "" && CboMCName.Text.Trim() != "")
            {
                RetriveData();
            }
            else
            {
                MessageBox.Show("សូមជ្រើសរើស <លេខម៉ាស៊ីន> និង <ឈ្មោះម៉ាស៊ីន> ជាមុនសិន!","Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }

        //Cbo
        private void CboMCType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CboMCName.Items.Clear();
            if (CboMCType.Text.Trim() != "")
            {
                Cursor = Cursors.WaitCursor;
                //Taking MCType
                ErrorText = "";
                DataTable dtMCName = new DataTable();
                try
                {
                    cnn.con.Open();
                    string SQLQuery = "SELECT MCName FROM tbMasterMCType WHERE MCType='"+CboMCType.Text+"' ";
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtMCName);
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                }
                cnn.con.Close();

                Cursor = Cursors.Default;

                if (ErrorText.Trim() == "")
                {
                    CboMCName.Items.Clear();
                    foreach (DataRow row in dtMCName.Rows)
                    {
                        CboMCName.Items.Add(row["MCName"].ToString());
                    }
                }
                else
                {
                    MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void CboMCNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CboMCType.Text = "";
        }

        private void WireCalcForProductionAddForm_Shown(object sender, EventArgs e)
        {
            CboMCNo.SelectedIndex = 0;
            dtColor = new DataTable();
            dtColor.Columns.Add("ShortText");
            dtColor.Columns.Add("ColorText");
            dtColor.Columns.Add("BackColor");
            dtColor.Rows.Add("RED", "White", "Red");
            dtColor.Rows.Add("BLK", "White", "Black");
            dtColor.Rows.Add("PINK", "Black", "Pink");
            dtColor.Rows.Add("YEL", "Black", "Yellow");
            dtColor.Rows.Add("BLU", "White", "Blue");
            dtColor.Rows.Add("BRN", "White", "Brown");
            dtColor.Rows.Add("G/Y", "Black", "GreenYellow");
            dtColor.Rows.Add("GRN", "White", "Green");
            dtColor.Rows.Add("GRY", "White", "Gray");
            dtColor.Rows.Add("ORG", "Black", "Orange");
            dtColor.Rows.Add("PNK", "Black", "Pink");
            dtColor.Rows.Add("SKY", "Black", "SkyBlue");
            dtColor.Rows.Add("VLT", "White", "Purple");
            dtColor.Rows.Add("WHT", "Black", "White");

            foreach (DataGridViewColumn col in dgvSearchResult.Columns)
            {
                //Console.WriteLine(col.Name.ToString());
                if (col.Index < 7)
                {
                    col.Frozen = true;
                }
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            Cursor = Cursors.WaitCursor;
            //Taking MCType
            ErrorText = "";
            DataTable dtMCType = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT MCType FROM tbMasterItemPlan2 GROUP BY MCType";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery,cnn.con);
                sda.Fill(dtMCType);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtMCType.Rows)
                {
                    CboMCType.Items.Add(row["MCType"].ToString());
                }
            }
            else
            {
                MessageBox.Show("មានបញ្ហា!\n"+ErrorText,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }
        private void DtpDelDate_ValueChanged(object sender, EventArgs e)
        {
            ChkDelDate.Checked = true;
        }
        private void DgvSearchResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSearchResult.Columns[e.ColumnIndex].Name == "ChkSelect")
            {
                if (e.RowIndex > -1)
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
                    ChkBtnOK();
                }
            }
        }

        //Method
        private void RetriveData()
        {
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            dgvSearchResult.Rows.Clear();
            Cursor = Cursors.WaitCursor;
            DataTable dtPlansSearch = new DataTable();
            DataTable dtAlreadyTransfer = new DataTable();
            try
            {
                cnn.con.Open();
                string SelectStarFrom = "PlanSeqNo, PosPDelDate, ItemCode, ItemName, PosCNo, Remarks1, Remarks2, Remarks3, SemiQty, PosCQty, Remarks, " + CboMCNo.Text + "Stat," + CboMCNo.Text + "ETA, " + CboMCNo.Text + "Name, KITStatus ";
                string WhereSelect = "WHERE NOT "+CboMCNo.Text+"Stat = 'FINISH' ";
                if (CboMCType.Text.Trim() != "")
                {
                    WhereSelect += " AND " + CboMCNo.Text.ToString() + "='" + CboMCType.Text.ToString() + "' ";
                }
                if (CboMCName.Text.Trim() != "")
                {
                    WhereSelect += " AND " + CboMCNo.Text.ToString() + "Name='" + CboMCName.Text.ToString() + "' ";
                }
                if (ChkDelDate.Checked==true)
                {
                    WhereSelect += " AND PosPDelDate BETWEEN '" + dtpDelDate.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dtpDelDate.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                }

                string FinalSQLQuery = "SELECT " + SelectStarFrom + " FROM " +
                                                        "\n\t(SELECT PlanSeqNo, PosPDelDate, T2.ItemCode, ItemName, T1.PosCNo, Remarks1, Remarks2, Remarks3, PosCQty, Remarks, SemiQty, " +
                                                        "\n\tCASE " +
                                                        "\n\t\tWHEN T1.MC1Type IS NULL THEN T3.MC1Type " +
                                                        "\n\t\tELSE T1.MC1Type " +
                                                        "\n\tEND AS MC1, CONCAT(MC1Name, '') AS MC1Name, " +
                                                        "\n\tCASE " +
                                                        "\n\t\tWHEN T1.MC1Status IS NULL THEN '' " +
                                                        "\n\t\tELSE " +
                                                        "\n\t\t\tCASE " +
                                                        "\n\t\t\t\tWHEN T1.MC1Status = 0 THEN 'STOP' " +
                                                        "\n\t\t\t\tWHEN T1.MC1Status = 1 THEN 'RUN' " +
                                                        "\n\t\t\t\tELSE 'FINISH' " +
                                                        "\n\t\t\tEND " +
                                                        "\n\tEND AS MC1Stat, T1.MC1ETA, " +
                                                        "\n\tCASE " +
                                                        "\n\t\tWHEN T1.MC2Type IS NULL THEN T3.MC2Type " +
                                                        "\n\t\tELSE T1.MC2Type " +
                                                        "\n\tEND AS MC2, CONCAT(MC2Name, '') AS MC2Name, " +
                                                        "\n\tCASE " +
                                                        "\n\t\tWHEN T1.MC2Status IS NULL THEN '' " +
                                                        "\n\t\tELSE " +
                                                        "\n\t\t\tCASE " +
                                                        "\n\t\t\t\tWHEN T1.MC2Status = 0 THEN 'STOP' " +
                                                        "\n\t\t\t\tWHEN T1.MC2Status = 1 THEN 'RUN' " +
                                                        "\n\t\t\t\tELSE 'FINISH' " +
                                                        "\n\t\t\tEND " +
                                                        "\n\t\tEND AS MC2Stat, T1.MC2ETA, " +
                                                        "\n\tCASE " +
                                                        "\n\t\tWHEN T1.MC3Type IS NULL THEN T3.MC3Type " +
                                                        "\n\t\tELSE T1.MC3Type " +
                                                        "\n\tEND AS MC3, CONCAT(MC3Name, '') AS MC3Name, " +
                                                        "\n\tCASE " +
                                                        "\n\t\tWHEN T1.MC3Status IS NULL THEN '' " +
                                                        "\n\t\tELSE " +
                                                        "\n\t\t\tCASE " +
                                                        "\n\t\t\tWHEN T1.MC3Status = 0 THEN 'STOP' " +
                                                        "\n\t\t\tWHEN T1.MC3Status = 1 THEN 'RUN' " +
                                                        "\n\t\t\tELSE 'FINISH' " +
                                                        "\n\t\tEND " +
                                                        "\n\tEND AS MC3Stat, T1.MC3ETA, T4.POSNo AS KITStatus FROM " +
                                                        "\n\t(SELECT PlanSeqNo, PosPDelDate, WIPCode, PosCNo, PosCQty, Remarks, SemiQty, MC1Type, MC1Name, MC1Status, MC1ETA, MC2Type, MC2Name, MC2Status, MC2ETA, MC3Type, MC3Name, MC3Status, MC3ETA, IPQCCheck, MC1QTY, MC2QTY, MC3QTY FROM tbPOSDetailofMC WHERE NOT PosCStatus = 2 AND PrintStatus=1 AND NOT PlanSeqNo IS NULL) T1 " +
                                                        "\n\tLEFT JOIN (SELECT ItemCode, ItemName, Remarks1, Remarks2, Remarks3 FROM tbMasterItem WHERE ItemType='Work In Process') T2 " +
                                                        "\n\tON T1.WIPCode = T2.ItemCode " +
                                                        "\n\tLEFT JOIN (SELECT ItemCode, Slot, MC1Type, MC2Type, MC3Type FROM tbMasterItemPlan) T3 " +
                                                        "\n\tON T2.ItemCode = T3.ItemCode " +
                                                        "\n\tLEFT JOIN (SELECT POSNo FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='KIT3' AND Funct=2) T4 " +
                                                        "\n\tON T1.PosCNo=T4.POSNo ) X " + WhereSelect +
                                                        "\n\tORDER BY PlanSeqNo ASC";
                //Console.WriteLine(FinalSQLQuery);
                SqlDataAdapter sda = new SqlDataAdapter(FinalSQLQuery, cnn.con);
                sda.Fill(dtPlansSearch);


                string POSNoIN = "";
                foreach (DataRow row in dtPlansSearch.Rows)
                {
                    if (POSNoIN.Contains(row["PosCNo"].ToString()) == false)
                    {
                        if (POSNoIN.Trim() == "")
                        {
                            POSNoIN = "'" + row["PosCNo"].ToString() + "'";
                        }
                        else
                        {
                            POSNoIN += ", '" + row["PosCNo"].ToString() + "'";
                        }
                    }
                }
                FinalSQLQuery = "SELECT * FROM tbSDAllocateStock " +
                    "\nWHERE POSNo IN (" + POSNoIN + ")";
                //Console.WriteLine(FinalSQLQuery);
                if (POSNoIN.Trim() != "")
                {
                    sda = new SqlDataAdapter(FinalSQLQuery, cnn.con);
                    sda.Fill(dtAlreadyTransfer);
                }
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

            //Remove Duplicate
            for (int i = dtPlansSearch.Rows.Count - 1; i > -1; i--)
            {
                int Found = 0;
                foreach (DataRow row in dtPlansSearch.Rows)
                {
                    if (dtPlansSearch.Rows[i]["PosCNo"].ToString() == row["PosCNo"].ToString())
                    {
                        Found++;
                        if (Found > 1)
                        {
                            break;
                        }
                    }
                }
                if (Found > 1)
                {
                    dtPlansSearch.Rows.RemoveAt(i);
                    dtPlansSearch.AcceptChanges();
                }
            }

            //Add to Dgv
            foreach (DataRow row in dtPlansSearch.Rows)
            {
                //Check already add or not
                int FoundDuplicate = 0;
                foreach (DataGridViewRow dgvRow in fgrid.dgvPOS.Rows)
                {
                    if (row["PosCNo"].ToString() == dgvRow.Cells["POSNo"].Value.ToString())
                    {
                        FoundDuplicate++;
                        break;
                    }
                }

                if (FoundDuplicate==0)
                {
                    dgvSearchResult.Rows.Add();
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["ChkSelect"].Value = false;
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["MachineName"].Value = row[CboMCNo.Text + "Name"].ToString();
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["SeqNo"].Value = Convert.ToInt32(row["PlanSeqNo"].ToString());
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["DelDate"].Value = Convert.ToDateTime(row["PosPDelDate"].ToString());
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["WIPCode"].Value = row["ItemCode"].ToString();
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["WIPName"].Value = row["ItemName"].ToString();
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["POSNo"].Value = row["PosCNo"].ToString();
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["PIN"].Value = row["Remarks1"].ToString();
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Wire"].Value = row["Remarks2"].ToString();
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Length"].Value = row["Remarks3"].ToString();
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["SemiQty"].Value = Convert.ToInt32(row["SemiQty"].ToString());
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Qty"].Value = Convert.ToInt32(row["PosCQty"].ToString());
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Remarks"].Value = row["Remarks"].ToString();
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Status"].Value = row[CboMCNo.Text + "Stat"].ToString();
                    bool Transfered = false;
                    foreach (DataRow rowTrans in dtAlreadyTransfer.Rows)
                    {
                        if (row["PosCNo"].ToString() == rowTrans["POSNo"].ToString())
                        {
                            Transfered = true;
                            break;
                        }
                    }
                    dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["AlreadyTransfer"].Value = Transfered;
                }
            }

            //Set Color to Dgv
            if (dgvSearchResult.Rows.Count > 0)
            {
                SetColorToDGV();
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                dgvSearchResult.ClearSelection();
                dgvSearchResult.CurrentCell = null;
                LbStatus.Text = "រកឃើញទិន្នន័យ " + dgvSearchResult.Rows.Count.ToString("N0");
                LbStatus.Refresh();
                ChkBtnOK();
            }
            else
            {
                MessageBox.Show("មានបញ្ហា!\n"+ErrorText,"Rachhan System",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SetColorToDGV()
        {
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                //Set Wire Color
                foreach (DataRow row in dtColor.Rows)
                {
                    if (dgvRow.Cells["Wire"].Value.ToString().Contains(row[0].ToString()))
                    {
                        dgvRow.Cells["Wire"].Style.ForeColor = Color.FromName(row[1].ToString());
                        dgvRow.Cells["Wire"].Style.BackColor = Color.FromName(row[2].ToString());
                    }
                }

                //Set Status Color
                if (dgvRow.Cells["Status"].Value.ToString() == "STOP")
                {
                    //SeqNo
                    dgvRow.Cells["SeqNo"].Style.ForeColor = Color.Black;
                    dgvRow.Cells["SeqNo"].Style.BackColor = Color.Green;
                    dgvRow.Cells["SeqNo"].Style.SelectionForeColor = Color.Black;
                    dgvRow.Cells["SeqNo"].Style.SelectionBackColor = Color.Green;

                    //Status
                    dgvRow.Cells["Status"].Style.ForeColor = Color.Black;
                    dgvRow.Cells["Status"].Style.BackColor = Color.Green;
                    dgvRow.Cells["Status"].Style.SelectionForeColor = Color.Black;
                    dgvRow.Cells["Status"].Style.SelectionBackColor = Color.Green;
                }
                else if (dgvRow.Cells["Status"].Value.ToString() == "RUN")
                {
                    //SeqNo
                    dgvRow.Cells["SeqNo"].Style.ForeColor = Color.Black;
                    dgvRow.Cells["SeqNo"].Style.BackColor = Color.Orange;
                    dgvRow.Cells["SeqNo"].Style.SelectionForeColor = Color.Black;
                    dgvRow.Cells["SeqNo"].Style.SelectionBackColor = Color.Orange;

                    //Status
                    dgvRow.Cells["Status"].Style.ForeColor = Color.Black;
                    dgvRow.Cells["Status"].Style.BackColor = Color.Orange;
                    dgvRow.Cells["Status"].Style.SelectionForeColor = Color.Black;
                    dgvRow.Cells["Status"].Style.SelectionBackColor = Color.Orange;
                }
                else if (dgvRow.Cells["Status"].Value.ToString() == "FINISH")
                {
                    //SeqNo
                    dgvRow.Cells["SeqNo"].Style.ForeColor = Color.White;
                    dgvRow.Cells["SeqNo"].Style.BackColor = Color.Red;
                    dgvRow.Cells["SeqNo"].Style.SelectionForeColor = Color.White;
                    dgvRow.Cells["SeqNo"].Style.SelectionBackColor = Color.Red;

                    //Status
                    dgvRow.Cells["Status"].Style.ForeColor = Color.White;
                    dgvRow.Cells["Status"].Style.BackColor = Color.Red;
                    dgvRow.Cells["Status"].Style.SelectionForeColor = Color.White;
                    dgvRow.Cells["Status"].Style.SelectionBackColor = Color.Red;
                }
            }

        }
        private void ChkBtnOK()
        {
            int CheckedFound = 0;
            foreach (DataGridViewRow dgvRow in dgvSearchResult.Rows)
            {
                if (dgvRow.Cells["ChkSelect"].Value.ToString().ToUpper() == "TRUE")
                {
                    CheckedFound++;
                    break;
                }
            }
            if (CheckedFound > 0)
            {
                btnOK.Enabled = true;
                btnOKGRAY.SendToBack();
            }
            else
            {
                btnOK.Enabled = false;
                btnOKGRAY.BringToFront();
            }
        }

    }
}
