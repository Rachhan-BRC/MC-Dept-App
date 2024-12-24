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

namespace MachineDeptApp.AllSection
{
    public partial class AllSectionMCReceiveForm : Form
    {
        SQLConnectAllSection cnnAll = new SQLConnectAllSection();
        SqlCommand cmd;

        public AllSectionMCReceiveForm()
        {
            InitializeComponent();
            this.cnnAll.Connection();
            this.txtBarcode.KeyDown += TxtBarcode_KeyDown;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnNew.Click += BtnNew_Click;
            this.btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (dgvScannedTag.Rows.Count > 0)
            {
                DialogResult DLR = MessageBox.Show("តើអ្នកចង់រក្សាទុកទិន្នន័យនេះមែនដែរ ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    DataTable dtError = new DataTable();
                    dtError.Columns.Add("POSNo");
                    dtError.Columns.Add("ItemCode");
                    dtError.Columns.Add("BoxNo");

                    //Add to DB
                    try
                    {
                        cnnAll.con.Open();
                        DateTime dateNow = DateTime.Now;
                        string TransNo = "";

                        //Find Last TransNo
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT SysNo FROM tbAllTransaction WHERE RegDate=(SELECT MAX(RegDate) FROM tbAllTransaction WHERE Funct =N'ទទួលចូល') AND " +
                                                            "SysNo=(SELECT MAX(SysNo) FROM tbAllTransaction WHERE Funct =N'ទទួលចូល') Group By SysNo", cnnAll.con);
                        DataTable dtTransNo = new DataTable();
                        sda.Fill(dtTransNo);
                        if (dtTransNo.Rows.Count == 0)
                        {
                            TransNo = "REC0000000001";
                        }
                        else
                        {
                            string LastTransNo = dtTransNo.Rows[0][0].ToString();
                            double NextTransNo = Convert.ToDouble(LastTransNo.Substring(LastTransNo.Length - 10)) + 1;
                            TransNo = "REC" + NextTransNo.ToString("0000000000");
                        }

                        foreach (DataGridViewRow DgvRow in dgvScannedTag.Rows)
                        {
                            //Check Already have or not 
                            SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM tbAllTransaction WHERE Funct =N'ទទួលចូល' AND LocCode='MC1' AND CancelStatus='0' AND " +
                                                                                                "POS_No='" + DgvRow.Cells[0].Value.ToString() + "' AND " +
                                                                                                "ItemCode='" + DgvRow.Cells[1].Value.ToString() + "' AND " +
                                                                                                "BoxNo=" + Convert.ToInt16(DgvRow.Cells[3].Value.ToString()) + " ", cnnAll.con);
                            DataTable dt = new DataTable();
                            sda1.Fill(dt);

                            if (dt.Rows.Count == 0)
                            {
                                //Add to tbAllTransaction
                                using (cmd = new SqlCommand("AddNewtbAllTransaction", cnnAll.con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@Sn ", TransNo);
                                    cmd.Parameters.AddWithValue("@Lc ", "MC1");
                                    cmd.Parameters.AddWithValue("@Fct ", "ទទួលចូល");
                                    cmd.Parameters.AddWithValue("@Pn ", DgvRow.Cells[0].Value.ToString());
                                    cmd.Parameters.AddWithValue("@Ic ", DgvRow.Cells[1].Value.ToString());
                                    cmd.Parameters.AddWithValue("@In ", DgvRow.Cells[2].Value.ToString());
                                    cmd.Parameters.AddWithValue("@Bn ", Convert.ToInt16(DgvRow.Cells[3].Value.ToString()));
                                    cmd.Parameters.AddWithValue("@Rq ", Convert.ToInt32(DgvRow.Cells[4].Value.ToString()));
                                    cmd.Parameters.AddWithValue("@Tq ", 0);
                                    cmd.Parameters.AddWithValue("@Rv ", 0);
                                    cmd.Parameters.AddWithValue("@Sd", Convert.ToDateTime(DgvRow.Cells[5].Value.ToString()));
                                    cmd.Parameters.AddWithValue("@Rd", dateNow);
                                    cmd.Parameters.AddWithValue("@Rb", MenuFormV2.UserForNextForm);
                                    cmd.Parameters.AddWithValue("@Ud", dateNow);
                                    cmd.Parameters.AddWithValue("@Ub ", MenuFormV2.UserForNextForm);
                                    cmd.Parameters.AddWithValue("@Cs", 0);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                dtError.Rows.Add(DgvRow.Cells[0].Value.ToString(), DgvRow.Cells[1].Value.ToString(), DgvRow.Cells[3].Value.ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cnnAll.con.Close();


                    if (dtError.Rows.Count == 0)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("រក្សាទិន្នន័យទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvScannedTag.Rows.Clear();
                        txtBarcode.Focus();

                    }
                    else
                    {
                        for (int i = dgvScannedTag.Rows.Count - 1; i > -1; i--)
                        {
                            int Found = 0;
                            foreach (DataRow row in dtError.Rows)
                            {
                                if (dgvScannedTag.Rows[i].Cells[0].Value.ToString() == row[0].ToString() && dgvScannedTag.Rows[i].Cells[1].Value.ToString() == row[1].ToString() && dgvScannedTag.Rows[i].Cells[3].Value.ToString() == row[2].ToString())
                                {
                                    Found = Found + 1;
                                    break;
                                }
                            }
                            if (Found == 0)
                            {
                                dgvScannedTag.Rows.RemoveAt(i);
                            }
                        }
                        dgvScannedTag.Refresh();
                        Cursor = Cursors.Default;
                        MessageBox.Show("មានបញ្ហា!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            dgvScannedTag.Rows.Clear();
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvScannedTag.SelectedCells.Count > 0)
            {
                if (dgvScannedTag.CurrentCell.RowIndex > -1)
                {
                    DialogResult DLR = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនដែរ ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DLR == DialogResult.Yes)
                    {
                        dgvScannedTag.Rows.RemoveAt(dgvScannedTag.CurrentCell.RowIndex);
                        dgvScannedTag.Refresh();
                        dgvScannedTag.ClearSelection();
                    }
                }
            }
        }

        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.Trim() == "")
                {

                }
                else
                {
                    if (txtBarcode.Text.Length >= 23)
                    {
                        string OriginBarcode = txtBarcode.Text;
                        try
                        {
                            string PosNo = "";
                            string separate = txtBarcode.Text;
                            // Part 1: split on a single character.
                            string[] array = separate.Split('/');
                            PosNo = array[0].Replace(" ", "");
                            string WipCode = array[1].Replace(" ", "");
                            int Qty = Convert.ToInt16(array[2].Replace(" ", ""));
                            int BoxNo = Convert.ToInt16(array[3].Replace(" ", ""));
                            txtBarcode.Text = "";
                            int DupRow = 0;


                            for (int i = 0; i < dgvScannedTag.Rows.Count; i++)
                            {
                                if (dgvScannedTag.Rows[i].Cells[0].Value.ToString().Trim() == PosNo && Convert.ToInt16(dgvScannedTag.Rows[i].Cells[3].Value.ToString().Trim()) == BoxNo)
                                {
                                    DupRow = DupRow + 1;
                                }
                            }

                            if (DupRow > 0)
                            {
                                txtBarcode.Text = OriginBarcode;
                                MessageBox.Show("ឡាប៊ែលនេះស្កេនរួចហើយ​ ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtBarcode.SelectAll();
                                txtBarcode.Focus();
                            }
                            else
                            {
                                if (PosNo.Trim() != "" && WipCode.ToString().Trim() != "" && Qty.ToString().Trim() != "" && BoxNo.ToString().Trim() != "")
                                {
                                    //Check already receive or not
                                    cnnAll.con.Open();
                                    SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbAllTransaction WHERE Funct =N'ទទួលចូល' AND LocCode='MC1' AND CancelStatus='0' AND POS_No='" + PosNo + "' AND ItemCode='" + WipCode + "' AND BoxNo=" + BoxNo + " ", cnnAll.con);
                                    DataTable dt = new DataTable();
                                    sda.Fill(dt);
                                    cnnAll.con.Close();

                                    if (dt.Rows.Count > 0)
                                    {
                                        txtBarcode.Text = OriginBarcode;
                                        MessageBox.Show("ឡាប៊ែលនេះស្កេនរួចហើយ ​! ", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        txtBarcode.SelectAll();
                                        txtBarcode.Focus();
                                    }
                                    else
                                    {
                                        //Check Assy already transfer or not
                                        cnnAll.con.Open();
                                        SqlDataAdapter sda1 = new SqlDataAdapter("SELECT ItemName, TransQty, ShipDate FROM tbAllTransaction WHERE CancelStatus=0 AND LocCode='Assy1' AND Funct=N'វេរចេញ' AND " +
                                                                                                            "POS_No='" + PosNo + "' AND ItemCode='" + WipCode + "' AND BoxNo=" + BoxNo, cnnAll.con);
                                        DataTable dtDetails = new DataTable();
                                        sda1.Fill(dtDetails);
                                        if (dtDetails.Rows.Count > 0)
                                        {
                                            string Itemname = dtDetails.Rows[0][0].ToString();
                                            Qty = Convert.ToInt32(dtDetails.Rows[0][1].ToString());
                                            DateTime ShipDate = Convert.ToDateTime(dtDetails.Rows[0][2].ToString());
                                            dgvScannedTag.Rows.Add(PosNo, WipCode, Itemname, BoxNo, Qty, ShipDate);
                                            dgvScannedTag.ClearSelection();
                                        }
                                        else
                                        {
                                            txtBarcode.Text = OriginBarcode;
                                            MessageBox.Show("MC1 មិនទាន់វេរទិន្នន័យនេះនៅក្នុងប្រព័ន្ធទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            txtBarcode.SelectAll();
                                            txtBarcode.Focus();
                                        }
                                        cnnAll.con.Close();
                                    }
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
    }
}
