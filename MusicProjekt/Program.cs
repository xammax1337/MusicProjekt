using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MusicProjekt.ApiHandler;
using MusicProjekt.Data;
using MusicProjekt.Models;
using MusicProjekt.Models.Dtos;
using MusicProjekt.Services;

namespace MusicProjekt
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

            app.MapGet("/genre/{userId}",GenreHandler.ListGenres);
            app.MapPost("/user/{userId}/genre/{genreId}",GenreHandler.ConnectUsersToGenres);
            


            app.Run();
        }
    }
}
