using MachineDeptApp.MsgClass;
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

namespace MachineDeptApp.TransferData
{
    public partial class SemiTransferComparisonForm : Form
    {
        InformationMsgClass InfoMsg = new InformationMsgClass();
        QuestionMsgClass QMsg = new QuestionMsgClass();
        ErrorMsgClass EMsg = new ErrorMsgClass();
        WarningMsgClass WMsg = new WarningMsgClass();
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();

        string ErrorText;

        public SemiTransferComparisonForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Shown += SemiTransferComparisonForm_Shown;
            this.btnSearch.Click += BtnSearch_Click;
            this.LbStatus.TextChanged += LbStatus_TextChanged;
            this.txtItemCode.TextChanged += TxtItemCode_TextChanged;
            this.txtItemName.TextChanged += TxtItemName_TextChanged;
            this.dgvSearch.CellPainting += DgvSearch_CellPainting;
        }

        private void DgvSearch_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex>=0)
            {
                if (dgvSearch.Columns[e.ColumnIndex].Name == "RegDateOBS" && dgvSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    dgvSearch.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Pink;
                }
            }
        }
        private void LbStatus_TextChanged(object sender, EventArgs e)
        {
            if (LbStatus.Text.ToString().Contains("WIP room មិនទាន់ទទួល") == true)
            {
                LbStatus.ForeColor = Color.Red;
            }
            else
            {
                if (LbStatus.Text.ToString().Contains("WIP room ទទួលរួចអស់ហើយ!") == true)
                {
                    LbStatus.ForeColor = Color.Green;
                }
                else
                {
                    LbStatus.ForeColor = Color.Black;
                }
            }
            LbStatus.Refresh();
        }
        private void TxtItemName_TextChanged(object sender, EventArgs e)
        {
            if(txtItemName.Text.Trim()!="")
                chkItemName.Checked = true;
            else
                chkItemName.Checked = false;
        }
        private void TxtItemCode_TextChanged(object sender, EventArgs e)
        {
            if(txtItemCode.Text.Trim()!="")
                chkItemCode.Checked = true;
            else
                chkItemCode.Checked = false;
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            Cursor = Cursors.WaitCursor;
            dgvSearch.Rows.Clear();

            string SQLConds = "";
            if (chkItemCode.Checked == true && txtItemCode.Text.Trim() != "")
            {
                string SearchValue = txtItemCode.Text;
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*", "%");
                    SQLConds += " AND WipCode LIKE '" + SearchValue+"'";                    
                }
                else
                    SQLConds += " AND WipCode = '" + SearchValue + "'";
            }
            if (chkItemName.Checked == true && txtItemName.Text.Trim() != "")
            {
                string SearchValue = txtItemName.Text;
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*", "");    
                }
                SQLConds += " AND WipDes LIKE '%" + SearchValue + "%'";
            }

            //Taking MC Data
            DataTable dtMC = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT PosNo, WipCode, WipDes, BoxNo, Qty, RegDate FROM tbWipTransactions WHERE DeleteState = 0 AND " +
                    "\nRegDate BETWEEN '"+DtDate.Value.ToString("yyyy-MM-dd")+" 00:00:00' AND '"+DtEndDate.Value.ToString("yyyy-MM-dd")+" 23:59:59'"+SQLConds+
                    " \nORDER BY RegDate ASC";
                Console.WriteLine(SQLQuery);
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery,cnn.con);
                sda.Fill(dtMC);
            }
            catch (Exception ex)
            {
                ErrorText = "Taking MC Data : " + ex.Message;
            }
            cnn.con.Close();

            //Taking OBS Data 
            if (ErrorText.Trim() == "" && dtMC.Rows.Count > 0)
            {
                string POSNoIN = "";
                foreach (DataRow row in dtMC.Rows)
                {
                    if (POSNoIN.Trim() == "")
                        POSNoIN = "'" + row["PosNo"].ToString() + "'";
                    else
                        POSNoIN += ", '" + row["PosNo"].ToString() + "'";
                }

                DataTable dtOBS = new DataTable();
                try
                {
                    cnnOBS.conOBS.Open();
                    string SQLQuery = "SELECT DONo, ItemCode, OKQty,Resv7 AS BoxNO, CreateDate FROM prgproductionresult " +
                        "\nWHERE DelFlag = 0 AND DONo IN ("+POSNoIN+") " +
                        "\nORDER BY CreateDate ASC";
                    //Console.WriteLine(SQLQuery);
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                    sda.Fill(dtOBS);
                }
                catch (Exception ex) 
                {
                    ErrorText = "Taking OBS Data : " + ex.Message;
                }
                cnnOBS.conOBS.Close();

                //Add to dtMC
                if (ErrorText.Trim() == "")
                {
                    dtMC.Columns.Add("QtyOBS");
                    dtMC.Columns.Add("RegDateOBS");
                    dtMC.AcceptChanges();
                    foreach (DataRow rowOBS in dtOBS.Rows)
                    {
                        string POSNo = rowOBS["DONo"].ToString();
                        string WipCode = rowOBS["ItemCode"].ToString();
                        string BoxNo = rowOBS["BoxNO"].ToString();
                        string Qty = rowOBS["OKQty"].ToString();
                        string RegDate = rowOBS["CreateDate"].ToString();

                        foreach (DataRow row in dtMC.Rows)
                        {
                            if (POSNo == row["PosNo"].ToString() && WipCode == row["WipCode"].ToString() && BoxNo == row["BoxNo"].ToString())
                            {
                                row["QtyOBS"] = Qty;
                                row["RegDateOBS"] = RegDate;
                                dtMC.AcceptChanges();
                                break;
                            }
                        }
                    }
                }
            }

            //Add to Dgv
            if (ErrorText.Trim() == "")
            {
                int FoundNotYet = 0;
                foreach (DataRow row in dtMC.Rows)
                {
                    dgvSearch.Rows.Add();
                    dgvSearch.Rows[dgvSearch.Rows.Count - 1].Cells["POSNo"].Value = row["PosNo"].ToString();
                    dgvSearch.Rows[dgvSearch.Rows.Count - 1].Cells["Code"].Value = row["WipCode"].ToString();
                    dgvSearch.Rows[dgvSearch.Rows.Count - 1].Cells["ItemName"].Value = row["WipDes"].ToString();
                    dgvSearch.Rows[dgvSearch.Rows.Count - 1].Cells["BoxNo"].Value = Convert.ToDouble(row["BoxNo"]);
                    dgvSearch.Rows[dgvSearch.Rows.Count - 1].Cells["Qty"].Value = Convert.ToDouble(row["Qty"]);
                    dgvSearch.Rows[dgvSearch.Rows.Count - 1].Cells["RegDate"].Value = Convert.ToDateTime(row["RegDate"]);

                    Nullable<double> QtyOBS = null;
                    if (row["QtyOBS"].ToString().Trim() != "")
                        QtyOBS = Convert.ToDouble(row["QtyOBS"]);
                    Nullable<DateTime> RegDateOBS = null;
                    if (row["RegDateOBS"].ToString().Trim() != "")
                        RegDateOBS = Convert.ToDateTime(row["RegDateOBS"]);
                    dgvSearch.Rows[dgvSearch.Rows.Count - 1].Cells["QtyOBS"].Value = QtyOBS;
                    dgvSearch.Rows[dgvSearch.Rows.Count - 1].Cells["RegDateOBS"].Value = RegDateOBS;

                    if (RegDateOBS == null)
                    {
                        FoundNotYet++;
                    }

                }

                dgvSearch.ClearSelection();
                if (dgvSearch.Rows.Count > 0)
                {
                    if (FoundNotYet == 0)
                        LbStatus.Text = "ទិន្នន័យត្រូវបាន WIP room ទទួលរួចអស់ហើយ!";
                    else
                        LbStatus.Text = "WIP room មិនទាន់ទទួល "+FoundNotYet.ToString("N0")+"  កាតុង";
                }
                else
                {
                    LbStatus.Text = "គ្មានទិន្នន័យវេរទេ!";
                }
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
            {
                LbStatus.Text = "មានបញ្ហា!";
                EMsg.AlertText = "មានបញ្ហា!\n" + ErrorText;
                EMsg.ShowingMsg();
            }
        }
        private void SemiTransferComparisonForm_Shown(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn col in dgvSearch.Columns)
            {
                //Console.WriteLine(col.Name);
            }
        }
    }
}
