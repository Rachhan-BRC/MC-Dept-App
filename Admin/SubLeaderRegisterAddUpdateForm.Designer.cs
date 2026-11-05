namespace MachineDeptApp.Admin
{
    partial class SubLeaderRegisterAddUpdateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubLeaderRegisterAddUpdateForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnSaveGRAY = new System.Windows.Forms.PictureBox();
            this.panelBody = new System.Windows.Forms.Panel();
            this.dtpStartD = new System.Windows.Forms.DateTimePicker();
            this.chkStartD = new System.Windows.Forms.CheckBox();
            this.cboPosition = new System.Windows.Forms.ComboBox();
            this.cboSex = new System.Windows.Forms.ComboBox();
            this.txtNFC = new System.Windows.Forms.TextBox();
            this.txtNameKH = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LbStartD = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cboStaff = new System.Windows.Forms.ComboBox();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveGRAY)).BeginInit();
            this.panelBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(48, 51);
            this.btnSave.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnSave, "Save");
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.btnSaveGRAY);
            this.panelHeader.Controls.Add(this.btnSave);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(498, 59);
            this.panelHeader.TabIndex = 4;
            // 
            // btnSaveGRAY
            // 
            this.btnSaveGRAY.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveGRAY.Image")));
            this.btnSaveGRAY.Location = new System.Drawing.Point(8, 8);
            this.btnSaveGRAY.Name = "btnSaveGRAY";
            this.btnSaveGRAY.Size = new System.Drawing.Size(40, 43);
            this.btnSaveGRAY.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnSaveGRAY.TabIndex = 17;
            this.btnSaveGRAY.TabStop = false;
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.dtpStartD);
            this.panelBody.Controls.Add(this.chkStartD);
            this.panelBody.Controls.Add(this.cboStaff);
            this.panelBody.Controls.Add(this.cboPosition);
            this.panelBody.Controls.Add(this.cboSex);
            this.panelBody.Controls.Add(this.txtNFC);
            this.panelBody.Controls.Add(this.txtNameKH);
            this.panelBody.Controls.Add(this.txtName);
            this.panelBody.Controls.Add(this.label4);
            this.panelBody.Controls.Add(this.txtID);
            this.panelBody.Controls.Add(this.label3);
            this.panelBody.Controls.Add(this.label2);
            this.panelBody.Controls.Add(this.LbStartD);
            this.panelBody.Controls.Add(this.label8);
            this.panelBody.Controls.Add(this.label6);
            this.panelBody.Controls.Add(this.label5);
            this.panelBody.Controls.Add(this.label7);
            this.panelBody.Controls.Add(this.label1);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelBody.Location = new System.Drawing.Point(0, 59);
            this.panelBody.Name = "panelBody";
            this.panelBody.Size = new System.Drawing.Size(498, 203);
            this.panelBody.TabIndex = 5;
            // 
            // dtpStartD
            // 
            this.dtpStartD.CustomFormat = "dd-MMM-yyyy";
            this.dtpStartD.Enabled = false;
            this.dtpStartD.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartD.Location = new System.Drawing.Point(113, 161);
            this.dtpStartD.Name = "dtpStartD";
            this.dtpStartD.Size = new System.Drawing.Size(136, 32);
            this.dtpStartD.TabIndex = 7;
            // 
            // chkStartD
            // 
            this.chkStartD.AutoSize = true;
            this.chkStartD.Location = new System.Drawing.Point(86, 169);
            this.chkStartD.Name = "chkStartD";
            this.chkStartD.Size = new System.Drawing.Size(15, 14);
            this.chkStartD.TabIndex = 6;
            this.chkStartD.UseVisualStyleBackColor = true;
            // 
            // cboPosition
            // 
            this.cboPosition.BackColor = System.Drawing.Color.White;
            this.cboPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPosition.FormattingEnabled = true;
            this.cboPosition.Items.AddRange(new object[] {
            "",
            "Sub Leader",
            "Leader"});
            this.cboPosition.Location = new System.Drawing.Point(340, 123);
            this.cboPosition.Name = "cboPosition";
            this.cboPosition.Size = new System.Drawing.Size(143, 32);
            this.cboPosition.TabIndex = 5;
            // 
            // cboSex
            // 
            this.cboSex.BackColor = System.Drawing.Color.White;
            this.cboSex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSex.FormattingEnabled = true;
            this.cboSex.Items.AddRange(new object[] {
            "",
            "ស្រី",
            "ប្រុស"});
            this.cboSex.Location = new System.Drawing.Point(113, 123);
            this.cboSex.Name = "cboSex";
            this.cboSex.Size = new System.Drawing.Size(136, 32);
            this.cboSex.TabIndex = 4;
            // 
            // txtNFC
            // 
            this.txtNFC.Location = new System.Drawing.Point(340, 9);
            this.txtNFC.Name = "txtNFC";
            this.txtNFC.Size = new System.Drawing.Size(143, 32);
            this.txtNFC.TabIndex = 1;
            // 
            // txtNameKH
            // 
            this.txtNameKH.Location = new System.Drawing.Point(113, 84);
            this.txtNameKH.Name = "txtNameKH";
            this.txtNameKH.Size = new System.Drawing.Size(370, 32);
            this.txtNameKH.TabIndex = 3;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(113, 46);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(370, 32);
            this.txtName.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 24);
            this.label4.TabIndex = 0;
            this.label4.Text = "ឈ្មោះជាភាសាខ្មែរ";
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(149, 8);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(100, 32);
            this.txtID.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 24);
            this.label3.TabIndex = 0;
            this.label3.Text = "ឈ្មោះ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(261, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 24);
            this.label2.TabIndex = 0;
            this.label2.Text = "NFC";
            // 
            // LbStartD
            // 
            this.LbStartD.AutoSize = true;
            this.LbStartD.Location = new System.Drawing.Point(10, 164);
            this.LbStartD.Name = "LbStartD";
            this.LbStartD.Size = new System.Drawing.Size(76, 24);
            this.LbStartD.TabIndex = 0;
            this.LbStartD.Text = "ថ្ងៃចូលធ្វើការ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(259, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 24);
            this.label6.TabIndex = 0;
            this.label6.Text = "មុខតំណែង";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 24);
            this.label5.TabIndex = 0;
            this.label5.Text = "ភេទ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(109, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 24);
            this.label7.TabIndex = 0;
            this.label7.Text = "MKH-";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "លេខកាត";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(259, 164);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 24);
            this.label8.TabIndex = 0;
            this.label8.Text = "គ្រប់គ្រងដោយ";
            // 
            // cboStaff
            // 
            this.cboStaff.BackColor = System.Drawing.Color.White;
            this.cboStaff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStaff.FormattingEnabled = true;
            this.cboStaff.Items.AddRange(new object[] {
            ""});
            this.cboStaff.Location = new System.Drawing.Point(340, 160);
            this.cboStaff.Name = "cboStaff";
            this.cboStaff.Size = new System.Drawing.Size(143, 32);
            this.cboStaff.TabIndex = 5;
            // 
            // SubLeaderRegisterAddUpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(498, 262);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SubLeaderRegisterAddUpdateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add New Sub-Leader";
            this.panelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveGRAY)).EndInit();
            this.panelBody.ResumeLayout(false);
            this.panelBody.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.PictureBox btnSaveGRAY;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNFC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNameKH;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label LbStartD;
        private System.Windows.Forms.ComboBox cboPosition;
        private System.Windows.Forms.ComboBox cboSex;
        private System.Windows.Forms.CheckBox chkStartD;
        private System.Windows.Forms.DateTimePicker dtpStartD;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboStaff;
        private System.Windows.Forms.Label label8;
    }
}