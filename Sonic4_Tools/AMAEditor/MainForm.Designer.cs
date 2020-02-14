namespace AMAEditor
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.bOpenFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGroup1 = new System.Windows.Forms.TabPage();
            this.listBoxGroup1 = new System.Windows.Forms.ListBox();
            this.tabGroup2 = new System.Windows.Forms.TabPage();
            this.listBoxGroup2 = new System.Windows.Forms.ListBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.VName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bSave = new System.Windows.Forms.Button();
            this.cbUnknowValues = new System.Windows.Forms.CheckBox();
            this.tabControl.SuspendLayout();
            this.tabGroup1.SuspendLayout();
            this.tabGroup2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 228);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(358, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip";
            // 
            // tbFileName
            // 
            this.tbFileName.Location = new System.Drawing.Point(20, 25);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.ReadOnly = true;
            this.tbFileName.Size = new System.Drawing.Size(202, 20);
            this.tbFileName.TabIndex = 1;
            // 
            // bOpenFile
            // 
            this.bOpenFile.Location = new System.Drawing.Point(228, 22);
            this.bOpenFile.Name = "bOpenFile";
            this.bOpenFile.Size = new System.Drawing.Size(32, 23);
            this.bOpenFile.TabIndex = 2;
            this.bOpenFile.Text = "...";
            this.bOpenFile.UseVisualStyleBackColor = true;
            this.bOpenFile.Click += new System.EventHandler(this.bOpenFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "File name:";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabGroup1);
            this.tabControl.Controls.Add(this.tabGroup2);
            this.tabControl.ItemSize = new System.Drawing.Size(62, 18);
            this.tabControl.Location = new System.Drawing.Point(12, 52);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(128, 173);
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl.TabIndex = 4;
            // 
            // tabGroup1
            // 
            this.tabGroup1.Controls.Add(this.listBoxGroup1);
            this.tabGroup1.Location = new System.Drawing.Point(4, 22);
            this.tabGroup1.Name = "tabGroup1";
            this.tabGroup1.Padding = new System.Windows.Forms.Padding(3);
            this.tabGroup1.Size = new System.Drawing.Size(120, 147);
            this.tabGroup1.TabIndex = 0;
            this.tabGroup1.Text = "Group 1";
            this.tabGroup1.UseVisualStyleBackColor = true;
            // 
            // listBoxGroup1
            // 
            this.listBoxGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxGroup1.FormattingEnabled = true;
            this.listBoxGroup1.Location = new System.Drawing.Point(3, 3);
            this.listBoxGroup1.Name = "listBoxGroup1";
            this.listBoxGroup1.Size = new System.Drawing.Size(114, 141);
            this.listBoxGroup1.TabIndex = 5;
            this.listBoxGroup1.SelectedIndexChanged += new System.EventHandler(this.listBoxGroup1_SelectedIndexChanged);
            // 
            // tabGroup2
            // 
            this.tabGroup2.Controls.Add(this.listBoxGroup2);
            this.tabGroup2.Location = new System.Drawing.Point(4, 22);
            this.tabGroup2.Name = "tabGroup2";
            this.tabGroup2.Padding = new System.Windows.Forms.Padding(3);
            this.tabGroup2.Size = new System.Drawing.Size(120, 147);
            this.tabGroup2.TabIndex = 1;
            this.tabGroup2.Text = "Group 2";
            this.tabGroup2.UseVisualStyleBackColor = true;
            // 
            // listBoxGroup2
            // 
            this.listBoxGroup2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxGroup2.FormattingEnabled = true;
            this.listBoxGroup2.Location = new System.Drawing.Point(3, 3);
            this.listBoxGroup2.Name = "listBoxGroup2";
            this.listBoxGroup2.Size = new System.Drawing.Size(114, 141);
            this.listBoxGroup2.TabIndex = 5;
            this.listBoxGroup2.SelectedIndexChanged += new System.EventHandler(this.listBoxGroup2_SelectedIndexChanged);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeColumns = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VName,
            this.Value});
            this.dataGridView.Location = new System.Drawing.Point(146, 52);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.ShowEditingIcon = false;
            this.dataGridView.Size = new System.Drawing.Size(200, 150);
            this.dataGridView.TabIndex = 5;
            this.dataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellEndEdit);
            // 
            // VName
            // 
            this.VName.HeaderText = "Name";
            this.VName.Name = "VName";
            this.VName.ReadOnly = true;
            this.VName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.VName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Value.Width = 75;
            // 
            // bSave
            // 
            this.bSave.Location = new System.Drawing.Point(282, 22);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(64, 23);
            this.bSave.TabIndex = 8;
            this.bSave.Text = "Save";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // cbUnknowValues
            // 
            this.cbUnknowValues.AutoSize = true;
            this.cbUnknowValues.Location = new System.Drawing.Point(146, 208);
            this.cbUnknowValues.Name = "cbUnknowValues";
            this.cbUnknowValues.Size = new System.Drawing.Size(134, 17);
            this.cbUnknowValues.TabIndex = 9;
            this.cbUnknowValues.Text = "Show unknown values";
            this.cbUnknowValues.UseVisualStyleBackColor = true;
            this.cbUnknowValues.CheckedChanged += new System.EventHandler(this.cbUnknowValues_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 250);
            this.Controls.Add(this.cbUnknowValues);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bOpenFile);
            this.Controls.Add(this.tbFileName);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "AMA Editor";
            this.tabControl.ResumeLayout(false);
            this.tabGroup1.ResumeLayout(false);
            this.tabGroup2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.Button bOpenFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGroup1;
        private System.Windows.Forms.TabPage tabGroup2;
        private System.Windows.Forms.ListBox listBoxGroup1;
        private System.Windows.Forms.ListBox listBoxGroup2;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn VName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.CheckBox cbUnknowValues;
    }
}