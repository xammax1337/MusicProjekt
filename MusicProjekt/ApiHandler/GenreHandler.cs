using MusicProjekt.Models.Dtos;
using MusicProjekt.Services;
using System.Net;

namespace MusicProjekt.ApiHandler
{
    public class GenreHandler
    {
        public static IResult ListGenres(IDbHelper dbHelper,  int userId)
        {
            var genres = dbHelper.GetAllGenresForUser(userId);
            return Results.Json(genres);


        }
        public static IResult ConnectUsersToGenres(IDbHelper dbHandler, int userId, int genreId)
        {
            dbHandler.AddGenreForUser(genreId, userId);
            return Results.StatusCode((int)HttpStatusCode.Created);


        }
    }
}
