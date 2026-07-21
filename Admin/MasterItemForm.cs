using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MachineDeptApp.Admin
{
    public partial class MasterItemForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SqlCommand cmd;

        public MasterItemForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.btnSearch.Click += BtnSearch_Click;
            this.Load += MasterItemForm_Load;
            this.dgvSearchResult.CellClick += DgvSearchResult_CellClick;
            this.dgvSearchResult.CellFormatting += DgvSearchResult_CellFormatting;
            this.dgvSearchResult.CellValueChanged += DgvSearchResult_CellValueChanged;
            this.dgvMCType.LostFocus += DgvMCType_LostFocus;
            this.dgvMCType.CellClick += DgvMCType_CellClick;
            this.DgvSlot.LostFocus += DgvSlot_LostFocus;
            this.DgvSlot.CellClick += DgvSlot_CellClick;
            this.dgvStatus.LostFocus += DgvStatus_LostFocus;
            this.dgvStatus.CellClick += DgvStatus_CellClick;
            this.cbmctype.TextChanged += Cbmctype_TextChanged;
            this.btnExport.Click += BtnExport_Click;
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            DialogResult DLS = MessageBox.Show("តើអ្នកចង់ទាញទិន្នន័យចេញមែន ឬទេ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DLS == DialogResult.Yes)
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV file (*.csv)|*.csv";
                saveDialog.FileName = "MasterItem.csv";
                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;
                    try
                    {
                        //Write Column name
                        int columnCount = 0;
                        foreach (DataGridViewColumn DgvCol in dgvSearchResult.Columns)
                        {
                            if (DgvCol.Visible == true)
                            {
                                columnCount = columnCount + 1;
                            }
                        }
                        string columnNames = "";

                        //String array for Csv
                        string[] outputCsv;
                        outputCsv = new string[dgvSearchResult.Rows.Count + 1];

                        //Set Column Name
                        for (int i = 0; i < columnCount; i++)
                        {
                            if (dgvSearchResult.Columns[i].Visible == true)
                            {
                                columnNames += dgvSearchResult.Columns[i].HeaderText.ToString() + ",";
                            }
                        }
                        outputCsv[0] += columnNames;

                        //Row of data 
                        for (int i = 1; (i - 1) < dgvSearchResult.Rows.Count; i++)
                        {
                            for (int j = 0; j < columnCount; j++)
                            {
                                if (dgvSearchResult.Columns[j].Visible == true)
                                {
                                    string Value = "";
                                    if (dgvSearchResult.Rows[i - 1].Cells[j].Value != null)
                                    {
                                        Value = dgvSearchResult.Rows[i - 1].Cells[j].Value.ToString();
                                    }
                                    //Fix don't separate if it contain '\n' or ','
                                    Value = "\"" + Value.Replace("\"", "\"\"") + "\"";
                                    outputCsv[i] += Value + ",";
                                }
                            }
                        }

                        File.WriteAllLines(saveDialog.FileName, outputCsv, Encoding.UTF8);
                        Cursor = Cursors.Default;
                        MessageBox.Show("ទាញទិន្នន័យចេញរួចរាល់!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Something went wrong !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Cbmctype_TextChanged(object sender, EventArgs e)
        {
            cbmcname.Items.Clear();
            if (cbmctype.Text.Trim() != "All")
            {
                string type ="";
                if (cbmctype.SelectedIndex == 1)
                {
                    type = "MC1Type";
                }
                else if (cbmctype.SelectedIndex == 2)
                {
                    type = "MC2Type";
                }
                else if (cbmctype.SelectedIndex == 3)
                {
                    type = "MC3Type";
                }
                string query = "SELECT "+type+" FROM tbMasterItemPlan GROUP BY "+type+"";
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(query,cnn.con);
                cbmcname.Items.Add("");
                sda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    string name = dr[0] == DBNull.Value ? null : dr[0].ToString();
                    if (name != null && name!="")
                    {
                        cbmcname.Items.Add(dr[0]);
                    }
                   
                }
            }
        }

        private void Cbmctype_SelectedIndexChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DgvStatus_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string cell = dgvStatus.Rows[e.RowIndex].Cells[0].Value?.ToString()??"";
            if (!string.IsNullOrEmpty(cell))
            {
                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = dgvStatus.Rows[e.RowIndex].Cells[0].Value.ToString();

            }
            else
            {
                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = "";
            }

            dgvSearchResult.Focus();
        }

        private void DgvStatus_LostFocus(object sender, EventArgs e)
        {
            this.dgvStatus.SendToBack();
        }

        private void DgvSlot_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvSlot.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() != "")
            {
                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = DgvSlot.Rows[e.RowIndex].Cells[0].Value.ToString();

            }
            else
            {
                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = null;
            }

            dgvSearchResult.Focus();
        }

        private void DgvSlot_LostFocus(object sender, EventArgs e)
        {
            this.DgvSlot.SendToBack();
        }

        private void DgvSearchResult_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string ItemCode = "";
            ItemCode = dgvSearchResult.Rows[e.RowIndex].Cells[0].Value.ToString();
            DataTable dt = new DataTable();

            //Find Already have or not yet
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode FROM tbMasterItemPlan WHERE ItemCode = '" + ItemCode + "' ", cnn.con);
                sda.Fill(dt);                
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

            try
            {
                cnn.con.Open();
                if (e.ColumnIndex > dgvSearchResult.Columns.Count - 6 && e.ColumnIndex < dgvSearchResult.Columns.Count - 2)
                {
                    //Update becuz it already have
                    if (dt.Rows.Count > 0)
                    {
                        string query = "";
                        if (dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                        {
                            query = "UPDATE tbMasterItemPlan SET " +
                                            "MC" + (-7 + e.ColumnIndex + 1) + "Type='" + dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() + "' " +
                                            "WHERE ItemCode = '" + ItemCode + "' ;";
                        }
                        else
                        {
                            query = "UPDATE tbMasterItemPlan SET " +
                                            "MC" + (-7 + e.ColumnIndex + 1) + "Type=NULL " +
                                            "WHERE ItemCode = '" + ItemCode + "' ;";
                        }
                        SqlCommand cmd = new SqlCommand(query, cnn.con);
                        cmd.ExecuteNonQuery();
                    }
                    //Add becuz it not yet have
                    else
                    {
                        if (dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                        {
                            cmd = new SqlCommand("INSERT INTO tbMasterItemPlan (ItemCode, MC" + (-7 + e.ColumnIndex + 1) + "Type) " +
                                                                            "VALUES (@Ic, @McT)", cnn.con);
                            cmd.Parameters.AddWithValue("@Ic", ItemCode);
                            cmd.Parameters.AddWithValue("@McT", dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else if (e.ColumnIndex == dgvSearchResult.Columns.Count - 2)
                {
                    if (dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        DataTable dtstatus = new DataTable();
                        string wipcode = dgvSearchResult.Rows[e.RowIndex].Cells[0].Value.ToString();
                        string query = "SELECT * FROM tbMasterItemStatus WHERE WipCode = '" + wipcode + "'";
                        SqlDataAdapter sda = new SqlDataAdapter(query, cnn.con);
                        sda.Fill(dtstatus);

                        if (dtstatus.Rows.Count > 0)
                        {
                            string queryupdate = "UPDATE tbMasterItemStatus SET Status = @status WHERE WipCode = @wipcode";
                            SqlCommand cmd = new SqlCommand(queryupdate, cnn.con);
                            cmd.Parameters.AddWithValue("@status", dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString()??"");
                            cmd.Parameters.AddWithValue("@wipcode",wipcode);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            string queryinsert = "INSERT INTO tbMasterItemStatus (Status, WipCode) VALUES (@status, @wipcode)";
                            SqlCommand cmd = new SqlCommand(queryinsert, cnn.con);
                            cmd.Parameters.AddWithValue("@status", dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "");
                            cmd.Parameters.AddWithValue("@wipcode", wipcode);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else if (e.ColumnIndex == dgvStatus.Columns.Count - 3)
                {
                    //Update becuz it already have
                    if (dt.Rows.Count > 0)
                    {
                        string query = "";
                        if (dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                        {
                            query = "UPDATE tbMasterItemPlan SET " +
                                            "Slot='" + dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() + "' " +
                                            "WHERE ItemCode = '" + ItemCode + "' ;";
                        }
                        else
                        {
                            query = "UPDATE tbMasterItemPlan SET " +
                                            "Slot=NULL " +
                                            "WHERE ItemCode = '" + ItemCode + "' ;";
                        }

                        SqlCommand cmd = new SqlCommand(query, cnn.con);
                        cmd.ExecuteNonQuery();
                    }
                    //Add becuz it not yet have
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO tbMasterItemPlan (ItemCode, Slot) " +
                                                                            "VALUES (@Ic, @Sl)", cnn.con);
                        cmd.Parameters.AddWithValue("@Ic", ItemCode);
                        cmd.Parameters.AddWithValue("@Sl", dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (e.ColumnIndex == dgvSearchResult.Columns.Count - 1)
                {
                    DataTable dtstatus = new DataTable();
                    string wipcode = dgvSearchResult.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string query = "SELECT * FROM tbMasterItemStatus WHERE WipCode = '" + wipcode + "'";
                    SqlDataAdapter sda = new SqlDataAdapter(query, cnn.con);
                    sda.Fill(dtstatus);

                    if (dtstatus.Rows.Count > 0)
                    {
                        string queryupdate = "UPDATE tbMasterItemStatus SET Remarks = @remark WHERE WipCode = @wipcode";
                        SqlCommand cmd = new SqlCommand(queryupdate, cnn.con);
                        cmd.Parameters.AddWithValue("@remark", dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "");
                        cmd.Parameters.AddWithValue("@wipcode", wipcode);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        string queryinsert = "INSERT INTO tbMasterItemStatus (Remarks, WipCode) VALUES (@Remarks, @wipcode)";
                        SqlCommand cmd = new SqlCommand(queryinsert, cnn.con);
                        cmd.Parameters.AddWithValue("@Remarks", dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "");
                        cmd.Parameters.AddWithValue("@wipcode", wipcode);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("មានបញ្ហា!\n"+ex.Message,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            cnn.con.Close();
        }

        private void DgvSearchResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex > dgvSearchResult.Columns.Count - 5)
            {
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambong", 9, FontStyle.Regular);
            }
        }

        private void DgvMCType_CellClick(object sender, DataGridViewCellEventArgs e)
        {  
            if (dgvMCType.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() != "")
            {
                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = dgvMCType.Rows[e.RowIndex].Cells[0].Value.ToString();

            }
            else
            {
                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = null;
            }

            dgvSearchResult.Focus();
            
        }

        private void DgvMCType_LostFocus(object sender, EventArgs e)
        {
            dgvMCType.SendToBack();
        }

        private void DgvSearchResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex > dgvSearchResult.Columns.Count - 7 && e.ColumnIndex < dgvSearchResult.Columns.Count - 2)
                {
                    dgvMCType.CurrentCell = dgvMCType.Rows[0].Cells[0];
                    dgvMCType.ClearSelection();
                    if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        foreach (DataGridViewRow dgvRow in dgvMCType.Rows)
                        {
                            if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value.ToString() == dgvRow.Cells[0].Value.ToString())
                            {
                                dgvMCType.CurrentCell = dgvRow.Cells[0];
                                dgvRow.Cells[0].Selected = true;
                            }
                        }
                    }

                    Rectangle oRectangle = dgvSearchResult.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    int X = oRectangle.X + 7;
                    int Y = oRectangle.Y + 65;
                    dgvMCType.Location = new Point(X, Y);
                    dgvMCType.BringToFront();
                    dgvMCType.Focus();
                }
                if (e.ColumnIndex == dgvSearchResult.Columns.Count - 3)
                {
                    DgvSlot.CurrentCell = DgvSlot.Rows[0].Cells[0];
                    DgvSlot.ClearSelection();
                    if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        foreach (DataGridViewRow dgvRow in DgvSlot.Rows)
                        {
                            if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value.ToString() == dgvRow.Cells[0].Value.ToString())
                            {
                                DgvSlot.CurrentCell = dgvRow.Cells[0];
                                dgvRow.Cells[0].Selected = true;
                            }
                        }
                    }

                    Rectangle oRectangle = dgvSearchResult.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    int X = oRectangle.X + 1;
                    int Y = oRectangle.Y + 65;
                    DgvSlot.Location = new Point(X, Y);
                    DgvSlot.BringToFront();
                    DgvSlot.Focus();
                }
                if (e.ColumnIndex == dgvSearchResult.Columns.Count - 2)
                {
                    dgvStatus.CurrentCell = dgvStatus.Rows[0].Cells[0];
                    dgvStatus.ClearSelection();
                    if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        foreach (DataGridViewRow dgvRow in dgvStatus.Rows)
                        {
                            if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value.ToString() == (dgvRow.Cells[0].Value?.ToString() ?? ""))
                            {
                                dgvStatus.CurrentCell = dgvRow.Cells[0];
                                dgvRow.Cells[0].Selected = true;
                            }
                        }
                    }

                    Rectangle oRectangle = dgvSearchResult.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    int X = oRectangle.X + 1;
                    int Y = oRectangle.Y + 110;
                    dgvStatus.Location = new Point(X, Y);
                    dgvStatus.BringToFront();
                    dgvStatus.Focus();
                }
            }
            
        }

        private void MasterItemForm_Load(object sender, EventArgs e)
        {
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbMasterItemPlan2", cnn.con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgvMCType.Rows.Add("");
                foreach (DataRow row in dt.Rows)
                {
                    dgvMCType.Rows.Add(row[0]);
                }
                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT SLOTName FROM tbMasterSLOT ORDER BY SLOTName ASC", cnn.con);
                DataTable dtSlot = new DataTable();
                sda1.Fill(dtSlot);
                DgvSlot.Rows.Add("");
                foreach (DataRow row in dtSlot.Rows)
                {
                    DgvSlot.Rows.Add(row[0]);
                }
                dgvStatus.Rows.Add();
                dgvStatus.Rows.Add();
                dgvStatus.Rows[dgvStatus.Rows.Count - 1].Cells[0].Value = "EOL";
                dgvStatus.Rows.Add();
                dgvStatus.Rows[dgvStatus.Rows.Count - 1].Cells[0].Value = "ACTIVE";
                dgvStatus.Rows.Add();
                dgvStatus.Rows[dgvStatus.Rows.Count - 1].Cells[0].Value = "EVENT";

            }
            catch(Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
            
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Visible = true;
            LbStatus.Refresh();
            dgvSearchResult.Rows.Clear();
            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col'");
            dtSQLCond.Columns.Add("Val'");
            string state = "";
            if (txtWIPName.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("ItemName LIKE '%"+txtWIPName.Text+"%' ");
            }
            if (cbstatus.Text.Trim() != "")
            {
               if (cbstatus.Text.Trim() != "All")
                {
                    dtSQLCond.Rows.Add("tbS.Status = '" + cbstatus.Text.Trim() + "'");
                }
            }
            if ( cbmcname.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("tbP.MC"+cbmctype.Text.Trim()+"Type= '"+cbmcname.Text.Trim()+"'");
            }

            string SQLCond = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                if (SQLCond.Trim() == "")
                {
                    SQLCond = " AND " + row[0] + row[1];
                }
                else
                {
                    SQLCond  += "AND " + row[0] + row[1];

                }
            }
            try
            {
                cnn.con.Open();
                string query = @"SELECT tbM.ItemCode, ItemName,Remarks1, Remarks2, Remarks3, COALESCE(MC1Type,'') AS MC1Type, COALESCE(MC2Type,'') AS MC2Type, COALESCE(MC3Type,'') AS MC3Type, COALESCE(Slot,'') AS Slot, tbS.Status, tbS.Remarks
                                        FROM tbMasterItem tbM
                                        LEFT JOIN (SELECT ItemCode, MC1Type, MC2Type, MC3Type, Slot FROM tbMasterItemPlan) tbP ON tbP.ItemCode = tbM.ItemCode
                                        LEFT JOIN (SELECT WipCode, Status, Remarks FROM tbMasterItemStatus) tbS ON tbM.ItemCode = tbS.WipCode
                                        where tbM.ItemType = 'Work In Process' "+SQLCond+" ORDER BY tbM.ItemCode ASC";
                SqlDataAdapter sda = new SqlDataAdapter(query, cnn.con);
                sda.SelectCommand.CommandTimeout = 5000;
                Console.WriteLine(query);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    dgvSearchResult.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], row[10]);
                }
                dgvSearchResult.ClearSelection();
                LbStatus.Text = "រកឃើញទិន្នន័យចំនួន " + dgvSearchResult.Rows.Count.ToString("N0") + " !";
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
        }

    }
}
