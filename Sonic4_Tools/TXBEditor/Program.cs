using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TXBEditor
{
    public static class TXB
    {
        public static List<byte[]> Read(byte[] RawFile)
        {
            int FileNumber = BitConverter.ToInt32(RawFile, 0x10);
            if (BitConverter.IsLittleEndian)
            { FileNumber = BitConverter.ToInt32(BitConverter.GetBytes(FileNumber).Reverse().ToArray(), 0); }

            List<byte[]> Return = new List<byte[]>();
            
            if (FileNumber > 0)
            {
                int NamePointer = BitConverter.ToInt32(RawFile, 0x1C);
                if (BitConverter.IsLittleEndian)
                { NamePointer = BitConverter.ToInt32(BitConverter.GetBytes(NamePointer).Reverse().ToArray(), 0); }

                for (int i = 0; i < (NamePointer - 0x20) / 4; i++)
                {
                    Return.Add(RawFile.Skip(0x20 + i * 4).Take(4).ToArray());
                }
            }

            return Return;
        }

        public static List<byte[]> Read(string FileName)
        {
            return TXB.Read(File.ReadAllBytes(FileName));
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
