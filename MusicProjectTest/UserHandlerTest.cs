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
using Moq;
using MusicProjekt.Models.Dtos;

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
                .UseInMemoryDatabase("MusicDatabaseTesting")
                .Options;

            using (var context = new ApplicationContext(options))
            {
                context.Users.AddRange(
                    new User { UserId = 11, UserName = "Jane" },
                    new User { UserId = 12, UserName = "John" }
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

        [TestMethod]
        public void AddUser_CallsDbsHelpersAddUser()
        {
            //Arrange
            string userName = "test-user";
            var mockService = new Mock<IDbHelper>();
            IDbHelper dbHelper = mockService.Object;

            UserDto user = new UserDto()
            {
                UserName = "TestUserName"
            };

            //Act
            dbHelper.AddUser(user);

            //Assert
            mockService.Verify(h => h.AddUser(user), Times.Once);
        }


        [TestMethod]
        public void AddUser_AddsNewUserToDb()
        {
            //Arrange
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDatabase1")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            DbHelper dbHelper = new DbHelper(context);

            //Act
            dbHelper.AddUser(new UserDto()
            {
                UserName = "TestUserName"
            });

            //Assert
            //Checks that a user exists in fake database (method is working)
            Assert.AreEqual(1, context.Users.Count());
        }

        [TestMethod]
        public void AddUser_ThrowsExceptionIfUserNameAlreadyExists()
        {
            //Arrange
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb3")
                .Options;

            using (ApplicationContext context = new ApplicationContext(options))
            {
                DbHelper dbHelper = new DbHelper(context);

                dbHelper.AddUser(new UserDto
                {
                    UserName = "test-user"
                });
                context.SaveChanges();

                //Act
                UserDto userDto = new UserDto
                {
                    UserName = "test-user"
                };

                //Assert
                //Using Assert.ThrowsException instead of ExpectedException
                Assert.ThrowsException<Exception>(() => dbHelper.AddUser(userDto));

            }

        }

    }
}
