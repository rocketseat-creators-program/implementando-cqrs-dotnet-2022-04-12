using FluentValidation;

using ShoppingCart.Application.Commands;

namespace ShoppingCart.Api.Validators;

public class CreateProductValidator : AbstractValidator<CreateProduct>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Product).SetValidator(new ProductValidator());
    }
}
