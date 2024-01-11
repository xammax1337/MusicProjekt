namespace MusicProjekt.Models
{
    public class Song
    {
        public int SongId { get; set; }
        public string Title { get; set;}

        public int ArtistId {  get; set; } 
        public int GenreId {  get; set; }

        public ICollection<User> Users { get; set; }
    }
}
