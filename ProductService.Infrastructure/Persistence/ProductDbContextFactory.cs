using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProductService.Infrastructure.Persistence
{
    public class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
    {
        public ProductDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=localhost\\SQLEXPRESS;Database=ProductServiceDb;Trusted_Connection=True;TrustServerCertificate=True;");

            return new ProductDbContext(optionsBuilder.Options);
        }
    }
}