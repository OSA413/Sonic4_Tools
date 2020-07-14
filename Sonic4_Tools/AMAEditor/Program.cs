using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Windows.Forms;

using SanityChecker;

namespace AMAEditor
{
    public class AMA: SanityCheckable
    {
        //https://github.com/OSA413/Sonic4_Tools/blob/master/docs/Specifications/AMA.md
        public List<Group1> Group1;
        public List<Group2> Group2;
        public bool         Strange;
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
                g2Dict[ptr].Unknown12 = BitConverter.ToInt32(fileRaw, ptr);
                StrangeIsntIt(fileRaw, ptr + 0x04, Group2.IndexOf(g2Dict[ptr]));
                g2Dict[ptr].Unknown13 = BitConverter.ToInt32(fileRaw, ptr + 0x08);
                StrangeIsntIt(fileRaw, ptr + 0x0C, 0x00);

                g2Dict[ptr].PositionX = BitConverter.ToSingle(fileRaw, ptr + 0x10);
                g2Dict[ptr].PositionY = BitConverter.ToSingle(fileRaw, ptr + 0x14);
                g2Dict[ptr].SizeX = BitConverter.ToSingle(fileRaw, ptr + 0x18);
                g2Dict[ptr].SizeY = BitConverter.ToSingle(fileRaw, ptr + 0x1C);

                g2Dict[ptr].Unknown8 = BitConverter.ToInt32(fileRaw, ptr + 0x20);
                g2Dict[ptr].Unknown15 = BitConverter.ToInt32(fileRaw, ptr + 0x28);
                g2Dict[ptr].Unknown16 = BitConverter.ToInt32(fileRaw, ptr + 0x2C);
                g2Dict[ptr].Unknown18 = BitConverter.ToInt32(fileRaw, ptr + 0x30);
                g2Dict[ptr].Unknown19 = BitConverter.ToInt32(fileRaw, ptr + 0x34);
                for (int i = 0x38; i < 0x40; i = i + 4)
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

                g2Dict[ptr].Unknown8 = BitConverter.ToInt32(fileRaw, ptr + 0x54);
                g2Dict[ptr].Unknown9 = BitConverter.ToInt32(fileRaw, ptr + 0x58);
                g2Dict[ptr].Unknown10 = BitConverter.ToInt32(fileRaw, ptr + 0x5C);
                g2Dict[ptr].Unknown11 = BitConverter.ToInt32(fileRaw, ptr + 0x60);
                g2Dict[ptr].Unknown14 = BitConverter.ToInt32(fileRaw, ptr + 0x64);

                for (int i = 0x68; i < 0x68; i = i + 4)
                    StrangeIsntIt(fileRaw, ptr + i, 0x00);
                g2Dict[ptr].Unknown20 = BitConverter.ToInt32(fileRaw, ptr + 0x68);
                g2Dict[ptr].Unknown21 = BitConverter.ToInt32(fileRaw, ptr + 0x6C);
                g2Dict[ptr].Unknown17 = BitConverter.ToInt32(fileRaw, ptr + 0x74);

                g2Dict[ptr].UVLeftEdge = BitConverter.ToSingle(fileRaw, ptr2 + 0x10);
                g2Dict[ptr].UVUpperEdge = BitConverter.ToSingle(fileRaw, ptr2 + 0x14);
                g2Dict[ptr].UVRightEdge = BitConverter.ToSingle(fileRaw, ptr2 + 0x18);
                g2Dict[ptr].UVBottomEdge = BitConverter.ToSingle(fileRaw, ptr2 + 0x1C);
            }

