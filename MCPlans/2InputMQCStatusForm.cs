using System;
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
    public partial class _2InputMQCStatusForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        DataTable dtColor;

        string UpdateColName;
        string UpdateValue;
        string UpdatePosCNo;

        public _2InputMQCStatusForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Load += _2InputMQCStatusForm_Load;

            //Button Click
            this.btnSearch.Click += BtnSearch_Click;
            this.btnOKMQCETA.Click += BtnOKMQCETA_Click;

            //Dgv Cell Formatting
            this.dgvSearchResult.CellFormatting += DgvSearchResult_CellFormatting;
            this.dgvMQCStatus.CellFormatting += DgvMQCStatus_CellFormatting;

            //Dgv Cell Click
            this.dgvSearchResult.CellClick += DgvSearchResult_CellClick;
            this.dgvMQC.CellClick += DgvMQC_CellClick;
            this.dgvMQCStatus.CellClick += DgvMQCStatus_CellClick;

            //Dgv Cell Value Changed
            this.dgvSearchResult.CellValueChanged += DgvSearchResult_CellValueChanged;

            //Dgv Focus and Lost Focus
            this.dgvMQCStatus.GotFocus += DgvMQCStatus_GotFocus;
            this.dgvMQCStatus.LostFocus += DgvMQCStatus_LostFocus;
            this.dgvMQC.GotFocus += DgvMQC_GotFocus;
            this.dgvMQC.LostFocus += DgvMQC_LostFocus;


        }

        //Dgv Focus and Lost Focus
        private void DgvMQC_LostFocus(object sender, EventArgs e)
        {
            dgvMQC.SendToBack();
        }
        private void DgvMQC_GotFocus(object sender, EventArgs e)
        {
            dgvMQC.BringToFront();
        }
        private void DgvMQCStatus_LostFocus(object sender, EventArgs e)
        {
            dgvMQCStatus.SendToBack();
        }
        private void DgvMQCStatus_GotFocus(object sender, EventArgs e)
        {
            dgvMQCStatus.BringToFront();
        }

        //Dgv Cell Value Changed
        private void DgvSearchResult_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateColName = "";
            UpdateValue = "";
            UpdatePosCNo = "";
                        
            //MQC
            if (e.ColumnIndex == 9)
            {
                UpdateColName = "MQCName";
                UpdateValue = dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                UpdatePosCNo = dgvSearchResult.Rows[e.RowIndex].Cells[4].Value.ToString();
            }

            //Status
            if (e.ColumnIndex == 10)
            {
                UpdateColName = "MQCStatus";
                UpdateValue = "";
                foreach (DataGridViewRow DgvRow in dgvMQCStatus.Rows)
                {
                    if (DgvRow.Cells[1].Value.ToString() == dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                    {
                        UpdateValue = DgvRow.Cells[0].Value.ToString();
                    }
                }
                UpdatePosCNo = dgvSearchResult.Rows[e.RowIndex].Cells[4].Value.ToString();
            }

            //ETA
            if (e.ColumnIndex == 11)
            {
                UpdateColName = "MQCETA";
                UpdateValue = dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                UpdatePosCNo = dgvSearchResult.Rows[e.RowIndex].Cells[4].Value.ToString();
            }

            UpdateFuct();
        }


        //Dgv Cell Click
        private void DgvSearchResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            panelMQCETA.SendToBack();
            if (e.RowIndex > -1)
            {
                //MQC
                if (e.ColumnIndex == 9)
                {
                    dgvMQC.CurrentCell = dgvMQC.Rows[0].Cells[0];
                    dgvMQC.ClearSelection();
                    foreach (DataGridViewRow dgvRow in dgvMQC.Rows)
                    {
                        if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value.ToString() == dgvRow.Cells[0].Value.ToString())
                        {
                            dgvMQC.CurrentCell = dgvRow.Cells[0];
                            dgvRow.Cells[0].Selected = true;
                        }
                    }
                    Rectangle oRectangle = dgvSearchResult.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    int X = oRectangle.X + 7;
                    int Y = oRectangle.Y + 7;
                    dgvMQC.Location = new Point(X, Y);
                    dgvMQC.Focus();
                }

                //Status
                if (e.ColumnIndex == 10)
                {
                    dgvMQCStatus.ClearSelection();
                    foreach (DataGridViewRow dgvRow in dgvMQCStatus.Rows)
                    {
                        if (dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == dgvRow.Cells[1].Value.ToString())
                        {
                            dgvRow.Selected = true;
                        }
                    }
                    Rectangle oRectangle = dgvSearchResult.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    int X = oRectangle.X + 7;
                    int Y = oRectangle.Y + 73;
                    dgvMQCStatus.Location = new Point(X, Y);
                    dgvMQCStatus.Focus();
                }

                //ETA
                if (e.ColumnIndex == 11)
                {
                    dtpMCETADate.Value = DateTime.Now;
                    dtpMCETATime.Value = DateTime.Now;
                    Rectangle oRectangle = dgvSearchResult.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    int X = oRectangle.X - 88;
                    int Y = oRectangle.Y + 108;
                    panelMQCETA.Location = new Point(X, Y);
                    panelMQCETA.BringToFront();
                }
            }
        }
        private void DgvMQCStatus_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = dgvMQCStatus.CurrentCell.Value.ToString();
            dgvSearchResult.Focus();
        }
        private void DgvMQC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = dgvMQC.CurrentCell.Value.ToString();
            dgvSearchResult.Focus();
        }

        //Dgv Cell Formatting
        private void DgvMQCStatus_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.Value.ToString() == "STOP")
            {
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.BackColor = Color.Green;
            }
            if (e.ColumnIndex == 1 && e.Value.ToString() == "RUN")
            {
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.BackColor = Color.Orange;
            }
            if (e.ColumnIndex == 1 && e.Value.ToString() == "FINISH")
            {
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.BackColor = Color.Red;
            }
        }
        private void DgvSearchResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 6 && e.Value.ToString() != "")
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

            if (e.ColumnIndex > dgvSearchResult.Columns.Count - 4)
            {
                //Status
                if (e.ColumnIndex != 10 )
                {
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambong", 9, FontStyle.Regular);
                }
            }

            //Status
            if (e.ColumnIndex == 10)
            {
                if (e.Value.ToString() == "STOP")
                {
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.BackColor = Color.Green;
                }
                if (e.Value.ToString() == "RUN")
                {
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.BackColor = Color.Orange;
                }
                if (e.Value.ToString() == "FINISH")
                {
                    e.CellStyle.ForeColor = Color.White;
                    e.CellStyle.BackColor = Color.Red;
                }
            }

        }


        //Button Click
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            panelMQCETA.SendToBack();
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
                    string POSC = txtPosPNo.Text;
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
                dtSQLCond.Rows.Add("T2.ItemName LIKE ", "'%" + txtWIPName.Text + "%'");
            }            
            if (ChkDate.Checked == true)
            {
                dtSQLCond.Rows.Add("T1.PosPDelDate BETWEEN ", "'" + dtpFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dtpToo.Value.ToString("yyyy-MM-dd") + " 23:59:59' ");
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
                SqlDataAdapter sda = new SqlDataAdapter("SELECT PosPNo, PosPDelDate, T2.ItemCode, ItemName, T1.PosCNo, Remarks1, Remarks2, Remarks3, PosCQty, "+
                                                                                    "CONCAT(T1.MQCName, '') AS MQC, "+
                                                                                    "  Case "+
                                                                                    "    WHEN MQCStatus IS NULL THEN '' " +
                                                                                    "    WHEN MQCStatus = 0 THEN 'STOP' "+
                                                                                    "    WHEN MQCStatus = 1 THEN 'RUN' "+
                                                                                    "    ELSE 'FINISH' "+
                                                                                    "  END, CONCAT(MQCETA, '') FROM "+
                                                                                    "(SELECT PosPNo, PosPDelDate, WIPCode, PosCNo, PosCQty, MQCName, MQCStatus, MQCETA "+
                                                                                    "FROM tbPOSDetailofMC WHERE NOT PosCStatus = 2) T1 "+
                                                                                    "LEFT JOIN "+
                                                                                    "(SELECT ItemCode, ItemName, Remarks1, Remarks2, Remarks3 FROM tbMasterItem WHERE LEN(ItemCode) > 4) T2 "+
                                                                                    "ON T1.WIPCode = T2.ItemCode "+ SQLConds+
                                                                                    "ORDER BY T1.PosCNo ASC ", cnn.con);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    string PosPNo;
                    DateTime PosPDelDate;
                    string ItemCode;
                    string ItemName;
                    string PosCNo;
                    string Remarks1;
                    string Remarks2;
                    string Remarks3;
                    double PosCQty;
                    string MQC;
                    string MQCStatus;
                    string MQCETA = "";

                    PosPNo = row[0].ToString();
                    PosPDelDate = Convert.ToDateTime(row[1].ToString());
                    ItemCode = row[2].ToString();
                    ItemName = row[3].ToString();
                    PosCNo = row[4].ToString();
                    Remarks1 = row[5].ToString();
                    Remarks2 = row[6].ToString();
                    Remarks3 = row[7].ToString();
                    PosCQty = Convert.ToDouble(row[8].ToString());
                    MQC = row[9].ToString();
                    MQCStatus = row[10].ToString();
                    if (row[11].ToString().Trim() != "")
                    {
                        MQCETA = Convert.ToDateTime(row[11].ToString()).ToString("dd-MM-yy hh:mm tt");
                    }

                    dgvSearchResult.Rows.Add(PosPNo, PosPDelDate, ItemCode, ItemName, PosCNo, Remarks1, Remarks2, Remarks3, PosCQty, MQC, MQCStatus, MQCETA);

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
        }
        private void BtnOKMQCETA_Click(object sender, EventArgs e)
        {
            string Date = "";
            Date = dtpMCETADate.Value.ToString("yyyy-MM-dd");
            Date = Date + " " + dtpMCETATime.Value.ToString("HH:mm") + ":00";
            dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = Convert.ToDateTime(Date);
            panelMQCETA.SendToBack();
        }


        private void UpdateFuct()
        {
            string query = "";

            if (UpdateColName.Contains("Name") == true)
            {
                query = "UPDATE tbPOSDetailofMC SET " +
                                            UpdateColName + "='" + UpdateValue + "'," +
                                            "UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            "UpdateBy=N'" + MenuFormV2.UserForNextForm + "' " +
                                            "WHERE PosCNo = '" + UpdatePosCNo + "' ;";
            }
            else if (UpdateColName.Contains("Status") == true)
            {
                query = "UPDATE tbPOSDetailofMC SET " +
                                            UpdateColName + "=" + Convert.ToDouble(UpdateValue) + "," +
                                            "UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            "UpdateBy=N'" + MenuFormV2.UserForNextForm + "' " +
                                            "WHERE PosCNo = '" + UpdatePosCNo + "' ;";

            }
            else if (UpdateColName.Contains("ETA") == true)
            {
                query = "UPDATE tbPOSDetailofMC SET " +
                                            UpdateColName + "='" + Convert.ToDateTime(UpdateValue).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            "UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            "UpdateBy=N'" + MenuFormV2.UserForNextForm + "' " +
                                            "WHERE PosCNo = '" + UpdatePosCNo + "' ;";
            }
            else
            {

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


        private void _2InputMQCStatusForm_Load(object sender, EventArgs e)
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

            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT MQC FROM tbMasterMQC ORDER BY MQC ASC", cnn.con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgvMQC.Rows.Add("");
                foreach (DataRow row in dt.Rows)
                {
                    dgvMQC.Rows.Add(row[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

            dgvMQCStatus.Rows.Add("0", "STOP");
            dgvMQCStatus.Rows.Add("1", "RUN");
            dgvMQCStatus.Rows.Add("2", "FINISH");



        }

    }
}
