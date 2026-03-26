namespace MachineDeptApp.RMConnector
{
    partial class RMConnectorAddForm
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
            this.lblRMCode        = new System.Windows.Forms.Label();
            this.lblRMDescription = new System.Windows.Forms.Label();
            this.lblType          = new System.Windows.Forms.Label();
            this.lblLocation      = new System.Windows.Forms.Label();
            this.tbRMCode         = new System.Windows.Forms.TextBox();
            this.tbRMDescription  = new System.Windows.Forms.TextBox();
            this.tbType           = new System.Windows.Forms.TextBox();
            this.tbLocation       = new System.Windows.Forms.TextBox();
            this.btnSave          = new System.Windows.Forms.Button();
            this.btnCancel        = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblRMCode
            //
            this.lblRMCode.AutoSize = true;
            this.lblRMCode.Location = new System.Drawing.Point(20, 22);
            this.lblRMCode.Name = "lblRMCode";
            this.lblRMCode.Size = new System.Drawing.Size(68, 19);
            this.lblRMCode.TabIndex = 0;
            this.lblRMCode.Text = "RM Code";
            //
            // tbRMCode
            //
            this.tbRMCode.Location = new System.Drawing.Point(160, 19);
            this.tbRMCode.Name = "tbRMCode";
            this.tbRMCode.Size = new System.Drawing.Size(220, 28);
            this.tbRMCode.TabIndex = 1;
            //
            // lblRMDescription
            //
            this.lblRMDescription.AutoSize = true;
            this.lblRMDescription.Location = new System.Drawing.Point(20, 62);
            this.lblRMDescription.Name = "lblRMDescription";
            this.lblRMDescription.Size = new System.Drawing.Size(110, 19);
            this.lblRMDescription.TabIndex = 2;
            this.lblRMDescription.Text = "RM Description";
            //
            // tbRMDescription
            //
            this.tbRMDescription.Location = new System.Drawing.Point(160, 59);
            this.tbRMDescription.Name = "tbRMDescription";
            this.tbRMDescription.Size = new System.Drawing.Size(220, 28);
            this.tbRMDescription.TabIndex = 3;
            //
            // lblType
            //
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(20, 102);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(36, 19);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "Type";
            //
            // tbType
            //
            this.tbType.Location = new System.Drawing.Point(160, 99);
            this.tbType.Name = "tbType";
            this.tbType.Size = new System.Drawing.Size(220, 28);
            this.tbType.TabIndex = 5;
            //
            // lblLocation
            //
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(20, 142);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(60, 19);
            this.lblLocation.TabIndex = 6;
            this.lblLocation.Text = "Location";
            //
            // tbLocation
            //
            this.tbLocation.Location = new System.Drawing.Point(160, 139);
            this.tbLocation.Name = "tbLocation";
            this.tbLocation.Size = new System.Drawing.Size(220, 28);
            this.tbLocation.TabIndex = 7;
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(160, 190);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(280, 190);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // RMConnectorAddForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 245);
            this.Controls.Add(this.lblRMCode);
            this.Controls.Add(this.tbRMCode);
            this.Controls.Add(this.lblRMDescription);
            this.Controls.Add(this.tbRMDescription);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.tbType);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.tbLocation);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Khmer OS Battambang", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RMConnectorAddForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add RM Connector";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblRMCode;
        private System.Windows.Forms.Label lblRMDescription;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.TextBox tbRMCode;
        private System.Windows.Forms.TextBox tbRMDescription;
        private System.Windows.Forms.TextBox tbType;
        private System.Windows.Forms.TextBox tbLocation;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
