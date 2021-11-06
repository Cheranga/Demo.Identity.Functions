namespace Demo.Identity.PurchaseOrders.Models
{
    public class OrderItem
    {
        public string OrderId { get; set; }
        public string ProductCode { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}