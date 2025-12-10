using MachineDeptApp.MsgClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    public partial class ShortageDetail : Form
    {
        private DataTable dtShortageDetail;
        ErrorMsgClass ErrorMsgClass;
        string ShortageQty;
        public ShortageDetail( DataTable dtDetail, string RMShortageQty)
        {
            InitializeComponent();
            this.dtShortageDetail = dtDetail;
            this.Load += ShortageDetail_Load;
             this.ShortageQty = RMShortageQty;
        }

        private void ShortageDetail_Load(object sender, EventArgs e)
        {
            if (dtShortageDetail.Rows.Count > 0)
            {
                try
                {
                    dgvStock.Rows.Clear();
                    if (dtShortageDetail.Rows.Count > 0)
                    {
                        double TotalUsed = 0;
                        for (int i = 0; i < dtShortageDetail.Rows.Count; i++)
                        {
                        dgvStock.Rows.Add();
                            dgvStock.Rows[i].Cells["PosDeliveryDate"].Value = Convert.ToDateTime( dtShortageDetail.Rows[i]["POSDeliveryDate"]).ToString("dd-MM-yyyy");
                            dgvStock.Rows[i].Cells["DONo"].Value = dtShortageDetail.Rows[i]["DONo"];
                            dgvStock.Rows[i].Cells["WipCode"].Value = dtShortageDetail.Rows[i]["WIPCode"];
                            dgvStock.Rows[i].Cells["WipName"].Value = dtShortageDetail.Rows[i]["WIPName"];
                            dgvStock.Rows[i].Cells["LineCode"].Value = dtShortageDetail.Rows[i]["LineCode"];
                            dgvStock.Rows[i].Cells["RMCode"].Value = dtShortageDetail.Rows[i]["RMCode"];
                            dgvStock.Rows[i].Cells["RMName"].Value = dtShortageDetail.Rows[i]["RMName"];
                            double.TryParse(dtShortageDetail.Rows[i]["TTLUsedQty"].ToString(), out double TTlUesd);
                            dgvStock.Rows[i].Cells["TTLusedQty"].Value = TTlUesd.ToString("N2");
                            TotalUsed += TTlUesd;
                        }
                        dgvStock.Rows.Add();
                        dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["TTLusedQty"].Value = TotalUsed.ToString("N2");
                        dgvStock.Rows[dgvStock.Rows.Count - 1].Cells["TTLusedQty"].Style.BackColor = Color.YellowGreen;
                        lbStatus.Text = dgvStock.Rows.Count.ToString() + " ជួរ";
                        this.lbShortage.Text = "Total Use Qty:  " + TotalUsed.ToString("N2");
                        this.lbLacking.Text = "RMShortage Qty: " + ShortageQty;
                        
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsgClass.AlertText = ex.Message;
                    ErrorMsgClass.ShowingMsg();
                }
            }
        }
    }
}


 