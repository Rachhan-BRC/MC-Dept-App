using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace MachineDeptApp
{
    public partial class transaction : Form
    {
        SQLConnect con = new SQLConnect();
        string dept = "MC";
        public transaction()
        {
            con.Connection();
            InitializeComponent();
            this.txtcode.TextChanged += Txtcode_TextChanged;
            this.txtPname.TextChanged += TxtPname_TextChanged;
            this.txtPno.TextChanged += TxtPno_TextChanged;
            this.btnSearch.Click += BtnSearch_Click;
            this.dgvTTL.CellClick += DgvTTL_CellClick;
            this.btnDelete.Click += BtnDelete_Click;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            string Code = dgvTTL.Rows[dgvTTL.CurrentCell.RowIndex].Cells["Code"].Value.ToString();
            string transNo = dgvTTL.Rows[dgvTTL.CurrentCell.RowIndex].Cells["TranNo"].Value.ToString();
            double stockIn = Convert.ToDouble(dgvTTL.Rows[dgvTTL.CurrentCell.RowIndex].Cells["stockin"].Value);
            double stockOut = Convert.ToDouble(dgvTTL.Rows[dgvTTL.CurrentCell.RowIndex].Cells["stockout"].Value);
            DialogResult result = MessageBox.Show("Are you sure you want to delete Transaction No: " + transNo + "?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }
            else
            {
                DataTable dtBal = new DataTable();
                DataTable dtPono = new DataTable();
                DataTable dtReq = new DataTable();
                try
                {
                    con.con.Open();
                    string query = "SELECT Code, SUM(Stock_Value) AS TotalStock FROM SparePartTrans where Dept =  '"+dept+"' AND Code =  '"+Code+"' GROUP BY Code";
                    SqlDataAdapter sdabal = new SqlDataAdapter(query, con.con);
                    sdabal.Fill(dtBal);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while selecting compare" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.con.Close();
                try
                {
                    con.con.Open();
                    string query = "SELECT *  FROM SparePartTrans where Dept =  '"+dept+"' AND TransNo =  '" + transNo +"'";
                    SqlDataAdapter sdasl = new SqlDataAdapter(query, con.con);
                    sdasl.Fill(dtPono);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while selecting compare" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.con.Close();
                if (stockIn > 0)
                {
                    try
                    {
                        double bal = dtBal.Rows.Count > 0 ? Convert.ToDouble(dtBal.Rows[0]["TotalStock"]) : 0;
                        if (stockIn > bal )
                        {
                            MessageBox.Show("Cannot delete this transaction as beause it bigger than balance", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            string pono = (dtPono.Rows.Count > 0) ? dtPono.Rows[0]["PO_No"].ToString() : null;
                            if (!string.IsNullOrWhiteSpace(pono))
                            {
                                try
                                {
                                    con.con.Open();
                                    string query = "SELECT * FROM MCSparePartRequest where Code = '" + Code + "' AND PO_No = '" + pono + "'";
                                    SqlDataAdapter sdarq = new SqlDataAdapter(query, con.con);
                                    sdarq.Fill(dtReq);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error while selecting upodate" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                              con.con.Close();
                                try
                                {
                                    con.con.Open();
                                    double rec = dtReq.Rows.Count > 0 ? Convert.ToDouble(dtReq.Rows[0]["ReceiveQTY"]) - stockIn : 0;
                                    double balQ = dtReq.Rows.Count > 0 ? Convert.ToDouble(dtReq.Rows[0]["Balance"]) + stockIn : 0;
                                    double amount = dtReq.Rows.Count > 0 ? Convert.ToDouble(dtReq.Rows[0]["RemainAmount"]) + (stockIn * Convert.ToDouble(dtReq.Rows[0]["UnitPrice"])) : 0;
                                    string status = (balQ == 0) ? "Completed" : "Waiting for PO Update";
                                    string remark = "Updated: " + DateTime.Now.ToString("dd-MM-yyyy");
                                    DateTime update = DateTime.Now;

                                    string updatePOQuery = "UPDATE MCSparePartRequest SET ReceiveQTY =  @rec, Balance = @bal, RemainAmount = @amount, Order_State = @status, Remark = @remark" +
                                                                       ", UpdateDate = @update WHERE PO_No = @PONo AND Code = @Code";
                                    SqlCommand cmd = new SqlCommand(updatePOQuery, con.con);
                                    cmd.Parameters.AddWithValue("@rec", rec);
                                    cmd.Parameters.AddWithValue("@bal", balQ);
                                    cmd.Parameters.AddWithValue("@amount", amount);
                                    cmd.Parameters.AddWithValue("@status", status);
                                    cmd.Parameters.AddWithValue("@remark", remark);
                                    cmd.Parameters.AddWithValue("@update", update);
                                    cmd.Parameters.AddWithValue("@PONo", pono);
                                    cmd.Parameters.AddWithValue("@Code", Code);
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error while updating" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                con.con.Close();
                            }
                            try
                            {
                                con.con.Open();
                                string deleteQuery = "DELETE FROM SparePartTrans WHERE TransNo = @TransNo AND Dept =  '"+dept+"'";
                                SqlCommand cmdDelete = new SqlCommand(deleteQuery, con.con);
                                cmdDelete.Parameters.AddWithValue("@TransNo", transNo);
                                cmdDelete.ExecuteNonQuery();
                                MessageBox.Show("Transaction deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("An error occurred while attempting to delete the transaction." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            con.con.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while deleting StockIN" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        con.con.Open();
                        string deleteQuery = "DELETE FROM SparePartTrans WHERE TransNo = @TransNo AND Dept =  '" + dept + "'";
                        SqlCommand cmdDelete = new SqlCommand(deleteQuery, con.con);
                        cmdDelete.Parameters.AddWithValue("@TransNo", transNo);
                        cmdDelete.ExecuteNonQuery();
                        MessageBox.Show("Transaction deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while attempting to delete the transaction." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    con.con.Close();
                }
               
            }
            con.con.Close();
            Cursor = Cursors.Default;
            btnSearch.PerformClick();
        }

        private void DgvTTL_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                    btnDelete.Enabled = true;
                    btnDelete.BringToFront();       
            }
        }

        private void TxtPno_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPno.Text))
            {
                chkPno.Checked = false;
            }
            else
            {
                chkPno.Checked = true;
            }
            btnSearch.PerformClick();
        }

        private void TxtPname_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPname.Text))
            {
                chkPname.Checked = false;
                chkPno.Checked = false;
            }
            else
            {
                chkPname.Checked = true;
                chkPno.Checked = true;
            }
            btnSearch.PerformClick();
        }

        private void Txtcode_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtcode.Text))
            {
                chkcode.Checked = false;
            }
            else
            {
                chkcode.Checked = true;
            }
            btnSearch.PerformClick();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            dgvTTL.Rows.Clear();
            Cursor = Cursors.WaitCursor;
            DataTable dt = new DataTable();
            try
            {
                DataTable cond = new DataTable();
                cond.Columns.Add("Value");
                if (chkcode.Checked == true)
                {
                    string val = txtcode.Text;
                    if (!string.IsNullOrEmpty(val))
                    {
                        // Correct placement of % wildcards
                        cond.Rows.Add($"Code LIKE '%{val}%'");
                    }
                }
                if (chkPname.Checked == true)
                {
                    string val = txtPname.Text;
                    if (!string.IsNullOrEmpty(val))
                    {
                        // Partial match for Part_Name
                        cond.Rows.Add($"Part_Name LIKE '%{val}%'");
                    }
                }
                if (chkPno.Checked == true)
                {
                    string val = txtPno.Text;
                    if (!string.IsNullOrEmpty(val))
                    {
                        // Partial match for Part_No
                        cond.Rows.Add($"Part_No LIKE '%{val}%'");
                    }
                }
                if (chkpono.Checked == true)
                {
                    string val = txtpono.Text;
                    if (!string.IsNullOrEmpty(val))
                    {
                        cond.Rows.Add($"PO_No LIKE '%{val}%'");
                    }
                }
                if (chkinvoice.Checked == true)
                {
                    string val = txtinvoice.Text;
                    if (!string.IsNullOrEmpty(val))
                    {
                        cond.Rows.Add($"Invoice LIKE '%{val}%'");
                    }
                }
                if (chkremark.Checked == true)
                {
                    string val = txtremark.Text;
                    if (!string.IsNullOrEmpty(val))
                    {
                        cond.Rows.Add($"Remark LIKE '%{val}%'");
                    }
                }
                if (chkrecdate.Checked == true)
                {
                    DateTime from = dtprecdatefrom.Value;
                    DateTime to = dtprecdateto.Value;
                    cond.Rows.Add($"RegDate BETWEEN '{from.ToString("yyyy-MM-dd")}' AND '{to.ToString("yyyy-MM-dd")}'");
                }
                if (chkstatus.Checked == true)
                {
                    string val = cbtype.Text;
                    if (!string.IsNullOrEmpty(val))
                    {
                        if (val == "Stock In")
                        {
                            cond.Rows.Add($"Stock_Out = 0");
                        }
                        else if (val == "Stock Out")
                        {
                            cond.Rows.Add($"Stock_In = 0");
                        }

                    }
                }
                string Conds = "";
                foreach (DataRow row in cond.Rows)
                {
                    if (Conds.Trim() == "")
                    {
                        Conds = " AND " + row["Value"];
                    }
                    else
                    {
                        Conds += " AND " + row["Value"];
                    }
                }
                con.con.Open();
                string query = "SELECT * FROM SparePartTrans WHERE Dept = '" + dept  +"'" + Conds + " ORDER BY TransNo ";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                Console.WriteLine(query);
                sda.Fill(dt);
                if (dt.Rows.Count < 0)
                {
                    MessageBox.Show("Please enter a valid Transaction ID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while fetching data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            foreach (DataRow row in dt.Rows)
            {
                string transactionID = row["TransNo"].ToString();
                string code = row["Code"].ToString();
                string remark = row["Remark"].ToString();
                string partno = row["Part_No"].ToString();
                string partname = row["Part_Name"].ToString();
                double stokin = row["Stock_In"] is DBNull ? 0 : Convert.ToDouble(row["Stock_In"]);
                double stockamount = row["Stock_Amount"] is DBNull ? 0 : Convert.ToDouble(row["Stock_Amount"]);
                double stokout = row["Stock_Out"] is DBNull ? 0 : Convert.ToDouble(row["Stock_Out"]);
                DateTime regdate = row["RegDate"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(row["RegDate"]);
                string regby = row["PIC"].ToString();
                string pono = row["PO_No"]?.ToString();
                string invoice = row["Invoice"]?.ToString();
                dgvTTL.Rows.Add();
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["TranNo"].Value = transactionID;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["Code"].Value = code;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["partno"].Value = partno;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["partname"].Value = partname;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["stockin"].Value = stokin;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["stockout"].Value = stokout;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["stockamount"].Value = stockamount.ToString("N2");
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["pono"].Value = pono;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["regdate"].Value = regdate.ToString("yyyy-MM-dd HH:mm:ss");
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["regby"].Value = regby;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["remark"].Value = remark;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["invoice"].Value = invoice;



            }

            dgvTTL.ClearSelection();
            con.con.Close();
            Cursor = Cursors.Default;
            lbFound.Text = "Found: " + dgvTTL.Rows.Count.ToString();
        }
    }
}
