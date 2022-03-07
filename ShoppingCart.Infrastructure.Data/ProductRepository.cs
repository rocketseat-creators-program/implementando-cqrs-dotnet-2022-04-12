using Microsoft.EntityFrameworkCore;

using ShoppingCart.Domain;
using ShoppingCart.Shared.DTO;

namespace ShoppingCart.Infrastructure.Data;

public interface IProductRepository
{
    Task<int> CreateProductAsync(ProductDTO product);
    Task<bool> DeleteProductAsync(int id);
    Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
    Task<ProductDTO> GetProductByIdAsync(int id);
    Task<bool> UpdateProductAsync(ProductDTO product);
}

public class ProductRepository : IProductRepository
{
    private readonly ShoppingCartContext _context;

    public ProductRepository(ShoppingCartContext context)
    {
        _context = context;
    }

    public async Task<ProductDTO> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .AsNoTracking()
            .Select(x => new ProductDTO()
            {
                Id = x.Id,
                Name = x.Name,
                UnitPrice = x.UnitPrice
            })
            .SingleAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
    {
        return await _context.Products
.AsNoTracking()
.Select(x => new ProductDTO()
{
    Id = x.Id,
    Name = x.Name,
    UnitPrice = x.UnitPrice
})
.ToListAsync();
    }

    public async Task<int> CreateProductAsync(ProductDTO product)
    {
        Product? model = new Product()
        {
            Name = product.Name.Trim(),
            UnitPrice = product.UnitPrice
        };

        _context.Products.Add(model);

        await _context.SaveChangesAsync();

        return _context.Entry(model).Entity.Id;
    }

    public async Task<bool> UpdateProductAsync(ProductDTO product)
    {
        Product? model = await _context.Products.FindAsync(product.Id);

        if (model is null)
        {
            return false;
        }

        model.Name = product.Name;
        product.UnitPrice = product.UnitPrice;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        Product? model = await _context.Products.FindAsync(id);

        if (model is null)
        {
            return false;
        }

        _context.Products.Remove(model);

        await _context.SaveChangesAsync();

        return true;
    }
}