using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Guid id);
    }
}
