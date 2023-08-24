using LowPrice.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using LowPrice.Models.Market;
using System.Net.Sockets;
using System.Net;
using UAParser;

namespace LowPrice.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            
            var model = AllItems.GetAll();
            var c = model.ci = ThisUserAgent();
            
            if (c.OS.Family.Contains("Android") || c.OS.Family.Contains("iOS"))
            {
                return RedirectToAction("Index", "Modile", model);
            }
            else
            {
                model.Ip = ThisIpAdress();
                return View(model);
            }

        }




        //Методы поиска товара в везде
        [HttpPost]
        public IActionResult Search(string name, string? priceMin, string? priceMax, string? creater, string? forthe, string? color)
        {
            var it = new Item()
            {
                Name = name,
            };

            if(color != null)
                it.Color = color;
            if(creater  != null)
                it.Creater = creater;
            
            return RedirectToAction("Search", "Home", it);
        }
        [HttpGet]
        public IActionResult Search(Item it)
        {
            return View(it);
        }




        //Сравнение товара
        public IActionResult CheckItem(string action, string hash)
        {
            string ip = ThisIpAdress();
            switch (action)
            {
                case ("CheckItem"):
                    var it = Item.GetByHash(hash);

                    return View(it);
                    break;

                case ("del"):
                    Auth.UpdateLiked(ip, hash, action);
                    return RedirectToAction("Liked");
                    break;
                    
                default: 
                    var buf = Auth.UpdateLiked(ip, hash, null);
                    if (!buf)
                        return RedirectToAction("Index", "Userd");
                    return RedirectToAction("Index");
                    break;
            }
            
        }




        //Работа с Избранным
        [HttpPost]
        public IActionResult Liked(string number)
        {
            //TODO: Parse List Liked

            var liked = new Liked();
            liked.Ip = ThisIpAdress();
            return RedirectToAction("Liked", "Home", liked);
        }
        [HttpGet]
        public IActionResult Liked(Liked model)
        {
            try
            {
                model.Ip = ThisIpAdress();
            }
            catch (Exception e)
            {
                Console.WriteLine(e + @"\n\nError with take IP");
                throw;
            }
            
            return View(model);
        }
        private void Unliked(string hash, string ip)
        {
            var num = Auth.Login(ip);

        }



        //Методы поиска товара в категориях
        [HttpPost]
        public IActionResult GrandSearch(string number)
        {
            //TODO: Parse List Liked

            var liked = new Liked()
            {

            };

            return RedirectToAction("GrandSearch", "Home", liked);
        }
        [HttpGet]
        public IActionResult GrandSearch(AllItems model)
        {
            //TODO: Parse List Liked
            model.Ip = ThisIpAdress();
            return View(model);
        }



        //Работа с Профилем
        public IActionResult Regist(string number)
        {
            var ip = ThisIpAdress();

            Auth.CreateUser(number, ip);

            return RedirectToAction("Index", "Home");
        }
        



        /// <summary>
        /// Получение IP адреса пользователя
        /// </summary>
        /// <returns>IP Адресс</returns>
        public string ThisIpAdress()
        {
            string userIpAddress = this.Request.HttpContext.Connection.RemoteIpAddress.ToString();

            return userIpAddress;
        }

        /// <summary>
        /// Получение UserAgent пользователя, для понимания какая платформа
        /// </summary>
        /// <returns></returns>
        public ClientInfo ThisUserAgent()
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"];
            var uaParser = Parser.GetDefault();
            ClientInfo c = uaParser.Parse(userAgent);
            return c;
        }
    }
}