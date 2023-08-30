using Microsoft.AspNetCore.Mvc;
using MultiSms.Interfaces;
using MultiSms.Models;
using Sms.Models;
using System.Diagnostics;

namespace Sms.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}