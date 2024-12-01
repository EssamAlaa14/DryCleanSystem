using Application.Common.DTOs.CreateDtos;
using Application.Common.Interfaces.Infrastructure;
using Application.Validators;
using Domain.Entities;
using Domain.Entities.Orders;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;

namespace Application.Features.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand : IRequest<Unit>
    {
        public bool IsNewCustomer { get; set; }
        public CreateCustomerDto? NewCustomer { get; set; }
        public Guid? SelectedCustomerId { get; set; }
        public CreateOrderDto Order { get; set; }
    }

    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator() 
        {
            When(x => x.SelectedCustomerId == null, () =>
            {
                RuleFor(x => x.NewCustomer)
                    .NotNull()
                    .WithMessage("Customer information is required for a new customer.");

                RuleFor(x => x.NewCustomer!.Name)
                    .NotEmpty().WithMessage("Customer name is required.")
                    .MaximumLength(100).WithMessage("Customer name must not exceed 100 characters.");

                RuleFor(x => x.NewCustomer!.PrimaryPhone)
                    .NotEmpty().WithMessage("Primary phone is required.")
                    .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Primary phone must be a valid international phone number.");

                RuleFor(x => x.NewCustomer!.ApartmentNumber)
                    .NotEmpty()
                    .WithMessage("Apartment number is required.");

                RuleFor(x => x.NewCustomer!.FloorNumber)
                    .NotEmpty()
                    .WithMessage("Floor number is required.");

                RuleFor(x => x.NewCustomer!.BuildingName)
                    .NotEmpty()
                    .WithMessage("Building name is required.");

                RuleFor(x => x.NewCustomer!.Street)
                    .NotEmpty()
                    .WithMessage("Street is required.");

                RuleFor(x => x.NewCustomer!.District)
                    .NotEmpty()
                    .WithMessage("District is required.");
            });

            // Order Type Validation
            RuleFor(x => x.Order.OrderType)
                .IsInEnum().WithMessage("OrderType is invalid.");

            // Delivery Date Validation
            RuleFor(x => x.Order.DeliveryDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow).When(x => x.Order.DeliveryDate.HasValue)
                .WithMessage("Delivery date cannot be in the past.");

            // Order Items Validation
            RuleForEach(x => x.Order.Items).SetValidator(new CreateOrderItemDtoValidator());
        }
    }

    public class CreateOrderCommandHandler(
        IUnitOfWork unitOfWork
        ) : IRequestHandler<CreateOrderCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // Handle Customer
                Customer customer;
                if (request.IsNewCustomer && request.SelectedCustomerId.HasValue)
                {
                    customer = await _unitOfWork.Customers.GetByIdAsync(request.SelectedCustomerId.Value, cancellationToken);
                }
                else
                {
                    var address = new Address(
                        request.NewCustomer!.ApartmentNumber,
                        request.NewCustomer!.FloorNumber,
                        request.NewCustomer!.BuildingName,
                        request.NewCustomer!.Street,
                        request.NewCustomer!.District,
                        request.NewCustomer!.City ?? "Alexandria"
                    );

                    customer = new Customer(
                        request.NewCustomer.Name,
                        new Phone(request.NewCustomer.PrimaryPhone),
                        address,
                        null,
                        string.IsNullOrWhiteSpace(request.NewCustomer.SecondaryPhone)
                            ? null
                            : new Phone(request.NewCustomer.SecondaryPhone)
                    );

                    await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
                }

                // Handle Order
                var order = new Order(customer.Id, request.Order.OrderType, deliveryDate: request.Order.DeliveryDate, request.Order.Notes);

                foreach (var item in request.Order.Items)
                {
                    var itemColor = new Color(item.Color);

                    var orderItem = new OrderItem(
                        order.Id,
                        item.Quantity,
                        item.SingleQuantityPrice,
                        itemColor,
                        item.Comment
                    );

                    order.AddItem(orderItem);

                    foreach (var cleaningTypeId in item.CleaningTypeIds)
                    {
                        var cleaningType = await _unitOfWork.CleaningTypes.GetByIdAsync(cleaningTypeId, cancellationToken);
                        orderItem.AddCleaningType(cleaningType);
                    }

                    await _unitOfWork.OrderItems.AddAsync(orderItem, cancellationToken);
                }

                await _unitOfWork.Orders.AddAsync(order, cancellationToken);

                await _unitOfWork.CommitAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return Unit.Value;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
