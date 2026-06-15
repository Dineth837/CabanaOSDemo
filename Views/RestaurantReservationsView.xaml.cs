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

        // Runtime storage dictionary tracking whether tables are available or occupied
        public Dictionary<string, bool> TableStatusTracker { get; private set; } = new Dictionary<string, bool>();

        public RestaurantReservationsView()
        {
            InitializeComponent();
            InitializeTableDatabaseStates();
            LoadStandardTierZoneFloorMap();
        }

        private void InitializeTableDatabaseStates()
        {
            for (int i = 1; i <= 10; i++) TableStatusTracker[$"ST C{(i < 10 ? "0" + i : "10")}"] = false;
            for (int i = 1; i <= 4; i++) TableStatusTracker[$"ST F0{i}"] = false;
            for (int i = 1; i <= 6; i++) TableStatusTracker[$"DLX C0{i}"] = false;
            for (int i = 1; i <= 2; i++) TableStatusTracker[$"DLX F0{i}"] = false;
        }

        private void ZoneFilterButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton;
            clickedButton = sender as Button;

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
            if (TableStatusTracker.ContainsKey(tableId))
            {
                TableStatusTracker[tableId] = true;
                RefreshActiveFloorMapPresentation();
            }
        }

        public void ReleaseTableRealTime(string tableId)
        {
            if (TableStatusTracker.ContainsKey(tableId))
            {
                TableStatusTracker[tableId] = false;
                RefreshActiveFloorMapPresentation();
                DynamicReservationWorkspace.Content = null;
            }
        }

        private void RefreshActiveFloorMapPresentation()
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
                bool isOccupied = TableStatusTracker[id];
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
                bool isOccupied = TableStatusTracker[id];
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
                bool isOccupied = TableStatusTracker[id];
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
                bool isOccupied = TableStatusTracker[id];
                familyMatrix.Children.Add(CreateGeometricTableComponent(id, "Family (6 Seats)", isOccupied, false, "Deluxe"));
            }
            familyRowPanel.Children.Add(familyMatrix);
            deluxeVerticalStack.Children.Add(familyRowPanel);

            GridFloorLayoutContentArea.Children.Add(deluxeVerticalStack);
        }

        private Button CreateGeometricTableComponent(string name, string seating, bool isOccupied, bool isCircle, string zone)
        {
            //  DECLARE STRICTLY
            string bgHex;
            string borderHex;
            Thickness buttonMargin;
            Button btn;
            Style borderStyle;
            int radiusValue;
            Setter radiusSetter;
            StackPanel textContainer;
            TextBlock txtTitle;
            TextBlock txtSeat;
            StackPanel statusPanel;
            Ellipse dot;
            TextBlock txtStatus;
            Action confirmReservationCallback;
            RestaurantBillSummaryView occupiedSummary;

            //  ASSIGN SEPARATELY
            bgHex = zone == "Deluxe" ? "#FFF4E8" : "#F0FDF4";
            borderHex = zone == "Deluxe" ? "#FCE8CE" : "#22C55E";

            if (isOccupied)
            {
                bgHex = "#FEF2F2";
                borderHex = "#FCA5A5";
            }

            buttonMargin = isCircle ? new Thickness(6, 4, 6, 4) : new Thickness(0, 5, 0, 5);
            if (zone == "Deluxe" && !isCircle) buttonMargin = new Thickness(0, 5, 25, 5);
            if (zone == "Deluxe" && isCircle) buttonMargin = new Thickness(27);

            // Strict object initiation
            btn = new Button();
            btn.Width = isCircle ? 85 : 195;
            btn.Height = isCircle ? 85 : 82;
            btn.Margin = buttonMargin;
            btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(bgHex));
            btn.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(borderHex));
            btn.BorderThickness = new Thickness(1.5);
            btn.Cursor = System.Windows.Input.Cursors.Hand;

            borderStyle = new Style(typeof(Border));
            radiusValue = isCircle ? 43 : 16;
            radiusSetter = new Setter(Border.CornerRadiusProperty, new CornerRadius(radiusValue));
            borderStyle.Setters.Add(radiusSetter);
            btn.Resources.Add(typeof(Border), borderStyle);

            textContainer = new StackPanel();
            textContainer.VerticalAlignment = VerticalAlignment.Center;

            txtTitle = new TextBlock();
            txtTitle.Text = name;
            txtTitle.FontWeight = FontWeights.Bold;
            txtTitle.FontSize = 11;
            txtTitle.HorizontalAlignment = HorizontalAlignment.Center;
            txtTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B"));

            txtSeat = new TextBlock();
            txtSeat.Text = seating;
            txtSeat.FontSize = 7.5;
            txtSeat.HorizontalAlignment = HorizontalAlignment.Center;
            txtSeat.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748B"));
            txtSeat.Margin = new Thickness(0, 1, 0, 2);

            statusPanel = new StackPanel();
            statusPanel.Orientation = Orientation.Horizontal;
            statusPanel.HorizontalAlignment = HorizontalAlignment.Center;

            dot = new Ellipse();
            dot.Width = 4.5;
            dot.Height = 4.5;
            dot.Fill = isOccupied ? Brushes.Red : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#22C55E"));
            dot.VerticalAlignment = VerticalAlignment.Center;
            dot.Margin = new Thickness(0, 0, 3, 0);

            txtStatus = new TextBlock();
            txtStatus.Text = isOccupied ? "Not Available" : "Available";
            txtStatus.FontSize = 8.5;
            txtStatus.FontWeight = FontWeights.Bold;
            txtStatus.Foreground = isOccupied ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B91C1C")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#15803D"));

            statusPanel.Children.Add(dot);
            statusPanel.Children.Add(txtStatus);

            textContainer.Children.Add(txtTitle);
            textContainer.Children.Add(txtSeat);
            textContainer.Children.Add(statusPanel);
            btn.Content = textContainer;

            btn.Click += (sender, e) =>
            {
                this.currentActiveZone = zone;
                string tableType = isCircle ? "Couple (2 Seats)" : "Family (6 Seats)";

                //  The reservation is completely free!
                string priceLabel = "Rs. 0.00";

                if (isOccupied)
                {
                    occupiedSummary = new RestaurantBillSummaryView(this, name, zone, "In-House Resident Guest", priceLabel);
                    DynamicReservationWorkspace.Content = occupiedSummary;
                }
                else
                {
                    confirmReservationCallback = () =>
                    {
                        // DECLARE STRICTLY FOR CALLBACK
                        FormTableBookingView activeForm;
                        string leadName;
                        string leadNic;
                        string orderIdToken;
                        RestaurantBillingRecord existingRecord;
                        RestaurantBillingRecord newRecord;
                        RestaurantBillSummaryView newSummary;

                        // 2. ASSIGN VALUES
                        activeForm = DynamicReservationWorkspace.Content as FormTableBookingView;

                        if (activeForm != null)
                        {
                            leadName = activeForm.TxtLeadGuestName.Text.Trim();
                            leadNic = activeForm.TxtLeadGuestNIC.Text.Trim();

                            if (!ValidationHelper.IsValidName(leadName))
                            {
                                MessageBox.Show("Please enter a valid guest name layout parameters.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                            if (!ValidationHelper.IsValidNIC(leadNic))
                            {
                                MessageBox.Show("Invalid Sri Lankan National Identity Card layout format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }

                            orderIdToken = IdGenerator.GenerateRestaurantReservationID(zone, tableType, leadNic);

                            foreach (var table in BillingRepository.MasterTables)
                            {
                                if (table != null && table.TableID == name)
                                {
                                    table.States = "Ongoing";
                                    break;
                                }
                            }

                            //  PREVENT DUPLICATES: Search list for an existing active record first
                            existingRecord = BillingRepository.RestaurantInvoices.FirstOrDefault(r => r.TableNumber == name && (r.States == "Occupied" || r.States == "Ongoing"));

                            if (existingRecord != null)
                            {
                                // UPDATE EXISTING
                                existingRecord.CustomerName = leadName;
                                existingRecord.States = "Ongoing";
                                existingRecord.TotalBill = priceLabel;
                            }
                            else
                            {
                                // CREATE NEW (Strict Assignment)
                                newRecord = new RestaurantBillingRecord();
                                newRecord.OrderID = orderIdToken;
                                newRecord.TableNumber = name;
                                newRecord.CustomerName = leadName;
                                newRecord.Date = DateTime.Now.ToString("yyyy.MM.dd");
                                newRecord.States = "Ongoing";
                                newRecord.TotalBill = priceLabel;

                                BillingRepository.RestaurantInvoices.Add(newRecord);
                            }

                            BillingRepository.SaveAllDataToFiles();
                            this.BookTableRealTime(name);

                            newSummary = new RestaurantBillSummaryView(this, name, zone, leadName, priceLabel);
                            this.DynamicReservationWorkspace.Content = newSummary;
                        }
                    };

                    FormTableBookingView bookingView;
                    bookingView = new FormTableBookingView(confirmReservationCallback, name, zone, tableType);
                    DynamicReservationWorkspace.Content = bookingView;
                }
            };
            return btn;
        }
    }
}