namespace MachineDeptApp.Admin
{
    partial class UserAddForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserAddForm));
            this.PicPass = new System.Windows.Forms.PictureBox();
            this.PicUsername = new System.Windows.Forms.PictureBox();
            this.PicRole = new System.Windows.Forms.PictureBox();
            this.PicID = new System.Windows.Forms.PictureBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.CboRole = new System.Windows.Forms.ComboBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PicPass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicUsername)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicRole)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicID)).BeginInit();
            this.SuspendLayout();
            // 
            // PicPass
            // 
            this.PicPass.Image = ((System.Drawing.Image)(resources.GetObject("PicPass.Image")));
            this.PicPass.Location = new System.Drawing.Point(417, 222);
            this.PicPass.Name = "PicPass";
            this.PicPass.Size = new System.Drawing.Size(19, 20);
            this.PicPass.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicPass.TabIndex = 31;
            this.PicPass.TabStop = false;
            this.PicPass.Visible = false;
            // 
            // PicUsername
            // 
            this.PicUsername.Image = ((System.Drawing.Image)(resources.GetObject("PicUsername.Image")));
            this.PicUsername.Location = new System.Drawing.Point(417, 172);
            this.PicUsername.Name = "PicUsername";
            this.PicUsername.Size = new System.Drawing.Size(19, 20);
            this.PicUsername.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicUsername.TabIndex = 32;
            this.PicUsername.TabStop = false;
            this.PicUsername.Visible = false;
            // 
            // PicRole
            // 
            this.PicRole.Image = ((System.Drawing.Image)(resources.GetObject("PicRole.Image")));
            this.PicRole.Location = new System.Drawing.Point(417, 120);
            this.PicRole.Name = "PicRole";
            this.PicRole.Size = new System.Drawing.Size(19, 20);
            this.PicRole.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicRole.TabIndex = 33;
            this.PicRole.TabStop = false;
            this.PicRole.Visible = false;
            // 
            // PicID
            // 
            this.PicID.Image = ((System.Drawing.Image)(resources.GetObject("PicID.Image")));
            this.PicID.Location = new System.Drawing.Point(260, 67);
            this.PicID.Name = "PicID";
            this.PicID.Size = new System.Drawing.Size(19, 20);
            this.PicID.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicID.TabIndex = 34;
            this.PicID.TabStop = false;
            this.PicID.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOK.BackgroundImage")));
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOK.Location = new System.Drawing.Point(212, 259);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(59, 48);
            this.btnOK.TabIndex = 30;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // CboRole
            // 
            this.CboRole.Font = new System.Drawing.Font("Khmer OS Battambang", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CboRole.ForeColor = System.Drawing.SystemColors.MenuText;
            this.CboRole.FormattingEnabled = true;
            this.CboRole.Location = new System.Drawing.Point(124, 112);
            this.CboRole.Name = "CboRole";
            this.CboRole.Size = new System.Drawing.Size(287, 37);
            this.CboRole.TabIndex = 27;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Khmer OS Battambang", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(124, 215);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(287, 37);
            this.txtPassword.TabIndex = 29;
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Khmer OS Battambang", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.Location = new System.Drawing.Point(124, 164);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(287, 37);
            this.txtUsername.TabIndex = 28;
            // 
            // txtID
            // 
            this.txtID.Font = new System.Drawing.Font("Khmer OS Battambang", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtID.Location = new System.Drawing.Point(124, 60);
            this.txtID.MaxLength = 7;
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(132, 37);
            this.txtID.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 29);
            this.label5.TabIndex = 23;
            this.label5.Text = "Role";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 218);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 29);
            this.label4.TabIndex = 24;
            this.label4.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 29);
            this.label3.TabIndex = 25;
            this.label3.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 29);
            this.label2.TabIndex = 26;
            this.label2.Text = "ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cooper Black", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(120, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 21);
            this.label1.TabIndex = 21;
            this.label1.Text = "Add new user account";
            // 
            // UserAddForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(460, 322);
            this.Controls.Add(this.PicPass);
            this.Controls.Add(this.PicUsername);
            this.Controls.Add(this.PicRole);
            this.Controls.Add(this.PicID);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.CboRole);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Khmer OS Battambang", 12F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.MaximizeBox = false;
            this.Name = "UserAddForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add New User";
            this.Load += new System.EventHandler(this.UserAddForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PicPass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicUsername)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicRole)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PicPass;
        private System.Windows.Forms.PictureBox PicUsername;
        private System.Windows.Forms.PictureBox PicRole;
        private System.Windows.Forms.PictureBox PicID;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox CboRole;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}