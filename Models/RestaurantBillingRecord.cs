namespace CabanaOSDemo.Models
{
    public class RestaurantBillingRecord
    {
        // 🚀 FIX: Appended '?' to allow properties to be null safely without throwing compiler errors
        public string? OrderID { get; set; }
        public string? TableNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? Date { get; set; }
        public string? States { get; set; }
        public string? TotalBill { get; set; }
    }
}