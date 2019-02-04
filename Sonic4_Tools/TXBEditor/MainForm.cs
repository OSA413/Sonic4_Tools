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
        }

        private void b_Open_Click(object sender, EventArgs e)
        {
            string tmp_file = DirectorySelectionDialog();
            if (tmp_file != null)
            { 
                l_File.Text = tmp_file;
                Values = TXB.Read(tmp_file);
                UpdateValues();
            }
        }
    }
}
