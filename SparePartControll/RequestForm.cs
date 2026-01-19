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
            this.txtcode.TextChanged += Txtcode_TextChanged;
            this.txtPname.TextChanged += TxtPname_TextChanged;
            this.txtPno.TextChanged += TxtPno_TextChanged;
            this.dgvRequest.CellClick += DgvRequest_CellClick;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnUpdate.Click += BtnUpdate_Click;
            this.chkcode.CheckedChanged += Chkcode_CheckedChanged;
            this.chkPname.CheckedChanged += ChkPname_CheckedChanged;
            this.chkPno.CheckedChanged += ChkPno_CheckedChanged;
        }

        private void ChkPno_CheckedChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void ChkPname_CheckedChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void Chkcode_CheckedChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void RequestForm_Shown(object sender, EventArgs e)
        {
            Search();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
           if (dgvRequest.Rows.Count > 0)
            {
                if (dgvRequest.Rows[dgvRequest.CurrentCell.RowIndex].Cells["mcdocno"].Value.ToString().Trim() == "")
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
                    Search();
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
                    Search();
                }
             
            }
        }

        private void TxtPno_TextChanged(object sender, EventArgs e)
        {
            
            if (txtPno.Text.Trim() == "")
            {
                chkPno.Checked = false;
                Search();
            }
            else

            {
                chkPno.Checked = true;
                Search();
            }
        }

        private void TxtPname_TextChanged(object sender, EventArgs e)
        {
           
            if (txtPname.Text.Trim() == "")
            {
                chkPname.Checked = false;
                Search();
            }
            else
            {
                chkPname.Checked = true;
                Search();
            }
        }

        private void Txtcode_TextChanged(object sender, EventArgs e)
        {
          
            if (txtcode.Text.Trim() == "")
            {
               chkcode.Checked = false;
                Search();
            }
            else
            {
                chkcode.Checked = true;
                Search();
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
