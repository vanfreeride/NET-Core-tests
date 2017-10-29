using System;
using RcOnline.Models;

namespace RcOnline
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("*** Стартер интеграций БО с ГИС ЖКХ *** \n");

            var app = new App();
            app.Start();
        }
    }
}
