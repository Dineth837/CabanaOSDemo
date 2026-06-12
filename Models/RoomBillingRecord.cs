using System.ComponentModel;

namespace CabanaOSDemo.Models
{
    public class RoomBillingRecord
    {
        // Adding "= string.Empty" tells the compiler the property is initialized
        public string BookingID { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string States { get; set; } = string.Empty;
        public string TotalDue { get; set; } = string.Empty;
    }
}