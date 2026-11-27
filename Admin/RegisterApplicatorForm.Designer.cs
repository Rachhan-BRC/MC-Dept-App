namespace MachineDeptApp.Admin
{
    partial class RegisterApplicatorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterApplicatorForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnExport = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelToolbar = new System.Windows.Forms.Panel();
            this.panelBody = new System.Windows.Forms.Panel();
            this.cardGrid = new System.Windows.Forms.Panel();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.sysno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.applicator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.regdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.regby = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updatedate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updateby = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelGridHead = new System.Windows.Forms.Panel();
            this.LbStatus = new System.Windows.Forms.Label();
            this.lbListTitle = new System.Windows.Forms.Label();
            this.spacerBottom = new System.Windows.Forms.Panel();
            this.cardInput = new System.Windows.Forms.Panel();
            this.panelEditButtons = new System.Windows.Forms.Panel();
            this.lbMode = new System.Windows.Forms.Label();
            this.shellRemark = new System.Windows.Forms.Panel();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.lbRemarkOptional = new System.Windows.Forms.Label();
            this.lbRemarkCap = new System.Windows.Forms.Label();
            this.shellApplicator = new System.Windows.Forms.Panel();
            this.txtApplicator = new System.Windows.Forms.TextBox();
            this.lbApplicatorCap = new System.Windows.Forms.Label();
            this.spacerTop = new System.Windows.Forms.Panel();
            this.cardSearch = new System.Windows.Forms.Panel();
            this.shellSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.panelHeader.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.cardGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.panelGridHead.SuspendLayout();
            this.cardInput.SuspendLayout();
            this.panelEditButtons.SuspendLayout();
            this.shellRemark.SuspendLayout();
            this.shellApplicator.SuspendLayout();
            this.cardSearch.SuspendLayout();
            this.shellSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.Color.White;
            this.btnExport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExport.BackgroundImage")));
            this.btnExport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Location = new System.Drawing.Point(938, 13);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(56, 56);
            this.btnExport.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btnExport, "Export");
            this.btnExport.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.White;
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Location = new System.Drawing.Point(178, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(56, 56);
            this.btnDelete.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnDelete, "Delete");
            this.btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.White;
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Location = new System.Drawing.Point(122, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(56, 56);
            this.btnEdit.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnEdit, "Edit");
            this.btnEdit.UseVisualStyleBackColor = false;
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.White;
            this.btnRegister.BackgroundImage = global::MachineDeptApp.Properties.Resources.DocumentAdd;
            this.btnRegister.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.Location = new System.Drawing.Point(66, 0);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(56, 56);
            this.btnRegister.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnRegister, "New");
            this.btnRegister.UseVisualStyleBackColor = false;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearch.BackgroundImage")));
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Location = new System.Drawing.Point(0, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(56, 56);
            this.btnSearch.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btnSearch, "Search");
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(66, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(56, 56);
            this.btnCancel.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnCancel, "Close");
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(0, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(56, 56);
            this.btnSave.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btnSave, "Save");
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.White;
            this.panelHeader.Controls.Add(this.btnExport);
            this.panelHeader.Controls.Add(this.panelToolbar);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1000, 82);
            this.panelHeader.TabIndex = 1;
            // 
            // panelToolbar
            // 
            this.panelToolbar.BackColor = System.Drawing.Color.White;
            this.panelToolbar.Controls.Add(this.btnDelete);
            this.panelToolbar.Controls.Add(this.btnEdit);
            this.panelToolbar.Controls.Add(this.btnRegister);
            this.panelToolbar.Controls.Add(this.btnSearch);
            this.panelToolbar.Location = new System.Drawing.Point(12, 13);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Size = new System.Drawing.Size(234, 56);
            this.panelToolbar.TabIndex = 2;
            // 
            // panelBody
            // 
            this.panelBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))));
            this.panelBody.Controls.Add(this.cardGrid);
            this.panelBody.Controls.Add(this.spacerBottom);
            this.panelBody.Controls.Add(this.cardInput);
            this.panelBody.Controls.Add(this.spacerTop);
            this.panelBody.Controls.Add(this.cardSearch);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 82);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(22, 18, 22, 22);
            this.panelBody.Size = new System.Drawing.Size(1000, 518);
            this.panelBody.TabIndex = 2;
            // 
            // cardGrid
            // 
            this.cardGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))));
            this.cardGrid.Controls.Add(this.dgvData);
            this.cardGrid.Controls.Add(this.panelGridHead);
            this.cardGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardGrid.Location = new System.Drawing.Point(22, 110);
            this.cardGrid.Name = "cardGrid";
            this.cardGrid.Padding = new System.Windows.Forms.Padding(18, 6, 18, 14);
            this.cardGrid.Size = new System.Drawing.Size(956, 264);
            this.cardGrid.TabIndex = 2;
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            this.dgvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvData.BackgroundColor = System.Drawing.Color.White;
            this.dgvData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvData.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightCyan;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightCyan;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvData.ColumnHeadersHeight = 40;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sysno,
            this.applicator,
            this.remark,
            this.regdate,
            this.regby,
            this.updatedate,
            this.updateby});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvData.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.EnableHeadersVisualStyles = false;
            this.dgvData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(210)))), ((int)(((byte)(224)))));
            this.dgvData.Location = new System.Drawing.Point(18, 56);
            this.dgvData.MultiSelect = false;
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.RowHeadersVisible = false;
            this.dgvData.RowTemplate.Height = 38;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData.Size = new System.Drawing.Size(920, 194);
            this.dgvData.TabIndex = 1;
            // 
            // sysno
            // 
            this.sysno.HeaderText = "SysNo";
            this.sysno.Name = "sysno";
            this.sysno.ReadOnly = true;
            this.sysno.Visible = false;
            // 
            // applicator
            // 
            this.applicator.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.applicator.FillWeight = 18F;
            this.applicator.HeaderText = "Applicator";
            this.applicator.MinimumWidth = 120;
            this.applicator.Name = "applicator";
            this.applicator.ReadOnly = true;
            // 
            // remark
            // 
            this.remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.remark.FillWeight = 34F;
            this.remark.HeaderText = "Remark";
            this.remark.MinimumWidth = 160;
            this.remark.Name = "remark";
            this.remark.ReadOnly = true;
            // 
            // regdate
            // 
            this.regdate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Format = "dd MMM yyyy  HH:mm";
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.regdate.DefaultCellStyle = dataGridViewCellStyle4;
            this.regdate.FillWeight = 15F;
            this.regdate.HeaderText = "Registered";
            this.regdate.MinimumWidth = 130;
            this.regdate.Name = "regdate";
            this.regdate.ReadOnly = true;
            // 
            // regby
            // 
            this.regby.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.regby.DefaultCellStyle = dataGridViewCellStyle5;
            this.regby.FillWeight = 12F;
            this.regby.HeaderText = "Registered by";
            this.regby.MinimumWidth = 100;
            this.regby.Name = "regby";
            this.regby.ReadOnly = true;
            // 
            // updatedate
            // 
            this.updatedate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.updatedate.DefaultCellStyle = dataGridViewCellStyle4;
            this.updatedate.FillWeight = 15F;
            this.updatedate.HeaderText = "Updated";
            this.updatedate.MinimumWidth = 130;
            this.updatedate.Name = "updatedate";
            this.updatedate.ReadOnly = true;
            // 
            // updateby
            // 
            this.updateby.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.updateby.DefaultCellStyle = dataGridViewCellStyle5;
            this.updateby.FillWeight = 12F;
            this.updateby.HeaderText = "Updated by";
            this.updateby.MinimumWidth = 100;
            this.updateby.Name = "updateby";
            this.updateby.ReadOnly = true;
            // 
            // panelGridHead
            // 
            this.panelGridHead.BackColor = System.Drawing.Color.White;
            this.panelGridHead.Controls.Add(this.LbStatus);
            this.panelGridHead.Controls.Add(this.lbListTitle);
            this.panelGridHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelGridHead.Location = new System.Drawing.Point(18, 6);
            this.panelGridHead.Name = "panelGridHead";
            this.panelGridHead.Size = new System.Drawing.Size(920, 50);
            this.panelGridHead.TabIndex = 0;
            // 
            // LbStatus
            // 
            this.LbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LbStatus.BackColor = System.Drawing.Color.White;
            this.LbStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(112)))), ((int)(((byte)(128)))));
            this.LbStatus.Location = new System.Drawing.Point(620, 14);
            this.LbStatus.Name = "LbStatus";
            this.LbStatus.Size = new System.Drawing.Size(300, 24);
            this.LbStatus.TabIndex = 1;
            this.LbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbListTitle
            // 
            this.lbListTitle.AutoSize = true;
            this.lbListTitle.BackColor = System.Drawing.Color.White;
            this.lbListTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbListTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(34)))), ((int)(((byte)(48)))));
            this.lbListTitle.Location = new System.Drawing.Point(2, 14);
            this.lbListTitle.Name = "lbListTitle";
            this.lbListTitle.Size = new System.Drawing.Size(86, 20);
            this.lbListTitle.TabIndex = 0;
            this.lbListTitle.Text = "Applicators";
            // 
            // spacerBottom
            // 
            this.spacerBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))));
            this.spacerBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.spacerBottom.Location = new System.Drawing.Point(22, 374);
            this.spacerBottom.Name = "spacerBottom";
            this.spacerBottom.Size = new System.Drawing.Size(956, 16);
            this.spacerBottom.TabIndex = 3;
            // 
            // cardInput
            // 
            this.cardInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))));
            this.cardInput.Controls.Add(this.panelEditButtons);
            this.cardInput.Controls.Add(this.lbMode);
            this.cardInput.Controls.Add(this.shellRemark);
            this.cardInput.Controls.Add(this.lbRemarkOptional);
            this.cardInput.Controls.Add(this.lbRemarkCap);
            this.cardInput.Controls.Add(this.shellApplicator);
            this.cardInput.Controls.Add(this.lbApplicatorCap);
            this.cardInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cardInput.Location = new System.Drawing.Point(22, 390);
            this.cardInput.Name = "cardInput";
            this.cardInput.Size = new System.Drawing.Size(956, 110);
            this.cardInput.TabIndex = 4;
            // 
            // panelEditButtons
            // 
            this.panelEditButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEditButtons.BackColor = System.Drawing.Color.White;
            this.panelEditButtons.Controls.Add(this.btnCancel);
            this.panelEditButtons.Controls.Add(this.btnSave);
            this.panelEditButtons.Location = new System.Drawing.Point(814, 41);
            this.panelEditButtons.Name = "panelEditButtons";
            this.panelEditButtons.Size = new System.Drawing.Size(122, 56);
            this.panelEditButtons.TabIndex = 2;
            // 
            // lbMode
            // 
            this.lbMode.BackColor = System.Drawing.Color.White;
            this.lbMode.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(112)))), ((int)(((byte)(128)))));
            this.lbMode.Location = new System.Drawing.Point(20, 12);
            this.lbMode.Name = "lbMode";
            this.lbMode.Size = new System.Drawing.Size(300, 26);
            this.lbMode.TabIndex = 6;
            this.lbMode.Text = "Register new applicator";
            this.lbMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // shellRemark
            // 
            this.shellRemark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.shellRemark.BackColor = System.Drawing.Color.White;
            this.shellRemark.Controls.Add(this.txtRemark);
            this.shellRemark.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.shellRemark.Location = new System.Drawing.Point(520, 48);
            this.shellRemark.Name = "shellRemark";
            this.shellRemark.Size = new System.Drawing.Size(282, 42);
            this.shellRemark.TabIndex = 1;
            // 
            // txtRemark
            // 
            this.txtRemark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRemark.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(34)))), ((int)(((byte)(48)))));
            this.txtRemark.Location = new System.Drawing.Point(13, 11);
            this.txtRemark.MaxLength = 500;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(256, 19);
            this.txtRemark.TabIndex = 0;
            // 
            // lbRemarkOptional
            // 
            this.lbRemarkOptional.AutoSize = true;
            this.lbRemarkOptional.BackColor = System.Drawing.Color.White;
            this.lbRemarkOptional.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRemarkOptional.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(161)))), ((int)(((byte)(176)))));
            this.lbRemarkOptional.Location = new System.Drawing.Point(456, 60);
            this.lbRemarkOptional.Name = "lbRemarkOptional";
            this.lbRemarkOptional.Size = new System.Drawing.Size(51, 13);
            this.lbRemarkOptional.TabIndex = 5;
            this.lbRemarkOptional.Text = "optional";
            // 
            // lbRemarkCap
            // 
            this.lbRemarkCap.AutoSize = true;
            this.lbRemarkCap.BackColor = System.Drawing.Color.White;
            this.lbRemarkCap.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRemarkCap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(63)))), ((int)(((byte)(78)))));
            this.lbRemarkCap.Location = new System.Drawing.Point(402, 59);
            this.lbRemarkCap.Name = "lbRemarkCap";
            this.lbRemarkCap.Size = new System.Drawing.Size(47, 15);
            this.lbRemarkCap.TabIndex = 4;
            this.lbRemarkCap.Text = "Remark";
            // 
            // shellApplicator
            // 
            this.shellApplicator.BackColor = System.Drawing.Color.White;
            this.shellApplicator.Controls.Add(this.txtApplicator);
            this.shellApplicator.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.shellApplicator.Location = new System.Drawing.Point(98, 48);
            this.shellApplicator.Name = "shellApplicator";
            this.shellApplicator.Size = new System.Drawing.Size(280, 42);
            this.shellApplicator.TabIndex = 0;
            // 
            // txtApplicator
            // 
            this.txtApplicator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtApplicator.BackColor = System.Drawing.Color.White;
            this.txtApplicator.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtApplicator.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApplicator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(34)))), ((int)(((byte)(48)))));
            this.txtApplicator.Location = new System.Drawing.Point(13, 11);
            this.txtApplicator.MaxLength = 100;
            this.txtApplicator.Name = "txtApplicator";
            this.txtApplicator.Size = new System.Drawing.Size(254, 19);
            this.txtApplicator.TabIndex = 0;
            // 
            // lbApplicatorCap
            // 
            this.lbApplicatorCap.AutoSize = true;
            this.lbApplicatorCap.BackColor = System.Drawing.Color.White;
            this.lbApplicatorCap.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbApplicatorCap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(63)))), ((int)(((byte)(78)))));
            this.lbApplicatorCap.Location = new System.Drawing.Point(20, 59);
            this.lbApplicatorCap.Name = "lbApplicatorCap";
            this.lbApplicatorCap.Size = new System.Drawing.Size(62, 15);
            this.lbApplicatorCap.TabIndex = 3;
            this.lbApplicatorCap.Text = "Applicator";
            // 
            // spacerTop
            // 
            this.spacerTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))));
            this.spacerTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.spacerTop.Location = new System.Drawing.Point(22, 94);
            this.spacerTop.Name = "spacerTop";
            this.spacerTop.Size = new System.Drawing.Size(956, 16);
            this.spacerTop.TabIndex = 1;
            // 
            // cardSearch
            // 
            this.cardSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))));
            this.cardSearch.Controls.Add(this.shellSearch);
            this.cardSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardSearch.Location = new System.Drawing.Point(22, 18);
            this.cardSearch.Name = "cardSearch";
            this.cardSearch.Size = new System.Drawing.Size(956, 76);
            this.cardSearch.TabIndex = 0;
            // 
            // shellSearch
            // 
            this.shellSearch.BackColor = System.Drawing.Color.White;
            this.shellSearch.Controls.Add(this.txtSearch);
            this.shellSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.shellSearch.Location = new System.Drawing.Point(20, 17);
            this.shellSearch.Name = "shellSearch";
            this.shellSearch.Size = new System.Drawing.Size(400, 42);
            this.shellSearch.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BackColor = System.Drawing.Color.White;
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(34)))), ((int)(((byte)(48)))));
            this.txtSearch.Location = new System.Drawing.Point(13, 11);
            this.txtSearch.MaxLength = 100;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(374, 19);
            this.txtSearch.TabIndex = 0;
            // 
            // RegisterApplicatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))));
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelHeader);
            this.Name = "RegisterApplicatorForm";
            this.Text = "Register Applicator";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelHeader.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.cardGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.panelGridHead.ResumeLayout(false);
            this.panelGridHead.PerformLayout();
            this.cardInput.ResumeLayout(false);
            this.cardInput.PerformLayout();
            this.panelEditButtons.ResumeLayout(false);
            this.shellRemark.ResumeLayout(false);
            this.shellRemark.PerformLayout();
            this.shellApplicator.ResumeLayout(false);
            this.shellApplicator.PerformLayout();
            this.cardSearch.ResumeLayout(false);
            this.shellSearch.ResumeLayout(false);
            this.shellSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelToolbar;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.Panel cardSearch;
        private System.Windows.Forms.Panel shellSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Panel spacerTop;
        private System.Windows.Forms.Panel cardGrid;
        private System.Windows.Forms.Panel panelGridHead;
        private System.Windows.Forms.Label lbListTitle;
        private System.Windows.Forms.Label LbStatus;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.DataGridViewTextBoxColumn sysno;
        private System.Windows.Forms.DataGridViewTextBoxColumn applicator;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn regdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn regby;
        private System.Windows.Forms.DataGridViewTextBoxColumn updatedate;
        private System.Windows.Forms.DataGridViewTextBoxColumn updateby;
        private System.Windows.Forms.Panel spacerBottom;
        private System.Windows.Forms.Panel cardInput;
        private System.Windows.Forms.Label lbMode;
        private System.Windows.Forms.Label lbApplicatorCap;
        private System.Windows.Forms.Panel shellApplicator;
        private System.Windows.Forms.TextBox txtApplicator;
        private System.Windows.Forms.Label lbRemarkCap;
        private System.Windows.Forms.Label lbRemarkOptional;
        private System.Windows.Forms.Panel shellRemark;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Panel panelEditButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
