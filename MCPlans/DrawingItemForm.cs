using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MachineDeptApp.MCPlans
{
    public partial class DrawingItemForm : Form
    {
        SQLConnect cnn = new SQLConnect();

        public DrawingItemForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Shown += DrawingItemForm_Shown;
            this.btnSearch.Click += BtnSearch_Click;
            this.txtSearch.KeyDown += TxtSearch_KeyDown;
            this.txtSearchPart.KeyDown += TxtSearch_KeyDown;
            this.btnNew.Click += BtnNew_Click;
            this.btnUpdate.Click += BtnUpdate_Click;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnView.Click += BtnView_Click;
            this.dgvItem.SelectionChanged += DgvItem_SelectionChanged;
            this.dgvItem.CellDoubleClick += DgvItem_CellDoubleClick;
            ApplyHeaderStyle();
        }

        private void DrawingItemForm_Shown(object sender, EventArgs e)
        {
            LoadData();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                LoadData();
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void DgvItem_SelectionChanged(object sender, EventArgs e)
        {
            CheckButtons();
        }

        private void DgvItem_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                OpenDetail(DrawingItemDetailForm.FormMode.View);
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            DrawingItemDetailForm frm = new DrawingItemDetailForm(DrawingItemDetailForm.FormMode.Create, 0);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            OpenDetail(DrawingItemDetailForm.FormMode.Update);
        }

        private void BtnView_Click(object sender, EventArgs e)
        {
            OpenDetail(DrawingItemDetailForm.FormMode.View);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int id = GetSelectedID();
            if (id <= 0) return;

            string itemName = dgvItem.CurrentRow.Cells[ColItemName.Name].Value?.ToString();
            DialogResult dlr = MessageBox.Show("Are you sure you want to delete \"" + itemName + "\"?", MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr != DialogResult.Yes) return;

            try
            {
                cnn.con.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE tbDrawingItem SET Status = 'Deleted', UpdateDate = @UpD, UpdateBy = @UpB WHERE DrawingID = @ID", cnn.con))
                {
                    cmd.Parameters.AddWithValue("@UpD", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpB", MenuFormV2.UserForNextForm ?? "");
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
                cnn.con.Close();
                MessageBox.Show("Deleted successfully!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete failed!\n" + ex.Message, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (cnn.con.State == ConnectionState.Open) cnn.con.Close();
            }
        }

        private void OpenDetail(DrawingItemDetailForm.FormMode mode)
        {
            int id = GetSelectedID();
            if (id <= 0) return;

            DrawingItemDetailForm frm = new DrawingItemDetailForm(mode, id);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                LoadData();
            }
        }

        private int GetSelectedID()
        {
            if (dgvItem.CurrentRow == null || dgvItem.SelectedRows.Count == 0) return 0;
            object val = dgvItem.CurrentRow.Cells[ColID.Name].Value;
            int id;
            return (val != null && int.TryParse(val.ToString(), out id)) ? id : 0;
        }

        private void LoadData()
        {
            Cursor = Cursors.WaitCursor;
            LbStatus.Text = "Searching . . .";
            LbStatus.Refresh();
            dgvItem.Rows.Clear();
            try
            {
                cnn.con.Open();
                string query = "SELECT DrawingID, ItemName, ItemPart, CreateDate, PrepareBy, PEChecker, QAChecker, Approval, UpdateDate, UpdateBy " +
                               "FROM tbDrawingItem WHERE ISNULL(Status, '') <> 'Deleted' ";
                if (txtSearch.Text.Trim() != "")
                {
                    query += "AND ItemName LIKE @SearchName ";
                }
                if (txtSearchPart.Text.Trim() != "")
                {
                    query += "AND ItemPart LIKE @SearchPart ";
                }
                query += "ORDER BY CreateDate DESC";

                DataTable dt = new DataTable();
                using (SqlDataAdapter sda = new SqlDataAdapter(query, cnn.con))
                {
                    sda.SelectCommand.Parameters.AddWithValue("@SearchName", "%" + txtSearch.Text.Trim() + "%");
                    sda.SelectCommand.Parameters.AddWithValue("@SearchPart", "%" + txtSearchPart.Text.Trim() + "%");
                    sda.Fill(dt);
                }

                int no = 1;
                foreach (DataRow row in dt.Rows)
                {
                    dgvItem.Rows.Add(row["DrawingID"], no, row["ItemName"], row["ItemPart"],
                        row["CreateDate"] == DBNull.Value ? (object)null : Convert.ToDateTime(row["CreateDate"]),
                        row["PrepareBy"], row["PEChecker"], row["QAChecker"], row["Approval"],
                        row["UpdateDate"] == DBNull.Value ? (object)null : Convert.ToDateTime(row["UpdateDate"]),
                        row["UpdateBy"]);
                    no++;
                }
                dgvItem.ClearSelection();
                LbStatus.Text = "Found " + dgvItem.Rows.Count.ToString("N0") + " item(s).";
            }
            catch (Exception ex)
            {
                LbStatus.Text = "";
                MessageBox.Show("Something went wrong! Please check the network connection.\n" + ex.Message, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (cnn.con.State == ConnectionState.Open) cnn.con.Close();
                Cursor = Cursors.Default;
            }
            CheckButtons();
        }

        private void CheckButtons()
        {
            bool hasSel = dgvItem.SelectedRows.Count > 0;
            foreach (Button btn in new[] { btnUpdate, btnDelete, btnView })
            {
                btn.Enabled = hasSel;
            }
        }

        #region Header styling (gradient, rounded search box, icon buttons)

        private class ButtonStyle
        {
            public Color Back, Border, Fore;
            public string Glyph;
            public ButtonStyle(Color back, Color border, Color fore, string glyph)
            {
                Back = back; Border = border; Fore = fore; Glyph = glyph;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        static readonly Color AccentBlue = Color.FromArgb(24, 103, 214);
        static readonly Color LightBlue = Color.FromArgb(219, 234, 254);
        static readonly Color LightBlueBorder = Color.FromArgb(125, 168, 236);
        static readonly Color LightBlueText = Color.FromArgb(22, 82, 180);

        Dictionary<Button, ButtonStyle> buttonStyles;
        Font iconFont;
        Button hoverButton;
        Button pressedButton;

        private void ApplyHeaderStyle()
        {
            iconFont = new Font("Segoe MDL2 Assets", 11F);
            buttonStyles = new Dictionary<Button, ButtonStyle>
            {
                { btnSearch, new ButtonStyle(AccentBlue, AccentBlue, Color.White, "") },
                { btnNew, new ButtonStyle(LightBlue, LightBlueBorder, LightBlueText, "") },
                { btnUpdate, new ButtonStyle(LightBlue, LightBlueBorder, LightBlueText, "") },
                { btnDelete, new ButtonStyle(Color.FromArgb(233, 72, 86), Color.FromArgb(233, 72, 86), Color.White, "") },
                { btnView, new ButtonStyle(Color.FromArgb(213, 219, 228), Color.FromArgb(188, 196, 208), Color.FromArgb(33, 41, 54), "") }
            };

            foreach (Button btn in buttonStyles.Keys)
            {
                btn.Paint += HeaderButton_Paint;
                btn.MouseEnter += (s, e) => { hoverButton = (Button)s; ((Button)s).Invalidate(); };
                btn.MouseLeave += (s, e) => { hoverButton = null; pressedButton = null; ((Button)s).Invalidate(); };
                btn.MouseDown += (s, e) => { pressedButton = (Button)s; ((Button)s).Invalidate(); };
                btn.MouseUp += (s, e) => { pressedButton = null; ((Button)s).Invalidate(); };
                btn.EnabledChanged += (s, e) => ((Button)s).Invalidate();
            }

            panelHeader.Paint += PanelHeader_Paint;
            panelHeader.Resize += (s, e) => panelHeader.Invalidate(true);
            SetupSearchBox(panelSearch, txtSearch, "Enter wip name...");
            SetupSearchBox(panelSearchPart, txtSearchPart, "Enter wip code...");

            this.FormClosed += (s, e) => iconFont.Dispose();
        }

        private void SetupSearchBox(Panel panel, TextBox txt, string placeholder)
        {
            panel.Tag = txt;
            panel.Paint += PanelSearch_Paint;
            panel.Click += (s, e) => txt.Focus();
            panel.Resize += (s, e) => LayoutSearchBox(panel, txt);
            txt.GotFocus += (s, e) => panel.Invalidate();
            txt.LostFocus += (s, e) => panel.Invalidate();
            txt.HandleCreated += (s, e) => SendMessage(txt.Handle, EM_SETCUEBANNER, (IntPtr)1, placeholder);
            LayoutSearchBox(panel, txt);
        }

        private void LayoutSearchBox(Panel panel, TextBox txt)
        {
            txt.Left = 12;
            txt.Width = panel.Width - 12 - 36;
            txt.Top = (panel.Height - txt.Height) / 2;
        }

        private LinearGradientBrush CreateHeaderBrush()
        {
            Rectangle r = panelHeader.ClientRectangle;
            if (r.Width <= 0) r.Width = 1;
            if (r.Height <= 0) r.Height = 1;
            LinearGradientBrush br = new LinearGradientBrush(r, Color.White, Color.FromArgb(214, 229, 252), LinearGradientMode.Horizontal);
            Blend blend = new Blend();
            blend.Positions = new float[] { 0f, 0.55f, 1f };
            blend.Factors = new float[] { 0f, 0.15f, 1f };
            br.Blend = blend;
            return br;
        }

        // Paints the header gradient behind a child control so its rounded corners blend in smoothly
        private void PaintHeaderBehind(Graphics g, Control c)
        {
            using (LinearGradientBrush br = CreateHeaderBrush())
            {
                br.TranslateTransform(-c.Left, -c.Top, MatrixOrder.Append);
                g.FillRectangle(br, 0, 0, c.Width, c.Height);
            }
        }

        private void PanelHeader_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush br = CreateHeaderBrush())
            {
                e.Graphics.FillRectangle(br, panelHeader.ClientRectangle);
            }
            using (Pen line = new Pen(Color.FromArgb(220, 228, 240)))
            {
                e.Graphics.DrawLine(line, 0, panelHeader.Height - 1, panelHeader.Width, panelHeader.Height - 1);
            }
        }

        private void PanelSearch_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            TextBox txt = (TextBox)panel.Tag;
            Graphics g = e.Graphics;
            PaintHeaderBehind(g, panel);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            bool focused = txt.Focused;
            using (GraphicsPath path = RoundedRect(new RectangleF(0.5f, 0.5f, panel.Width - 1.5f, panel.Height - 1.5f), 7))
            using (SolidBrush fill = new SolidBrush(Color.White))
            using (Pen pen = new Pen(focused ? AccentBlue : Color.FromArgb(203, 213, 225), focused ? 1.5f : 1f))
            {
                g.FillPath(fill, path);
                g.DrawPath(pen, path);
            }
            Rectangle iconRect = new Rectangle(panel.Width - 34, 0, 26, panel.Height);
            TextRenderer.DrawText(g, "", iconFont, iconRect, Color.FromArgb(51, 65, 85),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }

        private void HeaderButton_Paint(object sender, PaintEventArgs e)
        {
            Button btn = (Button)sender;
            ButtonStyle st = buttonStyles[btn];
            Graphics g = e.Graphics;

            PaintHeaderBehind(g, btn);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color back = st.Back, border = st.Border, fore = st.Fore;
            if (!btn.Enabled)
            {
                back = Mix(back, Color.White, 0.5f);
                border = Mix(border, Color.White, 0.5f);
                fore = Mix(fore, Color.White, 0.45f);
            }
            else if (btn == pressedButton)
            {
                back = Mix(back, Color.Black, 0.15f);
            }
            else if (btn == hoverButton)
            {
                back = Mix(back, Color.Black, 0.07f);
            }

            using (GraphicsPath path = RoundedRect(new RectangleF(0.5f, 0.5f, btn.Width - 1.5f, btn.Height - 1.5f), 7))
            using (SolidBrush fill = new SolidBrush(back))
            using (Pen pen = new Pen(border))
            {
                g.FillPath(fill, path);
                g.DrawPath(pen, path);
            }

            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine;
            Size iconSize = TextRenderer.MeasureText(g, st.Glyph, iconFont, Size.Empty, flags);
            Size textSize = TextRenderer.MeasureText(g, btn.Text, btn.Font, Size.Empty, flags);
            const int gap = 8;
            int x = (btn.Width - (iconSize.Width + gap + textSize.Width)) / 2;
            TextRenderer.DrawText(g, st.Glyph, iconFont, new Rectangle(x, 0, iconSize.Width, btn.Height), fore, flags);
            TextRenderer.DrawText(g, btn.Text, btn.Font, new Rectangle(x + iconSize.Width + gap, 0, textSize.Width + 2, btn.Height), fore, flags);
        }

        private static GraphicsPath RoundedRect(RectangleF r, float radius)
        {
            float d = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static Color Mix(Color a, Color b, float amount)
        {
            return Color.FromArgb(
                (int)(a.R + (b.R - a.R) * amount),
                (int)(a.G + (b.G - a.G) * amount),
                (int)(a.B + (b.B - a.B) * amount));
        }

        #endregion
    }
}
