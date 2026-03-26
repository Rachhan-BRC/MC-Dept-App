namespace MachineDeptApp.RMConnector
{
    partial class BalanceStockConnectorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BalanceStockConnectorForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnserch = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvRM = new System.Windows.Forms.DataGridView();
            this.colRMCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRMDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalReceive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalIssued = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbFound = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRM)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnserch);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1115, 67);
            this.panel1.TabIndex = 1;
            // 
            // btnserch
            // 
            this.btnserch.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnserch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnserch.BackgroundImage")));
            this.btnserch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnserch.Font = new System.Drawing.Font("Khmer OS Battambang", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnserch.Location = new System.Drawing.Point(15, 6);
            this.btnserch.Name = "btnserch";
            this.btnserch.Size = new System.Drawing.Size(55, 55);
            this.btnserch.TabIndex = 0;
            this.btnserch.UseVisualStyleBackColor = false;
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnExport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExport.BackgroundImage")));
            this.btnExport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExport.Font = new System.Drawing.Font("Khmer OS Battambang", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(78, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(55, 55);
            this.btnExport.TabIndex = 1;
            this.btnExport.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvRM);
            this.panel2.Controls.Add(this.lbFound);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 67);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1115, 414);
            this.panel2.TabIndex = 2;
            // 
            // dgvRM
            // 
            this.dgvRM.AllowUserToAddRows = false;
            this.dgvRM.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            this.dgvRM.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRM.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRMCode,
            this.colRMDescription,
            this.colType,
            this.colLocation,
            this.colTotalReceive,
            this.colTotalIssued,
            this.colBalance});
            this.dgvRM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRM.Location = new System.Drawing.Point(0, 69);
            this.dgvRM.Name = "dgvRM";
            this.dgvRM.ReadOnly = true;
            this.dgvRM.RowHeadersVisible = false;
            this.dgvRM.Size = new System.Drawing.Size(1115, 345);
            this.dgvRM.TabIndex = 3;
            // 
            // colRMCode
            // 
            this.colRMCode.HeaderText = "RM Code";
            this.colRMCode.Name = "colRMCode";
            this.colRMCode.ReadOnly = true;
            this.colRMCode.Width = 110;
            // 
            // colRMDescription
            // 
            this.colRMDescription.HeaderText = "RM Description";
            this.colRMDescription.Name = "colRMDescription";
            this.colRMDescription.ReadOnly = true;
            this.colRMDescription.Width = 200;
            // 
            // colType
            // 
            this.colType.HeaderText = "Type";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            this.colType.Width = 80;
            // 
            // colLocation
            // 
            this.colLocation.HeaderText = "Location";
            this.colLocation.Name = "colLocation";
            this.colLocation.ReadOnly = true;
            // 
            // colTotalReceive
            // 
            this.colTotalReceive.HeaderText = "Total Receive";
            this.colTotalReceive.Name = "colTotalReceive";
            this.colTotalReceive.ReadOnly = true;
            this.colTotalReceive.Width = 110;
            // 
            // colTotalIssued
            // 
            this.colTotalIssued.HeaderText = "Total Issued";
            this.colTotalIssued.Name = "colTotalIssued";
            this.colTotalIssued.ReadOnly = true;
            this.colTotalIssued.Width = 110;
            // 
            // colBalance
            // 
            this.colBalance.HeaderText = "Balance";
            this.colBalance.Name = "colBalance";
            this.colBalance.ReadOnly = true;
            this.colBalance.Width = 90;
            // 
            // lbFound
            // 
            this.lbFound.AutoSize = true;
            this.lbFound.Location = new System.Drawing.Point(3, 95);
            this.lbFound.Name = "lbFound";
            this.lbFound.Size = new System.Drawing.Size(59, 19);
            this.lbFound.TabIndex = 2;
            this.lbFound.Text = "Found : 0";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkDate);
            this.groupBox1.Controls.Add(this.dtpFrom);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dtpTo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbDescription);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbCode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1115, 69);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Condition";
            // 
            // chkDate
            // 
            this.chkDate.AutoSize = true;
            this.chkDate.Location = new System.Drawing.Point(557, 26);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(93, 23);
            this.chkDate.TabIndex = 9;
            this.chkDate.Text = "Date Range";
            this.chkDate.UseVisualStyleBackColor = true;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Enabled = false;
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(700, 23);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(130, 28);
            this.dtpFrom.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(645, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 19);
            this.label3.TabIndex = 11;
            this.label3.Text = "From";
            // 
            // dtpTo
            // 
            this.dtpTo.Enabled = false;
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(869, 23);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(130, 28);
            this.dtpTo.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(840, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 19);
            this.label4.TabIndex = 13;
            this.label4.Text = "To";
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(311, 23);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(200, 28);
            this.tbDescription.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(213, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "RM Description";
            // 
            // tbCode
            // 
            this.tbCode.Location = new System.Drawing.Point(66, 22);
            this.tbCode.Name = "tbCode";
            this.tbCode.Size = new System.Drawing.Size(123, 28);
            this.tbCode.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Code";
            // 
            // BalanceStockConnectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1115, 481);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Khmer OS Battambang", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BalanceStockConnectorForm";
            this.Text = "ទិន្នន័យ Stock Connector";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRM)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnserch;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbFound;
        private System.Windows.Forms.DataGridView dgvRM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRMCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRMDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalReceive;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalIssued;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBalance;
        private System.Windows.Forms.CheckBox chkDate;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label4;
    }
}
