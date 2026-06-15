using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.ObjectModel; // 🚀 ADDED: Required for dynamic UI lists
using CabanaOSDemo.Data;
using CabanaOSDemo.Models;

namespace CabanaOSDemo.Views
{
    public partial class RestaurantBillSummaryView : UserControl
    {
        private readonly RestaurantReservationsView? _mainView;

        // 🚀 ADDED: The dynamic list that talks directly to your XAML ItemsControl
        public ObservableCollection<FoodOrderItem> CurrentOrders { get; set; }

        public RestaurantBillSummaryView(RestaurantReservationsView? mainView, string tableName, string zone, string guestName, string totalBill = "Rs. 00.00", string orderId = "", string sourceModule = "Reservation")
        {
            InitializeComponent();
            _mainView = mainView;

            BtnReleaseTable.Click += BtnReleaseAction_Click;

            // --- 1. ALWAYS DO THIS: Setup Data and Text Fields ---
            // (This needs to run for both Billing AND Reservation sections!)
            CurrentOrders = new ObservableCollection<FoodOrderItem>();
            FoodOrdersList.ItemsSource = CurrentOrders;

            TxtTableNumber.Text = tableName;
            TxtCategoryName.Text = zone + " Zone";
            TxtCustomerName.Text = string.IsNullOrEmpty(guestName) ? "In-House Guest" : guestName;
            TxtFooterSubtitle.Text = $"{zone} Zone Table Occupied";
            TxtTotalStatementBill.Text = totalBill ?? "Rs. 00.00";

            // --- 2. CONTEXT AWARE ROUTING ENGINE ---
            // 1. Declare routing variables strictly
            bool isBillingSection;

            // 2. Assign routing values strictly
            isBillingSection = (sourceModule == "Billing");

            // 3. Execution Guard
            if (isBillingSection)
            {
                // Triggers your exact logic tree for the Billing Section
                ApplyBillingStyle(orderId);
            }
            else
            {
                // ONLY apply default Reservation colors if we are NOT in the Billing section
                if (zone == "Deluxe")
                {
                    SolidColorBrush deluxeBg = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF4E8"));
                    BdrMainStatusCard.Background = deluxeBg;
                    BdrTableNumCard.Background = deluxeBg;
                    BdrCategoryCard.Background = deluxeBg;
                    BdrBillSummaryCard.Background = deluxeBg;

                    BtnSettleBill.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEF08A"));
                    BtnSettleBill.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#713F12"));

                    BtnReleaseTable.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEF08A"));
                    BtnReleaseTable.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#713F12"));

                    txtHead.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#713F12"));
                }
                else
                {
                    SolidColorBrush standardBg = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0FDF4"));
                    BdrMainStatusCard.Background = standardBg;
                    BdrTableNumCard.Background = standardBg;
                    BdrCategoryCard.Background = standardBg;
                    BdrBillSummaryCard.Background = standardBg;

                    BtnSettleBill.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BBF7D0"));
                    BtnSettleBill.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14532D"));

                    BtnReleaseTable.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BBF7D0"));
                    BtnReleaseTable.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14532D"));

                    txtHead.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14532D"));
                }
            }
        }


