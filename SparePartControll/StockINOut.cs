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
using System.Web;
using System.Windows.Forms;

namespace MachineDeptApp
{

    public partial class StockINOut : Form
    {
        SQLConnect con = new SQLConnect();
        string dept = "MC";
        public StockINOut()
        {
            con.Connection();
            InitializeComponent();
            this.btnImport.Click += BtnImport_Click;
            this.dgvIn.CellClick += DgvIn_CellClick;
            this.dgvOut.CellClick += DgvOut_CellClick;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabIN)
            {
                if (dgvIn.Rows.Count <= 0)
                {
                    MessageBox.Show("Please Import data first !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult ask = MessageBox.Show("Are you sure you want to save this data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask == DialogResult.No)
                {
                    return;
                }
                SaveToDB(dgvIn);
            }
            if (tabControl1.SelectedTab == tabOut)
            {
                if (dgvOut.Rows.Count <= 0)
                {
                    MessageBox.Show("Please Import data first !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult ask = MessageBox.Show("Are you sure you want to save this data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask == DialogResult.No)
                {
                    return;
                }
                SaveToDB(dgvOut);
            }
        }

        private void DgvOut_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                btnDelete.Enabled = true;
                btnDelete.BringToFront();
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
           if (tabControl1.SelectedTab == tabIN)
            {
                int rowIndex = dgvIn.CurrentCell.RowIndex;
                if (rowIndex >= 0 && rowIndex < dgvIn.Rows.Count)
                {
                    dgvIn.Rows.RemoveAt(rowIndex);
                    btnDelete.Enabled = false;
                    btnDelete.SendToBack();
                    dgvIn.ClearSelection();
                    afterdelete(dgvIn);
                }
            }
           if (tabControl1.SelectedTab == tabOut)
            {
                int rowIndex = dgvOut.CurrentCell.RowIndex;
                if (rowIndex >= 0 && rowIndex < dgvOut.Rows.Count)
                {
                    dgvOut.Rows.RemoveAt(rowIndex);
                    btnDelete.Enabled = false;
                    btnDelete.SendToBack();
                    dgvOut.ClearSelection();
                    afterdelete(dgvOut);
                }
            }
        }
        private void DgvIn_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                btnDelete.Enabled = true;
                btnDelete.BringToFront();
            }
        }
        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabIN)
            {
                ImportExcel(dgvIn);
                lbfound1.Text = "Found: " + dgvIn.Rows.Count;
                dgvIn.ClearSelection();
            }
            if (tabControl1.SelectedTab ==tabOut)
            {
                ImportExcel(dgvOut);
                lbfound2.Text = "Found: " + dgvOut.Rows.Count;
                dgvOut.ClearSelection();
            }

        }
        private void SaveToDB(DataGridView dgv)
        {
          if (tabControl1.SelectedTab == tabIN)
            {
                Cursor = Cursors.WaitCursor;
                int success = 0;
                try
                {
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        string Code = row.Cells["code"]?.Value?.ToString() ?? "";
                        string partnumber = row.Cells["partnumber"]?.Value?.ToString() ?? "";
                        string partname = row.Cells["partname"]?.Value?.ToString() ?? "";
                        double recqty = double.TryParse(row.Cells["Qty"]?.Value?.ToString(), out double val) ? val : 0;
                        DateTime now = DateTime.Now;
                        string tranNo = "TR-A0000001";
                        try
                        {
                            DataTable dtTran = new DataTable();
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

                            Cursor = Cursors.Default;
                            return;
                        }
                        con.con.Close();
                        try
                        {
                            con.con.Open();
                            string query = "INSERT INTO SparePartTrans (TransNo, Code, Part_No, Part_Name, Dept, Stock_In, Stock_Out, Stock_Value, Stock_Amount, Status, RegDate, PIC, Remark) " +
                                                                                  " VALUES (@TransNo, @code, @partno, @partname, @dept, @stockIn, @stockOut, @stockVal, @stockAmount, @status, @Rd, @pic, @remark )";

                            SqlCommand cmd = new SqlCommand(query, con.con);
                            cmd.Parameters.AddWithValue("@TransNo", tranNo);
                            cmd.Parameters.AddWithValue("@code", Code);
                            cmd.Parameters.AddWithValue("@partno", partnumber);
                            cmd.Parameters.AddWithValue("@partname", partname);
                            cmd.Parameters.AddWithValue("@dept", dept);
                            cmd.Parameters.AddWithValue("@stockIn", recqty);
                            cmd.Parameters.AddWithValue("@stockOut", 0);
                            cmd.Parameters.AddWithValue("@stockVal", recqty);
                            cmd.Parameters.AddWithValue("@stockAmount", 0);
                            cmd.Parameters.AddWithValue("@status", 1);
                            cmd.Parameters.AddWithValue("@Rd", now);
                            cmd.Parameters.AddWithValue("@pic", MenuFormV2.UserForNextForm);
                            cmd.Parameters.AddWithValue("@remark", "Update: " + now.ToString("dd-MM-yyyy"));
                            cmd.ExecuteNonQuery();
                            success++;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error while insert" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            Cursor = Cursors.Default;
                            return;
                        }
                        con.con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while inserting data!" + ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Cursor = Cursors.Default;
                    return;
                }
                if (success > 0)
                {
                    MessageBox.Show("Save successfully !", "Done..", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgv.Rows.Clear();
                    lbshow.Text = "Save successfully !";
                }
                con.con.Close();
                Cursor = Cursors.Default;
            }
          if (tabControl1.SelectedTab == tabOut)
            {
                Cursor = Cursors.WaitCursor;
                int success = 0;
                try
                {
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        string Code = row.Cells["code2"]?.Value?.ToString() ?? "";
                        string partnumber = row.Cells["partnumber2"]?.Value?.ToString() ?? "";
                        string partname = row.Cells["partname2"]?.Value?.ToString() ?? "";
                        double recqty = double.TryParse(row.Cells["Qty2"]?.Value?.ToString(), out double val) ? val : 0;
                        DateTime now = DateTime.Now;
                        string tranNo = "TR-A0000001";
                        try
                        {
                            DataTable dtTran = new DataTable();
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

                            Cursor = Cursors.Default;
                            return;
                        }
                        con.con.Close();
                        try
                        {
                            con.con.Open();
                            string query = "INSERT INTO SparePartTrans (TransNo, Code, Part_No, Part_Name, Dept, Stock_In, Stock_Out, Stock_Value, Stock_Amount, Status, RegDate, PIC, Remark) " +
                                                                                  " VALUES (@TransNo, @code, @partno, @partname, @dept, @stockIn, @stockOut, @stockVal, @stockAmount, @status, @Rd, @pic, @remark )";

                            SqlCommand cmd = new SqlCommand(query, con.con);
                            cmd.Parameters.AddWithValue("@TransNo", tranNo);
                            cmd.Parameters.AddWithValue("@code", Code);
                            cmd.Parameters.AddWithValue("@partno", partnumber);
                            cmd.Parameters.AddWithValue("@partname", partname);
                            cmd.Parameters.AddWithValue("@dept", dept);
                            cmd.Parameters.AddWithValue("@stockIn", 0);
                            cmd.Parameters.AddWithValue("@stockOut", -recqty);
                            cmd.Parameters.AddWithValue("@stockVal", -recqty);
                            cmd.Parameters.AddWithValue("@stockAmount", 0);
                            cmd.Parameters.AddWithValue("@status", 1);
                            cmd.Parameters.AddWithValue("@Rd", now);
                            cmd.Parameters.AddWithValue("@pic", MenuFormV2.UserForNextForm);
                            cmd.Parameters.AddWithValue("@remark", "Update: " + now.ToString("dd-MM-yyyy"));
                            cmd.ExecuteNonQuery();
                            success++;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error while insert" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            Cursor = Cursors.Default;
                            return;
                        }
                        con.con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while inserting data!" + ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Cursor = Cursors.Default;
                    return;
                }
                if (success > 0)
                {
                    MessageBox.Show("Save successfully !", "Done..", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lbshow2.Text = "Save successfully ! ";
                    dgv.Rows.Clear();
                }
                con.con.Close();
                Cursor = Cursors.Default;
            }
        }
        private void ImportExcel(DataGridView dgv)
        {
            DataTable dtMaster = new DataTable();
            int err = 0;
            int err1 = 0;
            int err2 = 0;
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
            try
            {
                dgv.Rows.Clear();
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
                ///Stock In
                if (tabControl1.SelectedTab == tabIN)
                {
                    lbshow.Text = "សូមរង់ចាំ...(please wait)";
                    int startRow = 2;
                    for (int i = startRow; i <= rowCount; i++)
                    {
                        string code = Convert.ToString((xlRange.Cells[i, 1] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();
                        string instockqty = Convert.ToString((xlRange.Cells[i, 2] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();
                        string remark = Convert.ToString((xlRange.Cells[i, 3] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();

                        if (!string.IsNullOrEmpty(code) &&
                            !string.IsNullOrEmpty(instockqty)
                            )
                        {
                            dgv.Rows.Add();
                            dgv.Rows[dgv.Rows.Count -1].Cells["code"].Value = code;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["Qty"].Value = instockqty;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["remark"].Value = remark;
                        }
                    }
                    if (dgv.Rows.Count == 0)
                    {
                        MessageBox.Show("No data import, Please check!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Cursor = Cursors.Default;
                        return;
                    }
                    List<string> codelist1 = new List<string>();
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        codelist1.Add(row.Cells["code"].Value.ToString());
                    }
                    string codelist = "('" + string.Join("','", codelist1) + "')";
                    try
                    {
                        con.con.Open();
                        string query = "SELECT Code, Part_No, Part_Name, Maker, Use_For FROM MstMCSparePart WHERE Dept = '" + dept + "' AND Code IN "+ codelist;
                        SqlDataAdapter sda = new SqlDataAdapter(query, con.con); 
                        sda.Fill(dtMaster);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while selecting master" + ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    con.con.Close();
                    foreach (DataGridViewRow row1 in dgv.Rows)
                    {
                        string code1 = row1.Cells["code"]?.Value?.ToString() ?? "";
                        foreach (DataRow row2 in dtMaster.Rows)
                        {
                            string code2 = row2["Code"]?.ToString() ?? "";
                            if (code1 == code2)
                            {
                                row1.Cells["partnumber"].Value = row2["Part_No"].ToString();
                                row1.Cells["partname"].Value = row2["Part_Name"].ToString();
                                row1.Cells["maker"].Value = row2["Maker"].ToString();
                                row1.Cells["machinename"].Value = row2["Use_For"].ToString();
                            }
                        }
                    }
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        string partnumber = row.Cells["partnumber"]?.Value?.ToString() ?? "";
                        string partname = row.Cells["partname"]?.Value?.ToString() ?? "";
                        string maker = row.Cells["maker"]?.Value?.ToString() ?? "";
                        string machinename = row.Cells["machinename"]?.Value?.ToString() ?? "";
                        

                        if (string.IsNullOrEmpty(partname) &&
                            string.IsNullOrEmpty(partnumber)&&
                            string.IsNullOrEmpty(maker) &&
                            string.IsNullOrEmpty(machinename)
                            )
                        {
                            row.Cells["code"].Style.BackColor = Color.LightPink;
                            err++;
                        }
                        if (!double.TryParse(row.Cells["Qty"]?.Value?.ToString(), out double cellValue))
                        {
                            row.Cells["Qty"].Style.BackColor = Color.LightPink;
                            err1++;
                        }
                    }
                }
                //Stock Out
                if (tabControl1.SelectedTab == tabOut)
                {
                    lbshow2.Text = "សូមរង់ចាំ...(please wait)";
                    int startRow = 2;
                    for (int i = startRow; i <= rowCount; i++)
                    {
                        string code = Convert.ToString((xlRange.Cells[i, 1] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();
                        string instockqty = Convert.ToString((xlRange.Cells[i, 2] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();
                        string remark = Convert.ToString((xlRange.Cells[i, 3] as Microsoft.Office.Interop.Excel.Range)?.Text)?.Trim();

                        if (!string.IsNullOrEmpty(code) &&
                            !string.IsNullOrEmpty(instockqty)
                            )
                        {
                            dgv.Rows.Add();
                            dgv.Rows[dgv.Rows.Count - 1].Cells["code2"].Value = code;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["Qty2"].Value = instockqty;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["remark2"].Value = remark;
                        }
                    }
                    if (dgv.Rows.Count == 0)
                    {
                        MessageBox.Show("No data import, Please check!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Cursor = Cursors.Default;
                        return;
                    }
                    List<string> codelist1 = new List<string>();
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        codelist1.Add(row.Cells["code2"].Value.ToString());
                    }
                    string codelist = "('" + string.Join("','", codelist1) + "')";
                    try
                    {
                        con.con.Open();
                        string query = "SELECT tbTr.Code, tbTr.Part_No, tbTr.Part_Name, SUM(tbTr.Stock_Value) AS Balance, tbMst.Use_For, tbMst.Maker FROM SparePartTrans tbTr " +
                            " LEFT JOIN  MstMCSparePart tbMst ON tbTr.Code = tbMst.Code WHERE tbTr.Dept = '"+dept+"' AND tbTr.Code IN " +codelist+ "  Group BY tbTr.Code, TbTr.Part_No, tbTr.Part_Name, tbMst.Use_For, tbMst.Maker  ORDER BY tbTr.Code";
                        SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                        sda.Fill(dtMaster);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while selecting master" + ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    con.con.Close();
                    foreach (DataGridViewRow row1 in dgv.Rows)
                    {
                        string code1 = row1.Cells["code2"]?.Value?.ToString() ?? "";
                        foreach (DataRow row2 in dtMaster.Rows)
                        {
                            string code2 = row2["Code"]?.ToString() ?? "";
                            if (code1 == code2)
                            {
                                row1.Cells["partnumber2"].Value = row2["Part_No"].ToString();
                                row1.Cells["partname2"].Value = row2["Part_Name"].ToString();
                                row1.Cells["maker2"].Value = row2["Maker"].ToString();
                                row1.Cells["machinename2"].Value = row2["Use_For"].ToString();
                                row1.Cells["stockremain2"].Value = row2["Balance"].ToString();
                            }
                        }
                    }
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        string partnumber = row.Cells["partnumber2"]?.Value?.ToString() ?? "";
                        string partname = row.Cells["partname2"]?.Value?.ToString() ?? "";
                        string maker = row.Cells["maker2"]?.Value?.ToString() ?? "";
                        string machinename = row.Cells["machinename2"]?.Value?.ToString() ?? "";
                        double stockout = double.TryParse(row.Cells["Qty2"]?.Value?.ToString(), out double val) ? val : 0;
                        double remain = double.TryParse(row.Cells["stockremain2"]?.Value?.ToString(), out double val1) ? val1 : 0;
                        if (string.IsNullOrEmpty(partname) &&
                            string.IsNullOrEmpty(partnumber) &&
                            string.IsNullOrEmpty(maker) &&
                            string.IsNullOrEmpty(machinename)
                            )
                        {
                            row.Cells["code2"].Style.BackColor = Color.LightPink;
                            err++;
                        }
                        else
                        {
                            if (stockout > remain)
                            {
                                row.Cells["stockremain2"].Style.BackColor = Color.LightPink;
                                row.Cells["Qty2"].Style.BackColor = Color.LightPink;
                                err2++;
                            }
                        }
                        // Highlight qty2 if not a number
                        if (!double.TryParse(row.Cells["Qty2"]?.Value?.ToString(), out double cellValue))
                        {
                            row.Cells["Qty2"].Style.BackColor = Color.LightPink;
                            err1++;
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while read excel: " + ex.Message);
                err++;
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
            if (tabControl1.SelectedTab == tabIN)
            {
                if (err == 0)
                {
                    btnSave.Enabled = true;
                    btnSave.BringToFront();
                    lbshow.Text = "Import success, Please save!";
                }
                else
                {
                    btnSave.Enabled = false;
                    btnSaveGrey.BringToFront();
                    lbshow.Text = "Wrong Code, Please check!";
                }
                if (err1 > 0)
                {
                    btnSave.Enabled = false;
                    btnSaveGrey.BringToFront();
                    lbNum2.Text = "Qty must be number, Please check!";
                }
            }
            if (tabControl1.SelectedTab == tabOut)
            {
                if (err == 0)
                {
                    btnSave.Enabled = true;
                    btnSave.BringToFront();
                    lbshow2.Text = "Import success, Please save!";
                  
                }
                else
                {
                    btnSave.Enabled = false;
                    btnSaveGrey.BringToFront();
                    lbshow2.Text = "Wrong Code or Not have Instock, Please check!";
                }
                if (err1 > 0)
                {
                    btnSave.Enabled = false;
                    btnSaveGrey.BringToFront();
                    lbNum.Text = "Qty must be number, Please check!";
                }
                if (err2 > 0)
                {
                    btnSave.Enabled = false;
                    btnSaveGrey.BringToFront();
                    lbstock.Text = "Not enough stock, Please check!";
                }

            }
            Cursor = Cursors.Default;
          
        }
        private void afterdelete(DataGridView dgv)
        {
            try
            {
                if (tabControl1.SelectedTab == tabIN)
                {
                    int err = 0;
                    int err1 = 0;
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        string partnumber = row.Cells["partnumber"]?.Value?.ToString() ?? "";
                        string partname = row.Cells["partname"]?.Value?.ToString() ?? "";
                        string maker = row.Cells["maker"]?.Value?.ToString() ?? "";
                        string machinename = row.Cells["machinename"]?.Value?.ToString() ?? "";

                        if (string.IsNullOrEmpty(partname) &&
                            string.IsNullOrEmpty(partnumber) &&
                            string.IsNullOrEmpty(maker) &&
                            string.IsNullOrEmpty(machinename)
                            )
                        {
                            row.Cells["code"].Style.BackColor = Color.LightPink;
                            err++;
                        }
                        // Highlight qty2 if not a number
                        if (!double.TryParse(row.Cells["Qty"]?.Value?.ToString(), out double cellValue))
                        {
                            row.Cells["Qty"].Style.BackColor = Color.LightPink;
                            err1++;
                        }
                    }
                    if (err == 0)
                    {
                        lbshow.Text = "";
                    }
                    if (err1 == 0)
                    {
                        lbNum2.Text = "";
                    }
                    if (err == 0 && err1 == 0)
                    {
                        btnSave.BringToFront();
                        btnSave.Enabled = true;
                        lbshow.Text = "Success, Please save!";
                    }
                    if (dgv.Rows.Count == 0)
                    {
                        lbshow.Text = "";
                        lbNum2.Text = "";
                    }
                    lbfound1.Text = "Found: " + dgv.Rows.Count;
                }
                if (tabControl1.SelectedTab == tabOut)
                {
                    int err = 0;
                    int err1 = 0;   
                    int err2 = 0;
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        string partnumber = row.Cells["partnumber2"]?.Value?.ToString() ?? "";
                        string partname = row.Cells["partname2"]?.Value?.ToString() ?? "";
                        string maker = row.Cells["maker2"]?.Value?.ToString() ?? "";
                        string machinename = row.Cells["machinename2"]?.Value?.ToString() ?? "";
                        double stockout = double.TryParse(row.Cells["Qty2"]?.Value?.ToString(), out double val) ? val : 0;
                        double remain = double.TryParse(row.Cells["stockremain2"]?.Value?.ToString(), out double val1) ? val1 : 0;

                        if (string.IsNullOrEmpty(partname) &&
                            string.IsNullOrEmpty(partnumber) &&
                            string.IsNullOrEmpty(maker) &&
                            string.IsNullOrEmpty(machinename)
                            )
                        {
                            row.Cells["code2"].Style.BackColor = Color.LightPink;
                            err++;
                        }
                        else
                        {
                            if (stockout > remain)
                            {
                                err2++;
                            }
                        }
                        if (!double.TryParse(row.Cells["Qty2"]?.Value?.ToString(), out double cellValue))
                        {
                            row.Cells["Qty2"].Style.BackColor = Color.LightPink;
                            err1++;
                        }
                    }
                    if (err == 0)
                    {
                        lbshow2.Text = "";
                    }
                    if (err1 == 0)
                    {
                        lbNum.Text = "";
                    }
                    if (err2 == 0)
                    {
                        lbstock.Text = "";
                    }
                    if (err == 0 && err1 == 0 && err2 == 0)
                    {
                       btnSave.Enabled= true;
                        btnSave.BringToFront();
                        lbshow2.Text = "Success, Please save!";
                        lbNum.Text = "";
                        lbstock.Text = "";
                    }
                    if (dgv.Rows.Count == 0)
                    {
                        lbshow2.Text = "";
                        lbNum.Text =  "";
                        lbstock.Text = "";
                    }
                    lbfound2.Text = "Found: " + dgv.Rows.Count;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong with delete" + ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
