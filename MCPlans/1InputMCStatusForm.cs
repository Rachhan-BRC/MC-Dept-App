using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MachineDeptApp.MCPlans
{
    public partial class _1InputMCStatusForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        DataTable dtMCName;
        DataTable dtColor;

        string UpdateColName;
        string UpdateValue;
        string UpdatePosCNo;

        public _1InputMCStatusForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Load += _1InputMCStatusForm_Load;

            //Button Click
            this.btnSearch.Click += BtnSearch_Click;
            this.btnOKMCETA.Click += BtnOKMCETA_Click;
            this.btnCancelMCETA.Click += BtnCancelMCETA_Click;

            //Dgv Cell Formatting
            this.dgvSearchResult.CellFormatting += DgvSearchResult_CellFormatting;
            this.dgvMCStatus.CellFormatting += DgvMCStatus_CellFormatting;

            //Dgv Cell Click
            this.dgvSearchResult.CellClick += DgvSearchResult_CellClick;
            this.dgvMCType.CellClick += DgvMCType_CellClick;
            this.dgvMCName.CellClick += DgvMCName_CellClick;
            this.dgvMCStatus.CellClick += DgvMCStatus_CellClick;
            this.dgvLabelStatus.Click += DgvLabelStatus_Click;

            //Dgv Double Cell Click
            this.dgvSearchResult.CellDoubleClick += DgvSearchResult_CellDoubleClick;

            //Dgv Cell Value Changed
            this.dgvSearchResult.CellValueChanged += DgvSearchResult_CellValueChanged;

            //Dgv Focus and Lost Focus
            this.dgvLabelStatus.GotFocus += DgvLabelStatus_GotFocus;
            this.dgvLabelStatus.LostFocus += DgvLabelStatus_LostFocus;
            this.dgvMCStatus.GotFocus += DgvMCStatus_GotFocus;
            this.dgvMCStatus.LostFocus += DgvMCStatus_LostFocus;
            this.dgvMCType.GotFocus += DgvMCType_GotFocus;
            this.dgvMCType.LostFocus += DgvMCType_LostFocus;
            this.dgvMCName.GotFocus += DgvMCName_GotFocus;
            this.dgvMCName.LostFocus += DgvMCName_LostFocus;

            //Dgv Keydown
            this.dgvSearchResult.KeyDown += DgvSearchResult_KeyDown;
            this.dgvMCName.KeyDown += DgvMCName_KeyDown;
            this.dgvMCType.KeyDown += DgvMCType_KeyDown;

            //Cbo Value Changed
            CboMC1.SelectedValueChanged += CboMC1_SelectedValueChanged;

        }

        //Cbo Value Changed
        private void CboMC1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (CboMC1.Text.Trim() != "")
            {
                try
                {
                    cnn.con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT MCName FROM tbMasterMCType WHERE MCType='" + CboMC1.Text + "' ", cnn.con);
                    DataTable dtMCName = new DataTable();
                    sda.Fill(dtMCName);
                    CboMCName.Items.Clear();
                    CboMCName.Items.Add("");
                    foreach (DataRow row in dtMCName.Rows)
                    {
                        CboMCName.Items.Add(row[0].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnn.con.Close();
            }
        }

        //Dgv Keydown
        private void DgvSearchResult_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V && e.Control)
            {
                string MCNameOrDatePasteVal = "";
                MCNameOrDatePasteVal = Clipboard.GetText();
                if (MCNameOrDatePasteVal.Trim() != "")
                { 
                    //Find All Selected cell
                    DataTable dtSelectedCell = new DataTable();
                    dtSelectedCell.Columns.Add("RowIndex");
                    dtSelectedCell.Columns.Add("ColIndex");
                    foreach (DataGridViewCell cell in dgvSearchResult.SelectedCells)
                    {
                        dtSelectedCell.Rows.Add(cell.RowIndex.ToString(), cell.ColumnIndex.ToString());
                    }

                    DataView dataView = new DataView(dtSelectedCell);
                    dataView.Sort = "RowIndex ASC, ColIndex ASC";
                    dtSelectedCell = dataView.ToTable();
                    //MC Name
                    foreach (DataRow row in dtSelectedCell.Rows)
                    {
                        if (row[1].ToString() == "13" || row[1].ToString() == "17" || row[1].ToString() == "21")
                        {
                            int Found = 0;
                            //Find MCType
                            foreach (DataRow CheckRow in dtMCName.Rows)
                            {
                                if (CheckRow[1].ToString() == MCNameOrDatePasteVal && CheckRow[0].ToString()== dgvSearchResult.Rows[Convert.ToInt16(row[0].ToString())].Cells[Convert.ToInt16(row[1].ToString()) - 1].Value.ToString())
                                {
                                    Found = Found + 1;
                                    break;

                                }
                            }
                            if (Found>0)
                            {
                                dgvSearchResult.Rows[Convert.ToInt16(row[0].ToString())].Cells[Convert.ToInt16(row[1].ToString())].Value = MCNameOrDatePasteVal;
                            }
                        }                        
                    }
                    //MC ETA
                    int NotDateFound = 0;
                    foreach (DataRow row in dtSelectedCell.Rows)
                    {
                        if (row[1].ToString() == "15" || row[1].ToString() == "19" || row[1].ToString() == "23")
                        {
                            try
                            {
                                DateTime ETA = Convert.ToDateTime(MCNameOrDatePasteVal);
                                dgvSearchResult.Rows[Convert.ToInt16(row[0].ToString())].Cells[Convert.ToInt16(row[1].ToString())].Value = ETA;
                            }
                            catch
                            {
                                NotDateFound = NotDateFound + 1;
                            }
                        }
                    }
                    if (NotDateFound > 0)
                    {
                        MessageBox.Show("Your pasted value is not Date!","Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    }
                }
            }
            if (e.KeyCode == Keys.Back)
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់សម្អាតទិន្នន័យដែលបានជ្រើសរើសមែនដែរឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    //Find All Selected cell
                    DataTable dtSelectedCell = new DataTable();
                    dtSelectedCell.Columns.Add("RowIndex");
                    dtSelectedCell.Columns.Add("ColIndex");
                    foreach (DataGridViewCell cell in dgvSearchResult.SelectedCells)
                    {
                        dtSelectedCell.Rows.Add(cell.RowIndex.ToString(), cell.ColumnIndex.ToString());
                    }

                    DataView dataView = new DataView(dtSelectedCell);
                    dataView.Sort = "RowIndex ASC, ColIndex ASC";
                    dtSelectedCell = dataView.ToTable();
                    foreach (DataRow row in dtSelectedCell.Rows)
                    {
                        if (row[1].ToString() == "13" || row[1].ToString() == "17" || row[1].ToString() == "21" || row[1].ToString() == "15" || row[1].ToString() == "19" || row[1].ToString() == "23")
                        {
                            dgvSearchResult.Rows[Convert.ToInt16(row[0].ToString())].Cells[Convert.ToInt16(row[1].ToString())].Value = "";
                        }
                    }
                }
            }
        }
        private void DgvMCType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                dgvSearchResult.Focus();
            }
        }
        private void DgvMCName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                dgvSearchResult.Focus();
            }
        }

        //Dgv Double Cell Click
        private void DgvSearchResult_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            panelMCETA.SendToBack();
            if (e.RowIndex > -1)
            {  
                //MCName
                if (e.ColumnIndex == 13 || e.ColumnIndex == 17 || e.ColumnIndex == 21)
                {
                    if (dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString().Trim() != "")
                    {
                        dgvMCName.Rows.Clear();
                        dgvMCName.Rows.Add("");
                        foreach (DataRow row in dtMCName.Rows)
                        {
                            if (dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString() == row[0].ToString())
                            {
                                dgvMCName.Rows.Add(row[1]);
                            }
                        }
                        dgvMCName.CurrentCell = dgvMCName.Rows[0].Cells[0];
                        dgvMCName.ClearSelection();
                        if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value != null)
                        {
                            foreach (DataGridViewRow dgvRow in dgvMCName.Rows)
                            {
                                if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value.ToString() == dgvRow.Cells[0].Value.ToString())
                                {
                                    dgvMCName.CurrentCell = dgvRow.Cells[0];
                                    dgvRow.Cells[0].Selected = true;
                                }
                            }
                        }
                        Rectangle oRectangle = dgvSearchResult.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                        int X = oRectangle.X + 7;
                        int Y = oRectangle.Y + 40;
                        dgvMCName.Location = new Point(X, Y);
                        dgvMCName.Focus();
                    }
                }

                //MC ETA
                if (e.ColumnIndex == 15 || e.ColumnIndex == 19 || e.ColumnIndex == 23)
                {
                    DateTime dtMCETA;
                    if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "")
                        {
                            dtMCETA = Convert.ToDateTime(dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                        }
                        else
                        {
                            dtMCETA = DateTime.Now;
                        }
                    }
                    else
                    {
                        dtMCETA = DateTime.Now;
                    }

                    dtpMCETADate.Value = dtMCETA;
                    dtpMCETATime.Value = dtMCETA;

                    Rectangle oRectangle = dgvSearchResult.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    int X = oRectangle.X - 99;
                    int Y = oRectangle.Y + 141;
                    panelMCETA.Location = new Point(X, Y);
                    panelMCETA.BringToFront();
                    dtpMCETADate.Focus();
                }
            }
        }

        //dgv Cell Formatting
        private void DgvSearchResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //Wire Color
            if (e.ColumnIndex == 6 && e.Value.ToString() != "")
            {
                foreach (DataRow row in dtColor.Rows)
                {
                    if (e.Value.ToString().Contains(row[0].ToString()))
                    {
                        e.CellStyle.ForeColor = Color.FromName(row[1].ToString());
                        e.CellStyle.BackColor = Color.FromName(row[2].ToString());
                    }
                }
            }

            //All editable column
            if (e.ColumnIndex > dgvSearchResult.Columns.Count - 14)
            {
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.Font = new System.Drawing.Font("Khmer OS Battambong", 9, FontStyle.Regular);
            }

            //MC Status
            if (e.ColumnIndex == 13 || e.ColumnIndex == 17 || e.ColumnIndex == 21)
            {
                if (e.Value.ToString() == "STOP")
                {
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.BackColor = Color.Green;
                }
                if (e.Value.ToString() == "RUN")
                {
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.BackColor = Color.Orange;
                }
                if (e.Value.ToString() == "FINISH")
                {
                    e.CellStyle.ForeColor = Color.White;
                    e.CellStyle.BackColor = Color.Red;
                }
            }
        }
        private void DgvMCStatus_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.Value.ToString() == "STOP")
            {
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.BackColor = Color.Green;
            }
            if (e.ColumnIndex == 1 && e.Value.ToString() == "RUN")
            {
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.BackColor = Color.Orange;
            }
            if (e.ColumnIndex == 1 && e.Value.ToString() == "FINISH")
            {
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.BackColor = Color.Red;
            }
        }

        //Dgv Focus and Lost Focus
        private void DgvMCStatus_LostFocus(object sender, EventArgs e)
        {
            dgvMCStatus.SendToBack();
        }
        private void DgvMCStatus_GotFocus(object sender, EventArgs e)
        {
            dgvMCStatus.BringToFront();
        }
        private void DgvLabelStatus_LostFocus(object sender, EventArgs e)
        {
            dgvLabelStatus.SendToBack();
        }
        private void DgvLabelStatus_GotFocus(object sender, EventArgs e)
        {
            dgvLabelStatus.BringToFront();
        }
        private void DgvMCType_GotFocus(object sender, EventArgs e)
        {
            dgvMCType.BringToFront();
        }
        private void DgvMCType_LostFocus(object sender, EventArgs e)
        {
            dgvMCType.SendToBack();
        }
        private void DgvMCName_LostFocus(object sender, EventArgs e)
        {
            dgvMCName.SendToBack();
        }
        private void DgvMCName_GotFocus(object sender, EventArgs e)
        {
            dgvMCName.BringToFront();
        }

        //Dgv Cell Value Changed
        private void DgvSearchResult_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateColName = "";
            UpdateValue = "";
            UpdatePosCNo = "";

            //SeqNo
            if (e.ColumnIndex == 11)
            {
                if (dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim() == "")
                {
                    UpdateColName = "PlanSeqNo";
                    UpdateValue = "";
                    UpdatePosCNo = dgvSearchResult.Rows[e.RowIndex].Cells[4].Value.ToString();
                }
                else
                {
                    try
                    {
                        UpdateColName = "PlanSeqNo";
                        UpdateValue = Convert.ToDouble(dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value).ToString();
                        UpdatePosCNo = dgvSearchResult.Rows[e.RowIndex].Cells[4].Value.ToString();
                    }
                    catch
                    {
                        MessageBox.Show("អនុញ្ញាតឱ្យបញ្ចូលជា ចំនួនលេខ តែប៉ុណ្ណោះ!\nទិន្នន័យនេះមិនត្រូវបានរក្សាទុកទេ!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }

            //MCType
            if (e.ColumnIndex == 12 || e.ColumnIndex == 16 || e.ColumnIndex == 20)
            {
                string NumberOfMC = (dgvSearchResult.Columns[e.ColumnIndex].HeaderText).ToString();
                NumberOfMC = NumberOfMC.Substring(NumberOfMC.Length - 1, 1);
                UpdateColName = "MC" + NumberOfMC + "Type";               
                UpdateValue = dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                UpdatePosCNo = dgvSearchResult.Rows[e.RowIndex].Cells[4].Value.ToString();
            }

            //MCName
            if (e.ColumnIndex == 13 || e.ColumnIndex == 17 || e.ColumnIndex == 21)
            {
                string NumberOfMC = (dgvSearchResult.Columns[e.ColumnIndex - 1].HeaderText).ToString();
                NumberOfMC = NumberOfMC.Substring(NumberOfMC.Length - 1, 1);
                UpdateColName = "MC"+ NumberOfMC + "Name";
                UpdateValue = dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                UpdatePosCNo = dgvSearchResult.Rows[e.RowIndex].Cells[4].Value.ToString();
            }
                        
            //Status
            if (e.ColumnIndex == 14 || e.ColumnIndex == 18 || e.ColumnIndex == 22)
            {
                string NumberOfMC = (dgvSearchResult.Columns[e.ColumnIndex - 2].HeaderText).ToString();
                NumberOfMC = NumberOfMC.Substring(NumberOfMC.Length - 1, 1);
                UpdateColName = "MC" + NumberOfMC + "Status";
                UpdateValue = "";
                foreach (DataGridViewRow DgvRow in dgvMCStatus.Rows)
                {
                    if (DgvRow.Cells[1].Value.ToString() == dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                    {
                        UpdateValue = DgvRow.Cells[0].Value.ToString();
                    }
                }
                UpdatePosCNo = dgvSearchResult.Rows[e.RowIndex].Cells[4].Value.ToString();
            }

            //ETA
            if (e.ColumnIndex == 15 || e.ColumnIndex == 19 || e.ColumnIndex == 23)
            {
                string NumberOfMC = (dgvSearchResult.Columns[e.ColumnIndex - 3].HeaderText).ToString();
                NumberOfMC = NumberOfMC.Substring(NumberOfMC.Length - 1, 1);
                UpdateColName = "MC" + NumberOfMC + "ETA";
                UpdateValue = dgvSearchResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                UpdatePosCNo = dgvSearchResult.Rows[e.RowIndex].Cells[4].Value.ToString();
            }

            UpdateFuct();
        }

        //dgv Cell Click
        private void DgvSearchResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            panelMCETA.SendToBack();
            if (e.RowIndex > -1)
            {
                //MC Type
                if (e.ColumnIndex == 12 || e.ColumnIndex == 16 || e.ColumnIndex == 20)
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
                    int Y = oRectangle.Y + 40;
                    dgvMCType.Location = new Point(X, Y);
                    dgvMCType.Focus();
                }

                //MC Status
                if (e.ColumnIndex == 14 || e.ColumnIndex == 18 || e.ColumnIndex == 22)
                {
                    dgvMCStatus.ClearSelection();
                    if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        foreach (DataGridViewRow dgvRow in dgvMCStatus.Rows)
                        {
                            if (dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value.ToString() == dgvRow.Cells[0].Value.ToString())
                            {
                                dgvMCStatus.CurrentCell = dgvRow.Cells[0];
                                dgvRow.Cells[0].Selected = true;
                            }
                        }
                    }

                    Rectangle oRectangle = dgvSearchResult.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    int X = oRectangle.X + 7;
                    int Y = oRectangle.Y + 106;
                    dgvMCStatus.Location = new Point(X, Y);
                    dgvMCStatus.Focus();
                }

            }
        }
        private void DgvMCType_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvMCName.Rows.Clear();
            dgvMCName.Rows.Add("");
            foreach (DataRow row in dtMCName.Rows)
            {
                if (dgvMCType.Rows[e.RowIndex].Cells[0].Value.ToString() == row[0].ToString())
                {
                    dgvMCName.Rows.Add(row[1]);
                }
            }
            dgvMCName.ClearSelection();
            dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = dgvMCType.CurrentCell.Value.ToString();
            dgvSearchResult.Focus();
        }
        private void DgvMCName_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = dgvMCName.CurrentCell.Value.ToString();
            dgvSearchResult.Focus();
        }
        private void DgvMCStatus_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = dgvMCStatus.CurrentCell.Value.ToString();
            dgvSearchResult.Focus();
        }
        private void DgvLabelStatus_Click(object sender, EventArgs e)
        {
            dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = dgvLabelStatus.CurrentCell.Value.ToString();
            dgvSearchResult.Focus();
        }

        //Button Click
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            panelMCETA.SendToBack();
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();
            LbStatus.Visible = true;
            dgvSearchResult.Rows.Clear();
            DataTable dtSQLCond= new DataTable();
            dtSQLCond.Columns.Add("Col");
            dtSQLCond.Columns.Add("Value");

            if (txtPosPNo.Text.Trim() != "")
            {
                if (txtPosPNo.Text.Contains("*") == true)
                {
                    string POSP = txtPosPNo.Text;
                    POSP = POSP.Replace("*", "%");
                    dtSQLCond.Rows.Add("PosPNo LIKE ", "'" + POSP + "' ");
                }
                else
                {
                    dtSQLCond.Rows.Add("PosPNo = ", "'" + txtPosPNo.Text + "' ");
                }


            }
            if (txtPosCNo.Text.Trim() != "")
            {
                if (txtPosCNo.Text.Contains("*") == true)
                {
                    string POSC = txtPosCNo.Text;
                    POSC = POSC.Replace("*", "%");
                    dtSQLCond.Rows.Add("PosCNo LIKE ", "'" + POSC + "' ");
                }
                else
                {
                    dtSQLCond.Rows.Add("PosCNo = ", "'" + txtPosCNo.Text + "' ");
                }
            }
            if (txtWIPName.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("ItemName LIKE ", "'%" + txtWIPName.Text + "%'");
            }
            if (txtDocNo.Text.Trim() != "")
            {
                string DocNo = txtDocNo.Text;
                if (DocNo.Contains("*") == true)
                {
                    DocNo = DocNo.Replace("*", "%");
                    dtSQLCond.Rows.Add("DocNo LIKE ", "'" + DocNo + "' ");
                }
                else
                {
                    dtSQLCond.Rows.Add("DocNo = ", "'" + DocNo + "' ");
                }
            }
            if (ChkDate.Checked == true)
            {
                dtSQLCond.Rows.Add("PosPDelDate BETWEEN ", "'" + dtpFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dtpToo.Value.ToString("yyyy-MM-dd") + " 23:59:59' ");
            }
            if (CboMC1.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("MC1 = ", "'" + CboMC1.Text + "' ");
            }
            if (txtPin.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("Remarks1 LIKE ", "'%" + txtPin.Text + "%' ");
            }
            if (txtWire.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("Remarks2 LIKE ", "'%" + txtWire.Text + "%' ");
            }
            if (CboPrintStatus.Text.Trim() != "")
            {
                if (CboPrintStatus.Text == "NOT YET")
                {
                    dtSQLCond.Rows.Add("NOT PrintStatus = ", "1 ");
                }
                else
                {
                    dtSQLCond.Rows.Add("PrintStatus = ", "1 ");
                }
            }
            if (CboMCName.Text.Trim() != "")
            {
                dtSQLCond.Rows.Add("MC1Name = ", "'" + CboMCName.Text + "' ");
            }

            string SQLConds = "";
            foreach (DataRow row in dtSQLCond.Rows)
            {
                if (SQLConds.Trim() == "")
                {
                    SQLConds = "WHERE " + row[0] + row[1];
                }
                else
                {
                    SQLConds = SQLConds + "AND " + row[0] + row[1];
                }
            }
            
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM " +
                                                                                    "(SELECT PosPNo, PosPDelDate, T2.ItemCode, ItemName, T1.PosCNo, Remarks1, Remarks2, Remarks3, PosCQty, Remarks, CONCAT(Slot,'') AS Slot, T1.PlanSeqNo, " +
                                                                                    "CASE "+
                                                                                    "    WHEN T1.MC1Type IS NULL THEN T3.MC1Type "+
                                                                                    "    ELSE T1.MC1Type "+
                                                                                    "END AS MC1, CONCAT(MC1Name, '') AS MC1Name, " +
                                                                                    "CASE " +
                                                                                    "    WHEN T1.MC1Status IS NULL THEN '' " +
                                                                                    "    ELSE " +
                                                                                    "        CASE " +
                                                                                    "            WHEN T1.MC1Status = 0 THEN 'STOP' " +
                                                                                    "            WHEN T1.MC1Status = 1 THEN 'RUN' " +
                                                                                    "            ELSE 'FINISH' " +
                                                                                    "        END " +
                                                                                    "    END AS MC1Stat, T1.MC1ETA, " +
                                                                                    "CASE "+
                                                                                    "    WHEN T1.MC2Type IS NULL THEN T3.MC2Type "+
                                                                                    "    ELSE T1.MC2Type "+
                                                                                    "END AS MC2, CONCAT(MC2Name, '') AS MC2Name, " +
                                                                                    "CASE " +
                                                                                    "    WHEN T1.MC2Status IS NULL THEN '' " +
                                                                                    "    ELSE " +
                                                                                    "        CASE " +
                                                                                    "            WHEN T1.MC2Status = 0 THEN 'STOP' " +
                                                                                    "            WHEN T1.MC2Status = 1 THEN 'RUN' " +
                                                                                    "            ELSE 'FINISH' " +
                                                                                    "        END " +
                                                                                    "    END AS MC2Stat, T1.MC2ETA, " +
                                                                                    "CASE " +
                                                                                    "    WHEN T1.MC3Type IS NULL THEN T3.MC3Type "+
                                                                                    "    ELSE T1.MC3Type "+
                                                                                    "END AS MC3, CONCAT(MC3Name, '') AS MC3Name, " +
                                                                                    "CASE " +
                                                                                    "    WHEN T1.MC3Status IS NULL THEN '' " +
                                                                                    "    ELSE " +
                                                                                    "        CASE " +
                                                                                    "            WHEN T1.MC3Status = 0 THEN 'STOP' " +
                                                                                    "            WHEN T1.MC3Status = 1 THEN 'RUN' " +
                                                                                    "            ELSE 'FINISH' " +
                                                                                    "        END " +
                                                                                    "    END AS MC3Stat, T1.MC3ETA, PrintStatus, DocNo FROM " +
                                                                                    "(SELECT DocNo, PosPNo, PosPDelDate, WIPCode, PosCNo, PosCQty, Remarks, MC1Type, MC1Name, MC1Status, MC1ETA, MC2Type, MC2Name, MC2Status, MC2ETA, MC3Type, MC3Name, MC3Status, MC3ETA, PrintStatus, PlanSeqNo FROM tbPOSDetailofMC WHERE NOT PosCStatus = 2) T1 " +
                                                                                    "LEFT JOIN "+
                                                                                    "(SELECT ItemCode, ItemName, Remarks1, Remarks2, Remarks3 FROM tbMasterItem WHERE LEN(ItemCode) > 4) T2 "+
                                                                                    "ON T1.WIPCode = T2.ItemCode "+
                                                                                    "LEFT JOIN "+
                                                                                    "(SELECT ItemCode, Slot, MC1Type, MC2Type, MC3Type FROM tbMasterItemPlan) T3 "+
                                                                                    "ON T2.ItemCode = T3.ItemCode) X "+SQLConds+
                                                                                    "ORDER BY PosCNo ASC", cnn.con);
                

                DataTable dt = new DataTable();
                sda.Fill(dt);

                //Remove Duplicate
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    int Count = 0;
                    string POSNo = dt.Rows[i][4].ToString();

                    DataRow dr = dt.Rows[i];

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j][4].ToString() == dr[4].ToString())
                        {
                            Count = Count + 1;

                        }
                    }

                    if (Count > 1)
                    {
                        dr.Delete();
                        dt.AcceptChanges();
                    }
                }

                double TotalQty = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string PosPNo;
                    DateTime PosPDelDate;
                    string ItemCode;
                    string ItemName;
                    string PosCNo;
                    string Remarks1;
                    string Remarks2;
                    string Remarks3;
                    double PosCQty;
                    string Remarks;
                    string Slot;
                    string SeqNo;
                    string MC1;
                    string MC1Name;
                    string MC1Status;
                    string MC1ETA;
                    string MC2;
                    string MC2Name;
                    string MC2Status;
                    string MC2ETA;
                    string MC3;
                    string MC3Name;
                    string MC3Status;
                    string MC3ETA;

                    PosPNo = row[0].ToString();
                    PosPDelDate = Convert.ToDateTime(row[1].ToString());
                    ItemCode = row[2].ToString();
                    ItemName = row[3].ToString();
                    PosCNo = row[4].ToString();
                    Remarks1 = row[5].ToString();
                    Remarks2 = row[6].ToString();
                    Remarks3 = row[7].ToString();
                    PosCQty = Convert.ToDouble(row[8].ToString());
                    Remarks = row[9].ToString();
                    Slot = row[10].ToString();
                    SeqNo = "";
                    if (row[11].ToString().Trim() != "")
                    {
                        SeqNo = row[11].ToString();
                    }
                    MC1ETA = "";
                    MC1 = row[12].ToString();
                    MC1Name = row[13].ToString();
                    MC1Status = "";
                    if (row[14].ToString().Trim() != "")
                    {
                        MC1Status= row[14].ToString();
                    }
                    MC1ETA = "";
                    if (row[15].ToString().Trim() != "")
                    {
                        MC1ETA = Convert.ToDateTime(row[15].ToString()).ToString("dd-MM-yy hh:mm tt");
                    }
                    MC2 = row[16].ToString();
                    MC2Name = row[17].ToString();
                    MC2Status = "";
                    if (row[18].ToString().Trim() != "")
                    {
                        MC2Status = row[18].ToString();
                    }
                    MC2ETA = "";
                    if (row[19].ToString().Trim() != "")
                    {
                        MC2ETA = Convert.ToDateTime(row[19].ToString()).ToString("dd-MM-yy hh:mm tt");
                    }
                    MC3 = row[20].ToString();
                    MC3Name = row[21].ToString();
                    MC3Status = "";
                    if (row[22].ToString().Trim() != "")
                    {
                        MC3Status = row[22].ToString();
                    }
                    MC3ETA = "";
                    if (row[23].ToString().Trim() != "")
                    {
                        MC3ETA = Convert.ToDateTime(row[23].ToString()).ToString("dd-MM-yy hh:mm tt");
                    }

                    dgvSearchResult.Rows.Add(PosPNo, PosPDelDate, ItemCode, ItemName, PosCNo, Remarks1, Remarks2, Remarks3, PosCQty, Remarks, Slot, SeqNo, MC1, MC1Name, MC1Status, MC1ETA, MC2, MC2Name, MC2Status, MC2ETA, MC3, MC3Name, MC3Status, MC3ETA);
                    TotalQty = TotalQty + PosCQty;

                }
                dgvSearchResult.ClearSelection();
                Cursor = Cursors.Default;
                LbStatus.Text = "ស្វែងរកឃើញទិន្នន័យ " + dgvSearchResult.Rows.Count.ToString("N0")+" ស្មើនឹងចំនួន = "+TotalQty.ToString("N0");
                LbStatus.Refresh();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("មានបញ្ហា!" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            cnn.con.Close();
        }
        private void BtnOKMCETA_Click(object sender, EventArgs e)
        {
            string Date = "";
            Date = dtpMCETADate.Value.ToString("yyyy-MM-dd");
            Date = Date + " " + dtpMCETATime.Value.ToString("HH:mm")+":00";
            dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = Convert.ToDateTime(Date);
            panelMCETA.SendToBack();
        }
        private void BtnCancelMCETA_Click(object sender, EventArgs e)
        {
            dgvSearchResult.Rows[dgvSearchResult.CurrentCell.RowIndex].Cells[dgvSearchResult.CurrentCell.ColumnIndex].Value = "";
            panelMCETA.SendToBack();
        }

        private void _1InputMCStatusForm_Load(object sender, EventArgs e)
        {
            dtColor = new DataTable();
            dtColor.Columns.Add("ShortText");
            dtColor.Columns.Add("ColorText");
            dtColor.Columns.Add("BackColor");
            dtColor.Rows.Add("RED", "White", "Red");
            dtColor.Rows.Add("BLK", "White", "Black");
            dtColor.Rows.Add("PINK", "Black", "Pink");
            dtColor.Rows.Add("YEL", "Black", "Yellow");
            dtColor.Rows.Add("BLU", "White", "Blue");
            dtColor.Rows.Add("BRN", "White", "Brown");
            dtColor.Rows.Add("G/Y", "Black", "GreenYellow");
            dtColor.Rows.Add("GRN", "White", "Green");
            dtColor.Rows.Add("GRY", "White", "Gray");
            dtColor.Rows.Add("ORG", "Black", "Orange");
            dtColor.Rows.Add("PNK", "Black", "Pink");
            dtColor.Rows.Add("SKY", "Black", "SkyBlue");
            dtColor.Rows.Add("VLT", "White", "Purple");
            dtColor.Rows.Add("WHT", "Black", "White");

            for (int i = 0; i < 10; i++)
            {
                dgvSearchResult.Columns[i].Frozen = true;
            }
            for (int i = 10; i < dgvSearchResult.Columns.Count; i++)
            {
                if (i > 11 && i < 16)
                {
                    dgvSearchResult.Columns[i].HeaderCell.Style.BackColor = Color.SlateBlue;
                    dgvSearchResult.Columns[i].HeaderCell.Style.SelectionBackColor = Color.SlateBlue;
                    dgvSearchResult.Columns[i].HeaderCell.Style.SelectionForeColor = Color.White;
                    dgvSearchResult.Columns[i].HeaderCell.Style.ForeColor = Color.White;
                }
                if (i > 15 && i < 20)
                {
                    dgvSearchResult.Columns[i].HeaderCell.Style.BackColor = Color.Black;
                    dgvSearchResult.Columns[i].HeaderCell.Style.SelectionBackColor = Color.Black;
                    dgvSearchResult.Columns[i].HeaderCell.Style.SelectionForeColor = Color.White;
                    dgvSearchResult.Columns[i].HeaderCell.Style.ForeColor = Color.White;
                }
                if (i > 19 && i < 24)
                {
                    dgvSearchResult.Columns[i].HeaderCell.Style.BackColor = Color.DodgerBlue;
                    dgvSearchResult.Columns[i].HeaderCell.Style.SelectionBackColor = Color.DodgerBlue;
                    dgvSearchResult.Columns[i].HeaderCell.Style.SelectionForeColor = Color.White;
                    dgvSearchResult.Columns[i].HeaderCell.Style.ForeColor = Color.White;
                }
            }

            dgvLabelStatus.Rows.Add("NOT YET");
            dgvLabelStatus.Rows.Add("OK");

            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbMasterItemPlan2", cnn.con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgvMCType.Rows.Add("");
                CboMC1.Items.Add("");
                foreach (DataRow row in dt.Rows)
                {
                    dgvMCType.Rows.Add(row[0]);
                    CboMC1.Items.Add(row[0]);
                }
                CboMC1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

            dgvMCStatus.Rows.Add("0", "STOP");
            dgvMCStatus.Rows.Add("1", "RUN");
            dgvMCStatus.Rows.Add("2", "FINISH");

            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT MCType, MCName FROM tbMasterMCType ORDER BY MCType ASC, MCName ASC", cnn.con);
                dtMCName = new DataTable();
                sda.Fill(dtMCName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("មានបញ្ហា!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

            CboPrintStatus.Items.Add("NOT YET");
            CboPrintStatus.Items.Add("OK");
            CboPrintStatus.SelectedIndex = 0;


            for (int i = 0; i < dgvSearchResult.Columns.Count; i++)
            {
                if (i != 11)
                {

                    dgvSearchResult.Columns[i].ReadOnly = true;
                    
                }
            }

            
        }
        private void UpdateFuct()
        {
            string query = "";
            if (UpdateColName.Contains("Type") == true)
            {
                if (UpdateValue.Trim() != "")
                {
                    query = "UPDATE tbPOSDetailofMC SET " +
                                            UpdateColName + "='" + UpdateValue + "' " +
                                            "WHERE PosCNo = '" + UpdatePosCNo + "' ;";
                }
                else
                {
                    query = "UPDATE tbPOSDetailofMC SET " +
                                            UpdateColName + "=NULL " +
                                            "WHERE PosCNo = '" + UpdatePosCNo + "' ;";
                }
            }
            else if (UpdateColName.Contains("Name") == true)
            {
                if (UpdateValue.Trim() != "")
                {
                    query = "UPDATE tbPOSDetailofMC SET " +
                                            "PrintStatus = 0, " +
                                            "PrintDate = NULL, " +
                                            UpdateColName + "='" + UpdateValue + "'," +
                                            "UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            "UpdateBy=N'" + MenuFormV2.UserForNextForm + "' " +
                                            "WHERE PosCNo = '" + UpdatePosCNo + "' ;";
                }
                else
                {
                    query = "UPDATE tbPOSDetailofMC SET " +
                                            "PrintStatus = 0, " +
                                            "PrintDate = NULL, " +
                                            UpdateColName + "=NULL," +
                                            "UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            "UpdateBy=N'" + MenuFormV2.UserForNextForm + "' " +
                                            "WHERE PosCNo = '" + UpdatePosCNo + "' ;";
                }
            }
            else if (UpdateColName.Contains("Status") == true)
            {
                if (UpdateColName.Contains("Label") == true)
                {
                    query = "UPDATE tbPOSDetailofMC SET " +
                                            UpdateColName + "='" + UpdateValue + "' " +
                                            "WHERE PosCNo = '" + UpdatePosCNo + "' ;";
                }
                else
                {
                    query = "UPDATE tbPOSDetailofMC SET " +
                                            UpdateColName + "=" + Convert.ToDouble(UpdateValue) + " " +
                                            "WHERE PosCNo = '" + UpdatePosCNo + "' ;";
                }

            }
            else if (UpdateColName.Contains("ETA") == true)
            {
                if (UpdateValue.Trim() != "")
                {
                    query = "UPDATE tbPOSDetailofMC SET " +
                                            UpdateColName + "='" + Convert.ToDateTime(UpdateValue).ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                            "WHERE PosCNo = '" + UpdatePosCNo + "' ;";
                }
                else
                {
                    query = "UPDATE tbPOSDetailofMC SET " +
                                            UpdateColName + "=NULL " +
                                            "WHERE PosCNo = '" + UpdatePosCNo + "' ;";
                }
            }
            else if (UpdateColName.Contains("PlanSeqNo") == true)
            {
                if (UpdateValue.Trim() != "")
                {
                    query = "UPDATE tbPOSDetailofMC SET " +
                                            UpdateColName + "=" + UpdateValue + " " +
                                            "WHERE PosCNo = '" + UpdatePosCNo + "' ;";
                }
                else
                {
                    query = "UPDATE tbPOSDetailofMC SET " +
                                            UpdateColName + "=NULL " +
                                            "WHERE PosCNo = '" + UpdatePosCNo + "' ;";
                }
            }
            else
            {

            }

            if (query.Trim() != "")
            {
                try
                {
                    cnn.con.Open();
                    SqlCommand cmd = new SqlCommand(query, cnn.con);
                    cmd.ExecuteNonQuery();
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
