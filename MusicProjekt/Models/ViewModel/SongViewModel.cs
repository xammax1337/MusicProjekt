namespace MusicProjekt.Models.ViewModel
{
    public class SongViewModel
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public int ArtistId { get; set; }
        public int GenreId { get; set; }

        // Koppla ihop med UserViewModel för att kunna se alla Users som har lagt till en specifik låt
        //public UserViewModel[] Users { get; set; }
    }
}
