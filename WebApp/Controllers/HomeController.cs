using Application.Features.Customers.Commands.CreateCustomer;
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View(command); // Return the view with validation messages
            }

            try
            {
                var customerId = await _mediator.Send(command);
                TempData["SuccessMessage"] = $"Customer created successfully with ID: {customerId}";
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                // Handle any exceptions and display error messages
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return View(command);
            }
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
