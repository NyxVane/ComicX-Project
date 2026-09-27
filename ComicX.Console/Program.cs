using ComicX.Auth;
using ComicX.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicX.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool isLogedIn = AuthManager.Login();

            if (!isLogedIn)
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n****** Here are your options ******");
            Console.WriteLine("Please select the action.");
            Console.WriteLine("1. Show stock count for each theme of books");
            Console.WriteLine("2. Show total value of each theme type for all comic books in stock");
            Console.WriteLine("3. Register one comic book sold for a given theme");
            Console.WriteLine("4. Get stock status");

            Console.Write("\nPlease enter the number of your choice (1-4): ");
            string choice = Console.ReadLine();

            Console.WriteLine($"\nYou selected option: {choice}");
            //Itt kell majd behívnod az általad megírt menükezelőt!

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
