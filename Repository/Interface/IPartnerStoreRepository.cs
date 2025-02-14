using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IPartnerStoreRepository
    {
        Task<List<PartnerStore>> GetPartnerStoresAsync();
        Task<int> GetTotalOrdersAsync();
    }
}
