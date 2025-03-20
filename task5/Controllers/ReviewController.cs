using Microsoft.AspNetCore.Mvc;
using task5.Services.IServices;

namespace task5.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewGeneratorService reviewGeneratorService;
        public ReviewController(IReviewGeneratorService reviewGeneratorService)
        {
            this.reviewGeneratorService = reviewGeneratorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews(float avgReviews)
        {
            var reviews = await reviewGeneratorService.GenerateReviewsAsync(avgReviews);
            return Json(reviews);
        }
    }
}
