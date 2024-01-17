using System.Text.Json.Serialization;

namespace MusicProjekt.Models.Dtos
{
    public class DiscographyDto
    {
        [JsonPropertyName("album")]
        public List<Album> Albums { get; set; }

        public class Album
        {
            [JsonPropertyName("strAlbum")]
            public string StrAlbum { get; set; }

            [JsonPropertyName("intYearReleased")]
            public string IntYearReleased { get; set; }
        }
    }
}
