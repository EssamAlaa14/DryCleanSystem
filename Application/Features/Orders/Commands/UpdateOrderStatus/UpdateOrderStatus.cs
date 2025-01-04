using Application.Common.Interfaces.Infrastructure;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.Orders.Commands.UpdateOrderStatus
{
    public record UpdateOrderStatusCommand : IRequest
    {
        public Guid Id { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string? Reason { get; set; }
    }

    public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Order Id is required.")
                .NotNull()
                .WithMessage("Order Id is required.");

            RuleFor(x => x.OrderStatus)
                .NotEmpty()
                .WithMessage("Order status is required.")
                .NotNull()
                .WithMessage("Order status is required.")
                .IsInEnum();
        }
    }

    public class UpdateOrderStatusCommandHandler(
        IUnitOfWork unitOfWork
        ) : IRequestHandler<UpdateOrderStatusCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var order = await _unitOfWork.Orders.GetByIdAsync(request.Id, cancellationToken);
                order.UpdateOrderStatus(request.OrderStatus);
                if (!string.IsNullOrWhiteSpace(request.Reason))
                {
                    order.UpdateReason(request.Reason);
                }
                _unitOfWork.Orders.Update(order);
                await _unitOfWork.CommitAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
