using Application.Common.DTOs.CreateDtos;
using FluentValidation;

namespace Application.Validators
{
    public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
    {
        public CreateOrderItemDtoValidator()
        {
            RuleFor(x => x.PieceId)
                .NotNull()
                .NotEmpty()
                .WithMessage("The specified piece does not exist.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.SingleQuantityPrice)
                .GreaterThan(0)
                .WithMessage("Single quantity price must be greater than 0.");
        }
    }
}
