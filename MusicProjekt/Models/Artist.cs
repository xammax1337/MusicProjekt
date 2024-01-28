namespace MusicProjekt.Models
{
    public class Artist
    {
        public int ArtistId { get; set; }
        public string ArtistName { get; set;}
        public string Description { get; set;}

        //Artist can be connected to many users/songs.
        public ICollection<User> Users { get; set; }
        public ICollection<Song> Songs { get; set; }
    }
}
