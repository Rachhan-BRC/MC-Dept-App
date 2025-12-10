namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    partial class ShortageDetail
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbLacking = new System.Windows.Forms.Label();
            this.lbShortage = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvStock = new System.Windows.Forms.DataGridView();
            this.PosDeliveryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DONo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WipCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WipName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RMCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RMName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TTLusedQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbStatus = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbLacking);
            this.panel1.Controls.Add(this.lbShortage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(927, 48);
            this.panel1.TabIndex = 0;
            // 
            // lbLacking
            // 
            this.lbLacking.AutoSize = true;
            this.lbLacking.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLacking.Location = new System.Drawing.Point(294, 9);
            this.lbLacking.Name = "lbLacking";
            this.lbLacking.Size = new System.Drawing.Size(42, 24);
            this.lbLacking.TabIndex = 1;
            this.lbLacking.Text = "label1";
            // 
            // lbShortage
            // 
            this.lbShortage.AutoSize = true;
            this.lbShortage.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbShortage.Location = new System.Drawing.Point(12, 9);
            this.lbShortage.Name = "lbShortage";
            this.lbShortage.Size = new System.Drawing.Size(42, 24);
            this.lbShortage.TabIndex = 0;
            this.lbShortage.Text = "label1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvStock);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 48);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(927, 361);
            this.panel2.TabIndex = 1;
            // 
            // dgvStock
            // 
            this.dgvStock.AllowUserToAddRows = false;
            this.dgvStock.AllowUserToDeleteRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Lavender;
            this.dgvStock.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PosDeliveryDate,
            this.DONo,
            this.WipCode,
            this.WipName,
            this.LineCode,
            this.RMCode,
            this.RMName,
            this.TTLusedQty});
            this.dgvStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStock.Location = new System.Drawing.Point(0, 0);
            this.dgvStock.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.dgvStock.Name = "dgvStock";
            this.dgvStock.ReadOnly = true;
            this.dgvStock.RowHeadersVisible = false;
            this.dgvStock.Size = new System.Drawing.Size(927, 361);
            this.dgvStock.TabIndex = 2;
            // 
            // PosDeliveryDate
            // 
            this.PosDeliveryDate.HeaderText = "Pos Delivery Date";
            this.PosDeliveryDate.Name = "PosDeliveryDate";
            this.PosDeliveryDate.ReadOnly = true;
            this.PosDeliveryDate.Width = 120;
            // 
            // DONo
            // 
            this.DONo.HeaderText = "Do No";
            this.DONo.Name = "DONo";
            this.DONo.ReadOnly = true;
            // 
            // WipCode
            // 
            this.WipCode.HeaderText = "Wip Code";
            this.WipCode.Name = "WipCode";
            this.WipCode.ReadOnly = true;
            // 
            // WipName
            // 
            this.WipName.HeaderText = "Wip Name";
            this.WipName.Name = "WipName";
            this.WipName.ReadOnly = true;
            this.WipName.Width = 120;
            // 
            // LineCode
            // 
            this.LineCode.HeaderText = "Line Code";
            this.LineCode.Name = "LineCode";
            this.LineCode.ReadOnly = true;
            this.LineCode.Width = 120;
            // 
            // RMCode
            // 
            this.RMCode.HeaderText = "RM Code";
            this.RMCode.Name = "RMCode";
            this.RMCode.ReadOnly = true;
            // 
            // RMName
            // 
            this.RMName.HeaderText = "RM Name";
            this.RMName.Name = "RMName";
            this.RMName.ReadOnly = true;
            // 
            // TTLusedQty
            // 
            this.TTLusedQty.HeaderText = "TTLUsedQty";
            this.TTLusedQty.Name = "TTLusedQty";
            this.TTLusedQty.ReadOnly = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lbStatus);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 409);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(927, 36);
            this.panel3.TabIndex = 2;
            // 
            // lbStatus
            // 
            this.lbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(828, 6);
            this.lbStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(0, 19);
            this.lbStatus.TabIndex = 0;
            // 
            // ShortageDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 445);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Khmer OS Battambang", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ShortageDetail";
            this.Text = "ShortageDetail";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dgvStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn PosDeliveryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn DONo;
        private System.Windows.Forms.DataGridViewTextBoxColumn WipCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn WipName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn RMCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn RMName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TTLusedQty;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Label lbShortage;
        private System.Windows.Forms.Label lbLacking;
    }
}