using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Excel = Microsoft.Office.Interop.Excel;


namespace MachineDeptApp.NG_Input
{
    public partial class NGInputForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        string SysNSelected;
        string ReqStSelected;
        string fName;

        public NGInputForm()
        {
            InitializeComponent();
            cnn.Connection();
            cnnOBS.Connection();
            this.dgvNGalready.CellClick += DgvNGalready_CellClick;
            this.dgvNGalready.KeyDown += DgvNGalready_KeyDown;

        }

        private void DgvNGalready_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (SysNSelected.Trim() != "" && ReqStSelected == "False")
                {
                    DialogResult DLR = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DLR == DialogResult.Yes)
                    {
                        string ItemCode = dgvNGalready.Rows[dgvNGalready.CurrentCell.RowIndex].Cells[0].Value.ToString();
                        try
                        {
                            cnn.con.Open();
                            //Delete in tbNGHistory                     
                            SqlCommand cmd = new SqlCommand("DELETE FROM tbNGHistory WHERE SysNo ='" + SysNSelected + "' AND ItemCode='"+ItemCode+"' ;", cnn.con);
                            cmd.ExecuteNonQuery();
                            //Update tbTrasaction
                            SqlCommand cmd1 = new SqlCommand("UPDATE tbSDMCAllTransaction SET CancelStatus=1 WHERE SysNo ='" + SysNSelected + "' AND Code='"+ItemCode+"' ;", cnn.con);
                            cmd1.ExecuteNonQuery();
                            MessageBox.Show("Successfully deleted !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvNGalready.Rows.RemoveAt(dgvNGalready.CurrentCell.RowIndex);
                            dgvNGalready.Refresh();
                            dgvNGalready.ClearSelection();
                            SysNSelected = "";
                            ReqStSelected = "";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + "\nSomething wrong ! Please check the connection first !\nOr contact to developer !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        cnn.con.Close();
                    }
                }
            }
            else
            {

            }
        }

        private void DgvNGalready_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SysNSelected = "";
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvNGalready.Rows[e.RowIndex];
                SysNSelected = row.Cells[6].Value.ToString();
                ReqStSelected = row.Cells[3].Value.ToString();
            }
            else
            {

            }
        }

        private void btnSemiSet_Click(object sender, EventArgs e)
        {
            SysNSelected = "";
            ReqStSelected = "";
            NGInputBySetForm Nibsf = new NGInputBySetForm(this);
            Nibsf.ShowDialog();
        }

        private void btnScale_Click(object sender, EventArgs e)
        {
            SysNSelected = "";
            ReqStSelected = "";
            NGInputByUncountForm Nibuf = new NGInputByUncountForm(this);
            Nibuf.ShowDialog();
        }

        private void NGInputForm_Load(object sender, EventArgs e)
        {
            try
            {
                //Take last SysNo
                cnn.con.Open();
                SqlDataAdapter da = new SqlDataAdapter("Select * From  (Select * From tbNGHistory Where RegDate Between '" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59' AND ReqStat=0) t1 INNER JOIN (Select ItemCode,ItemName From tbMasterItem Where Len(ItemCode)<5 Group By ItemCode, ItemName) t2 ON t1.ItemCode=t2.ItemCode Order By t1.SysNo ASC", cnn.con);
                DataTable TodayData = new DataTable();
                da.Fill(TodayData);
                if (TodayData.Rows.Count > 0)
                {
                    for (int i = 0; i < TodayData.Rows.Count; i++)
                    {
                        bool stat;
                        if (TodayData.Rows[i][3].ToString() == "0")
                        {
                            stat = false;

                        }
                        else
                        {
                            stat = true;
                        }
                        dgvNGalready.Rows.Add(TodayData.Rows[i][1].ToString(), TodayData.Rows[i][7].ToString(), Convert.ToDouble(TodayData.Rows[i][2].ToString()), stat, Convert.ToDateTime(TodayData.Rows[i][4].ToString()), TodayData.Rows[i][5].ToString(), TodayData.Rows[i][0].ToString());
                    }
                }
                dgvNGalready.ClearSelection();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
            }
            cnn.con.Close();
        }

        private void btnCountable_Click(object sender, EventArgs e)
        {
            SysNSelected = "";
            ReqStSelected = "";
            NGInputByCountForm Nibcf = new NGInputByCountForm(this);
            Nibcf.ShowDialog();

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvNGalready.Rows.Count > 0)
            {
                DialogResult DLR = MessageBox.Show("តើអ្នកចង់ព្រីនមែនដែរឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    LbExportStatus.Visible = true;
                    LbExportStatus.Text = "កំពុងបង្កើត File​ . . . .";
                    LbExportStatus.Refresh();
                    DataTable dtStatus = new DataTable();
                    dtStatus.Columns.Add("SysNo", typeof(string));
                    DataTable dtPrint = new DataTable();
                    dtPrint.Columns.Add("Code", typeof(string));
                    dtPrint.Columns.Add("Items", typeof(string));
                    dtPrint.Columns.Add("Qty", typeof(double));

                    for (int i = 0; i < dgvNGalready.Rows.Count; i++)
                    {
                        int count = 0;
                        //Add to dtStatus
                        dtStatus.Rows.Add(dgvNGalready.Rows[i].Cells[6].Value.ToString());

                        //Add to dtPrint
                        //Check have or not
                        for (int j = 0; j < dtPrint.Rows.Count; j++)
                        {
                            if (dtPrint.Rows[j][0].ToString() == dgvNGalready.Rows[i].Cells[0].Value.ToString())
                            {
                                count = count + 1;
                            }
                        }
                        //If already have

                        if (count > 0)
                        {
                            for (int j = 0; j < dtPrint.Rows.Count; j++)
                            {
                                if (dtPrint.Rows[j][0].ToString() == dgvNGalready.Rows[i].Cells[0].Value.ToString())
                                {
                                    dtPrint.Rows[j][2] = Convert.ToDouble(dtPrint.Rows[j][2].ToString()) + Convert.ToDouble(dgvNGalready.Rows[i].Cells[2].Value.ToString());
                                }
                            }
                        }
                        //If not yet have
                        else
                        {
                            dtPrint.Rows.Add(dgvNGalready.Rows[i].Cells[0].Value.ToString(), dgvNGalready.Rows[i].Cells[1].Value.ToString(), Convert.ToDouble(dgvNGalready.Rows[i].Cells[2].Value.ToString()));
                        }

                    }

                    //Convert total
                    for (int i = 0; i < dtPrint.Rows.Count; i++)
                    {
                        dtPrint.Rows[i][2] = Math.Round(Convert.ToDouble(dtPrint.Rows[i][2].ToString()), 0, MidpointRounding.AwayFromZero);
                    }
                    int error = 0;

                    //Add type of Count/Uncount
                    try
                    {
                        string RMCodeIN = "";
                        foreach (DataRow row in dtPrint.Rows)
                        {
                            if (RMCodeIN.Trim() == "")
                            {
                                RMCodeIN = "'" + row["Code"].ToString() +"'";
                            }
                            else
                            {
                                RMCodeIN += ", '" + row["Code"].ToString() + "'";
                            }
                        }
                        cnnOBS.conOBS.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM mstitem WHERE DelFlag=0 AND ItemCode IN ("+RMCodeIN+") ORDER BY ItemCode ASC;", cnnOBS.conOBS);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        dtPrint.Columns.Add("MatCalcFlag");
                        foreach (DataRow row in dtPrint.Rows)
                        {
                            row["MatCalcFlag"] = "0";
                            foreach (DataRow rowOBS in dt.Rows)
                            {
                                if (row["Code"].ToString() == rowOBS["ItemCode"].ToString())
                                {
                                    row["MatCalcFlag"] = rowOBS["MatCalcFlag"].ToString();
                                    break;
                                }
                            }
                        }
                        dtPrint.AcceptChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\nSomething wrong ! Please check the connection first !\nOr contact to developer !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        error = error + 1;
                    }
                    //Update to SQL
                    if(error == 0)
                    {
                        for (int i = 0; i < dtStatus.Rows.Count; i++)
                        {
                            try
                            {
                                cnn.con.Open();
                                string query = "UPDATE tbNGHistory SET " +
                                                    "ReqStat=1" +
                                                    "WHERE SysNo = '" + dtStatus.Rows[i][0] + "';";
                                SqlCommand cmd = new SqlCommand(query, cnn.con);
                                cmd.ExecuteNonQuery();

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message + "\nSomething wrong ! Please check the connection first !\nOr contact to developer !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                error = error + 1;
                            }
                            cnn.con.Close();
                        }

                    }

                    if (error == 0)
                    {
                        //Print excel
                        var CDirectory = Environment.CurrentDirectory;
                        Excel.Application excelApp = new Excel.Application();
                        Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\NG_Calculate.xlsx", Editable: true);
                                                
                        try
                        {
                            //Countable
                            Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["Countable"];
                            int Countable = 0;
                            foreach (DataRow row in dtPrint.Rows)
                            {
                                if (row["MatCalcFlag"].ToString() == "0")
                                {
                                    Countable++;
                                }
                            }
                            if (Countable > 1)
                            {
                                worksheet.Range["5:" + (Countable - 1 + 4)].Insert();
                                worksheet.Range["A4:C" + (Countable - 1 + 4)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                            }
                            worksheet.Cells[2, 2] = DateTime.Now;
                            int RowIndex = 4;
                            for (int i = 0; i < dtPrint.Rows.Count; i++)
                            {
                                if (dtPrint.Rows[i]["MatCalcFlag"].ToString() == "0")
                                {
                                    for (int j = 0; j < dtPrint.Columns.Count - 1; j++)
                                    {
                                        if (dtPrint.Rows[i][j] != null)
                                        {
                                            worksheet.Cells[RowIndex, j + 1] = dtPrint.Rows[i][j];
                                        }
                                    }
                                    RowIndex++;
                                }
                            }

                            //Uncount
                            Excel.Worksheet worksheetUncount = (Excel.Worksheet)xlWorkBook.Sheets["Uncountable"];
                            int Uncountable = 0;
                            foreach (DataRow row in dtPrint.Rows)
                            {
                                if (row["MatCalcFlag"].ToString() == "1")
                                {
                                    Uncountable++;
                                }
                            }
                            if (Uncountable > 1)
                            {
                                worksheetUncount.Range["5:" + (Uncountable - 1 + 4)].Insert();
                                worksheetUncount.Range["A4:C" + (Uncountable - 1 + 4)].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                            }
                            worksheetUncount.Cells[2, 2] = DateTime.Now;
                            RowIndex = 4;
                            for (int i = 0; i < dtPrint.Rows.Count; i++)
                            {
                                if (dtPrint.Rows[i]["MatCalcFlag"].ToString() == "1")
                                {
                                    for (int j = 0; j < dtPrint.Columns.Count - 1; j++)
                                    {
                                        if (dtPrint.Rows[i][j] != null)
                                        {
                                            worksheetUncount.Cells[RowIndex, j + 1] = dtPrint.Rows[i][j];
                                        }
                                    }
                                    RowIndex++;
                                }
                            }

                            // Saving the modified Excel file
                            string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                            string file = "NG_Calculated ";
                            fName = file + "( " + date + " )";
                            worksheet.SaveAs(CDirectory.ToString() + @"\Report\NG\" + fName + ".xlsx");
                            xlWorkBook.Save();
                            xlWorkBook.Close();
                            excelApp.Quit();

                        }
                        catch (Exception ex)
                        {
                            excelApp.DisplayAlerts = false;
                            xlWorkBook.Close();
                            excelApp.DisplayAlerts = true;
                            excelApp.Quit();
                            LbExportStatus.Text = "ការរក្សាទុកមានបញ្ហា ៖​ " + ex.Message + ", អាចមកពី File នេះកំពុងតែបើក !";
                        }


                        //Refres dgv
                        dgvNGalready.Rows.Clear();
                        try
                        {
                            //Take last SysNo
                            cnn.con.Open();
                            SqlDataAdapter da = new SqlDataAdapter("Select * From  (Select * From tbNGHistory Where RegDate Between '" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59' AND ReqStat=0) t1 INNER JOIN (Select ItemCode,ItemName From tbMasterItem Where Len(ItemCode)<5) t2 ON t1.ItemCode=t2.ItemCode Order By t1.SysNo ASC", cnn.con);
                            DataTable TodayData = new DataTable();
                            da.Fill(TodayData);
                            if (TodayData.Rows.Count > 0)
                            {
                                for (int i = 0; i < TodayData.Rows.Count; i++)
                                {
                                    bool stat;
                                    if (TodayData.Rows[i][3].ToString() == "0")
                                    {
                                        stat = false;

                                    }
                                    else
                                    {
                                        stat = true;
                                    }
                                    dgvNGalready.Rows.Add(TodayData.Rows[i][1].ToString(), TodayData.Rows[i][7].ToString(), Convert.ToDouble(TodayData.Rows[i][2].ToString()), stat, Convert.ToDateTime(TodayData.Rows[i][4].ToString()), TodayData.Rows[i][5].ToString(), TodayData.Rows[i][0].ToString());
                                }
                            }
                            dgvNGalready.ClearSelection();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        cnn.con.Close();

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
                        System.Diagnostics.Process.Start(CDirectory.ToString() + @"\\Report\NG\" + fName + ".xlsx");
                        fName = "";
                        LbExportStatus.Visible = false;
                    }
                    else
                    {
                        LbExportStatus.Text = "ព្រីនមានបញ្ហា​ !";
                        MessageBox.Show("ព្រីនមានបញ្ហា​ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    }
                }
            }
        }

    }
}
