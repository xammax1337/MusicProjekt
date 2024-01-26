using MusicProjektClient.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicProjektClient.Helpers
{
    public class ArtistMethods
    {
        public static async Task ListUsersArtists(HttpClient client, int userId)
        {
            var response = await client.GetAsync($"/artist/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                await Console.Out.WriteLineAsync($"Error listing user's artists (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
                return;
            }

            SoundMethods.PlayListingSound();
            string responseData = await response.Content.ReadAsStringAsync();

            List<ListUsersArtists> artists = JsonSerializer.Deserialize<List<ListUsersArtists>>(responseData);

            foreach (var artist in artists)
            {
                Console.WriteLine($"Artist name: {artist.ArtistName}");
            }

            Console.ReadLine();
            Console.Clear();
            await MenuMethods.UserMenu(client, userId);
        }

        public static async Task ConnectUserToArtist(HttpClient client, int userId)
        {
            await Console.Out.WriteAsync("Enter artist ID to connect with: ");

            int artistId = Convert.ToInt32(Console.ReadLine());

            var response = await client.PostAsync($"/user/{userId}/artist/{artistId}", null);

            if (!response.IsSuccessStatusCode)
            {
                Console.Clear();
                SoundMethods.PlayUnsuccessfullConnect();
                await Console.Out.WriteLineAsync($"Error connecting user with artist (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
            }
            else
            {
                Console.Clear();
                await Console.Out.WriteLineAsync($"Succesfully connected user with artist (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
            }
            Console.ReadLine();
            Console.Clear();
            await MenuMethods.UserMenu(client, userId);
        }
    }
}
