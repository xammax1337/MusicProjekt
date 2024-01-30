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
            //Makes a GET call to API's endpoint, which is through user's ID
            var response = await client.GetAsync($"/artist/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                SoundMethods.PlayListingNotPossibleSound();

                await Console.Out.WriteLineAsync($"Error listing user's artists (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
                return;
            }

            SoundMethods.PlayListingSound();
            //The string answer we get from the call
            string responseData = await response.Content.ReadAsStringAsync();

            //Deserializing the Json string  into a list of of ListUsersArtists objects
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
            //This chosen artist ID is to be connected with chosen user ID in main menu
            int artistId;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                await Console.Out.WriteAsync("Enter artist ID to connect with: ");
                Console.ForegroundColor = ConsoleColor.White;
                if (int.TryParse(Console.ReadLine(), out artistId))
                {

                    break;
                }
                else
                {
                    SoundMethods.PlayWrongInputSound();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid artist ID. Please enter a valid number.\nPress enter to return to menu.");
                    return;
                }
            }

            var response = await client.PostAsync($"/user/{userId}/artist/{artistId}", null);

            if (!response.IsSuccessStatusCode)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.DarkRed;
                SoundMethods.PlayUnsuccessfulConnectSound();

                await Console.Out.WriteLineAsync($"Error connecting user with artist (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
            }
            else
            {
                Console.Clear();
                SoundMethods.PlaySuccessfulConnectSound();
                Console.ForegroundColor = ConsoleColor.Green;
                await Console.Out.WriteLineAsync($"Succesfully connected user with artist (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
            }
            Console.ReadLine();
            Console.Clear();
            await MenuMethods.UserMenu(client, userId);
        }
    }
}