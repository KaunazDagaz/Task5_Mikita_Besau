using System.Text.Json;
using task5.Models;
using task5.Utils.IUtils;

namespace task5.Utils
{
    public class JsonMapper : IJsonMapper
    {
        public BookViewModel MapJsonToBookViewModel(JsonElement element)
        {
            return new BookViewModel
            {
                ISBN = element.GetProperty("isbn").GetString() ?? string.Empty,
                Title = element.GetProperty("title").GetString()?.TrimEnd('.') ?? string.Empty,
                Author = element.GetProperty("author").GetString() ?? string.Empty,
                Publisher = element.GetProperty("publisher").GetString() ?? string.Empty,
            };
        }

        public ReviewViewModel MapJsonToReviewViewModel(JsonElement element)
        {
            return new ReviewViewModel
            {
                Username = element.GetProperty("author").GetString() ?? string.Empty,
                Comment = element.GetProperty("content").GetString() ?? string.Empty
            };
        }
    }
}
