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

        public static void InitializeStockFile()
        {
            if (!File.Exists(stockFile))
            {
                string[] defaultStock =
                {
                    "Batman,300",
                    "Joker,300",
                    "Cat,300",
                    "TheRiddler,300",
                    "Pinguin,300"
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

        public static bool OneBookSold(string themeType)
        {
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

            if (matchedKey != null && stock[matchedKey] > 0)
            {
                stock[matchedKey]--;

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
            } else if (totalStock >= 1000 && totalStock <= 1500) {
                return $"Low ({totalStock} books)";
            } else if (totalStock > 1500 && totalStock <= 5000)
            {
                return $"Normal ({totalStock} books)";
            } else
            {
                return $"Over ({totalStock} books)";
            }
        }
    }
}
