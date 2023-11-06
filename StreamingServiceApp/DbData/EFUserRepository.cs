using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StreamingServiceApp.Models;

namespace StreamingServiceApp.DbData
{
    public class EFUserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public EFUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserMoviesAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task SaveUserAsync(User user)
        {
            if (user.UserId == 0)
            {
                _context.Users.Add(user);
            }
            else
            {
                User dbEntry = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
                if (dbEntry != null)
                {
                    dbEntry.Email = user.Email;
                    dbEntry.Password = user.Password;
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
