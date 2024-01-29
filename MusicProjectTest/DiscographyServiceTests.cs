using Moq.Protected;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MusicProjekt.Services;

namespace MusicProjectTest
{
    [TestClass]
    public class DiscographyServiceTests
    {
        [TestMethod]
        public async Task GetAlbumAsync_ReturnsCorrectAlbums()
        {
            // Arrange
            //Using mock to make fake HTTP-calls
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                //Protected-method from Moq, that helps in making a fake SendAsync-method.
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                //SendAsync usually would make HTTP calls via HttpMessageHandler
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    //The album title and year that is to be asserted
                    Content = new StringContent("{\"album\": [{\"strAlbum\": \"album1\",\"intYearReleased\": \"year\"}]}")
                });

            //Creating mock HttpClient, that takes in mock HttpMessageHandler
            HttpClient mockClient = new HttpClient(mockHandler.Object);

            //Creating mock DiscographyService, that takes in mock HttpClient
            DiscographyService client = new DiscographyService(mockClient);

            // Act
            var result = await client.GetAlbumAsync("TestArtist");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Albums.Count);
            //Assert checking if album title and year are matching
            Assert.AreEqual("album1", result.Albums[0].StrAlbum); 
            Assert.AreEqual("year", result.Albums[0].IntYearReleased);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task GetAlbumAsync_GetException()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            HttpClient mockClient = new HttpClient(mockHandler.Object);

            DiscographyService client = new DiscographyService(mockClient);

            // Act
            var result = await client.GetAlbumAsync("TestArtist");
        }
    }
}
