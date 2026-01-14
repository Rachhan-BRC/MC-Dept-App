namespace MachineDeptApp
{
    partial class SDMstPic
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SDMstPic));
            this.panel2 = new System.Windows.Forms.Panel();
            this.lberror = new System.Windows.Forms.Label();
            this.lbfound = new System.Windows.Forms.Label();
            this.lbshow = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnSaveGrey = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panelRegUp = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtlbendno = new System.Windows.Forms.TextBox();
            this.txtlbstartno = new System.Windows.Forms.TextBox();
            this.txtpic = new System.Windows.Forms.TextBox();
            this.dgvPic = new System.Windows.Forms.DataGridView();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnAdd = new System.Windows.Forms.Button();
            this.location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pic = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbstartno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbendno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.regby = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.regdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updateby = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpdateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDeleteGray = new System.Windows.Forms.PictureBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelRegUp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteGray)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel2.Controls.Add(this.lberror);
            this.panel2.Controls.Add(this.lbfound);
            this.panel2.Controls.Add(this.lbshow);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 518);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(984, 43);
            this.panel2.TabIndex = 66;
            // 
            // lberror
            // 
            this.lberror.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lberror.AutoSize = true;
            this.lberror.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lberror.Location = new System.Drawing.Point(-838, 10);
            this.lberror.Name = "lberror";
            this.lberror.Size = new System.Drawing.Size(0, 24);
            this.lberror.TabIndex = 3;
            // 
            // lbfound
            // 
            this.lbfound.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbfound.AutoSize = true;
            this.lbfound.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbfound.Location = new System.Drawing.Point(849, 10);
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
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelHeader.Controls.Add(this.btnDeleteGray);
            this.panelHeader.Controls.Add(this.btnDelete);
            this.panelHeader.Controls.Add(this.btnAdd);
            this.panelHeader.Controls.Add(this.btnSearch);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(984, 68);
            this.panelHeader.TabIndex = 67;
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
            this.btnSearch.TabIndex = 38;
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnSaveGrey
            // 
            this.btnSaveGrey.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSaveGrey.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSaveGrey.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveGrey.BackgroundImage")));
            this.btnSaveGrey.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSaveGrey.Enabled = false;
            this.btnSaveGrey.Location = new System.Drawing.Point(920, 13);
            this.btnSaveGrey.Name = "btnSaveGrey";
            this.btnSaveGrey.Size = new System.Drawing.Size(37, 37);
            this.btnSaveGrey.TabIndex = 36;
            this.btnSaveGrey.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.Location = new System.Drawing.Point(920, 13);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(37, 37);
            this.btnSave.TabIndex = 35;
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // panelRegUp
            // 
            this.panelRegUp.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelRegUp.Controls.Add(this.label3);
            this.panelRegUp.Controls.Add(this.label2);
            this.panelRegUp.Controls.Add(this.label1);
            this.panelRegUp.Controls.Add(this.txtlbendno);
            this.panelRegUp.Controls.Add(this.txtlbstartno);
            this.panelRegUp.Controls.Add(this.txtpic);
            this.panelRegUp.Controls.Add(this.btnCancel);
            this.panelRegUp.Controls.Add(this.btnSaveGrey);
            this.panelRegUp.Controls.Add(this.btnSave);
            this.panelRegUp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelRegUp.Location = new System.Drawing.Point(0, 419);
            this.panelRegUp.Name = "panelRegUp";
            this.panelRegUp.Size = new System.Drawing.Size(984, 99);
            this.panelRegUp.TabIndex = 69;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(575, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 24);
            this.label3.TabIndex = 38;
            this.label3.Text = "ឡាប៊ែលបញ្ចប់";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(307, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 24);
            this.label2.TabIndex = 38;
            this.label2.Text = "ឡាប៊ែលចាប់ផ្ដើម";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 24);
            this.label1.TabIndex = 38;
            this.label1.Text = "ឈ្មោះអ្នកទទួលបន្ទុក";
            // 
            // txtlbendno
            // 
            this.txtlbendno.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtlbendno.Location = new System.Drawing.Point(667, 34);
            this.txtlbendno.Name = "txtlbendno";
            this.txtlbendno.Size = new System.Drawing.Size(136, 32);
            this.txtlbendno.TabIndex = 37;
            // 
            // txtlbstartno
            // 
            this.txtlbstartno.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtlbstartno.Location = new System.Drawing.Point(412, 32);
            this.txtlbstartno.Name = "txtlbstartno";
            this.txtlbstartno.Size = new System.Drawing.Size(136, 32);
            this.txtlbstartno.TabIndex = 37;
            // 
            // txtpic
            // 
            this.txtpic.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpic.Location = new System.Drawing.Point(152, 32);
            this.txtpic.Name = "txtpic";
            this.txtpic.Size = new System.Drawing.Size(136, 32);
            this.txtpic.TabIndex = 37;
            // 
            // dgvPic
            // 
            this.dgvPic.AllowUserToAddRows = false;
            this.dgvPic.AllowUserToDeleteRows = false;
            this.dgvPic.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvPic.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPic.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPic.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPic.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPic.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPic.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.location,
            this.pic,
            this.lbstartno,
            this.lbendno,
            this.regby,
            this.regdate,
            this.updateby,
            this.UpdateDate});
            this.dgvPic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPic.EnableHeadersVisualStyles = false;
            this.dgvPic.Location = new System.Drawing.Point(0, 68);
            this.dgvPic.Name = "dgvPic";
            this.dgvPic.ReadOnly = true;
            this.dgvPic.RowHeadersVisible = false;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Khmer OS Battambang", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White;
            this.dgvPic.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvPic.RowTemplate.Height = 25;
            this.dgvPic.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvPic.Size = new System.Drawing.Size(984, 351);
            this.dgvPic.TabIndex = 70;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
            this.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAdd.Location = new System.Drawing.Point(71, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(57, 56);
            this.btnAdd.TabIndex = 39;
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // location
            // 
            this.location.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.location.DefaultCellStyle = dataGridViewCellStyle3;
            this.location.HeaderText = "Location";
            this.location.Name = "location";
            this.location.ReadOnly = true;
            // 
            // pic
            // 
            this.pic.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.pic.HeaderText = "PIC";
            this.pic.Name = "pic";
            this.pic.ReadOnly = true;
            this.pic.Width = 120;
            // 
            // lbstartno
            // 
            this.lbstartno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.lbstartno.DefaultCellStyle = dataGridViewCellStyle4;
            this.lbstartno.HeaderText = "ឡាប៊ែលចាប់ផ្ដើម";
            this.lbstartno.Name = "lbstartno";
            this.lbstartno.ReadOnly = true;
            this.lbstartno.Width = 120;
            // 
            // lbendno
            // 
            this.lbendno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.lbendno.DefaultCellStyle = dataGridViewCellStyle5;
            this.lbendno.HeaderText = "ឡាប៊ែលបញ្ចប់ ";
            this.lbendno.Name = "lbendno";
            this.lbendno.ReadOnly = true;
            this.lbendno.Width = 120;
            // 
            // regby
            // 
            this.regby.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.regby.DefaultCellStyle = dataGridViewCellStyle6;
            this.regby.HeaderText = "Register By";
            this.regby.Name = "regby";
            this.regby.ReadOnly = true;
            // 
            // regdate
            // 
            this.regdate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle7.Format = "dd-MMM-yyyy hh:mm:ss";
            this.regdate.DefaultCellStyle = dataGridViewCellStyle7;
            this.regdate.HeaderText = "Register Date";
            this.regdate.Name = "regdate";
            this.regdate.ReadOnly = true;
            this.regdate.Width = 130;
            // 
            // updateby
            // 
            this.updateby.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.updateby.DefaultCellStyle = dataGridViewCellStyle8;
            this.updateby.HeaderText = "Update By";
            this.updateby.Name = "updateby";
            this.updateby.ReadOnly = true;
            // 
            // UpdateDate
            // 
            this.UpdateDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.Format = "dd-MMM-yyyy hh:mm:ss";
            this.UpdateDate.DefaultCellStyle = dataGridViewCellStyle9;
            this.UpdateDate.HeaderText = "Update Date";
            this.UpdateDate.Name = "UpdateDate";
            this.UpdateDate.ReadOnly = true;
            this.UpdateDate.Width = 130;
            // 
            // btnDeleteGray
            // 
            this.btnDeleteGray.Enabled = false;
            this.btnDeleteGray.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteGray.Image")));
            this.btnDeleteGray.Location = new System.Drawing.Point(134, 6);
            this.btnDeleteGray.Name = "btnDeleteGray";
            this.btnDeleteGray.Size = new System.Drawing.Size(47, 55);
            this.btnDeleteGray.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnDeleteGray.TabIndex = 41;
            this.btnDeleteGray.TabStop = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.Location = new System.Drawing.Point(134, 4);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(46, 56);
            this.btnDelete.TabIndex = 40;
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.Location = new System.Drawing.Point(920, 56);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(37, 37);
            this.btnCancel.TabIndex = 35;
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // SDMstPic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.dgvPic);
            this.Controls.Add(this.panelRegUp);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panel2);
            this.Name = "SDMstPic";
            this.Text = "អ្នកទទួលបន្ទុករាប់ស្តុក SD";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelRegUp.ResumeLayout(false);
            this.panelRegUp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteGray)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lberror;
        private System.Windows.Forms.Label lbfound;
        private System.Windows.Forms.Label lbshow;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnSaveGrey;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panelRegUp;
        private System.Windows.Forms.TextBox txtpic;
        public System.Windows.Forms.DataGridView dgvPic;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtlbendno;
        private System.Windows.Forms.TextBox txtlbstartno;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridViewTextBoxColumn location;
        private System.Windows.Forms.DataGridViewTextBoxColumn pic;
        private System.Windows.Forms.DataGridViewTextBoxColumn lbstartno;
        private System.Windows.Forms.DataGridViewTextBoxColumn lbendno;
        private System.Windows.Forms.DataGridViewTextBoxColumn regby;
        private System.Windows.Forms.DataGridViewTextBoxColumn regdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn updateby;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpdateDate;
        private System.Windows.Forms.PictureBox btnDeleteGray;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
    }
}