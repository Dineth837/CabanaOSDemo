using System;
using System.Windows;
using System.Windows.Controls;
using CabanaOSDemo.Data;
using CabanaOSDemo.Utils;
using CabanaOSDemo.Views;

namespace CabanaOSDemo.Views
{
    public partial class ManagementDashboardView : UserControl
    {
        public ManagementDashboardView()
        {
            InitializeComponent();
            this.IsVisibleChanged += ManagementDashboardView_IsVisibleChanged;
            LoadLiveManagementAnalytics();

        }



        private void ManagementDashboardView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true) LoadLiveManagementAnalytics();
        }

        public void LoadLiveManagementAnalytics()
        {
            double totalResRev = BillingRepository.GetTotalRestaurantRevenue();
            double totalSuiteRev = BillingRepository.GetTotalRoomRevenue();
            double totalIncome = totalResRev+totalSuiteRev;

            
            TxtRestaurantSalesVal.Text = $"Rs. {totalResRev:N2}";
            TxtSuiteSalesVal.Text = $"Rs. {totalSuiteRev:N2}";
            TxtCombinedRevenueVal.Text = $"Rs. {totalIncome:N2}";


            
            var history = BillingRepository.LoadAllRevenueHistory();

            
            double[] liveRest = new double[12];
            double[] liveSuite = new double[12];

            
            foreach (var record in history)
            {
                
                int monthIndex = DateTime.ParseExact(record.Month, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month - 1;

                if (monthIndex >= 0 && monthIndex < 12)
                {
                    liveRest[monthIndex] = record.RestaurantRevenue;
                    liveSuite[monthIndex] = record.RoomRevenue;
                }
            }

            
            MonthlyBarChartCtrl.SetManualValues(liveRest, liveSuite);

        }

        
        private void SuitesPieChartCtrl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void RestaurantPieChartCtrl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }

        private void MonthlyBarChartCtrl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow();

            
            reportWindow.Owner = Application.Current.MainWindow;

            
            reportWindow.ShowDialog();
        }
    }


}