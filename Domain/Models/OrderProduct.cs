

namespace Domain.Models
{
    public class OrderProduct
    {
        public Guid ProductId { get; set; } 
        public Product Product { get; set; }
        public Guid OrderId { get; set; }

        public Order Order { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }
        public decimal Discount { get; set; } 
    }
}
