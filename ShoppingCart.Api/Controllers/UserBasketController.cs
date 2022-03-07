using Microsoft.AspNetCore.Mvc;

using ShoppingCart.Infrastructure.Data;
using ShoppingCart.Shared.DTO;

using static System.Net.Mime.MediaTypeNames;

namespace ShoppingCart.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes(Application.Json)]
[Produces(Application.Json)]
public class UserBasketController : ControllerBase
{
    private readonly IUserBasketRepository _repository;

    public UserBasketController(IUserBasketRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{id:guid}", Name = nameof(GetUserBasketById))]
    [ProducesResponseType(typeof(UserBasketDTO), 200)]
    public async Task<IActionResult> GetUserBasketById(Guid id)
    {
        return Ok(await _repository.GetBasketByIdAsync(id));
    }

    [HttpPost(Name = nameof(Created))]
    [ProducesResponseType(typeof(Guid), 200)]
    public async Task<IActionResult> CreateUserBasket()
    {
        return Ok(await _repository.CreateBasketAsync());
    }


    [HttpPost("item", Name = nameof(AddUserBasketItem))]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AddUserBasketItem(UserBasketItemDTO item)
    {
        bool success = await _repository.AddItemToBasketAsync(item);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPut("item", Name = nameof(UpdateUserBasketItem))]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateUserBasketItem(UserBasketItemDTO item)
    {
        bool success = await _repository.UpdateBasketItemAsync(item);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}
