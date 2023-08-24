using LowPrice.Models.Admin;
using System;
using System.IO;
using System.Xml.Linq;
using UAParser;

namespace LowPrice.Models.Market
{
    public class AllItems
    {
        private static string PATH = Environment.CurrentDirectory + @"\AllItems";
        public string Ip { get; set; }
        public List<Category> Catygories { get; set; }
        public ClientInfo ci { get; set; }


        /// <summary>
        /// Метод для получения всех Товаров
        /// </summary>
        /// <returns></returns>
        public static AllItems GetAll()
        {
            var dir = Directory.GetDirectories(PATH); 
            var listCaty = new List<Category>();
            foreach (var caty in dir)
            {
                var name = caty.Replace($@"{PATH}\", "");
                listCaty.Add(Category.GetAll(name));
            }

            var allItems = new AllItems()
            {
               Catygories = listCaty
            };
            return allItems;
        }
    }

    public class Category
    {
        private static string PATH = Environment.CurrentDirectory + @"\AllItems";
        public string Name { get; set; }
        public List<Item>? Items { get; set; }


        /// <summary>
        /// Метод для получения всех Категорий
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Category GetAll(string name)
        {
            var dir = Directory.GetFiles(PATH + @$"\{name}", "*.json");
            var it = new List<Item>();
            foreach (var filename in dir)
            {
                var jsonString = File.ReadAllText(filename);
                it.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<Item>(jsonString));
            }
            var category = new Category()
            {
                Name = name,
                Items = it
            };
            return category;
        }
    }

    public class Item
    {
        /// <summary>
        /// Получить все цвета
        /// </summary>
        /// <returns></returns>
        public static List<string> GetColors()
        {
            var crts = File.ReadAllLines(Environment.CurrentDirectory + @"\wwwroot\colors\colors.txt");
            var creaters = new List<string>();
            creaters.AddRange(crts);
            return creaters;
        }

        /// <summary>
        /// Установить Цвет
        /// </summary>
        /// <param name="name"></param>
        public static void SetColor(string name)
        {
            name = name.Replace(@"\n", "");
            bool check = false;
            var list = GetColors();
            foreach (var color in list)
            {
                if (color == name)
                {
                    check = true; break;
                }
            }

            if (!check)
                System.IO.File.AppendAllText(Environment.CurrentDirectory + @"\wwwroot\colors\colors.txt", name + Environment.NewLine);
        }

        /// <summary>
        /// Получить всеx производителей
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCreaters()
        {
            var crts = File.ReadAllLines(Environment.CurrentDirectory + @"\wwwroot\creaters\creaters.txt");
            var creaters = new List<string>();
            creaters.AddRange(crts);
            return creaters;
        }

        /// <summary>
        /// Установить производителя
        /// </summary>
        /// <returns></returns>
        public static void SetCreater(string name)
        {
            name = name.Replace(@"\n","");
            bool check = false;
            var list = GetCreaters();
            foreach (var crt in list)
            {
                if (crt == name)
                {
                    check = true; break;
                }
            }

            if (!check)
                System.IO.File.AppendAllText(Environment.CurrentDirectory + @"\wwwroot\creaters\creaters.txt", name + Environment.NewLine);
        }

        /// <summary>
        /// Поиск по названию
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<Item> GetSearchByItem(string name)
        {
            var path = Environment.CurrentDirectory + @"\wwwroot\all";
            var files = Directory.GetFiles(path, "*.json");
            var items = new List<Item>();
            foreach (var file in files)
            {
                string jsonString = System.IO.File.ReadAllText(file);
                Item it = Newtonsoft.Json.JsonConvert.DeserializeObject<Item>(jsonString);
                var lname = it.Name.ToLower();
                var slname = name.ToLower();
                if (lname.Contains(slname))
                {
                    items.Add(it);
                }
            }

            return items;
        }

        /// <summary>
        /// Поиск по цвету
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<Item> GetSearchByColor(string name)
        {
            try
            {
                var path = Environment.CurrentDirectory + @"\wwwroot\all";
                var files = Directory.GetFiles(path, "*.json");
                var items = new List<Item>();
                foreach (var file in files)
                {
                    string jsonString = System.IO.File.ReadAllText(file);
                    Item it = Newtonsoft.Json.JsonConvert.DeserializeObject<Item>(jsonString);
                    if (it.Description.Color.Contains(name))
                    {
                        items.Add(it);
                    }
                }

                return items;
            }
            catch (Exception e)
            {
                var items = new List<Item>(); return items;
            }
        }

        /// <summary>
        /// Поиск по производителю
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<Item> GetSearchByCreater(string name)
        {
            var path = Environment.CurrentDirectory + @"\wwwroot\all";
            var files = Directory.GetFiles(path, "*.json");
            var items = new List<Item>();
            foreach (var file in files)
            {
                string jsonString = System.IO.File.ReadAllText(file);
                Item it = Newtonsoft.Json.JsonConvert.DeserializeObject<Item>(jsonString);
                if (it.Creater.Contains(name))
                {
                    items.Add(it);
                }
            }

            return items;
        }

        /// <summary>
        /// Поиск по Хэш коду
        /// </summary>
        /// <param hash="hash"></param>
        /// <returns></returns>
        public static Item GetByHash(string hash)
        {
            if (hash != "none")
            {
                string path = Environment.CurrentDirectory + @$"\wwwroot\all\{hash}.json";
                string jsonString = System.IO.File.ReadAllText(path);
                var it = Newtonsoft.Json.JsonConvert.DeserializeObject<Item>(jsonString);
                return it;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Метод для получения всех Предметов
        /// </summary>
        /// <returns></returns>
        public Item GetAll()
        {
            var item = new Item()
            {

            };
            return item;
        }


        /// <summary>
        /// Свойства объекта
        /// </summary>
        public string Hash { get; set; }//s
        public string Category { get; set; }//s
        public string Name { get; set; }
        public Description Description { get; set; }
        public bool Like { get; set; }
        public string Creater { get; set; }
        public string Color { get; set; }
        public string Price { get; set; }
        public string PhotoPath { get; set; }
    }

    public class Description
    {
        /// <summary>
        /// Свойства объекта
        /// </summary>
        public string Color { get; set; }
        public string Descr { get; set; }
        public string Matireal { get; set; }
    }
    


}
