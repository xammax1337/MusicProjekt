using MusicProjektClient.ApiModels;
using MusicProjektClient.Helpers;
using NAudio.Wave;
using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MusicProjektClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string ourText = "Greetings! What shall we do today?\n\nLoading...\n";
            MenuMethods.PrintOneByOne(ourText);
            Console.ResetColor();
            Thread.Sleep(100);

            SoundMethods.PlayIntroToConsoleClientSound();

            // Initialization and setup
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7048");

                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    // Display the main menu options
                    Console.Clear();
                    Console.WriteLine("[1] View all users");
                    Console.WriteLine("[2] Select a user");
                    Console.WriteLine("[3] Create new user");
                    Console.WriteLine("[4] Add new song");
                    Console.WriteLine("[5] View artist's albums");
                    Console.WriteLine("[Q] Quit program");
                    Console.ResetColor();

                    string input = Console.ReadLine();

                    if (input.ToLower() == "q")
                    {
                        break;
                    }

                    switch (input)
                    {
                        case "1":
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Clear();
                            Console.WriteLine("Listing all users. \nPress enter to return to main menu");
                            Console.WriteLine($"--------------------------");
                            await UserMethods.ListAllUsers(client);
                            Console.ResetColor();
                            break;

                        case "2":
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Clear();
                            Console.WriteLine("Select a user");
                            Console.Write("Enter user ID: ");
                            Console.ResetColor();

                            if (int.TryParse(Console.ReadLine(), out int userId))
                            {
                                bool userExists = await MenuMethods.CheckIfUserExists(client, userId);

                                if (userExists)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                    SoundMethods.PlayListingSound();
                                    Console.WriteLine($"Selected user ID: {userId}");
                                    await MenuMethods.UserMenu(client, userId);
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    SoundMethods.PlayWrongInputSound();
                                    Console.WriteLine("User not found. Please, enter a valid user ID. " +
                                        "\nPress enter to return to main menu.");
                                    Console.ResetColor();
                                    Console.ReadLine();
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                SoundMethods.PlayWrongInputSound();
                                Console.WriteLine("Oops, invalid user ID! Please, enter a valid number." +
                                    "\n Press enter to return to main menu.");
                                Console.ReadLine();
                                Console.ResetColor();
                            }
                            Console.ReadLine();
                            break;

                        case "3":
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Create new user");
                            await UserMethods.AddUser(client);
                            Console.ResetColor();
                            break;

                        case "4":
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Add new song");
                            await SongMethods.AddSong(client);
                            Console.ResetColor();
                            break;

                        case "5":
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            await GetAlbumAsync(client);
                            Console.ResetColor();
                            break;

                        case "Q":
                            Console.Clear();
                            Environment.Exit(0);
                            break;

                        default:
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            SoundMethods.PlayWrongInputSound();
                            Console.WriteLine("Nah, invalid input, press enter to return to main menu.");
                            Console.ResetColor();
                            break;
                    }
                }
            }


            static async Task GetAlbumAsync(HttpClient client)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                await Console.Out.WriteAsync("Enter artist name: ");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.White;
                string input = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                var response = await client.GetAsync($"/album/{input}");
                Console.ResetColor();

                if (!response.IsSuccessStatusCode)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    SoundMethods.PlayListingNotPossibleSound();
                    await Console.Out.WriteLineAsync($"Error listing albums for artist (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu.");
                    Console.ResetColor();

                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    SoundMethods.PlayListingSound();
                    //Console.ForegroundColor= ConsoleColor.Green;
                    await Console.Out.WriteLineAsync($"Successfully listing albums for artist (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu.");
                    Console.ResetColor();
                    string responseData = await response.Content.ReadAsStringAsync();
                    GetAlbum getAlbumResponse = JsonSerializer.Deserialize<GetAlbum>(responseData);

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"Showing albums for {input}:");
                    Console.WriteLine($"--------------------------");
                    Console.ResetColor();

                    foreach (var album in getAlbumResponse.ViewAlbums)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine($"{album.StrAlbum}, released in {album.IntYearReleased}");
                        Console.ResetColor();
                    }
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"--------------------------");
                    Console.WriteLine("Press enter to return to menu.");
                    Console.ResetColor();
                    Console.ReadLine();
                }
            }


        }
    }
}