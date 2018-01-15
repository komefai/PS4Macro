namespace PS4Macro.Forms
{
    partial class MacroCompressorForm
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
            this.inputGroupBox = new System.Windows.Forms.GroupBox();
            this.framesCountLabel = new System.Windows.Forms.Label();
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.exportButton = new System.Windows.Forms.Button();
            this.filterGroupBox = new System.Windows.Forms.GroupBox();
            this.deselectAllButton = new System.Windows.Forms.Button();
            this.selectAllButton = new System.Windows.Forms.Button();
            this.propertiesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.inputGroupBox.SuspendLayout();
            this.filterGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputGroupBox
            // 
            this.inputGroupBox.Controls.Add(this.framesCountLabel);
            this.inputGroupBox.Controls.Add(this.fileNameLabel);
            this.inputGroupBox.Location = new System.Drawing.Point(12, 4);
            this.inputGroupBox.Name = "inputGroupBox";
            this.inputGroupBox.Size = new System.Drawing.Size(191, 49);
            this.inputGroupBox.TabIndex = 3;
            this.inputGroupBox.TabStop = false;
            // 
            // framesCountLabel
            // 
            this.framesCountLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.framesCountLabel.Location = new System.Drawing.Point(6, 28);
            this.framesCountLabel.Name = "framesCountLabel";
            this.framesCountLabel.Size = new System.Drawing.Size(179, 15);
            this.framesCountLabel.TabIndex = 1;
            this.framesCountLabel.Text = "...";
            this.framesCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.framesCountLabel.Click += new System.EventHandler(this.fileNameLabel_Click);
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.Location = new System.Drawing.Point(6, 11);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(179, 15);
            this.fileNameLabel.TabIndex = 0;
            this.fileNameLabel.Text = "Select or drop a macro here (*.xml)";
            this.fileNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.fileNameLabel.Click += new System.EventHandler(this.fileNameLabel_Click);
            // 
            // exportButton
            // 
            this.exportButton.Enabled = false;
            this.exportButton.Location = new System.Drawing.Point(12, 306);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(191, 28);
            this.exportButton.TabIndex = 4;
            this.exportButton.Text = "Export Macro";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // filterGroupBox
            // 
            this.filterGroupBox.Controls.Add(this.deselectAllButton);
            this.filterGroupBox.Controls.Add(this.selectAllButton);
            this.filterGroupBox.Controls.Add(this.propertiesCheckedListBox);
            this.filterGroupBox.Location = new System.Drawing.Point(12, 58);
            this.filterGroupBox.Name = "filterGroupBox";
            this.filterGroupBox.Size = new System.Drawing.Size(191, 242);
            this.filterGroupBox.TabIndex = 5;
            this.filterGroupBox.TabStop = false;
            this.filterGroupBox.Text = "Include Filter";
            // 
            // deselectAllButton
            // 
            this.deselectAllButton.Location = new System.Drawing.Point(101, 17);
            this.deselectAllButton.Name = "deselectAllButton";
            this.deselectAllButton.Size = new System.Drawing.Size(75, 23);
            this.deselectAllButton.TabIndex = 5;
            this.deselectAllButton.Text = "Deselect All";
            this.deselectAllButton.UseVisualStyleBackColor = true;
            this.deselectAllButton.Click += new System.EventHandler(this.deselectAllButton_Click);
            // 
            // selectAllButton
            // 
            this.selectAllButton.Location = new System.Drawing.Point(15, 17);
            this.selectAllButton.Name = "selectAllButton";
            this.selectAllButton.Size = new System.Drawing.Size(75, 23);
            this.selectAllButton.TabIndex = 4;
            this.selectAllButton.Text = "Select All";
            this.selectAllButton.UseVisualStyleBackColor = true;
            this.selectAllButton.Click += new System.EventHandler(this.selectAllButton_Click);
            // 
            // propertiesCheckedListBox
            // 
            this.propertiesCheckedListBox.CheckOnClick = true;
            this.propertiesCheckedListBox.FormattingEnabled = true;
            this.propertiesCheckedListBox.Items.AddRange(new object[] {
            "ReportTimeStamp",
            "LX",
            "LY",
            "RX",
            "RY",
            "L2",
            "R2",
            "Triangle",
            "Circle",
            "Cross",
            "Square",
            "DPad_Up",
            "DPad_Down",
            "DPad_Left",
            "DPad_Right",
            "L1",
            "R3",
            "Share",
            "Options",
            "R1",
            "L3",
            "PS",
            "TouchButton",
            "Touch1",
            "Touch2",
            "TouchPacketCounter",
            "FrameCounter",
            "Battery",
            "IsCharging",
            "AccelX",
            "AccelY",
            "AccelZ",
            "GyroX",
            "GyroY",
            "GyroZ"});
            this.propertiesCheckedListBox.Location = new System.Drawing.Point(6, 48);
            this.propertiesCheckedListBox.Name = "propertiesCheckedListBox";
            this.propertiesCheckedListBox.Size = new System.Drawing.Size(179, 184);
            this.propertiesCheckedListBox.TabIndex = 3;
            this.propertiesCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.propertiesCheckedListBox_ItemCheck);
            // 
            // MacroCompressorForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(215, 341);
            this.Controls.Add(this.filterGroupBox);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.inputGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MacroCompressorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Macro Compressor";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MacroCompressorForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MacroCompressorForm_DragEnter);
            this.inputGroupBox.ResumeLayout(false);
            this.filterGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox inputGroupBox;
        private System.Windows.Forms.Label fileNameLabel;
        private System.Windows.Forms.Label framesCountLabel;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.GroupBox filterGroupBox;
        private System.Windows.Forms.Button deselectAllButton;
        private System.Windows.Forms.Button selectAllButton;
        private System.Windows.Forms.CheckedListBox propertiesCheckedListBox;
    }
}