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
            Console.ForegroundColor = ConsoleColor.White;
            string title = Console.ReadLine();
            Console.ResetColor();

            int artistId;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                await Console.Out.WriteAsync("Enter artist ID: ");
                Console.ForegroundColor = ConsoleColor.White;
                if (int.TryParse(Console.ReadLine(), out artistId)) 
                {
                   
                    break;
                }
                else 
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid artist ID. Please enter a valid number.");
                }
            }
            int genreId;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;              
                await Console.Out.WriteAsync("Enter genre ID: ");
                Console.ForegroundColor = ConsoleColor.White;
                if (int.TryParse(Console.ReadLine(), out genreId))
                {
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid genre ID. Please enter a valid number.");
                }
            }         

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
                Console.ForegroundColor = ConsoleColor.DarkRed;
                await Console.Out.WriteLineAsync($"Error adding song (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu");
                return;
            }
            else
            {
                Console.Clear();
                SoundMethods.PlaySuccessfulAddSound();
                Console.ForegroundColor = ConsoleColor.Green;
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
                Console.ForegroundColor = ConsoleColor.DarkRed;
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
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            await Console.Out.WriteAsync("Enter song ID to connect with: ");
            Console.ForegroundColor = ConsoleColor.White;
            
            int songId;
            while (true)
            {
               
                Console.ForegroundColor = ConsoleColor.White;
                if (int.TryParse(Console.ReadLine(), out songId))
                {
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid song ID. Please enter a valid number."); 
                    Console.WriteLine("Press enter to return to menu");

                    return;

                }
                
            }

            var response = await client.PostAsync($"/user/{userId}/song/{songId}", null);

            if (!response.IsSuccessStatusCode)
            {
                Console.Clear();
                Console.ForegroundColor= ConsoleColor.DarkRed;
                SoundMethods.PlayUnsuccessfulConnectSound();
                await Console.Out.WriteLineAsync($"Error connecting user with song (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
            }
            else
            {
                Console.Clear();
                SoundMethods.PlaySuccessfulConnectSound();
                Console.ForegroundColor = ConsoleColor.Green;
                await Console.Out.WriteLineAsync($"Succesfully connected user with song (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
            }
            Console.ReadLine();
            Console.Clear();
            await MenuMethods.UserMenu(client, userId);

        }
    }
}
