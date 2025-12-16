namespace MachineDeptApp.SparePartControll
{
    partial class DoSummary
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DoSummary));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgvDo = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbAssyCap = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbS3Ncap = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpShipmentEnd = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpShipmentStart = new System.Windows.Forms.DateTimePicker();
            this.cbShipement = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDo)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(961, 64);
            this.panel1.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearch.BackgroundImage")));
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.Location = new System.Drawing.Point(13, 2);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(58, 58);
            this.btnSearch.TabIndex = 30;
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 506);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(961, 10);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dgvDo);
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 64);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(961, 442);
            this.panel3.TabIndex = 2;
            // 
            // dgvDo
            // 
            this.dgvDo.AllowUserToAddRows = false;
            this.dgvDo.AllowUserToDeleteRows = false;
            this.dgvDo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDo.Location = new System.Drawing.Point(0, 70);
            this.dgvDo.Name = "dgvDo";
            this.dgvDo.ReadOnly = true;
            this.dgvDo.RowHeadersVisible = false;
            this.dgvDo.RowTemplate.Height = 50;
            this.dgvDo.Size = new System.Drawing.Size(961, 372);
            this.dgvDo.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbAssyCap);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbS3Ncap);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.dtpShipmentEnd);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtpShipmentStart);
            this.groupBox1.Controls.Add(this.cbShipement);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(961, 70);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Condition";
            // 
            // tbAssyCap
            // 
            this.tbAssyCap.Location = new System.Drawing.Point(697, 24);
            this.tbAssyCap.Name = "tbAssyCap";
            this.tbAssyCap.Size = new System.Drawing.Size(125, 28);
            this.tbAssyCap.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(629, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 19);
            this.label3.TabIndex = 6;
            this.label3.Text = "Assy.Cap";
            // 
            // tbS3Ncap
            // 
            this.tbS3Ncap.Location = new System.Drawing.Point(477, 23);
            this.tbS3Ncap.Name = "tbS3Ncap";
            this.tbS3Ncap.Size = new System.Drawing.Size(125, 28);
            this.tbS3Ncap.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(416, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 19);
            this.label2.TabIndex = 4;
            this.label2.Text = "S3N.Cap";
            // 
            // dtpShipmentEnd
            // 
            this.dtpShipmentEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpShipmentEnd.Location = new System.Drawing.Point(283, 23);
            this.dtpShipmentEnd.Name = "dtpShipmentEnd";
            this.dtpShipmentEnd.Size = new System.Drawing.Size(103, 28);
            this.dtpShipmentEnd.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(264, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "-";
            // 
            // dtpShipmentStart
            // 
            this.dtpShipmentStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpShipmentStart.Location = new System.Drawing.Point(155, 23);
            this.dtpShipmentStart.Name = "dtpShipmentStart";
            this.dtpShipmentStart.Size = new System.Drawing.Size(103, 28);
            this.dtpShipmentStart.TabIndex = 1;
            // 
            // cbShipement
            // 
            this.cbShipement.AutoSize = true;
            this.cbShipement.Location = new System.Drawing.Point(40, 29);
            this.cbShipement.Margin = new System.Windows.Forms.Padding(4);
            this.cbShipement.Name = "cbShipement";
            this.cbShipement.Size = new System.Drawing.Size(108, 23);
            this.cbShipement.TabIndex = 0;
            this.cbShipement.Text = "Shipment Date";
            this.cbShipement.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // DoSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 516);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Khmer OS Battambang", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DoSummary";
            this.Text = "DoSummary";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDo)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbShipement;
        private System.Windows.Forms.DateTimePicker dtpShipmentStart;
        private System.Windows.Forms.DataGridView dgvDo;
        private System.Windows.Forms.DateTimePicker dtpShipmentEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbAssyCap;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbS3Ncap;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}