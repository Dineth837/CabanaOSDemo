using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CabanaOSDemo.Data;
using CabanaOSDemo.Models;
using CabanaOSDemo.Utils;

namespace CabanaOSDemo.Views
{
    public partial class FormFamilyView : UserControl
    {
        private readonly Action _onConfirmedCallback;

        // Overloaded constructor accepting cross talk data from rooms grid layout views
        public FormFamilyView(Action onConfirmedCallback, string roomNumber, string tier)
        {
            InitializeComponent();
            _onConfirmedCallback = onConfirmedCallback;

            if (TxtRoomNumberDisplayBox != null)
            {
                TxtRoomNumberDisplayBox.Text = roomNumber;
            }

            ConfigureTierStyling(tier);
        }



        private void ConfigureTierStyling(string tier)
        {
            if (TxtRoomPackageTitle == null || TxtCostPerNight == null) return;

            if (tier == "Standard")
            {
                TxtRoomPackageTitle.Text = "Standard Suite";
                TxtCostPerNight.Text = "Rs. 15,000.00";
                BtnConfirm.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A7F3D0"));
                BtnConfirm.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));
                BtnCancle.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A7F3D0"));
                BtnCancle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));
                TxtTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));
                TxtRoomPackageTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));
                TxtCostPerNight.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));
                TxtRegTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));

            }
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            //  Declare variables
            string roomNumStr;
            string leadGuestName;
            string leadGuestNic;
            string guestCount;
            string parsedTier;
            string generatedId;

            //  Assign initial values from UI
            roomNumStr = TxtRoomNumberDisplayBox.Text.Trim();
            leadGuestName = txtLeadGuestName.Text.Trim();
            leadGuestNic = txtLeadGuestNIC.Text.Trim();
            guestCount = txtGuestCount.Text.Trim();

            //  Validation Guards
            if (!ValidationHelper.IsValidName(leadGuestName))
            {
                MessageBox.Show("Validation Failure: Guest Name must only contain letters and spaces.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                clearFields();
                return;
            }
            if (!ValidationHelper.IsValidNIC(leadGuestNic))
            {
                MessageBox.Show("Validation Failure: Please enter a valid Sri Lankan NIC number.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                clearFields();
                return;
            }

            //  Determine Tier
            parsedTier = "Standard";
            if (TxtRoomPackageTitle.Text.Contains("Superior"))
            {
                parsedTier = "Superior";
            }
            else if (TxtRoomPackageTitle.Text.Contains("Deluxe"))
            {
                parsedTier = "Deluxe";
            }

            string leadName = txtLeadGuestName.Text.Trim();
            string leadNic = txtLeadGuestNIC.Text.Trim();
            string leadCusID = IdGenerator.GenerateCustomerID(leadNic);
            BillingRepository.EnsureCustomerRegistered(leadName, leadNic, "Family", leadCusID);

            //  Generate ID
            string bookingID = IdGenerator.GenerateSuiteBookingID(parsedTier, "Family", leadGuestNic);

            //  Instantiate and manually assign billing record properties in separate steps
            RoomBillingRecord newRecord = new RoomBillingRecord();
            newRecord.RoomNumber = roomNumStr;
            newRecord.CustomerName = leadGuestName;
            newRecord.Category = $"{parsedTier} F Suite";
            newRecord.Date = DateTime.Now.ToString("yyyy.MM.dd");
            newRecord.States = "CheckedIn";
            newRecord.TotalDue = TxtCostPerNight.Text;
            newRecord.BookingID = bookingID;

            //  Save to C# Memory Repository
            BillingRepository.RoomInvoices.Add(newRecord);

            var currentSuite = BillingRepository.MasterSuites.FirstOrDefault(s => s != null && s.RoomID == roomNumStr);
            if (currentSuite != null)
            {
                currentSuite.States = "CheckedIn";
            }

            //  Commit C# Memory to Database/Files
            BillingRepository.SaveAllDataToFiles();

            
            MessageBox.Show($"Family Reservation Successful!\n\nBooking ID: {bookingID}", "System Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);

            _onConfirmedCallback?.Invoke();
        }

        private System.Collections.Generic.IEnumerable<T> FindVisualChildren<T>(DependencyObject? depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T) yield return (T)child;
                    foreach (T childOfChild in FindVisualChildren<T>(child)) yield return childOfChild;
                }
            }
        }

        private void BtnCancle_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
            TxtRoomNumberDisplayBox.Text = "";
        }

        public void clearFields()
        {
            txtLeadGuestName.Clear();
            txtLeadGuestNIC.Clear();
            txtGuestCount.Clear();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}