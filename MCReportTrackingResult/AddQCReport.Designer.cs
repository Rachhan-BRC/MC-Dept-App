namespace MachineDeptApp.MCReportTrackingResult
{
    partial class AddQCReport
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
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblMonth = new System.Windows.Forms.Label();
            this.lblQty = new System.Windows.Forms.Label();
            this.tbQty = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // panel1
            //
            this.panel1.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.panel1.Controls.Add(this.lblCategory);
            this.panel1.Controls.Add(this.lblType);
            this.panel1.Controls.Add(this.lblMonth);
            this.panel1.Controls.Add(this.lblQty);
            this.panel1.Controls.Add(this.tbQty);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(320, 220);
            this.panel1.TabIndex = 0;
            //
            // lblCategory
            //
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Khmer OS Battambang", 9F);
            this.lblCategory.Location = new System.Drawing.Point(16, 18);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(80, 19);
            this.lblCategory.TabIndex = 0;
            this.lblCategory.Text = "Category :";
            //
            // lblType
            //
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Khmer OS Battambang", 9F);
            this.lblType.Location = new System.Drawing.Point(16, 48);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(60, 19);
            this.lblType.TabIndex = 1;
            this.lblType.Text = "Type :";
            //
            // lblMonth
            //
            this.lblMonth.AutoSize = true;
            this.lblMonth.Font = new System.Drawing.Font("Khmer OS Battambang", 9F);
            this.lblMonth.Location = new System.Drawing.Point(16, 78);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(70, 19);
            this.lblMonth.TabIndex = 2;
            this.lblMonth.Text = "Month :";
            //
            // lblQty
            //
            this.lblQty.AutoSize = true;
            this.lblQty.Font = new System.Drawing.Font("Khmer OS Battambang", 9F);
            this.lblQty.Location = new System.Drawing.Point(16, 115);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(38, 19);
            this.lblQty.TabIndex = 3;
            this.lblQty.Text = "Qty :";
            //
            // tbQty
            //
            this.tbQty.Font = new System.Drawing.Font("Khmer OS Battambang", 9F);
            this.tbQty.Location = new System.Drawing.Point(100, 112);
            this.tbQty.Name = "tbQty";
            this.tbQty.Size = new System.Drawing.Size(200, 26);
            this.tbQty.TabIndex = 4;
            //
            // btnSave
            //
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(70, 130, 180);
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(100, 158);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 32);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            //
            // btnCancel
            //
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Khmer OS Battambang", 9F);
            this.btnCancel.Location = new System.Drawing.Point(204, 158);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 32);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            //
            // AddQCReport
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 220);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Khmer OS Battambang", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddQCReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Insert QC Data";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.TextBox tbQty;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
    }
}
