using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class PartnerStoreViewModel
    {
        public List<BookViewModel> Books {  get; set; }
        public int TotalOrders { get; set; }    
    }
}
