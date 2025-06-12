using System;
using System.Windows;
using System.Windows.Controls;

namespace Main
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private readonly LoginWindow loginRegisterWindow;
        private readonly API api = new API();
        private string ip;

        public LoginPage(LoginWindow tmp,string ip)
        {
            InitializeComponent();
            loginRegisterWindow = tmp;
            this.ip = ip;
            api.SetIpAddress("127.0.0.1"); // Default IP; update via UI if needed
        }

        private void ShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            passwordTxtBox.Text = passwordBox.Password;
            passwordBox.Visibility = Visibility.Collapsed;
            passwordTxtBox.Visibility = Visibility.Visible;
        }

        private void ShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            passwordBox.Password = passwordTxtBox.Text;
            passwordTxtBox.Visibility = Visibility.Collapsed;
            passwordBox.Visibility = Visibility.Visible;
        }

        private async void AuthenticateButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usrntxtbx.Text;
            string password=null;// Assume a TextBox named usernameTxtBox
            if (passwordBox.Visibility != Visibility.Collapsed)
            {
                password = passwordBox.Password; // Or passwordTxtBox.Text if visible
            }
            else
            {
                password=passwordTxtBox.Text;
            }

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and password are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var authResult = await api.AuthenticateAsync(username, password);
            if (authResult.Success)
            {
                UserWindow userWindow = new UserWindow(ip)
                {
                    UserId = authResult.UserId, // Pass the user ID to UserWindow
                    Username = username
                };
                userWindow.Show();
                loginRegisterWindow.Close();
            }
            else
            {
                MessageBox.Show("The username or password is wrong.", "Authentication Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}