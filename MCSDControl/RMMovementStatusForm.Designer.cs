namespace MachineDeptApp.MCSDControl
{
    partial class RMMovementStatusForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RMMovementStatusForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.txtRMCode = new System.Windows.Forms.TextBox();
            this.txtRMName = new System.Windows.Forms.TextBox();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.LbStatus = new System.Windows.Forms.Label();
            this.panelBody = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DgvSearch = new System.Windows.Forms.DataGridView();
            this.colRMCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRMName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRMType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastMoveDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grbSCondition = new System.Windows.Forms.GroupBox();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.lbStatusCond = new System.Windows.Forms.Label();
            this.cboRMType = new System.Windows.Forms.ComboBox();
            this.lbRMType = new System.Windows.Forms.Label();
            this.lbRMName = new System.Windows.Forms.Label();
            this.lbRMCode = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvSearch)).BeginInit();
            this.grbSCondition.SuspendLayout();
            this.SuspendLayout();
            //
            // btnSearch
            //
            this.btnSearch.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearch.BackgroundImage")));
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.Location = new System.Drawing.Point(4, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(48, 51);
            this.btnSearch.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btnSearch, "ស្វែងរក");
            this.btnSearch.UseVisualStyleBackColor = false;
            //
            // btnExport
            //
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnExport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExport.BackgroundImage")));
            this.btnExport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExport.Location = new System.Drawing.Point(933, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(48, 51);
            this.btnExport.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnExport, "Export Excel");
            this.btnExport.UseVisualStyleBackColor = false;
            //
            // txtRMCode
            //
            this.txtRMCode.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.txtRMCode.Location = new System.Drawing.Point(75, 26);
            this.txtRMCode.Name = "txtRMCode";
            this.txtRMCode.Size = new System.Drawing.Size(115, 32);
            this.txtRMCode.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtRMCode, "អាចប្រើ * ជំនួសតួអក្សរណាមួយ");
            //
            // txtRMName
            //
            this.txtRMName.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.txtRMName.Location = new System.Drawing.Point(277, 26);
            this.txtRMName.Name = "txtRMName";
            this.txtRMName.Size = new System.Drawing.Size(200, 32);
            this.txtRMName.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtRMName, "អាចប្រើ * ជំនួសតួអក្សរណាមួយ");
            //
            // panelHeader
            //
            this.panelHeader.Controls.Add(this.btnSearch);
            this.panelHeader.Controls.Add(this.btnExport);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(984, 59);
            this.panelHeader.TabIndex = 7;
            //
            // panelFooter
            //
            this.panelFooter.BackColor = System.Drawing.SystemColors.Control;
            this.panelFooter.Controls.Add(this.LbStatus);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 477);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(984, 34);
            this.panelFooter.TabIndex = 8;
            //
            // LbStatus
            //
            this.LbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LbStatus.Font = new System.Drawing.Font("Khmer OS Battambang", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbStatus.Location = new System.Drawing.Point(204, 5);
            this.LbStatus.Name = "LbStatus";
            this.LbStatus.Size = new System.Drawing.Size(775, 26);
            this.LbStatus.TabIndex = 7;
            this.LbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // panelBody
            //
            this.panelBody.Controls.Add(this.groupBox2);
            this.panelBody.Controls.Add(this.grbSCondition);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 59);
            this.panelBody.Margin = new System.Windows.Forms.Padding(2);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(3, 0, 3, 2);
            this.panelBody.Size = new System.Drawing.Size(984, 418);
            this.panelBody.TabIndex = 9;
            //
            // groupBox2
            //
            this.groupBox2.Controls.Add(this.DgvSearch);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Kh Battambang", 7.8F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(3, 71);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(6, 0, 6, 7);
            this.groupBox2.Size = new System.Drawing.Size(978, 345);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "តារាងទិន្នន័យ";
            //
            // DgvSearch
            //
            this.DgvSearch.AllowUserToAddRows = false;
            this.DgvSearch.AllowUserToDeleteRows = false;
            this.DgvSearch.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Kh Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            this.DgvSearch.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Kh Battambang", 7.8F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvSearch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.DgvSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvSearch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRMCode,
            this.colRMName,
            this.colRMType,
            this.colStockQty,
            this.colLastMoveDate,
            this.colStatus});
            this.DgvSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvSearch.EnableHeadersVisualStyles = false;
            this.DgvSearch.Location = new System.Drawing.Point(6, 20);
            this.DgvSearch.Name = "DgvSearch";
            this.DgvSearch.ReadOnly = true;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Kh Battambang", 7.8F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvSearch.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.DgvSearch.RowHeadersWidth = 60;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Blue;
            this.DgvSearch.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.DgvSearch.RowTemplate.Height = 25;
            this.DgvSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvSearch.Size = new System.Drawing.Size(966, 318);
            this.DgvSearch.TabIndex = 0;
            //
            // colRMCode
            //
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colRMCode.DefaultCellStyle = dataGridViewCellStyle3;
            this.colRMCode.Frozen = true;
            this.colRMCode.HeaderText = "RM Code";
            this.colRMCode.Name = "colRMCode";
            this.colRMCode.ReadOnly = true;
            this.colRMCode.Width = 100;
            //
            // colRMName
            //
            this.colRMName.HeaderText = "RM Name";
            this.colRMName.Name = "colRMName";
            this.colRMName.ReadOnly = true;
            this.colRMName.Width = 280;
            //
            // colRMType
            //
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colRMType.DefaultCellStyle = dataGridViewCellStyle4;
            this.colRMType.HeaderText = "RM Type";
            this.colRMType.Name = "colRMType";
            this.colRMType.ReadOnly = true;
            this.colRMType.Width = 110;
            //
            // colStockQty
            //
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "#,##0.###";
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.colStockQty.DefaultCellStyle = dataGridViewCellStyle5;
            this.colStockQty.HeaderText = "Stock Qty";
            this.colStockQty.Name = "colStockQty";
            this.colStockQty.ReadOnly = true;
            this.colStockQty.Width = 120;
            //
            // colLastMoveDate
            //
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Format = "dd-MM-yyyy";
            this.colLastMoveDate.DefaultCellStyle = dataGridViewCellStyle6;
            this.colLastMoveDate.HeaderText = "Last Movement Date";
            this.colLastMoveDate.Name = "colLastMoveDate";
            this.colLastMoveDate.ReadOnly = true;
            this.colLastMoveDate.Width = 150;
            //
            // colStatus
            //
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colStatus.DefaultCellStyle = dataGridViewCellStyle7;
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 140;
            //
            // grbSCondition
            //
            this.grbSCondition.Controls.Add(this.cboStatus);
            this.grbSCondition.Controls.Add(this.lbStatusCond);
            this.grbSCondition.Controls.Add(this.cboRMType);
            this.grbSCondition.Controls.Add(this.lbRMType);
            this.grbSCondition.Controls.Add(this.txtRMName);
            this.grbSCondition.Controls.Add(this.lbRMName);
            this.grbSCondition.Controls.Add(this.txtRMCode);
            this.grbSCondition.Controls.Add(this.lbRMCode);
            this.grbSCondition.Dock = System.Windows.Forms.DockStyle.Top;
            this.grbSCondition.Font = new System.Drawing.Font("Kh Battambang", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbSCondition.Location = new System.Drawing.Point(3, 0);
            this.grbSCondition.Margin = new System.Windows.Forms.Padding(2);
            this.grbSCondition.Name = "grbSCondition";
            this.grbSCondition.Padding = new System.Windows.Forms.Padding(2);
            this.grbSCondition.Size = new System.Drawing.Size(978, 71);
            this.grbSCondition.TabIndex = 0;
            this.grbSCondition.TabStop = false;
            this.grbSCondition.Text = "លក្ខខណ្ឌស្វែងរក";
            //
            // cboStatus
            //
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Location = new System.Drawing.Point(757, 26);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(150, 32);
            this.cboStatus.TabIndex = 7;
            //
            // lbStatusCond
            //
            this.lbStatusCond.AutoSize = true;
            this.lbStatusCond.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.lbStatusCond.Location = new System.Drawing.Point(705, 30);
            this.lbStatusCond.Name = "lbStatusCond";
            this.lbStatusCond.Size = new System.Drawing.Size(48, 24);
            this.lbStatusCond.TabIndex = 6;
            this.lbStatusCond.Text = "Status";
            //
            // cboRMType
            //
            this.cboRMType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRMType.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.cboRMType.FormattingEnabled = true;
            this.cboRMType.Location = new System.Drawing.Point(557, 26);
            this.cboRMType.Name = "cboRMType";
            this.cboRMType.Size = new System.Drawing.Size(130, 32);
            this.cboRMType.TabIndex = 5;
            //
            // lbRMType
            //
            this.lbRMType.AutoSize = true;
            this.lbRMType.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.lbRMType.Location = new System.Drawing.Point(493, 30);
            this.lbRMType.Name = "lbRMType";
            this.lbRMType.Size = new System.Drawing.Size(61, 24);
            this.lbRMType.TabIndex = 4;
            this.lbRMType.Text = "RM Type";
            //
            // lbRMName
            //
            this.lbRMName.AutoSize = true;
            this.lbRMName.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.lbRMName.Location = new System.Drawing.Point(205, 30);
            this.lbRMName.Name = "lbRMName";
            this.lbRMName.Size = new System.Drawing.Size(68, 24);
            this.lbRMName.TabIndex = 2;
            this.lbRMName.Text = "RM Name";
            //
            // lbRMCode
            //
            this.lbRMCode.AutoSize = true;
            this.lbRMCode.Font = new System.Drawing.Font("Kh Battambang", 9.75F);
            this.lbRMCode.Location = new System.Drawing.Point(8, 30);
            this.lbRMCode.Name = "lbRMCode";
            this.lbRMCode.Size = new System.Drawing.Size(63, 24);
            this.lbRMCode.TabIndex = 0;
            this.lbRMCode.Text = "RM Code";
            //
            // RMMovementStatusForm
            //
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(984, 511);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RMMovementStatusForm";
            this.Text = "RM Movement Status";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelHeader.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvSearch)).EndInit();
            this.grbSCondition.ResumeLayout(false);
            this.grbSCondition.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Label LbStatus;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView DgvSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRMCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRMName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRMType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastMoveDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.GroupBox grbSCondition;
        private System.Windows.Forms.TextBox txtRMCode;
        private System.Windows.Forms.Label lbRMCode;
        private System.Windows.Forms.TextBox txtRMName;
        private System.Windows.Forms.Label lbRMName;
        private System.Windows.Forms.ComboBox cboRMType;
        private System.Windows.Forms.Label lbRMType;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Label lbStatusCond;
    }
}
