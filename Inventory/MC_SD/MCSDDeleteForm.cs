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
using System.IO;

namespace MachineDeptApp.Inventory.MC_SD
{
    public partial class MCSDDeleteForm : Form
    {
        SQLConnect con = new SQLConnect();
        //true = loaded by Label No (txtlbinput) >> delete every Active row of this Label No
        //false = loaded by SysNo (txtscan) >> delete only that bobbin
        bool deleteByLabel = false;
        int deleteLabelNo = 0;
        int deleteRowCount = 0;

        public MCSDDeleteForm()
        {
            con.Connection();
            InitializeComponent();
            this.txtscan.KeyDown += Txtscan_KeyDown;
            this.txtlbinput.KeyDown += Txtlbinput_KeyDown;
            this.btnNew.Click += BtnNew_Click;
            this.btnDelete.Click += BtnDelete_Click;

        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(bobinQty.Text.Trim()) &&
                !string.IsNullOrEmpty(bobinw.Text.Trim()) &&
                !string.IsNullOrEmpty(txtQty.Text.Trim()) &&
                 !string.IsNullOrEmpty(txtttlweight.Text.Trim()))
                {
                string confirmText = deleteByLabel
                    ? "តើអ្នកចង់លុបឡាប៊ែល " + deleteLabelNo.ToString("D4") + " ទាំងអស់ (" + deleteRowCount + " ជួរ) មែនទេ?"
                    : "តើអ្នកចង់លុបព័ត៌មាននេះមែនទេ?";
                DialogResult dr = MessageBox.Show(confirmText, "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    return;
                }
                Cursor = Cursors.WaitCursor;
                try
                {
                    con.con.Open();
                    SqlCommand cmdDelete;
                    if (deleteByLabel)
                    {
                        //Every Active row of this Label No
                        cmdDelete = new SqlCommand("UPDATE tbSDMCStockInventory SET Status = @st, UpdateBy = @uby, UpdateDate = @ud WHERE LabelNo = @lbno AND Status = 'Active'", con.con);
                        cmdDelete.Parameters.AddWithValue("@lbno", deleteLabelNo);
                    }
                    else
                    {
                        cmdDelete = new SqlCommand("UPDATE tbSDMCStockInventory SET Status = @st, UpdateBy = @uby, UpdateDate = @ud WHERE SysNo = @SysNo AND Code = @code AND LabelNo = @lbno", con.con);
                        cmdDelete.Parameters.AddWithValue("@SysNo", txtscan.Text.Trim());
                        cmdDelete.Parameters.AddWithValue("@Code", txtcode.Text.Trim());
                        cmdDelete.Parameters.AddWithValue("@lbno", txtlabel.Text.Trim());
                    }
                    cmdDelete.Parameters.AddWithValue("@st", "Deleted");
                    cmdDelete.Parameters.AddWithValue("@uby", MenuFormV2.UserForNextForm);
                    cmdDelete.Parameters.AddWithValue("@ud", DateTime.Now);
                    int deleted = cmdDelete.ExecuteNonQuery();
                    ClearForm();
                    MessageBox.Show("លុបបានជោគជ័យ! (" + deleted + " ជួរ)", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 3 !" + ex.Message, "Error Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                txtscan.Enabled = true;
                txtlbinput.Enabled = true;
                con.con.Close();
                Cursor = Cursors.Default;
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
                    //Every Active row of this Label No (one Label No = one PIC + Code, can be many bobbins)
                    int lbNo;
                    DataTable dtselect = new DataTable();
                    if (int.TryParse(lb, out lbNo))
                    {
                        try
                        {
                            con.con.Open();
                            SqlDataAdapter sdaSelect = new SqlDataAdapter("SELECT * FROM tbSDMCStockInventory WHERE LabelNo = @lbno AND Status = 'Active'", con.con);
                            sdaSelect.SelectCommand.Parameters.AddWithValue("@lbno", lbNo);
                            sdaSelect.Fill(dtselect);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 2 !" + ex.Message, "Error ScanUpdateText", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        con.con.Close();
                    }
                    if (dtselect.Rows.Count <= 0)
                    {
                        string soundPath = Path.Combine(Environment.CurrentDirectory, @"Sound\NoCode.wav");
                        SoundPlayer player = new SoundPlayer(soundPath);
                        player.Play();
                        MessageBox.Show("មិនមានឡាប៊ែលប្រភេទនេះទេ !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Cursor = Cursors.Default;
                        return;
                    }
                    //Show the total of all rows of this Label No
                    List<string> codes = new List<string>();
                    List<string> names = new List<string>();
                    double ttlBobbinQty = 0, ttlBobinW = 0, ttlQty = 0, ttlWeight = 0;
                    foreach (DataRow row in dtselect.Rows)
                    {
                        if (!codes.Contains(row["Code"].ToString()))
                            codes.Add(row["Code"].ToString());
                        if (!names.Contains(row["RMName"].ToString()))
                            names.Add(row["RMName"].ToString());
                        ttlBobbinQty += ToDouble(row["BobbinQty"]);
                        ttlBobinW += ToDouble(row["BobinWeight"]);
                        ttlQty += ToDouble(row["Qty"]);
                        ttlWeight += ToDouble(row["TotalWeight"]);
                    }
                    txtcode.Text = string.Join(", ", codes);
                    txtrmname.Text = string.Join(", ", names);
                    txtlabel.Text = dtselect.Rows[0]["LabelNo"].ToString();
                    bobinQty.Text = Math.Round(ttlBobbinQty, 3).ToString();
                    bobinw.Text = Math.Round(ttlBobinW, 3).ToString();
                    txtQty.Text = Math.Round(ttlQty, 3).ToString();
                    txtttlweight.Text = Math.Round(ttlWeight, 3).ToString();
                    //SysNo only when there is one row
                    txtscan.Text = dtselect.Rows.Count == 1 ? dtselect.Rows[0]["SysNo"].ToString() : "";
                    txtlbinput.Enabled = false;
                    txtscan.Enabled = false;
                    deleteByLabel = true;
                    deleteLabelNo = lbNo;
                    deleteRowCount = dtselect.Rows.Count;
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
                        string querySelect = "SELECT * FROM tbSDMCStockInventory WHERE SysNo = '" + scan + "' AND Status = 'Active'";
                        SqlDataAdapter sdaSelect = new SqlDataAdapter(querySelect, con.con);
                        sdaSelect.Fill(dtselect);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("មានបញ្ហាបច្ចេកទេស​ សូមទាក់ទងទៅ​ IT ​(Phanun) 1 !" + ex.Message, "Error ScanUpdateText", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    con.con.Close();
                    if (dtselect.Rows.Count <= 0)
                    {
                        string soundPath = Path.Combine(Environment.CurrentDirectory, @"Sound\WrongSysNo.wav");
                        SoundPlayer player = new SoundPlayer(soundPath);
                        player.Play();
                        MessageBox.Show("មិនមានឡាប៊ែលប្រភេទនេះទេ !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    deleteByLabel = false;
                }
                Cursor = Cursors.Default;
            }
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
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
            deleteByLabel = false;
            deleteLabelNo = 0;
            deleteRowCount = 0;
        }

        private static double ToDouble(object value)
        {
            double d;
            return (value != null && value != DBNull.Value && double.TryParse(value.ToString(), out d)) ? d : 0;
        }
    }
}
