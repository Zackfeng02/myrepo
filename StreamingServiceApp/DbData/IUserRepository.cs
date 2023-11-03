using StreamingServiceApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StreamingServiceApp.DbData
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserMoviesAsync(string email);

        Task SaveUserAsync(User user);
    }
}
