using MediatR;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace ProductService.Application.Features.Products.Queries
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<Product>>
    {
        private readonly IProductRepository _repository;
        private readonly IDistributedCache _cache;

        public GetProductsQueryHandler(IProductRepository repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<List<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "products_list";
            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<List<Product>>(cachedData)!;
            }

            var products = await _repository.GetAllAsync();

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // 5 dk cache
            };

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(products), cacheOptions);

            return products;
        }
    }
}