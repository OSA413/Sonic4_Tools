using System;
using System.IO;
using System.Windows.Forms;

namespace TXBEditor
{
    public partial class MainForm : Form
    {
        TXB txbFile;

        public MainForm(string[] args)
        {
            InitializeComponent();

            if (args.Length > 0)
                OpenAmaFile(args[0]);
        }

        public void OpenAmaFile(string fileName)
        {
            statusBar.Text = "";

            tbFileName.Text = fileName;
            txbFile = new TXB();
            txbFile.Read(fileName);

            txbFile.SanityCheck(fileName);

            if (txbFile.WrongValues.Count > 0)
            {
                Console.WriteLine("Sanity check failed");
                statusBar.Text = "Warning! File won't be saved properly!";
                File.WriteAllText(fileName + "_check.txt", String.Join(" ", txbFile.StrangeList));
            }

            listBoxObjects.Items.Clear();
            foreach (var obj in txbFile.TXBObjects)
                listBoxObjects.Items.Add(obj.Name);

            if (txbFile.TXBObjects.Count > 0)
            {
                tabControl.SelectedIndex = 0;
                listBoxObjects.Select();
                listBoxObjects.SelectedIndex = 0;
            }
        }

        public void SaveAmaFile(string fileName)
        {
            txbFile.Write(fileName);
        }

        private void bOpenFile_Click(object sender, System.EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "TXB files|*.TXB|All files (*.*)|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    OpenAmaFile(ofd.FileName);
                }
            }
        }

        private void listBoxObjects_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            dataGridView.Rows.Clear();

            var obj = txbFile.TXBObjects[listBoxObjects.SelectedIndex];

            dataGridView.Rows.Add("Unknown1", obj.Unknown1);
        }

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 1)
                return;

            var cell = dataGridView[1, e.RowIndex];
            var ind = listBoxObjects.SelectedIndex;

            switch (e.RowIndex)
            {
                case 0: TryConvertApplyUpdate(cell, ref txbFile.TXBObjects[ind].Unknown1); break;
            }
        }

        public void TryConvertApplyUpdate(DataGridViewCell cell, ref float result)
        {
            Single.TryParse(cell.Value.ToString(), out result);
            cell.Value = result;
        }

        public void TryConvertApplyUpdate(DataGridViewCell cell, ref int result)
        {
            Int32.TryParse(cell.Value.ToString(), out result);
            cell.Value = result;
        }

        public void TryConvertApplyUpdate(DataGridViewCell cell, ref string result)
        {
            if (cell.Value.ToString() != "(None)")
                result = cell.Value.ToString();
            result = null;
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (txbFile != null)
                txbFile.Write(tbFileName.Text);
        }
    }
}
