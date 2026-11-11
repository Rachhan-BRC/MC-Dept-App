using MachineDeptApp;
using System;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Linq;
using MachineDeptApp.OtherClass;

namespace MachineDeptApp.Admin
{
    public partial class SubLeaderRegisterForm : Form
    {
        ExportAsCSVClass CSV = new ExportAsCSVClass();
        SQLConnect cnn = new SQLConnect();
        SQLConnect cnnMC = new SQLConnect();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        DataTable dtOBSMstItems = new DataTable(), dtMstLunchShift = new DataTable();

        string ErrorText;


        public SubLeaderRegisterForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnMC.Connection();
            this.cnnOBS.Connection();
            this.Shown += SubLeaderRegisterForm_Shown;
            this.btnSearch.Click += BtnSearch_Click;
            this.txtIDCard.TextChanged += TxtIDCard_TextChanged;
            this.txtIDCard.KeyDown += txtKeyDown;
            this.txtEngName.TextChanged += TxtEngName_TextChanged;
            this.txtEngName.KeyDown += txtKeyDown;
            this.txtNFC.TextChanged += TxtNFC_TextChanged;
            this.txtNFC.KeyDown += txtKeyDown;
            this.txtKhName.TextChanged += TxtKhName_TextChanged;
            this.txtKhName.KeyDown += txtKeyDown;
            this.DgvSearch.CellClick += DgvSearch_CellClick;
            this.DgvSearch.CurrentCellChanged += DgvSearch_CurrentCellChanged;
            this.DgvSearch.CellPainting += DgvSearch_CellPainting;
            this.btnPrint.EnabledChanged += BtnPrint_EnabledChanged;
            this.btnPrint.Click += BtnPrint_Click;
            this.btnExport.Click += BtnExport_Click;
            this.btnAdd.Click += BtnAdd_Click;
            this.btnUpdate.EnabledChanged += BtnUpdate_EnabledChanged;
            this.btnUpdate.Click += BtnUpdate_Click;
            this.btnDelete.EnabledChanged += BtnDelete_EnabledChanged;
            this.btnDelete.Click += BtnDelete_Click;

            //Member
            this.txtMemberS.KeyDown += TxtMemberS_KeyDown;
            this.btnMemberS.Click += BtnMemberS_Click;
            this.dgvMember.CellClick += DgvMember_CellClick;


            //Process
            this.dgvMstProcess.CellClick += DgvMstProcess_CellClick;

        }

