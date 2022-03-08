namespace ShoppingCart.Domain;

public class UserBasket
{
    public Guid Id { get; set; }
    public ICollection<UserBasketItem> Items { get; set; }

    public int TotalItems => Items?.Sum(x => x.Quantity) ?? 0;
    public decimal? TotalPrice => Items?.Sum(x => x.TotalPrice);
}