using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IUserService
    {
        HoneyShopUser GetUserByEmail(string email);
        Boolean Login(string email, string password);
        Boolean Register(string email, string fullname, string password, string phone, string confirmPassword);
        Task<HoneyShopUser> GetUserById(Guid id);
    }
}
