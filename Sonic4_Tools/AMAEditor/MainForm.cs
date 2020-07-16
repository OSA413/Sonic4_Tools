using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace AMAEditor
{
    public partial class MainForm:Form
    {
        AMA amaFile;

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
            amaFile = new AMA();
            amaFile.Read(fileName);

            amaFile.SanityCheck(fileName);

            if (amaFile.WrongValues.Count > 0)
            {
                Console.WriteLine("Sanity check failed");
                statusBar.Text = "Warning! File won't be saved properly!";
                File.WriteAllText(fileName + "_check.txt", String.Join(" ", amaFile.StrangeList));
            }

            listBoxGroup1.Items.Clear();
            foreach (var obj in amaFile.Group1)
                listBoxGroup1.Items.Add(obj.Name);

            listBoxGroup2.Items.Clear();
            foreach (var obj in amaFile.Group2)
                listBoxGroup2.Items.Add(obj.Name);

            if (amaFile.Group1.Count > 0)
            {
                tabControl.SelectedIndex = 0;
                listBoxGroup1.Select();
                listBoxGroup1.SelectedIndex = 0;
            }
            else if (amaFile.Group2.Count > 0)
            {
                tabControl.SelectedIndex = 1;
                listBoxGroup2.Select();
                listBoxGroup2.SelectedIndex = 0;
            }
        }

        public void SaveAmaFile(string fileName)
        {
            amaFile.Write(fileName);
        }

        private void bOpenFile_Click(object sender, System.EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "AMA files|*.AMA|All files (*.*)|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    OpenAmaFile(ofd.FileName);
                }
            }
        }

        private void listBoxGroup1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            dataGridView.Rows.Clear();
            if (listBoxGroup1.SelectedIndices.Count == 0)
                return;
            listBoxGroup2.ClearSelected();

            var obj = amaFile.Group1[listBoxGroup1.SelectedIndex];

            var G1List = new string[amaFile.Group1.Count + 1];
            G1List[0] = "(None)";
            for (int i = 0; i < amaFile.Group1.Count; i++)
                G1List[i + 1] = amaFile.Group1[i].Name;

            var comboboxG1 = new DataGridViewComboBoxCell();
            comboboxG1.Items.AddRange(G1List);
            comboboxG1.Value = "(None)";

            var G2List = new string[amaFile.Group2.Count + 1];
            G2List[0] = "(None)";
            for (int i = 0; i < amaFile.Group2.Count; i++)
                G2List[i + 1] = amaFile.Group2[i].Name;

            var comboboxG2 = new DataGridViewComboBoxCell();
            comboboxG2.Items.AddRange(G2List);
            comboboxG2.Value = "(None)";

            dataGridView.Rows.Add("Group 1 Child 0", "");
            dataGridView[1, 0] = (DataGridViewCell)comboboxG1.Clone();
            if (obj.G1Child0 != null)
                dataGridView[1, 0].Value = G1List[Array.IndexOf(G1List, obj.G1Child0.Name)];
            dataGridView.Rows.Add("Group 1 Child 1", "");
            dataGridView[1, 1] = (DataGridViewCell)comboboxG1.Clone();
            if (obj.G1Child1 != null)
                dataGridView[1, 1].Value = G1List[Array.IndexOf(G1List, obj.G1Child1.Name)];
            dataGridView.Rows.Add("Group 1 Parent", "");
            dataGridView[1, 2] = (DataGridViewCell)comboboxG1.Clone();
            if (obj.Parent != null)
                dataGridView[1, 2].Value = G1List[Array.IndexOf(G1List, obj.Parent.Name)];
            dataGridView.Rows.Add("Group 2 Child 0", "");
            dataGridView[1, 3] = (DataGridViewCell)comboboxG2.Clone();
            if (obj.G2Child0 != null)
                dataGridView[1, 3].Value = G2List[Array.IndexOf(G2List, obj.G2Child0.Name)];
        }

        private void listBoxGroup2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            dataGridView.Rows.Clear();
            if (listBoxGroup2.SelectedIndices.Count == 0)
                return;
            listBoxGroup1.ClearSelected();

            var obj = amaFile.Group2[listBoxGroup2.SelectedIndex];

            dataGridView.Rows.Add("Position X", obj.PositionX);
            dataGridView.Rows.Add("Position Y", obj.PositionY);
            dataGridView.Rows.Add("Size X", obj.SizeX);
            dataGridView.Rows.Add("Size Y", obj.SizeY);
            dataGridView.Rows.Add("Size Y", obj.UVLeftEdge);
            dataGridView.Rows.Add("Size Y", obj.UVUpperEdge);
            dataGridView.Rows.Add("Size Y", obj.UVRightEdge);
            dataGridView.Rows.Add("Size Y", obj.UVBottomEdge);

            if (cbUnknowValues.Checked)
            {
                dataGridView.Rows.Add("Unknown0", obj.Unknown0);
                dataGridView.Rows.Add("Unknown1", obj.Unknown1);
                dataGridView.Rows.Add("Unknown2", obj.Unknown2);
                dataGridView.Rows.Add("Unknown3", obj.Unknown3);
                dataGridView.Rows.Add("Unknown4", obj.Unknown4);
                dataGridView.Rows.Add("Unknown5", obj.Unknown5);
                dataGridView.Rows.Add("Unknown6", obj.Unknown6);
                dataGridView.Rows.Add("Unknown7", obj.Unknown7);
                dataGridView.Rows.Add("Unknown8", obj.Unknown8);
                dataGridView.Rows.Add("Unknown9", obj.Unknown9);
                dataGridView.Rows.Add("Unknown10", obj.Unknown10);
                dataGridView.Rows.Add("Unknown11", obj.Unknown11);
                dataGridView.Rows.Add("Unknown12", obj.Unknown12);
                dataGridView.Rows.Add("Unknown13", obj.Unknown13);
                dataGridView.Rows.Add("Unknown14", obj.Unknown14);
                dataGridView.Rows.Add("Unknown15", obj.Unknown15);
                dataGridView.Rows.Add("Unknown16", obj.Unknown16);
                dataGridView.Rows.Add("Unknown17", obj.Unknown17);
                dataGridView.Rows.Add("Unknown18", obj.Unknown18);
                dataGridView.Rows.Add("Unknown19", obj.Unknown19);
                dataGridView.Rows.Add("Unknown20", obj.Unknown20);
                dataGridView.Rows.Add("Unknown21", obj.Unknown21);
            }
        }

        private void cbUnknowValues_CheckedChanged(object sender, System.EventArgs e)
        {
            if (amaFile == null)
                return;
            if (listBoxGroup1.SelectedIndices.Count > 0)
                listBoxGroup1_SelectedIndexChanged(null, null);
            else if (listBoxGroup2.SelectedIndices.Count > 0)
                listBoxGroup2_SelectedIndexChanged(null, null);
        }

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 1)
                return;

            var cell = dataGridView[1, e.RowIndex];

            if (((Control)sender).Name == "listBoxGroup1")
            {
                var ind = listBoxGroup1.SelectedIndex;

                switch (e.RowIndex)
                {
                    case 0: TryConvertApplyUpdate(cell, ref amaFile.Group1[ind].G1Child0.Name); break;
                    case 1: TryConvertApplyUpdate(cell, ref amaFile.Group1[ind].G1Child1.Name); break;
                    case 2: TryConvertApplyUpdate(cell, ref amaFile.Group1[ind].Parent.Name);   break;
                    case 3: TryConvertApplyUpdate(cell, ref amaFile.Group1[ind].G2Child0.Name); break;
                }
            }
            else if (((Control)sender).Name == "listBoxGroup2")
            {
                var ind = listBoxGroup2.SelectedIndex;

                switch (e.RowIndex)
                {
                    case 0: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].PositionX); break;
                    case 1: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].PositionY); break;
                    case 2: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].SizeX); break;
                    case 3: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].SizeY); break;
                    case 4: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].UVLeftEdge); break;
                    case 5: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].UVUpperEdge); break;
                    case 6: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].UVRightEdge); break;
                    case 7: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].UVBottomEdge); break;
                    case 8: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown0); break;
                    case 9: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown1); break;
                    case 10: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown2); break;
                    case 11: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown3); break;
                    case 12: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown4); break;
                    case 13: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown5); break;
                    case 14: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown6); break;
                    case 15: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown7); break;
                    case 16: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown8); break;
                    case 17: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown9); break;
                    case 18: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown10); break;
                    case 19: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown11); break;
                    case 20: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown12); break;
                    case 21: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown13); break;
                    case 22: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown14); break;
                    case 23: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown15); break;
                    case 24: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown16); break;
                    case 25: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown17); break;
                    case 26: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown18); break;
                    case 27: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown19); break;
                    case 28: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown20); break;
                    case 29: TryConvertApplyUpdate(cell, ref amaFile.Group2[ind].Unknown21); break;
                }
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
            if (amaFile != null)
                amaFile.Write(tbFileName.Text);
        }
    }
}
