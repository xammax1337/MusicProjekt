using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MusicProjektClient.ApiModels
{
    internal class AddSong
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("artistId")]
        public int ArtistId { get; set; }
        [JsonPropertyName("genreId")]
        public int GenreId { get; set; }
    }
}
