namespace StreamingServiceApp.DbData
{
    public class MovieReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMovieRepository _movieRepository;

        public MovieReviewService(IReviewRepository reviewRepository, IMovieRepository movieRepository)
        {
            _reviewRepository = reviewRepository;
            _movieRepository = movieRepository;
        }

        public async Task UpdateMovieRatingAsync(int movieId)
        {
            var reviewList = (await _reviewRepository.GetReviewsByMovieIdAsync(movieId)).ToList();
            var movie = await _movieRepository.GetMovieAsync(movieId);
            if (movie != null && reviewList.Any())
            {
                double sumRate = reviewList.Sum(r => r.MovieRating);
                movie.Rating = sumRate / reviewList.Count;

                // Save the updated movie rating back to the database
                await _movieRepository.SaveMovieAsync(movie);
            }
        }
    }

}
