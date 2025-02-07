using MachineDeptApp.MsgClass;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.NG_Input
{
    public partial class NGHistoryForm : Form
    {
        ErrorMsgClass EMsg = new ErrorMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        InformationMsgClass InfoMsg = new InformationMsgClass();
        SQLConnect cnn = new SQLConnect();

        string ErrorText;

        public NGHistoryForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.btnSearch.Click += BtnSearch_Click;
            this.btnExport.Click += BtnExport_Click;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvNGHistory.Rows.Count > 0)
            {
                QMsg.QAText = "តើអ្នកចង់ទាញទិន្នន័យចេញជា Excel មែន ឬទេ?";
                QMsg.UserClickedYes = false;
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "CSV file (*.csv)|*.csv";
                    saveDialog.FileName = "NG History " + DateTime.Now.ToString("yyyyMMdd HHmmss") + ".csv";
                    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;
                        try
                        {
                            //Write Column name
                            int columnCount = 0;
                            foreach (DataGridViewColumn DgvCol in dgvNGHistory.Columns)
                            {
                                if (DgvCol.Visible == true)
                                {
                                    columnCount = columnCount + 1;
                                }
                            }
                            string columnNames = "";

                            //String array for Csv
                            string[] outputCsv;
                            outputCsv = new string[dgvNGHistory.Rows.Count + 1];

                            //Set Column Name
                            for (int i = 0; i < columnCount; i++)
                            {
                                if (dgvNGHistory.Columns[i].Visible == true)
                                {
                                    columnNames += dgvNGHistory.Columns[i].HeaderText.ToString() + ",";
                                }
                            }
                            outputCsv[0] += columnNames;

                            //Row of data 
                            for (int i = 1; (i - 1) < dgvNGHistory.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    if (dgvNGHistory.Columns[j].Visible == true)
                                    {
                                        string Value = "";
                                        if (dgvNGHistory.Rows[i - 1].Cells[j].Value != null)
                                        {
                                            Value = dgvNGHistory.Rows[i - 1].Cells[j].Value.ToString();
                                        }
                                        //Fix don't separate if it contain '\n' or ','
                                        Value = "\"" + Value.Replace("\"", "\"\"") + "\"";
                                        outputCsv[i] += Value + ",";
                                    }
                                }
                            }

                            File.WriteAllLines(saveDialog.FileName, outputCsv, Encoding.UTF8);
                            Cursor = Cursors.Default;
                            MessageBox.Show("ទាញទិន្នន័យចេញរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            Cursor = Cursors.Default;
                            MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            dgvNGHistory.Rows.Clear();
            ErrorText = "";

            //SQLCondition
            string SQLConds = "WHERE RegDate BETWEEN '"+DtStart.Value.ToString("yyyy-MM-dd")+" 00:00:00' AND '"+DtEnd.Value.ToString("yyyy-MM-dd") + " 23:59:59' ";
            if (txtRMCode.Text.Trim() != "")
            {
                SQLConds += "AND T1.ItemCod = '"+txtRMCode.Text+"' ";
            }
            if (txtRMName.Text.Trim() != "")
            {
                SQLConds += "AND ItemName = '" + txtRMName.Text + "' ";
            }

            //Taking Data
            DataTable dtSearchResult = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT SysNo, T1.ItemCode, ItemName, NGQty, RegBy, RegDate, T1.Remarks FROM " +
                    "\n(SELECT * FROM tbNGHistory WHERE ReqStat=1) T1 " +
                    "\nINNER JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') T2 ON T1.ItemCode=T2.ItemCode \n" + SQLConds +
                    "\nORDER BY RegDate ASC ";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtSearchResult);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnn.con.Close();

            //Add to Dgv
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtSearchResult.Rows)
                {
                    dgvNGHistory.Rows.Add();
                    dgvNGHistory.Rows[dgvNGHistory.Rows.Count-1].Cells["RMCode"].Value = row["ItemCode"].ToString();
                    dgvNGHistory.Rows[dgvNGHistory.Rows.Count - 1].Cells["RMName"].Value = row["ItemName"].ToString();
                    dgvNGHistory.Rows[dgvNGHistory.Rows.Count - 1].Cells["Qty"].Value = Convert.ToDouble(row["NGQty"]);
                    dgvNGHistory.Rows[dgvNGHistory.Rows.Count - 1].Cells["Remarks"].Value = row["Remarks"].ToString();
                    dgvNGHistory.Rows[dgvNGHistory.Rows.Count - 1].Cells["RegDate"].Value = Convert.ToDateTime(row["RegDate"]);
                    dgvNGHistory.Rows[dgvNGHistory.Rows.Count - 1].Cells["RegBy"].Value = row["RegBy"].ToString();
                }
                dgvNGHistory.ClearSelection();
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
            {
                EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                EMsg.ShowingMsg();
            }
        }
    }
}
