using MusicProjekt.Models;
using MusicProjekt.Models.Dtos;
using MusicProjekt.Services;
using System.Net;

namespace MusicProjekt.ApiHandler
{
    public class GenreHandler
    {
        public static IResult ListUsersGenres(IDbHelper dbHelper,  int userId)
        {
            try
            {
                var genres = dbHelper.GetAllGenresForUser(userId);
                return Results.Json(genres);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { Message = ex.Message });
            }


        }
        public static IResult ConnectUserToGenre(IDbHelper dbHelper, int userId, int genreId)
        {
            try
            {
                dbHelper.AddGenreForUser(genreId, userId);
                return Results.StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { Message = ex.Message });
            }

        }
    }
}
