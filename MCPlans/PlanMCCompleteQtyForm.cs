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
    public partial class PlanMCCompleteQtyForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        ExportAsCSVClass CSV = new ExportAsCSVClass();

        string ErrorText = "";

        //True while the grid is being filled: adding the first row auto-selects it before its cells have values
        bool IsLoading = false;

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
            this.dtpSDate.ValueChanged += DtpDate_ValueChanged;
            this.dtpEDate.ValueChanged += DtpDate_ValueChanged;
            //Enter key = Search, but Enter key inside the edit box = Update
            this.grbEdit.Enter += GrbEdit_Enter;
            this.grbEdit.Leave += GrbEdit_Leave;
        }

        private void PlanMCCompleteQtyForm_Shown(object sender, EventArgs e)
        {
            DgvSearch.RowHeadersDefaultCellStyle.Font = new Font(DgvSearch.AlternatingRowsDefaultCellStyle.Font, FontStyle.Regular);
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
            LbStatus.Text = "កំពុងស្វែងរក . . .";
            LbStatus.Refresh();

            //Taking Data
            DataTable dtData = new DataTable();
            try
            {
                cnn.con.Open();
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
                cmd.CommandText = @"SELECT T.PO, T.ItemCode, I.ItemName, T.Qty, T.RegBy, T.RegDate, T.UpdateBy, T.UpdateDate
                                    FROM tbPlanMCCompleteQty T
                                    OUTER APPLY (SELECT TOP 1 ItemName FROM tbMasterItem WHERE ItemCode = T.ItemCode) I
                                    WHERE T.Status = 1 " + SQLConds + @"
                                    ORDER BY T.RegDate DESC, T.PO, T.ItemCode";
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
                    drow.Cells[colPO.Index].Value = row["PO"].ToString();
                    drow.Cells[colItemCode.Index].Value = row["ItemCode"].ToString();
                    drow.Cells[colItemName.Index].Value = row["ItemName"].ToString();
                    drow.Cells[colQty.Index].Value = Convert.ToDouble(row["Qty"]);
                    drow.Cells[colRegBy.Index].Value = row["RegBy"].ToString();
                    drow.Cells[colRegDate.Index].Value = DBNullToNull(row["RegDate"]);
                    drow.Cells[colUpdateBy.Index].Value = row["UpdateBy"].ToString();
                    drow.Cells[colUpdateDate.Index].Value = DBNullToNull(row["UpdateDate"]);
                    drow.Tag = row["RegDate"];
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
                LbStatus.Text = "រកឃើញទិន្នន័យ  " + DgvSearch.Rows.Count.ToString("N0");
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
            if (DgvSearch.SelectedRows.Count == 1)
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

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (DgvSearch.SelectedRows.Count != 1)
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
            if (DgvSearch.SelectedRows.Count == 0)
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

        private void CheckButtons()
        {
            int SelectedCount = DgvSearch.SelectedRows.Count;
            btnUpdate.Enabled = SelectedCount == 1;
            btnDelete.Enabled = SelectedCount > 0;
            grbEdit.Enabled = SelectedCount == 1;
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
