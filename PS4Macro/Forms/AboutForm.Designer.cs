namespace PS4Macro
{
    partial class AboutForm
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
            this.createdByLabel = new System.Windows.Forms.Label();
            this.bioLinkLabel = new System.Windows.Forms.LinkLabel();
            this.iconPictureBox = new System.Windows.Forms.PictureBox();
            this.titleLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // createdByLabel
            // 
            this.createdByLabel.AutoSize = true;
            this.createdByLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.createdByLabel.Location = new System.Drawing.Point(109, 53);
            this.createdByLabel.Name = "createdByLabel";
            this.createdByLabel.Size = new System.Drawing.Size(154, 20);
            this.createdByLabel.TabIndex = 0;
            this.createdByLabel.Text = "Created By: Komefai";
            // 
            // bioLinkLabel
            // 
            this.bioLinkLabel.AutoSize = true;
            this.bioLinkLabel.Location = new System.Drawing.Point(113, 76);
            this.bioLinkLabel.Name = "bioLinkLabel";
            this.bioLinkLabel.Size = new System.Drawing.Size(98, 13);
            this.bioLinkLabel.TabIndex = 1;
            this.bioLinkLabel.TabStop = true;
            this.bioLinkLabel.Text = "http://komefai.com";
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.BackgroundImage = global::PS4Macro.Properties.Resources.icon_img;
            this.iconPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.iconPictureBox.Location = new System.Drawing.Point(12, 12);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new System.Drawing.Size(84, 87);
            this.iconPictureBox.TabIndex = 2;
            this.iconPictureBox.TabStop = false;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.titleLabel.Location = new System.Drawing.Point(109, 12);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(102, 24);
            this.titleLabel.TabIndex = 3;
            this.titleLabel.Text = "PS4 Macro";
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 110);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.iconPictureBox);
            this.Controls.Add(this.bioLinkLabel);
            this.Controls.Add(this.createdByLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label createdByLabel;
        private System.Windows.Forms.LinkLabel bioLinkLabel;
        private System.Windows.Forms.PictureBox iconPictureBox;
        private System.Windows.Forms.Label titleLabel;
    }
}