        private void ApplyBillingStyle(string currentOrderId)
        {
            // 0. Safety Guard to prevent crashes if a blank row is clicked
            if (string.IsNullOrEmpty(currentOrderId))
            {
                return;
            }

            // 1. Declare all variables strictly first
            System.Windows.Media.BrushConverter colorTool;
            System.Windows.Media.SolidColorBrush stForeground;
            System.Windows.Media.SolidColorBrush stBackground;
            System.Windows.Media.SolidColorBrush dlxForeground;
            System.Windows.Media.SolidColorBrush dlxBackground;

            System.Windows.Visibility visibleState;
            bool isStandardZone;
            bool isDeluxeZone;
            string billingTitle;

            // 2. Assign values strictly in separate steps (using '!' to prevent null warnings)
            colorTool = new System.Windows.Media.BrushConverter();

            // Standard Colors (#14532D and #BBF7D0)
            stForeground = (System.Windows.Media.SolidColorBrush)colorTool.ConvertFromString("#14532D")!;
            stBackground = (System.Windows.Media.SolidColorBrush)colorTool.ConvertFromString("#BBF7D0")!;

            // Deluxe Colors (#713F12 and #FEF08A)
            dlxForeground = (System.Windows.Media.SolidColorBrush)colorTool.ConvertFromString("#713F12")!;
            dlxBackground = (System.Windows.Media.SolidColorBrush)colorTool.ConvertFromString("#FEF08A")!;


            visibleState = System.Windows.Visibility.Visible;
            billingTitle = "Final Invoice Settlement";

            isStandardZone = currentOrderId.StartsWith("STCZ") || currentOrderId.StartsWith("STFZ");
            isDeluxeZone = currentOrderId.StartsWith("DLXCZ") || currentOrderId.StartsWith("DLXFZ");

            // 3. Apply standard billing section overrides
            txtHead.Text = billingTitle;
            BtnSettleBill.Visibility = visibleState;
            BtnReleaseTable.Visibility = visibleState;

            // 4. Execute your exact Order ID prefix logic tree
            if (isStandardZone)
            {
                txtHead.Foreground = stBackground;

                BtnSettleBill.Foreground = stForeground;
                BtnSettleBill.Background = stBackground;

                BtnReleaseTable.Foreground = stForeground;
                BtnReleaseTable.Background = stBackground;
            }
            else if (isDeluxeZone)
            {
                txtHead.Foreground = dlxBackground;

                BtnSettleBill.Foreground = dlxForeground;
                BtnSettleBill.Background = dlxBackground;

                BtnReleaseTable.Foreground = dlxForeground;
                BtnReleaseTable.Background = dlxBackground;
            }
        }

        private void BtnReleaseAction_Click(object sender, RoutedEventArgs e)
        {
            ExecuteGlobalTableRelease();
        }

        // 🚀 THE FIXED LIVE SYNC SETTLE RESTAURANT BILL ENGINE
        private void BtnSettleBill_Click(object sender, RoutedEventArgs e)
        {
            string targetTable = TxtTableNumber.Text;

            var activeBill = BillingRepository.RestaurantInvoices.FirstOrDefault(r => r != null && r.TableNumber == targetTable && r.States == "Ongoing");

            if (activeBill != null)
            {
                activeBill.States = "Complete";

                var assetTable = BillingRepository.MasterTables.FirstOrDefault(t => t != null && t.TableID == targetTable);
                if (assetTable != null)
                {
                    assetTable.States = "Complete";
                }

                BillingRepository.SaveAllDataToFiles();
                ExecuteGlobalTableRelease();

                MessageBox.Show($"Table {targetTable} Bill Settled Successfully!\n\n" +
                                $"The statement sum {TxtTotalStatementBill.Text} has been transferred to Today's Restaurant Sales Ledger.\n" +
                                $"Table visual card state refreshed instantly to Available.",
                                "POS Terminal Checkout Counter", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExecuteGlobalTableRelease()
        {
            string targetTable = TxtTableNumber.Text;

            // Mark completion states inside local repository tracking systems
            var assetTable = BillingRepository.MasterTables.FirstOrDefault(t => t != null && t.TableID == targetTable);
            if (assetTable != null) assetTable.States = "Complete";

            BillingRepository.UpdateTableStatus(targetTable,"Complete");

            // Intercept and query background visual trees to force live component mapping redraw loops
            if (_mainView != null)
            {
                _mainView.ReleaseTableRealTime(targetTable);
            }
            else
            {
                var appShell = Application.Current.MainWindow as HomePageShell;
                if (appShell != null)
                {
                    var activeRestTab = appShell.GetPersistentRestaurantView();
                    if (activeRestTab != null)
                    {
                        activeRestTab.ReleaseTableRealTime(targetTable);
                    }
                }
            }
        }
    }
}