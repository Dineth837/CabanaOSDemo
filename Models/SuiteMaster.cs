namespace CabanaOSDemo.Models
{
    public class SuiteMaster
    {
        public string RoomID { get; set; } = string.Empty;        // e.g., "RM 001"
        public string Tier { get; set; } = string.Empty;          // Standard, Superior, or Deluxe
        public double Price { get; set; }                         // Base numeric room rate
        public string Type { get; set; } = string.Empty;          // Family or Couple
        public string States { get; set; } = string.Empty;        // CheckedIn, CheckedOut, or UnderCleaning
    }
}