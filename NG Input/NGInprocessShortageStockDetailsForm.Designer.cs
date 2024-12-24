namespace MachineDeptApp.NG_Input
{
    partial class NGInprocessShortageStockDetailsForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NGInprocessShortageStockDetailsForm));
            this.dgvNGShortage = new System.Windows.Forms.DataGridView();
            this.POSNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RMCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RMName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NGQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNGShortage)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvNGShortage
            // 
            this.dgvNGShortage.AllowUserToAddRows = false;
            this.dgvNGShortage.AllowUserToDeleteRows = false;
            this.dgvNGShortage.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvNGShortage.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvNGShortage.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvNGShortage.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvNGShortage.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvNGShortage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNGShortage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.POSNo,
            this.RMCode,
            this.RMName,
            this.NGQty});
            this.dgvNGShortage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNGShortage.EnableHeadersVisualStyles = false;
            this.dgvNGShortage.Location = new System.Drawing.Point(1, 1);
            this.dgvNGShortage.Name = "dgvNGShortage";
            this.dgvNGShortage.ReadOnly = true;
            this.dgvNGShortage.RowHeadersVisible = false;
            this.dgvNGShortage.RowHeadersWidth = 60;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            this.dgvNGShortage.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvNGShortage.RowTemplate.Height = 25;
            this.dgvNGShortage.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvNGShortage.Size = new System.Drawing.Size(682, 309);
            this.dgvNGShortage.TabIndex = 12;
            // 
            // POSNo
            // 
            this.POSNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.POSNo.HeaderText = "លេខភីអូអេស";
            this.POSNo.Name = "POSNo";
            this.POSNo.ReadOnly = true;
            this.POSNo.Width = 120;
            // 
            // RMCode
            // 
            this.RMCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.RMCode.HeaderText = "លេខកូដ";
            this.RMCode.Name = "RMCode";
            this.RMCode.ReadOnly = true;
            this.RMCode.Width = 80;
            // 
            // RMName
            // 
            this.RMName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RMName.HeaderText = "ឈ្មោះវត្ថុធាតុដើម";
            this.RMName.Name = "RMName";
            this.RMName.ReadOnly = true;
            // 
            // NGQty
            // 
            this.NGQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N4";
            this.NGQty.DefaultCellStyle = dataGridViewCellStyle3;
            this.NGQty.HeaderText = "ចំនួន NG";
            this.NGQty.Name = "NGQty";
            this.NGQty.ReadOnly = true;
            // 
            // NGInprocessShortageStockDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 311);
            this.Controls.Add(this.dgvNGShortage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NGInprocessShortageStockDetailsForm";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shortage Stock Details";
            ((System.ComponentModel.ISupportInitialize)(this.dgvNGShortage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dgvNGShortage;
        private System.Windows.Forms.DataGridViewTextBoxColumn POSNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn RMCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn RMName;
        private System.Windows.Forms.DataGridViewTextBoxColumn NGQty;
    }
}