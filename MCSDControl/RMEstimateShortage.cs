using MachineDeptApp.MCSDControl.WIR1__Wire_Stock_;
using MachineDeptApp.MsgClass;
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

namespace MachineDeptApp.MCSDControl
{
    public partial class RMEstimateShortage : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS sobs = new SQLConnectOBS();
        ErrorMsgClass ErrorMsgClass = new ErrorMsgClass();
        DataTable dtDeliveryDetail = new DataTable();
        DataTable dtRMSchuleHeader = new DataTable();
        DataTable dtRMSchuleDetails = new DataTable();
        string ErrorText;

        public RMEstimateShortage()
        {
            InitializeComponent();
            cnn.Connection();
            sobs.Connection();
            btnSearch.Click += BtnSearch_Click;

            tbItemCode.TextChanged += TbItemCode_TextChanged;
            tbItemName.TextChanged += TbItemName_TextChanged;
            dgvStock.CellDoubleClick += DgvStock_CellDoubleClick;

        }

        private void DgvStock_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DateTime PosDeliveryDate = Convert.ToDateTime(dgvStock.Rows[dgvStock.CurrentCell.RowIndex].Cells["PosDeliveryDate"].Value);
            string RMCode = dgvStock.Rows[dgvStock.CurrentCell.RowIndex].Cells["RMCode"].Value.ToString();
            DataTable rows = dtDeliveryDetail.AsEnumerable().Where(r => r.Field<DateTime>("POSDeliveryDate") == PosDeliveryDate &&
            r.Field<string>("RMCode") == RMCode
            ).CopyToDataTable();
            //Console.WriteLine(rows.Count());
            //DataTable result = rows.CopyToDataTable();
            string RMShortageQty = dgvStock.Rows[dgvStock.CurrentCell.RowIndex].Cells["RMShortage"].Value.ToString();
            ShortageDetail shortageDetail = new ShortageDetail(rows, RMShortageQty);
            shortageDetail.ShowDialog();
        }

        private void TbItemName_TextChanged(object sender, EventArgs e)
        {
            cbItemName.Checked = true;
        }

        private void TbItemCode_TextChanged(object sender, EventArgs e)
        {
            cbItemCode.Checked = true;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            labelDataCount.Text = "SearchIng...";
            labelDataCount.Refresh();

            DataTable condition = new DataTable();
            condition.Columns.Add("conditions");
            if (cbItemCode.Checked == true)
            {
                condition.Rows.Add(" T2.ItemCode Like '%" + tbItemCode.Text + "%' ");
            }
            if (cbItemName.Checked == true)
            {
                condition.Rows.Add(" T3.ItemName like '%" + tbItemName.Text + "%'  ");
            }
            condition.Rows.Add(" T1.POSDeliveryDate between '" + dtpPosShipStart.Value.ToString("yyyy-MM-dd") + "' And '" + dtpPosShipend.Value.ToString("yyyy-MM-dd") + "' ");
            string conditionQuery = "";
            for (int i = 0; i < condition.Rows.Count; i++)
            {
                if (conditionQuery.Trim() == "")
                {
                    conditionQuery = " Where " + condition.Rows[i][0];
                }
                else
                {
                    conditionQuery += " And " + condition.Rows[i][0];
                }
            }
            try
            {
                string queryStock = @"
SELECT
    T1.POSDeliveryDate,
    T2.ItemCode,
    T3.ItemName,
    SUM(ConsumpQty) AS TTLUsedQty,
    MCQty,
    RMQty,
    SUM(ConsumpQty) - (COALESCE(MCQty,0) + COALESCE(RMQty,0)) AS ShortageQty
FROM 
    (SELECT * FROM prgproductionorder WHERE CreateDate > '2024-07-01') T1
INNER JOIN 
    prgconsumtionorder T2 ON T1.ProductionCode = T2.ProductionCode
INNER JOIN 
    (SELECT * FROM mstitem WHERE DelFlag = 0 AND ItemType = 2 AND MatCalcFlag = 1) T3 
        ON T2.ItemCode = T3.ItemCode
INNER JOIN 
    (SELECT * FROM mstitem WHERE DelFlag = 0 AND ItemType = 1) T5 
        ON T1.ItemCode = T5.ItemCode
INNER JOIN  [" + cnn.server + "]." + @" 
    [MachineDB].dbo.tbPOSDetailOfMC T6 
        ON T1.DONo = T6.PosCNo AND T6.PosCStatus <> 2
LEFT JOIN 
(
    SELECT Code, SUM(StockValue) AS MCQty
    FROM [" + cnn.server + "]." + @" [MachineDB].dbo.tbSDMCAllTransaction
    WHERE CancelStatus = 0
    GROUP BY Code
) tbMCStock ON T2.ItemCode = tbMCStock.Code
LEFT JOIN 
(
    SELECT 
        T1.LocCode, 
        T1.ItemCode, 
        (SUM(Quantity) - SUM(COALESCE(Qty,0))) AS RMQty
    FROM 
        prgstock T1
    INNER JOIN 
        mstitem T2 ON T1.ItemCode = T2.ItemCode AND T2.ItemType = 2
    LEFT JOIN 
    (
        SELECT LocCode, ItemCode, LotNo, BoxNO, SUM(RealValueQty) AS Qty
        FROM prgalltransaction
        WHERE TypeCode = 1 
        AND CAST(TransactionDate AS date) > CAST(GETDATE() AS date)
        GROUP BY LocCode, ItemCode, LotNo, BoxNO
    ) T3 
        ON T1.LocCode = T3.LocCode 
        AND T1.ItemCode = T3.ItemCode 
        AND T1.LotNo = T3.LotNo 
        AND T1.BoxNO = T3.BoxNO
    WHERE Quantity - COALESCE(Qty,0) > 0 
        AND T1.LocCode = 'RM1'
    GROUP BY T1.LocCode, T1.ItemCode
) tbRMStock ON T2.ItemCode = tbRMStock.ItemCode
" + conditionQuery + @"
GROUP BY 
    T1.POSDeliveryDate, T2.ItemCode, T3.ItemName, MCQty, RMQty
ORDER BY 
    T1.POSDeliveryDate ASC, 
    T2.ItemCode ASC;
";

                SqlDataAdapter SqlStock = new SqlDataAdapter(queryStock, sobs.conOBS);
                DataTable dtStock = new DataTable();
                SqlStock.Fill(dtStock);
                dgvStock.Rows.Clear();
                for (int i = dgvStock.Columns.Count; i > 7; i--)
                {
                    dgvStock.Columns.RemoveAt(dgvStock.Columns.Count - 1);
                }
                if (dtStock.Rows.Count > 0)
                {
                    for (int i = 0; i < dtStock.Rows.Count; i++)
                    {
                        dgvStock.Rows.Add();
                        dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["POSDeliveryDate"].Value = Convert.ToDateTime(dtStock.Rows[i]["POSDeliveryDate"]).ToString("dd-MMM-yyyy");
                        dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["RMCode"].Value = dtStock.Rows[i]["ItemCode"];
                        dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["RMDescription"].Value = dtStock.Rows[i]["ItemName"];
                        //dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["TTLRMUse"].Value = dtStock.Rows[i]["TTLUsedQty"];
                        //dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["Received"].Value = dtStock.Rows[i]["POSDeliveryDate"];
                        //dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["NotYet"].Value = dtStock.Rows[i]["POSDeliveryDate"];
                        //dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["McQty"].Value = dtStock.Rows[i]["MCQty"];
                        if (double.TryParse(dtStock.Rows[i]["TTLUsedQty"].ToString(), out double TTLUsedQty))
                        {
                            dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["TTLRMUse"].Value = TTLUsedQty.ToString("N2");
                        }
                        else
                        {
                            dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["TTLRMUse"].Value = (0.00).ToString("N2");
                        }
                        if (double.TryParse(dtStock.Rows[i]["MCQty"].ToString(), out double McQty))
                        {
                            dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["McQty"].Value = McQty.ToString("N2");
                        }
                        else
                        {
                            dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["McQty"].Value = (0.00).ToString("N2");
                        }
                        //dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["WHStock"].Value = dtStock.Rows[i]["RMQty"];
                        if (double.TryParse(dtStock.Rows[i]["RMQty"].ToString(), out double RMQty))
                        {
                            dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["WHStock"].Value = RMQty.ToString("N2");
                        }
                        else
                        {
                            dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["WHStock"].Value = (0.00).ToString("N2");
                        }

                        double.TryParse(dtStock.Rows[i]["ShortageQty"].ToString(), out double shortageQty);
                        if (shortageQty <= 0)
                        {
                            dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["RMShortage"].Value = (0.00).ToString("N2");
                        }
                        else
                        {
                            dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["RMShortage"].Value = (shortageQty).ToString("N2");
                            dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["RMShortage"].Style.ForeColor = Color.Red;
                        }
                    }
                }
                labelDataCount.Text = dtStock.Rows.Count.ToString() + "ជួរ";
                dtDeliveryDetail = new DataTable();
                SqlDataAdapter sqlDetail = new SqlDataAdapter(" SELECT T1.POSDeliveryDate, DONo, T1.ItemCode AS WIPCode, T5.ItemName  AS WIPName, T1.LineCode, T2.ItemCode AS RMCode, T3.ItemName AS RMName, SUM(ConsumpQty) AS TTLUsedQty FROM \r\n" +
                    " (SELECT * FROM prgproductionorder WHERE CreateDate > '" + dtpPosShipStart.Value.ToString("dd-MM-yyyy") + "') T1 \r\n " +
                    " INNER JOIN (SELECT * FROM prgconsumtionorder) T2 ON T1.ProductionCode = T2.ProductionCode \r\n" +
                    " INNER JOIN (SELECT * FROM mstitem WHERE DelFlag = 0 AND ItemType = 2 AND MatCalcFlag = 1) T3 ON T2.ItemCode=T3.ItemCode \r\n" +
                    " INNER JOIN (SELECT * FROM mstitem WHERE DelFlag = 0 AND ItemType = 1) T5 ON T1.ItemCode = T5.ItemCode \r\n" +
                    " INNER JOIN (SELECT * FROM [" + cnn.server + "].[MachineDB].dbo.tbPOSDetailofMC) T6 ON T1.DONo = T6.PosCNo AND T6.PosCStatus <> 2  \r\n" +
                    " GROUP BY T1.POSDeliveryDate, DONo, T1.ItemCode, T5.ItemName, T1.LineCode, T2.ItemCode, T3.ItemName\r\n " +
                    " ORDER BY T1.POSDeliveryDate ASC, T1.LineCode ASC, T2.ItemCode ASC ", sobs.conOBS);
                sqlDetail.Fill(dtDeliveryDetail);
                string RMCodeIN = "";

                if (dtStock.Rows.Count > 0) {
                    foreach (DataRow row in dtStock.Rows)
                    {
                        if (Convert.ToDouble(row["ShortageQty"]) > 0)
                        {
                            if (RMCodeIN.Trim() == "")
                                RMCodeIN = "'" + row["ItemCode"] + "'";
                            else
                                RMCodeIN += " , '" + row["ItemCode"] + "'";
                        }
                    }
                }
                Console.WriteLine(RMCodeIN);
                dtRMSchuleDetails = new DataTable();
                dtRMSchuleHeader = new DataTable();
                if (RMCodeIN.Trim() != "")
                {
                    string SQLQuery = "SELECT * FROM [PPDeptDB].dbo.tbRMSchedule WHERE Date>= CAST(GETDATE() AS date) AND Code IN (" + RMCodeIN + ") ";
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtRMSchuleDetails);

                    SQLQuery = "SELECT Date FROM [PPDeptDB].dbo.tbRMSchedule WHERE Date>= CAST(GETDATE() AS date) AND Code IN (" + RMCodeIN + ") " +
                        "\nGROUP BY Date ORDER BY Date ASC";
                    sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtRMSchuleHeader);
                }
                if (dtRMSchuleHeader.Rows.Count > 0)
                {
                    foreach (DataRow row in dtRMSchuleHeader.Rows)
                    {
                        string Date = row["Date"].ToString();
                        dgvStock.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            Name = Date,
                            HeaderText = Convert.ToDateTime(Date).ToString("dd-MMM"),
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                            DefaultCellStyle = new DataGridViewCellStyle
                            {
                                Format = "N0",
                                Alignment = DataGridViewContentAlignment.MiddleRight
                            }
                        });

                        int LColIndex = dgvStock.Columns.Count - 1;
                        dgvStock.Columns[LColIndex].HeaderCell.Style = new DataGridViewCellStyle
                        {
                            BackColor = Color.Orange,
                            Alignment = DataGridViewContentAlignment.MiddleCenter
                        };
                        dgvStock.Refresh();
                        foreach (DataRow detailsR in dtRMSchuleDetails.Rows)
                        {
                            if (detailsR["Date"].ToString() == Date)
                            {
                                string Code = detailsR["Code"].ToString();
                                double Qty = Convert.ToDouble(detailsR["Qty"]);
                                foreach (DataGridViewRow dgvR in dgvStock.Rows)
                                {
                                    if (dgvR.Cells["RMCode"].Value.ToString() == Code && dgvR.Cells["RMShortage"].Value != null && Convert.ToDouble(dgvR.Cells["RMShortage"].Value) > 0)
                                    {
                                        dgvR.Cells[LColIndex].Value = Qty;
                                    }
                                }
                            }
                        }
                    }

                    

                }
            }

            

            catch (Exception ex)
            {

                ErrorMsgClass.AlertText = ex.Message;
                ErrorMsgClass.ShowingMsg();
            }



            Cursor = Cursors.Default;
          

        }
    }
}
