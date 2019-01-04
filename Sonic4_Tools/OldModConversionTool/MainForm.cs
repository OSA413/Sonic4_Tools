using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace OldModConversionTool
{
    public partial class MainForm:Form
    {
        public MainForm()
        {
            InitializeComponent();
            RefreshStatus();
            IsReady();
        }

        private void RefreshStatus()
        {
            //AMBPatcher
            lAMBPatcherStatus.Text = "AMBPatcher.exe is missing";
            lAMBPatcherStatus.ForeColor = Color.Red;
            if (File.Exists("AMBPatcher.exe"))
            {
                lAMBPatcherStatus.Text = "OK";
                lAMBPatcherStatus.ForeColor = Color.Green;
            }

            //CsbEditor
            lCsbEditorStatus.ForeColor = Color.Red;
            if (File.Exists("CsbEditor.exe") && File.Exists("SonicAudioLib.dll"))
            {
                lCsbEditorStatus.Text = "OK";
                lCsbEditorStatus.ForeColor = Color.Green;
            }
            else if (File.Exists("CsbEditor.exe"))
            {
                lCsbEditorStatus.Text = "SonicAudioLib.dll is missing";
            }
            else if (File.Exists("SonicAudioLib.dll"))
            {
                lCsbEditorStatus.Text = "CsbEditor.exe is missing";
            }
            else
            {
                lCsbEditorStatus.Text = "CsbEditor.exe and SonicAudioLib.dll are missing";
            }
        }

        private void IsReady()
        {
            bConvert.Enabled = false;
            if (   lAMBPatcherStatus.Text   == "OK"
                && lCsbEditorStatus.Text    == "OK"
                && tbModPath.Text       != ""
                && tbGamePath.Text      != ""
                && tbOutputPath.Text    != "")
            {
                bConvert.Enabled = true;
            }
        }

        static string DirectorySelectionDialog(int type)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.ValidateNames = false;
                ofd.CheckFileExists = false;
                ofd.CheckPathExists = true;
                ofd.FileName = "[DIRECTORY]";

                if (type == 0)
                { ofd.Title = "Select the root directory of your mod"; }
                else if (type == 1)
                { ofd.Title = "Select the root directory the game"; }
                else if (type == 2)
                { ofd.Title = "Select the output directory"; }

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return Path.GetDirectoryName(ofd.FileName);
                }
            }
            return null;
        }

        private void bModPath_Click(object sender, EventArgs e)
        {
            string path = DirectorySelectionDialog(0);
            if (path != null)
            { tbModPath.Text = path; }
        }

        private void bGamePath_Click(object sender, EventArgs e)
        {
            string path = DirectorySelectionDialog(1);
            if (path != null)
            { tbGamePath.Text = path; }
        }

        private void bOutputPath_Click(object sender, EventArgs e)
        {
            string path = DirectorySelectionDialog(2);
            if (path != null)
            { tbOutputPath.Text = path; }
        }

        private void bConvert_Click(object sender, EventArgs e)
        {

        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            RefreshStatus();
            IsReady();
        }
    }
}
