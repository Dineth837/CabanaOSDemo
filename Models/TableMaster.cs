namespace CabanaOSDemo.Models
{
    public class TableMaster
    {
        public string TableID { get; set; } = string.Empty;       // e.g., "ST C01"
        public string Zone { get; set; } = string.Empty;          // Standard or Deluxe
        public string Type { get; set; } = string.Empty;          // Family or Couple
        public string States { get; set; } = string.Empty;        // Ongoing or Complete
    }
}