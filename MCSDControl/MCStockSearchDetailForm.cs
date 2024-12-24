using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.MCSDControl.SDRec
{
    public partial class MCStockSearchDetailForm : Form
    {
        MCStockSearchForm fgrid;
        public MCStockSearchDetailForm(MCStockSearchForm fg)
        {
            InitializeComponent();
            this.fgrid = fg;
            this.Load += MCStockSearchDetailForm_Load;
            this.dgvStockDetails.SelectionChanged += DgvStockDetails_SelectionChanged;
        }

        private void MCStockSearchDetailForm_Load(object sender, EventArgs e)
        {
            this.Text = MCStockSearchForm.SelectedCode + "  |  " + MCStockSearchForm.SelectedItems + "  |  " + MCStockSearchForm.SelectedLocName;

            foreach (DataRow row in fgrid.dtStockDetails.Rows)
            {
                if (row[0].ToString() == MCStockSearchForm.SelectedCode && row[3].ToString() == MCStockSearchForm.SelectedLocCode)
                {
                    dgvStockDetails.Rows.Add(row[1], row[2], Convert.ToDouble(row[4].ToString()));
                }
            }

            int rowNumber = 1;
            foreach (DataGridViewRow row in dgvStockDetails.Rows)
            {
                row.HeaderCell.Style.BackColor = Color.FromKnownColor(KnownColor.GradientActiveCaption);
                row.HeaderCell.Style.Font = new Font("Khmer OS Battambong", 11, FontStyle.Regular);
                row.HeaderCell.Value = rowNumber.ToString();
                rowNumber = rowNumber + 1;
            }

            dgvStockDetails.ClearSelection();
            LbTotalSelect.Text = "";
        }

        private void DgvStockDetails_SelectionChanged(object sender, EventArgs e)
        {
            LbTotalSelect.Text = "";
            double Total = 0;
            for (int i = 0; i < dgvStockDetails.Rows.Count; i++)
            {
                if (dgvStockDetails.Rows[i].Cells[2].Selected == true)
                {
                    Total = Total + Convert.ToDouble(dgvStockDetails.Rows[i].Cells[2].Value.ToString());
                }
            }
            LbTotalSelect.Text = Total.ToString("N0");
        }
    }
}
