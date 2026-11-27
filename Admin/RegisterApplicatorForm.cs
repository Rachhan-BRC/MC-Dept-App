using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MachineDeptApp.OtherClass;

namespace MachineDeptApp.Admin
{
    //Same process as MachineTypeMasterForm:
    //  Search / New / Edit / Delete / Export on the toolbar (Edit + Delete only when a row is selected)
    //  New or Edit opens the edit panel at the bottom, which has its own Save and Close buttons
    public partial class RegisterApplicatorForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        ExportAsCSVClass CSV = new ExportAsCSVClass();
        string ErrorText;

        //"" = edit panel closed, "Add" = registering a new applicator, "Edit" = editing EditSysNo
        string Mode = "";
        int EditSysNo = 0;
        string EditApplicator = "";

        //Colours of the page
        static readonly Color ClrBg = Color.FromArgb(246, 247, 251);
        static readonly Color ClrBorder = Color.FromArgb(229, 231, 238);
        static readonly Color ClrText = Color.FromArgb(30, 34, 48);
        static readonly Color ClrSubText = Color.FromArgb(107, 112, 128);
        static readonly Color ClrBlue = SystemColors.Highlight;             //same blue as the selected row
        static readonly Color ClrBlueTint = Color.FromArgb(225, 239, 252);
        static readonly Color ClrHover = Color.FromArgb(229, 241, 251);
        static readonly Color ClrDown = Color.FromArgb(204, 228, 247);
        static readonly Color ClrDangerHover = Color.FromArgb(253, 232, 232);   //Delete button: light red on hover
        static readonly Color ClrDangerDown = Color.FromArgb(250, 208, 208);

        //Colour icon of each button (sized from the button, minus this margin), and its grey copy for when the button is off
        const string MsgTitle = "System Alert";
        const int IconMargin = 10;
        Dictionary<Button, Image> BtnIcon = new Dictionary<Button, Image>();
        Dictionary<Button, Image> BtnIconGray = new Dictionary<Button, Image>();

        public RegisterApplicatorForm()
        {
            InitializeComponent();
            ApplyTheme();
            this.cnn.Connection();
            this.Shown += RegisterApplicatorForm_Shown;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnRegister.Click += BtnNew_Click;
            this.btnEdit.Click += BtnEdit_Click;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnExport.Click += BtnExport_Click;
            this.btnSave.Click += BtnSave_Click;
            this.btnCancel.Click += BtnClose_Click;
            this.txtSearch.KeyDown += TxtSearch_KeyDown;
            this.txtApplicator.KeyDown += TxtEdit_KeyDown;
            this.txtRemark.KeyDown += TxtEdit_KeyDown;
            this.dgvData.SelectionChanged += DgvData_SelectionChanged;
            this.dgvData.CellDoubleClick += DgvData_CellDoubleClick;
        }

        private void RegisterApplicatorForm_Shown(object sender, EventArgs e)
        {
            CloseEditPanel();
            Search();
            txtSearch.Focus();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            //Enter = Search
            if (e.KeyCode == Keys.Enter && Mode == "")
            {
                e.SuppressKeyPress = true;
                Search();
            }
        }

        private void TxtEdit_KeyDown(object sender, KeyEventArgs e)
        {
            //Enter in the edit panel = Save
            if (e.KeyCode == Keys.Enter && Mode != "")
            {
                e.SuppressKeyPress = true;
                BtnSave_Click(sender, e);
            }
        }

        private void DgvData_SelectionChanged(object sender, EventArgs e)
        {
            //Edit and Delete are only available when a row is selected
            UpdateButtons();
        }

