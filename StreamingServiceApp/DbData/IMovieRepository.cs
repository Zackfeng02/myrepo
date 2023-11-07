using StreamingServiceApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StreamingServiceApp.DbData
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetMoviesAsync();

        Task<Movie> GetMovieAsync(int id);

        Task SaveMovieAsync(Movie movie);

        Task<Movie> DeleteMovieAsync(int movieID);

        Task UpdateMovieAsync(Movie movie);
    }
}
