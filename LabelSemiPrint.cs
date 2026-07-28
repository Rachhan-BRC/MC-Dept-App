using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp
{
    public partial class LabelSemiPrint : Form
    {
        SQLConnect con = new SQLConnect();
        public LabelSemiPrint()
        {
            this.con.Connection();
            InitializeComponent();
            this.txtScan.KeyDown += TxtScan_KeyDown;
            this.btnNew.Click += BtnNew_Click;
            this.dgvList.CellClick += DgvList_CellClick;
            this.chkall.Click += Chkall_Click;
            this.btnPrint.Click += BtnPrint_Click;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (dgvList.Rows.Count > 0)
            {
                Cursor = Cursors.WaitCursor;
                int count = 0;
                foreach (DataGridViewRow row in dgvList.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["chk"].Value) == true)
                    {
                        count++;
                        break;
                    }
                }
                if (count > 0)
                { // Open Excel template
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(
                        Path.Combine(Environment.CurrentDirectory, @"Template\LabelSemiPrintTemplate.xlsx"), Editable: true);
                    Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets[1];
                    // Ensure Report folder exists
                    string reportFolder = Path.Combine(Environment.CurrentDirectory, @"Report\Label Semi Printed");
                    if (!Directory.Exists(reportFolder))
                    {
                        Directory.CreateDirectory(reportFolder);
                    }
                    // Create SaveFileDialog
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    sfd.DefaultExt = "xlsx";

                    // Build filename
                    string fileName = "LabelSemiPrint" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".xlsx";
                    sfd.FileName = Path.Combine(reportFolder, fileName);

                    // Final save path
                    string SavePath = sfd.FileName;
                    try
                    {
                        int startrow = 1;
                        int range = 16;
                        for (int k = 0; k < dgvList.Rows.Count; k++)
                        {
                            bool chk = Convert.ToBoolean(dgvList.Rows[k].Cells["chk"].Value);
                            if (chk == true)
                            {
                                if (k > 0)
                                {
                                    worksheet.Range["A1:A15"].EntireRow.Copy();
                                    worksheet.Range["A" + range].EntireRow.PasteSpecial(Excel.XlPasteType.xlPasteAll, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, Type.Missing, Type.Missing);
                                    range += 15;
                                    startrow += 15;

                                }
                                worksheet.Cells[startrow, 3] = dgvList.Rows[k].Cells["subpartno"].Value?.ToString();
                                worksheet.Cells[startrow + 2, 3] = dgvList.Rows[k].Cells["poscno"].Value?.ToString();
                                worksheet.Cells[startrow + 3, 3] = dgvList.Rows[k].Cells["posqty"].Value?.ToString();
                                worksheet.Cells[startrow + 5, 3] = dgvList.Rows[k].Cells["wirecolor"].Value?.ToString();
                                worksheet.Cells[startrow + 6, 3] = dgvList.Rows[k].Cells["length"].Value?.ToString();
                                worksheet.Cells[startrow + 7, 3] = dgvList.Rows[k].Cells["batchqty"].Value?.ToString();
                                worksheet.Cells[startrow + 13, 8] = dgvList.Rows[k].Cells["deldate"].Value?.ToString();
                                worksheet.Cells[startrow + 9, 11] = dgvList.Rows[k].Cells["wipcode"].Value?.ToString();
                            }
                        }
                        string DateExcel = DateTime.Now.ToString("yyMMdd");
                        xlWorkBook.SaveAs(SavePath);

                        excelApp.DisplayAlerts = false;
                        xlWorkBook.Close();
                        excelApp.Quit();
                        excelApp.DisplayAlerts = true;

                        // Release COM objects to avoid leaving Excel.exe open
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                        MessageBox.Show("Print Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Process.Start(SavePath);
                    }
                    catch (Exception ex)
                    {
                        // Cleanup
                        excelApp.DisplayAlerts = false;
                        xlWorkBook.Close();
                        excelApp.Quit();
                        excelApp.DisplayAlerts = true;
                        MessageBox.Show("Something went wrong ! \n" + ex.Message, "Contact to Phanun", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    }
                }
                else
                {
                    MessageBox.Show("សូមជ្រើសរើសឡាប៊ែលសម្រាប់ព្រីន ! ","Remind.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                Cursor = Cursors.Default;
            }
        }

        private void Chkall_Click(object sender, EventArgs e)
        {
            if (chkall.Checked == true)
            {
                foreach (DataGridViewRow row in dgvList.Rows)
                {
                    row.Cells["chk"].Value = true;
                }
            }
            else
            {
                foreach (DataGridViewRow row in dgvList.Rows)
                {
                    row.Cells["chk"].Value = false;
                }
            }
        }

        private void DgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                bool chk = Convert.ToBoolean(dgvList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                if ( chk == true)
                {
                    dgvList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                }
                else
                {
                    dgvList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                }
                dgvList.ClearSelection();
                int count = 0;
                foreach (DataGridViewRow row in dgvList.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["chk"].Value) == true)
                    {
                        count++;
                    }
                   
                }
                if (dgvList.Rows.Count == count)
                    chkall.Checked = true;
                else
                    chkall.Checked = false;
            }
        }


        private void BtnNew_Click(object sender, EventArgs e)
        {
            DialogResult ask = MessageBox.Show("Are you sure you want to clear this data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask == DialogResult.Yes)
            {
                dgvList.Rows.Clear();
                chkall.Checked = false;
            }
          
        }

        private void TxtScan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtScan.Text.Trim() != "")
                {
                    dgvList.Rows.Clear();
                    try
                    {
                        Cursor = Cursors.WaitCursor;
                        con.con.Open();
                        DataTable dtscan = new DataTable();
                        string queryselect = "SELECT tbD.PPOSNO AS PosCNo, tbI.ItemName, tbD.ItemCode AS WIPCode, PlanQty AS ChildQty, Remarks2, Remarks3, tbD.POSDeliveryDate AS PosPDelDate, tbO.BacthSize  FROM [192.168.1.21].[Marunix].[dbo].[prgproductionorder] tbD " +
                     "LEFT JOIN tbMasterItem tbI ON tbD.ItemCode = tbI.ItemCode " +
                     "LEFT JOIN (SELECT BacthSize, ItemCode FROM [192.168.1.21].[Marunix].[dbo].[mstitem])tbO ON tbD.ItemCode = tbO.ItemCode " +
                     "WHERE PPOSNO= '" + txtScan.Text.Trim() + "'";
                        Console.WriteLine(queryselect);
                        SqlDataAdapter sda = new SqlDataAdapter(queryselect, con.con);
                        sda.Fill(dtscan);

                        if (dtscan.Rows.Count > 0)
                        {
                            string remarks = dtscan.Rows[0]["Remarks2"]?.ToString();
                            string color = new Dictionary<string, string>{
                                        {"WHT","WHITE"},{"VLT","VIOLET"},{"GRY","GRAY"},{"GRN","GREEN"},
                                        {"BLK","BLACK"},{"YEL","YELLOW"},{"BRN","BROWN"},{"SKY","SKYBLUE"},
                                        {"BLU","BLUE"},{"ORG","ORANGE"},{"RED","RED"},{"PINK","PINK"}
                                    }
                            .FirstOrDefault(kv => remarks?.Contains(kv.Key) == true).Value ?? remarks;
                            string[] parts = dtscan.Rows[0]["Remarks3"]?.ToString().Split(',');
                            int partcount = parts.Length;

                            for (int i = 1; i <= partcount; i++)
                            {
                                dgvList.Rows.Add();
                                dgvList.Rows[dgvList.Rows.Count - 1].Cells["poscno"].Value = dtscan.Rows[0]["PosCNo"]?.ToString();
                                dgvList.Rows[dgvList.Rows.Count - 1].Cells["subpartno"].Value = dtscan.Rows[0]["ItemName"]?.ToString();
                                dgvList.Rows[dgvList.Rows.Count - 1].Cells["wipcode"].Value = dtscan.Rows[0]["WIPCode"]?.ToString();
                                dgvList.Rows[dgvList.Rows.Count - 1].Cells["posqty"].Value = Convert.ToDouble(dtscan.Rows[0]["ChildQty"] ?? 0);
                                dgvList.Rows[dgvList.Rows.Count - 1].Cells["wirecolor"].Value = color;
                                dgvList.Rows[dgvList.Rows.Count - 1].Cells["length"].Value = parts[i - 1];
                                dgvList.Rows[dgvList.Rows.Count - 1].Cells["batchqty"].Value = Convert.ToDouble(dtscan.Rows[0]["BacthSize"] ?? 0);
                                dgvList.Rows[dgvList.Rows.Count - 1].Cells["deldate"].Value = Convert.ToDateTime(dtscan.Rows[0]["PosPDelDate"].ToString());
                                dgvList.Rows[dgvList.Rows.Count - 1].Cells["chk"].Value = true;
                            }
                            txtScan.Text = "";
                            txtScan.Focus();
                            chkall.Checked = true;
                            dgvList.ClearSelection();
                        }
                        else
                        {
                            MessageBox.Show("This POS No. not found ! Please check", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtScan.Focus();
                            txtScan.SelectAll();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something went wrong, Please contact to Phanun !\n" + ex.Message, "Something wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                  
                    con.con.Close();
                    Cursor = Cursors.Default;
                }
            }
        }
    }
}
