using task5.Models;

namespace task5.Services.IServices
{
    public interface IImageProcessingService
    {
        Task<string> GenerateBookCoverImage(BookViewModel book);
    }
}
