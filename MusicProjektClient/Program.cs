using MusicProjektClient.ApiModels;
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
            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7048");

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("[1] To View all Users");
                    Console.WriteLine("[2] To select a User");
                    Console.WriteLine("[3] Create new User");
                    Console.WriteLine("[4] Add new Song");
                    Console.WriteLine("[5] View artists albums");
                    Console.WriteLine("[Q] Quit Program");

                    string input = Console.ReadLine();

                    if (input.ToLower() == "q")
                    {
                        break; 
                    }

                    switch (input)
                    {
                        case "1":
                            Console.WriteLine("View all users");
                            await ListAllUsers(client);
                            break;

                        case "2":
                            Console.WriteLine("Selecting a user");
                            Console.Write("Enter User ID: ");

                            if (int.TryParse(Console.ReadLine(), out int userId))
                            {
                                bool userExists = await CheckIfUserExists(client, userId);

                                if (userExists)
                                {
                                    Console.WriteLine($"Selected User ID: {userId}");
                                    await UserMenu(client, userId);
                                }
                                else
                                {
                                    Console.WriteLine("User not found. Please enter a valid User ID.");
                                    Console.ReadLine();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid user ID. Please enter a valid number.");
                                Console.ReadLine();
                            }
                            Console.ReadLine();
                            break;

                        case "3":
                            Console.WriteLine("Creating new user");
                            await AddUser(client);
                            break;

                        case "4":
                            Console.WriteLine("Adding new song");
                            await AddSong(client);
                            break;

                        case "5":
                            await GetAlbumAsync(client);
                            break;

                        case "Q":
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine("Invalid input, try again.");
                            break;
                    }
                }
            }

            static async Task UserMenu(HttpClient client, int userId)
            {
                Console.Clear();
                Console.WriteLine($"Current User ID: {userId}");
                Console.WriteLine($"Now choose an option for this user:");
                Console.WriteLine($"--------------------------");
                Console.WriteLine("[1] List User Artists");
                Console.WriteLine("[2] Connect User To Artist");
                Console.WriteLine("[3] List Users Genres");
                Console.WriteLine("[4] Connect User To Genre");
                Console.WriteLine("[5] List User Songs");
                Console.WriteLine("[6] Connect Song To User");
                Console.WriteLine("[Q] Return to Start menu");
                string input = Console.ReadLine();
                switch(input) 
                { 
                    case "1":
                        await ListUsersArtists(client, userId);
                        break;
                    case "2":
                        await ConnectUserToArtist(client, userId);
                        break;
                    case "3":
                        await ListUsersGenres(client, userId);
                        break;
                    case "4":
                        await ConnectUserToGenre(client, userId);
                        break;
                    case "5":
                        await ListUserSongs(client, userId); 
                        break;
                    case "6":
                        await ConnectSongToUser(client, userId);
                        break;
                    case "q":
                        return;
                    default:
                        Console.WriteLine("Wrong input!");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                }
            }

            static async Task<bool> CheckIfUserExists(HttpClient client, int userId)
            {
                var response = await client.GetAsync("/user/");

                if (!response.IsSuccessStatusCode)
                {
                    await Console.Out.WriteLineAsync($"Error listing users (status code {response.StatusCode})");
                    return false;
                }

                string responseData = await response.Content.ReadAsStringAsync();

                List<User> users = JsonSerializer.Deserialize<List<User>>(responseData);

                return users.Any(user => user.UserId == userId);
            }

            static async Task AddSong(HttpClient client)
            {
                await Console.Out.WriteAsync("Enter Song Title: ");
                string title = Console.ReadLine();

                await Console.Out.WriteAsync("Enter Artist Id: ");
                int artistId = Convert.ToInt32(Console.ReadLine());

                await Console.Out.WriteAsync("Enter Genre Id: ");
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
                    await Console.Out.WriteLineAsync($"Error Adding Song (status code {response.StatusCode})");
                    return;
                }
                else
                {
                    await Console.Out.WriteLineAsync($"Added Song (status code {response.StatusCode})");
                }
                Console.ReadLine();
                Console.Clear();
            }

            static async Task ListAllUsers(HttpClient client)
            {
                var response = await client.GetAsync($"/user/");

                if (!response.IsSuccessStatusCode)
                {
                    await Console.Out.WriteLineAsync($"Error listing users (status code {response.StatusCode})");
                    return;
                }

                string responseData = await response.Content.ReadAsStringAsync();

                List<User> users = JsonSerializer.Deserialize<List<User>>(responseData);

                foreach (var user in users)
                {
                    Console.WriteLine($"User ID: {user.UserId}, User Name: {user.UserName}");
                }

                Console.ReadLine();
            }

            static async Task AddUser(HttpClient client)
            {
                await Console.Out.WriteAsync("Enter Username: ");
                string userName = Console.ReadLine();

                AddUser adduser = new AddUser()
                {
                    UserName = userName
                };

                string json = JsonSerializer.Serialize(adduser);

                StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/newUser", jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    await Console.Out.WriteLineAsync($"Error Creating new user (status code {response.StatusCode})");
                }
                else
                {
                    await Console.Out.WriteLineAsync($"Succesfully Created a User with username: {userName} (status code {response.StatusCode})");
                }
                Console.ReadLine();
                Console.Clear();
            }

            static async Task ListUsersArtists(HttpClient client, int userId)
            {
                var response = await client.GetAsync($"/artist/{userId}");

                if (!response.IsSuccessStatusCode)
                {
                    await Console.Out.WriteLineAsync($"Error listing artists (status code {response.StatusCode})");
                    return;
                }

                string responseData = await response.Content.ReadAsStringAsync();

                List<ListUsersArtists> artists = JsonSerializer.Deserialize<List<ListUsersArtists>>(responseData);

                foreach (var artist in artists)
                {
                    Console.WriteLine($"Artist name: {artist.ArtistName}");
                }

                Console.ReadLine();
                Console.Clear();
                await UserMenu(client, userId);
            }

            static async Task ConnectUserToArtist(HttpClient client, int userId)
            {
                await Console.Out.WriteAsync("Enter ArtistId to Connect with: ");

                int artistId = Convert.ToInt32(Console.ReadLine());

                var response = await client.PostAsync($"/user/{userId}/artist/{artistId}", null);

                if (!response.IsSuccessStatusCode)
                {
                    await Console.Out.WriteLineAsync($"Error connecting User with Artist (status code {response.StatusCode})");
                }
                else
                {
                    await Console.Out.WriteLineAsync($"Succesfully connected User with Artist (status code {response.StatusCode})");
                }
                Console.ReadLine();
                Console.Clear();
                await UserMenu(client, userId);
            }

            static async Task ListUsersGenres(HttpClient client, int userId)
            {
                var response = await client.GetAsync($"/genre/{userId}");

                if (!response.IsSuccessStatusCode)
                {
                    await Console.Out.WriteLineAsync($"Error listing genres (status code {response.StatusCode})");
                    return;
                }

                string responseData = await response.Content.ReadAsStringAsync();

                List<ListUsersGenres> genres = JsonSerializer.Deserialize<List<ListUsersGenres>>(responseData);

                foreach (var genre in genres)
                {
                    Console.WriteLine($"Genre name: {genre.GenreName}");
                }

                Console.ReadLine();
                Console.Clear();
                await UserMenu(client, userId);
            }

            static async Task ConnectUserToGenre(HttpClient client, int userId)
            {
                await Console.Out.WriteAsync("Enter GenreId to Connect with: ");

                int genreId = Convert.ToInt32(Console.ReadLine());

                var response = await client.PostAsync($"/user/{userId}/genre/{genreId}", null);

                if (!response.IsSuccessStatusCode)
                {
                    await Console.Out.WriteLineAsync($"Error connecting User with Genre (status code {response.StatusCode})");
                }
                else
                {
                    await Console.Out.WriteLineAsync($"Succesfully connected User with Genre (status code {response.StatusCode})");
                }
                Console.ReadLine();
                Console.Clear();
                await UserMenu(client, userId);
            }

            static async Task ListUserSongs(HttpClient client, int userId)
            {
                var response = await client.GetAsync($"/song/{userId}");

                if (!response.IsSuccessStatusCode)
                {
                    await Console.Out.WriteLineAsync($"Error listing songs (status code {response.StatusCode})");
                    return;
                }

                string responseData = await response.Content.ReadAsStringAsync();

                List<ListUserSongs> songs = JsonSerializer.Deserialize<List<ListUserSongs>>(responseData);

                foreach (var song in songs)
                {
                    Console.WriteLine($"Title: {song.Title}, By: {song.ArtistName}");
                }

                Console.ReadLine();
                Console.Clear();
                await UserMenu(client, userId);
            }

            static async Task ConnectSongToUser(HttpClient client, int userId)
            {
                await Console.Out.WriteAsync("Enter SongId to Connect with: ");

                int songId = Convert.ToInt32(Console.ReadLine());

                var response = await client.PostAsync($"/user/{userId}/song/{songId}", null);

                if (!response.IsSuccessStatusCode)
                {
                    await Console.Out.WriteLineAsync($"Error connecting User with Song (status code {response.StatusCode})");
                }
                else
                {
                    await Console.Out.WriteLineAsync($"Succesfully connected User with Song (status code {response.StatusCode})");
                }
                Console.ReadLine();
                Console.Clear();
                await UserMenu(client, userId);

            }

            static async Task GetAlbumAsync(HttpClient client)
            {
                await Console.Out.WriteAsync("Enter Artist Name: ");

                string input = Console.ReadLine();

                var response = await client.GetAsync($"/album/{input}");

                if (!response.IsSuccessStatusCode)
                {
                    await Console.Out.WriteLineAsync($"Error listing albums for artist (status code {response.StatusCode})");

                }
                else
                {
                    await Console.Out.WriteLineAsync($"Successfully listing albums for artist (status code {response.StatusCode})");
                    string responseData = await response.Content.ReadAsStringAsync();
                    GetAlbum getAlbumResponse = JsonSerializer.Deserialize<GetAlbum>(responseData);

                    Console.Clear();
                    Console.WriteLine($"Showing albums for {input}:");
                    Console.WriteLine($"--------------------------");

                    foreach (var album in getAlbumResponse.ViewAlbums)
                    {
                        Console.WriteLine($"{album.StrAlbum}, {album.IntYearReleased}");
                    }

                    Console.ReadLine();
                }
            }

        }
    }
}
