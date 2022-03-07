namespace ShoppingCart.Shared.DTO;

public record class UserBasketItemDTO
{
    public int Id { get; init; }
    public ProductDTO Product { get; init; }
    public int Quantity { get; init; }
    public decimal TotalPrice { get; init; }
    public Guid BasketId { get; init; }
}
