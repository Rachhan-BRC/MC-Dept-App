using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MachineDeptApp.MCPlans
{
    public partial class DrawingItemDetailForm : Form
    {
        public enum FormMode { Create, Update, View }

        private class HeaderGroup
        {
            public int StartCol;
            public int EndCol;
            public string Text;
            public HeaderGroup(int startCol, int endCol, string text)
            {
                StartCol = startCol; EndCol = endCol; Text = text;
            }
        }

        SQLConnect cnn = new SQLConnect();
        FormMode mode;
        readonly int drawingID;
        HeaderGroup[] headerGroups;
        // Data columns in save order; bit i of a row's SameMask = column i is "same as above" (arrow)
        DataGridViewColumn[] dataColumns;
        bool syncingSame;
        const string DefaultRefText = "REF: PE-RE-19 Rev: 01 Date: 08-02-2020";

        public DrawingItemDetailForm(FormMode mode, int drawingID)
        {
            InitializeComponent();
            this.cnn.Connection();
            this.mode = mode;
            this.drawingID = drawingID;

            // Top-row group headers (column indexes in dgvDetail)
            headerGroups = new HeaderGroup[]
            {
                new HeaderGroup(ColWire.Index, ColLength.Index, "WIRE\n(ខ្សែភ្លើង)"),
                new HeaderGroup(ColCNAPin.Index, ColCNAPN.Index, "CONNECTOR A\n(ខនិកទ័រ A)"),
                new HeaderGroup(ColCNBStrip.Index, ColCNBPin.Index, "CONNECTOR B\n(ខនិកទ័រ B)")
            };

            this.Load += DrawingItemDetailForm_Load;
            this.btnSave.Click += BtnSave_Click;
            this.btnClose.Click += BtnClose_Click;
            this.btnAddRow.Click += BtnAddRow_Click;
            this.btnDeleteRow.Click += BtnDeleteRow_Click;
            this.dgvDetail.CellPainting += DgvDetail_CellPainting;
            this.dgvDetail.Paint += DgvDetail_Paint;
            this.dgvDetail.ColumnWidthChanged += (s, e) => dgvDetail.Invalidate();
            this.dgvDetail.Scroll += (s, e) => dgvDetail.Invalidate();
            this.dgvDetail.Resize += (s, e) => dgvDetail.Invalidate();
            this.dgvDetail.RowsAdded += (s, e) => RenumberRows();
            this.dgvDetail.RowsRemoved += (s, e) => { RenumberRows(); ResyncSameValues(); };

            dataColumns = new DataGridViewColumn[] { ColWire, ColColor, ColLength, ColCNAPin, ColCNAStrip,
                                                     ColCNAPN, ColCNBStrip, ColCNBPN, ColCNBPin };
            this.dgvDetail.CellPainting += DgvDetail_CellPaintingArrow;
            this.dgvDetail.CellValueChanged += DgvDetail_CellValueChanged;
            this.dgvDetail.KeyDown += DgvDetail_KeyDown;
            this.dgvDetail.CellMouseDown += DgvDetail_CellMouseDown;
            this.btnSameArrow.Click += (s, e) => ToggleSameArrow();
            this.txtItemPart.TextChanged += TxtItemPart_TextChanged;
            this.txtDrawingNo.TextChanged += TxtDrawingNo_TextChanged;
            // a P/N value that is not in the dropdown list must not crash the grid
            this.dgvDetail.DataError += (s, e) => { e.ThrowException = false; };
            this.dgvDetail.EditingControlShowing += DgvDetail_EditingControlShowing;
            this.dgvDetail.CellValidating += DgvDetail_CellValidating;
            BuildGridContextMenu();
        }

        #region Wip Code lookup (PEDeptDB.tbWipCode)

        bool loadingData;
        string lastLookupCode = "";
        const int WipCodeLength = 6;

        private void TxtItemPart_TextChanged(object sender, EventArgs e)
        {
            if (loadingData || mode == FormMode.View) return;
            string code = txtItemPart.Text.Trim();
            if (code.Length != WipCodeLength)
            {
                lastLookupCode = "";
                return;
            }
            if (code == lastLookupCode) return;
            lastLookupCode = code;
            LookupWipCode(code);
        }

        private void LookupWipCode(string code)
        {
            string wipName = null, wireTube = null;
            Cursor = Cursors.WaitCursor;
            try
            {
                cnn.con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 WipName, WireTube FROM [PEDeptDB].[dbo].[tbWipCode] WHERE WipCode = @Code", cnn.con))
                {
                    cmd.Parameters.AddWithValue("@Code", code);
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            wipName = rd["WipName"] == DBNull.Value ? "" : rd["WipName"].ToString().Trim();
                            wireTube = rd["WireTube"] == DBNull.Value ? "" : rd["WireTube"].ToString();
                        }
                    }
                }
                connectorPNs = QueryConnectorPNs(code);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Something went wrong! Please check the network connection.\n" + ex.Message, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                if (cnn.con.State == ConnectionState.Open) cnn.con.Close();
                Cursor = Cursors.Default;
            }

            ApplyPNItems(null);

            if (wipName == null)
            {
                MessageBox.Show("Wip Code \"" + code + "\" was not found!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtItemName.Text = wipName;
            txtDrawingNo.Text = code.Substring(0, 4);
            FillWires(SplitWires(wireTube));
        }

        #region Drawing No lookup (OBS mstitem)

        SQLConnectOBS cnnOBS;
        string lastDrawingLookup = "";

        // Drawing No = mstitem.ItemCode -> Customer = Remark, Document No. = ItemName
        private void TxtDrawingNo_TextChanged(object sender, EventArgs e)
        {
            if (loadingData || mode == FormMode.View) return;
            string code = txtDrawingNo.Text.Trim();
            if (code == "" || code == lastDrawingLookup) return;
            lastDrawingLookup = code;

            string itemName = null, remark = null;
            try
            {
                if (cnnOBS == null)
                {
                    cnnOBS = new SQLConnectOBS();
                    cnnOBS.Connection();
                }
                cnnOBS.conOBS.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 ItemName, Remark FROM mstitem " +
                    "WHERE ItemType <> '2' AND ComLoc = 'Assy1' AND ItemCode = @Code", cnnOBS.conOBS))
                {
                    cmd.Parameters.AddWithValue("@Code", code);
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            itemName = rd["ItemName"] == DBNull.Value ? "" : rd["ItemName"].ToString().Trim();
                            remark = rd["Remark"] == DBNull.Value ? "" : rd["Remark"].ToString().Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lastDrawingLookup = "";
                MessageBox.Show("Cannot read the item master (OBS).\n" + ex.Message, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                if (cnnOBS != null && cnnOBS.conOBS != null && cnnOBS.conOBS.State == ConnectionState.Open) cnnOBS.conOBS.Close();
            }

            // not found (e.g. still typing): leave the fields as they are
            if (itemName == null) return;
            txtCustomer.Text = remark;
            txtDocumentNo.Text = itemName;
        }

        #endregion

        // WireTube may hold several wires; accept line breaks, ',', ';' or '|' as separators
        private static string[] SplitWires(string wireTube)
        {
            if (string.IsNullOrWhiteSpace(wireTube)) return new string[0];
            var list = new System.Collections.Generic.List<string>();
            foreach (string part in wireTube.Split(new[] { "\r\n", "\n", "\r", ",", ";", "|" }, StringSplitOptions.RemoveEmptyEntries))
            {
                string w = part.Trim();
                if (w != "") list.Add(w);
            }
            return list.ToArray();
        }

        private void FillWires(string[] wires)
        {
            if (wires.Length == 0) return;
            dgvDetail.EndEdit();

            bool hasWire = false;
            foreach (DataGridViewRow row in dgvDetail.Rows)
            {
                if (!row.IsNewRow && CellText(row, ColWire) != "") { hasWire = true; break; }
            }
            if (hasWire && MessageBox.Show("Replace the existing Cable / Wire values with the wires of this Wip Code?", MenuFormV2.MsgTitle,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            int dataRows = 0;
            foreach (DataGridViewRow row in dgvDetail.Rows)
            {
                if (!row.IsNewRow) dataRows++;
            }
            for (int i = 0; i < wires.Length; i++)
            {
                DataGridViewRow row = i < dataRows ? dgvDetail.Rows[i] : dgvDetail.Rows[dgvDetail.Rows.Add()];
                SetSame(row, ColWire.Index, false);
                row.Cells[ColWire.Index].Value = wires[i];
            }
            dgvDetail.Invalidate();
        }

        #endregion

        #region Connector P/N dropdown (PEDeptDB.tbWipData + PEDeptDB.tbRawMaterial)

        string[] connectorPNs = new string[0];

        // Connector descriptions (non-wire raw materials) used by the Wip Code; the connection must already be open
        private string[] QueryConnectorPNs(string code)
        {
            var list = new System.Collections.Generic.List<string>();
            if (string.IsNullOrWhiteSpace(code)) return list.ToArray();
            using (SqlCommand cmd = new SqlCommand(
                "SELECT DISTINCT LTRIM(RTRIM(r.Description)) AS Description " +
                "FROM [PEDeptDB].[dbo].[tbWipData] d " +
                "INNER JOIN [PEDeptDB].[dbo].[tbRawMaterial] r ON r.Code = d.RMCode " +
                "WHERE d.WipCode = @Code AND r.Type <> 'Wire' AND ISNULL(r.Description, '') <> '' " +
                "ORDER BY Description", cnn.con))
            {
                cmd.Parameters.AddWithValue("@Code", code.Trim());
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read()) list.Add(rd["Description"].ToString());
                }
            }
            return list.ToArray();
        }

        private bool IsPNColumn(int columnIndex)
        {
            return columnIndex == ColCNAPN.Index || columnIndex == ColCNBPN.Index;
        }

        // P/N cells: pick from the list or type a new value
        private void DgvDetail_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ComboBox cb = e.Control as ComboBox;
            if (cb == null) return;
            if (IsPNColumn(dgvDetail.CurrentCell.ColumnIndex))
            {
                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
            else
            {
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }

        // a typed P/N that is not in the list is added to both P/N dropdowns so the cell accepts it
        private void DgvDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 || !IsPNColumn(e.ColumnIndex) || !dgvDetail.IsCurrentCellInEditMode) return;
            string text = Convert.ToString(e.FormattedValue); // not trimmed: must equal what the editor commits
            foreach (DataGridViewComboBoxColumn col in new[] { ColCNAPN, ColCNBPN })
            {
                if (!col.Items.Contains(text)) col.Items.Add(text);
            }
            dgvDetail.CurrentCell.Value = text;
        }

        // Fill both P/N dropdowns; values already in the grid (or in 'extra') are kept so old data still shows
        private void ApplyPNItems(System.Collections.Generic.IEnumerable<string> extra)
        {
            dgvDetail.EndEdit();
            foreach (DataGridViewComboBoxColumn col in new[] { ColCNAPN, ColCNBPN })
            {
                var items = new System.Collections.Generic.List<string> { "" }; // blank entry = clear the cell
                foreach (string pn in connectorPNs)
                {
                    if (!items.Contains(pn)) items.Add(pn);
                }
                foreach (DataGridViewRow row in dgvDetail.Rows)
                {
                    if (row.IsNewRow) continue;
                    string v = CellText(row, col);
                    if (!items.Contains(v)) items.Add(v);
                }
                if (extra != null)
                {
                    foreach (string v in extra)
                    {
                        string t = (v ?? "").Trim();
                        if (!items.Contains(t)) items.Add(t);
                    }
                }
                col.Items.Clear();
                col.Items.AddRange(items.ToArray());
            }
        }

        #endregion

        private void DrawingItemDetailForm_Load(object sender, EventArgs e)
        {
            switch (mode)
            {
                case FormMode.Create:
                    this.Text = "Drawing Item - Create";
                    txtPrepareBy.Text = MenuFormV2.UserForNextForm;
                    txtRef.Text = DefaultRefText;
                    dtpEffectiveDate.Value = DateTime.Today;
                    dtpEffectiveDate.Checked = false;
                    // sign-off fields are not filled in when the drawing is created
                    foreach (TextBox txt in new[] { txtPE, txtQA, txtApproval, txtRevNo })
                    {
                        txt.ReadOnly = true;
                        txt.TabStop = false;
                        txt.BackColor = Color.White;
                    }
                    dtpEffectiveDate.Enabled = false;
                    break;
                case FormMode.Update:
                    this.Text = "Drawing Item - Update";
                    LoadData();
                    if (txtPE.Text.Trim() != "")
                    {
                        // already accepted by PE -> locked, open as view only
                        mode = FormMode.View;
                        this.Text = "Drawing Item - View (PE Accepted)";
                        SetReadOnly();
                        MessageBox.Show("This drawing item has already been accepted by PE and cannot be updated!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;
                case FormMode.View:
                    this.Text = "Drawing Item - View";
                    LoadData();
                    SetReadOnly();
                    break;
            }
            dgvDetail.ClearSelection();
        }

        #region Grouped column header painting

        private void DgvDetail_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1 || e.ColumnIndex < 0) return;

            e.PaintBackground(e.CellBounds, false);
            Rectangle r = e.CellBounds;
            // "No." spans both header rows, other columns use only the lower half
            if (!IsInGroup(e.ColumnIndex))
            {
                DrawHeaderText(e.Graphics, dgvDetail.Columns[e.ColumnIndex].HeaderText, r);
            }
            else
            {
                int half = r.Height / 2;
                Rectangle lower = new Rectangle(r.X, r.Y + half, r.Width, r.Height - half);
                DrawHeaderText(e.Graphics, dgvDetail.Columns[e.ColumnIndex].HeaderText, lower);
            }
            e.Handled = true;
        }

        private void DgvDetail_Paint(object sender, PaintEventArgs e)
        {
            using (SolidBrush back = new SolidBrush(dgvDetail.ColumnHeadersDefaultCellStyle.BackColor))
            using (Pen border = new Pen(dgvDetail.GridColor))
            {
                foreach (HeaderGroup g in headerGroups)
                {
                    Rectangle union = Rectangle.Empty;
                    for (int c = g.StartCol; c <= g.EndCol; c++)
                    {
                        Rectangle cr = dgvDetail.GetCellDisplayRectangle(c, -1, false);
                        if (cr.IsEmpty) continue;
                        union = union.IsEmpty ? cr : Rectangle.Union(union, cr);
                    }
                    if (union.IsEmpty) continue;

                    Rectangle top = new Rectangle(union.X, union.Y, union.Width, union.Height / 2);
                    e.Graphics.FillRectangle(back, top.X, top.Y, top.Width - 1, top.Height);
                    e.Graphics.DrawLine(border, top.Left, top.Bottom, top.Right - 1, top.Bottom);
                    e.Graphics.DrawLine(border, top.Right - 1, top.Top, top.Right - 1, top.Bottom);
                    DrawHeaderText(e.Graphics, g.Text, top);
                }
            }
        }

        private bool IsInGroup(int colIndex)
        {
            foreach (HeaderGroup g in headerGroups)
            {
                if (colIndex >= g.StartCol && colIndex <= g.EndCol) return true;
            }
            return false;
        }

        private void DrawHeaderText(Graphics g, string text, Rectangle r)
        {
            TextRenderer.DrawText(g, text, dgvDetail.ColumnHeadersDefaultCellStyle.Font, r,
                dgvDetail.ColumnHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
        }

        #endregion

        private void RenumberRows()
        {
            int no = 1;
            foreach (DataGridViewRow row in dgvDetail.Rows)
            {
                if (row.IsNewRow) continue;
                row.Cells[ColNo.Index].Value = no++;
            }
        }

        private void BtnAddRow_Click(object sender, EventArgs e)
        {
            int idx = dgvDetail.Rows.Add();
            dgvDetail.CurrentCell = dgvDetail.Rows[idx].Cells[ColWire.Index];
            dgvDetail.BeginEdit(true);
        }

        private void BtnDeleteRow_Click(object sender, EventArgs e)
        {
            if (dgvDetail.CurrentRow == null || dgvDetail.CurrentRow.IsNewRow) return;
            dgvDetail.Rows.Remove(dgvDetail.CurrentRow);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SetReadOnly()
        {
            foreach (TextBox txt in new[] { txtRef, txtCustomer, txtDocumentNo, txtDrawingNo, txtItemName, txtItemPart,
                                            txtPrepareBy, txtPE, txtQA, txtApproval, txtRevNo })
            {
                txt.ReadOnly = true;
                txt.BackColor = Color.White;
            }
            txtRef.BackColor = SystemColors.Control;
            dtpEffectiveDate.Enabled = false;
            dgvDetail.ReadOnly = true;
            dgvDetail.AllowUserToAddRows = false;
            dgvDetail.AllowUserToDeleteRows = false;
            btnSave.Visible = false;
            btnAddRow.Visible = false;
            btnDeleteRow.Visible = false;
            btnSameArrow.Visible = false;
            dgvDetail.ContextMenuStrip = null;
        }

        private void LoadData()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                cnn.con.Open();
                DataTable dtHead = new DataTable();
                using (SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbDrawingItem WHERE DrawingID = @ID", cnn.con))
                {
                    sda.SelectCommand.Parameters.AddWithValue("@ID", drawingID);
                    sda.Fill(dtHead);
                }
                if (dtHead.Rows.Count == 0)
                {
                    MessageBox.Show("This drawing item no longer exists!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DataRow h = dtHead.Rows[0];
                string refText = h.Table.Columns.Contains("RefText") ? h["RefText"].ToString() : "";
                txtRef.Text = refText.Trim() == "" ? DefaultRefText : refText;
                txtCustomer.Text = h["Customer"].ToString();
                txtDocumentNo.Text = h["DocumentNo"].ToString();
                loadingData = true; // keep the saved Customer / Document No.
                txtDrawingNo.Text = h["DrawingNo"].ToString();
                loadingData = false;
                lastDrawingLookup = txtDrawingNo.Text.Trim();
                txtItemName.Text = h["ItemName"].ToString();
                loadingData = true;
                txtItemPart.Text = h["ItemPart"].ToString();
                loadingData = false;
                lastLookupCode = txtItemPart.Text.Trim();
                txtPrepareBy.Text = h["PrepareBy"].ToString();
                txtPE.Text = h["PEChecker"].ToString();
                txtQA.Text = h["QAChecker"].ToString();
                txtApproval.Text = h["Approval"].ToString();
                txtRevNo.Text = h["RevNo"].ToString();
                if (h["EffectiveDate"] != DBNull.Value)
                {
                    dtpEffectiveDate.Value = Convert.ToDateTime(h["EffectiveDate"]);
                    dtpEffectiveDate.Checked = true;
                }
                else
                {
                    dtpEffectiveDate.Checked = false;
                }

                DataTable dtDetail = new DataTable();
                using (SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbDrawingItemDetail WHERE DrawingID = @ID ORDER BY RowNo", cnn.con))
                {
                    sda.SelectCommand.Parameters.AddWithValue("@ID", drawingID);
                    sda.Fill(dtDetail);
                }
                // P/N dropdown: descriptions of this Wip Code + every P/N already saved
                connectorPNs = QueryConnectorPNs(txtItemPart.Text);
                var savedPNs = new System.Collections.Generic.List<string>();
                foreach (DataRow d in dtDetail.Rows)
                {
                    savedPNs.Add(d["CNA_PN"].ToString());
                    savedPNs.Add(d["CNB_PN"].ToString());
                }
                dgvDetail.Rows.Clear();
                ApplyPNItems(savedPNs);

                bool hasMask = dtDetail.Columns.Contains("SameMask");
                foreach (DataRow d in dtDetail.Rows)
                {
                    int idx = dgvDetail.Rows.Add(null, d["WireCable"], d["WireColor"], d["WireLength"],
                        d["CNA_Pin"], d["CNA_Strip"], d["CNA_PN"].ToString().Trim(),
                        d["CNB_Strip"], d["CNB_PN"].ToString().Trim(), d["CNB_Pin"]);
                    if (hasMask && d["SameMask"] != DBNull.Value)
                    {
                        dgvDetail.Rows[idx].Tag = Convert.ToInt32(d["SameMask"]);
                    }
                }
                RenumberRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong! Please check the network connection.\n" + ex.Message, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (cnn.con.State == ConnectionState.Open) cnn.con.Close();
                Cursor = Cursors.Default;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            dgvDetail.EndEdit();

            if (txtItemName.Text.Trim() == "")
            {
                MessageBox.Show("Please input Wip Name!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtItemName.Focus();
                return;
            }

            string question = mode == FormMode.Create ? "Do you want to save this drawing item?" : "Do you want to update this drawing item?";
            if (MessageBox.Show(question, MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            Cursor = Cursors.WaitCursor;
            SqlTransaction tran = null;
            try
            {
                cnn.con.Open();
                tran = cnn.con.BeginTransaction();
                string user = MenuFormV2.UserForNextForm ?? "";
                DateTime now = DateTime.Now;
                int id = drawingID;

                // PE may have accepted while this form was open -> block the update
                if (mode != FormMode.Create)
                {
                    SqlCommand chk = new SqlCommand("SELECT ISNULL(PEChecker, '') FROM tbDrawingItem WITH (UPDLOCK, HOLDLOCK) WHERE DrawingID = @ID", cnn.con, tran);
                    chk.Parameters.AddWithValue("@ID", id);
                    object pe = chk.ExecuteScalar();
                    if (pe != null && pe.ToString().Trim() != "")
                    {
                        tran.Rollback();
                        cnn.con.Close();
                        Cursor = Cursors.Default;
                        MessageBox.Show("This drawing item has already been accepted by PE and cannot be updated!",
                            MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // One active drawing item per Wip Code
                string wipCode = txtItemPart.Text.Trim();
                if (wipCode != "")
                {
                    SqlCommand dup = new SqlCommand("SELECT TOP 1 ItemName FROM tbDrawingItem WITH (UPDLOCK, HOLDLOCK) " +
                        "WHERE ItemPart = @Code AND Status = 'Active' AND DrawingID <> @ID", cnn.con, tran);
                    dup.Parameters.AddWithValue("@Code", wipCode);
                    dup.Parameters.AddWithValue("@ID", mode == FormMode.Create ? 0 : id);
                    object existing = dup.ExecuteScalar();
                    if (existing != null)
                    {
                        tran.Rollback();
                        cnn.con.Close();
                        Cursor = Cursors.Default;
                        MessageBox.Show("Wip Code \"" + wipCode + "\" already exists (" + existing + ")!\nCannot create a duplicate Wip Code.",
                            MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtItemPart.Focus();
                        return;
                    }
                }

                SqlCommand cmd;
                if (mode == FormMode.Create)
                {
                    cmd = new SqlCommand("INSERT INTO tbDrawingItem (ItemName, ItemPart, Customer, DocumentNo, DrawingNo, " +
                        "PrepareBy, PEChecker, QAChecker, Approval, EffectiveDate, RevNo, RefText, Status, CreateDate, CreateBy, UpdateDate, UpdateBy) " +
                        "VALUES (@ItemName, @ItemPart, @Customer, @DocumentNo, @DrawingNo, " +
                        "@PrepareBy, @PEChecker, @QAChecker, @Approval, @EffectiveDate, @RevNo, @RefText, 'Active', @Now, @User, @Now, @User); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);", cnn.con, tran);
                }
                else
                {
                    cmd = new SqlCommand("UPDATE tbDrawingItem SET ItemName = @ItemName, ItemPart = @ItemPart, Customer = @Customer, " +
                        "DocumentNo = @DocumentNo, DrawingNo = @DrawingNo, PrepareBy = @PrepareBy, PEChecker = @PEChecker, " +
                        "QAChecker = @QAChecker, Approval = @Approval, EffectiveDate = @EffectiveDate, RevNo = @RevNo, RefText = @RefText, " +
                        "UpdateDate = @Now, UpdateBy = @User WHERE DrawingID = @ID", cnn.con, tran);
                    cmd.Parameters.AddWithValue("@ID", id);
                }
                cmd.Parameters.AddWithValue("@ItemName", txtItemName.Text.Trim());
                cmd.Parameters.AddWithValue("@ItemPart", txtItemPart.Text.Trim());
                cmd.Parameters.AddWithValue("@Customer", txtCustomer.Text.Trim());
                cmd.Parameters.AddWithValue("@DocumentNo", txtDocumentNo.Text.Trim());
                cmd.Parameters.AddWithValue("@DrawingNo", txtDrawingNo.Text.Trim());
                cmd.Parameters.AddWithValue("@PrepareBy", txtPrepareBy.Text.Trim());
                cmd.Parameters.AddWithValue("@PEChecker", txtPE.Text.Trim());
                cmd.Parameters.AddWithValue("@QAChecker", txtQA.Text.Trim());
                cmd.Parameters.AddWithValue("@Approval", txtApproval.Text.Trim());
                cmd.Parameters.AddWithValue("@EffectiveDate", dtpEffectiveDate.Checked ? (object)dtpEffectiveDate.Value.Date : DBNull.Value);
                cmd.Parameters.AddWithValue("@RevNo", txtRevNo.Text.Trim());
                cmd.Parameters.AddWithValue("@RefText", txtRef.Text.Trim());
                cmd.Parameters.AddWithValue("@Now", now);
                cmd.Parameters.AddWithValue("@User", user);

                if (mode == FormMode.Create)
                {
                    id = (int)cmd.ExecuteScalar();
                }
                else
                {
                    cmd.ExecuteNonQuery();
                    SqlCommand del = new SqlCommand("DELETE FROM tbDrawingItemDetail WHERE DrawingID = @ID", cnn.con, tran);
                    del.Parameters.AddWithValue("@ID", id);
                    del.ExecuteNonQuery();
                }

                int rowNo = 1;
                foreach (DataGridViewRow row in dgvDetail.Rows)
                {
                    if (row.IsNewRow || IsRowEmpty(row)) continue;

                    SqlCommand ins = new SqlCommand("INSERT INTO tbDrawingItemDetail (DrawingID, RowNo, WireCable, WireColor, WireLength, " +
                        "CNA_Pin, CNA_Strip, CNA_PN, CNB_Strip, CNB_PN, CNB_Pin, SameMask) " +
                        "VALUES (@ID, @RowNo, @WireCable, @WireColor, @WireLength, @CNA_Pin, @CNA_Strip, @CNA_PN, @CNB_Strip, @CNB_PN, @CNB_Pin, @SameMask)", cnn.con, tran);
                    ins.Parameters.AddWithValue("@ID", id);
                    ins.Parameters.AddWithValue("@RowNo", rowNo++);
                    ins.Parameters.AddWithValue("@WireCable", CellText(row, ColWire));
                    ins.Parameters.AddWithValue("@WireColor", CellText(row, ColColor));
                    ins.Parameters.AddWithValue("@WireLength", CellText(row, ColLength));
                    ins.Parameters.AddWithValue("@CNA_Pin", CellText(row, ColCNAPin));
                    ins.Parameters.AddWithValue("@CNA_Strip", CellText(row, ColCNAStrip));
                    ins.Parameters.AddWithValue("@CNA_PN", CellText(row, ColCNAPN));
                    ins.Parameters.AddWithValue("@CNB_Strip", CellText(row, ColCNBStrip));
                    ins.Parameters.AddWithValue("@CNB_PN", CellText(row, ColCNBPN));
                    ins.Parameters.AddWithValue("@CNB_Pin", CellText(row, ColCNBPin));
                    ins.Parameters.AddWithValue("@SameMask", rowNo == 2 ? 0 : GetMask(row)); // first saved row can't point above
                    ins.ExecuteNonQuery();
                }

                tran.Commit();
                cnn.con.Close();
                Cursor = Cursors.Default;
                MessageBox.Show(mode == FormMode.Create ? "Saved successfully!" : "Updated successfully!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                try { tran?.Rollback(); } catch { }
                Cursor = Cursors.Default;
                MessageBox.Show("Save failed!\n" + ex.Message, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (cnn.con.State == ConnectionState.Open) cnn.con.Close();
            }
        }

        #region "Same as above" arrows

        private int GetMask(DataGridViewRow row)
        {
            return row.Tag is int ? (int)row.Tag : 0;
        }

        private int DataColumnBit(int columnIndex)
        {
            if (columnIndex < 0) return -1;
            return Array.IndexOf(dataColumns, dgvDetail.Columns[columnIndex]);
        }

        private bool IsSame(int rowIndex, int columnIndex)
        {
            if (rowIndex <= 0 || rowIndex >= dgvDetail.Rows.Count) return false;
            DataGridViewRow row = dgvDetail.Rows[rowIndex];
            int bit = DataColumnBit(columnIndex);
            if (row.IsNewRow || bit < 0) return false;
            return (GetMask(row) & (1 << bit)) != 0;
        }

        private void SetSame(DataGridViewRow row, int columnIndex, bool on)
        {
            int bit = DataColumnBit(columnIndex);
            if (bit < 0) return;
            int mask = GetMask(row);
            mask = on ? (mask | (1 << bit)) : (mask & ~(1 << bit));
            row.Tag = mask;
        }

        // Select cells (e.g. rows 2-4 of a column) and toggle: mark them "same as above" or clear the arrow
        private void ToggleSameArrow()
        {
            if (dgvDetail.ReadOnly) return;
            dgvDetail.EndEdit();

            var targets = new System.Collections.Generic.List<DataGridViewCell>();
            foreach (DataGridViewCell cell in dgvDetail.SelectedCells)
            {
                if (cell.RowIndex <= 0 || dgvDetail.Rows[cell.RowIndex].IsNewRow) continue;
                if (DataColumnBit(cell.ColumnIndex) < 0) continue;
                targets.Add(cell);
            }
            if (targets.Count == 0)
            {
                MessageBox.Show("Select the cells below the first value (e.g. rows 2-4 of a column), then click \"Same as above\".",
                    MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool allSame = targets.TrueForAll(c => IsSame(c.RowIndex, c.ColumnIndex));
            // top-down so each cell copies a value that is already final
            targets.Sort((a, b) => a.RowIndex != b.RowIndex ? a.RowIndex.CompareTo(b.RowIndex) : a.ColumnIndex.CompareTo(b.ColumnIndex));
            foreach (DataGridViewCell cell in targets)
            {
                DataGridViewRow row = dgvDetail.Rows[cell.RowIndex];
                SetSame(row, cell.ColumnIndex, !allSame);
                if (!allSame)
                {
                    cell.Value = dgvDetail.Rows[cell.RowIndex - 1].Cells[cell.ColumnIndex].Value;
                }
            }
            dgvDetail.Invalidate();
        }

        // Wire text code -> Color (checked in this order; matched as a whole word, case-insensitive)
        private static readonly string[,] WireColorMap =
        {
            { "RED", "Red (ក្រហម)" }, { "BLK", "Black (ខ្មៅ)" }, { "PINK", "Pink (ពណ៌ផ្កាឈូក)" }, { "YEL", "Yellow (លឿន)" },
            { "BLU", "Blue (ខៀវ)" }, { "BRN", "Brown (ពណ៌ត្នោត)" }, { "G/Y", "GreenYellow (ពណ៌ត្រួយចេក)" }, { "GRN", "Green (បៃតង)" },
            { "GRY", "Gray (ពណ៌ប្រផេះ)" }, { "ORG", "Orange (ពណ៌ទឹកក្រូច)" }, { "PNK", "Pink (ពណ៌ផ្កាឈូក)" }, { "SKY", "SkyBlue (​ពណ៌ផ្ទៃមេឃ)" },
            { "VLT", "Purple (ពណ៌ស្វាយ)" }, { "WHT", "White (ពណ៌ស)" }
        };

        private static string ColorFromWire(string wire)
        {
            if (string.IsNullOrWhiteSpace(wire)) return null;
            for (int i = 0; i < WireColorMap.GetLength(0); i++)
            {
                string pattern = @"(?<![A-Za-z0-9])" + System.Text.RegularExpressions.Regex.Escape(WireColorMap[i, 0]) + @"(?![A-Za-z0-9])";
                if (System.Text.RegularExpressions.Regex.IsMatch(wire, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    return WireColorMap[i, 1];
            }
            return null;
        }

        private void DgvDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (syncingSame || e.RowIndex < 0 || DataColumnBit(e.ColumnIndex) < 0) return;
            DataGridViewRow row = dgvDetail.Rows[e.RowIndex];
            object value = row.Cells[e.ColumnIndex].Value;

            // a wire that names a color fills the Color cell automatically
            if (e.ColumnIndex == ColWire.Index && !loadingData && mode != FormMode.View)
            {
                string color = ColorFromWire(Convert.ToString(value));
                if (color != null && Convert.ToString(row.Cells[ColColor.Index].Value) != color)
                {
                    row.Cells[ColColor.Index].Value = color;
                }
            }

            // typing a different value into an arrow cell removes its arrow
            if (IsSame(e.RowIndex, e.ColumnIndex))
            {
                object above = dgvDetail.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value;
                if (Convert.ToString(above) != Convert.ToString(value))
                {
                    SetSame(row, e.ColumnIndex, false);
                    dgvDetail.InvalidateColumn(e.ColumnIndex);
                }
            }

            // push the new value down through the arrow cells below
            syncingSame = true;
            try
            {
                for (int r = e.RowIndex + 1; IsSame(r, e.ColumnIndex); r++)
                {
                    dgvDetail.Rows[r].Cells[e.ColumnIndex].Value = value;
                }
            }
            finally { syncingSame = false; }
        }

        // after a row is deleted, arrow cells take the value of their new "above" cell
        private void ResyncSameValues()
        {
            if (dataColumns == null || dgvDetail.Rows.Count == 0) return;
            syncingSame = true;
            try
            {
                DataGridViewRow first = dgvDetail.Rows[0];
                if (!first.IsNewRow) first.Tag = 0;
                for (int r = 1; r < dgvDetail.Rows.Count; r++)
                {
                    foreach (DataGridViewColumn col in dataColumns)
                    {
                        if (IsSame(r, col.Index))
                        {
                            dgvDetail.Rows[r].Cells[col.Index].Value = dgvDetail.Rows[r - 1].Cells[col.Index].Value;
                        }
                    }
                }
            }
            finally { syncingSame = false; }
            dgvDetail.Invalidate();
        }

        private void DgvDetail_CellPaintingArrow(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || !IsSame(e.RowIndex, e.ColumnIndex)) return;

            // "same as above" cells show only the arrow, not the repeated text (the value is still kept for saving)
            e.Paint(e.ClipBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

            Rectangle r = e.CellBounds;
            int x = r.Left + r.Width / 2;
            bool last = !IsSame(e.RowIndex + 1, e.ColumnIndex);
            int bottom = last ? r.Bottom - 2 : r.Bottom;
            const int head = 7;

            Graphics g = e.Graphics;
            SmoothingMode old = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen pen = new Pen(Color.Black, 1.2f))
            {
                g.DrawLine(pen, x, r.Top - 1, x, last ? bottom - head + 1 : bottom);
            }
            if (last)
            {
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    g.FillPolygon(brush, new[] { new Point(x - 4, bottom - head), new Point(x + 4, bottom - head), new Point(x, bottom) });
                }
            }
            g.SmoothingMode = old;
            e.Handled = true;
        }

        private void DgvDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D)
            {
                ToggleSameArrow();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        // right-click selects the cell under the mouse (unless it is already part of the selection)
        private void DgvDetail_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.RowIndex < 0 || e.ColumnIndex < 0) return;
            DataGridViewCell cell = dgvDetail.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (!cell.Selected)
            {
                dgvDetail.ClearSelection();
                cell.Selected = true;
            }
        }

        private void BuildGridContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem miSame = new ToolStripMenuItem("Same as above ↓  /  clear arrow", null, (s, e) => ToggleSameArrow());
            miSame.ShortcutKeyDisplayString = "Ctrl+D";
            menu.Items.Add(miSame);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(new ToolStripMenuItem("Add Row", null, (s, e) => BtnAddRow_Click(s, e)));
            menu.Items.Add(new ToolStripMenuItem("Delete Row", null, (s, e) => BtnDeleteRow_Click(s, e)));
            dgvDetail.ContextMenuStrip = menu;
        }

        #endregion

        private string CellText(DataGridViewRow row, DataGridViewColumn col)
        {
            object v = row.Cells[col.Index].Value;
            return v == null ? "" : v.ToString().Trim();
        }

        private bool IsRowEmpty(DataGridViewRow row)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.ColumnIndex == ColNo.Index) continue;
                if (cell.Value != null && cell.Value.ToString().Trim() != "") return false;
            }
            return true;
        }
    }
}
