namespace PS4Macro.Forms
{
    partial class StatusCheckerForm
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
            this.interceptorTextLabel = new System.Windows.Forms.Label();
            this.interceptorStatusLabel = new System.Windows.Forms.Label();
            this.keyboardStatusLabel = new System.Windows.Forms.Label();
            this.keyboardTextLabel = new System.Windows.Forms.Label();
            this.mouseStatusLabel = new System.Windows.Forms.Label();
            this.mouseTextLabel = new System.Windows.Forms.Label();
            this.remotePlayProcessTextLabel = new System.Windows.Forms.Label();
            this.remotePlayProcessLabel = new System.Windows.Forms.Label();
            this.remotePlayHandleTextLabel = new System.Windows.Forms.Label();
            this.remotePlayHandleLabel = new System.Windows.Forms.Label();
            this.foregroundWindowTextLabel = new System.Windows.Forms.Label();
            this.foregroundWindowLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // interceptorTextLabel
            // 
            this.interceptorTextLabel.AutoSize = true;
            this.interceptorTextLabel.Location = new System.Drawing.Point(12, 104);
            this.interceptorTextLabel.Name = "interceptorTextLabel";
            this.interceptorTextLabel.Size = new System.Drawing.Size(61, 13);
            this.interceptorTextLabel.TabIndex = 0;
            this.interceptorTextLabel.Text = "Interceptor:";
            // 
            // interceptorStatusLabel
            // 
            this.interceptorStatusLabel.AutoSize = true;
            this.interceptorStatusLabel.Location = new System.Drawing.Point(90, 104);
            this.interceptorStatusLabel.Name = "interceptorStatusLabel";
            this.interceptorStatusLabel.Size = new System.Drawing.Size(67, 13);
            this.interceptorStatusLabel.TabIndex = 1;
            this.interceptorStatusLabel.Text = "Not Working";
            // 
            // keyboardStatusLabel
            // 
            this.keyboardStatusLabel.AutoSize = true;
            this.keyboardStatusLabel.Location = new System.Drawing.Point(90, 127);
            this.keyboardStatusLabel.Name = "keyboardStatusLabel";
            this.keyboardStatusLabel.Size = new System.Drawing.Size(67, 13);
            this.keyboardStatusLabel.TabIndex = 3;
            this.keyboardStatusLabel.Text = "Not Working";
            // 
            // keyboardTextLabel
            // 
            this.keyboardTextLabel.AutoSize = true;
            this.keyboardTextLabel.Location = new System.Drawing.Point(12, 127);
            this.keyboardTextLabel.Name = "keyboardTextLabel";
            this.keyboardTextLabel.Size = new System.Drawing.Size(55, 13);
            this.keyboardTextLabel.TabIndex = 2;
            this.keyboardTextLabel.Text = "Keyboard:";
            // 
            // mouseStatusLabel
            // 
            this.mouseStatusLabel.AutoSize = true;
            this.mouseStatusLabel.Location = new System.Drawing.Point(90, 150);
            this.mouseStatusLabel.Name = "mouseStatusLabel";
            this.mouseStatusLabel.Size = new System.Drawing.Size(67, 13);
            this.mouseStatusLabel.TabIndex = 5;
            this.mouseStatusLabel.Text = "Not Working";
            // 
            // mouseTextLabel
            // 
            this.mouseTextLabel.AutoSize = true;
            this.mouseTextLabel.Location = new System.Drawing.Point(12, 150);
            this.mouseTextLabel.Name = "mouseTextLabel";
            this.mouseTextLabel.Size = new System.Drawing.Size(42, 13);
            this.mouseTextLabel.TabIndex = 4;
            this.mouseTextLabel.Text = "Mouse:";
            // 
            // remotePlayProcessTextLabel
            // 
            this.remotePlayProcessTextLabel.AutoSize = true;
            this.remotePlayProcessTextLabel.Location = new System.Drawing.Point(12, 15);
            this.remotePlayProcessTextLabel.Name = "remotePlayProcessTextLabel";
            this.remotePlayProcessTextLabel.Size = new System.Drawing.Size(70, 13);
            this.remotePlayProcessTextLabel.TabIndex = 6;
            this.remotePlayProcessTextLabel.Text = "Remote Play:";
            // 
            // remotePlayProcessLabel
            // 
            this.remotePlayProcessLabel.AutoSize = true;
            this.remotePlayProcessLabel.Location = new System.Drawing.Point(90, 15);
            this.remotePlayProcessLabel.Name = "remotePlayProcessLabel";
            this.remotePlayProcessLabel.Size = new System.Drawing.Size(10, 13);
            this.remotePlayProcessLabel.TabIndex = 7;
            this.remotePlayProcessLabel.Text = "-";
            // 
            // remotePlayHandleTextLabel
            // 
            this.remotePlayHandleTextLabel.AutoSize = true;
            this.remotePlayHandleTextLabel.Location = new System.Drawing.Point(38, 37);
            this.remotePlayHandleTextLabel.Name = "remotePlayHandleTextLabel";
            this.remotePlayHandleTextLabel.Size = new System.Drawing.Size(44, 13);
            this.remotePlayHandleTextLabel.TabIndex = 8;
            this.remotePlayHandleTextLabel.Text = "Handle:";
            // 
            // remotePlayHandleLabel
            // 
            this.remotePlayHandleLabel.AutoSize = true;
            this.remotePlayHandleLabel.Location = new System.Drawing.Point(90, 37);
            this.remotePlayHandleLabel.Name = "remotePlayHandleLabel";
            this.remotePlayHandleLabel.Size = new System.Drawing.Size(10, 13);
            this.remotePlayHandleLabel.TabIndex = 9;
            this.remotePlayHandleLabel.Text = "-";
            // 
            // foregroundWindowTextLabel
            // 
            this.foregroundWindowTextLabel.AutoSize = true;
            this.foregroundWindowTextLabel.Location = new System.Drawing.Point(12, 70);
            this.foregroundWindowTextLabel.Name = "foregroundWindowTextLabel";
            this.foregroundWindowTextLabel.Size = new System.Drawing.Size(64, 13);
            this.foregroundWindowTextLabel.TabIndex = 10;
            this.foregroundWindowTextLabel.Text = "Foreground:";
            // 
            // foregroundWindowLabel
            // 
            this.foregroundWindowLabel.AutoSize = true;
            this.foregroundWindowLabel.Location = new System.Drawing.Point(90, 70);
            this.foregroundWindowLabel.Name = "foregroundWindowLabel";
            this.foregroundWindowLabel.Size = new System.Drawing.Size(10, 13);
            this.foregroundWindowLabel.TabIndex = 11;
            this.foregroundWindowLabel.Text = "-";
            // 
            // StatusCheckerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 181);
            this.Controls.Add(this.foregroundWindowLabel);
            this.Controls.Add(this.foregroundWindowTextLabel);
            this.Controls.Add(this.remotePlayHandleLabel);
            this.Controls.Add(this.remotePlayHandleTextLabel);
            this.Controls.Add(this.remotePlayProcessLabel);
            this.Controls.Add(this.remotePlayProcessTextLabel);
            this.Controls.Add(this.mouseStatusLabel);
            this.Controls.Add(this.mouseTextLabel);
            this.Controls.Add(this.keyboardStatusLabel);
            this.Controls.Add(this.keyboardTextLabel);
            this.Controls.Add(this.interceptorStatusLabel);
            this.Controls.Add(this.interceptorTextLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StatusCheckerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Status Checker";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StatusCheckerForm_FormClosed);
            this.Load += new System.EventHandler(this.StatusCheckerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label interceptorTextLabel;
        private System.Windows.Forms.Label interceptorStatusLabel;
        private System.Windows.Forms.Label keyboardStatusLabel;
        private System.Windows.Forms.Label keyboardTextLabel;
        private System.Windows.Forms.Label mouseStatusLabel;
        private System.Windows.Forms.Label mouseTextLabel;
        private System.Windows.Forms.Label remotePlayProcessTextLabel;
        private System.Windows.Forms.Label remotePlayProcessLabel;
        private System.Windows.Forms.Label remotePlayHandleTextLabel;
        private System.Windows.Forms.Label remotePlayHandleLabel;
        private System.Windows.Forms.Label foregroundWindowTextLabel;
        private System.Windows.Forms.Label foregroundWindowLabel;
    }
}