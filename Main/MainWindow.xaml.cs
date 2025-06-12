
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation =WindowStartupLocation.CenterScreen;
        }

        private async void authenticate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ServerIpTextBox.Text))
            {
                MessageBox.Show("U need to type an ip to connect to the server","Error",MessageBoxButton.OK,MessageBoxImage.Exclamation);
                return;
            }
            string response = await GetResponse();
            Console.WriteLine(response); // Debug output
            if (response?.ToLower() == "welcome onboard") // Case-insensitive check
            {
                LoginWindow login = new LoginWindow(ServerIpTextBox.Text);
                login.Title = "Login";
                login.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Authentication failed or unexpected response: " + (response ?? "null"),"Error Connection",MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }
        private async Task<string> GetResponse()
        {
            API api = new API();
            api.SetIpAddress(ServerIpTextBox.Text);
            return await api.GetDataAsyncForTestingConnection();
        }
    }
}