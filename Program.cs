using System;
using CG.Web.MegaApiClient;

namespace micro
{
    class Program
    {
        public static User User{ get; set; }
        static void Main()
        {
            Console.WriteLine("Welcome to micro v1.0\nDISCLAIMER: micro is in no way associated with MEGA.\n");
            try{
                User = Login();
            } catch (ApiException){
                Console.WriteLine("Login Failed! Invalid Credentials.");
                Environment.Exit(0);
            } 
            Console.WriteLine("Login Successful!.");
            MainMenu();         
            User.Client.Logout();
        }

        public static void MainMenu(){
            Menu.ShowMenu(Menu.MENU_MAIN);
            Menu.Select(Menu.MENU_MAIN, Convert.ToInt32(Console.ReadLine()));
        }

        static User Login(){
            Console.WriteLine("Please Log In:");
            var user = User.ReadUser();
            user.Client.Login(user.Username, user.Password);
            return user;
        }    
    }
}
