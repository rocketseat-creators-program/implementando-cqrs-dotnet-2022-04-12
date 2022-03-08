using ShoppingCart.Shared.DTO;

namespace ShoppingCart.Infrastructure.Data;

public interface IUserBasketRepository
{
    Task<bool> AddItemToBasketAsync(UserBasketItemDTO item);
    Task<Guid> CreateBasketAsync();
    Task<UserBasketDTO> GetBasketByIdAsync(Guid id);
    Task<bool> UpdateBasketItemAsync(UserBasketItemDTO item);
}
