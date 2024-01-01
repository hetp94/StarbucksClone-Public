using StarbucksModels.DbModels;

namespace StarbucksModels.ViewModels
{
    public class OrderVM
    {
        public OrderHeader orderHeader { get; set; }

        public List<OrderDetail> orderDetails { get; set; }
    }
}
