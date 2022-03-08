using MediatR;

using ShoppingCart.Shared.DTO;

namespace ShoppingCart.Application.Commands;

public record class CreateUserBasket : IRequest<Response<Guid>> { }
public record class AddItemToUserBasket(UserBasketItemDTO Item) : IRequest<Response> { }
public record class UpdateUserBasketItem(UserBasketItemDTO Item) : IRequest<Response> { }