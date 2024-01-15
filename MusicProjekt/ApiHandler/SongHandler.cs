using MusicProjekt.Data;
using MusicProjekt.Models;
using MusicProjekt.Services;
using System.Net;

namespace MusicProjekt.ApiHandler
{
    public class SongHandler
    {

        public static IResult ConnectSongToUser(IDbHelper dbHelper, int userId, int songId)
        {
            dbHelper.ConnectSongToUser(userId, songId);
            return Results.StatusCode((int)HttpStatusCode.Created);
        }

        public static IResult ListUserSongs (IDbHelper dbHelper, int userId)
        {
            var userSongs = dbHelper.ListUserSongs(userId);
            return Results.Json(userSongs);
        }
    }
}
