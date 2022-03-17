
using ShoppingCart.Domain;
using ShoppingCart.Shared.DTO;

namespace ShoppingCart.Infrastructure.Data;

public class DocumentUserBasketRepository : IUserBasketRepository
{
    private readonly UserBasketContext _context;

    public DocumentUserBasketRepository(UserBasketContext context)
    {
        _context = context;
    }

    public Task<bool> AddItemToBasketAsync(UserBasketItemDTO item)
    {
        UserBasket? basket = _context.Baskets.FindOne(x => x.Id == item.BasketId);

        if (basket is null)
        {
            return Task.FromResult(false);
        }

        basket.Items ??= new List<UserBasketItem>();

        if (!basket.Items.Any(x => x.Product.Id == item.Product.Id))
        {
            basket.Items.Add(new UserBasketItem()
            {
                Quantity = item.Quantity,
                Product = new Product()
                {
                    Id = item.Product.Id,
                    Name = item.Product.Name,
                    UnitPrice = item.Product.UnitPrice
                }
            });
        }
        else
        {
            basket.Items.Single(x => x.Product.Id == item.Product.Id).Quantity += item.Quantity;
        }

        _context.Baskets.Update(basket);

        return Task.FromResult(true);
    }

    public Task<Guid> CreateBasketAsync()
    {
        UserBasket? basket = new UserBasket() { Id = Guid.NewGuid() };

        _context.Baskets.Insert(basket);

        return Task.FromResult(basket.Id);
    }

    public Task<UserBasketDTO> GetBasketByIdAsync(Guid id)
    {
        UserBasket? basket = _context.Baskets.FindOne(x => x.Id == id);

        return Task.FromResult(new UserBasketDTO()
        {
            Id = basket.Id,
            Items = basket.Items.Select(x => new UserBasketItemDTO()
            {
                Id = x.Id,
                BasketId = basket.Id,
                Quantity = x.Quantity,
                Product = new ProductDTO()
                {
                    Id = x.Product.Id,
                    Name = x.Product.Name,
                    UnitPrice = x.Product.UnitPrice,
                },
                TotalPrice = x.TotalPrice ?? 0
            }),
            TotalItems = basket.TotalItems,
            TotalPrice = basket.TotalPrice ?? 0
        });
    }

    public Task<bool> UpdateBasketItemAsync(UserBasketItemDTO item)
    {
        UserBasket? basket = _context.Baskets.FindOne(x => x.Id == item.BasketId);

        if (basket is null)
        {
            return Task.FromResult(false);
        }

        basket.Items ??= new List<UserBasketItem>();

        if (item.Quantity > 0)
        {
            basket.Items.Single(x => x.Product.Id == item.Product.Id).Quantity = item.Quantity;
            basket.Items.Single(x => x.Product.Id == item.Product.Id).Product.UnitPrice = item.Product.UnitPrice;
            basket.Items.Single(x => x.Product.Id == item.Product.Id).Product.Name = item.Product.Name;
        }
        else
        {
            basket.Items.Remove(basket.Items.Single(x => x.Product.Id == item.Product.Id));
        }

        _context.Baskets.Update(basket);

        return Task.FromResult(true);
    }

    public async Task<bool> UpdateProductPrice(int productId, decimal newPrice)
    {
        IEnumerable<UserBasket>? baskets = _context.Baskets.FindAll();
        foreach (UserBasket? basket in baskets)
        {
            if (basket.Items?.Any(y => y.Product?.Id == productId) ?? false)
            {
                UserBasketItem? item = basket.Items.Single(x => x.Product.Id == productId);
                await UpdateBasketItemAsync(new UserBasketItemDTO()
                {
                    BasketId = basket.Id,
                    Quantity = item.Quantity,
                    Product = new ProductDTO()
                    {
                        Id = productId,
                        Name = item.Product.Name,
                        UnitPrice = newPrice
                    }
                });
            }
        }

        return true;
    }
}