namespace Domain.Repositories
{
    public class ProductSalesAndRevenueInfo // list thống kê top sản phẩm
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class CustomerOrderInfo
    {
        public string UserId { get; set; }
        public string CustomerName { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}