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
using System.Windows.Threading;


namespace Main
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LoginRegisterWindow : Window
    {
        bool _state = false;//true is for Login and false is for Register
        public LoginRegisterWindow(bool state)
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            _state = state;
            PageSetter();
        }
        //setting the page for the window (Login or Register an User)
        public void PageSetter()
        {
            if (_state) {
                Frame loginFrame = new Frame();
                loginFrame.Name = "LoginPage";
                ContentFrame.Children.Clear();
                ContentFrame.Children.Add(loginFrame);
                loginFrame.Navigate(new LoginPage());
            }
            else
            {
                Frame addUserFrame=new Frame();
                addUserFrame.Name = "AddUser";
                ContentFrame.Children.Clear();
                ContentFrame.Children.Add(addUserFrame);
                addUserFrame.Navigate(new RegisterPage());
            }
        }
    }
}
