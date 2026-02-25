namespace MachineDeptApp.SparePartControll
{
    partial class PrintForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSaveGrey = new System.Windows.Forms.Button();
            this.rdMTH = new System.Windows.Forms.RadioButton();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtPno = new System.Windows.Forms.TextBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.rdnormal = new System.Windows.Forms.RadioButton();
            this.btnUpdateGrey = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.chkPno = new System.Windows.Forms.CheckBox();
            this.txtPname = new System.Windows.Forms.TextBox();
            this.btnDeletColor = new System.Windows.Forms.Button();
            this.chkPname = new System.Windows.Forms.CheckBox();
            this.btnAddColor = new System.Windows.Forms.Button();
            this.txtcode = new System.Windows.Forms.TextBox();
            this.chkcode = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btndelete = new System.Windows.Forms.PictureBox();
            this.gpboxsearch = new System.Windows.Forms.GroupBox();
            this.dgvTTL = new System.Windows.Forms.DataGridView();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.machinename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unitprice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leadtime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.find = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mcdocno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seteta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btndelete)).BeginInit();
            this.gpboxsearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTTL)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSaveGrey
            // 
            this.btnSaveGrey.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSaveGrey.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveGrey.BackgroundImage")));
            this.btnSaveGrey.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSaveGrey.Enabled = false;
            this.btnSaveGrey.Location = new System.Drawing.Point(67, 6);
            this.btnSaveGrey.Name = "btnSaveGrey";
            this.btnSaveGrey.Size = new System.Drawing.Size(53, 56);
            this.btnSaveGrey.TabIndex = 131;
            this.toolTip1.SetToolTip(this.btnSaveGrey, "Save");
            this.btnSaveGrey.UseVisualStyleBackColor = false;
            // 
            // rdMTH
            // 
            this.rdMTH.AutoSize = true;
            this.rdMTH.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdMTH.Location = new System.Drawing.Point(13, 37);
            this.rdMTH.Name = "rdMTH";
            this.rdMTH.Size = new System.Drawing.Size(107, 28);
            this.rdMTH.TabIndex = 139;
            this.rdMTH.TabStop = true;
            this.rdMTH.Text = "Request MTH";
            this.rdMTH.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(67, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(53, 56);
            this.btnSave.TabIndex = 132;
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // txtPno
            // 
            this.txtPno.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPno.Location = new System.Drawing.Point(416, 21);
            this.txtPno.Name = "txtPno";
            this.txtPno.Size = new System.Drawing.Size(101, 30);
            this.txtPno.TabIndex = 136;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnPrint.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPrint.BackgroundImage")));
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrint.Location = new System.Drawing.Point(250, 6);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(57, 56);
            this.btnPrint.TabIndex = 130;
            this.toolTip1.SetToolTip(this.btnPrint, "Print Issue");
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // rdnormal
            // 
            this.rdnormal.AutoSize = true;
            this.rdnormal.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdnormal.Location = new System.Drawing.Point(13, 7);
            this.rdnormal.Name = "rdnormal";
            this.rdnormal.Size = new System.Drawing.Size(102, 28);
            this.rdnormal.TabIndex = 140;
            this.rdnormal.TabStop = true;
            this.rdnormal.Text = "Request IPO";
            this.rdnormal.UseVisualStyleBackColor = true;
            // 
            // btnUpdateGrey
            // 
            this.btnUpdateGrey.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnUpdateGrey.BackColor = System.Drawing.Color.Transparent;
            this.btnUpdateGrey.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUpdateGrey.BackgroundImage")));
            this.btnUpdateGrey.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUpdateGrey.Enabled = false;
            this.btnUpdateGrey.Location = new System.Drawing.Point(126, 7);
            this.btnUpdateGrey.Name = "btnUpdateGrey";
            this.btnUpdateGrey.Size = new System.Drawing.Size(57, 56);
            this.btnUpdateGrey.TabIndex = 128;
            this.toolTip1.SetToolTip(this.btnUpdateGrey, "Update");
            this.btnUpdateGrey.UseVisualStyleBackColor = false;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnUpdate.BackColor = System.Drawing.Color.Transparent;
            this.btnUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUpdate.BackgroundImage")));
            this.btnUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Location = new System.Drawing.Point(126, 6);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(57, 56);
            this.btnUpdate.TabIndex = 129;
            this.btnUpdate.UseVisualStyleBackColor = false;
            // 
            // chkPno
            // 
            this.chkPno.AutoSize = true;
            this.chkPno.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPno.Location = new System.Drawing.Point(337, 23);
            this.chkPno.Name = "chkPno";
            this.chkPno.Size = new System.Drawing.Size(73, 26);
            this.chkPno.TabIndex = 133;
            this.chkPno.Text = "Part No.";
            this.chkPno.UseVisualStyleBackColor = true;
            // 
            // txtPname
            // 
            this.txtPname.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPname.Location = new System.Drawing.Point(640, 21);
            this.txtPname.Name = "txtPname";
            this.txtPname.Size = new System.Drawing.Size(163, 30);
            this.txtPname.TabIndex = 137;
            // 
            // btnDeletColor
            // 
            this.btnDeletColor.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDeletColor.BackgroundImage")));
            this.btnDeletColor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDeletColor.Enabled = false;
            this.btnDeletColor.Location = new System.Drawing.Point(13, 7);
            this.btnDeletColor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnDeletColor.Name = "btnDeletColor";
            this.btnDeletColor.Size = new System.Drawing.Size(47, 56);
            this.btnDeletColor.TabIndex = 126;
            this.btnDeletColor.UseVisualStyleBackColor = true;
            // 
            // chkPname
            // 
            this.chkPname.AutoSize = true;
            this.chkPname.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPname.Location = new System.Drawing.Point(546, 23);
            this.chkPname.Name = "chkPname";
            this.chkPname.Size = new System.Drawing.Size(88, 26);
            this.chkPname.TabIndex = 134;
            this.chkPname.Text = "Part Name";
            this.chkPname.UseVisualStyleBackColor = true;
            // 
            // btnAddColor
            // 
            this.btnAddColor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnAddColor.BackColor = System.Drawing.Color.Transparent;
            this.btnAddColor.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddColor.BackgroundImage")));
            this.btnAddColor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAddColor.Location = new System.Drawing.Point(190, 7);
            this.btnAddColor.Name = "btnAddColor";
            this.btnAddColor.Size = new System.Drawing.Size(57, 56);
            this.btnAddColor.TabIndex = 125;
            this.toolTip1.SetToolTip(this.btnAddColor, "Add");
            this.btnAddColor.UseVisualStyleBackColor = false;
            // 
            // txtcode
            // 
            this.txtcode.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcode.Location = new System.Drawing.Point(206, 21);
            this.txtcode.Name = "txtcode";
            this.txtcode.Size = new System.Drawing.Size(101, 30);
            this.txtcode.TabIndex = 138;
            // 
            // chkcode
            // 
            this.chkcode.AutoSize = true;
            this.chkcode.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkcode.Location = new System.Drawing.Point(136, 23);
            this.chkcode.Name = "chkcode";
            this.chkcode.Size = new System.Drawing.Size(58, 26);
            this.chkcode.TabIndex = 135;
            this.chkcode.Text = "Code";
            this.chkcode.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btndelete);
            this.panel1.Controls.Add(this.btnSaveGrey);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnAddColor);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnDeletColor);
            this.panel1.Controls.Add(this.btnUpdateGrey);
            this.panel1.Controls.Add(this.btnUpdate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(1278, 69);
            this.panel1.TabIndex = 141;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnExport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExport.BackgroundImage")));
            this.btnExport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExport.Location = new System.Drawing.Point(1214, 7);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(48, 56);
            this.btnExport.TabIndex = 142;
            this.toolTip1.SetToolTip(this.btnExport, "ទាញជាឯកសារ Excel");
            this.btnExport.UseVisualStyleBackColor = false;
            // 
            // btndelete
            // 
            this.btndelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndelete.Image = ((System.Drawing.Image)(resources.GetObject("btndelete.Image")));
            this.btndelete.Location = new System.Drawing.Point(13, 7);
            this.btndelete.Name = "btndelete";
            this.btndelete.Size = new System.Drawing.Size(47, 55);
            this.btndelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btndelete.TabIndex = 141;
            this.btndelete.TabStop = false;
            this.toolTip1.SetToolTip(this.btndelete, "Delete");
            // 
            // gpboxsearch
            // 
            this.gpboxsearch.Controls.Add(this.txtcode);
            this.gpboxsearch.Controls.Add(this.txtPno);
            this.gpboxsearch.Controls.Add(this.txtPname);
            this.gpboxsearch.Controls.Add(this.rdMTH);
            this.gpboxsearch.Controls.Add(this.chkPno);
            this.gpboxsearch.Controls.Add(this.chkPname);
            this.gpboxsearch.Controls.Add(this.chkcode);
            this.gpboxsearch.Controls.Add(this.rdnormal);
            this.gpboxsearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpboxsearch.Location = new System.Drawing.Point(3, 72);
            this.gpboxsearch.Name = "gpboxsearch";
            this.gpboxsearch.Size = new System.Drawing.Size(1278, 70);
            this.gpboxsearch.TabIndex = 145;
            this.gpboxsearch.TabStop = false;
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
            this.status,
            this.Code,
            this.partno,
            this.partname,
            this.machinename,
            this.supplier,
            this.maker,
            this.qty,
            this.unitprice,
            this.amount,
            this.eta,
            this.leadtime,
            this.find,
            this.mcdocno,
            this.seteta});
            this.dgvTTL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTTL.EnableHeadersVisualStyles = false;
            this.dgvTTL.Location = new System.Drawing.Point(3, 142);
            this.dgvTTL.Name = "dgvTTL";
            this.dgvTTL.RowHeadersVisible = false;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.White;
            this.dgvTTL.RowsDefaultCellStyle = dataGridViewCellStyle16;
            this.dgvTTL.RowTemplate.Height = 25;
            this.dgvTTL.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvTTL.Size = new System.Drawing.Size(1278, 416);
            this.dgvTTL.TabIndex = 146;
            // 
            // status
            // 
            this.status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.status.DefaultCellStyle = dataGridViewCellStyle3;
            this.status.HeaderText = "Status";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Width = 120;
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
            // partno
            // 
            this.partno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.partno.DefaultCellStyle = dataGridViewCellStyle5;
            this.partno.HeaderText = "Part No.";
            this.partno.Name = "partno";
            this.partno.ReadOnly = true;
            this.partno.Width = 150;
            // 
            // partname
            // 
            this.partname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.partname.DefaultCellStyle = dataGridViewCellStyle6;
            this.partname.HeaderText = "Part Name";
            this.partname.Name = "partname";
            this.partname.ReadOnly = true;
            this.partname.Width = 150;
            // 
            // machinename
            // 
            this.machinename.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.machinename.DefaultCellStyle = dataGridViewCellStyle7;
            this.machinename.HeaderText = "Machine Name";
            this.machinename.Name = "machinename";
            this.machinename.ReadOnly = true;
            this.machinename.Width = 150;
            // 
            // supplier
            // 
            this.supplier.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.supplier.DefaultCellStyle = dataGridViewCellStyle8;
            this.supplier.HeaderText = "Supplier";
            this.supplier.Name = "supplier";
            this.supplier.ReadOnly = true;
            // 
            // maker
            // 
            this.maker.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.maker.DefaultCellStyle = dataGridViewCellStyle9;
            this.maker.HeaderText = "Maker";
            this.maker.Name = "maker";
            this.maker.ReadOnly = true;
            this.maker.Width = 120;
            // 
            // qty
            // 
            this.qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.Format = "N0";
            this.qty.DefaultCellStyle = dataGridViewCellStyle10;
            this.qty.HeaderText = "Qty";
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            this.qty.Width = 120;
            // 
            // unitprice
            // 
            this.unitprice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Format = "N4";
            this.unitprice.DefaultCellStyle = dataGridViewCellStyle11;
            this.unitprice.HeaderText = "Unit Price";
            this.unitprice.Name = "unitprice";
            this.unitprice.ReadOnly = true;
            this.unitprice.Width = 120;
            // 
            // amount
            // 
            this.amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Format = "N2";
            this.amount.DefaultCellStyle = dataGridViewCellStyle12;
            this.amount.HeaderText = "Amount";
            this.amount.Name = "amount";
            this.amount.ReadOnly = true;
            this.amount.Width = 120;
            // 
            // eta
            // 
            this.eta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.Format = "MM-yyyy";
            this.eta.DefaultCellStyle = dataGridViewCellStyle13;
            this.eta.HeaderText = "ETA";
            this.eta.Name = "eta";
            this.eta.ReadOnly = true;
            this.eta.Width = 150;
            // 
            // leadtime
            // 
            this.leadtime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.leadtime.DefaultCellStyle = dataGridViewCellStyle14;
            this.leadtime.HeaderText = "LeadTime";
            this.leadtime.Name = "leadtime";
            this.leadtime.ReadOnly = true;
            // 
            // find
            // 
            this.find.HeaderText = "have or not";
            this.find.Name = "find";
            this.find.Visible = false;
            // 
            // mcdocno
            // 
            this.mcdocno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.mcdocno.HeaderText = "MCDocNo";
            this.mcdocno.Name = "mcdocno";
            this.mcdocno.Visible = false;
            // 
            // seteta
            // 
            dataGridViewCellStyle15.Format = "MM-yyyy";
            this.seteta.DefaultCellStyle = dataGridViewCellStyle15;
            this.seteta.HeaderText = "SetEta";
            this.seteta.Name = "seteta";
            this.seteta.Visible = false;
            // 
            // PrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1284, 561);
            this.Controls.Add(this.dgvTTL);
            this.Controls.Add(this.gpboxsearch);
            this.Controls.Add(this.panel1);
            this.Name = "PrintForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "Request Form";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btndelete)).EndInit();
            this.gpboxsearch.ResumeLayout(false);
            this.gpboxsearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTTL)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSaveGrey;
        private System.Windows.Forms.RadioButton rdMTH;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtPno;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.RadioButton rdnormal;
        private System.Windows.Forms.Button btnUpdateGrey;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.CheckBox chkPno;
        private System.Windows.Forms.TextBox txtPname;
        private System.Windows.Forms.Button btnDeletColor;
        private System.Windows.Forms.CheckBox chkPname;
        private System.Windows.Forms.Button btnAddColor;
        private System.Windows.Forms.TextBox txtcode;
        private System.Windows.Forms.CheckBox chkcode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox btndelete;
        private System.Windows.Forms.GroupBox gpboxsearch;
        public System.Windows.Forms.DataGridView dgvTTL;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn partno;
        private System.Windows.Forms.DataGridViewTextBoxColumn partname;
        private System.Windows.Forms.DataGridViewTextBoxColumn machinename;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn maker;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn unitprice;
        private System.Windows.Forms.DataGridViewTextBoxColumn amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn eta;
        private System.Windows.Forms.DataGridViewTextBoxColumn leadtime;
        private System.Windows.Forms.DataGridViewTextBoxColumn find;
        private System.Windows.Forms.DataGridViewTextBoxColumn mcdocno;
        private System.Windows.Forms.DataGridViewTextBoxColumn seteta;
    }
}