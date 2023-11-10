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

        [HttpGet]
        public async Task<IActionResult> EditReview(string id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            // Fetch the associated movie details using the MovieId from the review
            var movie = await _movieRepository.GetMovieAsync(review.MovieId);
            if (movie == null)
            {
                return NotFound();
            }

            // Create an instance of MovieReview and populate it
            var movieReview = new MovieReview
            {
                Review = review,
                Movie = movie
            };

            // Pass the MovieReview instance to the view
            return View(movieReview);
        }

        [HttpPost]
        public async Task<IActionResult> EditReview(MovieReview movieReview)
        {
            // Check if the ReviewID is valid
            if (string.IsNullOrWhiteSpace(movieReview.Review.ReviewID))
            {
                ModelState.AddModelError("Review.ReviewID", "Invalid Review ID.");
            }

            // Verify that the review exists and the user has permission to edit it
            Review reviewToEdit = await _reviewRepository.GetReviewByIdAsync(movieReview.Review.ReviewID);
            if (reviewToEdit == null || reviewToEdit.UserEmail != User.Identity.Name)
            {
                ModelState.AddModelError(string.Empty, "You do not have permission to edit this review or it does not exist.");
            }

            // Validate the associated movie
            var movie = await _movieRepository.GetMovieAsync(movieReview.MovieId);
            if (movie == null)
            {
                ModelState.AddModelError("Movie.MovieId", "The movie does not exist.");
            }
            // Check for required fields
            if (string.IsNullOrWhiteSpace(movieReview.Review.Title))
            {
                ModelState.AddModelError("Review.Title", "Please enter a title for the review.");
            }
            if (string.IsNullOrWhiteSpace(movieReview.Review.ReviewDescription))
            {
                ModelState.AddModelError("Review.ReviewDescription", "Please enter the review content.");
            }

            // Validate the movie rating
            if (movieReview.Review.MovieRating < 1 || movieReview.Review.MovieRating > 5)
            {
                ModelState.AddModelError("Review.MovieRating", "The rating must be between 1 and 5.");
            }

            //if (ModelState.IsValid)
            //{
                reviewToEdit.Title = movieReview.Review.Title;
                reviewToEdit.ReviewDescription = movieReview.Review.ReviewDescription;
                reviewToEdit.MovieRating = movieReview.Review.MovieRating;
                reviewToEdit.MovieId = movieReview.Movie.MovieId;
                reviewToEdit.UserEmail = User.Identity.Name;
                reviewToEdit.CreatedAt = DateTime.Now;

                await _reviewRepository.UpdateReviewAsync(reviewToEdit);
                return RedirectToAction("ReviewsList", new { movieId = movieReview.Movie.MovieId });
            //}

        }

        public async Task<IActionResult> DeleteReview(string id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null || review.UserEmail != User.Identity.Name || (DateTime.Now - review.CreatedAt).TotalHours > 48)
            {
                return NotFound();
            }
            await _reviewRepository.DeleteReviewAsync(id);
            return RedirectToAction("ReviewsList", new { movieId = review.MovieId });
        }
    }
}
