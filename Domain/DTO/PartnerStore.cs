using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class PartnerStore
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string BookCover { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public Guid PublisherId { get; set; }
        public string Genre {  get; set; }
        public int QuantityAvaiable { get; set; }
        public double Price { get; set; }
        public Guid AuthorId { get; set; }

    }
}
