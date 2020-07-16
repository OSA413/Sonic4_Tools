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
                statusBar.Text = "Warning! File won't be saved properly!";
            else
                statusBar.Text = "Loaded \"" + fileName + "\"";

            UpdateTable();
        }

        public void UpdateTable()
        {
            listView.Items.Clear();
            for (int i = 0; i < txbFile.TXBObjects.Count; i++)
            {
                ListViewItem item = new ListViewItem(txbFile.TXBObjects[i].Name);
                item.SubItems.Add(i.ToString());
                item.SubItems.Add(txbFile.TXBObjects[i].Unknown1.ToString());

                listView.Items.Add(item);
            }
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
            {
                txbFile.Write(filePath);
                statusBar.Text = "Save complete";
            }
        }

        private void bMoveUp_Click(object sender, EventArgs e)
        {
            MoveObject(-1);
        }

        private void bMoveDown_Click(object sender, EventArgs e)
        {
            MoveObject(1);
        }

        private void MoveObject(int direction)
        {
            if (listView.SelectedIndices.Count == 0) return;
            var ind = listView.SelectedIndices[0];
            if (ind + direction >= listView.Items.Count || ind + direction < 0) return;

            var o = txbFile.TXBObjects[ind];
            txbFile.TXBObjects.RemoveAt(ind);
            txbFile.TXBObjects.Insert(ind + direction, o);

            var i = listView.Items[ind];
            listView.Items.RemoveAt(ind);
            listView.Items.Insert(ind + direction, i);

            RecalculateIndexes(ind + Math.Min(0, direction), ind + Math.Max(0, direction));
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedIndices.Count == 0) return;
            var ind = listView.SelectedIndices[0];
            tbName.Text = txbFile.TXBObjects[ind].Name;
            tbValue.Text = txbFile.TXBObjects[ind].Unknown1.ToString();
        }

        private void RecalculateIndexes(int ind = 0, int end = -1)
        {
            if (end == -1) end = listView.Items.Count;
            for (int i = ind; i < end; i++)
                listView.Items[i].SubItems[1].Text = i.ToString();
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            if (txbFile == null) return;
            if (listView.SelectedIndices.Count == 0) return;
            var ind = listView.SelectedIndices[0] + 1;
            txbFile.TXBObjects.Insert(ind, new TXBObject { Name = "" });
            listView.Items.Insert(ind, new ListViewItem(new [] { "", "", "0" }));
            RecalculateIndexes(ind);
        }

        private void bRemove_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices.Count == 0) return;
            var ind = listView.SelectedIndices[0];
            txbFile.TXBObjects.RemoveAt(ind);
            listView.Items.RemoveAt(ind);
            RecalculateIndexes(ind);
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            if (listView.SelectedIndices.Count == 0) return;
            var ind = listView.SelectedIndices[0];
            txbFile.TXBObjects[ind].Name = tbName.Text;
            listView.Items[ind].SubItems[0].Text = tbName.Text;
        }

        private void tbValue_TextChanged(object sender, EventArgs e)
        {
            if (listView.SelectedIndices.Count == 0) return;
            var ind = listView.SelectedIndices[0];
            Console.WriteLine(ind);
            Int32.TryParse(tbValue.Text, out txbFile.TXBObjects[ind].Unknown1);
            listView.Items[ind].SubItems[2].Text = txbFile.TXBObjects[ind].Unknown1.ToString();
        }
    }
}
