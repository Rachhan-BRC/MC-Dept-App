namespace MachineDeptApp
{
    partial class StockINOut
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockINOut));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabIN = new System.Windows.Forms.TabPage();
            this.dgvIn = new System.Windows.Forms.DataGridView();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partnumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.machinename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbfound1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lberror = new System.Windows.Forms.Label();
            this.lbNum2 = new System.Windows.Forms.Label();
            this.lbshow = new System.Windows.Forms.Label();
            this.tabOut = new System.Windows.Forms.TabPage();
            this.dgvOut = new System.Windows.Forms.DataGridView();
            this.code2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partnumber2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partname2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.machinename2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maker2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stockremain2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbstock = new System.Windows.Forms.Label();
            this.lbNum = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbfound2 = new System.Windows.Forms.Label();
            this.lbshow2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSaveGrey = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnDeleteGray = new System.Windows.Forms.PictureBox();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabIN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIn)).BeginInit();
            this.panel2.SuspendLayout();
            this.tabOut.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOut)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteGray)).BeginInit();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabIN);
            this.tabControl1.Controls.Add(this.tabOut);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 65);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1044, 516);
            this.tabControl1.TabIndex = 30;
            // 
            // tabIN
            // 
            this.tabIN.Controls.Add(this.dgvIn);
            this.tabIN.Controls.Add(this.panel2);
            this.tabIN.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabIN.Location = new System.Drawing.Point(4, 25);
            this.tabIN.Name = "tabIN";
            this.tabIN.Padding = new System.Windows.Forms.Padding(3);
            this.tabIN.Size = new System.Drawing.Size(1036, 487);
            this.tabIN.TabIndex = 0;
            this.tabIN.Text = "     Stock In     ";
            this.tabIN.UseVisualStyleBackColor = true;
            // 
            // dgvIn
            // 
            this.dgvIn.AllowUserToAddRows = false;
            this.dgvIn.AllowUserToDeleteRows = false;
            this.dgvIn.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvIn.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvIn.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvIn.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvIn.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvIn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIn.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.code,
            this.partnumber,
            this.partname,
            this.machinename,
            this.maker,
            this.Qty,
            this.remark});
            this.dgvIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvIn.EnableHeadersVisualStyles = false;
            this.dgvIn.Location = new System.Drawing.Point(3, 3);
            this.dgvIn.MultiSelect = false;
            this.dgvIn.Name = "dgvIn";
            this.dgvIn.ReadOnly = true;
            this.dgvIn.RowHeadersVisible = false;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            this.dgvIn.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvIn.RowTemplate.Height = 25;
            this.dgvIn.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvIn.Size = new System.Drawing.Size(1030, 438);
            this.dgvIn.TabIndex = 60;
            // 
            // code
            // 
            this.code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Format = "D4";
            this.code.DefaultCellStyle = dataGridViewCellStyle3;
            this.code.HeaderText = "Code";
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // partnumber
            // 
            this.partnumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.partnumber.HeaderText = "Part Number";
            this.partnumber.Name = "partnumber";
            this.partnumber.ReadOnly = true;
            this.partnumber.Width = 150;
            // 
            // partname
            // 
            this.partname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.partname.HeaderText = "Part Name";
            this.partname.Name = "partname";
            this.partname.ReadOnly = true;
            this.partname.Width = 150;
            // 
            // machinename
            // 
            this.machinename.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.machinename.HeaderText = "Machine Name ";
            this.machinename.Name = "machinename";
            this.machinename.ReadOnly = true;
            this.machinename.Width = 130;
            // 
            // maker
            // 
            this.maker.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.maker.HeaderText = "Maker";
            this.maker.Name = "maker";
            this.maker.ReadOnly = true;
            // 
            // Qty
            // 
            this.Qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Format = "N0";
            this.Qty.DefaultCellStyle = dataGridViewCellStyle4;
            this.Qty.HeaderText = "In Stock Qty";
            this.Qty.Name = "Qty";
            this.Qty.ReadOnly = true;
            this.Qty.Width = 130;
            // 
            // remark
            // 
            this.remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.remark.HeaderText = "Remark";
            this.remark.Name = "remark";
            this.remark.ReadOnly = true;
            this.remark.Width = 150;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbfound1);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.lberror);
            this.panel2.Controls.Add(this.lbNum2);
            this.panel2.Controls.Add(this.lbshow);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 441);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1030, 43);
            this.panel2.TabIndex = 59;
            // 
            // lbfound1
            // 
            this.lbfound1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbfound1.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbfound1.Location = new System.Drawing.Point(805, 12);
            this.lbfound1.Name = "lbfound1";
            this.lbfound1.Size = new System.Drawing.Size(222, 22);
            this.lbfound1.TabIndex = 5;
            this.lbfound1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(901, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 24);
            this.label3.TabIndex = 4;
            // 
            // lberror
            // 
            this.lberror.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lberror.AutoSize = true;
            this.lberror.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lberror.Location = new System.Drawing.Point(-792, 10);
            this.lberror.Name = "lberror";
            this.lberror.Size = new System.Drawing.Size(0, 24);
            this.lberror.TabIndex = 3;
            // 
            // lbNum2
            // 
            this.lbNum2.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNum2.Location = new System.Drawing.Point(261, 12);
            this.lbNum2.Name = "lbNum2";
            this.lbNum2.Size = new System.Drawing.Size(270, 22);
            this.lbNum2.TabIndex = 1;
            this.lbNum2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbshow
            // 
            this.lbshow.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbshow.Location = new System.Drawing.Point(0, 12);
            this.lbshow.Name = "lbshow";
            this.lbshow.Size = new System.Drawing.Size(246, 22);
            this.lbshow.TabIndex = 1;
            this.lbshow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabOut
            // 
            this.tabOut.Controls.Add(this.dgvOut);
            this.tabOut.Controls.Add(this.panel1);
            this.tabOut.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabOut.Location = new System.Drawing.Point(4, 25);
            this.tabOut.Name = "tabOut";
            this.tabOut.Padding = new System.Windows.Forms.Padding(3);
            this.tabOut.Size = new System.Drawing.Size(1036, 487);
            this.tabOut.TabIndex = 1;
            this.tabOut.Text = "     Stock Out     ";
            this.tabOut.UseVisualStyleBackColor = true;
            // 
            // dgvOut
            // 
            this.dgvOut.AllowUserToAddRows = false;
            this.dgvOut.AllowUserToDeleteRows = false;
            this.dgvOut.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            this.dgvOut.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvOut.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOut.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOut.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvOut.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOut.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.code2,
            this.partnumber2,
            this.partname2,
            this.machinename2,
            this.maker2,
            this.stockremain2,
            this.Qty2,
            this.remark2});
            this.dgvOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOut.EnableHeadersVisualStyles = false;
            this.dgvOut.Location = new System.Drawing.Point(3, 3);
            this.dgvOut.MultiSelect = false;
            this.dgvOut.Name = "dgvOut";
            this.dgvOut.ReadOnly = true;
            this.dgvOut.RowHeadersVisible = false;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White;
            this.dgvOut.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvOut.RowTemplate.Height = 25;
            this.dgvOut.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvOut.Size = new System.Drawing.Size(1030, 438);
            this.dgvOut.TabIndex = 63;
            // 
            // code2
            // 
            this.code2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle8.Format = "D4";
            this.code2.DefaultCellStyle = dataGridViewCellStyle8;
            this.code2.Frozen = true;
            this.code2.HeaderText = "Code";
            this.code2.Name = "code2";
            this.code2.ReadOnly = true;
            // 
            // partnumber2
            // 
            this.partnumber2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.partnumber2.Frozen = true;
            this.partnumber2.HeaderText = "Part Number";
            this.partnumber2.Name = "partnumber2";
            this.partnumber2.ReadOnly = true;
            this.partnumber2.Width = 150;
            // 
            // partname2
            // 
            this.partname2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.partname2.Frozen = true;
            this.partname2.HeaderText = "Part Name";
            this.partname2.Name = "partname2";
            this.partname2.ReadOnly = true;
            this.partname2.Width = 150;
            // 
            // machinename2
            // 
            this.machinename2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.machinename2.Frozen = true;
            this.machinename2.HeaderText = "Machine Name ";
            this.machinename2.Name = "machinename2";
            this.machinename2.ReadOnly = true;
            this.machinename2.Width = 130;
            // 
            // maker2
            // 
            this.maker2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.maker2.Frozen = true;
            this.maker2.HeaderText = "Maker";
            this.maker2.Name = "maker2";
            this.maker2.ReadOnly = true;
            // 
            // stockremain2
            // 
            this.stockremain2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.stockremain2.Frozen = true;
            this.stockremain2.HeaderText = "Stock Remain";
            this.stockremain2.Name = "stockremain2";
            this.stockremain2.ReadOnly = true;
            this.stockremain2.Width = 120;
            // 
            // Qty2
            // 
            this.Qty2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle9.Format = "N0";
            this.Qty2.DefaultCellStyle = dataGridViewCellStyle9;
            this.Qty2.Frozen = true;
            this.Qty2.HeaderText = "Out Stock Qty";
            this.Qty2.Name = "Qty2";
            this.Qty2.ReadOnly = true;
            this.Qty2.Width = 130;
            // 
            // remark2
            // 
            this.remark2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.remark2.HeaderText = "Remark";
            this.remark2.Name = "remark2";
            this.remark2.ReadOnly = true;
            this.remark2.Width = 150;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbstock);
            this.panel1.Controls.Add(this.lbNum);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lbfound2);
            this.panel1.Controls.Add(this.lbshow2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 441);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1030, 43);
            this.panel1.TabIndex = 62;
            // 
            // lbstock
            // 
            this.lbstock.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbstock.Location = new System.Drawing.Point(671, 12);
            this.lbstock.Name = "lbstock";
            this.lbstock.Size = new System.Drawing.Size(253, 22);
            this.lbstock.TabIndex = 5;
            this.lbstock.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbNum
            // 
            this.lbNum.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNum.Location = new System.Drawing.Point(350, 12);
            this.lbNum.Name = "lbNum";
            this.lbNum.Size = new System.Drawing.Size(315, 22);
            this.lbNum.TabIndex = 5;
            this.lbNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(901, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 24);
            this.label1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(-792, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 24);
            this.label2.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(895, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 24);
            this.label4.TabIndex = 2;
            // 
            // lbfound2
            // 
            this.lbfound2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbfound2.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbfound2.Location = new System.Drawing.Point(803, 12);
            this.lbfound2.Name = "lbfound2";
            this.lbfound2.Size = new System.Drawing.Size(222, 22);
            this.lbfound2.TabIndex = 1;
            this.lbfound2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbshow2
            // 
            this.lbshow2.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbshow2.Location = new System.Drawing.Point(0, 12);
            this.lbshow2.Name = "lbshow2";
            this.lbshow2.Size = new System.Drawing.Size(344, 22);
            this.lbshow2.TabIndex = 1;
            this.lbshow2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.Location = new System.Drawing.Point(71, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(53, 56);
            this.btnSave.TabIndex = 18;
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnSaveGrey
            // 
            this.btnSaveGrey.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSaveGrey.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveGrey.BackgroundImage")));
            this.btnSaveGrey.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSaveGrey.Enabled = false;
            this.btnSaveGrey.Location = new System.Drawing.Point(71, 4);
            this.btnSaveGrey.Name = "btnSaveGrey";
            this.btnSaveGrey.Size = new System.Drawing.Size(53, 56);
            this.btnSaveGrey.TabIndex = 18;
            this.btnSaveGrey.UseVisualStyleBackColor = false;
            // 
            // btnImport
            // 
            this.btnImport.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnImport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnImport.BackgroundImage")));
            this.btnImport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnImport.Location = new System.Drawing.Point(12, 4);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(53, 56);
            this.btnImport.TabIndex = 19;
            this.btnImport.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(131, 4);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(46, 56);
            this.btnDelete.TabIndex = 32;
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnDeleteGray
            // 
            this.btnDeleteGray.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteGray.Image")));
            this.btnDeleteGray.Location = new System.Drawing.Point(130, 5);
            this.btnDeleteGray.Name = "btnDeleteGray";
            this.btnDeleteGray.Size = new System.Drawing.Size(47, 55);
            this.btnDeleteGray.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnDeleteGray.TabIndex = 33;
            this.btnDeleteGray.TabStop = false;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelHeader.Controls.Add(this.btnDeleteGray);
            this.panelHeader.Controls.Add(this.btnDelete);
            this.panelHeader.Controls.Add(this.btnImport);
            this.panelHeader.Controls.Add(this.btnSaveGrey);
            this.panelHeader.Controls.Add(this.btnSave);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1044, 65);
            this.panelHeader.TabIndex = 21;
            // 
            // StockINOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 581);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panelHeader);
            this.Name = "StockINOut";
            this.Text = "Stock (In / Out)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tabControl1.ResumeLayout(false);
            this.tabIN.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIn)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabOut.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOut)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteGray)).EndInit();
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabIN;
        private System.Windows.Forms.TabPage tabOut;
        public System.Windows.Forms.DataGridView dgvIn;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn partnumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn partname;
        private System.Windows.Forms.DataGridViewTextBoxColumn machinename;
        private System.Windows.Forms.DataGridViewTextBoxColumn maker;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lberror;
        private System.Windows.Forms.Label lbshow;
        public System.Windows.Forms.DataGridView dgvOut;
        private System.Windows.Forms.DataGridViewTextBoxColumn code2;
        private System.Windows.Forms.DataGridViewTextBoxColumn partnumber2;
        private System.Windows.Forms.DataGridViewTextBoxColumn partname2;
        private System.Windows.Forms.DataGridViewTextBoxColumn machinename2;
        private System.Windows.Forms.DataGridViewTextBoxColumn maker2;
        private System.Windows.Forms.DataGridViewTextBoxColumn stockremain2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty2;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSaveGrey;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.PictureBox btnDeleteGray;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbshow2;
        private System.Windows.Forms.Label lbfound2;
        private System.Windows.Forms.Label lbfound1;
        private System.Windows.Forms.Label lbNum;
        private System.Windows.Forms.Label lbNum2;
        private System.Windows.Forms.Label lbstock;
    }
}