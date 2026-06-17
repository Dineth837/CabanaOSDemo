using CabanaOSDemo.Utils;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace CabanaOSDemo.Views
{
    public partial class InvoiceArchivedView : UserControl
    {
        // Private backing action routine to signal when to close the pane layout
        private readonly Action _onCloseRequested;

        public InvoiceArchivedView(Action onCloseRequested, string title, string id, string category, string absoluteBillAmount)
        {
            InitializeComponent();

            // Safe guard the closure callback delegate routine
            _onCloseRequested = onCloseRequested ?? (() => { });

            // Assign dynamic text content matrices onto structural UI element targets
            TxtHeaderTitle.Text = title ?? "Archived Statement";
            TxtIdentifier.Text = id ?? "N/A";
            TxtCategory.Text = category ?? "General Suite";
            TxtGrossVolume.Text = $"Rs {absoluteBillAmount}.00";
            TxtFinalPaid.Text =  $"Rs {absoluteBillAmount}.00";

            // Wire up the button action listeners dynamically at runtime
            BtnClosePane.Click += (s, e) => _onCloseRequested.Invoke();
            BtnPrintInvoice.Click += BtnPrintInvoice_Click;
        }

        private void BtnPrintInvoice_Click(object sender, RoutedEventArgs e)
        {

            if (TxtHeaderTitle.Text == "Room & Accommodation Bill")
            {
                string baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string folderPath = Path.Combine(baseFolder, "CabanaOS", "Suite Invoices");

                // 3. Ensure the folder exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // 4. Construct unique file name
                string fileName = $"{TxtIdentifier.Text}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string fullPath = Path.Combine(folderPath, fileName);

                // 5. Generate PDF
                try
                {
                    PdfInvoiceService.GenerateSuitesInvoice(
                        filePath: fullPath,
                        bookingNumber: $"{TxtIdentifier.Text}",
                        roomNumber: "00",
                        suiteCharge: TxtFinalPaid.Text,
                        exCharge: "Rs. 0.00",
                        invoiceDate: DateTime.Now.ToString("yyyy-MM-dd"),
                        totalCharge: TxtFinalPaid.Text
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to generate invoice: {ex.Message}");
                    return; // Exit if generation fails
                }
            }
            else if(TxtHeaderTitle.Text == "Restaurant & Reservation Bill")
            {
                string baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string folderPath = Path.Combine(baseFolder, "CabanaOS", "Restaurant Invoices");
                // 3. Ensure the folder exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                // 4. Construct unique file name
                string fileName = $"{TxtIdentifier.Text}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string fullPath = Path.Combine(folderPath, fileName);
                // 5. Generate PDF
                try
                {
                    PdfInvoiceService.GenerateRestaurantInvoice(
                        filePath: fullPath,
                        orderNumber: $"{TxtIdentifier.Text}",
                        roomNumber: "00",
                        orderLines: new string[] { "Item 1 - Rs. 100.00", "Item 2 - Rs. 200.00" },
                        invoiceDate: DateTime.Now.ToString("yyyy-MM-dd"),
                        totalDue: TxtFinalPaid.Text
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to generate invoice: {ex.Message}");
                    return; // Exit if generation fails
                }
            }


            // Execute simulated POS device spooling operations
            MessageBox.Show($"Spooling POS receipt hardware stream to default local system printer...\n\nInvoice print batch processed successfully for: {TxtIdentifier.Text}",
                            "POS Print Engine Status",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        private void BtnPrintInvoice_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}