        private void DgvSearch_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (DgvSearch.Columns[e.ColumnIndex].Name.ToString() == "LunchShift")
                {
                    e.CellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            DgvSearch.ClearSelection();
            DgvSearch.CurrentCell = null;
            ShowOrHideTabEdit();
            SubLeaderRegisterAddUpdateForm srauf = new SubLeaderRegisterAddUpdateForm("Add", this.DgvSearch);
            srauf.ShowDialog();
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (DgvSearch.SelectedCells.Count > 0 && DgvSearch.CurrentCell != null && DgvSearch.CurrentCell.RowIndex >= 0 && DgvSearch.CurrentCell.ColumnIndex >= 0)
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់លុប Sub-Leader នេះមែនដែរឬទេ?", MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;

                    string SubLID = DgvSearch.Rows[DgvSearch.CurrentCell.RowIndex].Cells["IDCard"].Value.ToString();
                    try
                    {
                        cnn.con.Open();

                        string SQLQuery = "DELETE FROM MstSubL WHERE SubL_IDNo = @sID";
                        SqlCommand cmd = new SqlCommand(SQLQuery, cnn.con);
                        cmd.Parameters.AddWithValue("@sID", SubLID);
                        cmd.ExecuteNonQuery();

                        SQLQuery = "DELETE FROM MstSubLMember WHERE SubL_IDNo = @sID";
                        cmd = new SqlCommand(SQLQuery, cnn.con);
                        cmd.Parameters.AddWithValue("@sID", SubLID);
                        cmd.ExecuteNonQuery();

                        SQLQuery = "DELETE FROM MstSubLProcess WHERE SubL_IDNo = @sID";
                        cmd = new SqlCommand(SQLQuery, cnn.con);
                        cmd.Parameters.AddWithValue("@sID", SubLID);
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        ErrorText = "Deleting : " + ex.Message;
                    }
                    cnn.con.Close();


                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        MessageBox.Show("លុបរួចរាល់!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DgvSearch.Rows.RemoveAt(DgvSearch.CurrentCell.RowIndex);
                        DgvSearch.ClearSelection();
                        DgvSearch.CurrentCell = null;
                    }
                    else
                    {
                        MessageBox.Show("មានបញ្ហា!\n"+ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (DgvSearch.SelectedCells.Count > 0 && DgvSearch.CurrentCell != null && DgvSearch.CurrentCell.RowIndex >= 0 && DgvSearch.CurrentCell.ColumnIndex >= 0)
            {
                SubLeaderRegisterAddUpdateForm srauf = new SubLeaderRegisterAddUpdateForm("Update", this.DgvSearch);
                srauf.ShowDialog();
            }
        }
        private void BtnDelete_EnabledChanged(object sender, EventArgs e)
        {
            if (btnDelete.Enabled == true)
                btnDeleteGRAY.SendToBack();
            else
                btnDeleteGRAY.BringToFront();
        }
        private void BtnUpdate_EnabledChanged(object sender, EventArgs e)
        {
            if (btnUpdate.Enabled == true)
                btnUpdateGRAY.SendToBack();
            else
                btnUpdateGRAY.BringToFront();
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (DgvSearch.Rows.Count > 0)
            {
                CSV.CSVErrorT = "";
                CSV.ExportAsCSV(this.DgvSearch, "Mst_SubL", 0);
                if (CSV.CSVErrorT.Trim() != "")
                {
                    if (CSV.CSVErrorT.Trim() == "OK")
                        MessageBox.Show("Export successfully!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Something went wrong!\n" + CSV.CSVErrorT, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void TxtKhName_TextChanged(object sender, EventArgs e)
        {
            if (txtKhName.Text.Trim() != "")
                chkKhName.Checked = true;
            else
                chkKhName.Checked = false;
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            DialogResult DSL = MessageBox.Show("តើអ្នកចង់ព្រីនបញ្ជីសមាជិកមែនដែរឬទេ?", MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(DSL == DialogResult.Yes)
            {
                ErrorText = "";
                Cursor = Cursors.WaitCursor;

                string SubLID = DgvSearch.Rows[DgvSearch.CurrentCell.RowIndex].Cells["IDCard"].Value.ToString();
                string SubLName = DgvSearch.Rows[DgvSearch.CurrentCell.RowIndex].Cells["EngName"].Value.ToString();
                DateTime PrintingDate = DateTime.Now;
                string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\SubL Member";
                string file = "SubL Member - " + SubLID;
                string fName = file + " ( " + PrintingDate.ToString("dd-MM-yyyy HH_mm_ss") + " )";

                //Taking the MstMember
                DataTable dtList = new DataTable();
                try
                {
                    cnn.con.Open();
                    string SQLQuery = "SELECT * FROM MstSubLMember WHERE SubL_IDNo = '"+SubLID+"' " +
                                                "\nORDER BY Memb_IDNo ASC";
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtList);
                }
                catch (Exception ex)
                {
                    ErrorText = "Taking the MstMember : " + ex.Message;
                }
                cnn.con.Close();

                //Print to excel
                if (ErrorText.Trim() == "" && dtList.Rows.Count>0)
                {                    
                    //ឆែករកមើល Folder បើគ្មាន => បង្កើត
                    if (!Directory.Exists(SavePath))
                    {
                        Directory.CreateDirectory(SavePath);
                    }

                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: Environment.CurrentDirectory.ToString() + @"\Template\SubLMember-Template.xlsx", Editable: true);
                    Excel.Worksheet ws = (Excel.Worksheet)xlWorkBook.Sheets["Rachhan System"];

                    try
                    {
                        //Header Info
                        foreach (Excel.Shape shape in ws.Shapes)
                        {
                            foreach (Excel.Shape childShape in shape.GroupItems) // Iterate through the grouped items
                            {
                                if (childShape.Name == "Leader") // Check for your shape by name
                                {
                                    childShape.TextFrame.Characters().Text = SubLName;
                                }
                                if (childShape.Name == "PrintDate") // Check for your shape by name
                                {
                                    childShape.TextFrame.Characters().Text = PrintingDate.ToString("dd-MMM-yyyy hh:mm tt");
                                }
                            }
                        }

                        int NextRowIndex = 8;
                        //List
                        foreach (DataRow row in dtList.Rows)
                        {
                            // Copy row 4 to 7 and paste it to next row
                            Excel.Range sourceRange = ws.get_Range("4:7");
                            Excel.Range destinationRow = ws.Rows[NextRowIndex];
                            //Paste
                            sourceRange.Copy(destinationRow);

                            //Set Value
                            ws.Cells[NextRowIndex-3, 2] = row["Memb_EngName"].ToString()+"\n"+ row["Memb_KhName"].ToString();
                            ws.Cells[NextRowIndex - 1, 2] = row["Memb_IDNo"].ToString();
                            ws.Cells[NextRowIndex - 3, 6] = "*" + row["Memb_IDNo"].ToString() + "*";

                            NextRowIndex +=4;
                        }


                        // Saving the modified Excel file
                        ws.SaveAs(SavePath + @"\" + fName + ".xlsx");
                        SavePath = SavePath + @"\" + fName + ".xlsx";
                    }
                    catch (Exception ex)
                    {
                        ErrorText = "Print Excel : " + ex.Message;
                    }

                    excelApp.DisplayAlerts = false;
                    xlWorkBook.Close();
                    excelApp.DisplayAlerts = true;
                    excelApp.Quit();

                    //Kill all Excel background process
                    var processes = from p in System.Diagnostics.Process.GetProcessesByName("EXCEL")
                                    select p;
                    foreach (var process in processes)
                    {
                        if (process.MainWindowTitle.ToString().Trim() == "")
                            process.Kill();
                    }
                }


                Cursor = Cursors.Default;

                if (ErrorText.Trim() == "")
                {
                    if (dtList.Rows.Count > 0)
                    {
                        MessageBox.Show("ព្រីនរួចរាល់", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (SavePath.Trim() != "")
                            System.Diagnostics.Process.Start(SavePath);
                    }
                    else
                    {
                        MessageBox.Show("គ្មានទិន្នន័យសមាជិកនុះទេ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void BtnPrint_EnabledChanged(object sender, EventArgs e)
        {
            if (btnPrint.Enabled == false)
                btnPrintGRAY.BringToFront();
            else
                btnPrintGRAY.SendToBack();
        }        
        private void DgvMstProcess_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && DgvSearch.SelectedCells.Count>0)
            {
                if (dgvMstProcess.Columns[e.ColumnIndex].Name == "RegStatus")
                {
                    string SubL = DgvSearch.Rows[DgvSearch.CurrentCell.RowIndex].Cells["IDCard"].Value.ToString();
                    string ProCode = dgvMstProcess.Rows[e.RowIndex].Cells["MstProcessCode"].Value.ToString();
                    string ProName = dgvMstProcess.Rows[e.RowIndex].Cells["MstProcess"].Value.ToString();
                    string ProRemark = dgvMstProcess.Rows[e.RowIndex].Cells["PRemark"].Value.ToString();
                    if (dgvMstProcess.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper() == "TRUE")
                    {
                        DeleteProcess(SubL, ProCode);
                        dgvMstProcess.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                    }
                    else
                    {
                        AddProcess(SubL, ProCode, ProName, ProRemark);
                        dgvMstProcess.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                    }
                    dgvMstProcess.ClearSelection();
                }
            }
        }        
        private void DgvMember_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) 
            {
                if (dgvMember.Columns[e.ColumnIndex].Name == "Remove")
                {
                    DialogResult DSL = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនឬទេ?", MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DSL == DialogResult.Yes)
                    {
                        ErrorText = "";
                        Cursor = Cursors.WaitCursor;

                        try
                        {
                            cnn.con.Open();
                            string SQLQuery = "DELETE FROM MstSubLMember WHERE Memb_IDNo='" + dgvMember.Rows[e.RowIndex].Cells["MIDCard"].Value.ToString() +"' ";
                            SqlCommand cmd = new SqlCommand(SQLQuery, cnn.con);
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            ErrorText = ex.Message;
                        }
                        cnn.con.Close();

                        Cursor = Cursors.Default;

                        if (ErrorText.Trim() == "")
                        {
                            dgvMember.Rows.RemoveAt(e.RowIndex);
                            dgvMember.ClearSelection();
                            dgvMember.Update();
                        }
                        else
                        {
                            MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private void DgvSearch_CurrentCellChanged(object sender, EventArgs e)
        {
            if (DgvSearch.SelectedCells.Count>0 && DgvSearch.CurrentCell != null && DgvSearch.CurrentCell.RowIndex>=0 && DgvSearch.CurrentCell.ColumnIndex >= 0)
            {
                Cursor = Cursors.WaitCursor;
                btnPrint.Enabled = true;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                TakingSubLDetails();
                Cursor = Cursors.Default;
            }
            else
            {
                btnPrint.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
        }
        private void DgvSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            TakingSubLDetails();
            ShowOrHideTabEdit();
            Cursor = Cursors.Default;
        }
        private void BtnMemberS_Click(object sender, EventArgs e)
        {
            this.TxtMemberS_KeyDown(sender, new KeyEventArgs(Keys.Enter));
        }
        private void TxtMemberS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtMemberS.Text.Trim()!="" && DgvSearch.SelectedCells.Count>0)
            {
                ErrorText = "";
                Cursor = Cursors.WaitCursor;

                string SQLCond="";
                string SearchValue = txtMemberS.Text.ToUpper();
                if (cboMemberSType.Text == "NFC")
                {
                    SQLCond = "Card_ID = '"+SearchValue+"'";
                }
                else
                {
                    SearchValue = SearchValue.Replace("MKH","");
                    SearchValue = SearchValue.Replace("-", "");
                    SQLCond = "Staff_No = 'MKH-" + SearchValue + "'";
                }

                //Taking Employee
                DataTable dtEmployee = new DataTable();
                try
                {
                    cnnMC.con.Open();
                    string SQLQuery = "SELECT *  FROM [TD_Database].[dbo].[Employee_list] WHERE Department =  'Machine' AND " + SQLCond;
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnMC.con);
                    Console.WriteLine(SQLQuery);
                    sda.Fill(dtEmployee);
                }
                catch (Exception ex)
                {
                    ErrorText = "Taking Employee : " + ex.Message;
                }
                cnnMC.con.Close();

                //Checking
                DataTable dtChecking = new DataTable();
                if (ErrorText.Trim() == "")
                {
                    if (dtEmployee.Rows.Count > 0)
                    {
                        try
                        {
                            cnn.con.Open();
                            string SQLQuery = "SELECT * FROM MstSubLMember WHERE Memb_IDNo = '" + dtEmployee.Rows[0]["Staff_No"].ToString() +"'";
                            SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                            sda.Fill(dtChecking);
                        }
                        catch (Exception ex)
                        {
                            ErrorText = "Checking : " + ex.Message;
                        }
                        cnn.con.Close();
                    }
                }

                Cursor = Cursors.Default;
                if (ErrorText.Trim() == "")
                {
                    if (dtEmployee.Rows.Count > 0)
                    {
                        string SubIDCard = DgvSearch.Rows[DgvSearch.CurrentCell.RowIndex].Cells["IDCard"].Value.ToString();
                        string NFC = dtEmployee.Rows[0]["Card_ID"].ToString();
                        string IDCard = dtEmployee.Rows[0]["Staff_No"].ToString();
                        string EngName = dtEmployee.Rows[0]["Name"].ToString().ToUpper();
                        string KhName = dtEmployee.Rows[0]["Khmer_Name"].ToString();
                        string Sex = dtEmployee.Rows[0]["Sex"].ToString();
                        string Position = dtEmployee.Rows[0]["Position"].ToString();
                        DateTime StartedWork = Convert.ToDateTime(dtEmployee.Rows[0]["Started"]);
                        StartedWork = Convert.ToDateTime(StartedWork.ToString("yyyy-MM-dd")+" 00:00:00");

                        string Details = " • លេខកាត \t : " + IDCard;
                        Details += "\n • ឈ្មោះ \t\t : " + EngName;
                        Details += "\n • ឈ្មោះជាភាសាខ្មែរ \t : " + KhName;

                        if (SubIDCard != IDCard)
                        {
                            if (Position == "Asst. Sub Leader" || Position == "Operator")
                            {
                                if (dtChecking.Rows.Count == 0)
                                {
                                    txtMemberS.Text = "";
                                    DialogResult DSL = MessageBox.Show("តើអ្នកចង់ចុះឈ្មោះទិន្នន័យនេះមែនឬទេ?\n" + Details, MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (DSL == DialogResult.Yes)
                                        AddEmployee(0, SubIDCard, IDCard, NFC, EngName, KhName, Sex, StartedWork);
                                }
                                else
                                {
                                    DialogResult DSL = MessageBox.Show("ទិន្នន័យនេះធ្លាប់បានចុះឈ្មោះរួចហើយ!\n" + Details +
                                                                                                "\n • លេខកាត SubL \t : " + dtChecking.Rows[0]["SubL_IDNo"].ToString() +
                                                                                                "\n\nតើអ្នកចង់បន្ដឬទេ?", MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                    if (DSL == DialogResult.Yes)
                                        AddEmployee(1, SubIDCard, IDCard, NFC, EngName, KhName, Sex, StartedWork);
                                    else
                                        txtMemberS.SelectAll();
                                }
                            }
                            else
                            {
                                //MessageBox.Show("តួនាទីនេះមិនអាចចុះឈ្មោះបានទេ!\n" + Details +
                                //                            "\n • តួនាទី \t\t : " + Position, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //txtMemberS.SelectAll();

                                txtMemberS.Text = "";
                                DialogResult DSL = MessageBox.Show("តើអ្នកច្បាស់ទេដែលចង់ចុះឈ្មោះសម្រាប់តួនាទីនេះ?\n" + Details +
                                                            "\n • តួនាទី \t\t : " + Position, MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (DSL == DialogResult.Yes)
                                {
                                    if (dtChecking.Rows.Count == 0)
                                        AddEmployee(0, SubIDCard, IDCard, NFC, EngName, KhName, Sex, StartedWork);
                                    else
                                        AddEmployee(1, SubIDCard, IDCard, NFC, EngName, KhName, Sex, StartedWork);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("មិនអាចចុះឈ្មោះខ្លួនឯងបានទេ!\n" + Details, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtMemberS.SelectAll();
                        }
                    }
                    else
                    {

                        MessageBox.Show("គ្មានទិន្នន័យនេះនៅក្នុងប្រព័ន្ធទេ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMemberS.SelectAll();
                    }
                }
                else
                {
                    MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void TxtNFC_TextChanged(object sender, EventArgs e)
        {
            if(txtNFC.Text.Trim()!="")
                chkNFC.Checked = true;
            else
                chkNFC.Checked = false;
        }
        private void TxtEngName_TextChanged(object sender, EventArgs e)
        {
            if(txtEngName.Text.Trim()!="")
                chkEngName.Checked = true;
            else
                chkEngName.Checked = false;
        }
        private void TxtIDCard_TextChanged(object sender, EventArgs e)
        {
            if (txtIDCard.Text.Trim() != "")
                chkIDCard.Checked = true;
            else
                chkIDCard.Checked = false;
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            DgvSearch.Rows.Clear();
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();

            DataTable dtSQLCond = new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Val");
            if (chkIDCard.Checked == true && txtIDCard.Text.Trim() != "")
            {
                string SearchValue = txtIDCard.Text.ToUpper();
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*","");
                }
                dtSQLCond.Rows.Add("SubL_IDNo LIKE ", "'%"+SearchValue+"%'");
            }
            if (chkEngName.Checked == true && txtEngName.Text.Trim() != "")
            {
                string SearchValue = txtEngName.Text;
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*", "");
                }
                dtSQLCond.Rows.Add("Name LIKE ", "'%" + SearchValue + "%'");
            }
            if (chkKhName.Checked == true && txtKhName.Text.Trim() != "")
            {
                string SearchValue = txtKhName.Text;
                if (SearchValue.Contains("*") == true)
                {
                    SearchValue = SearchValue.Replace("*", "");
                }
                dtSQLCond.Rows.Add("NameKH LIKE ", "N'%" + SearchValue + "%'");
            }
            if (chkNFC.Checked == true && txtNFC.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("NFC_ID = ", "'" + txtNFC.Text + "'");
            }
            string SQLConds = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                if (SQLConds.Trim() == "")
                    SQLConds = "WHERE " + row[0] + row[1];
                else
                    SQLConds += " AND " + row[0] + row[1];
            }

            //Taking Emplyee
            DataTable dtSubLeader = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT SubL_IDNo, Name, NameKH, Sex, StartDate, Position, NFC_ID, MstSubL.RegDate, " +
                    "\nCASE " +
                    "\n\tWHEN tbReg.Username IS NOT NULL THEN tbReg.Username " +
                    "\n\tELSE MstSubL.RegBy COLLATE Khmer_100_CI_AI_SC_UTF8 " +
                    "\nEND AS RegBy, MstSubL.UpdateDate, " +
                    "\nCASE " +
                    "\n\tWHEN tbUp.Username IS NOT NULL THEN tbUp.Username " +
                    "\n\tELSE MstSubL.UpdateBy COLLATE Khmer_100_CI_AI_SC_UTF8 " +
                    "\nEND AS UpdateBy, StaffNameKH FROM MstSubL " +
                    "\nLEFT JOIN (SELECT * FROM tbUser) tbReg ON tbReg.ID = MstSubL.RegBy " +
                    "\nLEFT JOIN (SELECT * FROM tbUser) tbUp ON tbUp.ID = MstSubL.UpdateBy " + SQLConds +
                    "\nORDER BY SubL_IDNo ASC";
                //Console.WriteLine(SQLQuery);
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtSubLeader);
            }
            catch (Exception ex)
            {
                ErrorText = "Taking Emplyee : " + ex.Message;
            }
            cnn.con.Close();

            //Add to Dgv
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtSubLeader.Rows)
                {
                    DgvSearch.Rows.Add();
                    DgvSearch.Rows[DgvSearch.Rows.Count - 1].Cells["IDCard"].Value = row["SubL_IDNo"];
                    DgvSearch.Rows[DgvSearch.Rows.Count - 1].Cells["EngName"].Value = row["Name"];
                    DgvSearch.Rows[DgvSearch.Rows.Count - 1].Cells["KhName"].Value = row["NameKH"];
                    if(row["StartDate"].ToString().Trim()!="")
                        DgvSearch.Rows[DgvSearch.Rows.Count - 1].Cells["StartWork"].Value = Convert.ToDateTime(row["StartDate"]);
                    DgvSearch.Rows[DgvSearch.Rows.Count - 1].Cells["Position"].Value = row["Position"];
                    if (row["Sex"].ToString() == "Male")
                        DgvSearch.Rows[DgvSearch.Rows.Count - 1].Cells["SubLSex"].Value = "ប្រុស";
                    else
                        DgvSearch.Rows[DgvSearch.Rows.Count - 1].Cells["SubLSex"].Value = "ស្រី";
                    DgvSearch.Rows[DgvSearch.Rows.Count - 1].Cells["SubLNFC"].Value = row["NFC_ID"];
                    if (row["StaffNameKH"].ToString().Trim() != "") 
                        DgvSearch.Rows[DgvSearch.Rows.Count - 1].Cells["ControlByStaff"].Value = row["StaffNameKH"].ToString();
                    DgvSearch.Rows[DgvSearch.Rows.Count - 1].Cells["SubLRegDate"].Value = Convert.ToDateTime(row["RegDate"]);
                    DgvSearch.Rows[DgvSearch.Rows.Count - 1].Cells["SubLRegBy"].Value = row["RegBy"];
                    DgvSearch.Rows[DgvSearch.Rows.Count - 1].Cells["SubLUpdateDate"].Value = Convert.ToDateTime(row["UpdateDate"]);
                    DgvSearch.Rows[DgvSearch.Rows.Count - 1].Cells["SubLUpdateBy"].Value = row["UpdateBy"];

                }
                DgvSearch.ClearSelection();
                DgvSearch.CurrentCell = null;
            }

            Cursor = Cursors.Default;   

            if (ErrorText.Trim() == "")
            {
                ShowOrHideTabEdit();
                LbStatus.Text = "រកឃើញទិន្នន័យ "+DgvSearch.Rows.Count.ToString("N0");
                LbStatus.Refresh();
            }
            else
            {
                LbStatus.Text = "មានបញ្ហា!";
                LbStatus.Refresh();
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SubLeaderRegisterForm_Shown(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;

            dgvMember.RowHeadersDefaultCellStyle.Font = new Font(dgvMember.RowHeadersDefaultCellStyle.Font, FontStyle.Regular);
            dgvMember.Columns[dgvMember.Columns.Count - 1].Resizable = DataGridViewTriState.False;
            dgvMember.Columns[dgvMember.Columns.Count - 1].SortMode = DataGridViewColumnSortMode.NotSortable;            
            dgvRegProcess.RowHeadersDefaultCellStyle.Font = new Font(dgvRegProcess.RowHeadersDefaultCellStyle.Font, FontStyle.Regular);
            foreach (DataGridViewColumn col in dgvMstProcess.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            ShowOrHideTabEdit();
            cboMemberSType.SelectedIndex = 0;
            foreach (DataGridViewColumn col in DgvSearch.Columns)
            {
                //Console.WriteLine(col.Name);
            }

            //Taking MstItem
            try
            {
                cnnOBS.conOBS.Open();
                string SQLQuery = "SELECT ItemCode, ItemName, Remark3 FROM mstitem WHERE DelFlag = 0 AND ItemType = 0 " +
                    "\nORDER BY ItemCode ASC";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnOBS.conOBS);
                sda.Fill(dtOBSMstItems);
            }
            catch (Exception ex)
            {
                ErrorText = "Taking MstItem : " + ex.Message;
            }
            cnnOBS.conOBS.Close();

            //Taking MstProcess & LunchShift
            //if (ErrorText.Trim() == "")
            //{
            //    try
            //    {
            //        cnn.con.Open();
            //        string SQLQuery = "SELECT * FROM MstProcess WHERE Status = 'Active' " +
            //            "\nORDER BY RegDate ASC";
            //        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
            //        DataTable dt = new DataTable();
            //        sda.Fill(dt);
            //        foreach (DataRow row in dt.Rows)
            //        {
            //            dgvMstProcess.Rows.Add();
            //            dgvMstProcess.Rows[dgvMstProcess.Rows.Count - 1].Cells["MstProcessCode"].Value = row["ProcessSysNo"].ToString();
            //            dgvMstProcess.Rows[dgvMstProcess.Rows.Count - 1].Cells["MstProcess"].Value = row["ProcessName"].ToString();
            //            dgvMstProcess.Rows[dgvMstProcess.Rows.Count - 1].Cells["PRemark"].Value = row["Remark"].ToString();
            //            dgvMstProcess.Rows[dgvMstProcess.Rows.Count - 1].Cells["RegStatus"].Value = false;
            //        }
            //        dgvMstProcess.ClearSelection();

            //    }
            //    catch (Exception ex)
            //    {
            //        ErrorText = "Taking MstProcess & LunchShift : " + ex.Message;
            //    }
            //    cnn.con.Close();
            //}

            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
            {
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Method
        private void txtKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                btnSearch.PerformClick();
        }
        private void ShowOrHideTabEdit()
        {
            if (DgvSearch.CurrentCell != null && DgvSearch.CurrentCell.RowIndex >= 0)
            {
                splitterBody.Visible = true;
                tabContrlEdit.Visible = true;
            }
            else
            {

                tabContrlEdit.Visible = false;
                splitterBody.Visible = false;
            }
        }
        private void TakingSubLDetails()
        {
            if (DgvSearch.CurrentCell != null && DgvSearch.CurrentCell.RowIndex >= 0 && DgvSearch.Rows[DgvSearch.CurrentCell.RowIndex].Cells["IDCard"].Value!=null)
            {
                dgvMember.Rows.Clear();
                dgvRegProcess.Rows.Clear();
                foreach (DataGridViewRow row in dgvMstProcess.Rows)
                {
                    row.Cells["RegStatus"].Value = false;
                }

                string SubIDCard = DgvSearch.Rows[DgvSearch.CurrentCell.RowIndex].Cells["IDCard"].Value.ToString();

                //Taking Member & FG & Process
                DataTable dtMembers = new DataTable();
                DataTable dtProcess = new DataTable();
                try
                {
                    cnn.con.Open();
                    string SQLQuery = "SELECT MstSubLMember.*, Username FROM MstSubLMember " +
                        "\nLEFT JOIN tbUser ON MstSubLMember.RegBy=tbUser.ID " +
                        "\nWHERE SubL_IDNo = '"+SubIDCard+"' " +
                        "\nORDER BY MstSubLMember.RegDate ASC";
                    SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    sda.Fill(dtMembers);

                    ////SQLQuery = "SELECT MstSubLProcess.*, Username, ProcessName, Remark FROM MstSubLProcess " +
                    ////    "\nLEFT JOIN tbUser ON MstSubLProcess.RegBy=tbUser.ID " +
                    ////    "\nINNER JOIN MstProcess ON MstSubLProcess.ProcessSysNo=MstProcess.ProcessSysNo " +
                    ////    "\nWHERE SubL_IDNo = '"+SubIDCard+"' " +
                    ////    "\nORDER BY MstSubLProcess.RegDate ASC";
                    ////Console.WriteLine(SQLQuery);
                    //sda = new SqlDataAdapter(SQLQuery, cnn.con);
                    //sda.Fill(dtProcess);
                }
                catch(Exception ex)
                {
                    ErrorText = "Taking Member & FG & Process : " + ex.Message;
                }
                cnn.con.Close();

                //Add to dgv
                if (ErrorText.Trim() == "")
                {
                    foreach (DataRow row in dtMembers.Rows)
                    {
                        dgvMember.Rows.Add();
                        dgvMember.Rows[dgvMember.Rows.Count - 1].HeaderCell.Value = dgvMember.Rows.Count.ToString();
                        dgvMember.Rows[dgvMember.Rows.Count-1].Cells["MIDCard"].Value = row["Memb_IDNo"];
                        dgvMember.Rows[dgvMember.Rows.Count - 1].Cells["MEngName"].Value = row["Memb_EngName"];
                        dgvMember.Rows[dgvMember.Rows.Count - 1].Cells["MKhName"].Value = row["Memb_KhName"];
                        if(row["Sex"].ToString() == "Male")
                            dgvMember.Rows[dgvMember.Rows.Count - 1].Cells["Sex"].Value = "ប្រុស";
                        else
                            dgvMember.Rows[dgvMember.Rows.Count - 1].Cells["Sex"].Value = "ស្រី";
                        dgvMember.Rows[dgvMember.Rows.Count - 1].Cells["MStartWork"].Value = Convert.ToDateTime(row["Memb_StartWork"]);
                        if (row["Username"].ToString().Trim() != "")
                            dgvMember.Rows[dgvMember.Rows.Count - 1].Cells["MRegBy"].Value = row["Username"];
                        else
                            dgvMember.Rows[dgvMember.Rows.Count - 1].Cells["MRegBy"].Value = row["RegBy"];
                        dgvMember.Rows[dgvMember.Rows.Count - 1].Cells["MRegDate"].Value = Convert.ToDateTime(row["RegDate"]);
                    }                    
                    foreach (DataRow row in dtProcess.Rows)
                    {
                        dgvRegProcess.Rows.Add();
                        dgvRegProcess.Rows[dgvRegProcess.Rows.Count - 1].HeaderCell.Value = dgvRegProcess.Rows.Count.ToString();
                        dgvRegProcess.Rows[dgvRegProcess.Rows.Count - 1].Cells["ProcessCode"].Value = row["ProcessSysNo"];
                        dgvRegProcess.Rows[dgvRegProcess.Rows.Count - 1].Cells["Process"].Value = row["ProcessName"];
                        dgvRegProcess.Rows[dgvRegProcess.Rows.Count - 1].Cells["Remark"].Value = row["Remark"];
                        if (row["Username"].ToString().Trim() != "")
                            dgvRegProcess.Rows[dgvRegProcess.Rows.Count - 1].Cells["PRegBy"].Value = row["Username"];
                        else
                            dgvRegProcess.Rows[dgvRegProcess.Rows.Count - 1].Cells["PRegBy"].Value = row["RegBy"];
                        dgvRegProcess.Rows[dgvRegProcess.Rows.Count - 1].Cells["PRegDate"].Value = Convert.ToDateTime(row["RegDate"]);

                        foreach (DataGridViewRow dgvRow in dgvMstProcess.Rows)
                        {
                            if (dgvRow.Cells["MstProcessCode"].Value.ToString() == row["ProcessSysNo"].ToString())
                            {
                                dgvRow.Cells["RegStatus"].Value = true;
                                break;
                            }
                        }
                    }
                    dgvRegProcess.ClearSelection();
                    dgvMstProcess.ClearSelection();
                    dgvMember.ClearSelection();
                }

                if (ErrorText.Trim() != "")
                {
                    MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void AddEmployee(int already, string SubIDNo, string Memb_IDNo, string Memb_NFC, string Memb_EngName, string Memb_KhName, string Sex, DateTime SDate)
        {
            Cursor = Cursors.WaitCursor;
            ErrorText = "";
            DateTime RegDate = DateTime.Now;

            //Insert to DB
            try
            {
                cnn.con.Open();
                //Delete if already have
                string SQLQuery = "DELETE FROM MstSubLMember WHERE Memb_IDNo = @Mid";
                SqlCommand cmd = new SqlCommand(SQLQuery, cnn.con);
                if (already != 0)
                {
                    cmd.Parameters.AddWithValue("@Mid", Memb_IDNo);
                    cmd.ExecuteNonQuery();
                }

                //Insert
                SQLQuery = "INSERT INTO MstSubLMember (SubL_IDNo, Memb_IDNo, Memb_NFC, Memb_EngName, Memb_KhName, Sex, Memb_StartWork, RegBy, RegDate) " +
                                                "\nVALUES (@Sid, @Mid, @Mnfc, @MenN, @MkhN, @sex, @Msd, @RegB, @RegD)";
                cmd = new SqlCommand(SQLQuery, cnn.con);
                cmd.Parameters.AddWithValue("@Sid", SubIDNo);
                cmd.Parameters.AddWithValue("@Mid", Memb_IDNo);
                if(Memb_NFC.Trim()!="")
                    cmd.Parameters.AddWithValue("@Mnfc", Memb_NFC);
                else
                    cmd.Parameters.AddWithValue("@Mnfc", DBNull.Value);
                cmd.Parameters.AddWithValue("@MenN", Memb_EngName);
                cmd.Parameters.AddWithValue("@MkhN", Memb_KhName);
                cmd.Parameters.AddWithValue("@sex", Sex);
                cmd.Parameters.AddWithValue("@Msd", SDate);
                cmd.Parameters.AddWithValue("@RegB", MenuFormV2.UserForNextForm);
                cmd.Parameters.AddWithValue("@RegD", RegDate);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrorText = "Insert to DB : " + ex.Message;
            }
            cnn.con.Close();

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                dgvMember.Rows.Add();
                dgvMember.Rows[dgvMember.Rows.Count - 1].HeaderCell.Value = dgvMember.Rows.Count.ToString();
                dgvMember.Rows[dgvMember.Rows.Count - 1].Cells["MIDCard"].Value = Memb_IDNo;
                dgvMember.Rows[dgvMember.Rows.Count - 1].Cells["MEngName"].Value = Memb_EngName;
                dgvMember.Rows[dgvMember.Rows.Count - 1].Cells["MKhName"].Value = Memb_KhName;
                if(Sex == "Male")
                    dgvMember.Rows[dgvMember.Rows.Count - 1].Cells["Sex"].Value = "ប្រុស";
                else
                    dgvMember.Rows[dgvMember.Rows.Count - 1].Cells["Sex"].Value = "ស្រី";
                dgvMember.Rows[dgvMember.Rows.Count - 1].Cells["MStartWork"].Value = SDate;
                dgvMember.Rows[dgvMember.Rows.Count - 1].Cells["MRegDate"].Value = RegDate;
                dgvMember.Rows[dgvMember.Rows.Count - 1].Cells["MRegBy"].Value = MenuFormV2.UserForNextForm;
                dgvMember.ClearSelection();
                txtMemberS.Focus();
            }
            else
            {
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AddProcess(string SubIDNo, string ProcessCode, string ProcessName, string Remark)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            DateTime RegDate = DateTime.Now;

            //Add to DB
            try
            {
                cnn.con.Open();
                string SQLQuery = "INSERT INTO MstSubLProcess (SubL_IDNo, ProcessSysNo, RegBy, RegDate) " +
                    "\n VALUES (@Sub, @PNo, @RegB, @RegD) ";
                SqlCommand cmd = new SqlCommand(SQLQuery, cnn.con);
                cmd.Parameters.AddWithValue("@Sub", SubIDNo);
                cmd.Parameters.AddWithValue("@PNo", ProcessCode);
                cmd.Parameters.AddWithValue("@RegB", MenuFormV2.UserForNextForm);
                cmd.Parameters.AddWithValue("@RegD", RegDate);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrorText = "Add to DB : " + ex.Message;
            }
            cnn.con.Close();

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                dgvRegProcess.Rows.Add();
                dgvRegProcess.Rows[dgvRegProcess.Rows.Count-1].HeaderCell.Value = dgvRegProcess.Rows.Count.ToString();
                dgvRegProcess.Rows[dgvRegProcess.Rows.Count - 1].Cells["ProcessCode"].Value = ProcessCode;
                dgvRegProcess.Rows[dgvRegProcess.Rows.Count - 1].Cells["Process"].Value = ProcessName;
                dgvRegProcess.Rows[dgvRegProcess.Rows.Count - 1].Cells["Remark"].Value = Remark;
                dgvRegProcess.Rows[dgvRegProcess.Rows.Count - 1].Cells["PRegBy"].Value = MenuFormV2.UserForNextForm;
                dgvRegProcess.Rows[dgvRegProcess.Rows.Count - 1].Cells["PRegDate"].Value = RegDate;
                dgvRegProcess.ClearSelection();
            }
            else
            {
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void DeleteProcess(string SubIDNo, string ProcessCode)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;

            //Delete From DB
            try
            {
                cnn.con.Open();
                string SQLQuery = "DELETE FROM MstSubLProcess WHERE SubL_IDNo = '"+SubIDNo+"' AND ProcessSysNo = '"+ProcessCode+"' ";
                SqlCommand cmd = new SqlCommand(SQLQuery,cnn.con);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                ErrorText = "Delete From DB : " + ex.Message;
            }
            cnn.con.Close();

            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                //Remove
                foreach (DataGridViewRow row in dgvRegProcess.Rows)
                {
                    if (row.Cells["ProcessCode"].Value.ToString() == ProcessCode)
                    {
                        dgvRegProcess.Rows.RemoveAt(row.Index);
                    }
                }
                //Re-Assign SeqNo
                foreach (DataGridViewRow row in dgvRegProcess.Rows)
                {
                    row.HeaderCell.Value = (row.Index + 1).ToString();
                }
                dgvRegProcess.ClearSelection();
            }
            else
            {
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        
    }
}
