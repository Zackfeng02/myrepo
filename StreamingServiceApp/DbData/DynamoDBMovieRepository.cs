using StreamingServiceApp.Models;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            };
        }
    }
}