            foreach (int ptr in g1Dict.Keys)
            {
                StrangeIsntIt(fileRaw, ptr, 0x00);
                StrangeIsntIt(fileRaw, ptr + 4, Group1.IndexOf(g1Dict[ptr]));

                int ptrG1Child = BitConverter.ToInt32(fileRaw, ptr + 0x8);
                if (ptrG1Child != 0)
                    g1Dict[ptr].G1Child0 = g1Dict[ptrG1Child];

                ptrG1Child = BitConverter.ToInt32(fileRaw, ptr + 0xC);
                if (ptrG1Child != 0)
                    g1Dict[ptr].G1Child1 = g1Dict[ptrG1Child];

                int ptrParent = BitConverter.ToInt32(fileRaw, ptr + 0x10);
                if (ptrParent != 0)
                    g1Dict[ptr].Parent = g1Dict[ptrParent];

                int ptrG2Child = BitConverter.ToInt32(fileRaw, ptr + 0x14);
                if (ptrG2Child != 0)
                    g1Dict[ptr].G2Child0 = g2Dict[ptrG2Child];

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

        public override byte[] Write()
        {
            int fileLength = 0;

            int headerLength = 0x20;
            int g1objLength = 8 * 4;
            int g2objLength = 38 * 4;

            int g1objNamesLength = 0;
            foreach (var obj in Group1)
                g1objNamesLength += obj.Name.Length + 1;
            int g2objNamesLength = 0;
            foreach (var obj in Group2)
                g2objNamesLength += obj.Name.Length + 1;

            fileLength += headerLength;
            fileLength += Group1.Count * (4 * 2 + g1objLength);
            fileLength += Group2.Count * (4 * 2 + g2objLength);

            fileLength += g1objNamesLength;
            fileLength += g2objNamesLength;

            var fileRaw = new byte[fileLength];

            ////Header////
            Array.Copy(Encoding.ASCII.GetBytes("#AMA"), 0, fileRaw, 0, 4);

            //Object number
            Array.Copy(BitConverter.GetBytes(Group1.Count), 0, fileRaw, 0x08, 4);
            Array.Copy(BitConverter.GetBytes(Group2.Count), 0, fileRaw, 0x0C, 4);

            //Object list pointers
            int g1listPointer = 0;
            if (Group1.Count != 0)
                g1listPointer = headerLength;
            Array.Copy(BitConverter.GetBytes(g1listPointer), 0, fileRaw, 0x10, 4);

            int g2listPointer = 0;
            if (Group2.Count != 0)
                g2listPointer = headerLength + Group1.Count * (g1objLength + 4);
            Array.Copy(BitConverter.GetBytes(g2listPointer), 0, fileRaw, 0x14, 4);

            //Object name list pointers
            int g1nameListPointer = 0;
            if (Group1.Count != 0)
                g1nameListPointer = g2listPointer + (4 + g2objLength) * Group2.Count;
            Array.Copy(BitConverter.GetBytes(g1nameListPointer), 0, fileRaw, 0x18, 4);

            int g2nameListPointer = 0;
            if (Group2.Count != 0)
            {
                if (g1nameListPointer != 0)
                    g2nameListPointer = g1nameListPointer + 4 * Group1.Count + g1objNamesLength;
                else
                    g2nameListPointer = g2listPointer + (4 + g2objLength) * Group2.Count;
            }
            Array.Copy(BitConverter.GetBytes(g2nameListPointer), 0, fileRaw, 0x1C, 4);

            //Group 1
            int curNamePtr = g1nameListPointer + Group1.Count * 4;
            for (int i = 0; i < Group1.Count; i++)
            {
                var obj = Group1[i];
                int objPtr = g1listPointer + 4 * Group1.Count + g1objLength * i;

                //Object list pointer
                Array.Copy(BitConverter.GetBytes(objPtr), 0, fileRaw, g1listPointer + 4 * i, 4);

                //Body
                //0x00
                Array.Copy(BitConverter.GetBytes(i), 0, fileRaw, objPtr + 0x04, 4);

                //Group 1 children objects
                int G1Child0 = -1;
                int G1Child1 = -1;
                int G1Parent = -1;

                for (int objNum = 0; objNum < Group1.Count; objNum++)
                {
                    if (obj.G1Child0 != null && Group1[objNum].Name == obj.G1Child0.Name)
                        G1Child0 = objNum;
                    if (obj.G1Child1 != null && Group1[objNum].Name == obj.G1Child1.Name)
                        G1Child1 = objNum;
                    if (obj.Parent != null && Group1[objNum].Name == obj.Parent.Name)
                        G1Parent = objNum;

                    if ((G1Child0 >= 0 || obj.G1Child0 == null)
                    && (G1Child1 >= 0 || obj.G1Child1 == null)
                    && (G1Parent >= 0 || obj.Parent == null))
                        break;
                }

                if (G1Child0 >= 0)
                    Array.Copy(BitConverter.GetBytes(g1listPointer + 4 * Group1.Count + g1objLength * G1Child0), 0, fileRaw, objPtr + 0x8, 4);
                if (G1Child1 >= 0)
                    Array.Copy(BitConverter.GetBytes(g1listPointer + 4 * Group1.Count + g1objLength * G1Child1), 0, fileRaw, objPtr + 0xC, 4);
                if (G1Parent >= 0)
                    Array.Copy(BitConverter.GetBytes(g1listPointer + 4 * Group1.Count + g1objLength * G1Parent), 0, fileRaw, objPtr + 0x10, 4);

                //Group 2 children objects
                int G2Child0 = -1;
                for (int objNum = 0; objNum < Group2.Count; objNum++)
                {
                    if (obj.G2Child0 != null && Group2[objNum].Name == obj.G2Child0.Name)
                        G2Child0 = objNum;

                    if (G2Child0 >= 0 || obj.G2Child0 == null)
                        break;
                }

                if (G2Child0 >= 0)
                    Array.Copy(BitConverter.GetBytes(g2listPointer + 4 * Group2.Count + g2objLength * G2Child0), 0, fileRaw, objPtr + 0x14, 4);

                //0x18
                //0x1C

                //Names
                Array.Copy(BitConverter.GetBytes(curNamePtr), 0, fileRaw, g1nameListPointer + 4 * i, 4);

                Array.Copy(Encoding.ASCII.GetBytes(obj.Name), 0, fileRaw, curNamePtr, obj.Name.Length);
                curNamePtr += obj.Name.Length + 1;
            }


            //Group 2
            curNamePtr = g2nameListPointer + Group2.Count * 4;
            for (int i = 0; i < Group2.Count; i++)
            {
                var obj = Group2[i];
                int objPtr = g2listPointer + 4 * Group2.Count + g2objLength * i;

                //Object list pointer
                Array.Copy(BitConverter.GetBytes(objPtr), 0, fileRaw, g2listPointer + 4 * i, 4);

                //Body
                Array.Copy(BitConverter.GetBytes(obj.Unknown12), 0, fileRaw, objPtr, 4);
                Array.Copy(BitConverter.GetBytes(i), 0, fileRaw, objPtr + 0x04, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown13), 0, fileRaw, objPtr + 0x08, 4);
                //0x0C
                Array.Copy(BitConverter.GetBytes(obj.PositionX), 0, fileRaw, objPtr + 0x10, 4);
                Array.Copy(BitConverter.GetBytes(obj.PositionY), 0, fileRaw, objPtr + 0x14, 4);
                Array.Copy(BitConverter.GetBytes(obj.SizeX), 0, fileRaw, objPtr + 0x18, 4);
                Array.Copy(BitConverter.GetBytes(obj.SizeY), 0, fileRaw, objPtr + 0x1C, 4);
                //0x20, ptr3 in the specification
                Array.Copy(BitConverter.GetBytes(objPtr + 0x40), 0, fileRaw, objPtr + 0x24, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown15), 0, fileRaw, objPtr + 0x28, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown16), 0, fileRaw, objPtr + 0x2C, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown18), 0, fileRaw, objPtr + 0x30, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown19), 0, fileRaw, objPtr + 0x34, 4);
                //Lots of 0x00
                Array.Copy(BitConverter.GetBytes(obj.Unknown0), 0, fileRaw, objPtr + 0x40, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown1), 0, fileRaw, objPtr + 0x44, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown2), 0, fileRaw, objPtr + 0x48, 4);
                Array.Copy(BitConverter.GetBytes(objPtr + 0x70), 0, fileRaw, objPtr + 0x4C, 4);
                Array.Copy(BitConverter.GetBytes(objPtr + 0x78), 0, fileRaw, objPtr + 0x50, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown8), 0, fileRaw, objPtr + 0x54, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown9), 0, fileRaw, objPtr + 0x58, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown10), 0, fileRaw, objPtr + 0x5C, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown11), 0, fileRaw, objPtr + 0x60, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown14), 0, fileRaw, objPtr + 0x64, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown20), 0, fileRaw, objPtr + 0x68, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown21), 0, fileRaw, objPtr + 0x6C, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown3), 0, fileRaw, objPtr + 0x70, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown17), 0, fileRaw, objPtr + 0x74, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown4), 0, fileRaw, objPtr + 0x78, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown5), 0, fileRaw, objPtr + 0x7C, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown6), 0, fileRaw, objPtr + 0x80, 4);
                Array.Copy(BitConverter.GetBytes(obj.Unknown7), 0, fileRaw, objPtr + 0x84, 4);
                Array.Copy(BitConverter.GetBytes(obj.UVLeftEdge), 0, fileRaw, objPtr + 0x88, 4);
                Array.Copy(BitConverter.GetBytes(obj.UVUpperEdge), 0, fileRaw, objPtr + 0x8C, 4);
                Array.Copy(BitConverter.GetBytes(obj.UVRightEdge), 0, fileRaw, objPtr + 0x90, 4);
                Array.Copy(BitConverter.GetBytes(obj.UVBottomEdge), 0, fileRaw, objPtr + 0x94, 4);

                //Names
                Array.Copy(BitConverter.GetBytes(curNamePtr), 0, fileRaw, g2nameListPointer + 4 * i, 4);

                Array.Copy(Encoding.ASCII.GetBytes(obj.Name), 0, fileRaw, curNamePtr, obj.Name.Length);
                curNamePtr += obj.Name.Length + 1;
            }

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

            info += "Group 1 objects:\n";
            info += "Number of objects: " + Group1.Count + "\n\n";
            for (int i = 0; i < Group1.Count; i++)
            {
                info += "Object #" + i + "\n";
                info += "Name: " + Group1[i].Name + "\n";
                if (Group1[i].Parent != null)
                    info += "Parent name: " + Group1[i].Parent.Name + "\n";
                if (Group1[i].G1Child0 != null)
                    info += "\tGroup 1 Child #0's name: " + Group1[i].G1Child0.Name + "\n";
                if (Group1[i].G1Child1 != null)
                    info += "\tGroup 1 Child #1's name: " + Group1[i].G1Child1.Name + "\n";
                if (Group1[i].G2Child0 != null)
                    info += "\tGroup 2 Child #0's name: " + Group1[i].G2Child0.Name + "\n";
                info += "\n";
            }

            info += "Group 2 objects:\n";
            info += "Number of objects: " + Group2.Count + "\n";
            for (int i = 0; i < Group2.Count; i++)
            {
                info += "Object #" + i + "\n";
                info += "Name: " + Group2[i].Name + "\n";

                info += "PositionX: " + Group2[i].PositionX + "\n";
                info += "PositionY: " + Group2[i].PositionY + "\n";
                info += "SizeX: " + Group2[i].SizeX + "\n";
                info += "SizeY: " + Group2[i].SizeY + "\n";

                info += "UVLeftEdge: " + Group2[i].UVLeftEdge + "\n";
                info += "UVUpperEdge: " + Group2[i].UVUpperEdge + "\n";
                info += "UVRightEdge: " + Group2[i].UVRightEdge + "\n";
                info += "UVBottomEdge: " + Group2[i].UVBottomEdge + "\n";

                info += "Unknown values:" + "\n";
                info += Group2[i].Unknown0 + " ";
                info += Group2[i].Unknown1 + " ";
                info += Group2[i].Unknown2 + " ";
                info += Group2[i].Unknown3 + " ";
                info += Group2[i].Unknown4 + " ";
                info += Group2[i].Unknown5 + " ";
                info += Group2[i].Unknown6 + " ";
                info += Group2[i].Unknown7 + " ";
                info += Group2[i].Unknown8 + " ";
                info += Group2[i].Unknown9 + " ";
                info += Group2[i].Unknown10 + " ";
                info += Group2[i].Unknown11 + " ";
                info += Group2[i].Unknown12 + " ";
                info += Group2[i].Unknown13 + " ";
                info += Group2[i].Unknown14 + " ";
                info += Group2[i].Unknown15 + " ";
                info += Group2[i].Unknown16 + " ";
                info += Group2[i].Unknown17 + " ";
                info += Group2[i].Unknown18 + " ";
                info += Group2[i].Unknown19 + " ";
                info += Group2[i].Unknown20 + " ";
                info += Group2[i].Unknown21 + "\n\n";
            }
            return info;
        }
    }

    public class Group1
    {
        public string       Name;
        public Group1       Parent;

        //Children
        public Group1 G1Child0;
        public Group1 G1Child1;
        public Group2 G2Child0;
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
        public int Unknown8;
        public int Unknown9;
        public int Unknown10;
        public int Unknown11;
        public int Unknown12;
        public int Unknown13;
        public int Unknown14;
        public int Unknown15;
        public int Unknown16;
        public int Unknown17;
        public int Unknown18;
        public int Unknown19;
        public int Unknown20;
        public int Unknown21;
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

                var ama = new AMA();

                string file;
                if (args.Length > 0)
                    file = args[1 + (sanityOnly ? 1 : 0)];
                else
                    file = Console.ReadLine();

                ama.Read(file);
                ama.SanityCheck(file);

                if (sanityOnly)
                {
                    if (ama.WrongValues.Count == 0)
                        Console.WriteLine("OK");
                    else
                    {
                        foreach (int i in ama.WrongValues)
                            Console.Write("0x" + i.ToString("X") + " ");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine(ama.InfoToString());

                    if (ama.Strange)
                    {
                        Console.WriteLine("This AMA file is strange");
                        Console.WriteLine("Strange values at:");
                        foreach (int i in ama.StrangeList)
                            Console.Write("0x" + i.ToString("X") + " ");
                        Console.WriteLine();
                    }

                    Console.Write("Sanity check ");
                    if (ama.WrongValues.Count == 0)
                        Console.WriteLine("passed");
                    else
                    {
                        Console.WriteLine("failed (" + ama.WrongValues.Count + ")");
                        foreach (int i in ama.WrongValues)
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
                Application.Run(new MainForm(args));
            }
        }
    }
}
