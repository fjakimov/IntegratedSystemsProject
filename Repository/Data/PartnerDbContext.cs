using Domain.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data
{
    public class PartnerDbContext : DbContext
    {
        public PartnerDbContext(DbContextOptions<PartnerDbContext> options) : base(options) { }
    
        public DbSet<PartnerStore> Books { get; set; }
        public DbSet<PartnerOrder>  Orders { get; set; }
    }
}
