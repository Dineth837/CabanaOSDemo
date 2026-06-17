using System;
using System.IO;
using System.Windows;

namespace CabanaOSDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            InitializeFileSystem();
        }

        private void InitializeFileSystem()
        {
            // 1. Get the base path in the user's Documents folder
            string docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string cabanaRoot = Path.Combine(docsPath, "CabanaOS");
            string reportsRoot = Path.Combine(cabanaRoot, "Reports");

            // 2. List all required subdirectories
            string[] directoriesToCreate =
            {
            // Existing directories
            Path.Combine(cabanaRoot, "Room Access Card"),
            Path.Combine(cabanaRoot, "Restaurant Access Token"),
            Path.Combine(cabanaRoot, "Suite Invoices"),
            Path.Combine(cabanaRoot, "Restaurant Invoices"),
        
            // New reporting directories
            Path.Combine(reportsRoot, "Monthly Revenue Summary"),
            Path.Combine(reportsRoot, "Daily Transactions"),
            Path.Combine(reportsRoot, "Customer Reports"),
            Path.Combine(reportsRoot, "Employee Reports")
        };

            // 3. Create them automatically
            foreach (string path in directoriesToCreate)
            {
                try
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to create directory {path}: {ex.Message}");
                }
            }
        }
    }
}