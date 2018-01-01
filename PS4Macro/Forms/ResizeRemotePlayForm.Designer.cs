namespace PS4Macro.Forms
{
    partial class ResizeRemotePlayForm
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
            this.widthLabel = new System.Windows.Forms.Label();
            this.heightLabel = new System.Windows.Forms.Label();
            this.resizeButton = new System.Windows.Forms.Button();
            this.widthNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.heightNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // widthLabel
            // 
            this.widthLabel.Location = new System.Drawing.Point(16, 6);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(38, 23);
            this.widthLabel.TabIndex = 0;
            this.widthLabel.Text = "Width";
            this.widthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // heightLabel
            // 
            this.heightLabel.Location = new System.Drawing.Point(16, 31);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(38, 23);
            this.heightLabel.TabIndex = 2;
            this.heightLabel.Text = "Height";
            this.heightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // resizeButton
            // 
            this.resizeButton.Location = new System.Drawing.Point(12, 61);
            this.resizeButton.Name = "resizeButton";
            this.resizeButton.Size = new System.Drawing.Size(158, 23);
            this.resizeButton.TabIndex = 4;
            this.resizeButton.Text = "Resize";
            this.resizeButton.UseVisualStyleBackColor = true;
            this.resizeButton.Click += new System.EventHandler(this.resizeButton_Click);
            // 
            // widthNumericUpDown
            // 
            this.widthNumericUpDown.Location = new System.Drawing.Point(88, 9);
            this.widthNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.widthNumericUpDown.Name = "widthNumericUpDown";
            this.widthNumericUpDown.Size = new System.Drawing.Size(82, 20);
            this.widthNumericUpDown.TabIndex = 5;
            // 
            // heightNumericUpDown
            // 
            this.heightNumericUpDown.Location = new System.Drawing.Point(88, 34);
            this.heightNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.heightNumericUpDown.Name = "heightNumericUpDown";
            this.heightNumericUpDown.Size = new System.Drawing.Size(82, 20);
            this.heightNumericUpDown.TabIndex = 6;
            // 
            // ResizeRemotePlayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(182, 96);
            this.Controls.Add(this.heightNumericUpDown);
            this.Controls.Add(this.widthNumericUpDown);
            this.Controls.Add(this.resizeButton);
            this.Controls.Add(this.heightLabel);
            this.Controls.Add(this.widthLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResizeRemotePlayForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Resize";
            this.Load += new System.EventHandler(this.ResizeRemotePlayForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.widthNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label widthLabel;
        private System.Windows.Forms.Label heightLabel;
        private System.Windows.Forms.Button resizeButton;
        private System.Windows.Forms.NumericUpDown widthNumericUpDown;
        private System.Windows.Forms.NumericUpDown heightNumericUpDown;
    }
}