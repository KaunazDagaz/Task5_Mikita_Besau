using System.Text.Json;
using task5.Models;
using task5.Utils.IUtils;

namespace task5.Utils
{
    public class Parser : IParser
    {
        private readonly IJsonMapper jsonMapper = new JsonMapper();
        private readonly Random random = new Random();

        public List<BookViewModel> ParseBooks(string jsonResponse, float avgLikes, List<ReviewViewModel> reviews)
        {
            var dataElement = ExtractDataArray(jsonResponse);
            var books = new List<BookViewModel>();
            foreach (var element in dataElement.EnumerateArray())
            {
                var book = jsonMapper.MapJsonToBookViewModel(element);
                book.Likes = CalculateLikes(avgLikes);
                book.Reviews = reviews;
                books.Add(book);
            }
            return books;
        }

        public List<ReviewViewModel> ParseReviews(string jsonResponse)
        {
            var dataElement = ExtractDataArray(jsonResponse);
            var reviews = new List<ReviewViewModel>();
            foreach (var element in dataElement.EnumerateArray())
            {
                var review = jsonMapper.MapJsonToReviewViewModel(element);
                reviews.Add(review);
            }
            return reviews;
        }

        private JsonElement ExtractDataArray(string jsonResponse)
        {
            using var jsonDocument = JsonDocument.Parse(jsonResponse);
            var root = jsonDocument.RootElement;
            if (root.TryGetProperty("data", out JsonElement dataElement) && dataElement.ValueKind == JsonValueKind.Array)
            {
                return dataElement.Clone();
            }
            throw new JsonException("Unexpected JSON format: 'data' property not found or is not an array.");
        }

        private int CalculateLikes(float avgLikes)
        {
            int baseLikes = (int)avgLikes;
            float extraProbability = avgLikes - baseLikes;
            return random.NextDouble() < extraProbability ? baseLikes + 1 : baseLikes;
        }
    }
}