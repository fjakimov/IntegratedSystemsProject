using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class PartnerOrder
    {
        public Guid Id { get; set; }
        public string OwnerId { get; set; }
    }
}
