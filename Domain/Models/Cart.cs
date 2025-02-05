using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Cart
    {
        public Guid CartId { get; set; }
        public string? SessionId { get; set; }
        public virtual List<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
        public Guid? UserId { get; set; } 
        public HoneyShopUser User { get; set; } 
    }
}
