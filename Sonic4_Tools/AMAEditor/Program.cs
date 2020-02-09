using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AMAEditor
{
    public class AMA
    {
        //https://github.com/OSA413/Sonic4_Tools/blob/master/docs/Specifications/AMA.md
        public List<Group1> Group1;
        public List<Group2> Group2;
        public bool Strange;
        public List<int>    StrangeList;

        public static bool IsAMA(byte[] fileRaw)
        {
            return fileRaw.Length >= 0x20
                && fileRaw[0] == '#'
                && fileRaw[1] == 'A'
                && fileRaw[2] == 'M'
                && fileRaw[3] == 'A';
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
            Group1 = new List<Group1>();
            Group2 = new List<Group2>();
            Strange = false;
            StrangeList = new List<int>();

            if (!AMA.IsAMA(fileRaw))
                return;

            StrangeIsntIt(fileRaw, 0x4, 0);

            int g1Number = BitConverter.ToInt32(fileRaw, 0x8);
            int g2Number = BitConverter.ToInt32(fileRaw, 0xC);

            int g1ListPointer = BitConverter.ToInt32(fileRaw, 0x10);
            int g2ListPointer = BitConverter.ToInt32(fileRaw, 0x14);

            int g1NameListPointer = BitConverter.ToInt32(fileRaw, 0x18);
            int g2NameListPointer = BitConverter.ToInt32(fileRaw, 0x1C);

            var g1Dict = new Dictionary<int, Group1>();
            var g2Dict = new Dictionary<int, Group2>();

            for (int i = 0; i < g1Number; i++)
            {
                int ptr = BitConverter.ToInt32(fileRaw, g1ListPointer + 4 * i);
                g1Dict.Add(ptr, new Group1());
                Group1.Add(g1Dict[ptr]);
            }

            for (int i = 0; i < g2Number; i++)
            {
                int ptr = BitConverter.ToInt32(fileRaw, g2ListPointer + 4 * i);
                g2Dict.Add(ptr, new Group2());
                Group2.Add(g2Dict[ptr]);
            }

            foreach (int ptr in g2Dict.Keys)
            {
                StrangeIsntIt(fileRaw, ptr, 0x09);
                StrangeIsntIt(fileRaw, ptr + 0x04, Group2.IndexOf(g2Dict[ptr]));
                StrangeIsntIt(fileRaw, ptr + 0x08, 0x01);
                StrangeIsntIt(fileRaw, ptr + 0x0C, 0x00);

                g2Dict[ptr].PositionX = BitConverter.ToSingle(fileRaw, ptr + 0x10);
                g2Dict[ptr].PositionY = BitConverter.ToSingle(fileRaw, ptr + 0x14);
                g2Dict[ptr].SizeX = BitConverter.ToSingle(fileRaw, ptr + 0x18);
                g2Dict[ptr].SizeY = BitConverter.ToSingle(fileRaw, ptr + 0x1C);

                StrangeIsntIt(fileRaw, ptr + 0x20, 0x00);
                for (int i = 0x28; i < 0x40; i = i + 4)
                    StrangeIsntIt(fileRaw, ptr + i, 0x00);

                int ptr0 = BitConverter.ToInt32(fileRaw, ptr + 0x24);

                g2Dict[ptr].Unknown0 = BitConverter.ToInt32(fileRaw, ptr0);
                g2Dict[ptr].Unknown1 = BitConverter.ToInt32(fileRaw, ptr0 + 4);
                g2Dict[ptr].Unknown2 = BitConverter.ToInt32(fileRaw, ptr0 + 8);

                int ptr1 = BitConverter.ToInt32(fileRaw, ptr0 + 0xC);

                g2Dict[ptr].Unknown3 = BitConverter.ToInt32(fileRaw, ptr1);

                int ptr2 = BitConverter.ToInt32(fileRaw, ptr0 + 0x10);

                g2Dict[ptr].Unknown4 = BitConverter.ToInt32(fileRaw, ptr2);
                g2Dict[ptr].Unknown5 = BitConverter.ToInt32(fileRaw, ptr2 + 0x04);
                g2Dict[ptr].Unknown6 = BitConverter.ToInt32(fileRaw, ptr2 + 0x08);
                g2Dict[ptr].Unknown7 = BitConverter.ToSingle(fileRaw, ptr2 + 0x0C);

                for (int i = 0x54; i < 0x70; i = i + 4)
                    StrangeIsntIt(fileRaw, ptr + i, 0x00);
                StrangeIsntIt(fileRaw, ptr + 0x74, 0x00);

                g2Dict[ptr].UVLeftEdge = BitConverter.ToSingle(fileRaw, ptr2 + 0x10);
                g2Dict[ptr].UVUpperEdge = BitConverter.ToSingle(fileRaw, ptr2 + 0x14);
                g2Dict[ptr].UVRightEdge = BitConverter.ToSingle(fileRaw, ptr2 + 0x18);
                g2Dict[ptr].UVBottomEdge = BitConverter.ToSingle(fileRaw, ptr2 + 0x1C);
            }

            foreach (int ptr in g1Dict.Keys)
            {
                g1Dict[ptr].G1Children = new List<Group1>();
                g1Dict[ptr].G2Children = new List<Group2>();

                StrangeIsntIt(fileRaw, ptr, 0x00);
                StrangeIsntIt(fileRaw, ptr + 4, Group1.IndexOf(g1Dict[ptr]));

                int ptrG1Child = BitConverter.ToInt32(fileRaw, ptr + 0x8);
                if (ptrG1Child != 0)
                    g1Dict[ptr].G1Children.Add(g1Dict[ptrG1Child]);

                ptrG1Child = BitConverter.ToInt32(fileRaw, ptr + 0xC);
                if (ptrG1Child != 0)
                    g1Dict[ptr].G1Children.Add(g1Dict[ptrG1Child]);

                int ptrParent = BitConverter.ToInt32(fileRaw, ptr + 0x10);
                if (ptrParent != 0)
                    g1Dict[ptr].Parent = g1Dict[ptrParent];

                int ptrG2Child = BitConverter.ToInt32(fileRaw, ptr + 0x14);
                if (ptrG2Child != 0)
                    g1Dict[ptr].G2Children.Add(g2Dict[ptrG2Child]);

                StrangeIsntIt(fileRaw, ptr + 0x18, 0x00);
                StrangeIsntIt(fileRaw, ptr + 0x1C, 0x00);
            }

            var sb = new StringBuilder();

            for (int i = 0; i < g1Number; i++)
            {
                int ptr = BitConverter.ToInt32(fileRaw, g1NameListPointer + 4*i);
                for (; fileRaw[ptr] != 0x00; ptr++)
                    sb.Append((char)fileRaw[ptr]);
                Group1[i].Name = sb.ToString();
                sb.Clear();
            }

            for (int i = 0; i < g2Number; i++)
            {
                int ptr = BitConverter.ToInt32(fileRaw, g2NameListPointer + 4 * i);
                for (; fileRaw[ptr] != 0x00; ptr++)
                    sb.Append((char)fileRaw[ptr]);
                Group2[i].Name = sb.ToString();
                sb.Clear();
            }
        }

        public void Read(string fileName)
        {
            this.Read(File.ReadAllBytes(fileName));
        }

        public byte[] Write()
        {
            return new byte[0];
        }

        public void Write(string fileName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            File.WriteAllBytes(fileName, this.Write());
        }
    }

    public class Group1
    {
        public string       Name;
        public Group1       Parent;
        public List<Group1> G1Children;
        public List<Group2> G2Children;
    }

    public class Group2
    {
        public string   Name;

        public float PositionX;
        public float PositionY;
        public float SizeX;
        public float SizeY;

        public float UVLeftEdge;
        public float UVUpperEdge;
        public float UVRightEdge;
        public float UVBottomEdge;

        public int Unknown0;
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public int Unknown5;
        public int Unknown6;
        public float Unknown7;
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            var ama = new AMA();

            if (args.Length > 0)
            {
                ama.Read(args[0]);
            }
            else
            {
                ama.Read(Console.ReadLine());
            }

            Console.WriteLine("Group 1 objects:");
            Console.WriteLine("Number of objects: " + ama.Group1.Count + "\n");
            for (int i = 0; i < ama.Group1.Count; i++)
            {
                Console.WriteLine("Object #" + i);
                Console.WriteLine("Name: " + ama.Group1[i].Name);
                if (ama.Group1[i].Parent != null)
                    Console.WriteLine("Parent name: " + ama.Group1[i].Parent.Name);

                Console.WriteLine("Group 1 child number: " + ama.Group1[i].G1Children.Count);
                for (int j = 0; j < ama.Group1[i].G1Children.Count; j++)
                    Console.WriteLine("\tChild #" + j + "'s name: " + ama.Group1[i].G1Children[j].Name);

                Console.WriteLine("Group 2 child number: " + ama.Group1[i].G2Children.Count);
                for (int j = 0; j < ama.Group1[i].G2Children.Count; j++)
                    Console.WriteLine("\tChild #" + j + "'s name: " + ama.Group1[i].G2Children[j].Name);

                Console.WriteLine();
            }

            Console.WriteLine("Group 2 objects:");
            Console.WriteLine("Number of objects: " + ama.Group2.Count + "\n");
            for (int i = 0; i < ama.Group2.Count; i++)
            {
                Console.WriteLine("Object #" + i);
                Console.WriteLine("Name: " + ama.Group2[i].Name);

                Console.WriteLine("PositionX: " + ama.Group2[i].PositionX);
                Console.WriteLine("PositionY: " + ama.Group2[i].PositionY);
                Console.WriteLine("SizeX: " + ama.Group2[i].SizeX);
                Console.WriteLine("SizeY: " + ama.Group2[i].SizeY);

                Console.WriteLine("UVLeftEdge: " + ama.Group2[i].UVLeftEdge);
                Console.WriteLine("UVUpperEdge: " + ama.Group2[i].UVUpperEdge);
                Console.WriteLine("UVRightEdge: " + ama.Group2[i].UVRightEdge);
                Console.WriteLine("UVBottomEdge: " + ama.Group2[i].UVBottomEdge);

                Console.WriteLine("Unknown values:");
                Console.Write(ama.Group2[i].Unknown0 + " ");
                Console.Write(ama.Group2[i].Unknown1 + " ");
                Console.Write(ama.Group2[i].Unknown2 + " ");
                Console.Write(ama.Group2[i].Unknown3 + " ");
                Console.Write(ama.Group2[i].Unknown4 + " ");
                Console.Write(ama.Group2[i].Unknown5 + " ");
                Console.Write(ama.Group2[i].Unknown6 + " ");
                Console.Write(ama.Group2[i].Unknown7 + "\n\n");
            }

            if (ama.Strange)
            {
                Console.WriteLine("This AMA file is strange");
                Console.WriteLine("Strange values at:");
                foreach (int i in ama.StrangeList)
                    Console.Write("0x" + i.ToString("X") + " ");
                Console.WriteLine();
            }

            if (args.Length == 0)
                Console.Read();
        }
    }
}
