using System;
using System.Collections.Generic;

namespace Revision.Orders.Functions.Models
{
    public class PurchaseOrder
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public DeliveryDetails DeliveryDetails { get; set; }
    }
}