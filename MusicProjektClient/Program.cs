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
            string ourText = "Greetings! What shall we do today?\n\nLoading...\n";
            MenuMethods.PrintOneByOne(ourText);
            Thread.Sleep(1000);

            SoundMethods.PlayIntroToConsoleClient();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7048");

                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
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
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Listing all users. \nPress enter to return to main menu");
                            Console.WriteLine($"--------------------------");
                            await UserMethods.ListAllUsers(client);
                            Console.ResetColor();
                            break;

                        case "2":
                            Console.Clear();
                            Console.ForegroundColor= ConsoleColor.DarkCyan;
                            Console.WriteLine("Select a user");
                            Console.Write("Enter user ID: ");
                            Console.ResetColor();

                            if (int.TryParse(Console.ReadLine(), out int userId))
                            {
                                bool userExists = await MenuMethods.CheckIfUserExists(client, userId);

                                if (userExists)
                                {
                                    Console.ForegroundColor=ConsoleColor.DarkCyan;
                                    Console.WriteLine($"Selected user ID: {userId}");
                                    await MenuMethods.UserMenu(client, userId);
                                    Console.ResetColor();
                                }
                                else
                                {
                                    SoundMethods.PlayWrongInput();
                                    Console.ForegroundColor= ConsoleColor.Red;
                                    Console.WriteLine("User not found. Please, enter a valid user ID. " +
                                        "\nPress enter to return to main menu.");
                                    Console.ResetColor();
                                    Console.ReadLine();
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Oops, invalid user ID! Please, enter a valid number.");
                                Console.ResetColor();
                                Console.ReadLine();
                            }
                            Console.ReadLine();
                            break;

                        case "3":
                            Console.ForegroundColor=ConsoleColor.DarkCyan;
                            Console.WriteLine("Create new user");
                            await UserMethods.AddUser(client);
                            Console.ResetColor();
                            break;

                        case "4":
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Add new song");
                            Console.ResetColor();
                            await SongMethods.AddSong(client);
                            break;

                        case "5":
                            await GetAlbumAsync(client);
                            break;

                        case "Q":
                            Console.Clear();
                            Environment.Exit(0);
                            break;

                        default:
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
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

                string input = Console.ReadLine();

                var response = await client.GetAsync($"/album/{input}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    await Console.Out.WriteLineAsync($"Error listing albums for artist (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu.");
                    Console.ResetColor();

                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    await Console.Out.WriteLineAsync($"Successfully listing albums for artist (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu.");
                    string responseData = await response.Content.ReadAsStringAsync();
                    GetAlbum getAlbumResponse = JsonSerializer.Deserialize<GetAlbum>(responseData);

                    Console.Clear();
                    Console.WriteLine($"Showing albums for {input}:");
                    Console.WriteLine($"--------------------------");

                    foreach (var album in getAlbumResponse.ViewAlbums)
                    {
                        Console.WriteLine($"{album.StrAlbum}, released in {album.IntYearReleased}");
                    }
                    Console.WriteLine($"--------------------------");
                    Console.WriteLine("Press enter to return to menu.");
                    Console.ReadLine();
                    Console.ResetColor();
                }
            }


        }
    }
}
