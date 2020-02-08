using System;
using System.Collections.Generic;
using System.IO;

namespace AMAEditor
{
    public class AMA
    {
        public List<Group1> Group1;
        public List<Group2> Group2;
        public bool Strange;

        public static bool IsAMA(byte[] fileRaw)
        {
            return fileRaw.Length >= 0x20
                && fileRaw[0] == '#'
                && fileRaw[1] == 'A'
                && fileRaw[2] == 'M'
                && fileRaw[3] == 'A';
        }

        public void Read(byte[] fileRaw)
        {
            Group1 = new List<Group1>();
            Group2 = new List<Group2>();

            if (!AMA.IsAMA(fileRaw))
                return;

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
                g2Dict[ptr].PositionX = BitConverter.ToSingle(fileRaw, ptr + 0x10);
                g2Dict[ptr].PositionY = BitConverter.ToSingle(fileRaw, ptr + 0x14);
                g2Dict[ptr].SizeX = BitConverter.ToSingle(fileRaw, ptr + 0x18);
                g2Dict[ptr].SizeY = BitConverter.ToSingle(fileRaw, ptr + 0x1C);

                int ptr0 = BitConverter.ToInt32(fileRaw, ptr + 0x24);
                int ptr1 = BitConverter.ToInt32(fileRaw, ptr0 + 0xC);
                int ptr2 = BitConverter.ToInt32(fileRaw, ptr0 + 0x10);

                g2Dict[ptr].UVLeftEdge = BitConverter.ToSingle(fileRaw, ptr2 + 0x10);
                g2Dict[ptr].UVUpperEdge = BitConverter.ToSingle(fileRaw, ptr2 + 0x14);
                g2Dict[ptr].UVRightEdge = BitConverter.ToSingle(fileRaw, ptr2 + 0x18);
                g2Dict[ptr].UVBottomEdge = BitConverter.ToSingle(fileRaw, ptr2 + 0x1C);
            }

            foreach (int ptr in g1Dict.Keys)
            {
                int ptrG1Child = BitConverter.ToInt32(fileRaw, ptr + 0x8);
                if (ptrG1Child != 0)
                    g1Dict[ptr].G1Child = g1Dict[ptrG1Child];

                int ptrG2Child = BitConverter.ToInt32(fileRaw, ptr + 0xC);
                if (ptrG2Child != 0)
                    g1Dict[ptr].G2Children.Add(g2Dict[ptrG2Child]);

                int ptrParent = BitConverter.ToInt32(fileRaw, ptr + 0x10);
                if (ptrParent != 0)
                    g1Dict[ptr].Parent = g1Dict[ptrParent];

                ptrG2Child = BitConverter.ToInt32(fileRaw, ptr + 0x14);
                if (ptrG2Child != 0)
                    g1Dict[ptr].G2Children.Add(g2Dict[ptrG2Child]);
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
        public Group1       G1Child;
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
        public int Unknown7;
        public float Unknown8;
    }

    class MainClass
    {
        public static void Main(string[] args)
        {

        }
    }
}
