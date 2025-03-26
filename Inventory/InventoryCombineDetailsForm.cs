using MachineDeptApp.MsgClass;
using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace MachineDeptApp.Inventory
{
    public partial class InventoryCombineDetailsForm : Form
    {
        string TypeMain;
        DataGridView DgvMain;
        DataTable dtMain;
        WarningMsgClass WMsg = new WarningMsgClass();
        ErrorMsgClass EMsg = new ErrorMsgClass();

        string CurrentRMCode;
        string CurrentRMName;
        string CurrentDocNo;

        string ErrorText = "";

        public InventoryCombineDetailsForm(string type, DataGridView dgv, DataTable dt)
        {
            InitializeComponent();
            this.TypeMain = type;
            this.DgvMain = dgv;
            this.dtMain = dt;
            this.Shown += InventoryCombineDetailsForm_Shown;
            this.dgvPOS.SelectionChanged += DgvPOS_SelectionChanged;
            this.dgvSemi.SelectionChanged += DgvSemi_SelectionChanged;
            this.dgvSD.SelectionChanged += DgvSD_SelectionChanged;
            this.dgvNG.SelectionChanged += DgvNG_SelectionChanged;
        }

        private void DgvNG_SelectionChanged(object sender, EventArgs e)
        {
            LbTotalSelected.Text = "";
            double Total = 0;
            foreach (DataGridViewRow row in dgvNG.Rows)
            {
                if (row.Cells["QtyNG"].Selected == true)
                {
                    Total = Total + Convert.ToDouble(row.Cells["QtyNG"].Value);
                }
            }
            LbTotalSelected.Text = Total.ToString("N2");
        }
        private void DgvSD_SelectionChanged(object sender, EventArgs e)
        {
            LbTotalSelected.Text = "";
            double Total = 0;
            foreach (DataGridViewRow row in dgvSD.Rows)
            {
                if (row.Cells["QtySD"].Selected == true)
                {
                    Total = Total + Convert.ToDouble(row.Cells["QtySD"].Value);
                }
            }
            LbTotalSelected.Text = Total.ToString("N0");
        }
        private void DgvSemi_SelectionChanged(object sender, EventArgs e)
        {
            LbTotalSelected.Text = "";
            double Total = 0;
            foreach (DataGridViewRow row in dgvSemi.Rows)
            {
                if (row.Cells["QtySemi"].Selected == true)
                {
                    Total = Total + Convert.ToDouble(row.Cells["QtySemi"].Value);
                }
            }
            LbTotalSelected.Text = Total.ToString("N2");
        }
        private void DgvPOS_SelectionChanged(object sender, EventArgs e)
        {
            LbTotalSelected.Text = "";
            double Total = 0;
            foreach (DataGridViewRow row in dgvPOS.Rows)
            {
                if (row.Cells["QtyPOS"].Selected == true)
                {
                    Total = Total + Convert.ToDouble(row.Cells["QtyPOS"].Value);
                }
            }
            LbTotalSelected.Text = Total.ToString("N0");
        }

        private void InventoryCombineDetailsForm_Shown(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;

            CurrentRMCode = DgvMain.Rows[DgvMain.CurrentCell.RowIndex].Cells["RMCode"].Value.ToString();
            CurrentRMName = DgvMain.Rows[DgvMain.CurrentCell.RowIndex].Cells["RMName"].Value.ToString();
            CurrentDocNo = DgvMain.Rows[DgvMain.CurrentCell.RowIndex].Cells["DocumentNo"].Value.ToString();
            this.Text = DgvMain.Rows[DgvMain.CurrentCell.RowIndex].Cells["MCName"].Value.ToString() + " | "+ CurrentRMCode + " | " + CurrentRMName + " | " + CurrentDocNo + " | "+TypeMain + " | "+Convert.ToDouble(DgvMain.Rows[DgvMain.CurrentCell.RowIndex].Cells[DgvMain.CurrentCell.ColumnIndex].Value).ToString("N0");

            dgvPOS.RowHeadersDefaultCellStyle.Font = new Font(dgvPOS.RowHeadersDefaultCellStyle.Font, FontStyle.Regular);
            dgvSemi.RowHeadersDefaultCellStyle.Font = new Font(dgvSemi.RowHeadersDefaultCellStyle.Font, FontStyle.Regular);
            dgvSD.RowHeadersDefaultCellStyle.Font = new Font(dgvSD.RowHeadersDefaultCellStyle.Font, FontStyle.Regular);
            dgvNG.RowHeadersDefaultCellStyle.Font = new Font(dgvNG.RowHeadersDefaultCellStyle.Font, FontStyle.Regular);

            if (TypeMain == "POS")
                ShowPOSType();
            else if (TypeMain == "Semi")
                ShowSemiType();
            else if (TypeMain == "SD")
                ShowSDType();
            else
                ShowNGType();

            LbTotalSelected.Focus();

            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
            {
                EMsg.AlertText = "Something's wrong!\n" + ErrorText;
                EMsg.ShowingMsg();
                this.Close();
            }

        }

        //Method
        private void ShowPOSType()
        {
            tabContrlType.TabPages.RemoveByKey("PageSemi");
            tabContrlType.TabPages.RemoveByKey("PageSD");
            tabContrlType.TabPages.RemoveByKey("PageNG");

            //Add to Dgv
            foreach (DataRow row in dtMain.Rows)
            {
                if (row["ItemCode"].ToString()==CurrentRMCode && row["DocumentNo"].ToString() == CurrentDocNo)
                {
                    dgvPOS.Rows.Add();
                    dgvPOS.Rows[dgvPOS.Rows.Count - 1].HeaderCell.Value = dgvPOS.Rows.Count.ToString();
                    dgvPOS.Rows[dgvPOS.Rows.Count - 1].Cells["LabelNoPOS"].Value = row["LabelNo"].ToString();
                    dgvPOS.Rows[dgvPOS.Rows.Count - 1].Cells["DocumentNoPOS"].Value = row["DocumentNo"].ToString();
                    dgvPOS.Rows[dgvPOS.Rows.Count - 1].Cells["QtyPOS"].Value = Convert.ToDouble(row["Qty"]);
                }
            }

            dgvPOS.ClearSelection();
            dgvPOS.CurrentCell = null;
        }
        private void ShowSemiType()
        {
            tabContrlType.TabPages.RemoveByKey("PagePOS");
            tabContrlType.TabPages.RemoveByKey("PageSD");
            tabContrlType.TabPages.RemoveByKey("PageNG");

            //Add to Dgv
            foreach (DataRow row in dtMain.Rows)
            {
                if (row["LowItemCode"].ToString() == CurrentRMCode && row["DocumentNo"].ToString() == CurrentDocNo)
                {
                    dgvSemi.Rows.Add();
                    dgvSemi.Rows[dgvSemi.Rows.Count - 1].HeaderCell.Value = dgvSemi.Rows.Count.ToString();
                    dgvSemi.Rows[dgvSemi.Rows.Count - 1].Cells["LabelNoSemi"].Value = row["LabelNo"].ToString();
                    dgvSemi.Rows[dgvSemi.Rows.Count - 1].Cells["WIPName"].Value = row["WIPName"].ToString();
                    dgvSemi.Rows[dgvSemi.Rows.Count - 1].Cells["WIPQty"].Value = Convert.ToDouble(row["WIPQty"]);
                    dgvSemi.Rows[dgvSemi.Rows.Count - 1].Cells["DocumentNoSemi"].Value = row["DocumentNo"].ToString();
                    dgvSemi.Rows[dgvSemi.Rows.Count - 1].Cells["POSNoSemi"].Value = row["POSNo"].ToString();
                    dgvSemi.Rows[dgvSemi.Rows.Count - 1].Cells["QtySemi"].Value = Convert.ToDouble(row["TotalQty"]);
                }
            }

            dgvSemi.ClearSelection();
            dgvSemi.CurrentCell = null;
            
        }
        private void ShowSDType()
        {
            tabContrlType.TabPages.RemoveByKey("PagePOS");
            tabContrlType.TabPages.RemoveByKey("PageSemi");
            tabContrlType.TabPages.RemoveByKey("PageNG");

            //Add to Dgv
            foreach (DataRow row in dtMain.Rows)
            {
                if (row["ItemCode"].ToString() == CurrentRMCode && row["DocumentNo"].ToString() == CurrentDocNo)
                {
                    dgvSD.Rows.Add();
                    dgvSD.Rows[dgvSD.Rows.Count - 1].HeaderCell.Value = dgvSD.Rows.Count.ToString();
                    dgvSD.Rows[dgvSD.Rows.Count - 1].Cells["LabelNoSD"].Value = row["LabelNo"].ToString();
                    dgvSD.Rows[dgvSD.Rows.Count - 1].Cells["DocumentNoSD"].Value = row["DocumentNo"].ToString();
                    dgvSD.Rows[dgvSD.Rows.Count - 1].Cells["QtySD"].Value = Convert.ToDouble(row["Qty"]);
                }
            }

            dgvSD.ClearSelection();
            dgvSD.CurrentCell = null;

        }
        private void ShowNGType()
        {
            tabContrlType.TabPages.RemoveByKey("PagePOS");
            tabContrlType.TabPages.RemoveByKey("PageSemi");
            tabContrlType.TabPages.RemoveByKey("PageSD");

            //Add to Dgv
            foreach (DataRow row in dtMain.Rows)
            {
                if (row["RMCode"].ToString() == CurrentRMCode && row["DocumentNo"].ToString() == CurrentDocNo)
                {
                    dgvNG.Rows.Add();
                    dgvNG.Rows[dgvNG.Rows.Count - 1].HeaderCell.Value = dgvNG.Rows.Count.ToString();
                    dgvNG.Rows[dgvNG.Rows.Count - 1].Cells["MCNoNG"].Value = row["MCSeqNo"].ToString();
                    dgvNG.Rows[dgvNG.Rows.Count - 1].Cells["DocumentNoNG"].Value = row["DocumentNo"].ToString();
                    dgvNG.Rows[dgvNG.Rows.Count - 1].Cells["POSNoNG"].Value = row["POSNo"].ToString();
                    dgvNG.Rows[dgvNG.Rows.Count - 1].Cells["QtyNG"].Value = Convert.ToDouble(row["Qty"]);
                }
            }

            dgvNG.ClearSelection();
            dgvNG.CurrentCell = null;

        }

    }
}
