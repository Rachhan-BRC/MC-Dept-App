namespace MachineDeptApp.MCPlans
{
    partial class PlanMCCompleteQtyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlanMCCompleteQtyForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.LbStatus = new System.Windows.Forms.Label();
            this.panelBody = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DgvSearch = new System.Windows.Forms.DataGridView();
            this.colPO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRegBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRegDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpdateBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpdateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grbEdit = new System.Windows.Forms.GroupBox();
            this.LbEditNote = new System.Windows.Forms.Label();
            this.txtEditQty = new System.Windows.Forms.TextBox();
            this.txtEditItemCode = new System.Windows.Forms.TextBox();
            this.txtEditPO = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.grbSCondition = new System.Windows.Forms.GroupBox();
            this.txtItemCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPO = new System.Windows.Forms.TextBox();
            this.lbPO = new System.Windows.Forms.Label();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.dtpEDate = new System.Windows.Forms.DateTimePicker();
            this.dtpSDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvSearch)).BeginInit();
            this.grbEdit.SuspendLayout();
            this.grbSCondition.SuspendLayout();
            this.SuspendLayout();
            //
            // btnSearch
            //
            this.btnSearch.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearch.BackgroundImage")));
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.Location = new System.Drawing.Point(4, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(48, 51);
            this.btnSearch.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btnSearch, "ស្វែងរក");
            this.btnSearch.UseVisualStyleBackColor = false;
            //
            // btnUpdate
            //
            this.btnUpdate.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUpdate.BackgroundImage")));
            this.btnUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Location = new System.Drawing.Point(56, 4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(48, 51);
            this.btnUpdate.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnUpdate, "កែប្រែ (Update)");
            this.btnUpdate.UseVisualStyleBackColor = false;
            //
            // btnDelete
            //
            this.btnDelete.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(108, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(48, 51);
            this.btnDelete.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnDelete, "លុប (Delete)");
            this.btnDelete.UseVisualStyleBackColor = false;
            //
            // btnExport
            //
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnExport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExport.BackgroundImage")));
            this.btnExport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(933, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(48, 51);
            this.btnExport.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnExport, "Export");
            this.btnExport.UseVisualStyleBackColor = false;
            //
            // panelHeader
            //
            this.panelHeader.Controls.Add(this.btnSearch);
            this.panelHeader.Controls.Add(this.btnUpdate);
            this.panelHeader.Controls.Add(this.btnDelete);
            this.panelHeader.Controls.Add(this.btnExport);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(984, 59);
            this.panelHeader.TabIndex = 7;
            //
            // panelFooter
            //
            this.panelFooter.BackColor = System.Drawing.SystemColors.Control;
            this.panelFooter.Controls.Add(this.LbStatus);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 477);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(984, 34);
            this.panelFooter.TabIndex = 8;
            //
            // LbStatus
            //
            this.LbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LbStatus.Font = new System.Drawing.Font("Khmer OS Battambang", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbStatus.Location = new System.Drawing.Point(404, 5);
            this.LbStatus.Name = "LbStatus";
            this.LbStatus.Size = new System.Drawing.Size(575, 26);
            this.LbStatus.TabIndex = 7;
            this.LbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // panelBody
            //
            this.panelBody.Controls.Add(this.groupBox2);
            this.panelBody.Controls.Add(this.grbEdit);
            this.panelBody.Controls.Add(this.grbSCondition);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 59);
            this.panelBody.Margin = new System.Windows.Forms.Padding(2);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(3, 0, 3, 2);
            this.panelBody.Size = new System.Drawing.Size(984, 418);
            this.panelBody.TabIndex = 9;
            //
            // groupBox2
            //
            this.groupBox2.Controls.Add(this.DgvSearch);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Kh Battambang", 7.8F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(3, 71);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(6, 0, 6, 7);
            this.groupBox2.Size = new System.Drawing.Size(728, 345);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "តារាងទិន្នន័យ";
            //
            // DgvSearch
            //
            this.DgvSearch.AllowUserToAddRows = false;
            this.DgvSearch.AllowUserToDeleteRows = false;
            this.DgvSearch.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Kh Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            this.DgvSearch.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Kh Battambang", 7.8F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvSearch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.DgvSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvSearch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPO,
            this.colItemCode,
            this.colItemName,
            this.colQty,
            this.colRegBy,
            this.colRegDate,
            this.colUpdateBy,
            this.colUpdateDate});
            this.DgvSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvSearch.EnableHeadersVisualStyles = false;
            this.DgvSearch.Location = new System.Drawing.Point(6, 20);
            this.DgvSearch.Name = "DgvSearch";
            this.DgvSearch.ReadOnly = true;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Kh Battambang", 7.8F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvSearch.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.DgvSearch.RowHeadersWidth = 60;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.Blue;
            this.DgvSearch.RowsDefaultCellStyle = dataGridViewCellStyle11;
            this.DgvSearch.RowTemplate.Height = 25;
            this.DgvSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvSearch.Size = new System.Drawing.Size(716, 318);
            this.DgvSearch.TabIndex = 0;
            //
            // colPO
            //
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colPO.DefaultCellStyle = dataGridViewCellStyle3;
            this.colPO.Frozen = true;
            this.colPO.HeaderText = "PO";
            this.colPO.Name = "colPO";
            this.colPO.ReadOnly = true;
            this.colPO.Width = 110;
            //
            // colItemCode
            //
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colItemCode.DefaultCellStyle = dataGridViewCellStyle4;
            this.colItemCode.Frozen = true;
            this.colItemCode.HeaderText = "Item Code";
            this.colItemCode.Name = "colItemCode";
            this.colItemCode.ReadOnly = true;
            this.colItemCode.Width = 90;
            //
            // colItemName
            //
            this.colItemName.HeaderText = "Item Name";
            this.colItemName.Name = "colItemName";
            this.colItemName.ReadOnly = true;
            this.colItemName.Width = 200;
            //
            // colQty
            //
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "#,##0.###";
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.colQty.DefaultCellStyle = dataGridViewCellStyle5;
            this.colQty.HeaderText = "Qty";
            this.colQty.Name = "colQty";
            this.colQty.ReadOnly = true;
            this.colQty.Width = 90;
            //
            // colRegBy
            //
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colRegBy.DefaultCellStyle = dataGridViewCellStyle7;
            this.colRegBy.HeaderText = "បញ្ចូលដោយ";
            this.colRegBy.Name = "colRegBy";
            this.colRegBy.ReadOnly = true;
            //
            // colRegDate
            //
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Format = "dd-MM-yyyy HH:mm:ss";
            this.colRegDate.DefaultCellStyle = dataGridViewCellStyle8;
            this.colRegDate.HeaderText = "ថ្ងៃបញ្ចូល";
            this.colRegDate.Name = "colRegDate";
            this.colRegDate.ReadOnly = true;
            this.colRegDate.Width = 150;
            //
            // colUpdateBy
            //
            this.colUpdateBy.DefaultCellStyle = dataGridViewCellStyle7;
            this.colUpdateBy.HeaderText = "កែប្រែដោយ";
            this.colUpdateBy.Name = "colUpdateBy";
            this.colUpdateBy.ReadOnly = true;
            //
            // colUpdateDate
            //
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.Format = "dd-MM-yyyy HH:mm:ss";
            this.colUpdateDate.DefaultCellStyle = dataGridViewCellStyle9;
            this.colUpdateDate.HeaderText = "ថ្ងៃកែប្រែ";
            this.colUpdateDate.Name = "colUpdateDate";
            this.colUpdateDate.ReadOnly = true;
            this.colUpdateDate.Width = 150;
            //
            // grbEdit
            //
            this.grbEdit.Controls.Add(this.LbEditNote);
            this.grbEdit.Controls.Add(this.txtEditQty);
            this.grbEdit.Controls.Add(this.txtEditItemCode);
            this.grbEdit.Controls.Add(this.txtEditPO);
            this.grbEdit.Controls.Add(this.label6);
            this.grbEdit.Controls.Add(this.label5);
            this.grbEdit.Controls.Add(this.label4);
            this.grbEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.grbEdit.Enabled = false;
            this.grbEdit.Font = new System.Drawing.Font("Kh Battambang", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbEdit.Location = new System.Drawing.Point(731, 71);
            this.grbEdit.Margin = new System.Windows.Forms.Padding(2);
            this.grbEdit.Name = "grbEdit";
            this.grbEdit.Padding = new System.Windows.Forms.Padding(2);
            this.grbEdit.Size = new System.Drawing.Size(250, 345);
            this.grbEdit.TabIndex = 1;
            this.grbEdit.TabStop = false;
            this.grbEdit.Text = "កែប្រែទិន្នន័យ";
            //
            // LbEditNote
            //
            this.LbEditNote.Font = new System.Drawing.Font("Kh Battambang", 8.25F, System.Drawing.FontStyle.Italic);
            this.LbEditNote.ForeColor = System.Drawing.Color.DimGray;
            this.LbEditNote.Location = new System.Drawing.Point(10, 136);
            this.LbEditNote.Name = "LbEditNote";
            this.LbEditNote.Size = new System.Drawing.Size(230, 70);
            this.LbEditNote.TabIndex = 8;
            this.LbEditNote.Text = "ជ្រើសរើសទិន្នន័យ 1 ជួរ កែ Qty រួចចុចប៊ូតុង Update។";
            //
            // txtEditQty
            //
            this.txtEditQty.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.txtEditQty.Location = new System.Drawing.Point(95, 98);
            this.txtEditQty.MaxLength = 20;
            this.txtEditQty.Name = "txtEditQty";
            this.txtEditQty.Size = new System.Drawing.Size(145, 32);
            this.txtEditQty.TabIndex = 6;
            //
            // txtEditItemCode
            //
            this.txtEditItemCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtEditItemCode.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.txtEditItemCode.Location = new System.Drawing.Point(95, 62);
            this.txtEditItemCode.Name = "txtEditItemCode";
            this.txtEditItemCode.ReadOnly = true;
            this.txtEditItemCode.Size = new System.Drawing.Size(145, 32);
            this.txtEditItemCode.TabIndex = 5;
            this.txtEditItemCode.TabStop = false;
            //
            // txtEditPO
            //
            this.txtEditPO.BackColor = System.Drawing.SystemColors.Control;
            this.txtEditPO.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.txtEditPO.Location = new System.Drawing.Point(95, 26);
            this.txtEditPO.Name = "txtEditPO";
            this.txtEditPO.ReadOnly = true;
            this.txtEditPO.Size = new System.Drawing.Size(145, 32);
            this.txtEditPO.TabIndex = 4;
            this.txtEditPO.TabStop = false;
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.label6.Location = new System.Drawing.Point(8, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 24);
            this.label6.TabIndex = 2;
            this.label6.Text = "Qty";
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.label5.Location = new System.Drawing.Point(8, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 24);
            this.label5.TabIndex = 1;
            this.label5.Text = "Item Code";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.label4.Location = new System.Drawing.Point(8, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 24);
            this.label4.TabIndex = 0;
            this.label4.Text = "PO";
            //
            // grbSCondition
            //
            this.grbSCondition.Controls.Add(this.txtItemCode);
            this.grbSCondition.Controls.Add(this.label2);
            this.grbSCondition.Controls.Add(this.txtPO);
            this.grbSCondition.Controls.Add(this.lbPO);
            this.grbSCondition.Controls.Add(this.chkDate);
            this.grbSCondition.Controls.Add(this.dtpEDate);
            this.grbSCondition.Controls.Add(this.dtpSDate);
            this.grbSCondition.Controls.Add(this.label1);
            this.grbSCondition.Dock = System.Windows.Forms.DockStyle.Top;
            this.grbSCondition.Font = new System.Drawing.Font("Kh Battambang", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbSCondition.Location = new System.Drawing.Point(3, 0);
            this.grbSCondition.Margin = new System.Windows.Forms.Padding(2);
            this.grbSCondition.Name = "grbSCondition";
            this.grbSCondition.Padding = new System.Windows.Forms.Padding(2);
            this.grbSCondition.Size = new System.Drawing.Size(978, 71);
            this.grbSCondition.TabIndex = 0;
            this.grbSCondition.TabStop = false;
            this.grbSCondition.Text = "លក្ខខណ្ឌស្វែងរក";
            //
            // txtItemCode
            //
            this.txtItemCode.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.txtItemCode.Location = new System.Drawing.Point(618, 26);
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(115, 32);
            this.txtItemCode.TabIndex = 7;
            this.toolTip1.SetToolTip(this.txtItemCode, "អាចប្រើ * ជំនួសតួអក្សរណាមួយ");
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.label2.Location = new System.Drawing.Point(543, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "Item Code";
            //
            // txtPO
            //
            this.txtPO.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.txtPO.Location = new System.Drawing.Point(395, 26);
            this.txtPO.Name = "txtPO";
            this.txtPO.Size = new System.Drawing.Size(135, 32);
            this.txtPO.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txtPO, "អាចប្រើ * ជំនួសតួអក្សរណាមួយ");
            //
            // lbPO
            //
            this.lbPO.AutoSize = true;
            this.lbPO.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.lbPO.Location = new System.Drawing.Point(363, 30);
            this.lbPO.Name = "lbPO";
            this.lbPO.Size = new System.Drawing.Size(27, 24);
            this.lbPO.TabIndex = 4;
            this.lbPO.Text = "PO";
            //
            // chkDate
            //
            this.chkDate.AutoSize = true;
            this.chkDate.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.chkDate.Location = new System.Drawing.Point(7, 28);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(84, 28);
            this.chkDate.TabIndex = 0;
            this.chkDate.Text = "ថ្ងៃបញ្ចូល";
            this.chkDate.UseVisualStyleBackColor = true;
            //
            // dtpEDate
            //
            this.dtpEDate.CustomFormat = "dd-MM-yyyy";
            this.dtpEDate.Font = new System.Drawing.Font("Kh Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpEDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEDate.Location = new System.Drawing.Point(236, 26);
            this.dtpEDate.Name = "dtpEDate";
            this.dtpEDate.Size = new System.Drawing.Size(110, 32);
            this.dtpEDate.TabIndex = 3;
            //
            // dtpSDate
            //
            this.dtpSDate.CustomFormat = "dd-MM-yyyy";
            this.dtpSDate.Font = new System.Drawing.Font("Kh Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpSDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSDate.Location = new System.Drawing.Point(97, 26);
            this.dtpSDate.Name = "dtpSDate";
            this.dtpSDate.Size = new System.Drawing.Size(110, 32);
            this.dtpSDate.TabIndex = 1;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Kh Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(208, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = " ~ ";
            //
            // PlanMCCompleteQtyForm
            //
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(984, 511);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PlanMCCompleteQtyForm";
            this.Text = "MC Complete Qty";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelHeader.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvSearch)).EndInit();
            this.grbEdit.ResumeLayout(false);
            this.grbEdit.PerformLayout();
            this.grbSCondition.ResumeLayout(false);
            this.grbSCondition.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Label LbStatus;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView DgvSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRegBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRegDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUpdateBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUpdateDate;
        private System.Windows.Forms.GroupBox grbEdit;
        private System.Windows.Forms.Label LbEditNote;
        private System.Windows.Forms.TextBox txtEditQty;
        private System.Windows.Forms.TextBox txtEditItemCode;
        private System.Windows.Forms.TextBox txtEditPO;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox grbSCondition;
        private System.Windows.Forms.TextBox txtItemCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPO;
        private System.Windows.Forms.Label lbPO;
        private System.Windows.Forms.CheckBox chkDate;
        private System.Windows.Forms.DateTimePicker dtpEDate;
        private System.Windows.Forms.DateTimePicker dtpSDate;
        private System.Windows.Forms.Label label1;
    }
}
