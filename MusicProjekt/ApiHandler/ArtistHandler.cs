using MusicProjekt.Models;
using MusicProjekt.Services;
using System.Net;

namespace MusicProjekt.ApiHandler
{
    public class ArtistHandler
    {
        //Handler for artist methods. IResult are here (in Handler folder) to  enable unit testing.
        public static IResult ListUsersArtists(IDbHelper dbHelper, int userId)
        {
            try
            {
                var artists = dbHelper.ListUsersArtists(userId);
                return Results.Json(artists);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { Message = ex.Message });
            }
        }

        public static IResult ConnectUserToArtist(IDbHelper dbHelper, int userId, int artistId)
        {
            try
            {
                dbHelper.ConnectUserToArtist(userId, artistId);
                return Results.StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { Message = ex.Message });
            }
        }
    }
}
