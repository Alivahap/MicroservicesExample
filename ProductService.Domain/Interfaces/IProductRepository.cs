using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProductService.Domain.Entities;

namespace ProductService.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
    }
}