using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicProjekt.Data;
using MusicProjekt.Models;
using MusicProjekt.Models.Dtos;
using MusicProjekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicProjectTest
{
    [TestClass]
    public class DbHelperSongTests
    {
        private ApplicationContext _context;
        private DbHelper _dbHelper;
        
        // Creates the TestDb using InMemory so every TestMethod that needs it can use it.
        [TestInitialize]
        public void TestInitialize()
        {
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _context = new ApplicationContext(options);
            _dbHelper = new DbHelper(_context);
        }

        // ListUserSongs Tests
        [TestMethod]
        public void ListUserSongs_ThrowsException_UserNotFound()
        {

        }


        // ConnectSongToUser Tests
        [TestMethod]
        public void ConnectSongToUser_Connects()
        {
            // Arrange
            _context.Users.Add(new User { UserId = 5, UserName = "TestConnectUser" });
            _context.SaveChanges();

            _context.Songs.Add(new Song { SongId = 5, ArtistId = 5, GenreId = 5, Title = "TestConnectTitle" }); ;
            _context.SaveChanges();

            // Act
            _dbHelper.ConnectSongToUser(5, 5);

            // Assert
            User user = _context.Users.Include(u => u.Songs).FirstOrDefault(u => u.UserId == 5);
            Assert.IsNotNull(user);
            Assert.AreEqual(1, user.Songs.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ConnectSongToUser_UserNotFound()
        {
            // Arrange

            // Act
            _dbHelper.ConnectSongToUser(0, 5);

            // Assert
            User user = _context.Users.Include(u => u.Songs).FirstOrDefault(u => u.UserId == 5);
            Assert.IsNotNull(user);
            Assert.AreEqual(1, user.Songs.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ConnectSongToUser_SongNotFound()
        {
            // Arrange

            // Act
            _dbHelper.ConnectSongToUser(5, 0);

            // Assert
            User user = _context.Users.Include(u => u.Songs).FirstOrDefault(u => u.UserId == 5);
            Assert.IsNotNull(user);
            Assert.AreEqual(1, user.Songs.Count);
        }

        // AddSong Tests
        [TestMethod]
        public void AddSong_AddsNewSong()
        {
            // Arrange
            _context.Artists.Add(new Artist { ArtistId = 1, ArtistName = "TestArtist", Description = "TestDescription" });
            _context.SaveChanges();

            _context.Genres.Add(new Genre { GenreId = 1, GenreName = "TestGenre" });
            _context.SaveChanges();

            // Act
            _dbHelper.AddSong(new AddSongDto()
            {
                Title = "TestTitle",
                ArtistId = 1,
                GenreId = 1,
            }) ;

            // Assert
            Assert.AreEqual(1, _context.Songs.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddSong_AddsNewSong_ArtistNotFound()
        {
            // Arrange

            // Act
            _dbHelper.AddSong(new AddSongDto()
            {
                Title = "TestTitle2",
                ArtistId = 2,
                GenreId = 1,
            });

            // Assert
            Assert.AreEqual(1, _context.Songs.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddSong_AddsNewSong_GenreNotFound()
        {
            // Arrange

            // Act
            _dbHelper.AddSong(new AddSongDto()
            {
                Title = "TestTitle3",
                ArtistId = 1,
                GenreId = 2,
            });

            // Assert
            Assert.AreEqual(1, _context.Songs.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddSong_AddsNewSong_TitleNull()
        {
            // Arrange

            // Act
            _dbHelper.AddSong(new AddSongDto()
            {
                Title = "",
                ArtistId = 1,
                GenreId = 1,
            });

            // Assert
            Assert.AreEqual(1, _context.Songs.Count());
        }
    }
}
