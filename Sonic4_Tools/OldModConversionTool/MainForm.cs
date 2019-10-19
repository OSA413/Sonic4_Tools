﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OldModConversionTool
{
    public partial class MainForm:Form
    {
        public void SettingsSave()
        {
            Settings.LastMod = tbModPath.Text;
            Settings.LastGame = tbGamePath.Text;
            Settings.LastOutput = tbOutputPath.Text;

            Settings.Save();
        }

        public void SettingsLoad()
        {
            Settings.Load();

            tbModPath.Text    = Settings.LastMod;
            tbGamePath.Text   = Settings.LastGame;
            tbOutputPath.Text = Settings.LastOutput;
        }
        
        public MainForm()
        {
            InitializeComponent();
            RefreshStatus();
            IsReady();
            SettingsLoad();
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
                lCsbEditorStatus.Text = "SonicAudioLib.dll is missing";
            else if (File.Exists("Common/SonicAudioLib.dll"))
                lCsbEditorStatus.Text = "CsbEditor.exe is missing";
            else
                lCsbEditorStatus.Text = "CsbEditor.exe and SonicAudioLib.dll are missing";
        }

        private void MakeUIActive(bool active)
        {
            tbModPath.Enabled    =
            tbGamePath.Enabled   =
            tbOutputPath.Enabled =
            bConvert.Enabled     =
            bRefresh.Enabled     =
            bModPath.Enabled     =
            bGamePath.Enabled    =
            bOutputPath.Enabled  = active;
        }

        private void IsReady()
        {
            bConvert.Enabled = false;
            if (   lAMBPatcherStatus.Text   == "OK"
                && lCsbEditorStatus.Text    == "OK")
                bConvert.Enabled = true;
        }
        
        private void UnpackFiles(string folder, string mod_path, string output_path, string game_path)
        {
            statusBar.Text = "Comparing files in \"" + folder + "\"...";
            bool check_again = false;
            
            //Think up a comment that describes what this block does
            string the_orig_path;
            if (Directory.Exists(Path.Combine(output_path, "orig", folder)))
                the_orig_path = Path.Combine(output_path, "orig", folder);
            else
                the_orig_path = Path.Combine(game_path, folder);

            string[] mod_files = Directory.GetFiles(Path.Combine(mod_path, folder), "*", SearchOption.AllDirectories);

            string[] game_files = Directory.GetFiles(the_orig_path, "*", SearchOption.AllDirectories);

            for (int i = 0; i < game_files.Length; i++)
                game_files[i] = game_files[i].Substring(the_orig_path.Length + 1);

            for (int i = 0; i < mod_files.Length; i++)
                mod_files[i] = mod_files[i].Substring(mod_path.Length + folder.Length + 2);

            foreach (string file in mod_files)
            {
                statusBar.Text = "Comparing files... (" + Path.Combine(folder, file) + ")";

                if (game_files.Contains(file))
                {
                    string orig_file = Path.Combine(the_orig_path, file);
                    string output_orig_file = Path.Combine(output_path, "orig", folder, file);
                    string output_mod_file = Path.Combine(output_path, folder, file);

                    if (!File.Exists(output_mod_file)) continue;

                    if (SHA.GetSHA512(output_mod_file) == SHA.GetSHA512(orig_file))
                    {
                        MyFile.Delete(output_mod_file);
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
                            Extract.AMB(output_mod_file);
                            MyFile.Delete(output_mod_file);
                            Directory.Move(output_mod_file + "_extracted", output_mod_file);

                            Extract.AMB(output_orig_file);
                            MyFile.Delete(output_orig_file);
                            Directory.Move(output_orig_file + "_extracted", output_orig_file);

                            check_again = true;
                        }
                        else if (file.EndsWith(".CSB", StringComparison.OrdinalIgnoreCase))
                        {
                            Extract.CSB(output_mod_file);
                            MyFile.Delete(output_mod_file);

                            Extract.CSB(output_orig_file);
                            MyFile.Delete(output_orig_file);

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

                                Extract.CSB(output_mod_file);
                                MyFile.Delete(output_mod_file);
                                MyFile.Delete(output_mod_file.Substring(0, output_mod_file.Length - 4) + ".CPK");

                                Extract.CSB(output_orig_file);
                                MyFile.Delete(output_orig_file);
                                MyFile.Delete(output_orig_file.Substring(0, output_orig_file.Length - 4) + ".CPK");

                                check_again = true;
                            }
                        }
                    }
                }
            }

            if (check_again)
                UnpackFiles(folder, output_path, output_path, game_path);
        }

        async private void Convert()
        {
            await Task.Run(() => 
            {
                statusBar.Text = "Getting list of mod files...";
                string[] mod_files = MyDirectory.GetRelativeFileNames(tbModPath.Text);

                statusBar.Text = "Getting list of game files...";
                string[] game_files = MyDirectory.GetRelativeFileNames(tbGamePath.Text);
                
                if (Directory.Exists(tbOutputPath.Text))
                {
                    statusBar.Text = "Removing output directory...";
                    MyDirectory.RemoveRecursively(tbOutputPath.Text);
                }
                
                foreach (string file in mod_files)
                {
                    statusBar.Text = "Copying mod files... ("+file+")";
                    string output = Path.Combine(tbOutputPath.Text, file);
                    Directory.CreateDirectory(Path.GetDirectoryName(output));
                    File.Copy(Path.Combine(tbModPath.Text, file), output, true);
                    File.SetAttributes(output, FileAttributes.Normal);
                }
                
                foreach (string folder in Directory.GetDirectories(tbModPath.Text))
                {
                    string ffolder = Path.GetFileName(folder);
                    UnpackFiles(ffolder, tbModPath.Text, tbOutputPath.Text, tbGamePath.Text);
                }

                statusBar.Text = "Removing temp files...";
                MyDirectory.RemoveRecursively(Path.Combine(tbOutputPath.Text, "orig"));

                statusBar.Text = "Removing empty directories...";
                MyDirectory.RemoveEmptyDirs(tbOutputPath.Text);

                statusBar.Text = "Done";

                if (tbModPath.InvokeRequired)
                {
                    tbModPath.Invoke(new MethodInvoker(delegate {
                        //Code goes here
                        MakeUIActive(true);
                        ;
                    }));
                }
                
                MyDirectory.OpenInExplorer(tbOutputPath.Text);
            });
        }

        private void bModPath_Click(object sender, EventArgs e)
        {
            string path = MyDirectory.SelectionDialog(0);
            if (path != null)
                tbModPath.Text = path;
        }

        private void bGamePath_Click(object sender, EventArgs e)
        {
            string path = MyDirectory.SelectionDialog(1);
            if (path != null)
                tbGamePath.Text = path;
        }

        private void bOutputPath_Click(object sender, EventArgs e)
        {
            string path = MyDirectory.SelectionDialog(2);
            if (path != null)
                tbOutputPath.Text = path;
        }

        private void bConvert_Click(object sender, EventArgs e)
        {
            SettingsSave();

            statusBar.Text = "Checking directory existence...";

            tbOutputPath.BackColor = 
            tbModPath.BackColor = 
            tbGamePath.BackColor = Color.White;

            if (!(Directory.Exists(tbModPath.Text) && Directory.Exists(tbGamePath.Text)))
            {
                if (!Directory.Exists(tbModPath.Text))  tbModPath.BackColor  = Color.FromArgb(255, 192, 192);
                if (!Directory.Exists(tbGamePath.Text)) tbGamePath.BackColor = Color.FromArgb(255, 192, 192);
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

            MakeUIActive(false);
            Convert();
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            RefreshStatus();
            IsReady();
        }
    }
}