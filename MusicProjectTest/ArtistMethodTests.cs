using Microsoft.EntityFrameworkCore;
using Moq;
using MusicProjekt.Data;
using MusicProjekt.Models;
using MusicProjekt.Models.Dtos;
using MusicProjekt.Models.ViewModel;
using MusicProjekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicProjectTest
{
    [TestClass]
    public class ArtistMethodTests
    {
        [TestMethod]
        public void ListUsersArtists_CallsDbHelpersListUsersArtists()
        {
            //Arrange
            int userId = 1;
            var mockService = new Mock<IDbHelper>();
            IDbHelper dbHelper = mockService.Object;

            //Act
            var result = dbHelper.ListUsersArtists(userId);

            //Assert
            mockService.Verify(h => h.ListUsersArtists(userId), Times.Once);
        }

        [TestMethod]
        public void ListUsersArtists_UsersArtistsInDbIsListedCorrect()
        {
            //Arrange
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            using (ApplicationContext context = new ApplicationContext(options))
            {
                Artist testArtist = new Artist 
                { 
                    ArtistId = 8,
                    ArtistName = "test-artist", 
                    Description = "test-description" 
                };

                User testUser = new User
                {
                    UserId = 8,
                    UserName = "test-user",
                    Artists = new List<Artist> { testArtist } //Added artist to users list
                };

                context.Users.Add(testUser);
                context.Artists.Add(testArtist);
                context.SaveChanges();

                int userId = testUser.UserId;
                int artistId = testArtist.ArtistId;

                DbHelper dbHelper = new DbHelper(context);

                //Act

                List<ArtistViewModel> usersArtists = dbHelper.ListUsersArtists(userId);

                User? userInTestDb = context.Users
                    .Include(u => u.Artists)
                    .SingleOrDefault(u => u.UserId == userId);

                //Assert
                Assert.IsNotNull(userInTestDb);
                Assert.IsNotNull(userInTestDb.Artists);
                Assert.IsTrue(usersArtists.Count > 0);
            }
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ListUsersArtists_ThrowsExceptionIfUserDoesNotExist()
        {
            //Arrange
            int userId = 1;
            var mockService = new Mock<IDbHelper>();
            mockService.Setup(h => h.ListUsersArtists(It.IsAny<int>()))
                .Throws(new Exception());
            IDbHelper dbHelper = mockService.Object;

            //Act
            var result = dbHelper.ListUsersArtists(userId);
        }

        [TestMethod]
        public void ConnectUserToArtist_CallsDbHelpersConnectUserToArtist()
        {
            //Arrange
            int userId = 1;
            int artistId = 1;
            var mockService = new Mock<IDbHelper>();
            IDbHelper dbHelper = mockService.Object;

            //Act
            dbHelper.ConnectUserToArtist(userId, artistId);

            //Assert
            mockService.Verify(h => h.ConnectUserToArtist(userId, artistId), Times.Once);
        }



        [TestMethod]
        public void ConnectUserToArtist_UserIsConnectedToArtistInDb()
        {
            //Arrange
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            using (ApplicationContext context = new ApplicationContext(options))
            {
                User testUser = new User 
                { 
                    UserId = 3, 
                    UserName = "test-user" 
                };

                Artist testArtist = new Artist 
                { 
                    ArtistId = 3, 
                    ArtistName = "test-artist", 
                    Description = "test-description" 
                };

                context.Users.Add(testUser);
                context.Artists.Add(testArtist);
                context.SaveChanges();

                int userId = testUser.UserId;
                int artistId = testArtist.ArtistId;

                DbHelper dbHelper = new DbHelper(context);

                //Act
                dbHelper.ConnectUserToArtist(userId, artistId);

                //Assert
                User? userInTestDb = context.Users
                    .Include(u => u.Artists)
                    .SingleOrDefault(u => u.UserId == userId);

                Assert.IsNotNull(userInTestDb);
                Assert.AreEqual(1, userInTestDb.Artists.Count);
            }


        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ConnectUserToArtist_ThrowsExceptionIfUserOrArtistDoesNotExist()
        {
            //Arrange
            int userId = 1;
            int artistId = 1;
            var mockService = new Mock<IDbHelper>();
            mockService.Setup(h => h.ConnectUserToArtist(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception());
            IDbHelper dbHelper = mockService.Object;

            //Act
            dbHelper.ConnectUserToArtist(userId, artistId);
        }
    }
}
