namespace MusicProjekt.Models
{
    public class Artist
    {
        public int ArtistId { get; set; }
        public string ArtistName { get; set;}
        public string Description { get; set;}

        public ICollection<User> Users { get; set; }
        public ICollection<Song> Songs { get; set; }
    }
}
