using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Common;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.SemiPress
{
    public partial class SemiPressSearchForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        System.Data.DataTable dtSemi1;
        System.Data.DataTable dtSemi2;
        System.Data.DataTable dtFinal;
        public static string POSNoSelectedForNextForm;
        public static string CodeSelectedForNextForm;
        public static string ItemsSelectedForNextForm;
        public static string QtySelectedForNextForm;

        public SemiPressSearchForm()
        {
            InitializeComponent();
            cnn.Connection();
            this.dgvSemiPressSearch.CellDoubleClick += DgvSemiPressSearch_CellDoubleClick;
            
        }

        private void DgvSemiPressSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dgvSemiPressSearch.Rows[e.RowIndex];
            POSNoSelectedForNextForm = row.Cells[0].Value.ToString();
            CodeSelectedForNextForm = row.Cells[1].Value.ToString();
            ItemsSelectedForNextForm = row.Cells[2].Value.ToString();
            QtySelectedForNextForm = row.Cells[3].Value.ToString();
            SemiPressSearchDetailsForm Spsdf = new SemiPressSearchDetailsForm(this);
            Spsdf.ShowDialog();
        }

        private void SemiPressSearchForm_Load(object sender, EventArgs e)
        {
            CboStatus.Items.Add("នៅសល់");
            CboStatus.Items.Add("រួចរាល់");
            CboStatus.Items.Add("POS រួចរាល់");
            CboStatus.SelectedIndex = 0;

            dtFinal = new System.Data.DataTable();
            dtFinal.Columns.Add("POSNo");
            dtFinal.Columns.Add("WIPCode");
            dtFinal.Columns.Add("WIPName");
            dtFinal.Columns.Add("POSQty");
            dtFinal.Columns.Add("POSRemainQty");
            dtFinal.Columns.Add("AutoQty");
            dtFinal.Columns.Add("SemiPressQty");
            dtFinal.Columns.Add("AutoRemainQty");
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dgvSemiPressSearch.Rows.Clear();
            dtFinal.Rows.Clear();
            LbExportStatus.Visible = true;
            LbExportStatus.Text = "កំពុងស្វែងរក . . . . .";
            LbExportStatus.Refresh();

            if (txtWipDes.Text.ToString().Trim() != "")
            {
                if (CboStatus.Text == "នៅសល់")
                {
                    try
                    {
                        cnn.con.Open();
                        //Search for SemiPress1
                        SqlDataAdapter sda = new SqlDataAdapter("Select * From (Select ItemCode, ItemName From tbMasterItem Where LEN(ItemCode) > 4   AND ItemName Like '%"+txtWipDes.Text+"%') t1 " +
                                                                                            "INNER JOIN (SELECT PosNo, WipCode, SUM(convert(decimal(9, 0), Qty)) as TotalQty FROM tbSemiPress WHERE DeleteState = '0' GROUP BY PosNo, WipCode)t2 ON t1.ItemCode = t2.WipCode", cnn.con);
                        dtSemi1 = new System.Data.DataTable();
                        sda.Fill(dtSemi1);
                        SqlDataAdapter sda1 = new SqlDataAdapter("SELECT PosNo, WipCode, SUM(convert(decimal(9, 0), Qty)) as TotalQty FROM tbSemiPress2 WHERE DeleteState = '0' GROUP BY PosNo, WipCode", cnn.con);
                        dtSemi2 = new System.Data.DataTable();
                        sda1.Fill(dtSemi2);
                        cnn.con.Close();

                        for (int i = 0; i < dtSemi1.Rows.Count; i++)
                        {
                            //Take tbSemiPress Qty
                            string POSNo = dtSemi1.Rows[i][2].ToString();
                            string WIPCode = dtSemi1.Rows[i][0].ToString();
                            string WIPName = dtSemi1.Rows[i][1].ToString();
                            double POSQty = 0;
                            double AutoQty = Convert.ToDouble(dtSemi1.Rows[i][4].ToString());

                            //Check tbSemiPress2 Qty
                            double SemiPress = 0;
                            for (int j = 0; j < dtSemi2.Rows.Count; j++)
                            {
                                if (POSNo == dtSemi2.Rows[j][0].ToString() && WIPCode == dtSemi2.Rows[j][1].ToString())
                                {
                                    SemiPress = Convert.ToDouble(dtSemi2.Rows[j][2].ToString());
                                }
                            }
                            //Calculate AutoRemain
                            double AutoRemain = Convert.ToDouble(dtSemi1.Rows[i][4].ToString()) - SemiPress;

                            //Add if have remain for Auto
                            if (AutoRemain > 0)
                            {
                                //Take POS Qty
                                cnn.con.Open();
                                SqlCommand cmd = new SqlCommand("SELECT * FROM tbPOSData WHERE POSNo ='" + POSNo + "' AND WipCode='" + WIPCode + "';", cnn.con);
                                SqlDataReader dr = cmd.ExecuteReader();
                                if (dr.Read())
                                {
                                    POSQty = Convert.ToDouble(dr.GetValue(2).ToString());
                                }
                                cnn.con.Close();
                                dtFinal.Rows.Add(POSNo, WIPCode, WIPName, POSQty, (POSQty - SemiPress), AutoQty, SemiPress, AutoRemain);

                            }
                        }
                        for (int i = 0; i < dtFinal.Rows.Count; i++)
                        {
                            string POSNo = dtFinal.Rows[i][0].ToString();
                            string WIPCode = dtFinal.Rows[i][1].ToString();
                            string WIPName = dtFinal.Rows[i][2].ToString();
                            double POSQty = Convert.ToDouble(dtFinal.Rows[i][3].ToString());
                            double AutoQty = Convert.ToDouble(dtFinal.Rows[i][4].ToString());
                            double SemiPress = Convert.ToDouble(dtFinal.Rows[i][5].ToString());
                            double AutoRemain = Convert.ToDouble(dtFinal.Rows[i][6].ToString());
                            double POSRemain = Convert.ToDouble(dtFinal.Rows[i][7].ToString());

                            dgvSemiPressSearch.Rows.Add(POSNo, WIPCode, WIPName, POSQty, AutoQty, SemiPress, AutoRemain, POSRemain);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (CboStatus.Text == "រួចរាល់")
                {
                    try
                    {
                        cnn.con.Open();
                        //Search for SemiPress1
                        SqlDataAdapter sda = new SqlDataAdapter("Select * From (Select ItemCode, ItemName From tbMasterItem Where LEN(ItemCode) > 4  AND ItemName Like '%"+txtWipDes.Text+"%') t1 " +
                                                                                            "INNER JOIN (SELECT PosNo, WipCode, SUM(convert(decimal(9, 0), Qty)) as TotalQty FROM tbSemiPress WHERE DeleteState = '0' GROUP BY PosNo, WipCode)t2 ON t1.ItemCode = t2.WipCode", cnn.con);
                        dtSemi1 = new System.Data.DataTable();
                        sda.Fill(dtSemi1);
                        SqlDataAdapter sda1 = new SqlDataAdapter("SELECT PosNo, WipCode, SUM(convert(decimal(9, 0), Qty)) as TotalQty FROM tbSemiPress2 WHERE DeleteState = '0' GROUP BY PosNo, WipCode", cnn.con);
                        dtSemi2 = new System.Data.DataTable();
                        sda1.Fill(dtSemi2);
                        cnn.con.Close();

                        for (int i = 0; i < dtSemi1.Rows.Count; i++)
                        {
                            //Take tbSemiPress Qty
                            string POSNo = dtSemi1.Rows[i][2].ToString();
                            string WIPCode = dtSemi1.Rows[i][0].ToString();
                            string WIPName = dtSemi1.Rows[i][1].ToString();
                            double POSQty = 0;
                            double AutoQty = Convert.ToDouble(dtSemi1.Rows[i][4].ToString());

                            //Check tbSemiPress2 Qty
                            double SemiPress = 0;
                            for (int j = 0; j < dtSemi2.Rows.Count; j++)
                            {
                                if (POSNo == dtSemi2.Rows[j][0].ToString() && WIPCode == dtSemi2.Rows[j][1].ToString())
                                {
                                    SemiPress = Convert.ToDouble(dtSemi2.Rows[j][2].ToString());
                                }
                            }
                            //Calculate AutoRemain
                            double AutoRemain = Convert.ToDouble(dtSemi1.Rows[i][4].ToString()) - SemiPress;

                            //Add if have remain for Auto
                            if (AutoRemain == 0)
                            {
                                //Take POS Qty
                                cnn.con.Open();
                                SqlCommand cmd = new SqlCommand("SELECT * FROM tbPOSData WHERE POSNo ='" + POSNo + "' AND WipCode='" + WIPCode + "';", cnn.con);
                                SqlDataReader dr = cmd.ExecuteReader();
                                if (dr.Read())
                                {
                                    POSQty = Convert.ToDouble(dr.GetValue(2).ToString());
                                }
                                cnn.con.Close();
                                if (POSQty > SemiPress)
                                {
                                    dtFinal.Rows.Add(POSNo, WIPCode, WIPName, POSQty, (POSQty - SemiPress), AutoQty, SemiPress, AutoRemain);

                                }
                            }
                        }
                        for (int i = 0; i < dtFinal.Rows.Count; i++)
                        {
                            string POSNo = dtFinal.Rows[i][0].ToString();
                            string WIPCode = dtFinal.Rows[i][1].ToString();
                            string WIPName = dtFinal.Rows[i][2].ToString();
                            double POSQty = Convert.ToDouble(dtFinal.Rows[i][3].ToString());
                            double AutoQty = Convert.ToDouble(dtFinal.Rows[i][4].ToString());
                            double SemiPress = Convert.ToDouble(dtFinal.Rows[i][5].ToString());
                            double AutoRemain = Convert.ToDouble(dtFinal.Rows[i][6].ToString());
                            double POSRemain = Convert.ToDouble(dtFinal.Rows[i][7].ToString());

                            dgvSemiPressSearch.Rows.Add(POSNo, WIPCode, WIPName, POSQty, AutoQty, SemiPress, AutoRemain, POSRemain);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (CboStatus.Text == "POS រួចរាល់")
                {
                    try
                    {
                        cnn.con.Open();
                        //Search for SemiPress1
                        SqlDataAdapter sda = new SqlDataAdapter("Select * From (Select ItemCode, ItemName From tbMasterItem Where LEN(ItemCode) > 4  AND ItemName Like '%"+txtWipDes.Text+"%') t1 " +
                                                                                            "INNER JOIN (SELECT PosNo, WipCode, SUM(convert(decimal(9, 0), Qty)) as TotalQty FROM tbSemiPress WHERE DeleteState = '0' GROUP BY PosNo, WipCode)t2 ON t1.ItemCode = t2.WipCode", cnn.con);
                        dtSemi1 = new System.Data.DataTable();
                        sda.Fill(dtSemi1);
                        SqlDataAdapter sda1 = new SqlDataAdapter("SELECT PosNo, WipCode, SUM(convert(decimal(9, 0), Qty)) as TotalQty FROM tbSemiPress2 WHERE DeleteState = '0' GROUP BY PosNo, WipCode", cnn.con);
                        dtSemi2 = new System.Data.DataTable();
                        sda1.Fill(dtSemi2);
                        cnn.con.Close();

                        for (int i = 0; i < dtSemi1.Rows.Count; i++)
                        {
                            //Take tbSemiPress Qty
                            string POSNo = dtSemi1.Rows[i][2].ToString();
                            string WIPCode = dtSemi1.Rows[i][0].ToString();
                            string WIPName = dtSemi1.Rows[i][1].ToString();
                            double POSQty = 0;
                            double AutoQty = Convert.ToDouble(dtSemi1.Rows[i][4].ToString());

                            //Check tbSemiPress2 Qty
                            double SemiPress = 0;
                            for (int j = 0; j < dtSemi2.Rows.Count; j++)
                            {
                                if (POSNo == dtSemi2.Rows[j][0].ToString() && WIPCode == dtSemi2.Rows[j][1].ToString())
                                {
                                    SemiPress = Convert.ToDouble(dtSemi2.Rows[j][2].ToString());
                                }
                            }
                            //Calculate AutoRemain
                            double AutoRemain = Convert.ToDouble(dtSemi1.Rows[i][4].ToString()) - SemiPress;

                            //Add if have remain for Auto
                            if (AutoRemain == 0)
                            {
                                //Take POS Qty
                                cnn.con.Open();
                                SqlCommand cmd = new SqlCommand("SELECT * FROM tbPOSData WHERE POSNo ='" + POSNo + "' AND WipCode='" + WIPCode + "';", cnn.con);
                                SqlDataReader dr = cmd.ExecuteReader();
                                if (dr.Read())
                                {
                                    POSQty = Convert.ToDouble(dr.GetValue(2).ToString());
                                }
                                cnn.con.Close();

                                if (POSQty == SemiPress)
                                {
                                    dtFinal.Rows.Add(POSNo, WIPCode, WIPName, POSQty, (POSQty - SemiPress), AutoQty, SemiPress, AutoRemain);

                                }
                            }
                        }
                        for (int i = 0; i < dtFinal.Rows.Count; i++)
                        {
                            string POSNo = dtFinal.Rows[i][0].ToString();
                            string WIPCode = dtFinal.Rows[i][1].ToString();
                            string WIPName = dtFinal.Rows[i][2].ToString();
                            double POSQty = Convert.ToDouble(dtFinal.Rows[i][3].ToString());
                            double AutoQty = Convert.ToDouble(dtFinal.Rows[i][4].ToString());
                            double SemiPress = Convert.ToDouble(dtFinal.Rows[i][5].ToString());
                            double AutoRemain = Convert.ToDouble(dtFinal.Rows[i][6].ToString());
                            double POSRemain = Convert.ToDouble(dtFinal.Rows[i][7].ToString());

                            dgvSemiPressSearch.Rows.Add(POSNo, WIPCode, WIPName, POSQty, AutoQty, SemiPress, AutoRemain, POSRemain);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {

                }
            }
            else if (txtWipDes.Text.ToString().Trim() == "")
            {
                if (CboStatus.Text == "នៅសល់")
                {
                    try
                    {
                        cnn.con.Open();
                        //Search for SemiPress1
                        SqlDataAdapter sda = new SqlDataAdapter("Select * From (Select ItemCode, ItemName From tbMasterItem Where LEN(ItemCode) > 4) t1 " +
                                                                                            "INNER JOIN (SELECT PosNo, WipCode, SUM(convert(decimal(9, 0), Qty)) as TotalQty FROM tbSemiPress WHERE DeleteState = '0' GROUP BY PosNo, WipCode)t2 ON t1.ItemCode = t2.WipCode", cnn.con);
                        dtSemi1 = new System.Data.DataTable();
                        sda.Fill(dtSemi1);
                        SqlDataAdapter sda1 = new SqlDataAdapter("SELECT PosNo, WipCode, SUM(convert(decimal(9, 0), Qty)) as TotalQty FROM tbSemiPress2 WHERE DeleteState = '0' GROUP BY PosNo, WipCode", cnn.con);
                        dtSemi2 = new System.Data.DataTable();
                        sda1.Fill(dtSemi2);
                        cnn.con.Close();

                        for (int i = 0; i < dtSemi1.Rows.Count; i++)
                        {
                            //Take tbSemiPress Qty
                            string POSNo = dtSemi1.Rows[i][2].ToString();
                            string WIPCode = dtSemi1.Rows[i][0].ToString();
                            string WIPName = dtSemi1.Rows[i][1].ToString();
                            double POSQty = 0;
                            double AutoQty = Convert.ToDouble(dtSemi1.Rows[i][4].ToString());

                            //Check tbSemiPress2 Qty
                            double SemiPress = 0;
                            for (int j = 0; j < dtSemi2.Rows.Count; j++)
                            {
                                if (POSNo == dtSemi2.Rows[j][0].ToString() && WIPCode == dtSemi2.Rows[j][1].ToString())
                                {
                                    SemiPress = Convert.ToDouble(dtSemi2.Rows[j][2].ToString());
                                }
                            }
                            //Calculate AutoRemain
                            double AutoRemain = Convert.ToDouble(dtSemi1.Rows[i][4].ToString()) - SemiPress;

                            //Add if have remain for Auto
                            if (AutoRemain > 0)
                            {
                                //Take POS Qty
                                cnn.con.Open();
                                SqlCommand cmd = new SqlCommand("SELECT * FROM tbPOSData WHERE POSNo ='" + POSNo + "' AND WipCode='" + WIPCode + "';", cnn.con);
                                SqlDataReader dr = cmd.ExecuteReader();
                                if (dr.Read())
                                {
                                    POSQty = Convert.ToDouble(dr.GetValue(2).ToString());
                                }
                                cnn.con.Close();
                                dtFinal.Rows.Add(POSNo, WIPCode, WIPName, POSQty, (POSQty - SemiPress), AutoQty, SemiPress, AutoRemain);

                            }
                        }
                        for (int i = 0; i < dtFinal.Rows.Count; i++)
                        {
                            string POSNo = dtFinal.Rows[i][0].ToString();
                            string WIPCode = dtFinal.Rows[i][1].ToString();
                            string WIPName = dtFinal.Rows[i][2].ToString();
                            double POSQty = Convert.ToDouble(dtFinal.Rows[i][3].ToString());
                            double AutoQty = Convert.ToDouble(dtFinal.Rows[i][4].ToString());
                            double SemiPress = Convert.ToDouble(dtFinal.Rows[i][5].ToString());
                            double AutoRemain = Convert.ToDouble(dtFinal.Rows[i][6].ToString());
                            double POSRemain = Convert.ToDouble(dtFinal.Rows[i][7].ToString());

                            dgvSemiPressSearch.Rows.Add(POSNo, WIPCode, WIPName, POSQty, AutoQty, SemiPress, AutoRemain, POSRemain);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (CboStatus.Text == "រួចរាល់")
                {
                    try
                    {
                        cnn.con.Open();
                        //Search for SemiPress1
                        SqlDataAdapter sda = new SqlDataAdapter("Select * From (Select ItemCode, ItemName From tbMasterItem Where LEN(ItemCode) > 4) t1 " +
                                                                                            "INNER JOIN (SELECT PosNo, WipCode, SUM(convert(decimal(9, 0), Qty)) as TotalQty FROM tbSemiPress WHERE DeleteState = '0' GROUP BY PosNo, WipCode)t2 ON t1.ItemCode = t2.WipCode", cnn.con);
                        dtSemi1 = new System.Data.DataTable();
                        sda.Fill(dtSemi1);
                        SqlDataAdapter sda1 = new SqlDataAdapter("SELECT PosNo, WipCode, SUM(convert(decimal(9, 0), Qty)) as TotalQty FROM tbSemiPress2 WHERE DeleteState = '0' GROUP BY PosNo, WipCode", cnn.con);
                        dtSemi2 = new System.Data.DataTable();
                        sda1.Fill(dtSemi2);
                        cnn.con.Close();

                        for (int i = 0; i < dtSemi1.Rows.Count; i++)
                        {
                            //Take tbSemiPress Qty
                            string POSNo = dtSemi1.Rows[i][2].ToString();
                            string WIPCode = dtSemi1.Rows[i][0].ToString();
                            string WIPName = dtSemi1.Rows[i][1].ToString();
                            double POSQty = 0;
                            double AutoQty = Convert.ToDouble(dtSemi1.Rows[i][4].ToString());

                            //Check tbSemiPress2 Qty
                            double SemiPress = 0;
                            for (int j = 0; j < dtSemi2.Rows.Count; j++)
                            {
                                if (POSNo == dtSemi2.Rows[j][0].ToString() && WIPCode == dtSemi2.Rows[j][1].ToString())
                                {
                                    SemiPress = Convert.ToDouble(dtSemi2.Rows[j][2].ToString());
                                }
                            }
                            //Calculate AutoRemain
                            double AutoRemain = Convert.ToDouble(dtSemi1.Rows[i][4].ToString()) - SemiPress;

                            //Add if have remain for Auto
                            if (AutoRemain == 0)
                            {
                                //Take POS Qty
                                cnn.con.Open();
                                SqlCommand cmd = new SqlCommand("SELECT * FROM tbPOSData WHERE POSNo ='" + POSNo + "' AND WipCode='" + WIPCode + "';", cnn.con);
                                SqlDataReader dr = cmd.ExecuteReader();
                                if (dr.Read())
                                {
                                    POSQty = Convert.ToDouble(dr.GetValue(2).ToString());
                                }
                                cnn.con.Close();
                                if (POSQty > SemiPress)
                                {
                                    dtFinal.Rows.Add(POSNo, WIPCode, WIPName, POSQty, (POSQty - SemiPress), AutoQty, SemiPress, AutoRemain);

                                }
                            }
                        }
                        for (int i = 0; i < dtFinal.Rows.Count; i++)
                        {
                            string POSNo = dtFinal.Rows[i][0].ToString();
                            string WIPCode = dtFinal.Rows[i][1].ToString();
                            string WIPName = dtFinal.Rows[i][2].ToString();
                            double POSQty = Convert.ToDouble(dtFinal.Rows[i][3].ToString());
                            double AutoQty = Convert.ToDouble(dtFinal.Rows[i][4].ToString());
                            double SemiPress = Convert.ToDouble(dtFinal.Rows[i][5].ToString());
                            double AutoRemain = Convert.ToDouble(dtFinal.Rows[i][6].ToString());
                            double POSRemain = Convert.ToDouble(dtFinal.Rows[i][7].ToString());

                            dgvSemiPressSearch.Rows.Add(POSNo, WIPCode, WIPName, POSQty, AutoQty, SemiPress, AutoRemain, POSRemain);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (CboStatus.Text == "POS រួចរាល់")
                {
                    try
                    {
                        cnn.con.Open();
                        //Search for SemiPress1
                        SqlDataAdapter sda = new SqlDataAdapter("Select * From (Select ItemCode, ItemName From tbMasterItem Where LEN(ItemCode) > 4) t1 " +
                                                                                            "INNER JOIN (SELECT PosNo, WipCode, SUM(convert(decimal(9, 0), Qty)) as TotalQty FROM tbSemiPress WHERE DeleteState = '0' GROUP BY PosNo, WipCode)t2 ON t1.ItemCode = t2.WipCode", cnn.con);
                        dtSemi1 = new System.Data.DataTable();
                        sda.Fill(dtSemi1);
                        SqlDataAdapter sda1 = new SqlDataAdapter("SELECT PosNo, WipCode, SUM(convert(decimal(9, 0), Qty)) as TotalQty FROM tbSemiPress2 WHERE DeleteState = '0' GROUP BY PosNo, WipCode", cnn.con);
                        dtSemi2 = new System.Data.DataTable();
                        sda1.Fill(dtSemi2);
                        cnn.con.Close();

                        for (int i = 0; i < dtSemi1.Rows.Count; i++)
                        {
                            //Take tbSemiPress Qty
                            string POSNo = dtSemi1.Rows[i][2].ToString();
                            string WIPCode = dtSemi1.Rows[i][0].ToString();
                            string WIPName = dtSemi1.Rows[i][1].ToString();
                            double POSQty = 0;
                            double AutoQty = Convert.ToDouble(dtSemi1.Rows[i][4].ToString());

                            //Check tbSemiPress2 Qty
                            double SemiPress = 0;
                            for (int j = 0; j < dtSemi2.Rows.Count; j++)
                            {
                                if (POSNo == dtSemi2.Rows[j][0].ToString() && WIPCode == dtSemi2.Rows[j][1].ToString())
                                {
                                    SemiPress = Convert.ToDouble(dtSemi2.Rows[j][2].ToString());
                                }
                            }
                            //Calculate AutoRemain
                            double AutoRemain = Convert.ToDouble(dtSemi1.Rows[i][4].ToString()) - SemiPress;

                            //Add if have remain for Auto
                            if (AutoRemain == 0)
                            {
                                //Take POS Qty
                                cnn.con.Open();
                                SqlCommand cmd = new SqlCommand("SELECT * FROM tbPOSData WHERE POSNo ='" + POSNo + "' AND WipCode='" + WIPCode + "';", cnn.con);
                                SqlDataReader dr = cmd.ExecuteReader();
                                if (dr.Read())
                                {
                                    POSQty = Convert.ToDouble(dr.GetValue(2).ToString());
                                }
                                cnn.con.Close();

                                if (POSQty == SemiPress)
                                {
                                    dtFinal.Rows.Add(POSNo, WIPCode, WIPName, POSQty, (POSQty - SemiPress), AutoQty, SemiPress, AutoRemain);

                                }
                            }
                        }
                        for (int i = 0; i < dtFinal.Rows.Count; i++)
                        {
                            string POSNo = dtFinal.Rows[i][0].ToString();
                            string WIPCode = dtFinal.Rows[i][1].ToString();
                            string WIPName = dtFinal.Rows[i][2].ToString();
                            double POSQty = Convert.ToDouble(dtFinal.Rows[i][3].ToString());
                            double AutoQty = Convert.ToDouble(dtFinal.Rows[i][4].ToString());
                            double SemiPress = Convert.ToDouble(dtFinal.Rows[i][5].ToString());
                            double AutoRemain = Convert.ToDouble(dtFinal.Rows[i][6].ToString());
                            double POSRemain = Convert.ToDouble(dtFinal.Rows[i][7].ToString());

                            dgvSemiPressSearch.Rows.Add(POSNo, WIPCode, WIPName, POSQty, AutoQty, SemiPress, AutoRemain, POSRemain);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {

                }
            }
            if (dgvSemiPressSearch.Rows.Count > 0)
            {
                LbExportStatus.Text = "ស្វែងរកឃើញ " + dgvSemiPressSearch.Rows.Count.ToString("N0") + " POS !";
            }
            else
            {
                LbExportStatus.Text = "ស្វែងរកឃើញមិនឃើញលក្ខខ័ណ្ឌដែលអ្នកស្វែងរកទេ !";
            }
        }

        private void btnReprint_Click(object sender, EventArgs e)
        {
            if (dgvSemiPressSearch.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("តើអ្នកចង់ទាញទិន្នន័យចេញជា Excel មែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    //Export to excel
                    //open excel application and create new workbook
                    Excel.Application app = new Excel.Application();
                    Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
                    Excel.Worksheet worksheet = null;

                    //hide new excel application and give a sheet name
                    app.Visible = false;
                    worksheet = workbook.Sheets["Sheet1"];
                    worksheet = workbook.ActiveSheet;
                    worksheet.Name = "Rachhan System";

                    try
                    {
                        //Add Header
                        for (int i = 0; i < dgvSemiPressSearch.Columns.Count; i++)
                        {
                            string ColN = dgvSemiPressSearch.Columns[i].HeaderText.ToString();
                            worksheet.Cells[1, 1 + i] = ColN;
                        }
                        for (int i = 0; i < dgvSemiPressSearch.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i + 1].Font.Name = "Khmer OS Battambang";
                            worksheet.Cells[1, i + 1].Font.Size = 12;
                            worksheet.Cells[1, i + 1].Font.Bold = true;
                        }

                        //Add list
                        for (int i = 0; i < dgvSemiPressSearch.Rows.Count; i++)
                        {
                            for (int j = 0; j < dgvSemiPressSearch.Columns.Count; j++)
                            {
                                if (dgvSemiPressSearch.Rows[i].Cells[j].Value != null)
                                {
                                    worksheet.Cells[i + 2, 1 + j] = dgvSemiPressSearch.Rows[i].Cells[j].Value.ToString();
                                }
                                else
                                {
                                    worksheet.Cells[i + 2, 1 + j] = "";
                                }
                            }
                        }

                        int rows = dgvSemiPressSearch.Rows.Count + 1;
                        worksheet.Range["A1:H" + rows].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        worksheet.Range["A2:H" + rows].Font.Name = "Khmer OS Battambang";
                        worksheet.Range["A2:H" + rows].Font.Size = 11;
                        worksheet.Range["A2:H" + rows].Font.Bold = false;

                        //Set Column Fit
                        for (int i = 0; i < dgvSemiPressSearch.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i + 1].EntireColumn.AutoFit();

                        }

                        //Getting the location and file name of the excel to save from user. 
                        SaveFileDialog saveDialog = new SaveFileDialog();
                        saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                        saveDialog.FilterIndex = 1;

                        if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            string saveDir = saveDialog.FileName;
                            app.DisplayAlerts = false;
                            workbook.SaveAs(saveDialog.FileName, Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, Type.Missing, false, Excel.XlSaveAsAccessMode.xlExclusive, Excel.XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
                            workbook.Close();
                            app.DisplayAlerts = true;
                            app.Quit();

                            //Kill all Excel background process
                            var processes = from p in Process.GetProcessesByName("EXCEL")
                                            select p;
                            foreach (var process in processes)
                            {
                                if (process.MainWindowTitle.ToString().Trim() == "")
                                    process.Kill();
                            }

                            MessageBox.Show("ទាញទិន្នន័យចេញបានជោគជ័យ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            System.Diagnostics.Process.Start(saveDir.ToString());

                        }
                        else
                        {
                            //don't save and close workbook that just created
                            app.DisplayAlerts = false;
                            workbook.Close();
                            app.DisplayAlerts = true;
                            app.Quit();

                        }
                    }
                    catch
                    {
                        app.DisplayAlerts = false;
                        workbook.Close();
                        app.DisplayAlerts = true;
                        app.Quit();

                    }
                }
            }
        }
    }
}
