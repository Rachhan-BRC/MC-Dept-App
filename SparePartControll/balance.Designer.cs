namespace MachineDeptApp
{
    partial class balance
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(balance));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle39 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle40 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle57 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle41 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle42 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle43 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle44 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle45 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle46 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle47 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle48 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle49 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle50 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle51 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle52 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle53 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle54 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle55 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle56 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRcode = new System.Windows.Forms.TextBox();
            this.chkRcode = new System.Windows.Forms.CheckBox();
            this.txtRname = new System.Windows.Forms.TextBox();
            this.chkRname = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lberror = new System.Windows.Forms.Label();
            this.lbFound = new System.Windows.Forms.Label();
            this.LbStatus = new System.Windows.Forms.Label();
            this.dgvTTL = new System.Windows.Forms.DataGridView();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prestock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stockout = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stockin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stockremain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.safetystock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.box = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderqty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leadtime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.balanceorder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.planeta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkstatus = new System.Windows.Forms.CheckBox();
            this.cbstatus = new System.Windows.Forms.ComboBox();
            this.panelHeader.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTTL)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelHeader.Controls.Add(this.btnPrint);
            this.panelHeader.Controls.Add(this.btnExport);
            this.panelHeader.Controls.Add(this.btnSearch);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1284, 68);
            this.panelHeader.TabIndex = 51;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnPrint.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPrint.BackgroundImage")));
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrint.Location = new System.Drawing.Point(71, 6);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(57, 56);
            this.btnPrint.TabIndex = 144;
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnExport.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnExport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExport.BackgroundImage")));
            this.btnExport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExport.Location = new System.Drawing.Point(1224, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(48, 56);
            this.btnExport.TabIndex = 143;
            this.btnExport.UseVisualStyleBackColor = false;
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
            this.groupBox1.Controls.Add(this.cbstatus);
            this.groupBox1.Controls.Add(this.dtpDate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtRcode);
            this.groupBox1.Controls.Add(this.chkRcode);
            this.groupBox1.Controls.Add(this.txtRname);
            this.groupBox1.Controls.Add(this.chkstatus);
            this.groupBox1.Controls.Add(this.chkRname);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1284, 76);
            this.groupBox1.TabIndex = 55;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Condition";
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "MMM-yyyy";
            this.dtpDate.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(457, 32);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(123, 30);
            this.dtpDate.TabIndex = 125;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(410, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 24);
            this.label1.TabIndex = 124;
            this.label1.Text = "Date:";
            // 
            // txtRcode
            // 
            this.txtRcode.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRcode.Location = new System.Drawing.Point(80, 32);
            this.txtRcode.Name = "txtRcode";
            this.txtRcode.Size = new System.Drawing.Size(101, 30);
            this.txtRcode.TabIndex = 122;
            // 
            // chkRcode
            // 
            this.chkRcode.AutoSize = true;
            this.chkRcode.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRcode.Location = new System.Drawing.Point(28, 34);
            this.chkRcode.Name = "chkRcode";
            this.chkRcode.Size = new System.Drawing.Size(58, 26);
            this.chkRcode.TabIndex = 117;
            this.chkRcode.Text = "Code";
            this.chkRcode.UseVisualStyleBackColor = true;
            // 
            // txtRname
            // 
            this.txtRname.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRname.Location = new System.Drawing.Point(285, 30);
            this.txtRname.Name = "txtRname";
            this.txtRname.Size = new System.Drawing.Size(101, 30);
            this.txtRname.TabIndex = 116;
            // 
            // chkRname
            // 
            this.chkRname.AutoSize = true;
            this.chkRname.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRname.Location = new System.Drawing.Point(201, 34);
            this.chkRname.Name = "chkRname";
            this.chkRname.Size = new System.Drawing.Size(88, 26);
            this.chkRname.TabIndex = 118;
            this.chkRname.Text = "Part Name";
            this.chkRname.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.lberror);
            this.panel2.Controls.Add(this.lbFound);
            this.panel2.Controls.Add(this.LbStatus);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 583);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1284, 43);
            this.panel2.TabIndex = 56;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1155, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 24);
            this.label3.TabIndex = 4;
            // 
            // lberror
            // 
            this.lberror.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lberror.AutoSize = true;
            this.lberror.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lberror.Location = new System.Drawing.Point(-538, 10);
            this.lberror.Name = "lberror";
            this.lberror.Size = new System.Drawing.Size(0, 24);
            this.lberror.TabIndex = 3;
            // 
            // lbFound
            // 
            this.lbFound.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbFound.AutoSize = true;
            this.lbFound.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFound.Location = new System.Drawing.Point(1149, 10);
            this.lbFound.Name = "lbFound";
            this.lbFound.Size = new System.Drawing.Size(0, 24);
            this.lbFound.TabIndex = 2;
            // 
            // LbStatus
            // 
            this.LbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LbStatus.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbStatus.Location = new System.Drawing.Point(666, 4);
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
            dataGridViewCellStyle39.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle39.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle39.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle39.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle39.SelectionForeColor = System.Drawing.Color.White;
            this.dgvTTL.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle39;
            this.dgvTTL.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTTL.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle40.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle40.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle40.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle40.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle40.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle40.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle40.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTTL.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle40;
            this.dgvTTL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTTL.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Code,
            this.partno,
            this.partname,
            this.supplier,
            this.prestock,
            this.stockout,
            this.stockin,
            this.stockremain,
            this.safetystock,
            this.box,
            this.orderqty,
            this.leadtime,
            this.eta,
            this.status,
            this.balanceorder,
            this.planeta});
            this.dgvTTL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTTL.EnableHeadersVisualStyles = false;
            this.dgvTTL.Location = new System.Drawing.Point(0, 144);
            this.dgvTTL.Name = "dgvTTL";
            this.dgvTTL.ReadOnly = true;
            this.dgvTTL.RowHeadersVisible = false;
            dataGridViewCellStyle57.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle57.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle57.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle57.SelectionForeColor = System.Drawing.Color.White;
            this.dgvTTL.RowsDefaultCellStyle = dataGridViewCellStyle57;
            this.dgvTTL.RowTemplate.Height = 25;
            this.dgvTTL.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvTTL.Size = new System.Drawing.Size(1284, 439);
            this.dgvTTL.TabIndex = 57;
            // 
            // Code
            // 
            this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle41.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Code.DefaultCellStyle = dataGridViewCellStyle41;
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            // 
            // partno
            // 
            this.partno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle42.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.partno.DefaultCellStyle = dataGridViewCellStyle42;
            this.partno.HeaderText = "Part No.";
            this.partno.Name = "partno";
            this.partno.ReadOnly = true;
            this.partno.Width = 150;
            // 
            // partname
            // 
            this.partname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle43.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.partname.DefaultCellStyle = dataGridViewCellStyle43;
            this.partname.HeaderText = "Part Name";
            this.partname.Name = "partname";
            this.partname.ReadOnly = true;
            this.partname.Width = 200;
            // 
            // supplier
            // 
            this.supplier.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle44.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.supplier.DefaultCellStyle = dataGridViewCellStyle44;
            this.supplier.HeaderText = "Supplyer";
            this.supplier.Name = "supplier";
            this.supplier.ReadOnly = true;
            this.supplier.Width = 90;
            // 
            // prestock
            // 
            this.prestock.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle45.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle45.Format = "N0";
            this.prestock.DefaultCellStyle = dataGridViewCellStyle45;
            this.prestock.HeaderText = "Pre-Stock";
            this.prestock.Name = "prestock";
            this.prestock.ReadOnly = true;
            this.prestock.Width = 80;
            // 
            // stockout
            // 
            this.stockout.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle46.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle46.Format = "N0";
            this.stockout.DefaultCellStyle = dataGridViewCellStyle46;
            this.stockout.HeaderText = "Stock Out ";
            this.stockout.Name = "stockout";
            this.stockout.ReadOnly = true;
            this.stockout.Width = 85;
            // 
            // stockin
            // 
            this.stockin.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle47.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle47.Format = "N0";
            this.stockin.DefaultCellStyle = dataGridViewCellStyle47;
            this.stockin.HeaderText = "Stock In";
            this.stockin.Name = "stockin";
            this.stockin.ReadOnly = true;
            this.stockin.Width = 80;
            // 
            // stockremain
            // 
            this.stockremain.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle48.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle48.Format = "N0";
            this.stockremain.DefaultCellStyle = dataGridViewCellStyle48;
            this.stockremain.HeaderText = "Stock Remain";
            this.stockremain.Name = "stockremain";
            this.stockremain.ReadOnly = true;
            this.stockremain.Width = 105;
            // 
            // safetystock
            // 
            this.safetystock.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle49.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle49.Format = "N0";
            this.safetystock.DefaultCellStyle = dataGridViewCellStyle49;
            this.safetystock.HeaderText = "Safety";
            this.safetystock.Name = "safetystock";
            this.safetystock.ReadOnly = true;
            this.safetystock.Width = 70;
            // 
            // box
            // 
            this.box.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle50.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.box.DefaultCellStyle = dataGridViewCellStyle50;
            this.box.HeaderText = "Loc No.";
            this.box.Name = "box";
            this.box.ReadOnly = true;
            this.box.Width = 80;
            // 
            // orderqty
            // 
            this.orderqty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle51.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle51.Format = "N0";
            this.orderqty.DefaultCellStyle = dataGridViewCellStyle51;
            this.orderqty.HeaderText = "Order Qty";
            this.orderqty.Name = "orderqty";
            this.orderqty.ReadOnly = true;
            this.orderqty.Width = 80;
            // 
            // leadtime
            // 
            this.leadtime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle52.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.leadtime.DefaultCellStyle = dataGridViewCellStyle52;
            this.leadtime.HeaderText = "LT (Week)";
            this.leadtime.Name = "leadtime";
            this.leadtime.ReadOnly = true;
            this.leadtime.Width = 85;
            // 
            // eta
            // 
            this.eta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle53.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle53.Format = "dd-MMM-yyyy";
            this.eta.DefaultCellStyle = dataGridViewCellStyle53;
            this.eta.HeaderText = "Estimate Arrival";
            this.eta.Name = "eta";
            this.eta.ReadOnly = true;
            this.eta.Width = 115;
            // 
            // status
            // 
            this.status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle54.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.status.DefaultCellStyle = dataGridViewCellStyle54;
            this.status.HeaderText = "Status";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            // 
            // balanceorder
            // 
            this.balanceorder.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle55.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.balanceorder.DefaultCellStyle = dataGridViewCellStyle55;
            this.balanceorder.HeaderText = "Balance Order";
            this.balanceorder.Name = "balanceorder";
            this.balanceorder.ReadOnly = true;
            this.balanceorder.Width = 115;
            // 
            // planeta
            // 
            this.planeta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle56.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle56.Format = "dd-MMM-yyyy";
            this.planeta.DefaultCellStyle = dataGridViewCellStyle56;
            this.planeta.HeaderText = "Plan ETA";
            this.planeta.Name = "planeta";
            this.planeta.ReadOnly = true;
            this.planeta.Width = 115;
            // 
            // chkstatus
            // 
            this.chkstatus.AutoSize = true;
            this.chkstatus.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkstatus.Location = new System.Drawing.Point(611, 32);
            this.chkstatus.Name = "chkstatus";
            this.chkstatus.Size = new System.Drawing.Size(63, 26);
            this.chkstatus.TabIndex = 118;
            this.chkstatus.Text = "Status";
            this.chkstatus.UseVisualStyleBackColor = true;
            // 
            // cbstatus
            // 
            this.cbstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbstatus.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbstatus.FormattingEnabled = true;
            this.cbstatus.Items.AddRange(new object[] {
            "",
            "Order",
            "No Order"});
            this.cbstatus.Location = new System.Drawing.Point(680, 30);
            this.cbstatus.Name = "cbstatus";
            this.cbstatus.Size = new System.Drawing.Size(98, 32);
            this.cbstatus.TabIndex = 126;
            // 
            // balance
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 626);
            this.Controls.Add(this.dgvTTL);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panelHeader);
            this.Name = "balance";
            this.Text = "balance";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelHeader.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTTL)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtRcode;
        private System.Windows.Forms.CheckBox chkRcode;
        private System.Windows.Forms.TextBox txtRname;
        private System.Windows.Forms.CheckBox chkRname;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lberror;
        private System.Windows.Forms.Label lbFound;
        private System.Windows.Forms.Label LbStatus;
        public System.Windows.Forms.DataGridView dgvTTL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn partno;
        private System.Windows.Forms.DataGridViewTextBoxColumn partname;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn prestock;
        private System.Windows.Forms.DataGridViewTextBoxColumn stockout;
        private System.Windows.Forms.DataGridViewTextBoxColumn stockin;
        private System.Windows.Forms.DataGridViewTextBoxColumn stockremain;
        private System.Windows.Forms.DataGridViewTextBoxColumn safetystock;
        private System.Windows.Forms.DataGridViewTextBoxColumn box;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderqty;
        private System.Windows.Forms.DataGridViewTextBoxColumn leadtime;
        private System.Windows.Forms.DataGridViewTextBoxColumn eta;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn balanceorder;
        private System.Windows.Forms.DataGridViewTextBoxColumn planeta;
        private System.Windows.Forms.CheckBox chkstatus;
        private System.Windows.Forms.ComboBox cbstatus;
    }
}