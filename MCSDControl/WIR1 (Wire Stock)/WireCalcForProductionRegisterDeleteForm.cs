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
    public partial class WireCalcForProductionRegisterDeleteForm : Form
    {
        ErrorMsgClass EMsg = new ErrorMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        WireCalcForProductionRegisterForm fgrid;

        public WireCalcForProductionRegisterDeleteForm(WireCalcForProductionRegisterForm fg)
        {
            InitializeComponent();
            this.fgrid = fg;
            this.txtBC.KeyDown += TxtBC_KeyDown;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnDelete.EnabledChanged += BtnDelete_EnabledChanged;
        }

        private void BtnDelete_EnabledChanged(object sender, EventArgs e)
        {
            if (btnDelete.Enabled == true)
            {
                btnDelete.BackColor = Color.Red;
                btnDelete.ForeColor = Color.White;
            }
            else
            {
                btnDelete.BackColor = Color.FromArgb(224, 224, 224) ;
                btnDelete.ForeColor = Color.FromArgb(64, 64, 64);
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (LbBobbinCode.Text.Trim() != "" && LbQty.Text.Trim() != "")
            {
                Cursor = Cursors.WaitCursor;

                //Remove
                foreach (DataGridViewRow row in fgrid.dgvInputted.Rows)
                {
                    if (row.Cells["BobbinCodeInputted"].Value.ToString()==LbBobbinCode.Text)
                    {
                        fgrid.dtAvailableBobbin.Rows.Add();
                        fgrid.dtAvailableBobbin.Rows[fgrid.dtAvailableBobbin.Rows.Count - 1]["BobbinSysNo"] = row.Cells["BobbinCodeInputted"].Value.ToString();
                        fgrid.dtAvailableBobbin.Rows[fgrid.dtAvailableBobbin.Rows.Count - 1]["Remain_W"] = Convert.ToDouble(row.Cells["WeightInputted"].Value);
                        fgrid.dtAvailableBobbin.Rows[fgrid.dtAvailableBobbin.Rows.Count - 1]["Remain_L"] = Convert.ToDouble(row.Cells["LengthInputted"].Value);
                        fgrid.dtAvailableBobbin.AcceptChanges();

                        fgrid.dgvInputted.Rows.Remove(row);
                        break;
                    }
                }
                
                //Assign No
                foreach (DataGridViewRow row in fgrid.dgvInputted.Rows)
                {
                    row.HeaderCell.Value = (row.Index + 1).ToString();
                }

                btnDelete.Enabled = false;
                LbBobbinCode.Text = "";
                LbQty.Text = "";

                Cursor = Cursors.Default;
                txtBC.Focus();
            }
        }
        private void TxtBC_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                string BC = txtBC.Text;
                LbBobbinCode.Text = "";
                LbQty.Text = "";
                foreach (DataGridViewRow row in fgrid.dgvInputted.Rows)
                {
                    if (row.Cells["BobbinCodeInputted"].Value.ToString()==BC)
                    {
                        LbBobbinCode.Text = row.Cells["BobbinCodeInputted"].Value.ToString();
                        LbQty.Text = Convert.ToDouble(row.Cells["LengthInputted"].Value).ToString("N0");
                        break;
                    }
                }

                if (LbBobbinCode.Text.Trim() != "" && LbQty.Text.Trim() != "")
                {
                    btnDelete.Enabled = true;
                    txtBC.Text = "";
                    btnDelete.Focus();
                }                    
                else
                {
                    WMsg.WarningText="ទិន្នន័យដែលបានបញ្ចូលរួចគឺគ្មានទិន្នន័យឡាប៊ែលនេះទេ!";
                    WMsg.ShowingMsg();
                    btnDelete.Enabled = false;
                    txtBC.Focus();
                    txtBC.SelectAll();
                }

            }
        }

    }
}
