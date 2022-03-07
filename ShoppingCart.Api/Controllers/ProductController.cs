using Microsoft.AspNetCore.Mvc;

using ShoppingCart.Infrastructure.Data;
using ShoppingCart.Shared.DTO;

using static System.Net.Mime.MediaTypeNames;

namespace ShoppingCart.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes(Application.Json)]
[Produces(Application.Json)]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet(Name = nameof(GetAllProducts))]
    [ProducesResponseType(typeof(IEnumerable<ProductDTO>), 200)]
    public async Task<IActionResult> GetAllProducts()
    {
        return Ok(await _repository.GetAllProductsAsync());
    }

    [HttpGet("{id:int}", Name = nameof(GetProductById))]
    [ProducesResponseType(typeof(ProductDTO), 200)]
    public async Task<IActionResult> GetProductById(int id)
    {
        return Ok(await _repository.GetProductByIdAsync(id));
    }

    [HttpPost(Name = nameof(CreateProduct))]
    [ProducesResponseType(200)]
    public async Task<IActionResult> CreateProduct(ProductDTO product)
    {
        int id = await _repository.CreateProductAsync(product);

        return Ok(id);
    }

    [HttpPut(Name = nameof(UpdateProduct))]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateProduct(ProductDTO product)
    {
        bool success = await _repository.UpdateProductAsync(product);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}", Name = nameof(DeleteProduct))]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        bool success = await _repository.DeleteProductAsync(id);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

}