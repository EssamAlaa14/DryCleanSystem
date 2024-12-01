using Application.Common.Interfaces.Infrastructure;
using Application.Features.Orders.Commands.CreateOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class OrderController(
        IMediator mediator,
        IUnitOfWork unitOfWork,
        ILogger<OrderController> logger
        ) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<OrderController> _logger = logger;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var customersList = await _unitOfWork.Customers.GetAllAsync(cancellationToken);
            var customersVm = customersList
                .Select(c => new CustomerDropdownViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    PrimaryPhone = c.PrimaryPhone.ToString()
                })
                .ToList();

            var cleaningTypes = await _unitOfWork.CleaningTypes.GetAllAsync(cancellationToken);
            var cleaningTypesVm = cleaningTypes
                .Select(ct => new CleaningTypeOptionViewModel
                {
                    Id = ct.Id,
                    Name = ct.Name,
                    NameArabic = ct.NameArabic
                })
                .ToList();

            var viewModel = new CreateOrderViewModel
            {
                Customers = customersVm,
                CleaningTypeOptions = cleaningTypesVm
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderViewModel model, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var customersList = await _unitOfWork.Customers.GetAllAsync(cancellationToken);
                    var customersVm = customersList
                        .Select(c => new CustomerDropdownViewModel
                        {
                            Id = c.Id,
                            Name = c.Name,
                            PrimaryPhone = c.PrimaryPhone.ToString()
                        })
                        .ToList();

                    var cleaningTypes = await _unitOfWork.CleaningTypes.GetAllAsync(cancellationToken);
                    var cleaningTypesVm = cleaningTypes
                        .Select(ct => new CleaningTypeOptionViewModel
                        {
                            Id = ct.Id,
                            Name = ct.Name,
                            NameArabic = ct.NameArabic
                        })
                        .ToList();

                    model.Customers = customersVm;
                    model.CleaningTypeOptions = cleaningTypesVm;
                    return View(model);
                }

                var command = new CreateOrderCommand
                {
                    IsNewCustomer = model.IsNewCustomer,
                    SelectedCustomerId = model.SelectedCustomerId,
                    NewCustomer = model.NewCustomer,
                    Order = model.Order
                };

                await _mediator.Send(command, cancellationToken);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception (use your preferred logging framework)
                _logger.LogError(ex, "Error occurred while creating an order.");

                // Add a general error message to ModelState
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");

                var customersList = await _unitOfWork.Customers.GetAllAsync(cancellationToken);
                var customersVm = customersList
                    .Select(c => new CustomerDropdownViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        PrimaryPhone = c.PrimaryPhone.ToString()
                    })
                    .ToList();

                var cleaningTypes = await _unitOfWork.CleaningTypes.GetAllAsync(cancellationToken);
                var cleaningTypesVm = cleaningTypes
                    .Select(ct => new CleaningTypeOptionViewModel
                    {
                        Id = ct.Id,
                        Name = ct.Name,
                        NameArabic = ct.NameArabic
                    })
                    .ToList();

                model.Customers = customersVm;
                model.CleaningTypeOptions = cleaningTypesVm;
                return View(model);
            }
        }
    }
}
