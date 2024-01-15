using Microsoft.EntityFrameworkCore;
using MusicProjekt.Data;
using MusicProjekt.Models;
using MusicProjekt.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;
using MusicProjekt.Models.Dtos;

namespace MusicProjekt.Services
{
    public interface IDbHelper
    {
        List<ListUserViewModel> GetAllUsers();
        void AddSong(AddSongDto song);
    }

    public class DbHelper : IDbHelper
    {
        private readonly ApplicationContext _context;
        public DbHelper(ApplicationContext context)
        {
            _context = context;
        }
        public List<ListUserViewModel> GetAllUsers()
        {
            return _context.Users
                .Select(u => new ListUserViewModel { UserId = u.UserId, UserName = u.UserName })
                .ToList();
        }

        public void AddSong(AddSongDto song)
        {
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