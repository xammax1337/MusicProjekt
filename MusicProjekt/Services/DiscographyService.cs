using System.Text.Json;

namespace MusicProjekt.Services
{
    public interface IDiscographyService
    {
        Task<Models.Dtos.DiscographyDto> GetAlbumAsync(string artist);
    }

    public class DiscographyService : IDiscographyService
    {
        private HttpClient _client;

        public DiscographyService() : this(new HttpClient()) { }

        public DiscographyService(HttpClient client)
        {
            _client = client;
        }

        public async Task<Models.Dtos.DiscographyDto> GetAlbumAsync(string artist)
        {
            var result = await _client.GetAsync($"https://www.theaudiodb.com/api/v1/json/2/discography.php?s={artist}");

            result.EnsureSuccessStatusCode();

            Models.Dtos.DiscographyDto content = JsonSerializer.Deserialize<Models.Dtos.DiscographyDto>(await result.Content.ReadAsStringAsync());

            return content;
        }

    }

}
