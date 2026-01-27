using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace MachineDeptApp
{
    public partial class InvoiceForm : Form
    {
        SQLConnect con = new SQLConnect();
        SQLConnectOBS conOBS = new SQLConnectOBS();
        string dept = "MC";
        public InvoiceForm()
        {
            this.con.Connection();
            this.conOBS.Connection();
            InitializeComponent();
            /*this.Load += InvoiceForm_Load;
            //this.btnImport.Click += BtnImport_Click;
            //this.btnSave.Click += BtnSave_Click;
            //this.dgvInvoice.CellClick += DgvInvoice_CellClick;
            //this.dgvPo.CellClick += DgvPo_CellClick;
            //this.btnDelete.Click += BtnDelete_Click;*/
            this.btnSave.Click += BtnSave_Click;
            this.btnSearch.Click += BtnSearch_Click;
            this.Load += InvoiceForm_Load;
            this.txtInvoice.KeyDown += TxtInvoice_KeyDown;
            this.btnNew.Click += BtnNew_Click;
            this.dgvInvoice.CellValueChanged += DgvInvoice_CellValueChanged;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            txtInvoice.Focus(); 
            SendKeys.Send("{ENTER}");
        }

        private void DgvInvoice_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            lbfound.Text = "Found : " + dgvInvoice.Rows.Count;
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            if (dgvInvoice.Rows.Count > 0)
            {
                DialogResult ask = MessageBox.Show("Are you want to clear all data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask == DialogResult.No)
                    return;
                else
                    dgvInvoice.Rows.Clear();
            }
        }
        private void TxtInvoice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtInvoice.Text.Trim()))
                {
                    btnSave.Enabled = false;
                    btnSaveGrey.BringToFront();
                    dgvInvoice.Rows.Clear();
                    Cursor = Cursors.WaitCursor;
                    DataTable dtselectCompare = new DataTable();
                    try
                    {
                        con.con.Open();
                        string queryselect = "SELECT Invoice FROM SparePartTrans WHERE Invoice = '" + txtInvoice.Text + "'";
                        SqlDataAdapter sdaselect = new SqlDataAdapter(queryselect, con.con);
                        sdaselect.Fill(dtselectCompare);
                    }
                    catch
                    {
                        MessageBox.Show("Error while selecting compare !", "Error Select", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    con.con.Close();
                    if (dtselectCompare.Rows.Count > 0)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("This Invoice already receive !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        DataTable dtfill = new DataTable();
                        try
                        {
                            con.con.Open();
                            string queryselectOBS = "SELECT tbInvoice.ItemCode,tbInvoice.DONo, tbName.Part_Name,tbName.Part_No, tbInvoice.Qty, tbInvoice.UnitPrice, tbInvoice.Amount, tbInvoice.RecvDate, tbType.ItemType, tbPono.PurchaseNo " +
                                "FROM [" + conOBS.server + "].[" + conOBS.db + "].[dbo].[prgrecvresult] tbInvoice " +
                                "INNER JOIN (SELECT  Code, Part_Name, Part_No FROM MstMCSparePart) tbName ON tbInvoice.ItemCode = tbName.Code " +
                                "INNER JOIN (SELECT ItemCode, ItemType FROM [" + conOBS.server + "].[" + conOBS.db + "].[dbo].mstitem) tbType ON tbInvoice.ItemCode = tbType.ItemCode " +
                                "INNER JOIN (SELECT PurchaseNo, PurchaseOrderCode FROM [" + conOBS.server + "].[" + conOBS.db + "].[dbo].prgpurchaseorderheader) tbPono ON tbInvoice.PurchaseOrderCode = tbPono.PurchaseOrderCode " +
                                "WHERE ItemType = 4 AND tbInvoice.DONo = '" + txtInvoice.Text + "'";
                            Console.WriteLine(queryselectOBS);
                            SqlDataAdapter sdaselect = new SqlDataAdapter(queryselectOBS, con.con);
                            sdaselect.Fill(dtfill);
                        }
                        catch
                        {
                            MessageBox.Show("Error while selecting fill dgv !", "Error SelectFill", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        con.con.Close();
                        if (dtfill.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtfill.Rows)
                            {
                                dgvInvoice.Rows.Add();
                                dgvInvoice.Rows[dgvInvoice.Rows.Count - 1].Cells["description"].Value = "SPARE PART";
                                dgvInvoice.Rows[dgvInvoice.Rows.Count - 1].Cells["code"].Value = row["ItemCode"].ToString();
                                dgvInvoice.Rows[dgvInvoice.Rows.Count - 1].Cells["partname"].Value = row["Part_Name"].ToString();
                                dgvInvoice.Rows[dgvInvoice.Rows.Count - 1].Cells["Pno"].Value = row["Part_No"].ToString();
                                dgvInvoice.Rows[dgvInvoice.Rows.Count - 1].Cells["pono"].Value = row["PurchaseNo"].ToString();
                                double qty;
                                if (double.TryParse(row["Qty"].ToString(), out qty))
                                {
                                    dgvInvoice.Rows[dgvInvoice.Rows.Count - 1].Cells["qty"].Value = qty;
                                }
                                double priceusd;
                                if (double.TryParse(row["UnitPrice"].ToString(), out priceusd))
                                {
                                    dgvInvoice.Rows[dgvInvoice.Rows.Count - 1].Cells["priceusd"].Value = priceusd;
                                }
                                double amt;
                                if (double.TryParse(row["Amount"].ToString(), out amt))
                                {
                                    dgvInvoice.Rows[dgvInvoice.Rows.Count - 1].Cells["amountusd"].Value = amt;
                                }
                                dgvInvoice.Rows[dgvInvoice.Rows.Count - 1].Cells["invoiceno"].Value = row["DONo"].ToString();
                                DateTime date;
                                if (DateTime.TryParse(row["RecvDate"].ToString(), out date))
                                {
                                    dgvInvoice.Rows[dgvInvoice.Rows.Count - 1].Cells["invoicedate"].Value = date;
                                }
                            }
                            btnSave.Enabled = true;
                            btnSave.BringToFront();
                            txtInvoice.Focus();
                        }

                    }
                    con.con.Close();
                    Cursor = Cursors.Default;
                }
            }
        }
        private void InvoiceForm_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            btnSaveGrey.BringToFront();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (dgvInvoice.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sur you want to save this invoice?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }
                SaveToDBInvoice();
            }
        }
        private void SaveToDBInvoice()
        {
            int error = 0;
            DataTable dtselect = new DataTable();
            DataTable dtMst = new DataTable();
            Cursor = Cursors.WaitCursor;
            List<string> code = new List<string>();
            foreach (DataGridViewRow row in dgvInvoice.Rows)
            {
                code.Add(row.Cells["code"].Value.ToString());
            }
            string codelist = "('" + string.Join("','", code) + "')";
            //select from request
            try
            {

                con.con.Open();
                string query = "SELECT * FROM MCSparePartRequest WHERE Dept = '" + dept + "' AND Code IN " + codelist + "ORDER BY Code";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dtselect);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while selecting table request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
       
                error++;
            }
            con.con.Close();
            //select from master
            try
            {
                con.con.Open();
                string query = "SELECT Code, Part_No, Part_Name FROM MstMCSparePart WHERE Dept =  '" + dept + "' AND Code IN " + codelist + "ORDER BY Code";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dtMst);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while selecting master: " + ex.Message);
                error++;
            }
            con.con.Close();
            if (dtselect.Rows.Count <= 0)
            {
                MessageBox.Show("This spare part not have in request", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Cursor = Cursors.Default;
                return;
            }
            //Compare stock
            try
            {
                Cursor = Cursors.WaitCursor;
                DateTime now = DateTime.Now;
                //Loop Compare
                foreach (DataGridViewRow row1 in dgvInvoice.Rows)
                {
                    string CodeIN = row1.Cells["code"]?.Value?.ToString() ?? "";
                    double recqty = double.TryParse(row1.Cells["qty"]?.Value?.ToString(), out var q) ? q : 0;
                    foreach (DataRow row2 in dtselect.Rows)
                    {
                        string MCdocNo = row2["MCDocNo"].ToString();
                        string CodeSelect = row2["Code"].ToString();
                        string ponoSelect = row2["PO_No"].ToString();
                        double balance = row2["Balance"] != DBNull.Value ? Convert.ToDouble(row2["Balance"]) : 0;

                        if (CodeIN == CodeSelect && string.IsNullOrEmpty(MCdocNo))
                        {
                            if (recqty > balance)
                            {
                                MessageBox.Show("Receive Qty cannot bigger than balance !", "Please check.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                con.con.Close();
                                Cursor = Cursors.Default;
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Saving" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                error++;
            }
            try
            {
                Cursor = Cursors.WaitCursor;
                DateTime now = DateTime.Now;
                //Loop Update 
                foreach (DataGridViewRow row1 in dgvInvoice.Rows)
                {
                    string CodeIN = row1.Cells["code"]?.Value?.ToString() ?? "";
                    string ponoIN = row1.Cells["pono"]?.Value?.ToString() ?? "";
                    double recqty = double.TryParse(row1.Cells["qty"]?.Value?.ToString(), out var q) ? q : 0;
                    double amount = double.TryParse(row1.Cells["amountusd"]?.Value?.ToString(), out var a) ? a : 0;
                    string invoicedate = row1.Cells["invoicedate"]?.Value?.ToString() ?? "";
                    foreach (DataRow row2 in dtselect.Rows)
                    {
                        string MCdocNo = row2["MCDocNo"].ToString();
                        string CodeSelect = row2["Code"].ToString();
                        string ponoSelect = row2["PO_No"].ToString();
                        double orderqty = row2["Order_Qty"] != DBNull.Value ? Convert.ToDouble(row2["Order_Qty"]) : 0;
                        double qtyZin = row2["ReceiveQTY"] != DBNull.Value ? Convert.ToDouble(row2["ReceiveQTY"]) : 0;
                        double balance = row2["Balance"] != DBNull.Value ? Convert.ToDouble(row2["Balance"]) : 0;
                        double unitPrice = row2["UnitPrice"] != DBNull.Value ? Convert.ToDouble(row2["UnitPrice"]) : 0;

                        double finalQty = qtyZin + recqty;
                        double finalBalance = orderqty - finalQty;
                        double reAmount = finalBalance * unitPrice;

                        string status = "Waiting for PO Update";
                        if (finalBalance == 0)
                        {
                            status = "Completed";
                        }
                        if (CodeIN == CodeSelect &&  string.IsNullOrEmpty(MCdocNo))
                        {
                            //Update
                            try
                            {
                                con.con.Open();
                                string query = "UPDATE MCSparePartRequest SET ReceiveQTY =  @recqty, Balance = @balance, RemainAmount = @reamount, Receive_Date = @recdate, Order_State = @orderstate, " +
                            " Remark = @remark, UpdateDate = @update" +
                            " WHERE Code = @code AND PO_No = @pono AND Order_State <> 'Completed' AND Dept = @dept";
                                SqlCommand cmd = new SqlCommand(query, con.con);
                                cmd.Parameters.AddWithValue("@recqty", finalQty);
                                cmd.Parameters.AddWithValue("@balance", finalBalance);
                                cmd.Parameters.AddWithValue("@reamount", reAmount);
                                cmd.Parameters.AddWithValue("@recdate", invoicedate);
                                cmd.Parameters.AddWithValue("@orderstate", status);
                                cmd.Parameters.AddWithValue("@remark", "Update: " + now.ToString("dd-MM-yyyy"));
                                cmd.Parameters.AddWithValue("@update", now);
                                cmd.Parameters.AddWithValue("@code", CodeSelect);
                                cmd.Parameters.AddWithValue("@pono", ponoIN);
                                cmd.Parameters.AddWithValue("@dept", dept);
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error while Update data" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                error++;
                            }
                            con.con.Close();
                            break;
                        }
                    }
                }
                //Loop insert
                foreach (DataGridViewRow row1 in dgvInvoice.Rows)
                {
                    string CodeIN = row1.Cells["code"]?.Value?.ToString() ?? "";
                    string ponoIN = row1.Cells["pono"]?.Value?.ToString() ?? "";
                    double recqty = double.TryParse(row1.Cells["qty"]?.Value?.ToString(), out var q) ? q : 0;
                    string PartNo = row1.Cells["Pno"]?.Value?.ToString() ?? "";
                    double amount = double.TryParse(row1.Cells["amountusd"]?.Value?.ToString(), out var a) ? a : 0;
                    string invoice = row1.Cells["invoiceno"]?.Value?.ToString() ?? "";
                    string pono = row1.Cells["pono"]?.Value?.ToString() ?? "";
                    foreach (DataRow row2 in dtMst.Rows)
                    {
                        string CodeSelect = row2["Code"]?.ToString() ?? "";
                        string Partname = row2["Part_Name"]?.ToString() ?? "";
                        if (CodeIN == CodeSelect)
                        {
                            DataTable dtTran = new DataTable();
                            string tranNo = "TR-A0000001";
                            try
                            {
                                con.con.Open();
                                string query = "SELECT MAX(TransNo) As TranNo FROM SparePartTrans ";
                                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                                sda.Fill(dtTran);
                                if (dtTran.Rows.Count > 0 && dtTran.Rows[0]["TranNo"] != DBNull.Value)
                                {
                                    string last = dtTran.Rows[0]["TranNo"].ToString();
                                    string prefix = last.Substring(0, 4);
                                    if (int.TryParse(last.Substring(4), out int number))
                                    {
                                        tranNo = prefix + (number + 1).ToString("D7");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error while selecting TransNo" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                error++;
                            }
                            con.con.Close();
                            //Insert
                            try
                            {
                                con.con.Open();
                                string query = "INSERT INTO SparePartTrans (TransNo, Code, Part_No, Part_Name, Dept, Stock_In, Stock_Out, Stock_Value, Stock_Amount, PO_No, Invoice, Status, RegDate, PIC, Remark) " +
                                                                                      " VALUES (@TransNo, @code, @partno, @partname, @dept, @stockIn, @stockOut, @stockVal, @stockAmount, @pono, @invoice, @status, @Rd, @pic, @remark )";

                                SqlCommand cmd = new SqlCommand(query, con.con);
                                cmd.Parameters.AddWithValue("@TransNo", tranNo);
                                cmd.Parameters.AddWithValue("@code", CodeSelect);
                                cmd.Parameters.AddWithValue("@partno", PartNo);
                                cmd.Parameters.AddWithValue("@partname", Partname);
                                cmd.Parameters.AddWithValue("@dept", dept);
                                cmd.Parameters.AddWithValue("@stockIn", recqty);
                                cmd.Parameters.AddWithValue("@stockOut", 0);
                                cmd.Parameters.AddWithValue("@stockVal", recqty);
                                cmd.Parameters.AddWithValue("@stockAmount", amount);
                                cmd.Parameters.AddWithValue("@pono", pono);
                                cmd.Parameters.AddWithValue("@invoice", invoice);
                                cmd.Parameters.AddWithValue("@status", 1);
                                cmd.Parameters.AddWithValue("@Rd", now);
                                cmd.Parameters.AddWithValue("@pic", MenuFormV2.UserForNextForm);
                                cmd.Parameters.AddWithValue("@remark", "Update: " + now.ToString("dd-MM-yyyy"));
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error while Update data" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                error++;
                            }
                            con.con.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Saving" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                error++;
            }
            if (error == 0)
            {
                MessageBox.Show("Save successfully !", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvInvoice.Rows.Clear();
                btnSave.Enabled = false;
                btnSave.BringToFront();
            }
            else
            {
                MessageBox.Show("Error Saving", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            con.con.Close();
            Cursor = Cursors.Default;
        }
    }
}
