using MusicProjekt.Services;
using System.Net;

namespace MusicProjekt.ApiHandler
{
    public class ArtistHandler
    {
        public static IResult ListUsersArtists(IDbHelper dbHelper, int userId)
        {
            var artists = dbHelper.ListUsersArtists(userId);
            return Results.Json(artists);
        }

        public static IResult ConnectUserToArtist(IDbHelper dbHelper, int userId, int artistId)
        {
            dbHelper.ConnectUserToArtist(userId, artistId);
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
    }
}
