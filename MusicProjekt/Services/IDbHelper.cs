using Microsoft.EntityFrameworkCore;
using MusicProjekt.Data;
using MusicProjekt.Models;
using MusicProjekt.Models.Dtos;
using MusicProjekt.Models.ViewModel;

namespace MusicProjekt.Services
{
    public interface IDbHelper
    {
        List<SongUserViewModel> ListUserSongs(int userId);
        void ConnectSongToUser(int userId, int songId); 
    }

    public class DbHelper : IDbHelper
    {
        private ApplicationContext _context;

        public DbHelper(ApplicationContext context)
        {
            _context = context;
        }


        // Connect a Song to a User
        public void ConnectSongToUser(int userId, int songId)
        {
            User? user = _context.Users
                .Include(u => u.Songs)
                .SingleOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                Results.NotFound();
            }

            Song? song = _context.Songs
                .SingleOrDefault(s => s.SongId == songId);

            if (song == null)
            {
                Results.NotFound();
            }

            user.Songs.Add(song);
            _context.SaveChanges();
        }

        // List all Songs for a Specific User
        public List<SongUserViewModel> ListUserSongs(int userId) 
        {
            List<SongUserViewModel> userSongs = _context.Users
                .Where(u => u.UserId == userId)
                .Include(u => u.Songs)
                .SelectMany(u => u.Songs)
                .Join(
                    _context.Artists,
                    song => song.ArtistId,
                    artist => artist.ArtistId,
                    (song, artist) => new SongUserViewModel 
                    {
                        Title = song.Title,
                        ArtistName = artist.ArtistName
                    })
                .ToList();

            return userSongs;
        }
    }
}
