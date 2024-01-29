using MusicProjektClient.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicProjektClient.Helpers
{
    public class SongMethods
    {
        public static async Task AddSong(HttpClient client)
        {
            await Console.Out.WriteAsync("Enter song title: ");
            string title = Console.ReadLine();

            await Console.Out.WriteAsync("Enter artist ID: ");
            int artistId = Convert.ToInt32(Console.ReadLine());

            await Console.Out.WriteAsync("Enter genre ID: ");
            int genreId = Convert.ToInt32(Console.ReadLine());

            AddSong addSong = new AddSong()
            {
                Title = title,
                ArtistId = artistId,
                GenreId = genreId
            };

            string json = JsonSerializer.Serialize(addSong);

            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/song", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                Console.Clear();
                SoundMethods.PlayUnsuccessfulAddSound();
                await Console.Out.WriteLineAsync($"Error adding song (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu");
                return;
            }
            else
            {
                Console.Clear();
                SoundMethods.PlaySuccessfulAddSound();
                await Console.Out.WriteLineAsync($"Added song (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
            }
            Console.ReadLine();
            Console.Clear();
        }

        public static async Task ListUserSongs(HttpClient client, int userId)
        {
            var response = await client.GetAsync($"/song/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                Console.Clear();
                SoundMethods.PlayListingNotPossibleSound();
                await Console.Out.WriteLineAsync($"Error listing user's songs (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
                return;
            }

            SoundMethods.PlayListingSound();
            string responseData = await response.Content.ReadAsStringAsync();

            List<ListUserSongs> songs = JsonSerializer.Deserialize<List<ListUserSongs>>(responseData);

            foreach (var song in songs)
            {
                Console.WriteLine($"Title: {song.Title}, by: {song.ArtistName}");
            }

            Console.ReadLine();
            Console.Clear();
            await MenuMethods.UserMenu(client, userId);
        }

        public static async Task ConnectSongToUser(HttpClient client, int userId)
        {
            await Console.Out.WriteAsync("Enter song ID to connect with: ");

            int songId = Convert.ToInt32(Console.ReadLine());

            var response = await client.PostAsync($"/user/{userId}/song/{songId}", null);

            if (!response.IsSuccessStatusCode)
            {
                Console.Clear();
                SoundMethods.PlayUnsuccessfulConnectSound();
                await Console.Out.WriteLineAsync($"Error connecting user with song (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
            }
            else
            {
                Console.Clear();
                SoundMethods.PlaySuccessfulConnectSound();
                await Console.Out.WriteLineAsync($"Succesfully connected user with song (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
            }
            Console.ReadLine();
            Console.Clear();
            await MenuMethods.UserMenu(client, userId);

        }
    }
}
