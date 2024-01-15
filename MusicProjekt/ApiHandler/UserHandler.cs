using MusicProjekt.Models.Dtos;
using MusicProjekt.Services;
using System.Net;

namespace MusicProjekt.ApiHandler
{
    public class UserHandler
    {
        public static IResult AddUser(IDbHelper dbHelper, UserDto user)
        {
            dbHelper.AddUser(user);
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
    }
}
