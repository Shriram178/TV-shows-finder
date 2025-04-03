using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Imaging;
using TVshows.Model;
using TVshows.UserContols;

namespace TVshows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowStaticPrompt();
        }

        private void ShowStaticPrompt()
        {
            StaticPromptControl1 staticPrompt = new StaticPromptControl1();
            ContentDisplay.Content = staticPrompt;
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text;
            if (!string.IsNullOrEmpty(query))
            {
                TVshow show = await GetTVShowAsync(query);
                if (show != null)
                {
                    ShowDetailsControl showDetails = new ShowDetailsControl();
                    showDetails.ShowImage.Source = new BitmapImage(new Uri(show.ImageURL));
                    showDetails.ShowName.Text = show.Name;
                    showDetails.ShowDescription.Text = show.Description;
                    showDetails.ShowRating.Text = $"Rating: {show.Rating}";
                    showDetails.ShowType.Text = $"Type: {show.Type}";
                    showDetails.ShowStatus.Text = $"Status: {show.Status}";
                    ContentDisplay.Content = showDetails;
                }
                else
                {
                    ShowStaticPrompt();
                }
            }
            else
            {
                ShowStaticPrompt();
            }
        }


        public async Task<TVshow> GetTVShowAsync(string query)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"https://api.tvmaze.com/search/shows?q={query}");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var shows = JsonSerializer.Deserialize<List<ShowResponse>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (shows != null && shows.Count > 0)
                    {
                        var firstShow = shows[0].Show;
                        return new TVshow
                        {
                            Name = firstShow.Name,
                            Description = firstShow.Summary,
                            ImageURL = firstShow.Image?.Medium,
                            Rating = firstShow.Rating?.Average.ToString(),
                            Type = firstShow.Type,
                            Status = firstShow.Status
                        };
                    }
                }
                return null;
            }
        }

        private void SearchByEnter(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                SearchButton_Click(sender, e);
            }
        }
    }
}