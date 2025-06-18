using ClientQzHalley.Models;
using ClientQzHalley.Pages;
using Main.Models;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Main.Pages
{
    public partial class UserPage : Page
    {
        private readonly HttpClient _httpClient;
        private Question[] _questions;
        private string ip;
        public int IdUser;
        public string Username;
        public UserPage(int id, string name, string ip)
        {
            InitializeComponent();
            IdUser = id;
            Username = name;
            this.ip = ip;
            usrtext.Text = $"Hello {Username}, let's start the test";
            _httpClient = new HttpClient();
            _questions = Array.Empty<Question>();
            verify();
            
        }

        private async void verify()
        {
            string score = await VerifySingleUse();
            if (score == "null")
            {
                startbtn.IsEnabled = true;
            }
            else
            {
                this.NavigationService.Navigate(new ScorePage(score));
            }
        }
        public UserPage(int id,string ip)
        {
            IdUser = id;
            this.ip = ip;
            _httpClient = new HttpClient(); // Initialize _httpClient
            _questions = Array.Empty<Question>();
        }

        public async Task<string> VerifySingleUse()
        {
            try
            {
                string apiUrl = $"http://{ip}:5000/quiz_results?sender={IdUser}";
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode(); // Throws if not 200 OK

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var scoreResponse = JsonSerializer.Deserialize<string>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return scoreResponse;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"API request failed: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Invalid response format: {ex.Message}");
                return null;
            }
        }
        public async Task<BitmapImage> LoadQuestionImage(string fileName)
        {
            try
            {
                string imageUrl = $"http://{ip}:5000/images/{fileName}";
                await SendImageRequest(fileName);

                BitmapImage bitmap = new();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imageUrl, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load image {fileName}: {ex.Message}");
                return new();
            }
        }

        public async Task SendScore(QuestionResult[] _results)
        {
            try
            {
                string apiUrl = $"http://{ip}:5000/submit_results";
                string json = JsonSerializer.Serialize(_results);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting quiz: {ex.Message}");
            }
        }

        private async Task SendImageRequest(string fileName)
        {
            try
            {
                var message = new
                {
                    sender = IdUser,
                    content = $"image:{fileName}"
                };
                string json = JsonSerializer.Serialize(message);
                StringContent content = new(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync($"http://{ip}:5000/send_message", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send image request for {fileName}: {ex.Message}");
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string apiUrl = $"http://{ip}:5000/get_questions?sender={IdUser}&content=start";
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

                Question[] questions;
                try
                {
                    questions = JsonSerializer.Deserialize<Question[]>(jsonResponse, options) ?? Array.Empty<Question>();
                }
                catch (JsonException)
                {
                    var wrappedResponse = JsonSerializer.Deserialize<WrappedQuestionsResponse>(jsonResponse, options);
                    questions = wrappedResponse?.Questions ?? Array.Empty<Question>();
                }

                _questions = questions;
                QuizWindow quizWindow = new QuizWindow((UserWindow)Window.GetWindow(this), questions, IdUser,ip);
                quizWindow.Show();
                Window.GetWindow(this).Visibility = Visibility.Collapsed;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"API request failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching questions: {ex.Message}");
            }
        }

        internal class WrappedQuestionsResponse
        {
            public Question[] Questions { get; set; } = Array.Empty<Question>();
            public string Message { get; set; } = string.Empty;
        }
    }
}