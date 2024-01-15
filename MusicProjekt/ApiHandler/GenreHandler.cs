using MusicProjekt.Models.Dtos;
using MusicProjekt.Services;
using System.Net;

namespace MusicProjekt.ApiHandler
{
    public class GenreHandler
    {
        public static IResult ListUsersGenres(IDbHelper dbHelper,  int userId)
        {
            var genres = dbHelper.GetAllGenresForUser(userId);
            return Results.Json(genres);


        }
        public static IResult ConnectUserToGenre(IDbHelper dbHelper, int userId, int genreId)
        {
            dbHelper.AddGenreForUser(genreId, userId);
            return Results.StatusCode((int)HttpStatusCode.Created);


        }
    }
}
