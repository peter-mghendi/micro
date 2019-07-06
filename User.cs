using System;
using CG.Web.MegaApiClient;

namespace micro
{
    class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public MegaApiClient Client { get; set; }
        public User(string Username, string Password, MegaApiClient Client)
        {
            this.Username = Username;
            this.Password = Password;
            this.Client = Client;
        }

        public static User ReadUser()
        {
            string username, password;
			Console.Write("Enter your username: ");
			username = Console.ReadLine();
			Console.Write("Enter your password: ");
            password = ReadPassword();
            return new User(username, password, new MegaApiClient());
        }

        static string ReadPassword(){ 
            string pass = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if(key.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        break;
                    }
                }
            } while (true);
            return pass;
        }
    }
}