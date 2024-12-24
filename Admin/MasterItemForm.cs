using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.Admin
{
    public partial class MasterItemForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SqlCommand cmd;

        public MasterItemForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.btnSearch.Click += BtnSearch_Click;
            this.Load += MasterItemForm_Load;
            this.dgvSearchResult.CellClick += DgvSearchResult_CellClick;
            this.dgvSearchResult.CellFormatting += DgvSearchResult_CellFormatting;
            this.dgvSearchResult.CellValueChanged += DgvSearchResult_CellValueChanged;
            this.dgvMCType.LostFocus += DgvMCType_LostFocus;
            this.dgvMCType.CellClick += DgvMCType_CellClick;
            this.DgvSlot.LostFocus += DgvSlot_LostFocus;
            this.DgvSlot.CellClick += DgvSlot_CellClick;

        }

        private void DgvSlot_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvSlot.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() != "")
            {
                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = DgvSlot.Rows[e.RowIndex].Cells[0].Value.ToString();

            }
            else
            {
                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = null;
            }

            dgvSearchResult.Focus();
        }

        private void DgvSlot_LostFocus(object sender, EventArgs e)
        {
            this.DgvSlot.SendToBack();
        }

        private void DgvSearchResult_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string ItemCode = "";
            ItemCode = dgvSearchResult.Rows[e.RowIndex].Cells[0].Value.ToString();
            DataTable dt = new DataTable();

            //Find Already have or not yet
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode FROM tbMasterItemPlan WHERE ItemCode = '" + ItemCode + "' ", cnn.con);
                sda.Fill(dt);                
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

            try
            {
                cnn.con.Open();
                if (e.ColumnIndex > dgvSearchResult.Columns.Count - 5 && e.ColumnIndex < dgvSearchResult.Columns.Count - 1)
                {
                    //Update becuz it already have
                    if (dt.Rows.Count > 0)
                    {
                        string query = "";
                        if (dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                        {
                            query = "UPDATE tbMasterItemPlan SET " +
                                            "MC" + (-5 + e.ColumnIndex + 1) + "Type='" + dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() + "' " +
                                            "WHERE ItemCode = '" + ItemCode + "' ;";
                        }
                        else
                        {
                            query = "UPDATE tbMasterItemPlan SET " +
                                            "MC" + (-5 + e.ColumnIndex + 1) + "Type=NULL " +
                                            "WHERE ItemCode = '" + ItemCode + "' ;";
                        }
                        SqlCommand cmd = new SqlCommand(query, cnn.con);
                        cmd.ExecuteNonQuery();
                    }
                    //Add becuz it not yet have
                    else
                    {
                        if (dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                        {
                            cmd = new SqlCommand("INSERT INTO tbMasterItemPlan (ItemCode, MC" + (-5 + e.ColumnIndex + 1) + "Type) " +
                                                                            "VALUES (@Ic, @McT)", cnn.con);
                            cmd.Parameters.AddWithValue("@Ic", ItemCode);
                            cmd.Parameters.AddWithValue("@McT", dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    //Update becuz it already have
                    if (dt.Rows.Count > 0)
                    {
                        string query = "";
                        if (dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                        {
                            query = "UPDATE tbMasterItemPlan SET " +
                                            "Slot='" + dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() + "' " +
                                            "WHERE ItemCode = '" + ItemCode + "' ;";
                        }
                        else
                        {
                            query = "UPDATE tbMasterItemPlan SET " +
                                            "Slot=NULL " +
                                            "WHERE ItemCode = '" + ItemCode + "' ;";
                        }

                        SqlCommand cmd = new SqlCommand(query, cnn.con);
                        cmd.ExecuteNonQuery();
                    }
                    //Add becuz it not yet have
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO tbMasterItemPlan (ItemCode, Slot) " +
                                                                            "VALUES (@Ic, @Sl)", cnn.con);
                        cmd.Parameters.AddWithValue("@Ic", ItemCode);
                        cmd.Parameters.AddWithValue("@Sl", dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("មានបញ្ហា!\n"+ex.Message,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            cnn.con.Close();
        }

        private void DgvSearchResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex > dgvSearchResult.Columns.Count - 5)
            {
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambong", 9, FontStyle.Regular);
            }
        }

        private void DgvMCType_CellClick(object sender, DataGridViewCellEventArgs e)
        {  
            if (dgvMCType.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() != "")
            {
                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = dgvMCType.Rows[e.RowIndex].Cells[0].Value.ToString();

            }
            else
            {
                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = null;
            }

            dgvSearchResult.Focus();
            
        }

        private void DgvMCType_LostFocus(object sender, EventArgs e)
        {
            dgvMCType.SendToBack();
        }

        private void DgvSearchResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex > dgvSearchResult.Columns.Count - 5 && e.ColumnIndex < dgvSearchResult.Columns.Count - 1)
                {
                    dgvMCType.CurrentCell = dgvMCType.Rows[0].Cells[0];
                    dgvMCType.ClearSelection();
                    if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        foreach (DataGridViewRow dgvRow in dgvMCType.Rows)
                        {
                            if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value.ToString() == dgvRow.Cells[0].Value.ToString())
                            {
                                dgvMCType.CurrentCell = dgvRow.Cells[0];
                                dgvRow.Cells[0].Selected = true;
                            }
                        }
                    }

                    Rectangle oRectangle = dgvSearchResult.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    int X = oRectangle.X + 7;
                    int Y = oRectangle.Y + 20;
                    dgvMCType.Location = new Point(X, Y);
                    dgvMCType.BringToFront();
                    dgvMCType.Focus();
                }
                if (e.ColumnIndex == dgvSearchResult.Columns.Count - 1)
                {
                    DgvSlot.CurrentCell = DgvSlot.Rows[0].Cells[0];
                    DgvSlot.ClearSelection();
                    if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        foreach (DataGridViewRow dgvRow in DgvSlot.Rows)
                        {
                            if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value.ToString() == dgvRow.Cells[0].Value.ToString())
                            {
                                DgvSlot.CurrentCell = dgvRow.Cells[0];
                                dgvRow.Cells[0].Selected = true;
                            }
                        }
                    }

                    Rectangle oRectangle = dgvSearchResult.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    int X = oRectangle.X + 1;
                    int Y = oRectangle.Y + 21;
                    DgvSlot.Location = new Point(X, Y);
                    DgvSlot.BringToFront();
                    DgvSlot.Focus();
                }
            }
            
        }

        private void MasterItemForm_Load(object sender, EventArgs e)
        {
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbMasterItemPlan2", cnn.con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgvMCType.Rows.Add("");
                foreach (DataRow row in dt.Rows)
                {
                    dgvMCType.Rows.Add(row[0]);
                }
                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT SLOTName FROM tbMasterSLOT ORDER BY SLOTName ASC", cnn.con);
                DataTable dtSlot = new DataTable();
                sda1.Fill(dtSlot);
                DgvSlot.Rows.Add("");
                foreach (DataRow row in dtSlot.Rows)
                {
                    DgvSlot.Rows.Add(row[0]);
                }



            }
            catch(Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
            
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Visible = true;
            LbStatus.Refresh();
            dgvSearchResult.Rows.Clear();
            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col'");
            dtSQLCond.Columns.Add("Val'");

            dtSQLCond.Rows.Add("LEN(ItemCode) > ", "4 ");
            if (txtWIPName.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("ItemName LIKE '%"+txtWIPName.Text+"%' ");
            }

            string SQLCond = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                if (SQLCond.Trim() == "")
                {
                    SQLCond = "WHERE " + row[0] + row[1];

                }
                else
                {
                    SQLCond = SQLCond + "AND " + row[0] + row[1];

                }
            }

            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT T1.ItemCode, ItemName,Remarks1, Remarks2, Remarks3, COALESCE(MC1Type,'') AS MC1Type, COALESCE(MC2Type,'') AS MC2Type, COALESCE(MC3Type,'') AS MC3Type, COALESCE(Slot,'') AS Slot  FROM " +
                                                                            "(SELECT ItemCode, ItemName, Remarks1, Remarks2, Remarks3 FROM tbMasterItem "+SQLCond+") T1 "+
                                                                            "LEFT JOIN "+
                                                                            "(SELECT ItemCode, MC1Type, MC2Type, MC3Type, Slot FROM tbMasterItemPlan) T2 " +
                                                                            "ON T1.ItemCode = T2.ItemCode ORDER BY T1.ItemCode ASC", cnn.con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    dgvSearchResult.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8]);
                }
                dgvSearchResult.ClearSelection();
                LbStatus.Text = "រកឃើញទិន្នន័យចំនួន " + dgvSearchResult.Rows.Count.ToString("N0") + " !";
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
        }

    }
}
