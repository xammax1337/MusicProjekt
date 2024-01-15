
using System.Net;
using MusicProjekt.Models;
using MusicProjekt.Models.Dtos;
using MusicProjekt.Services;
namespace MusicProjekt.ApiHandler 
{ 

    public class ApiHandler
    {
        public static IResult AddSong(IDbHelper dbHelper, AddSongDto song)
        {
            dbHelper.AddSong(song);
            return Results.StatusCode((int)HttpStatusCode.Created);
        }

        public static IResult ListUsers(IDbHelper dbHelper)
        {
            var users = dbHelper.GetAllUsers();
            return Results.Json(users);
        }
    }
}
