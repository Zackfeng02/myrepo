 using StreamingServiceApp.Models;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreamingServiceApp.DbData
{
    public class DynamoDBUserRepository : IUserRepository
    {
        private readonly DynamoDBHelper _dynamoDbHelper;
        private const string TableName = "StreamingServiceData"; // Updated table name

        public DynamoDBUserRepository()
        {
            _dynamoDbHelper = new DynamoDBHelper();
        }

        public IQueryable<User> Users => GetUsersAsync().Result.AsQueryable();

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var items = await _dynamoDbHelper.ScanTable(TableName, "Type = :type", new Dictionary<string, AttributeValue> { { ":type", new AttributeValue("User") } });
            return items.Select(DynamoDBItemToUser);
        }

        public User GetUserMovies(string email)
        {
            return GetUserMoviesAsync(email).Result;
        }

        public async Task<User> GetUserMoviesAsync(string email)
        {
            var key = new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue($"USER#{email}") },
                { "SK", new AttributeValue("DETAILS") }
            };
            var item = await _dynamoDbHelper.GetItem(TableName, key);
            return DynamoDBItemToUser(item);
        }

        public void SaveUser(User user)
        {
            SaveUserAsync(user).Wait();
        }

        public async Task SaveUserAsync(User user)
        {
            var item = UserToDynamoDBItem(user);
            await _dynamoDbHelper.PutItem(TableName, item);
        }

        private User DynamoDBItemToUser(Dictionary<string, AttributeValue> item)
        {
            return new User
            {
                UserId = int.Parse(item["UserId"].N),
                Email = item["Email"].S,
                Password = item["Password"].S,
            };
        }

        private Dictionary<string, AttributeValue> UserToDynamoDBItem(User user)
        {
            return new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue($"USER#{user.Email}") },
                { "SK", new AttributeValue("DETAILS") },
                { "Type", new AttributeValue("User") },
                { "UserId", new AttributeValue { N = user.UserId.ToString() } },
                { "Email", new AttributeValue { S = user.Email } },
                { "Password", new AttributeValue { S = user.Password } },
            };
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var key = new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue($"USER#{email}") },
                { "SK", new AttributeValue("DETAILS") }
            };
            var item = await _dynamoDbHelper.GetItem(TableName, key);
            return DynamoDBItemToUser(item);
        }
    }
}
