using MusicProjekt.Models.Dtos;
using MusicProjekt.Services;
using System.Net;

namespace MusicProjekt.ApiHandler
{
    public class UserHandler
    {
        public static IResult AddUser(IDbHelper dbHelper, UserDto user)
        {
            if (string.IsNullOrEmpty(user.UserName))
            {
                return Results.BadRequest(new { Message = "User must have a username" });
            }

            if (dbHelper.UserExists(user.UserName))
            {
                return Results.BadRequest(new { Message = "Username already exists" });
            }

            else
            {
                dbHelper.AddUser(user);
                return Results.StatusCode((int)HttpStatusCode.Created);
            }

        }
        public static IResult ListAllUsers(IDbHelper dbHelper)
        {
            var users = dbHelper.ListAllUsers();
            return Results.Json(users);
        }
    }
}
