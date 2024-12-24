using MachineDeptApp.InputTransferSemi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.SemiNGReq
{
    public partial class SemiNGReqInputFormConfirm : Form
    {
        SemiNGReqInputForm fgrid;
        public SemiNGReqInputFormConfirm(SemiNGReqInputForm fg)
        {
            InitializeComponent();
            this.fgrid = fg;
            this.txtQty.KeyPress += TxtQty_KeyPress;
        }

        private void TxtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SemiNGReqInputFormSearch_Load(object sender, EventArgs e)
        {
            foreach (DataRow row in fgrid.dtRecLocation.Rows)
            {
                CboLocation.Items.Add(row[0].ToString());
            }
            CboLocation.SelectedIndex = 0;

            txtWipCode.Text = SemiNGReqInputForm.WipCode;
            txtWipName.Text = SemiNGReqInputForm.WipName;
            txtPin.Text = SemiNGReqInputForm.Pin;
            txtWire.Text = SemiNGReqInputForm.Wire;
            txtLength.Text = SemiNGReqInputForm.Length;
            txtQty.Focus();

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtQty.Text.Trim() != "" && CboLocation.Text.Trim() != "")
            {
                try
                {
                    int qty = Convert.ToInt32(txtQty.Text.ToString());
                    int DupRow = 0;

                    for (int i = 0; i < fgrid.dgvInput.Rows.Count; i++)
                    {
                        string Code = txtWipCode.Text.Trim();
                        string Loc = CboLocation.Text;
                        if (fgrid.dgvInput.Rows[i].Cells[0].Value.ToString().Trim() == Code && Convert.ToInt32(fgrid.dgvInput.Rows[i].Cells[2].Value.ToString().Trim()) == qty && fgrid.dgvInput.Rows[i].Cells[6].Value.ToString() == Loc)
                        {
                            DupRow = DupRow + 1;
                        }
                    }
                    //Check In Dgv
                    if (DupRow > 0)
                    {
                        MessageBox.Show("ទិន្នន័យនេះអ្នកទើបតែបញ្ចូលរួចមុននេះ​ ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else
                    {
                        if (txtQty.Text.Trim().ToString() != "0")
                        {
                            int FoundLoc = 0;
                            for (int i = 0; i < fgrid.dtRecLocation.Rows.Count; i++)
                            {
                                if (CboLocation.Text == fgrid.dtRecLocation.Rows[i][0].ToString())
                                {
                                    FoundLoc=FoundLoc + 1;
                                }
                            }

                            if (FoundLoc > 0)
                            {
                                fgrid.dgvInput.Rows.Add(txtWipCode.Text, txtWipName.Text, qty, txtPin.Text, txtWire.Text, txtLength.Text, CboLocation.Text);
                                fgrid.dgvInput.ClearSelection();
                                fgrid.dgvWipCode.Rows.Clear();
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("មិនមាន <ផ្នែកទទួល> ដែលអ្នកបានបញ្ចូលទេ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            }

                        }
                        else
                        {
                            MessageBox.Show("ចំនួនមិនអាចស្មើ 0 បានទេ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ចំនួនត្រូវតែជាលេខ !" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("សូមបំពេញប្រអប់ដែលមានផ្ទៃក្រោយព៌ណទឹកក្រូចទាំងអស់ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }
    }
}
