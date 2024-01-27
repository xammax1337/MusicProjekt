using MusicProjekt.Models;
using MusicProjekt.Models.Dtos;
using MusicProjekt.Services;
using System.Net;

namespace MusicProjekt.ApiHandler
{
    public class UserHandler
    {
        public static IResult AddUser(IDbHelper dbHelper, UserDto user)
        {
            try
            {
                dbHelper.AddUser(user);
                return Results.StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { Message = ex.Message });
            }

        }
        //Get all users in database
        public static IResult ListAllUsers(IDbHelper dbHelper)
        {
            var users = dbHelper.ListAllUsers();
            return Results.Json(users);
        }
    }
}
