using Microsoft.EntityFrameworkCore;
using MusicProjekt.Data;
using MusicProjekt.Models;
using MusicProjekt.Models.Dtos;
using MusicProjekt.Services;
using MusicProjekt.ApiHandler;

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
            
            app.MapGet("/user", ApiHandler.ListAllUsers);
            app.MapPost("/song", ApiHandler.AddSong);
            
            app.Run();
        }
    }
}
