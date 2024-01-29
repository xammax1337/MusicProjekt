using MusicProjektClient.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicProjektClient.Helpers
{
    public class MenuMethods
    {
        public static async Task UserMenu(HttpClient client, int userId)
        {
            Console.Clear();
            Console.WriteLine($"Current user ID: {userId}");
            Console.WriteLine($"Now choose an option for this user:");
            Console.WriteLine($"--------------------------");
            Console.WriteLine("[1] List user's artists");
            Console.WriteLine("[2] Connect user to artist");
            Console.WriteLine("[3] List user's genres");
            Console.WriteLine("[4] Connect user to genre");
            Console.WriteLine("[5] List user's songs");
            Console.WriteLine("[6] Connect song to user");
            Console.WriteLine("[Q] Return to main menu");
            string input = Console.ReadLine().ToLower();
            switch (input)
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine($"Listing user's artists. \nPress enter to return to menu.");
                    Console.WriteLine($"--------------------------");

                    await ArtistMethods.ListUsersArtists(client, userId);
                    break;
                case "2":
                    Console.Clear();
                    await ArtistMethods.ConnectUserToArtist(client, userId);
                    break;
                case "3":
                    Console.Clear();
                    Console.WriteLine($"Listing user's genres. \nPress enter to return to menu.");
                    Console.WriteLine($"--------------------------");

                    await GenreMethods.ListUsersGenres(client, userId);
                    break;
                case "4":
                    Console.Clear();
                    await GenreMethods.ConnectUserToGenre(client, userId);
                    break;
                case "5":
                    Console.Clear();
                    Console.WriteLine($"Listing user's songs. \nPress enter to return to menu.");
                    Console.WriteLine($"--------------------------");

                    await SongMethods.ListUserSongs(client, userId);
                    break;
                case "6":
                    Console.Clear();
                    await SongMethods.ConnectSongToUser(client, userId);
                    break;
                case "q":
                    Console.Clear();
                    SoundMethods.PlayReturnToMainMenuSound();
                    Console.WriteLine("Press enter to return to main menu.");
                    return;
                default:
                    SoundMethods.PlayWrongInputSound();
                    Console.WriteLine("Eek, wrong input!");
                    Console.WriteLine("Press enter to return to menu.");
                    Console.ReadLine();
                    Console.Clear();
                    break;
            }
        }

        public static async Task<bool> CheckIfUserExists(HttpClient client, int userId)
        {
            var response = await client.GetAsync("/user/");

            if (!response.IsSuccessStatusCode)
            {
                Console.Clear();
                await Console.Out.WriteLineAsync($"Error finding user (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
                return false;
            }

            string responseData = await response.Content.ReadAsStringAsync();

            List<User> users = JsonSerializer.Deserialize<List<User>>(responseData);

            return users.Any(user => user.UserId == userId);
        }

        //Will print out a letter at a time with 100 milliseconds of pause in between
        public static void PrintOneByOne(string text)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(100);
            }
        }
    }
}
