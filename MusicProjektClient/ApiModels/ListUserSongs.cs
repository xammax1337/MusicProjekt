using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MusicProjektClient.ApiModels
{
    internal class ListUserSongs
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("artistName")]
        public string ArtistName { get; set; }
    }
}
