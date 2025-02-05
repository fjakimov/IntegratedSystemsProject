using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IOrderService
    {
        public Task<bool> Order(Guid userId, string status);
        public Task<List<Order>> GetAllOrders();
        public Task<Order> GetOrderById(Guid id);
    }
}
