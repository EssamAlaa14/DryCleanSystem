using Application.Common.DTOs.CreateDtos;

namespace WebApp.ViewModels
{
    public class CreateOrderViewModel
    {
        public Guid? SelectedCustomerId { get; set; }
        public List<CustomerDropdownViewModel> Customers { get; set; } = [];
        public CreateOrderDto Order { get; set; } = new CreateOrderDto();
        public CreateCustomerDto NewCustomer { get; set; } = new CreateCustomerDto();
        public bool IsNewCustomer { get; set; } = false;
        public List<CreateOrderItemDto> Items { get; set; } = [];
        public List<CleaningTypeOptionViewModel> CleaningTypeOptions { get; set; } = [];
    }
}
