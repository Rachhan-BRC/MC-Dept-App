using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.Inventory
{
    public partial class InventoryCombineDetailsForm : Form
    {
        InventoryCombineForm fgrid;
        public InventoryCombineDetailsForm(InventoryCombineForm fg)
        {
            InitializeComponent();
            this.fgrid = fg;
            this.Load += InventoryCombineDetailsForm_Load;
        }

        private void InventoryCombineDetailsForm_Load(object sender, EventArgs e)
        {
            dgvRMofSemi.Columns[0].Frozen=true;
            dgvRMofSemi.Columns[1].Frozen = true;
            dgvRMofSemi.Columns[2].Frozen = true;
            this.Text = InventoryCombineForm.CodeSelected + "\t" + InventoryCombineForm.NameSelected + "\t" + InventoryCombineForm.QtySelected;
            //RM Inprocess
            foreach (DataRow row in fgrid.dtInventorySumByLocCode.Rows)
            {
                if (row["LocName"].ToString() == "MC Inprocess")
                {
                    if (row["ItemCode"].ToString() == InventoryCombineForm.CodeSelected)
                    {
                        dgvRM.Rows.Add(row["ItemCode"].ToString(), row["ItemName"].ToString(), Convert.ToDouble(row["TotalQty"].ToString()));
                    }
                }
            }
            dgvRM.ClearSelection();

            //RM Inprocess of Semi
            foreach (DataRow row in fgrid.dtOBSBOM.Rows)
            {
                if (row["LowItemCode"].ToString() == InventoryCombineForm.CodeSelected)
                {
                    double Qty = Convert.ToDouble(row["UsageQty"].ToString());
                    string WipCode = row["UpItemCode"].ToString();
                    string WipName = "";
                    foreach (DataRow rowOBS in fgrid.dtOBSMstItem.Rows)
                    {
                        if (rowOBS["ItemCode"].ToString() == WipCode)
                        {
                            WipName = rowOBS["ItemName"].ToString();
                            break;
                        }
                    }
                    double WipQty = 0;
                    foreach (DataRow rowAll in fgrid.dtInventorySumByLocCode.Rows)
                    {
                        if (rowAll["LocName"].ToString() == "MC Inprocess")
                        {
                            if (rowAll["ItemCode"].ToString() == WipCode)
                            {
                                WipQty = Convert.ToDouble(rowAll["TotalQty"].ToString());
                                break;
                            }
                        }
                        
                    }
                    dgvRMofSemi.Rows.Add(InventoryCombineForm.CodeSelected, InventoryCombineForm.NameSelected, Qty,WipCode, WipName, WipQty);
                }
            }
            dgvRMofSemi.ClearSelection();
        }

    }
}
