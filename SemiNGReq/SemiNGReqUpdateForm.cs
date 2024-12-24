using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.SemiNGReq
{
    public partial class SemiNGReqUpdateForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SemiNGReqSearchForm fgrid;
        string[] RecLoc;

        public SemiNGReqUpdateForm(SemiNGReqSearchForm fg)
        {
            InitializeComponent();
            this.cnn.Connection();
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

        private void SemiNGReqUpdateForm_Load(object sender, EventArgs e)
        {
            RecLoc = new string[] {"WIP", "Assy" };
            for (int i = 0; i < RecLoc.Length; i++)
            {
                CboLocation.Items.Add(RecLoc[i]);
            }
            CboLocation.SelectedIndex= 0;

            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT WipCode, WipDes, Remarks1, Remarks2, Remarks3, Qty, RecSection FROM tbSemiNGReq "+
                                                                                  "INNER JOIN tbMasterItem ON tbMasterItem.ItemCode = tbSemiNGReq.WipCode "+
                                                                                  "WHERE SysNo = '"+SemiNGReqSearchForm.SysNoForNextForm+"'", cnn.con);
                DataTable table = new DataTable();
                da.Fill(table);
                txtWipCode.Text = table.Rows[0][0].ToString();
                txtWipName.Text = table.Rows[0][1].ToString();
                txtPin.Text = table.Rows[0][2].ToString();
                txtWire.Text = table.Rows[0][3].ToString();
                txtLength.Text = table.Rows[0][4].ToString();
                txtQty.Text = table.Rows[0][5].ToString();
                CboLocation.Text = table.Rows[0][6].ToString();
                txtQty.Focus();
                txtQty.SelectAll();
            }
            catch
            {
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtQty.Text.Trim() != "" && CboLocation.Text.Trim() != "")
            {
                try
                {
                    int qty = Convert.ToInt32(txtQty.Text.ToString());
                    if (txtQty.Text.Trim().ToString() != "0")
                    {
                        int FoundLoc = 0;
                        for (int i = 0; i < RecLoc.Length ; i++)
                        {
                            if (CboLocation.Text == RecLoc[i])
                            {
                                FoundLoc = FoundLoc + 1;
                            }
                        }

                        if (FoundLoc > 0)
                        {
                            DialogResult DLR = MessageBox.Show("តើអ្នកចង់អាប់ដេតទិន្នន័យនេះមែនដែរ ឬទេ ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (DLR == DialogResult.Yes)
                            {
                                DateTime UpDate = DateTime.Now;
                                try
                                {
                                    cnn.con.Open();
                                    string query = "UPDATE tbSemiNGReq SET " +
                                                        "Qty="+ qty + "," +
                                                        "RecSection='"+CboLocation.Text+"'," +
                                                        "UpdateDate='" + UpDate + "'," +
                                                        "UpdateBy=N'" + MenuFormV2.UserForNextForm + "'" +
                                                        "WHERE SysNo = '" + SemiNGReqSearchForm.SysNoForNextForm + "';";
                                    SqlCommand cmd = new SqlCommand(query, cnn.con);
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("អាប់ដេតទិន្នន័យរួចរាល់ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    fgrid.dgvAllData.Rows[fgrid.dgvAllData.CurrentCell.RowIndex].Cells[6].Value = qty;
                                    fgrid.dgvAllData.Rows[fgrid.dgvAllData.CurrentCell.RowIndex].Cells[7].Value = CboLocation.Text;
                                    fgrid.dgvAllData.Rows[fgrid.dgvAllData.CurrentCell.RowIndex].Cells[10].Value = UpDate;
                                    fgrid.dgvAllData.Rows[fgrid.dgvAllData.CurrentCell.RowIndex].Cells[11].Value = MenuFormV2.UserForNextForm;
                                    for (int i = 0; i < fgrid.dtPrint.Rows.Count; i++)
                                    {
                                        if (fgrid.dtPrint.Rows[i][0].ToString() == SemiNGReqSearchForm.SysNoForNextForm)
                                        {
                                            fgrid.dtPrint.Rows[i][6] = qty;
                                            fgrid.dtPrint.Rows[i][7] = CboLocation.Text;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញជាមុន ! \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                cnn.con.Close();
                                this.Close();
                            }
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
