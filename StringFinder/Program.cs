using System;
using System.IO;
using System.Linq;

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

            foreach (var dir in dirs)
            {
                var di = Directory.GetParent($@"{dir}\");
                var files = di.GetFiles();

                foreach (var file in files)
                {
                    try
                    {
                        string fileString = File.ReadAllText(file.FullName);

                        if (fileString.Contains(toSearch))
                            Directory.Move(dir, dir + "_НАДО");
                    }
                    catch (Exception) {  }                    
                }
            }
        }
    }
}
