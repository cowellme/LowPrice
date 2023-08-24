using LowPrice.Models.Market;
using Microsoft.AspNetCore.Mvc;
using UAParser;

namespace LowPrice.Controllers;

public class UserdController : Controller
{
    public IActionResult Index()
    {
        Liked model = new Liked();
        
        model.Ip = ThisIpAdress();
        return View(model);
    }


    //IP
    public string ThisIpAdress()
    {
        string userIpAddress = this.Request.HttpContext.Connection.RemoteIpAddress.ToString();

        return userIpAddress;
    }


    //UA
    public ClientInfo ThisUserAgent()
    {
        var userAgent = HttpContext.Request.Headers["User-Agent"];
        var uaParser = Parser.GetDefault();
        ClientInfo c = uaParser.Parse(userAgent);
        return c;
    }
}