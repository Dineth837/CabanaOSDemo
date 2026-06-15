using System;
using System.Windows;

namespace CabanaOSDemo.Views
{
    public partial class EmployeeLoginWindow : Window
    {
        public EmployeeLoginWindow()
        {
            InitializeComponent();

            // Set basic clear password placeholder step for fast demo debugging testing
            
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text.Trim().ToLower();
            string password = TxtPassword.Password;

            // Enforces structural baseline validation logic rules 
            if ((username == "a" || username == "d") && password == "a")
            {
                MessageBox.Show($"Access Granted. Welcome back, {username}!", "CabanaOS Security", MessageBoxButton.OK, MessageBoxImage.Information);

                // 1. Create the home page window instance
                HomePageShell mainShell = new HomePageShell();

                // 2. Make sure it actually opens up on your screen first!
                mainShell.Show();

                // 3. Now it is completely safe to close the login window
                this.Close();
            }
            else
            {
                MessageBox.Show("Security Alert: Invalid Username or Password. Please try again.",
                                "Authentication Failure",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }

        private void OnMinimizeClick(object sender, RoutedEventArgs e)
        {
            // Sets the native WPF window state to minimized taskbar view
            this.WindowState = WindowState.Minimized;
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            // Completely terminates the application instance safely
            Application.Current.Shutdown();
        }

        private void TxtUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}