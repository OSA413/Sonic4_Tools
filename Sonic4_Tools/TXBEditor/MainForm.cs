using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            
            if (Values.Count > 0)
            { cb_Num.SelectedIndex = 0; }

            l_VarNum.Text = Values.Count().ToString();
        }

        private void b_Open_Click(object sender, EventArgs e)
        {
            string tmp_file = DirectorySelectionDialog();
            if (tmp_file != null)
            { 
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

            if (cb_Endianness.Checked)
            { Array.Reverse(the_value); }

            tb_Hex.Text = BitConverter.ToString(the_value).Replace("-","");

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
    }
}
