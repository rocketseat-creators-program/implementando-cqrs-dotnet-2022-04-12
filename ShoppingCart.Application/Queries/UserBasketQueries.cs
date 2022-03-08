using MediatR;

using ShoppingCart.Shared.DTO;

namespace ShoppingCart.Application.Queries;

public record class GetUserBasketById(Guid Id) : IRequest<Response<UserBasketDTO>> { }