using Microsoft.EntityFrameworkCore;

using ShoppingCart.Domain;
using ShoppingCart.Shared.DTO;

namespace ShoppingCart.Infrastructure.Data;

public class UserBasketRepository : IUserBasketRepository
{
    private readonly ShoppingCartContext _context;

    public UserBasketRepository(ShoppingCartContext context)
    {
        _context = context;
    }

    public async Task<UserBasketDTO> GetBasketByIdAsync(Guid id)
    {
        return await _context.Baskets
            .AsNoTracking()
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .Select(x => new UserBasketDTO()
            {
                Id = x.Id,
                Items = x.Items.Select(y => new UserBasketItemDTO()
                {
                    Id = y.Id,
                    BasketId = id,
                    Quantity = y.Quantity,
                    Product = new ProductDTO()
                    {
                        Id = y.Product.Id,
                        Name = y.Product.Name,
                        UnitPrice = y.Product.UnitPrice
                    },
                    TotalPrice = y.TotalPrice ?? 0
                }),
                TotalItems = x.TotalItems,
                TotalPrice = x.TotalPrice ?? 0,
            })
            .SingleAsync(x => x.Id == id);
    }

    public async Task<Guid> CreateBasketAsync()
    {
        UserBasket model = new UserBasket()
        {
            Id = Guid.NewGuid()
        };

        _context.Baskets.Add(model);

        await _context.SaveChangesAsync();

        return model.Id;
    }

    public async Task<bool> AddItemToBasketAsync(UserBasketItemDTO item)
    {
        Domain.UserBasket? model = await _context.Baskets
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .SingleAsync(x => x.Id == item.BasketId);

        if (model is null)
        {
            return false;
        }

        if (model.Items.Any(x => x.Product.Id == item.Product.Id))
        {
            UserBasketItem? itemModel = model.Items.Single(x => x.Product.Id == item.Product.Id);
            itemModel.Quantity = itemModel.Quantity + item.Quantity;
        }
        else
        {
            model.Items.Add(new UserBasketItem()
            {
                Product = await _context.Products.FindAsync(item.Product.Id),
                Quantity = item.Quantity
            });
        }

        await _context.SaveChangesAsync();

        return true;
    }


    public async Task<bool> UpdateBasketItemAsync(UserBasketItemDTO item)
    {
        UserBasket? model = await _context.Baskets
            .Include(x => x.Items)
            .SingleAsync(x => x.Id == item.BasketId);

        if (model is null)
        {
            return false;
        }

        UserBasketItem? itemModel = model.Items.Single(x => x.Id == item.Id);
        if (item.Quantity == 0)
        {
            model.Items.Remove(itemModel);
        }
        else
        {
            itemModel.Quantity = item.Quantity;
        }

        await _context.SaveChangesAsync();

        return true;
    }
}
