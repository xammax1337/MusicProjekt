using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MusicProjektClient.ApiModels
{
    public class AddUser
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
    }
}
