namespace ShoppingCart.Domain;

public class UserBasketItem
{
    public int Id { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }

    public decimal? TotalPrice => Product?.UnitPrice * Quantity;
}
