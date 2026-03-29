namespace ProductService.Domain.Events
{
    public class ProductUpdatedEvent
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}