using StreamingServiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;
using StreamingServiceApp.DbData;

namespace StreamingServiceApp.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private static AmazonS3Client amazonS3 = new Connection().ConnectS3();
        private const string BUCKET_NAME = "mycomp306streamingserviceappbucket";
        private readonly MovieReviewService _movieReviewService;

        public MovieController(IMovieRepository movieRepository, MovieReviewService movieReviewService)
        {
            _movieRepository = movieRepository;
            _movieReviewService = movieReviewService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _movieRepository.GetMoviesAsync());
        }

        public async Task<IActionResult> Search(string mName)
        {
            var movies = await _movieRepository.GetMoviesAsync();
            var movie = movies.FirstOrDefault(m => m.MovieName == mName);
            if (movie == null)
            {
                return NotFound();
            }
            return RedirectToAction("Details", new { id = movie.MovieId });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieRepository.GetMovieAsync(id.Value);
            if (movie == null)
            {
                return NotFound();
            }
            await _movieReviewService.UpdateMovieRatingAsync(id.Value);
            return View(movie);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Movie());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                await _movieRepository.SaveMovieAsync(movie);
                TempData["Created"] = "Movie added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieRepository.GetMovieAsync(id.Value);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _movieRepository.SaveMovieAsync(movie);
                TempData["Updated"] = "Movie is updated successfully!";
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieRepository.GetMovieAsync(id.Value);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _movieRepository.DeleteMovieAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Assuming you'll handle the S3 related methods in a similar way since they're not directly related to DynamoDB
        // ... (rest of the methods related to S3)
    }
}
