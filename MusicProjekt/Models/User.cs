namespace MusicProjekt.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public ICollection<Song> Songs { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<Artist> Artists { get; set; }
    }
}
