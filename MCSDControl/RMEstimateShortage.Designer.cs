namespace MachineDeptApp.MCSDControl
{
    partial class RMEstimateShortage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RMEstimateShortage));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle91 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle92 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle99 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle93 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle94 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle95 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle96 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle97 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle98 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.labelDataCount = new System.Windows.Forms.Label();
            this.panelBody = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbItemCode = new System.Windows.Forms.CheckBox();
            this.tbItemCode = new System.Windows.Forms.TextBox();
            this.cbItemName = new System.Windows.Forms.CheckBox();
            this.tbItemName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpPosShipStart = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpPosShipend = new System.Windows.Forms.DateTimePicker();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.dgvStock = new System.Windows.Forms.DataGridView();
            this.PosDeliveryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RMCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RMDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TTLRMUse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.McQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WHStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RMShortage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(884, 65);
            this.panel1.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExport.BackgroundImage")));
            this.btnExport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExport.Location = new System.Drawing.Point(829, 4);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(50, 55);
            this.btnExport.TabIndex = 0;
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearch.BackgroundImage")));
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.Location = new System.Drawing.Point(5, 4);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 55);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panelFooter.Controls.Add(this.labelDataCount);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 576);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(884, 35);
            this.panelFooter.TabIndex = 2;
            // 
            // labelDataCount
            // 
            this.labelDataCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDataCount.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDataCount.Location = new System.Drawing.Point(231, 5);
            this.labelDataCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDataCount.Name = "labelDataCount";
            this.labelDataCount.Size = new System.Drawing.Size(649, 25);
            this.labelDataCount.TabIndex = 1;
            this.labelDataCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.dgvStock);
            this.panelBody.Controls.Add(this.splitter1);
            this.panelBody.Controls.Add(this.groupBox1);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 65);
            this.panelBody.Margin = new System.Windows.Forms.Padding(0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(5);
            this.panelBody.Size = new System.Drawing.Size(884, 511);
            this.panelBody.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.cbItemCode);
            this.groupBox1.Controls.Add(this.tbItemCode);
            this.groupBox1.Controls.Add(this.tbItemName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.dtpPosShipStart);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dtpPosShipend);
            this.groupBox1.Controls.Add(this.cbItemName);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Kh Battambang", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(874, 95);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = "";
            this.groupBox1.Text = "Search Condtions";
            // 
            // cbItemCode
            // 
            this.cbItemCode.AutoSize = true;
            this.cbItemCode.Font = new System.Drawing.Font("Kh Battambang", 9F);
            this.cbItemCode.Location = new System.Drawing.Point(7, 22);
            this.cbItemCode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbItemCode.Name = "cbItemCode";
            this.cbItemCode.Size = new System.Drawing.Size(74, 26);
            this.cbItemCode.TabIndex = 12;
            this.cbItemCode.Text = "RM Code";
            this.cbItemCode.UseVisualStyleBackColor = true;
            // 
            // tbItemCode
            // 
            this.tbItemCode.Font = new System.Drawing.Font("Kh Battambang", 9F);
            this.tbItemCode.Location = new System.Drawing.Point(87, 20);
            this.tbItemCode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbItemCode.Name = "tbItemCode";
            this.tbItemCode.Size = new System.Drawing.Size(100, 30);
            this.tbItemCode.TabIndex = 10;
            // 
            // cbItemName
            // 
            this.cbItemName.AutoSize = true;
            this.cbItemName.Font = new System.Drawing.Font("Kh Battambang", 9F);
            this.cbItemName.Location = new System.Drawing.Point(210, 22);
            this.cbItemName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbItemName.Name = "cbItemName";
            this.cbItemName.Size = new System.Drawing.Size(105, 26);
            this.cbItemName.TabIndex = 13;
            this.cbItemName.Text = "RM Description";
            this.cbItemName.UseVisualStyleBackColor = true;
            // 
            // tbItemName
            // 
            this.tbItemName.Font = new System.Drawing.Font("Kh Battambang", 9F);
            this.tbItemName.Location = new System.Drawing.Point(321, 20);
            this.tbItemName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbItemName.Name = "tbItemName";
            this.tbItemName.Size = new System.Drawing.Size(234, 30);
            this.tbItemName.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Kh Battambang", 9F);
            this.label2.Location = new System.Drawing.Point(4, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 22);
            this.label2.TabIndex = 14;
            this.label2.Text = "POS Del.Date";
            // 
            // dtpPosShipStart
            // 
            this.dtpPosShipStart.Font = new System.Drawing.Font("Kh Battambang", 9F);
            this.dtpPosShipStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPosShipStart.Location = new System.Drawing.Point(87, 54);
            this.dtpPosShipStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpPosShipStart.Name = "dtpPosShipStart";
            this.dtpPosShipStart.Size = new System.Drawing.Size(100, 30);
            this.dtpPosShipStart.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Kh Battambang", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(190, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 22);
            this.label3.TabIndex = 17;
            this.label3.Text = "-";
            // 
            // dtpPosShipend
            // 
            this.dtpPosShipend.Font = new System.Drawing.Font("Kh Battambang", 9F);
            this.dtpPosShipend.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPosShipend.Location = new System.Drawing.Point(207, 54);
            this.dtpPosShipend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpPosShipend.Name = "dtpPosShipend";
            this.dtpPosShipend.Size = new System.Drawing.Size(100, 30);
            this.dtpPosShipend.TabIndex = 16;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Enabled = false;
            this.splitter1.Location = new System.Drawing.Point(5, 100);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(874, 3);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // dgvStock
            // 
            this.dgvStock.AllowUserToAddRows = false;
            this.dgvStock.AllowUserToDeleteRows = false;
            this.dgvStock.AllowUserToResizeRows = false;
            dataGridViewCellStyle91.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle91.Font = new System.Drawing.Font("Kh Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvStock.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle91;
            this.dgvStock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle92.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle92.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle92.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle92.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle92.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            dataGridViewCellStyle92.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle92.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle92.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStock.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle92;
            this.dgvStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PosDeliveryDate,
            this.RMCode,
            this.RMDescription,
            this.TTLRMUse,
            this.McQty,
            this.WHStock,
            this.RMShortage});
            this.dgvStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStock.EnableHeadersVisualStyles = false;
            this.dgvStock.Location = new System.Drawing.Point(5, 103);
            this.dgvStock.Margin = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.dgvStock.Name = "dgvStock";
            this.dgvStock.ReadOnly = true;
            this.dgvStock.RowHeadersVisible = false;
            this.dgvStock.RowHeadersWidth = 51;
            dataGridViewCellStyle99.Font = new System.Drawing.Font("Kh Battambang", 9F);
            this.dgvStock.RowsDefaultCellStyle = dataGridViewCellStyle99;
            this.dgvStock.Size = new System.Drawing.Size(874, 403);
            this.dgvStock.TabIndex = 3;
            // 
            // PosDeliveryDate
            // 
            this.PosDeliveryDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle93.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.PosDeliveryDate.DefaultCellStyle = dataGridViewCellStyle93;
            this.PosDeliveryDate.Frozen = true;
            this.PosDeliveryDate.HeaderText = "POS|Delivery Date";
            this.PosDeliveryDate.MinimumWidth = 6;
            this.PosDeliveryDate.Name = "PosDeliveryDate";
            this.PosDeliveryDate.ReadOnly = true;
            this.PosDeliveryDate.Width = 110;
            // 
            // RMCode
            // 
            this.RMCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle94.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.RMCode.DefaultCellStyle = dataGridViewCellStyle94;
            this.RMCode.Frozen = true;
            this.RMCode.HeaderText = "RM Code";
            this.RMCode.MinimumWidth = 6;
            this.RMCode.Name = "RMCode";
            this.RMCode.ReadOnly = true;
            this.RMCode.Width = 95;
            // 
            // RMDescription
            // 
            this.RMDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.RMDescription.Frozen = true;
            this.RMDescription.HeaderText = "RM Description";
            this.RMDescription.MinimumWidth = 6;
            this.RMDescription.Name = "RMDescription";
            this.RMDescription.ReadOnly = true;
            this.RMDescription.Width = 200;
            // 
            // TTLRMUse
            // 
            this.TTLRMUse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle95.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.TTLRMUse.DefaultCellStyle = dataGridViewCellStyle95;
            this.TTLRMUse.Frozen = true;
            this.TTLRMUse.HeaderText = "TTL RM Use";
            this.TTLRMUse.MinimumWidth = 6;
            this.TTLRMUse.Name = "TTLRMUse";
            this.TTLRMUse.ReadOnly = true;
            this.TTLRMUse.Width = 125;
            // 
            // McQty
            // 
            this.McQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle96.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.McQty.DefaultCellStyle = dataGridViewCellStyle96;
            this.McQty.HeaderText = "MC Qty";
            this.McQty.MinimumWidth = 6;
            this.McQty.Name = "McQty";
            this.McQty.ReadOnly = true;
            this.McQty.Width = 125;
            // 
            // WHStock
            // 
            this.WHStock.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle97.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.WHStock.DefaultCellStyle = dataGridViewCellStyle97;
            this.WHStock.HeaderText = "WHStock";
            this.WHStock.MinimumWidth = 6;
            this.WHStock.Name = "WHStock";
            this.WHStock.ReadOnly = true;
            this.WHStock.Width = 125;
            // 
            // RMShortage
            // 
            this.RMShortage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle98.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.RMShortage.DefaultCellStyle = dataGridViewCellStyle98;
            this.RMShortage.HeaderText = "RM Shortage";
            this.RMShortage.MinimumWidth = 6;
            this.RMShortage.Name = "RMShortage";
            this.RMShortage.ReadOnly = true;
            this.RMShortage.Width = 125;
            // 
            // RMEstimateShortage
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(884, 611);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "RMEstimateShortage";
            this.Text = "MC RM Estimate Shortage";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelDataCount;
        private System.Windows.Forms.CheckBox cbItemCode;
        private System.Windows.Forms.TextBox tbItemCode;
        private System.Windows.Forms.CheckBox cbItemName;
        private System.Windows.Forms.TextBox tbItemName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpPosShipStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpPosShipend;
        private System.Windows.Forms.DataGridView dgvStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn PosDeliveryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn RMCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn RMDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn TTLRMUse;
        private System.Windows.Forms.DataGridViewTextBoxColumn McQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn WHStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn RMShortage;
        private System.Windows.Forms.Splitter splitter1;
    }
}