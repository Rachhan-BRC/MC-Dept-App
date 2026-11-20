namespace MachineDeptApp.TrackingPOS
{
    partial class POSTrackingSearchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POSTrackingSearchForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.btnReprint = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.LbTotalResult = new System.Windows.Forms.Label();
            this.LbExportStatus = new System.Windows.Forms.Label();
            this.panelBody = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvPOSData = new System.Windows.Forms.DataGridView();
            this.ColCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPOSNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTransferQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColRemainQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.DtEndInput = new System.Windows.Forms.DateTimePicker();
            this.DtInput = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPOSNo = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtWipCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPOSData)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.label10);
            this.panelHeader.Controls.Add(this.btnReprint);
            this.panelHeader.Controls.Add(this.btnSearch);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1274, 68);
            this.panelHeader.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Khmer OS Fasthand", 21.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(533, 2);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(207, 65);
            this.label10.TabIndex = 15;
            this.label10.Text = "ទិន្នន័យ POS";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnReprint
            // 
            this.btnReprint.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnReprint.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReprint.BackgroundImage")));
            this.btnReprint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReprint.Location = new System.Drawing.Point(67, 3);
            this.btnReprint.Name = "btnReprint";
            this.btnReprint.Size = new System.Drawing.Size(60, 60);
            this.btnReprint.TabIndex = 14;
            this.btnReprint.UseVisualStyleBackColor = false;
            this.btnReprint.Click += new System.EventHandler(this.btnReprint_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearch.BackgroundImage")));
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.Location = new System.Drawing.Point(4, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(60, 60);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.LbTotalResult);
            this.panelFooter.Controls.Add(this.LbExportStatus);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 620);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(1274, 41);
            this.panelFooter.TabIndex = 9;
            // 
            // LbTotalResult
            // 
            this.LbTotalResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LbTotalResult.AutoSize = true;
            this.LbTotalResult.Font = new System.Drawing.Font("Kh Battambang", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbTotalResult.Location = new System.Drawing.Point(2, 5);
            this.LbTotalResult.Name = "LbTotalResult";
            this.LbTotalResult.Size = new System.Drawing.Size(83, 29);
            this.LbTotalResult.TabIndex = 7;
            this.LbTotalResult.Text = "លុបរួចរាល់";
            this.LbTotalResult.Visible = false;
            // 
            // LbExportStatus
            // 
            this.LbExportStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LbExportStatus.Font = new System.Drawing.Font("Kh Battambang", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbExportStatus.Location = new System.Drawing.Point(321, 5);
            this.LbExportStatus.Name = "LbExportStatus";
            this.LbExportStatus.Size = new System.Drawing.Size(947, 34);
            this.LbExportStatus.TabIndex = 7;
            this.LbExportStatus.Text = "កំពុងបង្កើត File . . . .";
            this.LbExportStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LbExportStatus.Visible = false;
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.groupBox2);
            this.panelBody.Controls.Add(this.groupBox1);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 68);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.panelBody.Size = new System.Drawing.Size(1274, 552);
            this.panelBody.TabIndex = 10;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvPOSData);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Kh Battambang", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(3, 71);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(1268, 478);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "លទ្ធផលស្វែងរក";
            // 
            // dgvPOSData
            // 
            this.dgvPOSData.AllowUserToAddRows = false;
            this.dgvPOSData.AllowUserToDeleteRows = false;
            this.dgvPOSData.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.PaleTurquoise;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Kh Battambang", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvPOSData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPOSData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Kh Battambang", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPOSData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPOSData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPOSData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColCode,
            this.ColItemName,
            this.ColPOSNo,
            this.ColQty,
            this.ColTransferQty,
            this.ColRemainQty,
            this.ColStatus});
            this.dgvPOSData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPOSData.EnableHeadersVisualStyles = false;
            this.dgvPOSData.Location = new System.Drawing.Point(4, 21);
            this.dgvPOSData.MultiSelect = false;
            this.dgvPOSData.Name = "dgvPOSData";
            this.dgvPOSData.ReadOnly = true;
            this.dgvPOSData.RowHeadersVisible = false;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Kh Battambang", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.White;
            this.dgvPOSData.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvPOSData.RowTemplate.Height = 25;
            this.dgvPOSData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvPOSData.Size = new System.Drawing.Size(1260, 452);
            this.dgvPOSData.TabIndex = 12;
            // 
            // ColCode
            // 
            this.ColCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ColCode.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColCode.FillWeight = 190F;
            this.ColCode.HeaderText = "លេខកូដ";
            this.ColCode.Name = "ColCode";
            this.ColCode.ReadOnly = true;
            this.ColCode.Width = 90;
            // 
            // ColItemName
            // 
            this.ColItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.NullValue = null;
            this.ColItemName.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColItemName.HeaderText = "ឈ្មោះផលិតផល";
            this.ColItemName.Name = "ColItemName";
            this.ColItemName.ReadOnly = true;
            this.ColItemName.Width = 300;
            // 
            // ColPOSNo
            // 
            this.ColPOSNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColPOSNo.HeaderText = "លេខ POS";
            this.ColPOSNo.Name = "ColPOSNo";
            this.ColPOSNo.ReadOnly = true;
            this.ColPOSNo.Width = 110;
            // 
            // ColQty
            // 
            this.ColQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Kh Battambang", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.Format = "N0";
            dataGridViewCellStyle5.NullValue = null;
            this.ColQty.DefaultCellStyle = dataGridViewCellStyle5;
            this.ColQty.FillWeight = 160F;
            this.ColQty.HeaderText = "ចំនួន";
            this.ColQty.Name = "ColQty";
            this.ColQty.ReadOnly = true;
            this.ColQty.Width = 130;
            // 
            // ColTransferQty
            // 
            this.ColTransferQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            this.ColTransferQty.DefaultCellStyle = dataGridViewCellStyle6;
            this.ColTransferQty.HeaderText = "ចំនួនវេរ";
            this.ColTransferQty.Name = "ColTransferQty";
            this.ColTransferQty.ReadOnly = true;
            this.ColTransferQty.Width = 130;
            // 
            // ColRemainQty
            // 
            this.ColRemainQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N0";
            this.ColRemainQty.DefaultCellStyle = dataGridViewCellStyle7;
            this.ColRemainQty.HeaderText = "ចំនួននៅសល់";
            this.ColRemainQty.Name = "ColRemainQty";
            this.ColRemainQty.ReadOnly = true;
            this.ColRemainQty.Width = 130;
            // 
            // ColStatus
            // 
            this.ColStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColStatus.HeaderText = "ស្ថានភាព";
            this.ColStatus.Name = "ColStatus";
            this.ColStatus.ReadOnly = true;
            this.ColStatus.Width = 200;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkDate);
            this.groupBox1.Controls.Add(this.DtEndInput);
            this.groupBox1.Controls.Add(this.DtInput);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtPOSNo);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.txtWipCode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Kh Battambang", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(3, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1268, 71);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "លក្ខខណ្ឌស្វែងរក";
            // 
            // chkDate
            // 
            this.chkDate.AutoSize = true;
            this.chkDate.Font = new System.Drawing.Font("Kh Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDate.Location = new System.Drawing.Point(669, 29);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(127, 28);
            this.chkDate.TabIndex = 8;
            this.chkDate.Text = "ថ្ងៃខែឆ្នាំទម្លាក់ POS";
            this.chkDate.UseVisualStyleBackColor = true;
            // 
            // DtEndInput
            // 
            this.DtEndInput.CustomFormat = "MM-yyyy";
            this.DtEndInput.Font = new System.Drawing.Font("Kh Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtEndInput.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtEndInput.Location = new System.Drawing.Point(917, 27);
            this.DtEndInput.Name = "DtEndInput";
            this.DtEndInput.Size = new System.Drawing.Size(90, 32);
            this.DtEndInput.TabIndex = 6;
            // 
            // DtInput
            // 
            this.DtInput.CustomFormat = "MM-yyyy";
            this.DtInput.Font = new System.Drawing.Font("Kh Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtInput.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtInput.Location = new System.Drawing.Point(802, 27);
            this.DtInput.Name = "DtInput";
            this.DtInput.Size = new System.Drawing.Size(92, 32);
            this.DtInput.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Kh Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(898, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "-";
            // 
            // txtPOSNo
            // 
            this.txtPOSNo.Font = new System.Drawing.Font("Kh Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPOSNo.Location = new System.Drawing.Point(522, 27);
            this.txtPOSNo.MaxLength = 15;
            this.txtPOSNo.Name = "txtPOSNo";
            this.txtPOSNo.Size = new System.Drawing.Size(141, 32);
            this.txtPOSNo.TabIndex = 1;
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Kh Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(257, 27);
            this.txtName.MaxLength = 50;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(189, 32);
            this.txtName.TabIndex = 1;
            // 
            // txtWipCode
            // 
            this.txtWipCode.Font = new System.Drawing.Font("Kh Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWipCode.Location = new System.Drawing.Point(62, 27);
            this.txtWipCode.MaxLength = 10;
            this.txtWipCode.Name = "txtWipCode";
            this.txtWipCode.Size = new System.Drawing.Size(92, 32);
            this.txtWipCode.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Kh Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(452, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 24);
            this.label4.TabIndex = 0;
            this.label4.Text = "លេខ POS";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Kh Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(159, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 24);
            this.label2.TabIndex = 0;
            this.label2.Text = "ឈ្មោះផលិតផល";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Kh Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "លេខកូដ";
            // 
            // POSTrackingSearchForm
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1274, 661);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "POSTrackingSearchForm";
            this.Text = "ឆែកទិន្នន័យ POS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.POSTrackingSearchForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            this.panelBody.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPOSData)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Label LbTotalResult;
        private System.Windows.Forms.Label LbExportStatus;
        private System.Windows.Forms.Button btnReprint;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.DataGridView dgvPOSData;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkDate;
        private System.Windows.Forms.DateTimePicker DtEndInput;
        private System.Windows.Forms.DateTimePicker DtInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPOSNo;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtWipCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPOSNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTransferQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColRemainQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColStatus;
    }
}