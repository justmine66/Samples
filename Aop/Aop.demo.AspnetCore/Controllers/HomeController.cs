using System;
using System.Diagnostics;
using System.Net.Http;
using Aop.demo.AspnetCore.Attritbutes;
using Aop.demo.AspnetCore.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DiagnosticAdapter;

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
            DiagnosticListener.AllListeners.Subscribe(listener =>
            {
                Console.WriteLine(listener.Name);
                if (listener.Name == "HttpHandlerDiagnosticListener")
                {
                    listener.SubscribeWithAdapter(new HttpClientListener());
                }
            });

            Console.WriteLine(Description);

            Car.Fire();

            return View();
        }

        private sealed class HttpClientListener
        {
            [DiagnosticName("System.Net.Http.HttpRequestOut.Start")]
            public void OnSend(HttpRequestMessage request) => AddDefaultHeaders(request);

            [DiagnosticName("System.Net.Http.HttpRequestOut")]
            public void OnSend() { }
        }

        private static void AddDefaultHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("x-www-foo", "123");
            request.Headers.Add("x-www-bar", "456");
            request.Headers.Add("x-www-baz", "789");
        }

    }
}
