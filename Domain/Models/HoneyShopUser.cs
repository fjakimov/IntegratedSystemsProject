using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class HoneyShopUser
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime? DateBirth { get; set; }  // Make DateBirth nullable
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; } = "User";
        public Cart Cart { get; set; }
        public List<Order> Orders { get; set; }
    }
}
