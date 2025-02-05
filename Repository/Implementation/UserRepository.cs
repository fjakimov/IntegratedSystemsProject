using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Delete(HoneyShopUser entity)
        {
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<HoneyShopUser> GetAsync(Guid id)
        {
          return await _context.Users
         .Include(u => u.Orders)
         .ThenInclude(o => o.OrderProducts)
         .ThenInclude(p => p.Product)
         .FirstOrDefaultAsync(u => u.Id == id);
        }

        public List<HoneyShopUser> GetAll()
        {
            return _context.Users.ToList();
        }

        public void Insert(HoneyShopUser entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
        }

        public void Update(HoneyShopUser entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
        }
        public HoneyShopUser FindByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public HoneyShopUser FindById(Guid id)
        {
            return _context.Users.First(u => u.Id ==  id);
        }
    }
}
