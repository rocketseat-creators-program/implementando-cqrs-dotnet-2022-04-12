using MediatR;

using ShoppingCart.Application.Commands;
using ShoppingCart.Application.Notifications;
using ShoppingCart.Application.Queries;
using ShoppingCart.Infrastructure.Data;
using ShoppingCart.Shared.DTO;

namespace ShoppingCart.Application.Handlers;

public class UserBasketHandlers : IRequestHandler<CreateUserBasket, Response<Guid>>,
    IRequestHandler<UpdateUserBasketItem, Response>,
    IRequestHandler<AddItemToUserBasket, Response>,
    IRequestHandler<GetUserBasketById, Response<UserBasketDTO>>,
    INotificationHandler<PriceUpdated>
{
    private readonly IUserBasketRepository _repository;

    public UserBasketHandlers(IUserBasketRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<Guid>> Handle(CreateUserBasket request, CancellationToken cancellationToken)
    {
        return new Response<Guid>()
        {
            Success = true,
            Result = await _repository.CreateBasketAsync()
        };
    }

    public async Task<Response> Handle(UpdateUserBasketItem request, CancellationToken cancellationToken)
    {
        return new Response()
        {
            Success = await _repository.UpdateBasketItemAsync(request.Item)
        };
    }

    public async Task<Response> Handle(AddItemToUserBasket request, CancellationToken cancellationToken)
    {
        return new Response()
        {
            Success = await _repository.AddItemToBasketAsync(request.Item)
        };
    }

    public async Task<Response<UserBasketDTO>> Handle(GetUserBasketById request, CancellationToken cancellationToken)
    {
        return new Response<UserBasketDTO>()
        {
            Success = true,
            Result = await _repository.GetBasketByIdAsync(request.Id)
        };
    }

    public async Task Handle(PriceUpdated notification, CancellationToken cancellationToken)
    {
        await _repository.UpdateProductPrice(notification.ProductId, notification.NewPrice);
    }
}
