using System.ComponentModel.DataAnnotations;

namespace GameWebAPI.Model
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";

        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<Game>? Games { get; set; }

        

        //public Game? Game { get; set; }
        //public List<GenresGame> gameListGenres { get; set; } = new();
    }
}
