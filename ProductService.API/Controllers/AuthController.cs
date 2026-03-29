using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ProductService.Domain.Entities; // RefreshToken modeli
using ProductService.Infrastructure.Persistence; // DbContext
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ProductDbContext _context; // refresh token DB için

    public AuthController(IConfiguration config, ProductDbContext context)
    {
        _config = config;
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        // JWT key
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

        // JWT oluştur
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "testuser")
            }),
            Expires = DateTime.UtcNow.AddMinutes(15), // kısa ömürlü JWT
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        // Refresh token oluştur
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddDays(7),
            UserName = "testuser"
        };

        // DB'ye kaydet
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            token = jwtToken,
            refreshToken = refreshToken.Token
        });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == refreshToken);

        if (token == null || token.IsExpired)
            return Unauthorized("Refresh token geçersiz veya süresi dolmuş.");

        // JWT key
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

        // Yeni JWT oluştur
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, token.UserName)
            }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var newToken = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(newToken);

        return Ok(new { token = jwtToken });
    }
}