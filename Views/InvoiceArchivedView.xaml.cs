using System;
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
            TxtGrossVolume.Text = absoluteBillAmount ?? "Rs. 00.00";
            TxtFinalPaid.Text = absoluteBillAmount ?? "Rs. 00.00";

            // Wire up the button action listeners dynamically at runtime
            BtnClosePane.Click += (s, e) => _onCloseRequested.Invoke();
            BtnPrintInvoice.Click += BtnPrintInvoice_Click;
        }

        private void BtnPrintInvoice_Click(object sender, RoutedEventArgs e)
        {
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