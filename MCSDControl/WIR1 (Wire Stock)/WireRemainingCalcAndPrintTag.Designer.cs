namespace MachineDeptApp.MCSDControl.WIR1__Wire_Stock_
{
    partial class WireRemainingCalcAndPrintTag
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WireRemainingCalcAndPrintTag));
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.LbStatus = new System.Windows.Forms.Label();
            this.panelBody = new System.Windows.Forms.Panel();
            this.GroupBoxResult = new System.Windows.Forms.GroupBox();
            this.dgvInput = new System.Windows.Forms.DataGridView();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dgvBobbinsW = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RMCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RMName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RMType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Maker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BobbinOrReel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MOQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BobbinW = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NetW = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TototalW = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RemainQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LotNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnPrintGRAY = new System.Windows.Forms.PictureBox();
            this.btnDeleteGRAY = new System.Windows.Forms.PictureBox();
            this.panelHeader.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.GroupBoxResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBobbinsW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPrintGRAY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteGRAY)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.btnDeleteGRAY);
            this.panelHeader.Controls.Add(this.btnPrintGRAY);
            this.panelHeader.Controls.Add(this.btnAdd);
            this.panelHeader.Controls.Add(this.btnPrint);
            this.panelHeader.Controls.Add(this.btnNew);
            this.panelHeader.Controls.Add(this.btnDelete);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1184, 58);
            this.panelHeader.TabIndex = 25;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
            this.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAdd.Location = new System.Drawing.Point(57, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(50, 50);
            this.btnAdd.TabIndex = 35;
            this.toolTip1.SetToolTip(this.btnAdd, "បន្ថែម");
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // btnPrint
            // 
            this.btnPrint.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPrint.BackgroundImage")));
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrint.Enabled = false;
            this.btnPrint.Location = new System.Drawing.Point(109, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(50, 50);
            this.btnPrint.TabIndex = 34;
            this.toolTip1.SetToolTip(this.btnPrint, "ព្រីន");
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnNew.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNew.BackgroundImage")));
            this.btnNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNew.Location = new System.Drawing.Point(5, 4);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(50, 50);
            this.btnNew.TabIndex = 33;
            this.toolTip1.SetToolTip(this.btnNew, "សារថ្មីម្ដងទៀត");
            this.btnNew.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(161, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(50, 50);
            this.btnDelete.TabIndex = 32;
            this.toolTip1.SetToolTip(this.btnDelete, "លុប");
            this.btnDelete.UseVisualStyleBackColor = false;
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelFooter.Controls.Add(this.LbStatus);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 536);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(1184, 25);
            this.panelFooter.TabIndex = 26;
            // 
            // LbStatus
            // 
            this.LbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LbStatus.Font = new System.Drawing.Font("Khmer OS Battambang", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbStatus.Location = new System.Drawing.Point(734, 4);
            this.LbStatus.Name = "LbStatus";
            this.LbStatus.Size = new System.Drawing.Size(445, 18);
            this.LbStatus.TabIndex = 0;
            this.LbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.GroupBoxResult);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 58);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.panelBody.Size = new System.Drawing.Size(1184, 478);
            this.panelBody.TabIndex = 27;
            // 
            // GroupBoxResult
            // 
            this.GroupBoxResult.Controls.Add(this.dgvInput);
            this.GroupBoxResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupBoxResult.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBoxResult.Location = new System.Drawing.Point(5, 0);
            this.GroupBoxResult.Name = "GroupBoxResult";
            this.GroupBoxResult.Size = new System.Drawing.Size(1174, 478);
            this.GroupBoxResult.TabIndex = 24;
            this.GroupBoxResult.TabStop = false;
            this.GroupBoxResult.Text = "ទិន្នន័យដែលបញ្ចូលរួច";
            // 
            // dgvInput
            // 
            this.dgvInput.AllowUserToAddRows = false;
            this.dgvInput.AllowUserToDeleteRows = false;
            this.dgvInput.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Khmer OS Battambang", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvInput.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvInput.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInput.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvInput.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvInput.ColumnHeadersHeight = 70;
            this.dgvInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvInput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RMCode,
            this.RMName,
            this.RMType,
            this.Maker,
            this.BobbinOrReel,
            this.MOQ,
            this.BobbinW,
            this.NetW,
            this.TototalW,
            this.RemainQty,
            this.LotNo});
            this.dgvInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInput.EnableHeadersVisualStyles = false;
            this.dgvInput.Location = new System.Drawing.Point(3, 26);
            this.dgvInput.MultiSelect = false;
            this.dgvInput.Name = "dgvInput";
            this.dgvInput.RowHeadersWidth = 60;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Khmer OS Battambang", 9F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White;
            this.dgvInput.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvInput.RowTemplate.Height = 25;
            this.dgvInput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvInput.Size = new System.Drawing.Size(1168, 449);
            this.dgvInput.TabIndex = 11;
            // 
            // dgvBobbinsW
            // 
            this.dgvBobbinsW.AllowUserToAddRows = false;
            this.dgvBobbinsW.AllowUserToDeleteRows = false;
            this.dgvBobbinsW.AllowUserToResizeColumns = false;
            this.dgvBobbinsW.AllowUserToResizeRows = false;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBobbinsW.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvBobbinsW.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBobbinsW.ColumnHeadersVisible = false;
            this.dgvBobbinsW.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2});
            this.dgvBobbinsW.Location = new System.Drawing.Point(531, 213);
            this.dgvBobbinsW.Name = "dgvBobbinsW";
            this.dgvBobbinsW.ReadOnly = true;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBobbinsW.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvBobbinsW.RowHeadersVisible = false;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvBobbinsW.RowsDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvBobbinsW.Size = new System.Drawing.Size(122, 135);
            this.dgvBobbinsW.TabIndex = 32;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.HeaderText = "BobbinsW";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 102;
            // 
            // RMCode
            // 
            this.RMCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Khmer OS Battambang", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.RMCode.DefaultCellStyle = dataGridViewCellStyle3;
            this.RMCode.FillWeight = 160F;
            this.RMCode.HeaderText = "លេខកូដ";
            this.RMCode.Name = "RMCode";
            this.RMCode.Width = 90;
            // 
            // RMName
            // 
            this.RMName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.RMName.DefaultCellStyle = dataGridViewCellStyle4;
            this.RMName.FillWeight = 190F;
            this.RMName.HeaderText = "ឈ្មោះវត្ថុធាតុដើម";
            this.RMName.Name = "RMName";
            this.RMName.Width = 250;
            // 
            // RMType
            // 
            this.RMType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.RMType.HeaderText = "ប្រភេទវត្ថុធាតុដើម";
            this.RMType.Name = "RMType";
            this.RMType.Width = 130;
            // 
            // Maker
            // 
            this.Maker.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Maker.HeaderText = "អ្នកផលិត";
            this.Maker.Name = "Maker";
            this.Maker.Width = 120;
            // 
            // BobbinOrReel
            // 
            this.BobbinOrReel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.BobbinOrReel.HeaderText = "ប្រភេទ";
            this.BobbinOrReel.Name = "BobbinOrReel";
            this.BobbinOrReel.Width = 130;
            // 
            // MOQ
            // 
            this.MOQ.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N0";
            this.MOQ.DefaultCellStyle = dataGridViewCellStyle5;
            this.MOQ.HeaderText = "MOQ";
            this.MOQ.Name = "MOQ";
            // 
            // BobbinW
            // 
            this.BobbinW.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N2";
            this.BobbinW.DefaultCellStyle = dataGridViewCellStyle6;
            this.BobbinW.HeaderText = "រង្វង់តូច​​ mm/ទម្ងន់សំបកបូប៊ីន kg";
            this.BobbinW.Name = "BobbinW";
            this.BobbinW.Width = 150;
            // 
            // NetW
            // 
            this.NetW.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N2";
            this.NetW.DefaultCellStyle = dataGridViewCellStyle7;
            this.NetW.HeaderText = "រង្វង់ធំ mm/ទម្ងន់សុទ្ធ(គ្មានសំបក) kg";
            this.NetW.Name = "NetW";
            this.NetW.Width = 150;
            // 
            // TototalW
            // 
            this.TototalW.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N2";
            this.TototalW.DefaultCellStyle = dataGridViewCellStyle8;
            this.TototalW.HeaderText = "ប្រវែងសរុប mm/ទម្ងន់សរុប kg";
            this.TototalW.Name = "TototalW";
            this.TototalW.Width = 150;
            // 
            // RemainQty
            // 
            this.RemainQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N0";
            this.RemainQty.DefaultCellStyle = dataGridViewCellStyle9;
            this.RemainQty.HeaderText = "ប្រវែង/ចំនួននៅសល់";
            this.RemainQty.Name = "RemainQty";
            this.RemainQty.Width = 140;
            // 
            // LotNo
            // 
            this.LotNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LotNo.HeaderText = "លេខឡត";
            this.LotNo.Name = "LotNo";
            this.LotNo.Width = 150;
            // 
            // btnPrintGRAY
            // 
            this.btnPrintGRAY.Image = ((System.Drawing.Image)(resources.GetObject("btnPrintGRAY.Image")));
            this.btnPrintGRAY.Location = new System.Drawing.Point(113, 8);
            this.btnPrintGRAY.Name = "btnPrintGRAY";
            this.btnPrintGRAY.Size = new System.Drawing.Size(42, 42);
            this.btnPrintGRAY.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnPrintGRAY.TabIndex = 36;
            this.btnPrintGRAY.TabStop = false;
            // 
            // btnDeleteGRAY
            // 
            this.btnDeleteGRAY.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteGRAY.Image")));
            this.btnDeleteGRAY.Location = new System.Drawing.Point(168, 9);
            this.btnDeleteGRAY.Name = "btnDeleteGRAY";
            this.btnDeleteGRAY.Size = new System.Drawing.Size(37, 41);
            this.btnDeleteGRAY.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnDeleteGRAY.TabIndex = 36;
            this.btnDeleteGRAY.TabStop = false;
            // 
            // WireRemainingCalcAndPrintTag
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1184, 561);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.dgvBobbinsW);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WireRemainingCalcAndPrintTag";
            this.Text = "ព្រីនឡាប៊ែលវត្ថុធាតុដើមនៅសល់";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelHeader.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.GroupBoxResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBobbinsW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPrintGRAY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteGRAY)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Label LbStatus;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.GroupBox GroupBoxResult;
        public System.Windows.Forms.DataGridView dgvInput;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridView dgvBobbinsW;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn RMCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn RMName;
        private System.Windows.Forms.DataGridViewTextBoxColumn RMType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Maker;
        private System.Windows.Forms.DataGridViewTextBoxColumn BobbinOrReel;
        private System.Windows.Forms.DataGridViewTextBoxColumn MOQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn BobbinW;
        private System.Windows.Forms.DataGridViewTextBoxColumn NetW;
        private System.Windows.Forms.DataGridViewTextBoxColumn TototalW;
        private System.Windows.Forms.DataGridViewTextBoxColumn RemainQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn LotNo;
        private System.Windows.Forms.PictureBox btnPrintGRAY;
        private System.Windows.Forms.PictureBox btnDeleteGRAY;
    }
}