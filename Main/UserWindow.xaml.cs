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
using System.Windows.Shapes;

namespace Main
{
    /// <summary>
    /// Interaction logic for AminPage.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public int UserId=0;
        public string Username=null;
        private string ip;
        public UserWindow(string ip)
        {
            InitializeComponent();
            WindowStartupLocation=WindowStartupLocation.CenterScreen;
            Title = "UserPage";
            ResizeMode = ResizeMode.NoResize;
            this.ip = ip;
        }

        private void Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow?.Navigate(new Pages.UserPage(UserId,Username,ip));
        }
    }
}
