using Main.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Main
{
    public class API
    {
        string _clientName = "guest";
        private static readonly HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };
        private static string _serverUrl;
        private static bool _isConsoleAllocated = false;

        public void SetIpAddress(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                throw new ArgumentException("IP address cannot be null or empty.", nameof(ip));
            _serverUrl = $"http://{ip.Trim()}:5000";
        }

        public async Task<AuthResult> AuthenticateAsync(string username, string password)
        {

            if (string.IsNullOrEmpty(_serverUrl))
            {
                Console.WriteLine("Server URL not set. Call SetIpAddress first.");
                return new AuthResult { Success = false, UserId = 0 };
            }

            try
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
                });
                var response = await client.PostAsync($"{_serverUrl}/authenticate", content);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AuthResult>(json) ?? new AuthResult { Success = false, UserId = 0 };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network error: {ex.Message}");
                return new AuthResult { Success = false, UserId = 0 };
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON deserialization error: {ex.Message}");
                return new AuthResult { Success = false, UserId = 0 };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return new AuthResult { Success = false, UserId = 0 };
            }
        }

        //verifying for the corect server connection
        public async Task<string> GetDataAsyncForTestingConnection()
        {
            //if (!_isConsoleAllocated)
            //{
            //    if (!AllocConsole())
            //    {
            //        Console.WriteLine("Failed to allocate console.");
            //        return null;
            //    }
            //    _isConsoleAllocated = true;
            //}

            if (string.IsNullOrEmpty(_serverUrl))
            {
                Console.WriteLine("Server URL not set. Call SetIpAddress first.");
                return null;
            }

            try
            {
                var response = await client.GetAsync($"{_serverUrl}/get_messages?sender={Uri.EscapeDataString(_clientName)}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var message = JsonSerializer.Deserialize<Message>(json);
                if (message != null)
                {
                    Console.WriteLine($"Polled message: {json}");
                    return message.Content;
                }
                else
                {
                    Console.WriteLine("No valid message received.");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network error: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON deserialization error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return null;
            }
        }
    }

    public record Message
    {
        [JsonPropertyName("sender")]
        public string Sender { get; init; }
        [JsonPropertyName("content")]
        public string Content { get; init; }
    }
}