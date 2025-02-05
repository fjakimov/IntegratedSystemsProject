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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public HoneyShopUser GetUserByEmail(string email)
        {
            return _userRepo.FindByEmail(email);
        }

        public Task<HoneyShopUser> GetUserById(Guid id)
        {
            return _userRepo.GetAsync(id);
        }

        public bool Login(string email, string password)
        {
            var user = _userRepo.FindByEmail(email);
            if (user == null || user.Password != password) 
            {
                return false;
            }
            return true;
        }

        public bool Register(string email, string fullname, string password, string phone, string confirmPassword)
        {
            var userExists = _userRepo.FindByEmail(email);
            if(userExists != null)
            {
                return false;
            }
            if(password != confirmPassword) 
            {
                return false;
            }
            if (userExists == null)
            {
                var newUser = new HoneyShopUser
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    Password = password,
                    FullName = fullname,
                    Phone = phone,
                    Orders = new List<Order>(),
                    Cart = new Cart // Initialize the cart
                    {
                        CartId = Guid.NewGuid(),
                        CartProducts = new List<CartProduct>()
                    }
                };
                _userRepo.Insert(newUser);
            }
            return true;
        }
    }
}
