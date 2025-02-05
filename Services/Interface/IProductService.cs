using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IProductService
    {
        Task<List<Product>> getAllProducts();
        Task<Product?> findProductByName(string productName);
        Task<Product?> findProductById(Guid productId);
        Task CreateNewProduct(Product product, Guid userId);
        Task UpdateProduct(Product product, Guid userId);
        Task DeleteProduct(Guid productId, Guid userId);
    }
}
