using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace U2ADsRemover
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), "STREAML4RA.BUN");
            List<byte> searchBytes = new List<byte>();
            hexToByte(searchBytes, args[0]);
            byte[] bytesFile = File.ReadAllBytes(path);
            int fileSize = bytesFile.Length;
            int searchSize = searchBytes.Count;
            bool find = true;
            for (int i = 0; i + searchSize < fileSize; i++)
            {
                find = true;
                for (int j = 0; j < searchSize; j++)
                {
                    if (bytesFile[i + j] != searchBytes[j])
                    {
                        find = false;
                        break;
                    }
                }
                if (find && (bytesFile[i - 4] == 40 || bytesFile[i - 4] == 56) && bytesFile[i + bytesFile[i - 4]] == 0 && bytesFile[i + bytesFile[i - 4] + 1] == 65 && bytesFile[i + bytesFile[i - 4] + 2] == 19 && bytesFile[i + bytesFile[i - 4] + 3] == 128)
                {
                    for (int j = 0; j < bytesFile[i - 4]; j++)
                    {
                        bytesFile[i + j] = 0;
                    }
                    File.WriteAllBytes(path, bytesFile);
                    Console.WriteLine("Операция замены выполнена.");
                }
            }
            bytesFile = null;
        }

        static void hexToByte(List<byte> list, string line)
        {
            int count = line.Length;
            for (int i = 0; i + 1 < count; i += 2)
            {
                list.Add(Convert.ToByte(line.Substring(i, 2), 16));
            }
        }
    }
}
