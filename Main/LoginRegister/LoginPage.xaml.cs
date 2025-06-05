using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        LoginRegisterWindow loginRegisterWindow;
        public LoginPage(LoginRegisterWindow tmp)
        {
            InitializeComponent();
            loginRegisterWindow = tmp;
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

        private void AuthenticateButton_Click(object sender, RoutedEventArgs e)
        {
            //this is only temporary, rememba to delete this, u need to make a secure authenticator
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
            loginRegisterWindow.Close();
        }
    }
}
