using System;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (File.Exists(args[0]))
            {
                File.WriteAllBytes(args[0], File.ReadAllBytes(args[0]).Join(new byte[] { 0x80, 0x01, 0x00, 0x0e, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
                Console.WriteLine("OK");
            }
            else { Console.WriteLine("File not found"); }
        }
    }

    public static class Extensions
    {
        //This thing joins two byte arrays just like Concat().ToArray() but faster (i think)
        public static byte[] Join(this byte[] first, byte[] second)
        {
            byte[] output = new byte[first.Length + second.Length];

            Array.Copy(first, 0, output, 0, first.Length);
            Array.Copy(second, 0, output, first.Length, second.Length);

            return output;
        }
    }
}
