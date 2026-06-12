using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CabanaOSDemo.Views;

namespace CabanaOSDemo
{
    public partial class HomePageShell : Window
    {
        // Your existing private cached views
        private RoomsAccommodationsView _roomsView;
        private RestaurantReservationsView _restaurantView;
        private BillingView _billingView;

        public HomePageShell()
        {
            InitializeComponent();

            _roomsView = new RoomsAccommodationsView();
            _restaurantView = new RestaurantReservationsView();
            _billingView = new BillingView();

            MainViewPresenterWorkspace.Content = new DefaultWelcomeView();
            BdrWorkspaceBackground.Background = Brushes.Transparent;
        }

        // 🚀 ADD THIS METHOD HERE TO FIX THE ERROR
        public RoomsAccommodationsView GetPersistentRoomsView()
        {
            return _roomsView;
        }

        private void SidebarMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                // Reset button highlights
                BtnBilling.Style = (Style)this.FindResource("SidebarMenuNavigationButtonTemplate");
                BtnRestaurant.Style = (Style)this.FindResource("SidebarMenuNavigationButtonTemplate");
                BtnRooms.Style = (Style)this.FindResource("SidebarMenuNavigationButtonTemplate");
                BtnDashboard.Style = (Style)this.FindResource("SidebarMenuNavigationButtonTemplate");

                // Highlight clicked button
                clickedButton.Style = (Style)this.FindResource("SidebarActiveLinkHighlightButtonTemplate");

                // 🔄 THE FIX: Swap the cached instances instead of using 'new'
                if (clickedButton == BtnRooms)
                {
                    // Swaps to the pre-existing instance holding your live dictionary states!
                    MainViewPresenterWorkspace.Content = _roomsView;
                }
                else if (clickedButton == BtnRestaurant)
                {
                    MainViewPresenterWorkspace.Content = _restaurantView;
                }
                else if (clickedButton == BtnBilling)
                {
                    // Refresh fields inside your cached auditing workspace right before viewing
                    MainViewPresenterWorkspace.Content = _billingView;
                }
                else if (clickedButton == BtnDashboard)
                {
                    Action onSuccessfulAuthCallback = () => {
                        MainViewPresenterWorkspace.Content = new ManagementDashboardView();
                    };

                    MainViewPresenterWorkspace.Content = new ManagementLoginView(onSuccessfulAuthCallback);
                }
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // 🚀 ADD THIS HOOK METHOD INSIDE HomePageShell CLASS TO RESOLVE RESTAURANT GRID INTERACTION SYNC
        public Views.RestaurantReservationsView GetPersistentRestaurantView()
        {
            return _restaurantView;
        }




    }
}