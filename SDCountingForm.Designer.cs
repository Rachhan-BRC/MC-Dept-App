namespace MachineDeptApp
{
    partial class MCStock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCStock));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnDeleteGray = new System.Windows.Forms.PictureBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtrmname = new System.Windows.Forms.TextBox();
            this.chkrmname = new System.Windows.Forms.CheckBox();
            this.txtcode = new System.Windows.Forms.TextBox();
            this.chkcode = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lberror = new System.Windows.Forms.Label();
            this.lbFound = new System.Windows.Forms.Label();
            this.LbStatus = new System.Windows.Forms.Label();
            this.dgvTTL = new System.Windows.Forms.DataGridView();
            this.sysno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rmname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ttlw = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bbw = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bbqty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pic = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteGray)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTTL)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelHeader.Controls.Add(this.btnDeleteGray);
            this.panelHeader.Controls.Add(this.btnDelete);
            this.panelHeader.Controls.Add(this.btnSearch);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(984, 68);
            this.panelHeader.TabIndex = 59;
            // 
            // btnDeleteGray
            // 
            this.btnDeleteGray.Enabled = false;
            this.btnDeleteGray.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteGray.Image")));
            this.btnDeleteGray.Location = new System.Drawing.Point(76, 7);
            this.btnDeleteGray.Name = "btnDeleteGray";
            this.btnDeleteGray.Size = new System.Drawing.Size(47, 55);
            this.btnDeleteGray.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnDeleteGray.TabIndex = 33;
            this.btnDeleteGray.TabStop = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.Location = new System.Drawing.Point(76, 5);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(46, 56);
            this.btnDelete.TabIndex = 32;
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearch.BackgroundImage")));
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.Location = new System.Drawing.Point(12, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(53, 56);
            this.btnSearch.TabIndex = 29;
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Controls.Add(this.txtrmname);
            this.groupBox1.Controls.Add(this.chkrmname);
            this.groupBox1.Controls.Add(this.txtcode);
            this.groupBox1.Controls.Add(this.chkcode);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(984, 123);
            this.groupBox1.TabIndex = 60;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Condition";
            // 
            // txtrmname
            // 
            this.txtrmname.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtrmname.Location = new System.Drawing.Point(140, 74);
            this.txtrmname.Name = "txtrmname";
            this.txtrmname.Size = new System.Drawing.Size(101, 30);
            this.txtrmname.TabIndex = 126;
            // 
            // chkrmname
            // 
            this.chkrmname.AutoSize = true;
            this.chkrmname.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkrmname.Location = new System.Drawing.Point(31, 76);
            this.chkrmname.Name = "chkrmname";
            this.chkrmname.Size = new System.Drawing.Size(85, 26);
            this.chkrmname.TabIndex = 123;
            this.chkrmname.Text = "RM Name";
            this.chkrmname.UseVisualStyleBackColor = true;
            // 
            // txtcode
            // 
            this.txtcode.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcode.Location = new System.Drawing.Point(140, 29);
            this.txtcode.Name = "txtcode";
            this.txtcode.Size = new System.Drawing.Size(101, 30);
            this.txtcode.TabIndex = 128;
            // 
            // chkcode
            // 
            this.chkcode.AutoSize = true;
            this.chkcode.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkcode.Location = new System.Drawing.Point(31, 31);
            this.chkcode.Name = "chkcode";
            this.chkcode.Size = new System.Drawing.Size(58, 26);
            this.chkcode.TabIndex = 125;
            this.chkcode.Text = "Code";
            this.chkcode.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.lberror);
            this.panel2.Controls.Add(this.lbFound);
            this.panel2.Controls.Add(this.LbStatus);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 518);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(984, 43);
            this.panel2.TabIndex = 63;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(855, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 24);
            this.label3.TabIndex = 4;
            // 
            // lberror
            // 
            this.lberror.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lberror.AutoSize = true;
            this.lberror.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lberror.Location = new System.Drawing.Point(-838, 10);
            this.lberror.Name = "lberror";
            this.lberror.Size = new System.Drawing.Size(0, 24);
            this.lberror.TabIndex = 3;
            // 
            // lbFound
            // 
            this.lbFound.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbFound.AutoSize = true;
            this.lbFound.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFound.Location = new System.Drawing.Point(849, 10);
            this.lbFound.Name = "lbFound";
            this.lbFound.Size = new System.Drawing.Size(0, 24);
            this.lbFound.TabIndex = 2;
            // 
            // LbStatus
            // 
            this.LbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LbStatus.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbStatus.Location = new System.Drawing.Point(366, 4);
            this.LbStatus.Name = "LbStatus";
            this.LbStatus.Size = new System.Drawing.Size(614, 22);
            this.LbStatus.TabIndex = 1;
            this.LbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvTTL
            // 
            this.dgvTTL.AllowUserToAddRows = false;
            this.dgvTTL.AllowUserToDeleteRows = false;
            this.dgvTTL.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvTTL.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTTL.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTTL.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTTL.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTTL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTTL.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sysno,
            this.Code,
            this.rmname,
            this.ttlw,
            this.bbw,
            this.qty,
            this.bbqty,
            this.labelno,
            this.pic});
            this.dgvTTL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTTL.EnableHeadersVisualStyles = false;
            this.dgvTTL.Location = new System.Drawing.Point(0, 191);
            this.dgvTTL.Name = "dgvTTL";
            this.dgvTTL.ReadOnly = true;
            this.dgvTTL.RowHeadersVisible = false;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.White;
            this.dgvTTL.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvTTL.RowTemplate.Height = 25;
            this.dgvTTL.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvTTL.Size = new System.Drawing.Size(984, 327);
            this.dgvTTL.TabIndex = 64;
            // 
            // sysno
            // 
            this.sysno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.sysno.DefaultCellStyle = dataGridViewCellStyle3;
            this.sysno.HeaderText = "SysNo";
            this.sysno.Name = "sysno";
            this.sysno.ReadOnly = true;
            this.sysno.Width = 120;
            // 
            // Code
            // 
            this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Code.DefaultCellStyle = dataGridViewCellStyle4;
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            // 
            // rmname
            // 
            this.rmname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.rmname.DefaultCellStyle = dataGridViewCellStyle5;
            this.rmname.HeaderText = "RM Name";
            this.rmname.Name = "rmname";
            this.rmname.ReadOnly = true;
            this.rmname.Width = 200;
            // 
            // ttlw
            // 
            this.ttlw.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            this.ttlw.DefaultCellStyle = dataGridViewCellStyle6;
            this.ttlw.HeaderText = "Total Weight";
            this.ttlw.Name = "ttlw";
            this.ttlw.ReadOnly = true;
            this.ttlw.Width = 120;
            // 
            // bbw
            // 
            this.bbw.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N0";
            this.bbw.DefaultCellStyle = dataGridViewCellStyle7;
            this.bbw.HeaderText = "Bobin Weight";
            this.bbw.Name = "bbw";
            this.bbw.ReadOnly = true;
            this.bbw.Width = 120;
            // 
            // qty
            // 
            this.qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.qty.HeaderText = "Qty";
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            this.qty.Width = 120;
            // 
            // bbqty
            // 
            this.bbqty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.bbqty.HeaderText = "Bobin Qty";
            this.bbqty.Name = "bbqty";
            this.bbqty.ReadOnly = true;
            this.bbqty.Width = 120;
            // 
            // labelno
            // 
            this.labelno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.labelno.HeaderText = "Label No";
            this.labelno.Name = "labelno";
            this.labelno.ReadOnly = true;
            // 
            // pic
            // 
            this.pic.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.pic.HeaderText = "PIC";
            this.pic.Name = "pic";
            this.pic.ReadOnly = true;
            this.pic.Width = 120;
            // 
            // MCStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.dgvTTL);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panelHeader);
            this.Name = "MCStock";
            this.Text = "MC Stock";
            this.panelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteGray)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTTL)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.PictureBox btnDeleteGray;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtrmname;
        private System.Windows.Forms.CheckBox chkrmname;
        private System.Windows.Forms.TextBox txtcode;
        private System.Windows.Forms.CheckBox chkcode;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lberror;
        private System.Windows.Forms.Label lbFound;
        private System.Windows.Forms.Label LbStatus;
        public System.Windows.Forms.DataGridView dgvTTL;
        private System.Windows.Forms.DataGridViewTextBoxColumn sysno;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn rmname;
        private System.Windows.Forms.DataGridViewTextBoxColumn ttlw;
        private System.Windows.Forms.DataGridViewTextBoxColumn bbw;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn bbqty;
        private System.Windows.Forms.DataGridViewTextBoxColumn labelno;
        private System.Windows.Forms.DataGridViewTextBoxColumn pic;
    }
}