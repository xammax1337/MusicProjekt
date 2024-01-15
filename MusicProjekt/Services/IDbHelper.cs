using Microsoft.EntityFrameworkCore;
using MusicProjekt.Data;
using MusicProjekt.Models;
using MusicProjekt.Models.Dtos;
using MusicProjekt.Models.ViewModel;


namespace MusicProjekt.Services
{

    public interface IDbHelper
    {
        void AddUser(UserDto user);
        List<ArtistViewModel> ListUsersArtists(int userId);
        void ConnectUserToArtist(int userId, int songId);

        List<ListGenreViewModel> GetAllGenresForUser( int userId);
        void AddGenreForUser( int genreId,int userId);


    }


    public class DbHelper : IDbHelper
    {
        private ApplicationContext _context;

        public DbHelper(ApplicationContext context)
        {
            _context = context;
        }


        public void AddUser(UserDto user)
        {
            if (string.IsNullOrEmpty(user.UserName))
            {
                Results.BadRequest(new { Message = "User must have a username" });
            }

            _context.Users.Add(new User()
            {
                UserName = user.UserName
            });
            _context.SaveChanges();
        }
        public List<ArtistViewModel> ListUsersArtists(int userId)
        {
            User? user = _context.Users
                .Include(u => u.Artists)
                .SingleOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                Results.NotFound();
            }

            if (user.Artists == null)
            {
                Results.NotFound();
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
                .Include (u => u.Artists)
                .SingleOrDefault (u => u.UserId == userId);

            if (user == null)
            {
                Results.NotFound();
            }

            Artist? artist = _context.Artists
                .SingleOrDefault(a => a.ArtistId == artistId);

            if (user.Artists == null)
            {
                Results.NotFound();
            }
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
                    Results.NotFound();
                }
                if (user.Artists == null)
                {
                    Results.NotFound();
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
                .Include(u=>u.Genres)
            .SingleOrDefault(u => u.UserId == userId);

              Genre? genre = _context.Genres
                .SingleOrDefault(g => g.GenreId == genreId);
   
            user.Genres.Add(genre);

            _context.SaveChanges();

        }
        
    }
}


