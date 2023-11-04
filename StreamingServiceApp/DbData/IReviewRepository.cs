using StreamingServiceApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StreamingServiceApp.DbData
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetReviewsByMovieIdAsync(int movieId);
        Task SaveReviewAsync(Review review);

    }
}
