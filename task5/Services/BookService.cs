using Bogus;
using task5.Models;
using task5.Services.IServices;
using Microsoft.AspNetCore.Localization;

namespace task5.Services
{
    public class BookService : IBookService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IImageProcessingService imageProcessingService;
        private readonly Random random = new();

        public BookService(IHttpContextAccessor httpContextAccessor, IImageProcessingService imageProcessingService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.imageProcessingService = imageProcessingService;
        }

        public List<BookViewModel> GenerateBooks(int count, int seed, float avgLikes, float avgReviews)
        {
            string culture = GetCulture();
            var bookFaker = CreateBookFaker(seed, culture);
            var books = bookFaker.Generate(count);
            var reviewFaker = CreateReviewFaker(seed, culture);
            foreach (var book in books)
            {
                PopulateBookLikesAndReviews(book, reviewFaker, avgLikes, avgReviews);
                book.CoverImageUrl = imageProcessingService.GenerateBookCoverImage(book).Result;
            }
            return books;
        }

        private string GetCulture()
        {
            var requestCulture = httpContextAccessor.HttpContext?.Features.Get<IRequestCultureFeature>();
            return requestCulture?.RequestCulture.Culture.Name ?? "en-US";
        }

        private Faker<BookViewModel> CreateBookFaker(int seed, string culture)
        {
            return new Faker<BookViewModel>(culture.Split('-')[0])
                .UseSeed(seed)
                .RuleFor(b => b.ISBN, f => f.Random.Replace("978-#-##-######-#"))
                .RuleFor(b => b.Title, f => $"{f.Commerce.ProductAdjective()} {f.Commerce.Product()}")
                .RuleFor(b => b.Author, f => f.Name.FullName())
                .RuleFor(b => b.Publisher, f => f.Company.CompanyName());
        }

        private Faker<ReviewViewModel> CreateReviewFaker(int seed, string culture)
        {
            return new Faker<ReviewViewModel>(culture.Split('-')[0])
                .RuleFor(r => r.Username, f => f.Internet.UserName())
                .RuleFor(r => r.Comment, f =>
                {
                    if (culture.StartsWith("en"))
                    {
                        return f.Rant.Review();
                    }
                    else
                    {
                        return f.Lorem.Sentence(8);
                    }
                });
        }

        private void PopulateBookLikesAndReviews(BookViewModel book, Faker<ReviewViewModel> reviewFaker, float avgLikes, float avgReviews)
        {
            int countReviews = CalculateProbability(avgReviews);
            book.Reviews = reviewFaker.Generate(countReviews);
            int countLikes = CalculateProbability(avgLikes);
            book.Likes = countLikes;
        }

        private int CalculateProbability(float avg)
        {
            int baseValue = (int)avg;
            float extraProbability = avg - baseValue;
            return random.NextDouble() < extraProbability ? baseValue + 1 : baseValue;
        }
    }
}