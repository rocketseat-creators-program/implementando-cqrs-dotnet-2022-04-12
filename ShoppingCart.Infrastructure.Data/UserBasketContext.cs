using LiteDB;

using ShoppingCart.Domain;

namespace ShoppingCart.Infrastructure.Data
{
    public class UserBasketContext
    {
        private readonly ILiteDatabase _database;

        public UserBasketContext(ILiteDatabase database)
        {
            _database = database;

            database.GetCollection<UserBasket>().EnsureIndex(x => x.Id);
        }

        public ILiteCollection<UserBasket> Baskets => _database.GetCollection<UserBasket>();
    }
}
