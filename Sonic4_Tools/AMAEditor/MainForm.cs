using System.Windows.Forms;

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
            tbFileName.Text = fileName;
            amaFile = new AMA();
            amaFile.Read(fileName);

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
            if (listBoxGroup1.SelectedIndices.Count == 0)
                return;

            var obj = amaFile.Group1[listBoxGroup1.SelectedIndex];
            //todo
        }

        private void listBoxGroup2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            dataGridView.Rows.Clear();
            if (listBoxGroup2.SelectedIndices.Count == 0)
                return;

            var obj = amaFile.Group2[listBoxGroup2.SelectedIndex];
            
            dataGridView.Rows.Add("Position X", obj.PositionX);
            dataGridView.Rows.Add("Position Y", obj.PositionY);
            dataGridView.Rows.Add("Size X", obj.SizeX);
            dataGridView.Rows.Add("Size Y", obj.SizeY);

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
            if (amaFile.Group1.Count > 0)
                listBoxGroup1_SelectedIndexChanged(null, null);
            else if (amaFile.Group2.Count > 0)
                listBoxGroup2_SelectedIndexChanged(null, null);
        }
    }
}
