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
using Excel = Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using System.IO;
using System.Diagnostics;

namespace MachineDeptApp.SemiNGReq
{
    public partial class SemiNGReqSearchForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        public static string SysNoForNextForm;
        public DataTable dtPrint;
        string fName;

        public SemiNGReqSearchForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.dgvAllData.SelectionChanged += DgvAllData_SelectionChanged;

        }

        private void CleardtPrint()
        {
            dtPrint = new DataTable();
            dtPrint.Columns.Add("SysNo");
            dtPrint.Columns.Add("WipCode");
            dtPrint.Columns.Add("WipDes");
            dtPrint.Columns.Add("PIN");
            dtPrint.Columns.Add("Wire");
            dtPrint.Columns.Add("Length");
            dtPrint.Columns.Add("Qty");
            dtPrint.Columns.Add("RecSection");
            dtPrint.Columns.Add("RegDate");
            dtPrint.Columns.Add("RegBy");

        }

        private void DgvAllData_SelectionChanged(object sender, EventArgs e)
        {
            SysNoForNextForm = "";
            if (dgvAllData.SelectedCells.Count > 0 && dgvAllData.Rows[dgvAllData.CurrentCell.RowIndex].Cells[12].Value.ToString()=="False")
            {
                SysNoForNextForm = dgvAllData.Rows[dgvAllData.CurrentCell.RowIndex].Cells[0].Value.ToString();
                btnUpdate.Enabled = true;
                btnUpdate.BackColor = Color.White;
                btnDelete.Enabled = true;
                btnDelete.BackColor = Color.White;
            }
            else
            {
                btnUpdate.Enabled = false;
                btnUpdate.BackColor = Color.FromKnownColor(KnownColor.LightGray);
                btnDelete.Enabled = false;
                btnDelete.BackColor = Color.FromKnownColor(KnownColor.LightGray);
            }
        }

        private void btnPrintEnableCheck()
        {
            if (dtPrint.Rows.Count > 0)
            {
                btnPrint.Enabled = true;
                btnPrint.BackColor = Color.White;
            }
            else
            {
                btnPrint.Enabled = false;
                btnPrint.BackColor = Color.FromKnownColor(KnownColor.LightGray);
            }
        }

        private void CheckDeleteStatColor()
        {
            for (int i = 0; i < dgvAllData.Rows.Count; i++)
            {
                if (dgvAllData.Rows[i].Cells[12].Value.ToString()=="True")
                {
                    dgvAllData.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;

                }
            }
        }

        private void SemiNGReqSearchForm_Load(object sender, EventArgs e)
        {
            string[] RecLoc = new string[] { "ទាំងអស់","WIP", "Assy"};
            for (int i = 0; i < RecLoc.Length; i++)
            {
                CboRecLocSearch.Items.Add(RecLoc[i]);
            }
            CboRecLocSearch.SelectedIndex=0;

            string[] DelStat = new string[] { "មិនទាន់លុប", "លុបរួច", "ទាំងអស់"};
            for (int i = 0; i < DelStat.Length; i++)
            {
                CboDelStat.Items.Add(DelStat[i]);
            }
            CboDelStat.SelectedIndex = 0;

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor= Cursors.WaitCursor;
            LbExportStatus.Text = "កំពុងស្វែងរក . . . . . ";
            LbExportStatus.Visible = true;
            LbExportStatus.Refresh();
            dgvAllData.Rows.Clear();
            CleardtPrint();
            DataTable dtSearchCon=new DataTable();
            dtSearchCon.Columns.Add("Columns");
            dtSearchCon.Columns.Add("Value");
            if (txtWipDes.Text.ToString().Trim() != "")
            {
                dtSearchCon.Rows.Add("WipDes LIKE"," '%"+txtWipDes.Text.ToString()+"%' ");
            }
            if (CboRecLocSearch.SelectedIndex != 0)
            {
                dtSearchCon.Rows.Add("RecSection =", " '" + CboRecLocSearch.Text.ToString() + "' ");
            }
            if (CboDelStat.SelectedIndex != 2)
            {
                if (CboDelStat.SelectedIndex == 0)
                {
                    dtSearchCon.Rows.Add("DeleteStat =", " '0' ");
                }
                else
                {
                    dtSearchCon.Rows.Add("DeleteStat =", " '1' ");
                }
                
            }
            if (chkDate.Checked == true)
            {
                DateTime dateS = Convert.ToDateTime(DtInput.Value.ToString());
                DateTime dateE = Convert.ToDateTime(DtEndInput.Value.ToString()); 
                dtSearchCon.Rows.Add("RegDate BETWEEN", " '" + dateS.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dateE.ToString("yyyy-MM-dd") + " 23:59:59' ");
            }
            string SQLCondition = "";
            foreach (DataRow row in dtSearchCon.Rows)
            {
                if (SQLCondition.Trim() == "")
                {
                    SQLCondition = "WHERE " + row[0] + row[1];
                }
                else
                {
                    SQLCondition = SQLCondition +"AND " + row[0] + row[1];
                }
            }

            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT SysNo, WipCode, WipDes, Remarks1, Remarks2, Remarks3, Qty, RecSection, RegDate, RegBy, UpdateDate, UpdateBy, DeleteStat FROM  tbSemiNGReq "+
                                                                                "INNER JOIN tbMasterItem ON tbMasterItem.ItemCode= tbSemiNGReq.WipCode "+ SQLCondition+
                                                                                "GROUP BY SysNo, WipCode, WipDes, Remarks1, Remarks2, Remarks3, Qty, RecSection, RegDate, RegBy, UpdateDate, UpdateBy, DeleteStat "+
                                                                                "ORDER BY RegDate ASC", cnn.con);
                DataTable table = new DataTable();
                da.Fill(table);
                //to get rows
                foreach (DataRow row in table.Rows)
                {
                    if (row[12].ToString() == "0")
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], Convert.ToDouble(row[6].ToString()), row[7], Convert.ToDateTime(row[8].ToString()), row[9], Convert.ToDateTime(row[10].ToString()), row[11], false);
                        dtPrint.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], Convert.ToDouble(row[6].ToString()), row[7], Convert.ToDateTime(row[8].ToString()), row[9]);
                    }
                    else
                    {
                        dgvAllData.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], Convert.ToDouble(row[6].ToString()), row[7], Convert.ToDateTime(row[8].ToString()), row[9], Convert.ToDateTime(row[10].ToString()), row[11],true);
                    }
                }
            }
            catch
            {
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LbExportStatus.Visible = false;
            LbTotalResult.Text = "រកឃើញទិន្នន័យ ៖ " + dgvAllData.Rows.Count.ToString("N0");
            LbTotalResult.Visible = true;
            LbTotalResult.Refresh();
            CheckDeleteStatColor();
            btnPrintEnableCheck();
            dgvAllData.ClearSelection();
            Cursor= Cursors.Default;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult DLR = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនដែរ ឬទេ ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DLR == DialogResult.Yes)
            {
                DateTime DelDate = DateTime.Now;
                try
                {
                    cnn.con.Open();
                    string query = "UPDATE tbSemiNGReq SET " +
                                        "DeleteStat=1," +
                                        "UpdateDate='" + DelDate + "'," +
                                        "UpdateBy=N'" + MenuFormV2.UserForNextForm + "'" +
                                        "WHERE SysNo = '" + SysNoForNextForm + "';";
                    SqlCommand cmd = new SqlCommand(query, cnn.con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("លុបទិន្នន័យរួចរាល់ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvAllData.Rows[dgvAllData.CurrentCell.RowIndex].Cells[10].Value = DelDate;
                    dgvAllData.Rows[dgvAllData.CurrentCell.RowIndex].Cells[11].Value = MenuFormV2.UserForNextForm;
                    dgvAllData.Rows[dgvAllData.CurrentCell.RowIndex].Cells[12].Value = true;
                    for (int i = 0; i < dtPrint.Rows.Count; i++)
                    {
                        if (dtPrint.Rows[i][0].ToString() == SysNoForNextForm)
                        {
                            dtPrint.Rows.RemoveAt(i);
                        }
                    }
                    CheckDeleteStatColor();
                    btnPrintEnableCheck();
                    SysNoForNextForm = "";
                    dgvAllData.ClearSelection();

                }
                catch(Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញជាមុន ! \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!\n"+ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnn.con.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SemiNGReqUpdateForm Snruf = new SemiNGReqUpdateForm(this);
            Snruf.ShowDialog();
            dgvAllData.ClearSelection();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            LbExportStatus.Text = "កំពុងបង្កើត File សូមកុំចុចផ្ដេសផ្ដាស​ . . . .";
            LbExportStatus.Visible = true;
            LbExportStatus.Refresh();

            //ឆែករកមើល Folder បើគ្មាន => បង្កើត
            string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\SemiNGREQ";
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }

            var CDirectory = Environment.CurrentDirectory;
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\SemiNGREQTemplate.xlsx", Editable: true);
            //add to All Data
            try
            {
                Excel.Worksheet wsBRC = (Excel.Worksheet)xlWorkBook.Sheets["RachhanSystem"];
                wsBRC.Cells[4, 2] = DateTime.Now.ToString();

                int row = dtPrint.Rows.Count;
                int RowFormodify = row + 6;
                wsBRC.Range["8:" + RowFormodify].Insert();
                for (int i = 0; i < dtPrint.Rows.Count; i++)
                {
                    for (int j = 1; j < dtPrint.Columns.Count; j++)
                    {
                        wsBRC.Cells[i + 7, j] = dtPrint.Rows[i][j].ToString();

                    }
                }

                wsBRC.Range["A7:I" + RowFormodify].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                wsBRC.Range["A7:I" + RowFormodify].Font.Name = "Khmer OS Battambang";
                wsBRC.Range["A7:I" + RowFormodify].Font.Size = 10;
                wsBRC.Range["A7:I" + RowFormodify].Font.Bold = false;
                                
                // Saving the modified Excel file                        
                string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                string file = "Semi NG REQ ";
                fName = file + "( " + date + " )";
                wsBRC.SaveAs(CDirectory.ToString() + @"\Report\SemiNGREQ\" + fName + ".xlsx");
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

                LbExportStatus.Text = "ព្រីនបានជោគជ័យ​ !";
                Cursor = Cursors.Default;
                MessageBox.Show("ការព្រីនបានជោគជ័យ​ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(CDirectory.ToString() + @"\Report\SemiNGREQ\" + fName + ".xlsx");
                fName = "";
                LbExportStatus.Visible = false;

            }
            catch (System.Exception ex)
            {
                excelApp.DisplayAlerts = false;
                xlWorkBook.Close();
                excelApp.DisplayAlerts = true;
                excelApp.Quit();
                Cursor = Cursors.Default;
                LbExportStatus.Text = "ការរក្សាទុកមានបញ្ហា ៖​ " + ex.Message + ", អាចមកពី File នេះកំពុងតែបើក !";
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
    }
}
