using System.Text.Json.Serialization;

namespace Library.Models
{
    public class Author
    {
        

        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<BookAuthor> Books { get; set; }
    }
}
