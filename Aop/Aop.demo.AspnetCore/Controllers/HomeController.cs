using System;
using Aop.demo.AspnetCore.Attritbutes;
using Aop.demo.AspnetCore.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Aop.demo.AspnetCore.Controllers
{
    public class HomeController : Controller
    {
        [AutoWired]
        public ICar Car { set; get; }

        [Value("description")]
        public string Description { set; get; }

        public IActionResult Index()
        {
            Console.WriteLine(Description);

            Car.Fire();

            return View();
        }
    }
}
