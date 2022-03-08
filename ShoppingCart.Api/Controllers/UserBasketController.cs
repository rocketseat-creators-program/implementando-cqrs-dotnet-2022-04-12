using MediatR;

using Microsoft.AspNetCore.Mvc;

using ShoppingCart.Application.Commands;
using ShoppingCart.Application.Queries;
using ShoppingCart.Shared.DTO;

namespace ShoppingCart.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes(System.Net.Mime.MediaTypeNames.Application.Json)]
[Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
public class UserBasketController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserBasketController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}", Name = nameof(GetUserBasketById))]
    [ProducesResponseType(typeof(UserBasketDTO), 200)]
    public async Task<IActionResult> GetUserBasketById(Guid id)
    {
        return Ok(await _mediator.Send(new GetUserBasketById(id)));
    }

    [HttpPost(Name = nameof(Created))]
    [ProducesResponseType(typeof(Guid), 200)]
    public async Task<IActionResult> CreateUserBasket()
    {
        return Ok(await _mediator.Send(new CreateUserBasket()));
    }


    [HttpPost("item", Name = nameof(AddUserBasketItem))]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AddUserBasketItem(UserBasketItemDTO item)
    {
        Application.Response? response = await _mediator.Send(new AddItemToUserBasket(item));

        if (!response.Success)
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
        Application.Response? response = await _mediator.Send(new UpdateUserBasketItem(item));

        if (!response.Success)
        {
            return NotFound();
        }

        return NoContent();
    }
}
