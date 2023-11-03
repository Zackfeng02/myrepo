using StreamingServiceApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreamingServiceApp.DbData
{
    public class EFMovieRepository : IMovieRepository
    {
        private MovieAppDbContext context;

        public EFMovieRepository(MovieAppDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Movie> Movies => context.Movies;

        public Movie GetMovie(int id)
        {
            Movie dbEntry = context.Movies
                .FirstOrDefault(m => m.MovieId == id);
            return dbEntry;
        }

        public void SaveMovie(Movie movie)
        {
            if (movie.MovieId == 0)
            {
                context.Movies.Add(movie);
            }
            else
            {
                Movie dbEntry = context.Movies
                    .FirstOrDefault(m => m.MovieId == movie.MovieId);
                if (dbEntry != null)
                {
                    dbEntry.MovieName = movie.MovieName;
                    dbEntry.Rating = movie.Rating;
                    dbEntry.ReleaseDate = movie.ReleaseDate;
                    dbEntry.User = movie.User;
                    dbEntry.ImageUrl = movie.ImageUrl;
                    dbEntry.Genre = movie.Genre;
                    dbEntry.Description = movie.Description;
                }
            }
            context.SaveChanges();
        }

        public Movie DeleteMovie(int movieID)
        {
            Movie dbEntry = context.Movies
              .FirstOrDefault(m => m.MovieId == movieID);
            if (dbEntry != null)
            {
                context.Movies.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
