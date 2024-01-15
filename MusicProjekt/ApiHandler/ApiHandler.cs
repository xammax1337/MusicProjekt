using MusicProjekt.Data;
using MusicProjekt.Models;
using MusicProjekt.Models.Dtos;
using MusicProjekt.Models.ViewModel;
using Microsoft.AspNetCore.Mvc; //?
using MusicProjekt.Services;
using System.Net;

namespace MusicProjekt.ApiHandler
{
    public static class ApiHandler
    {
       
        public static IResult ListUsersGenres(IDbHelper dbHelper, int genreId,int userId)
        {
            var genres = dbHelper.GetAllGenresForUser( userId);
            return Results.Json(genres);
            
            
        }

        public static IResult AddUserToGenre(IDbHelper dbHandler, int userId, int genreId, GenreDto genreDtol)
        {


            dbHandler.AddGenreForUser(genreId, userId);
            return Results.StatusCode((int)HttpStatusCode.Created);


        }

      
    }
}
