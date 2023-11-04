using StreamingServiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StreamingServiceApp.DbData;

namespace StreamingServiceApp.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMovieRepository _movieRepository;
        private List<Review> reviewList;

        public ReviewController(IReviewRepository reviewRepo, IMovieRepository movieRepo)
        {
            _reviewRepository = reviewRepo;
            _movieRepository = movieRepo;
            reviewList = new List<Review>();
        }

        public async Task<IActionResult> ReviewsList(int movieId)
        {
            reviewList = (await _reviewRepository.GetReviewsByMovieIdAsync(movieId)).ToList();
            return View(reviewList);
        }

        [HttpGet]
        public IActionResult AddReview(int id)
        {
            Movie movie = _movieRepository.GetMovieAsync(id).Result;
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
            if (ModelState.IsValid)
            {
                movieReview.Review.MovieId = movieReview.MovieId;
                TempData["ReviewMovieId"] = movieReview.MovieId;
                await _reviewRepository.SaveReviewAsync(movieReview.Review);
                return RedirectToAction("ReviewsList", new { movieId = movieReview.MovieId });
            }
            return View(movieReview);
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
