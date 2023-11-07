using StreamingServiceApp.Models;
using System.Security.Claims;

namespace StreamingServiceApp.DbData
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public UserService(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userId, out int id))
            {
                return await _context.Users.FindAsync(id);
            }
            return null; // or throw an exception if user is not found/signed in
        }
    }

}
