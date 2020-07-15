using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

using SanityChecker;

namespace AMAEditor
{
    public class TXB: SanityCheckable
    {
        //https://github.com/OSA413/Sonic4_Tools/blob/master/docs/Specifications/TXB.md
        public bool Strange;
        public List<int> StrangeList = new List<int>();

        public List<TXBObject> TXBObjects = new List<TXBObject>();

        public static bool IsTXB(byte[] fileRaw)
        {
            return fileRaw.Length >= 0x18
                && fileRaw[0] == '#'
                && fileRaw[1] == 'T'
                && fileRaw[2] == 'X'
                && fileRaw[3] == 'B';
        }

        public static bool IsLittleEndian(byte[] fileRaw)
        {
            return BitConverter.ToInt32(fileRaw, 0x14) < 0xFFFF;
        }

        public static void ReverseEndianness(byte[] fileRaw)
        {
            var littleEndiann = IsLittleEndian(fileRaw);

            if (!littleEndiann)
            {
                Array.Reverse(fileRaw, 0x10, 4);
                Array.Reverse(fileRaw, 0x14, 4);
            }

            int fileNubmer = BitConverter.ToInt32(fileRaw, 0x10);
            int objectPointer = BitConverter.ToInt32(fileRaw, 0x14);

            for (int i = objectPointer; i < fileNubmer * 5 * 4 + objectPointer; i = i + 4)
            {
                Array.Reverse(fileRaw, i, 4);
            }

            if (littleEndiann)
            {
                Array.Reverse(fileRaw, 0x10, 4);
                Array.Reverse(fileRaw, 0x14, 4);
            }
        }

        private void StrangeIsntIt(byte[] fileRaw, int ptr, int intendedValue)
        {
            StrangeIsntIt(BitConverter.ToInt32(fileRaw, ptr), ptr, intendedValue);
        }

        private void StrangeIsntIt(int actualValue, int ptr, int intendedValue)
        {
            if (actualValue != intendedValue)
            {
                Strange = true;
                StrangeList.Add(ptr);
            }
        }

        public void Read(byte[] fileRaw)
        {
            Strange = false;
            StrangeList.Clear();
            TXBObjects.Clear();
            if (!IsLittleEndian(fileRaw))
                ReverseEndianness(fileRaw);

            var objectNumber = BitConverter.ToInt32(fileRaw, 0x10);
            var objectPointer = BitConverter.ToInt32(fileRaw, 0x14);
            StrangeIsntIt(fileRaw, 0x14, 0x18);

            var sb = new StringBuilder();

            for (int i = 0; i < objectNumber; i++)
            {
                var newTXB = new TXBObject();

                newTXB.Unknown0 = BitConverter.ToInt32(fileRaw, objectPointer + i * 5 * 4);
                newTXB.Unknown1 = BitConverter.ToInt32(fileRaw, objectPointer + i * 5 * 4 + 8);
                newTXB.Unknown2 = BitConverter.ToInt32(fileRaw, objectPointer + i * 5 * 4 + 0xC);
                newTXB.Unknown3 = BitConverter.ToInt32(fileRaw, objectPointer + i * 5 * 4 + 0x10);

                var namePointer = BitConverter.ToInt32(fileRaw, objectPointer + i * 5 * 4 + 4);
                for (int j = namePointer; fileRaw[j] != 0x00; j++)
                    sb.Append((char)fileRaw[j]);

                newTXB.Name = sb.ToString();
                sb.Clear();

                TXBObjects.Add(newTXB);
            }
        }

        public void Read(string fileName)
        {
            this.Read(File.ReadAllBytes(fileName));
        }

        public override byte[] Write()
        {
            var fileRaw = new byte[0];

            return fileRaw;
        }

        public void Write(string fileName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            File.WriteAllBytes(fileName, this.Write());
        }

        public string InfoToString()
        {
            var info = "";

            info += "Number of objects: " + TXBObjects.Count + "\n\n";
            for (int i = 0; i < TXBObjects.Count; i++)
            {
                info += "Object #" + i + "\n";
                info += "Name: " + TXBObjects[i].Name + "\n";
                info += "Unknown values:\n";
                info += TXBObjects[i].Unknown0 + " ";
                info += TXBObjects[i].Unknown1 + " ";
                info += TXBObjects[i].Unknown2 + " ";
                info += TXBObjects[i].Unknown3 + "\n"; 
                info += "\n";
            }
            return info;
        }
    }

    public class TXBObject
    {
        public string Name;
        public int Unknown0;
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
    }

    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (args[0] == "--check")
            {
                var sanityOnly = false;
                if (args[1] == "--sanity-only")
                    sanityOnly = true;

                var txb = new TXB();

                string file;
                if (args.Length > 0)
                    file = args[1 + (sanityOnly ? 1 : 0)];
                else
                    file = Console.ReadLine();

                txb.Read(file);
                txb.SanityCheck(file);

                if (sanityOnly)
                {
                    if (txb.WrongValues.Count == 0)
                        Console.WriteLine("OK");
                    else
                    {
                        foreach (int i in txb.WrongValues)
                            Console.Write("0x" + i.ToString("X") + " ");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine(txb.InfoToString());

                    if (txb.Strange)
                    {
                        Console.WriteLine("This TXB file is strange");
                        Console.WriteLine("Strange values at:");
                        foreach (int i in txb.StrangeList)
                            Console.Write("0x" + i.ToString("X") + " ");
                        Console.WriteLine();
                    }

                    Console.Write("Sanity check ");
                    if (txb.WrongValues.Count == 0)
                        Console.WriteLine("passed");
                    else
                    {
                        Console.WriteLine("failed (" + txb.WrongValues.Count + ")");
                        foreach (int i in txb.WrongValues)
                            Console.Write("0x" + i.ToString("X") + " ");
                        Console.WriteLine();
                    }

                    if (args.Length == 0)
                        Console.Read();
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new MainForm(args));
            }
        }
    }
}
