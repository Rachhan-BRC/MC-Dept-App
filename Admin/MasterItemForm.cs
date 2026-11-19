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

        DataTable dtMstMCType, dtMstMCName, dtMstSlot;
        string ErrorText = "", BeforeEdit = "";


        public MasterItemForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.btnSearch.Click += BtnSearch_Click;
            this.Load += MasterItemForm_Load;
            this.Shown += MasterItemForm_Shown;
            this.dgvSearchResult.CellClick += DgvSearchResult_CellClick;
            this.dgvSearchResult.CellFormatting += DgvSearchResult_CellFormatting;
            this.dgvSearchResult.Scroll += DgvSearchResult_Scroll;
            this.dgvSearchResult.CellBeginEdit += DgvSearchResult_CellBeginEdit;
            this.dgvSearchResult.CellEndEdit += DgvSearchResult_CellEndEdit;

            this.dgvMCType.LostFocus += DgvMCType_LostFocus;
            this.dgvMCType.CellClick += DgvMCType_CellClick;

            this.dgvMCName.LostFocus += DgvMCName_LostFocus;
            this.dgvMCName.CellClick += DgvMCName_CellClick;

            this.dgvSlot.LostFocus += DgvSlot_LostFocus;
            this.dgvSlot.CellClick += DgvSlot_CellClick;

            this.dgvStatus.LostFocus += DgvStatus_LostFocus;
            this.dgvStatus.CellClick += DgvStatus_CellClick;

            this.cboMCType.TextChanged += Cbmctype_TextChanged;

            this.btnExport.Click += BtnExport_Click;
        }

        private void DgvSearchResult_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvSearchResult.Columns[e.ColumnIndex].Name == "ColRemark")
                {
                    string NewValue = dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "";
                    if (BeforeEdit != NewValue)
                    {
                        if (dgvSearchResult.SelectedCells.Count > 0 && dgvSearchResult.CurrentCell != null &&
                        dgvSearchResult.CurrentRow.Index >= 0 && dgvSearchResult.CurrentCell.ColumnIndex >= 0)
                        {
                            string WIPCode = dgvSearchResult.CurrentRow.Cells["ColCode"].Value.ToString();
                            SaveStatusOrRemark(WIPCode, "Remarks", NewValue);
                        }
                    }

                }
            }

        }
        private void DgvSearchResult_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvSearchResult.Columns[e.ColumnIndex].Name == "ColRemark")
                    BeforeEdit = dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "";
            }
        }
        private void DgvSearchResult_Scroll(object sender, ScrollEventArgs e)
        {
            dgvMCType.SendToBack();
            dgvMCName.SendToBack();
            dgvSlot.SendToBack();
            dgvStatus.SendToBack();
        }
        private void DgvMCName_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSearchResult.SelectedCells.Count > 0 && dgvSearchResult.CurrentCell != null && 
                dgvSearchResult.CurrentRow.Index >= 0 && dgvSearchResult.CurrentCell.ColumnIndex >= 0 && 
                dgvMCName.SelectedCells.Count > 0 && dgvMCName.CurrentCell != null)
            {
                string WIPCode = dgvSearchResult.CurrentRow.Cells["ColCode"].Value.ToString(), 
                    ColName = dgvSearchResult.Columns[dgvSearchResult.CurrentCell.ColumnIndex].Name,
                    SelectedValue = dgvMCName.CurrentRow.Cells[0].Value?.ToString() ?? "";

                if (ColName.Contains("MCName"))
                {
                    ColName = ColName.Replace("ColMCName", "");
                    SaveMCName(WIPCode, ColName, SelectedValue);
                }

            }
        }
        private void DgvMCName_LostFocus(object sender, EventArgs e)
        {
            dgvMCName.SendToBack();
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
            if (cboMCType.Text.Trim() != "")
                chkMCType.Checked = true;
            else
                chkMCType.Checked = false;
        }
        private void DgvStatus_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSearchResult.SelectedCells.Count > 0 && dgvSearchResult.CurrentCell != null &&
                dgvSearchResult.CurrentRow.Index >= 0 && dgvSearchResult.CurrentCell.ColumnIndex >= 0 &&
                dgvStatus.SelectedCells.Count > 0 && dgvStatus.CurrentCell != null)
            {
                string WIPCode = dgvSearchResult.CurrentRow.Cells["ColCode"].Value.ToString(),
                    SelectedValue = dgvStatus.CurrentRow.Cells[0].Value?.ToString() ?? "";

                SaveStatusOrRemark(WIPCode, "Status", SelectedValue);

            }
        }
        private void DgvStatus_LostFocus(object sender, EventArgs e)
        {
            this.dgvStatus.SendToBack();
        }
        private void DgvSlot_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSearchResult.SelectedCells.Count > 0 && dgvSearchResult.CurrentCell != null &&
                dgvSearchResult.CurrentRow.Index >= 0 && dgvSearchResult.CurrentCell.ColumnIndex >= 0 &&
                dgvSlot.SelectedCells.Count > 0 && dgvSlot.CurrentCell != null)
            {
                string WIPCode = dgvSearchResult.CurrentRow.Cells["ColCode"].Value.ToString(),
                    SelectedValue = dgvSlot.CurrentRow.Cells[0].Value?.ToString() ?? "";

                SaveSlot(WIPCode, SelectedValue);

            }
        }
        private void DgvSlot_LostFocus(object sender, EventArgs e)
        {
            this.dgvSlot.SendToBack();
        }
        private void DgvSearchResult_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            /*
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

            */

        }
        private void DgvSearchResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvSearchResult.Columns[e.ColumnIndex].Name.Contains("MCType") || dgvSearchResult.Columns[e.ColumnIndex].Name.Contains("MCName") ||
                    dgvSearchResult.Columns[e.ColumnIndex].Name.Contains("Slot") || dgvSearchResult.Columns[e.ColumnIndex].Name.Contains("Remark"))
                {
                    e.CellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void DgvMCType_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSearchResult.SelectedCells.Count > 0 && dgvSearchResult.CurrentCell != null &&
                dgvSearchResult.CurrentRow.Index >= 0 && dgvSearchResult.CurrentCell.ColumnIndex >= 0 &&
                dgvMCType.SelectedCells.Count > 0 && dgvMCType.CurrentCell != null)
            {
                string WIPCode = dgvSearchResult.CurrentRow.Cells["ColCode"].Value.ToString(),
                    ColName = dgvSearchResult.Columns[dgvSearchResult.CurrentCell.ColumnIndex].Name,
                    SelectedValue = dgvMCType.CurrentRow.Cells[0].Value?.ToString() ?? "";

                if (ColName.Contains("MCType"))
                {
                    ColName = ColName.Replace("ColMCType", "");
                    SaveMCType(WIPCode, ColName, SelectedValue);
                }
            }
        }
        private void DgvMCType_LostFocus(object sender, EventArgs e)
        {
            dgvMCType.SendToBack();
        }
        private void DgvSearchResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSearchResult.SelectedCells.Count > 0 && dgvSearchResult.CurrentCell != null && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string SelColName = dgvSearchResult.Columns[e.ColumnIndex].Name;
                DataGridView dgvToBeShow = null;
                if (SelColName.Contains("MCType"))
                {
                    dgvToBeShow = dgvMCType;
                    dgvToBeShow.Rows.Clear();
                    dgvToBeShow.Rows.Add();
                    foreach(DataRow row in dtMstMCType.Rows)
                        dgvToBeShow.Rows.Add(row["MCType"].ToString());
                    string CurrentValue = dgvSearchResult.CurrentRow.Cells[e.ColumnIndex].Value?.ToString() ?? "";
                    if (CurrentValue.Trim() != "")
                    {
                        foreach (DataGridViewRow​ drow in dgvToBeShow.Rows)
                        {
                            if ((drow.Cells[0].Value?.ToString() ?? "") == CurrentValue)
                                dgvToBeShow.CurrentCell = drow.Cells[0];
                        }
                    }
                    else
                    {
                        dgvToBeShow.ClearSelection();
                        dgvToBeShow.CurrentCell = null;
                    }
                }
                if (SelColName.Contains("MCName"))
                {
                    dgvToBeShow = dgvMCName;
                    dgvToBeShow.Rows.Clear();
                    dgvToBeShow.Rows.Add();
                    string MCNoSelected = dgvSearchResult.Columns[e.ColumnIndex].Name.Replace("ColMCName",""),
                        MCTypeText = dgvSearchResult.CurrentRow.Cells["ColMCType" + MCNoSelected].Value?.ToString() ?? "";
                    foreach (DataRow row in dtMstMCName.Rows)
                    {
                        bool OkayToAdd = false;
                        if (MCTypeText.Trim() != "")
                        {
                            if (row["MCType"].ToString() == MCTypeText)
                                OkayToAdd = true;
                        }
                        else
                            OkayToAdd = true;
                        if (OkayToAdd)
                        {
                            string Value = row["MCName"].ToString();
                            bool Found = false;
                            foreach (DataGridViewRow drow in dgvToBeShow.Rows)
                            {
                                if ((drow.Cells[0].Value?.ToString() ?? "") == Value)
                                {
                                    Found = true;
                                    break;
                                }
                            }
                            if (!Found)
                                dgvToBeShow.Rows.Add(Value);
                        }
                    }
                    string CurrentValue = dgvSearchResult.CurrentRow.Cells[e.ColumnIndex].Value?.ToString() ?? "";
                    if (CurrentValue.Trim() != "")
                    {
                        foreach (DataGridViewRow​ drow in dgvToBeShow.Rows)
                        {
                            if ((drow.Cells[0].Value?.ToString() ?? "") == CurrentValue)
                                dgvToBeShow.CurrentCell = drow.Cells[0];
                        }
                    }
                    else
                    {
                        dgvToBeShow.ClearSelection();
                        dgvToBeShow.CurrentCell = null;
                    }

                }
                if (SelColName.Contains("Slot"))
                {
                    dgvToBeShow = dgvSlot;
                    dgvToBeShow.Rows.Clear();
                    dgvToBeShow.Rows.Add();
                    foreach (DataRow row in dtMstSlot.Rows)
                        dgvToBeShow.Rows.Add(row["SLOTName"].ToString());
                }
                if (SelColName.Contains("Status"))
                    dgvToBeShow = dgvStatus;

                if (dgvToBeShow != null)
                {
                    //Match the popup's width to the clicked column (its inner column is set to Fill)
                    dgvToBeShow.Width = dgvSearchResult.Columns[e.ColumnIndex].Width;

                    //Shrink the popup's height to fit its rows, but never taller than its Designer height
                    if (dgvToBeShow.Tag == null)
                        dgvToBeShow.Tag = dgvToBeShow.Height;
                    int maxHeight = (int)dgvToBeShow.Tag;
                    //DataGridView draws its border inside the client area, so add it manually (top + bottom)
                    int borderHeight = dgvToBeShow.BorderStyle == BorderStyle.Fixed3D ? 5 : dgvToBeShow.BorderStyle == BorderStyle.FixedSingle ? 3 : 0;
                    int rowsHeight = dgvToBeShow.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
                    dgvToBeShow.Height = Math.Min(maxHeight, rowsHeight + borderHeight);

                    //Position the popup just below the clicked cell, relative to the popup's own parent
                    Rectangle cellRect = dgvSearchResult.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                    Point cellBottomLeft = new Point(cellRect.Left, cellRect.Bottom);
                    Point popupLocation = dgvToBeShow.Parent.PointToClient(dgvSearchResult.PointToScreen(cellBottomLeft));

                    //Keep the popup inside its parent so it is never cut off
                    Size parentSize = dgvToBeShow.Parent.ClientSize;
                    if (popupLocation.Y + dgvToBeShow.Height > parentSize.Height)
                        popupLocation.Y = popupLocation.Y - cellRect.Height - dgvToBeShow.Height;
                    popupLocation.X = Math.Max(0, Math.Min(popupLocation.X, parentSize.Width - dgvToBeShow.Width));
                    popupLocation.Y = Math.Max(0, popupLocation.Y);

                    dgvToBeShow.Location = popupLocation;
                    dgvToBeShow.BringToFront();
                    dgvToBeShow.Focus();
                }
            }
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            Cursor = Cursors.WaitCursor;
            dgvSearchResult.Rows.Clear();
            TakingMstMC();
            ErrorText = "";

            string SQLCond = "";
            if (chkWIPCode.Checked && txtWIPCode.Text.Trim() != "")
                SQLCond += "AND tbM.ItemCode LIKE '%" + txtWIPCode.Text + "%' ";

            if (chkWIPName.Checked && txtWIPName.Text.Trim() != "")
                SQLCond += "AND tbM.ItemName LIKE '%" + txtWIPName.Text + "%' ";

            if (chkModel.Checked && txtModel.Text.Trim() != "")
                SQLCond += "AND tbFG.Remarks2 LIKE '%" + txtModel.Text + "%' ";

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

            foreach (DataGridViewColumn col in dgvSearchResult.Columns)
            {
                if (!col.Name.Contains("Remark"))
                    col.ReadOnly = true;

                if (col.Name.Contains("1"))
                {
                    col.HeaderCell.Style.BackColor = Color.Green;
                    col.HeaderCell.Style.SelectionBackColor = Color.Green;
                    col.HeaderCell.Style.ForeColor = Color.White;
                    col.HeaderCell.Style.SelectionForeColor = Color.White;
                }
                if (col.Name.Contains("2"))
                {
                    col.HeaderCell.Style.BackColor = Color.Orange;
                    col.HeaderCell.Style.SelectionBackColor = Color.Orange;
                    col.HeaderCell.Style.ForeColor = Color.White;
                    col.HeaderCell.Style.SelectionForeColor = Color.White;
                }
                if (col.Name.Contains("3"))
                {
                    col.HeaderCell.Style.BackColor = Color.Blue;
                    col.HeaderCell.Style.SelectionBackColor = Color.Blue;
                    col.HeaderCell.Style.ForeColor = Color.White;
                    col.HeaderCell.Style.SelectionForeColor = Color.White;
                }

            }

        }
        private void MasterItemForm_Shown(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;

            TakingMstMC();

            if (dtMstMCType.Rows.Count > 0)
            {
                try
                {
                    foreach (DataRow row in dtMstMCType.Rows)
                        cboMCType.Items.Add(row["MCType"]);

                }
                catch { }
            }

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
                SqlDataAdapter sda = new SqlDataAdapter("SELECT DISTINCT MCType FROM tbMasterMCType ORDER BY MCType", cnn.con);
                dtMstMCType = new DataTable();
                sda.Fill(dtMstMCType);
                sda = new SqlDataAdapter("SELECT MCType, MCName FROM tbMasterMCType ORDER BY MCName", cnn.con);
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
        private void SaveMCType(string WIPCode, string MCNo, string MCType)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;

            //Save MCType
            try
            {
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand(@"IF EXISTS (SELECT 1 FROM tbMasterItemPlan WHERE ItemCode = @code)
                        BEGIN
                            UPDATE tbMasterItemPlan SET MC" + MCNo + @"Type = @type WHERE ItemCode = @code; 
                        END
                    ELSE 
                        BEGIN 
                            INSERT INTO tbMasterItemPlan (ItemCode, MC" + MCNo + @"Type) VALUES (@code, @type); 
                        END", cnn.con);
                if(MCType.Trim()!="")
                    cmd.Parameters.AddWithValue("@type", MCType);
                else
                    cmd.Parameters.AddWithValue("@type", DBNull.Value);
                cmd.Parameters.AddWithValue("@code", WIPCode);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrorText = "Save MCType : \n" + ex.Message;
            }
            finally
            {
                cnn.con.Close();
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
                dgvSearchResult.CurrentRow.Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = MCType;
            else
                MessageBox.Show("Something went wrong! "+ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

            dgvSearchResult.Focus();

        }
        private void SaveMCName(string WIPCode, string MCNo, string MCName)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;

            //Save MCName
            try
            {
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand(@"IF EXISTS (SELECT 1 FROM tbMasterItemPlan WHERE ItemCode = @code)
                        BEGIN
                            UPDATE tbMasterItemPlan SET MC" + MCNo + @"Name = @name WHERE ItemCode = @code; 
                        END
                    ELSE 
                        BEGIN 
                            INSERT INTO tbMasterItemPlan (ItemCode, MC" + MCNo + @"Name) VALUES (@code, @name); 
                        END", cnn.con);
                if(MCName.Trim()!="")
                    cmd.Parameters.AddWithValue("@name", MCName);
                else
                    cmd.Parameters.AddWithValue("@name", DBNull.Value);
                cmd.Parameters.AddWithValue("@code", WIPCode);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrorText = "Save MCName : \n" + ex.Message;
            }
            finally
            {
                cnn.con.Close();
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
                dgvSearchResult.CurrentRow.Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = MCName;
            else
                MessageBox.Show("Something went wrong! " + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

            dgvSearchResult.Focus();

        }
        private void SaveSlot(string WIPCode, string Slot)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;

            //Save Slot
            try
            {
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand(@"IF EXISTS (SELECT 1 FROM tbMasterItemPlan WHERE ItemCode = @code)
                        BEGIN
                            UPDATE tbMasterItemPlan SET Slot = @slot WHERE ItemCode = @code; 
                        END
                    ELSE 
                        BEGIN 
                            INSERT INTO tbMasterItemPlan (ItemCode, Slot) VALUES (@code, @slot); 
                        END", cnn.con);
                if (Slot.Trim() != "")
                    cmd.Parameters.AddWithValue("@slot", Slot);
                else
                    cmd.Parameters.AddWithValue("@slot", DBNull.Value);
                cmd.Parameters.AddWithValue("@code", WIPCode);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrorText = "Save Slot : \n" + ex.Message;
            }
            finally
            {
                cnn.con.Close();
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
                dgvSearchResult.CurrentRow.Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = Slot;
            else
                MessageBox.Show("Something went wrong! " + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

            dgvSearchResult.Focus();

        }
        private void SaveStatusOrRemark(string WIPCode, string StatusOrRemark, string Value)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;

            //Save StatusOrRemark
            try
            {
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand(@"IF EXISTS (SELECT 1 FROM tbMasterItemStatus WHERE WipCode = @code)
                        BEGIN
                            UPDATE tbMasterItemStatus SET " + StatusOrRemark + @" = @value WHERE WipCode = @code; 
                        END
                    ELSE 
                        BEGIN 
                            INSERT INTO tbMasterItemStatus (WipCode, " + StatusOrRemark + @") VALUES (@code, @value); 
                        END", cnn.con);
                if (Value.Trim() != "")
                    cmd.Parameters.AddWithValue("@value", Value);
                else
                    cmd.Parameters.AddWithValue("@value", DBNull.Value);
                cmd.Parameters.AddWithValue("@code", WIPCode);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrorText = "Save StatusOrRemark : \n" + ex.Message;
            }
            finally
            {
                cnn.con.Close();
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
                dgvSearchResult.CurrentRow.Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = Value;
            else
                MessageBox.Show("Something went wrong! " + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

            dgvSearchResult.Focus();

        }

    }
}
