using ClientQzHalley.Models;
using Main.Models;
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
using System.Windows.Media.TextFormatting;
using System.IO;
using Main.Pages;
using ClientQzHalley.Pages;

namespace Main
{
    /// <summary>
    /// Interaction logic for QuizPage.xaml
    /// </summary>
    public partial class QuizWindow : Window
    {
        private UserWindow userWindow;
        private Question[] _questions;
        private QuestionResult[] results;
        private int index = -1;
        private int _id;
        string ip;
        BitmapImage[] images;
        public QuizWindow(UserWindow tmp, Question[] temp, int id,string ip)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            userWindow = tmp;
            ResizeMode = ResizeMode.NoResize;
            WindowState = WindowState.Maximized;
            _questions = temp;
            results = new QuestionResult[temp.Length]; // Use Length
            images = new BitmapImage[temp.Length];
            _id = id;
            this.ip = ip;
            Loaded += async (s, e) => await InitializeAsync();
        }
        private async Task InitializeAsync()
        {
            // Load first image immediately if needed
            if (_questions.Length > 0)
            {
                string? fileName = Path.GetFileName(_questions[0].img);
                if (!string.IsNullOrEmpty(fileName))
                {
                    try
                    {
                        images[0] = await new UserPage(_id,ip).LoadQuestionImage(fileName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to load first image: {ex.Message}");
                        images[0] = new BitmapImage(); // Fallback
                    }
                }
                else
                {
                    images[0] = new BitmapImage();
                }
            }
            QuizGrid.Visibility = Visibility.Visible;
            NextQuestion(); // Display first question
            _ = LoadPhotosAsync(); // Start background loading for other images
        }
        private async Task LoadPhotosAsync()
        {
            var userPage = new UserPage(_id,ip); // Single instance
            for (int i = 0; i < _questions.Length; i++)
            {
                try
                {
                    string? fileName = Path.GetFileName(_questions[i].img); // Use i, not index
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        var image = await userPage.LoadQuestionImage(fileName);
                        await Dispatcher.InvokeAsync(() => images[i] = image); // UI thread
                    }
                    else
                    {
                        await Dispatcher.InvokeAsync(() => images[i] = new BitmapImage()); // UI thread
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load image for question {i}: {ex.Message}");
                    await Dispatcher.InvokeAsync(() => images[i] = new BitmapImage()); // Fallback
                }
            }
        }

        private void NextQuestion()
        {
            index++;
            string? fileName = Path.GetFileName(_questions[index].img);
            answbtn1.Content = _questions[index].Option1;
            answbtn2.Content = _questions[index].Option2;
            answbtn3.Content = _questions[index].Option3;
            answbtn4.Content = _questions[index].Option4;
            QuestionTextBlock.Text = _questions[index].QuestionText;
            switch (_questions[index].OptionsCount)
            {
                case 2:
                    answbtn1.Visibility = Visibility.Visible;
                    answbtn2.Visibility = Visibility.Visible;
                    answbtn3.Visibility = Visibility.Collapsed;
                    answbtn4.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    answbtn1.Visibility = Visibility.Visible;
                    answbtn2.Visibility = Visibility.Visible;
                    answbtn3.Visibility = Visibility.Visible;
                    answbtn4.Visibility = Visibility.Collapsed;
                    break;
                case 4:
                    answbtn1.Visibility = Visibility.Visible;
                    answbtn2.Visibility = Visibility.Visible;
                    answbtn3.Visibility = Visibility.Visible;
                    answbtn4.Visibility = Visibility.Visible;
                    break;
            }
            if (fileName != null)
            {
                QuestionImageDefault.Visibility = Visibility.Collapsed;
                QuestionImage.Visibility = Visibility.Visible;
                QuestionImage.Source = images[index];
            }
            else
            {
                QuestionImage.Source = new BitmapImage();
                QuestionImage.Visibility = Visibility.Collapsed;
                QuestionImageDefault.Visibility = Visibility.Visible;
            }
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            userWindow.Visibility=Visibility.Visible;
            new UserPage(_id,ip).SendScore(results);
            Thread.Sleep(2000);
            string score= await new UserPage(_id, ip).VerifySingleUse();
            userWindow.MainWindow.Navigate(new ScorePage(score));
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            Button answer = (Button)sender;
            results[index] = new QuestionResult();
            results[index].UserId = _id;
            results[index].QuestionId = _questions[index].Id;
            results[index].SelectedOption = answer.Content.ToString();
            if (index == _questions.Count()-1)
            {
                this.Close();
            }
            else
            {
                NextQuestion();
            }
        }
    }
}
