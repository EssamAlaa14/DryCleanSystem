using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Controllers.Base;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController(
        IMediator mediator,
        ILogger<HomeController> logger
        ) : BaseController(mediator)
    {
        private readonly ILogger<HomeController> _logger = logger;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
