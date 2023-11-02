using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using StreamingServiceApp.DbData;
using StreamingServiceApp.Models;
using Amazon.S3;
using Amazon.S3.Model;

namespace StreamingServiceApp.Controllers
{
    public class MovieController : Controller
    {

        static Connection conn = new Connection();
        private static AmazonDynamoDBClient client = conn.Connect();
        static Connection conn1 = new Connection();
        private static AmazonS3Client amazonS3 = conn1.ConnectS3();
        private DynamoDBContext _dbContext;
        private readonly MovieAppDbContext _context;
        List<Review> reviewList;
        string BUCKET_NAME = "new-pradyumna";
        Movie updateMovie;

        public MovieController(MovieAppDbContext context)
        {
            _context = context;
            _dbContext = new DynamoDBContext(client);
            reviewList = new List<Review>();

        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        public async Task<IActionResult> Search(string mName)
        {
            var movie = await _context.Movies
              .FirstOrDefaultAsync(m => m.MovieName == mName);
            if (movie == null)
            {
                return NotFound();
            }
            return Redirect("Details");
        }

        public async Task<IActionResult> Details(int? id)
        {
            int movieId = 0;
            if (id == null)
            {
                return NotFound();
            }
            if (TempData["RevewMovieId"] != null)
            {
                movieId = (int)TempData["ReviewMovieId"];
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id || m.MovieId == movieId);

            if (movie == null)
            {
                return NotFound();
            }
            await GetReviewsList(movie);
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
                int id = (int)TempData["UserId"];
                User user = _context.Users.FirstOrDefault(u => u.UserId == id);
                movie.User = user;
                _context.Add(movie);
                await _context.SaveChangesAsync();
                TempData["Created"] = "Movie added successfull!";
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

            var movie = await _context.Movies.FindAsync(id);
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
                try
                {
                    updateMovie = new Movie
                    {
                        MovieId = movie.MovieId,
                        MovieName = movie.MovieName,
                        Description = movie.Description,
                        FilePath = movie.FilePath,
                        Genre = movie.Genre,
                        ImageUrl = movie.ImageUrl,
                        Rating = movie.Rating,
                        ReleaseDate = movie.ReleaseDate,
                        User = movie.User
                    };
                    _context.Update(updateMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
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

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
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
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }

        public async Task GetReviewsList(Movie movie)
        {
            string tabName = "Reviews";
            double calculateAvgRate = 0;
            int sumRate = 0;
            var currentTables = await client.ListTablesAsync();
            if (currentTables.TableNames.Contains(tabName))
            {
                IEnumerable<Review> movieReviews;
                var conditions = new List<ScanCondition> { new ScanCondition("MovieId", ScanOperator.Equal, movie.MovieId) };
                movieReviews = await _dbContext.ScanAsync<Review>(conditions).GetRemainingAsync();

                Console.WriteLine("List retrieved " + movieReviews);
                foreach (var result in movieReviews)
                {
                    if (result != null)
                    {
                        Review review = new Review()
                        {
                            ReviewDescription = result.ReviewDescription,
                            MovieRating = result.MovieRating,
                            Title = result.Title,
                            MovieId = result.MovieId,
                            ReviewID = result.ReviewID,
                            UserEmail = result.UserEmail,
                        };
                        reviewList.Add(review);
                        sumRate += result.MovieRating;
                        calculateAvgRate++;
                    }

                }
                movie.Rating = sumRate / calculateAvgRate;
            }
        }


        [HttpPost]
        public async Task<IActionResult> DownloadMovie(int id)
        {
            Movie movie = await _context.Movies.FindAsync(id);
            if (movie.FilePath != null)
            {
                try
                {
                    using (amazonS3)
                    {
                        string keyName = movie.FilePath;
                        GetPreSignedUrlRequest request =
                                  new GetPreSignedUrlRequest()
                                  {
                                      BucketName = BUCKET_NAME,
                                      Key = keyName,
                                      Expires = DateTime.Now.AddMinutes(15)
                                  };

                        string url = amazonS3.GetPreSignedURL(request);
                        return Redirect(url);
                    }
                }
                catch (Exception)
                {
                    string Failure = "File download failed. Please try after some time.";
                    return View(Failure);
                }
            }
            else
            {
                TempData["NoFile"] = "No file exist to download";
                return RedirectToAction("Index");
            }

        }

        public async Task ListingObjectsAsync(string filePath)
        {
            try
            {
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = BUCKET_NAME,
                    MaxKeys = 10
                };
                ListObjectsV2Response response;
                do
                {
                    response = await amazonS3.ListObjectsV2Async(request);

                    foreach (S3Object entry in response.S3Objects)
                    {
                        if (entry.Key == filePath)
                        {

                        }
                        Console.WriteLine("key = {0} size = {1}",
                            entry.Key, entry.Size);
                    }
                    Console.WriteLine("Next Continuation Token: {0}", response.NextContinuationToken);
                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                Console.WriteLine("S3 error occurred. Exception: " + amazonS3Exception.ToString());
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
                Console.ReadKey();
            }
        }


    }

}
