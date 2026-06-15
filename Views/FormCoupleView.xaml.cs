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
    public partial class FormCoupleView : UserControl
    {
        private readonly Action _onConfirmedCallback;

        public FormCoupleView(Action onConfirmedCallback, string roomNumber, string tier)
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
            if (TxtRoomPackageTitle == null || TxtCostPerNight == null || BtnConfirm == null || BtnCancle == null) return;

            if (tier == "Superior")
            {
                TxtRoomPackageTitle.Text = "Superior Suite";
                TxtCostPerNight.Text = "Rs. 12,500.00";
                BtnConfirm.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ADC4F7"));
                BtnConfirm.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A"));
                BtnCancle.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ADC4F7"));
                BtnCancle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A"));
                TxtTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A"));
                TxtRoomPackageTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A"));
                TxtCostPerNight.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A"));
                TxtRegTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A"));
            }
            else if (tier == "Deluxe")
            {
                TxtRoomPackageTitle.Text = "Deluxe Suite";
                TxtCostPerNight.Text = "Rs. 20,000.00";
                BtnConfirm.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FCE8CE"));
                BtnConfirm.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));
                BtnCancle.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FCE8CE"));
                BtnCancle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));
                TxtTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));
                TxtRoomPackageTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));
                TxtCostPerNight.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));
                TxtRegTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));
            }
            else
            {
                TxtRoomPackageTitle.Text = "Standard Suite";
                TxtCostPerNight.Text = "Rs. 8,500.00";
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
            // 1. Declare all variables first
            string roomNumStr;
            string leadGuestName;
            string leadGuestNic;
            string companionGuestName;
            string companionGuestNic;
            string parsedTier;
            

            // 2. Assign values from UI in separate steps
            roomNumStr = TxtRoomNumberDisplayBox.Text.Trim();
            leadGuestName = txtLeadGuestName.Text.Trim();
            leadGuestNic = txtLeadGuestNIC.Text.Trim();
            companionGuestName = txtCompanionGuestName.Text.Trim();
            companionGuestNic = txtCompanionGuestNIC.Text.Trim();

            // 3. Validation Guards for Lead Guest
            if (!ValidationHelper.IsValidName(leadGuestName))
            {
                MessageBox.Show("Validation Failure: Guest Name can only contain alphabet letters and spaces.", "Input Gate Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                clearFields();
                return;
            }

            if (!ValidationHelper.IsValidNIC(leadGuestNic))
            {
                MessageBox.Show("Validation Failure: Please enter a valid Sri Lankan NIC format string.", "Input Gate Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                clearFields();
                return;
            }

            // 4. Validation Guards for Companion Guest
            if (!ValidationHelper.IsValidName(companionGuestName))
            {
                MessageBox.Show("Validation Failure: Guest Name can only contain alphabet letters and spaces.", "Input Gate Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                clearFields();
                return;
            }

            if (!ValidationHelper.IsValidNIC(companionGuestNic))
            {
                MessageBox.Show("Validation Failure: Please enter a valid Sri Lankan NIC format string.", "Input Gate Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                clearFields();
                return;
            }

            // 5. Determine Tier
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
            BillingRepository.EnsureCustomerRegistered(leadName, leadNic, "Couple", leadCusID);

            string comName=txtCompanionGuestName.Text.Trim();
            string comNic=txtCompanionGuestNIC.Text.Trim();
            string comCusID = IdGenerator.GenerateCustomerID(comNic);
            BillingRepository.EnsureCustomerRegistered(comName, comNic, "Couple", comCusID);



            string bookingID = IdGenerator.GenerateSuiteBookingID(parsedTier, "Couple", leadGuestNic);


            RoomBillingRecord newRecord = new RoomBillingRecord();
            newRecord = new RoomBillingRecord();
            newRecord.RoomNumber = roomNumStr;
            newRecord.CustomerName = leadGuestName;
            newRecord.Category = parsedTier + "C Suite";
            newRecord.Date = DateTime.Now.ToString("yyyy.MM.dd");
            newRecord.States = "CheckedIn";
            newRecord.TotalDue = TxtCostPerNight.Text;
            newRecord.BookingID = bookingID;

            // 8. Save to memory and update state
            BillingRepository.RoomInvoices.Add(newRecord);

            var currentSuite = BillingRepository.MasterSuites.FirstOrDefault(s => s != null && s.RoomID == roomNumStr);
            if (currentSuite != null)
            {
                currentSuite.States = "CheckedIn";
            }

            // 9. Persist to files
            BillingRepository.SaveAllDataToFiles();

            // 10. Success State
            MessageBox.Show($"Reservation Processed Successfully!\n\nBooking ID: {bookingID}", "Terminal Receipt", MessageBoxButton.OK, MessageBoxImage.Information);

            _onConfirmedCallback?.Invoke();
        }

        private void Button_Click(object sender, RoutedEventArgs e) { }

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
            txtCompanionGuestName.Clear();
            txtCompanionGuestNIC.Clear();
        }
    }
}