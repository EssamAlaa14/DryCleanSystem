using Application.Common.DTOs.InfoDtos;
using Application.Common.Interfaces.Infrastructure;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Features.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersQuery : IRequest<List<OrderDto>>
    {
    }

    public class GetAllOrdersQueryValidator : AbstractValidator<GetAllOrdersQuery>
    {
        public GetAllOrdersQueryValidator() { }
    }

    public class GetAllOrdersQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.Orders.GetAllAsync(cancellationToken, x => x.Customer);
            var ordersList = _mapper.Map<List<OrderDto>>(orders);
            return ordersList;
        }
    }
}
