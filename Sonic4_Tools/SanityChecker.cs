using System;
using System.IO;
using System.Collections.Generic;

namespace SanityChecker
{
    public class SanityCheckable
    {
        public bool Strange;
        public List<int> StrangeList { get; private set; }
        public List<int> WrongValues { get; private set; }

        public SanityCheckable()
        {
            StrangeList = new List<int>();
            WrongValues = new List<int>();
        }

        public void SanityCheck(byte[] original, byte[] recreation)
        {
            WrongValues = new List<int>();

            if (original.Length == recreation.Length)
            {
                for (int i = 0; i < original.Length; i++)
                    if (original[i] != recreation[i])
                        WrongValues.Add(i);
            }
            else
            {
                WrongValues.Add(-1);
                WrongValues.Add(original.Length);
                WrongValues.Add(recreation.Length);
            }
        }

        public void SanityCheck(string originalFile)
        {
            SanityCheck(File.ReadAllBytes(originalFile), Write());
        }

        public static bool PrintChecksIfNecessary<T>(string[] args)
            where T : SanityCheckable, new()
        {
            if (args.Length > 0 && args[0] == "--check")
            {
                var sanityOnly = false;
                var strangeOnly = false;
                if (args[1] == "--sanity-only")
                    sanityOnly = true;
                if (args[1] == "--strange-only")
                    strangeOnly = true;

                var fileFormat = new T();

                string file;
                if (args.Length > 0)
                    file = args[1 + (sanityOnly || strangeOnly ? 1 : 0)];
                else
                    file = Console.ReadLine();

                fileFormat.Read(file);
                fileFormat.SanityCheck(file);


                if (sanityOnly || strangeOnly)
                {
                    var checkCounter = sanityOnly ? fileFormat.WrongValues.Count :
                                        fileFormat.Strange ? 1 : 0;

                    var checkList = sanityOnly ? fileFormat.WrongValues : fileFormat.StrangeList;

                    if (checkCounter == 0)
                        Console.WriteLine("OK");
                    else
                    {
                        foreach (int i in fileFormat.WrongValues)
                            Console.Write("0x" + i.ToString("X") + " ");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine(fileFormat.InfoToString());

                    if (fileFormat.Strange)
                    {
                        Console.WriteLine("This file is strange");
                        Console.WriteLine("Strange values at:");
                        foreach (int i in fileFormat.StrangeList)
                            Console.Write("0x" + i.ToString("X") + " ");
                        Console.WriteLine();
                    }

                    Console.Write("Sanity check ");
                    if (fileFormat.WrongValues.Count == 0)
                        Console.WriteLine("passed");
                    else
                    {
                        Console.WriteLine("failed (" + fileFormat.WrongValues.Count + ")");
                        foreach (int i in fileFormat.WrongValues)
                            Console.Write("0x" + i.ToString("X") + " ");
                        Console.WriteLine();
                    }
                }
                return true;
            }
            return false;
        }

        //Strange value checker
        protected void StrangeIsntIt(byte[] fileRaw, int ptr, int intendedValue) =>
            StrangeIsntIt(BitConverter.ToInt32(fileRaw, ptr), ptr, intendedValue);

        protected void StrangeIsntIt(int actualValue, int ptr, int intendedValue)
        {
            if (actualValue != intendedValue)
            {
                Strange = true;
                StrangeList.Add(ptr);
            }
        }

        //Actual methods placeholders
        public virtual byte[] Write() => new byte[0];
        public virtual void Write(string fileName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            File.WriteAllBytes(fileName, this.Write());
        }
        public virtual void Read(byte[] fileRaw)
        {
            Strange = false;
            StrangeList.Clear();
            WrongValues.Clear();
        }
        public virtual void Read(string fileName) => Read(File.ReadAllBytes(fileName));
        public virtual string InfoToString() => "";
    }
}
