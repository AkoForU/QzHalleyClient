using System.Windows;
using System.Windows.Controls;
namespace ClientQzHalley.Pages
{
    /// <summary>
    /// Interaction logic for ScorePage.xaml
    /// </summary>
    public partial class ScorePage : Page
    {
        public ScorePage(string score)
        {
            InitializeComponent();
            ScoreProcents.Text = $"Your score is {score}%";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
