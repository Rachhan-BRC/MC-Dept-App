namespace MachineDeptApp.Inventory.MC_SD
{
    partial class MCSDCountingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCSDCountingForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.numPrintQty = new System.Windows.Forms.NumericUpDown();
            this.CboCountType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.chkPrintStatus = new System.Windows.Forms.CheckBox();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.LbStatus = new System.Windows.Forms.Label();
            this.panelBody = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.CboBobbinW = new System.Windows.Forms.ComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.LbTotalQty = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.LbCode = new System.Windows.Forms.Label();
            this.LbQtyTitle = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.LbType = new System.Windows.Forms.Label();
            this.LbItems = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.LbMaker = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LbBobbinsWTitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LbNetWTitle = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.LbNetW = new System.Windows.Forms.Label();
            this.LbBobbinQty = new System.Windows.Forms.Label();
            this.btnShowDgvItems = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtItems = new System.Windows.Forms.TextBox();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrintQty)).BeginInit();
            this.panelFooter.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnShowDgvItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.numPrintQty);
            this.panelHeader.Controls.Add(this.CboCountType);
            this.panelHeader.Controls.Add(this.label6);
            this.panelHeader.Controls.Add(this.btnStart);
            this.panelHeader.Controls.Add(this.btnNew);
            this.panelHeader.Controls.Add(this.btnSave);
            this.panelHeader.Controls.Add(this.chkPrintStatus);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1174, 59);
            this.panelHeader.TabIndex = 8;
            // 
            // numPrintQty
            // 
            this.numPrintQty.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numPrintQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numPrintQty.Location = new System.Drawing.Point(1041, 19);
            this.numPrintQty.Name = "numPrintQty";
            this.numPrintQty.Size = new System.Drawing.Size(34, 21);
            this.numPrintQty.TabIndex = 23;
            this.numPrintQty.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPrintQty.Visible = false;
            // 
            // CboCountType
            // 
            this.CboCountType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.CboCountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboCountType.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CboCountType.FormattingEnabled = true;
            this.CboCountType.Location = new System.Drawing.Point(450, 9);
            this.CboCountType.Name = "CboCountType";
            this.CboCountType.Size = new System.Drawing.Size(244, 42);
            this.CboCountType.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(387, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 34);
            this.label6.TabIndex = 21;
            this.label6.Text = "រាប់ជា";
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnStart.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStart.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnStart.Location = new System.Drawing.Point(700, 9);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(92, 42);
            this.btnStart.TabIndex = 20;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnNew.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNew.BackgroundImage")));
            this.btnNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNew.Location = new System.Drawing.Point(5, 4);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(48, 51);
            this.btnNew.TabIndex = 16;
            this.toolTip1.SetToolTip(this.btnNew, "សារថ្មី");
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.Location = new System.Drawing.Point(57, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(48, 51);
            this.btnSave.TabIndex = 16;
            this.toolTip1.SetToolTip(this.btnSave, "រក្សាទុក");
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Visible = false;
            // 
            // chkPrintStatus
            // 
            this.chkPrintStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPrintStatus.AutoSize = true;
            this.chkPrintStatus.Checked = true;
            this.chkPrintStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrintStatus.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPrintStatus.Location = new System.Drawing.Point(1082, 21);
            this.chkPrintStatus.Name = "chkPrintStatus";
            this.chkPrintStatus.Size = new System.Drawing.Size(85, 19);
            this.chkPrintStatus.TabIndex = 17;
            this.chkPrintStatus.Text = "Print Label";
            this.chkPrintStatus.UseVisualStyleBackColor = true;
            this.chkPrintStatus.Visible = false;
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.SystemColors.Control;
            this.panelFooter.Controls.Add(this.LbStatus);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 527);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(1174, 34);
            this.panelFooter.TabIndex = 19;
            // 
            // LbStatus
            // 
            this.LbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LbStatus.Font = new System.Drawing.Font("Khmer OS Battambang", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbStatus.Location = new System.Drawing.Point(593, 5);
            this.LbStatus.Name = "LbStatus";
            this.LbStatus.Size = new System.Drawing.Size(575, 26);
            this.LbStatus.TabIndex = 7;
            this.LbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.groupBox3);
            this.panelBody.Controls.Add(this.btnShowDgvItems);
            this.panelBody.Controls.Add(this.label5);
            this.panelBody.Controls.Add(this.txtItems);
            this.panelBody.Controls.Add(this.dgvItems);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 59);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.panelBody.Size = new System.Drawing.Size(1174, 468);
            this.panelBody.TabIndex = 20;
            this.panelBody.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.tableLayoutPanel1);
            this.groupBox3.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(6, 43);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1162, 419);
            this.groupBox3.TabIndex = 127;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "ព៌ត័មានរបស់វត្ថុធាតុដើម";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.Controls.Add(this.CboBobbinW, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label24, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.LbTotalQty, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.label17, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtQty, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.label13, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.LbCode, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.LbQtyTitle, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label20, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label22, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.LbType, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.LbItems, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label23, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label21, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.LbMaker, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.LbBobbinsWTitle, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.LbNetWTitle, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label9, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.LbNetW, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.LbBobbinQty, 3, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 39);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1156, 377);
            this.tableLayoutPanel1.TabIndex = 26;
            // 
            // CboBobbinW
            // 
            this.CboBobbinW.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CboBobbinW.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboBobbinW.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CboBobbinW.FormattingEnabled = true;
            this.CboBobbinW.Location = new System.Drawing.Point(233, 217);
            this.CboBobbinW.Name = "CboBobbinW";
            this.CboBobbinW.Size = new System.Drawing.Size(253, 42);
            this.CboBobbinW.TabIndex = 22;
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(3, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(194, 53);
            this.label24.TabIndex = 0;
            this.label24.Text = "លេខកូដ";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LbTotalQty
            // 
            this.LbTotalQty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LbTotalQty.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbTotalQty.Location = new System.Drawing.Point(959, 318);
            this.LbTotalQty.Name = "LbTotalQty";
            this.LbTotalQty.Size = new System.Drawing.Size(194, 59);
            this.LbTotalQty.TabIndex = 24;
            this.LbTotalQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(203, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(24, 53);
            this.label17.TabIndex = 0;
            this.label17.Text = "៖";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtQty
            // 
            this.txtQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQty.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQty.Location = new System.Drawing.Point(233, 326);
            this.txtQty.MaxLength = 9999;
            this.txtQty.Name = "txtQty";
            this.txtQty.Size = new System.Drawing.Size(720, 43);
            this.txtQty.TabIndex = 2;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(203, 318);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(24, 59);
            this.label13.TabIndex = 22;
            this.label13.Text = "៖";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LbCode
            // 
            this.LbCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LbCode.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbCode.Location = new System.Drawing.Point(233, 0);
            this.LbCode.Name = "LbCode";
            this.LbCode.Size = new System.Drawing.Size(720, 53);
            this.LbCode.TabIndex = 0;
            this.LbCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LbQtyTitle
            // 
            this.LbQtyTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LbQtyTitle.AutoSize = true;
            this.LbQtyTitle.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbQtyTitle.Location = new System.Drawing.Point(3, 318);
            this.LbQtyTitle.Name = "LbQtyTitle";
            this.LbQtyTitle.Size = new System.Drawing.Size(194, 59);
            this.LbQtyTitle.TabIndex = 0;
            this.LbQtyTitle.Text = "ទម្ងន់សរុប";
            this.LbQtyTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(3, 53);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(194, 53);
            this.label20.TabIndex = 0;
            this.label20.Text = "ឈ្មោះវត្ថុធាតុដើម";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(3, 106);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(194, 53);
            this.label22.TabIndex = 0;
            this.label22.Text = "អ្នកផ្គត់ផ្គង់";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LbType
            // 
            this.LbType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LbType.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbType.Location = new System.Drawing.Point(233, 159);
            this.LbType.Name = "LbType";
            this.LbType.Size = new System.Drawing.Size(720, 53);
            this.LbType.TabIndex = 0;
            this.LbType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LbItems
            // 
            this.LbItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LbItems.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbItems.Location = new System.Drawing.Point(233, 53);
            this.LbItems.Name = "LbItems";
            this.LbItems.Size = new System.Drawing.Size(720, 53);
            this.LbItems.TabIndex = 0;
            this.LbItems.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(3, 159);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(194, 53);
            this.label23.TabIndex = 0;
            this.label23.Text = "ប្រភេទ";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(203, 159);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(24, 53);
            this.label21.TabIndex = 0;
            this.label21.Text = "៖";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LbMaker
            // 
            this.LbMaker.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LbMaker.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbMaker.Location = new System.Drawing.Point(233, 106);
            this.LbMaker.Name = "LbMaker";
            this.LbMaker.Size = new System.Drawing.Size(720, 53);
            this.LbMaker.TabIndex = 0;
            this.LbMaker.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(203, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 53);
            this.label4.TabIndex = 0;
            this.label4.Text = "៖";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(203, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 53);
            this.label3.TabIndex = 0;
            this.label3.Text = "៖";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LbBobbinsWTitle
            // 
            this.LbBobbinsWTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LbBobbinsWTitle.AutoSize = true;
            this.LbBobbinsWTitle.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbBobbinsWTitle.Location = new System.Drawing.Point(3, 212);
            this.LbBobbinsWTitle.Name = "LbBobbinsWTitle";
            this.LbBobbinsWTitle.Size = new System.Drawing.Size(194, 53);
            this.LbBobbinsWTitle.TabIndex = 0;
            this.LbBobbinsWTitle.Text = "ទម្ងន់ប៊ូប៊ីន (KG)";
            this.LbBobbinsWTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(203, 212);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 53);
            this.label2.TabIndex = 0;
            this.label2.Text = "៖";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LbNetWTitle
            // 
            this.LbNetWTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LbNetWTitle.AutoSize = true;
            this.LbNetWTitle.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbNetWTitle.Location = new System.Drawing.Point(3, 265);
            this.LbNetWTitle.Name = "LbNetWTitle";
            this.LbNetWTitle.Size = new System.Drawing.Size(194, 53);
            this.LbNetWTitle.TabIndex = 0;
            this.LbNetWTitle.Text = "ទម្ងន់សាច់សុទ្ធ (KG)";
            this.LbNetWTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(203, 265);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 53);
            this.label9.TabIndex = 0;
            this.label9.Text = "៖";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LbNetW
            // 
            this.LbNetW.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LbNetW.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbNetW.Location = new System.Drawing.Point(233, 265);
            this.LbNetW.Name = "LbNetW";
            this.LbNetW.Size = new System.Drawing.Size(720, 53);
            this.LbNetW.TabIndex = 0;
            this.LbNetW.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LbBobbinQty
            // 
            this.LbBobbinQty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LbBobbinQty.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbBobbinQty.Location = new System.Drawing.Point(959, 265);
            this.LbBobbinQty.Name = "LbBobbinQty";
            this.LbBobbinQty.Size = new System.Drawing.Size(194, 53);
            this.LbBobbinQty.TabIndex = 24;
            this.LbBobbinQty.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnShowDgvItems
            // 
            this.btnShowDgvItems.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnShowDgvItems.Image = ((System.Drawing.Image)(resources.GetObject("btnShowDgvItems.Image")));
            this.btnShowDgvItems.Location = new System.Drawing.Point(859, 8);
            this.btnShowDgvItems.Name = "btnShowDgvItems";
            this.btnShowDgvItems.Size = new System.Drawing.Size(33, 30);
            this.btnShowDgvItems.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnShowDgvItems.TabIndex = 126;
            this.btnShowDgvItems.TabStop = false;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Khmer OS Battambang", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label5.Location = new System.Drawing.Point(260, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(182, 27);
            this.label5.TabIndex = 124;
            this.label5.Text = "ស្កេន / ស្វែងរកវត្ថុធាតុដើម";
            // 
            // txtItems
            // 
            this.txtItems.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtItems.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItems.Location = new System.Drawing.Point(446, 6);
            this.txtItems.MaxLength = 20;
            this.txtItems.Name = "txtItems";
            this.txtItems.Size = new System.Drawing.Size(411, 32);
            this.txtItems.TabIndex = 125;
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.AllowUserToResizeColumns = false;
            this.dgvItems.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvItems.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvItems.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dgvItems.BackgroundColor = System.Drawing.Color.White;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.ColumnHeadersVisible = false;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            this.dgvItems.Location = new System.Drawing.Point(446, 43);
            this.dgvItems.MultiSelect = false;
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.ReadOnly = true;
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItems.Size = new System.Drawing.Size(446, 231);
            this.dgvItems.TabIndex = 128;
            this.dgvItems.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn5.HeaderText = "Code";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 85;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn6.HeaderText = "Items";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 343;
            // 
            // MCSDCountingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1174, 561);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MCSDCountingForm";
            this.Text = "បញ្ចូលរាប់ស្តុក (MC SD)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrintQty)).EndInit();
            this.panelFooter.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.panelBody.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnShowDgvItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox chkPrintStatus;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Label LbStatus;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.ComboBox CboCountType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label LbTotalQty;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label LbCode;
        private System.Windows.Forms.Label LbQtyTitle;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label LbType;
        private System.Windows.Forms.Label LbItems;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label LbMaker;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LbBobbinsWTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LbNetWTitle;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label LbNetW;
        private System.Windows.Forms.PictureBox btnShowDgvItems;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtItems;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.ComboBox CboBobbinW;
        private System.Windows.Forms.Label LbBobbinQty;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown numPrintQty;
    }
}