namespace task5.Models
{
    public class BookViewModel
    {
        public required string ISBN { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string Publisher { get; set; }
        public int Likes { get; set; }
        public List<ReviewViewModel>? Reviews { get; set; }
    }
}
