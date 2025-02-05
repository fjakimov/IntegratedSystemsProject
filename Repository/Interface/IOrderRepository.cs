using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetUserOrdersAsync(Guid userId);
        Task<List<Order>> GetAllOrders();
    }
}
