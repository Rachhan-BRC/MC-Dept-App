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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvStock = new System.Windows.Forms.DataGridView();
            this.PosDeliveryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RMCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RMDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TTLRMUse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.McQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WHStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RMShortage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpPosShipend = new System.Windows.Forms.DateTimePicker();
            this.dtpPosShipStart = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.cbItemName = new System.Windows.Forms.CheckBox();
            this.cbItemCode = new System.Windows.Forms.CheckBox();
            this.tbItemName = new System.Windows.Forms.TextBox();
            this.tbItemCode = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelDataCount = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1094, 59);
            this.panel1.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearch.BackgroundImage")));
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.Location = new System.Drawing.Point(14, 4);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 50);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvStock);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 59);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1094, 439);
            this.panel2.TabIndex = 1;
            // 
            // dgvStock
            // 
            this.dgvStock.AllowUserToAddRows = false;
            this.dgvStock.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            this.dgvStock.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvStock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
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
            this.dgvStock.Location = new System.Drawing.Point(0, 102);
            this.dgvStock.Margin = new System.Windows.Forms.Padding(4);
            this.dgvStock.Name = "dgvStock";
            this.dgvStock.ReadOnly = true;
            this.dgvStock.RowHeadersVisible = false;
            this.dgvStock.Size = new System.Drawing.Size(1094, 337);
            this.dgvStock.TabIndex = 1;
            // 
            // PosDeliveryDate
            // 
            this.PosDeliveryDate.HeaderText = "Pos Delivery Date";
            this.PosDeliveryDate.Name = "PosDeliveryDate";
            this.PosDeliveryDate.ReadOnly = true;
            this.PosDeliveryDate.Width = 121;
            // 
            // RMCode
            // 
            this.RMCode.HeaderText = "RM Code";
            this.RMCode.Name = "RMCode";
            this.RMCode.ReadOnly = true;
            this.RMCode.Width = 77;
            // 
            // RMDescription
            // 
            this.RMDescription.HeaderText = "RM Description";
            this.RMDescription.Name = "RMDescription";
            this.RMDescription.ReadOnly = true;
            this.RMDescription.Width = 107;
            // 
            // TTLRMUse
            // 
            this.TTLRMUse.HeaderText = "TTL RM Use";
            this.TTLRMUse.Name = "TTLRMUse";
            this.TTLRMUse.ReadOnly = true;
            this.TTLRMUse.Width = 90;
            // 
            // McQty
            // 
            this.McQty.HeaderText = "MC Qty";
            this.McQty.Name = "McQty";
            this.McQty.ReadOnly = true;
            this.McQty.Width = 70;
            // 
            // WHStock
            // 
            this.WHStock.HeaderText = "WHStock";
            this.WHStock.Name = "WHStock";
            this.WHStock.ReadOnly = true;
            this.WHStock.Width = 84;
            // 
            // RMShortage
            // 
            this.RMShortage.HeaderText = "RM Shortage";
            this.RMShortage.Name = "RMShortage";
            this.RMShortage.ReadOnly = true;
            this.RMShortage.Width = 96;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dtpPosShipend);
            this.groupBox1.Controls.Add(this.dtpPosShipStart);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbItemName);
            this.groupBox1.Controls.Add(this.cbItemCode);
            this.groupBox1.Controls.Add(this.tbItemName);
            this.groupBox1.Controls.Add(this.tbItemCode);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1094, 102);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Condtion";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(205, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 19);
            this.label3.TabIndex = 9;
            this.label3.Text = "-";
            // 
            // dtpPosShipend
            // 
            this.dtpPosShipend.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPosShipend.Location = new System.Drawing.Point(224, 64);
            this.dtpPosShipend.Name = "dtpPosShipend";
            this.dtpPosShipend.Size = new System.Drawing.Size(92, 28);
            this.dtpPosShipend.TabIndex = 8;
            // 
            // dtpPosShipStart
            // 
            this.dtpPosShipStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPosShipStart.Location = new System.Drawing.Point(108, 64);
            this.dtpPosShipStart.Name = "dtpPosShipStart";
            this.dtpPosShipStart.Size = new System.Drawing.Size(91, 28);
            this.dtpPosShipStart.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 19);
            this.label2.TabIndex = 6;
            this.label2.Text = "Pos Ship Date";
            // 
            // cbItemName
            // 
            this.cbItemName.AutoSize = true;
            this.cbItemName.Location = new System.Drawing.Point(322, 29);
            this.cbItemName.Name = "cbItemName";
            this.cbItemName.Size = new System.Drawing.Size(82, 23);
            this.cbItemName.TabIndex = 3;
            this.cbItemName.Text = "ItemName";
            this.cbItemName.UseVisualStyleBackColor = true;
            // 
            // cbItemCode
            // 
            this.cbItemCode.AutoSize = true;
            this.cbItemCode.Location = new System.Drawing.Point(17, 31);
            this.cbItemCode.Name = "cbItemCode";
            this.cbItemCode.Size = new System.Drawing.Size(80, 23);
            this.cbItemCode.TabIndex = 2;
            this.cbItemCode.Text = "ItemCode";
            this.cbItemCode.UseVisualStyleBackColor = true;
            // 
            // tbItemName
            // 
            this.tbItemName.Location = new System.Drawing.Point(410, 27);
            this.tbItemName.Name = "tbItemName";
            this.tbItemName.Size = new System.Drawing.Size(208, 28);
            this.tbItemName.TabIndex = 1;
            // 
            // tbItemCode
            // 
            this.tbItemCode.Location = new System.Drawing.Point(108, 26);
            this.tbItemCode.Name = "tbItemCode";
            this.tbItemCode.Size = new System.Drawing.Size(208, 28);
            this.tbItemCode.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelDataCount);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 498);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1094, 37);
            this.panel3.TabIndex = 1;
            // 
            // labelDataCount
            // 
            this.labelDataCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDataCount.AutoSize = true;
            this.labelDataCount.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDataCount.Location = new System.Drawing.Point(1039, 8);
            this.labelDataCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDataCount.Name = "labelDataCount";
            this.labelDataCount.Size = new System.Drawing.Size(0, 24);
            this.labelDataCount.TabIndex = 0;
            // 
            // RMEstimateShortage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1094, 535);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Khmer OS Battambang", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RMEstimateShortage";
            this.Text = "RMEstimateShortage";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelDataCount;
        private System.Windows.Forms.DataGridView dgvStock;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbItemName;
        private System.Windows.Forms.CheckBox cbItemCode;
        private System.Windows.Forms.TextBox tbItemName;
        private System.Windows.Forms.TextBox tbItemCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpPosShipStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpPosShipend;
        private System.Windows.Forms.DataGridViewTextBoxColumn PosDeliveryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn RMCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn RMDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn TTLRMUse;
        private System.Windows.Forms.DataGridViewTextBoxColumn McQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn WHStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn RMShortage;
    }
}