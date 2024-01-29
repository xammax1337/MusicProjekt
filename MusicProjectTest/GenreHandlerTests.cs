using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using MusicProjekt.ApiHandler;
using MusicProjekt.Data;
using MusicProjekt.Models;
using MusicProjekt.Models.Dtos;
using MusicProjekt.Models.ViewModel;
using MusicProjekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;



namespace MusicProjectTest
{
    [TestClass]
    public class GenreHandlerTests
    {
        // Test method to ensure that adding a genre to a user works as expected
        [TestMethod]
        public void AddGenreForUser_Should_Add_Genre_To_User()
        {
            // Arrange
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("MusicDbTest")
                .Options;

            using (var _context = new ApplicationContext(options))
            {
                _context.Genres.Add(new Genre { GenreId = 1, GenreName = "Pop" });
                _context.SaveChanges();
            }


            ApplicationContext context = new ApplicationContext(options);
            DbHelper dbHelper = new DbHelper(context);

            // Act
            dbHelper.AddUser(new UserDto { UserName = "TestUser" });

            dbHelper.AddGenreForUser(1, 1);

            // Assert
            var userGenres = dbHelper.GetAllGenresForUser(1);
            Assert.IsNotNull(userGenres);
            Assert.AreEqual(1, userGenres.Count);
            Assert.AreEqual("Pop", userGenres[0].GenreName);

        }

        // Test method to ensure that attempting to add a genre for a non-existing user throws an exception
        [TestMethod]
        public void AddGenreForNonExistingUser_Should_Throw_Exception()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("MusicDbTest")
                .Options;

            using (var context = new ApplicationContext(options))
            {
                var dbHelper = new DbHelper(context);

                // Act & Assert
                Assert.ThrowsException<Exception>(() => dbHelper.AddGenreForUser(1, 999));
            }
        }

        // Test method to ensure that retrieving all genres for a user works as expected
        [TestMethod]
        public void GetAllGenresForUser_Should_Return_Genres()
        {
            // Arrange
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("MusicDb")
                .Options;

            using (var _context = new ApplicationContext(options))
            {
                _context.Users.Add(new User { UserId = 2, UserName = "B", Genres = new List<Genre> { new Genre { GenreName = "Rock" } } });
                _context.SaveChanges();
            }

            ApplicationContext context = new ApplicationContext(options);
            DbHelper dbHelper = new DbHelper(context);

            // Act
            var userGenres = dbHelper.GetAllGenresForUser(2);

            // Assert
            Assert.IsNotNull(userGenres);
            Assert.AreEqual(1, userGenres.Count);

            Assert.AreEqual("Rock", userGenres[0].GenreName);
        }

        // Test method to ensure that listing genres for a user through the GenreHandler works as expected
        [TestMethod]
        public void ListUsersGenres_Should_Return_Genres()
        {
            // Arrange
            var userId = 1;
            var expectedGenres = new List<ListGenreViewModel>
            {
             new ListGenreViewModel { GenreName = "Rock" }
            };

            var mockDbHelper = new Mock<IDbHelper>();
            mockDbHelper.Setup(d => d.GetAllGenresForUser(userId)).Returns(expectedGenres);

            // Act
            var result = GenreHandler.ListUsersGenres(mockDbHelper.Object, userId);

            //Assert
            mockDbHelper.Verify(g => g.GetAllGenresForUser(It.IsAny<int>()), Times.Once());
            Assert.IsNotNull(result);


        }
    } 
}

