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
using System.Windows.Media.Animation;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp
{
    public partial class ReportNGList : Form
    {
        SQLConnect con = new SQLConnect();
        public ReportNGList()
        {
            InitializeComponent();
            this.con.Connection();
            this.Shown += ReportNGList_Shown;
            this.btnSearchExport.Click += BtnSearchExport_Click;
            this.btnSearchExport.MouseEnter += BtnSearchExport_MouseEnter;
            this.btnSearchExport.MouseLeave += BtnSearchExport_MouseLeave;
            this.picadd.MouseEnter +=BtnSearchExport_MouseEnter;
            this.picadd.MouseLeave += BtnSearchExport_MouseLeave;
            this.picsearch.MouseEnter += BtnSearchExport_MouseEnter;
            this.picsearch.MouseLeave += BtnSearchExport_MouseLeave;
            this.btnSwitch.Click += BtnSwitch_Click;
            this.picsearch.Click += BtnSearchExport_Click;
            this.picadd.Click += BtnSearchExport_Click;
            this.btnExport.Click += BtnExport_Click;
            this.dgvList.CellClick += DgvList_CellClick;
        }

        private void DgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 12)
            {
                dgvList.ClearSelection();
                DialogResult ask = MessageBox.Show("Are you sure you want to delete this ?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask == DialogResult.Yes)
                {
                    con.con.Open();
                    string sysno = dgvList.Rows[e.RowIndex].Cells["sysno"].Value.ToString();
                    Cursor = Cursors.WaitCursor;
                    try
                    {
                        string querydelete = "DELETE FROM tbNGTypeDetails WHERE SysNo = '"+sysno+"'";
                        SqlCommand cmd = new SqlCommand(querydelete, con.con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Deleted successfully !", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something went wrong! Please contact Phanun \n" + ex.Message, "Something went wrong.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    con.con.Close();
                    Cursor = Cursors.Default;
                    search();
                }
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            DialogResult DLS = MessageBox.Show("Are you sure you want to export the data?", "Confirm export", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DLS == DialogResult.Yes)
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV file (*.csv)|*.csv";
                saveDialog.FileName = "ReportNG " + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;
                    try
                    {
                        //Write Column name
                        int columnCount = 0;
                        foreach (DataGridViewColumn DgvCol in dgvList.Columns)
                        {
                            if (DgvCol.Visible == true)
                            {
                                columnCount = columnCount + 1;
                            }
                        }
                        string columnNames = "";

                        //String array for Csv
                        string[] outputCsv;
                        outputCsv = new string[dgvList.Rows.Count + 1];

                        //Set Column Name
                        for (int i = 0; i < columnCount; i++)
                        {
                            if (dgvList.Columns[i].Visible == true)
                            {
                                columnNames += dgvList.Columns[i].HeaderText.ToString() + ",";
                            }
                        }
                        outputCsv[0] += columnNames;

                        //Row of data 
                        for (int i = 1; (i - 1) < dgvList.Rows.Count; i++)
                        {
                            for (int j = 0; j < columnCount; j++)
                            {
                                if (dgvList.Columns[j].Visible == true)
                                {
                                    string Value = "";
                                    if (dgvList.Rows[i - 1].Cells[j].Value != null)
                                    {
                                        Value = dgvList.Rows[i - 1].Cells[j].Value.ToString();
                                    }
                                    //Fix don't separate if it contain '\n' or ','
                                    Value = "\"" + Value.Replace("\"", "\"\"") + "\"";
                                    outputCsv[i] += Value + ",";
                                }
                            }
                        }

                        File.WriteAllLines(saveDialog.FileName, outputCsv, Encoding.UTF8);
                        string file = saveDialog.FileName;
                        Cursor = Cursors.Default;
                        MessageBox.Show("ទាញទិន្នន័យចេញរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Process.Start(file);
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void ReportNGList_Shown(object sender, EventArgs e)
        {
            try
            {
                //type
                DataTable dttype = new DataTable();
                string querytype = "SELECT * FROM tbNGTypeMst WHERE Funct <> 3";
                SqlDataAdapter sda = new SqlDataAdapter(querytype, con.con);
                sda.Fill(dttype);

                if (dttype.Rows.Count > 0)
                {
                    foreach (DataRow row in dttype.Rows)
                    {
                        searchcbng.Items.Add(row["Name"].ToString());
                    }
                }
                lbwire.Text = "0";
                lbplan.Text = "0";
                lbNG.Text = "0";
                lbMC.Text = "0";
                lbttlqty.Text = "0";
                lbsubprice.Text = "0";
                lbRMqty.Text = "0";
                lbcostNG.Text = "0";

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong! Please contact Phanun" + ex.Message, "Something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnSwitch_Click(object sender, EventArgs e)
        {
            if (btnSearchExport.Text == "ស្វែងរក / Search")
            {
                picadd.BringToFront();
                btnSearchExport.Text = "ព្រីនចេញ / Print";
            }
            else
            {
                picsearch.BringToFront();
                btnSearchExport.Text = "ស្វែងរក / Search";
            }
        }
        private void BtnSearchExport_MouseLeave(object sender, EventArgs e)
        {
            picsearch.BackColor = Color.White;
            btnSearchExport.BackColor = Color.White;
            picadd.BackColor = Color.White;
        }
        private void BtnSearchExport_MouseEnter(object sender, EventArgs e)
        {
            picsearch.BackColor = Color.SkyBlue;
            btnSearchExport.BackColor = Color.SkyBlue;
            picadd.BackColor = Color.SkyBlue;

        }
        private void BtnSearchExport_Click(object sender, EventArgs e)
        {
            if (btnSearchExport.Text == "ស្វែងរក / Search")
            {
                search();
            }
            else
            {
                if (dgvList.Rows.Count > 0)
                {
                    DialogResult DLS = MessageBox.Show("Are you sure you want to print the data?", "Confirm print", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DLS == DialogResult.Yes)
                    {
                        Cursor = Cursors.WaitCursor;
                        Excel.Application excelApp = new Excel.Application();
                        Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(
                                 Path.Combine(Environment.CurrentDirectory, @"Template\ReportNGTemplate.xlsx"), Editable: false);
                        Excel.Worksheet worksheetSubpart = (Excel.Worksheet)xlWorkBook.Sheets[1];
                        Excel.Worksheet worksheetRM = (Excel.Worksheet)xlWorkBook.Sheets[2];
                        // Ensure folder exists
                        string SavePath = Path.Combine(Environment.CurrentDirectory, @"Report\ReportNGList");
                        Directory.CreateDirectory(SavePath);
                        int startrow = 9;
                        try
                        {
                            //value subpart
                            try
                            {
                                for (int i = 0; i < dgvList.Rows.Count; i++)
                                {
                                    if (i > 0)
                                    {
                                        // Insert new row with same format
                                        Excel.Range sourceRow = worksheetSubpart.Rows[startrow];
                                        sourceRow.Copy();

                                        Excel.Range insertRow = worksheetSubpart.Rows[startrow + i];
                                        insertRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                                    }
                                    worksheetSubpart.Cells[startrow + i, 1] = dgvList.Rows[i].Cells["posc"].Value.ToString();
                                    worksheetSubpart.Cells[startrow + i, 2] = dgvList.Rows[i].Cells["code"].Value.ToString();
                                    worksheetSubpart.Cells[startrow + i, 3] = dgvList.Rows[i].Cells["type"].Value.ToString();
                                    worksheetSubpart.Cells[startrow + i, 4] = dgvList.Rows[i].Cells["stopinfo"].Value.ToString();
                                    worksheetSubpart.Cells[startrow + i, 5] = dgvList.Rows[i].Cells["qty"].Value.ToString();
                                    worksheetSubpart.Cells[startrow + i, 6] = dgvList.Rows[i].Cells["price"].Value.ToString();
                                    worksheetSubpart.Cells[startrow + i, 7] = dgvList.Rows[i].Cells["pic"].Value.ToString();

                                }
                                string date = "Date : All ~ " + DateTime.Now.ToString("dd-MMMM-yyyy");
                                if (chkdate.Checked == true)
                                {
                                    date = "Date : " + dtpfrom.Value.ToString("dd-MMMM-yyyy") + " ~ " + dtpto.Value.ToString("dd-MMMM-yyyy");
                                }
                                worksheetSubpart.Cells[2, 10] = date;
                                worksheetSubpart.Cells[2, 9] = lbttlqty.Text;
                                worksheetSubpart.Cells[3, 9] = lbRMqty.Text;
                                worksheetSubpart.Cells[4, 9] = lbsubprice.Text;
                                worksheetSubpart.Cells[5, 9] = lbcostNG.Text;
                                worksheetSubpart.Cells[6, 9] = lbwire.Text;
                                worksheetSubpart.Cells[7, 9] = lbplan.Text;
                                worksheetSubpart.Cells[8, 9] = lbNG.Text;
                                worksheetSubpart.Cells[9, 9] = lbMC.Text;

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error print sub part ! "+ ex.Message, "Contact to Phanun", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            //RM
                            try
                            {
                                for (int i = 0; i < dgvCost.Rows.Count; i++)
                                {
                                    if (i > 0)
                                    {
                                        // Insert new row with same format
                                        Excel.Range sourceRow = worksheetRM.Rows[startrow];
                                        sourceRow.Copy();

                                        Excel.Range insertRow = worksheetRM.Rows[startrow + i];
                                        insertRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                                    }
                                    worksheetRM.Cells[startrow + i, 1] = dgvCost.Rows[i].Cells["posc2"].Value.ToString();
                                    worksheetRM.Cells[startrow + i, 2] = dgvCost.Rows[i].Cells["itemcode2"].Value.ToString();
                                    worksheetRM.Cells[startrow + i, 3] = dgvCost.Rows[i].Cells["rmcode2"].Value.ToString();
                                    worksheetRM.Cells[startrow + i, 4] = dgvCost.Rows[i].Cells["type2"].Value.ToString();
                                    worksheetRM.Cells[startrow + i, 5] = dgvCost.Rows[i].Cells["stopinfo2"].Value.ToString();
                                    worksheetRM.Cells[startrow + i, 6] = dgvCost.Rows[i].Cells["qty2"].Value.ToString();
                                    worksheetRM.Cells[startrow + i, 7] = dgvCost.Rows[i].Cells["price2"].Value.ToString();
                                    worksheetRM.Cells[startrow + i, 8] = dgvCost.Rows[i].Cells["pic2"].Value.ToString();

                                }
                                string date = "Date : All ~ " + DateTime.Now.ToString("dd-MMMM-yyyy");
                                if (chkdate.Checked == true)
                                {
                                    date = "Date : " + dtpfrom.Value.ToString("dd-MMMM-yyyy") + " ~ " + dtpto.Value.ToString("dd-MMMM-yyyy");
                                }
                                worksheetRM.Cells[2, 11] = date;
                                worksheetRM.Cells[2, 10] = lbttlqty.Text;
                                worksheetRM.Cells[3, 10] = lbRMqty.Text;
                                worksheetRM.Cells[4, 10] = lbsubprice.Text;
                                worksheetRM.Cells[5, 10] = lbcostNG.Text;
                                worksheetRM.Cells[6, 10] = lbwire.Text;
                                worksheetRM.Cells[7, 10] = lbplan.Text;
                                worksheetRM.Cells[8, 10] = lbNG.Text;
                                worksheetRM.Cells[9, 10] = lbMC.Text;

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error print RM  ! " +ex.Message , "Contact to Phanun", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            //Close 
                            string DateExcel = DateTime.Now.ToString("yyMMdd");
                            string fileName = "Report NG List" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".xlsx";

                            string fullPath = Path.Combine(SavePath, fileName);
                            xlWorkBook.SaveAs(fullPath);
                            excelApp.DisplayAlerts = false;
                            xlWorkBook.Close();
                            excelApp.Quit();
                            excelApp.DisplayAlerts = true;
                            MessageBox.Show("Print sucessfully !", "Done.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Cursor = Cursors.Default;
                            Process.Start(fullPath);
                        }
                        catch (Exception ex)
                        {
                            excelApp.DisplayAlerts = false;
                            xlWorkBook.Close();
                            excelApp.Quit();
                            excelApp.DisplayAlerts = true;
                            Cursor = Cursors.Default;
                            MessageBox.Show("File excel នេះកំពុងបើក, សូមបិទជាមុនសិន​ រួច Print ម្ដងទៀត!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
        }
        private void search()
        {
            DataTable dtconds = new DataTable();
            dtconds.Columns.Add("Val");
            if (chkdate.Checked == true)
            {
                dtconds.Rows.Add("tbNG.RegDate BETWEEN '" + dtpfrom.Value.ToString("yyyy-MM-dd 00:00:00.000") + "' AND '" + dtpto.Value.ToString("yyyy-MM-dd 23:59:59.000") + "'");
            }
            if (searchcode.Text != "")
            {
                dtconds.Rows.Add("tbNG.ItemCode LIKE '%" + searchcode.Text + "%'");
            }
            if (searchposc.Text != "")
            {
                dtconds.Rows.Add("tbNG.POSC LIKE '%" + searchposc.Text + "%'");
            }
            if (searchcbstop.Text != "")
            {
                dtconds.Rows.Add("tbNG.StopInfo = '" + searchcbstop.Text + "'");
            }
            if (searchcbng.Text != "")
            {
                dtconds.Rows.Add("tbNG.Type = '" + searchcbng.Text + "'");
            }
            string where = "";
            foreach (DataRow row in dtconds.Rows)
            {
                if (where == "")
                {
                    where = " WHERE " + row["Val"].ToString();
                }
                else
                {
                    where += " AND " + row["Val"].ToString();
                }
            }
            con.con.Open();
            try
            {
                dgvList.Rows.Clear();
                dgvCost.Rows.Clear();
                DataTable dtsearch = new DataTable();
                string query = "SELECT * FROM tbNGTypeDetails tbNG " +
                    "LEFT JOIN (SELECT ItemCode, Resv4 FROM [192.168.1.21].[Marunix].[dbo].[mstitem] WHERE ItemType = 1) tbp ON tbNG.ItemCode = tbp.ItemCode" + where + " ORDER BY tbNG.ItemCode";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dtsearch);
                DataTable dtsearch2 = new DataTable();

                double wire = 0, plan = 0, NG = 0, MC = 0, totalqty = 0, ttlCostNG = 0, totalsubprice = 0, totalrm = 0;

                foreach (DataRow row in dtsearch.Rows)
                {
                    string code = row["ItemCode"].ToString();
                    string query2 = @"SELECT * FROM tbNGTypeDetails tbNG 
                                                LEFT JOIN 
                                                (SELECT UpItemCode, LowItemCode, LowQty FROM MstBOM )tbBom ON tbNG.ItemCode = tbBom.UpItemCode
                                                LEFT JOIN 
                                                (SELECT RMCode, UnitPrice FROM [RawMaterialWHDB].[dbo].[tbMstUnitPrice]) tbP ON tbBom.LowItemCode = tbP.RMCode
                                                WHERE tbNG.ItemCode = '" + code + "' ";
                    SqlDataAdapter sda2 = new SqlDataAdapter(query2, con.con);
                    sda2.Fill(dtsearch2);
                    double qty = Convert.ToDouble(row["Qty"]), price = Convert.ToDouble(row["Resv4"]);
                    double subprice = qty * price;
                    dgvList.Rows.Add();
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["sysno"].Value = row["SysNo"].ToString();
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["posc"].Value = row["POSC"].ToString();
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["code"].Value = row["ItemCode"].ToString();
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["stopinfo"].Value = row["StopInfo"].ToString();
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["type"].Value = row["Type"].ToString();
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["qty"].Value = Convert.ToDouble(row["Qty"]);
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["price"].Value = subprice;
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["pic"].Value = row["PIC"].ToString();
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["regdate"].Value = Convert.ToDateTime(row["RegDate"]);
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["regby"].Value = row["RegBy"].ToString();
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["update"].Value = Convert.ToDateTime(row["UpdateDate"]);
                    dgvList.Rows[dgvList.Rows.Count - 1].Cells["upby"].Value = row["UpdateBy"].ToString();
                    totalqty += Convert.ToDouble(row["Qty"]);
                    totalsubprice += subprice;
                    if (row["StopInfo"].ToString() == "Change Wire")
                    {
                        wire += Convert.ToDouble(row["Qty"]);
                    }
                    else if (row["StopInfo"].ToString() == "Change Plan")
                    {
                        plan += Convert.ToDouble(row["Qty"]);
                    }
                    else if (row["StopInfo"].ToString() == "Machine Broken")
                    {
                        MC += Convert.ToDouble(row["Qty"]);
                    }
                    else
                    {
                        NG += Convert.ToDouble(row["Qty"]);
                    }


                }
                foreach (DataRow row in dtsearch2.Rows)
                {
                    double lowqty = Convert.ToDouble(row["LowQty"]), price = Convert.ToDouble(row["UnitPrice"]), qty = Convert.ToDouble(row["Qty"]);
                    double ttlqty = lowqty * qty;
                    double costNG = ttlqty * price;
                    dgvCost.Rows.Add();
                    dgvCost.Rows[dgvCost.Rows.Count - 1].Cells["posc2"].Value = row["POSC"].ToString();
                    dgvCost.Rows[dgvCost.Rows.Count - 1].Cells["itemcode2"].Value = row["ItemCode"].ToString();
                    dgvCost.Rows[dgvCost.Rows.Count - 1].Cells["rmcode2"].Value = row["RMCode"].ToString();
                    dgvCost.Rows[dgvCost.Rows.Count - 1].Cells["stopinfo2"].Value = row["StopInfo"].ToString();
                    dgvCost.Rows[dgvCost.Rows.Count - 1].Cells["type2"].Value = row["Type"].ToString();
                    dgvCost.Rows[dgvCost.Rows.Count - 1].Cells["qty2"].Value = ttlqty;
                    dgvCost.Rows[dgvCost.Rows.Count - 1].Cells["price2"].Value = costNG;
                    dgvCost.Rows[dgvCost.Rows.Count - 1].Cells["pic2"].Value = row["PIC"].ToString();
                    dgvCost.Rows[dgvCost.Rows.Count - 1].Cells["regdate2"].Value = Convert.ToDateTime(row["RegDate"]);
                    dgvCost.Rows[dgvCost.Rows.Count - 1].Cells["regby2"].Value = row["RegBy"].ToString();
                    dgvCost.Rows[dgvCost.Rows.Count - 1].Cells["updatedate2"].Value = Convert.ToDateTime(row["UpdateDate"]);
                    dgvCost.Rows[dgvCost.Rows.Count - 1].Cells["updateby2"].Value = row["UpdateBy"].ToString();
                    ttlCostNG += costNG;
                    totalrm += ttlqty;
                }
                lbwire.Text = wire.ToString("N0");
                lbplan.Text = plan.ToString("N0");
                lbMC.Text = MC.ToString("N0");
                lbNG.Text = NG.ToString("N0");
                lbttlqty.Text = totalqty.ToString();
                lbsubprice.Text = totalsubprice.ToString("N4");
                lbcostNG.Text = ttlCostNG.ToString("N4");
                lbRMqty.Text = totalrm.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong! Please contact Phanun\n" + ex.Message, "Something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.con.Close();
            dgvList.ClearSelection();
            dgvCost.ClearSelection();
        }

    }
}
