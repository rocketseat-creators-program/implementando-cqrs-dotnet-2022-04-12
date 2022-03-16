using MediatR;

using Microsoft.AspNetCore.Mvc;

using ShoppingCart.Application;
using ShoppingCart.Application.Commands;
using ShoppingCart.Application.Queries;
using ShoppingCart.Shared.DTO;

namespace ShoppingCart.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes(System.Net.Mime.MediaTypeNames.Application.Json)]
[Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(Name = nameof(GetAllProducts))]
    [ProducesResponseType(typeof(IEnumerable<ProductDTO>), 200)]
    public async Task<IActionResult> GetAllProducts()
    {
        return Ok(await _mediator.Send(new GetAllProducts()));
    }

    [HttpGet("{id:int}", Name = nameof(GetProductById))]
    [ProducesResponseType(typeof(ProductDTO), 200)]
    public async Task<IActionResult> GetProductById(int id)
    {
        return Ok(await _mediator.Send(new GetProductById(id)));
    }

    [HttpPost(Name = nameof(CreateProduct))]
    [ProducesResponseType(200)]
    public async Task<IActionResult> CreateProduct([FromBody] ProductDTO product)
    {
        Response<int>? response = await _mediator.Send(new CreateProduct(product));

        if (!response.Success)
        {
            return BadRequest(response.Errors);
        }

        return Ok(response.Result);
    }

    [HttpPut(Name = nameof(UpdateProduct))]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateProduct([FromBody] ProductDTO product)
    {
        Response? response = await _mediator.Send(new UpdateProduct(product));

        if (!response.Success)
        {
            return BadRequest(response.Errors);
        }

        return NoContent();
    }

    [HttpDelete("{id:int}", Name = nameof(DeleteProduct))]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        Response? response = await _mediator.Send(new DeleteProduct(id));

        if (!response.Success)
        {
            return NotFound();
        }

        return NoContent();
    }

}