using ProductService.Domain.Events;
using System.Threading.Tasks;

namespace ProductService.Application.Events
{
    public interface IEventPublisher
    {
        Task PublishProductCreatedAsync(ProductCreatedEvent @event);
    }
}