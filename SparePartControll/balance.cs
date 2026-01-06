using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp
{
    public partial class balance : Form
    {
        string dept = "MC";
        SQLConnect con = new SQLConnect();
        public balance()
        {
            this.con.Connection();
            InitializeComponent();
            this.btnSearch.Click += BtnSearch_Click;
            this.txtRcode.TextChanged += TxtRcode_TextChanged;
            this.txtRname.TextChanged += TxtRname_TextChanged;
            this.Load += Balance_Load;
        }

        private void Balance_Load(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

        private void TxtRname_TextChanged(object sender, EventArgs e)
        {
            chkRname.Checked = true;

            if (txtRname.Text.Trim() == "")
            {
                chkRname.Checked = false;
            }
            btnSearch.PerformClick();
        }

        private void TxtRcode_TextChanged(object sender, EventArgs e)
        {
            chkRcode.Checked = true;
            if (txtRcode.Text.Trim() == "")
            {
                chkRcode.Checked = false;
            }
            btnSearch.PerformClick();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            dgvTTL.Rows.Clear();
            Cursor = Cursors.WaitCursor;
            DataTable cond = new DataTable();
            DataTable dtselect = new DataTable();
            cond.Columns.Add("Value");
            if (chkRcode.Checked == true)
            {
                string val = txtRcode.Text;
                if (val != "")
                {
                    cond.Rows.Add("tbTr.Code LIKE '%" + val + "%'");
                }
            }
            if (chkRname.Checked == true)
            {
                string val = txtRname.Text;
                if (val != "")
                {
                    cond.Rows.Add("tbMst.Part_Name LIKE '%" + val + "%'");
                }
            }
            string Conds = "";
            foreach (DataRow row in cond.Rows)
            {
                if (Conds.Trim() == "")
                {
                    Conds = " AND " + row["Value"];
                }
                else
                {
                    Conds += " AND " + row["Value"];
                }
            }
            try
            {
                con.con.Open();
                DateTime prev = dtpDate.Value.AddMonths(-1);
                DateTime last = new DateTime(prev.Year, prev.Month, DateTime.DaysInMonth(prev.Year, prev.Month));
                DateTime date1 = Convert.ToDateTime(dtpDate.Value);
                DateTime firstDay = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, 1);

                // Last day of the month
                DateTime lastDay = new DateTime(date1.Year, date1.Month, DateTime.DaysInMonth(date1.Year, date1.Month));
                string lastDayStr = lastDay.ToString("yyyy-MM-dd");
                string query = "SELECT tbTr.Code, tbMSt.Supplier, tbMst.Part_No, tbMst.Part_Name, tbPre.QtyIn,tbPre.QtyOut , tbPreS.PreQty " +
                    "FROM SparePartTrans tbTr " +
                    "LEFT JOIN  " +
                    "(SELECT Code,Supplier,Part_Name,Part_No FROM MstMCSparePart) tbMst ON tbMst.Code = tbTr.Code " +
                    "LEFT JOIN " +
                    "(SELECT Code, SUM(Stock_In) AS QtyIn,SUM(Stock_Out) AS QtyOut FROM SparePartTrans " +
                    "WHERE RegDate BETWEEN '"+firstDay+"' AND '"+lastDay+"' AND Dept ='MC' GROUP BY Code ) tbPre ON tbPre.Code = tbTr.Code " +
                    "LEFT JOIN (SELECT Code, SUM(Stock_Value) AS PreQty FROM SparePartTrans " +
                    "WHERE CAST(RegDate AS date) <= '"+last+"' AND Dept ='MC' GROUP BY Code ) tbPreS ON tbPreS.Code = tbPre.Code " +
                    "WHERE CAST(tbTr.RegDate AS DATE) > '"+last+"' AND tbtr.Dept ='MC' " +
                    "GROUP BY tbTr.Code, tbMst.Supplier,tbMst.Part_No, tbMst.Part_Name,tbPre.QtyIn,tbPre.QtyOut, tbPreS.PreQty " +
                    "Order by tbTr.Code";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dtselect);
                if (dtselect.Rows.Count > 0)
                {
                    foreach (DataRow row in dtselect.Rows)
                    {
                        string code = row["Code"]?.ToString() ?? "";
                        string partno = row["Part_No"]?.ToString() ?? "";
                        string partname = row["Part_Name"]?.ToString() ?? "";
                        string supplier = row["Supplier"]?.ToString() ?? "";
                        double prestock = double.TryParse(row["PreQty"]?.ToString(), out var ps) ? ps : 0;
                        double stockout = double.TryParse(row["QtyOut"]?.ToString(), out var so) ? so : 0;
                        double stockin = double.TryParse(row["QtyIn"]?.ToString(), out var si) ? si : 0;
                        double stockremain = prestock + stockin + stockout;
                        dgvTTL.Rows.Add();
                        dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["code"].Value = code;
                        dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["partno"].Value = partno;
                        dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["partname"].Value = partname;
                        dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["supplier"].Value = supplier;
                        dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["prestock"].Value = prestock;
                        dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["stockout"].Value = -stockout;
                        dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["stockin"].Value = stockin;
                        dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["stockremain"].Value = stockremain;
                        dgvTTL.ClearSelection();
                        Cursor = Cursors.Default;
                        lbFound.Text = "Found: " + dgvTTL.Rows.Count.ToString();
                    }
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Error while selecting data !" + ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.con.Close();
            Cursor = Cursors.Default;
        }
    }
}
