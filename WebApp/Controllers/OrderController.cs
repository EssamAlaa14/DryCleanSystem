using Application.Common.DTOs.UpdateDtos;
using Application.Features.CleaningTypes.GetAllCleaningTypes;
using Application.Features.Customers.Queries.GetAllCustomers;
using Application.Features.Customers.Queries.GetCustomerAddresses;
using Application.Features.Districts.Queries.GetAllDistricts;
using Application.Features.Orders.Commands.UpdateOrderStatus;
using Application.Features.Orders.Queries.GetAllOrders;
using Application.Features.Orders.Queries.GetOrderById;
using Application.Features.Pieces.Queries.GetAllPiecesByOrderType;
using Domain.Enums;
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

        [HttpGet]
        [Route("/orders")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var allOrders = await _mediator.Send(new GetAllOrdersQuery(), cancellationToken);
            return View(allOrders);
        }

        [HttpGet]
        [Route("[controller]/[action]/{orderType:int:min(1):max(3)}")]
        public async Task<IActionResult> Create(int orderType, CancellationToken cancellationToken)
        {
            var orderListModel = await GetOrderListItemsViewModel((OrderType)orderType, cancellationToken);
            var viewModel = new CreateOrderViewModel
            {
                Command = new Application.Features.Orders.Commands.CreateOrder.CreateOrderCommand
                {
                    Order = new Application.Common.DTOs.CreateDtos.CreateOrderDto
                    {
                        OrderType = (OrderType)orderType
                    }
                },
                OrderListItemsModel = orderListModel,
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateOrderViewModel model, CancellationToken cancellationToken)
        {
            var orderType = model.Command.Order.OrderType;
            try
            {
                if (!ModelState.IsValid)
                {
                    var orderListModel = await GetOrderListItemsViewModel(orderType, cancellationToken);
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

                var orderListModel = await GetOrderListItemsViewModel(orderType, cancellationToken);
                model.OrderListItemsModel = orderListModel;
                return View(model);
            }
        }

        [HttpGet]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery { Id = id }, cancellationToken);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details([FromBody] UpdateOrderDto order, CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(new UpdateOrderStatusCommand { Id = order.OrderId, OrderStatus = order.OrderStatus }, cancellationToken);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating order status.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Route("[controller]/[action]/{customerId}")]
        public async Task<IActionResult> GetCustomerAddressesList(Guid customerId, CancellationToken cancellationToken)
        {
            var addressesList = await _mediator.Send(new GetCustomerAddressesQuery { CustomerId = customerId }, cancellationToken);
            return Json(addressesList);
        }

        private async Task<BaseOrderListItemsViewModel> GetOrderListItemsViewModel(OrderType orderType, CancellationToken cancellationToken)
        {
            var customersList = await _mediator.Send(new GetAllCustomersQuery(), cancellationToken);
            var piecesList = await _mediator.Send(new GetAllPiecesByOrderTypeQuery() { OrderType = orderType }, cancellationToken);
            var cleaningTypesList = await _mediator.Send(new GetAllCleaningTypesQuery(), cancellationToken);
            var districtsList = await _mediator.Send(new GetAllDistrictsQuery(), cancellationToken);

            return new BaseOrderListItemsViewModel
            {
                Customers = customersList,
                Pieces = piecesList,
                CleaningTypeOptions = cleaningTypesList,
                Districts = districtsList
            };
        }
    }
}
