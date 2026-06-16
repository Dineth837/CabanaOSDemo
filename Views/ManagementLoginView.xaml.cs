using System;
using System.Windows;
using System.Windows.Controls;

namespace CabanaOSDemo.Views
{
    public partial class ManagementLoginView : UserControl
    {
        private readonly Action _onLoginSuccess;

        // Constructor receives a callback trigger from the main navigation shell
        public ManagementLoginView(Action onLoginSuccess)
        {
            InitializeComponent();
            _onLoginSuccess = onLoginSuccess;
            TxtUsername.Focus(); // Automatically puts cursor in the username field
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text.Trim();
            string password = TxtPassword.Password; // Secure retrieval

            // 🔐 CREDENTIAL CHECK: Set your preferred administrative access text here
            if (username == "admin" && password == "1234")
            {
                // Run the success callback routine to swap views instantly
                _onLoginSuccess?.Invoke();
            }
            else
            {
                // Display default system warning layout when credential processing fails
                MessageBox.Show("Invalid management username or password. Access Denied.",
                                "Authentication Failed",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                TxtPassword.Clear();
                TxtPassword.Focus();
            }
        }
    }
}