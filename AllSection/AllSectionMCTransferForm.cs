using MachineDeptApp.InputTransferSemi;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.AllSection
{    
    public partial class AllSectionMCTransferForm : Form
    {
        SQLConnectAllSection cnnAll = new SQLConnectAllSection();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SqlCommand cmd;
        DataTable dtAllDataForExcel;
        DataTable dtMstAllow;
        string fName;

        public AllSectionMCTransferForm()
        {
            InitializeComponent();
            this.cnnAll.Connection();
            this.cnnOBS.Connection();
            this.Load += AllSectionMCTransferForm_Load;
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnNew.Click += BtnNew_Click;
            this.btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (dgvScannedTag.Rows.Count > 0)
            {
                DialogResult DLR = MessageBox.Show("តើអ្នកចង់រក្សាទុកនិងព្រីនទិន្នន័យនេះមែនដែរ ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    int Error = 0;
                    var CDirectory = Environment.CurrentDirectory;

                    //Print Label as Excel
                    try
                    {
                        Excel.Application excelApp = new Excel.Application();
                        Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\MCUncompleteLabelTemplate.xlsx", Editable: true);
                        Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["RachhanSystem"];

                        //Write All for base data
                        if (dgvScannedTag.Rows.Count > 1)
                        {
                            //Insert More Paper
                            int paper = dgvScannedTag.Rows.Count;
                            for (int i = 1; i < paper; i++)
                            {
                                int indexExcel = 12 * i + 1;
                                worksheet.Range["A1:A12"].EntireRow.Copy();
                                worksheet.Range["A" + indexExcel].Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                            }
                        }

                        //Write all data
                        for (int i = 0; i < dgvScannedTag.Rows.Count; i++)
                        {
                            string Barcode = "";
                            string POSNo = "";
                            string ItemCode = "";
                            string ItemName = "";
                            string Wire = "";
                            string PIN = "";
                            string Length = "";
                            string Qty = "";
                            string BoxNo = "";
                            string ShipDate = "";

                            foreach (DataRow row in dtAllDataForExcel.Rows)
                            {
                                if (dgvScannedTag.Rows[i].Cells[0].Value.ToString() == row[0].ToString() && dgvScannedTag.Rows[i].Cells[3].Value.ToString() == row[3].ToString())
                                {
                                    POSNo = row[0].ToString();
                                    ItemCode = row[1].ToString();
                                    ItemName = row[2].ToString();
                                    BoxNo = row[3].ToString();
                                    Qty = row[4].ToString();
                                    ShipDate = row[5].ToString();
                                    Wire = row[6].ToString();
                                    PIN = row[7].ToString();
                                    Length = row[8].ToString();
                                    Barcode = "*" + POSNo + "/" + ItemCode + "/" + Qty + "/" + Convert.ToDouble(BoxNo).ToString("000") + "*";
                                }
                            }


                            worksheet.Cells[i * 12 + 2, 2] = Barcode;
                            worksheet.Cells[i * 12 + 3, 3] = POSNo;
                            worksheet.Cells[i * 12 + 5, 2] = ItemCode;
                            worksheet.Cells[i * 12 + 4, 3] = ItemName;
                            worksheet.Cells[i * 12 + 6, 3] = Wire;
                            worksheet.Cells[i * 12 + 7, 3] = PIN;
                            worksheet.Cells[i * 12 + 8, 3] = Length;
                            worksheet.Cells[i * 12 + 9, 3] = Qty;
                            worksheet.Cells[i * 12 + 9, 5] = BoxNo;
                            worksheet.Cells[i * 12 + 9, 7] = Convert.ToDateTime(ShipDate);

                        }

                        //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                        string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\Uncomplete Tag";
                        if (!Directory.Exists(SavePath))
                        {
                            Directory.CreateDirectory(SavePath);
                        }

                        // Saving the modified Excel file                        
                        string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                        string file = "Uncomplete Tag ";
                        fName = file + "( " + date + " )";
                        worksheet.SaveAs(CDirectory.ToString() + @"\Report\Uncomplete Tag\" + fName + ".xlsx");
                        xlWorkBook.Save();
                        xlWorkBook.Close();
                        excelApp.Quit();

                        //Kill all Excel background process
                        var processes = from p in Process.GetProcessesByName("EXCEL")
                                        select p;
                        foreach (var process in processes)
                        {
                            if (process.MainWindowTitle.ToString().Trim() == "")
                                process.Kill();
                        }
                    }
                    catch
                    {
                        Error = Error + 1;
                    }

                    //Add to DB

                    try
                    {
                        cnnAll.con.Open();
                        DateTime dateNow = DateTime.Now;
                        string TransNo = "";

                        //Find Last TransNo
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT SysNo FROM tbAllTransaction WHERE RegDate=(SELECT MAX(RegDate) FROM tbAllTransaction WHERE Funct =N'វេរចេញ') AND " +
                                                            "SysNo=(SELECT MAX(SysNo) FROM tbAllTransaction WHERE Funct =N'វេរចេញ') Group By SysNo", cnnAll.con);
                        DataTable dtTransNo = new DataTable();
                        sda.Fill(dtTransNo);
                        if (dtTransNo.Rows.Count == 0)
                        {
                            TransNo = "TRF0000000001";
                        }
                        else
                        {
                            string LastTransNo = dtTransNo.Rows[0][0].ToString();
                            double NextTransNo = Convert.ToDouble(LastTransNo.Substring(LastTransNo.Length - 10)) + 1;
                            TransNo = "TRF" + NextTransNo.ToString("0000000000");

                        }

                        foreach (DataGridViewRow DgvRow in dgvScannedTag.Rows)
                        {
                            DateTime ShipDate = DateTime.Now;
                            foreach (DataRow row in dtAllDataForExcel.Rows)
                            {
                                if (DgvRow.Cells[0].Value.ToString() == row[0].ToString() && DgvRow.Cells[3].Value.ToString() == row[3].ToString())
                                {
                                    ShipDate = Convert.ToDateTime(Convert.ToDateTime(row[5].ToString()).ToString("yyyy-MM-dd") + " 00:00:00");
                                    break;
                                }
                            }
                            
                            //Add to tbAllTransaction
                            using (cmd = new SqlCommand("AddNewtbAllTransaction", cnnAll.con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@Sn ", TransNo);
                                cmd.Parameters.AddWithValue("@Lc ", "MC1");
                                cmd.Parameters.AddWithValue("@Fct ", "វេរចេញ");
                                cmd.Parameters.AddWithValue("@Pn ", DgvRow.Cells[0].Value.ToString());
                                cmd.Parameters.AddWithValue("@Ic ", DgvRow.Cells[1].Value.ToString());
                                cmd.Parameters.AddWithValue("@In ", DgvRow.Cells[2].Value.ToString());
                                cmd.Parameters.AddWithValue("@Bn ", Convert.ToInt16(DgvRow.Cells[3].Value.ToString()));
                                cmd.Parameters.AddWithValue("@Rq ", 0);
                                cmd.Parameters.AddWithValue("@Tq ", Convert.ToInt32(DgvRow.Cells[4].Value.ToString()));
                                cmd.Parameters.AddWithValue("@Rv ", 0);
                                cmd.Parameters.AddWithValue("@Sd", ShipDate);
                                cmd.Parameters.AddWithValue("@Rd", dateNow);
                                cmd.Parameters.AddWithValue("@Rb", MenuFormV2.UserForNextForm);
                                cmd.Parameters.AddWithValue("@Ud", dateNow);
                                cmd.Parameters.AddWithValue("@Ub ", MenuFormV2.UserForNextForm);
                                cmd.Parameters.AddWithValue("@Cs", 0);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    catch
                    {
                        Error = Error + 1;
                    }
                    cnnAll.con.Close();


                    if (Error == 0)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("ឯកសារ Excel រួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvScannedTag.Rows.Clear();
                        System.Diagnostics.Process.Start(CDirectory.ToString() + @"\\Report\Uncomplete Tag\" + fName + ".xlsx");
                        fName = "";
                    }
                    else
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("មានបញ្ហា!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }            
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            txtBarcode.Text = "";
            dgvScannedTag.Rows.Clear();
            ClearAll();
            txtBarcode.Focus();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvScannedTag.SelectedCells.Count > 0)
            {
                if (dgvScannedTag.CurrentCell.RowIndex > -1)
                {
                    DialogResult DLR = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនដែរ ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DLR == DialogResult.Yes)
                    {
                        string DgvPOS = dgvScannedTag.Rows[dgvScannedTag.CurrentCell.RowIndex].Cells[0].Value.ToString();
                        string DgvBox = dgvScannedTag.Rows[dgvScannedTag.CurrentCell.RowIndex].Cells[3].Value.ToString();
                        for (int i = dtAllDataForExcel.Rows.Count - 1; i > -1; i--)
                        {
                            string dtPOS = dtAllDataForExcel.Rows[i][0].ToString();
                            string dtBox = dtAllDataForExcel.Rows[i][3].ToString();

                            if (DgvPOS == dtPOS && DgvBox ==dtBox)
                            {
                                dtAllDataForExcel.Rows.RemoveAt(i);
                                dtAllDataForExcel.AcceptChanges();
                                break;
                            }
                        }

                        dgvScannedTag.Rows.RemoveAt(dgvScannedTag.CurrentCell.RowIndex);
                        dgvScannedTag.Refresh();
                        dgvScannedTag.ClearSelection();
                    }
                }
            }
        }

        private void AllSectionMCTransferForm_Load(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.Trim() == "")
                {

                }
                else
                {
                    if (txtBarcode.Text.Length >= 23)
                    {
                        string OriginBarcode = txtBarcode.Text;
                        try
                        {
                            string PosNo = "";
                            string separate = txtBarcode.Text;
                            // Part 1: split on a single character.
                            string[] array = separate.Split('/');
                            PosNo = array[0].Replace(" ", "");
                            string WipCode = array[1].Replace(" ", "");
                            int Qty = Convert.ToInt16(array[2].Replace(" ", ""));
                            int BoxNo = Convert.ToInt16(array[3].Replace(" ", ""));
                            txtBarcode.Text = "";
                            int DupRow = 0;


                            for (int i = 0; i < dgvScannedTag.Rows.Count; i++)
                            {
                                if (dgvScannedTag.Rows[i].Cells[0].Value.ToString().Trim() == PosNo && Convert.ToInt16(dgvScannedTag.Rows[i].Cells[3].Value.ToString().Trim()) == BoxNo)
                                {
                                    DupRow = DupRow + 1;
                                }
                            }

                            if (DupRow > 0)
                            {
                                txtBarcode.Text = OriginBarcode;
                                MessageBox.Show("ឡាប៊ែលនេះស្កេនរួចហើយ​ ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtBarcode.SelectAll();
                                txtBarcode.Focus();
                            }
                            else
                            {
                                if (PosNo.Trim() != "" && WipCode.ToString().Trim() != "" && Qty.ToString().Trim() != "" && BoxNo.ToString().Trim() != "")
                                {

                                    cnnAll.con.Open();
                                    SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbAllTransaction WHERE Funct =N'វេរចេញ' AND LocCode='MC1' AND CancelStatus='0' AND POS_No='"+PosNo+"' AND ItemCode='"+WipCode+"' AND BoxNo="+BoxNo+" ", cnnAll.con);
                                    DataTable dt = new DataTable();
                                    sda.Fill(dt);
                                    SqlDataAdapter sda2 = new SqlDataAdapter("SELECT * FROM tbMstAllow", cnnAll.con);
                                    dtMstAllow = new DataTable();
                                    sda2.Fill(dtMstAllow);
                                    cnnAll.con.Close();

                                    if (dt.Rows.Count > 0)
                                    {
                                        txtBarcode.Text = OriginBarcode;
                                        MessageBox.Show("ឡាប៊ែលនេះស្កេនរួចហើយ ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        txtBarcode.SelectAll();
                                        txtBarcode.Focus();
                                    }
                                    else
                                    {
                                        int FoundAllow = 0;
                                        foreach (DataRow row in dtMstAllow.Rows)
                                        {
                                            if (WipCode.Substring(0, WipCode.Length - 2) == row[0].ToString())
                                            {
                                                FoundAllow = FoundAllow + 1;
                                                break;
                                            }
                                        }

                                        if (FoundAllow > 0)
                                        {
                                            cnnOBS.conOBS.Open();
                                            SqlDataAdapter sda1 = new SqlDataAdapter("SELECT DONo, T1.ItemCode, T2.ItemName, Remark3, Remark2, Remark4, POSDeliveryDate FROM " +
                                                                                                                "(SELECT DONo, ItemCode, POSDeliveryDate FROM prgproductionorder WHERE DONo='" + PosNo + "' AND ItemCode='" + WipCode + "') T1 " +
                                                                                                                "INNER JOIN " +
                                                                                                                "(SELECT ItemCode, ItemName,Remark3, Remark2, Remark4 FROM mstitem WHERE ItemType=1) T2 " +
                                                                                                                "ON T1.ItemCode=T2.ItemCode", cnnOBS.conOBS);
                                            DataTable dtDetails = new DataTable();
                                            sda1.Fill(dtDetails);
                                            if (dtDetails.Rows.Count > 0)
                                            {
                                                string Itemname = dtDetails.Rows[0][2].ToString();
                                                DateTime DelDate = Convert.ToDateTime(dtDetails.Rows[0][6].ToString());
                                                string Wire = dtDetails.Rows[0][3].ToString();
                                                string Pin = dtDetails.Rows[0][4].ToString();
                                                string Lenght = dtDetails.Rows[0][5].ToString();
                                                dgvScannedTag.Rows.Add(PosNo, WipCode, Itemname, BoxNo, Qty);
                                                dtAllDataForExcel.Rows.Add(PosNo, WipCode, Itemname, BoxNo, Qty, DelDate, Wire, Pin, Lenght);
                                                dgvScannedTag.ClearSelection();
                                            }
                                            else
                                            {
                                                txtBarcode.Text = OriginBarcode;
                                                MessageBox.Show("មិនមាន POS នេះនៅក្នុងប្រព័ន្ធទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                txtBarcode.SelectAll();
                                                txtBarcode.Focus();
                                            }
                                            cnnOBS.conOBS.Close();

                                        }
                                        else
                                        {
                                            string FGList = "";
                                            foreach (DataRow row in dtMstAllow.Rows)
                                            {
                                                if (FGList.Trim() == "")
                                                {
                                                    FGList = "• " + row[0].ToString() + "XX\t   " + row[1].ToString()+"-XX";
                                                }
                                                else
                                                {
                                                    FGList = FGList + "\n• " + row[0].ToString() + "XX\t   " + row[1].ToString() + "-XX";
                                                }
                                            }
                                            MessageBox.Show("ប្រព័ន្ធអនុញ្ញាតសម្រាប់តែផលិតផលទាំងនេះទេ៖\n" + FGList, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                            txtBarcode.SelectAll();
                                            txtBarcode.Focus();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Wrong Barcode format ! \n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtBarcode.SelectAll();
                            txtBarcode.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Wrong Barcode format ! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.SelectAll();
                        txtBarcode.Focus();
                    }
                }
            }
        }

        private void ClearAll()
        {
            dtAllDataForExcel = new DataTable();
            dtAllDataForExcel.Columns.Add("POS_No");
            dtAllDataForExcel.Columns.Add("ItemCode");
            dtAllDataForExcel.Columns.Add("ItemName");
            dtAllDataForExcel.Columns.Add("BoxNo");
            dtAllDataForExcel.Columns.Add("TransQty");
            dtAllDataForExcel.Columns.Add("ShipDate");
            dtAllDataForExcel.Columns.Add("Wire");
            dtAllDataForExcel.Columns.Add("PIN");
            dtAllDataForExcel.Columns.Add("Length");

        }

    }
}
