using MediatR;

using ShoppingCart.Shared.DTO;

namespace ShoppingCart.Application.Commands;

public record class CreateProduct(ProductDTO Product) : IRequest<Response<int>?> { }
public record class UpdateProduct(ProductDTO Product) : IRequest<Response> { }
public record class DeleteProduct(int Id) : IRequest<Response> { }
