using MachineDeptApp.OtherClass;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MachineDeptApp.MCPlans
{
    public partial class RestRoomTimeForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        ExportAsCSVClass CSV = new ExportAsCSVClass();

        string ErrorText = "";

        public RestRoomTimeForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Shown += RestRoomTimeForm_Shown;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnExport.Click += BtnExport_Click;
            this.txtName.TextChanged += TxtName_TextChanged;
            this.txtNameKH.TextChanged += TxtNameKH_TextChanged;
            this.txtSubLName.TextChanged += TxtSubLName_TextChanged;
        }

        private void TxtSubLName_TextChanged(object sender, EventArgs e)
        {
            if (txtSubLName.Text.Trim() != "")
                chkSubLName.Checked = true;
            else
                chkSubLName.Checked = false;
        }
        private void TxtNameKH_TextChanged(object sender, EventArgs e)
        {
            if (txtNameKH.Text.Trim() != "")
                chkNameKH.Checked = true;
            else
                chkNameKH.Checked = false;
        }
        private void TxtName_TextChanged(object sender, EventArgs e)
        {
            if(txtName.Text.Trim()!="")
                chkNameEng.Checked = true;
            else
                chkNameEng.Checked = false;
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (DgvSearch.Rows.Count > 0)
            {
                CSV.CSVErrorT = "";
                CSV.ExportAsCSV(this.DgvSearch, "Rest Room - " + DateTime.Now.ToString("yyyyMMdd HH_mm"), 0);
                if (CSV.CSVErrorT.Trim() != "")
                {
                    if (CSV.CSVErrorT.Trim() == "OK")
                        MessageBox.Show("Export successfully.", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Something went wrong!\n" + CSV.CSVErrorT, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            DgvSearch.Rows.Clear();
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();

            string SQLConds = "";
            if (chkNameEng.Checked && txtName.Text.Trim() != "")
            {
                if (SQLConds.Trim() == "")
                    SQLConds = "AND T.Memb_EngName LIKE '%" + txtName.Text + "%' ";
                else
                    SQLConds += "AND T.Memb_EngName LIKE '%" + txtName.Text + "%' ";
            }
            if (chkNameKH.Checked && txtNameKH.Text.Trim() != "")
            {
                if (SQLConds.Trim() == "")
                    SQLConds = "AND T.Memb_KhName LIKE N'%" + txtNameKH.Text + "%' ";
                else
                    SQLConds += "AND T.Memb_KhName LIKE N'%" + txtNameKH.Text + "%' ";
            }
            if (chkSubLName.Checked && txtSubLName.Text.Trim() != "")
            {
                if (SQLConds.Trim() == "")
                    SQLConds = "AND S.NameKH LIKE N'%" + txtSubLName.Text + "%' ";
                else
                    SQLConds += "AND S.NameKH LIKE N'%" + txtSubLName.Text + "%' ";
            }

            //Taking Data
            DataTable dtData = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = @"SELECT T.Memb_IDNo, T.Memb_EngName, T.Memb_KhName, S.NameKH AS SubLName, 
                                            S.StaffNameKH AS StaffName, CAST(T.TimesOut AS date) AS Date , T.TimesOut, T.TimesIn, ROUND(DurationS/60.0,2) AS TTLMinutes, RegByDevice FROM tbToiletRequest T
                                            INNER JOIN MstSubLMember M ON T.Memb_IDNo = M.Memb_IDNo 
                                            INNER JOIN MstSubL S ON M.SubL_IDNo = S.SubL_IDNo 
                                            WHERE CAST(T.TimesOut AS date) BETWEEN '"+dtpSDate.Value.ToString("yyyy-MM-dd")+"' AND '"+ dtpEDate.Value.ToString("yyyy-MM-dd") + "' "+ SQLConds + @" 
                                            ORDER BY S.StaffNameKH, S.SubL_IDNo, T.Memb_IDNo, T.TimesOut";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtData);
            }
            catch (Exception ex)
            {
                ErrorText = "Taking Data : \n" + ex.Message;
            }
            cnn.con.Close();

            //Add to DGV
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtData.Rows)
                {
                    DgvSearch.Rows.Add();
                    DataGridViewRow drow = DgvSearch.Rows[DgvSearch.Rows.Count - 1];
                    drow.HeaderCell.Value = DgvSearch.Rows.Count.ToString();
                    drow.Cells["IDCard"].Value = row["Memb_IDNo"].ToString();
                    drow.Cells["NameEng"].Value = row["Memb_EngName"].ToString();
                    drow.Cells["NameKH"].Value = row["Memb_KhName"].ToString();
                    drow.Cells["SubLName"].Value = row["SubLName"].ToString();
                    drow.Cells["StaffName"].Value = row["StaffName"].ToString();
                    drow.Cells["Date"].Value = Convert.ToDateTime(row["Date"]).Date;
                    drow.Cells["TimeOut"].Value = Convert.ToDateTime(row["TimesOut"]);
                    if(DateTime.TryParse(row["TimesIn"].ToString(), out DateTime InTime))
                        drow.Cells["TimeIN"].Value = InTime;
                    if(double.TryParse(row["TTLMinutes"].ToString(), out double TTLMinutes))
                        drow.Cells["TTLMinutes"].Value = TTLMinutes;
                    drow.Cells["RegByDevice"].Value = row["RegByDevice"].ToString();
                }
                DgvSearch.ClearSelection();
                DgvSearch.CurrentCell = null;
            }

            if (DgvSearch.Rows.Count > 0)
            {
                btnExport.Enabled = true;
                btnExportGRAY.SendToBack();
            }
            else
            {
                btnExport.Enabled = false;
                btnExportGRAY.BringToFront();
            }
          
            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                LbStatus.Text = "រកឃើញទិន្នន័យ  "+DgvSearch.Rows.Count.ToString("N0");
                LbStatus.Refresh();
            }
            else
            {
                LbStatus.Text = "មានបញ្ហា!";
                LbStatus.Refresh();
                MessageBox.Show("មានបញ្ហា! " + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }

        }

        private void RestRoomTimeForm_Shown(object sender, EventArgs e)
        {
            DgvSearch.RowHeadersDefaultCellStyle.Font = new Font(DgvSearch.AlternatingRowsDefaultCellStyle.Font, FontStyle.Regular);
        }

    }
}
