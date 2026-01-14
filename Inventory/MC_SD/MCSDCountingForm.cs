using MachineDeptApp.Inventory.MC_SD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.TextFormatting;
using System.IO;

namespace MachineDeptApp
{
    public partial class MCSDCountingForm : Form
    {
        SQLConnect con = new SQLConnect();
        public MCSDCountingForm()
        {
            con.Connection();
            InitializeComponent();
            this.Shown += MCSDCountingForm_Shown;
            this.cbPic.DropDown += CbPic_DropDown;
            this.txtscan.KeyDown += Txtscan_KeyDown;
            this.btnOk.Click += BtnOk_Click;
            this.btnChange.Click += BtnChange_Click;
            this.cbPic.TextChanged += CbPic_TextChanged;
            this.txtttllenght.TextChanged += Txtttllenght_TextChanged;
            this.btnSave.Click += BtnSave_Click;
            this.txtcode.KeyDown += Txtcode_KeyDown;
            this.txtremainL.TextChanged += TxtremainL_TextChanged;
            this.txtses.TextChanged += Txtses_TextChanged;
            this.btnPrint.Click += BtnPrint_Click;

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tablabel)
            {
                for (int i = 0; i < numPrintQty.Value; i ++)
                {

                }
            }
            if (tabControl1.SelectedTab == tabcode)
            {
                for (int i = 0; i < numPrintQty.Value; i++)
                {

                }
            }

        }

        private void Txtses_TextChanged(object sender, EventArgs e)
        {
            if (txtses.Text.Trim() != "")
            {
                double remainL = Convert.ToDouble(txtremainL.Text.Trim());
                double bobin = Convert.ToDouble(txtbobin.Text.Trim());
                double ses = Convert.ToDouble(txtses.Text.Trim());
                double qty = 0;
                qty = remainL * bobin + ses;
                txtremainL2.Text = qty.ToString("N3");
                txtremainL3.Text = qty.ToString("N3");
            }
        }

        private void TxtremainL_TextChanged(object sender, EventArgs e)
        {
            if (txtremainL.Text.Trim() != "" && txtbobin.Text.Trim() != "")
            {
                double remainL = Convert.ToDouble(txtremainL.Text.Trim());
                double bobin = Convert.ToDouble(txtbobin.Text.Trim());
                double ses = Convert.ToDouble(txtses.Text.Trim());
                double qty = 0;
                qty = remainL * bobin + ses;
                txtremainL2.Text = qty.ToString("N3");
                txtremainL3.Text = qty.ToString("N3");
            }
        }

