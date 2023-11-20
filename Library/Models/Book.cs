using System.Text.Json.Serialization;

namespace Library.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsAvailable { get; set; }
        [JsonIgnore]
        public List<BookAuthor> Authors { get; set; }


    }
}
