namespace MachineDeptApp.RMConnector
{
    partial class TransactionConnectorAddForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.lblSearchCode = new System.Windows.Forms.Label();
            this.tbSearchCode = new System.Windows.Forms.TextBox();
            this.lblSearchDesc = new System.Windows.Forms.Label();
            this.tbSearchDesc = new System.Windows.Forms.TextBox();
            this.btnRMSearch = new System.Windows.Forms.Button();
            this.dgvRMSearch = new System.Windows.Forms.DataGridView();
            this.colRMCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRMDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpInput = new System.Windows.Forms.GroupBox();
            this.lblItemCode = new System.Windows.Forms.Label();
            this.tbItemCode = new System.Windows.Forms.TextBox();
            this.lblItemName = new System.Windows.Forms.Label();
            this.tbItemName = new System.Windows.Forms.TextBox();
            this.lblReceiveQty = new System.Windows.Forms.Label();
            this.tbReceiveQty = new System.Windows.Forms.TextBox();
            this.lblIssuedQty = new System.Windows.Forms.Label();
            this.tbIssuedQty = new System.Windows.Forms.TextBox();
            this.lblRemark = new System.Windows.Forms.Label();
            this.tbRemark = new System.Windows.Forms.TextBox();
            this.lblBalanceTitle = new System.Windows.Forms.Label();
            this.lbBalance = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRMSearch)).BeginInit();
            this.grpInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.lblSearchCode);
            this.grpSearch.Controls.Add(this.tbSearchCode);
            this.grpSearch.Controls.Add(this.lblSearchDesc);
            this.grpSearch.Controls.Add(this.tbSearchDesc);
            this.grpSearch.Controls.Add(this.btnRMSearch);
            this.grpSearch.Controls.Add(this.dgvRMSearch);
            this.grpSearch.Location = new System.Drawing.Point(12, 12);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Size = new System.Drawing.Size(560, 210);
            this.grpSearch.TabIndex = 0;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "Search RM Connector";
            // 
            // lblSearchCode
            // 
            this.lblSearchCode.AutoSize = true;
            this.lblSearchCode.Location = new System.Drawing.Point(10, 28);
            this.lblSearchCode.Name = "lblSearchCode";
            this.lblSearchCode.Size = new System.Drawing.Size(38, 19);
            this.lblSearchCode.TabIndex = 0;
            this.lblSearchCode.Text = "Code";
            // 
            // tbSearchCode
            // 
            this.tbSearchCode.Location = new System.Drawing.Point(90, 25);
            this.tbSearchCode.Name = "tbSearchCode";
            this.tbSearchCode.Size = new System.Drawing.Size(120, 28);
            this.tbSearchCode.TabIndex = 1;
            // 
            // lblSearchDesc
            // 
            this.lblSearchDesc.AutoSize = true;
            this.lblSearchDesc.Location = new System.Drawing.Point(220, 28);
            this.lblSearchDesc.Name = "lblSearchDesc";
            this.lblSearchDesc.Size = new System.Drawing.Size(72, 19);
            this.lblSearchDesc.TabIndex = 2;
            this.lblSearchDesc.Text = "Description";
            // 
            // tbSearchDesc
            // 
            this.tbSearchDesc.Location = new System.Drawing.Point(305, 25);
            this.tbSearchDesc.Name = "tbSearchDesc";
            this.tbSearchDesc.Size = new System.Drawing.Size(150, 28);
            this.tbSearchDesc.TabIndex = 3;
            // 
            // btnRMSearch
            // 
            this.btnRMSearch.Location = new System.Drawing.Point(465, 23);
            this.btnRMSearch.Name = "btnRMSearch";
            this.btnRMSearch.Size = new System.Drawing.Size(80, 30);
            this.btnRMSearch.TabIndex = 4;
            this.btnRMSearch.Text = "Search";
            this.btnRMSearch.UseVisualStyleBackColor = true;
            // 
            // dgvRMSearch
            // 
            this.dgvRMSearch.AllowUserToAddRows = false;
            this.dgvRMSearch.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            this.dgvRMSearch.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRMSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRMSearch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRMCode,
            this.colRMDescription,
            this.colType,
            this.colLocation});
            this.dgvRMSearch.Location = new System.Drawing.Point(10, 62);
            this.dgvRMSearch.Name = "dgvRMSearch";
            this.dgvRMSearch.ReadOnly = true;
            this.dgvRMSearch.RowHeadersVisible = false;
            this.dgvRMSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRMSearch.Size = new System.Drawing.Size(535, 135);
            this.dgvRMSearch.TabIndex = 5;
            // 
            // colRMCode
            // 
            this.colRMCode.HeaderText = "RM Code";
            this.colRMCode.Name = "colRMCode";
            this.colRMCode.ReadOnly = true;
            this.colRMCode.Width = 130;
            // 
            // colRMDescription
            // 
            this.colRMDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colRMDescription.HeaderText = "RM Description";
            this.colRMDescription.Name = "colRMDescription";
            this.colRMDescription.ReadOnly = true;
            // 
            // colType
            // 
            this.colType.HeaderText = "Type";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            this.colType.Visible = false;
            // 
            // colLocation
            // 
            this.colLocation.HeaderText = "Location";
            this.colLocation.Name = "colLocation";
            this.colLocation.ReadOnly = true;
            this.colLocation.Visible = false;
            // 
            // grpInput
            // 
            this.grpInput.Controls.Add(this.lblItemCode);
            this.grpInput.Controls.Add(this.tbItemCode);
            this.grpInput.Controls.Add(this.lblItemName);
            this.grpInput.Controls.Add(this.tbItemName);
            this.grpInput.Controls.Add(this.lblReceiveQty);
            this.grpInput.Controls.Add(this.tbReceiveQty);
            this.grpInput.Controls.Add(this.lblIssuedQty);
            this.grpInput.Controls.Add(this.tbIssuedQty);
            this.grpInput.Controls.Add(this.lblRemark);
            this.grpInput.Controls.Add(this.tbRemark);
            this.grpInput.Controls.Add(this.lblBalanceTitle);
            this.grpInput.Controls.Add(this.lbBalance);
            this.grpInput.Location = new System.Drawing.Point(12, 230);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(560, 260);
            this.grpInput.TabIndex = 1;
            this.grpInput.TabStop = false;
            this.grpInput.Text = "Transaction Detail";
            // 
            // lblItemCode
            // 
            this.lblItemCode.AutoSize = true;
            this.lblItemCode.Location = new System.Drawing.Point(10, 28);
            this.lblItemCode.Name = "lblItemCode";
            this.lblItemCode.Size = new System.Drawing.Size(64, 19);
            this.lblItemCode.TabIndex = 0;
            this.lblItemCode.Text = "Item Code";
            // 
            // tbItemCode
            // 
            this.tbItemCode.BackColor = System.Drawing.SystemColors.Control;
            this.tbItemCode.Location = new System.Drawing.Point(160, 25);
            this.tbItemCode.Name = "tbItemCode";
            this.tbItemCode.ReadOnly = true;
            this.tbItemCode.Size = new System.Drawing.Size(370, 28);
            this.tbItemCode.TabIndex = 1;
            // 
            // lblItemName
            // 
            this.lblItemName.AutoSize = true;
            this.lblItemName.Location = new System.Drawing.Point(10, 68);
            this.lblItemName.Name = "lblItemName";
            this.lblItemName.Size = new System.Drawing.Size(92, 19);
            this.lblItemName.TabIndex = 2;
            this.lblItemName.Text = "RM Description";
            // 
            // tbItemName
            // 
            this.tbItemName.BackColor = System.Drawing.SystemColors.Control;
            this.tbItemName.Location = new System.Drawing.Point(160, 65);
            this.tbItemName.Name = "tbItemName";
            this.tbItemName.ReadOnly = true;
            this.tbItemName.Size = new System.Drawing.Size(370, 28);
            this.tbItemName.TabIndex = 3;
            // 
            // lblReceiveQty
            // 
            this.lblReceiveQty.AutoSize = true;
            this.lblReceiveQty.Location = new System.Drawing.Point(10, 108);
            this.lblReceiveQty.Name = "lblReceiveQty";
            this.lblReceiveQty.Size = new System.Drawing.Size(80, 19);
            this.lblReceiveQty.TabIndex = 4;
            this.lblReceiveQty.Text = "Receive Qty";
            // 
            // tbReceiveQty
            // 
            this.tbReceiveQty.Location = new System.Drawing.Point(160, 105);
            this.tbReceiveQty.Name = "tbReceiveQty";
            this.tbReceiveQty.Size = new System.Drawing.Size(370, 28);
            this.tbReceiveQty.TabIndex = 5;
            // 
            // lblIssuedQty
            // 
            this.lblIssuedQty.AutoSize = true;
            this.lblIssuedQty.Location = new System.Drawing.Point(10, 148);
            this.lblIssuedQty.Name = "lblIssuedQty";
            this.lblIssuedQty.Size = new System.Drawing.Size(70, 19);
            this.lblIssuedQty.TabIndex = 6;
            this.lblIssuedQty.Text = "Issued Qty";
            // 
            // tbIssuedQty
            // 
            this.tbIssuedQty.Location = new System.Drawing.Point(160, 145);
            this.tbIssuedQty.Name = "tbIssuedQty";
            this.tbIssuedQty.Size = new System.Drawing.Size(370, 28);
            this.tbIssuedQty.TabIndex = 7;
            // 
            // lblRemark
            // 
            this.lblRemark.AutoSize = true;
            this.lblRemark.Location = new System.Drawing.Point(10, 188);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(51, 19);
            this.lblRemark.TabIndex = 8;
            this.lblRemark.Text = "Remark";
            // 
            // tbRemark
            // 
            this.tbRemark.Location = new System.Drawing.Point(160, 185);
            this.tbRemark.Name = "tbRemark";
            this.tbRemark.Size = new System.Drawing.Size(370, 28);
            this.tbRemark.TabIndex = 9;
            //
            // lblBalanceTitle
            //
            this.lblBalanceTitle.AutoSize = true;
            this.lblBalanceTitle.Location = new System.Drawing.Point(10, 228);
            this.lblBalanceTitle.Name = "lblBalanceTitle";
            this.lblBalanceTitle.Size = new System.Drawing.Size(100, 19);
            this.lblBalanceTitle.TabIndex = 10;
            this.lblBalanceTitle.Text = "Current Balance";
            //
            // lbBalance
            //
            this.lbBalance.AutoSize = true;
            this.lbBalance.Font = new System.Drawing.Font("Khmer OS Battambang", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBalance.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbBalance.Location = new System.Drawing.Point(157, 228);
            this.lbBalance.Name = "lbBalance";
            this.lbBalance.Size = new System.Drawing.Size(14, 19);
            this.lbBalance.TabIndex = 11;
            this.lbBalance.Text = "-";
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(352, 503);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(472, 503);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // TransactionConnectorAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 550);
            this.Controls.Add(this.grpSearch);
            this.Controls.Add(this.grpInput);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Khmer OS Battambang", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TransactionConnectorAddForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Transaction Connector";
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRMSearch)).EndInit();
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.Label lblSearchCode;
        private System.Windows.Forms.TextBox tbSearchCode;
        private System.Windows.Forms.Label lblSearchDesc;
        private System.Windows.Forms.TextBox tbSearchDesc;
        private System.Windows.Forms.Button btnRMSearch;
        private System.Windows.Forms.DataGridView dgvRMSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRMCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRMDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocation;
        private System.Windows.Forms.GroupBox grpInput;
        private System.Windows.Forms.Label lblItemCode;
        private System.Windows.Forms.TextBox tbItemCode;
        private System.Windows.Forms.Label lblItemName;
        private System.Windows.Forms.TextBox tbItemName;
        private System.Windows.Forms.Label lblReceiveQty;
        private System.Windows.Forms.TextBox tbReceiveQty;
        private System.Windows.Forms.Label lblIssuedQty;
        private System.Windows.Forms.TextBox tbIssuedQty;
        private System.Windows.Forms.Label lblRemark;
        private System.Windows.Forms.TextBox tbRemark;
        private System.Windows.Forms.Label lblBalanceTitle;
        private System.Windows.Forms.Label lbBalance;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
