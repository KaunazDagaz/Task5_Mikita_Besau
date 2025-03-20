using DotNetEnv;
using task5.Models;
using System.Text.Json;
using task5.Services.IServices;
using MathNet.Numerics.Distributions;
using System.Globalization;

namespace task5.Services
{
    public class ReviewGeneratorService : IReviewGeneratorService
    {
        private readonly HttpClient httpClient;
        private readonly static string culture = CultureInfo.CurrentCulture.Name;
        private readonly static string baseUrl = Env.GetString("FAKER_API_BASE_URL");
        private readonly Random random = new Random();

        public ReviewGeneratorService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<ReviewViewModel>> GenerateReviewsAsync(float avgReviews)
        {
            int baseReviews = (int)avgReviews;
            float extraProbability = avgReviews - baseReviews;
            int count = baseReviews;
            if (random.NextDouble() < extraProbability) count++;
            if (count == 0) return new List<ReviewViewModel>();
            string requestUrl = $"{baseUrl}/texts?_quantity={count}&_locale={culture}&_characters=50";
            var response = await httpClient.GetStringAsync(requestUrl);
            return ParseReviews(response);
        }


        private List<ReviewViewModel> ParseReviews(string jsonResponse)
        {
            using var jsonDocument = JsonDocument.Parse(jsonResponse);
            var root = jsonDocument.RootElement;
            if (root.TryGetProperty("data", out JsonElement dataElement) && dataElement.ValueKind == JsonValueKind.Array)
            {
                var reviews = new List<ReviewViewModel>();
                foreach (var element in dataElement.EnumerateArray())
                {
                    var review = MapJsonToReviewViewModel(element);
                    reviews.Add(review);
                }
                return reviews;
            }
            else throw new JsonException("Unexpected JSON format: 'data' property not found or is not an array.");
        }

        private ReviewViewModel MapJsonToReviewViewModel(JsonElement element)
        {
            return new ReviewViewModel
            {
                Username = element.GetProperty("author").GetString() ?? string.Empty,
                Comment = element.GetProperty("content").GetString() ?? string.Empty
            };
        }
    }
}
