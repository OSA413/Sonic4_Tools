namespace OldModConversionTool
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tbModPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bModPath = new System.Windows.Forms.Button();
            this.bGamePath = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbGamePath = new System.Windows.Forms.TextBox();
            this.bConvert = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lAMBPatcherStatus = new System.Windows.Forms.Label();
            this.lCsbEditorStatus = new System.Windows.Forms.Label();
            this.bRefresh = new System.Windows.Forms.Button();
            this.bOutputPath = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tbOutputPath = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(166, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Old Mod Conversion Tool";
            // 
            // tbModPath
            // 
            this.tbModPath.Location = new System.Drawing.Point(12, 48);
            this.tbModPath.Name = "tbModPath";
            this.tbModPath.Size = new System.Drawing.Size(384, 20);
            this.tbModPath.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(181, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Path to the root directory of your mod";
            // 
            // bModPath
            // 
            this.bModPath.Location = new System.Drawing.Point(402, 45);
            this.bModPath.Name = "bModPath";
            this.bModPath.Size = new System.Drawing.Size(32, 23);
            this.bModPath.TabIndex = 3;
            this.bModPath.Text = "...";
            this.bModPath.UseVisualStyleBackColor = true;
            this.bModPath.Click += new System.EventHandler(this.bModPath_Click);
            // 
            // bGamePath
            // 
            this.bGamePath.Location = new System.Drawing.Point(402, 94);
            this.bGamePath.Name = "bGamePath";
            this.bGamePath.Size = new System.Drawing.Size(32, 23);
            this.bGamePath.TabIndex = 6;
            this.bGamePath.Text = "...";
            this.bGamePath.UseVisualStyleBackColor = true;
            this.bGamePath.Click += new System.EventHandler(this.bGamePath_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(182, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Path to the root directory of the game";
            // 
            // tbGamePath
            // 
            this.tbGamePath.Location = new System.Drawing.Point(12, 96);
            this.tbGamePath.Name = "tbGamePath";
            this.tbGamePath.Size = new System.Drawing.Size(384, 20);
            this.tbGamePath.TabIndex = 4;
            // 
            // bConvert
            // 
            this.bConvert.Location = new System.Drawing.Point(167, 213);
            this.bConvert.Name = "bConvert";
            this.bConvert.Size = new System.Drawing.Size(128, 32);
            this.bConvert.TabIndex = 7;
            this.bConvert.Text = "Convert";
            this.bConvert.UseVisualStyleBackColor = true;
            this.bConvert.Click += new System.EventHandler(this.bConvert_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(95, 170);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "AMBPatcher:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(95, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "CsbEditor:";
            // 
            // lAMBPatcherStatus
            // 
            this.lAMBPatcherStatus.AutoSize = true;
            this.lAMBPatcherStatus.Location = new System.Drawing.Point(171, 170);
            this.lAMBPatcherStatus.Name = "lAMBPatcherStatus";
            this.lAMBPatcherStatus.Size = new System.Drawing.Size(35, 13);
            this.lAMBPatcherStatus.TabIndex = 10;
            this.lAMBPatcherStatus.Text = "status";
            // 
            // lCsbEditorStatus
            // 
            this.lCsbEditorStatus.AutoSize = true;
            this.lCsbEditorStatus.Location = new System.Drawing.Point(171, 189);
            this.lCsbEditorStatus.Name = "lCsbEditorStatus";
            this.lCsbEditorStatus.Size = new System.Drawing.Size(35, 13);
            this.lCsbEditorStatus.TabIndex = 11;
            this.lCsbEditorStatus.Text = "status";
            // 
            // bRefresh
            // 
            this.bRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bRefresh.Location = new System.Drawing.Point(57, 170);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(32, 32);
            this.bRefresh.TabIndex = 12;
            this.bRefresh.Text = "⟳";
            this.bRefresh.UseVisualStyleBackColor = true;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // bOutputPath
            // 
            this.bOutputPath.Location = new System.Drawing.Point(402, 142);
            this.bOutputPath.Name = "bOutputPath";
            this.bOutputPath.Size = new System.Drawing.Size(32, 23);
            this.bOutputPath.TabIndex = 15;
            this.bOutputPath.Text = "...";
            this.bOutputPath.UseVisualStyleBackColor = true;
            this.bOutputPath.Click += new System.EventHandler(this.bOutputPath_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Output directory";
            // 
            // tbOutputPath
            // 
            this.tbOutputPath.Location = new System.Drawing.Point(12, 144);
            this.tbOutputPath.Name = "tbOutputPath";
            this.tbOutputPath.Size = new System.Drawing.Size(384, 20);
            this.tbOutputPath.TabIndex = 13;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 255);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(446, 22);
            this.statusStrip1.TabIndex = 16;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusBar
            // 
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(59, 17);
            this.statusBar.Text = "Status Bar";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 277);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.bOutputPath);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbOutputPath);
            this.Controls.Add(this.bRefresh);
            this.Controls.Add(this.lCsbEditorStatus);
            this.Controls.Add(this.lAMBPatcherStatus);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bConvert);
            this.Controls.Add(this.bGamePath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbGamePath);
            this.Controls.Add(this.bModPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbModPath);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "Old Mod Conversion Tool";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbModPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bModPath;
        private System.Windows.Forms.Button bGamePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbGamePath;
        private System.Windows.Forms.Button bConvert;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lAMBPatcherStatus;
        private System.Windows.Forms.Label lCsbEditorStatus;
        private System.Windows.Forms.Button bRefresh;
        private System.Windows.Forms.Button bOutputPath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbOutputPath;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusBar;
    }
}

