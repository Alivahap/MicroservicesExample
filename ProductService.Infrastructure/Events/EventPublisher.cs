using ProductService.Application.Events;
using ProductService.Domain.Events;
using System;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Events
{
    public class EventPublisher : IEventPublisher
    {
        public Task PublishProductCreatedAsync(ProductCreatedEvent @event)
        {
            // konsola yazdırıyoruz
            Console.WriteLine($"Event Fırlatıldı: ProductCreated | {@event.ProductId} | {@event.Name}");
            return Task.CompletedTask;
        }
    }
}