using MediatR;

namespace ShoppingCart.Application;

public interface IResponse
{
    bool Success { get; init; }
    IDictionary<string, string[]> Errors { get; init; }
}

public record class Response<T> : IResponse
{
    public bool Success { get; init; }
    public IDictionary<string, string[]> Errors { get; init; } = new Dictionary<string, string[]>();
    public T Result { get; init; }
}

public record class Response : Response<Unit> { }