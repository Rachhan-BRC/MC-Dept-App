namespace MachineDeptApp
{
    partial class InvoiceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InvoiceForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInvoice = new System.Windows.Forms.TextBox();
            this.btnSaveGrey = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lberror = new System.Windows.Forms.Label();
            this.lbfound = new System.Windows.Forms.Label();
            this.lbshow = new System.Windows.Forms.Label();
            this.dgvInvoice = new System.Windows.Forms.DataGridView();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pono = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceusd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountusd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoiceno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoicedate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelHeader.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoice)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelHeader.Controls.Add(this.label1);
            this.panelHeader.Controls.Add(this.txtInvoice);
            this.panelHeader.Controls.Add(this.btnSaveGrey);
            this.panelHeader.Controls.Add(this.btnSave);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1015, 67);
            this.panelHeader.TabIndex = 53;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(113, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 24);
            this.label1.TabIndex = 35;
            this.label1.Text = "Invoice No.";
            // 
            // txtInvoice
            // 
            this.txtInvoice.Font = new System.Drawing.Font("Khmer OS Battambang", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInvoice.Location = new System.Drawing.Point(195, 16);
            this.txtInvoice.Name = "txtInvoice";
            this.txtInvoice.Size = new System.Drawing.Size(135, 37);
            this.txtInvoice.TabIndex = 34;
            this.txtInvoice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnSaveGrey
            // 
            this.btnSaveGrey.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSaveGrey.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveGrey.BackgroundImage")));
            this.btnSaveGrey.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSaveGrey.Enabled = false;
            this.btnSaveGrey.Location = new System.Drawing.Point(12, 6);
            this.btnSaveGrey.Name = "btnSaveGrey";
            this.btnSaveGrey.Size = new System.Drawing.Size(53, 56);
            this.btnSaveGrey.TabIndex = 32;
            this.btnSaveGrey.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.Location = new System.Drawing.Point(12, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(53, 56);
            this.btnSave.TabIndex = 33;
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lberror);
            this.panel2.Controls.Add(this.lbfound);
            this.panel2.Controls.Add(this.lbshow);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 518);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1015, 43);
            this.panel2.TabIndex = 58;
            // 
            // lberror
            // 
            this.lberror.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lberror.AutoSize = true;
            this.lberror.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lberror.Location = new System.Drawing.Point(-807, 10);
            this.lberror.Name = "lberror";
            this.lberror.Size = new System.Drawing.Size(0, 24);
            this.lberror.TabIndex = 3;
            // 
            // lbfound
            // 
            this.lbfound.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbfound.AutoSize = true;
            this.lbfound.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbfound.Location = new System.Drawing.Point(880, 10);
            this.lbfound.Name = "lbfound";
            this.lbfound.Size = new System.Drawing.Size(0, 24);
            this.lbfound.TabIndex = 2;
            // 
            // lbshow
            // 
            this.lbshow.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbshow.Location = new System.Drawing.Point(8, 12);
            this.lbshow.Name = "lbshow";
            this.lbshow.Size = new System.Drawing.Size(456, 22);
            this.lbshow.TabIndex = 1;
            this.lbshow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvInvoice
            // 
            this.dgvInvoice.AllowUserToAddRows = false;
            this.dgvInvoice.AllowUserToDeleteRows = false;
            this.dgvInvoice.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvInvoice.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvInvoice.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInvoice.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvInvoice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvInvoice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInvoice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.description,
            this.code,
            this.Pno,
            this.partname,
            this.pono,
            this.qty,
            this.priceusd,
            this.amountusd,
            this.invoiceno,
            this.invoicedate});
            this.dgvInvoice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInvoice.EnableHeadersVisualStyles = false;
            this.dgvInvoice.Location = new System.Drawing.Point(0, 67);
            this.dgvInvoice.Name = "dgvInvoice";
            this.dgvInvoice.ReadOnly = true;
            this.dgvInvoice.RowHeadersVisible = false;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.White;
            this.dgvInvoice.RowsDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvInvoice.RowTemplate.Height = 25;
            this.dgvInvoice.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvInvoice.Size = new System.Drawing.Size(1015, 451);
            this.dgvInvoice.TabIndex = 63;
            // 
            // description
            // 
            this.description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.description.HeaderText = "Description";
            this.description.Name = "description";
            this.description.ReadOnly = true;
            this.description.Width = 150;
            // 
            // code
            // 
            this.code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.code.DefaultCellStyle = dataGridViewCellStyle3;
            this.code.HeaderText = "Code";
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // Pno
            // 
            this.Pno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Pno.DefaultCellStyle = dataGridViewCellStyle4;
            this.Pno.HeaderText = "Part No.";
            this.Pno.Name = "Pno";
            this.Pno.ReadOnly = true;
            this.Pno.Width = 150;
            // 
            // partname
            // 
            this.partname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.partname.DefaultCellStyle = dataGridViewCellStyle5;
            this.partname.HeaderText = "Part Name ";
            this.partname.Name = "partname";
            this.partname.ReadOnly = true;
            this.partname.Width = 200;
            // 
            // pono
            // 
            this.pono.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.pono.DefaultCellStyle = dataGridViewCellStyle6;
            this.pono.HeaderText = "PO No.";
            this.pono.Name = "pono";
            this.pono.ReadOnly = true;
            // 
            // qty
            // 
            this.qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N4";
            this.qty.DefaultCellStyle = dataGridViewCellStyle7;
            this.qty.HeaderText = "Qty";
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            this.qty.Width = 120;
            // 
            // priceusd
            // 
            this.priceusd.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.priceusd.DefaultCellStyle = dataGridViewCellStyle8;
            this.priceusd.HeaderText = "Price(USD)";
            this.priceusd.Name = "priceusd";
            this.priceusd.ReadOnly = true;
            // 
            // amountusd
            // 
            this.amountusd.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N0";
            dataGridViewCellStyle9.NullValue = null;
            this.amountusd.DefaultCellStyle = dataGridViewCellStyle9;
            this.amountusd.HeaderText = "Amount(USD)";
            this.amountusd.Name = "amountusd";
            this.amountusd.ReadOnly = true;
            this.amountusd.Width = 120;
            // 
            // invoiceno
            // 
            this.invoiceno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.invoiceno.HeaderText = "Invoice No";
            this.invoiceno.Name = "invoiceno";
            this.invoiceno.ReadOnly = true;
            this.invoiceno.Width = 120;
            // 
            // invoicedate
            // 
            this.invoicedate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.invoicedate.DefaultCellStyle = dataGridViewCellStyle10;
            this.invoicedate.HeaderText = "Invoice Date";
            this.invoicedate.Name = "invoicedate";
            this.invoicedate.ReadOnly = true;
            this.invoicedate.Width = 150;
            // 
            // InvoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1015, 561);
            this.Controls.Add(this.dgvInvoice);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelHeader);
            this.Name = "InvoiceForm";
            this.Text = "Invoice";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoice)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lberror;
        private System.Windows.Forms.Label lbfound;
        private System.Windows.Forms.Label lbshow;
        private System.Windows.Forms.Button btnSaveGrey;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInvoice;
        public System.Windows.Forms.DataGridView dgvInvoice;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pno;
        private System.Windows.Forms.DataGridViewTextBoxColumn partname;
        private System.Windows.Forms.DataGridViewTextBoxColumn pono;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceusd;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountusd;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoiceno;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoicedate;
    }
}