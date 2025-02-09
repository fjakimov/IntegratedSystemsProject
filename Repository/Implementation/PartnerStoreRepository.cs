using Domain.DTO;
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
    public class PartnerStoreRepository : IPartnerStoreRepository
    {
        private readonly PartnerDbContext _context;
        public PartnerStoreRepository(PartnerDbContext context )
        {
            _context = context;
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<List<PartnerStore>> GetPartnerStoresAsync()
        {
            return await _context.Books.ToListAsync();
        }

        
    }
}
