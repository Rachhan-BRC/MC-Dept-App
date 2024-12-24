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
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SDReceiveForm fgrid;
        DataTable dtOBSTrans;
        public static string ID;
        public static string Password;
        int FoundAbnormal;
        int width;
        int height;

        public SDReceiveFormConfirm(SDReceiveForm fg)
        {
            InitializeComponent();
            this.fgrid = fg;
            this.cnnOBS.Connection();
            this.dgvConsumption.CellFormatting += DgvConsumption_CellFormatting;
            this.dgvConsumption.SelectionChanged += DgvConsumption_SelectionChanged;
            this.dgvTransfered.SelectionChanged += DgvTransfered_SelectionChanged;
            this.btnOK.Click += BtnOK_Click;
            this.btnEdit.Click += BtnEdit_Click;
            this.Load += SDReceiveFormConfirm_Load;
            this.btnSwap.Click += BtnSwap_Click;
        }

        private void BtnSwap_Click(object sender, EventArgs e)
        {
            int dgvConsSelectedRow = dgvConsumption.CurrentCell.RowIndex;
            int dgvTransSelectedRow = dgvTransfered.CurrentCell.RowIndex;
            this.dgvConsumption.Rows[dgvConsSelectedRow].Cells[0].Value = dgvTransfered.Rows[dgvConsSelectedRow].Cells[0].Value.ToString();
            this.dgvConsumption.Rows[dgvConsSelectedRow].Cells[1].Value = dgvTransfered.Rows[dgvConsSelectedRow].Cells[1].Value.ToString();
            this.dgvConsumption.Rows[dgvConsSelectedRow].Cells[4].Value = Convert.ToDouble(dgvTransfered.Rows[dgvConsSelectedRow].Cells[2].Value.ToString());
            this.dgvTransfered.Rows.RemoveAt(dgvTransSelectedRow);
            dgvConsumption.ClearSelection();
            dgvTransfered.ClearSelection();
            CheckButtonSwap();

        }

        private void SDReceiveFormConfirm_Load(object sender, EventArgs e)
        {
            LbPOSNo.Text = SDReceiveForm.POSNo;
            LbShipDate.Text = SDReceiveForm.ShipDate;
            LbWipCode.Text = SDReceiveForm.WIPCode;
            LbWipName.Text = SDReceiveForm.WIPName;
            LbQty.Text = Convert.ToDouble(SDReceiveForm.Qty).ToString("N0");
            this.GrbOBSTransfer.Visible = false;
            this.splitter1.Visible = false;
            width = GrbBOM.Width;
            height = GrbBOM.Height;
            this.GrbBOM.Size = new Size(width + 380, height);
            ReloadBOM();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            int Width = GrbBOM.Width;
            for (int i = 0; i < 1000; i++)
            {
                if (Width == width)
                {
                    break;
                }
                else
                {
                    Width = Width - 1;
                    this.GrbBOM.Size = new Size(Width, height);
                }
            }
            this.GrbBOM.Size = new Size(width, height);
            this.GrbOBSTransfer.Visible = true;
            this.splitter1.Visible = true;
            CheckOBSTransfer();
            Calc();

        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            FoundAbnormal = 0;
            foreach (DataGridViewRow row in dgvConsumption.Rows)
            {
                if (Convert.ToDouble(row.Cells[3].Value.ToString()) != Convert.ToDouble(row.Cells[4].Value.ToString()))
                {
                    FoundAbnormal++;
                }
            }
            if (FoundAbnormal == 0)
            {
                InsertOK();
                this.Close();
            }
            else
            {
                DialogResult DLS = MessageBox.Show("មានវត្ថុធាតុដើមដែល Kitting Room វេរឱ្យមិនទាន់គ្រប់!\nតើអ្នកចង់បន្ដឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (DLS == DialogResult.Yes)
                {
                    ID = "";
                    Password = "";
                    SDReceiveFormConfirmUser Srfcu = new SDReceiveFormConfirmUser();
                    Srfcu.ShowDialog();
                    if (ID.Trim() != "" && Password.Trim() != "")
                    {
                        InsertOK();
                        this.Close();
                    }
                }
            }
        }

        private void DgvTransfered_SelectionChanged(object sender, EventArgs e)
        {
            CheckButtonSwap();
        }

        private void DgvConsumption_SelectionChanged(object sender, EventArgs e)
        {
            CheckButtonSwap();
        }

        private void DgvConsumption_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 4 && e.Value.ToString() != "")
            {
                if (Convert.ToDouble(dgvConsumption[3, e.RowIndex].Value.ToString()) == Convert.ToDouble(dgvConsumption[4, e.RowIndex].Value.ToString()))
                {
                    e.CellStyle.ForeColor = Color.Blue;
                    e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambong", 9, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambong", 9, FontStyle.Bold);
                }

            }

        }

        private void CheckButtonSwap()
        {
            btnSwap.Enabled = false;
            btnSwap.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
            if (dgvConsumption.SelectedRows.Count > 0 && dgvTransfered.SelectedRows.Count > 0)
            {
                btnSwap.Enabled = true;
                btnSwap.BackColor = Color.White;
            }
        }

        private void ReloadBOM()
        {
            dgvConsumption.Rows.Clear();
            foreach (DataRow row in fgrid.dtPOSConsump.Rows)
            {
                dgvConsumption.Rows.Add(row[0], row[1], Convert.ToDouble(row[2].ToString()), Convert.ToDouble(row[3].ToString()));
            }
            CheckOBSTransfer();
            Calc();
            dgvConsumption.ClearSelection();
        }

        private void CheckOBSTransfer()
        {
            dgvTransfered.Rows.Clear();
            try
            {
                //Check BOM
                SqlDataAdapter sda = new SqlDataAdapter("SELECT prgalltransaction.ItemCode, mstitem.ItemName, SUM(prgalltransaction.RealValueQty) as TotalQty FROM prgalltransaction " +
                                                                                    "INNER JOIN mstitem ON mstitem.ItemCode = prgalltransaction.ItemCode " +
                                                                                    "INNER JOIN mstgri ON mstgri.GRICode = prgalltransaction.GRICode " +
                                                                                    "WHERE prgalltransaction.LocCode = 'MC1' AND mstgri.GRIName LIKE 'Transfer%' AND prgalltransaction.Remark = '" + LbPOSNo.Text + "' " +
                                                                                    "GROUP BY prgalltransaction.ItemCode, mstitem.ItemName", cnnOBS.conOBS);
                dtOBSTrans = new DataTable();
                sda.Fill(dtOBSTrans);
                foreach (DataRow row in dtOBSTrans.Rows)
                {
                    if (Convert.ToDouble(row[2].ToString()) > 0)
                    {
                        dgvTransfered.Rows.Add(row[0], row[1], Convert.ToDouble(row[2].ToString()));
                    }
                }
                dgvTransfered.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ការភ្ជាប់បណ្ដាញមានបញ្ហា!\nសូមពិនិត្យមើល Wifi/Internet!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void Calc()
        {
            //Cal TransQty
            foreach (DataGridViewRow DgvRow in dgvConsumption.Rows)
            {
                double Trans = 0;
                foreach (DataRow row in dtOBSTrans.Rows)
                {
                    if (DgvRow.Cells[0].Value.ToString() == row[0].ToString())
                    {
                        Trans = Convert.ToDouble(row[2].ToString());
                        break;
                    }
                }
                DgvRow.Cells[4].Value = Trans;
            }
            //Remove from Swap Item
            for (int i = dgvTransfered.Rows.Count - 1; i > -1; i--)
            {
                foreach (DataGridViewRow DgvRow in dgvConsumption.Rows)
                {
                    if (dgvTransfered.Rows[i].Cells[0].Value.ToString() == DgvRow.Cells[0].Value.ToString())
                    {
                        dgvTransfered.Rows.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private void InsertOK()
        {
            if (FoundAbnormal > 0)
            {
                fgrid.dgvScanned.Rows.Add(LbPOSNo.Text, LbWipCode.Text, LbWipName.Text, Convert.ToDouble(LbQty.Text), Convert.ToDateTime(LbShipDate.Text), false);
            }
            else
            {
                fgrid.dgvScanned.Rows.Add(LbPOSNo.Text, LbWipCode.Text, LbWipName.Text, Convert.ToDouble(LbQty.Text), Convert.ToDateTime(LbShipDate.Text), true);
            }
            foreach (DataGridViewRow row in this.dgvConsumption.Rows)
            {
                fgrid.dtSQLSaving.Rows.Add(LbPOSNo.Text, row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString(), row.Cells[4].Value.ToString());
            }
        }

    }
}
