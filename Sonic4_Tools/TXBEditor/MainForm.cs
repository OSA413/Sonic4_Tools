using System;
using System.IO;
using System.Windows.Forms;

namespace TXBEditor
{
    public partial class MainForm : Form
    {
        string filePath;
        TXB txbFile;

        public MainForm(string[] args)
        {
            InitializeComponent();

            if (args.Length > 0)
                OpenTXBFile(args[0]);
        }

        public void OpenTXBFile(string fileName)
        {
            filePath = fileName;
            statusBar.Text = "";
            saveToolStripMenuItem.Enabled = true;

            txbFile = new TXB();
            txbFile.Read(fileName);

            txbFile.SanityCheck(fileName);

            if (txbFile.WrongValues.Count > 0)
            {
                Console.WriteLine("Sanity check failed");
                statusBar.Text = "Warning! File won't be saved properly!";
                File.WriteAllText(fileName + "_check.txt", String.Join(" ", txbFile.StrangeList));
            }

            UpdateTable();
        }

        public void UpdateTable()
        {
            listView.Items.Clear();
            for (int i = 0; i < txbFile.TXBObjects.Count; i++)
            {
                ListViewItem item = new ListViewItem(i.ToString());
                item.SubItems.Add(txbFile.TXBObjects[i].Name);
                item.SubItems.Add(txbFile.TXBObjects[i].Unknown1.ToString());

                listView.Items.Add(item);
            }
        }

        public void TryConvertApplyUpdate(DataGridViewCell cell, ref int result)
        {
            Int32.TryParse(cell.Value.ToString(), out result);
            cell.Value = result;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "TXB files|*.TXB|All files (*.*)|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                    OpenTXBFile(ofd.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txbFile != null)
                txbFile.Write(filePath);
        }

        private void bMoveUp_Click(object sender, EventArgs e)
        {

        }

        private void bMoveDown_Click(object sender, EventArgs e)
        {

        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ind = listView.SelectedIndices[0];
            tbName.Text = txbFile.TXBObjects[ind].Name;
            tbValue.Text = txbFile.TXBObjects[ind].Unknown1.ToString();
        }

        private void bAdd_Click(object sender, EventArgs e)
        {

        }

        private void bRemove_Click(object sender, EventArgs e)
        {

        }
    }
}
