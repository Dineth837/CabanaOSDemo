using CabanaOSDemo.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CabanaOSDemo.Views
{
    public partial class InvoiceConfirmedView : UserControl
    {
        private readonly RoomsAccommodationsView? _mainView;
        private readonly string _roomNumber;
        private readonly string _tier;

        public InvoiceConfirmedView(RoomsAccommodationsView mainView, string roomNumber, string tier)
        {
            InitializeComponent();
            _mainView = mainView;
            _roomNumber = roomNumber;
            _tier = tier;

            // Initialize visual states
            TxtInvoiceRoomNumber.Text = roomNumber;
            ApplyTierInvoiceTheme(tier);
        }

        private void ApplyTierInvoiceTheme(string tier)
        {
            if (tier == "Superior")
            {
                

                TxtSuiteBookedLabel.Text = "Superior Suite Booked";
                TxtInvoiceCategory.Text = "Superior Suite";
                TxtAccommodationCharge.Text = "Rs. 12,500.00";
                TxtTotalStatementDue.Text = "Rs. 12,500.00";

                
                SolidColorBrush supBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8F0FE"));
                SolidColorBrush supButtonBg = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ADC4F7"));
                SolidColorBrush supButtonFg = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A"));

                BdrOccupantStatus.Background = supBackground;
                BdrRoomNumberBadge.Background = supBackground;
                BdrCategoryBadge.Background = supBackground;
                BdrInvoiceStatementCard.Background = supBackground;
                BdrInvoiceStatementCard.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ADC4F7"));

                BtnSettleDue.Background = supButtonBg;
                BtnSettleDue.Foreground = supButtonFg;
                BtnReleaseRoom.Background = supButtonBg;
                BtnReleaseRoom.Foreground = supButtonFg;

                TxtMainTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A"));
                TxtDueTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A"));
                TxtCusName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A"));
                TxtInvoiceRoomNumber.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A"));
                TxtInvoiceCategory.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A"));
                TxtInvoiceTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A"));
                TxtTotalStatementDue.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A"));
            }
            else if (tier == "Deluxe")
            {
                TxtSuiteBookedLabel.Text = "Deluxe Suite Booked";
                TxtInvoiceCategory.Text = "Deluxe Suite";
                TxtAccommodationCharge.Text = "Rs. 20,000.00";
                TxtTotalStatementDue.Text = "Rs. 20,000.00";

                
                SolidColorBrush dlxBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF4E8"));
                SolidColorBrush dlxButtonBg = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FCE8CE"));
                SolidColorBrush dlxButtonFg = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));

                BdrOccupantStatus.Background = dlxBackground;
                BdrRoomNumberBadge.Background = dlxBackground;
                BdrCategoryBadge.Background = dlxBackground;
                BdrInvoiceStatementCard.Background = dlxBackground;
                BdrInvoiceStatementCard.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FCE8CE"));

                BtnSettleDue.Background = dlxButtonBg;
                BtnSettleDue.Foreground = dlxButtonFg;
                BtnReleaseRoom.Background = dlxButtonBg;
                BtnReleaseRoom.Foreground = dlxButtonFg;

                TxtMainTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));
                TxtDueTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));
                TxtCusName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));
                TxtInvoiceRoomNumber.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));
                TxtInvoiceCategory.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));
                TxtInvoiceTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));
                TxtTotalStatementDue.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));
            }
            else 
            {
                TxtSuiteBookedLabel.Text = "Standard Suite Booked";
                TxtInvoiceCategory.Text = "Standard Suite";
                TxtAccommodationCharge.Text = "Rs. 8,500.00";
                TxtTotalStatementDue.Text = "Rs. 8,500.00";

                SolidColorBrush stdBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0FDF4"));
                SolidColorBrush stdButtonBg = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A7F3D0"));
                SolidColorBrush stdButtonFg = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));

                BdrOccupantStatus.Background = stdBackground;
                BdrRoomNumberBadge.Background = stdBackground;
                BdrCategoryBadge.Background = stdBackground;
                BdrInvoiceStatementCard.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0FDF4"));

                BtnSettleDue.Background = stdButtonBg;
                BtnSettleDue.Foreground = stdButtonFg;
                BtnReleaseRoom.Background = stdButtonBg;
                BtnReleaseRoom.Foreground = stdButtonFg;

                TxtMainTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));
                TxtDueTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));
                TxtCusName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));
                TxtInvoiceRoomNumber.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));
                TxtInvoiceCategory.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));
                TxtInvoiceTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));
                TxtTotalStatementDue.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));
            }
        }

        //  Added the missing click handler your XAML action button is searching for
        private void BtnSettleDue_Click(object sender, RoutedEventArgs e)
        {
            string targetRoom = _roomNumber;
            string totalDue = TxtTotalStatementDue.Text;

            // Update Repository states
            var invoice = BillingRepository.RoomInvoices.FirstOrDefault(r => r != null && r.RoomNumber == targetRoom && r.States == "CheckedIn");
            if (invoice != null) invoice.States = "CheckedOut";

            var suite = BillingRepository.MasterSuites.FirstOrDefault(s => s != null && s.RoomID == targetRoom);
            if (suite != null) suite.States = "CheckedOut";

            BillingRepository.UpdateSuiteStatus(targetRoom, "CheckedOut");
            BillingRepository.SaveAllDataToFiles(); // Ensure persistence

            // Trigger global refresh
            ExecuteGlobalRoomRelease();

            MessageBox.Show($"Suite No {targetRoom} Bill Settled Successfully!\n\n" +
                            $"The statement sum {totalDue} has been transferred to Today's Suite Sales Ledger.\n" +
                            $"Room visual card state refreshed instantly to Available.",
                            "POS Terminal Checkout Counter", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //  RELEASE ROOM: Mirroring Restaurant "ReleaseTableAction"
        private void BtnReleaseRoom_Click(object sender, RoutedEventArgs e)
        {
            ExecuteGlobalRoomRelease();
            MessageBox.Show($"Room {_roomNumber} returned to vacant pool.", "System Success");
        }

        //  THE FIXED SYNC ENGINE: Matches your Restaurant "ExecuteGlobalTableRelease"
        private void ExecuteGlobalRoomRelease()
        {
            // Update repository
            var suite = BillingRepository.MasterSuites.FirstOrDefault(s => s != null && s.RoomID == _roomNumber);
            if (suite != null) suite.States = "CheckedOut";

            BillingRepository.UpdateSuiteStatus(_roomNumber, "CheckedOut");

            // Intercept and query background visual tree
            if (_mainView != null)
            {
                _mainView.ReleaseRoomRealTime(_roomNumber);
            }
            else
            {
                // Fallback to Shell if MainView reference is somehow lost
                var appShell = Application.Current.MainWindow as HomePageShell;
                appShell?.GetPersistentRoomsView()?.ReleaseRoomRealTime(_roomNumber);
            }
        }

    }
}