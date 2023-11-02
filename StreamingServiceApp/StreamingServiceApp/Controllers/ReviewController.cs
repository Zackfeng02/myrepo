﻿using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using StreamingServiceApp.DbData;
using StreamingServiceApp.Models;

namespace StreamingServiceApp.Controllers
{
    public class ReviewController : Controller
    {
        private static DynamoDBContext _context;
        static Connection conn = new Connection();
        private static AmazonDynamoDBClient client = conn.Connect();
        IMovieRepository repository;
        List<Review> reviewList;

        public static Review newReview;
        public ReviewController(IMovieRepository movieRepo)
        {
            repository = movieRepo;
            _context = new DynamoDBContext(client);
            reviewList = new List<Review>();
        }


        public async Task<IActionResult> ReviewsList(int movieId)
        {
            await GetReviewsList(movieId);
            return View(reviewList);
        }


        [HttpGet]
        public IActionResult AddReview(int id)
        {

            Movie movie = repository.Movies.FirstOrDefault(r => r.MovieId == id);
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
                await CreateTable("Reviews", "ReviewID", movieReview.Review);
                return RedirectToAction("ReviewsList", movieReview.MovieId);
            }
            return View(movieReview);
        }

        private async Task CreateTable(string tabNam, string hashKey, Review review)
        {

            var tableResponse = await client.ListTablesAsync();
            if (!tableResponse.TableNames.Contains(tabNam))
            {
                await Task.Run(async () =>
                {
                    await client.CreateTableAsync(new CreateTableRequest
                    {
                        TableName = tabNam,
                        ProvisionedThroughput = new ProvisionedThroughput
                        {
                            ReadCapacityUnits = 3,
                            WriteCapacityUnits = 1
                        },
                        KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = hashKey,
                            KeyType = KeyType.HASH
                        }
                    },
                        AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition { AttributeName = hashKey, AttributeType=ScalarAttributeType.S }
                    }

                    });

                    bool isTableAvailable = false;
                    while (!isTableAvailable)
                    {
                        Console.WriteLine("Waiting for table to be active...");
                        Thread.Sleep(5000);
                        var tableStatus = await client.DescribeTableAsync(tabNam);
                        isTableAvailable = tableStatus.Table.TableStatus == "ACTIVE";
                    }
                });
                await SaveReview(review);

            }
            else
            {
                await SaveReview(review);
            }
        }

        public async Task SaveReview(Review review)
        {
            Movie movie = repository.Movies.FirstOrDefault(m => m.MovieId == review.MovieId);
            string email = TempData["UserEmail"].ToString();
            newReview = new Review
            {
                ReviewDescription = review.ReviewDescription,
                ReviewID = Guid.NewGuid().ToString(),
                MovieId = review.MovieId,
                Movie = movie,
                Title = review.Title,
                UserEmail = email,
                MovieRating = review.MovieRating
            };
            await _context.SaveAsync<Review>(newReview);

        }

        public async Task<IActionResult> GetReviews(int id)
        {
            await GetReviewsList(id);
            return View("ReviewsList", reviewList);
        }
        public async Task GetReviewsList(int movieId)
        {
            if (movieId != 0)
            {
                string tabName = "Reviews";
                double calculateAvgRate = 0;
                int sumRate = 0;
                var currentTables = await client.ListTablesAsync();
                if (currentTables.TableNames.Contains(tabName))
                {
                    IEnumerable<Review> movieReviews;
                    var conditions = new List<ScanCondition> { new ScanCondition("MovieId", ScanOperator.Equal, movieId) };
                    movieReviews = await _context.ScanAsync<Review>(conditions).GetRemainingAsync();
                    Movie movie = repository.Movies.FirstOrDefault(m => m.MovieId == movieId);
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

        }
    }
}
