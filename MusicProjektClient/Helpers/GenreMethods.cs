﻿using MusicProjektClient.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicProjektClient.Helpers
{
    public class GenreMethods
    {
        // Asynchronously lists genres associated with a specific user
        public static async Task ListUsersGenres(HttpClient client, int userId)
        {
            // Send HTTP GET request to retrieve user's genres
            var response = await client.GetAsync($"/genre/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                await Console.Out.WriteLineAsync($"Error listing user's genres (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
                return;
            }

            // Read response data
            string responseData = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response to a list of ListUsersGenres objects
            List<ListUsersGenres> genres = JsonSerializer.Deserialize<List<ListUsersGenres>>(responseData);

            foreach (var genre in genres)
            {
                Console.WriteLine($"Genre name: {genre.GenreName}");
            }

            Console.ReadLine();
            Console.Clear();
            await MenuMethods.UserMenu(client, userId);
        }

        // Asynchronously connects a user to a genre
        public static async Task ConnectUserToGenre(HttpClient client, int userId)
        {
            await Console.Out.WriteAsync("Enter genre ID to connect with: ");

            int genreId = Convert.ToInt32(Console.ReadLine());

            // Send HTTP POST request to connect user to the specified genre
            var response = await client.PostAsync($"/user/{userId}/genre/{genreId}", null);

            if (!response.IsSuccessStatusCode)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
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
            await MenuMethods.UserMenu(client, userId);
        }
    }
}