        private void DgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && Mode == "")
                BtnEdit_Click(sender, e);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            //Nothing highlighted, so it doesn't look like an existing row is being edited
            dgvData.ClearSelection();
            dgvData.CurrentCell = null;
            ClearInput();
            Mode = "Add";
            EditSysNo = 0;
            EditApplicator = "";
            OpenEditPanel();
            txtApplicator.Focus();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = SelectedRow();
            if (row == null)
                return;

            Mode = "Edit";
            EditSysNo = Convert.ToInt32(row.Cells["sysno"].Value);
            EditApplicator = row.Cells["applicator"].Value?.ToString() ?? "";
            txtApplicator.Text = EditApplicator;
            txtRemark.Text = row.Cells["remark"].Value?.ToString() ?? "";
            OpenEditPanel();
            txtApplicator.Focus();
            txtApplicator.SelectAll();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (Mode == "Add")
                SaveToDB();
            else if (Mode == "Edit")
                UpdateToDB();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            CloseEditPanel();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DataGridViewRow DelRow = SelectedRow();
            if (DelRow == null)
                return;

            int SysNo = Convert.ToInt32(DelRow.Cells["sysno"].Value);
            string Applicator = DelRow.Cells["applicator"].Value?.ToString() ?? "";

            //Default answer is "No" so pressing Enter by mistake doesn't delete
            DialogResult DLR = MessageBox.Show("Are you sure you want to delete this Applicator?\n\n" + Applicator,
                MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (DLR != DialogResult.Yes)
                return;

            ErrorText = "";
            int Deleted = 0;
            Cursor = Cursors.WaitCursor;
            try
            {
                cnn.con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM tbMasterApplicator WHERE SysNo = @id", cnn.con);
                cmd.Parameters.AddWithValue("@id", SysNo);
                Deleted = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            finally
            {
                cnn.con.Close();
            }
            Cursor = Cursors.Default;

            if (ErrorText != "")
            {
                MessageBox.Show("Cannot delete this Applicator.\nPlease contact Phanun for support.\n" + ErrorText, MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Remove it from the grid (also when someone else had already deleted it)
            dgvData.Rows.Remove(DelRow);
            dgvData.ClearSelection();
            dgvData.CurrentCell = null;
            UpdateCount();
            UpdateButtons();

            if (Deleted > 0)
                MessageBox.Show("Deleted successfully.", MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("This Applicator was already deleted.", MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult DLS = MessageBox.Show("Do you want to export the data?", MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DLS != DialogResult.Yes)
                return;

            CSV.CSVErrorT = "";
            CSV.ExportAsCSV(this.dgvData, "Applicator_List", 0);
            if (CSV.CSVErrorT.Trim() != "")
            {
                if (CSV.CSVErrorT.Trim() == "OK")
                    MessageBox.Show("Exported successfully.", MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Something went wrong!\n" + CSV.CSVErrorT, MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //New applicator: insert, add to the grid, keep the panel open for the next one
        private void SaveToDB()
        {
            string Applicator = txtApplicator.Text.Trim();
            string Remark = txtRemark.Text.Trim();

            if (Applicator == "")
            {
                MessageBox.Show("Please input Applicator.", MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApplicator.Focus();
                return;
            }

            DialogResult DLR = MessageBox.Show("Are you sure you want to register this Applicator?\n\n" + Applicator, MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DLR != DialogResult.Yes)
                return;

            ErrorText = "";
            bool Duplicate = false;
            int NewSysNo = 0;
            DateTime RegDate = DateTime.Now;
            Cursor = Cursors.WaitCursor;
            try
            {
                cnn.con.Open();
                SqlCommand cmdCheck = new SqlCommand("SELECT COUNT(*) FROM tbMasterApplicator WHERE Applicator = @app", cnn.con);
                cmdCheck.Parameters.AddWithValue("@app", Applicator);
                Duplicate = Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0;

                if (!Duplicate)
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO tbMasterApplicator (Applicator, Remark, RegDate, RegBy, UpdateDate, UpdateBy) " +
                                                    "OUTPUT INSERTED.SysNo " +
                                                    "VALUES (@app, @rmk, @RegD, @RegB, @UpD, @UpB)", cnn.con);
                    cmd.Parameters.AddWithValue("@app", Applicator);
                    cmd.Parameters.AddWithValue("@rmk", Remark == "" ? (object)DBNull.Value : Remark);
                    cmd.Parameters.AddWithValue("@RegD", RegDate);
                    cmd.Parameters.AddWithValue("@RegB", MenuFormV2.UserForNextForm);
                    cmd.Parameters.AddWithValue("@UpD", RegDate);
                    cmd.Parameters.AddWithValue("@UpB", MenuFormV2.UserForNextForm);
                    NewSysNo = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            finally
            {
                cnn.con.Close();
            }
            Cursor = Cursors.Default;

            if (ErrorText != "")
            {
                MessageBox.Show("Cannot register this Applicator.\nPlease contact Phanun for support.\n" + ErrorText, MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Duplicate)
            {
                MessageBox.Show("This Applicator is already registered.\n\n" + Applicator, MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApplicator.Focus();
                txtApplicator.SelectAll();
                return;
            }

            //Add the new row to the grid
            int NewIndex = dgvData.Rows.Add(NewSysNo, Applicator, Remark, RegDate, MenuFormV2.UserForNextForm, RegDate, MenuFormV2.UserForNextForm);
            dgvData.ClearSelection();
            dgvData.CurrentCell = null;
            try
            {
                dgvData.FirstDisplayedScrollingRowIndex = NewIndex;
            }
            catch { }
            UpdateCount();

            MessageBox.Show("Registered successfully.", MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

            //Ready for the next one
            ClearInput();
            txtApplicator.Focus();
        }

        //Edit applicator: update, refresh the row, close the panel
        private void UpdateToDB()
        {
            string Applicator = txtApplicator.Text.Trim();
            string Remark = txtRemark.Text.Trim();

            if (Applicator == "")
            {
                MessageBox.Show("Please input Applicator.", MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApplicator.Focus();
                return;
            }

            DataGridViewRow EditRow = FindRowBySysNo(EditSysNo);
            if (EditRow != null &&
                Applicator == (EditRow.Cells["applicator"].Value?.ToString() ?? "") &&
                Remark == (EditRow.Cells["remark"].Value?.ToString() ?? ""))
            {
                MessageBox.Show("Nothing has changed.", MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult DLR = MessageBox.Show("Are you sure you want to update this Applicator?", MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DLR != DialogResult.Yes)
                return;

            ErrorText = "";
            bool Duplicate = false;
            DateTime UpdateDate = DateTime.Now;
            Cursor = Cursors.WaitCursor;
            try
            {
                cnn.con.Open();
                SqlCommand cmdCheck = new SqlCommand("SELECT COUNT(*) FROM tbMasterApplicator WHERE Applicator = @app AND SysNo <> @id", cnn.con);
                cmdCheck.Parameters.AddWithValue("@app", Applicator);
                cmdCheck.Parameters.AddWithValue("@id", EditSysNo);
                Duplicate = Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0;

                if (!Duplicate)
                {
                    SqlCommand cmd = new SqlCommand("UPDATE tbMasterApplicator SET Applicator = @app, Remark = @rmk, UpdateDate = @UpD, UpdateBy = @UpB WHERE SysNo = @id", cnn.con);
                    cmd.Parameters.AddWithValue("@app", Applicator);
                    cmd.Parameters.AddWithValue("@rmk", Remark == "" ? (object)DBNull.Value : Remark);
                    cmd.Parameters.AddWithValue("@UpD", UpdateDate);
                    cmd.Parameters.AddWithValue("@UpB", MenuFormV2.UserForNextForm);
                    cmd.Parameters.AddWithValue("@id", EditSysNo);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            finally
            {
                cnn.con.Close();
            }
            Cursor = Cursors.Default;

            if (ErrorText != "")
            {
                MessageBox.Show("Cannot update this Applicator.\nPlease contact Phanun for support.\n" + ErrorText, MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Duplicate)
            {
                MessageBox.Show("This Applicator is already registered.\n\n" + Applicator, MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApplicator.Focus();
                txtApplicator.SelectAll();
                return;
            }

            //Update the row in the grid
            if (EditRow != null)
            {
                EditRow.Cells["applicator"].Value = Applicator;
                EditRow.Cells["remark"].Value = Remark;
                EditRow.Cells["updatedate"].Value = UpdateDate;
                EditRow.Cells["updateby"].Value = MenuFormV2.UserForNextForm;
            }

            MessageBox.Show("Updated successfully.", MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            CloseEditPanel();
        }

        private void Search()
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "Searching…";
            LbStatus.Refresh();

            DataTable dt = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT SysNo, Applicator, Remark, RegDate, RegBy, UpdateDate, UpdateBy FROM tbMasterApplicator ";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.con;
                if (txtSearch.Text.Trim() != "")
                {
                    //One box finds by Applicator or Remark
                    SQLQuery += "WHERE Applicator LIKE @key OR Remark LIKE @key ";
                    cmd.Parameters.AddWithValue("@key", "%" + EscapeLike(txtSearch.Text.Trim()) + "%");
                }
                SQLQuery += "ORDER BY Applicator ASC";
                cmd.CommandText = SQLQuery;

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            finally
            {
                cnn.con.Close();
            }

            dgvData.Rows.Clear();
            foreach (DataRow row in dt.Rows)
            {
                dgvData.Rows.Add(
                    row["SysNo"],
                    row["Applicator"].ToString(),
                    row["Remark"].ToString(),
                    row["RegDate"] == DBNull.Value ? null : row["RegDate"],
                    row["RegBy"].ToString(),
                    row["UpdateDate"] == DBNull.Value ? null : row["UpdateDate"],
                    row["UpdateBy"].ToString());
            }
            dgvData.ClearSelection();
            dgvData.CurrentCell = null;
            UpdateButtons();
            Cursor = Cursors.Default;

            if (ErrorText != "")
            {
                LbStatus.Text = "";
                MessageBox.Show("Cannot search.\nPlease contact Phanun for support.\n" + ErrorText, MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                UpdateCount();
            }
        }

        //Show the edit panel and lock the toolbar + grid until Save/Close
        private void OpenEditPanel()
        {
            cardInput.Visible = true;
            spacerBottom.Visible = true;
            dgvData.Enabled = false;
            UpdateButtons();
        }

        //Hide the edit panel and give the toolbar + grid back
        private void CloseEditPanel()
        {
            Mode = "";
            EditSysNo = 0;
            EditApplicator = "";
            ClearInput();
            cardInput.Visible = false;
            spacerBottom.Visible = false;
            dgvData.Enabled = true;

            //After Save or Close: no row selected (Edit and Delete go grey again)
            dgvData.ClearSelection();
            dgvData.CurrentCell = null;
            UpdateButtons();
        }

        //Panel closed : Search, New, Export (+ Edit and Delete when a row is selected)
        //Panel open   : only Save and Close in the panel (+ Export)
        private void UpdateButtons()
        {
            bool PanelOpen = Mode != "";
            bool RowSelected = SelectedRow() != null;

            SetButton(btnSearch, !PanelOpen);
            SetButton(btnRegister, !PanelOpen);
            SetButton(btnEdit, !PanelOpen && RowSelected);
            SetButton(btnDelete, !PanelOpen && RowSelected);
            SetButton(btnExport, true);
            SetButton(btnSave, PanelOpen);
            SetButton(btnCancel, PanelOpen);

            lbMode.Text = Mode == "Edit" ? "Editing  " + EditApplicator : "Register new applicator";
            lbMode.Invalidate();
        }

        private void SetButton(Button btn, bool Enabled)
        {
            btn.Enabled = Enabled;
            btn.Invalidate();
        }

        private void ClearInput()
        {
            txtApplicator.Text = "";
            txtRemark.Text = "";
        }

        private void UpdateCount()
        {
            int n = dgvData.Rows.Count;
            LbStatus.Text = n == 1 ? "1 applicator" : n.ToString("N0") + " applicators";
        }

        private DataGridViewRow SelectedRow()
        {
            return dgvData.SelectedRows.Count > 0 ? dgvData.SelectedRows[0] : null;
        }

        private DataGridViewRow FindRowBySysNo(int SysNo)
        {
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (Convert.ToInt32(row.Cells["sysno"].Value) == SysNo)
                    return row;
            }
            return null;
        }

        //Make [ % _ match literally inside LIKE
        private string EscapeLike(string Value)
        {
            return Value.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
        }

        #region Look & feel (cards, input boxes, icon buttons, grid)

        //Hint text colour inside empty input boxes (lighter than the Windows default)
        static readonly Color ClrHint = Color.FromArgb(168, 173, 186);
        List<PlaceholderPainter> Placeholders = new List<PlaceholderPainter>();

        private void ApplyTheme()
        {
            foreach (Control c in new Control[] { panelHeader, panelToolbar, cardSearch, cardInput, cardGrid, shellSearch, shellApplicator, shellRemark, lbMode, panelEditButtons, dgvData })
            {
                //Smooth repaint, and repaint everything when resized (rounded corners)
                typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(c, true, null);
                if (!(c is DataGridView))
                    c.Resize += (s, e) => ((Control)s).Invalidate();
            }

            panelHeader.Paint += PanelHeader_Paint;
            panelToolbar.Paint += PanelToolbar_Paint;
            cardSearch.Paint += Card_Paint;
            cardInput.Paint += Card_Paint;
            cardGrid.Paint += Card_Paint;
            shellSearch.Paint += Shell_Paint;
            shellApplicator.Paint += Shell_Paint;
            shellRemark.Paint += Shell_Paint;
            lbMode.Paint += LbMode_Paint;

            //Input boxes: click anywhere in the box to type, highlight the box while typing
            foreach (TextBox txt in new TextBox[] { txtSearch, txtApplicator, txtRemark })
            {
                TextBox box = txt;
                Control shell = box.Parent;
                shell.Click += (s, e) => box.Focus();
                box.Enter += (s, e) => shell.Invalidate();
                box.Leave += (s, e) => shell.Invalidate();
            }
            SetPlaceholder(txtSearch, "Search applicator or remark  ·  Enter");
            SetPlaceholder(txtApplicator, "Type an applicator");
            SetPlaceholder(txtRemark, "Add a note");
            lbRemarkOptional.Left = lbRemarkCap.Right + 2;

            //Buttons: colour icons, soft blue background on hover
            foreach (Button btn in new Button[] { btnSearch, btnRegister, btnEdit, btnDelete, btnExport, btnSave, btnCancel })
                StyleToolButton(btn);

            //Grid: no dotted focus box, friendly message when empty
            dgvData.RowPrePaint += (s, e) => e.PaintParts &= ~DataGridViewPaintParts.Focus;
            dgvData.Paint += DgvData_Paint;
        }

        private void SetPlaceholder(TextBox txt, string text)
        {
            //Light grey hint text shown while the box is empty
            Placeholders.Add(new PlaceholderPainter(txt, text, ClrHint));
        }

        //Draws the hint on top of an empty TextBox after it paints itself
        //(Windows' own cue banner can't change colour, so it is drawn here instead)
        private sealed class PlaceholderPainter : NativeWindow
        {
            const int WM_PAINT = 0x000F;
            readonly TextBox Box;
            readonly string Hint;
            readonly Color HintColor;

            public PlaceholderPainter(TextBox box, string hint, Color hintColor)
            {
                Box = box;
                Hint = hint;
                HintColor = hintColor;
                if (box.IsHandleCreated)
                    AssignHandle(box.Handle);
                box.HandleCreated += (s, e) => AssignHandle(Box.Handle);
                box.HandleDestroyed += (s, e) => ReleaseHandle();
                box.TextChanged += (s, e) => Box.Invalidate();
                box.Enter += (s, e) => Box.Invalidate();
                box.Leave += (s, e) => Box.Invalidate();
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if (m.Msg == WM_PAINT && Box.TextLength == 0)
                {
                    using (Graphics g = Graphics.FromHwnd(Handle))
                        TextRenderer.DrawText(g, Hint, Box.Font, new Rectangle(1, 0, Box.ClientSize.Width - 1, Box.ClientSize.Height), HintColor,
                            TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
                }
            }
        }

        private void StyleToolButton(Button btn)
        {
            //Take the icon set in the designer, shrink it once to fit the button (sharp + fast), keep a grey copy for "off"
            //(the icon follows the button size, so a bigger button in the designer = a bigger icon)
            if (btn.BackgroundImage != null)
            {
                int size = Math.Max(16, Math.Min(btn.Width, btn.Height) - IconMargin);
                Bitmap small = new Bitmap(size, size);
                using (Graphics g = Graphics.FromImage(small))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    Image src = btn.BackgroundImage;
                    float scale = Math.Min((float)size / src.Width, (float)size / src.Height);
                    float w = src.Width * scale, h = src.Height * scale;
                    g.DrawImage(src, (size - w) / 2, (size - h) / 2, w, h);
                }
                BtnIcon[btn] = small;
                BtnIconGray[btn] = ToolStripRenderer.CreateDisabledImage(small);
                btn.BackgroundImage = null;
            }
            btn.Text = "";
            btn.Cursor = Cursors.Hand;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.Invalidate();
            btn.MouseLeave += (s, e) => btn.Invalidate();
            btn.MouseDown += (s, e) => btn.Invalidate();
            btn.MouseUp += (s, e) => btn.Invalidate();
            btn.Paint += ToolButton_Paint;
        }

        private void ToolButton_Paint(object sender, PaintEventArgs e)
        {
            Button btn = (Button)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(btn.Parent.BackColor);

            bool hover = btn.Enabled && btn.ClientRectangle.Contains(btn.PointToClient(Cursor.Position));
            bool down = hover && (Control.MouseButtons & MouseButtons.Left) != 0;

            if (hover)
            {
                bool danger = btn == btnDelete;
                using (GraphicsPath path = RoundRect(new RectangleF(0, 0, btn.Width, btn.Height), 12))
                using (SolidBrush brush = new SolidBrush(danger ? (down ? ClrDangerDown : ClrDangerHover) : (down ? ClrDown : ClrHover)))
                    g.FillPath(brush, path);
            }

            Image icon;
            if (btn.Enabled ? BtnIcon.TryGetValue(btn, out icon) : BtnIconGray.TryGetValue(btn, out icon))
            {
                int size = icon.Width;
                int x = (btn.Width - size) / 2, y = (btn.Height - size) / 2 + (down ? 1 : 0);
                if (btn.Enabled)
                    g.DrawImage(icon, x, y, size, size);
                else
                {
                    //Grey and faded, so it clearly reads as "off"
                    using (System.Drawing.Imaging.ImageAttributes fade = new System.Drawing.Imaging.ImageAttributes())
                    {
                        fade.SetColorMatrix(new System.Drawing.Imaging.ColorMatrix { Matrix33 = 0.45f });
                        g.DrawImage(icon, new Rectangle(x, y, size, size), 0, 0, size, size, GraphicsUnit.Pixel, fade);
                    }
                }
            }
        }

        private void PanelHeader_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(ClrBorder))
                e.Graphics.DrawLine(pen, 0, panelHeader.Height - 1, panelHeader.Width, panelHeader.Height - 1);
        }

        private void PanelToolbar_Paint(object sender, PaintEventArgs e)
        {
            //Small divider between Search and New | Edit | Delete (Export sits on its own at the right)
            using (Pen pen = new Pen(ClrBorder))
            {
                int x = (btnSearch.Right + btnRegister.Left) / 2;
                e.Graphics.DrawLine(pen, x, 12, x, panelToolbar.Height - 12);
            }
        }

        private void Card_Paint(object sender, PaintEventArgs e)
        {
            //White card with rounded corners and a soft border
            Control card = (Control)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(ClrBg);
            using (GraphicsPath path = RoundRect(new RectangleF(0.5f, 0.5f, card.Width - 1.5f, card.Height - 1.5f), 14))
            using (Pen pen = new Pen(ClrBorder))
            {
                g.FillPath(Brushes.White, path);
                g.DrawPath(pen, path);
            }
        }

        private void Shell_Paint(object sender, PaintEventArgs e)
        {
            //Rounded input box, blue border while typing
            Control shell = (Control)sender;
            bool focused = shell.ContainsFocus;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);
            float w = focused ? 1.6f : 1f;
            using (GraphicsPath path = RoundRect(new RectangleF(w / 2 + 0.5f, w / 2 + 0.5f, shell.Width - w - 1.5f, shell.Height - w - 1.5f), 9))
            using (Pen pen = new Pen(focused ? ClrBlue : ClrBorder, w))
                g.DrawPath(pen, path);
        }

        private void LbMode_Paint(object sender, PaintEventArgs e)
        {
            //Pill at the top of the edit panel: "Register new applicator" or "Editing ..."
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);
            int w = Math.Min(lbMode.Width, TextRenderer.MeasureText(lbMode.Text, lbMode.Font).Width + 38);
            using (GraphicsPath path = RoundRect(new RectangleF(0, 0, w, lbMode.Height), lbMode.Height / 2f))
            using (SolidBrush brush = new SolidBrush(ClrBlueTint))
                g.FillPath(brush, path);
            using (SolidBrush dot = new SolidBrush(ClrBlue))
                g.FillEllipse(dot, 12, lbMode.Height / 2f - 3, 6, 6);
            TextRenderer.DrawText(g, lbMode.Text, lbMode.Font, new Rectangle(24, 0, w - 30, lbMode.Height), ClrBlue,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        }

        private void DgvData_Paint(object sender, PaintEventArgs e)
        {
            if (dgvData.Rows.Count > 0)
                return;
            Rectangle area = new Rectangle(0, dgvData.ColumnHeadersHeight, dgvData.Width, Math.Max(0, dgvData.Height - dgvData.ColumnHeadersHeight));
            if (area.Height < 60)
                return;
            int mid = area.Y + area.Height / 2;
            using (Font title = new Font("Segoe UI Semibold", 10.5f))
            using (Font sub = new Font("Segoe UI", 9f))
            {
                TextRenderer.DrawText(e.Graphics, "No applicators to show", title, new Rectangle(0, mid - 24, area.Width, 22), ClrText,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                TextRenderer.DrawText(e.Graphics, "Click the New button to register one", sub, new Rectangle(0, mid, area.Width, 20), ClrSubText,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private static GraphicsPath RoundRect(RectangleF r, float radius)
        {
            float d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            GraphicsPath path = new GraphicsPath();
            if (d <= 0)
            {
                path.AddRectangle(r);
                return path;
            }
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        #endregion
    }
}
