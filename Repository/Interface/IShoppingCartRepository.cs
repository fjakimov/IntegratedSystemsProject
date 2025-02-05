using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IShoppingCartRepository
    {
        Task<Cart> GetCartByUserIdAsync(Guid userId);
        Task AddItemToCartAsync(Guid userId, CartProduct cartProduct);
        Task UpdateCartItemAsync(Guid userId, CartProduct cartProduct);
        Task UpdateCart(Cart cart);
    }
}
