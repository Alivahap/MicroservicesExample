namespace ProductService.API.Models
{
    public class LoginRequest
    {
        public string UserName { get; set; } = "testuser";
        public string Password { get; set; } = "any";
    }
}