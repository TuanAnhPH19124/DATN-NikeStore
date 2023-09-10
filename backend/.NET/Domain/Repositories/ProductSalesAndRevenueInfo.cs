namespace Domain.Repositories
{
    public class ProductSalesAndRevenueInfo // list thống kê top sản phẩm
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}