﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OldModConversionTool
{
    public partial class MainForm:Form
    {
        public void Settings_Save()
        {
            string[] text = new string[]
            {
                "LastMod="    + tbModPath.Text,
                "LastGame="   + tbGamePath.Text,
                "LastOutput=" + tbOutputPath.Text,
            };

            File.WriteAllLines("omct.cfg", text);
        }

        public void Settings_Load()
        {
            if (!File.Exists("omct.cfg")) {return;}

            string[] cfg_file = File.ReadAllLines("omct.cfg");

            foreach (string line in cfg_file)
            {
                if (!line.Contains("=")) {continue;}

                string formatted_line = line.Substring(line.IndexOf("=") + 1);

                if (line.StartsWith("LastMod="))
                { tbModPath.Text    = formatted_line; }

                else if (line.StartsWith("LastGame="))
                { tbGamePath.Text   = formatted_line; }
                
                else if (line.StartsWith("LastOutput="))
                { tbOutputPath.Text = formatted_line; }
            }
        }
        
        static void File_Delete(string file)
        {
            //Program will crash if it tries to delete a read-only file
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        static void DirectoryRemoveRecursively(string dir)
        {
            foreach (string file in Directory.GetFiles(dir))
            {
                File_Delete(file);
            }

            foreach (string dirr in Directory.GetDirectories(dir))
            {
                DirectoryRemoveRecursively(dirr);
            }
            Directory.Delete(dir);
        }

        public MainForm()
        {
            InitializeComponent();
            RefreshStatus();
            IsReady();
            Settings_Load();
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

        static void RemoveEmptyDirs(string parent_dir)
        {
            foreach (string dir in Directory.GetDirectories(parent_dir))
            { RemoveEmptyDirs(dir); }

            if (Directory.GetFileSystemEntries(parent_dir).Length == 0)
            { DirectoryRemoveRecursively(parent_dir); }
        }

        private void RefreshStatus()
        {
            //AMBPatcher
            lAMBPatcherStatus.Text = "AMBPatcher.exe is missing";
            lAMBPatcherStatus.ForeColor = Color.Red;
            if (File.Exists("Common/AMBPatcher.exe"))
            {
                lAMBPatcherStatus.Text = "OK";
                lAMBPatcherStatus.ForeColor = Color.Green;
            }

            //CsbEditor
            lCsbEditorStatus.ForeColor = Color.Red;
            if (File.Exists("Common/CsbEditor.exe") && File.Exists("Common/SonicAudioLib.dll"))
            {
                lCsbEditorStatus.Text = "OK";
                lCsbEditorStatus.ForeColor = Color.Green;
            }
            else if (File.Exists("Common/CsbEditor.exe"))
            {
                lCsbEditorStatus.Text = "SonicAudioLib.dll is missing";
            }
            else if (File.Exists("Common/SonicAudioLib.dll"))
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

                switch (type)
                {
                    case 0: ofd.Title = "Select the root directory of your mod"; break;
                    case 1: ofd.Title = "Select the root directory the game"; break;
                    case 2: ofd.Title = "Select the output directory"; break;
                }

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return Path.GetDirectoryName(ofd.FileName);
                }
            }
            return null;
        }

        private void UnpackFiles(string folder, string mod_path, string output_path, string game_path)
        {
            statusBar.Text = "Comparing files in \"" + folder + "\"...";
            bool check_again = false;
            
            //Think up a comment that describes what this block does
            string the_orig_path;
            if (Directory.Exists(Path.Combine(output_path, "orig", folder)))
            { the_orig_path = Path.Combine(output_path, "orig", folder); }
            else
            { the_orig_path = Path.Combine(game_path, folder); }

            string[] mod_files = Directory.GetFiles(Path.Combine(mod_path, folder), "*", SearchOption.AllDirectories);

            string[] game_files = Directory.GetFiles(the_orig_path, "*", SearchOption.AllDirectories);

            for (int i = 0; i < game_files.Length; i++)
            { game_files[i] = game_files[i].Substring(the_orig_path.Length + 1); }

            for (int i = 0; i < mod_files.Length; i++)
            { mod_files[i] = mod_files[i].Substring(mod_path.Length + folder.Length + 2); }

            foreach (string file in mod_files)
            {
                statusBar.Text = "Comparing files... (" + Path.Combine(folder, file) + ")";

                if (game_files.Contains(file))
                {
                    string orig_file = Path.Combine(the_orig_path, file);
                    string output_orig_file = Path.Combine(output_path, "orig", folder, file);
                    string output_mod_file = Path.Combine(output_path, folder, file);

                    if (!File.Exists(output_mod_file))
                    { continue; }

                    if (Sha(output_mod_file) == Sha(orig_file))
                    {
                        File_Delete(output_mod_file);
                        check_again = true;
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(output_orig_file));
                        if (orig_file != output_orig_file)
                        {
                            File.Copy(orig_file, output_orig_file, true);
                        }

                        if (file.EndsWith(".AMB", StringComparison.OrdinalIgnoreCase))
                        {
                            ProcessStartInfo startInfo = new ProcessStartInfo
                            {
                                FileName = @".\Common\AMBPatcher.exe",
                                Arguments = output_mod_file,
                                WindowStyle = ProcessWindowStyle.Hidden
                            };

                            Process.Start(startInfo).WaitForExit();
                            File_Delete(output_mod_file);
                            Directory.Move(output_mod_file + "_extracted", output_mod_file);

                            startInfo.Arguments = output_orig_file;
                            Process.Start(startInfo).WaitForExit();
                            File_Delete(output_orig_file);
                            Directory.Move(output_orig_file + "_extracted", output_orig_file);

                            check_again = true;
                        }
                        else if (file.EndsWith(".CSB", StringComparison.OrdinalIgnoreCase))
                        {
                            ProcessStartInfo startInfo = new ProcessStartInfo
                            {
                                FileName = @".\Common\CsbEditor.exe",
                                Arguments = output_mod_file,
                                WindowStyle = ProcessWindowStyle.Hidden
                            };
                            Process.Start(startInfo).WaitForExit();
                            File_Delete(output_mod_file);

                            startInfo.Arguments = output_orig_file;
                            Process.Start(startInfo).WaitForExit();
                            File_Delete(output_orig_file);

                            check_again = true;
                        }
                        else if (file.EndsWith(".CPK", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!mod_files.Contains(file.Substring(0, file.Length - 4) + ".CBS"))
                            {
                                orig_file = Path.Combine(game_path, folder, file.Substring(0, file.Length - 4) + ".CSB");
                                output_orig_file = output_orig_file.Substring(0, output_orig_file.Length - 4) + ".CSB";
                                output_mod_file = output_mod_file.Substring(0, output_mod_file.Length - 4) + ".CSB";

                                File.Copy(orig_file, output_orig_file);
                                File.Copy(orig_file, output_mod_file);

                                ProcessStartInfo startInfo = new ProcessStartInfo
                                {
                                    FileName = @".\Common\CsbEditor.exe",
                                    Arguments = output_mod_file,
                                    WindowStyle = ProcessWindowStyle.Hidden
                                };
                                Process.Start(startInfo).WaitForExit();
                                File_Delete(output_mod_file);
                                File_Delete(output_mod_file.Substring(0, output_mod_file.Length - 4) + ".CPK");

                                startInfo.Arguments = output_orig_file;
                                Process.Start(startInfo).WaitForExit();
                                File_Delete(output_orig_file);
                                File_Delete(output_orig_file.Substring(0, output_orig_file.Length - 4) + ".CPK");

                                check_again = true;
                            }
                        }

                    }
                }
            }

            if (check_again)
            {
                UnpackFiles(folder, output_path, output_path, game_path);
            }
        }

        async private void Convert()
        {
            await Task.Run(() => 
            {
                statusBar.Text = "Getting list of mod files...";

                string[] mod_files = Directory.GetFiles(tbModPath.Text, "*", SearchOption.AllDirectories);
                for (int i = 0; i < mod_files.Length; i++)
                { mod_files[i] = mod_files[i].Substring(tbModPath.Text.Length + 1); }

                statusBar.Text = "Getting list of game files...";

                string[] game_files = Directory.GetFiles(tbGamePath.Text, "*", SearchOption.AllDirectories);
                for (int i = 0; i < game_files.Length; i++)
                { game_files[i] = game_files[i].Substring(tbGamePath.Text.Length + 1); }
                
                if (Directory.Exists(tbOutputPath.Text))
                {
                    statusBar.Text = "Removing output directory...";
                    DirectoryRemoveRecursively(tbOutputPath.Text);
                }
                
                foreach (string file in mod_files)
                {
                    statusBar.Text = "Copying mod files... ("+file+")";
                    Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(tbOutputPath.Text, file)));
                    File.Copy(Path.Combine(tbModPath.Text, file), Path.Combine(tbOutputPath.Text, file), true);
                }
                
                foreach (string folder in Directory.GetDirectories(tbModPath.Text))
                {
                    string ffolder = Path.GetFileName(folder);
                    UnpackFiles(ffolder, tbModPath.Text, tbOutputPath.Text, tbGamePath.Text);
                }

                statusBar.Text = "Removing temp files...";
                DirectoryRemoveRecursively(Path.Combine(tbOutputPath.Text, "orig"));

                statusBar.Text = "Removing empty directories...";
                RemoveEmptyDirs(tbOutputPath.Text);

                statusBar.Text = "Done";

                if (tbModPath.InvokeRequired)
                {
                    tbModPath.Invoke(new MethodInvoker(delegate {
                        //Code goes here
                        tbModPath.Enabled    =
                        tbGamePath.Enabled   =
                        tbOutputPath.Enabled =
                        bConvert.Enabled     =
                        bRefresh.Enabled     =
                        bModPath.Enabled     =
                        bGamePath.Enabled    =
                        bOutputPath.Enabled  = true;
                        ;
                    }));
                }
                

                string local_explorer = "";
                switch ((int) Environment.OSVersion.Platform)
                {
                    //Windows
                    case 2: local_explorer = "explorer"; break;
                    //Linux (with xdg)
                    case 4: local_explorer = "xdg-open"; break;
                    //MacOS (not tested)
                    case 6: local_explorer = "open"; break;
                }

                Process.Start(local_explorer, tbOutputPath.Text);
            });
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
            Settings_Save();

            statusBar.Text = "Checking directory existence...";

            tbOutputPath.BackColor = 
            tbModPath.BackColor = 
            tbGamePath.BackColor = Color.White;

            if (!(Directory.Exists(tbModPath.Text) && Directory.Exists(tbGamePath.Text)))
            {
                if (!Directory.Exists(tbModPath.Text)) { tbModPath.BackColor = Color.FromArgb(255, 192, 192); }
                if (!Directory.Exists(tbGamePath.Text)) { tbGamePath.BackColor = Color.FromArgb(255, 192, 192); }
                MessageBox.Show("The mod root directory and/or the game root directory not found.", "Directory not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Directory.Exists(tbOutputPath.Text))
            {
                if (Directory.GetFileSystemEntries(tbOutputPath.Text).Length != 0)
                {
                    DialogResult wait_continue = MessageBox.Show("The output directory is not empty. If you continue, it will delete all files in it! Are you sure?"
                                                                , "Watch out, you are going to crash!"
                                                                , MessageBoxButtons.OKCancel
                                                                , MessageBoxIcon.Exclamation);
                    if (wait_continue != DialogResult.OK) { statusBar.Text = "Canceled"; return; }
                }
            }

            tbModPath.Enabled    =
            tbGamePath.Enabled   =
            tbOutputPath.Enabled =
            bConvert.Enabled     =
            bRefresh.Enabled     =
            bModPath.Enabled     =
            bGamePath.Enabled    =
            bOutputPath.Enabled  = false;

            Convert();
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            RefreshStatus();
            IsReady();
        }
    }
}