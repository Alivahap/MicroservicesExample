namespace ProductService.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string UserName { get; set; } = null!;
        public bool IsExpired => DateTime.UtcNow >= Expires;
		public string Role { get; set; }
		
    }
}