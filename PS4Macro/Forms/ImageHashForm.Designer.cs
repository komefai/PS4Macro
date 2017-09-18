namespace PS4Macro.Forms
{
    partial class ImageHashForm
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
            this.compareButton = new System.Windows.Forms.Button();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.imageAGroupBox = new System.Windows.Forms.GroupBox();
            this.imageAPictureBox = new System.Windows.Forms.PictureBox();
            this.imageBGroupBox = new System.Windows.Forms.GroupBox();
            this.imageBPictureBox = new System.Windows.Forms.PictureBox();
            this.imageAHashTextBox = new System.Windows.Forms.TextBox();
            this.imageBHashTextBox = new System.Windows.Forms.TextBox();
            this.bottomPanel.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.imageAGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageAPictureBox)).BeginInit();
            this.imageBGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // compareButton
            // 
            this.compareButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.compareButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.compareButton.ForeColor = System.Drawing.Color.MidnightBlue;
            this.compareButton.Location = new System.Drawing.Point(10, 5);
            this.compareButton.Name = "compareButton";
            this.compareButton.Size = new System.Drawing.Size(774, 30);
            this.compareButton.TabIndex = 9;
            this.compareButton.Text = "SIMILARITY";
            this.compareButton.UseVisualStyleBackColor = true;
            this.compareButton.Click += new System.EventHandler(this.compareButton_Click);
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.compareButton);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 361);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.bottomPanel.Size = new System.Drawing.Size(794, 40);
            this.bottomPanel.TabIndex = 14;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.imageBHashTextBox, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.imageAHashTextBox, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.imageBGroupBox, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.imageAGroupBox, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.87671F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.123288F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(794, 361);
            this.tableLayoutPanel.TabIndex = 18;
            // 
            // imageAGroupBox
            // 
            this.imageAGroupBox.Controls.Add(this.imageAPictureBox);
            this.imageAGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageAGroupBox.ForeColor = System.Drawing.Color.Orange;
            this.imageAGroupBox.Location = new System.Drawing.Point(10, 10);
            this.imageAGroupBox.Margin = new System.Windows.Forms.Padding(10);
            this.imageAGroupBox.Name = "imageAGroupBox";
            this.imageAGroupBox.Padding = new System.Windows.Forms.Padding(10);
            this.imageAGroupBox.Size = new System.Drawing.Size(377, 315);
            this.imageAGroupBox.TabIndex = 17;
            this.imageAGroupBox.TabStop = false;
            this.imageAGroupBox.Text = "Image A";
            // 
            // imageAPictureBox
            // 
            this.imageAPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageAPictureBox.Location = new System.Drawing.Point(10, 23);
            this.imageAPictureBox.Name = "imageAPictureBox";
            this.imageAPictureBox.Size = new System.Drawing.Size(357, 282);
            this.imageAPictureBox.TabIndex = 1;
            this.imageAPictureBox.TabStop = false;
            // 
            // imageBGroupBox
            // 
            this.imageBGroupBox.Controls.Add(this.imageBPictureBox);
            this.imageBGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBGroupBox.ForeColor = System.Drawing.Color.Orange;
            this.imageBGroupBox.Location = new System.Drawing.Point(407, 10);
            this.imageBGroupBox.Margin = new System.Windows.Forms.Padding(10);
            this.imageBGroupBox.Name = "imageBGroupBox";
            this.imageBGroupBox.Padding = new System.Windows.Forms.Padding(10);
            this.imageBGroupBox.Size = new System.Drawing.Size(377, 315);
            this.imageBGroupBox.TabIndex = 18;
            this.imageBGroupBox.TabStop = false;
            this.imageBGroupBox.Text = "Image B";
            // 
            // imageBPictureBox
            // 
            this.imageBPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBPictureBox.Location = new System.Drawing.Point(10, 23);
            this.imageBPictureBox.Name = "imageBPictureBox";
            this.imageBPictureBox.Size = new System.Drawing.Size(357, 282);
            this.imageBPictureBox.TabIndex = 1;
            this.imageBPictureBox.TabStop = false;
            // 
            // imageAHashTextBox
            // 
            this.imageAHashTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageAHashTextBox.Location = new System.Drawing.Point(15, 338);
            this.imageAHashTextBox.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.imageAHashTextBox.Name = "imageAHashTextBox";
            this.imageAHashTextBox.ReadOnly = true;
            this.imageAHashTextBox.Size = new System.Drawing.Size(367, 20);
            this.imageAHashTextBox.TabIndex = 19;
            // 
            // imageBHashTextBox
            // 
            this.imageBHashTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBHashTextBox.Location = new System.Drawing.Point(412, 338);
            this.imageBHashTextBox.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.imageBHashTextBox.Name = "imageBHashTextBox";
            this.imageBHashTextBox.ReadOnly = true;
            this.imageBHashTextBox.Size = new System.Drawing.Size(367, 20);
            this.imageBHashTextBox.TabIndex = 20;
            // 
            // ImageHashForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 401);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.bottomPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(542, 400);
            this.Name = "ImageHashForm";
            this.Text = "Image Hash Tool";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ImageHashForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ImageHashForm_DragEnter);
            this.bottomPanel.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.imageAGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageAPictureBox)).EndInit();
            this.imageBGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageBPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button compareButton;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.GroupBox imageBGroupBox;
        private System.Windows.Forms.PictureBox imageBPictureBox;
        private System.Windows.Forms.GroupBox imageAGroupBox;
        private System.Windows.Forms.PictureBox imageAPictureBox;
        private System.Windows.Forms.TextBox imageBHashTextBox;
        private System.Windows.Forms.TextBox imageAHashTextBox;
    }
}