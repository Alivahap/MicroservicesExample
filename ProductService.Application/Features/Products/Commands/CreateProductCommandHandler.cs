using MediatR;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using ProductService.Application.Events;
using ProductService.Domain.Events;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace ProductService.Application.Features.Products.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _repository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IDistributedCache _cache;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(
            IProductRepository repository,
            IEventPublisher eventPublisher,
            IDistributedCache cache,
            ILogger<CreateProductCommandHandler> logger)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(product);

            // Cache temizleme
            await _cache.RemoveAsync("products_list", cancellationToken);

            // ✅ Logger kullanımı
            _logger.LogInformation("Ürün oluşturuldu: {ProductId}, Name: {Name}, Price: {Price}", 
                product.Id, product.Name, product.Price);

            // Event fırlatma
            var productCreatedEvent = new ProductCreatedEvent
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CreatedAt = product.CreatedAt
            };

            await _eventPublisher.PublishProductCreatedAsync(productCreatedEvent);

            return product.Id;
        }
    }
}