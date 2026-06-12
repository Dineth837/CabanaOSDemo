using System;

namespace CabanaOSDemo.Models
{
    public class BillingSummary
    {
        // --- 🍔 RESTAURANT FINANCIAL METRICS ---
        public double TotalRestaurantRevenue { get; set; }
        public double OngoingOrderDues { get; set; }
        public int AvailableTables { get; set; }
        public int OccupiedTables { get; set; }

        // --- 🛏️ ACCOMMODATION FINANCIAL METRICS ---
        public double TotalCabanaRevenue { get; set; }
        public double OngoingSuiteDues { get; set; }
        public int AvailableRooms { get; set; }
        public int OccupiedRooms { get; set; }

        // --- 📊 UNIFIED METRICS ---
        public string LastUpdatedDate { get; set; } = DateTime.Now.ToString("yyyy.MM.dd");

        public double CombinedTotalRevenue => TotalRestaurantRevenue + TotalCabanaRevenue;
        public double CombinedTotalDues => OngoingOrderDues + OngoingSuiteDues;
    }
}