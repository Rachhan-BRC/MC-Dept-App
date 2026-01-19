namespace MachineDeptApp.SparePartControll
{
    partial class AddPrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddPrint));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnMstRM = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvSearch = new System.Windows.Forms.DataGridView();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtpeta = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.cbsupplier = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtcode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtpnumber = new System.Windows.Forms.TextBox();
            this.txtamount = new System.Windows.Forms.TextBox();
            this.txtpname = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtmachinename = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbmaker = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtqty = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtunitprice = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtmoq = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtleadtime = new System.Windows.Forms.TextBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.partnumber1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvmst = new System.Windows.Forms.DataGridView();
            this.btnAddtoDgv = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvmst)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMstRM
            // 
            this.btnMstRM.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMstRM.BackgroundImage")));
            this.btnMstRM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMstRM.Location = new System.Drawing.Point(180, 47);
            this.btnMstRM.Name = "btnMstRM";
            this.btnMstRM.Size = new System.Drawing.Size(31, 32);
            this.btnMstRM.TabIndex = 193;
            this.btnMstRM.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnSearch.Location = new System.Drawing.Point(159, 47);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(19, 15);
            this.btnSearch.TabIndex = 192;
            this.btnSearch.Text = "button1";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // dgvSearch
            // 
            this.dgvSearch.AllowUserToAddRows = false;
            this.dgvSearch.AllowUserToDeleteRows = false;
            this.dgvSearch.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearch.ColumnHeadersVisible = false;
            this.dgvSearch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.code,
            this.partname});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSearch.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSearch.Location = new System.Drawing.Point(224, 47);
            this.dgvSearch.Name = "dgvSearch";
            this.dgvSearch.ReadOnly = true;
            this.dgvSearch.RowHeadersVisible = false;
            this.dgvSearch.Size = new System.Drawing.Size(289, 347);
            this.dgvSearch.TabIndex = 191;
            // 
            // code
            // 
            this.code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.code.HeaderText = "Code";
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // partname
            // 
            this.partname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.partname.HeaderText = "Part Name";
            this.partname.Name = "partname";
            this.partname.ReadOnly = true;
            this.partname.Width = 300;
            // 
            // dtpeta
            // 
            this.dtpeta.CalendarFont = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpeta.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpeta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpeta.Location = new System.Drawing.Point(12, 605);
            this.dtpeta.Name = "dtpeta";
            this.dtpeta.Size = new System.Drawing.Size(160, 32);
            this.dtpeta.TabIndex = 190;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(9, 335);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 24);
            this.label7.TabIndex = 189;
            this.label7.Text = "Supplier";
            // 
            // cbsupplier
            // 
            this.cbsupplier.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbsupplier.FormattingEnabled = true;
            this.cbsupplier.Items.AddRange(new object[] {
            "",
            "IPO",
            "Hear office"});
            this.cbsupplier.Location = new System.Drawing.Point(13, 362);
            this.cbsupplier.Name = "cbsupplier";
            this.cbsupplier.Size = new System.Drawing.Size(159, 32);
            this.cbsupplier.TabIndex = 188;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(14, 20);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(52, 24);
            this.label13.TabIndex = 187;
            this.label13.Text = "Search";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 24);
            this.label2.TabIndex = 186;
            this.label2.Text = "Code";
            // 
            // txtcode
            // 
            this.txtcode.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcode.Location = new System.Drawing.Point(14, 109);
            this.txtcode.Name = "txtcode";
            this.txtcode.Size = new System.Drawing.Size(161, 32);
            this.txtcode.TabIndex = 168;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 581);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 24);
            this.label4.TabIndex = 182;
            this.label4.Text = "ETA";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(12, 519);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(66, 24);
            this.label11.TabIndex = 181;
            this.label11.Text = "Unit Price";
            // 
            // txtpnumber
            // 
            this.txtpnumber.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpnumber.Location = new System.Drawing.Point(14, 171);
            this.txtpnumber.Name = "txtpnumber";
            this.txtpnumber.Size = new System.Drawing.Size(161, 32);
            this.txtpnumber.TabIndex = 167;
            // 
            // txtamount
            // 
            this.txtamount.Enabled = false;
            this.txtamount.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtamount.Location = new System.Drawing.Point(223, 546);
            this.txtamount.Name = "txtamount";
            this.txtamount.Size = new System.Drawing.Size(160, 32);
            this.txtamount.TabIndex = 177;
            // 
            // txtpname
            // 
            this.txtpname.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpname.Location = new System.Drawing.Point(12, 232);
            this.txtpname.Name = "txtpname";
            this.txtpname.Size = new System.Drawing.Size(161, 32);
            this.txtpname.TabIndex = 165;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(220, 519);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(86, 24);
            this.label10.TabIndex = 184;
            this.label10.Text = "Total Amount";
            // 
            // txtmachinename
            // 
            this.txtmachinename.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtmachinename.Location = new System.Drawing.Point(13, 294);
            this.txtmachinename.Name = "txtmachinename";
            this.txtmachinename.Size = new System.Drawing.Size(161, 32);
            this.txtmachinename.TabIndex = 166;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(220, 457);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 24);
            this.label5.TabIndex = 183;
            this.label5.Text = "Order Qty";
            // 
            // cbmaker
            // 
            this.cbmaker.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbmaker.FormattingEnabled = true;
            this.cbmaker.Items.AddRange(new object[] {
            "",
            "CHELIC",
            "CHINA",
            "CKD",
            "FUJI",
            "IAI",
            "JAM",
            "JMC",
            "JST",
            "KANIGEN",
            "KEYENCE",
            "KOKANEI",
            "MISUMI",
            "Mitsubishi",
            "MONOTARO",
            "MSC",
            "MURATA(MSC)",
            "OMRON",
            "Panasonic",
            "RIKO",
            "Schleuniger",
            "SHUNKE",
            "SMC",
            "STNC",
            "TE",
            "THK",
            "TYCO",
            "YINHUA"});
            this.cbmaker.Location = new System.Drawing.Point(14, 424);
            this.cbmaker.Name = "cbmaker";
            this.cbmaker.Size = new System.Drawing.Size(159, 32);
            this.cbmaker.TabIndex = 170;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(220, 397);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(42, 24);
            this.label12.TabIndex = 185;
            this.label12.Text = "MOQ";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(12, 144);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(84, 24);
            this.label15.TabIndex = 174;
            this.label15.Text = "Part Number";
            // 
            // txtqty
            // 
            this.txtqty.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtqty.Location = new System.Drawing.Point(224, 484);
            this.txtqty.Name = "txtqty";
            this.txtqty.Size = new System.Drawing.Size(159, 32);
            this.txtqty.TabIndex = 180;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(12, 206);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(72, 24);
            this.label14.TabIndex = 172;
            this.label14.Text = "Part Name";
            // 
            // txtunitprice
            // 
            this.txtunitprice.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtunitprice.Location = new System.Drawing.Point(12, 546);
            this.txtunitprice.Name = "txtunitprice";
            this.txtunitprice.Size = new System.Drawing.Size(160, 32);
            this.txtunitprice.TabIndex = 178;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 267);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 24);
            this.label6.TabIndex = 173;
            this.label6.Text = "Machine Name";
            // 
            // txtmoq
            // 
            this.txtmoq.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtmoq.Location = new System.Drawing.Point(224, 424);
            this.txtmoq.Name = "txtmoq";
            this.txtmoq.Size = new System.Drawing.Size(159, 32);
            this.txtmoq.TabIndex = 179;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 397);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 24);
            this.label8.TabIndex = 171;
            this.label8.Text = "Maker";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(12, 459);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 24);
            this.label9.TabIndex = 176;
            this.label9.Text = "Lead Time";
            // 
            // txtleadtime
            // 
            this.txtleadtime.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtleadtime.Location = new System.Drawing.Point(12, 486);
            this.txtleadtime.Name = "txtleadtime";
            this.txtleadtime.Size = new System.Drawing.Size(159, 32);
            this.txtleadtime.TabIndex = 175;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(13, 47);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(166, 32);
            this.txtSearch.TabIndex = 195;
            // 
            // partnumber1
            // 
            this.partnumber1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.partnumber1.HeaderText = "Part Number";
            this.partnumber1.Name = "partnumber1";
            this.partnumber1.ReadOnly = true;
            this.partnumber1.Width = 250;
            // 
            // code1
            // 
            this.code1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.code1.HeaderText = "Code";
            this.code1.Name = "code1";
            this.code1.ReadOnly = true;
            this.code1.Width = 50;
            // 
            // dgvmst
            // 
            this.dgvmst.AllowUserToAddRows = false;
            this.dgvmst.AllowUserToDeleteRows = false;
            this.dgvmst.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvmst.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvmst.ColumnHeadersVisible = false;
            this.dgvmst.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.code1,
            this.partnumber1});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvmst.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvmst.Location = new System.Drawing.Point(12, 82);
            this.dgvmst.Name = "dgvmst";
            this.dgvmst.ReadOnly = true;
            this.dgvmst.RowHeadersVisible = false;
            this.dgvmst.Size = new System.Drawing.Size(199, 244);
            this.dgvmst.TabIndex = 194;
            this.dgvmst.Visible = false;
            // 
            // btnAddtoDgv
            // 
            this.btnAddtoDgv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnAddtoDgv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddtoDgv.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddtoDgv.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnAddtoDgv.Location = new System.Drawing.Point(243, 601);
            this.btnAddtoDgv.Name = "btnAddtoDgv";
            this.btnAddtoDgv.Size = new System.Drawing.Size(111, 40);
            this.btnAddtoDgv.TabIndex = 196;
            this.btnAddtoDgv.Text = "Add";
            this.btnAddtoDgv.UseVisualStyleBackColor = false;
            // 
            // AddPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(525, 672);
            this.Controls.Add(this.btnAddtoDgv);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.dgvmst);
            this.Controls.Add(this.btnMstRM);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dgvSearch);
            this.Controls.Add(this.dtpeta);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbsupplier);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtcode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtpnumber);
            this.Controls.Add(this.txtamount);
            this.Controls.Add(this.txtpname);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtmachinename);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbmaker);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtqty);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtunitprice);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtmoq);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtleadtime);
            this.Name = "AddPrint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add to print ";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvmst)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnMstRM;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvSearch;
        private System.Windows.Forms.DateTimePicker dtpeta;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbsupplier;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtcode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtpnumber;
        private System.Windows.Forms.TextBox txtamount;
        private System.Windows.Forms.TextBox txtpname;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtmachinename;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbmaker;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtqty;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtunitprice;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtmoq;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtleadtime;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn partname;
        private System.Windows.Forms.DataGridViewTextBoxColumn partnumber1;
        private System.Windows.Forms.DataGridViewTextBoxColumn code1;
        private System.Windows.Forms.DataGridView dgvmst;
        private System.Windows.Forms.Button btnAddtoDgv;
    }
}