using CabanaOSDemo.Data;
using CabanaOSDemo.Models;
using CabanaOSDemo.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CabanaOSDemo.Views
{
    public partial class FormTableBookingView : UserControl
    {
        //  Declare the action delegate field here so all methods can access it
        private readonly Action _onConfirmedCallback;

        //  Update constructor signature to accept the incoming Action parameter
        public FormTableBookingView(Action onConfirmedCallback, string tableName, string zone, string type)
        {
            InitializeComponent();

            // Save it into our class field variable
            _onConfirmedCallback = onConfirmedCallback;

            TxtTableNumberLabel.Text = tableName;
            TxtZoneLabel.Text = zone + " Zone";
            TxtTypeLabel.Text = type;

            if (type.Contains("Family"))
            {
                TxtTotalGuestHeader.Visibility = Visibility.Visible;
                TxtTotalGuestInput.Visibility = Visibility.Visible;
            }

            if (zone == "Deluxe")
            {
                BtnConfirm.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEF08A"));
                BtnConfirm.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#713F12"));

                BtnCancel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEF08A"));
                BtnCancel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#713F12"));

                TxtTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#713F12"));
                TxtRegTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#713F12"));
                TxtTableNumberLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#713F12"));
                TxtNumberLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#713F12"));
                TxtZoneLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#713F12"));
                TxtTypeLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#713F12"));


            }
            if (zone == "Standard")
            {
                BtnConfirm.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BBF7D0"));
                BtnConfirm.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14532D"));

                BtnCancel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BBF7D0"));
                BtnCancel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14532D"));

                TxtRegTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14532D"));
                TxtRegTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14532D"));
                TxtTableNumberLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14532D"));
                TxtNumberLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14532D"));
                TxtTypeLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14532D"));
                TxtZoneLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14532D"));
            }
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            //  Declare all variables first
            string tableNumStr;
            string leadGuestName;
            string leadGuestNic;
            string guestCountStr;
            string tableZone;
            string tableType;
            string generatedId;
            RestaurantBillingRecord newRecord; // Note: Adjust class name if yours is slightly different

            //  Assign values from UI in separate steps
            // Removing the " Zone" suffix if it was added in the constructor
            tableNumStr = TxtTableNumberLabel.Text.Trim();
            tableZone = TxtZoneLabel.Text.Replace(" Zone", "").Trim();
            tableType = TxtTypeLabel.Text.Trim();

            leadGuestName = TxtLeadGuestName.Text.Trim();
            leadGuestNic = TxtLeadGuestNIC.Text.Trim();
            guestCountStr = TxtTotalGuestInput.Text.Trim();

            //  Validation Guards for Lead Guest
            if (!ValidationHelper.IsValidName(leadGuestName))
            {
                MessageBox.Show("Validation Failure: Guest Name can only contain alphabet letters and spaces.", "Input Gate Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (TxtLeadGuestName != null) TxtLeadGuestName.Clear();
                return;
            }

            if (!ValidationHelper.IsValidNIC(leadGuestNic))
            {
                MessageBox.Show("Validation Failure: Please enter a valid Sri Lankan NIC format string.", "Input Gate Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (TxtLeadGuestNIC != null) TxtLeadGuestNIC.Clear();
                return;
            }

            //  Validation Guard for Family Table Guest Count
            if (tableType.Contains("Family"))
            {
                if (string.IsNullOrWhiteSpace(guestCountStr))
                {
                    MessageBox.Show("Validation Failure: Please enter the total number of guests for this family table.", "Input Gate Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            string reservationID = IdGenerator.GenerateRestaurantReservationID(tableZone, tableType, leadGuestNic);

            string leadCusID = IdGenerator.GenerateCustomerID(leadGuestNic);

            BillingRepository.EnsureCustomerRegistered(leadGuestName, leadGuestNic, "Table", leadCusID);

            //  Instantiate and manually assign billing record properties one by one
            newRecord = new RestaurantBillingRecord();
            newRecord.OrderID = reservationID;
            newRecord.TableNumber = tableNumStr;
            newRecord.CustomerName = leadGuestName;
            newRecord.Date = DateTime.Now.ToString("yyyy.MM.dd");
            newRecord.States = "Occupied";
            newRecord.TotalBill = "0.00"; // Starts at 0 until they order food

            //  Save to memory and update state
            BillingRepository.RestaurantInvoices.Add(newRecord);

            var currentTable = BillingRepository.MasterTables.FirstOrDefault(t => t != null && t.TableID == tableNumStr);
            if (currentTable != null)
            {
                currentTable.States = "Occupied";
            }

            //  Persist to files
            BillingRepository.SaveAllDataToFiles();

            //  Success State
            MessageBox.Show($"Table Reservation Processed Successfully!\n\nOrder ID: {reservationID}", "Terminal Receipt", MessageBoxButton.OK, MessageBoxImage.Information);

            // Execute Callback (using the traditional null check approach you established)
            if (_onConfirmedCallback != null)
            {
                _onConfirmedCallback.Invoke();
            }
        }

        private void TxtLeadGuestName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}