using FluentValidation;

using ShoppingCart.Application.Commands;

namespace ShoppingCart.Api.Validators;

public class UpdateProductValidator : AbstractValidator<UpdateProduct>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Product).SetValidator(new ProductValidator());
    }
}
