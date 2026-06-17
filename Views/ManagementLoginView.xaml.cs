using System;
using System.Windows;
using System.Windows.Controls;

namespace CabanaOSDemo.Views
{
    public partial class ManagementLoginView : UserControl
    {
        private readonly Action _onLoginSuccess;

        public ManagementLoginView(Action onLoginSuccess)
        {
            InitializeComponent();
            _onLoginSuccess = onLoginSuccess;
            TxtUsername.Focus(); 
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text.Trim();
            string password = TxtPassword.Password;

            
            if (username == "admin" && password == "admin")
            {
                
                _onLoginSuccess?.Invoke();
            }
            else
            {
                
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