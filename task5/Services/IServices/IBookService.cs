using task5.Models;

namespace task5.Services.IServices
{
    public interface IBookService
    {
        public Task<List<BookViewModel>> GenerateBooksAsync(int count, int seed, float avgLikes, float avgReviews);
    }
}
