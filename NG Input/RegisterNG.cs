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

namespace MachineDeptApp.NG_Input
{
    public partial class RegisterNG : Form
    {
        SQLConnect conn = new SQLConnect();
        DataTable dtPoscNg = new DataTable();
        public RegisterNG()
        {
            
            this.conn.Connection();
            InitializeComponent();
            tbPosScan.KeyDown += TbPosScan_KeyDown;
            btnAddRegisterNG.Click += BtnAddRegisterNG_Click;
            tbPosCQty.KeyPress += TbPosCQty_KeyPress;
            btnSave.Click += BtnSave_Click;
            btnRemove.Click += BtnRemove_Click;
            btnToAdd.Click += BtnToAdd_Click;
            btnCancel.Click += BtnCancel_Click;
           tbPosPQty.ReadOnly = true;
            tbWipCode.ReadOnly = true;
            tbDeliveryDate.ReadOnly = true;
            tbDoc.DropDownStyle = ComboBoxStyle.DropDownList;
            
            
        }

        private void HideEditTabContrl()
        {
            tcRegisterNg.Visible = false;
            groupBoxRegisterList.Height = groupBoxRegisterList.Height + groupBoxRegisterList.Height;
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            HideEditTabContrl();
        }

        private void ShowEditTabContrl()
        {
            groupBoxRegisterList.Height = groupBoxRegisterList.Height - tcRegisterNg.Height;
            tcRegisterNg.Visible = true;
        }
        private void BtnToAdd_Click(object sender, EventArgs e)
        {
            ShowEditTabContrl();
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (dgvNgRegisterList.Rows.Count > 0 && dgvNgRegisterList.CurrentCell.RowIndex > -1)
            {
                DialogResult dlr = MessageBox.Show("Do you want to delete?", "Stauts Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlr == DialogResult.Yes)
                {
                    dgvNgRegisterList.Rows.RemoveAt(dgvNgRegisterList.CurrentCell.RowIndex);
                }
                }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (dgvNgRegisterList.Rows.Count > 0) 
            {
                DialogResult dlr = MessageBox.Show("Do you want to Save?", "Stauts Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlr == DialogResult.Yes)
                {
                    try
                    {
                        conn.con.Open();
                        for (global::System.Int32 i = 0; i < dgvNgRegisterList.Rows.Count; i++)
                        {
                            SqlCommand cmd = new SqlCommand("INSERT INTO tbPOSDetailofMC (FGCode, PosPNo, PosPQty, PosPDelDate, WIPCode, PosCNo, SemiQty, PosCQty, PosCRemainQty, PosCResultQty, PosCStatus, Remarks, DocNo, KITPromise, LabelStatus, RegDate, RegBy, UpdateDate, UpdateBy, PrintStatus) " +
                                                                                "VALUES (@Fc, @Ppn, @Ppqty, @Ppd, @Wc, @Pcn, @Sqty, @Pcqty, @Pcr, @Pcrs, @Pcs, @Rm, @Dn, @Kpm, @Ls, @Rd, @Rb, @Ud, @Ub, @Ps)", conn.con);
                            string todayPrefix = dgvNgRegisterList.Rows[i].Cells["DocNo"].Value.ToString() + DateTime.Now.ToString("yyMMdd"); // Get "yyMMdd"
                            string query = "SELECT MAX(DocNo) FROM tbPOSDetailofMC WHERE DocNo LIKE @prefix + '%'";
                            string DocNo;
                            SqlCommand sqlCommand = new SqlCommand(query, conn.con);
                            sqlCommand.Parameters.AddWithValue("@prefix", todayPrefix);
                            object result = sqlCommand.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                MessageBox.Show(result.ToString());
                                // Extract last number part
                                string lastID = result.ToString();
                                int lastNumber = int.Parse(lastID.Substring(lastID.Length - 3));
                                lastNumber++; // Increment 
                                DocNo = todayPrefix + lastNumber.ToString("D3"); // Format back to "yyMMddNNN"
                            }
                            else
                            {
                                DocNo = todayPrefix + "000";  // First entry of the day
                            }
                            DateTime Now = DateTime.Now;
                            cmd.Parameters.AddWithValue("@Fc", dgvNgRegisterList.Rows[i].Cells["FGCode"].Value.ToString());
                            cmd.Parameters.AddWithValue("@Ppn", dgvNgRegisterList.Rows[i].Cells["PosPNo"].Value.ToString());
                            cmd.Parameters.AddWithValue("@Ppqty", dgvNgRegisterList.Rows[i].Cells["PosPQty"].Value.ToString());
                            cmd.Parameters.AddWithValue("@Ppd", Convert.ToDateTime(dgvNgRegisterList.Rows[i].Cells["PosPDelDate"].Value));
                            cmd.Parameters.AddWithValue("@Wc", dgvNgRegisterList.Rows[i].Cells["WIPCode"].Value.ToString());
                            cmd.Parameters.AddWithValue("@Pcn", dgvNgRegisterList.Rows[i].Cells["PosCNo"].Value.ToString());
                            cmd.Parameters.AddWithValue("@Sqty", dgvNgRegisterList.Rows[i].Cells["SemiQty"].Value.ToString());
                            cmd.Parameters.AddWithValue("@Pcqty", dgvNgRegisterList.Rows[i].Cells["PosCQty"].Value.ToString());
                            cmd.Parameters.AddWithValue("@Pcr", dgvNgRegisterList.Rows[i].Cells["PosCRemainQty"].Value.ToString());
                            cmd.Parameters.AddWithValue("@Pcrs", "0");
                            cmd.Parameters.AddWithValue("@Pcs", "0");
                            cmd.Parameters.AddWithValue("@Rm", "");
                            cmd.Parameters.AddWithValue("@Dn", DocNo.ToString());
                            cmd.Parameters.AddWithValue("@Kpm", dgvNgRegisterList.Rows[i].Cells["KITPromise"].Value.ToString());
                            cmd.Parameters.AddWithValue("@Ls", "NOT YET");
                            cmd.Parameters.AddWithValue("@Rd", Now);
                            cmd.Parameters.AddWithValue("@Rb", MenuFormV2.UserForNextForm);
                            cmd.Parameters.AddWithValue("@Ud", Now);
                            cmd.Parameters.AddWithValue("@Ub", MenuFormV2.UserForNextForm);
                            cmd.Parameters.AddWithValue("@Ps", "0");
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("POSCNo. already Have !", "POSNO. Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    conn.con.Close();
                    dgvNgRegisterList.Rows.Clear();
                }
            }

        }

        private void TbPosCQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Block non-numeric input
            }
        }

        private void BtnAddRegisterNG_Click(object sender, EventArgs e)
        {
            if (dtPoscNg.Rows.Count > 0 && tbDoc.Text.Trim().ToString() != "" && tbPosCQty.Text.Trim().ToString() != "" )
            {
                try
                {
                    conn.con.Open();
                    foreach (DataRow row in dtPoscNg.Rows)
                    {
                        dgvNgRegisterList.Rows.Add();
                        dgvNgRegisterList.Rows[dgvNgRegisterList.Rows.Count - 1].Cells["FGCode"].Value = row["FGCode"].ToString();
                        dgvNgRegisterList.Rows[dgvNgRegisterList.Rows.Count - 1].Cells["PosPNo"].Value = row["PosPNo"].ToString();
                        dgvNgRegisterList.Rows[dgvNgRegisterList.Rows.Count - 1].Cells["PosPQty"].Value = row["PosPQty"].ToString();
                        dgvNgRegisterList.Rows[dgvNgRegisterList.Rows.Count - 1].Cells["PosPDelDate"].Value = row["PosPDelDate"].ToString();
                        dgvNgRegisterList.Rows[dgvNgRegisterList.Rows.Count - 1].Cells["WIPCode"].Value = row["WIPCode"].ToString();
                        dgvNgRegisterList.Rows[dgvNgRegisterList.Rows.Count - 1].Cells["PosCNo"].Value = row["PosCNo"].ToString() + "NG";
                        dgvNgRegisterList.Rows[dgvNgRegisterList.Rows.Count - 1].Cells["SemiQty"].Value = row["SemiQty"].ToString();
                        dgvNgRegisterList.Rows[dgvNgRegisterList.Rows.Count - 1].Cells["PosCQty"].Value = tbPosCQty.Text.Trim().ToString();
                        dgvNgRegisterList.Rows[dgvNgRegisterList.Rows.Count - 1].Cells["PosCRemainQty"].Value = tbPosCQty.Text.Trim().ToString();
                        dgvNgRegisterList.Rows[dgvNgRegisterList.Rows.Count - 1].Cells["PosCResultQty"].Value = "0";
                        dgvNgRegisterList.Rows[dgvNgRegisterList.Rows.Count - 1].Cells["PosCStatus"].Value = "0";
                        dgvNgRegisterList.Rows[dgvNgRegisterList.Rows.Count - 1].Cells["Remarks"].Value = "";
                        dgvNgRegisterList.Rows[dgvNgRegisterList.Rows.Count - 1].Cells["DocNo"].Value = tbDoc.Text.Trim().ToString();
                        dgvNgRegisterList.Rows[dgvNgRegisterList.Rows.Count - 1].Cells["KITPromise"].Value = row["KITPromise"].ToString();
                        dgvNgRegisterList.Rows[dgvNgRegisterList.Rows.Count - 1].Cells["LabelStatus"].Value = "NOT YET";
                        dgvNgRegisterList.Rows[dgvNgRegisterList.Rows.Count - 1].Cells["PrintStatus"].Value = "0";
                       

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                conn.con.Close();
                tbDeliveryDate.Text = "";
                tbDoc.Text = "";
                tbPosCQty.Text = "";
                tbPosPQty.Text = "";
                tbPosScan.Text = "";
                tbWipCode.Text = "";
            }
        }

        private void TbPosScan_KeyDown(object sender, KeyEventArgs e)
        {
            if (tbPosScan.Text.Trim() != "")
            {
                try
                {
                    dtPoscNg.Clear();
                String query = "Select FGCode, PosPNo, PosPQty, PosPDelDate, WIPCode, PosCNo, SemiQty, PosCQty, PosCRemainQty, PosCResultQty, PosCStatus, Remarks, DocNo, KITPromise, LabelStatus, RegDate, RegBy, UpdateDate, UpdateBy, PrintStatus  from tbPOSDetailofMC where PosCNo  = '"
                   + tbPosScan.Text.ToString() + "'";
                    SqlDataAdapter dtAdapter = new SqlDataAdapter(query, conn.con);
                    
                    dtAdapter.Fill(dtPoscNg);
                    foreach (DataRow row in dtPoscNg.Rows)
                    {
                        tbPosPQty.Text = row["PosPQty"].ToString();
                        tbWipCode.Text = row["WIPCode"].ToString();
                        //DateTime deliveryDate = Convert.ToDateTime(row["PosPDelDate"]);
                        tbDeliveryDate.Text = Convert.ToDateTime(row["PosPDelDate"]).ToString("dd-MM-yyyy");

                    }
                } catch ( Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
        }
    }
}
