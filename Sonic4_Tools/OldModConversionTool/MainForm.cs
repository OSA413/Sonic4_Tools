using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

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

        static string Sha(string file)
        {
            byte[] hash;
            byte[] raw_file = File.ReadAllBytes(file);
            string str_hash = "";
            
            hash = new SHA512CryptoServiceProvider().ComputeHash(raw_file);
            
            foreach (byte b in hash) { str_hash += b.ToString("X"); }

            return str_hash;
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
                && lCsbEditorStatus.Text    == "OK")
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

        static List<string> UnpackFiles(string file, string mod_path, string output_path, string game_path)
        {
            List<string> FilesNeededToBeChecked = new List<string> { };
            List<string> DirsNeededToBeChecked = new List<string> { };

            string[] game_files;

            //Think up a comment that describes what this block does
            string the_orig_path;
            if (Directory.Exists(Path.GetDirectoryName(Path.Combine(output_path, "orig", file))))
            { the_orig_path = Path.Combine(output_path, "orig"); }
            else
            { the_orig_path = game_path; }
    

            if (Directory.Exists(Path.GetDirectoryName(Path.Combine(the_orig_path, file))))
            {
                game_files = Directory.GetFiles(Path.GetDirectoryName(Path.Combine(game_path, file)), "*", SearchOption.AllDirectories);
            }
            else
            {
                game_files = new string[0];
            }

            for (int i = 0; i < game_files.Length; i++)
            { game_files[i] = game_files[i].Substring(game_path.Length + 1); }

            if (game_files.Contains(file))
            {
                if (Sha(Path.Combine(output_path, file)) == Sha(Path.Combine(the_orig_path, file)))
                {
                    File.Delete(Path.Combine(output_path, file));
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(output_path, "orig", file)));
                    File.Copy(Path.Combine(game_path, file), Path.Combine(output_path, "orig", file), true);

                    if (file.EndsWith(".AMB", StringComparison.OrdinalIgnoreCase))
                    {
                        Process.Start("AMBPatcher.exe", Path.Combine(output_path, file)).WaitForExit();
                        File.Delete(Path.Combine(output_path, file));
                        Directory.Move(Path.Combine(output_path, file + "_extracted"), Path.Combine(output_path, file));

                        Process.Start("AMBPatcher.exe", Path.Combine(output_path, "orig", file)).WaitForExit();
                        File.Delete(Path.Combine(output_path, "orig", file));
                        Directory.Move(Path.Combine(output_path, "orig", file + "_extracted"), Path.Combine(output_path, "orig", file));

                        DirsNeededToBeChecked.Add(Path.Combine(output_path, file));
                    }
                    else if (file.EndsWith(".CSB", StringComparison.OrdinalIgnoreCase))
                    {
                        Process.Start("CsbEditor.exe", Path.Combine(output_path, file)).WaitForExit();
                        File.Delete(Path.Combine(output_path, file));

                        Process.Start("CsbEditor.exe", Path.Combine(output_path, "orig", file)).WaitForExit();
                        File.Delete(Path.Combine(output_path, "orig", file));

                        DirsNeededToBeChecked.Add(Path.Combine(output_path, "orig", file));
                    }
                }
            }

            return DirsNeededToBeChecked;
        }

        private void Convert()
        {
            statusBar.Text = "Checking directory existence...";
            Refresh();

            tbModPath.BackColor = Color.White;
            tbGamePath.BackColor = Color.White;

            if (!(Directory.Exists(tbModPath.Text) && Directory.Exists(tbGamePath.Text)))
            {
                if (!Directory.Exists(tbModPath.Text)) { tbModPath.BackColor = Color.FromArgb(255, 192, 192); }
                if (!Directory.Exists(tbGamePath.Text)) { tbGamePath.BackColor = Color.FromArgb(255, 192, 192); }
                MessageBox.Show("The mod root directory and/or the game root directory not found.", "Directory not found");
                return;
            }

            statusBar.Text = "Getting list of files...";
            Refresh();

            string[] mod_files = Directory.GetFiles(tbModPath.Text, "*", SearchOption.AllDirectories);
            for (int i = 0; i < mod_files.Length; i++)
            { mod_files[i] = mod_files[i].Substring(tbModPath.Text.Length + 1); }

            string[] game_files = Directory.GetFiles(tbGamePath.Text, "*", SearchOption.AllDirectories);
            for (int i = 0; i < game_files.Length; i++)
            { game_files[i] = game_files[i].Substring(tbGamePath.Text.Length + 1); }
            
            statusBar.Text = "Copying mod files...";
            Refresh();

            if (Directory.Exists(tbOutputPath.Text))
            { Directory.Delete(tbOutputPath.Text, true); }

            foreach (string file in mod_files)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(tbOutputPath.Text, file)));
                File.Copy(Path.Combine(tbModPath.Text, file), Path.Combine(tbOutputPath.Text, file), true);
            }

            statusBar.Text = "Comparing files... (first stage)";
            Refresh();
            
            List<string> FilesNeededToBeChecked = new List<string>(mod_files);

            while (FilesNeededToBeChecked.Count != 0)
            {
                Console.WriteLine(FilesNeededToBeChecked[0]);
                if (File.Exists(Path.Combine(tbModPath.Text, FilesNeededToBeChecked[0])))
                {
                    var a = UnpackFiles(FilesNeededToBeChecked[0], tbModPath.Text, tbOutputPath.Text, tbGamePath.Text);
                    if (a.Count != 0)
                    { FilesNeededToBeChecked.AddRange(a); }
                }
                FilesNeededToBeChecked.RemoveAt(0);
            }
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
            Convert();
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            RefreshStatus();
            IsReady();
        }
    }
}
