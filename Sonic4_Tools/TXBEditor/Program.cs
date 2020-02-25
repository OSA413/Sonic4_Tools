using System;
using System.Collections.Generic;
using System.IO;

namespace AMAEditor
{
    public class TXB
    {
        //https://github.com/OSA413/Sonic4_Tools/blob/master/docs/Specifications/TXB.md
        public bool Strange;
        public List<int> StrangeList;

        public static bool IsTXB(byte[] fileRaw)
        {
            return fileRaw.Length >= 0x18
                && fileRaw[0] == '#'
                && fileRaw[1] == 'T'
                && fileRaw[2] == 'X'
                && fileRaw[3] == 'B';
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

        public static List<int> SanityCheck(byte[] orig, byte[] recreation)
        {
            var dif = new List<int>();

            if (orig.Length == recreation.Length)
            {
                for (int i = 0; i < orig.Length; i++)
                    if (orig[i] != recreation[i])
                        dif.Add(i);
            }
            else
                dif.Add(-1);

            Console.WriteLine(orig.Length);
            Console.WriteLine(recreation.Length);

            return dif;
        }

        public void Read(byte[] fileRaw)
        {
        }

        public void Read(string fileName)
        {
            this.Read(File.ReadAllBytes(fileName));
        }

        public byte[] Write()
        {
            var fileRaw = new byte[0];

            return fileRaw;
        }

        public void Write(string fileName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            File.WriteAllBytes(fileName, this.Write());
        }
    }

    public class TXBObject
    {
        public string Name;
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello world!");
        }
    }
}
