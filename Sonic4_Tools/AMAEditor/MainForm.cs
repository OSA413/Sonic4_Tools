using System.Windows.Forms;

namespace AMAEditor
{
    public partial class MainForm:Form
    {
        public MainForm(string[] args)
        {
            InitializeComponent();
            for (int i = 0; i < 25; i++)
                dataGridView1.Rows.Add();
        }
    }
}
