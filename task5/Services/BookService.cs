using DotNetEnv;
using System.Globalization;
using System.Text.Json;
using task5.Models;
using task5.Services.IServices;

namespace task5.Services
{
    public class BookService : IBookService
    {
        private readonly HttpClient httpClient;
        private readonly static string culture = CultureInfo.CurrentCulture.Name;
        private readonly static string baseUrl = Env.GetString("FAKER_API_BASE_URL");

        public BookService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<BookViewModel>> GenerateBooksAsync(int count, int seed)
        {
            string requestUrl = $"{baseUrl}?_quantity={count}&_locale={culture}&_seed={seed}&_fields=isbn,title,author,publisher";
            string jsonResponse = await FetchDataAsync(requestUrl);
            return ParseBooks(jsonResponse);
        }

        private async Task<string> FetchDataAsync(string requestUrl)
        {
            var response = await httpClient.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode) throw new HttpRequestException($"Failed to fetch data: {response.StatusCode}");
            return await response.Content.ReadAsStringAsync();
        }

        private List<BookViewModel> ParseBooks(string jsonResponse)
        {
            using var jsonDocument = JsonDocument.Parse(jsonResponse);
            var root = jsonDocument.RootElement;
            if (root.TryGetProperty("data", out JsonElement dataElement) && dataElement.ValueKind == JsonValueKind.Array)
            {
                var filteredBooks = new List<BookViewModel>();
                foreach (var element in dataElement.EnumerateArray())
                {
                    filteredBooks.Add(MapJsonToBookViewModel(element));
                }
                return filteredBooks;
            }
            else throw new JsonException("Unexpected JSON format: 'data' property not found or is not an array.");
        }

        private BookViewModel MapJsonToBookViewModel(JsonElement element)
        {
            return new BookViewModel
            {
                ISBN = element.GetProperty("isbn").GetString() ?? string.Empty,
                Title = element.GetProperty("title").GetString()?.TrimEnd('.') ?? string.Empty,
                Author = element.GetProperty("author").GetString() ?? string.Empty,
                Publisher = element.GetProperty("publisher").GetString() ?? string.Empty
            };
        }
    }
}
