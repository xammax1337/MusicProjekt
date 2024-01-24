using Microsoft.EntityFrameworkCore;
using MusicProjekt.Data;
using MusicProjekt.Models;
using MusicProjekt.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;
using MusicProjekt.Models.Dtos;
using Microsoft.IdentityModel.Protocols;

namespace MusicProjekt.Services
{

    public interface IDbHelper
    {
        void AddUser(UserDto user);
        List<ListUserViewModel> ListAllUsers();
        ListUserViewModel GetUserById(int userId);//Nya

        List<ArtistViewModel> ListUsersArtists(int userId);
        void ConnectUserToArtist(int userId, int songId);

        List<ListGenreViewModel> GetAllGenresForUser(int userId);
        void AddGenreForUser(int genreId, int userId);

        List<SongUserViewModel> ListUserSongs(int userId);
        void ConnectSongToUser(int userId, int songId);

        void AddSong(AddSongDto song);

        //Exceptions new
        bool UserExists(string username);
    }


    public class DbHelper : IDbHelper
    {
        private readonly ApplicationContext _context;
        public DbHelper(ApplicationContext context)
        {
            _context = context;
        }

        public bool UserExists(string username)
        {
            return _context.Users.Any(u => u.UserName == username);
        }
        
        public void AddUser(UserDto user)//moved around exception handling from handler, to enable
                                            //exception testing
        {
            if (string.IsNullOrEmpty(user.UserName))
            {
                throw new Exception("User must have a username");
            }
            if (UserExists(user.UserName))
            {
                throw new Exception("Username already exists");
            }

            _context.Users.Add(new User()
            {
                UserName = user.UserName
            });
            _context.SaveChanges();
        }

        public List<ListUserViewModel> ListAllUsers()
        {
            return _context.Users
                .Select(u => new ListUserViewModel { UserId = u.UserId, UserName = u.UserName })
                .ToList();
        }

        //Nya
        public ListUserViewModel GetUserById(int userId)
        {
            var user = _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => new ListUserViewModel { UserId = u.UserId, UserName = u.UserName })
                .FirstOrDefault();

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user;
        }

        public List<ArtistViewModel> ListUsersArtists(int userId)
        {
            User? user = _context.Users
                .Include(u => u.Artists)
                .SingleOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            List<ArtistViewModel> result = user.Artists
               .Select(a => new ArtistViewModel()
               {
                   ArtistName = a.ArtistName
               }).ToList();

            return result;
        }

        public void ConnectUserToArtist(int userId, int artistId)
        {

            User? user = _context.Users
            .Include(u => u.Artists)
            .SingleOrDefault(u => u.UserId == userId);
            Artist? artist = _context.Artists
                .SingleOrDefault(a => a.ArtistId == artistId);

            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (artist == null)
            {
                throw new Exception("Artist not found");
            }
            //tog bort if (user.Artists == null)

            user.Artists.Add(artist);
            _context.SaveChanges();
        }

        public List<ListGenreViewModel> GetAllGenresForUser(int userId)
        {


            User? user = _context.Users
                 .Include(u => u.Genres)
                 .SingleOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            List<ListGenreViewModel> result = user.Genres
                   .Select(a => new ListGenreViewModel()
                   {
                       GenreName = a.GenreName,
                   }).ToList();
            return result;

        }

        public void AddGenreForUser(int genreId, int userId)
        {
            User? user = _context.Users
                .Include(u => u.Genres)
            .SingleOrDefault(u => u.UserId == userId);

            Genre? genre = _context.Genres
              .SingleOrDefault(g => g.GenreId == genreId);

            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (genre == null)
            {
                throw new Exception("Genre not found");
            }

            user.Genres.Add(genre);

            _context.SaveChanges();

        }

        // List all Songs for a Specific User
        public List<SongUserViewModel> ListUserSongs(int userId)
        {
            User? user = _context.Users
                .Include(u => u.Songs)
                .SingleOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            List<SongUserViewModel> userSongs = user.Songs
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

        // Connect a Song to a User
        public void ConnectSongToUser(int userId, int songId)
        {
            User? user = _context.Users
                .Include(u => u.Songs)
                .SingleOrDefault(u => u.UserId == userId);

            Song? song = _context.Songs
                .SingleOrDefault(s => s.SongId == songId);

            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (song == null)
            {
                throw new Exception("Song not found");
            }

            user.Songs.Add(song);
            _context.SaveChanges();
        }

        public void AddSong(AddSongDto song)
        {

            if (string.IsNullOrEmpty(song.Title))
            {
                throw new Exception("Song must have a title");
            }

            //Retrieve artist and genre from our database, to enable exception
            var artist = _context.Artists.Find(song.ArtistId);
            var genre = _context.Genres.Find(song.GenreId);

            if (artist == null)
            {
                throw new Exception("Artist not found");
            }
            if (genre == null)
            {
                throw new Exception("Genre not found");
            }

            var newSong = new Song
            {
                Title = song.Title,
                ArtistId = song.ArtistId,
                GenreId = song.GenreId,
                
            };
            _context.Songs.Add(newSong);
            _context.SaveChanges();
        }
    }
}

