using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class BookViewModel
    {
        public string Title { get; set; }
        public string BookCover { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }   
        public string Genre {  get; set; }
        public float Price { get; set; }
    }
}
