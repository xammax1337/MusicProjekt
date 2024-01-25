using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MusicProjektClient.ApiModels
{
    internal class GetAlbum
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
