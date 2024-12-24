using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Reflection;

namespace MachineDeptApp.TrackingPOS
{
    public partial class POSTrackingSearchForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        string fName = "";
        DataTable dtPosDetails;
        DataTable dtPosPQty;

        public POSTrackingSearchForm()
        {
            InitializeComponent();
            cnn.Connection();
            this.DtInput.ValueChanged += DtInput_ValueChanged;
            this.DtEndInput.ValueChanged += DtEndInput_ValueChanged;
        }

        private void DtEndInput_ValueChanged(object sender, EventArgs e)
        {
            chkDate.Checked = true;
        }

        private void DtInput_ValueChanged(object sender, EventArgs e)
        {
            chkDate.Checked = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dgvPOSData.Rows.Clear();

            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Value");

            if (txtWipCode.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("T1.WIPCode LIKE ", "'" + txtWipCode.Text + "%' ");
            }
            if (txtName.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("T2.ItemName LIKE ", "'%" + txtName.Text + "%' ");
            }
            if (chkDate.Checked == true)
            {
                string POSFrom = DtInput.Value.ToString("yy-MM") + "00001";
                string POSToo = DtEndInput.Value.ToString("yy-MM") + "99999";
                dtSQLCond.Rows.Add("T1.PosCNo BETWEEN ", "'" + POSFrom + "' AND '" + POSToo + "' ");
            }

            string SQLConds = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                if (SQLConds.Trim() == "")
                {
                    SQLConds = "WHERE " + row[0] + row[1];
                }
                else
                {
                    SQLConds = SQLConds + "AND " + row[0] + row[1];
                }
            }

            SqlDataAdapter sda = new SqlDataAdapter("SELECT WIPCode, ItemName, PosCNo, PosCQty, PosCResultQty, PosCRemainQty, " +
                                                                                "  CASE " +
                                                                                "    WHEN PosCStatus=0 THEN N'មិនទាន់ផលិត' " +
                                                                                "    WHEN PosCStatus=1 THEN N'នៅសល់' " +
                                                                                "    ELSE N'រួចរាល់' " +
                                                                                "  END AS POSStatus FROM " +
                                                                                "  (SELECT WIPCode, PosCNo, PosCQty, PosCResultQty, PosCRemainQty, PosCStatus FROM tbPOSDetailofMC) T1 " +
                                                                                "  LEFT JOIN (SELECT ItemCode, ItemName FROM tbMasterItem) T2 ON T1.WIPCode=T2.ItemCode " + SQLConds + "ORDER BY T1.PosCNo ASC", cnn.con);
            dtPosDetails = new DataTable();
            sda.Fill(dtPosDetails);

            SqlDataAdapter sda1 = new SqlDataAdapter("SELECT PosPNo FROM "+
                                                                                "(SELECT PosPNo, WIPCode, PosCNo FROM tbPOSDetailofMC) T1 "+
                                                                                "LEFT JOIN(SELECT ItemCode, ItemName FROM tbMasterItem) T2 ON T1.WIPCode = T2.ItemCode "+ SQLConds +
                                                                                "GROUP BY  T1.PosPNo "+
                                                                                "ORDER BY T1.PosPNo ASC", cnn.con);
            dtPosPQty = new DataTable();
            sda1.Fill(dtPosPQty);


