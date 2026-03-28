using MediatR;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using ProductService.Application.Events; 
using ProductService.Domain.Events;
namespace ProductService.Application.Features.Products.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _repository;
        private readonly IEventPublisher _eventPublisher;

        public CreateProductCommandHandler(IProductRepository repository, IEventPublisher eventPublisher)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
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