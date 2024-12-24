using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.MCPlans
{
    public partial class _3EditPOSMCDetailForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        DataTable dtMCName;
        DataTable dtColor;

        string UpdateColName;
        string UpdateValue;
        string UpdatePosCNo;

        public _3EditPOSMCDetailForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Load += _3EditPOSMCDetailForm_Load;
            this.btnSearch.Click += BtnSearch_Click;
            this.dgvSearchResult.CellFormatting += DgvSearchResult_CellFormatting;
            this.dgvSearchResult.CellValueChanged += DgvSearchResult_CellValueChanged;
            this.dgvSearchResult.CellClick += DgvSearchResult_CellClick;
            this.btnDelete.Click += BtnDelete_Click;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[9].Value.ToString() == "Not Yet")
            {
                DialogResult DLS = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះចេញពីកម្មវិធីនេះមែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    try
                    {
                        cnn.con.Open();

                        //Re-Checking the status
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT PosCStatus FROM tbPOSDetailofMC WHERE PosCNo='"+ dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[3].Value.ToString() + "' ", cnn.con);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() == "0")
                            {
                                //Delete
                                string query = "";
                                query = "DELETE FROM tbPOSDetailofMC WHERE PosCNo = '" + dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[3].Value.ToString() + "' ;";
                                SqlCommand cmd = new SqlCommand(query, cnn.con);
                                cmd.ExecuteNonQuery();
                                int selectedIndex = dgvSearchResult.CurrentCell.RowIndex;
                                if (selectedIndex > -1)
                                {
                                    dgvSearchResult.Rows.RemoveAt(selectedIndex);
                                    dgvSearchResult.Refresh();
                                }
                                dgvSearchResult.ClearSelection();
                                CheckBtnDelete();
                                btnSearch.Focus();
                                Cursor = Cursors.Default;
                            }
                            else
                            {

                                Cursor = Cursors.Default;
                                MessageBox.Show("ភីអូអេសនេះមិនអាចលុបចេញបានទេ! ព្រោះមានទិន្នន័យផ្សេងទៀតដែលទាក់ទង !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cnn.con.Close();
                }
            }
            else
            {
                MessageBox.Show("ភីអូអេសនេះមិនអាចលុបចេញបានទេ! ព្រោះស្ថានភាពគឺ <"+ dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[9].Value.ToString() + "> !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }

        private void DgvSearchResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CheckBtnDelete();
        }

        private void DgvSearchResult_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //Remark
            if (e.ColumnIndex == 10)
            {
                string query = "";
                if (dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    query = "UPDATE tbPOSDetailofMC SET " +
                                            "Remarks = NULL " +
                                            "WHERE PosCNo = '" + dgvSearchResult.Rows[e.RowIndex].Cells[3].Value.ToString() + "' ;";
                    
                }
                else
                {
                    if (dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim() != "")
                    {
                        query = "UPDATE tbPOSDetailofMC SET " +
                                           "Remarks = N'" + dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() + "' " +
                                           "WHERE PosCNo = '" + dgvSearchResult.Rows[e.RowIndex].Cells[3].Value.ToString() + "' ;";
                    }
                    else
                    {
                        query = "UPDATE tbPOSDetailofMC SET " +
                                            "Remarks = NULL " +
                                            "WHERE PosCNo = '" + dgvSearchResult.Rows[e.RowIndex].Cells[3].Value.ToString() + "' ;";

                    }
                }
                
                if (query.Trim() != "")
                {
                    try
                    {
                        cnn.con.Open();
                        SqlCommand cmd = new SqlCommand(query, cnn.con);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cnn.con.Close();
                }
            }
        }

        private void DgvSearchResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 5 && e.Value.ToString() != "")
            {
                foreach (DataRow row in dtColor.Rows)
                {
                    if (e.Value.ToString().Contains(row[0].ToString()))
                    {
                        e.CellStyle.ForeColor = Color.FromName(row[1].ToString());
                        e.CellStyle.BackColor = Color.FromName(row[2].ToString());
                    }
                }
            }

            if (e.ColumnIndex > dgvSearchResult.Columns.Count - 11 && e.ColumnIndex < dgvSearchResult.Columns.Count - 5)
            {
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambong", 9, FontStyle.Regular);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            panelMCETA.SendToBack();
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            LbStatus.Visible = true;
            dgvSearchResult.Rows.Clear();
            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Value");

            if (txtPosPNo.Text.Trim() != "")
            {
                if (txtPosPNo.Text.Contains("*") == true)
                {
                    string POSP = txtPosPNo.Text;
                    POSP = POSP.Replace("*", "%");
                    dtSQLCond.Rows.Add("T1.PosPNo LIKE ", "'" + POSP + "' ");
                }
                else
                {
                    dtSQLCond.Rows.Add("T1.PosPNo = ", "'" + txtPosPNo.Text + "' ");
                }                
            }
            if (txtPosCNo.Text.Trim() != "")
            {
                if (txtPosCNo.Text.Contains("*") == true)
                {
                    string POSC = txtPosCNo.Text;
                    POSC = POSC.Replace("*", "%");
                    dtSQLCond.Rows.Add("T1.PosCNo LIKE ", "'" + POSC + "' ");
                }
                else
                {
                    dtSQLCond.Rows.Add("T1.PosCNo = ", "'" + txtPosCNo.Text + "' ");
                }
            }
            if (txtWIPName.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("T3.ItemName LIKE ", "'%" + txtWIPName.Text + "%'");
            }            
            if (ChkDate.Checked == true)
            {
                dtSQLCond.Rows.Add("T1.PosPDelDate BETWEEN ", "'" + dtpFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dtpToo.Value.ToString("yyyy-MM-dd") + " 23:59:59' ");
            }
            if (CboMC1.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("T2.MCType = ", "'" + CboMC1.Text + "' ");
            }
            if (CboPrintStatus.Text.Trim() != "All")
            {
                if (CboPrintStatus.Text == "NOT YET")
                {
                    dtSQLCond.Rows.Add("NOT T1.PrintStatus = ", "1 ");
                }
                else
                {
                    dtSQLCond.Rows.Add("T1.PrintStatus = ", "1 ");
                }
            }

            string SQLConds = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                if (SQLConds.Trim() == "")
                {
                    SQLConds = "WHERE " + row[0] + row[1];
                }
                else
                {
                    SQLConds = SQLConds + "AND " + row[0] + row[1];
                }
            }

            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT PosPNo, WIPCode, ItemName, PosCNo, Remarks1, Remarks2, Remarks3, PosCQty, PosCRemainQty, "+
                                                                                    "  CASE " +
                                                                                    "    WHEN PosCStatus = 0 THEN 'Not Yet' " +
                                                                                    "    WHEN PosCStatus = 1 THEN 'Producing' " +
                                                                                    "    ELSE 'Completed' " +
                                                                                    "  END AS PosCStatus, Remarks,  " +
                                                                                    "  CASE "+
                                                                                    "    WHEN MC1Name IS NULL THEN T4.MC1Type "+
                                                                                    "    WHEN MC1Name = '' THEN T4.MC1Type "+
                                                                                    "    ELSE T2.MCType "+
                                                                                    "  END AS MC1, CONCAT(MC1Name, '') AS MC1Name, " +
                                                                                    "  CASE "+
                                                                                    "    WHEN MC1Status IS NULL THEN '' "+
                                                                                    "    WHEN MC1Status = 0 THEN 'STOP' "+
                                                                                    "    WHEN MC1Status = 1 THEN 'RUN' "+
                                                                                    "    ELSE 'FINISH' "+
                                                                                    "  END AS MC1Status, CONCAT(MC1ETA, '') AS MC1ETA, PrintStatus, T1.RegDate, T1.RegBy, T1.UpdateDate, T1.UpdateBy FROM " +
                                                                                    "(SELECT * FROM tbPOSDetailofMC) T1 "+
                                                                                    "LEFT JOIN "+
                                                                                    "(SELECT * FROM tbMasterMCType) T2 "+
                                                                                    "ON T1.MC1Name = T2.MCName "+
                                                                                    "LEFT JOIN "+
                                                                                    "(SELECT ItemCode, ItemName, Remarks1, Remarks2, Remarks3 FROM tbMasterItem WHERE LEN(ItemCode) > 4) T3 "+
                                                                                    "ON T1.WIPCode = T3.ItemCode "+
                                                                                    "LEFT JOIN "+
                                                                                    "(SELECT * FROM tbMasterItemPlan) T4 "+
                                                                                    "ON T1.WIPCode = T4.ItemCode "+SQLConds+
                                                                                    "ORDER BY T1.PosCNo ASC", cnn.con);

                DataTable dt = new DataTable();
                sda.Fill(dt);

                //Remove Duplicate
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    int Count = 0;
                    string POSNo = dt.Rows[i][3].ToString();

                    DataRow dr = dt.Rows[i];

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j][3].ToString() == dr[3].ToString())
                        {
                            Count = Count + 1;

                        }
                    }

                    if (Count > 1)
                    {
                        dr.Delete();
                        dt.AcceptChanges();
                    }
                }


                foreach (DataRow row in dt.Rows)
                {
                    string PosPNo;
                    string ItemCode;
                    string ItemName;
                    string PosCNo;
                    string Remarks1;
                    string Remarks2;
                    string Remarks3;
                    double PosCQty;
                    double PosCRemainQty;
                    string PosCStatus;
                    string Remarks;
                    string MC1;
                    string MC1Name;
                    string MC1Status;
                    string MC1ETA = "";
                    Boolean PrintStatus=true;
                    DateTime RegDate;
                    string RegBy;
                    DateTime UpdateDate;
                    string UpdateBy;


                    PosPNo = row[0].ToString();
                    ItemCode = row[1].ToString();
                    ItemName = row[2].ToString();
                    PosCNo = row[3].ToString();
                    Remarks1 = row[4].ToString();
                    Remarks2 = row[5].ToString();
                    Remarks3 = row[6].ToString();
                    PosCQty = Convert.ToDouble(row[7].ToString());
                    PosCRemainQty = Convert.ToDouble(row[8].ToString());
                    PosCStatus = row[9].ToString();
                    Remarks = row[10].ToString();
                    MC1 = row[11].ToString();
                    MC1Name = row[12].ToString();
                    MC1Status = row[13].ToString();
                    if (row[14].ToString().Trim() != "")
                    {
                        MC1ETA = Convert.ToDateTime(row[14].ToString()).ToString("dd-MM-yy hh:mm tt");
                    }
                    if (row[15].ToString().Trim() == "0")
                    {
                        PrintStatus = false;
                    }
                    RegDate = Convert.ToDateTime(row[16].ToString());
                    RegBy = row[17].ToString();
                    UpdateDate = Convert.ToDateTime(row[18].ToString());
                    UpdateBy = row[19].ToString();

                    dgvSearchResult.Rows.Add(PosPNo, ItemCode, ItemName, PosCNo, Remarks1, Remarks2, Remarks3, PosCQty, PosCRemainQty, PosCStatus, Remarks, MC1, MC1Name, MC1Status, MC1ETA, PrintStatus, RegDate, RegBy, UpdateDate, UpdateBy);

                }

                dgvSearchResult.ClearSelection();
                Cursor = Cursors.Default;
                LbStatus.Text = "ស្វែងរកឃើញទិន្នន័យ " + dgvSearchResult.Rows.Count.ToString("N0");
                LbStatus.Refresh();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("មានបញ្ហា!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            cnn.con.Close();

            CheckBtnDelete();
        }

        private void _3EditPOSMCDetailForm_Load(object sender, EventArgs e)
        {
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


            //Set Frozen to Column
            for (int i = 0; i < dgvSearchResult.Columns.Count-13; i++)
            {
                dgvSearchResult.Columns[i].Frozen=true;
            }

            for(int i =0; i<dgvSearchResult.Columns.Count; i++)
            {
                if (i != dgvSearchResult.Columns.Count - 10)
                {
                    dgvSearchResult.Columns[i].ReadOnly=true;
                }
            }

            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbMasterItemPlan2", cnn.con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgvMCType.Rows.Add("");
                CboMC1.Items.Add("");
                foreach (DataRow row in dt.Rows)
                {
                    dgvMCType.Rows.Add(row[0]);
                    CboMC1.Items.Add(row[0]);
                }
                CboMC1.SelectedIndex = 0;
                
                SqlDataAdapter sda2 = new SqlDataAdapter("SELECT MCType, MCName FROM tbMasterMCType ORDER BY MCType ASC, MCName ASC", cnn.con);
                dtMCName = new DataTable();
                sda2.Fill(dtMCName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

            dgvMCStatus.Rows.Add("0", "STOP");
            dgvMCStatus.Rows.Add("1", "RUN");
            dgvMCStatus.Rows.Add("2", "FINISH");

            CboPrintStatus.Items.Add("NOT YET");
            CboPrintStatus.Items.Add("OK");
            CboPrintStatus.Items.Add("All");
            CboPrintStatus.SelectedIndex = 0;

        }

        private void CheckBtnDelete()
        {
            if (dgvSearchResult.SelectedCells.Count > 0)
            {
                btnDelete.Enabled = true;
                btnDelete.BackColor = Color.White;

            }
            else
            {
                btnDelete.Enabled = false;
                btnDelete.BackColor = Color.FromKnownColor(KnownColor.LightGray);
            }
        }

    }
}
