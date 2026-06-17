using System;
using System.Windows;
using System.Windows.Controls;
using CabanaOSDemo.Data;
using CabanaOSDemo.Utils;

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
            // 🚀 TEMPORARY TEST: Bypass the DB to see if bars respond to hardcoded values
            double[] testRest = { 10000, 20000, 30000, 40000, 50000, 100000, 0, 0, 0, 0, 0, 0 };
            double[] testSuite = { 90000, 80000, 70000, 60000, 50000, 40000, 0, 0, 0, 0, 0, 0 };

            // Call the manual method directly
            MonthlyBarChartCtrl.SetManualValues(testRest, testSuite);
        }

        // ... inside your class ...
        private void SuitesPieChartCtrl_Loaded(object sender, RoutedEventArgs e)
        {
            // If you don't need code here, leave it empty, but keep the signature!
        }

        private void RestaurantPieChartCtrl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // You can leave this empty if you don't need logic here
        }


    }


}