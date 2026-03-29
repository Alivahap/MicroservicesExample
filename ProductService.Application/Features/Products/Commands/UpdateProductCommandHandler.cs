using MediatR;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Events;
using ProductService.Application.Events;
using Microsoft.Extensions.Caching.Distributed;

namespace ProductService.Application.Features.Products.Commands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _repository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IDistributedCache _cache;

        public UpdateProductCommandHandler(
            IProductRepository repository,
            IEventPublisher eventPublisher,
            IDistributedCache cache)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
            _cache = cache;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null) return false;

            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;

            await _repository.UpdateAsync(product);

            // 🔥 Cache temizleme
            await _cache.RemoveAsync("products_list");

            // 🔥 Event fırlatma
            var updatedEvent = new ProductUpdatedEvent
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                UpdatedAt = DateTime.UtcNow
            };

            await _eventPublisher.PublishProductUpdatedAsync(updatedEvent);

            return true;
        }
    }
}