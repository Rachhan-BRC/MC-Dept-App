namespace MachineDeptApp
{
    partial class LabelSemiPrint
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LabelSemiPrint));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle44 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle37 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle38 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle39 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle40 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle41 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle42 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle43 = new System.Windows.Forms.DataGridViewCellStyle();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtScan = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkall = new System.Windows.Forms.CheckBox();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.poscno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subpartno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wipcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.posqty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wirecolor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.batchqty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deldate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.toolStrip);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(931, 90);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(254, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Scan Barcode POS";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Khmer OS Bokor", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(233, -1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 61);
            this.label1.TabIndex = 1;
            this.label1.Text = "ស្កេនបារកូដ POS";
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnPrint});
            this.toolStrip.Location = new System.Drawing.Point(11, 15);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(103, 53);
            this.toolStrip.TabIndex = 0;
            // 
            // btnNew
            // 
            this.btnNew.AutoSize = false;
            this.btnNew.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNew.BackgroundImage")));
            this.btnNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(50, 50);
            this.btnNew.Text = "toolStripButton1";
            // 
            // btnPrint
            // 
            this.btnPrint.AutoSize = false;
            this.btnPrint.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPrint.BackgroundImage")));
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(50, 50);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtScan);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 90);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(931, 79);
            this.panel2.TabIndex = 1;
            // 
            // txtScan
            // 
            this.txtScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.txtScan.Font = new System.Drawing.Font("Kh Battambang", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtScan.Location = new System.Drawing.Point(138, 15);
            this.txtScan.Name = "txtScan";
            this.txtScan.Size = new System.Drawing.Size(398, 47);
            this.txtScan.TabIndex = 0;
            this.txtScan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkall);
            this.groupBox1.Controls.Add(this.dgvList);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 169);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(931, 281);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data List";
            // 
            // chkall
            // 
            this.chkall.AutoSize = true;
            this.chkall.BackColor = System.Drawing.Color.LightGreen;
            this.chkall.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkall.Location = new System.Drawing.Point(12, 25);
            this.chkall.Name = "chkall";
            this.chkall.Size = new System.Drawing.Size(15, 14);
            this.chkall.TabIndex = 1;
            this.chkall.UseVisualStyleBackColor = false;
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToResizeRows = false;
            dataGridViewCellStyle34.BackColor = System.Drawing.Color.Lavender;
            this.dgvList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle34;
            dataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle35.Font = new System.Drawing.Font("Kh Battambang", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle35.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle35.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle35.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle35.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle35;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chk,
            this.poscno,
            this.subpartno,
            this.wipcode,
            this.posqty,
            this.wirecolor,
            this.length,
            this.batchqty,
            this.deldate});
            dataGridViewCellStyle44.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle44.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle44.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle44.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle44.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle44.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle44.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvList.DefaultCellStyle = dataGridViewCellStyle44;
            this.dgvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvList.EnableHeadersVisualStyles = false;
            this.dgvList.Location = new System.Drawing.Point(3, 16);
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.Size = new System.Drawing.Size(925, 262);
            this.dgvList.TabIndex = 0;
            // 
            // chk
            // 
            this.chk.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.chk.HeaderText = "";
            this.chk.Name = "chk";
            this.chk.ReadOnly = true;
            this.chk.Width = 30;
            // 
            // poscno
            // 
            this.poscno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle36.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.poscno.DefaultCellStyle = dataGridViewCellStyle36;
            this.poscno.HeaderText = "POS No.";
            this.poscno.Name = "poscno";
            this.poscno.ReadOnly = true;
            this.poscno.Width = 130;
            // 
            // subpartno
            // 
            this.subpartno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle37.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.subpartno.DefaultCellStyle = dataGridViewCellStyle37;
            this.subpartno.HeaderText = "Sub Part No.";
            this.subpartno.Name = "subpartno";
            this.subpartno.ReadOnly = true;
            this.subpartno.Width = 130;
            // 
            // wipcode
            // 
            this.wipcode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle38.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.wipcode.DefaultCellStyle = dataGridViewCellStyle38;
            this.wipcode.HeaderText = "Wip Code";
            this.wipcode.Name = "wipcode";
            this.wipcode.ReadOnly = true;
            // 
            // posqty
            // 
            this.posqty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle39.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.posqty.DefaultCellStyle = dataGridViewCellStyle39;
            this.posqty.HeaderText = "POS Qty";
            this.posqty.Name = "posqty";
            this.posqty.ReadOnly = true;
            // 
            // wirecolor
            // 
            this.wirecolor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle40.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.wirecolor.DefaultCellStyle = dataGridViewCellStyle40;
            this.wirecolor.HeaderText = "Wire Color";
            this.wirecolor.Name = "wirecolor";
            this.wirecolor.ReadOnly = true;
            this.wirecolor.Width = 130;
            // 
            // length
            // 
            this.length.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle41.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.length.DefaultCellStyle = dataGridViewCellStyle41;
            this.length.HeaderText = "Length";
            this.length.Name = "length";
            this.length.ReadOnly = true;
            // 
            // batchqty
            // 
            this.batchqty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle42.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.batchqty.DefaultCellStyle = dataGridViewCellStyle42;
            this.batchqty.HeaderText = "Batch / Qty";
            this.batchqty.Name = "batchqty";
            this.batchqty.ReadOnly = true;
            this.batchqty.Width = 120;
            // 
            // deldate
            // 
            this.deldate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle43.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle43.Format = "dd-MM-yyyy";
            this.deldate.DefaultCellStyle = dataGridViewCellStyle43;
            this.deldate.HeaderText = "Delivery Date";
            this.deldate.Name = "deldate";
            this.deldate.ReadOnly = true;
            this.deldate.Width = 130;
            // 
            // LabelSemiPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(931, 450);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "LabelSemiPrint";
            this.Text = "ព្រីន Semi Label";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtScan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.CheckBox chkall;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn poscno;
        private System.Windows.Forms.DataGridViewTextBoxColumn subpartno;
        private System.Windows.Forms.DataGridViewTextBoxColumn wipcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn posqty;
        private System.Windows.Forms.DataGridViewTextBoxColumn wirecolor;
        private System.Windows.Forms.DataGridViewTextBoxColumn length;
        private System.Windows.Forms.DataGridViewTextBoxColumn batchqty;
        private System.Windows.Forms.DataGridViewTextBoxColumn deldate;
    }
}