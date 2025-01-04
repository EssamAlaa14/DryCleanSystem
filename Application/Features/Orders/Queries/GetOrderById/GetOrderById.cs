using Application.Common.DTOs.InfoDtos;
using Application.Common.Interfaces.Infrastructure;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Features.Orders.Queries.GetOrderById
{
    public record GetOrderByIdQuery : IRequest<OrderDto>
    {
        public Guid Id { get; set; }
    }

    public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
    {
        public GetOrderByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Order Id is required.")
                .NotNull()
                .WithMessage("Order Id is required.");
        }
    }

    public class GetOrderByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetByIdIncludeStringAsync(request.Id, cancellationToken, new string[]
            {
                "Customer", "Items", "Items.Piece", "Items.CleaningTypes"
            });
            var address = await _unitOfWork.Addresses.GetByIdAsync(order.AddressId, cancellationToken, x => x.District);
            var orderDto = _mapper.Map<OrderDto>(order);
            orderDto.AddressDetails = address.ToString();
            return orderDto;
        }
    }
}
