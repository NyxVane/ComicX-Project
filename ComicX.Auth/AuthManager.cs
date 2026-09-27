using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ComicX.Auth
{
    public class AuthManager
    {
        public static bool Login()
        {
            string filePath = "users.txt";

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "admin,1234\n");
            }

            int attempts = 3;

            while (attempts > 0)
            {
                Console.Write("Please provide username to access the ComicX system: ");
                string username = Console.ReadLine();
                Console.Write("Please provide password: ");
                string password = Console.ReadLine();

                string[] lines = File.ReadAllLines(filePath);
                bool isAuthenticated = false;

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');

                    if (parts.Length>= 2 && parts[0].Trim() == username && parts[1].Trim() == password)
                    {
                        isAuthenticated = true;
                        break;
                    }
                }

                if (isAuthenticated)
                {
                    return true;
                }
                else
                {
                    attempts--;
                    if (attempts > 0)
                    {
                        Console.WriteLine($"Wrong username and/or password. Remaining attempts: {attempts}\n");
                    }
                }
            }
            Console.WriteLine("You are not authorized to access this service");
            return false;
        }
    }
}
