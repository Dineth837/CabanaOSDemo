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
            if ((username == "admin" || username == "ADMIN") && password == "admin")
            {   
                /*
                string role = "Employee201";
                MessageBox.Show($"Access Granted. Welcome back, {role}!", "CabanaOS Security", MessageBoxButton.OK, MessageBoxImage.Information);
                */

                HomePageShell mainShell = new HomePageShell();

                mainShell.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Security Alert: Invalid Username or Password. Please try again.",
                                "Authentication Failure",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                TxtPassword.Clear();
                TxtUsername.Clear();
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