using Microsoft.EntityFrameworkCore;
using MusicProjekt.Data;
using MusicProjekt.ApiHandler;
using MusicProjekt.Services;
using MusicProjekt.Models;

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

            //Max Methods
            app.MapGet("/{userId}", SongHandler.ListUserSongs);
            app.MapPost("/{userId}/{songId}", SongHandler.AddSongToUser);

            app.Run();
        }
    }
}
