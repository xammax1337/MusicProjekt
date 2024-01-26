using MusicProjektClient.ApiModels;
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
            PrintOneByOne(ourText);
            Thread.Sleep(1000);

            PlayIntroToConsoleClient();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7048");

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("[1] View all users");
                    Console.WriteLine("[2] Select a user");
                    Console.WriteLine("[3] Create new user");
                    Console.WriteLine("[4] Add new song");
                    Console.WriteLine("[5] View artist's albums");
                    Console.WriteLine("[Q] Quit program");

                    string input = Console.ReadLine();

                    if (input.ToLower() == "q")
                    {
                        break; 
                    }

                    switch (input)
                    {
                        case "1":
                            Console.Clear();
                            Console.WriteLine("Listing all users. \nPress enter to return to main menu");
                            Console.WriteLine($"--------------------------");
                            await ListAllUsers(client);
                            break;

                        case "2":
                            Console.Clear();
                            Console.WriteLine("Select a user");
                            Console.Write("Enter user ID: ");

                            if (int.TryParse(Console.ReadLine(), out int userId))
                            {
                                bool userExists = await CheckIfUserExists(client, userId);

                                if (userExists)
                                {
                                    Console.WriteLine($"Selected user ID: {userId}");
                                    await UserMenu(client, userId);
                                }
                                else
                                {
                                    PlayWrongInput();
                                    Console.WriteLine("User not found. Please, enter a valid user ID. " +
                                        "\nPress enter to return to main menu.");
                                    Console.ReadLine();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Oops, invalid user ID! Please, enter a valid number.");
                                Console.ReadLine();
                            }
                            Console.ReadLine();
                            break;

                        case "3":
                            Console.WriteLine("Create new user");
                            await AddUser(client);
                            break;

                        case "4":
                            Console.Clear();
                            Console.WriteLine("Add new song");
                            await AddSong(client);
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
                            Console.WriteLine("Nah, invalid input, press enter to return to main menu.");
                            break;
                    }
                }
            }

            static async Task UserMenu(HttpClient client, int userId)
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
                switch(input) 
                { 
                    case "1":
                        Console.Clear();
                        Console.WriteLine($"Listing user's artists. \nPress enter to return to menu.");
                        Console.WriteLine($"--------------------------");

                        await ListUsersArtists(client, userId);
                        break;
                    case "2":
                        Console.Clear();
                        await ConnectUserToArtist(client, userId);
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine($"Listing user's genres. \nPress enter to return to menu.");
                        Console.WriteLine($"--------------------------");

                        await ListUsersGenres(client, userId);
                        break;
                    case "4":
                        Console.Clear();
                        await ConnectUserToGenre(client, userId);
                        break;
                    case "5":
                        Console.Clear();
                        Console.WriteLine($"Listing user's songs. \nPress enter to return to menu.");
                        Console.WriteLine($"--------------------------");

                        await ListUserSongs(client, userId); 
                        break;
                    case "6":
                        Console.Clear();
                        await ConnectSongToUser(client, userId);
                        break;
                    case "q":
                        Console.Clear();
                        Console.WriteLine("Press enter to return to main menu.");
                        return;
                    default:
                        PlayWrongInput();
                        Console.WriteLine("Eek, wrong input!");
                        Console.WriteLine("Press enter to return to menu.");
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
                    Console.Clear();
                    await Console.Out.WriteLineAsync($"Error listing user's (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu.");
                    return false;
                }

                string responseData = await response.Content.ReadAsStringAsync();

                List<User> users = JsonSerializer.Deserialize<List<User>>(responseData);

                return users.Any(user => user.UserId == userId);
            }

            static async Task AddSong(HttpClient client)
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
                    PlayUnsuccessfullAdd();
                    await Console.Out.WriteLineAsync($"Error adding song (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu");
                    return;
                }
                else
                {
                    Console.Clear();
                    PlaySuccessfullAdd();
                    await Console.Out.WriteLineAsync($"Added song (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu.");
                }
                Console.ReadLine();
                Console.Clear();
            }

            static async Task ListAllUsers(HttpClient client)
            {
                var response = await client.GetAsync($"/user/");

                if (!response.IsSuccessStatusCode)
                {
                    await Console.Out.WriteLineAsync($"Error listing user's (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu");
                    return;
                }

                PlayListingSound();
                string responseData = await response.Content.ReadAsStringAsync();

                List<User> users = JsonSerializer.Deserialize<List<User>>(responseData);

                foreach (var user in users)
                {
                    Console.WriteLine($"User ID: {user.UserId}, username: {user.UserName}");
                }

                Console.ReadLine();
            }

            static async Task AddUser(HttpClient client)
            {
                await Console.Out.WriteAsync("Enter username: ");
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
                    Console.Clear();
                    await Console.Out.WriteLineAsync($"Error creating new user (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu.");
                }
                else
                {
                    Console.Clear();
                    PlaySuccessfullAdd();
                    await Console.Out.WriteLineAsync($"Succesfully created a user with username: {userName} (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu.");
                }
                Console.ReadLine();
                Console.Clear();
            }

            static async Task ListUsersArtists(HttpClient client, int userId)
            {
                var response = await client.GetAsync($"/artist/{userId}");

                if (!response.IsSuccessStatusCode)
                {
                    await Console.Out.WriteLineAsync($"Error listing user's artists (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu.");
                    return;
                }

                PlayListingSound();
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
                await Console.Out.WriteAsync("Enter artist ID to connect with: ");

                int artistId = Convert.ToInt32(Console.ReadLine());

                var response = await client.PostAsync($"/user/{userId}/artist/{artistId}", null);

                if (!response.IsSuccessStatusCode)
                {
                    Console.Clear();
                    PlayUnsuccessfullConnect();
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
                await UserMenu(client, userId);
            }

            static async Task ListUsersGenres(HttpClient client, int userId)
            {
                var response = await client.GetAsync($"/genre/{userId}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.Clear();
                    await Console.Out.WriteLineAsync($"Error listing user's genres (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu.");
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
                await Console.Out.WriteAsync("Enter genre ID to connect with: ");

                int genreId = Convert.ToInt32(Console.ReadLine());

                var response = await client.PostAsync($"/user/{userId}/genre/{genreId}", null);

                if (!response.IsSuccessStatusCode)
                {
                    Console.Clear();
                    await Console.Out.WriteLineAsync($"Error connecting user with genre (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu." +
                        $"");
                }
                else
                {
                    Console.Clear();
                    await Console.Out.WriteLineAsync($"Succesfully connected user with genre (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu.");
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
                    Console.Clear();
                    await Console.Out.WriteLineAsync($"Error listing user's songs (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu.");
                    return;
                }

                string responseData = await response.Content.ReadAsStringAsync();

                List<ListUserSongs> songs = JsonSerializer.Deserialize<List<ListUserSongs>>(responseData);

                foreach (var song in songs)
                {
                    Console.WriteLine($"Title: {song.Title}, by: {song.ArtistName}");
                }

                Console.ReadLine();
                Console.Clear();
                await UserMenu(client, userId);
            }

            static async Task ConnectSongToUser(HttpClient client, int userId)
            {
                await Console.Out.WriteAsync("Enter song ID to connect with: ");

                int songId = Convert.ToInt32(Console.ReadLine());

                var response = await client.PostAsync($"/user/{userId}/song/{songId}", null);

                if (!response.IsSuccessStatusCode)
                {
                    Console.Clear();
                    await Console.Out.WriteLineAsync($"Error connecting user with song (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu.");
                }
                else
                {
                    Console.Clear();
                    await Console.Out.WriteLineAsync($"Succesfully connected user with song (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu.");
                }
                Console.ReadLine();
                Console.Clear();
                await UserMenu(client, userId);

            }

            static async Task GetAlbumAsync(HttpClient client)
            {
                Console.Clear();
                await Console.Out.WriteAsync("Enter artist name: ");

                string input = Console.ReadLine();

                var response = await client.GetAsync($"/album/{input}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.Clear();
                    await Console.Out.WriteLineAsync($"Error listing albums for artist (status code: {response.StatusCode}). " +
                        $"\nPress enter to return to menu.");

                }
                else
                {
                    Console.Clear();
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
                }
            }

            static void PrintOneByOne(string text)
            {
                foreach (char c in text)
                {
                    Console.Write(c);
                    Thread.Sleep(100);
                }
            }

            static void PlayIntroToConsoleClient()
            {
                string audioFilePath = "Sounds\\515615__mrthenoronha__8-bit-game-theme (mp3cut.net) (1).wav";

                using (WaveFileReader waveFileReader = new WaveFileReader(audioFilePath))
                {
                    using (WaveOutEvent waveOutEvent = new WaveOutEvent())
                    {
                        waveOutEvent.Init(waveFileReader);

                        waveOutEvent.Play();

                        while (waveOutEvent.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
            }

            static void PlayWrongInput()
            {
                string audioFilePath = "Sounds\\45654__simon_lacelle__dun-dun-dun.wav";

                using (WaveFileReader waveFileReader = new WaveFileReader(audioFilePath))
                {
                    using (WaveOutEvent waveOutEvent = new WaveOutEvent())
                    {
                        waveOutEvent.Init(waveFileReader);

                        waveOutEvent.Play();

                        while (waveOutEvent.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
            }

            static void PlaySuccessfullAdd()
            {
                string audioFilePath = "Sounds\\609336__kenneth_cooney__completed.wav";

                using (WaveFileReader waveFileReader = new WaveFileReader(audioFilePath))
                {
                    using (WaveOutEvent waveOutEvent = new WaveOutEvent())
                    {
                        waveOutEvent.Init(waveFileReader);

                        waveOutEvent.Play();

                        while (waveOutEvent.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
            }

            static void PlayUnsuccessfullAdd()
            {
                string audioFilePath = "Sounds\\29938__halleck__record_scratch_short.wav";

                using (WaveFileReader waveFileReader = new WaveFileReader(audioFilePath))
                {
                    using (WaveOutEvent waveOutEvent = new WaveOutEvent())
                    {
                        waveOutEvent.Init(waveFileReader);

                        waveOutEvent.Play();

                        while (waveOutEvent.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
            }

            static void PlayListingSound()
            {
                string audioFilePath = "Sounds\\253177__suntemple__retro-accomplished-sfx.wav";

                using (WaveFileReader waveFileReader = new WaveFileReader(audioFilePath))
                {
                    using (WaveOutEvent waveOutEvent = new WaveOutEvent())
                    {
                        waveOutEvent.Init(waveFileReader);

                        waveOutEvent.Play();

                        while (waveOutEvent.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
            }

            static void PlayUnsuccessfullConnect()
            {
                string audioFilePath = "Sounds\\29938__halleck__record_scratch_short.wav";

                using (WaveFileReader waveFileReader = new WaveFileReader(audioFilePath))
                {
                    using (WaveOutEvent waveOutEvent = new WaveOutEvent())
                    {
                        waveOutEvent.Init(waveFileReader);

                        waveOutEvent.Play();

                        while (waveOutEvent.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
            }

 

        }
    }
}
