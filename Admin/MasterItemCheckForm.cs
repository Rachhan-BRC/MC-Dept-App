using MachineDeptApp.TrackingPOS;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.Admin
{
    public partial class MasterItemCheckForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        
        SqlCommand cmd;

        DataTable dtFound;
        DataTable OBSItem;


        public MasterItemCheckForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.btnSearch.Click += BtnSearch_Click;
            this.LbTotal.DoubleClick += LbTotal_DoubleClick;
            this.LbNoBoth.DoubleClick += LbNoBoth_DoubleClick;
            this.LbNoMC1.DoubleClick += LbNoMC1_DoubleClick;
            this.LbNoSlot.DoubleClick += LbNoSlot_DoubleClick;
            this.btnExport.Click += BtnExport_Click;

        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dtFound != null)
            {
                if (dtFound.Rows.Count > 0)
                {
                    Cursor = Cursors.WaitCursor;
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
                        for (int i = 0; i < dgvSearchResult.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i + 1] = dgvSearchResult.Columns[i].HeaderText.ToString();
                            worksheet.Cells[1, i + 1].Style.Font.Name = "Khmer OS Battambang";
                            worksheet.Cells[1, i + 1].Font.Size = 12;
                            worksheet.Cells[1, i + 1].Font.Bold = true;

                        }

                        for (int i = 0; i < dtFound.Rows.Count; i++)
                        {
                            worksheet.Cells[i + 2, 1] = dtFound.Rows[i][0].ToString();
                            worksheet.Cells[i + 2, 2] = dtFound.Rows[i][1].ToString();
                            worksheet.Cells[i + 2, 3] = dtFound.Rows[i][2].ToString();
                            worksheet.Cells[i + 2, 4] = dtFound.Rows[i][3].ToString();
                            worksheet.Cells[i + 2, 5] = dtFound.Rows[i][4].ToString();
                            worksheet.Cells[i + 2, 6] = dtFound.Rows[i][5].ToString();
                        }
                        for (int i = 0; i < dgvSearchResult.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i + 1].EntireColumn.AutoFit();

                        }

                        Cursor = Cursors.Default;
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

                            //Kill all Excel background process
                            var processes = from p in Process.GetProcessesByName("EXCEL")
                                            select p;
                            foreach (var process in processes)
                            {
                                if (process.MainWindowTitle.ToString().Trim() == "")
                                    process.Kill();
                            }
                        }
                    }
                    catch
                    {

                        app.DisplayAlerts = false;
                        workbook.Close();
                        app.DisplayAlerts = true;
                        app.Quit();

                        //Kill all Excel background process
                        var processes = from p in Process.GetProcessesByName("EXCEL")
                                        select p;
                        foreach (var process in processes)
                        {
                            if (process.MainWindowTitle.ToString().Trim() == "")
                                process.Kill();
                        }
                        Cursor = Cursors.Default;
                    }
                }
            }
            
        }

        private void LbTotal_DoubleClick(object sender, EventArgs e)
        {
            dgvSearchResult.Rows.Clear();
            foreach (DataRow row in dtFound.Rows)
            {
                dgvSearchResult.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5]);
            }
            dgvSearchResult.ClearSelection();
        }

        private void LbNoSlot_DoubleClick(object sender, EventArgs e)
        {
            dgvSearchResult.Rows.Clear();
            foreach (DataRow row in dtFound.Rows)
            {
                if (row[4].ToString().Trim() != "" && row[5].ToString().Trim() == "")
                {
                    dgvSearchResult.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5]);
                }
            }
            dgvSearchResult.ClearSelection();

        }

        private void LbNoMC1_DoubleClick(object sender, EventArgs e)
        {
            dgvSearchResult.Rows.Clear();
            foreach (DataRow row in dtFound.Rows)
            {
                if (row[4].ToString().Trim() == "" && row[5].ToString().Trim() != "")
                {
                    dgvSearchResult.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5]);
                }
            }
            dgvSearchResult.ClearSelection();

        }

        private void LbNoBoth_DoubleClick(object sender, EventArgs e)
        {
            dgvSearchResult.Rows.Clear();
            foreach (DataRow row in dtFound.Rows)
            {
                if (row[4].ToString().Trim() == "" && row[5].ToString().Trim() == "")
                {
                    dgvSearchResult.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5]);
                }
            }
            dgvSearchResult.ClearSelection();

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            LbStatus.Visible = true;

            CalcAndFind();

            Cursor = Cursors.Default;
            LbStatus.Text = "រកឃើញទិន្នន័យ " + dtFound.Rows.Count.ToString("N0");
            LbStatus.Refresh();

        }

        private void CalcAndFind()
        {
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT LEFT(WIPCode,4) AS FGCode, T4.ItemName AS FGName, WIPCode, T3.ItemName, MC1Type, Slot FROM " +
                                                                            "(SELECT WIPCode FROM tbPOSDetailofMC GROUP BY WIPCode) T1 " +
                                                                            "LEFT JOIN " +
                                                                            "(SELECT * FROM tbMasterItemPlan) T2 " +
                                                                            "ON T1.WIPCode=T2.ItemCode " +
                                                                            "LEFT JOIN " +
                                                                            "(SELECT ItemCode, ItemName FROM tbMasterItem WHERE LEN(ItemCode)>4) T3 " +
                                                                            "ON T1.WIPCode=T3.ItemCode " +
                                                                            "LEFT JOIN " +
                                                                            "(SELECT ItemCode, ItemName FROM tbMasterItem WHERE LEN(ItemCode)=4) T4 " +
                                                                            "ON LEFT(T1.WIPCode,4)=T4.ItemCode " +
                                                                            "WHERE T2.MC1Type IS NULL OR TRIM(T2.MC1Type)='' OR Slot IS NULL OR TRIM(Slot)='' " +
                                                                            "ORDER BY T1.WIPCode ASC", cnn.con);
                dtFound = new DataTable();
                sda.Fill(dtFound);
                dgvSearchResult.Rows.Clear();
                foreach (DataRow row in dtFound.Rows)
                {
                    dgvSearchResult.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5]);
                }
                dgvSearchResult.ClearSelection();

                int NoBoth = 0;
                int NoMC1 = 0;
                int NoSlot = 0;
                foreach (DataRow row in dtFound.Rows)
                {
                    if (row[4].ToString().Trim() == "" && row[5].ToString().Trim() == "")
                    {
                        NoBoth = NoBoth + 1;
                    }
                    else if (row[4].ToString().Trim() == "")
                    {
                        NoMC1 = NoMC1 + 1;
                    }
                    else
                    {
                        NoSlot = NoSlot + 1;
                    }
                }
                LbTotal.Text=dtFound.Rows.Count.ToString("N0");
                LbNoBoth.Text=NoBoth.ToString("N0");
                LbNoMC1.Text = NoMC1.ToString("N0");
                LbNoSlot.Text = NoSlot.ToString("N0");
            }
            catch(Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n"+ex.Message,"Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
        }

    }
}
