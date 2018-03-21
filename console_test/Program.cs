using System;

namespace console_test
{
    class Program
    {
        private static int _menuIndex;
        private static Array Menu {get; set;} = Enum.GetValues(typeof(ConsoleColor));
        private static ConsoleColor CurrentColor {get; set;} = Console.BackgroundColor;
        private static int MenuWidth {get; set;} = 12;

        public static int menuIndex 
        {
            get {return _menuIndex; }
            set 
            {
                if (value < 0)
                    _menuIndex = 0;

                else if (value > Menu.Length - 1)
                    _menuIndex = Menu.Length - 1;
                
                else
                    _menuIndex = value;
            }
        }

        static void Main(string[] args)
        { 
            while (true)
            {
                DrawMenu(menuIndex);

                Console.SetCursorPosition(0,menuIndex);

                var key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        menuIndex--;
                        Console.SetCursorPosition(0,menuIndex);
                        break;

                    case ConsoleKey.DownArrow:
                        menuIndex++;
                        Console.SetCursorPosition(0,menuIndex);
                        break;

                    case ConsoleKey.Enter:
                        MakeChoice(menuIndex);
                        break;
                }
        }   }

        private static void MakeChoice(int menuIndex)
        {
            Console.BackgroundColor = CurrentColor = (ConsoleColor)Menu.GetValue(menuIndex);
            Console.Clear();
        }

        private static void DrawMenu(int menuIndex)
        {
            Console.SetCursorPosition(0,0);

            for (int i = 0; i < Menu.Length; i++)
            {
                string name = ((ConsoleColor)Menu.GetValue(i))
                                .ToString().PadRight(MenuWidth, ' ');

                if (menuIndex == i)
                {                    
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.WriteLine(name);
                    Console.BackgroundColor = CurrentColor;
                }    
                else
                {
                    Console.WriteLine(name); 
                }       
            }
        }
    }
}
