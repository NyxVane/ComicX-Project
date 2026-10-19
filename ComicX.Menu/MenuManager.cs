using System;
using System.Collections.Generic;
using System.IO;

namespace ComicX.Menu
{
    public class MenuManager
    {
        private static string stockFile = "stock.txt";

        public static readonly Dictionary<string, decimal> ComicPrices = new Dictionary<string, decimal>
        {
            { "Batman", 22m },
            { "Joker", 13m },
            { "Cat", 11m },
            { "TheRiddler", 15m },
            { "Pinguin", 19m }
        };

        // Csak akkor hozza létre az alapértelmezett fájlt, ha még egyáltalán NEM létezik!
        public static void InitializeStockFile()
        {
            if (!File.Exists(stockFile))
            {
                string[] defaultStock = {
                    "Batman,1200",
                    "Joker,3500",
                    "Cat,865",
                    "TheRiddler,412",
                    "Pinguin,2000"
                };
                File.WriteAllLines(stockFile, defaultStock);
            }
        }

        public static Dictionary<string, int> GetStock()
        {
            InitializeStockFile();
            var stockDict = new Dictionary<string, int>();

            string[] lines = File.ReadAllLines(stockFile);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length >= 2)
                {
                    string theme = parts[0].Trim();
                    if (int.TryParse(parts[1], out int count))
                    {
                        stockDict[theme] = count;
                    }
                }
            }
            return stockDict;
        }

        public static Dictionary<string, decimal> GetTotalValues()
        {
            var stock = GetStock();
            var values = new Dictionary<string, decimal>();

            foreach (var item in stock)
            {
                string theme = item.Key;
                int count = item.Value;

                if (ComicPrices.ContainsKey(theme))
                {
                    values[theme] = count * ComicPrices[theme];
                }
            }
            return values;
        }

        // 1 db eladása (visszafelé kompatibilitás miatt)
        public static bool OneBookSold(string themeType)
        {
            return RegisterSale(themeType, 1);
        }

        // Új metódus tetszőleges mennyiség eladására készletellenőrzéssel
        public static bool RegisterSale(string themeType, int quantity)
        {
            if (quantity <= 0) return false;

            var stock = GetStock();

            string matchedKey = null;
            foreach (var key in stock.Keys)
            {
                if (key.Equals(themeType?.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    matchedKey = key;
                    break;
                }
            }

            // Ellenőrizzük, hogy létezik-e a téma ÉS van-e elég készleten
            if (matchedKey != null && stock[matchedKey] >= quantity)
            {
                stock[matchedKey] -= quantity;

                var lines = new List<string>();
                foreach (var item in stock)
                {
                    lines.Add($"{item.Key},{item.Value}");
                }
                File.WriteAllLines(stockFile, lines);
                return true;
            }

            return false;
        }

        public static string GetStockStatus()
        {
            var stock = GetStock();
            int totalStock = 0;
            foreach (var item in stock)
            {
                totalStock += item.Value;
            }

            if (totalStock < 1000)
            {
                return $"veryLow ({totalStock} books)";
            }
            else if (totalStock >= 1000 && totalStock <= 1500)
            {
                return $"Low ({totalStock} books)";
            }
            else if (totalStock > 1500 && totalStock <= 5000)
            {
                return $"Normal ({totalStock} books)";
            }
            else
            {
                return $"Over ({totalStock} books)";
            }
        }
    }
}