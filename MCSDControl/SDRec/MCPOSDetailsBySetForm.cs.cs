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
    public partial class MCPOSDetailsBySetForm : Form
    {
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        string[] CountRMExcept;

        public MCPOSDetailsBySetForm()
        {
            InitializeComponent();
            this.cnnOBS.Connection();
            this.Load += MCPOSDetailsBySetForm_Load;
            this.btnSearch.Click += BtnSearch_Click;

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            LbStatus.Visible = true;
            DgvPOS.Rows.Clear();
            string T1Cond = "";
            if (txtPOSCNo.Text.Trim() != "")
            {
                if (txtPOSCNo.Text.Contains("*") == true)
                {
                    string SearchValue = txtPOSCNo.Text.ToString();
                    SearchValue = SearchValue.Replace("*","%");
                    T1Cond = "AND DONo LIKE '"+SearchValue+"' ";
                }
                else
                {
                    T1Cond = "AND DONo = '" + txtPOSCNo.Text + "' ";
                }
            }            
            string ExceptRM = "";
            for (int i = 0; i < CountRMExcept.Length; i++)
            {
                if (ExceptRM.Trim() == "")
                {
                    ExceptRM = "'" + CountRMExcept[i] + "'";
                }
                else
                {
                    ExceptRM = ExceptRM + ", " + "'" + CountRMExcept[i] + "'";
                }
            }

            DataTable dtSQLCond=new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Value");
            if (txtCode.Text.Trim() != "")
            {
                string SearchValue = txtCode.Text.ToString();
                SearchValue = SearchValue.Replace("*", "");
                dtSQLCond.Rows.Add("T1.ItemCode LIKE ", "'" + SearchValue.Substring(0, 4) + "%' ");
            }
            if (txtItem.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("T3.ItemName LIKE ", "'%" + txtItem.Text + "%' ");
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
                    SQLCond = SQLCond+"AND " + row[0] + row[1];
                }
            }

            try
            {
                cnnOBS.conOBS.Open();
                //SqlDataAdapter sda = new SqlDataAdapter("SELECT PPOSNO, T1.DONo, T1.ItemCode, T3.ItemName, PlanQty, POSDeliveryDate FROM " +
                //                                                                "(SELECT PPOSNO, DONo, ItemCode, PlanQty, POSDeliveryDate FROM prgproductionorder " +
                //                                                                "WHERE PPOSNO IN (SELECT PPOSNO FROM prgproductionorder WHERE LineCode='MC1' " + T1Cond+ " GROUP BY PPOSNO)) T1 " +
                //                                                                "INNER JOIN " +
                //                                                                "(SELECT prgconsumtionorder.ProductionCode,prgproductionorder.DONo FROM prgconsumtionorder  " +
                //                                                                "INNER JOIN prgproductionorder ON prgproductionorder.ProductionCode = prgconsumtionorder.ProductionCode INNER JOIN mstitem ON  mstitem.ItemCode = prgconsumtionorder.ItemCode " +
                //                                                                "WHERE LEN(prgproductionorder.ItemCode) > 4 AND mstitem.MatCalcFlag = 0 AND mstitem.ItemType = 2 AND NOT mstitem.ItemCode IN ("+ ExceptRM + ") " +
                //                                                                "GROUP BY prgconsumtionorder.ProductionCode,prgproductionorder.DONo) T2 " +
                //                                                                "ON T1.DONo=T2.DONo " +
                //                                                                "LEFT JOIN " +
                //                                                                "(SELECT ItemCode, ItemName FROM mstitem WHERE ItemType=1) T3 " +
                //                                                                "ON T1.ItemCode=T3.ItemCode " + SQLCond+
                //                                                                "ORDER BY T1.DONo ASC, T1.PPOSNO ASC", cnnOBS.conOBS);

                SqlDataAdapter sda = new SqlDataAdapter("SELECT PPOSNO, T1.DONo, T1.ItemCode, T3.ItemName, PlanQty, POSDeliveryDate FROM " +
                                                                                "(SELECT PPOSNO, DONo, ItemCode, PlanQty, POSDeliveryDate FROM prgproductionorder " +
                                                                                "WHERE PPOSNO IN (SELECT PPOSNO FROM prgproductionorder WHERE LineCode='MC1' " + T1Cond + " GROUP BY PPOSNO)) T1 " +
                                                                                "INNER JOIN " +
                                                                                "(SELECT prgconsumtionorder.ProductionCode,prgproductionorder.DONo FROM prgconsumtionorder  " +
                                                                                "INNER JOIN prgproductionorder ON prgproductionorder.ProductionCode = prgconsumtionorder.ProductionCode INNER JOIN mstitem ON  mstitem.ItemCode = prgconsumtionorder.ItemCode " +
                                                                                "WHERE LEN(prgproductionorder.ItemCode) > 4 AND mstitem.MatCalcFlag = 0 AND mstitem.ItemType = 2 " +
                                                                                "GROUP BY prgconsumtionorder.ProductionCode,prgproductionorder.DONo) T2 " +
                                                                                "ON T1.DONo=T2.DONo " +
                                                                                "LEFT JOIN " +
                                                                                "(SELECT ItemCode, ItemName FROM mstitem WHERE ItemType=1) T3 " +
                                                                                "ON T1.ItemCode=T3.ItemCode " + SQLCond +
                                                                                "ORDER BY T1.DONo ASC, T1.PPOSNO ASC", cnnOBS.conOBS);

                DataTable dt = new DataTable(); 
                sda.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    DgvPOS.Rows.Add(row[0], row[1], row[2], row[3], Convert.ToDouble(row[4].ToString()), Convert.ToDateTime(row[5].ToString()));
                }
                DgvPOS.ClearSelection();
                Cursor = Cursors.Default;
                LbStatus.Text = "រកឃើញទិន្នន័យ "+DgvPOS.Rows.Count.ToString("N0");
                LbStatus.Refresh();
            }
            catch(Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("មានបញ្ហា!\n"+ex.Message,"Rachhan System",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnnOBS.conOBS.Close();
        }

        private void MCPOSDetailsBySetForm_Load(object sender, EventArgs e)
        {
            CountRMExcept = new string[] { "2114", "1000", "1053", "1132", "1138", "0386", "1015", "1019" };

        }
    }
}
