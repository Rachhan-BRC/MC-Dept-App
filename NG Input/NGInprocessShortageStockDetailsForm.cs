using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.NG_Input
{
    public partial class NGInprocessShortageStockDetailsForm : Form
    {
        NGInprocessSearchForm fgrid;
        public NGInprocessShortageStockDetailsForm(NGInprocessSearchForm fg)
        {
            InitializeComponent();
            this.fgrid = fg;
            this.Load += NGInprocessShortageStockDetailsForm_Load;
        }

        private void NGInprocessShortageStockDetailsForm_Load(object sender, EventArgs e)
        {
            foreach (DataRow row in fgrid.dtNotEnough.Rows)
            {
                dgvNGShortage.Rows.Add();
                dgvNGShortage.Rows[dgvNGShortage.Rows.Count - 1].Cells["POSNo"].Value = row["POSNo"].ToString();
                dgvNGShortage.Rows[dgvNGShortage.Rows.Count - 1].Cells["RMCode"].Value = row["ItemCode"].ToString();
                dgvNGShortage.Rows[dgvNGShortage.Rows.Count - 1].Cells["RMName"].Value = row["ItemName"].ToString();
                dgvNGShortage.Rows[dgvNGShortage.Rows.Count - 1].Cells["NGQty"].Value = Convert.ToDouble(row["Qty"].ToString());
            }
            dgvNGShortage.ClearSelection();
        }
    }
}
