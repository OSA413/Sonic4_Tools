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

        private void StatusBarProgress()
        {
            char progress = statusBar.Text.Substring(statusBar.Text.Length - 1).ToCharArray()[0];

            switch (progress)
            {
                case '\\': progress = '|'; break;
                case '|': progress = '/'; break;
                case '/': progress = '-'; break;
                case '-': progress = '\\'; break;
            }

            statusBar.Text = statusBar.Text.Substring(0, statusBar.Text.Length - 1) + progress.ToString();
            Refresh();
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

        private void UnpackFiles(string folder, string mod_path, string output_path, string game_path)
        {
            bool check_again = false;
            
            //Think up a comment that describes what this block does
            string the_orig_path;
            if (Directory.Exists(Path.Combine(output_path, "orig", folder)))
            { the_orig_path = Path.Combine(output_path, "orig", folder); }
            else
            { the_orig_path = Path.Combine(game_path, folder); }

            StatusBarProgress();

            string[] mod_files = Directory.GetFiles(Path.Combine(mod_path, folder), "*", SearchOption.AllDirectories);

            string[] game_files = Directory.GetFiles(the_orig_path, "*", SearchOption.AllDirectories);
            
            StatusBarProgress();

            for (int i = 0; i < game_files.Length; i++)
            { game_files[i] = game_files[i].Substring(the_orig_path.Length + 1); }

            for (int i = 0; i < mod_files.Length; i++)
            { mod_files[i] = mod_files[i].Substring(mod_path.Length + folder.Length + 2); }

            StatusBarProgress();

            foreach (string file in mod_files)
            {
                if (game_files.Contains(file))
                {
                    string orig_file = Path.Combine(the_orig_path, file);
                    string output_orig_file = Path.Combine(output_path, "orig", folder, file);
                    string output_mod_file = Path.Combine(output_path, folder, file);
                    
                    if (Sha(output_mod_file) == Sha(orig_file))
                    {
                        StatusBarProgress();
                        File.Delete(output_mod_file);
                        check_again = true;
                    }
                    else
                    {
                        StatusBarProgress();
                        Directory.CreateDirectory(Path.GetDirectoryName(output_orig_file));
                        if (orig_file != output_orig_file)
                        {
                            File.Copy(orig_file, output_orig_file, true);
                        }

                        if (file.EndsWith(".AMB", StringComparison.OrdinalIgnoreCase))
                        {
                            StatusBarProgress();
                            Process.Start("AMBPatcher.exe", output_mod_file, ).WaitForExit();
                            File.Delete(output_mod_file);
                            Directory.Move(output_mod_file + "_extracted", output_mod_file);

                            StatusBarProgress();
                            Process.Start("AMBPatcher.exe", output_orig_file).WaitForExit();
                            File.Delete(output_orig_file);
                            Directory.Move(output_orig_file + "_extracted", output_orig_file);

                            check_again = true;
                        }
                        else if (file.EndsWith(".CSB", StringComparison.OrdinalIgnoreCase))
                        {
                            StatusBarProgress();
                            Process.Start("CsbEditor.exe", output_mod_file).WaitForExit();
                            File.Delete(output_mod_file);

                            StatusBarProgress();
                            Process.Start("CsbEditor.exe", output_orig_file).WaitForExit();
                            File.Delete(output_orig_file);

                            check_again = true;
                        }
                    }
                }
            }

            if (check_again)
            {
                StatusBarProgress();
                UnpackFiles(folder, output_path, output_path, Path.Combine(output_path, "orig"));
            }
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
            
            statusBar.Text = "Copying mod files... \\";
            Refresh();

            if (Directory.Exists(tbOutputPath.Text))
            { Directory.Delete(tbOutputPath.Text, true); }
            
            foreach (string file in mod_files)
            {
                StatusBarProgress();
                Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(tbOutputPath.Text, file)));
                File.Copy(Path.Combine(tbModPath.Text, file), Path.Combine(tbOutputPath.Text, file), true);
            }

            statusBar.Text = "Comparing files... \\";
            Refresh();

            foreach (string folder in Directory.GetDirectories(tbModPath.Text))
            {
                StatusBarProgress();
                string ffolder = Path.GetFileName(folder);
                UnpackFiles(ffolder, tbModPath.Text, tbOutputPath.Text, tbGamePath.Text);
            }

            statusBar.Text = "Removing temp files...";
            Refresh();

            Directory.Delete(Path.Combine(tbOutputPath.Text, "orig"), true);

            statusBar.Text = "Done";

            Process.Start("explorer", tbOutputPath.Text);
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
