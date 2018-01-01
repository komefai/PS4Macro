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
            this.macrosGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.macrosDataGridView)).BeginInit();
            this.mappingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mappingsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(256, 10);
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
            this.macrosGroupBox.Size = new System.Drawing.Size(339, 153);
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
            this.macrosDataGridView.Size = new System.Drawing.Size(327, 128);
            this.macrosDataGridView.TabIndex = 0;
            this.macrosDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.macrosDataGridView_CellContentClick);
            this.macrosDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.macrosDataGridView_CellValueChanged);
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
            this.mappingsDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.mappingsDataGridView_CellValueChanged);
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
            // RemapperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 406);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.macrosGroupBox);
            this.Controls.Add(this.mappingsGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "RemapperForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Remapper";
            this.Load += new System.EventHandler(this.RemapperForm_Load);
            this.macrosGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.macrosDataGridView)).EndInit();
            this.mappingsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mappingsDataGridView)).EndInit();
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
    }
}