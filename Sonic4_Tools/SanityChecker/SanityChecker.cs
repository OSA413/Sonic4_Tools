using System;
using System.IO;
using System.Collections.Generic;

namespace SanityChecker
{
    public class SanityCheckable
    {
        public List<int> WrongValues {get; private set;}

        public SanityCheckable()
        {
            WrongValues = new List<int>();
        }

        public virtual byte[] Write()
        {
            return new byte[0];
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
    }
}
