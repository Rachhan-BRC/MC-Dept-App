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
using System.Xml;

namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    public partial class KITandSDForProductionSearch : Form
    {
        SQLConnect cnn = new SQLConnect();
        DataTable dtLoc;
        string SelectedLocCode;
        string ErrorText;

        public KITandSDForProductionSearch()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Shown += KITandSDForProductionSearch_Shown;
            
            //btn
            this.btnSearch.Click += BtnSearch_Click;
            this.btnDetails.Click += BtnDetails_Click;

            //dgv
            this.dgvSearchResult.CurrentCellChanged += DgvSearchResult_CurrentCellChanged;
            this.dgvSearchResult.DoubleClick += DgvSearchResult_DoubleClick;

        }

        //btn
        private void BtnDetails_Click(object sender, EventArgs e)
        {
            ShowingDetails();
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            HideDetails();
            ErrorText = "";
            dgvSearchResult.Rows.Clear();
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . . ";
            LbStatus.Refresh();

            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Val");

            //Check Loc First
            SelectedLocCode = "";
            foreach (DataRow row in dtLoc.Rows)
            {
                if (CboLoc.Text.ToString() == row["LocName"].ToString())
                {
                    SelectedLocCode = row["LocCode"].ToString();
                }
            }

            string SQLQuery = "";
            if (SelectedLocCode.Trim() != "")
            {
                //SD MC
                if (SelectedLocCode == "WIR1")
                {
                    dtSQLCond.Rows.Add("TbTsct.RegDate BETWEEN ", "'" + dtpFromDate.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dtpTooDate.Value.ToString("yyyy-MM-dd") + " 23:59:59'");
                    if (txtCode.Text.Trim() != "")
                    {
                        dtSQLCond.Rows.Add("TbTsct.Code = ", "'" + txtCode.Text + "'");
                    }
                    if (txtName.Text.Trim() != "")
                    {
                        dtSQLCond.Rows.Add("TbMstItem.ItemName LIKE ", "'%" + txtName.Text + "%'");
                    }
                    if (txtPOSNo.Text.Trim() != "")
                    {
                        string SearchValue = txtPOSNo.Text;
                        if (SearchValue.Contains("*") == true)
                        {
                            SearchValue = SearchValue.Replace("*", "%");
                            dtSQLCond.Rows.Add("TbAllo.POSNo = ", "'" + SearchValue + "'");
                        }
                        else
                        {
                            dtSQLCond.Rows.Add("TbAllo.POSNo = ", "'" + SearchValue + "'");
                        }
                    }
                    if (txtDocNo.Text.Trim() != "")
                    {
                        string SearchValue = txtDocNo.Text;
                        if (SearchValue.Contains("*") == true)
                        {
                            SearchValue = SearchValue.Replace("*", "%");
                            dtSQLCond.Rows.Add("TbTsct.Remarks = ", "'" + SearchValue + "'");
                        }
                        else
                        {
                            dtSQLCond.Rows.Add("TbTsct.Remarks = ", "'" + SearchValue + "'");
                        }
                    }
                    if (CboMCName.Text.Trim() != "")
                    {
                        dtSQLCond.Rows.Add("TbAllo.MCName LIKE ", "'%" + CboMCName.Text + "%'");
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

                    SQLQuery = "SELECT RMCode, RMName, RegDate, MCName, TransferQty, DocNo FROM " +
                        "\n (SELECT Code AS RMCode, TbMstItem.ItemName AS RMName, TbTsct.RegDate, TbAllo.MCName, TransferQty, TbTsct.Remarks AS DocNo FROM " +
                        "\n\t(SELECT Code, CAST(RegDate AS date) AS RegDate, SUM(TransferQty) AS TransferQty, Remarks FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND Funct=2 AND LocCode='WIR1' GROUP BY LocCode, Code, RegDate, Remarks) TbTsct " +
                        "\n\tLEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') TbMstItem ON TbTsct.Code=TbMstItem.ItemCode " +
                        "\n\tINNER JOIN (SELECT SysNo, MCName, POSNo FROM tbSDAllocateStock) TbAllo ON TbTsct.Remarks=TbAllo.SysNo \n\t" + SQLConds +
                        "\n ) X " +
                        "\nGROUP BY RMCode, RMName, RegDate, MCName, TransferQty, DocNo " +
                        "\nORDER BY RMCode ASC, RegDate ASC, DocNo ASC";
                }
                //KIT
                else
                {
                    dtSQLCond.Rows.Add("TbTsct.RegDate BETWEEN ", "'" + dtpFromDate.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dtpTooDate.Value.ToString("yyyy-MM-dd") + " 23:59:59'");
                    if (txtCode.Text.Trim() != "")
                    {
                        dtSQLCond.Rows.Add("TbTsct.Code = ", "'" + txtCode.Text + "'");
                    }
                    if (txtName.Text.Trim() != "")
                    {
                        dtSQLCond.Rows.Add("TbMstItem.ItemName LIKE ", "'%" + txtName.Text + "%'");
                    }
                    if (txtPOSNo.Text.Trim() != "" || txtDocNo.Text.Trim() != "")
                    {
                        if (txtPOSNo.Text.Trim() != "" && txtDocNo.Text.Trim() != "")
                        {
                            string SearchValue = txtPOSNo.Text;
                            if (SearchValue.Contains("*") == true)
                            {
                                SearchValue = SearchValue.Replace("*", "%");
                                dtSQLCond.Rows.Add("TbTsct.POSNo LIKE '%" + SearchValue + "%' ", "OR TbTsct.Remarks LIKE '%" + SearchValue + "%'");
                            }
                            else
                            {
                                dtSQLCond.Rows.Add("TbTsct.POSNo = '" + SearchValue + "' ", "OR TbTsct.Remarks = '" + SearchValue + "'");
                            }
                        }
                        else
                        {
                            string SearchValue = txtPOSNo.Text;
                            if (SearchValue.Contains("*") == true)
                            {
                                SearchValue = SearchValue.Replace("*", "%");
                                dtSQLCond.Rows.Add("TbTsct.POSNo LIKE '%" + SearchValue + "%' ", "OR TbTsct.Remarks LIKE '%" + SearchValue + "%'");
                            }
                            else
                            {
                                dtSQLCond.Rows.Add("TbTsct.POSNo = '" + SearchValue + "' ", "OR TbTsct.Remarks = '" + SearchValue + "'");
                            }
                        }
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
                    string SQLConds2 = "";
                    if (CboMCName.Text.Trim() != "")
                    {
                        SQLConds2 = "WHERE TbPOS.MCName LIKE '%"+CboMCName.Text+"%'";
                    }

                    SQLQuery = "SELECT RMCode, RMName, X.RegDate, TbPOS.MCName, TransferQty, X.DocNo FROM " +
                        "\n(SELECT Code AS RMCode, TbMstItem.ItemName AS RMName, TbTsct.RegDate, TransferQty, " +
                        "\nCASE " +
                        "\n\tWHEN TbTsct.POSNo IS NOT NULL AND NOT TRIM(TbTsct.POSNo) = '' AND TbTsct.Remarks IS NULL THEN TbTsct.POSNo " +
                        "\n\tELSE TbTsct.Remarks " +
                        "\nEND AS DocNo FROM " +
                        "\n (SELECT Code, CAST(RegDate AS date) AS RegDate, SUM(TransferQty) AS TransferQty, POSNo, Remarks FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND Funct=2 AND LocCode='KIT3' GROUP BY LocCode, Code, RegDate, POSNo, Remarks) TbTsct " +
                        "\n LEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') TbMstItem ON TbTsct.Code=TbMstItem.ItemCode \n" + SQLConds +
                        "\n) X " +
                        "\n INNER JOIN (SELECT PosCNo , " +
                        "\n NULLIF(CONCAT(MC1Name, " +
                        "\n CASE  " +
                        "\n\tWHEN LEN(MC2Name)>1 THEN ' & '  " +
                        "\n\tELSE '' " +
                        "\n END, MC2Name, " +
                        "\n CASE " +
                        "\n\tWHEN LEN(MC3Name)>1 THEN ' & ' " +
                        "\n\tELSE '' " +
                        "\n END, MC3Name),'') AS MCName FROM tbPOSDetailofMC) TbPOS " +
                        "\nON LEFT(X.DocNo,10) = TbPOS.PosCNo \n" + SQLConds2 +
                        "\nGROUP BY RMCode, RMName, X.RegDate, MCName, TransferQty, X.DocNo " +
                        "\nORDER BY RMCode ASC, RegDate ASC, DocNo ASC";
                }
            }

            //Console.WriteLine(SQLQuery);

            //Taking data
            DataTable dtSearchResult = new DataTable();
            if (SelectedLocCode.Trim() != "")
            {
                try
                {
                    cnn.con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtSearchResult);
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                }
                cnn.con.Close();
            }

            //Add DGV
            foreach (DataRow row in dtSearchResult.Rows)
            {
                dgvSearchResult.Rows.Add();
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].HeaderCell.Value = dgvSearchResult.Rows.Count.ToString();
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["RMCode"].Value = row["RMCode"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["RMName"].Value = row["RMName"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["RegDate"].Value = Convert.ToDateTime(row["RegDate"].ToString());
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["MCName"].Value = row["MCName"].ToString();
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["Qty"].Value = Convert.ToDouble(row["TransferQty"].ToString());
                dgvSearchResult.Rows[dgvSearchResult.Rows.Count - 1].Cells["DocNo"].Value = row["DocNo"].ToString();
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
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Dgv
        private void DgvSearchResult_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dgvSearchResult.SelectedCells.Count > 0 && dgvSearchResult.CurrentCell != null && dgvSearchResult.CurrentCell.RowIndex > -1)
            {
                btnDetails.Enabled = true;
                btnDetailsGRAY.SendToBack();
                if (GrbDetails.Visible == true)
                {
                    ShowingDetails();
                }
            }
            else
            {
                btnDetails.Enabled = false;
                btnDetailsGRAY.BringToFront();
            }
        }
        private void DgvSearchResult_DoubleClick(object sender, EventArgs e)
        {
            ShowingDetails();
        }

        private void KITandSDForProductionSearch_Shown(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;

            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbSDMCLocation WHERE LocCode IN ('KIT3', 'WIR1') ORDER BY LocCode ASC", cnn.con);
                dtLoc = new DataTable();
                sda.Fill(dtLoc);
                sda = new SqlDataAdapter("SELECT * FROM tbMasterMCType ORDER BY MCName ASC", cnn.con);
                DataTable dtMCName = new DataTable();
                sda.Fill(dtMCName);

                foreach (DataRow row in dtLoc.Rows)
                {
                    CboLoc.Items.Add(row["LocName"]);
                }
                foreach (DataRow row in dtMCName.Rows)
                {
                    CboMCName.Items.Add(row["MCName"]);
                }

            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();
            CboLoc.SelectedIndex = 0;
            HideDetails();

            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
            {
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //Method
        private void ShowingDetails()
        {
            if(dgvSearchResult.SelectedCells.Count > 0 && dgvSearchResult.CurrentCell!=null && dgvSearchResult.CurrentCell.RowIndex>-1 && SelectedLocCode.Trim()!="")
            {
                ErrorText = "";
                Cursor = Cursors.WaitCursor;
                dgvDetails.Rows.Clear();
                string SelectedRMCode = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells["RMCode"].Value.ToString();
                string SelectedDocNo = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells["DocNo"].Value.ToString();
                DataTable dtDetails = new DataTable();
                string SQLQuery = "";
                if (SelectedLocCode.Trim() == "WIR1")
                {
                    SQLQuery = "SELECT SysNo, WIPCode, MCName, POSNo, ItemName, PosCQty, (PosCQty * LowQty) AS ActaulUseQty, TransferQty FROM " +
                        "\n(SELECT Code, SUM(TransferQty) AS TransferQty, Remarks FROM tbSDMCAllTransaction " +
                        "\nWHERE CancelStatus=0 AND Funct=2 AND LocCode='WIR1' AND Remarks='"+SelectedDocNo+"' AND Code='"+SelectedRMCode+"' " +
                        "\nGROUP BY LocCode, Code, RegDate, POSNo, Remarks) TbTst " +
                        "\nINNER JOIN (SELECT SysNo, POSNo FROM tbSDAllocateStock) TbAllo " +
                        "\nON TbTst.Remarks=TbAllo.SysNo " +
                        "\nINNER JOIN (SELECT WIPCode, PosCNo, PosCQty, " +
                        "\n NULLIF(CONCAT(MC1Name, " +
                        "\n CASE  " +
                        "\n\tWHEN LEN(MC2Name)>1 THEN ' & '  " +
                        "\n\tELSE '' " +
                        "\n END, MC2Name, " +
                        "\n CASE " +
                        "\n\tWHEN LEN(MC3Name)>1 THEN ' & ' " +
                        "\n\tELSE '' END, MC3Name),'') AS MCName FROM tbPOSDetailofMC) TbPOSDetailSS " +
                        "\n ON TbAllo.POSNo=TbPOSDetailSS.PosCNo " +
                        "\n LEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Work In Process') TbMstItem " +
                        "\n ON TbPOSDetailSS.WIPCode=TbMstItem.ItemCode" +
                        "\n INNER JOIN (SELECT * FROM MstBOM) TbBOMM " +
                        "\n ON TbPOSDetailSS.WIPCode=TbBOMM.UpItemCode AND TbTst.Code=TbBOMM.LowItemCode";
                }
                else
                {
                    SQLQuery = "SELECT '' AS SysNo, WIPCode, MCName, POSNo , ItemName, PosCQty, ActaulUseQty, SUM(TransferQty) AS TransferQty FROM " +
                        "\n (SELECT WIPCode, MCName, TbTst.Remarks AS POSNo, ItemName, PosCQty, (PosCQty * LowQty) AS ActaulUseQty, TransferQty FROM " +
                        "\n (SELECT Code, SUM(TransferQty) AS TransferQty, LEFT(Remarks, 10) AS Remarks FROM tbSDMCAllTransaction WHERE CancelStatus = 0 AND LocCode = 'KIT3' AND Funct=2 AND Code = '"+SelectedRMCode+"' AND Remarks LIKE '"+SelectedDocNo.Substring(0,10).ToString()+"%' GROUP BY LocCode, Code, RegDate, Remarks) TbTst " +
                        "\n INNER JOIN (SELECT WIPCode, PosCNo, PosCQty, " +
                        "\n\t NULLIF(CONCAT(MC1Name, " +
                        "\n\t CASE " +
                        "\n\t\tWHEN LEN(MC2Name)>1 THEN ' & '  " +
                        "\n\t\tELSE '' " +
                        "\n\t END, MC2Name, " +
                        "\n\t CASE " +
                        "\n\t\tWHEN LEN(MC3Name)>1 THEN ' & ' " +
                        "\n\t\tELSE '' END, MC3Name),'') AS MCName FROM tbPOSDetailofMC) TbPOSDetailSS ON TbTst.Remarks = TbPOSDetailSS.PosCNo " +
                        "\n LEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Work In Process') TbMstItem ON TbPOSDetailSS.WIPCode=TbMstItem.ItemCode " +
                        "\n INNER JOIN (SELECT * FROM MstBOM) TbBOMM ON TbPOSDetailSS.WIPCode=TbBOMM.UpItemCode AND TbTst.Code=TbBOMM.LowItemCode) X " +
                        "\nGROUP BY WIPCode, MCName, POSNo, ItemName, PosCQty, ActaulUseQty";
                }

                //Console.WriteLine(SQLQuery);

                try
                {
                    cnn.con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtDetails);
                }
                catch (Exception ex) 
                {
                    ErrorText = ex.Message;
                }
                cnn.con.Close();

                if (ErrorText.Trim() == "")
                {
                    foreach (DataRow row in dtDetails.Rows)
                    {
                        dgvDetails.Rows.Add();
                        dgvDetails.Rows[dgvDetails.Rows.Count-1].HeaderCell.Value = dgvDetails.Rows.Count.ToString();
                        dgvDetails.Rows[dgvDetails.Rows.Count-1].Cells["MCNameDetails"].Value = row["MCName"].ToString();
                        dgvDetails.Rows[dgvDetails.Rows.Count-1].Cells["POSNo"].Value = row["POSNo"].ToString();
                        dgvDetails.Rows[dgvDetails.Rows.Count-1].Cells["WIPName"].Value = row["ItemName"].ToString();
                        dgvDetails.Rows[dgvDetails.Rows.Count-1].Cells["POSQty"].Value = Convert.ToDouble(row["PosCQty"].ToString());
                        dgvDetails.Rows[dgvDetails.Rows.Count-1].Cells["ActualQty"].Value = Convert.ToDouble(row["ActaulUseQty"].ToString());
                        dgvDetails.Rows[dgvDetails.Rows.Count-1].Cells["TransferQty"].Value = Convert.ToDouble(row["TransferQty"].ToString());
                    }
                    dgvDetails.ClearSelection();
                }

                Cursor = Cursors.Default;

                if (ErrorText.Trim() == "")
                {
                    GrbDetails.Visible = true;
                    splitter1.Visible = true;
                }
                else
                {
                    MessageBox.Show("មានបញ្ហា!\n"+ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void HideDetails()
        {
            GrbDetails.Visible = false;
            splitter1.Visible = false;
        }
    }
}
