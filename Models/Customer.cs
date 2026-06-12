namespace CabanaOSDemo.Models
{
    public class Customer
    {
        // Maps to CustomerID (INTEGER)
        public string CustomerID { get; set; } = string.Empty;

        // Maps to FullName (TEXT)
        public string FullName { get; set; } = string.Empty;

        // Maps to NICNumber (TEXT)
        public string NICNumber { get; set; } = string.Empty;

        // Maps to CustomerType (TEXT) - Now specifically for "Couple" or "Family"
        public string CustomerType { get; set; } = "Couple";
    }
}