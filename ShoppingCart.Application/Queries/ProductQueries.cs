using MediatR;

using ShoppingCart.Shared.DTO;

namespace ShoppingCart.Application.Queries;

public record class GetAllProducts : IRequest<Response<IEnumerable<ProductDTO>>> { }
public record class GetProductById(int Id) : IRequest<Response<ProductDTO>> { }