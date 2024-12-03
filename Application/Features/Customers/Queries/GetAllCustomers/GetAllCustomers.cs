using Application.Common.DTOs.InfoDtos;
using Application.Common.Interfaces.Infrastructure;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Features.Customers.Queries.GetAllCustomers
{
    public record GetAllCustomersQuery : IRequest<List<CustomerDto>>
    {
    }

    public class GetAllCustomersQueryValidator : AbstractValidator<GetAllCustomersQuery>
    {
        public GetAllCustomersQueryValidator() { }
    }

    public class GetAllCustomersQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<GetAllCustomersQuery, List<CustomerDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<List<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _unitOfWork.Customers.GetAllAsync(cancellationToken);
            var customersList = _mapper.Map<List<CustomerDto>>(customers);
            return customersList;
        }
    }
}
