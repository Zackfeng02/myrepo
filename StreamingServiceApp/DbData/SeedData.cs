using StreamingServiceApp.Models;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;


namespace StreamingServiceApp.DbData
{
    public class SeedData
    {
        private static AmazonDynamoDBClient _client;

        static SeedData()
        {
            // Create an instance of the Connection class
            var connection = new Connection();
            // Use the Connect method to get an instance of the AmazonDynamoDBClient
            _client = connection.Connect();
        }

        public static async Task InitializeAsync()
        {
            await SeedMoviesAsync();
        }

        private static async Task SeedMoviesAsync()
        {
            var movies = new List<Movie>
            {
                new Movie
                {
                    MovieId = 1,
                    MovieName = "No Time To Die",
                    ReleaseDate = DateTime.Parse("2021-9-30"),
                    Genre = Genre.ACTION,
                    Rating = 3,
                    FilePath = "AWS Certified Solutions Architect Study Guide.pdf",
                    Description = "James Bond is enjoying a tranquil life in Jamaica after leaving active service. However, his peace is short-lived as his old CIA friend, Felix Leiter, shows up and asks for help.",
                    ImageUrl = "https://m.media-amazon.com/images/I/616x9pOCRTL._AC_SY355_.jpg",
                },
                new Movie
                {
                    MovieId = 2,
                    MovieName = "Venom: Let There Be Carnage",
                    ReleaseDate = DateTime.Parse("2021-10-1"),
                    Genre = Genre.HORROR,
                    Rating = 4,
                    FilePath = "AWS Certified Solutions Architect Study Guide.pdf",
                    Description = "Eddie Brock is still struggling to coexist with the shape-shifting extraterrestrial Venom. ",
                    ImageUrl = "https://m.media-amazon.com/images/M/MV5BNTFiNzBlYmEtMTcxZS00ZTEyLWJmYmQtMjYzYjAxNGQwODAzXkEyXkFqcGdeQXVyMTEyMjM2NDc2._V1_.jpg"
                },
                new Movie
                {
                    MovieId = 3,
                    MovieName = "Greyhound",
                    ReleaseDate = DateTime.Parse("2020-7-10"),
                    Genre = Genre.ACTION,
                    Rating = 3,
                    FilePath = "AWS Certified Solutions Architect Study Guide.pdf",
                    Description = "U.S. Navy Cmdr. Ernest Krause is assigned to lead an Allied convoy across the Atlantic during World War II. His convoy, however, is pursued by German U-boats",
                    ImageUrl = "https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcRqGlJ1E_dsszf-lreRdhk3LiSe9gK1SBzNnw63UIxXiyveYR4I"
                },

                new Movie
                {
                    MovieId = 4,
                    MovieName = "Tenet",
                    ReleaseDate = DateTime.Parse("2020-8-12"),
                    Genre = Genre.THRILLER,
                    Rating = 4,
                    FilePath = "AWS Certified Solutions Architect Study Guide.pdf",
                    Description = "When a few objects that can be manipulated and used as weapons in the future fall into the wrong hands, a CIA operative, known as the Protagonist, must save the world.",
                    ImageUrl = "https://m.media-amazon.com/images/M/MV5BYzg0NGM2NjAtNmIxOC00MDJmLTg5ZmYtYzM0MTE4NWE2NzlhXkEyXkFqcGdeQXVyMTA4NjE0NjEy._V1_.jpg"
                }


            };

            foreach (var movie in movies)
            {
                var request = new PutItemRequest
                {
                    TableName = "StreamingServiceData",
                    Item = new Dictionary<string, AttributeValue>
                    {
                        { "PK", new AttributeValue($"MOVIE#{movie.MovieId}") },
                        { "SK", new AttributeValue("DETAILS") },
                        { "Type", new AttributeValue("Movie") },
                        { "MovieId", new AttributeValue { N = movie.MovieId.ToString() } },
                        { "MovieName", new AttributeValue { S = movie.MovieName } },
                        { "ReleaseDate", new AttributeValue { S = movie.ReleaseDate.ToString("o") } },
                        { "Genre", new AttributeValue { S = movie.Genre.ToString() } },
                        { "Rating", new AttributeValue { N = movie.Rating.ToString() } },
                        { "FilePath", new AttributeValue { S = movie.FilePath } },
                        { "Description", new AttributeValue { S = movie.Description } },
                        { "ImageUrl", new AttributeValue { S = movie.ImageUrl } },
                    }
                };
                await _client.PutItemAsync(request);
            }
        }
    }
}
