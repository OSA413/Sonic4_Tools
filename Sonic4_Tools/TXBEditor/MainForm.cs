using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TXBEditor
{
    public partial class MainForm:Form
    {
        public List<byte[]> Values { get; set; }

        static string DirectorySelectionDialog()
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "TXB|*.TXB|All files|*.*";
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;
                ofd.Title = "Select a TXB file";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }
            return null;
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void UpdateValues()
        {
            cb_Num.Items.Clear();
            for (int i = 0; i < Values.Count; i++)
            {
                cb_Num.Items.Add(i);
            }

            tb_Value.Enabled = false;
            tb_Hex.Enabled   = false;
            b_Save.Enabled   = false;
            tb_Value.Text = "";
            tb_Hex.Text   = "";

            if (Values.Count > 0)
            {
                cb_Num.SelectedIndex = 0;
                tb_Value.Enabled = true;
                tb_Hex.Enabled   = true;
                b_Save.Enabled   = true;
            }

            l_VarNum.Text = Values.Count().ToString();
        }

        private void b_Open_Click(object sender, EventArgs e)
        {
            string tmp_file = DirectorySelectionDialog();
            if (tmp_file != null)
            {
                b_Reload.Enabled = true;
                tb_File.Text = tmp_file;
                Values = TXB.Read(tmp_file);
                UpdateValues();
            }
        }

        private void rb_Int_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_Num.SelectedIndex == -1)
            { return; }

            byte[] the_value = new byte[Values[cb_Num.SelectedIndex].Length];
            Array.Copy(Values[cb_Num.SelectedIndex], the_value, Values[cb_Num.SelectedIndex].Length);

            tb_Hex.Text = BitConverter.ToString(the_value).Replace("-", "");

            if (cb_Endianness.Checked)
            { Array.Reverse(the_value); }

            if (rb_Int.Checked)
            {
                tb_Value.Text = BitConverter.ToInt32(the_value, 0).ToString();
            }
            else
            {
                tb_Value.Text = BitConverter.ToSingle(the_value, 0).ToString();
            }
        }

        private void cb_Endianness_CheckStateChanged(object sender, EventArgs e)
        {
            rb_Int_CheckedChanged(sender, e);
        }

        private void cb_Num_SelectedIndexChanged(object sender, EventArgs e)
        {
            rb_Int_CheckedChanged(sender, e);
        }

        private void tb_Value_TextChanged(object sender, EventArgs e)
        {
            int int_num;
            float float_num;
            if (rb_Int.Checked && Int32.TryParse(tb_Value.Text, out int_num))
            {
                byte[] the_value = BitConverter.GetBytes(int_num);

                if (cb_Endianness.Checked)
                { Array.Reverse(the_value); }

                tb_Hex.Text = BitConverter.ToString(the_value).Replace("-", "");
            }
            else if (Single.TryParse(tb_Value.Text, out float_num))
            {
                byte[] the_value = BitConverter.GetBytes(float_num);

                if (cb_Endianness.Checked)
                { Array.Reverse(the_value); }
                
                tb_Hex.Text = BitConverter.ToString(the_value).Replace("-", "");
            }
            else
            {
                tb_Hex.Text = "Error";
                
                if (rb_Int.Checked)
                    if (Int64.TryParse(tb_Value.Text, out Int64 tmp_int64))
                        tb_Hex.Text = "int32 overflow";
                
                if (tb_Value.Text == "")
                { tb_Hex.Text = BitConverter.ToString(BitConverter.GetBytes(0)).Replace("-", ""); }
            }
        }

        private void tb_Hex_TextChanged(object sender, EventArgs e)
        {
            if (tb_Hex.Text == "" || cb_Num.SelectedIndex == -1) { return; }

            //Handeling the Delete key (yes, that's it)
            if (tb_Hex.Text.Length < 8 && tb_Hex.Focused)
            {
                int prev_pos = tb_Hex.SelectionStart;
                
                tb_Hex.Text = tb_Hex.Text.Substring(0, prev_pos)
                            + '0'
                            + tb_Hex.Text.Substring(prev_pos, tb_Hex.Text.Length - prev_pos);
                tb_Hex.SelectionStart = prev_pos + 1;
            }
            
            for (int i = 0; i < 4; i++)
            {
                string[] vals = new string[2] { tb_Hex.Text[i*2].ToString(),
                                                tb_Hex.Text[i*2 + 1].ToString()};

                for (int j = 0; j < 2; j++)
                {
                    if (vals[j][0] >= 65)
                    { vals[j] = (vals[j][0] - 55).ToString(); }
                }

                Values[cb_Num.SelectedIndex][i] = (byte)(int.Parse(vals[0])*16 + int.Parse(vals[1]));
            }

            rb_Int_CheckedChanged(sender, e);
        }

        private void tb_Value_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && tb_Value.Text.Length > 16)
            { e.Handled = true; }

            if (rb_Int.Checked)
            {
                if (!char.IsControl(e.KeyChar)
                    && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            else
            {
                //It really annoys me that my numeric keyboard presses "," instead of "." when layout is not English
                if (e.KeyChar == ',') { e.KeyChar = '.'; }
                if (e.KeyChar == 'e') { e.KeyChar = 'E'; }

                if (!char.IsControl(e.KeyChar)
                    && !char.IsDigit(e.KeyChar)
                    && e.KeyChar != '.'
                    && e.KeyChar != 'E'
                    && e.KeyChar != '+'
                    && e.KeyChar != '-')
                {
                    e.Handled = true;
                }
            }
        }

        private void tb_Hex_KeyPress(object sender, KeyPressEventArgs e)
        {
            int prev_pos = tb_Hex.SelectionStart;

            //Backspace
            if (e.KeyChar == (char)Keys.Back)
            {
                if (prev_pos == 0) { return; }
                tb_Hex.Text = tb_Hex.Text.Substring(0, prev_pos - 1)
                            + '0'
                            + tb_Hex.Text.Substring(prev_pos, 8 - prev_pos);
                tb_Hex.SelectionStart = prev_pos - 1;
            }
            else if (tb_Hex.SelectionStart < 8)
            {
                //Converting [a-f] into [A-F]
                if ((int)e.KeyChar >= 97 && (int)e.KeyChar <= 102) { e.KeyChar = (char)((int)e.KeyChar - 32); }

                if (!char.IsControl(e.KeyChar)
                    && !char.IsDigit(e.KeyChar)
                    && e.KeyChar != 'A'
                    && e.KeyChar != 'B'
                    && e.KeyChar != 'C'
                    && e.KeyChar != 'D'
                    && e.KeyChar != 'E'
                    && e.KeyChar != 'F')
                    {
                        e.Handled = true;
                        return;
                    }

                tb_Hex.Text = tb_Hex.Text.Substring(0, prev_pos)
                            + e.KeyChar
                            + tb_Hex.Text.Substring(prev_pos + 1, 7 - prev_pos);
                tb_Hex.SelectionStart = prev_pos + 1;
            }
            e.Handled = true;
        }

        private void b_Save_Click(object sender, EventArgs e)
        {
            TXB.Rewrite(tb_File.Text, Values);
        }

        private void b_Reload_Click(object sender, EventArgs e)
        {
            if (File.Exists(tb_File.Text))
            {
                Values = TXB.Read(tb_File.Text);
                UpdateValues();
            }
        }
    }
}
