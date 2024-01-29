using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MusicProjektClient.ApiModels
{
    // Represents the data model for listing genres associated with a user
    internal class ListUsersGenres
    {
        [JsonPropertyName("genreName")]
        public string GenreName { get; set; }
    }
}
