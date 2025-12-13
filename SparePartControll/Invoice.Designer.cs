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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle55 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle56 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle60 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle57 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle58 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle59 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle61 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle62 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle72 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle63 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle64 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle65 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle66 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle67 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle68 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle69 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle70 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle71 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnSaveGrey = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDeleteGray = new System.Windows.Forms.PictureBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lberror = new System.Windows.Forms.Label();
            this.lbfound = new System.Windows.Forms.Label();
            this.lbshow = new System.Windows.Forms.Label();
            this.tabPo = new System.Windows.Forms.TabPage();
            this.dgvPo = new System.Windows.Forms.DataGridView();
            this.pono1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.docno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.find1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabInvoice = new System.Windows.Forms.TabPage();
            this.dgvInvoice = new System.Windows.Forms.DataGridView();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ooc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pono = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceusd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountusd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoiceno1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoicedate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.find = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteGray)).BeginInit();
            this.panel2.SuspendLayout();
            this.tabPo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPo)).BeginInit();
            this.tabInvoice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoice)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelHeader.Controls.Add(this.btnSaveGrey);
            this.panelHeader.Controls.Add(this.btnSave);
            this.panelHeader.Controls.Add(this.btnDeleteGray);
            this.panelHeader.Controls.Add(this.btnDelete);
            this.panelHeader.Controls.Add(this.btnImport);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1015, 68);
            this.panelHeader.TabIndex = 53;
            // 
            // btnSaveGrey
            // 
            this.btnSaveGrey.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSaveGrey.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveGrey.BackgroundImage")));
            this.btnSaveGrey.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSaveGrey.Enabled = false;
            this.btnSaveGrey.Location = new System.Drawing.Point(71, 6);
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
            this.btnSave.Location = new System.Drawing.Point(71, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(53, 56);
            this.btnSave.TabIndex = 33;
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnDeleteGray
            // 
            this.btnDeleteGray.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteGray.Image")));
            this.btnDeleteGray.Location = new System.Drawing.Point(130, 6);
            this.btnDeleteGray.Name = "btnDeleteGray";
            this.btnDeleteGray.Size = new System.Drawing.Size(47, 55);
            this.btnDeleteGray.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnDeleteGray.TabIndex = 31;
            this.btnDeleteGray.TabStop = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(131, 5);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(46, 56);
            this.btnDelete.TabIndex = 30;
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnImport.BackColor = System.Drawing.Color.Transparent;
            this.btnImport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnImport.BackgroundImage")));
            this.btnImport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnImport.Location = new System.Drawing.Point(12, 6);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(53, 56);
            this.btnImport.TabIndex = 29;
            this.btnImport.UseVisualStyleBackColor = false;
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
            // tabPo
            // 
            this.tabPo.Controls.Add(this.dgvPo);
            this.tabPo.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPo.Location = new System.Drawing.Point(4, 25);
            this.tabPo.Name = "tabPo";
            this.tabPo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPo.Size = new System.Drawing.Size(1007, 421);
            this.tabPo.TabIndex = 2;
            this.tabPo.Text = "     PO List     ";
            this.tabPo.UseVisualStyleBackColor = true;
            // 
            // dgvPo
            // 
            this.dgvPo.AllowUserToAddRows = false;
            this.dgvPo.AllowUserToDeleteRows = false;
            this.dgvPo.AllowUserToResizeRows = false;
            dataGridViewCellStyle55.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle55.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle55.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle55.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle55.SelectionForeColor = System.Drawing.Color.White;
            this.dgvPo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle55;
            this.dgvPo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle56.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle56.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle56.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle56.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle56.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle56.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle56.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle56;
            this.dgvPo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pono1,
            this.itemcode,
            this.docno,
            this.plan,
            this.remark1,
            this.find1});
            this.dgvPo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPo.EnableHeadersVisualStyles = false;
            this.dgvPo.Location = new System.Drawing.Point(3, 3);
            this.dgvPo.Name = "dgvPo";
            this.dgvPo.ReadOnly = true;
            this.dgvPo.RowHeadersVisible = false;
            dataGridViewCellStyle60.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle60.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle60.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle60.SelectionForeColor = System.Drawing.Color.White;
            this.dgvPo.RowsDefaultCellStyle = dataGridViewCellStyle60;
            this.dgvPo.RowTemplate.Height = 25;
            this.dgvPo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvPo.Size = new System.Drawing.Size(1001, 415);
            this.dgvPo.TabIndex = 63;
            // 
            // pono1
            // 
            this.pono1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.pono1.HeaderText = "PO No.";
            this.pono1.Name = "pono1";
            this.pono1.ReadOnly = true;
            this.pono1.Width = 130;
            // 
            // itemcode
            // 
            this.itemcode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle57.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.itemcode.DefaultCellStyle = dataGridViewCellStyle57;
            this.itemcode.HeaderText = "Item Code";
            this.itemcode.Name = "itemcode";
            this.itemcode.ReadOnly = true;
            // 
            // docno
            // 
            this.docno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle58.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.docno.DefaultCellStyle = dataGridViewCellStyle58;
            this.docno.HeaderText = "Document No.";
            this.docno.Name = "docno";
            this.docno.ReadOnly = true;
            this.docno.Width = 130;
            // 
            // plan
            // 
            this.plan.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle59.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle59.Format = "N4";
            this.plan.DefaultCellStyle = dataGridViewCellStyle59;
            this.plan.HeaderText = "Plan";
            this.plan.Name = "plan";
            this.plan.ReadOnly = true;
            this.plan.Width = 120;
            // 
            // remark1
            // 
            this.remark1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.remark1.HeaderText = "Remark ";
            this.remark1.Name = "remark1";
            this.remark1.ReadOnly = true;
            this.remark1.Width = 150;
            // 
            // find1
            // 
            this.find1.HeaderText = "find";
            this.find1.Name = "find1";
            this.find1.ReadOnly = true;
            this.find1.Visible = false;
            // 
            // tabInvoice
            // 
            this.tabInvoice.Controls.Add(this.dgvInvoice);
            this.tabInvoice.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabInvoice.Location = new System.Drawing.Point(4, 25);
            this.tabInvoice.Name = "tabInvoice";
            this.tabInvoice.Padding = new System.Windows.Forms.Padding(3);
            this.tabInvoice.Size = new System.Drawing.Size(1007, 421);
            this.tabInvoice.TabIndex = 0;
            this.tabInvoice.Text = "     Receive Invoice     ";
            this.tabInvoice.UseVisualStyleBackColor = true;
            // 
            // dgvInvoice
            // 
            this.dgvInvoice.AllowUserToAddRows = false;
            this.dgvInvoice.AllowUserToDeleteRows = false;
            this.dgvInvoice.AllowUserToResizeRows = false;
            dataGridViewCellStyle61.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle61.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle61.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle61.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle61.SelectionForeColor = System.Drawing.Color.White;
            this.dgvInvoice.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle61;
            this.dgvInvoice.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInvoice.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle62.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle62.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle62.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle62.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle62.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle62.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle62.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvInvoice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle62;
            this.dgvInvoice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInvoice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.description,
            this.code,
            this.Pno,
            this.ooc,
            this.pono,
            this.qty,
            this.unit,
            this.priceusd,
            this.amountusd,
            this.invoiceno1,
            this.invoicedate,
            this.remark,
            this.find});
            this.dgvInvoice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInvoice.EnableHeadersVisualStyles = false;
            this.dgvInvoice.Location = new System.Drawing.Point(3, 3);
            this.dgvInvoice.Name = "dgvInvoice";
            this.dgvInvoice.ReadOnly = true;
            this.dgvInvoice.RowHeadersVisible = false;
            dataGridViewCellStyle72.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle72.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle72.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle72.SelectionForeColor = System.Drawing.Color.White;
            this.dgvInvoice.RowsDefaultCellStyle = dataGridViewCellStyle72;
            this.dgvInvoice.RowTemplate.Height = 25;
            this.dgvInvoice.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvInvoice.Size = new System.Drawing.Size(1001, 415);
            this.dgvInvoice.TabIndex = 62;
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
            dataGridViewCellStyle63.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.code.DefaultCellStyle = dataGridViewCellStyle63;
            this.code.HeaderText = "Code";
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // Pno
            // 
            this.Pno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle64.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Pno.DefaultCellStyle = dataGridViewCellStyle64;
            this.Pno.HeaderText = "Part No.";
            this.Pno.Name = "Pno";
            this.Pno.ReadOnly = true;
            this.Pno.Width = 150;
            // 
            // ooc
            // 
            this.ooc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle65.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ooc.DefaultCellStyle = dataGridViewCellStyle65;
            this.ooc.HeaderText = "OOC";
            this.ooc.Name = "ooc";
            this.ooc.ReadOnly = true;
            this.ooc.Width = 130;
            // 
            // pono
            // 
            this.pono.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle66.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.pono.DefaultCellStyle = dataGridViewCellStyle66;
            this.pono.HeaderText = "PO No.";
            this.pono.Name = "pono";
            this.pono.ReadOnly = true;
            // 
            // qty
            // 
            this.qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle67.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle67.Format = "N4";
            this.qty.DefaultCellStyle = dataGridViewCellStyle67;
            this.qty.HeaderText = "Qty";
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            this.qty.Width = 120;
            // 
            // unit
            // 
            this.unit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle68.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle68.Format = "N2";
            this.unit.DefaultCellStyle = dataGridViewCellStyle68;
            this.unit.HeaderText = "Unit";
            this.unit.Name = "unit";
            this.unit.ReadOnly = true;
            this.unit.Width = 120;
            // 
            // priceusd
            // 
            this.priceusd.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle69.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.priceusd.DefaultCellStyle = dataGridViewCellStyle69;
            this.priceusd.HeaderText = "Price(USD)";
            this.priceusd.Name = "priceusd";
            this.priceusd.ReadOnly = true;
            // 
            // amountusd
            // 
            this.amountusd.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle70.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle70.Format = "N0";
            dataGridViewCellStyle70.NullValue = null;
            this.amountusd.DefaultCellStyle = dataGridViewCellStyle70;
            this.amountusd.HeaderText = "Amount(USD)";
            this.amountusd.Name = "amountusd";
            this.amountusd.ReadOnly = true;
            this.amountusd.Width = 120;
            // 
            // invoiceno1
            // 
            this.invoiceno1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.invoiceno1.HeaderText = "Invoice No";
            this.invoiceno1.Name = "invoiceno1";
            this.invoiceno1.ReadOnly = true;
            this.invoiceno1.Width = 120;
            // 
            // invoicedate
            // 
            this.invoicedate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle71.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.invoicedate.DefaultCellStyle = dataGridViewCellStyle71;
            this.invoicedate.HeaderText = "Invoice Date";
            this.invoicedate.Name = "invoicedate";
            this.invoicedate.ReadOnly = true;
            this.invoicedate.Width = 150;
            // 
            // remark
            // 
            this.remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.remark.HeaderText = "Remark";
            this.remark.Name = "remark";
            this.remark.ReadOnly = true;
            // 
            // find
            // 
            this.find.HeaderText = "have or not";
            this.find.Name = "find";
            this.find.ReadOnly = true;
            this.find.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPo);
            this.tabControl1.Controls.Add(this.tabInvoice);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 68);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1015, 450);
            this.tabControl1.TabIndex = 59;
            // 
            // InvoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1015, 561);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelHeader);
            this.Name = "InvoiceForm";
            this.Text = "Invoice";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteGray)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabPo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPo)).EndInit();
            this.tabInvoice.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoice)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.PictureBox btnDeleteGray;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lberror;
        private System.Windows.Forms.Label lbfound;
        private System.Windows.Forms.Label lbshow;
        private System.Windows.Forms.Button btnSaveGrey;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TabPage tabPo;
        public System.Windows.Forms.DataGridView dgvPo;
        private System.Windows.Forms.TabPage tabInvoice;
        public System.Windows.Forms.DataGridView dgvInvoice;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pno;
        private System.Windows.Forms.DataGridViewTextBoxColumn ooc;
        private System.Windows.Forms.DataGridViewTextBoxColumn pono;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceusd;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountusd;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoiceno1;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoicedate;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn find;
        private System.Windows.Forms.DataGridViewTextBoxColumn pono1;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn docno;
        private System.Windows.Forms.DataGridViewTextBoxColumn plan;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark1;
        private System.Windows.Forms.DataGridViewTextBoxColumn find1;
    }
}