using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly AppDbContext _appDbContext;
        public ShoppingCartRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task AddItemToCartAsync(Guid userId, CartProduct cartProduct)
        {
            var cart = await _appDbContext.Carts
       .Include(c => c.CartProducts)
       .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartProducts = new List<CartProduct>()
                };
                _appDbContext.Carts.Add(cart);
            }

            var existingItem = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == cartProduct.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += cartProduct.Quantity;
            }
            else
            {
                cart.CartProducts.Add(new CartProduct
                {
                    ProductId = cartProduct.ProductId,
                    Quantity = cartProduct.Quantity
                });
            }

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Cart> GetCartByUserIdAsync(Guid userId)
        {
            return await _appDbContext.Carts
                .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product) // Include related Product data
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task UpdateCart(Cart cart)
        {
            _appDbContext.Update(cart);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(Guid userId, CartProduct cartProduct)
        {
            var cart = await GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                var existingItem = cart.CartProducts.FirstOrDefault(item => item.ProductId == cartProduct.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity = cartProduct.Quantity;
                    await _appDbContext.SaveChangesAsync();
                }
            }
        }
    }
}
