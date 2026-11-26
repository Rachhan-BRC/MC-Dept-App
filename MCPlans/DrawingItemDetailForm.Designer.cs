namespace MachineDeptApp.MCPlans
{
    partial class DrawingItemDetailForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelTop = new System.Windows.Forms.Panel();
            this.tlpHeader = new System.Windows.Forms.TableLayoutPanel();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.lblDocumentNo = new System.Windows.Forms.Label();
            this.txtDocumentNo = new System.Windows.Forms.TextBox();
            this.lblDrawingNo = new System.Windows.Forms.Label();
            this.txtDrawingNo = new System.Windows.Forms.TextBox();
            this.lblItemName = new System.Windows.Forms.Label();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.lblItemPart = new System.Windows.Forms.Label();
            this.txtItemPart = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtRef = new System.Windows.Forms.TextBox();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.tlpFooter = new System.Windows.Forms.TableLayoutPanel();
            this.lblCompany = new System.Windows.Forms.Label();
            this.lblPrepareBy = new System.Windows.Forms.Label();
            this.lblPE = new System.Windows.Forms.Label();
            this.lblQA = new System.Windows.Forms.Label();
            this.lblApproval = new System.Windows.Forms.Label();
            this.lblEffectiveDate = new System.Windows.Forms.Label();
            this.lblRevNo = new System.Windows.Forms.Label();
            this.txtPrepareBy = new System.Windows.Forms.TextBox();
            this.txtPE = new System.Windows.Forms.TextBox();
            this.txtQA = new System.Windows.Forms.TextBox();
            this.txtApproval = new System.Windows.Forms.TextBox();
            this.dtpEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this.txtRevNo = new System.Windows.Forms.TextBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnAddRow = new System.Windows.Forms.Button();
            this.btnDeleteRow = new System.Windows.Forms.Button();
            this.btnSameArrow = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvDetail = new System.Windows.Forms.DataGridView();
            this.ColNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColWire = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCNAPin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCNAStrip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCNAPN = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColCNBStrip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCNBPN = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColCNBPin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelTop.SuspendLayout();
            this.tlpHeader.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.tlpFooter.SuspendLayout();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.tlpHeader);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Controls.Add(this.txtRef);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(10, 10);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1080, 140);
            this.panelTop.TabIndex = 0;
            // 
            // tlpHeader
            // 
            this.tlpHeader.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpHeader.ColumnCount = 6;
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.tlpHeader.Controls.Add(this.lblCustomer, 0, 0);
            this.tlpHeader.Controls.Add(this.txtCustomer, 1, 0);
            this.tlpHeader.Controls.Add(this.lblDocumentNo, 2, 0);
            this.tlpHeader.Controls.Add(this.txtDocumentNo, 3, 0);
            this.tlpHeader.Controls.Add(this.lblDrawingNo, 4, 0);
            this.tlpHeader.Controls.Add(this.txtDrawingNo, 5, 0);
            this.tlpHeader.Controls.Add(this.lblItemName, 0, 1);
            this.tlpHeader.Controls.Add(this.txtItemName, 1, 1);
            this.tlpHeader.Controls.Add(this.lblItemPart, 2, 1);
            this.tlpHeader.Controls.Add(this.txtItemPart, 3, 1);
            this.tlpHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpHeader.Location = new System.Drawing.Point(0, 55);
            this.tlpHeader.Name = "tlpHeader";
            this.tlpHeader.RowCount = 2;
            this.tlpHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpHeader.Size = new System.Drawing.Size(1080, 85);
            this.tlpHeader.TabIndex = 2;
            // 
            // lblCustomer
            // 
            this.lblCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCustomer.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCustomer.Location = new System.Drawing.Point(4, 1);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(101, 41);
            this.lblCustomer.TabIndex = 0;
            this.lblCustomer.Text = "Customer";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCustomer
            // 
            this.txtCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomer.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.txtCustomer.Location = new System.Drawing.Point(112, 8);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(208, 27);
            this.txtCustomer.TabIndex = 1;
            this.txtCustomer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDocumentNo
            // 
            this.lblDocumentNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDocumentNo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDocumentNo.Location = new System.Drawing.Point(327, 1);
            this.lblDocumentNo.Name = "lblDocumentNo";
            this.lblDocumentNo.Size = new System.Drawing.Size(122, 41);
            this.lblDocumentNo.TabIndex = 2;
            this.lblDocumentNo.Text = "Document No.";
            this.lblDocumentNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDocumentNo
            // 
            this.txtDocumentNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDocumentNo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.txtDocumentNo.Location = new System.Drawing.Point(456, 8);
            this.txtDocumentNo.Name = "txtDocumentNo";
            this.txtDocumentNo.Size = new System.Drawing.Size(251, 27);
            this.txtDocumentNo.TabIndex = 3;
            this.txtDocumentNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDrawingNo
            // 
            this.lblDrawingNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDrawingNo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDrawingNo.Location = new System.Drawing.Point(714, 1);
            this.lblDrawingNo.Name = "lblDrawingNo";
            this.lblDrawingNo.Size = new System.Drawing.Size(122, 41);
            this.lblDrawingNo.TabIndex = 4;
            this.lblDrawingNo.Text = "Drawing No.";
            this.lblDrawingNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDrawingNo
            // 
            this.txtDrawingNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDrawingNo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.txtDrawingNo.Location = new System.Drawing.Point(843, 8);
            this.txtDrawingNo.Name = "txtDrawingNo";
            this.txtDrawingNo.Size = new System.Drawing.Size(233, 27);
            this.txtDrawingNo.TabIndex = 5;
            this.txtDrawingNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblItemName
            // 
            this.lblItemName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblItemName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblItemName.Location = new System.Drawing.Point(4, 43);
            this.lblItemName.Name = "lblItemName";
            this.lblItemName.Size = new System.Drawing.Size(101, 41);
            this.lblItemName.TabIndex = 6;
            this.lblItemName.Text = "Wip Name *";
            this.lblItemName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtItemName
            // 
            this.txtItemName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtItemName.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtItemName.Location = new System.Drawing.Point(112, 50);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.Size = new System.Drawing.Size(208, 27);
            this.txtItemName.TabIndex = 7;
            this.txtItemName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblItemPart
            // 
            this.lblItemPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblItemPart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblItemPart.Location = new System.Drawing.Point(327, 43);
            this.lblItemPart.Name = "lblItemPart";
            this.lblItemPart.Size = new System.Drawing.Size(122, 41);
            this.lblItemPart.TabIndex = 8;
            this.lblItemPart.Text = "Wip Code";
            this.lblItemPart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtItemPart
            // 
            this.txtItemPart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtItemPart.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtItemPart.Location = new System.Drawing.Point(456, 50);
            this.txtItemPart.Name = "txtItemPart";
            this.txtItemPart.Size = new System.Drawing.Size(251, 27);
            this.txtItemPart.TabIndex = 9;
            this.txtItemPart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(0, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1080, 40);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Pin Assignment && Wire Length";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtRef
            // 
            this.txtRef.BackColor = System.Drawing.SystemColors.Control;
            this.txtRef.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRef.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtRef.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.txtRef.Location = new System.Drawing.Point(0, 0);
            this.txtRef.MaxLength = 200;
            this.txtRef.Name = "txtRef";
            this.txtRef.Size = new System.Drawing.Size(1080, 15);
            this.txtRef.TabIndex = 0;
            this.txtRef.Text = "REF: PE-RE-19 Rev: 01 Date: 08-02-2020";
            this.txtRef.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.tlpFooter);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(10, 543);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.panelFooter.Size = new System.Drawing.Size(1080, 100);
            this.panelFooter.TabIndex = 2;
            // 
            // tlpFooter
            // 
            this.tlpFooter.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpFooter.ColumnCount = 7;
            this.tlpFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31F));
            this.tlpFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.5F));
            this.tlpFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.5F));
            this.tlpFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.5F));
            this.tlpFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.5F));
            this.tlpFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.5F));
            this.tlpFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.5F));
            this.tlpFooter.Controls.Add(this.lblCompany, 0, 0);
            this.tlpFooter.Controls.Add(this.lblPrepareBy, 1, 0);
            this.tlpFooter.Controls.Add(this.lblPE, 2, 0);
            this.tlpFooter.Controls.Add(this.lblQA, 3, 0);
            this.tlpFooter.Controls.Add(this.lblApproval, 4, 0);
            this.tlpFooter.Controls.Add(this.lblEffectiveDate, 5, 0);
            this.tlpFooter.Controls.Add(this.lblRevNo, 6, 0);
            this.tlpFooter.Controls.Add(this.txtPrepareBy, 1, 1);
            this.tlpFooter.Controls.Add(this.txtPE, 2, 1);
            this.tlpFooter.Controls.Add(this.txtQA, 3, 1);
            this.tlpFooter.Controls.Add(this.txtApproval, 4, 1);
            this.tlpFooter.Controls.Add(this.dtpEffectiveDate, 5, 1);
            this.tlpFooter.Controls.Add(this.txtRevNo, 6, 1);
            this.tlpFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFooter.Location = new System.Drawing.Point(0, 10);
            this.tlpFooter.Name = "tlpFooter";
            this.tlpFooter.RowCount = 2;
            this.tlpFooter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpFooter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFooter.Size = new System.Drawing.Size(1080, 90);
            this.tlpFooter.TabIndex = 0;
            // 
            // lblCompany
            // 
            this.lblCompany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCompany.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblCompany.Location = new System.Drawing.Point(4, 1);
            this.lblCompany.Name = "lblCompany";
            this.tlpFooter.SetRowSpan(this.lblCompany, 2);
            this.lblCompany.Size = new System.Drawing.Size(326, 88);
            this.lblCompany.TabIndex = 0;
            this.lblCompany.Text = "MARUNIX (CAMBODIA) CO., LTD";
            this.lblCompany.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPrepareBy
            // 
            this.lblPrepareBy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPrepareBy.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblPrepareBy.Location = new System.Drawing.Point(337, 1);
            this.lblPrepareBy.Name = "lblPrepareBy";
            this.lblPrepareBy.Size = new System.Drawing.Size(117, 28);
            this.lblPrepareBy.TabIndex = 1;
            this.lblPrepareBy.Text = "Prepared by";
            this.lblPrepareBy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPE
            // 
            this.lblPE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPE.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblPE.Location = new System.Drawing.Point(461, 1);
            this.lblPE.Name = "lblPE";
            this.lblPE.Size = new System.Drawing.Size(117, 28);
            this.lblPE.TabIndex = 2;
            this.lblPE.Text = "PE";
            this.lblPE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblQA
            // 
            this.lblQA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQA.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblQA.Location = new System.Drawing.Point(585, 1);
            this.lblQA.Name = "lblQA";
            this.lblQA.Size = new System.Drawing.Size(117, 28);
            this.lblQA.TabIndex = 3;
            this.lblQA.Text = "QA";
            this.lblQA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblApproval
            // 
            this.lblApproval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblApproval.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblApproval.Location = new System.Drawing.Point(709, 1);
            this.lblApproval.Name = "lblApproval";
            this.lblApproval.Size = new System.Drawing.Size(117, 28);
            this.lblApproval.TabIndex = 4;
            this.lblApproval.Text = "Approved by";
            this.lblApproval.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblEffectiveDate
            // 
            this.lblEffectiveDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEffectiveDate.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblEffectiveDate.Location = new System.Drawing.Point(833, 1);
            this.lblEffectiveDate.Name = "lblEffectiveDate";
            this.lblEffectiveDate.Size = new System.Drawing.Size(117, 28);
            this.lblEffectiveDate.TabIndex = 5;
            this.lblEffectiveDate.Text = "Effective date";
            this.lblEffectiveDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRevNo
            // 
            this.lblRevNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRevNo.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblRevNo.Location = new System.Drawing.Point(957, 1);
            this.lblRevNo.Name = "lblRevNo";
            this.lblRevNo.Size = new System.Drawing.Size(119, 28);
            this.lblRevNo.TabIndex = 6;
            this.lblRevNo.Text = "Rev. No";
            this.lblRevNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPrepareBy
            // 
            this.txtPrepareBy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrepareBy.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F);
            this.txtPrepareBy.Location = new System.Drawing.Point(337, 43);
            this.txtPrepareBy.Name = "txtPrepareBy";
            this.txtPrepareBy.Size = new System.Drawing.Size(117, 32);
            this.txtPrepareBy.TabIndex = 7;
            this.txtPrepareBy.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtPE
            // 
            this.txtPE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPE.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F);
            this.txtPE.Location = new System.Drawing.Point(461, 43);
            this.txtPE.Name = "txtPE";
            this.txtPE.Size = new System.Drawing.Size(117, 32);
            this.txtPE.TabIndex = 8;
            this.txtPE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtQA
            // 
            this.txtQA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQA.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F);
            this.txtQA.Location = new System.Drawing.Point(585, 43);
            this.txtQA.Name = "txtQA";
            this.txtQA.Size = new System.Drawing.Size(117, 32);
            this.txtQA.TabIndex = 9;
            this.txtQA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtApproval
            // 
            this.txtApproval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtApproval.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F);
            this.txtApproval.Location = new System.Drawing.Point(709, 43);
            this.txtApproval.Name = "txtApproval";
            this.txtApproval.Size = new System.Drawing.Size(117, 32);
            this.txtApproval.TabIndex = 10;
            this.txtApproval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dtpEffectiveDate
            // 
            this.dtpEffectiveDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpEffectiveDate.CustomFormat = "dd-MM-yyyy";
            this.dtpEffectiveDate.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.dtpEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEffectiveDate.Location = new System.Drawing.Point(833, 47);
            this.dtpEffectiveDate.Name = "dtpEffectiveDate";
            this.dtpEffectiveDate.ShowCheckBox = true;
            this.dtpEffectiveDate.Size = new System.Drawing.Size(117, 25);
            this.dtpEffectiveDate.TabIndex = 11;
            // 
            // txtRevNo
            // 
            this.txtRevNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRevNo.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtRevNo.Location = new System.Drawing.Point(957, 47);
            this.txtRevNo.Name = "txtRevNo";
            this.txtRevNo.Size = new System.Drawing.Size(119, 25);
            this.txtRevNo.TabIndex = 12;
            this.txtRevNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnAddRow);
            this.panelButtons.Controls.Add(this.btnDeleteRow);
            this.panelButtons.Controls.Add(this.btnSameArrow);
            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Controls.Add(this.btnClose);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(10, 643);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1080, 50);
            this.panelButtons.TabIndex = 3;
            // 
            // btnAddRow
            // 
            this.btnAddRow.BackColor = System.Drawing.Color.White;
            this.btnAddRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddRow.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnAddRow.Location = new System.Drawing.Point(0, 10);
            this.btnAddRow.Name = "btnAddRow";
            this.btnAddRow.Size = new System.Drawing.Size(100, 32);
            this.btnAddRow.TabIndex = 0;
            this.btnAddRow.Text = "Add Row";
            this.btnAddRow.UseVisualStyleBackColor = false;
            // 
            // btnDeleteRow
            // 
            this.btnDeleteRow.BackColor = System.Drawing.Color.White;
            this.btnDeleteRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteRow.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnDeleteRow.Location = new System.Drawing.Point(106, 10);
            this.btnDeleteRow.Name = "btnDeleteRow";
            this.btnDeleteRow.Size = new System.Drawing.Size(100, 32);
            this.btnDeleteRow.TabIndex = 1;
            this.btnDeleteRow.Text = "Delete Row";
            this.btnDeleteRow.UseVisualStyleBackColor = false;
            // 
            // btnSameArrow
            // 
            this.btnSameArrow.BackColor = System.Drawing.Color.White;
            this.btnSameArrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSameArrow.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnSameArrow.Location = new System.Drawing.Point(212, 10);
            this.btnSameArrow.Name = "btnSameArrow";
            this.btnSameArrow.Size = new System.Drawing.Size(170, 32);
            this.btnSameArrow.TabIndex = 2;
            this.btnSameArrow.Text = "Same as above ↓ (Ctrl+D)";
            this.btnSameArrow.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(874, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 32);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnClose.Location = new System.Drawing.Point(980, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 32);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // dgvDetail
            // 
            this.dgvDetail.AllowUserToResizeRows = false;
            this.dgvDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDetail.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvDetail.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDetail.ColumnHeadersHeight = 80;
            this.dgvDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColNo,
            this.ColWire,
            this.ColColor,
            this.ColLength,
            this.ColCNAPin,
            this.ColCNAStrip,
            this.ColCNAPN,
            this.ColCNBStrip,
            this.ColCNBPN,
            this.ColCNBPin});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Khmer OS Battambang", 9.75F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDetail.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetail.EnableHeadersVisualStyles = false;
            this.dgvDetail.GridColor = System.Drawing.Color.Gray;
            this.dgvDetail.Location = new System.Drawing.Point(10, 150);
            this.dgvDetail.Name = "dgvDetail";
            this.dgvDetail.RowHeadersVisible = false;
            this.dgvDetail.Size = new System.Drawing.Size(1080, 393);
            this.dgvDetail.TabIndex = 1;
            // 
            // ColNo
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ColNo.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColNo.FillWeight = 5F;
            this.ColNo.HeaderText = "No.";
            this.ColNo.Name = "ColNo";
            this.ColNo.ReadOnly = true;
            this.ColNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColWire
            // 
            this.ColWire.FillWeight = 22F;
            this.ColWire.HeaderText = "Cable / Wire No.(MFR)";
            this.ColWire.Name = "ColWire";
            this.ColWire.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColColor
            // 
            this.ColColor.FillWeight = 9F;
            this.ColColor.HeaderText = "Color";
            this.ColColor.Name = "ColColor";
            this.ColColor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColLength
            // 
            this.ColLength.FillWeight = 8F;
            this.ColLength.HeaderText = "Length (mm)";
            this.ColLength.Name = "ColLength";
            this.ColLength.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColCNAPin
            // 
            this.ColCNAPin.FillWeight = 9F;
            this.ColCNAPin.HeaderText = "CN (pin)";
            this.ColCNAPin.Name = "ColCNAPin";
            this.ColCNAPin.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColCNAStrip
            // 
            this.ColCNAStrip.FillWeight = 8F;
            this.ColCNAStrip.HeaderText = "Strip (mm)";
            this.ColCNAStrip.Name = "ColCNAStrip";
            this.ColCNAStrip.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColCNAPN
            // 
            this.ColCNAPN.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.ColCNAPN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ColCNAPN.FillWeight = 13F;
            this.ColCNAPN.HeaderText = "P/N";
            this.ColCNAPN.Name = "ColCNAPN";
            this.ColCNAPN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColCNBStrip
            // 
            this.ColCNBStrip.FillWeight = 8F;
            this.ColCNBStrip.HeaderText = "Strip (mm)";
            this.ColCNBStrip.Name = "ColCNBStrip";
            this.ColCNBStrip.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColCNBPN
            // 
            this.ColCNBPN.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.ColCNBPN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ColCNBPN.FillWeight = 13F;
            this.ColCNBPN.HeaderText = "P/N";
            this.ColCNBPN.Name = "ColCNBPN";
            this.ColCNBPN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColCNBPin
            // 
            this.ColCNBPin.FillWeight = 9F;
            this.ColCNBPin.HeaderText = "CN (pin)";
            this.ColCNBPin.Name = "ColCNBPin";
            this.ColCNBPin.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DrawingItemDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1100, 703);
            this.Controls.Add(this.dgvDetail);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelTop);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(900, 550);
            this.Name = "DrawingItemDetailForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Drawing Item";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.tlpHeader.ResumeLayout(false);
            this.tlpHeader.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.tlpFooter.ResumeLayout(false);
            this.tlpFooter.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.TextBox txtRef;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel tlpHeader;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.Label lblDocumentNo;
        private System.Windows.Forms.TextBox txtDocumentNo;
        private System.Windows.Forms.Label lblDrawingNo;
        private System.Windows.Forms.TextBox txtDrawingNo;
        private System.Windows.Forms.Label lblItemName;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.Label lblItemPart;
        private System.Windows.Forms.TextBox txtItemPart;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.TableLayoutPanel tlpFooter;
        private System.Windows.Forms.Label lblCompany;
        private System.Windows.Forms.Label lblPrepareBy;
        private System.Windows.Forms.Label lblPE;
        private System.Windows.Forms.Label lblQA;
        private System.Windows.Forms.Label lblApproval;
        private System.Windows.Forms.Label lblEffectiveDate;
        private System.Windows.Forms.Label lblRevNo;
        private System.Windows.Forms.TextBox txtPrepareBy;
        private System.Windows.Forms.TextBox txtPE;
        private System.Windows.Forms.TextBox txtQA;
        private System.Windows.Forms.TextBox txtApproval;
        private System.Windows.Forms.DateTimePicker dtpEffectiveDate;
        private System.Windows.Forms.TextBox txtRevNo;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnAddRow;
        private System.Windows.Forms.Button btnDeleteRow;
        private System.Windows.Forms.Button btnSameArrow;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColWire;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCNAPin;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCNAStrip;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColCNAPN;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCNBStrip;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColCNBPN;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCNBPin;
    }
}
