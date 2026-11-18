namespace MachineDeptApp.MCPlans
{
    partial class DrawingItemForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnView = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panelSearchPart = new System.Windows.Forms.Panel();
            this.txtSearchPart = new System.Windows.Forms.TextBox();
            this.lblSearchPart = new System.Windows.Forms.Label();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.LbStatus = new System.Windows.Forms.Label();
            this.dgvItem = new System.Windows.Forms.DataGridView();
            this.ColID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColItemPart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCreate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPrepareBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPEChecker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColQAChecker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColApproval = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColUpdateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColUpdateBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelHeader.SuspendLayout();
            this.panelSearchPart.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItem)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.White;
            this.panelHeader.Controls.Add(this.btnView);
            this.panelHeader.Controls.Add(this.btnDelete);
            this.panelHeader.Controls.Add(this.btnUpdate);
            this.panelHeader.Controls.Add(this.btnNew);
            this.panelHeader.Controls.Add(this.btnSearch);
            this.panelHeader.Controls.Add(this.panelSearchPart);
            this.panelHeader.Controls.Add(this.lblSearchPart);
            this.panelHeader.Controls.Add(this.panelSearch);
            this.panelHeader.Controls.Add(this.lblSearch);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1147, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // btnView
            // 
            this.btnView.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnView.Enabled = false;
            this.btnView.FlatAppearance.BorderSize = 0;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnView.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.btnView.Location = new System.Drawing.Point(974, 12);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(96, 36);
            this.btnView.TabIndex = 8;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.btnDelete.Location = new System.Drawing.Point(870, 12);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(96, 36);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdate.Enabled = false;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.btnUpdate.Location = new System.Drawing.Point(766, 12);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(96, 36);
            this.btnUpdate.TabIndex = 6;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            // 
            // btnNew
            // 
            this.btnNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNew.FlatAppearance.BorderSize = 0;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.btnNew.Location = new System.Drawing.Point(662, 12);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(96, 36);
            this.btnNew.TabIndex = 5;
            this.btnNew.Text = "Create";
            this.btnNew.UseVisualStyleBackColor = false;
            // 
            // btnSearch
            // 
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.btnSearch.Location = new System.Drawing.Point(558, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(96, 36);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // panelSearchPart
            // 
            this.panelSearchPart.Controls.Add(this.txtSearchPart);
            this.panelSearchPart.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.panelSearchPart.Location = new System.Drawing.Point(366, 12);
            this.panelSearchPart.Name = "panelSearchPart";
            this.panelSearchPart.Size = new System.Drawing.Size(180, 36);
            this.panelSearchPart.TabIndex = 3;
            // 
            // txtSearchPart
            // 
            this.txtSearchPart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchPart.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSearchPart.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearchPart.Location = new System.Drawing.Point(12, 9);
            this.txtSearchPart.Name = "txtSearchPart";
            this.txtSearchPart.Size = new System.Drawing.Size(132, 18);
            this.txtSearchPart.TabIndex = 0;
            // 
            // lblSearchPart
            // 
            this.lblSearchPart.AutoSize = true;
            this.lblSearchPart.BackColor = System.Drawing.Color.Transparent;
            this.lblSearchPart.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblSearchPart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblSearchPart.Location = new System.Drawing.Point(290, 21);
            this.lblSearchPart.Name = "lblSearchPart";
            this.lblSearchPart.Size = new System.Drawing.Size(69, 17);
            this.lblSearchPart.TabIndex = 2;
            this.lblSearchPart.Text = "Wip Code:";
            // 
            // panelSearch
            // 
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.panelSearch.Location = new System.Drawing.Point(98, 12);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(180, 36);
            this.panelSearch.TabIndex = 1;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location = new System.Drawing.Point(12, 9);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(132, 18);
            this.txtSearch.TabIndex = 0;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.BackColor = System.Drawing.Color.Transparent;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblSearch.Location = new System.Drawing.Point(12, 21);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(80, 17);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Wip Name:";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.LbStatus);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 567);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(1147, 30);
            this.panelFooter.TabIndex = 2;
            // 
            // LbStatus
            // 
            this.LbStatus.AutoSize = true;
            this.LbStatus.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F);
            this.LbStatus.Location = new System.Drawing.Point(12, 3);
            this.LbStatus.Name = "LbStatus";
            this.LbStatus.Size = new System.Drawing.Size(0, 24);
            this.LbStatus.TabIndex = 0;
            // 
            // dgvItem
            // 
            this.dgvItem.AllowUserToAddRows = false;
            this.dgvItem.AllowUserToDeleteRows = false;
            this.dgvItem.AllowUserToResizeRows = false;
            this.dgvItem.BackgroundColor = System.Drawing.Color.White;
            this.dgvItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvItem.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(105)))), ((int)(((byte)(214)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(105)))), ((int)(((byte)(214)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItem.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvItem.ColumnHeadersHeight = 40;
            this.dgvItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvItem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColID,
            this.ColNo,
            this.ColItemName,
            this.ColItemPart,
            this.ColCreate,
            this.ColPrepareBy,
            this.ColPEChecker,
            this.ColQAChecker,
            this.ColApproval,
            this.ColUpdateDate,
            this.ColUpdateBy});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItem.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItem.EnableHeadersVisualStyles = false;
            this.dgvItem.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(222)))), ((int)(((byte)(235)))));
            this.dgvItem.Location = new System.Drawing.Point(0, 60);
            this.dgvItem.MultiSelect = false;
            this.dgvItem.Name = "dgvItem";
            this.dgvItem.ReadOnly = true;
            this.dgvItem.RowHeadersVisible = false;
            this.dgvItem.RowTemplate.Height = 30;
            this.dgvItem.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItem.Size = new System.Drawing.Size(1147, 507);
            this.dgvItem.TabIndex = 1;
            // 
            // ColID
            // 
            this.ColID.HeaderText = "ID";
            this.ColID.Name = "ColID";
            this.ColID.ReadOnly = true;
            this.ColID.Visible = false;
            // 
            // ColNo
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColNo.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColNo.HeaderText = "No.";
            this.ColNo.Name = "ColNo";
            this.ColNo.ReadOnly = true;
            this.ColNo.Width = 50;
            // 
            // ColItemName
            // 
            this.ColItemName.HeaderText = "Wip Name";
            this.ColItemName.Name = "ColItemName";
            this.ColItemName.ReadOnly = true;
            this.ColItemName.Width = 200;
            // 
            // ColItemPart
            // 
            this.ColItemPart.HeaderText = "Wip Code";
            this.ColItemPart.Name = "ColItemPart";
            this.ColItemPart.ReadOnly = true;
            this.ColItemPart.Width = 160;
            // 
            // ColCreate
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Format = "dd-MM-yyyy HH:mm";
            this.ColCreate.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColCreate.HeaderText = "Create";
            this.ColCreate.Name = "ColCreate";
            this.ColCreate.ReadOnly = true;
            this.ColCreate.Width = 140;
            // 
            // ColPrepareBy
            // 
            this.ColPrepareBy.HeaderText = "Prepare By";
            this.ColPrepareBy.Name = "ColPrepareBy";
            this.ColPrepareBy.ReadOnly = true;
            this.ColPrepareBy.Width = 120;
            // 
            // ColPEChecker
            // 
            this.ColPEChecker.HeaderText = "PE Checker";
            this.ColPEChecker.Name = "ColPEChecker";
            this.ColPEChecker.ReadOnly = true;
            this.ColPEChecker.Width = 120;
            // 
            // ColQAChecker
            // 
            this.ColQAChecker.HeaderText = "QA Checker";
            this.ColQAChecker.Name = "ColQAChecker";
            this.ColQAChecker.ReadOnly = true;
            this.ColQAChecker.Width = 120;
            // 
            // ColApproval
            // 
            this.ColApproval.HeaderText = "Approval";
            this.ColApproval.Name = "ColApproval";
            this.ColApproval.ReadOnly = true;
            this.ColApproval.Width = 120;
            // 
            // ColUpdateDate
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Format = "dd-MM-yyyy HH:mm";
            this.ColUpdateDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColUpdateDate.HeaderText = "Last Update";
            this.ColUpdateDate.Name = "ColUpdateDate";
            this.ColUpdateDate.ReadOnly = true;
            this.ColUpdateDate.Width = 140;
            // 
            // ColUpdateBy
            // 
            this.ColUpdateBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColUpdateBy.HeaderText = "Last Update By";
            this.ColUpdateBy.Name = "ColUpdateBy";
            this.ColUpdateBy.ReadOnly = true;
            // 
            // DrawingItemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1147, 597);
            this.Controls.Add(this.dgvItem);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.Name = "DrawingItemForm";
            this.Text = "Drawing Item";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelSearchPart.ResumeLayout(false);
            this.panelSearchPart.PerformLayout();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItem)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearchPart;
        private System.Windows.Forms.Panel panelSearchPart;
        private System.Windows.Forms.TextBox txtSearchPart;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Label LbStatus;
        private System.Windows.Forms.DataGridView dgvItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColItemPart;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCreate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPrepareBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPEChecker;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColQAChecker;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColApproval;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColUpdateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColUpdateBy;
    }
}
