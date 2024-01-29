
namespace MusicProjekt.Models.Dtos
{
    // Represents a Data Transfer Object for Genre information
    public class GenreDto
    { 
        public int UserId { get; set; }
        public int GenreId { get; set; }
        public string GenreName { get; set; }
    }
}
