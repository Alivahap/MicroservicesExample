using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using ProductService.API.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ProductDbContext _context;

    public AuthController(IConfiguration config, ProductDbContext context)
    {
        _config = config;
        _context = context;
    }

  [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest request)
{
    // Kullanıcıyı veritabanından bul
    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
    if (user == null)
        return Unauthorized("Kullanıcı bulunamadı");

    // Şifre doğrulama 
    var hash = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(request.Password)));
    if (user.PasswordHash != hash)
        return Unauthorized("Şifre yanlış");

    var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

    var claims = new[]
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Role, user.Role)
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddMinutes(15),
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    var jwtToken = tokenHandler.WriteToken(token);

    var refreshToken = new RefreshToken
    {
        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
        Expires = DateTime.UtcNow.AddDays(7),
        UserName = user.UserName,
        Role = user.Role
    };

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
            return Unauthorized();

        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, token.UserName),
            new Claim(ClaimTypes.Role, token.Role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
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