using System.Collections.Generic;

namespace OrderDB.Models
{
    public class Order
    {
        public int Id { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string OrderStatusText { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }

    public enum OrderStatus
    {
        Created = 1,
        Approved = 2,
        Failed = 3,
        Cancelled = 4
    }
}