        private void Txtcode_KeyDown(object sender, KeyEventArgs e)
        {
            string txtpic = cbPic.Text.Trim();
            Cursor = Cursors.WaitCursor;
            if (e.KeyCode == Keys.Enter)
            {
                if (cbPic.Text.Trim() == "")
                {
                    MessageBox.Show("សូមជ្រើសរើសអ្នករាប់ មុននឹងប្រើប្រាស់ !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Cursor = Cursors.Default;
                    return;
                }
                if (btnOk.Enabled == true)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show("សូមចុចរក្សាទុក មុននឹងប្រើប្រាស់ !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string input = txtcode.Text.Trim();
                if (!string.IsNullOrEmpty(input))
                {
                    int labelNo = Convert.ToInt32(lbstart.Text.Trim());
                    //Compare Code
                    DataTable dtcompare = new DataTable();
                    Cursor = Cursors.WaitCursor;
                    try
                    {
                        con.con.Open();
                        string queryCompare = "SELECT * FROM tbMasterItem " +
                            "WHERE ItemType='Material' AND (RMTypeName IS NOT NULL AND RMTypeName <> 'Connector') AND ItemCode = '"+input+"' " +
                            "ORDER BY ItemCode ";
                        SqlDataAdapter sdaCompare = new SqlDataAdapter(queryCompare, con.con);
                        sdaCompare.Fill(dtcompare);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 9 !" + ex.Message, "Error CodeTextdown'", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    con.con.Close();
                    if (dtcompare.Rows.Count <= 0)
                    {
                        string soundPath = Path.Combine(Environment.CurrentDirectory, @"Sound\NoCode.wav");
                        SoundPlayer player = new SoundPlayer(soundPath);
                        player.Play();
                        MessageBox.Show("មិនមានកូដប្រភេទនេះទេ !", "Alert'", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Cursor = Cursors.Default;
                        return;
                    }
                    //Get Max LabelNo
                    DataTable dtlabel = new DataTable();
                    try
                    {
                        con.con.Open();
                        string selectquery = " SELECT MAX(LabelNo) FROM tbSDMCStockInventory WHERE PIC = '" + txtpic + "'";
                        SqlDataAdapter sdaselect = new SqlDataAdapter(selectquery, con.con);
                        sdaselect.Fill(dtlabel);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 10 !" + ex.Message, "Error Scantextdown'", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (dtlabel.Rows.Count > 0 && dtlabel.Rows[0][0] != DBNull.Value)
                    {
                        int lb = Convert.ToInt32(dtlabel.Rows[0][0]);
                        labelNo = lb + 1;
                    }
                    con.con.Close();
                    //Get Stock from tbSDMCAllTransaction
                    DataTable dtstock = new DataTable();
                    try
                    {
                        con.con.Open();
                        string queryStock = "SELECT SUM(StockValue) AS StockRemain FROM [MachineDB].[dbo].[tbSDMCAllTransaction] " +
                            "WHERE LocCode = 'WIR1' AND CancelStatus = 0  AND Code ='" + input + "'";
                        SqlDataAdapter sdastock = new SqlDataAdapter(queryStock, con.con);
                        sdastock.Fill(dtstock);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 11 !" + ex.Message, "ErrorScantextdown", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Cursor = Cursors.Default;
                        return;
                    }
                    con.con.Close();
                    //StockAlreadyCount 
                    DataTable dtCount = new DataTable();
                    try
                    {
                        con.con.Open();
                        string queryCount = " SELECT SUM(Qty) AS StockCount FROM [MachineDB].[dbo].[tbSDMCStockInventory] WHERE Code = '" + input + "'";
                        SqlDataAdapter sdacount = new SqlDataAdapter(queryCount, con.con);
                        sdacount.Fill(dtCount);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 12 !" + ex.Message, "ErrorScantextdown", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Cursor = Cursors.Default;
                        return;
                    }
                    con.con.Close();
                    if (dtCount.Rows[0]["StockCount"] == DBNull.Value)
                    {
                        txtttllenght.Text = "0";
                    }
                    else
                    {
                        double stoccount = Convert.ToDouble(dtCount.Rows[0]["StockCount"]);
                        txtttllenght.Text = stoccount.ToString("N2");
                    }
                    con.con.Close();
                    int count = Convert.ToInt32(dgvSd.Rows.Count) + 1;
                    //fill data grid
                    ClearFields();
                    txtrmname2.Text = dtcompare.Rows[0]["ItemName"].ToString();
                    txtremainL.Text = "0";
                    txtbobin.Text = "1";
                    txtses.Text = "0";
                    txtremainL2.Text = "0";
                    txtremainL3.Text = "0";
                    txtcount.Text = count.ToString();
                    txtstockcard.Text = dtstock.Rows[0]["StockRemain"].ToString();
                    txtlabel.Text = labelNo.ToString();
                    txtremainL.Focus();
                }
            }
            Cursor = Cursors.Default;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (!string.IsNullOrWhiteSpace(txtrmname2.Text) &&
                !string.IsNullOrWhiteSpace(txtremainL.Text) &&
                !string.IsNullOrWhiteSpace(txtbobin.Text) &&
                !string.IsNullOrWhiteSpace(txtses.Text) &&
                !string.IsNullOrWhiteSpace(txtremainL2.Text) &&
                !string.IsNullOrWhiteSpace(txtlabel.Text))
            {
                int success = 0;
                try
                {
                    con.con.Open();
                    string queryInsert = "INSERT INTO tbSDMCStockInventory (SysNo, Code, RMName, TotalWeight, BobinWeight, Qty, BobbinQty, LabelNo, PIC, Status, RegBy, RegDate, Updateby, UpdateDate) " +
                           "VALUES (@SysNo, @Code, @RMName, @TotalWeight, @BobinWeight, @Qty, @BobbinQty, @LabelNo, @PIC, @st, @Regby, @Regdate, @UpdateBy, @UpdateDate)";
                    SqlCommand cmdInsert = new SqlCommand(queryInsert, con.con);
                    cmdInsert.Parameters.AddWithValue("@SysNo", "");
                    cmdInsert.Parameters.AddWithValue("@Code", txtcode.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@RMName", txtrmname2.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@TotalWeight", Convert.ToDouble(0));
                    cmdInsert.Parameters.AddWithValue("@BobinWeight", Convert.ToDouble(0));
                    cmdInsert.Parameters.AddWithValue("@Qty", Convert.ToDouble(txtremainL2.Text.Trim()));
                    cmdInsert.Parameters.AddWithValue("@BobbinQty", Convert.ToDouble(txtbobin.Text.Trim()));
                    cmdInsert.Parameters.AddWithValue("@LabelNo", Convert.ToInt32(txtlabel.Text.Trim()));
                    cmdInsert.Parameters.AddWithValue("@PIC", cbPic.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@st", "Active");
                    cmdInsert.Parameters.AddWithValue("@Regby", MenuFormV2.UserForNextForm);
                    cmdInsert.Parameters.AddWithValue("@Regdate", DateTime.Now);
                    cmdInsert.Parameters.AddWithValue("@UpdateBy", MenuFormV2.UserForNextForm);
                    cmdInsert.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                    cmdInsert.ExecuteNonQuery();
                    success++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 13 !" + ex.Message, "ErrorSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.con.Close();

                if (success > 0)
                {
                    SelectData();
                    txtcount.Text = dgvSd.Rows.Count.ToString();
                    labelstatus.Text = "OK";
                    Panelstatus.BackColor = Color.LimeGreen;
                    string soundPath = Path.Combine(Environment.CurrentDirectory, @"Sound\OK.wav");
                    SoundPlayer player = new SoundPlayer(soundPath);
                    player.Play();
                }
            }
            Cursor = Cursors.Default;
        }

        private void MCSDCountingForm_Shown(object sender, EventArgs e)
        {
            btnChange.Enabled = false;
            MessageBox.Show("សូមជ្រើសរើសអ្នករាប់ មុននឹងប្រើប្រាស់ !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Txtttllenght_TextChanged(object sender, EventArgs e)
        {
            if (txtttllenght.Text.Trim() != "" && txtstockcard.Text.Trim() != "")
            {
                double ttlL = Convert.ToDouble(txtttllenght.Text.Trim());
                double stockcard = Convert.ToDouble(txtstockcard.Text.Trim());

                if (ttlL == stockcard)
                {
                    labelstatus.Text = "Complete";
                    Panelstatus.BackColor = Color.LimeGreen;
                    string soundPath = Path.Combine(Environment.CurrentDirectory, @"Sound\Complete.wav");
                    SoundPlayer player = new SoundPlayer(soundPath);
                    player.Play();
                }
            }
        }

        private void CbPic_TextChanged(object sender, EventArgs e)
        {
            string selectedPic = cbPic.Text.Trim();
            if (!string.IsNullOrEmpty(selectedPic))
            {
                DataTable dt = new DataTable();
                try
                {
                    con.con.Open();
                    string query = "SELECT  LabelNoStart, LabelNoEnd FROM tbMstInventoryPic WHERE PIC = '" + selectedPic+"'";
                    SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        string lbS = dt.Rows[0]["LabelNoStart"].ToString();
                        string lbE = dt.Rows[0]["LabelNoEnd"].ToString();
                        lbstart.Text = lbS;
                        lbend.Text = lbE;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាបច្ចេកទេស សូមទាក់ទងទៅ IT (Phanun) 2" + ex.Message, "Error CbTextChange", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.con.Close();
            }
        }

        private void BtnChange_Click(object sender, EventArgs e)
        {
            cbPic.Enabled = true;
            btnChange.Enabled = false;
            btnOk.Enabled = true;
            btnOk.BringToFront();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (cbPic.Text.Trim() != "" && lbstart.Text.Trim() != "" && lbend.Text.Trim() != "")
            {
                btnOk.Enabled = false;
                btnChange.Enabled = true;
                cbPic.Enabled = false;
                btnChange.BringToFront();
                SelectData();
                ClearFields();
                txtscan.Focus();
            }
        }

        private void Txtscan_KeyDown(object sender, KeyEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (e.KeyCode == Keys.Enter)
            {
                if (cbPic.Text.Trim() == "")
                {
                    MessageBox.Show("សូមជ្រើសរើសអ្នករាប់ មុននឹងប្រើប្រាស់ !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Cursor = Cursors.Default;
                    return;
                }
                if (btnOk.Enabled == true)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show("សូមចុចរក្សាទុក មុននឹងប្រើប្រាស់ !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string input = txtscan.Text.Trim();
                if (!string.IsNullOrEmpty(input))
                {
                    int labelNo = Convert.ToInt32(lbstart.Text.Trim());
                    string txtpic = cbPic.Text.Trim();
                    //Compare SysNo
                    DataTable dtcompare = new DataTable();
                    try
                    {
                        con.con.Open();
                        string queryCompare = "SELECT SysNo FROM tbSDMCStockInventory WHERE SysNo = '" + input + "' AND Status = 'Active'";
                        SqlDataAdapter sdaCompare = new SqlDataAdapter(queryCompare, con.con);
                        sdaCompare.Fill(dtcompare);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 8 !" + ex.Message, "Error Scantextdown'", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    con.con.Close();
                    if (dtcompare.Rows.Count > 0)
                    {
                        labelstatus.Text = "Error";
                        Panelstatus.BackColor = Color.Red;
                        string soundPath = Path.Combine(Environment.CurrentDirectory, @"Sound\AlreadyScan.wav");
                        SoundPlayer player = new SoundPlayer(soundPath);
                        player.Play();
                        MessageBox.Show("ឡាប៊ែលនេះស្កេនរួចហើយ !", "Alert'", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Cursor = Cursors.Default;
                        return;
                    }
                    //Get Data from tbMstRMRegister
                    DataTable datafill = new DataTable();
                    try
                    {
                        con.con.Open();
                        string queryfill = "SELECT [tbMstRMRegister].*, [tbMasterItem].[ItemName] FROM [tbMstRMRegister] " +
                            "JOIN [tbMasterItem] " +
                            "ON [tbMstRMRegister].RMCode = [tbMasterItem].ItemCode " +
                            "WHERE [BobbinSysNo] = '" + input + "' AND Status = 'Active'";
                        SqlDataAdapter sdafill = new SqlDataAdapter(queryfill, con.con);
                        Console.WriteLine(datafill.Rows.Count);
                        sdafill.Fill(datafill);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 3 !" + ex.Message, "Error Scantextdown'", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    con.con.Close();
                    if (datafill.Rows.Count <= 0)
                    {
                        labelstatus.Text = "Error";
                        Panelstatus.BackColor = Color.Red;
                        string soundPath = Path.Combine(Environment.CurrentDirectory, @"Sound\WrongSysNo.wav");
                        SoundPlayer player = new SoundPlayer(soundPath);
                        player.Play();
                        MessageBox.Show("មិនមានឡាប៊ែលប្រភេទនេះទេ !", "Error'", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Cursor = Cursors.Default;
                        return;
                    }
                    //Get Max LabelNo
                    DataTable dtlabel = new DataTable();
                    try
                    {
                        con.con.Open();
                        string selectquery = " SELECT MAX(LabelNo) FROM tbSDMCStockInventory WHERE PIC = '"+txtpic+"' AND Status = 'Active'";
                        SqlDataAdapter sdaselect = new SqlDataAdapter(selectquery, con.con);
                       sdaselect.Fill(dtlabel);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 5 !" + ex.Message, "Error Scantextdown'", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (dtlabel.Rows.Count > 0 && dtlabel.Rows[0][0] != DBNull.Value)
                    {
                        int lb = Convert.ToInt32(dtlabel.Rows[0][0]);
                        labelNo = lb + 1;
                    }
                    con.con.Close();
                    //Get Stock from tbSDMCAllTransaction
                    DataTable dtstock = new DataTable();
                    string code = datafill.Rows[0]["RMCode"].ToString();
                    try
                    {
                        con.con.Open();
                        string queryStock = "SELECT SUM(StockValue) AS StockRemain FROM [MachineDB].[dbo].[tbSDMCAllTransaction] " +
                            "WHERE LocCode = 'WIR1' AND CancelStatus = 0  AND Code ='" + code + "'";
                        SqlDataAdapter sdastock = new SqlDataAdapter(queryStock, con.con);
                        sdastock.Fill(dtstock);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 4 !" + ex.Message, "ErrorScantextdown", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Cursor = Cursors.Default;
                        return;
                    }
                    con.con.Close();
                    txtcode.Clear();
                    ClearFields();
                    string sysno = datafill.Rows[0]["BobbinSysNo"].ToString();
                    string type = datafill.Rows[0]["RMType"].ToString();
                    double bobinw = Convert.ToDouble(datafill.Rows[0]["BobbinW"].ToString());
                    double moqw = Convert.ToDouble(datafill.Rows[0]["MOQ_W"].ToString());
                    double moql = Convert.ToDouble(datafill.Rows[0]["MOQ_L"].ToString());
                    double remainw = Convert.ToDouble(datafill.Rows[0]["Remain_W"].ToString());
                    double remainl = Convert.ToDouble(datafill.Rows[0]["Remain_L"].ToString());
                    double perunit = Convert.ToDouble(datafill.Rows[0]["Per_Unit"].ToString());
                    string itemname = datafill.Rows[0]["ItemName"].ToString();
                    txtsysno.Text = sysno;
                    txtname.Text = itemname;
                    txtremainw.Text = remainw.ToString("N2");
                    txtbobinw.Text = bobinw.ToString("N2");
                    txtttlremain.Text = remainl.ToString("N2");
                    txtremainL3.Text = remainl.ToString("N2");
                    txtstockcard.Text = dtstock.Rows[0]["StockRemain"].ToString();
                    txtlabel.Text = labelNo.ToString();
                    
                    txtscan.Text = "";
                    //Insert data to tbSDMCStockInventory
                    int success = 0;
                    try
                    {
                        con.con.Open();
                        string queryinsert = "INSERT INTO tbSDMCStockInventory (SysNo, Code, RMName, TotalWeight, BobinWeight, Qty, BobbinQty, LabelNo, PIC, Status, RegBy, RegDate, Updateby, UpdateDate) " +
                            "VALUES (@SysNo, @Code, @RMName, @TotalWeight, @BobinWeight, @Qty, @BobbinQty, @LabelNo, @PIC, @st, @Regby, @Regdate, @UpdateBy, @UpdateDate)";
                        SqlCommand cmdinsert = new SqlCommand(queryinsert, con.con);
                        cmdinsert.Parameters.AddWithValue("@SysNo", txtsysno.Text.Trim());
                        cmdinsert.Parameters.AddWithValue("@Code", code );
                        cmdinsert.Parameters.AddWithValue("@RMName", txtname.Text.Trim());
                        cmdinsert.Parameters.AddWithValue("@TotalWeight", Convert.ToDouble(txtremainw.Text.Trim()));
                        cmdinsert.Parameters.AddWithValue("@BobinWeight", Convert.ToDouble(txtbobinw.Text.Trim()));
                        cmdinsert.Parameters.AddWithValue("@Qty", Convert.ToDouble(txtremainL3.Text.Trim()));
                        cmdinsert.Parameters.AddWithValue("@BobbinQty", 1);
                        cmdinsert.Parameters.AddWithValue("@LabelNo", Convert.ToInt32(txtlabel.Text.Trim()));
                        cmdinsert.Parameters.AddWithValue("@PIC", cbPic.Text.Trim());
                        cmdinsert.Parameters.AddWithValue("@st", "Active");
                        cmdinsert.Parameters.AddWithValue("@Regby", MenuFormV2.UserForNextForm);
                        cmdinsert.Parameters.AddWithValue("@Regdate", DateTime.Now);
                        cmdinsert.Parameters.AddWithValue("@UpdateBy", MenuFormV2.UserForNextForm);
                        cmdinsert.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                        cmdinsert.ExecuteNonQuery();
                        success++;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 7 !" + ex.Message, "Error Insert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    con.con.Close();
                    if (success > 0)
                    {
                        SelectData();
                        txtcount.Text = dgvSd.Rows.Count.ToString();
                        labelstatus.Text = "OK";
                        Panelstatus.BackColor = Color.LimeGreen;
                        string soundPath = Path.Combine(Environment.CurrentDirectory, @"Sound\OK.wav");
                        SoundPlayer player = new SoundPlayer(soundPath);
                        player.Play();

                        //StockAlreadyCount 
                        DataTable dtCount = new DataTable();
                        try
                        {
                            con.con.Open();
                            string queryCount = " SELECT SUM(Qty) AS StockCount FROM [MachineDB].[dbo].[tbSDMCStockInventory] WHERE Code = '" + code + "' AND Status = 'Active'";
                            SqlDataAdapter sdacount = new SqlDataAdapter(queryCount, con.con);
                            sdacount.Fill(dtCount);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 4 !" + ex.Message, "ErrorScantextdown", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Cursor = Cursors.Default;
                            return;
                        }
                        con.con.Close();
                        if (dtCount.Rows.Count <= 0)
                        {
                            txtttllenght.Text = "0";
                        }
                        else
                        {
                            double stoccount = Convert.ToDouble(dtCount.Rows[0]["StockCount"].ToString());
                            txtttllenght.Text = stoccount.ToString("N2");
                        }
                    }
                    else
                    {
                        labelstatus.Text = "Error";
                        Panelstatus.BackColor = Color.Red;
                    }
                    Cursor = Cursors.Default;
                }
            }
            Cursor = Cursors.Default;
        }
        private void CbPic_DropDown(object sender, EventArgs e)
        {
            cbPic.Items.Clear();
            DataTable dt = new DataTable();
            try
            {
                con.con.Open();
                string query = "SELECT * FROM tbMstInventoryPic";
                SqlDataAdapter sda = new SqlDataAdapter(query, con.con);
                sda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    string picName = row["PIC"].ToString();
                    cbPic.Items.Add("  " + picName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហាបច្ចេកទេស សូមទាក់ទងទៅ IT (Phanun) 1" + ex.Message, "Error CbDropDown", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.con.Close();
        }
       private void SelectData()
        {
            dgvSd.Rows.Clear();
            string txtpic = cbPic.Text.Trim();
            DataTable dtdgv = new DataTable();
            try
            {
                con.con.Open();
                string querydgv = "SELECT * FROM tbSDMCStockInventory WHERE PIC = '"+txtpic+"' AND Status = 'Active'";
                SqlDataAdapter sdadgv = new SqlDataAdapter(querydgv, con.con);
                sdadgv.Fill(dtdgv);
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 6 !" + ex.Message, "Error Method'", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.con.Close();
             foreach (DataRow row in dtdgv.Rows)
            {
                dgvSd.Rows.Add();
                dgvSd.Rows[dgvSd.Rows.Count - 1].Cells["sysno"].Value = row["SysNo"].ToString();
                dgvSd.Rows[dgvSd.Rows.Count - 1].Cells["code"].Value = row["Code"].ToString();
                dgvSd.Rows[dgvSd.Rows.Count - 1].Cells["rmname"].Value = row["RMName"].ToString();
                dgvSd.Rows[dgvSd.Rows.Count - 1].Cells["ttlw"].Value = row["TotalWeight"].ToString();
                dgvSd.Rows[dgvSd.Rows.Count - 1].Cells["bbw"].Value = row["BobinWeight"].ToString();
                dgvSd.Rows[dgvSd.Rows.Count - 1].Cells["Qty"].Value = row["Qty"].ToString();
                dgvSd.Rows[dgvSd.Rows.Count - 1].Cells["bbqty"].Value = row["BobbinQty"].ToString();
                dgvSd.Rows[dgvSd.Rows.Count - 1].Cells["lbno"].Value = row["LabelNo"].ToString();
               dgvSd.Rows[dgvSd.Rows.Count - 1].Cells["pic"].Value = row["PIC"].ToString();

            }
        }
        private void ClearFields()
        {
            txtname.Clear();
            txtremainw.Clear();
            txtbobinw.Clear();
            txtttlremain.Clear();
            txtremainL3.Clear();
            txtstockcard.Clear();
            txtlabel.Clear();
            txtcount.Clear();
            txtsysno.Clear();
        }
    }
}
