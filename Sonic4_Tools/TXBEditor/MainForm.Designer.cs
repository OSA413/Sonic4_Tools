namespace TXBEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.cb_Num = new System.Windows.Forms.ComboBox();
            this.rb_Int = new System.Windows.Forms.RadioButton();
            this.rb_Single = new System.Windows.Forms.RadioButton();
            this.b_Open = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_Value = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.b_Reload = new System.Windows.Forms.Button();
            this.b_Save = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cb_Endianness = new System.Windows.Forms.CheckBox();
            this.tb_File = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Hex = new System.Windows.Forms.TextBox();
            this.l_VarNum = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cb_Num
            // 
            this.cb_Num.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Num.FormattingEnabled = true;
            this.cb_Num.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cb_Num.Location = new System.Drawing.Point(64, 45);
            this.cb_Num.Name = "cb_Num";
            this.cb_Num.Size = new System.Drawing.Size(80, 21);
            this.cb_Num.TabIndex = 0;
            this.cb_Num.SelectedIndexChanged += new System.EventHandler(this.cb_Num_SelectedIndexChanged);
            // 
            // rb_Int
            // 
            this.rb_Int.AutoSize = true;
            this.rb_Int.Checked = true;
            this.rb_Int.Location = new System.Drawing.Point(176, 57);
            this.rb_Int.Name = "rb_Int";
            this.rb_Int.Size = new System.Drawing.Size(58, 17);
            this.rb_Int.TabIndex = 1;
            this.rb_Int.TabStop = true;
            this.rb_Int.Text = "Integer";
            this.rb_Int.UseVisualStyleBackColor = true;
            this.rb_Int.CheckedChanged += new System.EventHandler(this.rb_Int_CheckedChanged);
            // 
            // rb_Single
            // 
            this.rb_Single.AutoSize = true;
            this.rb_Single.Location = new System.Drawing.Point(176, 80);
            this.rb_Single.Name = "rb_Single";
            this.rb_Single.Size = new System.Drawing.Size(48, 17);
            this.rb_Single.TabIndex = 2;
            this.rb_Single.Text = "Float";
            this.rb_Single.UseVisualStyleBackColor = true;
            // 
            // b_Open
            // 
            this.b_Open.Location = new System.Drawing.Point(12, 9);
            this.b_Open.Name = "b_Open";
            this.b_Open.Size = new System.Drawing.Size(75, 23);
            this.b_Open.TabIndex = 3;
            this.b_Open.Text = "Open File";
            this.b_Open.UseVisualStyleBackColor = true;
            this.b_Open.Click += new System.EventHandler(this.b_Open_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Number:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Value:";
            // 
            // tb_Value
            // 
            this.tb_Value.Location = new System.Drawing.Point(64, 72);
            this.tb_Value.Name = "tb_Value";
            this.tb_Value.Size = new System.Drawing.Size(80, 20);
            this.tb_Value.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(173, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Type mode";
            // 
            // b_Reload
            // 
            this.b_Reload.Location = new System.Drawing.Point(28, 126);
            this.b_Reload.Name = "b_Reload";
            this.b_Reload.Size = new System.Drawing.Size(75, 23);
            this.b_Reload.TabIndex = 9;
            this.b_Reload.Text = "Reload";
            this.b_Reload.UseVisualStyleBackColor = true;
            // 
            // b_Save
            // 
            this.b_Save.Location = new System.Drawing.Point(165, 126);
            this.b_Save.Name = "b_Save";
            this.b_Save.Size = new System.Drawing.Size(75, 23);
            this.b_Save.TabIndex = 10;
            this.b_Save.Text = "Save";
            this.b_Save.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(0, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(275, 26);
            this.label5.TabIndex = 11;
            this.label5.Text = "The purpose of those values is unknown at this moment.\r\nIf you have any suggestio" +
    "ns, contact OSA413";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cb_Endianness
            // 
            this.cb_Endianness.AutoSize = true;
            this.cb_Endianness.Location = new System.Drawing.Point(151, 103);
            this.cb_Endianness.Name = "cb_Endianness";
            this.cb_Endianness.Size = new System.Drawing.Size(111, 17);
            this.cb_Endianness.TabIndex = 12;
            this.cb_Endianness.Text = "Swap Endianness";
            this.cb_Endianness.UseVisualStyleBackColor = true;
            this.cb_Endianness.CheckStateChanged += new System.EventHandler(this.cb_Endianness_CheckStateChanged);
            // 
            // tb_File
            // 
            this.tb_File.Location = new System.Drawing.Point(93, 11);
            this.tb_File.Name = "tb_File";
            this.tb_File.ReadOnly = true;
            this.tb_File.Size = new System.Drawing.Size(169, 20);
            this.tb_File.TabIndex = 13;
            this.tb_File.Text = "No file";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Hex:";
            // 
            // tb_Hex
            // 
            this.tb_Hex.Location = new System.Drawing.Point(64, 98);
            this.tb_Hex.Name = "tb_Hex";
            this.tb_Hex.Size = new System.Drawing.Size(80, 20);
            this.tb_Hex.TabIndex = 15;
            // 
            // l_VarNum
            // 
            this.l_VarNum.Location = new System.Drawing.Point(109, 126);
            this.l_VarNum.Name = "l_VarNum";
            this.l_VarNum.Size = new System.Drawing.Size(50, 23);
            this.l_VarNum.TabIndex = 16;
            this.l_VarNum.Text = "Var Num";
            this.l_VarNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 185);
            this.Controls.Add(this.l_VarNum);
            this.Controls.Add(this.tb_Hex);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_File);
            this.Controls.Add(this.cb_Endianness);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.b_Save);
            this.Controls.Add(this.b_Reload);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_Value);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.b_Open);
            this.Controls.Add(this.rb_Single);
            this.Controls.Add(this.rb_Int);
            this.Controls.Add(this.cb_Num);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "TXB Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_Num;
        private System.Windows.Forms.RadioButton rb_Int;
        private System.Windows.Forms.RadioButton rb_Single;
        private System.Windows.Forms.Button b_Open;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_Value;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button b_Reload;
        private System.Windows.Forms.Button b_Save;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cb_Endianness;
        private System.Windows.Forms.TextBox tb_File;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_Hex;
        private System.Windows.Forms.Label l_VarNum;
    }
}

