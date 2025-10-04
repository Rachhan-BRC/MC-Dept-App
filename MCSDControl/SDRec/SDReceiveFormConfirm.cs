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
    public partial class SDReceiveFormConfirm : Form
    {
        DataGridView dgvMain;
        DataTable dtMain, dtPOSDetail, dtRMDetails;

        public SDReceiveFormConfirm(DataTable dt, DataGridView dgv, DataTable dtPOS, DataTable dtRM)
        {
            InitializeComponent();
            this.dtMain = dt;
            this.dgvMain = dgv;
            this.dtPOSDetail = dtPOS;
            this.dtRMDetails = dtRM;
            this.dgvConsumption.CellFormatting += DgvConsumption_CellFormatting;
            this.btnOK.Click += BtnOK_Click;
            this.Load += SDReceiveFormConfirm_Load;

        }

        private void SDReceiveFormConfirm_Load(object sender, EventArgs e)
        {
            dgvConsumption.RowHeadersDefaultCellStyle.Font = new Font("Calibri", 10,FontStyle.Regular);
            LbPOSNo.Text = dtPOSDetail.Rows[0]["DONO"].ToString();
            LbShipDate.Text = Convert.ToDateTime(dtPOSDetail.Rows[0]["POSDeliveryDate"]).ToString("dd-MM-yyyy");
            LbWipCode.Text = dtPOSDetail.Rows[0]["ItemCode"].ToString();
            LbWipName.Text = dtPOSDetail.Rows[0]["ItemName"].ToString();
            LbQty.Text = Convert.ToDouble(dtPOSDetail.Rows[0]["PlanQty"]).ToString("N0");

            foreach (DataRow row in dtRMDetails.Rows)
            {
                dgvConsumption.Rows.Add();
                dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].HeaderCell.Value = dgvConsumption.Rows.Count.ToString();
                dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["RMCode"].Value = row["ItemCode"];
                dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["RMName"].Value = row["ItemName"];
                dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["BOMQty"].Value = Convert.ToDouble(row["BOMQty"]);
                dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["ConsumptionQty"].Value = Convert.ToDouble(row["ConsumpQty"]);
                dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["KITTransferQty"].Value = Convert.ToDouble(row["KITTranQty"]);
                if(row["RecQty"].ToString().Trim()!="")
                    dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["MCRecQty"].Value = Convert.ToDouble(row["RecQty"]);
                dgvConsumption.Rows[dgvConsumption.Rows.Count - 1].Cells["MCRemQty"].Value = Convert.ToDouble(row["RemainMC_KITQty"]);
            }
            dgvConsumption.ClearSelection();

        }
        private void BtnOK_Click(object sender, EventArgs e)
        {
            int FoundAbnormal = 0;
            foreach (DataGridViewRow row in dgvConsumption.Rows)
            {
                if (Convert.ToDouble(row.Cells["KITTransferQty"].Value.ToString()) < Convert.ToDouble(row.Cells["ConsumptionQty"].Value.ToString()))
                {
                    FoundAbnormal++;
                }
            }
            if (FoundAbnormal == 0)
            {
                AddToMainForm();
                this.Close();
            }
            else
            {
                DialogResult DLS = MessageBox.Show("មានវត្ថុធាតុដើមដែល Kitting Room វេរឱ្យមិនទាន់គ្រប់!\nតើអ្នកចង់បន្ដឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (DLS == DialogResult.Yes)
                {
                    SDReceiveFormConfirmUser Srfcu = new SDReceiveFormConfirmUser();
                    Srfcu.ShowDialog();
                    if (Srfcu.ID.ToString().Trim()!="" && Srfcu.Password.ToString().Trim()!="")
                    {
                        AddToMainForm();
                        this.Close();
                    }
                }
            }

        }
        private void DgvConsumption_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvConsumption.Columns[e.ColumnIndex].Name == "KITTransferQty")
                {
                    double ConsumptionQty = Convert.ToDouble(dgvConsumption.Rows[e.RowIndex].Cells["ConsumptionQty"].Value);
                    double KITTransferQty = Convert.ToDouble(dgvConsumption.Rows[e.RowIndex].Cells["KITTransferQty"].Value);
                    if (KITTransferQty >= ConsumptionQty)
                    {
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.Font = new Font(dgvConsumption.AlternatingRowsDefaultCellStyle.Font, FontStyle.Bold);
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.Red;
                        e.CellStyle.Font = new Font(dgvConsumption.AlternatingRowsDefaultCellStyle.Font, FontStyle.Bold);
                    }
                }

                if (dgvConsumption.Columns[e.ColumnIndex].Name == "MCRemQty")
                {
                    double ConsumptionQty = Convert.ToDouble(dgvConsumption.Rows[e.RowIndex].Cells["ConsumptionQty"].Value);
                    double MCRemainQty = Convert.ToDouble(dgvConsumption.Rows[e.RowIndex].Cells["MCRemQty"].Value);
                    if (MCRemainQty > 0 )
                    {
                        if (MCRemainQty == ConsumptionQty)
                        {
                            e.CellStyle.ForeColor = Color.Green;
                            e.CellStyle.Font = new Font(dgvConsumption.AlternatingRowsDefaultCellStyle.Font, FontStyle.Bold);
                        }
                        else
                        {
                            e.CellStyle.ForeColor = Color.Orange;
                            e.CellStyle.Font = new Font(dgvConsumption.AlternatingRowsDefaultCellStyle.Font, FontStyle.Bold);
                        }
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.Font = new Font(dgvConsumption.AlternatingRowsDefaultCellStyle.Font, FontStyle.Bold);
                    }
                }

            }
        }


        //Method
        private void AddToMainForm()
        {
            Cursor = Cursors.WaitCursor;
            //Check Complete-Set or not
            int CountNCompleteSet = 0;
            foreach (DataGridViewRow row in dgvConsumption.Rows)
            {
                if (Convert.ToDouble(row.Cells["MCRemQty"].Value) != Convert.ToDouble(row.Cells["ConsumptionQty"].Value))
                {
                    CountNCompleteSet++;
                    break;
                }
            }

            //Add to DGV
            dgvMain.Rows.Add();
            dgvMain.Rows[dgvMain.Rows.Count-1].Cells["POSNo"].Value = LbPOSNo.Text;
            dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["WIPCode"].Value = LbWipCode.Text;
            dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["WIPName"].Value = LbWipName.Text;
            dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["POSQty"].Value = Convert.ToDouble(LbQty.Text);
            dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["ShipDate"].Value = Convert.ToDateTime(LbShipDate.Text);
            //Complete Set
            if (CountNCompleteSet == 0)
                dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["CompleteSet"].Value = true;
            //Not-Complete Set
            else
                dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["CompleteSet"].Value = false;

            //Add Consumption to dtSQLSaving-Main
            foreach (DataGridViewRow row in dgvConsumption.Rows)
            {
                string POSNo = LbPOSNo.Text;
                string RMCode = row.Cells["RMCode"].Value.ToString();
                string RMName = row.Cells["RMName"].Value.ToString();
                double BOMQty = Convert.ToDouble(row.Cells["BOMQty"].Value);
                double ConsumptionQty = Convert.ToDouble(row.Cells["ConsumptionQty"].Value);
                double KITTransferQty = Convert.ToDouble(row.Cells["KITTransferQty"].Value);
                double MCRemQty = Convert.ToDouble(row.Cells["MCRemQty"].Value);

                dtMain.Rows.Add(POSNo, RMCode); //Need to Assign during add because it's PrimaryKey
                dtMain.Rows[dtMain.Rows.Count-1]["ItemName"] = RMName;
                dtMain.Rows[dtMain.Rows.Count-1]["BOMQty"] = BOMQty;
                dtMain.Rows[dtMain.Rows.Count-1]["ConsumpQty"] = ConsumptionQty;
                dtMain.Rows[dtMain.Rows.Count-1]["KITTranQty"] = KITTransferQty;
                if(row.Cells["MCRecQty"].Value != null && row.Cells["MCRecQty"].Value.ToString().Trim() != "")
                    dtMain.Rows[dtMain.Rows.Count-1]["RecQty"]=Convert.ToDouble(row.Cells["MCRecQty"].Value);
                dtMain.Rows[dtMain.Rows.Count-1]["RemainMC_KITQty"] = MCRemQty;
                dtMain.AcceptChanges();
            }

            Cursor = Cursors.Default;
            
        }

    }
}
