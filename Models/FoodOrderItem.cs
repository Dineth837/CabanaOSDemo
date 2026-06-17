namespace CabanaOSDemo.Models
{
    public class FoodOrder
    {
        public string OrderID { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public double Price { get; set; }   = 0.0;
        public int Quantity { get; set; }   = 0;
    }
}