using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MusicProjekt.Services;
using MusicProjekt.ApiHandler;
using MusicProjekt.Data;
using MusicProjekt.Models;
using MusicProjekt.Models.Dtos;
using System.Net.Http;
using System.Text.Json;


namespace MusicProjekt.ApiHandler
{
    public class Program
    {
        public static void Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("ApplicationContext");
            builder.Services.AddDbContext<ApplicationContext>(opt => opt.UseSqlServer(connectionString));
            builder.Services.AddScoped<IDbHelper, DbHelper>();
            var app = builder.Build();
           
            app.MapGet("/", () => "Hello World!");



            app.MapPost("/newUser", UserHandler.AddUser);
            app.MapGet("/user", ApiHandler.ListAllUsers);
          
            app.MapGet("/artist/{userId}", ArtistHandler.ListUsersArtists);
            app.MapPost("/user/{userId}/artist/{artistId}", ArtistHandler.ConnectUserToArtist);
            
            app.MapGet("/genre/{userId}",GenreHandler.ListGenres);
            app.MapPost("/user/{userId}/genre/{genreId}",GenreHandler.ConnectUsersToGenres);
          
            app.MapGet("/user/{userId}", SongHandler.ListUserSongs);
            app.MapPost("/user/{userId}/song/{songId}", SongHandler.ConnectSongToUser);
          
            
            app.MapPost("/song", ApiHandler.AddSong);
            




            app.Run();
        }
    }
}
