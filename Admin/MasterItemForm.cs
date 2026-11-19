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
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

namespace MachineDeptApp.Admin
{
    public partial class MasterItemForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SqlCommand cmd;

        DataTable dtMstMCType, dtMstMCName, dtMstSlot;
        string ErrorText = "";


        public MasterItemForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.btnSearch.Click += BtnSearch_Click;
            this.Load += MasterItemForm_Load;
            this.Shown += MasterItemForm_Shown;
            this.dgvSearchResult.CellClick += DgvSearchResult_CellClick;
            this.dgvSearchResult.CellFormatting += DgvSearchResult_CellFormatting;
            this.dgvSearchResult.CellValueChanged += DgvSearchResult_CellValueChanged;
            this.dgvMCType.LostFocus += DgvMCType_LostFocus;
            this.dgvMCType.CellClick += DgvMCType_CellClick;
            this.DgvSlot.LostFocus += DgvSlot_LostFocus;
            this.DgvSlot.CellClick += DgvSlot_CellClick;
            this.dgvStatus.LostFocus += DgvStatus_LostFocus;
            this.dgvStatus.CellClick += DgvStatus_CellClick;
            this.cboMCType.TextChanged += Cbmctype_TextChanged;
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
            cboMCName.Items.Clear();
            if (cboMCType.Text.Trim() != "All")
            {
                string type ="";
                if (cboMCType.SelectedIndex == 1)
                {
                    type = "MC1Type";
                }
                else if (cboMCType.SelectedIndex == 2)
                {
                    type = "MC2Type";
                }
                else if (cboMCType.SelectedIndex == 3)
                {
                    type = "MC3Type";
                }
                string query = "SELECT "+type+" FROM tbMasterItemPlan GROUP BY "+type+"";
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(query,cnn.con);
                cboMCName.Items.Add("");
                sda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    string name = dr[0] == DBNull.Value ? null : dr[0].ToString();
                    if (name != null && name!="")
                    {
                        cboMCName.Items.Add(dr[0]);
                    }
                   
                }
            }
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
                if (dgvSearchResult.Columns[e.ColumnIndex].Name == "ColMCType1" || dgvSearchResult.Columns[e.ColumnIndex].Name == "ColMCType2" || dgvSearchResult.Columns[e.ColumnIndex].Name == "ColMCType3")
                {
                    int mc = 0;
                    if (dgvSearchResult.Columns[e.ColumnIndex].Name == "ColMCType1")
                    {
                        mc = 1;
                    }
                    else if (dgvSearchResult.Columns[e.ColumnIndex].Name == "ColMCType2")
                    {
                        mc = 2;
                    }
                    else
                    { 
                        mc = 3;
                    }

                    //Update becuz it already have
                    if (dt.Rows.Count > 0)
                    {
                        string query = "";
                        if (dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                        {
                            query = "UPDATE tbMasterItemPlan SET " +
                                            "MC" + (mc) + "Type='" + dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() + "' " +
                                            "WHERE ItemCode = '" + ItemCode + "' ;";
                        }
                        else
                        {
                            query = "UPDATE tbMasterItemPlan SET " +
                                            "MC" + (mc) + "Type=NULL " +
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
                else if (dgvSearchResult.Columns[e.ColumnIndex].Name =="ColStatus")
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
                else if (dgvSearchResult.Columns[e.ColumnIndex].Name == "ColSlot")
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
                        SqlCommand cmd = new SqlCommand("INSERT INTO tbMasterItemPlan (ItemCode, Slot) " +
                                                                            "VALUES (@Ic, @Sl)", cnn.con);
                        cmd.Parameters.AddWithValue("@Ic", ItemCode);
                        cmd.Parameters.AddWithValue("@Sl", dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (dgvSearchResult.Columns[e.ColumnIndex].Name == "ColRemark")
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
            if (e.ColumnIndex > dgvSearchResult.Columns.Count - 7)
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
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            Cursor = Cursors.WaitCursor;
            dgvSearchResult.Rows.Clear();

            string SQLCond = "";
            if (chkWIPCode.Checked && txtWIPCode.Text.Trim() != "")
                SQLCond += "AND tbM.ItemCode LIKE '%" + txtWIPCode.Text + "%' ";

            if (chkWIPName.Checked && txtWIPName.Text.Trim() != "")
                SQLCond += "AND tbM.ItemName LIKE '%" + txtWIPName.Text + "%' ";

            if (chkModel.Checked && cboModel.Text.Trim() != "")
                SQLCond += "AND tbFG.Remarks2 LIKE '%" + cboModel.Text + "%' ";

            if (chkMCType.Checked && cboMCType.Text.Trim() != "")
                SQLCond += "AND (MC1Type = '" + cboMCType.Text + "' OR MC2Type = '" + cboMCType.Text + "' OR MC3Type = '" + cboMCType.Text + "') ";
            
            if (chkMCName.Checked && txtMCName.Text.Trim() != "")
                SQLCond += "AND (MC1Name LIKE '%" + txtMCName.Text + "%' OR MC2Name LIKE '%" + txtMCName.Text + "%' OR MC3Name LIKE '%" + txtMCName.Text + "%') ";
            
            if (cbstatus.Text.Trim() != "")
            {
                if (cbstatus.Text.Trim() != "All")
                {
                    SQLCond += "AND tbS.Status = '" + cbstatus.Text + "' ";
                }
            }

            if(chkOnlyPlanItems.Checked)
                SQLCond += "AND tbPlan.WIPCode IS NOT NULL ";

            //Searching Data
            DataTable dt = new DataTable();
            try
            {
                cnn.con.Open();
                string query = @"SELECT tbM.ItemCode, tbM.ItemName, tbFG.Remarks2 AS Model, 
	                    tbM.Remarks1 AS PinAssign, 
	                    tbM.Remarks2 AS WireTube, 
	                    tbM.Remarks3 AS Length, 
	                    COALESCE(MC1Type,'') AS MC1Type, MC1Name, 
	                    COALESCE(MC2Type,'') AS MC2Type, MC2Name, 
	                    COALESCE(MC3Type,'') AS MC3Type, MC3Name, 
	                    COALESCE(Slot,'') AS Slot, tbS.Status, tbS.Remarks 
                    FROM tbMasterItem tbM 
                    LEFT JOIN tbMasterItemPlan tbP ON tbP.ItemCode = tbM.ItemCode 
                    LEFT JOIN tbMasterItemStatus tbS ON tbM.ItemCode = tbS.WipCode 
                    LEFT JOIN tbMasterItem tbFG ON LEFT(tbM.ItemCode, LEN(tbM.ItemCode)-2) = tbFG.ItemCode AND tbFG.ItemType = 'Product' 
                    LEFT JOIN (SELECT DISTINCT WIPCode FROM tbPOSDetailofMC WHERE PosCStatus <> 2) tbPlan ON tbM.ItemCode = tbPlan.WIPCode 
                    WHERE tbM.ItemType = 'Work In Process' " + SQLCond + @" 
                    ORDER BY tbM.ItemCode ASC";
                SqlDataAdapter sda = new SqlDataAdapter(query, cnn.con);
                sda.SelectCommand.CommandTimeout = 5000;
                //Console.WriteLine(query);
                sda.Fill(dt);
                
            }
            catch (Exception ex)
            {
                ErrorText = "Searching Data : \n" + ex.Message;
            }
            cnn.con.Close();

            //Add to DGV
            if (ErrorText.Trim() == "" && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataGridViewRow dgvRow = dgvSearchResult.Rows[dgvSearchResult.Rows.Add()];
                    dgvRow.Cells[ColCode.Name].Value = row["ItemCode"];
                    dgvRow.Cells[ColItemName.Name].Value = row["ItemName"];
                    dgvRow.Cells[ColModel.Name].Value = row["Model"];
                    dgvRow.Cells[ColPin.Name].Value = row["PinAssign"];
                    dgvRow.Cells[ColWireTubeColor.Name].Value = row["WireTube"];
                    dgvRow.Cells[ColLength.Name].Value = row["Length"];
                    dgvRow.Cells[ColMCType1.Name].Value = row["MC1Type"];
                    dgvRow.Cells[ColMCName1.Name].Value = row["MC1Name"];
                    dgvRow.Cells[ColMCType2.Name].Value = row["MC2Type"];
                    dgvRow.Cells[ColMCName2.Name].Value = row["MC2Name"];
                    dgvRow.Cells[ColMCType3.Name].Value = row["MC3Type"];
                    dgvRow.Cells[ColMCName3.Name].Value = row["MC3Name"];
                    dgvRow.Cells[ColSlot.Name].Value = row["Slot"];
                    dgvRow.Cells[ColStatus.Name].Value = row["Status"];
                    dgvRow.Cells[ColRemark.Name].Value = row["Remarks"];
                }
                dgvSearchResult.ClearSelection();
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                if(dgvSearchResult.Rows.Count == 0)
                    LbStatus.Text = "រកមិនឃើញទិន្នន័យទេ!";
                else
                    LbStatus.Text = "រកឃើញទិន្នន័យចំនួន " + dgvSearchResult.Rows.Count.ToString("N0") + " ";
            }
            else
                MessageBox.Show("មានបញ្ហា! " + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void MasterItemForm_Load(object sender, EventArgs e)
        {
            dgvStatus.Rows.Add();
            dgvStatus.Rows.Add();
            dgvStatus.Rows[dgvStatus.Rows.Count - 1].Cells[0].Value = "EOL";
            dgvStatus.Rows.Add();
            dgvStatus.Rows[dgvStatus.Rows.Count - 1].Cells[0].Value = "ACTIVE";
            dgvStatus.Rows.Add();
            dgvStatus.Rows[dgvStatus.Rows.Count - 1].Cells[0].Value = "EVENT";

        }
        private void MasterItemForm_Shown(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;

            TakingMstMC();

            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
                MessageBox.Show("មានបញ្ហា! " + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        //Method
        private void TakingMstMC()
        {
            //Taking Mst Data
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbMasterItemPlan2", cnn.con);
                dtMstMCType = new DataTable();
                sda.Fill(dtMstMCType);
                sda = new SqlDataAdapter("SELECT DISTINCT MCName FROM tbMasterMCType ORDER BY MCName", cnn.con);
                dtMstMCName = new DataTable();
                sda.Fill(dtMstMCName);
                sda = new SqlDataAdapter("SELECT SLOTName FROM tbMasterSLOT ORDER BY SLOTName ASC", cnn.con);
                dtMstSlot = new DataTable();
                sda.Fill(dtMstSlot);
            }
            catch (Exception ex)
            {
                ErrorText = "Taking Mst Data:\n" + ex.Message;
                //MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

        }

    }
}
