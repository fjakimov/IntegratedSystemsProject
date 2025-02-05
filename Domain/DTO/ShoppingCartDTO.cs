using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ShoppingCartDTO
    {
        public List<CartItemDTO> Items { get; set; }
        public decimal TotalPrice => Items?.Sum(item => item.Price * item.Quantity) ?? 0;
    }
}
