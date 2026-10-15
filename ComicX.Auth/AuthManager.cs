using System;
using System.Globalization;
using System.IO;

namespace ComicX.Auth
{
    public class AuthManager
    {
        private static string userFilePath = "users.txt";
        private static string lockoutFilePath = "lockout.txt";

        public static void InitializeUsersFile()
        {
            if (!File.Exists(userFilePath))
            {
                File.WriteAllText(userFilePath, "admin,1234\n");
            }
        }

        public static bool IsLockedOut(out TimeSpan remainingTime)
        {
            remainingTime = TimeSpan.Zero;

            if (File.Exists(lockoutFilePath))
            {
                string lockoutTimeStr = File.ReadAllText(lockoutFilePath);
                if (DateTime.TryParse(lockoutTimeStr, out DateTime lockoutExpiration))
                {
                    if (DateTime.Now < lockoutExpiration)
                    {
                        remainingTime = lockoutExpiration - DateTime.Now;
                        return true;
                    }
                    else
                    {
                        File.Delete(lockoutFilePath);
                    }
                }
            }
            return false;
        }

        public static void SetLockout()
        {
            DateTime experationTime = DateTime.Now.AddMinutes(30);
            File.WriteAllText(lockoutFilePath, experationTime.ToString());
        }

        public static bool ValidateCredentials(string username, string password)
        {
            InitializeUsersFile();

            string[] lines = File.ReadAllLines(userFilePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');

                if (parts.Length >= 2 && parts[0].Trim() == username && parts[1].Trim() == password) return true;
            }

            return false;
        }
    }
}
