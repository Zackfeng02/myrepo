using StreamingServiceApp.Models;

namespace StreamingServiceApp.DbData
{
    public interface IMovieRepository
    {
        IQueryable<Movie> Movies { get; }

        void SaveMovie(Movie movie);
        Movie DeleteMovie(int movieID);
        Movie GetMovie(int id);
    }
}
