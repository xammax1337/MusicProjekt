using Microsoft.EntityFrameworkCore;
using MusicProjekt.ApiHandler;
using MusicProjekt.Data;
using MusicProjekt.Models;
using MusicProjekt.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicProjectTest
{
    [TestClass]
    public class UserHandlerTest
    {
        [TestMethod]
        public void GetsAllUsersFromDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("MusicDbTest")
                .Options;

            using (var context = new ApplicationContext(options))
            {
                context.Users.AddRange(
                    new User { UserId = 1, UserName = "Jane" },
                    new User { UserId = 2, UserName = "John" }
                );
                context.SaveChanges();
            }

            using (var context = new ApplicationContext(options))
            {
                var dbHelper = new DbHelper(context);

                // Act
                var result = UserHandler.ListAllUsers(dbHelper);

                // Assert
                Assert.AreEqual(2, context.Users.Count());
                Assert.IsNotNull(result);

            }
        }
    }
}
