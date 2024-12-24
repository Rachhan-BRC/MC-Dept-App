using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using MachineDeptApp.MsgClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
//using Microsoft.Office.Interop.Excel;

namespace MachineDeptApp.Admin
{
    public partial class UncountableRMMasterForm : Form
    {
        QuestionMsgClass QMsg = new QuestionMsgClass();
        ErrorMsgClass EMsg = new ErrorMsgClass();
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SQLConnect cnn = new SQLConnect();
        SqlCommand cmd;
        DataTable dtRM;
        string AddOrUpdate;
        string SavePath = (Environment.CurrentDirectory).ToString() + @"\Report\RM Label";
        string fName;

        string ErrorText;

        public UncountableRMMasterForm()
        {
            InitializeComponent();
            cnnOBS.Connection();
            cnn.Connection();
            this.Load += UncountableRMMasterForm_Load;

            //Button
            this.btnShowDgvItems.Click += BtnShowDgvItems_Click;
            this.btnNew.Click += BtnNew_Click;
            this.btnClose.Click += BtnClose_Click;
            this.btnUpdate.Click += BtnUpdate_Click;
            this.btnSave.Click += BtnSave_Click;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnExport.Click += BtnExport_Click;
            this.btnPrint.Click += BtnPrint_Click;

            //txt
            this.txtSearchItem.TextChanged += TxtSearchItem_TextChanged;
            this.txtSearchItem.LostFocus += TxtSearchItem_LostFocus;
            this.txtCodeEdit.Leave += TxtCodeEdit_Leave;
            this.txtCodeEdit.KeyDown += TxtCodeEdit_KeyDown;
            this.txtR2orBobbinsW.KeyPress += TxtR2orBobbinsW_KeyPress;
            this.txtR2orBobbinsW.Leave += TxtR2orBobbinsW_Leave;
            this.txtR1orNetW.KeyPress += TxtR1orNetW_KeyPress;
            this.txtR1orNetW.Leave += TxtR1orNetW_Leave;
            this.txtMOQ.KeyPress += TxtMOQ_KeyPress;
            this.txtMOQ.Leave += TxtMOQ_Leave;

            //Dgv
            this.DgvSearchItem.LostFocus += DgvSearchItem_LostFocus;
            this.DgvSearchItem.CellClick += DgvSearchItem_CellClick;
            this.dgvSearchResult.SelectionChanged += DgvSearchResult_SelectionChanged;

            //Cbo
            this.CboTypeEdit.SelectedIndexChanged += CboTypeEdit_SelectedIndexChanged;

        }

        

