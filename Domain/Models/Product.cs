namespace Domain.Models
{
    public class Product    
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

        public List<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
    }
}
