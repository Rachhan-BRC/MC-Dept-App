namespace MachineDeptApp.NG_Input
{
    partial class RegisterNG
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterNG));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnToAdd = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBoxRegisterList = new System.Windows.Forms.GroupBox();
            this.dgvNgRegisterList = new System.Windows.Forms.DataGridView();
            this.FGCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PosPNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PosPQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PosPDelDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WIPCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PosCNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SemiQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PosCQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PosCRemainQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PosCResultQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PosCStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KITPromise = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LabelStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrintStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tcRegisterNg = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tbPosPQty = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbWipCode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbDeliveryDate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbDoc = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAddRegisterNG = new System.Windows.Forms.Button();
            this.tbPosCQty = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPosScan = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBoxRegisterList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNgRegisterList)).BeginInit();
            this.tcRegisterNg.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Controls.Add(this.btnRemove);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnToAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1841, 90);
            this.panel1.TabIndex = 0;
            // 
            // btnRemove
            // 
            this.btnRemove.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRemove.BackgroundImage")));
            this.btnRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRemove.Location = new System.Drawing.Point(212, 9);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 75);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.Location = new System.Drawing.Point(119, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 75);
            this.btnSave.TabIndex = 1;
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnToAdd
            // 
            this.btnToAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnToAdd.BackgroundImage")));
            this.btnToAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnToAdd.Location = new System.Drawing.Point(25, 9);
            this.btnToAdd.Name = "btnToAdd";
            this.btnToAdd.Size = new System.Drawing.Size(75, 75);
            this.btnToAdd.TabIndex = 0;
            this.btnToAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnToAdd.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBoxRegisterList);
            this.panel2.Controls.Add(this.tcRegisterNg);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 90);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1841, 965);
            this.panel2.TabIndex = 1;
            // 
            // groupBoxRegisterList
            // 
            this.groupBoxRegisterList.Controls.Add(this.dgvNgRegisterList);
            this.groupBoxRegisterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxRegisterList.Location = new System.Drawing.Point(0, 0);
            this.groupBoxRegisterList.Name = "groupBoxRegisterList";
            this.groupBoxRegisterList.Size = new System.Drawing.Size(1841, 672);
            this.groupBoxRegisterList.TabIndex = 2;
            this.groupBoxRegisterList.TabStop = false;
            this.groupBoxRegisterList.Text = "groupBox2";
            // 
            // dgvNgRegisterList
            // 
            this.dgvNgRegisterList.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Kh Battambang", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            this.dgvNgRegisterList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvNgRegisterList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNgRegisterList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FGCode,
            this.PosPNo,
            this.PosPQty,
            this.PosPDelDate,
            this.WIPCode,
            this.PosCNo,
            this.SemiQty,
            this.PosCQty,
            this.PosCRemainQty,
            this.PosCResultQty,
            this.PosCStatus,
            this.Remarks,
            this.DocNo,
            this.KITPromise,
            this.LabelStatus,
            this.PrintStatus});
            this.dgvNgRegisterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNgRegisterList.Location = new System.Drawing.Point(3, 18);
            this.dgvNgRegisterList.Name = "dgvNgRegisterList";
            this.dgvNgRegisterList.RowHeadersWidth = 51;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Kh Battambang", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvNgRegisterList.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvNgRegisterList.RowTemplate.Height = 24;
            this.dgvNgRegisterList.Size = new System.Drawing.Size(1835, 651);
            this.dgvNgRegisterList.TabIndex = 0;
            // 
            // FGCode
            // 
            this.FGCode.HeaderText = "FGCode";
            this.FGCode.MinimumWidth = 6;
            this.FGCode.Name = "FGCode";
            this.FGCode.Width = 125;
            // 
            // PosPNo
            // 
            this.PosPNo.HeaderText = "PosPNo";
            this.PosPNo.MinimumWidth = 6;
            this.PosPNo.Name = "PosPNo";
            this.PosPNo.Width = 125;
            // 
            // PosPQty
            // 
            this.PosPQty.HeaderText = "PosPQty";
            this.PosPQty.MinimumWidth = 6;
            this.PosPQty.Name = "PosPQty";
            this.PosPQty.Width = 125;
            // 
            // PosPDelDate
            // 
            this.PosPDelDate.HeaderText = "PosPDelDate";
            this.PosPDelDate.MinimumWidth = 6;
            this.PosPDelDate.Name = "PosPDelDate";
            this.PosPDelDate.Width = 125;
            // 
            // WIPCode
            // 
            this.WIPCode.HeaderText = "WIPCode";
            this.WIPCode.MinimumWidth = 6;
            this.WIPCode.Name = "WIPCode";
            this.WIPCode.Width = 125;
            // 
            // PosCNo
            // 
            this.PosCNo.HeaderText = "PosCNo";
            this.PosCNo.MinimumWidth = 6;
            this.PosCNo.Name = "PosCNo";
            this.PosCNo.Width = 125;
            // 
            // SemiQty
            // 
            this.SemiQty.HeaderText = "SemiQty";
            this.SemiQty.MinimumWidth = 6;
            this.SemiQty.Name = "SemiQty";
            this.SemiQty.Width = 125;
            // 
            // PosCQty
            // 
            this.PosCQty.HeaderText = "PosCQty";
            this.PosCQty.MinimumWidth = 6;
            this.PosCQty.Name = "PosCQty";
            this.PosCQty.Width = 125;
            // 
            // PosCRemainQty
            // 
            this.PosCRemainQty.HeaderText = "PosCRemainQty";
            this.PosCRemainQty.MinimumWidth = 6;
            this.PosCRemainQty.Name = "PosCRemainQty";
            this.PosCRemainQty.Width = 125;
            // 
            // PosCResultQty
            // 
            this.PosCResultQty.HeaderText = "PosCResultQty";
            this.PosCResultQty.MinimumWidth = 6;
            this.PosCResultQty.Name = "PosCResultQty";
            this.PosCResultQty.Width = 125;
            // 
            // PosCStatus
            // 
            this.PosCStatus.HeaderText = "PosCStatus";
            this.PosCStatus.MinimumWidth = 6;
            this.PosCStatus.Name = "PosCStatus";
            this.PosCStatus.Width = 125;
            // 
            // Remarks
            // 
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.MinimumWidth = 6;
            this.Remarks.Name = "Remarks";
            this.Remarks.Width = 125;
            // 
            // DocNo
            // 
            this.DocNo.HeaderText = "DocNo";
            this.DocNo.MinimumWidth = 6;
            this.DocNo.Name = "DocNo";
            this.DocNo.Width = 125;
            // 
            // KITPromise
            // 
            this.KITPromise.HeaderText = "KITPromise";
            this.KITPromise.MinimumWidth = 6;
            this.KITPromise.Name = "KITPromise";
            this.KITPromise.Width = 125;
            // 
            // LabelStatus
            // 
            this.LabelStatus.HeaderText = "LabelStatus";
            this.LabelStatus.MinimumWidth = 6;
            this.LabelStatus.Name = "LabelStatus";
            this.LabelStatus.Width = 125;
            // 
            // PrintStatus
            // 
            this.PrintStatus.HeaderText = "PrintStatus";
            this.PrintStatus.MinimumWidth = 6;
            this.PrintStatus.Name = "PrintStatus";
            this.PrintStatus.Width = 125;
            // 
            // tcRegisterNg
            // 
            this.tcRegisterNg.Controls.Add(this.tabPage1);
            this.tcRegisterNg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tcRegisterNg.Font = new System.Drawing.Font("Kh Battambang", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcRegisterNg.Location = new System.Drawing.Point(0, 672);
            this.tcRegisterNg.Name = "tcRegisterNg";
            this.tcRegisterNg.SelectedIndex = 0;
            this.tcRegisterNg.Size = new System.Drawing.Size(1841, 293);
            this.tcRegisterNg.TabIndex = 0;
            this.tcRegisterNg.Visible = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbPosPQty);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.tbWipCode);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.tbDeliveryDate);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.tbDoc);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.btnCancel);
            this.tabPage1.Controls.Add(this.btnAddRegisterNG);
            this.tabPage1.Controls.Add(this.tbPosCQty);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.tbPosScan);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Font = new System.Drawing.Font("Kh Battambang", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 42);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1833, 247);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "បញ្ចូល NG";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tbPosPQty
            // 
            this.tbPosPQty.Location = new System.Drawing.Point(476, 100);
            this.tbPosPQty.Name = "tbPosPQty";
            this.tbPosPQty.Size = new System.Drawing.Size(199, 39);
            this.tbPosPQty.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(350, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 31);
            this.label7.TabIndex = 12;
            this.label7.Text = "POS(P) QTY";
            // 
            // tbWipCode
            // 
            this.tbWipCode.Location = new System.Drawing.Point(829, 100);
            this.tbWipCode.Name = "tbWipCode";
            this.tbWipCode.Size = new System.Drawing.Size(199, 39);
            this.tbWipCode.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(723, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 31);
            this.label6.TabIndex = 10;
            this.label6.Text = "Wip Code.";
            // 
            // tbDeliveryDate
            // 
            this.tbDeliveryDate.Location = new System.Drawing.Point(125, 100);
            this.tbDeliveryDate.Name = "tbDeliveryDate";
            this.tbDeliveryDate.Size = new System.Drawing.Size(199, 39);
            this.tbDeliveryDate.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 31);
            this.label5.TabIndex = 8;
            this.label5.Text = "Del. Date";
            // 
            // tbDoc
            // 
            this.tbDoc.FormattingEnabled = true;
            this.tbDoc.Items.AddRange(new object[] {
            "MOC",
            "NG",
            "WOV"});
            this.tbDoc.Location = new System.Drawing.Point(829, 22);
            this.tbDoc.Name = "tbDoc";
            this.tbDoc.Size = new System.Drawing.Size(199, 39);
            this.tbDoc.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(732, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 31);
            this.label3.TabIndex = 6;
            this.label3.Text = "NG Type";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.Location = new System.Drawing.Point(1741, 103);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnCancel.Size = new System.Drawing.Size(75, 75);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnAddRegisterNG
            // 
            this.btnAddRegisterNG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRegisterNG.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddRegisterNG.BackgroundImage")));
            this.btnAddRegisterNG.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddRegisterNG.Location = new System.Drawing.Point(1741, 5);
            this.btnAddRegisterNG.Name = "btnAddRegisterNG";
            this.btnAddRegisterNG.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnAddRegisterNG.Size = new System.Drawing.Size(75, 75);
            this.btnAddRegisterNG.TabIndex = 4;
            this.btnAddRegisterNG.UseVisualStyleBackColor = true;
            // 
            // tbPosCQty
            // 
            this.tbPosCQty.Location = new System.Drawing.Point(476, 22);
            this.tbPosCQty.Name = "tbPosCQty";
            this.tbPosCQty.Size = new System.Drawing.Size(199, 39);
            this.tbPosCQty.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(350, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 31);
            this.label2.TabIndex = 2;
            this.label2.Text = "POS(C) QTY";
            // 
            // tbPosScan
            // 
            this.tbPosScan.Location = new System.Drawing.Point(125, 22);
            this.tbPosScan.Name = "tbPosScan";
            this.tbPosScan.Size = new System.Drawing.Size(199, 39);
            this.tbPosScan.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "POS(C) ";
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 997);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1841, 58);
            this.panel3.TabIndex = 2;
            // 
            // RegisterNG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1841, 1055);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "RegisterNG";
            this.Text = "RegisterNG";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBoxRegisterList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNgRegisterList)).EndInit();
            this.tcRegisterNg.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnToAdd;
        private System.Windows.Forms.TabControl tcRegisterNg;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPosScan;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPosCQty;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAddRegisterNG;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox tbDoc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbDeliveryDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbWipCode;
        private System.Windows.Forms.TextBox tbPosPQty;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBoxRegisterList;
        private System.Windows.Forms.DataGridView dgvNgRegisterList;
        private System.Windows.Forms.DataGridViewTextBoxColumn FGCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PosPNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn PosPQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn PosPDelDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn WIPCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PosCNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn SemiQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn PosCQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn PosCRemainQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn PosCResultQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn PosCStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn KITPromise;
        private System.Windows.Forms.DataGridViewTextBoxColumn LabelStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrintStatus;
        private System.Windows.Forms.Button btnRemove;
    }
}