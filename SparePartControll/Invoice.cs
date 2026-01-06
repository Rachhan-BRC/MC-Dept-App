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
        string dept = "MC";
        public InvoiceForm()
        {
            this.con.Connection();
            InitializeComponent();
            this.Load += InvoiceForm_Load;
            this.btnImport.Click += BtnImport_Click;
            this.btnSave.Click += BtnSave_Click;
            this.dgvInvoice.CellClick += DgvInvoice_CellClick;
            this.dgvPo.CellClick += DgvPo_CellClick;
            this.btnDelete.Click += BtnDelete_Click;

        }

        private void InvoiceForm_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
        }

        private void DgvPo_CellClick(object sender, DataGridViewCellEventArgs e)
        {       
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                btnDelete.Enabled = true;
                btnDelete.BringToFront();
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPo)
            {
                int rowIndex1 = dgvPo.CurrentCell.RowIndex;
                if (rowIndex1 >= 0 && rowIndex1 < dgvPo.Rows.Count)
                {
                    dgvPo.Rows.RemoveAt(rowIndex1);
                    btnDelete.Enabled = false;
                    btnDelete.SendToBack();
                    dgvPo.ClearSelection();
                }
            }
            if (tabControl1.SelectedTab == tabInvoice)
            {
                int rowIndex = dgvInvoice.CurrentCell.RowIndex;
                if (rowIndex >= 0 && rowIndex < dgvInvoice.Rows.Count)
                {
                    dgvInvoice.Rows.RemoveAt(rowIndex);
                    btnDelete.Enabled = false;
                    btnDelete.SendToBack();
                    dgvInvoice.ClearSelection();
                }
            }
        }
        private void DgvInvoice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                btnDelete.Enabled = true;
                btnDelete.BringToFront();
            }
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            DialogResult save = MessageBox.Show("Do you want to save data to database?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (save == DialogResult.No)
            {
                return;
            }
            if (tabControl1.SelectedTab == tabInvoice)
            {
                SaveToDBInvoice();
            }
            if (tabControl1.SelectedTab == tabPo)
            {
                SaveToDbPo();
            }
        }
        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabInvoice)
            {
                ImportExcelToGrid(dgvInvoice);
            }
            if (tabControl1.SelectedTab == tabPo)
            {
                ImportExcelToGrid(dgvPo);
            }
        }
        private void ImportExcelToGrid(DataGridView dgv)
        {

            dgv.Rows.Clear();

            OpenFileDialog openFD = new OpenFileDialog
            {
                Title = "Choose an Excel file for Import",
                Filter = "Excel Files|*.xls;*.xlsx;"
            };

            if (openFD.ShowDialog() != DialogResult.OK) return;

            string strFileName = openFD.FileName;

            Microsoft.Office.Interop.Excel.Application xlApp = null;
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = null;
            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = null;
            Microsoft.Office.Interop.Excel.Range xlRange = null;

            int error = 0;
            int cannot = 0;
            int errCode = 0;
            int errDoc = 0;
            int errDate = 0;
            int bigger = 0;
            try
            {
                lbshow.Text = "សូមរង់ចាំ...(please wait)";
                Cursor = Cursors.WaitCursor;
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkbook = xlApp.Workbooks.Open(strFileName);
                xlWorksheet = xlWorkbook.Worksheets[1];
                xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;
               
                dgv.ResumeLayout();
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                // --- Fill rows starting from row 2 ---
                string now = DateTime.Now.ToString("yyyy-MM-dd");
                if (tabControl1.SelectedTab == tabInvoice)
                {
                    int startRow = 13; // Assuming data starts from row 12
                    for (int i = startRow; i <= rowCount; i++)
                    {
                        string description = Convert.ToString((xlRange.Cells[i, 1] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();
                        string code1 = Convert.ToString((xlRange.Cells[i, 2] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();
                        string partno = Convert.ToString((xlRange.Cells[i, 3] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();
                        string occ = Convert.ToString((xlRange.Cells[i, 4] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();
                        string pono = Convert.ToString((xlRange.Cells[i, 5] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();
                        string qty = Convert.ToString((xlRange.Cells[i, 6] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();
                        string unit = Convert.ToString((xlRange.Cells[i, 7] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();
                        string price = Convert.ToString((xlRange.Cells[i, 8] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();
                        string amount = Convert.ToString((xlRange.Cells[i, 9] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();
                        string invoiceno = Convert.ToString((xlRange.Cells[i, 10] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();

                        if (!string.IsNullOrEmpty(description) &&
                            !string.IsNullOrEmpty(code1) &&
                            !string.IsNullOrEmpty(partno) &&
                            !string.IsNullOrEmpty(qty) &&
                            !string.IsNullOrEmpty(unit) &&
                            !string.IsNullOrEmpty(price) &&
                            !string.IsNullOrEmpty(amount))
                        {
                            dgv.Rows.Add();
                            dgv.Rows[dgv.Rows.Count - 1].Cells["description"].Value = description;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["code"].Value = code1;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["Pno"].Value = partno;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["ooc"].Value = occ;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["pono"].Value = pono;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["qty"].Value = qty;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["unit"].Value = unit;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["priceusd"].Value = price;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["amountusd"].Value = amount;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["invoicedate"].Value = now;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["invoiceno1"].Value = invoiceno;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["find"].Value = "Not";
                        }
                    }
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
                        lbshow.Text = "Error: " + ex.Message;
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
                        lbshow.Text = "Error: " + ex.Message;
                        error++;
                    }
                    con.con.Close();
                    if (dtselect.Rows.Count <= 0)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("This spare part not have in request", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    foreach (DataGridViewRow row1 in dgv.Rows)
                    {
                        string code1 = row1.Cells["code"].Value.ToString();
                        string pono = row1.Cells["pono"].Value.ToString();
                        double qty = Convert.ToDouble(row1.Cells["qty"].Value);
                        foreach (DataRow row in dtselect.Rows)
                        {
                            double bal = Convert.ToDouble(row["Balance"]);
                            string pono1 = row["PO_No"].ToString();
                            string code2 = row["Code"].ToString();  
                            if (code1 == code2)
                            {
                                row1.Cells["find"].Value = "Pono";
                            }
                            if (pono == pono1)
                            {
                                row1.Cells["find"].Value = "Code";
                            }
                         
                            if (code1 == code2 && pono == pono1)
                            {
                                if (qty > bal)
                                {
                                    row1.Cells["find"].Value = "Qty";
                                }
                                else
                                {
                                    row1.Cells["find"].Value = "Have";
                                }
                            }
                        }
                    }
                    foreach (DataGridViewRow row1 in dgv.Rows)
                    {
                        if (row1.Cells["find"].Value?.ToString() == "Not")
                        {
                            row1.Cells["code"].Style.BackColor = Color.LightPink;
                            row1.Cells["pono"].Style.BackColor = Color.LightPink;
                            cannot++;
                        }
                        if (row1.Cells["find"].Value?.ToString() == "Code")
                        {
                            row1.Cells["code"].Style.BackColor = Color.LightPink;
                            errCode++;
                        }
                        if (row1.Cells["find"].Value?.ToString() == "Pono")
                        {
                            row1.Cells["pono"].Style.BackColor = Color.LightPink;
                            errDoc++;
                        }
                        if (row1.Cells["find"].Value?.ToString() == "Qty")
                        {
                            row1.Cells["qty"].Style.BackColor = Color.LightPink;
                            bigger++;
                        }
                    }
                    if (errCode > 0)
                    {
                        btnSave.Enabled = false;
                        btnSave.SendToBack();
                        lbshow.Text = "Code not have in request.";
                    }
                    if (errDoc > 0)
                    {
                        btnSave.Enabled = false;
                        btnSave.SendToBack();
                        lbshow.Text = "Po No. not have in request.";
                    }
                    if (bigger >0)
                    {

                        btnSave.Enabled = false;
                        btnSave.SendToBack();
                        lbshow.Text = "Receive Qty bigger than Order Qty.";
                    }
                    if (cannot > 0)
                    {
                        btnSave.Enabled = false;
                        btnSave.SendToBack();
                        lbshow.Text = "Po No and Code not have in request.";
                    }
                    if (error == 0 && dgv.Rows.Count > 0 && cannot == 0 && errCode == 0 && errDoc == 0 && bigger ==0 )
                    {
                        btnSave.Enabled = true;
                        btnSave.BringToFront();
                        lbfound.Text = "Found : " + dgv.Rows.Count;
                        lbshow.Text = "រួចរាល់ (Done)";
                    }
                }
                if (tabControl1.SelectedTab == tabPo)
                {
                    int startRow = 2; 
                    for (int i = startRow; i <= rowCount; i++)
                    {
                        string pono = Convert.ToString((xlRange.Cells[i, 1] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();
                        string code = Convert.ToString((xlRange.Cells[i, 2] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();
                        string name = Convert.ToString((xlRange.Cells[i, 3] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();
                        string plan = Convert.ToString((xlRange.Cells[i, 4] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();
                        string remark = Convert.ToString((xlRange.Cells[i, 5] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();

                        if (!string.IsNullOrEmpty(pono) &&
                            !string.IsNullOrEmpty(code) &&
                            !string.IsNullOrEmpty(name) &&
                            !string.IsNullOrEmpty(plan))
                        {
                            dgv.Rows.Add();
                            dgv.Rows[dgv.Rows.Count - 1].Cells["pono1"].Value = pono;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["itemcode"].Value = code;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["docno"].Value = name;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["plan"].Value = plan;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["remark1"].Value = remark;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["find1"].Value = "Not";
                        }
                    }
                    try
                    {
                        Cursor = Cursors.WaitCursor;
                        DataTable dtselect = new DataTable();
                        List<string> code = new List<string>();
                        foreach (DataGridViewRow row in dgvPo.Rows)
                        {
                            code.Add(row.Cells["itemcode"].Value.ToString());
                        }
                        string codelist = "('" + string.Join("','", code) + "')";
                        //select from request
                        try
                        {

                            con.con.Open();
                            string query = "SELECT * FROM MCSparePartRequest WHERE Dept = '" + dept + "' AND Code IN " + codelist + " AND Order_State <> 'Completed' ORDER BY Code";
                            SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                            sda.Fill(dtselect);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error while selecting table request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            lbshow.Text = "Error: " + ex.Message;
                        }
                        con.con.Close();

                        if (dtselect.Rows.Count <= 0)
                        {
                            lbshow.Text = " Spare part not have in request ";
                            MessageBox.Show("This spare part not have in request", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Cursor = Cursors.Default;
                            return;
                        }
                        foreach (DataGridViewRow row1 in dgv.Rows)
                        {
                            string code1 = row1.Cells["itemcode"].Value?.ToString() ?? "";
                            string docno = row1.Cells["docno"].Value?.ToString() ?? "";
                            string raw = Convert.ToString(row1.Cells["plan"].Value);

                          
                            foreach (DataRow row in dtselect.Rows)
                            {
                                string code2 = row["Code"] == DBNull.Value ? string.Empty : row["Code"].ToString();
                                string docno2 = row["PO_No"] == DBNull.Value ? string.Empty : row["PO_No"].ToString();
                                if (code1 == code2)
                                {
                                    row1.Cells["find1"].Value = "Docno";
                                }
                                if (docno == docno2)
                                {
                                    row1.Cells["find1"].Value = "Code";
                                }
                                if (DateTime.TryParseExact(raw, "dd-MM-yy",
System.Globalization.CultureInfo.InvariantCulture,
System.Globalization.DateTimeStyles.None,
out DateTime plan1))
                                {

                                }
                                else
                                {
                                    row1.Cells["find1"].Value = "Date";
                                }

                                if (code1 == code2 && docno == docno2 && DateTime.TryParseExact(raw, "dd-MM-yy",
                          System.Globalization.CultureInfo.InvariantCulture,
                          System.Globalization.DateTimeStyles.None,
                          out DateTime plan))
                                {
                                    row1.Cells["find1"].Value = "Have";
                                    break;
                                }
                            
                            }
                        }
                        foreach (DataGridViewRow row1 in dgv.Rows)
                        {
                            if (row1.Cells["find1"].Value?.ToString() == "Not")
                            {
                                cannot++;
                            }
                            if (row1.Cells["find1"].Value?.ToString() == "Code")
                            {
                                row1.Cells["itemcode"].Style.BackColor = Color.LightPink;
                                errCode++;  
                            }
                            if (row1.Cells["find1"].Value?.ToString() == "Docno")
                            {
                                row1.Cells["docno"].Style.BackColor = Color.LightPink;
                                errDoc++;
                            }
                            if (row1.Cells["find1"].Value?.ToString() == "Date")
                            {
                                row1.Cells["plan"].Style.BackColor = Color.LightPink;
                                errDate++;
                            }
                        }
                    }
                    catch
                    {
                        error++;
                    }
                    if (errCode > 0)
                    {
                        btnSave.Enabled = false;
                        btnSave.SendToBack();
                        lbshow.Text = "Code not have in request.";
                    }
                    if (errDoc > 0)
                    {
                        btnSave.Enabled = false;
                        btnSave.SendToBack();
                        lbshow.Text = "Document No. not have in request.";
                    }
                    if (cannot > 0)
                    {
                        btnSave.Enabled = false;
                        btnSave.SendToBack();
                        lbshow.Text = "Document No And Code not have in request !";
                    }
                    if (error == 0 && dgv.Rows.Count > 0 && cannot == 0 && errCode == 0 && errDoc == 0 && errDate == 0)
                    {
                        btnSave.Enabled = true;
                        btnSave.BringToFront();
                        lbfound.Text = "Found : " + dgv.Rows.Count;
                        lbshow.Text = "រួចរាល់ (Done)";
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                lbshow.Text = "Error: " + ex.Message;
            }
            finally
            {
                if (xlWorkbook != null) xlWorkbook.Close(false);
                if (xlApp != null) xlApp.Quit();
                if (xlRange != null) Marshal.ReleaseComObject(xlRange);
                if (xlWorksheet != null) Marshal.ReleaseComObject(xlWorksheet);
                if (xlWorkbook != null) Marshal.ReleaseComObject(xlWorkbook);
                if (xlApp != null) Marshal.ReleaseComObject(xlApp);

                xlRange = null;
                xlWorksheet = null;
                xlWorkbook = null;
                xlApp = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            dgv.ClearSelection();
            Cursor = Cursors.Default;
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
                string query = "SELECT * FROM MCSparePartRequest WHERE Dept = '" +dept+"' AND Code IN " + codelist + "ORDER BY Code";
                SqlDataAdapter sda =  new SqlDataAdapter(query, con.con);
                sda.Fill(dtselect);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while selecting table request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lbshow.Text = "Error: " + ex.Message;
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
                lbshow.Text = "Error: " + ex.Message;
                error++;
            }
            con.con.Close();
            if (dtselect.Rows.Count <= 0)
            {
                MessageBox.Show("This spare part not have in request", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Cursor = Cursors.Default;
                return;
            }
            //Save to db
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
                        if (CodeIN == CodeSelect && ponoIN == ponoSelect)
                        {
                            if (recqty > balance)
                            {
                                MessageBox.Show("Receive Qty cannot bigger than balance !", "Please check.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                con.con.Close();
                                Cursor = Cursors.Default;
                                return;
                            }
                            //Update
                            try
                            {
                                con.con.Open();
                                string query = "UPDATE MCSparePartRequest SET ReceiveQTY =  @recqty, Balance = @balance, RemainAmount = @reamount, Receive_Date = @recdate, Order_State = @orderstate, " +
                            " Remark = @remark, UpdateDate = @update " +
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
                                cmd.Parameters.AddWithValue("@pono", ponoSelect);
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
                    string invoice = row1.Cells["invoiceno1"]?.Value?.ToString() ?? "";
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
                btnDeleteGray.BringToFront();
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
        private void SaveToDbPo()
        {
            int success = 0;
            DataTable dtselect = new DataTable();
            Cursor = Cursors.WaitCursor;
            List<string> code = new List<string>();
            int cannot = 0;
            DateTime now = DateTime.Now;
            foreach (DataGridViewRow row in dgvPo.Rows)
            {               
                code.Add(row.Cells["itemcode"].Value.ToString());
            }
            string codelist = "('" + string.Join("','", code) + "')";
            //select from request
            try
            {
                con.con.Open();
                string query = "SELECT * FROM MCSparePartRequest WHERE Dept = '" + dept + "' AND Code IN " + codelist + " AND Order_State <> 'Completed' ORDER BY Code";
                Console.WriteLine(query);
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dtselect);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while selecting table request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lbshow.Text = "Error: " + ex.Message;
            }
            con.con.Close();
            
            if (dtselect.Rows.Count <= 0)
            {
                MessageBox.Show("This spare part not have in request", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cannot++;
                return;
            }
            if (cannot == 0)
            {
                try
                {
                    con.con.Open();
                    foreach (DataGridViewRow row in dgvPo.Rows)
                    {
                        string codePo = row.Cells["itemcode"].Value?.ToString();
                        string pono = row.Cells["pono1"].Value?.ToString();
                        string docno = row.Cells["docno"].Value?.ToString();
                        DateTime plan = Convert.ToDateTime(row.Cells["plan"].Value);
                        foreach (DataRow row2 in dtselect.Rows)
                        {
                            string codeselect = row2["Code"] == DBNull.Value ? string.Empty : row2["Code"].ToString();
                            string docno2 = row2["PO_No"] == DBNull.Value ? string.Empty : row2["PO_No"].ToString();
                            string cond = row2["Order_State"] == DBNull.Value ? string.Empty : row2["Order_State"].ToString();
                            DateTime plan2 = row2["Receive_Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row2["ETA"]);
                            if (codePo == codeselect && docno == docno2 && cond != "Completed")
                            {
                                string query = "UPDATE MCSparePartRequest SET PO_No = @pono, ETA = @eta, Receive_Date = @recdate, Remark = @remark, UpdateDate = @update WHERE Code = @code AND Order_State <> 'Completed' AND Dept = @dept";
                                SqlCommand cmd = new SqlCommand(query, con.con);
                                cmd.Parameters.AddWithValue("@pono", pono);
                                cmd.Parameters.AddWithValue("@eta", plan);
                                cmd.Parameters.AddWithValue("@recdate", plan);
                                cmd.Parameters.AddWithValue("@remark", "Updated by Minamikawa san on " + now.ToString("dd-MM-yyyy"));
                                cmd.Parameters.AddWithValue("@update", now);
                                cmd.Parameters.AddWithValue("@code", codeselect);
                                cmd.Parameters.AddWithValue("@dept", dept);
                                cmd.ExecuteNonQuery();
                                success++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while updating po " + ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (success > 0)
                {
                    dgvPo.Rows.Clear();
                    btnSave.Enabled = false;
                    btnSaveGrey.BringToFront();
                    MessageBox.Show("Save successfully ! ", "Done.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Wrong Document No or Code ! ", "Failed.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
               lbshow.Text = "Wrong Document No or Code";
            }
            con.con.Close();
            Cursor = Cursors.Default;
        }
    }
}
