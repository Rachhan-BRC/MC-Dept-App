namespace MachineDeptApp.Inventory.Inprocess
{
    partial class InprocessCountingTypeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InprocessCountingTypeForm));
            this.PicConnector = new System.Windows.Forms.PictureBox();
            this.PicWIP = new System.Windows.Forms.PictureBox();
            this.PicWireTerminal = new System.Windows.Forms.PictureBox();
            this.LbConnector = new System.Windows.Forms.Label();
            this.LbWIP = new System.Windows.Forms.Label();
            this.LbWireTerminal = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PicConnector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicWIP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicWireTerminal)).BeginInit();
            this.SuspendLayout();
            // 
            // PicConnector
            // 
            this.PicConnector.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PicConnector.BackgroundImage")));
            this.PicConnector.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PicConnector.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PicConnector.Location = new System.Drawing.Point(21, 20);
            this.PicConnector.Name = "PicConnector";
            this.PicConnector.Size = new System.Drawing.Size(69, 65);
            this.PicConnector.TabIndex = 0;
            this.PicConnector.TabStop = false;
            // 
            // PicWIP
            // 
            this.PicWIP.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PicWIP.BackgroundImage")));
            this.PicWIP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PicWIP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PicWIP.Location = new System.Drawing.Point(21, 101);
            this.PicWIP.Name = "PicWIP";
            this.PicWIP.Size = new System.Drawing.Size(69, 65);
            this.PicWIP.TabIndex = 0;
            this.PicWIP.TabStop = false;
            // 
            // PicWireTerminal
            // 
            this.PicWireTerminal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PicWireTerminal.BackgroundImage")));
            this.PicWireTerminal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PicWireTerminal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PicWireTerminal.Location = new System.Drawing.Point(21, 183);
            this.PicWireTerminal.Name = "PicWireTerminal";
            this.PicWireTerminal.Size = new System.Drawing.Size(69, 65);
            this.PicWireTerminal.TabIndex = 0;
            this.PicWireTerminal.TabStop = false;
            // 
            // LbConnector
            // 
            this.LbConnector.AutoSize = true;
            this.LbConnector.BackColor = System.Drawing.Color.White;
            this.LbConnector.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LbConnector.Font = new System.Drawing.Font("Khmer OS Niroth", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbConnector.ForeColor = System.Drawing.Color.Black;
            this.LbConnector.Location = new System.Drawing.Point(96, 27);
            this.LbConnector.Name = "LbConnector";
            this.LbConnector.Size = new System.Drawing.Size(188, 52);
            this.LbConnector.TabIndex = 1;
            this.LbConnector.Text = "រាប់ខនិកទ័រ (ស្កេន)";
            // 
            // LbWIP
            // 
            this.LbWIP.AutoSize = true;
            this.LbWIP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LbWIP.Font = new System.Drawing.Font("Khmer OS Niroth", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbWIP.Location = new System.Drawing.Point(96, 108);
            this.LbWIP.Name = "LbWIP";
            this.LbWIP.Size = new System.Drawing.Size(157, 52);
            this.LbWIP.TabIndex = 1;
            this.LbWIP.Text = "រាប់សឺមី (ស្កេន)";
            // 
            // LbWireTerminal
            // 
            this.LbWireTerminal.AutoSize = true;
            this.LbWireTerminal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LbWireTerminal.Font = new System.Drawing.Font("Khmer OS Niroth", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbWireTerminal.Location = new System.Drawing.Point(96, 191);
            this.LbWireTerminal.Name = "LbWireTerminal";
            this.LbWireTerminal.Size = new System.Drawing.Size(350, 52);
            this.LbWireTerminal.TabIndex = 1;
            this.LbWireTerminal.Text = "រាប់ខ្សែភ្លើង/ធើមីណល (ថ្លឹង/គណនា)";
            // 
            // InprocessCountingTypeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(443, 268);
            this.Controls.Add(this.LbWireTerminal);
            this.Controls.Add(this.LbWIP);
            this.Controls.Add(this.LbConnector);
            this.Controls.Add(this.PicWireTerminal);
            this.Controls.Add(this.PicWIP);
            this.Controls.Add(this.PicConnector);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InprocessCountingTypeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "សូមជ្រើសរើសប្រភេទ";
            ((System.ComponentModel.ISupportInitialize)(this.PicConnector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicWIP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicWireTerminal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PicConnector;
        private System.Windows.Forms.PictureBox PicWIP;
        private System.Windows.Forms.PictureBox PicWireTerminal;
        private System.Windows.Forms.Label LbConnector;
        private System.Windows.Forms.Label LbWIP;
        private System.Windows.Forms.Label LbWireTerminal;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}