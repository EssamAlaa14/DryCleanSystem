using Application.Common.DTOs.InfoDtos;
using Application.Features.Orders.Commands.CreateOrder;

namespace WebApp.ViewModels
{
    public class BaseOrderListItemsViewModel
    {
        public List<CustomerDto> Customers { get; set; } = [];
        public List<PieceDto> Pieces { get; set; } = [];
        public List<CleaningTypeDto> CleaningTypeOptions { get; set; } = [];
    }

    public class CreateOrderViewModel
    {
        public BaseOrderListItemsViewModel? OrderListItemsModel { get; set; } = new BaseOrderListItemsViewModel();
        public CreateOrderCommand Command { get; set; } = new CreateOrderCommand();
    }
}
