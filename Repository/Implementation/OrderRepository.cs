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
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            try
            {
                _context.Orders.Add(order); // Add the order
                await _context.SaveChangesAsync(); // Save the changes to the database
                return order; // Return the created order
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Error in CreateOrderAsync: {ex.Message}");
                throw; // Rethrow the exception to be caught in the calling method
            }// Return the created order
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders.Include(o => o.User)  // Include user details if needed
                                    .ToListAsync();
        }


        public async Task<Order> GetOrderByIdAsync(Guid id) =>
             await _context.Orders.Include(o=>o.User).Include(o => o.OrderProducts).ThenInclude(oi => oi.Product)
                 .FirstOrDefaultAsync(o => o.OrderId == id);

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(Guid userId) =>
           await _context.Orders.Include(o => o.OrderProducts).ThenInclude(op => op.Product)
               .Where(o => o.UserId == userId).ToListAsync();
    }
}
