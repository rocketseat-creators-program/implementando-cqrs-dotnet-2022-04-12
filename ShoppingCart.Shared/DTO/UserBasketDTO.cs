namespace ShoppingCart.Shared.DTO;

public record class UserBasketDTO
{
    public Guid Id { get; init; }
    public IEnumerable<UserBasketItemDTO> Items { get; init; }
    public int TotalItems { get; init; }
    public decimal TotalPrice { get; init; }
}
