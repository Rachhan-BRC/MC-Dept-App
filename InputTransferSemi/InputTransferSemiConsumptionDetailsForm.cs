using MachineDeptApp.MCSDControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.InputTransferSemi
{
    public partial class InputTransferSemiConsumptionDetailsForm : Form
    {
        InputTransferSemiForm fgrid;
        public InputTransferSemiConsumptionDetailsForm(InputTransferSemiForm fg)
        {
            InitializeComponent();
            this.fgrid = fg;
            this.Load += InputTransferSemiConsumptionDetailsForm_Load;
            this.dgvPOSConsumption.SelectionChanged += DgvPOSConsumption_SelectionChanged;
        }

        private void DgvPOSConsumption_SelectionChanged(object sender, EventArgs e)
        {
            LbTotalSelect.Text = "";
            double Total = 0;
            for (int i = 0; i < dgvPOSConsumption.Rows.Count; i++)
            {
                if (dgvPOSConsumption.Rows[i].Cells[5].Selected == true)
                {
                    Total = Total + Convert.ToDouble(dgvPOSConsumption.Rows[i].Cells[5].Value.ToString());
                }
            }
            LbTotalSelect.Text = Total.ToString("N3");
        }

        private void InputTransferSemiConsumptionDetailsForm_Load(object sender, EventArgs e)
        {
            this.Text = InputTransferSemiForm.SelectedRMCode + "  |  " + InputTransferSemiForm.SelectedRMName;
            foreach (DataRow row in fgrid.dtRMConsumption.Rows)
            {
                string POSNo = "";
                string BoxNo = "";
                double BOM = 0;
                double Usage = 0;
                if (row[2].ToString() == InputTransferSemiForm.SelectedRMCode)
                {
                    POSNo = row[0].ToString();
                    BoxNo = row[1].ToString();
                    BOM = Convert.ToDouble(row[4].ToString());
                    Usage = Convert.ToDouble(row[5].ToString());
                }
                if (POSNo.Trim() != "" && BoxNo.Trim() != "")
                {
                    string Items = "";
                    double Qty = 0;
                    foreach (DataGridViewRow DgvRow in fgrid.dgvScannedTag.Rows)
                    {
                        if (DgvRow.Cells[0].Value.ToString() == POSNo && DgvRow.Cells[3].Value.ToString() == BoxNo)
                        {
                            Items = DgvRow.Cells[2].Value.ToString();
                            Qty = Convert.ToDouble(DgvRow.Cells[4].Value.ToString());
                            break;
                        }
                    }
                    dgvPOSConsumption.Rows.Add(POSNo, Items, BoxNo, Qty, BOM, Usage);
                }
            }
            dgvPOSConsumption.ClearSelection();
        }

    }
}