        private void CboTypeEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CboTypeEdit.Text.Contains("Bobbin") == true || CboTypeEdit.Text.Contains("Reel") == true)
            {
                LbR2orBobbinsW.Visible = true;
                txtR2orBobbinsW.Visible = true;
                LbR1orNetW.Visible = true;
                txtR1orNetW.Visible = true;
                if (CboTypeEdit.Text.Contains("Bobbin") == true)
                {
                    LbR2orBobbinsW.Text = "ទម្ងន់សំបកបូប៊ីន kg";
                    LbR1orNetW.Text = "ទម្ងន់សុទ្ធ(គ្មានសំបក) kg";
                    txtR2orBobbinsW.BackColor = Color.White;
                }
                else
                {
                    LbR2orBobbinsW.Text = "រង្វង់តូច​​ mm";
                    LbR1orNetW.Text = "រង្វង់ធំ mm";
                    txtR2orBobbinsW.BackColor = Color.Bisque;
                }
            }
            else
            {
                LbR2orBobbinsW.Visible=false;
                txtR2orBobbinsW.Visible = false;
                LbR1orNetW.Visible = false;
                txtR1orNetW.Visible = false;
            }
        }


        //Dgv
        private void DgvSearchItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCodeEdit.Text = DgvSearchItem.Rows[e.RowIndex].Cells[0].Value.ToString();
            TakeItemName();
        }
        private void DgvSearchItem_LostFocus(object sender, EventArgs e)
        {
            HidePanelItems();
        }
        private void DgvSearchResult_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSearchResult.SelectedCells.Count > 0)
            {
                if (dgvSearchResult.CurrentCell.RowIndex > -1)
                {
                    CboTypeEdit.Text = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[3].Value.ToString();
                    txtCodeEdit.Text = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[0].Value.ToString();
                    txtNameEdit.Text = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[1].Value.ToString();
                    if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[4].Value != null)
                    {
                        txtR2orBobbinsW.Text = Convert.ToDouble(dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[4].Value.ToString()).ToString("N2");
                    }
                    else
                    {
                        txtR2orBobbinsW.Text = "";
                    }                    
                    txtR1orNetW.Text = Convert.ToDouble(dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[5].Value.ToString()).ToString("N2");
                    txtMOQ.Text = Convert.ToDouble(dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[6].Value.ToString()).ToString("N0");
                }
            }
            CheckBtnUpdateDel();
        }

        //txt
        private void TxtSearchItem_LostFocus(object sender, EventArgs e)
        {
            HidePanelItems();
        }
        private void TxtSearchItem_TextChanged(object sender, EventArgs e)
        {
            DgvSearchItem.Rows.Clear();
            if (txtSearchItem.Text.Trim() == "")
            {
                foreach (DataRow row in dtRM.Rows)
                {
                    DgvSearchItem.Rows.Add(row[0].ToString(), row[1].ToString());
                }
            }
            else
            {

                foreach (DataRow row in dtRM.Rows)
                {
                    int Found = 0;
                    if (row[0].ToString().ToLower().Contains(txtSearchItem.Text.ToString().ToLower()) == true || row[1].ToString().ToLower().Contains(txtSearchItem.Text.ToString().ToLower()) == true)
                    {
                        Found = Found + 1;
                    }
                    if (Found > 0)
                    {
                        DgvSearchItem.Rows.Add(row[0].ToString(), row[1].ToString());
                    }
                }
            }
            if (DgvSearchItem.Rows.Count > 8)
            {
                DgvSearchItem.Columns[1].Width = 336 - 16;
            }
            else
            {
                DgvSearchItem.Columns[1].Width = 338;
            }
        }
        private void TxtCodeEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtCodeEdit.Text.Trim() != "")
                {
                    TakeItemName();
                }
            }
        }
        private void TxtCodeEdit_Leave(object sender, EventArgs e)
        {
            if (txtCodeEdit.Text.Trim() != "")
            {
                TakeItemName();
            }
        }
        private void TxtR1orNetW_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        private void TxtR2orBobbinsW_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        private void TxtMOQ_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void TxtR1orNetW_Leave(object sender, EventArgs e)
        {
            try
            {
                double InputNumber = Convert.ToDouble(txtR1orNetW.Text);
                InputNumber = Math.Round(InputNumber, 3);
                txtR1orNetW.Text = InputNumber.ToString("N2");
            }
            catch
            {
                txtR1orNetW.Text = "";
            }
        }
        private void TxtR2orBobbinsW_Leave(object sender, EventArgs e)
        {
            try
            {
                double InputNumber = Convert.ToDouble(txtR2orBobbinsW.Text);
                InputNumber = Math.Round(InputNumber, 3);
                txtR2orBobbinsW.Text = InputNumber.ToString("N2");
            }
            catch
            {
                txtR2orBobbinsW.Text = "";
            }
        }
        private void TxtMOQ_Leave(object sender, EventArgs e)
        {
            try
            {
                double InputNumber = Convert.ToDouble(txtMOQ.Text);
                InputNumber = Math.Round(InputNumber, 1);
                txtMOQ.Text = InputNumber.ToString("N0");
            }
            catch
            {
                txtMOQ.Text = "";
            }
        }

        //Button
        private void BtnShowDgvItems_Click(object sender, EventArgs e)
        {
            DgvSearchItem.Rows.Clear();
            int PanelItemY = tabContrlEdit.Location.Y+93 + txtNameEdit.Location.Y - 5;
            if (dtRM.Rows.Count > 0)
            {
                if (dtRM.Rows.Count > 8)
                {
                    txtSearchItem.Visible = true;
                    panelSearchItem.Size = new Size(355, 247);
                    panelSearchItem.Location = new Point(153, PanelItemY - panelSearchItem.Height);
                }
                else
                {
                    int HeightofDgv = dtRM.Rows.Count * 26;
                    txtSearchItem.Visible = false;
                    panelSearchItem.Size = new Size(355, HeightofDgv);
                    panelSearchItem.Location = new Point(153, PanelItemY - panelSearchItem.Height);
                }
                foreach (DataRow row in dtRM.Rows)
                {
                    DgvSearchItem.Rows.Add(row[0].ToString(), row[1].ToString());
                }
            }
            DgvSearchItem.ClearSelection();
            panelSearchItem.BringToFront();
            DgvSearchItem.Focus();

        }
        private void BtnClose_Click(object sender, EventArgs e)
        {
            HideEditTabContrl();
            btnSearch.Enabled = true;
            btnNew.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            CheckBtnUpdateDel();
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearAdd();
            tabContrlEdit.TabPages[0].Text = "បន្ថែមវត្ថុធាតុដើម";
            AddOrUpdate = "1";
            ReadOnlySet();
            toolTip1.SetToolTip(btnSave, "រក្សាទុក");
            ShowEditTabContrl();
            btnSearch.Enabled = false;
            btnNew.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            txtCodeEdit.Focus();
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            tabContrlEdit.TabPages[0].Text = "អាប់ដេតវត្ថុធាតុដើម";
            AddOrUpdate = "2";
            ReadOnlySet();
            toolTip1.SetToolTip(btnSave, "អាប់ដេត");
            ShowEditTabContrl();
            btnSearch.Enabled = false;
            btnNew.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            txtCodeEdit.Focus();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (AddOrUpdate == "1")
            {
                SaveToDB();
            }
            else
            {
                UpdateToDB();
            }
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();

            //Take MstItem From OBS
            dgvSearchResult.Rows.Clear();
            try
            {
                cnnOBS.conOBS.Open();
                dtRM = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, MatTypeName FROM " +
                                                                                "(SELECT * FROM mstitem WHERE DelFlag=0 AND MatCalcFlag=1) T1 " +
                                                                                "INNER JOIN " +
                                                                                "(SELECT * FROM MstMatType WHERE DelFlag=0) T2 " +
                                                                                "ON T1.MatTypeCode=T2.MatTypeCode", cnnOBS.conOBS);
                sda.Fill(dtRM);

                DataTable dtRMType = new DataTable();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnnOBS.conOBS.Close();

            DataTable dtSQLConds = new DataTable();
            dtSQLConds.Columns.Add("Columns");
            dtSQLConds.Columns.Add("Value");

            if (txtCode.Text.Trim() != "")
            {
                dtSQLConds.Rows.Add("Code = ", "'" + txtCode.Text.ToString() + "' ");
            }
            if (CboRMType.Text.Trim() != "")
            {
                dtSQLConds.Rows.Add("RMType = ", "'" + CboRMType.Text.ToString() + "' ");
            }
            if (CboType.Text.Trim() != "")
            {
                dtSQLConds.Rows.Add("BobbinsOrReil = ", "'" + CboType.Text.ToString() + "' ");
            }

            string SQLConds = "";
            foreach (DataRow row in dtSQLConds.Rows)
            {
                if (SQLConds.Trim() == "")
                {
                    SQLConds = "WHERE " + row[0].ToString() + row[1].ToString();
                }
                else
                {
                    SQLConds = SQLConds + "AND " + row[0].ToString() + row[1].ToString();
                }
            }

            //Find Record from DB
            try
            {
                cnn.con.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM tbSDMstUncountMat " + SQLConds + "ORDER BY Code ASC", cnn.con);
                sda1.Fill(dt);
                dt.Columns.Add("ItemName");
                dt.AcceptChanges();
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataRow rowOBS in dtRM.Rows)
                    {
                        if (row[0].ToString() == rowOBS[0].ToString())
                        {
                            row[10] = rowOBS[1].ToString();
                            dt.AcceptChanges();
                            break;
                        }
                    }
                }
                if (txtName.Text.Trim() != "")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string Code = row[0].ToString();
                        string Name = row[10].ToString();
                        string RMType = row[1].ToString();
                        string BobbinsOrReil = row[2].ToString();
                        Nullable<double> R2 = null;
                        if (row[3].ToString().Trim() != "")
                        {
                            R2 = Convert.ToDouble(row[3].ToString());
                        }
                        double R1 = Convert.ToDouble(row[4].ToString());
                        double MOQ = Convert.ToDouble(row[5].ToString());
                        DateTime RegDate = Convert.ToDateTime(row[6].ToString());
                        string RegBy = row[7].ToString();
                        DateTime UpDate = Convert.ToDateTime(row[8].ToString());
                        string UpBy = row[9].ToString();

                        if (Name.ToLower().Contains(txtName.Text.ToLower()))
                        {
                            dgvSearchResult.Rows.Add(Code, Name, RMType, BobbinsOrReil, R2, R1, MOQ, RegDate, RegBy, UpDate, UpBy);
                        }
                    }
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string Code = row[0].ToString();
                        string Name = row[10].ToString();
                        string RMType = row[1].ToString();
                        string BobbinsOrReil = row[2].ToString();
                        Nullable<double> R2 = null;
                        if (row[3].ToString().Trim() != "")
                        {
                            R2 = Convert.ToDouble(row[3].ToString());
                        }
                        double R1 = Convert.ToDouble(row[4].ToString());
                        double MOQ = Convert.ToDouble(row[5].ToString());
                        DateTime RegDate = Convert.ToDateTime(row[6].ToString());
                        string RegBy = row[7].ToString();
                        DateTime UpDate = Convert.ToDateTime(row[8].ToString());
                        string UpBy = row[9].ToString();

                        dgvSearchResult.Rows.Add(Code, Name, RMType, BobbinsOrReil, R2, R1, MOQ, RegDate, RegBy, UpDate, UpBy);
                    }
                }
                dgvSearchResult.ClearSelection();
                Cursor = Cursors.Default;
                LbStatus.Text = "រកឃើញទិន្នន័យ " + dgvSearchResult.Rows.Count.ToString("N0");
                LbStatus.Refresh();
                dgvSearchResult.CurrentCell = null;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvSearchResult.SelectedCells.Count > 0)
            {
                if (dgvSearchResult.CurrentCell.RowIndex > -1)
                {
                    DialogResult DSL = MessageBox.Show("តើអ្នកចង់លុបទិន្នន័យនេះមែនឬទេ?","Rachhan System",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                    if (DSL == DialogResult.Yes)
                    {
                        try
                        {
                            cnn.con.Open();
                            string query = "DELETE FROM tbSDMstUncountMat WHERE Code='" + dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[0].Value.ToString() + "' ";
                            cmd = new SqlCommand(query, cnn.con);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("លុបទិន្នន័យរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvSearchResult.Rows.RemoveAt(dgvSearchResult.CurrentCell.RowIndex);
                            dgvSearchResult.Refresh();
                            dgvSearchResult.ClearSelection();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        cnn.con.Close();
                    }
                }
            }
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvSearchResult.Rows.Count > 0)
            {
                DialogResult DLS = MessageBox.Show("តើអ្នកចង់ទាញទិន្នន័យចេញមែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLS == DialogResult.Yes)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "CSV file (*.csv)|*.csv";
                    saveDialog.FileName = "Master_UncountableRM.csv";
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
                                    string ColHeaderText = dgvSearchResult.Columns[i].HeaderText.ToString();
                                    ColHeaderText = ColHeaderText.Replace("\n"," | ");
                                    columnNames += ColHeaderText + ",";
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
                            MessageBox.Show("ទាញទិន្នន័យចេញរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            Cursor = Cursors.Default;
                            MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (dgvSearchResult.SelectedCells.Count > 0 && dgvSearchResult.CurrentCell != null && dgvSearchResult.CurrentCell.RowIndex > -1)
            {
                QMsg.QAText = "តើអ្នកចង់ព្រីនឡាប៊ែលមែនឬទេ?";
                QMsg.UserClickedYes = false;
                QMsg.ShowingMsg();
                if (QMsg.UserClickedYes == true)
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;
                    string RMCode = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[0].Value.ToString();
                    string RMName = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[1].Value.ToString();
                    string MOQ = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[6].Value.ToString();
                    string RMType = dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[2].Value.ToString();
                    string Maker = "";
                    DataTable dt = new DataTable();
                    try
                    {
                        cnnOBS.conOBS.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT Resv1 FROM mstitem WHERE ItemCode='" + RMCode +"'", cnnOBS.conOBS);
                        sda.Fill(dt);
                        Maker = dt.Rows[0]["Resv1"].ToString();
                    }
                    catch (Exception ex)
                    {
                        ErrorText = "MC DB : " + ex.Message;
                    }
                    cnnOBS.conOBS.Close();

                    var CDirectory = Environment.CurrentDirectory;
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(Filename: CDirectory.ToString() + @"\Template\RMLabel_Template.xlsx", Editable: true);

                    try
                    {
                        Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkBook.Sheets["LabelTemplate"];
                        worksheet.Cells[1, 1] = RMName;
                        worksheet.Cells[2, 1] = "*" + RMCode + "*";
                        worksheet.Cells[3, 2] = RMType;
                        worksheet.Cells[3, 3] = Maker;
                        worksheet.Cells[3, 4] = MOQ;

                        //Rename Sheet
                        worksheet.Name = "RachhanSystem";

                        //ឆែករកមើល Folder បើគ្មាន => បង្កើត            
                        if (!Directory.Exists(SavePath))
                        {
                            Directory.CreateDirectory(SavePath);
                        }

                        // Saving the modified Excel file                        
                        string date = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                        string file = RMCode;
                        fName = file + " ( " + date + " )";
                        worksheet.PrintOut();
                        worksheet.SaveAs(SavePath + @"\" + fName + ".xlsx");
                        xlWorkBook.Save();
                        xlWorkBook.Close();
                        excelApp.Quit();
                    }
                    catch (Exception ex)
                    {
                        ErrorText = "មានបញ្ហា!\n" + ex.Message;
                        excelApp.DisplayAlerts = false;
                        xlWorkBook.Save();
                        xlWorkBook.Close();
                        excelApp.Quit();
                        excelApp.DisplayAlerts = true;
                    }

                    //Kill all Excel background process
                    var processes = from p in Process.GetProcessesByName("EXCEL")
                                    select p;
                    foreach (var process in processes)
                    {
                        if (process.MainWindowTitle.ToString().Trim() == "")
                            process.Kill();
                    }

                    Cursor = Cursors.Default;

                }
            }
        }


        private void UncountableRMMasterForm_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn dgvCol in dgvSearchResult.Columns)
            {
                string Text = dgvCol.HeaderText.ToString();
                Text = Text.Replace("/","\n");
                dgvCol.HeaderText = Text;
            }
            DataTable dtType = new DataTable();
            dtType.Columns.Add("TypeName");
            dtType.Rows.Add("Bobbin");
            dtType.Rows.Add("Reel");
            foreach (DataRow row in dtType.Rows)
            {
                CboType.Items.Add(row[0].ToString()); 
                CboTypeEdit.Items.Add(row[0].ToString());
            }
            CboTypeEdit.SelectedIndex = 0;

            try
            {
                cnnOBS.conOBS.Open();
                dtRM = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemCode, ItemName, MatTypeName FROM " +
                                                                                "(SELECT * FROM mstitem WHERE DelFlag=0 AND MatCalcFlag=1) T1 " +
                                                                                "INNER JOIN " +
                                                                                "(SELECT * FROM MstMatType WHERE DelFlag=0) T2 " +
                                                                                "ON T1.MatTypeCode=T2.MatTypeCode", cnnOBS.conOBS);
                sda.Fill(dtRM); 
                
                DataTable dtRMType = new DataTable();
                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT T1.MatTypeCode, MatTypeName FROM " +
                                                                                    "(SELECT MatTypeCode FROM mstitem WHERE DelFlag=0 AND ItemType=2 AND MatCalcFlag=1 AND NOT MatTypeCode IS NULL GROUP BY MatTypeCode) T1 " +
                                                                                    "INNER JOIN (SELECT * FROM MstMatType WHERE DelFlag=0) T2 " +
                                                                                    "ON T1.MatTypeCode=T2.MatTypeCode ORDER BY MatTypeName ASC", cnnOBS.conOBS);
                sda1.Fill(dtRMType);
                foreach (DataRow row in dtRMType.Rows)
                {
                    CboRMType.Items.Add(row[1].ToString());
                }

                HideEditTabContrl();                
            }
            catch(Exception ex )
            {
                MessageBox.Show("មានបញ្ហា!\n"+ex.Message,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            cnnOBS.conOBS.Close();
        }

        //Function
        private void HidePanelItems()
        {
            if (DgvSearchItem.Focused == false && txtSearchItem.Focused == false)
            {
                panelSearchItem.SendToBack();
            }
        }
        private void TakeItemName()
        {
            txtNameEdit.Text = "";
            foreach (DataRow row in dtRM.Rows)
            {
                if (row[0].ToString() == txtCodeEdit.Text)
                {
                    txtNameEdit.Text = row[1].ToString();
                    break;
                }
            }
            txtMOQ.Focus();
        }
        private void ClearAdd()
        {           
            txtCodeEdit.Text = "";
            txtNameEdit.Text = "";
            txtR2orBobbinsW.Text = "";
            txtR1orNetW.Text = "";
            txtMOQ.Text = "";
        }
        private void ShowEditTabContrl()
        {
            GroupBoxResult.Height = GroupBoxResult.Height - tabContrlEdit.Height;
            tabContrlEdit.Visible = true;
        }
        private void HideEditTabContrl()
        {
            tabContrlEdit.Visible = false;
            GroupBoxResult.Height = GroupBoxResult.Height + tabContrlEdit.Height;
        }
        private void CheckBtnUpdateDel()
        {
            if (dgvSearchResult.SelectedCells.Count > 0 && dgvSearchResult.CurrentCell != null && dgvSearchResult.CurrentCell.RowIndex>-1)
            {
                btnUpdate.Enabled = true;
                btnUpdate.BackColor = Color.White;
                btnDelete.Enabled = true;
                btnDelete.BackColor = Color.White;
                btnPrint.Enabled = true;
                btnPrintGRAY.SendToBack();

            }
            else
            {
                btnUpdate.Enabled = false;
                btnUpdate.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
                btnDelete.Enabled = false;
                btnDelete.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
                btnPrint.Enabled = false;
                btnPrintGRAY.BringToFront();
            }
        }
        private void SaveToDB()
        {
            if (CboTypeEdit.Text.Contains("Bobbin") == true || CboTypeEdit.Text.Contains("Reel") == true)
            {
                //Bobbins
                if (CboTypeEdit.Text.Contains("Bobbin") == true)
                {
                    if (txtCodeEdit.Text.Trim() != "" && txtNameEdit.Text.Trim() != "" && txtR1orNetW.Text.Trim() != "" && txtMOQ.Text.Trim() != "")
                    {
                        string Code = txtCodeEdit.Text;
                        string Name = txtNameEdit.Text;
                        string RMType = "";
                        foreach (DataRow row in dtRM.Rows)
                        {
                            if (row[0].ToString() == Code)
                            {
                                RMType = row[2].ToString();
                                break;
                            }
                        }
                        string BobbinsOrReil = CboTypeEdit.Text;
                        double R2OrBobbinsW = 0;
                        if (txtR2orBobbinsW.Text.Trim() != "")
                        {
                            R2OrBobbinsW = Math.Round(Convert.ToDouble(txtR2orBobbinsW.Text), 2);
                        }
                        double R1OrNetW = Math.Round(Convert.ToDouble(txtR1orNetW.Text), 2);
                        int MOQ = Convert.ToInt32(txtMOQ.Text.Replace(",", ""));
                        DateTime RegDate = DateTime.Now;
                        string RegBy = MenuFormV2.UserForNextForm;
                        if (R2OrBobbinsW < R1OrNetW)
                        {
                            DialogResult DSL = MessageBox.Show("តើអ្នកចង់រក្សាទុកទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (DSL == DialogResult.Yes)
                            {
                                //Check already have or not yet
                                DataTable dtChecking = new DataTable();
                                try
                                {
                                    cnn.con.Open();
                                    SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbSDMstUncountMat WHERE Code='" + txtCodeEdit.Text + "'", cnn.con);
                                    sda.Fill(dtChecking);
                                    if (dtChecking.Rows.Count == 0)
                                    {
                                        cmd = new SqlCommand("INSERT INTO tbSDMstUncountMat (Code, RMType, BobbinsOrReil, R2OrBobbinsW, R1OrNetW, MOQ, RegDate, RegBy, UpdateDate, UpdateBy) " +
                                                                                                                                    "VALUES (@Cd, @Rt, @Br, " + R2OrBobbinsW + ", @R1, @Moq, @Rd, @Rb, @UD, @Ub)", cnn.con);
                                        cmd.Parameters.AddWithValue("@Cd", Code);
                                        cmd.Parameters.AddWithValue("@Rt", RMType);
                                        cmd.Parameters.AddWithValue("@Br", BobbinsOrReil);
                                        cmd.Parameters.AddWithValue("@R1", R1OrNetW);
                                        cmd.Parameters.AddWithValue("@Moq", MOQ);
                                        cmd.Parameters.AddWithValue("@Rd", RegDate.ToString("yyyy-MM-dd HH:mm:ss"));
                                        cmd.Parameters.AddWithValue("@Rb", RegBy);
                                        cmd.Parameters.AddWithValue("@UD", RegDate.ToString("yyyy-MM-dd HH:mm:ss"));
                                        cmd.Parameters.AddWithValue("@Ub", RegBy);
                                        cmd.ExecuteNonQuery();
                                        MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        dgvSearchResult.Rows.Add(Code, Name, RMType, BobbinsOrReil, R2OrBobbinsW, R1OrNetW, MOQ, RegDate, RegBy, RegDate, RegBy);
                                        dgvSearchResult.ClearSelection();
                                        ClearAdd();
                                    }
                                    else
                                    {
                                        MessageBox.Show("វត្ថុធាតុដើមនេះបានបញ្ចូលរួចហើយ!\nប្រសិនបើអ្នកចង់ផ្លាស់ប្ដូរ សូមធ្វើការអាប់ដេត!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                cnn.con.Close();
                            }                            
                        }
                        else
                        {
                            MessageBox.Show("<" + LbR2orBobbinsW.Text + "> ត្រូវតែតូចជាង <" + LbR1orNetW.Text + "> !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        MessageBox.Show("សូមបញ្ចូលនូវព័ត៌មានដែលមានផ្ទាំងក្រោយ\nពណ៌ <ទឹកក្រូច> ទាំងអស់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                //Reel
                else
                {
                    if (txtCodeEdit.Text.Trim() != "" && txtNameEdit.Text.Trim() != "" && txtR2orBobbinsW.Text.Trim() != "" && txtR1orNetW.Text.Trim() != "" && txtMOQ.Text.Trim() != "")
                    {
                        string Code = txtCodeEdit.Text;
                        string Name = txtNameEdit.Text;
                        string RMType = "";
                        foreach (DataRow row in dtRM.Rows)
                        {
                            if (row[0].ToString() == Code)
                            {
                                RMType = row[2].ToString();
                                break;
                            }
                        }
                        string BobbinsOrReil = CboTypeEdit.Text;
                        double R2OrBobbinsW = 0;
                        if (txtR2orBobbinsW.Text.Trim() != "")
                        {
                            R2OrBobbinsW = Math.Round(Convert.ToDouble(txtR2orBobbinsW.Text), 2);
                        }
                        double R1OrNetW = Math.Round(Convert.ToDouble(txtR1orNetW.Text), 2);
                        int MOQ = Convert.ToInt32(txtMOQ.Text.Replace(",", ""));
                        DateTime RegDate = DateTime.Now;
                        string RegBy = MenuFormV2.UserForNextForm;

                        if (R2OrBobbinsW<R1OrNetW)
                        {
                            DialogResult DSL = MessageBox.Show("តើអ្នកចង់រក្សាទុកទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (DSL == DialogResult.Yes)
                            {
                                
                                //Check already have or not yet
                                DataTable dtChecking = new DataTable();
                                try
                                {
                                    cnn.con.Open();
                                    SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbSDMstUncountMat WHERE Code='" + txtCodeEdit.Text + "'", cnn.con);
                                    sda.Fill(dtChecking);
                                    if (dtChecking.Rows.Count == 0)
                                    {
                                        cmd = new SqlCommand("INSERT INTO tbSDMstUncountMat (Code, RMType, BobbinsOrReil, R2OrBobbinsW, R1OrNetW, MOQ, RegDate, RegBy, UpdateDate, UpdateBy) " +
                                                                                                                                    "VALUES (@Cd, @Rt, @Br, " + R2OrBobbinsW + ", @R1, @Moq, @Rd, @Rb, @UD, @Ub)", cnn.con);
                                        cmd.Parameters.AddWithValue("@Cd", Code);
                                        cmd.Parameters.AddWithValue("@Rt", RMType);
                                        cmd.Parameters.AddWithValue("@Br", BobbinsOrReil);
                                        cmd.Parameters.AddWithValue("@R1", R1OrNetW);
                                        cmd.Parameters.AddWithValue("@Moq", MOQ);
                                        cmd.Parameters.AddWithValue("@Rd", RegDate.ToString("yyyy-MM-dd HH:mm:ss"));
                                        cmd.Parameters.AddWithValue("@Rb", RegBy);
                                        cmd.Parameters.AddWithValue("@UD", RegDate.ToString("yyyy-MM-dd HH:mm:ss"));
                                        cmd.Parameters.AddWithValue("@Ub", RegBy);
                                        cmd.ExecuteNonQuery();
                                        MessageBox.Show("រក្សាទុករួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        dgvSearchResult.Rows.Add(Code, Name, RMType, BobbinsOrReil, R2OrBobbinsW, R1OrNetW, MOQ, RegDate, RegBy, RegDate, RegBy);
                                        dgvSearchResult.ClearSelection();
                                        ClearAdd();
                                    }
                                    else
                                    {
                                        MessageBox.Show("វត្ថុធាតុដើមនេះបានបញ្ចូលរួចហើយ!\nប្រសិនបើអ្នកចង់ផ្លាស់ប្ដូរ សូមធ្វើការអាប់ដេត!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                cnn.con.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("<" + LbR2orBobbinsW.Text + "> ត្រូវតែតូចជាង <" + LbR1orNetW.Text + "> !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }                        
                    }
                    else
                    {
                        MessageBox.Show("សូមបញ្ចូលនូវព័ត៌មានដែលមានផ្ទាំងក្រោយ\nពណ៌ <ទឹកក្រូច> ទាំងអស់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }
        private void UpdateToDB()
        {
            //Bobbins
            if (CboTypeEdit.SelectedIndex == 0)
            {
                if (txtCodeEdit.Text.Trim() != "" && txtNameEdit.Text.Trim() != "" && txtR1orNetW.Text.Trim() != "" && txtMOQ.Text.Trim() != "")
                {
                    string Code = txtCodeEdit.Text;
                    string RMType = "";
                    foreach (DataRow row in dtRM.Rows)
                    {
                        if (row[0].ToString() == Code)
                        {
                            RMType = row[2].ToString();
                            break;
                        }
                    }
                    string BobbinsOrReil = CboTypeEdit.Text;
                    double R2OrBobbinsW = 0;
                    if (txtR2orBobbinsW.Text.Trim() != "")
                    {
                        R2OrBobbinsW = Math.Round(Convert.ToDouble(txtR2orBobbinsW.Text), 2);
                    }
                    double R1OrNetW = Math.Round(Convert.ToDouble(txtR1orNetW.Text), 2);
                    int MOQ = Convert.ToInt32(txtMOQ.Text.Replace(",", ""));
                    DateTime UpdateDate = DateTime.Now;
                    string UpdateBy = MenuFormV2.UserForNextForm;

                    if (R2OrBobbinsW < R1OrNetW)
                    {
                        DialogResult DSL = MessageBox.Show("តើអ្នកចង់អាប់ដេតទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (DSL == DialogResult.Yes)
                        {
                            try
                            {
                                cnn.con.Open();
                                string query = "UPDATE tbSDMstUncountMat SET RMType='" + RMType + "', " +
                                                        "BobbinsOrReil='" + BobbinsOrReil + "', " +
                                                        "R2OrBobbinsW=" + R2OrBobbinsW + ", " +
                                                        "R1OrNetW=" + R1OrNetW + ", " +
                                                        "MOQ=" + MOQ + ", " +
                                                        "UpdateDate='" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                        "UpdateBy=N'" + UpdateBy + "' WHERE Code='" + Code + "' ";
                                cmd = new SqlCommand(query, cnn.con);
                                cmd.ExecuteNonQuery();

                                MessageBox.Show("អាប់ដេតទិន្នន័យរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[2].Value = RMType;
                                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[3].Value = BobbinsOrReil;
                                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[4].Value = R2OrBobbinsW;
                                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[5].Value = R1OrNetW;
                                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[6].Value = MOQ;
                                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[9].Value = UpdateDate;
                                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[10].Value = UpdateBy;
                                dgvSearchResult.ClearSelection();
                                ClearAdd();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            cnn.con.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("<" + LbR2orBobbinsW.Text + "> ត្រូវតែតូចជាង <" + LbR1orNetW.Text + "> !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }
                else
                {
                    MessageBox.Show("សូមបញ្ចូលនូវព័ត៌មានដែលមានផ្ទាំងក្រោយ\nពណ៌ <ទឹកក្រូច> ទាំងអស់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            //Reel
            else
            {
                if (txtCodeEdit.Text.Trim() != "" && txtNameEdit.Text.Trim() != "" && txtR2orBobbinsW.Text.Trim() != "" && txtR1orNetW.Text.Trim() != "" && txtMOQ.Text.Trim() != "")
                {
                    string Code = txtCodeEdit.Text;
                    string RMType = "";
                    foreach (DataRow row in dtRM.Rows)
                    {
                        if (row[0].ToString() == Code)
                        {
                            RMType = row[2].ToString();
                            break;
                        }
                    }
                    string BobbinsOrReil = CboTypeEdit.Text;
                    double R2OrBobbinsW = Math.Round(Convert.ToDouble(txtR2orBobbinsW.Text), 2);
                    double R1OrNetW = Math.Round(Convert.ToDouble(txtR1orNetW.Text), 2);
                    int MOQ = Convert.ToInt32(txtMOQ.Text.Replace(",", ""));
                    DateTime UpdateDate = DateTime.Now;
                    string UpdateBy = MenuFormV2.UserForNextForm;

                    if (R2OrBobbinsW<R1OrNetW)
                    {
                        DialogResult DSL = MessageBox.Show("តើអ្នកចង់អាប់ដេតទិន្នន័យនេះមែនឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (DSL == DialogResult.Yes)
                        {
                            try
                            {
                                cnn.con.Open();
                                string query = "UPDATE tbSDMstUncountMat SET RMType='" + RMType + "', " +
                                                        "BobbinsOrReil='" + BobbinsOrReil + "', " +
                                                        "R2OrBobbinsW=" + R2OrBobbinsW + ", " +
                                                        "R1OrNetW=" + R1OrNetW + ", " +
                                                        "MOQ=" + MOQ + ", " +
                                                        "UpdateDate='" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                        "UpdateBy=N'" + UpdateBy + "' WHERE Code='" + Code + "' ";
                                cmd = new SqlCommand(query, cnn.con);
                                cmd.ExecuteNonQuery();

                                MessageBox.Show("អាប់ដេតទិន្នន័យរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[2].Value = RMType;
                                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[3].Value = BobbinsOrReil;
                                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[4].Value = Convert.ToDouble(R2OrBobbinsW);
                                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[5].Value = R1OrNetW;
                                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[6].Value = MOQ;
                                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[9].Value = UpdateDate;
                                dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[10].Value = UpdateBy;

                                dgvSearchResult.ClearSelection();
                                ClearAdd();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            cnn.con.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("<" + LbR2orBobbinsW.Text + "> ត្រូវតែតូចជាង <" + LbR1orNetW.Text + "> !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                    
                }
                else
                {
                    MessageBox.Show("សូមបញ្ចូលនូវព័ត៌មានដែលមានផ្ទាំងក្រោយ\nពណ៌ <ទឹកក្រូច> ទាំងអស់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void ReadOnlySet()
        {
            //New
            if (AddOrUpdate == "1")
            {
                txtCodeEdit.ReadOnly=false;
                txtCodeEdit.BackColor=Color.Bisque;
                txtCodeEdit.ForeColor=Color.Black;
                btnShowDgvItems.Visible=true;

            }
            //Update
            else
            {
                txtCodeEdit.ReadOnly = true;
                txtCodeEdit.BackColor = Color.FromArgb(224, 224, 224);
                txtCodeEdit.ForeColor = Color.Blue;
                btnShowDgvItems.Visible = false;
            }
        }

    }
}
