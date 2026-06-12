using CabanaOSDemo.Data;
using System.Collections.Generic;

public class MonthlyRevenueReport
{
    public string MonthName { get; set; }
    public double RestaurantRevenue { get; set; }
    public double SuiteRevenue { get; set; }
}

public static class ReportEngine
{
    public static List<MonthlyRevenueReport> GetAnnualReport()
    {
        var report = new List<MonthlyRevenueReport>();
        string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        for (int i = 1; i <= 12; i++)
        {
            report.Add(new MonthlyRevenueReport
            {
                MonthName = months[i - 1],
                RestaurantRevenue = BillingRepository.GetMonthlyRevenue(i, "Restaurant"),
                SuiteRevenue = BillingRepository.GetMonthlyRevenue(i, "Suites")
            });
        }
        return report;
    }
}