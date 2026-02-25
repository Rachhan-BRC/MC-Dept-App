
using MachineDeptApp.Inventory.Inprocess;
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
using static System.Runtime.CompilerServices.RuntimeHelpers;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.SparePartControll
{
    public partial class PrintForm : Form
    {
        SQLConnect con = new SQLConnect();
        string dept = "MC";
        string nextdocno = "";
        DataTable dtup = new DataTable();
        
        public PrintForm(DataTable dtupdate)
        {
            dtup = dtupdate;
            con.Connection();
            InitializeComponent();
            this.btnAddColor.Click += BtnAddColor_Click;
            this.dgvTTL.CellClick += DgvTTL_CellClick;
            this.btnDeletColor.Click += BtnDeletColor_Click;
            this.Shown += PrintForm_Shown;
            this.btnPrint.Click += BtnPrint_Click;
            this.btnSave.Click += BtnSave_Click;
            this.btnUpdate.Click += BtnUpdate_Click;
            this.dgvTTL.CellEndEdit += DgvTTL_CellEndEdit;
            this.txtcode.TextChanged += Txtcode_TextChanged;
            this.txtPname.TextChanged += TxtPname_TextChanged;
            this.txtPno.TextChanged += TxtPno_TextChanged;
            this.chkcode.CheckedChanged += Chkcode_CheckedChanged;
            this.chkPname.CheckedChanged += ChkPname_CheckedChanged;
            this.chkPno.CheckedChanged += ChkPno_CheckedChanged;
            this.btnExport.Click += BtnExport_Click;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (rdnormal.Checked == true)
            {
                ExportToExcelFromTemplate();
            }
            if (rdMTH.Checked == true)
            {
                ExportToExcelFromTemplateMTH();
            }
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
        private void TxtPno_TextChanged(object sender, EventArgs e)
        {
            textchange();
        }
        private void TxtPname_TextChanged(object sender, EventArgs e)
        {
            textchange();
        }
        private void Txtcode_TextChanged(object sender, EventArgs e)
        {
            textchange();
        }
        private void textchange()
        {
            if (!string.IsNullOrEmpty(txtPno.Text))
            {
                chkPno.Checked = true;
                Search();
            }
            else 
            {
                chkPno.Checked = false;
                Search();
            }

            if (!string.IsNullOrEmpty(txtPname.Text))
            {
                chkPname.Checked = true;
                Search();
            }
            else{
                chkPname.Checked = false;
                Search();
            }

            if (!string.IsNullOrEmpty(txtcode.Text))
            {
                chkcode.Checked = true;
                Search();
            }
            else
            {
                chkcode.Checked = false;
                Search();
            }
        }
        private void DgvTTL_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        { 
            btnSave.Enabled = true;
            btnSave.BringToFront();
            if (dgvTTL.Columns[e.ColumnIndex].Name == "qty")

            {
                var row = dgvTTL.Rows[e.RowIndex];
                if (decimal.TryParse(row.Cells["qty"].Value?.ToString(), out decimal qty) &&
                    decimal.TryParse(row.Cells["unitprice"].Value?.ToString(), out decimal unitprice))
                {
                    row.Cells["amount"].Value = qty * unitprice;
                }
                else
                {
                    MessageBox.Show("Please input number only !", "Only number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    row.Cells["qty"].Value = 0;
                }
            }
            if (dgvTTL.Columns[e.ColumnIndex].Name == "unitprice")
            {
                var row = dgvTTL.Rows[e.RowIndex];
                if (decimal.TryParse(row.Cells["qty"].Value?.ToString(), out decimal qty) &&
                    decimal.TryParse(row.Cells["unitprice"].Value?.ToString(), out decimal unitprice))
                {
                    row.Cells["amount"].Value = qty * unitprice;
                }
                else
                {
                    MessageBox.Show("Please input number only !", "Only number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    row.Cells["unitprice"].Value = 0;
                }
            }
            if (dgvTTL.Columns[e.ColumnIndex].Name == "eta")
            {
                var row = dgvTTL.Rows[e.RowIndex];
                if (DateTime.TryParse(row.Cells["eta"].Value?.ToString(), out DateTime eta))
                {

                    row.Cells["eta"].Value = eta.ToString("dd-MMM-yyyy");
                    DateTime now = DateTime.Now;
                    DateTime a = Convert.ToDateTime(eta);
                    int monthDiff = (a.Year - now.Year) * 12 + (a.Month - now.Month);
                    int LT = monthDiff * 4;
                    row.Cells["leadtime"].Value = LT;
                }
                else
                {
                    MessageBox.Show("Please input date only !", "Only date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    row.Cells["eta"].Value = DateTime.Now.ToString("dd-MMM-yyyy");
                }
            }
            if (dgvTTL.Columns[e.ColumnIndex].Name == "leadtime")
            {
                var row = dgvTTL.Rows[e.RowIndex];
                if (int.TryParse(row.Cells["leadtime"].Value?.ToString(), out int Leadtime))
                {
                    int leadtime = Convert.ToInt32(row.Cells["leadtime"].Value);
                    int leadmonth = leadtime / 4;
                    DateTime eta = DateTime.Now.AddMonths(leadmonth);
                    row.Cells["eta"].Value = eta;
                }
                else
                {
                    MessageBox.Show("Please input date only !", "Only date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    row.Cells["eta"].Value = DateTime.Now.ToString("dd-MMM-yyyy");
                }
            }
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvTTL.Rows.Count > 0)
            {
                if (dtup.Rows.Count == 0)
                {
                    var row = dgvTTL.Rows[dgvTTL.CurrentCell.RowIndex];
                    row.Cells["eta"].ReadOnly = false;
                    row.Cells["qty"].ReadOnly = false;
                    row.Cells["unitprice"].ReadOnly = false;
                    row.Cells["leadtime"].ReadOnly = false;
                }
                else
                {
                    foreach (DataGridViewRow row in dgvTTL.Rows)
                    {
                        row.Cells["eta"].ReadOnly = false;
                        row.Cells["qty"].ReadOnly = false;
                        row.Cells["unitprice"].ReadOnly = false;
                        row.Cells["leadtime"].ReadOnly = false;
                    }
                }
            }
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;


            if (dtup.Rows.Count > 0)
            {
                int error = 0;
                try
                {
                    con.con.Open();
                    foreach (DataGridViewRow row in dgvTTL.Rows)
                    {
                        string code = row.Cells["code"].Value.ToString();
                        string mcdocno = row.Cells["mcdocno"].Value.ToString();
                        DateTime eta = Convert.ToDateTime(row.Cells["eta"].Value.ToString());
                        decimal qty = Convert.ToDecimal(row.Cells["qty"].Value.ToString());
                        decimal unitprice = Convert.ToDecimal(row.Cells["unitprice"].Value.ToString());
                        decimal amount = Convert.ToDecimal(row.Cells["amount"].Value.ToString());
                        string queryUpdate = "UPDATE MCSparePartRequest SET ETA = @eta, Order_Qty = @orderqty, UnitPrice = @unitprice, Amount = @amount, Balance = @balance, RemainAmount = @reamount" +
                       " WHERE Code = @code AND PO_No = @mcdocno";
                        SqlCommand cmd = new SqlCommand(queryUpdate, con.con);
                        cmd.Parameters.AddWithValue("@code", code);
                        cmd.Parameters.AddWithValue("@mcdocno", mcdocno);
                        cmd.Parameters.AddWithValue("@eta", eta);
                        cmd.Parameters.AddWithValue("@orderqty", qty);
                        cmd.Parameters.AddWithValue("@unitprice", unitprice);
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Parameters.AddWithValue("@balance", qty);
                        cmd.Parameters.AddWithValue("@reamount", amount);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    error++;
                    MessageBox.Show("Error while updating data !"+ ex.Message, "Error update", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                }

                con.con.Close();
                if (error == 0)
                {
                    dgvTTL.Rows.Clear();
                    MessageBox.Show("Save successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
           else
            {
                int error = 0;
                try
                {
                    string code = dgvTTL.Rows[dgvTTL.CurrentCell.RowIndex].Cells["code"].Value.ToString();
                    DateTime eta = Convert.ToDateTime(dgvTTL.Rows[dgvTTL.CurrentCell.RowIndex].Cells["eta"].Value.ToString());
                    DateTime set = Convert.ToDateTime(dgvTTL.Rows[dgvTTL.CurrentCell.RowIndex].Cells["seteta"].Value.ToString());
                    double qty = Convert.ToDouble(dgvTTL.Rows[dgvTTL.CurrentCell.RowIndex].Cells["qty"].Value);
                    double unitprice = Convert.ToDouble(dgvTTL.Rows[dgvTTL.CurrentCell.RowIndex].Cells["unitprice"].Value);
                    double amount = Convert.ToDouble(dgvTTL.Rows[dgvTTL.CurrentCell.RowIndex].Cells["amount"].Value);
                    
                    DateTime ? seteta = null;
                    if (eta != set )
                    {
                        seteta = Convert.ToDateTime(eta);
                    }
                    else
                    {
                        seteta = set;
                    }
                        con.con.Open();
                    string queryUpdate = "UPDATE tbPrintPending_temp SET OrderQty = @orderqty, UnitPrice = @unitprice, Amount = @amount, SetETA = @seteta WHERE Code = @code AND Dept = @dept";
                    SqlCommand cmd = new SqlCommand(queryUpdate, con.con);
                    cmd.Parameters.AddWithValue("@orderqty", qty);
                    cmd.Parameters.AddWithValue("@unitprice", unitprice);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@seteta", seteta);
                    cmd.Parameters.AddWithValue("@code", code);
                    cmd.Parameters.AddWithValue("@dept", dept);
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    error++;
                    MessageBox.Show("Error while updating data !" + ex.Message, "Error update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (error==0)
                {
                    con.con.Close();
                    Search();
                    MessageBox.Show("Save successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                con.con.Close();
            }
            Cursor = Cursors.Default;
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (dgvTTL.Rows.Count >  0)
            {
                int cannot = 0;
                try
                {
                    DataTable dtMst = new DataTable();
                    Cursor = Cursors.WaitCursor;
                    List<string> code = new List<string>();
                    foreach (DataGridViewRow row in dgvTTL.Rows)
                    {
                        code.Add(row.Cells["Code"].Value.ToString());
                    }
                    string codelist = "('" + string.Join("','", code) + "')";
                    con.con.Close();
                    //select from master
                    try
                    {
                        con.con.Open();
                        string query = "SELECT Code, Part_No, Unit_Price FROM MstMCSparePart WHERE Dept =  '" + dept + "' AND Code IN " + codelist + "ORDER BY Code";
                        SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                        sda.Fill(dtMst);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while selecting master: " + ex.Message);
                    }
                    con.con.Close();
                    if (dtMst.Rows.Count == 0)
                    {
                        MessageBox.Show("This spare part not have in Master", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    foreach (DataGridViewRow row1 in dgvTTL.Rows)
                    {
                        string code1 = row1.Cells["code"].Value.ToString();
                        foreach (DataRow row in dtMst.Rows)
                        {
                            string code2 = row["Code"].ToString();
                            if (code1 == code2)
                            {
                                row1.Cells["find"].Value = "Have";
                                break;
                            }
                        }
                    }
                    foreach (DataGridViewRow row1 in dgvTTL.Rows)
                    {
                        if (row1.Cells["find"].Value?.ToString() == "Not")
                        {
                            row1.Cells["code"].Style.BackColor = Color.LightPink;
                            cannot++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error while compare master!" + ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (cannot == 0)
                {
                    try
                    {

                        DateTime now1 = DateTime.Now;
                        string now = now1.ToString("yyyy-MM-dd");
                        string pono = GetNextDocNo();
                        nextdocno = pono;
                        foreach (DataGridViewRow row1 in dgvTTL.Rows)
                        {
                            string code = row1.Cells["Code"].Value.ToString();
                            double orderqty = Convert.ToDouble(row1.Cells["qty"].Value);
                            double unitprice = Convert.ToDouble(row1.Cells["unitprice"].Value);
                            double amount = Convert.ToDouble(row1.Cells["amount"].Value);
                            DateTime Eta = Convert.ToDateTime(row1.Cells["eta"].Value);

                            try
                            {
                                con.con.Open();
                                string query = "INSERT INTO MCSparePartRequest (Code, PO_No, MCDocNo, Dept, IssueDate, ETA, Order_Qty, UnitPrice, Amount, ReceiveQTY, Balance, RemainAmount, Receive_Date, Order_State, Remark) " +
                                                                                            " VALUES (@code, @pono, @mcdocno, @dept, @issuedate, @eta, @orderqty, @unitprice, @amount, @receiveqty, @balance, @reAmount, @recdate, @orderstate, @remark)";
                                SqlCommand cmd = new SqlCommand(query, con.con);
                                cmd.Parameters.AddWithValue("@code", code);
                                cmd.Parameters.AddWithValue("@pono", pono);
                                cmd.Parameters.AddWithValue("@mcdocno", pono);
                                cmd.Parameters.AddWithValue("@dept", dept);
                                cmd.Parameters.AddWithValue("@issuedate", now);
                                cmd.Parameters.AddWithValue("@orderqty", orderqty);
                                cmd.Parameters.AddWithValue("@unitprice", unitprice);
                                cmd.Parameters.AddWithValue("@amount", amount);
                                cmd.Parameters.AddWithValue("@receiveqty", 0);
                                cmd.Parameters.AddWithValue("@balance", orderqty);
                                cmd.Parameters.AddWithValue("@reAmount", amount);
                                cmd.Parameters.AddWithValue("@eta", Eta);
                                cmd.Parameters.AddWithValue("@recdate", Eta);
                                cmd.Parameters.AddWithValue("@orderstate", "Waiting for PO Update");
                                cmd.Parameters.AddWithValue("@remark", "Update: " + now);
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error while inserting to table! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            con.con.Close();
                        }
                        try
                        {
                            con.con.Open();
                            string queryDocNo = "INSERT INTO MCDocNo (DocNo) VALUES (@DocNo)";
                            SqlCommand cmdDocNo = new SqlCommand(queryDocNo, con.con);
                            cmdDocNo.Parameters.AddWithValue("@DocNo", pono);
                            cmdDocNo.ExecuteNonQuery();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error while inserting data! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        con.con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while inserting data! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (rdnormal.Checked == true)
                    {
                        ExportToExcelFromTemplate();
                        delete();
                    }
                    if (rdMTH.Checked == true)
                    {
                        ExportToExcelFromTemplateMTH();
                        delete();
                    }
                  
                }
                if (cannot > 0)
                {
                    MessageBox.Show("Some spare part not have in Master", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
              
                Cursor = Cursors.Default;
            }
          
        }
        private void PrintForm_Shown(object sender, EventArgs e)
        {
            Search();
           rdnormal.Checked = true;
            if (dtup.Rows.Count > 0)
            {
                dgvTTL.Rows.Clear();
                gpboxsearch.Visible = false;
                btnUpdate.Enabled = true;
                btnUpdate.BringToFront();
                
                btnAddColor.Visible = false; 
                btnPrint.Visible = false;

                foreach (DataRow row in dtup.Rows)
                {
                    dgvTTL.Columns["machinename"].Visible = false;
                    dgvTTL.Columns["supplier"].Visible = false;
                    dgvTTL.Columns["maker"].Visible = false;
                    dgvTTL.Columns["leadtime"].Visible = false;
                    dgvTTL.Columns["partno"].Visible = false;
                    dgvTTL.Columns["partname"].Visible = false;
                    DateTime eta = Convert.ToDateTime(row["ETA"]);
                    dgvTTL.Rows.Add();
                    dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["status"].Value = "Printed";
                    dgvTTL.Rows[dgvTTL.Rows.Count -1].Cells["code"].Value = row["Code"].ToString();
                    dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["qty"].Value = row["Order_Qty"].ToString();
                    dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["unitprice"].Value = row["UnitPrice"].ToString();
                    dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["amount"].Value = row["Amount"].ToString();
                    dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["eta"].Value = eta.ToString("dd-MMM-yyyy");
                    dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["mcdocno"].Value = row["PO_No"].ToString();
                }
                btnUpdate.PerformClick();
                btnUpdate.Visible = false;
                btnUpdateGrey.Visible = false;
            }
            else
            {
                gpboxsearch.Visible = true;
                btnAddColor.Visible = true;
                btnPrint.Visible = true;
                dgvTTL.Columns["partno"].Visible = true;
                dgvTTL.Columns["partname"].Visible = true;
                dgvTTL.Columns["machinename"].Visible = true;
                dgvTTL.Columns["supplier"].Visible = true;
                dgvTTL.Columns["maker"].Visible = true;
                dgvTTL.Columns["leadtime"].Visible = true;
                Search();
            }
         
        }
        private void BtnDeletColor_Click(object sender, EventArgs e)
        {
            DialogResult ask = MessageBox.Show("Are you sure you want to delete this data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask == DialogResult.No)
            {
                return;
            }
           if (dtup.Rows.Count > 0)
            {
                int erro = 0;
                try
                {
                    con.con.Open();
                    string code = dgvTTL.Rows[dgvTTL.CurrentCell.RowIndex].Cells["Code"].Value.ToString();
                    string pono = dgvTTL.Rows[dgvTTL.CurrentCell.RowIndex].Cells["mcdocno"].Value.ToString();
                    string query = "DELETE FROM MCSparePartRequest WHERE Code = @code AND PO_No = @pono AND Dept = @dept ";
                    SqlCommand cmd = new SqlCommand(query, con.con);
                    cmd.Parameters.AddWithValue("@code", code);
                    cmd.Parameters.AddWithValue("@pono", pono);
                    cmd.Parameters.AddWithValue("@dept", dept);
                    cmd.ExecuteNonQuery();
                    int rowIndex1 = dgvTTL.CurrentCell.RowIndex;
                    if (rowIndex1 >= 0 && rowIndex1 < dgvTTL.Rows.Count)
                    {
                        dgvTTL.Rows.RemoveAt(rowIndex1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while deleting data " + ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    erro++;
                }
                con.con.Close();
                Cursor = Cursors.Default;
                if (erro == 0)
                {
                    MessageBox.Show("Delete successfully", "Done.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btndelete.Enabled = false;
                    btndelete.BringToFront();
                    dgvTTL.ClearSelection();
                    Search();
                }

            }
            else
            {
                int rowIndex = dgvTTL.CurrentCell.RowIndex;
                if (rowIndex >= 0 && rowIndex < dgvTTL.Rows.Count)
                {
                    int success = 0;
                    try
                    {
                        con.con.Open();
                        string code = dgvTTL.Rows[dgvTTL.CurrentCell.RowIndex].Cells["Code"].Value.ToString();
                        string supplier = dgvTTL.Rows[dgvTTL.CurrentCell.RowIndex].Cells["supplier"].Value.ToString();
                        string querydelete = "DELETE FROM tbPrintPending_temp WHERE Code = @code AND Supplier = @supplier";
                        SqlCommand cmd = new SqlCommand(querydelete, con.con);
                        cmd.Parameters.AddWithValue("@code", code);
                        cmd.Parameters.AddWithValue("@supplier", supplier);
                        cmd.ExecuteNonQuery();
                        success++;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while deteting data !" + ex.Message, "Error delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (success > 0)
                    {
                        dgvTTL.Rows.RemoveAt(rowIndex);
                        btnDeletColor.Enabled = false;
                        btnDeletColor.SendToBack();
                        dgvTTL.ClearSelection();
                    }
                    con.con.Close();

                }
            }
        }
        private void DgvTTL_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >=0)
            {
                btnDeletColor.Enabled = true;
                btnDeletColor.BringToFront();
                btnUpdate.Enabled = true;
                btnUpdate.BringToFront();
            }
        }
        private void BtnAddColor_Click(object sender, EventArgs e)
        {
            AddPrint ad = new AddPrint(dgvTTL);
            ad.ShowDialog();
            Search();
        }
        private void ExportToExcelFromTemplate()
        {
            if (dgvTTL.Rows.Count == 0)
            {
                MessageBox.Show("No data to export!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Cursor = Cursors.WaitCursor;
            try
            {
                // Ensure folder exists
                string SavePath = Path.Combine(Environment.CurrentDirectory, @"Report\SparePartRequestSheetIPO");
                Directory.CreateDirectory(SavePath);

                // Open Excel template
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(
                    Path.Combine(Environment.CurrentDirectory, @"Template\SparePartRequestSheetIPO.xlsx"), Editable: true);
                Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets[1];

                int startRow = 4;

                // Fill Excel from DataGridView
                string date = DateTime.Now.ToString("dd");
                string month = DateTime.Now.ToString("MM");
                string year = DateTime.Now.ToString("yyyy");
                for (int i = 0; i < dgvTTL.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvTTL.Rows[i];

                    if (i > 0)
                    {
                        // Insert new row with same format
                        Excel.Range sourceRow = worksheet.Rows[startRow];
                        sourceRow.Copy();

                        Excel.Range insertRow = worksheet.Rows[startRow + i];
                        insertRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                    }

                    worksheet.Cells[startRow + i, 1] = row.Cells["Code"].Value?.ToString();
                    worksheet.Cells[startRow + i, 2] = row.Cells["partno"].Value?.ToString();
                    worksheet.Cells[startRow + i, 3] = row.Cells["partname"].Value?.ToString();
                    worksheet.Cells[startRow + i, 4] = row.Cells["machinename"].Value?.ToString();
                    worksheet.Cells[startRow + i, 5] = row.Cells["supplier"].Value?.ToString();
                    worksheet.Cells[startRow + i, 6] = row.Cells["maker"].Value?.ToString();
                    worksheet.Cells[startRow + i, 7] = row.Cells["qty"].Value?.ToString();
                    worksheet.Cells[startRow + i, 8] = row.Cells["unitprice"].Value?.ToString();
                    worksheet.Cells[startRow + i, 9] = row.Cells["amount"].Value?.ToString();

                }
                // Save Excel

                string DateExcel = DateTime.Now.ToString("yyMMdd");
                string fileName = "Unit Price Request " + nextdocno + ").xlsx";

                string fullPath = Path.Combine(SavePath, fileName);

                worksheet.Cells[2, 10] = nextdocno;

                xlWorkBook.SaveAs(fullPath);

                // Cleanup
                excelApp.DisplayAlerts = false;
                xlWorkBook.Close();
                excelApp.Quit();
                excelApp.DisplayAlerts = true;

                // Release COM objects to avoid leaving Excel.exe open
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                MessageBox.Show("Print Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(fullPath);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("File excel នេះកំពុងបើក, សូមបិទជាមុនសិន​ រួច Print ម្ដងទៀត!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Cursor = Cursors.Default;
        }
        private void ExportToExcelFromTemplateMTH()
        {
            if (dgvTTL.Rows.Count == 0)
            {
                MessageBox.Show("No data to export!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Cursor = Cursors.WaitCursor;
            try
            {
                // Ensure folder exists
                string SavePath = Path.Combine(Environment.CurrentDirectory, @"Report\SparePartRequestSheetMTH");
                Directory.CreateDirectory(SavePath);

                // Open Excel template
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(
                    Path.Combine(Environment.CurrentDirectory, @"Template\SparePartRequestSheetMTH.xlsx"), Editable: true);
                Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets[1];

                int startRow = 4;

                // Fill Excel from DataGridView
                string date = DateTime.Now.ToString("dd");
                string month = DateTime.Now.ToString("MM");
                string year = DateTime.Now.ToString("yyyy");
                for (int i = 0; i < dgvTTL.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvTTL.Rows[i];

                    if (i > 0)
                    {
                        // Insert new row with same format
                        Excel.Range sourceRow = worksheet.Rows[startRow];
                        sourceRow.Copy();

                        Excel.Range insertRow = worksheet.Rows[startRow + i];
                        insertRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                    }

                    worksheet.Cells[startRow + i, 1] = row.Cells["Code"].Value?.ToString();
                    worksheet.Cells[startRow + i, 2] = row.Cells["partno"].Value?.ToString();
                    worksheet.Cells[startRow + i, 3] = row.Cells["partname"].Value?.ToString();
                    worksheet.Cells[startRow + i, 4] = row.Cells["machinename"].Value?.ToString();
                    worksheet.Cells[startRow + i, 5] = row.Cells["supplier"].Value?.ToString();
                    worksheet.Cells[startRow + i, 6] = row.Cells["maker"].Value?.ToString();
                    worksheet.Cells[startRow + i, 7] = row.Cells["qty"].Value?.ToString();
                    worksheet.Cells[startRow + i, 8] = row.Cells["unitprice"].Value?.ToString();
                    worksheet.Cells[startRow + i, 9] = row.Cells["amount"].Value?.ToString();

                }
                // Save Excel
                string nextDocNo = nextdocno;
                string DateExcel = DateTime.Now.ToString("yyMMdd");
                string fileName = "Unit Price Request " + nextDocNo + ".xlsx";

                string fullPath = Path.Combine(SavePath, fileName);


                worksheet.Cells[2, 10] = nextDocNo;
                xlWorkBook.SaveAs(fullPath);

                // Cleanup
                excelApp.DisplayAlerts = false;
                xlWorkBook.Close();
                excelApp.Quit();
                excelApp.DisplayAlerts = true;

                // Release COM objects to avoid leaving Excel.exe open
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                MessageBox.Show("Print Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(fullPath);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("File excel នេះកំពុងបើក, សូមបិទជាមុនសិន​ រួច Print ម្ដងទៀត!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Cursor = Cursors.Default;
        }
        private string GetNextDocNo()
        {
            string currentDocNo = "";
            // Build prefix: Dept + YYMMDD
            string currentDateString = dept + DateTime.Now.ToString("yyMMdd");
            Cursor = Cursors.WaitCursor;
            try
            {
                DataTable docno = new DataTable();
                con.con.Open();
                string query = "SELECT MAX(DocNo) AS DocNo FROM MCDocNo WHERE DocNo LIKE '%" + currentDateString + "%' ";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(docno);
                con.con.Close();
                string docnoLast = docno.Rows[0][0] == DBNull.Value ? null : docno.Rows[0][0].ToString();
                if (docnoLast == null)
                {
                    currentDocNo = currentDateString + "01";
                }
                else
                {
                    string lastIndex = docnoLast.Substring(docnoLast.Length - 2);
                    int NewIndex = Convert.ToInt32(lastIndex) + 1;
                    string NewIndexStr = NewIndex.ToString("D2");
                    currentDocNo = currentDateString + NewIndexStr;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while give Docno !" + ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor = Cursors.Default;
            return currentDocNo;
        }
        private void Search()
        {
            DataTable dtcond = new DataTable();
            dtcond.Columns.Add("Cond");
            if (chkcode.Checked == true)
            {
                if (!string.IsNullOrEmpty(txtcode.Text))
                {
                    dtcond.Rows.Add("Code LIKE '%" +txtcode.Text+ "%'");
                }
            }
            if (chkPname.Checked == true)
            {
                if (!string.IsNullOrEmpty(txtPname.Text))
                {
                    dtcond.Rows.Add("PartName LIKE '%" + txtPname.Text + "%'");
                }
            }
            if (chkPno.Checked == true)
            {
                if (!string.IsNullOrEmpty(txtPno.Text))
                {
                    dtcond.Rows.Add("PartNo LIKE '%" + txtPno.Text + "%'");
                }
            }
            string WHERE = "";
            foreach (DataRow dr in dtcond.Rows)
            {
                if (WHERE.Trim() == "")
                {
                    WHERE = " AND " + dr["Cond"].ToString();
                }
                else
                {
                    WHERE += " AND " + dr["Cond"].ToString();
                }
            }

            dgvTTL.Rows.Clear();
            Cursor = Cursors.WaitCursor;
            DataTable dt = new DataTable();
            try
            {
                con.con.Open();
                string query = " SELECT * FROM tbPrintPending_temp WHERE Dept = '" + dept + "'" + WHERE;
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while select data pending" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.con.Close();

            foreach (DataRow row in dt.Rows)
            {
                DateTime? seteta = string.IsNullOrEmpty(row["SetETA"]?.ToString())
                 ? (DateTime?)null
                 : Convert.ToDateTime(row["SetETA"]);
                string machinename = row["MCName"]?.ToString() ?? string.Empty;
                string code = row["Code"]?.ToString() ?? string.Empty;
                string pno = row["PartNo"]?.ToString() ?? string.Empty;
                string pname = row["PartName"]?.ToString() ?? string.Empty;
                string supplier = row["Supplier"]?.ToString() ?? string.Empty;
                string Leadtime = row["ETA"]?.ToString() ?? string.Empty;
                int ld = Convert.ToInt32(Leadtime);
                int totalmonth = ld / 4;
                DateTime lead = DateTime.Now.AddMonths(totalmonth);
                double orderqty = row["OrderQty"] == DBNull.Value ? 0 : Convert.ToDouble(row["OrderQty"]);
                double unitprice = row["UnitPrice"] == DBNull.Value ? 0 : Convert.ToDouble(row["UnitPrice"]);
                double amount = row["Amount"] == DBNull.Value ? 0 : Convert.ToDouble(row["Amount"]);
                string status = row["Status"]?.ToString() ?? string.Empty;
                string maker = row["Maker"]?.ToString() ?? string.Empty;
                dgvTTL.Rows.Add();
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["status"].Value = status;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["Code"].Value = code;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["partno"].Value = pno;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["partname"].Value = pname;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["machinename"].Value = machinename;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["supplier"].Value = supplier;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["maker"].Value = maker;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["qty"].Value = orderqty;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["unitprice"].Value = unitprice;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["amount"].Value = amount;
                dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["seteta"].Value = lead.ToString("MM-yyyy");
                if (seteta != null)
                {
                    DateTime now = DateTime.Now;
                    DateTime a = Convert.ToDateTime(seteta);
                    int monthDiff = (a.Year - now.Year) * 12 + (a.Month - now.Month);
                    int LT = monthDiff * 4;
                    dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["eta"].Value = a.ToString("MM-yyyy");
                    dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["leadtime"].Value = LT;
                }
                else
                {
                    dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["eta"].Value = lead.ToString("MM-yyyy");
                    dgvTTL.Rows[dgvTTL.Rows.Count - 1].Cells["leadtime"].Value = Leadtime;
                }

            }
            btnSave.Enabled = false;
            btnSaveGrey.BringToFront();
            dgvTTL.ClearSelection();
            Cursor = Cursors.Default;
            btnDeletColor.Enabled = false;
            btndelete.BringToFront();
            btnUpdate.Enabled = false;
            btnUpdateGrey.BringToFront();
            foreach (DataGridViewRow row in dgvTTL.Rows)
            {
                row.Cells["qty"].ReadOnly = true;
                row.Cells["amount"].ReadOnly = true;
                row.Cells["unitprice"].ReadOnly = true;
                row.Cells["eta"].ReadOnly = true;
                row.Cells["leadtime"].ReadOnly = true;
                row.Cells["eta"].Style.ForeColor = Color.Black;
                row.Cells["qty"].Style.ForeColor = Color.Black;
                row.Cells["unitprice"].Style.ForeColor = Color.Black;
                row.Cells["leadtime"].Style.ForeColor = Color.Black;
            }
        }
        private void delete()
        {
            int success = 0;
            try
            {
                con.con.Open();
                foreach (DataGridViewRow row in dgvTTL.Rows)
                {
                    string code = row.Cells["Code"].Value.ToString();
                    string querydelete = "DELETE FROM tbPrintPending_temp WHERE Code = @code";
                    SqlCommand cmd = new SqlCommand(querydelete, con.con);
                    cmd.Parameters.AddWithValue("@code", code);
                    cmd.ExecuteNonQuery();
                    success++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while deteting data !" + ex.Message, "Error delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.con.Close();
            Search();
        }
    }
}