            foreach (DataRow row in dtPosDetails.Rows)
            {
                dgvPOSData.Rows.Add(row[0], row[1], row[2], Convert.ToDouble(row[3]), Convert.ToDouble(row[4]), Convert.ToDouble(row[5]), row[6]);
            }
            dgvPOSData.ClearSelection();
        }

        private void POSTrackingSearchForm_Load(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 250;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;
            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.btnSearch, "រក្សាទុក");
            toolTip1.SetToolTip(this.btnReprint, "ព្រីនចេញជា Excel");

        }

        private void dgvPOSData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 6 && e.Value.ToString() == "មិនទាន់ផលិត")
            {
                e.CellStyle.BackColor = Color.Yellow;
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambang", 11, FontStyle.Bold);
            }
            else if (e.ColumnIndex == 6 && e.Value.ToString() == "នៅសល់")
            {
                e.CellStyle.BackColor = Color.Orange;
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambang", 11, FontStyle.Bold);
            }
            else if (e.ColumnIndex == 6 && e.Value.ToString() == "រួចរាល់")
            {
                e.CellStyle.BackColor = Color.Green;
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambang", 11, FontStyle.Bold);
            }
            else if (e.ColumnIndex == 6 && e.Value.ToString() != "រួចរាល់" && e.Value.ToString() != "នៅសល់" && e.Value.ToString() != "មិនទាន់ផលិត")
            {
                e.CellStyle.BackColor = Color.Red;
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambang", 11, FontStyle.Bold);
            }
        }

        private void btnReprint_Click(object sender, EventArgs e)
        {            
            if (dgvPOSData.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("តើអ្នកចង់​ព្រីនមែន​ ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    LbExportStatus.Visible = true;
                    LbExportStatus.Text = "កំពុងបង្កើត File​ . . . .";
                    int row = dgvPOSData.Rows.Count+dtPosPQty.Rows.Count-1;

                    var CDirectory = Environment.CurrentDirectory;
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\POS_Checking.xlsx", Editable: true);
                    Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["SemiComplete_C"];
                    //add to All Data
                    try
                    {
                        if (dgvPOSData.Rows.Count == 1)
                        {
                            worksheet.Cells[2, 16] = DateTime.Now;

                            for (int i = 0; i < dgvPOSData.Rows.Count; i++)
                            {
                                for (int j = 0; j < dgvPOSData.Columns.Count; j++)
                                {
                                    if (dgvPOSData.Rows[i].Cells[j].Value != null)
                                    {
                                        worksheet.Cells[i + 6, j + 11] = dgvPOSData.Rows[i].Cells[j].Value.ToString();

                                    }
                                    else
                                    {
                                        //worksheet1.Cells[i + 5, j + 1] = "";
                                    }
                                }
                            }  
                            
                            // Saving the modified Excel file                        
                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                            string file = "POS_Checking ";
                            fName = file + "( " + date + " )";
                            worksheet.SaveAs(CDirectory.ToString() + @"\Report\POS_Checking\" + fName + ".xlsx");
                            xlWorkBook.Save();
                            xlWorkBook.Close();
                            excelApp.Quit();
                        }
                        
                        else if (dgvPOSData.Rows.Count > 1)
                        {
                            worksheet.Cells[2, 16] = DateTime.Now;
                            worksheet.Range["7:" + (row + 5)].Insert();
                            int increase = 0;
                            for (int i = 0; i < dgvPOSData.Rows.Count; i++)
                            {
                                if (i > 0)
                                {
                                    double LastWIPCode = Convert.ToDouble(dgvPOSData.Rows[i-1].Cells[0].Value.ToString());
                                    double CurrentWIPCode = Convert.ToDouble(dgvPOSData.Rows[i].Cells[0].Value.ToString());
                                    if (CurrentWIPCode - 1 == LastWIPCode)
                                    {
                                        worksheet.Range["K" + (i + increase +6) + ":Q" + (i + increase + 6)].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDot;
                                        for (int j = 0; j < dgvPOSData.Columns.Count; j++)
                                        {
                                            if (dgvPOSData.Rows[i].Cells[j].Value != null)
                                            {
                                                worksheet.Cells[i + increase + 6, j + 11] = dgvPOSData.Rows[i].Cells[j].Value.ToString();

                                            }
                                            else
                                            {
                                                //worksheet1.Cells[i + 5, j + 1] = "";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        worksheet.Range["K" + (i + increase + 6) + ":Q" + (i + increase + 6)].Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;                                        
                                        increase =increase+1;
                                        worksheet.Range["K" + (i + increase + 6) + ":Q" + (i + increase + 6)].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDot;
                                        for (int j = 0; j < dgvPOSData.Columns.Count; j++)
                                        {
                                            if (dgvPOSData.Rows[i].Cells[j].Value != null)
                                            {
                                                worksheet.Cells[i + increase + 6, j + 11] = dgvPOSData.Rows[i].Cells[j].Value.ToString();

                                            }
                                            else
                                            {
                                                //worksheet1.Cells[i + 5, j + 1] = "";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    worksheet.Range["K" + (i+6) + ":Q" + (i + 6)].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDot;
                                    for (int j = 0; j < dgvPOSData.Columns.Count; j++)
                                    {
                                        if (dgvPOSData.Rows[i].Cells[j].Value != null)
                                        {
                                            worksheet.Cells[i + 6, j + 11] = dgvPOSData.Rows[i].Cells[j].Value.ToString();

                                        }
                                        else
                                        {
                                            //worksheet1.Cells[i + 5, j + 1] = "";
                                        }
                                    }
                                }
                            }

                            // Saving the modified Excel file                        
                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                            string file = "POS_Checking ";
                            fName = file + "( " + date + " )";
                            worksheet.SaveAs(CDirectory.ToString() + @"\Report\POS_Checking\" + fName + ".xlsx");
                            xlWorkBook.Save();
                            xlWorkBook.Close();
                            excelApp.Quit();
                        }    
                    }
                    catch (System.Exception ex)
                    {
                        excelApp.DisplayAlerts = false;
                        xlWorkBook.Close();
                        excelApp.DisplayAlerts = true;
                        excelApp.Quit();
                        LbExportStatus.Text = "ការរក្សាទុកមានបញ្ហា ៖​ " + ex.Message + ", អាចមកពី File នេះកំពុងតែបើក !";
                    }

                    //Kill all Excel background process
                    var processes = from p in Process.GetProcessesByName("EXCEL")
                                    select p;
                    foreach (var process in processes)
                    {
                        if (process.MainWindowTitle.ToString().Trim() == "")
                            process.Kill();
                    }


                    LbExportStatus.Text = "ព្រីនបានជោគជ័យ​ !";
                    MessageBox.Show("ការព្រីនបានជោគជ័យ​ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(CDirectory.ToString() + @"\\Report\POS_Checking\" + fName + ".xlsx");
                    fName = "";
                    LbExportStatus.Visible = false;
                }
            }
        }
    }
}
