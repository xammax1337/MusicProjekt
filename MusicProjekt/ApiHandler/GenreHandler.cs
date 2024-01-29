using MusicProjekt.Models;
using MusicProjekt.Models.Dtos;
using MusicProjekt.Services;
using System.Net;

namespace MusicProjekt.ApiHandler
{
    // GenreHandler provides HTTP request handling for genre-related operations
    public class GenreHandler
    {
        // Retrieve all genres associated with a specific user
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
        // Connect a user to a genre
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
