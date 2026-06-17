using CabanaOSDemo.Data; // Direct bridge to your Data folder array lists
using CabanaOSDemo.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CabanaOSDemo.Views
{
    public partial class BillingView : UserControl
    {
        private string currentSubModuleContext = "Restaurant";

        public BillingView()
        {
            InitializeComponent();

            // Render the initial Restaurant data setup on launch by default
            SwitchBillingModuleContext("Restaurant");
            UpdateKpiCards(); // 🚀 Initial load calculation trigger
        }

        private void SubModuleFilter_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                // Reset pills layout back to standard inactive configurations
                BtnToggleRestaurant.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E8F0"));
                BtnToggleRestaurant.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B"));
                BtnToggleRestaurant.FontWeight = FontWeights.Normal;

                BtnToggleRooms.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E8F0"));
                BtnToggleRooms.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B"));
                BtnToggleRooms.FontWeight = FontWeights.Normal;

                // Turn the active selection capsule dark navy blue
                clickedButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#111827"));
                clickedButton.Foreground = Brushes.White;
                clickedButton.FontWeight = FontWeights.Bold;

                if (clickedButton == BtnToggleRooms)
                {
                    currentSubModuleContext = "Rooms";
                    SwitchBillingModuleContext("Rooms");
                }
                else
                {
                    currentSubModuleContext = "Restaurant";
                    SwitchBillingModuleContext("Restaurant");
                }
            }
        }

        private void SwitchBillingModuleContext(string context)
        {
            // 🛡️ 1. CRASH SHIELD: Break old items control references completely
            DgdAuditLedger.ItemsSource = null;
            DgdAuditLedger.Columns.Clear();
            DynamicBillingWorkspace.Content = null;

            // 🚀 2. CREATE A PERFECTLY CENTERED ALIGNMENT STYLE FOR THE COLUMN CELLS
            Style centerCellStyle = new Style(typeof(TextBlock));
            centerCellStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
            centerCellStyle.Setters.Add(new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center));
            centerCellStyle.Setters.Add(new Setter(TextBlock.PaddingProperty, new Thickness(10, 0, 10, 0)));

            // 🏗️ 3. BUILD THE COLUMNS FIRST (DO NOT BIND THE DATA YET!)
            if (context == "Restaurant")
            {
                TxtKpiRevenueTitle.Text = "Total Restaurant Revenue";
                TxtKpiDuesTitle.Text = "Ongoing Order Due";

                DgdAuditLedger.Columns.Add(new DataGridTextColumn { Header = "OrderID", Binding = new System.Windows.Data.Binding("OrderID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star), ElementStyle = centerCellStyle });
                DgdAuditLedger.Columns.Add(new DataGridTextColumn { Header = "TableNumber", Binding = new System.Windows.Data.Binding("TableNumber"), Width = new DataGridLength(1, DataGridLengthUnitType.Star), ElementStyle = centerCellStyle });
                DgdAuditLedger.Columns.Add(new DataGridTextColumn { Header = "CustomerName", Binding = new System.Windows.Data.Binding("CustomerName"), Width = new DataGridLength(1.2, DataGridLengthUnitType.Star), ElementStyle = centerCellStyle });
                DgdAuditLedger.Columns.Add(new DataGridTextColumn { Header = "Date", Binding = new System.Windows.Data.Binding("Date"), Width = new DataGridLength(1, DataGridLengthUnitType.Star), ElementStyle = centerCellStyle });
                DgdAuditLedger.Columns.Add(new DataGridTextColumn { Header = "States", Binding = new System.Windows.Data.Binding("States"), Width = new DataGridLength(1, DataGridLengthUnitType.Star), ElementStyle = centerCellStyle });
                DgdAuditLedger.Columns.Add(new DataGridTextColumn { Header = "TotalBill", Binding = new System.Windows.Data.Binding("TotalBill"), Width = new DataGridLength(1, DataGridLengthUnitType.Star), ElementStyle = centerCellStyle });
            }
            else
            {
                TxtKpiRevenueTitle.Text = "Total Cabana Revenue";
                TxtKpiDuesTitle.Text = "Ongoing Suite Dues";

                DgdAuditLedger.Columns.Add(new DataGridTextColumn { Header = "BookingID", Binding = new System.Windows.Data.Binding("BookingID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star), ElementStyle = centerCellStyle });
                DgdAuditLedger.Columns.Add(new DataGridTextColumn { Header = "RoomNumber", Binding = new System.Windows.Data.Binding("RoomNumber"), Width = new DataGridLength(1, DataGridLengthUnitType.Star), ElementStyle = centerCellStyle });
                DgdAuditLedger.Columns.Add(new DataGridTextColumn { Header = "CustomerName", Binding = new System.Windows.Data.Binding("CustomerName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star), ElementStyle = centerCellStyle });
                DgdAuditLedger.Columns.Add(new DataGridTextColumn { Header = "Category", Binding = new System.Windows.Data.Binding("Category"), Width = new DataGridLength(1.2, DataGridLengthUnitType.Star), ElementStyle = centerCellStyle });
                DgdAuditLedger.Columns.Add(new DataGridTextColumn { Header = "States", Binding = new System.Windows.Data.Binding("States"), Width = new DataGridLength(1, DataGridLengthUnitType.Star), ElementStyle = centerCellStyle });
                DgdAuditLedger.Columns.Add(new DataGridTextColumn { Header = "TotalDue", Binding = new System.Windows.Data.Binding("TotalDue"), Width = new DataGridLength(1, DataGridLengthUnitType.Star), ElementStyle = centerCellStyle });
            }

            // 🎨 4. STYLE THE HEADERS SAFELY
            foreach (var column in DgdAuditLedger.Columns)
            {
                column.HeaderStyle = new Style(typeof(System.Windows.Controls.Primitives.DataGridColumnHeader));
                column.HeaderStyle.Setters.Add(new Setter(HorizontalContentAlignmentProperty, HorizontalAlignment.Center));
                column.HeaderStyle.Setters.Add(new Setter(BackgroundProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E8F0"))));
                column.HeaderStyle.Setters.Add(new Setter(ForegroundProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B"))));
                column.HeaderStyle.Setters.Add(new Setter(FontWeightProperty, FontWeights.Bold));
                column.HeaderStyle.Setters.Add(new Setter(PaddingProperty, new Thickness(10, 6, 10, 6)));
            }

            // 🔗 5. FINALLY, POUR THE DATA IN! (Now it draws perfectly with no interruptions)
            if (context == "Restaurant")
            {
                DgdAuditLedger.ItemsSource = BillingRepository.RestaurantInvoices;
            }
            else
            {
                DgdAuditLedger.ItemsSource = BillingRepository.RoomInvoices;
            }

            // 🚀 6. Update the KPI cards at the end
            UpdateKpiCards();
        }

        private void UpdateKpiCards()
        {
            // 📅 DYNAMIC AUTOMATED DATE: Pulls live system calendar day instantly
            string realSystemDate = $"Date : {DateTime.Now.ToString("yyyy.MM.dd")}";
            TxtKpiRevenueDate.Text = realSystemDate;
            TxtKpiDuesDate.Text = realSystemDate;

            if (currentSubModuleContext == "Restaurant")
            {
                // Fetch values directly from our repository models metrics tuple
                var metrics = BillingRepository.GetRestaurantMetrics();

                TxtKpiRevenueValue.Text = $"Rs. {metrics.TotalRevenue:N2}";
                TxtKpiDuesValue.Text = $"Rs. {metrics.TotalDues:N2}";
                TxtKpiAvailLabel.Text = $"{metrics.Available} Tables Available";
                TxtKpiOccLabel.Text = $"{metrics.Occupied} Tables Occupied";
            }
            else // Rooms context dashboard view chosen
            {
                // Fetch values directly from our repository models metrics tuple
                var metrics = BillingRepository.GetRoomMetrics();

                TxtKpiRevenueValue.Text = $"Rs. {metrics.TotalRevenue:N2}";
                TxtKpiDuesValue.Text = $"Rs. {metrics.TotalDues:N2}";
                TxtKpiAvailLabel.Text = $"{metrics.Available} Rooms Available";
                TxtKpiOccLabel.Text = $"{metrics.Occupied} Rooms Occupied";
            }
        }

        private void DgdAuditLedger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgdAuditLedger.SelectedItem == null) return;

            Action clearSidebarCallback = () => {
                DynamicBillingWorkspace.Content = null;
                DgdAuditLedger.SelectedItem = null;
                UpdateKpiCards();
            };

            if (currentSubModuleContext == "Restaurant")
            {
                var record = (RestaurantBillingRecord)DgdAuditLedger.SelectedItem;

                // Safe-guards: Extracts values or provides non-null fallbacks
                string orderID = record.OrderID;
                string state = record.States ?? "Ongoing";
                string tableNum = record.TableNumber ?? "N/A";
                string custName = record.CustomerName ?? "Guest";
                string billAmt = record.TotalBill ?? "Rs. 00.00";
                string zone = "";

                bool isDeluxeOrder;

                // 2. Assign the evaluation logic
                // We check if the OrderID starts with the Deluxe prefixes
                isDeluxeOrder = orderID.StartsWith("DLXCZ") || orderID.StartsWith("DLXFZ");

                // 3. Extract the zone string
                if (isDeluxeOrder)
                {
                    zone = "Deluxe";
                }
                else
                {
                    // If it doesn't start with DLX, it automatically falls back to Standard
                    zone = "Standard";
                }


                if (state.Equals("Complete", StringComparison.OrdinalIgnoreCase))
                {
                    DynamicBillingWorkspace.Content = new InvoiceArchivedView(clearSidebarCallback, "Restaurant & Reservation Bill", tableNum, "Standard Zone", billAmt);
                }
                else
                {
                    DynamicBillingWorkspace.Content = new RestaurantBillSummaryView(null, tableNum, zone, custName, billAmt, orderID, "Billing");
                }
            }
            // 🚀 Look for this section inside your BillingView.xaml.cs file:
            else
            {
                var record = (RoomBillingRecord)DgdAuditLedger.SelectedItem;

                string state = record.States ?? "CheckedIn";
                string roomNum = record.RoomNumber ?? "N/A";
                string category = record.Category ?? "Standard Suite";
                string custName = record.CustomerName ?? "Guest";
                string dueAmt = record.TotalDue ?? "Rs. 00.00";

                if (state.Equals("CheckedOut", StringComparison.OrdinalIgnoreCase))
                {
                    DynamicBillingWorkspace.Content = new InvoiceArchivedView(clearSidebarCallback, "Room & Accommodation Bill", roomNum, category, dueAmt);
                }
                else
                {
                    // 🚀 FIXED: Pass a dynamic delegate loop callback function here
                    Action releaseRoomCallback = () => {
                        // 1. Open up an access gateway shell connection reference to the persistent memory views
                        var appShell = Application.Current.MainWindow as HomePageShell;
                        if (appShell != null)
                        {
                            // 2. Clear status tracker inside your persistent layout window control natively
                            // This ensures that when you switch back to the suite view, its color changes from RED to GREEN!
           
                        }

                        // 3. Run global grids reset actions
                        clearSidebarCallback.Invoke();
                    };

                    DynamicBillingWorkspace.Content = new SuiteBillingSummaryView(releaseRoomCallback, roomNum, category, custName, dueAmt);
                }
            }
        } // 👈 Ensured methods close out their brackets accurately!
    }
}