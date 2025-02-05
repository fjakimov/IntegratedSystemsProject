using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class CartItemDTO
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Stock { get; set; }
    }
}
