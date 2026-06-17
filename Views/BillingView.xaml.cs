using CabanaOSDemo.Data;
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

            
            SwitchBillingModuleContext("Restaurant");
            UpdateKpiCards(); 
        }

        private void SubModuleFilter_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                BtnToggleRestaurant.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E8F0"));
                BtnToggleRestaurant.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B"));
                BtnToggleRestaurant.FontWeight = FontWeights.Normal;

                BtnToggleRooms.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E8F0"));
                BtnToggleRooms.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B"));
                BtnToggleRooms.FontWeight = FontWeights.Normal;

                
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
            
            DgdAuditLedger.ItemsSource = null;
            DgdAuditLedger.Columns.Clear();
            DynamicBillingWorkspace.Content = null;

            
            Style centerCellStyle = new Style(typeof(TextBlock));
            centerCellStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
            centerCellStyle.Setters.Add(new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center));
            centerCellStyle.Setters.Add(new Setter(TextBlock.PaddingProperty, new Thickness(10, 0, 10, 0)));

            BillingRepository.LoadAllDataFromFiles();

            
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

            
            foreach (var column in DgdAuditLedger.Columns)
            {
                column.HeaderStyle = new Style(typeof(System.Windows.Controls.Primitives.DataGridColumnHeader));
                column.HeaderStyle.Setters.Add(new Setter(HorizontalContentAlignmentProperty, HorizontalAlignment.Center));
                column.HeaderStyle.Setters.Add(new Setter(BackgroundProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E8F0"))));
                column.HeaderStyle.Setters.Add(new Setter(ForegroundProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B"))));
                column.HeaderStyle.Setters.Add(new Setter(FontWeightProperty, FontWeights.Bold));
                column.HeaderStyle.Setters.Add(new Setter(PaddingProperty, new Thickness(10, 6, 10, 6)));
            }

            
            if (context == "Restaurant")
            {
                DgdAuditLedger.ItemsSource = BillingRepository.RestaurantInvoices;
            }
            else
            {
                DgdAuditLedger.ItemsSource = BillingRepository.RoomInvoices;
            }

            
            UpdateKpiCards();
        }

        private void UpdateKpiCards()
        {
            
            string realSystemDate = $"Date : {DateTime.Now.ToString("yyyy.MM.dd")}";
            TxtKpiRevenueDate.Text = realSystemDate;
            TxtKpiDuesDate.Text = realSystemDate;

            if (currentSubModuleContext == "Restaurant")
            {
                
                var metrics = BillingRepository.GetRestaurantMetrics();

                TxtKpiRevenueValue.Text = $"Rs. {metrics.TotalRevenue:N2}";
                TxtKpiDuesValue.Text = $"Rs. {metrics.TotalDues:N2}";
                TxtKpiAvailLabel.Text = $"{metrics.Available} Tables Available";
                TxtKpiOccLabel.Text = $"{metrics.Occupied} Tables Occupied";
            }
            else 
            {
                
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

                
                string orderID = record.OrderID;
                string state = record.States ?? "Ongoing";
                string tableNum = record.TableNumber ?? "N/A";
                string custName = record.CustomerName ?? "Guest";
                string billAmt = BillingRepository.CalculateTotalBill(orderID).ToString();
                string zone = "";

                bool isDeluxeOrder;

                
                isDeluxeOrder = orderID.StartsWith("DLXCZ") || orderID.StartsWith("DLXFZ");

                
                if (isDeluxeOrder)
                {
                    zone = "Deluxe";
                }
                else
                {
                    
                    zone = "Standard";
                }


                if (state.Equals("Complete", StringComparison.OrdinalIgnoreCase))
                {
                    DynamicBillingWorkspace.Content = new InvoiceArchivedView(clearSidebarCallback, "Restaurant & Reservation Bill", orderID, "Standard Zone", billAmt);
                }
                else
                {
                    DynamicBillingWorkspace.Content = new RestaurantBillSummaryView(null, tableNum, zone, custName, billAmt, orderID, "Billing");
                }
            }
            
            else
            {
                var record = (RoomBillingRecord)DgdAuditLedger.SelectedItem;

                string bookingID = record.BookingID;
                string state = record.States ?? "CheckedIn";
                string roomNum = record.RoomNumber ?? "N/A";
                string category = record.Category ?? "Standard Suite";
                string custName = record.CustomerName ?? "Guest";
                string dueAmt = record.TotalDue.Replace("Rs. ", "").Split('.')[0];

                if (state.Equals("CheckedOut", StringComparison.OrdinalIgnoreCase))
                {
                    DynamicBillingWorkspace.Content = new InvoiceArchivedView(clearSidebarCallback, "Room & Accommodation Bill", bookingID, category, dueAmt);
                }
                else
                {
                    
                    Action releaseRoomCallback = () => {
                        
                        var appShell = Application.Current.MainWindow as HomePageShell;
                        if (appShell != null)
                        {
                            // Clear status tracker inside persistent layout window control natively
                            // This ensures that when switch back to the suite view, its color changes from RED to GREEN!
           
                        }

                        
                        clearSidebarCallback.Invoke();
                    };

                    DynamicBillingWorkspace.Content = new SuiteBillingSummaryView(releaseRoomCallback, roomNum, category, custName, dueAmt);
                }
            }
        } 
    }
}