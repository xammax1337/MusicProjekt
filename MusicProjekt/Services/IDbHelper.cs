using MusicProjekt.Data;

namespace MusicProjekt.Services
{
    public interface IDbHelper
    {
    }

    public class DbHelper : IDbHelper
    {
        private ApplicationContext _context;

        public DbHelper(ApplicationContext context)
        {
            _context = context;
        }
    }
}
