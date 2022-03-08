using MediatR;

using ShoppingCart.Application.Commands;
using ShoppingCart.Application.Queries;
using ShoppingCart.Infrastructure.Data;
using ShoppingCart.Shared.DTO;

namespace ShoppingCart.Application.Handlers;

public class ProductHandlers : IRequestHandler<GetAllProducts, Response<IEnumerable<ProductDTO>>>,
    IRequestHandler<GetProductById, Response<ProductDTO>>,
    IRequestHandler<CreateProduct, Response<int>>,
    IRequestHandler<UpdateProduct, Response>,
    IRequestHandler<DeleteProduct, Response>
{
    private readonly IProductRepository _repository;

    public ProductHandlers(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<IEnumerable<ProductDTO>>> Handle(GetAllProducts request, CancellationToken cancellationToken)
    {
        return new Response<IEnumerable<ProductDTO>>()
        {
            Success = true,
            Result = await _repository.GetAllProductsAsync()
        };
    }

    public async Task<Response<ProductDTO>> Handle(GetProductById request, CancellationToken cancellationToken)
    {
        return new Response<ProductDTO>()
        {
            Success = true,
            Result = await _repository.GetProductByIdAsync(request.Id)
        };
    }


    public async Task<Response<int>> Handle(CreateProduct request, CancellationToken cancellationToken)
    {
        return new Response<int>()
        {
            Success = true,
            Result = await _repository.CreateProductAsync(request.Product)
        };
    }

    public async Task<Response> Handle(UpdateProduct request, CancellationToken cancellationToken)
    {
        return new Response()
        {
            Success = await _repository.UpdateProductAsync(request.Product)
        };
    }

    public async Task<Response> Handle(DeleteProduct request, CancellationToken cancellationToken)
    {
        return new Response()
        {
            Success = await _repository.DeleteProductAsync(request.Id)
        };
    }
}