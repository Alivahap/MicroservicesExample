using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using AuthService.API.Data;


public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=localhost\\SQLEXPRESS;Database=AuthDb;Trusted_Connection=True;TrustServerCertificate=True;");

        return new AuthDbContext(optionsBuilder.Options);
    }
}