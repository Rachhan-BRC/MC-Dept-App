namespace MachineDeptApp.RMConnector
{
    partial class TransactionConnectorEditForm
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
            this.lblId         = new System.Windows.Forms.Label();
            this.lblItemCode   = new System.Windows.Forms.Label();
            this.lblReceiveQty = new System.Windows.Forms.Label();
            this.lblIssuedQty  = new System.Windows.Forms.Label();
            this.lblRemark     = new System.Windows.Forms.Label();
            this.tbId          = new System.Windows.Forms.TextBox();
            this.tbItemCode    = new System.Windows.Forms.TextBox();
            this.tbReceiveQty  = new System.Windows.Forms.TextBox();
            this.tbIssuedQty   = new System.Windows.Forms.TextBox();
            this.tbRemark      = new System.Windows.Forms.TextBox();
            this.btnSave       = new System.Windows.Forms.Button();
            this.btnCancel     = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblId
            //
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(20, 22);
            this.lblId.Name = "lblId";
            this.lblId.TabIndex = 0;
            this.lblId.Text = "Id";
            //
            // tbId
            //
            this.tbId.BackColor = System.Drawing.SystemColors.Control;
            this.tbId.Location = new System.Drawing.Point(160, 19);
            this.tbId.Name = "tbId";
            this.tbId.ReadOnly = true;
            this.tbId.Size = new System.Drawing.Size(220, 28);
            this.tbId.TabIndex = 1;
            //
            // lblItemCode
            //
            this.lblItemCode.AutoSize = true;
            this.lblItemCode.Location = new System.Drawing.Point(20, 62);
            this.lblItemCode.Name = "lblItemCode";
            this.lblItemCode.TabIndex = 2;
            this.lblItemCode.Text = "Item Code";
            //
            // tbItemCode
            //
            this.tbItemCode.Location = new System.Drawing.Point(160, 59);
            this.tbItemCode.Name = "tbItemCode";
            this.tbItemCode.Size = new System.Drawing.Size(220, 28);
            this.tbItemCode.TabIndex = 3;
            //
            // lblReceiveQty
            //
            this.lblReceiveQty.AutoSize = true;
            this.lblReceiveQty.Location = new System.Drawing.Point(20, 102);
            this.lblReceiveQty.Name = "lblReceiveQty";
            this.lblReceiveQty.TabIndex = 4;
            this.lblReceiveQty.Text = "Receive Qty";
            //
            // tbReceiveQty
            //
            this.tbReceiveQty.Location = new System.Drawing.Point(160, 99);
            this.tbReceiveQty.Name = "tbReceiveQty";
            this.tbReceiveQty.Size = new System.Drawing.Size(220, 28);
            this.tbReceiveQty.TabIndex = 5;
            //
            // lblIssuedQty
            //
            this.lblIssuedQty.AutoSize = true;
            this.lblIssuedQty.Location = new System.Drawing.Point(20, 142);
            this.lblIssuedQty.Name = "lblIssuedQty";
            this.lblIssuedQty.TabIndex = 6;
            this.lblIssuedQty.Text = "Issued Qty";
            //
            // tbIssuedQty
            //
            this.tbIssuedQty.Location = new System.Drawing.Point(160, 139);
            this.tbIssuedQty.Name = "tbIssuedQty";
            this.tbIssuedQty.Size = new System.Drawing.Size(220, 28);
            this.tbIssuedQty.TabIndex = 7;
            //
            // lblRemark
            //
            this.lblRemark.AutoSize = true;
            this.lblRemark.Location = new System.Drawing.Point(20, 182);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.TabIndex = 8;
            this.lblRemark.Text = "Remark";
            //
            // tbRemark
            //
            this.tbRemark.Location = new System.Drawing.Point(160, 179);
            this.tbRemark.Name = "tbRemark";
            this.tbRemark.Size = new System.Drawing.Size(220, 28);
            this.tbRemark.TabIndex = 9;
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(160, 230);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(280, 230);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // TransactionConnectorEditForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 280);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.tbId);
            this.Controls.Add(this.lblItemCode);
            this.Controls.Add(this.tbItemCode);
            this.Controls.Add(this.lblReceiveQty);
            this.Controls.Add(this.tbReceiveQty);
            this.Controls.Add(this.lblIssuedQty);
            this.Controls.Add(this.tbIssuedQty);
            this.Controls.Add(this.lblRemark);
            this.Controls.Add(this.tbRemark);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Khmer OS Battambang", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TransactionConnectorEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Transaction Connector";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblItemCode;
        private System.Windows.Forms.Label lblReceiveQty;
        private System.Windows.Forms.Label lblIssuedQty;
        private System.Windows.Forms.Label lblRemark;
        private System.Windows.Forms.TextBox tbId;
        private System.Windows.Forms.TextBox tbItemCode;
        private System.Windows.Forms.TextBox tbReceiveQty;
        private System.Windows.Forms.TextBox tbIssuedQty;
        private System.Windows.Forms.TextBox tbRemark;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
