namespace MachineDeptApp
{
    partial class transaction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(transaction));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvTTL = new System.Windows.Forms.DataGridView();
            this.TranNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stockout = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stockin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stockamount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pono = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.regby = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.regdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lberror = new System.Windows.Forms.Label();
            this.lbFound = new System.Windows.Forms.Label();
            this.LbStatus = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbtype = new System.Windows.Forms.ComboBox();
            this.chkstatus = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtprecdateto = new System.Windows.Forms.DateTimePicker();
            this.dtprecdatefrom = new System.Windows.Forms.DateTimePicker();
            this.txtpono = new System.Windows.Forms.TextBox();
            this.chkpono = new System.Windows.Forms.CheckBox();
            this.txtremark = new System.Windows.Forms.TextBox();
            this.chkrecdate = new System.Windows.Forms.CheckBox();
            this.chkremark = new System.Windows.Forms.CheckBox();
            this.txtinvoice = new System.Windows.Forms.TextBox();
            this.chkinvoice = new System.Windows.Forms.CheckBox();
            this.txtPno = new System.Windows.Forms.TextBox();
            this.chkPno = new System.Windows.Forms.CheckBox();
            this.txtPname = new System.Windows.Forms.TextBox();
            this.chkPname = new System.Windows.Forms.CheckBox();
            this.txtcode = new System.Windows.Forms.TextBox();
            this.chkcode = new System.Windows.Forms.CheckBox();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnDeleteGray = new System.Windows.Forms.PictureBox();
            this.btnDelete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTTL)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteGray)).BeginInit();
            this.SuspendLayout();
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
            // dgvTTL
            // 
            this.dgvTTL.AllowUserToAddRows = false;
            this.dgvTTL.AllowUserToDeleteRows = false;
            this.dgvTTL.AllowUserToResizeRows = false;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.White;
            this.dgvTTL.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle16;
            this.dgvTTL.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTTL.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTTL.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.dgvTTL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTTL.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TranNo,
            this.Code,
            this.partno,
            this.partname,
            this.stockout,
            this.stockin,
            this.stockamount,
            this.pono,
            this.invoice,
            this.regby,
            this.regdate,
            this.remark});
            this.dgvTTL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTTL.EnableHeadersVisualStyles = false;
            this.dgvTTL.Location = new System.Drawing.Point(0, 175);
            this.dgvTTL.Name = "dgvTTL";
            this.dgvTTL.ReadOnly = true;
            this.dgvTTL.RowHeadersVisible = false;
            dataGridViewCellStyle30.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle30.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle30.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle30.SelectionForeColor = System.Drawing.Color.White;
            this.dgvTTL.RowsDefaultCellStyle = dataGridViewCellStyle30;
            this.dgvTTL.RowTemplate.Height = 25;
            this.dgvTTL.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvTTL.Size = new System.Drawing.Size(1484, 308);
            this.dgvTTL.TabIndex = 61;
            // 
            // TranNo
            // 
            this.TranNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.TranNo.DefaultCellStyle = dataGridViewCellStyle18;
            this.TranNo.HeaderText = "TransNo.";
            this.TranNo.Name = "TranNo";
            this.TranNo.ReadOnly = true;
            this.TranNo.Width = 120;
            // 
            // Code
            // 
            this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Code.DefaultCellStyle = dataGridViewCellStyle19;
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            // 
            // partno
            // 
            this.partno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.partno.DefaultCellStyle = dataGridViewCellStyle20;
            this.partno.HeaderText = "Part No.";
            this.partno.Name = "partno";
            this.partno.ReadOnly = true;
            this.partno.Width = 150;
            // 
            // partname
            // 
            this.partname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.partname.DefaultCellStyle = dataGridViewCellStyle21;
            this.partname.HeaderText = "Part Name";
            this.partname.Name = "partname";
            this.partname.ReadOnly = true;
            this.partname.Width = 200;
            // 
            // stockout
            // 
            this.stockout.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle22.Format = "N0";
            this.stockout.DefaultCellStyle = dataGridViewCellStyle22;
            this.stockout.HeaderText = "Stock Out ";
            this.stockout.Name = "stockout";
            this.stockout.ReadOnly = true;
            this.stockout.Width = 120;
            // 
            // stockin
            // 
            this.stockin.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle23.Format = "N0";
            this.stockin.DefaultCellStyle = dataGridViewCellStyle23;
            this.stockin.HeaderText = "Stock In";
            this.stockin.Name = "stockin";
            this.stockin.ReadOnly = true;
            this.stockin.Width = 120;
            // 
            // stockamount
            // 
            this.stockamount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.stockamount.DefaultCellStyle = dataGridViewCellStyle24;
            this.stockamount.HeaderText = "Stock Amount";
            this.stockamount.Name = "stockamount";
            this.stockamount.ReadOnly = true;
            this.stockamount.Width = 120;
            // 
            // pono
            // 
            this.pono.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.pono.DefaultCellStyle = dataGridViewCellStyle25;
            this.pono.HeaderText = "PO No";
            this.pono.Name = "pono";
            this.pono.ReadOnly = true;
            this.pono.Width = 120;
            // 
            // invoice
            // 
            this.invoice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.invoice.DefaultCellStyle = dataGridViewCellStyle26;
            this.invoice.HeaderText = "Invoice";
            this.invoice.Name = "invoice";
            this.invoice.ReadOnly = true;
            this.invoice.Width = 120;
            // 
            // regby
            // 
            this.regby.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.regby.DefaultCellStyle = dataGridViewCellStyle27;
            this.regby.HeaderText = "Register By";
            this.regby.Name = "regby";
            this.regby.ReadOnly = true;
            // 
            // regdate
            // 
            this.regdate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle28.Format = "N0";
            this.regdate.DefaultCellStyle = dataGridViewCellStyle28;
            this.regdate.HeaderText = "Register Date";
            this.regdate.Name = "regdate";
            this.regdate.ReadOnly = true;
            this.regdate.Width = 130;
            // 
            // remark
            // 
            this.remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.remark.DefaultCellStyle = dataGridViewCellStyle29;
            this.remark.HeaderText = "Remark";
            this.remark.Name = "remark";
            this.remark.ReadOnly = true;
            this.remark.Width = 150;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.lberror);
            this.panel2.Controls.Add(this.lbFound);
            this.panel2.Controls.Add(this.LbStatus);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 483);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1484, 43);
            this.panel2.TabIndex = 60;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1355, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 24);
            this.label3.TabIndex = 4;
            // 
            // lberror
            // 
            this.lberror.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lberror.AutoSize = true;
            this.lberror.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lberror.Location = new System.Drawing.Point(-338, 10);
            this.lberror.Name = "lberror";
            this.lberror.Size = new System.Drawing.Size(0, 24);
            this.lberror.TabIndex = 3;
            // 
            // lbFound
            // 
            this.lbFound.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbFound.AutoSize = true;
            this.lbFound.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFound.Location = new System.Drawing.Point(1349, 10);
            this.lbFound.Name = "lbFound";
            this.lbFound.Size = new System.Drawing.Size(0, 24);
            this.lbFound.TabIndex = 2;
            // 
            // LbStatus
            // 
            this.LbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LbStatus.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbStatus.Location = new System.Drawing.Point(866, 4);
            this.LbStatus.Name = "LbStatus";
            this.LbStatus.Size = new System.Drawing.Size(614, 22);
            this.LbStatus.TabIndex = 1;
            this.LbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Controls.Add(this.cbtype);
            this.groupBox1.Controls.Add(this.chkstatus);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.dtprecdateto);
            this.groupBox1.Controls.Add(this.dtprecdatefrom);
            this.groupBox1.Controls.Add(this.txtpono);
            this.groupBox1.Controls.Add(this.chkpono);
            this.groupBox1.Controls.Add(this.txtremark);
            this.groupBox1.Controls.Add(this.chkrecdate);
            this.groupBox1.Controls.Add(this.chkremark);
            this.groupBox1.Controls.Add(this.txtinvoice);
            this.groupBox1.Controls.Add(this.chkinvoice);
            this.groupBox1.Controls.Add(this.txtPno);
            this.groupBox1.Controls.Add(this.chkPno);
            this.groupBox1.Controls.Add(this.txtPname);
            this.groupBox1.Controls.Add(this.chkPname);
            this.groupBox1.Controls.Add(this.txtcode);
            this.groupBox1.Controls.Add(this.chkcode);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1484, 107);
            this.groupBox1.TabIndex = 59;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Condition";
            // 
            // cbtype
            // 
            this.cbtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbtype.FormattingEnabled = true;
            this.cbtype.Items.AddRange(new object[] {
            "",
            "Stock In",
            "Stock Out"});
            this.cbtype.Location = new System.Drawing.Point(814, 27);
            this.cbtype.Name = "cbtype";
            this.cbtype.Size = new System.Drawing.Size(121, 30);
            this.cbtype.TabIndex = 140;
            // 
            // chkstatus
            // 
            this.chkstatus.AutoSize = true;
            this.chkstatus.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkstatus.Location = new System.Drawing.Point(745, 29);
            this.chkstatus.Name = "chkstatus";
            this.chkstatus.Size = new System.Drawing.Size(56, 26);
            this.chkstatus.TabIndex = 139;
            this.chkstatus.Text = "Type";
            this.chkstatus.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(958, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 22);
            this.label2.TabIndex = 138;
            this.label2.Text = "-";
            // 
            // dtprecdateto
            // 
            this.dtprecdateto.CustomFormat = "     dd-MMM-yy";
            this.dtprecdateto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtprecdateto.Location = new System.Drawing.Point(979, 63);
            this.dtprecdateto.Name = "dtprecdateto";
            this.dtprecdateto.Size = new System.Drawing.Size(138, 30);
            this.dtprecdateto.TabIndex = 136;
            // 
            // dtprecdatefrom
            // 
            this.dtprecdatefrom.CustomFormat = "     dd-MMM-yy";
            this.dtprecdatefrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtprecdatefrom.Location = new System.Drawing.Point(814, 63);
            this.dtprecdatefrom.Name = "dtprecdatefrom";
            this.dtprecdatefrom.Size = new System.Drawing.Size(138, 30);
            this.dtprecdatefrom.TabIndex = 137;
            // 
            // txtpono
            // 
            this.txtpono.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpono.Location = new System.Drawing.Point(93, 63);
            this.txtpono.Name = "txtpono";
            this.txtpono.Size = new System.Drawing.Size(101, 30);
            this.txtpono.TabIndex = 133;
            // 
            // chkpono
            // 
            this.chkpono.AutoSize = true;
            this.chkpono.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkpono.Location = new System.Drawing.Point(29, 68);
            this.chkpono.Name = "chkpono";
            this.chkpono.Size = new System.Drawing.Size(68, 26);
            this.chkpono.TabIndex = 129;
            this.chkpono.Text = "PO No.";
            this.chkpono.UseVisualStyleBackColor = true;
            // 
            // txtremark
            // 
            this.txtremark.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtremark.Location = new System.Drawing.Point(512, 61);
            this.txtremark.Name = "txtremark";
            this.txtremark.Size = new System.Drawing.Size(172, 30);
            this.txtremark.TabIndex = 134;
            // 
            // chkrecdate
            // 
            this.chkrecdate.AutoSize = true;
            this.chkrecdate.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkrecdate.Location = new System.Drawing.Point(705, 65);
            this.chkrecdate.Name = "chkrecdate";
            this.chkrecdate.Size = new System.Drawing.Size(104, 26);
            this.chkrecdate.TabIndex = 130;
            this.chkrecdate.Text = "Register Date";
            this.chkrecdate.UseVisualStyleBackColor = true;
            // 
            // chkremark
            // 
            this.chkremark.AutoSize = true;
            this.chkremark.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkremark.Location = new System.Drawing.Point(418, 65);
            this.chkremark.Name = "chkremark";
            this.chkremark.Size = new System.Drawing.Size(72, 26);
            this.chkremark.TabIndex = 131;
            this.chkremark.Text = "Remark";
            this.chkremark.UseVisualStyleBackColor = true;
            // 
            // txtinvoice
            // 
            this.txtinvoice.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtinvoice.Location = new System.Drawing.Point(296, 63);
            this.txtinvoice.Name = "txtinvoice";
            this.txtinvoice.Size = new System.Drawing.Size(101, 30);
            this.txtinvoice.TabIndex = 126;
            // 
            // chkinvoice
            // 
            this.chkinvoice.AutoSize = true;
            this.chkinvoice.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkinvoice.Location = new System.Drawing.Point(217, 68);
            this.chkinvoice.Name = "chkinvoice";
            this.chkinvoice.Size = new System.Drawing.Size(87, 26);
            this.chkinvoice.TabIndex = 123;
            this.chkinvoice.Text = "Invoice No.";
            this.chkinvoice.UseVisualStyleBackColor = true;
            // 
            // txtPno
            // 
            this.txtPno.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPno.Location = new System.Drawing.Point(296, 27);
            this.txtPno.Name = "txtPno";
            this.txtPno.Size = new System.Drawing.Size(101, 30);
            this.txtPno.TabIndex = 126;
            // 
            // chkPno
            // 
            this.chkPno.AutoSize = true;
            this.chkPno.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPno.Location = new System.Drawing.Point(217, 29);
            this.chkPno.Name = "chkPno";
            this.chkPno.Size = new System.Drawing.Size(73, 26);
            this.chkPno.TabIndex = 123;
            this.chkPno.Text = "Part No.";
            this.chkPno.UseVisualStyleBackColor = true;
            // 
            // txtPname
            // 
            this.txtPname.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPname.Location = new System.Drawing.Point(512, 27);
            this.txtPname.Name = "txtPname";
            this.txtPname.Size = new System.Drawing.Size(173, 30);
            this.txtPname.TabIndex = 127;
            // 
            // chkPname
            // 
            this.chkPname.AutoSize = true;
            this.chkPname.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPname.Location = new System.Drawing.Point(418, 29);
            this.chkPname.Name = "chkPname";
            this.chkPname.Size = new System.Drawing.Size(88, 26);
            this.chkPname.TabIndex = 124;
            this.chkPname.Text = "Part Name";
            this.chkPname.UseVisualStyleBackColor = true;
            // 
            // txtcode
            // 
            this.txtcode.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcode.Location = new System.Drawing.Point(93, 27);
            this.txtcode.Name = "txtcode";
            this.txtcode.Size = new System.Drawing.Size(101, 30);
            this.txtcode.TabIndex = 128;
            // 
            // chkcode
            // 
            this.chkcode.AutoSize = true;
            this.chkcode.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkcode.Location = new System.Drawing.Point(29, 29);
            this.chkcode.Name = "chkcode";
            this.chkcode.Size = new System.Drawing.Size(58, 26);
            this.chkcode.TabIndex = 125;
            this.chkcode.Text = "Code";
            this.chkcode.UseVisualStyleBackColor = true;
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
            this.panelHeader.Size = new System.Drawing.Size(1484, 68);
            this.panelHeader.TabIndex = 58;
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
            // transaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 526);
            this.Controls.Add(this.dgvTTL);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panelHeader);
            this.Name = "transaction";
            this.Text = "Transaction ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvTTL)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteGray)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        public System.Windows.Forms.DataGridView dgvTTL;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lberror;
        private System.Windows.Forms.Label lbFound;
        private System.Windows.Forms.Label LbStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.PictureBox btnDeleteGray;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox txtPno;
        private System.Windows.Forms.CheckBox chkPno;
        private System.Windows.Forms.TextBox txtPname;
        private System.Windows.Forms.CheckBox chkPname;
        private System.Windows.Forms.TextBox txtcode;
        private System.Windows.Forms.CheckBox chkcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn TranNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn partno;
        private System.Windows.Forms.DataGridViewTextBoxColumn partname;
        private System.Windows.Forms.DataGridViewTextBoxColumn stockout;
        private System.Windows.Forms.DataGridViewTextBoxColumn stockin;
        private System.Windows.Forms.DataGridViewTextBoxColumn stockamount;
        private System.Windows.Forms.DataGridViewTextBoxColumn pono;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoice;
        private System.Windows.Forms.DataGridViewTextBoxColumn regby;
        private System.Windows.Forms.DataGridViewTextBoxColumn regdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtprecdateto;
        private System.Windows.Forms.DateTimePicker dtprecdatefrom;
        private System.Windows.Forms.TextBox txtpono;
        private System.Windows.Forms.CheckBox chkpono;
        private System.Windows.Forms.TextBox txtremark;
        private System.Windows.Forms.CheckBox chkrecdate;
        private System.Windows.Forms.CheckBox chkremark;
        private System.Windows.Forms.TextBox txtinvoice;
        private System.Windows.Forms.CheckBox chkinvoice;
        private System.Windows.Forms.ComboBox cbtype;
        private System.Windows.Forms.CheckBox chkstatus;
    }
}