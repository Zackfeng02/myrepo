using StreamingServiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;
using StreamingServiceApp.DbData;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace StreamingServiceApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private static AmazonS3Client amazonS3 = new Connection().ConnectS3();
        private const string BUCKET_NAME = "mycomp306streamingserviceappbucket";
        private readonly MovieReviewService _movieReviewService;
        private readonly IUserService _userService;


        public MoviesController(IMovieRepository movieRepository, MovieReviewService movieReviewService, IUserService userService)
        {
            _movieRepository = movieRepository;
            _movieReviewService = movieReviewService;
            _userService = userService;
        }

        public async Task<IActionResult> Index(Genre? genre, double? rating)
        {
            IEnumerable<Movie> movies;

            if (genre.HasValue && rating.HasValue)
            {
                movies = await _movieRepository.GetMoviesByGenreAndRatingAsync(genre.Value, rating.Value);
            }
            else if (genre.HasValue)
            {
                movies = await _movieRepository.GetMoviesByGenreAsync(genre.Value);
            }
            else if (rating.HasValue)
            {
                movies = await _movieRepository.GetMoviesByRatingAsync(rating.Value);
            }
            else
            {
                movies = await _movieRepository.GetMoviesAsync();
            }

            return View(movies);
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
            if (movie.MovieId == 0)
            {
                int nextMovieId = await GetNextMovieIdAsync(); // Implement this method to retrieve the next movie ID
                movie.MovieId = nextMovieId;
            }

            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                ModelState.AddModelError("", "User must be logged in to add a movie.");
                return View(movie);
            }
            // Assuming User is set based on the current logged-in user context
            // This is just an example, replace with your actual logic to retrieve the current user
            movie.MovieUserId = currentUser.UserId;

            if (movie.UploadedFile != null && movie.UploadedFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(movie.UploadedFile.FileName);
                var key = $"movies/{fileName}";

                using (var stream = new MemoryStream())
                {
                    await movie.UploadedFile.CopyToAsync(stream);
                    var putRequest = new PutObjectRequest
                    {
                        BucketName = BUCKET_NAME,
                        Key = key,
                        InputStream = stream,
                        ContentType = movie.UploadedFile.ContentType
                    };

                    var response = await amazonS3.PutObjectAsync(putRequest);
                    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        movie.FilePath = key; // Set the file path only if upload succeeds
                    }
                    else
                    {
                        ModelState.AddModelError("", "File upload failed.");
                        return View(movie);
                    }
                }
            }
            else
            {
                // If file upload is required, add a model error
                ModelState.AddModelError("UploadedFile", "Uploading a movie file is required.");
            }

            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors);
            //    foreach (var error in errors)
            //    {
            //        // Log the error description
            //        // For example, you could use Console.WriteLine for debugging purposes
            //        Console.WriteLine(error.ErrorMessage);
            //    }
            //    return View(movie);
            //}


            // Save the movie if all validations pass
            await _movieRepository.SaveMovieAsync(movie);
            TempData["Created"] = "Movie added successfully!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<int> GetNextMovieIdAsync()
        {
            // Retrieve all movie IDs, find the maximum, and add 1
            // This is a simplified example and should be optimized for production use
            var allMovies = await _movieRepository.GetMoviesAsync();
            int maxId = allMovies.Max(m => m.MovieId);
            return maxId >= 4 ? maxId + 1 : 4;
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

            // Retrieve the current user
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null )
            {
                // If the current user is not the owner of the movie, do not allow editing
                return Forbid(); // or return a view with an error message
            }

            // Check if the current user is the owner of the movie
            var existingMovie = await _movieRepository.GetMovieAsync(id);
            if (existingMovie == null || existingMovie.MovieUserId != currentUser.UserId)
            {
                // If the movie doesn't exist or the current user is not the owner, do not allow editing
                return Forbid(); // or return a view with an error message
            }


            if (ModelState.IsValid)
            {
                try
                {
                    // Update the movie with the new details
                    await _movieRepository.UpdateMovieAsync(movie);
                    TempData["Updated"] = "Movie updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex) // Catch a more specific exception if possible
                {
                    ModelState.AddModelError("", "An error occurred while updating the movie.");
                    // Log the exception
                    // For example, you could use _logger.LogError(ex, "An error occurred while updating the movie with id {MovieId}", movie.MovieId);
                }
            }
            return View(movie);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var currentUser = await _userService.GetCurrentUserAsync();
            var movie = await _movieRepository.GetMovieAsync(id.Value);
            if (movie == null)
            {
                return NotFound();
            }
            if (movie.MovieUserId != currentUser.UserId)
            {
                return NotFound();
            }
            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _movieRepository.GetMovieAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            // Retrieve the current user
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null || movie.MovieUserId != currentUser.UserId)
            {
                // If the current user is not the owner of the movie, do not allow deletion
                return Forbid(); // or return a view with an error message
            }

            await _movieRepository.DeleteMovieAsync(id);
            TempData["Deleted"] = "Movie deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DownloadMovie(int id)
        {
            var movie = await _movieRepository.GetMovieAsync(id);
            var currentUser = await _userService.GetCurrentUserAsync();
            if (movie == null || string.IsNullOrEmpty(movie.FilePath))
            {
                return NotFound();
            }

            if (movie.MovieUserId == currentUser.UserId)
            {
                var request = new GetObjectRequest
                {
                    BucketName = BUCKET_NAME,
                    Key = movie.FilePath
                };

                try
                {
                    using (var response = await amazonS3.GetObjectAsync(request))
                    using (var responseStream = response.ResponseStream)
                    using (var memoryStream = new MemoryStream())
                    {
                        await responseStream.CopyToAsync(memoryStream);
                        var contentType = response.Headers["Content-Type"];
                        var fileDownloadName = Path.GetFileName(movie.FilePath);
                        return File(memoryStream.ToArray(), contentType, fileDownloadName);
                    }
                }
                catch (AmazonS3Exception e)
                {
                    return NotFound();
                }
            }
            else
            {
                return Forbid();
            }

            
        }

        public async Task<IActionResult> FilterByRating(double minRating)
        {
            var movies = await _movieRepository.GetMoviesByRatingAsync(minRating);
            return View("Index", movies); // "Index" is the view you want to return to
        }
        public async Task<IActionResult> FilterByGenre(Genre genre)
        {
            var movies = await _movieRepository.GetMoviesByGenreAsync(genre);
            return View("Index", movies); // Assuming "Index" is the view displaying the list of movies
        }
    }
}
