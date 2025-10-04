namespace MachineDeptApp.MCSDControl.SDRec
{
    partial class SDReceiveForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SDReceiveForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.LbStatus = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnSaveGray = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnDeleteGray = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.panelBody2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.splitterBody = new System.Windows.Forms.Splitter();
            this.panelBody = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvScanned = new System.Windows.Forms.DataGridView();
            this.POSNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WIPCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WIPName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.POSQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShipDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CompleteSet = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvConsumption = new System.Windows.Forms.DataGridView();
            this.RMCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RMName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BOMQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConsumptionQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KITTransferQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MCRecQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MCRemQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelFooter.SuspendLayout();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveGray)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteGray)).BeginInit();
            this.panelBody2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScanned)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConsumption)).BeginInit();
            this.SuspendLayout();
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.LightGray;
            this.panelFooter.Controls.Add(this.LbStatus);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 526);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(1184, 35);
            this.panelFooter.TabIndex = 16;
            // 
            // LbStatus
            // 
            this.LbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LbStatus.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbStatus.Location = new System.Drawing.Point(535, 4);
            this.LbStatus.Name = "LbStatus";
            this.LbStatus.Size = new System.Drawing.Size(645, 26);
            this.LbStatus.TabIndex = 0;
            this.LbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LbStatus.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(6, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(53, 56);
            this.btnSave.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnSave, "រក្សាទុក");
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.White;
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(63, 5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(53, 56);
            this.btnDelete.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnDelete, "លុប");
            this.btnDelete.UseVisualStyleBackColor = false;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.btnSaveGray);
            this.panelHeader.Controls.Add(this.pictureBox1);
            this.panelHeader.Controls.Add(this.btnSave);
            this.panelHeader.Controls.Add(this.btnDeleteGray);
            this.panelHeader.Controls.Add(this.btnDelete);
            this.panelHeader.Controls.Add(this.label2);
            this.panelHeader.Controls.Add(this.txtBarcode);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1184, 90);
            this.panelHeader.TabIndex = 19;
            // 
            // btnSaveGray
            // 
            this.btnSaveGray.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveGray.Image")));
            this.btnSaveGray.Location = new System.Drawing.Point(10, 9);
            this.btnSaveGray.Name = "btnSaveGray";
            this.btnSaveGray.Size = new System.Drawing.Size(45, 48);
            this.btnSaveGray.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnSaveGray.TabIndex = 27;
            this.btnSaveGray.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1112, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(69, 57);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 26;
            this.pictureBox1.TabStop = false;
            // 
            // btnDeleteGray
            // 
            this.btnDeleteGray.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteGray.Image")));
            this.btnDeleteGray.Location = new System.Drawing.Point(67, 9);
            this.btnDeleteGray.Name = "btnDeleteGray";
            this.btnDeleteGray.Size = new System.Drawing.Size(45, 48);
            this.btnDeleteGray.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnDeleteGray.TabIndex = 27;
            this.btnDeleteGray.TabStop = false;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Khmer OS Battambang", 14.25F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label2.Location = new System.Drawing.Point(359, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 34);
            this.label2.TabIndex = 21;
            this.label2.Text = "ស្កេនបាកូដត្រង់នេះ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBarcode
            // 
            this.txtBarcode.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtBarcode.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcode.Location = new System.Drawing.Point(537, 56);
            this.txtBarcode.MaxLength = 10;
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(305, 32);
            this.txtBarcode.TabIndex = 0;
            // 
            // panelBody2
            // 
            this.panelBody2.Controls.Add(this.groupBox3);
            this.panelBody2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBody2.Location = new System.Drawing.Point(0, 317);
            this.panelBody2.Name = "panelBody2";
            this.panelBody2.Padding = new System.Windows.Forms.Padding(3, 1, 3, 2);
            this.panelBody2.Size = new System.Drawing.Size(1184, 209);
            this.panelBody2.TabIndex = 25;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvConsumption);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox3.Location = new System.Drawing.Point(3, 1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1178, 206);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "វត្ថុធាតុដើមដែលប្រើប្រាស់";
            // 
            // splitterBody
            // 
            this.splitterBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.splitterBody.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterBody.Location = new System.Drawing.Point(0, 315);
            this.splitterBody.Name = "splitterBody";
            this.splitterBody.Size = new System.Drawing.Size(1184, 2);
            this.splitterBody.TabIndex = 26;
            this.splitterBody.TabStop = false;
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.groupBox2);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 90);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(3, 1, 3, 2);
            this.panelBody.Size = new System.Drawing.Size(1184, 225);
            this.panelBody.TabIndex = 27;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvScanned);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(3, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1178, 222);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ភីអូអេសដែលស្កេនរួច";
            // 
            // dgvScanned
            // 
            this.dgvScanned.AllowUserToAddRows = false;
            this.dgvScanned.AllowUserToDeleteRows = false;
            this.dgvScanned.AllowUserToResizeRows = false;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.White;
            this.dgvScanned.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle12;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvScanned.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvScanned.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScanned.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.POSNo,
            this.WIPCode,
            this.WIPName,
            this.POSQty,
            this.ShipDate,
            this.CompleteSet});
            this.dgvScanned.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvScanned.EnableHeadersVisualStyles = false;
            this.dgvScanned.Location = new System.Drawing.Point(3, 26);
            this.dgvScanned.MultiSelect = false;
            this.dgvScanned.Name = "dgvScanned";
            this.dgvScanned.ReadOnly = true;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvScanned.RowHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.dgvScanned.RowHeadersWidth = 50;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.Color.White;
            this.dgvScanned.RowsDefaultCellStyle = dataGridViewCellStyle19;
            this.dgvScanned.RowTemplate.Height = 25;
            this.dgvScanned.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvScanned.Size = new System.Drawing.Size(1172, 193);
            this.dgvScanned.TabIndex = 13;
            // 
            // POSNo
            // 
            this.POSNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.POSNo.HeaderText = "លេខភីអូអេស";
            this.POSNo.Name = "POSNo";
            this.POSNo.ReadOnly = true;
            this.POSNo.Width = 120;
            // 
            // WIPCode
            // 
            this.WIPCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.WIPCode.DefaultCellStyle = dataGridViewCellStyle14;
            this.WIPCode.FillWeight = 190F;
            this.WIPCode.HeaderText = "លេខកូដសឺមី";
            this.WIPCode.Name = "WIPCode";
            this.WIPCode.ReadOnly = true;
            this.WIPCode.Width = 120;
            // 
            // WIPName
            // 
            this.WIPName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.WIPName.DefaultCellStyle = dataGridViewCellStyle15;
            this.WIPName.HeaderText = "ឈ្មោះផលិតផល";
            this.WIPName.Name = "WIPName";
            this.WIPName.ReadOnly = true;
            this.WIPName.Width = 250;
            // 
            // POSQty
            // 
            this.POSQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle16.Format = "N0";
            this.POSQty.DefaultCellStyle = dataGridViewCellStyle16;
            this.POSQty.HeaderText = "ចំនួន";
            this.POSQty.Name = "POSQty";
            this.POSQty.ReadOnly = true;
            this.POSQty.Width = 120;
            // 
            // ShipDate
            // 
            this.ShipDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle17.Format = "dd-MM-yyyy";
            this.ShipDate.DefaultCellStyle = dataGridViewCellStyle17;
            this.ShipDate.HeaderText = "ថ្ងៃចេញទំនិញ";
            this.ShipDate.Name = "ShipDate";
            this.ShipDate.ReadOnly = true;
            this.ShipDate.Width = 140;
            // 
            // CompleteSet
            // 
            this.CompleteSet.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CompleteSet.HeaderText = "គ្រប់ឈុត (SET)";
            this.CompleteSet.Name = "CompleteSet";
            this.CompleteSet.ReadOnly = true;
            this.CompleteSet.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CompleteSet.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.CompleteSet.Width = 150;
            // 
            // dgvConsumption
            // 
            this.dgvConsumption.AllowUserToAddRows = false;
            this.dgvConsumption.AllowUserToDeleteRows = false;
            this.dgvConsumption.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvConsumption.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvConsumption.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvConsumption.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvConsumption.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConsumption.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RMCode,
            this.RMName,
            this.BOMQty,
            this.ConsumptionQty,
            this.KITTransferQty,
            this.MCRecQty,
            this.MCRemQty});
            this.dgvConsumption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvConsumption.EnableHeadersVisualStyles = false;
            this.dgvConsumption.Location = new System.Drawing.Point(3, 26);
            this.dgvConsumption.MultiSelect = false;
            this.dgvConsumption.Name = "dgvConsumption";
            this.dgvConsumption.ReadOnly = true;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Calibri", 10F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvConsumption.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvConsumption.RowHeadersWidth = 50;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.White;
            this.dgvConsumption.RowsDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvConsumption.RowTemplate.Height = 25;
            this.dgvConsumption.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvConsumption.Size = new System.Drawing.Size(1172, 177);
            this.dgvConsumption.TabIndex = 14;
            // 
            // RMCode
            // 
            this.RMCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.RMCode.DefaultCellStyle = dataGridViewCellStyle3;
            this.RMCode.HeaderText = "លេខកូដ";
            this.RMCode.Name = "RMCode";
            this.RMCode.ReadOnly = true;
            this.RMCode.Width = 80;
            // 
            // RMName
            // 
            this.RMName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.RMName.DefaultCellStyle = dataGridViewCellStyle4;
            this.RMName.FillWeight = 190F;
            this.RMName.HeaderText = "ឈ្មោះវត្ថុធាតុដើម";
            this.RMName.Name = "RMName";
            this.RMName.ReadOnly = true;
            this.RMName.Width = 200;
            // 
            // BOMQty
            // 
            this.BOMQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Format = "N0";
            this.BOMQty.DefaultCellStyle = dataGridViewCellStyle5;
            this.BOMQty.HeaderText = "BOM Qty";
            this.BOMQty.Name = "BOMQty";
            this.BOMQty.ReadOnly = true;
            this.BOMQty.Width = 90;
            // 
            // ConsumptionQty
            // 
            this.ConsumptionQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            this.ConsumptionQty.DefaultCellStyle = dataGridViewCellStyle6;
            this.ConsumptionQty.HeaderText = "ប្រើសរុប";
            this.ConsumptionQty.Name = "ConsumptionQty";
            this.ConsumptionQty.ReadOnly = true;
            this.ConsumptionQty.Width = 125;
            // 
            // KITTransferQty
            // 
            this.KITTransferQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N0";
            this.KITTransferQty.DefaultCellStyle = dataGridViewCellStyle7;
            this.KITTransferQty.HeaderText = "PP-KIT វេររួច";
            this.KITTransferQty.Name = "KITTransferQty";
            this.KITTransferQty.ReadOnly = true;
            this.KITTransferQty.Width = 125;
            // 
            // MCRecQty
            // 
            this.MCRecQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N0";
            this.MCRecQty.DefaultCellStyle = dataGridViewCellStyle8;
            this.MCRecQty.HeaderText = "MC-KIT ធ្លាប់ទទួល";
            this.MCRecQty.Name = "MCRecQty";
            this.MCRecQty.ReadOnly = true;
            this.MCRecQty.Width = 125;
            // 
            // MCRemQty
            // 
            this.MCRemQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N0";
            this.MCRemQty.DefaultCellStyle = dataGridViewCellStyle9;
            this.MCRemQty.HeaderText = "MC-KIT ត្រូវទទួល";
            this.MCRemQty.Name = "MCRemQty";
            this.MCRemQty.ReadOnly = true;
            this.MCRemQty.Width = 125;
            // 
            // SDReceiveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1184, 561);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.splitterBody);
            this.Controls.Add(this.panelBody2);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelFooter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SDReceiveForm";
            this.Text = "SD Receive";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelFooter.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveGray)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteGray)).EndInit();
            this.panelBody2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvScanned)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConsumption)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Label LbStatus;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.PictureBox btnSaveGray;
        private System.Windows.Forms.PictureBox btnDeleteGray;
        private System.Windows.Forms.Panel panelBody2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Splitter splitterBody;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.DataGridView dgvScanned;
        private System.Windows.Forms.DataGridViewTextBoxColumn POSNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn WIPCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn WIPName;
        private System.Windows.Forms.DataGridViewTextBoxColumn POSQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn ShipDate;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CompleteSet;
        public System.Windows.Forms.DataGridView dgvConsumption;
        private System.Windows.Forms.DataGridViewTextBoxColumn RMCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn RMName;
        private System.Windows.Forms.DataGridViewTextBoxColumn BOMQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConsumptionQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn KITTransferQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn MCRecQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn MCRemQty;
    }
}