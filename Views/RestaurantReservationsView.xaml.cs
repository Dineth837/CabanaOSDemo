using CabanaOSDemo.Data;
using CabanaOSDemo.Models;
using CabanaOSDemo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CabanaOSDemo.Views
{
    public partial class RestaurantReservationsView : UserControl
    {
        private string currentActiveZone = "Standard";

        public RestaurantReservationsView()
        {
            InitializeComponent();
            RefreshActiveFloorMapPresentation();
        }

        // 🚀 FIX: Removed TableStatusTracker entirely. Using repository source of truth.
        private bool IsTableOccupied(string tableId)
        {
            var table = BillingRepository.MasterTables.FirstOrDefault(t => t != null && t.TableID == tableId);
            return table != null && table.States == "Ongoing";
        }

        public string getCustomerName(string tableId)
        {
            var suite = BillingRepository.RestaurantInvoices.FirstOrDefault(s => s != null && s.TableNumber == tableId);
            return suite?.CustomerName ?? string.Empty;
        }

        private void ZoneFilterButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                BtnStandardZone.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E8F0"));
                BtnStandardZone.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B"));
                BtnDeluxeZone.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E8F0"));
                BtnDeluxeZone.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B"));

                clickedButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#111827"));
                clickedButton.Foreground = Brushes.White;

                if (clickedButton == BtnDeluxeZone)
                {
                    currentActiveZone = "Deluxe";
                    LoadDeluxeTierZoneFloorMap();
                }
                else
                {
                    currentActiveZone = "Standard";
                    LoadStandardTierZoneFloorMap();
                }
            }
        }

        public void BookTableRealTime(string tableId)
        {
            // 🚀 FIX: Update database and memory via repository
            BillingRepository.UpdateTableStatus(tableId, "Ongoing");
            RefreshActiveFloorMapPresentation();
        }

        public void ReleaseTableRealTime(string tableId)
        {
            // 🚀 FIX: Update database and memory via repository
            BillingRepository.UpdateTableStatus(tableId, "Complete");
            RefreshActiveFloorMapPresentation();
            DynamicReservationWorkspace.Content = null;
        }

        public void RefreshActiveFloorMapPresentation()
        {
            if (currentActiveZone == "Standard") LoadStandardTierZoneFloorMap();
            else LoadDeluxeTierZoneFloorMap();
        }

        private void LoadStandardTierZoneFloorMap()
        {
            GridFloorLayoutContentArea.Children.Clear();

            Grid standardSplitGrid = new Grid { HorizontalAlignment = HorizontalAlignment.Stretch };
            standardSplitGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            standardSplitGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Grid coupleContainerGrid = new Grid();
            coupleContainerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coupleContainerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            TextBlock coupleHeader = new TextBlock { Text = "Couple Seating Arrangement", FontSize = 14, FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748B")), Margin = new Thickness(5, 0, 0, 12) };
            Grid.SetRow(coupleHeader, 0);
            coupleContainerGrid.Children.Add(coupleHeader);

            UniformGrid coupleMatrixGrid = new UniformGrid { Columns = 2, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };
            for (int i = 1; i <= 10; i++)
            {
                string id = i < 10 ? $"ST C0{i}" : "ST C10";
                bool isOccupied = IsTableOccupied(id);
                coupleMatrixGrid.Children.Add(CreateGeometricTableComponent(id, "Couple (2 Seats)", isOccupied, true, "Standard"));
            }
            Grid.SetRow(coupleMatrixGrid, 1);
            coupleContainerGrid.Children.Add(coupleMatrixGrid);
            Grid.SetColumn(coupleContainerGrid, 0);
            standardSplitGrid.Children.Add(coupleContainerGrid);

            Grid familyContainerGrid = new Grid { Margin = new Thickness(10, 0, 0, 0) };
            familyContainerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            familyContainerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            TextBlock familyHeader = new TextBlock { Text = "Family Seating Arrangement", FontSize = 14, FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748B")), Margin = new Thickness(5, 0, 0, 12) };
            Grid.SetRow(familyHeader, 0);
            familyContainerGrid.Children.Add(familyHeader);

            UniformGrid familyMatrixGrid = new UniformGrid { Columns = 1, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };
            for (int i = 1; i <= 4; i++)
            {
                string id = $"ST F0{i}";
                bool isOccupied = IsTableOccupied(id);
                familyMatrixGrid.Children.Add(CreateGeometricTableComponent(id, "Family (6 Seats)", isOccupied, false, "Standard"));
            }
            Grid.SetRow(familyMatrixGrid, 1);
            familyContainerGrid.Children.Add(familyMatrixGrid);
            Grid.SetColumn(familyContainerGrid, 1);
            standardSplitGrid.Children.Add(familyContainerGrid);

            GridFloorLayoutContentArea.Children.Add(standardSplitGrid);
        }

        private void LoadDeluxeTierZoneFloorMap()
        {
            GridFloorLayoutContentArea.Children.Clear();
            StackPanel deluxeVerticalStack = new StackPanel { VerticalAlignment = VerticalAlignment.Top };

            StackPanel coupleRowPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 25) };
            coupleRowPanel.Children.Add(new TextBlock { Text = "Couple Seating Arrangement", FontSize = 14, FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748B")), Margin = new Thickness(5, 0, 0, 12) });

            UniformGrid coupleMatrix = new UniformGrid { Columns = 3, HorizontalAlignment = HorizontalAlignment.Left };
            for (int i = 1; i <= 6; i++)
            {
                string id = $"DLX C0{i}";
                bool isOccupied = IsTableOccupied(id);
                coupleMatrix.Children.Add(CreateGeometricTableComponent(id, "Couple (2 Seats)", isOccupied, true, "Deluxe"));
            }
            coupleRowPanel.Children.Add(coupleMatrix);
            deluxeVerticalStack.Children.Add(coupleRowPanel);

            StackPanel familyRowPanel = new StackPanel();
            familyRowPanel.Children.Add(new TextBlock { Text = "Family Seating Arrangement", FontSize = 14, FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748B")), Margin = new Thickness(5, 0, 0, 12) });

            UniformGrid familyMatrix = new UniformGrid { Columns = 2, HorizontalAlignment = HorizontalAlignment.Left };
            for (int i = 1; i <= 2; i++)
            {
                string id = $"DLX F0{i}";
                bool isOccupied = IsTableOccupied(id);
                familyMatrix.Children.Add(CreateGeometricTableComponent(id, "Family (6 Seats)", isOccupied, false, "Deluxe"));
            }
            familyRowPanel.Children.Add(familyMatrix);
            deluxeVerticalStack.Children.Add(familyRowPanel);

            GridFloorLayoutContentArea.Children.Add(deluxeVerticalStack);
        }

        

        private Button CreateGeometricTableComponent(string name, string seating, bool isOccupied, bool isCircle, string zone)
        {
            string bgHex = zone == "Deluxe" ? "#FFF4E8" : "#F0FDF4";
            string borderHex = zone == "Deluxe" ? "#FCE8CE" : "#22C55E";

            if (isOccupied)
            {
                bgHex = "#FEF2F2";
                borderHex = "#FCA5A5";
            }

            Thickness buttonMargin = isCircle ? new Thickness(6, 4, 6, 4) : new Thickness(0, 5, 0, 5);
            if (zone == "Deluxe" && !isCircle) buttonMargin = new Thickness(0, 5, 25, 5);
            if (zone == "Deluxe" && isCircle) buttonMargin = new Thickness(27);

            Button btn = new Button();
            btn.Width = isCircle ? 85 : 195;
            btn.Height = isCircle ? 85 : 82;
            btn.Margin = buttonMargin;
            btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(bgHex));
            btn.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(borderHex));
            btn.BorderThickness = new Thickness(1.5);
            btn.Cursor = System.Windows.Input.Cursors.Hand;

            Style borderStyle = new Style(typeof(Border));
            borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(isCircle ? 43 : 16)));
            btn.Resources.Add(typeof(Border), borderStyle);

            StackPanel textContainer = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
            textContainer.Children.Add(new TextBlock { Text = name, FontWeight = FontWeights.Bold, FontSize = 11, HorizontalAlignment = HorizontalAlignment.Center, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B")) });
            textContainer.Children.Add(new TextBlock { Text = seating, FontSize = 7.5, HorizontalAlignment = HorizontalAlignment.Center, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748B")), Margin = new Thickness(0, 1, 0, 2) });

            StackPanel statusPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            statusPanel.Children.Add(new Ellipse { Width = 4.5, Height = 4.5, Fill = isOccupied ? Brushes.Red : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#22C55E")), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 3, 0) });
            statusPanel.Children.Add(new TextBlock { Text = isOccupied ? "Not Available" : "Available", FontSize = 8.5, FontWeight = FontWeights.Bold, Foreground = isOccupied ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B91C1C")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#15803D")) });

            textContainer.Children.Add(statusPanel);
            btn.Content = textContainer;

            btn.Click += (sender, e) =>
            {
                this.currentActiveZone = zone;
                string tableType = isCircle ? "Couple (2 Seats)" : "Family (6 Seats)";
                string priceLabel = "Rs. 0.00";

                if (isOccupied)
                {
                    DynamicReservationWorkspace.Content = new RestaurantBillSummaryView(this, name, zone, getCustomerName(name), priceLabel);
                }
                else
                {
                    Action confirmReservationCallback = () =>
                    {
                        FormTableBookingView activeForm = DynamicReservationWorkspace.Content as FormTableBookingView;
                        if (activeForm != null)
                        {
                            string leadName = activeForm.TxtLeadGuestName.Text.Trim();
                            string leadNic = activeForm.TxtLeadGuestNIC.Text.Trim();

                            if (!ValidationHelper.IsValidName(leadName) || !ValidationHelper.IsValidNIC(leadNic))
                            {
                                MessageBox.Show("Please enter valid guest details.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }

                            string orderIdToken = IdGenerator.GenerateRestaurantReservationID(zone, tableType, leadNic);
                            RestaurantBillingRecord newRecord = new RestaurantBillingRecord
                            {
                                OrderID = orderIdToken,
                                TableNumber = name,
                                CustomerName = leadName,
                                Date = DateTime.Now.ToString("yyyy.MM.dd"),
                                States = "Ongoing",
                                TotalBill = priceLabel
                            };

                            BillingRepository.RestaurantInvoices.Add(newRecord);
                            BillingRepository.SaveAllDataToFiles();
                            this.BookTableRealTime(name);
                            DynamicReservationWorkspace.Content = new RestaurantBillSummaryView(this, name, zone, leadName, priceLabel);
                        }
                    };
                    DynamicReservationWorkspace.Content = new FormTableBookingView(confirmReservationCallback, name, zone, tableType);
                }
            };
            return btn;
        }
    }
}