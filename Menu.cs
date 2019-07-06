using System;

namespace micro
{
    class Menu
    {
        public const int MENU_MAIN = 0;

        public static void ShowMenu(int context){
            Console.WriteLine("What would you like to do?\n1: List Everything\n0: Exit");
        }

        public static void Select(int context, int selection){
            switch (context)
            {
                case MENU_MAIN:
                    SelectMain(selection);
                    break;
                default:
                    Console.WriteLine("That option does not exist, please try again.");
                    Program.MainMenu();
                    break;
            }
        }

        static void SelectMain(int selection){
            switch (selection)
            {
                case 0:
                    Console.WriteLine("You will be logged out.");
                    break;
                
                case 1:
                    Data.ListAll(Program.User.Client);
                    Program.MainMenu();
                    break;
                
                default:
                    Console.WriteLine("That option does not exist, please try again.");
                    Program.MainMenu();
                    break;
            }   
        }
    }
}