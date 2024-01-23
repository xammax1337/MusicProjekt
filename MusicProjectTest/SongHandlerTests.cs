using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MusicProjectTest
{
    [TestClass]
    public class SongHandlerTests
    {
        [TestMethod]
        public void ConnectSongToUser_CallsDbHelper_ConnectSongtoUser()
        {
            // Arrange
            var mockService = new Mock<IDbHelper>();
            mockService.Setup(m => m.ConnectSongToUser(It.IsAny<int>(), It.IsAny<int>()));
            var userId = 1;
            var songId = 1;

            // Act
            var result = SongHandler.ConnectSongToUser(mockService.Object, userId, songId);

            // Assert
            mockService.Verify(m => m.ConnectSongToUser(userId, songId), Times.Once);
        }

        [TestMethod]
        public void ListUserSongs_ChecksResult()
        {
            // Arrange
            var mockService = new Mock<IDbHelper>();
            var expectedUserSongs = new List<SongUserViewModel>();
            mockService.Setup(db => db.ListUserSongs(1)).Returns(expectedUserSongs);

            // Act
            var result = SongHandler.ListUserSongs(mockService.Object, 1);

            // Assert
            mockService.Verify(dh => dh.ListUserSongs(1), Times.Once());
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IResult));
        }

        [TestMethod]
        public void AddSong_CallsDbHelper_AddSong()
        {
            // Arrange
            var mockService = new Mock<IDbHelper>();
            IDbHelper dbHelper = mockService.Object;

            AddSongDto songDto = new AddSongDto()
            {
                Title = "Test_Title",
                ArtistId = 1,
                GenreId = 1,
            };

            // Act
            SongHandler.AddSong(dbHelper, songDto);

            // Assert
            mockService.Verify(s => s.AddSong(songDto), Times.Once());
        }
    }
}
