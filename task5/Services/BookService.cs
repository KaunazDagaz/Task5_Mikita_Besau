using DotNetEnv;
using System.Globalization;
using task5.Models;
using task5.Services.IServices;
using task5.Utils;
using task5.Utils.IUtils;

namespace task5.Services
{
    public class BookService : IBookService
    {
        private readonly HttpClient httpClient;
        private readonly static string culture = CultureInfo.CurrentCulture.Name;
        private readonly static string baseUrl = Env.GetString("FAKER_API_BASE_URL");
        private readonly IParser parser = new Parser();
        private readonly Random random = new Random();

        public BookService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<BookViewModel>> GenerateBooksAsync(int count, int seed, float avgLikes, float avgReviews)
        {
            string booksRequestUrl = $"{baseUrl}/books?_quantity={count}&_locale={culture}&_seed={seed}";
            string jsonResponse = await FetchDataAsync(booksRequestUrl);
            var books = parser.ParseBooks(jsonResponse, avgLikes, new List<ReviewViewModel>());

            int totalReviews = books.Sum(book => CalculateReviewCount(avgReviews));
            string reviewsRequestUrl = $"{baseUrl}/texts?_quantity={totalReviews}&_locale={culture}&_characters=50";
            var allReviews = await FetchAndParseReviewsAsync(reviewsRequestUrl);

            var reviewIndex = 0;
            foreach (var book in books)
            {
                int countReviews = CalculateReviewCount(avgReviews);
                book.Reviews = allReviews.Skip(reviewIndex).Take(countReviews).ToList();
                reviewIndex += countReviews;
            }

            return books;
        }

        private int CalculateReviewCount(float avgReviews)
        {
            int baseReviews = (int)avgReviews;
            float extraProbability = avgReviews - baseReviews;
            return random.NextDouble() < extraProbability ? baseReviews + 1 : baseReviews;
        }

        private async Task<List<ReviewViewModel>> FetchAndParseReviewsAsync(string requestUrl)
        {
            string jsonResponse = await FetchDataAsync(requestUrl);
            return parser.ParseReviews(jsonResponse);
        }

        private async Task<string> FetchDataAsync(string requestUrl)
        {
            var response = await httpClient.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Failed to fetch data: {response.StatusCode}");
            return await response.Content.ReadAsStringAsync();
        }
    }
}