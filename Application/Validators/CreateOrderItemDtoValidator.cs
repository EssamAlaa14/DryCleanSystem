using Application.Common.DTOs.CreateDtos;
using FluentValidation;

namespace Application.Validators
{
    public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
    {
        public CreateOrderItemDtoValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.SingleQuantityPrice)
                .GreaterThan(0)
                .WithMessage("Single quantity price must be greater than 0.");

            RuleFor(x => x.Color)
                .NotEmpty()
                .WithMessage("Color is required.");

            RuleFor(x => x.CleaningTypeIds)
                .NotEmpty()
                .WithMessage("At least one cleaning type is required.");
        }
    }

}
