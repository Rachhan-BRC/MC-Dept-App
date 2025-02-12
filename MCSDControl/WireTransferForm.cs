using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    public partial class WireTransferForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SqlCommand cmd;
        DataTable dtLocation;
        DataTable dtLocStock;
        DataTable dtTransferSelected;

        string LocCode;
        string ErrorText;

        public WireTransferForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnOBS.Connection();
            this.Load += WireTransferForm_Load;

            //Cbo
            this.CboOutLocation.SelectedIndexChanged += CboOutLocation_SelectedIndexChanged;

            //Button
            this.btnShowDgvItems.Click += BtnShowDgvItems_Click;
            this.btnTakeIn.Click += BtnTakeIn_Click;
            this.btnTakeOut.Click += BtnTakeOut_Click;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnNew.Click += BtnNew_Click;
            this.btnSave.Click += BtnSave_Click;

            //txtSearchItem
            this.txtSearchItem.LostFocus += TxtSearchItem_LostFocus;

            //DgvSearchItem
            this.DgvSearchItem.CellClick += DgvSearchItem_CellClick;
            this.DgvSearchItem.LostFocus += DgvSearchItem_LostFocus;

            //DgvScanned
            this.dgvScanned.CellClick += DgvScanned_CellClick;
            this.dgvScanned.CurrentCellChanged += DgvScanned_CurrentCellChanged;

            //txtCode
            this.txtCode.Leave += TxtCode_Leave;
            this.txtCode.KeyDown += TxtCode_KeyDown;
            this.txtSearchItem.TextChanged += TxtSearchItem_TextChanged;
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;

        }

        //DgvScanned
        private void DgvScanned_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                CboOutLocation.Text = dgvScanned.Rows[e.RowIndex].Cells[2].Value.ToString();
                CboInLocation.Text = dgvScanned.Rows[e.RowIndex].Cells[3].Value.ToString(); 
                txtCode.Text = dgvScanned.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtItems.Text = dgvScanned.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtTransferQty.Text = dgvScanned.Rows[e.RowIndex].Cells[4].Value.ToString();
                if (dgvScanned.Rows[e.RowIndex].Cells["Remarks"].Value != null)
                {
                    txtRemark.Text = dgvScanned.Rows[e.RowIndex].Cells["Remarks"].Value.ToString();
                }
                ShowStockAvailableAndSelected();
                CheckBtnSaveAndDelete();
            }
        }
        private void DgvScanned_CurrentCellChanged(object sender, EventArgs e)
        {
            CheckBtnSaveAndDelete();
        }

        //Button
        private void BtnTakeOut_Click(object sender, EventArgs e)
        {
            if (txtTransferQty.Text.Trim() != "" && CboInLocation.Text.Trim()!="")
            {
                if (CboOutLocation.Text != CboInLocation.Text)
                {
                    if (Convert.ToDouble(txtTransferQty.Text) > 0 && dgvStockAvailable.SelectedCells.Count > 0)
                    {
                        string OutLoc = CboOutLocation.Text;
                        string InLoc = CboInLocation.Text;
                        double TransferQty = Convert.ToDouble(txtTransferQty.Text);
                        double SelectedQty = Convert.ToDouble(dgvStockAvailable.Rows[dgvStockAvailable.CurrentCell.RowIndex].Cells[1].Value.ToString());
                        //CheckDgv
                        double AlreadyTransfer = 0;
                        int Found = 0;
                        foreach (DataGridViewRow DgvRow in dgvScanned.Rows)
                        {
                            if (DgvRow.Cells[0].Value.ToString() == txtCode.Text && DgvRow.Cells[2].Value.ToString() == OutLoc && DgvRow.Cells[3].Value.ToString() == InLoc)
                            {
                                Found = Found + 1;
                                AlreadyTransfer = Convert.ToDouble(DgvRow.Cells[5].Value.ToString());
                                break;
                            }
                        }
                        if (Found == 0)
                        {
                            if (Convert.ToDouble(txtTransferQty.Text) >= SelectedQty)
                            {
                                dgvScanned.Rows.Add(txtCode.Text, txtItems.Text, CboOutLocation.Text, CboInLocation.Text, Convert.ToDouble(txtTransferQty.Text), SelectedQty, txtRemark.Text);
                                foreach (DataRow row in dtLocStock.Rows)
                                {
                                    if (row[0].ToString() == txtCode.Text && row[2].ToString() == dgvStockAvailable.Rows[dgvStockAvailable.CurrentCell.RowIndex].Cells[0].Value.ToString())
                                    {
                                        row[3] = 0;
                                        dtLocStock.AcceptChanges();
                                        break;
                                    }
                                }
                                dtTransferSelected.Rows.Add(txtCode.Text, CboOutLocation.Text, CboInLocation.Text, SelectedQty, dgvStockAvailable.Rows[dgvStockAvailable.CurrentCell.RowIndex].Cells[0].Value.ToString());
                                dtTransferSelected.AcceptChanges();
                                ShowStockAvailableAndSelected();
                            }
                            else
                            {
                                dgvScanned.Rows.Add(txtCode.Text, txtItems.Text, CboOutLocation.Text, CboInLocation.Text, Convert.ToDouble(txtTransferQty.Text), Convert.ToDouble(txtTransferQty.Text), txtRemark.Text);
                                foreach (DataRow row in dtLocStock.Rows)
                                {
                                    if (row[0].ToString() == txtCode.Text && row[2].ToString() == dgvStockAvailable.Rows[dgvStockAvailable.CurrentCell.RowIndex].Cells[0].Value.ToString())
                                    {
                                        row[3] = Convert.ToDouble(row[3].ToString())- Convert.ToDouble(txtTransferQty.Text);
                                        dtLocStock.AcceptChanges();
                                        break;
                                    }
                                }
                                dtTransferSelected.Rows.Add(txtCode.Text, CboOutLocation.Text, CboInLocation.Text, Convert.ToDouble(txtTransferQty.Text), dgvStockAvailable.Rows[dgvStockAvailable.CurrentCell.RowIndex].Cells[0].Value.ToString());
                                dtTransferSelected.AcceptChanges();
                                ShowStockAvailableAndSelected();
                            }
                        }
                        else
                        {
                            foreach (DataGridViewRow DgvRow in dgvScanned.Rows)
                            {
                                if (DgvRow.Cells[0].Value.ToString() == txtCode.Text && DgvRow.Cells[2].Value.ToString() == OutLoc && DgvRow.Cells[3].Value.ToString() == InLoc)
                                {
                                    DgvRow.Cells[4].Value = Convert.ToDouble(txtTransferQty.Text);
                                    if (Convert.ToDouble(DgvRow.Cells[4].Value.ToString()) > Convert.ToDouble(DgvRow.Cells[5].Value.ToString()))
                                    {
                                        if (TransferQty - AlreadyTransfer>=SelectedQty)
                                        {
                                            DgvRow.Cells[5].Value = AlreadyTransfer + SelectedQty;
                                            foreach (DataRow row in dtLocStock.Rows)
                                            {
                                                if (row[0].ToString() == txtCode.Text && row[2].ToString() == dgvStockAvailable.Rows[dgvStockAvailable.CurrentCell.RowIndex].Cells[0].Value.ToString())
                                                {
                                                    row[3] = 0;
                                                    dtLocStock.AcceptChanges();
                                                    break;
                                                }
                                            }

                                            int Found2 = 0;
                                            foreach (DataRow row in dtTransferSelected.Rows)
                                            {
                                                if (txtCode.Text == row[0].ToString() && CboOutLocation.Text == row[1].ToString() && CboInLocation.Text == row[2].ToString() && dgvStockAvailable.Rows[dgvStockAvailable.CurrentCell.RowIndex].Cells[0].Value.ToString() == row[4].ToString())
                                                {
                                                    Found2 = Found2 + 1;
                                                    row[3] = Convert.ToDouble(row[3].ToString()) + SelectedQty;
                                                }
                                            }
                                            if (Found2 == 0)
                                            {
                                                dtTransferSelected.Rows.Add(txtCode.Text, CboOutLocation.Text, CboInLocation.Text, SelectedQty, dgvStockAvailable.Rows[dgvStockAvailable.CurrentCell.RowIndex].Cells[0].Value.ToString());
                                            }
                                            dtTransferSelected.AcceptChanges();
                                            ShowStockAvailableAndSelected();
                                        }
                                        else
                                        {
                                            DgvRow.Cells[5].Value = AlreadyTransfer + (TransferQty - AlreadyTransfer);
                                            foreach (DataRow row in dtLocStock.Rows)
                                            {
                                                if (row[0].ToString() == txtCode.Text && row[2].ToString() == dgvStockAvailable.Rows[dgvStockAvailable.CurrentCell.RowIndex].Cells[0].Value.ToString())
                                                {
                                                    row[3] = Convert.ToDouble(row[3].ToString()) - (TransferQty - AlreadyTransfer);
                                                    dtLocStock.AcceptChanges();
                                                    break;
                                                }
                                            }
                                            int Found2 = 0;
                                            foreach (DataRow row in dtTransferSelected.Rows)
                                            {
                                                if (txtCode.Text == row[0].ToString() && CboOutLocation.Text == row[1].ToString() && CboInLocation.Text == row[2].ToString() && dgvStockAvailable.Rows[dgvStockAvailable.CurrentCell.RowIndex].Cells[0].Value.ToString() == row[4].ToString())
                                                {
                                                    Found2 = Found2 + 1;
                                                    row[3] = Convert.ToDouble(row[3].ToString()) + (TransferQty - AlreadyTransfer);
                                                }
                                            }
                                            if (Found2 == 0)
                                            {
                                                dtTransferSelected.Rows.Add(txtCode.Text, CboOutLocation.Text, CboInLocation.Text, TransferQty - AlreadyTransfer, dgvStockAvailable.Rows[dgvStockAvailable.CurrentCell.RowIndex].Cells[0].Value.ToString());
                                            }
                                            dtTransferSelected.AcceptChanges();

                                            ShowStockAvailableAndSelected();
                                        }
                                    }
                                }
                            }
                        }
                        dgvScanned.ClearSelection();

                        ShowStockAvailableAndSelected();

                        //Console.WriteLine("WTF:");
                        //foreach (DataRow row in dtTransferSelected.Rows)
                        //{
                        //    foreach (DataColumn col in dtTransferSelected.Columns)
                        //    {
                        //        Console.Write(row[col].ToString());
                        //        Console.Write("\t");
                        //    }
                        //    Console.Write("\n");
                        //}
                    }
                }
            }
            CheckBtnSaveAndDelete();
        }
        private void BtnTakeIn_Click(object sender, EventArgs e)
        {
            if (dgvStockSelected.SelectedCells.Count > 0)
            {
                foreach (DataRow row in dtLocStock.Rows)
                {
                    if (txtCode.Text == row[0].ToString() && row[2].ToString()== dgvStockSelected.Rows[dgvStockSelected.CurrentCell.RowIndex].Cells[0].Value.ToString())
                    {
                        row[3] = Convert.ToDouble(row[3].ToString())+Convert.ToDouble(dgvStockSelected.Rows[dgvStockSelected.CurrentCell.RowIndex].Cells[1].Value.ToString());
                        dtLocStock.AcceptChanges();
                        foreach (DataGridViewRow DgvRow in dgvScanned.Rows)
                        {
                            if (DgvRow.Cells[0].Value.ToString() == txtCode.Text && DgvRow.Cells[2].Value.ToString() == CboOutLocation.Text &&
                                DgvRow.Cells[3].Value.ToString() == CboInLocation.Text)
                            {
                                DgvRow.Cells[5].Value = Convert.ToDouble(DgvRow.Cells[5].Value.ToString()) - Convert.ToDouble(dgvStockSelected.Rows[dgvStockSelected.CurrentCell.RowIndex].Cells[1].Value.ToString());
                            }
                        }
                        break;
                    }
                }
                foreach (DataRow row in dtTransferSelected.Rows)
                {
                    if (row[0].ToString() == txtCode.Text && row[1].ToString() == CboOutLocation.Text && row[2].ToString() == CboInLocation.Text && row[4].ToString() == dgvStockSelected.Rows[dgvStockSelected.CurrentCell.RowIndex].Cells[0].Value.ToString())
                    {
                        dtTransferSelected.Rows.Remove(row);
                        dtTransferSelected.AcceptChanges();
                        break;
                    }
                }
                ShowStockAvailableAndSelected();

                //foreach (DataRow row in dtTransferSelected.Rows)
                //{
                //    foreach (DataColumn col in dtTransferSelected.Columns)
                //    {
                //        Console.Write(row[col].ToString());
                //        Console.Write("\t");
                //    }
                //    Console.Write("\n");
                //}
            }
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            //Return to LocStock
            foreach (DataRow row in dtTransferSelected.Rows)
            {
                foreach (DataRow rowLocStock in dtLocStock.Rows)
                {
                    if (rowLocStock[0].ToString() == row[0].ToString() && rowLocStock[2].ToString() == row[4].ToString())
                    {
                        rowLocStock[3] = Convert.ToDouble(rowLocStock[3].ToString()) + Convert.ToDouble(row[3].ToString());
                        dtLocStock.AcceptChanges();
                    }
                }
            }
            //Clear TransferSelected
            CleardtTransferSelected();
            dgvScanned.Rows.Clear();
            ClearText();
            ShowStockAvailableAndSelected();
            CboOutLocation.SelectedIndex = 0;
            CboInLocation.SelectedIndex = 0;
            Console.WriteLine("WTF:");
            foreach (DataRow row in dtTransferSelected.Rows)
            {
                foreach (DataColumn col in dtTransferSelected.Columns)
                {
                    Console.Write(row[col].ToString());
                    Console.Write("\t");
                }
                Console.Write("\n");
            }
            CheckBtnSaveAndDelete();
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvScanned.SelectedCells.Count > 0)
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    //Return to LocStock
                    foreach (DataRow row in dtTransferSelected.Rows)
                    {
                        if (row[0].ToString() == dgvScanned.Rows[dgvScanned.CurrentCell.RowIndex].Cells[0].Value.ToString() && row[1].ToString() == dgvScanned.Rows[dgvScanned.CurrentCell.RowIndex].Cells[2].Value.ToString() && row[2].ToString() == dgvScanned.Rows[dgvScanned.CurrentCell.RowIndex].Cells[3].Value.ToString())
                        {
                            foreach (DataRow rowLocStock in dtLocStock.Rows)
                            {
                                if (rowLocStock[0].ToString() == row[0].ToString() && rowLocStock[2].ToString() == row[4].ToString())
                                {
                                    rowLocStock[3] = Convert.ToDouble(rowLocStock[3].ToString()) + Convert.ToDouble(row[3].ToString());
                                    dtLocStock.AcceptChanges();
                                }
                            }
                        }
                    }
                    //Remove TransferSelected
                    for (int i = dtTransferSelected.Rows.Count - 1; i > -1; i--)
                    {
                        if (dtTransferSelected.Rows[i][0].ToString() == dgvScanned.Rows[dgvScanned.CurrentCell.RowIndex].Cells[0].Value.ToString() && dtTransferSelected.Rows[i][1].ToString() == dgvScanned.Rows[dgvScanned.CurrentCell.RowIndex].Cells[2].Value.ToString() && dtTransferSelected.Rows[i][2].ToString() == dgvScanned.Rows[dgvScanned.CurrentCell.RowIndex].Cells[3].Value.ToString())
                        {
                            dtTransferSelected.Rows.RemoveAt(i);
                        }
                    }
                    dtTransferSelected.AcceptChanges();

                    dgvScanned.Rows.RemoveAt(dgvScanned.CurrentCell.RowIndex);
                    dgvScanned.Update();
                    dgvScanned.ClearSelection();

                    /*
                     
                    Console.WriteLine("WTF:");
                    foreach (DataRow row in dtTransferSelected.Rows)
                    {
                        foreach (DataColumn col in dtTransferSelected.Columns)
                        {
                            Console.Write(row[col].ToString());
                            Console.Write("\t");
                        }
                        Console.Write("\n");
                    }
                    
                    */

                    ClearText();
                    CheckBtnSaveAndDelete();
                }
            }
        }
        private void BtnShowDgvItems_Click(object sender, EventArgs e)
        {
            if (dtLocStock != null && dtLocStock.Rows.Count > 0)
            {
                panelSearchItem.BringToFront();
                DgvSearchItem.Rows.Clear();
                int PanelItemY = panelChooseItemAndStock.Location.Y + txtItems.Location.Y - txtItems.Height - 10;
                if (dtLocStock.Rows.Count > 0)
                {
                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("Code");
                    dtItem.Columns.Add("Item");
                    foreach (DataRow row in dtLocStock.Rows)
                    {
                        if (Convert.ToDouble(row[3].ToString()) > 0)
                        {
                            int Found = 0;
                            foreach (DataRow rowCheck in dtItem.Rows)
                            {
                                if (row[0].ToString() == rowCheck[0].ToString())
                                {
                                    Found = Found + 1;
                                    break;
                                }
                            }
                            if (Found == 0)
                            {
                                dtItem.Rows.Add(row[0].ToString(), row[1].ToString());
                            }
                        }
                    }
                    if (dtItem.Rows.Count > 8)
                    {
                        txtSearchItem.Visible = true;
                        panelSearchItem.Size = new Size(253, 247);
                        DgvSearchItem.Size = new Size(253, 226);
                        DgvSearchItem.Columns[1].Width = 200 - 16;
                        panelSearchItem.Location = new Point(82, PanelItemY - panelSearchItem.Height);
                    }
                    else
                    {
                        int HeightofDgv = dtItem.Rows.Count * 26;
                        txtSearchItem.Visible = false;
                        panelSearchItem.Size = new Size(253, HeightofDgv);
                        DgvSearchItem.Columns[1].Width = 202;
                        DgvSearchItem.Size = new Size(253, HeightofDgv);
                        panelSearchItem.Location = new Point(82, PanelItemY - panelSearchItem.Height);
                    }
                    foreach (DataRow row in dtItem.Rows)
                    {
                        DgvSearchItem.Rows.Add(row[0].ToString(), row[1].ToString());
                    }
                }
                DgvSearchItem.ClearSelection();
                DgvSearchItem.Focus();
            }
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            int FoundZero = 0;
            foreach (DataGridViewRow DgvRow in dgvScanned.Rows)
            {
                if (Convert.ToDouble(DgvRow.Cells[5].Value.ToString())<=0)
                {
                    FoundZero = FoundZero + 1;
                }
            }
            if (FoundZero == 0)
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់រក្សាទុកទិន្នន័យនេះមែនឬទេ?","Rachhan System",MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    SaveToDB();
                }
            }
            else
            {
                DialogResult DSL = MessageBox.Show("មានទិន្នន័យខ្លះ ចំនួនត្រូវវេរ=0 !\nតើអ្នកចង់បន្តរក្សាទុកទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (DSL == DialogResult.Yes)
                {
                    SaveToDB();
                }
            }
        }

        //txtCode
        private void TxtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtCode.Text.Trim() != "")
                {
                    TakeItemName();
                }
            }
        }
        private void TxtCode_Leave(object sender, EventArgs e)
        {
            if (txtCode.Text.Trim() != "")
            {
                TakeItemName();
            }
        }
        private void TxtSearchItem_TextChanged(object sender, EventArgs e)
        {
            DgvSearchItem.Rows.Clear();
            if (txtSearchItem.Text.Trim() == "")
            {
                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("Code");
                dtItem.Columns.Add("Item");
                foreach (DataRow row in dtLocStock.Rows)
                {
                    if (Convert.ToDouble(row[3].ToString()) > 0)
                    {
                        int Found = 0;
                        foreach (DataRow rowCheck in dtItem.Rows)
                        {
                            if (row[0].ToString() == rowCheck[0].ToString())
                            {
                                Found = Found + 1;
                                break;
                            }
                        }
                        if (Found == 0)
                        {
                            dtItem.Rows.Add(row[0].ToString(), row[1].ToString());
                        }
                    }
                }
                foreach (DataRow row in dtItem.Rows)
                {
                    DgvSearchItem.Rows.Add(row[0].ToString(), row[1].ToString());
                }
            }
            else
            {
                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("Code");
                dtItem.Columns.Add("Item");
                foreach (DataRow row in dtLocStock.Rows)
                {
                    if (Convert.ToDouble(row[3].ToString()) > 0)
                    {
                        if (row[0].ToString().ToLower().Contains(txtSearchItem.Text.ToString().ToLower()) == true || row[1].ToString().ToLower().Contains(txtSearchItem.Text.ToString().ToLower()) == true)
                        {
                            int Found = 0;
                            foreach (DataRow rowCheck in dtItem.Rows)
                            {
                                if (row[0].ToString() == rowCheck[0].ToString())
                                {
                                    Found = Found + 1;
                                    break;
                                }
                            }
                            if (Found == 0)
                            {
                                dtItem.Rows.Add(row[0].ToString(), row[1].ToString());
                            }
                        }
                    }
                }
                foreach (DataRow row in dtItem.Rows)
                {
                    DgvSearchItem.Rows.Add(row[0].ToString(), row[1].ToString());
                }

            }
            if (DgvSearchItem.Rows.Count > 8)
            {
                DgvSearchItem.Columns[1].Width = 200 - 16;
            }
            else
            {
                DgvSearchItem.Columns[1].Width = 202;
            }
        }
        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.Trim() != "")
                {
                    if (txtBarcode.Text.ToString().ToUpper().Contains("NG") == true)
                    {
                        RetrivePOSConsumpNGRate();
                    }
                    else
                    {
                        //RetrivePOSConsump();
                    }
                }
            }
        }

        //DgvSearchItem
        private void DgvSearchItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCode.Text = DgvSearchItem.Rows[e.RowIndex].Cells[0].Value.ToString();
            TakeItemName();
        }
        private void DgvSearchItem_LostFocus(object sender, EventArgs e)
        {
            HidePanelItems();
        }

        //txtSearchItem
        private void TxtSearchItem_LostFocus(object sender, EventArgs e)
        {
            HidePanelItems();
        }
       
        //Cbo
        private void CboOutLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CboOutLocation.Text.Trim() != "")
            {
                Cursor = Cursors.WaitCursor;
                ClearText();
                LocCode = " ";
                foreach (DataRow row in dtLocation.Rows)
                {
                    if (row[1].ToString() == CboOutLocation.Text)
                    {
                        LocCode = row[0].ToString();
                    }
                }
                RetriveLocStock(); 
                Cursor = Cursors.Default;
            }
        }

        private void WireTransferForm_Load(object sender, EventArgs e)
        {
            dtLocStock = new DataTable();
            dtLocation = new DataTable();
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbSDMCLocation", cnn.con);
                sda.Fill(dtLocation);
                foreach (DataRow row in dtLocation.Rows)
                {
                    CboInLocation.Items.Add(row[1].ToString());
                    CboOutLocation.Items.Add(row[1].ToString());
                }
                CleardtTransferSelected();
            }
            catch(Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n"+ex.Message,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            cnn.con.Close();
            
        }

        //Method
        private void RetriveLocStock()
        {            
            DgvSearchItem.Rows.Clear();
            dtLocStock=new DataTable();
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT Code, ItemName, POSNo, StockValue FROM " +
                    "(SELECT Code, POSNo, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction " +
                    "WHERE CancelStatus=0 AND LocCode='"+LocCode+"' " +
                    "GROUP BY Code, POSNo) T1 " +
                    "FULL OUTER JOIN " +
                    "(SELECT ItemCode, ItemName FROM tbMasterItem WHERE Remarks1 IS NULL) T2 " +
                    "ON T1.Code = T2.ItemCode " +
                    "WHERE NOT StockValue IS NULL AND StockValue>0 ORDER BY Code ASC, POSNo ASC", cnn.con);
                sda.Fill(dtLocStock);
            }
            catch( Exception ex )
            {
                MessageBox.Show("មានបញ្ហា!\n"+ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
            
            if (dtTransferSelected.Rows.Count > 0)
            {
                foreach (DataRow row in dtLocStock.Rows)
                {
                    foreach (DataRow rowTransferSelected in dtTransferSelected.Rows)
                    {
                        if (row[0].ToString() == rowTransferSelected[0].ToString() && CboOutLocation.Text == rowTransferSelected[1].ToString() && row[2].ToString() == rowTransferSelected[4].ToString())
                        {
                            row[3] = Convert.ToDouble(row[3].ToString())-Convert.ToDouble(rowTransferSelected[3].ToString());
                            break;
                        }
                    }
                    dtLocStock.AcceptChanges();
                }
            }

        }
        private void HidePanelItems()
        {
            if (DgvSearchItem.Focused == false && txtSearchItem.Focused == false)
            {
                panelSearchItem.SendToBack();
            }
        }
        private void TakeItemName()
        {
            txtItems.Text = "";
            txtTransferQty.Text = "";
            txtRemark.Text = "";
            //Take ItemName
            foreach (DataRow row in dtLocStock.Rows)
            {
                if(txtCode.Text == row[0].ToString())
                {
                    txtItems.Text= row[1].ToString();
                    break;
                }
            }
            //TakeStockRemain
            ShowStockAvailableAndSelected();
            txtTransferQty.Focus();
        }
        private void ClearText()
        {
            txtCode.Text = "";
            txtItems.Text = "";
            txtTransferQty.Text = "";
            txtRemark.Text = "";
            dgvStockAvailable.Rows.Clear();
            dgvStockSelected.Rows.Clear();
        }
        private void CleardtTransferSelected()
        {
            dtTransferSelected = new DataTable();
            dtTransferSelected.Columns.Add("Code");
            dtTransferSelected.Columns.Add("OutLoc");
            dtTransferSelected.Columns.Add("InLoc");
            dtTransferSelected.Columns.Add("TransferQty");
            dtTransferSelected.Columns.Add("Remark");
        }
        private void ShowStockAvailableAndSelected()
        {
            dgvStockAvailable.Rows.Clear();
            dgvStockSelected.Rows.Clear();

            foreach (DataRow row in dtLocStock.Rows)
            {
                if (txtCode.Text == row["Code"].ToString() && Convert.ToDouble(row["StockValue"].ToString()) > 0)
                {
                    int OK = 0;
                    if (txtRemark.Text.ToString().Contains("_NGRate") == true)
                    {
                        if (row["POSNo"].ToString().Trim() == "")
                        {
                            OK++;
                        }
                    }
                    else
                    {
                        OK++;
                    }

                    if (OK > 0)
                    {
                        dgvStockAvailable.Rows.Add();
                        dgvStockAvailable.Rows[dgvStockAvailable.Rows.Count - 1].Cells["POSNoAvailable"].Value = row["POSNo"].ToString();
                        dgvStockAvailable.Rows[dgvStockAvailable.Rows.Count - 1].Cells["QtyAvailable"].Value = Convert.ToDouble(row["StockValue"].ToString());
                    }
                }
            }

            //Console dtLocStock
            /*
            string ConsoleText = "";
            foreach (DataColumn col in dtLocStock.Columns)
            {
                ConsoleText += col.ColumnName + "\t";
            }
            Console.WriteLine(ConsoleText);
            foreach (DataRow row in dtLocStock.Rows)
            {
                ConsoleText = "";
                foreach (DataColumn col in dtLocStock.Columns)
                {
                    ConsoleText += row[col.ColumnName].ToString() + "\t";
                }
                Console.WriteLine(ConsoleText);
            }
            */

            foreach (DataRow row in dtTransferSelected.Rows)
            {
                if (txtCode.Text == row[0].ToString() && CboOutLocation.Text == row[1].ToString() && CboInLocation.Text == row[2].ToString() && Convert.ToDouble(row[3].ToString()) > 0)
                {
                    dgvStockSelected.Rows.Add(row[4].ToString(), Convert.ToDouble(row[3].ToString()));
                }
            }
        }
        private void CheckBtnSaveAndDelete()
        {
            if (dgvScanned.SelectedCells.Count > 0)
            {
                btnDelete.Enabled = true;
                btnDeleteGRAY.SendToBack();
            }
            else
            {
                btnDelete.Enabled = false;
                btnDeleteGRAY.BringToFront();
            }

            if (dgvScanned.Rows.Count > 0)
            {
                btnSave.Enabled = true;
                btnSaveGRAY.SendToBack();
            }
            else
            {
                btnSave.Enabled = false;
                btnSaveGRAY.BringToFront();
            }
        }
        private void RetrivePOSConsump()
        {
            DataTable dt = new DataTable();
            try
            {
                cnnOBS.conOBS.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT DONo, ConsumpSeqNo, T2.ItemCode, T3.ItemName, ConsumpQty, PackSize, UnitCode FROM " +
                                            "(SELECT * FROM prgproductionorder WHERE LineCode='MC1' AND NOT PlanStatus=2) T1 " +
                                            "INNER JOIN " +
                                            "(SELECT * FROM prgconsumtionorder) T2 " +
                                            "ON T1.ProductionCode = T2.ProductionCode " +
                                            "INNER JOIN " +
                                            "(SELECT * FROM mstitem WHERE DelFlag=0 AND ItemType=2 AND MatCalcFlag=1) T3 " +
                                            "ON T2.ItemCode = T3.ItemCode " +
                                            "WHERE DONo = '"+txtBarcode.Text+"' " +
                                            "ORDER BY DONo ASC, ConsumpSeqNo ASC", cnnOBS.conOBS);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnnOBS.conOBS.Close();

            try
            {
                cnn.con.Open();
                if (dt.Rows.Count > 0)
                {
                    dgvScanned.Rows.Clear();
                    CleardtTransferSelected();
                    ClearText();
                    CboOutLocation.SelectedIndex = 0;
                    CboInLocation.SelectedIndex = 0;

                    /*
                    Console.WriteLine("WTF:");
                    foreach (DataRow row in dtTransferSelected.Rows)
                    {
                        foreach (DataColumn col in dtTransferSelected.Columns)
                        {
                            Console.Write(row[col].ToString());
                            Console.Write("\t");
                        }
                        Console.Write("\n");
                    }
                    */

                    SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM " +
                        "(SELECT Code, POSNo, SUM(ReceiveQty) AS TransferQty FROM tbSDMCAllTransaction " +
                        "WHERE CancelStatus = 0 AND LocCode='MC1' " +
                        "GROUP BY Code, POSNo) T1 " +
                        "WHERE POSNo='"+txtBarcode.Text+"' ", cnn.con);
                    DataTable dtMC1 = new DataTable();
                    sda.Fill(dtMC1 );

                    string OutLoc = "WIR1";
                    string InLoc = "MC1";
                    foreach (DataRow row in dtLocation.Rows)
                    {
                        if (row[0].ToString() == OutLoc)
                        {
                            OutLoc = row[1].ToString();
                        }
                        if (row[0].ToString() == InLoc)
                        {
                            InLoc = row[1].ToString();
                        }
                    }

                    ShowStockAvailableAndSelected();

                    foreach (DataRow row in dt.Rows)
                    {
                        double Receive = 0;
                        foreach (DataRow rowMC1 in dtMC1.Rows)
                        {
                            if (row[2].ToString() == rowMC1[0].ToString() && row[0].ToString() == rowMC1[1].ToString())
                            {
                                Receive = Convert.ToDouble(rowMC1[2].ToString());
                            }
                        }

                        if (Receive < Convert.ToDouble(row[4].ToString()))
                        {
                            double TotalByPackSize = (Convert.ToDouble(row[4].ToString()) - Receive) / Convert.ToDouble(row[5].ToString());
                            TotalByPackSize = Math.Ceiling(TotalByPackSize);
                            TotalByPackSize = TotalByPackSize * Convert.ToDouble(row[5].ToString());
                            dgvScanned.Rows.Add(row[2], row[3], OutLoc, InLoc, TotalByPackSize, 0, row[0]);
                        }
                    }
                    dgvScanned.ClearSelection();
                    if (dgvScanned.Rows.Count > 0)
                    {
                        txtBarcode.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("ភីអូអេសនេះវេរគ្រប់ហើយ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtBarcode.Focus();
                        txtBarcode.SelectAll();
                    }
                }
                else
                {
                    MessageBox.Show("គ្មានទិន្នន័យភីអូអេសនេះទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtBarcode.Focus();
                    txtBarcode.SelectAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
        }
        private void RetrivePOSConsumpNGRate()
        {
            ErrorText = "";
            dgvScanned.Rows.Clear();
            Cursor = Cursors.WaitCursor;

            //Taking Consumption data
            DataTable dt = new DataTable();            
            try
            {
                cnnOBS.conOBS.Open();
                string[] BC = txtBarcode.Text.Split('.');
                SqlDataAdapter sda = new SqlDataAdapter("SELECT DONo, ConsumpSeqNo, T2.ItemCode, T3.ItemName, ConsumpQty, PackSize, UnitCode FROM " +
                                            "(SELECT * FROM prgproductionorder WHERE LineCode='MC1' AND NOT PlanStatus=2) T1 " +
                                            "INNER JOIN " +
                                            "(SELECT * FROM prgconsumtionorder) T2 " +
                                            "ON T1.ProductionCode = T2.ProductionCode " +
                                            "INNER JOIN " +
                                            "(SELECT * FROM mstitem WHERE DelFlag=0 AND ItemType=2 AND MatCalcFlag=0) T3 " +
                                            "ON T2.ItemCode = T3.ItemCode " +
                                            "WHERE DONo = '" + BC[0].ToString() + "' " +
                                            "ORDER BY DONo ASC, ConsumpSeqNo ASC", cnnOBS.conOBS);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            cnnOBS.conOBS.Close();

            //Taking Stock 
            if (ErrorText.Trim() == "" && dt.Rows.Count > 0)
            {
                LocCode = "";
                foreach (DataRow rowLoc in dtLocation.Rows)
                {
                    if (rowLoc["LocCode"].ToString() == "KIT3")
                    {
                        LocCode = rowLoc["LocName"].ToString();
                        break;
                    }
                }
                RetriveLocStock();
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                if(dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        dgvScanned.Rows.Add();
                        dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["RMCode"].Value = row["ItemCode"].ToString();
                        dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["RMName"].Value = row["ItemName"].ToString();
                        string LocOUTName = "";
                        string LocINName = "";
                        foreach (DataRow rowLoc in dtLocation.Rows)
                        {
                            if (rowLoc["LocCode"].ToString() == "KIT3")
                            {
                                LocOUTName = rowLoc["LocName"].ToString();
                            }
                            if (rowLoc["LocCode"].ToString() == "MC1")
                            {
                                LocINName = rowLoc["LocName"].ToString();
                            }
                        }
                        dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["LocOUT"].Value = LocOUTName;
                        dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["LocIN"].Value = LocINName;
                        dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["TransferQty"].Value = 0;
                        dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["ActualTransferQty"].Value = 0;
                        dgvScanned.Rows[dgvScanned.Rows.Count - 1].Cells["Remarks"].Value = row["DONo"].ToString()+"_NGRate";                        
                    }
                    dgvScanned.ClearSelection();
                    txtBarcode.Text = "";
                    CheckBtnSaveAndDelete();

                }
                else
                {
                    MessageBox.Show("គ្មានទិន្នន័យភីអូអេសនេះទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtBarcode.Focus();
                    txtBarcode.SelectAll();
                }
            }
            else
            {
                MessageBox.Show("មានបញ្ហា!\n"+ErrorText,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void SaveToDB()
        {
            //try
            //{
            //Dt for error
            DataTable dtError = new DataTable();
                dtError.Columns.Add("Code");
                dtError.Columns.Add("Items");
                dtError.Columns.Add("OutLoc");
                dtError.Columns.Add("Remark");
                dtError.Columns.Add("Qty");

                //Take SysNo
                string User = MenuFormV2.UserForNextForm;
                string TransNo = "";
                cnn.con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT SysNo FROM tbSDMCAllTransaction " +
                                                                                "WHERE SysNo=(SELECT MAX(SysNo) FROM tbSDMCAllTransaction WHERE Funct =2) Group By SysNo", cnn.con);
                DataTable dtTransNo = new DataTable();
                da.Fill(dtTransNo);
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

                DateTime RegDate = DateTime.Now;
                foreach (DataGridViewRow DgvRow in dgvScanned.Rows)
                {
                    if (Convert.ToDouble(DgvRow.Cells[5].Value.ToString()) > 0)
                    {
                        string Code = DgvRow.Cells[0].Value.ToString();
                        string Items = DgvRow.Cells[1].Value.ToString();
                        string OutLocName = DgvRow.Cells[2].Value.ToString();
                        string OutLocCode = "";
                        string InLocCode = "";
                        string Remark = DgvRow.Cells[6].Value.ToString();
                        foreach (DataRow row in dtLocation.Rows)
                        {
                            if (OutLocName == row[1].ToString())
                            {
                                OutLocCode = row[0].ToString();
                            }
                            if (DgvRow.Cells[3].Value.ToString() == row[1].ToString())
                            {
                                InLocCode = row[0].ToString();
                            }
                        }

                        //CheckRemaining Stock
                        foreach (DataRow row in dtTransferSelected.Rows)
                        {
                            if (Code == row[0].ToString() && OutLocName == row[1].ToString())
                            {
                                string POSNo = row[4].ToString();
                                double TransferQty = Convert.ToDouble(row[3].ToString());
                                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM " +
                                    "(SELECT Code, LocCode, POSNo, SUM(StockValue) AS StockValue FROM tbSDMCAllTransaction " +
                                    "WHERE CancelStatus=0 GROUP BY Code, LocCode, POSNo) T1 " +
                                    "WHERE LocCode='" + OutLocCode + "' AND Code='" + Code + "' AND POSNo='" + POSNo + "' ", cnn.con);
                                DataTable dtCheck = new DataTable();
                                sda.Fill(dtCheck);

                                if (Convert.ToDouble(dtCheck.Rows[0][3].ToString()) >= TransferQty)
                                {
                                    //Save To DB Stock-Out
                                    cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                                                   "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs, @Rem)", cnn.con);
                                    cmd.Parameters.AddWithValue("@Sn", TransNo);
                                    cmd.Parameters.AddWithValue("@Ft", 2);
                                    cmd.Parameters.AddWithValue("@Lc", OutLocCode);
                                    cmd.Parameters.AddWithValue("@Pn", POSNo);
                                    cmd.Parameters.AddWithValue("@Cd", Code);
                                    cmd.Parameters.AddWithValue("@Rmd", Items);
                                    cmd.Parameters.AddWithValue("@Rq", 0);
                                    cmd.Parameters.AddWithValue("@Tq", TransferQty);
                                    cmd.Parameters.AddWithValue("@Sv", (TransferQty * (-1)));
                                    cmd.Parameters.AddWithValue("@Rd", RegDate.ToString("yyyy-MM-dd HH:mm:ss"));
                                    cmd.Parameters.AddWithValue("@Rb", User);
                                    cmd.Parameters.AddWithValue("@Cs", 0);
                                    cmd.Parameters.AddWithValue("@Rem", Remark);
                                    cmd.ExecuteNonQuery();

                                    //Save To DB Stock-IN
                                    cmd = new SqlCommand("INSERT INTO tbSDMCAllTransaction ( SysNo, Funct, LocCode, POSNo, Code, RMDes, ReceiveQty, TransferQty, StockValue, RegDate, RegBy, CancelStatus, Remarks) " +
                                                                   "VALUES ( @Sn, @Ft, @Lc, @Pn, @Cd, @Rmd, @Rq, @Tq, @Sv, @Rd, @Rb, @Cs, @Rem)", cnn.con);
                                    cmd.Parameters.AddWithValue("@Sn", TransNo);
                                    cmd.Parameters.AddWithValue("@Ft", 2);
                                    cmd.Parameters.AddWithValue("@Lc", InLocCode);
                                    //For SD Additional Transfer Stock
                                    if (OutLocCode == "WIR1" && InLocCode == "MC1")
                                    {
                                        if (Remark.Trim()!="" && Remark.Substring(0,2).ToString() == "SD")
                                        {
                                            int NotSDFormatFound = 0;
                                            //Check Date
                                            try
                                            {
                                                string StringDate = Remark.Replace("SD","");
                                                StringDate = StringDate.Substring(0,StringDate.Length-3);
                                                string yyyy = "20" + StringDate.Substring(0,2);
                                                string mm = StringDate.Substring(2, 2);
                                                string dd = StringDate.Substring(4, 2);
                                                DateTime SDDate = Convert.ToDateTime(yyyy+"-"+mm+"-"+dd);
                                                Console.WriteLine(SDDate);
                                            }
                                            catch
                                            {
                                                NotSDFormatFound++;
                                            }

                                            //Check Last 3 digit
                                            try
                                            {
                                                string StringNo = Remark.Replace("SD", "");
                                                double No = Convert.ToDouble(Remark.Substring(StringNo.Length - 3,3));
                                                Console.WriteLine(No);
                                            }
                                            catch
                                            {
                                                NotSDFormatFound++;
                                            }

                                            if (NotSDFormatFound == 0)
                                            {
                                                cmd.Parameters.AddWithValue("@Pn", Remark);
                                            }
                                            else
                                            {
                                                cmd.Parameters.AddWithValue("@Pn", POSNo);
                                            }
                                        }
                                        else
                                        {
                                            cmd.Parameters.AddWithValue("@Pn", POSNo);
                                        }
                                    }
                                    //Other
                                    else 
                                    {
                                        if (InLocCode == "WIR1")
                                        {
                                            cmd.Parameters.AddWithValue("@Pn", "");
                                        }
                                        else
                                        {
                                            if (Remark.Contains("_NGRate") == true)
                                            {
                                                cmd.Parameters.AddWithValue("@Pn", Remark.Replace("_NGRate", ""));
                                            }
                                            else
                                            {
                                                cmd.Parameters.AddWithValue("@Pn", POSNo);
                                            }
                                        }
                                    }
                                    cmd.Parameters.AddWithValue("@Cd", Code);
                                    cmd.Parameters.AddWithValue("@Rmd", Items);
                                    cmd.Parameters.AddWithValue("@Rq", TransferQty);
                                    cmd.Parameters.AddWithValue("@Tq", 0);
                                    cmd.Parameters.AddWithValue("@Sv", TransferQty);
                                    cmd.Parameters.AddWithValue("@Rd", RegDate.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss"));
                                    cmd.Parameters.AddWithValue("@Rb", User);
                                    cmd.Parameters.AddWithValue("@Cs", 0);
                                    cmd.Parameters.AddWithValue("@Rem", Remark);
                                    cmd.ExecuteNonQuery();

                                }
                                else
                                {
                                    dtError.Rows.Add(Code, Items, OutLocName, POSNo, TransferQty);
                                    dtError.AcceptChanges();
                                }
                            }
                        }
                    }
                }
                if (dtError.Rows.Count == 0)
                {
                    MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CleardtTransferSelected();
                    dgvScanned.Rows.Clear();
                    CboOutLocation.SelectedIndex = 0;
                    CboInLocation.SelectedIndex = 0;
                    ClearText();
                    ShowStockAvailableAndSelected();
                    CheckBtnSaveAndDelete();
                }
                else
                {
                    //Remove Dgv
                    for (int i = dgvScanned.Rows.Count - 1; i > -1; i--)
                    {
                        int Found = 0;
                        foreach (DataRow row in dtError.Rows)
                        {
                            if (dgvScanned.Rows[i].Cells[0].Value.ToString() == row[0].ToString() && dgvScanned.Rows[i].Cells[2].Value.ToString() == row[2].ToString())
                            {
                                Found = Found + 1;
                                break;
                            }
                        }
                        if (Found == 0)
                        {
                            dgvScanned.Rows.RemoveAt(i);
                        }
                    }
                    //Remove dtTransfer
                    foreach (DataRow row in dtTransferSelected.Rows)
                    {
                        int Found = 0;
                        foreach (DataRow rowError in dtError.Rows)
                        {
                            if (row[0].ToString() == rowError[0].ToString() && row[1].ToString() == rowError[2].ToString() && row[4].ToString() == rowError[3].ToString())
                            {
                                Found = Found + 1;
                                break;
                            }
                        }
                        if (Found == 0)
                        {
                            dtTransferSelected.Rows.Remove(row);
                            dtTransferSelected.AcceptChanges();
                        }
                    }
                    dgvScanned.Update();
                    string MsgBox = "រក្សាទុកមានបញ្ហា!\n";
                    foreach (DataRow row in dtError.Rows)
                    {
                        MsgBox = MsgBox + row[0].ToString() + "\t" + row[1].ToString() + "\t\t" + row[2].ToString() + "\t" + row[3].ToString() + "\t" + row[4].ToString() + "\t : ខ្វះស្តុក!\n";
                    }
                    MessageBox.Show(MsgBox, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            cnn.con.Close();
        }

    }
}
