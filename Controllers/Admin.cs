using System.Net.Http.Headers;
using System.Net.Mime;
using LowPrice.Models.Admin;
using LowPrice.Models.Market;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using LowPrice.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Net.Mime.MediaTypeNames;

namespace LowPrice.Controllers
{
    public class Admin : Controller
    {
        private string PATH = Environment.CurrentDirectory + @"\AllItems";
        private string PATHwww = Environment.CurrentDirectory + @"\wwwroot";




        [HttpGet] //Первая страница Админа
        public IActionResult Index()
        {
            var AllItems = new AllItems()
            {
                Catygories = new List<Category>()
            };
            var dir = System.IO.File.ReadAllLines(PATH + @"\Categories.txt");
            foreach (var caty in dir)
            {
                var str = caty.Replace($@"{PATH}\", "");
                var categories = new Category()
                {
                    Name = Slovar.CategoruRus(str)
                };
                
                AllItems.Catygories.Add(categories);
            }
            return View(AllItems);
        }
        [HttpPost] //Обработка действий Админа
        public IActionResult Index(string categoryEN, string categoryRU)
        {
            if(categoryEN == null || categoryRU == null)
                return BadRequest();


            Directory.CreateDirectory(PATH + @$"\{categoryEN}");
            System.IO.File.AppendAllText(PATH + @"\Categories.tmp", $"{categoryEN}::{categoryRU}" + Environment.NewLine);
            return RedirectToAction("Index", "Admin");
        }




        [HttpGet]
        public IActionResult Category(string name)
        {
            name = Slovar.CategoruEng(name);
            
            var model = CreateKatalog(name);
            return View(model);
        }




        /// <summary>
        /// Создание нового каталога
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Объект класса Category</returns>
        private Category CreateKatalog(string name)
        {
            var path = PATH + @$"\{name}";
            var caty = new Category()
            {
                Name = name
            };
            var dir = Directory.GetFiles(path, "*.json");
            var list = new List<Item>();
            foreach (var item in dir)
            {
                var jsonString = System.IO.File.ReadAllText(item);
                var it = Newtonsoft.Json.JsonConvert.DeserializeObject<Item>(jsonString);
                list.Add(it);
            }

            caty.Items = list;

            return caty;
        }




        /// <summary>
        /// Создание нового товара
        /// </summary>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="descr"></param>
        /// <param name="material"></param>
        /// <param name="color"></param>
        /// <param name="category"></param>
        /// <param name="creater"></param>
        /// 
        public IActionResult AddItem(string name, string price, string descr, string material, string color, string category, string creater)
        {
            Item.SetColor(color);
            Item.SetCreater(creater);
            var hash = CreateHashCode(name);
            var descrp = new Description()
            {
                Color = color,
                Matireal = material,
                Descr = descr,
            };
            var it = new Item()
            {
                Hash = hash,
                Category = category,
                Name = name,
                Price = price,
                Creater = creater,
                Description = descrp,
                PhotoPath = $@"{PATH}\photo\{hash}.jpeg"
            };

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(it);

            System.IO.File.WriteAllText($@"{PATH}\{category}\{hash}.json", jsonString);
            System.IO.File.WriteAllText($@"{PATHwww}\all\{hash}.json", jsonString);
            Auth.ParserFace(hash);
            var ph = new Photo()
            {
                Hash = hash,
                Category = category
            };
            return RedirectToAction("AddPhoto", "Admin", ph);
        }



        //Создание зашифрованного названия
        private string CreateHashCode(string str)
        {
            StringBuilder sb = new StringBuilder();
            
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashValue = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                
                foreach (byte b in hashValue)
                {
                    sb.Append($"{b:X2}");
                }
            }

            return sb.ToString();
        }




        [HttpGet] //GET запрос для загрузки фото
        public async Task<IActionResult> AddPhoto(Photo ph)
        {
            return View(ph);
        } 

        [HttpPost] //POST запрос для загрузки фото
        public async Task<IActionResult> AddPhoto(IFormFile file,string category, string hash)
        {
            if (file.Length > 0)
            {
                string filePath = $@"{PATHwww}\photo\{hash}.jpeg";
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }


            return RedirectToAction("Index");
        }




        /// <summary>
        /// Удаление предмета из каталога
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="caty"></param>
        /// <returns>Возврат на страницу Админа</returns>
        public IActionResult Delete(string hash, string caty)
        {
            System.IO.File.Delete(Environment.CurrentDirectory + $@"\wwwroot\all\{hash}.json"); 
            System.IO.File.Delete(Environment.CurrentDirectory + $@"\wwwroot\photo\{hash}.jpeg");
            System.IO.File.Delete(Environment.CurrentDirectory + $@"\AllItems\{caty}\{hash}.json");

            var AllItems = new AllItems()
            {
                Catygories = new List<Category>()
            };
            var dir = System.IO.File.ReadAllLines(PATH + @"\Categories.txt");
            foreach (var acaty in dir)
            {
                var str = acaty.Replace($@"{PATH}\", "");
                var categories = new Category()
                {
                    Name = Slovar.CategoruRus(str)
                };

                AllItems.Catygories.Add(categories);
            }
            return View("Index", AllItems);
        }
    }
}
