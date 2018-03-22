using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Шо искать? ");
            string toSearch = Console.ReadLine();

            var rootDir = AppDomain.CurrentDomain.BaseDirectory;
            var dirs = Directory.GetDirectories(rootDir);

            int dirNum = 0;

            foreach (var dir in dirs)
            {
                dirNum++;

                var di = Directory.GetParent($@"{dir}\");
                var files = di.GetFiles();

                bool match = false;

                foreach (var file in files)
                {
                    string fileString = File.ReadAllText(file.FullName);

                    if (fileString.Contains(toSearch))
                    {
                        match = true;
                        break;
                    }

                }

                if (match)
                    foreach (var file in files)
                    {
                        System.IO.Directory.CreateDirectory($"c:\\temp\\sf\\{dirNum}\\");
                        file.CopyTo($"c:\\temp\\sf\\{dirNum}\\{file.Name}");
                    }
            }
        }
    }
}
