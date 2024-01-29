namespace MusicProjekt.Models
{
    // Represents the Genre entity in the database
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set;}

        //Genre can be connected to many users/songs.
        public ICollection<User> Users { get; set; }
        public ICollection<Song> Songs { get; set; }    
    }
}
