using task5.Models;

namespace task5.Utils.IUtils
{
    public interface IParser
    {
        List<BookViewModel> ParseBooks(string jsonResponse, float avgLikes, List<ReviewViewModel> reviews);
        List<ReviewViewModel> ParseReviews(string jsonResponse);
    }
}
