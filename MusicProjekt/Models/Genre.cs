namespace MusicProjekt.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set;}

        public ICollection<User> Users { get; set; }
        public ICollection<Song> Songs { get; set; }    
    }
}
