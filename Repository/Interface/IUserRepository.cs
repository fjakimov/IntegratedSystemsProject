using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IUserRepository
    {
        List<HoneyShopUser> GetAll();
        Task<HoneyShopUser> GetAsync(Guid id);
        void Insert(HoneyShopUser entity);
        void Update(HoneyShopUser entity);
        void Delete(HoneyShopUser entity);
        HoneyShopUser FindByEmail(string email);
        HoneyShopUser FindById(Guid id);
    }
}
