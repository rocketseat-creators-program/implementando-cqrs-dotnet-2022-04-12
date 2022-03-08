namespace ShoppingCart.Application;

public record class Response
{
    public bool Success { get; init; }
    public IDictionary<string, string[]> Errors { get; init; }
}

public record class Response<T> : Response
{
    public T Result { get; init; }
}
