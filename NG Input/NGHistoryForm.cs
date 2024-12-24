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

namespace MachineDeptApp.NG_Input
{
    public partial class NGHistoryForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        DataTable dtExport;

        public NGHistoryForm()
        {
            InitializeComponent();
            cnn.Connection();
            
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            dgvNGHistory.Rows.Clear();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From (Select * From tbNGHistory Where RegDate Between '" + DtStart.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '"+DtEnd.Value.ToString("yyyy-MM-dd")+ " 23:59:59') t1 INNER JOIN (Select ItemCode, ItemName From tbMasterItem Where LEN(ItemCode)<5) t2 ON t1.ItemCode=t2.ItemCode Order By RegDate ASC", cnn.con);
                DataTable table = new DataTable();
                da.Fill(table);
                //to get rows
                foreach (DataRow row in table.Rows)
                {
                    bool ReqSt;
                    if (row[3].ToString()=="1")
                    {
                        ReqSt = true;
                    }
                    else
                    {
                        ReqSt = false;
                    }
                    dgvNGHistory.Rows.Add(row[1], row[7], Convert.ToDouble(row[2].ToString()), ReqSt,Convert.ToDateTime(row[4].ToString()), row[5]);

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!"+ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dgvNGHistory.ClearSelection();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvNGHistory.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("តើអ្នកចង់ទាញទិន្នន័យចេញជា Excel មែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    //Add to dtExport first;
                    dtExport.Rows.Clear();
                    for (int i = 0; i < dgvNGHistory.Rows.Count; i++)
                    {
                        string Code = dgvNGHistory.Rows[i].Cells[0].Value.ToString();
                        string Items = dgvNGHistory.Rows[i].Cells[1].Value.ToString();
                        double Total = 0;

                        //Check Duplicate
                        int Duplicate = 0;
                        for (int j = 0; j < dtExport.Rows.Count; j++)
                        {
                            if (dtExport.Rows[j][0].ToString() == Code)
                            {
                                Duplicate = Duplicate + 1;
                            }
                        }

                        //If not Duplicate
                        if (Duplicate == 0)
                        {
                            //Sum all Qty
                            for (int sumRow = 0; sumRow < dgvNGHistory.Rows.Count; sumRow++)
                            {
                                if (dgvNGHistory.Rows[sumRow].Cells[0].Value.ToString() == Code)
                                {
                                    Total = Total + Convert.ToDouble(dgvNGHistory.Rows[sumRow].Cells[2].Value.ToString());
                                }
                            }

                            //Add to dtExport
                            dtExport.Rows.Add(Code,Items,Total);
                        }
                    }

                    //Sort for Code Smallest to Largest
                    DataView view = new DataView(dtExport);
                    view.Sort="Code ASC";
                    DataTable dtSortedTable = view.ToTable();

                    //Export to excel
                    //open excel application and create new workbook
                    Excel.Application app = new Excel.Application();
                    Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
                    Excel.Worksheet worksheet = null;

                    //hide new excel application and give a sheet name
                    app.Visible = false;
                    worksheet = workbook.Sheets["Sheet1"];
                    worksheet = workbook.ActiveSheet;
                    worksheet.Name = "Rachhan System";

                    try
                    {
                        worksheet.Cells[1, 1] = "Code";
                        worksheet.Cells[1, 1].Font.Size = 12;
                        worksheet.Cells[1, 1].Font.Bold = true;

                        worksheet.Cells[1, 2] = "Items";
                        worksheet.Cells[1, 2].Font.Size = 12;
                        worksheet.Cells[1, 2].Font.Bold = true;

                        worksheet.Cells[1, 3] = "Total NG Qty";
                        worksheet.Cells[1, 3].Font.Size = 12;
                        worksheet.Cells[1, 3].Font.Bold = true;

                        for (int i = 0; i < dtSortedTable.Rows.Count; i++)
                        {
                            worksheet.Cells[i + 2, 1] = dtSortedTable.Rows[i][0].ToString();
                            worksheet.Cells[i + 2, 2] = dtSortedTable.Rows[i][1].ToString();
                            worksheet.Cells[i + 2, 3] =Math.Round(Convert.ToDouble(dtSortedTable.Rows[i][2].ToString()),0);
                        }
                        for (int i = 1; i < dtSortedTable.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i].EntireColumn.AutoFit();

                        }

                        //Getting the location and file name of the excel to save from user. 
                        SaveFileDialog saveDialog = new SaveFileDialog();
                        saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                        saveDialog.FilterIndex = 1;

                        if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            string saveDir = saveDialog.FileName;
                            app.DisplayAlerts = false;
                            workbook.SaveAs(saveDialog.FileName, Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, Type.Missing, false, Excel.XlSaveAsAccessMode.xlExclusive, Excel.XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
                            app.DisplayAlerts = true;
                            workbook.Save();
                            workbook.Close();
                            app.Quit();

                            //Kill all Excel background process
                            var processes = from p in Process.GetProcessesByName("EXCEL")
                                            select p;
                            foreach (var process in processes)
                            {
                                if (process.MainWindowTitle.ToString().Trim() == "")
                                    process.Kill();
                            }


                            MessageBox.Show("ទាញទិន្នន័យចេញបានជោគជ័យ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            System.Diagnostics.Process.Start(saveDir.ToString());

                        }
                        else
                        {
                            //don't save and close workbook that just created
                            app.DisplayAlerts = false;
                            workbook.Close();
                            app.DisplayAlerts = true;
                            app.Quit();                          

                        }
                    }
                    catch
                    {
                        app.DisplayAlerts = false;
                        workbook.Close();
                        app.DisplayAlerts = true;
                        app.Quit();
                        
                    }
                }
            }
        }

        private void NGHistoryForm_Load(object sender, EventArgs e)
        {
            dtExport = new DataTable();
            dtExport.Columns.Add("Code",typeof(string));
            dtExport.Columns.Add("Items", typeof(string));
            dtExport.Columns.Add("TotalQty", typeof(double));
        }

    }
}
