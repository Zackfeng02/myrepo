using StreamingServiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StreamingServiceApp.DbData;

namespace StreamingServiceApp.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMovieRepository _movieRepository;
        private List<Review> reviewList;
        private readonly IUserService _userService;

        public ReviewsController(IReviewRepository reviewRepo, IMovieRepository movieRepo, IUserService userService)
        {
            _reviewRepository = reviewRepo;
            _movieRepository = movieRepo;
            reviewList = new List<Review>();
            _userService = userService;
        }

        public async Task<IActionResult> ReviewsList(int movieId)
        {
            reviewList = (await _reviewRepository.GetReviewsByMovieIdAsync(movieId)).ToList();
            return View(reviewList);
        }

        [HttpGet]
        public async Task<IActionResult> AddReview(int id)
        {
            Movie movie = await _movieRepository.GetMovieAsync(id);
            if (movie != null)
            {
                MovieReview mrm = new MovieReview()
                {
                    MovieId = id,
                    Review = new Review()
                };
                TempData["movieName"] = $"{movie.MovieName}";
                return View(mrm);
            }
            else
            {
                return RedirectToAction("Index", "Movies");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(MovieReview movieReview)
        {
            // Retrieve the current user
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                // If no user is logged in, add an error to the ModelState and return the view
                ModelState.AddModelError(string.Empty, "User must be logged in to add a review.");
                return View(movieReview);
            }

            // Generate a new ReviewID if it's not already set
            if (string.IsNullOrEmpty(movieReview.Review.ReviewID))
            {
                movieReview.Review.ReviewID = Guid.NewGuid().ToString();
            }

            // Bind the current user's ID to the review
            movieReview.Review.UserEmail = currentUser.Email;

            // Save the review to the database
            await _reviewRepository.SaveReviewAsync(movieReview.Review);

            // Redirect to the ReviewsList action for the current movie
            return RedirectToAction("ReviewsList", new { movieId = movieReview.MovieId });
           
        }



        public async Task<IActionResult> GetReviews(int id)
        {
            reviewList = (await _reviewRepository.GetReviewsByMovieIdAsync(id)).ToList();
            return View("ReviewsList", reviewList);
        }

        public async Task GetReviewsList(int movieId)
        {
            if (movieId != 0)
            {
                reviewList = (await _reviewRepository.GetReviewsByMovieIdAsync(movieId)).ToList();
                Movie movie = await _movieRepository.GetMovieAsync(movieId);
                if (movie != null && reviewList.Any())
                {
                    double sumRate = reviewList.Sum(r => r.MovieRating);
                    movie.Rating = sumRate / reviewList.Count;

                    await _movieRepository.SaveMovieAsync(movie);
                }
            }
        }
    }
}
