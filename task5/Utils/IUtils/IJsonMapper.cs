using System.Text.Json;
using task5.Models;

namespace task5.Utils.IUtils
{
    public interface IJsonMapper
    {
        BookViewModel MapJsonToBookViewModel(JsonElement element);
        ReviewViewModel MapJsonToReviewViewModel(JsonElement element);
    }
}
