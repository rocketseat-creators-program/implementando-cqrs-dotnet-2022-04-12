using FluentValidation;

using ShoppingCart.Shared.DTO;

namespace ShoppingCart.Api.Validators
{
    public class ProductValidator : AbstractValidator<ProductDTO>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(3, 255);

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0.00M);
        }
    }
}
