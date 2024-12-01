using Application.Common.Interfaces.Infrastructure;
using Domain.Entities;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;

namespace Application.Features.Customers.Commands.CreateCustomer
{
    public record CreateCustomerCommand : IRequest<Guid>
    {
        required public string Name { get; set; }
        required public string PrimaryPhone { get; set; }
        public string? SecondaryPhone { get; set; }
        public string? Email { get; set; }

        // Address components
        required public string AppartmentNumber { get; set; }
        required public string FloorNumber { get; set; }
        required public string BuildingName { get; set; }
        required public string Street { get; set; }
        required public string District { get; set; }
        public string? City { get; set; }
    }

    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Customer name is required.")
                .MaximumLength(100)
                .WithMessage("Customer name must not exceed 100 characters.");

            RuleFor(x => x.PrimaryPhone)
                .NotEmpty()
                .WithMessage("Primary phone is required.")
                .Matches(@"^\d{7,15}$")
                .WithMessage("Primary phone must be a valid phone number.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Email must be a valid email address.");

            RuleFor(x => x.AppartmentNumber)
                .NotEmpty()
                .WithMessage("Appartment number is required.");

            RuleFor(x => x.FloorNumber)
                .NotEmpty()
                .WithMessage("Floor number is required.");

            RuleFor(x => x.BuildingName)
                .NotEmpty()
                .WithMessage("Building name is required.");

            RuleFor(x => x.Street)
                .NotEmpty()
                .WithMessage("Street is required.");

            RuleFor(x => x.District)
                .NotEmpty()
                .WithMessage("District is required.");
        }
    }

    public class CreateCustomerCommandHandler(
        IUnitOfWork unitOfWork
        ) : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // Construct the Address value object
            var address = new Address(
                appartmentNumber: request.AppartmentNumber,
                floorNumber: request.FloorNumber,
                buildingName: request.BuildingName,
                street: request.Street,
                district: request.District,
                city: request.City ?? "Alexandria"
            );

            // Construct the Customer entity
            var customer = new Customer(
                name: request.Name,
                primaryPhone: new Phone(request.PrimaryPhone),
                address: address,
                email: string.IsNullOrWhiteSpace(request.Email) ? null : new Email(request.Email),
                secondaryPhone: string.IsNullOrWhiteSpace(request.SecondaryPhone) ? null : new Phone(request.SecondaryPhone)
            );

            // Persist the customer to the database
            await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return customer.Id; // Return the ID of the created customer
        }
    }
}
