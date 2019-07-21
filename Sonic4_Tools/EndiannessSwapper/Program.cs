using System;
using System.IO;

namespace EndiannessSwapper
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: EndiannessSwapper [file]");
            }
            else if (File.Exists(args[0]))
            {
                byte[] raw_file = File.ReadAllBytes(args[0]);

                for (int i = 0; i < raw_file.Length/4; i++)
                {
                    Array.Reverse(raw_file, i*4, 4);
                }

                File.WriteAllBytes(args[0], raw_file);
            }
        }
    }
}
