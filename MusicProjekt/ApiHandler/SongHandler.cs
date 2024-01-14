using MusicProjekt.Data;
using MusicProjekt.Models;
using MusicProjekt.Services;
using System.Net;

namespace MusicProjekt.ApiHandler
{
    public class SongHandler
    {

        public static IResult AddSongToUser(IDbHelper dbHelper, int userId, int songId)
        {
            dbHelper.AddSongToUser(userId, songId);
            return Results.StatusCode((int)HttpStatusCode.Created);
        }

        public static IResult ListUserSongs (IDbHelper dbHelper, int userId)
        {
            var userSongs = dbHelper.GetUserSongs(userId);
            return Results.Json(userSongs);
        }
    }
}
