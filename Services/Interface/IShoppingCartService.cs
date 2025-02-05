using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IShoppingCartService
    {
        Task AddProductToShoppingCart(Guid userId, AddToCartDTO addToCartDTO);
        Task<ShoppingCartDTO> GetCartBySessionId(string sessionId);
        Task RemoveProductFromShoppingCart(Guid userId, CartItemDTO cartItemDTO);
        Task UpdateCartItemQuantity(Guid userId, Guid productId, int quantity);
       /*Task<bool> Order(Guid userId);*/
        Task<ShoppingCartDTO> GetCartByUserId(Guid userId);
    }
}
