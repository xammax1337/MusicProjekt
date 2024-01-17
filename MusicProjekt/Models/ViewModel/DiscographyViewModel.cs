using System.Text.Json.Serialization;

namespace MusicProjekt.Models.ViewModel
{
    public class DiscographyViewModel
    {
        [JsonPropertyName("album")]
        public List<ViewAlbum> ViewAlbums { get; set; }

        public class ViewAlbum
        {
            [JsonPropertyName("strAlbum")]
            public string StrAlbum { get; set; }

            [JsonPropertyName("intYearReleased")]
            public string IntYearReleased { get; set; }
        }
    }
}
