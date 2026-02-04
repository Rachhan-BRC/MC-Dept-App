using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.SparePartControll
{
    public partial class RequestForm : Form
    {
        SQLConnect con = new SQLConnect();
        string dept = "MC";
        string nextdocno = "";
        public RequestForm()
        {
            this.con.Connection();
            InitializeComponent();
            this.Shown += RequestForm_Shown;

            //txt
            this.txtcode.TextChanged += Txtcode_TextChanged;
            this.txtPname.TextChanged += TxtPname_TextChanged;
            this.txtPno.TextChanged += TxtPno_TextChanged;
            this.txtdocno.TextChanged += Txtdocno_TextChanged;
            this.txtpono.TextChanged += Txtpono_TextChanged;
            this.txtremark.TextChanged += Txtremark_TextChanged;

            //dtp
            this.dtpissuefrom.ValueChanged += Dtpissuefrom_ValueChanged;
            this.dtpissueto.ValueChanged += Dtpissueto_ValueChanged;
            this.dtprecdatefrom.ValueChanged += Dtprecdatefrom_ValueChanged;
            this.dtprecdateto.ValueChanged += Dtprecdateto_ValueChanged;

            //dgv
            this.dgvRequest.CellClick += DgvRequest_CellClick;

            //button
            this.btnDelete.Click += BtnDelete_Click;
            this.btnUpdate.Click += BtnUpdate_Click;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnExport.Click += BtnExport_Click;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvRequest.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("Are yoiu want to export data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "CSV file (*.csv)|*.csv";
                    saveDialog.FileName = "PO Request" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;
                        try
                        {
                            //Write Column name
                            int columnCount = 0;
                            foreach (DataGridViewColumn DgvCol in dgvRequest.Columns)
                            {
                                if (DgvCol.Visible == true)
                                {
                                    columnCount = columnCount + 1;
                                }
                            }
                            string columnNames = "";

                            //String array for Csv
                            string[] outputCsv;
                            outputCsv = new string[dgvRequest.Rows.Count + 1];

                            //Set Column Name
                            for (int i = 0; i < columnCount; i++)
                            {
                                if (dgvRequest.Columns[i].Visible == true)
                                {
                                    columnNames += dgvRequest.Columns[i].HeaderText.ToString() + ",";
                                }
                            }
                            outputCsv[0] += columnNames;

                            //Row of data 
                            for (int i = 1; (i - 1) < dgvRequest.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    if (dgvRequest.Columns[j].Visible == true)
                                    {
                                        string Value = "";
                                        if (dgvRequest.Rows[i - 1].Cells[j].Value != null)
                                        {
                                            Value = dgvRequest.Rows[i - 1].Cells[j].Value.ToString();
                                        }
                                        //Fix don't separate if it contain '\n' or ','
                                        Value = "\"" + Value.Replace("\"", "\"\"") + "\"";
                                        outputCsv[i] += Value + ",";
                                    }
                                }
                            }

                            File.WriteAllLines(saveDialog.FileName, outputCsv, Encoding.UTF8);
                            Cursor = Cursors.Default;
                            MessageBox.Show("Export successfully !", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            Cursor = Cursors.Default;
                            MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Error Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void Dtpissueto_ValueChanged(object sender, EventArgs e)
        {
            chkissue.Checked = true;
        }

        private void Dtprecdateto_ValueChanged(object sender, EventArgs e)
        {
            chkrecdate.Checked = true;
        }

        private void Dtprecdatefrom_ValueChanged(object sender, EventArgs e)
        {
            chkrecdate.Checked = true;
        }

        private void Dtpissuefrom_ValueChanged(object sender, EventArgs e)
        {
            chkissue.Checked = true;
        }

        private void Txtremark_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtremark.Text))
            {
                chkremark.Checked = true;
            }
            else
            {
                chkremark.Checked = false;
            }
        }

        private void Txtpono_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtpono.Text))
            {
                chkpono.Checked = true;
            }
            else
            {
                chkpono.Checked = false;
            }
        }

        private void Txtdocno_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtdocno.Text))
            {
                chkdocno.Checked = true;
            }
            else
            {
                chkdocno.Checked = false;
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void RequestForm_Shown(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
           if (dgvRequest.Rows.Count > 0)
            {
                int rec = Convert.ToInt32(dgvRequest.Rows[dgvRequest.CurrentCell.RowIndex].Cells["receiveqty"].Value);
                if ( rec == 0)
                {
                    DataTable update = new DataTable();
                    string mcdocno = dgvRequest.Rows[dgvRequest.CurrentCell.RowIndex].Cells["pono"].Value.ToString();
                    PrintForm prf = new PrintForm(update);
                    string queryselect = "SELECT * FROM MCSparePartRequest WHERE PO_No = '" + mcdocno + "'";
                    SqlDataAdapter sda = new SqlDataAdapter(queryselect, con.con);
                    sda.Fill(update);
                    prf.WindowState = FormWindowState.Normal;
                    prf.FormBorderStyle = FormBorderStyle.FixedSingle;
                    prf.StartPosition = FormStartPosition.CenterScreen;
                    prf.Text = "Update";
                    prf.ShowDialog();
                    
                }
                else
                {
                    MessageBox.Show("This Document already receive !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            
        }

        private void DgvRequest_CellClick(object sender, DataGridViewCellEventArgs e)
        {
       
            if (e.RowIndex >= 0 && e.ColumnIndex >=0)
            {
                double receive = Convert.ToDouble(dgvRequest.Rows[e.RowIndex].Cells["receiveqty"].Value);
                if (receive == 0)
                {
                    btnDelete.Enabled = true;
                    btnDelete.BringToFront();
                    btnUpdate.Enabled = true;
                    btnUpdate.BringToFront();
                }

            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            DialogResult ask = MessageBox.Show("Are you sure you want to delete this request?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask == DialogResult.Yes)
            {
                int erro = 0;
                try
                {
                    con.con.Open();
                    string code = dgvRequest.Rows[dgvRequest.CurrentCell.RowIndex].Cells["code"].Value.ToString();
                    string pono = dgvRequest.Rows[dgvRequest.CurrentCell.RowIndex].Cells["pono"].Value.ToString();
                    string issuedate = dgvRequest.Rows[dgvRequest.CurrentCell.RowIndex].Cells["issuedate"].Value.ToString();
                    string query = "DELETE FROM MCSparePartRequest WHERE Code = @code AND PO_No = @pono AND  IssueDate = @issuedate AND Dept = @dept ";
                    SqlCommand cmd = new SqlCommand(query, con.con);
                    cmd.Parameters.AddWithValue("@code", code);
                    cmd.Parameters.AddWithValue("@pono", pono);
                    cmd.Parameters.AddWithValue("@issuedate", issuedate); 
                    cmd.Parameters.AddWithValue("@dept", dept);
                    cmd.ExecuteNonQuery();
                    int rowIndex1 = dgvRequest.CurrentCell.RowIndex;
                    if (rowIndex1 >= 0 && rowIndex1 < dgvRequest.Rows.Count)
                    {
                        dgvRequest.Rows.RemoveAt(rowIndex1);
                    
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while deleting data "+ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    erro++;
                }
                con.con.Close();
                Cursor = Cursors.Default;
                if (erro == 0)
                {
                    MessageBox.Show("Delete successfully", "Done.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnDelete.Enabled = false;
                    btnDeleteGray.BringToFront();
                    dgvRequest.ClearSelection();
                    
                }
             
            }
        }

        private void TxtPno_TextChanged(object sender, EventArgs e)
        {
            
            if (txtPno.Text.Trim() == "")
            {
                chkPno.Checked = false;
                
            }
            else

            {
                chkPno.Checked = true;
                
            }
        }

        private void TxtPname_TextChanged(object sender, EventArgs e)
        {
           
            if (txtPname.Text.Trim() == "")
            {
                chkPname.Checked = false;
                
            }
            else
            {
                chkPname.Checked = true;
                
            }
        }

        private void Txtcode_TextChanged(object sender, EventArgs e)
        {
          
            if (txtcode.Text.Trim() == "")
            {
               chkcode.Checked = false;
                
            }
            else
            {
                chkcode.Checked = true;
                
            }
        }

       private void Search()
        {

            dgvRequest.Rows.Clear();
            Cursor = Cursors.WaitCursor;

            con.con.Close();
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
                        cond.Rows.Add($"R.Code LIKE '%{val}%'");
                    }
                }
                if (chkPname.Checked == true)
                {
                    string val = txtPname.Text;
                    if (!string.IsNullOrEmpty(val))
                    {
                        // Partial match for Part_Name
                        cond.Rows.Add($"M.Part_Name LIKE '%{val}%'");
                    }
                }
                if (chkPno.Checked == true) // <-- use a different checkbox for Part_No
                {
                    string val = txtPno.Text;
                    if (!string.IsNullOrEmpty(val))
                    {
                        // Exact match for Part_No
                        cond.Rows.Add($"M.Part_No = '{val}'");
                    }
                }
                if (chkdocno.Checked == true) 
                {
                    string val = txtdocno.Text;
                    if (!string.IsNullOrEmpty(val))
                    {
                        cond.Rows.Add($"R.MCDocNo LIKE '%{val}%'");
                    }
                }
                if (chkpono.Checked == true)
                {
                    string val = txtpono.Text;
                    if (!string.IsNullOrEmpty(val))
                    {
                        cond.Rows.Add($"R.PO_No LIKE '%{val}%'");
                    }
                }
                if (chkremark.Checked == true)
                {
                    string val = txtremark.Text;
                    if (!string.IsNullOrEmpty(val))
                    {
                        cond.Rows.Add($"R.Remark LIKE '%{val}%'");
                    }
                }
                if (chkstatus.Checked == true)
                {
                    string val = cbstatus.Text;
                    if (!string.IsNullOrEmpty(val))
                    {
                        if (val == "Completed")
                        {
                            cond.Rows.Add($"R.Balance = 0");
                        }
                        else if(val == "Not Completed")
                        {
                            cond.Rows.Add($"R.Balance > 0");
                        }
                       
                    }
                }
                if (chkissue.Checked == true)
                {
                    DateTime from = dtpissuefrom.Value;
                    DateTime to = dtpissueto.Value;
                    cond.Rows.Add($"R.IssueDate BETWEEN '{from.ToString("yyyy-MM-dd")}' AND '{to.ToString("yyyy-MM-dd")}'");
                }
                if (chkrecdate.Checked == true)
                {
                    DateTime from = dtprecdatefrom.Value;
                    DateTime to = dtprecdateto.Value;
                    cond.Rows.Add($"R.receive_Date BETWEEN '{from.ToString("yyyy-MM-dd")}' AND '{to.ToString("yyyy-MM-dd")}'");
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
                DataTable dt = new DataTable();
                string query = "SELECT R.Code, R.PO_No, R.IssueDate, R.ETA, R.Order_Qty, R.UnitPrice, R.Amount, R.ReceiveQTY, " +
                    " R.Balance, R.RemainAmount, R.Receive_Date, R.Order_State, R.Remark, R.UpdateDate, R.MCDocNo, M.Part_No, M.Part_Name FROM MCSparePartRequest R " +
                    " LEFT JOIN MstMCSparePart M ON R.Code = M.Code where R.Dept = '" + dept + "'" + Conds;
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    string code = row["Code"]?.ToString() ?? string.Empty;
                    string Docno = row["MCDocNo"]?.ToString() ?? string.Empty;
                    string Pono = row["PO_No"]?.ToString() ?? string.Empty;
                    string pno = row["Part_No"]?.ToString() ?? string.Empty;
                    string pname = row["Part_Name"]?.ToString() ?? string.Empty;

                    DateTime? issuedate = row["IssueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["IssueDate"]);
                    DateTime? eta = row["ETA"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["ETA"]);

                    double orderqty = row["Order_Qty"] == DBNull.Value ? 0 : Convert.ToDouble(row["Order_Qty"]);
                    double unitprice = row["UnitPrice"] == DBNull.Value ? 0 : Convert.ToDouble(row["UnitPrice"]);
                    double amount = row["Amount"] == DBNull.Value ? 0 : Convert.ToDouble(row["Amount"]);
                    double receiveqty = row["ReceiveQTY"] == DBNull.Value ? 0 : Convert.ToDouble(row["ReceiveQTY"]);
                    double balance = row["Balance"] == DBNull.Value ? 0 : Convert.ToDouble(row["Balance"]);
                    double remainamount = row["RemainAmount"] == DBNull.Value ? 0 : Convert.ToDouble(row["RemainAmount"]);

                    DateTime? receivedate = row["Receive_Date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["Receive_Date"]);

                    string orderstate = row["Order_State"]?.ToString() ?? string.Empty;
                    string remark = row["Remark"]?.ToString() ?? string.Empty;
                    string updatedate = row["UpdateDate"]?.ToString() ?? string.Empty;


                    dgvRequest.Rows.Add();
                    dgvRequest.Rows[dgvRequest.Rows.Count - 1].Cells["code"].Value = code;
                    dgvRequest.Rows[dgvRequest.Rows.Count - 1].Cells["Pno"].Value = pno;
                    dgvRequest.Rows[dgvRequest.Rows.Count - 1].Cells["Pname"].Value = pname;
                    dgvRequest.Rows[dgvRequest.Rows.Count - 1].Cells["issuedate"].Value = issuedate;
                    dgvRequest.Rows[dgvRequest.Rows.Count - 1].Cells["orderqty"].Value = orderqty;
                    dgvRequest.Rows[dgvRequest.Rows.Count - 1].Cells["unitprice"].Value = unitprice;
                    dgvRequest.Rows[dgvRequest.Rows.Count - 1].Cells["amount"].Value = amount;
                    dgvRequest.Rows[dgvRequest.Rows.Count - 1].Cells["eta"].Value = eta;
                    dgvRequest.Rows[dgvRequest.Rows.Count - 1].Cells["pono"].Value = Pono;
                    dgvRequest.Rows[dgvRequest.Rows.Count - 1].Cells["mcdocno"].Value = Docno;
                    dgvRequest.Rows[dgvRequest.Rows.Count - 1].Cells["receivedate"].Value = receivedate;
                    dgvRequest.Rows[dgvRequest.Rows.Count - 1].Cells["receiveqty"].Value = receiveqty;
                    dgvRequest.Rows[dgvRequest.Rows.Count - 1].Cells["balance"].Value = balance;
                    dgvRequest.Rows[dgvRequest.Rows.Count - 1].Cells["remainamount"].Value = remainamount;
                    dgvRequest.Rows[dgvRequest.Rows.Count - 1].Cells["orderstatus"].Value = orderstate;
                    dgvRequest.Rows[dgvRequest.Rows.Count - 1].Cells["remark"].Value = remark;
                    dgvRequest.Columns["code"].Frozen = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while select data" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dgvRequest.ClearSelection();
            btnUpdate.Enabled = false;
            btnUpdateGrey.BringToFront();
            con.con.Close();
            Cursor = Cursors.Default;
            lbFound.Text = "Found: " + dgvRequest.Rows.Count.ToString();
        }
    }
}
