using Application.Common.DTOs.InfoDtos;
using Application.Common.Interfaces.Infrastructure;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Features.Customers.Queries.GetCustomerAddresses
{
    public record GetCustomerAddressesQuery : IRequest<List<CustomerAddressDto>>
    {
        public Guid CustomerId { get; set; }
    }

    public class GetCustomerAddressesQueryValidator : AbstractValidator<GetCustomerAddressesQuery>
    {
        public GetCustomerAddressesQueryValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("Customer Id is required.")
                .NotNull()
                .WithMessage("Customer Id is required.");
        }
    }

    public class GetCustomerAddressesQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<GetCustomerAddressesQuery, List<CustomerAddressDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<List<CustomerAddressDto>> Handle(GetCustomerAddressesQuery request, CancellationToken cancellationToken)
        {
            var addresses = await _unitOfWork.Addresses.GetByPredicateAsync(x => x.CustomerId == request.CustomerId, cancellationToken, x => x.District);
            var addressesList = _mapper.Map<List<CustomerAddressDto>>(addresses);
            return addressesList;
        }
    }
}
