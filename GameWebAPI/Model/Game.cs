using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameWebAPI.Model
{

    public class Game
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string StudioDeveloper { get; set; } = string.Empty;

        public ICollection<Genre>? Genres { get; set; }

        
        //[ForeignKey(nameof(Genre))]
        // public int GenresId { get; set; }

        //public List<Genre> Genres { get; set; } = new();
    }
}
