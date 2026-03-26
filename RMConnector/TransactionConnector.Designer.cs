namespace MachineDeptApp.RMConnector
{
    partial class TransactionConnector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransactionConnector));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvRM = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceiveQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IssuedQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RegisterBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RegisterDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRM)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnEdit);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(910, 67);
            this.panel1.TabIndex = 1;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.Location = new System.Drawing.Point(174, 8);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(48, 51);
            this.btnDelete.TabIndex = 19;
            this.btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
            this.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAdd.Location = new System.Drawing.Point(120, 8);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(48, 51);
            this.btnAdd.TabIndex = 18;
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEdit.Location = new System.Drawing.Point(66, 9);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(48, 51);
            this.btnEdit.TabIndex = 17;
            this.btnEdit.UseVisualStyleBackColor = false;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearch.BackgroundImage")));
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.Location = new System.Drawing.Point(12, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(48, 51);
            this.btnSearch.TabIndex = 16;
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvRM);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 67);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(910, 414);
            this.panel2.TabIndex = 2;
            // 
            // dgvRM
            // 
            this.dgvRM.AllowUserToAddRows = false;
            this.dgvRM.AllowUserToDeleteRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Lavender;
            this.dgvRM.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvRM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRM.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.ItemCode,
            this.ItemName,
            this.ReceiveQty,
            this.IssuedQty,
            this.Type,
            this.Remark,
            this.Location,
            this.RegisterBy,
            this.RegisterDate});
            this.dgvRM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRM.Location = new System.Drawing.Point(0, 60);
            this.dgvRM.Name = "dgvRM";
            this.dgvRM.ReadOnly = true;
            this.dgvRM.RowHeadersVisible = false;
            this.dgvRM.Size = new System.Drawing.Size(910, 354);
            this.dgvRM.TabIndex = 2;
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // ItemCode
            // 
            this.ItemCode.HeaderText = "RM Code";
            this.ItemCode.Name = "ItemCode";
            this.ItemCode.ReadOnly = true;
            // 
            // ItemName
            // 
            this.ItemName.HeaderText = "RM Name";
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            // 
            // ReceiveQty
            // 
            this.ReceiveQty.HeaderText = "Receive Qty";
            this.ReceiveQty.Name = "ReceiveQty";
            this.ReceiveQty.ReadOnly = true;
            // 
            // IssuedQty
            // 
            this.IssuedQty.HeaderText = "Issued Qty";
            this.IssuedQty.Name = "IssuedQty";
            this.IssuedQty.ReadOnly = true;
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // Remark
            // 
            this.Remark.HeaderText = "Remarks";
            this.Remark.Name = "Remark";
            this.Remark.ReadOnly = true;
            // 
            // Location
            // 
            this.Location.HeaderText = "Location";
            this.Location.Name = "Location";
            this.Location.ReadOnly = true;
            // 
            // RegisterBy
            // 
            this.RegisterBy.HeaderText = "Register By";
            this.RegisterBy.Name = "RegisterBy";
            this.RegisterBy.ReadOnly = true;
            // 
            // RegisterDate
            // 
            this.RegisterDate.HeaderText = "Register Date";
            this.RegisterDate.Name = "RegisterDate";
            this.RegisterDate.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkDate);
            this.groupBox1.Controls.Add(this.dtpFrom);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dtpTo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbDescription);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbCode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(910, 60);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Condition";
            //
            // chkDate
            //
            this.chkDate.AutoSize = true;
            this.chkDate.Location = new System.Drawing.Point(470, 26);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(93, 23);
            this.chkDate.TabIndex = 4;
            this.chkDate.Text = "Date Range";
            this.chkDate.UseVisualStyleBackColor = true;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(572, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 19);
            this.label3.TabIndex = 5;
            this.label3.Text = "From";
            //
            // dtpFrom
            //
            this.dtpFrom.Enabled = false;
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(615, 23);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(120, 28);
            this.dtpFrom.TabIndex = 6;
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(742, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 19);
            this.label4.TabIndex = 7;
            this.label4.Text = "To";
            //
            // dtpTo
            //
            this.dtpTo.Enabled = false;
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(768, 23);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(120, 28);
            this.dtpTo.TabIndex = 8;
            //
            // tbDescription
            //
            this.tbDescription.Location = new System.Drawing.Point(311, 23);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(138, 28);
            this.tbDescription.TabIndex = 3;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(213, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "RM Description";
            //
            // tbCode
            //
            this.tbCode.Location = new System.Drawing.Point(66, 22);
            this.tbCode.Name = "tbCode";
            this.tbCode.Size = new System.Drawing.Size(123, 28);
            this.tbCode.TabIndex = 1;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Code";
            // 
            // TransactionConnector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 481);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Khmer OS Battambang", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TransactionConnector";
            this.Text = "វេរចេញ/ចូល Connector";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRM)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkDate;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgvRM;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceiveQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn IssuedQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn Location;
        private System.Windows.Forms.DataGridViewTextBoxColumn RegisterBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn RegisterDate;
    }
}