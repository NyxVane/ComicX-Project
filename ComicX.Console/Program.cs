using ComicX.Auth;
using ComicX.Menu;
using System;

namespace ComicX.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AuthManager.InitializeUsersFile();

            if (AuthManager.IsLockedOut(out TimeSpan remainingTime))
            {
                Console.WriteLine("Too many failed login attempts.");
                Console.WriteLine($"System is locked. Please try again in {Math.Ceiling(remainingTime.TotalMinutes)} minutes.");
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                return;
            }

            int attempts = 3;
            bool isLoggedIn = false;

            while (attempts > 0)
            {
                Console.Write("Please provide username to access the ComicX system: ");
                string username = Console.ReadLine();
                Console.Write("Please provide password: ");
                string password = Console.ReadLine();

                if (AuthManager.ValidateCredentials(username, password))
                {
                    isLoggedIn = true;
                    break;
                }
                else
                {
                    attempts--;
                    Console.WriteLine($"Wrong username and/or password. Remaining attempts: {attempts}\n");
                }
            }

            if (!isLoggedIn)
            {
                AuthManager.SetLockout();

                Console.WriteLine("\nYou are not authorized to access this service.");
                Console.WriteLine("Account locked for 30 minutes due to 3 failed attempts.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nLogin successful! Press any key to open the menu...");

            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("****** Here are your options ******");
                Console.WriteLine("Please select the action.");
                Console.WriteLine("1. Show stock count for each theme of books");
                Console.WriteLine("2. Show total value of each theme type for all comic books in stock");
                Console.WriteLine("3. Register one comic book sold for a given theme");
                Console.WriteLine("4. Get stock status");
                Console.WriteLine("5. Exit");

                Console.Write("\nPlease enter the number of your choice (1-5): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowStockUI();
                        break;
                    case "2":
                        ShowTotalValuesUI();
                        break;
                    case "3":
                        RegisterSaleUI();
                        break;
                    case "4":
                        ShowStockStatusUI();
                        break;
                    case "5":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice. Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void ShowStockUI()
        {
            Console.Clear();
            Console.WriteLine("--- Stock Count for Each Theme ---");
            var stock = MenuManager.GetStock();
            foreach (var item in stock)
            {
                Console.WriteLine($"Theme: {item.Key} - Quantity: {item.Value} pcs");
            }
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private static void ShowTotalValuesUI()
        {
            Console.Clear();
            Console.WriteLine("--- Total Value of Each Theme Type ---");
            var values = MenuManager.GetTotalValues();
            foreach (var item in values)
            {
                Console.WriteLine($"Theme: {item.Key} - Total Value: {item.Value} USD");
            }
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private static void RegisterSaleUI()
        {
            Console.Clear();
            Console.WriteLine("--- Register a Comic Book Sold ---");
            Console.WriteLine("Available themes: Batman, Joker, Cat, TheRiddler, Pinguin");
            Console.Write("Enter the theme name to sell one: ");
            string theme = Console.ReadLine();

            if (MenuManager.OneBookSold(theme))
            {
                Console.WriteLine($"\nSale registered seccessfully for {theme}!");
            } else
            {
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
            }
        }

        private static void ShowStockStatusUI()
        {
            Console.Clear();
            Console.WriteLine("--- Stock Status ---");
            string status = MenuManager.GetStockStatus();
            Console.WriteLine($"Current overall stock status: {status}");
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }
    }
}
