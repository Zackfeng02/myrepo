using StreamingServiceApp.Models;

namespace StreamingServiceApp.DbData
{
    public interface IUserService
    {
        Task<User> GetCurrentUserAsync();
    }
}
