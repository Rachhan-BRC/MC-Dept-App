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
using System.Windows.Markup.Localizer;

namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    public partial class StockCardForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        DataTable dtLoc;
        DataTable dtRM;

        string ErrorText;

        public StockCardForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Shown += StockCardForm_Shown;
            this.CboLoc.SelectedIndexChanged += CboLoc_SelectedIndexChanged;
            this.txtRMCode.Leave += TxtRMCode_Leave;
            this.txtRMCode.KeyDown += TxtRMCode_KeyDown;
            this.CboType.SelectedIndexChanged += CboType_SelectedIndexChanged;
            this.dtpFromDate.ValueChanged += DtpFromDate_ValueChanged;
            this.dtpTooDate.ValueChanged += DtpTooDate_ValueChanged;

            //Search RM
            this.btnShowFGSearch.Click += BtnShowFGSearch_Click;
            this.dgvRMSearch.LostFocus += DgvRMSearch_LostFocus;
            this.txtRMSearch.LostFocus += TxtRMSearch_LostFocus;
            this.txtRMSearch.TextChanged += TxtRMSearch_TextChanged;
            this.dgvRMSearch.CellClick += DgvRMSearch_CellClick;
        }
                
        //Search RM
        private void TxtRMSearch_LostFocus(object sender, EventArgs e)
        {
            if (txtRMSearch.Focused == false && dgvRMSearch.Focused == false)
            {
                panelRMSearch.SendToBack();
            }
        }
        private void DgvRMSearch_LostFocus(object sender, EventArgs e)
        {
            if (txtRMSearch.Focused == false && dgvRMSearch.Focused == false)
            {
                panelRMSearch.SendToBack();
            }
        }
        private void BtnShowFGSearch_Click(object sender, EventArgs e)
        {
            dgvRMSearch.Focus();
            RMSearch();
            panelRMSearch.BringToFront();
        }
        private void TxtRMSearch_TextChanged(object sender, EventArgs e)
        {
            RMSearch();
        }
                
        private void StockCardForm_Shown(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            foreach (DataGridViewColumn dgvCol in dgvTransaction.Columns)
            {
                string ColText = dgvCol.HeaderText.ToString();
                ColText = ColText.Replace("|","\n");
                dgvCol.HeaderText= ColText;
            }
            try
            {
                cnn.con.Open();
                dtLoc = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbSDMCLocation WHERE LocCode IN ('WIR1', 'KIT3')", cnn.con);
                sda.Fill(dtLoc);
                foreach (DataRow row in dtLoc.Rows)
                {
                    CboLoc.Items.Add(row["LocName"].ToString());
                }
                CboLoc.SelectedIndex = 0;
                dtRM = new DataTable();
                sda = new SqlDataAdapter("SELECT * FROM tbMasterItem WHERE ItemType='Material' ORDER BY ItemCode ASC", cnn.con);
                sda.Fill(dtRM);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();
            CboType.SelectedIndex = 1;
            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
            {
                MessageBox.Show("មានបញ្ហា!\n"+ ErrorText,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        private void CboLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SelectLocCode = "";
            foreach (DataRow row in dtLoc.Rows)
            {
                if (row["LocName"].ToString() == CboLoc.Text.ToString())
                {
                    SelectLocCode = row["LocCode"].ToString();
                    break;
                }
            }
            if (SelectLocCode == "KIT3")
            {
                LbType.Visible = true;
                CboType.Visible = true;
            }
            else
            {
                LbType.Visible = false;
                CboType.Visible = false;
            }
            TakingStockCardData();
        }
        private void TxtRMCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TakingStockCardData();
            }
        }
        private void TxtRMCode_Leave(object sender, EventArgs e)
        {
            TakingStockCardData();
        }
        private void CboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            TakingStockCardData();
        }
        private void DgvRMSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvRMSearch.SelectedCells.Count > 0 && dgvRMSearch.CurrentCell != null && dgvRMSearch.CurrentCell.RowIndex > -1)
            {
                txtRMCode.Text = dgvRMSearch.Rows[e.RowIndex].Cells["RMCode"].Value.ToString();
                txtRMName.Focus();
                TakingStockCardData();
            }
        }
        private void DtpTooDate_ValueChanged(object sender, EventArgs e)
        {
            TakingStockCardData();
        }
        private void DtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            TakingStockCardData();
        }

        //Method
        private void RMSearch()
        {
            dgvRMSearch.Rows.Clear();
            foreach (DataRow row in dtRM.Rows)
            {
                int OK = 0;
                if (txtRMSearch.Text.Trim() == "")
                {
                    OK++;
                }
                else
                {
                    if (row["ItemCode"].ToString().ToLower().Contains(txtRMSearch.Text.ToLower()) == true || row["ItemName"].ToString().ToLower().Contains(txtRMSearch.Text.ToLower()) == true)
                    {
                        OK++;
                    }
                }

                if (OK > 0)
                {
                    dgvRMSearch.Rows.Add();
                    dgvRMSearch.Rows[dgvRMSearch.Rows.Count - 1].Cells["RMCode"].Value = row["ItemCode"].ToString();
                    dgvRMSearch.Rows[dgvRMSearch.Rows.Count - 1].Cells["RMName"].Value = row["ItemName"].ToString();
                }
            }
            dgvRMSearch.ClearSelection();
        }
        private void TakingStockCardData()
        {
            if (txtRMCode.Text.Trim() != "")
            {
                dgvTransaction.Rows.Clear();
                txtRMName.Text = "";
                foreach (DataRow row in dtRM.Rows)
                {
                    if (row["ItemCode"].ToString() == txtRMCode.Text)
                    {
                        txtRMName.Text = row["ItemName"].ToString();
                        break;
                    }
                }

                if (txtRMName.Text.Trim() != "")
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;
                    DataTable dtSQLCond = new DataTable();
                    dtSQLCond.Columns.Add("Col");
                    dtSQLCond.Columns.Add("Val");
                    dtSQLCond.Rows.Add("CancelStatus = ", "0");
                    string LocCode = "KIT3";
                    foreach (DataRow row in dtLoc.Rows)
                    {
                        if (row["LocName"].ToString() == CboLoc.Text)
                        {
                            LocCode = row["LocCode"].ToString();
                        }
                    }
                    dtSQLCond.Rows.Add("LocCode = ", "'" + LocCode + "'");
                    if (CboType.Visible == true && CboType.Text.ToUpper() != "ALL")
                    {
                        dtSQLCond.Rows.Add("POSNo = ", "''");
                    }
                    dtSQLCond.Rows.Add("Code = ", "'" + txtRMCode.Text + "'");

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

                    DataTable dtBeginningStock = new DataTable();
                    DataTable dtTransaction = new DataTable();
                    try
                    {
                        cnn.con.Open();
                        string SQLQuery = "SELECT StockValue, RegDate, ReceiveQty, TransferQty, " +
                        "\nCASE " +
                        "\n\tWHEN Remarks IS NOT NULL AND NOT TRIM(Remarks) = '' THEN Remarks " +
                        "\n\tELSE POSNo " +
                        "\nEND AS DocNo,  RegBy FROM tbSDMCAllTransaction " + SQLConds + " AND RegDate BETWEEN '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dtpTooDate.Value.ToString("yyyy-MM-dd") + " 23:59:59' " +
                        "\nORDER BY RegDate ASC";
                        Console.WriteLine(SQLQuery + "\n\n");
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        sda.Fill(dtTransaction);


                        SQLQuery = "SELECT Code, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction " + SQLConds + " AND RegDate < '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + " 00:00:00' " +
                            "\nGROUP BY Code";
                        Console.WriteLine(SQLQuery);
                        sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        sda.Fill(dtBeginningStock);


                    }
                    catch (Exception ex)
                    {
                        ErrorText = ex.Message;
                    }
                    cnn.con.Close();

                    //Beginnin Stock
                    double BalanceStock = 0;
                    if (dtBeginningStock.Rows.Count > 0 && dtBeginningStock.Rows[0]["StockValue"].ToString().Trim() != "")
                    {
                        dgvTransaction.Rows.Add();
                        dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["Date"].Value = dtpFromDate.Value.AddDays(-1);
                        dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["StockQty"].Value = Convert.ToDouble(dtBeginningStock.Rows[0]["StockValue"].ToString());
                        dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["DocNo"].Value = "Beginning Stock";
                        BalanceStock = Convert.ToDouble(dtBeginningStock.Rows[0]["StockValue"].ToString());
                    }
                    else
                    {
                        dgvTransaction.Rows.Add();
                        dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["Date"].Value = dtpFromDate.Value.AddDays(-1);
                        dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["StockQty"].Value = 0;
                        dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["DocNo"].Value = "Beginning Stock";
                    }

                    //Transaction
                    foreach (DataRow row in dtTransaction.Rows)
                    {
                        dgvTransaction.Rows.Add();
                        double RecQty = Convert.ToDouble(row["ReceiveQty"].ToString());
                        double TransQty = Convert.ToDouble(row["TransferQty"].ToString());
                        dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["Date"].Value = Convert.ToDateTime(row["RegDate"].ToString());
                        if (RecQty > 0)
                        {
                            dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["InStock"].Value = RecQty;
                            BalanceStock += RecQty;
                        }
                        else
                        {
                            dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["OutStock"].Value = TransQty;
                            BalanceStock -= TransQty;
                        }
                        dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["StockQty"].Value = BalanceStock;
                        dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["DocNo"].Value = row["DocNo"].ToString();
                        dgvTransaction.Rows[dgvTransaction.Rows.Count - 1].Cells["PIC"].Value = row["RegBy"].ToString();
                    }

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        dgvTransaction.ClearSelection();
                    }
                    else
                    {
                        MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
