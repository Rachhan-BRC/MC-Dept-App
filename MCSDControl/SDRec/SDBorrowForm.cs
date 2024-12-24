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

namespace MachineDeptApp.MCSDControl.SDRec
{
    public partial class SDBorrowForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SqlCommand cmd;
        string SelectedSysNo;
        string SelectedPOSNo;
        string SelectedCode;
        string SelectedItems;
        DataTable dtStock;
        DataTable dtStockOtherPOS;
        DataTable dtBorrow;

        public SDBorrowForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Load += SDBorrowForm_Load;
            this.dgvPOS.CellClick += DgvPOS_CellClick;
            this.dgvConsumpDetails.CellFormatting += DgvConsumpDetails_CellFormatting;
            this.dgvConsumpDetails.CellClick += DgvConsumpDetails_CellClick;
            this.btnTakeIn.Click += BtnTakeIn_Click;
            this.btnTakeOut.Click += BtnTakeOut_Click;
            this.btnSave.Click += BtnSave_Click;
            this.btnSearch.Click += BtnSearch_Click;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            dgvPOS.Rows.Clear();
            dgvConsumpDetails.Rows.Clear();
            dgvOtherPOS.ClearSelection();
            groupBox3.Text = "ដែលមាននៅ POS ផ្សេង";
            groupBox3.Refresh();
            Cursor = Cursors.WaitCursor;
            Search();
            AssignDgvNo();
            Cursor = Cursors.Default;

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DialogResult DLS = MessageBox.Show("តើអ្នកចង់រក្សាទុកទិន្នន័យនេះមែនដែរឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DLS == DialogResult.Yes)
            {
                string Errors = "";
                //Take System No 
                string RegNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string User = MenuFormV2.UserForNextForm;
                string TransNo = "";
                try
                {
                    //Find Last TransNo
                    cnn.con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT SysNo FROM tbSDMCAllTransaction WHERE RegDate=(SELECT MAX(RegDate) FROM tbSDMCAllTransaction WHERE Funct =3) Group By SysNo", cnn.con);
                    DataTable dtTransNo = new DataTable();
                    da.Fill(dtTransNo);
                    if (dtTransNo.Rows.Count == 0)
                    {
                        TransNo = "BOR0000000001";
                    }
                    else
                    {
                        string LastTransNo = dtTransNo.Rows[0][0].ToString();
                        double NextTransNo = Convert.ToDouble(LastTransNo.Substring(LastTransNo.Length - 10)) + 1;
                        TransNo = "BOR" + NextTransNo.ToString("0000000000");

                    }

                }
                catch (Exception ex)
                {
                    if (Errors.Trim() == "")
                    {
                        Errors = ex.Message;
                    }
                    else
                    {
                        Errors = Errors + "\t" + ex.Message;
                    }
                }
                cnn.con.Close();

                if (Errors.Trim() == "")
                {
                    //Update Status to tbSDConn1Rec​ if complete KIT
                    int FoundAbnormal = 0;
                    foreach (DataGridViewRow row in dgvConsumpDetails.Rows)
                    {
                        if (Convert.ToDouble(row.Cells[2].Value.ToString()) > Convert.ToDouble(row.Cells[3].Value.ToString()))
                        {
                            FoundAbnormal = FoundAbnormal + 1;
                        }
                    }
                    if (FoundAbnormal == 0)
                    {
                        try
                        {
                            cnn.con.Open();
                            string query = "UPDATE tbSDConn1Rec SET " +
                                                "Status=2, " +
                                                "UpdateDate='" + RegNow + "'," +
                                                "UpdateBy=N'" + User + "' " +
                                                "WHERE SysNo = '" + SelectedSysNo + "';";
                            SqlCommand cmd = new SqlCommand(query, cnn.con);
                            cmd.ExecuteNonQuery();

                        }
                        catch (Exception ex)
                        {
                            if (Errors.Trim() == "")
                            {
                                Errors = ex.Message;
                            }
                            else
                            {
                                Errors = Errors + "\t" + ex.Message;
                            }
                        }
                        cnn.con.Close();
                    }

                    //Update Status to tbSDConn1Rec for tbBorrow (borrow from) && Add to Transaction
                    foreach (DataRow row in dtBorrow.Rows)
                    {
                        string Code = row[0].ToString();
                        string RMDes = row[1].ToString();
                        string BorrowTo = row[2].ToString();
                        string BorrowFrom = row[3].ToString();
                        double Qty = Convert.ToDouble(row[4].ToString());

                        //Update Status to tbSDConn1Rec for tbBorrow (borrow from)
                        try
                        {
                            cnn.con.Open();
                            string query = "UPDATE tbSDConn1Rec SET " +
                                                "Status=1, " +
                                                "UpdateDate='" + RegNow + "'," +
                                                "UpdateBy=N'" + User + "' " +
                                                "WHERE NOT Status=0 AND POSNo = '" + BorrowFrom + "';";
                            SqlCommand cmd = new SqlCommand(query, cnn.con);
                            cmd.ExecuteNonQuery();

                        }
                        catch (Exception ex)
                        {
                            if (Errors.Trim() == "")
                            {
                                Errors = ex.Message;
                            }
                            else
                            {
                                Errors = Errors + "\t" + ex.Message;
                            }
                        }
                        cnn.con.Close();

                        //Add to Transaction
                        try
                        {
                            cnn.con.Open();
                            //Transaction add
                            cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus ) " +
                                                           "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs)", cnn.con);
                            cmd.Parameters.AddWithValue("@Sn", TransNo);
                            cmd.Parameters.AddWithValue("@Ft", 3);
                            cmd.Parameters.AddWithValue("@Lc", "KIT3");
                            cmd.Parameters.AddWithValue("@Pn", BorrowTo);
                            cmd.Parameters.AddWithValue("@Cd", Code);
                            cmd.Parameters.AddWithValue("@Rmd", RMDes);
                            cmd.Parameters.AddWithValue("@Rq", Qty);
                            cmd.Parameters.AddWithValue("@Tq", 0);
                            cmd.Parameters.AddWithValue("@Sv", Qty);
                            cmd.Parameters.AddWithValue("@Rd", RegNow);
                            cmd.Parameters.AddWithValue("@Rb", User);
                            cmd.Parameters.AddWithValue("@Cs", 0);
                            cmd.ExecuteNonQuery();

                            //Add to Transaction Minus
                            cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus ) " +
                                                           "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs)", cnn.con);
                            cmd.Parameters.AddWithValue("@Sn", TransNo);
                            cmd.Parameters.AddWithValue("@Ft", 3);
                            cmd.Parameters.AddWithValue("@Lc", "KIT3");
                            cmd.Parameters.AddWithValue("@Pn", BorrowFrom);
                            cmd.Parameters.AddWithValue("@Cd", Code);
                            cmd.Parameters.AddWithValue("@Rmd", RMDes);
                            cmd.Parameters.AddWithValue("@Rq", 0);
                            cmd.Parameters.AddWithValue("@Tq", Qty);
                            cmd.Parameters.AddWithValue("@Sv", (Qty * (-1)));
                            cmd.Parameters.AddWithValue("@Rd", RegNow);
                            cmd.Parameters.AddWithValue("@Rb", User);
                            cmd.Parameters.AddWithValue("@Cs", 0);
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            if (Errors.Trim() == "")
                            {
                                Errors = ex.Message;
                            }
                            else
                            {
                                Errors = Errors + "\t" + ex.Message;
                            }
                        }
                        cnn.con.Close();

                    }
                }

                if (Errors.Trim() == "")
                {
                    dgvPOS.ClearSelection();
                    dgvConsumpDetails.Rows.Clear();
                    dgvOtherPOS.Rows.Clear();
                    CleardtBorrow();
                    CheckBtnSave();
                    MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(Errors, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void BtnTakeOut_Click(object sender, EventArgs e)
        {
            if (dgvConsumpDetails.SelectedCells.Count > 0 && dtBorrow.Rows.Count > 0)
            {
                for (int i = dtBorrow.Rows.Count - 1; i > -1; i--)
                {
                    if (SelectedCode == dtBorrow.Rows[i][0].ToString())
                    {
                        //Return back to dgvOther 
                        int foundRemain = 0;
                        for (int j = 0; j < dgvOtherPOS.Rows.Count; j++)
                        {
                            if (dgvOtherPOS.Rows[j].Cells[0].Value.ToString() == dtBorrow.Rows[i][3].ToString())
                            {
                                foundRemain = foundRemain + 1;
                            }
                        }
                        if (foundRemain > 0)
                        {
                            for (int a = 0; a < dgvOtherPOS.Rows.Count; a++)
                            {
                                if (dgvOtherPOS.Rows[a].Cells[0].Value.ToString() == dtBorrow.Rows[i][3].ToString())
                                {
                                    dgvOtherPOS.Rows[a].Cells[1].Value = Convert.ToDouble(dgvOtherPOS.Rows[a].Cells[1].Value.ToString()) + Convert.ToDouble(dtBorrow.Rows[i][4].ToString());
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (dgvOtherPOS.Rows.Count == 0)
                            {
                                dgvOtherPOS.Rows.Add(dtBorrow.Rows[i][3].ToString(), Convert.ToDouble(dtBorrow.Rows[i][4].ToString()));
                            }
                            else
                            {
                                DataGridViewRow newRow = (DataGridViewRow)dgvOtherPOS.Rows[0].Clone();
                                newRow.Cells[0].Value = dtBorrow.Rows[i][3].ToString();
                                newRow.Cells[1].Value = Convert.ToDouble(dtBorrow.Rows[i][4].ToString());
                                dgvOtherPOS.Rows.Insert(0, newRow);
                            }
                        }

                        //Return back to dtStockOtherPOS 
                        foundRemain = 0;
                        foreach (DataRow row in dtStockOtherPOS.Rows)
                        {
                            if (SelectedCode == row[0].ToString() && dtBorrow.Rows[i][3].ToString() == row[1].ToString())
                            {
                                foundRemain = foundRemain + 1;
                            }
                        }
                        if (foundRemain > 0)
                        {
                            foreach (DataRow row in dtStockOtherPOS.Rows)
                            {
                                if (SelectedCode == row[0].ToString() && dtBorrow.Rows[i][3].ToString() == row[1].ToString())
                                {
                                    row[2] = Convert.ToDouble(row[2].ToString()) + Convert.ToDouble(dtBorrow.Rows[i][4].ToString());
                                }
                            }
                        }
                        else
                        {
                            //Create a new row
                            DataRow newRow = dtStockOtherPOS.NewRow();
                            //Set the values for the new row
                            newRow[0] = SelectedCode;
                            newRow[1] = dtBorrow.Rows[i][3].ToString();
                            newRow[2] = dtBorrow.Rows[i][4].ToString();
                            //Insert the new row at the first index
                            dtStockOtherPOS.Rows.InsertAt(newRow, 0);
                        }

                        //Calc Qty in dgvConsump
                        dgvConsumpDetails.Rows[dgvConsumpDetails.CurrentCell.RowIndex].Cells[3].Value = Convert.ToDouble(dgvConsumpDetails.Rows[dgvConsumpDetails.CurrentCell.RowIndex].Cells[3].Value.ToString()) - Convert.ToDouble(dtBorrow.Rows[i][4].ToString());
                        //Remove from dtBorrow
                        dtBorrow.Rows.RemoveAt(i);
                        break;
                    }
                }

                CheckBtnSave();

            }
        }

        private void BtnTakeIn_Click(object sender, EventArgs e)
        {
            if (dgvConsumpDetails.SelectedCells.Count > 0 && dgvOtherPOS.SelectedCells.Count > 0)
            {
                double ConsumpQty = 0;
                double CurrentTranQty = 0;
                double ShortageQty = 0;
                ConsumpQty = Convert.ToDouble(dgvConsumpDetails.CurrentRow.Cells[2].Value.ToString());
                CurrentTranQty = Convert.ToDouble(dgvConsumpDetails.CurrentRow.Cells[3].Value.ToString());
                ShortageQty = ConsumpQty - CurrentTranQty;

                if (ConsumpQty > CurrentTranQty)
                {
                    double OtherPOSQty = 0;
                    OtherPOSQty = Convert.ToDouble(dgvOtherPOS.CurrentRow.Cells[1].Value.ToString());

                    if (ShortageQty >= OtherPOSQty)
                    {
                        for (int i = dtStockOtherPOS.Rows.Count - 1; i > -1; i--)
                        {
                            if (dtStockOtherPOS.Rows[i][0].ToString() == SelectedCode && dtStockOtherPOS.Rows[i][1].ToString() == dgvOtherPOS.CurrentRow.Cells[0].Value.ToString())
                            {
                                dtStockOtherPOS.Rows.RemoveAt(i);
                            }
                        }
                        dtBorrow.Rows.Add(SelectedCode, SelectedItems, SelectedPOSNo, dgvOtherPOS.CurrentRow.Cells[0].Value.ToString(), Convert.ToDouble(dgvOtherPOS.CurrentRow.Cells[1].Value.ToString()));
                        CurrentTranQty = CurrentTranQty + OtherPOSQty;
                        dgvConsumpDetails.CurrentRow.Cells[3].Value = CurrentTranQty;
                        int selectedIndex = dgvOtherPOS.CurrentCell.RowIndex;
                        if (selectedIndex > -1)
                        {
                            dgvOtherPOS.Rows.RemoveAt(selectedIndex);
                            dgvOtherPOS.Refresh();
                        }
                    }
                    else
                    {
                        double RemQty = OtherPOSQty - ShortageQty;
                        for (int i = dtStockOtherPOS.Rows.Count - 1; i > -1; i--)
                        {
                            if (dtStockOtherPOS.Rows[i][0].ToString() == SelectedCode && dtStockOtherPOS.Rows[i][1].ToString() == dgvOtherPOS.CurrentRow.Cells[0].Value.ToString())
                            {
                                dtStockOtherPOS.Rows[i][2] = RemQty;
                            }
                        }
                        dtBorrow.Rows.Add(SelectedCode, SelectedItems, SelectedPOSNo, dgvOtherPOS.CurrentRow.Cells[0].Value.ToString(), ShortageQty);
                        CurrentTranQty = CurrentTranQty + ShortageQty;
                        dgvConsumpDetails.CurrentRow.Cells[3].Value = CurrentTranQty;
                        dgvOtherPOS.CurrentRow.Cells[1].Value = RemQty;

                    }
                }

                CheckBtnSave();
            }
        }

        private void SDBorrowForm_Load(object sender, EventArgs e)
        {
            CleardtBorrow();
        }

        private void DgvConsumpDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvOtherPOS.Rows.Clear();
            SelectedCode = dgvConsumpDetails.CurrentRow.Cells[0].Value.ToString();
            SelectedItems = dgvConsumpDetails.CurrentRow.Cells[1].Value.ToString();
            groupBox3.Text = SelectedItems + " : ដែលមាននៅ POS ផ្សេង";
            groupBox3.Refresh();
            foreach (DataRow row in dtStockOtherPOS.Rows)
            {
                if (SelectedCode == row[0].ToString() && Convert.ToDouble(row[2].ToString()) > 0)
                {
                    dgvOtherPOS.Rows.Add(row[1], Convert.ToDouble(row[2].ToString()));
                }
            }
            dgvOtherPOS.ClearSelection();
            CheckBtnSave();
        }

        private void DgvConsumpDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.Value.ToString() != "")
            {
                if (Convert.ToDouble(dgvConsumpDetails[2, e.RowIndex].Value.ToString()) == Convert.ToDouble(dgvConsumpDetails[3, e.RowIndex].Value.ToString()))
                {
                    e.CellStyle.ForeColor = Color.Blue;
                    e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambong", 9, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambong", 9, FontStyle.Bold);
                }
            }
        }

        private void DgvPOS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvConsumpDetails.Rows.Clear();
            dgvOtherPOS.Rows.Clear();
            groupBox3.Text = "ដែលមាននៅ POS ផ្សេង";
            groupBox3.Refresh();
            SelectedSysNo = dgvPOS.CurrentRow.Cells[0].Value.ToString();
            SelectedPOSNo = dgvPOS.CurrentRow.Cells[1].Value.ToString();
            CalcSelectedPOS();
            CheckBtnSave();
        }

        private void CalcSelectedPOS()
        {
            try
            {
                cnn.con.Open();
                //Select Consumption and Calculate Remain Stock
                SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, ConsumpQty FROM tbSDConn2Consump " +
                                                                                "WHERE SysNo = '" + SelectedSysNo + "' " +
                                                                                "ORDER BY SeqNo ASC", cnn.con);
                dtStock = new DataTable();
                sda.Fill(dtStock);

                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT Code, SUM(StockValue) AS StockQty FROM tbSDMCAllTransaction " +
                                                                                "WHERE CancelStatus=0 AND POSNo = '" + SelectedPOSNo + "' " +
                                                                                "GROUP BY POSNo, Code, RMDes", cnn.con);
                DataTable dt = new DataTable();
                sda1.Fill(dt);
                dtStock.Columns.Add("StockQty");
                foreach (DataRow dr in dtStock.Rows)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (dr[0].ToString() == row[0].ToString())
                        {
                            dr[3] = Convert.ToDouble(row[1].ToString());
                        }
                    }
                }

                //Add to Dgv
                foreach (DataRow row in dtStock.Rows)
                {
                    dgvConsumpDetails.Rows.Add(row[0], row[1], Convert.ToDouble(row[2].ToString()), Convert.ToDouble(row[3].ToString()));
                }

                dgvConsumpDetails.ClearSelection();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
            CalcRemainRMOtherPOS();
        }

        private void CalcRemainRMOtherPOS()
        {
            CleardtBorrow();
            string AllCode = "";
            foreach (DataGridViewRow dgvRow in dgvConsumpDetails.Rows)
            {
                if (AllCode.Trim() == "")
                {
                    AllCode = "'" + dgvRow.Cells[0].Value.ToString() + "'";
                }
                else
                {
                    AllCode = AllCode + ", '" + dgvRow.Cells[0].Value.ToString() + "'";
                }
            }
            try
            {
                cnn.con.Open();
                //Select Consumption and Calculate Remain Stock
                SqlDataAdapter sda = new SqlDataAdapter("SELECT  Code, POSNo, SUM(StockValue) AS StockQty FROM tbSDMCAllTransaction " +
                                                                            "WHERE CancelStatus=0 AND Code IN (" + AllCode + ") AND NOT POSNo='" + SelectedPOSNo + "' " +
                                                                            "GROUP BY  Code, POSNo", cnn.con);
                dtStockOtherPOS = new DataTable();
                sda.Fill(dtStockOtherPOS);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
        }

        private void AssignDgvNo()
        {
            int rowNumber = 1;
            foreach (DataGridViewRow row in dgvPOS.Rows)
            {
                row.HeaderCell.Style.BackColor = Color.FromKnownColor(KnownColor.GradientActiveCaption);
                row.HeaderCell.Style.Font = new Font("Khmer OS Battambong", 10, FontStyle.Regular);
                row.HeaderCell.Value = rowNumber.ToString();
                rowNumber = rowNumber + 1;
            }
        }

        private void Search()
        {
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Visible = true;
            LbStatus.Refresh();
            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Column");
            dtSQLCond.Columns.Add("Value");
            dtSQLCond.Rows.Add("tbSDConn1Rec.Status= ", "1 ");
            if (txtWipName.Text.ToString().Trim() != "")
            {
                dtSQLCond.Rows.Add("WIPName LIKE ", "'%" + txtWipName.Text + "%' ");
            }
            if (txtRMName.Text.ToString().Trim() != "")
            {
                dtSQLCond.Rows.Add("tbSDConn2Consump.ItemName LIKE ", "'%" + txtRMName.Text + "%' ");
            }

            string SQLCond = "";
            foreach (DataRow dr in dtSQLCond.Rows)
            {
                if (SQLCond.Trim() == "")
                {
                    SQLCond = "WHERE " + dr[0].ToString() + dr[1].ToString();
                }
                else
                {
                    SQLCond = SQLCond + "AND " + dr[0].ToString() + dr[1].ToString();
                }
            }

            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT tbSDConn1Rec.SysNo, POSNo, WIPCode, WIPName, PosQty, PosShipD FROM tbSDConn1Rec " +
                                                                                    "INNER JOIN tbSDConn2Consump ON tbSDConn2Consump.SysNo = tbSDConn1Rec.SysNo​ " + SQLCond +
                                                                                    "GROUP BY tbSDConn1Rec.SysNo, POSNo, WIPCode, WIPName, PosQty, PosShipD " +
                                                                                    "ORDER BY POSNo ASC", cnn.con);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    dgvPOS.Rows.Add(dr[0], dr[1], dr[2], dr[3], Convert.ToDouble(dr[4].ToString()), Convert.ToDateTime(dr[5].ToString()));
                }
                dgvPOS.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

            LbStatus.Text = "រកឃើញទិន្នន័យ " + dgvPOS.Rows.Count.ToString("N0");
            LbStatus.Refresh();

        }

        private void CheckBtnSave()
        {
            if (dtBorrow.Rows.Count > 0)
            {
                btnSave.Enabled = true;
                btnSave.BackColor = Color.White;
            }
            else
            {
                btnSave.Enabled = false;
                btnSave.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
            }
        }

        private void CleardtBorrow()
        {
            dtBorrow = new DataTable();
            dtBorrow.Columns.Add("Code");
            dtBorrow.Columns.Add("ItemName");
            dtBorrow.Columns.Add("BTooPOSNo");
            dtBorrow.Columns.Add("BFromPOSNo");
            dtBorrow.Columns.Add("Qty");


        }

    }
}
