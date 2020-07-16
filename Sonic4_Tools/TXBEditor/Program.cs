using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using SanityChecker;

namespace TXBEditor
{
    public class TXB: SanityCheckable
    {
        //https://github.com/OSA413/Sonic4_Tools/blob/master/docs/Specifications/TXB.md
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
            return BitConverter.ToInt32(fileRaw, 4) < 0xFFFF;
        }

        public static void ReverseEndianness(byte[] fileRaw)
        {
            var littleEndiann = IsLittleEndian(fileRaw);

            if (!littleEndiann)
            {
                Array.Reverse(fileRaw, 4, 4);
                Array.Reverse(fileRaw, 0x10, 4);
                Array.Reverse(fileRaw, 0x14, 4);
            }

            int fileNubmer = BitConverter.ToInt32(fileRaw, 0x10);
            int objectPointer = BitConverter.ToInt32(fileRaw, 0x14);

            for (int i = objectPointer; i < fileNubmer * 5 * 4 + objectPointer; i = i + 4)
                Array.Reverse(fileRaw, i, 4);

            if (littleEndiann)
            {
                Array.Reverse(fileRaw, 4, 4);
                Array.Reverse(fileRaw, 0x10, 4);
                Array.Reverse(fileRaw, 0x14, 4);
            }
        }

        public override void Read(byte[] fileRaw)
        {
            base.Read(fileRaw);
            TXBObjects.Clear();
            if (!IsLittleEndian(fileRaw))
                ReverseEndianness(fileRaw);

            var objectNumber = BitConverter.ToInt32(fileRaw, 0x10);
            var objectPointer = BitConverter.ToInt32(fileRaw, 0x14);

            var sb = new StringBuilder();

            for (int i = 0; i < objectNumber; i++)
            {
                var newTXB = new TXBObject();

                StrangeIsntIt(fileRaw, objectPointer + i * 5 * 4, 0);
                newTXB.Unknown1 = BitConverter.ToInt32(fileRaw, objectPointer + i * 5 * 4 + 0x8);
                StrangeIsntIt(fileRaw, objectPointer + i * 5 * 4 + 0xC, 0);
                StrangeIsntIt(fileRaw, objectPointer + i * 5 * 4 + 0x10, 0);

                var namePointer = BitConverter.ToInt32(fileRaw, objectPointer + i * 5 * 4 + 4);
                for (int j = namePointer; fileRaw[j] != 0x00; j++)
                    sb.Append((char)fileRaw[j]);

                newTXB.Name = sb.ToString();
                sb.Clear();

                TXBObjects.Add(newTXB);
            }
        }

        public override byte[] Write()
        {
            var fileLength = 0;

            //Header
            fileLength += 0x18;

            foreach (var o in TXBObjects)
            {
                fileLength += 5 * 4;
                fileLength += o.Name.Length + 1;
            }

            var fileRaw = new byte[fileLength];

            //Header
            Array.Copy(Encoding.ASCII.GetBytes("#TXB"), 0, fileRaw, 0, 4);
            Array.Copy(BitConverter.GetBytes(0x10), 0, fileRaw, 4, 4);
            Array.Copy(BitConverter.GetBytes(TXBObjects.Count), 0, fileRaw, 0x10, 4);
            if (TXBObjects.Count > 0)
                Array.Copy(BitConverter.GetBytes(0x18), 0, fileRaw, 0x14, 4);

            //Body
            var namePointer = 0x18 + TXBObjects.Count * 5 * 4;
            for (int i = 0; i < TXBObjects.Count; i++)
            {
                var ptr = 0x18 + i * 5 * 4;
                var o = TXBObjects[i];
                Array.Copy(BitConverter.GetBytes(o.Unknown1), 0, fileRaw, ptr + 0x8, 4);

                //Name pointer
                Array.Copy(BitConverter.GetBytes(namePointer), 0, fileRaw, ptr + 4, 4);
                foreach (char c in o.Name)
                    fileRaw[namePointer++] = (byte)c;
                namePointer++;
            }

            ReverseEndianness(fileRaw);

            return fileRaw;
        }

        public override string InfoToString()
        {
            var info = "";

            info += "Number of objects: " + TXBObjects.Count + "\n\n";
            for (int i = 0; i < TXBObjects.Count; i++)
            {
                info += "Object #" + i + "\n";
                info += "Name: " + TXBObjects[i].Name + "\n";
                info += "Unknown value: " + TXBObjects[i].Unknown1 + "\n"; 
                info += "\n";
            }
            return info;
        }
    }

    public class TXBObject
    {
        public string Name;
        public int Unknown1;
    }

    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (!SanityCheckable.PrintChecksIfNecessary<TXB>(args))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm(args));
            }
        }
    }
}
