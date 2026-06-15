using CabanaOSDemo.Data;
using CabanaOSDemo.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace CabanaOSDemo.Views
{
    public partial class RoomsAccommodationsView : UserControl
    {
        private string currentActiveTier = "Standard";

        public RoomsAccommodationsView()
        {
            InitializeComponent();
            PopulateStandardTierRooms();
            LoadCleanFormWorkspace("ST01", "Standard");
          
        }

        
        // This removes the need for the dangerous RoomStatusTracker dictionary
        private bool IsRoomReserved(string roomId)
        {
            var suite = BillingRepository.MasterSuites.FirstOrDefault(s => s != null && s.RoomID == roomId);
            return suite != null && suite.States == "CheckedIn";
        }

        public void ReleaseRoomRealTime(string roomNumber)
        {
            // 1. Refresh the grid immediately
            RefreshActiveRoomsMatrixGrid();

            // 2. Use a "Dispatcher.Invoke" to clear the workspace. 
            // This delay ensures the UI thread is not busy rendering the grid 
            // when the workspace is cleared.
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                DynamicFormWorkspace.Content = null;
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        public void RefreshActiveRoomsMatrixGrid()
        {
            if (currentActiveTier == "Superior") PopulateSuperiorTierRooms();
            else if (currentActiveTier == "Deluxe") PopulateDeluxeTierRooms();
            else PopulateStandardTierRooms();
        }

        private void LoadCleanFormWorkspace(string roomNumber, string tier)
        {
            Action onRegistrationSuccessCallback = () =>
            {
                var suite = BillingRepository.MasterSuites.FirstOrDefault(s => s != null && s.RoomID == roomNumber);
                if (suite != null) suite.States = "CheckedIn";

                BillingRepository.UpdateSuiteStatus(roomNumber, "CheckedIn");
                RefreshActiveRoomsMatrixGrid();
                DynamicFormWorkspace.Content = new InvoiceConfirmedView(this,roomNumber, tier);
            };

            if (roomNumber.Contains("Fam"))
                DynamicFormWorkspace.Content = new FormFamilyView(onRegistrationSuccessCallback, roomNumber, tier);
            else
                DynamicFormWorkspace.Content = new FormCoupleView(onRegistrationSuccessCallback, roomNumber, tier);
        }

        private void TierFilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                // Reset tab styles
                BtnStandardTier.Background = BtnSuperiorTier.Background = BtnDeluxeTier.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E8F0"));
                BtnStandardTier.Foreground = BtnSuperiorTier.Foreground = BtnDeluxeTier.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B"));

                clickedButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#111827"));
                clickedButton.Foreground = Brushes.White;
                clickedButton.FontWeight = FontWeights.Bold;

                if (clickedButton == BtnSuperiorTier) 
                { 
                    currentActiveTier = "Superior"; 
                    PopulateSuperiorTierRooms(); 
                    LoadCleanFormWorkspace("SUP01", "Superior");
                    TxtMiddleSuiteTitle.Text = "Superior Tier Suites";
              
                    TxtMiddleSuiteTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A"));
                    //BdrMiddleShowcaseCard.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ADC4F7"));
                    TxtMiddleSuiteDescription.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A"));
                }
                else if (clickedButton == BtnDeluxeTier) 
                { 
                    currentActiveTier = "Deluxe"; 
                    PopulateDeluxeTierRooms(); 
                    LoadCleanFormWorkspace("DLX01", "Deluxe");
                    TxtMiddleSuiteTitle.Text = "Deluxe Tier Suites";
                 
                    TxtMiddleSuiteTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));
                    //BdrMiddleShowcaseCard.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FCE8CE"));
                    TxtMiddleSuiteDescription.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));
                }
                else 
                { 
                    currentActiveTier = "Standard"; 
                    PopulateStandardTierRooms(); 
                    LoadCleanFormWorkspace("ST01", "Standard");
                    TxtMiddleSuiteTitle.Text = "Standard Tier Suites";
                    
                    TxtMiddleSuiteTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));
                    //BdrMiddleShowcaseCard.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#86EFAC"));
                    TxtMiddleSuiteDescription.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));
                }
            }
        }

        // 🚀 CORRECTED POPULATION METHODS: No dictionary reliance
        public void PopulateStandardTierRooms()
        {
            RoomsGridMatrixDisplay.Children.Clear();
            // Standard Couple (ST01 - ST06)
            for (int i = 1; i <= 6; i++) { string id = $"ST0{i}"; RoomsGridMatrixDisplay.Children.Add(CreateRoomCardUiComponent(id, "Rs. 8,500 / night", IsRoomReserved(id), "Standard")); }
            // Standard Family (Fam ST07 - Fam ST010)
            for (int i = 7; i <= 10; i++) { string id = $"Fam ST0{i}"; RoomsGridMatrixDisplay.Children.Add(CreateRoomCardUiComponent(id, "Rs. 15,000 / night", IsRoomReserved(id), "Standard")); }
        }

        public void PopulateSuperiorTierRooms()
        {
            RoomsGridMatrixDisplay.Children.Clear();
            for (int i = 1; i <= 6; i++) { string id = $"SUP0{i}"; RoomsGridMatrixDisplay.Children.Add(CreateRoomCardUiComponent(id, "Rs. 12,500 / night", IsRoomReserved(id), "Superior")); }
        }

        public void PopulateDeluxeTierRooms()
        {
            RoomsGridMatrixDisplay.Children.Clear();
            for (int i = 1; i <= 6; i++) { string id = $"DLX0{i}"; RoomsGridMatrixDisplay.Children.Add(CreateRoomCardUiComponent(id, "Rs. 20,000 / night", IsRoomReserved(id), "Deluxe")); }
        }

        private Button CreateRoomCardUiComponent(string roomName, string price, bool isReserved, string tierMode)
        {
            string bgHex = "#F0FDF4"; string borderHex = "#86EFAC";
            if (tierMode == "Superior") { bgHex = "#F0F4FF"; borderHex = "#ADC4F7"; }
            else if (tierMode == "Deluxe") { bgHex = "#FFF4E8"; borderHex = "#FCE8CE"; }

            Button cardBtn = new Button
            {
                Height = 95,
                Margin = new Thickness(6),
                Background = isReserved ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEF2F2")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString(bgHex)),
                BorderBrush = isReserved ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FCA5A5")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString(borderHex)),
                BorderThickness = new Thickness(1.5),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            Style borderStyle = new Style(typeof(Border)); borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(16))); cardBtn.Resources.Add(typeof(Border), borderStyle);

            StackPanel mainStack = new StackPanel { VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(15, 0, 0, 0) };
            string displayTitle = roomName.Contains("Fam") ? roomName.Replace("Fam ", "| Fam ") : roomName;
            mainStack.Children.Add(new TextBlock { Text = $"Room {displayTitle}", FontWeight = FontWeights.Bold, FontSize = 14, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B")) });

            StackPanel statusStack = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 4, 0, 6) };
            statusStack.Children.Add(new Ellipse { Width = 6, Height = 6, Fill = isReserved ? Brushes.Red : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#22C55E")), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 6, 0) });
            statusStack.Children.Add(new TextBlock { Text = isReserved ? "Reserved" : "Available", FontSize = 12, FontWeight = FontWeights.Bold, Foreground = isReserved ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B91C1C")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#15803D")) });
            mainStack.Children.Add(statusStack);

            mainStack.Children.Add(new TextBlock { Text = price, FontSize = 12, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#475569")), FontWeight = FontWeights.Medium });
            cardBtn.Content = mainStack;

            cardBtn.Click += (sender, e) => { if (isReserved) DynamicFormWorkspace.Content = new InvoiceConfirmedView(this,roomName, tierMode); else LoadCleanFormWorkspace(roomName, tierMode); };
            return cardBtn;
        }

        

    }
}