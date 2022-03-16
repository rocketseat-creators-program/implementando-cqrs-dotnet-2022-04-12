
using MediatR;

namespace ShoppingCart.Application.Notifications;

public record class PriceUpdated : INotification
{
    public int ProductId { get; init; }
    public decimal NewPrice { get; init; }
}