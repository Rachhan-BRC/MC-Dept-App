namespace MachineDeptApp
{
    partial class MenuFormV2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuFormV2));
            this.panelFooter = new System.Windows.Forms.Panel();
            this.LbLoginID = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCredit = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelHeaderTable = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.LbLogo = new System.Windows.Forms.Label();
            this.btnCheckForUpdate = new System.Windows.Forms.Button();
            this.btnHideHeader = new System.Windows.Forms.Button();
            this.btnUnhideHeader = new System.Windows.Forms.Button();
            this.treeViewMenu = new System.Windows.Forms.TreeView();
            this.imageListTreeview = new System.Windows.Forms.ImageList(this.components);
            this.btnHide = new System.Windows.Forms.Button();
            this.btnUnhide = new System.Windows.Forms.Button();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.splitterMenuVSMdi = new System.Windows.Forms.Splitter();
            this.panelOpenTab = new System.Windows.Forms.Panel();
            this.tabControlOpenForm = new System.Windows.Forms.TabControl();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.PicUpdate = new System.Windows.Forms.PictureBox();
            this.panelFooter.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelHeaderTable.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelOpenTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicUpdate)).BeginInit();
            this.SuspendLayout();
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.LbLoginID);
            this.panelFooter.Controls.Add(this.label2);
            this.panelFooter.Controls.Add(this.btnCredit);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 518);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(1184, 43);
            this.panelFooter.TabIndex = 0;
            // 
            // LbLoginID
            // 
            this.LbLoginID.AutoSize = true;
            this.LbLoginID.Font = new System.Drawing.Font("Khmer OS Battambang", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbLoginID.Location = new System.Drawing.Point(96, 8);
            this.LbLoginID.Name = "LbLoginID";
            this.LbLoginID.Size = new System.Drawing.Size(0, 27);
            this.LbLoginID.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Khmer OS Battambang", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 27);
            this.label2.TabIndex = 31;
            this.label2.Text = "#Login As :";
            // 
            // btnCredit
            // 
            this.btnCredit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCredit.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnCredit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCredit.BackgroundImage")));
            this.btnCredit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCredit.Location = new System.Drawing.Point(1127, 1);
            this.btnCredit.Name = "btnCredit";
            this.btnCredit.Size = new System.Drawing.Size(55, 41);
            this.btnCredit.TabIndex = 28;
            this.toolTip1.SetToolTip(this.btnCredit, "ក្រេដីត");
            this.btnCredit.UseVisualStyleBackColor = false;
            this.btnCredit.Click += new System.EventHandler(this.btnCredit_Click);
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.PicUpdate);
            this.panelHeader.Controls.Add(this.panelHeaderTable);
            this.panelHeader.Controls.Add(this.btnHideHeader);
            this.panelHeader.Controls.Add(this.btnUnhideHeader);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1184, 52);
            this.panelHeader.TabIndex = 1;
            // 
            // panelHeaderTable
            // 
            this.panelHeaderTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHeaderTable.ColumnCount = 3;
            this.panelHeaderTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelHeaderTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelHeaderTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelHeaderTable.Controls.Add(this.label1, 1, 0);
            this.panelHeaderTable.Controls.Add(this.LbLogo, 0, 0);
            this.panelHeaderTable.Controls.Add(this.btnCheckForUpdate, 2, 0);
            this.panelHeaderTable.Location = new System.Drawing.Point(4, 3);
            this.panelHeaderTable.Name = "panelHeaderTable";
            this.panelHeaderTable.RowCount = 1;
            this.panelHeaderTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelHeaderTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.panelHeaderTable.Size = new System.Drawing.Size(1144, 46);
            this.panelHeaderTable.TabIndex = 34;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Khmer OS Niroth", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(289, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(566, 46);
            this.label1.TabIndex = 34;
            this.label1.Text = "កម្មវិធីសម្រួលការគ្រប់គ្រងសម្រាប់ផ្នែកម៉ាស៊ីន";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // LbLogo
            // 
            this.LbLogo.AutoSize = true;
            this.LbLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LbLogo.Font = new System.Drawing.Font("Calibri", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbLogo.ForeColor = System.Drawing.SystemColors.Highlight;
            this.LbLogo.Location = new System.Drawing.Point(3, 0);
            this.LbLogo.Name = "LbLogo";
            this.LbLogo.Size = new System.Drawing.Size(280, 46);
            this.LbLogo.TabIndex = 33;
            this.LbLogo.Text = "MARUNIX";
            this.LbLogo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.LbLogo, "Marunix (Cambodia) Co., Ltd");
            // 
            // btnCheckForUpdate
            // 
            this.btnCheckForUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckForUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCheckForUpdate.BackgroundImage")));
            this.btnCheckForUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCheckForUpdate.Font = new System.Drawing.Font("Cambria", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckForUpdate.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.btnCheckForUpdate.Location = new System.Drawing.Point(1097, 3);
            this.btnCheckForUpdate.Name = "btnCheckForUpdate";
            this.btnCheckForUpdate.Size = new System.Drawing.Size(44, 40);
            this.btnCheckForUpdate.TabIndex = 35;
            this.btnCheckForUpdate.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.btnCheckForUpdate, "Check for the new update");
            this.btnCheckForUpdate.UseVisualStyleBackColor = true;
            // 
            // btnHideHeader
            // 
            this.btnHideHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHideHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHideHeader.BackgroundImage")));
            this.btnHideHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnHideHeader.Location = new System.Drawing.Point(1154, 27);
            this.btnHideHeader.Name = "btnHideHeader";
            this.btnHideHeader.Size = new System.Drawing.Size(27, 23);
            this.btnHideHeader.TabIndex = 0;
            this.btnHideHeader.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.btnHideHeader, "លាក់ក្បាលលើ (ផ្ទាំងឈ្មោះកម្មវិធី)");
            this.btnHideHeader.UseVisualStyleBackColor = true;
            this.btnHideHeader.Click += new System.EventHandler(this.btnHideHeader_Click);
            // 
            // btnUnhideHeader
            // 
            this.btnUnhideHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnhideHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUnhideHeader.BackgroundImage")));
            this.btnUnhideHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUnhideHeader.Location = new System.Drawing.Point(1154, 27);
            this.btnUnhideHeader.Name = "btnUnhideHeader";
            this.btnUnhideHeader.Size = new System.Drawing.Size(27, 23);
            this.btnUnhideHeader.TabIndex = 35;
            this.btnUnhideHeader.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.btnUnhideHeader, "បង្ហាញក្បាលលើ (ផ្ទាំងឈ្មោះកម្មវិធី)");
            this.btnUnhideHeader.UseVisualStyleBackColor = true;
            this.btnUnhideHeader.Visible = false;
            this.btnUnhideHeader.Click += new System.EventHandler(this.btnUnhideHeader_Click);
            // 
            // treeViewMenu
            // 
            this.treeViewMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewMenu.Font = new System.Drawing.Font("Khmer OS Battambang", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewMenu.ImageIndex = 0;
            this.treeViewMenu.ImageList = this.imageListTreeview;
            this.treeViewMenu.Location = new System.Drawing.Point(4, 30);
            this.treeViewMenu.Name = "treeViewMenu";
            this.treeViewMenu.SelectedImageIndex = 0;
            this.treeViewMenu.Size = new System.Drawing.Size(221, 427);
            this.treeViewMenu.TabIndex = 0;
            // 
            // imageListTreeview
            // 
            this.imageListTreeview.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeview.ImageStream")));
            this.imageListTreeview.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeview.Images.SetKeyName(0, "Boeun Rachhan System_logo.ico");
            this.imageListTreeview.Images.SetKeyName(1, "Group Icon");
            this.imageListTreeview.Images.SetKeyName(2, "Form Icon");
            this.imageListTreeview.Images.SetKeyName(3, "Form Selected Icon");
            // 
            // btnHide
            // 
            this.btnHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHide.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHide.BackgroundImage")));
            this.btnHide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnHide.Location = new System.Drawing.Point(199, 3);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(27, 23);
            this.btnHide.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btnHide, "លាក់ផ្ទាំងមុខងារ");
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // btnUnhide
            // 
            this.btnUnhide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnhide.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUnhide.BackgroundImage")));
            this.btnUnhide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUnhide.Location = new System.Drawing.Point(199, 3);
            this.btnUnhide.Name = "btnUnhide";
            this.btnUnhide.Size = new System.Drawing.Size(27, 23);
            this.btnUnhide.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btnUnhide, "បង្ហាញផ្ទាំងមុខងារ");
            this.btnUnhide.UseVisualStyleBackColor = true;
            this.btnUnhide.Visible = false;
            this.btnUnhide.Click += new System.EventHandler(this.btnUnhide_Click);
            // 
            // panelLeft
            // 
            this.panelLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelLeft.Controls.Add(this.btnHide);
            this.panelLeft.Controls.Add(this.treeViewMenu);
            this.panelLeft.Controls.Add(this.btnUnhide);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 52);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(233, 466);
            this.panelLeft.TabIndex = 7;
            // 
            // splitterMenuVSMdi
            // 
            this.splitterMenuVSMdi.Location = new System.Drawing.Point(233, 52);
            this.splitterMenuVSMdi.Name = "splitterMenuVSMdi";
            this.splitterMenuVSMdi.Size = new System.Drawing.Size(3, 466);
            this.splitterMenuVSMdi.TabIndex = 9;
            this.splitterMenuVSMdi.TabStop = false;
            // 
            // panelOpenTab
            // 
            this.panelOpenTab.Controls.Add(this.tabControlOpenForm);
            this.panelOpenTab.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOpenTab.Font = new System.Drawing.Font("Khmer OS Battambang", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelOpenTab.Location = new System.Drawing.Point(236, 491);
            this.panelOpenTab.Name = "panelOpenTab";
            this.panelOpenTab.Size = new System.Drawing.Size(948, 27);
            this.panelOpenTab.TabIndex = 11;
            this.panelOpenTab.Visible = false;
            // 
            // tabControlOpenForm
            // 
            this.tabControlOpenForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlOpenForm.Location = new System.Drawing.Point(0, 0);
            this.tabControlOpenForm.Name = "tabControlOpenForm";
            this.tabControlOpenForm.SelectedIndex = 0;
            this.tabControlOpenForm.Size = new System.Drawing.Size(948, 27);
            this.tabControlOpenForm.TabIndex = 0;
            this.toolTip1.SetToolTip(this.tabControlOpenForm, "មុខងារដែលធ្លាប់បានបើក");
            // 
            // PicUpdate
            // 
            this.PicUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PicUpdate.BackColor = System.Drawing.Color.White;
            this.PicUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PicUpdate.BackgroundImage")));
            this.PicUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PicUpdate.Location = new System.Drawing.Point(1135, 9);
            this.PicUpdate.Name = "PicUpdate";
            this.PicUpdate.Size = new System.Drawing.Size(7, 7);
            this.PicUpdate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicUpdate.TabIndex = 38;
            this.PicUpdate.TabStop = false;
            this.toolTip1.SetToolTip(this.PicUpdate, "New update are available");
            this.PicUpdate.Visible = false;
            // 
            // MenuFormV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1184, 561);
            this.Controls.Add(this.panelOpenTab);
            this.Controls.Add(this.splitterMenuVSMdi);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelFooter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MenuFormV2";
            this.Text = "MC Dept App";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MenuFormV2_Load);
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeaderTable.ResumeLayout(false);
            this.panelHeaderTable.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.panelOpenTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicUpdate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnCredit;
        private System.Windows.Forms.Label LbLoginID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView treeViewMenu;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.Button btnUnhide;
        private System.Windows.Forms.Splitter splitterMenuVSMdi;
        private System.Windows.Forms.Panel panelOpenTab;
        private System.Windows.Forms.TabControl tabControlOpenForm;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TableLayoutPanel panelHeaderTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LbLogo;
        private System.Windows.Forms.Button btnHideHeader;
        private System.Windows.Forms.ImageList imageListTreeview;
        private System.Windows.Forms.Button btnUnhideHeader;
        private System.Windows.Forms.Button btnCheckForUpdate;
        private System.Windows.Forms.PictureBox PicUpdate;
    }
}