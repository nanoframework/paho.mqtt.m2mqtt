using System;
using System.Diagnostics;

namespace MessageUnitTests
{
    internal static class Helpers
    {
        public static void DumpBuffer(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                Debug.Write($"{buffer[i]},");
            }

            Debug.WriteLine("");
        }
    }
}
