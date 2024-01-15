namespace MusicProjekt.Models.Dtos
{
    public class AddSongDto
    {
        public string Title { get; set; }
        public int ArtistId { get; set; }
        public int GenreId { get; set; }
    }
}
