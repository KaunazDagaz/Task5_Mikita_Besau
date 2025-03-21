using task5.Models;

namespace task5.Services.IServices
{
    public interface IBookService
    {
        public List<BookViewModel> GenerateBooks(int count, int seed, float avgLikes, float avgReviews);
    }
}
