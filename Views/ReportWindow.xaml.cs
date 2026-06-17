using CabanaOSDemo.Data;
using CabanaOSDemo.Utils;
using System.IO;
using System.Windows;

namespace CabanaOSDemo.Views
{
    
    public partial class ReportWindow : Window
    {
        public ReportWindow()
        {
            InitializeComponent();
        }

        
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnMonthly_Click(object sender, RoutedEventArgs e)
        {
                

            
            string reportFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "CabanaOS", "Reports", "Monthly Revenue Summary"
            );

            
            if (!Directory.Exists(reportFolder))
            {
                Directory.CreateDirectory(reportFolder);
            }

            
            string fileName = $"Monthly_Report_{DateTime.Now:yyyyMMdd}.pdf";
            string fullPath = Path.Combine(reportFolder, fileName);


            string suiteRev = BillingRepository.GetTotalRoomRevenue().ToString();
            string resRev = BillingRepository.GetTotalRestaurantRevenue().ToString();
            string totalRev = (BillingRepository.GetTotalRoomRevenue() + BillingRepository.GetTotalRestaurantRevenue()).ToString();

            try
            {
                PdfReportService.GenerateMonthlyRevenueReport(fullPath, suiteRev, resRev, totalRev);

                MessageBox.Show($"Report generated successfully at:\n{fullPath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating PDF: {ex.Message}");
            }
        }

        private void BtnDaily_Click(object sender, RoutedEventArgs e)
        {
            
            var dailyData = new List<(string, string)>
            {
                ("Room 101 - Suite", "Rs. 15,000.00"),
                ("Restaurant - Dinner", "Rs. 2,450.00"),
                ("Room 202 - Suite", "Rs. 12,000.00"),
                ("Restaurant - Drinks", "Rs. 850.00")
            };

            string grandTotal = "Rs. 30,300.00";
            string fullPath = GetCustomPath("Daily Transactions", "Daily_Report.pdf");

            
            PdfReportService.GenerateDailyTransactionsReport(fullPath, dailyData, grandTotal);
            MessageBox.Show("Daily report generated!");
        }

        private void BtnCustomer_Click(object sender, RoutedEventArgs e)
        {
            
            var customerModels = BillingRepository.GetAllCustomers();

            var customerListForPdf = customerModels.Select(c =>
                (c.CustomerID, c.FullName, c.NICNumber)
            ).ToList();

            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CabanaOS", "Reports", "Customer Reports");
            Directory.CreateDirectory(folder);
            string fileName = $"Customer_Report_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
            string fullPath = Path.Combine(folder, fileName);

            try
            {
                PdfReportService.GenerateCustomerReport(fullPath, customerListForPdf);
                MessageBox.Show($"Customer report generated successfully:\n{fullPath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating PDF: {ex.Message}");
            }
        }

        private void BtnEmployee_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Employee report is being generated.");
        }

        private string GetCustomPath(string subFolder, string fileName)
        {
            
            string reportFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "CabanaOS", "Reports", subFolder
            );

            
            if (!Directory.Exists(reportFolder))
            {
                Directory.CreateDirectory(reportFolder);
            }

            
            return Path.Combine(reportFolder, fileName);
        }
    }
}