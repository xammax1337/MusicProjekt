using Microsoft.EntityFrameworkCore;
using MusicProjekt.Data;
using MusicProjekt.Models;
using MusicProjekt.Models.Dtos;
using MusicProjekt.Models.ViewModel;



namespace MusicProjekt.Services
{

    public interface IDbHelper
    {
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


