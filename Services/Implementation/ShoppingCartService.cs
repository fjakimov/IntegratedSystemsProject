using Domain.DTO;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Interface;
using Services.Interface;

namespace Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartService(AppDbContext context, IProductRepository productRepository, IUserRepository userRepository, IOrderRepository orderRepository, IShoppingCartRepository shoppingCartRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task AddProductToShoppingCart(Guid userId, AddToCartDTO addToCartDto)
        {
            var product = await _productRepository.GetByIdAsync(addToCartDto.SelectedProductId);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            // Create a new CartProduct object
            var cartProduct = new CartProduct
            {
                ProductId = product.Id,
                Quantity = addToCartDto.Quantity
            };

            // Delegate to the repository for handling cart updates
            await _shoppingCartRepository.AddItemToCartAsync(userId, cartProduct);
        }


        public async Task<ShoppingCartDTO> GetCartBySessionId(string sessionId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
                .FirstOrDefaultAsync(c => c.SessionId == sessionId);

            if (cart == null)
            {
                return new ShoppingCartDTO { Items = new List<CartItemDTO>() };
            }

            return new ShoppingCartDTO
            {
                Items = cart.CartProducts.Select(cp => new CartItemDTO
                {
                    ProductId = cp.ProductId,
                    ProductName = cp.Product.Name,
                    Price = cp.Product.Price,
                    Quantity = cp.Quantity
                }).ToList()
            };
        }
        public async Task UpdateCartItemQuantity(Guid userId, Guid productId, int quantity)
        {
            var cart = await _context.Carts
                .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);


            if (cart == null) return;

            var cartProduct = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == productId);

            if (cartProduct != null)
            {
                if (cartProduct.Product.Stock < quantity)
                {
                    throw new Exception($"Insufficient stock for {cartProduct.Product.Name}. Only {cartProduct.Product.Stock} items available.");
                }

                cartProduct.Quantity = quantity;

                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveProductFromShoppingCart(Guid userId, CartItemDTO cartItemDTO)
        {
            var cart = await _context.Carts
                .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);


            var productToRemove = cart.CartProducts
                .FirstOrDefault(cp => cp.ProductId == cartItemDTO.ProductId);

            if (productToRemove != null)
            {
                cart.CartProducts.Remove(productToRemove);

                await _context.SaveChangesAsync();
            }
        }
        public async Task<ShoppingCartDTO> GetCartByUserId(Guid userId)
        {
            var cart = await _context.Carts
            .Include(c => c.CartProducts)  // Include the products associated with the cart
            .ThenInclude(cp => cp.Product) // Include the product details for each cart item
            .FirstOrDefaultAsync(c => c.UserId == userId);

            // If the cart does not exist, return an empty shopping cart DTO
            if (cart == null)
            {
                return new ShoppingCartDTO { Items = new List<CartItemDTO>() };
            }

            // Map the cart products to CartItemDTO
            var cartItems = cart.CartProducts.Select(cp => new CartItemDTO
            {
                ProductId = cp.ProductId,
                ProductName = cp.Product.Name,
                Quantity = cp.Quantity,
                Price = cp.Product.Price,
                Stock = cp.Product.Stock
            }).ToList();

            // Return the shopping cart DTO with the mapped items
            return new ShoppingCartDTO
            {
                Items = cartItems
            };
        }
    }
}