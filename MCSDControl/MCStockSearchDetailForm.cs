using System;
using System.Data;
using System.Windows.Forms;

namespace MachineDeptApp.MCSDControl.SDRec
{
    public partial class MCStockSearchDetailForm : Form
    {
        DataGridView dgvMain;
        DataTable dtMain;
        public MCStockSearchDetailForm(DataGridView dgv, DataTable dt)
        {
            InitializeComponent();
            this.dgvMain = dgv;
            this.dtMain = dt;
            this.Load += MCStockSearchDetailForm_Load;
            this.dgvStockDetails.SelectionChanged += DgvStockDetails_SelectionChanged;

        }

        private void MCStockSearchDetailForm_Load(object sender, EventArgs e)
        {
            DataGridViewCell cell = dgvMain.CurrentCell;
            string RMCode = dgvMain.Rows[cell.RowIndex].Cells["RMCode"].Value.ToString();
            string RMDes = dgvMain.Rows[cell.RowIndex].Cells["RMDes"].Value.ToString();
            string LocName = dgvMain.Columns[cell.ColumnIndex].HeaderText.ToString();
            string LocCode = dgvMain.Columns[cell.ColumnIndex].Name.ToString();
            this.Text = RMCode + " | " + RMDes + "  |  " + LocName;
            foreach (DataRow row in dtMain.Rows)
            {
                if (row[0].ToString() == RMCode && row[3].ToString().ToUpper() == LocCode.ToUpper())
                {
                    dgvStockDetails.Rows.Add(row[1], row[2], Convert.ToDouble(row[4].ToString()));
                    dgvStockDetails.Rows[dgvStockDetails.Rows.Count-1].HeaderCell.Value = dgvStockDetails.Rows.Count.ToString();
                }
            }
            dgvStockDetails.ClearSelection();
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
            LbTotalSelect.Text = Total.ToString("N3");
        }
    }
}
