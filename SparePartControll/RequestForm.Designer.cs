namespace MachineDeptApp.SparePartControll
{
    partial class RequestForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RequestForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnDeleteGray = new System.Windows.Forms.PictureBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPno = new System.Windows.Forms.TextBox();
            this.chkPno = new System.Windows.Forms.CheckBox();
            this.txtPname = new System.Windows.Forms.TextBox();
            this.chkPname = new System.Windows.Forms.CheckBox();
            this.txtcode = new System.Windows.Forms.TextBox();
            this.chkcode = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lberror = new System.Windows.Forms.Label();
            this.lbFound = new System.Windows.Forms.Label();
            this.LbStatus = new System.Windows.Forms.Label();
            this.dgvRequest = new System.Windows.Forms.DataGridView();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.issuedate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderqty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unitprice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pono = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receivedate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receiveqty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remainamount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderstatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteGray)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequest)).BeginInit();
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
            this.panelHeader.Size = new System.Drawing.Size(800, 68);
            this.panelHeader.TabIndex = 52;
            // 
            // btnDeleteGray
            // 
            this.btnDeleteGray.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteGray.Image")));
            this.btnDeleteGray.Location = new System.Drawing.Point(72, 7);
            this.btnDeleteGray.Name = "btnDeleteGray";
            this.btnDeleteGray.Size = new System.Drawing.Size(47, 55);
            this.btnDeleteGray.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnDeleteGray.TabIndex = 31;
            this.btnDeleteGray.TabStop = false;
            this.toolTip1.SetToolTip(this.btnDeleteGray, "Delete");
            // 
            // btnDelete
            // 
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(72, 6);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(46, 56);
            this.btnDelete.TabIndex = 30;
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
            this.toolTip1.SetToolTip(this.btnSearch, "Search");
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
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
            this.groupBox1.Size = new System.Drawing.Size(800, 123);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Condition";
            // 
            // txtPno
            // 
            this.txtPno.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPno.Location = new System.Drawing.Point(143, 72);
            this.txtPno.Name = "txtPno";
            this.txtPno.Size = new System.Drawing.Size(101, 30);
            this.txtPno.TabIndex = 122;
            // 
            // chkPno
            // 
            this.chkPno.AutoSize = true;
            this.chkPno.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPno.Location = new System.Drawing.Point(34, 74);
            this.chkPno.Name = "chkPno";
            this.chkPno.Size = new System.Drawing.Size(73, 26);
            this.chkPno.TabIndex = 117;
            this.chkPno.Text = "Part No.";
            this.chkPno.UseVisualStyleBackColor = true;
            // 
            // txtPname
            // 
            this.txtPname.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPname.Location = new System.Drawing.Point(377, 74);
            this.txtPname.Name = "txtPname";
            this.txtPname.Size = new System.Drawing.Size(101, 30);
            this.txtPname.TabIndex = 122;
            // 
            // chkPname
            // 
            this.chkPname.AutoSize = true;
            this.chkPname.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPname.Location = new System.Drawing.Point(274, 76);
            this.chkPname.Name = "chkPname";
            this.chkPname.Size = new System.Drawing.Size(88, 26);
            this.chkPname.TabIndex = 117;
            this.chkPname.Text = "Part Name";
            this.chkPname.UseVisualStyleBackColor = true;
            // 
            // txtcode
            // 
            this.txtcode.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcode.Location = new System.Drawing.Point(143, 27);
            this.txtcode.Name = "txtcode";
            this.txtcode.Size = new System.Drawing.Size(101, 30);
            this.txtcode.TabIndex = 122;
            // 
            // chkcode
            // 
            this.chkcode.AutoSize = true;
            this.chkcode.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkcode.Location = new System.Drawing.Point(34, 29);
            this.chkcode.Name = "chkcode";
            this.chkcode.Size = new System.Drawing.Size(58, 26);
            this.chkcode.TabIndex = 117;
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
            this.panel2.Location = new System.Drawing.Point(0, 407);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 43);
            this.panel2.TabIndex = 57;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(671, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 24);
            this.label3.TabIndex = 4;
            // 
            // lberror
            // 
            this.lberror.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lberror.AutoSize = true;
            this.lberror.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lberror.Location = new System.Drawing.Point(-1022, 10);
            this.lberror.Name = "lberror";
            this.lberror.Size = new System.Drawing.Size(0, 24);
            this.lberror.TabIndex = 3;
            // 
            // lbFound
            // 
            this.lbFound.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbFound.AutoSize = true;
            this.lbFound.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFound.Location = new System.Drawing.Point(665, 10);
            this.lbFound.Name = "lbFound";
            this.lbFound.Size = new System.Drawing.Size(0, 24);
            this.lbFound.TabIndex = 2;
            // 
            // LbStatus
            // 
            this.LbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LbStatus.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbStatus.Location = new System.Drawing.Point(182, 4);
            this.LbStatus.Name = "LbStatus";
            this.LbStatus.Size = new System.Drawing.Size(614, 22);
            this.LbStatus.TabIndex = 1;
            this.LbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvRequest
            // 
            this.dgvRequest.AllowUserToAddRows = false;
            this.dgvRequest.AllowUserToDeleteRows = false;
            this.dgvRequest.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvRequest.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRequest.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRequest.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRequest.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRequest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRequest.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.code,
            this.Pno,
            this.Pname,
            this.issuedate,
            this.orderqty,
            this.unitprice,
            this.amount,
            this.eta,
            this.pono,
            this.receivedate,
            this.receiveqty,
            this.balance,
            this.remainamount,
            this.orderstatus,
            this.remark});
            this.dgvRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRequest.EnableHeadersVisualStyles = false;
            this.dgvRequest.Location = new System.Drawing.Point(0, 191);
            this.dgvRequest.Name = "dgvRequest";
            this.dgvRequest.ReadOnly = true;
            this.dgvRequest.RowHeadersVisible = false;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.White;
            this.dgvRequest.RowsDefaultCellStyle = dataGridViewCellStyle18;
            this.dgvRequest.RowTemplate.Height = 25;
            this.dgvRequest.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvRequest.Size = new System.Drawing.Size(800, 216);
            this.dgvRequest.TabIndex = 58;
            // 
            // code
            // 
            this.code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.code.DefaultCellStyle = dataGridViewCellStyle3;
            this.code.HeaderText = "Code";
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // Pno
            // 
            this.Pno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Pno.DefaultCellStyle = dataGridViewCellStyle4;
            this.Pno.HeaderText = "Part No.";
            this.Pno.Name = "Pno";
            this.Pno.ReadOnly = true;
            this.Pno.Width = 150;
            // 
            // Pname
            // 
            this.Pname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Pname.DefaultCellStyle = dataGridViewCellStyle5;
            this.Pname.HeaderText = "Part Name";
            this.Pname.Name = "Pname";
            this.Pname.ReadOnly = true;
            this.Pname.Width = 150;
            // 
            // issuedate
            // 
            this.issuedate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Format = "dd-MMM-yyyy";
            this.issuedate.DefaultCellStyle = dataGridViewCellStyle6;
            this.issuedate.HeaderText = "Issue Date";
            this.issuedate.Name = "issuedate";
            this.issuedate.ReadOnly = true;
            this.issuedate.Width = 130;
            // 
            // orderqty
            // 
            this.orderqty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.orderqty.DefaultCellStyle = dataGridViewCellStyle7;
            this.orderqty.HeaderText = "Order Qty";
            this.orderqty.Name = "orderqty";
            this.orderqty.ReadOnly = true;
            // 
            // unitprice
            // 
            this.unitprice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N4";
            this.unitprice.DefaultCellStyle = dataGridViewCellStyle8;
            this.unitprice.HeaderText = "Unit Price";
            this.unitprice.Name = "unitprice";
            this.unitprice.ReadOnly = true;
            this.unitprice.Width = 120;
            // 
            // amount
            // 
            this.amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N2";
            this.amount.DefaultCellStyle = dataGridViewCellStyle9;
            this.amount.HeaderText = "Amount";
            this.amount.Name = "amount";
            this.amount.ReadOnly = true;
            this.amount.Width = 120;
            // 
            // eta
            // 
            this.eta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.Format = "dd-MMM-yyyy";
            this.eta.DefaultCellStyle = dataGridViewCellStyle10;
            this.eta.HeaderText = "ETA";
            this.eta.Name = "eta";
            this.eta.ReadOnly = true;
            this.eta.Width = 130;
            // 
            // pono
            // 
            this.pono.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.Format = "N0";
            dataGridViewCellStyle11.NullValue = null;
            this.pono.DefaultCellStyle = dataGridViewCellStyle11;
            this.pono.HeaderText = "PO No.";
            this.pono.Name = "pono";
            this.pono.ReadOnly = true;
            this.pono.Width = 120;
            // 
            // receivedate
            // 
            this.receivedate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Format = "dd-MMM-yyyy";
            this.receivedate.DefaultCellStyle = dataGridViewCellStyle12;
            this.receivedate.HeaderText = "Receive Date";
            this.receivedate.Name = "receivedate";
            this.receivedate.ReadOnly = true;
            this.receivedate.Width = 130;
            // 
            // receiveqty
            // 
            this.receiveqty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle13.Format = "N0";
            dataGridViewCellStyle13.NullValue = null;
            this.receiveqty.DefaultCellStyle = dataGridViewCellStyle13;
            this.receiveqty.HeaderText = "Receive Qty";
            this.receiveqty.Name = "receiveqty";
            this.receiveqty.ReadOnly = true;
            this.receiveqty.Width = 140;
            // 
            // balance
            // 
            this.balance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.balance.DefaultCellStyle = dataGridViewCellStyle14;
            this.balance.HeaderText = "Balance";
            this.balance.Name = "balance";
            this.balance.ReadOnly = true;
            // 
            // remainamount
            // 
            this.remainamount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle15.Format = "N2";
            this.remainamount.DefaultCellStyle = dataGridViewCellStyle15;
            this.remainamount.HeaderText = "Remain Amount";
            this.remainamount.Name = "remainamount";
            this.remainamount.ReadOnly = true;
            this.remainamount.Width = 150;
            // 
            // orderstatus
            // 
            this.orderstatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.orderstatus.DefaultCellStyle = dataGridViewCellStyle16;
            this.orderstatus.HeaderText = "Order State";
            this.orderstatus.Name = "orderstatus";
            this.orderstatus.ReadOnly = true;
            this.orderstatus.Width = 150;
            // 
            // remark
            // 
            this.remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.remark.DefaultCellStyle = dataGridViewCellStyle17;
            this.remark.HeaderText = "Remark";
            this.remark.Name = "remark";
            this.remark.ReadOnly = true;
            this.remark.Width = 150;
            // 
            // RequestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvRequest);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panelHeader);
            this.Name = "RequestForm";
            this.Text = "Request";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteGray)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequest)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.PictureBox btnDeleteGray;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtPno;
        private System.Windows.Forms.CheckBox chkPno;
        private System.Windows.Forms.TextBox txtPname;
        private System.Windows.Forms.CheckBox chkPname;
        private System.Windows.Forms.TextBox txtcode;
        private System.Windows.Forms.CheckBox chkcode;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lberror;
        private System.Windows.Forms.Label lbFound;
        private System.Windows.Forms.Label LbStatus;
        public System.Windows.Forms.DataGridView dgvRequest;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pno;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pname;
        private System.Windows.Forms.DataGridViewTextBoxColumn issuedate;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderqty;
        private System.Windows.Forms.DataGridViewTextBoxColumn unitprice;
        private System.Windows.Forms.DataGridViewTextBoxColumn amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn eta;
        private System.Windows.Forms.DataGridViewTextBoxColumn pono;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivedate;
        private System.Windows.Forms.DataGridViewTextBoxColumn receiveqty;
        private System.Windows.Forms.DataGridViewTextBoxColumn balance;
        private System.Windows.Forms.DataGridViewTextBoxColumn remainamount;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderstatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
    }
}