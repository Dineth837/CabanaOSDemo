namespace CabanaOSDemo.Models
{
    public class RevenueRecord
    {
        public string Month { get; set; } = string.Empty;
        public int Year { get; set; }
        public double RoomRevenue { get; set; }
        public double RestaurantRevenue { get; set; }
    }
}