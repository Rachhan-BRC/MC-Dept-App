using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.Inventory.MC_SD
{
    public partial class MCSDUpdateForm : Form
    {
        SQLConnect con = new SQLConnect();
        SQLConnectOBS conOBS = new SQLConnectOBS();

        public MCSDUpdateForm()
        {
            con.Connection();
            InitializeComponent();
            this.txtscan.KeyDown += Txtscan_KeyDown;
            this.txtlbinput.KeyDown += Txtlbinput_KeyDown;
            this.btnSave.Click += BtnSave_Click;
            this.btnNew.Click += BtnNew_Click;
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            txtscan.Clear();
            txtcode.Text = "";
            txtrmname.Text = "";
            txtlabel.Text = "";
            bobinQty.Text = "";
            bobinw.Text = "";
            txtQty.Text = "";
            txtttlweight.Text = "";
            txtlbinput.Clear();
            txtscan.Enabled = true;
            txtlbinput.Enabled = true;
            txtscan.Focus();

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(bobinQty.Text.Trim()) &&
                !string.IsNullOrEmpty(bobinw.Text.Trim()) &&
                !string.IsNullOrEmpty(txtQty.Text.Trim()) &&
                 !string.IsNullOrEmpty(txtttlweight.Text.Trim())
                )
            {
                DialogResult dr = MessageBox.Show("តើអ្នកចង់រក្សាទុកការកែប្រែទិន្នន័យនេះមែនទេ?", "Confirm Save'", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    return;
                }
                Cursor = Cursors.WaitCursor;
                try
                {
                    con.con.Open();
                    string queryUpdate = "UPDATE tbSDMCStockInventory SET Qty = @Qty, TotalWeight = @TotalWeight, UpdateBy = @uby, UpdateDate = @ud WHERE SysNo = @SysNo AND Code = @code AND LabelNo = @lbno";
                    SqlCommand cmdUpdate = new SqlCommand(queryUpdate, con.con);
                    cmdUpdate.Parameters.AddWithValue("@Qty", txtQty.Text.Trim());
                    cmdUpdate.Parameters.AddWithValue("@TotalWeight", txtttlweight.Text.Trim());
                    cmdUpdate.Parameters.AddWithValue("@SysNo", txtscan.Text.Trim());
                    cmdUpdate.Parameters.AddWithValue("@Code", txtcode.Text.Trim());
                    cmdUpdate.Parameters.AddWithValue("@lbno", txtlabel.Text.Trim());
                    cmdUpdate.Parameters.AddWithValue("@uby" , MenuFormV2.UserForNextForm);
                    cmdUpdate.Parameters.AddWithValue("@ud", DateTime.Now);
                    cmdUpdate.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 3!" + ex.Message, "ErrorSaveData'", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Cursor = Cursors.Default;
                    return;
                }
                con.con.Close();
                Cursor = Cursors.Default;
                txtscan.Clear();
                txtcode.Text = "";
                txtrmname.Text = "";
                txtlabel.Text = "";
                bobinQty.Text = "";
                bobinw.Text = "";
                txtQty.Text = "";
                txtttlweight.Text = "";
                txtlbinput.Text = "";
                txtscan.Enabled = true;
                txtlbinput.Enabled = true;

                txtscan.Focus();
                MessageBox.Show("ធ្វើការកែប្រែទិន្នន័យបានជោគជ័យ !", "Success'", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
          
        }

        private void Txtlbinput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Cursor = Cursors.WaitCursor;
                string lb = txtlbinput.Text.Trim();
                if (!string.IsNullOrEmpty(lb))
                {
                    DataTable dtselect = new DataTable();
                    try
                    {
                        con.con.Open();
                        string querySelect = "SELECT * FROM tbSDMCStockInventory WHERE LabelNo = '" + lb + "' AND Status = 'Active'";
                        SqlDataAdapter sdaSelect = new SqlDataAdapter(querySelect, con.con);
                        sdaSelect.Fill(dtselect);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 2 !" + ex.Message, "Error ScanUpdateText'", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    con.con.Close();
                    if (dtselect.Rows.Count <= 0)
                    {
                        string soundPath = Path.Combine(Environment.CurrentDirectory, @"Sound\NoCode.wav");
                        SoundPlayer player = new SoundPlayer(soundPath);
                        player.Play();
                        MessageBox.Show("មិនមានឡាប៊ែលប្រភេទនេះទេ !", "Alert'", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Cursor = Cursors.Default;
                        return;
                    }
                    txtcode.Text = dtselect.Rows[0]["Code"].ToString();
                    txtrmname.Text = dtselect.Rows[0]["RMName"].ToString();
                    txtlabel.Text = dtselect.Rows[0]["LabelNo"].ToString();
                    bobinQty.Text = dtselect.Rows[0]["BobbinQty"].ToString();
                    bobinw.Text = dtselect.Rows[0]["BobinWeight"].ToString();
                    txtQty.Text = dtselect.Rows[0]["Qty"].ToString();
                    txtttlweight.Text = dtselect.Rows[0]["TotalWeight"].ToString();
                    txtscan.Text = dtselect.Rows[0]["SysNo"].ToString();
                    txtlbinput.Enabled = false;
                    txtscan.Enabled = false;
                }
                Cursor = Cursors.Default;
            }
        }

        private void Txtscan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Cursor = Cursors.WaitCursor;
                string scan = txtscan.Text.Trim();
                if (!string.IsNullOrEmpty(scan))
                {
                    DataTable dtselect = new DataTable();
                    try
                    {
                        con.con.Open();
                        string querySelect = "SELECT * FROM tbSDMCStockInventory WHERE SysNo = '"+scan+"' AND Status = 'Active'";
                        SqlDataAdapter sdaSelect = new SqlDataAdapter(querySelect, con.con);
                        sdaSelect.Fill(dtselect);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 1 !" + ex.Message, "Error ScanUpdateText'", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    con.con.Close();
                    if (dtselect.Rows.Count <= 0)
                    {
                        string soundPath = Path.Combine(Environment.CurrentDirectory, @"Sound\WrongSysNo.wav");
                        SoundPlayer player = new SoundPlayer(soundPath);
                        player.Play();
                        MessageBox.Show("មិនមានឡាប៊ែលប្រភេទនេះទេ !", "Error'", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Cursor = Cursors.Default;
                        return;
                    }
                    txtcode.Text = dtselect.Rows[0]["Code"].ToString();
                    txtrmname.Text = dtselect.Rows[0]["RMName"].ToString();
                    txtlabel.Text = dtselect.Rows[0]["LabelNo"].ToString();
                    bobinQty.Text = dtselect.Rows[0]["BobbinQty"].ToString();
                    bobinw.Text = dtselect.Rows[0]["BobinWeight"].ToString();
                    txtQty.Text = dtselect.Rows[0]["Qty"].ToString();
                    txtttlweight.Text = dtselect.Rows[0]["TotalWeight"].ToString();
                    txtlbinput.Text = dtselect.Rows[0]["LabelNo"].ToString();
                    txtscan.Enabled = false;
                    txtlbinput.Enabled = false;
                }
                Cursor = Cursors.Default;
            }
        }
    }
}
