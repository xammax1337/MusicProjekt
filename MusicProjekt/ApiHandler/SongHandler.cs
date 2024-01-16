using MusicProjekt.Data;
using MusicProjekt.Models;
using MusicProjekt.Models.Dtos;
using MusicProjekt.Services;
using System.Net;

namespace MusicProjekt.ApiHandler
{
    public class SongHandler
    {

        public static IResult ConnectSongToUser(IDbHelper dbHelper, int userId, int songId)
        {
            try
            {
                dbHelper.ConnectSongToUser(userId, songId);
                return Results.StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { Message = ex.Message });
            }
        }

        public static IResult ListUserSongs (IDbHelper dbHelper, int userId)
        {
            try
            {
                var userSongs = dbHelper.ListUserSongs(userId);
                return Results.Json(userSongs);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { Message = ex.Message });
            }
        }

        public static IResult AddSong(IDbHelper dbHelper, AddSongDto song)
        {
            try
            {
                dbHelper.AddSong(song);
                return Results.StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { Message = ex.Message });
            }
        }
    }
}
