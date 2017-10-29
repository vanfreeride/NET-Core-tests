using System;

namespace RcOnline.Models
{
    public static class Logger
    {
        public static void WriteLineSuccess(string mess)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(mess);
            Console.ResetColor();
        }

        public static void WriteLineError(string mess)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(mess);
            Console.ResetColor();
        }
    }
}