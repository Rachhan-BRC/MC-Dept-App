using MachineDeptApp.OtherClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace MachineDeptApp.MCPlans
{
    //Search / Update / Delete for tbPlanMCCompleteQty
    //Only Status = 1 rows are active. Delete does not remove the row, it sets Status = 0.
    //Shift comes from RegDate: D = 07:30 ~ before 19:30, N = other time
    //Summary Qty (PO / Machine) shows SUM(Qty) in the same grid; Update / Delete only work on the detail view
    public partial class PlanMCCompleteQtyForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        ExportAsCSVClass CSV = new ExportAsCSVClass();

        string ErrorText = "";

        //True while the grid is being filled: adding the first row auto-selects it before its cells have values
        bool IsLoading = false;

        //Values of cboSummary. "" = detail (1 grid row = 1 table row)
        const string SummaryByPO = "PO";
        const string SummaryByMachine = "Machine";
        //Summary Qty used by the last search (the combobox can change after searching)
        string ViewMode = "";

        //Machine / Remarks columns checked once per app run (same SQL as the tablet app / SQL\tbPlanMCCompleteQty.sql)
        static bool ColumnsReady = false;

        //Shift of T.RegDate (no RegDate >> "")
        const string ShiftSql = "CASE WHEN T.RegDate IS NULL THEN '' WHEN CAST(T.RegDate AS TIME) >= '07:30' AND CAST(T.RegDate AS TIME) < '19:30' THEN 'D' ELSE 'N' END";

        //tbPlanMCCompleteQty has no primary key, so one row is identified by PO + ItemCode + RegDate.
        //The original RegDate (DateTime or DBNull) is kept in DataGridViewRow.Tag.
        const string RowKeyCondition = "WHERE Status = 1 AND PO = @KeyPO AND ItemCode = @KeyItemCode AND ((RegDate IS NULL AND @KeyRegDate IS NULL) OR RegDate = @KeyRegDate) ";

        //Normal image and gray image of the header buttons (swap when Enabled changes)
        Dictionary<Button, Image[]> BtnImages = new Dictionary<Button, Image[]>();

        public PlanMCCompleteQtyForm()
        {
            InitializeComponent();
            this.cnn.Connection();

            dtpSDate.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpEDate.Value = DateTime.Today;

            foreach (Button btn in new Button[] { btnUpdate, btnDelete, btnExport })
            {
                BtnImages[btn] = new Image[] { btn.BackgroundImage, ToGrayImage(btn.BackgroundImage) };
                btn.EnabledChanged += HeaderButton_EnabledChanged;
                HeaderButton_EnabledChanged(btn, EventArgs.Empty);
            }

            this.Shown += PlanMCCompleteQtyForm_Shown;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnUpdate.Click += BtnUpdate_Click;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnExport.Click += BtnExport_Click;
            this.DgvSearch.SelectionChanged += DgvSearch_SelectionChanged;
            this.DgvSearch.CellFormatting += DgvSearch_CellFormatting;
            this.dtpSDate.ValueChanged += DtpDate_ValueChanged;
            this.dtpEDate.ValueChanged += DtpDate_ValueChanged;
            //Enter key = Search, but Enter key inside the edit box = Update
            this.grbEdit.Enter += GrbEdit_Enter;
            this.grbEdit.Leave += GrbEdit_Leave;
        }

        private void PlanMCCompleteQtyForm_Shown(object sender, EventArgs e)
        {
            DgvSearch.RowHeadersDefaultCellStyle.Font = new Font(DgvSearch.AlternatingRowsDefaultCellStyle.Font, FontStyle.Regular);
            LoadMachineList();
        }

        //cboMachine items = "" + MC names from tbMasterMCType
        private void LoadMachineList()
        {
            cboMachine.Items.Clear();
            cboMachine.Items.Add("");
            DataTable dt = new DataTable();
            string LoadError = "";
            try
            {
                cnn.con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT DISTINCT MCName FROM tbMasterMCType WHERE ISNULL(MCName, '') <> '' ORDER BY MCName", cnn.con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                LoadError = ex.Message;
            }
            cnn.con.Close();

            foreach (DataRow row in dt.Rows)
                cboMachine.Items.Add(row["MCName"].ToString());
            if (LoadError != "")
                MessageBox.Show("មានបញ្ហា! Machine list : \n" + LoadError, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DtpDate_ValueChanged(object sender, EventArgs e)
        {
            chkDate.Checked = true;
        }

        private void GrbEdit_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = btnUpdate;
        }

        private void GrbEdit_Leave(object sender, EventArgs e)
        {
            this.AcceptButton = btnSearch;
        }

        private void HeaderButton_EnabledChanged(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackgroundImage = btn.Enabled ? BtnImages[btn][0] : BtnImages[btn][1];
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            IsLoading = true;
            DgvSearch.Rows.Clear();
            ClearEditBox();
            ViewMode = cboSummary.Text;
            ShowColumnsForView();
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();

            //Taking Data
            DataTable dtData = new DataTable();
            try
            {
                cnn.con.Open();
                EnsureColumns();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.con;

                string SQLConds = "";
                if (chkDate.Checked)
                {
                    SQLConds += "AND T.RegDate >= @SDate AND T.RegDate < @EDate ";
                    cmd.Parameters.Add("@SDate", SqlDbType.DateTime).Value = dtpSDate.Value.Date;
                    cmd.Parameters.Add("@EDate", SqlDbType.DateTime).Value = dtpEDate.Value.Date.AddDays(1);
                }
                if (txtPO.Text.Trim() != "")
                {
                    SQLConds += "AND T.PO LIKE @PO ";
                    cmd.Parameters.Add("@PO", SqlDbType.NVarChar, 200).Value = ToLikePattern(txtPO.Text);
                }
                if (txtItemCode.Text.Trim() != "")
                {
                    SQLConds += "AND T.ItemCode LIKE @ItemCode ";
                    cmd.Parameters.Add("@ItemCode", SqlDbType.NVarChar, 200).Value = ToLikePattern(txtItemCode.Text);
                }
                if (cboShift.Text != "")
                {
                    SQLConds += "AND S.Shift = @Shift ";
                    cmd.Parameters.Add("@Shift", SqlDbType.VarChar, 1).Value = cboShift.Text;
                }
                if (cboMachine.Text != "")
                {
                    SQLConds += "AND T.Machine = @Machine ";
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 50).Value = cboMachine.Text;
                }

                string FromWhere = @" FROM tbPlanMCCompleteQty T
                                    CROSS APPLY (SELECT " + ShiftSql + @" AS Shift) S
                                    OUTER APPLY (SELECT TOP 1 ItemName FROM tbMasterItem WHERE ItemCode = T.ItemCode) I
                                    WHERE T.Status = 1 " + SQLConds;
                if (ViewMode == SummaryByPO)
                    cmd.CommandText = "SELECT T.PO, T.ItemCode, MAX(I.ItemName) AS ItemName, SUM(T.Qty) AS Qty" + FromWhere +
                                      " GROUP BY T.PO, T.ItemCode ORDER BY T.PO, T.ItemCode";
                else if (ViewMode == SummaryByMachine)
                    cmd.CommandText = "SELECT ISNULL(T.Machine, '') AS Machine, SUM(T.Qty) AS Qty" + FromWhere +
                                      " GROUP BY ISNULL(T.Machine, '') ORDER BY ISNULL(T.Machine, '')";
                else
                    cmd.CommandText = "SELECT T.PO, T.ItemCode, I.ItemName, T.Machine, S.Shift, T.Qty, T.Remarks, T.RegBy, T.RegDate, T.UpdateBy, T.UpdateDate" + FromWhere +
                                      " ORDER BY T.RegDate DESC, T.PO, T.ItemCode";
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dtData);
            }
            catch (Exception ex)
            {
                ErrorText = "Taking Data : \n" + ex.Message;
            }
            cnn.con.Close();

            //Add to DGV
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtData.Rows)
                {
                    int i = DgvSearch.Rows.Add();
                    DataGridViewRow drow = DgvSearch.Rows[i];
                    drow.HeaderCell.Value = (i + 1).ToString();
                    drow.Cells[colQty.Index].Value = Convert.ToDouble(row["Qty"]);
                    if (ViewMode != SummaryByMachine)
                    {
                        drow.Cells[colPO.Index].Value = row["PO"].ToString();
                        drow.Cells[colItemCode.Index].Value = row["ItemCode"].ToString();
                        drow.Cells[colItemName.Index].Value = row["ItemName"].ToString();
                    }
                    if (ViewMode != SummaryByPO)
                    {
                        drow.Cells[colMachine.Index].Value = row["Machine"].ToString();
                    }
                    if (ViewMode == "")
                    {
                        drow.Cells[colShift.Index].Value = row["Shift"].ToString();
                        drow.Cells[colRemarks.Index].Value = row["Remarks"].ToString();
                        drow.Cells[colRegBy.Index].Value = row["RegBy"].ToString();
                        drow.Cells[colRegDate.Index].Value = DBNullToNull(row["RegDate"]);
                        drow.Cells[colUpdateBy.Index].Value = row["UpdateBy"].ToString();
                        drow.Cells[colUpdateDate.Index].Value = DBNullToNull(row["UpdateDate"]);
                        drow.Tag = row["RegDate"];
                    }
                }
                DgvSearch.ClearSelection();
                DgvSearch.CurrentCell = null;
            }
            IsLoading = false;

            btnExport.Enabled = DgvSearch.Rows.Count > 0;
            CheckButtons();
            Cursor = Cursors.Default;

            if (ErrorText.Trim() == "")
            {
                LbStatus.Text = "រកឃើញទិន្នន័យ  " + DgvSearch.Rows.Count.ToString("N0") + (ViewMode == "" ? "" : "  (Summary Qty : " + ViewMode + ")");
                LbStatus.Refresh();
            }
            else
            {
                LbStatus.Text = "មានបញ្ហា!";
                LbStatus.Refresh();
                MessageBox.Show("មានបញ្ហា! " + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvSearch_SelectionChanged(object sender, EventArgs e)
        {
            if (IsLoading)
                return;

            CheckButtons();
            if (ViewMode == "" && DgvSearch.SelectedRows.Count == 1)
            {
                DataGridViewRow drow = DgvSearch.SelectedRows[0];
                txtEditPO.Text = CellText(drow, colPO);
                txtEditItemCode.Text = CellText(drow, colItemCode);
                txtEditQty.Text = CellText(drow, colQty);
            }
            else
            {
                ClearEditBox();
            }
        }

        //Qty = 0 >> the whole row is red (also after Update, and in the summary views)
        private void DgvSearch_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            object Qty = DgvSearch.Rows[e.RowIndex].Cells[colQty.Index].Value;
            if (Qty is double && (double)Qty == 0)
                e.CellStyle.ForeColor = Color.Red;
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (ViewMode != "" || DgvSearch.SelectedRows.Count != 1)
                return;
            DataGridViewRow drow = DgvSearch.SelectedRows[0];

            //Check input
            double Qty;
            if (!double.TryParse(txtEditQty.Text.Trim(), out Qty) || Qty < 0)
            {
                MessageBox.Show("សូមបញ្ចូល Qty ជាលេខ (ចាប់ពី 0 ឡើងទៅ)!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEditQty.Focus();
                txtEditQty.SelectAll();
                return;
            }

            DialogResult DSL = MessageBox.Show("តើអ្នកចង់កែប្រែទិន្នន័យនេះមែនដែរឬទេ?\n" + RowText(drow), MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DSL != DialogResult.Yes)
                return;

            ErrorText = "";
            string ProblemText = "";
            DateTime UpdateDate = DateTime.Now;
            Cursor = Cursors.WaitCursor;
            try
            {
                cnn.con.Open();
                using (SqlTransaction tran = cnn.con.BeginTransaction())
                {
                    int Found = CountRow(drow, tran);
                    if (Found == 1)
                    {
                        SqlCommand cmd = new SqlCommand("UPDATE tbPlanMCCompleteQty SET Qty = @Qty, UpdateBy = @UpdateBy, UpdateDate = @UpdateDate " + RowKeyCondition, cnn.con, tran);
                        AddRowKeyParameters(cmd, drow);
                        cmd.Parameters.Add("@Qty", SqlDbType.Float).Value = Qty;
                        cmd.Parameters.Add("@UpdateBy", SqlDbType.NVarChar, 50).Value = MenuFormV2.UserForNextForm ?? "";
                        cmd.Parameters.Add("@UpdateDate", SqlDbType.DateTime).Value = UpdateDate;
                        cmd.ExecuteNonQuery();
                        tran.Commit();
                    }
                    else
                    {
                        ProblemText = NotFoundText(Found);
                        tran.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorText = "Updating : \n" + ex.Message;
            }
            cnn.con.Close();
            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
            {
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (ProblemText.Trim() != "")
            {
                MessageBox.Show(ProblemText + "\n" + RowText(drow), MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                drow.Cells[colQty.Index].Value = Qty;
                drow.Cells[colUpdateBy.Index].Value = MenuFormV2.UserForNextForm;
                drow.Cells[colUpdateDate.Index].Value = UpdateDate;
                LbStatus.Text = "កែប្រែរួចរាល់!";
                LbStatus.Refresh();
                MessageBox.Show("កែប្រែរួចរាល់!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (ViewMode != "" || DgvSearch.SelectedRows.Count == 0)
                return;
            List<DataGridViewRow> SelRows = DgvSearch.SelectedRows.Cast<DataGridViewRow>().OrderBy(r => r.Index).ToList();

            string AskText;
            if (SelRows.Count == 1)
                AskText = "តើអ្នកចង់លុបទិន្នន័យនេះមែនដែរឬទេ?\n" + RowText(SelRows[0]);
            else
                AskText = "តើអ្នកចង់លុបទិន្នន័យ " + SelRows.Count.ToString("N0") + " ជួរនេះមែនដែរឬទេ?";
            DialogResult DSL = MessageBox.Show(AskText, MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DSL != DialogResult.Yes)
                return;

            ErrorText = "";
            string ProblemText = "";
            DateTime UpdateDate = DateTime.Now;
            Cursor = Cursors.WaitCursor;
            try
            {
                cnn.con.Open();
                using (SqlTransaction tran = cnn.con.BeginTransaction())
                {
                    //All selected rows are deleted together, or nothing is deleted
                    //Delete = set Status = 0 (the row stays in the table)
                    foreach (DataGridViewRow drow in SelRows)
                    {
                        int Found = CountRow(drow, tran);
                        if (Found != 1)
                        {
                            ProblemText = NotFoundText(Found) + "\n" + RowText(drow);
                            break;
                        }
                        SqlCommand cmd = new SqlCommand("UPDATE tbPlanMCCompleteQty SET Status = 0, UpdateBy = @UpdateBy, UpdateDate = @UpdateDate " + RowKeyCondition, cnn.con, tran);
                        AddRowKeyParameters(cmd, drow);
                        cmd.Parameters.Add("@UpdateBy", SqlDbType.NVarChar, 50).Value = MenuFormV2.UserForNextForm ?? "";
                        cmd.Parameters.Add("@UpdateDate", SqlDbType.DateTime).Value = UpdateDate;
                        cmd.ExecuteNonQuery();
                    }

                    if (ProblemText.Trim() == "")
                        tran.Commit();
                    else
                        tran.Rollback();
                }
            }
            catch (Exception ex)
            {
                ErrorText = "Deleting : \n" + ex.Message;
            }
            cnn.con.Close();
            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
            {
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (ProblemText.Trim() != "")
            {
                MessageBox.Show(ProblemText + "\n\nមិនបានលុបទិន្នន័យណាមួយទេ។", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                for (int i = SelRows.Count - 1; i >= 0; i--)
                    DgvSearch.Rows.Remove(SelRows[i]);
                for (int i = 0; i < DgvSearch.Rows.Count; i++)
                    DgvSearch.Rows[i].HeaderCell.Value = (i + 1).ToString();
                DgvSearch.ClearSelection();
                DgvSearch.CurrentCell = null;
                ClearEditBox();
                CheckButtons();
                btnExport.Enabled = DgvSearch.Rows.Count > 0;
                LbStatus.Text = "លុបរួចរាល់  " + SelRows.Count.ToString("N0") + " ជួរ";
                LbStatus.Refresh();
                MessageBox.Show("លុបរួចរាល់!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (DgvSearch.Rows.Count > 0)
            {
                CSV.CSVErrorT = "";
                CSV.ExportAsCSV(this.DgvSearch, "MC Complete Qty - " + DateTime.Now.ToString("yyyyMMdd HH_mm"), 0);
                if (CSV.CSVErrorT.Trim() != "")
                {
                    if (CSV.CSVErrorT.Trim() == "OK")
                        MessageBox.Show("Export successfully.", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Something went wrong!\n" + CSV.CSVErrorT, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Table made before the Machine / Remarks column existed >> add the column (old rows keep NULL)
        //cnn.con must be open
        private void EnsureColumns()
        {
            if (ColumnsReady)
                return;
            SqlCommand cmd = new SqlCommand("IF OBJECT_ID(N'dbo.tbPlanMCCompleteQty', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.tbPlanMCCompleteQty', N'Machine') IS NULL " +
                                            "ALTER TABLE dbo.tbPlanMCCompleteQty ADD Machine NVARCHAR(50) NULL; " +
                                            "IF OBJECT_ID(N'dbo.tbPlanMCCompleteQty', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.tbPlanMCCompleteQty', N'Remarks') IS NULL " +
                                            "ALTER TABLE dbo.tbPlanMCCompleteQty ADD Remarks NVARCHAR(100) NULL;", cnn.con);
            cmd.ExecuteNonQuery();
            ColumnsReady = true;
        }

        //Count how many rows in DB match this grid row (should be exactly 1)
        private int CountRow(DataGridViewRow drow, SqlTransaction tran)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tbPlanMCCompleteQty WITH (UPDLOCK, HOLDLOCK) " + RowKeyCondition, cnn.con, tran);
            AddRowKeyParameters(cmd, drow);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private void AddRowKeyParameters(SqlCommand cmd, DataGridViewRow drow)
        {
            cmd.Parameters.Add("@KeyPO", SqlDbType.NVarChar, 50).Value = CellText(drow, colPO);
            cmd.Parameters.Add("@KeyItemCode", SqlDbType.NVarChar, 50).Value = CellText(drow, colItemCode);
            cmd.Parameters.Add("@KeyRegDate", SqlDbType.DateTime).Value = drow.Tag ?? DBNull.Value;
        }

        private string NotFoundText(int Found)
        {
            if (Found == 0)
                return "រកមិនឃើញទិន្នន័យនេះទេ! ប្រហែលជាត្រូវបានកែប្រែ ឬលុបរួចហើយ។ សូមស្វែងរកម្តងទៀត។";
            return "ទិន្នន័យនេះមានស្ទួនគ្នា " + Found + " ជួរ (PO, Item Code, ថ្ងៃបញ្ចូល ដូចគ្នា)! មិនអាចកែប្រែ/លុបតាមទម្រង់នេះបានទេ។";
        }

        private string RowText(DataGridViewRow drow)
        {
            return "PO : " + drow.Cells[colPO.Index].Value + "\nItem Code : " + drow.Cells[colItemCode.Index].Value;
        }

        //Summary view rows are totals, not table rows >> no Update / Delete
        private void CheckButtons()
        {
            int SelectedCount = ViewMode == "" ? DgvSearch.SelectedRows.Count : 0;
            btnUpdate.Enabled = SelectedCount == 1;
            btnDelete.Enabled = SelectedCount > 0;
            grbEdit.Enabled = SelectedCount == 1;
        }

        //Detail = all columns. Summary by PO = PO, Item Code, Item Name, Qty. Summary by Machine = Machine, Qty
        private void ShowColumnsForView()
        {
            bool Detail = ViewMode == "";
            bool ByMachine = ViewMode == SummaryByMachine;
            colPO.Visible = !ByMachine;
            colItemCode.Visible = !ByMachine;
            colItemName.Visible = !ByMachine;
            colMachine.Visible = Detail || ByMachine;
            foreach (DataGridViewColumn col in new DataGridViewColumn[] { colShift, colRemarks, colRegBy, colRegDate, colUpdateBy, colUpdateDate })
                col.Visible = Detail;
        }

        private void ClearEditBox()
        {
            txtEditPO.Text = "";
            txtEditItemCode.Text = "";
            txtEditQty.Text = "";
        }

        private static string CellText(DataGridViewRow drow, DataGridViewColumn col)
        {
            object value = drow.Cells[col.Index].Value;
            return value == null ? "" : value.ToString();
        }

        private static object DBNullToNull(object value)
        {
            return value == DBNull.Value ? null : value;
        }

        //Text box value -> LIKE pattern. '*' works as wildcard, other LIKE special characters are escaped.
        private static string ToLikePattern(string text)
        {
            string s = text.Trim().Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%");
            return "%" + s + "%";
        }

        private static Image ToGrayImage(Image source)
        {
            if (source == null)
                return null;
            Bitmap gray = new Bitmap(source.Width, source.Height);
            using (Graphics g = Graphics.FromImage(gray))
            using (ImageAttributes attr = new ImageAttributes())
            {
                ColorMatrix cm = new ColorMatrix(new float[][]
                {
                    new float[] { 0.30f, 0.30f, 0.30f, 0, 0 },
                    new float[] { 0.59f, 0.59f, 0.59f, 0, 0 },
                    new float[] { 0.11f, 0.11f, 0.11f, 0, 0 },
                    new float[] { 0, 0, 0, 0.5f, 0 },
                    new float[] { 0, 0, 0, 0, 1 }
                });
                attr.SetColorMatrix(cm);
                g.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height), 0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attr);
            }
            return gray;
        }
    }
}
