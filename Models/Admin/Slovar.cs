using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;

namespace LowPrice.Models.Admin
{
    public class Slovar
    {
        private static string PATH = Environment.CurrentDirectory + @"\AllItems";

        /// <summary>
        /// Перевод категорий
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string CategoruRus(string name)
        {
            var strings = File.ReadAllLines(PATH + @"\Categories.txt");
            foreach (var str in strings)
            {
                if (str == name)
                {
                    name = Regex.Match(str, "(.*)::(.*)").Groups[2].Value;
                }
            }

            return name;
        }
        public static string CategoryToRus(string name)
        {
            var strings = File.ReadAllLines(PATH + @"\Categories.txt");
            foreach (var str in strings)
            {
                if (str.Contains(name))
                    name = Regex.Match(str, "(.*)::(.*)").Groups[2].Value;
            }
            return name;
        }
        public static string CategoruEng(string name)
        {
            var strings = File.ReadAllLines(PATH + @"\Categories.txt");
            foreach (var str in strings)
            {
                var buf = Regex.Match(str, "(.*)::(.*)").Groups[2].Value;
                if (buf == name)
                {
                    name = Regex.Match(str, "(.*)::(.*)").Groups[1].Value;
                }
            }
            return name;
        }
    }
}
