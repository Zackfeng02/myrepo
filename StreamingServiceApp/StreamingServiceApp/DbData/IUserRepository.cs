using StreamingServiceApp.Models;

namespace StreamingServiceApp.DbData
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
        void SaveUser(User user);
        User GetUserMovies(string email);
    }
}
