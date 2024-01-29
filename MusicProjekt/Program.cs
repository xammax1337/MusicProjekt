using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MusicProjekt.Services;
using MusicProjekt.ApiHandler;
using MusicProjekt.Data;
using MusicProjekt.Models;
using MusicProjekt.Models.Dtos;
using System.Net.Http;
using System.Text.Json;
using MusicProjekt.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;


namespace MusicProjekt.ApiHandler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //A container for dependency injection, also to help with unit testing
            builder.Services.AddScoped<IDiscographyService, DiscographyService>();

            string connectionString = builder.Configuration.GetConnectionString("ApplicationContext");
            builder.Services.AddDbContext<ApplicationContext>(opt => opt.UseSqlServer(connectionString));
            //Another container for dependency injection
            builder.Services.AddScoped<IDbHelper, DbHelper>();
            var app = builder.Build();
           
            app.MapGet("/", () => "Hello World!");

        
            //Endpoints for API
            app.MapPost("/newUser", UserHandler.AddUser); 
            app.MapGet("/user", UserHandler.ListAllUsers); 
          
            app.MapGet("/artist/{userId}", ArtistHandler.ListUsersArtists); 
            app.MapPost("/user/{userId}/artist/{artistId}", ArtistHandler.ConnectUserToArtist); 
            
            app.MapGet("/genre/{userId}",GenreHandler.ListUsersGenres); 
            app.MapPost("/user/{userId}/genre/{genreId}",GenreHandler.ConnectUserToGenre);
          
            app.MapGet("/song/{userId}", SongHandler.ListUserSongs);
            app.MapPost("/user/{userId}/song/{songId}", SongHandler.ConnectSongToUser);
          
            
            app.MapPost("/song", SongHandler.AddSong);

            //GET call for external API. It lists a selected artist's discography.
            app.MapGet("album/{artist}", async (string artist, IDiscographyService DiscographyService) =>
            {
                try
                {
                    DiscographyDto testApi = await DiscographyService.GetAlbumAsync(artist);

                    List<DiscographyDto.Album> albums = testApi?.Albums;

                    DiscographyViewModel result = new DiscographyViewModel
                    {
                        ViewAlbums = albums?.Select(dtoAlbum => new DiscographyViewModel.ViewAlbum
                        {
                            StrAlbum = dtoAlbum.StrAlbum,
                            IntYearReleased = dtoAlbum.IntYearReleased
                        }).ToList()
                    };

                    return Results.Json(result);
                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately
                    return Results.Json(new { ErrorMessage = ex.Message });
                }
            });
            app.Run();
        }
    }
}
