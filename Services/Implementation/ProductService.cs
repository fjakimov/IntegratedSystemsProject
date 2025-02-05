using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public ProductService(IProductRepository productRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<List<Product>> getAllProducts()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product?> findProductByName(string productName)
        {
            var products = await _productRepository.GetAllAsync();
            return products.FirstOrDefault(p => p.Name.Equals(productName, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Product?> findProductById(Guid productId)
        {
            return await _productRepository.GetByIdAsync(productId);
        }

        public async Task CreateNewProduct(Product product, Guid userId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null || user.Role != "Admin")
            {
                throw new UnauthorizedAccessException("You are not authorized to create products.");
            }

            await _productRepository.AddAsync(product);
        }

        public async Task UpdateProduct(Product product, Guid userId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null || user.Role != "Admin")
            {
                throw new UnauthorizedAccessException("You are not authorized to update products.");
            }

            var existingProduct = await _productRepository.GetByIdAsync(product.Id);
            if (existingProduct == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;

            await _productRepository.UpdateAsync(existingProduct);
        }

        public async Task DeleteProduct(Guid productId, Guid userId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null || user.Role != "Admin")
            {
                throw new UnauthorizedAccessException("You are not authorized to delete products.");
            }

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            await _productRepository.DeleteAsync(product.Id);
        }
    }
}
