using Application.Features.CleaningTypes.GetAllCleaningTypes;
using Application.Features.Customers.Queries.GetAllCustomers;
using Application.Features.Pieces.Queries.GetAllPieces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers.Base;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class OrderController(
        IMediator mediator,
        ILogger<OrderController> logger
        ) : BaseController(mediator)
    {
        private readonly ILogger<OrderController> _logger = logger;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var orderListModel = await GetOrderListItemsViewModel(cancellationToken);
            var viewModel = new CreateOrderViewModel
            {
                OrderListItemsModel = orderListModel,
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateOrderViewModel model, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var orderListModel = await GetOrderListItemsViewModel(cancellationToken);
                    model.OrderListItemsModel = orderListModel;
                    return View(model);
                }

                await _mediator.Send(model.Command, cancellationToken);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an order.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");

                var orderListModel = await GetOrderListItemsViewModel(cancellationToken);
                model.OrderListItemsModel = orderListModel;
                return View(model);
            }
        }

        private async Task<BaseOrderListItemsViewModel> GetOrderListItemsViewModel(CancellationToken cancellationToken)
        {
            var customersList = await _mediator.Send(new GetAllCustomersQuery(), cancellationToken);
            var piecesList = await _mediator.Send(new GetAllPiecesQuery(), cancellationToken);
            var cleaningTypesList = await _mediator.Send(new GetAllCleaningTypesQuery(), cancellationToken);

            return new BaseOrderListItemsViewModel
            {
                Customers = customersList,
                Pieces = piecesList,
                CleaningTypeOptions = cleaningTypesList
            };
        }
    }
}
