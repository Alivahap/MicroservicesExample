using System;

namespace ProductService.Domain.Events
{
    public class ProductCreatedEvent
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}