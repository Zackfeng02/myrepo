﻿using StreamingServiceApp.Models;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StreamingServiceApp.DbData
{
    public class DynamoDBMovieRepository : IMovieRepository
    {
        private readonly DynamoDBHelper _dynamoDbHelper;
        private const string TableName = "StreamingServiceData";

        public DynamoDBMovieRepository()
        {
            _dynamoDbHelper = new DynamoDBHelper();
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            var filterExpression = "begins_with(PK, :pk) AND SK = :sk";
            var expressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", new AttributeValue { S = "MOVIE#" } },
                { ":sk", new AttributeValue { S = "DETAILS" } }
            };

            var items = await _dynamoDbHelper.ScanTable(TableName, filterExpression, expressionAttributeValues);
            return items.Select(DynamoDBItemToMovie);
        }

        public async Task<Movie> GetMovieAsync(int id)
        {
            var key = new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue { S = $"MOVIE#{id}" } },
                { "SK", new AttributeValue { S = "DETAILS" } }
            };
            var item = await _dynamoDbHelper.GetItem(TableName, key);
            return DynamoDBItemToMovie(item);
        }

        public async Task SaveMovieAsync(Movie movie)
        {
            var item = MovieToDynamoDBItem(movie);
            await _dynamoDbHelper.PutItem(TableName, item);
        }

        public async Task<Movie> DeleteMovieAsync(int movieID)
        {
            var key = new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue { S = $"MOVIE#{movieID}" } },
                { "SK", new AttributeValue { S = "DETAILS" } }
            };

            // get the item you want to delete
            var movieToDelete = await GetMovieAsync(movieID);

            // delete the item from DynamoDB
            await _dynamoDbHelper.DeleteItem(TableName, key);

            return movieToDelete;
        }

        private Dictionary<string, AttributeValue> MovieToDynamoDBItem(Movie movie)
        {
            return new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue { S = $"MOVIE#{movie.MovieId}" } },
                { "SK", new AttributeValue { S = "DETAILS" } },
                { "MovieId", new AttributeValue { N = movie.MovieId.ToString() } },
                { "MovieName", new AttributeValue { S = movie.MovieName } },
                { "Genre", new AttributeValue { S = movie.Genre.ToString() } },
                { "Description", new AttributeValue { S = movie.Description } },
                { "ReleaseDate", new AttributeValue { S = movie.ReleaseDate.ToString("o") } }, // Using ISO 8601 format
                { "Rating", new AttributeValue { N = movie.Rating.ToString() } },
                { "FilePath", new AttributeValue { S = movie.FilePath } },
                { "ImageUrl", new AttributeValue { S = movie.ImageUrl } },
                { "MovieUserId", new AttributeValue { N = movie.MovieUserId.ToString() } },
                { "Type", new AttributeValue { S = "Movie"}}
            };
        }

        private Movie DynamoDBItemToMovie(Dictionary<string, AttributeValue> item)
        {
            return new Movie
            {
                MovieId = int.Parse(item["MovieId"].N),
                MovieName = item["MovieName"].S,
                Genre = (Genre)Enum.Parse(typeof(Genre), item["Genre"].S),
                Description = item["Description"].S,
                ReleaseDate = DateTime.Parse(item["ReleaseDate"].S),
                Rating = double.Parse(item["Rating"].N),
                FilePath = item["FilePath"].S,
                ImageUrl = item["ImageUrl"].S,
                MovieUserId = int.Parse(item["MovieUserId"].N)
            };
        }

        public async Task<IEnumerable<Movie>> GetMoviesByGenreAsync(Genre genre)
        {
            var filterExpression = "begins_with(PK, :pk) AND SK = :sk AND Genre = :genre";
            var expressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", new AttributeValue { S = "MOVIE#" } },
                { ":sk", new AttributeValue { S = "DETAILS" } },
                { ":genre", new AttributeValue { S = genre.ToString() } }
            };

            var items = await _dynamoDbHelper.ScanTable(TableName, filterExpression, expressionAttributeValues);
            return items.Select(DynamoDBItemToMovie);
        }


        public async Task UpdateMovieAsync(Movie movie)
        {
            // Assuming that the movie.MovieId is the primary key for the movie item
            var key = new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue { S = $"MOVIE#{movie.MovieId}" } },
                { "SK", new AttributeValue { S = "DETAILS" } }
            };

            var updatedAttributes = new Dictionary<string, AttributeValueUpdate>();

            // Convert Movie object to AttributeValueUpdate for each property
            updatedAttributes["MovieName"] = new AttributeValueUpdate(new AttributeValue { S = movie.MovieName }, "PUT");
            updatedAttributes["Genre"] = new AttributeValueUpdate(new AttributeValue { S = movie.Genre.ToString() }, "PUT");
            updatedAttributes["Description"] = new AttributeValueUpdate(new AttributeValue { S = movie.Description }, "PUT");
            updatedAttributes["ReleaseDate"] = new AttributeValueUpdate(new AttributeValue { S = movie.ReleaseDate.ToString("o") }, "PUT");
            updatedAttributes["Rating"] = new AttributeValueUpdate(new AttributeValue { N = movie.Rating.ToString() }, "PUT");
            updatedAttributes["FilePath"] = new AttributeValueUpdate(new AttributeValue { S = movie.FilePath }, "PUT");
            updatedAttributes["ImageUrl"] = new AttributeValueUpdate(new AttributeValue { S = movie.ImageUrl }, "PUT");

            await _dynamoDbHelper.UpdateItem(TableName, key, updatedAttributes);
        }

        public async Task<IEnumerable<Movie>> GetMoviesByRatingAsync(double rating)
        {
            var keyConditionExpression = "Rating = :rating";
            var expressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":rating", new AttributeValue { N = rating.ToString() } }
            };

            // Query the GSI for movies with the specified rating
            var items = await _dynamoDbHelper.QueryIndex(
                TableName,
                "MovieRatingIndex", // actual GSI name for movie ratings
                keyConditionExpression,
                expressionAttributeValues);

            return items.Select(DynamoDBItemToMovie);
        }

        public async Task<IEnumerable<Movie>> GetMoviesByGenreAndRatingAsync(Genre genre, double rating)
        {
            var filterExpression = "begins_with(PK, :pk) AND SK = :sk AND Genre = :genre AND Rating = :rating";
            var expressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", new AttributeValue { S = "MOVIE#" } },
                { ":sk", new AttributeValue { S = "DETAILS" } },
                { ":genre", new AttributeValue { S = genre.ToString() } },
                { ":rating", new AttributeValue { N = rating.ToString() } }
            };

            var items = await _dynamoDbHelper.ScanTable(TableName, filterExpression, expressionAttributeValues);
            return items.Select(DynamoDBItemToMovie).ToList();
        }


    }
}
