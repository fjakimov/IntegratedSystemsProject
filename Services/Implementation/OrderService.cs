using Domain.DTO;
using Domain.Models;
using Repository.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IUserRepository userRepository;
        private readonly IOrderRepository ordersRepository;
        public OrderService(IOrderRepository orderRepository, IShoppingCartRepository shoppingCartRepository, IUserRepository userRepository, IOrderRepository ordersRepository)
        {
            this.orderRepository = orderRepository;
            this.shoppingCartRepository = shoppingCartRepository;
            this.userRepository = userRepository;
            this.ordersRepository = ordersRepository;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await orderRepository.GetAllOrders();
        }

        public async Task<Order> GetOrderById(Guid id)
        {
            return await orderRepository.GetOrderByIdAsync(id);
        }

        public async Task<bool> Order(Guid userId, string status)
        {
            var cart = await shoppingCartRepository.GetCartByUserIdAsync(userId);
            var user = userRepository.FindById(userId);
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = userId,
                User = user,
                OrderDate = DateTime.UtcNow,
                TotalPrice = cart.CartProducts.Sum(cp => cp.Quantity * cp.Product.Price),
                OrderProducts = cart.CartProducts.Select(cp => new OrderProduct
                {
                    ProductId = cp.ProductId,
                    Quantity = cp.Quantity
                }).ToList(),
                Status = status
            };
            await orderRepository.CreateOrderAsync(order);
            cart.CartProducts.Clear();
            await shoppingCartRepository.UpdateCart(cart);

            return true;
        }
    }
}
