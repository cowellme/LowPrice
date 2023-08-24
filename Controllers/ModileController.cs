using LowPrice.Models.Market;
using LowPrice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using System;
using System.Net.Sockets;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using UAParser;

namespace LowPrice.Controllers
{
    public class ModileController : Controller
    {
        //Основная страница
        public ActionResult Index()
        {

            var model = AllItems.GetAll();
            model.ci = ThisUserAgent();
            return View(model);

        }



        //Сравнение товара
        public IActionResult CheckItem(string action, string hash)
        {
            if (action == "check")
            {
                var it = Item.GetByHash(hash);

                return View(it);
            }
            else if(action != "del")
            {
                string ip = ThisIpAdress();
                Auth.UpdateLiked(ip, hash, null);
                return RedirectToAction("Index");
            }
            else
            {
                string ip = ThisIpAdress();
                Auth.UpdateLiked(ip, hash, action);
                return RedirectToAction("Liked");
            }
        }



        //Посик везде
        [HttpPost]
        public IActionResult Search(string name, string? priceMin, string? priceMax, string? creater, string? forthe, string? color)
        {
            var it = new Item()
            {
                Name = name,
            };

            if (color != null)
                it.Color = color;
            if (creater != null)
                it.Creater = creater;

            return RedirectToAction("Search", "Modile", it);
        }
        [HttpGet]
        public IActionResult Search(Item it)
        {
            return View(it);
        }
        public IActionResult Search()
        {
            throw new NotImplementedException();
        }


        
        

        //Регистрация
        public IActionResult Regist(string number, string? act, string? ips)
        {
            if (act == "regist")
            {
                var ip = ThisIpAdress();

                Auth.CreateUser(number, ip);

                return RedirectToAction("Liked", "Modile");
            }
            else
            {
                return RedirectToAction("Liked", "Modile");
            }
            
        }


        //Избранное
        public IActionResult Liked(Liked model)
        {
            if (model.Ip == null)
            {
                model.Ip = ThisIpAdress();
            }
            return View(model);
        }



        //Получение IP
        public string ThisIpAdress()
        {
            string userIpAddress = this.Request.HttpContext.Connection.RemoteIpAddress.ToString();

            return userIpAddress;
        }



        //Получение User-Agenta
        public ClientInfo ThisUserAgent()
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"];
            var uaParser = Parser.GetDefault();
            ClientInfo c = uaParser.Parse(userAgent);
            return c;
        }

        public IActionResult Userd()
        {
            return View(); 
        }
        public IActionResult Catygory()
        {
            return View();
        }
    }
}
