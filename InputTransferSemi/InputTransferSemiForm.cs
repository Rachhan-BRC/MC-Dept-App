using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MachineDeptApp.InputTransferSemi
{
    public partial class InputTransferSemiForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        ErrorReportAsTxt OutputError = new ErrorReportAsTxt();
        SqlCommand cmd;
        public static string Barcode;
        DataTable dtSaveError;
        DataTable dtCuttingStock;

        string ErrorText;

        public InputTransferSemiForm()
        {
            InitializeComponent();
            cnn.Connection();
            cnnOBS.Connection();
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;

            //Dgv
            this.dgvScannedTag.RowsAdded += DgvScannedTag_RowsAdded;
            this.dgvScannedTag.RowsRemoved += DgvScannedTag_RowsRemoved;
            this.dgvScannedTag.CurrentCellChanged += DgvScannedTag_CurrentCellChanged;
            //this.dgvTotalConsumption.CellFormatting += DgvTotalConsumption_CellFormatting;
            //this.dgvTotalConsumption.CellDoubleClick += DgvTotalConsumption_CellDoubleClick;

        }

        //Dgv
        private void DgvScannedTag_CurrentCellChanged(object sender, EventArgs e)
        {
            dgvPOSConsumption.Rows.Clear();
            if (dgvScannedTag.SelectedCells.Count > 0 && dgvScannedTag.CurrentCell != null && dgvScannedTag.CurrentCell.RowIndex > -1)
            {
                ErrorText = "";
                Cursor = Cursors.WaitCursor;

                //Taking OBS Data
                DataTable dtConsumption = new DataTable();
                try
                {
                    cnnOBS.conOBS.Open();
                    string SQLQuery = "SELECT DONo, ConsumpSeqNo, T2.ItemCode, ItemName, MatTypeName, " +
                                                    "\nCASE " +
                                                    "\n\tWHEN MatTypeName='Wire' THEN ROUND(BOMQty,3) " +
                                                    "\n\tWHEN MatTypeName='Connector' THEN ROUND(BOMQty,3) " +
                                                    "\n\tWHEN MatTypeName='Terminal' THEN ROUND(BOMQty,3) " +
                                                    "\n\tELSE ROUND(BOMQty,3) " +
                                                    "\nEND AS BOMQty FROM " +
                                                    "\n(SELECT * FROM prgproductionorder WHERE LineCode='MC1') T1 " +
                                                    "\nINNER JOIN (SELECT * FROM prgconsumtionorder) T2 ON T1.ProductionCode=T2.ProductionCode " +
                                                    "\nINNER JOIN (SELECT * FROM mstitem) T3 ON T2.ItemCode=T3.ItemCode " +
                                                    "\nINNER JOIN (SELECT * FROM MstMatType) T4 ON T3.MatTypeCode=T4.MatTypeCode " +
                                                    "\nWHERE DONo='" + dgvScannedTag.Rows[dgvScannedTag.CurrentCell.RowIndex].Cells["POSNo"].Value.ToString() + "' ";
                    //Console.WriteLine(SQLQuery);
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                    sda.Fill(dtConsumption);

                }
                catch (Exception ex)
                {
                    ErrorText = "OBS : " + ex.Message;
                }
                cnnOBS.conOBS.Close();

                //Taking MC Stock
                if (ErrorText.Trim() == "" && dtConsumption.Rows.Count > 0)
                {
                    dtConsumption.Columns.Add("UsageQty");
                    dtConsumption.Columns.Add("POSStock");
                    dtConsumption.Columns.Add("OtherStock");
                    dtConsumption.AcceptChanges();
                    string CodeIN = "";
                    foreach (DataRow row in dtConsumption.Rows)
                    {
                        if (CodeIN.Trim() == "")
                        {
                            CodeIN = "'" + row["ItemCode"].ToString() + "'";
                        }
                        else
                        {
                            CodeIN += ", '" + row["ItemCode"].ToString() + "'";
                        }
                    }
                    DataTable dtPOSStock = new DataTable();
                    DataTable dtOtherStock = new DataTable();
                    try
                    {
                        cnn.con.Open();
                        string SQLQuery = "SELECT POSNo, Code, SUM (StockValue) AS StockValue FROM " +
                            "\n(SELECT TbTst.Code, TbTst.POSNo AS LotNo, StockValue, " +
                            "\nCASE " +
                            "\n\tWHEN NOT TRIM(TbTst.POSNo) = '' THEN " +
                            "\n\t\tCASE " +
                            "\n\t\t\tWHEN LEFT(TbTst.POSNo,2) = 'SD' THEN TbAllo.POSNo " +
                            "\n\t\t\tWHEN RIGHT(TbTst.POSNo,6) = 'NGRate' THEN LEFT(TbTst.POSNo,10) " +
                            "\n\t\t\tELSE TbTst.POSNo " +
                            "\n\t\tEND " +
                            "\n\tELSE '' " +
                            "\nEND AS POSNo, TbRegDate.RegDate FROM " +
                            "\n(SELECT Code, POSNo, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='MC1' AND RegDate>'2024-11-26' GROUP BY Code, POSNo) TbTst " +
                            "\nLEFT JOIN (SELECT * FROM tbSDAllocateStock) TbAllo ON TbTst.POSNo=TbAllo.SysNo " +
                            "\nINNER JOIN (SELECT Code, POSNo, MIN(RegDate) AS RegDate FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='MC1' AND Funct=2 AND NOT TRIM(POSNo) = '' GROUP BY Code, POSNo) TbRegDate ON TbTst.Code=TbRegDate.Code AND TbTst.POSNo=TbRegDate.POSNo) X " +
                            "\nWHERE StockValue>0 AND POSNo='" + dgvScannedTag.Rows[dgvScannedTag.CurrentCell.RowIndex].Cells["POSNo"].Value.ToString() + "' " +
                            "\nGROUP BY POSNo, Code " +
                            "\nORDER BY Code ASC";
                        //Console.WriteLine(SQLQuery);
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        sda.Fill(dtPOSStock);

                        SQLQuery = "SELECT Code, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction " +
                            "\nWHERE CancelStatus=0 AND LocCode='MC1' AND POSNo = '' AND Code IN (" + CodeIN + ") " +
                            "\nGROUP BY Code";
                        //Console.WriteLine(SQLQuery);
                        sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        sda.Fill(dtOtherStock);
                    }
                    catch (Exception ex)
                    {
                        ErrorText = "MC DB : " + ex.Message;
                    }
                    cnn.con.Close();

                    if (ErrorText.Trim() == "")
                    {
                        foreach (DataRow row in dtConsumption.Rows)
                        {
                            double UseQty = Convert.ToDouble(row["BOMQty"]) * Convert.ToDouble(dgvScannedTag.Rows[dgvScannedTag.CurrentCell.RowIndex].Cells["Qty"].Value.ToString());
                            row["UsageQty"] = UseQty;

                            //POSStock
                            double Qty = 0;
                            foreach (DataRow rowPOS in dtPOSStock.Rows)
                            {
                                if (row["ItemCode"].ToString() == rowPOS["Code"].ToString())
                                {
                                    Qty = Convert.ToDouble(rowPOS["StockValue"].ToString());
                                    break;
                                }
                            }
                            row["POSStock"] = Qty;

                            //OtherStock
                            Qty = 0;
                            foreach (DataRow rowOther in dtOtherStock.Rows)
                            {
                                if (row["ItemCode"].ToString() == rowOther["Code"].ToString())
                                {
                                    Qty = Convert.ToDouble(rowOther["StockValue"].ToString());
                                    break;
                                }
                            }
                            row["OtherStock"] = Qty;
                        }
                    }
                }

                //Add to DGV
                foreach (DataRow row in dtConsumption.Rows)
                {
                    dgvPOSConsumption.Rows.Add();
                    dgvPOSConsumption.Rows[dgvPOSConsumption.Rows.Count - 1].Cells["RMCode"].Value = row["ItemCode"];
                    dgvPOSConsumption.Rows[dgvPOSConsumption.Rows.Count - 1].Cells["RMName"].Value = row["ItemName"];
                    dgvPOSConsumption.Rows[dgvPOSConsumption.Rows.Count - 1].Cells["BOM"].Value = Convert.ToDouble(row["BOMQty"]);
                    dgvPOSConsumption.Rows[dgvPOSConsumption.Rows.Count - 1].Cells["UsageQty"].Value = Convert.ToDouble(row["UsageQty"]);
                    dgvPOSConsumption.Rows[dgvPOSConsumption.Rows.Count - 1].Cells["POSStock"].Value = Convert.ToDouble(row["POSStock"]);
                    dgvPOSConsumption.Rows[dgvPOSConsumption.Rows.Count - 1].Cells["StockFree"].Value = Convert.ToDouble(row["OtherStock"]);
                }

                Cursor = Cursors.Default;
                if (ErrorText.Trim() == "")
                {
                    dgvPOSConsumption.ClearSelection();
                }
                else
                {
                    MessageBox.Show("មានបញ្ហា!\n" + ErrorText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void DgvScannedTag_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            AssignSeqNo();
            this.dgvPOSConsumption.Rows.Clear();
            Cursor = Cursors.Default;
        }
        private void DgvScannedTag_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            AssignSeqNo();
        }

        /*
        private void DgvTotalConsumption_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                if (Convert.ToDouble(dgvTotalConsumption.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) >= Convert.ToDouble(dgvTotalConsumption.Rows[e.RowIndex].Cells[2].Value.ToString()))
                {
                    e.CellStyle.ForeColor = Color.Green;
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
            }
        }
        private void DgvTotalConsumption_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                SelectedRMCode = dgvTotalConsumption.Rows[e.RowIndex].Cells[0].Value.ToString();
                SelectedRMName = dgvTotalConsumption.Rows[e.RowIndex].Cells[1].Value.ToString();
                InputTransferSemiConsumptionDetailsForm Ifscdf = new InputTransferSemiConsumptionDetailsForm(this);
                Ifscdf.ShowDialog();
            }
        }
        */


        //txt
        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.Trim() != "")
                {
                    if (txtBarcode.Text.Length >= 23)
                    {
                        Barcode = txtBarcode.Text.Trim();
                        try
                        {
                            //split on a single character.
                            string[] separate = Barcode.Split('/');
                            string PosNo = separate[0].ToString();
                            string WipCode = separate[1].ToString();
                            double Qty = Convert.ToDouble(separate[2].ToString());
                            double BoxNo = Convert.ToDouble(separate[3].ToString());

                            int DupRow = 0;
                            foreach (DataGridViewRow dgvRow in dgvScannedTag.Rows)
                            {
                                if (dgvRow.Cells["POSNo"].Value.ToString() == PosNo && Convert.ToDouble(dgvRow.Cells["BoxNo"].Value.ToString()) == BoxNo)
                                {
                                    DupRow = DupRow + 1;
                                }
                            }
                            if (DupRow > 0)
                            {
                                MessageBox.Show("ឡាប៊ែលនេះស្កេនរួចហើយ​ ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                txtBarcode.SelectAll();
                                txtBarcode.Focus();
                            }
                            else
                            {
                                if (PosNo.Trim() != "" && WipCode.ToString().Trim() != "" && Qty.ToString().Trim() != "" && BoxNo.ToString().Trim() != "")
                                {
                                    cnn.con.Open();
                                    SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbWipTransactions WHERE PosNo ='" + PosNo + "' AND WipCode='"+WipCode+"' AND BoxNo='" + BoxNo + "' AND DeleteState='0';", cnn.con);
                                    DataTable dt = new DataTable();
                                    sda.Fill(dt);
                                    if (dt.Rows.Count > 0)
                                    {
                                        MessageBox.Show("ឡាប៊ែលនេះស្កេនរួចហើយ ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        txtBarcode.SelectAll();
                                        txtBarcode.Focus();
                                    }
                                    else
                                    {
                                        sda = new SqlDataAdapter("SELECT * FROM tbPOSDetailofMC WHERE PosCNo ='" + PosNo + "' AND WIPCode='" + WipCode + "';", cnn.con);
                                        dt = new DataTable();
                                        sda.Fill(dt);
                                        if (dt.Rows.Count > 0)
                                        {
                                            txtBarcode.Text = "";
                                            txtBarcode.Focus();
                                            InputTransferSemiConfirmForm itscf = new InputTransferSemiConfirmForm(this);
                                            itscf.ShowDialog();
                                        }
                                        else
                                        {
                                            MessageBox.Show("មិនមាន POS នេះនៅក្នុងប្រព័ន្ធទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            txtBarcode.SelectAll();
                                            txtBarcode.Focus();
                                        }
                                    }
                                    cnn.con.Close();
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
        private void InputTransferSemiForm_Load(object sender, EventArgs e)
        {
            ClearDtSaveError();            
        }

        //Button Click
        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearDtSaveError();
            this.dgvScannedTag.Rows.Clear();
            this.dgvPOSConsumption.Rows.Clear();
            txtBarcode.Focus();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvScannedTag.SelectedCells.Count > 0)
            {
                DialogResult DLR = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនដែរ ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    int selectedIndex = dgvScannedTag.CurrentCell.RowIndex;

                    ////Remove RM Consumpt first
                    //for (int i = dtRMConsumption.Rows.Count - 1; i > -1; i--)
                    //{
                    //    if (dtRMConsumption.Rows[i][0].ToString() == dgvScannedTag.Rows[selectedIndex].Cells[0].Value.ToString() && dtRMConsumption.Rows[i][1].ToString() == dgvScannedTag.Rows[selectedIndex].Cells[3].Value.ToString())
                    //    {
                    //        dtRMConsumption.Rows.RemoveAt(i);
                    //    }
                    //}
                    //dtRMConsumption.AcceptChanges();

                    if (selectedIndex > -1)
                    {
                        dgvScannedTag.Rows.RemoveAt(selectedIndex);
                        dgvScannedTag.Refresh();
                    }
                    dgvScannedTag.ClearSelection();
                    selectedIndex = -1;

                    //foreach (DataRow row in dtRMConsumption.Rows)
                    //{
                    //    string text = "";
                    //    foreach (DataColumn col in dtRMConsumption.Columns)
                    //    {
                    //        text =text + row[col].ToString()+"\t";
                    //    }
                    //    Console.WriteLine(text);
                    //}

                    this.dgvPOSConsumption.Rows.Clear();
                }
            }    
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvScannedTag.Rows.Count > 0)
            {
                DialogResult DRS = MessageBox.Show("តើអ្នកចង់រក្សាទុកទិន្នន័យទាំងនេះមែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DRS == DialogResult.Yes)
                {
                    ClearDtSaveError();
                    foreach (DataGridViewRow dgvRow in dgvScannedTag.Rows)
                    {
                        ErrorText = "";
                        string PosNo = dgvRow.Cells["POSNo"].Value.ToString();
                        double BoxNo = Convert.ToDouble(dgvRow.Cells["BoxNo"].Value.ToString());
                        double PosQty = Convert.ToDouble(dgvRow.Cells["Qty"].Value.ToString());

                        //Taking OBS 
                        DataTable dtPOSConsumpt = new DataTable();
                        try
                        {
                            cnnOBS.conOBS.Open();
                            string SQLQuery = "SELECT DONo, ConsumpSeqNo, T2.ItemCode, ItemName, MatTypeName, " +
                                "\nCASE " +
                                "\n\tWHEN MatTypeName='Wire' THEN ROUND(BOMQty*"+PosQty+", 3) " +
                                "\n\tWHEN MatTypeName='Connector' THEN ROUND(BOMQty*"+PosQty+",3) " +
                                "\n\tWHEN MatTypeName='Terminal' THEN ROUND(BOMQty*"+PosQty+",3) " +
                                "\n\tELSE ROUND(BOMQty*"+PosQty+",3) " +
                                "\nEND AS UsageQty FROM " +
                                "\n(SELECT * FROM prgproductionorder WHERE LineCode='MC1') T1 " +
                                "\nINNER JOIN (SELECT * FROM prgconsumtionorder) T2 ON T1.ProductionCode=T2.ProductionCode " +
                                "\nINNER JOIN (SELECT * FROM mstitem) T3 ON T2.ItemCode=T3.ItemCode " +
                                "\nINNER JOIN (SELECT * FROM MstMatType) T4 ON T3.MatTypeCode=T4.MatTypeCode " +
                                "\nWHERE DONo='"+PosNo+"' ";
                            //Console.WriteLine(SQLQuery);
                            SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                            sda.Fill(dtPOSConsumpt);

                        }
                        catch (Exception ex)
                        {
                            ErrorText = "OBS : " + ex.Message;
                        }
                        cnnOBS.conOBS.Close();

                        //Calculate Stock Remain 
                        if (ErrorText.Trim() == "")
                        {
                            if (dtPOSConsumpt.Rows.Count > 0)
                            {
                                ClearDtCuttingStock();
                                DataTable dtPOSStock = new DataTable();
                                DataTable dtOtherStock = new DataTable();
                                string RMCodeIN = "";
                                foreach (DataRow row in dtPOSConsumpt.Rows)
                                {
                                    if (RMCodeIN.Trim() == "")
                                    {
                                        RMCodeIN = "'" + row["ItemCode"].ToString() + "'";
                                    }
                                    else
                                    {
                                        RMCodeIN += ", '" + row["ItemCode"].ToString() + "'";
                                    }
                                }
                                //Taking Stock Data
                                try
                                {
                                    cnn.con.Open();
                                    string SQLQuery = "SELECT * FROM " +
                                        "\n(SELECT TbTst.Code, TbTst.POSNo AS LotNo, StockValue, " +
                                        "\nCASE " +
                                        "\n\tWHEN NOT TRIM(TbTst.POSNo) = '' THEN " +
                                        "\n\t\tCASE " +
                                        "\n\t\t\tWHEN LEFT(TbTst.POSNo,2) = 'SD' THEN TbAllo.POSNo " +
                                        "\n\t\t\tWHEN RIGHT(TbTst.POSNo,6) = 'NGRate' THEN LEFT(TbTst.POSNo,10) " +
                                        "\n\t\t\tELSE TbTst.POSNo " +
                                        "\n\t\tEND " +
                                        "\n\tELSE '' " +
                                        "\nEND AS POSNo, TbRegDate.RegDate FROM " +
                                        "\n(SELECT Code, POSNo, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='MC1' GROUP BY Code, POSNo) TbTst " +
                                        "\nLEFT JOIN (SELECT * FROM tbSDAllocateStock) TbAllo ON TbTst.POSNo=TbAllo.SysNo " +
                                        "\nINNER JOIN (SELECT Code, POSNo, MIN(RegDate) AS RegDate FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='MC1' AND Funct=2 AND NOT TRIM(POSNo) = '' GROUP BY Code, POSNo) TbRegDate ON TbTst.Code=TbRegDate.Code AND TbTst.POSNo=TbRegDate.POSNo) X " +
                                        "\nWHERE StockValue>0 AND POSNo='"+PosNo+"' " +
                                        "\nORDER BY Code ASC, RegDate ASC ";
                                    Console.WriteLine("\ndtPOSStock\n" + SQLQuery);
                                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                                    sda.Fill(dtPOSStock);

                                    SQLQuery = "SELECT Code, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction " +
                                                    "\nWHERE CancelStatus=0 AND LocCode='MC1' AND POSNo = '' AND Code IN (" + RMCodeIN + ") " +
                                                    "\nGROUP BY Code"; 
                                    //Console.WriteLine("\ndtOtherStock\n" + SQLQuery);
                                    sda = new SqlDataAdapter(SQLQuery, cnn.con);
                                    sda.Fill(dtOtherStock);

                                }
                                catch (Exception ex)
                                {
                                    ErrorText = "MC Stock : " + ex.Message;
                                }
                                cnn.con.Close();

                                if (ErrorText.Trim() == "")
                                {
                                    foreach (DataRow rowCons in dtPOSConsumpt.Rows)
                                    {
                                        string RMCode = rowCons["ItemCode"].ToString();
                                        string RMName = rowCons["ItemName"].ToString();
                                        double UsageQty = Convert.ToDouble(rowCons["UsageQty"].ToString());
                                        double StockOutRemain = UsageQty;

                                        //POS
                                        foreach (DataRow rowPOS in dtPOSStock.Rows)
                                        {
                                            if (StockOutRemain > 0)
                                            {
                                                if (RMCode == rowPOS["Code"].ToString() && PosNo == rowPOS["POSNo"].ToString() && Convert.ToDouble(rowPOS["StockValue"].ToString()) > 0)
                                                {
                                                    string LotOrPOSNo = rowPOS["LotNo"].ToString();
                                                    double StockRemain = Convert.ToDouble(rowPOS["StockValue"].ToString());
                                                    if (StockRemain >= StockOutRemain)
                                                    {
                                                        dtCuttingStock.Rows.Add();
                                                        dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["Code"] = RMCode;
                                                        dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["RMName"] = RMName;
                                                        dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["LotOrPOSNo"] = LotOrPOSNo;
                                                        dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["Qty"] = StockOutRemain;
                                                        StockOutRemain = 0;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        dtCuttingStock.Rows.Add();
                                                        dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["Code"] = RMCode;
                                                        dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["RMName"] = RMName;
                                                        dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["LotOrPOSNo"] = LotOrPOSNo;
                                                        dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["Qty"] = StockRemain;
                                                        StockOutRemain -= StockRemain;
                                                    }
                                                }
                                            }
                                            else 
                                            {
                                                break;
                                            }                                            
                                        }

                                        //Other
                                        foreach (DataRow rowOther in dtOtherStock.Rows)
                                        {
                                            if (StockOutRemain > 0)
                                            {
                                                if (RMCode == rowOther["Code"].ToString() && Convert.ToDouble(rowOther["StockValue"].ToString()) > 0)
                                                {
                                                    string LotOrPOSNo = "";
                                                    double StockRemain = Convert.ToDouble(rowOther["StockValue"].ToString());
                                                    if (StockRemain >= StockOutRemain)
                                                    {
                                                        dtCuttingStock.Rows.Add();
                                                        dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["Code"] = RMCode;
                                                        dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["RMName"] = RMName;
                                                        dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["LotOrPOSNo"] = LotOrPOSNo;
                                                        dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["Qty"] = StockOutRemain;
                                                        StockOutRemain = 0;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        dtCuttingStock.Rows.Add();
                                                        dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["Code"] = RMCode;
                                                        dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["RMName"] = RMName;
                                                        dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["LotOrPOSNo"] = LotOrPOSNo;
                                                        dtCuttingStock.Rows[dtCuttingStock.Rows.Count - 1]["Qty"] = StockRemain;
                                                        StockOutRemain -= StockRemain;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }

                                        if (StockOutRemain > 0)
                                        {
                                            ErrorText = "Stock not enough ( "+RMCode+" )";
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ErrorText = "OBS : No BOM!";
                            }
                        }

                        if (ErrorText.Trim() == "" && dtCuttingStock.Rows.Count > 0)
                        {
                            //Console Show dtCuttingStock
                            /*
                            foreach (DataRow row in dtCuttingStock.Rows)
                            {
                                string Text = "";
                                foreach (DataColumn col in dtCuttingStock.Columns)
                                {
                                    Text += row[col.ColumnName].ToString() + "\t";
                                }
                                Console.WriteLine(Text);
                            }
                            */

                            try
                            {
                                DateTime RegNow = DateTime.Now;
                                string TransNo = "";

                                //SysNo for transaction
                                cnn.con.Open();
                                SqlDataAdapter sda = new SqlDataAdapter("SELECT SysNo FROM tbWipTransactions WHERE SysNo=(SELECT max(CAST(SysNo AS INT)) FROM tbWipTransactions);", cnn.con);
                                DataTable dt = new DataTable();
                                sda.Fill(dt);
                                int nextLabelNo = 1;
                                if (dt.Rows.Count > 0)
                                {
                                    nextLabelNo = Convert.ToInt32(dt.Rows[0][0].ToString());
                                    nextLabelNo = nextLabelNo + 1;
                                }

                                //Add to transaction
                                cmd = new SqlCommand("INSERT INTO tbWipTransactions (PosNo, WipCode, WipDes, BoxNo, Qty, Remarks, RegDate, RegBy, UpdateDate, UpdateBy, SysNo, DeleteState) " +
                                                                                                            "VALUES (@Pn, @wc, @wd, @bn,@qty, @rm, @Rd, @Rb, @Ud, @Ub, @Sn, @DS)", cnn.con);
                                cmd.Parameters.AddWithValue("@Pn", PosNo);
                                cmd.Parameters.AddWithValue("@wc", dgvRow.Cells["WIPCode"].Value.ToString());
                                cmd.Parameters.AddWithValue("@wd", dgvRow.Cells["WIPName"].Value.ToString());
                                cmd.Parameters.AddWithValue("@bn", BoxNo);
                                cmd.Parameters.AddWithValue("@qty", PosQty);
                                cmd.Parameters.AddWithValue("@rm", dgvRow.Cells["Remarks"].Value.ToString());
                                cmd.Parameters.AddWithValue("@Rd", RegNow);
                                cmd.Parameters.AddWithValue("@Rb", MenuFormV2.UserForNextForm);
                                cmd.Parameters.AddWithValue("@Ud", RegNow);
                                cmd.Parameters.AddWithValue("@Ub", MenuFormV2.UserForNextForm);
                                cmd.Parameters.AddWithValue("@Sn", nextLabelNo.ToString());
                                cmd.Parameters.AddWithValue("@DS", "0");
                                cmd.ExecuteNonQuery();

                                //SysNo for tbSDMCAllTransaction
                                SqlDataAdapter da = new SqlDataAdapter("SELECT SysNo FROM tbSDMCAllTransaction " +
                                                                            "WHERE " +
                                                                            "RegDate=(SELECT MAX(RegDate) FROM tbSDMCAllTransaction WHERE Funct =5) AND " +
                                                                            "SysNo=(SELECT MAX(SysNo) FROM tbSDMCAllTransaction WHERE Funct =5) Group By SysNo", cnn.con);
                                DataTable dtTransNo = new DataTable();
                                da.Fill(dtTransNo);
                                if (dtTransNo.Rows.Count == 0)
                                {
                                    TransNo = "PRO0000000001";
                                }
                                else
                                {
                                    string LastTransNo = dtTransNo.Rows[0][0].ToString();
                                    double NextTransNo = Convert.ToDouble(LastTransNo.Substring(LastTransNo.Length - 10)) + 1;
                                    TransNo = "PRO" + NextTransNo.ToString("0000000000");
                                }

                                //Add to tbSDMCAllTransaction
                                foreach (DataRow row in dtCuttingStock.Rows)
                                {
                                    cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction (SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                                                                                                            "VALUES (@Sn, @Fc, @Lc, @POSN, @Cd, @Rmd, @Rqty, @Tqty, @SV, @Rd, @Rb, @Cs, @Rm)", cnn.con);
                                    cmd.Parameters.AddWithValue("@Sn", TransNo);
                                    cmd.Parameters.AddWithValue("@Fc", 5);
                                    cmd.Parameters.AddWithValue("@Lc", "MC1");
                                    cmd.Parameters.AddWithValue("@POSN", row["LotOrPOSNo"]);
                                    cmd.Parameters.AddWithValue("@Cd", row["Code"].ToString());
                                    cmd.Parameters.AddWithValue("@Rmd", row["RMName"].ToString());
                                    cmd.Parameters.AddWithValue("@Rqty", 0);
                                    cmd.Parameters.AddWithValue("@Tqty", Convert.ToDouble(row["Qty"].ToString()));
                                    cmd.Parameters.AddWithValue("@SV", Convert.ToDouble(row["Qty"].ToString()) * (-1));
                                    cmd.Parameters.AddWithValue("@Rd", RegNow);
                                    cmd.Parameters.AddWithValue("@Rb", MenuFormV2.UserForNextForm);
                                    cmd.Parameters.AddWithValue("@Cs", 0);
                                    cmd.Parameters.AddWithValue("@Rm", nextLabelNo.ToString());
                                    cmd.ExecuteNonQuery();

                                }

                                //Update Status of tbPOSDetailofMC
                                using (cmd = new SqlCommand("UpdatePOSDetailofMC_TransStatus", cnn.con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@NewResultPosC", PosQty);
                                    cmd.Parameters.AddWithValue("@PosC", PosNo);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                ErrorText = "POS Status, Cutting Stock : " + ex.Message;                                
                            }
                            cnn.con.Close();
                        }

                        //Add if Error
                        if (ErrorText.Trim() != "")
                        {
                            dtSaveError.Rows.Add();
                            dtSaveError.Rows[dtSaveError.Rows.Count - 1]["PosNo"] = PosNo;
                            dtSaveError.Rows[dtSaveError.Rows.Count - 1]["BoxNo"] = BoxNo;
                            dtSaveError.Rows[dtSaveError.Rows.Count - 1]["ErrorDetails"] = ErrorText;
                        }
                    }


                    if (dtSaveError.Rows.Count == 0)
                    {
                        MessageBox.Show("រក្សាទុកបានជោគជ័យ​ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvScannedTag.Rows.Clear();
                        dgvScannedTag.ClearSelection();
                        dgvScannedTag.CurrentCell = null;
                        btnNew.PerformClick();
                        txtBarcode.Focus();
                    }
                    else
                    {
                        MessageBox.Show("រក្សាទុកមានបញ្ហា​!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //Remove Success Row
                        for (int i = dgvScannedTag.Rows.Count - 1; i > -1; i--)
                        {
                            int Found = 0;
                            foreach (DataRow row in dtSaveError.Rows)
                            {
                                if (row["PosNo"].ToString() == dgvScannedTag.Rows[i].Cells["POSNo"].Value.ToString() && row["BoxNo"].ToString() == dgvScannedTag.Rows[i].Cells["BoxNo"].Value.ToString())
                                {
                                    Found++;
                                    break;
                                }
                            }
                            if(Found == 0)
                            {
                                dgvScannedTag.Rows.RemoveAt(i);
                                dgvScannedTag.Refresh();
                            }
                        }
                        dgvScannedTag.ClearSelection();
                        dgvScannedTag.CurrentCell = null;

                        //Report Error as txt file
                        string ErrorDetails = "";
                        foreach (DataColumn col in dtSaveError.Columns)
                        {
                            if (ErrorDetails.Trim() == "")
                            {
                                ErrorDetails = col.ColumnName.ToString();
                            }
                            else
                            {
                                if (col.ColumnName.ToString() == "BoxNo")
                                {
                                    ErrorDetails += "\t\t" + col.ColumnName.ToString();
                                }
                                else
                                {
                                    ErrorDetails += "\t" + col.ColumnName.ToString();
                                }
                            }
                        }
                        foreach (DataRow row in dtSaveError.Rows)
                        {
                            string Text = "";
                            foreach (DataColumn col in dtSaveError.Columns)
                            {
                                if (Text.Trim() == "")
                                {
                                    Text = row[col.ColumnName].ToString();
                                }
                                else
                                {
                                    Text += "\t" + row[col.ColumnName].ToString();
                                }
                            }
                            ErrorDetails += "\n" + Text;
                        }
                        ErrorReportAsTxt.PrintError = ErrorDetails;
                        this.OutputError.Output();
                    }
                }
            }
        }
        
        //Function
        private void ClearDtSaveError()
        {
            dtSaveError = new DataTable();
            dtSaveError.Columns.Add("PosNo");
            dtSaveError.Columns.Add("BoxNo");
            dtSaveError.Columns.Add("ErrorDetails");
        }
        private void ClearDtCuttingStock()
        {
            dtCuttingStock = new DataTable();
            dtCuttingStock.Columns.Add("Code");
            dtCuttingStock.Columns.Add("RMName");
            dtCuttingStock.Columns.Add("LotOrPOSNo");
            dtCuttingStock.Columns.Add("Qty");
        }

        private void AssignSeqNo()
        {
            foreach (DataGridViewRow dgvRow in dgvScannedTag.Rows)
            {
                dgvRow.HeaderCell.Value = (dgvRow.Index+1).ToString();
            }
            dgvScannedTag.Refresh();
        }
    }
}
