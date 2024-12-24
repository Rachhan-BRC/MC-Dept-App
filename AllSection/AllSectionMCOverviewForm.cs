using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Lifetime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.AllSection
{
    public partial class AllSectionMCOverviewForm : Form
    {
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SQLConnectAllSection cnnAll = new SQLConnectAllSection();
        DataTable dtColor;

        public static string PosNo;
        public static string Code;
        public static string FGName;

        public AllSectionMCOverviewForm()
        {
            InitializeComponent();
            this.cnnOBS.Connection();
            this.cnnAll.Connection();
            this.btnSearch.Click += BtnSearch_Click;
            this.Load += AllSectionMCOverviewForm_Load;
            this.dgvSearchResult.CellFormatting += DgvSearchResult_CellFormatting;
            this.dgvSearchResult.CellDoubleClick += DgvSearchResult_CellDoubleClick;

        }

        private void DgvSearchResult_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            PosNo = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[1].Value.ToString();
            Code = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[2].Value.ToString();
            FGName = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[3].Value.ToString();
            AllSectionMCOverviewFormStock Asmofs = new AllSectionMCOverviewFormStock();
            Asmofs.ShowDialog();
        }
        private void DgvSearchResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //Wire Color
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
        }
        private void AllSectionMCOverviewForm_Load(object sender, EventArgs e)
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

            for (int i = 6; i < dgvSearchResult.Columns.Count; i++)
            {
                if (i > 6 && i < 8 || i > 11)
                {
                    dgvSearchResult.Columns[i].HeaderCell.Style.BackColor = Color.SlateBlue;
                    dgvSearchResult.Columns[i].HeaderCell.Style.SelectionBackColor = Color.SlateBlue;
                    dgvSearchResult.Columns[i].HeaderCell.Style.SelectionForeColor = Color.White;
                    dgvSearchResult.Columns[i].HeaderCell.Style.ForeColor = Color.White;
                }
                if (i > 7 && i < 10)
                {
                    dgvSearchResult.Columns[i].HeaderCell.Style.BackColor = Color.Yellow;
                    dgvSearchResult.Columns[i].HeaderCell.Style.SelectionBackColor = Color.Yellow;
                    dgvSearchResult.Columns[i].HeaderCell.Style.SelectionForeColor = Color.Black;
                    dgvSearchResult.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
                }
                if (i > 9 && i < 12)
                {
                    dgvSearchResult.Columns[i].HeaderCell.Style.BackColor = Color.DodgerBlue;
                    dgvSearchResult.Columns[i].HeaderCell.Style.SelectionBackColor = Color.DodgerBlue;
                    dgvSearchResult.Columns[i].HeaderCell.Style.SelectionForeColor = Color.White;
                    dgvSearchResult.Columns[i].HeaderCell.Style.ForeColor = Color.White;
                }
            }

        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            LbStatus.Visible = true;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            dgvSearchResult.Rows.Clear();
            DataTable dtSQLAllCond = new DataTable();
            dtSQLAllCond.Columns.Add("Col");
            dtSQLAllCond.Columns.Add("Val");
            DataTable dtSQLOBSCond = new DataTable();
            dtSQLOBSCond.Columns.Add("Col");
            dtSQLOBSCond.Columns.Add("Val");

            //Add SQL Cond for All DB
            dtSQLAllCond.Rows.Add("tAll.ShipDate BETWEEN '"+ dtpFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '"+dtpToo.Value.ToString("yyyy-MM-dd")+" 23:59:59' ");
            if (txtPOSNo.Text.Trim() != "")
            {
                string POSNo = txtPOSNo.Text.ToString();
                if (POSNo.Contains("*") == true)
                {
                    POSNo = POSNo.Replace("*", "%");
                    dtSQLAllCond.Rows.Add("tAll.POS_No LIKE ","'"+POSNo+"' ");
                }
                else
                {
                    dtSQLAllCond.Rows.Add("tAll.POS_No = ", "'" + POSNo + "' ");
                }
            }
            if (txtWIPCode.Text.Trim() != "")
            {
                string WIPCode = txtWIPCode.Text;
                if (txtWIPCode.Text.ToString().Contains("*") == true)
                {
                    WIPCode = WIPCode.Replace("*", "%");
                    dtSQLAllCond.Rows.Add("tAll.ItemCode LIKE ", "'" + WIPCode + "' ");
                    dtSQLOBSCond.Rows.Add("ItemCode LIKE ","'"+WIPCode+"' ");
                }
                else
                {
                    dtSQLAllCond.Rows.Add("tAll.ItemCode = ", "'" + WIPCode + "' ");
                    dtSQLOBSCond.Rows.Add("ItemCode = ", "'" + WIPCode + "' ");
                }
            }

            //Add SQL Cond for OBS DB
            dtSQLOBSCond.Rows.Add("ItemType = ", "1 ");
            dtSQLOBSCond.Rows.Add("DelFlag = ", "0 ");
            if (txtWIPName.Text.Trim() != "")
            {
                dtSQLOBSCond.Rows.Add("ItemName LIKE ", "'%" + txtWIPName.Text + "%' ");
            }


            string SQLAllConds = "";
            foreach (DataRow row in dtSQLAllCond.Rows)
            {
                if (SQLAllConds.Trim() == "")
                {
                    SQLAllConds = "WHERE " + row[0] + row[1];
                }
                else
                {
                    SQLAllConds = SQLAllConds + "AND " + row[0] + row[1];
                }
            }

            string SQLOBSConds = "";
            foreach (DataRow row in dtSQLOBSCond.Rows)
            {
                if (SQLOBSConds.Trim() == "")
                {
                    SQLOBSConds = "WHERE " + row[0] + row[1];
                }
                else
                {
                    SQLOBSConds = SQLOBSConds + "AND " + row[0] + row[1];
                }
            }

            DataTable dtAllData = new DataTable();
            DataTable dtOBSData = new DataTable();

            //Take Mst from OBS
            try
            {
                cnnOBS.conOBS.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, Remark2, Remark3, Remark4 FROM mstitem "+SQLOBSConds+" ORDER BY ItemCode ASC", cnnOBS.conOBS);
                sda.Fill(dtOBSData);
            }
            catch(Exception ex)
            {
                MessageBox.Show("មានបញ្ហា OBS !\n"+ex.Message,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            cnnOBS.conOBS.Close();

            //Take All data
            try
            {
                cnnAll.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT tAll.ShipDate, tAll.POS_No, tAll.ItemCode, ItemName, PIN, Wire, Length, MC1TransQty, WIP1RecQty, WIP1TransQty, Assy1RecQty, Assy1TransQty, MC1RecQty FROM " +
                    "(SELECT ShipDate, POS_No, ItemCode,'' AS ItemName, '' AS Wire, '' AS PIN, '' AS Length FROM tbAllTransaction WHERE CancelStatus=0 GROUP BY ShipDate,POS_No, ItemCode) tAll " +
                    "LEFT JOIN " +
                    "(SELECT POS_No, ItemCode, SUM(RecQty) AS MC1RecQty, SUM(TransQty) AS MC1TransQty FROM tbAllTransaction WHERE LocCode='MC1' AND CancelStatus=0 GROUP BY POS_No, ItemCode) tMC1 " +
                    "ON tALL.POS_No=tMC1.POS_No AND tAll.ItemCode=tMC1.ItemCode " +
                    "LEFT JOIN " +
                    "(SELECT POS_No, ItemCode, SUM(RecQty) AS WIP1RecQty, SUM(TransQty) AS WIP1TransQty FROM tbAllTransaction WHERE LocCode='WIP1' AND CancelStatus=0 GROUP BY POS_No, ItemCode) tWIP1 " +
                    "ON tALL.POS_No=tWIP1.POS_No AND tAll.ItemCode=tWIP1.ItemCode " +
                    "LEFT JOIN " +
                    "(SELECT POS_No, ItemCode, SUM(RecQty) AS Assy1RecQty, SUM(TransQty) AS Assy1TransQty FROM tbAllTransaction WHERE LocCode='Assy1' AND CancelStatus=0 GROUP BY POS_No, ItemCode) tAssy1 " +
                    "ON tALL.POS_No=tAssy1.POS_No AND tAll.ItemCode=tAssy1.ItemCode "+SQLAllConds, cnnAll.con);
                sda.Fill(dtAllData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា Uncomplete Semi !\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnnAll.con.Close();

            //Assign Semi information to All data
            foreach (DataRow allRow in dtAllData.Rows)
            {
                foreach(DataRow row in dtOBSData.Rows) 
                {
                    if (allRow[2].ToString() == row[0].ToString())
                    {
                        allRow[3] = row[1].ToString();
                        allRow[4] = row[2].ToString();
                        allRow[5] = row[3].ToString();
                        allRow[6] = row[4].ToString();
                        dtAllData.AcceptChanges();
                        break;
                    }
                }
            }
            foreach (DataRow row in dtAllData.Rows)
            {
                DateTime ShipDate = Convert.ToDateTime(row[0].ToString());
                string POSNo = row[1].ToString();
                string WipCode = row[2].ToString();
                string WipName = row[3].ToString();
                string PIN = row[4].ToString();
                string Wire = row[5].ToString();
                string Length = row[6].ToString();
                double MC1Rec = 0;
                if (row[7].ToString().Trim() != "")
                {
                    MC1Rec = Convert.ToDouble(row[7].ToString());
                }
                double MC1Trans = 0;
                if (row[8].ToString().Trim() != "")
                {
                    MC1Trans = Convert.ToDouble(row[8].ToString());
                }
                double WIP1Rec = 0;
                if (row[9].ToString().Trim() != "")
                {
                    WIP1Rec = Convert.ToDouble(row[9].ToString());
                }
                double WIP1Trans = 0;
                if (row[10].ToString().Trim() != "")
                {
                    WIP1Trans = Convert.ToDouble(row[10].ToString());
                }
                double Assy1Rec = 0;
                if (row[11].ToString().Trim() != "")
                {
                    Assy1Rec = Convert.ToDouble(row[11].ToString());
                }
                double Assy1Trans = 0;
                if (row[12].ToString().Trim() != "")
                {
                    Assy1Trans = Convert.ToDouble(row[12].ToString());
                }

                if (WipName.Trim() != "")
                {
                    dgvSearchResult.Rows.Add(ShipDate, POSNo, WipCode, WipName, PIN, Wire, Length, MC1Rec, MC1Trans, WIP1Rec, WIP1Trans, Assy1Rec, Assy1Trans);

                }
            }

            Cursor = Cursors.Default;
            LbStatus.Text = "រកឃើញទិន្នន័យ " + dgvSearchResult.Rows.Count.ToString("N0");
            LbStatus.Refresh();
            dgvSearchResult.ClearSelection();
        }

    }
}
