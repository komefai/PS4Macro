namespace PS4Macro.Forms
{
    partial class RemapperForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemapperForm));
            this.saveButton = new System.Windows.Forms.Button();
            this.macrosGroupBox = new System.Windows.Forms.GroupBox();
            this.macrosDataGridView = new System.Windows.Forms.DataGridView();
            this.Browse = new System.Windows.Forms.DataGridViewButtonColumn();
            this._Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mappingsGroupBox = new System.Windows.Forms.GroupBox();
            this.mappingsDataGridView = new System.Windows.Forms.DataGridView();
            this.Button = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mouseInputGroupBox = new System.Windows.Forms.GroupBox();
            this.movementJoystickLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rightStickRadioButton = new System.Windows.Forms.RadioButton();
            this.leftStickRadioButton = new System.Windows.Forms.RadioButton();
            this.rightMouseComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.leftMouseComboBox = new System.Windows.Forms.ComboBox();
            this.leftMouseLabel = new System.Windows.Forms.Label();
            this.enableMouseCheckBox = new System.Windows.Forms.CheckBox();
            this.deadzoneNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.decayThresholdNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.decayRateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.sensitivityNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.deadzoneLabel = new System.Windows.Forms.Label();
            this.decayThresholdLabel = new System.Windows.Forms.Label();
            this.decayRateLabel = new System.Windows.Forms.Label();
            this.sensitivityLabel = new System.Windows.Forms.Label();
            this.axisDisplay = new PS4Macro.Controls.AxisDisplay();
            this.macrosGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.macrosDataGridView)).BeginInit();
            this.mappingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mappingsDataGridView)).BeginInit();
            this.mouseInputGroupBox.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deadzoneNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decayThresholdNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decayRateNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sensitivityNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(456, 10);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(96, 23);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Save Bindings";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // macrosGroupBox
            // 
            this.macrosGroupBox.Controls.Add(this.macrosDataGridView);
            this.macrosGroupBox.Location = new System.Drawing.Point(13, 244);
            this.macrosGroupBox.Name = "macrosGroupBox";
            this.macrosGroupBox.Size = new System.Drawing.Size(339, 168);
            this.macrosGroupBox.TabIndex = 4;
            this.macrosGroupBox.TabStop = false;
            this.macrosGroupBox.Text = "Macros";
            // 
            // macrosDataGridView
            // 
            this.macrosDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.macrosDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Browse,
            this._Name,
            this.dataGridViewTextBoxColumn1,
            this.Path});
            this.macrosDataGridView.Location = new System.Drawing.Point(6, 19);
            this.macrosDataGridView.Name = "macrosDataGridView";
            this.macrosDataGridView.Size = new System.Drawing.Size(327, 143);
            this.macrosDataGridView.TabIndex = 0;
            this.macrosDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.macrosDataGridView_CellContentClick);
            this.macrosDataGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.macrosDataGridView_CellValidating);
            this.macrosDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.macrosDataGridView_CellValueChanged);
            this.macrosDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.macrosDataGridView_DataError);
            // 
            // Browse
            // 
            this.Browse.FillWeight = 40F;
            this.Browse.HeaderText = "...";
            this.Browse.Name = "Browse";
            this.Browse.Text = "...";
            this.Browse.UseColumnTextForButtonValue = true;
            this.Browse.Width = 40;
            // 
            // _Name
            // 
            this._Name.DataPropertyName = "Name";
            this._Name.HeaderText = "Name";
            this._Name.Name = "_Name";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Key";
            this.dataGridViewTextBoxColumn1.HeaderText = "Key";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // Path
            // 
            this.Path.DataPropertyName = "Path";
            this.Path.HeaderText = "Path";
            this.Path.Name = "Path";
            // 
            // mappingsGroupBox
            // 
            this.mappingsGroupBox.Controls.Add(this.mappingsDataGridView);
            this.mappingsGroupBox.Location = new System.Drawing.Point(13, 34);
            this.mappingsGroupBox.Name = "mappingsGroupBox";
            this.mappingsGroupBox.Size = new System.Drawing.Size(339, 204);
            this.mappingsGroupBox.TabIndex = 3;
            this.mappingsGroupBox.TabStop = false;
            this.mappingsGroupBox.Text = "Mappings";
            // 
            // mappingsDataGridView
            // 
            this.mappingsDataGridView.AllowUserToAddRows = false;
            this.mappingsDataGridView.AllowUserToDeleteRows = false;
            this.mappingsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mappingsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Button,
            this.Key});
            this.mappingsDataGridView.Location = new System.Drawing.Point(6, 19);
            this.mappingsDataGridView.Name = "mappingsDataGridView";
            this.mappingsDataGridView.Size = new System.Drawing.Size(327, 179);
            this.mappingsDataGridView.TabIndex = 0;
            this.mappingsDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.mappingsDataGridView_CellContentClick);
            this.mappingsDataGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.mappingsDataGridView_CellValidating);
            this.mappingsDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.mappingsDataGridView_CellValueChanged);
            this.mappingsDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.mappingsDataGridView_DataError);
            // 
            // Button
            // 
            this.Button.DataPropertyName = "Name";
            this.Button.HeaderText = "Button";
            this.Button.Name = "Button";
            this.Button.ReadOnly = true;
            // 
            // Key
            // 
            this.Key.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Key.DataPropertyName = "Key";
            this.Key.HeaderText = "Key";
            this.Key.Name = "Key";
            // 
            // mouseInputGroupBox
            // 
            this.mouseInputGroupBox.Controls.Add(this.axisDisplay);
            this.mouseInputGroupBox.Controls.Add(this.movementJoystickLabel);
            this.mouseInputGroupBox.Controls.Add(this.panel1);
            this.mouseInputGroupBox.Controls.Add(this.rightMouseComboBox);
            this.mouseInputGroupBox.Controls.Add(this.label1);
            this.mouseInputGroupBox.Controls.Add(this.leftMouseComboBox);
            this.mouseInputGroupBox.Controls.Add(this.leftMouseLabel);
            this.mouseInputGroupBox.Controls.Add(this.enableMouseCheckBox);
            this.mouseInputGroupBox.Controls.Add(this.deadzoneNumericUpDown);
            this.mouseInputGroupBox.Controls.Add(this.decayThresholdNumericUpDown);
            this.mouseInputGroupBox.Controls.Add(this.decayRateNumericUpDown);
            this.mouseInputGroupBox.Controls.Add(this.sensitivityNumericUpDown);
            this.mouseInputGroupBox.Controls.Add(this.deadzoneLabel);
            this.mouseInputGroupBox.Controls.Add(this.decayThresholdLabel);
            this.mouseInputGroupBox.Controls.Add(this.decayRateLabel);
            this.mouseInputGroupBox.Controls.Add(this.sensitivityLabel);
            this.mouseInputGroupBox.Location = new System.Drawing.Point(358, 34);
            this.mouseInputGroupBox.Name = "mouseInputGroupBox";
            this.mouseInputGroupBox.Size = new System.Drawing.Size(194, 378);
            this.mouseInputGroupBox.TabIndex = 7;
            this.mouseInputGroupBox.TabStop = false;
            this.mouseInputGroupBox.Text = "Mouse Input";
            // 
            // movementJoystickLabel
            // 
            this.movementJoystickLabel.AutoSize = true;
            this.movementJoystickLabel.Location = new System.Drawing.Point(9, 195);
            this.movementJoystickLabel.Name = "movementJoystickLabel";
            this.movementJoystickLabel.Size = new System.Drawing.Size(98, 13);
            this.movementJoystickLabel.TabIndex = 9;
            this.movementJoystickLabel.Text = "Movement Joystick";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rightStickRadioButton);
            this.panel1.Controls.Add(this.leftStickRadioButton);
            this.panel1.Location = new System.Drawing.Point(9, 211);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(179, 24);
            this.panel1.TabIndex = 10;
            // 
            // rightStickRadioButton
            // 
            this.rightStickRadioButton.AutoSize = true;
            this.rightStickRadioButton.Checked = true;
            this.rightStickRadioButton.Location = new System.Drawing.Point(99, 3);
            this.rightStickRadioButton.Name = "rightStickRadioButton";
            this.rightStickRadioButton.Size = new System.Drawing.Size(77, 17);
            this.rightStickRadioButton.TabIndex = 1;
            this.rightStickRadioButton.TabStop = true;
            this.rightStickRadioButton.Text = "Right Stick";
            this.rightStickRadioButton.UseVisualStyleBackColor = true;
            this.rightStickRadioButton.CheckedChanged += new System.EventHandler(this.rightStickRadioButton_CheckedChanged);
            // 
            // leftStickRadioButton
            // 
            this.leftStickRadioButton.AutoSize = true;
            this.leftStickRadioButton.Location = new System.Drawing.Point(3, 3);
            this.leftStickRadioButton.Name = "leftStickRadioButton";
            this.leftStickRadioButton.Size = new System.Drawing.Size(70, 17);
            this.leftStickRadioButton.TabIndex = 0;
            this.leftStickRadioButton.Text = "Left Stick";
            this.leftStickRadioButton.UseVisualStyleBackColor = true;
            this.leftStickRadioButton.CheckedChanged += new System.EventHandler(this.leftStickRadioButton_CheckedChanged);
            // 
            // rightMouseComboBox
            // 
            this.rightMouseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rightMouseComboBox.FormattingEnabled = true;
            this.rightMouseComboBox.Location = new System.Drawing.Point(81, 349);
            this.rightMouseComboBox.Name = "rightMouseComboBox";
            this.rightMouseComboBox.Size = new System.Drawing.Size(104, 21);
            this.rightMouseComboBox.TabIndex = 14;
            this.rightMouseComboBox.SelectedIndexChanged += new System.EventHandler(this.rightMouseComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 353);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Right Click";
            // 
            // leftMouseComboBox
            // 
            this.leftMouseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.leftMouseComboBox.FormattingEnabled = true;
            this.leftMouseComboBox.Location = new System.Drawing.Point(81, 323);
            this.leftMouseComboBox.Name = "leftMouseComboBox";
            this.leftMouseComboBox.Size = new System.Drawing.Size(104, 21);
            this.leftMouseComboBox.TabIndex = 12;
            this.leftMouseComboBox.SelectedIndexChanged += new System.EventHandler(this.leftMouseComboBox_SelectedIndexChanged);
            // 
            // leftMouseLabel
            // 
            this.leftMouseLabel.AutoSize = true;
            this.leftMouseLabel.Location = new System.Drawing.Point(9, 327);
            this.leftMouseLabel.Name = "leftMouseLabel";
            this.leftMouseLabel.Size = new System.Drawing.Size(51, 13);
            this.leftMouseLabel.TabIndex = 11;
            this.leftMouseLabel.Text = "Left Click";
            // 
            // enableMouseCheckBox
            // 
            this.enableMouseCheckBox.AutoSize = true;
            this.enableMouseCheckBox.Location = new System.Drawing.Point(12, 27);
            this.enableMouseCheckBox.Name = "enableMouseCheckBox";
            this.enableMouseCheckBox.Size = new System.Drawing.Size(94, 17);
            this.enableMouseCheckBox.TabIndex = 0;
            this.enableMouseCheckBox.Text = "Enable Mouse";
            this.enableMouseCheckBox.UseVisualStyleBackColor = true;
            this.enableMouseCheckBox.CheckedChanged += new System.EventHandler(this.enableMouseCheckBox_CheckedChanged);
            // 
            // deadzoneNumericUpDown
            // 
            this.deadzoneNumericUpDown.DecimalPlaces = 2;
            this.deadzoneNumericUpDown.Location = new System.Drawing.Point(136, 136);
            this.deadzoneNumericUpDown.Name = "deadzoneNumericUpDown";
            this.deadzoneNumericUpDown.Size = new System.Drawing.Size(52, 20);
            this.deadzoneNumericUpDown.TabIndex = 8;
            this.deadzoneNumericUpDown.ValueChanged += new System.EventHandler(this.deadzoneNumericUpDown_ValueChanged);
            // 
            // decayThresholdNumericUpDown
            // 
            this.decayThresholdNumericUpDown.DecimalPlaces = 2;
            this.decayThresholdNumericUpDown.Location = new System.Drawing.Point(136, 110);
            this.decayThresholdNumericUpDown.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.decayThresholdNumericUpDown.Name = "decayThresholdNumericUpDown";
            this.decayThresholdNumericUpDown.Size = new System.Drawing.Size(52, 20);
            this.decayThresholdNumericUpDown.TabIndex = 6;
            this.decayThresholdNumericUpDown.ValueChanged += new System.EventHandler(this.decayThresholdNumericUpDown_ValueChanged);
            // 
            // decayRateNumericUpDown
            // 
            this.decayRateNumericUpDown.DecimalPlaces = 2;
            this.decayRateNumericUpDown.Location = new System.Drawing.Point(136, 84);
            this.decayRateNumericUpDown.Minimum = new decimal(new int[] {
            101,
            0,
            0,
            131072});
            this.decayRateNumericUpDown.Name = "decayRateNumericUpDown";
            this.decayRateNumericUpDown.Size = new System.Drawing.Size(52, 20);
            this.decayRateNumericUpDown.TabIndex = 4;
            this.decayRateNumericUpDown.Value = new decimal(new int[] {
            101,
            0,
            0,
            131072});
            this.decayRateNumericUpDown.ValueChanged += new System.EventHandler(this.decayRateNumericUpDown_ValueChanged);
            // 
            // sensitivityNumericUpDown
            // 
            this.sensitivityNumericUpDown.DecimalPlaces = 2;
            this.sensitivityNumericUpDown.Location = new System.Drawing.Point(136, 58);
            this.sensitivityNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.sensitivityNumericUpDown.Name = "sensitivityNumericUpDown";
            this.sensitivityNumericUpDown.Size = new System.Drawing.Size(52, 20);
            this.sensitivityNumericUpDown.TabIndex = 2;
            this.sensitivityNumericUpDown.ValueChanged += new System.EventHandler(this.sensitivityNumericUpDown_ValueChanged);
            // 
            // deadzoneLabel
            // 
            this.deadzoneLabel.AutoSize = true;
            this.deadzoneLabel.Location = new System.Drawing.Point(9, 138);
            this.deadzoneLabel.Name = "deadzoneLabel";
            this.deadzoneLabel.Size = new System.Drawing.Size(56, 13);
            this.deadzoneLabel.TabIndex = 7;
            this.deadzoneLabel.Text = "Deadzone";
            // 
            // decayThresholdLabel
            // 
            this.decayThresholdLabel.AutoSize = true;
            this.decayThresholdLabel.Location = new System.Drawing.Point(9, 112);
            this.decayThresholdLabel.Name = "decayThresholdLabel";
            this.decayThresholdLabel.Size = new System.Drawing.Size(88, 13);
            this.decayThresholdLabel.TabIndex = 5;
            this.decayThresholdLabel.Text = "Decay Threshold";
            // 
            // decayRateLabel
            // 
            this.decayRateLabel.AutoSize = true;
            this.decayRateLabel.Location = new System.Drawing.Point(9, 86);
            this.decayRateLabel.Name = "decayRateLabel";
            this.decayRateLabel.Size = new System.Drawing.Size(64, 13);
            this.decayRateLabel.TabIndex = 3;
            this.decayRateLabel.Text = "Decay Rate";
            // 
            // sensitivityLabel
            // 
            this.sensitivityLabel.AutoSize = true;
            this.sensitivityLabel.Location = new System.Drawing.Point(9, 60);
            this.sensitivityLabel.Name = "sensitivityLabel";
            this.sensitivityLabel.Size = new System.Drawing.Size(54, 13);
            this.sensitivityLabel.TabIndex = 1;
            this.sensitivityLabel.Text = "Sensitivity";
            // 
            // axisDisplay
            // 
            this.axisDisplay.InnerColor = System.Drawing.Color.GhostWhite;
            this.axisDisplay.InnerSize = 12;
            this.axisDisplay.Location = new System.Drawing.Point(67, 240);
            this.axisDisplay.Name = "axisDisplay";
            this.axisDisplay.OuterColor = System.Drawing.Color.DodgerBlue;
            this.axisDisplay.Size = new System.Drawing.Size(60, 60);
            this.axisDisplay.TabIndex = 15;
            this.axisDisplay.Value = ((System.Drawing.PointF)(resources.GetObject("axisDisplay.Value")));
            // 
            // RemapperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 421);
            this.Controls.Add(this.mouseInputGroupBox);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.macrosGroupBox);
            this.Controls.Add(this.mappingsGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "RemapperForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Remapper";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RemapperForm_FormClosed);
            this.Load += new System.EventHandler(this.RemapperForm_Load);
            this.macrosGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.macrosDataGridView)).EndInit();
            this.mappingsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mappingsDataGridView)).EndInit();
            this.mouseInputGroupBox.ResumeLayout(false);
            this.mouseInputGroupBox.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deadzoneNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decayThresholdNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decayRateNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sensitivityNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.GroupBox macrosGroupBox;
        private System.Windows.Forms.DataGridView macrosDataGridView;
        private System.Windows.Forms.DataGridViewButtonColumn Browse;
        private System.Windows.Forms.DataGridViewTextBoxColumn _Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Path;
        private System.Windows.Forms.GroupBox mappingsGroupBox;
        private System.Windows.Forms.DataGridView mappingsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Button;
        private System.Windows.Forms.DataGridViewTextBoxColumn Key;
        private System.Windows.Forms.GroupBox mouseInputGroupBox;
        private System.Windows.Forms.Label sensitivityLabel;
        private System.Windows.Forms.Label decayThresholdLabel;
        private System.Windows.Forms.Label decayRateLabel;
        private System.Windows.Forms.Label deadzoneLabel;
        private System.Windows.Forms.CheckBox enableMouseCheckBox;
        private System.Windows.Forms.NumericUpDown deadzoneNumericUpDown;
        private System.Windows.Forms.NumericUpDown decayThresholdNumericUpDown;
        private System.Windows.Forms.NumericUpDown decayRateNumericUpDown;
        private System.Windows.Forms.NumericUpDown sensitivityNumericUpDown;
        private System.Windows.Forms.ComboBox rightMouseComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox leftMouseComboBox;
        private System.Windows.Forms.Label leftMouseLabel;
        private System.Windows.Forms.Label movementJoystickLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rightStickRadioButton;
        private System.Windows.Forms.RadioButton leftStickRadioButton;
        private Controls.AxisDisplay axisDisplay;
    }
}