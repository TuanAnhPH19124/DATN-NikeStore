namespace Webapi.Models
{
    public class ProductRatingStatistics
    {
        public string ProductId { get; set; }
        public int TotalVotes { get; set; }
        public int TotalRating { get; set; }
        public double AverageRating { get; set; }
    }
}
