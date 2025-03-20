using task5.Models;

namespace task5.Services.IServices
{
    public interface IReviewGeneratorService
    {
        public Task<List<ReviewViewModel>> GenerateReviewsAsync(float avgReviews);
    }
}
