

namespace Domain.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public HoneyShopUser User { get; set; }
        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }
}
