namespace MusicProjekt.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }


        //User can be connected with many songs/genres/artists.
        public ICollection<Song> Songs { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<Artist> Artists { get; set; }
    }
}
