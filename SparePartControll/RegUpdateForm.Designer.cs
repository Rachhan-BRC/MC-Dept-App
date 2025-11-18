namespace MachineDeptApp
{
    partial class RegUpdateForm
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
            this.btnSave = new System.Windows.Forms.Button();
            this.txtcode = new System.Windows.Forms.TextBox();
            this.txtpnumber = new System.Windows.Forms.TextBox();
            this.txtpname = new System.Windows.Forms.TextBox();
            this.cbusefor = new System.Windows.Forms.ComboBox();
            this.cbmaker = new System.Windows.Forms.ComboBox();
            this.txtsafetystock = new System.Windows.Forms.TextBox();
            this.txtmoq = new System.Windows.Forms.TextBox();
            this.txtunitprice = new System.Windows.Forms.TextBox();
            this.cbcodeprefix = new System.Windows.Forms.ComboBox();
            this.cbcategory = new System.Windows.Forms.ComboBox();
            this.cbsupplier = new System.Windows.Forms.ComboBox();
            this.txtleadtime = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.chkup = new System.Windows.Forms.RadioButton();
            this.chkupcn = new System.Windows.Forms.RadioButton();
            this.chkupjp = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Khmer OS Battambang", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSave.Location = new System.Drawing.Point(385, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 43);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Add";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // txtcode
            // 
            this.txtcode.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcode.Location = new System.Drawing.Point(64, 93);
            this.txtcode.Name = "txtcode";
            this.txtcode.Size = new System.Drawing.Size(161, 32);
            this.txtcode.TabIndex = 3;
            // 
            // txtpnumber
            // 
            this.txtpnumber.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpnumber.Location = new System.Drawing.Point(64, 155);
            this.txtpnumber.Name = "txtpnumber";
            this.txtpnumber.Size = new System.Drawing.Size(161, 32);
            this.txtpnumber.TabIndex = 3;
            // 
            // txtpname
            // 
            this.txtpname.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpname.Location = new System.Drawing.Point(64, 217);
            this.txtpname.Name = "txtpname";
            this.txtpname.Size = new System.Drawing.Size(161, 32);
            this.txtpname.TabIndex = 3;
            // 
            // cbusefor
            // 
            this.cbusefor.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbusefor.FormattingEnabled = true;
            this.cbusefor.Items.AddRange(new object[] {
            "",
            "Air Compressor",
            "All Applicator",
            "All Machine ",
            "CPR-ZERO",
            "XPR-ZERO-ST",
            "DECAM7",
            "Double End",
            "DWS-1900",
            "JN07-SDII",
            "JN07SDW",
            "JN07SDW-ST",
            "JN07SDW-ST/JN07SDII",
            "JST Semi Press",
            "MS-01 Plus II",
            "Pneumatic crimping",
            "Semi Crimping ",
            "Semi Press",
            "SK-A20",
            "Unistrip 2015",
            "Unistrip 2016",
            "Unistrip 2017",
            "Wire feeder",
            "WRS18",
            "WRS200R",
            "WRS20DR",
            "WRS-BT01ST"});
            this.cbusefor.Location = new System.Drawing.Point(66, 31);
            this.cbusefor.Name = "cbusefor";
            this.cbusefor.Size = new System.Drawing.Size(159, 32);
            this.cbusefor.TabIndex = 4;
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
            this.cbmaker.Location = new System.Drawing.Point(64, 279);
            this.cbmaker.Name = "cbmaker";
            this.cbmaker.Size = new System.Drawing.Size(159, 32);
            this.cbmaker.TabIndex = 4;
            // 
            // txtsafetystock
            // 
            this.txtsafetystock.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtsafetystock.Location = new System.Drawing.Point(64, 341);
            this.txtsafetystock.Name = "txtsafetystock";
            this.txtsafetystock.Size = new System.Drawing.Size(84, 32);
            this.txtsafetystock.TabIndex = 3;
            // 
            // txtmoq
            // 
            this.txtmoq.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtmoq.Location = new System.Drawing.Point(165, 341);
            this.txtmoq.Name = "txtmoq";
            this.txtmoq.Size = new System.Drawing.Size(84, 32);
            this.txtmoq.TabIndex = 3;
            // 
            // txtunitprice
            // 
            this.txtunitprice.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtunitprice.Location = new System.Drawing.Point(265, 341);
            this.txtunitprice.Name = "txtunitprice";
            this.txtunitprice.Size = new System.Drawing.Size(84, 32);
            this.txtunitprice.TabIndex = 3;
            // 
            // cbcodeprefix
            // 
            this.cbcodeprefix.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbcodeprefix.FormattingEnabled = true;
            this.cbcodeprefix.Items.AddRange(new object[] {
            "",
            "AP",
            "C",
            "D",
            "E",
            "J",
            "M",
            "N",
            "S",
            "U"});
            this.cbcodeprefix.Location = new System.Drawing.Point(255, 93);
            this.cbcodeprefix.Name = "cbcodeprefix";
            this.cbcodeprefix.Size = new System.Drawing.Size(94, 32);
            this.cbcodeprefix.TabIndex = 4;
            // 
            // cbcategory
            // 
            this.cbcategory.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbcategory.FormattingEnabled = true;
            this.cbcategory.Items.AddRange(new object[] {
            "",
            "Applicator Part",
            "Auto Machine Part",
            "Semi Machine Part",
            "UniStrip Machine Part",
            "Air Compressor",
            "Casting Machine"});
            this.cbcategory.Location = new System.Drawing.Point(255, 155);
            this.cbcategory.Name = "cbcategory";
            this.cbcategory.Size = new System.Drawing.Size(159, 32);
            this.cbcategory.TabIndex = 4;
            // 
            // cbsupplier
            // 
            this.cbsupplier.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbsupplier.FormattingEnabled = true;
            this.cbsupplier.Items.AddRange(new object[] {
            "",
            "IPO",
            "Hear office"});
            this.cbsupplier.Location = new System.Drawing.Point(255, 217);
            this.cbsupplier.Name = "cbsupplier";
            this.cbsupplier.Size = new System.Drawing.Size(159, 32);
            this.cbsupplier.TabIndex = 4;
            // 
            // txtleadtime
            // 
            this.txtleadtime.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtleadtime.Location = new System.Drawing.Point(255, 279);
            this.txtleadtime.Name = "txtleadtime";
            this.txtleadtime.Size = new System.Drawing.Size(159, 32);
            this.txtleadtime.TabIndex = 3;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Khmer OS Battambang", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClear.Location = new System.Drawing.Point(385, 61);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(90, 43);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(62, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "Use For";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(62, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "New Code";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(251, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 24);
            this.label3.TabIndex = 6;
            this.label3.Text = "Code Prefix";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(62, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 24);
            this.label4.TabIndex = 6;
            this.label4.Text = "Part Number";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(251, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 24);
            this.label5.TabIndex = 6;
            this.label5.Text = "Category";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(63, 190);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 24);
            this.label6.TabIndex = 6;
            this.label6.Text = "Part Name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(251, 190);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 24);
            this.label7.TabIndex = 6;
            this.label7.Text = "Supplier";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(62, 252);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 24);
            this.label8.TabIndex = 6;
            this.label8.Text = "Maker";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(252, 252);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 24);
            this.label9.TabIndex = 6;
            this.label9.Text = "Lead Time";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(62, 314);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(87, 24);
            this.label10.TabIndex = 6;
            this.label10.Text = "Safety Stock";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(274, 314);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(66, 24);
            this.label11.TabIndex = 6;
            this.label11.Text = "Unit Price";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(183, 314);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(42, 24);
            this.label12.TabIndex = 6;
            this.label12.Text = "MOQ";
            // 
            // chkup
            // 
            this.chkup.AutoSize = true;
            this.chkup.Location = new System.Drawing.Point(367, 330);
            this.chkup.Name = "chkup";
            this.chkup.Size = new System.Drawing.Size(31, 17);
            this.chkup.TabIndex = 7;
            this.chkup.TabStop = true;
            this.chkup.Text = "$";
            this.chkup.UseVisualStyleBackColor = true;
            // 
            // chkupcn
            // 
            this.chkupcn.AutoSize = true;
            this.chkupcn.Location = new System.Drawing.Point(367, 350);
            this.chkupcn.Name = "chkupcn";
            this.chkupcn.Size = new System.Drawing.Size(46, 17);
            this.chkupcn.TabIndex = 7;
            this.chkupcn.TabStop = true;
            this.chkupcn.Text = "CN¥";
            this.chkupcn.UseVisualStyleBackColor = true;
            // 
            // chkupjp
            // 
            this.chkupjp.AutoSize = true;
            this.chkupjp.Location = new System.Drawing.Point(367, 373);
            this.chkupjp.Name = "chkupjp";
            this.chkupjp.Size = new System.Drawing.Size(43, 17);
            this.chkupjp.TabIndex = 7;
            this.chkupjp.TabStop = true;
            this.chkupjp.Text = "JP¥";
            this.chkupjp.UseVisualStyleBackColor = true;
            // 
            // RegUpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(530, 404);
            this.Controls.Add(this.chkupjp);
            this.Controls.Add(this.chkupcn);
            this.Controls.Add(this.chkup);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbsupplier);
            this.Controls.Add(this.cbcategory);
            this.Controls.Add(this.cbcodeprefix);
            this.Controls.Add(this.cbmaker);
            this.Controls.Add(this.cbusefor);
            this.Controls.Add(this.txtleadtime);
            this.Controls.Add(this.txtpname);
            this.Controls.Add(this.txtpnumber);
            this.Controls.Add(this.txtunitprice);
            this.Controls.Add(this.txtmoq);
            this.Controls.Add(this.txtsafetystock);
            this.Controls.Add(this.txtcode);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSave);
            this.Name = "RegUpdateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Register";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtcode;
        private System.Windows.Forms.TextBox txtpnumber;
        private System.Windows.Forms.TextBox txtpname;
        private System.Windows.Forms.ComboBox cbusefor;
        private System.Windows.Forms.ComboBox cbmaker;
        private System.Windows.Forms.TextBox txtsafetystock;
        private System.Windows.Forms.TextBox txtmoq;
        private System.Windows.Forms.TextBox txtunitprice;
        private System.Windows.Forms.ComboBox cbcodeprefix;
        private System.Windows.Forms.ComboBox cbcategory;
        private System.Windows.Forms.ComboBox cbsupplier;
        private System.Windows.Forms.TextBox txtleadtime;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RadioButton chkup;
        private System.Windows.Forms.RadioButton chkupcn;
        private System.Windows.Forms.RadioButton chkupjp;
    }
}