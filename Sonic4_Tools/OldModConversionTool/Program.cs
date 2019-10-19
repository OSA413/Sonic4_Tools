using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace OldModConversionTool
{
    public static class Settings
    {
        public static string LastMod;
        public static string LastGame;
        public static string LastOutput;

        public static void Save()
        {
            string[] text = new string[]
            {
                "LastMod="    + LastMod,
                "LastGame="   + LastGame,
                "LastOutput=" + LastOutput,
            };

            File.WriteAllLines("omct.cfg", text);
        }

        public static void Load()
        {
            LastMod = LastGame = LastOutput = "";

            if (!File.Exists("omct.cfg")) return;

            string[] cfg_file = File.ReadAllLines("omct.cfg");

            foreach (string line in cfg_file)
            {
                if (!line.Contains("=")) continue;
                string key   = line.Substring(0, line.IndexOf("="));
                string value = line.Substring(line.IndexOf("=") + 1);

                switch (key)
                {
                    case "LastMod":     LastMod     = value; break;
                    case "LastGame":    LastGame    = value; break;
                    case "LastOutput":  LastOutput  = value; break;
                }
            }
        }
    }
    
    public static class MyFile
    {
        public static void Delete(string file)
        {
            //Program will crash if it tries to delete a read-only file
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }
    }

    public static class Extract
    {
        public static void AMB(string file)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine("Common", "AMBPatcher.exe"),
                Arguments = "\"" + file + "\"",
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process.Start(startInfo).WaitForExit();
        }

        public static void CSB(string file)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine("Common", "CsbEditor.exe"),
                Arguments = "\"" + file + "\"",
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process.Start(startInfo).WaitForExit();
        }
    }

    public static class MyDirectory
    {
        public static void RemoveRecursively(string dir)
        {
            foreach (string file in Directory.GetFiles(dir))
                MyFile.Delete(file);

            foreach (string dirr in Directory.GetDirectories(dir))
                MyDirectory.RemoveRecursively(dirr);

            Directory.Delete(dir);
        }

        public static void RemoveEmptyDirs(string parent_dir)
        {
            foreach (string dir in Directory.GetDirectories(parent_dir))
            { RemoveEmptyDirs(dir); }

            if (Directory.GetFileSystemEntries(parent_dir).Length == 0)
                MyDirectory.RemoveRecursively(parent_dir);
        }

        public static string SelectionDialog(int type)
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

        public static void OpenInExplorer(string folder)
        {
            string local_explorer = "";
            switch ((int)Environment.OSVersion.Platform)
            {
                //Windows
                case 2: local_explorer = "explorer"; break;
                //Linux (with xdg)
                case 4: local_explorer = "xdg-open"; break;
                //MacOS (not tested)
                case 6: local_explorer = "open"; break;
            }
            Process.Start(local_explorer, folder);
        }

        public static string[] GetRelativeFileNames(string path)
        {
            int extra_slash = path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? 0 : 1;
            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
                files[i] = files[i].Substring(path.Length + extra_slash);

            return files;
        }
    }

    public static class SHA
    {
        public static string GetSHA512(string file)
        {
            byte[] hash;
            byte[] raw_file = File.ReadAllBytes(file);
            string str_hash = "";

            hash = new SHA512CryptoServiceProvider().ComputeHash(raw_file);

            foreach (byte b in hash) { str_hash += b.ToString("X"); }

            return str_hash;
        }
    }

    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
