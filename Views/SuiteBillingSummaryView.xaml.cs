using CabanaOSDemo.Data;
using CabanaOSDemo.Utils;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;


namespace CabanaOSDemo.Views
{
    public partial class SuiteBillingSummaryView : UserControl
    {
        private readonly RoomsAccommodationsView? _mainView;
        private readonly Action _onActionCompleted;
        private readonly string _roomNumber;

        public SuiteBillingSummaryView(Action onActionCompleted, string roomNumber, string category, string guestName, string totalDue)
        {
            InitializeComponent();
            _onActionCompleted = onActionCompleted ?? (() => { });
            _roomNumber = roomNumber ?? "N/A";
            string totaldue = totalDue ?? "Rs. 00.00";

            TxtRoomNumber.Text = _roomNumber;
            TxtCategory.Text = category ?? "Standard Suite";
            TxtCustomerName.Text = string.IsNullOrEmpty(guestName) ? "In-House Guest" : guestName;
            TxtSuiteBookedLabel.Text = $"{category} Occupied";
            TxtBaseCharge.Text = totalDue ?? "Rs. 00.00";
            TxtTotalDue.Text = totalDue ?? "Rs. 00.00";
        }

        private void BtnSettleDue_Click(object sender, RoutedEventArgs e)
        {   
            double totaldue = TxtTotalDue.Text.Replace("Rs. ", "").Trim() == "" ? 0 : double.Parse(TxtTotalDue.Text.Replace("Rs. ", "").Trim());
            // Perform only the data update
            BillingRepository.UpdateRoomRevenue(totaldue);

            string baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folderPath = Path.Combine(baseFolder, "CabanaOS", "Suite Invoices");

            // 3. Ensure the folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // 4. Construct unique file name
            string fileName = $"{getBookingID()}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            string fullPath = Path.Combine(folderPath, fileName);

            // 5. Generate PDF
            try
            {
                PdfInvoiceService.GenerateSuitesInvoice(
                    filePath: fullPath,
                    bookingNumber: $"{getBookingID()}",
                    roomNumber: _roomNumber,
                    suiteCharge: TxtBaseCharge.Text,
                    exCharge: "Rs. 0.00",
                    invoiceDate: DateTime.Now.ToString("yyyy-MM-dd"),
                    totalCharge: TxtTotalDue.Text
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to generate invoice: {ex.Message}");
                return; // Exit if generation fails
            }

            ExecuteGlobalRoomRelease();

            // Show the success message (this is okay)
            MessageBox.Show($"Payment Settled Successfully for Room {_roomNumber}!");

            // REMOVE _onActionCompleted?.Invoke();
            // Do NOT destroy the view here. Let the RoomsView handle it.
        }

        private void BtnReleaseRoom_Click(object sender, RoutedEventArgs e)
        {

            ExecuteGlobalRoomRelease();
            MessageBox.Show($"Room {_roomNumber} returned to vacant pool.", "System Success");
            _onActionCompleted?.Invoke();
            
        }


        private void ExecuteGlobalRoomRelease()
        {
            // 1. Update the Invoice Record (This is what makes the Audit list change)
            var invoice = BillingRepository.RoomInvoices.FirstOrDefault(r => r != null && r.RoomNumber == _roomNumber && r.States == "CheckedIn");
            if (invoice != null)
            {
                invoice.States = "CheckedOut"; // Change state here
            }

            // 2. Update the Room/Suite Status (This is what makes the Room card green/available)
            var suite = BillingRepository.MasterSuites.FirstOrDefault(s => s != null && s.RoomID == _roomNumber);
            if (suite != null)
            {
                suite.States = "CheckedOut"; // Change state here

            }

            // 3. Persist to Database (Crucial step)
            BillingRepository.SaveAllDataToFiles();

            // 4. Force a UI refresh for the Room Card grid
            var appShell = Application.Current.MainWindow as HomePageShell;
            appShell?.GetPersistentRoomsView()?.RefreshActiveRoomsMatrixGrid();
        }

        public string getBookingID()
        {
            var invoice = BillingRepository.RoomInvoices.FirstOrDefault(r => r != null && r.RoomNumber == _roomNumber && r.States == "CheckedIn");
            if (invoice != null)
            {
                return invoice.BookingID;
            }
            else
            {
                return "N/A";
            }
        }
    }

}